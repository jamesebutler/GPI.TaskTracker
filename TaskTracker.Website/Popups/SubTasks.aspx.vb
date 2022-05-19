'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 09-24-2010
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class PopupsSubTasks
    Inherits IP.Bids.BasePage
    Private mTaskHeaderNumber As String = String.Empty
    Private mTaskItemNumber As String = String.Empty
    Private mSubTaskNumber As String = String.Empty
    Private IsTaskTemplate As Boolean

    ''' <summary>
    ''' Gets or sets the task header number.
    ''' </summary>
    ''' <value>The task header number.</value>    
    Public Property TaskHeaderNumber() As String
        Get
            Return mTaskHeaderNumber
        End Get
        Set(ByVal value As String)
            mTaskHeaderNumber = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the task item number.
    ''' </summary>
    ''' <value>The task item number.</value>
    Public Property TaskItemNumber() As String
        Get
            Return mTaskItemNumber
        End Get
        Set(ByVal value As String)
            mTaskItemNumber = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Sub task item number.
    ''' </summary>
    ''' <value>The Sub task item number.</value>
    Private Property SubTaskNumber() As String
        Get
            Return mSubTaskNumber
        End Get
        Set(ByVal value As String)
            mSubTaskNumber = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        IP.Bids.SharedFunctions.DisablePageCache(Response)
        If Request.QueryString("HeaderNumber") IsNot Nothing Then
            mTaskHeaderNumber = Request.QueryString("HeaderNumber")
        End If
        If Request.QueryString("TaskNumber") IsNot Nothing Then
            mTaskItemNumber = Request.QueryString("TaskNumber")
        End If
        If Request.QueryString("SubTaskNumber") IsNot Nothing Then
            mSubTaskNumber = Request.QueryString("SubTaskNumber")
        End If
        Dim plantCode As String = String.Empty
        Dim taskHeader As TaskHeaderBll = Nothing
        Try
            taskHeader = New TaskHeaderBll(CInt(TaskHeaderNumber))
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("HandlePageLoad", "Error attempting to create a new instance of TaskHeaderBLL for [" & TaskHeaderNumber & "]", ex)
            IP.Bids.SharedFunctions.ResponseRedirect("~/TaskHeader.aspx")
            'HttpContext.Current.ApplicationInstance.CompleteRequest()
        End Try
        If taskHeader IsNot Nothing AndAlso taskHeader.CurrentTaskHeaderRecord IsNot Nothing Then
            plantCode = taskHeader.CurrentTaskHeaderRecord.PlantCode

            If IsNumeric(taskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID) AndAlso taskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID = IP.Bids.SharedFunctions.SourceSystemID.Template Then 'Template Header
                Me.IsTaskTemplate = True
            End If
            taskHeader = Nothing
        End If
        'If IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults IsNot Nothing Then
        '    If IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults.Item("Facility") IsNot Nothing Then
        '        plantCode = IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults.Item("Facility")
        '    End If
        'End If
        If plantCode.Length > 0 Then
            Me._employeeList.PlantCode = plantCode 'IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults.Item("Facility")
            Me._employeeList.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
            If Me.IsTaskTemplate Then
                Me._employeeList.UserMode = User_Controls.AdvancedEmployeeListDropdown.UserModes.RolesOnly ' UserControlsEmployeeList2.UserModes.RolesOnly
                Me._employeeList.PlantCode = "9998"
                Me._employeeList.AllowFacilityChange = False
                Me._employeeList.DefaultUserName = String.Empty
            End If
            Me._employeeList.PopulateEmployeeList()
        End If
        If Not Page.IsPostBack Then
            GetSubTasks(TaskItemNumber)
        End If
    End Sub

    Private Sub GetSubTasks(ByVal taskItemNumber As Integer, Optional ByVal subTaskItemNumber As Integer = 0)

        Dim taskTrackerBLL As New TaskTrackerItemBll
        Dim subTaskItems As New System.Collections.Generic.List(Of SubTaskItem)
        Try
            With taskTrackerBLL

                subTaskItems = .GetSubTaskItemList(taskItemNumber, True)
            End With
            Me._btnAddSubTask.CommandArgument = -1
            'If subTaskItems IsNot Nothing AndAlso subTaskItems.Count > 0 Then
            Me._gvSubTask.DataSource = subTaskItems
            Me._gvSubTask.DataBind()
            Me._gvSubTask.AutoGenerateColumns = False

            If subTaskItemNumber > 0 Then
                Me._btnAddSubTask.CommandArgument = subTaskItemNumber
                For Each item As SubTaskItem In subTaskItems
                    If item.TaskItemSeqId = subTaskItemNumber Then
                        Me._txtDaysAfterComplete.Text = item.DaysAfter
                        Me._txtDescription.Text = item.Description
                        Me._txtTitle.Text = item.Title
                        If item.ResponsibleRoleSeqId <= 0 Then
                            Me._employeeList.DefaultUserName = item.ResponsibleUserName
                        Else
                            Me._employeeList.DefaultUserName = item.ResponsibleRoleSeqId
                            Me._employeeList.plantcode = item.ResponsibleRolePlantCode
                        End If
                    End If
                Next


            End If
            'End If
        Catch
            Throw
        End Try
        subTaskItems = Nothing
        taskTrackerBLL = Nothing
    End Sub


    Protected Sub _gvSubTask_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles _gvSubTask.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblResponsible As Label = e.Row.FindControl("_lblResponsible")
            If lblResponsible IsNot Nothing Then
                Dim subtask As SubTaskItem = TryCast(e.Row.DataItem, SubTaskItem)
                If subtask IsNot Nothing Then
                    If subtask.ResponsibleUserName.Length > 0 Then
                        lblResponsible.Text = subtask.ResponsibleName
                    Else
                        lblResponsible.Text = IP.Bids.SharedFunctions.LocalizeValue(subtask.ResponsibleRoleSiteName, True) & " - " & IP.Bids.SharedFunctions.LocalizeValue(subtask.RoleDescription, True)
                    End If
                End If
            End If
        End If
    End Sub


    Protected Sub _btnAddSubTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnAddSubTask.Click
        Dim taskTrackerBLL As New TaskTrackerItemBll
        Dim item As New SubTaskItem
        With item
            .CreatedBy = IP.Bids.SharedFunctions.GetCurrentUser.Username
            .CreatedDate = Now.ToShortDateString
            .DaysAfter = Me._txtDaysAfterComplete.Text
            .Description = Me._txtDescription.Text
            .LastUpdateDate = Now.ToShortDateString
            .LastUpdateUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username

            Dim responsible As String = Me._employeeList.SelectedValue

            If responsible.Length > 0 Then
                If IsNumeric(responsible.Trim) Then
                    'User has selected a role
                    .ResponsibleName = String.Empty
                    .ResponsibleRoleSeqId = CInt(responsible.Trim)
                    .ResponsibleUserName = String.Empty
                    .RoleName = Me._employeeList.SelectedText.Trim
                    .ResponsibleRolePlantCode = Me._employeeList.plantcode
                Else
                    .ResponsibleName = IP.Bids.SharedFunctions.LocalizeValue(Me._employeeList.SelectedText.Trim, True)
                    .ResponsibleRoleSeqId = 0
                    .ResponsibleUserName = responsible.Trim
                    .RoleName = String.Empty
                    .ResponsibleRolePlantCode = String.Empty
                End If
            End If
            .ParentSubTaskSeqID = Me.TaskItemNumber
            .RootTaskItemSeqId = ""
            If Me._btnAddSubTask.CommandArgument <> -1 Then
                .TaskItemSeqId = Me._btnAddSubTask.CommandArgument
            Else
                .TaskItemSeqId = ""
            End If

            .TaskHeaderSeqId = Me.TaskHeaderNumber
            .Title = Me._txtTitle.Text
        End With
        Me._txtDaysAfterComplete.Text = 1
        Me._txtDescription.Text = String.Empty
        Me._txtTitle.Text = String.Empty

        taskTrackerBLL.SaveSubTaskItem(item)
        GetSubTasks(item.ParentSubTaskSeqID)
    End Sub

    Public Sub EditSubTask(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As LinkButton = TryCast(sender, LinkButton)
        If btn IsNot Nothing AndAlso IsNumeric(btn.CommandArgument) Then
            GetSubTasks(TaskItemNumber, CInt(btn.CommandArgument))
        End If
    End Sub
    Public Sub DeleteSubTask(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As LinkButton = TryCast(sender, LinkButton)
        Dim taskTrackerBLL As New TaskTrackerItemBll

        If btn IsNot Nothing AndAlso IsNumeric(btn.CommandArgument) Then
            taskTrackerBLL.DeleteSubTaskItem(CInt(btn.CommandArgument))
            GetSubTasks(TaskItemNumber)
        End If

    End Sub

End Class
