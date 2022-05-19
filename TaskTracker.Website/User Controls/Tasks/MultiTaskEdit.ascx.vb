Option Explicit On
Option Strict On
Imports System.Drawing
Imports System.Linq
Imports App_Code.BusinessLogicLayer.LinqHelper

Partial Class User_Controls_Tasks_MultiTaskEdit
    Inherits System.Web.UI.UserControl
    'Implements ICallbackEventHandler

    Private refSite As String = String.Empty
    Private _taskstatus As TaskTrackerListsBll
    Private TaskHeaderNumber As String = String.Empty
    Private taskNumber As String = String.Empty
    Const PageSize As Integer = 50
    Const FirstPage As Integer = 1
    Private TotalNumberOfTasks As Integer = 0
    Private PageCount As Integer = 1
    Public Event MultiTaskPostBack()
    Private _postBackTriggerControlId As String = String.Empty
    Private _currentPage As Integer = 1
    Private InspectionTypeId As String = String.Empty

    Public Property AllowReturnToTaskHeader As Boolean
    Public Property AllowTasksToBeFiltered As Boolean

    Dim FilePath As String = ""


    Public Property PostBackTriggerControlId() As String
        Get
            Return _postBackTriggerControlId
        End Get
        Set(ByVal value As String)
            _postBackTriggerControlId = value
        End Set
    End Property
    Public Property CurrentPage() As Integer
        Get
            _currentPage = 1
            If Request.QueryString("Page") IsNot Nothing Then
                If IsNumeric(Request.QueryString("Page")) Then
                    _currentPage = CInt(Request.QueryString("Page"))
                End If
            End If
            Return _currentPage
        End Get
        Set(ByVal value As Integer)
            _currentPage = 1
        End Set
    End Property

    Public Property ShowAllTasks As Boolean


    Private Sub SetInspectionTypeProperty()
        If refSite.ToLower = "tanks" Then
            InspectionTypeId = "0"
            If Request.QueryString("TankInspectionType") IsNot Nothing Then
                If IsNumeric(Request.QueryString("TankInspectionType")) Then
                    InspectionTypeId = Request.QueryString("TankInspectionType")

                    If Page.IsPostBack Then
                        If Request.Form(_ddlInspectionTypeList.UniqueID) IsNot Nothing Then
                            InspectionTypeId = Request.Form(_ddlInspectionTypeList.UniqueID)
                        End If
                    End If

                End If
            End If
            DisplayInspectionTypes()
        End If

    End Sub

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
            .Items.Insert(1, New ListItem("All Repairs", "-1"))
            If .Items.FindByValue(InspectionTypeId) IsNot Nothing Then
                .Items.FindByValue(InspectionTypeId).Selected = True
            End If
        End With
        list = Nothing
    End Sub

    Private Function GetSortableDate(ByVal inputDate As String) As String
        If IsDate(inputDate) Then
            Return CDate(inputDate).ToString("yyyyMMdd")
        End If
        Return inputDate
    End Function
    Public Sub LoadTasks(ByVal useCachedData As Boolean)
        Select Case refSite.ToLower
            Case "tanks"
                SetDefaultSort("ItemTitle")
            Case Else
                SetDefaultSort("group")
        End Select
        If Request.QueryString("HeaderNumber") IsNot Nothing Then
            TaskHeaderNumber = Request.QueryString("HeaderNumber")
        End If
        If Session.Item("TaskNumber") IsNot Nothing Then
            taskNumber = Session.Item("TaskNumber").ToString
        End If

        refSite = String.Empty
        If Request.QueryString("RefSite") IsNot Nothing Then
            refSite = Request.QueryString("RefSite").ToUpper
        End If

        If Request.QueryString("ShowDetails") IsNot Nothing Then
            If Request.QueryString("ShowDetails") = "True" Then
                _cbExpandRows.Checked = True
            End If
        End If
        'ScriptManager.RegisterClientScriptBlock(Page, Page.GetType, "ShowOrHideDetailRow", String.Format(System.Globalization.CultureInfo.CurrentCulture, "toggleDetailRow('{0}');", Me._cbExpandRows.Checked.ToString), True)
        ' $('.DetailRow').hide();

        If TaskHeaderNumber.Length > 0 Then
            Me.AllowTasksToBeFiltered = True
        End If
        _cbShowClosedTasks.Visible = AllowTasksToBeFiltered
        SetInspectionTypeProperty()
        If ShowAllTasks Then _cbShowClosedTasks.Checked = True

        If Request.Form(_cbShowClosedTasks.UniqueID) IsNot Nothing Then
            _cbShowClosedTasks.Checked = True
        Else
            If Page.IsPostBack Then
                _cbShowClosedTasks.Checked = False
            Else
                If Request.QueryString("ShowClosed") IsNot Nothing Then
                    _cbShowClosedTasks.Checked = CBool(Request.QueryString("ShowClosed"))
                End If
            End If

        End If


        Dim listing As System.Collections.Generic.List(Of TaskListing) = Nothing
        'If Not Page.IsPostBack Then
        'IP.Bids.SharedFunctions.DisablePageCache(Response)
        Dim taskSearch As New TaskListingBll
        If TaskHeaderNumber.Length > 0 Then
            taskSearch.HeaderSeqID = TaskHeaderNumber
            listing = taskSearch.GetHeaderItemListing(useCachedData)
            Me.AllowTasksToBeFiltered = True
        Else
            If Session.Item("TaskSearchCriteria") IsNot Nothing Then
                taskSearch = CType(Session.Item("TaskSearchCriteria"), TaskListingBll)
                If taskSearch IsNot Nothing Then
                    listing = taskSearch.GetHeaderItemListing(useCachedData)
                    If taskSearch.HeaderSeqID.Length > 0 Then
                        TaskHeaderNumber = taskSearch.HeaderSeqID
                        Me.AllowTasksToBeFiltered = True
                    End If
                End If
            End If
        End If
        Dim showClosedTasks = (_cbShowClosedTasks.Checked And Me.AllowTasksToBeFiltered = True)
        Dim selectedTask As IQueryable(Of TaskListing) = Nothing

        Dim originalCount As Integer = 0

        If listing IsNot Nothing Then
            originalCount = listing.Count

            If showClosedTasks = False AndAlso AllowTasksToBeFiltered Then
                selectedTask = From item In listing.AsQueryable
                               Where item.ClosedDate.Length = 0
                               Select item
            Else
                selectedTask = From item In listing.AsQueryable
                               Select item
            End If

            If IsNumeric(InspectionTypeId) AndAlso CInt(InspectionTypeId) > 0 Then
                selectedTask = From item In selectedTask.AsQueryable
                               Where item.TankInspectionTypeId = InspectionTypeId
                               Select item

            ElseIf IsNumeric(InspectionTypeId) AndAlso CInt(InspectionTypeId) = -1 Then
                selectedTask = From item In selectedTask.AsQueryable
                               Where item.TankInspectionTypeId.Length = 0
                               Select item

            End If

        End If



        'If Session.Item("TaskItemsToEdit") IsNot Nothing Then
        'listing = TryCast(Session.Item("TaskItemsToEdit"), System.Collections.Generic.List(Of TaskListing))

        If selectedTask IsNot Nothing Then
            TotalNumberOfTasks = selectedTask.Count
            ' _cbShowClosedTasks.Text = String.Format(IP.Bids.SharedFunctions.LocalizeValue("Show All Tasks") & " ({0})", listing.Count)

            'If showClosedTasks Then
            '    _cbShowClosedTasks.Text = String.Format(IP.Bids.SharedFunctions.LocalizeValue("Unclick to only show Open Tasks") & " ({0}/{1})", TotalNumberOfTasks, originalCount)
            'Else
            _cbShowClosedTasks.Text = String.Format(IP.Bids.SharedFunctions.LocalizeValue("Click to show All task (Open and Closed) ") & " ({0}/{1})", TotalNumberOfTasks, originalCount)
            'End If

            If originalCount = 0 Then
                IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveUrl("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=-1&refsite=" & refSite))
            End If

            '_lblPageHeaderBottom.Text = PageLabel()


            If refSite.ToLower = "tanks" Then
                selectedTask = From tasks In selectedTask
                               Order By tasks.ItemTitle, CDate(tasks.DueDate)
                               Select tasks
            Else
                selectedTask = (From tasks In selectedTask
                                Select tasks)
            End If




            Select Case CStr(Session("sortColumnTasks")).ToLower
                Case "sitename"
                    selectedTask = From tasks In selectedTask
                                   Let sortableDueDate = GetSortableDate(tasks.DueDate)
                                   Order By tasks.SiteName Ascending, sortableDueDate
                                   Select tasks
                Case "itemtitle"
                    selectedTask = From tasks In selectedTask
                                   Let sortableDueDate = GetSortableDate(tasks.DueDate)
                                   Order By tasks.ItemTitle Ascending, sortableDueDate
                                   Select tasks
                Case "responsiblename"
                    selectedTask = From tasks In selectedTask
                                   Let sortableDueDate = GetSortableDate(tasks.DueDate)
                                   Order By tasks.ResponsibleName Ascending, sortableDueDate
                                   Select tasks
                Case "priority"
                    selectedTask = From tasks In selectedTask
                                   Let sortableDueDate = GetSortableDate(tasks.DueDate)
                                   Order By tasks.PriorityName Ascending, sortableDueDate
                                   Select tasks
                Case "statusname"
                    selectedTask = From tasks In selectedTask
                                   Let sortableDueDate = GetSortableDate(tasks.DueDate)
                                   Order By tasks.StatusName Ascending, sortableDueDate
                                   Select tasks
                Case "group"
                    selectedTask = From tasks In selectedTask
                                   Let sortableDueDate = GetSortableDate(tasks.ParentDueDate)
                                   Order By sortableDueDate Ascending, tasks.ParentTaskSeqId,
                                      tasks.TaskItemLevel, tasks.DueDate
                                   Select tasks
                Case "duedate"
                    selectedTask = From tasks In selectedTask Let sortableDueDate = GetSortableDate(tasks.DueDate) Order By sortableDueDate Ascending, tasks.ParentTaskSeqId, tasks.TaskItemLevel, tasks.DueDate Select tasks
                Case "closeddate"
                    selectedTask = (From tasks In selectedTask
                                    Let sortableDueDate = GetSortableDate(tasks.ClosedDate)
                                    Order By sortableDueDate
                                    Select tasks)
                Case Else
                    selectedTask = selectedTask.OrderByField(CStr(Session("sortColumnTasks")), CBool(Session.Item("sortDirectionTasks").ToString = "Ascending"))
            End Select

            _grdEditTaskList.AllowPaging = False

            'Only page when a task number has not been specified

            Dim filteredTaskList As List(Of TaskListing)
            If taskNumber Is Nothing OrElse IsNumeric(taskNumber) = False Then
                filteredTaskList = selectedTask.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList()
                PageCount = (TotalNumberOfTasks \ PageSize)
                If PageCount = 0 Then
                    PageCount = 1
                ElseIf TotalNumberOfTasks Mod PageSize > 0 Then
                    PageCount += 1
                End If

                If CurrentPage > PageCount Then
                    CurrentPage = 1
                End If
            Else
                filteredTaskList = selectedTask.ToList()
                PageCount = 1
            End If
            _lblPageHeader.Text = PageLabel()
            _grdEditTaskList.DataSource = filteredTaskList 'selectedTask.Skip((CurrentPage - 1) * PageSize).Take(PageSize)
            _grdEditTaskList.AutoGenerateColumns = False
            _grdEditTaskList.DataBind()
            '_grdEditTaskList.Caption = IP.Bids.SharedFunctions.LocalizeValue("Click on the column headers to sort the Tasks", True)
            _taskListCount.Text = IP.Bids.SharedFunctions.LocalizeValue("Record Count", True) & " : " & TotalNumberOfTasks.ToString
            If taskNumber IsNot Nothing AndAlso IsNumeric(taskNumber) Then
                For i As Integer = 0 To _grdEditTaskList.Rows.Count - 1
                    If _grdEditTaskList.DataKeys.Item(i).Value.ToString = taskNumber Then
                        _grdEditTaskList.SelectRow(i)
                        _grdEditTaskList.SelectedRow.BorderColor = Color.Red
                        _grdEditTaskList.SelectedRow.BorderStyle = BorderStyle.Solid
                        _grdEditTaskList.SelectedRow.Focus()
                        Exit For
                    End If
                Next
            End If
            'End If
            'Dim enableClienSideSort As Boolean = False
            'If enableClienSideSort Then
            If _grdEditTaskList.Rows.Count > 0 Then
                'Dim gridId As String = _grdEditTaskList.ClientID
                'ScriptManager.RegisterStartupScript(Page, Page.GetType, "SortGrid_" & gridId, String.Format(System.Globalization.CultureInfo.CurrentCulture, "$(function(){{$('#{0}').tablesorter(); }});", gridId), True)
                _grdEditTaskList.UseAccessibleHeader = True
                _grdEditTaskList.HeaderRow.TableSection = TableRowSection.TableHeader
            End If
            'End If

            ConfigureNavigationButtons()
            ConfigureSubmitButton()
            ConfigureAddTasksButton(TaskHeaderNumber)
            HandleRepairOnlyItems()
        End If
    End Sub

    Private Sub HandleRepairOnlyItems()
        If Request.QueryString("RepairOnly") IsNot Nothing Then
            If Request.QueryString("RepairOnly").ToString.ToLower = "true" Then
                _btnSaveEdits.Visible = False
                _btnAddNewTask.Visible = False
                _grdEditTaskList.Caption = ""
                _taskListCount.Text = ""
            End If
        End If
    End Sub
    Private Sub ConfigureSubmitButton()
        Page.ClientScript.RegisterOnSubmitStatement(Page.GetType, Me.ID & "DisableSubmitButtons", "window.DisplayBusy();DisplayBusy();return true;")
        _btnSaveEdits.Visible = TotalNumberOfTasks > 0
    End Sub

    Private Sub ConfigureAddTasksButton(ByVal headerNumber As String)
        If headerNumber.Length > 0 Then
            If refSite = "IRIS" Or refSite = "TANKS" Then
                _btnAddNewTask.OnClientClick = "Javascript:try{window.parent.DisplayBusy();}catch(e){};try{window.location='../blank.htm';window.location='" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & headerNumber & "&TaskNumber=-1" & "&refsite=" & refSite) & "'}catch(e){} return false" '& "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItemNumber & "&refsite=" & refSite) & "',false);" & "};"
            Else
                _btnAddNewTask.OnClientClick = "Javascript:try{window.parent.DisplayBusy();}catch(e){};try{window.location='../blank.htm';window.parent.location='" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & headerNumber & "&TaskNumber=-1" & "&refsite=" & refSite) & "'}catch(e){} return false" '& "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItemNumber & "&refsite=" & refSite) & "',false);" & "};"
            End If
            '_btnAddNewTask.OnClientClick = "window.location='" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & headerNumber & "&TaskNumber=-1&refsite=" & refSite) & "'; return false"
            _btnAddNewTask.Visible = True
        Else
            _btnAddNewTask.Visible = False
        End If
    End Sub

    Private Sub ConfigureNavigationButtons()
        ConfigureFirstPageButton()
        ConfigureLastButton()
        ConfigureNextButton()
        ConfigurePreviousButton()
        ConfigureShowAllButton()
        ConfigureReturnToHeaderButton()
    End Sub

    Private Sub ConfigureReturnToHeaderButton()
        If Me.TaskHeaderNumber IsNot Nothing AndAlso Me.TaskHeaderNumber.Length > 0 AndAlso AllowReturnToTaskHeader Then
            _btnReturnToTaskHeader.Visible = True
            Dim url As String = IP.Bids.SiteURLs.GetTaskHeaderURL(TaskHeaderNumber, refSite)  '"window.DisplayBusy();window.location='" &IP.Bids.SharedFunctions.ResponseRedirect("~/TaskHeader.aspx?HeaderNumber=" & TaskHeaderNumber & "&RefSite=" & refSite)
            _btnReturnToTaskHeader.OnClientClick = "window.DisplayBusy();window.location='" & Page.ResolveClientUrl(url) & "';return false;"
        Else
            _btnReturnToTaskHeader.Visible = False
        End If
    End Sub
    Private Sub ConfigureShowAllButton()
        '_cbShowClosedTasks.Attributes.Add("onClick", "window.DisplayBusy();window.location='" & AddPageNumberToQueryString(CurrentPage, True) & "';return false;")
    End Sub
    Private Sub ConfigureFirstPageButton()
        _btnPageFirst.OnClientClick = "window.DisplayBusy();window.location='" & AddPageNumberToQueryString(FirstPage, True) & "';return false;"
        _btnPageFirst.Enabled = PageCount > 1 And CurrentPage <> 1

        _btnPageFirst.Visible = PageCount > 1
        '_btnPageFirstBottom.OnClientClick = "window.location='" & AddPageNumberToQueryString(FirstPage) & "';return false;"
        '_btnPageFirstBottom.Enabled = PageCount > 1 And CurrentPage <> 1
    End Sub

    Private Sub ConfigureNextButton()
        _btnPageNext.OnClientClick = "window.DisplayBusy();window.location='" & AddPageNumberToQueryString(CurrentPage + 1, True) & "';return false;"
        _btnPageNext.Enabled = CurrentPage + 1 <= PageCount
        _btnPageNext.Visible = PageCount > 1
        '_btnPageNextBottom.OnClientClick = "window.location='" & AddPageNumberToQueryString(CurrentPage + 1) & "';return false;"
        '_btnPageNextBottom.Enabled = CurrentPage + 1 <= PageCount
    End Sub

    Private Sub ConfigurePreviousButton()
        _btnPagePrevious.OnClientClick = "window.DisplayBusy();window.location='" & AddPageNumberToQueryString(CurrentPage - 1, True) & "';return false;"
        _btnPagePrevious.Enabled = CurrentPage - 1 >= FirstPage
        _btnPagePrevious.Visible = PageCount > 1
        '_btnPagePreviousBottom.OnClientClick = "window.location='" & AddPageNumberToQueryString(CurrentPage - 1) & "';return false;"
        '_btnPagePreviousBottom.Enabled = CurrentPage - 1 >= FirstPage
    End Sub

    Private Sub ConfigureLastButton()
        _btnPageLast.OnClientClick = "window.DisplayBusy();window.location='" & AddPageNumberToQueryString(PageCount, True) & "';return false;"
        _btnPageLast.Enabled = PageCount > 1 And CurrentPage <> PageCount
        _btnPageLast.Visible = PageCount > 1
        '_btnPageLastBottom.OnClientClick = "window.location='" & AddPageNumberToQueryString(PageCount) & "';return false;"
        '_btnPageLastBottom.Enabled = PageCount > 1 And CurrentPage <> PageCount
    End Sub

    Private Function AddPageNumberToQueryString(ByVal pageNumber As Integer, ByVal useCachedData As Boolean) As String
        Dim url As String = HttpContext.Current.Request.Url.AbsoluteUri
        Dim separateUrl() = url.Split(CChar("?"))
        If separateUrl.Length > 1 Then
            If separateUrl.Length > 0 Then
                Dim queryString As NameValueCollection
                queryString = System.Web.HttpUtility.ParseQueryString(separateUrl(1))

                If queryString.Item("Page") Is Nothing Then
                    queryString.Add("Page", CStr(pageNumber))
                Else
                    queryString.Item("Page") = CStr(pageNumber)
                End If

                If queryString.Item("useCachedData") Is Nothing Then
                    queryString.Add("useCachedData", CStr(useCachedData))
                Else
                    queryString.Item("useCachedData") = CStr(useCachedData)
                End If

                If queryString.Item("ShowClosed") Is Nothing Then
                    queryString.Add("ShowClosed", CStr(_cbShowClosedTasks.Checked))
                Else
                    queryString.Item("ShowClosed") = CStr(_cbShowClosedTasks.Checked)
                End If

                If queryString.Item("TankInspectionType") Is Nothing Then
                    queryString.Add("TankInspectionType", CStr(InspectionTypeId))
                Else
                    queryString.Item("TankInspectionType") = CStr(InspectionTypeId)
                End If

                If queryString.Item("ShowDetails") Is Nothing Then
                    queryString.Add("ShowDetails", Me._cbExpandRows.Checked.ToString)
                Else
                    queryString.Item("ShowDetails") = Me._cbExpandRows.Checked.ToString
                End If
                url = separateUrl(0) & "?" & queryString.ToString()
            End If
        Else
            url = url & "?Page=" & pageNumber.ToString
        End If
        Return url
    End Function

    'Private Function FormKeyContains(ByVal key As String) As Boolean
    '    For Each key In 
    'End Function
    Protected Sub _grdEditTaskList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles _grdEditTaskList.Load
        'If Page.IsPostBack Then
        'controlIndex.tostring

        FilePath = HttpContext.Current.Server.MapPath("~\\TraceLog\\")



        For Each row As GridViewRow In _grdEditTaskList.Rows

            IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "MultitaskEdit_grdEditTaskList_Load.txt", "_grdEditTaskList_Load Start : " & DateTime.Now.ToString, True)


            If row.RowType = DataControlRowType.DataRow Then
                Dim EstimatedDueDate As UserControlsJQDateRange = CType(row.FindControl("_EstimatedDueDate" & (row.RowIndex + (_grdEditTaskList.PageIndex * _grdEditTaskList.PageSize)).ToString), UserControlsJQDateRange)
                If EstimatedDueDate IsNot Nothing Then 'AndAlso Request.Form.AllKeys.Contains(EstimatedDueDate.UniqueID & "$") Then
                    For Each key As String In Request.Form.AllKeys '.Where(Function(k) k.Contains(EstimatedDueDate.UniqueID & "$")) '.ToArray() 'And Not k.EndsWith("Critical")
                        If key Is Nothing Then Continue For 'For Each key As String In Request.Form.AllKeys
                        If key.Contains(EstimatedDueDate.UniqueID & "$") AndAlso Not key.EndsWith("Critical") Then
                            If Request.Form(key) IsNot Nothing AndAlso Request.Form(key).Length > 0 Then
                                EstimatedDueDate.StartDate = Request.Form(key)
                                Exit For
                            End If
                        End If
                    Next
                End If
                '_ClosedDate
                Dim ClosedDate As UserControlsJQDateRange = CType(row.FindControl("_ClosedDate" & (row.RowIndex + (_grdEditTaskList.PageIndex * _grdEditTaskList.PageSize)).ToString), UserControlsJQDateRange)
                If ClosedDate IsNot Nothing Then 'AndAlso Request.Form.AllKeys.Contains(ClosedDate.UniqueID & "$") Then

                    For Each key As String In Request.Form.AllKeys '.Where(Function(k) k.Contains(ClosedDate.UniqueID & "$") And Not k.EndsWith("Critical")).ToArray()
                        If key Is Nothing Then Continue For
                        If key.Contains(ClosedDate.UniqueID & "$") AndAlso Not key.EndsWith("Critical") Then
                            'If key.Contains(EstimatedDueDate.UniqueID) Then
                            If Request.Form(key) IsNot Nothing AndAlso Request.Form(key).Length > 0 Then
                                ClosedDate.StartDate = Request.Form(key)
                            End If
                            Exit For
                        End If
                    Next
                End If

                '_txtTitle CType(e.Row.FindControl("_txtTitle"), AdvancedTextBox.AdvancedTextBox)
                Dim title As TextBox = CType(row.FindControl("_txtTitle" & (row.RowIndex + (_grdEditTaskList.PageIndex * _grdEditTaskList.PageSize)).ToString), TextBox)
                'Dim title As AdvancedTextBox.AdvancedTextBox = CType(row.FindControl("_txtTitle" & (row.RowIndex + (_grdEditTaskList.PageIndex * _grdEditTaskList.PageSize)).ToString), AdvancedTextBox.AdvancedTextBox)
                If title IsNot Nothing Then 'AndAlso Request.Form.AllKeys.Contains(title.UniqueID & "$") Then

                    For Each key As String In Request.Form.AllKeys '.Where(Function(k) k.EndsWith(title.UniqueID)).ToArray()
                        If key Is Nothing Then Continue For
                        If key.EndsWith(title.UniqueID) Then
                            'If key.Contains(EstimatedDueDate.UniqueID) Then
                            If Request.Form(key) IsNot Nothing AndAlso Request.Form(key).Length > 0 Then
                                title.Text = Request.Form(key)
                            End If
                            Exit For
                        End If
                    Next
                End If

                Dim description As TextBox = CType(row.FindControl("_txtDescription" & (row.RowIndex + (_grdEditTaskList.PageIndex * _grdEditTaskList.PageSize)).ToString), TextBox)
                If description IsNot Nothing Then

                    For Each key As String In Request.Form.AllKeys
                        If key Is Nothing Then Continue For
                        If key.EndsWith(description.UniqueID) Then
                            If Request.Form(key) IsNot Nothing AndAlso Request.Form(key).Length > 0 Then
                                description.Text = Request.Form(key)
                            End If
                            Exit For
                        End If
                    Next
                End If

            End If


            Dim ddlStatus As DropDownList = CType(row.FindControl("_ddlTaskStatus" & (row.RowIndex + (_grdEditTaskList.PageIndex * _grdEditTaskList.PageSize)).ToString), DropDownList)
                If ddlStatus IsNot Nothing Then 'AndAlso Request.Form.AllKeys.Contains(ddlStatus.UniqueID & "$") Then
                    For Each key As String In Request.Form.AllKeys '.Where(Function(k) k.EndsWith(ddlStatus.UniqueID)).ToArray()
                        If key Is Nothing Then Continue For
                        If key.EndsWith(ddlStatus.UniqueID) Then
                            'If key.Contains(EstimatedDueDate.UniqueID) Then
                            If Request.Form(key) IsNot Nothing AndAlso Request.Form(key).Length > 0 Then
                                If ddlStatus.Items.FindByValue(Request.Form(key)) IsNot Nothing Then
                                    ddlStatus.ClearSelection()
                                    ddlStatus.Items.FindByValue(Request.Form(key)).Selected = True
                                End If
                            End If
                            Exit For
                        End If
                    Next
                End If
                Dim ddlPriority As DropDownList = CType(row.FindControl("_ddlPriority" & (row.RowIndex + (_grdEditTaskList.PageIndex * _grdEditTaskList.PageSize)).ToString), DropDownList)
                If ddlPriority IsNot Nothing Then 'AndAlso Request.Form.AllKeys.Contains(ddlStatus.UniqueID & "$") Then
                    For Each key As String In Request.Form.AllKeys '.Where(Function(k) k.EndsWith(ddlStatus.UniqueID)).ToArray()
                        If key Is Nothing Then Continue For
                        If key.EndsWith(ddlPriority.UniqueID) Then
                            'If key.Contains(EstimatedDueDate.UniqueID) Then
                            If Request.Form(key) IsNot Nothing AndAlso Request.Form(key).Length > 0 Then
                                If ddlPriority.Items.FindByValue(Request.Form(key)) IsNot Nothing Then
                                    ddlPriority.ClearSelection()
                                    ddlPriority.Items.FindByValue(Request.Form(key)).Selected = True
                                End If
                            End If
                            Exit For
                        End If
                    Next
                End If

            IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "MultitaskEdit_grdEditTaskList_Load.txt", "_grdEditTaskList_Load END : " & DateTime.Now.ToString, False)


        Next
        If TaskHeaderNumber.Length > 0 Then
            _grdEditTaskList.Columns(1).Visible = False
            _grdEditTaskList.Columns(2).Visible = False
        Else
            If Session.Item("TaskSearchCriteria") IsNot Nothing Then
                Dim taskSearch = CType(Session.Item("TaskSearchCriteria"), TaskListingBll)
                If taskSearch IsNot Nothing AndAlso taskSearch.PlantCode.ToLower <> "all" Then
                    _grdEditTaskList.Columns(1).Visible = False
                End If
            End If
            _grdEditTaskList.Columns(2).Visible = True
        End If
    End Sub

    'Protected Sub _grdEditTaskList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles _grdEditTaskList.PageIndexChanging
    '    e.Cancel = True
    '    Exit Sub
    '    With _grdEditTaskList
    '        Dim listing As System.Collections.Generic.List(Of TaskListing) = TryCast(Session.Item("TaskItemsToEdit"), System.Collections.Generic.List(Of TaskListing))
    '        .DataSource = listing
    '        .PageIndex = e.NewPageIndex
    '        .DataBind()
    '    End With
    'End Sub

    'Protected Sub _btnPageFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    SaveTasks()
    '    With _grdEditTaskList
    '        Dim listing As System.Collections.Generic.List(Of TaskListing) = TryCast(Session.Item("TaskItemsToEdit"), System.Collections.Generic.List(Of TaskListing))
    '        .DataSource = listing
    '        .PageIndex = 0
    '        .DataBind()
    '    End With
    'End Sub

    'Protected Sub _btnPageLast_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    SaveTasks()
    '    With _grdEditTaskList
    '        Dim listing As System.Collections.Generic.List(Of TaskListing) = TryCast(Session.Item("TaskItemsToEdit"), System.Collections.Generic.List(Of TaskListing))
    '        .DataSource = listing
    '        .PageIndex = .PageCount
    '        .DataBind()
    '    End With
    'End Sub

    'Protected Sub _btnPagePrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    SaveTasks()
    '    Dim currentIndex As Integer
    '    With _grdEditTaskList
    '        currentIndex = .PageIndex
    '        If currentIndex > 0 Then
    '            currentIndex = currentIndex - 1
    '        End If
    '        Dim listing As System.Collections.Generic.List(Of TaskListing) = TryCast(Session.Item("TaskItemsToEdit"), System.Collections.Generic.List(Of TaskListing))
    '        .DataSource = listing
    '        .PageIndex = currentIndex
    '        .DataBind()
    '    End With
    'End Sub

    'Protected Sub _btnPageNext_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    SaveTasks()
    '    Dim currentIndex As Integer
    '    With _grdEditTaskList
    '        currentIndex = .PageIndex
    '        If currentIndex < .PageCount Then
    '            currentIndex = currentIndex + 1
    '        End If
    '        Dim listing As System.Collections.Generic.List(Of TaskListing) = TryCast(Session.Item("TaskItemsToEdit"), System.Collections.Generic.List(Of TaskListing))
    '        .DataSource = listing
    '        .PageIndex = currentIndex
    '        .DataBind()
    '    End With
    'End Sub

    Private Function PageLabel() As String
        If PageCount = 1 Then Return ""
        If CurrentPage > PageCount Then
            Return String.Format(IP.Bids.SharedFunctions.LocalizeValue("Page") & " {0} - {1}", 1, PageCount)
        Else
            Return String.Format(IP.Bids.SharedFunctions.LocalizeValue("Page") & " {0} - {1}", CurrentPage, PageCount)
        End If
    End Function


    Private Sub ReselectResponsibleUser()
        'If Page.IsPostBack Then
        For Each r As GridViewRow In _grdEditTaskList.Rows
            If r.RowType = DataControlRowType.DataRow Then
                Dim ResponsiblePerson As User_Controls.AdvancedEmployeeListDropdown = CType(r.FindControl(String.Format("_ResponsiblePerson{0}", r.RowIndex)), User_Controls.AdvancedEmployeeListDropdown)
                If ResponsiblePerson IsNot Nothing Then
                    If Request.Form(ResponsiblePerson.EmployeeListUniqueId) IsNot Nothing Then
                        ResponsiblePerson.PopulateEmployeeList(Request.Form(ResponsiblePerson.EmployeeListUniqueId))
                    End If
                End If
            End If
        Next
        'End If
    End Sub






    Protected Sub SaveTasks()
        Dim taskItemBLL As New TaskTrackerItemBll
        Dim newTaskItem As New TaskItem
        Dim updatedUser As String = IP.Bids.SharedFunctions.GetCurrentUser.Username
        Dim statusItems As System.Collections.Generic.List(Of TaskStatus)
        Dim taskIsClosed As Boolean
        Dim TaskItemNumber As String
        Dim TaskHeaderNumber As String
        Dim sendEmail As Boolean
        Dim taskHasChanged As Boolean

        If _taskstatus Is Nothing Then
            _taskstatus = New TaskTrackerListsBll
        End If
        statusItems = _taskstatus.GetTaskStatus
        'If _grdEditTaskList.DirtyRows.Count > 0 Then
        For Each r As GridViewRow In _grdEditTaskList.Rows '.DirtyRows

            newTaskItem = taskItemBLL.GetTaskItem(CInt(Me._grdEditTaskList.DataKeys.Item(r.RowIndex).Value))
            Dim title As TextBox = TryCast(r.FindControl(String.Format("_txtTitle{0}", r.RowIndex)), TextBox)
            'Dim title As AdvancedTextBox.AdvancedTextBox = TryCast(r.FindControl(String.Format("_txtTitle", r.RowIndex)), AdvancedTextBox.AdvancedTextBox)
            Dim dueDate = TryCast(r.FindControl(String.Format("_EstimatedDueDate{0}", r.RowIndex)), UserControlsJQDateRange)
            Dim closedDate = TryCast(r.FindControl(String.Format("_ClosedDate{0}", r.RowIndex)), UserControlsJQDateRange)
            Dim responsible As User_Controls.AdvancedEmployeeListDropdown = TryCast(r.FindControl(String.Format("_ResponsiblePerson{0}", r.RowIndex)), User_Controls.AdvancedEmployeeListDropdown)
            Dim status = TryCast(r.FindControl(String.Format("_ddlTaskStatus{0}", r.RowIndex)), DropDownList)
            Dim priority = TryCast(r.FindControl(String.Format("_ddlPriority{0}", r.RowIndex)), DropDownList)
            Dim description = TryCast(r.FindControl(String.Format("_txtDescription{0}", r.RowIndex)), TextBox)
            Dim comments = TryCast(r.FindControl(String.Format("_txtNewComment{0}", r.RowIndex)), TextBox)
            taskHasChanged = False
            With newTaskItem
                .UpdateFlag = .UpdateFlag
                TaskHeaderNumber = CStr(.TaskHeaderSeqId)
                TaskItemNumber = CStr(.TaskItemSeqId)
                'Determine if task is currently closed 
                If .ClosedDate.Length > 0 Then
                    taskIsClosed = True

                    If comments IsNot Nothing Then
                        If comments.Text.Length > 0 Then
                            .Comments = comments.Text
                            taskHasChanged = True
                        End If
                    End If
                    'The only things that can changed on a closed task is the Closed Date and Status
                    If closedDate IsNot Nothing AndAlso status IsNot Nothing Then
                        'If .ClosedDate <> closedDate.StartDate Then
                        '    'Closed Date has changed
                        '    taskHasChanged = True
                        '    If closedDate.StartDate.Length > 0 Then
                        '        .ClosedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(closedDate.StartDate))
                        '    Else
                        '        .ClosedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Now)
                        '    End If
                        'End If

                        If .StatusSeqId <> CDbl(status.SelectedValue) Then
                            taskHasChanged = True
                            .StatusSeqId = CInt(status.SelectedValue)
                        End If
                        If .StatusSeqId = 1 Then
                            .ClosedDate = ""
                        End If
                    End If
                    If dueDate.StartDate.Length > 0 Then
                        .DueDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(dueDate.StartDate))
                    End If
                Else
                    'Task is currently open which means that all fields can change
                    taskIsClosed = False
                    If priority.SelectedValue <> .Priority Then
                        taskHasChanged = True
                        .Priority = priority.SelectedValue
                    End If
                    'Status
                    If .StatusSeqId <> CDbl(status.SelectedValue) And CDbl(status.SelectedValue) <> 1 Then
                        taskHasChanged = True
                        .StatusSeqId = CInt(status.SelectedValue)
                        .ClosedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Now)
                        taskIsClosed = True
                    End If

                    'Title
                    If title IsNot Nothing Then
                        If title.Text <> .Title AndAlso title.Text.Length > 0 Then
                            taskHasChanged = True
                            .Title = HttpContext.Current.Server.HtmlEncode(title.Text)
                        End If
                    End If

                    If description IsNot Nothing Then
                        If description.Text <> .Description Then
                            taskHasChanged = True
                            .Description = HttpContext.Current.Server.HtmlEncode(description.Text)
                        End If
                    End If

                    If comments IsNot Nothing Then
                        If comments.Text.Length > 0 Then
                            .Comments = HttpContext.Current.Server.HtmlEncode(comments.Text)
                            taskHasChanged = True
                        End If

                    End If
                    'Responsible
                    If responsible IsNot Nothing Then
                        If .ResponsibleRoleSeqId > 0 Then 'Role
                            If .ResponsibleRoleSeqId.ToString <> CStr(responsible.SelectedValue) Then
                                'Responsible has changed
                                taskHasChanged = True
                                If IsNumeric(responsible.SelectedValue) Then 'Role
                                    .ResponsibleRoleSeqId = CInt(responsible.SelectedValue)
                                    .ResponsibleRolePlantCode = responsible.PlantCode
                                    .ResponsibleUserName = ""
                                Else
                                    'Username
                                    .ResponsibleUserName = responsible.SelectedValue
                                    .ResponsibleRolePlantCode = responsible.PlantCode
                                    .ResponsibleRoleSeqId = -1
                                End If
                                sendEmail = True
                            ElseIf taskIsClosed And .ResponsibleUserName.Length = 0 Then
                                taskHasChanged = True
                                .ResponsibleRoleSeqId = -1
                                .ResponsibleUserName = updatedUser
                            End If
                        Else
                            'User
                            If .ResponsibleUserName <> responsible.SelectedValue Then
                                'Responsible has changed
                                taskHasChanged = True
                                If IsNumeric(responsible.SelectedValue) Then 'Role
                                    .ResponsibleRoleSeqId = CInt(responsible.SelectedValue)
                                    .ResponsibleRolePlantCode = responsible.PlantCode
                                    .ResponsibleUserName = ""
                                Else
                                    'Username
                                    .ResponsibleUserName = responsible.SelectedValue
                                    .ResponsibleRolePlantCode = responsible.PlantCode
                                    .ResponsibleRoleSeqId = -1
                                End If
                                sendEmail = True
                            ElseIf taskIsClosed And .ResponsibleUserName.Length = 0 Then
                                taskHasChanged = True
                                .ResponsibleRoleSeqId = -1
                                .ResponsibleUserName = updatedUser
                            End If
                        End If
                    End If

                    If taskIsClosed Then
                        If .ResponsibleRoleSeqId > 0 Then
                            taskHasChanged = True
                            .ResponsibleRoleSeqId = -1
                            .ResponsibleUserName = updatedUser
                            sendEmail = False
                        End If
                    End If

                    'Due Date
                    If dueDate IsNot Nothing Then
                        If Date.Parse(newTaskItem.DueDate) <> Date.Parse(dueDate.StartDate) Then
                            'Duedate has changed
                            taskHasChanged = True
                            .DueDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(dueDate.StartDate))
                        Else
                            .DueDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(dueDate.StartDate))
                        End If

                        If dueDate.DateIsCritical = True Then
                            If .DateCritical <> "Y" Then
                                taskHasChanged = True
                                .DateCritical = "Y"
                            End If
                        Else
                            If .DateCritical <> "N" Then
                                taskHasChanged = True
                                .DateCritical = "N"
                            End If
                        End If
                    End If

                    'Closed Date
                    If closedDate IsNot Nothing Then
                        If closedDate.StartDate.Length > 0 Then
                            'Task has been closed
                            taskHasChanged = True
                            .ClosedDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(closedDate.StartDate))
                            'Status should be changed from Open and Responsible needs to be a person
                            If .ResponsibleUserName.Length = 0 Then
                                .ResponsibleUserName = updatedUser
                                .ResponsibleRoleSeqId = -1
                            End If
                            If .StatusSeqId = 1 Then 'Open
                                For Each item As TaskStatus In statusItems
                                    If item.StatusName.ToLower = "complete" Then
                                        .StatusSeqId = item.StatusSeqid
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If
                If taskHasChanged = True Then
                    .DaysAfter = -1
                    .DaysBefore = -1
                    .LastUpdateDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Now)
                    .LastUpdateUserName = updatedUser

                End If
            End With


            Dim updatedTaskItem As System.Collections.Generic.List(Of TaskItem) = Nothing
            If taskHasChanged = True Then
                'verify required fields
                Dim requiredFieldsAreSet As Boolean = True
                If newTaskItem.ResponsibleUserName.Length = 0 And newTaskItem.ResponsibleRoleSeqId <= 1 Then
                    requiredFieldsAreSet = False
                End If
                If newTaskItem.DueDate.Length = 0 Then
                    requiredFieldsAreSet = False
                End If
                If newTaskItem.Title.Length = 0 Then
                    requiredFieldsAreSet = False
                End If
                If requiredFieldsAreSet Then
                    updatedTaskItem = taskItemBLL.SaveTaskItem(newTaskItem)
                Else
                    updatedTaskItem = Nothing
                    r.BackColor = Drawing.Color.Red
                End If
            End If

            If updatedTaskItem IsNot Nothing AndAlso updatedTaskItem.Count > 0 Then
                If closedDate IsNot Nothing AndAlso closedDate.StartDate.Length > 0 Then
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
                If sendEmail And closedDate.StartDate.Length = 0 Then
                    EmailDataBll.GetAndSendImmediateEmail(updatedTaskItem.Item(0).TaskItemSeqId, CInt(TaskHeaderNumber))
                End If
            End If
        Next

        'LoadTasks(False)
    End Sub

    Private Function ShowMultipleFacilities() As Boolean
        Return TaskHeaderNumber.Length = 0
    End Function
    'Protected Sub _btnReturnToTaskList_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetTaskListURL(TaskHeaderNumber, RefSite, False, True, True))
    '    'HttpContext.Current.ApplicationInstance.CompleteRequest()
    'End Sub

    Protected Sub _btnSaveEdits_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSaveEdits.Click
        'todo - Disable the save button on postback
        SaveTasks()
        RaiseEvent MultiTaskPostBack()
        If Request.QueryString("postBackTrigger") IsNot Nothing Then
            PostBackTriggerControlId = Request.QueryString("postBackTrigger")
        End If
        _grdEditTaskList.Visible = False
        'Session.Item("TaskEditCurrentPage") = CurrentPage
        'If PostBackTriggerControlId.Length > 0 Then
        '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "ReloadParent_" & Me.ClientID, "try{parent.document.getElementById('" & PostBackTriggerControlId & "').click();}catch(e){alert(e);}", True)
        'Else
        IP.Bids.SharedFunctions.ResponseRedirect(AddPageNumberToQueryString(CurrentPage, False))
        'End If
    End Sub

    Protected Sub _grdEditTaskList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles _grdEditTaskList.RowCreated
        Dim OpenTaskSeqId As Integer
        Dim task As BO.TaskListing = Nothing
        Dim ddlStatus As DropDownList
        Dim ddlPriority As DropDownList
        Dim EstimatedDueDate As UserControlsJQDateRange
        Dim ClosedDate As UserControlsJQDateRange
        Dim EditButton As LinkButton
        Dim DeleteButton As LinkButton
        Dim title As TextBox 'AdvancedTextBox.AdvancedTextBox
        Dim description As TextBox
        Dim comments As TextBox
        Dim taskIsClosed As Boolean = False
        Static status As System.Collections.Generic.List(Of TaskStatus)
        Dim controlIndex As String = CStr(e.Row.RowIndex + (_grdEditTaskList.PageIndex * _grdEditTaskList.PageSize))
        Dim readOnlyTitle As Literal
        Dim readOnlyDescription As Literal
        Dim readOnlyResponsible As Literal
        Dim readOnlyDueDate As Literal
        Dim readOnlyClosedDate As Literal
        Dim HasOpenSubTasks As Boolean
        Dim isIRISTask As Boolean
        Dim sortableClosedDate As Label
        Dim taskItemBLL As New TaskTrackerItemBll
        Dim commentsGrid As GridView
        Dim newCommentButton As LinkButton
        Dim childRow As Label
        Dim taskTypeIcon As Literal

        FilePath = HttpContext.Current.Server.MapPath("~\\TraceLog\\")

        'If e.Row.DataItem IsNot Nothing Then
        'If Page.IsPostBack = False Then
        If e.Row.RowType = DataControlRowType.DataRow Then

            IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "MultitaskEdit_grdEditTaskList_RowCreated.txt", "_grdEditTaskList_RowCreated Start : " & DateTime.Now.ToString, True)


            HasOpenSubTasks = False
            taskTypeIcon = CType(e.Row.FindControl("_litTaskTypeIcon"), Literal)
            childRow = CType(e.Row.FindControl("_lblChildTask"), Label)
            ddlStatus = CType(e.Row.FindControl("_ddlTaskStatus"), DropDownList)
            ddlPriority = CType(e.Row.FindControl("_ddlPriority"), DropDownList)
            EstimatedDueDate = CType(e.Row.FindControl("_EstimatedDueDate"), UserControlsJQDateRange)
            ClosedDate = CType(e.Row.FindControl("_ClosedDate"), UserControlsJQDateRange)
            title = CType(e.Row.FindControl("_txtTitle"), TextBox)
            description = CType(e.Row.FindControl("_txtDescription"), TextBox)
            comments = CType(e.Row.FindControl("_txtNewComment"), TextBox)
            EditButton = CType(e.Row.FindControl("_lnkEditTask"), LinkButton)
            DeleteButton = CType(e.Row.FindControl("_lnkDeleteTask"), LinkButton)
            newCommentButton = CType(e.Row.FindControl("_lnkComment"), LinkButton)
            readOnlyTitle = CType(e.Row.FindControl("_litTitle"), Literal)
            readOnlyDescription = CType(e.Row.FindControl("_litDescription"), Literal)
            readOnlyResponsible = CType(e.Row.FindControl("_litResponsiblePerson"), Literal)
            readOnlyDueDate = CType(e.Row.FindControl("_litDueDate"), Literal)
            readOnlyClosedDate = CType(e.Row.FindControl("_litClosedDate"), Literal)
            sortableClosedDate = CType(e.Row.FindControl("_sortableClosedDate"), Label)
            commentsGrid = CType(e.Row.FindControl("_gvComments"), GridView)
            'title = CType(e.Row.FindControl("_txtTitle"), AdvancedTextBox.AdvancedTextBox)
            If _taskstatus Is Nothing Then
                _taskstatus = New TaskTrackerListsBll
            End If
            If status Is Nothing OrElse status.Count = 0 Then
                status = _taskstatus.GetTaskStatus
            End If
            If sortableClosedDate IsNot Nothing Then
                sortableClosedDate.Text = New Date(1900, 1, 1).ToString("yyyyMMdd")
            End If
            If ddlPriority IsNot Nothing Then
                With ddlPriority
                    .Items.Clear()
                    .Width = Unit.Percentage(90)
                    .ID = .ID & controlIndex.ToString
                    'Dim taskItemBll As New TaskTrackerItemBll
                    Dim priorities = taskItemBLL.GetPriorityList
                    If priorities IsNot Nothing AndAlso priorities.Count > 0 Then
                        For Each item As TaskPriorities In priorities
                            .Items.Add(New ListItem(item.PriorityName, CStr(item.PriorityID)))
                        Next
                        .DataBind()
                    End If
                End With
            End If
            If ddlStatus IsNot Nothing Then
                With ddlStatus
                    .Items.Clear()
                    .Width = Unit.Percentage(90)
                    .ID = .ID & controlIndex.ToString
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

                    '.SelectedValue = CStr(OpenTaskSeqId)
                End With
            End If

            task = TryCast(e.Row.DataItem, BO.TaskListing)

            If ClosedDate IsNot Nothing Then
                ClosedDate.MaxDate = Now
                ClosedDate.ID = ClosedDate.ID & controlIndex.ToString
                AddHandler ClosedDate.TextChanged, AddressOf _grdEditTaskList.HandleRowChanged
            End If
            If EstimatedDueDate IsNot Nothing Then
                EstimatedDueDate.ID = EstimatedDueDate.ID & controlIndex.ToString
                AddHandler EstimatedDueDate.TextChanged, AddressOf _grdEditTaskList.HandleRowChanged
            End If
            If title IsNot Nothing Then
                title.ID = title.ID & controlIndex.ToString
            End If
            If description IsNot Nothing Then
                description.ID = description.ID & controlIndex.ToString
            End If
            If comments IsNot Nothing Then
                comments.ID = comments.ID & controlIndex.ToString
            End If
            If commentsGrid IsNot Nothing Then
                commentsGrid.ID = commentsGrid.ID & controlIndex.ToString
            End If
            If newCommentButton IsNot Nothing Then
                newCommentButton.ID = newCommentButton.ID & controlIndex.ToString
            End If
            If refSite = "IRIS" Or refSite = "TANKS" Then
                isIRISTask = True
            End If
            Dim currentTaskItem As IP.MEAS.BO.TaskItem = Nothing
            If task IsNot Nothing Then
                'task.TaskItemSeqId

                currentTaskItem = taskItemBLL.GetTaskItem(CInt(task.TaskItemSeqId))
                HasOpenSubTasks = (task.OpenSubTasks.ToUpper = "Y")
                'If a Task has Open Subtasks it cannot be closed
                If HasOpenSubTasks Then
                    ClosedDate.Visible = False
                    ddlStatus.Enabled = False
                    ddlPriority.Enabled = False
                    readOnlyClosedDate.Visible = True
                    readOnlyClosedDate.Text = "*** " & IP.Bids.SharedFunctions.LocalizeValue("This task cannot be closed until all of the Sub Tasks have been closed", True) & " ***"
                    ddlStatus.ToolTip = "*** " & IP.Bids.SharedFunctions.LocalizeValue("This task cannot be closed until all of the Sub Tasks have been closed", True) & " ***"
                End If
                If EditButton IsNot Nothing Then
                    'EditButton.OnClientClick = "Javascript:parent.window.location=('" & String.Format(Page.ResolveUrl("~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}"), task.TaskHeaderSeqId, task.TaskItemSeqId) & ";return false;"
                    If isIRISTask Then
                        EditButton.OnClientClick = "Javascript:try{window.parent.DisplayBusy();}catch(e){};try{window.location='../blank.htm';window.location='" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & task.TaskHeaderSeqId & "&TaskNumber=" & task.TaskItemSeqId & "&refsite=" & refSite) & "'}catch(e){} return false" '& "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItemNumber & "&refsite=" & refSite) & "',false);" & "};"
                    Else
                        EditButton.OnClientClick = "Javascript:try{window.parent.DisplayBusy();}catch(e){};try{window.location='../blank.htm';window.parent.location='" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & task.TaskHeaderSeqId & "&TaskNumber=" & task.TaskItemSeqId & "&refsite=" & refSite) & "'}catch(e){} return false" '& "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskDetails.aspx?HeaderNumber=" & TaskHeaderNumber & "&TaskNumber=" & taskItemNumber & "&refsite=" & refSite) & "',false);" & "};"
                    End If
                End If
                If newCommentButton IsNot Nothing Then
                    newCommentButton.OnClientClick = String.Format("showNewCommentRow('comment{0}');return false;", task.TaskItemSeqId)
                End If
                If DeleteButton IsNot Nothing Then
                    Dim myRoles As System.Collections.Generic.List(Of UserRoles) = GeneralTaskTrackerBll.GetUserRoles(IP.Bids.SharedFunctions.GetCurrentUser.Username)
                    DeleteButton.Enabled = False
                    For Each role As UserRoles In myRoles
                        If role.RoleName.ToUpper = "FACILITYADMIN" AndAlso role.PlantCode = task.PlantCode Then
                            DeleteButton.Enabled = True
                            Exit For
                        ElseIf role.RoleName.ToUpper = "SUPPORT" Then
                            DeleteButton.Enabled = True
                            Exit For
                        End If
                    Next
                End If
                If title IsNot Nothing Then
                    title.Text = task.ItemTitle
                    IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "MultitaskEdit_grdEditTaskList_RowCreated.txt", "_grdEditTaskList_RowCreated TITLE: " & title.Text & "  " & DateTime.Now.ToString, False)

                End If
                If description IsNot Nothing Then
                    description.Text = task.ItemDesc
                End If

                If commentsGrid IsNot Nothing Then
                    commentsGrid.DataSource = taskItemBLL.GetTaskItemCommentsList(CInt(task.TaskItemSeqId))
                    commentsGrid.DataBind()
                End If
                If ddlStatus IsNot Nothing Then
                    ddlStatus.SelectedValue = task.StatusSeqId
                End If
                If ddlPriority IsNot Nothing Then
                    ddlPriority.SelectedValue = CStr(task.Priority)
                End If

                If task.ClosedDate IsNot Nothing Then
                    If task.ClosedDate.Length > 0 And CDbl(task.StatusSeqId) <> OpenTaskSeqId Then
                        ClosedDate.StartDate = task.ClosedDate
                        sortableClosedDate.Text = CDate(task.ClosedDate).ToString("yyyyMMdd")
                        taskIsClosed = True
                    End If
                End If

                If EstimatedDueDate IsNot Nothing Then
                    If task.DateCritical = "Y" Then
                        EstimatedDueDate.DateIsCritical = True
                        EstimatedDueDate.Enabled = False
                    Else
                        EstimatedDueDate.DateIsCritical = False
                        EstimatedDueDate.Enabled = True
                    End If
                    If taskIsClosed Then
                        'verify required fields
                        If task.StartDate.Length > 0 Then
                            EstimatedDueDate.Enabled = False
                            readOnlyDueDate.Text = IP.Bids.SharedFunctions.GetDate(task.DueDate)
                            readOnlyDueDate.Visible = True
                            EstimatedDueDate.Attributes.CssStyle.Item("Display") = "none"
                            EstimatedDueDate.Visible = False
                        Else
                            readOnlyDueDate.Visible = False
                            EstimatedDueDate.Visible = True
                        End If
                    End If



                    'For Each key As String In Request.Form.AllKeys
                    '    If key.Contains(EstimatedDueDate.UniqueID & "$") AndAlso Not key.EndsWith("Critical") Then
                    '        If Request.Form(key) IsNot Nothing AndAlso Request.Form(key).Length > 0 Then
                    '            EstimatedDueDate.StartDate = Request.Form(key)
                    '        End If
                    '        Exit For
                    '    End If
                    'Next

                    'For Each key As String In Request.Form.AllKeys.Where(Function(k) k.Contains(ClosedDate.UniqueID & "$") And Not k.Contains("Pricing")).ToArray()
                    '    If key.Contains(ClosedDate.UniqueID & "$") AndAlso Not key.EndsWith("Critical") Then
                    '        'If key.Contains(EstimatedDueDate.UniqueID) Then
                    '        If Request.Form(key) IsNot Nothing AndAlso Request.Form(key).Length > 0 Then
                    '            ClosedDate.StartDate = Request.Form(key)
                    '        End If
                    '        Exit For
                    '    End If
                    'Next

                    'For Each key As String In Request.Form.AllKeys
                    '    If key.Contains(ClosedDate.UniqueID & "$") AndAlso Not key.EndsWith("Critical") Then
                    '        'If key.Contains(EstimatedDueDate.UniqueID) Then
                    '        If Request.Form(key) IsNot Nothing AndAlso Request.Form(key).Length > 0 Then
                    '            ClosedDate.StartDate = Request.Form(key)
                    '        End If
                    '        Exit For
                    '    End If
                    'Next

                End If
                If task.TaskItemLevel = 1 Then e.Row.Font.Bold = True
                If taskTypeIcon IsNot Nothing Then
                    Select Case task.TaskItemLevel
                        Case 1
                            taskTypeIcon.Text = "<span class='glyphicon glyphicon-list-alt btn-lg' aria-hidden='true'></span>"

                        Case 2
                            taskTypeIcon.Text = "<span class='glyphicon glyphicon-chevron-right btn-lg' aria-hidden='true'></span"
                        Case 3
                            taskTypeIcon.Text = "<span class='glyphicon glyphicon-chevron-left btn-lg' aria-hidden='true'></span>"
                        Case Else
                            taskTypeIcon.Text = ""
                    End Select
                End If
                e.Row.BackColor = GetTaskBackColor(task.TaskItemLevel)
            End If


            Dim ResponsiblePerson As User_Controls.AdvancedEmployeeListDropdown = CType(e.Row.FindControl("_ResponsiblePerson"), User_Controls.AdvancedEmployeeListDropdown)
            'If taskIsClosed Then
            '    ResponsiblePerson = Nothing
            'End If
            If ResponsiblePerson IsNot Nothing Then
                ResponsiblePerson.ID = ResponsiblePerson.ID & controlIndex.ToString


                If currentTaskItem IsNot Nothing Then

                    If Request.QueryString("RepairOnly") IsNot Nothing Then
                        If Request.QueryString("RepairOnly").ToString.ToLower = "true" Then
                            If currentTaskItem.RecurringTasks IsNot Nothing Then
                                e.Row.Visible = False
                                _grdEditTaskList.Caption = ""
                                _taskListCount.Text = ""
                                _btnAddNewTask.Visible = False
                                _btnSaveEdits.Visible = False
                            End If
                        End If
                    End If

                    If currentTaskItem.ResponsibleUserName.Trim.Length = 0 Then 'Role
                        ResponsiblePerson.PlantCode = currentTaskItem.ResponsibleRolePlantCode
                        ResponsiblePerson.PopulateEmployeeList(CStr(currentTaskItem.ResponsibleRoleSeqId))

                    Else
                        'Person                       
                        ResponsiblePerson.PopulateEmployeeList(currentTaskItem.ResponsibleUserName)
                    End If
                    currentTaskItem = Nothing
                End If
                For Each key As String In Request.Form.AllKeys

                    If key IsNot Nothing AndAlso key.Contains(ResponsiblePerson.EmployeeListUniqueId) Then
                        If Request.Form(key) IsNot Nothing AndAlso Request.Form(key).Length > 0 Then
                            ResponsiblePerson.PopulateEmployeeList(Request.Form(key))
                        End If
                    End If
                Next

                If taskIsClosed Then
                    If ResponsiblePerson.SelectedValue.Length > 0 Then
                        ResponsiblePerson.Enabled = False
                        readOnlyResponsible.Visible = True
                        readOnlyResponsible.Text = ResponsiblePerson.SelectedText
                        ResponsiblePerson.Visible = False
                    Else
                        readOnlyResponsible.Visible = False
                        ResponsiblePerson.Visible = True
                    End If
                    ddlPriority.Enabled = False
                    title.Attributes.CssStyle.Item("Display") = "none" ' = False
                    readOnlyTitle.Visible = True
                    readOnlyTitle.Text = title.Text
                    readOnlyDescription.Text = description.Text
                    description.Attributes.CssStyle.Item("Display") = "none" ' = False
                End If
            End If
            'If TaskHeaderNumber.Length > 0 Then
            '    e.Row.Cells(0).Visible = False
            'End If

            If task IsNot Nothing AndAlso task.ExternalSourceName.ToLower = "tanks" Then
                If task.ItemTitle.Length > 0 AndAlso task.TankInspectionTypeId.Length > 0 Then
                    title.Attributes.CssStyle.Item("Display") = "none" ' = False
                    readOnlyTitle.Visible = True
                    readOnlyTitle.Text = title.Text
                Else
                    title.Enabled = True
                    readOnlyTitle.Visible = False
                End If
                'If TaskHeaderNumber.Length > 0 Then
                'Dim taskHeader = New TaskHeaderBll(CInt(IP.Bids.SharedFunctions.DataClean(TaskHeaderNumber, CStr(0))))
                'If taskHeader.CurrentTaskHeaderRecord.ExternalSourceName.ToLower = "tanks" Then
                Dim userHasUpdateAccess As Boolean = GeneralTaskTrackerBll.DoesUserHaveUpdateAccess(IP.Bids.SharedFunctions.GetCurrentUser.Username, task.PlantCode)
                If userHasUpdateAccess = False Then
                    ddlStatus.Enabled = False
                    ClosedDate.Enabled = False
                    ddlPriority.Enabled = False
                    EstimatedDueDate.Enabled = False
                    ResponsiblePerson.Enabled = False
                    Me._btnSaveEdits.Enabled = True
                    Me._btnAddNewTask.Enabled = True
                    title.Enabled = False
                End If
            End If
            IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "MultitaskEdit_grdEditTaskList_RowCreated.txt", "_grdEditTaskList_RowCreated End : " & DateTime.Now.ToString, False)

        End If


        'End If
    End Sub

    Public Function GetTaskBackColor(taskItemLevel As Integer) As Color
        Select Case taskItemLevel
            Case 1
                Return Drawing.ColorTranslator.FromHtml("#DDDDDD")
            Case 2
                Return Drawing.ColorTranslator.FromHtml("#FFFFFF")
            Case 3
                Return Drawing.ColorTranslator.FromHtml("#FFFFFF")
            Case Else
                Return Drawing.ColorTranslator.FromHtml("#DDDDDD")
        End Select
    End Function

    Public Function GetTaskBackColorAsHex(taskitemLevel As Integer) As String
        Dim c = GetTaskBackColor(taskitemLevel)
        Return "#" & c.R.ToString("x2") & c.G.ToString("x2") & c.B.ToString("x2")
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Protected Sub _cbShowClosedTasks_CheckedChanged(sender As Object, e As EventArgs) Handles _cbShowClosedTasks.CheckedChanged
        Response.Redirect(AddPageNumberToQueryString(CurrentPage, True), True)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        FilePath = HttpContext.Current.Server.MapPath("~\\TraceLog\\")

        IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "MultitaskEdit_Load.txt", "Page_Load Start : " & DateTime.Now.ToString, True)


        If IP.Bids.SharedFunctions.CausedPostBack(_cbShowClosedTasks.UniqueID) Then
                Response.Redirect(AddPageNumberToQueryString(CurrentPage, True), True)
            End If

    End Sub

    Protected Sub _ddlInspectionTypeList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _ddlInspectionTypeList.SelectedIndexChanged
        Response.Redirect(AddPageNumberToQueryString(CurrentPage, True), True)
    End Sub

    Private Sub ReadOnlyMode()

    End Sub

    Protected Sub _grdEditTaskList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles _grdEditTaskList.Sorting
        SetSortColumnAndDirection(e.SortExpression)
        Response.Redirect(AddPageNumberToQueryString(CurrentPage, True), True)
    End Sub

    Private Sub SetDefaultSort(ByVal defaultColumn As String)
        If Session("sortColumnTasks") Is Nothing Then Session("sortColumnTasks") = defaultColumn
        If Session("sortDirectionTasks") Is Nothing Then Session("sortDirectionTasks") = "Descending"
    End Sub

    Private Sub SetSortColumnAndDirection(ByVal itemToSort As String)
        If itemToSort.ToUpper = Session("sortColumnTasks").ToString.ToUpper Then
            'Column is the same so change the sort order
            Select Case Session("sortDirectionTasks").ToString
                Case SortDirection.Descending.ToString
                    Session("sortDirectionTasks") = SortDirection.Ascending.ToString
                Case Else
                    Session("sortDirectionTasks") = SortDirection.Descending.ToString
            End Select
        Else
            Session("sortColumnTasks") = itemToSort
            Session("sortDirectionTasks") = SortDirection.Ascending.ToString
        End If
    End Sub

    Private Sub EditTaskList_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles _grdEditTaskList.RowDeleting
        Dim taskItemBll As New TaskTrackerItemBll
        Try
            Dim userName As String = IP.Bids.SharedFunctions.GetCurrentUser.Username
            Dim currentTaskItem = taskItemBll.GetTaskItem(CInt(_grdEditTaskList.DataKeys(e.RowIndex).Value))

            taskItemBll.DeleteTaskItem(taskNumber:=CInt(currentTaskItem.TaskItemSeqId), rootTaskNumber:=currentTaskItem.RootTaskItemSeqId, userName:=userName)
            IP.Bids.SharedFunctions.ResponseRedirect(AddPageNumberToQueryString(CurrentPage, False))
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("Deleting Task", , ex, String.Format("Deleting task {0} caused an unexpected error.", (_grdEditTaskList.DataKeys(e.RowIndex).Value)))
        End Try
    End Sub
End Class
