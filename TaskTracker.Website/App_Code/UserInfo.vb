'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 09-09-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-09-2010
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
Imports Devart.Data.Oracle


Namespace IP.Bids
    <Serializable()> _
    Public Class UserInfo
#Region "Fields"
        Private _UserName As String = String.Empty
        Private _Name As String = String.Empty
        'Private _UserID As Integer = -1
        Private _Domain As String = String.Empty
        Private _IsActive As Boolean
        Private _IsAuthenticated As Boolean
        Private _Groups As String()
        Private _IsUserInEmployeeTable As Boolean
        Private _UserDefaults As RIUserDefaults.CurrentUserDefaults  'Specialized.StringDictionary = Nothing
        Private _Email As String = String.Empty
        Private _FirstName As String = String.Empty
        Private _LastName As String = String.Empty
        Private _MiddleInitial As String = String.Empty
        Private _PhoneNumber As String = String.Empty
        Private _PlantCode As String = String.Empty

        'Private _ProfileTableHtml As String
#End Region

#Region "Properties"
        ''' <summary>
        ''' Gets or sets the user defaults.
        ''' </summary>
        ''' <value>The user defaults.</value>
        Public ReadOnly Property UserDefaults() As RIUserDefaults.CurrentUserDefaults
            Get
                Return _UserDefaults
            End Get
        End Property

        'Public ReadOnly Property ProfileTableHtml() As String
        '    Get
        '        Return _ProfileTableHtml
        '    End Get
        'End Property
        ''' <summary>
        ''' Gets or sets whether or not the user is in the Employee table
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsUserInEmployeeTable() As Boolean
            Get
                Return _IsUserInEmployeeTable
            End Get
            Set(ByVal value As Boolean)
                _IsUserInEmployeeTable = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the network username for the current user
        ''' </summary>
        ''' <returns>String - Returns the network username for the current user (i.e. mjpope)</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Username() As String
            Get
                Return _UserName.ToUpper
            End Get
        End Property

        ''' <summary>
        ''' Gets the domain for the current user
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Domain() As String
            Get
                Return _Domain
            End Get
        End Property

        Public ReadOnly Property Email() As String
            Get
                Return _Email
            End Get
        End Property

        Public ReadOnly Property FirstName() As String
            Get
                Return _FirstName
            End Get
        End Property

        Public ReadOnly Property LastName() As String
            Get
                Return _LastName
            End Get
        End Property

        Public ReadOnly Property MiddleInitial() As String
            Get
                Return _MiddleInitial
            End Get
        End Property

        Public ReadOnly Property PhoneNumber() As String
            Get
                Return _PhoneNumber
            End Get
        End Property
        Public ReadOnly Property PlantCode() As String
            Get
                Return _PlantCode
            End Get
        End Property
        '''' <summary>
        '''' Gets the UserID for the current user 
        '''' </summary>
        '''' <returns>String - Returns the UserID for the current user</returns>
        '''' <remarks></remarks>
        'Public ReadOnly Property UserID() As Integer
        '    Get
        '        Return _UserID
        '    End Get
        'End Property

        ''' <summary>
        ''' Gets the Full name of the current user 
        ''' </summary>
        ''' <returns>String - Returns the full name of the current user</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Name() As String
            Get
                Return _Name
            End Get
        End Property



        ''' <summary>
        ''' Gets information about the status of the current user
        ''' </summary>
        ''' <value></value>
        ''' <returns>Boolean - Returns True if the user is marked as Active, otherwise False</returns>
        ''' <remarks></remarks>
        Public Property IsActive() As Boolean
            Get
                Return _IsActive
            End Get
            Set(ByVal value As Boolean)
                _IsActive = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a value indicating whether this instance is authenticated.
        ''' </summary>
        ''' <value>
        ''' <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        ''' </value>
        Public ReadOnly Property IsAuthenticated() As Boolean
            Get
                Return _IsAuthenticated
            End Get
        End Property

        ''' <summary>
        ''' Gets an array of the Groups that the user has access to
        ''' </summary>
        ''' <returns>String Array - Returns an array of the groups that a user has access to</returns>
        ''' <remarks></remarks>
        Public Function GetGetGroups() As String()
            Return _Groups
        End Function


#End Region

#Region "Methods"
        ''' <summary>
        ''' Populates the user default.
        ''' </summary>
        ''' <param name="userName">Name of the user.</param>
        Private Sub PopulateUserDefault(ByVal userName As String)

            Dim FilePath As String = HttpContext.Current.Server.MapPath("~\\TraceLog\\")
            IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "UserInfo_PopulateUserDefault.txt", "PopulateUserDefault Start : " & userName & " " & DateTime.Now.ToString, True)


            Dim taskList As New TaskTrackerListsBll
            Dim user() As String = userName.Split(CChar("\"))
            If user.Length >= 2 Then
                userName = user(1)
            End If
            _UserDefaults = taskList.GetUserDefaults(userName)
            taskList = Nothing

            IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "UserInfo_PopulateUserDefault.txt", "PopulateUserDefault END : " & userName & " " & DateTime.Now.ToString, False)


        End Sub
#End Region
        Private Sub SetProfile(ByVal domain As String, ByVal userName As String)

            Dim FilePath As String = HttpContext.Current.Server.MapPath("~\\TraceLog\\")
            IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "UserInfo_SetProfile.txt", "SetProfile Start : " & userName & " " & DateTime.Now.ToString, True)

            Dim userProfile As CurrentUserProfile
            Dim userRow As DataMaintenanceDAL.EmployeeRow


            Dim cacheKeySetProfile As String = "userName" & PlantCode & "_SetProfile_" & UCase(userName)
            Dim cacheHoursSetProfile As Integer = 1
            Dim useruserRowCache As DataMaintenanceDAL.EmployeeRow

            If userName.Length > 0 Then
                useruserRowCache = CType(HelperDal.GetDataFromCache(cacheKeySetProfile, cacheHoursSetProfile), DataMaintenanceDAL.EmployeeRow)
                If useruserRowCache IsNot Nothing Then



                    userRow = useruserRowCache
                    With Me
                        ._Name = userRow.FIRSTNAME & " " & userRow.LASTNAME
                        ._IsAuthenticated = True 'userProfile.IsAuthenticated
                        .IsUserInEmployeeTable = True
                        ._Domain = domain 'userRow.DOMAIN
                        ._UserName = userName
                        If userRow.INACTIVE_FLAG.ToUpper = "Y" Then
                            ._IsActive = False
                        Else
                            .IsActive = True
                        End If
                        ._Email = userRow.EMAIL
                        ._FirstName = userRow.FIRSTNAME
                        ._LastName = userRow.LASTNAME
                        ._PlantCode = userRow.PLANTCODE
                        '.PhoneNumber = userRow.phonenumber
                        '.MiddleInitial = userRow.mi
                    End With
                    userRow = Nothing

                    PopulateUserDefault(userName)
                    userProfile = Nothing

                    IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "UserInfo_SetProfile.txt", "SetProfile CACHE End : " & userName & " " & DateTime.Now.ToString, False)


                    Exit Sub

                End If
            End If





            'Dim mUserProfile As New CurrentUserProfile(Domain, Username, "")

            'If userProfile IsNot Nothing Then
            userRow = Me.GetEmployeeRecord(userName)




            If userRow IsNot Nothing Then
                If domain.Length = 0 Then
                    domain = userRow.DOMAIN
                End If
            End If

            If userRow IsNot Nothing AndAlso userRow.USERNAME.Length > 0 AndAlso userRow.DOMAIN.ToUpper = domain.ToUpper Then

                HelperDal.InsertDataIntoCache(cacheKeySetProfile, cacheHoursSetProfile, userRow)

                With Me
                    ._Name = userRow.FIRSTNAME & " " & userRow.LASTNAME
                    ._IsAuthenticated = True 'userProfile.IsAuthenticated
                    .IsUserInEmployeeTable = True
                    ._Domain = domain 'userRow.DOMAIN
                    ._UserName = userName
                    If userRow.INACTIVE_FLAG.ToUpper = "Y" Then
                        ._IsActive = False
                    Else
                        .IsActive = True
                    End If
                    ._Email = userRow.EMAIL
                    ._FirstName = userRow.FIRSTNAME
                    ._LastName = userRow.LASTNAME
                    ._PlantCode = userRow.PLANTCODE
                    '.PhoneNumber = userRow.phonenumber
                    '.MiddleInitial = userRow.mi
                End With
                userRow = Nothing
            Else
                userProfile = New CurrentUserProfile(domain, userName, "")

                If userProfile IsNot Nothing Then
                    'Check to see if user account is ICC_GO
                    Dim listOfDomains = {"ICC_GO", "IPCFPULP"}
                    If listOfDomains.Contains(domain.ToUpper) Then
                        domain = userProfile.DomainName
                        userName = userProfile.Username
                    End If
                    With Me
                            ._Name = userProfile.FullName
                            ._IsAuthenticated = userProfile.IsAuthenticated

                            userRow = Me.GetEmployeeRecord(userProfile.Username)
                            If userRow IsNot Nothing AndAlso userRow.USERNAME.Length > 0 AndAlso userRow.DOMAIN.ToUpper = userProfile.DomainName.ToUpper Then
                                .IsUserInEmployeeTable = True
                            Else
                                'Dim updateemployee As New DataMaintenanceDALTableAdapters.EmployeeTableAdapter
                                'updateemployee.UPDATEEMPLOYEE(Me._networkid.Text, Me._ddldomain.SelectedValue, Me._firstname.Text, Me._lastname.Text, Me._email.Text, Me._phone.Text, "N", Me._ddldefaultlang.SelectedValue, Me._ddlsitelist.SelectedValue, "", IP.Bids.SharedFunctions.GetCurrentUser.Username, Me._midinit.Text, "", "", out_status)
                                'Dim out_status As Decimal? = 0
                                'updateemployee.UPDATEEMPLOYEE(userProfile.Username, userProfile.DomainName, Me.FirstName, Me.LastName, userProfile.Email, "", "Y", "", Me.PlantCode, "", Me.Username, "", "", "", out_status)
                                'userRow = Me.GetEmployeeRecord(userProfile.Username)
                                'If userRow IsNot Nothing AndAlso userRow.USERNAME.Length > 0 AndAlso userRow.DOMAIN.ToUpper = userProfile.DomainName.ToUpper Then
                                '.IsUserInEmployeeTable = True
                                'Else
                                .IsUserInEmployeeTable = False
                                'End If
                            End If
                            userRow = Nothing
                            ._Domain = userProfile.DomainName
                            ._UserName = userName
                            ._IsActive = True
                            ._Email = userProfile.Email
                            Dim name() As String
                            Dim NameFromEmail() As String = _Email.Split(CChar("@"))
                            If NameFromEmail.Length = 2 Then
                                name = NameFromEmail(0).Split(CChar("."))
                                If name.Length >= 2 Then
                                    ._FirstName = name(0)
                                    ._MiddleInitial = name(1)
                                    If ._MiddleInitial.Length > 1 Then
                                        ._MiddleInitial = ""
                                        ._LastName = name(1)
                                    Else
                                        ._LastName = name(2)
                                    End If
                                End If
                            Else
                                name = _Name.Split(CChar(" "))
                                If name.Length >= 2 Then
                                    ._FirstName = name(0)
                                    ._MiddleInitial = name(1)
                                    If ._MiddleInitial.Length > 1 Then
                                        ._MiddleInitial = ""
                                        ._LastName = name(1)
                                    Else
                                        ._LastName = name(2)
                                    End If
                                End If
                            End If


                        End With
                        Dim sb As New StringBuilder
                        If userProfile IsNot Nothing AndAlso userProfile.PropertyNameValueCollection IsNot Nothing Then
                            For Each i As DictionaryEntry In userProfile.PropertyNameValueCollection
                                If i.Key IsNot Nothing AndAlso i.Value IsNot Nothing Then
                                    sb.AppendLine(i.Key.ToString & "-" & i.Value.ToString)
                                End If
                            Next
                        End If
                    End If
                End If


            PopulateUserDefault(userName)
            userProfile = Nothing

            IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "UserInfo_SetProfile.txt", "SetProfile END : " & userName & " " & DateTime.Now.ToString, False)


            'End If
        End Sub
        Public Sub New()
            Dim user() As String = My.User.Name.Split(CChar("\"))
            'Dim user() As String = "APACIPAPER\BUNDAVA".Split(CChar("\")) 'My.User.Name.Split(CChar("\"))
            If user.Length = 2 Then
                _Domain = user(0)
                _UserName = user(1)


                'Dim mUserProfile As New CurrentUserProfile(Domain, Username, "")
                'If mUserProfile IsNot Nothing Then
                'SetProfile(_UserName, mUserProfile)
                SetProfile(Domain, _UserName)
                'End If


                'If mUserProfile IsNot Nothing Then
                '    Dim userRow As DataMaintenanceDAL.EmployeeRow = Me.GetEmployeeRecord(_UserName)
                '    If userRow IsNot Nothing Then
                '        With Me
                '            ._Name = userRow.FIRSTNAME & " " & userRow.LASTNAME
                '            ._IsAuthenticated = mUserProfile.IsAuthenticated
                '            .IsUserInEmployeeTable = True
                '            ._Domain = Domain
                '            ._UserName = Username
                '            If userRow.INACTIVE_FLAG.ToUpper = "Y" Then
                '                ._IsActive = False
                '            Else
                '                .IsActive = True
                '            End If
                '        End With
                '        userRow = Nothing
                '    Else
                '        With Me
                '            ._Name = mUserProfile.FullName
                '            ._IsAuthenticated = mUserProfile.IsAuthenticated
                '            .IsUserInEmployeeTable = False
                '            ._Domain = Domain
                '            ._UserName = Username
                '            ._IsActive = True
                '        End With
                '    End If                   
                '    PopulateUserDefault(Username)
                '    mUserProfile = Nothing
                'End If
            End If
        End Sub
        Public Sub New(ByVal domain As String, ByVal userName As String)
            'Dim mUserProfile As New CurrentUserProfile(domain, userName, "")
            'If mUserProfile IsNot Nothing Then
            SetProfile(domain, userName)
            'End If
            'If mUserProfile IsNot Nothing Then
            '    With Me
            '        ._Name = mUserProfile.FullName
            '        ._IsAuthenticated = mUserProfile.IsAuthenticated
            '        '._UserID = UserID
            '        .IsUserInEmployeeTable = False
            '        ._Domain = domain
            '        ._UserName = userName
            '        ._IsActive = True
            '    End With
            '    PopulateUserDefault(userName)
            '    mUserProfile = Nothing
            'End If
        End Sub
        Public Sub New(ByVal domain As String, ByVal userName As String, ByVal password As String)
            Dim mUserProfile As New CurrentUserProfile(domain, userName, password)
            If mUserProfile IsNot Nothing Then
                SetProfile(domain, userName)
            End If
            'If mUserProfile IsNot Nothing Then
            '    With Me
            '        ._Name = mUserProfile.FullName
            '        ._IsAuthenticated = mUserProfile.IsAuthenticated
            '        ._Domain = domain
            '        .IsUserInEmployeeTable = False
            '        ._UserName = userName
            '        ._IsActive = True
            '    End With
            '    PopulateUserDefault(userName)
            '    mUserProfile = Nothing
            'End If
        End Sub

        'Public Sub New(ByVal userName As String, ByVal fullName As String, ByVal isActive As Boolean)
        '    Dim user() As String = userName.Split(CChar("\"))


        '    If fullName.Length > 0 Then
        '        With Me
        '            ._Name = fullName
        '            ._IsAuthenticated = True
        '            .IsUserInEmployeeTable = True
        '            ._Domain = user(0)
        '            ._UserName = user(1)
        '            ._IsActive = isActive
        '        End With
        '    Else
        '        'Get the name of the current user from LDAP
        '        Dim mUserProfile As New CurrentUserProfile
        '        SetProfile(_UserName, mUserProfile)
        '        Me.IsActive = isActive
        '        'If mUserProfile IsNot Nothing Then
        '        '    With Me
        '        '        ._Name = mUserProfile.FullName
        '        '        ._IsAuthenticated = mUserProfile.IsAuthenticated
        '        '        '._UserID = userID
        '        '        ._UserName = userName
        '        '        ._IsActive = True
        '        '    End With
        '        'End If
        '    End If
        '    PopulateUserDefault(userName)
        'End Sub

        Private Function GetEmployeeRecord(ByVal userName As String) As DataMaintenanceDAL.EmployeeRow
            Dim userTA As New DataMaintenanceDALTableAdapters.EmployeeTableAdapter  ' UserDataTableAdapters.UserInfoTableAdapter
            Dim userDT As DataMaintenanceDAL.EmployeeDataTable = Nothing
            Dim userRow As DataMaintenanceDAL.EmployeeRow = Nothing

            Dim userDTCache As DataMaintenanceDAL.EmployeeDataTable = Nothing

            Try



                If userName = "" Then
                    Return Nothing
                End If

                Dim FilePath As String = HttpContext.Current.Server.MapPath("~\\TraceLog\\")
                IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "UserInfo_GetEmployeeRecord.txt", "GetEmployeeRecord Start : " & userName & " " & DateTime.Now.ToString, True)


                Dim cacheKeyGetEmployeeRecord As String = "userName" & PlantCode & "_" & UCase(userName)
                Dim cacheHoursGetEmployeeRecord As Integer = 1

                userDTCache = CType(HelperDal.GetDataFromCache(cacheKeyGetEmployeeRecord, cacheHoursGetEmployeeRecord), DataMaintenanceDAL.EmployeeDataTable)
                If userDTCache IsNot Nothing Then
                    userDT = userDTCache
                    IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "UserInfo_GetEmployeeRecord.txt", "GetEmployeeRecord CACHE End : " & DateTime.Now.ToString, False)

                Else
                    userTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                    userDT = userTA.GetUser(userName, Nothing) '= userTA.GetUser(userName)

                    HelperDal.InsertDataIntoCache(cacheKeyGetEmployeeRecord, cacheHoursGetEmployeeRecord, userDT)
                    IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "UserInfo_GetEmployeeRecord.txt", "GetEmployeeRecord GETUSER End : " & DateTime.Now.ToString, False)

                End If



                If userDT IsNot Nothing AndAlso userDT.Rows.Count = 0 Then
                    userDT = Nothing
                    Return Nothing
                Else
                    userRow = CType(userDT.Rows(0), DataMaintenanceDAL.EmployeeRow)
                    If userRow.IsINACTIVE_FLAGNull Then
                        userRow.INACTIVE_FLAG = "N"
                    End If
                    If userRow.IsFIRSTNAMENull Then
                        userRow.FIRSTNAME = ""
                    End If
                    If userRow.IsLASTNAMENull Then
                        userRow.LASTNAME = ""
                    End If
                    If userRow.IsDOMAINNull Then
                        userRow.DOMAIN = "NA"
                    End If
                    If userRow.IsEMAILNull Then
                        userRow.EMAIL = ""
                    End If
                    Return userRow
                End If
            Catch
                Throw
            Finally
                userTA = Nothing
                userDT = Nothing
            End Try
        End Function
    End Class
End Namespace
