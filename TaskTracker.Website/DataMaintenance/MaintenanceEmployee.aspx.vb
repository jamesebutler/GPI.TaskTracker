Imports System.Linq





Partial Class MaintenanceMain
    Inherits IP.Bids.BasePage

    Public Sub New()

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Master.PageName = Master.GetLocalizedValue("Employee Maintenance", False)
        _lblHeaderTitle.Text = Master.PageName
        Master.SetActiveNav(Enums.NavPages.DataMaintenance.ToString)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandlePageLoad()
    End Sub
    Private Sub HandlePageLoad()
        Dim userName As String
        Dim userPlantCode As String

        Try
            If Not Page.IsPostBack Then
                userName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                userPlantCode = IP.Bids.SharedFunctions.GetCurrentUser.PlantCode
                'Dim defaults As Specialized.StringDictionary = Nothing
                'If Master.CurrentUser IsNot Nothing AndAlso Master.CurrentUser.UserDefaults IsNot Nothing Then
                '   defaults = Master.CurrentUser.UserDefaults
                'End If

                If Request.QueryString("Application") = "RI" Then
                    userPlantCode = Request.QueryString("PlantCode")
                    Master.HideBannerFooter()
                End If

                PopulateUserInfo()
                '_btnInactiveFlag.HideDisplayButton()
                If Request.QueryString("MaintFlag") IsNot Nothing And Request.QueryString("MaintFlag") = "MAINT" Then
                    PopulateSitebyUser(userName, userPlantCode)
                    _gvEmployeeList.Visible = True
                    _ddlsitelist.AutoPostBack = True
                    PopulateEmployeeList(userPlantCode)
                Else
                    PopulateSite()
                    _btnTransfer.Visible = "False"
                    _btnUpdateEmp.Visible = "False"
                    _cbShowHideInactiveUsers.Visible = False

                End If

            End If


        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("HandlePageLoad", , ex)
        End Try

    End Sub
    Private Sub PopulateUserInfo()
        Dim UserInfo As IP.Bids.UserInfo = IP.Bids.SharedFunctions.GetCurrentUser

        Dim myRoles As System.Collections.Generic.List(Of UserRoles) = GeneralTaskTrackerBll.GetUserRoles(IP.Bids.SharedFunctions.GetCurrentUser.Username)
        Try
            If myRoles IsNot Nothing AndAlso myRoles.Count > 0 Then
                Me._networkid.ReadOnly = True
                Me._ddldomain.Enabled = False
                For Each role As UserRoles In myRoles
                    If role.RoleName.ToUpper = "SUPPORT" Or role.RoleName.ToUpper = "FACILITYADMIN" Then
                        Me._networkid.ReadOnly = False
                        Me._ddldomain.Enabled = True
                    End If
                Next
            Else
                Me._networkid.ReadOnly = True
                Me._ddldomain.Enabled = False
            End If

            With Me
                If UserInfo.IsUserInEmployeeTable = False Then
                    ._lblEmployeeStatus.Text = "Your userid was not found in the Task Tracker Employee list. Please select your facility, add your employee information, and Click Add Employee button. This will add your information to the Task Tracker Reference data."
                Else
                    ._lblEmployeeStatus.Visible = False
                End If
                If Request.QueryString("MaintFlag") Is Nothing Then 'AndAlso Request.QueryString("MaintFlag") = "" Then
                    ._networkid.Text = UserInfo.Username
                    ._lastname.Text = UserInfo.LastName
                    ._firstname.Text = UserInfo.FirstName
                    ._email.Text = UserInfo.Email
                    ._phone.Text = UserInfo.PhoneNumber
                    ._midinit.Text = UserInfo.MiddleInitial
                    PopulateLanguageList(UserInfo.UserDefaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Language, "EN-US"))
                    PopulateDomain(UserInfo.Domain)
                Else
                    PopulateLanguageList(UserInfo.UserDefaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Language, "EN-US"))
                    PopulateDomain(UserInfo.Domain)
                End If


            End With
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateUserInfo", , ex)
        End Try
    End Sub
    Private Sub PopulateEmployeeList(ByVal inPlantCode As String)
        Dim empItem As New DataMaintenanceBLL
        Dim employeeList As System.Collections.Generic.List(Of Employee) = DataMaintenanceBLL.GetEmployeeList(inPlantCode)

        Try

            If employeeList Is Nothing OrElse employeeList.Count = 0 Then
                Me._gvEmployeeList.Visible = "False"
            Else
                Dim inactiveFlagList As New List(Of String)
                inactiveFlagList.Add("N")
                If _cbShowHideInactiveUsers.Checked = True Then inactiveFlagList.Add("Y")
                _gvEmployeeList.DataSource = employeeList.Where(Function(obj) inactiveFlagList.Contains(obj.InactiveFlag)).ToList
                _gvEmployeeList.DataBind()
            End If
            'PopulateCurrentUserProfile()
            'If _gvEmployeeList.Rows.Count > 100 Then
            '    Response.BufferOutput = False
            'Else
            '    Response.BufferOutput = True
            'End If
            'Me._ddlsitelist.Items.Insert(0, New ListItem("----Select Facility----", 0))
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateEmployeeList", , ex)
        End Try

    End Sub

    Private Sub PopulateCurrentUserProfile()
        Dim userData As New IP.Bids.UserInfo()
        Dim empList As New List(Of Employee)
        With userData
            empList.Add(New Employee(.Username, .FirstName, .LastName, .MiddleInitial, .Email, .PhoneNumber, .Domain, "", "Y", "0"))
        End With

        With _gvMyProfile
            .DataSource = empList.ToList
            .DataBind()
            .Enabled = False
        End With
    End Sub
    Private Sub PopulateSite()
        Dim siteItem As New TaskTrackerSiteBll
        Dim facilityList As System.Collections.Generic.List(Of Facility) = siteItem.GetFacility("", "")

        Try
            For Each item As Facility In facilityList
                With Me._ddlsitelist
                    If item.SiteName.Length > 0 Then
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
            Me._ddlsitelist.Items.Insert(0, New ListItem("----Select Facility----", 0))

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateSite", , ex)
        End Try
    End Sub
    Private Sub PopulateSitebyUser(ByVal userName As String, ByVal inPlantCode As String)
        Dim siteItem As New TaskTrackerSiteBll
        Dim facilityList As System.Collections.Generic.List(Of Facility) = siteItem.GetFacilitybyUser(userName)

        Try
            For Each item As Facility In facilityList
                With Me._ddlsitelist
                    If item.SiteName.Length > 0 Then
                        .Items.Add(New ListItem(item.SiteName, item.PlantCode))
                    End If
                End With
                If _ddlsitelist.Items.FindByValue(inPlantCode) IsNot Nothing Then
                    _ddlsitelist.ClearSelection()
                    _ddlsitelist.Items.FindByValue(inPlantCode).Selected = True
                End If
            Next
            Me._ddlsitelist.Items.Insert(0, New ListItem("----Select Facility----", 0))

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateSitebyUser", , ex)
        End Try
    End Sub
    Private Sub PopulateLanguageList(ByVal defaultLanguage As String)
        Dim IPResources As IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization

        Try
            IPResources = New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization
            If IPResources IsNot Nothing AndAlso IPResources.ApplicationLocaleList IsNot Nothing Then
                _ddldefaultlang.Items.Clear()
                For Each item As DictionaryEntry In IPResources.ApplicationLocaleList
                    If item.Key.ToString.ToUpper = "EN-US" Then
                        _ddldefaultlang.Items.Add(New ListItem(item.Value, item.Key.ToString.ToUpper))
                    End If
                Next
                If defaultLanguage IsNot Nothing Then
                    If _ddldefaultlang.Items.FindByValue(defaultLanguage.ToUpper) IsNot Nothing Then
                        _ddldefaultlang.ClearSelection()
                        _ddldefaultlang.Items.FindByValue(defaultLanguage.ToUpper).Selected = True
                    End If
                Else
                    _ddldefaultlang.Items.FindByValue("EN-US").Selected = True
                End If
            End If
            Session.Add("DefaultLangList", IPResources.ApplicationLocaleList)
            IPResources = Nothing
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateLanguageList", , ex)
        End Try
    End Sub
    Private Sub PopulateDomain(ByVal defaultdomain As String)
        Dim DomainList As System.Collections.Generic.List(Of Domain) = Domain.GetListOfDomains

        Try
            For Each item As Domain In DomainList
                With Me._ddldomain
                    If item.DomainName.Length > 0 Then
                        If item.DomainName.ToUpper = "NA" Then
                            .Items.Add(New ListItem(item.DomainName, item.DomainName.ToUpper))
                        End If
                    End If
                End With
            Next
            If _ddldomain.Items.FindByValue(defaultdomain.ToUpper) IsNot Nothing Then
                _ddldomain.ClearSelection()
                _ddldomain.Items.FindByValue(defaultdomain.ToUpper).Selected = True
            End If
            Session.Add("DomainList", DomainList)
            'Me._ddldomain.Items.Insert(0, "")
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateDomain", , ex)
        End Try
    End Sub
    Private Sub _btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSubmit.Click
        Dim updateemployee As New DataMaintenanceDALTableAdapters.EmployeeTableAdapter
        Dim out_status As Decimal = 0

        Try
            If Request.QueryString("MaintFlag") Is Nothing Then 'AndAlso Request.QueryString("MaintFlag") = "" Then
                Me.Validate("DataMaintEmp")
                If Page.IsValid Then
                    updateemployee.UPDATEEMPLOYEE(Me._networkid.Text, Me._ddldomain.SelectedValue, Me._firstname.Text, Me._lastname.Text, Me._email.Text, Me._phone.Text, "N", Me._ddldefaultlang.SelectedValue, Me._ddlsitelist.SelectedValue, "", IP.Bids.SharedFunctions.GetCurrentUser.Username, Me._midinit.Text, "", "", out_status)
                    If out_status = 0 Then
                        'Me._networkid.Text = Nothing
                        'Me._firstname.Text = Nothing
                        'Me._lastname.Text = Nothing
                        'Me._email.Text = Nothing
                        'Me._phone.Text = Nothing
                        'Me._midinit.Text = Nothing
                        'Me._ddldefaultlang.Items.Clear()
                        'Me._ddldomain.Items.Clear()
                        Session.Remove("CurrentUserInfo")
                        'Server.Transfer("~/ViewTasks.aspx", False)
                        IP.Bids.SharedFunctions.ResponseRedirect("~/ViewTasks.aspx")
                    Else
                        Me._lblEmployeeStatus.Text = "An update error has occurred. Please make sure you have selected a valid Facility and retry. If problems continue please contact the Task Tracker System Administrator."
                        'Server.Transfer("~/DataMaintenance/MaintenanceEmployee.aspx", False)
                    End If
                Else
                    Me._lblEmployeeStatus.Text = "A validation error has occurred. Please make sure you have entered all required fields."
                End If
            Else
                If Me._networkid.Text IsNot Nothing And Me._networkid.Text <> "" Then
                    Me.Validate("DataMaintEmp")
                    If Page.IsValid Then
                        updateemployee.UPDATEEMPLOYEE(Me._networkid.Text, Me._ddldomain.SelectedValue, Me._firstname.Text, Me._lastname.Text, Me._email.Text, Me._phone.Text, "N", Me._ddldefaultlang.SelectedValue, Me._ddlsitelist.SelectedValue, "", IP.Bids.SharedFunctions.GetCurrentUser.Username, Me._midinit.Text, "", "", out_status)
                        If out_status <> 0 Then
                            Me._lblEmployeeStatus.Text = "An update error has occurred. Please make sure you have selected a valid Facility and retry. If problems continue please contact the Task Tracker System Administrator."
                        Else
                            'If Not Me._btnInactiveFlag.Checked Then
                            Me._networkid.Text = Nothing
                            Me._firstname.Text = Nothing
                            Me._lastname.Text = Nothing
                            Me._email.Text = Nothing
                            Me._phone.Text = Nothing
                            Me._midinit.Text = Nothing
                            'Me._ddldefaultlang.Items.Clear()
                            'Me._ddldomain.Items.Clear()
                            PopulateEmployeeList(Me._ddlsitelist.SelectedValue)
                        End If
                    End If
                End If
            End If
            CacheHelper.DeleteEntireCache()
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("_btnSubmit_Click", , ex)
        End Try

    End Sub

    Protected Sub _btnTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnTransfer.Click
        Dim updateemployee As New DataMaintenanceDALTableAdapters.EmployeeTableAdapter
        Dim out_status As Decimal = 0
        Dim strTransferFlag As String

        Try
            strTransferFlag = "Y"

            If _ResponsiblePerson.SelectedValue IsNot Nothing And _ResponsiblePerson.SelectedValue <> "" Then
                updateemployee.UPDATEEMPLOYEE(_ResponsiblePerson.SelectedValue, Me._ddldomain.SelectedValue, Me._firstname.Text, Me._lastname.Text, Me._email.Text, Me._phone.Text, "N", Me._ddldefaultlang.SelectedValue, Me._ddlsitelist.SelectedValue, "", IP.Bids.SharedFunctions.GetCurrentUser.Username, Me._midinit.Text, "", strTransferFlag, out_status)
                If out_status <> 0 Then
                    Me._lblEmployeeStatus.Text = "An update error has occurred. Please make sure you have selected a valid Facility and retry. If problems continue please contact the Task Tracker System Administrator."
                Else
                    Me._networkid.Text = Nothing
                    Me._firstname.Text = Nothing
                    Me._lastname.Text = Nothing
                    Me._email.Text = Nothing
                    Me._phone.Text = Nothing
                    Me._midinit.Text = Nothing
                    PopulateEmployeeList(Me._ddlsitelist.SelectedValue)
                End If
            End If
            CacheHelper.DeleteEntireCache()
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("_btnTransfer_Click", , ex)
        End Try
    End Sub

    Protected Sub _btnUpdateEmp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnUpdateEmp.Click
        Dim intRow As Integer
        'Dim employeerow As Employee
        Dim ddlDomain, ddldefaultlang As DropDownList
        Dim cbInactiveFlag As CheckBox
        Dim tbNetworkID, tbLastName, tbFirstName, tbPhone, tbMidInit, tbEmail As TextBox
        Dim strPlantCode As String
        Dim strDomain As String
        Dim strNetworkID As String
        Dim strNetworkIDKey As String
        Dim strLastName As String
        Dim strFirstName As String
        Dim strPhone As String
        Dim strMidInit As String
        Dim strEmail As String
        Dim strInactiveFlag As String
        Dim strDefaultLang As String
        Dim i As Integer = 0
        Dim outStatus As Decimal = 0
        Dim updateemployee As New DataMaintenanceDALTableAdapters.EmployeeTableAdapter
        Try
            Me._lblEmployeeStatus.Visible = False
            strPlantCode = CType(Me._ddlsitelist.SelectedValue, String)
            For i = 0 To _gvEmployeeList.DirtyRows.Count - 1
                intRow = _gvEmployeeList.DirtyRows.Item(i).DataItemIndex
                'employeerow = CType(Me._gvEmployeeList.DataKeys(intRow).Item("Networkid"), Employee)
                tbNetworkID = CType(Me._gvEmployeeList.Rows(intRow).FindControl("_tbNetworkID"), TextBox)

                tbLastName = CType(Me._gvEmployeeList.Rows(intRow).FindControl("_tbLastName"), TextBox)
                tbFirstName = CType(Me._gvEmployeeList.Rows(intRow).FindControl("_tbFirstName"), TextBox)
                tbMidInit = CType(Me._gvEmployeeList.Rows(intRow).FindControl("_tbMidInit"), TextBox)
                tbPhone = CType(Me._gvEmployeeList.Rows(intRow).FindControl("_tbPhone"), TextBox)
                tbEmail = CType(Me._gvEmployeeList.Rows(intRow).FindControl("_tbEmail"), TextBox)
                ddlDomain = CType(Me._gvEmployeeList.Rows(intRow).FindControl("_ddldomain2"), DropDownList)
                ddldefaultlang = CType(Me._gvEmployeeList.Rows(intRow).FindControl("_ddldefaultlang2"), DropDownList)
                cbInactiveFlag = CType(Me._gvEmployeeList.Rows(intRow).FindControl("_cbInactiveFlag"), CheckBox)

                If tbNetworkID.Text IsNot Nothing AndAlso tbNetworkID.Text <> "" AndAlso tbFirstName.Text IsNot Nothing AndAlso tbFirstName.Text <> "" AndAlso tbLastName.Text IsNot Nothing AndAlso tbLastName.Text <> "" AndAlso tbEmail.Text IsNot Nothing AndAlso tbEmail.Text <> " " AndAlso tbEmail.Text <> "" Then
                    strDomain = ddlDomain.SelectedValue
                    strDefaultLang = ddldefaultlang.SelectedValue
                    strNetworkIDKey = Me._gvEmployeeList.DataKeys(intRow).Item("Networkid").ToString
                    strNetworkID = tbNetworkID.Text
                    strLastName = tbLastName.Text
                    strFirstName = tbFirstName.Text
                    strPhone = tbPhone.Text
                    strMidInit = tbMidInit.Text
                    strEmail = tbEmail.Text
                    'Me._gvEmployeeList.Rows(intRow).BackColor = Drawing.Color.DarkKhaki

                    If cbInactiveFlag.Checked Then
                        strInactiveFlag = "Y"
                    Else
                        strInactiveFlag = "N"
                    End If
                    updateemployee.UPDATEEMPLOYEE(strNetworkID, strDomain, strFirstName, strLastName, strEmail, strPhone, strInactiveFlag, strDefaultLang, strPlantCode, "", IP.Bids.SharedFunctions.GetCurrentUser.Username, strMidInit, strNetworkIDKey, "", outStatus)
                    If outStatus <> 0 Then
                        Me._lblEmployeeStatus.Visible = True
                        Me._gvEmployeeList.Rows(intRow).BackColor = Drawing.Color.Red
                        Me._lblEmployeeStatus.Text = "An update error has occurred. Please make sure you have selected a valid Facility and retry. If problems continue please contact the Task Tracker System Administrator."
                        'Else
                        '   Me._gvEmployeeList.Rows(intRow).BackColor = Drawing.Color.Green
                    End If
                Else
                    Me._gvEmployeeList.Rows(intRow).BackColor = Drawing.Color.Red
                    Me._lblEmployeeStatus.Visible = True
                    Me._lblEmployeeStatus.Text = "A validation error has occurred. Please make sure you have entered all required fields."
                End If
            Next
            CacheHelper.DeleteEntireCache()
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("_btnUpDateEmp_Click", , ex)
        End Try
    End Sub
    Protected Sub _gvEmployeeList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles _gvEmployeeList.RowDataBound
        Dim _ddldomain2 As DropDownList = e.Row.FindControl("_ddldomain2")
        Dim DomainList As System.Collections.Generic.List(Of Domain)
        Dim defaultdomain As String
        Dim employeerow As Employee
        Dim defaultlanglist As System.Collections.Hashtable
        Dim _ddldefaultlang2 As DropDownList = e.Row.FindControl("_ddldefaultlang2")
        Dim defaultlang As String
        Dim _cbInactiveFlag As CheckBox = e.Row.FindControl("_cbInactiveFlag")
        Dim inactiveflag As String
        Dim urltxt As String = "../Popups/ProcessEmployees.aspx?userid={0}&plantcode={1}&firstname={2}&lastname={3}&taskcount={4}"
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim hdRowChange As HiddenField = TryCast(e.Row.FindControl("_rowChanged"), HiddenField)
                If hdRowChange IsNot Nothing Then
                    Dim rowChangedJS As String = String.Format("document.getElementById('{0}').value='changed';", hdRowChange.ClientID)
                    DomainList = Session.Item("DomainList")
                    employeerow = CType(e.Row.DataItem, Employee)
                    defaultlanglist = CType(Session.Item("defaultlanglist"), Hashtable)

                    If _ddldomain2 IsNot Nothing AndAlso DomainList IsNot Nothing And employeerow IsNot Nothing Then
                        _ddldomain2.Attributes.Add("onChange", rowChangedJS)
                        defaultdomain = employeerow.Domain
                        For Each item As Domain In DomainList
                            With _ddldomain2
                                If item.DomainName.ToUpper = "NA" Then
                                    If item.DomainName.Length > 0 Then
                                        .Items.Add(New ListItem(item.DomainName, item.DomainName.ToUpper))
                                    End If
                                End If
                            End With
                        Next
                        If _ddldomain2.Items.FindByValue(defaultdomain.ToUpper) IsNot Nothing Then
                            _ddldomain2.ClearSelection()
                            _ddldomain2.Items.FindByValue(defaultdomain.ToUpper).Selected = True
                        End If
                    End If

                    If defaultlanglist IsNot Nothing AndAlso _ddldefaultlang2 IsNot Nothing Then
                        ' _ddldefaultlang2.Items.Clear()


                        _ddldefaultlang2.Attributes.Add("onChange", rowChangedJS)
                        defaultlang = employeerow.DefaultLang

                        For Each item As DictionaryEntry In defaultlanglist
                            If item.Key.ToString.ToUpper = "EN-US" Then
                                _ddldefaultlang2.Items.Add(New ListItem(item.Value, item.Key.ToString.ToUpper))
                            End If
                        Next
                        If defaultlang.Length > 0 Then
                            If _ddldefaultlang2.Items.FindByValue(defaultlang.ToUpper) IsNot Nothing Then
                                _ddldefaultlang2.ClearSelection()
                                _ddldefaultlang2.Items.FindByValue(defaultlang.ToUpper).Selected = True
                            End If
                        Else
                            _ddldefaultlang2.Items.FindByValue("EN-US").Selected = True
                        End If
                    End If

                    If _cbInactiveFlag IsNot Nothing Then
                        inactiveflag = employeerow.InactiveFlag
                        '_cbInactiveFlag.Attributes.Add("onClick", rowChangedJS & " if (this.checked==true){" & "Javascript:window.open('" & String.Format(Page.ResolveUrl("~/Popups/ProcessEmployees.aspx?userid={0}&plantcode={1}&firstname={2}&lastname={3}"), employeerow.NetworkId, Me._ddlsitelist.SelectedValue, employeerow.FirstName, employeerow.LastName)) '; } ")
                        _cbInactiveFlag.Attributes.Add("onClick", rowChangedJS & " if (this.checked==true){" & "Javascript:window.open('" & String.Format(Page.ResolveUrl("~/Popups/ProcessEmployees.aspx?userid={0}&plantcode={1}&firstname={2}&lastname={3}&taskcount={4}"), employeerow.NetworkId.Replace("'", "&#8217"), Me._ddlsitelist.SelectedValue, employeerow.FirstName.Replace("'", "&#8217"), employeerow.LastName.Replace("'", "&#8217"), employeerow.TaskCount) & "&cb='+this.id,'InactivateEmployee','width=1200px,height=200px,resizable=yes,scrollbars=yes,left=150,top=500'); } ")
                        'Me._btnAttachments.OnClientClick = "Javascript:viewPopUp('FileUpload.aspx?RINumber=" & currentRI.RINumber & "'," & confirmMessage & ",'fu');return false"
                        'href="Javascript:parent.window.location=('<%#string.format(CultureInfo.CurrentCulture, Page.ResolveUrl("~/DataMaintenance/EditRoles.aspx?Plantcode={0}&Roleid={1}&RoleDescription={2}&Userid={3}&Busunit={4}&Area={5}&Line={6}"),DirectCast(Container.DataItem, SiteUserRole).PlantCode , DirectCast(Container.DataItem, SiteUserRole).Roleid , DirectCast(Container.DataItem, SiteUserRole).RoleDescription , DirectCast(Container.DataItem, SiteUserRole).AssigneeNetworkID ,DirectCast(Container.DataItem, SiteUserRole).BusUnit ,DirectCast(Container.DataItem, SiteUserRole).Area, DirectCast(Container.DataItem, SiteUserRole).Line) %>');"
                        'hyp.NavigateUrl = "Javascript:parent.window.location=('" & String.Format(Page.ResolveUrl("~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}"), HeaderItem.TaskHeaderSeqId, itemData(2)) & "');"
                        '_btnResetReportSelections.OnClientClick = "Javascript:window.location='" & Page.ResolveClientUrl(Page.AppRelativeVirtualPath) & "';return false"
                        '.OKScript = "return false;" '"Javascript:window.location='" & Page.ResolveClientUrl("~/ViewTasks.aspx?HeaderNumber=" & TaskHeaderNumber) & "'; return false;"
                        '_cbInactiveFlag.Attributes.Add("onClick", rowChangedJS & " if (this.checked==true){" & Replace(_btnInactiveFlag.TriggerPopupJS(String.Format(urltxt, employeerow.NetworkId, Me._ddlsitelist.SelectedValue, employeerow.FirstName, employeerow.LastName)), "return false", "return true") & " }")
                        If inactiveflag = "Y" Then
                            _cbInactiveFlag.Checked = True
                        Else
                            _cbInactiveFlag.Checked = False
                        End If
                    End If

                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError(" _gvEmployeeList_RowDataBound", , ex)
        End Try
    End Sub

    Protected Sub _ddlsitelist_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ddlsitelist.SelectedIndexChanged
        If Request.QueryString("MaintFlag") IsNot Nothing And Request.QueryString("MaintFlag") = "MAINT" Then
            _networkid.Text = ""
            _lastname.Text = ""
            _firstname.Text = ""
            _email.Text = ""
            _phone.Text = ""
            _midinit.Text = ""
            _lblEmployeeStatus.Visible = False
            PopulateEmployeeList(_ddlsitelist.SelectedItem.Value)
        End If
    End Sub

    Private Sub _cbShowHideInactiveUsers_CheckedChanged(sender As Object, e As EventArgs) Handles _cbShowHideInactiveUsers.CheckedChanged
        PopulateEmployeeList(_ddlsitelist.SelectedItem.Value)
    End Sub

    Private Sub _gvMyProfile_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles _gvMyProfile.RowDataBound
        Dim _ddldomain2 As DropDownList = e.Row.FindControl("_ddldomain2")
        Dim DomainList As System.Collections.Generic.List(Of Domain)
        Dim employeerow As Employee
        Dim defaultlanglist As System.Collections.Hashtable
        Dim _ddldefaultlang2 As DropDownList = e.Row.FindControl("_ddldefaultlang2")
        Dim defaultlang As String
        Dim _cbInactiveFlag As CheckBox = e.Row.FindControl("_cbInactiveFlag")

        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                DomainList = Session.Item("DomainList")
                employeerow = CType(e.Row.DataItem, Employee)
                defaultlanglist = CType(Session.Item("defaultlanglist"), Hashtable)

                If defaultlanglist IsNot Nothing AndAlso _ddldefaultlang2 IsNot Nothing Then
                    defaultlang = employeerow.DefaultLang

                    For Each item As DictionaryEntry In defaultlanglist
                        _ddldefaultlang2.Items.Add(New ListItem(item.Value, item.Key.ToString.ToUpper))
                    Next
                    If defaultlang.Length > 0 Then
                        If _ddldefaultlang2.Items.FindByValue(defaultlang.ToUpper) IsNot Nothing Then
                            _ddldefaultlang2.ClearSelection()
                            _ddldefaultlang2.Items.FindByValue(defaultlang.ToUpper).Selected = True
                        End If
                    Else
                        _ddldefaultlang2.Items.FindByValue("EN-US").Selected = True
                    End If
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError(" _gvEmployeeList_RowDataBound", , ex)
        End Try
    End Sub


    Protected Sub ButtonSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSearch.Click
        TextBoxEmailLookup.Text = "James"
        TextBoxEmailLookup.Text = "Refreshed at " + DateTime.Now.ToLongTimeString()

    End Sub

End Class
