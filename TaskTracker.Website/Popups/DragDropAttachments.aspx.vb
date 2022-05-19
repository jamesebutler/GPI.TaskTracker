Option Explicit On
Option Strict On
Imports System.Globalization
Partial Class Popups_DragDropAttachments
    Inherits IP.Bids.BasePage
    Property UploadsUrl As String
    Property SaveAsPath As String
    Property TaskItemNumber As String
    Property TaskHeaderNumber As String

    Private Sub Popups_DragDropAttachments_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request.Files.Count > 0 Then
            TaskHeaderNumber = GetTaskHeader()
            TaskItemNumber = GetTaskItemNumber()
            UploadFiles()
        End If
    End Sub

    Private Function GetTaskHeader() As String
        If TaskHeaderNumber IsNot Nothing AndAlso IsNumeric(TaskHeaderNumber) Then
            Return TaskHeaderNumber
        End If
        If Request.QueryString("HeaderNumber") IsNot Nothing Then
            Return Request.QueryString("HeaderNumber")
        End If
        If HttpUtility.ParseQueryString(Request.UrlReferrer.Query).Item("headernumber") IsNot Nothing Then
            Return HttpUtility.ParseQueryString(Request.UrlReferrer.Query).Item("headernumber")
        End If
        Return String.Empty
    End Function

    Private Function GetTaskItemNumber() As String
        If TaskItemNumber IsNot Nothing AndAlso IsNumeric(TaskItemNumber) Then
            Return TaskItemNumber
        End If
        If Request.QueryString("TaskNumber") IsNot Nothing Then
            Return Request.QueryString("TaskNumber")
        End If
        If HttpUtility.ParseQueryString(Request.UrlReferrer.Query).Item("TaskNumber") IsNot Nothing Then
            Return HttpUtility.ParseQueryString(Request.UrlReferrer.Query).Item("TaskNumber")
        End If
        Return String.Empty
    End Function

    Private Sub UploadFiles()
        SetUploadFolder()
        Dim filePath As String = SaveAsPath.Trim

        If filePath.Length = 0 Then
            Throw New Web.HttpException("The required Save As Path has not been defined")
        End If

        If Right(filePath, 1) <> "\" Then
            filePath = filePath & "\"
        End If

        If Not My.Computer.FileSystem.DirectoryExists(filePath) Then
            Try
                My.Computer.FileSystem.CreateDirectory(filePath)
            Catch
                Throw
            End Try
        End If
        Dim filePreFix As String = String.Empty
        If Me.TaskItemNumber IsNot Nothing Then
            filePreFix = Me.TaskHeaderNumber & "_" & Me.TaskItemNumber.ToString & "_"
        Else
            filePreFix = Me.TaskHeaderNumber & "_"
        End If
        Try
            For i As Integer = 0 To Request.Files.Count - 1
                Dim file As String = filePreFix & Request.Files(i).FileName.Replace("#", "No").Replace("%", "Pct").Replace("?", "_")
                Request.Files(i).SaveAs(filePath & file)
                DocumentsAndLinksBll.SaveDocumentsAndLinks("-1", Me.TaskHeaderNumber, Me.TaskItemNumber, file, filePath, file, IP.Bids.SharedFunctions.GetCurrentUser.Username, "I") 'TODO:  Get Current User master.CurrentUser.Username 
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub SetUploadFolder()
        Dim uploadsUrl As String = ConfigurationManager.AppSettings.Item("DevelopmentServer")
        Dim uploadsFolder As String = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")

        If HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("localhost") Then
            uploadsUrl = ConfigurationManager.AppSettings.Item("DevelopmentUploadsUrl")
            uploadsFolder = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")
        ElseIf HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("ridev") Then
            uploadsUrl = ConfigurationManager.AppSettings.Item("DevelopmentUploadsUrl")
            uploadsFolder = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")
        ElseIf HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("ritest") Then
            uploadsUrl = ConfigurationManager.AppSettings.Item("DevelopmentUploadsUrl")
            uploadsFolder = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")
        Else
            uploadsUrl = ConfigurationManager.AppSettings.Item("ProductionUploadsUrl")
            uploadsFolder = ConfigurationManager.AppSettings.Item("ProductionUploadsFolder")
        End If

        uploadsUrl = ConfigurationManager.AppSettings.Item("ProductionUploadsUrl")
        uploadsFolder = ConfigurationManager.AppSettings.Item("ProductionUploadsFolder")
        Me.UploadsUrl = uploadsUrl
        Me.SaveAsPath = uploadsFolder
    End Sub
End Class
