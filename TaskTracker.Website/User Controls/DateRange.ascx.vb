'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 08-17-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports System.Globalization

Namespace IP.Bids.UserControl
    Partial Class DateRange
        Inherits UserControlValidation

        Private DateRangeCollection As New Hashtable
#Region "Enum"
        ''' <summary>
        ''' These are the available date range values
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum Range
            CustomRange = -1
            Current = 0
            MonthToDate = 1
            QuarterToDate = 2
            YearToDate = 3
            LastMonth = 4
            LastQuarter = 5
            LastYear = 6
            CurrentYear = 7
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
        End Enum
#End Region

#Region "Properties"
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

        Public WriteOnly Property DateRange() As Range
            Set(ByVal value As Range)
                Dim startDT As Date
                Dim endDT As Date
                Me.SetDateRange(startDT, endDT, value)
                StartDate = startDT
                EndDate = endDT
                Me._ddlDateRange.SelectedValue = value
            End Set
        End Property
#End Region

#Region "Private Events"
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Try
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
                    Me._calStartDate.OnClientDateSelectionChanged = "ValidateStartEndDates"
                    Me._calEndDate.OnClientDateSelectionChanged = "ValidateStartEndDates"
                Else
                    Me.ControlToValidate = _txtEndDate.ID
                    Me.ClientValidationFunction = ""
                    Me._calStartDate.OnClientDateSelectionChanged = ""
                    Me._calEndDate.OnClientDateSelectionChanged = ""
                End If
            Catch
                Throw
            End Try
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try


                If Not Page.ClientScript.IsClientScriptBlockRegistered(Page.GetType, "DateRange") Then
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType, "DateRange", GetDateRangeJS(), True)
                End If
                Me._ddlDateRange.Attributes.Add("onchange", "ChangeDate('" & _txtStartDate.ClientID & "','" & _txtEndDate.ClientID & "',this.value);")
                Me._txtEndDate.ReadOnly = True
                Me._txtStartDate.ReadOnly = True
                Dim selectedRange As Range = Range.CustomRange
                For Each item As DictionaryEntry In DateRangeCollection
                    Dim rangeValue As DateRangeValues
                    rangeValue = CType(item.Value, DateRangeValues)
                    If rangeValue.StartDate = StartDate And rangeValue.EndDate = EndDate Then
                        selectedRange = rangeValue.Range
                    End If
                Next
                _ddlDateRange.SelectedValue = selectedRange
            Catch
                Throw
            End Try
        End Sub
        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If Me.OnClientChange.Length > 0 Then
                'We need to add onchange event handlers for any control that can change
                Dim resetRangeJS As String = ";$get('" & Me._ddlDateRange.ClientID & "').selectedIndex=-1;"
                AppendAttribute(_txtEndDate, "onchange", OnClientChange & resetRangeJS)
                AppendAttribute(_txtStartDate, "onchange", OnClientChange & resetRangeJS)
                AppendAttribute(_ddlDateRange, "onchange", OnClientChange)
            End If
        End Sub
#End Region

#Region "Public Methods"
#End Region

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
                    .Items.Add(New ListItem("", Range.CustomRange))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("CurrentYear"), Range.CurrentYear))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("MonthToDate"), Range.MonthToDate))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("QuarterToDate"), Range.QuarterToDate))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("YearToDate"), Range.YearToDate))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("LastMonth"), Range.LastMonth))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("LastQuarter"), Range.LastQuarter))
                    .Items.Add(New ListItem(ipResources.GetResourceValue("LastYear"), Range.LastYear))
                End With

                For i As Integer = 1 To _ddlDateRange.Items.Count - 1
                    Dim rangeValues As New DateRangeValues
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
        Private Sub SetQuarter(ByVal currentDate As DateTime, ByRef startDT As DateTime, ByRef endDT As DateTime, ByVal quarterToSet As Qtr)
            Try
                Dim qtr As Qtr = quarterToSet
                If quarterToSet = Qtr.CurrentQuarter Then
                    qtr = Me.DetermineQuarter(currentDate)
                End If
                Select Case qtr
                    Case Qtr.FirstQuarter
                        startDT = DateSerial(Year(currentDate), 1, 1)
                        endDT = DateSerial(Year(currentDate), 4, 0)
                    Case Qtr.SecondQuarter
                        startDT = DateSerial(Year(currentDate), 4, 1)
                        endDT = DateSerial(Year(currentDate), 7, 0)
                    Case Qtr.ThirdQuarter
                        startDT = DateSerial(Year(currentDate), 7, 1)
                        endDT = DateSerial(Year(currentDate), 10, 0)
                    Case Qtr.FourthQuarter
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
            sb.Append("function ChangeDate(startDate,endDate,range)<<startDate = document.getElementById(startDate);endDate = document.getElementById(endDate);")
            sb.Append("if (startDate!=null && endDate!=null)<<")
            sb.Append("switch (range)<< ")
            sb.Append("case '-1':return false;break;")
            sb.Append("case '0':return false;break;")
            sb.Append("case '1':startDate.value={0};endDate.value={1};break;")
            sb.Append("case '2':startDate.value={2};endDate.value={3};break;")
            sb.Append("case '3':startDate.value={4};endDate.value={5};break;")
            sb.Append("case '4':startDate.value={6};endDate.value={7};break;")
            sb.Append("case '5':startDate.value={8};endDate.value={9};break;")
            sb.Append("case '6':startDate.value={10};endDate.value={11};break;")
            sb.Append("case '7':startDate.value={12};endDate.value={13};break;")
            'sb.Append("case '7':startDate.value={12};endDate.value={13};break;")

            sb.Append(">>")
            sb.Append(">>UpdateStartEndDates(startDate.value,endDate.value);>>")
            Dim js As String = sb.ToString

            Dim ts As New StringBuilder
            Dim tmpStartDate As DateTime
            Dim tmpEndDate As DateTime

            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, Range.CurrentYear))
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, Range.MonthToDate))
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, Range.QuarterToDate))
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, Range.YearToDate))
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, Range.LastMonth))
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, Range.LastQuarter))
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, Range.LastYear))
            ts.Append(",")
            ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, Range.CustomRange))
            'ts.Append(",")
            'ts.Append(Me.SetDateRange(tmpStartDate, tmpEndDate, range.CurrentYear))
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
        Private Function SetDateRange(ByRef startDT As String, ByRef endDT As String, ByVal dtRange As Range) As String
            Dim todaysDate As Date = Now
            Select Case dtRange
                Case Range.LastMonth
                    startDT = DateSerial(Year(todaysDate), Month(todaysDate) - 1, 1).ToShortDateString
                    endDT = DateSerial(Year(todaysDate), Month(todaysDate), 0).ToShortDateString
                Case Range.MonthToDate
                    startDT = DateSerial(Year(todaysDate), Month(todaysDate), 1).ToShortDateString
                    endDT = DateSerial(Year(todaysDate), Month(todaysDate), Day(todaysDate)).ToShortDateString
                Case Range.QuarterToDate
                    Me.SetQuarter(todaysDate, startDT, endDT, Qtr.CurrentQuarter)
                    endDT = Now.ToShortDateString
                Case Range.YearToDate '"year to date"
                    startDT = DateSerial(Year(todaysDate), 1, 1).ToShortDateString
                    endDT = todaysDate.ToShortDateString
                Case Range.LastQuarter
                    Me.SetQuarter(todaysDate, startDT, endDT, Me.DeterminePreviousQuarter(todaysDate))
                Case Range.LastYear
                    startDT = DateSerial(Year(todaysDate) - 1, 1, 1).ToShortDateString
                    endDT = DateSerial(Year(todaysDate) - 1, 12, 31).ToShortDateString
                Case Else
                    startDT = Now.ToShortDateString
                    endDT = Now.ToShortDateString

            End Select
            Return "'" & startDT & "','" & endDT & "'"

        End Function
#End Region

        Friend Structure DateRangeValues
            Public StartDate As Date
            Public EndDate As Date
            Public Range As Range

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
    End Class
End Namespace