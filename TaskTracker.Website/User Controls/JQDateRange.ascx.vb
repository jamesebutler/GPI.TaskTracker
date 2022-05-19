
Option Explicit On
Option Strict On
Imports System.Globalization

Partial Class UserControlsJQDateRange
    Inherits IP.Bids.UserControl.UserControlValidation

#Region "Enum"
    ''' <summary>
    ''' These are the available date range values
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum JQDateRange
        ClearDate = -2
        CustomRange = -1
        Current = 0
        CurrentMonth = 1
        CurrentQuarter = 2
        CurrentYear = 3
        CurrentDateToQuarter = 4
        CurrentDateToEndOfYear = 5
        LastMonth = 6
        LastQuarter = 7
        LastYear = 8
        MonthToDate = 9
        NextMonth = 10
        NextQuarter = 11
        NextYear = 12
        QuarterToDate = 13
        YearToDate = 14

        Overdue = 15
        OverdueNext7Days = 16
        OverdueNext14Days = 17
        OverdueNext30Days = 18
        Next7Days = 19
        Next14Days = 20
        Next30Days = 21
    End Enum

    ''' <summary>
    ''' Contains the available quarters that will be used along with the date range
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Qtr
        CurrentQuarter = 0
        FirstQuarter = 1
        SecondQuarter = 2
        ThirdQuarter = 3
        FourthQuarter = 4
        PreviousQuarter = 5
    End Enum
#End Region

#Region "Custom Events"
    Public Event DateChanged(ByVal sender As TextBox)
    Public Event TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
#End Region

#Region "Fields"
    Private mShowFromDate As Boolean
    Private mShowLabelsOnSameLine As Boolean
    Private mAllowDateCritical As Boolean
    Private DateRangeCollection As New Hashtable
    Private mShowDateRange As Boolean
    Private mEnabled As Boolean = True
    Private mAllowPostback As Boolean
    Private mMaxDate As Date = New Date(2030, 1, 1)
    Private mMinDate As Date = New Date(1901, 1, 1)
#End Region

#Region "Public Properties"
    Public Property CloseText As String = "Close"
    Public Property PrevText As String = "Previous"
    Public Property NextText As String = "Next"
    Public Property CurrentText As String = "Today"
    Public Property ShowReportDateRange As Boolean = False
    Public Property MaxDate() As Date
        Get
            Return mMaxDate
        End Get
        Set(ByVal value As Date)
            mMaxDate = value
        End Set
    End Property
    Public Property MinDate() As Date
        Get
            Return mMinDate
        End Get
        Set(ByVal value As Date)
            mMinDate = value
        End Set
    End Property
    Public Property AllowPostBack() As Boolean
        Get
            Return mAllowPostback
        End Get
        Set(ByVal value As Boolean)
            mAllowPostback = value
        End Set
    End Property
    Public Property AllowDateCritical() As Boolean
        Get
            Return mAllowDateCritical
        End Get
        Set(ByVal value As Boolean)
            mAllowDateCritical = value
        End Set
    End Property

    Public Property DateIsCritical() As Boolean
        Get
            Return Me._cbdateCritical.Checked
        End Get
        Set(ByVal value As Boolean)
            Me._cbdateCritical.Checked = value
        End Set
    End Property
    Public Property ShowLabelsOnSameLine() As Boolean
        Get
            Return mShowLabelsOnSameLine
        End Get
        Set(ByVal value As Boolean)
            mShowLabelsOnSameLine = value
        End Set
    End Property
    Public Property ShowDateRange() As Boolean
        Get
            Return mShowDateRange
        End Get
        Set(ByVal value As Boolean)
            mShowDateRange = value
        End Set
    End Property
    Public Property FromLabel() As String
        Get
            Return "" 'Me._lblDateFrom.Text
        End Get
        Set(ByVal value As String)
            'Me._lblDateFrom.Text = value
        End Set
    End Property
    Public Property ToLabel() As String
        Get
            Return Me._lblDateTo.Text
        End Get
        Set(ByVal value As String)
            Me._lblDateTo.Text = value
        End Set
    End Property
    Public Property ShowFromDate() As Boolean
        Get
            Return mShowFromDate
        End Get
        Set(ByVal value As Boolean)
            mShowFromDate = value
        End Set
    End Property
    Public Property StartDate() As String
        Get
            If _txtDateFrom.Text.Length > 0 Then
                Dim dateValue = _txtDateFrom.Text.Split(CChar(" "))
            End If
            If IsDate(Me._txtDateFrom.Text) Then
                Return CDate(Me._txtDateFrom.Text).ToShortDateString
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            If Me._txtDateFrom.Text <> CStr(Value) AndAlso Value.Trim.Length > 0 Then
                _txtDateFrom.Text = (IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(Value, DirectCast(System.Threading.Thread.CurrentThread.CurrentCulture, System.Globalization.CultureInfo).Name.ToLower, IP.Bids.SharedFunctions.GetLocalizedDateFormat))
                RaiseEvent TextChanged(Me._txtDateFrom, Nothing)
            Else
                Me._txtDateFrom.Text = CStr(Value)
            End If

        End Set
    End Property

    Public Property EndDate() As String
        Get
            If Me.ShowFromDate = True Then
                If IsDate(Me._txtDateTo.Text) Then
                    Return CDate(Me._txtDateTo.Text).ToShortDateString
                Else
                    Return ""
                End If
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As String)
            If Me._txtDateTo.Text <> CStr(Value) AndAlso Value.Length > 0 Then
                _txtDateTo.Text = (IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(Value, DirectCast(System.Threading.Thread.CurrentThread.CurrentCulture, System.Globalization.CultureInfo).Name.ToLower, IP.Bids.SharedFunctions.GetLocalizedDateFormat))
                RaiseEvent TextChanged(Me._txtDateTo, Nothing)
            Else
                _txtDateTo.Text = CStr(Value)
            End If
        End Set
    End Property

    'Protected Overrides Function SaveControlState() As Object
    '    Dim controlState As Object() = New Object(2) {}
    '    controlState(0) = MyBase.SaveControlState()
    '    controlState(1) = StartDate
    '    controlState(2) = EndDate
    '    Return controlState
    'End Function

    'Protected Overrides Sub LoadControlState(ByVal savedState As Object)
    '    Dim controlState As Object() = DirectCast(savedState, Object())
    '    MyBase.LoadControlState(controlState(0))
    '    StartDate = DirectCast(controlState(1), String)
    '    EndDate = DirectCast(controlState(2), String)
    'End Sub
    Public ReadOnly Property SelectedDateRange() As String
        Get
            If Request IsNot Nothing AndAlso Request(_ddlDateRange.UniqueID) IsNot Nothing Then
                Return Request(_ddlDateRange.UniqueID)
            Else
                If _ddlDateRange.SelectedValue IsNot Nothing Then
                    Return _ddlDateRange.SelectedValue
                Else
                    Return ""
                End If
            End If
        End Get

    End Property
    Public Property DateRange() As Integer 'JQDateRange
        Get
            If _ddlDateRange.SelectedValue IsNot Nothing AndAlso IsNumeric(_ddlDateRange.SelectedValue) Then
                Return CType(System.Enum.Parse(GetType(JQDateRange), _ddlDateRange.SelectedValue), JQDateRange)
            Else
                Return JQDateRange.Current
            End If
        End Get
        Set(ByVal value As Integer)
            Dim startDT As String = String.Empty
            Dim endDT As String = String.Empty
            Me.SetDateRange(startDT, endDT, CType(value, JQDateRange))
            StartDate = startDT
            EndDate = endDT
            If _ddlDateRange.Items.FindByValue(CType(value, String)) IsNot Nothing Then
                _ddlDateRange.ClearSelection()
                Me._ddlDateRange.SelectedValue = CStr(value)
            End If
        End Set
    End Property

    Public Property Enabled() As Boolean
        Get
            Return mEnabled
        End Get
        Set(ByVal value As Boolean)
            mEnabled = value
        End Set
    End Property

    Public ReadOnly Property StartDateUniqueId() As String
        Get
            Return _txtDateFrom.UniqueID
        End Get
    End Property

    Public ReadOnly Property EndDateUniqueId() As String
        Get
            Return _txtDateTo.UniqueID
        End Get
    End Property
#End Region

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            If Me.EnableValidation = True Then
                Me.ControlToValidate = Me._txtDateFrom.ID
                Me.ClientValidationFunction = "ValidateStartEndDates"
            Else
                Me.ControlToValidate = _txtDateFrom.ID
                Me.ClientValidationFunction = ""
            End If
        Catch
            Throw
        End Try
    End Sub

    Protected Overrides Sub OnInit(ByVal e As EventArgs)

        Page.RegisterRequiresControlState(Me)
        MyBase.OnInit(e)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sm As ScriptManager = ScriptManager.GetCurrent(Page)
        Dim lang As String = System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower
        If ShowFromDate Then
            If sm.IsInAsyncPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "JQDateRange_" & Me.ClientID, AddJQueryDatePickerJS(), True)
                ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "JQDate_" & Me.ClientID & "_" & lang, GetLocalizeDateLabels(), True)
            Else
                If Not Page.ClientScript.IsStartupScriptRegistered(Page.GetType, "JQDateRange_" & Me.ClientID) Then
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "JQDateRange_" & Me.ClientID, AddJQueryDatePickerJS(), True)
                End If
                Page.ClientScript.RegisterStartupScript(Page.GetType, "JQDate_" & Me.ClientID & "_" & lang, GetLocalizeDateLabels(), True)
            End If



            Me._lblDateTo.Visible = True
            Me._txtDateTo.Visible = True
            Me.ShowLabelsOnSameLine = True
        Else
            Me.ShowDateRange = False
            If sm.IsInAsyncPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "JQSingleDate_" & Me.ClientID, AddJQuerySingleDatePickerJS(CStr(MaxDate)), True)
                ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "JQDate_" & Me.ClientID & "_" & lang, GetLocalizeDateLabels(), True)
            Else
                If Not Page.ClientScript.IsStartupScriptRegistered(Page.GetType, "JQSingleDate_" & Me.ClientID) Then
                    Page.ClientScript.RegisterStartupScript(Page.GetType, "JQSingleDate_" & Me.ClientID, AddJQuerySingleDatePickerJS(CStr(MaxDate)), True)
                End If
                Page.ClientScript.RegisterStartupScript(Page.GetType, "JQDate_" & Me.ClientID & "_" & lang, GetLocalizeDateLabels(), True)
            End If

            Me._lblDateTo.Visible = False
            Me._txtDateTo.Visible = False
        End If
        If Me._ddlDateRange.Items.Count = 0 Then
            PopulateDateRange()
        End If
        If Not Page.IsPostBack Then
            'If Me._txtDateFrom.Text.Length = 0 And Me._txtDateTo.Text.Length = 0 Then
            '    StartDate = Now.ToShortDateString
            '    EndDate = Now.ToShortDateString
            'End If
        Else
            'If Page.EnableViewState = False Then



            If Request.Form(_txtDateFrom.UniqueID) IsNot Nothing And Request.Form(_txtDateTo.UniqueID) IsNot Nothing Then
                StartDate = Request.Form(_txtDateFrom.UniqueID)
                EndDate = Request.Form(_txtDateTo.UniqueID)
            ElseIf Request.Form(_txtDateFrom.UniqueID) IsNot Nothing Then
                StartDate = Request.Form(_txtDateFrom.UniqueID)
            ElseIf Request.Form(_txtDateTo.UniqueID) IsNot Nothing Then
                EndDate = Request.Form(_txtDateTo.UniqueID)
            End If
            'End If
        End If
        If ShowDateRange = True Then
            Me._lblDateRange.Visible = True
            Me._ddlDateRange.Visible = True
        Else
            Me._lblDateRange.Visible = False
            Me._ddlDateRange.Visible = False
        End If

        If sm.IsInAsyncPostBack Then
            ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "JQDateRange", GetDateRangeJS(), True)
        Else
            If Not Page.ClientScript.IsClientScriptBlockRegistered(Page.GetType, "JQDateRange") Then
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType, "JQDateRange", GetDateRangeJS(), True)
            End If
        End If
        If ShowLabelsOnSameLine Then
            'Me._lineBreakFrom.Visible = False
            Me._lineBreakTo.Visible = False
        Else
            'Me._lineBreakFrom.Visible = True
            Me._lineBreakTo.Visible = True
        End If
        If AllowDateCritical Then
            Me._cbdateCritical.Visible = True
        Else
            Me._cbdateCritical.Visible = False
        End If

        Try



            Me._ddlDateRange.Attributes.Add("onchange", "ChangeDate('" & _txtDateFrom.ClientID & "','" & _txtDateTo.ClientID & "',this.value);")
            Me._txtDateFrom.ReadOnly = True
            Me._txtDateTo.ReadOnly = True
            If Me.ShowDateRange = True Then
                Dim selectedRange As JQDateRange = JQDateRange.CustomRange

                Dim cStartDate As String = StartDate
                Dim cEndDate As String = EndDate
                For Each item As DictionaryEntry In DateRangeCollection
                    Dim rangeValue As DateRangeValues
                    rangeValue = CType(item.Value, DateRangeValues)
                    If IsDate(cStartDate) AndAlso IsDate(cEndDate) Then
                        If rangeValue.StartDate = CDate(cStartDate) And rangeValue.EndDate = CDate(cEndDate) Then
                            selectedRange = rangeValue.Range
                        End If
                    End If
                Next
                _ddlDateRange.SelectedValue = CStr(selectedRange)
            End If

            'If Me.EnableValidation = True Then
            '    _lblRequiredField.Visible = True
            'Else
            '    _lblRequiredField.Visible = False
            'End If
        Catch
            Throw
        End Try
    End Sub

    Private Function AddJQueryDatePickerJS() As String
        Dim sbDatePicker As New StringBuilder
        Dim lang As String = ""

        If Page IsNot Nothing AndAlso Page.Culture IsNot Nothing AndAlso Page.Culture.Length > 0 Then
            Select Case DirectCast(System.Threading.Thread.CurrentThread.CurrentCulture, System.Globalization.CultureInfo).Name.ToLower
                Case "en-us"
                    lang = "" 'Default
                Case "ru-ru"
                    lang = "ru"
                Case "fr-fr"
                    lang = "fr"
                Case "pt-br"
                    lang = "pt-br"
                Case Else
                    lang = DirectCast(System.Threading.Thread.CurrentThread.CurrentCulture, System.Globalization.CultureInfo).Name.ToLower
            End Select
        End If
        With sbDatePicker
            .Append("$(function() {$( "".selector"" ).datepicker({});")
            .Append(" var dates = $('input[type=text][id*=" & Me._txtDateFrom.ClientID & "],input[type=text][id*=" & Me._txtDateTo.ClientID & " ]').datepicker({	")
            '.Append("		defaultDate: """",")
            .Append("		defaultDate: ""+1w"",")
            .Append("		changeMonth: true,")
            .Append("		numberOfMonths: 1,")
            .Append("		changeYear: true,")
            .Append("		showOtherMonths: true,")
            .Append("		showButtonPanel: false,")
            .Append("       selectOtherMonths: true,")
            .Append("		buttonImageOnly: false,")
            .Append("		minDate:new Date(2007, 1 - 1, 1),")
            .Append("		maxDate:new Date(2030, 1 - 1, 1),")
            '.Append("		dateFormat:""M d, yy "",") 'MJP added
            .Append("     altformat:""yy-mm-dd"",")
            .Append("		dateFormat:""" & IP.Bids.SharedFunctions.GetLocalizedJQueryDateFormat & """,")
            .Append("     regional: """ & lang & """,")
            .Append("		onSelect: function( selectedDate ) {")
            .Append("		    var dateFrom = dates[0].id;")
            .Append("			var option = this.id == dateFrom ? ""minDate"" : ""maxDate"",")
            .Append("				instance = $( this ).data( ""datepicker"" );")
            .Append("				date = $.datepicker.parseDate(")
            .Append("					instance.settings.dateFormat ||")
            .Append("					$.datepicker._defaults.dateFormat,")
            .Append("					selectedDate, instance.settings );")
            .Append("			dates.not( this ).datepicker( ""option"", option, date );")
            .Append("		}")
            .Append("	});")
            .Append("});")
        End With
        Return sbDatePicker.ToString
    End Function
    Private Function AddJQuerySingleDatePickerJS(Optional ByVal maxDate As String = "1/1/2030") As String
        Dim sbDatePicker As New StringBuilder
        Dim jsMaxDate As String = String.Empty
        Dim jsMinDate As String = String.Empty
        Dim lang As String = ""

        If Page IsNot Nothing AndAlso Page.Culture IsNot Nothing AndAlso Page.Culture.Length > 0 Then
            Select Case DirectCast(System.Threading.Thread.CurrentThread.CurrentCulture, System.Globalization.CultureInfo).Name.ToLower
                Case "en-us"
                    lang = "" 'Default
                Case "ru-ru"
                    lang = "ru"
                Case "fr-fr"
                    lang = "fr"
                Case "pt-br"
                    lang = "pt-br"
                Case Else
                    lang = DirectCast(System.Threading.Thread.CurrentThread.CurrentCulture, System.Globalization.CultureInfo).Name.ToLower
            End Select
        End If

        If IsDate(maxDate) Then
            Dim dt As Date = CDate(maxDate)
            jsMaxDate = "          maxDate:new Date(" & dt.Year & "," & dt.Month - 1 & "," & dt.Day & "),"
        End If
        If IsDate(MinDate) Then
            If MinDate.Year < Now.Year - 7 Then
                MinDate = New Date(Now.Year - 7, 1, 1)
            End If
            jsMinDate = "          minDate:new Date(" & MinDate.Year & "," & MinDate.Month - 1 & "," & MinDate.Day & "),"
        End If
        With sbDatePicker
            .Append("$(function() {")
            .Append("$( ""#" & Me._txtDateFrom.ClientID & """ ).datepicker( {")
            .Append("     changeYear: true,")
            .Append("		defaultDate: ""+1w"",")
            .Append("     numberOfMonths: 1,")
            .Append("     showButtonPanel: false,")
            .Append("     showOtherMonths: true,")
            .Append("     selectOtherMonths: true,")
            .Append("     regional: """ & lang & """,")
            '.Append("     dateFormat:""M d, yy "",")
            .Append("     altformat:""yy-mm-dd"",")
            .Append("	  dateFormat:""" & IP.Bids.SharedFunctions.GetLocalizedJQueryDateFormat & """,") 'MJP added
            .Append(jsMaxDate)
            .Append(jsMinDate)
            '.Append("	  maxDate:new Date(2030, 1 - 1, 1),")
            .Append("     changeMonth: true});")
            .Append("});")
        End With
        Return sbDatePicker.ToString
    End Function

    Private Function GetLocalizeDateLabels() As String
        Dim sb As New StringBuilder
        Dim lang As String = System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower
        Dim ipResources As New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization()

        sb.AppendLine("jQuery(function($){")
        sb.AppendFormat("$.datepicker.regional['{0}'] = ", Left(lang, 2).ToLower)
        sb.Append("{")
        sb.AppendFormat(" closeText:  '{0}',", ipResources.GetResourceValue(CloseText))
        sb.AppendFormat(" prevText:   '&#x3c;{0}',", ipResources.GetResourceValue(PrevText))
        sb.AppendFormat(" nextText:   '{0}&#x3e;',", ipResources.GetResourceValue(NextText))
        sb.AppendFormat(" currentText:  '{0}',", ipResources.GetResourceValue(CurrentText))
        sb.AppendFormat(" monthNames:[{0}],", ConvertCollectionToDelimitedList(GetLocalizedMonthNames))
        sb.AppendFormat(" monthNamesShort:[{0}],", ConvertCollectionToDelimitedList(GetLocalizedMonthNames)) 'ConvertCollectionToDelimitedList(GetLocalizedShortMonthNames))
        sb.AppendFormat(" dayNames:[{0}],", ConvertCollectionToDelimitedList(GetLocalizedDayNames))
        sb.AppendFormat(" dayNamesShort:[{0}],", ConvertCollectionToDelimitedList(GetLocalizedShortDayNames))
        sb.AppendFormat(" dayNamesMin:[{0}],", ConvertCollectionToDelimitedList(GetLocalizedTwoCharDayNames))
        sb.Append(" weekHeader: 'Wk',")
        sb.Append(" dateFormat: 'd M yy',")
        sb.AppendFormat(" firstDay: {0},", GetFirstDayOfWeek)
        sb.Append(" isRTL: false,")
        sb.Append(" showMonthAfterYear: false,")
        sb.Append(" yearSuffix:  ''};")
        sb.AppendFormat(" 	$.datepicker.setDefaults($.datepicker.regional['{0}']);", Left(lang, 2).ToLower)
        sb.Append(" });")
        Return sb.ToString
    End Function

    Private Function GetFirstDayOfWeek() As Integer
        Dim current = Threading.Thread.CurrentThread.CurrentCulture
        Return current.DateTimeFormat.FirstDayOfWeek
    End Function
    Private Function GetLocalizedMonthNames() As SortedList
        Dim listOfMonths As New SortedList
        For i As Integer = 1 To 12
            listOfMonths.Add(i, MonthName(i))
        Next
        Return listOfMonths
    End Function

    Private Function GetLocalizedShortMonthNames() As SortedList
        Dim listOfMonths As New SortedList
        For i As Integer = 1 To 12
            listOfMonths.Add(i, MonthName(i, True))
        Next
        Return listOfMonths
    End Function

    Private Function GetLocalizedDayNames() As SortedList
        Dim listOfDays As New SortedList

        For i As Integer = 1 To 7
            listOfDays.Add(i, WeekdayName(i))
        Next
        Return listOfDays
    End Function

    Private Function GetLocalizedShortDayNames() As SortedList
        Dim listOfDays As New SortedList
        Dim current = Threading.Thread.CurrentThread.CurrentCulture
        Dim index As Integer = 0
        For Each item In current.DateTimeFormat.ShortestDayNames
            index += 1
            listOfDays.Add(index, item)
        Next
        Return listOfDays
    End Function

    Private Function GetLocalizedTwoCharDayNames() As SortedList
        Return GetLocalizedShortDayNames()
    End Function

    Private Function ConvertCollectionToDelimitedList(ByVal input As SortedList) As String
        Dim sb As New StringBuilder

        For Each item As DictionaryEntry In input
            If sb.Length > 0 Then sb.Append(",")
            sb.AppendFormat("'{0}'", item.Value)
        Next
        Return sb.ToString
    End Function
    'Public Function AddEnableOrDisableJavascript(ByVal disabled As Boolean, ByVal mainCtrl As String) As String
    '    Dim sb As New StringBuilder
    '    With sb
    '        .Append("function EnableOrDisable(){")
    '        '_txtDateFrom
    '        .AppendFormat("var obj=document.getElementById(mainCtrl+{0});", Me._txtDateFrom.UniqueID)
    '        .Append("if (obj!=null){obj.disabled=disabled} ")

    '        '_txtDateTo
    '        .AppendFormat("obj=document.getElementById(mainCtrl+{0});", Me._txtDateTo.UniqueID)
    '        .Append("if (obj!=null){obj.disabled=disabled} ")

    '        .Append("}")
    '    End With

    '    Return sb.ToString
    'End Function

    Public Function ListOfClientIds() As Array
        Dim list() As String = {_txtDateFrom.ClientID, _txtDateTo.ClientID, _ddlDateRange.ClientID}
        Return list
    End Function
    ''' <summary>
    ''' Adds data to the Date Range dropdown.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateDateRange()
        Try
            Dim ipResources As New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization()
            'Populate Date Range Dropdown
            With _ddlDateRange
                .Items.Clear()
                .Items.Add(New ListItem("", CStr(JQDateRange.CustomRange)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Clear Date"), CStr(JQDateRange.ClearDate)))
                If ShowReportDateRange Then
                    .Items.Add(New ListItem(ipResources.GetResourceValue("Overdue"), CStr(JQDateRange.Overdue)))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("Overdue") & String.Format(" {0} ", ipResources.GetResourceValue("and")) & ipResources.GetResourceValue("Next 7 Days"), CStr(JQDateRange.OverdueNext7Days)))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("Overdue") & String.Format(" {0} ", ipResources.GetResourceValue("and")) & ipResources.GetResourceValue("Next 14 Days"), CStr(JQDateRange.OverdueNext14Days)))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("Overdue") & String.Format(" {0} ", ipResources.GetResourceValue("and")) & ipResources.GetResourceValue("Next 30 Days"), CStr(JQDateRange.OverdueNext30Days)))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("Next 7 Days"), CStr(JQDateRange.Next7Days)))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("Next 14 Days"), CStr(JQDateRange.Next14Days)))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("Next 30 Days"), CStr(JQDateRange.Next30Days)))
                    .Items.Add(New ListItem("------", CStr(JQDateRange.CustomRange)))
                End If


                .Items.Add(New ListItem(ipResources.GetResourceValue("Current Month"), CStr(JQDateRange.CurrentMonth)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Current Quarter"), CStr(JQDateRange.CurrentQuarter)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Current Year"), CStr(JQDateRange.CurrentYear)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Current Date to End of Quarter"), CStr(JQDateRange.CurrentDateToQuarter)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Current Date to End of Year"), CStr(JQDateRange.CurrentDateToEndOfYear)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Last Month"), CStr(JQDateRange.LastMonth)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Last Quarter"), CStr(JQDateRange.LastQuarter)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Last Year"), CStr(JQDateRange.LastYear)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Month To Date"), CStr(JQDateRange.MonthToDate)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Next Month"), CStr(JQDateRange.NextMonth)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Next Quarter"), CStr(JQDateRange.NextQuarter)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Next Year"), CStr(JQDateRange.NextYear)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Quarter To Date"), CStr(JQDateRange.QuarterToDate)))
                .Items.Add(New ListItem(ipResources.GetResourceValue("Year To Date"), CStr(JQDateRange.YearToDate)))
            End With

            For i As Integer = 1 To _ddlDateRange.Items.Count - 1
                Dim rangeValues As New DateRangeValues
                If IsNumeric(_ddlDateRange.Items.Item(i).Value) Then
                    Me.SetDateRange(rangeValues.StartDate.ToShortDateString, rangeValues.EndDate.ToShortDateString, CType(_ddlDateRange.Items.Item(i).Value, JQDateRange))
                    rangeValues.Range = CType(_ddlDateRange.Items.Item(i).Value, JQDateRange)
                    With DateRangeCollection
                        .Add(rangeValues.Range, rangeValues)
                    End With
                End If
            Next
            ipResources = Nothing
        Catch
            Throw
        End Try

    End Sub

    ''' <summary>
    ''' Gets the javascript that will be used to update the Start and End Dates based on the daterange selection
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDateRangeJS() As String
        Dim sb As New StringBuilder
        sb.Append("function GetSelectedRadioIndex()<<var selectedList=null;if (event.srcElement.id!='')<<selectedList = document.getElementById(event.srcElement.id);>>else if(event.srcElement.htmlFor!=''&&event.srcElement.htmlFor!=undefined)<<selectedList = document.getElementById(event.srcElement.htmlFor);>>if (selectedList!=null)<<return selectedList.value;>>else<<return 0;>>>>")
        sb.Append("function ChangeDate(startDate,endDate,range)<<startDate = document.getElementById(startDate);endDate = document.getElementById(endDate);")
        sb.Append("if (startDate!=null && endDate!=null)<<")
        sb.Append("switch (range)<< ")
        sb.AppendFormat("case '{0}':return false;break;", CInt(JQDateRange.CustomRange))
        sb.AppendFormat("case '{0}':return false;break;", CInt(JQDateRange.Current))
        sb.AppendFormat("case '{0}':startDate.value='';endDate.value='';break;", CInt(JQDateRange.ClearDate))
        sb.Append("case '1':startDate.value={0};endDate.value={1};break;")
        sb.Append("case '2':startDate.value={2};endDate.value={3};break;")
        sb.Append("case '3':startDate.value={4};endDate.value={5};break;")
        sb.Append("case '4':startDate.value={6};endDate.value={7};break;")
        sb.Append("case '5':startDate.value={8};endDate.value={9};break;")
        sb.Append("case '6':startDate.value={10};endDate.value={11};break;")
        sb.Append("case '7':startDate.value={12};endDate.value={13};break;")
        sb.Append("case '8':startDate.value={14};endDate.value={15};break;")
        sb.Append("case '9':startDate.value={16};endDate.value={17};break;")
        sb.Append("case '10':startDate.value={18};endDate.value={19};break;")
        sb.Append("case '11':startDate.value={20};endDate.value={21};break;")
        sb.Append("case '12':startDate.value={22};endDate.value={23};break;")
        sb.Append("case '13':startDate.value={24};endDate.value={25};break;")
        sb.Append("case '14':startDate.value={26};endDate.value={27};break;")
        'If ShowReportDateRange Then
        sb.Append("case '15':startDate.value={28};endDate.value={29};break;")
            sb.Append("case '16':startDate.value={30};endDate.value={31};break;")
            sb.Append("case '17':startDate.value={32};endDate.value={33};break;")
            sb.Append("case '18':startDate.value={34};endDate.value={35};break;")
            sb.Append("case '19':startDate.value={36};endDate.value={37};break;")
            sb.Append("case '20':startDate.value={38};endDate.value={39};break;")
            sb.Append("case '21':startDate.value={40};endDate.value={41};break;")
        'End If
        sb.Append(">>")
        sb.Append(">>UpdateStartEndDates(startDate.value,endDate.value);>>")
        Dim js As String = sb.ToString

        Dim ts As New StringBuilder
        Dim tmpStartDate As DateTime
        Dim tmpEndDate As DateTime

        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.CurrentMonth))
        ts.Append("!")
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.CurrentQuarter))
        ts.Append("!")
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.CurrentYear))
        ts.Append("!")
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.CurrentDateToQuarter))
        ts.Append("!")
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.CurrentDateToEndOfYear))
        ts.Append("!")
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.LastMonth))
        ts.Append("!")
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.LastQuarter))
        ts.Append("!")
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.LastYear))
        ts.Append("!")
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.MonthToDate))
        ts.Append("!")
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.NextMonth))
        ts.Append("!")
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.NextQuarter))
        ts.Append("!")
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.NextYear))
        ts.Append("!")
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.QuarterToDate))
        ts.Append("!")
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.YearToDate))
        ts.Append("!")

        'If ShowReportDateRange Then
        ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.Overdue))
            ts.Append("!")
            ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.OverdueNext7Days))
            ts.Append("!")
            ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.OverdueNext14Days))
            ts.Append("!")
            ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.OverdueNext30Days))
            ts.Append("!")
            ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.Next7Days))
            ts.Append("!")
            ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.Next14Days))
            ts.Append("!")
            ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.Next30Days))
            ts.Append("!")
        'End If
        'ts.Append(Me.SetDateRange(tmpStartDate.ToShortDateString, tmpEndDate.ToShortDateString, JQDateRange.CustomRange))
        'ts.Append(",")
        ts.Append("''!''")
        'ts.Append(",")
        'ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, range.CurrentYear))
        Dim jsArgs As String = ts.ToString
        Dim args As Object() = Split(jsArgs, "!")

        js = String.Format(CultureInfo.CurrentCulture, js, args)
        js = js.Replace("<<", "{")
        js = js.Replace(">>", "}")
        Return js
    End Function

    Private Function DetermineQuarter(ByVal dt As DateTime) As Qtr
        Dim mon As Integer = Month(dt)
        Select Case mon
            Case 1 To 3
                Return Qtr.FirstQuarter
            Case 4 To 6
                Return Qtr.SecondQuarter
            Case 7 To 9
                Return Qtr.ThirdQuarter
            Case 10 To 12
                Return Qtr.FourthQuarter
            Case Else
                Return Qtr.FirstQuarter
        End Select
    End Function

    ''' <summary>
    ''' Determines the Previous quarter of the year that the specified date falls in
    ''' </summary>
    ''' <param name="dt">DateTime - The date used to determine the quarter</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DeterminePreviousQuarter(ByVal dt As DateTime) As Qtr 'Integer
        Dim currentQtr As Integer = DetermineQuarter(dt)
        Select Case currentQtr
            Case 1
                Return Qtr.FourthQuarter
            Case 2
                Return Qtr.FirstQuarter
            Case 3
                Return Qtr.SecondQuarter
            Case 4
                Return Qtr.ThirdQuarter
            Case Else
                Return Qtr.FirstQuarter
        End Select
    End Function

    Private Function DetermineNextQuarter(ByVal dt As DateTime) As Qtr 'Integer
        Dim currentQtr As Integer = DetermineQuarter(dt)
        Select Case currentQtr
            Case 1
                Return Qtr.SecondQuarter
            Case 2
                Return Qtr.ThirdQuarter
            Case 3
                Return Qtr.FourthQuarter
            Case 4
                Return Qtr.FirstQuarter
            Case Else
                Return Qtr.FirstQuarter
        End Select
    End Function

    ''' <summary>
    ''' Sets the Start and End Dates to match the specified quarter
    ''' </summary>
    ''' <param name="currentDate">DateTime - Date that will be used to determine the current quarter</param>
    ''' <param name="quarterToSet">Qtrs - Specified quarter that will be used to set the dates</param>
    ''' <remarks></remarks>
    Private Function GetQuarterRange(ByVal currentDate As DateTime, ByVal quarterToSet As Qtr) As String
        Dim startDT As DateTime
        Dim endDT As DateTime
        Dim qtr As Qtr = quarterToSet
        Try
            Dim currentQuarter As Qtr = Me.DetermineQuarter(currentDate)
            If quarterToSet = qtr.CurrentQuarter Then
                qtr = CType(Me.DetermineQuarter(currentDate), UserControlsJQDateRange.Qtr)
            End If

            Select Case qtr
                Case qtr.FirstQuarter
                    startDT = DateSerial(Year(currentDate), 1, 1)
                    endDT = DateSerial(Year(currentDate), 4, 0)
                Case qtr.SecondQuarter
                    startDT = DateSerial(Year(currentDate), 4, 1)
                    endDT = DateSerial(Year(currentDate), 7, 0)
                Case qtr.ThirdQuarter
                    startDT = DateSerial(Year(currentDate), 7, 1)
                    endDT = DateSerial(Year(currentDate), 10, 0)
                Case qtr.FourthQuarter
                    If currentQuarter = UserControlsJQDateRange.Qtr.FirstQuarter Then
                        currentDate = currentDate.AddYears(-1)
                    End If
                    startDT = DateSerial(Year(currentDate), 10, 1)
                    endDT = DateSerial(Year(currentDate), 12, 31)
            End Select
        Catch
            Throw
        Finally
            GetQuarterRange = startDT & "|" & endDT
        End Try

    End Function

    ''' <summary>
    ''' Sets the Start and End Dates to match the specified date range
    ''' </summary>
    ''' <param name="startDT">DateTime - Used to store the Start Date</param>
    ''' <param name="endDT">DateTime - Used to store the End Date</param>
    ''' <param name="dtRange"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetDateRange(ByRef startDT As String, ByRef endDT As String, ByVal dtRange As JQDateRange) As String
        Dim todaysDate As Date = Now
        Dim qtrDateRange As String

        Select Case dtRange
            Case JQDateRange.CurrentMonth
                startDT = DateSerial(Year(todaysDate), Month(todaysDate), 1).ToShortDateString
                endDT = DateSerial(Year(todaysDate), Month(todaysDate) + 1, 0).ToShortDateString
            Case JQDateRange.CurrentQuarter
                qtrDateRange = Me.GetQuarterRange(todaysDate, Qtr.CurrentQuarter)
                Dim range As String() = qtrDateRange.Split(CChar("|"))
                If range.Length = 2 Then
                    startDT = range(0)
                    endDT = range(1)
                End If
            Case JQDateRange.CurrentYear
                startDT = DateSerial(Year(todaysDate), 1, 1).ToShortDateString
                endDT = DateSerial(Year(todaysDate), 12, 31).ToShortDateString
            Case JQDateRange.CurrentDateToQuarter
                qtrDateRange = Me.GetQuarterRange(todaysDate, Qtr.CurrentQuarter)
                Dim range As String() = qtrDateRange.Split(CChar("|"))
                If range.Length = 2 Then
                    startDT = Now.ToShortDateString
                    endDT = range(1)
                End If
            Case JQDateRange.CurrentDateToEndOfYear
                startDT = Now.ToShortDateString
                endDT = DateSerial(Year(todaysDate), 12, 31).ToShortDateString
            Case JQDateRange.NextMonth
                startDT = DateSerial(Year(todaysDate), Month(todaysDate) + 1, 1).ToShortDateString
                endDT = DateSerial(Year(todaysDate), Month(todaysDate) + 2, 0).ToShortDateString
            Case JQDateRange.LastMonth
                Dim lastMonth = Now.AddMonths(-1)
                Dim endOfLastMonth = DateSerial(Year(Now), Month(Now), 1).AddDays(-1)
                startDT = DateSerial(Year(lastMonth), Month(lastMonth), 1).ToShortDateString
                endDT = endOfLastMonth.ToShortDateString
            Case JQDateRange.LastQuarter
                qtrDateRange = Me.GetQuarterRange(todaysDate, DeterminePreviousQuarter(todaysDate))
                Dim range As String() = qtrDateRange.Split(CChar("|"))
                If range.Length = 2 Then
                    startDT = range(0)
                    endDT = range(1)
                End If
            Case JQDateRange.LastYear
                startDT = DateSerial(Year(todaysDate.AddYears(-1)), 1, 1).ToShortDateString
                endDT = DateSerial(Year(todaysDate.AddYears(-1)), 12, 31).ToShortDateString
            Case JQDateRange.MonthToDate
                startDT = DateSerial(Year(todaysDate), Month(todaysDate), 1).ToShortDateString
                endDT = Now.ToShortDateString
            Case JQDateRange.NextQuarter
                qtrDateRange = Me.GetQuarterRange(todaysDate, DetermineNextQuarter(todaysDate))
                Dim range As String() = qtrDateRange.Split(CChar("|"))
                If range.Length = 2 Then
                    startDT = range(0)
                    endDT = range(1)
                End If
            Case JQDateRange.NextYear
                startDT = DateSerial(Year(todaysDate) + 1, 1, 1).ToShortDateString
                endDT = DateSerial(Year(todaysDate) + 1, 12, 31).ToShortDateString
            Case JQDateRange.YearToDate
                startDT = DateSerial(Year(todaysDate), 1, 1).ToShortDateString
                endDT = todaysDate.ToShortDateString
            Case JQDateRange.QuarterToDate
                qtrDateRange = Me.GetQuarterRange(todaysDate, Qtr.CurrentQuarter)
                Dim range As String() = qtrDateRange.Split(CChar("|"))
                If range.Length = 2 Then
                    startDT = range(0)
                    endDT = todaysDate.ToShortDateString
                End If
            Case JQDateRange.ClearDate, JQDateRange.CustomRange
                startDT = ""
                endDT = ""

            Case JQDateRange.Overdue
                startDT = New Date(2001, 1, 1).ToShortDateString
                endDT = todaysDate.ToShortDateString
            Case JQDateRange.OverdueNext7Days
                startDT = New Date(2001, 1, 1).ToShortDateString
                endDT = todaysDate.AddDays(7).ToShortDateString
            Case JQDateRange.OverdueNext14Days
                startDT = New Date(2001, 1, 1).ToShortDateString
                endDT = todaysDate.AddDays(14).ToShortDateString
            Case JQDateRange.OverdueNext30Days
                startDT = New Date(2001, 1, 1).ToShortDateString
                endDT = todaysDate.AddDays(30).ToShortDateString
            Case JQDateRange.Next7Days
                startDT = todaysDate.ToShortDateString
                endDT = todaysDate.AddDays(7).ToShortDateString
            Case JQDateRange.Next14Days
                startDT = todaysDate.ToShortDateString
                endDT = todaysDate.AddDays(14).ToShortDateString
            Case JQDateRange.Next30Days
                startDT = todaysDate.ToShortDateString
                endDT = todaysDate.AddDays(30).ToShortDateString
            Case Else
                startDT = Now.ToShortDateString
                endDT = Now.ToShortDateString

        End Select
        If IsDate(startDT) AndAlso IsDate(endDT) Then
            startDT = IP.Bids.SharedFunctions.FormatDate(CDate(startDT))
            endDT = IP.Bids.SharedFunctions.FormatDate(CDate(endDT))
        End If
        Return "'" & startDT & "'!'" & endDT & "'"



    End Function

    Friend Structure DateRangeValues
        Public StartDate As Date
        Public EndDate As Date
        Public Range As JQDateRange

        Public Overloads Overrides Function GetHashCode() As Integer
            Throw New NotImplementedException()
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As [Object]) As Boolean
            Throw New NotImplementedException()
        End Function

        Public Shared Operator =(ByVal left As DateRangeValues, ByVal right As DateRangeValues) As Boolean
            Throw New NotImplementedException()
        End Operator

        Public Shared Operator <>(ByVal left As DateRangeValues, ByVal right As DateRangeValues) As Boolean
            Throw New NotImplementedException()
        End Operator
    End Structure

    Protected Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Enabled = False Then
            Me._cbdateCritical.Enabled = False
            Me._txtDateFrom.Enabled = False
            Me._txtDateTo.Enabled = False
            Me._ddlDateRange.Enabled = False
        Else
            Me._cbdateCritical.Enabled = True
            Me._txtDateFrom.Enabled = True
            Me._txtDateTo.Enabled = True
            Me._ddlDateRange.Enabled = True
        End If
        Me._txtDateFrom.AutoPostBack = AllowPostBack
        Me._txtDateTo.AutoPostBack = AllowPostBack
        If Me.Attributes.Item("onFocus") IsNot Nothing Then
            Me._txtDateFrom.Attributes.Add("onFocus", Me.Attributes.Item("onFocus"))
            Me._txtDateTo.Attributes.Add("onFocus", Me.Attributes.Item("onFocus"))
        End If
    End Sub

    Protected Sub _txtDateTo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _txtDateTo.TextChanged
        RaiseEvent TextChanged(sender, e)
    End Sub

    Protected Sub _txtDateFrom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _txtDateFrom.TextChanged
        RaiseEvent TextChanged(sender, e)
    End Sub
End Class
