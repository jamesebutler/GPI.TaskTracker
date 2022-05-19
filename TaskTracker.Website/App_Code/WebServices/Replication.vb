Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

''' <summary>
''' 
''' </summary>
<WebService(Namespace:="http://MTT/")> _
<System.Web.Script.Services.ScriptService()> _
Public Class Replication
    Inherits System.Web.Services.WebService

    ''' <summary>
    ''' Creates Replicated Tasks for the specified template number
    ''' </summary>
    ''' <param name="templateNumber"></param>
    ''' <remarks></remarks>
    <System.Web.Services.WebMethod(Description:="Creates Replicated Tasks for the specified template number"), Script.Services.ScriptMethod()> _
    Public Sub ProcessReplication(ByVal templateNumber As String, ByVal webServerName As String)
        Dim outStatus As Decimal
        Dim ta As New TaskHeaderDALTableAdapters.QueriesTableAdapter
        Try
            'Context.Current
            ta.ProcessReplicationTasks(templateNumber, outStatus)
            EmailDataBll.GetAndSendReplicationEmail(templateNumber, webServerName)
            IP.Bids.SharedFunctions.InsertAuditRecord("MTT:ProcessReplicationWS", "Replication complete for:" & templateNumber)
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("MTT:ProcessReplicationWS", "Error Processing Replication Tasks for:" & templateNumber, ex)
            'IP.Bids.SharedFunctions.InsertAuditRecord("MTT:ProcessReplicationWS", "Error Processing Replication Tasks for:" & templateNumber)

        Finally
            ta = Nothing
        End Try

    End Sub

End Class
