'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 04-21-2011
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports System.Linq
Partial Class UserControlsTasksTaskListing
    Inherits System.Web.UI.UserControl

#Region "Private Fields"
    Private _MinimumItems As Integer = 1 'Minimum number of tasks to display
    Private _AllowEdit As Boolean = True
    Private _AllowDelete As Boolean = False
    Private _TaskHeaderNumber As String = String.Empty
    Private _ParentTaskNumber As String = String.Empty
    Private _TaskNumber As String = String.Empty
    Private _InFrame As Boolean ' = False
    Private _DisplaySimpleList As Boolean   'Default is false
    Private TaskHeader As TaskHeaderBll 'Holds an instance of the Task Header Business Logic Layer
    Private _updateFlag As UpdateFlagValues = UpdateFlagValues.Current
    Private refSite As String = String.Empty
    Private _showCloseTasks As Boolean = True
    Private _showInspectionTypes As Boolean = False
    Private InspectionTypeId As String = ""
#End Region

#Region "Custom Events"
    Public Event DeleteTask(ByVal taskItemNumber As String)
    Public Event TaskLoadComplete(ByVal taskCount As Integer)
    Public Event TaskDeleteComplete(ByVal parentTaskNumber As String)
#End Region

#Region "Public Enum"
    Public Enum UpdateFlagValues
        Current
        Future
    End Enum
#End Region

#Region "Public Properties"


    Public Property ShowInspectionTypes As Boolean
        Get
            Return _showInspectionTypes
        End Get
        Set(value As Boolean)
            _showInspectionTypes = value
        End Set
    End Property
    Public Property AllowTasksToBeFiltered As Boolean
        Get
            Return _cbShowClosedTasks.Visible
        End Get
        Set(value As Boolean)
            _cbShowClosedTasks.Visible = value
        End Set
    End Property

    Public Property UpdateFlag() As UpdateFlagValues
        Get
            Return _updateFlag
        End Get
        Set(ByVal value As UpdateFlagValues)
            _updateFlag = value
        End Set
    End Property

    Public Property AllowDelete() As Boolean
        Get
            Return _AllowDelete
        End Get
        Set(ByVal value As Boolean)
            _AllowDelete = value
        End Set
    End Property

    Public Property AllowEdit() As Boolean
        Get
            Return _AllowEdit
        End Get
        Set(ByVal value As Boolean)
            _AllowEdit = value
        End Set
    End Property

    Public Property InFrame() As Boolean
        Get
            Return _InFrame
        End Get
        Set(ByVal value As Boolean)
            _InFrame = value
        End Set
    End Property

    Public Property TaskHeaderNumber() As String
        Get
            Return _TaskHeaderNumber
        End Get
        Set(ByVal value As String)
            _TaskHeaderNumber = value
        End Set
    End Property

    Public Property ParentTaskNumber() As String
        Get
            Return _ParentTaskNumber
        End Get
        Set(ByVal value As String)
            _ParentTaskNumber = value
        End Set
    End Property

    Public Property TaskNumber() As String
        Get
            Return _TaskNumber
        End Get
        Set(ByVal value As String)
            _TaskNumber = value
        End Set
    End Property

    Public Property MinimumItems() As Integer
        Get
            Return _MinimumItems
        End Get
        Set(ByVal value As Integer)
            _MinimumItems = value
        End Set
    End Property

    Public Property DisplaySimpleList() As Boolean
        Get
            Return _DisplaySimpleList
        End Get
        Set(ByVal value As Boolean)
            _DisplaySimpleList = value
        End Set
    End Property

    Public Property ShowClosedTasks() As Boolean
        Get
            Return _showCloseTasks
        End Get
        Set(value As Boolean)
            _showCloseTasks = value
        End Set
    End Property
#End Region

#Region "Public Methods"

    Private Sub DisplayInspectionTypes()
        Dim list = BusinessObjects.InspectionType.GetAllInspectionTypes
        If list Is Nothing Then Exit Sub
        _lblInspectionTypes.Visible = True
        With _ddlInspectionTypeList
            .Visible = True
            .ClearSelection()
            .Items.Clear()
            .AutoPostBack = True
            For Each item In list
                .Items.Add(New ListItem(item.Description, CStr(item.InspectionID)))
            Next
            .Items.Insert(0, New ListItem("All", "0"))
            If .Items.FindByValue(InspectionTypeId) IsNot Nothing Then
                .Items.FindByValue(InspectionTypeId).Selected = True
            End If
        End With
        list = Nothing
    End Sub

    ''' <summary>
    ''' Loads the Task Items in a displayable list
    ''' </summary>
    ''' <returns>Returns the TaskItem Count</returns>
    ''' <remarks></remarks>
    Public Function LoadTaskItems() As Integer
        Dim taskItem As New TaskTrackerItemBll
        Dim tasklist As System.Collections.Generic.List(Of TaskItem)
        Dim taskCount As Integer = 0

        Try
            SetShowAllTasksProperty()
            SetInspectionTypeProperty()

            refSite = String.Empty
            If Request.QueryString("RefSite") IsNot Nothing Then
                refSite = Request.QueryString("RefSite")
            End If

            Me._mifDependentTasks.HideDisplayButton()
            If TaskHeaderNumber.Length = 0 Or IsNumeric(TaskHeaderNumber) = False Then
                Return 0
            End If

            _pnlTaskItemsHeading.Text = IP.Bids.SharedFunctions.LocalizeValue("Task Items for Header", True) & " # [" & TaskHeaderNumber & "]"
            TaskHeader = New TaskHeaderBll(CInt(TaskHeaderNumber))
            If TaskHeader IsNot Nothing AndAlso TaskHeader.CurrentTaskHeaderRecord IsNot Nothing Then
                If TaskHeader.CurrentTaskHeaderRecord.LastReplicatedDate.Length > 0 Then 'Header has been replicated
                    _pnlTaskItemsHeading.Text = IP.Bids.SharedFunctions.LocalizeValue("Template Task Items for Header", True) & " # [" & TaskHeaderNumber & "], " & IP.Bids.SharedFunctions.LocalizeValue("Last Replicated on", True) & " " & TaskHeader.CurrentTaskHeaderRecord.LastReplicatedDate
                End If

                If Me.DisplaySimpleList = False Then
                    'TaskHeader = Nothing
                    If IsNumeric(TaskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID) AndAlso TaskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID = IP.Bids.SharedFunctions.SourceSystemID.OutageTemplate Then
                        tasklist = taskItem.GetOutageTemplateTaskItemList(CInt(TaskHeaderNumber))
                        _pnlTaskItemsHeading.Text = IP.Bids.SharedFunctions.LocalizeValue("Outage Task Items", True)
                    Else
                        If Me.ParentTaskNumber.Length = 0 Then
                            tasklist = taskItem.GetTaskItemList(CInt(TaskHeaderNumber))
                        Else
                            tasklist = taskItem.GetDependentTaskItemList(CInt(ParentTaskNumber))
                            _pnlTaskItemsHeading.Text = IP.Bids.SharedFunctions.LocalizeValue("Sub Task Items", True)

                            Me._mifDependentTasks.Url = Page.ResolveUrl("~/Popups/Tasks.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskNumber & "&ParentNumber=" & ParentTaskNumber)
                        End If
                    End If

                    Dim originalCount As Integer
                    If tasklist IsNot Nothing Then originalCount = tasklist.Count

                    Dim selectedTask As IEnumerable(Of TaskItem)
                    If ShowClosedTasks Then
                        selectedTask = From singleTask In tasklist.AsQueryable
                                       Select New TaskItem(singleTask.ClosedDate, singleTask.CreatedBy, singleTask.CreatedDate, singleTask.DateCritical,
                                            singleTask.Description, singleTask.DueDate, singleTask.LastUpdateDate, singleTask.LastUpdateUserName,
                                            singleTask.LeadTime, singleTask.Priority, singleTask.ResponsibleName, singleTask.ResponsibleRoleSeqId, singleTask.ResponsibleUserName,
                                            singleTask.RoleName, singleTask.RootTaskItemSeqId, singleTask.StatusSeqId, singleTask.TaskHeaderSeqId, singleTask.TaskItemSeqId, singleTask.Title,
                                            singleTask.ResponsibleRolePlantCode, singleTask.RoleDescription, singleTask.ResponsibleRoleSiteName, singleTask.RecurringTasks, singleTask.DaysBefore,
                                            singleTask.Dependenttaskseqid, singleTask.DependentChildSeqid, singleTask.DaysAfter, singleTask.WorkOrder, singleTask.SortOrder, singleTask.EstimatedCost, singleTask.ActualCost, singleTask.TankInspectionId, singleTask.OriginalRoleSeqId, singleTask.CreatedByUserName)
                    Else

                        selectedTask = From singleTask In tasklist.AsQueryable
                                       Where singleTask.ClosedDate.Length = 0 Or singleTask.RecurringTasks IsNot Nothing
                                       Select New TaskItem(singleTask.ClosedDate, singleTask.CreatedBy, singleTask.CreatedDate, singleTask.DateCritical,
                                            singleTask.Description, singleTask.DueDate, singleTask.LastUpdateDate, singleTask.LastUpdateUserName,
                                            singleTask.LeadTime, singleTask.Priority, singleTask.ResponsibleName, singleTask.ResponsibleRoleSeqId, singleTask.ResponsibleUserName,
                                            singleTask.RoleName, singleTask.RootTaskItemSeqId, singleTask.StatusSeqId, singleTask.TaskHeaderSeqId, singleTask.TaskItemSeqId, singleTask.Title,
                                            singleTask.ResponsibleRolePlantCode, singleTask.RoleDescription, singleTask.ResponsibleRoleSiteName, singleTask.RecurringTasks, singleTask.DaysBefore,
                                            singleTask.Dependenttaskseqid, singleTask.DependentChildSeqid, singleTask.DaysAfter, singleTask.WorkOrder, singleTask.SortOrder, singleTask.EstimatedCost, singleTask.ActualCost, singleTask.TankInspectionId, singleTask.OriginalRoleSeqId, singleTask.CreatedByUserName)

                    End If

                    If IsNumeric(InspectionTypeId) AndAlso CInt(InspectionTypeId) > 0 Then
                        selectedTask = From item In selectedTask.AsQueryable
                                       Where item.TankInspectionId = InspectionTypeId
                                       Select item
                    End If

                    Dim newCount As Integer = 0
                    If selectedTask IsNot Nothing Then
                        newCount = selectedTask.Count
                    End If
                    If ShowClosedTasks Then
                        _cbShowClosedTasks.Text = String.Format(IP.Bids.SharedFunctions.LocalizeValue("Click to show All task (Open and Closed) ") & " ({0}/{1})", newCount, originalCount)
                    Else
                        _cbShowClosedTasks.Text = String.Format(IP.Bids.SharedFunctions.LocalizeValue("Click to show All task (Open and Closed) ") & " ({0}/{1})", selectedTask.Count, originalCount)
                    End If

                    Me._rpTasks.DataSource = selectedTask.OrderBy(Function(obj) CDate(obj.DueDate)).ThenBy(Function(obj) obj.Title).ToList()   'tasklist
                    Me._rpTasks.DataBind()

                    If _rpTasks.Items.Count >= MinimumItems Then
                        _rpTasks.Visible = True
                        _pnlTaskItems.Visible = True
                    Else
                        If ShowClosedTasks = True Then
                            _rpTasks.Visible = False
                            _pnlTaskItems.Visible = False
                        Else
                            _rpTasks.Visible = True
                            _pnlTaskItems.Visible = True
                        End If
                    End If
                    taskCount = _rpTasks.Items.Count
                    If taskCount = 0 And ShowClosedTasks = False Then
                        taskCount = 1
                    End If
                    _btnAddNewTask.Enabled = False
                    _btnAddNewTask.Visible = False
                    '_btnEditTasks.Enabled = False
                    '_btnEditTasks.Visible = False
                Else
                    Dim taskSearch As New TaskListingBll
                    taskSearch.HeaderSeqID = TaskHeaderNumber
                    Me._btnAddNewTask.OnClientClick = GetTaskDetailUrl(-1) & ";return false"
                    Me._btnAddNewTask.Enabled = True
                    Me._btnAddNewTask.Visible = True
                    '_btnEditTasks.Enabled = False
                    '_btnEditTasks.Visible = False
                    '_btnEditTasks.OnClientClick = GetBulkTaskEditsUrl() & ";return false"
                    'listing.Item(0).ItemTitle

                    Dim listing = taskSearch.GetTaskListing(True)
                    Dim selectedTask As IQueryable(Of TaskListing)
                    If ShowClosedTasks Then
                        selectedTask = From singleTask In listing.AsQueryable
                                       Order By singleTask.DueDate
                                       Select New TaskListing(singleTask.ActivityName, singleTask.ActivitySeqID, singleTask.AttachmentCount, singleTask.BusinessUnitArea, singleTask.Division, singleTask.DueDate, singleTask.EndDate, singleTask.ExternalRef, singleTask.ExternalSourceID, singleTask.ExternalSourceName, singleTask.HeaderCreateDate, singleTask.HeaderCreatedBy, singleTask.HeaderDesc, singleTask.HeaderLastUpdateDate, singleTask.HeaderLastUpdateUserName, singleTask.HeaderTitle, singleTask.ItemCreatedDate, singleTask.ItemDesc, singleTask.ItemLastUpdateDate, singleTask.ItemTitle, singleTask.Line, singleTask.PlantCode, singleTask.Region, singleTask.SecurityLevel, singleTask.SiteName, singleTask.StartDate, singleTask.StatusName, singleTask.StatusSeqId, singleTask.TaskHeaderSeqId, singleTask.TaskItemSeqId, singleTask.TaskType, singleTask.HeaderStatus, singleTask.RoleDescription, singleTask.ResponsibleName, singleTask.RoleSiteName, singleTask.ClosedDate, singleTask.DateCritical, singleTask.OpenSubTasks, singleTask.TankInspectionTypeId, singleTask.Priority, singleTask.Comments, singleTask.ParentDueDate, singleTask.ParentSubTaskSeqId, singleTask.DependentTaskSeqId)
                    Else

                        selectedTask = From singleTask In listing.AsQueryable
                                       Where singleTask.StatusSeqId = 1
                                       Order By singleTask.DueDate
                                       Select New TaskListing(singleTask.ActivityName, singleTask.ActivitySeqID, singleTask.AttachmentCount, singleTask.BusinessUnitArea, singleTask.Division, singleTask.DueDate, singleTask.EndDate, singleTask.ExternalRef, singleTask.ExternalSourceID, singleTask.ExternalSourceName, singleTask.HeaderCreateDate, singleTask.HeaderCreatedBy, singleTask.HeaderDesc, singleTask.HeaderLastUpdateDate, singleTask.HeaderLastUpdateUserName, singleTask.HeaderTitle, singleTask.ItemCreatedDate, singleTask.ItemDesc, singleTask.ItemLastUpdateDate, singleTask.ItemTitle, singleTask.Line, singleTask.PlantCode, singleTask.Region, singleTask.SecurityLevel, singleTask.SiteName, singleTask.StartDate, singleTask.StatusName, singleTask.StatusSeqId, singleTask.TaskHeaderSeqId, singleTask.TaskItemSeqId, singleTask.TaskType, singleTask.HeaderStatus, singleTask.RoleDescription, singleTask.ResponsibleName, singleTask.RoleSiteName, singleTask.ClosedDate, singleTask.DateCritical, singleTask.OpenSubTasks, singleTask.TankInspectionTypeId, singleTask.Priority, singleTask.Comments, singleTask.ParentDueDate, singleTask.ParentSubTaskSeqId, singleTask.DependentTaskSeqId)

                    End If

                    With _gvSimpleList
                        _gvSimpleList.DataSource = selectedTask
                        _gvSimpleList.DataBind()
                        .UseAccessibleHeader = True
                        If _gvSimpleList.Rows.Count > 0 Then
                            .HeaderRow.TableSection = TableRowSection.TableHeader
                        End If
                        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "SortGrid_" & .ClientID, String.Format(System.Globalization.CultureInfo.CurrentCulture, "$(function(){{$('#{0}').tablesorter(); }});", .ClientID), True)

                        'If _gvSimpleList.Rows.Count >= MinimumItems Then
                        _gvSimpleList.Visible = True
                        _pnlTaskItems.Visible = True
                        'Else
                        '_gvSimpleList.Visible = False
                        '_pnlTaskItems.Visible = False
                        'End If
                    End With
                    taskCount = _gvSimpleList.Rows.Count
                    If ShowClosedTasks Then
                        _cbShowClosedTasks.Text = String.Format(IP.Bids.SharedFunctions.LocalizeValue("Unclick to only show Open Tasks") & " ({0}/{1})", taskCount, listing.Count)
                    Else
                        _cbShowClosedTasks.Text = String.Format(IP.Bids.SharedFunctions.LocalizeValue("Click to show Open & Closed Tasks") & " ({0}/{1})", taskCount, listing.Count)
                    End If
                End If

            Else
                'Header record doesn't exist
                'Container page will have to handle this case
                taskCount = -1
            End If
        Catch
            Throw
        Finally
            tasklist = Nothing
            taskItem = Nothing
        End Try
        ' Me._cbShowClosedTasks.Visible = Me.Visible
        Return taskCount
    End Function

    Private Function FilterRecurring(ByVal tasks As Generic.List(Of BO.RecurringTasks)) As IQueryable(Of RecurringTasks)
        Dim filtered = From item In tasks.AsQueryable
                       Where item.StatusSeqId = 1
                       Select New BO.RecurringTasks(item.DueDate, item.TaskItemSeqId, item.RootTaskItemSeqId, item.StatusSeqId)
        Return filtered
    End Function

    ''' <summary>
    ''' Returns a url to the Task Details page for the specified Task Item Number
    ''' </summary>
    ''' <param name="taskItemNumber">String - Task Item Number</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTaskDetailUrl(ByVal taskItemNumber As String) As String
        If InFrame = False Then
            If taskItemNumber.Length > 0 And AllowEdit = True Then
                Return "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItemNumber & "&refsite=" & refSite) & "',false);"
            Else
                Return "#"
            End If
        Else
            If taskItemNumber.Length > 0 And AllowEdit = True Then
                Return "Javascript:try{window.parent.DisplayBusy();window.location='../blank.htm';window.parent.location='" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItemNumber & "&refsite=" & refSite) & "'}catch(e){" & "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItemNumber & "&refsite=" & refSite) & "',false);" & "};"
                'Return "Javascript:window.parent.DisplayBusy();window.location='../blank.htm';window.parent.location='" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItemNumber & "&refsite=" & refSite) & "';"
            Else
                Return "#"
            End If
        End If
    End Function

    Public Function GetBulkTaskEditsUrl() As String
        If InFrame = False Then
            If TaskHeaderNumber.Length > 0 And AllowEdit = True Then
                Return "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl("~/Popups/BulkEditTasks.aspx?HeaderNumber=" & TaskHeaderNumber & "&refsite=" & refSite) & "',false);"
            Else
                Return "#"
            End If
        Else
            If TaskHeaderNumber.Length > 0 And AllowEdit = True Then
                Return "Javascript:try{window.parent.DisplayBusy();window.location='../blank.htm';window.parent.location='" & Page.ResolveClientUrl("~/Popups/BulkEditTasks.aspx?HeaderNumber=" & TaskHeaderNumber & "&refsite=" & refSite) & "'}catch(e){" & "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & TaskHeaderNumber & "&refsite=" & refSite) & "',false);" & "};"
                'Return "Javascript:window.parent.DisplayBusy();window.location='../blank.htm';window.parent.location='" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItemNumber & "&refsite=" & refSite) & "';"
            Else
                Return "#"
            End If
        End If
    End Function

    ''' <summary>
    ''' Returns a url to the Task Details page for the specified Task Item Number
    ''' </summary>
    ''' <param name="taskItemNumber">String - Task Item Number</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetOutageTaskDetailUrl(ByVal taskItemNumber As String) As String

        If InFrame = False Then
            If taskItemNumber.Length > 0 And AllowEdit = True Then
                'TODO: verify the name of OutageTaskDetails.aspx page
                Return "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl("~/templatetaskdetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItemNumber & "&refsite=" & refSite) & "',false);"
            Else
                Return "#"
            End If
        Else
            If taskItemNumber.Length > 0 And AllowEdit = True Then
                Return "Javascript:window.parent.DisplayBusy();window.location='../blank.htm';window.parent.location='" & Page.ResolveClientUrl("~/templatetaskdetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItemNumber & "&refsite=" & refSite) & "';"
            Else
                Return "#"
            End If
        End If
    End Function

    ''' <summary>
    ''' Returns a url to the Task Details page for the specified Task Item Number
    ''' </summary>
    ''' <param name="taskItemNumber">String - Task Item Number</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDependentTaskUrl(ByVal subTaskItemNumber As String, ByVal taskItemNumber As String) As String

        If InFrame = False Then
            If taskItemNumber.Length > 0 And AllowEdit = True Then
                'Page.ResolveUrl("~/Popups/Tasks.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItem.Dependenttaskseqid)
                Return "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItemNumber & "&SubTaskNumber=" & subTaskItemNumber & "&popupwindow=dependenttasks&refsite=" & refSite) & "',false);"
            Else
                Return "#"
            End If
        Else
            If taskItemNumber.Length > 0 And AllowEdit = True Then
                'Return "Javascript:window.parent.DisplayBusy();window.location='../blank.htm';window.parent.location='" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItemNumber & "&SubTaskNumber=" & subTaskItemNumber & "&popupwindow=dependenttasks") & "';"
                Return Me._mifDependentTasks.TriggerPopupJS(Page.ResolveUrl("~/Popups/Tasks.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & subTaskItemNumber & "&ParentNumber=" & ParentTaskNumber & "&refsite=" & refSite)) & ";return false;"
            Else
                Return "#"
            End If
        End If
    End Function
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me._mifDependentTasks.HideDisplayButton()
    End Sub

    ''' <summary>
    ''' Deletes the Selected Task
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub DeleteTask_Clicked(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim taskItemNumber As String = String.Empty
        Try
            Dim btnDelete As LinkButton = TryCast(sender, LinkButton)
            If btnDelete IsNot Nothing AndAlso btnDelete.CommandArgument.ToString.Length > 0 Then
                taskItemNumber = btnDelete.CommandArgument.ToString
                Dim taskItem As New TaskTrackerItemBll
                Dim update As String
                'Checking sourceseq to call different delete routine for outage templates.
                If IsNumeric(TaskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID) AndAlso TaskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID = IP.Bids.SharedFunctions.SourceSystemID.OutageTemplate Then
                    taskItem.DeleteSubTaskItem(taskItemNumber)
                Else
                    If UpdateFlag = UpdateFlagValues.Future Then
                        update = "FUTURE"
                    Else
                        update = "CURRENT"
                    End If
                    taskItem.DeleteDependentTaskItem(taskItemNumber, update)
                End If
                LoadTaskItems()
                RaiseEvent TaskDeleteComplete(Me.ParentTaskNumber)
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("DeleteSelectedTask", "Error while attempting to delete the subtask [" & taskItemNumber & "]", ex)
        End Try
    End Sub

    Protected Sub _rpTasks_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles _rpTasks.ItemCreated
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim rb As DataList = CType(e.Item.FindControl("_rbRecurringTasks"), DataList)
            Dim taskItem As IP.MEAS.BO.TaskItem = CType(e.Item.DataItem, IP.MEAS.BO.TaskItem)
            Dim taskItemBLL As New TaskTrackerItemBll
            If taskItem IsNot Nothing And taskItemBLL IsNot Nothing Then 'AndAlso taskItem.RootTaskItemSeqId = "-1" Then
                If rb IsNot Nothing Then
                    rb.DataSource = taskItem.RecurringTasks
                    If AllowEdit Then
                        rb.Enabled = True
                    Else
                        rb.Enabled = False

                    End If
                    rb.DataBind()
                End If
                'Display Recurring Tasks
                Dim thrRecurringTask As TableHeaderRow = TryCast(e.Item.FindControl("_thrRecurringTask"), TableHeaderRow)
                If thrRecurringTask IsNot Nothing And rb.Items.Count > 0 Then
                    thrRecurringTask.Visible = True
                ElseIf thrRecurringTask IsNot Nothing Then
                    thrRecurringTask.Visible = False
                End If
                Dim trRecurringTask As TableRow = TryCast(e.Item.FindControl("_trRecurringTask"), TableRow)
                If trRecurringTask IsNot Nothing And rb.Items.Count > 0 Then
                    trRecurringTask.Visible = True
                ElseIf trRecurringTask IsNot Nothing Then
                    trRecurringTask.Visible = False
                End If
                Dim rbl As RadioButtonList = CType(e.Item.FindControl("_rblStatus"), RadioButtonList)
                Dim taskSearch As New TaskSearchBll
                If rbl IsNot Nothing Then
                    With rbl
                        .Items.Clear()
                        .RepeatColumns = 6
                        .Width = Unit.Percentage(90)
                        .RepeatDirection = RepeatDirection.Horizontal
                        Dim taskStatus As Collection = taskSearch.GetTaskStatusList(True, IP.Bids.SiteURLs.GetTaskStatusImageUrl)
                        If taskStatus IsNot Nothing AndAlso taskStatus.Count > 0 Then
                            For i As Integer = 1 To taskStatus.Count
                                .Items.Add(taskStatus.Item(i).ToString)
                            Next
                            .DataBind()
                        End If
                    End With
                End If

                Dim lblRecurringTasks As Label = CType(e.Item.FindControl("_lblRecurringTasks"), Label)
                If lblRecurringTasks IsNot Nothing AndAlso rb IsNot Nothing Then
                    lblRecurringTasks.Text = IP.Bids.SharedFunctions.LocalizeValue("Recurring Tasks", True) & " (" & rb.Items.Count & ")"
                End If
                Dim priorities As System.Collections.Generic.List(Of TaskPriorities)

                priorities = taskItemBLL.GetPriorityList
                If priorities IsNot Nothing AndAlso priorities.Count > 0 Then
                    Dim lblTaskPriority As Label = CType(e.Item.FindControl("_lblTaskPriority"), Label)
                    If lblTaskPriority IsNot Nothing Then
                        For Each item As TaskPriorities In priorities
                            If IsNumeric(taskItem.Priority) Then
                                If item.PriorityID = CDbl(taskItem.Priority) Then
                                    'lblTaskPriority.Text = IP.Bids.SharedFunctions.LocalizeValue(item.PriorityName, True)
                                    lblTaskPriority.Text = item.PriorityName
                                End If
                            Else
                                If item.PriorityID = CDbl(1) Then
                                    'lblTaskPriority.Text = IP.Bids.SharedFunctions.LocalizeValue(item.PriorityName, True)
                                    lblTaskPriority.Text = item.PriorityName
                                End If
                            End If
                        Next
                    End If
                End If
                'End With
                Dim lblTaskDueDate As Label = CType(e.Item.FindControl("_lblTaskDueDate"), Label)
                Dim lblTaskDueDateLabel As Label = CType(e.Item.FindControl("_lblDueDate"), Label)

                If lblTaskDueDate IsNot Nothing AndAlso lblTaskDueDateLabel IsNot Nothing Then
                    If taskItem.DueDate.Length > 0 Then
                        lblTaskDueDate.Text = IP.Bids.SharedFunctions.FormatDate(CDate(taskItem.DueDate)) 'IP.Bids.SharedFunctions.FormatDate(taskItem.DueDate)
                        lblTaskDueDateLabel.Text = IP.Bids.SharedFunctions.LocalizeValue("Due Date", True)
                    ElseIf taskItem.DaysBefore >= 0 Then
                        lblTaskDueDate.Text = taskItem.DaysBefore
                        lblTaskDueDateLabel.Text = IP.Bids.SharedFunctions.LocalizeValue("Days Before", True)
                    ElseIf taskItem.DaysAfter >= 0 Then
                        lblTaskDueDate.Text = taskItem.DaysAfter
                        lblTaskDueDateLabel.Text = IP.Bids.SharedFunctions.LocalizeValue("Days After", True)
                    End If
                End If
                Dim lblTaskResponsiblePerson As Label = CType(e.Item.FindControl("_lblTaskResponsiblePerson"), Label)
                If lblTaskResponsiblePerson IsNot Nothing Then
                    If taskItem.ResponsibleRoleSeqId > 0 Then
                        lblTaskResponsiblePerson.Text = taskItem.ResponsibleRoleSiteName & " - " & IP.Bids.SharedFunctions.LocalizeValue(taskItem.RoleDescription, True) 'IP.Bids.SharedFunctions.LocalizeValue(taskItem.ResponsibleRoleSiteName, True) & " - " & IP.Bids.SharedFunctions.LocalizeValue(taskItem.RoleDescription, True)
                    Else
                        lblTaskResponsiblePerson.Text = taskItem.ResponsibleName
                    End If
                End If
                Dim lblTaskClosedDate As Label = CType(e.Item.FindControl("_lblTaskClosedDate"), Label)
                If lblTaskClosedDate IsNot Nothing AndAlso taskItem.ClosedDate.Length > 0 Then
                    lblTaskClosedDate.Text = IP.Bids.SharedFunctions.FormatDate(CDate(taskItem.ClosedDate)) 'IP.Bids.SharedFunctions.FormatDate(taskItem.ClosedDate)
                End If

                Dim lblTaskLeadTime As Label = CType(e.Item.FindControl("_lblTaskLeadTime"), Label)
                If lblTaskLeadTime IsNot Nothing Then
                    lblTaskLeadTime.Text = CStr(taskItem.LeadTime)
                End If

                Dim lblTaskTitle As Label = CType(e.Item.FindControl("_lblTaskTitle"), Label)
                If lblTaskTitle IsNot Nothing Then
                    lblTaskTitle.Text = taskItem.Title
                End If

                Dim lblTaskDescription As Label = CType(e.Item.FindControl("_lblTaskDescription"), Label)
                If lblTaskDescription IsNot Nothing Then
                    lblTaskDescription.Text = taskItem.Description
                End If

                Dim lblEstimatedCost As Label = CType(e.Item.FindControl("_lblEstimatedCost"), Label)
                If lblEstimatedCost IsNot Nothing Then lblEstimatedCost.Text = taskItem.EstimatedCost
                Dim lblActualCost As Label = CType(e.Item.FindControl("_lblActualCost"), Label)
                If lblActualCost IsNot Nothing Then lblActualCost.Text = taskItem.ActualCost

                Dim lblTaskWorkOrder As Label = CType(e.Item.FindControl("_lblTaskWorkOrder"), Label)
                If lblTaskWorkOrder IsNot Nothing Then
                    lblTaskWorkOrder.Text = taskItem.WorkOrder
                End If

                Dim _lblTaskStatus As Label = CType(e.Item.FindControl("_lblTaskStatus"), Label)
                If _lblTaskStatus IsNot Nothing Then
                    If taskItem.StatusSeqId = 1 Then
                        'Open Task
                        If taskItem.DueDate.Length > 0 Then
                            If taskItem.StatusSeqId = 1 And CDate(taskItem.DueDate) < Now Then
                                'Open and Past Due
                                _lblTaskStatus.Text = Global.TaskSearchBll.GetTaskStatus(0, True, , IP.Bids.SiteURLs.GetTaskStatusImageUrl)
                            Else
                                _lblTaskStatus.Text = Global.TaskSearchBll.GetTaskStatus(taskItem.StatusSeqId, True, , IP.Bids.SiteURLs.GetTaskStatusImageUrl)
                            End If
                        Else
                            _lblTaskStatus.Text = Global.TaskSearchBll.GetTaskStatus(taskItem.StatusSeqId, True, , IP.Bids.SiteURLs.GetTaskStatusImageUrl)
                        End If
                    Else
                        _lblTaskStatus.Text = Global.TaskSearchBll.GetTaskStatus(taskItem.StatusSeqId, True, , IP.Bids.SiteURLs.GetTaskStatusImageUrl)
                    End If
                End If


                If IsNumeric(TaskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID) AndAlso TaskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID = IP.Bids.SharedFunctions.SourceSystemID.OutageTemplate Then
                    lblTaskClosedDate.Visible = False
                    If taskItem.DaysBefore >= 0 Then
                        lblTaskDueDateLabel.Text = IP.Bids.SharedFunctions.LocalizeValue("Weeks Before", True)
                        lblTaskDueDate.Text = taskItem.DaysBefore / 7
                    ElseIf taskItem.DaysAfter >= 0 Then
                        lblTaskDueDateLabel.Text = IP.Bids.SharedFunctions.LocalizeValue("Weeks After", True)
                        lblTaskDueDate.Text = taskItem.DaysAfter / 7
                    End If
                End If

                '_btnTaskItems.OnClientClick = "JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & taskItem.TaskHeaderSeqId & "&TaskNumber=" & taskItem.TaskItemSeqId) & "'); return false;"
                Dim btnEdit As LinkButton = CType(e.Item.FindControl("_btnEdit"), LinkButton)
                If btnEdit IsNot Nothing Then
                    If AllowEdit Then
                        With btnEdit
                            If IsNumeric(TaskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID) AndAlso TaskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID = IP.Bids.SharedFunctions.SourceSystemID.OutageTemplate Then
                                .OnClientClick = GetOutageTaskDetailUrl(taskItem.TaskItemSeqId) & ";return false" '"JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & taskItem.TaskHeaderSeqId & "&TaskNumber=" & taskItem.TaskItemSeqId) & "',false); return false;"
                            ElseIf taskItem.Dependenttaskseqid.Length > 0 And taskItem.Dependenttaskseqid.ToString <> CStr(taskItem.TaskItemSeqId) Then
                                .OnClientClick = GetDependentTaskUrl(taskItem.TaskItemSeqId, taskItem.Dependenttaskseqid) & ";return false" '"JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & taskItem.TaskHeaderSeqId & "&TaskNumber=" & taskItem.TaskItemSeqId) & "',false); return false;"
                            Else
                                .OnClientClick = GetTaskDetailUrl(taskItem.TaskItemSeqId) & ";return false" '"JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & taskItem.TaskHeaderSeqId & "&TaskNumber=" & taskItem.TaskItemSeqId) & "',false); return false;"
                            End If

                        End With
                        btnEdit.Visible = True
                    Else
                        btnEdit.Visible = False
                    End If
                End If


                Dim btnDelete As LinkButton = CType(e.Item.FindControl("_btnDelete"), LinkButton)
                If btnDelete IsNot Nothing Then
                    With btnDelete
                        .CommandArgument = taskItem.TaskItemSeqId
                        AddHandler btnDelete.Click, AddressOf DeleteTask_Clicked
                    End With
                    If AllowDelete And taskItem.ClosedDate.Length = 0 Then
                        btnDelete.Visible = True
                    Else
                        btnDelete.Visible = False
                    End If
                End If
                taskItemBLL = Nothing
                taskItem = Nothing
            Else

            End If
        Else
        End If

    End Sub

    Protected Sub _gvSimpleList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles _gvSimpleList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim taskItem As IP.MEAS.BO.TaskListing = CType(e.Row.DataItem, IP.MEAS.BO.TaskListing)
            Dim lblTaskDueDate As Label = CType(e.Row.FindControl("_lblTaskDueDate"), Label)
            Dim lblStatus As Label = CType(e.Row.FindControl("_lblStatus"), Label)
            Dim btnEdit As Button = CType(e.Row.FindControl("_btnEdit"), Button)
            'Dim lblTaskDueDateLabel As Label = CType(e.Row.FindControl("_lblDueDate"), Label)

            If lblStatus IsNot Nothing Then
                'lblStatus.Text = taskItem.StatusName
                If taskItem.StatusSeqId = 1 Then
                    'Open Task
                    If taskItem.DueDate.Length > 0 Then
                        If taskItem.StatusSeqId = 1 And CDate(taskItem.DueDate) < Now Then
                            'Open and Past Due
                            lblStatus.Text = Global.TaskSearchBll.GetTaskStatus(0, True, , IP.Bids.SiteURLs.GetTaskStatusImageUrl)
                        Else
                            lblStatus.Text = Global.TaskSearchBll.GetTaskStatus(taskItem.StatusSeqId, True, , IP.Bids.SiteURLs.GetTaskStatusImageUrl)
                        End If
                    Else
                        lblStatus.Text = Global.TaskSearchBll.GetTaskStatus(taskItem.StatusSeqId, True, , IP.Bids.SiteURLs.GetTaskStatusImageUrl)
                    End If
                Else
                    lblStatus.Text = Global.TaskSearchBll.GetTaskStatus(taskItem.StatusSeqId, True, , IP.Bids.SiteURLs.GetTaskStatusImageUrl)
                End If
            End If

            If lblTaskDueDate IsNot Nothing Then
                If taskItem.DueDate.Length > 0 Then
                    lblTaskDueDate.Text = IP.Bids.SharedFunctions.FormatDate(CDate(taskItem.DueDate))
                End If
            End If
            Dim lblTaskResponsiblePerson As Label = CType(e.Row.FindControl("_lblTaskResponsiblePerson"), Label)
            If lblTaskResponsiblePerson IsNot Nothing Then
                If taskItem.ResponsibleName.Length <= 2 Then
                    lblTaskResponsiblePerson.Text = taskItem.RoleSiteName & " - " & taskItem.RoleDescription
                Else
                    lblTaskResponsiblePerson.Text = taskItem.ResponsibleName
                End If
            End If
            If btnEdit IsNot Nothing Then
                If AllowEdit Then
                    With btnEdit
                        If IsNumeric(TaskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID) AndAlso TaskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID = IP.Bids.SharedFunctions.SourceSystemID.OutageTemplate Then
                            .OnClientClick = GetOutageTaskDetailUrl(taskItem.TaskItemSeqId) & ";return false" '"JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & taskItem.TaskHeaderSeqId & "&TaskNumber=" & taskItem.TaskItemSeqId) & "',false); return false;"
                            'ElseIf taskItem.Dependenttaskseqid.Length > 0 And taskItem.Dependenttaskseqid.ToString <> CStr(taskItem.TaskItemSeqId) Then
                            '    .OnClientClick = GetDependentTaskUrl(taskItem.TaskItemSeqId, taskItem.Dependenttaskseqid) & ";return false" '"JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & taskItem.TaskHeaderSeqId & "&TaskNumber=" & taskItem.TaskItemSeqId) & "',false); return false;"
                        Else
                            .OnClientClick = GetTaskDetailUrl(taskItem.TaskItemSeqId) & ";return false" '"JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & taskItem.TaskHeaderSeqId & "&TaskNumber=" & taskItem.TaskItemSeqId) & "',false); return false;"

                        End If

                    End With
                    btnEdit.Visible = True
                Else
                    btnEdit.Visible = False
                End If
            End If
        End If
    End Sub


    Private Sub SetShowAllTasksProperty()
        ShowClosedTasks = _cbShowClosedTasks.Checked
        If Page.IsPostBack Then
            If Request.Form(Me._cbShowClosedTasks.UniqueID) IsNot Nothing Then
                ShowClosedTasks = True
            Else
                ShowClosedTasks = False
            End If
        End If
    End Sub

    Private Sub SetInspectionTypeProperty()
        If Request.QueryString("TankInspectionType") IsNot Nothing Then
            If IsNumeric(Request.QueryString("TankInspectionType")) Then
                InspectionTypeId = Request.QueryString("TankInspectionType")

                If Page.IsPostBack Then
                    If Request.Form(_ddlInspectionTypeList.UniqueID) IsNot Nothing Then
                        InspectionTypeId = Request.Form(_ddlInspectionTypeList.UniqueID)
                    End If
                End If
                DisplayInspectionTypes()
            End If
        End If
    End Sub


#End Region





End Class
