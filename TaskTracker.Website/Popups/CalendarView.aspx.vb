
Partial Class Popups_CalendarView
    Inherits IP.Bids.BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session.Item("CalendarMonthView") IsNot Nothing Then
            Dim listing As System.Collections.Generic.List(Of TaskListing) = Session.Item("CalendarMonthView")
            Me._calEvents.DisplayEventCalendar(listing)
        End If
    End Sub
End Class
