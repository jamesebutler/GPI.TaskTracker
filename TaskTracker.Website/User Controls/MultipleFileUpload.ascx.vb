'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 09-10-2010
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class UserControlsMultipleFileUpload
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.ClientScript.RegisterClientScriptInclude("MultipleFileUpload", "Scripts/MultipleFileUpload.js")
    End Sub

    Protected Sub _btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnUpload.Click
        Dim uploads As HttpFileCollection
        uploads = HttpContext.Current.Request.Files

        For i As Integer = 0 To (uploads.Count - 1)

            If (uploads(i).ContentLength > 0) Then
                Dim c As String = System.IO.Path.GetFileName(uploads(i).FileName)

                Try
                    uploads(i).SaveAs("C:\UploadedUserFiles\" + c)
                    'Span1.InnerHtml = "File Uploaded Sucessfully."
                Catch exp As Exception
                    'Span1.InnerHtml = "Some Error occured."
                End Try

            End If

        Next i

    End Sub
End Class
