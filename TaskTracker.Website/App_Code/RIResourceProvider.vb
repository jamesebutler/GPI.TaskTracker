Imports System.Globalization
Imports Devart.Data.Oracle
Imports IP.MEASFramework.Interfaces
Imports IP.MEASFramework.ResourceProvider

Public Class RIResourceProvider
    Implements IP.MEASFramework.Interfaces.IResourceDataProvider

    Private LocaleList As New StringCollection

    Private resourceCollections As New ResourceDataDictionary
    Public ReadOnly Property Name As String Implements IResourceDataProvider.Name
        Get
            Return "RIResourceProvider"
        End Get
    End Property

    Public ReadOnly Property Version As String Implements IResourceDataProvider.Version
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Description As String Implements IResourceDataProvider.Description
        Get
            Return "1.0"
        End Get
    End Property


    Public Function RetrieveApplicationLocaleList(ByVal applicationId As String, ByVal connectionString As String) As StringCollection Implements IResourceDataProvider.RetrieveApplicationLocaleList
        If (applicationId.Length <= 0) Then
            Return Nothing
        End If
        Return Me.LocaleList
    End Function

    Public Function RetrieveResourceData(ByVal locale As String, ByVal applicationId As String, ByVal connectionString As String) As ResourceDataDictionary Implements IResourceDataProvider.RetrieveResourceData
        Dim data As OracleDataReader = Nothing
        Try
            Try
                data = RIResourceProvider.GetData(connectionString, locale, applicationId)
                While data.Read()
                    Dim resourceDataItem As IP.MEASFramework.ResourceProvider.ResourceDataItem = New IP.MEASFramework.ResourceProvider.ResourceDataItem With
                        {
                            .ResourceKey = RIResourceProvider.DataClean(data.GetValue(data.GetOrdinal("ResourceKey"))).ToUpper(CultureInfo.CurrentCulture),
                            .ResourceType = RIResourceProvider.DataClean(data.GetValue(data.GetOrdinal("ResourceType"))).ToUpper(CultureInfo.CurrentCulture),
                            .ResourceValue = RIResourceProvider.DataClean(data.GetValue(data.GetOrdinal("ResourceValue")), "Global")
                        }
                    Try
                        If (Not Me.resourceCollections.Contains(resourceDataItem.ResourceKey)) Then
                            Me.resourceCollections.Add(resourceDataItem.ResourceKey, resourceDataItem.ResourceType, resourceDataItem.ResourceValue)
                        End If
                    Catch exception As System.Exception
                        Throw
                    End Try
                End While
                data.NextResult()
                While data.Read()
                    Dim upper As String = RIResourceProvider.DataClean(data.GetValue(data.GetOrdinal("Name"))).ToUpper(CultureInfo.CurrentCulture)
                    Me.LocaleList.Add(upper)
                End While
            Catch exception1 As System.Exception
                Throw
            End Try
        Finally
            If (data IsNot Nothing) Then
                data.Close()
            End If
        End Try
        Return Me.resourceCollections
    End Function

    ''' <summary>
    ''' This method is used to remove undesirable characters from the data that has been provided
    ''' </summary>
    ''' <param name="inputValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function DataClean(ByVal inputValue As Object) As String
        Dim str As String = Nothing
        Dim empty As String = String.Empty
        Try
            Try
                If inputValue Is Nothing Then Return String.Empty
                empty = inputValue.ToString()
                If (empty.ToString().Length <= 0) Then
                    empty = String.Empty
                Else
                    empty = empty.Replace("|", ":")
                    empty = empty.Replace("""", "'")
                End If
            Catch
                Throw
            End Try
        Finally
            str = empty.Trim()
        End Try
        Return str
    End Function

    Private Shared Function DataClean(ByVal inputValue As Object, ByVal defaultvalue As String) As String
        Dim str As String = RIResourceProvider.DataClean(inputValue)
        If (str.Length = 0) Then
            str = defaultvalue
        End If
        Return str.Trim()
    End Function

    Private Shared Function GetData(ByVal connectionString As String, ByVal localeName As String, ByVal application As String) As Devart.Data.Oracle.OracleDataReader
        Dim oracleDataReader As Devart.Data.Oracle.OracleDataReader = Nothing
        Dim oraclePackageDR As Devart.Data.Oracle.OracleDataReader = Nothing
        Dim oracleCommand As OracleCommand = New OracleCommand()
        Dim oracleParameter As OracleParameter = New OracleParameter()

        Try
            If (localeName.Length = 0) Then
                Throw New ArgumentException("Missing Argument [Locale Name]")
            End If
            If application.Length = 0 Then application = "RI"
            If connectionString.Length = 0 Then connectionString = ""

            Dim paramCollection As OracleParameterCollection = oracleCommand.Parameters
            Dim param As New OracleParameter

            param = New OracleParameter
            param.ParameterName = "IN_CULTURECODE"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = localeName
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "IN_APPLICATION"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = application
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "ref_cursor"
            param.OracleDbType = OracleDbType.Cursor
            param.Direction = Data.ParameterDirection.Output
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "ref_locales"
            param.OracleDbType = OracleDbType.Cursor
            param.Direction = Data.ParameterDirection.Output
            paramCollection.Add(param)

            oracleDataReader = RIResourceProvider.GetOraclePackageDR(paramCollection, "RELADMIN.LOCALIZATION.GETLOCALESTRINGS", connectionString)

        Catch
            Throw
        End Try
        Return oracleDataReader
    End Function
    Private Shared Function GetOraclePackageDR(ByRef parms As OracleParameterCollection, ByVal packageName As String, ByVal connectionString As String) As OracleDataReader
        Dim conCust As OracleConnection = Nothing
        Dim cmdSQL As OracleCommand = Nothing
        Dim connection As String = connectionString
        Dim dr As OracleDataReader = Nothing
        Dim cnConnection As OracleConnection = Nothing
        Dim returnParamName As String = String.Empty
        Dim returnValue As String = String.Empty
        Dim returnParms As New StringCollection
        Try

            If connection.Length = 0 Then
                connection = ConfigurationManager.ConnectionStrings.Item("connectionRCFATST").ConnectionString
            End If

            cmdSQL = New OracleCommand

            With cmdSQL
                cnConnection = New OracleConnection(connection)
                cnConnection.Open()
                .Connection = cnConnection
                .CommandText = packageName
                .CommandType = CommandType.StoredProcedure
                .PassParametersByName = True
                Dim sb As New StringBuilder
                For i As Integer = 0 To parms.Count - 1
                    If parms.Item(i).Value Is Nothing Then parms.Item(i).Value = DBNull.Value
                    Dim parm As New OracleParameter
                    parm.Direction = parms.Item(i).Direction
                    parm.DbType = parms.Item(i).DbType
                    parm.OracleDbType = parms.Item(i).OracleDbType
                    parm.Size = parms.Item(i).Size
                    If parms.Item(i).Direction = ParameterDirection.Input Or parms.Item(i).Direction = ParameterDirection.InputOutput Then
                        If parms.Item(i).Value IsNot Nothing Then
                            parm.Value = parms.Item(i).Value
                            If parm.Value.ToString = "" Then
                                parm.IsNullable = True
                                parm.Value = System.DBNull.Value
                            End If
                        Else
                            If parm.OracleDbType = OracleDbType.NVarChar Then
                                'parm.Value = DBNull.Value
                                'parm.Size = 2
                            End If
                        End If
                    ElseIf parms.Item(i).Direction = ParameterDirection.Output Then
                        returnParms.Add(parms.Item(i).ParameterName)
                        returnParamName = parms.Item(i).ParameterName
                    End If
                    parm.ParameterName = parms.Item(i).ParameterName
                    .Parameters.Add(parm)
                    If sb.Length > 0 Then sb.Append(",")
                    If parm.OracleDbType = OracleDbType.VarChar Then
                        If parm.Value IsNot Nothing Then
                            sb.Append(parm.ParameterName & "= '" & parm.Value.ToString & "' Type=" & parm.OracleDbType.ToString)
                        Else
                            sb.Append(parm.ParameterName & "= '" & "Null" & "' Type=" & parm.OracleDbType.ToString)
                        End If
                    Else
                        If parm.Value IsNot Nothing Then
                            sb.Append(parm.ParameterName & "= '" & parm.Value.ToString & "' Type=" & parm.OracleDbType.ToString)
                        Else
                            sb.Append(parm.ParameterName)
                        End If
                    End If
                    sb.AppendLine()

                Next

            End With

            dr = cmdSQL.ExecuteReader(Data.CommandBehavior.CloseConnection)
            'cmdSQL.ExecuteNonQuery()

            'Populate the original parms collection with the data from the output parameters
            For i As Integer = 0 To returnParms.Count - 1
                parms.Item(cmdSQL.Parameters(returnParms.Item(i)).ToString).Value = cmdSQL.Parameters(returnParms.Item(i)).Value.ToString
            Next
            '// return the return value if there is one
            If returnParamName.Length > 0 Then
                returnValue = cmdSQL.Parameters(returnParamName).Value.ToString
            Else
                returnValue = CStr(0)
            End If

        Catch ex As Exception
            If returnValue.Length = 0 Then returnValue = "Error Occurred"
            Throw New DataException("GetOraclePackageDR", ex)
            If Not conCust Is Nothing Then conCust = Nothing

        Finally
            GetOraclePackageDR = dr
            If Not dr Is Nothing Then dr = Nothing
            If Not cmdSQL Is Nothing Then cmdSQL = Nothing

        End Try
    End Function

End Class
