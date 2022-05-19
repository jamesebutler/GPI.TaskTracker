Imports System.Globalization

'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 08-17-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class DetectScreenResolution
    Inherits System.Web.UI.Page


    Public Sub Page_Load(ByVal sender As [Object], ByVal e As EventArgs) Handles form1.Load

        If Request.QueryString("action") IsNot Nothing Then
            ' store the screen resolution in Session["ScreenResolution"]
            ' and redirect back to default.aspx
            If Request.QueryString("res") IsNot Nothing Then
                Session("ScreenResolution") = Request.QueryString("res").ToString(CultureInfo.CurrentCulture)
            End If
            'Response.Redirect("default.aspx")     
            If Request.QueryString("url") IsNot Nothing Then
                IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveUrl(Request.QueryString("url")))
            Else
                IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveUrl("~/ViewTasks.aspx"))
            End If
            'HttpContext.Current.ApplicationInstance.CompleteRequest()
        Else
            'Session("DetectScreenResolutionURL") = Page.ResolveUrl(Page.AppRelativeVirtualPath) & "?url=" & Server.HtmlEncode(Request.RawUrl)
        End If
    End Sub
    ' JavaScript code below will determine the user screen resolution
    ' and redirect to itself with action=set QueryString parameter

End Class
