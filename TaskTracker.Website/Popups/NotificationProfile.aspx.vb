'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 04-27-2011
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class PopupsNotificationProfile
    Inherits IP.Bids.BasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandlePageLoad()
    End Sub

    Private Sub HandlePageLoad()
        Dim userName As String = IP.Bids.SharedFunctions.GetCurrentUser.Username
        Dim record As System.Collections.Generic.List(Of BO.NotificationProfile) = Nothing
        If Not Page.IsPostBack Then
            _NotificationUser.DefaultUserName = userName

            record = UserDataBll.GetNotificationProfileList(userName)

            If record IsNot Nothing Then
                Me._notificationBusinessManager.notificationProfile = record
                Me._notificationResponsible.notificationProfile = record
                Me._notificationTaskCreator.notificationProfile = record
                Me._notificationTypeManager.notificationProfile = record
            End If
            '        With Me._notificationBusinessManager
            '.
            '        End With
        End If
    End Sub
End Class
