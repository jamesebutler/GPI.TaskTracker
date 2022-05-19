'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 10-11-2010
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Option Explicit On
Option Strict On
Partial Class UserControlsNotificationFrequency
    Inherits System.Web.UI.UserControl



#Region "Collections"
    'Private PreviousPeriodDictionary As New StringDictionary
    'Private FuturePeriodDictionary As New StringDictionary
    Private notificationDefaults As New StringDictionary
    Private _notificationProfile As System.Collections.Generic.List(Of BO.NotificationProfile)
#End Region
#Region "Enum"
    Public Enum NotificationPeriod
        FuturePeriod
        PreviousPeriod
        NoPeriod
    End Enum
    Public Enum PreviousPeriod
        Previous7Days
        PreviousCalendarMonth
    End Enum

    Public Enum FuturePeriod
        All
        Next7Days
        Next14Days
        ThisMonth
        ThisQuarter
    End Enum

    'Public Enum EmailTypeValues
    '    OverDue
    '    Complete
    '    Future
    '    Entered
    'End Enum

    Public Enum UserTypeValue
        Creator
        Responsible
        BusinessUnitManager
        TypeManager
    End Enum
#End Region
    Private _AllowOptOut As Boolean ' = False
#Region "Properties"

    Public Property AllowOptOut() As Boolean
        Get
            Return _AllowOptOut
        End Get
        Set(ByVal value As Boolean)
            _AllowOptOut = value
        End Set
    End Property
    Public Property NotificationProfile() As System.Collections.Generic.List(Of BO.NotificationProfile)
        Get
            Return _notificationProfile 'GetSelectedNotificationProfile()
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of BO.NotificationProfile))
            _notificationProfile = value
        End Set
    End Property
    Public Property Enabled() As Boolean
        Get
            Return Me._pnlNotificationFrequency.Enabled
        End Get
        Set(ByVal value As Boolean)
            Me._pnlNotificationFrequency.Enabled = value
        End Set
    End Property


    'Private mEmailType As EmailTypeValues = EmailTypeValues.Complete
    'Public Property EmailType() As EmailTypeValues
    '    Get
    '        Return mEmailType
    '    End Get
    '    Set(ByVal value As EmailTypeValues)
    '        mEmailType = value
    '        SetupNotificationOptions()
    '    End Set
    'End Property

    Private mUserType As UserTypeValue = UserTypeValue.Creator
    Public Property UserType() As UserTypeValue
        Get
            Return mUserType
        End Get
        Set(ByVal value As UserTypeValue)
            mUserType = value
            SetupNotificationOptions()
        End Set
    End Property
    'Private mTaskNotificationPeriod As NotificationPeriod
    'Public Property TaskNotificationPeriod() As NotificationPeriod
    '    Get
    '        Return mTaskNotificationPeriod
    '    End Get
    '    Set(ByVal value As NotificationPeriod)
    '        mTaskNotificationPeriod = value
    '    End Set
    'End Property
    Public Property GroupingText() As String
        Get
            Return Me._pnlNotificationFrequency.GroupingText
        End Get
        Set(ByVal value As String)
            Me._pnlNotificationFrequency.GroupingText = value.Trim
        End Set
    End Property

    Public Property OptOutLabel() As String
        Get
            Return _cbOptOut.Text
        End Get
        Set(ByVal value As String)
            Me._cbOptOut.Text = value
        End Set
    End Property
    Public Property NotificationLabel() As String
        Get
            Return _lblNotificationHeading.Text
        End Get
        Set(ByVal value As String)
            _lblNotificationHeading.Text = value
        End Set
    End Property

    Public Property ImmediateLabel() As String
        Get
            Return _rblImmediate.Text
        End Get
        Set(ByVal value As String)
            _rblImmediate.Text = value
        End Set
    End Property

    Public Property EveryDayLabel() As String
        Get
            Return _rbEveryDay.Text
        End Get
        Set(ByVal value As String)
            _rbEveryDay.Text = value
        End Set
    End Property

    Public Property EveryWeekLabel() As String
        Get
            Return _rbEveryWeek.Text
        End Get
        Set(ByVal value As String)
            _rbEveryWeek.Text = value
        End Set
    End Property

    'Public Property DayOfWeek() As ListItemCollection
    '    Get
    '        Return _ddlDayOfWeek.Items
    '    End Get
    '    Set(ByVal value As ListItemCollection)
    '        _ddlDayOfWeek.Items.Clear()
    '        For Each item As ListItem In value
    '            If _ddlDayOfWeek.Items.FindByValue(item.Value) Is Nothing Then
    '                _ddlDayOfWeek.Items.Add(item)
    '            End If
    '        Next
    '    End Set
    'End Property

    Public Property EveryMonthLabel() As String
        Get
            Return _rblEveryMonth.Text
        End Get
        Set(ByVal value As String)
            _rblEveryMonth.Text = value
        End Set
    End Property

    'Public Property Ordinals() As ListItemCollection
    '    Get
    '        Return _ddlOrdinalMonth.Items
    '    End Get
    '    Set(ByVal value As ListItemCollection)
    '        _ddlOrdinalMonth.Items.Clear()
    '        For Each item As ListItem In value
    '            If _ddlOrdinalMonth.Items.FindByValue(item.Value) Is Nothing Then
    '                _ddlOrdinalMonth.Items.Add(item)
    '            End If
    '        Next
    '    End Set
    'End Property

    Public Property WhenEnteredLabel() As String
        Get
            Return _lblWhenEntered.Text
        End Get
        Set(value As String)
            _lblWhenEntered.Text = value
        End Set
    End Property

    Public Property FutureTimePeriodLabel() As String
        Get
            Return _lblFutureTimePeriodHeader.Text
        End Get
        Set(ByVal value As String)
            _lblFutureTimePeriodHeader.Text = value
        End Set
    End Property

    'Public Property CompleteTimePeriodLabel() As String
    '    Get
    '        Return _lblCompleteTimePeriodHeader.Text
    '    End Get
    '    Set(ByVal value As String)
    '        _lblCompleteTimePeriodHeader.Text = value
    '    End Set
    'End Property

    'Public Property NotificationPeriod() As ListItemCollection
    '    Get
    '        Return _rblNotificationPeriod.Items
    '    End Get
    '    Set(ByVal value As ListItemCollection)
    '        _rblNotificationPeriod.Items.Clear()
    '        For Each item As ListItem In value
    '            If _rblNotificationPeriod.Items.FindByValue(item.Value) Is Nothing Then
    '                _rblNotificationPeriod.Items.Add(item)
    '            End If
    '        Next
    '    End Set
    'End Property


#End Region

#Region "Methods"
    Private Sub DefaultSettings()
        'If GroupingText.Length = 0 Then
        '    GroupingText = IP.Bids.SharedFunctions.LocalizeValue("Notification Frequency", True)
        'End If
        If OptOutLabel.Length = 0 Then
            OptOutLabel = IP.Bids.SharedFunctions.LocalizeValue("Do not send Emails", True)
        End If
        If NotificationLabel.Length = 0 Then
            NotificationLabel = IP.Bids.SharedFunctions.LocalizeValue("Task Notification Frequency", True)
        End If
        'If ImmediateLabel.Length = 0 Then ImmediateLabel = "Immediately"
        If EveryDayLabel.Length = 0 Then
            EveryDayLabel = IP.Bids.SharedFunctions.LocalizeValue("Daily", True)
        End If
        'If _rblDaily.Text.Length = 0 Then _rblDaily.Text = "Daily"
        If EveryWeekLabel.Length = 0 Then
            EveryWeekLabel = IP.Bids.SharedFunctions.LocalizeValue("Weekly on", True)
        End If
        If EveryMonthLabel.Length = 0 Then
            EveryMonthLabel = IP.Bids.SharedFunctions.LocalizeValue("Monthly on day", True)
        End If
        If FutureTimePeriodLabel.Length = 0 Then
            FutureTimePeriodLabel = IP.Bids.SharedFunctions.LocalizeValue("Notification Time Period for Future Tasks", True)
        End If
        'If CompleteTimePeriodLabel.Length = 0 Then CompleteTimePeriodLabel = "Notification Time Period for Completed Tasks"
        If WhenEnteredLabel.Length = 0 Then
            WhenEnteredLabel = IP.Bids.SharedFunctions.LocalizeValue("When tasks are Entered Notify Me", True)
        End If

        'Me._rblDaily.GroupName = "GroupEntered" & ID
        'Me._rblImmediate.GroupName = "GroupEntered" & ID
        Me._rbEveryDay.GroupName = "Group" & ID
        Me._rbEveryWeek.GroupName = "Group" & ID
        Me._rblEveryMonth.GroupName = "Group" & ID
        PopulateImmediateOptions()
        SetupNotificationOptions()


    End Sub

    Private Sub SetupNotificationOptions()
        Select Case UserType
            Case UserTypeValue.BusinessUnitManager
                'Daily,Weekly,Monthly
                SetFrequencyOptions(False, True, True, True, False)
                SetPeriodOptions(True)
            Case UserTypeValue.Creator
                'Daily,Weekly,Monthly
                SetFrequencyOptions(False, True, True, True, True)
                SetPeriodOptions(True)
            Case UserTypeValue.Responsible
                'Daily,Weekly
                SetFrequencyOptions(True, True, True, False, False)
                SetPeriodOptions(True)
            Case UserTypeValue.TypeManager
                'Daily,Weekly,Monthly
                SetFrequencyOptions(False, True, True, True, False)
                SetPeriodOptions(True)
                'Last Week, Last Month
        End Select

    End Sub


    Public Sub SetFrequencyOptions(ByVal allowImmediate As Boolean, ByVal allowDaily As Boolean, ByVal allowWeekly As Boolean, ByVal allowMonthly As Boolean, ByVal allowOptOut As Boolean)
        Me._rblImmediate.Enabled = allowImmediate
        Me._rblImmediate.Visible = allowImmediate
        Me._rbEveryDay.Visible = allowDaily
        Me._lblWhenEntered.Enabled = allowImmediate
        Me._lblWhenEntered.Visible = allowImmediate
        Me._rbEveryWeek.Visible = allowWeekly
        Me._ddlDayOfWeek.Visible = allowWeekly
        Me._rblEveryMonth.Visible = allowMonthly
        Me._ddlOrdinalMonth.Visible = allowMonthly
        Me._cbOptOut.Visible = allowOptOut
        _lblOptOut.Visible = allowOptOut
        _litOptOut.Visible = allowOptOut
        '_cbExcludeComplete.Visible = True 'Not allowImmediate
    End Sub

    Public Sub SetPeriodOptions(ByVal showNotificationDateRange As Boolean)
        'Me._lblCompleteTimePeriodHeader.Visible = True
        If showNotificationDateRange Then
            'With _rblCompleteNotificationPeriod
            '    'set with past periods
            '    .Items.Clear()
            '    .Items.Add(New ListItem("Exclude Completed Tasks", CStr(0)))
            '    .Items.Add(New ListItem("Last 7 Days", CStr(PreviousPeriod.Previous7Days)))
            '    .Items.Add(New ListItem("Last 30 Days", CStr(PreviousPeriod.PreviousCalendarMonth)))
            'End With       
            With _rblFutureNotificationPeriod
                'set with future periods
                .Items.Clear()
                .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue("All", True), "ALL"))
                .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Next 7 Days", True), "NEXT 7 DAYS"))
                .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Next 14 Days", True), "NEXT 14 DAYS"))
                .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Next 30 Days", True), "NEXT 30 DAYS"))
                .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Next 90 Days", True), "NEXT 90 DAYS"))
            End With
        ElseIf showNotificationDateRange = False Then
            With _rblFutureNotificationPeriod
                'set with future periods
                .Items.Clear()
            End With
            Me._lblFutureTimePeriodHeader.Visible = False
            'With _rblCompleteNotificationPeriod
            '    'set with future periods
            '    .Items.Clear()
            'End With
            'Me._lblCompleteTimePeriodHeader.Visible = False
        End If
    End Sub



    Private Sub PopulateDaysOfWeek()
        With _ddlDayOfWeek
            .Items.Clear()
            For i As Integer = 0 To 6
                If .Items.FindByValue(CStr(i + 1)) Is Nothing Then
                    .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue([Enum].GetName(GetType(System.DayOfWeek), i), True), CStr(i + 1)))
                End If
            Next
        End With
    End Sub

    Private Sub PopulateMonthOrdinals()
        With Me._ddlOrdinalMonth
            .Items.Clear()
            For i As Integer = 1 To 31
                If .Items.FindByValue(CStr(i)) Is Nothing Then
                    .Items.Add(New ListItem(CStr(i), CStr(i)))
                End If
            Next
        End With
    End Sub
    Private Sub PopulateImmediateOptions()
        With _rblImmediate
            .Items.Clear()
            .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Immediately", True), "IMMEDIATE"))
            .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Daily", True), "DAILY"))
        End With
    End Sub

    Public Function GetSelectedNotificationProfile() As String 'System.Collections.Generic.List(Of BO.NotificationProfile)
        Dim newProfile As New System.Collections.Generic.List(Of BO.NotificationProfile)
        Dim item As New TaskTrackerItemBll
        Dim rpl As System.Collections.Generic.List(Of BO.RecurringParametersList)
        Dim roleID As Integer = -1
        Dim specificDayProfileTypeSeqID As Integer = -1
        Dim daysOfWeekProfileTypeSeqID As Integer = -1
        Dim recurrencePatternProfileTypeSeqID As Integer = -1
        Dim dateRangeProfileTypeSeqID As Integer = -1
        Dim specificMonthSeqID As Integer = -1

        If NotificationProfile Is Nothing Then
            Return Nothing
        End If

        If item IsNot Nothing Then
            rpl = item.GetRecurringParameterList
            If rpl IsNot Nothing AndAlso rpl.Count > 0 Then
                For i As Integer = 0 To rpl.Count - 1
                    If rpl.Item(i).ProfileTypeName.ToUpper = "SPECIFICDAY" Then
                        specificDayProfileTypeSeqID = rpl.Item(i).ProfileTypeSeqId
                    ElseIf rpl.Item(i).ProfileTypeName.ToUpper = "DAYSOFWEEK" Then
                        daysOfWeekProfileTypeSeqID = rpl.Item(i).ProfileTypeSeqId
                    ElseIf rpl.Item(i).ProfileTypeName.ToUpper = "RECURRENCEPATTERN" Then
                        recurrencePatternProfileTypeSeqID = rpl.Item(i).ProfileTypeSeqId
                    ElseIf rpl.Item(i).ProfileTypeName.ToUpper = "DATERANGE" Then
                        dateRangeProfileTypeSeqID = rpl.Item(i).ProfileTypeSeqId
                    ElseIf rpl.Item(i).ProfileTypeName.ToUpper = "SPECIFICMONTH" Then
                        specificMonthSeqID = rpl.Item(i).ProfileTypeSeqId
                    End If

                Next
            End If
        End If
        roleID = notificationProfile.Item(0).RoleSeqId

        Select Case UserType

            Case UserTypeValue.Responsible
                'Entered
                newProfile.Add(New NotificationProfile("Responsible", roleID, "ENTERED", recurrencePatternProfileTypeSeqID, Me._rblImmediate.SelectedValue, "RecurrencePattern"))

                'FUTURE
                If Me._rbEveryDay.Checked Then
                    newProfile.Add(New NotificationProfile("Responsible", roleID, "FUTURE", recurrencePatternProfileTypeSeqID, "DAILY", "RecurrencePattern"))
                ElseIf Me._rbEveryWeek.Checked Then
                    newProfile.Add(New NotificationProfile("Responsible", roleID, "FUTURE", recurrencePatternProfileTypeSeqID, "WEEKLY", "RecurrencePattern"))
                    'DAY OF WEEK                    
                    newProfile.Add(New NotificationProfile("Responsible", roleID, "FUTURE", daysOfWeekProfileTypeSeqID, Me._ddlDayOfWeek.SelectedValue, "DaysOfWeek"))
                    'ElseIf Me._rblEveryMonth.Checked Then
                    '    newProfile.Add(New NotificationProfile("Responsible", RoleID, "FUTURE", RecurrencePatternProfileTypeSeqID, "MONTHLY", "RecurrencePattern"))
                    '    'SPECIFICDAY
                    '    newProfile.Add(New NotificationProfile("Responsible", RoleID, "FUTURE", SpecificDayProfileTypeSeqID, Me._ddlOrdinalMonth.SelectedValue, "SpecificDay"))
                End If

                'DATERANGE
                newProfile.Add(New NotificationProfile("Responsible", roleID, "FUTURE", dateRangeProfileTypeSeqID, Me._rblFutureNotificationPeriod.SelectedValue, "DateRange"))

            Case UserTypeValue.Creator
                If Me._cbOptOut.Checked = False Then
                    'FUTURE
                    If Me._rbEveryDay.Checked Then
                        newProfile.Add(New NotificationProfile("Creator", roleID, "FUTURE", recurrencePatternProfileTypeSeqID, "DAILY", "RecurrencePattern"))
                    ElseIf Me._rbEveryWeek.Checked Then
                        newProfile.Add(New NotificationProfile("Creator", roleID, "FUTURE", recurrencePatternProfileTypeSeqID, "WEEKLY", "RecurrencePattern"))
                        'DAY OF WEEK                    
                        newProfile.Add(New NotificationProfile("Creator", roleID, "FUTURE", daysOfWeekProfileTypeSeqID, Me._ddlDayOfWeek.SelectedValue, "DaysOfWeek"))
                    ElseIf Me._rblEveryMonth.Checked Then
                        newProfile.Add(New NotificationProfile("Creator", roleID, "FUTURE", recurrencePatternProfileTypeSeqID, "MONTHLY", "RecurrencePattern"))
                        'SPECIFICDAY
                        newProfile.Add(New NotificationProfile("Creator", roleID, "FUTURE", specificDayProfileTypeSeqID, Me._ddlOrdinalMonth.SelectedValue, "SpecificDay"))
                    End If

                    'DATERANGE
                    newProfile.Add(New NotificationProfile("Creator", roleID, "FUTURE", dateRangeProfileTypeSeqID, Me._rblFutureNotificationPeriod.SelectedValue, "DateRange"))
                Else
                    newProfile.Add(New NotificationProfile("Creator", roleID, "FUTURE", dateRangeProfileTypeSeqID, "NO EMAIL", "DateRange"))
                End If
            Case UserTypeValue.BusinessUnitManager
                'FUTURE
                If Me._rbEveryDay.Checked Then
                    newProfile.Add(New NotificationProfile("BusUnitMgr", roleID, "FUTURE", recurrencePatternProfileTypeSeqID, "DAILY", "RecurrencePattern"))
                ElseIf Me._rbEveryWeek.Checked Then
                    newProfile.Add(New NotificationProfile("BusUnitMgr", roleID, "FUTURE", recurrencePatternProfileTypeSeqID, "WEEKLY", "RecurrencePattern"))
                    'DAY OF WEEK                    
                    newProfile.Add(New NotificationProfile("BusUnitMgr", roleID, "FUTURE", daysOfWeekProfileTypeSeqID, Me._ddlDayOfWeek.SelectedValue, "DaysOfWeek"))
                ElseIf Me._rblEveryMonth.Checked Then
                    newProfile.Add(New NotificationProfile("BusUnitMgr", roleID, "FUTURE", recurrencePatternProfileTypeSeqID, "MONTHLY", "RecurrencePattern"))
                    'SPECIFICDAY
                    newProfile.Add(New NotificationProfile("BusUnitMgr", roleID, "FUTURE", specificDayProfileTypeSeqID, Me._ddlOrdinalMonth.SelectedValue, "SpecificDay"))
                End If

                'DATERANGE
                newProfile.Add(New NotificationProfile("BusUnitMgr", roleID, "FUTURE", dateRangeProfileTypeSeqID, Me._rblFutureNotificationPeriod.SelectedValue, "DateRange"))
            Case UserTypeValue.TypeManager
                'FUTURE
                If Me._rbEveryDay.Checked Then
                    newProfile.Add(New NotificationProfile("TypeMgr", roleID, "FUTURE", recurrencePatternProfileTypeSeqID, "DAILY", "RecurrencePattern"))
                ElseIf Me._rbEveryWeek.Checked Then
                    newProfile.Add(New NotificationProfile("TypeMgr", roleID, "FUTURE", recurrencePatternProfileTypeSeqID, "WEEKLY", "RecurrencePattern"))
                    'DAY OF WEEK                    
                    newProfile.Add(New NotificationProfile("TypeMgr", roleID, "FUTURE", daysOfWeekProfileTypeSeqID, Me._ddlDayOfWeek.SelectedValue, "DaysOfWeek"))
                ElseIf Me._rblEveryMonth.Checked Then
                    newProfile.Add(New NotificationProfile("TypeMgr", roleID, "FUTURE", recurrencePatternProfileTypeSeqID, "MONTHLY", "RecurrencePattern"))
                    'SPECIFICDAY
                    newProfile.Add(New NotificationProfile("TypeMgr", roleID, "FUTURE", specificDayProfileTypeSeqID, Me._ddlOrdinalMonth.SelectedValue, "SpecificDay"))
                End If

                'DATERANGE
                newProfile.Add(New NotificationProfile("TypeMgr", roleID, "FUTURE", dateRangeProfileTypeSeqID, Me._rblFutureNotificationPeriod.SelectedValue, "DateRange"))
        End Select

        Dim sbPattern As New StringBuilder
        For i As Integer = 0 To newProfile.Count - 1
            With newProfile.Item(i)
                If sbPattern.Length > 0 Then
                    sbPattern.Append(",")
                End If
                sbPattern.Append(.EmailType & "|" & .ProfileTypeSeqId & "|" & .ProfileTypeValue & "|" & .RoleSeqId)
            End With
        Next
        Return sbPattern.ToString
    End Function
    Public Sub Refresh()
        If Page.IsPostBack = False Then
            PopulateDaysOfWeek()
            PopulateMonthOrdinals()
            'DefaultSettings()
        End If
        SetDefaultValues()
    End Sub
    Private Sub SetDefaultValues()
        Try

            Me.Enabled = True
            'JEB
            'If NotificationProfile Is Nothing OrElse NotificationProfile.Count = 0 Then
            '    Me.Enabled = False
            '    Exit Sub
            'Else
            '    Me.Enabled = True
            'End If
            Me._rblImmediate.ClearSelection()
            Me._ddlDayOfWeek.ClearSelection()
            Me._rblFutureNotificationPeriod.ClearSelection()
            Select Case UserType
                Case UserTypeValue.Responsible
                    notificationDefaults.Clear()
                    'Set defaults

                    Me._rblImmediate.Items(0).Selected = True
                    Me._rbEveryWeek.Checked = True
                    'Dim itemDayOfWeek As ListItem = _ddlDayOfWeek.Items.FindByValue(CStr(5))
                    'If itemDayOfWeek IsNot Nothing Then
                    '    _ddlDayOfWeek.SelectedIndex = _ddlDayOfWeek.Items.IndexOf(itemDayOfWeek) 'Me._ddlDayOfWeek.Items(5).Selected = True
                    'End If
                    Me._rblFutureNotificationPeriod.Items(3).Selected = True

                    If NotificationProfile IsNot Nothing Then
                        For i As Integer = 0 To Me.NotificationProfile.Count - 1
                            If Me.NotificationProfile.Item(i).RoleName.ToUpper = "RESPONSIBLE" Then
                                Select Case Me.NotificationProfile.Item(i).ProfileTypeName.ToUpper
                                    Case "RECURRENCEPATTERN"
                                        If Me.NotificationProfile.Item(i).EmailType.ToUpper = "ENTERED" Then
                                            Select Case Me.NotificationProfile.Item(i).ProfileTypeValue.ToUpper
                                                Case "IMMEDIATE" 'ENTERED ONLY
                                                    Me._rblImmediate.ClearSelection()
                                                    Me._rblImmediate.Items(0).Selected = True
                                                Case "DAILY" 'ENTERED ONLY
                                                    Me._rblImmediate.ClearSelection()
                                                    Me._rblImmediate.Items(1).Selected = True
                                            End Select
                                        ElseIf Me.NotificationProfile.Item(i).EmailType.ToUpper = "FUTURE" Then
                                            Select Case Me.NotificationProfile.Item(i).ProfileTypeValue.ToUpper
                                                Case "DAILY" 'FUTURE
                                                    Me._rbEveryDay.Checked = True
                                                    Me._rbEveryWeek.Checked = False
                                                    Me._rblEveryMonth.Checked = False
                                                Case "WEEKLY" 'FUTURE
                                                    Me._rbEveryDay.Checked = False
                                                    Me._rbEveryWeek.Checked = True
                                                    Me._rblEveryMonth.Checked = False
                                                Case "MONTHLY" 'FUTURE
                                                    Me._rbEveryDay.Checked = False
                                                    Me._rbEveryWeek.Checked = False
                                                    Me._rblEveryMonth.Checked = True
                                            End Select
                                        End If
                                    Case "DAYSOFWEEK"
                                        If _ddlDayOfWeek.Items.FindByValue(Me.NotificationProfile.Item(i).ProfileTypeValue) IsNot Nothing Then
                                            _ddlDayOfWeek.ClearSelection()
                                            _ddlDayOfWeek.Items.FindByValue(Me.NotificationProfile.Item(i).ProfileTypeValue).Selected = True
                                        End If
                                    Case "DATERANGE"
                                        If Me._rblFutureNotificationPeriod.Items.FindByValue(Me.NotificationProfile.Item(i).ProfileTypeValue) IsNot Nothing Then
                                            Me._rblFutureNotificationPeriod.ClearSelection()
                                            Me._rblFutureNotificationPeriod.Items.FindByValue(Me.NotificationProfile.Item(i).ProfileTypeValue).Selected = True
                                        End If
                                End Select
                            End If
                        Next
                    End If
                Case UserTypeValue.Creator, UserTypeValue.TypeManager, UserTypeValue.BusinessUnitManager
                    If UserType = UserTypeValue.Creator Then
                        _rbEveryWeek.Checked = True
                        'Dim itemDayOfWeek As ListItem = _ddlDayOfWeek.Items.FindByValue(CStr(1))
                        'If itemDayOfWeek IsNot Nothing Then
                        '    _ddlDayOfWeek.SelectedIndex = _ddlDayOfWeek.Items.IndexOf(itemDayOfWeek) 'Me._ddlDayOfWeek.Items(5).Selected = True
                        'End If
                        _rblFutureNotificationPeriod.Items(3).Selected = True
                        Me._cbOptOut.Checked = False
                    ElseIf UserType = UserTypeValue.BusinessUnitManager Then
                        'Me._rblImmediate.Items(0).Selected = True
                        _rblEveryMonth.Checked = True
                        _ddlOrdinalMonth.ClearSelection()
                        _ddlOrdinalMonth.Items(4).Selected = True
                        _rblFutureNotificationPeriod.Items(3).Selected = True
                    ElseIf UserType = UserTypeValue.TypeManager Then
                        _rblEveryMonth.Checked = True
                        _ddlOrdinalMonth.ClearSelection()
                        _ddlOrdinalMonth.Items(4).Selected = True
                        _rblFutureNotificationPeriod.Items(3).Selected = True
                    End If

                    If NotificationProfile IsNot Nothing Then
                        For i As Integer = 0 To Me.NotificationProfile.Count - 1
                            'If Me.NotificationProfile.Item(i).RoleName.ToUpper = "CREATOR" Then
                            Select Case Me.NotificationProfile.Item(i).ProfileTypeName.ToUpper
                                Case "RECURRENCEPATTERN"
                                    If Me.NotificationProfile.Item(i).EmailType.ToUpper = "FUTURE" Then
                                        Select Case Me.NotificationProfile.Item(i).ProfileTypeValue.ToUpper
                                            Case "DAILY" 'FUTURE
                                                Me._rbEveryDay.Checked = True
                                                _rbEveryWeek.Checked = False
                                                _rblEveryMonth.Checked = False
                                            Case "WEEKLY" 'FUTURE                                                   
                                                Me._rbEveryDay.Checked = False
                                                Me._rbEveryWeek.Checked = True
                                                _rblEveryMonth.Checked = False
                                            Case "MONTHLY" 'FUTURE
                                                Me._rbEveryDay.Checked = False
                                                Me._rbEveryWeek.Checked = True
                                                _rblEveryMonth.Checked = True
                                        End Select
                                    End If
                                Case "DAYSOFWEEK"
                                    If _ddlDayOfWeek.Items.FindByValue(Me.NotificationProfile.Item(i).ProfileTypeValue) IsNot Nothing Then
                                        _ddlDayOfWeek.ClearSelection()
                                        _ddlDayOfWeek.Items.FindByValue(Me.NotificationProfile.Item(i).ProfileTypeValue).Selected = True
                                    End If
                                Case "DATERANGE"
                                    If Me._rblFutureNotificationPeriod.Items.FindByValue(Me.NotificationProfile.Item(i).ProfileTypeValue) IsNot Nothing Then
                                        Me._rblFutureNotificationPeriod.ClearSelection()
                                        Me._rblFutureNotificationPeriod.Items.FindByValue(Me.NotificationProfile.Item(i).ProfileTypeValue).Selected = True
                                    ElseIf Me.NotificationProfile.Item(i).ProfileTypeValue.ToUpper = "NO EMAIL" Then
                                        Me._cbOptOut.Checked = True
                                    End If
                                Case "SPECIFICDAY"
                                    If _ddlOrdinalMonth.Items.FindByValue(Me.NotificationProfile.Item(i).ProfileTypeValue) IsNot Nothing Then
                                        Me._ddlOrdinalMonth.ClearSelection()
                                        Me._ddlOrdinalMonth.Items.FindByValue(Me.NotificationProfile.Item(i).ProfileTypeValue).Selected = True
                                    End If
                            End Select
                            ' End If
                        Next
                    End If

            End Select
        Catch ex As Exception
            Dim x As String = ex.Message ' COMMENTED BY CODEIT.RIGHT
        End Try
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then

            PopulateDaysOfWeek()
            PopulateMonthOrdinals()
            DefaultSettings()
            SetDefaultValues()
        End If
    End Sub

    'Public Overrides Sub DataBind()
    '    SetupNotificationOptions()
    'End Sub

#End Region

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Page.IsPostBack = True Then
            'SetDefaultValues()
        End If
    End Sub
End Class
