Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

''' <summary>
''' Collection of Task Tracker Web Services
''' </summary>
<WebService(Namespace:="http://MTT/")> _
<System.Web.Script.Services.ScriptService()> _
Public Class TaskTracker
    Inherits System.Web.Services.WebService

    ''' <summary>
    ''' Creates the MTT task header.
    ''' </summary>
    ''' <param name="Title">The title.</param>
    ''' <param name="ExtRef">The ext ref.</param>
    ''' <param name="ExtSource">The ext source.</param>
    ''' <param name="StartDate">The start date.</param>
    ''' <param name="EndDate">The end date.</param>
    ''' <param name="SiteID">The site ID.</param>
    ''' <param name="BusinessUnit">The business unit.</param>
    ''' <param name="Line">The line.</param>
    ''' <param name="Description">The description.</param>
    ''' <param name="Type">The type.</param>
    ''' <param name="Activity">The activity.</param>
    ''' <param name="CreatedBy">The created by.</param>
    ''' <param name="CreatedDate">The created date.</param>
    ''' <returns>returns the header number of the Task Header</returns>
    <WebMethod(Description:="Creates a new mtt task header record.", EnableSession:=True)> _
    Public Function CreateMTTTaskHeader(ByVal title As String, ByVal extRef As String, ByVal extSource As String, ByVal startDate As String, ByVal endDate As String, ByVal siteID As String, ByVal businessUnit As String, ByVal line As String, ByVal description As String, ByVal type As String, ByVal activity As String, ByVal createdBy As String, ByVal createdDate As String) As String
        Dim newHeaderNumber As Integer

        Try
            IP.Bids.SharedFunctions.InsertAuditRecord("CreateMTTTaskHeader", title & " is getting created by " & createdBy)
            newHeaderNumber = TaskHeaderBll.SaveMttTaskHeader(title, extRef, extSource, startDate, endDate, siteID, businessUnit, line, description, type, activity, createdBy, createdDate)
            If IsNumeric(newHeaderNumber) = False Then
                newHeaderNumber = "Error Saving Task Header for " & title
            End If
        Catch ex As Exception
            newHeaderNumber = ex.Message
            IP.Bids.SharedFunctions.HandleError("CreateMTTTaskHeader", "Error Saving Task Header: " & ex.Message, ex)
        End Try
        Return newHeaderNumber
    End Function

    ''' <summary>
    ''' Creates the MTT task header.
    ''' </summary>
    ''' <param name="Title">The title.</param>
    ''' <param name="ExtRef">The ext ref.</param>
    ''' <param name="ExtSource">The ext source.</param>
    ''' <param name="StartDate">The start date.</param>
    ''' <param name="EndDate">The end date.</param>
    ''' <param name="SiteID">The site ID.</param>
    ''' <param name="BusinessUnit">The business unit.</param>
    ''' <param name="Line">The line.</param>
    ''' <param name="Description">The description.</param>
    ''' <param name="Type">The type.</param>
    ''' <param name="Activity">The activity.</param>
    ''' <param name="CreatedBy">The created by.</param>
    ''' <param name="CreatedDate">The created date.</param>
    ''' <returns>returns the header number of the Task Header</returns>
    <WebMethod(Description:="Creates a new mtt task header record.", EnableSession:=True)> _
    Public Function CreateIRISTaskHeader(ByVal title As String, ByVal taskHeaderNumber As String, ByVal extRef As String, ByVal extSource As String, ByVal startDate As String, ByVal endDate As String, ByVal siteID As String, ByVal businessUnit As String, ByVal line As String, ByVal description As String, ByVal type As String, ByVal activity As String, ByVal createdBy As String, ByVal createdDate As String) As String
        Dim newHeaderNumber As Integer

        Try
            newHeaderNumber = TaskHeaderBll.SaveIrisTaskHeader(title, taskHeaderNumber, extRef, extSource, startDate, endDate, siteID, businessUnit, line, description, type, activity, createdBy, createdDate)
            If IsNumeric(newHeaderNumber) = False Then
                newHeaderNumber = "Error Saving Task Header for " & title
            End If
        Catch ex As Exception
            newHeaderNumber = ex.Message
            IP.Bids.SharedFunctions.HandleError("CreateMTTTaskHeader", "Error Saving Task Header: " & ex.Message, ex)
        End Try
        Return newHeaderNumber
    End Function

    <WebMethod(Description:="Creates the default mtt task header for IRIS.")> _
  Public Function TestIRISTaskHeader(ByVal createdBy As String) As String
        Dim strTaskHeaderSeqID As String = ""
        'Dim paramCollection As New OracleParameterCollection
        'Dim param As New OracleParameter
        Dim startDateFmt, createdDateFmt, endDateFmt As DateTime
        Dim Title As String = "New IRIS Event" & Now.ToShortDateString & " " & Now.ToLongTimeString
        Dim ExtRef As String = "-1"
        Dim TaskHeader As String = "116385" '"-1"
        Dim ExtSource As String = "IRIS"
        Dim StartDate As String = Now.ToShortDateString
        Dim EndDate As String = Now.ToShortDateString
        Dim SiteID As String = "Memphis Towers"
        Dim BusinessUnit As String = "Millwide - Safety"
        Dim Line As String = ""
        Dim Description As String = ""
        Dim Type As String = "Health & Safety"
        Dim Activity As String = "Incident"
        Dim CreatedDate As String = Now.ToShortDateString
        Try

            If IsDate(StartDate) Then
                startDateFmt = FormatDateTime(StartDate, DateFormat.ShortDate)
            Else
                startDateFmt = FormatDateTime(Now(), DateFormat.ShortDate)
            End If
            If IsDate(EndDate) Then
                endDateFmt = FormatDateTime(EndDate, DateFormat.ShortDate)
            Else
                endDateFmt = FormatDateTime(Now(), DateFormat.ShortDate)
            End If
            If IsDate(CreatedDate) Then
                createdDateFmt = FormatDateTime(CreatedDate, DateFormat.ShortDate)
            Else
                createdDateFmt = FormatDateTime(Now(), DateFormat.ShortDate)
            End If

            Dim createdByUserName() As String = createdBy.Split("\")
            If createdByUserName.Length > 1 Then
                createdBy = createdByUserName(0)
            End If
            'ws.Credentials = System.Net.CredentialCache.DefaultCredentials
            'ws.PreAuthenticate = True
            strTaskHeaderSeqID = CreateIRISTaskHeader(Title, TaskHeader, ExtRef, ExtSource, startDateFmt, endDateFmt, SiteID, BusinessUnit, Line, Description, Type, Activity, createdBy, createdDateFmt)

        Catch ex As Exception
            Throw New Exception("Create MTT Task Header Web Service Error", ex.InnerException)
            'RI.SharedFunctions.HandleError()
        Finally
            TestIRISTaskHeader = strTaskHeaderSeqID
        End Try
    End Function
    <WebMethod(Description:="Creates the default mtt task header for IRIS.")> _
    Public Function GetAvailableRoles() As System.Collections.Generic.List(Of String)
        Dim allroleList As System.Collections.Generic.List(Of Role) = DataMaintenanceBLL.GetAllRoles()
        Dim listOfRoleNames As New Generic.List(Of String)

        For Each item As Role In allroleList
            listOfRoleNames.Add(item.RoleDescription)
        Next
        Return listOfRoleNames
    End Function
    '  ''' <summary>
    '  ''' Gets the task count by reference number.
    '  ''' </summary>
    '  ''' <param name="externalReferenceNumber">The external reference number.</param>
    '  ''' <param name="externalSourceName">Name of the external source.</param>
    '  ''' <returns>
    '  ''' Returns the Total number of Task Items (>=0)
    '  '''</returns>
    '  <WebMethod(Description:="Gets the count of the Task Items for the specified external reference number and source")> _
    '  Public Function GetTaskCountByReferenceNumber(ByVal externalReferenceNumber As String, ByVal externalSourceName As String) As Integer
    '      Return 0
    '  End Function

    '  ''' <summary>
    '  ''' Gets the task count by task header number.
    '  ''' </summary>
    '  ''' <param name="taskHeaderNumber">The task header number.</param>
    '  ''' <returns>
    '  ''' Integer - Total count of Task Items (>=0)
    '  '''</returns>
    '  <WebMethod(Description:="Gets the count of the Task Items for the specified Task Header number")> _
    '  Public Function GetTaskCountByTaskHeaderNumber(ByVal taskHeaderNumber As String) As Integer
    '      Return 0
    '  End Function

  


End Class
