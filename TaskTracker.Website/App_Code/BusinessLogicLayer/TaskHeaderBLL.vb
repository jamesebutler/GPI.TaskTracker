'***********************************************************************
' Assembly         :http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          :09-08-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On :09-08-2010
' Description      :Contains the business logic needed to maintain Header Records
'
' Copyright        :(c) International Paper. All rights reserved.
' Purpose          :The purpose of the Business Logic Layer is to separate the presentation layer from the data access layer.
'***********************************************************************

Option Explicit On
Option Strict On
Imports System
Imports HelperDal
Imports IP.MEAS.BO
Imports Devart.Data.Oracle

''' <summary>
''' Contains the business logic for the Task Header page
''' </summary>
''' 
<Serializable()>
Public Class TaskHeaderBll

#Region "Fields"
    Private _taskHeaderRecord As System.Collections.Generic.List(Of TaskHeaderRecord)
    Private _currentTaskHeaderRecord As TaskHeaderRecord
    Private _taskTypes As System.Collections.Generic.List(Of TaskType)
    Private _businessManagers As System.Collections.Generic.List(Of BusinessManagers)
    Private _taskTypeManagers As System.Collections.Generic.List(Of TypeManagers)
#End Region

#Region "Properties"
    ''' <summary>
    ''' Gets a collection of Task Header records
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TaskHeaderRecord() As System.Collections.Generic.List(Of IP.MEAS.BO.TaskHeaderRecord)
        Get
            Return _taskHeaderRecord
        End Get
    End Property

    ''' <summary>
    ''' Gets a single Task header record
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CurrentTaskHeaderRecord() As IP.MEAS.BO.TaskHeaderRecord 'System.Collections.ObjectModel.Collection(Of TaskHeaderRecord)
        Get
            Return _currentTaskHeaderRecord
        End Get
    End Property

    ''' <summary>
    ''' Gets a collection of Task Types
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TaskTypes() As System.Collections.Generic.List(Of IP.MEAS.BO.TaskType)
        Get
            Return _taskTypes
        End Get
    End Property

    ''' <summary>
    ''' Gets a collection of Business Managers
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property BusinessManagers() As System.Collections.Generic.List(Of IP.MEAS.BO.BusinessManagers)
        Get
            Return _businessManagers
        End Get
    End Property

    ''' <summary>
    ''' Gets a collection of Type Managers
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TaskTypeManagers() As System.Collections.Generic.List(Of IP.MEAS.BO.TypeManagers)
        Get
            Return _taskTypeManagers
        End Get
    End Property
#End Region

#Region "Methods"
    ''' <summary>
    ''' Populates a Task Header record for the specified Header Number
    ''' </summary>
    ''' <param name="taskHeaderNumber"></param>
    ''' <remarks></remarks>
    Private Sub PopulateTaskHeader(ByVal taskHeaderNumber As Integer) 'As System.Collections.Generic.List(Of TaskHeaderRecord)
        Dim adapter As TaskHeaderDALTableAdapters.TaskHeaderTableAdapter
        Dim itemDataTable As TaskHeaderDAL.TaskHeaderDataTable = Nothing
        Dim emptyRecord As Object = Nothing
        Dim headerRecord As System.Collections.Generic.List(Of TaskHeaderRecord) = Nothing
        Try
            If itemDataTable Is Nothing OrElse itemDataTable.Rows.Count = 0 Then
                adapter = New TaskHeaderDALTableAdapters.TaskHeaderTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                itemDataTable = adapter.GetTaskHeaderByNumber(CStr(taskHeaderNumber), emptyRecord)
                If itemDataTable IsNot Nothing Then
                    headerRecord = New System.Collections.Generic.List(Of TaskHeaderRecord)
                    For Each rowItem As TaskHeaderDAL.TaskHeaderRow In itemDataTable.Rows
                        With rowItem
                            'We need to identify and fix any null records
                            If .IsACTIVITYNAMENull Then
                                .ACTIVITYNAME = String.Empty
                            End If
                            If .IsACTIVITYSEQIDNull Then
                                .ACTIVITYSEQID = 0
                            End If
                            If .IsATTACHMENTCOUNTNull Then
                                .ATTACHMENTCOUNT = 0
                            End If
                            If .IsBUSUNIT_AREANull Then
                                .BUSUNIT_AREA = String.Empty
                            End If
                            If .IsCREATEDBYNull Then
                                .CREATEDBY = String.Empty
                            End If
                            If .IsCREATEDDATENull Then
                                .CREATEDDATE = New Date(1901, 1, 1)
                            End If
                            If .IsDESCRIPTIONNull Then
                                .DESCRIPTION = String.Empty
                            End If
                            If .IsDIVISIONNull Then
                                .DESCRIPTION = String.Empty
                            End If
                            If .IsSITEREGIONNull Then
                                .SITEREGION = String.Empty
                            End If
                            If .IsDIVISIONNull Then
                                .DIVISION = String.Empty
                            End If
                            If .IsENDDATENull Then
                                If .IsSTARTDATENull Then
                                    .STARTDATE = .CREATEDDATE.ToShortDateString
                                End If
                                .ENDDATE = .STARTDATE
                                'Else
                                '    .ENDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.ENDDATE).ToShortDateString
                            End If
                            If .IsEXTERNALREFNull Then
                                .EXTERNALREF = String.Empty
                            End If
                            If .IsEXTERNALSOURCENAMENull Then
                                .EXTERNALSOURCENAME = String.Empty
                            End If
                            If .IsEXTERNALSOURCESEQIDNull Then
                                .EXTERNALSOURCESEQID = String.Empty
                            End If
                            If .IsLASTUPDATEDATENull Then
                                .LASTUPDATEDATE = .CREATEDDATE
                                'Else
                                '    .LASTUPDATEDATE = IP.Bids.SharedFunctions.StripOffTime(.LASTUPDATEDATE)
                                '    .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
                            End If
                            If .IsLAST_REPLICATION_DATENull Then
                                .LAST_REPLICATION_DATE = String.Empty
                            Else
                                .LAST_REPLICATION_DATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LAST_REPLICATION_DATE).ToShortDateString
                            End If

                            headerRecord.Add(New TaskHeaderRecord(.BUSUNIT_AREA, .LINE, .SITENAME, .ACTIVITYNAME, IP.Bids.SharedFunctions.CDateFromEnglishDate(.STARTDATE), IP.Bids.SharedFunctions.CDateFromEnglishDate(.ENDDATE), .CREATEDDATE, .LASTUPDATEDATE.ToShortDateString, CInt(.ATTACHMENTCOUNT), .EXTERNALSOURCENAME, CInt(.TASKHEADERSEQID), .TITLE, .EXTERNALREF, .EXTERNALSOURCESEQID, .SECURITYLEVEL, CInt(.ACTIVITYSEQID), .PLANTCODE, .CREATEDBY, .LASTUPDATEUSERNAME, .DESCRIPTION, .SITEREGION, .DIVISION, "", .LAST_REPLICATION_DATE))
                        End With
                    Next
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateTaskHeader", "Error getting Task Header data", ex)
        Finally
            adapter = Nothing
            itemDataTable = Nothing
        End Try
        _taskHeaderRecord = headerRecord
        If headerRecord IsNot Nothing AndAlso headerRecord.Count > 0 Then
            _currentTaskHeaderRecord = headerRecord.Item(0)
        Else
            _currentTaskHeaderRecord = Nothing
        End If
        adapter = Nothing
    End Sub

    ''' <summary>
    ''' Populates a collection of Task Types for the specified Task Header
    ''' </summary>
    ''' <param name="taskHeaderNumber"></param>
    ''' <remarks></remarks>
    Private Sub PopulateTaskTypes(ByVal taskHeaderNumber As Integer) 'As System.Collections.Generic.List(Of TaskHeaderRecord)
        Dim adapter As TaskHeaderDALTableAdapters.TaskHeaderTaskTypesTableAdapter
        Dim itemDataTable As TaskHeaderDAL.TaskHeaderTaskTypesDataTable = Nothing
        Dim emptyRecord As Object = Nothing
        Dim taskTypeRecord As System.Collections.Generic.List(Of TaskType) = Nothing
        Try
            If itemDataTable Is Nothing OrElse itemDataTable.Rows.Count = 0 Then
                adapter = New TaskHeaderDALTableAdapters.TaskHeaderTaskTypesTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                itemDataTable = adapter.GetTaskHeaderTaskTypes(CStr(taskHeaderNumber), emptyRecord)
                If itemDataTable IsNot Nothing Then
                    taskTypeRecord = New System.Collections.Generic.List(Of TaskType)
                    For Each rowItem As TaskHeaderDAL.TaskHeaderTaskTypesRow In itemDataTable.Rows
                        With rowItem
                            taskTypeRecord.Add(New TaskType(.TASKTYPENAME, CInt(.TASKTYPESEQID)))
                        End With
                    Next
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateTaskTypes", "Error getting the Task Types", ex)
        Finally
            adapter = Nothing
            itemDataTable = Nothing
        End Try
        _taskTypes = taskTypeRecord
        adapter = Nothing
    End Sub

    ''' <summary>
    ''' Populates a collection of Type Managers
    ''' </summary>
    ''' <param name="taskHeaderNumber"></param>
    ''' <remarks></remarks>
    Private Sub PopulateTypeManagers(ByVal taskHeaderNumber As Integer) 'As System.Collections.Generic.List(Of TaskHeaderRecord)
        Dim adapter As TaskHeaderDALTableAdapters.TaskHeaderTypeManagersTableAdapter
        Dim itemDataTable As TaskHeaderDAL.TaskHeaderTypeManagersDataTable = Nothing
        Dim emptyRecord As Object = Nothing
        Dim typeManagerRecord As System.Collections.Generic.List(Of TypeManagers) = Nothing
        Try
            If itemDataTable Is Nothing OrElse itemDataTable.Rows.Count = 0 Then
                adapter = New TaskHeaderDALTableAdapters.TaskHeaderTypeManagersTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                itemDataTable = adapter.GetTaskHeaderTypeManagers(CStr(taskHeaderNumber), emptyRecord)
                If itemDataTable IsNot Nothing Then
                    typeManagerRecord = New System.Collections.Generic.List(Of TypeManagers)
                    For Each rowItem As TaskHeaderDAL.TaskHeaderTypeManagersRow In itemDataTable.Rows
                        With rowItem
                            typeManagerRecord.Add(New TypeManagers(.USERNAME, .NAME))
                        End With
                    Next
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateTypeManagers", "Error while populating the Type Managers for [" & taskHeaderNumber & "]", ex)
        Finally
            adapter = Nothing
            itemDataTable = Nothing
        End Try
        _taskTypeManagers = typeManagerRecord
        adapter = Nothing
    End Sub

    ''' <summary>
    ''' Populates a collection of Business Managers for the specified Header Number
    ''' </summary>
    ''' <param name="taskHeaderNumber"></param>
    ''' <remarks></remarks>
    Private Sub PopulateBusinessManagers(ByVal taskHeaderNumber As Integer) 'As System.Collections.Generic.List(Of TaskHeaderRecord)
        Dim headerTA As TaskHeaderDALTableAdapters.TaskHeaderBusinessManagersTableAdapter
        Dim headerDT As TaskHeaderDAL.TaskHeaderBusinessManagersDataTable = Nothing
        Dim rsHeader As Object = Nothing
        Dim businessManagerRecord As System.Collections.Generic.List(Of BusinessManagers) = Nothing
        Try
            If headerDT Is Nothing OrElse headerDT.Rows.Count = 0 Then
                headerTA = New TaskHeaderDALTableAdapters.TaskHeaderBusinessManagersTableAdapter
                headerTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                headerDT = headerTA.GetTaskHeaderBusinessManagers(CStr(taskHeaderNumber), rsHeader)
                If headerDT IsNot Nothing Then
                    businessManagerRecord = New System.Collections.Generic.List(Of BusinessManagers)
                    For Each rowItem As TaskHeaderDAL.TaskHeaderBusinessManagersRow In headerDT.Rows
                        With rowItem
                            businessManagerRecord.Add(New BusinessManagers(.USERNAME, .NAME))
                        End With
                    Next
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateBusinessManagers", "Error while populating the Business Managers for [" & taskHeaderNumber & "]", ex)
        Finally
            headerTA = Nothing
            headerDT = Nothing
        End Try
        _businessManagers = businessManagerRecord
        headerTA = Nothing
    End Sub

    ''' <summary>
    ''' Populates all Task Header records for the specified user
    ''' </summary>
    ''' <param name="userName"></param>
    ''' <remarks></remarks>
    Private Sub PopulateAllTaskHeadersForUser(ByVal userName As String)
        Dim headerTA As TaskHeaderDALTableAdapters.TaskHeaderTableAdapter
        Dim headerDT As TaskHeaderDAL.TaskHeaderDataTable = Nothing
        Dim rsHeader As Object = Nothing
        Dim headerRecord As System.Collections.Generic.List(Of TaskHeaderRecord) = Nothing
        Dim CacheKey As String = String.Format("AllTaskHeadersFor{0}", userName)
        Dim cacheHours As Decimal = CDec(0)
        Dim storeInCache As Boolean
        Try
            If headerDT Is Nothing OrElse headerDT.Rows.Count = 0 Then

                headerRecord = CType(HelperDal.GetDataFromCache(CacheKey, cacheHours), System.Collections.Generic.List(Of TaskHeaderRecord))
                If headerRecord Is Nothing OrElse headerRecord.Count = 0 Then
                    headerTA = New TaskHeaderDALTableAdapters.TaskHeaderTableAdapter
                    headerTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                    headerDT = headerTA.GetTaskHeaderByUserName(userName, rsHeader)
                End If
                If headerDT IsNot Nothing Then
                    storeInCache = True
                    headerRecord = New System.Collections.Generic.List(Of TaskHeaderRecord)
                    For Each rowItem As TaskHeaderDAL.TaskHeaderRow In headerDT.Rows
                        With rowItem
                            'We need to identify and fix any null records
                            If .IsACTIVITYNAMENull Then
                                .ACTIVITYNAME = String.Empty
                            End If
                            If .IsACTIVITYSEQIDNull Then
                                .ACTIVITYSEQID = 0
                            End If
                            If .IsATTACHMENTCOUNTNull Then
                                .ATTACHMENTCOUNT = 0
                            End If
                            If .IsBUSUNIT_AREANull Then
                                .BUSUNIT_AREA = String.Empty
                            End If
                            If .IsCREATEDBYNull Then
                                .CREATEDBY = String.Empty
                            End If
                            If .IsCREATEDDATENull Then
                                .CREATEDDATE = New Date(1901, 1, 1)
                            End If
                            If .IsDESCRIPTIONNull Then
                                .DESCRIPTION = String.Empty
                            End If
                            If .IsDIVISIONNull Then
                                .DESCRIPTION = String.Empty
                            End If
                            If .IsENDDATENull Then
                                If .IsSTARTDATENull Then
                                    .STARTDATE = .CREATEDDATE.ToShortDateString
                                End If
                                .ENDDATE = .CREATEDDATE.ToShortDateString
                            Else
                                .ENDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.ENDDATE).ToShortDateString
                            End If
                            If .IsSTARTDATENull Then
                                .STARTDATE = .CREATEDDATE.ToShortDateString
                            Else
                                .STARTDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.STARTDATE).ToShortDateString
                            End If
                            If .IsEXTERNALREFNull Then
                                .EXTERNALREF = String.Empty
                            End If
                            If .IsEXTERNALSOURCENAMENull Then
                                .EXTERNALSOURCENAME = String.Empty
                            End If
                            If .IsEXTERNALSOURCESEQIDNull Then
                                .EXTERNALSOURCESEQID = String.Empty
                            End If
                            If .IsLASTUPDATEDATENull Then
                                .LASTUPDATEDATE = .CREATEDDATE 'String.Empty
                                'Else
                                '    .LASTUPDATEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LASTUPDATEDATE).ToShortDateString
                            End If
                            If .IsTITLENull Then
                                .TITLE = "Missing"
                            End If
                            If .IsSITEREGIONNull Then
                                .SITEREGION = String.Empty
                            End If
                            If .IsDIVISIONNull Then
                                .DIVISION = String.Empty
                            End If
                            If .IsSECURITYLEVELNull Then
                                .SECURITYLEVEL = String.Empty
                            End If
                            If .IsLAST_REPLICATION_DATENull Then
                                .LAST_REPLICATION_DATE = String.Empty
                            Else
                                .LAST_REPLICATION_DATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.LAST_REPLICATION_DATE).ToShortDateString
                            End If
                            headerRecord.Add(New TaskHeaderRecord(.BUSUNIT_AREA, .LINE, .SITENAME, .ACTIVITYNAME, CDate(.STARTDATE), CDate(.ENDDATE), .CREATEDDATE, .LASTUPDATEDATE.ToShortDateString, CInt(.ATTACHMENTCOUNT), .EXTERNALSOURCENAME, CInt(.TASKHEADERSEQID), .TITLE, .EXTERNALREF, (.EXTERNALSOURCESEQID), .SECURITYLEVEL, CInt(.ACTIVITYSEQID), .PLANTCODE, .CREATEDBY, .LASTUPDATEUSERNAME, .DESCRIPTION, .SITEREGION, .DIVISION, "", .LAST_REPLICATION_DATE))
                        End With
                    Next
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateAllTaskHeadersForUser", "Error while populating the list of Task Headers for the following user [" & userName & "]", ex)
        Finally
            headerTA = Nothing
            headerDT = Nothing
        End Try
        _taskHeaderRecord = headerRecord
        If storeInCache Then HelperDal.InsertDataIntoCache(CacheKey, cacheHours, headerRecord)
        headerTA = Nothing
    End Sub

    ''' <summary>
    ''' Saves a Task Header Record
    ''' </summary>
    ''' <param name="headerRecord"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SaveTaskHeader(ByVal headerRecord As TaskHeaderRecord) As Integer  'As System.Collections.Generic.List(Of TaskHeaderRecord)
        Dim headerTA As TaskHeaderDALTableAdapters.TaskHeaderTableAdapter
        Dim outHeaderNumber As Decimal = -1

        Try
            With headerRecord
                Dim busUnitArea() As String = .BusinessUnitArea.Split("-".ToCharArray, 2) ' .BusinessUnitArea.Split(CChar("-"), 1)
                Dim business As String
                Dim area As String
                Dim lineLineBreak() As String = .Line.Split("-".ToCharArray, 2) '.Split(CChar("-"))
                Dim line As String
                Dim machine As String
                '                Dim outStatus As String = "-1" ' COMMENTED BY CODEIT.RIGHT
                Dim headerNumber As String = CStr(.TaskHeaderSeqID)
                Dim out As Decimal = -1
                If busUnitArea.Length = 2 Then
                    business = busUnitArea(0).Trim
                    area = busUnitArea(1).Trim
                Else
                    business = ""
                    area = ""
                End If

                If lineLineBreak.Length = 2 Then
                    line = lineLineBreak(0).Trim
                    machine = lineLineBreak(1).Trim
                Else
                    line = ""
                    machine = ""
                End If
                If .TaskHeaderSeqID = -1 Then
                    headerNumber = ""
                Else
                    headerNumber = .TaskHeaderSeqID.ToString
                End If
                headerTA = New TaskHeaderDALTableAdapters.TaskHeaderTableAdapter

                headerTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                'headerTA.SaveTaskHeader2(.PlantCode, CStr(.StartDate), CStr(.EndDate), Business, Area, Line, .Title, .TaskTypes, .Description, .LastUpdateUserName, Machine, .SecurityLevel, .ExternalSourceSeqID, .ExternalRef, CStr(.ActivitySeqID), headerNumber, CInt(outHeaderNumber), out)

                headerTA.SaveTaskHeader(.PlantCode, CStr(IP.Bids.SharedFunctions.FormatDateTimeToEnglish(.StartDate)), CStr(IP.Bids.SharedFunctions.FormatDateTimeToEnglish(.EndDate)), business, area, line, .Title, .TaskTypes, .Description, .LastUpdateUserName.ToUpper, machine, .SecurityLevel, .ExternalSourceSeqID, .ExternalRef, CStr(.ActivitySeqID), headerNumber, outHeaderNumber, out)

                'headerTA.SaveTaskHeader(.PlantCode, CStr(.StartDate), CStr(.EndDate), business, area, line, .Title, .TaskTypes, .Description, .LastUpdateUserName.ToUpper, machine, .SecurityLevel, .ExternalSourceSeqID, .ExternalRef, CStr(.ActivitySeqID), headerNumber, outHeaderNumber, out)

            End With
        Catch ex As Exception
            If headerRecord IsNot Nothing Then
                If headerRecord.TaskHeaderSeqID = -1 Then
                    'New Task
                    IP.Bids.SharedFunctions.HandleError("SaveTaskHeader", "Error while attempting to save a new Task Header", ex, "Error while attempting to save a new Task Header")
                Else
                    IP.Bids.SharedFunctions.HandleError("SaveTaskHeader", "Error while saving header number [" & headerRecord.TaskHeaderSeqID & "]", ex, "Error while saving header number [" & headerRecord.TaskHeaderSeqID & "]")
                End If

            Else
                IP.Bids.SharedFunctions.HandleError("SaveTaskHeader", "TaskHeaderRecord is null", ex)
            End If
        Finally
            headerTA = Nothing
        End Try
        headerTA = Nothing
        If IsNumeric(outHeaderNumber) Then
            Return CInt(outHeaderNumber)
        Else
            Return -1
        End If
    End Function

    ''' <summary>
    ''' Saves the task header.	
    ''' </summary>
    ''' <param name="title">The title.</param>
    ''' <param name="extRef">The ext ref.</param>
    ''' <param name="extSource">The ext source.</param>
    ''' <param name="startDate">The start date.</param>
    ''' <param name="endDate">The end date.</param>
    ''' <param name="siteID">The site ID.</param>
    ''' <param name="businessUnit">The business unit.</param>
    ''' <param name="line">The line.</param>
    ''' <param name="description">The description.</param>
    ''' <param name="type">The type.</param>
    ''' <param name="activity">The activity.</param>
    ''' <param name="createdBy">The created by.</param>
    ''' <param name="createdDate">The created date.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SaveTaskHeader(ByVal title As String, ByVal extRef As String, ByVal extSource As String, ByVal startDate As String, ByVal endDate As String, ByVal siteID As String, ByVal businessUnit As String, ByVal line As String, ByVal description As String, ByVal type As String, ByVal activity As String, ByVal createdBy As String, ByVal createdDate As String) As Integer
        Dim headerTA As TaskHeaderDALTableAdapters.TaskHeaderTableAdapter
        Dim outHeaderNumber As Decimal = -1
        Dim startDateFormatted As DateTime
        Dim createdDateFormatted As DateTime
        Dim endDateFormatted As DateTime
        Dim outStatus As String = "0"

        Try

            If IsDate(startDate) Then
                startDateFormatted = CDate(FormatDateTime(CDate(startDate), DateFormat.ShortDate))
            Else
                startDateFormatted = CDate(FormatDateTime(Now(), DateFormat.ShortDate))
            End If
            If IsDate(endDate) Then
                endDateFormatted = CDate(FormatDateTime(CDate(endDate), DateFormat.ShortDate))
            Else
                endDateFormatted = CDate(FormatDateTime(Now(), DateFormat.ShortDate))
            End If
            If IsDate(createdDate) Then
                createdDateFormatted = CDate(FormatDateTime(CDate(createdDate), DateFormat.ShortDate))
            Else
                createdDateFormatted = CDate(FormatDateTime(Now(), DateFormat.ShortDate))
            End If

            headerTA = New TaskHeaderDALTableAdapters.TaskHeaderTableAdapter
            headerTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            headerTA.CreateTaskHeader(title, extRef, extSource, startDateFormatted, endDateFormatted, siteID, businessUnit, line, description, type, activity, createdBy, createdDateFormatted, CType(outHeaderNumber, Global.System.Nullable(Of Decimal)), CType(outStatus, Global.System.Nullable(Of Decimal)))
            Dim CacheKey As String = String.Format("AllTaskHeadersFor{0}", createdBy)
            HelperDal.DeleteFromCache(CacheKey)
            'If IsNumeric(outHeaderNumber) AndAlso outheadernumber <> -1 Then
            '    'outheadernumber = 1
            'End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("SaveTaskHeader", "Error while saving header  [" & title & "]", ex)
        Finally
            headerTA = Nothing
        End Try
        headerTA = Nothing
        If IsNumeric(outHeaderNumber) Then
            Return CInt(outHeaderNumber)
        Else
            Return -1
        End If

    End Function

    ''' <summary>
    ''' Saves the task header.	
    ''' </summary>
    ''' <param name="title">The title.</param>
    ''' <param name="extRef">The ext ref.</param>
    ''' <param name="extSource">The ext source.</param>
    ''' <param name="startDate">The start date.</param>
    ''' <param name="endDate">The end date.</param>
    ''' <param name="siteID">The site ID.</param>
    ''' <param name="businessUnit">The business unit.</param>
    ''' <param name="line">The line.</param>
    ''' <param name="description">The description.</param>
    ''' <param name="type">The type.</param>
    ''' <param name="activity">The activity.</param>
    ''' <param name="createdBy">The created by.</param>
    ''' <param name="createdDate">The created date.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SaveMttTaskHeader(ByVal title As String, ByVal extRef As String, ByVal extSource As String, ByVal startDate As String, ByVal endDate As String, ByVal siteID As String, ByVal businessUnit As String, ByVal line As String, ByVal description As String, ByVal type As String, ByVal activity As String, ByVal createdBy As String, ByVal createdDate As String) As Integer
        Dim headerTA As TaskHeaderDALTableAdapters.QueriesTableAdapter
        Dim outHeaderNumber As Decimal = -1
        Dim startDateFormatted As DateTime
        Dim createdDateFormatted As DateTime
        Dim endDateFormatted As DateTime
        Dim outStatus As Decimal = -1

        Try

            If IsDate(startDate) Then
                startDateFormatted = CDate(FormatDateTime(CDate(startDate), DateFormat.ShortDate))
            Else
                startDateFormatted = CDate(FormatDateTime(Now(), DateFormat.ShortDate))
            End If
            If IsDate(endDate) Then
                endDateFormatted = CDate(FormatDateTime(CDate(endDate), DateFormat.ShortDate))
            Else
                endDateFormatted = CDate(FormatDateTime(Now(), DateFormat.ShortDate))
            End If
            If IsDate(createdDate) Then
                createdDateFormatted = CDate(FormatDateTime(CDate(createdDate), DateFormat.ShortDate))
            Else
                createdDateFormatted = CDate(FormatDateTime(Now(), DateFormat.ShortDate))
            End If

            If description.Length = 0 Then
                description = Nothing
            End If
            headerTA = New TaskHeaderDALTableAdapters.QueriesTableAdapter
            'headerTA.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            'headerTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)

            headerTA.CreateMTTTaskHeader(title, extRef, extSource, startDateFormatted, endDateFormatted, siteID, businessUnit, line, description, type, activity, createdBy, createdDateFormatted, outHeaderNumber, outStatus)


        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("SaveMTTTaskHeader", "Error while saving header  [" & title & "]", ex)
        Finally
            headerTA = Nothing
        End Try
        headerTA = Nothing
        Return CInt(outHeaderNumber)
    End Function

    ''' <summary>
    ''' Saves the task header.	
    ''' </summary>
    ''' <param name="title">The title.</param>
    ''' <param name="extRef">The ext ref.</param>
    ''' <param name="extSource">The ext source.</param>
    ''' <param name="startDate">The start date.</param>
    ''' <param name="endDate">The end date.</param>
    ''' <param name="siteID">The site ID.</param>
    ''' <param name="businessUnit">The business unit.</param>
    ''' <param name="line">The line.</param>
    ''' <param name="description">The description.</param>
    ''' <param name="type">The type.</param>
    ''' <param name="activity">The activity.</param>
    ''' <param name="createdBy">The created by.</param>
    ''' <param name="createdDate">The created date.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SaveIrisTaskHeader(ByVal title As String, ByVal taskHeaderNumber As String, ByVal extRef As String, ByVal extSource As String, ByVal startDate As String, ByVal endDate As String, ByVal siteID As String, ByVal businessUnit As String, ByVal line As String, ByVal description As String, ByVal type As String, ByVal activity As String, ByVal createdBy As String, ByVal createdDate As String) As Integer
        Dim headerTA As TaskHeaderDALTableAdapters.QueriesTableAdapter
        Dim outHeaderNumber As Decimal = -1
        Dim startDateFormatted As DateTime
        Dim createdDateFormatted As DateTime
        Dim endDateFormatted As DateTime
        Dim outStatus As Decimal = -1

        Try

            If IsDate(startDate) Then
                startDateFormatted = CDate(FormatDateTime(CDate(startDate), DateFormat.ShortDate))
            Else
                startDateFormatted = CDate(FormatDateTime(Now(), DateFormat.ShortDate))
            End If
            If IsDate(endDate) Then
                endDateFormatted = CDate(FormatDateTime(CDate(endDate), DateFormat.ShortDate))
            Else
                endDateFormatted = CDate(FormatDateTime(Now(), DateFormat.ShortDate))
            End If
            If IsDate(createdDate) Then
                createdDateFormatted = CDate(FormatDateTime(CDate(createdDate), DateFormat.ShortDate))
            Else
                createdDateFormatted = CDate(FormatDateTime(Now(), DateFormat.ShortDate))
            End If

            headerTA = New TaskHeaderDALTableAdapters.QueriesTableAdapter
            'headerTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            If description.Length = 0 Then
                description = Nothing
            End If
            headerTA.CreateIRISTaskHeader(title, extRef, extSource, startDateFormatted, endDateFormatted, siteID, businessUnit, line, description, type, activity, createdBy, createdDateFormatted, CType(taskHeaderNumber, Global.System.Nullable(Of Decimal)), outHeaderNumber, outStatus)

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("SaveIRISTaskHeader", "Error while saving header number [" & title & "]", ex)
        Finally
            headerTA = Nothing
        End Try
        headerTA = Nothing
        Return CInt(outHeaderNumber)

    End Function

    ''' <summary>
    ''' Deletes the specified Task Header Record
    ''' </summary>   
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteTaskHeader(ByVal headerNumber As String, ByVal userName As String) As Boolean
        Dim headerTA As New TaskHeaderDALTableAdapters.TaskHeaderTableAdapter
        Dim outStatus As String = "0"
        Try
            headerTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            headerTA.DeleteTaskHeader(headerNumber, userName, CType(outStatus, Global.System.Nullable(Of Decimal)))
            If outStatus <> "0" Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("DeleteTaskHeader", "Error while attempting to delete [" & headerNumber & "]", ex)
            Return False
        End Try
    End Function
#End Region

#Region "Constructor(s)"
    Private Sub New()
        'Do nothing - We need to hide this constructor from developers
    End Sub
    Public Sub New(ByVal headerNumber As Integer)
        Try
            PopulateTaskHeader(headerNumber)
            PopulateTaskTypes(headerNumber)
            PopulateBusinessManagers(headerNumber)
            PopulateTypeManagers(headerNumber)
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("TaskHeaderBLL", , ex, "Error accessing new task")
        End Try
    End Sub

    Public Sub New(ByVal username As String)
        Try
            PopulateAllTaskHeadersForUser(username)
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("TaskHeaderBLL", "New - Error attempting to Populate All Task Headers for [" & username & "]", ex)
        End Try
    End Sub
#End Region
End Class
