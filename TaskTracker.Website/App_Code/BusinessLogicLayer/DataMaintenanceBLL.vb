Imports HelperDal
Imports IP.TaskTrackerDAL
Imports Devart.Data.Oracle

Public Class DataMaintenanceBLL




    ''' <summary>
    ''' Gets a list of employees for the specified Plant Code
    ''' </summary>
    ''' <param name="inPlantCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetEmployeeList(ByVal inPlantCode As String) As System.Collections.Generic.List(Of Employee)
        Dim adapter As DataMaintTableAdapters.EmployeeListTableAdapter 'DataMaintenanceDALTableAdapters.EmployeeTableAdapter
        Dim table As DataMaint.EmployeeListDataTable = Nothing 'DataMaintenanceDAL.EmployeeDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim cacheKey As String = "MTTEmployeeList_" & inPlantCode
        Dim cacheHours As Double = 0
        Dim employeelist As System.Collections.Generic.List(Of Employee) = Nothing

        Try
            employeelist = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of Employee))
            If employeelist Is Nothing OrElse employeelist.Count > 0 Then
                employeelist = New System.Collections.Generic.List(Of Employee)
                adapter = New DataMaintTableAdapters.EmployeeListTableAdapter ' New DataMaintenanceDALTableAdapters.EmployeeTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetEmployeeList(inPlantCode, emptyCursor)
                If table IsNot Nothing AndAlso table.Rows.Count > 0 Then
                    For Each rowItem As DataMaint.EmployeeListRow In table.Rows
                        If rowItem.IsFIRSTNAMENull Then
                            rowItem.FIRSTNAME = ""
                        End If
                        If rowItem.IsLASTNAMENull Then
                            rowItem.LASTNAME = ""
                        End If
                        If rowItem.IsMIDDLEINITNull Then
                            rowItem.MIDDLEINIT = ""
                        End If
                        If rowItem.IsEMAILNull Then
                            rowItem.EMAIL = ""
                        End If
                        If rowItem.IsEXTENSIONNull Then
                            rowItem.EXTENSION = ""
                        End If
                        'If rowItem.IsDOMAINNul Then
                        '    rowItem.DOMAIN = ""
                        'End If
                        If rowItem.IsDEFAULT_LANGUAGENull Then
                            rowItem.DEFAULT_LANGUAGE = ""
                        End If
                        If rowItem.IsINACTIVE_FLAGNull Then
                            rowItem.INACTIVE_FLAG = ""
                        End If
                        If rowItem.IsTASKCOUNTNull Then
                            rowItem.TASKCOUNT = ""
                        End If
                        'If rowItem.FIRSTNAME.Contains("'") OrElse rowItem.LASTNAME.Contains("'") OrElse rowItem.EMAIL.Contains("'") OrElse rowItem.USERNAME.Contains("'") Then
                        employeelist.Add(New IP.MEAS.BO.Employee(rowItem.USERNAME, rowItem.FIRSTNAME, rowItem.LASTNAME, rowItem.MIDDLEINIT, rowItem.EMAIL, rowItem.EXTENSION, rowItem.DOMAIN, rowItem.DEFAULT_LANGUAGE, rowItem.INACTIVE_FLAG, rowItem.TASKCOUNT))
                        'End If
                    Next
                    InsertDataIntoCache(cacheKey, cacheHours, employeelist)
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetEmployeeList", "Error getting the Employee List from the database", ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return employeelist
    End Function

    ''' <summary>
    ''' Gets the Site Role List for the specified Plant Code
    ''' </summary>
    ''' <param name="inPlantCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetSiteUserRoleList(ByVal inPlantCode As String) As System.Collections.Generic.List(Of SiteUserRole)
        Dim adapter As DataMaintenanceDALTableAdapters.SiteUserRoleListTableAdapter
        Dim table As DataMaintenanceDAL.SiteUserRoleListDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim cacheKey As String = "MTTRoleList_" & inPlantCode
        Dim cacheHours As Double = 0
        Dim rolelist As System.Collections.Generic.List(Of SiteUserRole) = Nothing

        Try
            rolelist = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of SiteUserRole))
            If rolelist Is Nothing OrElse rolelist.Count > 0 Then
                rolelist = New System.Collections.Generic.List(Of SiteUserRole)
                adapter = New DataMaintenanceDALTableAdapters.SiteUserRoleListTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetSiteRoles(inPlantCode, emptyCursor)
                If table IsNot Nothing AndAlso table.Rows.Count > 0 Then
                    For Each rowItem As DataMaintenanceDAL.SiteUserRoleListRow In table.Rows
                        If rowItem.IsNAMENull Then
                            rowItem.NAME = ""
                        End If
                        If rowItem.IsROLENAMENull Then
                            rowItem.ROLENAME = ""
                        End If
                        If rowItem.IsUSERNAMENull Then
                            rowItem.USERNAME = ""
                        End If
                        If rowItem.IsROLEDESCRIPTIONNull Then
                            rowItem.ROLEDESCRIPTION = ""
                        End If

                        rolelist.Add(New IP.MEAS.BO.SiteUserRole(rowItem.PLANTCODE, rowItem.ROLEDESCRIPTION, rowItem.NAME, rowItem.USERNAME, rowItem.ROLESEQID, rowItem.ROLENAME, rowItem.BUSUNIT, rowItem.AREA, rowItem.LINE))
                    Next
                    InsertDataIntoCache(cacheKey, cacheHours, rolelist)
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetRoleList", "Error getting the Role List from the database", ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return rolelist
    End Function
    ''' <summary>
    ''' Gets the Role List by Site for the specified Role
    ''' </summary>
    ''' <param name="inRoleSeqId"></param>
    ''' <param name="inBusType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetRolesbySiteList(ByVal inRoleSeqId As String, ByVal inBusType As String, ByVal inDivision As String) As System.Collections.Generic.List(Of RoleBySite)
        Dim adapter As DataMaintTableAdapters.GETROLESBYSITETableAdapter
        Dim table As DataMaint.GETROLESBYSITEDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim cacheKey As String = "MTTRolebySiteList_" & inRoleSeqId
        Dim cacheHours As Double = 0
        Dim rolelist As System.Collections.Generic.List(Of RoleBySite) = Nothing

        Try
            rolelist = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of RoleBySite))
            If rolelist Is Nothing OrElse rolelist.Count > 0 Then
                rolelist = New System.Collections.Generic.List(Of RoleBySite)
                adapter = New DataMaintTableAdapters.GETROLESBYSITETableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetRolesbySite(inRoleSeqId, inBusType, inDivision, emptyCursor)
                If table IsNot Nothing AndAlso table.Rows.Count > 0 Then
                    For Each rowItem As DataMaint.GETROLESBYSITERow In table.Rows
                        If rowItem.IsNAMENull Then
                            rowItem.NAME = ""
                        End If
                        If rowItem.IsROLENAMENull Then
                            rowItem.ROLENAME = ""
                        End If
                        If rowItem.IsUSERNAMENull Then
                            rowItem.USERNAME = ""
                        End If
                        If rowItem.IsROLEDESCRIPTIONNull Then
                            rowItem.ROLEDESCRIPTION = ""
                        End If

                        rolelist.Add(New IP.MEAS.BO.RoleBySite(rowItem.SITENAME, rowItem.PLANTCODE, rowItem.ROLEDESCRIPTION, rowItem.NAME, rowItem.USERNAME, rowItem.ROLESEQID, rowItem.ROLENAME, rowItem.BUSUNIT, rowItem.AREA, rowItem.LINE))
                    Next
                    InsertDataIntoCache(cacheKey, cacheHours, rolelist)
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetRoleBySiteList", "Error getting the Role List from the database", ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return rolelist
    End Function
    ''' <summary>
    ''' Gets the Division List by Site for the specified Role
    ''' </summary>
    ''' <param name="inBusType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDivisionListByBusType(ByVal inBusType As String) As System.Collections.Generic.List(Of Division)
        Dim adapter As DataMaintTableAdapters.GETDIVISIONLISTBYBUSTYPETableAdapter
        Dim table As DataMaint.GETDIVISIONLISTBYBUSTYPEDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim cacheKey As String = "MTTDivisionList_" & inBusType
        Dim cacheHours As Double = 0
        Dim divisionlist As System.Collections.Generic.List(Of Division) = Nothing

        Try
            divisionlist = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of Division))
            If divisionlist Is Nothing OrElse divisionlist.Count > 0 Then
                divisionlist = New System.Collections.Generic.List(Of Division)
                adapter = New DataMaintTableAdapters.GETDIVISIONLISTBYBUSTYPETableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetDivisionListByBusType(inBusType, emptyCursor)
                If table IsNot Nothing AndAlso table.Rows.Count > 0 Then
                    For Each rowItem As DataMaint.GETDIVISIONLISTBYBUSTYPERow In table.Rows
                        'If rowItem.IsNAMENull Then
                        '    rowItem.NAME = ""
                        'End If
                        'If rowItem.IsROLENAMENull Then
                        '    rowItem.ROLENAME = ""
                        'End If
                        'If rowItem.IsUSERNAMENull Then
                        '    rowItem.USERNAME = ""
                        'End If
                        'If rowItem.IsROLEDESCRIPTIONNull Then
                        '    rowItem.ROLEDESCRIPTION = ""
                        'End If

                        divisionlist.Add(New IP.MEAS.BO.Division(rowItem.DIVISION))
                    Next
                    InsertDataIntoCache(cacheKey, cacheHours, divisionlist)
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetDivisionListByBusType", "Error getting the Division List from the database", ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return divisionlist
    End Function
    ''' <summary>
    ''' Gets the Reassign Task List for the specified User
    ''' </summary>
    ''' <param name="username"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetReassignTaskList(ByVal username As String, ByVal PlantCode As String, ByVal BusUnit As String, ByVal Area As String, ByVal Line As String, ByVal Type As String, ByVal Activity As String) As System.Collections.Generic.List(Of ReassignTask)
        Dim adapter As DataMaintTableAdapters.GETREASSIGNTASKLISTTableAdapter
        Dim table As DataMaint.GETREASSIGNTASKLISTDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim cacheKey As String = "MTTEReassignTaskList_" & username
        Dim cacheHours As Double = 0
        Dim reassigntasklist As System.Collections.Generic.List(Of ReassignTask) = Nothing

        Try
            reassigntasklist = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of ReassignTask))
            If reassigntasklist Is Nothing OrElse reassigntasklist.Count > 0 Then
                reassigntasklist = New System.Collections.Generic.List(Of ReassignTask)
                adapter = New DataMaintTableAdapters.GETREASSIGNTASKLISTTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetReassignTaskList(username, PlantCode, BusUnit, Area, Line, Type, Activity, emptyCursor)
                If table IsNot Nothing AndAlso table.Rows.Count > 0 Then
                    For Each rowItem As DataMaint.GETREASSIGNTASKLISTRow In table.Rows
                        If rowItem.IsTASKDESCRIPTIONNull Then
                            rowItem.TASKDESCRIPTION = ""
                        End If
                        If rowItem.IsTASKTITLENull Then
                            rowItem.TASKTITLE = ""
                        End If
                        If rowItem.IsHEADERTITLENull Then
                            rowItem.HEADERTITLE = ""
                        End If
                        If rowItem.IsDUEDATENull Then
                            rowItem.DUEDATE = ""
                        End If
                        If rowItem.IsRESPONSIBLEUSERNAMENull Then
                            rowItem.RESPONSIBLEUSERNAME = ""
                        End If
                        If rowItem.IsSITENAMENull Then
                            rowItem.SITENAME = ""
                        End If
                        If rowItem.IsPLANTCODENull Then
                            rowItem.PLANTCODE = ""
                        End If

                        If rowItem.IsTASKHEADERSEQIDNull Then
                            rowItem.TASKHEADERSEQID = ""
                        End If

                        reassigntasklist.Add(New IP.MEAS.BO.ReassignTask(rowItem.SITENAME, rowItem.PLANTCODE, rowItem.HEADERTITLE, rowItem.TASKTITLE, rowItem.TASKDESCRIPTION, rowItem.TASKITEMSEQID, rowItem.TASKHEADERSEQID, rowItem.DUEDATE, rowItem.RESPONSIBLEUSERNAME))
                    Next
                    InsertDataIntoCache(cacheKey, cacheHours, reassigntasklist)
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetReassignTaskList", "Error getting the Reassign Task List from the database", ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return reassigntasklist
    End Function
    ''' <summary>
    ''' Gets the Bus Type List for the specified Role
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAllBusTypes() As System.Collections.Generic.List(Of BusType)
        Dim adapter As DataMaintTableAdapters.GETBUSTYPELISTTableAdapter
        Dim table As DataMaint.GETBUSTYPELISTDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim cacheKey As String = "MTTBusTypeList"
        Dim cacheHours As Double = 0
        Dim bustypelist As System.Collections.Generic.List(Of BusType) = Nothing

        Try
            bustypelist = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of BusType))
            If bustypelist Is Nothing OrElse bustypelist.Count > 0 Then
                bustypelist = New System.Collections.Generic.List(Of BusType)
                adapter = New DataMaintTableAdapters.GETBUSTYPELISTTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetBusTypeList(emptyCursor)
                If table IsNot Nothing AndAlso table.Rows.Count > 0 Then
                    For Each rowItem As DataMaint.GETBUSTYPELISTRow In table.Rows
                        'If rowItem.IsNull Then
                        '    rowItem.NAME = ""
                        'End If
                        'If rowItem.IsROLENAMENull Then
                        '    rowItem.ROLENAME = ""
                        'End If
                        'If rowItem.IsUSERNAMENull Then
                        '    rowItem.USERNAME = ""
                        'End If
                        'If rowItem.IsROLEDESCRIPTIONNull Then
                        '    rowItem.ROLEDESCRIPTION = ""
                        'End If

                        bustypelist.Add(New IP.MEAS.BO.BusType(rowItem.BUSTYPE, rowItem.BUSNAME))
                    Next
                    InsertDataIntoCache(cacheKey, cacheHours, bustypelist)
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetBusTypeList", "Error getting the Business Type List from the database", ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return bustypelist
    End Function
    ''' <summary>
    ''' Gets a list of all Roles
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAllRoles() As System.Collections.Generic.List(Of Role)
        Dim adapter As DataMaintenanceDALTableAdapters.RoleListTableAdapter
        Dim table As DataMaintenanceDAL.RoleListDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim cacheKey As String = "MTTRoleList_" '& inPlantCode
        Dim cacheHours As Double = 0
        Dim allrolelist As System.Collections.Generic.List(Of Role) = Nothing

        Try
            allrolelist = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of Role))
            If allrolelist Is Nothing OrElse allrolelist.Count > 0 Then
                allrolelist = New System.Collections.Generic.List(Of Role)
                adapter = New DataMaintenanceDALTableAdapters.RoleListTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetRoleList(emptyCursor)
                If table IsNot Nothing AndAlso table.Rows.Count > 0 Then
                    For Each rowItem As DataMaintenanceDAL.RoleListRow In table.Rows
                        If rowItem.IsROLENAMENull Then
                            rowItem.ROLENAME = ""
                        End If
                        If rowItem.IsROLEDESCRIPTIONNull Then
                            rowItem.ROLEDESCRIPTION = ""
                        End If
                        If rowItem.IsROLETYPENull Then
                            rowItem.ROLETYPE = ""
                        End If
                        If rowItem.IsDISPLAYINRESPDROPDOWNNull Then
                            rowItem.DISPLAYINRESPDROPDOWN = ""
                        End If
                        allrolelist.Add(New IP.MEAS.BO.Role(rowItem.ROLESEQID, rowItem.ROLENAME, rowItem.ROLEDESCRIPTION, rowItem.ROLETYPE, rowItem.DISPLAYINRESPDROPDOWN))
                    Next
                    InsertDataIntoCache(cacheKey, cacheHours, allrolelist)
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetAllRoles", "Error getting the Role List from the database", ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return allrolelist
    End Function

    ''' <summary>
    ''' Deletes the Role Assignment for the specified User Role Key
    ''' </summary>
    ''' <param name="userrolekey"></param>
    ''' <remarks></remarks>
    Public Sub DeleteRoleAssignment(ByVal userrolekey As String, Optional ByVal delimiter As Char = ",")
        Dim adapter As DataMaintenanceDALTableAdapters.SiteUserRoleListTableAdapter
        Dim table As DataMaintenanceDAL.SiteUserRoleListDataTable = Nothing
        Dim out_status As Decimal
        Dim txtplantcode As String = ""
        Dim txtroleseqid As String = ""
        Dim txtusername As String = ""
        Dim txtbusunit As String = ""
        Dim txtarea As String = ""
        Dim txtline As String = ""
        Dim txtuserrolekey As String() = userrolekey.Split(delimiter)

        If txtuserrolekey.Length = 6 Then
            txtplantcode = txtuserrolekey(0)
            txtroleseqid = CInt(txtuserrolekey(1))
            txtusername = txtuserrolekey(2)
            txtbusunit = txtuserrolekey(3)
            txtarea = txtuserrolekey(4)
            txtline = txtuserrolekey(5)
        End If
        Try
            adapter = New DataMaintenanceDALTableAdapters.SiteUserRoleListTableAdapter
            adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            adapter.DELETEUSERROLES(txtusername, txtroleseqid, txtplantcode, txtbusunit, txtarea, txtline, IP.Bids.SharedFunctions.GetCurrentUser.Username, CType(out_status, Global.System.Nullable(Of Decimal)))
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("DeleteRoleAssignment", "Error attempting to delete. SQL code= " & out_status, ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
    End Sub
End Class
