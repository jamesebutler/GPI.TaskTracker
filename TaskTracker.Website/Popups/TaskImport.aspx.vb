Option Explicit On
Option Strict On
Imports ClosedXML.Excel

Partial Class Popups_AuditImport
    Inherits IP.Bids.BasePage
#Region "Private Styles"
    Private mFileInputInstructions As WebControls.Style

    ''' <summary>
    ''' The purpose of this method is to control the position of the controls on this user control without being concerned about the developer using the control.
    ''' </summary>
    Private Sub ApplyStyles()
        'With _pnlAttachments
        '    .Style("position") = "relative"
        'End With

        'With _lblFileToUpload
        '    .Style("text-align") = "right"
        '    .Style("padding-right") = "10px"
        '    .Font.Bold = True
        '    .Width = Unit.Pixel(150)
        'End With

        With _btnUploadFile
            .Width = Unit.Pixel(150)
        End With
        'With _btnCancel
        '    .Width = Unit.Pixel(150)
        'End With

        'With _txtFileName
        '    .Width = Unit.Pixel(600) 'Percentage(95)
        'End With
    End Sub
#End Region

#Region "Fields"
    Private TaskHeaderNumber As String = String.Empty 'Holds the current Header Number
    Private importedTaskList As New System.Collections.Generic.List(Of TaskItem)
#End Region

    Public Sub DisplayMessage(ByVal msg As String)
        With Me.ctl00__AlertMessage
            .Message = msg
            .Title = IP.Bids.SharedFunctions.LocalizeValue("Import Tasks", True)
            .ShowMessage()
        End With
    End Sub

    Private Sub HandleLoad()
        If Request.QueryString("HeaderNumber") IsNot Nothing AndAlso IsNumeric(Request.QueryString("HeaderNumber")) Then
            'Store Header Number
            TaskHeaderNumber = Request.QueryString("HeaderNumber")
            'Me.fileUpEx.Attributes.Add("onchange", "document.getElementById('" & Me._txtFileName.ClientID & "').value = this.value; ") 'onchange="javascript: document.getElementById('_fileName').value = this.value"
            'fileUpEx.Attributes.Add("onClick", String.Format("$('#{0}').click", _btnUploadFile.ClientID))
            Me._btnUploadFile.OnClientClick = "return OnUpload('" & fileUpEx.ClientID & "');"
            'var obj = document.getElementById("<%=fileUpEx.ClientID%>"); 
            '_fileUpload.Attributes.Add("onchange", "document.getElementById('" & Me._txtFileName.ClientID & "').value = this.value; ") 'onchange="javascript: document.getElementById('_fileName').value = this.value"
            'Me._btnAddTasksToHeader.OnClientClick = "Javascript:DisplayBusy();return true;"
            Page.ClientScript.RegisterClientScriptInclude("FileAttachments", Page.ResolveUrl("~/Scripts/FileAttachments.js"))
        Else
            Me.DisplayMessage(IP.Bids.SharedFunctions.LocalizeValue("Missing Task Header", True))
        End If

        _btnAddTasksToHeader.Visible = False

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandleLoad()
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ApplyStyles()
    End Sub

    'Protected Sub _btnUploadFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnUploadFile.Click
    '    Dim uploads As HttpFileCollection
    '    uploads = HttpContext.Current.Request.Files

    '    For i As Integer = 0 To (uploads.Count - 1)
    '        If (uploads(i).ContentLength > 0) Then
    '            Dim c As String = System.IO.Path.GetFileName(uploads(i).FileName)

    '            Dim dt As OleDb.OleDbCommand = CSVReader.ExcelConnection(uploads(i).FileName)
    '            With _gvData
    '                .DataSource = dt.ExecuteReader
    '                .AutoGenerateColumns = True
    '                .DataBind()
    '            End With

    '        End If
    '    Next
    'End Sub

    Private Function GetUploadFolder() As String
        Dim uploadsFolder As String = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")

        If HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("localhost") Then
            uploadsFolder = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")
        ElseIf HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("ridev") Then
            uploadsFolder = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")
        ElseIf HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("ritest") Then
            uploadsFolder = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")
        Else
            uploadsFolder = ConfigurationManager.AppSettings.Item("ProductionUploadsFolder")
        End If
        Return uploadsFolder
    End Function
    Protected Sub _btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnUploadFile.Click
        Dim uploads As HttpFileCollection
        Dim filePath As String = GetUploadFolder() 'Server.MapPath("~/Uploads")

        Try
            _rpTasks.Visible = False
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

            uploads = HttpContext.Current.Request.Files

            If uploads.Count > 0 Then
                Dim i As Integer = 0
                'For i As Integer = 0 To (uploads.Count - 1)

                If (uploads(i).ContentLength > 0) Then
                    'Dim c As String = System.IO.Path.GetFileName(uploads(i).FileName)
                    Dim filename As String = filePath
                    Dim file As String = System.IO.Path.GetFileName(uploads(i).FileName)

                    Try
                        If My.Computer.FileSystem.FileExists(filename & file) Then
                            Try
                                Dim versions As Integer = 1
                                Dim versionedFile As String
                                versionedFile = "V" & versions & "_" & file
                                Do While My.Computer.FileSystem.FileExists(filename & versionedFile) = True
                                    versions = versions + 1
                                    versionedFile = "V" & versions.ToString & "_" & file
                                    If versions >= 100 Then
                                        Exit Do
                                    End If
                                Loop
                                file = versionedFile
                            Catch ex As Exception
                                IP.Bids.SharedFunctions.HandleError(, "Error Renaming " & filename, ex)
                            End Try
                        End If
                        filename = filename & file
                        uploads(i).SaveAs(filename) 'filePath + Me.TaskHeaderNumber & "_" & Me.TaskItemNumber & "_" & c)

                        Dim dt = ImportExcel(filename)
                        'csvreader.ReadExcel(filename)
                        Dim dr = dt.CreateDataReader 'As OleDb.OleDbDataReader = excelCommand.ExecuteReader

                        Dim row As Integer = 0

                        'Validate Required Fields
                        Dim requiredFieldCount As Integer
                        For requiredFieldIndex As Integer = 0 To dr.FieldCount - 1

                            Select Case dr.GetName(requiredFieldIndex)
                                'Total of 64 = MTT or Outage Template                              
                                Case "ResponsibleRole"
                                    requiredFieldCount += 1
                                Case "DueDate"
                                    requiredFieldCount += 3
                                Case "Title"
                                    requiredFieldCount += 5
                                Case "RoleID"
                                    requiredFieldCount += 7
                                Case "Priority"
                                    requiredFieldCount += 9
                                Case "Description"
                                    requiredFieldCount += 11
                                Case "WeeksBefore"
                                    requiredFieldCount += 13
                                Case "WeeksAfter"
                                    requiredFieldCount += 15
                            End Select
                        Next
                        If requiredFieldCount <> 64 Then
                            'Invalid File
                            DisplayMessage("The selected file is missing the required fields.  Please confirm that the file was created from an approved Task Tracker Template.")
                            Exit Sub
                        End If
                        'If hasRequired = False Then
                        '    'Required Fields are missing
                        '    Exit Sub
                        'End If
                        'Dim columnList As New StringCollection
                        'Dim requiredFields As New StringCollection
                        'Dim optionalFields As New StringCollection

                        'With requiredFields
                        '    .Add("ResponsibleName")
                        '    .Add("DueDate")
                        '    .Add("Title")
                        'End With

                        'With optionalFields
                        '    .Add("Priority")
                        '    .Add("Description")
                        'End With

                        Dim fieldResponsibleRoleID As String = String.Empty
                        Dim fieldResponsibleRole As String = String.Empty
                        Dim fieldDueDate As String = String.Empty
                        Dim fieldTitle As String = String.Empty
                        Dim fieldPriority As String = String.Empty
                        Dim fieldDescription As String = String.Empty
                        Dim fieldWeeksBefore As String = String.Empty
                        Dim fieldWeeksAfter As String = String.Empty

                        Do While dr.Read

                            If dr.Item("RoleID") IsNot Nothing AndAlso IsNumeric(dr.Item("RoleID")) Then
                                fieldResponsibleRoleID = CStr(dr.Item("RoleID"))
                            Else
                                fieldResponsibleRoleID = String.Empty
                                'missing required field
                                Continue Do
                            End If

                            'Determine if this is an MTT Import or Outage Import
                            If dr.Item("DueDate") IsNot Nothing OrElse dr.Item("WeeksBefore") IsNot Nothing OrElse dr.Item("WeeksAfter") IsNot Nothing Then
                                If dr.Item("DueDate") IsNot Nothing AndAlso IsDate(dr.Item("DueDate")) Then
                                    fieldDueDate = CStr(dr.Item("DueDate"))
                                ElseIf dr.Item("WeeksBefore") IsNot Nothing AndAlso IsNumeric(dr.Item("WeeksBefore")) Then
                                    fieldWeeksBefore = CStr(dr.Item("WeeksBefore"))
                                ElseIf dr.Item("WeeksAfter") IsNot Nothing AndAlso IsNumeric(dr.Item("WeeksAfter")) Then
                                    fieldWeeksAfter = CStr(dr.Item("WeeksAfter"))
                                Else
                                    Continue Do
                                End If
                            Else
                                'missing required field
                                Continue Do
                            End If

                            If dr.Item("Title") IsNot Nothing AndAlso dr.Item("Title").ToString.Length > 0 Then
                                fieldTitle = CStr(dr.Item("Title"))
                            Else
                                fieldTitle = String.Empty
                                'missing required field
                                Continue Do
                            End If
                            If dr.Item("Priority") IsNot Nothing Then
                                fieldPriority = IP.Bids.SharedFunctions.DataClean(dr.Item("Priority"))
                            Else
                                fieldPriority = String.Empty
                            End If
                            If dr.Item("Description") IsNot Nothing Then
                                fieldDescription = IP.Bids.SharedFunctions.DataClean(dr.Item("Description"))
                            Else
                                fieldDescription = String.Empty
                            End If
                            If dr.Item("ResponsibleRole") IsNot Nothing Then
                                fieldResponsibleRole = IP.Bids.SharedFunctions.DataClean(dr.Item("ResponsibleRole"))
                            Else
                                fieldResponsibleRole = String.Empty
                            End If

                            Dim newTaskItem As New TaskItem
                            With newTaskItem
                                .ResponsibleRoleSeqId = CInt(fieldResponsibleRoleID)
                                .DueDate = fieldDueDate
                                .Title = fieldTitle
                                .Priority = fieldPriority
                                .Description = fieldDescription
                                .RoleName = fieldResponsibleRole
                                If fieldWeeksBefore.Length > 0 Then
                                    .DaysBefore = CInt(fieldWeeksBefore) 'CInt((CInt(fieldWeeksBefore) / 7))
                                End If
                                If fieldWeeksAfter.Length > 0 Then
                                    .DaysAfter = CInt(fieldWeeksAfter) 'CInt((CInt(fieldWeeksAfter) / 7))
                                End If
                            End With
                            If importedTaskList IsNot Nothing Then
                                importedTaskList.Add(newTaskItem)
                            End If
                            'select case dr.Item( 
                            'If row = 0 Then
                            '        '            If importedTaskList IsNot Nothing Then
                            '        '                Dim newTaskItem As New TaskItem
                            '        '                With newTaskItem
                            '        '                    If Me._gvData.Columns.Contains Then
                            '        'End With
                            '        '                importedTaskList.Add(newTaskItem)
                            '        '            End If
                            '    End If
                            'row += 1
                        Loop

                        With _rpTasks
                            .DataSource = importedTaskList 'excelCommand.ExecuteReader
                            '.AutoGenerateColumns = True
                            .DataBind()
                            .Visible = True
                        End With

                        If importedTaskList.Count > 0 Then
                            'Logic needs to be added to validate the excel data
                            Session.Add("ImportedTaskList", importedTaskList)
                            _btnAddTasksToHeader.Visible = True
                            _btnUploadFile.Visible = False
                        End If
                    Catch exp As System.Web.HttpException
                        ': The System.Web.Configuration.HttpRuntimeSection.RequireRootedSaveAsPath 
                        'property of the System.Web.Configuration.HttpRuntimeSection object is set to true, 
                        'but filename is not an absolute path.  
                        IP.Bids.SharedFunctions.HandleError("File Upload Failed.", , exp)
                        Me.DisplayMessage("The application is unable to read the selected excel file")
                    Catch ex As Exception
                        IP.Bids.SharedFunctions.HandleError("File Upload Failed.", , ex)
                        Me.DisplayMessage("The application is unable to read the selected excel file")
                    End Try

                End If

                'Next i
            Else

            End If
            _lblFileUploadStatus.Text = importedTaskList.Count.ToString & " " & IP.Bids.SharedFunctions.LocalizeValue("tasks are available to be added.", True)
        Catch
            Throw
        End Try
    End Sub


    Protected Sub _btnAddTasksToHeader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnAddTasksToHeader.Click
        Dim taskstatus As New TaskTrackerListsBll
        Dim status As System.Collections.Generic.List(Of TaskStatus)
        Dim openTaskSeqId As Integer
        Dim taskHeader As New TaskHeaderBll(CInt(TaskHeaderNumber))
        Dim headerPlantCode As String = String.Empty

        If taskHeader IsNot Nothing AndAlso taskHeader.CurrentTaskHeaderRecord IsNot Nothing Then
            headerPlantCode = taskHeader.CurrentTaskHeaderRecord.PlantCode
        End If

        If Session.Item("ImportedTaskList") IsNot Nothing Then
            importedTaskList = TryCast(Session.Item("ImportedTaskList"), System.Collections.Generic.List(Of TaskItem))
        End If

        status = taskstatus.GetTaskStatus
        For Each item As TaskStatus In status
            If item.StatusName.ToLower = "open" Then
                openTaskSeqId = item.StatusSeqid
                Exit For
            End If
        Next
        If importedTaskList IsNot Nothing AndAlso importedTaskList.Count > 0 Then
            'Add tasks
            Dim taskItemBLL As New TaskTrackerItemBll
            For Each item As TaskItem In importedTaskList
                With taskItemBLL
                    item.TaskHeaderSeqId = CInt(TaskHeaderNumber)
                    item.CreatedBy = IP.Bids.SharedFunctions.GetCurrentUser.Username
                    item.CreatedDate = Now.ToShortDateString
                    item.TaskItemSeqId = -1
                    item.DateCritical = "N"
                    item.ClosedDate = String.Empty
                    item.LastUpdateDate = Now.ToShortDateString
                    item.LastUpdateUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                    item.LeadTime = 0
                    item.StatusSeqId = openTaskSeqId
                    item.ResponsibleName = String.Empty
                    item.ResponsibleUserName = String.Empty
                    item.RoleDescription = String.Empty
                    item.ResponsibleRolePlantCode = headerPlantCode
                    item.Priority = CStr(GetValidPriority(item.Priority))
                    If item.DueDate.Length > 0 Then
                        .SaveTaskItem(item)
                    Else
                        .SaveOutageTemplateTaskItem(item)
                    End If
                End With
            Next
            Me._rpTasks.Visible = False
            Me._btnAddTasksToHeader.Visible = False

            If Request.QueryString("CloseMe") IsNot Nothing Then
                Me.ctl00__AlertMessage.OKScript = "try{parent.document.getElementById('" & Request.QueryString("CloseMe") & "').click();}catch(e){alert(e);}"
            End If
            'Me.DisplayMessage(importedTaskList.Count.ToString & " " & IP.Bids.SharedFunctions.LocalizeValue("tasks have been added.", True))
        End If
        _lblFileUploadStatus.Text = importedTaskList.Count.ToString & " " & IP.Bids.SharedFunctions.LocalizeValue("tasks have been added.", True)
    End Sub

    Private Function GetValidPriority(ByVal priority As String) As Integer
        Select Case priority.ToUpper
            Case "LOW", "1"
                Return 1
            Case "MEDIUM", "2"
                Return 2
            Case "HIGH", "3"
                Return 3
            Case Else
                Return 1
        End Select
    End Function
    Protected Sub _rpTasks_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles _rpTasks.ItemCreated
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lblTaskDueDate As Label = CType(e.Item.FindControl("_lblTaskDueDate"), Label)
            Dim lblTaskDueDateLabel As Label = CType(e.Item.FindControl("_lblDueDate"), Label)
            Dim taskItem As IP.MEAS.BO.TaskItem = CType(e.Item.DataItem, IP.MEAS.BO.TaskItem)

            If taskItem IsNot Nothing Then
                If lblTaskDueDate IsNot Nothing AndAlso lblTaskDueDateLabel IsNot Nothing Then
                    If taskItem.DueDate.Length > 0 Then
                        lblTaskDueDate.Text = taskItem.DueDate
                        lblTaskDueDateLabel.Text = IP.Bids.SharedFunctions.LocalizeValue("Due Date", True)
                    ElseIf taskItem.DaysBefore >= 0 Then
                        lblTaskDueDate.Text = CStr(taskItem.DaysBefore * 7)
                        lblTaskDueDateLabel.Text = IP.Bids.SharedFunctions.LocalizeValue("Days Before", True)
                    ElseIf taskItem.DaysAfter >= 0 Then
                        lblTaskDueDate.Text = CStr(taskItem.DaysAfter * 7)
                        lblTaskDueDateLabel.Text = IP.Bids.SharedFunctions.LocalizeValue("Days After", True)
                    End If
                End If
            End If
        End If
    End Sub

    Private Function ImportExcel(ByVal fileName As String) As DataTable
        'Create a new DataTable.
        Dim dt As New DataTable()

        'Open the Excel file using ClosedXML.
        Using workBook As New XLWorkbook(fileName)
            'Read the first Sheet from Excel file.
            Dim workSheet As IXLWorksheet = workBook.Worksheet(1)

            'Loop through the Worksheet rows.
            Dim firstRow As Boolean = True
            For Each row As IXLRow In workSheet.Rows()
                'Use the first row to add columns to DataTable.
                If firstRow Then
                    For Each cell As IXLCell In row.Cells()
                        dt.Columns.Add(cell.Value.ToString())
                    Next
                    firstRow = False
                Else
                    'Add rows to DataTable.
                    dt.Rows.Add()
                    Dim i As Integer = 0
                    Dim firstCell As Boolean = True
                    For Each cell As IXLCell In row.Cells()
                        If cell.HasFormula Then
                            Dim evaluatedValue = cell.ValueCached

                            dt.Rows(dt.Rows.Count - 1)(i) = evaluatedValue
                        Else
                            If firstCell And cell.Value.ToString() = "" Then Return dt
                            Dim evaluatedValue = cell.Value.ToString()
                            If IsDate(evaluatedValue) Then
                                evaluatedValue = FormatDateTime(CDate(evaluatedValue))
                            End If
                            dt.Rows(dt.Rows.Count - 1)(i) = evaluatedValue
                        End If
                        firstCell = False
                        If i + 1 >= dt.Columns.Count Then Exit For
                        i += 1
                    Next
                End If
            Next
        End Using
        Return dt
    End Function
End Class
