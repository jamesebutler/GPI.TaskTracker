'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 02-21-2011
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class UserControlsEmployeeList2
    Inherits IP.Bids.UserControl.UserControlValidation
    Public Event UserChanged()
#Region "Fields"
    Private _PlantCode As String = ""
    Private _DefaultUserName As String = String.Empty
    Private _EmplyoeeLabel As String = "Employee List"
    Private _Visible As Boolean = True
    'Private _AllowUserRoles As Boolean = True
    Private _UserMode As UserModes
    Private _AllowFacilityChange As Boolean = True
    Private _Width As Unit = Unit.Pixel(400)
#End Region

#Region "Enum"
    Public Enum UserModes
        UsersAndRoles = 0
        UsersOnly = 1
        RolesOnly = 2
    End Enum
#End Region
#Region "Properties"
    Public Property CssClass As String = String.Empty

    Public Property Width() As WebControls.Unit
        Get
            Return Me._ddlResponsibleUser.Width
        End Get
        Set(ByVal value As WebControls.Unit)
            Me._ddlResponsibleUser.Width = value
        End Set
    End Property
    Public Property AllowFacilityChange() As Boolean
        Get
            Return _AllowFacilityChange
        End Get
        Set(ByVal value As Boolean)
            _AllowFacilityChange = value
        End Set
    End Property
    Public Property UserMode() As UserModes
        Get
            Return _UserMode
        End Get
        Set(ByVal value As UserModes)
            _UserMode = value
        End Set
    End Property
    'Public Property AllowUserRoles() As Boolean
    '    Get
    '        Return _AllowUserRoles
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _AllowUserRoles = value
    '    End Set
    'End Property

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
            Return "ResponsibleUser.ShowResponsibleUser(this);" 'this.onmousemove=ResponsibleUser.captureMousePosition;"
        End Get
    End Property

    Public Shadows Property Visible() As Boolean
        Get
            Return _Visible
        End Get
        Set(ByVal value As Boolean)
            _Visible = value
        End Set
    End Property
    Public Property Enabled() As Boolean
        Get
            Return Me._ddlResponsibleUser.Enabled
        End Get
        Set(ByVal value As Boolean)
            Me._ddlResponsibleUser.Enabled = value
            Me._btnFacilityList.Enabled = value
            Me._btnClearSelection.Enabled = value
        End Set
    End Property
    Public Property Employeelabel() As String
        Get
            Return _EmplyoeeLabel 'Me._lblEmployeeListCaption.Text
        End Get
        Set(ByVal value As String)
            _EmplyoeeLabel = value 'Me._lblEmployeeListCaption.Text = value
        End Set
    End Property
    Public Property DefaultUserName() As String
        Get
            Return _DefaultUserName
        End Get
        Set(ByVal value As String)
            _DefaultUserName = value
            If value.Length > 0 Then
                SelectCurrentUser()
            Else
                Me._ddlResponsibleUser.ClearSelection()
            End If
        End Set
    End Property

    Public Property SelectedValue() As String
        Get
            Return Me._ddlResponsibleUser.SelectedValue
        End Get
        Set(ByVal value As String)
            Me._ddlResponsibleUser.ClearSelection()
            If Me._ddlResponsibleUser.Items.FindByValue(value) IsNot Nothing Then
                Me._ddlResponsibleUser.Items.FindByValue(value).Selected = True
                'Me._ddlResponsibleUser.SelectedValue = value
                DefaultUserName = value
            End If
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

    ''' <summary>
    ''' Gets or sets the Plant Code based on the selected facility
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PlantCode() As String
        Get
            Return _PlantCode
        End Get
        Set(ByVal value As String)
            _PlantCode = value
            If _lbFacility.Items.Count = 0 Then PopulateSite()
            If PlantCode.Length > 0 AndAlso _lbFacility.Items.FindByValue(value) IsNot Nothing Then
                Me._lbFacility.ClearSelection()
                _lbFacility.Items.FindByValue(value).Selected = True
                Me._btnFacilityList.Text = _lbFacility.SelectedItem.Text
            Else
                If _lbFacility.Items.Count > 0 Then
                    Me._lbFacility.ClearSelection()
                    Me._lbFacility.SelectedIndex = 1
                    _PlantCode = _lbFacility.SelectedItem.Value
                End If
            End If
        End Set
    End Property
#End Region

#Region "Methods"
    Private Function GetGlobalJS() As String
        Dim sb As New StringBuilder
        With sb
            .Append("var btnFacilityList" & Me.ID & " = '" & Me._btnFacilityList.ClientID & "';")
        End With
        Return sb.ToString
    End Function

    Private Sub SelectCurrentUser()
        'If Me.DefaultUserName.Length > 0 Then
        Dim newPlantCode As String = String.Empty
        If Me._ddlResponsibleUser.Items.FindByValue(Me.DefaultUserName) IsNot Nothing Then
            Me._ddlResponsibleUser.ClearSelection()
            Me._ddlResponsibleUser.Items.FindByValue(Me.DefaultUserName).Selected = True
        Else
            If IsNumeric(DefaultUserName) Then 'role 
                'If PlantCode.Length > 0 Then
                '    Me.PopulateEmployeeList()
                'End If
            Else ' we have a user.  So lets get the users plant code
                Dim currentUser As IP.Bids.UserInfo = IP.Bids.SharedFunctions.GetCurrentUser
                Dim user As IP.Bids.UserInfo = Nothing
                If currentUser IsNot Nothing AndAlso currentUser.Username IsNot Nothing Then
                    If currentUser.Username.ToUpper <> DefaultUserName.ToUpper Then
                        'user = New IP.Bids.UserInfo("NAIPAPER", DefaultUserName)
                        user = New IP.Bids.UserInfo("NA", DefaultUserName)
                    Else
                        user = currentUser
                    End If
                End If

                If user IsNot Nothing Then
                    If user.UserDefaults IsNot Nothing Then
                        If user.UserDefaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.PlantCode) Then
                            newPlantCode = user.UserDefaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
                            user = Nothing
                        End If
                        'If user.UserDefaults.Item("Facility") IsNot Nothing Then
                        '    newPlantCode = user.UserDefaults.Item("Facility")
                        '    user = Nothing
                        'End If
                    End If
                End If
            End If
            If PlantCode <> newPlantCode And newPlantCode.Length > 0 And Me._ddlResponsibleUser.Items.Count > 0 Then
                PlantCode = newPlantCode
                PopulateEmployeeList()
            End If
        End If
        'End If


    End Sub
    Public Sub PopulateEmployeeList()
        Dim taskItem As New TaskTrackerListsBll
        'Dim plantCode As String = Me._ddlResponsibleFacility.SelectedValue
        'If Me._ddlResponsibleUser.Items.Count > 0 Then Exit Sub
        If PlantCode.Length = 0 Then
            PlantCode = "9998"
        End If
        Dim userList As System.Collections.Generic.List(Of ResponsibleUsers) = TaskTrackerListsBll.GetResponsibleUsers(PlantCode)
        Dim roleDescription As String = String.Empty
        Dim userIsSelected As Boolean
        _ddlResponsibleUser.Items.Clear()
        roleDescription = String.Empty
        If Me._lbFacility.SelectedIndex > 0 Then
            Me._btnFacilityList.Text = IP.Bids.SharedFunctions.LocalizeValue(_lbFacility.SelectedItem.Text)
        Else
            Me._btnFacilityList.Text = IP.Bids.SharedFunctions.LocalizeValue("Select Facility")
        End If
        If Me.AllowFacilityChange = True And Me.Enabled = True Then
            Me._btnFacilityList.Enabled = True
        Else
            Me._btnFacilityList.Enabled = False
        End If
        Dim spaceChar As String = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
        For Each item As ResponsibleUsers In userList
            If Me.UserMode = UserModes.UsersOnly Then
                'If Me.AllowUserRoles = False Then
                If item.RoleSeqID.Length > 0 Then
                    Continue For
                End If
            ElseIf Me.UserMode = UserModes.RolesOnly Then
                If item.RoleSeqID.Length = 0 Then
                    Continue For
                End If
            End If
            With _ddlResponsibleUser

                If item.RoleDescription <> roleDescription Then 'New Group
                    Dim roleItem As New ListItem
                    roleDescription = item.RoleDescription
                    roleItem.Text = IP.Bids.SharedFunctions.LocalizeValue(item.RoleDescription.ToUpper)
                    roleItem.Value = item.RoleSeqID

                    If .Items.Count > 0 Then
                        Dim blankItem As New ListItem
                        With blankItem
                            '.Attributes.Add("disabled", "true")
                            .Text = ""
                            .Value = "" '-1
                        End With
                        .Items.Add(blankItem)
                    End If
                    If item.RoleSeqID.Length = 0 Then
                        item.RoleSeqID = "-1"
                    End If
                    If roleDescription.Length > 0 And CInt(item.RoleSeqID) > 0 Then
                        'roleItem.Attributes.Add("optgroup", RoleDescription)

                        roleItem.Attributes.Add("style", "background-color:ActiveBorder; color:black; font-size:Larger;")
                        .Items.Add(roleItem)
                    Else
                        roleItem.Attributes.Add("style", "background-color:ActiveBorder; color:black;")
                        roleItem.Attributes.Add("disabled", "true")
                        .Items.Add(roleItem)
                    End If


                End If
                If UserMode = UserModes.UsersAndRoles Or UserMode = UserModes.UsersOnly Then
                    Dim useritem As New ListItem
                    With useritem

                        .Text = Server.HtmlDecode(spaceChar & IP.Bids.SharedFunctions.ToTitleCase(item.FullName.ToLower))
                        .Value = item.UserName
                        If item.UserName.ToUpper = Me.DefaultUserName.ToUpper Then
                            If Not userIsSelected Then
                                .Selected = True
                                'DefaultUser = IP.Bids.SharedFunctions.ToTitleCase(item.FullName.ToLower)
                                userIsSelected = True
                            End If
                        End If
                    End With
                    .Items.Add(useritem)
                End If
            End With
        Next
        _ddlResponsibleUser.Items.Insert(0, "")
        Me.SelectCurrentUser()
    End Sub

    Public Sub AddUser(ByVal userName As String, ByVal fullName As String)
        Dim spaceChar As String = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
        If Me._ddlResponsibleUser.Items.FindByValue(userName) Is Nothing Then
            fullName = IP.Bids.SharedFunctions.ToTitleCase(fullName.ToLower)
            Me._ddlResponsibleUser.Items.Add(New ListItem(Server.HtmlDecode(spaceChar & fullName), userName, True))
            Me._ddlResponsibleUser.ClearSelection()
            Me._ddlResponsibleUser.Items.FindByValue(userName).Selected = True
        End If
    End Sub

    Public Function ListOfClientIds() As Array
        Dim list() As String = {Me._btnClearSelection.ClientID, Me._ddlResponsibleUser.ClientID, Me._btnFacilityList.ClientID}
        Return list
    End Function

    Private Sub PopulateSite()
        Dim siteItem As New TaskTrackerSiteBll
        Dim facilityList As System.Collections.Generic.List(Of Facility) = siteItem.GetFacility("", "")
        _lbFacility.Items.Clear()
        For Each item As Facility In facilityList
            With Me._lbFacility
                If item.SiteName.Length > 0 Then
                    .Items.Add(New ListItem(item.SiteName, item.PlantCode))
                End If
            End With
        Next
        Me._lbFacility.Items.Insert(0, "")
    End Sub

    Private Sub HandlePageLoad()

        Try

            'Dim siteItem As New TaskTrackerSiteBLL
            'Dim FacilityList As System.Collections.Generic.List(Of Facility) = siteItem.GetFacility("", "")

            '**************************************************
            'Configure page to use a web service'
            Dim sc As ScriptManager
            sc = CType(Page.Form.FindControl("_scriptManager"), ScriptManager)
            If sc IsNot Nothing Then
                Dim loService As New ServiceReference
                loService.InlineScript = False
                loService.Path = "~/WebServices/SiteDropDownsWS.asmx"
                sc.Services.Add(loService)
            End If
            ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType, "EmployeeList_" & Me.ID, GetGlobalJS(), True)
            '***************************************************

            If Not Page.IsPostBack Or Me._lbFacility.Items.Count = 0 Then
                'For Each item As Facility In FacilityList
                '    With Me._lbFacility
                '        If item.SiteName.Length > 0 Then
                '            .Items.Add(New ListItem(item.SiteName, item.PlantCode))
                '        End If
                '    End With
                'Next
                'Me._lbFacility.Items.Insert(0, "")
                PopulateSite()
                If PlantCode.Length > 0 AndAlso _lbFacility.Items.FindByValue(PlantCode) IsNot Nothing Then
                    Me._lbFacility.ClearSelection()
                    _lbFacility.Items.FindByValue(PlantCode).Selected = True
                    Me._btnFacilityList.Text = _lbFacility.SelectedItem.Text
                Else
                    If _lbFacility.Items.Count > 0 Then
                        Me._lbFacility.ClearSelection()
                        Me._lbFacility.SelectedIndex = 1
                        PlantCode = _lbFacility.SelectedItem.Value
                    End If
                End If
            Else
                'reselect facility
                If Request(Me._lbFacility.UniqueID) IsNot Nothing Then
                    If _lbFacility.Items.FindByValue(Request(Me._lbFacility.UniqueID)) IsNot Nothing Then
                        _lbFacility.ClearSelection()
                        _lbFacility.Items.FindByValue(Request(Me._lbFacility.UniqueID)).Selected = True
                    End If
                End If
                If _lbFacility.SelectedIndex < 0 Then
                    Me._lbFacility.SelectedIndex = 0
                End If
                Me._btnFacilityList.Text = _lbFacility.SelectedItem.Text
                PlantCode = _lbFacility.SelectedItem.Value
                PopulateEmployeeList()
            End If
            If Request.Form(Me._ddlResponsibleUser.UniqueID) IsNot Nothing Then
                If Me._ddlResponsibleUser.Items.FindByValue(Request.Form(Me._ddlResponsibleUser.UniqueID)) IsNot Nothing Then
                    Me._ddlResponsibleUser.ClearSelection()
                    Me._ddlResponsibleUser.Items.FindByValue(Request.Form(Me._ddlResponsibleUser.UniqueID)).Selected = True
                End If
            End If

            _lbFacility.Attributes.Add("onChange", "this.blur;ResponsibleUser.populateResponsibleUsers(this,'" & Me._ddlResponsibleUser.ClientID & "','" & Me._btnFacilityList.ClientID & "','" & Me.UserMode & "');AjaxControlToolkit.PopupControlBehavior.__VisiblePopup.hidePopup();")
            _btnClearSelection.OnClientClick = "ResponsibleUser.ClearResponsibleUser('" & Me._ddlResponsibleUser.ClientID & "');return false;"
            'Me._btnClearUser.OnClientClick = "ResponsibleUser.ClearResponsibleUser();return false;"
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
            'If Not Page.IsPostBack OrElse Me.EnableViewState = False Then
            Me.PopulateEmployeeList()
        End If
    End Sub

    Protected Sub _lbFacility_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _lbFacility.SelectedIndexChanged

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me._lblEmployeeListCaption.Text = Me.Employeelabel
        Me._btnFacilityList.Visible = _Visible
        Me._ddlResponsibleUser.Visible = _Visible
        Me._lblEmployeeListCaption.Visible = _Visible
        _ddlResponsibleUser.CssClass = CssClass
        _lbFacility.CssClass = CssClass
    End Sub

    Protected Sub _ddlResponsibleUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ddlResponsibleUser.SelectedIndexChanged
        If Me.AutoPostBack Then
            RaiseEvent UserChanged()
        End If
    End Sub
End Class
