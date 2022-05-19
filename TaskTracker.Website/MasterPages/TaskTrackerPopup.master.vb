
Partial Class MasterPages_TaskTrackerPopup
    Inherits BaseMasterPage

    Public Overrides Sub HideFooter()
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub HideHeaderAndMenu()
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub HideBannerFooter()

    End Sub
    Public Sub DisplayMessageToUser(ByVal message As String, ByVal title As String)
        _alertInfo.Visible = True
        _lblAlertInfo.Text = message
        _lblAlertTitle.Text = title
    End Sub

    Public Sub ClearUserMessage()
        _alertInfo.Visible = False
        _lblAlertInfo.Text = String.Empty
        _lblAlertTitle.Text = String.Empty
    End Sub

    Private Sub MasterPages_TaskTrackerResponsive_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Me.pageHeader.Title = "Task Tracker| " & PageName
    End Sub

    Private Sub MasterPages_TaskTrackerResponsive_Init(sender As Object, e As EventArgs) Handles Me.Init
        ClearUserMessage()
    End Sub
End Class

