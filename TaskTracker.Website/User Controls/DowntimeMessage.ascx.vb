
Partial Class User_Controls_DowntimeMessage
    Inherits System.Web.UI.UserControl
    Public Property AlertMessage As String
    Public Property ShowAlert As Boolean

    Private Sub User_Controls_AlertBox_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If AlertMessage IsNot Nothing AndAlso AlertMessage.Length > 0 Then
            _lblAlertInfo.Text = AlertMessage
        End If
        _alertInfo.Visible = ShowAlert
    End Sub

    Private Sub User_Controls_DowntimeMessage_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim downtimeMessage As New Downtime
        If downtimeMessage IsNot Nothing Then
            With downtimeMessage
                If .ShowMessage = False Then Exit Sub
                If .StartDate > Now Then Exit Sub
                If .EndDate < Now Then Exit Sub
                If .Message.Length = 0 Then Exit Sub

                AlertMessage = .Message
                ShowAlert = True
            End With
        End If
    End Sub
End Class
