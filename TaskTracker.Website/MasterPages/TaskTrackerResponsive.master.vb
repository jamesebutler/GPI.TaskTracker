
Partial Class MasterPages_TaskTrackerResponsive
    Inherits BaseMasterPage

    Public Overrides Sub HideFooter()
        'Throw New NotImplementedException()
    End Sub

    Public Overrides Sub HideHeaderAndMenu()
        _pnlNavBar.Visible = False
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
    Public Sub SetActiveNav(id As String)
        'SetActiveNav
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "SetActiveNav", String.Format(System.Globalization.CultureInfo.CurrentCulture, "SetActiveNav('{0}');", id), True)
    End Sub

    Private Sub MasterPages_TaskTrackerResponsive_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Me.pageHeader.Title = "Task Tracker| " & PageName
    End Sub

    Private Sub MasterPages_TaskTrackerResponsive_Init(sender As Object, e As EventArgs) Handles Me.Init
        ClearUserMessage()

        If CType(Application.Item("TestDatabase"), String) = "YES" Then
            warningTestDatabase.Visible = CBool("True")
        Else
            warningTestDatabase.Visible = CBool("False")
        End If
    End Sub
End Class

