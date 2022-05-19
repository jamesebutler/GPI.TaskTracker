'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : mjpope
' Created          : 10-05-2010
'
' Last Modified By : mjpope
' Last Modified On : 06-27-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class UserProfileNotifications
    Inherits IP.Bids.BasePage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Master.PageName = Master.GetLocalizedValue("Data Maintenance Notifications", False)
        _lblHeaderTitle.Text = Master.PageName
        Master.SetActiveNav(Enums.NavPages.DataMaintenance.ToString)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub UpdateNotificationSelections(ByVal userName As String)
        Dim record As System.Collections.Generic.List(Of BO.NotificationProfile) = Nothing
        record = UserDataBLL.GetNotificationProfileList(userName)

        If record IsNot Nothing Then
            Dim responsibleRecord As New System.Collections.Generic.List(Of BO.NotificationProfile)
            Dim creatorRecord As New System.Collections.Generic.List(Of BO.NotificationProfile)
            Dim businessManagerRecord As New System.Collections.Generic.List(Of BO.NotificationProfile)
            'Dim typeManagerRecord As New System.Collections.Generic.List(Of BO.NotificationProfile)

            For i As Integer = 0 To record.Count - 1
                Select Case record.Item(i).RoleName.ToUpper
                    Case "RESPONSIBLE"
                        responsibleRecord.Add(record.Item(i))
                    Case "CREATOR"
                        creatorRecord.Add(record.Item(i))
                    Case "BUSUNITMGR", "TYPEMGR"
                        businessManagerRecord.Add(record.Item(i))
                        '_notificationBusinessManager.UserType = UserControlsNotificationFrequency.UserTypeValue.BusinessUnitManager
                    Case "TYPEMGR"
                        'businessManagerRecord.Add(record.Item(i))
                        '_notificationBusinessManager.UserType = UserControlsNotificationFrequency.UserTypeValue.TypeManager
                End Select
            Next
            Me._notificationBusinessManager.NotificationProfile = businessManagerRecord
            Me._notificationResponsible.NotificationProfile = responsibleRecord
            Me._notificationTaskCreator.NotificationProfile = creatorRecord
            'Me._notificationTypeManager.NotificationProfile = typeManagerRecord

            If Page.IsPostBack = False Then
                'Only refresh the data the first time
                Me._notificationBusinessManager.Refresh()
                Me._notificationResponsible.Refresh()
                Me._notificationTaskCreator.Refresh()
                'Me._notificationTypeManager.Refresh()
            End If
        End If
    End Sub
    Private Sub HandlePageLoad()
        Dim userName As String
        Dim userPlantCode As String = ""

        userName = IP.Bids.SharedFunctions.GetCurrentUser.Username
        userPlantCode = IP.Bids.SharedFunctions.GetCurrentUser.PlantCode

        If Page.IsPostBack Then
            'userName = _NotificationUser.SelectedValue.DefaultIfEmpty(userName)
            If _NotificationUser.SelectedValue.Length > 0 Then userName = _NotificationUser.SelectedValue
        End If
        'If Not Page.IsPostBack Then
        '_NotificationUser.AllowUserRoles = False
        _NotificationUser.UserMode = User_Controls.AdvancedEmployeeListDropdown.UserModes.UsersOnly ' UserControlsEmployeeList2.UserModes.UsersOnly
        Dim myRoles As System.Collections.Generic.List(Of UserRoles) = GeneralTaskTrackerBll.GetUserRoles(IP.Bids.SharedFunctions.GetCurrentUser.Username)
        If myRoles IsNot Nothing AndAlso myRoles.Count > 0 Then
            _NotificationUser.Enabled = False
            For Each role As UserRoles In myRoles '            if myroles.Item(1).RoleName  
                If role.RoleName.ToUpper = "SUPPORT" Or role.RoleName.ToUpper = "FACILITYADMIN" Then
                    _NotificationUser.Enabled = True
                End If
            Next
        Else
            _NotificationUser.Enabled = False
        End If

        'Me._notificationTypeManager.Visible = False
        _NotificationUser.AutoPostBack = True
        _NotificationUser.PlantCode = userPlantCode
        _NotificationUser.DefaultUserName = userName
        _NotificationUser.PopulateEmployeeList()
        _NotificationUser.SelectedValue = userName
        'If Not Page.IsPostBack Then
        UpdateNotificationSelections(userName)
        'End If

        '        With Me._notificationBusinessManager
        '.
        '        End With
        'End If
    End Sub

    Protected Sub _btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSave.Click
        Dim pattern As New StringBuilder
        Dim value As String = String.Empty
        If Me._notificationBusinessManager.Enabled Then
            value = Me._notificationBusinessManager.GetSelectedNotificationProfile
            If value IsNot Nothing Then
                If value.Length > 0 Then
                    If pattern.Length > 0 Then
                        pattern.Append(",")
                    End If
                    pattern.Append(value)
                End If
            End If
        End If
        'If Me._notificationTypeManager.Enabled Then
        '    value = Me._notificationTypeManager.GetSelectedNotificationProfile
        '    If value IsNot Nothing Then
        '        If value.Length > 0 Then
        '            If pattern.Length > 0 Then
        '                pattern.Append(",")
        '            End If
        '            pattern.Append(value)
        '        End If
        '    End If
        'End If
        If Me._notificationTaskCreator.Enabled Then
            value = Me._notificationTaskCreator.GetSelectedNotificationProfile
            If value IsNot Nothing Then
                If value.Length > 0 Then
                    If pattern.Length > 0 Then
                        pattern.Append(",")
                    End If
                    pattern.Append(value)
                End If
            End If
        End If
        If Me._notificationResponsible.Enabled Then
            value = Me._notificationResponsible.GetSelectedNotificationProfile
            If value IsNot Nothing Then
                If value.Length > 0 Then
                    If pattern.Length > 0 Then
                        pattern.Append(",")
                    End If
                    pattern.Append(value)
                End If
            End If
        End If
        If pattern.Length > 0 Then
            UserDataBll.SaveNotificationProfile(Me._NotificationUser.DefaultUserName, pattern.ToString, IP.Bids.SharedFunctions.GetCurrentUser.Username)
        End If
    End Sub

    Protected Sub _NotificationUser_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles _NotificationUser.Load
        HandlePageLoad()
    End Sub

    Protected Sub _NotificationUser_UserChanged() Handles _NotificationUser.UserChanged
        Me._notificationBusinessManager.Refresh()
        Me._notificationResponsible.Refresh()
        Me._notificationTaskCreator.Refresh()
        'Me._notificationTypeManager.Refresh()
    End Sub

    Protected Sub _btnDefaultSettings_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnDefaultSettings.Click
        UpdateNotificationSelections("DEFAULT")
        Me._notificationBusinessManager.Refresh()
        Me._notificationResponsible.Refresh()
        Me._notificationTaskCreator.Refresh()
        'Me._notificationTypeManager.Refresh()
    End Sub
End Class
