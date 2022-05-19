
Partial Class User_Controls_AlertBox
    Inherits System.Web.UI.UserControl
    Public Property AlertTitle As String
    Public Property AlertMessage As String
    Public Property ShowAlert As Boolean

    Private Sub User_Controls_AlertBox_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        If AlertTitle IsNot Nothing AndAlso AlertTitle.Length > 0 Then
            _lblAlertTitle.Text = AlertTitle
        End If
        If AlertMessage IsNot Nothing AndAlso AlertMessage.Length > 0 Then
            _lblAlertInfo.Text = AlertMessage
        End If
        _alertInfo.Visible = ShowAlert
    End Sub
End Class
