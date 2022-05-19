'***********************************************************************
' Assembly         :http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          :09-08-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On :09-08-2010
' Description      :Contains the business logic needed to access Report Data
'
' Copyright        :(c) International Paper. All rights reserved.
' Purpose          :The purpose of the Business Logic Layer is to separate the presentation layer from the data access layer.
'***********************************************************************
Option Explicit On
Option Strict On
Imports HelperDal
Imports Devart.Data.Oracle

''' <summary>
''' Contains the business logic needed to access Report Data
''' </summary>
Public Class ReportSelectionBll
#Region "Fields"
    Private _application As String = "MTT"
    Private _RequiredReportParameters As RequiredReportParameters
#End Region

#Region "Properties"
    ''' <summary>
    ''' Gets or sets the name of the application that holds the reports
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Application() As String
        Get
            Return _application
        End Get
        Set(ByVal value As String)
            _application = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the required parameters for the reports
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RequiredParameters() As RequiredReportParameters
        Get
            Return _RequiredReportParameters
        End Get
        Set(ByVal value As RequiredReportParameters)
            _RequiredReportParameters = value
        End Set
    End Property
#End Region

#Region "Structures"
    ''' <summary>
    ''' Used to hold the boolean values for the required report parameters
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure RequiredReportParameters
        Dim Site As Boolean
        Dim ReportType As Boolean
        Dim Types As Boolean
        Dim Activity As Boolean
        Dim TaskStatus As Boolean
        Dim CreatedBy As Boolean
        Dim Responsible As Boolean
        Dim Role As Boolean
        Dim SourceSystem As Boolean
        Dim HighLevelSecurity As Boolean
        Dim DueDate As Boolean
        Dim HeaderDate As Boolean
        Dim ClosedDate As Boolean
        Dim Title As Boolean
        Dim Description As Boolean
        Dim EstimatedDueDate As Boolean
        Dim TaskListing As Boolean
        Dim GMSElement As Boolean
        Dim TaskHeader As Boolean

        Public Overloads Overrides Function GetHashCode() As Integer
            Throw New NotImplementedException()
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As [Object]) As Boolean
            Throw New NotImplementedException()
        End Function

        Public Shared Operator =(ByVal left As RequiredReportParameters, ByVal right As RequiredReportParameters) As Boolean
            Throw New NotImplementedException()
        End Operator

        Public Shared Operator <>(ByVal left As RequiredReportParameters, ByVal right As RequiredReportParameters) As Boolean
            Throw New NotImplementedException()
        End Operator
    End Structure
#End Region

#Region "Methods"
    ''' <summary>
    ''' Gets a listing of the available Report Titles
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetReportTitles() As System.Collections.Generic.List(Of ReportTitles)
        Dim adapter As ReportSelectionDALTableAdapters.ReportTitlesTableAdapter
        Dim data As ReportSelectionDAL.ReportTitlesDataTable = Nothing
        Dim rsCursor As Object = Nothing
        Dim list As System.Collections.Generic.List(Of ReportTitles) = Nothing
        Dim cacheKey As String = "ReportTitles_" & Application
        Dim cacheHours As Integer = 8

        Try
            list = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of ReportTitles))
            If list Is Nothing OrElse list.Count = 0 Then
                adapter = New ReportSelectionDALTableAdapters.ReportTitlesTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                data = adapter.GetReportTitles(Application, rsCursor)
                list = New System.Collections.Generic.List(Of ReportTitles)
                For Each rowItem As ReportSelectionDAL.ReportTitlesRow In data.Rows
                    If rowItem.IsREPORTTITLENull Then
                        rowItem.REPORTTITLE = "Missing Title"
                    End If
                    list.Add(New ReportTitles(rowItem.REPORTTITLE))
                Next
                'InsertDataIntoCache(cacheKey, cacheHours, list)
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetReportTitles", "Error getting the report titles", ex)
        Finally
            adapter = Nothing
            data = Nothing
        End Try
        Return list
        adapter = Nothing
    End Function

    ''' <summary>
    ''' Gets the sort values for the reports
    ''' </summary>
    ''' <param name="reportTitle">String - The title of the report that we need parameters for</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetReportSortValues(ByVal reportTitle As String) As System.Collections.Generic.List(Of ReportSortValues)
        Dim adapter As ReportSelectionDALTableAdapters.ReportSortsTableAdapter
        Dim data As ReportSelectionDAL.ReportSortsDataTable = Nothing
        Dim rsCursor As Object = Nothing
        Dim list As System.Collections.Generic.List(Of ReportSortValues) = Nothing
        Dim cacheKey As String = "ReportSortValue_" & Application & "_" & reportTitle
        Dim cacheHours As Integer = 8

        Try
            list = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of ReportSortValues))
            If list Is Nothing OrElse list.Count = 0 Then
                adapter = New ReportSelectionDALTableAdapters.ReportSortsTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                data = adapter.GetReportSorts(reportTitle, Application, rsCursor)
                list = New System.Collections.Generic.List(Of ReportSortValues)
                For Each rowItem As ReportSelectionDAL.ReportSortsRow In data.Rows
                    If rowItem.IsREPORTTITLENull Then
                        rowItem.REPORTTITLE = "Missing Title"
                    End If
                    If rowItem.IsINACTIVE_FLAGNull Then
                        rowItem.INACTIVE_FLAG = "N"
                    End If
                    If rowItem.IsREPORTNAMENull Then
                        rowItem.REPORTNAME = ""
                    End If
                    If rowItem.IsSORTVALUEORDERNull Then
                        rowItem.SORTVALUEORDER = 1
                    End If
                    If rowItem.IsREPORTSORTVALUENull Then
                        rowItem.REPORTSORTVALUE = ""
                    End If

                    list.Add(New ReportSortValues(rowItem.REPORTTITLE, rowItem.REPORTSORTVALUE, rowItem.REPORTNAME, rowItem.SORTVALUEORDER, rowItem.INACTIVE_FLAG))
                Next
                'InsertDataIntoCache(cacheKey, cacheHours, list)
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetReportSortValues", "Error getting the report sort values", ex)
        Finally
            adapter = Nothing
            data = Nothing
        End Try
        Return list
        adapter = Nothing
    End Function

    ''' <summary>
    ''' Gets a listing of Report Parameters for the specified report
    ''' </summary>
    ''' <param name="reportTitle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetReportParameters(ByVal reportTitle As String) As System.Collections.Generic.List(Of ReportParameters)
        Dim adapter As ReportSelectionDALTableAdapters.ReportParametersTableAdapter
        Dim data As ReportSelectionDAL.ReportParametersDataTable = Nothing
        Dim rsCursor As Object = Nothing
        Dim list As System.Collections.Generic.List(Of ReportParameters) = Nothing
        Dim cacheKey As String = "ReportParameters_" & Application & "_" & reportTitle
        Dim cacheHours As Integer = 2

        Try
            list = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of ReportParameters))
            If list Is Nothing OrElse list.Count = 0 Then
                list = New System.Collections.Generic.List(Of ReportParameters)
            End If
            If list IsNot Nothing Then
                adapter = New ReportSelectionDALTableAdapters.ReportParametersTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                data = adapter.GetReportParameters(reportTitle.Trim, Application, rsCursor)
                For Each rowItem As ReportSelectionDAL.ReportParametersRow In data.Rows
                    If rowItem.IsREPORTTITLENull Then
                        rowItem.REPORTTITLE = "Missing Title"
                    End If
                    If rowItem.IsPARAMETERSORTNull Then
                        rowItem.PARAMETERSORT = 1
                    End If
                    If rowItem.IsREPORTPARAMETERSNull Then
                        rowItem.REPORTPARAMETERS = ""
                    End If
                    If rowItem.IsREPORTPARAMETERTYPENull Then
                        rowItem.REPORTPARAMETERTYPE = ""
                    End If
                    With _RequiredReportParameters
                        Select Case rowItem.REPORTPARAMETERTYPE.ToLower
                            Case "site"
                                .Site = True
                            Case "reporttype"
                                .ReportType = True
                        End Select
                    End With
                    list.Add(New ReportParameters(rowItem.REPORTTITLE, CStr(rowItem.PARAMETERSORT), rowItem.REPORTPARAMETERS, rowItem.REPORTPARAMETERTYPE))
                Next
                'InsertDataIntoCache(cacheKey, cacheHours, list)
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetReportParameters", "Error getting the report parameters", ex)
        Finally
            adapter = Nothing
            data = Nothing
        End Try
        Return list
        adapter = Nothing
    End Function
#End Region

#Region "Constructor(s)"
    Public Sub New(ByVal application As String)
        _application = application
    End Sub
#End Region
End Class
