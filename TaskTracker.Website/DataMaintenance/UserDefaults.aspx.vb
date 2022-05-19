Option Strict On
Imports System.Linq

Partial Class DataMaintenance_UserDefaults
    Inherits IP.Bids.BasePage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        HandlePageLoad()
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Me.UseBootStrap = False
        Master.PageName = Master.GetLocalizedValue("Data Maintenance User Defaults", False)
        _lblHeaderTitle.Text = Master.PageName
        Master.SetActiveNav(Enums.NavPages.DataMaintenance.ToString)
    End Sub

    Private Sub HandlePageLoad()
        _CurrentUser.UserMode = User_Controls.AdvancedEmployeeListDropdown.UserModes.UsersOnly
        _CurrentUser.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
        _CurrentUser.PopulateEmployeeList()
        _CurrentUser.Enabled = False

        If Not Page.IsPostBack Then
            PopulateApplications()
            DisplaySelectedApplications()
        End If
        DisableUnavailableApplications()
    End Sub

    Private Sub PopulateApplications()
        With _cblApplications
            .Items.Clear()
            .RepeatDirection = RepeatDirection.Horizontal
            .RepeatLayout = RepeatLayout.Table
            .RepeatColumns = 4
            .DataSource = GetApplications()
            .DataTextField = "Value"
            .DataValueField = "Key"
            .DataBind()
        End With
       
    End Sub

    'Private Sub SetSelectedApplications()
    '    If _cblApplications.Items.FindByValue(RIUserDefaults.Applications.AvailabilityTracking.ToString) IsNot Nothing Then
    '        _cblApplications.Items.FindByValue(RIUserDefaults.Applications.AvailabilityTracking.ToString).Selected = True
    '    End If
    'End Sub
    Private Sub ReselectSelectedApplications()
        _cblApplications.ClearSelection()
        DisplaySelectedApplications()
    End Sub

    'Private Sub SetApplicationsForCurrentUser()
    '    Dim defaults As New RIUserDefaults.CurrentUserDefaults(_CurrentUser.SelectedValue, ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
    '    Dim query = From fruit In defaults.UserDefaults.AsQueryable _
    '                        Select fruit.Application.Distinct

    '    For Each item In query.

    '    Next
    '    For Each item As ListItem In _cblApplications.Items
    '        If query.Conta Then
    '    Next
    'End Sub

    Private Sub DisplaySelectedApplications()
        _pnlAvailability.Visible = False
        _pnlMTT.Visible = False
        _pnlTanks.Visible = False

        If _cblApplications.Items.FindByValue(RIUserDefaults.Applications.MTT.ToString) IsNot Nothing Then
            If _cblApplications.Items.FindByValue(RIUserDefaults.Applications.MTT.ToString).Selected = True Then ConfigureTaskTracker()
        End If

        If _cblApplications.Items.FindByValue(RIUserDefaults.Applications.Availability.ToString) IsNot Nothing Then
            If _cblApplications.Items.FindByValue(RIUserDefaults.Applications.Availability.ToString).Selected = True Then ConfigureAvailabilityTracking()
        End If

        If _cblApplications.Items.FindByValue(RIUserDefaults.Applications.Tanks.ToString) IsNot Nothing Then
            If _cblApplications.Items.FindByValue(RIUserDefaults.Applications.Tanks.ToString).Selected = True Then ConfigureTanks()
        End If


    End Sub

    Private Sub DisableUnavailableApplications()
        Dim applicationsToEnable As New List(Of String)
        applicationsToEnable.Add(RIUserDefaults.Applications.Availability.ToString)
        applicationsToEnable.Add(RIUserDefaults.Applications.MTT.ToString)
        applicationsToEnable.Add(RIUserDefaults.Applications.Tanks.ToString)
        applicationsToEnable.Add(RIUserDefaults.Applications.All.ToString)

        For Each item As ListItem In _cblApplications.Items
            item.Enabled = False
            If applicationsToEnable.Contains(item.Value) Then item.Enabled = True

            'If item.Value.Equals(RIUserDefaults.Applications.Availability.ToString, StringComparison.InvariantCultureIgnoreCase) Then item.Enabled = True
            'If item.Value.Equals("ALL", StringComparison.InvariantCultureIgnoreCase) Then item.Enabled = True
            'If item.Value.Equals(RIUserDefaults.Applications.MTT.ToString, StringComparison.InvariantCultureIgnoreCase) Then item.Enabled = True
            'If item.Value.Equals(RIUserDefaults.Applications.Tanks.ToString, StringComparison.InvariantCultureIgnoreCase) Then item.Enabled = True
        Next
    End Sub

    Private Function GetApplications() As SortedDictionary(Of String, String)
        Dim applicationList As New SortedDictionary(Of String, String)
        With applicationList
            .Add(RIUserDefaults.Applications.Availability.ToString, IP.Bids.SharedFunctions.LocalizeValue("Availability Tracking"))
            .Add(RIUserDefaults.Applications.MTT.ToString, IP.Bids.SharedFunctions.LocalizeValue("Task Tracker"))
            .Add(RIUserDefaults.Applications.MOC.ToString, IP.Bids.SharedFunctions.LocalizeValue("Management of Change"))
            .Add(RIUserDefaults.Applications.RI.ToString, IP.Bids.SharedFunctions.LocalizeValue("Reliability"))
            .Add(RIUserDefaults.Applications.Outage.ToString, IP.Bids.SharedFunctions.LocalizeValue("Outage"))
            .Add(RIUserDefaults.Applications.Tanks.ToString, IP.Bids.SharedFunctions.LocalizeValue("Tanks Plus"))
        End With

        Return applicationList
    End Function

    Private Sub ConfigureTanks()
        Dim defaults As New RIUserDefaults.CurrentUserDefaults(_CurrentUser.SelectedValue, RIUserDefaults.Applications.Tanks, ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
        'Set page defaults
        _pnlTanks.Visible = True
        'If Page.IsPostBack Then Exit Sub
        If defaults IsNot Nothing AndAlso defaults.UserDefaults.Count > 0 Then
            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.BusinessUnit) Then
                _siteTanks.DefaultBusinessUnit = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.BusinessUnit)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Area) Then
                _siteTanks.DefaultArea = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Area)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Business) Then
                _siteTanks.DefaultDivision = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Business)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Region) Then
                _siteTanks.DefaultRegion = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Region)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.PlantCode) Then
                _siteTanks.DefaultFacility = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Machine) Then
                _siteTanks.DefaultLineBreak = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Machine)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Line) Then
                _siteTanks.DefaultLine = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Line)
            End If
        End If
    End Sub

    Private Sub ConfigureTaskTracker()
        Dim defaults As New RIUserDefaults.CurrentUserDefaults(_CurrentUser.SelectedValue, RIUserDefaults.Applications.MTT, ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
        'Set page defaults
        _pnlMTT.Visible = True
        'If Page.IsPostBack Then Exit Sub
        If defaults IsNot Nothing AndAlso defaults.UserDefaults.Count > 0 Then
            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.BusinessUnit) Then
                _siteTaskTracker.DefaultBusinessUnit = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.BusinessUnit)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Area) Then
                _siteTaskTracker.DefaultArea = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Area)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Business) Then
                _siteTaskTracker.DefaultDivision = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Business)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Region) Then
                _siteTaskTracker.DefaultRegion = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Region)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.PlantCode) Then
                _siteTaskTracker.DefaultFacility = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Machine) Then
                _siteTaskTracker.DefaultLineBreak = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Machine)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Line) Then
                _siteTaskTracker.DefaultLine = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Line)
            End If
        End If
       
    End Sub
    Private Sub ConfigureAvailabilityTracking()
        Dim defaults As New RIUserDefaults.CurrentUserDefaults(_CurrentUser.SelectedValue, RIUserDefaults.Applications.Availability, ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)

        _pnlAvailability.Visible = True
        'If Page.IsPostBack Then Exit Sub
        'Set page defaults       
        If defaults IsNot Nothing AndAlso defaults.UserDefaults.Count > 0 Then
            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.BusinessUnit) Then
                _siteAvailabilityTracking.DefaultBusinessUnit = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.BusinessUnit)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Area) Then
                _siteAvailabilityTracking.DefaultArea = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Area)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Business) Then
                _siteAvailabilityTracking.DefaultDivision = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Business)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Region) Then
                _siteAvailabilityTracking.DefaultRegion = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Region)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.PlantCode) Then
                _siteAvailabilityTracking.DefaultFacility = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Machine) Then
                _siteAvailabilityTracking.DefaultLineBreak = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Machine)
            End If

            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Line) Then
                _siteAvailabilityTracking.DefaultLine = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Line)
            End If
        End If
        With _siteAvailabilityTracking
            .DisableRegion = True
            .DisableDivision = True
            .Refresh()
        End With
    End Sub

    Protected Sub _btnSave_Click(sender As Object, e As EventArgs) Handles _btnSave.Click
        SaveUserDefaults()
    End Sub

    Private Sub SaveUserDefaults()
        'For Each item As ListItem In _cblApplications.SelectedValue
        Select Case _cblApplications.SelectedValue
                Case RIUserDefaults.Applications.Availability.ToString
                    SaveAvailabilityTracker()
                Case RIUserDefaults.Applications.MTT.ToString
                    SaveTaskTracker()
                Case RIUserDefaults.Applications.Tanks.ToString
                    SaveTanks()
            End Select
        'Next
    End Sub

    Private Sub SaveTaskTracker()
        Dim userName As String = _CurrentUser.SelectedValue
        Dim defaults As New RIUserDefaults.CurrentUserDefaults(userName, RIUserDefaults.Applications.MTT, ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
        With _siteTaskTracker
            'If .RegionValue.Equals("ALL", StringComparison.InvariantCultureIgnoreCase) = False Then
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Region, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Region.ToString, .RegionValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Business, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Business.ToString, .DivisionValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.PlantCode, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.PlantCode.ToString, .FacilityValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.BusinessUnit, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.BusinessUnit.ToString, .BusinessUnitValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Area, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Area.ToString, .AreaValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Line, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Line.ToString, .LineValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Machine, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Machine.ToString, .LineBreakValue))
            'End If
        End With
    End Sub

    Private Sub SaveTanks()
        Dim userName As String = _CurrentUser.SelectedValue
        Dim defaults As New RIUserDefaults.CurrentUserDefaults(userName, RIUserDefaults.Applications.Tanks, ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
        With _siteTanks
            'If .RegionValue.Equals("ALL", StringComparison.InvariantCultureIgnoreCase) = False Then
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Region, RIUserDefaults.Applications.Tanks.ToString, RIUserDefaults.UserProfileTypes.Region.ToString, .RegionValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Business, RIUserDefaults.Applications.Tanks.ToString, RIUserDefaults.UserProfileTypes.Business.ToString, .DivisionValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.PlantCode, RIUserDefaults.Applications.Tanks.ToString, RIUserDefaults.UserProfileTypes.PlantCode.ToString, .FacilityValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.BusinessUnit, RIUserDefaults.Applications.Tanks.ToString, RIUserDefaults.UserProfileTypes.BusinessUnit.ToString, .BusinessUnitValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Area, RIUserDefaults.Applications.Tanks.ToString, RIUserDefaults.UserProfileTypes.Area.ToString, .AreaValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Line, RIUserDefaults.Applications.Tanks.ToString, RIUserDefaults.UserProfileTypes.Line.ToString, .LineValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Machine, RIUserDefaults.Applications.Tanks.ToString, RIUserDefaults.UserProfileTypes.Machine.ToString, .LineBreakValue))
            'End If
        End With
    End Sub

    Private Sub SaveAvailabilityTracker()
        Dim userName As String = _CurrentUser.SelectedValue
        Dim defaults As New RIUserDefaults.CurrentUserDefaults(userName, RIUserDefaults.Applications.Availability, ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
        With _siteAvailabilityTracking
            'If .RegionValue.Equals("ALL", StringComparison.InvariantCultureIgnoreCase) = False Then
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Region, RIUserDefaults.Applications.Availability.ToString, RIUserDefaults.UserProfileTypes.Region.ToString, .RegionValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Business, RIUserDefaults.Applications.Availability.ToString, RIUserDefaults.UserProfileTypes.Business.ToString, .DivisionValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.PlantCode, RIUserDefaults.Applications.Availability.ToString, RIUserDefaults.UserProfileTypes.PlantCode.ToString, .FacilityValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.BusinessUnit, RIUserDefaults.Applications.Availability.ToString, RIUserDefaults.UserProfileTypes.BusinessUnit.ToString, .BusinessUnitValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Area, RIUserDefaults.Applications.Availability.ToString, RIUserDefaults.UserProfileTypes.Area.ToString, .AreaValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Line, RIUserDefaults.Applications.Availability.ToString, RIUserDefaults.UserProfileTypes.Line.ToString, .LineValue))
            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Machine, RIUserDefaults.Applications.Availability.ToString, RIUserDefaults.UserProfileTypes.Machine.ToString, .LineBreakValue))
            'End If
        End With
    End Sub

    Protected Sub _cblApplications_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _cblApplications.SelectedIndexChanged
        DisplaySelectedApplications()
    End Sub

    Protected Sub _CurrentUser_UserChanged(sender As Object, args As EventArgs) Handles _CurrentUser.UserChanged
       ReselectSelectedApplications
    End Sub
End Class
