'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-02-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports System.Globalization

Namespace IP.Bids.UserControl
    Partial Class CoolCalendarVB
        Inherits UserControlValidation
        Private DateRangeCollection As New Hashtable

#Region "Enum"
        ''' <summary>
        ''' These are the available date range values
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum EnhancedRange
            CustomRange = -1
            Current = 0
            MonthToDate = 1
            QuarterToDate = 2
            YearToDate = 3
            LastMonth = 4
            LastQuarter = 5
            LastYear = 6
            CurrentYear = 7
            Today = 8
            CurrentWeek = 9
            CurrentMonth = 10
        End Enum

        ''' <summary>
        ''' Contains the available quarters that will be used along with the date range
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum EnhancedQtr
            CurrentQuarter = 0
            FirstQuarter = 1
            SecondQuarter = 2
            ThirdQuarter = 3
            FourthQuarter = 4
        End Enum
#End Region

#Region "Properties"

        Public Property StartDateLabel() As String
            Get
                Return Me._lblStartDate.Text
            End Get
            Set(ByVal value As String)
                Me._lblStartDate.Text = value
            End Set
        End Property

        Public Property EndDateLabel() As String
            Get
                Return Me._lblEndDate.Text
            End Get
            Set(ByVal value As String)
                Me._lblEndDate.Text = value
            End Set
        End Property

        'Public Property ValidationGroup() As String
        '    Get
        '        Return Me._txtStartDateRequired.ValidationGroup

        '    End Get
        '    Set(ByVal value As String)
        '        Me._txtStartDateRequired.ValidationGroup = value
        '        Me._txtEndDateRequired.ValidationGroup = value
        '    End Set
        'End Property 
        Public Property GroupingText() As String
            Get
                Return _pnlBorder.GroupingText
            End Get
            Set(ByVal value As String)
                _pnlBorder.GroupingText = value
            End Set
        End Property
        Public Property Position() As AjaxControlToolkit.PopupControlPopupPosition
            Get
                Return Me._pceStartDate.Position
            End Get
            Set(ByVal value As AjaxControlToolkit.PopupControlPopupPosition)
                Select Case value
                    Case AjaxControlToolkit.PopupControlPopupPosition.Bottom
                        Me._pceStartDate.OffsetY = 0
                        Me._pceEndDate.OffsetY = 0
                    Case AjaxControlToolkit.PopupControlPopupPosition.Center
                        Me._pceStartDate.OffsetY = 0
                        Me._pceEndDate.OffsetY = 0
                    Case AjaxControlToolkit.PopupControlPopupPosition.Left
                        Me._pceStartDate.OffsetY = 0
                        Me._pceEndDate.OffsetY = 0
                    Case AjaxControlToolkit.PopupControlPopupPosition.Right
                        Me._pceStartDate.OffsetY = 0
                        Me._pceEndDate.OffsetY = 0
                    Case AjaxControlToolkit.PopupControlPopupPosition.Top
                        Me._pceStartDate.OffsetY = -200
                        Me._pceEndDate.OffsetY = -200
                End Select
                Me._pceStartDate.Position = value
                Me._pceEndDate.Position = value
            End Set
        End Property
        'Public Property StartDateValue() As String
        '    Get
        '        Return _txtStartDate.Text
        '    End Get
        '    Set(ByVal value As String)
        '        _txtStartDate.Text = value
        '    End Set
        'End Property
        'Public Property EndDateValue() As String
        '    Get
        '        Return _txtEndDate.Text
        '    End Get
        '    Set(ByVal value As String)
        '        _txtEndDate.Text = value
        '    End Set
        'End Property

        Public Property StartDateTextRequired() As Boolean
            Get
                Return Me.EnableValidation
            End Get
            Set(ByVal value As Boolean)
                EnableValidation = value
            End Set
        End Property

        'Public Property StartDateTextRequiredText() As String
        '    Get
        '        Return _txtStartDateRequired.ErrorMessage
        '    End Get
        '    Set(ByVal value As String)
        '        _txtStartDateRequired.ErrorMessage = value
        '    End Set
        'End Property

        Public Property EndDateTextRequired() As Boolean
            Get
                Return EnableValidation
            End Get
            Set(ByVal value As Boolean)
                EnableValidation = value
            End Set
        End Property

        'Public Property EndDateTextRequiredText() As String
        '    Get
        '        Return _txtEndDateRequired.ErrorMessage
        '    End Get
        '    Set(ByVal value As String)
        '        _txtEndDateRequired.ErrorMessage = value
        '    End Set
        'End Property

        ''' <summary>
        ''' Gets or sets the Start Date for the control
        ''' </summary>
        Public Property StartDate() As Date
            Get
                Return IIf(IsDate(Me._txtStartDate.Text), Me._txtStartDate.Text, Now.ToShortDateString)
            End Get
            Set(ByVal value As Date)
                Me._txtStartDate.Text = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the End Date for the control
        ''' </summary>
        Public Property EndDate() As Date
            Get
                Return IIf(IsDate(Me._txtEndDate.Text), Me._txtEndDate.Text, Now.ToShortDateString)
            End Get
            Set(ByVal value As Date)
                Me._txtEndDate.Text = value
            End Set
        End Property

        Public WriteOnly Property DateRange() As EnhancedRange
            Set(ByVal value As EnhancedRange)
                Dim startDT As Date
                Dim endDT As Date
                Me.SetDateRange(startDT, endDT, value)
                StartDate = startDT
                EndDate = endDT
                Me._ddlDateRange.SelectedValue = value
            End Set
        End Property

        Public Property DisplayDateRange() As Boolean
            Get
                Return Me._pnlDateRange.Visible
            End Get
            Set(ByVal value As Boolean)
                Me._pnlDateRange.Visible = value
            End Set
        End Property
#End Region
        'function DateRange(){
        '       this.StartDateID='<%=_txtStartDate.ClientID %>';
        '       this.StartMonthDDLID='<%=_ddlStartMonth.ClientID %>';
        '       this.StartYearDDLID='<%=_ddlStartYear.ClientID %>';
        '       this.StartDatePCE='<%=_pceStartDate.ClientID%>';
        '       this.StartDateCalID='ce1';

        '       this.EndDateID='<%=_txtEndDate.ClientID %>';
        '       this.EndMonthDDLID='<%=_ddlEndMonth.ClientID %>';
        '       this.EndYearDDLID='<%=_ddlEndYear.ClientID %>';
        '       this.EndDatePCE='<%=_pceEndDate.ClientID%>';
        '       this.EndDateCalID='ce2';
        '       //var StartDate = $find("ce1");
        '       //var EndDate = $find("ce2");
        '   }


        Private Function GetGlobalJSVar() As String
            Dim sb As New StringBuilder
            sb.AppendLine()
            sb.Append(" function DateRange(){")
            sb.Append("this.StartDateID='" & Me._txtStartDate.ClientID & "';")
            sb.Append("this.StartMonthDDLID='" & Me._ddlStartMonth.ClientID & "';")
            sb.Append("this.StartYearDDLID='" & Me._ddlStartYear.ClientID & "';")
            sb.Append("this.StartDatePCE='" & Me._pceStartDate.ClientID & "';")
            sb.Append("this.StartDateCalID='ce1';")
            sb.Append("this.EndDateID='" & Me._txtEndDate.ClientID & "';")
            sb.Append("this.EndMonthDDLID='" & Me._ddlEndMonth.ClientID & "';")
            sb.Append("this.EndYearDDLID='" & Me._ddlEndYear.ClientID & "';")
            sb.Append("this.EndDatePCE='" & Me._pceEndDate.ClientID & "';")
            sb.Append("this.EndDateCalID='ce2';")
            sb.Append("}")
            Return sb.ToString
        End Function
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If Me._ddlDateRange.Items.Count = 0 Then
                PopulateDateRange()
            End If
            If Not Page.IsPostBack Then
                If Me._txtStartDate.Text.Length = 0 And Me._txtEndDate.Text.Length = 0 Then
                    StartDate = Now.ToShortDateString
                    EndDate = Now.ToShortDateString
                End If
            Else
                If Request.Form(_txtStartDate.UniqueID) IsNot Nothing And Request.Form(_txtEndDate.UniqueID) IsNot Nothing Then
                    StartDate = Request.Form(_txtStartDate.UniqueID)
                    EndDate = Request.Form(_txtEndDate.UniqueID)

                End If
            End If
            If Me.EnableValidation = True Then
                Me.ControlToValidate = _txtEndDate.ID
                Me.ClientValidationFunction = "ValidateStartEndDates"
                Me._ceEndCalendar.OnClientDateSelectionChanged = "SetNewDate"
                Me._ceStartCalendar.OnClientDateSelectionChanged = "SetNewDate"

                '_mevStartDate.ValidationGroup = Me.Validation.ValidationGroup()
            Else
                Me.ControlToValidate = _txtEndDate.ID
                Me.ClientValidationFunction = ""
                Me._ceStartCalendar.OnClientDateSelectionChanged = ""
                Me._ceEndCalendar.OnClientDateSelectionChanged = ""
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'If Not Page.IsPostBack Then


            'Dim startDateJS As String = SetStartDateJS()
            If Not Page.ClientScript.IsClientScriptBlockRegistered(Page.GetType, "ChangeDateRange") Then
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType, "ChangeDateRange", GetDateRangeJS() & " " & GetGlobalJSVar(), True)
            End If
            Me._ddlDateRange.Attributes.Add("onchange", "ChangeDateRange('" & _txtStartDate.ClientID & "','" & _txtEndDate.ClientID & "',this.value);")
            PopulateMonthList()
            PopulateYearList()
            Dim dt As Date
            If Not (Not _txtStartDate.Text Is Nothing AndAlso String.IsNullOrEmpty(_txtStartDate.Text)) Then
                dt = _txtStartDate.Text
                'Calendar1.TodaysDate = dt
            End If
            'End If
            Dim setMonthYearJS As String = "SetMonthYear('{0}','{1}',{2});"
            'SetMonthYear
            Me._ddlStartMonth.Attributes.Add("onchange", String.Format(CultureInfo.CurrentCulture, setMonthYearJS, Me._ddlStartMonth.ClientID, Me._ddlStartYear.ClientID, 1))
            Me._ddlStartYear.Attributes.Add("onchange", String.Format(CultureInfo.CurrentCulture, setMonthYearJS, Me._ddlStartMonth.ClientID, Me._ddlStartYear.ClientID, 1))
            Me._ddlEndMonth.Attributes.Add("onchange", String.Format(CultureInfo.CurrentCulture, setMonthYearJS, Me._ddlEndMonth.ClientID, Me._ddlEndYear.ClientID, 2))
            Me._ddlEndYear.Attributes.Add("onchange", String.Format(CultureInfo.CurrentCulture, setMonthYearJS, Me._ddlEndMonth.ClientID, Me._ddlEndYear.ClientID, 2))
            If Me.EnableValidation = True Then
                _lblRequiredField.Visible = True
            Else
                _lblRequiredField.Visible = False
            End If
            'ValidateStartEndDates
        End Sub

        '' SET THE FIRST CALENDAR (FROM DATE) 
        'Protected Sub Set_Calendar(ByVal sender As Object, ByVal e As System.EventArgs)
        '    Dim theDate As String = _ddlStartMonth.SelectedItem.Value + " 1, " + _ddlStartYear.SelectedItem.Value
        '    Dim dtFoo As DateTime = System.Convert.ToDateTime(theDate)
        '    'Calendar1.TodaysDate = dtFoo
        'End Sub

        Protected Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            ' Popup result is the selected date
            '_ddlStartMonth.SelectedIndex = Calendar1.SelectedDate.Month - 1
            _ddlStartYear.SelectedItem.Selected = False
            'drpCalYear.Items.FindByValue(Calendar1.SelectedDate.Year).Selected = True

            ' Popup result is the selected date
            '_pceStartDate.Commit(Calendar1.SelectedDate.ToShortDateString())
        End Sub

        ' INNOVATIVE CALENDARS VIA AJAX THAT ALLOW USER TO PICK MONTH AND DATE VIA DROPDOWN
        Sub PopulateMonthList()
            _ddlStartMonth.Items.Clear()
            _ddlEndMonth.Items.Clear()
            For i As Integer = 1 To 12
                _ddlStartMonth.Items.Add(New ListItem(MonthName(i, False), i))
                _ddlEndMonth.Items.Add(New ListItem(MonthName(i, False), i))
            Next

            Dim strMonth As String

            If (Not Me._txtStartDate.Text Is Nothing AndAlso String.IsNullOrEmpty(Me._txtStartDate.Text)) Then
                strMonth = DateTime.Now.Month
            Else
                strMonth = CDate(_txtStartDate.Text.ToString).Month
            End If

            _ddlStartMonth.Items.FindByValue(strMonth).Selected = True

            If (Not Me._txtEndDate.Text Is Nothing AndAlso String.IsNullOrEmpty(Me._txtEndDate.Text)) Then
                strMonth = DateTime.Now.Month
            Else
                strMonth = CDate(_txtStartDate.Text.ToString).Month
            End If
            _ddlEndMonth.Items.FindByValue(strMonth).Selected = True
        End Sub

        ' POPULATE THE YEARLIST FROM 20 YEARS AGO TO ONE YEAR HENCE 
        Sub PopulateYearList()
            Dim intYear As Integer
            _ddlStartYear.Items.Clear()
            _ddlEndYear.Items.Clear()
            ' Year list can be changed by changing the lower and upper 
            ' limits of the For statement
            For intYear = DateTime.Now.Year - 10 To DateTime.Now.Year + 10
                _ddlStartYear.Items.Add(intYear.ToString(CultureInfo.CurrentCulture))
                _ddlEndYear.Items.Add(intYear.ToString(CultureInfo.CurrentCulture))
            Next
            Dim strYear As String

            If (Not _txtStartDate.Text Is Nothing AndAlso String.IsNullOrEmpty(_txtStartDate.Text)) Then
                _ddlStartYear.Items.FindByValue(DateTime.Now.Year.ToString(CultureInfo.CurrentCulture)).Selected = True
            Else
                strYear = CDate(_txtStartDate.Text.ToString).Year.ToString
                _ddlStartYear.Items.FindByValue(strYear).Selected = True
            End If

            If (Not _txtEndDate.Text Is Nothing AndAlso String.IsNullOrEmpty(_txtEndDate.Text)) Then
                _ddlEndYear.Items.FindByValue(DateTime.Now.Year.ToString(CultureInfo.CurrentCulture)).Selected = True
            Else
                strYear = CDate(_txtEndDate.Text.ToString).Year.ToString
                _ddlEndYear.Items.FindByValue(strYear).Selected = True
            End If

        End Sub
#Region "Private Methods"


        ''' <summary>
        ''' Adds data to the Date Range dropdown.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateDateRange()
            Try
                Dim ipResources As New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization()
                With _ddlDateRange
                    .Items.Clear()
                    .Items.Add(New ListItem("", EnhancedRange.CustomRange))                    
                    .Items.Add(New ListItem(ipResources.GetResourceValue("Today"), EnhancedRange.Today))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("Current Week"), EnhancedRange.CurrentWeek))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("Current Month"), EnhancedRange.CurrentMonth))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("CurrentYear"), EnhancedRange.CurrentYear))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("MonthToDate"), EnhancedRange.MonthToDate))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("QuarterToDate"), EnhancedRange.QuarterToDate))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("YearToDate"), EnhancedRange.YearToDate))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("LastMonth"), EnhancedRange.LastMonth))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("LastQuarter"), EnhancedRange.LastQuarter))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("LastYear"), EnhancedRange.LastYear))
                End With

                For i As Integer = 1 To _ddlDateRange.Items.Count - 1
                    Dim rangeValues As New EnhancedDateRangeValues
                    If IsNumeric(_ddlDateRange.Items.Item(i).Value) Then
                        Me.SetDateRange(rangeValues.StartDate, rangeValues.EndDate, _ddlDateRange.Items.Item(i).Value)
                        rangeValues.Range = _ddlDateRange.Items.Item(i).Value
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
        ''' Determines the Quarter of the year that the specified date falls in
        ''' </summary>
        ''' <param name="dt">DateTime - The date used to determine the quarter</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function DetermineQuarter(ByVal dt As DateTime) As Integer
            Dim mon As Integer = Month(dt)
            Select Case mon
                Case 1 To 3
                    Return 1
                Case 4 To 6
                    Return 2
                Case 7 To 9
                    Return 3
                Case 10 To 12
                    Return 4
                Case Else
                    Return 1
            End Select
        End Function

        ''' <summary>
        ''' Determines the Previous quarter of the year that the specified date falls in
        ''' </summary>
        ''' <param name="dt">DateTime - The date used to determine the quarter</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function DeterminePreviousQuarter(ByVal dt As DateTime) As Integer
            Dim currentQtr As Integer = DetermineQuarter(dt)
            Select Case currentQtr
                Case 1
                    Return 4
                Case 2
                    Return 1
                Case 3
                    Return 2
                Case 4
                    Return 3
                Case Else
                    Return 1
            End Select
        End Function

        ''' <summary>
        ''' Sets the Start and End Dates to match the specified quarter
        ''' </summary>
        ''' <param name="currentDate">DateTime - Date that will be used to determine the current quarter</param>
        ''' <param name="startDT">DateTime - Used to store the Start Date</param>
        ''' <param name="endDT">DateTime - Used to store the End Date</param>
        ''' <param name="quarterToSet">Qtrs - Specified quarter that will be used to set the dates</param>
        ''' <remarks></remarks>
        Private Sub SetQuarter(ByVal currentDate As DateTime, ByRef startDT As DateTime, ByRef endDT As DateTime, ByVal quarterToSet As EnhancedQtr)
            Try
                Dim qtr As EnhancedQtr = quarterToSet
                If quarterToSet = EnhancedQtr.CurrentQuarter Then
                    qtr = Me.DetermineQuarter(currentDate)
                End If
                Select Case qtr
                    Case EnhancedQtr.FirstQuarter
                        startDT = DateSerial(Year(currentDate), 1, 1)
                        endDT = DateSerial(Year(currentDate), 4, 0)
                    Case EnhancedQtr.SecondQuarter
                        startDT = DateSerial(Year(currentDate), 4, 1)
                        endDT = DateSerial(Year(currentDate), 7, 0)
                    Case EnhancedQtr.ThirdQuarter
                        startDT = DateSerial(Year(currentDate), 7, 1)
                        endDT = DateSerial(Year(currentDate), 10, 0)
                    Case EnhancedQtr.FourthQuarter
                        startDT = DateSerial(Year(currentDate), 10, 1)
                        endDT = DateSerial(Year(currentDate), 12, 31)
                End Select
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
            sb.Append("function ChangeDateRange(startDate,endDate,range)<<startDate = document.getElementById(startDate);endDate = document.getElementById(endDate);")
            sb.Append("if (startDate!=null && endDate!=null)<<")
            sb.Append("switch (range)<< ")
            'sb.Append("case '-1':return false;break;")
            '            sb.Append("case '0':return false;break;")
            sb.Append("case '-1':startDate.value='';endDate.value='';break;")
            sb.Append("case '0':startDate.value='';endDate.value='';break;")
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
            'sb.Append("case '7':startDate.value={12};endDate.value={13};break;")
            'Today = 8
            'CurrentWeek = 9
            'CurrentMonth = 10
            sb.Append(">>")
            sb.Append(">>UpdateStartEndDates(startDate.value,endDate.value);>>")
            Dim js As String = sb.ToString

            Dim ts As New StringBuilder
            Dim tmpStartDate As DateTime
            Dim tmpEndDate As DateTime

            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, EnhancedRange.MonthToDate)) '1
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, EnhancedRange.QuarterToDate)) '2
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, EnhancedRange.YearToDate)) '3
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, EnhancedRange.LastMonth)) '4
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, EnhancedRange.LastQuarter)) '5
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, EnhancedRange.LastYear)) '6
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, EnhancedRange.CurrentYear)) '7
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, EnhancedRange.Today)) '8
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, EnhancedRange.CurrentWeek)) '9
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, EnhancedRange.CurrentMonth)) '10
            'Else
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, EnhancedRange.CustomRange))

            Dim jsArgs As String = ts.ToString
            Dim args As Object() = Split(jsArgs, ",")

            js = String.Format(CultureInfo.CurrentCulture, js, args)
            js = js.Replace("<<", "{")
            js = js.Replace(">>", "}")
            Return js
        End Function



        ''' <summary>
        ''' Sets the Start and End Dates to match the specified date range
        ''' </summary>
        ''' <param name="startDT">DateTime - Used to store the Start Date</param>
        ''' <param name="endDT">DateTime - Used to store the End Date</param>
        ''' <param name="dtRange"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function SetDateRange(ByRef startDT As String, ByRef endDT As String, ByVal dtRange As EnhancedRange) As String
            Dim todaysDate As Date = Now
            Select Case dtRange
                Case EnhancedRange.LastMonth
                    startDT = DateSerial(Year(todaysDate), Month(todaysDate) - 1, 1).ToShortDateString
                    endDT = DateSerial(Year(todaysDate), Month(todaysDate), 0).ToShortDateString
                Case EnhancedRange.MonthToDate
                    startDT = DateSerial(Year(todaysDate), Month(todaysDate), 1).ToShortDateString
                    endDT = DateSerial(Year(todaysDate), Month(todaysDate), Day(todaysDate)).ToShortDateString
                Case EnhancedRange.QuarterToDate
                    Me.SetQuarter(todaysDate, startDT, endDT, EnhancedQtr.CurrentQuarter)
                    endDT = Now.ToShortDateString
                Case EnhancedRange.YearToDate '"year to date"
                    startDT = DateSerial(Year(todaysDate), 1, 1).ToShortDateString
                    endDT = todaysDate.ToShortDateString
                Case EnhancedRange.LastQuarter
                    Me.SetQuarter(todaysDate, startDT, endDT, Me.DeterminePreviousQuarter(todaysDate))
                Case EnhancedRange.LastYear
                    startDT = DateSerial(Year(todaysDate) - 1, 1, 1).ToShortDateString
                    endDT = DateSerial(Year(todaysDate) - 1, 12, 31).ToShortDateString
                Case EnhancedRange.CurrentYear
                    startDT = DateSerial(Year(todaysDate), 1, 1).ToShortDateString
                    endDT = DateSerial(Year(todaysDate), 12, 31).ToShortDateString
                Case EnhancedRange.CurrentMonth
                    startDT = DateSerial(Year(todaysDate), Month(todaysDate), 1).ToShortDateString
                    endDT = IP.Bids.SharedFunctions.LastDayOfMonth(todaysDate).ToShortDateString
                Case EnhancedRange.CurrentWeek
                    startDT = IP.Bids.SharedFunctions.FirstDayOfWeek(todaysDate).ToShortDateString
                    endDT = IP.Bids.SharedFunctions.LastDayOfWeek(todaysDate).ToShortDateString
                Case EnhancedRange.Today
                    startDT = todaysDate.ToShortDateString
                    endDT = todaysDate.ToShortDateString
                Case Else
                    startDT = Now.ToShortDateString
                    endDT = Now.ToShortDateString

            End Select
            Return "'" & startDT & "','" & endDT & "'"

        End Function
#End Region
        Friend Structure EnhancedDateRangeValues
            Public StartDate As Date
            Public EndDate As Date
            Public Range As EnhancedRange

            Public Overloads Overrides Function GetHashCode() As Integer
                Throw New NotImplementedException()
            End Function

            Public Overloads Overrides Function Equals(ByVal obj As [Object]) As Boolean
                Throw New NotImplementedException()
            End Function

            Public Shared Operator =(ByVal left As EnhancedDateRangeValues, ByVal right As EnhancedDateRangeValues) As Boolean
                Throw New NotImplementedException()
            End Operator

            Public Shared Operator <>(ByVal left As EnhancedDateRangeValues, ByVal right As EnhancedDateRangeValues) As Boolean
                Throw New NotImplementedException()
            End Operator
        End Structure

        Protected Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If Me.OnClientChange.Length > 0 Then
                'We need to add onchange event handlers for any control that can change
                Dim resetRangeJS As String = ";$get('" & Me._ddlDateRange.ClientID & "').selectedIndex=-1;"
                AppendAttribute(_txtEndDate, "onchange", OnClientChange & resetRangeJS)
                AppendAttribute(_txtStartDate, "onchange", OnClientChange & resetRangeJS)
                AppendAttribute(Me._txtEndDateHiddenValue, "onchange", OnClientChange & resetRangeJS)
                AppendAttribute(Me._txtStartDateHiddenValue, "onchange", OnClientChange & resetRangeJS)
                AppendAttribute(_ddlDateRange, "onchange", OnClientChange)
            End If
           
        End Sub
    End Class
End Namespace