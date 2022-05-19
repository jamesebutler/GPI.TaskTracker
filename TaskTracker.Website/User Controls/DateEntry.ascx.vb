'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 09-02-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-07-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports System.Globalization

Namespace IP.Bids.UserControl
    Partial Class DateEntry
        Inherits UserControlValidation
        Private DateRangeCollection As New Hashtable


#Region "Properties"

        Public Property StartDateLabel() As String
            Get
                Return Me._lblStartDate.Text
            End Get
            Set(ByVal value As String)
                Me._lblStartDate.Text = value
            End Set
        End Property

        Private mDateIsCritical As Boolean
        Public Property DateIsCritical() As Boolean
            Get
                Return mDateIsCritical
            End Get
            Set(ByVal value As Boolean)
                mDateIsCritical = value
            End Set
        End Property

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
                    Case AjaxControlToolkit.PopupControlPopupPosition.Center
                        Me._pceStartDate.OffsetY = 0
                    Case AjaxControlToolkit.PopupControlPopupPosition.Left
                        Me._pceStartDate.OffsetY = 0
                    Case AjaxControlToolkit.PopupControlPopupPosition.Right
                        Me._pceStartDate.OffsetY = 0
                    Case AjaxControlToolkit.PopupControlPopupPosition.Top
                        Me._pceStartDate.OffsetY = -200
                End Select
                Me._pceStartDate.Position = value
            End Set
        End Property      

        Public Property StartDateTextRequired() As Boolean
            Get
                Return Me.EnableValidation
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

#End Region      


'  COMMENTED BY CODEIT.RIGHT
'        Private Function GetGlobalJSVar() As String
'            Dim sb As New StringBuilder
'            sb.AppendLine()
'            sb.Append(" function DateRange(){")
'            sb.Append("this.StartDateID='" & Me._txtStartDate.ClientID & "';")
'            sb.Append("this.StartMonthDDLID='" & Me._ddlStartMonth.ClientID & "';")
'            sb.Append("this.StartYearDDLID='" & Me._ddlStartYear.ClientID & "';")
'            sb.Append("this.StartDatePCE='" & Me._pceStartDate.ClientID & "';")
'            sb.Append("this.StartDateCalID='ce1';")           
'            sb.Append("}")
'            Return sb.ToString
'        End Function
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init            
            If Not Page.IsPostBack Then
                If Me._txtStartDate.Text.Length = 0 Then
                    StartDate = Now.ToShortDateString                   
                End If
            Else
                If Request.Form(_txtStartDate.UniqueID) IsNot Nothing Then
                    StartDate = Request.Form(_txtStartDate.UniqueID)                   
                End If
            End If
            'If Me.EnableValidation = True Then              
            '    Me.ClientValidationFunction = "ValidateStartEndDates"              
            '    Me._ceStartCalendar.OnClientDateSelectionChanged = "SetNewDate"

            '    '_mevStartDate.ValidationGroup = Me.Validation.ValidationGroup()
            'Else
            '    Me.ControlToValidate = _txtStartDate.ID
            '    Me.ClientValidationFunction = ""
            '    Me._ceStartCalendar.OnClientDateSelectionChanged = ""
            'End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'If Not Page.IsPostBack Then


            'Dim startDateJS As String = SetStartDateJS()
            'If Not Page.ClientScript.IsClientScriptBlockRegistered(Page.GetType, "ChangeDateRange") Then Page.ClientScript.RegisterClientScriptBlock(Page.GetType, "ChangeDateRange", GetDateRangeJS() & " " & GetGlobalJSVar(), True)
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
            For i As Integer = 1 To 12
                _ddlStartMonth.Items.Add(New ListItem(MonthName(i, False), i))
            Next

            Dim strMonth As String

            If (Not Me._txtStartDate.Text Is Nothing AndAlso String.IsNullOrEmpty(Me._txtStartDate.Text)) Then
                strMonth = DateTime.Now.Month
            Else
                strMonth = CDate(_txtStartDate.Text.ToString).Month
            End If

            _ddlStartMonth.Items.FindByValue(strMonth).Selected = True
        End Sub

        ' POPULATE THE YEARLIST FROM 20 YEARS AGO TO ONE YEAR HENCE 
        Sub PopulateYearList()
            Dim intYear As Integer
            _ddlStartYear.Items.Clear()
            ' Year list can be changed by changing the lower and upper 
            ' limits of the For statement
            For intYear = DateTime.Now.Year - 10 To DateTime.Now.Year + 10
                _ddlStartYear.Items.Add(intYear.ToString(CultureInfo.CurrentCulture))
            Next
            Dim strYear As String

            If (Not _txtStartDate.Text Is Nothing AndAlso String.IsNullOrEmpty(_txtStartDate.Text)) Then
                _ddlStartYear.Items.FindByValue(DateTime.Now.Year.ToString(CultureInfo.CurrentCulture)).Selected = True
            Else
                strYear = CDate(_txtStartDate.Text.ToString).Year.ToString
                _ddlStartYear.Items.FindByValue(strYear).Selected = True
            End If

        End Sub


        Protected Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender           
            Me._cbdateCritical.Visible = Me.DateIsCritical            
        End Sub
    End Class
End Namespace