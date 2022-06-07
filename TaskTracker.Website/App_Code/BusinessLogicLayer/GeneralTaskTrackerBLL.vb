'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : mjpope
' Created          : 04-25-2011
'
' Last Modified By : mjpope
' Last Modified On : 06-27-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports HelperDal
Imports IP.Bids.SharedFunctions
Imports Devart.Data.Oracle

'TODO: Add more comments

''' <summary>
''' Contains Shared Methods ...
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class GeneralTaskTrackerBll
    ''' <summary>
    ''' Gets the name of the service.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetServiceName() As String
        Dim adapter As GeneralTaskTrackerDALTableAdapters.ServiceNameTableAdapter
        Dim table As GeneralTaskTrackerDAL.ServiceNameDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim retValue As String = String.Empty
        Dim cacheKey As String = "MTTServiceName"
        Dim cacheHours As Double = 8

        Try
            retValue = CType(GetDataFromCache(cacheKey, cacheHours), String)
            If retValue Is Nothing OrElse retValue.Length > 0 Then
                adapter = New GeneralTaskTrackerDALTableAdapters.ServiceNameTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetServiceName(emptyCursor)
                If table IsNot Nothing AndAlso table.Rows.Count > 0 Then
                    For Each rowItem As GeneralTaskTrackerDAL.ServiceNameRow In table.Rows
                        If rowItem.IsSERVICE_NAMENull Then
                            rowItem.SERVICE_NAME = "Missing"
                        Else
                            InsertDataIntoCache(cacheKey, cacheHours, rowItem.SERVICE_NAME)
                        End If
                        retValue = rowItem.SERVICE_NAME
                    Next
                End If
            End If
        Catch ex As Exception
            retValue = "Error"
            IP.Bids.SharedFunctions.HandleError("GetServiceName", "Error getting the Service Name from the database", ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return retValue
    End Function


    ''' <summary>
    ''' Gets the User Roles for the specified user
    ''' </summary>
    ''' <param name="userName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserRoles(ByVal userName As String) As System.Collections.Generic.List(Of UserRoles)
        Dim adapter As GeneralTaskTrackerDALTableAdapters.UserRolesTableAdapter
        Dim itemDataTable As GeneralTaskTrackerDAL.UserRolesDataTable = Nothing
        Dim emptyRecord As Object = Nothing
        Dim cacheKey As String = "MTT_UserRoles_" & userName
        Dim cacheHours As Double = 0
        Dim userRoleList As System.Collections.Generic.List(Of UserRoles) = Nothing

        Try
            userRoleList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of UserRoles))
            If userRoleList Is Nothing OrElse userRoleList.Count = 0 Then
                adapter = New GeneralTaskTrackerDALTableAdapters.UserRolesTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                itemDataTable = adapter.GetUserRoles(userName, emptyRecord)
                userRoleList = New System.Collections.Generic.List(Of UserRoles)
                For Each rowItem As GeneralTaskTrackerDAL.UserRolesRow In itemDataTable.Rows

                    If rowItem.IsROLENAMENull Then
                        rowItem.ROLENAME = String.Empty
                    End If
                    If rowItem.IsROLEDESCRIPTIONNull Then
                        rowItem.ROLEDESCRIPTION = String.Empty
                    End If
                    If rowItem.IsUSERNAMENull Then
                        rowItem.USERNAME = userName
                    End If
                    userRoleList.Add(New UserRoles(rowItem.PLANTCODE, rowItem.ROLEDESCRIPTION, rowItem.NAME, rowItem.USERNAME, rowItem.ROLESEQID, rowItem.ROLENAME))
                Next
                InsertDataIntoCache(cacheKey, cacheHours, userRoleList)
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetUserRoles", "Error getting user role data for [" & userName & "]", ex)
        Finally
            adapter = Nothing
            itemDataTable = Nothing
        End Try
        Return userRoleList
    End Function

    ''' <summary>
    ''' Gets a list of help files
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetHelpFiles() As System.Collections.Generic.List(Of HelpFiles)
        Dim adapter As GeneralTaskTrackerDALTableAdapters.DemoListTableAdapter
        Dim itemDataTable As GeneralTaskTrackerDAL.DemoListDataTable = Nothing
        Dim emptyRecord As Object = Nothing
        Dim cacheKey As String = "MTT_HelpFiles"
        Dim cacheHours As Double = 8
        Dim helpFileList As System.Collections.Generic.List(Of HelpFiles) = Nothing

        Try
            helpFileList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of HelpFiles))
            If helpFileList Is Nothing OrElse helpFileList.Count = 0 Then
                adapter = New GeneralTaskTrackerDALTableAdapters.DemoListTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                itemDataTable = adapter.GetDemoList(emptyRecord)
                helpFileList = New System.Collections.Generic.List(Of HelpFiles)
                For Each rowItem As GeneralTaskTrackerDAL.DemoListRow In itemDataTable.Rows

                    If rowItem.IsDEMODESCNull Then
                        rowItem.DEMODESC = String.Empty
                    End If
                    If rowItem.IsDEMOFILENAMENull Then
                        rowItem.DEMOFILENAME = String.Empty
                    End If
                    If rowItem.IsDEMONAMENull Then
                        rowItem.DEMONAME = "Missing"
                    End If
                    helpFileList.Add(New HelpFiles(IP.Bids.SharedFunctions.LocalizeValue(rowItem.DEMONAME, True), IP.Bids.SharedFunctions.LocalizeValue(rowItem.DEMODESC, True), rowItem.DEMOFILENAME))
                Next
                InsertDataIntoCache(cacheKey, cacheHours, helpFileList)
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetHelpFiles", "Error attempting to read the Help File list", ex)
        Finally
            adapter = Nothing
            itemDataTable = Nothing
        End Try
        Return helpFileList 'divisionDT
    End Function

    Public Shared Function GetUserDataByPrefix(ByVal prefix As String) As System.Collections.Generic.List(Of EmployeeProfile)
        Dim userRoleList As System.Collections.Generic.List(Of EmployeeProfile) = Nothing
        Dim paramCollection As New OracleParameterCollection
        Dim param As New OracleParameter
        Dim ds As System.Data.DataSet = Nothing
        Dim dr As Data.DataTableReader = Nothing

        Try
            'Set Procedure Parameters
            param = New OracleParameter
            param.ParameterName = "in_nameprefix"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = prefix
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "rsUser"
            param.OracleDbType = OracleDbType.Cursor
            param.Direction = Data.ParameterDirection.Output
            paramCollection.Add(param)

            ds = HelperDal.GetDSFromPackage(paramCollection, "Reladmin.MTTGENERALDATA.GetUserDataByPrefix")

            If ds IsNot Nothing Then
                If ds.Tables.Count = 1 Then
                    dr = ds.Tables(0).CreateDataReader
                End If
            End If

            If dr IsNot Nothing Then
                If dr.HasRows Then
                    userRoleList = New System.Collections.Generic.List(Of EmployeeProfile)
                    Dim employeeRecord As EmployeeProfile
                    Do While dr.Read
                        employeeRecord = New EmployeeProfile(DataClean(dr.Item("USERNAME")).ToUpper, DataClean(dr.Item("FIRSTNAME")).ToUpper, DataClean(dr.Item("LASTNAME")).ToUpper, DataClean(dr.Item("SITENAME")).ToUpper, DataClean(dr.Item("PLANTCODE")), DataClean(dr.Item("DOMAIN")))
                        userRoleList.Add(employeeRecord)
                    Loop
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetUserRoles", "Error getting user role data for [" & prefix & "]", ex)
        Finally
            ds = Nothing
        End Try
        Return userRoleList
    End Function

    Public Shared Function GetTaskItem(ByVal in_TaskITem As String) As DataSet
        Dim userRoleList As System.Collections.Generic.List(Of EmployeeProfile) = Nothing
        Dim paramCollection As New OracleParameterCollection
        Dim param As New OracleParameter
        Dim ds As System.Data.DataSet = Nothing
        Dim dr As Data.DataTableReader = Nothing

        Try
            'Set Procedure Parameters
            param = New OracleParameter
            param.ParameterName = "in_TaskITem"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_TaskITem
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "rsTaskItem"
            param.OracleDbType = OracleDbType.Cursor
            param.Direction = Data.ParameterDirection.Output
            paramCollection.Add(param)

            ds = HelperDal.GetDSFromPackage(paramCollection, "mtttaskitem.GetTaskItem")

            'If ds IsNot Nothing Then
            '    If ds.Tables.Count = 1 Then
            '        dr = ds.Tables(0).CreateDataReader
            '    End If
            'End If

            'If dr IsNot Nothing Then
            '    If dr.HasRows Then
            '        userRoleList = New System.Collections.Generic.List(Of EmployeeProfile)
            '        Dim employeeRecord As EmployeeProfile
            '        Do While dr.Read
            '            employeeRecord = New EmployeeProfile(DataClean(dr.Item("USERNAME")).ToUpper, DataClean(dr.Item("FIRSTNAME")).ToUpper, DataClean(dr.Item("LASTNAME")).ToUpper, DataClean(dr.Item("SITENAME")).ToUpper, DataClean(dr.Item("PLANTCODE")), DataClean(dr.Item("DOMAIN")))
            '            userRoleList.Add(employeeRecord)
            '        Loop
            '    End If
            'End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetTaskItem", "Error getting task item for [" & in_TaskITem & "]", ex)
        Finally
            GetTaskItem = ds
            ds = Nothing
        End Try
        Return GetTaskItem
    End Function


    Public Shared Function GetUserNotiftyProfile(ByVal in_UserName As String,
                                                 ByVal in_ProfileValuetype As String,
                                                 ByVal in_EmailType As String,
                                                 ByVal in_ROLESEQID As Integer) As String
        Dim paramCollection As New OracleParameterCollection
        Dim param As New OracleParameter
        Dim ds As System.Data.DataSet = Nothing
        Dim dr As Data.DataTableReader = Nothing
        Dim outUserName As String = Nothing

        Try
            'Set Procedure Parameters
            param = New OracleParameter
            param.ParameterName = "in_UserName"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_UserName
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "in_ProfileValuetype"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_ProfileValuetype
            paramCollection.Add(param)


            param = New OracleParameter
            param.ParameterName = "in_EmailType"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_EmailType
            paramCollection.Add(param)


            param = New OracleParameter
            param.ParameterName = "in_ROLESEQID"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_ROLESEQID
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "sys_refcursor"
            param.OracleDbType = OracleDbType.Cursor
            param.Direction = Data.ParameterDirection.Output
            paramCollection.Add(param)

            ds = HelperDal.GetDSFromPackage(paramCollection, "SP_Get_UserNotiftyProfile")

            If ds IsNot Nothing Then
                If ds.Tables.Count = 1 Then
                    dr = ds.Tables(0).CreateDataReader
                End If
            End If

            If dr IsNot Nothing Then
                If dr.HasRows Then
                    'userRoleList = New System.Collections.Generic.List(Of EmployeeProfile)
                    'Dim employeeRecord As EmployeeProfile
                    Do While dr.Read
                        outUserName = DataClean(dr.Item("USERNAME")).ToUpper

                    Loop
                End If
            End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetUserNotiftyProfile", "Error getting task item for [" & in_UserName & "]", ex)
        Finally
            GetUserNotiftyProfile = outUserName
            ds = Nothing
        End Try
        Return GetUserNotiftyProfile
    End Function





    Public Shared Function DoesUserHaveUpdateAccess(ByVal userName As String, ByVal plantCode As String) As Boolean
        Dim adapter As New IP.TaskTrackerDAL.TanksTableAdapters.UserAccessTableAdapter
        Dim updateOrReadOnly As Object = Nothing

        adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
        adapter.GetUserAccessBySite(userName, plantCode, updateOrReadOnly)
        If IsDBNull(updateOrReadOnly) = False AndAlso updateOrReadOnly.ToString.ToLower = "update" Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
