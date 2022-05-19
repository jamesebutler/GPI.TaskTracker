Imports System.Data
Imports Devart.Data.Oracle
Imports IP.Bids

Partial Class ucCultureSelection
    Inherits System.Web.UI.UserControl
    Private mRILocalization As New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization
    Private mLocaleLanguages As New Hashtable
    'Private userprofile As IP.Bids.CurrentUserProfile = Nothing
    Public Property LocaleLanguages() As Hashtable
        Get
            mLocaleLanguages = mRILocalization.ApplicationLocaleList
            Return mLocaleLanguages
        End Get
        Set(ByVal value As Hashtable)
            mLocaleLanguages = value
        End Set
    End Property

    Public Function RemoveQueryStringValue(ByVal key As String) As String
        Dim nameValues = HttpUtility.ParseQueryString(HttpContext.Current.Request.QueryString.ToString())
        If nameValues.Item(key) IsNot Nothing Then
            nameValues.Remove(key)
        End If
        Dim url = HttpContext.Current.Request.Url.AbsolutePath
        Dim updatedQueryString = "?" + nameValues.ToString()
        Return url + updatedQueryString
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim currentUser As IP.Bids.UserInfo
        Dim languages As New Hashtable
        Dim defaultLanguage As String = "EN-US" 'currentUser.DefaultLanguage
        Dim currentLanguage As String = System.Threading.Thread.CurrentThread.CurrentCulture.Name

        Try
            
            currentUser = IP.Bids.SharedFunctions.GetCurrentUser
            If currentUser IsNot Nothing AndAlso currentUser.UserDefaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Language) Then '   currentUser.UserDefaults.Item("language") IsNot Nothing Then
                currentLanguage = currentUser.UserDefaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Language)
            End If
            If Request.QueryString("lang") IsNot Nothing Then
                currentLanguage = Request.QueryString("lang")
                SaveDefaultLanguage(currentUser.Username, currentLanguage)
                IP.Bids.SharedFunctions.ClearCurrentUserSession()
                'IP.Bids.SharedFunctions.DeleteEntireCache()
                IP.Bids.SharedFunctions.InitCulture(IP.Bids.SharedFunctions.DataClean(Me._rblLanguages.SelectedValue, "EN-US"))
                ' IP.Bids.SharedFunctions.DisablePageCache(Page.Response)
                IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveUrl(RemoveQueryStringValue("lang")))
            End If
            If LocaleLanguages.Count = 0 Then
                languages.Add(defaultLanguage, defaultLanguage)
            Else
                languages = LocaleLanguages
            End If

            If Not Page.IsPostBack Then
                Me._rblLanguages.Items.Clear()
                For Each lng As DictionaryEntry In languages
                    If lng.Value.ToString.ToUpper <> "UNKNOWN" Then
                        Me._rblLanguages.Items.Add(New ListItem(lng.Value.ToString, lng.Key))
                    End If
                Next
                If _rblLanguages.Items.FindByValue(currentLanguage) IsNot Nothing Then
                    _rblLanguages.ClearSelection()
                    _rblLanguages.Items.FindByValue(currentLanguage).Selected = True
                Else
                    For Each item As ListItem In _rblLanguages.Items
                        If item.Value.ToUpper = currentLanguage.ToUpper Then
                            _rblLanguages.ClearSelection()
                            item.Selected = True
                        End If
                    Next
                End If
            End If

            If System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToUpper <> defaultLanguage Then
                Me._lblCurrentLanguage.Text = System.Threading.Thread.CurrentThread.CurrentCulture.EnglishName & ": " & System.Threading.Thread.CurrentThread.CurrentCulture.NativeName
            Else
                Me._lblCurrentLanguage.Text = System.Threading.Thread.CurrentThread.CurrentCulture.EnglishName
            End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("Page_Load", , ex)
        End Try

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        mRILocalization.Dispose()
    End Sub

    Protected Sub _btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnOK.Click
        UserRequestedLanguageChange()
    End Sub

    Private Sub UserRequestedLanguageChange()
        'Save the default language
        Dim currentUser As IP.Bids.UserInfo

        currentUser = IP.Bids.SharedFunctions.GetCurrentUser
        If currentUser IsNot Nothing Then
            SaveDefaultLanguage(currentUser.Username, Me._rblLanguages.SelectedValue)
            IP.Bids.SharedFunctions.ClearCurrentUserSession()
            'IP.Bids.SharedFunctions.DeleteEntireCache()
            IP.Bids.SharedFunctions.InitCulture(IP.Bids.SharedFunctions.DataClean(Me._rblLanguages.SelectedValue, "EN-US"))
            ' IP.Bids.SharedFunctions.DisablePageCache(Page.Response)
            IP.Bids.SharedFunctions.ResponseRedirect(Page.ResolveUrl(Page.AppRelativeVirtualPath & Request.Url.Query))
            'Server.Transfer(Page.ResolveUrl("~/DetectScreenResolution.aspx")) ' & "?url=" & Server.HtmlEncode(Page.Request.RawUrl))

            'HttpContext.Current.ApplicationInstance.CompleteRequest()
            'Server.Transfer(Page.AppRelativeVirtualPath & Request.Url.Query, False)

        End If
        '(in_username IN varchar2,in_defaultlanguage in varchar2)
    End Sub
    Private Sub SaveDefaultLanguage(ByVal userName As String, ByVal defaultLanguage As String)
        Dim cmdSQL As OracleCommand = Nothing
        Dim connection As String = String.Empty
        Dim provider As String = String.Empty
        Dim cnConnection As OracleConnection = Nothing
        Dim userDomain As String() = Nothing
        Dim out_status As Decimal = 0

        Try
            Dim currentUser As IP.Bids.UserInfo
            Dim updateemployee As New DataMaintenanceDALTableAdapters.EmployeeTableAdapter
            currentUser = IP.Bids.SharedFunctions.GetCurrentUser
            If currentUser IsNot Nothing Then
                With currentUser
                    updateemployee.UPDATEEMPLOYEE(.Username, .Domain, .FirstName, .LastName, .Email, .PhoneNumber, "N", defaultLanguage, .PlantCode, "", userName, .MiddleInitial, "", "", out_status)
                End With
            End If
        Catch ex As Exception

        End Try
        'Try
        '    If connection.Length = 0 Then
        '        connection = ConfigurationManager.ConnectionStrings.Item("connectionRCFATST").ConnectionString
        '    End If
        '    If provider.Length = 0 Then
        '        provider = ConfigurationManager.ConnectionStrings.Item("connectionRCFATST").ProviderName
        '    End If


        '    If userName Is Nothing OrElse userName.Length = 0 Then
        '        Throw New UnauthorizedAccessException("Invalid username specified (" & userName & ").")
        '        Exit Sub
        '    End If


        '    cmdSQL = New OracleCommand
        '    With cmdSQL
        '        cnConnection = New OracleConnection(connection)
        '        cnConnection.Open()
        '        .Connection = cnConnection
        '        .CommandText = "ri.SaveDefaultLanguage"
        '        .CommandType = CommandType.StoredProcedure
        '        Dim param As New OracleParameter

        '        param.ParameterName = "IN_USERNAME"
        '        param.OracledbType = OracledbType.VarChar
        '        param.Value = userName
        '        param.Direction = ParameterDirection.Input
        '        .Parameters.Add(param)

        '        param = New OracleParameter
        '        param.ParameterName = "IN_DEFAULTLANGUAGE"
        '        param.OracledbType = OracledbType.VarChar
        '        param.Value = defaultLanguage
        '        param.Direction = ParameterDirection.Input
        '        .Parameters.Add(param)
        '    End With


        '    Dim rowsAffected As Integer = cmdSQL.ExecuteNonQuery()
        'Catch
        '    Throw
        'Finally
        '    If cnConnection IsNot Nothing Then cnConnection = Nothing
        '    If Not cmdSQL Is Nothing Then cmdSQL = Nothing
        '    'cnConnection.Close()
        'End Try
    End Sub

    Private Sub Languages_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _rblLanguages.SelectedIndexChanged
        UserRequestedLanguageChange()
    End Sub
End Class
