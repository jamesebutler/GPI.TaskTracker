<%@ Application Language="VB" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.IO.Compression" %>

<script RunAt="server">

    Sub Application_OnStart(ByVal sender As Object, ByVal e As EventArgs)

    End Sub


    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)

        ' Prepare an error report.
        'Response.Clear()
        'Response.Write("<H1>An exception has occurred:</H1>")
        '' Display information on the page being processed.
        'Response.Write("<b>URL = </b>" + Request.Path + "<br />")
        'Response.Write("<b>QueryString = </b>" + Request.QueryString.ToString() + "<p>")
        'Response.Write("<b>Error details</b><p>")

        ' Display information on the (real) error that occurred.
        Dim ex As Exception

        if HttpContext.Current.Server  isnot nothing then
            if HttpContext.Current.Server.GetLastError isnot nothing then
                ex =  HttpContext.Current.Server.GetLastError.InnerException
                if ex isnot nothing then
                    Dim errMsg As String = HttpContext.Current.Server.HtmlEncode(ex.ToString)
                    errMsg = errMsg.Replace(ControlChars.CrLf, "<BR />")
                    Response.Write(errMsg)
                    IP.Bids.SharedFunctions.HandleError("MTT Application Error", errMsg, ex)
                    Response.End()
                end if
            end if
        end if
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        Session.Timeout = 60 '480 '120       
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.


    End Sub

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        ' Code that runs on application startup
        Dim ServiceName As String = "Database ({0})"
        ServiceName = String.Format(ServiceName, IP.Bids.SharedFunctions.GetDatabaseName())
        Application.Add("ServiceName", ServiceName)
        Application("OfflineMessage") = "This website is offline."
        Application("IsOffline") = False

        If ServiceName.Contains("GPCIOD02") Then
            Application.Add("TestDatabase", "YES")
        Else
            Application.Add("TestDatabase", "NO")
        End If




    End Sub



</script>

