'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 12-06-2010
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class PopupsExcel
    Inherits System.Web.UI.Page

    Public Sub WritOutExcelFile()
        'Write the HTML back to the browser

        Dim fileName As String = "TaskTracker" & Now.ToString & ".xlsx"
        Response.ClearHeaders()
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AppendHeader("Content-Disposition", (Convert.ToString("attachment; filename=") & filename) & "")
        Response.Buffer = True
        Dim renderedExcelData As MemoryStream = TryCast(Session("ExcelXML"), MemoryStream)
        Response.BinaryWrite(renderedExcelData.ToArray)
        Response.End()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Dim currentCulture As String = System.Globalization.CultureInfo.CurrentCulture.Name
            If currentCulture.ToLower <> "en-us" Then
                Dim ci As System.Globalization.CultureInfo
                ci = System.Globalization.CultureInfo.GetCultureInfo("en-us")
                System.Threading.Thread.CurrentThread.CurrentCulture = ci
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci

            End If

            WritOutExcelFile()

            'Dim excelDoc As System.IO.StreamWriter
            '' Send output to response stream
            'excelDoc = New System.IO.StreamWriter(Me.Response.OutputStream)
            'Dim excelFile As String = "TaskTracker" & Now.ToString & ".xls"

            'Try
            '    If Session("ExcelXML") IsNot Nothing Then
            '        Response.Clear()
            '        'If excelVersion > 10 Then
            '        Response.AddHeader("Content-Disposition", "Attachment; FileName=" & excelFile)
            '        'End If
            '        Response.BufferOutput = True
            '        'Response.Cache.SetCacheability(HttpCacheability.NoCache)
            '        Response.Charset = ""
            '        Response.ContentType = "application/vnd.ms-excel"
            '        Response.ContentEncoding = System.Text.Encoding.UTF7
            '        Me.EnableViewState = False
            '        excelDoc.Write(Session("ExcelXML"))



            '        'Page.ClientScript.RegisterStartupScript(Page.GetType, "CloseWindow", sb.ToString, False)
            '    Else
            '        Response.Write("<h2>We are missing the Excel Data</h2>")
            '    End If
            'Catch ex As Exception
            '    'Response.Write(ex.Message)
            '    Server.ClearError()

            'Finally
            '    Session.Remove("ExcelXML")
            '    If currentCulture.ToLower <> "en-us" Then
            '        Dim ci As System.Globalization.CultureInfo
            '        ci = System.Globalization.CultureInfo.GetCultureInfo(currentCulture)
            '        System.Threading.Thread.CurrentThread.CurrentCulture = ci
            '        System.Threading.Thread.CurrentThread.CurrentUICulture = ci

            '    End If
            '    excelDoc.Flush()
            '    excelDoc.Close()
            '    Response.End()
            'End Try
        End If
    End Sub
End Class
