'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 04-26-2011
'
' Last Modified By : MJPOPE
' Last Modified On : 04-26-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class MissingAccountInfo
    Inherits IP.Bids.BasePage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Master.PageName = "Unauthorized Account"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load       
        Dim mCurrentUserInfo As IP.Bids.UserInfo = IP.Bids.SharedFunctions.GetCurrentUser
        If mCurrentUserInfo IsNot Nothing Then
            _missingAccount.InnerHtml = "Warning! Your account [" & mCurrentUserInfo.Domain & "\" & mCurrentUserInfo.Username & "] is not authorized to use this website.  Please login with your [" & mCurrentUserInfo.Domain & "] credentials." '"Missing Account Information For: " & mCurrentUserInfo.Name & " [" & mCurrentUserInfo.Username & "] is currently InActive.  "
        End If
        IP.Bids.SharedFunctions.InsertAuditRecord("Unauthorized Account", _missingAccount.InnerHtml)
    End Sub
End Class
