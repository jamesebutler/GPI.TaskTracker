'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 08-17-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Security.Principal
Imports System.DirectoryServices
Imports System.Web.TraceContext
Imports System
Imports System.Text
Imports System.Collections
Imports System.Globalization
Imports System.Linq

Namespace IP.Bids
    ''' <summary>
    ''' The CurrentUserProfile module contains properties and procedures for creating a user profile for the current user
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()>
    Public Class CurrentUserProfile
#Region "Fields"
        Private _Username As String = String.Empty
        Private _Domain As String = "NA"
        Private _InActiveFlag As String = String.Empty
        Private _FullName As String = String.Empty
        Private _Groups() As String
        Private _PropertyNameValueCollection As Specialized.OrderedDictionary
        Private _DistinguishedName As String = String.Empty
        Private _MemberOf As String = String.Empty
        Private _EMail As String = String.Empty
        Private _ProfileTable As String = String.Empty
        Private _IsAuthenticated As Boolean ' = False
#End Region

#Region "Properties"
        ''' <summary>
        ''' Gets the Authenticated status of the specified user
        ''' </summary>
        ''' <returns>Boolean - Returns true if the user has successfully authenticated, returns false if the user's authentication has failed</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsAuthenticated() As Boolean
            Get
                Return _IsAuthenticated
            End Get
        End Property

        ''' <summary>
        ''' Gets the email address for the current user if one exist
        ''' </summary>
        ''' <returns>String - Returns the email address for the current user if one exist</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Email() As String
            Get
                Return _EMail
            End Get
        End Property

        ''' <summary>
        ''' Gets the InActiveFlag value for the current user
        ''' </summary>
        ''' <returns>String - Returns the InActiveFlag value for the current user.  Possible values are D - Divested, Y - Yes, N - No</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property InActiveFlag() As String
            Get
                Return _InActiveFlag
            End Get
        End Property

        ''' <summary>
        ''' Gets the network username for the current user
        ''' </summary>
        ''' <returns>String - Returns the network username for the current user (i.e. mjpope)</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Username() As String
            Get
                Return _Username
            End Get
        End Property

        ''' <summary>
        ''' Gets the domain name for the current user (i.e. naipaper)
        ''' </summary>
        ''' <returns>String - Returns the domain name for the current user (i.e. naipaper)</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DomainName() As String
            Get
                Return _Domain
            End Get
        End Property

        ''' <summary>
        ''' Gets the Full name of the current user as specified in LDAP
        ''' </summary>
        ''' <returns>String - Returns the full name of the current user as specified in LDAP</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FullName() As String
            Get
                Return _FullName
            End Get
        End Property

        ''' <summary>
        ''' Gets the user profile table for the current authenticated user
        ''' </summary>
        ''' <returns>String - Returns an HTML table representation of the user profile</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ProfileTable() As String
            Get
                Return _ProfileTable
            End Get

        End Property

        ''' <summary>
        ''' Gets the Groups that the user is a memember of
        ''' </summary>
        ''' <returns>String - </returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GroupName() As String
            Get
                Return _MemberOf
            End Get
        End Property

        ''' <summary>
        ''' Gets an array of the Groups that the user has access to
        ''' </summary>
        ''' <returns>String Array - Returns an array of the groups that a user has access to</returns>
        ''' <remarks></remarks>
        Public Function GetGroups() As String()
            Return _Groups
        End Function


        Public ReadOnly Property PropertyNameValueCollection() As Specialized.OrderedDictionary
            Get
                Return _PropertyNameValueCollection
            End Get
        End Property

        ''' <summary>
        ''' Gets the distinguished name
        ''' </summary>
        ''' <returns>String - Returns the distinguished name</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DistinguishedName() As String
            Get
                Return _DistinguishedName
            End Get
        End Property

#End Region
        Public Function GetProfileTableHtml() As String
            Dim sb As New StringBuilder
            Dim tblRow As String = "<tr><td style='text-align:left;width:200px'><b>{0}</b></td><td style='text-align:left'>{1}</td></tr>"
            With Me
                sb.Append("<table 'width=100%' border=0 cellpadding=2 cellspacing=1>")
                sb.Append(String.Format(CultureInfo.CurrentCulture, tblRow, "Group Name:", ._MemberOf))

                For Each obj As DictionaryEntry In Me._PropertyNameValueCollection
                    sb.Append(String.Format(CultureInfo.CurrentCulture, tblRow, obj.Key, obj.Value))
                Next

                sb.Append("</table>")
                Return sb.ToString
            End With
        End Function

        ''' <summary>
        ''' Provides the name for the current user (i.e. naipaper\username)
        ''' </summary>
        ''' <returns>String - Returns the name for the current user (i.e. naipaper\username)</returns>
        ''' <remarks></remarks>
        Public Shared Function GetCurrentUser() As String
            Dim user As System.Security.Principal.IPrincipal
            Dim currentUser As String = String.Empty

            Try
                If System.Web.HttpContext.Current.Session IsNot Nothing Then
                    If System.Web.HttpContext.Current.Session.Item("CurrentUser") IsNot Nothing Then
                        currentUser = CStr(System.Web.HttpContext.Current.Session.Item("CurrentUser"))
                    End If
                End If
                If currentUser Is Nothing Then
                    currentUser = String.Empty
                End If
                If currentUser.Length = 0 Then
                    If System.Web.HttpContext.Current.User.Identity.IsAuthenticated Then
                        user = System.Web.HttpContext.Current.User
                        If user IsNot Nothing Then
                            currentUser = user.Identity.Name
                            If System.Web.HttpContext.Current.Session IsNot Nothing Then
                                System.Web.HttpContext.Current.Session.Add("CurrentUser", currentUser)
                            End If
                        End If
                    Else
                        'We probably need to force a login
                    End If
                    'Supply a username and domain below to impersonate a user                    
                End If
            Catch ex As Exception
                Throw
            End Try
            Return currentUser
        End Function

        ''' <summary>
        ''' Parses the CN field from the ADS Path
        ''' </summary>
        ''' <param name="path">String - Active Directory Service Path(ADS Path)</param>
        ''' <returns>String - the CN field from the ADS Path</returns>
        ''' <remarks></remarks>
        Private Function TrimToName(ByVal path As String) As String
            Dim parts() As String
            Dim cn As String = String.Empty
            Try
                parts = path.Split(CChar(","))
                cn = parts(0).Replace("CN=", String.Empty)
            Catch ex As Exception
                Throw
            Finally
                TrimToName = cn
            End Try
        End Function

        Public Function Authenticate(ByVal domain As String, ByVal username As String, ByVal password As String) As Boolean
            'If password.Length = 0 Then
            '    _IsAuthenticated = True
            'Else
            If domain.Length = 0 Then domain = "NA"
            GetUserProfile(domain & "\" & username, password)
            Dim listOfDomains = {"ICC_GO", "IPCFPULP"}
            If listOfDomains.Contains(Me.DomainName.ToUpper) Then
                Dim newUserName As String = String.Empty
                newUserName = Me.GetNAIPAPERUserNameFromEmail(Me.Email)
                If newUserName.Length > 0 Then
                    GetUserProfile("NA" & "\" & newUserName, "")
                End If
            End If
            'End If

            Return _IsAuthenticated
        End Function
        ''' <summary>
        ''' Creates a new instance of the AuthUser public class for the current user.
        ''' </summary>
        ''' <returns>AuthUser - Returns an instance of the AuthUser public class</returns>
        ''' <remarks></remarks>
        Public Function GetAuthUser() As CurrentUserProfile
            Dim myAuthUser As CurrentUserProfile = Nothing
            Dim username As String

            Try
                username = GetCurrentUser()
                If username.Length > 0 Then
                    'If HttpContext.Current.Session.Item("UserProfile_" & username) IsNot Nothing Then
                    '    myAuthUser = TryCast(HttpContext.Current.Session.Item("UserProfile_" & username), CurrentUserProfile)                        
                    'End If 
                    If myAuthUser Is Nothing Then
                        GetUserProfile(username)
                        myAuthUser = Me
                        'HttpContext.Current.Session.Add("UserProfile_" & username, myAuthUser)
                    End If

                End If
            Catch ex As Exception
                Throw
            Finally
                GetAuthUser = myAuthUser
                If myAuthUser IsNot Nothing Then
                    myAuthUser = Nothing
                End If
            End Try
        End Function


        ''' <summary>
        ''' Populates the AuthUser public class with the User Profile information for the current user
        ''' </summary>
        ''' <param name="username">String - The current user (i.e. naipaper\username) </param>
        ''' <remarks>Assumes that the following import statements have been added:
        ''' -- Imports system.Security.Principal
        ''' -- Imports System.DirectoryServices</remarks>
        Private Sub GetUserProfile(ByVal username As String, Optional ByVal password As String = "")
            'Dim enTry As DirectoryEntry = Nothing
            Dim userDomain As String()
            Try
                If username.Length = 0 Then
                    Throw New UnauthorizedAccessException("Required Username is missing!")
                    Exit Sub
                End If

                userDomain = username.Split(CChar("\"))

                If userDomain.Length = 2 Then
                    _Username = userDomain(1)
                    _Domain = userDomain(0)
                Else
                    Throw New UnauthorizedAccessException("Invalid username specified (" & username & ").")
                    Exit Sub
                End If

                '"Use LDAP Service"

                Dim ldap As LdapProfile = Nothing
                'Dim propertiesToLoad As New ArrayList
                'propertiesToLoad.Add("DisplayName")
                If password.Length = 0 Then
                    'ldap = GetLDAPUser(_Username, _Domain, "", propertiesToLoad)
                    If _Domain <> "APACIPAPER" Then
                        ldap = GetLdapUser(_Username, _Domain, "")
                    End If
                    _IsAuthenticated = True
                Else
                    ldap = GetLdapUser(_Username, _Domain, password)
                End If

                '                Dim sb As New StringBuilder ' COMMENTED BY CODEIT.RIGHT
                If ldap IsNot Nothing AndAlso ldap.Adspath IsNot Nothing Then
                    _DistinguishedName = ldap.Distinguishedname
                    _Groups = Split(ldap.Memberof, ",")
                    _MemberOf = ldap.Memberof
                    If ldap.Displayname IsNot Nothing AndAlso ldap.Displayname.Length > 0 Then
                        _FullName = ldap.Displayname
                    Else
                        _FullName = ldap.Name
                    End If
                    If ldap.Mail IsNot Nothing AndAlso ldap.Mail.Length > 0 Then
                        _EMail = ldap.Mail
                    Else
                        _EMail = _Username.Trim & "@graphicpkg.com"
                    End If

                    _PropertyNameValueCollection = New Specialized.OrderedDictionary

                    For Each dr As DataRow In ldap.PropertyNameValueDataTable.Rows
                        For Each col As DataColumn In dr.Table.Columns
                            Me._PropertyNameValueCollection.Add(col.ColumnName, dr.Item(col.ColumnName))
                        Next
                    Next
                    _IsAuthenticated = True
                Else
                    _IsAuthenticated = False
                End If

                Exit Try

            Catch
                Dim grp() As String = Nothing
                _Groups = grp
                Throw
            Finally
            End Try
        End Sub


        ''' <summary>
        ''' Creates a new instance of the CurrentUserProfile class
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.GetAuthUser()
            'Me.GetAllUsers()
        End Sub

        ''' <summary>
        ''' Creates a new instance of the CurrentUserProfile class
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal domain As String, ByVal userName As String, ByVal password As String)
            Me.Authenticate(domain, userName, password)
        End Sub

        ''' <summary>
        ''' Creates a new instance of the CurrentUserProfile class
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal domain As String, ByVal userName As String)
            Me.Authenticate(domain, userName, "")
        End Sub

        Public Function GetLdapUser(ByVal userName As String, ByVal domainName As String, ByVal password As String, Optional ByVal propertiesToLoad As ArrayList = Nothing) As LdapProfile
            ' setting up the lookup to AD
            Dim ldapUser As String = System.Configuration.ConfigurationManager.AppSettings("LDAPUserName").ToString()
            Dim ldapPassword As String = System.Configuration.ConfigurationManager.AppSettings("LDAPPassword").ToString
            Dim ldapPath As String = System.Configuration.ConfigurationManager.AppSettings("LDAPPath").ToString
            Dim ldap As New LdapProfile
            Dim dt As New DataTable("AD_Users")
            Dim adEntry As DirectoryEntry
            Dim adSearcher As DirectorySearcher

            If userName Is Nothing OrElse userName.Length = 0 Then Return Nothing

            If password.Length = 0 Then
                adEntry = New DirectoryEntry("LDAP://" & domainName, ldapUser, ldapPassword, AuthenticationTypes.Secure)
                'adEntry = New DirectoryEntry(ldapPath, ldapUser, ldapPassword, AuthenticationTypes.Secure)
                System.Diagnostics.Debug.Print("ldappath:" & ldapPath)
                IP.Bids.SharedFunctions.InsertAuditRecord("GetLdapUser", "LDAP://" & domainName & "\" & ldapUser & " for " & userName)
            Else
                adEntry = New DirectoryEntry("LDAP://" & domainName, userName, password, AuthenticationTypes.Secure)
            End If


            'enTry.Path = "LDAP://" & DomainName
            If domainName.ToUpper = "NA" Then
                adEntry.Path = "LDAP://na.ipaper.com"
            ElseIf domainName.ToUpper = "ICC_GO" Then
                adEntry.Path = "LDAP://myinland.com"
            Else
                adEntry.Path = "LDAP://" & domainName
            End If
            'adEntry.Path = "LDAP://" & domainName

            ' define which fields to retrieve from AD

            Try

                adSearcher = New DirectorySearcher(adEntry)
                If adSearcher IsNot Nothing Then
                    With adSearcher
                        .Filter = "(&(objectClass=user)(samAccountName=" & userName & ")(objectCategory=person))"
                        .PropertyNamesOnly = False
                        .CacheResults = True
                        If propertiesToLoad IsNot Nothing Then
                            For i As Integer = 0 To propertiesToLoad.Count - 1
                                .PropertiesToLoad.Add(CStr(propertiesToLoad(i)))
                            Next
                        End If

                    End With
                End If
                ' define a datatable and add the results to it

                Dim adResults As SearchResult
                'With adSearcher
                '    .ServerTimeLimit = New TimeSpan(0, 2, 0)
                '    .ClientTimeout = New TimeSpan(0, 2, 0)
                '    .ServerPageTimeLimit = New TimeSpan(0, 2, 0)                    
                'End With
                System.Diagnostics.Debug.Print(String.Format("ClientTimeout:{0} - ServerPageTimeLimit:{1} - ServerTimeLimit:{2}", adSearcher.ClientTimeout, adSearcher.ServerPageTimeLimit, adSearcher.ServerTimeLimit))
                Try
                    adResults = adSearcher.FindOne
                Catch ex As Exception
                    IP.Bids.SharedFunctions.HandleError(excep:=ex, friendlyMessage:="adSearcher.FindOne failed for (&(objectClass=user)(samAccountName=" & userName & ")(objectCategory=person))")
                    adResults = Nothing
                End Try


                Dim dr As DataRow

                If adResults IsNot Nothing Then
                    Dim propertyCount As Integer = 0
                    propertyCount = propertyCount + 1
                    ' add the results to the datatable
                    dr = dt.NewRow()
                    Dim ienum As IEnumerator
                    ienum = adResults.Properties.PropertyNames.GetEnumerator

                    Try
                        '                        Dim PropertyNameValueCollection As New Specialized.OrderedDictionary ' COMMENTED BY CODEIT.RIGHT
                        While ienum.MoveNext
                            If propertyCount = 1 Then
                                If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                    dt.Columns.Add(New DataColumn(ienum.Current.ToString, GetType(System.String)))
                                End If
                            End If
                            If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                dr.Item(ienum.Current.ToString) = (adResults.Properties.Item(ienum.Current.ToString)(0))
                            End If

                            Select Case ienum.Current.ToString.ToUpper
                                Case UCase("distinguishedname")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Distinguishedname = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("countrycode")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Countrycode = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("cn")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.CN = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("lastlogoff")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Lastlogoff = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("useraccountcontrol")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Useraccountcontrol = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("usncreated")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Usncreated = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                    'Case UCase("objectguid")
                                    'If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                    'ldap.objectguid = (adResults.Properties.Item(ienum.Current.ToString)(0))
                                    'End If
                                Case UCase("postalcode")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Postalcode = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("whenchanged")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Whenchanged = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("memberof")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Memberof = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("accountexpires")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Accountexpires = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("displayname")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Displayname = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("employeenumber")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Employeenumber = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("primarygroupid")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Primarygroupid = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("streetaddress")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Streetaddress = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("badpwdcount")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Badpwdcount = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("objectclass")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Objectclass = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("objectcategory")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Objectcategory = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("instancetype")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Instancetype = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("homedrive")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Homedrive = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("samaccounttype")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Samaccounttype = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("homedirectory")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Homedirectory = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("whencreated")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Whencreated = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("lastlogon")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Lastlogon = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("l")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.L = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("st")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.ST = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("co")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.CO = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("title")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Title = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("c")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.C = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("samaccountname")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Samaccountname = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("employeetype")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Employeetype = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("givenname")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Givenname = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("mail")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Mail = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("adspath")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Adspath = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("lockouttime")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Lockouttime = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("scriptpath")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Scriptpath = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("pwdlastset")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Pwdlastset = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("logoncount")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Logoncount = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("codepage")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Codepage = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("name")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Name = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("physicaldeliveryofficename")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Physicaldeliveryofficename = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("manager")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Manager = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("usnchanged")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Usnchanged = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                    'Case UCase("dscorepropagationdata")
                                    '    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                    '        ldap.dscorepropagationdata = (adResults.Properties.Item(ienum.Current.ToString)(0))
                                    '    End If
                                Case UCase("userprincipalname")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Userprincipalname = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("badpasswordtime")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Badpasswordtime = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("objectsid")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        'ldap.objectsid = (adResults.Properties.Item(ienum.Current.ToString)(0))
                                    End If
                                Case UCase("sn")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.SN = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                Case UCase("telephonenumber")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Telephonenumber = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                                    'Case UCase("logonhours")
                                    '    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                    '        ldap.logonhours = (adResults.Properties.Item(ienum.Current.ToString)(0))
                                    '    End If
                                Case UCase("lastlogontimestamp")
                                    If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                                        ldap.Lastlogontimestamp = CStr((adResults.Properties.Item(ienum.Current.ToString)(0)))
                                    End If
                            End Select

                            'If adResults.Properties.Item(CStr(ienum.Current)).Count > 0 Then
                            '    PropertyNameValueCollection.Add(ienum.Current.ToString, adResults.Properties.Item(ienum.Current.ToString)(0))
                            'End If
                        End While
                        If ldap.Distinguishedname.Contains("OU=Services") = True Then
                            ldap = Nothing
                        End If
                    Catch
                        ldap = Nothing
                        Throw
                    End Try
                    dt.Rows.Add(dr)
                End If
                If ldap IsNot Nothing Then
                    ldap.PropertyNameValueDataTable = dt
                End If
            Catch
                ldap = Nothing
                Throw
            Finally
                dt = Nothing
                adEntry = Nothing
                adSearcher = Nothing
            End Try

            Return ldap

        End Function

        Public Function GetNAIPAPERUserNameFromEmail(ByVal emailAddress As String) As String
            Dim ldapUser As String = System.Configuration.ConfigurationManager.AppSettings("LDAPUserName").ToString()
            Dim ldapPassword As String = System.Configuration.ConfigurationManager.AppSettings("LDAPPassword").ToString
            Dim ldapPath As String = System.Configuration.ConfigurationManager.AppSettings("LDAPPath").ToString
            Dim ldap As New LdapProfile
            Dim adEntry As DirectoryEntry
            Dim userName As String = String.Empty
            Dim adSearcher As DirectorySearcher
            Dim adResults As SearchResult = Nothing

            Try
                If emailAddress Is Nothing Then Throw New ArgumentNullException("Missing email address")
                adEntry = New DirectoryEntry(ldapPath, ldapUser, ldapPassword, AuthenticationTypes.Secure)
                'adEntry.Path = "LDAP://NAIPAPER"
                adEntry.Path = "LDAP://GRAPHICPKG.PRI"


                adSearcher = New DirectorySearcher(adEntry)
                If adSearcher IsNot Nothing Then
                    With adSearcher
                        .Filter = "(&(objectClass=user)(OBJECTCATEGORY=Person)(mail=" & emailAddress & "))"
                        .PropertyNamesOnly = False
                        .CacheResults = True
                        .SizeLimit = 5 'SizeLimit
                        .PropertiesToLoad.Add("SAMACCOUNTNAME") 'Username                    
                    End With


                    adResults = adSearcher.FindOne

                    If adResults IsNot Nothing Then
                        If adResults.Properties.Item("samaccountname").Count > 0 Then
                            userName = CStr(adResults.Properties.Item("samaccountname").Item(0))
                        End If
                    End If
                End If
            Catch
                IP.Bids.SharedFunctions.InsertAuditRecord("GetNAIPAPERUserNameFromEmail", String.Format("Error getting the user name - Username:{0}, Email Address:{1} {2}", userName, emailAddress, adResults.ToString))
                Throw
            Finally
                adSearcher = Nothing
                adEntry = Nothing
            End Try
            IP.Bids.SharedFunctions.InsertAuditRecord("GetNAIPAPERUserNameFromEmail", String.Format("Username:{0}, Email Address:{1}", userName, emailAddress))
            Return userName
        End Function
    End Class



End Namespace
