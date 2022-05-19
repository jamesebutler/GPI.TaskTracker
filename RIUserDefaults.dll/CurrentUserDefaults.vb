Imports RIUserDefaults.UserDefaults

<Serializable()> _
Public Class CurrentUserDefaults
    Public Property UserDefaults As Generic.List(Of UserDefaultValues)
    Public Property ConnectionString As String

    Public Overrides Function ToString() As String
        If UserDefaults Is Nothing Then Return ""
        Dim sb As New Text.StringBuilder
        Dim index As Integer = 0
        For Each item In UserDefaults
            index += 1
            If sb.Length > 0 Then sb.Append(", ")
            sb.AppendFormat("{0}:{1}", index.ToString, item.ToString)
        Next
        Return sb.ToString
    End Function
    Public Sub New(ByVal userName As String, ByVal oracleConnectionString As String)
        ConnectionString = oracleConnectionString
        UserDefaults = GetUserDefaults(userName)
    End Sub

    Public Sub New(ByVal userName As String, ByVal application As Applications, ByVal oracleConnectionString As String)
        ConnectionString = oracleConnectionString
        UserDefaults = GetUserDefaults(userName, application)
    End Sub

    Public Function DoesDefaultValueExist(ByVal profileType As UserProfileTypes) As Boolean
        If UserDefaults Is Nothing Then Throw New MissingFieldException("There is no profile data for the current user")

        Dim hasValue = (From defaultValue In UserDefaults
         Where defaultValue.ProfileTypeName.Equals(profileType.ToString, StringComparison.InvariantCultureIgnoreCase)
         Select defaultValue).FirstOrDefault() IsNot Nothing

        Return hasValue
    End Function

    Public Function GetDefaultValue(ByVal profileType As UserProfileTypes) As String
        If UserDefaults Is Nothing Then Throw New MissingFieldException("There is no profile data for the current user")

        Dim values = From defaultvalue In UserDefaults _
                     Where defaultvalue.ProfileTypeName.Equals(profileType.ToString, StringComparison.InvariantCultureIgnoreCase) _
                     Select defaultvalue.ProfileTypeValue
        If values.Any() Then
            Return values.FirstOrDefault
        End If
        Return ""
    End Function

    Public Function GetDefaultValue(ByVal profileType As String) As String
        If UserDefaults Is Nothing Then Throw New MissingFieldException("There is no profile data for the current user")

        Dim values = From defaultvalue In UserDefaults _
                     Where defaultvalue.ProfileTypeName.Equals(profileType, StringComparison.InvariantCultureIgnoreCase) _
                     Select defaultvalue.ProfileTypeValue
        If values.Any() Then
            Return values.FirstOrDefault
        End If
        Return ""
    End Function

    Public Function GetDefaultValue(ByVal profileType As UserProfileTypes, ByVal defaultValueIfNoValueExist As String) As String
        If UserDefaults Is Nothing Then Throw New MissingFieldException("There is no profile data for the current user")

        Dim values = From defaultvalue In UserDefaults _
                     Where defaultvalue.ProfileTypeName.Equals(profileType.ToString, StringComparison.InvariantCultureIgnoreCase) _
                     Select defaultvalue.ProfileTypeValue
        If values.Any() Then
            Return values.FirstOrDefault
        End If
        Return defaultValueIfNoValueExist
    End Function

    Public Function GetDefaultValue(ByVal profileType As String, ByVal defaultValueIfNoValueExist As String) As String
        If UserDefaults Is Nothing Then Throw New MissingFieldException("There is no profile data for the current user")

        Dim values = From defaultvalue In UserDefaults _
                     Where defaultvalue.ProfileTypeName.Equals(profileType, StringComparison.InvariantCultureIgnoreCase) _
                     Select defaultvalue.ProfileTypeValue
        If values.Any() Then
            Return values.FirstOrDefault
        End If
        Return defaultValueIfNoValueExist
    End Function

    Private Function GetUserNameWithoutDomain(ByVal userName As String) As String
        Dim user() As String = userName.Split("\")
        If user.Length >= 2 Then Return user(1)
        Return userName
    End Function

    Public Function InsertOrUpdateDefaults(defaults As RIUserDefaults.UserDefaultValues) As String
        Dim userDefaultsTA As New UserDefaultsTableAdapters.UserDefaultsTableAdapter
        Dim status As Decimal?
        Dim currentUser As String() = My.User.Name.Split("\")
        Dim userName As String = My.User.Name
        Try

            If currentUser.Length > 1 Then userName = currentUser(1)
            userDefaultsTA.Connection = New Devart.Data.Oracle.OracleConnection(ConnectionString)
            userDefaultsTA.AddOrInsertUserDefaults(defaults.userName, defaults.ProfileTypeSeqId, defaults.ProfileTypeValue, defaults.Application.ToString, userName, status)
            If status <> 0 Then Throw New DataException(String.Format("InsertOrUpdateDefaults Failed for :{0}, Oracle Status:{1}", defaults.ToString, status))
            If defaults.ProfileTypeName.Equals(UserProfileTypes.PlantCode.ToString, StringComparison.InvariantCultureIgnoreCase) Then
                Dim siteInfo As New SiteHelper(defaults.ProfileTypeValue)
                userDefaultsTA.AddOrInsertUserDefaults(defaults.userName, UserProfileTypes.SiteId, siteInfo.SiteId, defaults.Application.ToString, userName, status)
                If status <> 0 Then Throw New DataException(String.Format("InsertOrUpdateDefaults Failed for :{0}, Oracle Status:{1}", defaults.ToString, status))
            End If

            userDefaultsTA.Dispose()
        Catch
            Throw
        End Try
        Return status.ToString
    End Function

    Private Function GetUserDefaults(ByVal userName As String) As Generic.List(Of UserDefaultValues)
        Dim userDefaultsTA As UserDefaultsTableAdapters.UserDefaultsTableAdapter
        Dim userDefaultsDT As UserDefaultsDataTable = Nothing
        Dim rsUserDefaults As Object = Nothing
        Dim userDefaultList As Generic.List(Of UserDefaultValues)

        Try
            userName = GetUserNameWithoutDomain(userName.ToUpper)
            userDefaultsTA = New UserDefaultsTableAdapters.UserDefaultsTableAdapter
            userDefaultsTA.Connection = New Devart.Data.Oracle.OracleConnection(ConnectionString)
            userDefaultsDT = userDefaultsTA.GetUserDefaults(userName, rsUserDefaults)
            userDefaultList = New Generic.List(Of UserDefaultValues)
            For Each rowItem As UserDefaultsRow In userDefaultsDT.Rows
                If rowItem.IsPROFILETYPENAMENull = False And rowItem.IsPROFILETYPEVALUENull = False Then
                    Dim j = New UserDefaultValues
                    With j
                        .AddNew(rowItem.USERNAME, rowItem.PROFILETYPESEQID, rowItem.APPLICATION, rowItem.PROFILETYPENAME, rowItem.PROFILETYPEVALUE)
                    End With
                    userDefaultList.Add(j)
                End If
            Next
           
        Catch
            Throw
        Finally
            userDefaultsTA = Nothing
            userDefaultsDT = Nothing
        End Try
        Return userDefaultList
    End Function

    Private Function GetUserDefaults(ByVal userName As String, ByVal application As Applications) As Generic.List(Of UserDefaultValues)
        Dim userDefaultsTA As UserDefaultsTableAdapters.UserDefaultsTableAdapter
        Dim userDefaultsDT As UserDefaultsDataTable = Nothing
        Dim rsUserDefaults As Object = Nothing
        Dim userDefaultList As Generic.List(Of UserDefaultValues)

        Try
            userName = GetUserNameWithoutDomain(userName.ToUpper)
            userDefaultsTA = New UserDefaultsTableAdapters.UserDefaultsTableAdapter
            userDefaultsTA.Connection = New Devart.Data.Oracle.OracleConnection(ConnectionString)
            userDefaultsDT = userDefaultsTA.GetUserDefaults(userName, rsUserDefaults)
            userDefaultList = New Generic.List(Of UserDefaultValues)
            For Each rowItem As UserDefaultsRow In userDefaultsDT.Rows
                If rowItem.IsPROFILETYPENAMENull = False And rowItem.IsPROFILETYPEVALUENull = False Then
                    If rowItem.APPLICATION.Equals(application.ToString, StringComparison.InvariantCultureIgnoreCase) OrElse rowItem.APPLICATION.Equals(Applications.All.ToString, StringComparison.InvariantCultureIgnoreCase) Then
                        userDefaultList.Add(New UserDefaultValues(rowItem.USERNAME, rowItem.PROFILETYPESEQID, rowItem.APPLICATION, rowItem.PROFILETYPENAME, rowItem.PROFILETYPEVALUE))
                    End If
                End If
            Next           
        Catch
            Throw
        Finally
            userDefaultsTA = Nothing
            userDefaultsDT = Nothing
        End Try
        Return userDefaultList
    End Function
End Class