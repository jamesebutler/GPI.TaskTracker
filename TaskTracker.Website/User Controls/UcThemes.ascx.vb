'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 08-24-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class UserControlsThemes
    Inherits System.Web.UI.UserControl

    Protected Sub _rblThemes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _rblThemes.SelectedIndexChanged
        'Profile("ThemeID") = _rblThemes.SelectedValue
        'Profile("ThemeDesc") = _rblThemes.SelectedItem.Text
        'Response.Redirect(Request.FilePath, True)
        
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    End Sub
    Private Sub LoadThemeList()

        With Me._rblThemes
            .Items.Clear()
            .Items.Add(New ListItem("IRIS", "IRIS"))
            .Items.Add(New ListItem("RI", "RI"))
            '.Items.Add(New ListItem("EIS", "EIS"))
            If Request.Cookies.Item("Theme") IsNot Nothing AndAlso Request.Cookies.Item("Theme").Value IsNot Nothing Then
                If .Items.FindByValue(Request.Cookies.Item("Theme").Value) IsNot Nothing Then
                    .ClearSelection()
                    .Items.FindByValue(Request.Cookies.Item("Theme").Value).Selected = True
                End If
            Else
                .ClearSelection()
                .Items(1).Selected = True
            End If
        End With
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack AndAlso _rblThemes.Items.Count = 0 Then
            LoadThemeList()
        End If

    End Sub

    Protected Sub _btnChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnChange.Click
        If Request.Cookies.Item("Theme") IsNot Nothing Then
            Request.Cookies.Remove("Theme")
        End If
        Dim themeCookie As New HttpCookie("Theme", _rblThemes.SelectedValue)
        With themeCookie
            .Expires = Now.AddDays(30)
        End With
        Response.Cookies.Add(themeCookie)
        IP.Bids.SharedFunctions.ResponseRedirect(Request.FilePath)
        'HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub
End Class
