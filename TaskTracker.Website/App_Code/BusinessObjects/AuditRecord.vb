Option Explicit On
Option Strict On
Imports System.Linq
Public Class AuditRecord
    Public Property TaskNumber As Integer
    Public Property FullName As String
    Public Property DateEntered As Date
    Public Property Description As String

    Public Sub New(taskNumber As Integer, fullName As String, dateEntered As Date, description As String)
        Me.TaskNumber = taskNumber
        Me.FullName = fullName
        Me.DateEntered = dateEntered
        Me.Description = description
    End Sub
    Public Shared Function GetTaskHistory(ByVal taskNumber As String) As System.Collections.Generic.List(Of AuditRecord)
        Dim ta As New IP.TaskTrackerDAL.AuditLogTableAdapters.AuditRecordsTableAdapter
        Dim ref As New Object
        ta.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
        Dim listOfTankTypes As New System.Collections.Generic.List(Of AuditRecord)
        For Each rowdata As IP.TaskTrackerDAL.AuditLog.AuditRecordsRow In ta.GetAuditRecords(taskNumber, IN_APPLICATION:="MTT", RSAUDITRECORDS:=ref)
            listOfTankTypes.Add(New AuditRecord(CInt(rowdata.RINUMBER), If(rowdata.IsFULLNAMENull, "", rowdata.FULLNAME), CDate(rowdata.UPDATEDATE), If(rowdata.IsDESCRIPTIONNull, "", rowdata.DESCRIPTION)))
        Next

        ta.Dispose()
        Return listOfTankTypes.OrderBy(Function(obj) obj.DateEntered).ToList

    End Function
End Class
