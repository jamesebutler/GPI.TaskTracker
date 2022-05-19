'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : mjpope
' Created          : 10-22-2010
'
' Last Modified By : mjpope
' Last Modified On : 06-22-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
'Imports System.Data
'Imports System.Data.OracleClient
Imports HelperDal
Imports Devart.Data.Oracle
Imports IP.TaskTrackerDAL

'Imports TaskTrackersiteDAL

Public Class TaskTrackerSiteBll
    'This is an extension of the TaskTrackerSiteDAL.xsd file.  The purpose is to provide a simplified way of getting the 
    'Task Tracker Site Data

    ''' <summary>
    ''' Gets a distinct list of Divisions
    ''' </summary>
    ''' <returns>Returns a Strongly Typed DataTable containing a distinct list of Divisions</returns>
    ''' <remarks></remarks>
    Public Function GetDivision() As System.Collections.Generic.List(Of Division) 'MTTDivisionDataTable
        Dim divisionTA As TaskTrackerSiteDALTableAdapters.MTTDivisionTableAdapter
        Dim divisionDT As TaskTrackerSiteDAL.MTTDivisionDataTable = Nothing
        Dim rsDivisionList As Object = Nothing
        Dim cacheKey As String = "MTTSite_DivisionList"
        Dim cacheHours As Double = 8
        Dim divisionList As System.Collections.Generic.List(Of Division)

        Try
            divisionList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of Division))
            If divisionList Is Nothing OrElse divisionList.Count = 0 Then
                divisionTA = New TaskTrackerSiteDALTableAdapters.MTTDivisionTableAdapter
                divisionTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                divisionDT = divisionTA.GetDivisionList(rsDivisionList)
                divisionList = New System.Collections.Generic.List(Of Division)
                For Each rowItem As TaskTrackerSiteDAL.MTTDivisionRow In divisionDT.Rows
                    If rowItem.IsDIVISIONNull = False Then
                        divisionList.Add(New IP.MEAS.BO.Division(rowItem.DIVISION))
                    End If
                Next
                InsertDataIntoCache(cacheKey, cacheHours, divisionList)
            End If
        Catch
            Throw
        Finally
            divisionTA = Nothing
            divisionDT = Nothing
        End Try
        Return divisionList 'divisionDT
    End Function





    ''' <summary>
    ''' Gets a list of Regions By Division
    ''' </summary>
    ''' <param name="division">String - Name of Division</param>
    ''' <returns>Returns a strongly typed datatable containing a list of Regions By Division</returns>
    ''' <remarks></remarks>
    Public Function GetRegion(ByVal division As String) As System.Collections.Generic.List(Of Region) 'MTTRegionDataTable
        Dim regionTA As TaskTrackerSiteDALTableAdapters.MTTRegionTableAdapter
        Dim regionDT As TaskTrackerSiteDAL.MTTRegionDataTable = Nothing
        Dim regionList As System.Collections.Generic.List(Of Region)
        Dim rsRegionList As Object = Nothing
        Dim cacheKey As String = "MTTSite_RegionList_all" '& division
        Dim cacheHours As Double = 0
        Try
            If division.ToLower = "all" Or division.Length = 0 Then
                cacheHours = 8
            End If
            regionList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of Region))
            If regionList Is Nothing OrElse regionList.Count = 0 Then
                regionTA = New TaskTrackerSiteDALTableAdapters.MTTRegionTableAdapter
                regionTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                regionDT = regionTA.GetRegionList(division, rsRegionList)
                regionList = New System.Collections.Generic.List(Of Region)
                For Each rowItem As TaskTrackerSiteDAL.MTTRegionRow In regionDT.Rows
                    If rowItem.IsREGIONNAMENull = False Then
                        regionList.Add(New IP.MEAS.BO.Region(rowItem.REGIONNAME))
                    End If
                Next
                InsertDataIntoCache(cacheKey, cacheHours, regionList)
            End If
        Catch
            Throw
        Finally
            regionTA = Nothing
            regionDT = Nothing
        End Try
        Return regionList
    End Function

    ''' <summary>
    ''' Gets a list of Facilities by Division and Region
    ''' </summary>
    ''' <param name="division">String - Name of Division</param>
    ''' <param name="region">String - Name of Region</param>
    ''' <returns>Returns a Strongly Typed DataTable containing a list of Facilities by Division and Region</returns>
    ''' <remarks></remarks>
    Public Function GetFacility(ByVal division As String, ByVal region As String) As System.Collections.Generic.List(Of Facility)
        Dim facilityTA As TaskTrackerSiteDALTableAdapters.MTTFacilityTableAdapter
        Dim facilityDT As TaskTrackerSiteDAL.MTTFacilityDataTable = Nothing
        Dim facilityList As System.Collections.Generic.List(Of Facility)
        Dim rsfacilityList As Object = Nothing
        Dim cacheKey As String = "MTTSite_FacilityList_all" '& division & "_" & region
        Dim cacheHours As Integer = 0
        Try
            If (division.ToLower = "all" Or division.Length = 0) And (region.ToLower = "all" Or region.Length = 0) Then
                cacheHours = 8
            End If
            facilityList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of Facility))
            If facilityList Is Nothing OrElse facilityList.Count = 0 Then
                facilityTA = New TaskTrackerSiteDALTableAdapters.MTTFacilityTableAdapter
                facilityTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                facilityDT = facilityTA.GetFacilityList(division, region, rsfacilityList)
                facilityList = New System.Collections.Generic.List(Of Facility)
                For Each rowItem As TaskTrackerSiteDAL.MTTFacilityRow In facilityDT.Rows
                    If rowItem.IsSITENAMENull = False AndAlso rowItem.IsPLANTCODENull = False Then
                        facilityList.Add(New IP.MEAS.BO.Facility(rowItem.SITENAME, rowItem.PLANTCODE))
                    End If
                Next
                InsertDataIntoCache(cacheKey, cacheHours, facilityList)
            End If
        Catch
            Throw
        Finally
            facilityTA = Nothing
            facilityDT = Nothing
        End Try
        Return facilityList
    End Function

    Public Function GetSiteList() As System.Collections.Generic.List(Of Facility)
        Dim facilityTA As DataMaintTableAdapters.GETSITELISTTableAdapter
        Dim facilityDT As DataMaint.GETSITELISTDataTable = Nothing
        Dim facilityList As System.Collections.Generic.List(Of Facility)
        Dim rsfacilityList As Object = Nothing
        Dim cacheKey As String = "MTTSite_List_all"
        Dim cacheHours As Integer = 1
        Try

            facilityList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of Facility))
            If facilityList Is Nothing OrElse facilityList.Count = 0 Then
                facilityTA = New DataMaintTableAdapters.GETSITELISTTableAdapter
                facilityTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                facilityDT = facilityTA.GetSiteList("All", "All", Nothing)
                facilityList = New System.Collections.Generic.List(Of Facility)
                For Each rowItem As DataMaint.GETSITELISTRow In facilityDT.Rows
                    If rowItem.IsSITENAMENull = False AndAlso rowItem.IsPLANTCODENull = False Then
                        facilityList.Add(New IP.MEAS.BO.Facility(rowItem.SITENAME, rowItem.PLANTCODE))
                    End If
                Next
                InsertDataIntoCache(cacheKey, cacheHours, facilityList)
            End If
        Catch
            Throw
        Finally
            facilityTA = Nothing
            facilityDT = Nothing
        End Try
        Return facilityList
    End Function

    ''' <summary>
    ''' Gets a list of Facilities by Division and Region
    ''' </summary>
    ''' <param name="username">String - Network Userid of User</param>
    ''' <returns>Returns a Strongly Typed DataTable containing a list of Facilities by Auth User</returns>
    ''' <remarks></remarks>
    Public Function GetFacilitybyUser(ByVal UserName As String) As System.Collections.Generic.List(Of Facility)
        Dim facilityTA As DataMaintenanceDALTableAdapters.FacilitybyUserTableAdapter
        Dim facilityDT As DataMaintenanceDAL.FacilitybyUserDataTable = Nothing
        Dim facilityList As System.Collections.Generic.List(Of Facility) = Nothing
        Dim rsfacilityList As Object = Nothing
        Dim cacheKey As String = "MTTSite_FacilityList_all" '& division & "_" & region
        Dim cacheHours As Integer = 0
        Try
            ' facilityList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of Facility))
            If facilityList Is Nothing OrElse facilityList.Count = 0 Then
                facilityTA = New DataMaintenanceDALTableAdapters.FacilitybyUserTableAdapter
                facilityTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                facilityDT = facilityTA.GetSitebyUser(UserName, rsfacilityList)
                facilityList = New System.Collections.Generic.List(Of Facility)
                For Each rowItem As DataMaintenanceDAL.FacilitybyUserRow In facilityDT.Rows
                    If rowItem.IsSITENAMENull = False AndAlso rowItem.IsPLANTCODENull = False Then
                        facilityList.Add(New IP.MEAS.BO.Facility(rowItem.SITENAME, rowItem.PLANTCODE))
                    End If
                Next
                'InsertDataIntoCache(cacheKey, cacheHours, facilityList)
            End If
        Catch
            Throw
        Finally
            facilityTA = Nothing
            facilityDT = Nothing
        End Try
        Return facilityList
    End Function
    ''' <summary>
    ''' Gets a list of Area  records.
    ''' </summary>
    ''' <param name="division">String - Name of division.</param>
    ''' <param name="region">String - Name of region.</param>
    ''' <param name="plantcode">String - plantcode value.</param>
    ''' <returns>Returns a Strongly Typed DataTable containing a list of Business Units</returns>
    Public Function GetArea(ByVal division As String, ByVal region As String, ByVal plantcode As String, ByVal businessUnit As String) As System.Collections.Generic.List(Of Area)
        Dim areaTA As TaskTrackerSiteDALTableAdapters.MTTAreaTableAdapter
        Dim areaDT As TaskTrackerSiteDAL.MTTAreaDataTable = Nothing
        Dim areaList As System.Collections.Generic.List(Of Area)
        Dim rsareaList As Object = Nothing
        Dim cacheKey As String = "MTTSite_areaList_all" '& division & "_" & region & "_" & plantcode & "_" & businessUnit
        Dim cacheHours As Integer = 0
        Try
            'If (division.ToLower = "all" Or division.Length = 0) And (region.ToLower = "all" Or region.Length = 0) Then
            '    cacheHours = 8
            'End If
            areaList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of Area))
            If areaList Is Nothing OrElse areaList.Count = 0 Then
                areaTA = New TaskTrackerSiteDALTableAdapters.MTTAreaTableAdapter
                areaTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                areaDT = areaTA.GetArea(plantcode, division, region, businessUnit, rsareaList)
                areaList = New System.Collections.Generic.List(Of Area)
                For Each rowItem As TaskTrackerSiteDAL.MTTAreaRow In areaDT.Rows
                    If rowItem.IsAREANull = False Then
                        areaList.Add(New IP.MEAS.BO.Area(rowItem.AREA))
                    End If
                Next
                InsertDataIntoCache(cacheKey, cacheHours, areaList)
            End If
        Catch
            Throw
        Finally
            areaTA = Nothing
            areaDT = Nothing
        End Try
        Return areaList
    End Function

    ''' <summary>
    ''' Gets a list of business unit / area records.
    ''' </summary>
    ''' <param name="division">String - Name of division.</param>
    ''' <param name="region">String - Name of region.</param>
    ''' <param name="plantcode">String - plantcode value.</param>
    ''' <returns>Returns a Strongly Typed DataTable containing a list of Business Unit Areas</returns>
    Public Function GetBusinessUnitArea(ByVal division As String, ByVal region As String, ByVal plantcode As String) As System.Collections.Generic.List(Of BusinessUnitArea)
        Dim businessUnitAreaTA As TaskTrackerSiteDALTableAdapters.MTTBusinessUnitAreaTableAdapter
        Dim businessUnitAreaDT As TaskTrackerSiteDAL.MTTBusinessUnitAreaDataTable = Nothing
        Dim businessUnitAreaList As System.Collections.Generic.List(Of BusinessUnitArea)
        Dim rsbusinessUnitAreaList As Object = Nothing
        Dim cacheKey As String = "MTTSite_businessUnitAreaList_all" ' & division & "_" & region & "_" & plantcode
        Dim cacheHours As Integer = 0
        Try
            If (division.ToLower = "all" Or division.Length = 0) And (region.ToLower = "all" Or region.Length = 0) And (plantcode.Length = 0) Then
                cacheHours = 8
            End If
            businessUnitAreaList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of BusinessUnitArea))
            If businessUnitAreaList Is Nothing OrElse businessUnitAreaList.Count = 0 Then
                businessUnitAreaTA = New TaskTrackerSiteDALTableAdapters.MTTBusinessUnitAreaTableAdapter
                businessUnitAreaTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                businessUnitAreaDT = businessUnitAreaTA.GetBusinessUnitArea(plantcode, division, region, rsbusinessUnitAreaList)
                businessUnitAreaList = New System.Collections.Generic.List(Of BusinessUnitArea)
                For Each rowItem As TaskTrackerSiteDAL.MTTBusinessUnitAreaRow In businessUnitAreaDT.Rows
                    If rowItem.IsBUSUNITNull = False Then
                        businessUnitAreaList.Add(New IP.MEAS.BO.BusinessUnitArea(rowItem.BUSUNIT))
                    End If
                Next
                InsertDataIntoCache(cacheKey, cacheHours, businessUnitAreaList)
            End If
        Catch
            Throw
        Finally
            businessUnitAreaTA = Nothing
            businessUnitAreaDT = Nothing
        End Try
        Return businessUnitAreaList
    End Function

    ''' <summary>
    ''' Gets a list of business unit  records.
    ''' </summary>
    ''' <param name="division">String - Name of division.</param>
    ''' <param name="region">String - Name of region.</param>
    ''' <param name="plantcode">String - plantcode value.</param>
    ''' <returns>Returns a Strongly Typed DataTable containing a list of Business Units</returns>
    Public Function GetBusinessUnit(ByVal division As String, ByVal region As String, ByVal plantcode As String) As System.Collections.Generic.List(Of BusinessUnit)
        Dim businessUnitTA As TaskTrackerSiteDALTableAdapters.MTTBusinessUnitTableAdapter
        Dim businessUnitDT As TaskTrackerSiteDAL.MTTBusinessUnitDataTable = Nothing
        Dim businessUnitList As System.Collections.Generic.List(Of BusinessUnit)
        Dim rsbusinessUnitList As Object = Nothing
        Dim cacheKey As String = "MTTSite_businessUnitList_all" ' & division & "_" & region & "_" & plantcode
        Dim cacheHours As Integer = 0
        Try
            'If (division.Length = 0 Or division.ToLower = "all") And (region.Length = 0 Or region.ToLower = "all") And (plantcode.Length = 0) Then
            '    cacheHours = 8
            'End If
            businessUnitList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of BusinessUnit))
            If businessUnitList Is Nothing OrElse businessUnitList.Count = 0 Then
                businessUnitTA = New TaskTrackerSiteDALTableAdapters.MTTBusinessUnitTableAdapter
                businessUnitTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                businessUnitDT = businessUnitTA.GetBusinessUnit(plantcode, division, region, rsbusinessUnitList)
                businessUnitList = New System.Collections.Generic.List(Of BusinessUnit)
                For Each rowItem As TaskTrackerSiteDAL.MTTBusinessUnitRow In businessUnitDT.Rows
                    If rowItem.IsBUSUNITNull = False Then
                        businessUnitList.Add(New IP.MEAS.BO.BusinessUnit(rowItem.BUSUNIT))
                    End If
                Next
                InsertDataIntoCache(cacheKey, cacheHours, businessUnitList)
            End If
        Catch
            Throw
        Finally
            businessUnitTA = Nothing
            businessUnitDT = Nothing
        End Try
        Return businessUnitList
    End Function

    ''' <summary>
    ''' Gets a list of Line\Machine records
    ''' </summary>
    ''' <param name="division">String - Name of division.</param>
    ''' <param name="region">String - Name of region.</param>
    ''' <param name="plantcode">String - plantcode value.</param>
    ''' <param name="businessunitarea">String - name of business unit area</param>
    ''' <returns>Returns a Strongly Typed DataTable containing a list of Line\Machine records</returns>
    ''' <remarks></remarks>
    Public Function GetLineLineBreak(ByVal division As String, ByVal region As String, ByVal plantcode As String, ByVal businessUnitArea As String) As System.Collections.Generic.List(Of LineLineBreak)
        Dim lineLineBreakTA As TaskTrackerSiteDALTableAdapters.MTTLineLineBreakTableAdapter
        Dim lineLineBreakDT As TaskTrackerSiteDAL.MTTLineLineBreakDataTable = Nothing
        Dim rslineLineBreakList As Object = Nothing
        Dim lineLineBreakList As System.Collections.Generic.List(Of LineLineBreak)
        Dim cacheKey As String = "MTTSite_lineLineBreakList_all" '& division & "_" & region & "_" & plantcode & "_" & businessUnitArea
        Dim cacheHours As Integer = 0
        Try
            'If (division.Length = 0 Or division.ToLower = "all") And (region.Length = 0 Or region.ToLower = "all") And (plantcode.Length = 0) And (businessUnitArea.Length = 0 Or businessUnitArea = "all") Then
            '    cacheHours = 8
            'End If
            lineLineBreakList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of LineLineBreak))
            If lineLineBreakList Is Nothing OrElse lineLineBreakList.Count = 0 Then
                lineLineBreakTA = New TaskTrackerSiteDALTableAdapters.MTTLineLineBreakTableAdapter
                lineLineBreakTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                lineLineBreakDT = lineLineBreakTA.GetLineLineBreak(plantcode, businessUnitArea, division, region, rslineLineBreakList)
                lineLineBreakList = New System.Collections.Generic.List(Of LineLineBreak)
                For Each rowItem As TaskTrackerSiteDAL.MTTLineLineBreakRow In lineLineBreakDT.Rows
                    If rowItem.IsLINENull = False Then
                        lineLineBreakList.Add(New IP.MEAS.BO.LineLineBreak(rowItem.LINE))
                    End If
                Next
                InsertDataIntoCache(cacheKey, cacheHours, lineLineBreakList)
            End If
        Catch
            Throw
        Finally
            lineLineBreakTA = Nothing
            lineLineBreakDT = Nothing
        End Try
        Return lineLineBreakList
        lineLineBreakTA = Nothing
    End Function



    ''' <summary>
    ''' Gets a list of Line records
    ''' </summary>
    ''' <param name="division">String - Name of division.</param>
    ''' <param name="region">String - Name of region.</param>
    ''' <param name="plantcode">String - plantcode value.</param>
    ''' <param name="businessUnit">String - name of business unit area</param>
    ''' <returns>Returns a Strongly Typed DataTable containing a list of Line\Machine records</returns>
    ''' <remarks></remarks>
    Public Function GetLine(ByVal division As String, ByVal region As String, ByVal plantcode As String, ByVal businessUnit As String, ByVal area As String) As System.Collections.Generic.List(Of Line)
        Dim lineTA As TaskTrackerSiteDALTableAdapters.MTTLineTableAdapter
        Dim lineDT As TaskTrackerSiteDAL.MTTLineDataTable = Nothing
        Dim rslineList As Object = Nothing
        Dim lineList As System.Collections.Generic.List(Of Line)
        Dim cacheKey As String = "MTTSite_lineList_all" ' & division & "_" & region & "_" & plantcode & "_" & businessUnit & "_" & area
        Dim cacheHours As Integer = 0
        Try
            'If (division.ToLower = "all" Or division.Length = 0) And (region = "all" Or region.Length = 0) Then
            '    cacheHours = 8
            'End If
            lineList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of Line))
            If lineList Is Nothing OrElse lineList.Count = 0 Then
                lineTA = New TaskTrackerSiteDALTableAdapters.MTTLineTableAdapter
                lineTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                lineDT = lineTA.GetLine(plantcode, businessUnit, area, division, region, rslineList)
                lineList = New System.Collections.Generic.List(Of Line)
                For Each rowItem As TaskTrackerSiteDAL.MTTLineRow In lineDT.Rows
                    If rowItem.IsLINENull = False Then
                        lineList.Add(New IP.MEAS.BO.Line(rowItem.LINE))
                    End If
                Next
                InsertDataIntoCache(cacheKey, cacheHours, lineList)
            End If
        Catch
            Throw
        Finally
            lineTA = Nothing
            lineDT = Nothing
        End Try
        Return lineList
        lineTA = Nothing
    End Function

    ''' <summary>
    ''' Gets a list of Line\Machine records
    ''' </summary>
    ''' <param name="division">String - Name of division.</param>
    ''' <param name="region">String - Name of region.</param>
    ''' <param name="plantcode">String - plantcode value.</param>
    ''' <param name="businessunit">String - name of business unit area</param>
    ''' <returns>Returns a Strongly Typed DataTable containing a list of Line\Machine records</returns>
    ''' <remarks></remarks>
    Public Function GetlineBreak(ByVal division As String, ByVal region As String, ByVal plantcode As String, ByVal businessunit As String, ByVal area As String, ByVal line As String) As System.Collections.Generic.List(Of LineBreak)
        Dim lineBreakTA As TaskTrackerSiteDALTableAdapters.MTTLineBreakTableAdapter
        Dim lineBreakDT As TaskTrackerSiteDAL.MTTLineBreakDataTable = Nothing
        Dim rslineBreakList As Object = Nothing
        Dim lineBreakList As System.Collections.Generic.List(Of LineBreak)
        Dim cacheKey As String = "MTTSite_lineBreakList_all" '& division & "_" & region & "_" & plantcode & "_" & businessunit & "_" & area & "_" & line
        Dim cacheHours As Integer = 0
        Try
            If (division.ToLower = "all" Or division.Length = 0) And (region.ToLower = "all" Or region.Length = 0) And (plantcode.ToLower = "all" Or plantcode.Length = 0) And (businessunit.ToLower = "all" Or businessunit.Length = 0) And (area.Length = 0 Or area.ToLower = "all") Then
                cacheHours = 8
            End If
            lineBreakList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of LineBreak))
            If lineBreakList Is Nothing OrElse lineBreakList.Count = 0 Then
                lineBreakTA = New TaskTrackerSiteDALTableAdapters.MTTLineBreakTableAdapter
                lineBreakTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                lineBreakDT = lineBreakTA.GetLineBreak(plantcode, businessunit, area, line, division, region, rslineBreakList)
                lineBreakList = New System.Collections.Generic.List(Of LineBreak)
                For Each rowItem As TaskTrackerSiteDAL.MTTLineBreakRow In lineBreakDT.Rows
                    If rowItem.IsLINEBREAKNull = False Then
                        lineBreakList.Add(New IP.MEAS.BO.LineBreak(rowItem.LINEBREAK))
                    End If
                Next
                InsertDataIntoCache(cacheKey, cacheHours, lineBreakList)
            End If
        Catch
            Throw
        Finally
            lineBreakTA = Nothing
            lineBreakDT = Nothing
        End Try
        Return lineBreakList
        lineBreakTA = Nothing
    End Function

    Public Function GetBusinessRegionFacility(ByVal templateNumber As String) As System.Collections.Generic.List(Of BusinessRegionSite)
        Dim facilityTA As TaskHeaderDALTableAdapters.BusinessRegionSiteTableAdapter 'MJPTestTableAdapters.BusinessRegionSiteTableAdapter
        Dim facilityDT As TaskHeaderDAL.BusinessRegionSiteDataTable = Nothing ' MJPTest.BusinessRegionSiteDataTable = Nothing
        Dim facilityList As System.Collections.Generic.List(Of BusinessRegionSite)
        Dim rsfacilityList As Object = Nothing
        Dim cacheKey As String = "MTTSite_BusinessRegionSite"
        Dim cacheHours As Integer = 0
        Try

            facilityList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of BusinessRegionSite))
            If facilityList Is Nothing OrElse facilityList.Count = 0 Then
                facilityTA = New TaskHeaderDALTableAdapters.BusinessRegionSiteTableAdapter
                facilityTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                facilityDT = facilityTA.GetBusinessRegionSiteList(templateNumber, rsfacilityList)
                facilityList = New System.Collections.Generic.List(Of BusinessRegionSite)
                For Each rowItem As TaskHeaderDAL.BusinessRegionSiteRow In facilityDT.Rows
                    If rowItem.IsDIVISIONNull Then
                        rowItem.DIVISION = String.Empty
                    End If

                    If rowItem.Is_REGIONNull Then
                        rowItem._REGION = "Division " & rowItem.DIVISION
                    End If

                    If rowItem.IsPROCESSED_FLAGNull Then
                        rowItem.PROCESSED_FLAG = String.Empty
                    End If

                    If rowItem.IsSITENAMENull = False AndAlso rowItem.IsPLANTCODENull = False Then
                        facilityList.Add(New IP.MEAS.BO.BusinessRegionSite(rowItem.DIVISION, rowItem._REGION, rowItem.SITENAME, rowItem.PLANTCODE, rowItem.PROCESSED_FLAG))
                    End If
                Next
                InsertDataIntoCache(cacheKey, cacheHours, facilityList)
            End If
        Catch
            Throw
        Finally
            facilityTA = Nothing
            facilityDT = Nothing
        End Try
        Return facilityList
    End Function
End Class

