Imports System.Globalization
Imports IP.TaskTrackerDAL


Partial Class MaintenanceRolebySite
    Inherits IP.Bids.BasePage


#Region "Private Events"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Master.PageName = Master.GetLocalizedValue("Role Maint Across Sites", False)
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
            updaterole.UPDATEUSERROLES(Me._employeeList2.SelectedValue, Me._ddlrolelist.SelectedValue, Me._newddlBusUnit.Text, Me._newddlArea.Text, Me._newddlLine.Text, Me._newddlFacility.SelectedValue, "", "", "", "", IP.Bids.SharedFunctions.GetCurrentUser.Username, "", out_status)
            If out_status = 0 Then
                PopulateRoleList(Me._ddlrolelist.SelectedValue, Me._ddlbustypelist.SelectedValue, Me._ddldivisionlist.SelectedValue)
                Me._newddlArea.Text = Nothing
                Me._newddlBusUnit.Text = Nothing
                Me._newddlLine.Text = Nothing
                Me._employeeList2.SelectedValue = Nothing
                Me._newddlFacility.SelectedValue = Nothing
            Else
                Master.DisplayMessageToUser(IP.Bids.SharedFunctions.LocalizeValue("An update error has occurred. Please make sure you have entered all required fields. If problems continue please contact the Task Tracker System Administrator."), IP.Bids.SharedFunctions.LocalizeValue("Update Error"))
            End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("_btnAddUserRole_Click", , ex)
        End Try

    End Sub

    'Protected Sub _ddlemployeelist_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ddlemployeelist.SelectedIndexChanged
    '    If _employeeList2.PlantCode <> _ddlrolelist.SelectedItem.Value Then
    '        PopulateRoleList(_ddlrolelist.SelectedItem.Value)
    '        _employeeList.PlantCode = _ddlrolelist.SelectedValue
    '        _employeeList.PopulateEmployeeList()
    '        'Me._cddlFacility.SelectedValue = _ddlsitelist.SelectedValue
    '        Me._cddlBusinessUnit.SelectedValue = "facility:" & _ddlrolelist.SelectedValue
    '    End If
    'End Sub

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
                PopulateRoleList(Me._ddlrolelist.SelectedItem.Value, Me._ddlbustypelist.SelectedItem.Value, Me._ddldivisionlist.SelectedItem.Value)
            End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("DeleteRoleAssignment", , ex)
        End Try

    End Sub
#End Region

#Region "Methods"
    Private Sub HandlePageLoad()
        Dim userName As String
        Dim RoleSeqId As String = ""
        Dim Division As String = ""
        Dim BusType As String = ""

        Try
            If Not Page.IsPostBack Then

                PopulateAllRoles()
                PopulateAllBusTypes()
                PopulateSite()
                PopulateAllDivision("All")

                _pnlRoleAssignment.Visible = False
                _pnlAddRoleAssignment.Visible = False
                userName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                If Request.QueryString("RoleSeqId") IsNot Nothing Then
                    RoleSeqId = Request.QueryString("RoleSeqId")
                    Me._ddlrolelist.Items.FindByValue(RoleSeqId).Selected = True
                    If Request.QueryString("BusType") IsNot Nothing And Request.QueryString("BusType") <> "0" Then
                        BusType = Request.QueryString("BusType")
                        Me._ddlbustypelist.Items.FindByValue(BusType).Selected = True
                        If Request.QueryString("Division") IsNot Nothing Then
                            Division = Request.QueryString("Division")
                            Me._ddldivisionlist.Items.FindByValue(Division).Selected = True
                            PopulateRoleList(RoleSeqId, BusType, Division)
                        Else
                            PopulateRoleList(RoleSeqId, BusType, "All")
                        End If
                    Else
                        PopulateRoleList(RoleSeqId, "All", "All")
                    End If
                End If
                'PopulateSitebyUser(userName, userPlantCode)
                ' PopulateRoleList(userPlantCode)


                'PopulateAllDivision("All")
                ' _employeeList.PlantCode = userPlantCode

            End If


        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("HandlePageLoad", , ex)
        End Try

    End Sub

    Private Sub PopulateRoleList(ByVal inRoleSeqid As Integer, ByVal inBusType As String, ByVal inDivision As String)
        'Dim roleItem As New DataMaintenanceBLL
        'Dim roleList As System.Collections.Generic.List(Of SiteUserRole) = DataMaintenanceBLL.GetSiteUserRoleList(inPlantCode)
        Dim roleList As System.Collections.Generic.List(Of RoleBySite) = DataMaintenanceBLL.GetRolesbySiteList(inRoleSeqid, inBusType, inDivision)

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
        Dim allroleList As System.Collections.Generic.List(Of Role) = DataMaintenanceBLL.GetAllRoles()

        Try
            _ddlrolelist.Items.Clear()
            For Each item As Role In allroleList '.Where(Function(obj) obj.DisplayFlag = "Y")
                With Me._ddlrolelist
                    If item.RoleDescription.Length > 0 Then
                        .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.RoleDescription, True), item.RoleId))
                    End If
                End With
            Next

            Me._ddlrolelist.Items.Insert(0, New ListItem("---" & IP.Bids.SharedFunctions.LocalizeValue("Select Role", True) & "---", 0))
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateAllRoles", , ex)
        End Try

    End Sub

    Private Sub PopulateAllBusTypes()
        'Dim allroleItem As New DataMaintenanceBLL
        Dim allbustypeList As System.Collections.Generic.List(Of BusType) = DataMaintenanceBLL.GetAllBusTypes()

        Try
            For Each item As BusType In allbustypeList
                With Me._ddlbustypelist
                    If item.BusName.Length > 0 Then
                        .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.BusName, True), item.BusType))
                    End If
                End With
            Next

            Me._ddlbustypelist.Items.Insert(0, New ListItem(IP.Bids.SharedFunctions.LocalizeValue("All", True), 0))
            If _ddlbustypelist.Items.FindByValue(0) IsNot Nothing Then
                _ddlbustypelist.ClearSelection()
                _ddlbustypelist.Items.FindByValue(0).Selected = True
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateAllBusTypes", , ex)
        End Try

    End Sub

    Private Sub PopulateAllDivision(ByVal inBusType As String)
        'Dim allroleItem As New DataMaintenanceBLL
        Dim alldivisionList As System.Collections.Generic.List(Of Division) = DataMaintenanceBLL.GetDivisionListByBusType(inBusType)

        Try
            For Each item As Division In alldivisionList
                With Me._ddldivisionlist
                    If item.DivisionName.Length > 0 Then
                        .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.DivisionName, True), item.DivisionName))
                    End If
                End With
            Next

            'Me._ddlbustypelist.Items.Insert(0, New ListItem(IP.Bids.SharedFunctions.LocalizeValue("All", True), 0))
            Me._ddldivisionlist.Items.FindByValue("All").Selected = True
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateAllDivisions", , ex)
        End Try

    End Sub

    Private Sub PopulateSite()
        Dim siteItem As New TaskTrackerSiteBll
        Dim facilityList As System.Collections.Generic.List(Of Facility) = siteItem.GetFacility("", "")

        Try
            For Each item As Facility In facilityList
                With Me._newddlFacility
                    If item.SiteName.Length > 0 AndAlso item.PlantCode.ToUpper <> "ALL" AndAlso item.PlantCode <> "9998" Then
                        .Items.Add(New ListItem(item.SiteName, item.PlantCode))
                    End If
                End With
                'If Request.QueryString("MaintFlag") IsNot Nothing AndAlso Request.QueryString("MaintFlag") = "MAINT" Then
                '    If _ddlsitelist.Items.FindByValue(inPlantCode) IsNot Nothing Then
                '        _ddlsitelist.ClearSelection()
                '        _ddlsitelist.Items.FindByValue(inPlantCode).Selected = True
                '    End If
                'End If
            Next
            ' Me._newddlFacility.Items.Insert(0, New ListItem("----Select Facility----", 0))

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateSite", , ex)
        End Try
    End Sub

    'Public Function GetBusTypeDivision() As String

    '    Return String.Format("&Bustype={0}&Division={1}", _ddlbustypelist.SelectedValue, _ddldivisionlist.SelectedValue)

    'End Function

    Private Sub _btnSubmit_Click(sender As Object, e As EventArgs) Handles _btnSubmit.Click
        Try
            If Me._ddlrolelist.SelectedItem IsNot Nothing And Me._ddlrolelist.SelectedItem.Value > 0 Then
                If Me._ddlbustypelist.SelectedItem IsNot Nothing And Me._ddlbustypelist.SelectedItem.Value <> "" Then
                    If Me._ddldivisionlist.SelectedItem IsNot Nothing Then 'And Me._ddldivisionlist.SelectedItem.Value <> "" Then
                        PopulateRoleList(Me._ddlrolelist.SelectedItem.Value, Me._ddlbustypelist.SelectedItem.Value, Me._ddldivisionlist.SelectedItem.Value)
                    Else
                        PopulateRoleList(Me._ddlrolelist.SelectedItem.Value, Me._ddlbustypelist.SelectedItem.Value, "All")
                        PopulateAllDivision(Me._ddlbustypelist.SelectedItem.Value)
                    End If
                Else
                    PopulateRoleList(Me._ddlrolelist.SelectedItem.Value, "All", "All")
                End If
                _pnlRoleAssignment.Visible = True
                _pnlAddRoleAssignment.Visible = True
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("SubmitClick", , ex)
        End Try
    End Sub

    Protected Sub _gvRoleList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles _gvRoleList.RowDataBound

        Dim rolelist As RoleBySite

        If e.Row.RowType = DataControlRowType.DataRow Then

            rolelist = TryCast(e.Row.DataItem, BO.RoleBySite)
            If rolelist Is Nothing Then Exit Sub

            'Person    
            Dim ResponsiblePerson As User_Controls.AdvancedEmployeeListDropdown = CType(e.Row.FindControl("_ResponsiblePerson"), User_Controls.AdvancedEmployeeListDropdown)
            ResponsiblePerson.PlantCode = rolelist.PlantCode
            ResponsiblePerson.PopulateEmployeeList(rolelist.AssigneeNetworkId)
            'ResponsiblePerson.ID = "_ResponsiblePerson" & e.Row.RowIndex




        End If
    End Sub
    Protected Sub _btnUpdateRoles_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnUpdateRoles.Click
        Dim updaterole As New DataMaintenanceDALTableAdapters.SiteUserRoleListTableAdapter
        Dim out_status As Decimal = 0

        Dim busunit As String = "All"
        Dim area As String = "All"
        Dim line As String = "All"
        Dim introw As Integer
        Dim role As String
        Dim bustype As String
        Dim division As String
        Dim busunitkey As String = "All"
        Dim areakey As String = "All"
        Dim linekey As String = "All"
        role = _ddlrolelist.SelectedValue
        bustype = _ddlbustypelist.SelectedValue
        division = _ddldivisionlist.SelectedValue


        Try

            For Each r As GridViewRow In _gvRoleList.Rows
                introw = r.RowIndex
                Dim plantcode As String = Me._gvRoleList.DataKeys(introw).Values(0).ToString()
                Dim userkey As String = Me._gvRoleList.DataKeys(introw).Values(1).ToString()
                'Dim busunitkey As String = Me._gvRoleList.DataKeys(introw).Values(2).ToString()
                'Dim areakey As String = Me._gvRoleList.DataKeys(introw).Values(3).ToString()
                'Dim linekey As String = Me._gvRoleList.DataKeys(introw).Values(4).ToString()
                'busunit = CType(Me._gvRoleList.Rows(introw).FindControl("_ddlBusUnit"), DropDownList).SelectedValue
                'area = CType(Me._gvRoleList.Rows(introw).FindControl("_ddlArea"), DropDownList).SelectedValue
                'line = CType(Me._gvRoleList.Rows(introw).FindControl("_ddlLine"), DropDownList).SelectedValue
                Dim responsible As User_Controls.AdvancedEmployeeListDropdown = TryCast(Me._gvRoleList.Rows(introw).FindControl("_ResponsiblePerson"), User_Controls.AdvancedEmployeeListDropdown)
                Dim facility As Label = TryCast(Me._gvRoleList.Rows(introw).FindControl("_lblFacility"), Label)
                'facility = Me._gvRoleList.Rows(introw).FindControl("_lblFacility")
                If userkey <> responsible.SelectedValue Then 'Or busunitkey <> busunit Or areakey <> area Or linekey <> line Then
                    updaterole.UPDATEUSERROLES(responsible.SelectedValue, role, busunit, area, line, plantcode, userkey, busunitkey, areakey, linekey, IP.Bids.SharedFunctions.GetCurrentUser.Username, "Y", out_status)
                End If
            Next

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("_btnUpdateRoles_Click", , ex)
        End Try
    End Sub

#End Region


End Class
