'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 09-23-2010
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class PopupsAttachments
    Inherits IP.Bids.BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        IP.Bids.SharedFunctions.DisablePageCache(Response)
        If Request.QueryString("HeaderNumber") IsNot Nothing Then
            Me._fa.TaskHeaderNumber = Request.QueryString("HeaderNumber")
        End If
        If Request.QueryString("TaskNumber") IsNot Nothing Then
            Me._fa.TaskItemNumber = Request.QueryString("TaskNumber")
        End If

        'Dim uploadsUrl As String = ConfigurationManager.AppSettings.Item("DevelopmentServer")
        'Dim uploadsFolder As String = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")

        'If HttpContext.Current.Request.UserHostAddress = "127.0.0.1" Then
        '    uploadsUrl = ConfigurationManager.AppSettings.Item("DevelopmentUploadsUrl")
        '    uploadsFolder = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")
        'ElseIf HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("ridev") Then
        '    uploadsUrl = ConfigurationManager.AppSettings.Item("DevelopmentUploadsUrl")
        '    uploadsFolder = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")
        'ElseIf HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("ritest") Then
        '    uploadsUrl = ConfigurationManager.AppSettings.Item("DevelopmentUploadsUrl")
        '    uploadsFolder = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")
        'Else
        '    uploadsUrl = ConfigurationManager.AppSettings.Item("ProductionUploadsUrl")
        '    uploadsFolder = ConfigurationManager.AppSettings.Item("ProductionUploadsFolder")
        'End If

        '_fa.UploadsUrl = uploadsUrl
        '_fa.SaveAsPath = uploadsFolder

    End Sub
End Class
