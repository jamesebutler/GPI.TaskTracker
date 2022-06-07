Option Explicit On
Option Strict On
Imports System.Globalization
Imports BootstrapHelper

Partial Class TaskDetailsGPI
    Inherits IP.Bids.BasePage



#Region "Fields"
    Private TaskHeaderNumber As String = String.Empty
    Private TaskItemNumber As String = "-1"
    Private SubTaskItemNumber As String = String.Empty
    Private OpenTaskSeqId As Integer ' = 0
    Private IsTaskTemplate As Boolean
    'Private RefSite As String = String.Empty
    Private HasSubTasks As Boolean ' Used to determine if the current task depends on sub tasks to be closed before it can be closed
    Private HasOpenSubtasks As Boolean
    Private TankType As TankTaskTypes = TankTaskTypes.Repair




#End Region


    Private Enum TankTaskTypes
        Inspection
        Repair
    End Enum

#Region "Methods"
    ''' <summary>
    ''' Displays a popup message to the user.
    ''' </summary>
    ''' <param name="msg">The MSG.</param>
    ''' <param name="okScript">The ok script.</param>
    Public Sub DisplayMessage(ByVal msg As String, Optional ByVal okScript As String = "")
        Master.DisplayMessageToUser(msg, IP.Bids.SharedFunctions.LocalizeValue("Task Details", True))
    End Sub

    ''' <summary>
    ''' Handles the page load.
    ''' </summary>
    Public Sub HandlePageLoad()
        Dim taskItemBLL As New TaskTrackerItemBll
        Dim currentTaskItem As TaskItem
        Dim priorities As System.Collections.Generic.List(Of TaskPriorities)
        Dim taskstatus As New TaskTrackerListsBll
        Dim taskHeader As TaskHeaderBll
        Dim status As System.Collections.Generic.List(Of TaskStatus)


        '_lblDataBusiness.Text = "JAMES BUTLER"


        Try
            IP.Bids.SharedFunctions.DisablePageCache(Response)


            'Determine if a message should be displayed to the user
            If Session("TaskDetailMessage") IsNot Nothing Then
                DisplayMessage(CStr(Session("TaskDetailMessage")))
                Session.Remove("TaskDetailMessage")
            End If

            If Request.QueryString("TankInspectionType") IsNot Nothing Then
                If String.Equals(Request.QueryString("TankInspectionType"), "INSPECTION", StringComparison.OrdinalIgnoreCase) Then
                    TankType = TankTaskTypes.Inspection
                Else
                    TankType = TankTaskTypes.Repair
                End If
            End If

            Dim jsValidateForSave As String = "var canPostBack=CheckForm('TaskDetailsGPI'); if (canPostBack) DisplayBusy(); return canPostBack;"
            _btnTaskHeader.OnClientClick = "DisplayBusy();"
            _btnReturnToTaskList.OnClientClick = "DisplayBusy();"
            _btnSaveTask.OnClientClick = jsValidateForSave
            _btnAddTask.OnClientClick = jsValidateForSave
            _btnSaveandShowSubTasks.OnClientClick = jsValidateForSave
            _btnSaveandShowRecurrence.OnClientClick = jsValidateForSave
            _btnSaveandShowAttachments.OnClientClick = jsValidateForSave
            _ClosedDate.MaxDate = Now

            'Get Header Number
            If Request.QueryString("HeaderNumber") IsNot Nothing AndAlso IsNumeric(Request.QueryString("HeaderNumber")) Then
                'Store Header Number
                TaskHeaderNumber = Request.QueryString("HeaderNumber")

                'Configure Attachments
                Me._fa.TaskHeaderNumber = TaskHeaderNumber
                'Me._fa.SaveAsPath = Server.MapPath("~/Uploads")
                Me._fa.DataBind()

                'Display Task Items for the current Header
                'jeb _ifrTaskItems.Attributes.Item("src") = Page.ResolveUrl("~/Popups/TaskList.aspx?HeaderNumber=" & TaskHeaderNumber & "&ShowHeaderInfo=True")
                'jeb LoadIframe()

                'Populate the Task Header Object
                taskHeader = New TaskHeaderBll(CInt(IP.Bids.SharedFunctions.DataClean(TaskHeaderNumber, CStr(0))))
                If taskHeader Is Nothing Then 'OrElse taskHeader.CurrentTaskHeaderRecord Is Nothing Then
                    'An invalid Header Number was specified.  The user should be redirected to the View screen
                    IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetViewTasksURL("", RefSite, "", ""))
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    Exit Sub
                End If
            Else
                'An invalid Header Number was specified.  The user should be redirected to the View screen
                IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetViewTasksURL("", RefSite, "", ""))
                'HttpContext.Current.ApplicationInstance.CompleteRequest()
                Exit Sub
            End If

            'Get Task Item Number            
            If Request.QueryString("TaskNumber") IsNot Nothing AndAlso IsNumeric(Request.QueryString("TaskNumber")) Then
                'Store Task Item Number
                TaskItemNumber = Request.QueryString("TaskNumber")
                _btnCopyTask.Enabled = True
                _btnCopyTask.Url = Page.ResolveUrl(IP.Bids.SiteURLs.GetCopyTaskUrl(TaskItemNumber))
                _btnHistory.Enabled = True
                _btnHistory.Url = Page.ResolveUrl(IP.Bids.SiteURLs.GetTaskHistoryUrl(TaskItemNumber))
            Else
                'Invalid Tasknumber was specified
                IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetViewTasksURL("", RefSite, "", ""))
                'HttpContext.Current.ApplicationInstance.CompleteRequest()
                Exit Sub
            End If

            If Request.QueryString("SubTaskNumber") IsNot Nothing Then
                SubTaskItemNumber = Request.QueryString("SubTaskNumber")
                Session.Item("SubTaskNumber") = SubTaskItemNumber
            ElseIf Session.Item("SubTaskNumber") IsNot Nothing Then
                SubTaskItemNumber = CStr(Session.Item("SubTaskNumber"))
            Else
                SubTaskItemNumber = "-1"
            End If

            If Request.QueryString("popupwindow") IsNot Nothing Then
                Exit Sub
            End If

            If Not Page.IsPostBack Then
                'The page is being loaded and accessed for the first time.  

                'Populate the Priority List
                Try
                    With Me._ddlPriority
                        .Items.Clear()
                        priorities = taskItemBLL.GetPriorityList
                        If priorities IsNot Nothing AndAlso priorities.Count > 0 Then
                            For Each item As TaskPriorities In priorities
                                .Items.Add(New ListItem(item.PriorityName, CStr(item.PriorityID)))
                            Next
                            .DataBind()
                        End If
                    End With
                Catch ex As Exception
                    IP.Bids.SharedFunctions.HandleError("HandlePageLoad", "Error populating Priority List", ex)
                End Try

                'Populate the Status List
                Try
                    With Me._rblStatus
                        .Items.Clear()
                        .RepeatColumns = 6
                        .Width = Unit.Percentage(90)
                        .RepeatDirection = RepeatDirection.Horizontal
                        status = taskstatus.GetTaskStatus
                        For Each item As TaskStatus In status
                            If item.StatusName.ToLower = "open" Then
                                OpenTaskSeqId = item.StatusSeqid
                            End If
                            If item.StatusSeqid = -1 Then
                                Continue For
                            End If
                            .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.StatusName, True), CStr(item.StatusSeqid), True))
                        Next
                        .DataBind()
                        .ClearSelection()
                        .SelectedValue = CStr(OpenTaskSeqId)
                    End With
                Catch ex As Exception
                    IP.Bids.SharedFunctions.HandleError("HandlePageLoad", "Error populating Status List", ex)
                End Try

                'Populate the Employee List
                If taskHeader IsNot Nothing AndAlso taskHeader.CurrentTaskHeaderRecord IsNot Nothing Then
                    'Default the responsible person dropdown to the header facility
                    Me._ResponsiblePerson.PlantCode = taskHeader.CurrentTaskHeaderRecord.PlantCode
                    Me._ResponsiblePerson.PopulateEmployeeList()
                End If
            End If

            'Handle Tasks from external sites           
            If Request.QueryString("RefSite") IsNot Nothing Then
                RefSite = Request.QueryString("RefSite").ToUpper
                'ElseIf Session.Item("RefSite") IsNot Nothing Then
                'RefSite = CStr(Session.Item("RefSite"))
            End If
            'If Request.QueryString("RefSite") IsNot Nothing Then
            'RefSite = Request.QueryString("RefSite").ToLower

            If RefSite.ToLower = "moc" Or RefSite.ToLower = "ri" Or RefSite.ToLower = "outage" Or RefSite.ToLower = "iris" Or RefSite.ToLower = "tanks" Then
                Master.HideFooter()
                Master.HideHeaderAndMenu() 'MJP 3/29/12
                Me._btnTaskHeader.Visible = False
                _btnReturnToTaskList.Visible = True
                _btnReturnToTaskSearch.Visible = False
                'jeb Me._ifrTaskItems.Attributes.Item("src") = ""
                LoadIframe()

                If RefSite.ToLower = "tanks" Then
                    If TankType = TankTaskTypes.Inspection Then DisplayInspectionTypes()
                End If
            Else
                Me._btnTaskHeader.Visible = True
                _btnReturnToTaskSearch.Visible = True
                _btnReturnToTaskList.Visible = False
                'Display Task Items for the current Header
                'jeb _ifrTaskItems.Attributes.Item("src") = Page.ResolveUrl(IP.Bids.SiteURLs.GetTaskListURL(TaskHeaderNumber, RefSite, True, True, True))
                'jeb LoadIframe()

            End If
            'End If

            'Handle internal tasks that were created in an external system
            If taskHeader.CurrentTaskHeaderRecord.ExternalSourceName.ToLower = "moc" Or taskHeader.CurrentTaskHeaderRecord.ExternalSourceName.ToLower = "reliability incident" Or taskHeader.CurrentTaskHeaderRecord.ExternalSourceName.ToLower = "outage" Then 'Or taskHeader.CurrentTaskHeaderRecord.ExternalSourceName.ToLower = "iris" Then
                Me._btnTaskHeader.Visible = False
                PopulateExternalSourceButton(CInt(taskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID), taskHeader.CurrentTaskHeaderRecord.ExternalRef)
            End If

            If taskHeader.CurrentTaskHeaderRecord.ExternalSourceName.ToLower = "tanks" Then 'Or taskHeader.CurrentTaskHeaderRecord.ExternalSourceName.ToLower = "iris" Then
                If RefSite.ToLower <> "tanks" Then
                    Me._btnTaskHeader.Visible = False
                    PopulateExternalSourceButton(CInt(taskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID), taskHeader.CurrentTaskHeaderRecord.ExternalRef)
                End If
            End If


            'Check for Template Task
            If IsNumeric(taskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID) AndAlso CDbl(taskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID) = IP.Bids.SharedFunctions.SourceSystemID.Template Then 'Template Header
                Me.IsTaskTemplate = True
            End If

            'If the task item number is 0 then we need to display the Root Task Item
            If CInt(TaskItemNumber) = 0 Then
                'Select the first task
                Dim taskList As System.Collections.Generic.List(Of TaskItem) = Nothing
                taskList = taskItemBLL.GetRootTaskItemList(CInt(TaskHeaderNumber))
                If taskList IsNot Nothing AndAlso taskList.Count > 0 Then
                    TaskItemNumber = CStr(taskList.Item(0).TaskItemSeqId)
                Else
                    'Assume that we are creating a new task
                    TaskItemNumber = CStr(-1)
                End If
                IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveClientUrl(IP.Bids.SiteURLs.GetTaskDetailsURL(TaskHeaderNumber, RefSite, TaskItemNumber)))
                'Response.Redirect(Page.ResolveClientUrl(String.Format(CultureInfo.CurrentCulture, "~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}&RefSite={2}", TaskHeaderNumber, TaskItemNumber, RefSite)), False)
                'HttpContext.Current.ApplicationInstance.CompleteRequest()
                Exit Sub
            ElseIf CInt(TaskItemNumber) > 0 Then
                'Populate the current Task Item Object
                currentTaskItem = taskItemBLL.GetTaskItem(CInt(TaskItemNumber))
                If currentTaskItem Is Nothing OrElse currentTaskItem.TaskItemSeqId = 0 Then
                    If Session.Item("RootTaskFor_" & TaskItemNumber) IsNot Nothing Then
                        'Lets redirect the user back to the root task because the task item that was last used has been deleted
                        'Response.Redirect(Page.ResolveClientUrl(String.Format(CultureInfo.CurrentCulture, "~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}&RefSite={2}", TaskHeaderNumber, Session.Item("RootTaskFor_" & TaskItemNumber), RefSite)), False)
                        IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveClientUrl(IP.Bids.SiteURLs.GetTaskDetailsURL(TaskHeaderNumber, RefSite, CStr(Session.Item("RootTaskFor_" & TaskItemNumber)))))
                        'HttpContext.Current.ApplicationInstance.CompleteRequest()
                        Session.Remove("RootTaskFor_" & TaskItemNumber)
                    Else
                        Session("TaskDetailMessage") = String.Format(IP.Bids.SharedFunctions.LocalizeValue("An invalid item number [{0}] was specified.", True), TaskItemNumber)
                        TaskItemNumber = CStr(-1)
                        'Response.Redirect(Page.ResolveClientUrl(String.Format(CultureInfo.CurrentCulture, "~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}&RefSite={2}", TaskHeaderNumber, TaskItemNumber, RefSite)), False)
                        IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveClientUrl(IP.Bids.SiteURLs.GetTaskDetailsURL(TaskHeaderNumber, RefSite, TaskItemNumber)))
                        'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    End If
                    'currenttaskitem = taskitembll.GetRootTaskItemList
                End If

                If currentTaskItem IsNot Nothing AndAlso currentTaskItem.TaskItemSeqId > 0 Then
                    _btnDelete.Enabled = False
                    If currentTaskItem.TaskItemSeqId <> -1 Then
                        Dim myRoles As System.Collections.Generic.List(Of UserRoles) = GeneralTaskTrackerBll.GetUserRoles(IP.Bids.SharedFunctions.GetCurrentUser.Username)
                        For Each role As UserRoles In myRoles
                            If role.RoleName.ToUpper = "FACILITYADMIN" AndAlso role.PlantCode = taskHeader.CurrentTaskHeaderRecord.PlantCode Then
                                _btnDelete.Enabled = True
                                Exit For
                            ElseIf role.RoleName.ToUpper = "SUPPORT" Then
                                _btnDelete.Enabled = True
                                Exit For
                            End If
                        Next
                        If currentTaskItem.CreatedByUserName.ToUpper = IP.Bids.SharedFunctions.GetCurrentUser.Username.ToUpper Then
                            _btnDelete.Enabled = True
                        End If
                    End If
                    'TODO - Check to see if this is a dependenttask.  If it is then redirect using the parent
                    If currentTaskItem.Dependenttaskseqid.Length > 0 AndAlso CDbl(currentTaskItem.Dependenttaskseqid) <> currentTaskItem.TaskItemSeqId Then
                        IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & currentTaskItem.Dependenttaskseqid & "&RefSite=" & RefSite)
                        'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    End If
                    'Updating an existing Task Item
                    If Not Page.IsPostBack Then
                        If currentTaskItem.ResponsibleRolePlantCode.Length > 0 Then
                            Me._ResponsiblePerson.PlantCode = currentTaskItem.ResponsibleRolePlantCode
                            Me._ResponsiblePerson.DefaultUserName = currentTaskItem.ResponsibleRoleSeqId.ToString
                            Me._ResponsiblePerson.PopulateEmployeeList()
                        Else
                            Me._ResponsiblePerson.DefaultUserName = currentTaskItem.ResponsibleUserName
                            Me._ResponsiblePerson.PopulateEmployeeList()
                        End If
                    End If

                    'Populate the Attachments list
                    'Me._fa.TaskItemNumber = TaskItemNumber
                    'Populate the Attachments list
                    If currentTaskItem.RootTaskItemSeqId IsNot Nothing AndAlso IsNumeric(currentTaskItem.RootTaskItemSeqId) Then
                        Me._fa.TaskHeaderNumber = CStr(currentTaskItem.TaskHeaderSeqId)
                        Me._fa.TaskItemNumber = currentTaskItem.RootTaskItemSeqId
                    Else
                        Me._fa.TaskHeaderNumber = CStr(currentTaskItem.TaskHeaderSeqId)
                        Me._fa.TaskItemNumber = TaskItemNumber
                    End If
                    Me._fa.DataBind()
                    'MJP Do I need theese?
                    '_btnSubTasks.Visible = False
                    '_btnAttachments.Visible = False
                    '_btnRecurrence.Visible = False



                    If Not Page.IsPostBack Then
                        'populate the fields from database data
                        With currentTaskItem
                            'If .ClosedDate.Length > 0 Then Me._ClosedDate.Enabled = False
                            If .ClosedDate.Length > 0 Then
                                Me._ClosedDate.StartDate = CDate(.ClosedDate).ToShortDateString
                            Else
                                Me._ClosedDate.StartDate = String.Empty
                            End If
                            Me._txtTitle.Text = .Title
                            '_lblTranslatedTitle.Text = Translation.TranslateUsingApi(.Title)
                            If _lblTranslatedTitle.Text.Trim = .Title.Trim Then _lblTranslatedTitle.Visible = False
                            If IsNumeric(.TankInspectionId) AndAlso CInt(.TankInspectionId) > 0 Then
                                TankType = TankTaskTypes.Inspection
                                'RefSite = "Tanks"
                                DisplayInspectionTypes()
                                If _ddlInspectionTypeList.Items.FindByValue(.TankInspectionId) IsNot Nothing Then
                                    _ddlInspectionTypeList.ClearSelection()
                                    _ddlInspectionTypeList.Items.FindByValue(.TankInspectionId).Selected = True
                                End If
                            End If
                            Me._txtDescription.Text = .Description
                            '_lblTranslatedDescription.Text = Translation.TranslateUsingApi(.Description)
                            If _lblTranslatedDescription.Text.Trim = .Description.Trim Then _lblTranslatedDescription.Visible = False
                            Me._txtLeadTime.Text = .LeadTime.ToString
                            Me._txtWorkOrder.Text = .WorkOrder.ToString
                            Me._lblCreatedBy.Text = .CreatedBy
                            If currentTaskItem.CreatedDate.Length > 0 Then
                                Me._lblCreatedDate.Text = IP.Bids.SharedFunctions.FormatDate(CDate(currentTaskItem.CreatedDate)) ' "EN-US", "d") '.CreatedDate
                            Else
                                Me._lblCreatedDate.Text = String.Empty
                            End If
                            If currentTaskItem.LastUpdateDate.Length > 0 Then
                                Me._lblLastUpdateDate.Text = IP.Bids.SharedFunctions.FormatDate(CDate(currentTaskItem.LastUpdateDate)) ', "EN-US", "d")
                            Else
                                Me._lblLastUpdateDate.Text = String.Empty
                            End If
                            Me._lblLastUpdatedBy.Text = .LastUpdateUserName

                            If Me._rblStatus.Items.FindByValue(CStr(.StatusSeqId)) IsNot Nothing Then
                                Me._rblStatus.ClearSelection()
                                Me._rblStatus.Items.FindByValue(CStr(.StatusSeqId)).Selected = True
                            Else
                                Me._rblStatus.ClearSelection()
                                _rblStatus.SelectedIndex = 0
                            End If
                            '_lblStatus.Text = _rblStatus.SelectedItem.Text
                            _lblStatus.Text = GetTaskStatus(CDate(.DueDate), .StatusSeqId, True)
                            If Me._ddlPriority.Items.FindByValue(.Priority) IsNot Nothing Then
                                Me._ddlPriority.ClearSelection()
                                Me._ddlPriority.Items.FindByValue(.Priority).Selected = True
                            End If

                            Me._txtEstimatedCost.Text = .EstimatedCost
                            Me._txtActualCost.Text = .ActualCost

                            If .ResponsibleRoleSeqId > 0 Then 'Use Role
                                Me._ResponsiblePerson.SelectedValue = CStr(.ResponsibleRoleSeqId)
                                If Me._ResponsiblePerson.SelectedValue.Length = 0 Then
                                    'Me._ResponsiblePerson.AddUser("", "MISSING: " & .RoleDescription)
                                    'Me._ResponsiblePerson.AddUser(CStr(.ResponsibleRoleSeqId), "Missing " & .RoleDescription)
                                    Dim missingEmployeeOnRole As String = IP.Bids.SharedFunctions.LocalizeValue(String.Format("The following role: {0} does not have an employee assigned to it.  Please select a user from the responsible list.", .RoleDescription), True)
                                    DisplayMessage(missingEmployeeOnRole)
                                End If
                                'Me._txtResponsiblePerson.Text = .RoleName & Space(500) & "|" & .ResponsibleRoleSeqId
                            Else
                                'Me._txtResponsiblePerson.Text = .ResponsibleName & Space(500) & "|" & .ResponsibleUserName
                                'Me._employeeList.DefaultUserName = .ResponsibleUserName
                                'If Not Page.IsPostBack Then
                                'Me._employeeList.PopulateEmployeeList()
                                'End If
                                Me._ResponsiblePerson.SelectedValue = .ResponsibleUserName
                                If Me._ResponsiblePerson.SelectedValue.Length = 0 Then
                                    Me._ResponsiblePerson.AddUser(.ResponsibleUserName, .ResponsibleName)
                                End If
                            End If
                            If .DueDate.Length > 0 Then
                                'CDate(currentTaskItem.LastUpdateDate).ToShortDateString
                                _EstimatedDueDate.StartDate = CDate(.DueDate).ToShortDateString
                            End If
                            If .DateCritical = "Y" Then
                                If taskItemBLL.IsDateCriticalChangeAllowed(TaskItemNumber, IP.Bids.SharedFunctions.GetCurrentUser.Username) Then
                                    _EstimatedDueDate.DateIsCritical = True
                                    _EstimatedDueDate.Enabled = True
                                Else
                                    _EstimatedDueDate.DateIsCritical = True
                                    _EstimatedDueDate.Enabled = False
                                End If

                            Else
                                _EstimatedDueDate.DateIsCritical = False
                                _EstimatedDueDate.Enabled = True
                            End If
                        End With
                    End If
                    'Recurring Tasks
                    If currentTaskItem.RecurringTasks IsNot Nothing AndAlso currentTaskItem.RecurringTasks.Count > 0 Then

                        Dim dl As DataList = _dlRecurringTasks
                        Dim dt As New Data.DataTable
                        Dim dr As Data.DataRow
                        If dl IsNot Nothing Then
                            dt.Columns.Add("taskdate")
                            dt.Columns.Add("url")
                            dt.Columns.Add("TaskItemSeqId")

                            'With Me._rblrecurringTasks
                            '    .Items.Clear()
                            '    .ClearSelection()
                            For Each task As RecurringTasks In currentTaskItem.RecurringTasks
                                '.Items.Add(New ListItem(task.DueDate, CStr(task.TaskItemSeqId)))
                                dr = dt.NewRow
                                dr.Item("TaskDate") = GetTaskStatus(CDate(task.DueDate), task.StatusSeqId, False)
                                'dr.Item("URL") = "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl(String.Format(CultureInfo.CurrentCulture, "~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}&RefSite={2}", currentTaskItem.TaskHeaderSeqId, task.TaskItemSeqId, RefSite)) & "',false);"
                                'Response.Redirect(Page.ResolveClientUrl(IP.Bids.SiteURLs.GetTaskDetailsURL(TaskHeaderNumber, RefSite, TaskItemNumber)), False)
                                dr.Item("URL") = "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl(IP.Bids.SiteURLs.GetTaskDetailsURL(CStr(currentTaskItem.TaskHeaderSeqId), RefSite, CStr(task.TaskItemSeqId))) & "',false);"
                                dr.Item("TaskItemSeqId") = task.TaskItemSeqId
                                '"JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskHeader.aspx?HeaderNumber=" & TaskHeaderNumber) & "'); return false;"
                                dt.Rows.Add(dr)
                            Next
                            'End With
                            'dt.DefaultView.Sort = "TaskItemSeqId"
                            dl.DataSource = dt.DefaultView
                            dl.DataBind()
                        End If
                        Me._rblUpdateTasks.Visible = True
                        Me._rblUpdateTasks.Enabled = True
                        _lblNoRecurringTasks.Visible = False
                        Me._cpeRecurringTasks.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Recurring Tasks", True) & GetBadgeLabel(currentTaskItem.RecurringTasks.Count.ToString)
                        Me._cpeRecurringTasks.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Recurring Tasks", True) & GetBadgeLabel(currentTaskItem.RecurringTasks.Count.ToString)
                        Me._cpeRecurringTasks.Collapsed = True
                        Me._cpeRecurringTasks.ClientState = "true"
                        Me._btnCancelRecurrence.Visible = True
                    Else
                        Me._cpeRecurringTasks.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Recurring Tasks", True) & GetBadgeLabel("0")
                        Me._cpeRecurringTasks.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Recurring Tasks", True) & GetBadgeLabel("0")
                        _lblNoRecurringTasks.Visible = True
                        Me._rblUpdateTasks.Visible = True
                        Me._rblUpdateTasks.Enabled = False
                        Me._btnCancelRecurrence.Visible = False
                    End If

                    Me._btnRecurrence.Url = Page.ResolveUrl(String.Format(CultureInfo.CurrentCulture, "~/Popups/Recurrence.aspx?RootTaskNumber={0}", currentTaskItem.RootTaskItemSeqId))
                    Session.Item("RootTaskFor_" & TaskItemNumber) = currentTaskItem.RootTaskItemSeqId

                    GetSubTasks(CInt(currentTaskItem.RootTaskItemSeqId))
                    'If currentTaskItem.Dependenttaskseqid.Length > 0 Then
                    '    _ifrDependentTasks.Attributes.Item("src") = Page.ResolveUrl("~/Popups/TaskList.aspx?ParentTaskNumber=" & currentTaskItem.Dependenttaskseqid & "&HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber & "&ShowHeaderInfo=False&AllowEdit=true&AllowDelete=true")
                    '    _ifrDependentTasks.Attributes.Item("onload") = "$('#" & _ifrDependentTasks.ClientID & "').load(function() {$('#" & _ifrDependentTasks.ClientID & "').css('display', 'block'); $('#preload-img2').css('display', 'none');  resizeFrame(document.getElementById('" & Me._ifrDependentTasks.ClientID & "'));});"
                    'Else
                    '    _ifrDependentTasks.Attributes.Item("src") = "" 'Page.ResolveUrl("~/Popups/TaskList.aspx?ParentTaskNumber=" & currentTaskItem.Dependenttaskseqid & "&HeaderNumber=" & TaskHeaderNumber & "&ShowHeaderInfo=False&AllowEdit=false")
                    '    _ifrDependentTasks.Attributes.Item("onload") = "$('#" & _ifrDependentTasks.ClientID & "').load(function() {$('#" & _ifrDependentTasks.ClientID & "').css('display', 'block'); $('#preload-img2').css('display', 'none');  resizeFrame(document.getElementById('" & Me._ifrDependentTasks.ClientID & "'));});"
                    'End If
                    GetDependentTasks(currentTaskItem.Dependenttaskseqid)
                    If currentTaskItem.RootTaskItemSeqId.Length > 0 Then
                        _btnAttachments.Url = Page.ResolveUrl("~/Popups/Attachments.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & currentTaskItem.RootTaskItemSeqId) 'TaskItemNumber)
                        Me._btnSubTasks.Url = Page.ResolveUrl("~/Popups/Subtasks.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & currentTaskItem.RootTaskItemSeqId)
                    Else
                        _btnAttachments.Url = Page.ResolveUrl("~/Popups/Attachments.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber) 'TaskItemNumber)
                        Me._btnSubTasks.Url = Page.ResolveUrl("~/Popups/Subtasks.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber)
                    End If

                    If SubTaskItemNumber Is Nothing OrElse IsNumeric(SubTaskItemNumber) = False Then
                        SubTaskItemNumber = "-1"
                    End If
                    Me._btnDependentTaskItems.Url = Page.ResolveUrl("~/Popups/Tasks.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & SubTaskItemNumber & "&ParentNumber=" & currentTaskItem.TaskItemSeqId)
                    Me._btnDependentTaskItems.AllowChildToCloseParent = True
                    Me._btnDependentTaskItems.ReloadPageOnClose = True
                    Session.Remove("SubTaskNumber")

                    PopulateComments()

                    'Dim comments As System.Collections.Generic.List(Of TaskItemComments) = taskItemBLL.GetTaskItemCommentsList(CInt(TaskItemNumber))
                    'If comments IsNot Nothing AndAlso comments.Count > 0 Then
                    '    With Me._gvComments
                    '        .DataSource = comments
                    '        .DataBind()
                    '    End With
                    '    _cpeComments.CollapsedText = "+ Show Comments (" & comments.Count & ")"
                    '    _cpeComments.ExpandedText = "- Hide Comments (" & comments.Count & ")"
                    'Else
                    '    _cpeComments.CollapsedText = "+ Show Comments (0)"
                    '    _cpeComments.ExpandedText = "- Hide Comments (0)"
                    'End If

                End If
            ElseIf CInt(TaskItemNumber) <> -1 Then

                'Response.Redirect("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=-1", False)
                'HttpContext.Current.ApplicationInstance.CompleteRequest()
            Else
                Me._EstimatedDueDate.StartDate = Now.ToShortDateString
                _btnAttachments.Enabled = False
                _btnAttachments.Visible = False
                _btnSaveandShowAttachments.Enabled = True
                _btnSaveandShowAttachments.Visible = True
                _btnCopyTask.Enabled = False
                _btnSubTasks.Enabled = False
                _btnSubTasks.Visible = False
                _btnSaveandShowSubTasks.Enabled = True
                _btnSaveandShowSubTasks.Visible = True
                _btnSaveandShowRecurrence.Enabled = True
                _btnSaveandShowRecurrence.Visible = True
                Me._btnRecurrence.Enabled = False
                _btnRecurrence.Visible = False
                '''''Me._EstimatedDueDate.StartDate = String.Empty 'Now.ToShortDateString  - JEB commented out on 11/15/2018
                If (Not Me._rblStatus.SelectedValue Is Nothing AndAlso String.IsNullOrEmpty(Me._rblStatus.SelectedValue)) Then
                    Me._rblStatus.SelectedIndex = 0
                End If
                _lblNoRecurringTasks.Visible = True
                Me._rblUpdateTasks.Visible = True
                Me._rblUpdateTasks.Enabled = False
                Me._cpeRecurringTasks.Collapsed = False
                Me._cpeRecurringTasks.ClientState = "false"
                _btnAttachments.Enabled = False
                _btnCopyTask.Enabled = False
                _btnSubTasks.Enabled = False


                Me._cpeAttachments.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Attachments and Links", True) & GetBadgeLabel("0")
                Me._cpeAttachments.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Attachments and Links", True) & GetBadgeLabel("0")
                Me._cpeRecurringTasks.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Recurring Tasks", True) & GetBadgeLabel("0")
                Me._cpeRecurringTasks.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Recurring Tasks", True) & GetBadgeLabel("0")
                Me._cpeSubTasks.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Subsequent Task Items", True) & GetBadgeLabel("0")
                Me._cpeSubTasks.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Subsequent Task Items", True) & GetBadgeLabel("0")
                Me._cpeDependentTasks.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Sub Task Items", True) & GetBadgeLabel("0")
                Me._cpeDependentTasks.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Sub Task Items", True) & GetBadgeLabel("0")
                _cpeComments.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Comments", True) & GetBadgeLabel("0")
                _cpeComments.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Comments", True) & GetBadgeLabel("0")
                Me._cpeSubTasks.Collapsed = True
                Me._cpeSubTasks.ClientState = "true"
                Me._cpeDependentTasks.Collapsed = True
                Me._cpeDependentTasks.ClientState = "true"
                _fa.Visible = False
            End If
            If Page.IsPostBack Then
                If IP.Bids.SharedFunctions.CausedPostBack(_ClosedDate.UniqueID) Then
                    If Request(_ClosedDate.UniqueID & "$_txtDateFrom") IsNot Nothing Then
                        _ClosedDate.StartDate = Request(_ClosedDate.UniqueID & "$_txtDateFrom")
                    End If
                    _ClosedDate_DateChanged(Nothing)
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("HandlePageLoad", , ex)
        End Try
    End Sub
    Private Sub PopulateExternalSourceButton(ByVal currentSourceId As Integer, ByVal externalSourceReferenceNumber As String)
        Dim newIncident As TaskTrackerListsBll
        Dim displaySourceSystemJs As String = "PopupWindow('{0}{2}','{1}',600,600,'yes','yes','true');return false;"
        Dim sourceFound As Boolean
        Dim externalSourceName As String = String.Empty
        Try
            newIncident = New TaskTrackerListsBll()
            If newIncident IsNot Nothing Then
                Dim externalSites As System.Collections.Generic.List(Of SourceSystems) = newIncident.GetSourceSystems

                For Each item As SourceSystems In externalSites
                    If item.ExternalSourceSeqid = currentSourceId And currentSourceId <> IP.Bids.SharedFunctions.SourceSystemID.IRIS Then
                        _btnReturnToSourceSystem.OnClientClick = String.Format(displaySourceSystemJs, item.ExternalSourceUrl, "MTTSource", externalSourceReferenceNumber)
                        sourceFound = True
                        externalSourceName = item.ExternalSource
                    End If
                Next
            End If
            If sourceFound = True Then
                _btnReturnToSourceSystem.Visible = True
                _lblReturnToSourceSystem.Text = Master.IPResources.GetResourceValue("Go To") & " " & Master.GetLocalizedValue(externalSourceName, True)
            Else
                _btnReturnToSourceSystem.Visible = False
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateExternalSourceButton", , ex)
        Finally
            newIncident = Nothing
        End Try
    End Sub
    Public Sub PopulateComments()
        Dim taskItemBLL As New TaskTrackerItemBll
        Dim comments As System.Collections.Generic.List(Of TaskItemComments) = taskItemBLL.GetTaskItemCommentsList(CInt(TaskItemNumber))
        If comments IsNot Nothing AndAlso comments.Count > 0 Then
            _btnEditComments.Enabled = True
            With Me._gvComments
                .DataSource = comments
                .DataBind()
            End With
            _cpeComments.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Comments", True) & GetBadgeLabel(comments.Count.ToString)
            _cpeComments.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Comments", True) & GetBadgeLabel(comments.Count.ToString)
        Else
            _btnEditComments.Enabled = False
            _cpeComments.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Comments", True) & GetBadgeLabel("0")
            _cpeComments.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Comments", True) & GetBadgeLabel("0")
        End If
    End Sub
    Public Function GetTaskStatus(ByVal dueDate As Date, ByVal statusID As Integer, ByVal includeLabel As Boolean) As String
        Dim taskSearch As New TaskTrackerListsBll
        Dim imgPath As String = Page.ResolveUrl("~/Images/")
        Dim status As String = String.Empty
        Dim overdueStatusID As Integer = -1
        Dim spacer As String = String.Empty

        If includeLabel Then
            spacer = "&nbsp; - "
        End If

        If dueDate < Now And statusID = OpenTaskSeqId Then
            status = taskSearch.GetTaskStatus(overdueStatusID, includeLabel, imgPath) & spacer & IP.Bids.SharedFunctions.FormatDate(dueDate) '& " - " & statusName
        Else
            status = taskSearch.GetTaskStatus(statusID, includeLabel, imgPath) & spacer & IP.Bids.SharedFunctions.FormatDate(dueDate) '& " - " & statusName
        End If
        taskSearch = Nothing
        Return status
    End Function

    Public Sub RemoveUpdateAccess()
        Me._btnSaveTask.Enabled = False
        Me._rblUpdateTasks.Visible = False
        Me._btnSaveandShowAttachments.Enabled = False
        Me._rblStatus.Enabled = False
        Me._ClosedDate.Enabled = False
        Me._ddlInspectionTypeList.Enabled = False
    End Sub

    Public Sub ReadOnlyMode()
        Me._btnSaveTask.Enabled = True
        Me._ResponsiblePerson.Enabled = False
        Me._txtTitle.Enabled = False
        Me._ddlPriority.Enabled = False
        Me._txtLeadTime.Enabled = False
        Me._txtDescription.Enabled = False
        'Me._rblStatus.Enabled = False
        'Me._ClosedDate.Enabled = False
        Me._EstimatedDueDate.Enabled = False
        Me._rblUpdateTasks.Enabled = False
        Me._rblUpdateTasks.Visible = True
        _btnAssignReplication.Visible = False
        Me._btnSaveandShowAttachments.Enabled = True
        Me._btnSaveandShowRecurrence.Enabled = False
        Me._btnSaveandShowSubTasks.Enabled = False
        Me._btnSaveAndShowDependentTaskItems.Enabled = False
        'Me._btnSaveTask.Enabled = False
        Me._txtWorkOrder.Enabled = False
        _ResponsiblePerson.EnableValidation = False
        _rfvTitle.Enabled = False
        Me._txtActualCost.Enabled = False
        Me._txtEstimatedCost.Enabled = False
        ' Me._txtComments.Enabled = False
        'Me._btnAddTask.Enabled = False
    End Sub
    Public Sub EditMode()
        Me._btnSaveTask.Enabled = True
        Me._ResponsiblePerson.Enabled = True
        _btnAssignReplication.Visible = False

        Me._txtTitle.Enabled = True
        Me._ddlPriority.Enabled = True
        Me._txtLeadTime.Enabled = True
        Me._txtDescription.Enabled = True
        'Me._rblStatus.Enabled = False
        Me._ClosedDate.Enabled = True
        'Me._EstimatedDueDate.Enabled = True
        'Me._rblUpdateTasks.Enabled = True
        Me._rblUpdateTasks.Visible = True
        Me._btnSaveandShowAttachments.Enabled = True
        Me._btnSaveandShowRecurrence.Enabled = True
        Me._btnSaveandShowSubTasks.Enabled = True
        Me._btnSaveAndShowDependentTaskItems.Enabled = True
        Me._btnSaveTask.Enabled = True
        Me._btnAddTask.Enabled = True
        Me._txtWorkOrder.Enabled = True
        Me._txtActualCost.Enabled = True
        Me._txtEstimatedCost.Enabled = True
        Me._txtComments.Enabled = True
        _ResponsiblePerson.EnableValidation = True
        _rfvTitle.Enabled = True
    End Sub

    Private Sub GetSubTasks(ByVal rootTaskItemNumber As Integer)

        Dim taskTrackerBLL As New TaskTrackerItemBll
        Dim subTaskItems As New System.Collections.Generic.List(Of SubTaskItem)
        Try
            With taskTrackerBLL

                subTaskItems = .GetSubTaskItemList(rootTaskItemNumber, True)
            End With
            ' Me._btnAddSubTask.CommandArgument = -1
            'If subTaskItems IsNot Nothing AndAlso subTaskItems.Count > 0 Then
            Me._gvSubTask.DataSource = subTaskItems
            Me._gvSubTask.DataBind()
            Me._gvSubTask.AutoGenerateColumns = False
            If subTaskItems IsNot Nothing AndAlso subTaskItems.Count > 0 Then
                Me._cpeSubTasks.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Subsequent Task Items", True) & GetBadgeLabel(subTaskItems.Count.ToString)
                Me._cpeSubTasks.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Subsequent Task Items", True) & GetBadgeLabel(subTaskItems.Count.ToString)
                Me._cpeSubTasks.Collapsed = False
                Me._cpeSubTasks.ClientState = "false"
            Else
                Me._cpeSubTasks.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Subsequent Task Items", True) & GetBadgeLabel("0")
                Me._cpeSubTasks.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Subsequent Task Items", True) & GetBadgeLabel("0")
                Me._cpeSubTasks.Collapsed = True
                Me._cpeSubTasks.ClientState = "true"

            End If
        Catch
            Throw
        End Try
        subTaskItems = Nothing
        taskTrackerBLL = Nothing
    End Sub

    Private Sub GetDependentTasks(ByVal rootTaskItemNumber As String)

        Dim taskTrackerBLL As New TaskTrackerItemBll
        Dim dependentTaskItems As New System.Collections.Generic.List(Of TaskItem)
        HasOpenSubtasks = False

        Try
            If rootTaskItemNumber.Length = 0 OrElse IsNumeric(rootTaskItemNumber) = False Then
                rootTaskItemNumber = "-1"
            End If
            With _dependenttask
                .AllowDelete = True
                .AllowEdit = True
                .ParentTaskNumber = rootTaskItemNumber
                .TaskHeaderNumber = TaskHeaderNumber
                .TaskNumber = TaskItemNumber
                If _rblUpdateTasks.Items.Item(0).Selected Then
                    .UpdateFlag = UserControlsTasksTaskListing.UpdateFlagValues.Current
                Else
                    .UpdateFlag = UserControlsTasksTaskListing.UpdateFlagValues.Future
                End If
                .LoadTaskItems()
            End With
            With taskTrackerBLL
                If IsNumeric(rootTaskItemNumber) Then
                    dependentTaskItems = .GetDependentTaskItemList(CInt(rootTaskItemNumber))
                End If
            End With

            'Determine if we have any sub tasks that are still open
            For i As Integer = 0 To dependentTaskItems.Count - 1
                If dependentTaskItems(i).ClosedDate.Length = 0 Then
                    HasOpenSubtasks = True
                End If
            Next

            If dependentTaskItems IsNot Nothing AndAlso dependentTaskItems.Count > 0 Then
                Me._cpeDependentTasks.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Sub Task Items", True) & GetBadgeLabel(dependentTaskItems.Count.ToString)
                Me._cpeDependentTasks.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Sub Task Items", True) & GetBadgeLabel(dependentTaskItems.Count.ToString)
                Me._cpeDependentTasks.Collapsed = False
                Me._cpeDependentTasks.ClientState = "false"
                Me.HasSubTasks = True

            Else
                Me._cpeDependentTasks.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Sub Task Items", True) & GetBadgeLabel("0")
                Me._cpeDependentTasks.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Sub Task Items", True) & GetBadgeLabel("0")
                Me._cpeDependentTasks.Collapsed = True
                Me._cpeDependentTasks.ClientState = "true"
                Me.HasSubTasks = False

            End If
        Catch
            Throw
        End Try
        dependentTaskItems = Nothing
        taskTrackerBLL = Nothing
    End Sub


    Protected Sub _gvSubsequentTask_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles _gvSubTask.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lblResponsible As Label = CType(e.Row.FindControl("_lblResponsible"), Label)
            If lblResponsible IsNot Nothing Then
                Dim subsequentTask As SubTaskItem = TryCast(e.Row.DataItem, SubTaskItem)
                If subsequentTask IsNot Nothing Then
                    'If subsequentTask.TaskItemSeqId <> subsequentTask.ParentSubTaskSeqID Then
                    '    e.Row.Visible = False
                    'End If
                    If subsequentTask.ResponsibleUserName.Length > 0 Then
                        lblResponsible.Text = subsequentTask.ResponsibleName
                    Else
                        lblResponsible.Text = subsequentTask.ResponsibleRoleSiteName & " - " & subsequentTask.RoleDescription
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub SaveTask(Optional ByVal afterSaveUrl As String = "#")
        Dim newTaskItem As New TaskItem()
        Dim taskItemBLL As New TaskTrackerItemBll
        Dim isDirty As Boolean
        Dim isNewTask As Boolean
        Dim sendEmail As Boolean
        Dim isTankTask As Boolean
        'Dim currentTaskItem As New TaskItem

        Me.Validate("TaskDetails")
        If Page.IsValid Then
            newTaskItem = taskItemBLL.GetTaskItem(CInt(TaskItemNumber))
            If newTaskItem Is Nothing Or CInt(TaskItemNumber) = -1 Then
                'Updating a Task
                newTaskItem = New TaskItem()
                isDirty = True
                isNewTask = True
                newTaskItem.TaskItemSeqId = -1
                newTaskItem.RootTaskItemSeqId = ""
                sendEmail = True
            End If

            With newTaskItem
                If .TaskItemSeqId = -1 Then
                    .CreatedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Now) 'Now.ToShortDateString
                    .CreatedBy = Master.CurrentUser.Username
                    .LastUpdateDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Now)
                    .LastUpdateUserName = Master.CurrentUser.Username
                    .ClosedDate = String.Empty
                    isDirty = True
                Else
                    If IsDate(.CreatedDate) Then
                        .CreatedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(.CreatedDate))
                    End If
                    .LastUpdateDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Now)
                    .LastUpdateUserName = Master.CurrentUser.Username
                End If

                If isNewTask Then
                    '.ClosedDate = Me._ClosedDate.StartDate
                    If Me._ClosedDate.StartDate.Length > 0 AndAlso IsDate(Me._ClosedDate.StartDate) Then
                        .ClosedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(Me._ClosedDate.StartDate))
                    Else
                        .ClosedDate = String.Empty
                    End If
                Else
                    If IsDate(.ClosedDate) AndAlso IsDate(Me._ClosedDate.StartDate) Then
                        If CDate(.ClosedDate) <> CDate(Me._ClosedDate.StartDate) Then
                            .ClosedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(Me._ClosedDate.StartDate))
                            isDirty = True
                        Else
                            'lets insure that we have an english date
                            .ClosedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(Me._ClosedDate.StartDate))
                        End If
                    ElseIf .ClosedDate <> Me._ClosedDate.StartDate AndAlso IsDate(Me._ClosedDate.StartDate) Then
                        .ClosedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(Me._ClosedDate.StartDate))
                        isDirty = True
                    Else
                        .ClosedDate = ""
                    End If
                End If

                If isNewTask Then
                    If _EstimatedDueDate.DateIsCritical Then
                        .DateCritical = "Y"
                    Else
                        .DateCritical = "N"
                    End If
                    isDirty = True
                Else
                    If _EstimatedDueDate.DateIsCritical = True And .DateCritical.ToUpper = "Y" Then
                        'no change
                    ElseIf _EstimatedDueDate.DateIsCritical = False And .DateCritical.ToUpper = "N" Then
                        'no change
                    Else
                        If _EstimatedDueDate.DateIsCritical Then
                            .DateCritical = "Y"
                        Else
                            .DateCritical = "N"
                        End If
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .Description = _txtDescription.Text
                    isDirty = True
                Else
                    If .Description <> _txtDescription.Text Then
                        .Description = _txtDescription.Text
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .DueDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(_EstimatedDueDate.StartDate))
                Else
                    If .DueDate <> _EstimatedDueDate.StartDate Then
                        .DueDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(_EstimatedDueDate.StartDate))
                        isDirty = True
                    Else
                        'Let's make sure that the due date is in English
                        .DueDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(_EstimatedDueDate.StartDate))
                    End If
                End If



                If isNewTask Then
                    If IsNumeric(_txtLeadTime.Text) Then
                        .LeadTime = CInt(Me._txtLeadTime.Text)
                    Else
                        .LeadTime = 0
                    End If
                Else
                    If IsNumeric(_txtLeadTime.Text) AndAlso .LeadTime <> CInt(Me._txtLeadTime.Text) Then
                        If IsNumeric(_txtLeadTime.Text) Then
                            .LeadTime = CInt(Me._txtLeadTime.Text)
                        Else
                            .LeadTime = 0
                        End If
                        isDirty = True
                    ElseIf IsNumeric(_txtLeadTime.Text) AndAlso .LeadTime = CInt(Me._txtLeadTime.Text) Then
                        .LeadTime = CInt(Me._txtLeadTime.Text)
                        'no change
                    Else
                        .LeadTime = 0
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .Priority = Me._ddlPriority.SelectedValue
                Else
                    If .Priority <> _ddlPriority.SelectedValue Then
                        .Priority = Me._ddlPriority.SelectedValue
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .StatusSeqId = CInt(Me._rblStatus.SelectedValue)
                Else
                    If .StatusSeqId <> CInt(Me._rblStatus.SelectedValue) Then
                        .StatusSeqId = CInt(Me._rblStatus.SelectedValue)
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .WorkOrder = Me._txtWorkOrder.Text
                Else
                    If .WorkOrder <> Me._txtWorkOrder.Text Then
                        .WorkOrder = Me._txtWorkOrder.Text
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .EstimatedCost = Me._txtEstimatedCost.Text
                Else
                    If .EstimatedCost <> _txtEstimatedCost.Text Then
                        .EstimatedCost = Me._txtEstimatedCost.Text
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .ActualCost = Me._txtActualCost.Text
                Else
                    If .ActualCost <> _txtActualCost.Text Then
                        .ActualCost = Me._txtActualCost.Text
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
                        If IsNumeric(_ResponsiblePerson.SelectedValue) AndAlso .ClosedDate IsNot Nothing AndAlso .ClosedDate.Length > 0 Then 'User is attempting to close a task that is assigned to a role
                            If IP.Bids.SharedFunctions.GetCurrentUser.IsUserInEmployeeTable = True Then
                                _ResponsiblePerson.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                                _ResponsiblePerson.PlantCode = IP.Bids.SharedFunctions.GetCurrentUser.PlantCode
                                _ResponsiblePerson.PopulateEmployeeList()
                            Else
                                .ClosedDate = String.Empty
                                _rblStatus.ClearSelection()
                                .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
                                _rblStatus.Items(OpenTaskSeqId).Selected = True
                                Session.Add("TaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("An employee has to be selected before a Task can be Completed.", True))
                                isDirty = True
                                afterSaveUrl = "RELOAD"
                            End If
                        End If
                    End If
                    If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
                        If IsNumeric(_ResponsiblePerson.SelectedValue) Then 'Role
                            .ResponsibleRoleSeqId = CInt(_ResponsiblePerson.SelectedValue)
                            .RoleName = _ResponsiblePerson.SelectedText
                            .ResponsibleUserName = String.Empty
                            .ResponsibleName = String.Empty
                            .ResponsibleRolePlantCode = _ResponsiblePerson.PlantCode

                            If .ClosedDate IsNot Nothing AndAlso .ClosedDate.Length > 0 Then
                                'Someone other than a Role Name should be used for the Responsible Person
                                .ClosedDate = String.Empty
                                _rblStatus.ClearSelection()
                                .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
                                _rblStatus.Items(OpenTaskSeqId).Selected = True
                                Session.Add("TaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("An employee has to be selected before a Task can be Completed.", True))

                            End If
                            isDirty = True
                        Else
                            .ResponsibleRoleSeqId = 0
                            .RoleName = String.Empty
                            .ResponsibleRolePlantCode = String.Empty
                            .ResponsibleUserName = _ResponsiblePerson.SelectedValue
                            .ResponsibleName = _ResponsiblePerson.SelectedText
                            isDirty = True
                        End If
                    End If
                Else
                    If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
                        If IsNumeric(_ResponsiblePerson.SelectedValue) AndAlso .ClosedDate.Length > 0 Then 'User is attempting to close a task that is assigned to a role
                            'New - Set the role to the current user
                            If IP.Bids.SharedFunctions.GetCurrentUser.IsUserInEmployeeTable = True Then
                                _ResponsiblePerson.PlantCode = IP.Bids.SharedFunctions.GetCurrentUser.PlantCode
                                _ResponsiblePerson.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                                _ResponsiblePerson.PopulateEmployeeList()
                            Else
                                .ClosedDate = String.Empty
                                _rblStatus.ClearSelection()
                                .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
                                _rblStatus.Items(OpenTaskSeqId).Selected = True
                                Session.Add("TaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("An employee has to be selected before a Task can be Completed.", True))
                                isDirty = True
                                afterSaveUrl = "RELOAD"
                            End If
                        End If
                    Else
                        If .ClosedDate.Length > 0 Then 'User is attempting to close a task that is assigned to a role
                            'New - Set the role to the current user
                            If IP.Bids.SharedFunctions.GetCurrentUser.IsUserInEmployeeTable = True Then
                                _ResponsiblePerson.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                                _ResponsiblePerson.PlantCode = IP.Bids.SharedFunctions.GetCurrentUser.PlantCode
                                _ResponsiblePerson.PopulateEmployeeList()
                            Else
                                .ClosedDate = String.Empty
                                _rblStatus.ClearSelection()
                                .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
                                _rblStatus.Items(OpenTaskSeqId).Selected = True
                                Session.Add("TaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("An employee has to be selected before a Task can be Completed.", True))
                                isDirty = True
                                afterSaveUrl = "RELOAD"
                            End If
                        End If
                    End If
                    If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
                        If IsNumeric(_ResponsiblePerson.SelectedValue) Then 'Role
                            If CInt(_ResponsiblePerson.SelectedValue) <> .ResponsibleRoleSeqId Or .ResponsibleRolePlantCode <> _ResponsiblePerson.PlantCode Then
                                .ResponsibleRoleSeqId = CInt(_ResponsiblePerson.SelectedValue)
                                .RoleName = _ResponsiblePerson.SelectedText
                                .ResponsibleRolePlantCode = _ResponsiblePerson.PlantCode
                                .ResponsibleUserName = String.Empty
                                .ResponsibleName = String.Empty
                                sendEmail = True
                                isDirty = True
                            End If
                            If .ClosedDate.Length > 0 Then
                                'Someone other than a Role Name should be used for the Responsible Person
                                .ClosedDate = String.Empty
                                _rblStatus.ClearSelection()
                                .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
                                _rblStatus.Items(OpenTaskSeqId).Selected = True
                                Session.Add("TaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("An employee has to be selected before a Task can be Completed.", True))
                                isDirty = True
                            End If
                            'isDirty = True

                        Else
                            If .ResponsibleUserName <> _ResponsiblePerson.SelectedValue Then 'AndAlso .ResponsibleRolePlantCode <> _ResponsiblePerson.PlantCode Then ' MJP 8/22/2013 to fix a saving issue
                                .ResponsibleRoleSeqId = 0
                                .RoleName = String.Empty
                                .ResponsibleRolePlantCode = String.Empty
                                .ResponsibleUserName = _ResponsiblePerson.SelectedValue
                                .ResponsibleName = _ResponsiblePerson.SelectedText
                                isDirty = True
                                sendEmail = True
                            End If
                        End If
                    Else
                        'this should be a required field
                    End If
                End If



                If isNewTask Then
                    .TaskHeaderSeqId = CInt(TaskHeaderNumber)
                Else
                    If .TaskHeaderSeqId <> CInt(TaskHeaderNumber) Then
                        .TaskHeaderSeqId = CInt(TaskHeaderNumber)
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .TaskItemSeqId = CInt(TaskItemNumber)
                Else
                    If .TaskItemSeqId <> CInt(TaskItemNumber) Then
                        .TaskItemSeqId = CInt(TaskItemNumber)
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .Title = Me._txtTitle.Text
                Else
                    If .Title <> Me._txtTitle.Text Then
                        .Title = Me._txtTitle.Text
                        isDirty = True
                    End If
                End If


                If (RefSite.ToLower = "tanks" Or (IsNumeric(.TankInspectionId) AndAlso CInt(.TankInspectionId) > 0)) Then
                    If TankType = TankTaskTypes.Inspection Then
                        isTankTask = True
                        If isNewTask Then
                            .Title = _ddlInspectionTypeList.SelectedItem.Text

                        Else
                            If .Title <> _ddlInspectionTypeList.SelectedItem.Text Then
                                .Title = _ddlInspectionTypeList.SelectedItem.Text
                                isDirty = True
                            End If
                        End If
                        .TankInspectionId = _ddlInspectionTypeList.SelectedValue
                    End If
                End If

                If isNewTask Then
                    .Comments = Me._txtComments.Text
                Else
                    If .Comments <> Me._txtComments.Text Then
                        .Comments = Me._txtComments.Text
                        isDirty = True
                    End If
                End If
                Me._txtComments.Text = String.Empty

                If isNewTask Then
                    .UpdateFlag = Me._rblUpdateTasks.SelectedValue
                Else
                    If .UpdateFlag <> Me._rblUpdateTasks.SelectedValue Then
                        .UpdateFlag = Me._rblUpdateTasks.SelectedValue
                        isDirty = True
                    End If
                End If
            End With

            If Me._ClosedDate.StartDate.Length > 0 Then
                ReadOnlyMode()
                If newTaskItem.Dependenttaskseqid IsNot Nothing AndAlso newTaskItem.Dependenttaskseqid.Length = 0 Then
                    _btnSaveandShowRecurrence.Enabled = True
                End If
            Else
                EditMode()
            End If

            If isDirty Then
                ' Dim taskTrackerBLL As New TaskTrackerItemBll
                ' Dim subTaskItems As New System.Collections.Generic.List(Of SubTaskItem)
                If Me._ClosedDate.StartDate.Length > 0 Then
                    '    If newTaskItem.RootTaskItemSeqId IsNot Nothing AndAlso IsNumeric(newTaskItem.RootTaskItemSeqId) Then

                    '    End If
                End If

                Dim updatedTaskItem As System.Collections.Generic.List(Of TaskItem) = taskItemBLL.SaveTaskItem(newTaskItem)

                If updatedTaskItem IsNot Nothing AndAlso updatedTaskItem.Count > 0 Then

                    If Me._ClosedDate.StartDate.Length > 0 Then
                        Dim taskTrackerBLL As New TaskTrackerItemBll
                        Dim subTaskItems As New System.Collections.Generic.List(Of SubTaskItem)
                        If newTaskItem.RootTaskItemSeqId.Length > 0 AndAlso IsNumeric(newTaskItem.RootTaskItemSeqId) Then
                            subTaskItems = taskTrackerBLL.GetSubTaskItemList(CInt(newTaskItem.RootTaskItemSeqId), False)
                        End If
                        If subTaskItems IsNot Nothing AndAlso subTaskItems.Count > 0 Then
                            For i As Integer = 0 To subTaskItems.Count - 1
                                'TODO - Need a way to determine if the status was changed from Complete to NWN or Cancel
                                EmailDataBll.GetAndSendSubsequentTasksImmediateEmail(CInt(subTaskItems.Item(i).TaskItemSeqId), CInt(TaskHeaderNumber), IP.Bids.SharedFunctions.CDateFromEnglishDate(subTaskItems.Item(i).DueDate))
                            Next
                        End If
                    End If
                    TaskItemNumber = CStr(updatedTaskItem.Item(0).TaskItemSeqId)
                    If isTankTask And isNewTask Then
                        CreatTanksSubTask(String.Format("{0} - {1} {0} Date", newTaskItem.Title, "Review and Verify next"), newTaskItem.TankInspectionId)
                    End If
                    If sendEmail And Me._ClosedDate.StartDate.Length = 0 Then
                        EmailDataBll.GetAndSendImmediateEmail(updatedTaskItem.Item(0).TaskItemSeqId, CInt(TaskHeaderNumber))
                    End If
                    If afterSaveUrl = "#" Then
                        'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
                    ElseIf afterSaveUrl.ToLower = "subtasks" Then
                        Session.Add("popupwindow", "subtasks")
                        'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
                    ElseIf afterSaveUrl.ToLower = "dependenttasks" Then
                        Session.Add("popupwindow", "dependenttasks")
                    ElseIf afterSaveUrl.ToLower = "editcomments" Then
                        Session.Add("popupwindow", "editcomments")
                    ElseIf afterSaveUrl.ToLower = "recurrence" Then
                        Session.Add("popupwindow", "recurrence")
                        'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
                    ElseIf afterSaveUrl.ToLower = "attachments" Then
                        Session.Add("popupwindow", "attachments")
                        'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
                    ElseIf afterSaveUrl.ToLower = "reload" Then
                        IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId & "&RefSite=" & RefSite)
                        'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    Else
                        IP.Bids.SharedFunctions.ResponseRedirect(afterSaveUrl) '& "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
                        'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    End If

                    'If isNewTask Then
                    IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId & "&RefSite=" & RefSite)
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    'End If
                    Me._lblLastUpdateDate.Text = IP.Bids.SharedFunctions.FormatDate(updatedTaskItem.Item(0).LastUpdateDate) 'updatedTaskItem.Item(0).LastUpdateDate
                    Me._lblLastUpdatedBy.Text = updatedTaskItem.Item(0).LastUpdateUserName
                    updatedTaskItem = Nothing
                    'jeb Me._udpTaskItems.Update()
                End If
            Else
                If afterSaveUrl = "#" Then
                ElseIf afterSaveUrl.ToLower = "subtasks" Then
                    Session.Add("popupwindow", "subtasks")
                ElseIf afterSaveUrl.ToLower = "dependenttasks" Then
                    Session.Add("popupwindow", "dependenttasks")
                ElseIf afterSaveUrl.ToLower = "recurrence" Then
                    Session.Add("popupwindow", "recurrence")
                ElseIf afterSaveUrl.ToLower = "attachments" Then
                    Session.Add("popupwindow", "attachments")
                ElseIf afterSaveUrl.ToLower = "editcomments" Then
                    Session.Add("popupwindow", "editcomments")
                ElseIf afterSaveUrl.ToLower = "reload" Then
                    IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber & "&RefSite=" & RefSite)
                Else
                    IP.Bids.SharedFunctions.ResponseRedirect(afterSaveUrl)
                End If
            End If
        End If
        PopulateComments()
    End Sub



    Private Sub SaveTaskAdd(Optional ByVal afterSaveUrl As String = "#")
        Dim newTaskItem As New TaskItem()
        Dim taskItemBLL As New TaskTrackerItemBll
        Dim isDirty As Boolean
        Dim isNewTask As Boolean
        Dim sendEmail As Boolean
        Dim isTankTask As Boolean
        'Dim currentTaskItem As New TaskItem
        TaskItemNumber = "-1"


        Me.Validate("TaskDetails")
        If Page.IsValid Then
            newTaskItem = taskItemBLL.GetTaskItem(CInt(-1))
            If newTaskItem Is Nothing Or CInt(TaskItemNumber) = -1 Then
                'Updating a Task
                newTaskItem = New TaskItem()
                isDirty = True
                isNewTask = True
                newTaskItem.TaskItemSeqId = -1
                newTaskItem.RootTaskItemSeqId = ""
                sendEmail = True
            End If


            With newTaskItem
                If .TaskItemSeqId = -1 Then
                    .CreatedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Now) 'Now.ToShortDateString
                    .CreatedBy = Master.CurrentUser.Username
                    .LastUpdateDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Now)
                    .LastUpdateUserName = Master.CurrentUser.Username
                    .ClosedDate = String.Empty
                    isDirty = True
                Else
                    If IsDate(.CreatedDate) Then
                        .CreatedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(.CreatedDate))
                    End If
                    .LastUpdateDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Now)
                    .LastUpdateUserName = Master.CurrentUser.Username
                End If

                If isNewTask Then
                    '.ClosedDate = Me._ClosedDate.StartDate
                    If Me._ClosedDate.StartDate.Length > 0 AndAlso IsDate(Me._ClosedDate.StartDate) Then
                        .ClosedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(Me._ClosedDate.StartDate))
                    Else
                        .ClosedDate = String.Empty
                    End If
                Else
                    If IsDate(.ClosedDate) AndAlso IsDate(Me._ClosedDate.StartDate) Then
                        If CDate(.ClosedDate) <> CDate(Me._ClosedDate.StartDate) Then
                            .ClosedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(Me._ClosedDate.StartDate))
                            isDirty = True
                        Else
                            'lets insure that we have an english date
                            .ClosedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(Me._ClosedDate.StartDate))
                        End If
                    ElseIf .ClosedDate <> Me._ClosedDate.StartDate AndAlso IsDate(Me._ClosedDate.StartDate) Then
                        .ClosedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(Me._ClosedDate.StartDate))
                        isDirty = True
                    Else
                        .ClosedDate = ""
                    End If
                End If

                If isNewTask Then
                    If _EstimatedDueDate.DateIsCritical Then
                        .DateCritical = "Y"
                    Else
                        .DateCritical = "N"
                    End If
                    isDirty = True
                Else
                    If _EstimatedDueDate.DateIsCritical = True And .DateCritical.ToUpper = "Y" Then
                        'no change
                    ElseIf _EstimatedDueDate.DateIsCritical = False And .DateCritical.ToUpper = "N" Then
                        'no change
                    Else
                        If _EstimatedDueDate.DateIsCritical Then
                            .DateCritical = "Y"
                        Else
                            .DateCritical = "N"
                        End If
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .Description = _txtDescription.Text
                    isDirty = True
                Else
                    If .Description <> _txtDescription.Text Then
                        .Description = _txtDescription.Text
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .DueDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(_EstimatedDueDate.StartDate))
                Else
                    If .DueDate <> _EstimatedDueDate.StartDate Then
                        .DueDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(_EstimatedDueDate.StartDate))
                        isDirty = True
                    Else
                        'Let's make sure that the due date is in English
                        .DueDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(_EstimatedDueDate.StartDate))
                    End If
                End If



                If isNewTask Then
                    If IsNumeric(_txtLeadTime.Text) Then
                        .LeadTime = CInt(Me._txtLeadTime.Text)
                    Else
                        .LeadTime = 0
                    End If
                Else
                    If IsNumeric(_txtLeadTime.Text) AndAlso .LeadTime <> CInt(Me._txtLeadTime.Text) Then
                        If IsNumeric(_txtLeadTime.Text) Then
                            .LeadTime = CInt(Me._txtLeadTime.Text)
                        Else
                            .LeadTime = 0
                        End If
                        isDirty = True
                    ElseIf IsNumeric(_txtLeadTime.Text) AndAlso .LeadTime = CInt(Me._txtLeadTime.Text) Then
                        .LeadTime = CInt(Me._txtLeadTime.Text)
                        'no change
                    Else
                        .LeadTime = 0
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .Priority = Me._ddlPriority.SelectedValue
                Else
                    If .Priority <> _ddlPriority.SelectedValue Then
                        .Priority = Me._ddlPriority.SelectedValue
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .StatusSeqId = CInt(Me._rblStatus.SelectedValue)
                Else
                    If .StatusSeqId <> CInt(Me._rblStatus.SelectedValue) Then
                        .StatusSeqId = CInt(Me._rblStatus.SelectedValue)
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .WorkOrder = Me._txtWorkOrder.Text
                Else
                    If .WorkOrder <> Me._txtWorkOrder.Text Then
                        .WorkOrder = Me._txtWorkOrder.Text
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .EstimatedCost = Me._txtEstimatedCost.Text
                Else
                    If .EstimatedCost <> _txtEstimatedCost.Text Then
                        .EstimatedCost = Me._txtEstimatedCost.Text
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .ActualCost = Me._txtActualCost.Text
                Else
                    If .ActualCost <> _txtActualCost.Text Then
                        .ActualCost = Me._txtActualCost.Text
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
                        If IsNumeric(_ResponsiblePerson.SelectedValue) AndAlso .ClosedDate IsNot Nothing AndAlso .ClosedDate.Length > 0 Then 'User is attempting to close a task that is assigned to a role
                            If IP.Bids.SharedFunctions.GetCurrentUser.IsUserInEmployeeTable = True Then
                                _ResponsiblePerson.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                                _ResponsiblePerson.PlantCode = IP.Bids.SharedFunctions.GetCurrentUser.PlantCode
                                _ResponsiblePerson.PopulateEmployeeList()
                            Else
                                .ClosedDate = String.Empty
                                _rblStatus.ClearSelection()
                                .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
                                _rblStatus.Items(OpenTaskSeqId).Selected = True
                                Session.Add("TaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("An employee has to be selected before a Task can be Completed.", True))
                                isDirty = True
                                afterSaveUrl = "RELOAD"
                            End If
                        End If
                    End If
                    If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
                        If IsNumeric(_ResponsiblePerson.SelectedValue) Then 'Role
                            .ResponsibleRoleSeqId = CInt(_ResponsiblePerson.SelectedValue)
                            .RoleName = _ResponsiblePerson.SelectedText
                            .ResponsibleUserName = String.Empty
                            .ResponsibleName = String.Empty
                            .ResponsibleRolePlantCode = _ResponsiblePerson.PlantCode

                            If .ClosedDate IsNot Nothing AndAlso .ClosedDate.Length > 0 Then
                                'Someone other than a Role Name should be used for the Responsible Person
                                .ClosedDate = String.Empty
                                _rblStatus.ClearSelection()
                                .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
                                _rblStatus.Items(OpenTaskSeqId).Selected = True
                                Session.Add("TaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("An employee has to be selected before a Task can be Completed.", True))

                            End If
                            isDirty = True
                        Else
                            .ResponsibleRoleSeqId = 0
                            .RoleName = String.Empty
                            .ResponsibleRolePlantCode = String.Empty
                            .ResponsibleUserName = _ResponsiblePerson.SelectedValue
                            .ResponsibleName = _ResponsiblePerson.SelectedText
                            isDirty = True
                        End If
                    End If
                Else
                    If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
                        If IsNumeric(_ResponsiblePerson.SelectedValue) AndAlso .ClosedDate.Length > 0 Then 'User is attempting to close a task that is assigned to a role
                            'New - Set the role to the current user
                            If IP.Bids.SharedFunctions.GetCurrentUser.IsUserInEmployeeTable = True Then
                                _ResponsiblePerson.PlantCode = IP.Bids.SharedFunctions.GetCurrentUser.PlantCode
                                _ResponsiblePerson.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                                _ResponsiblePerson.PopulateEmployeeList()
                            Else
                                .ClosedDate = String.Empty
                                _rblStatus.ClearSelection()
                                .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
                                _rblStatus.Items(OpenTaskSeqId).Selected = True
                                Session.Add("TaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("An employee has to be selected before a Task can be Completed.", True))
                                isDirty = True
                                afterSaveUrl = "RELOAD"
                            End If
                        End If
                    Else
                        If .ClosedDate.Length > 0 Then 'User is attempting to close a task that is assigned to a role
                            'New - Set the role to the current user
                            If IP.Bids.SharedFunctions.GetCurrentUser.IsUserInEmployeeTable = True Then
                                _ResponsiblePerson.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                                _ResponsiblePerson.PlantCode = IP.Bids.SharedFunctions.GetCurrentUser.PlantCode
                                _ResponsiblePerson.PopulateEmployeeList()
                            Else
                                .ClosedDate = String.Empty
                                _rblStatus.ClearSelection()
                                .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
                                _rblStatus.Items(OpenTaskSeqId).Selected = True
                                Session.Add("TaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("An employee has to be selected before a Task can be Completed.", True))
                                isDirty = True
                                afterSaveUrl = "RELOAD"
                            End If
                        End If
                    End If
                    If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
                        If IsNumeric(_ResponsiblePerson.SelectedValue) Then 'Role
                            If CInt(_ResponsiblePerson.SelectedValue) <> .ResponsibleRoleSeqId Or .ResponsibleRolePlantCode <> _ResponsiblePerson.PlantCode Then
                                .ResponsibleRoleSeqId = CInt(_ResponsiblePerson.SelectedValue)
                                .RoleName = _ResponsiblePerson.SelectedText
                                .ResponsibleRolePlantCode = _ResponsiblePerson.PlantCode
                                .ResponsibleUserName = String.Empty
                                .ResponsibleName = String.Empty
                                sendEmail = True
                                isDirty = True
                            End If
                            If .ClosedDate.Length > 0 Then
                                'Someone other than a Role Name should be used for the Responsible Person
                                .ClosedDate = String.Empty
                                _rblStatus.ClearSelection()
                                .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
                                _rblStatus.Items(OpenTaskSeqId).Selected = True
                                Session.Add("TaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("An employee has to be selected before a Task can be Completed.", True))
                                isDirty = True
                            End If
                            'isDirty = True

                        Else
                            If .ResponsibleUserName <> _ResponsiblePerson.SelectedValue Then 'AndAlso .ResponsibleRolePlantCode <> _ResponsiblePerson.PlantCode Then ' MJP 8/22/2013 to fix a saving issue
                                .ResponsibleRoleSeqId = 0
                                .RoleName = String.Empty
                                .ResponsibleRolePlantCode = String.Empty
                                .ResponsibleUserName = _ResponsiblePerson.SelectedValue
                                .ResponsibleName = _ResponsiblePerson.SelectedText
                                isDirty = True
                                sendEmail = True
                            End If
                        End If
                    Else
                        'this should be a required field
                    End If
                End If



                If isNewTask Then
                    .TaskHeaderSeqId = CInt(TaskHeaderNumber)
                Else
                    If .TaskHeaderSeqId <> CInt(TaskHeaderNumber) Then
                        .TaskHeaderSeqId = CInt(TaskHeaderNumber)
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .TaskItemSeqId = CInt(TaskItemNumber)
                Else
                    If .TaskItemSeqId <> CInt(TaskItemNumber) Then
                        .TaskItemSeqId = CInt(TaskItemNumber)
                        isDirty = True
                    End If
                End If

                If isNewTask Then
                    .Title = Me._txtTitle.Text
                Else
                    If .Title <> Me._txtTitle.Text Then
                        .Title = Me._txtTitle.Text
                        isDirty = True
                    End If
                End If


                If (RefSite.ToLower = "tanks" Or (IsNumeric(.TankInspectionId) AndAlso CInt(.TankInspectionId) > 0)) Then
                    If TankType = TankTaskTypes.Inspection Then
                        isTankTask = True
                        If isNewTask Then
                            .Title = _ddlInspectionTypeList.SelectedItem.Text

                        Else
                            If .Title <> _ddlInspectionTypeList.SelectedItem.Text Then
                                .Title = _ddlInspectionTypeList.SelectedItem.Text
                                isDirty = True
                            End If
                        End If
                        .TankInspectionId = _ddlInspectionTypeList.SelectedValue
                    End If
                End If

                If isNewTask Then
                    .Comments = Me._txtComments.Text
                Else
                    If .Comments <> Me._txtComments.Text Then
                        .Comments = Me._txtComments.Text
                        isDirty = True
                    End If
                End If
                Me._txtComments.Text = String.Empty

                If isNewTask Then
                    .UpdateFlag = Me._rblUpdateTasks.SelectedValue
                Else
                    If .UpdateFlag <> Me._rblUpdateTasks.SelectedValue Then
                        .UpdateFlag = Me._rblUpdateTasks.SelectedValue
                        isDirty = True
                    End If
                End If
            End With

            If Me._ClosedDate.StartDate.Length > 0 Then
                ReadOnlyMode()
                If newTaskItem.Dependenttaskseqid IsNot Nothing AndAlso newTaskItem.Dependenttaskseqid.Length = 0 Then
                    _btnSaveandShowRecurrence.Enabled = True
                End If
            Else
                EditMode()
            End If

            If isDirty Then
                ' Dim taskTrackerBLL As New TaskTrackerItemBll
                ' Dim subTaskItems As New System.Collections.Generic.List(Of SubTaskItem)
                If Me._ClosedDate.StartDate.Length > 0 Then
                    '    If newTaskItem.RootTaskItemSeqId IsNot Nothing AndAlso IsNumeric(newTaskItem.RootTaskItemSeqId) Then

                    '    End If
                End If

                Dim updatedTaskItem As System.Collections.Generic.List(Of TaskItem) = taskItemBLL.SaveTaskItem(newTaskItem)

                If updatedTaskItem IsNot Nothing AndAlso updatedTaskItem.Count > 0 Then

                    If Me._ClosedDate.StartDate.Length > 0 Then
                        Dim taskTrackerBLL As New TaskTrackerItemBll
                        Dim subTaskItems As New System.Collections.Generic.List(Of SubTaskItem)
                        If newTaskItem.RootTaskItemSeqId.Length > 0 AndAlso IsNumeric(newTaskItem.RootTaskItemSeqId) Then
                            subTaskItems = taskTrackerBLL.GetSubTaskItemList(CInt(newTaskItem.RootTaskItemSeqId), False)
                        End If
                        If subTaskItems IsNot Nothing AndAlso subTaskItems.Count > 0 Then
                            For i As Integer = 0 To subTaskItems.Count - 1
                                'TODO - Need a way to determine if the status was changed from Complete to NWN or Cancel
                                EmailDataBll.GetAndSendSubsequentTasksImmediateEmail(CInt(subTaskItems.Item(i).TaskItemSeqId), CInt(TaskHeaderNumber), IP.Bids.SharedFunctions.CDateFromEnglishDate(subTaskItems.Item(i).DueDate))
                            Next
                        End If
                    End If
                    TaskItemNumber = CStr(updatedTaskItem.Item(0).TaskItemSeqId)
                    If isTankTask And isNewTask Then
                        CreatTanksSubTask(String.Format("{0} - {1} {0} Date", newTaskItem.Title, "Review and Verify next"), newTaskItem.TankInspectionId)
                    End If


                    'go see if an immediate email should be sent.
                    Dim sendEmailUser As String = GeneralTaskTrackerBll.GetUserNotiftyProfile(updatedTaskItem.Item(0).ResponsibleUserName, "IMMEDIATE", "ENTERED", 4)

                    If sendEmailUser = "" Then
                        'might want to send an email about something
                        IP.Bids.SharedFunctions.SendEmail(ConfigurationManager.AppSettings.Item("BBCemail"), "manufacturing.task@graphicpkg.com", "Task added for: " & updatedTaskItem.Item(0).ResponsibleUserName.ToString, "Email was not sent for an added Task: " & updatedTaskItem.Item(0).TaskItemSeqId.ToString)
                    Else
                        If sendEmail And Me._ClosedDate.StartDate.Length = 0 Then
                            EmailDataBll.GetAndSendImmediateEmail(
                                updatedTaskItem.Item(0).TaskItemSeqId,
                                CInt(TaskHeaderNumber))
                        End If

                    End If



                    If afterSaveUrl = "#" Then
                        'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
                    ElseIf afterSaveUrl.ToLower = "subtasks" Then
                        Session.Add("popupwindow", "subtasks")
                        'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
                    ElseIf afterSaveUrl.ToLower = "dependenttasks" Then
                        Session.Add("popupwindow", "dependenttasks")
                    ElseIf afterSaveUrl.ToLower = "editcomments" Then
                        Session.Add("popupwindow", "editcomments")
                    ElseIf afterSaveUrl.ToLower = "recurrence" Then
                        Session.Add("popupwindow", "recurrence")
                        'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
                    ElseIf afterSaveUrl.ToLower = "attachments" Then
                        Session.Add("popupwindow", "attachments")
                        'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
                    ElseIf afterSaveUrl.ToLower = "reload" Then
                        IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId & "&RefSite=" & RefSite)
                        'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    Else
                        IP.Bids.SharedFunctions.ResponseRedirect(afterSaveUrl) '& "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
                        'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    End If

                    'If isNewTask Then
                    IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId & "&RefSite=" & RefSite)
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    'End If
                    Me._lblLastUpdateDate.Text = IP.Bids.SharedFunctions.FormatDate(updatedTaskItem.Item(0).LastUpdateDate) 'updatedTaskItem.Item(0).LastUpdateDate
                    Me._lblLastUpdatedBy.Text = updatedTaskItem.Item(0).LastUpdateUserName
                    updatedTaskItem = Nothing
                    'jeb Me._udpTaskItems.Update()
                End If
            Else
                If afterSaveUrl = "#" Then
                ElseIf afterSaveUrl.ToLower = "subtasks" Then
                    Session.Add("popupwindow", "subtasks")
                ElseIf afterSaveUrl.ToLower = "dependenttasks" Then
                    Session.Add("popupwindow", "dependenttasks")
                ElseIf afterSaveUrl.ToLower = "recurrence" Then
                    Session.Add("popupwindow", "recurrence")
                ElseIf afterSaveUrl.ToLower = "attachments" Then
                    Session.Add("popupwindow", "attachments")
                ElseIf afterSaveUrl.ToLower = "editcomments" Then
                    Session.Add("popupwindow", "editcomments")
                ElseIf afterSaveUrl.ToLower = "reload" Then
                    IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber & "&RefSite=" & RefSite)
                Else
                    IP.Bids.SharedFunctions.ResponseRedirect(afterSaveUrl)
                End If
            End If
        End If
        PopulateComments()
    End Sub

    'Private Sub GetTasksCreatedFromSubTasks(ByVal parentNumber As String)
    '    Dim taskTrackerBLL As New TaskTrackerItemBll
    '    Dim tasklist As System.Collections.Generic.List(Of TaskItem) = taskTrackerBLL.GetTaskItemList(CInt(Me.TaskHeaderNumber))
    '    If tasklist IsNot Nothing AndAlso tasklist.Count > 0 Then
    '        For i As Integer = 0 To tasklist.Count - 1
    '            If tasklist.Item(i).DependentChildSeqid = parentNumber Then
    '                EmailDataBll.GetAndSendImmediateEmail(CInt(tasklist.Item(i).TaskItemSeqId), CInt(TaskHeaderNumber))
    '            End If
    '        Next
    '        tasklist = Nothing
    '        taskTrackerBLL = Nothing
    '    End If
    'End Sub
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.PageName = Master.GetLocalizedValue("Task Details", False) '"Task Details"
        _lblHeaderTitle.Text = Master.PageName
        HandlePageLoad()
    End Sub

    Protected Sub _btnSaveTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSaveTask.Click
        SaveTask()
    End Sub

    Protected Sub _rblStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _rblStatus.SelectedIndexChanged
        Dim taskSearch As New TaskTrackerListsBll
        Dim status As System.Collections.Generic.List(Of TaskStatus) = taskSearch.GetTaskStatus()
        Dim currentStatus As String = String.Empty
        Dim currentStatusID As Integer
        If IsNumeric(Me._rblStatus.SelectedValue) Then
            currentStatusID = CInt(Me._rblStatus.SelectedValue)
        End If
        For Each statusItem As TaskStatus In status
            If statusItem.StatusSeqid = currentStatusID Then
                currentStatus = statusItem.StatusName.ToLower
                Exit For
            End If
        Next

        Select Case currentStatus
            Case "open"
                Me._ClosedDate.StartDate = ""
            Case "complete"
                Me._ClosedDate.StartDate = Now.ToShortDateString
            Case "no work needed"
                Me._ClosedDate.StartDate = Now.ToShortDateString
            Case "cancelled"
                Me._ClosedDate.StartDate = Now.ToShortDateString
        End Select
        SaveTask()
    End Sub


    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Try
            If Me._ClosedDate.StartDate.Length > 0 Then
                ReadOnlyMode()
                _btnSaveandShowRecurrence.Enabled = True
            Else
                EditMode()
            End If

            If IsNumeric(TaskHeaderNumber) = False Then
                Throw New MissingFieldException("Task Header Number needs to be numeric!  Please provide the TaskHeaderNumber property with a value")
                Exit Sub
            End If

            If Me.IsTaskTemplate Then
                Me._ClosedDate.Enabled = False
                Me._rblUpdateTasks.Enabled = True ' False
                Me._gvComments.Enabled = False
                Me._rblStatus.Enabled = False
                Me._txtComments.Enabled = False
                Me._btnAssignReplication.Enabled = True
                _btnAssignReplication.Visible = True
                _btnAssignReplication.Url = Page.ResolveUrl("~/Popups/ReplicationAssignment.aspx?HeaderNumber=" & TaskHeaderNumber)
                _btnAssignReplication.AllowChildToCloseParent = True
                _btnAssignReplication.ReloadPageOnClose = False
                _btnAssignReplication.LoadIFrame()
                Me._ResponsiblePerson.UserMode = User_Controls.AdvancedEmployeeListDropdown.UserModes.RolesOnly ' UserControlsEmployeeList2.UserModes.RolesOnly
                Me._ResponsiblePerson.PlantCode = "9998"
                Me._ResponsiblePerson.AllowFacilityChange = False
                Me._ResponsiblePerson.PopulateEmployeeList()

                Dim taskSite As New TaskTrackerSiteBll
                Dim siteList As System.Collections.Generic.List(Of BusinessRegionSite) = Nothing
                siteList = taskSite.GetBusinessRegionFacility(TaskHeaderNumber)

                If siteList IsNot Nothing AndAlso siteList.Count > 0 Then
                    For i As Integer = 0 To siteList.Count - 1
                        If siteList.Item(i).ProcessedFlag.ToUpper = "Y" Then
                            Me.ReadOnlyMode()
                            Me._btnAddTask.Enabled = False
                            Exit For
                        End If
                    Next
                End If
                taskSite = Nothing
                siteList = Nothing
            End If

            Dim taskHeader = New TaskHeaderBll(CInt(IP.Bids.SharedFunctions.DataClean(TaskHeaderNumber, CStr(0))))
            If taskHeader.CurrentTaskHeaderRecord.ExternalSourceName.ToLower = "tanks" Then
                Dim userHasUpdateAccess As Boolean = GeneralTaskTrackerBll.DoesUserHaveUpdateAccess(IP.Bids.SharedFunctions.GetCurrentUser.Username, taskHeader.CurrentTaskHeaderRecord.PlantCode)
                If userHasUpdateAccess = False Then
                    Me.ReadOnlyMode()
                    Me.RemoveUpdateAccess()
                    Me._btnAddTask.Enabled = False
                End If
            End If

            If Me.HasOpenSubtasks = True Then
                _ClosedDate.Enabled = False
                Me._rblStatus.Enabled = True
                For Each statusItem As ListItem In _rblStatus.Items
                    If statusItem.Selected = False Then
                        statusItem.Enabled = False
                    End If
                Next
                'Me._lblMessageToUser.align = "center"
                Me._lblTaskStatusMessage.Visible = True
                Me._lblTaskStatusMessage.Text = "*** " & IP.Bids.SharedFunctions.LocalizeValue("This task cannot be closed until all of the Sub Tasks have been closed", True) & " ***"
                _lblClosedDateTip.CssClass = "tip"
            Else
                _lblClosedDateTip.CssClass = "hidetip"
                _lblClosedDateTip.Visible = False
                _ClosedDate.Enabled = True
            End If

            If Request.QueryString("popupwindow") IsNot Nothing Then
                Session.Item("popupwindow") = Request.QueryString("popupwindow")

                'Response.Redirect("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber, False)
                IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetTaskDetailsGPIURL(TaskHeaderNumber, RefSite, TaskItemNumber))
                'HttpContext.Current.ApplicationInstance.CompleteRequest()
                Exit Sub
            End If

            If Session.Item("popupwindow") IsNot Nothing OrElse Request.QueryString("popupwindow") IsNot Nothing Then
                Dim show As String = String.Empty
                If Session.Item("popupwindow") IsNot Nothing Then
                    show = CStr(Session.Item("popupwindow"))
                ElseIf Request.QueryString("popupwindow") IsNot Nothing Then
                    show = Request.QueryString("popupwindow")
                End If

                Session.Remove("popupwindow")

                Dim btn As ModalIframePopup
                btn = CType(LoadControl("~\User Controls\ModalIframPopup.ascx"), ModalIframePopup)
                With btn
                    .HideDisplayButton()
                    If show.ToLower = "subtasks" Then
                        .Url = Me._btnSubTasks.Url
                        .BannerText = Me._btnSubTasks.BannerText
                        .Width = Me._btnSubTasks.Width
                        .Height = Me._btnSubTasks.Height
                        .ReloadPageOnClose = Me._btnSubTasks.ReloadPageOnClose
                        .AllowChildToCloseParent = Me._btnSubTasks.AllowChildToCloseParent
                    ElseIf show.ToLower = "dependenttasks" Then
                        .Url = Me._btnDependentTaskItems.Url
                        .BannerText = Me._btnDependentTaskItems.BannerText
                        .Width = Me._btnDependentTaskItems.Width
                        .Height = Me._btnDependentTaskItems.Height
                        .ReloadPageOnClose = Me._btnDependentTaskItems.ReloadPageOnClose
                        .AllowChildToCloseParent = Me._btnDependentTaskItems.AllowChildToCloseParent
                    ElseIf show.ToLower = "editcomments" Then
                        .Url = Page.ResolveClientUrl(IP.Bids.SiteURLs.GetCommentsUrl(TaskItemNumber, TaskHeaderNumber))
                        .BannerText = Me._btnDisplayComments.BannerText
                        .Width = Me._btnDisplayComments.Width
                        .Height = Me._btnDisplayComments.Height
                        .ReloadPageOnClose = Me._btnDisplayComments.ReloadPageOnClose
                        .AllowChildToCloseParent = Me._btnDisplayComments.AllowChildToCloseParent
                    ElseIf show.ToLower = "recurrence" Then
                        .Url = Me._btnRecurrence.Url
                        .BannerText = Me._btnRecurrence.BannerText
                        .Width = Me._btnRecurrence.Width
                        .Height = Me._btnRecurrence.Height
                        .ReloadPageOnClose = Me._btnRecurrence.ReloadPageOnClose
                        .AllowChildToCloseParent = Me._btnRecurrence.AllowChildToCloseParent
                    ElseIf show.ToLower = "attachments" Then
                        .Url = Me._btnAttachments.Url
                        .BannerText = Me._btnAttachments.BannerText
                        .Width = Me._btnAttachments.Width
                        .Height = Me._btnAttachments.Height
                        .ReloadPageOnClose = Me._btnAttachments.ReloadPageOnClose
                        .AllowChildToCloseParent = Me._btnAttachments.AllowChildToCloseParent

                    Else
                        Exit Sub
                    End If

                    '_udpTaskDetails.Controls.Add(btn)
                    _phTaskDetails.Controls.Add(btn)

                    .LoadIFrame()
                    .ShowPopup()
                    .TriggerPopupJS()
                    'mjp_ifrDependentTasks.Attributes.Item("src") = "" 'Page.ResolveUrl("~/Popups/TaskList.aspx?ParentTaskNumber=" & currentTaskItem.Dependenttaskseqid & "&HeaderNumber=" & TaskHeaderNumber & "&ShowHeaderInfo=False&AllowEdit=false")
                    'mjp_ifrDependentTasks.Attributes.Item("onload") = "$('#" & _ifrDependentTasks.ClientID & "').load(function() {$('#" & _ifrDependentTasks.ClientID & "').css('display', 'block'); $('#preload-img2').css('display', 'none');  resizeFrame(document.getElementById('" & Me._ifrDependentTasks.ClientID & "'));});"
                    'jeb Me._ifrTaskItems.Attributes.Item("src") = ""
                    'jeb LoadIframe()
                End With
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PreRender", , ex)
        End Try
    End Sub

    Protected Sub _ClosedDate_DateChanged(ByVal sender As System.Web.UI.WebControls.TextBox) Handles _ClosedDate.DateChanged
        Dim taskstatus As New TaskTrackerListsBll
        Dim status As System.Collections.Generic.List(Of TaskStatus)
        '        Dim imgPath As String = Page.ResolveUrl("~/Images/") ' COMMENTED BY CODEIT.RIGHT

        status = taskstatus.GetTaskStatus
        If Me._rblStatus.SelectedValue = "1" Then
            For Each item As TaskStatus In status
                If item.StatusName.ToLower = "complete" Then
                    If Me._rblStatus.Items.FindByValue(CStr(item.StatusSeqid)) IsNot Nothing Then
                        Me._rblStatus.ClearSelection()
                        Me._rblStatus.Items.FindByValue(CStr(item.StatusSeqid)).Selected = True
                    End If
                End If
            Next
        End If
        SaveTask()

    End Sub

    '    Protected Sub _btnCopyTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnCopyTask.Click
    '        Me.TaskItemNumber = CStr(-1)
    '        Me._txtTitle.Text = "Copy " & Me._txtTitle.Text
    '        Dim taskstatus As New TaskTrackerListsBll
    '        Dim status As System.Collections.Generic.List(Of TaskStatus)
    ''        Dim imgPath As String = Page.ResolveUrl("~/Images/") ' COMMENTED BY CODEIT.RIGHT

    '        status = taskstatus.GetTaskStatus
    '        For Each item As TaskStatus In status
    '            If item.StatusName.ToLower = "open" Then
    '                If Me._rblStatus.Items.FindByValue(CStr(item.StatusSeqid)) IsNot Nothing Then
    '                    Me._rblStatus.ClearSelection()
    '                    Me._rblStatus.Items.FindByValue(CStr(item.StatusSeqid)).Selected = True
    '                End If
    '            End If
    '        Next
    '        Me._ClosedDate.StartDate = ""
    '        Me._txtComments.Text = ""
    '        SaveTask()
    '    End Sub

    Protected Sub _fa_DocumentsLoaded() Handles _fa.DocumentsLoaded
        Me._cpeAttachments.ExpandedText = minusLabel & IP.Bids.SharedFunctions.LocalizeValue("Hide Attachments and Links", True) & GetBadgeLabel(_fa.AttachmentCount.ToString)
        Me._cpeAttachments.CollapsedText = plusLabel & IP.Bids.SharedFunctions.LocalizeValue("Show Attachments and Links", True) & GetBadgeLabel(_fa.AttachmentCount.ToString)
        If _fa.AttachmentCount > 0 Then
            Me._cpeAttachments.Collapsed = False
            Me._cpeAttachments.ClientState = "false"
        Else
            Me._cpeAttachments.Collapsed = True
            Me._cpeAttachments.ClientState = "true"
        End If
    End Sub


    Protected Sub ReturnToTaskList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnReturnToTaskList.Click
        IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetTaskListURL(TaskHeaderNumber, RefSite, False, True, True))
        'HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub ReturnToTaskSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnReturnToTaskSearch.Click
        IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetTaskSearchUrl(True))
        'HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub _btnTaskHeader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnTaskHeader.Click
        'Me.Validate("TaskDetails")
        'If Page.IsValid Then
        '    Me.SaveTask(Page.ResolveClientUrl("~/TaskHeader.aspx?HeaderNumber=" & TaskHeaderNumber))
        'Else
        IP.Bids.SharedFunctions.ResponseRedirect("~/TaskHeader.aspx?HeaderNumber=" & TaskHeaderNumber & "&RefSite=" & RefSite)
        'HttpContext.Current.ApplicationInstance.CompleteRequest()
        'End If
    End Sub

    'Protected Sub _btnTasks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnTasks.Click
    '    Me.SaveTask(Page.ResolveClientUrl("~/ViewTasks.aspx"))
    'End Sub
    Protected Sub _btnSaveAndShowDependentTaskItems_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSaveAndShowDependentTaskItems.Click
        Me.SaveTask("dependenttasks")
    End Sub
    Protected Sub _btnSaveandShowSubTasks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSaveandShowSubTasks.Click
        Me.SaveTask("subtasks")
    End Sub

    Protected Sub _btnSaveandShowAttachments_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSaveandShowAttachments.Click
        Me.SaveTask("attachments")
    End Sub

    Protected Sub _btnSaveandShowRecurrence_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSaveandShowRecurrence.Click
        Me.SaveTask("recurrence")
    End Sub

    Protected Sub _btnAddTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnAddTask.Click
        'SaveTask(Page.ResolveClientUrl(IP.Bids.SiteURLs.GetTaskDetailsGPIURL(TaskHeaderNumber, RefSite, "-1")))

        SaveTaskAdd(Page.ResolveClientUrl(IP.Bids.SiteURLs.GetTaskDetailsGPIURL(TaskHeaderNumber, RefSite, "-1")))
    End Sub

    'Protected Sub _gvDependentTasks_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles _gvDependentTasks.RowCreated
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim lblResponsible As Label = CType(e.Row.FindControl("_lblResponsible"), Label)
    '        If lblResponsible IsNot Nothing Then
    '            Dim SubTask As TaskItem = TryCast(e.Row.DataItem, TaskItem)
    '            If SubTask IsNot Nothing Then
    '                If SubTask.ResponsibleUserName.Length > 0 Then
    '                    lblResponsible.Text = SubTask.ResponsibleName
    '                Else
    '                    lblResponsible.Text = SubTask.ResponsibleRoleSiteName & " - " & SubTask.RoleDescription
    '                End If
    '            End If
    '        End If
    '    End If
    'End Sub

    Private Sub DisplayInspectionTypes()
        Dim list = BusinessObjects.InspectionType.GetAllInspectionTypes
        Dim currentValue As String = _ddlInspectionTypeList.SelectedValue
        If list Is Nothing Then Exit Sub
        _txtTitle.Visible = False
        _rfvTitle.Enabled = False
        With _ddlInspectionTypeList
            .Visible = True
            .ClearSelection()
            .Items.Clear()

            For Each item In list
                .Items.Add(New ListItem(item.Description, CStr(item.InspectionID)))
            Next
            If _ddlInspectionTypeList.Items.FindByValue(currentValue) IsNot Nothing Then
                _ddlInspectionTypeList.Items.FindByValue(currentValue).Selected = True
            End If
        End With
        list = Nothing
    End Sub

    Private Sub CreatTanksSubTask(title As String, ByVal tankInspectionId As String)
        Dim taskTrackerBll As New TaskTrackerItemBll
        Dim item As New SubTaskItem

        Dim taskHeader = New TaskHeaderBll(CInt(IP.Bids.SharedFunctions.DataClean(TaskHeaderNumber, CStr(0))))

        With item
            .CreatedBy = IP.Bids.SharedFunctions.GetCurrentUser.Username
            .CreatedDate = Now.ToShortDateString
            .DaysAfter = 0
            .Description = ""
            .LastUpdateDate = Now.ToShortDateString
            .LastUpdateUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
            .TankInspectionId = tankInspectionId
            Dim responsible As String = "Get Tank Coordinator ID"
            Dim roleName As String = String.Empty
            Dim siteRoleList As System.Collections.Generic.List(Of SiteUserRole) = DataMaintenanceBLL.GetSiteUserRoleList(taskHeader.CurrentTaskHeaderRecord.PlantCode)

            For Each roleItem In siteRoleList
                If roleItem.RoleName.ToLower = "tank coordinator" Then
                    responsible = roleItem.RoleId
                    roleName = roleItem.RoleDescription
                End If
            Next

            If responsible.Length > 0 Then
                If IsNumeric(responsible.Trim) Then
                    .ResponsibleName = String.Empty
                    .ResponsibleRoleSeqId = CInt(responsible.Trim)
                    .ResponsibleUserName = String.Empty
                    .RoleName = roleName
                    .ResponsibleRolePlantCode = taskHeader.CurrentTaskHeaderRecord.PlantCode
                End If
            End If
            .ParentSubTaskSeqID = Me.TaskItemNumber
            .RootTaskItemSeqId = ""
            .TaskHeaderSeqId = Me.TaskHeaderNumber
            .Title = title
        End With

        taskTrackerBll.SaveSubTaskItem(item)
    End Sub

    Protected Sub Dependenttask_TaskDeleteComplete(ByVal parentTaskNumber As String) Handles _dependenttask.TaskDeleteComplete
        IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveClientUrl(IP.Bids.SiteURLs.GetTaskDetailsURL(TaskHeaderNumber, RefSite, TaskItemNumber)))
    End Sub

    Private Sub LoadIframe()

        'jeb _ifrTaskItems.Attributes.Item("onload") = "$('#" & _ifrTaskItems.ClientID & "').css('display', 'block'); $('#preload-img').css('display', 'none');  resizeFrame(document.getElementById('" & Me._ifrTaskItems.ClientID & "'));"
    End Sub

    Private Sub _btnCancelRecurrence_Click(sender As Object, e As EventArgs) Handles _btnCancelRecurrence.Click
        CancelRecurrence()
        IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveClientUrl(IP.Bids.SiteURLs.GetTaskDetailsURL(TaskHeaderNumber, RefSite, TaskItemNumber)))
    End Sub

    Public Sub CancelRecurrence()
        Try
            Dim record As New System.Collections.Generic.List(Of RecurringParameters)
            Dim currentRecord As New System.Collections.Generic.List(Of RecurringParameters)
            Dim userName As String = IP.Bids.SharedFunctions.GetCurrentUser.Username
            Dim taskItemBll As New TaskTrackerItemBll
            Dim recurrenceParameterList As System.Collections.Generic.List(Of RecurringParametersList)
            Dim currentTaskItem = taskItemBll.GetTaskItem(CInt(TaskItemNumber))

            recurrenceParameterList = taskItemBll.GetRecurringParameterList
            Dim isValid As Boolean = True

            'currentpattern.EndByDate = Now.AddDays(-1)
            currentRecord = taskItemBll.GetRecurringParameters(CInt(currentTaskItem.RootTaskItemSeqId))
            Dim endByDateSeqId As Integer
            For Each rec In currentRecord
                If rec.ProfileTypeName <> "Occurrences" AndAlso rec.ProfileTypeName <> "EndByDate" Then
                    record.Add(New RecurringParameters(rec.TaskItemSeqId, rec.ProfileTypeSeqId, rec.ProfileTypeName, rec.ProfileTypeValue, userName, Now))
                End If
            Next
            For Each paramrecord As RecurringParametersList In recurrenceParameterList
                Select Case paramrecord.ProfileTypeName
                    Case "EndByDate"
                        endByDateSeqId = paramrecord.ProfileTypeSeqId
                End Select
            Next
            recurrenceParameterList = Nothing
            record.Add(New RecurringParameters(CInt(currentTaskItem.RootTaskItemSeqId), endByDateSeqId, "EndByDate", IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Now.AddDays(-1)), userName, Now))
            taskItemBll.SaveRecurringParameters(record, currentRecord)
            taskItemBll = Nothing

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("Cancel Recurrence", , ex, "Changes to the Recurrence pattern were not saved due to an unexpected error.")
        End Try
    End Sub

    Private Sub _btnDelete_Click(sender As Object, e As EventArgs) Handles _btnDelete.Click
        Dim taskItemBll As New TaskTrackerItemBll
        Try
            Dim userName As String = IP.Bids.SharedFunctions.GetCurrentUser.Username
            Dim currentTaskItem = taskItemBll.GetTaskItem(CInt(TaskItemNumber))

            taskItemBll.DeleteTaskItem(taskNumber:=CInt(TaskItemNumber), rootTaskNumber:=currentTaskItem.RootTaskItemSeqId, userName:=userName)
            IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetTaskDetailsURL(TaskHeaderNumber, RefSite, "-1"))
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("Cancel Recurrence", , ex, "Changes to the Recurrence pattern were not saved due to an unexpected error.")
        End Try
    End Sub

    Private Sub EditComments_Click(sender As Object, e As EventArgs) Handles _btnEditComments.Click
        Me.SaveTask("editcomments")
    End Sub
End Class

