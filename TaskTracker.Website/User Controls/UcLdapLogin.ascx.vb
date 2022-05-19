'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-09-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports System.Globalization
Imports IP.Bids


Namespace IP.Bids.UserControl
    Partial Class LdapLogin
        Inherits System.Web.UI.UserControl
#Region "Fields"
        Private mCurrentUserInfo As UserInfo = Nothing
        Private mHorizontalAlign As UI.WebControls.HorizontalAlign = WebControls.HorizontalAlign.Left
        Private mAuthenticatedUrl As String = String.Empty
        Private mNonAuthenticatedUrl As String = "~/default.aspx"
        Private _ipResources As IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization ' ENCAPSULATE FIELD BY CODEIT.RIGHT
        Private mIsAuthenticated As Boolean

        Public Property IsAuthenticated() As Boolean
            Get
                Return mIsAuthenticated
            End Get
            Set(ByVal value As Boolean)
                mIsAuthenticated = value
            End Set
        End Property
        Public Property IPResources() As IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization
            Get
                Return _ipResources
            End Get
            Set(ByVal Value As IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization)
                _ipResources = Value
            End Set
        End Property

#End Region

#Region "Properties"
        ''' <summary>
        ''' Gets or sets the User Profile info
        ''' </summary>        
        Public Property CurrentUserInfo() As UserInfo
            Get
                Return mCurrentUserInfo
            End Get
            Private Set(ByVal value As UserInfo)
                mCurrentUserInfo = value
            End Set
        End Property

        Public Property NonAuthenticatedUrl() As String
            Get
                Return mNonAuthenticatedUrl
            End Get
            Set(ByVal value As String)
                mNonAuthenticatedUrl = value
            End Set
        End Property

        Public Property AuthenticatedUrl() As String
            Get
                Return mAuthenticatedUrl
            End Get
            Set(ByVal value As String)
                If Page.ResolveUrl(value).Length > 0 Then
                    mAuthenticatedUrl = value
                Else
                    mAuthenticatedUrl = ""
                End If
            End Set
        End Property
        ''' <summary>
        ''' Gets or sets the horizontal alignment of the contents within the panel.
        ''' </summary>
        Public Property HorizontalAlign() As UI.WebControls.HorizontalAlign
            Get
                Return mHorizontalAlign
            End Get
            Set(ByVal value As UI.WebControls.HorizontalAlign)
                mHorizontalAlign = value
            End Set
        End Property
#End Region

#Region "Public Methods"
        ''' <summary>
        ''' Sets the welcome text that will be displayed after the user has logged in
        ''' </summary>
        ''' <param name="userName">String - current username i.e JADoe</param>
        ''' <param name="fullName">String - User fullname from LDAP</param>
        ''' <param name="welcomeText">Optional String - The welcome text that will be displayed to the user</param>
        ''' <remarks></remarks>
        Public Sub SetWelcomeText(ByVal userName As String, ByVal fullName As String, Optional ByVal welcomeText As String = "")
            Dim ServerName = System.Environment.MachineName
            If userName.Length = 0 Then
                Me._lblWelcome.Visible = False
                Me._loginStatus.Visible = False

            Else
                If welcomeText.Length = 0 Then
                    'welcomeText = IPResources.GetResourceValue("WelcomeText")
                    welcomeText = IP.Bids.SharedFunctions.LocalizeValue("Database", True) & ": {2}, Web Server: {3}, Version: {4} " & IP.Bids.SharedFunctions.LocalizeValue("Current User", True) & ": {0} ({1})"
                End If
                _lblWelcome.Text = String.Format(CultureInfo.CurrentCulture, welcomeText, fullName, userName.ToUpper, GeneralTaskTrackerBll.GetServiceName, ServerName, GetApplicationVersion)
                Me._lblWelcome.Visible = True
                Me._loginStatus.Visible = True
            End If
        End Sub

        Private Function GetApplicationVersion() As String
            Dim buildDate = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)

            Return String.Format("{0}.{1}.{2}.1", buildDate.Year, buildDate.Month, buildDate.Day)
        End Function

        ''' <summary>
        ''' Attempts to authenticate the user using credentials of the user that is logged onto the machine
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AuthenticateUser()
'            Dim userData As String = String.Empty ' COMMENTED BY CODEIT.RIGHT
            Dim SuccessfulLogin As Boolean
            Try

                'if mCurrentUserInfo is nothing then mCurrentUserInfo = 
                _lblMessageToUser.Text = String.Empty
                'Session.Remove("RedirectURL")
                If System.Web.HttpContext.Current.User.Identity.IsAuthenticated Then
                    SuccessfulLogin = True
                    Dim userName As String() = {IP.Bids.SharedFunctions.GetCurrentUser.Domain, IP.Bids.SharedFunctions.GetCurrentUser.Username} '  System.Web.HttpContext.Current.User.Identity.Name.Split("\")
                    mCurrentUserInfo = CType(Session.Item("CurrentUserInfo"), UserInfo)
                    If mCurrentUserInfo Is Nothing Then
                        mCurrentUserInfo = New UserInfo(userName(0), userName(1))
                    End If
                    If mCurrentUserInfo IsNot Nothing Then
                        Session.Add("CurrentUserInfo", mCurrentUserInfo)
                    End If

                    If mCurrentUserInfo.IsAuthenticated = False Then
                        SuccessfulLogin = False
                        Session.Add("RedirectURL", Page.AppRelativeVirtualPath)
                        'Me._mpeLogin.Show()
                        If Not Page.AppRelativeVirtualPath.Contains("MissingAccountInfo.aspx") Then
                            'Server.Transfer("~/MissingAccountInfo.aspx", False)
                            IP.Bids.SharedFunctions.ResponseRedirect("~/MissingAccountInfo.aspx")
                        End If
                        Session.Remove("CurrentUserInfo")
                        Exit Sub
                    End If

                    Dim listOfDomains = {"ICC_GO", "IPCFPULP"}
                    If listOfDomains.Contains(mCurrentUserInfo.Domain.ToUpper.ToUpper) Then
                        SuccessfulLogin = False
                        Session.Add("RedirectURL", Page.AppRelativeVirtualPath)
                        'Temple Inland users
                        _lblMessageToUser.Text = "Warning! Your account [" & mCurrentUserInfo.Domain & "\" & mCurrentUserInfo.Username & "] is not authorized to use this website.  Please login with your credentials."
                        'Me._mpeLogin.Show()
                        If Not Page.AppRelativeVirtualPath.Contains("MissingAccountInfo.aspx") Then
                                'Server.Transfer("~/MissingAccountInfo.aspx", False)
                                IP.Bids.SharedFunctions.ResponseRedirect("~/MissingAccountInfo.aspx")
                            End If
                            Exit Sub
                        End If
                        If mCurrentUserInfo.IsUserInEmployeeTable = False Then
                            SuccessfulLogin = False
                            If Not Page.AppRelativeVirtualPath.Contains("MaintenanceEmployee.aspx") Then
                                Session.Add("RedirectURL", Page.AppRelativeVirtualPath)
                                'Server.Transfer("~/DataMaintenance/MaintenanceEmployee.aspx", False)
                                IP.Bids.SharedFunctions.ResponseRedirect("~/DataMaintenance/MaintenanceEmployee.aspx")
                                Exit Sub
                            End If
                        ElseIf mCurrentUserInfo.IsActive = False Then
                            SuccessfulLogin = False
                            If Not Page.AppRelativeVirtualPath.Contains("InactiveUser.aspx") Then
                                Session.Add("RedirectURL", Page.AppRelativeVirtualPath)
                                'Server.Transfer("~/InactiveUser.aspx", False)
                                IP.Bids.SharedFunctions.ResponseRedirect("~/InactiveUser.aspx")
                                Exit Sub
                            End If
                        End If

                        'Dim userTA As New DataMaintenanceDALTableAdapters.EmployeeTableAdapter  ' UserDataTableAdapters.UserInfoTableAdapter
                        'Dim userDT As Data.DataTable = Nothing

                        'Dim fullName As String = String.Empty


                        'Session.Clear()
                        'If mCurrentUserInfo Is Nothing OrElse mCurrentUserInfo.IsUserInEmployeeTable = False OrElse mCurrentUserInfo.IsActive = False Then
                        '    userDT = (userTA.GetUser(UserName(1), Nothing)) '= userTA.GetUser(userName)
                        '    If userDT IsNot Nothing AndAlso userDT.Rows.Count = 0 Then
                        '        userDT = Nothing
                        '    End If


                        '    If userDT IsNot Nothing AndAlso userDT.Rows.Count > 0 Then

                        '        Dim userRow As DataMaintenanceDAL.EmployeeRow = userDT.Rows(0)
                        '        Dim Inactive_Flag As Boolean
                        '        fullName = IP.Bids.SharedFunctions.DataClean(userRow.FIRSTNAME & " " & userRow.LASTNAME)
                        '        If userRow.IsINACTIVE_FLAGNull Then
                        '            userRow.INACTIVE_FLAG = "N"
                        '            Inactive_Flag = True
                        '        ElseIf userRow.INACTIVE_FLAG.ToUpper = "Y" Then
                        '            Inactive_Flag = False
                        '        End If
                        '        mCurrentUserInfo = New UserInfo(userRow.DOMAIN & "\" & userRow.USERNAME, fullName, Inactive_Flag) 'userRow.UserID, userRow.UserName, IP.Bids.SharedFunctions.DataClean(userRow.Name), userRow.IsActive)
                        '        If mCurrentUserInfo IsNot Nothing Then
                        '            Session.Add("CurrentUserInfo", mCurrentUserInfo)
                        '        End If
                        '        userRow = Nothing

                        '    ElseIf userDT IsNot Nothing Then
                        '        'User is Not Active
                        '        mCurrentUserInfo = New UserInfo(UserName(0), UserName(1)) 'userRow.UserID, userRow.UserName, IP.Bids.SharedFunctions.DataClean(userRow.Name), userRow.IsActive)
                        '        If mCurrentUserInfo IsNot Nothing Then
                        '            mCurrentUserInfo.IsActive = False
                        '            mCurrentUserInfo.IsUserInEmployeeTable = True
                        '            Session.Add("CurrentUserInfo", mCurrentUserInfo)
                        '        End If
                        '        If Not Page.AppRelativeVirtualPath.Contains("InactiveUser.aspx") Then
                        '            Server.Transfer("~/InactiveUser.aspx", False)
                        '        End If
                        '    Else
                        '        'User is not in RefEmployee Table
                        '        'fullName = IP.Bids.SharedFunctions.DataClean(userRow.FIRSTNAME & " " & userRow.LASTNAME)
                        '        mCurrentUserInfo = New UserInfo(UserName(0), UserName(1)) 'userRow.UserID, userRow.UserName, IP.Bids.SharedFunctions.DataClean(userRow.Name), userRow.IsActive)
                        '        If mCurrentUserInfo IsNot Nothing Then
                        '            mCurrentUserInfo.IsActive = False
                        '            mCurrentUserInfo.IsUserInEmployeeTable = False
                        '            Session.Add("CurrentUserInfo", mCurrentUserInfo)
                        '        End If

                        '        If Not Page.AppRelativeVirtualPath.Contains("MissingAccountInfo.aspx") Then
                        '            Server.Transfer("~/MissingAccountInfo.aspx", False)
                        '        End If
                        '    End If
                        '    userDT = Nothing
                        '    userTA = Nothing
                        'End If

                        If mCurrentUserInfo IsNot Nothing Then
                            SetWelcomeText(mCurrentUserInfo.Domain & "\" & mCurrentUserInfo.Username, mCurrentUserInfo.Name)
                        Else
                            SetWelcomeText(userName(1), "")
                        End If
                        Me._logOut.Visible = False 'true
                        Me._runAs.Visible = False
                        Me._loginStatus.Text = IPResources.GetResourceValue("SwitchUser")
                        If Session.Item("RedirectURL") IsNot Nothing And SuccessfulLogin = True Then
                            Dim redirectUrl As String = Session.Item("RedirectURL")
                            Session.Remove("RedirectURL")
                            IP.Bids.SharedFunctions.ResponseRedirect(redirectUrl)
                            'HttpContext.Current.ApplicationInstance.CompleteRequest()
                        End If
                    Else
                        Me._logOut.Visible = False
                    Me._runAs.Visible = False
                    'TODO: 'Handle Non Authenticated User
                    SetWelcomeText("Handle Non Authenticated User", "")
                    Me._loginStatus.Text = IPResources.GetResourceValue("Login")
                End If
                'End If
            Catch ex As Exception
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Attempts to authenticate the user using the specified credentials
        ''' </summary>
        ''' <param name="domain"></param>
        ''' <param name="username"></param>
        ''' <param name="password"></param>
        ''' <remarks></remarks>
        Public Sub AuthenticateUser(ByVal domain As String, ByVal username As String, ByVal password As String)
'            Dim userData As String = String.Empty ' COMMENTED BY CODEIT.RIGHT

            Try
                mCurrentUserInfo = New UserInfo(domain, username, password)
                If mCurrentUserInfo IsNot Nothing AndAlso mCurrentUserInfo.IsAuthenticated = True Then
                    Session.Add("CurrentUserInfo", mCurrentUserInfo)
                    AuthenticateUser()
                End If

            Catch
                Throw
            End Try             
        End Sub

        ''' <summary>
        ''' Attempts to authenticate the user using the specified credentials
        ''' </summary>
        ''' <param name="domain"></param>
        ''' <param name="username"></param>
        ''' <remarks></remarks>
        Public Sub AuthenticateUser(ByVal domain As String, ByVal username As String)
            '            Dim userData As String = String.Empty ' COMMENTED BY CODEIT.RIGHT

            Try
                mCurrentUserInfo = New UserInfo(domain, username)
                If mCurrentUserInfo IsNot Nothing AndAlso mCurrentUserInfo.IsAuthenticated = True Then
                    Session.Add("CurrentUserInfo", mCurrentUserInfo)
                    AuthenticateUser()
                End If

            Catch
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Attempts to authenticate the user using the specified credentials
        ''' </summary>
        ''' <param name="username"></param>
        ''' <remarks></remarks>
        Public Sub AuthenticateUser(ByVal username As String)
            '            Dim userData As String = String.Empty ' COMMENTED BY CODEIT.RIGHT
            Dim userTA As New DataMaintenanceDalTableAdapters.EmployeeTableAdapter ' UserDataTableAdapters.UserInfoTableAdapter
            Dim userDT As DataMaintenanceDAL.EmployeeDataTable
            Try
                userDT = userTA.GetUser(username, Nothing)
                If userDT IsNot Nothing AndAlso userDT.Rows.Count > 0 Then

                    Dim userRow As DataMaintenanceDAL.EmployeeRow = userDT.Rows(0)
                    If userRow IsNot Nothing Then
                        AuthenticateUser(userRow.DOMAIN, userRow.USERNAME)
                    End If
                End If              
            Catch
                Throw
            End Try
        End Sub

        Private Sub PopulateDomain(ByVal defaultdomain As String)
            Dim DomainList As System.Collections.Generic.List(Of Domain) = Domain.GetListOfDomains

            Try
                _ddlDomain.Items.Clear()
                For Each item As Domain In DomainList
                    With Me._ddlDomain
                        If item.DomainName.Length > 0 Then
                            .Items.Add(New ListItem(item.DomainName, item.DomainName.ToUpper))
                        End If
                    End With
                Next
                If _ddlDomain.Items.FindByValue(defaultdomain.ToUpper) IsNot Nothing Then
                    _ddlDomain.ClearSelection()
                    _ddlDomain.Items.FindByValue(defaultdomain.ToUpper).Selected = True
                End If
                Session.Add("DomainList", DomainList)
                'Me._ddldomain.Items.Insert(0, "")
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("PopulateDomain", , ex)
            End Try
        End Sub
#End Region

#Region "Private Events"
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            IPResources = New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization

            If Request.QueryString("TargetUrl") IsNot Nothing Then
                Me.AuthenticatedUrl = Request.QueryString("TargetUrl")
            End If
            'Response.Redirect("~/default.aspx")
            If SharedFunctions.CausedPostBack(Me._logOut.UniqueID) Then
                LogOut(Nothing, Nothing)
            End If
            'Exit Sub
            'PopulateDomain("NAIPAPER")
            PopulateDomain("NA")
            If Session("UserLoggedOut") Is Nothing Then
                Me.AuthenticateUser()
                
            Else
                SetWelcomeText(IPResources.GetResourceValue("PleaseLogin"), "")
                Me._logOut.Visible = False
                Me._loginStatus.Text = IPResources.GetResourceValue("Login")
                'Me._udpLogin.Update()
                ' Me._mpeLogin.Show()
                If Not mCurrentUserInfo Is Nothing Then
                    _runAs.Text = IPResources.GetResourceValue("PleaseLogin") & " (" & mCurrentUserInfo.Username & ")"
                ElseIf HttpContext.Current.User.Identity.IsAuthenticated Then
                    _runAs.Text = IPResources.GetResourceValue("RunAs") & " (" & HttpContext.Current.User.Identity.Name & ")"
                Else
                    _runAs.Visible = False
                End If
            End If
            'MsgBox(My.User.Name)
        End Sub

        ''' <summary>
        ''' Attempts to log the current user out of the website
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub LogOut(ByVal sender As Object, ByVal e As System.EventArgs)
            Session.Clear()
            Session.Add("UserLoggedOut", "true")
            mCurrentUserInfo = Nothing
            SetWelcomeText(IPResources.GetResourceValue("PleaseLogin"), "")
            'Me._mpeLogin.Show()
            Me._logOut.Visible = False
            Me._loginStatus.Text = IPResources.GetResourceValue("Login")
            'Me._udpLogin.Update()
            IP.Bids.SharedFunctions.ResponseRedirect("~/default.aspx?TargetURL=" & Page.AppRelativeVirtualPath)
            'HttpContext.Current.ApplicationInstance.CompleteRequest()
        End Sub

        'Protected Sub _login_Authenticate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles _login.Authenticate
        '    Dim domain As DropDownList = CType(Me._login.FindControl("_ddlDomain"), DropDownList)
        '    Dim userName As TextBox = CType(Me._login.FindControl("UserName"), TextBox)
        '    Dim password As TextBox = CType(Me._login.FindControl("Password"), TextBox)
        '    Session.Remove("CurrentUser")
        '    If domain IsNot Nothing And userName IsNot Nothing And password IsNot Nothing Then
        '        Session.Remove("CurrentUserInfo")
        '        AuthenticateUser(domain.SelectedValue, userName.Text, password.Text)
        '        If mCurrentUserInfo IsNot Nothing Then
        '            If mCurrentUserInfo.IsAuthenticated Then
        '                Session.Add("CurrentUser", mCurrentUserInfo.Username)
        '            End If
        '            e.Authenticated = mCurrentUserInfo.IsAuthenticated

        '        Else
        '            e.Authenticated = False
        '        End If

        '    End If
        '    _udpLogin.Update()
        '    If e.Authenticated = True Then
        '        Me._mpeLogin.Hide()
        '        Session.Remove("UserLoggedOut")
        '    Else
        '        Session.Remove("CurrentUserInfo")
        '        Me.AuthenticateUser()
        '        Me._mpeLogin.Show()
        '    End If
        'End Sub

        Public Sub LoginAuthenticate()           
            
        End Sub
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Dim btn As HtmlInputButton = CType(Me._login.FindControl("_btnCancel2"), HtmlInputButton)
            'If btn IsNot Nothing Then
            '_btnCancel2.Attributes.Add("onclick", "javascript:$get('" & Me._btnHideLogin.ClientID & "').click();return false;")
            'End If
            'Dim btn2 As HtmlInputButton = CType(Me._login.FindControl("_btnCancelImpersonate"), HtmlInputButton)
            'If btn2 IsNot Nothing Then
            '_btnCancelImpersonate.Attributes.Add("onclick", "javascript:$get('" & Me._btnHideLogin.ClientID & "').click();return false;")
            'End If

            Dim authenticatedUser As String() = My.User.Name.Split("\") '{IP.Bids.SharedFunctions.GetCurrentUser.Domain, IP.Bids.SharedFunctions.GetCurrentUser.Username} 'My.User.Name.Split("\")
            Dim myRoles As System.Collections.Generic.List(Of UserRoles) = GeneralTaskTrackerBll.GetUserRoles(authenticatedUser(1).ToUpper) 'IP.Bids.SharedFunctions.GetCurrentUser.Username.ToUpper)
            If myRoles IsNot Nothing AndAlso myRoles.Count > 0 Then
                Me._pnlImpersonate.Visible = False
                For Each role As UserRoles In myRoles '            if myroles.Item(1).RoleName  
                    If role.RoleName.ToUpper = "SUPPORT" Then
                        Me._pnlImpersonate.Visible = True
                        PasswordRequired.Enabled = False
                        Me.Password.BackColor = Drawing.Color.LightGray
                    End If
                Next
            Else
                Me._pnlImpersonate.Visible = False
            End If

            If Not Page.IsPostBack Then
                Me.UserName.Text = Me.CurrentUserInfo.Username
                If Me.mCurrentUserInfo IsNot Nothing AndAlso Me._pnlImpersonate.Visible = True Then
                    If Me.mCurrentUserInfo.UserDefaults IsNot Nothing AndAlso Me.mCurrentUserInfo.UserDefaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.PlantCode) Then '   Me.mCurrentUserInfo.UserDefaults.Item("Facility") IsNot Nothing Then
                        'Dim userList As User_Controls_EmployeeList2 = CType(Me._login.FindControl("_ImpersonateList"), User_Controls_EmployeeList2)
                        'If userList IsNot Nothing Then
                        If _pnlImpersonate.Visible = True Then
                            With _ImpersonateList
                                .PlantCode = Me.mCurrentUserInfo.UserDefaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
                                .DefaultUserName = Me.CurrentUserInfo.Username
                                .PopulateEmployeeList()
                            End With
                            _pnlImpersonate.EnableViewState = True
                        Else

                        End If
                    End If
                End If
            End If
        End Sub

        Protected Sub _runAs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _runAs.Click
            Session.Remove("UserLoggedOut")
            Me.AuthenticateUser()
            'Me._mpeLogin.Hide()
            Me._runAs.Visible = False
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If mCurrentUserInfo IsNot Nothing Then
                If mCurrentUserInfo.IsAuthenticated = False Then
                    If LCase(Request.Url.AbsolutePath) <> LCase(Page.ResolveUrl(NonAuthenticatedUrl)) Then
                        IP.Bids.SharedFunctions.ResponseRedirect(NonAuthenticatedUrl)
                        'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    End If
                Else
                    If Me.AuthenticatedUrl.Length > 0 Then
                        If LCase(Request.Url.AbsolutePath) <> LCase(Page.ResolveUrl(AuthenticatedUrl)) Then
                            IP.Bids.SharedFunctions.ResponseRedirect(AuthenticatedUrl)
                            'HttpContext.Current.ApplicationInstance.CompleteRequest()
                        End If
                    End If
                End If
            Else
                'The user is not logged in
                If LCase(Request.Url.AbsolutePath) <> LCase(Page.ResolveUrl(NonAuthenticatedUrl)) Then
                    IP.Bids.SharedFunctions.ResponseRedirect(NonAuthenticatedUrl)
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    'Server.Transfer("~/default.aspx", False)
                End If
            End If
        End Sub
#End Region        

        Protected Sub _btnImpersonate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnImpersonate.Click
            Session.Clear()
            Session.Remove("CurrentUser")
            Session.Remove("CurrentUserInfo")
            AuthenticateUser(Me._ImpersonateList.SelectedValue)
            If mCurrentUserInfo IsNot Nothing Then
                If mCurrentUserInfo.IsAuthenticated Then
                    Session.Add("CurrentUser", mCurrentUserInfo.Username)
                End If
                IsAuthenticated = mCurrentUserInfo.IsAuthenticated

            Else
                IsAuthenticated = False
            End If

            '_udpLogin.Update()
            If IsAuthenticated = True Then
                'Me._mpeLogin.Hide()
                Session.Remove("UserLoggedOut")
                IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath)
                'HttpContext.Current.ApplicationInstance.CompleteRequest()
            Else
                Session.Remove("CurrentUserInfo")
                Me.AuthenticateUser()
                'Me._mpeLogin.Show()
            End If
        End Sub

        Protected Sub LoginButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LoginButton.Click
            Try
                Session.Clear()
                Session.Remove("CurrentUser")
                Session.Remove("CurrentUserInfo")
                Try
                    Dim user() As String = UserName.Text.Split("\")
                    If user.Length <> 1 Then
                        'Invalid usersname was entered
                        UserName.Text = user(user.Length - 1)
                        FailureText.Text = IP.Bids.SharedFunctions.LocalizeValue("Please enter a valid username (i.e. jadoe)", True)
                        'Me._mpeLogin.Show()
                        Exit Sub
                    End If
                    AuthenticateUser(Me._ddlDomain.SelectedValue, UserName.Text, Password.Text)
                Catch ex As DirectoryServices.DirectoryServicesCOMException
                    Me.FailureText.Text = ex.Message
                    Session.Remove("CurrentUserInfo")
                    'Me.AuthenticateUser()
                    ' Me._mpeLogin.Show()
                    Exit Sub
                End Try
                If mCurrentUserInfo IsNot Nothing Then
                    IsAuthenticated = mCurrentUserInfo.IsAuthenticated
                    If mCurrentUserInfo.IsAuthenticated Then
                        Session.Add("CurrentUser", mCurrentUserInfo.Username)
                    Else
                        Me.FailureText.Text = UserName.Text & " - " & IP.Bids.SharedFunctions.LocalizeValue("Is not a valid username.", True)
                        Session.Remove("CurrentUserInfo")
                        'Me.AuthenticateUser()
                        'Me._mpeLogin.Show()
                    End If


                Else
                    IsAuthenticated = False
                End If

                '_udpLogin.Update()
                If IsAuthenticated = True Then
                    'Me._mpeLogin.Hide()
                    Session.Remove("UserLoggedOut")
                    IP.Bids.SharedFunctions.ResponseRedirect("~/ViewTasks.aspx")
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    'Response.Redirect(Page.AppRelativeVirtualPath, True)

                Else
                    Session.Remove("CurrentUserInfo")
                    Me.AuthenticateUser()
                    'Me._mpeLogin.Show()
                End If
            Catch
                Throw
                IsAuthenticated = False
                Session.Remove("CurrentUserInfo")
                Me.AuthenticateUser()
                ' Me._mpeLogin.Show()
            End Try
        End Sub

        
    End Class
End Namespace