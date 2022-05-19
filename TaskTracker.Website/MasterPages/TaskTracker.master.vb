Imports System.Globalization
Partial Class TaskTrackerMaster
    Inherits BaseMasterPage

#Region "Public Methods"
    ''' <summary>
    ''' Used to display a message to the user in the form of a message box
    ''' </summary>   
    Public Sub DisplayMessageToUser(ByVal message As String, ByVal title As String, Optional ByVal okScript As String = "", Optional ByVal cancelScript As String = "", Optional ByVal buttontype As IP.Bids.UserControl.MessageBox.ButtonType1 = IP.Bids.UserControl.MessageBox.ButtonType1.OK)
        With _msgToUser
            .Title = title
            .Message = message
            If okScript.Length > 0 Then
                .OKScript = okScript
            End If
            If cancelScript.Length > 0 Then
                .CancelScript = cancelScript
            End If
            .ButtonType = buttontype
            .ShowMessage()
        End With
    End Sub

    ''' <summary> 
    ''' Sets the text for the page banner.
    ''' </summary>
    ''' <param name="bannerText">The banner text.</param>
    Public Sub SetBanner(ByVal bannerText As String)
        Me._imgBanner.BannerText = bannerText
        Me._imgBanner.SetBanner()
        Me.Head1.Title = "Task Tracker| " & bannerText
        _lblWelcome.Text = bannerText
    End Sub

    ''' <summary>
    ''' Populates the menu.
    ''' </summary>
    Public Sub PopulateMenu()
        Dim mnuDefaultMenu As New MenuItemCollection
        Dim mnuItem As New MenuItem
        Dim mnuChildItem As New MenuItem
        Dim confirmUrl As String = "{0}"
        '        Dim confirmPageChange As Boolean = False ' COMMENTED BY CODEIT.RIGHT

        'Dim mnuDataMaintenanceSubItem As New MenuItem

        Dim currentPage As String = Request.Url.PathAndQuery
        If currentPage.Contains("TaskHeader.aspx") Then 'Or currentPage.Contains("TaskDetails.aspx") Then
            confirmUrl = "javascript:JQConfirmRedirect('{0}',false);"
        End If



        With mnuDefaultMenu
            mnuItem = New MenuItem(GetLocalizedValue("Enter Task Header", True), "TaskHeader", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/TaskHeader.aspx")))

            .Add(mnuItem)
            mnuItem = New MenuItem(GetLocalizedValue("Tasks", True), "ViewTasks", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/ViewTasks.aspx")))
            .Add(mnuItem)
            mnuItem = New MenuItem(GetLocalizedValue("Reports", True), "Home", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/ReportSelection.aspx")))
            .Add(mnuItem)

            mnuItem = New MenuItem(GetLocalizedValue("Notification Profile", True), "NotificationProfile", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/DataMaintenance/Notifications.aspx")))
            .Add(mnuItem)

            'Data Maintenance
            mnuItem = New MenuItem(GetLocalizedValue("Data Maintenance", True), "Home", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/DataMaintenance.aspx")))
            mnuItem.Enabled = True




            Dim hideMaintenanceMenuItems As Boolean = True
            If hideMaintenanceMenuItems = False Then
                mnuChildItem = New MenuItem(GetLocalizedValue("Notification Profile", True), "NotificationProfile", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/DataMaintenance/Notifications.aspx")))
                mnuItem.ChildItems.Add(mnuChildItem)
                mnuChildItem = New MenuItem(GetLocalizedValue("Application Defaults", False), "ApplicationDefaults", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/DataMaintenance/UserDefaults.aspx")))
                mnuItem.ChildItems.Add(mnuChildItem)
                '.Add(mnuItem)
                'Added new Datamaintenace functions to dropdown menu
                Dim myRoles As System.Collections.Generic.List(Of UserRoles) = GeneralTaskTrackerBll.GetUserRoles(IP.Bids.SharedFunctions.GetCurrentUser.Username)
                Dim txtAdminFlag As String = "False"
                If myRoles IsNot Nothing AndAlso myRoles.Count > 0 Then
                    For Each role As UserRoles In myRoles
                        If role.RoleName.ToUpper = "SUPPORT" Or role.RoleName.ToUpper = "FACILITYADMIN" Then
                            txtAdminFlag = "True"
                        End If
                    Next
                End If
                If txtAdminFlag = "True" Then
                    mnuChildItem = New MenuItem(GetLocalizedValue("Employee Maintenance", True), "EmployeeMaintenance", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/DataMaintenance/MaintenanceEmployee.aspx?MaintFlag=MAINT")))
                    mnuItem.ChildItems.Add(mnuChildItem)
                    mnuChildItem = New MenuItem(GetLocalizedValue("Role Maintenance", True), "RoleMaintenance", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/DataMaintenance/MaintenanceRoles.aspx")))
                    mnuItem.ChildItems.Add(mnuChildItem)
                End If
            End If
            .Add(mnuItem)

            'Help
            mnuItem = New MenuItem(GetLocalizedValue("Help", True), "Help", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/Help/Default.aspx")))
            mnuItem.Enabled = True
            mnuChildItem = New MenuItem(GetLocalizedValue("Using MyHelp", True), "UsingMyHelp", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/Help/UsingMyHelp.aspx")))
            mnuItem.ChildItems.Add(mnuChildItem)

            mnuChildItem = New MenuItem(GetLocalizedValue("Online Training", True), "OnlineTraining", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/Help/OnlineTraining.aspx")))
            mnuItem.ChildItems.Add(mnuChildItem)

            If Me.HasSupportRole Then
                mnuChildItem = New MenuItem(GetLocalizedValue("Cache Viewer", True), "CacheViewer", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/Admin/CacheViewer.aspx")))
                mnuItem.ChildItems.Add(mnuChildItem)
            End If
            .Add(mnuItem)
        End With
        PopulateMenu(mnuDefaultMenu)
    End Sub

    Public Sub PopulateHelpMenu()
        Dim mnuDefaultMenu As New MenuItemCollection
        Dim mnuItem As New MenuItem
        Dim mnuChildItem As New MenuItem
        Dim confirmUrl As String = "{0}"

        With mnuDefaultMenu
            'Help
            mnuItem = New MenuItem(GetLocalizedValue("Help", True), "Help", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/Help/Default.aspx")))
            mnuItem.Enabled = True
            mnuChildItem = New MenuItem(GetLocalizedValue("Using MyHelp", True), "UsingMyHelp", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/Help/UsingMyHelp.aspx")))
            mnuItem.ChildItems.Add(mnuChildItem)
            mnuChildItem = New MenuItem(GetLocalizedValue("Online Training", True), "OnlineTraining", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/Help/OnlineTraining.aspx")))
            mnuItem.ChildItems.Add(mnuChildItem)
            If Me.HasSupportRole Then
                mnuChildItem = New MenuItem(GetLocalizedValue("Cache Viewer", True), "CacheViewer", Nothing, String.Format(CultureInfo.CurrentCulture, confirmUrl, Page.ResolveUrl("~/Admin/CacheViewer.aspx")))
                mnuItem.ChildItems.Add(mnuChildItem)
            End If
            .Add(mnuItem)
        End With
        PopulateMenu(mnuDefaultMenu)
    End Sub

    ''' <summary>
    ''' Populates the menu with the specified menu items.
    ''' </summary>
    ''' <param name="menu">The menu.</param>
    Public Sub PopulateMenu(ByVal menu As MenuItemCollection)
        Try
            If menu IsNot Nothing AndAlso menu.Count > 0 Then
                With Me._mnuLeftNav
                    .DataSourceID = ""
                    .Items.Clear()

                    For i As Integer = 0 To menu.Count - 1
                        .Items.Add(menu.Item(i))
                    Next
                End With
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateMenu", "Error Populating the Menu", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Selects the menu.
    ''' </summary>
    Public Sub SelectMenu()
        Dim thisPage As String = Page.AppRelativeVirtualPath
        Dim slashPos As Integer = InStrRev(thisPage, "/")
        Dim pageName As String = Right(thisPage, Len(thisPage) - slashPos)
        Dim clientQry As String = Page.ClientQueryString

        'Deselect All menu items
        For i As Integer = 0 To _mnuLeftNav.Items.Count - 1
            _mnuLeftNav.Items(i).Selected = False
            If _mnuLeftNav.Items(i).NavigateUrl.Contains(pageName) Then
                If _mnuLeftNav.Items(i).Enabled = True Then
                    _mnuLeftNav.Items(i).Selected = True
                End If

            End If
        Next


        If clientQry.Contains("RINumber=") Then 'Deselect Current page from menu
            _mnuLeftNav.StaticSelectedStyle.CssClass = "normallink"
        Else
            _mnuLeftNav.StaticSelectedStyle.CssClass = "selectedlink"
        End If


    End Sub
    Public Overrides Sub HideBannerFooter()
        _dvHeader.Style.Item("Display") = "none"
        _dvMenu.Style.Item("Display") = "none"
        _dvWelcome.Style.Item("Display") = "none"
        _dvFooter.Style.Item("Display") = "none"
    End Sub

    Public Overrides Sub HideHeaderAndMenu()
        _dvMenu.Style.Item("Display") = "none"
    End Sub
    Public Overrides Sub HideFooter()
        _dvFooter.Style.Item("Display") = "none"
    End Sub

#End Region

#Region "Private Methods"

    Private Sub HandlePageLoad()
        Try
            If Page.AppRelativeVirtualPath.Contains("MissingAccountInfo.aspx") Or Page.AppRelativeVirtualPath.Contains("InactiveUser.aspx") Then
                Exit Sub
            End If
            Dim myRoles As System.Collections.Generic.List(Of UserRoles) = GeneralTaskTrackerBll.GetUserRoles(IP.Bids.SharedFunctions.GetCurrentUser.Username)
            If myRoles IsNot Nothing AndAlso myRoles.Count > 0 Then
                For Each role As UserRoles In myRoles '            if myroles.Item(1).RoleName  
                    If role.RoleName.ToUpper = "SUPPORT" Then
                        Me.HasSupportRole = True
                    End If
                Next
            End If
            myRoles = Nothing

            'Handle Tasks from external sites           
            If Request.QueryString("RefSite") IsNot Nothing Then
                RefSite = Request.QueryString("RefSite")
                If RefSite.Length = 0 Then
                    If Session.Item("RefSite") IsNot Nothing Then
                        If CStr(Session.Item("RefSite")).Length > 0 Then
                            RefSite = CStr(Session.Item("RefSite"))
                        End If
                    End If
                End If
                'ElseIf Session.Item("RefSite") IsNot Nothing Then
                'refSite = CStr(Session.Item("RefSite"))
            ElseIf Session.Item("RefSite") IsNot Nothing Then
                RefSite = CStr(Session.Item("RefSite"))
            End If
            'If Request.QueryString("RefSite") IsNot Nothing Then
            'RefSite = Request.QueryString("RefSite").ToLower
            If RefSite.ToLower = "moc" Or RefSite.ToLower = "ri" Or RefSite.ToLower = "outage" Or RefSite.ToLower = "iris" Then
                HideFooter()
                HideHeaderAndMenu()

            End If
            If IP.Bids.SharedFunctions.GetPageError().Length > 0 Then
                With _AlertMessage
                    .Message = IP.Bids.SharedFunctions.GetPageError()
                    .ShowMessage()
                    IP.Bids.SharedFunctions.ClearPageError()
                End With
            End If
        Catch
            Throw
            HasSupportRole = False
        End Try
        PopulateMenu()
        SelectMenu()
    End Sub

#End Region

#Region "Events"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        SetCurrentUser(Me._login.CurrentUserInfo)
        If CurrentUser Is Nothing Then Throw New ApplicationException("Missing Current User Information")
        Session.Item("CurrentUserInfo") = CurrentUser
        Context.Request.Browser.Adapters.Clear()
        Request.Browser.Adapters.Clear()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandlePageLoad()
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        SetBanner(Me.PageName)
    End Sub
#End Region
End Class

