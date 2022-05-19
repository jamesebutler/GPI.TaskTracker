'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : mjpope
' Created          : 10-24-2010
'
' Last Modified By : mjpope
' Last Modified On : 06-27-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Option Explicit On
Option Strict On

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports AjaxControlToolkit
Imports System.Data

Imports IP.Bids
Imports System.Collections.Generic

''' <summary>
''' CascadingDropDown is an ASP.NET AJAX extender that can be attached to an ASP.NET DropDownList control to get automatic population of a set of DropDownList controls. Each time the selection of one the DropDownList controls changes, the CascadingDropDown makes a call to a specified web service to retrieve the list of values for the next DropDownList in the set. 
''' </summary>
''' <remarks>Example Code - http://asp.net/ajax/control-toolkit/live/</remarks>
''' 
<WebService(Namespace:="http://MTT/")> _
<System.Web.Script.Services.ScriptService()> _
Public Class SiteDropDownsWS
    Inherits System.Web.Services.WebService


    ''' <summary>
    ''' Gets the list of values for the Region Dropdown List
    ''' </summary>
    ''' <param name="knownCategoryValues"></param>
    ''' <param name="category"></param>
    ''' <param name="contextKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Services.WebMethod(cacheduration:=0, enableSession:=True), Script.Services.ScriptMethod()> _
    Public Function GetRegionList(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()
        Dim values As New Generic.List(Of CascadingDropDownNameValue)
        Dim knownCategoryValuesDictionary As New StringDictionary
        Dim division As String = String.Empty
        Try
            If contextKey Is Nothing Then
                contextKey = String.Empty
            End If
            knownCategoryValues = SharedFunctions.CleanKnownCategoryValues(knownCategoryValues)
            knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
            If Not (knownCategoryValuesDictionary.ContainsKey("Division") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                division = "All"
            ElseIf knownCategoryValuesDictionary.ContainsKey("Division") Then
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Division"))
            Else
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            Dim siteBll As TaskTrackerSiteBll = New TaskTrackerSiteBll() 'New TaskTrackerSiteBLL("", "", division, "")
            Dim regionList As System.Collections.Generic.List(Of Region) = CType(HelperDal.GetDataFromCache("RegionList", 1), Global.System.Collections.Generic.List(Of Global.IP.MEAS.BO.Region)) '", System.Collections.Generic.List(Of Region))
            If regionList Is Nothing OrElse regionList.Count = 0 Then
                regionList = siteBll.GetRegion(division)
                If regionList IsNot Nothing AndAlso regionList.Count > 0 Then
                    'HelperDal.InsertDataIntoCache("RegionList", 180, regionList)
                End If
            End If

            If regionList IsNot Nothing Then
                If contextKey.ToLower = "all" Then
                    values.Insert(0, New CascadingDropDownNameValue(IP.Bids.SharedFunctions.LocalizeValue("All"), "All", False))
                    contextKey = String.Empty
                Else
                    values.Insert(0, New CascadingDropDownNameValue(IP.Bids.SharedFunctions.LocalizeValue("All"), "All", False))
                End If
                For Each item As Region In regionList
                    Dim val As String = SharedFunctions.DataClean(item.RegionName)
                    Dim desc As String = SharedFunctions.DataClean(item.RegionName)
                    If contextKey.Length > 0 And contextKey <> "AL" Then
                        'If contextKey.Contains(val) Then
                        values.Add(New CascadingDropDownNameValue(desc, val))
                        'End If
                    Else
                        If val.ToLower <> "al" AndAlso val.ToLower <> "all" Then
                            values.Add(New CascadingDropDownNameValue(desc, val))
                        End If
                    End If
                Next
            End If
            If values.Count > 0 Then
                If contextKey.Length > 0 Then
                    If values.FindIndex(Function(o) o.value = contextKey) > 0 Then
                        values.Item(values.FindIndex(Function(o) o.value = contextKey)).isDefaultValue = True
                    End If
                End If
            End If
        Catch ex As Exception
            SharedFunctions.HandleError("GetRegionList", , ex)
        Finally
            GetRegionList = SharedFunctions.LocalizeDropDownNameValue(values.ToArray)

        End Try

    End Function

    <Services.WebMethod(cacheduration:=0, enableSession:=True), Script.Services.ScriptMethod()> _
   Public Function GetDivisionList(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()
        Dim values As New Generic.List(Of CascadingDropDownNameValue)

        Try
            If contextKey Is Nothing Then
                contextKey = String.Empty
            End If
            Dim siteBll As TaskTrackerSiteBll = New TaskTrackerSiteBll()

            Dim divisionList As System.Collections.Generic.List(Of Division) = CType(HelperDal.GetDataFromCache("DivisionList", 1), Global.System.Collections.Generic.List(Of Global.IP.MEAS.BO.Division))

            If divisionList Is Nothing OrElse divisionList.Count = 0 Then
                divisionList = siteBll.GetDivision()
                If divisionList IsNot Nothing AndAlso divisionList.Count > 0 Then
                    'HelperDal.InsertDataIntoCache("DivisionList", 180, divisionList)
                End If
            End If

            If divisionList IsNot Nothing Then

                If contextKey.ToLower = "all" Then
                    'values.Add(New CascadingDropDownNameValue(SharedFunctions.LocalizeValue("All"), "AL"))
                    contextKey = String.Empty
                End If

                For Each item As Division In divisionList
                    Dim val As String = SharedFunctions.DataClean(item.DivisionName)
                    Dim desc As String = SharedFunctions.DataClean(item.DivisionName)
                    If contextKey.Length > 0 And contextKey <> "AL" Then
                        'If contextKey.Contains(val) Then
                        values.Add(New CascadingDropDownNameValue(desc, val))
                        'End If
                    Else
                        values.Add(New CascadingDropDownNameValue(desc, val))
                    End If
                    'Next
                Next
            End If
            If values.Count > 0 Then
                If contextKey.Length > 0 Then
                    If values.FindIndex(Function(o) o.value = contextKey) > 0 Then
                        values.Item(values.FindIndex(Function(o) o.value = contextKey)).isDefaultValue = True
                    End If
                End If
            End If
        Catch ex As Exception
            SharedFunctions.HandleError("GetDivisionList", , ex)
        Finally
            GetDivisionList = SharedFunctions.LocalizeDropDownNameValue(values.ToArray)

        End Try

    End Function

    <Services.WebMethod(cacheduration:=0, enableSession:=True), Script.Services.ScriptMethod()> _
   Public Function GetFacilityList(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()
        Dim values As New Generic.List(Of CascadingDropDownNameValue)
        Dim knownCategoryValuesDictionary As New StringDictionary
        Dim division As String = String.Empty
        Dim region As String = String.Empty
        Try
            If contextKey Is Nothing Then
                contextKey = String.Empty
            End If
            If knownCategoryValues.Length = 0 Then
                knownCategoryValues = "division:all"
            End If
            knownCategoryValues = SharedFunctions.CleanKnownCategoryValues(knownCategoryValues)
            knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
            If Not (knownCategoryValuesDictionary.ContainsKey("Division") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                'Return Nothing
                division = "all"
            ElseIf knownCategoryValuesDictionary.ContainsKey("Division") Then
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Division"))
            Else
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Region") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                'Return Nothing
                region = "all"
            ElseIf knownCategoryValuesDictionary.ContainsKey("Region") Then
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Region"))
            Else
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If division.ToLower = "all" Then
                division = String.Empty
            End If
            If region.ToLower = "all" Then
                region = String.Empty
            End If
            Dim siteBll As TaskTrackerSiteBll = New TaskTrackerSiteBll()

            'Dim FacilityDT As TaskTrackerSiteDAL.MTTFacilityDataTable = SiteBLL.GetFacility(Division, Region)
            Dim facilityList As System.Collections.Generic.List(Of Facility) = CType(HelperDal.GetDataFromCache("FacilityList", 1), Global.System.Collections.Generic.List(Of Global.IP.MEAS.BO.Facility))
            If facilityList Is Nothing OrElse facilityList.Count = 0 Then
                facilityList = siteBll.GetFacility(division, region)
                If facilityList IsNot Nothing AndAlso facilityList.Count > 0 Then
                    'HelperDal.InsertDataIntoCache("FacilityList", 180, facilityList)
                End If
            End If
            '


            If contextKey.ToLower = "all" Then
                values.Insert(0, New CascadingDropDownNameValue(IP.Bids.SharedFunctions.LocalizeValue("All"), "All", False))
                contextKey = String.Empty
            Else
                values.Insert(0, New CascadingDropDownNameValue(IP.Bids.SharedFunctions.LocalizeValue("All"), "All", False))
            End If

            If facilityList IsNot Nothing Then
                For Each item As Facility In facilityList
                    Dim val As String = SharedFunctions.DataClean(item.PlantCode)
                    Dim desc As String = SharedFunctions.DataClean(item.SiteName)
                    If contextKey.Length > 0 And contextKey <> "AL" Then
                        'If contextKey.Contains(val) Then
                        values.Add(New CascadingDropDownNameValue(desc, val))
                        'End If
                    Else
                        'Remove All
                        If val <> "9998" AndAlso val.ToLower <> "all" Then
                            values.Add(New CascadingDropDownNameValue(desc, val))
                        End If
                    End If
                Next
            End If
            If values.Count > 0 Then
                If values.FindIndex(Function(o) o.value = "9998") > 0 Then
                    values.Remove(values.Find(Function(o) o.value = "9998"))
                End If
                If contextKey.Length > 0 Then
                    If values.FindIndex(Function(o) o.value = contextKey) > 0 Then
                        values.Item(values.FindIndex(Function(o) o.value = contextKey)).isDefaultValue = True
                    End If
                End If
            End If

        Catch ex As Exception
            SharedFunctions.HandleError("GetFacilityList", , ex)
        Finally
            GetFacilityList = values.ToArray 'SharedFunctions.LocalizeDropDownNameValue(values.ToArray)

        End Try

    End Function

    <Services.WebMethod(cacheduration:=0, enableSession:=True), Script.Services.ScriptMethod()> _
 Public Function GetBusinessUnitList(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()
        Dim values As New Generic.List(Of CascadingDropDownNameValue)
        Dim knownCategoryValuesDictionary As New StringDictionary
        Dim division As String = String.Empty
        Dim region As String = String.Empty
        Dim facility As String = String.Empty
        Try
            If contextKey Is Nothing Then
                contextKey = String.Empty
            End If
            knownCategoryValues = SharedFunctions.CleanKnownCategoryValues(knownCategoryValues)
            knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
            If Not (knownCategoryValuesDictionary.ContainsKey("Division") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                'Return Nothing
                division = "all"
            ElseIf knownCategoryValuesDictionary.ContainsKey("Division") Then
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Division"))
            Else
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Region") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                'Return Nothing
                region = "all"
            ElseIf knownCategoryValuesDictionary.ContainsKey("Region") Then
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Region"))
            Else
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Facility") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("Facility") Then
                facility = SharedFunctions.DataClean(knownCategoryValuesDictionary("Facility"))
            Else
                facility = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If division.ToLower = "all" Then
                division = String.Empty
            End If
            If region.ToLower = "all" Then
                region = String.Empty
            End If
            If facility.ToLower = "all" Or facility = "9998" Then
                facility = String.Empty
            End If

            Dim siteBll As TaskTrackerSiteBll = New TaskTrackerSiteBll()
            'Dim BusinessUnitAreaDT As TaskTrackerSiteDAL.MTTBusinessUnitAreaDataTable = SiteBLL.GetBusinessUnitArea(Division, Region, Facility)
            'Dim businessUnitList As System.Collections.Generic.List(Of BusinessUnit) = siteBll.GetBusinessUnit(division, region, facility)

            Dim businessUnitList As System.Collections.Generic.List(Of BusinessUnit) = CType(HelperDal.GetDataFromCache("BusinessUnitList", 1), Global.System.Collections.Generic.List(Of Global.IP.MEAS.BO.BusinessUnit))
            If businessUnitList Is Nothing OrElse businessUnitList.Count = 0 Then
                businessUnitList = siteBll.GetBusinessUnit(division, region, facility)
                If businessUnitList IsNot Nothing AndAlso businessUnitList.Count > 0 Then
                    'HelperDal.InsertDataIntoCache("BusinessUnitList", 180, businessUnitList)
                End If
            End If

            If businessUnitList IsNot Nothing Then

                If contextKey.ToLower = "all" Then
                    contextKey = String.Empty
                End If
                values.Insert(0, New CascadingDropDownNameValue(IP.Bids.SharedFunctions.LocalizeValue("All"), "All", False))

                For Each item As BusinessUnit In businessUnitList
                    Dim val As String = SharedFunctions.DataClean(item.BusinessUnit)
                    Dim desc As String = SharedFunctions.DataClean(item.BusinessUnit)
                    If contextKey.Length > 0 And contextKey <> "AL" Then
                        'If contextKey.Contains(val) Then
                        values.Add(New CascadingDropDownNameValue(desc, val))
                        'End If
                    Else
                        'Remove All
                        If val.ToLower <> "al" AndAlso val.ToLower <> "all" Then
                            values.Add(New CascadingDropDownNameValue(desc, val))
                        End If
                    End If
                Next
            End If
            If values.Count > 0 Then
                If contextKey.Length > 0 Then
                    If values.FindIndex(Function(o) o.value = contextKey) > 0 Then
                        values.Item(values.FindIndex(Function(o) o.value = contextKey)).isDefaultValue = True
                    End If
                End If
            End If
        Catch ex As Exception
            SharedFunctions.HandleError("GetBusinessUnitList", , ex)
        Finally
            GetBusinessUnitList = SharedFunctions.LocalizeDropDownNameValue(values.ToArray, "-")

        End Try

    End Function

    <Services.WebMethod(cacheduration:=0, enableSession:=True), Script.Services.ScriptMethod()> _
Public Function GetAreaList(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()
        Dim values As New Generic.List(Of CascadingDropDownNameValue)
        Dim knownCategoryValuesDictionary As New StringDictionary
        Dim division As String = String.Empty
        Dim region As String = String.Empty
        Dim facility As String = String.Empty
        Dim busUnit As String = String.Empty
        Try
            If contextKey Is Nothing Then
                contextKey = String.Empty
            End If
            knownCategoryValues = SharedFunctions.CleanKnownCategoryValues(knownCategoryValues)
            knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
            If Not (knownCategoryValuesDictionary.ContainsKey("Division") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                'Return Nothing
                division = "all"
            ElseIf knownCategoryValuesDictionary.ContainsKey("Division") Then
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Division"))
            Else
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Region") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                'Return Nothing
                region = "all"
            ElseIf knownCategoryValuesDictionary.ContainsKey("Region") Then
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Region"))
            Else
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Facility") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("Facility") Then
                facility = SharedFunctions.DataClean(knownCategoryValuesDictionary("Facility"))
            Else
                facility = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Businessunit") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("Businessunit") Then
                busUnit = SharedFunctions.DataClean(knownCategoryValuesDictionary("Businessunit"))
            Else
                busUnit = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If division.ToLower = "all" Then
                division = String.Empty
            End If
            If region.ToLower = "all" Then
                region = String.Empty
            End If
            If facility.ToLower = "all" Or facility = "9998" Then
                facility = String.Empty
            End If
            If busUnit.ToLower = "all" Then
                busUnit = String.Empty
            End If
            Dim siteBll As TaskTrackerSiteBll = New TaskTrackerSiteBll()
            'Dim BusinessUnitAreaDT As TaskTrackerSiteDAL.MTTBusinessUnitAreaDataTable = SiteBLL.GetBusinessUnitArea(Division, Region, Facility)
            'Dim areaList As System.Collections.Generic.List(Of Area) = siteBll.GetArea(division, region, facility, busUnit)

            Dim areaList As System.Collections.Generic.List(Of Area) = CType(HelperDal.GetDataFromCache("AreaList", 1), Global.System.Collections.Generic.List(Of Global.IP.MEAS.BO.Area))
            If areaList Is Nothing OrElse areaList.Count = 0 Then
                areaList = siteBll.GetArea(division, region, facility, busUnit)
                If areaList IsNot Nothing AndAlso areaList.Count > 0 Then
                    'HelperDal.InsertDataIntoCache("AreaList", 180, areaList)
                End If
            End If

            If areaList IsNot Nothing Then
                If contextKey.ToLower = "all" Then
                    contextKey = String.Empty
                End If
                values.Insert(0, New CascadingDropDownNameValue(IP.Bids.SharedFunctions.LocalizeValue("All"), "All", False))

                For Each item As Area In areaList
                    Dim val As String = SharedFunctions.DataClean(item.Area)
                    Dim desc As String = SharedFunctions.DataClean(item.Area)
                    If contextKey.Length > 0 And contextKey <> "AL" Then
                        'If contextKey.Contains(val) Then
                        values.Add(New CascadingDropDownNameValue(desc, val))
                        'End If
                    Else
                        'Remove All
                        If val.ToLower <> "al" AndAlso val.ToLower <> "all" Then
                            values.Add(New CascadingDropDownNameValue(desc, val))
                        End If
                    End If
                Next
            End If
            If values.Count > 0 Then
                If contextKey.Length > 0 Then
                    If values.FindIndex(Function(o) o.value = contextKey) > 0 Then
                        values.Item(values.FindIndex(Function(o) o.value = contextKey)).isDefaultValue = True
                    End If
                End If
            End If
        Catch ex As Exception
            SharedFunctions.HandleError("GetAreaList", , ex)
        Finally
            GetAreaList = SharedFunctions.LocalizeDropDownNameValue(values.ToArray)

        End Try

    End Function

    <Services.WebMethod(cacheduration:=0, enableSession:=True), Script.Services.ScriptMethod()> _
Public Function GetLineList(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()
        Dim values As New Generic.List(Of CascadingDropDownNameValue)
        Dim knownCategoryValuesDictionary As New StringDictionary
        Dim division As String = String.Empty
        Dim region As String = String.Empty
        Dim facility As String = String.Empty
        Dim busUnit As String = String.Empty
        Dim area As String = String.Empty
        Try
            If contextKey Is Nothing Then
                contextKey = String.Empty
            End If
            knownCategoryValues = SharedFunctions.CleanKnownCategoryValues(knownCategoryValues)
            knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
            If Not (knownCategoryValuesDictionary.ContainsKey("Division") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                'Return Nothing
                division = "all"
            ElseIf knownCategoryValuesDictionary.ContainsKey("Division") Then
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Division"))
            Else
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Region") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                'Return Nothing
                region = "all"
            ElseIf knownCategoryValuesDictionary.ContainsKey("Region") Then
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Region"))
            Else
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Facility") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("Facility") Then
                facility = SharedFunctions.DataClean(knownCategoryValuesDictionary("Facility"))
            Else
                facility = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Businessunit") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("Businessunit") Then
                busUnit = SharedFunctions.DataClean(knownCategoryValuesDictionary("Businessunit"))
            Else
                busUnit = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Area") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("Area") Then
                area = SharedFunctions.DataClean(knownCategoryValuesDictionary("Area"))
            Else
                area = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If division.ToLower = "all" Then
                division = String.Empty
            End If
            If region.ToLower = "all" Then
                region = String.Empty
            End If
            If facility.ToLower = "all" Or facility = "9998" Then
                facility = String.Empty
            End If
            If busUnit.ToLower = "all" Then
                busUnit = String.Empty
            End If
            If area.ToLower = "all" Then
                area = String.Empty
            End If

            Dim siteBll As TaskTrackerSiteBll = New TaskTrackerSiteBll()
            'Dim BusinessUnitAreaDT As TaskTrackerSiteDAL.MTTBusinessUnitAreaDataTable = SiteBLL.GetBusinessUnitArea(Division, Region, Facility)
            'Dim lineList As System.Collections.Generic.List(Of Line) = siteBll.GetLine(division, region, facility, busUnit, area)

            Dim lineList As System.Collections.Generic.List(Of Line) = CType(HelperDal.GetDataFromCache("LineList", 1), Global.System.Collections.Generic.List(Of Global.IP.MEAS.BO.Line))
            If lineList Is Nothing OrElse lineList.Count = 0 Then
                lineList = siteBll.GetLine(division, region, facility, busUnit, area)
                If lineList IsNot Nothing AndAlso lineList.Count > 0 Then
                    'HelperDal.InsertDataIntoCache("LineList", 180, lineList)
                End If
            End If

            If lineList IsNot Nothing Then

                If contextKey.ToLower = "all" Then
                    contextKey = String.Empty
                End If
                values.Insert(0, New CascadingDropDownNameValue(IP.Bids.SharedFunctions.LocalizeValue("All"), "All", False))

                For Each item As Line In lineList
                    Dim val As String = SharedFunctions.DataClean(item.Line)
                    Dim desc As String = SharedFunctions.DataClean(item.Line)
                    If contextKey.Length > 0 And contextKey <> "AL" Then
                        'If contextKey.Contains(val) Then
                        values.Add(New CascadingDropDownNameValue(desc, val))
                        'End If
                    Else
                        'Remove All
                        If val.ToLower <> "al" AndAlso val.ToLower <> "all" Then
                            values.Add(New CascadingDropDownNameValue(desc, val))
                        End If
                    End If
                Next
            End If
            If values.Count > 0 Then
                If contextKey.Length > 0 Then
                    If values.FindIndex(Function(o) o.value = contextKey) > 0 Then
                        values.Item(values.FindIndex(Function(o) o.value = contextKey)).isDefaultValue = True
                    End If
                End If
            End If
        Catch ex As Exception
            SharedFunctions.HandleError("GetLineList", , ex)
        Finally
            GetLineList = SharedFunctions.LocalizeDropDownNameValue(values.ToArray)

        End Try

    End Function

    <Services.WebMethod(cacheduration:=0, enableSession:=True), Script.Services.ScriptMethod()> _
Public Function GetLineBreakList(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()
        Dim values As New Generic.List(Of CascadingDropDownNameValue)
        Dim knownCategoryValuesDictionary As New StringDictionary
        Dim division As String = String.Empty
        Dim region As String = String.Empty
        Dim facility As String = String.Empty
        Dim busUnit As String = String.Empty
        Dim area As String = String.Empty
        Dim line As String = String.Empty
        Try
            If contextKey Is Nothing Then
                contextKey = String.Empty
            End If
            knownCategoryValues = SharedFunctions.CleanKnownCategoryValues(knownCategoryValues)
            knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
            If Not (knownCategoryValuesDictionary.ContainsKey("Division") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("Division") Then
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Division"))
            Else
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Region") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("Region") Then
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Region"))
            Else
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Facility") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("Facility") Then
                facility = SharedFunctions.DataClean(knownCategoryValuesDictionary("Facility"))
            Else
                facility = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Businessunit") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("Businessunit") Then
                busUnit = SharedFunctions.DataClean(knownCategoryValuesDictionary("Businessunit"))
            Else
                busUnit = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Area") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("Area") Then
                area = SharedFunctions.DataClean(knownCategoryValuesDictionary("Area"))
            Else
                area = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Line") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("Line") Then
                line = SharedFunctions.DataClean(knownCategoryValuesDictionary("Line"))
            Else
                line = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If division.ToLower = "all" Then
                division = String.Empty
            End If
            If region.ToLower = "all" Then
                region = String.Empty
            End If
            If facility.ToLower = "all" Or facility = "9998" Then
                facility = String.Empty
            End If
            If busUnit.ToLower = "all" Then
                busUnit = String.Empty
            End If
            If area.ToLower = "all" Then
                area = String.Empty
            End If
            If line.ToLower = "all" Then
                line = String.Empty
            End If

            Dim siteBll As TaskTrackerSiteBll = New TaskTrackerSiteBll()
            'Dim BusinessUnitAreaDT As TaskTrackerSiteDAL.MTTBusinessUnitAreaDataTable = SiteBLL.GetBusinessUnitArea(Division, Region, Facility)
            'Dim lineList As System.Collections.Generic.List(Of LineBreak) = siteBll.GetlineBreak(division, region, facility, busUnit, area, line)
            Dim lineList As System.Collections.Generic.List(Of LineBreak) = CType(HelperDal.GetDataFromCache("LineBreakList", 1), Global.System.Collections.Generic.List(Of Global.IP.MEAS.BO.LineBreak))
            If lineList Is Nothing OrElse lineList.Count = 0 Then
                lineList = siteBll.GetlineBreak(division, region, facility, busUnit, area, line)
                If lineList IsNot Nothing AndAlso lineList.Count > 0 Then
                    'HelperDal.InsertDataIntoCache("LineBreakList", 180, lineList)
                End If
            End If
            If lineList IsNot Nothing Then

                If contextKey.ToLower = "all" Then
                    contextKey = String.Empty
                End If
                values.Add(New CascadingDropDownNameValue(SharedFunctions.LocalizeValue("All"), "All"))

                For Each item As LineBreak In lineList
                    Dim val As String = SharedFunctions.DataClean(item.LineBreak)
                    Dim desc As String = SharedFunctions.DataClean(item.LineBreak)
                    If contextKey.Length > 0 And contextKey <> "AL" Then
                        'If contextKey.Contains(val) Then
                        values.Add(New CascadingDropDownNameValue(desc, val))
                        'End If
                    Else
                        'Remove All
                        If val.ToLower <> "al" AndAlso val.ToLower <> "all" Then
                            values.Add(New CascadingDropDownNameValue(desc, val))
                        End If
                    End If
                Next
            End If
            If values.Count > 0 Then
                If contextKey.Length > 0 Then
                    If values.FindIndex(Function(o) o.value = contextKey) > 0 Then
                        values.Item(values.FindIndex(Function(o) o.value = contextKey)).isDefaultValue = True
                    End If
                End If
            End If
        Catch ex As Exception
            SharedFunctions.HandleError("GetLineBreakList", , ex)
        Finally
            GetLineBreakList = SharedFunctions.LocalizeDropDownNameValue(values.ToArray, "-")

        End Try

    End Function

    <Services.WebMethod(cacheduration:=0, enableSession:=True), Script.Services.ScriptMethod()> _
  Public Function GetBusinessUnitAreaList(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()
        Dim values As New Generic.List(Of CascadingDropDownNameValue)
        Dim knownCategoryValuesDictionary As New StringDictionary
        Dim division As String = String.Empty
        Dim region As String = String.Empty
        Dim facility As String = String.Empty
        Try
            If contextKey Is Nothing Then
                contextKey = String.Empty
            End If
            knownCategoryValues = SharedFunctions.CleanKnownCategoryValues(knownCategoryValues)
            knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
            If Not (knownCategoryValuesDictionary.ContainsKey("Division") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                division = "All"
            ElseIf knownCategoryValuesDictionary.ContainsKey("Division") Then
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Division"))
            Else
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Region") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                region = "All"
            ElseIf knownCategoryValuesDictionary.ContainsKey("Region") Then
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Region"))
            Else
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Facility") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("Facility") Then
                facility = SharedFunctions.DataClean(knownCategoryValuesDictionary("Facility"))
            Else
                facility = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If division.ToLower = "all" Then
                division = String.Empty
            End If
            If region.ToLower = "all" Then
                region = String.Empty
            End If
            If facility.ToLower = "all" Then
                facility = String.Empty
            End If
            Dim siteBll As TaskTrackerSiteBll = New TaskTrackerSiteBll()
            'Dim BusinessUnitAreaDT As TaskTrackerSiteDAL.MTTBusinessUnitAreaDataTable = SiteBLL.GetBusinessUnitArea(Division, Region, Facility)
            'Dim businessUnitAreaList As System.Collections.Generic.List(Of BusinessUnitArea) = siteBll.GetBusinessUnitArea(division, region, facility)
            Dim businessUnitAreaList As System.Collections.Generic.List(Of BusinessUnitArea) = CType(HelperDal.GetDataFromCache("BusinessUnitAreaList", 1), Global.System.Collections.Generic.List(Of Global.IP.MEAS.BO.BusinessUnitArea))
            If businessUnitAreaList Is Nothing OrElse businessUnitAreaList.Count = 0 Then
                businessUnitAreaList = siteBll.GetBusinessUnitArea(division, region, facility)
                If businessUnitAreaList IsNot Nothing AndAlso businessUnitAreaList.Count > 0 Then
                    'HelperDal.InsertDataIntoCache("BusinessUnitAreaList", 180, businessUnitAreaList)
                End If
            End If

            If businessUnitAreaList IsNot Nothing Then
                If contextKey = "All" Then
                    'values.Add(New CascadingDropDownNameValue(SharedFunctions.LocalizeValue("All"), "AL"))
                    contextKey = String.Empty
                End If

                For Each item As BusinessUnitArea In businessUnitAreaList
                    Dim val As String = SharedFunctions.DataClean(item.BusinessUnit)
                    Dim desc As String = SharedFunctions.DataClean(item.BusinessUnit)
                    If contextKey.Length > 0 And contextKey <> "AL" Then
                        'If contextKey.Contains(val) Then
                        values.Add(New CascadingDropDownNameValue(desc, val))
                        'End If
                    Else
                        'Remove All
                        If val.ToLower <> "al" AndAlso val.ToLower <> "all" Then
                            values.Add(New CascadingDropDownNameValue(desc, val))
                        End If
                    End If
                Next
            End If
            If values.Count > 0 Then
                If contextKey.Length > 0 Then
                    If values.FindIndex(Function(o) o.value = contextKey) > 0 Then
                        values.Item(values.FindIndex(Function(o) o.value = contextKey)).isDefaultValue = True
                    End If
                End If
            End If
        Catch ex As Exception
            SharedFunctions.HandleError("GetBusinessUnitAreaList", , ex)
        Finally
            GetBusinessUnitAreaList = SharedFunctions.LocalizeDropDownNameValue(values.ToArray, "-")

        End Try

    End Function

    <Services.WebMethod(cacheduration:=0, enableSession:=True), Script.Services.ScriptMethod()> _
  Public Function GetLineLineBreakList(ByVal knownCategoryValues As String, ByVal category As String, ByVal contextKey As String) As CascadingDropDownNameValue()
        Dim values As New Generic.List(Of CascadingDropDownNameValue)
        Dim knownCategoryValuesDictionary As New StringDictionary
        Dim division As String = String.Empty
        Dim region As String = String.Empty
        Dim facility As String = String.Empty
        Dim businessUnitArea As String = String.Empty

        Try
            If contextKey Is Nothing Then
                contextKey = String.Empty
            End If
            knownCategoryValues = SharedFunctions.CleanKnownCategoryValues(knownCategoryValues)
            knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
            If Not (knownCategoryValuesDictionary.ContainsKey("Division") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                division = "All"
            ElseIf knownCategoryValuesDictionary.ContainsKey("Division") Then
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Division"))
            Else
                division = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Region") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                region = "All"
            ElseIf knownCategoryValuesDictionary.ContainsKey("Region") Then
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Region"))
            Else
                region = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("Facility") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                facility = ""
            ElseIf knownCategoryValuesDictionary.ContainsKey("Facility") Then
                facility = SharedFunctions.DataClean(knownCategoryValuesDictionary("Facility"))
            Else
                facility = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If Not (knownCategoryValuesDictionary.ContainsKey("BusinessUnitArea") Or knownCategoryValuesDictionary.ContainsKey("Undefined")) Then
                Return Nothing
            ElseIf knownCategoryValuesDictionary.ContainsKey("BusinessUnitArea") Then
                businessUnitArea = SharedFunctions.DataClean(knownCategoryValuesDictionary("BusinessUnitArea"))
            Else
                businessUnitArea = SharedFunctions.DataClean(knownCategoryValuesDictionary("Undefined"))
            End If

            If division.ToLower = "all" Then
                division = String.Empty
            End If
            If region.ToLower = "all" Then
                region = String.Empty
            End If
            If facility.ToLower = "all" Then
                facility = String.Empty
            End If
            If businessUnitArea.ToLower = "all" Then
                businessUnitArea = String.Empty
            End If
            Dim siteBll As TaskTrackerSiteBll = New TaskTrackerSiteBll()

            'Dim LineLineBreakDT As TaskTrackerSiteDAL.MTTLineLineBreakDataTable = SiteBLL.GetLineLineBreak(Division, Region, Facility, BusinessUnitArea)
            'Dim lineLineBreakList As System.Collections.Generic.List(Of LineLineBreak) = siteBll.GetLineLineBreak(division, region, facility, businessUnitArea)
            Dim lineLineBreakList As System.Collections.Generic.List(Of LineLineBreak) = CType(HelperDal.GetDataFromCache("LineLineBreakList", 1), Global.System.Collections.Generic.List(Of Global.IP.MEAS.BO.LineLineBreak))
            If lineLineBreakList Is Nothing OrElse lineLineBreakList.Count = 0 Then
                lineLineBreakList = siteBll.GetLineLineBreak(division, region, facility, businessUnitArea)
                If lineLineBreakList IsNot Nothing AndAlso lineLineBreakList.Count > 0 Then
                    'HelperDal.InsertDataIntoCache("LineLineBreakList", 180, lineLineBreakList)
                End If
            End If

            If lineLineBreakList IsNot Nothing Then

                If contextKey = "All" Then
                    'values.Add(New CascadingDropDownNameValue(SharedFunctions.LocalizeValue("All"), "AL"))
                    contextKey = String.Empty
                End If

                For Each item As LineLineBreak In lineLineBreakList
                    Dim val As String = SharedFunctions.DataClean(item.LineBreak)
                    Dim desc As String = SharedFunctions.DataClean(item.LineBreak)
                    If contextKey.Length > 0 And contextKey <> "AL" Then
                        'If contextKey.Contains(val) Then
                        values.Add(New CascadingDropDownNameValue(desc, val))
                        'End If
                    Else
                        'Remove All
                        If val.ToLower <> "al" AndAlso val.ToLower <> "all" Then
                            values.Add(New CascadingDropDownNameValue(desc, val))
                        End If
                    End If

                Next
            End If
            If values.Count > 0 Then
                If contextKey.Length > 0 Then
                    If values.FindIndex(Function(o) o.value = contextKey) > 0 Then
                        values.Item(values.FindIndex(Function(o) o.value = contextKey)).isDefaultValue = True
                    End If
                End If
            End If
        Catch ex As Exception
            SharedFunctions.HandleError("GetBusinessUnitAreaList", , ex)
        Finally
            GetLineLineBreakList = SharedFunctions.LocalizeDropDownNameValue(values.ToArray, "-")

        End Try

    End Function


    <Services.WebMethod()> _
<Script.Services.ScriptMethod()> _
 Public Function GetEmployeeCompletionList(ByVal prefixText As String, ByVal dataControlId As String) As String()
        Dim userRoleList As System.Collections.Generic.List(Of EmployeeProfile)
        If prefixText.Length > 0 Then
            userRoleList = GeneralTaskTrackerBll.GetUserDataByPrefix(prefixText)
            Dim items As New List(Of String)(userRoleList.Count + 1)
            items.Add(dataControlId)
            If userRoleList IsNot Nothing Then
                For i As Integer = 0 To userRoleList.Count - 1
                    items.Add((userRoleList.Item(i).FirstName.Trim & " " & userRoleList.Item(i).LastName.Trim).PadRight(30, CChar(" ")) & " (" & userRoleList.Item(i).Domain & ": " & userRoleList.Item(i).SiteName & ")$" & userRoleList.Item(i).PlantCode & "|" & userRoleList.Item(i).NetworkId)
                Next
            End If
            Return items.ToArray()
        Else
            Return Nothing
        End If

    End Function

    <Services.WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function GetDDLData(ByVal plantCode As String, ByVal userControlId As String, ByVal userMode As String, ByVal showInactiveUsers As Boolean) As Collections.Generic.List(Of ListItem)
        Dim taskItem As New TaskTrackerListsBll
        Dim userList As System.Collections.Generic.List(Of ResponsibleUsers) '= TaskTrackerListsBll.GetResponsibleUsers(plantCode)
        Dim roleDescription As String = String.Empty
        Dim ddlList As New Collections.Generic.List(Of ListItem)
        Dim currentUserMode As Integer = 0
        roleDescription = String.Empty
        ddlList.Add(New ListItem(userControlId, CStr(0)))


        Select Case userMode.Trim
            Case "0" 'Users and Roles
                currentUserMode = 0
            Case "1" 'Users Only
                currentUserMode = 1
            Case "2" 'Roles Only
                currentUserMode = 2
            Case Else 'Users and Roles
                currentUserMode = 0
        End Select

        If plantCode = "9998" OrElse currentUserMode = 2 Then
            userList = TaskTrackerListsBll.GetResponsibleRoles
        Else
            userList = TaskTrackerListsBll.GetResponsibleUsers(plantCode)
        End If

        For Each item As ResponsibleUsers In userList
            Dim spaceChar As String = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
            With ddlList
                If showInactiveUsers = False Then
                    If item.InActiveFlag.ToLower = "y" Then Continue For
                End If
                If currentUserMode = 1 Then
                    'If Me.AllowUserRoles = False Then
                    If item.RoleSeqID <> "-1" And item.RoleSeqID.Length > 0 Then
                        Continue For
                    End If
                ElseIf currentUserMode = 2 Then
                    If item.RoleSeqID.Length = 0 Then
                        Continue For
                    End If
                End If
                If item.RoleDescription <> roleDescription Then 'New Group
                    Dim roleItem As New ListItem
                    roleDescription = item.RoleDescription
                    roleItem.Text = item.RoleDescription.ToUpper
                    roleItem.Value = item.RoleSeqID

                    If ddlList.Count = 1 Then
                        Dim blankItem As New ListItem
                        With blankItem
                            .Attributes.Add("disabled", "true")
                            .Text = ""
                            .Value = -1 & item.RoleSeqID
                        End With
                        ddlList.Add(blankItem)
                    End If

                    If currentUserMode = 0 Or currentUserMode = 2 Then
                        roleItem.Attributes.Add("style", "color:black; font-size:Larger;")
                        roleItem.Attributes.Add("data-icon", "glyphicon-user")
                        If roleDescription.Length = 0 Then
                            roleItem.Attributes.Add("disabled", "true")
                        End If
                        If item.RoleSeqID.Length = 0 Then
                            roleItem.Attributes.Add("disabled", "true")
                        End If

                        ddlList.Add(roleItem)
                    End If

                End If

                If currentUserMode = 0 Then
                    Dim useritem As New ListItem
                    With useritem
                        .Text = Server.HtmlDecode(spaceChar & SharedFunctions.ToTitleCase(item.FullName.ToLower)) & " (" & item.UserName & ")"
                        .Value = item.UserName
                    End With
                    .Add(useritem)
                ElseIf currentUserMode = 1 And (item.RoleSeqID.Length = 0 OrElse item.RoleSeqID = "-1") Then
                    Dim useritem As New ListItem
                    With useritem
                        .Text = Server.HtmlDecode(spaceChar & SharedFunctions.ToTitleCase(item.FullName.ToLower)) & " (" & item.UserName & ")"
                        .Value = item.UserName
                    End With
                    .Add(useritem)
                End If
            End With
        Next

        Return ddlList
    End Function

    <Services.WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function GetUserList(ByVal plantCode As String, ByVal userControlId As String, ByVal userMode As String, ByVal userName As String, ByVal showInactiveUsers As Boolean) As Collections.Generic.List(Of ListItem)
        'Dim taskItem As New TaskTrackerListsBll
        Dim userList As System.Collections.Generic.List(Of ResponsibleUsers) = TaskTrackerListsBll.GetResponsibleUsers(plantCode)
        Dim roleDescription As String = String.Empty
        Dim ddlList As New Collections.Generic.List(Of ListItem)
        Dim currentUserMode As Integer = 0
        roleDescription = String.Empty
        ddlList.Add(New ListItem(userControlId, CStr(0)))
        ddlList.Add(New ListItem(userName, CStr(0)))

        Select Case userMode.Trim
            Case "0" 'Users and Roles
                currentUserMode = 0
            Case "1" 'Users Only
                currentUserMode = 1
            Case "2" 'Roles Only
                currentUserMode = 2
            Case Else 'Users and Roles
                currentUserMode = 0
        End Select
        For Each item As ResponsibleUsers In userList
            Dim spaceChar As String = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
            With ddlList
                If showInactiveUsers = False Then Continue For
                If currentUserMode = 1 Then
                    'If Me.AllowUserRoles = False Then
                    If item.RoleSeqID.Length > 0 Then
                        Continue For
                    End If
                ElseIf currentUserMode = 2 Then
                    If item.RoleSeqID.Length = 0 Then
                        Continue For
                    End If
                End If
                If item.RoleDescription <> roleDescription Then 'New Group
                    Dim roleItem As New ListItem
                    roleDescription = item.RoleDescription
                    roleItem.Text = item.RoleDescription.ToUpper
                    roleItem.Value = item.RoleSeqID

                    If ddlList.Count = 1 Then
                        Dim blankItem As New ListItem
                        With blankItem
                            .Attributes.Add("disabled", "true")
                            .Text = ""
                            .Value = -1 & item.RoleSeqID
                        End With
                        ddlList.Add(blankItem)
                    End If
                    If currentUserMode = 0 Or currentUserMode = 2 Then
                        roleItem.Attributes.Add("style", "color:black; font-size:Larger;background-color:lightgray")
                        roleItem.Attributes.Add("data-icon", "glyphicon-user")
                        If roleDescription.Length = 0 Then
                            roleItem.Attributes.Add("disabled", "true")
                        End If
                        If item.RoleSeqID.Length = 0 Then
                            roleItem.Attributes.Add("disabled", "true")
                        End If

                        ddlList.Add(roleItem)
                    End If

                End If

                If currentUserMode = 0 Then
                    Dim useritem As New ListItem
                    With useritem
                        .Text = Server.HtmlDecode(spaceChar & SharedFunctions.ToTitleCase(item.FullName.ToLower)) & " (" & item.UserName & ")"
                        .Value = item.UserName
                    End With
                    .Add(useritem)
                ElseIf currentUserMode = 1 And (item.RoleSeqID.Length = 0 OrElse item.RoleSeqID = "-1") Then
                    Dim useritem As New ListItem
                    With useritem
                        .Text = Server.HtmlDecode(spaceChar & IP.Bids.SharedFunctions.ToTitleCase(item.FullName.ToLower)) & " (" & item.UserName & ")"
                        .Value = item.UserName
                    End With
                    .Add(useritem)
                End If
            End With
        Next

        Return ddlList
    End Function

End Class
