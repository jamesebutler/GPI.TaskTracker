
Imports System.Globalization
Imports ClosedXML.Excel

Partial Class UserControlsReportSelector
    Inherits System.Web.UI.UserControl


#Region "Fields"
    'Private TaskSearch As New TaskSearch
    Private TaskListing As Collections.Specialized.OrderedDictionary   'System.Collections.Generic.List(Of TaskListing) 'Collection 's.Specialized.StringDictionary
    Private _searchMode As TaskSearchMode = TaskSearchMode.View
    Private _AvailableReportParameters As ReportSelectionBll.RequiredReportParameters
    Private _ScreenWidth As Integer = 640
    Private _ScreenHeight As Integer = 800
    Private _currentUser As IP.Bids.UserInfo
    Private _RefSite As String = String.Empty
    Private _taskstatus As TaskTrackerListsBll
    Private currentTab As Integer = 0
#End Region

#Region "Properties"

    Public Property ScreenWidth() As Integer
        Get
            Return _ScreenWidth
        End Get
        Set(ByVal value As Integer)
            _ScreenWidth = value
        End Set
    End Property

    Public Property ScreenHeight() As Integer
        Get
            Return _ScreenHeight
        End Get
        Set(ByVal value As Integer)
            _ScreenHeight = value
        End Set
    End Property
    Public Property SearchMode() As TaskSearchMode
        Get
            Return _searchMode
        End Get
        Set(ByVal value As TaskSearchMode)
            _searchMode = value
        End Set
    End Property

    Public Property AvailableReportParameters() As ReportSelectionBll.RequiredReportParameters
        Get
            Return _AvailableReportParameters
        End Get
        Set(ByVal value As ReportSelectionBll.RequiredReportParameters)
            _AvailableReportParameters = value
        End Set
    End Property

    Public Property RefSite() As String
        Get
            Return _RefSite
        End Get
        Set(ByVal value As String)
            _RefSite = value
        End Set
    End Property
#End Region

#Region "Enum"
    Private Enum SearchView
        Day
        Week
        Month
        HeaderList
        TaskList
        Export
        Report
        Word
        MultiView
    End Enum

    Public Enum TaskSearchMode
        View
        Reporting
    End Enum
#End Region

    Private Structure ReportParms
        Dim Report As String
        Dim Division As String
        Dim Region As String
        Dim PlantName As String
        Dim BusUnit As String
        Dim Area As String
        Dim LineSystem As String
        Dim LineBreak As String
        Dim Status As String
        Dim Safety As String
        Dim Quality As String
        Dim Environmental As String
        Dim Reliability As String
        Dim Project As String
        Dim Health As String
        Dim Outage As String
        Dim DueDate As String
        Dim Overdue As String
        Dim Next7Days As String
        Dim Next14Days As String
        Dim Next30Days As String
        Dim StartMonth As String
        Dim StartDay As String
        Dim Startyear As String
        Dim EndDay As String
        Dim EndMonth As String
        Dim EndYear As String
        Dim CreatedBy As String
        Dim ResponsiblePerson As String
        Dim Role As String
        Dim RoleId As String
        Dim Complaint As String
        Dim Permit As String
        Dim Audit As String
        Dim Inspection As String
        Dim Incident As String
        Dim MOC As String
        Dim TaskType As String
        Dim Activity As String
        Dim Person As String
        Dim TaskListing As String
        Dim SourceSystem As String
        Dim SourceReference As String
        Dim Title As String
        Dim Other As String
        Dim NOV As String
        Dim Order As String
        Dim Citation As String
        Dim ComplianceCertification As String
        Dim CostReduction As String '
        Dim DeliberateImprovement As String
        Dim People As String
        Dim Capital As String
        Dim GMS As String
        Dim GMSElement As String
        Dim SRRLearning As String
        Dim ManufacturingAlert As String
        Dim PSM As String
        Dim NCGTRS As String
        Dim ActivityList As String
        Dim TypeList As String
        Dim TaskHeader As String

        Public Overloads Overrides Function GetHashCode() As Integer
            Throw New NotImplementedException()
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As [Object]) As Boolean
            Throw New NotImplementedException()
        End Function

        Public Shared Operator =(ByVal left As ReportParms, ByVal right As ReportParms) As Boolean
            Throw New NotImplementedException()
        End Operator

        Public Shared Operator <>(ByVal left As ReportParms, ByVal right As ReportParms) As Boolean
            Throw New NotImplementedException()
        End Operator
    End Structure


#Region "Methods"
    Private Sub CleanUpTable(ByVal maxColumns As Integer)
        With Me._tblTaskSearch
            For i As Integer = 0 To .Rows.Count - 1
                Dim j As Integer = Me.GetAvailableCells(.Rows(i), maxColumns)
                If j > 0 Then
                    For k As Integer = 1 To j
                        Dim cell As New TableCell
                        cell.Width = Unit.Percentage(25)
                        .Rows(i).Cells.Add(cell)
                    Next
                End If
            Next
        End With
    End Sub
    Private Sub AddControlsToTable(ByVal control As Object, ByVal lblControl As Label, ByVal colSpan As Integer, Optional ByVal newLineAfterLabel As Boolean = True)
        Try
            With Me._tblTaskSearch
                Dim currentRowIndex As Integer = .Rows.Count - 1
                Dim currentRow As New WebControls.TableRow
                Dim currentCellIndex As Integer
                Dim currentCell As New WebControls.TableCell
                Dim maxColumns As Integer = 4
                Dim availableCells As Integer = 0
                If lblControl IsNot Nothing Then
                    lblControl.EnableViewState = False
                    If lblControl.ID Is Nothing Then
                        lblControl.ID = "lblNone" & control.id
                    End If
                Else
                        lblControl = New Label()
                    lblControl.ID = "lblNone" & control.id
                End If


                '1. Get Current Row
                currentRowIndex = .Rows.Count - 1
                If currentRowIndex < 0 Then
                    'Add a new row
                    currentRow = New TableRow
                    'currentRow.CssClass = "Border"
                    currentRowIndex = .Rows.Add(currentRow)
                Else
                    currentRow = .Rows(currentRowIndex)
                End If

                '2. Get Number of used cells
                Dim usedCells As Integer = 0 '= maxColumns - currentRow.Cells.Count
                For i As Integer = 0 To currentRow.Cells.Count - 1
                    usedCells = usedCells + currentRow.Cells(i).ColumnSpan
                Next
                availableCells = maxColumns - usedCells

                '3. Compare available Cells with requested cells 
                If availableCells >= colSpan Then
                    'Add new cell to current row
                    currentCell.ColumnSpan = colSpan
                    currentCell.Width = Unit.Percentage(25 * colSpan)
                    currentCell.VerticalAlign = VerticalAlign.Top
                    currentCellIndex = .Rows(currentRowIndex).Cells.Add(currentCell)
                Else
                    'Add new cell to new row
                    currentRow = New TableRow
                    'currentRow.CssClass = "Border"
                    currentRowIndex = .Rows.Add(currentRow)
                    currentCell.ColumnSpan = colSpan
                    currentCell.Width = Unit.Percentage(25 * colSpan)
                    currentCell.VerticalAlign = VerticalAlign.Top
                    currentCellIndex = .Rows(currentRowIndex).Cells.Add(currentCell)
                End If

                '4.

                currentCell = currentRow.Cells(currentCellIndex)
                currentCell.ID = "tc_" & lblControl.ID
                If lblControl.Text.Length > 0 Then
                    currentCell.Controls.Add(lblControl)
                    If newLineAfterLabel Then
                        currentCell.Controls.Add(New LiteralControl("<br/>"))
                    End If
                End If
                Dim formDiv As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
                formDiv.Attributes.Add("class", "form-group")
                formDiv.Style.Add("width", Unit.Percentage(90).ToString)
                formDiv.Controls.Add(control)

                currentCell.Controls.Add(formDiv)


            End With
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("AddControlsToTable", , ex)
        End Try
    End Sub
    Private Function GetAvailableCells(ByVal row As WebControls.TableRow, ByVal maxCols As Integer) As Integer
        Dim totalCells As Integer = 0

        If row.Cells.Count >= maxCols Then
            Return 0
        Else
            For i As Integer = 0 To row.Cells.Count - 1
                totalCells = totalCells + row.Cells(i).ColumnSpan
            Next
            Return maxCols - totalCells
        End If

    End Function

    Private Sub PopulateReportTitles(ByVal application As String)
        Dim reportTitleList As System.Collections.Generic.List(Of ReportTitles) = Nothing
        Dim reportSelection As New ReportSelectionBll(application)
        If Not Page.IsPostBack Then
            reportTitleList = reportSelection.GetReportTitles()
            Me._ddlReportTitles.Items.Clear()
            If reportTitleList IsNot Nothing AndAlso reportTitleList.Count > 0 Then
                For Each item As ReportTitles In reportTitleList
                    Me._ddlReportTitles.Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.ReportTitle, True), item.ReportTitle))
                Next

                For Each item As ListItem In _ddlReportTitles.Items
                    item.Attributes.Add("onchange", String.Format("window.location='{0}';", Page.ResolveClientUrl("~/ReportSelection.aspx?rn=" & item.Value)))
                Next
                ' Me._ddlReportTitles.Items.Insert(0, New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Please Select Report Title", True))) ', True), ""))
                If _ddlReportTitles.Items.Count > 0 Then
                    _ddlReportTitles.ClearSelection()
                End If
                Me._ddlReportTitles.SelectedIndex = 0
                PopulateData()
                SetReportDefaults()
            End If
            If Request(_ddlReportTitles.UniqueID) IsNot Nothing Then
                If _ddlReportTitles.Items.FindByValue(Request(_ddlReportTitles.UniqueID)) IsNot Nothing Then
                    _ddlReportTitles.ClearSelection()
                    _ddlReportTitles.Items.FindByValue(Request(_ddlReportTitles.UniqueID)).Selected = True
                End If
            End If
            Me._lblReportSortValues.Visible = False
            Me._ddlReportSortValue.Visible = False
            Me._pnlCalendarSearch.Visible = False
            Me._divReportSearchButtons.Visible = False
            _pnlSearchContainer.Visible = False
            SetReportDefaults()
            'mjp
        End If '  
    End Sub
    ''' <summary>
    ''' Purpose is to populate the required report selection controls based on the selected report.  If a report is not selected then 
    ''' a list of available reports will be displayed
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateReportListingControls(ByVal application As String)
        'Dim reportTitleList As System.Collections.Generic.List(Of ReportTitles) = Nothing
        Dim reportSortValueList As System.Collections.Generic.List(Of ReportSortValues) = Nothing
        Dim reportParameterList As System.Collections.Generic.List(Of ReportParameters) = Nothing
        Dim reportSelection As New ReportSelectionBll(application)
        Dim availableReportParms As ReportSelectionBll.RequiredReportParameters

        If reportSelection IsNot Nothing Then
            'If Not Page.IsPostBack Then
            '    reportTitleList = reportSelection.GetReportTitles()
            '    Me._ddlReportTitles.Items.Clear()
            '    If reportTitleList IsNot Nothing AndAlso reportTitleList.Count > 0 Then
            '        For Each item As ReportTitles In reportTitleList
            '            Me._ddlReportTitles.Items.Add(New ListItem(item.ReportTitle, item.ReportTitle))
            '        Next

            '        Me._ddlReportTitles.Items.Insert(0, New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Please Select Report Title"))) ', True), ""))
            '        Me._ddlReportTitles.SelectedIndex = 0
            '    End If
            '    Me._lblReportSortValues.Visible = False
            '    Me._ddlReportSortValue.Visible = False
            '    Me._pnlCalendarSearch.Visible = False
            '    Me._divReportSearchButtons.Visible = False
            '    Me._divViewSearchButtons.Visible = False
            '    'mjp
            'End If '        Else
            If Me._ddlReportTitles.SelectedIndex >= 0 Then
                reportSortValueList = reportSelection.GetReportSortValues(Me._ddlReportTitles.SelectedValue)
                If reportSortValueList IsNot Nothing AndAlso reportSortValueList.Count > 0 Then
                    Dim currentSortValue As String = _ddlReportSortValue.SelectedValue
                    _ddlReportSortValue.Items.Clear()
                    For Each item As ReportSortValues In reportSortValueList
                        Me._ddlReportSortValue.Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.ReportSortValue, True), item.ReportName))
                    Next
                    If Me._ddlReportSortValue.Items.Count > 0 Then
                        If Me._ddlReportSortValue.Items.Count = 1 Then
                            Me._lblReportSortValues.Visible = False
                            Me._ddlReportSortValue.Visible = False
                        Else
                            Me._lblReportSortValues.Visible = True
                            Me._ddlReportSortValue.Visible = True
                            _lblReportSortValues.Text = IP.Bids.SharedFunctions.LocalizeValue("Sort Order", True)
                        End If
                        If _ddlReportSortValue.Items.FindByValue(currentSortValue) IsNot Nothing Then
                            _ddlReportSortValue.Items.FindByValue(currentSortValue).Selected = True
                        Else
                            Me._ddlReportSortValue.SelectedIndex = 0
                        End If
                        'Me._ddlReportSortValue.SelectedIndex = 0
                    Else
                        Me._lblReportSortValues.Visible = False
                        Me._ddlReportSortValue.Visible = False
                    End If
                Else
                    Me._lblReportSortValues.Visible = False
                    Me._ddlReportSortValue.Visible = False
                End If

                reportParameterList = reportSelection.GetReportParameters(Me._ddlReportTitles.SelectedValue)
                If reportParameterList IsNot Nothing AndAlso reportParameterList.Count > 0 Then
                    'Hide report options until requested
                    ' _taskSearch.Visible = False

                    For Each item As ReportParameters In reportParameterList
                        'Build Report Selection Screen
                        Select Case item.ReportParameterType.ToLower
                            Case "site"
                                availableReportParms.Site = True
                            Case "report type"
                                _lblReportSortValues.Text = IP.Bids.SharedFunctions.LocalizeValue("Report Type", True)
                                availableReportParms.ReportType = True
                            Case "task type"
                                availableReportParms.Types = True
                            Case "created by"
                                availableReportParms.CreatedBy = True
                            Case "task activity"
                                availableReportParms.Activity = True
                            Case "due date"
                                availableReportParms.EstimatedDueDate = True
                            Case "calendar"
                                availableReportParms.DueDate = True
                            Case "task status"
                                availableReportParms.TaskStatus = True
                            Case "responsible"
                                availableReportParms.Responsible = True
                            Case "header date"
                                availableReportParms.HeaderDate = True
                            Case "task listing"
                                availableReportParms.TaskListing = True
                            Case "source system"
                                availableReportParms.SourceSystem = True
                            Case "title"
                                availableReportParms.Title = True
                            Case "role"
                                availableReportParms.Role = True
                            Case "element"
                                availableReportParms.GMSElement = True
                            Case "task header"
                                availableReportParms.TaskHeader = True
                        End Select
                    Next
                    _lblReportSelections.Visible = True
                    'Me._trReportSelection.Visible = True
                    '_taskSearch.Visible = True
                    Me._pnlCalendarSearch.Visible = True
                    AvailableReportParameters = availableReportParms
                    DisplaySearchSelections()
                End If
            Else
                _lblReportSelections.Visible = False
                Me._pnlCalendarSearch.Visible = False
                Me._divReportSearchButtons.Visible = False
                _pnlSearchContainer.Visible = False
                'Me._trReportSelection.Visible = False
                ' _taskSearch.Visible = False
            End If

        End If


        'End If
        Me.ReselectControls()
    End Sub

    Public Sub DisplaySearchSelections()
        Select Case SearchMode
            Case TaskSearchMode.View
                _pnlReportCriteria.Visible = False
                Me._tabSearchResults.Visible = False 'True
                _tabSearchTabs.Visible = False
                _pnlReportSelection.Visible = False
                Me._divReportSearchButtons.Visible = False
                _pnlSearchContainer.Visible = True
                Me._cpeSearchSelections.Enabled = True
                Me._cpeSearchSelections.CollapsedText = "+ <span class='glyphicon glyphicon-filter' aria-hidden='True'></span>&nbsp;" & IP.Bids.SharedFunctions.LocalizeValue("Show Search Criteria", True)
                Me._cpeSearchSelections.ExpandedText = "- <span Class='glyphicon glyphicon-filter' aria-hidden='True'></span>&nbsp;" & IP.Bids.SharedFunctions.LocalizeValue("Hide Search Criteria", True)
                'Me._cpeReportSelections.Enabled = False
                Me._ReportingSite.Visible = True
                Me._lblActivity.Visible = True
                Me._cblActivity.Visible = True
                Me._lblIncidentType.Visible = True
                Me._cblIncidentType.Visible = True
                Me._lblTaskStatus.Visible = True
                Me._rblTaskStatus.Visible = False
                Me._cblTaskStatus.Visible = True
                'Me._lblCreatedBy.Visible = True
                Me._CreatedBy.Visible = True
                Me._CreatedBy.UserMode = User_Controls.AdvancedEmployeeListDropdown.UserModes.UsersOnly ' UserControlsEmployeeList2.UserModes.UsersOnly
                'Me._lblResponsiblePerson.Visible = True
                Me._ResponsiblePerson.Visible = True
                Me._ResponsiblePerson.UserMode = User_Controls.AdvancedEmployeeListDropdown.UserModes.UsersAndRoles ' UserControlsEmployeeList2.UserModes.UsersAndRoles
                Me._ddlSourceSystem.Visible = True
                Me._lblIncidentEventNumber.Visible = True
                Me._txtIncidentEventNumber.Visible = True
                Me._lblHighLevelSecurity.Visible = True
                Me._cbHighLevelSecurity.Visible = True
                _lblClosedDate.Visible = True
                Me._dtClosedDate.Visible = True
                Me._lblDueDate.Visible = True
                Me._dtDueDate.Visible = True
                Me._lblHeaderDate.Visible = True
                Me._dtHeaderDate.Visible = True
                Me._lblTitle.Visible = True
                Me._txtTitle.Visible = True
                Me._lblDescription.Visible = True
                Me._txtDescription.Visible = True
                _rblReportType.Visible = False
                _lblReportType.Visible = False
                _cblEstimatedDueDateRange.Visible = False
                _lblEstimatedDueDateRange.Visible = False
                _lblTaskListing.Visible = False
                _rblTaskListing.Visible = False

                Me._cpeSearchSelections.Collapsed = False
                Me._cpeSearchSelections.ClientState = "false"
            Case TaskSearchMode.Reporting
                _pnlReportCriteria.Visible = True
                _btnSaveSearchCriteria.Visible = False
                _btnResetSearchCriteria.Visible = False
                'Me._cpeReportSelections.Enabled = True
                Me._tabSearchResults.Visible = False
                _tabSearchTabs.Visible = False
                _pnlReportSelection.Visible = True
                Me._divReportSearchButtons.Visible = True
                _pnlSearchContainer.Visible = False
                Me._cpeSearchSelections.Enabled = False
                _cblEstimatedDueDateRange.Visible = True

                If _ddlReportTitles.SelectedIndex >= 0 Then

                    With Me.AvailableReportParameters
                        'Me._tblSiteInfo.Visible = False
                        _tblTaskSearch.Visible = True
                        _tblTaskSearch.Rows.Clear()

                        Me._ReportingSite.Visible = .Site
                        'If .Site Then AddControlsToTable(_ReportingSite, New Label(), 4)

                        Me._lblSubElement.Visible = .GMSElement
                        Me._ddlSubElement.Visible = .GMSElement
                        If .GMSElement Then
                            AddControlsToTable(_ddlSubElement, _lblSubElement, 2, True)
                        End If

                        'AddControlsToTable(_cal, New Label(), 2)
                        Me._lblIncidentType.Visible = .Types
                        Me._cblIncidentType.Visible = .Types
                        If .Types Then
                            'Me._cblIncidentType.Text = Nothing
                            AddControlsToTable(_cblIncidentType, _lblIncidentType, 2)
                            If _cblIncidentType.SelectedListItems.Count = 0 Then
                                _cblIncidentType.CheckAll()
                            End If
                        End If
                        Me._lblActivity.Visible = .Activity
                        Me._cblActivity.Visible = .Activity
                        If .Activity Then
                            'Me._cblActivity.Text = ""
                            AddControlsToTable(_cblActivity, _lblActivity, 2)
                            If _cblActivity.SelectedListItems.Count = 0 Then
                                _cblActivity.CheckAll()
                            End If
                        End If

                        Me._lblTaskStatus.Visible = .TaskStatus
                        Me._cblTaskStatus.Visible = False
                        Me._rblTaskStatus.Visible = .TaskStatus
                        Me._rblTaskStatus.AutoPostBack = True
                        If .TaskStatus Then
                            AddControlsToTable(_rblTaskStatus, _lblTaskStatus, 2)
                        End If

                        Me._lblDueDate.Visible = .DueDate
                        Me._dtDueDate.Visible = .DueDate
                        _dtDueDate.ShowDateRange = True
                        _dtDueDate.DateRange = UserControlsJQDateRange.JQDateRange.ClearDate

                        'MJP - 7/21/2011 Commented this out to fix Estimated Due Date Range
                        Dim phDueDate As New PlaceHolder
                        phDueDate.ID = "_phDueDate"
                        phDueDate.EnableViewState = False
                        With phDueDate
                            If AvailableReportParameters.DueDate Then
                                .Controls.Add(_dtDueDate)
                            End If
                            If AvailableReportParameters.EstimatedDueDate Then
                                .Controls.Add(_cblEstimatedDueDateRange)
                            End If
                        End With
                        _dtDueDate.ShowReportDateRange = (Request.Form(Me._rblTaskStatus.UniqueID) = "Open")
                        If Request.Form(Me._rblTaskStatus.UniqueID) IsNot Nothing Then
                            If _rblTaskStatus.Items.FindByValue(Request.Form(Me._rblTaskStatus.UniqueID)) IsNot Nothing Then
                                _rblTaskStatus.Items.FindByValue(Request.Form(Me._rblTaskStatus.UniqueID)).Selected = True
                            End If
                        End If
                        If _rblTaskStatus.SelectedValue = "All" Or _rblTaskStatus.SelectedValue = "Complete" Or (Request.Form(Me._rblTaskStatus.UniqueID) IsNot Nothing AndAlso (Request.Form(Me._rblTaskStatus.UniqueID) = "All") Or (Request.Form(Me._rblTaskStatus.UniqueID) = "Complete")) Then
                            If .DueDate Then
                                AddControlsToTable(phDueDate, _lblDueDate, 2)
                                'AddControlsToTable(_cblEstimatedDueDateRange, _lblDueDate, 2)
                                _cblEstimatedDueDateRange.Visible = True
                                _cblEstimatedDueDateRange.Style.Item("Display") = "none"
                                _dtDueDate.Visible = True
                                'Me.Controls.Remove(_cblEstimatedDueDateRange)                        
                            End If
                        ElseIf _rblTaskStatus.SelectedValue = "Open" Or ((Request.Form(Me._rblTaskStatus.UniqueID) = "Open")) Then
                            If .EstimatedDueDate OrElse .DueDate Then
                                _cblEstimatedDueDateRange.Visible = False
                                _cblEstimatedDueDateRange.Style.Item("Display") = "block"
                                AddControlsToTable(phDueDate, _lblDueDate, 2)
                                'AddControlsToTable(_cblEstimatedDueDateRange, _lblDueDate, 2)
                                If _cblEstimatedDueDateRange.SelectedValue.Length = 0 Then
                                    _cblEstimatedDueDateRange.CheckAll()
                                End If
                                _dtDueDate.ShowReportDateRange = True
                                ' If .DueDate Then _dtDueDate.ShowReportDateRange = True
                                _dtDueDate.Visible = True

                                'Me.Controls.Remove(_dtDueDate)                        

                            Else
                                'Me.Controls.Remove(_cblEstimatedDueDateRange)
                            End If
                        ElseIf _rblTaskStatus.SelectedValue = "Overdue" Or ((Request.Form(Me._rblTaskStatus.UniqueID) = "Overdue")) Then
                            If .EstimatedDueDate Then
                                _cblEstimatedDueDateRange.Visible = True
                                _cblEstimatedDueDateRange.Style.Item("Display") = "none"
                                _lblDueDate.Style.Item("Display") = "none"
                                AddControlsToTable(phDueDate, _lblDueDate, 2)
                                'AddControlsToTable(_cblEstimatedDueDateRange, _lblDueDate, 2)
                                If _cblEstimatedDueDateRange.Items.FindByValue("Overdue") IsNot Nothing Then
                                    _cblEstimatedDueDateRange.ClearSelection()
                                    _cblEstimatedDueDateRange.Items.FindByValue("Overdue").Selected = True
                                End If
                                _dtDueDate.Visible = False
                                'Me.Controls.Remove(_dtDueDate)                        

                            Else
                                'Me.Controls.Remove(_cblEstimatedDueDateRange)
                            End If
                        Else
                            If .DueDate Then
                                _lblDueDate.Text = IP.Bids.SharedFunctions.LocalizeValue("Closed Date", True)
                                AddControlsToTable(phDueDate, _lblDueDate, 2)
                                'AddControlsToTable(_cblEstimatedDueDateRange, _lblDueDate, 2)
                                _cblEstimatedDueDateRange.Visible = True
                                _cblEstimatedDueDateRange.Style.Item("Display") = "none"
                                _dtDueDate.Visible = True
                                'Me.Controls.Remove(_cblEstimatedDueDateRange)
                            Else
                                'Me.Controls.Remove(_dtDueDate)
                            End If
                        End If

                        'Me._lblCreatedBy.Visible = .CreatedBy

                        If RefSite.ToUpper = "IRIS" Then
                            Me._CreatedBy.Visible = .CreatedBy
                            If .CreatedBy Then
                                '_CreatedBy.EnableViewState = False
                                _CreatedBy.PopulateEmployeeList()
                                '_lblCreatedBy.Text = ""
                                AddControlsToTable(_CreatedBy, Nothing, 2, False)
                            End If
                            'Me._lblResponsiblePerson.Visible = .Responsible
                            Me._ResponsiblePerson.Visible = .Responsible
                            If .Responsible Then
                                _ResponsiblePerson.PopulateEmployeeList()
                                '_ResponsiblePerson.EnableViewState = False
                                '_lblResponsiblePerson.Text = ""
                                AddControlsToTable(_ResponsiblePerson, Nothing, 2, False)
                            End If

                            Me._ddlSourceSystem.Visible = .SourceSystem
                            _lblIncidentEventSource.Visible = .SourceSystem
                            Me._lblIncidentEventNumber.Visible = .SourceSystem
                            Me._txtIncidentEventNumber.Visible = .SourceSystem
                            If .SourceSystem Then
                                AddControlsToTable(_ddlSourceSystem, _lblIncidentEventSource, 1)
                                AddControlsToTable(_txtIncidentEventNumber, _lblIncidentEventNumber, 1)
                            End If
                        Else
                            Me._CreatedBy.Visible = .CreatedBy
                            If .CreatedBy Then
                                '_CreatedBy.EnableViewState = False
                                _CreatedBy.PopulateEmployeeList()
                                AddControlsToTable(_CreatedBy, Nothing, 1, False)
                            End If
                            'Me._lblResponsiblePerson.Visible = .Responsible
                            Me._ResponsiblePerson.Visible = .Responsible
                            If .Responsible Then
                                _ResponsiblePerson.PopulateEmployeeList()
                                '_ResponsiblePerson.EnableViewState = False
                                AddControlsToTable(_ResponsiblePerson, Nothing, 1, False)
                            End If
                            Me._RoleList.Visible = .Role
                            If .Role Then
                                _RoleList.PopulateEmployeeList()
                                '_ResponsiblePerson.EnableViewState = False
                                AddControlsToTable(_RoleList, Nothing, 1, False)
                            End If

                            Me._ddlSourceSystem.Visible = .SourceSystem
                            _lblIncidentEventSource.Visible = .SourceSystem
                            Me._lblIncidentEventNumber.Visible = .SourceSystem
                            Me._txtIncidentEventNumber.Visible = .SourceSystem
                            If .SourceSystem Then
                                AddControlsToTable(_ddlSourceSystem, _lblIncidentEventSource, 1)
                                AddControlsToTable(_txtIncidentEventNumber, _lblIncidentEventNumber, 1)
                            End If
                        End If




                        Me._lblHighLevelSecurity.Visible = .HighLevelSecurity
                        Me._cbHighLevelSecurity.Visible = .HighLevelSecurity
                        If .HighLevelSecurity Then
                            AddControlsToTable(_cbHighLevelSecurity, _lblHighLevelSecurity, 1)
                        End If

                        _lblClosedDate.Visible = .ClosedDate
                        Me._dtClosedDate.Visible = .ClosedDate
                        If .ClosedDate Then
                            AddControlsToTable(_dtClosedDate, _lblClosedDate, 2)
                        Else
                            Me.Controls.Remove(_dtClosedDate)
                        End If

                        Me._lblHeaderDate.Visible = .HeaderDate
                        Me._dtHeaderDate.Visible = .HeaderDate
                        Me._dtHeaderDate.EnableViewState = False
                        If .HeaderDate Then

                            AddControlsToTable(_dtHeaderDate, _lblHeaderDate, 2)
                        Else
                            Me.Controls.Remove(_dtHeaderDate)
                        End If

                        Me._lblTitle.Visible = .Title
                        Me._txtTitle.Visible = .Title
                        If .Title Then
                            AddControlsToTable(_txtTitle, _lblTitle, 2)
                        End If

                        Me._lblDescription.Visible = .Description
                        Me._txtDescription.Visible = .Description
                        If .Description Then
                            AddControlsToTable(_txtDescription, _lblDescription, 2)
                        End If

                        Me._lblHeaderNumber.Visible = .TaskHeader
                        Me._txtHeaderNumber.Visible = .TaskHeader
                        If .TaskHeader Then
                            AddControlsToTable(_txtHeaderNumber, _lblHeaderNumber, 2)
                        End If

                        _lblTaskListing.Visible = .TaskListing
                        _rblTaskListing.Visible = .TaskListing
                        If .TaskListing Then
                            AddControlsToTable(_rblTaskListing, _lblTaskListing, 1)
                        End If
                        If .TaskHeader Then
                            AddControlsToTable(_txtHeaderNumber_FilteredTextBoxExtender, New Label(), 4)
                        End If
                        '_rblReportType.Visible = .ReportType
                        '_lblReportType.Visible = .ReportType
                        'AddControlsToTable(_rblReportType, _lblReportType, 2)

                        '_cblEstimatedDueDateRange.Visible = .EstimatedDueDate
                        '_lblEstimatedDueDateRange.Visible = .EstimatedDueDate
                        'If .EstimatedDueDate Then AddControlsToTable(_cblEstimatedDueDateRange, _lblEstimatedDueDateRange, 2)
                        CleanUpTable(4)
                    End With
                End If
                ReselectControls()
        End Select
    End Sub
    Private Sub ReselectControls()
        If Me.SearchMode = TaskSearchMode.View Then
            Dim taskSearch As TaskListingBll = Nothing

            Dim defaults As RIUserDefaults.CurrentUserDefaults = Nothing
            If IP.Bids.SharedFunctions.GetCurrentUser IsNot Nothing AndAlso IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults IsNot Nothing Then
                defaults = IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults
            End If

            If Page.IsPostBack = False Then
                Dim searchXml = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.TaskSearchCriteria, Nothing)
                taskSearch = TaskListingBll.LoadXML(searchXml)
            End If

            If taskSearch Is Nothing Then
                'User does not have a profile so set the default dates
                Me._dtDueDate.EndDate = Now.AddMonths(1).ToShortDateString
                Me._dtDueDate.StartDate = New Date(Now.Year(), 1, 1).ToShortDateString
            End If
            Dim userRequestedToReturnToTaskListing As Boolean = False
            If Request.QueryString("Search") IsNot Nothing Then
                Dim performSearchQs = Request.QueryString("Search")
                If CBool(performSearchQs) = True Then
                    userRequestedToReturnToTaskListing = True
                End If
            End If

            'Get value from Querystring
            If userRequestedToReturnToTaskListing Or taskSearch Is Nothing Then
                If Session.Item("TaskSearchCriteria") IsNot Nothing Then
                    taskSearch = Session.Item("TaskSearchCriteria")
                End If
            End If

            If taskSearch IsNot Nothing Then
                With taskSearch
                    If .Activity.Length > 0 Then
                        Me._cblActivity.ClearSelection()
                        Me._cblActivity.SelectedValue = .Activity
                    End If
                    If .Type.Length > 0 Then
                        Me._cblIncidentType.ClearSelection()
                        Me._cblIncidentType.SelectedValue = .Type
                    End If
                    If .Description.Length > 0 Then
                        Me._txtDescription.Text = .Description
                    End If
                    If .Title.Length > 0 Then
                        Me._txtTitle.Text = .Title
                    End If
                    If .SourceSytemRef.Length > 0 Then
                        Me._txtIncidentEventNumber.Text = .SourceSytemRef
                    End If
                    If .SourceSystem.Length > 0 Then
                        'Note: FindByValue does a case sensitive search
                        If Me._ddlSourceSystem.Items.FindByValue(.SourceSystem) IsNot Nothing Then
                            Me._ddlSourceSystem.ClearSelection()
                            Me._ddlSourceSystem.Items.FindByValue(.SourceSystem).Selected = True
                        End If
                    End If
                    If .TaskStatus.Length > 0 Then
                        Me._cblTaskStatus.ClearSelection()
                        Me._cblTaskStatus.SelectedValue = .TaskStatus.Replace(",", "|")
                    End If
                    If .HeaderSeqID.Length > 0 Then
                        Me._txtHeaderNumber.Text = .HeaderSeqID
                    End If
                    If .HeaderFromDate.Length > 0 AndAlso IsDate(.HeaderFromDate) Then
                        Me._dtHeaderDate.StartDate = .HeaderFromDate
                    End If
                    If .HeaderToDate.Length > 0 AndAlso IsDate(.HeaderToDate) Then
                        Me._dtHeaderDate.EndDate = .HeaderToDate
                    End If
                    If IsNumeric(.HeaderDateRange) AndAlso CInt(.HeaderDateRange) > -1 Then
                        If IsNumeric(.HeaderDateRange) Then _dtHeaderDate.DateRange = CInt(.HeaderDateRange)
                    End If
                    If .DueDateFrom.Length > 0 AndAlso IsDate(.DueDateFrom) Then
                        Me._dtDueDate.StartDate = .DueDateFrom
                    End If
                    If .DueDateTo.Length > 0 AndAlso IsDate(.DueDateTo) Then
                        Me._dtDueDate.EndDate = .DueDateTo
                    End If
                    If IsNumeric(.DueDateDateRange) AndAlso CInt(.DueDateDateRange) > -1 Then
                        If IsNumeric(.DueDateDateRange) Then _dtDueDate.DateRange = CInt(.DueDateDateRange)
                    End If
                    If .ClosedDateFrom.Length > 0 AndAlso IsDate(.ClosedDateFrom) Then
                        Me._dtClosedDate.StartDate = .ClosedDateFrom
                    End If
                    If .ClosedDateTo.Length > 0 AndAlso IsDate(.ClosedDateTo) Then
                        Me._dtClosedDate.EndDate = .ClosedDateTo
                    End If
                    If IsNumeric(.CloseDateRange) AndAlso CInt(.CloseDateRange) > -1 Then
                        If IsNumeric(.CloseDateRange) Then _dtClosedDate.DateRange = CInt(.CloseDateRange)
                    End If
                    'If .Responsible.Length > 0 Then

                    If IsNumeric(.Responsible) Then 'Role
                        Me._ResponsiblePerson.PlantCode = .ResponsiblePlantCode
                        Me._ResponsiblePerson.PopulateEmployeeList(.Responsible)
                        'Me._ResponsiblePerson.DefaultUserName = .Responsible
                    Else
                        Me._ResponsiblePerson.PopulateEmployeeList(.Responsible)
                        'Me._ResponsiblePerson.DefaultUserName = .Responsible
                    End If
                    'End If
                    If .CreatedBy.Length > 0 Then
                        Me._CreatedBy.PlantCode = .CreatedByPlantCode
                        Me._CreatedBy.DefaultUserName = .CreatedBy
                        Me._CreatedBy.PopulateEmployeeList()
                    Else
                        Me._CreatedBy.PlantCode = .CreatedByPlantCode
                        Me._CreatedBy.PopulateEmployeeList()
                    End If
                    If .SecurityLevel.Length > 0 Then
                        If .SecurityLevel = "Y" Then
                            Me._cbHighLevelSecurity.Checked = True
                        Else
                            Me._cbHighLevelSecurity.Checked = False
                        End If
                    End If

                    If .Division.Length > 0 Then
                        Me._ReportingSite.DefaultDivision = .Division
                    End If
                    If .Region.Length > 0 Then
                        Me._ReportingSite.DefaultRegion = .Region
                    End If
                    If .PlantCode.Length > 0 Then
                        Me._ReportingSite.DefaultFacility = .PlantCode
                    End If
                    If .BusinessUnit.Length > 0 Then
                        Me._ReportingSite.DefaultBusinessUnit = .BusinessUnit
                    End If
                    If .Area.Length > 0 Then
                        Me._ReportingSite.DefaultArea = .Area
                    End If
                    If .Line.Length > 0 Then
                        Me._ReportingSite.DefaultLine = .Line
                    End If
                    If .Machine.Length > 0 Then
                        Me._ReportingSite.DefaultLineBreak = .Machine
                    End If
                    ReselectActiveTab(taskSearch)
                    'If .TaskSearchView.Length > 0 Then
                    '    If Me._ddlSearchOptions.Items.FindByValue(.TaskSearchView) IsNot Nothing Then
                    '        _ddlSearchOptions.ClearSelection()
                    '        Me._ddlSearchOptions.Items.FindByValue(.TaskSearchView).Selected = True
                    '    End If
                    'End If
                    ''If Request.Form(Me._txtResponsiblePerson.UniqueID) IsNot Nothing Then
                    ''    Me._txtResponsiblePerson.Text = Request.Form(Me._txtResponsiblePerson.UniqueID)
                    ''End If
                    'If Request.Form(Me._txtTitle.UniqueID) IsNot Nothing Then
                    '    Me._txtTitle.Text = Request.Form(Me._txtTitle.UniqueID)
                    'End If
                    'If Request.Form(Me._rblTaskStatus.UniqueID) IsNot Nothing Then
                    '    If _rblTaskStatus.Items.FindByValue(Request.Form(Me._rblTaskStatus.UniqueID)) IsNot Nothing Then
                    '        _rblTaskStatus.Items.FindByValue(Request.Form(Me._rblTaskStatus.UniqueID)).Selected = True
                    '    End If
                    'End If
                    'If Request.Form(Me._cbHighLevelSecurity.UniqueID) IsNot Nothing Then
                    '    If Request.Form(Me._cbHighLevelSecurity.UniqueID) = "on" Then
                    '        Me._cbHighLevelSecurity.Checked = True
                    '    Else
                    '        Me._cbHighLevelSecurity.Checked = False
                    '    End If
                    'End If
                    'If Request.Form(Me._dtDueDate.UniqueID) IsNot Nothing Then
                    '    Me._dtDueDate.StartDate = Request.Form(Me._dtDueDate.UniqueID)
                    'End If
                    'If Request.Form(Me._cblEstimatedDueDateRange.UniqueID) IsNot Nothing Then
                    '    Me._cblEstimatedDueDateRange.SelectedValue = Request.Form(Me._cblEstimatedDueDateRange.UniqueID)
                    'End If
                End With
            End If
        Else
            ReselectReportControls()
        End If
    End Sub

    Public Sub ReselectReportControls()
        If Request.Form(Me._cblActivity.UniqueID) IsNot Nothing Then
            Me._cblActivity.SelectedValue = Request.Form(Me._cblActivity.UniqueID)
        End If
        If Request.Form(Me._cblIncidentType.UniqueID) IsNot Nothing Then
            Me._cblIncidentType.SelectedValue = Request.Form(Me._cblIncidentType.UniqueID)
        End If
        'If Request.Form(Me._txtCreatedBy.UniqueID) IsNot Nothing Then
        '    Me._txtCreatedBy.Text = Request.Form(Me._txtCreatedBy.UniqueID)
        'End If
        If Request.Form(Me._txtDescription.UniqueID) IsNot Nothing Then
            Me._txtDescription.Text = Request.Form(Me._txtDescription.UniqueID)
        End If
        If Request.Form(Me._txtIncidentEventNumber.UniqueID) IsNot Nothing Then
            Me._txtIncidentEventNumber.Text = Request.Form(Me._txtIncidentEventNumber.UniqueID)
        End If
        'If Request.Form(Me._txtResponsiblePerson.UniqueID) IsNot Nothing Then
        '    Me._txtResponsiblePerson.Text = Request.Form(Me._txtResponsiblePerson.UniqueID)
        'End If
        If Request.Form(Me._txtTitle.UniqueID) IsNot Nothing Then
            Me._txtTitle.Text = Request.Form(Me._txtTitle.UniqueID)
        End If
        If Request.Form(Me._rblTaskStatus.UniqueID) IsNot Nothing Then
            If _rblTaskStatus.Items.FindByValue(Request.Form(Me._rblTaskStatus.UniqueID)) IsNot Nothing Then
                _rblTaskStatus.ClearSelection()
                _rblTaskStatus.Items.FindByValue(Request.Form(Me._rblTaskStatus.UniqueID)).Selected = True
            End If
        End If
        If Request.Form(Me._cbHighLevelSecurity.UniqueID) IsNot Nothing Then
            If Request.Form(Me._cbHighLevelSecurity.UniqueID) = "on" Then
                Me._cbHighLevelSecurity.Checked = True
            Else
                Me._cbHighLevelSecurity.Checked = False
            End If
        End If
        If Request.Form(Me._dtDueDate.UniqueID) IsNot Nothing Then
            Me._dtDueDate.StartDate = Request.Form(Me._dtDueDate.UniqueID)
        End If
        If Request.Form(Me._dtHeaderDate.UniqueID) IsNot Nothing Then
            Me._dtHeaderDate.StartDate = Request.Form(Me._dtHeaderDate.UniqueID)
        End If
        If Request.Form(Me._cblEstimatedDueDateRange.UniqueID) IsNot Nothing Then
            Me._cblEstimatedDueDateRange.SelectedValue = Request.Form(Me._cblEstimatedDueDateRange.UniqueID)
        End If
        If Request.Form(Me._ddlSourceSystem.UniqueID) IsNot Nothing Then
            If Me._ddlSourceSystem.Items.FindByValue(Request.Form(Me._ddlSourceSystem.UniqueID)) IsNot Nothing Then
                Me._ddlSourceSystem.ClearSelection()
                Me._ddlSourceSystem.Items.FindByValue(Request.Form(Me._ddlSourceSystem.UniqueID)).Selected = True
            End If
        End If
        If Request.Form(Me._ddlSubElement.UniqueID) IsNot Nothing Then
            If Me._ddlSubElement.Items.FindByValue(Request.Form(Me._ddlSubElement.UniqueID)) IsNot Nothing Then
                Me._ddlSubElement.ClearSelection()
                Me._ddlSubElement.Items.FindByValue(Request.Form(Me._ddlSubElement.UniqueID)).Selected = True
            End If
        End If
        If Request.Form(Me._txtIncidentEventNumber.UniqueID) IsNot Nothing Then
            _txtIncidentEventNumber.Text = Request.Form(Me._txtIncidentEventNumber.UniqueID)
        End If
        Dim defaults As RIUserDefaults.CurrentUserDefaults = Nothing
        If IP.Bids.SharedFunctions.GetCurrentUser IsNot Nothing AndAlso IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults IsNot Nothing Then
            defaults = IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults
        End If

        'If defaults IsNot Nothing Then
        '    Me._ResponsiblePerson.PlantCode = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode, Me._ReportingSite.FacilityValue)
        '    Me._CreatedBy.PlantCode = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode, Me._ReportingSite.FacilityValue)

        '    If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.PlantCode) Then
        '        Me._ReportingSite.DefaultFacility = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode, Me._ReportingSite.FacilityValue)
        '    End If
        'End If

        If Not Page.IsPostBack AndAlso RefSite.ToUpper = "IRIS" Then
            'Dim taskHeaderNumber As String = Request.QueryString("HeaderNumber")
            'Dim taskHeader As TaskHeaderBll
            'taskHeader = New TaskHeaderBll(CInt(IP.Bids.SharedFunctions.DataClean(taskHeaderNumber, CStr(0))))
            'If taskHeader IsNot Nothing AndAlso taskHeader.CurrentTaskHeaderRecord IsNot Nothing Then                    
            'Me._ReportingSite.DefaultFacility = taskHeader.CurrentTaskHeaderRecord.PlantCode
            'End If
            If defaults IsNot Nothing Then
                If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.PlantCode) Then
                    Me._ReportingSite.DefaultFacility = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
                End If
            End If
            'Deliberate Improvement(ME),Environmental,Health & Safety,Outage,Project/Other,Quality,Reliability
            Me._cblIncidentType.SelectedValue = "Health & Safety,Environmental"
            Me._ddlSourceSystem.ClearSelection()
            Me._ddlSourceSystem.Items.FindByValue(IP.Bids.SharedFunctions.SourceSystemID.IRIS).Selected = True
            _rblTaskStatus.RepeatColumns = 3
            Me._cblActivity.RepeatColumns = 2
            Me._cblIncidentType.RepeatColumns = 2
            Me._dtDueDate.ShowDateRange = False
        End If
        'If Not Page.IsPostBack Then
        '    Dim taskSearch As New TaskListingBLL

        '    If Session.Item("TaskSearchCriteria") IsNot Nothing Then
        '        taskSearch = Session.Item("TaskSearchCriteria")
        '    End If

        '    With taskSearch
        '        If .Activity.Length > 0 Then
        '            Me._cblActivity.SelectedValue = .Activity
        '        End If
        '        If .Type.Length > 0 Then
        '            Me._cblIncidentType.SelectedValue = .Type
        '        End If
        '        If .Description.Length > 0 Then
        '            Me._txtDescription.Text = .Description
        '        End If
        '        If .Title.Length > 0 Then
        '            Me._txtTitle.Text = .Title
        '        End If
        '        If .SourceSytemRef.Length > 0 Then
        '            Me._txtIncidentEventNumber.Text = .SourceSytemRef
        '        End If

        '        'If Request.Form(Me._txtResponsiblePerson.UniqueID) IsNot Nothing Then
        '        '    Me._txtResponsiblePerson.Text = Request.Form(Me._txtResponsiblePerson.UniqueID)
        '        'End If
        '        If Request.Form(Me._txtTitle.UniqueID) IsNot Nothing Then
        '            Me._txtTitle.Text = Request.Form(Me._txtTitle.UniqueID)
        '        End If
        '        If Request.Form(Me._rblTaskStatus.UniqueID) IsNot Nothing Then
        '            If _rblTaskStatus.Items.FindByValue(Request.Form(Me._rblTaskStatus.UniqueID)) IsNot Nothing Then
        '                _rblTaskStatus.Items.FindByValue(Request.Form(Me._rblTaskStatus.UniqueID)).Selected = True
        '            End If
        '        End If
        '        If Request.Form(Me._cbHighLevelSecurity.UniqueID) IsNot Nothing Then
        '            If Request.Form(Me._cbHighLevelSecurity.UniqueID) = "on" Then
        '                Me._cbHighLevelSecurity.Checked = True
        '            Else
        '                Me._cbHighLevelSecurity.Checked = False
        '            End If
        '        End If
        '        If Request.Form(Me._dtDueDate.UniqueID) IsNot Nothing Then
        '            Me._dtDueDate.StartDate = Request.Form(Me._dtDueDate.UniqueID)
        '        End If
        '        If Request.Form(Me._cblEstimatedDueDateRange.UniqueID) IsNot Nothing Then
        '            Me._cblEstimatedDueDateRange.SelectedValue = Request.Form(Me._cblEstimatedDueDateRange.UniqueID)
        '        End If
        '    End With
        'End If

        ''.Area = Me._ReportingSite.AreaValue
        ''.BusinessUnit = Me._ReportingSite.BusinessUnitValue
        '.ClosedDateFrom = Me._dtClosedDate.StartDate
        '.ClosedDateTo = Me._dtClosedDate.EndDate
        '.CreatedBy = Me._txtCreatedBy.Text
        '.Description = Me._txtDescription.Text
        '.Division = _ReportingSite.DivisionValue
        '.DueDateFrom = Me._dtDueDate.StartDate
        '.DueDateTo = Me._dtDueDate.EndDate
        '.HeaderFromDate = Me._dtHeaderDate.StartDate
        '.HeaderToDate = Me._dtHeaderDate.EndDate
        '.Line = Me._ReportingSite.LineValue
        '.Machine = Me._ReportingSite.LineBreakValue
        '.PlantCode = Me._ReportingSite.FacilityValue
        '.Region = Me._ReportingSite.RegionValue
        '.Responsible = Me._txtResponsiblePerson.Text
        'If Me._cbHighLevelSecurity.Checked = True Then
        '    .SecurityLevel = "Y"
        'Else
        '    .SecurityLevel = ""
        'End If
        ''.SourceSystem = Me._ddlSourceSystem.SelectedValue
        '.SourceSytemRef = Me._txtIncidentEventNumber.Text
        '.TaskStatus = Me._rblTaskStatus.SelectedValue
        '.Title = Me._txtTitle.Text
        '.Type = Me._cblIncidentType.SelectedValue
        '.UserName = "MJPOPE" 'TODO: Get current username
        'End With

    End Sub
    Public Sub PopulateData()
        Try

            Dim newIncident As New TaskTrackerListsBll
            If newIncident IsNot Nothing Then
                Dim types As System.Collections.Generic.List(Of TaskType) = newIncident.GetTaskTypes
                Dim activities As System.Collections.Generic.List(Of TaskActivity) = newIncident.GetActivities
                Dim externalSites As System.Collections.Generic.List(Of SourceSystems) = newIncident.GetSourceSystems
                Dim status As System.Collections.Generic.List(Of TaskStatus) = newIncident.GetTaskStatus
                Dim elements As System.Collections.Generic.List(Of GMSElementList) = newIncident.GetGMSElements

                If Me.SearchMode = TaskSearchMode.Reporting Then
                    _cblIncidentType.Items.Clear()
                    '_cblIncidentType.Text = ""
                End If

                _cblIncidentType.RepeatLayout = RepeatLayout.Flow
                _cblIncidentType.RepeatDirection = RepeatDirection.Horizontal
                _cblIncidentType.RepeatColumns = 12

                For Each item As TaskType In types
                    'MJP - We need to populate the value field using the English name so that we can pass the text value into MTTView
                    If _cblIncidentType.Items.FindByValue(CStr(item.TaskName)) Is Nothing Then
                        _cblIncidentType.Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.TaskName, True), item.TaskName)) 'CStr(item.TaskSeqid)))
                        _cblIncidentType.Items.FindByValue(CStr(item.TaskName)).Attributes.Add("class", "col-xs-12 col-sm-4")
                    End If
                Next

                _cblActivity.RepeatLayout = RepeatLayout.Flow
                _cblActivity.RepeatDirection = RepeatDirection.Horizontal
                _cblActivity.RepeatColumns = 12

                If Me.SearchMode = TaskSearchMode.Reporting Then
                    For Each item As TaskActivity In activities
                        If _cblActivity.Items.FindByValue(CStr(item.ActivityName)) Is Nothing Then
                            _cblActivity.Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.ActivityName, True), item.ActivityName)) ' CStr(item.ActivitySeqid)))
                            _cblActivity.Items.FindByValue(CStr(item.ActivityName)).Attributes.Add("class", "col-xs-12 col-sm-4")
                        End If
                    Next
                Else
                    For Each item As TaskActivity In activities
                        If _cblActivity.Items.FindByValue(CStr(item.ActivitySeqid)) Is Nothing Then
                            _cblActivity.Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.ActivityName, True), item.ActivitySeqid)) ' CStr(item.ActivitySeqid)))
                            _cblActivity.Items.FindByValue(CStr(item.ActivitySeqid)).Attributes.Add("class", "col-xs-12 col-sm-4")
                        End If
                    Next
                End If


                For Each item As SourceSystems In externalSites
                    If Me._ddlSourceSystem.Items.FindByValue(CStr(item.ExternalSourceSeqid)) Is Nothing Then
                        _ddlSourceSystem.Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.ExternalSource, True), CStr(item.ExternalSourceSeqid)))
                    End If
                Next
                '_site.DefaultFacility = "0627" 'TODO: MJP - Populate the default from the UserDefaults table

                Dim imgPath As String = Page.ResolveUrl("~/Images/")
                Dim openStatusText As String = String.Empty
                Dim openStatusValue As String = String.Empty
                Dim closedStatusText As String = String.Empty
                Dim closedStatusValue As String = String.Empty
                With Me._rblTaskStatus
                    .Items.Clear()

                    For Each item As TaskStatus In status
                        'If item.StatusSeqid = -1 Then Continue For
                        If Me.SearchMode = TaskSearchMode.View Then
                            Dim img As String = "<img src='{0}' align=center width=15 height=15 title='{1}' alt='{1}'>{1}"
                            Dim statusText As String = String.Format(img, imgPath & item.ImageIcon, IP.Bids.SharedFunctions.LocalizeValue(item.StatusName, True))
                            Select Case item.StatusName
                                Case "Open", "Overdue"
                                    If openStatusText.Length > 0 Then
                                        openStatusText = openStatusText & "&nbsp;" & statusText
                                    Else
                                        openStatusText = statusText
                                    End If

                                    If openStatusValue.Length > 0 Then
                                        openStatusValue = openStatusValue & "|" & item.StatusName
                                    Else
                                        openStatusValue = item.StatusName
                                    End If
                                    'If openStatusValue.Length > 0 Then
                                    '    openStatusValue = openStatusValue & "," & item.StatusSeqid
                                    'Else
                                    '    openStatusValue = item.StatusSeqid
                                    'End If
                                    .Items.Add(New ListItem(openStatusText, openStatusValue, True))
                                Case "Complete", "Cancelled", "No Work Needed"
                                    If closedStatusText.Length > 0 Then
                                        closedStatusText = closedStatusText & "&nbsp;" & statusText
                                    Else
                                        closedStatusText = statusText
                                    End If

                                    If closedStatusValue.Length > 0 Then
                                        closedStatusValue = closedStatusValue & "|" & item.StatusName
                                    Else
                                        closedStatusValue = item.StatusName
                                    End If
                                    'If closedStatusValue.Length > 0 Then
                                    '    closedStatusValue = closedStatusValue & "|" & item.StatusSeqid
                                    'Else
                                    '    closedStatusValue = item.StatusSeqid
                                    'End If
                                    .Items.Add(New ListItem(closedStatusText, closedStatusValue, True))
                            End Select


                        Else
                            .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.StatusName, True), item.StatusName, True))
                        End If
                    Next
                    .Items.Insert(0, New ListItem(IP.Bids.SharedFunctions.LocalizeValue("All", True), "All"))
                    .ClearSelection()
                    .SelectedIndex = 0
                    .DataBind()
                End With
                openStatusText = String.Empty
                openStatusValue = String.Empty
                closedStatusText = String.Empty
                closedStatusValue = String.Empty
                With Me._cblTaskStatus
                    .Items.Clear()
                    For Each item As TaskStatus In status
                        'If item.StatusSeqid = -1 Then Continue For
                        If Me.SearchMode = TaskSearchMode.View Then
                            Dim img As String = "<img src='{0}' align=center width=15 height=15 title='{1}' alt='{1}'>{1}"
                            Dim statusText As String = String.Format(img, imgPath & item.ImageIcon, IP.Bids.SharedFunctions.LocalizeValue(item.StatusName, True))
                            Select Case item.StatusName
                                Case "Open", "Overdue"
                                    If openStatusText.Length > 0 Then
                                        openStatusText = openStatusText & "&nbsp;" & statusText
                                    Else
                                        openStatusText = statusText
                                    End If

                                    'If openStatusValue.Length > 0 Then
                                    '    openStatusValue = openStatusValue & "|" & item.StatusName
                                    'Else
                                    '    openStatusValue = item.StatusName
                                    'End If
                                    If openStatusValue.Length > 0 Then
                                        openStatusValue = openStatusValue & "|" & item.StatusSeqid
                                    Else
                                        openStatusValue = item.StatusSeqid
                                    End If
                                Case "Complete", "Cancelled", "No Work Needed"
                                    If closedStatusText.Length > 0 Then
                                        closedStatusText = closedStatusText & "&nbsp;" & statusText
                                    Else
                                        closedStatusText = statusText
                                    End If

                                    'If closedStatusValue.Length > 0 Then
                                    '    closedStatusValue = closedStatusValue & "|" & item.StatusName
                                    'Else
                                    '    closedStatusValue = item.StatusName
                                    'End If
                                    If closedStatusValue.Length > 0 Then
                                        closedStatusValue = closedStatusValue & "|" & item.StatusSeqid
                                    Else
                                        closedStatusValue = item.StatusSeqid
                                    End If
                            End Select
                        Else
                            .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.StatusName, True), item.StatusName, True))
                        End If
                    Next
                    If Me.SearchMode = TaskSearchMode.View Then
                        .Items.Add(New ListItem(openStatusText, openStatusValue, True))
                        .Items.Add(New ListItem(closedStatusText, closedStatusValue, True))
                    End If
                    .Items.Insert(0, New ListItem(IP.Bids.SharedFunctions.LocalizeValue("All", True), "All"))
                    .ClearSelection()
                    .SelectedIndex = 0
                    .DataBind()
                End With
                'With Me._rblTaskStatus
                '    .Items.Clear()
                '    Dim taskStatus As Collection
                '    If Me.SearchMode = TaskSearchMode.View Then
                '        taskStatus = TaskSearch.GetTaskStatusList(True, Page.ResolveUrl("~/Images/"))
                '    Else
                '        taskStatus = TaskSearch.GetTaskStatusList(False, "")
                '    End If
                '    For i As Integer = 1 To taskStatus.Count
                '        .Items.Add(taskStatus.Item(i))
                '    Next
                '    .Items.Insert(0, New ListItem("All", "All"))
                '    .SelectedIndex = 0
                '    .DataBind()
                'End With

                If AvailableReportParameters.DueDate Then
                    With _cblEstimatedDueDateRange
                        Dim dueDateOptions As Collection
                        dueDateOptions = newIncident.GetDueDateOptions
                        .Items.Clear()

                        For i As Integer = 1 To dueDateOptions.Count
                            .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(dueDateOptions.Item(i).ToString, True)))
                        Next
                        .DataBind()
                        If .Items.FindByValue("All") IsNot Nothing Then
                            .CheckAll()
                        End If
                        ' .Items.Add(new list
                    End With
                End If
                If Me.SearchMode = TaskSearchMode.Reporting Then
                    _ddlSubElement.Items.Clear()
                    Me._cblIncidentType.CheckAll()
                    If elements IsNot Nothing AndAlso elements.Count > 0 Then 'AvailableReportParameters.GMSElement = True Then
                        For Each item As GMSElementList In elements
                            If _ddlSubElement.Items.FindByValue(CStr(item.ElementTitle)) Is Nothing Then
                                _ddlSubElement.Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.ElementTitle, True), item.ElementTitle)) ' CStr(item.ActivitySeqid)))

                            End If
                        Next
                    End If
                End If
            Else
                IP.Bids.SharedFunctions.HandleError("PopulateData", "Error Instantiating clsEnterIncident")
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateData", "Error Populating Task Header Data", ex)
        End Try
    End Sub
    Private Function GetTaskStatusIds(ByVal nameList As String) As String
        Dim newIncident As New TaskTrackerListsBll
        Dim list() As String = Split(Replace(nameList, "|", ","), ",")

        Dim statusNames As New StringBuilder
        If newIncident IsNot Nothing Then
            Dim status As System.Collections.Generic.List(Of TaskStatus) = newIncident.GetTaskStatus
            For i As Integer = 0 To list.Length - 1
                If statusNames.Length > 0 Then
                    statusNames.Append(",")
                End If

                For Each item As TaskStatus In status
                    If item.StatusName = list(i) Or item.StatusSeqid.ToString = list(i) Then
                        statusNames.Append(item.StatusSeqid)
                    End If
                Next
            Next
        End If
        Return statusNames.ToString
    End Function
    Private Function GetTaskStatusNames(ByVal idList As String) As String
        Dim newIncident As New TaskTrackerListsBll
        Dim list() As String = Split(Replace(idList, "|", ","), ",")

        Dim statusNames As New StringBuilder
        If newIncident IsNot Nothing Then
            Dim status As System.Collections.Generic.List(Of TaskStatus) = newIncident.GetTaskStatus
            For i As Integer = 0 To list.Length - 1
                If statusNames.Length > 0 Then
                    statusNames.Append(",")
                End If

                For Each item As TaskStatus In status
                    If item.StatusName = list(i) Or item.StatusSeqid.ToString = list(i) Then
                        statusNames.Append(item.StatusName)
                        Exit For
                    End If
                Next
            Next
        End If
        Return statusNames.ToString
    End Function

    Private Function GetActivityNames(ByVal idList As String) As String
        Dim newIncident As New TaskTrackerListsBll

        Dim list() As String = Split(Replace(idList, "|", ","), ",")

        Dim statusNames As New StringBuilder
        If newIncident IsNot Nothing Then
            Dim activities As System.Collections.Generic.List(Of TaskActivity) = newIncident.GetActivities
            For i As Integer = 0 To list.Length - 1
                If statusNames.Length > 0 Then
                    statusNames.Append(",")
                End If

                For Each item As TaskActivity In activities
                    If item.ActivityName = list(i) Or item.ActivitySeqid.ToString = list(i) Then
                        statusNames.Append(item.ActivityName)
                        Exit For
                    End If
                Next
            Next
        End If
        Return statusNames.ToString
    End Function
    'Private Sub PopulateViewSelector(ByVal view As SearchView)
    '    '_ddlViewSelector
    '    If TaskListing Is Nothing OrElse TaskListing.Count = 0 Then
    '        Exit Sub
    '    End If
    '    Select Case view
    '        Case SearchView.Day
    '            _ddlDayViewSelector.Items.Clear()
    '            For Each item As DictionaryEntry In TaskListing
    '                If _ddlDayViewSelector.Items.FindByValue(item.Key) Is Nothing Then
    '                    Dim viewText As String = CDate(item.Key).ToLongDateString
    '                    Dim viewDate As Date = CDate(item.Key)
    '                    _ddlDayViewSelector.Items.Add(New ListItem(viewText, viewDate))
    '                End If
    '            Next
    '            SortDdlByValue(_ddlDayViewSelector)
    '            _ddlDayViewSelector.SelectedIndex = 0
    '            'Case SearchView.Week
    '            '    _ddlWeekViewSelector.Items.Clear()
    '            '    For Each item As DictionaryEntry In TaskListing

    '            '        Dim viewText As String = IP.Bids.SharedFunctions.FirstDayOfWeek(CDate(item.Key)).ToLongDateString & " - " & IP.Bids.SharedFunctions.FirstDayOfWeek(CDate(item.Key)).AddDays(6).ToLongDateString
    '            '        Dim viewDate As Date = IP.Bids.SharedFunctions.FirstDayOfWeek(CDate(item.Key))
    '            '        If _ddlWeekViewSelector.Items.FindByValue(viewDate) Is Nothing Then
    '            '            _ddlWeekViewSelector.Items.Add(New ListItem(viewText, viewDate))
    '            '        End If
    '            '    Next
    '            '    SortDdlByValue(_ddlWeekViewSelector)
    '            '    _ddlWeekViewSelector.SelectedIndex = 0
    '            '    Dim startDate As Date = CDate(Me._ddlWeekViewSelector.SelectedValue)

    '            '    startDate = IP.Bids.SharedFunctions.FirstDayOfWeek(startDate)

    '            '    If Session.Item("TaskListing") IsNot Nothing Then
    '            '        TaskListing = Session.Item("TaskListing")
    '            '        _calWeek.VisibleDate = startDate
    '            '    End If
    '            '    CreateCalendarHtml(_calWeek)

    '        Case SearchView.Month
    '            '_ddlMonthViewSelector.Items.Clear()
    '            'For Each item As DictionaryEntry In TaskListing

    '            '    Dim viewText As String = MonthName(CDate(item.Key).Month, False) & " " & CDate(item.Key).Year
    '            '    Dim viewDate As Date = New Date(CDate(item.Key).Year, CDate(item.Key).Month, 1)
    '            '    If _ddlMonthViewSelector.Items.FindByValue(viewDate) Is Nothing Then
    '            '        _ddlMonthViewSelector.Items.Add(New ListItem(viewText, viewDate))
    '            '    End If
    '            'Next
    '            'SortDdlByValue(_ddlMonthViewSelector)
    '            '_ddlMonthViewSelector.SelectedIndex = 0
    '            '_cal.VisibleDate = CDate(Me._ddlMonthViewSelector.SelectedValue)

    '            'CreateCalendarHtml(_cal)
    '    End Select
    'End Sub
    '  COMMENTED BY CODEIT.RIGHT
    '    Private Function GetActivityID(ByVal activity As String) As Integer
    '        Dim newIncident As New TaskTrackerListsBll
    '        Dim activityID As Integer = -1
    '        Dim activities As System.Collections.Generic.List(Of TaskActivity) = newIncident.GetActivities
    '        For Each item As TaskActivity In activities
    '            If item.ActivityName = activity Then
    '                activityID = item.ActivitySeqid
    '            End If
    '        Next
    '        Return activityID
    '    End Function
    Public Sub SortDdlByValue(ByRef objDdl As DropDownList)
        'Dim textList As New ArrayList()
        'Dim valueList As New ArrayList()
        Dim textList As New Generic.List(Of String)
        Dim valueList As New Generic.List(Of DateTime)


        For Each li As ListItem In objDdl.Items
            If IsDate(li.Value) Then
                valueList.Add(CDate(li.Value))
            End If
        Next

        valueList.Sort()


        For Each item As DateTime In valueList
            If objDdl.Items.FindByValue(item) IsNot Nothing Then
                Dim text As String = objDdl.Items.FindByValue(item).Text
                textList.Add(text)
            End If
        Next
        objDdl.Items.Clear()

        For i As Integer = 0 To textList.Count - 1
            Dim objItem As New ListItem(textList(i).ToString(), valueList(i).ToString())
            objDdl.Items.Add(objItem)
        Next
    End Sub

    Private Sub PersistSearchCriteria(ByVal taskSearch As TaskListingBll)
        Try
            'Persist the Search Criteria into Session
            If Session.Item("TaskSearchCriteria") IsNot Nothing Then
                Session.Remove("TaskSearchCriteria")
            End If
            Session.Add("TaskSearchCriteria", taskSearch)
        Catch
            Throw
        End Try
    End Sub

    Private Sub SaveTaskTrackerSearchCriteria(ByVal taskSearch As TaskListingBll)
        Dim userName As String = IP.Bids.SharedFunctions.GetCurrentUser.Username
        Dim searchXml As String = TaskListingBll.GetXml(taskSearch)
        If searchXml.Length > 0 Then
            'Store the search criteria as a xml file and store into the User Profile table

            Dim defaults As New RIUserDefaults.CurrentUserDefaults(userName, RIUserDefaults.Applications.MTT, ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            'If .RegionValue.Equals("ALL", StringComparison.InvariantCultureIgnoreCase) = False Then
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.TaskSearchCriteria, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.TaskSearchCriteria.ToString, searchXml))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Region, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Region.ToString, taskSearch.Region))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Business, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Business.ToString, taskSearch.Division))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.PlantCode, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.PlantCode.ToString, taskSearch.PlantCode))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.BusinessUnit, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.BusinessUnit.ToString, taskSearch.BusinessUnit))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Area, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Area.ToString, taskSearch.Area))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Line, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Line.ToString, taskSearch.Line))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Machine, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Machine.ToString, taskSearch.Machine))
        End If
    End Sub
    'Private Sub SavePageDefaults()
    '    Dim userName As String = IP.Bids.SharedFunctions.GetCurrentUser.Username
    '    Dim defaults As New RIUserDefaults.CurrentUserDefaults(userName, RIUserDefaults.Applications.MTT, ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
    '    With _site
    '        'If .RegionValue.Equals("ALL", StringComparison.InvariantCultureIgnoreCase) = False Then
    '        defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Region, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Region.ToString, .RegionValue))
    '        defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Business, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Business.ToString, .DivisionValue))
    '        defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.PlantCode, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.PlantCode.ToString, .FacilityValue))
    '        If .BusinessUnitAreaValue.Length > 0 Then
    '            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.BusinessUnit, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.BusinessUnit.ToString, .BusinessUnitAreaValue.Split(CType("-", Char())).ElementAtOrDefault(0).Trim))
    '            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Area, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Area.ToString, .BusinessUnitAreaValue.Split(CType("-", Char())).ElementAtOrDefault(1).Trim))
    '        End If
    '        If .LineBreakValue.Length > 0 Then
    '            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Line, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Line.ToString, .LineBreakValue.Split(CType("-", Char())).ElementAtOrDefault(0).Trim))
    '            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Machine, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Machine.ToString, .LineBreakValue.Split(CType("-", Char())).ElementAtOrDefault(1).Trim))
    '        End If
    '    End With
    'End Sub
    Private Function CollectSearchCriteria() As TaskListingBll
        Dim taskSearch As New TaskListingBll
        Me._cpeSearchSelections.Collapsed = True
        Me._cpeSearchSelections.ClientState = "true"

        With taskSearch
            If _txtHeaderNumber.Text.Length > 0 Then
                .HeaderSeqID = Trim(Me._txtHeaderNumber.Text)
                Me._cblActivity.CheckAll()
                _cblTaskStatus.CheckAll()
                Me._cblIncidentType.CheckAll()
                Me._ResponsiblePerson.PopulateEmployeeList(IP.Bids.SharedFunctions.GetCurrentUser.Username)
                .Activity = "All"
                .TaskStatus = "All"
                .Type = "All"
                .Responsible = Me._ResponsiblePerson.SelectedValue
            Else
                .HeaderSeqID = ""

                If Me._cblActivity.SelectedValue.Length = 0 Then
                    Me._cblActivity.CheckAll()
                End If
                If Me._cblActivity.SelectedListItems.Count = Me._cblActivity.Items.Count Then
                    'All items are selected
                    .Activity = "All"
                Else
                    .Activity = Me._cblActivity.SelectedValue
                End If

                If Me._cblTaskStatus.SelectedListItems.Count + 1 >= Me._cblTaskStatus.Items.Count Then
                    'All items are selected
                    .TaskStatus = "All"
                ElseIf Me._cblTaskStatus.SelectedValue = "All" Then
                    .TaskStatus = "All"
                Else
                    .TaskStatus = GetTaskStatusIds(Me._cblTaskStatus.SelectedValue)
                End If

                .AndOr = "And" 'Me._rblDisplayResultsWith.SelectedValue
                .Area = Me._ReportingSite.AreaValue
                .BusinessUnit = Me._ReportingSite.BusinessUnitValue
                If Me._dtClosedDate.StartDate.Length > 0 Then
                    .ClosedDateFrom = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Me._dtClosedDate.StartDate)
                Else
                    .ClosedDateFrom = ""
                End If
                If Me._dtClosedDate.EndDate.Length > 0 Then
                    .ClosedDateTo = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Me._dtClosedDate.EndDate)
                Else
                    .ClosedDateTo = ""
                End If
                If _dtClosedDate IsNot Nothing Then
                    .CloseDateRange = _dtClosedDate.SelectedDateRange
                End If
                .CreatedBy = Me._CreatedBy.SelectedValue ' Me._txtCreatedBy.Text
                .CreatedByPlantCode = Me._CreatedBy.PlantCode
                .Description = Me._txtDescription.Text
                .Division = _ReportingSite.DivisionValue
                If _dtDueDate IsNot Nothing Then
                    .DueDateDateRange = Me._dtDueDate.SelectedDateRange
                End If
                If Me._dtDueDate.StartDate.Length > 0 Then
                    .DueDateFrom = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Me._dtDueDate.StartDate)
                Else
                    .DueDateFrom = ""
                End If
                If Me._dtDueDate.EndDate.Length > 0 Then
                    .DueDateTo = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Me._dtDueDate.EndDate)
                Else
                    .DueDateTo = ""
                End If
                'If _dtDueDate IsNot Nothing Then
                '    .DueDateDateRange = _dtDueDate.DateRange
                'End If
                If _dtHeaderDate IsNot Nothing Then
                    .HeaderDateRange = _dtHeaderDate.SelectedDateRange
                End If
                If Me._dtHeaderDate.StartDate.Length > 0 Then
                    .HeaderFromDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Me._dtHeaderDate.StartDate)
                Else
                    .HeaderFromDate = ""
                End If
                If Me._dtHeaderDate.EndDate.Length > 0 Then
                    .HeaderToDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Me._dtHeaderDate.EndDate)
                Else
                    .HeaderToDate = ""
                End If
                .Line = Me._ReportingSite.LineValue
                .Machine = Me._ReportingSite.LineBreakValue
                .PlantCode = Me._ReportingSite.FacilityValue
                .Region = Me._ReportingSite.RegionValue
                If IsNumeric(Me._ResponsiblePerson.SelectedValue) Then 'Role Selected
                    .ResponsiblePlantCode = Me._ResponsiblePerson.PlantCode
                    .Responsible = Me._ResponsiblePerson.SelectedValue ' Me._txtResponsiblePerson.Text
                Else
                    .ResponsiblePlantCode = String.Empty 'Me._ResponsiblePerson.PlantCode
                    .Responsible = Me._ResponsiblePerson.SelectedValue ' Me._txtResponsiblePerson.Text
                End If

                If IsNumeric(Me._RoleList.SelectedValue) Then 'Role Selected
                    .Role = Me._RoleList.SelectedText 'SelectedValue ' Me._txtResponsiblePerson.Text              
                    .RoleId = Me._RoleList.SelectedValue
                End If

                If Me._cbHighLevelSecurity.Checked = True Then
                    .SecurityLevel = "Y"
                Else
                    .SecurityLevel = ""
                End If
                If Me._ddlSourceSystem.SelectedItem IsNot Nothing Then
                    .SourceSystem = Me._ddlSourceSystem.SelectedItem.Value
                End If

                .SourceSytemRef = Me._txtIncidentEventNumber.Text
                .Title = Me._txtTitle.Text

                If Me._cblIncidentType.SelectedValue.Length = 0 Then
                    Me._cblIncidentType.CheckAll()
                End If
                If Me._cblIncidentType.SelectedListItems.Count = Me._cblIncidentType.Items.Count Then
                    'All items are selected
                    .Type = "All"
                Else
                    .Type = Me._cblIncidentType.SelectedValue
                End If
                _currentUser = IP.Bids.SharedFunctions.GetCurrentUser
                .UserName = _currentUser.Username

                If Me._ddlSubElement.SelectedItem IsNot Nothing Then
                    .Element = Me._ddlSubElement.SelectedItem.Value
                End If

            End If
            .TaskSearchView = _ddlSearchOptions.SelectedValue

        End With
        Return taskSearch

        'PopulateViewSelector(View)
    End Function

    Private Sub HandleNoResultsFound()
        Me._cpeSearchSelections.Collapsed = False
        Me._cpeSearchSelections.ClientState = "false"
        DisplayMessage("Your search criteria did not produce any results")
        _pnlCalendarView.Visible = False
        _pnlCalendarListing.Visible = False
        '_pnlWeekView.Visible = False
        '_pnlDayView.Visible = False
        '_pnlTaskListing.Visible = False
    End Sub
    Public Sub DisplayMessage(ByVal msg As String)
        With _messageBox
            .Message = msg
            .Title = "View Tasks"
            .ShowMessage()
        End With



    End Sub
    Public Function GetResponsiblePerson(ByVal responsiblePerson As String, ByVal responsibleSiteName As String, ByVal roleDescription As String) As String
        Dim retVal As String = String.Empty
        If roleDescription.Trim.Length > 0 Then
            retVal = IP.Bids.SharedFunctions.LocalizeValue(roleDescription) & " - " & responsibleSiteName
        Else
            retVal = responsiblePerson
        End If
        Return retVal
        'Eval("RESPNAME"), Eval("RESPROLESITENAME"), Eval("ROLEDESCRIPTION")
    End Function


    Public Function GetTaskStatus(ByVal dueDate As Object, ByVal statusID As Integer, ByVal statusName As String, Optional ByVal iconOnly As Boolean = False) As String
        Dim taskSearch As New TaskTrackerListsBll
        Dim imgPath As String = Page.ResolveUrl("~/Images/")
        Dim status As String = String.Empty
        Dim overdueStatusID As Integer = -1
        Dim includeLabel As Boolean = True
        If iconOnly Then
            includeLabel = False
        End If

        dueDate = IP.Bids.SharedFunctions.DataClean(dueDate)
        If dueDate.ToString.Length = 0 Then
            Return taskSearch.GetTaskStatus(statusID, includeLabel, imgPath)
            Exit Function
        End If


        If CDate(dueDate) < Now And statusName.ToLower = "open" Or statusName = "1" Then
            status = taskSearch.GetTaskStatus(overdueStatusID, includeLabel, imgPath) '& " - " & statusName
        Else
            status = taskSearch.GetTaskStatus(statusID, includeLabel, imgPath) '& " - " & statusName
        End If
        Return status
    End Function
    Private Sub CombineTasksForCalendarView(ByVal tasks As System.Collections.Generic.List(Of TaskListing))
        'Tasks are sorted by due date
        Dim taskSearch As New TaskTrackerListsBll
        Dim imgPath As String = Page.ResolveUrl("~/Images/")
        Dim dueDate As String = ""
        Dim sbTasks As New StringBuilder
        imgPath = Mid(Page.Request.Url.ToString, 1, Page.Request.Url.ToString.LastIndexOf("/", StringComparison.CurrentCulture)) & "/Images/"
        TaskListing = New Collections.Specialized.OrderedDictionary
        If tasks Is Nothing OrElse tasks.Count = 0 Then
            Exit Sub
        End If
        Dim overdueStatusID As Integer = -1

        For Each item As TaskListing In tasks
            If dueDate.Length = 0 Then
                dueDate = item.DueDate
            End If
            If item.DueDate <> dueDate Then
                'Due Date has changed
                If TaskListing.Contains(CDate(dueDate).ToShortDateString) = False Then
                    TaskListing.Add(CDate(dueDate).ToShortDateString, sbTasks.ToString)
                    dueDate = item.DueDate
                    sbTasks.Length = 0
                    If item.DueDate < Now And item.StatusName.ToLower = "open" Then
                        sbTasks.Append(taskSearch.GetTaskStatus(overdueStatusID, False, imgPath) & "<a href='TaskDetails.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "&TaskNumber=" & item.TaskItemSeqId & "' title='" & item.ItemTitle & "'>")
                    Else
                        sbTasks.Append(taskSearch.GetTaskStatus(item.StatusSeqId, False, imgPath) & "<a href='TaskDetails.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "&TaskNumber=" & item.TaskItemSeqId & "' title='" & item.ItemTitle & "'>")
                    End If

                    'If item.ItemTitle.Length > 25 Then
                    '    sbTasks.Append(item.ItemTitle.Substring(0, 25) & "...")
                    'Else
                    '    sbTasks.Append(item.ItemTitle)
                    'End If
                    sbTasks.Append(item.ItemTitle)
                    sbTasks.Append("</a>")
                    sbTasks.Append("<br/><br/>")
                End If
            Else
                If item.DueDate < Now And item.StatusName.ToLower = "open" Then
                    sbTasks.Append(taskSearch.GetTaskStatus(overdueStatusID, False, imgPath) & "<a href='TaskDetails.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "&TaskNumber=" & item.TaskItemSeqId & "' title='" & item.ItemTitle & "'> ")
                Else
                    sbTasks.Append(taskSearch.GetTaskStatus(item.StatusSeqId, False, imgPath) & "<a href='TaskDetails.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "&TaskNumber=" & item.TaskItemSeqId & "' title='" & item.ItemTitle & "'> ")
                End If
                '"Javascript:parent.window.location=('<%#string.format(Page.ResolveUrl("~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}"),EVAL("TASKHEADERSEQID"),EVAL("TASKITEMSEQID")) %>');"
                'sbTasks.Append(taskSearch.GetTaskStatus(item.StatusSeqId, True, imgPath) & " - <a href='TaskItems.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "' title='" & item.ItemTitle & "'>")
                sbTasks.Append(item.ItemTitle)
                sbTasks.Append("</a>")
                sbTasks.Append("<br/><br/>")
            End If
        Next

        If TaskListing.Item(CDate(dueDate).ToShortDateString) Is Nothing Then
            TaskListing.Add(CDate(dueDate).ToShortDateString, sbTasks.ToString)
        End If
        If Session.Item("TaskListing") IsNot Nothing Then
            Session.Remove("TaskListing")
        End If
        Session.Add("TaskListing", TaskListing)
        'For Each item As DictionaryEntry In TaskListing
        '    System.Diagnostics.Debug.Print(item.Key)
        '    System.Diagnostics.Debug.Print(item.Value)
        'Next

    End Sub

    Public Function GetRenderedControl(ByVal ctrl As Control) As String
        Dim sb As New StringBuilder()
        Dim tw As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tw)
        ctrl.RenderControl(hw)
        Return sb.ToString()
    End Function

    'Public Sub VerifyRenderingInServerForm(ByVal ct As Control)
    '    Exit Sub
    'End Sub


    'Private Sub CombineTasksForHeaderView(ByVal tasks As System.Collections.Generic.List(Of TaskListing))
    '    'Tasks are sorted by due date
    '    Dim taskSearch As New TaskTrackerListsBLL
    '    Dim dueDate As String = ""
    '    Dim sbTasks As New StringBuilder
    '    Dim HeaderTaskListing As System.Collections.Generic.List(Of TaskListing) = New System.Collections.Generic.List(Of TaskListing)
    '    If tasks Is Nothing OrElse tasks.Count = 0 Then Exit Sub

    '    For Each item As TaskListing In tasks
    '        If dueDate.Length = 0 Then
    '            dueDate = item.DueDate
    '        End If
    '        If item.DueDate <> dueDate Then
    '            'Due Date has changed
    '            If TaskListing.Contains(CDate(dueDate).ToShortDateString) = False Then
    '                TaskListing.Add(CDate(dueDate).ToShortDateString, sbTasks.ToString)
    '                dueDate = item.DueDate
    '                sbTasks.Length = 0
    '                If item.DueDate < Now And item.StatusName.ToLower = "open" Then
    '                    sbTasks.Append(taskSearch.GetTaskStatus(overdueStatusID, False, imgPath) & "<a href='TaskDetails.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "&TaskNumber=" & item.TaskItemSeqId & "' title='" & item.ItemTitle & "'>")
    '                Else
    '                    sbTasks.Append(taskSearch.GetTaskStatus(item.StatusSeqId, False, imgPath) & "<a href='TaskDetails.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "&TaskNumber=" & item.TaskItemSeqId & "' title='" & item.ItemTitle & "'>")
    '                End If

    '                'If item.ItemTitle.Length > 25 Then
    '                '    sbTasks.Append(item.ItemTitle.Substring(0, 25) & "...")
    '                'Else
    '                '    sbTasks.Append(item.ItemTitle)
    '                'End If
    '                sbTasks.Append(item.ItemTitle)
    '                sbTasks.Append("</a>")
    '                sbTasks.Append("<br/><br/>")
    '            End If
    '        Else
    '            If item.DueDate < Now And item.StatusName.ToLower = "open" Then
    '                sbTasks.Append(taskSearch.GetTaskStatus(overdueStatusID, False, imgPath) & "<a href='TaskDetails.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "&TaskNumber=" & item.TaskItemSeqId & "' title='" & item.ItemTitle & "'> ")
    '            Else
    '                sbTasks.Append(taskSearch.GetTaskStatus(item.StatusSeqId, False, imgPath) & "<a href='TaskDetails.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "&TaskNumber=" & item.TaskItemSeqId & "' title='" & item.ItemTitle & "'> ")
    '            End If
    '            '"Javascript:parent.window.location=('<%#string.format(Page.ResolveUrl("~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}"),EVAL("TASKHEADERSEQID"),EVAL("TASKITEMSEQID")) %>');"
    '            'sbTasks.Append(taskSearch.GetTaskStatus(item.StatusSeqId, True, imgPath) & " - <a href='TaskItems.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "' title='" & item.ItemTitle & "'>")
    '            sbTasks.Append(item.ItemTitle)
    '            sbTasks.Append("</a>")
    '            sbTasks.Append("<br/><br/>")
    '        End If
    '    Next
    '    TaskListing.Add(CDate(dueDate).ToShortDateString, sbTasks.ToString)
    '    If Session.Item("TaskListing") IsNot Nothing Then
    '        Session.Remove("TaskListing")
    '    End If
    '    Session.Add("TaskListing", TaskListing)
    '    'For Each item As DictionaryEntry In TaskListing
    '    '    System.Diagnostics.Debug.Print(item.Key)
    '    '    System.Diagnostics.Debug.Print(item.Value)
    '    'Next

    'End Sub
#End Region

#Region "Events"
    'Protected Sub _cal_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles _cal.DayRender
    '    Dim dt As String = e.Day.Date.Day.ToString()

    '    If e.Day.Date = Now.Date Then
    '        e.Cell.Font.Bold = True
    '    End If

    '    If TaskListing IsNot Nothing AndAlso TaskListing(e.Day.Date.ToShortDateString) IsNot Nothing Then
    '        dt = dt & "<br/><br/>" & TaskListing(e.Day.Date.ToShortDateString)
    '    End If
    '    'e.Cell.Height = Unit.Percentage(20)
    '    e.Cell.VerticalAlign = VerticalAlign.Top
    '    e.Cell.HorizontalAlign = HorizontalAlign.Left
    '    e.Cell.Text = dt
    'End Sub

    'Protected Sub _calWeek_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles _calWeek.DayRender

    '    Dim startDate As Date = _calWeek.VisibleDate


    '    Dim dt As String = e.Day.Date.Day.ToString()

    '    If e.Day.Date = Now.Date Then
    '        e.Cell.Font.Bold = True
    '    End If
    '    If (e.Day.Date < startDate) Or (e.Day.Date >= startDate.AddDays(7)) Then
    '        e.Cell.Text = ""
    '        e.Cell.Height = 0
    '        e.Cell.Style.Item("display") = "none"
    '        Exit Sub
    '    Else
    '        e.Cell.Style.Item("display") = ""
    '        e.Cell.Height = Unit.Percentage(100) ' Unit.Pixel(400)
    '    End If

    '    If TaskListing IsNot Nothing AndAlso TaskListing(e.Day.Date.ToShortDateString) IsNot Nothing Then
    '        dt = dt & "<br/><br/>" & TaskListing(e.Day.Date.ToShortDateString)
    '    End If

    '    e.Cell.VerticalAlign = VerticalAlign.Top
    '    e.Cell.HorizontalAlign = HorizontalAlign.Left
    '    e.Cell.Text = dt
    'End Sub

    Protected Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
        IP.Bids.SharedFunctions.HandleError("Page_Error")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandlePageLoad()
    End Sub

    Private Sub HandlePageLoad()

        Me._CreatedBy.UserMode = User_Controls.AdvancedEmployeeListDropdown.UserModes.UsersOnly ' UserControlsEmployeeList2.UserModes.UsersOnly
        Me._ResponsiblePerson.UserMode = User_Controls.AdvancedEmployeeListDropdown.UserModes.UsersAndRoles
        _ddlSearchOptions.Visible = False
        If Not Page.IsPostBack Then

            PopulateData()
            _btnResetReportSelections.OnClientClick = "Javascript:window.location='" & Page.ResolveClientUrl(Page.AppRelativeVirtualPath) & "';return false"
            '_txtResponsiblePerson.Attributes.Add("onclick", Me._ResponsiblePerson.GetShowUserJS)
            '_txtCreatedBy.Attributes.Add("onclick", Me._ResponsiblePerson.GetShowUserJS)
            Dim defaults As RIUserDefaults.CurrentUserDefaults = Nothing
            If IP.Bids.SharedFunctions.GetCurrentUser IsNot Nothing AndAlso IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults IsNot Nothing Then
                defaults = IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults

                If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.PlantCode) Then
                    Me._ResponsiblePerson.PlantCode = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
                    Me._CreatedBy.PlantCode = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
                End If
                If Me._dtHeaderDate.StartDate.Length = 0 Then
                    Me._dtHeaderDate.DateRange = UserControlsJQDateRange.JQDateRange.CurrentYear
                End If
            End If
            ReselectControls()
        Else
            PopulateData()
            With _cblEstimatedDueDateRange
                Dim dueDateOptions As Collection
                Dim newIncident As New TaskTrackerListsBll
                dueDateOptions = newIncident.GetDueDateOptions
                .Items.Clear()

                For i As Integer = 1 To dueDateOptions.Count
                    .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(dueDateOptions.Item(i).ToString), dueDateOptions.Item(i).ToString))
                Next
                .DataBind()
                If .Items.FindByValue("All") IsNot Nothing Then
                    .CheckAll()
                End If
                newIncident = Nothing
                dueDateOptions = Nothing
                ' .Items.Add(new list
            End With
        End If
        PopulateReportTitles("MTT")

        If Request.QueryString("SV") IsNot Nothing Then
            Dim sortValue As String = Request.QueryString("SV")
            If Me._ddlReportSortValue.Items.FindByValue(sortValue) IsNot Nothing Then
                Me._ddlReportSortValue.ClearSelection()
                Me._ddlReportSortValue.Items.FindByValue(sortValue).Selected = True
                _ddlReportSortValue.Enabled = False
            End If
        End If
        ReselectControls()
        If Request.QueryString("RN") IsNot Nothing Then
            Dim reportName As String = Request.QueryString("RN")
            If Me._ddlReportTitles.Items.FindByValue(reportName) IsNot Nothing Then
                Me._ddlReportTitles.ClearSelection()
                Me._ddlReportTitles.Items.FindByValue(reportName).Selected = True
                'Me._ddlReportTitles.Enabled = False
                'Me._btnResetReportSelections.Visible = False
            End If
            PopulateReportListingControls("MTT")
            _pnlReportSelection.Visible = False
        Else
            PopulateReportListingControls("MTT")
        End If
        'PopulateData()
        DisplaySearchSelections()
    End Sub
    Protected Sub _btnReportView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnReportView.Click
        Dim taskSearch As TaskListingBll

        taskSearch = CollectSearchCriteria()
        _pnlCalendarView.Visible = False
        _pnlCalendarListing.Visible = False
        '_pnlWeekView.Visible = False
        '_pnlDayView.Visible = False
        '_pnlTaskListing.Visible = False
        DisplayReport(taskSearch)
    End Sub

    Private Function GetExcelData(dt As Data.DataTable) As MemoryStream
        If dt Is Nothing Then Return Nothing

        Dim workbook As New XLWorkbook
        Dim worksheet As IXLWorksheet
        ' dt = LocalizeData(dt)
        worksheet = workbook.Worksheets.Add(dt)
        worksheet.Tables.Table(0).ShowAutoFilter = False

        Dim HeaderTitleCol As Integer
        Dim HeaderDescCol As Integer
        Dim ItemDescCol As Integer
        Dim ItemTitleCol As Integer

        For i As Integer = 1 To worksheet.Rows.Count
            If i = 1 Then
                For j As Integer = 1 To worksheet.Columns.Count
                    Select Case worksheet.Row(i).Cell(j).Value
                        Case "HeaderTitle"
                            HeaderTitleCol = j
                        Case "HeaderDesc"
                            HeaderDescCol = j
                        Case "ItemDesc"
                            ItemDescCol = j
                        Case "ItemTitle"
                            ItemTitleCol = j
                    End Select
                Next
            Else
                For j As Integer = 1 To worksheet.Columns.Count
                    If IP.Bids.SharedFunctions.IsEnglishDate(worksheet.Row(i).Cell(j).Value) Then
                        worksheet.Row(i).Cell(j).SetValue(IP.Bids.SharedFunctions.FormatDate(IP.Bids.SharedFunctions.GetEnglishDate(worksheet.Row(i).Cell(j).Value)))
                    ElseIf IsDate(worksheet.Row(i).Cell(j).Value) Then
                        worksheet.Row(i).Cell(j).SetValue(IP.Bids.SharedFunctions.FormatDate(worksheet.Row(i).Cell(j).Value))
                    ElseIf j = HeaderTitleCol OrElse j = HeaderDescCol OrElse j = ItemDescCol OrElse j = ItemTitleCol Then
                        worksheet.Row(i).Cell(j).SetValue(IP.Bids.SharedFunctions.LocalizeValue(worksheet.Row(i).Cell(j).Value))
                    Else
                        worksheet.Row(i).Cell(j).SetValue(IP.Bids.SharedFunctions.LocalizeValue(worksheet.Row(i).Cell(j).Value))
                    End If
                Next
            End If
        Next

        Dim renderedExcelData = GetStream(workbook)
        worksheet = Nothing
        workbook.Dispose()
        Return renderedExcelData
    End Function

    Private Function LocalizeData(ByVal dt As Data.DataTable) As Data.DataTable
        dt.Locale = System.Globalization.CultureInfo.CurrentCulture
        For currentRow As Integer = 0 To dt.Rows.Count - 1
            For col As Integer = 0 To dt.Columns.Count - 1
                If dt.Rows(currentRow).Item(col) Is Nothing Then Continue For
                If IsDBNull(dt.Rows(currentRow).Item(col)) Then Continue For
                'If IsDate(dt.Rows(currentRow).Item(col)) Then
                '    dt.Columns(col).DataType = GetType(String)
                '    dt.Rows(currentRow).Item(col) = IP.Bids.SharedFunctions.FormatDate(dt.Rows(currentRow).Item(col))
                'Else
                dt.Rows(currentRow).Item(col) = IP.Bids.SharedFunctions.LocalizeValue(dt.Rows(currentRow).Item(col))
                'End If

            Next
        Next
        Return dt
    End Function

    Private Function GetStream(excelWorkbook As XLWorkbook) As MemoryStream
        Dim fs As Stream = New MemoryStream()
        excelWorkbook.SaveAs(fs)
        fs.Position = 0
        Return fs
    End Function

    Private Sub ExportToExcel(ByVal taskSearch As TaskListingBll)
        Me._cpeSearchSelections.Collapsed = False
        Me._cpeSearchSelections.ClientState = "false"
        currentTab = 0
        DisplaySearchResults()
        Dim dt = taskSearch.GetTaskListingDataTable()
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim excel = GetExcelData(dt) 'IP.Bids.SharedFunctions.WriteExcelXml(dt.CreateDataReader, Nothing)
            If Session.Item("ExcelXML") IsNot Nothing Then
                Session.Remove("ExcelXML")
            End If
            Session.Add("ExcelXML", excel)
            Dim btn As ModalIframePopup
            btn = CType(LoadControl("~\User Controls\ModalIframPopup.ascx"), ModalIframePopup)
            With btn
                '.Attributes.Add("display", "none")
                .HideDisplayButton()
                .BannerText = "Excel"
                .Url = "Popups/Excel.aspx"
                'Me._udpCalendarView.Controls.Item(0).Controls.Add(btn)
                .LoadIFrame()
            End With
        Else
            HandleNoResultsFound()
        End If

    End Sub
    Private Sub DisplayCalendarView(ByVal listing As System.Collections.Generic.List(Of TaskListing))
        'Dim listing As System.Collections.Generic.List(Of TaskListing)

        If listing IsNot Nothing Then
            'listing = taskSearch.GetHeaderItemListing(False)  'taskSearch.GetTaskListing()
            If listing.Count > 0 Then
                _calEvents.DisplayEventCalendar(listing)
                _tabSearchResults.Visible = True
                _tabSearchTabs.Visible = True
                'Me._tabMonthView.Visible = True
            Else
                'Me._tabMonthView.Visible = False
                '_tabSearchResults.Visible = False
            End If
        End If
    End Sub

    Private Sub DisplayTaskList(ByVal listing As System.Collections.Generic.List(Of TaskListing))
        'Dim listing As System.Collections.Generic.List(Of TaskListing)

        'If taskSearch IsNot Nothing Then
        '    listing = taskSearch.GetHeaderItemListing()
        Dim currentPage As Integer = 1
        'If Session.Item("TaskEditCurrentPage") IsNot Nothing Then
        '    If IsNumeric(Session.Item("TaskEditCurrentPage")) Then
        '        currentPage = CInt(Session.Item("TaskEditCurrentPage"))
        '    End If
        '    Session.Remove("TaskEditCurrentPage")
        'End If

        If listing IsNot Nothing AndAlso listing.Count > 0 Then
            'Me._tabTaskItemListing.Visible = True

            _pnlTaskListing.Visible = True
            _ifrMultiEdit.Visible = True

            _ifrMultiEdit.Attributes.Item("src") = Page.ResolveUrl(String.Format("~/Popups/BulkEditTasks.aspx?useCachedData=True&postBackTrigger={0}&Page={1}", _btnDisplaySearchResults.ClientID, currentPage))
            _ifrMultiEdit.Attributes.Item("onload") = "$('#" & _ifrMultiEdit.ClientID & "').css('display', 'block'); $('#preload-img').css('display', 'none');" 'resizeFrame(document.getElementById('" & Me._ifrMultiEdit.ClientID & "'));
            '_ifrMultiEdit.Attributes.Item("onload") = "$('#" & _ifrMultiEdit.ClientID & "').load(function() {$('#" & _ifrMultiEdit.ClientID & "').css('display', 'block'); $('#preload-img').css('display', 'none');});" 'resizeFrame(document.getElementById('" & Me._ifrMultiEdit.ClientID & "'));

        Else
            _ifrMultiEdit.Visible = False
            _pnlTaskListing.Visible = False
        End If
        'End If
    End Sub

    Private Sub DisplayReport(ByVal taskSearch As TaskListingBll)

        Dim mttReportQry As String = "?Report={0}&Division={1}&Region={2}&PlantName={3}&BusUnit={4}&Area={5}&LineSystem={6}&LineBreak={7}&Status={8}&Safety={9}&Quality={10}&Environmental={11}&Reliability={12}&Project/Other={13}&Health={14}&Outage={15}&DueDate={16}&Overdue={17}&Next7days={18}&Next14days={19}&Next30days={20}&StartMonth={21}&StartDay={22}&StartYear={23}&EndDay={24}&EndMonth={25}&EndYear={26}&CreatedBy={27}&ResponsiblePerson={28}&Responsible={28}&Complaint={29}&Permit={30}&Audit={31}&Inspection={32}&Incident={33}&MOC={34}&TaskType={35}&Activity={36}&PERSON={37}&TaskListing={38}&SourceSystem={39}&Reference={40}&Title={41}&Other={42}&NOV={43}&Order={44}&Citation={45}&ComplianceCertification={46}&CostReduction={47}&Capital={48}&People={49}&GMS={50}&Role={51}&Element={52}&SRRLearning={53}&AppName=MTT Reports&DeliberateImprovement={54}&ManufacturingAlert={55}&PSM={56}&NCGTRS={57}&TypeList={58}&ActivityList={59}&TaskHeader={60}&LANG={61}&RoleId={62}"
        Dim taskReportParms As New ReportParms

        taskReportParms.Report = Me._ddlReportSortValue.SelectedValue
        With Me.AvailableReportParameters
            If .Site Then
                taskReportParms.PlantName = Server.UrlEncode(Me._ReportingSite.Facility)
                taskReportParms.Region = Me._ReportingSite.RegionValue
                If Me._ReportingSite.DivisionValue = "P&C Papers" Then
                    'taskReportParms.Division = "PandC Papers"
                    taskReportParms.Division = Server.UrlEncode(Me._ReportingSite.DivisionValue)
                Else
                    taskReportParms.Division = IIf(Me._ReportingSite.DivisionValue.Length > 0, Me._ReportingSite.DivisionValue, "All")
                End If

                taskReportParms.BusUnit = IIf(Me._ReportingSite.BusinessUnitValue.Length > 0, Me._ReportingSite.BusinessUnitValue, "All")
                taskReportParms.Area = IIf(Me._ReportingSite.AreaValue.Length > 0, Me._ReportingSite.AreaValue, "All")
                taskReportParms.LineSystem = IIf(Me._ReportingSite.LineValue.Length > 0, Me._ReportingSite.LineValue, "All")
                taskReportParms.LineBreak = IIf(Me._ReportingSite.LineBreakValue.Length > 0, Me._ReportingSite.LineBreakValue, "All")
            End If
            If .Activity Then
                If Me._cblActivity.SelectedValue.Length = 0 Then
                    Me._cblActivity.CheckAll()
                End If
                If _cblActivity.SelectedListItems.Count = _cblActivity.Items.Count - 1 Then
                    taskReportParms.Activity = "All"
                    Me._cblActivity.CheckAll()
                End If

                Dim sbActivityItems As New StringBuilder
                For Each item As ListItem In _cblActivity.Items
                    If item.Selected Then
                        If sbActivityItems.Length > 0 Then sbActivityItems.Append(",")
                        sbActivityItems.Append(item.Value)
                    End If
                Next
                taskReportParms.ActivityList = sbActivityItems.ToString
                If Me._cblActivity.Items.FindByValue("All") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("All").Selected Then
                    taskReportParms.Activity = "All" 'GetActivityID("activity")
                End If
                If Me._cblActivity.Items.FindByValue("Audit") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("Audit").Selected Then
                    taskReportParms.Audit = "Audit"
                Else
                    taskReportParms.Audit = "All"
                End If
                If Me._cblActivity.Items.FindByValue("Inspection") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("Inspection").Selected Then
                    taskReportParms.Inspection = "Inspection"
                Else
                    taskReportParms.Inspection = "All"
                End If
                If Me._cblActivity.Items.FindByValue("Complaint") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("Complaint").Selected Then
                    taskReportParms.Complaint = "Complaint"
                Else
                    taskReportParms.Complaint = "All"
                End If
                If Me._cblActivity.Items.FindByValue("MOC") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("MOC").Selected Then
                    taskReportParms.MOC = "MOC"
                Else
                    taskReportParms.MOC = "All"
                End If
                If Me._cblActivity.Items.FindByValue("Incident") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("Incident").Selected Then
                    taskReportParms.Incident = "Incident"
                Else
                    taskReportParms.Incident = "All"
                End If
                If Me._cblActivity.Items.FindByValue("Permit") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("Permit").Selected Then
                    taskReportParms.Permit = "Permit"
                Else
                    taskReportParms.Permit = "All"
                End If
                If Me._cblActivity.Items.FindByValue("Other") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("Other").Selected Then
                    taskReportParms.Other = "Other"
                Else
                    taskReportParms.Other = "All"
                End If
                If Me._cblActivity.Items.FindByValue("NOV") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("NOV").Selected Then
                    taskReportParms.NOV = "NOV"
                Else
                    taskReportParms.NOV = "All"
                End If
                If Me._cblActivity.Items.FindByValue("Order") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("Order").Selected Then
                    taskReportParms.Order = "Order"
                Else
                    taskReportParms.Order = "All"
                End If
                If Me._cblActivity.Items.FindByValue("Citation") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("Citation").Selected Then
                    taskReportParms.Citation = "Citation"
                Else
                    taskReportParms.Citation = "All"
                End If
                If Me._cblActivity.Items.FindByValue("Compliance Certification") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("Compliance Certification").Selected Then
                    taskReportParms.ComplianceCertification = "Compliance Certification"
                Else
                    taskReportParms.ComplianceCertification = "All"
                End If
                If Me._cblActivity.Items.FindByValue("GMS") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("GMS").Selected Then
                    taskReportParms.GMS = "GMS"
                Else
                    taskReportParms.GMS = "All"
                End If
                If Me._cblActivity.Items.FindByValue("SRR Learning") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("SRR Learning").Selected Then
                    taskReportParms.SRRLearning = "SRR Learning"
                Else
                    taskReportParms.SRRLearning = "All"
                End If
                If Me._cblActivity.Items.FindByValue("Deliberate Improvement") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("Deliberate Improvement").Selected Then
                    taskReportParms.DeliberateImprovement = "Deliberate Improvement"
                Else
                    taskReportParms.DeliberateImprovement = "All"
                End If
                If Me._cblActivity.Items.FindByValue("Manufacturing Alert") IsNot Nothing AndAlso Me._cblActivity.Items.FindByValue("Manufacturing Alert").Selected Then
                    taskReportParms.ManufacturingAlert = "Manufacturing Alert"
                Else
                    taskReportParms.ManufacturingAlert = "All"
                End If
            End If

            If .Types Then
                If Me._cblIncidentType.SelectedValue.Length = 0 Then
                    Me._cblIncidentType.CheckAll()
                End If

                If _cblIncidentType.SelectedListItems.Count = _cblIncidentType.Items.Count - 1 Then
                    taskReportParms.TaskType = "All"
                    Me._cblIncidentType.CheckAll()
                End If
                Dim sbTypeList As New StringBuilder
                For Each item As ListItem In _cblIncidentType.Items
                    If item.Selected Then
                        If sbTypeList.Length > 0 Then sbTypeList.Append(",")
                        sbTypeList.Append(item.Value)
                    End If
                Next
                taskReportParms.TypeList = sbTypeList.ToString
                If Me._cblIncidentType.Items.FindByValue("All") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("All").Selected Then
                    taskReportParms.TaskType = "All"
                End If
                If Me._cblIncidentType.Items.FindByValue("Outage") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("Outage").Selected Then
                    taskReportParms.Outage = "Outage"
                Else
                    taskReportParms.Outage = "All"
                End If
                If Me._cblIncidentType.Items.FindByValue("Reliability") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("Reliability").Selected Then
                    taskReportParms.Reliability = "Reliability"
                Else
                    taskReportParms.Reliability = "All"
                End If
                If Me._cblIncidentType.Items.FindByValue("Environmental") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("Environmental").Selected Then
                    taskReportParms.Environmental = "Environmental"
                Else
                    taskReportParms.Environmental = "All"
                End If
                If Me._cblIncidentType.Items.FindByValue("Project/Other") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("Project/Other").Selected Then
                    taskReportParms.Project = "Project/Other"
                Else
                    taskReportParms.Project = "All"
                End If
                If Me._cblIncidentType.Items.FindByValue("Safety") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("Safety").Selected Then
                    taskReportParms.Safety = "Safety"
                Else
                    taskReportParms.Safety = "All"
                End If
                If Me._cblIncidentType.Items.FindByValue("Health & Safety") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("Health & Safety").Selected Then
                    taskReportParms.Health = "Health & Safety"
                Else
                    taskReportParms.Health = "All"
                End If
                If Me._cblIncidentType.Items.FindByValue("Quality") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("Quality").Selected Then
                    taskReportParms.Quality = "Quality"
                Else
                    taskReportParms.Quality = "All"
                End If
                'If Me._cblIncidentType.Items.FindByValue("Deliberate Improvement(ME)") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("Deliberate Improvement(ME)").Selected Then
                '    taskReportParms.DeliberateImprovement = "Deliberate Improvement(ME)"
                'Else
                '    taskReportParms.DeliberateImprovement = "All"
                'End If
                If Me._cblIncidentType.Items.FindByValue("Cost Reduction") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("Cost Reduction").Selected Then
                    taskReportParms.CostReduction = "Cost Reduction"
                Else
                    taskReportParms.CostReduction = "All"
                End If
                If Me._cblIncidentType.Items.FindByValue("Capital") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("Capital").Selected Then
                    taskReportParms.Capital = "Capital"
                Else
                    taskReportParms.Capital = "All"
                End If
                If Me._cblIncidentType.Items.FindByValue("People") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("People").Selected Then
                    taskReportParms.People = "People"
                Else
                    taskReportParms.People = "All"
                End If
                If Me._cblIncidentType.Items.FindByValue("PSM") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("PSM").Selected Then
                    taskReportParms.PSM = "PSM"
                Else
                    taskReportParms.PSM = "All"
                End If
                If Me._cblIncidentType.Items.FindByValue("NCG/TRS") IsNot Nothing AndAlso Me._cblIncidentType.Items.FindByValue("NCG/TRS").Selected Then
                    taskReportParms.NCGTRS = "NCG/TRS"
                Else
                    taskReportParms.NCGTRS = "All"
                End If
            End If
            If .TaskStatus Then
                taskReportParms.Status = Me._rblTaskStatus.SelectedValue
            End If
            If .CreatedBy Then
                'Dim createdBy As String() = Me._txtCreatedBy.Text.Split("|")
                'If createdBy.Length = 2 Then
                If taskSearch.CreatedBy.Length > 0 Then
                    taskReportParms.CreatedBy = taskSearch.CreatedBy
                Else
                    taskReportParms.CreatedBy = "All"
                End If
                'End If
            Else
                taskReportParms.CreatedBy = "All"
            End If

            If .Responsible Then
                'Dim Responsible As String() = Me._txtResponsiblePerson.Text.Split("|")
                'If Responsible.Length = 2 Then
                If taskSearch.Responsible.Length > 0 Then
                    taskReportParms.ResponsiblePerson = taskSearch.Responsible 'Responsible(1).Trim
                    'End If
                Else
                    taskReportParms.ResponsiblePerson = "All"
                End If
            Else
                taskReportParms.ResponsiblePerson = "All"
            End If
            If .Role Then
                'Dim Responsible As String() = Me._txtResponsiblePerson.Text.Split("|")
                'If Responsible.Length = 2 Then
                If taskSearch.Role.Length > 0 Then
                    taskReportParms.Role = taskSearch.Role 'Responsible(1).Trim
                    taskReportParms.RoleId = taskSearch.RoleId
                    'End If
                Else
                    taskReportParms.Role = "All"
                    taskReportParms.RoleId = "All"
                End If
            Else
                taskReportParms.Role = "All"
            End If
            If .DueDate And taskReportParms.Status <> "NewOpen" Then
                If IsDate(Me._dtDueDate.StartDate) And IsDate(Me._dtDueDate.EndDate) Then
                    Dim dueDate As DateTime = CDate(Me._dtDueDate.StartDate)
                    taskReportParms.StartDay = dueDate.Day
                    taskReportParms.StartMonth = dueDate.Month
                    taskReportParms.Startyear = dueDate.Year

                    dueDate = CDate(Me._dtDueDate.EndDate)
                    taskReportParms.EndDay = dueDate.Day
                    taskReportParms.EndMonth = dueDate.Month
                    taskReportParms.EndYear = dueDate.Year
                Else
                    Dim dueDate As DateTime = Now.AddYears(-20)
                    taskReportParms.StartDay = dueDate.Day
                    taskReportParms.StartMonth = dueDate.Month
                    taskReportParms.Startyear = dueDate.Year

                    dueDate = Now.AddYears(20)
                    taskReportParms.EndDay = dueDate.Day
                    taskReportParms.EndMonth = dueDate.Month
                    taskReportParms.EndYear = dueDate.Year
                End If
            End If
            'If .EstimatedDueDate And taskReportParms.Status = "Open" Then
            '    If Me._cblEstimatedDueDateRange.SelectedValue.Length = 0 Then
            '        Me._cblEstimatedDueDateRange.CheckAll()
            '    End If
            '    If Me._cblEstimatedDueDateRange.Items.FindByValue("All") IsNot Nothing AndAlso Me._cblEstimatedDueDateRange.Items.FindByValue("All").Selected = True Then
            '        taskReportParms.DueDate = "All"
            '    Else
            '        taskReportParms.DueDate = ""

            '        If Me._cblEstimatedDueDateRange.Items.FindByValue("Next 7 Days") IsNot Nothing AndAlso Me._cblEstimatedDueDateRange.Items.FindByValue("Next 7 Days").Selected = True Then
            '            taskReportParms.Next7Days = "Next 7 Days"
            '        Else
            '            taskReportParms.Next7Days = ""
            '        End If
            '        If Me._cblEstimatedDueDateRange.Items.FindByValue("Next 14 Days") IsNot Nothing AndAlso Me._cblEstimatedDueDateRange.Items.FindByValue("Next 14 Days").Selected = True Then
            '            taskReportParms.Next14Days = "Next 14 Days"
            '        Else
            '            taskReportParms.Next14Days = ""
            '        End If
            '        If Me._cblEstimatedDueDateRange.Items.FindByValue("Next 30 Days") IsNot Nothing AndAlso Me._cblEstimatedDueDateRange.Items.FindByValue("Next 30 Days").Selected = True Then
            '            taskReportParms.Next30Days = "Next 30 Days"
            '        Else
            '            taskReportParms.Next30Days = ""
            '        End If
            '        If Me._cblEstimatedDueDateRange.Items.FindByValue("Overdue") IsNot Nothing AndAlso Me._cblEstimatedDueDateRange.Items.FindByValue("Overdue").Selected = True Then
            '            taskReportParms.Overdue = "Overdue"
            '        Else
            '            taskReportParms.Overdue = ""
            '        End If
            '    End If


            'End If
            If .HeaderDate Then
                If IsDate(Me._dtHeaderDate.StartDate) Then
                    Dim headerDate As DateTime = CDate(Me._dtHeaderDate.StartDate)
                    taskReportParms.StartDay = headerDate.Day
                    taskReportParms.StartMonth = headerDate.Month
                    taskReportParms.Startyear = headerDate.Year
                Else
                    Dim headerDate As DateTime = Now.AddYears(-10)
                    taskReportParms.StartDay = headerDate.Day
                    taskReportParms.StartMonth = headerDate.Month
                    taskReportParms.Startyear = headerDate.Year
                End If
                If IsDate(Me._dtHeaderDate.EndDate) Then
                    Dim headerDate As DateTime = CDate(Me._dtHeaderDate.EndDate)
                    taskReportParms.EndDay = headerDate.Day
                    taskReportParms.EndMonth = headerDate.Month
                    taskReportParms.EndYear = headerDate.Year

                Else
                    Dim headerDate As DateTime = Now.AddYears(10)
                    taskReportParms.EndDay = headerDate.Day
                    taskReportParms.EndMonth = headerDate.Month
                    taskReportParms.EndYear = headerDate.Year
                End If
            End If
            If .TaskListing Then
                taskReportParms.TaskListing = Me._rblTaskListing.SelectedValue
            Else
                taskReportParms.TaskListing = String.Empty
            End If
            taskReportParms.Person = taskSearch.UserName

            If .Title Then
                If Me._txtTitle.Text.Length > 0 Then
                    taskReportParms.Title = Me._txtTitle.Text
                Else
                    taskReportParms.Title = "All"
                End If
            End If
            If .SourceSystem Then
                If Me._ddlSourceSystem.SelectedItem IsNot Nothing Then
                    taskReportParms.SourceSystem = Me._ddlSourceSystem.SelectedItem.Value
                End If
                taskReportParms.SourceReference = Me._txtIncidentEventNumber.Text
            End If

            If .GMSElement Then
                If Me._ddlSubElement.SelectedItem IsNot Nothing Then
                    taskReportParms.GMSElement = Me._ddlSubElement.SelectedItem.Value
                Else
                    taskReportParms.GMSElement = "All"
                End If
            End If

            If .TaskHeader Then
                If Me._txtHeaderNumber.Text.Length > 0 Then
                    taskReportParms.TaskHeader = Me._txtHeaderNumber.Text
                Else
                    taskReportParms.TaskHeader = "All"
                End If
            End If

        End With
        With taskReportParms
            Dim lang = System.Globalization.CultureInfo.CurrentCulture
            mttReportQry = String.Format(CultureInfo.CurrentCulture, mttReportQry, .Report, .Division, .Region, .PlantName, .BusUnit, .Area, .LineSystem, .LineBreak, .Status, .Safety, .Quality, .Environmental, .Reliability, .Project, .Health, .Outage, .DueDate, .Overdue, .Next7Days, .Next14Days, .Next30Days, .StartMonth, .StartDay, .Startyear, .EndDay, .EndMonth, .EndYear, .CreatedBy, .ResponsiblePerson, .Complaint, .Permit, .Audit, .Inspection, .Incident, .MOC, .TaskType, .Activity, .Person, .TaskListing, .SourceSystem, .SourceReference, .Title, .Other, .NOV, .Order, .Citation, .ComplianceCertification, .CostReduction, .Capital, .People, .GMS, Server.UrlEncode(.Role), .GMSElement, .SRRLearning, .DeliberateImprovement, .ManufacturingAlert, .PSM, .NCGTRS, .TypeList, .ActivityList, .TaskHeader, lang, .RoleId)
        End With

        'display in new window
        'Dim url As String = String.Format("http://{0}.ipaper.com/CEReporting/CrystalReportDisplay.aspx" & mttReportQry, IP.Bids.SharedFunctions.GetServerName)
        Dim url As String = String.Format("http://gpimv.graphicpkg.com/CEReporting/CrystalReportDisplay.aspx" & mttReportQry, IP.Bids.SharedFunctions.GetServerName)

        '_lblReportUrl.Text = url
        '_lblReportUrl.Attributes.Add("Display", "none")
        'Page.ClientScript.RegisterStartupScript(Page.GetType, "pop", "PopupWindow('" & url & "','MonthlyReport',800,600,'yes','no','true');", True)
        Web.UI.ScriptManager.RegisterStartupScript(Me._udpCalendarView, _udpCalendarView.GetType, "pop", "PopupWindow('" & url & "','MonthlyReport',800,600,'yes','no','true');", True)
        Exit Sub
        'end display in new window
    End Sub



    Private Sub ReselectActiveTab(ByVal taskSearch As TaskListingBll)
        If Not IsNumeric(taskSearch.TaskSearchView) Then
            currentTab = 2
            If _ddlSearchOptions.Items.FindByValue(currentTab) IsNot Nothing Then
                _ddlSearchOptions.ClearSelection()
                _ddlSearchOptions.Items.FindByValue(currentTab).Selected = True
            End If
            Exit Sub
        End If
        If taskSearch.HeaderSeqID.Length > 0 Then taskSearch.TaskSearchView = 2
        If IsNumeric(taskSearch.TaskSearchView) Then
            If CInt(taskSearch.TaskSearchView) >= 0 And CInt(taskSearch.TaskSearchView) <= 2 Then
                currentTab = taskSearch.TaskSearchView
                If _ddlSearchOptions.Items.FindByValue(currentTab) IsNot Nothing Then
                    _ddlSearchOptions.ClearSelection()
                    _ddlSearchOptions.Items.FindByValue(currentTab).Selected = True
                End If
            End If
        End If
    End Sub

    Private Sub DisplaySearchResults()
        Dim taskSearch As TaskListingBll

        If Session.Item("TaskListing") IsNot Nothing Then
            Session.Remove("TaskListing")
        End If

        taskSearch = CollectSearchCriteria()
        _tabSearchResults.Visible = False
        _tabSearchTabs.Visible = False
        ReselectActiveTab(taskSearch)
        PersistSearchCriteria(taskSearch)
        Dim listing As System.Collections.Generic.List(Of TaskListing)
        listing = taskSearch.GetHeaderItemListing(False)
        DisplayTaskList(listing)
        DisplayCalendarView(listing)

        _pnlCalendarView.Visible = True
        _pnlCalendarListing.Visible = True
        'ReselectControls()
        '_pnlDayView.Visible = False
        '_pnlTaskListing.Visible = True
    End Sub


    'Protected Sub _calDay_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles _calDay.DayRender
    '    Dim startDate As Date = _calDay.VisibleDate


    '    Dim dt As String = e.Day.Date.ToLongDateString

    '    If e.Day.Date = Now.Date Then
    '        e.Cell.Font.Bold = True
    '    End If
    '    If e.Day.Date <> startDate Then
    '        e.Cell.Text = ""
    '        e.Cell.Height = 0
    '        e.Cell.Style.Item("display") = "none"
    '        Exit Sub
    '    Else
    '        e.Cell.Style.Item("display") = ""
    '        e.Cell.Height = Unit.Percentage(100) ' Unit.Pixel(400)
    '        e.Cell.Style.Item("min-height") = "400px"
    '        e.Cell.Width = Unit.Percentage(100)
    '    End If

    '    If TaskListing IsNot Nothing AndAlso TaskListing(e.Day.Date.ToShortDateString) IsNot Nothing Then
    '        dt = dt & "<br/><br/>" & TaskListing(e.Day.Date.ToShortDateString)
    '    End If

    '    e.Cell.VerticalAlign = VerticalAlign.Top
    '    e.Cell.HorizontalAlign = HorizontalAlign.Left
    '    e.Cell.Text = dt
    'End Sub

    Private Sub CreateCalendarHtml(ByVal cal As WebControls.Calendar)
        Dim strBody As String = String.Empty
        strBody = "<html xmlns:o='urn:schemas-microsoft-com:office:office' " & "xmlns:w='urn:schemas-microsoft-com:office:word'" & "xmlns='http://www.w3.org/TR/REC-html40'>"

        strBody = strBody & "<!--[if gte mso 9]>" & "<xml>" & "<w:WordDocument>" & "<w:View>Print</w:View>" & "<w:Zoom>100</w:Zoom>" & "</w:WordDocument>" & "</xml>" & "<![endif]-->"

        Dim wordHTML As String = String.Format("<h2>{0}</h2>", MonthName(cal.VisibleDate.Month) & " " & cal.VisibleDate.Year) & GetRenderedControl(cal)
        If Session.Item("WordXML") IsNot Nothing Then
            Session.Remove("WordXML")
        End If
        'wordHTML = wordHTML.Replace("<img src='/EHSTaskTracker/Images", "<img src='http://ri/Tasktracker/Images")
        Session.Add("WordXML", strBody & wordHTML)
    End Sub
    'Protected Sub _ddlMonthViewSelector_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ddlMonthViewSelector.SelectedIndexChanged
    '    Me.DisplaySearchResults()
    '    If Session.Item("TaskListing") IsNot Nothing Then
    '        TaskListing = Session.Item("TaskListing")
    '    End If
    '    _cal.VisibleDate = CDate(Me._ddlMonthViewSelector.SelectedValue)

    '    CreateCalendarHtml(_cal)
    'End Sub

    'Protected Sub _ddlDayViewSelector_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ddlDayViewSelector.SelectedIndexChanged
    '    Dim startDate As Date = CDate(Me._ddlDayViewSelector.SelectedValue)

    '    If Session.Item("TaskListing") IsNot Nothing Then
    '        TaskListing = Session.Item("TaskListing")
    '        _calDay.VisibleDate = startDate
    '    End If
    'End Sub

    'Protected Sub _ddlWeekViewSelector_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ddlWeekViewSelector.SelectedIndexChanged
    '    Dim startDate As Date = CDate(Me._ddlWeekViewSelector.SelectedValue)
    '    Me.DisplaySearchResults()
    '    startDate = IP.Bids.SharedFunctions.FirstDayOfWeek(startDate)

    '    If Session.Item("TaskListing") IsNot Nothing Then
    '        TaskListing = Session.Item("TaskListing")
    '        _calWeek.VisibleDate = startDate
    '    End If
    '    CreateCalendarHtml(_calWeek)
    'End Sub

    Protected Sub _btnExportTasks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnExportTasks.Click
        Dim taskSearch As TaskListingBll

        taskSearch = CollectSearchCriteria()
        _pnlCalendarView.Visible = False
        '_pnlWeekView.Visible = False
        '_pnlDayView.Visible = False
        _pnlCalendarListing.Visible = False
        ExportToExcel(taskSearch)
        'Me.DisplaySearchResults()
        'IP.Bids.SharedFunctions.ResponseRedirect(Request.Url.AbsolutePath)
    End Sub
    'Protected Sub _btnExportWord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnExportWord.click
    '    _pnlCalendarView.Visible = True
    '    _pnlWeekView.Visible = False
    '    _pnlDayView.Visible = False
    '    _pnlCalendarListing.Visible = False
    '    'CollectSearchCriteria(SearchView.Word)
    '    If Session.Item("TaskListing") IsNot Nothing Then
    '        TaskListing = Session.Item("TaskListing")
    '    End If
    '    _cal.VisibleDate = CDate(Me._ddlMonthViewSelector.SelectedValue)

    '    Dim btn As ModalIframePopup
    '    btn = CType(LoadControl("~\User Controls\ModalIframPopup.ascx"), ModalIframePopup)
    '    With btn
    '        '.Attributes.Add("display", "none")
    '        .HideDisplayButton()
    '        .BannerText = "MS Word"
    '        .Url = "Popups/Word.aspx"
    '        Me.UpdatePanel1.Controls.Item(0).Controls.Add(btn)
    '        .LoadIFrame()
    '        'ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "ExcelJS", .TriggerPopupJS, True)
    '    End With
    'End Sub
    'Protected Sub _btnExportWordWeek_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnExportWordWeek.Click
    '    _pnlCalendarView.Visible = False
    '    _pnlWeekView.Visible = True
    '    _pnlDayView.Visible = False
    '    _pnlCalendarListing.Visible = False
    '    'CollectSearchCriteria(SearchView.Word)
    '    If Session.Item("TaskListing") IsNot Nothing Then
    '        TaskListing = Session.Item("TaskListing")
    '    End If
    '    _calWeek.VisibleDate = CDate(Me._ddlWeekViewSelector.SelectedValue)

    '    Dim btn As ModalIframePopup
    '    btn = CType(LoadControl("~\User Controls\ModalIframPopup.ascx"), ModalIframePopup)
    '    With btn
    '        '.Attributes.Add("display", "none")
    '        .HideDisplayButton()
    '        .BannerText = "MS Word"
    '        .Url = "Popups/Word.aspx"
    '        Me.UpdatePanel1.Controls.Item(0).Controls.Add(btn)
    '        .LoadIFrame()
    '        'ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "ExcelJS", .TriggerPopupJS, True)
    '    End With
    'End Sub

    Protected Sub _ResponsiblePerson_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ResponsiblePerson.Load
        '_txtResponsiblePerson.Attributes.Add("onclick", Me._ResponsiblePerson.GetShowUserJS)
        '_txtCreatedBy.Attributes.Add("onclick", Me._ResponsiblePerson.GetShowUserJS)
    End Sub

    Protected Sub _rblTaskStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _rblTaskStatus.SelectedIndexChanged
        DisplaySearchSelections()
    End Sub
#End Region



    Protected Sub _ddlReportTitles_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ddlReportTitles.SelectedIndexChanged
        PopulateData()
        SetReportDefaults()
    End Sub

    Public Sub SetReportDefaults()
        Dim taskSearch As TaskListingBll = Nothing

        Dim defaults As RIUserDefaults.CurrentUserDefaults = Nothing
        If IP.Bids.SharedFunctions.GetCurrentUser IsNot Nothing AndAlso IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults IsNot Nothing Then
            defaults = IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults
        End If
        If defaults Is Nothing Then Exit Sub

        Dim searchXml = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.TaskSearchCriteria, Nothing)
        taskSearch = TaskListingBll.LoadXML(searchXml)
        If taskSearch Is Nothing Then Exit Sub

        With taskSearch
            If .Division.Length > 0 Then
                Me._ReportingSite.DefaultDivision = .Division
            End If
            If .Region.Length > 0 Then
                Me._ReportingSite.DefaultRegion = .Region
            End If
            If .PlantCode.Length > 0 Then
                Me._ReportingSite.DefaultFacility = .PlantCode
            End If
            If .BusinessUnit.Length > 0 Then
                Me._ReportingSite.DefaultBusinessUnit = .BusinessUnit
            End If
            If .Area.Length > 0 Then
                Me._ReportingSite.DefaultArea = .Area
            End If
            If .Line.Length > 0 Then
                Me._ReportingSite.DefaultLine = .Line
            End If
            If .Machine.Length > 0 Then
                Me._ReportingSite.DefaultLineBreak = .Machine
            End If
            If .Activity.Length > 0 Then
                Me._cblActivity.ClearSelection()
                Me._cblActivity.SelectedValue = GetActivityNames(.Activity)
            End If
            If .Type.Length > 0 Then
                Me._cblIncidentType.ClearSelection()
                Me._cblIncidentType.SelectedValue = .Type
            End If
            If .Description.Length > 0 Then
                Me._txtDescription.Text = .Description
            End If
            If .Title.Length > 0 Then
                Me._txtTitle.Text = .Title
            End If
            If .SourceSytemRef.Length > 0 Then
                Me._txtIncidentEventNumber.Text = .SourceSytemRef
            End If
            If .SourceSystem.Length > 0 Then
                'Note: FindByValue does a case sensitive search
                If Me._ddlSourceSystem.Items.FindByValue(.SourceSystem) IsNot Nothing Then
                    Me._ddlSourceSystem.ClearSelection()
                    Me._ddlSourceSystem.Items.FindByValue(.SourceSystem).Selected = True
                End If
            End If
            If .TaskStatus.Length > 0 Then
                Me._cblTaskStatus.ClearSelection()
                Me._cblTaskStatus.SelectedValue = .TaskStatus.Replace(",", "|")
            End If
            If .HeaderSeqID.Length > 0 Then
                Me._txtHeaderNumber.Text = .HeaderSeqID
            End If
            If .HeaderFromDate.Length > 0 AndAlso IsDate(.HeaderFromDate) Then
                Me._dtHeaderDate.StartDate = .HeaderFromDate
            End If
            If .HeaderToDate.Length > 0 AndAlso IsDate(.HeaderToDate) Then
                Me._dtHeaderDate.EndDate = .HeaderToDate
            End If
            If IsNumeric(.HeaderDateRange) AndAlso CInt(.HeaderDateRange) > -1 Then
                If IsNumeric(.HeaderDateRange) Then _dtHeaderDate.DateRange = CInt(.HeaderDateRange)
            End If
            If .DueDateFrom.Length > 0 AndAlso IsDate(.DueDateFrom) Then
                Me._dtDueDate.StartDate = .DueDateFrom
            End If
            If .DueDateTo.Length > 0 AndAlso IsDate(.DueDateTo) Then
                Me._dtDueDate.EndDate = .DueDateTo
            End If
            If IsNumeric(.DueDateDateRange) AndAlso CInt(.DueDateDateRange) > -1 Then
                If IsNumeric(.DueDateDateRange) Then _dtDueDate.DateRange = CInt(.DueDateDateRange)
            End If
            If .ClosedDateFrom.Length > 0 AndAlso IsDate(.ClosedDateFrom) Then
                Me._dtClosedDate.StartDate = .ClosedDateFrom
            End If
            If .ClosedDateTo.Length > 0 AndAlso IsDate(.ClosedDateTo) Then
                Me._dtClosedDate.EndDate = .ClosedDateTo
            End If
            If IsNumeric(.CloseDateRange) AndAlso CInt(.CloseDateRange) > -1 Then
                If IsNumeric(.CloseDateRange) Then _dtClosedDate.DateRange = CInt(.CloseDateRange)
            End If
            'If .Responsible.Length > 0 Then

            If IsNumeric(.Responsible) Then 'Role
                Me._ResponsiblePerson.PlantCode = .ResponsiblePlantCode
                Me._ResponsiblePerson.PopulateEmployeeList(.Responsible)
                'Me._ResponsiblePerson.DefaultUserName = .Responsible
            Else
                Me._ResponsiblePerson.PopulateEmployeeList(.Responsible)
                'Me._ResponsiblePerson.DefaultUserName = .Responsible
            End If
            'End If
            If .CreatedBy.Length > 0 Then
                Me._CreatedBy.PlantCode = .CreatedByPlantCode
                Me._CreatedBy.DefaultUserName = .CreatedBy
                Me._CreatedBy.PopulateEmployeeList()
            Else
                Me._CreatedBy.PlantCode = .CreatedByPlantCode
                Me._CreatedBy.PopulateEmployeeList()
            End If
            If .SecurityLevel.Length > 0 Then
                If .SecurityLevel = "Y" Then
                    Me._cbHighLevelSecurity.Checked = True
                Else
                    Me._cbHighLevelSecurity.Checked = False
                End If
            End If


        End With
    End Sub

    'Protected Sub _btnResetReportSelections_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnResetReportSelections.Click
    '    Me._ddlReportTitles.SelectedIndex = 0
    '    'Response.Redirect(Page.ResolveUrl(Page.AppRelativeVirtualPath), True)
    'End Sub


    'Protected Sub _gvCalendarListing_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles _gvCalendarListing.RowCreated
    '    Dim _phTaskStatus As PlaceHolder = e.Row.FindControl("_phTaskStatus")
    '    If _phTaskStatus IsNot Nothing Then
    '        'Dim HeaderItem As New System.Collections.Generic.List(Of IP.MEAS.BO.TaskListing)
    '        'HeaderItem.Add(CType(e.Row.DataItem, IP.MEAS.BO.TaskListing))
    '        Dim HeaderItem As IP.MEAS.BO.TaskListing = CType(e.Row.DataItem, IP.MEAS.BO.TaskListing)
    '        Dim taskSearch As New TaskListingBLL
    '        If HeaderItem IsNot Nothing Then
    '            'taskSearch.HeaderSeqID = HeaderItem.TaskHeaderSeqId
    '            'Dim taskListingdata As System.Collections.Generic.List(Of TaskListing) = Nothing 'taskSearch.GetHeaderItemListing("DueDate")
    '            Dim headerStatus As String() = HeaderItem.HeaderStatus.Split(",")
    '            For Each item As String In headerStatus
    '                Dim itemData() As String = item.Split("@")
    '                If itemData.Length = 3 Then
    '                    Dim hyp As New HyperLink
    '                    hyp.ID = "_hypTaskItem_" & itemData(1)
    '                    If _phTaskStatus.Controls.Count > 0 Then
    '                        _phTaskStatus.Controls.Add(New LiteralControl("<br>"))
    '                    End If
    '                    hyp.NavigateUrl = "Javascript:parent.window.location=('" & String.Format(Page.ResolveUrl("~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}"), HeaderItem.TaskHeaderSeqId, itemData(2)) & "');"
    '                    hyp.Text = GetTaskStatus(itemData(0), itemData(1), itemData(1), True) & CDate(itemData(0)).ToShortDateString
    '                    _phTaskStatus.Controls.Add(hyp)
    '                    '_phTaskStatus.Controls.Add(New LiteralControl(item.DueDate))
    '                End If
    '            Next

    '        End If
    '    End If
    'End Sub

    Protected Sub _btnDisplaySearchResults_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnDisplaySearchResults.Click
        Me.DisplaySearchResults()
    End Sub

    Private Sub SaveSearchCriteria_Click(sender As Object, e As EventArgs) Handles _btnSaveSearchCriteria.Click
        Dim taskSearch As TaskListingBll
        taskSearch = CollectSearchCriteria()
        SaveTaskTrackerSearchCriteria(taskSearch)
        PersistSearchCriteria(taskSearch)
        IP.Bids.SharedFunctions.ClearCurrentUserSession()
        Dim currentUser = IP.Bids.SharedFunctions.GetCurrentUser()
        ReselectControls()
        If Me._tabSearchResults.Visible = True Then
            DisplaySearchResults()
        End If
        Me._cpeSearchSelections.Collapsed = False
        Me._cpeSearchSelections.ClientState = "false"
    End Sub

    Private Sub _btnResetSearchCriteria_Click(sender As Object, e As EventArgs) Handles _btnResetSearchCriteria.Click
        IP.Bids.SharedFunctions.ResponseRedirect(Request.Url.AbsolutePath)
    End Sub


    Private Sub UserControlsTaskSearch_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'Web.UI.ScriptManager.RegisterStartupScript(Me._udpCalendarView, _udpCalendarView.GetType, "setActiveTab", " var currentTabNumber=" & currentTab & ";SetActiveTab(" & currentTab & ");", True)
        _cblActivity.RepeatLayout = RepeatLayout.Flow
        _cblActivity.RepeatDirection = RepeatDirection.Horizontal
        _cblActivity.RepeatColumns = 12
        For Each item As ListItem In _cblActivity.Items
            item.Attributes.Add("class", "col-xs-12 col-sm-6 col-md-4")
        Next
        _cblIncidentType.RepeatLayout = RepeatLayout.Flow
        _cblIncidentType.RepeatDirection = RepeatDirection.Horizontal
        _cblIncidentType.RepeatColumns = 12
        For Each item As ListItem In _cblIncidentType.Items
            item.Attributes.Add("class", "col-xs-12 col-sm-6 col-md-4")
        Next
        _cblTaskStatus.RepeatLayout = RepeatLayout.Flow
        _cblTaskStatus.RepeatDirection = RepeatDirection.Horizontal
        _cblTaskStatus.RepeatColumns = 12
        For Each item As ListItem In _cblTaskStatus.Items
            If item.Text.Length < 6 Then
                item.Attributes.Add("class", "col-xs-12 col-sm-2 col-md-2 col-lg-4")
            Else
                item.Attributes.Add("class", "col-xs-12 col-sm-5 col-md-5 col-lg-4")
            End If

        Next
    End Sub

    Private Sub _gvCalendarListing_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles _gvCalendarListing.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).Attributes("data-title") = _gvCalendarListing.Columns(i).HeaderText
            Next
        End If
    End Sub

End Class

'Public Class DistinctHeaderSeqID
'    Implements IEqualityComparer(Of TaskListing)
'    Public Function Equals(ByVal x As Integer, ByVal y As Integer) As Boolean
'        Return x.title.Equals(y.title)
'    End Function

'    Public Function GetHashCode(ByVal obj As SourceType) As Integer
'        Return obj.title.GetHashCode()
'    End Function
'End Class
