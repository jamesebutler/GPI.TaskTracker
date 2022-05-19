'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : mjpope
' Created          : 04-06-2011
'
' Last Modified By : mjpope
' Last Modified On : 04-06-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports IP.MEASFramework.ExtensibleLocalizationAssembly
Partial Class DataMaintenanceDefault
    Inherits IP.Bids.BasePage

#Region "Methods"
    Public Sub HandlePageLoad()

        IP.Bids.SharedFunctions.ResponseRedirect("~/DataMaintenance/MaintenanceEmployee.aspx")
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'HandlePageLoad()

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ' Me.UseBootStrap = False
        Master.PageName = Master.GetLocalizedValue("Data Maintenance", False)
        _RIDataMaintenance.ConnectionString = ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString
        _RIDataMaintenance.UserName = IP.Bids.CurrentUserProfile.GetCurrentUser
        If Not Page.IsPostBack Then _RIDataMaintenance.DefaultSite = IP.Bids.SharedFunctions.GetCurrentUser.PlantCode
        _RIDataMaintenance.PopulateData()
    End Sub
#End Region

    Protected Sub _RIDataMaintenance_DataMaintenanceLoaded(sender As Object, e As EventArgs) Handles _RIDataMaintenance.DataMaintenanceLoaded
        _RIDataMaintenance.ConnectionString = ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString
        _RIDataMaintenance.UserName = IP.Bids.CurrentUserProfile.GetCurrentUser
        Dim appList = _RIDataMaintenance.GetApplicationList
        For Each item In appList
            item.ApplicationName = IP.Bids.SharedFunctions.LocalizeValue(item.ApplicationName, True)
        Next

        _RIDataMaintenance.SiteText = IP.Bids.SharedFunctions.LocalizeValue(_RIDataMaintenance.SiteText, True)
        _RIDataMaintenance.ApplicationText = IP.Bids.SharedFunctions.LocalizeValue(_RIDataMaintenance.ApplicationText, True)
        _RIDataMaintenance.FunctionName = IP.Bids.SharedFunctions.LocalizeValue(_RIDataMaintenance.FunctionName, True)
        _RIDataMaintenance.FunctionNameDescription = IP.Bids.SharedFunctions.LocalizeValue(_RIDataMaintenance.FunctionNameDescription, True)
        If Not IsPostBack Then
            _RIDataMaintenance.SetSelectedSite(_RIDataMaintenance.DefaultSite)
            _RIDataMaintenance.SetSelectedApp(_RIDataMaintenance.DefaultApplication)
        End If
        _RIDataMaintenance.PopulateApplicationList(appList)
        Dim maintenanceFunctions = _RIDataMaintenance.GetMaintenanceFunctions
        For Each item In maintenanceFunctions
            item.PageName = IP.Bids.SharedFunctions.LocalizeValue(item.PageName, True)
            item.Description = IP.Bids.SharedFunctions.LocalizeValue(item.Description, True)
        Next
        _RIDataMaintenance.PopulateMaintenanceTable(maintenanceFunctions)
    End Sub
End Class
