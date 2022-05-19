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
Option Explicit On
Option Strict On
Imports System.Globalization

Partial Class PopupsRecurrence
    Inherits IP.Bids.BasePage

#Region "Fields"
    Private currentpattern As New RecurrenceProfile
    Private currentPatternSeqID As New RecurrenceProfileSeqID
    Private taskSeqId As Integer
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandlePageLoad()

    End Sub
    Private Structure RecurrenceProfile
        Dim RecurrencePattern As String
        Dim Occurrences As Integer
        Dim RepeatUnitsQty As Integer
        Dim RangeOfRecurrence As Integer
        Dim SpecificDay As Integer
        Dim SpecificMonth As Integer
        Dim DaysOfWeek As String
        Dim Ordinals As Integer
        Dim EndByDate As Date

        Public Overloads Overrides Function GetHashCode() As Integer
            Throw New NotImplementedException()
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As [Object]) As Boolean
            Throw New NotImplementedException()
        End Function

        Public Shared Operator =(ByVal left As RecurrenceProfile, ByVal right As RecurrenceProfile) As Boolean
            Throw New NotImplementedException()
        End Operator

        Public Shared Operator <>(ByVal left As RecurrenceProfile, ByVal right As RecurrenceProfile) As Boolean
            Throw New NotImplementedException()
        End Operator
    End Structure
    Private Structure RecurrenceProfileSeqID
        Dim RecurrencePattern As Integer
        Dim Occurrences As Integer
        Dim RepeatUnitsQty As Integer
        Dim RangeOfRecurrence As Integer
        Dim SpecificDay As Integer
        Dim SpecificMonth As Integer
        Dim DaysOfWeek As Integer
        Dim Ordinals As Integer
        Dim EndByDate As Integer

        Public Overloads Overrides Function GetHashCode() As Integer
            Throw New NotImplementedException()
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As [Object]) As Boolean
            Throw New NotImplementedException()
        End Function

        Public Shared Operator =(ByVal left As RecurrenceProfileSeqID, ByVal right As RecurrenceProfileSeqID) As Boolean
            Throw New NotImplementedException()
        End Operator

        Public Shared Operator <>(ByVal left As RecurrenceProfileSeqID, ByVal right As RecurrenceProfileSeqID) As Boolean
            Throw New NotImplementedException()
        End Operator
    End Structure

    Public Sub GetRecurrenceData(ByVal taskItemNumber As Integer)
        Dim taskItemBLL As New TaskTrackerItemBll
        Dim recurrenceList As System.Collections.Generic.List(Of RecurringParameters)

        taskSeqId = taskItemNumber
        With taskItemBLL
            recurrenceList = .GetRecurringParameters(taskItemNumber)
        End With
        taskItemBLL = Nothing
        currentpattern.EndByDate = New Date(1901, 1, 1) 'Default date



        For Each record As RecurringParameters In recurrenceList
            Select Case record.ProfileTypeName
                Case "RecurrencePattern"
                    currentpattern.RecurrencePattern = record.ProfileTypeValue
                Case "Occurrences"
                    If record.ProfileTypeValue IsNot Nothing AndAlso IsNumeric(record.ProfileTypeValue) Then
                        currentpattern.Occurrences = CInt(record.ProfileTypeValue)
                        'Else
                        '    currentpattern.Occurrences = 1
                    End If
                Case "RepeatUnitsQty"
                    If record.ProfileTypeValue IsNot Nothing AndAlso IsNumeric(record.ProfileTypeValue) Then
                        currentpattern.RepeatUnitsQty = CInt(record.ProfileTypeValue)
                        'Else
                        '    currentpattern.RepeatUnitsQty = 1
                    End If
                Case "RangeOfRecurrence"
                    If record.ProfileTypeValue IsNot Nothing AndAlso IsNumeric(record.ProfileTypeValue) Then
                        currentpattern.RangeOfRecurrence = CInt(record.ProfileTypeValue)
                        'Else
                        '    currentpattern.RangeOfRecurrence = 1
                    End If
                Case "SpecificDay"
                    If record.ProfileTypeValue IsNot Nothing AndAlso IsNumeric(record.ProfileTypeValue) Then
                        currentpattern.SpecificDay = CInt(record.ProfileTypeValue)
                        'Else
                        '    currentpattern.SpecificDay = 1
                    End If
                Case "SpecificMonth"
                    If record.ProfileTypeValue IsNot Nothing AndAlso IsNumeric(record.ProfileTypeValue) Then
                        currentpattern.SpecificMonth = CInt(record.ProfileTypeValue)
                        'Else
                        '    currentpattern.SpecificMonth = 1
                    End If
                Case "DaysOfWeek"
                    currentpattern.DaysOfWeek = record.ProfileTypeValue
                Case "Ordinals"
                    If record.ProfileTypeValue IsNot Nothing AndAlso IsNumeric(record.ProfileTypeValue) Then
                        currentpattern.Ordinals = CInt(record.ProfileTypeValue)
                    End If
                Case "EndByDate"
                    If record.ProfileTypeValue IsNot Nothing AndAlso IsDate(record.ProfileTypeValue) Then
                        currentpattern.EndByDate = CDate(record.ProfileTypeValue)
                    End If
            End Select
        Next

        With currentpattern
            'Note: FindByValue does a case sensitive search
            If Me._rblFrequency.Items.FindByValue(.RecurrencePattern) IsNot Nothing Then
                Me._rblFrequency.Items.FindByValue(.RecurrencePattern).Selected = True
                Me._rblFrequency.SelectedValue = .RecurrencePattern
            End If
            HandleSelectedFrequency()

            Select Case _rblFrequency.SelectedValue.ToLower
                Case "daily"
                    If .RepeatUnitsQty = 0 Then
                        _txtEveryDay.Text = CStr(1)
                        Me._rbEndAfter.Checked = True
                        Me._txtEndAfter.Text = CStr(1)
                    Else
                        _txtEveryDay.Text = CStr(.RepeatUnitsQty)
                    End If


                Case "weekly"
                    _txtEveryXWeeks.Text = CStr(.RepeatUnitsQty)
                    Dim daysofWeek As String() = .DaysOfWeek.Split(CChar(","))
                    If daysofWeek.Length > 0 Then
                        With Me._cblDaysOfWeek
                            .ClearSelection()
                            For i As Integer = 0 To daysofWeek.Length - 1
                                If .Items.FindByValue(daysofWeek(i)) IsNot Nothing Then
                                    .Items.FindByValue(daysofWeek(i)).Selected = True
                                End If
                            Next
                        End With
                    End If
                Case "monthly"
                    If .Ordinals > 0 Then
                        'Ordinal week day or Last day of month
                        If .Ordinals = 999 And .DaysOfWeek Is Nothing Then 'Last day of month
                            _rblLastDayOfMonth.Checked = True
                            _txtLastDayEveryXMonths.Text = CStr(.RepeatUnitsQty)
                        Else
                            _rbWeekDayOfMonth.Checked = True
                            If _ddlOrdinalDayOfMonth.Items.FindByValue(CStr(.Ordinals)) IsNot Nothing Then
                                _ddlOrdinalDayOfMonth.Items.FindByValue(CStr(.Ordinals)).Selected = True
                            End If
                            If _ddlMonthDayOfWeek.Items.FindByValue(CStr(.DaysOfWeek)) IsNot Nothing Then
                                _ddlMonthDayOfWeek.Items.FindByValue(CStr(.DaysOfWeek)).Selected = True
                            End If
                            _txtDayOfWeekForEveryXMonths.Text = CStr(.RepeatUnitsQty)
                        End If
                    Else
                        _rbDayXOfMonth.Checked = True
                        _txtDayXOfMonth.Text = CStr(.SpecificDay)
                        _txtEveryXMonths.Text = CStr(.RepeatUnitsQty)
                    End If
                    'Case "quarterly"
                    '    If .Ordinals > 0 Then
                    '        'Ordinal week day or Last day of month
                    '        If .Ordinals = 999 Then 'Last day of month
                    '            _rblLastDayOfQuarter.Checked = True
                    '        Else
                    '            _rbWeekDayOfQuarter.Checked = True
                    '            If _ddlOrdinalDayOfQuarter.Items.FindByValue(CStr(.Ordinals)) IsNot Nothing Then
                    '                _ddlOrdinalDayOfQuarter.Items.FindByValue(CStr(.Ordinals)).Selected = True
                    '            End If
                    '            If _ddlQuarterDayOfWeek.Items.FindByValue(CStr(.SpecificDay)) IsNot Nothing Then
                    '                _ddlQuarterDayOfWeek.Items.FindByValue(CStr(.SpecificDay)).Selected = True
                    '            End If
                    '            _txtDayOfWeekForEveryXMonths.Text = CStr(.RepeatUnitsQty)
                    '        End If
                    '    Else
                    '        _rblDayXOfQuarter.Checked = True
                    '        _txtDayXOfQuarter.Text = CStr(.SpecificDay)
                    '    End If
                Case "yearly"
                    If .Ordinals > 0 Then
                        _rbWeekDayOfYear.Checked = True

                        If _ddlOrdinalDayOfYear.Items.FindByValue(CStr(.Ordinals)) IsNot Nothing Then
                            _ddlOrdinalDayOfYear.Items.FindByValue(CStr(.Ordinals)).Selected = True
                        End If
                        If _ddlYearlyDayOfWeek.Items.FindByValue(CStr(.DaysOfWeek)) IsNot Nothing Then
                            _ddlYearlyDayOfWeek.Items.FindByValue(CStr(.DaysOfWeek)).Selected = True
                        End If
                        If _ddlMonthsOfYear.Items.FindByValue(CStr(.SpecificMonth)) IsNot Nothing Then
                            _ddlMonthsOfYear.Items.FindByValue(CStr(.SpecificMonth)).Selected = True
                        End If
                        _txtWeekDayOfXYears.Text = CStr(.RepeatUnitsQty)
                    Else
                        _rblDayXOfYear.Checked = True
                        _txtDayXOfYear.Text = CStr(.SpecificDay)

                        If _ddlDayXMonthsOfYear.Items.FindByValue(CStr(.SpecificMonth)) IsNot Nothing Then
                            _ddlDayXMonthsOfYear.Items.FindByValue(CStr(.SpecificMonth)).Selected = True
                        End If
                        _txtDayOfWeekForEveryXYears.Text = CStr(.RepeatUnitsQty)
                    End If
                Case Else
                    Me._pnlRangeOfRecurrence.Visible = False
            End Select

            If Me._pnlRangeOfRecurrence.Visible Then
                If .Occurrences > 0 Then
                    _rbEndAfter.Checked = True
                    _txtEndAfter.Text = CStr(.Occurrences)
                Else
                    If .EndByDate > New Date(2000, 1, 1) Then
                        _rbEndBy.Checked = True
                        _dtEndbyDate.StartDate = CStr(.EndByDate)
                    Else
                        _rbNoEndDate.Checked = True
                    End If
                End If
            End If
        End With
    End Sub
    Public Sub SaveRecurrence()
        'TODO - Only save if the items change from the previously saved recurrence schedule.
        'TODO - Display a popup notification
        Try
            Dim record As New System.Collections.Generic.List(Of RecurringParameters)
            Dim currentRecord As New System.Collections.Generic.List(Of RecurringParameters)
            Dim userName As String = IP.Bids.SharedFunctions.GetCurrentUser.Username
            Dim taskItemBLL As New TaskTrackerItemBll
            Dim recurrenceParameterList As System.Collections.Generic.List(Of RecurringParametersList)
            recurrenceParameterList = taskItemBLL.GetRecurringParameterList
            Dim isValid As Boolean = True

            currentpattern.EndByDate = New Date(1901, 1, 1) 'Default date


            currentRecord = taskItemBLL.GetRecurringParameters(taskSeqId)

            For Each paramrecord As RecurringParametersList In recurrenceParameterList
                Select Case paramrecord.ProfileTypeName
                    Case "RecurrencePattern"
                        currentPatternSeqID.RecurrencePattern = paramrecord.ProfileTypeSeqId
                    Case "Occurrences"
                        currentPatternSeqID.Occurrences = paramrecord.ProfileTypeSeqId
                    Case "RepeatUnitsQty"
                        currentPatternSeqID.RepeatUnitsQty = paramrecord.ProfileTypeSeqId
                    Case "RangeOfRecurrence"
                        currentPatternSeqID.RangeOfRecurrence = paramrecord.ProfileTypeSeqId
                    Case "SpecificDay"
                        currentPatternSeqID.SpecificDay = paramrecord.ProfileTypeSeqId
                    Case "SpecificMonth"
                        currentPatternSeqID.SpecificMonth = paramrecord.ProfileTypeSeqId
                    Case "DaysOfWeek"
                        currentPatternSeqID.DaysOfWeek = paramrecord.ProfileTypeSeqId
                    Case "Ordinals"
                        currentPatternSeqID.Ordinals = paramrecord.ProfileTypeSeqId
                    Case "EndByDate"
                        currentPatternSeqID.EndByDate = paramrecord.ProfileTypeSeqId
                End Select
            Next
            recurrenceParameterList = Nothing
            record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.RecurrencePattern, "RecurrencePattern", _rblFrequency.SelectedValue, userName, Now))
            Select Case _rblFrequency.SelectedValue.ToLower
                Case "one time only"
                    isValid = True
                    Me._rbEndBy.Checked = True
                    Me._dtEndbyDate.StartDate = Now.AddDays(-1).ToShortDateString
                Case "daily"
                    If IsNumeric(_txtEveryDay.Text) AndAlso CDbl(_txtEveryDay.Text) > 0 Then
                        record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.RepeatUnitsQty, "RepeatUnitsQty", _txtEveryDay.Text, userName, Now))
                    Else
                        isValid = False
                    End If
                Case "weekly"
                    If IsNumeric(_txtEveryXWeeks.Text) AndAlso CDbl(_txtEveryXWeeks.Text) > 0 Then
                        record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.RepeatUnitsQty, "RepeatUnitsQty", _txtEveryXWeeks.Text, userName, Now))
                    Else
                        isValid = False
                    End If
                    Dim daysofweek As String = IP.Bids.SharedFunctions.GetCheckBoxValues(_cblDaysOfWeek)
                    If daysofweek.Length > 0 Then
                        record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.DaysOfWeek, "DaysOfWeek", Replace(daysofweek, ",", "@"), userName, Now))
                    Else
                        isValid = False
                    End If
                Case "monthly"
                    If IsNumeric(_txtDayXOfMonth.Text) AndAlso CDbl(_txtDayXOfMonth.Text) >= 31 Then
                        'Switch
                        Me._rblLastDayOfMonth.Checked = True
                        Me._rbDayXOfMonth.Checked = False
                        Me._txtLastDayEveryXMonths.Text = Me._txtEveryXMonths.Text
                    End If
                    If Me._rbDayXOfMonth.Checked = True Then
                        If IsNumeric(Me._txtDayXOfMonth.Text) Then
                            record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.SpecificDay, "SpecificDay", Me._txtDayXOfMonth.Text, userName, Now))
                        Else
                            isValid = False
                        End If
                        If IsNumeric(Me._txtEveryXMonths.Text) Then
                            record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.RepeatUnitsQty, "RepeatUnitsQty", Me._txtEveryXMonths.Text, userName, Now))
                        Else
                            isValid = False
                        End If
                    ElseIf Me._rbWeekDayOfMonth.Checked = True Then
                        record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.Ordinals, "Ordinals", Me._ddlOrdinalDayOfMonth.SelectedValue, userName, Now))
                        record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.DaysOfWeek, "DaysOfWeek", Me._ddlMonthDayOfWeek.SelectedValue, userName, Now))
                        If IsNumeric(Me._txtDayOfWeekForEveryXMonths.Text) Then
                            record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.RepeatUnitsQty, "RepeatUnitsQty", Me._txtDayOfWeekForEveryXMonths.Text, userName, Now))
                        Else
                            isValid = False
                        End If
                    ElseIf Me._rblLastDayOfMonth.Checked = True Then
                        record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.Ordinals, "Ordinals", CStr(999), userName, Now))
                        If IsNumeric(Me._txtLastDayEveryXMonths.Text) Then
                            record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.RepeatUnitsQty, "RepeatUnitsQty", Me._txtLastDayEveryXMonths.Text, userName, Now))
                        Else
                            isValid = False
                        End If
                    End If
                Case "yearly"
                    If _rblDayXOfYear.Checked = True Then
                        If IsNumeric(Me._txtDayXOfYear.Text) Then
                            record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.SpecificDay, "SpecificDay", Me._txtDayXOfYear.Text, userName, Now))
                        Else
                            isValid = False
                        End If
                        record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.SpecificMonth, "SpecificMonth", Me._ddlDayXMonthsOfYear.SelectedValue, userName, Now))
                        record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.RepeatUnitsQty, "RepeatUnitsQty", Me._txtDayOfWeekForEveryXYears.Text, userName, Now))
                    ElseIf _rbWeekDayOfYear.Checked = True Then
                        record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.Ordinals, "Ordinals", Me._ddlOrdinalDayOfYear.SelectedValue, userName, Now))
                        record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.DaysOfWeek, "DaysOfWeek", Me._ddlYearlyDayOfWeek.SelectedValue, userName, Now))
                        record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.SpecificMonth, "SpecificMonth", Me._ddlMonthsOfYear.SelectedValue, userName, Now))
                        If IsNumeric(_txtWeekDayOfXYears.Text) Then
                            record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.RepeatUnitsQty, "RepeatUnitsQty", Me._txtWeekDayOfXYears.Text, userName, Now))
                        Else
                            isValid = False
                        End If
                    End If


                    'Case "yearly"
                    '    If .Ordinals > 0 Then
                    '        _rbWeekDayOfYear.Checked = True

                    '        If _ddlOrdinalDayOfYear.Items.FindByValue(CStr(.Ordinals)) IsNot Nothing Then
                    '            _ddlOrdinalDayOfYear.Items.FindByValue(CStr(.Ordinals)).Selected = True
                    '        End If
                    '        If _ddlYearlyDayOfWeek.Items.FindByValue(CStr(.SpecificDay)) IsNot Nothing Then
                    '            _ddlYearlyDayOfWeek.Items.FindByValue(CStr(.SpecificDay)).Selected = True
                    '        End If
                    '        If _ddlMonthsOfYear.Items.FindByValue(CStr(.SpecificMonth)) IsNot Nothing Then
                    '            _ddlMonthsOfYear.Items.FindByValue(CStr(.SpecificMonth)).Selected = True
                    '        End If
                    '        _txtWeekDayOfXYears.Text = CStr(.RepeatUnitsQty)
                    '    Else
                    '        _rblDayXOfYear.Checked = True
                    '        _txtDayXOfYear.Text = CStr(.SpecificDay)

                    '        If _ddlDayXMonthsOfYear.Items.FindByValue(CStr(.SpecificMonth)) IsNot Nothing Then
                    '            _ddlDayXMonthsOfYear.Items.FindByValue(CStr(.SpecificMonth)).Selected = True
                    '        End If
                    '        _txtDayOfWeekForEveryXYears.Text = CStr(.RepeatUnitsQty)
                    '    End If
                    'Case Else
                    '    Me._pnlRangeOfRecurrence.Visible = False
                Case Else
                    isValid = False
            End Select

            If Me._pnlRangeOfRecurrence.Visible Then
                If _rbEndBy.Checked Then
                    If IsDate(_dtEndbyDate.StartDate) Then
                        record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.EndByDate, "EndByDate", IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(_dtEndbyDate.StartDate)), userName, Now))
                    Else
                        isValid = False
                    End If
                ElseIf _rbEndAfter.Checked = True Then
                    If IsNumeric(_txtEndAfter.Text) AndAlso (CInt(_txtEndAfter.Text) > 0 And CInt(_txtEndAfter.Text) <= 500) Then
                        record.Add(New RecurringParameters(taskSeqId, currentPatternSeqID.Occurrences, "Occurrences", _txtEndAfter.Text, userName, Now))
                    Else
                        isValid = False
                        Me._rfEndAfter.Validate()
                    End If
                Else 'No end date

                End If
            End If
            Me.Validate(Me._btnSaveRecurrence.ValidationGroup)
            If isValid And Page.IsValid Then
                'SaveRecurringParameters

                taskItemBLL.SaveRecurringParameters(record, currentRecord)
                taskItemBLL = Nothing
                If Request.QueryString("CloseMe") IsNot Nothing Then
                    ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "CloseMe", "try{parent.document.getElementById('" & Request.QueryString("CloseMe") & "').click();}catch(e){alert(e);}", True)
                    'Me._btnSaveRecurrence.OnClientClick = "try{parent.document.getElementById('" & Request.QueryString("CloseMe") & "').click();}catch(e){alert(e);}" '"ctl00__cphMainContent__btnAddTask__btnClose').click();"
                    'Me._btnSaveRecurrence.OnClientClick = Me._btnSaveRecurrence.OnClientClick & "try{parent.document.getElementById('" & Request.QueryString("CloseMe") & "').click();}catch(e){}" '"ctl00__cphMainContent__btnAddTask__btnClose').click();"
                    'http://localhost:4593/EHSTaskTracker/Popups/Tasks.aspx?HeaderNumber=145&TaskNumber=&CloseMe=document.getElementById('ctl00__cphMainContent__btnAddTask__btnClose').click();
                End If
            Else
                Throw New DataException("Missing Data")
            End If
            If IsNumeric(taskSeqId) Then
                PopulateRecurringTasks(CInt(taskSeqId))
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("Save Recurrence", , ex, "Changes to the Recurrence pattern were not saved due to an unexpected error.")
        End Try
    End Sub


    Public Sub HandlePageLoad()
        _lblHeaderTitle.Text = IP.Bids.SharedFunctions.LocalizeValue("Recurrence Schedule", True)
        IP.Bids.SharedFunctions.DisablePageCache(Response)
        Me._pnlDailyFrequency.Visible = False
        Me._pnlWeeklyFrequency.Visible = False
        Me._pnlMonthlyFrequency.Visible = False
        Me._pnlYearly.Visible = False
        'Me._pnlQuarterly.Visible = False
        Me._pnlRangeOfRecurrence.Visible = True
        Me._btnSaveRecurrence.Enabled = False

        If Request.QueryString("RootTaskNumber") IsNot Nothing AndAlso IsNumeric(Request.QueryString("RootTaskNumber")) Then
            taskSeqId = CInt(Request.QueryString("RootTaskNumber"))
            Me._btnSaveRecurrence.Enabled = True
        End If



        If Not Page.IsPostBack Then
            PopulateFrequency()
            PopulateDayOfWeek()
            PopulateMonthsOfYear()
            Me._rblFrequency.ClearSelection()
            Me._rblFrequency.Items(0).Selected = True
            Me._pnlRangeOfRecurrence.Visible = True

            If IsNumeric(taskSeqId) Then
                GetRecurrenceData(CInt(taskSeqId))
            End If

            Dim taskItem As New TaskTrackerItemBll()
            Dim item = taskItem.GetTaskItem(CInt(taskSeqId))

            Dim taskHeader = New TaskHeaderBll(CInt(IP.Bids.SharedFunctions.DataClean(item.TaskHeaderSeqId, CStr(0))))
            If taskHeader.CurrentTaskHeaderRecord.ExternalSourceName.ToLower = "tanks" Then
                If Me._rblFrequency.Items.FindByValue("Yearly") IsNot Nothing Then
                    Me._rblFrequency.ClearSelection()
                    Me._rblFrequency.Items.FindByValue("Yearly").Selected = True
                    Me._pnlDailyFrequency.Visible = False
                    Me._pnlWeeklyFrequency.Visible = False
                    Me._pnlMonthlyFrequency.Visible = False
                    Me._pnlYearly.Visible = True
                End If
                'End If
            End If
            taskItem = Nothing
            item = Nothing
            taskHeader = Nothing
        End If
        If IsNumeric(taskSeqId) Then
            PopulateRecurringTasks(CInt(taskSeqId))
        End If


        HandleSelectedFrequency()
        'Dim dt As Date = Now
        'OrdinalDayOfWeekOfMonth(dt, DayOfWeek.Monday, Ordinal.First, RepeatingMonthQuantity.TwoMonths, 12)
        'OrdinalDayOfWeekOfMonth(dt, DayOfWeek.Monday, Ordinal.First, RepeatingMonthQuantity.ThreeMonths, 12)
        'OrdinalDayOfWeekOfMonth(dt, DayOfWeek.Monday, Ordinal.First, RepeatingMonthQuantity.TwoMonths, New Date(2011, 1, 1))
        'OrdinalDayOfWeekOfMonth(dt, DayOfWeek.Monday, Ordinal.Fifth, RepeatingMonthQuantity.OneMonth, New Date(2014, 1, 1))
        'DayOfRepeatingMonth(New Date(2010, 11, 2), 5, RepeatingMonthQuantity.OneMonth, 12)
        'DayOfRepeatingMonth(New Date(2010, 11, 2), 5, RepeatingMonthQuantity.OneMonth, New Date(2012, 9, 4))
        'DayOfRepeatingMonth(New Date(2010, 11, 2), 5, RepeatingMonthQuantity.OneMonth)
        'DayOfRepeatingMonth(New Date(2010, 11, 2), 5, RepeatingMonthQuantity.OneMonth, 20)


        'TODO  - Convert specific days > 30 to Ordinal.LastDay




    End Sub
    Public Sub PopulateMonthsOfYear()
        Dim dtMonthOfYear As New Data.DataTable
        With dtMonthOfYear
            .Columns.Add("Name", System.Type.GetType("System.String"))
            .Columns.Add("Value", System.Type.GetType("System.String"))
            Dim dr As Data.DataRow
            For i As Integer = 1 To 12
                dr = .NewRow
                dr.Item("Name") = MonthName(i)
                dr.Item("Value") = i.ToString
                .Rows.Add(dr)
            Next
            'With _ddlMonthDayOfWeek
            '    .DataSource = dtMonthOfYear
            '    .DataTextField = "Name"
            '    .DataValueField = "Value"
            '    .DataBind()
            'End With
            With _ddlMonthsOfYear
                .DataSource = dtMonthOfYear
                .DataTextField = "Name"
                .DataValueField = "Value"
                .DataBind()
            End With

            With _ddlDayXMonthsOfYear
                .DataSource = dtMonthOfYear
                .DataTextField = "Name"
                .DataValueField = "Value"
                .DataBind()
            End With
        End With
    End Sub

    Public Sub PopulateDayOfWeek()
        Dim dtDayOfWeek As New Data.DataTable
        With dtDayOfWeek

            .Columns.Add("Name", System.Type.GetType("System.String"))
            .Columns.Add("Value", System.Type.GetType("System.String"))
            Dim dr As Data.DataRow
            dr = .NewRow
            dr.Item("Name") = IP.Bids.SharedFunctions.LocalizeValue("Sunday", True)
            dr.Item("Value") = 1
            .Rows.Add(dr)
            dr = .NewRow
            dr.Item("Name") = IP.Bids.SharedFunctions.LocalizeValue("Monday", True)
            dr.Item("Value") = 2
            .Rows.Add(dr)
            dr = .NewRow
            dr.Item("Name") = IP.Bids.SharedFunctions.LocalizeValue("Tuesday", True)
            dr.Item("Value") = 3
            .Rows.Add(dr)
            dr = .NewRow
            dr.Item("Name") = IP.Bids.SharedFunctions.LocalizeValue("Wednesday", True)
            dr.Item("Value") = 4
            .Rows.Add(dr)
            dr = .NewRow
            dr.Item("Name") = IP.Bids.SharedFunctions.LocalizeValue("Thursday", True)
            dr.Item("Value") = 5
            .Rows.Add(dr)
            dr = .NewRow
            dr.Item("Name") = IP.Bids.SharedFunctions.LocalizeValue("Friday", True)
            dr.Item("Value") = 6
            .Rows.Add(dr)
            dr = .NewRow
            dr.Item("Name") = IP.Bids.SharedFunctions.LocalizeValue("Saturday", True)
            dr.Item("Value") = 7
            .Rows.Add(dr)

            'Weekly list should not contain Weekday 
            With _cblDaysOfWeek
                .DataSource = dtDayOfWeek
                .DataTextField = "Name"
                .DataValueField = "Value"
                .DataBind()
            End With


            dr = .NewRow
            dr.Item("Name") = "Weekday"
            dr.Item("Value") = 8
            .Rows.Add(dr)

            With _ddlMonthDayOfWeek
                .DataSource = dtDayOfWeek
                .DataTextField = "Name"
                .DataValueField = "Value"
                .DataBind()
            End With
            'With _ddlQuarterDayOfWeek
            '    .DataSource = dtDayOfWeek
            '    .DataTextField = "Name"
            '    .DataValueField = "Value"
            '    .DataBind()
            'End With
            With _ddlYearlyDayOfWeek
                .DataSource = dtDayOfWeek
                .DataTextField = "Name"
                .DataValueField = "Value"
                .DataBind()
            End With
        End With


    End Sub
    Public Sub PopulateFrequency()
        Dim dtTypeManager As New Data.DataTable
        Try
            'TODO: Connect to Data Access Layer instead of using Test Data
            'TODO: Add an option for Ending the recurrence schedule - This would change it back to being a one time task
            'Test Data
            With dtTypeManager
                .Columns.Add("Name", System.Type.GetType("System.String"))
                .Columns.Add("Value", System.Type.GetType("System.String"))
                Dim dr As Data.DataRow
                'dr = .NewRow
                'dr.Item("Name") = "One Time Only"
                'dr.Item("Value") = "One Time Only"
                '.Rows.Add(dr)
                dr = .NewRow
                dr.Item("Name") = IP.Bids.SharedFunctions.LocalizeValue("Daily", True)
                dr.Item("Value") = "Daily"
                .Rows.Add(dr)
                dr = .NewRow
                dr.Item("Name") = IP.Bids.SharedFunctions.LocalizeValue("Weekly", True)
                dr.Item("Value") = "Weekly"
                .Rows.Add(dr)
                dr = .NewRow
                dr.Item("Name") = IP.Bids.SharedFunctions.LocalizeValue("MonthlyQuarterly", True) '"Monthly/<br>&nbsp;&nbsp;&nbsp;&nbsp;Quarterly/<br>&nbsp;&nbsp;&nbsp;&nbsp;Semi-Annual"
                dr.Item("Value") = "Monthly"
                .Rows.Add(dr)
                dr = .NewRow
                'dr.Item("Name") = "Quarterly"
                'dr.Item("Value") = "Quarterly"
                '.Rows.Add(dr)
                'dr = .NewRow
                'dr.Item("Name") = "Semi-Annual"
                'dr.Item("Value") = "Semi-Annual"
                '.Rows.Add(dr)
                dr = .NewRow
                dr.Item("Name") = IP.Bids.SharedFunctions.LocalizeValue("Yearly", True)
                dr.Item("Value") = "Yearly"
                .Rows.Add(dr)
                'dr = .NewRow
                'dr.Item("Name") = "Twice Monthly"
                'dr.Item("Value") = "Twice Monthly"
                '.Rows.Add(dr)

            End With

            With Me._rblFrequency
                .DataSource = dtTypeManager
                .DataTextField = "Name"
                .DataValueField = "Value"
                .DataBind()

            End With
        Catch e As System.Data.DuplicateNameException ': The collection already has a column with the specified name. (The comparison is not case-sensitive.) 
            Throw New Data.DataException("GetActivity: Error Getting the Frequency Data.", e.InnerException)
        Catch e As System.Data.InvalidExpressionException ': The expression is invalid. See the System.Data.DataColumn.Expression property for more information about how to create expressions.  
            Throw New Data.DataException("GetActivity: Error Getting the Frequency Data.", e.InnerException)
        Catch
            Throw
        Finally
        End Try

    End Sub

    Public Sub HandleSelectedFrequency()
        Dim validationGroup As String = String.Empty
        Me._pnlRangeOfRecurrence.Visible = True
        Select Case _rblFrequency.SelectedValue.ToLower
            Case "daily"
                Me._txtEveryDay.Focus()
                validationGroup = "ValidationDays"
                Me._pnlDailyFrequency.Visible = True
                Me._btnSaveRecurrence.ValidationGroup = validationGroup
                Me._btnSaveRecurrence.OnClientClick = "return CheckForm('" & validationGroup & "');"
            Case "weekly"
                Me._txtEveryXWeeks.Focus()
                validationGroup = "ValidationWeeks"
                Me._pnlWeeklyFrequency.Visible = True
                'Me._rfvDaysOfWeek.ControlToValidate = Me._cblDaysOfWeek.ID
                'Me._rfvDaysOfWeek.ControlToValidate = Me._cblDaysOfWeek.ClientID

                Me._btnSaveRecurrence.ValidationGroup = validationGroup
                Me._btnSaveRecurrence.OnClientClick = "return CheckForm('" & validationGroup & "');"
            Case "monthly"

                If _rbDayXOfMonth.Checked Then
                    _rbWeekDayOfMonth.Checked = False
                    _rblLastDayOfMonth.Checked = False
                    validationGroup = "ValidationDayXOfMonth"
                    _txtDayXOfMonth.ValidationGroup = validationGroup
                    _txtEveryXMonths.ValidationGroup = validationGroup
                    _rfvDayXOfMonth.ValidationGroup = validationGroup
                    _rfvEveryXMonths.ValidationGroup = validationGroup
                    _rvEveryXMonths.Enabled = True
                    _rvEveryXMonths.ValidationGroup = validationGroup
                    _rfvDayXOfMonth.Enabled = True
                    _rfvEveryXMonths.Enabled = True
                    _rfvDayOfWeekForEveryXMonths.ValidationGroup = ""
                    _rfvDayOfWeekForEveryXMonths.Enabled = False
                    _rfvLastDayEveryXMonths.Enabled = False
                    _txtLastDayEveryXMonths.ValidationGroup = ""
                ElseIf _rbWeekDayOfMonth.Checked Then
                    _rbDayXOfMonth.Checked = False
                    _rblLastDayOfMonth.Checked = False
                    validationGroup = "ValidationEveryXMonths"
                    _txtDayOfWeekForEveryXMonths.ValidationGroup = validationGroup
                    _rfvDayOfWeekForEveryXMonths.ValidationGroup = validationGroup
                    _rvDayOfWeekForEveryXMonths.ValidationGroup = validationGroup
                    _rvDayOfWeekForEveryXMonths.Enabled = True
                    _rfvDayOfWeekForEveryXMonths.Enabled = True
                    _rfvLastDayEveryXMonths.Enabled = False
                    _txtLastDayEveryXMonths.ValidationGroup = ""
                Else
                    _rbDayXOfMonth.Checked = False
                    _rbWeekDayOfMonth.Checked = False
                    validationGroup = "ValidationLastDayOfMonth"
                    Me._rblLastDayOfMonth.Checked = True
                    _txtLastDayEveryXMonths.ValidationGroup = validationGroup
                    _rvLastDayEveryXMonths.ValidationGroup = validationGroup
                    _rvLastDayEveryXMonths.Enabled = True
                    _rfvDayOfWeekForEveryXMonths.ValidationGroup = ""
                    _rfvDayOfWeekForEveryXMonths.Enabled = False
                    _rfvLastDayEveryXMonths.Enabled = True
                    _rfvLastDayEveryXMonths.ValidationGroup = validationGroup

                End If

                Me._pnlMonthlyFrequency.Visible = True
                Me._txtDayXOfMonth.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rbDayXOfMonth.ClientID & "');")
                _txtEveryXMonths.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rbDayXOfMonth.ClientID & "');")

                '_rbWeekDayOfMonth
                _ddlOrdinalDayOfMonth.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rbWeekDayOfMonth.ClientID & "');")
                _ddlMonthDayOfWeek.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rbWeekDayOfMonth.ClientID & "');")
                _txtDayOfWeekForEveryXMonths.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rbWeekDayOfMonth.ClientID & "');")

                '_rblLastDayOfMonth
                _txtLastDayEveryXMonths.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rblLastDayOfMonth.ClientID & "');")
                Me._btnSaveRecurrence.ValidationGroup = validationGroup
                Me._btnSaveRecurrence.OnClientClick = "return CheckForm('" & validationGroup & "');"
            Case "yearly"
                If Me._rblDayXOfYear.Checked Then
                    Me._rbWeekDayOfYear.Checked = False
                    Me._txtDayXOfYear.Focus()
                    validationGroup = "ValidationEveryXYears"
                    _rfvDayOfWeekXYears.ValidationGroup = validationGroup
                    _rfvDayOfWeekXYears.Enabled = True
                    _rfvDayOfWeekForEveryXYears.ValidationGroup = validationGroup
                    _rfvDayOfWeekForEveryXYears.Enabled = True
                    _rvDayXOfYear.Enabled = True
                    _rvDayXOfYear.ValidationGroup = validationGroup
                    _rvDayOfWeekForEveryXYears.Enabled = True
                    _rvDayOfWeekForEveryXYears.ValidationGroup = validationGroup
                Else
                    Me._rblDayXOfYear.Checked = False
                    Me._rbWeekDayOfYear.Checked = True
                    _txtWeekDayOfXYears.Focus()
                    validationGroup = "ValidationDayOfWeekForEveryXYears"
                    _rfvWeekDayOfXYears.ValidationGroup = validationGroup
                    _rfvWeekDayOfXYears.Enabled = True
                    _rvWeekDayOfXYears.Enabled = True
                    _rvWeekDayOfXYears.ValidationGroup = validationGroup
                End If
                Me._pnlYearly.Visible = True
                '.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rblLastDayOfMonth.ClientID & "');")
                '_rblDayXOfYear
                _txtDayXOfYear.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rblDayXOfYear.ClientID & "');")
                _ddlDayXMonthsOfYear.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rblDayXOfYear.ClientID & "');")
                _txtDayOfWeekForEveryXYears.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rblDayXOfYear.ClientID & "');")

                '_rbWeekDayOfYear
                _ddlOrdinalDayOfYear.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rbWeekDayOfYear.ClientID & "');")
                _ddlYearlyDayOfWeek.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rbWeekDayOfYear.ClientID & "');")
                _ddlMonthsOfYear.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rbWeekDayOfYear.ClientID & "');")
                _txtWeekDayOfXYears.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rbWeekDayOfYear.ClientID & "');")

            Case Else
                Me._pnlRangeOfRecurrence.Visible = False
        End Select
        If Me._rbEndAfter.Checked = True Then
            '_txtEndAfter.ValidationGroup = validationGroup
            _rvEndAfter.ValidationGroup = validationGroup
            _rvEndAfter.Enabled = True
            _rfEndAfter.ValidationGroup = validationGroup
            _rfEndAfter.Enabled = True
            _dtEndbyDate.EnableValidation = False
            _dtEndbyDate.Validation.ValidationGroup = ""
            _dtEndbyDate.StartDate = String.Empty
        ElseIf Me._rbEndBy.Checked = True Then
            _dtEndbyDate.EnableValidation = True
            _dtEndbyDate.Validation.ValidationGroup = validationGroup
            '_rvEndAfter.ValidationGroup = ""
            '_rvEndAfter.Enabled = False
            _rfEndAfter.ValidationGroup = ""
            _rfEndAfter.Enabled = False
            _txtEndAfter.ValidationGroup = ""
        Else
            _dtEndbyDate.StartDate = String.Empty
            _dtEndbyDate.EnableValidation = False
            _dtEndbyDate.Validation.ValidationGroup = ""
            _txtEndAfter.ValidationGroup = ""
            '_rvEndAfter.ValidationGroup = ""
            '_rvEndAfter.Enabled = False
            _rfEndAfter.ValidationGroup = ""
            _rfEndAfter.Enabled = False
        End If
        '_rbEndAfter
        _txtEndAfter.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rbEndAfter.ClientID & "');")
        '_rbEndBy        
        _dtEndbyDate.Attributes.Add("onFocus", "SelectRadioButton ('" & Me._rbEndBy.ClientID & "');")
        _valSummary.ValidationGroup = validationGroup
        Me._btnSaveRecurrence.ValidationGroup = validationGroup
        Me._btnSaveRecurrence.OnClientClick = "return CheckForm('" & validationGroup & "');"

    End Sub

    Public Sub CheckboxValidation(ByVal source As Object, ByVal args As ServerValidateEventArgs)
        Try
            ' Test whether the value entered into the text box is even.
            Dim i As Integer = Integer.Parse(args.Value)
            args.IsValid = ((i Mod 2) = 0)
        Catch ex As Exception
            args.IsValid = False
        End Try
    End Sub

    Protected Sub _rblFrequency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _rblFrequency.SelectedIndexChanged
        HandleSelectedFrequency()
    End Sub

    Protected Sub _btnSaveRecurrence_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSaveRecurrence.Click
        SaveRecurrence()
    End Sub

    Protected Sub _btnCancelRecurrence_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnCancelRecurrence.Click
        Me._rbEndBy.Checked = True
        Me._dtEndbyDate.StartDate = Now.AddDays(-1).ToShortDateString
        'Me._dtEndbyDate.StartDate = IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Now.AddDays(-1))
        'IP.Bids.SharedFunctions.FormatDateTimeToEnglish(Now.AddDays(-1))
        'Me._rblFrequency.ClearSelection()
        'Me._rblFrequency.Items(0).Selected = True
        SaveRecurrence()
    End Sub

    Protected Sub _rbEndBy_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _rbEndBy.CheckedChanged
        If _dtEndbyDate.StartDate.Length = 0 Then
            _dtEndbyDate.StartDate = Now.ToShortDateString
        End If
        _txtEndAfter.Text = String.Empty
    End Sub

    Protected Sub _rbEndAfter_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _rbEndAfter.CheckedChanged
        _dtEndbyDate.StartDate = String.Empty
        _txtEndAfter.Text = "1"
    End Sub

    Protected Sub _rbNoEndDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _rbNoEndDate.CheckedChanged
        _dtEndbyDate.StartDate = String.Empty
        _txtEndAfter.Text = String.Empty
    End Sub


    'Protected Sub _btnMonthly_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnMonthly.Click, _btnQuarterly.Click, _btnSemiAnnual.Click
    '    Dim months As String = "1"
    '    Dim btn As Button = TryCast(sender, Button)

    '    Select Case btn.CommandArgument.ToLower 'e.SelectedValue.ToLower
    '        Case "monthly"
    '            months = "1"
    '        Case "quarterly"
    '            months = "3"
    '        Case "semi-annual"
    '            months = "6"
    '    End Select
    '    _txtLastDayEveryXMonths.Text = String.Empty
    '    _txtEveryXMonths.Text = String.Empty
    '    _txtDayOfWeekForEveryXMonths.Text = String.Empty

    '    If Me._rblLastDayOfMonth.Checked Then
    '        _txtLastDayEveryXMonths.Text = months
    '    ElseIf Me._rbDayXOfMonth.Checked Then
    '        _txtEveryXMonths.Text = months
    '    Else
    '        '_rbWeekDayOfMonth
    '        _txtDayOfWeekForEveryXMonths.Text = months
    '    End If
    'End Sub

    Private Sub PopulateRecurringTasks(ByVal taskItemNumber As Integer)
        Dim taskItemBLL As New TaskTrackerItemBll
        Dim currentTaskItem As TaskItem
'        Dim taskSearch As New TaskTrackerListsBLL ' COMMENTED BY CODEIT.RIGHT
        Try
            currentTaskItem = taskItemBLL.GetTaskItem(CInt(taskItemNumber))
            If currentTaskItem IsNot Nothing Then
                With currentTaskItem

                    If .RecurringTasks IsNot Nothing AndAlso .RecurringTasks.Count > 0 Then

                        Dim dl As DataList = _dlRecurringTasks
                        Dim dt As New Data.DataTable
                        Dim dr As Data.DataRow
                        If dl IsNot Nothing Then
                            dt.Columns.Add("taskdate")
                            dt.Columns.Add("url")
                            dt.Columns.Add("TaskItemSeqId")
                            dt.Columns.Add("taskdatesort")


                            'With Me._rblrecurringTasks
                            '    .Items.Clear()
                            '    .ClearSelection()
                            Dim imgPath As String = Page.ResolveClientUrl("~/images") & "/"
                            For Each task As RecurringTasks In currentTaskItem.RecurringTasks
                                '.Items.Add(New ListItem(task.DueDate, CStr(task.TaskItemSeqId)))
                                dr = dt.NewRow
                                dr.Item("TaskDate") = Global.TaskSearchBll.GetTaskStatus(task.StatusSeqId, False, task.DueDate, imgPath) & IP.Bids.SharedFunctions.FormatDate(CDate(task.DueDate)) 'taskSearch.GetTaskStatus (task.StatusSeqId ,False , task.DueDate 'GetTaskStatus(task.DueDate, task.StatusSeqId, "")
                                dr.Item("URL") = "javascript:JQConfirmRedirect('" & Page.ResolveClientUrl(String.Format(CultureInfo.CurrentCulture, "~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}", .TaskHeaderSeqId, task.TaskItemSeqId)) & "');"
                                dr.Item("TaskItemSeqId") = task.TaskItemSeqId
                                dr.Item("taskdatesort") = Format(task.DueDate, "yyyy-mm-dd")

                                '"JQConfirmRedirect('" & Page.ResolveClientUrl("~/TaskHeader.aspx?HeaderNumber=" & TaskHeaderNumber) & "'); return false;"
                                dt.Rows.Add(dr)
                            Next
                            'End With
                            dt.DefaultView.Sort = "taskdatesort" '"TaskItemSeqId"
                            dl.DataSource = dt.DefaultView
                            dl.DataBind()
                        End If
                        _lblNoRecurringTasks.Visible = False
                    Else
                        _lblNoRecurringTasks.Visible = True
                    End If
                End With
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateRecurringTasks", , ex)
        Finally
            taskItemBLL = Nothing
            currentTaskItem = Nothing
        End Try
    End Sub

    Private Sub PopupsRecurrence_Init(sender As Object, e As EventArgs) Handles Me.Init
        'Me.UseBootStrap = False
    End Sub

    Private Sub _rblFrequency_PreRender(sender As Object, e As EventArgs) Handles _rblFrequency.PreRender
        _rblFrequency.RepeatLayout = RepeatLayout.Flow
        _rblFrequency.RepeatDirection = RepeatDirection.Horizontal
        _rblFrequency.RepeatColumns = 12
        For Each item As ListItem In _rblFrequency.Items
            item.Attributes.Add("class", "col-xs-12 col-sm-6 col-md-2")
        Next
    End Sub
End Class
