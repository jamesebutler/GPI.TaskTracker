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
Namespace User_Controls

    Partial Class EmployeeSearch
        Inherits System.Web.UI.UserControl
        'Inherits IP.Bids.UserControl.UserControlValidation
        Public Event UserChanged(ByVal sender As Object, ByVal args As System.EventArgs)
#Region "Fields"
        Private _plantCode As String = ""
        Private _defaultUserName As String = String.Empty
        Private _emplyoeeLabel As String = "Employee List"
        Private _visible As Boolean = True
        Private _userMode As UserModes
        Private _allowFacilityChange As Boolean = True
        Private _width As Unit = Unit.Pixel(400)
        Private _showDropdown As Boolean = True
        Private _targetControlId As String = String.Empty
#End Region

#Region "Enum"
        Public Enum UserModes
            UsersAndRoles = 0
            UsersOnly = 1
            RolesOnly = 2
        End Enum
#End Region

#Region "Properties"
        <IDReferenceProperty(GetType(UserControl))> _
        Public Property TargetControlId() As String
            Get
                Return _targetControlId
            End Get
            Set(ByVal value As String)
                _targetControlId = value
            End Set
        End Property

        Public Property ShowDropdown() As Boolean
            Get
                Return _showDropdown
            End Get
            Set(ByVal value As Boolean)
                _showDropdown = value
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
                Return _visible
            End Get
            Set(ByVal value As Boolean)
                _visible = value
            End Set
        End Property

        Public Property Enabled() As Boolean
            Get
                Return _btnSearch.Enabled
            End Get
            Set(ByVal value As Boolean)
                Me._btnClearSelection.Enabled = value
                Me._btnSearch.Enabled = value
                'Me._lblResponsibleUser. = value
                If value = False Then
                    Me._btnClearSelection.OnClientClick = ""
                End If
                _pceUsers.Enabled = value
            End Set
        End Property

        Public Property Employeelabel() As String
            Get
                Return _emplyoeeLabel 'Me._lblEmployeeListCaption.Text
            End Get
            Set(ByVal value As String)
                _emplyoeeLabel = value 'Me._lblEmployeeListCaption.Text = value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return _lblResponsibleUser.Text
            End Get
            Set(ByVal value As String)
                _lblResponsibleUser.Text = value
            End Set
        End Property

        Public Property SelectedValue() As String
            Get
                Return _txtResponsibleUserValue.Text
            End Get
            Set(ByVal value As String)
                _txtResponsibleUserValue.Text = value
            End Set
        End Property
        'Public Property SelectedValue() As String
        '    Get
        '        Return Me._ddlResponsibleUser.SelectedValue
        '    End Get
        '    Set(ByVal value As String)
        '        Me._ddlResponsibleUser.ClearSelection()
        '        If Me._ddlResponsibleUser.Items.FindByValue(value) IsNot Nothing Then
        '            Me._ddlResponsibleUser.Items.FindByValue(value).Selected = True
        '            'Me._ddlResponsibleUser.SelectedValue = value
        '            DefaultUserName = value
        '        End If
        '    End Set
        'End Property

        'Public ReadOnly Property SelectedText() As String
        '    Get
        '        If Me._ddlResponsibleUser.SelectedItem IsNot Nothing Then
        '            Return Me._ddlResponsibleUser.SelectedItem.Text
        '        Else
        '            Return ""
        '        End If

        '    End Get

        'End Property
#End Region

#Region "Methods"
        Private Sub HandlePageLoad()
            Try
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
                'ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType, "EmployeeList_" & Me.ID, GetGlobalJS(), True)
                '***************************************************

                If Not Page.IsPostBack Then
                    'For Each item As Facility In FacilityList
                    '    With Me._lbFacility
                    '        If item.SiteName.Length > 0 Then
                    '            .Items.Add(New ListItem(item.SiteName, item.PlantCode))
                    '        End If
                    '    End With
                    'Next
                    'Me._lbFacility.Items.Insert(0, "")
                    'PopulateSite()
                    'If PlantCode.Length > 0 AndAlso _lbFacility.Items.FindByValue(PlantCode) IsNot Nothing Then
                    '    Me._lbFacility.ClearSelection()
                    '    _lbFacility.Items.FindByValue(PlantCode).Selected = True
                    '    Me._btnFacilityList.Text = _lbFacility.SelectedItem.Text
                    'Else
                    '    If _lbFacility.Items.Count > 0 Then
                    '        Me._lbFacility.ClearSelection()
                    '        Me._lbFacility.SelectedIndex = 1
                    '        PlantCode = _lbFacility.SelectedItem.Value
                    '    End If
                    'End If
                Else
                    If Request(_txtResponsibleUserValue.UniqueID) IsNot Nothing Then
                        _txtResponsibleUserValue.Text = Request(_txtResponsibleUserValue.UniqueID)
                    End If

                    If Request(_lblResponsibleUser.UniqueID) IsNot Nothing Then
                        _lblResponsibleUser.Text = Request(_lblResponsibleUser.UniqueID)
                    End If
                    'reselect facility
                    'If Request(Me._lbFacility.UniqueID) IsNot Nothing Then
                    '    If _lbFacility.Items.FindByValue(Request(Me._lbFacility.UniqueID)) IsNot Nothing Then
                    '        _lbFacility.ClearSelection()
                    '        _lbFacility.Items.FindByValue(Request(Me._lbFacility.UniqueID)).Selected = True
                    '    End If
                    'End If
                    'If _lbFacility.SelectedIndex < 0 Then
                    '    Me._lbFacility.SelectedIndex = 0
                    'End If
                    'Me._btnFacilityList.Text = _lbFacility.SelectedItem.Text
                    'PlantCode = _lbFacility.SelectedItem.Value
                    'PopulateEmployeeList()
                End If
                'If Request.Form(Me._ddlResponsibleUser.UniqueID) IsNot Nothing Then
                '    If Me._ddlResponsibleUser.Items.FindByValue(Request.Form(Me._ddlResponsibleUser.UniqueID)) IsNot Nothing Then
                '        Me._ddlResponsibleUser.ClearSelection()
                '        Me._ddlResponsibleUser.Items.FindByValue(Request.Form(Me._ddlResponsibleUser.UniqueID)).Selected = True
                '    End If
                'End If
                _pceUsers.BehaviorID = Me.ID & "_bhUsers"
                '_lbFacility.Attributes.Add("onChange", "this.blur;ResponsibleUser.populateResponsibleUsers(this,'" & Me._ddlResponsibleUser.ClientID & "','" & Me._btnFacilityList.ClientID & "','" & Me.UserMode & "');AjaxControlToolkit.PopupControlBehavior.__VisiblePopup.hidePopup();")
                _btnClearSelection.OnClientClick = "return false;" '"ResponsibleUser.ClearResponsibleUser('" & Me._ddlResponsibleUser.ClientID & "');return false;"
                '_aceLookup.OnClientItemSelected = "ResponsibleUser.OnUserSelected"
                '_pceUsers.CommitScript = SetFacilityJS()
                _txtSearchBox.Attributes.Add("onkeyup", String.Format("ResponsibleUser.PerformNameSearch(this.value,'{0}');", _lbSearchResults.ClientID))
                _btnCancel.OnClientClick = String.Format("ResponsibleUser.HidePopup('{0}');return false;", _pceUsers.BehaviorID) ' _pceUsers.BehaviorID)
                '_btnSelect.OnClientClick = String.Format("ResponsibleUser.HidePopup('{0}');ResponsibleUser.SetUserAndFacility('{1}','{2}','{3}','{4}');return false;", _pceUsers.BehaviorID, _btnFacilityList.ClientID, _lbFacility.ClientID, _ddlResponsibleUser.ClientID, _lbSearchResults.ClientID)
                _lbSearchResults.Attributes.Add("onClick", String.Format("ResponsibleUser.HidePopup('{0}');ResponsibleUser.SetUser('{1}','{2}','{3}','{4}');", _pceUsers.BehaviorID, _txtResponsibleUserValue.ClientID, _lbSearchResults.ClientID, _lblResponsibleUser.ClientID, TargetControlId))
                ' SetUserAndFacility:function(facilityLabel, facilityListBox, userList, selectedList)
                'Me._btnClearUser.OnClientClick = "ResponsibleUser.ClearResponsibleUser();return false;"
                '_pceUsers.CommitScript = _lbFacility.ClientID

                'If Me.EnableValidation = True Then
                '    _lblRequiredField.Visible = True
                'Else
                '    _lblRequiredField.Visible = False
                'End If
            Catch ex As Exception

            End Try
        End Sub

        '        SELECT USERNAME, 
        '(SELECT R.SITENAME FROM REFSITE R WHERE R.SITEID = E.SITEID) AS SITENAME
        ', DOMAIN, FIRSTNAME, LASTNAME,PLANTCODE
        '      FROM         REFEMPLOYEE e
        '       WHERE     UPPER(E.FIRSTNAME) || ' ' || UPPER(E.LASTNAME) LIKE UPPER('joe%') AND ROWNUM <= 200
        '       ORDER BY E.FIRSTNAME,E.LASTNAME
#End Region

        'Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '    Me.ControlToValidate = Me._ddlResponsibleUser.ID
        '    Me.ClientValidationFunction = "ResponsibleUser.ResponsibleUserIsSelected"
        'End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            HandlePageLoad()
            'If Me.EnableViewState = False Or Me._ddlResponsibleUser.Items.Count = 0 And ShowDropdown = True Then
            '    'If Not Page.IsPostBack OrElse Me.EnableViewState = False Then
            '    Me.PopulateEmployeeList()
            'End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Me._lblEmployeeListCaption.Text = Me.Employeelabel
            'Me._btnFacilityList.Visible = _visible
            'Me._ddlResponsibleUser.Visible = _visible
            Me._lblEmployeeListCaption.Visible = _visible
            Me._btnSearch.Visible = _visible
            Me._btnClearSelection.Visible = _visible
            'If UserMode = UserModes.RolesOnly Then
            '    Me._btnFacilityList.Visible = False
            '    Me._btnSearch.Visible = False
            '    _pceUsers.Enabled = False
            '    _pceFacility.Enabled = False
            'End If
        End Sub


        Protected Sub _txtResponsibleUserValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _txtResponsibleUserValue.TextChanged
            RaiseEvent UserChanged(sender, e)
        End Sub
    End Class

End Namespace
