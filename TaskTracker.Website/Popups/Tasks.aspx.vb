'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 01-23-2011
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Option Explicit On
Option Strict On
Imports System.Globalization
Partial Class PopupsTasks
    Inherits IP.Bids.BasePage
#Region "Fields"
    Private TaskHeaderNumber As String = String.Empty
    Private TaskItemNumber As String = "-1"
    Private ParentTaskItemNumber As String = "-1"
    Private OpenTaskSeqId As Integer ' = 0
    Private IsTaskTemplate As Boolean
    'Private RefSite As String = String.Empty
#End Region

#Region "Methods"
    Public Sub DisplayMessage(ByVal msg As String)
        Me._msgTaskDetail.Message = msg
        Me._msgTaskDetail.ShowMessage()
    End Sub
    Public Sub HandlePageLoad()

        Dim taskItemBLL As TaskTrackerItemBll
        Dim currentTaskItem As TaskItem
        Dim priorities As System.Collections.Generic.List(Of TaskPriorities)
        Dim taskstatus As TaskTrackerListsBll
        Dim taskHeader As TaskHeaderBll
        Dim parentTaskItem As TaskItem
        Dim maxDaysBefore As Integer = 365
        Dim maxDaysBeforeError As String = IP.Bids.SharedFunctions.LocalizeValue("Days Before Parent should be between", True) & " {0} " & IP.Bids.SharedFunctions.LocalizeValue("and", True) & " {1}"

        Try
            taskItemBLL = New TaskTrackerItemBll
            IP.Bids.SharedFunctions.DisablePageCache(Response)
            If Session("SubTaskDetailMessage") IsNot Nothing Then
                DisplayMessage(CStr(Session("SubTaskDetailMessage")))
                Session.Remove("SubTaskDetailMessage")
            End If
            Dim jsValidateForSave As String = "var canPostBack=CheckForm('TaskDetails'); if (canPostBack) DisplayBusy(); return canPostBack;"
            '_btnSaveTask.OnClientClick = jsValidateForSave

            'Get Header Number
            If Request.QueryString("HeaderNumber") IsNot Nothing AndAlso IsNumeric(Request.QueryString("HeaderNumber")) Then
                TaskHeaderNumber = Request.QueryString("HeaderNumber")
            Else
                DisplayMessage(IP.Bids.SharedFunctions.LocalizeValue("The Header Information is missing for this Sub Task.", True))
                Exit Sub
            End If

            'Get Parent Task Number
            If Request.QueryString("ParentNumber") IsNot Nothing AndAlso IsNumeric(Request.QueryString("ParentNumber")) Then
                ParentTaskItemNumber = Request.QueryString("ParentNumber")
            Else
                DisplayMessage(IP.Bids.SharedFunctions.LocalizeValue("The Parent Task Number Information is missing for this Sub Task.", True))
                Exit Sub
            End If
            Me._ClosedDate.MaxDate = Now

            'Get Task Item Number
            If Request.QueryString("TaskNumber") IsNot Nothing AndAlso IsNumeric(Request.QueryString("TaskNumber")) Then
                TaskItemNumber = Request.QueryString("TaskNumber")
            Else
                IP.Bids.SharedFunctions.ResponseRedirect("~/ViewTasks.aspx")
                'HttpContext.Current.ApplicationInstance.CompleteRequest()
            End If

            'If Header Number is missing then we should send the user back to the Task Search screen
            If TaskHeaderNumber.Length = 0 Or TaskItemNumber.Length = 0 Then
                IP.Bids.SharedFunctions.ResponseRedirect("~/ViewTasks.aspx")
                'HttpContext.Current.ApplicationInstance.CompleteRequest()
            End If

            parentTaskItem = taskItemBLL.GetTaskItem(CInt(Me.ParentTaskItemNumber))
            If parentTaskItem IsNot Nothing Then
                Dim parentDueDate As Date
                If IsDate(parentTaskItem.DueDate) Then
                    parentDueDate = CDate(parentTaskItem.DueDate)
                    If parentDueDate >= Date.Now Then
                        maxDaysBefore = (parentDueDate - Date.Now).Days
                        Me._rvDaysBefore.ErrorMessage = String.Format(maxDaysBeforeError, Me._rvDaysBefore.MinimumValue, maxDaysBefore)
                    Else
                        maxDaysBeforeError = IP.Bids.SharedFunctions.LocalizeValue("The Parent Due Date is currently in the past so the Max Days Before is 0", True)
                        maxDaysBefore = 0
                        Me._rvDaysBefore.ErrorMessage = maxDaysBeforeError
                    End If
                Else
                    Me._rvDaysBefore.ErrorMessage = String.Format(maxDaysBeforeError, Me._rvDaysBefore.MinimumValue, maxDaysBefore)
                End If
            End If
            Me._rvDaysBefore.MaximumValue = CStr(maxDaysBefore)
            Me._rvDaysBefore.Text = "* " & IP.Bids.SharedFunctions.LocalizeValue("Max Days is", True) & String.Format(" ({0})", maxDaysBefore)
            If Not Page.IsPostBack Then
                taskstatus = New TaskTrackerListsBll
                taskHeader = New TaskHeaderBll(CInt(TaskHeaderNumber))

                If taskHeader Is Nothing OrElse taskHeader.CurrentTaskHeaderRecord Is Nothing Then
                    'An invalid Header Number was specified.  The user should be redirected to the View screen
                    IP.Bids.SharedFunctions.ResponseRedirect("~/ViewTasks.aspx")
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    Exit Sub
                End If

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

                Dim status As System.Collections.Generic.List(Of TaskStatus)
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

                'original end of If not Postback
                If taskHeader IsNot Nothing AndAlso taskHeader.CurrentTaskHeaderRecord IsNot Nothing Then
                    'Default the responsible person dropdown to the header facility
                    If Not Page.IsPostBack Then
                        Me._ResponsiblePerson.PlantCode = taskHeader.CurrentTaskHeaderRecord.PlantCode
                        Me._ResponsiblePerson.PopulateEmployeeList()
                    End If
                End If

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
                        TaskItemNumber = CStr(-1)
                    End If
                    IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveClientUrl(String.Format(CultureInfo.CurrentCulture, "~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}&RefSite={2}", TaskHeaderNumber, TaskItemNumber, RefSite)))
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
                ElseIf CInt(TaskItemNumber) > 0 Then 'MJP 6/14/11 AndAlso Page.IsPostBack = False Then
                    'currentTaskItem = taskItemBLL.GetDependentTaskItem(CInt(ParentTaskItemNumber))
                    currentTaskItem = taskItemBLL.GetTaskItem(CInt(TaskItemNumber))
                    If currentTaskItem Is Nothing OrElse currentTaskItem.TaskItemSeqId = 0 Then
                        If Session.Item("RootTaskFor_" & TaskItemNumber) IsNot Nothing Then
                            'Lets redirect the user back to the root task because the task item that was last used has been deleted
                            IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveClientUrl(String.Format(CultureInfo.CurrentCulture, "~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}&RefSite={2}", TaskHeaderNumber, Session.Item("RootTaskFor_" & TaskItemNumber), RefSite)))
                            Session.Remove("RootTaskFor_" & TaskItemNumber)
                            'HttpContext.Current.ApplicationInstance.CompleteRequest()
                        Else
                            Session("SubTaskDetailMessage") = IP.Bids.SharedFunctions.LocalizeValue("An invalid item number was specified.", True) & " " & String.Format("[{0}]", TaskItemNumber)
                            TaskItemNumber = CStr(-1)
                            IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveClientUrl(String.Format(CultureInfo.CurrentCulture, "~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}&RefSite={2}", TaskHeaderNumber, TaskItemNumber, RefSite)))
                            'HttpContext.Current.ApplicationInstance.CompleteRequest()
                        End If
                    End If
                    Me._btnCopyParentTask.Enabled = False
                    PopulateForm(currentTaskItem, taskItemBLL, False)
                    PopulateComments()
                    'ElseIf CInt(TaskItemNumber) <> -1 Then
                Else
                    Me._txtDaysBefore.Text = CStr(0)
                    'Me._EstimatedDueDate.StartDate = Now.ToShortDateString
                    'Me._EstimatedDueDate.StartDate = String.Empty 'Now.ToShortDateString
                    If (Not Me._rblStatus.SelectedValue Is Nothing AndAlso String.IsNullOrEmpty(Me._rblStatus.SelectedValue)) Then
                        Me._rblStatus.SelectedIndex = 0
                    End If
                End If
            End If
            If IP.Bids.SharedFunctions.CausedPostBack(_ClosedDate.UniqueID) Then
                If Request(_ClosedDate.UniqueID & "$_txtDateFrom") IsNot Nothing Then
                    _ClosedDate.StartDate = Request(_ClosedDate.UniqueID & "$_txtDateFrom")
                End If
                _ClosedDate_DateChanged(Nothing)
            End If
            With _Comments
                .taskNumber = TaskItemNumber
                .taskHeaderNumber = TaskHeaderNumber
            End With
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("HandlePageLoad", , ex)
        Finally
            taskItemBLL = Nothing
            currentTaskItem = Nothing
            priorities = Nothing
            taskstatus = Nothing
            taskHeader = Nothing
        End Try
    End Sub

    Public Sub PopulateComments()

        Dim taskItemBLL As New TaskTrackerItemBll
        Dim comments As System.Collections.Generic.List(Of TaskItemComments) = taskItemBLL.GetTaskItemCommentsList(CInt(TaskItemNumber))
        If comments IsNot Nothing AndAlso comments.Count > 0 Then
            'With Me._gvComments
            '    .DataSource = comments
            '    .DataBind()
            'End With
            With _Comments
                .taskNumber = TaskItemNumber
                .taskHeaderNumber = TaskHeaderNumber
                .LoadComments()
            End With
            Me._txtComments.Text = String.Empty
            _cpeComments.CollapsedText = "+ Show Comments (" & comments.Count & ")"
            _cpeComments.ExpandedText = "- Hide Comments (" & comments.Count & ")"
            _cpeComments.CollapsedText = "+ " & IP.Bids.SharedFunctions.LocalizeValue("Show Comments", True) & " (" & comments.Count & ")"
            _cpeComments.ExpandedText = "- " & IP.Bids.SharedFunctions.LocalizeValue("Hide Comments", True) & " (" & comments.Count & ")"
        Else
            _cpeComments.CollapsedText = "+ " & IP.Bids.SharedFunctions.LocalizeValue("Show Comments", True) & " (0)"
            _cpeComments.ExpandedText = "- " & IP.Bids.SharedFunctions.LocalizeValue("Hide Comments", True) & " (0)"
        End If
    End Sub

    Public Sub PopulateForm(ByVal currentTaskItem As TaskItem, ByVal taskItemBLL As TaskTrackerItemBll, ByVal useParentData As Boolean)
        'Todo: Add logic to only show roles for templates
        Try
            If currentTaskItem IsNot Nothing AndAlso currentTaskItem.TaskItemSeqId > 0 Then
                If Not Page.IsPostBack Or useParentData Then
                    If currentTaskItem.ResponsibleRoleSeqId > 0 Then
                        'If currentTaskItem.ResponsibleRolePlantCode.Length > 0 Then
                        Me._ResponsiblePerson.PlantCode = currentTaskItem.ResponsibleRolePlantCode
                        Me._ResponsiblePerson.DefaultUserName = CStr(currentTaskItem.ResponsibleRoleSeqId)
                        Me._ResponsiblePerson.PopulateEmployeeList()
                    Else
                        Me._ResponsiblePerson.DefaultUserName = currentTaskItem.ResponsibleUserName
                        Me._ResponsiblePerson.PopulateEmployeeList()
                    End If
                End If

                If Not Page.IsPostBack Or useParentData Then 'populate the fields from database data
                    With currentTaskItem
                        If useParentData Then
                            Me._lblCreatedBy.Text = String.Empty
                            Me._lblCreatedDate.Text = String.Empty
                            Me._lblLastUpdateDate.Text = String.Empty
                            Me._lblLastUpdatedBy.Text = String.Empty
                            Me._rblStatus.ClearSelection()
                            _rblStatus.SelectedIndex = 0
                            _lblStatus.Text = String.Empty
                            Me._txtDaysBefore.Text = "0"
                            Me._ClosedDate.StartDate = String.Empty
                            Me._txtActualCost.Text = String.Empty
                            Me._txtEstimatedCost.Text = String.Empty
                        Else
                            Me._lblCreatedBy.Text = .CreatedBy
                            Me._lblCreatedDate.Text = IP.Bids.SharedFunctions.FormatDate(CDate(.CreatedDate))
                            Me._lblLastUpdateDate.Text = IP.Bids.SharedFunctions.FormatDate(CDate(.LastUpdateDate))
                            Me._lblLastUpdatedBy.Text = .LastUpdateUserName
                            If Me._rblStatus.Items.FindByValue(CStr(.StatusSeqId)) IsNot Nothing Then
                                Me._rblStatus.ClearSelection()
                                Me._rblStatus.Items.FindByValue(CStr(.StatusSeqId)).Selected = True
                            Else
                                Me._rblStatus.ClearSelection()
                                _rblStatus.SelectedIndex = 0
                            End If
                            _lblStatus.Text = GetTaskStatus(.DueDate, .StatusSeqId, True)
                            Me._txtDaysBefore.Text = CStr(.DaysBefore)
                            Me._ClosedDate.StartDate = .ClosedDate
                            Me._txtEstimatedCost.Text = .EstimatedCost
                            Me._txtActualCost.Text = .ActualCost
                            '_EstimatedDueDate.StartDate = .DueDate
                            'If .DateCritical = "Y" Then
                            '    If taskItemBLL.IsDateCriticalChangeAllowed(TaskItemNumber, IP.Bids.SharedFunctions.GetCurrentUser.Username) Then
                            '        _EstimatedDueDate.DateIsCritical = True
                            '        _EstimatedDueDate.Enabled = True
                            '    Else
                            '        _EstimatedDueDate.DateIsCritical = True
                            '        _EstimatedDueDate.Enabled = False
                            '    End If

                            'Else
                            '    _EstimatedDueDate.DateIsCritical = False
                            '    _EstimatedDueDate.Enabled = True
                            'End If
                        End If

                        Me._txtTitle.Text = .Title
                        Me._txtDescription.Text = .Description
                        Me._txtLeadTime.Text = .LeadTime.ToString
                        Me._txtWorkOrder.Text = .WorkOrder


                        If Me._ddlPriority.Items.FindByValue(.Priority) IsNot Nothing Then
                            Me._ddlPriority.ClearSelection()
                            Me._ddlPriority.Items.FindByValue(.Priority).Selected = True
                        End If

                        If .ResponsibleRoleSeqId > 0 Then 'Use Role
                            Me._ResponsiblePerson.SelectedValue = CStr(.ResponsibleRoleSeqId)
                            If Me._ResponsiblePerson.SelectedValue.Length = 0 Then
                                DisplayMessage(IP.Bids.SharedFunctions.LocalizeValue("The following role", True) & ": " & IP.Bids.SharedFunctions.LocalizeValue(.RoleDescription, True) & IP.Bids.SharedFunctions.LocalizeValue(" does not have an employee assigned to it.  Please select a user from the responsible list.", True))
                            End If
                        Else
                            Me._ResponsiblePerson.SelectedValue = .ResponsibleUserName
                            If Me._ResponsiblePerson.SelectedValue.Length = 0 Then
                                Me._ResponsiblePerson.AddUser(.ResponsibleUserName, .ResponsibleName)
                            End If
                        End If

                    End With
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateForm", "Error while populating the Subtask form.", ex)
        End Try
    End Sub
    Public Function GetTaskStatus(ByVal dueDate As String, ByVal statusID As Integer, ByVal includeLabel As Boolean) As String
        Dim taskSearch As New TaskTrackerListsBll
        Dim imgPath As String = Page.ResolveUrl("~/Images/")
        Dim status As String = String.Empty
        Dim overdueStatusID As Integer = -1
        Dim spacer As String = String.Empty

        If includeLabel Then
            spacer = "&nbsp; - "
        End If

        If CDate(dueDate) < Now And statusID = OpenTaskSeqId Then
            status = taskSearch.GetTaskStatus(overdueStatusID, includeLabel, imgPath) & spacer & IP.Bids.SharedFunctions.FormatDate(CDate(dueDate)) '& " - " & statusName
        Else
            status = taskSearch.GetTaskStatus(statusID, includeLabel, imgPath) & spacer & IP.Bids.SharedFunctions.FormatDate(CDate(dueDate)) '& " - " & statusName
        End If
        taskSearch = Nothing
        Return status
    End Function

    Public Sub ReadOnlyMode()
        Me._btnSaveTask.Enabled = True
        Me._ResponsiblePerson.Enabled = False
        Me._txtTitle.Enabled = False
        Me._ddlPriority.Enabled = False
        Me._txtLeadTime.Enabled = False
        Me._txtDescription.Enabled = False
        Me._txtDaysBefore.Enabled = False
        Me._txtEstimatedCost.Enabled = False
        Me._txtActualCost.Enabled = False
        Me._txtWorkOrder.Enabled = False
        'Me._rblStatus.Enabled = False
        Me._ClosedDate.Enabled = True
        Me._btnSaveTask.Enabled = False
        'Me._btnAddTask.Enabled = False
    End Sub
    Public Sub EditMode()
        Me._btnSaveTask.Enabled = True
        Me._ResponsiblePerson.Enabled = True
        Me._txtEstimatedCost.Enabled = True
        Me._txtActualCost.Enabled = True
        Me._txtTitle.Enabled = True
        Me._ddlPriority.Enabled = True
        Me._txtLeadTime.Enabled = True
        Me._txtWorkOrder.Enabled = True
        Me._txtDescription.Enabled = True
        Me._txtDaysBefore.Enabled = True
        'Me._rblStatus.Enabled = False
        Me._ClosedDate.Enabled = True
        'Me._EstimatedDueDate.Enabled = True
        'Me._rblUpdateTasks.Enabled = True
        Me._btnSaveTask.Enabled = True
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
        Me.UpdateSubTask()
    End Sub
    'Public Sub PopulateComments()
    '    Dim taskItemBLL As New TaskTrackerItemBll
    '    Dim comments As System.Collections.Generic.List(Of TaskItemComments) = taskItemBLL.GetTaskItemCommentsList(CInt(TaskItemNumber))
    '    If comments IsNot Nothing AndAlso comments.Count > 0 Then
    '        With Me._gvComments
    '            .DataSource = comments
    '            .DataBind()
    '        End With
    '        _cpeComments.CollapsedText = "+ Show Comments (" & comments.Count & ")"
    '        _cpeComments.ExpandedText = "- Hide Comments (" & comments.Count & ")"
    '    Else
    '        _cpeComments.CollapsedText = "+ Show Comments (0)"
    '        _cpeComments.ExpandedText = "- Hide Comments (0)"
    '    End If
    'End Sub
    'Public Function GetTaskStatus(ByVal dueDate As String, ByVal statusID As Integer, ByVal includeLabel As Boolean) As String
    '    Dim taskSearch As New TaskTrackerListsBll
    '    Dim imgPath As String = Page.ResolveUrl("~/Images/")
    '    Dim status As String = String.Empty
    '    Dim overdueStatusID As Integer = -1
    '    Dim spacer As String = String.Empty

    '    If includeLabel Then
    '        spacer = "&nbsp; - "
    '    End If

    '    If CDate(dueDate) < Now And statusID = OpenTaskSeqId Then
    '        status = taskSearch.GetTaskStatus(overdueStatusID, includeLabel, imgPath) & spacer & dueDate '& " - " & statusName
    '    Else
    '        status = taskSearch.GetTaskStatus(statusID, includeLabel, imgPath) & spacer & dueDate '& " - " & statusName
    '    End If
    '    taskSearch = Nothing
    '    Return status
    'End Function

    'Public Sub ReadOnlyMode()
    '    Me._btnSaveTask.Enabled = True
    '    Me._ResponsiblePerson.Enabled = False
    '    Me._txtTitle.Enabled = False
    '    Me._ddlPriority.Enabled = False
    '    Me._txtLeadTime.Enabled = False
    '    Me._txtDescription.Enabled = False
    '    'Me._rblStatus.Enabled = False
    '    'Me._ClosedDate.Enabled = False
    '    Me._EstimatedDueDate.Enabled = False
    '    Me._rblUpdateTasks.Enabled = False
    '    Me._rblUpdateTasks.Visible = True
    '    _btnAssignReplication.Visible = False
    '    Me._btnSaveandShowAttachments.Enabled = False
    '    Me._btnSaveandShowRecurrence.Enabled = False
    '    Me._btnSaveandShowSubTasks.Enabled = False
    '    Me._btnSaveTask.Enabled = False
    '    'Me._btnAddTask.Enabled = False
    'End Sub
    'Public Sub EditMode()
    '    Me._btnSaveTask.Enabled = True
    '    Me._ResponsiblePerson.Enabled = True
    '    _btnAssignReplication.Visible = False

    '    Me._txtTitle.Enabled = True
    '    Me._ddlPriority.Enabled = True
    '    Me._txtLeadTime.Enabled = True
    '    Me._txtDescription.Enabled = True
    '    'Me._rblStatus.Enabled = False
    '    Me._ClosedDate.Enabled = True
    '    'Me._EstimatedDueDate.Enabled = True
    '    'Me._rblUpdateTasks.Enabled = True
    '    Me._rblUpdateTasks.Visible = True
    '    Me._btnSaveandShowAttachments.Enabled = True
    '    Me._btnSaveandShowRecurrence.Enabled = True
    '    Me._btnSaveandShowSubTasks.Enabled = True
    '    Me._btnSaveTask.Enabled = True
    '    Me._btnAddTask.Enabled = True
    'End Sub


    'Private Sub GetDependentTasks(ByVal rootTaskItemNumber As Integer)

    '    Dim taskTrackerBLL As New TaskTrackerItemBll
    '    Dim dependentTaskItems As New System.Collections.Generic.List(Of SubTaskItem)
    '    Try
    '        With taskTrackerBLL

    '            dependentTaskItems = .GetSubTaskItemList(rootTaskItemNumber)
    '        End With
    '        ' Me._btnAddSubTask.CommandArgument = -1
    '        'If subTaskItems IsNot Nothing AndAlso subTaskItems.Count > 0 Then
    '        Me._gvDependentTasks.DataSource = dependentTaskItems
    '        Me._gvDependentTasks.DataBind()
    '        Me._gvDependentTasks.AutoGenerateColumns = False
    '        If dependentTaskItems IsNot Nothing AndAlso dependentTaskItems.Count > 0 Then
    '            Me._cpeDependentTasks.CollapsedText = "+ Show Dependent Task Items (" & dependentTaskItems.Count.ToString & ")"
    '            Me._cpeDependentTasks.ExpandedText = "- Hide Dependent Task Items (" & dependentTaskItems.Count.ToString & ")"
    '            Me._cpeDependentTasks.Collapsed = False
    '            Me._cpeDependentTasks.ClientState = "false"
    '        Else
    '            Me._cpeDependentTasks.CollapsedText = "+ Show Dependent Task Items (0)"
    '            Me._cpeDependentTasks.ExpandedText = "- Hide Dependent Task Items (0)"
    '            Me._cpeDependentTasks.Collapsed = True
    '            Me._cpeDependentTasks.ClientState = "true"

    '        End If
    '    Catch
    '        Throw
    '    End Try
    '    dependentTaskItems = Nothing
    '    taskTrackerBLL = Nothing
    'End Sub


    'Protected Sub _gvSubsequentTask_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles _gvSubTask.RowCreated
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim lblResponsible As Label = CType(e.Row.FindControl("_lblResponsible"), Label)
    '        If lblResponsible IsNot Nothing Then
    '            Dim SubsequentTask As SubTaskItem = TryCast(e.Row.DataItem, SubTaskItem)
    '            If SubsequentTask IsNot Nothing Then
    '                If SubsequentTask.ResponsibleUserName.Length > 0 Then
    '                    lblResponsible.Text = SubsequentTask.ResponsibleName
    '                Else
    '                    lblResponsible.Text = SubsequentTask.ResponsibleRoleSiteName & " - " & SubsequentTask.RoleDescription
    '                End If
    '            End If
    '        End If
    '    End If
    'End Sub
    'Private Sub SaveTask(Optional ByVal afterSaveUrl As String = "#")
    '    Dim newTaskItem As New TaskItem()
    '    Dim taskItemBLL As New TaskTrackerItemBll
    '    Dim isDirty As Boolean
    '    Dim isNewTask As Boolean
    '    Dim sendEmail As Boolean
    '    'Dim currentTaskItem As New TaskItem

    '    Me.Validate("TaskDetails")
    '    If Page.IsValid Then
    '        newTaskItem = taskItemBLL.GetTaskItem(CInt(TaskItemNumber))
    '        If newTaskItem Is Nothing Or CInt(TaskItemNumber) = -1 Then
    '            'Updating a Task
    '            newTaskItem = New TaskItem()
    '            isDirty = True
    '            isNewTask = True
    '            newTaskItem.TaskItemSeqId = -1
    '            sendEmail = True
    '        End If

    '        With newTaskItem
    '            If .TaskItemSeqId = -1 Then
    '                .CreatedDate = Now.ToShortDateString
    '                .CreatedBy = Master.CurrentUser.Username
    '                isDirty = True
    '            End If

    '            If isNewTask Then
    '                .ClosedDate = Me._ClosedDate.StartDate
    '            Else
    '                If IsDate(.ClosedDate) AndAlso IsDate(Me._ClosedDate.StartDate) Then
    '                    If CDate(.ClosedDate) <> CDate(Me._ClosedDate.StartDate) Then
    '                        .ClosedDate = Me._ClosedDate.StartDate
    '                        isDirty = True
    '                    End If
    '                ElseIf .ClosedDate <> Me._ClosedDate.StartDate Then
    '                    .ClosedDate = Me._ClosedDate.StartDate
    '                    isDirty = True
    '                End If
    '            End If

    '            If isNewTask Then
    '                If _EstimatedDueDate.DateIsCritical Then
    '                    .DateCritical = "Y"
    '                Else
    '                    .DateCritical = "N"
    '                End If
    '                isDirty = True
    '            Else
    '                If _EstimatedDueDate.DateIsCritical = True And .DateCritical.ToUpper = "Y" Then
    '                    'no change
    '                ElseIf _EstimatedDueDate.DateIsCritical = False And .DateCritical.ToUpper = "N" Then
    '                    'no change
    '                Else
    '                    If _EstimatedDueDate.DateIsCritical Then
    '                        .DateCritical = "Y"
    '                    Else
    '                        .DateCritical = "N"
    '                    End If
    '                    isDirty = True
    '                End If
    '            End If

    '            If isNewTask Then
    '                .Description = _txtDescription.Text
    '                isDirty = True
    '            Else
    '                If .Description <> _txtDescription.Text Then
    '                    .Description = _txtDescription.Text
    '                    isDirty = True
    '                End If
    '            End If

    '            If isNewTask Then
    '                .DueDate = _EstimatedDueDate.StartDate
    '            Else
    '                If .DueDate <> _EstimatedDueDate.StartDate Then
    '                    .DueDate = _EstimatedDueDate.StartDate
    '                    isDirty = True
    '                End If
    '            End If

    '            .LastUpdateDate = Now.ToShortDateString
    '            .LastUpdateUserName = Master.CurrentUser.Username

    '            If isNewTask Then
    '                If IsNumeric(_txtLeadTime.Text) Then
    '                    .LeadTime = CInt(Me._txtLeadTime.Text)
    '                Else
    '                    .LeadTime = 0
    '                End If
    '            Else
    '                If IsNumeric(_txtLeadTime.Text) AndAlso .LeadTime <> CInt(Me._txtLeadTime.Text) Then
    '                    If IsNumeric(_txtLeadTime.Text) Then
    '                        .LeadTime = CInt(Me._txtLeadTime.Text)
    '                    Else
    '                        .LeadTime = 0
    '                    End If
    '                    isDirty = True
    '                ElseIf IsNumeric(_txtLeadTime.Text) AndAlso .LeadTime = CInt(Me._txtLeadTime.Text) Then
    '                    .LeadTime = CInt(Me._txtLeadTime.Text)
    '                    'no change
    '                Else
    '                    .LeadTime = 0
    '                    isDirty = True
    '                End If
    '            End If

    '            If isNewTask Then
    '                .Priority = Me._ddlPriority.SelectedValue
    '            Else
    '                If .Priority <> _ddlPriority.SelectedValue Then
    '                    .Priority = Me._ddlPriority.SelectedValue
    '                    isDirty = True
    '                End If
    '            End If

    '            If isNewTask Then
    '                .StatusSeqId = CInt(Me._rblStatus.SelectedValue)
    '            Else
    '                If .StatusSeqId <> CInt(Me._rblStatus.SelectedValue) Then
    '                    .StatusSeqId = CInt(Me._rblStatus.SelectedValue)
    '                    isDirty = True
    '                End If
    '            End If

    '            If isNewTask Then
    '                If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
    '                    If IsNumeric(_ResponsiblePerson.SelectedValue) AndAlso .ClosedDate.Length > 0 Then 'User is attempting to close a task that is assigned to a role
    '                        If IP.Bids.SharedFunctions.GetCurrentUser.IsUserInEmployeeTable = True Then
    '                            _ResponsiblePerson.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
    '                            _ResponsiblePerson.PopulateEmployeeList()
    '                        Else
    '                            .ClosedDate = String.Empty
    '                            _rblStatus.ClearSelection()
    '                            .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
    '                            _rblStatus.Items(OpenTaskSeqId).Selected = True
    '                            Session.Add("SubTaskDetailMessage", "An employee has to be selected before a Task can be Completed.")
    '                            isDirty = True
    '                            afterSaveUrl = "RELOAD"
    '                        End If
    '                    End If
    '                End If
    '                If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
    '                    If IsNumeric(_ResponsiblePerson.SelectedValue) Then 'Role
    '                        .ResponsibleRoleSeqId = CInt(_ResponsiblePerson.SelectedValue)
    '                        .RoleName = _ResponsiblePerson.SelectedText
    '                        .ResponsibleUserName = String.Empty
    '                        .ResponsibleName = String.Empty
    '                        .ResponsibleRolePlantCode = _ResponsiblePerson.PlantCode

    '                        If .ClosedDate.Length > 0 Then
    '                            'Someone other than a Role Name should be used for the Responsible Person
    '                            .ClosedDate = String.Empty
    '                            _rblStatus.ClearSelection()
    '                            .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
    '                            _rblStatus.Items(OpenTaskSeqId).Selected = True
    '                            Session.Add("SubTaskDetailMessage", "An employee has to be selected before a Task can be Completed.")

    '                        End If
    '                        isDirty = True
    '                    Else
    '                        .ResponsibleRoleSeqId = 0
    '                        .RoleName = String.Empty
    '                        .ResponsibleRolePlantCode = String.Empty
    '                        .ResponsibleUserName = _ResponsiblePerson.SelectedValue
    '                        .ResponsibleName = _ResponsiblePerson.SelectedText
    '                        isDirty = True
    '                    End If
    '                End If
    '            Else
    '                If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
    '                    If IsNumeric(_ResponsiblePerson.SelectedValue) AndAlso .ClosedDate.Length > 0 Then 'User is attempting to close a task that is assigned to a role
    '                        'New - Set the role to the current user
    '                        If IP.Bids.SharedFunctions.GetCurrentUser.IsUserInEmployeeTable = True Then
    '                            _ResponsiblePerson.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
    '                            _ResponsiblePerson.PopulateEmployeeList()
    '                        Else
    '                            .ClosedDate = String.Empty
    '                            _rblStatus.ClearSelection()
    '                            .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
    '                            _rblStatus.Items(OpenTaskSeqId).Selected = True
    '                            Session.Add("SubTaskDetailMessage", "An employee has to be selected before a Task can be Completed.")
    '                            isDirty = True
    '                            afterSaveUrl = "RELOAD"
    '                        End If
    '                    End If
    '                End If
    '                If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
    '                    If IsNumeric(_ResponsiblePerson.SelectedValue) Then 'Role
    '                        If CInt(_ResponsiblePerson.SelectedValue) <> .ResponsibleRoleSeqId Then
    '                            .ResponsibleRoleSeqId = CInt(_ResponsiblePerson.SelectedValue)
    '                            .RoleName = _ResponsiblePerson.SelectedText
    '                            .ResponsibleRolePlantCode = _ResponsiblePerson.PlantCode
    '                            .ResponsibleUserName = String.Empty
    '                            .ResponsibleName = String.Empty
    '                            sendEmail = True
    '                            isDirty = True
    '                        End If
    '                        If .ClosedDate.Length > 0 Then
    '                            'Someone other than a Role Name should be used for the Responsible Person
    '                            .ClosedDate = String.Empty
    '                            _rblStatus.ClearSelection()
    '                            .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
    '                            _rblStatus.Items(OpenTaskSeqId).Selected = True
    '                            Session.Add("SubTaskDetailMessage", "An employee has to be selected before a Task can be Completed.")
    '                            isDirty = True
    '                        End If
    '                        'isDirty = True

    '                    Else
    '                        If .ResponsibleUserName <> _ResponsiblePerson.SelectedValue Then
    '                            .ResponsibleRoleSeqId = 0
    '                            .RoleName = String.Empty
    '                            .ResponsibleRolePlantCode = String.Empty
    '                            .ResponsibleUserName = _ResponsiblePerson.SelectedValue
    '                            .ResponsibleName = _ResponsiblePerson.SelectedText
    '                            isDirty = True
    '                            sendEmail = True
    '                        End If
    '                    End If
    '                Else
    '                    'this should be a required field
    '                End If
    '            End If

    '            .RootTaskItemSeqId = ""

    '            If isNewTask Then
    '                .TaskHeaderSeqId = CInt(TaskHeaderNumber)
    '            Else
    '                If .TaskHeaderSeqId <> CInt(TaskHeaderNumber) Then
    '                    .TaskHeaderSeqId = CInt(TaskHeaderNumber)
    '                    isDirty = True
    '                End If
    '            End If

    '            If isNewTask Then
    '                .TaskItemSeqId = CInt(TaskItemNumber)
    '            Else
    '                If .TaskItemSeqId <> CInt(TaskItemNumber) Then
    '                    .TaskItemSeqId = CInt(TaskItemNumber)
    '                    isDirty = True
    '                End If
    '            End If

    '            If isNewTask Then
    '                .Title = Me._txtTitle.Text
    '            Else
    '                If .Title <> Me._txtTitle.Text Then
    '                    .Title = Me._txtTitle.Text
    '                    isDirty = True
    '                End If
    '            End If

    '            If isNewTask Then
    '                .Comments = Me._txtComments.Text
    '            Else
    '                If .Comments <> Me._txtComments.Text Then
    '                    .Comments = Me._txtComments.Text
    '                    isDirty = True
    '                End If
    '            End If
    '            Me._txtComments.Text = String.Empty

    '            If isNewTask Then
    '                .UpdateFlag = Me._rblUpdateTasks.SelectedValue
    '            Else
    '                If .UpdateFlag <> Me._rblUpdateTasks.SelectedValue Then
    '                    .UpdateFlag = Me._rblUpdateTasks.SelectedValue
    '                    isDirty = True
    '                End If
    '            End If
    '        End With

    '        If isDirty Then
    '            Dim updatedTaskItem As System.Collections.Generic.List(Of TaskItem) = taskItemBLL.SaveTaskItem(newTaskItem)

    '            If updatedTaskItem IsNot Nothing AndAlso updatedTaskItem.Count > 0 Then

    '                If sendEmail Then
    '                    EmailDataBll.GetAndSendImmediateEmail(updatedTaskItem.Item(0).TaskItemSeqId, CInt(TaskHeaderNumber))
    '                End If
    '                If afterSaveUrl = "#" Then
    '                    'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
    '                ElseIf afterSaveUrl.ToLower = "subtasks" Then
    '                    Session.Add("popupwindow", "subtasks")
    '                    'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
    '                ElseIf afterSaveUrl.ToLower = "dependenttasks" Then
    '                    Session.Add("popupwindow", "dependenttasks")
    '                ElseIf afterSaveUrl.ToLower = "recurrence" Then
    '                    Session.Add("popupwindow", "recurrence")
    '                    'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
    '                ElseIf afterSaveUrl.ToLower = "attachments" Then
    '                    Session.Add("popupwindow", "attachments")
    '                    'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
    '                ElseIf afterSaveUrl.ToLower = "reload" Then
    '                   IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId & "&RefSite=" & RefSite)
    '                    HttpContext.Current.ApplicationInstance.CompleteRequest()
    '                Else
    '                   IP.Bids.SharedFunctions.ResponseRedirect(afterSaveUrl) '& "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
    '                    HttpContext.Current.ApplicationInstance.CompleteRequest()
    '                End If

    '                If isNewTask Then
    '                   IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId & "&RefSite=" & RefSite)
    '                    HttpContext.Current.ApplicationInstance.CompleteRequest()
    '                End If
    '                Me._lblLastUpdateDate.Text = updatedTaskItem.Item(0).LastUpdateDate
    '                Me._lblLastUpdatedBy.Text = updatedTaskItem.Item(0).LastUpdateUserName
    '                updatedTaskItem = Nothing
    '                Me._udpTaskItems.Update()
    '            End If
    '        Else
    '            If afterSaveUrl = "#" Then
    '                'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber)
    '            ElseIf afterSaveUrl.ToLower = "subtasks" Then
    '                Session.Add("popupwindow", "subtasks")
    '                'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber)
    '            ElseIf afterSaveUrl.ToLower = "dependenttasks" Then
    '                Session.Add("popupwindow", "dependenttasks")
    '            ElseIf afterSaveUrl.ToLower = "recurrence" Then
    '                Session.Add("popupwindow", "recurrence")
    '                'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber)
    '            ElseIf afterSaveUrl.ToLower = "attachments" Then
    '                Session.Add("popupwindow", "attachments")
    '                'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber)
    '            ElseIf afterSaveUrl.ToLower = "reload" Then
    '               IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber & "&RefSite=" & RefSite)
    '                HttpContext.Current.ApplicationInstance.CompleteRequest()
    '            Else
    '               IP.Bids.SharedFunctions.ResponseRedirect(afterSaveUrl) '& "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
    '                HttpContext.Current.ApplicationInstance.CompleteRequest()
    '            End If
    '        End If
    '    End If
    '    PopulateComments()
    '    'HandlePageLoad()
    'End Sub

    Public Sub CopyParentTask()
        Dim currentTaskItem As TaskItem
        Dim taskItemBLL As New TaskTrackerItemBll
        currentTaskItem = taskItemBLL.GetTaskItem(CInt(Me.ParentTaskItemNumber))
        Me.PopulateForm(currentTaskItem, taskItemBLL, True)
    End Sub

    Private Sub UpdateSubTask()
        Dim parentTaskItem As TaskItem
        Dim taskItemBLL As New TaskTrackerItemBll
        Dim currentTaskItem As TaskItem
        Dim currentUser As IP.Bids.UserInfo = IP.Bids.SharedFunctions.GetCurrentUser
        Dim sendEmail As Boolean
        Dim subTaskWasClosed As Boolean
        Dim isDirty As Boolean

        If Page.IsPostBack = False Then
            Exit Sub
        End If
        parentTaskItem = taskItemBLL.GetTaskItem(CInt(Me.ParentTaskItemNumber))
        currentTaskItem = taskItemBLL.GetTaskItem(CInt(Me.TaskItemNumber))
        If Me.TaskItemNumber = "-1" Then
            'New Sub Task
            'currentTaskItem = New TaskItem
            With currentTaskItem
                .CreatedBy = currentUser.Username
                .CreatedDate = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(Now, "EN-US", "d")
                .DependentChildSeqid = ""
                .Dependenttaskseqid = CStr(parentTaskItem.TaskItemSeqId)
                .DueDate = ""
                .TaskHeaderSeqId = CInt(TaskHeaderNumber)
                .TaskItemSeqId = -1
                .Title = Me._txtTitle.Text
                .ClosedDate = String.Empty
                .EstimatedCost = _txtEstimatedCost.Text
                .ActualCost = _txtActualCost.Text
                sendEmail = True
                isDirty = True
            End With
        Else
            'Existing Task


        End If
        With currentTaskItem
                If .EstimatedCost <> _txtEstimatedCost.Text Then
                    .EstimatedCost = Me._txtEstimatedCost.Text
                    isDirty = True
                End If

            
                If .ActualCost <> _txtActualCost.Text Then
                    .ActualCost = Me._txtActualCost.Text
                    isDirty = True
                End If

            If IsDate(.ClosedDate) AndAlso IsDate(Me._ClosedDate.StartDate) Then
                If CDate(.ClosedDate) <> CDate(Me._ClosedDate.StartDate) Then
                    .ClosedDate = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(Me._ClosedDate.StartDate, "EN-US", "d") ' Me._ClosedDate.StartDate
                    isDirty = True
                    subTaskWasClosed = True
                Else
                    .ClosedDate = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(Me._ClosedDate.StartDate, "EN-US", "d") ' Me._ClosedDate.StartDate
                    subTaskWasClosed = True
                End If
            ElseIf .ClosedDate <> Me._ClosedDate.StartDate Then
                .ClosedDate = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(Me._ClosedDate.StartDate, "EN-US", "d")
                isDirty = True
                subTaskWasClosed = True
            Else
                .ClosedDate = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(Me._ClosedDate.StartDate, "EN-US", "d")
                subTaskWasClosed = True
            End If

            If .Comments <> Me._txtComments.Text Then
                .Comments = Me._txtComments.Text
                isDirty = True
            End If

            If .DateCritical <> parentTaskItem.DateCritical Then
                .DateCritical = parentTaskItem.DateCritical
                isDirty = True
            End If

            If .DaysBefore <> CInt(IP.Bids.SharedFunctions.DataClean(Me._txtDaysBefore.Text, CStr(0))) Then
                .DaysBefore = CInt(IP.Bids.SharedFunctions.DataClean(Me._txtDaysBefore.Text, CStr(0)))
                isDirty = True
            End If

            If .Description <> Me._txtDescription.Text Then
                .Description = Me._txtDescription.Text
                isDirty = True
            End If

            If .Title <> Me._txtTitle.Text Then
                .Title = Me._txtTitle.Text
                isDirty = True
            End If

            .LastUpdateDate = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(Now.ToShortDateString, "EN-US")
            .LastUpdateUserName = currentUser.Username

            If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
                If IsNumeric(_ResponsiblePerson.SelectedValue) AndAlso .ClosedDate.Length > 0 Then 'User is attempting to close a task that is assigned to a role
                    If IP.Bids.SharedFunctions.GetCurrentUser.IsUserInEmployeeTable = True Then
                        _ResponsiblePerson.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                        _ResponsiblePerson.PopulateEmployeeList()
                    Else
                        subTaskWasClosed = False
                        .ClosedDate = String.Empty
                        _rblStatus.ClearSelection()
                        .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
                        _rblStatus.Items(OpenTaskSeqId).Selected = True
                        Session.Add("SubTaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("An employee has to be selected before a Task can be Completed.", True))
                    End If
                End If
            End If
            'If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
            '    If IsNumeric(_ResponsiblePerson.SelectedValue) Then 'Role
            '        .ResponsibleRoleSeqId = CInt(_ResponsiblePerson.SelectedValue)
            '        .RoleName = _ResponsiblePerson.SelectedText
            '        .ResponsibleUserName = String.Empty
            '        .ResponsibleName = String.Empty
            '        .ResponsibleRolePlantCode = _ResponsiblePerson.PlantCode

            '        If .ClosedDate.Length > 0 Then
            '            'Someone other than a Role Name should be used for the Responsible Person
            '            .ClosedDate = String.Empty
            '            _rblStatus.ClearSelection()
            '            .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
            '            _rblStatus.Items(OpenTaskSeqId).Selected = True
            '            Session.Add("SubTaskDetailMessage", "An employee has to be selected before a Task can be Completed.")

            '        End If
            '    Else
            '        .ResponsibleRoleSeqId = 0
            '        .RoleName = String.Empty
            '        .ResponsibleRolePlantCode = String.Empty
            '        .ResponsibleUserName = _ResponsiblePerson.SelectedValue
            '        .ResponsibleName = _ResponsiblePerson.SelectedText
            '    End If
            'End If

            If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
                If IsNumeric(_ResponsiblePerson.SelectedValue) Then 'Role
                    If CInt(_ResponsiblePerson.SelectedValue) <> .ResponsibleRoleSeqId Then
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
                        subTaskWasClosed = False
                        .ClosedDate = String.Empty
                        _rblStatus.ClearSelection()
                        .StatusSeqId = CInt(_rblStatus.Items(OpenTaskSeqId).Value)
                        _rblStatus.Items(OpenTaskSeqId).Selected = True
                        Session.Add("SubTaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("An employee has to be selected before a Task can be Completed.", True))
                        isDirty = True
                        sendEmail = False
                    End If
                    'isDirty = True

                Else
                    If .ResponsibleUserName <> _ResponsiblePerson.SelectedValue Then
                        .ResponsibleRoleSeqId = 0
                        .RoleName = String.Empty
                        .ResponsibleRolePlantCode = String.Empty
                        .ResponsibleUserName = _ResponsiblePerson.SelectedValue
                        .ResponsibleName = _ResponsiblePerson.SelectedText
                        isDirty = True
                        If .ClosedDate.Length > 0 Then
                            sendEmail = False
                        Else
                            sendEmail = True
                        End If

                    End If
                End If
            Else
                'this should be a required field
            End If

            If CDbl(_txtLeadTime.Text) <> .LeadTime Then
                If IsNumeric(_txtLeadTime.Text) Then
                    .LeadTime = CInt(Me._txtLeadTime.Text)
                Else
                    .LeadTime = 0
                End If
                isDirty = True
            End If

            If .Priority <> Me._ddlPriority.SelectedValue Then
                .Priority = Me._ddlPriority.SelectedValue
                isDirty = True
            End If

            If .WorkOrder <> Me._txtWorkOrder.Text Then
                .WorkOrder = Me._txtWorkOrder.Text
                isDirty = True
            End If

            If .StatusSeqId <> CInt(Me._rblStatus.SelectedValue) Then
                .StatusSeqId = CInt(Me._rblStatus.SelectedValue)
                isDirty = True
            End If
            .UpdateFlag = "FUTURE"
        End With
        If Request.QueryString("CloseMe") IsNot Nothing Then
            ScriptManager.RegisterStartupScript(_udpTaskDetails, _udpTaskDetails.GetType, "CloseMe", "try{parent.document.getElementById('" & Request.QueryString("CloseMe") & "').click();}catch(e){alert(e);}", True)
        End If
        If isDirty = True Then
            Dim updatedTaskItem As System.Collections.Generic.List(Of TaskItem) = taskItemBLL.SaveDependedentTaskItem(currentTaskItem, Me.ParentTaskItemNumber, IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(parentTaskItem.DueDate)))
            If sendEmail Then
                EmailDataBll.GetAndSendImmediateEmail(updatedTaskItem.Item(0).TaskItemSeqId, CInt(TaskHeaderNumber))
            End If

            If subTaskWasClosed Then
                'Determine if all parent tasks have been closed
                If ParentHasOpenSubtasks(CInt(Me.ParentTaskItemNumber)) = False Then
                    'send email
                    EmailDataBll.GetAndSendSubtaskClosedEmail(CInt(Me.ParentTaskItemNumber), CInt(TaskHeaderNumber))
                End If
            End If

            If TaskItemNumber = "-1" Then
                Try
                    Session.Add("SubTaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("Your new subtask has been successfully saved. You can now add another subtask or simply close this window.", True))
                    'Response.Redirect(Page.AppRelativeVirtualPath & "?" & Replace(Request.QueryString.ToString, "&TaskNumber=-1", "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId), True)
                    IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?" & Request.QueryString.ToString)
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    Exit Sub
                Catch ex As Threading.ThreadAbortException
                    'do nothing 
                Catch ex As Exception
                    IP.Bids.SharedFunctions.HandleError("UpdateSubTask", "Error redirecting to new Subtask", ex)
                End Try
            Else
                Session.Add("SubTaskDetailMessage", IP.Bids.SharedFunctions.LocalizeValue("Your subtask has been successfully saved. You can now add another subtask or simply close this window.", True))
                'Response.Redirect(Page.AppRelativeVirtualPath & "?" & Replace(Request.QueryString.ToString, "&TaskNumber=-1", "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId), True)
                IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?" & Request.QueryString.ToString)
            End If
        End If
        'PopulateComments()

    End Sub

    Public Function ParentHasOpenSubtasks(ByVal parentTaskNumber As Integer) As Boolean
        Dim taskTrackerBLL As New TaskTrackerItemBll
        Dim dependentTaskItems As New System.Collections.Generic.List(Of TaskItem)
        Dim HasOpenSubtasks As Boolean

        With taskTrackerBLL
            dependentTaskItems = .GetDependentTaskItemList(parentTaskNumber)
        End With

        'Determine if we have any sub tasks that are still open
        For i As Integer = 0 To dependentTaskItems.Count - 1
            If dependentTaskItems(i).ClosedDate.Length = 0 Then
                HasOpenSubtasks = True
            End If
        Next
        Return HasOpenSubtasks
    End Function
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandlePageLoad()
    End Sub

    Protected Sub _btnCopyParentTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnCopyParentTask.Click
        CopyParentTask()
    End Sub

    Protected Sub _btnSaveTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSaveTask.Click
        UpdateSubTask()
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
        Me.UpdateSubTask()
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Me._ClosedDate.StartDate.Length > 0 Then
            ReadOnlyMode()
        Else
            EditMode()
        End If
    End Sub

    Private Sub _Comments_CommentsUpdated() Handles _Comments.CommentsUpdated
        PopulateComments()
        UpdateSubTask()
    End Sub
End Class
