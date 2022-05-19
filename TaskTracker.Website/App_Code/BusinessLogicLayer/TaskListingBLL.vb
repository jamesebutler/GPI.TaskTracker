'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : mjpope
' Created          : 11-29-2010
'
' Last Modified By : mjpope
' Last Modified On : 06-27-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Option Explicit On
Option Strict On
Imports HelperDal
Imports IP.TaskTrackerDAL
Imports Devart.Data.Oracle
Imports Devart.Data
''' <summary>
''' 
''' </summary>
<Serializable()> _
Public Class TaskListingBll
#Region "Fields"
    Private _PlantCode As String = String.Empty
    Private _BusinessUnit As String = String.Empty
    Private _Area As String = String.Empty
    Private _Line As String = String.Empty
    Private _Machine As String = String.Empty
    Private _Division As String = String.Empty
    Private _Region As String = String.Empty
    Private _CreatedBy As String = String.Empty
    Private _CreatedByPlantCode As String = String.Empty
    Private _Responsible As String = String.Empty
    Private _Role As String = String.Empty
    Private _ResponsiblePlantCode As String = String.Empty
    Private _SourceSystem As String = String.Empty
    Private _SourceSytemRef As String = String.Empty
    Private _SecurityLevel As String = String.Empty
    Private _Title As String = String.Empty
    Private _Description As String = String.Empty
    Private _Type As String = String.Empty
    Private _Activity As String = String.Empty
    Private _TaskStatus As String = String.Empty
    Private _Element As String = String.Empty
    Private mHeaderFromDate As String = String.Empty
    Private mHeaderToDate As String = String.Empty
    Private mDueDateFrom As String = String.Empty
    Private mDueDateTo As String = String.Empty
    Private mClosedDateFrom As String = String.Empty
    Private mClosedDateTo As String = String.Empty
    Private mAndOr As String = String.Empty
    Private mUserName As String = String.Empty
    Private mHeaderSeqId As String = String.Empty
    Private mTaskSearchView As String = String.Empty
#End Region

#Region "Properties"
    Public Property DueDateDateRange As String = String.Empty
    Public Property HeaderDateRange As String = String.Empty
    Public Property CloseDateRange As String = String.Empty

    ''' <summary>
    ''' Gets or sets the plant code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PlantCode() As String
        Get
            Return _PlantCode
        End Get
        Set(ByVal Value As String)
            _PlantCode = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Business Unit
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BusinessUnit() As String
        Get
            Return _BusinessUnit
        End Get
        Set(ByVal value As String)
            _BusinessUnit = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Area value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Area() As String
        Get
            Return _Area
        End Get
        Set(ByVal Value As String)
            _Area = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the line value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Line() As String
        Get
            Return _Line
        End Get
        Set(ByVal value As String)
            _Line = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Machine Value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Machine() As String
        Get
            Return _Machine
        End Get
        Set(ByVal Value As String)
            _Machine = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the division.
    ''' </summary>
    ''' <value>The division.</value>
    Public Property Division() As String
        Get
            Return _Division
        End Get
        Set(ByVal Value As String)
            _Division = Value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Region
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Region() As String
        Get
            Return _Region
        End Get
        Set(ByVal value As String)
            _Region = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Created By
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CreatedBy() As String
        Get
            Return _CreatedBy
        End Get
        Set(ByVal value As String)
            _CreatedBy = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the created by plant code.
    ''' </summary>
    ''' <value>The created by plant code.</value>
    Public Property CreatedByPlantCode() As String
        Get
            Return _CreatedByPlantCode
        End Get
        Set(ByVal value As String)
            _CreatedByPlantCode = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Responsible users
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Responsible() As String
        Get
            Return _Responsible
        End Get
        Set(ByVal value As String)
            _Responsible = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Source System value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SourceSystem() As String
        Get
            Return _SourceSystem
        End Get
        Set(ByVal value As String)
            _SourceSystem = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Source System Reference number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SourceSytemRef() As String
        Get
            Return _SourceSytemRef
        End Get
        Set(ByVal value As String)
            _SourceSytemRef = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the security level
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SecurityLevel() As String
        Get
            Return _SecurityLevel
        End Get
        Set(ByVal value As String)
            _SecurityLevel = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Title
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Title() As String
        Get
            Return _Title
        End Get
        Set(ByVal value As String)
            _Title = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Description
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Type value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Type() As String
        Get
            Return _Type
        End Get
        Set(ByVal value As String)
            _Type = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Activity value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Activity() As String
        Get
            Return _Activity
        End Get
        Set(ByVal value As String)
            _Activity = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Task Status
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TaskStatus() As String
        Get
            Return _TaskStatus
        End Get
        Set(ByVal value As String)
            _TaskStatus = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Header From Date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HeaderFromDate() As String
        Get
            Return mHeaderFromDate
        End Get
        Set(ByVal value As String)
            mHeaderFromDate = value
        End Set
    End Property

    Public Property HeaderToDate() As String
        Get
            Return mHeaderToDate
        End Get
        Set(ByVal value As String)
            mHeaderToDate = value
        End Set
    End Property

    Public Property DueDateFrom() As String
        Get
            Return mDueDateFrom
        End Get
        Set(ByVal value As String)
            mDueDateFrom = value
        End Set
    End Property

    Public Property DueDateTo() As String
        Get
            Return mDueDateTo
        End Get
        Set(ByVal value As String)
            mDueDateTo = value
        End Set
    End Property

    Public Property ClosedDateFrom() As String
        Get
            Return mClosedDateFrom
        End Get
        Set(ByVal value As String)
            mClosedDateFrom = value
        End Set
    End Property

    Public Property ClosedDateTo() As String
        Get
            Return mClosedDateTo
        End Get
        Set(ByVal value As String)
            mClosedDateTo = value
        End Set
    End Property

    Public Property AndOr() As String
        Get
            Return mAndOr
        End Get
        Set(ByVal value As String)
            mAndOr = value
        End Set
    End Property


    Public Property UserName() As String
        Get
            Return mUserName
        End Get
        Set(ByVal value As String)
            mUserName = value
        End Set
    End Property

    Public Property HeaderSeqID() As String
        Get
            Return mHeaderSeqId
        End Get
        Set(ByVal value As String)
            mHeaderSeqId = value
        End Set
    End Property

    Public Property TaskSearchView() As String
        Get
            Return mTaskSearchView
        End Get
        Set(ByVal value As String)
            mTaskSearchView = value
        End Set
    End Property

    Public Property ResponsiblePlantCode() As String
        Get
            Return _ResponsiblePlantCode
        End Get
        Set(ByVal value As String)
            _ResponsiblePlantCode = value
        End Set
    End Property
    Public Property Role() As String
        Get
            Return _Role
        End Get
        Set(ByVal value As String)
            _Role = value
        End Set
    End Property

    Public Property RoleId As String

    Public Property Element() As String
        Get
            Return _Element
        End Get
        Set(ByVal value As String)
            _Element = value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' Gets a collection of tasks
    ''' </summary>    
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetHeaderListing() As System.Collections.Generic.List(Of BO.TaskListing) 'MTTDivisionDataTable
        Dim adapter As TaskListingDSTableAdapters.TaskListingTableAdapter
        Dim listingDT As TaskListingDS.TaskListingDataTable = Nothing
        Dim rsTaskList As Object = Nothing
        Dim taskList As System.Collections.Generic.List(Of BO.TaskListing) = Nothing

        Try
            If taskList Is Nothing OrElse taskList.Count = 0 Then
                adapter = New TaskListingDSTableAdapters.TaskListingTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)

                'MJP add cache support 
                Dim cacheKey As String
                Dim originalSearchView As String
                originalSearchView = Me.TaskSearchView
                Me.TaskSearchView = ""
                Me.mTaskSearchView = ""
                cacheKey = IP.Bids.SharedFunctions.CreateKey(Me)
                Me.TaskSearchView = originalSearchView
                Me.mTaskSearchView = originalSearchView
                listingDT = CType(GetDataFromCache(cacheKey, 0.1), IP.TaskTrackerDAL.TaskListingDS.TaskListingDataTable)
                If listingDT Is Nothing Then
                    listingDT = adapter.GetTaskListing(PlantCode, BusinessUnit, Area, Line, Machine, Division, Region, CreatedBy, Responsible, SourceSystem, SourceSytemRef, SecurityLevel, Title, Description, Type, Activity, TaskStatus, HeaderFromDate, HeaderToDate, DueDateFrom, DueDateTo, ClosedDateFrom, ClosedDateTo, AndOr, UserName, HeaderSeqID, ResponsiblePlantCode, rsTaskList)
                    If listingDT IsNot Nothing AndAlso listingDT.Count > 0 Then
                        InsertDataIntoCache(cacheKey, 0.05, listingDT)
                    End If
                End If
                'MJP End Cache Support

                'listingDT = adapter.GetTaskListing(PlantCode, BusinessUnit, Area, Line, Machine, Division, Region, CreatedBy, Responsible, SourceSystem, SourceSytemRef, SecurityLevel, Title, Description, Type, Activity, TaskStatus, HeaderFromDate, HeaderToDate, DueDateFrom, DueDateTo, ClosedDateFrom, ClosedDateTo, AndOr, UserName, HeaderSeqID, ResponsiblePlantCode, rsTaskList)
                taskList = New System.Collections.Generic.List(Of BO.TaskListing)
                Dim distinctListingDT As New TaskListingDS.TaskListingDataTable
                If listingDT IsNot Nothing AndAlso listingDT.Rows.Count > 0 Then
                    'listingDT.DefaultView.Sort = sortExpression
                    distinctListingDT.Load(listingDT.DefaultView.ToTable(True, New String() {"HEADERCREATEDDATE", "SiteName", "TaskHeaderSeqId", "HeaderTitle", "BUSUNIT_AREA", "Line", "TASKTYPE", "ACTIVITYNAME", "HEADERSTATUS"}).CreateDataReader)
                    'distinctListingDT.Load(listingDT.DefaultView.ToTable().CreateDataReader) 
                End If
                'TODO - This logic doesn't work when we have multiple tasks for a header
                Dim dueDate As String = String.Empty
                If distinctListingDT IsNot Nothing AndAlso distinctListingDT.Rows.Count > 0 Then
                    For Each rowItem As TaskListingDS.TaskListingRow In distinctListingDT.Rows
                        With rowItem
                            If .IsPLANTCODENull Then
                                .PLANTCODE = String.Empty
                            End If
                            If .Is_REGIONNull Then
                                ._REGION = String.Empty
                            End If
                            If .IsACTIVITYNAMENull Then
                                .ACTIVITYNAME = String.Empty
                            End If
                            If .IsACTIVITYSEQIDNull Then
                                .ACTIVITYSEQID = -1
                            End If
                            If .IsATTACHMENTCOUNTNull Then
                                .ATTACHMENTCOUNT = 0
                            End If
                            If .IsBUSUNIT_AREANull Then
                                .BUSUNIT_AREA = String.Empty
                            End If
                            If .IsDIVISIONNull Then
                                .DIVISION = String.Empty
                            End If
                            If .IsDUEDATENull Then
                                dueDate = String.Empty
                            Else
                                dueDate = .DUEDATE
                            End If
                            If .IsENDDATENull Then
                                .ENDDATE = String.Empty
                            End If
                            If .IsEXTERNALREFNull Then
                                .EXTERNALREF = String.Empty
                            End If
                            If .IsEXTERNALSOURCENAMENull Then
                                .EXTERNALSOURCENAME = String.Empty
                            End If
                            If .IsEXTERNALSOURCESEQIDNull Then
                                .EXTERNALSOURCESEQID = 0
                            End If
                            If .IsHEADERCREATEDBYNull Then
                                .HEADERCREATEDBY = String.Empty
                            End If
                            If .IsHEADERCREATEDDATENull Then
                                .HEADERCREATEDDATE = Now
                            End If
                            If .IsHEADERDESCNull Then
                                .HEADERDESC = String.Empty
                            End If
                            If .IsHEADERLASTUPDATEDATENull Then
                                .HEADERLASTUPDATEDATE = Now
                            End If
                            If .IsHEADERLASTUPDATEUSERNAMENull Then
                                .HEADERLASTUPDATEUSERNAME = String.Empty
                            End If
                            If .IsHEADERTITLENull Then
                                .HEADERTITLE = String.Empty
                            End If
                            If .IsITEMCREATEDDATENull Then
                                .ITEMCREATEDDATE = Now
                            End If
                            If .IsITEMDESCNull Then
                                .ITEMDESC = String.Empty
                            End If
                            If .IsITEMLASTUPDATEDATENull Then
                                .ITEMLASTUPDATEDATE = Now
                            End If
                            If .IsITEMTITLENull Then
                                .ITEMTITLE = String.Empty
                            End If
                            If .IsLINENull Then
                                .LINE = String.Empty
                            End If
                            'If .IsPLANTCODENull Then
                            '    .PLANTCODE = String.Empty
                            'End If
                            If .IsSECURITYLEVELNull Then
                                .SECURITYLEVEL = "N"
                            End If
                            If .IsSITENAMENull Then
                                .SITENAME = String.Empty
                            End If
                            If .IsSTARTDATENull Then
                                .STARTDATE = String.Empty
                            End If
                            If .IsSTATUSNAMENull Then
                                .STATUSNAME = String.Empty
                            End If
                            If .IsSTATUSSEQIDNull Then
                                .STATUSSEQID = 0
                            End If
                            If .IsTASKITEMSEQIDNull Then
                                .TASKITEMSEQID = 0
                            End If
                            If .IsTASKTYPENull Then
                                .TASKTYPE = String.Empty
                            End If
                            If .IsHEADERSTATUSNull Then
                                .HEADERSTATUS = String.Empty
                            End If

                            If .IsROLEDESCRIPTIONNull Then
                                .ROLEDESCRIPTION = String.Empty
                            End If

                            If .IsRESPROLESITENAMENull Then
                                .RESPROLESITENAME = String.Empty
                            End If

                            If .IsRESPNAMENull Then
                                .RESPNAME = String.Empty
                            End If
                            If .IsCLOSEDDATENull Then
                                .CLOSEDDATE = String.Empty
                            End If
                            If .IsOPENSUBTASKSNull Then
                                .OPENSUBTASKS = "N"
                            End If
                            If .IsDATECRITICALNull Then
                                .DATECRITICAL = "N"
                            End If
                            If .IsCOMMENTSNull Then
                                .COMMENTS = ""
                            End If
                            If .IsTASKITEMSEQIDNull = False And .DUEDATE.Length > 0 Then
                                taskList.Add(New IP.MEAS.BO.TaskListing(.ACTIVITYNAME, CInt(.ACTIVITYSEQID), CInt(.ATTACHMENTCOUNT), .BUSUNIT_AREA, .DIVISION, dueDate, .ENDDATE, .EXTERNALREF, CStr(.EXTERNALSOURCESEQID), .EXTERNALSOURCENAME, .HEADERCREATEDDATE.ToShortDateString, .HEADERCREATEDBY, CStr(.HEADERDESC), .HEADERLASTUPDATEDATE.ToShortDateString, .HEADERLASTUPDATEUSERNAME, .HEADERTITLE, .ITEMCREATEDDATE.ToShortDateString, .ITEMDESC, .ITEMLASTUPDATEDATE.ToShortDateString, CStr(.ITEMTITLE), .LINE, .PLANTCODE, ._REGION, .SECURITYLEVEL, .SITENAME, .STARTDATE, .STATUSNAME, CStr(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TASKTYPE, .HEADERSTATUS, .ROLEDESCRIPTION, .RESPNAME, .RESPROLESITENAME, .CLOSEDDATE, .DATECRITICAL, .OPENSUBTASKS, .TANKINSPECTIONTYPEID, 1, .COMMENTS, .PARENTDUEDATE, CInt(.PARENTSUBTASKSEQID), CInt(.DEPENDENTTASKSEQID)))
                            Else
                                .DUEDATE = .DUEDATE
                            End If
                        End With
                    Next
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetHeaderListing", "Error while getting the list of Task Headers", ex)
        Finally
            adapter = Nothing
            listingDT = Nothing
        End Try
        Return taskList 'divisionDT
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTaskSearchListing() As TaskListingDS.TaskListingDataTable
        Dim listingDT As TaskListingDS.TaskListingDataTable = Nothing
        Dim adapter As TaskListingDSTableAdapters.TaskListingTableAdapter
        Dim rsTaskList As Object = Nothing

        Try
            adapter = New TaskListingDSTableAdapters.TaskListingTableAdapter
            adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            listingDT = adapter.GetTaskListing(PlantCode, BusinessUnit, Area, Line, Machine, Division, Region, CreatedBy, Responsible, SourceSystem, SourceSytemRef, SecurityLevel, Title, Description, Type, Activity, TaskStatus, HeaderFromDate, HeaderToDate, DueDateFrom, DueDateTo, ClosedDateFrom, ClosedDateTo, AndOr, UserName, HeaderSeqID, ResponsiblePlantCode, rsTaskList)
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetTaskSearchListing", "Error while getting the list of Task Search Results", ex)
        Finally
            GetTaskSearchListing = listingDT
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>    
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetHeaderItemListing(Optional ByVal useCachedData As Boolean = False) As System.Collections.Generic.List(Of BO.TaskListing) 'MTTDivisionDataTable
        Dim adapter As TaskListingDSTableAdapters.TaskListingTableAdapter
        Dim listingDT As TaskListingDS.TaskListingDataTable = Nothing
        Dim rsTaskList As Object = Nothing
        Dim taskList As System.Collections.Generic.List(Of BO.TaskListing) = Nothing

        Try
            If taskList Is Nothing OrElse taskList.Count = 0 Then
                adapter = New TaskListingDSTableAdapters.TaskListingTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)

                'MJP add cache support 
                Dim cacheKey As String
                Dim originalSearchView As String
                originalSearchView = Me.TaskSearchView
                Me.TaskSearchView = ""
                Me.mTaskSearchView = ""
                cacheKey = IP.Bids.SharedFunctions.CreateKey(Me)
                Me.TaskSearchView = originalSearchView
                Me.mTaskSearchView = originalSearchView
                If useCachedData = True Then
                    listingDT = CType(GetDataFromCache(cacheKey, 0.05), TaskListingDS.TaskListingDataTable)
                End If
                If listingDT Is Nothing Then
                    listingDT = adapter.GetTaskListing(PlantCode, BusinessUnit, Area, Line, Machine, Division, Region, CreatedBy, Responsible, SourceSystem, SourceSytemRef, SecurityLevel, Title, Description, Type, Activity, TaskStatus, HeaderFromDate, HeaderToDate, DueDateFrom, DueDateTo, ClosedDateFrom, ClosedDateTo, AndOr, UserName, HeaderSeqID, ResponsiblePlantCode, rsTaskList)
                    If listingDT IsNot Nothing AndAlso listingDT.Count > 0 Then
                        InsertDataIntoCache(cacheKey, 0.05, listingDT)
                    End If
                End If
                'MJP End Cache Support

                'listingDT = adapter.GetTaskListing(PlantCode, BusinessUnit, Area, Line, Machine, Division, Region, CreatedBy, Responsible, SourceSystem, SourceSytemRef, SecurityLevel, Title, Description, Type, Activity, TaskStatus, HeaderFromDate, HeaderToDate, DueDateFrom, DueDateTo, ClosedDateFrom, ClosedDateTo, AndOr, UserName, HeaderSeqID, ResponsiblePlantCode, rsTaskList)
                taskList = New System.Collections.Generic.List(Of BO.TaskListing)
                'If sortExpression.Length = 0 Then
                '    listingDT.DefaultView.Sort = "TASKITEMSEQID"
                'Else
                '    listingDT.DefaultView.Sort = sortExpression
                'End If

                Dim distinctListingDT As New TaskListingDS.TaskListingDataTable
                distinctListingDT.Load(listingDT.DefaultView.ToTable().CreateDataReader) 'True, New String() {"HEADERCREATEDDATE", "SiteName", "TaskHeaderSeqId", "HeaderTitle", "BUSUNIT_AREA", "Line", "TASKTYPE", "ACTIVITYNAME", "HEADERSTATUS"}).CreateDataReader)
                For Each rowItem As TaskListingDS.TaskListingRow In distinctListingDT.Rows 'listingDT.Rows
                    With rowItem
                        If .Is_REGIONNull Then
                            ._REGION = String.Empty
                        End If
                        If .IsACTIVITYNAMENull Then
                            .ACTIVITYNAME = String.Empty
                        End If
                        If .IsACTIVITYSEQIDNull Then
                            .ACTIVITYSEQID = -1
                        End If
                        If .IsATTACHMENTCOUNTNull Then
                            .ATTACHMENTCOUNT = 0
                        End If
                        If .IsBUSUNIT_AREANull Then
                            .BUSUNIT_AREA = String.Empty
                        End If
                        If .IsDIVISIONNull Then
                            .DIVISION = String.Empty
                        End If
                        If .IsDUEDATENull Then
                            .DUEDATE = String.Empty
                        Else
                            .DUEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.DUEDATE).ToShortDateString
                        End If
                        If .IsENDDATENull Then
                            .ENDDATE = String.Empty
                        Else
                            .ENDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.ENDDATE).ToShortDateString
                        End If
                        If .IsEXTERNALREFNull Then
                            .EXTERNALREF = String.Empty
                        End If
                        If .IsEXTERNALSOURCENAMENull Then
                            .EXTERNALSOURCENAME = String.Empty
                        End If
                        If .IsEXTERNALSOURCESEQIDNull Then
                            .EXTERNALSOURCESEQID = 0
                        End If
                        If .IsHEADERCREATEDBYNull Then
                            .HEADERCREATEDBY = String.Empty
                        End If
                        If .IsHEADERCREATEDDATENull Then
                            .HEADERCREATEDDATE = Now
                            'Else
                            '    .HEADERCREATEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.HEADERCREATEDDATE).ToShortDateString
                        End If
                        If .IsHEADERDESCNull Then
                            .HEADERDESC = String.Empty
                        End If
                        If .IsHEADERLASTUPDATEDATENull Then
                            .HEADERLASTUPDATEDATE = Now
                        End If
                        If .IsHEADERLASTUPDATEUSERNAMENull Then
                            .HEADERLASTUPDATEUSERNAME = String.Empty
                        End If
                        If .IsHEADERTITLENull Then
                            .HEADERTITLE = String.Empty
                        End If
                        If .IsITEMCREATEDDATENull Then
                            .ITEMCREATEDDATE = Now
                        End If
                        If .IsITEMDESCNull Then
                            .ITEMDESC = String.Empty
                        End If
                        If .IsITEMLASTUPDATEDATENull Then
                            .ITEMLASTUPDATEDATE = Now
                        End If
                        If .IsITEMTITLENull Then
                            .ITEMTITLE = String.Empty
                        End If
                        If .IsLINENull Then
                            .LINE = String.Empty
                        End If
                        'If .IsPLANTCODENull Then
                        '    .PLANTCODE = String.Empty
                        'End If
                        If .IsSECURITYLEVELNull Then
                            .SECURITYLEVEL = "N"
                        End If
                        If .IsSITENAMENull Then
                            .SITENAME = String.Empty
                        End If
                        If .IsSTARTDATENull Then
                            .STARTDATE = String.Empty
                        Else
                            .STARTDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.STARTDATE).ToShortDateString
                        End If
                        If .IsSTATUSNAMENull Then
                            .STATUSNAME = String.Empty
                        End If
                        If .IsSTATUSSEQIDNull Then
                            .STATUSSEQID = 0
                        End If
                        If .IsTASKITEMSEQIDNull Then
                            .TASKITEMSEQID = 0
                        End If
                        If .IsTASKTYPENull Then
                            .TASKTYPE = String.Empty
                        End If
                        If .IsHEADERSTATUSNull Then
                            .HEADERSTATUS = String.Empty
                        End If
                        If .IsRESPNAMENull Then
                            .RESPNAME = String.Empty
                        End If
                        If .IsRESPROLESITENAMENull Then
                            .RESPROLESITENAME = String.Empty
                        End If
                        If .IsROLEDESCRIPTIONNull Then
                            .ROLEDESCRIPTION = String.Empty
                        End If
                        If .IsCLOSEDDATENull Then
                            .CLOSEDDATE = String.Empty
                        Else
                            .CLOSEDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.CLOSEDDATE).ToShortDateString
                        End If
                        If .IsOPENSUBTASKSNull Then
                            .OPENSUBTASKS = "N"
                        End If
                        If .IsDATECRITICALNull Then
                            .DATECRITICAL = "N"
                        End If
                        If .IsCOMMENTSNull Then
                            .COMMENTS = String.Empty
                        End If
                        If .IsPARENTDUEDATENull Then
                            .PARENTDUEDATE = .DUEDATE
                        End If
                        If .IsDEPENDENTTASKSEQIDNull Then
                            .DEPENDENTTASKSEQID = 0
                        End If
                        If .IsPARENTSUBTASKSEQIDNull Then
                            .PARENTSUBTASKSEQID = 0
                        End If
                        If .DUEDATE.Length > 0 And CStr(.ITEMTITLE).Length > 0 Then
                            Dim currentTaskItem As IP.MEAS.BO.TaskItem
                            Dim taskItemBll As New TaskTrackerItemBll
                            currentTaskItem = taskItemBll.GetTaskItem(CInt(.TASKITEMSEQID))
                            If currentTaskItem.Priority = "" Then currentTaskItem.Priority = CStr(1)
                            Select Case currentTaskItem.Priority.ToLower
                                Case "low"
                                    currentTaskItem.Priority = CStr(1)
                                Case "medium"
                                    currentTaskItem.Priority = CStr(2)
                                Case "high"
                                    currentTaskItem.Priority = CStr(3)
                            End Select
                            If .IsCOMMENTSNull Then
                                .COMMENTS = ""
                            End If
                            If IsNumeric(currentTaskItem.Priority) = False Then currentTaskItem.Priority = CStr(1)
                            taskList.Add(New IP.MEAS.BO.TaskListing(.ACTIVITYNAME, CInt(.ACTIVITYSEQID), CInt(.ATTACHMENTCOUNT), .BUSUNIT_AREA, .DIVISION, .DUEDATE, .ENDDATE, .EXTERNALREF, CStr(.EXTERNALSOURCESEQID), .EXTERNALSOURCENAME, .HEADERCREATEDDATE.ToShortDateString, .HEADERCREATEDBY, CStr(.HEADERDESC), .HEADERLASTUPDATEDATE.ToShortDateString, .HEADERLASTUPDATEUSERNAME, .HEADERTITLE, .ITEMCREATEDDATE.ToShortDateString, .ITEMDESC, .ITEMLASTUPDATEDATE.ToShortDateString, CStr(.ITEMTITLE), .LINE, .PLANTCODE, ._REGION, .SECURITYLEVEL, .SITENAME, .STARTDATE, .STATUSNAME, CStr(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TASKTYPE, .HEADERSTATUS, .ROLEDESCRIPTION, .RESPNAME, .RESPROLESITENAME, .CLOSEDDATE, .DATECRITICAL, .OPENSUBTASKS, .TANKINSPECTIONTYPEID, CInt(currentTaskItem.Priority), .COMMENTS, .PARENTDUEDATE, CInt(.PARENTSUBTASKSEQID), CInt(.DEPENDENTTASKSEQID)))

                        End If
                    End With
                Next
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetHeaderItemListing", "Error getting the Header Item Listing", ex, "Error getting the Header Item Listing")
        Finally
            adapter = Nothing
            listingDT = Nothing
        End Try

       
        Return taskList 'divisionDT
    End Function

    Public Function GetTaskListingDataTable() As System.Data.DataTable
        Try
            Return IP.Bids.SharedFunctions.ListToDataTable(GetHeaderItemListing(True))
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetTaskListingDataTable", , ex)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTaskListingDataTableold() As TaskListingDS.TaskListingDataTable
        Dim adapter As TaskListingDSTableAdapters.TaskListingTableAdapter
        Dim listingDT As TaskListingDS.TaskListingDataTable = Nothing
        Dim rsTaskList As Object = Nothing

        Dim divisionList As System.Collections.Generic.List(Of TaskListing) = Nothing

        Try
            If divisionList Is Nothing OrElse divisionList.Count = 0 Then
                adapter = New TaskListingDSTableAdapters.TaskListingTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)

                'MJP add cache support 
                Dim cacheKey As String
                Dim originalSearchView As String
                originalSearchView = Me.TaskSearchView
                Me.TaskSearchView = ""
                Me.mTaskSearchView = ""
                cacheKey = IP.Bids.SharedFunctions.CreateKey(Me)
                Me.TaskSearchView = originalSearchView
                Me.mTaskSearchView = originalSearchView
                listingDT = CType(GetDataFromCache(cacheKey, 0.05), TaskListingDS.TaskListingDataTable)
                If listingDT Is Nothing Then
                    listingDT = adapter.GetTaskListing(PlantCode, BusinessUnit, Area, Line, Machine, Division, Region, CreatedBy, Responsible, SourceSystem, SourceSytemRef, SecurityLevel, Title, Description, Type, Activity, TaskStatus, HeaderFromDate, HeaderToDate, DueDateFrom, DueDateTo, ClosedDateFrom, ClosedDateTo, AndOr, UserName, HeaderSeqID, ResponsiblePlantCode, rsTaskList)
                    If listingDT IsNot Nothing AndAlso listingDT.Count > 0 Then
                        InsertDataIntoCache(cacheKey, 0.05, listingDT)
                    End If
                End If
                'MJP End Cache Support

                'listingDT = adapter.GetTaskListing(PlantCode, BusinessUnit, Area, Line, Machine, Division, Region, CreatedBy, Responsible, SourceSystem, SourceSytemRef, SecurityLevel, Title, Description, Type, Activity, TaskStatus, HeaderFromDate, HeaderToDate, DueDateFrom, DueDateTo, ClosedDateFrom, ClosedDateTo, AndOr, UserName, HeaderSeqID, ResponsiblePlantCode, rsTaskList)
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetTaskListingDataTable", , ex)
        Finally
            adapter = Nothing
        End Try
        Return listingDT 'divisionDT
    End Function

    ''' <summary>
    ''' 
    ''' </summary>   
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTaskListing(Optional ByVal noCache As Boolean = False) As System.Collections.Generic.List(Of IP.MEAS.BO.TaskListing) 'MTTDivisionDataTable
        Dim adapter As TaskListingDSTableAdapters.TaskListingTableAdapter
        Dim listingDT As TaskListingDS.TaskListingDataTable = Nothing
        Dim rsTaskList As Object = Nothing
        Dim taskList As System.Collections.Generic.List(Of IP.MEAS.BO.TaskListing) = Nothing

        Try
            If taskList Is Nothing OrElse taskList.Count = 0 Then
                adapter = New TaskListingDSTableAdapters.TaskListingTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)

                'MJP add cache support 
                Dim cacheKey As String
                Dim originalSearchView As String
                originalSearchView = Me.TaskSearchView
                Me.TaskSearchView = ""
                Me.mTaskSearchView = ""
                cacheKey = IP.Bids.SharedFunctions.CreateKey(Me)
                Me.TaskSearchView = originalSearchView
                Me.mTaskSearchView = originalSearchView
                If noCache = False Then
                    listingDT = CType(GetDataFromCache(cacheKey, 0.01), TaskListingDS.TaskListingDataTable)
                Else
                    DeleteFromCache(cacheKey)
                    listingDT = Nothing
                End If
                If listingDT Is Nothing Then
                    listingDT = adapter.GetTaskListing(PlantCode, BusinessUnit, Area, Line, Machine, Division, Region, CreatedBy, Responsible, SourceSystem, SourceSytemRef, SecurityLevel, Title, Description, Type, Activity, TaskStatus, HeaderFromDate, HeaderToDate, DueDateFrom, DueDateTo, ClosedDateFrom, ClosedDateTo, AndOr, UserName, HeaderSeqID, ResponsiblePlantCode, rsTaskList)
                    If listingDT IsNot Nothing AndAlso listingDT.Count > 0 Then
                        InsertDataIntoCache(cacheKey, 0.01, listingDT)
                    End If
                End If
                'MJP End Cache Support

                'Dim cacheKey As String
                'Dim originalSearchView As String
                'originalSearchView = Me.TaskSearchView
                'Me.TaskSearchView = ""
                'Me.mTaskSearchView = ""
                'cacheKey = IP.Bids.SharedFunctions.CreateKey(Me)
                'Me.TaskSearchView = originalSearchView
                'Me.mTaskSearchView = originalSearchView
                'listingDT = CType(GetDataFromCache(cacheKey, 0.2), TaskListing.TaskListingDataTable)
                'If listingDT Is Nothing Then
                '    listingDT = adapter.GetTaskListing(PlantCode, BusinessUnit, Area, Line, Machine, Division, Region, CreatedBy, Responsible, SourceSystem, SourceSytemRef, SecurityLevel, Title, Description, Type, Activity, TaskStatus, HeaderFromDate, HeaderToDate, DueDateFrom, DueDateTo, ClosedDateFrom, ClosedDateTo, AndOr, UserName, HeaderSeqID, ResponsiblePlantCode, rsTaskList)
                '    If listingDT IsNot Nothing AndAlso listingDT.Count > 0 Then
                '        InsertDataIntoCache(cacheKey, 0.2, listingDT)
                '    End If
                'End If
                taskList = New System.Collections.Generic.List(Of IP.MEAS.BO.TaskListing)
                Dim distinctListingDT As New TaskListingDS.TaskListingDataTable

                'Dim dt As DataTable = listingDT.DefaultView.ToTable(True, New String() {"DUEDATE", "ITEMTITLE", "TASKITEMSEQID", "STATUSNAME", "STATUSSEQID", "TASKHEADERSEQID"})
                'For Each rowitem As Data.DataRow In dt.Rows
                '    With rowitem
                '        If .IsNull(0) Then
                '            .Item(0) = Now.ToShortDateString
                '        End If
                '        If .IsNull(1) Then
                '            .Item(1) = "Missing Title"
                '        End If
                '        If .IsNull(3) Then
                '            .Item(3) = ""
                '        End If
                '        If .IsNull(4) Then
                '            .Item(4) = -1
                '        End If
                '        If .IsNull(5) Then
                '            '.Item(5) =
                '        End If
                '    End With
                '    distinctListingDT.LoadDataRow(rowitem.ItemArray, True)
                'Next

                'If sortDescription.Length > 0 Then
                '    listingDT.DefaultView.Sort = sortDescription
                'Else
                '    listingDT.DefaultView.Sort = "DUEDATE,STATUSNAME"
                'End If
                distinctListingDT.Load(listingDT.DefaultView.ToTable(True, New String() {"DUEDATE", "ITEMTITLE", "TASKITEMSEQID", "STATUSNAME", "STATUSSEQID", "TASKHEADERSEQID", "ROLEDESCRIPTION", "RESPNAME", "RESPROLESITENAME"}).CreateDataReader)


                For i As Integer = 0 To distinctListingDT.DefaultView.Count - 1
                    Dim row As TaskListingDS.TaskListingRow = TryCast(distinctListingDT.DefaultView.Item(i).Row, TaskListingDS.TaskListingRow)
                    With row

                        If .IsPLANTCODENull Then
                            .PLANTCODE = String.Empty
                        End If
                        If .Is_REGIONNull Then
                            ._REGION = String.Empty
                        End If
                        If .IsACTIVITYNAMENull Then
                            .ACTIVITYNAME = String.Empty
                        End If
                        If .IsACTIVITYSEQIDNull Then
                            .ACTIVITYSEQID = -1
                        End If
                        If .IsATTACHMENTCOUNTNull Then
                            .ATTACHMENTCOUNT = 0
                        End If
                        If .IsBUSUNIT_AREANull Then
                            .BUSUNIT_AREA = String.Empty
                        End If
                        If .IsDIVISIONNull Then
                            .DIVISION = String.Empty
                        End If
                        'If .IsDUEDATENull Then
                        '    .DUEDATE = CDate(String.Empty)
                        'End If
                        If .IsENDDATENull Then
                            .ENDDATE = String.Empty
                        Else
                            .ENDDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.ENDDATE).ToShortDateString
                        End If
                        If .IsEXTERNALREFNull Then
                            .EXTERNALREF = String.Empty
                        End If
                        If .IsEXTERNALSOURCENAMENull Then
                            .EXTERNALSOURCENAME = String.Empty
                        End If
                        If .IsEXTERNALSOURCESEQIDNull Then
                            .EXTERNALSOURCESEQID = 0
                        End If
                        If .IsHEADERCREATEDBYNull Then
                            .HEADERCREATEDBY = String.Empty
                        End If
                        If .IsHEADERCREATEDDATENull Then
                            .HEADERCREATEDDATE = Now
                        End If
                        If .IsHEADERDESCNull Then
                            .HEADERDESC = String.Empty
                        End If
                        If .IsHEADERLASTUPDATEDATENull Then
                            .HEADERLASTUPDATEDATE = Now
                        End If
                        If .IsHEADERLASTUPDATEUSERNAMENull Then
                            .HEADERLASTUPDATEUSERNAME = String.Empty
                        End If
                        If .IsHEADERTITLENull Then
                            .HEADERTITLE = String.Empty
                        End If
                        If .IsITEMCREATEDDATENull Then
                            .ITEMCREATEDDATE = Now
                        End If
                        If .IsITEMDESCNull Then
                            .ITEMDESC = String.Empty
                        End If
                        If .IsITEMLASTUPDATEDATENull Then
                            .ITEMLASTUPDATEDATE = Now
                        End If
                        If .IsITEMTITLENull Then
                            .ITEMTITLE = String.Empty
                            'Else
                            '    .ITEMTITLE = IP.Bids.SharedFunctions.DataClean(.ITEMTITLE)
                        End If
                        If .IsLINENull Then
                            .LINE = String.Empty
                        End If
                        'If .IsPLANTCODENull Then
                        '    .PLANTCODE = String.Empty
                        'End If
                        If .IsSECURITYLEVELNull Then
                            .SECURITYLEVEL = "N"
                        End If
                        If .IsSITENAMENull Then
                            .SITENAME = String.Empty
                        End If
                        If .IsSTARTDATENull Then
                            .STARTDATE = String.Empty
                        Else
                            .STARTDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.STARTDATE).ToShortDateString
                        End If
                        If .IsSTATUSNAMENull Then
                            .STATUSNAME = String.Empty
                        End If
                        If .IsSTATUSSEQIDNull Then
                            .STATUSSEQID = 0
                        End If
                        If .IsTASKITEMSEQIDNull Then
                            .TASKITEMSEQID = 0
                        End If
                        If .IsTASKTYPENull Then
                            .TASKTYPE = String.Empty
                        End If
                        If .IsHEADERSTATUSNull Then
                            .HEADERSTATUS = String.Empty
                        End If
                        If .IsROLEDESCRIPTIONNull Then
                            .ROLEDESCRIPTION = String.Empty
                        End If

                        If .IsRESPROLESITENAMENull Then
                            .RESPROLESITENAME = String.Empty
                        End If

                        If .IsRESPNAMENull Then
                            .RESPNAME = String.Empty
                        End If
                        If .IsDUEDATENull = False Then
                            .DUEDATE = IP.Bids.SharedFunctions.CDateFromEnglishDate(.DUEDATE).ToShortDateString
                        End If
                        If .IsCLOSEDDATENull Then
                            .CLOSEDDATE = String.Empty
                        End If
                        If .IsOPENSUBTASKSNull Then
                            .OPENSUBTASKS = "N"
                        End If
                        If .IsDATECRITICALNull Then
                            .DATECRITICAL = "N"
                        End If
                        If .IsCOMMENTSNull Then
                            .COMMENTS = ""
                        End If
                        If .IsPARENTDUEDATENull Then
                            .PARENTDUEDATE = .DUEDATE
                        End If
                        If .IsDEPENDENTTASKSEQIDNull Then
                            .DEPENDENTTASKSEQID = 0
                        End If
                        If .IsPARENTSUBTASKSEQIDNull Then
                            .PARENTSUBTASKSEQID = 0
                        End If
                        If .IsDUEDATENull = False AndAlso .DUEDATE.Length > 0 Then
                            Dim currentTaskItem As IP.MEAS.BO.TaskItem
                            Dim taskItemBll As New TaskTrackerItemBll
                            currentTaskItem = taskItemBll.GetTaskItem(CInt(.TASKITEMSEQID))
                            taskList.Add(New IP.MEAS.BO.TaskListing(.ACTIVITYNAME, CInt(.ACTIVITYSEQID), CInt(.ATTACHMENTCOUNT), .BUSUNIT_AREA, .DIVISION, .DUEDATE, .ENDDATE, .EXTERNALREF, CStr(.EXTERNALSOURCESEQID), .EXTERNALSOURCENAME, .HEADERCREATEDDATE.ToShortDateString, .HEADERCREATEDBY, CStr(.HEADERDESC), .HEADERLASTUPDATEDATE.ToShortDateString, .HEADERLASTUPDATEUSERNAME, .HEADERTITLE, .ITEMCREATEDDATE.ToShortDateString, .ITEMDESC, .ITEMLASTUPDATEDATE.ToShortDateString, CStr(.ITEMTITLE), .LINE, .PLANTCODE, ._REGION, .SECURITYLEVEL, .SITENAME, .STARTDATE, .STATUSNAME, CStr(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TASKTYPE, .HEADERSTATUS, .ROLEDESCRIPTION, .RESPNAME, .RESPROLESITENAME, .CLOSEDDATE, .DATECRITICAL, .OPENSUBTASKS, .TANKINSPECTIONTYPEID, CInt(currentTaskItem.Priority), .COMMENTS, .PARENTDUEDATE, CInt(.PARENTSUBTASKSEQID), CInt(.DEPENDENTTASKSEQID)))
                        End If

                    End With
                Next
                'For Each rowItem As TaskListing.TaskListingRow In distinctListingDT.Rows
                '    With rowItem
                '        If .Is_REGIONNull Then
                '            ._REGION = String.Empty
                '        End If
                '        If .IsACTIVITYNAMENull Then
                '            .ACTIVITYNAME = String.Empty
                '        End If
                '        If .IsACTIVITYSEQIDNull Then
                '            .ACTIVITYSEQID = -1
                '        End If
                '        If .IsATTACHMENTCOUNTNull Then
                '            .ATTACHMENTCOUNT = 0
                '        End If
                '        If .IsBUSUNIT_AREANull Then
                '            .BUSUNIT_AREA = String.Empty
                '        End If
                '        If .IsDIVISIONNull Then
                '            .DIVISION = String.Empty
                '        End If
                '        If .IsDUEDATENull Then
                '            .DUEDATE = String.Empty
                '        End If
                '        If .IsENDDATENull Then
                '            .ENDDATE = String.Empty
                '        End If
                '        If .IsEXTERNALREFNull Then
                '            .EXTERNALREF = String.Empty
                '        End If
                '        If .IsEXTERNALSOURCENAMENull Then
                '            .EXTERNALSOURCENAME = String.Empty
                '        End If
                '        If .IsEXTERNALSOURCESEQIDNull Then
                '            .EXTERNALSOURCESEQID = 0
                '        End If
                '        If .IsHEADERCREATEDBYNull Then
                '            .HEADERCREATEDBY = String.Empty
                '        End If
                '        If .IsHEADERCREATEDDATENull Then
                '            .HEADERCREATEDDATE = Now
                '        End If
                '        If .IsHEADERDESCNull Then
                '            .HEADERDESC = String.Empty
                '        End If
                '        If .IsHEADERLASTUPDATEDATENull Then
                '            .HEADERLASTUPDATEDATE = Now
                '        End If
                '        If .IsHEADERLASTUPDATEUSERNAMENull Then
                '            .HEADERLASTUPDATEUSERNAME = String.Empty
                '        End If
                '        If .IsHEADERTITLENull Then
                '            .HEADERTITLE = String.Empty
                '        End If
                '        If .IsITEMCREATEDDATENull Then
                '            .ITEMCREATEDDATE = Now
                '        End If
                '        If .IsITEMDESCNull Then
                '            .ITEMDESC = String.Empty
                '        End If
                '        If .IsITEMLASTUPDATEDATENull Then
                '            .ITEMLASTUPDATEDATE = Now
                '        End If
                '        If .IsITEMTITLENull Then
                '            .ITEMTITLE = String.Empty
                '        End If
                '        If .IsLINENull Then
                '            .LINE = String.Empty
                '        End If
                '        If .IsPLANTCODENull Then
                '            .PLANTCODE = String.Empty
                '        End If
                '        If .IsSECURITYLEVELNull Then
                '            .SECURITYLEVEL = "N"
                '        End If
                '        If .IsSITENAMENull Then
                '            .SITENAME = String.Empty
                '        End If
                '        If .IsSTARTDATENull Then
                '            .STARTDATE = String.Empty
                '        End If
                '        If .IsSTATUSNAMENull Then
                '            .STATUSNAME = String.Empty
                '        End If
                '        If .IsSTATUSSEQIDNull Then
                '            .STATUSSEQID = 0
                '        End If
                '        If .IsTASKITEMSEQIDNull Then
                '            .TASKITEMSEQID = 0
                '        End If
                '        If .IsTASKTYPENull Then
                '            .TASKTYPE = String.Empty
                '        End If
                '        If .IsHEADERSTATUSNull Then
                '            .HEADERSTATUS = String.Empty
                '        End If

                '        divisionList.Add(New IP.MEAS.BO.TaskListing(.ACTIVITYNAME, CInt(.ACTIVITYSEQID), CInt(.ATTACHMENTCOUNT), .BUSUNIT_AREA, .DIVISION, .DUEDATE, .ENDDATE, .EXTERNALREF, CStr(.EXTERNALSOURCESEQID), .EXTERNALSOURCENAME, .HEADERCREATEDDATE.ToShortDateString, .HEADERCREATEDBY, .HEADERDESC, .HEADERLASTUPDATEDATE.ToShortDateString, .HEADERLASTUPDATEUSERNAME, .HEADERTITLE, .ITEMCREATEDDATE.ToShortDateString, .ITEMDESC, .ITEMLASTUPDATEDATE.ToShortDateString, .ITEMTITLE, .LINE, .PLANTCODE, ._REGION, .SECURITYLEVEL, .SITENAME, .STARTDATE, .STATUSNAME, CStr(.STATUSSEQID), CInt(.TASKHEADERSEQID), CInt(.TASKITEMSEQID), .TASKTYPE, .HEADERSTATUS))
                '    End With
                'Next
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetTaskListing", "Error getting the task listing.", ex)
        Finally
            adapter = Nothing
            listingDT = Nothing
        End Try
        Return taskList 'divisionDT
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="instanceOfObject"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetXml(ByVal instanceOfObject As TaskListingBll) As String
        Try
            If instanceOfObject IsNot Nothing Then
                Dim xmlRFQ As New System.Xml.Serialization.XmlSerializer(instanceOfObject.GetType)
                Dim swRFQWriter As New IO.StringWriter()
                xmlRFQ.Serialize(swRFQWriter, instanceOfObject)

                Return swRFQWriter.ToString()
            Else
                Throw New Exception("GetXml: Specified object is nothing")
            End If
        Catch ex As Exception
            Throw New Exception("GetXml: Could Not Serialize object to string", ex)
        End Try
    End Function

    ''' <summary>
    ''' XMLs to object.	
    ''' </summary>
    ''' <param name="containingXml">The containing XML.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function LoadXML(ByVal containingXml As String) As TaskListingBll
        Try
            Dim x As New System.Xml.Serialization.XmlSerializer(GetType(TaskListingBll))
            Dim sr As New IO.StringReader(containingXml)
            Dim obj As TaskListingBll = CType(x.Deserialize(sr), TaskListingBll)
            Return obj
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class

