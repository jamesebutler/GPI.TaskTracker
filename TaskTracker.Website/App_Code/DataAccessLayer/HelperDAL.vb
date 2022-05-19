'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : mjpope
' Created          : 10-24-2010
'
' Last Modified By : mjpope
' Last Modified On : 06-22-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports System.Data
Imports Devart.Data.Oracle

Public NotInheritable Class HelperDal
    Public Shared Function GetDSFromPackage(ByRef parms As OracleParameterCollection, ByVal packageName As String, Optional ByVal cacheKey As String = "", Optional ByVal cacheHours As Integer = 0) As DataSet
        Dim conCust As OracleConnection = Nothing
        Dim cmdSQL As OracleCommand = Nothing
        Dim connection As String = String.Empty
        Dim provider As String = String.Empty
        Dim ds As DataSet = Nothing
        Dim daData As OracleDataAdapter = Nothing
        Dim cnConnection As OracleConnection = Nothing

        Try

            'Dim cachedData As System.Web.Caching.Cache = HttpRuntime.Cache 'HttpContext.Current.Cache
            'If cachedData IsNot Nothing Then
            If cacheKey.Length > 0 And cacheHours > 0 Then
                If HttpRuntime.Cache.Item(cacheKey) IsNot Nothing Then
                    ds = CType(HttpRuntime.Cache.Item(cacheKey), DataSet)
                    Exit Try
                End If
            End If
            'End If

            If connection.Length = 0 Then
                connection = ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString
            End If
            If provider.Length = 0 Then
                provider = ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ProviderName
            End If
            cmdSQL = New OracleCommand

            With cmdSQL
                cnConnection = New OracleConnection(connection)
                cnConnection.Open()
                .Connection = cnConnection
                .CommandText = packageName

                .CommandType = CommandType.StoredProcedure
                Dim sb As New StringBuilder
                For i As Integer = 0 To parms.Count - 1
                    If parms.Item(i).Value Is Nothing Then
                        parms.Item(i).Value = DBNull.Value
                    End If
                    Dim parm As New OracleParameter
                    parm.Direction = parms.Item(i).Direction
                    parm.DbType = parms.Item(i).DbType
                    parm.OracleDbType = parms.Item(i).OracleDbType
                    parm.Size = parms.Item(i).Size
                    If parms.Item(i).Direction = ParameterDirection.Input Or parms.Item(i).Direction = ParameterDirection.InputOutput Then
                        If parms.Item(i).Value IsNot Nothing Then
                            parm.Value = parms.Item(i).Value
                            'If (Not parm.Value.ToString Is Nothing AndAlso String.IsNullOrEmpty(parm.Value.ToString)) Then
                            '    'parm.Value = DBNull.Value
                            'End If

                            'Else
                            '    If parm.OracledbType = OracledbType.NVarChar Then
                            '        'parm.Value = DBNull.Value
                            '        'parm.Size = 2
                            '    End If
                        End If
                    End If
                    parm.ParameterName = parms.Item(i).ParameterName
                    .Parameters.Add(parm)
                    If sb.Length > 0 Then
                        sb.Append(",")
                    End If
                    If parm.OracleDbType = OracleDbType.VarChar Then
                        If parm.Value IsNot Nothing Then
                            sb.Append("'" & parm.Value.ToString & "'")
                        Else
                            sb.Append("Null")
                        End If
                    Else
                        sb.Append(parm.Value)
                    End If
                Next

            End With

            ds = New DataSet()
            ds.EnforceConstraints = False
            daData = New OracleDataAdapter(cmdSQL)
            daData.Fill(ds)
            ds.EnforceConstraints = True
            If cacheHours > 0 And cacheKey.Length > 0 Then
                If ds IsNot Nothing Then
                    HttpRuntime.Cache.Insert(cacheKey, ds, Nothing, DateTime.Now.AddHours(cacheHours), TimeSpan.Zero)
                End If
            End If
        Catch ex As Exception
            ds = Nothing
            Throw New DataException("GetDSFromPackage - " & packageName, ex)
            If Not conCust Is Nothing Then
                conCust = Nothing
            End If
        Finally
            GetDSFromPackage = ds
            If Not daData Is Nothing Then
                daData = Nothing
            End If
            If Not ds Is Nothing Then
                ds = Nothing
            End If
            If Not cmdSQL Is Nothing Then
                cmdSQL = Nothing
            End If
            If cnConnection IsNot Nothing Then
                If cnConnection.State = ConnectionState.Open Then
                    cnConnection.Close()
                End If
                cnConnection = Nothing
            End If
        End Try
    End Function
    Public Shared Function CallDROraclePackage(ByRef parms As OracleParameterCollection, ByVal packageName As String) As String 'OracleDataReader
        Dim conCust As OracleConnection = Nothing
        Dim cmdSQL As OracleCommand = Nothing
        Dim connection As String = String.Empty
        Dim provider As String = String.Empty
        Dim dr As OracleDataReader = Nothing
        Dim cnConnection As OracleConnection = Nothing
        Dim returnParamName As String = String.Empty
        Dim returnValue As String = String.Empty
        Dim returnParms As New StringCollection
        Try

            If connection.Length = 0 Then
                connection = ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString
            End If
            If provider.Length = 0 Then
                provider = ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ProviderName
            End If
            cmdSQL = New OracleCommand

            With cmdSQL
                cnConnection = New OracleConnection(connection)
                cnConnection.Open()
                .Connection = cnConnection
                .CommandText = packageName
                .CommandType = CommandType.StoredProcedure
                Dim sb As New StringBuilder
                For i As Integer = 0 To parms.Count - 1
                    If parms.Item(i).Value Is Nothing Then
                        parms.Item(i).Value = DBNull.Value
                    End If
                    Dim parm As New OracleParameter
                    parm.Direction = parms.Item(i).Direction
                    parm.DbType = parms.Item(i).DbType
                    parm.OracleDbType = parms.Item(i).OracleDbType
                    parm.Size = parms.Item(i).Size
                    If parms.Item(i).Direction = ParameterDirection.Input Or parms.Item(i).Direction = ParameterDirection.InputOutput Then
                        If parms.Item(i).Value IsNot Nothing Then
                            parm.Value = parms.Item(i).Value
                            If (Not parm.Value.ToString Is Nothing AndAlso String.IsNullOrEmpty(parm.Value.ToString)) Then
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
                    If sb.Length > 0 Then
                        sb.Append(",")
                    End If
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

            'dr = cmdSQL.ExecuteReader
            cmdSQL.ExecuteNonQuery()

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
            If returnValue.Length = 0 Then
                returnValue = "Error Occurred"
            End If
            Throw New DataException("CallDROraclePackage", ex)
            If Not conCust Is Nothing Then
                conCust = Nothing
            End If

        Finally
            CallDROraclePackage = returnValue
            If Not dr Is Nothing Then
                dr = Nothing
            End If
            If Not cmdSQL Is Nothing Then
                cmdSQL = Nothing
            End If
            If cnConnection IsNot Nothing Then
                If cnConnection.State = ConnectionState.Open Then
                    cnConnection.Close()
                End If
                cnConnection = Nothing
            End If
        End Try
    End Function
    Public Shared Function GetDatabaseName() As String
        Dim paramCollection As New OracleParameterCollection
        Dim param As New OracleParameter
        '        Dim ds As System.Data.DataSet = Nothing ' COMMENTED BY CODEIT.RIGHT
        Dim ret As String = String.Empty
        Try

            param = New OracleParameter
            param.ParameterName = "rsServiceName"
            param.OracleDbType = OracleDbType.Cursor
            param.Direction = Data.ParameterDirection.Output
            paramCollection.Add(param)

            Dim dr As OracleDataReader = HelperDal.GetOraclePackageDR(paramCollection, "Reladmin.RI.GETSERVICENAME")

            If dr IsNot Nothing Then
                Do While dr.Read
                    If dr.Item("SERVICE_NAME") IsNot Nothing Then
                        ret = CStr(dr.Item("SERVICE_NAME"))
                    End If
                Loop
            End If
        Catch ex As Exception
            Throw
        Finally
            GetDatabaseName = ret
        End Try
    End Function
    Public Shared Function GetOraclePackageDR(ByRef parms As OracleParameterCollection, ByVal packageName As String) As OracleDataReader
        Dim conCust As OracleConnection = Nothing
        Dim cmdSQL As OracleCommand = Nothing
        Dim connection As String = String.Empty
        Dim provider As String = String.Empty
        Dim dr As OracleDataReader = Nothing
        Dim cnConnection As OracleConnection = Nothing
        Dim returnParamName As String = String.Empty
        Dim returnValue As String = String.Empty
        Dim returnParms As New StringCollection
        Try

            If connection.Length = 0 Then
                connection = ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString
            End If
            If provider.Length = 0 Then
                provider = ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ProviderName
            End If
            cmdSQL = New OracleCommand

            With cmdSQL
                cnConnection = New OracleConnection(connection)
                cnConnection.Open()
                .Connection = cnConnection
                .CommandText = packageName
                .CommandType = CommandType.StoredProcedure
                Dim sb As New StringBuilder
                For i As Integer = 0 To parms.Count - 1
                    If parms.Item(i).Value Is Nothing Then
                        parms.Item(i).Value = DBNull.Value
                    End If
                    Dim parm As New OracleParameter
                    parm.Direction = parms.Item(i).Direction
                    parm.DbType = parms.Item(i).DbType
                    parm.OracleDbType = parms.Item(i).OracleDbType
                    parm.Size = parms.Item(i).Size
                    If parms.Item(i).Direction = ParameterDirection.Input Or parms.Item(i).Direction = ParameterDirection.InputOutput Then
                        If parms.Item(i).Value IsNot Nothing Then
                            parm.Value = parms.Item(i).Value
                            If (Not parm.Value.ToString Is Nothing AndAlso String.IsNullOrEmpty(parm.Value.ToString)) Then
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
                    If sb.Length > 0 Then
                        sb.Append(",")
                    End If
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
            If returnValue.Length = 0 Then
                returnValue = "Error Occurred"
            End If
            Throw New DataException("GetOraclePackageDR", ex)
            If Not conCust Is Nothing Then
                conCust = Nothing
            End If

        Finally
            GetOraclePackageDR = dr
            If Not dr Is Nothing Then
                dr = Nothing
            End If
            If Not cmdSQL Is Nothing Then
                cmdSQL = Nothing
            End If
            'If cnConnection IsNot Nothing Then
            '    If cnConnection.State = ConnectionState.Open Then cnConnection.Close()
            '    cnConnection = Nothing
            'End If
        End Try
    End Function
    Public Shared Function GetDataFromCache(ByVal cacheKey As String, ByVal cacheHours As Double) As Object
        Dim cachedObject As Object = Nothing
        If cacheKey.Length > 0 And cacheHours > 0 Then
            If HttpRuntime.Cache.Item(cacheKey) IsNot Nothing Then
                cachedObject = HttpRuntime.Cache.Item(cacheKey)
            End If
        End If
        Return cachedObject
    End Function

    Public Shared Sub InsertDataIntoCache(ByVal cacheKey As String, ByVal cacheHours As Double, ByVal dataToCache As Object)
        'Exit Sub 'Turn Cache Off
        If cacheHours > 0 And cacheKey.Length > 0 Then
            HttpRuntime.Cache.Insert(cacheKey, dataToCache, Nothing, GetNextHour, TimeSpan.Zero)
            'HttpRuntime.Cache.Insert(cacheKey, dataToCache, Nothing, DateTime.Now.AddHours(cacheHours), TimeSpan.Zero)
        End If
    End Sub

    Public Shared Sub DeleteFromCache(ByVal cacheKey As String)
        If cacheKey.Length > 0 Then
            HttpRuntime.Cache.Remove(cacheKey)
        End If
    End Sub

    Public Shared Function GetNextHour() As DateTime
        Dim newTime = DateTime.Now.AddHours(1)
        Dim nextHour = New DateTime(newTime.Year, newTime.Month, newTime.Day, newTime.Hour, 0, 0, 0)
        Return nextHour
    End Function
End Class
