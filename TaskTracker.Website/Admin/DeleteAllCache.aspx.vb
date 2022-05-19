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

Partial Class AdminDeleteAllCache
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("key") IsNot Nothing Then
            CacheHelper.DeleteSingleCacheItem(Request("key"))
        Else
            CacheHelper.DeleteEntireCache()
        End If

        Dim url As String
        If Request.QueryString("URL") IsNot Nothing AndAlso Request.QueryString("URL").Length > 0 Then
            url = Request.QueryString("URL")
        Else
            url = "~/Admin/CacheViewer.aspx"
        End If
        IP.Bids.SharedFunctions.ResponseRedirect(url)
    End Sub
    Public Sub DeleteEntireCache()
        Dim objItem As Object
        Dim strName As String = String.Empty
        For Each objItem In Cache
            strName = objItem.Key
            'Comment the If..Then if you want to see ALL (System, etc.) items the cache
            'We don't want to see ASP.NET cached system items or ASP.NET Worker Processes
            If (Left(strName, 7) <> "ISAPIWo") Then '(Left(strName, 7) <> "System.") And 
                If Cache.Item(strName) IsNot Nothing Then
                    Cache.Remove(strName)
                End If
            End If
        Next

    End Sub
End Class
