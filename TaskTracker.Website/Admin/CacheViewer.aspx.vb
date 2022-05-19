'***********************************************************************
' Assembly         : CacheViewer.aspx.vb
' Author           : Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By : Michael J Pope Jr
' Last Modified On : 08-20-2010
' Description      : The purpose of this page is to provide the ability to view and clear data that is currently stored in cache.
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Option Explicit On
Option Strict Off
Imports System.Data
Imports System.Globalization

Partial Public Class AdminDisplayCache
    Inherits IP.Bids.BasePage

#Region "Private Events"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Master.PageName = IP.Bids.SharedFunctions.LocalizeValue("Cache Viewer", False)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim sb As New StringBuilder
        'Dim insertSQL As String = "INSERT INTO ""RELADMIN"".""TBLRESOURCES"" (RESOURCETYPEID, LOCALEID, RESOURCEVALUE, APPLICATIONID, RESOURCEKEY) VALUES ('2', '3', '{0}', '61', '{0}');"
        Dim insertSQL As String = "INSERT INTO ""RELADMIN"".""TBLRESOURCES"" (RESOURCETYPEID, LOCALEID, RESOURCEVALUE, APPLICATIONID, RESOURCEKEY, LASTUPDATEDATE, LASTUPDATEUSERNAME) VALUES ('2', '3', 'TRANSLATE: {0}', '61', '{0}',TO_DATE('" & IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(Now, "EN-US", "dd-MMM-yy") & "', 'DD-MON-RR'),'" & IP.Bids.SharedFunctions.GetCurrentUser.Username & "');<br>"
        Page.EnableViewState = False
        Dim url = Page.ResolveClientUrl("~/Admin/DeleteAllCache.aspx")
        _btnDeleteAll.OnClientClick = String.Format("window.location='{0}';return false", url)
        If Not Page.IsPostBack Then
            GetAndDisplayCacheItems()

            For Each item As DictionaryEntry In Master.IPResources.GetMissingResourceKeys
                sb.AppendLine(String.Format(insertSQL, item.Key)) '(item.Key & "-" & item.Value)
            Next
        Else
            'These have to be bound to fire the event. This method 
            'Bind the link buttons, but without the rest of the over head 
            CreateAndBindRemoveButtons()
        End If
        '_txtMissingKeyInsert.Text = sb.ToString
        'Response.Write(Master.IPResources.ExportMissingToExcel()) 'ExportToResourceFile("c:\Personal Documents\")
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="buttonID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateRemoveLinkButton(ByVal buttonID As String) As LinkButton
        Dim removeLinkButton As New LinkButton()
        removeLinkButton.Text = "Remove"
        removeLinkButton.Font.Bold = True
        'Set the ID of the LinkButton to the Cache Key Name. 
        'This is going to be used in the eventhandler, to remove item from cache 
        removeLinkButton.ID = buttonID
        Dim url = Page.ResolveClientUrl(String.Format("~/Admin/DeleteAllCache.aspx?key={0}", buttonID))
        removeLinkButton.OnClientClick = String.Format("window.location='{0}';return false", url)
        AddHandler removeLinkButton.Click, AddressOf RemoveLinkButton_Click
        'Wire up event 
        Return removeLinkButton
    End Function

    Private Sub RemoveLinkButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Server.Transfer(String.Format("~/Admin/DeleteAllCache.aspx?key={0}", CType(sender.ID, String)))
    End Sub

    Protected Sub _btnDeleteAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnDeleteAll.Click
        Server.Transfer("~/Admin/DeleteAllCache.aspx")
    End Sub

    Protected Sub _btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnRefresh.Click
        Me.phTable.Controls.Clear()
        GetAndDisplayCacheItems()
    End Sub
#End Region

#Region "Methods"
    ''' <summary>
    ''' Purpose is to display the cached items in an easy to view format
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetAndDisplayCacheItems()
        Try
            phTable.Controls.Clear()
            'this stringbuilder gets used throughout the method as needed. 
            Dim sb As New StringBuilder()

            'Grab the context item, for reference so it has local scope to 
            'save having to get walk back and get the Context item 
            Dim ctx As HttpContext = HttpContext.Current
            '        Dim xlist As Collections.SortedList = Nothing ' COMMENTED BY CODEIT.RIGHT
            Dim myCache As New SortedList
            Dim objItem As Object
            Dim strName As String = String.Empty
            Dim cacheEnumerator As IDictionaryEnumerator  'ctx.Cache.GetEnumerator()

            If ctx IsNot Nothing Then
                For Each objItem In ctx.Cache
                    strName = objItem.Key
                    If myCache.Contains(strName) = False And Not strName.Contains("System.Web.Script.Services.WebServiceData") Then
                        myCache.Add(strName, ctx.Cache(strName).GetType().ToString())
                    End If
                Next
            End If
            cacheEnumerator = myCache.GetEnumerator
            While cacheEnumerator.MoveNext()
                ' Create HTML table for display of each item in cache 
                Dim t As New HtmlTable()
                'Set General Table Properties 
                t.BorderColor = "000000"
                t.CellPadding = 10
                t.CellSpacing = 0
                t.Border = 1
                t.Width = Unit.Percentage(98).ToString

                Dim r As New HtmlTableRow()
                Dim cacheKeyTableCell As New HtmlTableCell()
                cacheKeyTableCell.VAlign = "top"

                'Clear the stringbuilder and add the CachItem Type and value 
                sb.Length = 0
                sb.Append("<B>Cache Item Type: </B>")
                sb.Append(ctx.Cache(cacheEnumerator.Key.ToString()).[GetType]().ToString())
                sb.Append("<BR /><B>Value: </B>")
                sb.Append(Server.HtmlEncode(cacheEnumerator.Value.ToString()))

                'Add the Item Type 
                cacheKeyTableCell.Controls.Add(New LiteralControl(sb.ToString()))
                r.Cells.Add(cacheKeyTableCell)

                Dim r2 As New HtmlTableRow
                Dim cacheValueTableCell As New HtmlTableCell()
                Select Case cacheEnumerator.Value.ToString()
                    Case "System.Data.DataSet"
                        'This is the fun code, that turns dataset tables, 
                        'into HTML Tables 
                        'Coutner to Create a label 
                        Dim tblCounter As Integer = 0
                        'Create a new dataset to be used as a copy of the cached dataset 
                        'We do this, becuase some of the values get manipulated and we 
                        'don't want to chagne the actual values. 
                        Dim ds As New DataSet()
                        'Get copy of dataset from Cache 
                        ds = DirectCast(ctx.Cache(cacheEnumerator.Key.ToString()), DataSet).Copy()
                        'Iterate throguht the tables in the dataset 
                        For Each dt As DataTable In ds.Tables
                            'Set flag to see if the table has rows. 
                            'We won't add empty tables to the view 
                            Dim rowsProcessed As Boolean = False
                            For Each dr As DataRow In dt.Rows
                                'Found rows, set flag to true 
                                rowsProcessed = True
                                For x As Integer = 0 To dr.ItemArray.GetLength(0) - 1
                                    'loop through each colomn int he DataRow and convert 
                                    'any text, to HTML text. A lot of cached data, is data that 
                                    'gets rendered as XML, or HTML, so this makes the data human 
                                    'readable. This is the reason for working with a copy of the 
                                    'dataset, and not the real cached item. 
                                    'This is my sloppy way to catch nulls, and any other 
                                    'conversion errors. 
                                    If dr(x) IsNot Nothing AndAlso Not dr(x) Is DBNull.Value Then
                                        Dim columnData As String = Server.HtmlEncode(dr(x).ToString())
                                        dr(x) = columnData
                                    End If


                                Next
                            Next
                            'Add populated tables to ValueTableCell, via 
                            'new datagrid.AutoGenerateColumns 
                            If rowsProcessed Then
                                'Add Seperation bar between Tables 
                                If tblCounter > 0 Then
                                    cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                                End If
                                Dim dg As New DataGrid()
                                dg.CssClass = "Border"
                                dg.GridLines = GridLines.Both
                                dg.CellSpacing = 2
                                dg.AutoGenerateColumns = True
                                dg.BorderColor = Drawing.Color.Black
                                dg.BorderWidth = Unit.Pixel(2)
                                dg.HeaderStyle.CssClass = "BorderSecondary"
                                dg.HeaderStyle.Font.Bold = True
                                dg.DataSource = dt
                                dg.DataBind()
                                dg.Width = Unit.Percentage(100)
                                'Set some styles on the columns for readability 
                                For Each c As DataGridColumn In dg.Columns
                                    c.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                                    c.ItemStyle.VerticalAlign = VerticalAlign.Top
                                Next
                                'Add a Table Counter Label 
                                sb.Length = 0
                                sb.Append("DataTable ")
                                sb.Append(tblCounter.ToString(CultureInfo.CurrentCulture))
                                sb.Append("<BR />")
                                cacheValueTableCell.Controls.Add(New LiteralControl(sb.ToString()))
                                cacheValueTableCell.Controls.Add(dg)
                            End If
                            tblCounter += 1
                        Next
                        r2.Cells.Add(cacheValueTableCell)
                        ds = Nothing
                        Exit Select
                    Case "System.Collections.Generic.List"
                        Dim gv As New GridView
                        gv.DataSource = ctx.Cache(cacheEnumerator.Key.ToString())
                        gv.DataBind()
                        gv.GridLines = GridLines.Both
                        gv.CellSpacing = 2
                        cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                        cacheValueTableCell.Controls.Add(gv)
                        r2.Cells.Add(cacheValueTableCell)
                        gv = Nothing
                    Case "IP.MEASFramework.ResourceProvider.ResourceDataDictionary"
                        Dim list As IP.MEASFramework.ResourceProvider.ResourceDataDictionary = TryCast(ctx.Cache(cacheEnumerator.Key.ToString()), IP.MEASFramework.ResourceProvider.ResourceDataDictionary)
                        '  ds = DirectCast(ctx.Cache(d.Key.ToString()), DataSet).Copy()
                        If list IsNot Nothing Then
                            Dim dt As New DataTable(cacheEnumerator.Key.ToString)
                            dt.Columns.Add(New DataColumn("Resource Key"))
                            dt.Columns.Add(New DataColumn("Resource Type"))
                            dt.Columns.Add(New DataColumn("Resource Value"))
                            For Each lsd As DictionaryEntry In list
                                Dim newRow As DataRow
                                Dim data As IP.MEASFramework.ResourceProvider.ResourceDataItem = TryCast(lsd.Value, IP.MEASFramework.ResourceProvider.ResourceDataItem)
                                newRow = dt.NewRow
                                newRow.Item("Resource Key") = data.ResourceKey
                                newRow.Item("Resource Type") = data.ResourceType
                                newRow.Item("Resource Value") = data.ResourceValue
                                dt.Rows.Add(newRow)
                            Next
                            Dim gv As New GridView
                            dt.DefaultView.Sort = "[Resource Type], [Resource Key]"
                            gv.DataSource = dt.DefaultView
                            gv.DataBind()
                            gv.GridLines = GridLines.Both
                            gv.CellSpacing = 2
                            'Dim ls As New Web.UI.WebControls.RadioButtonList

                            'ls.DataSource = list
                            'ls.DataTextField = "Key"
                            'ls.DataValueField = "Value"
                            'ls.DataBind()
                            cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                            cacheValueTableCell.Controls.Add(gv)
                            r2.Cells.Add(cacheValueTableCell)
                            gv = Nothing
                        End If

                    Case "System.Collections.SortedList"
                        Dim list As SortedList = TryCast(ctx.Cache(cacheEnumerator.Key.ToString()), SortedList)
                        '  ds = DirectCast(ctx.Cache(d.Key.ToString()), DataSet).Copy()
                        If list IsNot Nothing Then
                            Dim dt As New DataTable(cacheEnumerator.Key.ToString)
                            dt.Columns.Add(New DataColumn("Key"))
                            dt.Columns.Add(New DataColumn("Resource Type"))

                            For Each lsd As DictionaryEntry In list
                                Dim newRow As DataRow
                                newRow = dt.NewRow
                                newRow.Item("Key") = lsd.Key
                                newRow.Item("Resource Type") = lsd.Value
                                dt.Rows.Add(newRow)
                            Next
                            Dim gv As New GridView
                            gv.DataSource = dt
                            gv.DataBind()
                            gv.GridLines = GridLines.Both
                            gv.CellSpacing = 2
                            'Dim ls As New Web.UI.WebControls.RadioButtonList

                            'ls.DataSource = list
                            'ls.DataTextField = "Key"
                            'ls.DataValueField = "Value"
                            'ls.DataBind()
                            cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                            cacheValueTableCell.Controls.Add(gv)
                            r2.Cells.Add(cacheValueTableCell)
                            gv = Nothing
                        End If
                    Case "System.String"
                        Dim xml As String = String.Empty
                        'Try
                        xml = SerializeAnObject(ctx.Cache(cacheEnumerator.Key.ToString()))
                        'Catch
                        'Server.ClearError()
                        'xml = String.Empty
                        'End Try

                        cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                        Dim lb As New Label
                        lb.Text = xml
                        cacheValueTableCell.Controls.Add(lb)
                        r2.Cells.Add(cacheValueTableCell)
                    Case "System.Collections.Hashtable"
                        Dim list As Hashtable = CType(ctx.Cache(cacheEnumerator.Key.ToString()), Hashtable)
                        Dim sbHashTable As New StringBuilder
                        sbHashTable.Append("<ul><li>Key -- Value</li>")
                        For Each de As DictionaryEntry In list
                            sbHashTable.Append("<li>")
                            sbHashTable.Append(de.Key)
                            sbHashTable.Append("--")
                            sbHashTable.Append(de.Value)
                            sbHashTable.Append("</li>")
                        Next
                        Dim lb As New Label
                        lb.Text = sbHashTable.ToString
                        cacheValueTableCell.Controls.Add(lb)
                        r2.Cells.Add(cacheValueTableCell)
                        'Case "IP.Bids.AirEmissionsInventorySelectionData"
                        '    Dim clsAEICollection As IP.Bids.AirEmissionsInventorySelectionData = Nothing
                        '    clsAEICollection = TryCast(ctx.Cache(cacheEnumerator.Key.ToString()), IP.Bids.AirEmissionsInventorySelectionData)

                        '    With clsAEICollection
                        '        If .BusinessGroup IsNot Nothing AndAlso .BusinessGroup.Count > 0 Then
                        '            Dim gvBusinessGroup As New GridView
                        '            gvBusinessGroup.DataSource = .BusinessGroup
                        '            gvBusinessGroup.DataBind()
                        '            gvBusinessGroup.GridLines = GridLines.Both
                        '            gvBusinessGroup.CellSpacing = 2
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("Business Group"))
                        '            cacheValueTableCell.Controls.Add(gvBusinessGroup)
                        '        End If
                        '        If .Facility IsNot Nothing AndAlso .Facility.Count > 0 Then
                        '            Dim gvFacility As New GridView
                        '            gvFacility.DataSource = .Facility
                        '            gvFacility.DataBind()
                        '            gvFacility.GridLines = GridLines.Both
                        '            gvFacility.CellSpacing = 2
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("Facility"))
                        '            cacheValueTableCell.Controls.Add(gvFacility)
                        '        End If
                        '        If .Sources IsNot Nothing AndAlso .Sources.Count > 0 Then
                        '            Dim gvSources As New GridView
                        '            gvSources.DataSource = .Sources
                        '            gvSources.DataBind()
                        '            gvSources.GridLines = GridLines.Both
                        '            gvSources.CellSpacing = 2
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("Sources"))
                        '            cacheValueTableCell.Controls.Add(gvSources)
                        '        End If
                        '        If .SourceFacilities IsNot Nothing AndAlso .SourceFacilities.Count > 0 Then
                        '            Dim gvSourceFacilities As New GridView
                        '            gvSourceFacilities.DataSource = .SourceFacilities
                        '            gvSourceFacilities.DataBind()
                        '            gvSourceFacilities.GridLines = GridLines.Both
                        '            gvSourceFacilities.CellSpacing = 2
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("Source Facilities"))
                        '            cacheValueTableCell.Controls.Add(gvSourceFacilities)
                        '        End If
                        '        If .PollutantCategory IsNot Nothing AndAlso .PollutantCategory.Count > 0 Then
                        '            Dim gvPollutantCategory As New GridView
                        '            gvPollutantCategory.DataSource = .PollutantCategory
                        '            gvPollutantCategory.DataBind()
                        '            gvPollutantCategory.GridLines = GridLines.Both
                        '            gvPollutantCategory.CellSpacing = 2
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("Pollutant Category"))
                        '            cacheValueTableCell.Controls.Add(gvPollutantCategory)
                        '        End If

                        '        If .PollutantList IsNot Nothing AndAlso .PollutantList.Count > 0 Then
                        '            Dim gvPollutantList As New GridView
                        '            gvPollutantList.DataSource = .PollutantList
                        '            gvPollutantList.DataBind()
                        '            gvPollutantList.GridLines = GridLines.Both
                        '            gvPollutantList.CellSpacing = 2
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("Pollutant List"))
                        '            cacheValueTableCell.Controls.Add(gvPollutantList)
                        '        End If

                        '        If .PollutantListFiltered IsNot Nothing AndAlso .PollutantListFiltered.Count > 0 Then
                        '            Dim gvPollutantListFiltered As New GridView
                        '            gvPollutantListFiltered.DataSource = .PollutantListFiltered
                        '            gvPollutantListFiltered.DataBind()
                        '            gvPollutantListFiltered.GridLines = GridLines.Both
                        '            gvPollutantListFiltered.CellSpacing = 2
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("Pollutant List Filtered"))
                        '            cacheValueTableCell.Controls.Add(gvPollutantListFiltered)
                        '        End If

                        '        If .ReportDisplayList IsNot Nothing AndAlso .ReportDisplayList.Count > 0 Then
                        '            Dim gvReportDisplayList As New GridView
                        '            gvReportDisplayList.DataSource = .ReportDisplayList
                        '            gvReportDisplayList.DataBind()
                        '            gvReportDisplayList.GridLines = GridLines.Both
                        '            gvReportDisplayList.CellSpacing = 2
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("Report Display List Filtered"))
                        '            cacheValueTableCell.Controls.Add(gvReportDisplayList)
                        '        End If

                        '        If .ReportList IsNot Nothing AndAlso .ReportList.Count > 0 Then
                        '            Dim gvReportList As New GridView
                        '            gvReportList.DataSource = .ReportList
                        '            gvReportList.DataBind()
                        '            gvReportList.GridLines = GridLines.Both
                        '            gvReportList.CellSpacing = 2
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                        '            cacheValueTableCell.Controls.Add(New LiteralControl("Report List Filtered"))
                        '            cacheValueTableCell.Controls.Add(gvReportList)
                        '        End If
                        '    End With
                        '    r2.Cells.Add(cacheValueTableCell)

                    Case Else
                        Dim isDataSource As Boolean
                        isDataSource = False
                        If ctx.Cache(cacheEnumerator.Key.ToString()).GetType.GetInterface("IListSource") IsNot Nothing Then
                            isDataSource = True
                        ElseIf ctx.Cache(cacheEnumerator.Key.ToString()).GetType.GetInterface("IList") IsNot Nothing Then
                            isDataSource = True '
                        ElseIf ctx.Cache(cacheEnumerator.Key.ToString()).GetType.GetInterface("IDataSource") IsNot Nothing Then
                            isDataSource = True
                        End If
                        If isDataSource = True Then
                            Dim gv As New GridView
                            gv.DataSource = ctx.Cache(cacheEnumerator.Key.ToString())
                            gv.DataBind()
                            gv.GridLines = GridLines.Both
                            gv.CellSpacing = 2
                            cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                            cacheValueTableCell.Controls.Add(gv)
                            r2.Cells.Add(cacheValueTableCell)
                            gv = Nothing
                        Else
                            Dim xml As String = String.Empty
                            'Try
                            xml = SerializeAnObject(ctx.Cache(cacheEnumerator.Key.ToString()))
                            'Catch
                            'Throw
                            'Server.ClearError()
                            'End Try

                            cacheValueTableCell.Controls.Add(New LiteralControl("<HR>"))
                            Dim lb As New Label

                            lb.Text = xml
                            cacheValueTableCell.Controls.Add(lb)
                            r2.Cells.Add(cacheValueTableCell)
                        End If
                End Select

                'Add the row to the HTML table 
                t.Rows.Add(r)
                t.Rows.Add(r2)

                'Start <P> Tag 

                phTable.Controls.Add(New LiteralControl("<table width='100%' border=1 class='help'><tr><td>"))

                'Create and add Remove Link Button 
                phTable.Controls.Add(CreateRemoveLinkButton(cacheEnumerator.Key.ToString()))

                'Add a spacer between the remove link button and the open/close javascript 
                phTable.Controls.Add(New LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;"))

                'Create and add the anchor tag to open/close javascript 
                Dim a As New HtmlAnchor()
                a.Style.Add("font-weight", "Bold")
                a.HRef = "Javascript:OpenOrCloseSpan('span_" + cacheEnumerator.Key.ToString() + "')"
                a.Controls.Add(New LiteralControl("Open/Close"))
                phTable.Controls.Add(a)

                'Create and add the Name of the Cache Item (preeceded with a space) 
                Dim cacheItemDisplayName As New System.Text.StringBuilder()
                cacheItemDisplayName.Append("&nbsp;&nbsp;&nbsp;&nbsp;")
                cacheItemDisplayName.Append(cacheEnumerator.Key.ToString())
                'CacheItemDisplayName.Append("&nbsp;")
                'CacheItemDisplayName.Append(Server.HtmlEncode(d.Value.ToString()))           

                'Add the Open Span Tag and ID for javascript to open and close 
                cacheItemDisplayName.Append("<span id=""span_")
                cacheItemDisplayName.Append(cacheEnumerator.Key.ToString())
                cacheItemDisplayName.Append(""" style=""Display: None"">")
                phTable.Controls.Add(New LiteralControl(cacheItemDisplayName.ToString()))

                'Add the data table to the placeholder 
                phTable.Controls.Add(t)
                'Add the closing Span and P tags. 
                phTable.Controls.Add(New LiteralControl("</span></td></tr></table>"))

            End While
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Purpose is to create the Remove Buttons
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateAndBindRemoveButtons()
        Dim ctx As HttpContext = HttpContext.Current
        Dim d As IDictionaryEnumerator = ctx.Cache.GetEnumerator()
        Try
            While d.MoveNext()
                phTable.Controls.Add(CreateRemoveLinkButton(d.Key.ToString()))
            End While

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("CreateAndBindRemoveButtons", , ex)
        End Try
    End Sub


    ''' <summary> 
    ''' Serialize an object 
    ''' </summary> 
    ''' <param name="obj"></param> 
    ''' <returns></returns> 
    Private Function SerializeAnObject(ByVal obj As Object) As String
        Dim stream As New System.IO.MemoryStream()
        Try
            Dim doc As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            Dim serializer As New System.Xml.Serialization.XmlSerializer(obj.[GetType]())

            serializer.Serialize(stream, obj)
            stream.Position = 0
            doc.Load(stream)
            Return doc.InnerXml
        Catch ex As System.InvalidOperationException ': An error occurred during serialization. The original exception is available using the System.Exception.InnerException property. 
            Return String.Empty
        Catch
            Throw
        Finally
            stream.Close()
            stream.Dispose()
        End Try
    End Function
#End Region

End Class