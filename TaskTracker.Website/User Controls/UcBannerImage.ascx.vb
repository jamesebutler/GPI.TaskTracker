'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 08-27-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class UserControlsUCBannerImage
    Inherits System.Web.UI.UserControl

    Private m_BannerText As String = String.Empty
    Public Property BannerText() As String
        Get
            Return m_BannerText
        End Get
        Set(ByVal value As String)
            m_BannerText = value
        End Set
    End Property

    Private m_DisplayPopupBanner As Boolean
    Public Property DisplayPopupBanner() As Boolean
        Get
            Return m_DisplayPopupBanner
        End Get
        Set(ByVal value As Boolean)
            m_DisplayPopupBanner = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetBanner()
    End Sub
    Public Sub SetBanner()
        Me._imgBanner.ImageUrl = "~/Banner.aspx?textMessage=" & BannerText & "&Theme=" & Page.Theme
    End Sub
End Class
