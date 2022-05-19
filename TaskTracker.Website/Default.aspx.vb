'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 09-08-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-08-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class _Default
    Inherits IP.Bids.BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        IP.Bids.SharedFunctions.ResponseRedirect("~/ViewTasks.aspx?refsite=MTT")
        'HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub
End Class
