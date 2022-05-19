Imports System.Globalization

Partial Class MaintenanceRole
    Inherits IP.Bids.BasePage

#Region "Private Events"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Master.PageName = Master.GetLocalizedValue("Role Maintenance", False)
        Master.SetActiveNav(Enums.NavPages.DataMaintenance.ToString)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandlePageLoad()
    End Sub

    Protected Sub _btnAddUserRole_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnAddUserRole.Click
        Dim updaterole As New DataMaintenanceDALTableAdapters.SiteUserRoleListTableAdapter
        Dim out_status As Decimal = 0

        Try
            'Me.Validate("DataMaintEmp")
            'If Page.IsValid Then
            updaterole.UPDATEUSERROLES(Me._employeeList.SelectedValue, Me._newddlRoleDescription.SelectedValue, Me._newddlBusUnit.Text, Me._newddlArea.Text, Me._newddlLine.Text, Me._ddlsitelist.SelectedValue, "", "", "", "", IP.Bids.SharedFunctions.GetCurrentUser.Username, "", out_status)
            If out_status = 0 Then
                PopulateRoleList(Me._ddlsitelist.SelectedValue)
                Me._newddlArea.Text = Nothing
                Me._newddlBusUnit.Text = Nothing
                Me._newddlLine.Text = Nothing
                Me._newddlRoleDescription.Text = Nothing
                Me._employeeList.SelectedValue = Nothing
            Else
                Master.DisplayMessageToUser(IP.Bids.SharedFunctions.LocalizeValue("An update error has occurred. Please make sure you have entered all required fields. If problems continue please contact the Task Tracker System Administrator."), IP.Bids.SharedFunctions.LocalizeValue("Update Error"))
            End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("_btnAddUserRole_Click", , ex)
        End Try

    End Sub
    Protected Sub _ddlsitelist_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ddlsitelist.SelectedIndexChanged
        If _employeeList.PlantCode <> _ddlsitelist.SelectedItem.Value Then
            PopulateRoleList(_ddlsitelist.SelectedItem.Value)
            _employeeList.PlantCode = _ddlsitelist.SelectedValue
            _employeeList.PopulateEmployeeList()
            'Me._cddlFacility.SelectedValue = _ddlsitelist.SelectedValue
            Me._cddlBusinessUnit.SelectedValue = "facility:" & _ddlsitelist.SelectedValue
        End If
    End Sub

    Public Sub EditRoleAssignment(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim btn As Button = TryCast(sender, Button)
        'If btn IsNot Nothing Then

        'End If
    End Sub

    Public Sub DeleteRoleAssignment(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As LinkButton = TryCast(sender, LinkButton)
        Dim DataMaintenanceBLL As New DataMaintenanceBLL
        Try
            If btn IsNot Nothing Then
                DataMaintenanceBLL.DeleteRoleAssignment(btn.CommandArgument)
                PopulateRoleList(_ddlsitelist.SelectedValue)
            End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("DeleteRoleAssignment", , ex)
        End Try

    End Sub
#End Region

#Region "Methods"
    Private Sub HandlePageLoad()
        Dim userName As String
        Dim userPlantCode As String = ""

        Try
            If Not Page.IsPostBack Then
                userName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                If Request.QueryString("plantcode") IsNot Nothing Then
                    userPlantCode = Request.QueryString("plantcode")
                Else
                    userPlantCode = IP.Bids.SharedFunctions.GetCurrentUser.PlantCode
                End If
                PopulateSitebyUser(userName, userPlantCode)
                PopulateRoleList(userPlantCode)
                PopulateAllRoles()
                _employeeList.PlantCode = userPlantCode
            End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("HandlePageLoad", , ex)
        End Try

    End Sub

    Private Sub PopulateRoleList(ByVal inPlantCode As String)
        'Dim roleItem As New DataMaintenanceBLL
        Dim roleList As System.Collections.Generic.List(Of SiteUserRole) = DataMaintenanceBLL.GetSiteUserRoleList(inPlantCode)

        Try

            If roleList Is Nothing Then
                Me._gvRoleList.Visible = "False"
            Else
                _gvRoleList.DataSource = roleList
                _gvRoleList.DataBind()
                _gvRoleList.UseAccessibleHeader = True
                _gvRoleList.HeaderRow.TableSection = TableRowSection.TableHeader
                ScriptManager.RegisterStartupScript(Me._udpRoleList, _udpRoleList.GetType(), "SortGrid", String.Format(CultureInfo.CurrentCulture, "$(function(){{$('#{0}').tablesorter(); }});", _gvRoleList.ClientID), True)

            End If


            'Me._ddlsitelist.Items.Insert(0, New ListItem("----Select Facility----", 0))
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateRoleList", , ex)
        End Try

    End Sub

    Private Sub PopulateAllRoles()
        'Dim allroleItem As New DataMaintenanceBLL
        Dim allroleList As System.Collections.Generic.List(Of Role) = DataMaintenanceBLL.GetAllRoles()

        Try
            For Each item As Role In allroleList
                With Me._newddlRoleDescription
                    If item.RoleDescription.Length > 0 Then
                        .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.RoleDescription, True), item.RoleId))
                    End If
                End With
            Next

            Me._newddlRoleDescription.Items.Insert(0, New ListItem("---" & IP.Bids.SharedFunctions.LocalizeValue("Select Role", True) & "---", 0))
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateAllRoles", , ex)
        End Try

    End Sub

    Private Sub PopulateSitebyUser(ByVal userName As String, ByVal inPlantCode As String)
        Dim siteItem As New TaskTrackerSiteBll
        Dim facilityList As System.Collections.Generic.List(Of Facility) = siteItem.GetFacilitybyUser(userName)

        Try
            For Each item As Facility In facilityList
                With Me._ddlsitelist
                    If item.SiteName.Length > 0 Then
                        '.Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.SiteName, True), item.PlantCode))
                        .Items.Add(New ListItem(item.SiteName, item.PlantCode))
                    End If
                End With
                If _ddlsitelist.Items.FindByValue(inPlantCode) IsNot Nothing Then
                    _ddlsitelist.ClearSelection()
                    _ddlsitelist.Items.FindByValue(inPlantCode).Selected = True
                End If
            Next
            Me._ddlsitelist.Items.Insert(0, New ListItem("---" & IP.Bids.SharedFunctions.LocalizeValue("Select Facility", True) & "---", 0))

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateSitebyUser", , ex)
        End Try
    End Sub

    Private Sub _gvRoleList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles _gvRoleList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).Attributes("data-title") = _gvRoleList.Columns(i).HeaderText
            Next
        End If
    End Sub
#End Region






End Class
