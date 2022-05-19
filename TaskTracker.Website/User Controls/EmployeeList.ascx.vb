'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 11-19-2010
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class UserControlsEmployeeList
    Inherits IP.Bids.UserControl.UserControlValidation

#Region "Fields"
    Private _PlantCode As String = -1
    Private _DefaultUserName As String = String.Empty
    Private _DefaultUser As String = String.Empty
    Private _AllowUserRoles As Boolean = True
#End Region

#Region "Properties"

    ''' <summary>
    ''' Gets or sets a value indicating whether a postback to the server automatically occurs when the user changes the list selection.  
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AutoPostBack() As Boolean
        Get
            Return Me._ddlResponsibleUser.AutoPostBack
        End Get
        Set(ByVal value As Boolean)
            Me._ddlResponsibleUser.AutoPostBack = value
        End Set
    End Property


    Public Property AllowUserRoles() As Boolean
        Get
            Return _AllowUserRoles
        End Get
        Set(ByVal value As Boolean)
            _AllowUserRoles = value
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

    Public Property DefaultUserName() As String
        Get
            Return _DefaultUserName
        End Get
        Set(ByVal value As String)
            _DefaultUserName = value
        End Set
    End Property

    Public Property DefaultUser() As String
        Get
            Return _DefaultUser
        End Get
        Set(ByVal value As String)
            _DefaultUser = value
        End Set
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
        End Set
    End Property
#End Region

#Region "Methods"
    Private Sub HandlePageLoad()

        Try

            Dim siteItem As New TaskTrackerSiteBll
            Dim facilityList As System.Collections.Generic.List(Of Facility) = siteItem.GetFacility("", "")

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
            ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType, "EmployeeList", GetGlobalJS(), True)
            '***************************************************

            If Not Page.IsPostBack Then
                For Each item As Facility In facilityList
                    With Me._ddlResponsibleFacility
                        .Items.Add(New ListItem(item.SiteName, item.PlantCode))
                    End With
                Next
                Me._ddlResponsibleFacility.Items.Insert(0, "")
                If _ddlResponsibleFacility.Items.FindByValue(PlantCode) IsNot Nothing Then
                    _ddlResponsibleFacility.Items.FindByValue(PlantCode).Selected = True
                    PopulateEmployeeList()

                Else
                    Me._ddlResponsibleFacility.SelectedIndex = 0
                End If
            Else
                Me._ddlResponsibleFacility.SelectedIndex = 0
            End If


            _ddlResponsibleFacility.Attributes.Add("onChange", "this.blur;ResponsibleUser.populateResponsibleUsers(this,'" & Me._ddlResponsibleUser.ClientID & "');")
            Me._btnClearUser.OnClientClick = "ResponsibleUser.ClearResponsibleUser();return false;"
        Catch ex As Exception

        End Try
    End Sub

    Public Sub PopulateEmployeeList()
        Dim taskItem As New TaskTrackerListsBll
        Dim plantCode As String = Me._ddlResponsibleFacility.SelectedValue
        If plantCode.Length = 0 Then
            plantCode = Me.PlantCode
        End If
        Dim userList As System.Collections.Generic.List(Of ResponsibleUsers) = TaskTrackerListsBll.GetResponsibleUsers(plantCode)
        Dim roleDescription As String = String.Empty
        Dim userIsSelected As Boolean
        _ddlResponsibleUser.Items.Clear()
        roleDescription = String.Empty

        For Each item As ResponsibleUsers In userList
            Dim spaceChar As String = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
            If Me.AllowUserRoles = False Then
                If item.RoleSeqID.Length = 0 Then
                    Continue For
                End If
            End If
            With _ddlResponsibleUser

                If item.RoleDescription <> roleDescription Then 'New Group
                    Dim roleItem As New ListItem
                    roleDescription = item.RoleDescription
                    roleItem.Text = item.RoleDescription.ToUpper
                    roleItem.Value = item.RoleSeqID

                    If .Items.Count > 0 Then
                        Dim blankItem As New ListItem
                        With blankItem
                            .Attributes.Add("disabled", "true")
                            .Text = ""
                            .Value = -1
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
                Dim useritem As New ListItem
                With useritem

                    .Text = Server.HtmlDecode(spaceChar & IP.Bids.SharedFunctions.ToTitleCase(item.FullName.ToLower))
                    .Value = item.UserName
                    If item.UserName = Me.DefaultUserName Then
                        If Not userIsSelected Then
                            .Selected = True
                            DefaultUser = IP.Bids.SharedFunctions.ToTitleCase(item.FullName.ToLower)
                            userIsSelected = True
                        End If
                    End If
                End With
                .Items.Add(useritem)
            End With
        Next
    End Sub

    Private Function GetGlobalJS() As String
        Dim sb As New StringBuilder
        With sb
            .Append("var _ddlResponsibleFacility = '" & Me._ddlResponsibleFacility.ClientID & "';")
            .Append("var _ddlResponsibleUser = '" & Me._ddlResponsibleUser.ClientID & "';")
            .Append("var _txtTargetID = '" & Me._txtTargetID.ClientID & "';")
            .Append("var _txtResponsibleUserName = '" & Me._txtResponsibleUserName.ClientID & "';")
        End With
        Return sb.ToString
    End Function

    'Private Function EmployeeListExists() As Boolean
    '    Return True
    'End Function
#End Region

#Region "Events"
    Private RenderMe As Boolean

    Protected Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.ControlToValidate = Me._ddlResponsibleUser.ID
        Me.ClientValidationFunction = ""
    End Sub
    'Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    '    RenderMe = Not EmployeeListExists()
    '    Page.Items.Item(GetType(User_Controls_EmployeeList)) = Me
    'End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandlePageLoad()
    End Sub
#End Region

End Class
