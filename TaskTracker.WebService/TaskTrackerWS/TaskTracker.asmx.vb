Imports System.Web.Services
''' <summary>
''' Collection of Task Tracker Web Services
''' </summary>
<WebService(Namespace:="http://MTT/")> _
<System.Web.Script.Services.ScriptService()> _
Public Class TaskTracker
    Inherits System.Web.Services.WebService

    <WebMethod(Description:="Creates a new mtt task header record.")> _
    Public Function CreateMTTTaskHeader(ByVal title As String, ByVal extRef As String, ByVal extSource As String, ByVal startDate As String, ByVal endDate As String, ByVal siteID As String, ByVal businessUnit As String, ByVal line As String, ByVal description As String, ByVal type As String, ByVal activity As String, ByVal createdBy As String, ByVal createdDate As String) As String
        Dim newHeaderNumber As Integer = -1

        Try
            newHeaderNumber = SaveMttTaskHeader(title, extRef, extSource, startDate, endDate, siteID, businessUnit, line, description, type, activity, createdBy, createdDate)
            If IsNumeric(newHeaderNumber) = False Then
                newHeaderNumber = "Error Saving Task Header for " & title
            End If
        Catch ex As Exception
            newHeaderNumber = ex.Message
        End Try
        Return newHeaderNumber
    End Function

    <WebMethod(Description:="Gets the database connection string")> _
    Public Function GetDatabaseName() As String
        Dim connectionString As String = String.Empty
        If System.Web.Configuration.WebConfigurationManager.ConnectionStrings.Item("DatabaseConnection") IsNot Nothing Then
            connectionString = Web.Configuration.WebConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString.Split(";").First
        End If
        If connectionString.Length = 0 Then Throw New MissingMemberException("The web.config file is missing a valid Connection String value for [DatabaseConnection]")
        If connectionString.ToString.ToLower.ToString.Contains("data source") Then Return connectionString
        Return "Missing"
    End Function

    Public Shared Function SaveMttTaskHeader(ByVal title As String, ByVal extRef As String, ByVal extSource As String, ByVal startDate As String, ByVal endDate As String, ByVal siteID As String, ByVal businessUnit As String, ByVal line As String, ByVal description As String, ByVal type As String, ByVal activity As String, ByVal createdBy As String, ByVal createdDate As String) As Integer
        Dim headerTA As IP.TaskTrackerDAL.TaskHeaderTableAdapters.TaskHeaderTableAdapter = Nothing
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
            headerTA = New IP.TaskTrackerDAL.TaskHeaderTableAdapters.TaskHeaderTableAdapter
            headerTA.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)

            headerTA.SaveMTTTaskHeader(title, extRef, extSource, startDateFormatted, endDateFormatted, siteID, businessUnit, line, description, type, activity, createdBy, createdDateFormatted, outHeaderNumber, outStatus)

        Catch ex As Exception
            InsertAuditRecord("SaveMTTTaskHeader", "Error while saving header  [" & title & "]")
            Try
                Dim input As String = String.Format("Title:{0}, ExternalRef:{1}, ExternalSource:{2}, StartDate:{3}, EndDate:{4}, SiteId:{5}, Business Unit:{6}, Line:{7}, Description:{8}, Type:{9}, Activity:{10}, Created By:{11}", title, extRef, extSource, startDateFormatted, endDateFormatted, siteID, businessUnit, line, description, type, activity, createdBy)
                InsertAuditRecord("SaveMTTTaskHeader", input)
            Catch

            End Try
            Throw New ApplicationException("SaveMTTTaskHeader - Error while saving header  [" & title & "]", ex)
        Finally
            If headerTA IsNot Nothing Then
                headerTA.Dispose()
            End If
            headerTA = Nothing
        End Try
        If IsNumeric(outHeaderNumber) Then
            Return CInt(outHeaderNumber)
        Else
            Return -1
        End If
    End Function

    Public Shared Function SaveIrisTaskHeader(ByVal title As String, ByVal taskHeaderNumber As String, ByVal extRef As String, ByVal extSource As String, ByVal startDate As String, ByVal endDate As String, ByVal siteID As String, ByVal businessUnit As String, ByVal line As String, ByVal description As String, ByVal type As String, ByVal activity As String, ByVal createdBy As String, ByVal createdDate As String) As Integer
        Dim headerTA As IP.TaskTrackerDAL.TaskHeaderTableAdapters.TaskHeaderTableAdapter = Nothing
        Dim outHeaderNumber As Decimal = -1
        Dim startDateFormatted As DateTime
        Dim createdDateFormatted As DateTime
        Dim endDateFormatted As DateTime
        Dim outStatus As Decimal = -1

        Try
            If Not IsNumeric(taskHeaderNumber) Then
                taskHeaderNumber = -1
            End If

            If Len(taskHeaderNumber) < 2 Then
                taskHeaderNumber = -1
            End If

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

            headerTA = New IP.TaskTrackerDAL.TaskHeaderTableAdapters.TaskHeaderTableAdapter
            If description.Length = 0 Then
                description = Nothing
            End If
            headerTA.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            headerTA.SaveIRISTaskHeader(title, extRef, extSource, startDateFormatted, endDateFormatted, siteID, businessUnit, line, description, type, activity, createdBy, createdDateFormatted, CType(taskHeaderNumber, Global.System.Nullable(Of Decimal)), outHeaderNumber, outStatus)

        Catch ex As Exception            
            InsertAuditRecord("SaveIRISTaskHeader", "Error while saving header  [" & title & "]")
            Try
                Dim input As String = String.Format("Title:{0}, Task Header Number:{1}, ExternalRef:{2}, ExternalSource:{3}, StartDate:{4}, EndDate:{5}, SiteId:{6}, Business Unit:{7}, Line:{8}, Description:{9}, Type:{10}, Activity:{11}, Created By:{12}", title, taskHeaderNumber, extRef, extSource, startDateFormatted, endDateFormatted, siteID, businessUnit, line, description, type, activity, createdBy)
                InsertAuditRecord("SaveIRISTaskHeader", input)
            Catch

            End Try
            Throw New ApplicationException("SaveIRISTaskHeader - Error while saving header  [" & title & "]", ex)
        Finally
            If headerTA IsNot Nothing Then
                headerTA.Dispose()
            End If
            headerTA = Nothing
        End Try
        headerTA = Nothing
        If IsNumeric(outHeaderNumber) Then
            Return CInt(outHeaderNumber)
        Else
            InsertAuditRecord("SaveIRISTaskHeader", "Error while saving header  [" & title & "]")
            Return -1
        End If

    End Function

    Private Shared Sub InsertAuditRecord(ByVal sourceName As String, ByVal errorMessage As String)
        Dim dt As New IP.TaskTrackerDAL.AuditLogTableAdapters.AuditLogTableAdapter
        dt.InsertAuditRecord(sourceName, errorMessage)
    End Sub
End Class
