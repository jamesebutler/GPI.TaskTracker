'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 09-07-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-07-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Option Explicit On
Option Strict On
Imports System.Globalization


Partial Class TemplateTaskDetails
    Inherits IP.Bids.BasePage

#Region "Fields"
    Private TaskHeaderNumber As String = String.Empty
    Private TaskItemNumber As String = "-1"
    Private OpenTaskSeqId As Integer ' = 0
    Private IsTaskTemplate As Boolean
    'Private RefSite As String = String.Empty
#End Region

#Region "Methods"
    Public Sub DisplayMessage(ByVal msg As String)
        'Me._msgTaskDetail.Message = msg
        'Me._msgTaskDetail.ShowMessage()
    End Sub
    Public Sub HandlePageLoad()

        Dim taskItemBLL As New TaskTrackerItemBll
        Dim currentTaskItem As TaskItem
        Dim priorities As System.Collections.Generic.List(Of TaskPriorities)
        Dim taskstatus As New TaskTrackerListsBll

        Me._pnlComments.Visible = False
        _lblShowHideComments.Visible = False
        Me._txtComments.Visible = False
        _lblComments.Visible = False

        IP.Bids.SharedFunctions.DisablePageCache(Response)
        If Session("TemplateTaskDetailMessage") IsNot Nothing Then
            DisplayMessage(CStr(Session("TemplateTaskDetailMessage")))
            Session.Remove("TemplateTaskDetailMessage")
        End If
        Dim jsValidateForSave As String = "var canPostBack=CheckForm('TaskDetails'); if (canPostBack) DisplayBusy(); return canPostBack;"
        _btnTaskHeader.OnClientClick = "DisplayBusy();" '"return CheckForm('TaskDetails');"
        _btnSaveTask.OnClientClick = jsValidateForSave '"return CheckForm('TaskDetails');"
        _btnSaveandShowRecurrence.OnClientClick = jsValidateForSave '"var canPostBack=CheckForm('TaskDetails'); if (canPostBack) DisplayBusy(); return canPostBack;"
        _btnSaveandShowAttachments.OnClientClick = jsValidateForSave '"DisplayBusy();return CheckForm('TaskDetails');"

        'Get Header Number
        If Request.QueryString("HeaderNumber") IsNot Nothing AndAlso IsNumeric(Request.QueryString("HeaderNumber")) Then
            TaskHeaderNumber = Request.QueryString("HeaderNumber")
            'Display Task Items for the current Header
            _ifrTaskItems.Attributes.Item("src") = Page.ResolveUrl("~/Popups/TaskList.aspx?HeaderNumber=" & TaskHeaderNumber & "&ShowHeaderInfo=True&InFrame=true&AllowDelete=true")
            '_ifrTaskItems.Attributes.Item("src") = Page.ResolveUrl("~/Popups/TaskList.aspx?HeaderNumber=" & TaskHeaderNumber & "&ShowHeaderInfo=False&AllowDelete=True&InFrame=false&AllowEdit=true")
            _ifrTaskItems.Attributes.Item("onload") = "$('#" & _ifrTaskItems.ClientID & "').load(function() {$('#" & _ifrTaskItems.ClientID & "').css('display', 'block'); $('#preload-img').css('display', 'none');  resizeFrame(document.getElementById('" & Me._ifrTaskItems.ClientID & "'));});"

            _btnAddTask.OnClientClick = jsValidateForSave '"return CheckForm('TaskDetails');"
            Me._fa.TaskHeaderNumber = TaskHeaderNumber
            _fa.SaveAsPath = Server.MapPath("~/Uploads")
        Else
            IP.Bids.SharedFunctions.ResponseRedirect("~/ViewTasks.aspx")
            'HttpContext.Current.ApplicationInstance.CompleteRequest()
        End If

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

        If Not Page.IsPostBack Then
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

            'Dim status As System.Collections.Generic.List(Of TaskStatus)
        End If

        Dim taskHeader As TaskHeaderBll = New TaskHeaderBll(CInt(TaskHeaderNumber))

        'If IsNumeric(taskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID) AndAlso CDbl(taskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID) = IP.Bids.SharedFunctions.SourceSystemID.Template Then 'Template Header
        'Me.IsTaskTemplate = True
        'End If

        If CInt(TaskItemNumber) = 0 Then
            'Select the first task
            Dim taskList As System.Collections.Generic.List(Of TaskItem) = Nothing
            taskList = taskItemBLL.GetRootTaskItemList(CInt(TaskHeaderNumber))
            If taskList IsNot Nothing AndAlso taskList.Count > 0 Then
                TaskItemNumber = CStr(taskList.Item(0).TaskItemSeqId)
            Else
                TaskItemNumber = CStr(-1)
            End If
            IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveClientUrl(String.Format(CultureInfo.CurrentCulture, "~/templateTaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}&RefSite={2}", TaskHeaderNumber, TaskItemNumber, RefSite)))
            'HttpContext.Current.ApplicationInstance.CompleteRequest()

        End If

        If CInt(TaskItemNumber) > 0 Then
            currentTaskItem = taskItemBLL.GetOutageTemplateTaskItem(CInt(TaskItemNumber))
            If currentTaskItem Is Nothing OrElse currentTaskItem.TaskItemSeqId = 0 Then
                If Session.Item("RootTaskFor_" & TaskItemNumber) IsNot Nothing Then
                    'Lets redirect the user back to the root task because the task item that was last used has been deleted
                    IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveClientUrl(String.Format(CultureInfo.CurrentCulture, "~/templateTaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}&RefSite={2}", TaskHeaderNumber, Session.Item("RootTaskFor_" & TaskItemNumber), RefSite)))
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    Session.Remove("RootTaskFor_" & TaskItemNumber)
                End If
                'currenttaskitem = taskitembll.GetRootTaskItemList
            End If
            If currentTaskItem IsNot Nothing AndAlso currentTaskItem.TaskItemSeqId > 0 Then
                If Not Page.IsPostBack Then
                    If currentTaskItem.ResponsibleRolePlantCode.Length > 0 Then
                        Me._ResponsiblePerson.PlantCode = currentTaskItem.ResponsibleRolePlantCode
                        Me._ResponsiblePerson.PopulateEmployeeList()
                    Else
                        Me._ResponsiblePerson.DefaultUserName = currentTaskItem.ResponsibleUserName
                        Me._ResponsiblePerson.PopulateEmployeeList()
                    End If
                End If

                Me._fa.TaskItemNumber = TaskItemNumber
                _btnAttachments.Visible = False
                _btnRecurrence.Visible = False


                If Not Page.IsPostBack Then 'populate the fields from database data
                    With currentTaskItem
                        Me._txtTitle.Text = .Title
                        Me._txtDescription.Text = .Description
                        If .DaysBefore = 0 Or .DaysAfter = 0 Then
                            Me._txtWeeksBefore.Text = "0"
                            Me._rblWeeks.SelectedValue = "Weeks Before"
                        Else
                            If .DaysBefore > 0 Then
                                Me._txtWeeksBefore.Text = (.DaysBefore / 7).ToString
                                Me._rblWeeks.SelectedValue = "Weeks Before"
                            Else
                                Me._txtWeeksBefore.Text = (.DaysAfter / 7).ToString
                                Me._rblWeeks.SelectedValue = "Weeks After"
                            End If
                        End If

                        Me._lblCreatedBy.Text = .CreatedBy
                        Me._lblCreatedDate.Text = .CreatedDate
                        Me._lblLastUpdateDate.Text = .LastUpdateDate
                        Me._lblLastUpdatedBy.Text = .LastUpdateUserName

                        If Me._ddlPriority.Items.FindByValue(.Priority) IsNot Nothing Then
                            Me._ddlPriority.ClearSelection()
                            Me._ddlPriority.Items.FindByValue(.Priority).Selected = True
                        End If


                        If .ResponsibleRoleSeqId > 0 Then 'Use Role
                            Me._ResponsiblePerson.SelectedValue = CStr(.ResponsibleRoleSeqId)
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
                            dr.Item("URL") = "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl(String.Format(CultureInfo.CurrentCulture, "~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}&RefSite={2}", currentTaskItem.TaskHeaderSeqId, task.TaskItemSeqId, RefSite)) & "',false);"
                            dr.Item("TaskItemSeqId") = task.TaskItemSeqId
                            '"JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskHeader.aspx?HeaderNumber=" & TaskHeaderNumber) & "'); return false;"
                            dt.Rows.Add(dr)
                        Next
                        'End With
                        'dt.DefaultView.Sort = "TaskItemSeqId"
                        dl.DataSource = dt.DefaultView
                        dl.DataBind()
                    End If
                    _lblNoRecurringTasks.Visible = False
                    Me._cpeRecurringTasks.CollapsedText = "+ Show Recurring Tasks (" & currentTaskItem.RecurringTasks.Count.ToString & ")"
                    Me._cpeRecurringTasks.ExpandedText = "- Hide Recurring Tasks (" & currentTaskItem.RecurringTasks.Count.ToString & ")"
                    Me._cpeRecurringTasks.Collapsed = True
                    Me._cpeRecurringTasks.ClientState = "true"
                Else
                    Me._cpeRecurringTasks.CollapsedText = "+ Show Recurring Tasks (0)"
                    Me._cpeRecurringTasks.ExpandedText = "- Hide Recurring Tasks (0)"
                    Me._cpeRecurringTasks.Collapsed = True
                    _lblNoRecurringTasks.Visible = True
                End If

                Me._btnRecurrence.Url = Page.ResolveUrl(String.Format(CultureInfo.CurrentCulture, "~/Popups/Recurrence.aspx?RootTaskNumber={0}", currentTaskItem.RootTaskItemSeqId))
                Session.Item("RootTaskFor_" & TaskItemNumber) = currentTaskItem.RootTaskItemSeqId

                _btnAttachments.Url = Page.ResolveUrl("~/Popups/Attachments.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & currentTaskItem.RootTaskItemSeqId) 'TaskItemNumber)

                'PopulateComments()

            End If
        ElseIf CInt(TaskItemNumber) <> -1 Then

            'Response.Redirect("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=-1", False)
            'HttpContext.Current.ApplicationInstance.CompleteRequest()
        Else
            _btnAttachments.Enabled = False
            _btnAttachments.Visible = False
            _btnSaveandShowAttachments.Enabled = True
            _btnSaveandShowAttachments.Visible = True
            _btnSaveandShowRecurrence.Enabled = True
            _btnSaveandShowRecurrence.Visible = True
            Me._btnRecurrence.Enabled = False
            _btnRecurrence.Visible = False
            _lblNoRecurringTasks.Visible = True
            Me._cpeRecurringTasks.Collapsed = True
            'Me._cpeRecurringTasks.ClientState = "false"
            _btnAttachments.Enabled = False

            Me._cpeRecurringTasks.CollapsedText = "+ Show Recurring Tasks (0)"
            Me._cpeRecurringTasks.ExpandedText = "- Hide Recurring Tasks (0)"
            _fa.Visible = False
        End If
    End Sub

    Public Sub PopulateComments()
        Dim taskItemBLL As New TaskTrackerItemBll
        Dim comments As System.Collections.Generic.List(Of TaskItemComments) = taskItemBLL.GetTaskItemCommentsList(CInt(TaskItemNumber))
        If comments IsNot Nothing AndAlso comments.Count > 0 Then
            With Me._gvComments
                .DataSource = comments
                .DataBind()
            End With
            _cpeComments.CollapsedText = "+ Show Comments (" & comments.Count & ")"
            _cpeComments.ExpandedText = "- Hide Comments (" & comments.Count & ")"
        Else
            _cpeComments.CollapsedText = "+ Show Comments (0)"
            _cpeComments.ExpandedText = "- Hide Comments (0)"
        End If
    End Sub
    'Public Sub EditMode()
    '    Me._btnSaveTask.Enabled = True
    '    Me._ResponsiblePerson.Enabled = True

    '    Me._txtTitle.Enabled = True
    '    Me._ddlPriority.Enabled = True
    '    Me._txtWeeksBefore.Enabled = True
    '    Me._txtDescription.Enabled = True
    '    Me._btnSaveandShowAttachments.Enabled = True
    '    Me._btnSaveandShowRecurrence.Enabled = True
    '    Me._btnSaveTask.Enabled = True
    '    Me._btnAddTask.Enabled = True
    'End Sub

    'Private Sub GetSubTasks(ByVal rootTaskItemNumber As Integer)

    '    Dim taskTrackerBLL As New TaskTrackerItemBll
    '    Dim subTaskItems As New System.Collections.Generic.List(Of SubTaskItem)
    '    Try
    '        With taskTrackerBLL

    '            subTaskItems = .GetSubTaskItemList(rootTaskItemNumber)
    '        End With
    '        ' Me._btnAddSubTask.CommandArgument = -1
    '        'If subTaskItems IsNot Nothing AndAlso subTaskItems.Count > 0 Then
    '        Me._gvSubTask.DataSource = subTaskItems
    '        Me._gvSubTask.DataBind()
    '        Me._gvSubTask.AutoGenerateColumns = False
    '        If subTaskItems IsNot Nothing AndAlso subTaskItems.Count > 0 Then
    '            Me._cpeSubTasks.CollapsedText = "+ Show Subsequent Task Items (" & subTaskItems.Count.ToString & ")"
    '            Me._cpeSubTasks.ExpandedText = "- Hide Subsequent Task Items (" & subTaskItems.Count.ToString & ")"
    '            Me._cpeSubTasks.Collapsed = False
    '            Me._cpeSubTasks.ClientState = "false"
    '        Else
    '            Me._cpeSubTasks.CollapsedText = "+ Show Subsequent Task Items (0)"
    '            Me._cpeSubTasks.ExpandedText = "- Hide Subsequent Task Items (0)"
    '            Me._cpeSubTasks.Collapsed = True
    '            Me._cpeSubTasks.ClientState = "true"

    '        End If
    '    Catch
    '        Throw
    '    End Try
    '    subTaskItems = Nothing
    '    taskTrackerBLL = Nothing
    'End Sub


    'Protected Sub _gvSubTask_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles _gvSubTask.RowCreated
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim lblResponsible As Label = CType(e.Row.FindControl("_lblResponsible"), Label)
    '        If lblResponsible IsNot Nothing Then
    '            Dim subtask As SubTaskItem = TryCast(e.Row.DataItem, SubTaskItem)
    '            If subtask IsNot Nothing Then
    '                If subtask.ResponsibleUserName.Length > 0 Then
    '                    lblResponsible.Text = subtask.ResponsibleName
    '                Else
    '                    lblResponsible.Text = subtask.ResponsibleRoleSiteName & " - " & subtask.RoleDescription
    '                End If
    '            End If
    '        End If
    '    End If
    'End Sub

    Private Sub SaveTask(Optional ByVal afterSaveUrl As String = "#")
        Dim newTaskItem As New TaskItem()
        Dim taskItemBLL As New TaskTrackerItemBll
        Dim isDirty As Boolean
        Dim isNewTask As Boolean
        Dim sendEmail As Boolean
        'Dim currentTaskItem As New TaskItem

        Me.Validate("TaskDetails")
        If Page.IsValid Then
            newTaskItem = taskItemBLL.GetOutageTemplateTaskItem(CInt(TaskItemNumber))
            If newTaskItem Is Nothing Or CInt(TaskItemNumber) = -1 Then
                'Updating a Task
                newTaskItem = New TaskItem()
                isDirty = True
                isNewTask = True
                newTaskItem.TaskItemSeqId = -1
                sendEmail = True
            End If

            With newTaskItem
                If .TaskItemSeqId = -1 Then
                    .CreatedDate = Now.ToShortDateString
                    .CreatedBy = Master.CurrentUser.Username
                    isDirty = True
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

                .LastUpdateDate = Now.ToShortDateString
                .LastUpdateUserName = Master.CurrentUser.Username

                'NEED TO CONVERT WEEKS ENTERED INTO DAYS
                ' If Me._txtWeeksBefore.Text = "0" Then
                '.DaysAfter = 999
                ' .DaysBefore = 0
                ' Else
                    If isNewTask Then
                        If Me._rblWeeks.SelectedValue = "Weeks Before" Then
                            If IsNumeric(_txtWeeksBefore.Text) Then
                                .DaysBefore = CInt(Me._txtWeeksBefore.Text) * 7
                                .DaysAfter = 999
                            Else
                                .DaysBefore = 999
                            End If
                        Else
                            If IsNumeric(_txtWeeksBefore.Text) Then
                                .DaysAfter = CInt(Me._txtWeeksBefore.Text) * 7
                                .DaysBefore = 999
                            Else
                                .DaysAfter = 999
                            End If

                        End If
                    Else
                        If Me._rblWeeks.SelectedValue = "Weeks Before" Then
                            If IsNumeric(_txtWeeksBefore.Text) AndAlso .DaysBefore <> CInt(Me._txtWeeksBefore.Text) Then
                                If IsNumeric(_txtWeeksBefore.Text) Then
                                    .DaysBefore = CInt(Me._txtWeeksBefore.Text) * 7
                                    .DaysAfter = 999
                                Else
                                    .DaysBefore = 999
                                End If
                                isDirty = True
                            ElseIf IsNumeric(_txtWeeksBefore.Text) AndAlso .DaysBefore = CInt(Me._txtWeeksBefore.Text) Then
                                .DaysBefore = CInt(Me._txtWeeksBefore.Text) * 7
                                .DaysAfter = 999
                            Else
                                .DaysBefore = 999
                                isDirty = True
                            End If
                        Else
                            If IsNumeric(_txtWeeksBefore.Text) AndAlso .DaysBefore <> CInt(Me._txtWeeksBefore.Text) Then
                                If IsNumeric(_txtWeeksBefore.Text) Then
                                    .DaysAfter = CInt(Me._txtWeeksBefore.Text) * 7
                                    .DaysBefore = 999
                                Else
                                    .DaysAfter = 999
                                End If
                                isDirty = True
                            ElseIf IsNumeric(_txtWeeksBefore.Text) AndAlso .DaysBefore = CInt(Me._txtWeeksBefore.Text) Then
                                .DaysAfter = CInt(Me._txtWeeksBefore.Text) * 7
                                .DaysBefore = 999
                            Else
                                .DaysAfter = 999
                                isDirty = True
                            End If
                        End If

                    End If
                    'End If

                    If isNewTask Then
                        .Priority = Me._ddlPriority.SelectedValue
                    Else
                        If .Priority <> _ddlPriority.SelectedValue Then
                            .Priority = Me._ddlPriority.SelectedValue
                            isDirty = True
                        End If
                    End If

                    If isNewTask Then
                        If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
                            If IsNumeric(_ResponsiblePerson.SelectedValue) Then 'Role
                                .ResponsibleRoleSeqId = CInt(_ResponsiblePerson.SelectedValue)
                                .RoleName = _ResponsiblePerson.SelectedText
                                .ResponsibleUserName = String.Empty
                                .ResponsibleName = String.Empty
                                .ResponsibleRolePlantCode = _ResponsiblePerson.PlantCode

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
                        'If Me._ResponsiblePerson.SelectedValue.Length > 0 Then
                        '    If IsNumeric(_ResponsiblePerson.SelectedValue) AndAlso .ClosedDate.Length > 0 Then 'User is attempting to close a task that is assigned to a role
                        '        'New - Set the role to the current user
                        '        If IP.Bids.SharedFunctions.GetCurrentUser.IsUserInEmployeeTable = True Then
                        '            _ResponsiblePerson.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                        '            _ResponsiblePerson.PopulateEmployeeList()
                        '        End If
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
                            End If
                        Else
                            'this should be a required field
                        End If
                    End If

                    .RootTaskItemSeqId = ""

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

                    If isNewTask Then
                        .Comments = Me._txtComments.Text
                    Else
                        If .Comments <> Me._txtComments.Text Then
                            .Comments = Me._txtComments.Text
                            isDirty = True
                        End If
                    End If
                    Me._txtComments.Text = String.Empty

            End With

            If isDirty Then
                Dim updatedTaskItem As System.Collections.Generic.List(Of TaskItem) = taskItemBLL.SaveOutageTemplateTaskItem(newTaskItem)

                If updatedTaskItem IsNot Nothing AndAlso updatedTaskItem.Count > 0 Then

                    'If sendEmail Then
                    'EmailDataBll.GetAndSendImmediateEmail(updatedTaskItem.Item(0).TaskItemSeqId, CInt(TaskHeaderNumber))
                    'End If
                    If afterSaveUrl = "#" Then
                        IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
                        'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    ElseIf afterSaveUrl.ToLower = "subtasks" Then
                        Session.Add("popupwindow", "subtasks")
                        'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
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

                    If isNewTask Then
                        IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId & "&RefSite=" & RefSite)
                        'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    End If
                    Me._lblLastUpdateDate.Text = updatedTaskItem.Item(0).LastUpdateDate
                    Me._lblLastUpdatedBy.Text = updatedTaskItem.Item(0).LastUpdateUserName
                    updatedTaskItem = Nothing
                    Me._udpTemplateTaskItems.Update()
                End If
            Else
                If afterSaveUrl = "#" Then
                    'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber)
                ElseIf afterSaveUrl.ToLower = "recurrence" Then
                    Session.Add("popupwindow", "recurrence")
                    'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber)
                ElseIf afterSaveUrl.ToLower = "attachments" Then
                    Session.Add("popupwindow", "attachments")
                    'Response.Redirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber)
                ElseIf afterSaveUrl.ToLower = "reload" Then
                    IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath & "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskItemNumber & "&RefSite=" & RefSite)
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
                Else
                    IP.Bids.SharedFunctions.ResponseRedirect(afterSaveUrl) '& "?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & updatedTaskItem.Item(0).TaskItemSeqId)
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
                End If
            End If
        End If
        'PopulateComments()
        'HandlePageLoad()
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.PageName = Master.GetLocalizedValue("Outage Template Task Details", False)
        HandlePageLoad()
    End Sub

    Protected Sub _btnSaveTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSaveTask.Click
        SaveTask()
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ' we would do something here if we only want certain people updating the templates.

        'Me._gvComments.Enabled = False
        'Me._gvComments.Visible = False
        'Me._txtComments.Enabled = False
        'Me._txtComments.Visible = False

        'Show only the roles in the responsible drop down list
        Me._ResponsiblePerson.UserMode = User_Controls.AdvancedEmployeeListDropdown.UserModes.RolesOnly 'UserControlsEmployeeList2.UserModes.RolesOnly
        Me._ResponsiblePerson.PlantCode = "9998"
        Me._ResponsiblePerson.AllowFacilityChange = False
        Me._ResponsiblePerson.PopulateEmployeeList()

        Dim taskSite As New TaskTrackerSiteBll
        Dim siteList As System.Collections.Generic.List(Of BusinessRegionSite) = Nothing
        siteList = taskSite.GetBusinessRegionFacility(TaskHeaderNumber)

        'If siteList IsNot Nothing AndAlso siteList.Count > 0 Then
        '    For i As Integer = 0 To siteList.Count - 1
        '        If siteList.Item(i).ProcessedFlag.ToUpper = "Y" Then
        '            Me.ReadOnlyMode()
        '            Exit For
        '        End If
        '    Next
        'End If
        taskSite = Nothing
        siteList = Nothing
        'End If

        If Session.Item("popupwindow") IsNot Nothing Then
            Dim show As String = CStr(Session.Item("popupwindow"))

            Session.Remove("popupwindow")

            Dim btn As ModalIframePopup
            btn = CType(LoadControl("~\User Controls\ModalIframPopup.ascx"), ModalIframePopup)
            With btn
                .HideDisplayButton()
                If show.ToLower = "recurrence" Then
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
            End With
        End If
    End Sub

    Protected Sub _fa_DocumentsLoaded() Handles _fa.DocumentsLoaded
        Me._cpeAttachments.ExpandedText = "- Hide Attachments and Links (" & _fa.AttachmentCount.ToString & ")"
        Me._cpeAttachments.CollapsedText = "+ Show Attachments and Links (" & _fa.AttachmentCount.ToString & ")"
        If _fa.AttachmentCount > 0 Then
            Me._cpeAttachments.Collapsed = False
            Me._cpeAttachments.ClientState = "false"
        Else
            Me._cpeAttachments.Collapsed = True
            Me._cpeAttachments.ClientState = "true"
        End If
    End Sub


    Protected Sub _btnTaskHeader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnTaskHeader.Click
        Me.Validate("TemplateTaskDetails")
        If Page.IsValid Then
            ''''we will save the task once i get the code updated for daysbefore and daysafter.
            IP.Bids.SharedFunctions.ResponseRedirect("~/TaskHeader.aspx?HeaderNumber=" & TaskHeaderNumber)
            'Me.SaveTask(Page.ResolveClientUrl("~/TaskHeader.aspx?HeaderNumber=" & TaskHeaderNumber))
        Else
            IP.Bids.SharedFunctions.ResponseRedirect("~/TaskHeader.aspx?HeaderNumber=" & TaskHeaderNumber)
            'HttpContext.Current.ApplicationInstance.CompleteRequest()
        End If
    End Sub

    Protected Sub _btnSaveandShowAttachments_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSaveandShowAttachments.Click
        Me.SaveTask("attachments")
    End Sub

    Protected Sub _btnSaveandShowRecurrence_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSaveandShowRecurrence.Click
        Me.SaveTask("recurrence")
    End Sub

    Protected Sub _btnAddTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnAddTask.Click
        SaveTask(Page.ResolveClientUrl("~/TemplateTaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=-1"))
    End Sub

End Class
