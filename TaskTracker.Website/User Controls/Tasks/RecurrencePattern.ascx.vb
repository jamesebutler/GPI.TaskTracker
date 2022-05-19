'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 02-04-2011
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class UserControlsRecurrencePattern
    Inherits System.Web.UI.UserControl

    Public Sub HandleLoad()
        Me._pnlDailyFrequency.Visible = False
        Me._pnlWeeklyFrequency.Visible = False
        Me._pnlMonthlyFrequency.Visible = False
        'Me._pnlSemiAnnual.Visible = False
        Me._pnlYearly.Visible = False
        Me._pnlQuarterly.Visible = False
        Me._pnlRangeOfRecurrence.Visible = True
        If Not Page.IsPostBack Then
            PopulateFrequency()
            PopulateDayOfWeek()
            PopulateMonthsOfYear()
            Me._rblFrequency.Items(0).Selected = True
            Me._pnlRangeOfRecurrence.Visible = False
        End If
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
                dr.Item("Value") = MonthName(i)
                .Rows.Add(dr)
            Next
            With _ddlMonths
                .DataSource = dtMonthOfYear
                .DataTextField = "Name"
                .DataValueField = "Value"
                .DataBind()
            End With
            With _ddlMonthsOfYear
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
            dr.Item("Name") = "Sunday"
            dr.Item("Value") = "Sunday"
            .Rows.Add(dr)
            dr = .NewRow
            dr.Item("Name") = "Monday"
            dr.Item("Value") = "Monday"
            .Rows.Add(dr)
            dr = .NewRow
            dr.Item("Name") = "Tuesday"
            dr.Item("Value") = "Tuesday"
            .Rows.Add(dr)
            dr = .NewRow
            dr.Item("Name") = "Wednesday"
            dr.Item("Value") = "Wednesday"
            .Rows.Add(dr)
            dr = .NewRow
            dr.Item("Name") = "Thursday"
            dr.Item("Value") = "Thursday"
            .Rows.Add(dr)
            dr = .NewRow
            dr.Item("Name") = "Friday"
            dr.Item("Value") = "Friday"
            .Rows.Add(dr)
            dr = .NewRow
            dr.Item("Name") = "Saturday"
            dr.Item("Value") = "Saturday"
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
            dr.Item("Value") = "Weekday"
            .Rows.Add(dr)

            With _ddlQtrDayOfWeek
                .DataSource = dtDayOfWeek
                .DataTextField = "Name"
                .DataValueField = "Value"
                .DataBind()
            End With

            With _ddlMonthDayOfWeek
                .DataSource = dtDayOfWeek
                .DataTextField = "Name"
                .DataValueField = "Value"
                .DataBind()
            End With
            'With _ddlSemiDayOfWeek
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
                dr = .NewRow
                dr.Item("Name") = "One Time Only"
                dr.Item("Value") = "One Time Only"
                .Rows.Add(dr)
                dr = .NewRow
                dr.Item("Name") = "Daily"
                dr.Item("Value") = "Daily"
                .Rows.Add(dr)
                dr = .NewRow
                dr.Item("Name") = "Weekly"
                dr.Item("Value") = "Weekly"
                .Rows.Add(dr)
                dr = .NewRow
                dr.Item("Name") = "Monthly/Quarterly/Semi-Annual"
                dr.Item("Value") = "Monthly"
                .Rows.Add(dr)
                dr = .NewRow
                dr.Item("Name") = "Quarterly"
                dr.Item("Value") = "Quarterly"
                .Rows.Add(dr)
                dr = .NewRow
                'dr.Item("Name") = "Semi-Annual"
                'dr.Item("Value") = "Semi-Annual"
                '.Rows.Add(dr)
                'dr = .NewRow
                dr.Item("Name") = "Yearly"
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


#Region "Events"


    Protected Sub _rblFrequency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _rblFrequency.SelectedIndexChanged

        Select Case _rblFrequency.SelectedValue.ToLower
            Case "daily"
                Me._pnlDailyFrequency.Visible = True
            Case "weekly"
                Me._pnlWeeklyFrequency.Visible = True
            Case "monthly", "quarterly", ""
                Me._pnlMonthlyFrequency.Visible = True
            Case "quarterly"
                Me._pnlQuarterly.Visible = True
            Case "semi-annual"
                'Me._pnlSemiAnnual.Visible = True
            Case "yearly"
                Me._pnlYearly.Visible = True
            Case Else
                Me._pnlRangeOfRecurrence.Visible = False
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandleLoad()
    End Sub
#End Region


End Class
