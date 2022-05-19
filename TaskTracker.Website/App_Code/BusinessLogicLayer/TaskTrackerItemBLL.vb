'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : mjpope
' Created          : 11-17-2010
'
' Last Modified By : mjpope
' Last Modified On : 06-27-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Option Explicit On
Option Strict On
Imports IP.Bids.SharedFunctions
Imports HelperDal
Imports IP.TaskTrackerDAL
Imports Devart.Data.Oracle




Public Class TaskTrackerItemBll

    Public Sub DeleteTaskItem(ByVal taskNumber As Integer, ByVal rootTaskNumber As String, ByVal userName As String)
        Dim adapter As TaskDetailTableAdapters.TaskItemTableAdapter
        Dim outStatus As Decimal?
        Try
            adapter = New TaskDetailTableAdapters.TaskItemTableAdapter
            adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            adapter.DeleteTask(CType(taskNumber, String), userName, rootTaskNumber, outStatus)
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("DeleteTaskItem", "Error while deleting Task Item [" & taskNumber & "]", ex)
        Finally
            adapter = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Saves the Task Item
    ''' </summary>
    ''' <param name="item"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveTaskItem(ByVal item As IP.MEAS.BO.TaskItem) As System.Collections.Generic.List(Of IP.MEAS.BO.TaskItem)
        Dim adapter As TaskDetailTableAdapters.TaskItemTableAdapter
        Dim table As TaskDetail.TaskItemDataTable = Nothing
        Dim record As System.Collections.Generic.List(Of IP.MEAS.BO.TaskItem) = Nothing
        Dim out_status As Decimal
        Dim out_taskitem As Decimal
        Try


            adapter = New TaskDetailTableAdapters.TaskItemTableAdapter
            adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            If item Is Nothing Then
                Return Nothing
                Exit Function
            End If
            With item
                Dim taskID As String = CStr(.TaskItemSeqId)
                If taskID = "-1" Then
                    taskID = String.Empty
                End If
                HelperDal.DeleteFromCache("GetRootTaskItemList_" & item.TaskHeaderSeqId)
                adapter.UpdateTaskItem(taskID, CStr(.TaskHeaderSeqId), .DueDate, .ClosedDate, .Title, .LastUpdateUserName.ToUpper, .Description, .Priority, .DateCritical, CStr(.StatusSeqId), CStr(.LeadTime), .ResponsibleUserName.ToUpper, CStr(.ResponsibleRoleSeqId), .ResponsibleRolePlantCode, .UpdateFlag, .Comments, CStr(.DaysBefore), CStr(.DaysAfter), .WorkOrder, CStr(.SortOrder), CStr(.EstimatedCost), CStr(.ActualCost), .TankInspectionId, CStr(.OriginalRoleSeqId), CType(out_taskitem, Global.System.Nullable(Of Decimal)), CType(out_status, Global.System.Nullable(Of Decimal)))
                If taskID.Length = 0 Then
                    table = adapter.GetLastCreatedTaskItem(.LastUpdateUserName, Nothing)
                Else
                    table = adapter.GetTaskItem(taskID, Nothing)
                End If
            End With
            If table IsNot Nothing And table.Rows.Count > 0 Then
                record = New System.Collections.Generic.List(Of TaskItem)
                For Each rowItem As TaskDetail.TaskItemRow In table.Rows
                    With rowItem
                        If .IsCLOSEDDATENull Then
                            .CLOSEDDATE = String.Empty
                        Else
                            .CLOSEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CLOSEDDATE).ToShortDateString
                        End If
                        If .IsCREATEDBYNull Then
                            .CREATEDBY = String.Empty
                        End If
                        If .IsCREATEDDATENull Then
                            .CREATEDDATE = Now.ToShortDateString 'String.Empty
                        Else
                            .CREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CREATEDDATE).ToShortDateString
                        End If
                        If .IsDATECRITICALNull Then
                            .DATECRITICAL = "N"
                        End If
                        If .IsDESCRIPTIONNull Then
                            .DESCRIPTION = String.Empty
                        End If
                        If .IsDUEDATENull Then
                            .DUEDATE = String.Empty
                        Else
                            .DUEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.DUEDATE).ToShortDateString
                        End If
                        If .IsLASTUPDATEDATENull Then
                            .LASTUPDATEDATE = Now.ToShortDateString
                        Else
                            .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
                        End If
                        If .IsLASTUPDATEUSERNAMENull OrElse .LASTUPDATEUSERNAME.Trim.Length = 0 Then
                            .LASTUPDATEUSERNAME = IP.Bids.SharedFunctions.GetCurrentUser.Username
                        End If
                        If .IsLEADTIMENull Then
                            .LEADTIME = CStr(0)
                        End If
                        If .IsPRIORITYNull Then
                            .PRIORITY = String.Empty
                        End If
                        If .IsRESPONSIBLENAMENull Then
                            .RESPONSIBLEUSERNAME = String.Empty
                        End If
                        If .IsRESPONSIBLEROLESEQIDNull Then
                            .RESPONSIBLEROLESEQID = CStr(-1)
                        End If
                        If .IsRESPONSIBLEUSERNAMENull Then
                            .RESPONSIBLEUSERNAME = String.Empty
                        End If
                        If .IsROLENAMENull Then
                            .ROLENAME = String.Empty
                        End If
                        If .IsROOTTASKITEMSEQIDNull Then
                            .ROOTTASKITEMSEQID = -1
                        End If
                        If .IsSTATUSNAMENull Then
                            .STATUSNAME = String.Empty
                        End If
                        If .IsSTATUSSEQIDNull Then
                            .STATUSSEQID = CStr(-1)
                        End If
                        If .IsTASKHEADERSEQIDNull Then
                            Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
                        End If
                        If .IsTITLENull Then
                            .TITLE = String.Empty
                        End If
                        If .IsROLEDESCRIPTIONNull Then
                            .ROLEDESCRIPTION = String.Empty
                        End If
                        If .IsRESPROLEPLANTCODENull Then
                            .RESPROLEPLANTCODE = String.Empty
                        End If
                        If .IsRESPROLESITENAMENull Then
                            .RESPROLESITENAME = String.Empty
                        End If
                        If .IsDAYSBEFORENull Then
                            .DAYSBEFORE = CStr(-1)
                        End If
                        If .IsDEPENDENTTASKSEQIDNull Then
                            .DEPENDENTTASKSEQID = String.Empty
                        End If
                        If .IsDEPENDENTCHILDIDNull Then
                            .DEPENDENTCHILDID = 0 'String.Empty
                        End If
                        If .IsDAYSAFTERNull Then
                            .DAYSAFTER = CStr(-1)
                        End If
                        If .IsWORKORDERNull Then
                            .WORKORDER = String.Empty
                        End If
                        If .IsCREATEDBYUSERNAMENull Then
                            .CREATEDBYUSERNAME = String.Empty
                        End If
                        If .IsDUEDATENull = False Then
                            record.Add(New IP.MEAS.BO.TaskItem(.CLOSEDDATE, .CREATEDBY, .CREATEDDATE, .DATECRITICAL, .DESCRIPTION, .DUEDATE, .LASTUPDATEDATE, .LASTUPDATEUSERNAME, CInt(.LEADTIME), .PRIORITY, .RESPONSIBLENAME, CInt(.RESPONSIBLEROLESEQID), .RESPONSIBLEUSERNAME, .ROLENAME, CStr(.ROOTTASKITEMSEQID), CInt(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TITLE, .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME, CInt(.DAYSBEFORE), CStr(.DEPENDENTTASKSEQID), CStr(.DEPENDENTCHILDID), CInt(.DAYSAFTER), .WORKORDER, CStr(.SORTORDER), CStr(.ESTIMATEDCOST), CStr(.ACTUALCOST), CStr(.TANKINSPECTIONTYPEID), CStr(.ORIGINALROLESEQID), .CREATEDBYUSERNAME))
                        End If
                    End With
                Next
            End If
        Catch ex As Exception
            If item IsNot Nothing Then
                IP.Bids.SharedFunctions.HandleError("SaveTaskItem", "Error while saving Task Item [" & item.TaskItemSeqId & "]", ex)
            Else
                IP.Bids.SharedFunctions.HandleError("SaveTaskItem", "TaskItem Record is null", ex)
            End If
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
    End Function

    ''' <summary>
    ''' Saves the Dependent Task Item
    ''' </summary>
    ''' <param name="item"></param>
    ''' <param name="parentSeqID"></param>
    ''' <param name="parentDueDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveDependedentTaskItem(ByVal item As IP.MEAS.BO.TaskItem, ByVal parentSeqID As String, ByVal parentDueDate As String) As System.Collections.Generic.List(Of IP.MEAS.BO.TaskItem)
        Dim adapter As TaskDetailTableAdapters.TaskItemTableAdapter
        Dim table As TaskDetail.TaskItemDataTable = Nothing
        Dim record As System.Collections.Generic.List(Of IP.MEAS.BO.TaskItem) = Nothing
        Dim out_status As Decimal
        Dim out_taskitem As Decimal

        Try

            adapter = New TaskDetailTableAdapters.TaskItemTableAdapter
            adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            If item Is Nothing Then
                Return Nothing
                Exit Function
            End If
            With item
                Dim taskID As String = CStr(.TaskItemSeqId)
                If taskID = "-1" Then
                    taskID = String.Empty
                End If
                'HelperDal.DeleteFromCache("GetRootTaskItemList_" & item.Ta skHeaderSeqId)
                Dim retval As Object = adapter.UpdateDependentTask(taskID, CStr(.TaskHeaderSeqId), parentSeqID, parentDueDate, .Title, .LastUpdateUserName.ToUpper, .Description, .ResponsibleUserName.ToUpper, CStr(.ResponsibleRoleSeqId), .ResponsibleRolePlantCode, .DueDate, .ClosedDate, CStr(.StatusSeqId), .Priority, .DateCritical, CStr(.DaysBefore), .RootTaskItemSeqId, .UpdateFlag, .DependentChildSeqid, .Comments, CStr(.LeadTime), .WorkOrder, CStr(.SortOrder), CStr(.EstimatedCost), CStr(.ActualCost), .TankInspectionId, CStr(.OriginalRoleSeqId), CType(out_taskitem, Global.System.Nullable(Of Decimal)), CType(out_status, Global.System.Nullable(Of Decimal)))
                If taskID.Length = 0 Then
                    table = adapter.GetLastCreatedTaskItem(.LastUpdateUserName, Nothing)
                Else
                    table = adapter.GetTaskItem(taskID, Nothing)
                End If
            End With
            If table IsNot Nothing And table.Rows.Count > 0 Then
                record = New System.Collections.Generic.List(Of TaskItem)
                For Each rowItem As TaskDetail.TaskItemRow In table.Rows
                    With rowItem
                        If .IsCLOSEDDATENull Then
                            .CLOSEDDATE = String.Empty
                        Else
                            .CLOSEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CLOSEDDATE).ToShortDateString
                        End If
                        If .IsCREATEDBYNull Then
                            .CREATEDBY = String.Empty
                        End If
                        If .IsCREATEDDATENull Then
                            .CREATEDDATE = Now.ToShortDateString 'String.Empty
                        Else
                            .CREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CREATEDDATE).ToShortDateString
                        End If
                        If .IsDATECRITICALNull Then
                            .DATECRITICAL = "N"
                        End If
                        If .IsDESCRIPTIONNull Then
                            .DESCRIPTION = String.Empty
                        End If
                        'If .IsDUEDATENull Then
                        '    .DUEDATE = CDate(String.Empty)
                        'End If
                        If .IsLASTUPDATEDATENull Then
                            .LASTUPDATEDATE = .CREATEDDATE ' String.Empty
                        Else
                            .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
                        End If
                        If .IsLASTUPDATEUSERNAMENull Then
                            .LASTUPDATEUSERNAME = String.Empty
                        End If
                        If .IsLEADTIMENull Then
                            .LEADTIME = CStr(0)
                        End If
                        If .IsPRIORITYNull Then
                            .PRIORITY = String.Empty
                        End If
                        If .IsRESPONSIBLENAMENull Then
                            .RESPONSIBLEUSERNAME = String.Empty
                        End If
                        If .IsRESPONSIBLEROLESEQIDNull Then
                            .RESPONSIBLEROLESEQID = CStr(-1)
                        End If
                        If .IsRESPONSIBLEUSERNAMENull Then
                            .RESPONSIBLEUSERNAME = String.Empty
                        End If
                        If .IsROLENAMENull Then
                            .ROLENAME = String.Empty
                        End If
                        If .IsROOTTASKITEMSEQIDNull Then
                            .ROOTTASKITEMSEQID = -1
                        End If
                        If .IsSTATUSNAMENull Then
                            .STATUSNAME = String.Empty
                        End If
                        If .IsSTATUSSEQIDNull Then
                            .STATUSSEQID = CStr(-1)
                        End If
                        If .IsTASKHEADERSEQIDNull Then
                            Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
                        End If
                        If .IsTITLENull Then
                            .TITLE = String.Empty
                        End If
                        If .IsROLEDESCRIPTIONNull Then
                            .ROLEDESCRIPTION = String.Empty
                        End If
                        If .IsRESPROLEPLANTCODENull Then
                            .RESPROLEPLANTCODE = String.Empty
                        End If
                        If .IsRESPROLESITENAMENull Then
                            .RESPROLESITENAME = String.Empty
                        End If
                        If .IsDAYSBEFORENull Then
                            .DAYSBEFORE = CStr(-1)
                        End If
                        If .IsDEPENDENTTASKSEQIDNull Then
                            .DEPENDENTTASKSEQID = String.Empty
                        End If
                        If .IsDEPENDENTCHILDIDNull Then
                            .DEPENDENTCHILDID = 0
                        End If
                        If .IsDAYSAFTERNull Then
                            .DAYSAFTER = CStr(-1)
                        End If
                        If .IsWORKORDERNull Then
                            .WORKORDER = String.Empty
                        End If
                        If .IsCREATEDBYUSERNAMENull Then
                            .CREATEDBYUSERNAME = String.Empty
                        End If
                        If .IsDUEDATENull = False Then
                            record.Add(New IP.MEAS.BO.TaskItem(.CLOSEDDATE, .CREATEDBY, .CREATEDDATE, .DATECRITICAL, .DESCRIPTION, .DUEDATE, .LASTUPDATEDATE, .LASTUPDATEUSERNAME, CInt(.LEADTIME), .PRIORITY, .RESPONSIBLENAME, CInt(.RESPONSIBLEROLESEQID), .RESPONSIBLEUSERNAME, .ROLENAME, CStr(.ROOTTASKITEMSEQID), CInt(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TITLE, .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME, CInt(.DAYSBEFORE), CStr(.DEPENDENTTASKSEQID), CStr(.DEPENDENTCHILDID), CInt(.DAYSAFTER), .WORKORDER, .SORTORDER, .ESTIMATEDCOST, .ACTUALCOST, .TANKINSPECTIONTYPEID, .ORIGINALROLESEQID, .CREATEDBYUSERNAME))
                        End If
                    End With
                Next
            End If
        Catch ex As Exception
            If item IsNot Nothing Then
                IP.Bids.SharedFunctions.HandleError("SaveTaskItem", "Error while saving Task Item [" & item.TaskItemSeqId & "]", ex)
            Else
                IP.Bids.SharedFunctions.HandleError("SaveTaskItem", "TaskItem Record is null", ex)
            End If
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
    End Function

    ''' <summary>
    ''' Saves the Sub Task Item
    ''' </summary>
    ''' <param name="item"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveSubTaskItem(ByVal item As IP.MEAS.BO.SubTaskItem) As System.Collections.Generic.List(Of IP.MEAS.BO.SubTaskItem)
        Dim adapter As TaskDetailTableAdapters.SubTaskTableAdapter
        Dim table As TaskDetail.SubTaskDataTable = Nothing
        Dim record As System.Collections.Generic.List(Of IP.MEAS.BO.SubTaskItem) = Nothing
        Dim out_status As Decimal
        Dim out_taskitem As Decimal

        Try
            adapter = New TaskDetailTableAdapters.SubTaskTableAdapter
            adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            With item
                table = adapter.UpdateSubTask(CStr(.TaskItemSeqId), CStr(.TaskHeaderSeqId), CStr(.ParentSubTaskSeqID), .Title, .LastUpdateUserName, .Description, .DaysAfter.ToString, .ResponsibleUserName, CStr(.ResponsibleRoleSeqId), .ResponsibleRolePlantCode, CStr(.SortOrder), CStr(.EstimatedCost), CStr(.ActualCost), .TankInspectionId, CStr(.OriginalRoleSeqId), CType(out_taskitem, Global.System.Nullable(Of Decimal)), CType(out_status, Global.System.Nullable(Of Decimal)))
            End With
            record = GetSubTaskItemList(CInt(item.ParentSubTaskSeqID), True)
            'If table IsNot Nothing And table.Rows.Count > 0 Then
            '    For Each rowItem As TaskDetail.SubTaskRow In table.Rows
            '        With rowItem
            '            If .IsCREATEDBYNull Then
            '                .CREATEDBY = String.Empty
            '            End If
            '            If .IsCREATEDDATENull Then
            '                .CREATEDDATE = Now.ToShortDateString 'String.Empty
            '            Else
            '                .CREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CREATEDDATE).ToShortDateString
            '            End If

            '            If .IsDESCRIPTIONNull Then
            '                .DESCRIPTION = String.Empty
            '            End If

            '            If .IsLASTUPDATEDATENull Then
            '                .LASTUPDATEDATE = .CREATEDDATE ' String.Empty
            '            Else
            '                .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
            '            End If
            '            If .IsLASTUPDATEUSERNAMENull Then
            '                .LASTUPDATEUSERNAME = String.Empty
            '            End If

            '            If .IsRESPONSIBLENAMENull Then
            '                .RESPONSIBLEUSERNAME = String.Empty
            '            End If
            '            If .IsRESPONSIBLEROLESEQIDNull Then
            '                .RESPONSIBLEROLESEQID = -1
            '            End If
            '            If .IsRESPONSIBLEUSERNAMENull Then
            '                .RESPONSIBLEUSERNAME = String.Empty
            '            End If
            '            If .IsROLENAMENull Then
            '                .ROLENAME = String.Empty
            '            End If
            '            If .IsROOTTASKITEMSEQIDNull Then
            '                .ROOTTASKITEMSEQID = -1
            '            End If

            '            If .IsTASKHEADERSEQIDNull Then
            '                Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
            '            End If
            '            If .IsTITLENull Then
            '                .TITLE = String.Empty
            '            End If

            '            If .IsDAYSAFTERNull Then
            '                .DAYSAFTER = -1
            '            End If

            '            If .IsPARENTSUBTASKSEQIDNull Then
            '                .PARENTSUBTASKSEQID = -1
            '            End If
            '            If .IsROLEDESCRIPTIONNull Then
            '                .ROLEDESCRIPTION = String.Empty
            '            End If
            '            If .IsRESPROLEPLANTCODENull Then
            '                .RESPROLEPLANTCODE = String.Empty
            '            End If
            '            If .IsRESPROLESITENAMENull Then
            '                .RESPROLESITENAME = String.Empty
            '            End If
            '            record.Add(New BO.SubTaskItem(.CREATEDBY.ToUpper, .CREATEDDATE, .DESCRIPTION, .LASTUPDATEDATE, .LASTUPDATEUSERNAME.ToUpper, .RESPONSIBLENAME, CInt(.RESPONSIBLEROLESEQID), .RESPONSIBLEUSERNAME.ToUpper, .ROLENAME, CStr(.ROOTTASKITEMSEQID), CStr(.TASKHEADERSEQID), CStr(.TASKITEMSEQID), .TITLE, CInt(.DAYSAFTER), CStr(.PARENTSUBTASKSEQID), .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME))
            '        End With
            '    Next
            'End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("SaveSubTaskItem", "Error while saving Subsequent Task", ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
    End Function

    ''' <summary>
    ''' Deletes the Sub Task Item
    ''' </summary>
    ''' <param name="taskItemNumber"></param>
    ''' <remarks></remarks>
    Public Sub DeleteSubTaskItem(ByVal taskItemNumber As String)
        Dim adapter As TaskDetailTableAdapters.SubTaskTableAdapter
        Dim table As TaskDetail.SubTaskDataTable = Nothing
        Dim out_status As Decimal

        Try
            adapter = New TaskDetailTableAdapters.SubTaskTableAdapter
            adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            adapter.DeleteSubTasks(taskItemNumber, CType(out_status, Global.System.Nullable(Of Decimal)))
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("DeleteSubTaskItem", "Error attempting to delete [" & taskItemNumber & "]", ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Deletes the Dependent Task Item
    ''' </summary>
    ''' <param name="dependentTaskItem"></param>
    ''' <param name="UpdateFlag"></param>
    ''' <remarks></remarks>
    Public Sub DeleteDependentTaskItem(ByVal dependentTaskItem As String, ByVal UpdateFlag As String)
        Dim adapter As TaskDetailTableAdapters.TaskItemTableAdapter
        Dim table As TaskDetail.TaskItemDataTable = Nothing
        Dim out_status As Decimal

        Try
            adapter = New TaskDetailTableAdapters.TaskItemTableAdapter
            adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            If IsNumeric(dependentTaskItem) Then
                Dim taskItem As IP.MEAS.BO.TaskItem = Me.GetTaskItem(CInt(dependentTaskItem))
                If taskItem IsNot Nothing Then
                    adapter.DeleteDependentTask(CStr(taskItem.TaskItemSeqId), UpdateFlag.ToUpper, taskItem.Dependenttaskseqid, IP.Bids.SharedFunctions.FormatDateTimeToEnglish(CDate(taskItem.DueDate)), taskItem.DependentChildSeqid, CType(out_status, Global.System.Nullable(Of Decimal)))
                Else
                    'TODO Throw exception
                End If
            Else
                'TODO Throw exception
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("DeleteSubTaskItem", "Error attempting to delete [" & dependentTaskItem & "]", ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Determines if Date Critical has been marked
    ''' </summary>
    ''' <param name="taskItemNumber"></param>
    ''' <param name="userName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsDateCriticalChangeAllowed(ByVal taskItemNumber As String, ByVal userName As String) As Boolean
        Dim adapter As TaskDetailTableAdapters.CriticalDateTableAdapter
        Dim table As TaskDetail.CriticalDateDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim retValue As Boolean

        Try
            adapter = New TaskDetailTableAdapters.CriticalDateTableAdapter
            adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            table = adapter.GetCriticalDateAccess(taskItemNumber, userName, emptyCursor)
            If table IsNot Nothing AndAlso table.Rows.Count > 0 Then
                retValue = True
            End If
        Catch ex As Exception
            retValue = False
            IP.Bids.SharedFunctions.HandleError("IsDateCriticalChangeAllowed", , ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return retValue
    End Function

    ''' <summary>
    ''' Gets the Priority List
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPriorityList() As System.Collections.Generic.List(Of IP.MEAS.BO.TaskPriorities)
        Dim record As System.Collections.Generic.List(Of IP.MEAS.BO.TaskPriorities) = Nothing
        Try
            record = New System.Collections.Generic.List(Of IP.MEAS.BO.TaskPriorities)
            record.Add(New IP.MEAS.BO.TaskPriorities(IP.MEAS.BO.TaskPriorities.TaskItemPriority.Low, IP.Bids.SharedFunctions.LocalizeValue("Low", True)))
            record.Add(New IP.MEAS.BO.TaskPriorities(IP.MEAS.BO.TaskPriorities.TaskItemPriority.Medium, IP.Bids.SharedFunctions.LocalizeValue("Medium", True)))
            record.Add(New IP.MEAS.BO.TaskPriorities(IP.MEAS.BO.TaskPriorities.TaskItemPriority.High, IP.Bids.SharedFunctions.LocalizeValue("High", True)))
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetPriorityList", , ex)
        End Try
        Return record
    End Function

    'Public Function GetTaskItemList(ByVal taskHeaderNumber As Integer) As System.Collections.Generic.List(Of BO.TaskItem)
    '    Dim adapter As TaskDetailTableAdapters.TaskItemTableAdapter
    '    Dim table As TaskDetail.TaskItemDataTable = Nothing
    '    Dim emptyCursor As Object = Nothing
    '    Dim record As System.Collections.Generic.List(Of BO.TaskItem) = Nothing
    '    Try
    '        If table Is Nothing OrElse table.Rows.Count = 0 Then
    '            adapter = New TaskDetailTableAdapters.TaskItemTableAdapter
    '            table = adapter.GetTaskItemList(CStr(taskHeaderNumber), emptyCursor)
    '            If table IsNot Nothing Then
    '                record = New System.Collections.Generic.List(Of BO.TaskItem)
    '                For Each rowItem As TaskDetail.TaskItemRow In table.Rows
    '                    With rowItem
    '                        If .IsCLOSEDDATENull Then
    '                            .CLOSEDDATE = String.Empty
    '                        End If
    '                        If .IsCREATEDBYNull Then
    '                            .CREATEDBY = String.Empty
    '                        End If
    '                        If .IsCREATEDDATENull Then
    '                            .CREATEDDATE = String.Empty
    '                        End If
    '                        If .IsDATECRITICALNull Then
    '                            .DATECRITICAL = "N"
    '                        End If
    '                        If .IsDESCRIPTIONNull Then
    '                            .DESCRIPTION = String.Empty
    '                        End If
    '                        'If .IsDUEDATENull Then
    '                        '    .DUEDATE = String.Empty
    '                        'End If
    '                        If .IsLASTUPDATEDATENull Then
    '                            .LASTUPDATEDATE = String.Empty
    '                        End If
    '                        If .IsLASTUPDATEUSERNAMENull Then
    '                            .LASTUPDATEUSERNAME = String.Empty
    '                        End If
    '                        If .IsLEADTIMENull Then
    '                            .LEADTIME = 0
    '                        End If
    '                        If .IsPRIORITYNull Then
    '                            .PRIORITY = String.Empty
    '                        End If
    '                        If .IsRESPONSIBLENAMENull Then
    '                            .RESPONSIBLEUSERNAME = String.Empty
    '                        End If
    '                        If .IsRESPONSIBLEROLESEQIDNull Then
    '                            .RESPONSIBLEROLESEQID = -1
    '                        End If
    '                        If .IsRESPONSIBLEUSERNAMENull Then
    '                            .RESPONSIBLEUSERNAME = String.Empty
    '                        End If
    '                        If .IsROLENAMENull Then
    '                            .ROLENAME = String.Empty
    '                        End If
    '                        If .IsROOTTASKITEMSEQIDNull Then
    '                            .ROOTTASKITEMSEQID = -1
    '                        End If
    '                        If .IsSTATUSNAMENull Then
    '                            .STATUSNAME = String.Empty
    '                        End If
    '                        If .IsSTATUSSEQIDNull Then
    '                            .STATUSSEQID = -1
    '                        End If
    '                        If .IsROLEDESCRIPTIONNull Then
    '                            .ROLEDESCRIPTION = String.Empty
    '                        End If
    '                        If .IsTASKHEADERSEQIDNull Then
    '                            Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
    '                        End If
    '                        If .IsTITLENull Then
    '                            .TITLE = String.Empty
    '                        End If
    '                        If .IsRESPROLESITENAMENull Then
    '                            .RESPROLESITENAME = String.Empty
    '                        End If
    '                        If .IsDUEDATENull = False Then
    '                            record.Add(New TaskItem(.CLOSEDDATE, .CREATEDBY, CDate(.CREATEDDATE).ToShortDateString, .DATECRITICAL, .DESCRIPTION, CStr(.DUEDATE), .LASTUPDATEDATE, .LASTUPDATEUSERNAME, .LEADTIME, .PRIORITY, .RESPONSIBLENAME, .RESPONSIBLEROLESEQID, .RESPONSIBLEUSERNAME, .ROLENAME, CStr(.ROOTTASKITEMSEQID), .STATUSSEQID, .TASKHEADERSEQID, .TASKITEMSEQID, .TITLE, .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME))
    '                        End If
    '                    End With
    '                Next
    '            End If
    '        End If
    '    Catch
    '        Throw
    '    Finally
    '        adapter = Nothing
    '        table = Nothing
    '    End Try
    '    Return record
    '    adapter = Nothing
    'End Function
    ''' <summary>
    ''' Gets the Root Task Item List
    ''' </summary>
    ''' <param name="taskHeaderNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRootTaskItemList(ByVal taskHeaderNumber As Integer) As System.Collections.Generic.List(Of TaskItem)
        Dim adapter As TaskDetailTableAdapters.TaskItemTableAdapter
        Dim recurringAdapter As TaskDetailTableAdapters.RecurringTasksTableAdapter
        Dim table As TaskDetail.TaskItemDataTable = Nothing
        Dim recurringTable As TaskDetail.RecurringTasksDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim record As System.Collections.Generic.List(Of TaskItem) = Nothing
        Dim recurringRecord As System.Collections.Generic.List(Of RecurringTasks) = Nothing

        Try
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.TaskItemTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                record = CType(GetDataFromCache("GetRootTaskItemList_" & taskHeaderNumber, 0.5), System.Collections.Generic.List(Of TaskItem))
                If record Is Nothing OrElse record.Count = 0 Then
                    table = adapter.GetTaskItemList(CStr(taskHeaderNumber), emptyCursor)
                    If table IsNot Nothing Then
                        record = New System.Collections.Generic.List(Of TaskItem)
                        For Each rowItem As TaskDetail.TaskItemRow In table.Rows
                            With rowItem
                                'recurringTable=adapter.get
                                If .IsDEPENDENTTASKSEQIDNull Then
                                    .DEPENDENTTASKSEQID = String.Empty
                                End If
                                If .IsDEPENDENTCHILDIDNull Then
                                    .DEPENDENTCHILDID = 0
                                End If
                                If (.IsROOTTASKITEMSEQIDNull OrElse .ROOTTASKITEMSEQID = CDec(.TASKITEMSEQID)) And (.DEPENDENTTASKSEQID.Length = 0 Or .DEPENDENTTASKSEQID = .TASKITEMSEQID.ToString) Then '= False AndAlso .ROOTTASKITEMSEQID = -1 Then
                                    recurringAdapter = New TaskDetailTableAdapters.RecurringTasksTableAdapter
                                    recurringAdapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                                    recurringTable = recurringAdapter.GetRecurringTasks(CStr(.TASKITEMSEQID), Nothing)
                                    If recurringTable IsNot Nothing AndAlso recurringTable.Rows.Count > 0 Then
                                        'Dim recurringDates As New StringBuilder
                                        recurringRecord = New System.Collections.Generic.List(Of RecurringTasks)
                                        For Each recurringItem As TaskDetail.RecurringTasksRow In recurringTable.Rows
                                            If recurringItem.IsDUEDATENull = False Then
                                                'If recurringDates.Length > 0 Then
                                                '    recurringDates.Append(",")
                                                'End If
                                                'recurringDates.Append(recurringItem.DUEDATE)
                                                recurringRecord.Add(New RecurringTasks(IP.Bids.SharedFunctions.CDateFromEnglishDate(recurringItem.DUEDATE).ToShortDateString, CInt(recurringItem.TASKITEMSEQID), CInt(recurringItem.ROOTTASKITEMSEQID), CInt(recurringItem.STATUSSEQID)))
                                            End If
                                        Next
                                    End If
                                    If .IsCLOSEDDATENull Then
                                        .CLOSEDDATE = String.Empty
                                    Else
                                        .CLOSEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CLOSEDDATE).ToShortDateString
                                    End If
                                    If .IsCREATEDBYNull Then
                                        .CREATEDBY = String.Empty
                                    End If
                                    If .IsCREATEDDATENull Then
                                        .CREATEDDATE = Now.ToShortDateString
                                    Else
                                        .CREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CREATEDDATE).ToShortDateString
                                    End If
                                    If .IsDATECRITICALNull Then
                                        .DATECRITICAL = "N"
                                    End If
                                    If .IsDESCRIPTIONNull Then
                                        .DESCRIPTION = String.Empty
                                    End If
                                    'If .IsDUEDATENull Then
                                    '    .DUEDATE = CDate(String.Empty)
                                    'End If
                                    If .IsLASTUPDATEDATENull Then
                                        .LASTUPDATEDATE = .CREATEDDATE 'String.Empty
                                    Else
                                        .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
                                    End If
                                    If .IsLASTUPDATEUSERNAMENull Then
                                        .LASTUPDATEUSERNAME = String.Empty
                                    End If
                                    If .IsLEADTIMENull Then
                                        .LEADTIME = CStr(0)
                                    End If
                                    If .IsPRIORITYNull Then
                                        .PRIORITY = String.Empty
                                    End If
                                    If .IsRESPONSIBLENAMENull Then
                                        .RESPONSIBLEUSERNAME = String.Empty
                                    End If
                                    If .IsRESPONSIBLEROLESEQIDNull Then
                                        .RESPONSIBLEROLESEQID = CStr(-1)
                                    End If
                                    If .IsRESPONSIBLEUSERNAMENull Then
                                        .RESPONSIBLEUSERNAME = String.Empty
                                    End If
                                    If .IsROLENAMENull Then
                                        .ROLENAME = String.Empty
                                    End If
                                    If .IsROOTTASKITEMSEQIDNull Then
                                        .ROOTTASKITEMSEQID = -1
                                    End If
                                    If .IsSTATUSNAMENull Then
                                        .STATUSNAME = String.Empty
                                    End If
                                    If .IsSTATUSSEQIDNull Then
                                        .STATUSSEQID = CStr(-1)
                                    End If
                                    If .IsROLEDESCRIPTIONNull Then
                                        .ROLEDESCRIPTION = String.Empty
                                    End If
                                    If .IsTASKHEADERSEQIDNull Then
                                        Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
                                    End If
                                    If .IsTITLENull Then
                                        .TITLE = String.Empty
                                    End If
                                    If .IsRESPROLEPLANTCODENull Then
                                        .RESPROLEPLANTCODE = String.Empty
                                    End If
                                    If .IsRESPROLESITENAMENull Then
                                        .RESPROLESITENAME = String.Empty
                                    End If
                                    If .IsDAYSBEFORENull Then
                                        .DAYSBEFORE = CStr(-1)
                                    End If
                                    If .IsDEPENDENTTASKSEQIDNull Then
                                        .DEPENDENTTASKSEQID = String.Empty
                                    End If
                                    If .IsDEPENDENTCHILDIDNull Then
                                        .DEPENDENTCHILDID = 0
                                    End If
                                    If .IsDAYSAFTERNull Then
                                        .DAYSAFTER = CStr(-1)
                                    End If
                                    If .IsWORKORDERNull Then
                                        .WORKORDER = String.Empty
                                    End If
                                    If .IsCREATEDBYUSERNAMENull Then
                                        .CREATEDBYUSERNAME = String.Empty
                                    End If
                                    'If .ROOTTASKITEMSEQID = -1 Then
                                    If .IsDUEDATENull = False And (.DEPENDENTTASKSEQID.Length = 0 Or .DEPENDENTTASKSEQID = .TASKITEMSEQID.ToString) Then
                                        record.Add(New TaskItem(.CLOSEDDATE, .CREATEDBY, .CREATEDDATE, .DATECRITICAL, .DESCRIPTION, .DUEDATE, .LASTUPDATEDATE, .LASTUPDATEUSERNAME, CInt(.LEADTIME), .PRIORITY, .RESPONSIBLENAME, CInt(.RESPONSIBLEROLESEQID), .RESPONSIBLEUSERNAME, .ROLENAME, CStr(.ROOTTASKITEMSEQID), CInt(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TITLE, .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME, recurringRecord, CInt(.DAYSBEFORE), CStr(.DEPENDENTTASKSEQID), CStr(.DEPENDENTCHILDID), CInt(.DAYSAFTER), .WORKORDER, CStr(.SORTORDER), CStr(.ESTIMATEDCOST), CStr(.ACTUALCOST), CStr(.TANKINSPECTIONTYPEID), CStr(.ORIGINALROLESEQID), .CREATEDBYUSERNAME))
                                        recurringRecord = Nothing
                                    End If
                                End If
                            End With
                        Next
                        'InsertDataIntoCache("GetRootTaskItemList_" & taskHeaderNumber, 0.5, record)
                    End If
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetRootTaskItemList", , ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
        adapter = Nothing
    End Function

    ''' <summary>
    ''' Gets the Task Item List
    ''' </summary>
    ''' <param name="taskHeaderNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTaskItemList(ByVal taskHeaderNumber As Integer) As System.Collections.Generic.List(Of TaskItem)
        Dim adapter As TaskDetailTableAdapters.TaskItemTableAdapter
        Dim recurringAdapter As TaskDetailTableAdapters.RecurringTasksTableAdapter
        Dim table As TaskDetail.TaskItemDataTable = Nothing
        Dim recurringTable As TaskDetail.RecurringTasksDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim record As System.Collections.Generic.List(Of TaskItem) = Nothing
        Dim recurringRecord As System.Collections.Generic.List(Of RecurringTasks) = Nothing
        Try
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.TaskItemTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                'record = CType(GetDataFromCache("GetTaskItemList_" & taskHeaderNumber, 0.5), System.Collections.Generic.List(Of TaskItem))
                If record Is Nothing OrElse record.Count = 0 Then
                    table = adapter.GetTaskItemList(CStr(taskHeaderNumber), emptyCursor)
                    If table IsNot Nothing Then
                        record = New System.Collections.Generic.List(Of TaskItem)
                        For Each rowItem As TaskDetail.TaskItemRow In table.Rows
                            With rowItem

                                'recurringTable=adapter.get
                                If .IsDEPENDENTTASKSEQIDNull Then
                                    .DEPENDENTTASKSEQID = String.Empty
                                End If
                                If .IsDEPENDENTCHILDIDNull Then
                                    .DEPENDENTCHILDID = 0
                                End If
                                'If (.IsROOTTASKITEMSEQIDNull OrElse .ROOTTASKITEMSEQID = .TASKITEMSEQID) And (.DEPENDENTTASKSEQID.Length = 0 Or .DEPENDENTTASKSEQID = .TASKITEMSEQID.ToString) Then '= False AndAlso .ROOTTASKITEMSEQID = -1 Then
                                recurringAdapter = New TaskDetailTableAdapters.RecurringTasksTableAdapter
                                recurringAdapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                                recurringTable = recurringAdapter.GetRecurringTasks(CStr(.TASKITEMSEQID), Nothing)
                                If recurringTable IsNot Nothing AndAlso recurringTable.Rows.Count > 0 Then
                                    'Dim recurringDates As New StringBuilder
                                    recurringRecord = New System.Collections.Generic.List(Of RecurringTasks)
                                    For Each recurringItem As TaskDetail.RecurringTasksRow In recurringTable.Rows
                                        If recurringItem.IsDUEDATENull = False Then
                                            'If recurringDates.Length > 0 Then
                                            '    recurringDates.Append(",")
                                            'End If
                                            'recurringDates.Append(recurringItem.DUEDATE)
                                            recurringRecord.Add(New RecurringTasks(IP.Bids.SharedFunctions.CDateFromEnglishDate(recurringItem.DUEDATE).ToShortDateString, CInt(recurringItem.TASKITEMSEQID), CInt(recurringItem.ROOTTASKITEMSEQID), CInt(recurringItem.STATUSSEQID)))
                                        End If
                                    Next
                                End If
                                If .IsDUEDATENull = False Then
                                    .DUEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.DUEDATE).ToShortDateString
                                End If
                                If .IsCLOSEDDATENull Then
                                    .CLOSEDDATE = String.Empty
                                Else
                                    .CLOSEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CLOSEDDATE).ToShortDateString
                                End If
                                If .IsCREATEDBYNull Then
                                    .CREATEDBY = String.Empty
                                End If
                                If .IsCREATEDDATENull Then
                                    .CREATEDDATE = Now.ToShortDateString
                                Else
                                    .CREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CREATEDDATE).ToShortDateString
                                End If
                                If .IsDATECRITICALNull Then
                                    .DATECRITICAL = "N"
                                End If
                                If .IsDESCRIPTIONNull Then
                                    .DESCRIPTION = String.Empty
                                End If
                                'If .IsDUEDATENull Then
                                '    .DUEDATE = CDate(String.Empty)
                                'End If
                                If .IsLASTUPDATEDATENull Then
                                    .LASTUPDATEDATE = .CREATEDDATE 'String.Empty
                                Else
                                    .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
                                End If
                                If .IsLASTUPDATEUSERNAMENull Then
                                    .LASTUPDATEUSERNAME = String.Empty
                                End If
                                If .IsLEADTIMENull Then
                                    .LEADTIME = CStr(0)
                                End If
                                If .IsPRIORITYNull Then
                                    .PRIORITY = String.Empty
                                End If
                                If .IsRESPONSIBLENAMENull Then
                                    .RESPONSIBLEUSERNAME = String.Empty
                                End If
                                If .IsRESPONSIBLEROLESEQIDNull Then
                                    .RESPONSIBLEROLESEQID = CStr(-1)
                                End If
                                If .IsRESPONSIBLEUSERNAMENull Then
                                    .RESPONSIBLEUSERNAME = String.Empty
                                End If
                                If .IsROLENAMENull Then
                                    .ROLENAME = String.Empty
                                End If
                                If .IsROOTTASKITEMSEQIDNull Then
                                    .ROOTTASKITEMSEQID = -1
                                End If
                                If .IsSTATUSNAMENull Then
                                    .STATUSNAME = String.Empty
                                End If
                                If .IsSTATUSSEQIDNull Then
                                    .STATUSSEQID = CStr(-1)
                                End If
                                If .IsROLEDESCRIPTIONNull Then
                                    .ROLEDESCRIPTION = String.Empty
                                End If
                                If .IsTASKHEADERSEQIDNull Then
                                    Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
                                End If
                                If .IsTITLENull Then
                                    .TITLE = String.Empty
                                End If
                                If .IsRESPROLEPLANTCODENull Then
                                    .RESPROLEPLANTCODE = String.Empty
                                End If
                                If .IsRESPROLESITENAMENull Then
                                    .RESPROLESITENAME = String.Empty
                                End If
                                If .IsDAYSBEFORENull Then
                                    .DAYSBEFORE = CStr(-1)
                                End If
                                If .IsDEPENDENTTASKSEQIDNull Then
                                    .DEPENDENTTASKSEQID = String.Empty
                                End If
                                If .IsDEPENDENTCHILDIDNull Then
                                    .DEPENDENTCHILDID = 0
                                End If
                                If .IsDAYSAFTERNull Then
                                    .DAYSAFTER = CStr(-1)
                                End If
                                If .IsWORKORDERNull Then
                                    .WORKORDER = String.Empty
                                End If
                                If .IsCREATEDBYUSERNAMENull Then
                                    .CREATEDBYUSERNAME = String.Empty
                                End If
                                'If .ROOTTASKITEMSEQID = -1 Then
                                If .IsDUEDATENull = False And (.DEPENDENTTASKSEQID.Length = 0 Or .DEPENDENTTASKSEQID = .TASKITEMSEQID.ToString) Then
                                    record.Add(New TaskItem(.CLOSEDDATE, .CREATEDBY, .CREATEDDATE, .DATECRITICAL, .DESCRIPTION, .DUEDATE, .LASTUPDATEDATE, .LASTUPDATEUSERNAME, CInt(.LEADTIME), .PRIORITY, .RESPONSIBLENAME, CInt(.RESPONSIBLEROLESEQID), .RESPONSIBLEUSERNAME, .ROLENAME, CStr(.ROOTTASKITEMSEQID), CInt(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TITLE, .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME, recurringRecord, CInt(.DAYSBEFORE), CStr(.DEPENDENTTASKSEQID), CStr(.DEPENDENTCHILDID), CInt(.DAYSAFTER), .WORKORDER, .SORTORDER, .ESTIMATEDCOST, .ACTUALCOST, .TANKINSPECTIONTYPEID, .ORIGINALROLESEQID, .CREATEDBYUSERNAME))
                                    recurringRecord = Nothing
                                End If
                                'End If
                            End With
                        Next
                        'InsertDataIntoCache("GetRootTaskItemList_" & taskHeaderNumber, 0.5, record)
                    End If
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetTaskItemList", , ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
        adapter = Nothing
    End Function

    ''' <summary>
    ''' Gets Dependent Task Item List
    ''' </summary>
    ''' <param name="parentTaskNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDependentTaskItemList(ByVal parentTaskNumber As Integer) As System.Collections.Generic.List(Of TaskItem)
        Dim adapter As TaskDetailTableAdapters.TaskItemTableAdapter
        Dim recurringAdapter As TaskDetailTableAdapters.RecurringTasksTableAdapter
        Dim table As TaskDetail.TaskItemDataTable = Nothing
        Dim recurringTable As TaskDetail.RecurringTasksDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim record As System.Collections.Generic.List(Of TaskItem) = Nothing
        Dim recurringRecord As System.Collections.Generic.List(Of RecurringTasks) = Nothing

        Try
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.TaskItemTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                'record = CType(GetDataFromCache("GetRootTaskItemList_" & parentTaskNumber, 0.5), System.Collections.Generic.List(Of TaskItem))
                If record Is Nothing OrElse record.Count = 0 Then
                    table = adapter.GetDependentTasks(CStr(parentTaskNumber), emptyCursor)
                    If table IsNot Nothing Then
                        record = New System.Collections.Generic.List(Of TaskItem)
                        For Each rowItem As TaskDetail.TaskItemRow In table.Rows
                            With rowItem
                                'recurringTable=adapter.get
                                If .IsROOTTASKITEMSEQIDNull OrElse .ROOTTASKITEMSEQID = CDec(.TASKITEMSEQID) Then '= False AndAlso .ROOTTASKITEMSEQID = -1 Then
                                    recurringAdapter = New TaskDetailTableAdapters.RecurringTasksTableAdapter
                                    recurringAdapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                                    recurringTable = recurringAdapter.GetRecurringTasks(CStr(.TASKITEMSEQID), Nothing)
                                    If recurringTable IsNot Nothing AndAlso recurringTable.Rows.Count > 0 Then
                                        'Dim recurringDates As New StringBuilder
                                        recurringRecord = New System.Collections.Generic.List(Of RecurringTasks)
                                        For Each recurringItem As TaskDetail.RecurringTasksRow In recurringTable.Rows
                                            If recurringItem.IsDUEDATENull = False Then
                                                'If recurringDates.Length > 0 Then
                                                '    recurringDates.Append(",")
                                                'End If
                                                'recurringDates.Append(recurringItem.DUEDATE)
                                                recurringRecord.Add(New RecurringTasks(IP.Bids.SharedFunctions.CDateFromEnglishDate(recurringItem.DUEDATE).ToShortDateString, CInt(recurringItem.TASKITEMSEQID), CInt(recurringItem.ROOTTASKITEMSEQID), CInt(recurringItem.STATUSSEQID)))
                                            End If
                                        Next
                                    End If
                                    If .IsDUEDATENull = False Then
                                        .DUEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.DUEDATE).ToShortDateString
                                    End If
                                    If .IsCLOSEDDATENull Then
                                        .CLOSEDDATE = String.Empty
                                    Else
                                        .CLOSEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CLOSEDDATE).ToShortDateString
                                    End If
                                    If .IsCREATEDBYNull Then
                                        .CREATEDBY = String.Empty
                                    End If
                                    If .IsCREATEDDATENull Then
                                        .CREATEDDATE = Now.ToShortDateString
                                    Else
                                        .CREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CREATEDDATE).ToShortDateString
                                    End If
                                    If .IsDATECRITICALNull Then
                                        .DATECRITICAL = "N"
                                    End If
                                    If .IsDESCRIPTIONNull Then
                                        .DESCRIPTION = String.Empty
                                    End If
                                    'If .IsDUEDATENull Then
                                    '    .DUEDATE = CDate(String.Empty)
                                    'End If
                                    If .IsLASTUPDATEDATENull Then
                                        .LASTUPDATEDATE = .CREATEDDATE 'String.Empty
                                    Else
                                        .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
                                    End If
                                    If .IsLASTUPDATEUSERNAMENull Then
                                        .LASTUPDATEUSERNAME = String.Empty
                                    End If
                                    If .IsLEADTIMENull Then
                                        .LEADTIME = CStr(0)
                                    End If
                                    If .IsPRIORITYNull Then
                                        .PRIORITY = String.Empty
                                    End If
                                    If .IsRESPONSIBLENAMENull Then
                                        .RESPONSIBLEUSERNAME = String.Empty
                                    End If
                                    If .IsRESPONSIBLEROLESEQIDNull Then
                                        .RESPONSIBLEROLESEQID = CStr(-1)
                                    End If
                                    If .IsRESPONSIBLEUSERNAMENull Then
                                        .RESPONSIBLEUSERNAME = String.Empty
                                    End If
                                    If .IsROLENAMENull Then
                                        .ROLENAME = String.Empty
                                    End If
                                    If .IsROOTTASKITEMSEQIDNull Then
                                        .ROOTTASKITEMSEQID = -1
                                    End If
                                    If .IsSTATUSNAMENull Then
                                        .STATUSNAME = String.Empty
                                    End If
                                    If .IsSTATUSSEQIDNull Then
                                        .STATUSSEQID = CStr(-1)
                                    End If
                                    If .IsROLEDESCRIPTIONNull Then
                                        .ROLEDESCRIPTION = String.Empty
                                    End If
                                    If .IsTASKHEADERSEQIDNull Then
                                        Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
                                    End If
                                    If .IsTITLENull Then
                                        .TITLE = String.Empty
                                    End If
                                    If .IsRESPROLEPLANTCODENull Then
                                        .RESPROLEPLANTCODE = String.Empty
                                    End If
                                    If .IsRESPROLESITENAMENull Then
                                        .RESPROLESITENAME = String.Empty
                                    End If
                                    If .IsDAYSBEFORENull Then
                                        .DAYSBEFORE = CStr(-1)
                                    End If
                                    If .IsDEPENDENTTASKSEQIDNull Then
                                        .DEPENDENTTASKSEQID = String.Empty
                                    End If
                                    If .IsDEPENDENTCHILDIDNull Then
                                        .DEPENDENTCHILDID = 0
                                    End If
                                    If .IsDAYSAFTERNull Then
                                        .DAYSAFTER = CStr(-1)
                                    End If
                                    If .IsWORKORDERNull Then
                                        .WORKORDER = String.Empty
                                    End If
                                    If .IsCREATEDBYUSERNAMENull Then
                                        .CREATEDBYUSERNAME = String.Empty
                                    End If
                                    'If .ROOTTASKITEMSEQID = -1 Then
                                    If .IsDUEDATENull = False Then
                                        record.Add(New TaskItem(.CLOSEDDATE, .CREATEDBY, .CREATEDDATE, .DATECRITICAL, .DESCRIPTION, .DUEDATE, .LASTUPDATEDATE, .LASTUPDATEUSERNAME, CInt(.LEADTIME), .PRIORITY, .RESPONSIBLENAME, CInt(.RESPONSIBLEROLESEQID), .RESPONSIBLEUSERNAME, .ROLENAME, CStr(.ROOTTASKITEMSEQID), CInt(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TITLE, .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME, recurringRecord, CInt(.DAYSBEFORE), CStr(.DEPENDENTTASKSEQID), CStr(.DEPENDENTCHILDID), CInt(.DAYSAFTER), .WORKORDER, .SORTORDER, .ESTIMATEDCOST, .ACTUALCOST, .TANKINSPECTIONTYPEID, .ORIGINALROLESEQID, .CREATEDBYUSERNAME))
                                        recurringRecord = Nothing
                                    End If
                                End If
                            End With
                        Next
                        'InsertDataIntoCache("GetRootTaskItemList_" & taskHeaderNumber, 0.5, record)
                    End If
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetDependentTaskItemList", , ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
        adapter = Nothing
    End Function

    ''' <summary>
    ''' Gets a Task Item
    ''' </summary>
    ''' <param name="taskItemNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTaskItem(ByVal taskItemNumber As Integer) As TaskItem
        Dim adapter As TaskDetailTableAdapters.TaskItemTableAdapter
        Dim table As TaskDetail.TaskItemDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim record As TaskItem = Nothing


        Dim recurringAdapter As TaskDetailTableAdapters.RecurringTasksTableAdapter
        Dim recurringTable As TaskDetail.RecurringTasksDataTable = Nothing
        Dim recurringRecord As System.Collections.Generic.List(Of RecurringTasks) = Nothing

        Try
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.TaskItemTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetTaskItem(CStr(taskItemNumber), emptyCursor)
                If table IsNot Nothing Then
                    record = New TaskItem
                    For Each rowItem As TaskDetail.TaskItemRow In table.Rows
                        With rowItem
                            If Not .IsROOTTASKITEMSEQIDNull Then
                                recurringAdapter = New TaskDetailTableAdapters.RecurringTasksTableAdapter
                                recurringAdapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                                recurringTable = recurringAdapter.GetRecurringTasks(CStr(.ROOTTASKITEMSEQID), Nothing)
                                If recurringTable IsNot Nothing AndAlso recurringTable.Rows.Count > 0 Then
                                    'Dim recurringDates As New StringBuilder
                                    recurringRecord = New System.Collections.Generic.List(Of RecurringTasks)
                                    For Each recurringItem As TaskDetail.RecurringTasksRow In recurringTable.Rows
                                        If recurringItem.IsDUEDATENull = False Then
                                            recurringRecord.Add(New RecurringTasks(IP.Bids.SharedFunctions.CDateFromEnglishDate(recurringItem.DUEDATE).ToShortDateString, CInt(recurringItem.TASKITEMSEQID), CInt(recurringItem.ROOTTASKITEMSEQID), CInt(recurringItem.STATUSSEQID)))
                                        End If
                                    Next
                                End If
                            End If
                            If .IsDUEDATENull = False Then
                                .DUEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.DUEDATE).ToShortDateString
                            End If
                            If .IsCLOSEDDATENull Then
                                .CLOSEDDATE = String.Empty
                            Else
                                .CLOSEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CLOSEDDATE).ToShortDateString
                            End If
                            If .IsCREATEDBYNull Then
                                .CREATEDBY = Now.ToShortDateString
                            End If
                            If .IsCREATEDDATENull Then
                                .CREATEDDATE = Now.ToShortDateString 'String.Empty
                            Else
                                .CREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CREATEDDATE).ToShortDateString
                            End If
                            If .IsDATECRITICALNull Then
                                .DATECRITICAL = "N"
                            End If
                            If .IsDESCRIPTIONNull Then
                                .DESCRIPTION = String.Empty
                            End If
                            'If .IsDUEDATENull Then
                            '    .DUEDATE = CDate(String.Empty)
                            'End If
                            If .IsLASTUPDATEDATENull Then
                                .LASTUPDATEDATE = .CREATEDDATE
                            Else
                                .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
                            End If
                            If .IsLASTUPDATEUSERNAMENull OrElse .LASTUPDATEUSERNAME.Trim.Length = 0 Then
                                .LASTUPDATEUSERNAME = .CREATEDBY
                            End If
                            If .IsLEADTIMENull Then
                                .LEADTIME = CStr(0)
                            End If
                            If .IsPRIORITYNull Then
                                .PRIORITY = String.Empty
                            End If
                            If .IsRESPONSIBLENAMENull Then
                                .RESPONSIBLEUSERNAME = String.Empty
                            End If
                            If .IsRESPONSIBLEROLESEQIDNull Then
                                .RESPONSIBLEROLESEQID = CStr(-1)
                            End If
                            If .IsRESPONSIBLEUSERNAMENull Then
                                .RESPONSIBLEUSERNAME = String.Empty
                            End If
                            If .IsROLENAMENull Then
                                .ROLENAME = String.Empty
                            End If
                            If .IsROOTTASKITEMSEQIDNull Then
                                .ROOTTASKITEMSEQID = CDec(.TASKITEMSEQID)
                            End If
                            If .IsSTATUSNAMENull Then
                                .STATUSNAME = String.Empty
                            End If
                            If .IsSTATUSSEQIDNull Then
                                .STATUSSEQID = CStr(-1)
                            End If
                            If .IsROLEDESCRIPTIONNull Then
                                .ROLEDESCRIPTION = String.Empty
                            End If
                            If .IsTASKHEADERSEQIDNull Then
                                Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
                            End If
                            If .IsTITLENull Then
                                .TITLE = String.Empty
                            End If
                            If .IsRESPROLEPLANTCODENull Then
                                .RESPROLEPLANTCODE = String.Empty
                            End If
                            If .IsRESPROLESITENAMENull Then
                                .RESPROLESITENAME = String.Empty
                            End If
                            If .IsDAYSBEFORENull Then
                                .DAYSBEFORE = CStr(-1)
                            End If
                            If .IsDEPENDENTTASKSEQIDNull Then
                                .DEPENDENTTASKSEQID = String.Empty
                            End If
                            If .IsDEPENDENTCHILDIDNull Then
                                .DEPENDENTCHILDID = 0
                            End If
                            If .IsDAYSAFTERNull Then
                                .DAYSAFTER = CStr(-1)
                            End If
                            If .IsWORKORDERNull Then
                                .WORKORDER = String.Empty
                            End If
                            If .IsCREATEDBYUSERNAMENull Then
                                .CREATEDBYUSERNAME = String.Empty
                            End If
                            If .IsDUEDATENull = False Then
                                record = New TaskItem(.CLOSEDDATE, .CREATEDBY, .CREATEDDATE, .DATECRITICAL, .DESCRIPTION, .DUEDATE, .LASTUPDATEDATE, .LASTUPDATEUSERNAME, CInt(.LEADTIME), .PRIORITY, .RESPONSIBLENAME, CInt(.RESPONSIBLEROLESEQID), .RESPONSIBLEUSERNAME, .ROLENAME, CStr(.ROOTTASKITEMSEQID), CInt(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TITLE, .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME, recurringRecord, CInt(.DAYSBEFORE), .DEPENDENTTASKSEQID, CStr(.DEPENDENTCHILDID), CInt(.DAYSAFTER), .WORKORDER, .SORTORDER, .ESTIMATEDCOST, .ACTUALCOST, .TANKINSPECTIONTYPEID, .ORIGINALROLESEQID, .CREATEDBYUSERNAME)
                            End If
                            recurringRecord = Nothing
                        End With
                    Next
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetTaskItem", , ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
        adapter = Nothing
    End Function

    ''' <summary>
    ''' Get Dependent Task Item
    ''' </summary>
    ''' <param name="taskItemNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDependentTaskItem(ByVal taskItemNumber As Integer) As TaskItem
        Dim adapter As TaskDetailTableAdapters.TaskItemTableAdapter
        Dim table As TaskDetail.TaskItemDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim record As TaskItem = Nothing


        Dim recurringAdapter As TaskDetailTableAdapters.RecurringTasksTableAdapter
        Dim recurringTable As TaskDetail.RecurringTasksDataTable = Nothing
        Dim recurringRecord As System.Collections.Generic.List(Of RecurringTasks) = Nothing

        Try
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.TaskItemTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetDependentTasks(CStr(taskItemNumber), emptyCursor)
                If table IsNot Nothing Then
                    record = New TaskItem
                    For Each rowItem As TaskDetail.TaskItemRow In table.Rows
                        With rowItem
                            If Not .IsROOTTASKITEMSEQIDNull Then
                                recurringAdapter = New TaskDetailTableAdapters.RecurringTasksTableAdapter
                                recurringAdapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                                recurringTable = recurringAdapter.GetRecurringTasks(CStr(.ROOTTASKITEMSEQID), Nothing)
                                If recurringTable IsNot Nothing AndAlso recurringTable.Rows.Count > 0 Then
                                    'Dim recurringDates As New StringBuilder
                                    recurringRecord = New System.Collections.Generic.List(Of RecurringTasks)
                                    For Each recurringItem As TaskDetail.RecurringTasksRow In recurringTable.Rows
                                        If recurringItem.IsDUEDATENull = False Then
                                            recurringRecord.Add(New RecurringTasks(IP.Bids.SharedFunctions.CDateFromEnglishDate(recurringItem.DUEDATE).ToShortDateString, CInt(recurringItem.TASKITEMSEQID), CInt(recurringItem.ROOTTASKITEMSEQID), CInt(recurringItem.STATUSSEQID)))
                                        End If
                                    Next
                                End If
                            End If
                            If .IsDUEDATENull = False Then
                                .DUEDATE = CDate(.DUEDATE).ToShortDateString
                            End If
                            If .IsCLOSEDDATENull Then
                                .CLOSEDDATE = String.Empty
                            Else
                                .CLOSEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CLOSEDDATE).ToShortDateString
                            End If
                            If .IsCREATEDBYNull Then
                                .CREATEDBY = String.Empty
                            End If
                            If .IsCREATEDDATENull Then
                                .CREATEDDATE = Now.ToShortDateString 'String.Empty
                            Else
                                .CREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CREATEDDATE).ToShortDateString
                            End If
                            If .IsDATECRITICALNull Then
                                .DATECRITICAL = "N"
                            End If
                            If .IsDESCRIPTIONNull Then
                                .DESCRIPTION = String.Empty
                            End If
                            'If .IsDUEDATENull Then
                            '    .DUEDATE = CDate(String.Empty)
                            'End If
                            If .IsLASTUPDATEDATENull Then
                                .LASTUPDATEDATE = .CREATEDDATE 'String.Empty
                            Else
                                .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
                            End If
                            If .IsLASTUPDATEUSERNAMENull Then
                                .LASTUPDATEUSERNAME = String.Empty
                            End If
                            If .IsLEADTIMENull Then
                                .LEADTIME = CStr(0)
                            End If
                            If .IsPRIORITYNull Then
                                .PRIORITY = String.Empty
                            End If
                            If .IsRESPONSIBLENAMENull Then
                                .RESPONSIBLEUSERNAME = String.Empty
                            End If
                            If .IsRESPONSIBLEROLESEQIDNull Then
                                .RESPONSIBLEROLESEQID = CStr(-1)
                            End If
                            If .IsRESPONSIBLEUSERNAMENull Then
                                .RESPONSIBLEUSERNAME = String.Empty
                            End If
                            If .IsROLENAMENull Then
                                .ROLENAME = String.Empty
                            End If
                            If .IsROOTTASKITEMSEQIDNull Then
                                .ROOTTASKITEMSEQID = CDec(.TASKITEMSEQID)
                            End If
                            If .IsSTATUSNAMENull Then
                                .STATUSNAME = String.Empty
                            End If
                            If .IsSTATUSSEQIDNull Then
                                .STATUSSEQID = CStr(-1)
                            End If
                            If .IsROLEDESCRIPTIONNull Then
                                .ROLEDESCRIPTION = String.Empty
                            End If
                            If .IsTASKHEADERSEQIDNull Then
                                Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
                            End If
                            If .IsTITLENull Then
                                .TITLE = String.Empty
                            End If
                            If .IsRESPROLEPLANTCODENull Then
                                .RESPROLEPLANTCODE = String.Empty
                            End If
                            If .IsRESPROLESITENAMENull Then
                                .RESPROLESITENAME = String.Empty
                            End If
                            If .IsDAYSBEFORENull Then
                                .DAYSBEFORE = CStr(-1)
                            End If
                            If .IsDEPENDENTTASKSEQIDNull Then
                                .DEPENDENTTASKSEQID = String.Empty
                            End If
                            If .IsDEPENDENTCHILDIDNull Then
                                .DEPENDENTCHILDID = 0
                            End If
                            If .IsDAYSAFTERNull Then
                                .DAYSAFTER = CStr(-1)
                            End If
                            If .IsWORKORDERNull Then
                                .WORKORDER = String.Empty
                            End If
                            If .IsCREATEDBYUSERNAMENull Then
                                .CREATEDBYUSERNAME = String.Empty
                            End If
                            If .IsDUEDATENull = False Then
                                record = New TaskItem(.CLOSEDDATE, .CREATEDBY, .CREATEDDATE, .DATECRITICAL, .DESCRIPTION, .DUEDATE, .LASTUPDATEDATE, .LASTUPDATEUSERNAME, CInt(.LEADTIME), .PRIORITY, .RESPONSIBLENAME, CInt(.RESPONSIBLEROLESEQID), .RESPONSIBLEUSERNAME, .ROLENAME, CStr(.ROOTTASKITEMSEQID), CInt(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TITLE, .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME, recurringRecord, CInt(.DAYSBEFORE), .DEPENDENTTASKSEQID, CStr(.DEPENDENTCHILDID), CInt(.DAYSAFTER), .WORKORDER, .SORTORDER, .ESTIMATEDCOST, .ACTUALCOST, .TANKINSPECTIONTYPEID, .ORIGINALROLESEQID, .CREATEDBYUSERNAME)
                            End If
                            recurringRecord = Nothing
                        End With
                    Next
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetDependentTaskItem", , ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
        adapter = Nothing
    End Function

    Public Function GetSubTaskItemList(ByVal taskItemNumber As Integer, ByVal displayTemplatesOnly As Boolean) As System.Collections.Generic.List(Of SubTaskItem)
        Dim paramCollection As New OracleParameterCollection
        Dim param As New OracleParameter
        Dim ds As System.Data.DataSet = Nothing
        Dim dr As Data.DataTableReader = Nothing
        Dim templateFlag As String = String.Empty

        Dim adapter As TaskDetailTableAdapters.SubTaskTableAdapter
        Dim table As TaskDetail.SubTaskDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim record As System.Collections.Generic.List(Of SubTaskItem) = Nothing

        Try
            If displayTemplatesOnly = True Then
                templateFlag = "Y"
            Else
                templateFlag = "N"
            End If

            'Set Procedure Parameters
            param = New OracleParameter
            param.ParameterName = "in_TaskITem"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = taskItemNumber.ToString
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "in_TemplateFlag"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = templateFlag
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "rsTaskItem"
            param.OracleDbType = OracleDbType.Cursor
            param.Direction = Data.ParameterDirection.Output
            paramCollection.Add(param)

            Dim key As String = "GetSubsequentTasks" & taskItemNumber.ToString

            ds = HelperDal.GetDSFromPackage(paramCollection, "Reladmin.MTTTASKITEM.GetSubsequentTasks", key, 0)

            If ds IsNot Nothing Then
                If ds.Tables.Count = 1 Then
                    dr = ds.Tables(0).CreateDataReader
                End If
            End If


            If dr IsNot Nothing Then
                If dr.HasRows Then
                    record = New System.Collections.Generic.List(Of SubTaskItem)
                    Do While dr.Read
                        Dim newSubTaskItem As New SubTaskItem
                        With newSubTaskItem
                            If dr.Item("TASKITEMSEQID") IsNot Nothing Then
                                .TaskItemSeqId = DataClean(dr.Item("TASKITEMSEQID"))
                            End If
                            If dr.Item("TASKHEADERSEQID") IsNot Nothing Then
                                .TaskHeaderSeqId = DataClean(dr.Item("TASKHEADERSEQID"))
                            End If
                            If dr.Item("TITLE") IsNot Nothing Then
                                .Title = DataClean(dr.Item("TITLE"))
                            End If
                            If dr.Item("roottaskitemseqid") IsNot Nothing Then
                                .RootTaskItemSeqId = DataClean(dr.Item("roottaskitemseqid"))
                            End If
                            If dr.Item("parentsubtaskseqid") IsNot Nothing Then
                                .ParentSubTaskSeqID = DataClean(dr.Item("parentsubtaskseqid"))
                            End If
                            If dr.Item("RESPONSIBLEname") IsNot Nothing Then
                                .ResponsibleName = DataClean(dr.Item("RESPONSIBLEname"))
                            End If
                            If dr.Item("RESPONSIBLEUSERNAME") IsNot Nothing Then
                                .ResponsibleUserName = DataClean(dr.Item("RESPONSIBLEUSERNAME"))
                            End If
                            If dr.Item("rolename") IsNot Nothing Then
                                .RoleName = DataClean(dr.Item("rolename"))
                            End If
                            If dr.Item("RESPONSIBLEROLESEQID") IsNot Nothing Then
                                .ResponsibleRoleSeqId = CInt(DataClean(dr.Item("RESPONSIBLEROLESEQID"), "-1"))
                            End If
                            If dr.Item("CREATEDBY") IsNot Nothing Then
                                .CreatedBy = DataClean(dr.Item("CREATEDBY"))
                            End If
                            If dr.Item("CREATEDDATE") IsNot Nothing Then
                                .CreatedDate = DataClean(dr.Item("CREATEDDATE"))
                            End If
                            If dr.Item("LASTUPDATEUSERNAME") IsNot Nothing Then
                                .LastUpdateUserName = DataClean(dr.Item("LASTUPDATEUSERNAME"))
                            End If
                            If dr.Item("LASTUPDATEDATE") IsNot Nothing Then
                                .LastUpdateDate = DataClean(dr.Item("LASTUPDATEDATE"))
                            End If
                            If dr.Item("DESCRIPTION") IsNot Nothing Then
                                .Description = DataClean(dr.Item("DESCRIPTION"))
                            End If
                            If dr.Item("DaysAfter") IsNot Nothing Then
                                .DaysAfter = CInt(DataClean(dr.Item("DaysAfter"), CStr(0)))
                            End If
                            If dr.Item("resproleplantcode") IsNot Nothing Then
                                .ResponsibleRolePlantCode = DataClean(dr.Item("resproleplantcode"))
                            End If
                            If dr.Item("roledescription") IsNot Nothing Then
                                .RoleDescription = DataClean(dr.Item("roledescription"))
                            End If
                            If dr.Item("resprolesitename") IsNot Nothing Then
                                .ResponsibleRoleSiteName = DataClean(dr.Item("resprolesitename"))
                            End If
                            If dr.Item("DUEDATE") IsNot Nothing Then
                                .DueDate = DataClean(dr.Item("DUEDATE"))
                            End If
                        End With
                        record.Add(newSubTaskItem)
                    Loop
                End If
            End If


            'If table Is Nothing OrElse table.Rows.Count = 0 Then
            '    adapter = New TaskDetailTableAdapters.SubTaskTableAdapter
            '    adapter.Connection = New Devart.Data.Oracle.OracleConnection (ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            '    table = adapter.GetSubTasks(CStr(taskItemNumber), emptyCursor)
            '    If table IsNot Nothing Then
            '        record = New System.Collections.Generic.List(Of SubTaskItem)
            '        For Each rowItem As TaskDetail.SubTaskRow In table.Rows
            '            With rowItem

            '                If .IsCREATEDBYNull Then
            '                    .CREATEDBY = String.Empty
            '                End If
            '                If .IsCREATEDDATENull Then
            '                    .CREATEDDATE = Now.ToShortDateString 'String.Empty
            '                Else
            '                    .CREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CREATEDDATE).ToShortDateString
            '                End If
            '                If .IsDESCRIPTIONNull Then
            '                    .DESCRIPTION = String.Empty
            '                End If

            '                If .IsLASTUPDATEDATENull Then
            '                    .LASTUPDATEDATE = .CREATEDDATE 'String.Empty
            '                Else
            '                    .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
            '                End If
            '                If .IsLASTUPDATEUSERNAMENull Then
            '                    .LASTUPDATEUSERNAME = String.Empty
            '                End If

            '                If .IsRESPONSIBLENAMENull Then
            '                    .RESPONSIBLEUSERNAME = String.Empty
            '                End If
            '                If .IsRESPONSIBLEROLESEQIDNull Then
            '                    .RESPONSIBLEROLESEQID = -1
            '                End If
            '                If .IsRESPONSIBLEUSERNAMENull Then
            '                    .RESPONSIBLEUSERNAME = String.Empty
            '                End If
            '                If .IsROLENAMENull Then
            '                    .ROLENAME = String.Empty
            '                End If
            '                If .IsROOTTASKITEMSEQIDNull Then
            '                    .ROOTTASKITEMSEQID = -1
            '                End If
            '                If .IsROLEDESCRIPTIONNull Then
            '                    .ROLEDESCRIPTION = String.Empty
            '                End If
            '                If .IsTASKHEADERSEQIDNull Then
            '                    Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
            '                End If
            '                If .IsTITLENull Then
            '                    .TITLE = String.Empty
            '                End If

            '                If .IsDAYSAFTERNull Then
            '                    .DAYSAFTER = -1
            '                End If

            '                If .IsPARENTSUBTASKSEQIDNull Then
            '                    .PARENTSUBTASKSEQID = -1
            '                End If
            '                If .IsRESPROLEPLANTCODENull Then
            '                    .RESPROLEPLANTCODE = String.Empty
            '                End If
            '                If .IsRESPROLESITENAMENull Then
            '                    .RESPROLESITENAME = String.Empty
            '                End If
            '                record.Add(New SubTaskItem(.CREATEDBY, .CREATEDDATE, .DESCRIPTION, .LASTUPDATEDATE, .LASTUPDATEUSERNAME, .RESPONSIBLENAME, CInt(.RESPONSIBLEROLESEQID), .RESPONSIBLEUSERNAME, .ROLENAME, CStr(.ROOTTASKITEMSEQID), CStr(.TASKHEADERSEQID), CStr(.TASKITEMSEQID), .TITLE, CInt(.DAYSAFTER), CStr(.PARENTSUBTASKSEQID), .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME))
            '            End With
            '        Next
            '    End If
            'End If
        Catch
            Throw
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
        adapter = Nothing
    End Function

    Public Function oldGetSubTaskItemList(ByVal taskItemNumber As Integer) As System.Collections.Generic.List(Of SubTaskItem)
        Dim adapter As TaskDetailTableAdapters.SubTaskTableAdapter
        Dim table As TaskDetail.SubTaskDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim record As System.Collections.Generic.List(Of SubTaskItem) = Nothing
        Try
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.SubTaskTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetSubTasks(CStr(taskItemNumber), emptyCursor)
                If table IsNot Nothing Then
                    record = New System.Collections.Generic.List(Of SubTaskItem)
                    For Each rowItem As TaskDetail.SubTaskRow In table.Rows
                        With rowItem

                            If .IsCREATEDBYNull Then
                                .CREATEDBY = String.Empty
                            End If
                            If .IsCREATEDDATENull Then
                                .CREATEDDATE = Now.ToShortDateString 'String.Empty
                            Else
                                .CREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CREATEDDATE).ToShortDateString
                            End If
                            If .IsDESCRIPTIONNull Then
                                .DESCRIPTION = String.Empty
                            End If

                            If .IsLASTUPDATEDATENull Then
                                .LASTUPDATEDATE = .CREATEDDATE 'String.Empty
                            Else
                                .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
                            End If
                            If .IsLASTUPDATEUSERNAMENull Then
                                .LASTUPDATEUSERNAME = String.Empty
                            End If

                            If .IsRESPONSIBLENAMENull Then
                                .RESPONSIBLEUSERNAME = String.Empty
                            End If
                            If .IsRESPONSIBLEROLESEQIDNull Then
                                .RESPONSIBLEROLESEQID = -1
                            End If
                            If .IsRESPONSIBLEUSERNAMENull Then
                                .RESPONSIBLEUSERNAME = String.Empty
                            End If
                            If .IsROLENAMENull Then
                                .ROLENAME = String.Empty
                            End If
                            If .IsROOTTASKITEMSEQIDNull Then
                                .ROOTTASKITEMSEQID = -1
                            End If
                            If .IsROLEDESCRIPTIONNull Then
                                .ROLEDESCRIPTION = String.Empty
                            End If
                            If .IsTASKHEADERSEQIDNull Then
                                Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
                            End If
                            If .IsTITLENull Then
                                .TITLE = String.Empty
                            End If

                            If .IsDAYSAFTERNull Then
                                .DAYSAFTER = -1
                            End If

                            If .IsPARENTSUBTASKSEQIDNull Then
                                .PARENTSUBTASKSEQID = -1
                            End If
                            If .IsRESPROLEPLANTCODENull Then
                                .RESPROLEPLANTCODE = String.Empty
                            End If
                            If .IsRESPROLESITENAMENull Then
                                .RESPROLESITENAME = String.Empty
                            End If
                            'record.Add(New SubTaskItem(.CREATEDBY, .CREATEDDATE, .DESCRIPTION, .LASTUPDATEDATE, .LASTUPDATEUSERNAME, .RESPONSIBLENAME, CInt(.RESPONSIBLEROLESEQID), .RESPONSIBLEUSERNAME, .ROLENAME, CStr(.ROOTTASKITEMSEQID), CStr(.TASKHEADERSEQID), CStr(.TASKITEMSEQID), .TITLE, CInt(.DAYSAFTER), CStr(.PARENTSUBTASKSEQID), .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME))
                        End With
                    Next
                End If
            End If
        Catch
            Throw
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
        adapter = Nothing
    End Function


    Public Function GetRecurringParameters(ByVal rootTaskItemNumber As Integer) As System.Collections.Generic.List(Of IP.MEAS.BO.RecurringParameters)
        Dim adapter As TaskDetailTableAdapters.RecurringParametersTableAdapter
        Dim table As TaskDetail.RecurringParametersDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim record As System.Collections.Generic.List(Of IP.MEAS.BO.RecurringParameters) = Nothing
        Try
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.RecurringParametersTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetRecurringParameters(CStr(rootTaskItemNumber), emptyCursor)
                If table IsNot Nothing Then
                    record = New System.Collections.Generic.List(Of IP.MEAS.BO.RecurringParameters)
                    For Each rowItem As TaskDetail.RecurringParametersRow In table.Rows
                        With rowItem
                            If .IsLASTUPDATEDATENull Then
                                .LASTUPDATEDATE = Now
                            End If
                            If .IsLASTUPDATEUSERNAMENull Then
                                .LASTUPDATEUSERNAME = String.Empty
                            End If
                            If .IsPROFILETYPENAMENull Then
                                .PROFILETYPENAME = "Missing"
                            End If
                            If .IsPROFILETYPEVALUENull Then
                                .PROFILETYPEVALUE = String.Empty
                            End If
                            record.Add(New RecurringParameters(CInt(.TASKITEMSEQID), CInt(.PROFILETYPESEQID), .PROFILETYPENAME, .PROFILETYPEVALUE, .LASTUPDATEUSERNAME, .LASTUPDATEDATE))
                        End With
                    Next
                End If
            End If
        Catch
            Throw
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
        adapter = Nothing
    End Function

    Public Function GetRecurringParameterList() As System.Collections.Generic.List(Of IP.MEAS.BO.RecurringParametersList)
        Dim adapter As TaskDetailTableAdapters.RecurringParametersListTableAdapter
        Dim table As TaskDetail.RecurringParametersListDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim record As System.Collections.Generic.List(Of IP.MEAS.BO.RecurringParametersList) = Nothing
        Try
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.RecurringParametersListTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetRecurringParametersList(emptyCursor)
                If table IsNot Nothing Then
                    record = New System.Collections.Generic.List(Of IP.MEAS.BO.RecurringParametersList)
                    For Each rowItem As TaskDetail.RecurringParametersListRow In table.Rows
                        With rowItem
                            If .IsPROFILETYPENull Then
                                .PROFILETYPE = String.Empty
                            End If
                            If .IsPROFILETYPENAMENull Then
                                .PROFILETYPENAME = "Missing"
                            End If
                            record.Add(New RecurringParametersList(CInt(.PROFILETYPESEQID), .PROFILETYPENAME, .PROFILETYPE))
                        End With
                    Next
                End If
            End If
        Catch
            Throw
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
        adapter = Nothing
    End Function

    Public Sub SaveRecurringParameters(ByVal item As System.Collections.Generic.List(Of IP.MEAS.BO.RecurringParameters), ByVal originalItem As System.Collections.Generic.List(Of IP.MEAS.BO.RecurringParameters))
        Dim adapter As TaskDetailTableAdapters.RecurringParametersTableAdapter
        Dim table As TaskDetail.RecurringParametersDataTable = Nothing

        Dim out_status As Decimal

        Try
            adapter = New TaskDetailTableAdapters.RecurringParametersTableAdapter
            adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            With item
                Dim taskID As String = String.Empty
                Dim userName As String = String.Empty
                Dim sbRepeatingData As New StringBuilder
                Dim sbCurrentRepeatingData As New StringBuilder
                For Each param As IP.MEAS.BO.RecurringParameters In item
                    If sbRepeatingData.Length = 0 Then
                        taskID = CStr(param.TaskItemSeqId)
                        userName = CStr(param.LastUpdateUsername)
                        sbRepeatingData.Append(param.ProfileTypeSeqId & "|" & param.ProfileTypeValue)
                    Else
                        sbRepeatingData.Append(",")
                        sbRepeatingData.Append(param.ProfileTypeSeqId & "|" & param.ProfileTypeValue)
                    End If
                Next

                For Each param As IP.MEAS.BO.RecurringParameters In originalItem
                    If sbCurrentRepeatingData.Length = 0 Then
                        taskID = CStr(param.TaskItemSeqId)
                        userName = CStr(param.LastUpdateUsername)
                        sbCurrentRepeatingData.Append(param.ProfileTypeSeqId & "|" & param.ProfileTypeValue)
                    Else
                        sbCurrentRepeatingData.Append(",")
                        sbCurrentRepeatingData.Append(param.ProfileTypeSeqId & "|" & param.ProfileTypeValue)
                    End If
                Next
                Dim arCurrent As String() = sbCurrentRepeatingData.ToString.Split(CChar(","))
                Dim arNew As String() = sbRepeatingData.ToString.Split(CChar(","))
                Array.Sort(arCurrent)
                Array.Sort(arNew)
                Dim currentData As String = String.Join(",", arCurrent)
                Dim newData As String = String.Join(",", arNew)
                If currentData <> newData Then
                    adapter.UpdateRecurringParameters(taskID, newData, userName.ToUpper, CType(out_status, Global.System.Nullable(Of Decimal)))
                End If
            End With
        Catch
            Throw
        Finally
            adapter = Nothing
            table = Nothing
        End Try

    End Sub

    Public Sub UpdateTaskItemComment(ByVal comments As TaskItemComments)
        Dim adapter As TaskDetailTableAdapters.TaskItemCommentsTableAdapter
        Dim outStatus As Decimal?
        Try
            adapter = New TaskDetailTableAdapters.TaskItemCommentsTableAdapter
            adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)

            adapter.UpdateComments(comments.CommentsSeqId, comments.Comments, "U", comments.Username, outStatus)
            If outStatus <> 0 Then
                Throw New DataException(String.Format("Error updating comments for {0}", comments.CommentsSeqId))
            End If
        Catch
            Throw
        End Try
    End Sub

    Public Sub DeleteTaskItemComment(ByVal comments As TaskItemComments)
        Dim adapter As TaskDetailTableAdapters.TaskItemCommentsTableAdapter
        Dim outStatus As Decimal?
        Try
            adapter = New TaskDetailTableAdapters.TaskItemCommentsTableAdapter
            adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)

            adapter.UpdateComments(comments.CommentsSeqId, comments.Comments, "D", comments.Username, outStatus)
            If outStatus <> 0 Then
                Throw New DataException(String.Format("Error deleting comments for {0}", comments.CommentsSeqId))
            End If
        Catch
            Throw
        End Try
    End Sub
    Public Function GetTaskItemCommentsList(ByVal taskItemNumber As Integer) As System.Collections.Generic.List(Of IP.MEAS.BO.TaskItemComments)
        Dim adapter As TaskDetailTableAdapters.TaskItemCommentsTableAdapter
        Dim table As TaskDetail.TaskItemCommentsDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim record As System.Collections.Generic.List(Of IP.MEAS.BO.TaskItemComments) = Nothing

        Dim FilePath As String = HttpContext.Current.Server.MapPath("~\\TraceLog\\")

        IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "TaskTrackerItemBLL_GetTaskItemCommentsList.txt", "GetTaskItemCommentsList Start: " & DateTime.Now.ToString, True)


        Try
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.TaskItemCommentsTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetComments(CStr(taskItemNumber), emptyCursor)
                If table IsNot Nothing Then
                    record = New System.Collections.Generic.List(Of IP.MEAS.BO.TaskItemComments)
                    For Each rowItem As TaskDetail.TaskItemCommentsRow In table.Rows
                        With rowItem
                            If .IsCOMMENTSNull Then
                                .COMMENTS = String.Empty
                            End If
                            If .IsUSERNAMENull Then
                                .USERNAME = String.Empty
                            End If
                            If .IsLASTUPDATEDATENull Then
                                .LASTUPDATEDATE = Now.ToShortDateString 'String.Empty
                            End If
                            If .IsLASTUPDATEUSERNAMENull Then
                                .LASTUPDATEUSERNAME = String.Empty
                            End If
                            If .IsCREATEDBYNull Then
                                .CREATEDBY = String.Empty
                            End If

                            record.Add(New TaskItemComments(CStr(.COMMENTSEQID), .USERNAME, .LASTUPDATEUSERNAME, IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE), .COMMENTS, .CREATEDBY))
                        End With
                    Next
                End If
            End If
        Catch
            Throw
        Finally
            adapter = Nothing
            table = Nothing
        End Try

        IP.Bids.SharedFunctions.AddToTraceLog(FilePath & "TaskTrackerItemBLL_GetTaskItemCommentsList.txt", "GetTaskItemCommentsList End: " & DateTime.Now.ToString, True)

        Return record
        adapter = Nothing
    End Function

    Public Function GetOutageTemplateTaskItem(ByVal taskItemNumber As Integer) As TaskItem
        Dim adapter As TaskDetailTableAdapters.TaskItemTableAdapter
        Dim table As TaskDetail.TaskItemDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim record As TaskItem = Nothing
        Dim tempduedate As String = String.Empty

        Dim recurringAdapter As TaskDetailTableAdapters.RecurringTasksTableAdapter
        Dim recurringTable As TaskDetail.RecurringTasksDataTable = Nothing
        Dim recurringRecord As System.Collections.Generic.List(Of RecurringTasks) = Nothing

        Try
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.TaskItemTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetOutageTemplateTaskItem(CStr(taskItemNumber), emptyCursor)
                If table IsNot Nothing Then
                    record = New TaskItem
                    For Each rowItem As TaskDetail.TaskItemRow In table.Rows
                        With rowItem
                            If Not .IsROOTTASKITEMSEQIDNull Then
                                recurringAdapter = New TaskDetailTableAdapters.RecurringTasksTableAdapter
                                recurringAdapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                                recurringTable = recurringAdapter.GetRecurringTasks(CStr(.ROOTTASKITEMSEQID), Nothing)
                                If recurringTable IsNot Nothing AndAlso recurringTable.Rows.Count > 0 Then
                                    'Dim recurringDates As New StringBuilder
                                    recurringRecord = New System.Collections.Generic.List(Of RecurringTasks)
                                    For Each recurringItem As TaskDetail.RecurringTasksRow In recurringTable.Rows
                                        If recurringItem.IsDUEDATENull = False Then
                                            recurringRecord.Add(New RecurringTasks(IP.Bids.SharedFunctions.CDateFromEnglishDate(recurringItem.DUEDATE).ToShortDateString, CInt(recurringItem.TASKITEMSEQID), CInt(recurringItem.ROOTTASKITEMSEQID), CInt(recurringItem.STATUSSEQID)))
                                        End If
                                    Next
                                End If
                            End If
                            If .IsCLOSEDDATENull Then
                                .CLOSEDDATE = String.Empty
                            Else
                                .CLOSEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CLOSEDDATE).ToShortDateString
                            End If
                            If .IsCREATEDBYNull Then
                                .CREATEDBY = String.Empty
                            End If
                            If .IsCREATEDDATENull Then
                                .CREATEDDATE = Now.ToShortDateString 'String.Empty
                            Else
                                .CREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CREATEDDATE).ToShortDateString
                            End If
                            If .IsDATECRITICALNull Then
                                .DATECRITICAL = "N"
                            End If
                            If .IsDESCRIPTIONNull Then
                                .DESCRIPTION = String.Empty
                            End If
                            'If .IsDUEDATENull Then
                            '.DUEDATE = CDate("12/31/2099")
                            '.DUEDATE = CDate(String.Empty)
                            'End If
                            If .IsLASTUPDATEDATENull Then
                                .LASTUPDATEDATE = .CREATEDDATE 'String.Empty
                            Else
                                .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
                            End If
                            If .IsLASTUPDATEUSERNAMENull Then
                                .LASTUPDATEUSERNAME = String.Empty
                            End If
                            If .IsLEADTIMENull Then
                                .LEADTIME = CStr(0)
                            End If
                            If .IsPRIORITYNull Then
                                .PRIORITY = String.Empty
                            End If
                            If .IsRESPONSIBLENAMENull Then
                                .RESPONSIBLEUSERNAME = String.Empty
                            End If
                            If .IsRESPONSIBLEROLESEQIDNull Then
                                .RESPONSIBLEROLESEQID = CStr(-1)
                            End If
                            If .IsRESPONSIBLEUSERNAMENull Then
                                .RESPONSIBLEUSERNAME = String.Empty
                            End If
                            If .IsROLENAMENull Then
                                .ROLENAME = String.Empty
                            End If
                            If .IsROOTTASKITEMSEQIDNull Then
                                .ROOTTASKITEMSEQID = CDec(.TASKITEMSEQID)
                            End If
                            If .IsSTATUSNAMENull Then
                                .STATUSNAME = String.Empty
                            End If
                            If .IsSTATUSSEQIDNull Then
                                .STATUSSEQID = CStr(-1)
                            End If
                            If .IsROLEDESCRIPTIONNull Then
                                .ROLEDESCRIPTION = String.Empty
                            End If
                            If .IsTASKHEADERSEQIDNull Then
                                Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
                            End If
                            If .IsTITLENull Then
                                .TITLE = String.Empty
                            End If
                            If .IsRESPROLEPLANTCODENull Then
                                .RESPROLEPLANTCODE = "9998"
                            End If
                            If .IsRESPROLESITENAMENull Then
                                .RESPROLESITENAME = String.Empty
                            End If
                            If .IsDAYSBEFORENull Then
                                .DAYSBEFORE = CStr(-1)
                            End If
                            If .IsDEPENDENTTASKSEQIDNull Then
                                .DEPENDENTTASKSEQID = String.Empty
                            End If
                            If .IsDEPENDENTCHILDIDNull Then
                                .DEPENDENTCHILDID = 0
                            End If
                            If .IsDUEDATENull = False Then
                                'tempduedate = CStr(.DUEDATE)
                                tempduedate = IP.Bids.SharedFunctions.CDateFromEnglishDate(.DUEDATE).ToShortDateString
                            End If
                            If .IsDAYSAFTERNull Then
                                .DAYSAFTER = CStr(-1)
                            End If
                            If .IsWORKORDERNull Then
                                .WORKORDER = String.Empty
                            End If
                            If .IsCREATEDBYUSERNAMENull Then
                                .CREATEDBYUSERNAME = String.Empty
                            End If
                            record = New TaskItem(.CLOSEDDATE, .CREATEDBY, .CREATEDDATE, .DATECRITICAL, .DESCRIPTION, tempduedate, .LASTUPDATEDATE, .LASTUPDATEUSERNAME, CInt(.LEADTIME), .PRIORITY, .RESPONSIBLENAME, CInt(.RESPONSIBLEROLESEQID), .RESPONSIBLEUSERNAME, .ROLENAME, CStr(.ROOTTASKITEMSEQID), CInt(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TITLE, .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME, recurringRecord, CInt(.DAYSBEFORE), .DEPENDENTTASKSEQID, CStr(.DEPENDENTCHILDID), CInt(.DAYSAFTER), .WORKORDER, .SORTORDER, .ESTIMATEDCOST, .ACTUALCOST, .TANKINSPECTIONTYPEID, .ORIGINALROLESEQID, .CREATEDBYUSERNAME)
                            recurringRecord = Nothing
                        End With
                    Next
                End If
            End If
        Catch
            Throw
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
        adapter = Nothing
    End Function

    Public Function GetOutageTemplateTaskItemList(ByVal taskHeaderNumber As Integer) As System.Collections.Generic.List(Of TaskItem)
        Dim adapter As TaskDetailTableAdapters.TaskItemTableAdapter
        Dim recurringAdapter As TaskDetailTableAdapters.RecurringTasksTableAdapter
        Dim table As TaskDetail.TaskItemDataTable = Nothing
        Dim recurringTable As TaskDetail.RecurringTasksDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim record As System.Collections.Generic.List(Of TaskItem) = Nothing
        Dim recurringRecord As System.Collections.Generic.List(Of RecurringTasks) = Nothing

        Dim tempduedate As String = String.Empty

        Try
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.TaskItemTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                record = CType(GetDataFromCache("GetOutageTemplateTaskItemList_" & taskHeaderNumber, 0.5), System.Collections.Generic.List(Of TaskItem))
                If record Is Nothing OrElse record.Count = 0 Then
                    table = adapter.GetOutageTemplateTaskItemList(CStr(taskHeaderNumber), emptyCursor)
                    If table IsNot Nothing Then
                        record = New System.Collections.Generic.List(Of TaskItem)
                        For Each rowItem As TaskDetail.TaskItemRow In table.Rows
                            With rowItem
                                'recurringTable=adapter.get
                                If .IsDEPENDENTTASKSEQIDNull Then
                                    .DEPENDENTTASKSEQID = String.Empty
                                End If
                                If .IsDEPENDENTCHILDIDNull Then
                                    .DEPENDENTCHILDID = 0
                                End If
                                If (.IsROOTTASKITEMSEQIDNull OrElse .ROOTTASKITEMSEQID = CDec(.TASKITEMSEQID)) And (.DEPENDENTTASKSEQID.Length = 0 Or .DEPENDENTTASKSEQID = .TASKITEMSEQID.ToString) Then '= False AndAlso .ROOTTASKITEMSEQID = -1 Then

                                    recurringAdapter = New TaskDetailTableAdapters.RecurringTasksTableAdapter
                                    recurringAdapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                                    recurringTable = recurringAdapter.GetRecurringTasks(CStr(.TASKITEMSEQID), Nothing)
                                    If recurringTable IsNot Nothing AndAlso recurringTable.Rows.Count > 0 Then
                                        'Dim recurringDates As New StringBuilder
                                        recurringRecord = New System.Collections.Generic.List(Of RecurringTasks)
                                        For Each recurringItem As TaskDetail.RecurringTasksRow In recurringTable.Rows
                                            If recurringItem.IsDUEDATENull = False Then
                                                'If recurringDates.Length > 0 Then
                                                '    recurringDates.Append(",")
                                                'End If
                                                'recurringDates.Append(recurringItem.DUEDATE)
                                                recurringRecord.Add(New RecurringTasks(IP.Bids.SharedFunctions.CDateFromEnglishDate(recurringItem.DUEDATE).ToShortDateString, CInt(recurringItem.TASKITEMSEQID), CInt(recurringItem.ROOTTASKITEMSEQID), CInt(recurringItem.STATUSSEQID)))
                                            End If
                                        Next
                                    End If
                                    If .IsDUEDATENull = False Then
                                        .DUEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.DUEDATE).ToShortDateString
                                    End If
                                    If .IsCLOSEDDATENull Then
                                        .CLOSEDDATE = String.Empty
                                    Else
                                        .CLOSEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CLOSEDDATE).ToShortDateString
                                    End If
                                    If .IsCREATEDBYNull Then
                                        .CREATEDBY = String.Empty
                                    End If
                                    If .IsCREATEDDATENull Then
                                        .CREATEDDATE = Now.ToShortDateString
                                    Else
                                        .CREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CREATEDDATE).ToShortDateString
                                    End If
                                    If .IsDATECRITICALNull Then
                                        .DATECRITICAL = "N"
                                    End If
                                    If .IsDESCRIPTIONNull Then
                                        .DESCRIPTION = String.Empty
                                    End If
                                    'If .IsDUEDATENull Then
                                    '    .DUEDATE = CDate(String.Empty)
                                    'End If
                                    If .IsLASTUPDATEDATENull Then
                                        .LASTUPDATEDATE = .CREATEDDATE ' String.Empty
                                    Else
                                        .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
                                    End If
                                    If .IsLASTUPDATEUSERNAMENull Then
                                        .LASTUPDATEUSERNAME = String.Empty
                                    End If
                                    If .IsLEADTIMENull Then
                                        .LEADTIME = CStr(0)
                                    End If
                                    If .IsPRIORITYNull Then
                                        .PRIORITY = String.Empty
                                    End If
                                    If .IsRESPONSIBLENAMENull Then
                                        .RESPONSIBLEUSERNAME = String.Empty
                                    End If
                                    If .IsRESPONSIBLEROLESEQIDNull Then
                                        .RESPONSIBLEROLESEQID = CStr(-1)
                                    End If
                                    If .IsRESPONSIBLEUSERNAMENull Then
                                        .RESPONSIBLEUSERNAME = String.Empty
                                    End If
                                    If .IsROLENAMENull Then
                                        .ROLENAME = String.Empty
                                    End If
                                    If .IsROOTTASKITEMSEQIDNull Then
                                        .ROOTTASKITEMSEQID = -1
                                    End If
                                    If .IsSTATUSNAMENull Then
                                        .STATUSNAME = String.Empty
                                    End If
                                    If .IsSTATUSSEQIDNull Then
                                        .STATUSSEQID = CStr(-1)
                                    End If
                                    If .IsROLEDESCRIPTIONNull Then
                                        .ROLEDESCRIPTION = String.Empty
                                    End If
                                    If .IsTASKHEADERSEQIDNull Then
                                        Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
                                    End If
                                    If .IsTITLENull Then
                                        .TITLE = String.Empty
                                    End If
                                    If .IsRESPROLEPLANTCODENull Then
                                        .RESPROLEPLANTCODE = String.Empty
                                    End If
                                    If .IsRESPROLESITENAMENull Then
                                        .RESPROLESITENAME = String.Empty
                                    End If
                                    If .IsDAYSBEFORENull Then
                                        .DAYSBEFORE = CStr(-1)
                                    End If
                                    If .IsDEPENDENTTASKSEQIDNull Then
                                        .DEPENDENTTASKSEQID = String.Empty
                                    End If
                                    If .IsDEPENDENTCHILDIDNull Then
                                        .DEPENDENTCHILDID = 0
                                    End If
                                    If .IsDUEDATENull = False Then
                                        tempduedate = .DUEDATE
                                    End If
                                    If .IsDAYSAFTERNull Then
                                        .DAYSAFTER = CStr(-1)
                                    End If
                                    If .IsWORKORDERNull Then
                                        .WORKORDER = String.Empty
                                    End If
                                    If .IsCREATEDBYUSERNAMENull Then
                                        .CREATEDBYUSERNAME = String.Empty
                                    End If
                                    record.Add(New TaskItem(.CLOSEDDATE, .CREATEDBY, .CREATEDDATE, .DATECRITICAL, .DESCRIPTION, tempduedate, .LASTUPDATEDATE, .LASTUPDATEUSERNAME, CInt(.LEADTIME), .PRIORITY, .RESPONSIBLENAME, CInt(.RESPONSIBLEROLESEQID), .RESPONSIBLEUSERNAME, .ROLENAME, CStr(.ROOTTASKITEMSEQID), CInt(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TITLE, .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME, recurringRecord, CInt(.DAYSBEFORE), CStr(.DEPENDENTTASKSEQID), CStr(.DEPENDENTCHILDID), CInt(.DAYSAFTER), .WORKORDER, .SORTORDER, .ESTIMATEDCOST, .ACTUALCOST, .TANKINSPECTIONTYPEID, .ORIGINALROLESEQID, .CREATEDBYUSERNAME))
                                    recurringRecord = Nothing
                                    'End If
                                End If
                            End With
                        Next
                        'InsertDataIntoCache("GetRootTaskItemList_" & taskHeaderNumber, 0.5, record)
                    End If
                End If
            End If
        Catch
            Throw
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
        adapter = Nothing
    End Function
    Public Function SaveOutageTemplateTaskItem(ByVal item As IP.MEAS.BO.TaskItem) As System.Collections.Generic.List(Of IP.MEAS.BO.TaskItem)
        Dim adapter As TaskDetailTableAdapters.TaskItemTableAdapter
        Dim table As TaskDetail.TaskItemDataTable = Nothing
        Dim record As System.Collections.Generic.List(Of IP.MEAS.BO.TaskItem) = Nothing
        Dim out_status As Decimal
        Dim out_taskitem As Decimal
        Dim tempduedate As String = String.Empty
        Try

            adapter = New TaskDetailTableAdapters.TaskItemTableAdapter
            adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            If item Is Nothing Then
                Return Nothing
                Exit Function
            End If
            With item
                Dim taskID As String = CStr(.TaskItemSeqId)
                If taskID = "-1" Then
                    taskID = String.Empty
                End If
                HelperDal.DeleteFromCache("GetRootTaskItemList_" & item.TaskHeaderSeqId)
                'adapter.UpdateTaskItem(taskID, CStr(.TaskHeaderSeqId), tempduedate, .ClosedDate, .Title, .LastUpdateUserName.ToUpper, .Description, .Priority, .DateCritical, CStr(.StatusSeqId), CStr(.LeadTime), .ResponsibleUserName.ToUpper, CStr(.ResponsibleRoleSeqId), .ResponsibleRolePlantCode, .UpdateFlag, .Comments, CType(out_taskitem, Global.System.Nullable(Of Decimal)), CType(out_status, Global.System.Nullable(Of Decimal)))
                adapter.UpdateTaskItem(taskID, CStr(.TaskHeaderSeqId), tempduedate, tempduedate, .Title, .LastUpdateUserName.ToUpper, .Description, .Priority, tempduedate, "1", CStr(.LeadTime), .ResponsibleUserName.ToUpper, CStr(.ResponsibleRoleSeqId), .ResponsibleRolePlantCode, .UpdateFlag, .Comments, CStr(.DaysBefore), CStr(.DaysAfter), .WorkOrder, .SortOrder, .EstimatedCost, .ActualCost, .TankInspectionId, .OriginalRoleSeqId, CType(out_taskitem, Global.System.Nullable(Of Decimal)), CType(out_status, Global.System.Nullable(Of Decimal)))
                If taskID.Length = 0 Then
                    table = adapter.GetLastCreatedTaskItem(.LastUpdateUserName, Nothing)
                Else
                    table = adapter.GetOutageTemplateTaskItem(taskID, Nothing)
                End If
            End With
            If table IsNot Nothing And table.Rows.Count > 0 Then
                record = New System.Collections.Generic.List(Of TaskItem)
                For Each rowItem As TaskDetail.TaskItemRow In table.Rows
                    With rowItem
                        If .IsDUEDATENull = False Then
                            .DUEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.DUEDATE).ToShortDateString
                        End If

                        If .IsCLOSEDDATENull Then
                            .CLOSEDDATE = String.Empty
                        Else
                            .CLOSEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CLOSEDDATE).ToShortDateString
                        End If
                        If .IsCREATEDBYNull Then
                            .CREATEDBY = String.Empty
                        End If
                        If .IsCREATEDDATENull Then
                            .CREATEDDATE = Now.ToShortDateString 'String.Empty
                        Else
                            .CREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CREATEDDATE).ToShortDateString
                        End If
                        If .IsDATECRITICALNull Then
                            .DATECRITICAL = "N"
                        End If
                        If .IsDESCRIPTIONNull Then
                            .DESCRIPTION = String.Empty
                        End If
                        'If .IsDUEDATENull Then
                        '.DUEDATE = CDate(String.Empty)
                        'End If
                        If .IsLASTUPDATEDATENull Then
                            .LASTUPDATEDATE = .CREATEDDATE ' String.Empty
                        Else
                            .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
                        End If
                        If .IsLASTUPDATEUSERNAMENull Then
                            .LASTUPDATEUSERNAME = String.Empty
                        End If
                        If .IsLEADTIMENull Then
                            .LEADTIME = CStr(0)
                        End If
                        If .IsPRIORITYNull Then
                            .PRIORITY = String.Empty
                        End If
                        If .IsRESPONSIBLENAMENull Then
                            .RESPONSIBLEUSERNAME = String.Empty
                        End If
                        If .IsRESPONSIBLEROLESEQIDNull Then
                            .RESPONSIBLEROLESEQID = CStr(-1)
                        End If
                        If .IsRESPONSIBLEUSERNAMENull Then
                            .RESPONSIBLEUSERNAME = String.Empty
                        End If
                        If .IsROLENAMENull Then
                            .ROLENAME = String.Empty
                        End If
                        If .IsROOTTASKITEMSEQIDNull Then
                            .ROOTTASKITEMSEQID = -1
                        End If
                        If .IsSTATUSNAMENull Then
                            .STATUSNAME = String.Empty
                        End If
                        If .IsSTATUSSEQIDNull Then
                            .STATUSSEQID = CStr(-1)
                        End If
                        If .IsTASKHEADERSEQIDNull Then
                            Throw New StrongTypingException("This Task Item [" & .TASKITEMSEQID & "] is missing the Task Header number that goes with it")
                        End If
                        If .IsTITLENull Then
                            .TITLE = String.Empty
                        End If
                        If .IsROLEDESCRIPTIONNull Then
                            .ROLEDESCRIPTION = String.Empty
                        End If
                        If .IsRESPROLEPLANTCODENull Then
                            .RESPROLEPLANTCODE = "9998"
                        End If
                        If .IsRESPROLESITENAMENull Then
                            .RESPROLESITENAME = String.Empty
                        End If
                        If .IsDAYSBEFORENull Then
                            .DAYSBEFORE = CStr(-1)
                        End If
                        If .IsDEPENDENTTASKSEQIDNull Then
                            .DEPENDENTTASKSEQID = String.Empty
                        End If
                        If .IsDEPENDENTCHILDIDNull Then
                            .DEPENDENTCHILDID = 0 'String.Empty
                        End If
                        If .IsDUEDATENull = False Then
                            tempduedate = IP.Bids.SharedFunctions.CDateFromEnglishDate(.DUEDATE).ToShortDateString
                        End If
                        If .IsDAYSAFTERNull Then
                            .DAYSAFTER = CStr(-1)
                        End If
                        If .IsWORKORDERNull Then
                            .WORKORDER = String.Empty
                        End If
                        If .IsCREATEDBYUSERNAMENull Then
                            .CREATEDBYUSERNAME = String.Empty
                        End If
                        record.Add(New IP.MEAS.BO.TaskItem(.CLOSEDDATE, .CREATEDBY, .CREATEDDATE, .DATECRITICAL, .DESCRIPTION, tempduedate, .LASTUPDATEDATE, .LASTUPDATEUSERNAME, CInt(.LEADTIME), .PRIORITY, .RESPONSIBLENAME, CInt(.RESPONSIBLEROLESEQID), .RESPONSIBLEUSERNAME, .ROLENAME, CStr(.ROOTTASKITEMSEQID), CInt(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TITLE, .RESPROLEPLANTCODE, .ROLEDESCRIPTION, .RESPROLESITENAME, CInt(.DAYSBEFORE), CStr(.DEPENDENTTASKSEQID), CStr(.DEPENDENTCHILDID), CInt(.DAYSAFTER), .WORKORDER, CStr(.SORTORDER), CStr(.ESTIMATEDCOST), CStr(.ACTUALCOST), CStr(.TANKINSPECTIONTYPEID), CStr(.ORIGINALROLESEQID), .CREATEDBYUSERNAME))
                    End With
                Next
            End If
        Catch ex As Exception
            If item IsNot Nothing Then
                IP.Bids.SharedFunctions.HandleError("SaveTaskItem", "Error while saving Task Item [" & item.TaskItemSeqId & "]", ex)
            Else
                IP.Bids.SharedFunctions.HandleError("SaveTaskItem", "TaskItem Record is null", ex)
            End If
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
    End Function

    Public Sub CopyTask(ByVal userName As String, ByVal taskHeader As Integer, ByVal taskNumber As String, ByVal listOfResponsibles As List(Of ResponsibleUserForCopy))
        Dim instanceOfTaskTrackerDal As New IP.TaskTrackerDAL.TaskDetailTableAdapters.TaskItemTableAdapter
        Dim outStatus As Decimal?
        Dim outTaskNumber As Decimal?
        instanceOfTaskTrackerDal.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
        For Each task In listOfResponsibles
            Dim newTaskNumber = instanceOfTaskTrackerDal.CopyTask(IN_USERNAME:=userName, IN_TASKNBR:=taskNumber, IN_RESPONSIBLEUSERNAME:=task.ResponsibleUserName, IN_RESPONSIBLEROLESEQID:=task.ResponsibleUserRoleId, IN_RESPROLEPLANTCODE:=task.PlantCode, IN_DUEDATE:=IP.Bids.SharedFunctions.FormatDateTimeToEnglish(task.DueDate), IN_DATECRITICAL:=CType(IIf(task.DateIsCritical = True, "Y", "N"), String), OUT_TASKITEMSEQID:=outTaskNumber, OUT_STATUS:=outStatus)
            If outStatus <> 0 Then
                Throw New DataException(String.Format("Error! Unable to Copy Task {0} for {1}", taskNumber, task.ToString))
            End If
            If outTaskNumber IsNot Nothing AndAlso IsNumeric(outTaskNumber) Then
                EmailDataBll.GetAndSendImmediateEmail(CInt(outTaskNumber), taskHeader)
            End If
            outStatus = 0
        Next
    End Sub
End Class
