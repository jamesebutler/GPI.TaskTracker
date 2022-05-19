'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : mjpope
' Created          : 12-30-2010
'
' Last Modified By : mjpope
' Last Modified On : 12-30-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class AdminDefault
    Inherits System.Web.UI.Page

#Region "Private Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        IP.Bids.SharedFunctions.ResponseRedirect("~/Admin/CacheViewer.aspx")
    End Sub
#End Region

End Class
