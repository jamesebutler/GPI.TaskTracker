'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 05-10-2011
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class PopupsWord
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Dim currentCulture As String = System.Globalization.CultureInfo.CurrentCulture.Name
            If currentCulture.ToLower <> "en-us" Then
                Dim ci As System.Globalization.CultureInfo
                ci = System.Globalization.CultureInfo.GetCultureInfo("en-us")
                System.Threading.Thread.CurrentThread.CurrentCulture = ci
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci

            End If
            Dim wordDoc As System.IO.StreamWriter
            ' Send output to response stream
            wordDoc = New System.IO.StreamWriter(Me.Response.OutputStream)
            Dim wordFile As String = "TaskTracker" & Now.ToString & ".doc"

            Try
                If Session("WordXML") IsNot Nothing Then
                    Response.Clear()
                    'If excelVersion > 10 Then
                    Response.AddHeader("Content-Disposition", "Attachment; FileName=" & wordFile)
                    'End If
                    Response.BufferOutput = True
                    'Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Response.Charset = ""
                    Response.ContentType = "application/vnd.ms-word"
                    Response.ContentEncoding = System.Text.Encoding.UTF7
                    Me.EnableViewState = False
                    wordDoc.Write(Session("WordXML"))



                    'Page.ClientScript.RegisterStartupScript(Page.GetType, "CloseWindow", sb.ToString, False)
                Else
                    Response.Write("<h2>We are missing the Calendar Data</h2>")
                End If
            Catch ex As Exception
                'Response.Write(ex.Message)
                Server.ClearError()

            Finally
                Session.Remove("WordXML")
                If currentCulture.ToLower <> "en-us" Then
                    Dim ci As System.Globalization.CultureInfo
                    ci = System.Globalization.CultureInfo.GetCultureInfo(currentCulture)
                    System.Threading.Thread.CurrentThread.CurrentCulture = ci
                    System.Threading.Thread.CurrentThread.CurrentUICulture = ci

                End If
                wordDoc.Flush()
                wordDoc.Close()
                Response.End()
            End Try
        End If
    End Sub

End Class
