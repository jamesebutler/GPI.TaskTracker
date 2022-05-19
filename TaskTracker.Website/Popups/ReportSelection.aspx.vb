
Partial Class Popups_ReportSelection
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.ClientQueryString.Length > 0 Then
            IP.Bids.SharedFunctions.ResponseRedirect("~/ReportSelection.aspx?" & Page.ClientQueryString)
        End If
    End Sub
End Class
