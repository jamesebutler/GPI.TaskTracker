'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 06-21-2011
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class HelpDefault
    Inherits IP.Bids.BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Server.Transfer("~/Help/UsingMyHelp.aspx")
        IP.Bids.SharedFunctions.ResponseRedirect("~/Help/UsingMyHelp.aspx")
    End Sub
End Class
