'***********************************************************************
' Assembly         :http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          :09-08-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On :09-08-2010
' Description      :Contains the business logic needed to store and retrieve documents/links
'
' Copyright        :(c) International Paper. All rights reserved.
' Purpose          :The purpose of the Business Logic Layer is to separate the presentation layer from the data access layer.
'***********************************************************************

Option Explicit On
Option Strict On

Imports Devart.Data.Oracle

Public NotInheritable Class DocumentsAndLinksBll

#Region "Methods"
    ''' <summary>
    ''' Gets a collection of documents and links that have been attached to the specified Task Header or Task Item
    ''' </summary>
    ''' <param name="taskHeaderNumber">String - Task Header Number for the current task</param>
    ''' <param name="taskItem">String - Task Item Number for the current task</param>
    ''' <returns>Returns a collection of documents and links that have been attached to the specified Task Header or Task Item</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDocumentsAndLinks(ByVal taskHeaderNumber As String, ByVal taskItem As String) As System.Collections.Generic.List(Of MttDocuments)
        Dim adapter As DocumentsAndLinksDALTableAdapters.DocumentsAndLinksTableAdapter
        Dim itemDataTable As DocumentsAndLinksDAL.DocumentsAndLinksDataTable = Nothing
        Dim emptyRecord As Object = Nothing
        Dim documentList As System.Collections.Generic.List(Of MttDocuments) = Nothing

        Try
            If documentList Is Nothing OrElse documentList.Count = 0 Then
                adapter = New DocumentsAndLinksDALTableAdapters.DocumentsAndLinksTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                itemDataTable = adapter.GetDocuments(CStr(taskHeaderNumber), CStr(taskItem), emptyRecord)
                documentList = New System.Collections.Generic.List(Of MttDocuments)
                For Each rowItem As DocumentsAndLinksDAL.DocumentsAndLinksRow In itemDataTable.Rows
                    documentList.Add(New MttDocuments(rowItem.TASKHEADERSEQID, rowItem.FILENAME, rowItem.LOCATION, rowItem.DESCRIPTION, rowItem.TASKDOCUMENTSEQID))
                Next
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetDocumentsAndLinks", "Error getting document list", ex)
        Finally
            adapter = Nothing
            itemDataTable = Nothing
        End Try
        Return documentList
        adapter = Nothing
    End Function

    ''' <summary>
    ''' Saves documents and links into the database for the specified Task Header or Task Item
    ''' </summary>
    ''' <param name="documentId">String - Represents the Unique ID for the document that will be updated</param>
    ''' <param name="taskHeaderNumber">String - Task Header Number for the current task</param>
    ''' <param name="taskItemNumber">String - Task Item Number for the current task</param>
    ''' <param name="fileName"></param>
    ''' <param name="location"></param>
    ''' <param name="description"></param>
    ''' <param name="userName"></param>
    ''' <param name="action"></param>
    ''' <remarks></remarks>
    Public Shared Sub SaveDocumentsAndLinks(ByVal documentId As String, ByVal taskHeaderNumber As String, ByVal taskItemNumber As String, ByVal fileName As String, ByVal location As String, ByVal description As String, ByVal userName As String, ByVal action As String)
        Dim documentTA As DocumentsAndLinksDALTableAdapters.DocumentsAndLinksTableAdapter
        Dim statusCode As Decimal
        Try
            documentTA = New DocumentsAndLinksDALTableAdapters.DocumentsAndLinksTableAdapter
            documentTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            documentTA.SaveDocuments(documentId, taskHeaderNumber, taskItemNumber, fileName, location, description, userName.ToUpper, action, statusCode)
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("SaveDocumentsAndLinks", "Error Saving Documents", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Deletes the specified Document or link
    ''' </summary>
    ''' <param name="documentId"></param>
    ''' <param name="userName"></param>
    ''' <param name="action"></param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteDocumentsAndLinks(ByVal documentId As String, ByVal userName As String, ByVal action As String)
        Dim documentTA As DocumentsAndLinksDALTableAdapters.DocumentsAndLinksTableAdapter
        Dim statusCode As Decimal
        Try
            documentTA = New DocumentsAndLinksDALTableAdapters.DocumentsAndLinksTableAdapter
            documentTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            documentTA.DeleteDocument(documentId, "", "", "", "", "", userName, action, statusCode)
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("DeleteDocumentsAndLinks", "Error Deleting document [" & documentId & "]", ex)
        End Try
    End Sub
#End Region
End Class
