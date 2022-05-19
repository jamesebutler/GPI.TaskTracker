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
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Partial Class ImagesLoadImages
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim imageUrl As String = String.Empty
        Dim cacheDays As Integer = 10
        Try
            If Request.QueryString("ImageURL") IsNot Nothing Then
                imageUrl = Page.ResolveClientUrl(Request.QueryString("ImageURL"))
            Else
                imageUrl = Page.ResolveClientUrl("~/Images/spacer.gif")
            End If
            If Request.QueryString("CacheDays") IsNot Nothing Then
                cacheDays = CType(Request.QueryString("CacheDays"), Integer)
                If cacheDays <= 0 Then
                    cacheDays = 10
                End If
            End If
            Response.Cache.SetExpires(DateTime.Now.AddDays(cacheDays))
            Response.Cache.SetCacheability(HttpCacheability.Public)
            Response.Cache.SetValidUntilExpires(False)
            Dim ext As New System.IO.FileInfo(Server.MapPath(imageUrl))
            Response.AddHeader("content-disposition", "inline; filename=" + ext.Name)
            Select Case ext.Extension.ToLower
                Case ".gif"
                    Response.ContentType = "image/gif"
                Case ".png"
                    Response.ContentType = "image/png"
                Case ".jpg"
                    Response.ContentType = "image/jpg"
                Case ".tif", ".tiff"
                    Response.ContentType = "image/Tif"
                Case ".ico"
                    Response.ContentType = "image/ico"
                Case Else
                    Response.ContentType = "image/jpg"
            End Select
            Response.WriteFile(imageUrl)
        Catch
            Throw
        End Try
    End Sub

End Class
