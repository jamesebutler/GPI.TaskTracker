'Option Explicit On
'Option Strict On
Imports System.Diagnostics

Namespace User_Controls

    Partial Class AdvancedEmployeeListDropdown
        Inherits IP.Bids.UserControl.UserControlValidation
        Public Event UserChanged(ByVal sender As Object, ByVal args As EventArgs)

        Dim FilePath As String = ""



#Region "Fields"
        Private _plantCode As String = ""
        Const AllPlantCode As String = "9998"



#End Region

#Region "Enum"
        Public Enum UserModes
            UsersAndRoles = 0
            UsersOnly = 1
            RolesOnly = 2
        End Enum
#End Region

#Region "Properties"
        Public Property CssClass As String = "form-control input-sm selectpicker"
        Public Property UseOnDemandListPopulation As Boolean = False
        Public Property ShowInactiveUsers As Boolean = False
        Public Property DisplayClearLink() As Boolean = True
        Public Property FacilityOnChange() As String = String.Empty

        Public ReadOnly Property EmployeeListUniqueId() As String
            Get
                Return _ddlResponsibleUser.UniqueID
            End Get
        End Property
        Public Property Width() As WebControls.Unit
            Get
                Return Me._ddlResponsibleUser.Width
            End Get
            Set(ByVal value As WebControls.Unit)
                Me._ddlResponsibleUser.Width = value
            End Set
        End Property
        Public Property AllowFacilityChange() As Boolean = True

        Public Property UserMode() As UserModes

        Public Property AutoPostBack() As Boolean
            Get
                Return Me._ddlResponsibleUser.AutoPostBack
            End Get
            Set(ByVal value As Boolean)
                Me._ddlResponsibleUser.AutoPostBack = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the javascript that is used to display the Employee List picker
        ''' </summary>
        ''' <value></value>
        ''' <returns>String - javascript syntax that will be used on the onClick event to display the Employee List Picker</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GetShowUserJS() As String
            Get
                Return "ResponsibleUser.ShowResponsibleUser(this);"
            End Get
        End Property

        Public Property Enabled() As Boolean
            Get
                Return Me._ddlResponsibleUser.Enabled
            End Get
            Set(ByVal value As Boolean)
                If value = True Then
                    EnableControls()
                Else
                    DisableControls()
                End If
            End Set
        End Property

        Public Property Employeelabel() As String = "Employee List"

        Public Property DefaultUserName() As String = String.Empty

        Public Property SelectedValue() As String
            Get
                Return Me._ddlResponsibleUser.SelectedValue
            End Get
            Set(ByVal value As String)
                DefaultUserName = value
                SetSelectedEmployee()
            End Set
        End Property

        Public ReadOnly Property SelectedText() As String
            Get
                If Me._ddlResponsibleUser.SelectedItem IsNot Nothing Then
                    Return Me._ddlResponsibleUser.SelectedItem.Text
                Else
                    Return ""
                End If
            End Get
        End Property

        Public Property PlantCode() As String
            Get
                Return _plantCode
            End Get
            Set(ByVal value As String)
                _plantCode = value
                SetPlantCode(value)
            End Set
        End Property
#End Region 'Properties

#Region "Methods"
        Private Sub SetPlantCode(value As String)
            Try
                If _lbFacility.Items.Count = 0 Then PopulateSite()
                If PlantCode.Length > 0 AndAlso _lbFacility.Items.FindByValue(value) IsNot Nothing Then
                    Me._lbFacility.ClearSelection()
                    _lbFacility.Items.FindByValue(value).Selected = True
                    SetFacilityButtonText()
                Else
                    If _lbFacility.Items.Count > 0 Then
                        Me._lbFacility.ClearSelection()
                        Me._lbFacility.SelectedIndex = 1
                        PlantCode = _lbFacility.SelectedItem.Value
                    End If
                End If
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("SetPlantCode", , ex)
            End Try
        End Sub

        Private Sub EnableControls()
            Me._ddlResponsibleUser.Enabled = True
            Me._btnFacilityList.Enabled = True
            Me._btnClearSelection.Enabled = True
            Me._btnSearch.Enabled = True
            _pceUsers.Enabled = True
        End Sub

        Private Sub DisableControls()
            Me._ddlResponsibleUser.Enabled = False
            Me._btnFacilityList.Enabled = False
            Me._btnClearSelection.Enabled = False
            Me._btnSearch.Enabled = False
            Me._btnClearSelection.OnClientClick = ""
            _pceUsers.Enabled = False
        End Sub

        Private Function GetGlobalJS() As String
            Return "var btnFacilityList" & Me.ID & " = '" & Me._btnFacilityList.ClientID & "';"
        End Function

        Private Sub SetSelectedEmployee()
            Try
                Me._ddlResponsibleUser.ClearSelection()
                If Me.DefaultUserName.ToLower <> _ddlResponsibleUser.SelectedValue.ToLower Then
                    If Me._ddlResponsibleUser.Items.FindByValue(Me.DefaultUserName) IsNot Nothing Then
                        If DefaultUserName.Length > 0 Then
                            Me._ddlResponsibleUser.Items.FindByValue(Me.DefaultUserName).Selected = True
                            Dim l As ListItem = Me._ddlResponsibleUser.Items.FindByValue(Me.DefaultUserName)
                            Me._ddlResponsibleUser.SelectedValue = l.Value
                        Else
                            Me._ddlResponsibleUser.SelectedIndex = -1
                        End If
                    End If
                End If
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("SetSelectedEmployee", , ex)
            End Try
        End Sub

        ''' <summary>
        ''' Selects the current user.
        ''' </summary>
        Private Sub SelectCurrentUser()
            Dim newPlantCode As String = String.Empty
            Try
                If Me._ddlResponsibleUser.Items.FindByValue(Me.DefaultUserName) IsNot Nothing AndAlso Me.DefaultUserName.Length > 0 Then
                    SetSelectedEmployee()
                Else
                    If IsNumeric(DefaultUserName) Then 'role 
                        If PlantCode = AllPlantCode Then 'All Roles 
                            Me.PopulateAllRoles()
                            SetSelectedEmployee()
                        End If
                    Else ' we have a user.  So lets get the users plant code
                        If DefaultUserName.Length = 0 Then Exit Sub
                        Dim user As IP.Bids.UserInfo = New IP.Bids.UserInfo("", DefaultUserName)

                        If user IsNot Nothing Then
                            newPlantCode = user.PlantCode
                        End If
                        'End If
                        If PlantCode <> newPlantCode And newPlantCode.Length > 0 And Me._ddlResponsibleUser.Items.Count > 0 Then
                            PlantCode = newPlantCode
                            PopulateEmployeeList()
                        End If
                    End If
                End If
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("SelectCurrentUser", , ex)
            End Try
        End Sub

        Private Sub PopulateAllRoles()
            Try
                Dim allroleList As System.Collections.Generic.List(Of Role) = DataMaintenanceBLL.GetAllRoles()
                _ddlResponsibleUser.Items.Clear()
                For Each item As Role In allroleList.Where(Function(obj) obj.RoleDescription.Length > 0)
                    With Me._ddlResponsibleUser
                        .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.RoleDescription, True), item.RoleId))
                    End With
                Next
                Me._btnFacilityList.Visible = False
                Me._btnSearch.Visible = False
                _pceUsers.Enabled = False
                Me._ddlResponsibleUser.Items.Insert(0, "")
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("PopulateAllRoles", , ex)
            End Try
        End Sub

        Private Sub PopulateSiteRoles()
            Try
                Dim allroleList As System.Collections.Generic.List(Of SiteUserRole) = DataMaintenanceBLL.GetSiteUserRoleList(PlantCode)
                _ddlResponsibleUser.Items.Clear()
                For Each item As SiteUserRole In allroleList.Where(Function(obj) obj.RoleDescription.Length > 0)
                    With Me._ddlResponsibleUser
                        .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.RoleDescription, True), item.RoleId))
                    End With
                Next
                Me._btnFacilityList.Visible = False
                Me._btnSearch.Visible = False
                _pceUsers.Enabled = False
                Me._ddlResponsibleUser.Items.Insert(0, "")
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("PopulateAllRoles", , ex)
            End Try
        End Sub

        Public Sub PopulateEmployeeList(ByVal selectedUserName As String)
            DefaultUserName = selectedUserName
            PopulateEmployeeList()
        End Sub

        Private Sub SetFacilityButtonText()
            Me._btnFacilityList.Text = Left(_lbFacility.SelectedItem.Text, 15)

            If _lbFacility.SelectedItem.Text.Length > 15 Then
                Me._btnFacilityList.Text = Me._btnFacilityList.Text & "..."
            End If
            _btnFacilityList.ToolTip = _lbFacility.SelectedItem.Text
        End Sub

        Public Sub PopulateEmployeeList()

            FilePath = HttpContext.Current.Server.MapPath("~\\TraceLog\\")

            If PlantCode.Length = 0 Then
                If IsNumeric(DefaultUserName) Then
                    PlantCode = AllPlantCode
                Else
                    If DefaultUserName.Length > 0 Then
                        Dim user As IP.Bids.UserInfo = New IP.Bids.UserInfo("", DefaultUserName)
                        If user IsNot Nothing Then
                            PlantCode = user.PlantCode

                            If PlantCode.Length > 0 And PlantCode = user.PlantCode AndAlso _lbFacility.Items.FindByValue(PlantCode) IsNot Nothing Then
                                Me._lbFacility.ClearSelection()
                                _lbFacility.Items.FindByValue(PlantCode).Selected = True
                                _lbFacility.SelectedValue = PlantCode
                                SetFacilityButtonText()
                            End If
                        End If
                        user = Nothing
                    End If
                End If
            End If
            If UserMode = UserModes.RolesOnly Then
                PopulateAllRoles()
                Me.SelectCurrentUser()
                Exit Sub
            End If
            If PlantCode = AllPlantCode Then
                PopulateAllRoles()
                Me.SelectCurrentUser()
                Exit Sub
            End If

            If Me._lbFacility.SelectedIndex > 0 Then
                SetFacilityButtonText()
            Else
                'JEB
                Me._btnFacilityList.Text = IP.Bids.SharedFunctions.LocalizeValue("Select Facility")
                Me._btnFacilityList.ToolTip = IP.Bids.SharedFunctions.LocalizeValue("Select Facility")
            End If
            If Me.AllowFacilityChange = True And Me.Enabled = True Then
                'JEB
                Me._btnFacilityList.Enabled = True

            Else
                'JEB
                Me._btnFacilityList.Visible = False
                Me._btnSearch.Visible = False
                _pceUsers.Enabled = False

            End If
            _ddlResponsibleUser.Items.Clear()
            Dim collectionOfUsers As New ListItemCollection
            'collectionOfUsers = AddItemsToListControl()
            Dim roleToAdd As New ListItem
            If IsNumeric(DefaultUserName) Then
                Dim siteRoleList As System.Collections.Generic.List(Of SiteUserRole) = DataMaintenanceBLL.GetSiteUserRoleList(PlantCode)
                Dim roleQuery = From selectedRole As SiteUserRole In siteRoleList
                                Where selectedRole.RoleId = DefaultUserName Or DefaultUserName.Length = 0
                                Select New Role(selectedRole.RoleId, selectedRole.RoleName, selectedRole.RoleDescription, "", "Y")
                If UseOnDemandListPopulation = False Then
                    collectionOfUsers = AddItemsToListControl()
                    collectionOfUsers = ClearSelectedUser(collectionOfUsers)
                End If

                If roleQuery.Count > 0 Then
                    For Each item As Role In roleQuery
                        If collectionOfUsers.FindByValue(item.RoleId) Is Nothing Then
                            roleToAdd.Value = item.RoleId
                            roleToAdd.Text = item.RoleDescription
                            'collectionOfUsers.Insert(1, New ListItem(item.RoleDescription, item.RoleId))
                        End If
                    Next
                Else
                    Dim allroleList As System.Collections.Generic.List(Of Role) = DataMaintenanceBLL.GetAllRoles()
                    Dim allRoleQuery = From selectedRole As Role In allroleList
                                       Where selectedRole.RoleId = DefaultUserName Or DefaultUserName.Length = 0
                                       Select New Role(selectedRole.RoleId, selectedRole.RoleName, selectedRole.RoleDescription, "", "Y")
                    If allRoleQuery.Count > 0 Then
                        For Each item As Role In allRoleQuery
                            If collectionOfUsers.FindByValue(item.RoleId) Is Nothing Then
                                roleToAdd.Value = item.RoleId
                                roleToAdd.Text = item.RoleDescription
                                'collectionOfUsers.Insert(1, New ListItem(item.RoleDescription, item.RoleId))
                            End If
                        Next
                    End If
                    '
                End If

            ElseIf DefaultUserName.Length > 0 Then

                If UseOnDemandListPopulation = False Then
                    collectionOfUsers = AddItemsToListControl()
                    collectionOfUsers = ClearSelectedUser(collectionOfUsers)
                End If
                'Dim user As IP.Bids.UserInfo = New IP.Bids.UserInfo("", DefaultUserName)
                'If collectionOfUsers.FindByValue(user.Username) Is Nothing Then
                '    Dim spaceChar As String = "      "
                '    collectionOfUsers.Insert(1, New ListItem(spaceChar & user.LastName & ", " & user.FirstName & " (" & user.Username & ")", user.Username))
                'End If
            Else
                    collectionOfUsers.Insert(0, "")
                If UseOnDemandListPopulation = False Then
                    collectionOfUsers = AddItemsToListControl()
                    collectionOfUsers = ClearSelectedUser(collectionOfUsers)
                End If
            End If

            _ddlResponsibleUser.Items.Clear()

            Dim NewGuid As Guid
            NewGuid = Guid.NewGuid

            For Each item As ListItem In collectionOfUsers ' responsibleQuery.ToList 'Distinct
                'IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "PopulateEmployeeListcollectionOfUsersLoop.txt", "collectionOfUsers Start : " & NewGuid.ToString & " - " & DateTime.Now.ToString, True)

                With _ddlResponsibleUser
                    .Items.Add(New ListItem(item.Text, item.Value))

                    'IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "PopulateEmployeeListcollectionOfUsersLoop.txt", item.Text, False)
                    If (item.Attributes.Count > 0) Then
                        For Each att As String In item.Attributes.Keys
                            item.Attributes.Add(att, item.Attributes.Item(att))
                        Next
                    End If
                End With
                ' IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "PopulateEmployeeListcollectionOfUsersLoop.txt", "collectionOfUsers End : " & NewGuid.ToString & " - " & DateTime.Now.ToString, False)

            Next




            If IsNumeric(DefaultUserName) Then
                If _ddlResponsibleUser.Items.FindByValue(roleToAdd.Value) Is Nothing Then
                    _ddlResponsibleUser.Items.Insert(1, New ListItem(roleToAdd.Text, roleToAdd.Value))
                End If
            Else
                Dim user As IP.Bids.UserInfo = New IP.Bids.UserInfo("", DefaultUserName)
                If _ddlResponsibleUser.Items.FindByValue(user.Username) Is Nothing Then
                    Dim spaceChar As String = "      "
                    _ddlResponsibleUser.Items.Insert(1, New ListItem(spaceChar & user.LastName & ", " & user.FirstName & " (" & user.Username & ")", user.Username))
                End If

            End If

            Me.SelectCurrentUser()

            'IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "PopulateEmployeeList.txt", "PopulateEmployeeList END : " & DateTime.Now.ToString, False)

        End Sub

        Private Function AddItemsToListControl() As ListItemCollection
            Dim userList As System.Collections.Generic.List(Of ResponsibleUsers)
            Dim roleDescription As String = String.Empty
            Dim spaceChar As String = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
            Dim collectionOfUsers As ListItemCollection = Nothing
            Dim cacheKey As String = "ResponsibleUsers_" & PlantCode & "_" & Me.UserMode & "_" & ShowInactiveUsers.ToString
            Dim cacheHours As Integer = 1

            Dim cacheKeyUserList As String = "userList_" & PlantCode & "_" & Me.UserMode & "_" & ShowInactiveUsers.ToString
            Dim cacheHoursUserList As Integer = 1

            collectionOfUsers = CType(HelperDal.GetDataFromCache(cacheKey, cacheHours), ListItemCollection)
            If collectionOfUsers IsNot Nothing AndAlso collectionOfUsers.Count > 0 Then
                Return collectionOfUsers
                Exit Function
            End If

            userList = HelperDal.GetDataFromCache(cacheKeyUserList, cacheHoursUserList)

            If userList IsNot Nothing Then

            Else
                'IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "AddItemsToListControl.txt", "collectionOfUsers Start : " & DateTime.Now.ToString, True)

                userList = TaskTrackerListsBll.GetResponsibleUsers(PlantCode)
            End If



            'Session("userListCount") = 1

            'Dim SessionVariableName As String
            'Dim SessionVariableValue As String

            'For Each SessionVariable As String In Session.Keys
            '    ' Find Session variable name
            '    SessionVariableName = SessionVariable
            '    ' Find Session variable value
            '    SessionVariableValue = Session(SessionVariableName).ToString()

            '    ' Do something with this data


            'Next

            'Try



            '    If Session("userListCount") = Nothing Then
            '        userList = TaskTrackerListsBll.GetResponsibleUsers(PlantCode)
            '        Session("userList") = userList


            '    Else
            '        userList = Session("userList")
            '    End If

            'Catch ex As Exception
            '    Debug.Print(ex.Message.ToString)
            '    userList = TaskTrackerListsBll.GetResponsibleUsers(PlantCode)
            '    'Session("userList") = userList
            'End Try


            collectionOfUsers = New ListItemCollection
            For Each item As ResponsibleUsers In userList
                If ShowInactiveUsers = False Then
                    If item.InActiveFlag.ToLower.Trim = "y" Then Continue For
                End If
                If Me.UserMode = UserModes.UsersOnly Then
                    'If Me.AllowUserRoles = False Then
                    If item.RoleSeqID <> "-1" And item.RoleSeqID.Length > 0 Then
                        Continue For
                    End If
                ElseIf Me.UserMode = UserModes.RolesOnly Then
                    If item.RoleSeqID.Length = 0 Then
                        Continue For
                    End If
                End If
                With collectionOfUsers '_ddlResponsibleUser
                    If item.RoleDescription <> roleDescription Then 'New Group
                        Dim roleItem As New ListItem
                        roleDescription = item.RoleDescription
                        roleItem.Text = IP.Bids.SharedFunctions.LocalizeValue(item.RoleDescription.ToUpper)
                        roleItem.Value = item.RoleSeqID

                        If .Count = 1 Then
                            Dim blankItem As New ListItem
                            With blankItem
                                '.Attributes.Add("disabled", "true")
                                .Text = ""
                                .Value = "" '-1
                                .Selected = False
                            End With
                            .Add(blankItem)
                        End If
                        If item.RoleSeqID.Length = 0 Then
                            item.RoleSeqID = "-1"
                        End If
                        If roleDescription.Length > 0 And CInt(item.RoleSeqID) > 0 Then
                            'roleItem.Attributes.Add("optgroup", RoleDescription)

                            roleItem.Attributes.Add("style", "color:black; font-size:Larger; background-color:lightgray")
                            roleItem.Attributes.Add("data-icon", "glyphicon-user")
                            .Add(roleItem)
                            roleItem.Selected = False
                        Else
                            roleItem.Attributes.Add("style", "color:black; font-size:Larger; background-color:lightgray")
                            roleItem.Attributes.Add("disabled", "true")
                            roleItem.Selected = False
                            .Add(roleItem)
                        End If


                    End If
                    If UserMode = UserModes.UsersAndRoles Or UserMode = UserModes.UsersOnly Then
                        Dim useritem As New ListItem
                        With useritem
                            .Text = HttpContext.Current.Server.HtmlDecode(spaceChar & IP.Bids.SharedFunctions.ToTitleCase(item.FullName.ToLower) & " (" & item.UserName & ")")
                            .Value = item.UserName
                            .Selected = False
                            'If item.UserName.ToUpper = Me.DefaultUserName.ToUpper Then
                            '    If Not userIsSelected Then                                    
                            '        .Selected = True
                            '        'DefaultUser = IP.Bids.SharedFunctions.ToTitleCase(item.FullName.ToLower)
                            '        userIsSelected = True
                            '    End If
                            'End If
                        End With
                        .Add(useritem)
                    End If
                End With
            Next


            HelperDal.InsertDataIntoCache(cacheKeyUserList, cacheHoursUserList, userList)

            'If collectionOfUsers.FindByValue("") Is Nothing Then
            collectionOfUsers.Insert(0, "")
            'End If
            collectionOfUsers = ClearSelectedUser(collectionOfUsers)
            'HelperDal.InsertDataIntoCache(cacheKey, cacheHours, collectionOfUsers)

            HelperDal.InsertDataIntoCache(cacheKey, cacheHours, collectionOfUsers)

            Return collectionOfUsers



        End Function

        Private Function ClearSelectedUser(ByVal collectionOfUsers As ListItemCollection) As ListItemCollection
            For Each item As ListItem In collectionOfUsers
                item.Selected = False
            Next
            Return collectionOfUsers
        End Function

        Public Sub AddUser(ByVal userName As String, ByVal fullName As String)
            Dim spaceChar As String = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
            If Me._ddlResponsibleUser.Items.FindByValue(userName) Is Nothing Then
                fullName = IP.Bids.SharedFunctions.ToTitleCase(fullName.ToLower)
                If fullName.Length = 0 Then

                End If
                Me._ddlResponsibleUser.Items.Add(New ListItem(HttpContext.Current.Server.HtmlDecode(spaceChar & fullName), userName, True))
                DefaultUserName = userName
                SetSelectedEmployee()
            End If
        End Sub

        Public Function ListOfClientIds() As Array
            Dim list() As String = {Me._btnClearSelection.ClientID, Me._ddlResponsibleUser.ClientID, Me._btnFacilityList.ClientID}
            Return list
        End Function

        Private Sub PopulateSite()
            Dim siteItem As New TaskTrackerSiteBll
            Dim facilityList As System.Collections.Generic.List(Of Facility) = siteItem.GetSiteList 'GetFacility("", "")
            _lbFacility.Items.Clear()
            For Each item As Facility In facilityList
                With Me._lbFacility
                    If item.SiteName.Length > 0 Then
                        .Items.Add(New ListItem(item.SiteName, item.PlantCode))
                    End If
                End With
            Next
            'Me._lbFacility.Items.Insert(0, New ListItem("-- Search List --", "?", True))
            Me._lbFacility.Items.Insert(0, "")
        End Sub

        Private Sub HandlePageLoad()

            Try

                'Dim siteItem As New TaskTrackerSiteBLL
                'Dim FacilityList As System.Collections.Generic.List(Of Facility) = siteItem.GetFacility("", "")

                '**************************************************
                'Configure page to use a web service'
                'Dim sc As ScriptManager
                'sc = CType(Page.Form.FindControl("_scriptManager"), ScriptManager)
                'If sc IsNot Nothing Then
                '    Dim loService As New ServiceReference
                '    loService.InlineScript = False
                '    loService.Path = "~/WebServices/SiteDropDownsWS.asmx"
                '    sc.Services.Add(loService)
                'End If
                'ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType, "EmployeeList_" & Me.ID, GetGlobalJS(), True)
                ''***************************************************

                If Not Page.IsPostBack Then 'Or Me._lbFacility.Items.Count = 0 Then                 
                    PopulateSite()
                    If PlantCode.Length > 0 AndAlso _lbFacility.Items.FindByValue(PlantCode) IsNot Nothing Then
                        Me._lbFacility.ClearSelection()
                        _lbFacility.Items.FindByValue(PlantCode).Selected = True
                        _lbFacility.SelectedValue = PlantCode
                        SetFacilityButtonText()
                    Else
                        If _lbFacility.Items.Count > 0 Then
                            Me._lbFacility.ClearSelection()
                            Me._lbFacility.SelectedIndex = 1
                            Me._btnFacilityList.ToolTip = _lbFacility.SelectedItem.Text
                            PlantCode = _lbFacility.SelectedItem.Value
                        End If
                    End If
                Else
                    If _lbFacility.Items.Count = 0 Then PopulateSite()
                    'reselect facility
                    If Request(Me._lbFacility.UniqueID) IsNot Nothing Then
                        If _lbFacility.Items.FindByValue(Request(Me._lbFacility.UniqueID)) IsNot Nothing Then
                            _lbFacility.ClearSelection()
                            _lbFacility.Items.FindByValue(Request(Me._lbFacility.UniqueID)).Selected = True
                            _lbFacility.SelectedValue = Request(Me._lbFacility.UniqueID)
                        End If
                    End If
                    If _lbFacility.SelectedIndex < 0 Then
                        Me._lbFacility.SelectedIndex = 0
                    End If
                    SetFacilityButtonText()
                    PlantCode = _lbFacility.SelectedItem.Value
                    PopulateEmployeeList()
                End If
                If Request.Form(Me._ddlResponsibleUser.UniqueID) IsNot Nothing Then
                    'If Me._ddlResponsibleUser.Items.FindByValue(Request.Form(Me._ddlResponsibleUser.UniqueID)) IsNot Nothing Then
                    '    DefaultUserName = Request.Form(Me._ddlResponsibleUser.UniqueID)
                    '    SetSelectedEmployee()
                    'Else
                    DefaultUserName = Request.Form(Me._ddlResponsibleUser.UniqueID)
                    PopulateEmployeeList(DefaultUserName)
                    'End If
                End If
                _pceUsers.BehaviorID = Me.ID & "_bhUsers"
                _btnFacilityList.OnClientClick = "$('#" & Me._lbFacility.ClientID & "').selectpicker('show');$('#" & Me._lbFacility.ClientID & "').selectpicker('toggle');return false"
                _lbFacility.Attributes.Add("onChange", "ResponsibleUser.populateResponsibleUsers(this,'" & Me._ddlResponsibleUser.ClientID & "','" & Me._btnFacilityList.ClientID & "','" & Me.UserMode & "','" & ShowInactiveUsers.ToString & "'); ResponsibleUser.HidePopup('" & _pceUsers.BehaviorID & "');$('#" & Me._ddlResponsibleUser.ClientID & "').selectpicker('refresh');$('#" & Me._ddlResponsibleUser.ClientID & "').click();$('#" & Me._lbFacility.ClientID & "').selectpicker('hide');" & FacilityOnChange)
                '_lbFacility.Attributes.Add("onChange", "this.blur;ResponsibleUser.populateResponsibleUsers(this,'" & Me._ddlResponsibleUser.ClientID & "','" & Me._btnFacilityList.ClientID & "','" & Me.UserMode & "','" & ShowInactiveUsers.ToString & "'); ResponsibleUser.HidePopup('" & _pceUsers.BehaviorID & "');$('#" & Me._ddlResponsibleUser.ClientID & "').selectpicker('refresh')")
                _btnClearSelection.OnClientClick = "ResponsibleUser.ClearResponsibleUser('" & Me._ddlResponsibleUser.ClientID & "');return false;"

                _txtSearchBox.Attributes.Add("onkeyup", String.Format("ResponsibleUser.PerformNameSearch(this.value,'{0}');", _lbSearchResults.ClientID))
                _btnCancel.OnClientClick = String.Format("ResponsibleUser.HidePopup('{0}');return false;", _pceUsers.BehaviorID)
                _lbSearchResults.Attributes.Add("onClick", String.Format("ResponsibleUser.HidePopup('{0}');ResponsibleUser.SetUserAndFacility('{1}','{2}','{3}','{4}');", _pceUsers.BehaviorID, _btnFacilityList.ClientID, _lbFacility.ClientID, _ddlResponsibleUser.ClientID, _lbSearchResults.ClientID))

                ''Populate users on demand
                If UseOnDemandListPopulation Then
                    _ddlResponsibleUser.Attributes.Add("onClick", "ResponsibleUser.populateResponsibleUsersOnDemand($get('" & _lbFacility.ClientID & "'),'" & Me._ddlResponsibleUser.ClientID & "','" & Me._btnFacilityList.ClientID & "','" & Me.UserMode & "','" & ShowInactiveUsers.ToString & "');$('#" & Me._ddlResponsibleUser.ClientID & "').selectpicker('refresh');")
                End If
                ''End Populate users on demand
                'Dim filterDuplicateItemsJs As String = "var usedNames = {};$(""select[id='" & Me._ddlResponsibleUser.ClientID & "'] > option"").each(function () {if(usedNames[this.text]) {$(this).remove();} else {usedNames[this.text] = this.value;}});"
                'ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType, "FilterList_" & Me.ID, filterDuplicateItemsJs, True)

                If Me.EnableValidation = True Then
                    _lblRequiredField.Visible = True
                Else
                    _lblRequiredField.Visible = False
                End If
            Catch ex As Exception

            End Try
        End Sub

#End Region

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Me.ControlToValidate = Me._ddlResponsibleUser.ID
            Me.ClientValidationFunction = "ResponsibleUser.ResponsibleUserIsSelected"
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            HandlePageLoad()
            If Me.EnableViewState = False Or Me._ddlResponsibleUser.Items.Count = 0 Then
                Me.PopulateEmployeeList()
            End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Me._lblEmployeeListCaption.Text = Me.Employeelabel
            Me._btnFacilityList.Visible = Visible
            Me._ddlResponsibleUser.Visible = Visible
            Me._lblEmployeeListCaption.Visible = Visible
            Me._btnSearch.Visible = Visible
            If Visible = False Then
                Me._btnClearSelection.Visible = Visible
            Else
                Me._btnClearSelection.Visible = DisplayClearLink
            End If
            If UserMode = UserModes.RolesOnly Then
                Me._btnFacilityList.Visible = False
                Me._btnSearch.Visible = False
                _pceUsers.Enabled = False
            End If
            If _ddlResponsibleUser.Width.Value = 0 Then
                _ddlResponsibleUser.Width = Unit.Pixel(240)
            End If
            If _lblEmployeeListCaption.Text.Length = 0 Then
                _plhUserTop.Controls.Add(_btnSearch)

                If _btnSearch.Visible = True Then
                    _plhUserTop.Controls.Add(New LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"))
                End If

                _plhUserTop.Controls.Add(_btnFacilityList)
                _plhUserTop.Controls.Add(New LiteralControl("</br>"))
            Else
                _plhUserTop.Controls.Add(_lblEmployeeListCaption)
                _plhUserTop.Controls.Add(New LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"))
                _plhUserTop.Controls.Add(_btnFacilityList)
                _plhUserTop.Controls.Add(New LiteralControl("</br>"))
                _plhUserBottom.Controls.Add(New LiteralControl("</br>"))
                _plhUserBottom.Controls.Add(_btnSearch)
                If _btnSearch.Visible = True Then
                    _plhUserBottom.Controls.Add(New LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"))
                End If
                _plhUserBottom.Controls.Add(_btnClearSelection)
            End If

            _ddlResponsibleUser.CssClass = CssClass
        End Sub

        Protected Sub ResponsibleUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ddlResponsibleUser.SelectedIndexChanged
            If Me.AutoPostBack Then
                RaiseEvent UserChanged(sender, e)
            End If
        End Sub
    End Class

End Namespace
