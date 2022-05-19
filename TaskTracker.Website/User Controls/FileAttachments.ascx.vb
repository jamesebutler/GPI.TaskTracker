'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 09-14-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-15-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Option Explicit On
Option Strict On
Imports System.Globalization
Imports AjaxControlToolkit

''' <summary>
''' 
''' </summary>
Partial Class UserControlsFileAttachments
    Inherits System.Web.UI.UserControl
    Public Event DocumentsLoaded()

#Region "Enums"
    Private Enum PageMode
        Edit
        View
    End Enum
#End Region
#Region "Fields"
    Private mSaveAsPath As String = String.Empty
    Private mTaskHeaderNumber As String = String.Empty
    Private mTaskItemNumber As String = String.Empty
    Private mCurrentUser As IP.Bids.UserInfo
    Private mAllowAttachmentUpload As Boolean = True
    Private mUploadsUrl As String = String.Empty
#End Region

#Region "Properties"
    Public Property AttachmentCount() As Integer = 0

    Public Property AllowAttachmentUpload() As Boolean = True


    ''' <summary>
    ''' Gets or sets the task header number.
    ''' </summary>
    ''' <value>The task header number.</value>    
    Public Property TaskHeaderNumber() As String

    ''' <summary>
    ''' Gets or sets the task item number.
    ''' </summary>
    ''' <value>The task item number.</value>
    Public Property TaskItemNumber() As String

    ''' <summary>
    ''' Gets or sets the save as path.
    ''' </summary>
    ''' <value>The save as path.</value>
    Public Property SaveAsPath() As String

    Public Property UploadsUrl As String
        Get
            If mUploadsUrl.EndsWith("/") Then
                Return mUploadsUrl
            Else
                Return mUploadsUrl & "/"
            End If
        End Get
        Set(value As String)
            mUploadsUrl = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the attach button text.
    ''' </summary>
    ''' <value>The attach button text.</value>
    Public Property AttachButtonText() As String
        Get
            Return Me._btnAttachLink.Text
        End Get
        Set(ByVal value As String)
            Me._btnAttachLink.Text = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the cancel link text.
    ''' </summary>
    ''' <value>The cancel link text.</value>
    Public Property CancelLinkText() As String
        Get
            Return Me._btnCancelLink.Text
        End Get
        Set(ByVal value As String)
            Me._btnCancelLink.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the link file instructions text.
    ''' </summary>
    ''' <value>The link file instructions text.</value>
    Public Property LinkFileInstructionsText() As String
        Get
            Return Me._lblFileAttachmentInstructions.Text
        End Get
        Set(ByVal value As String)
            Me._lblFileAttachmentInstructions.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the link file description text.
    ''' </summary>
    ''' <value>The link file description text.</value>
    Public Property LinkFileDescriptionText() As String
        Get
            Return Me._lblLinkDescription.Text
        End Get
        Set(ByVal value As String)
            Me._lblLinkDescription.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the link description text.
    ''' </summary>
    ''' <value>The link description text.</value>
    Public Property LinkDescriptionText() As String
        Get
            Return Me._lblLinkDescription.Text
        End Get
        Set(ByVal value As String)
            Me._lblLinkDescription.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the link instructions text.
    ''' </summary>
    ''' <value>The link instructions text.</value>
    Public Property LinkInstructionsText() As String
        Get
            Return Me._lblLinkInstructions.Text
        End Get
        Set(ByVal value As String)
            Me._lblLinkInstructions.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the link to attach text.
    ''' </summary>
    ''' <value>The link to attach text.</value>
    Public Property LinkToAttachText() As String
        Get
            Return Me._lblLinkToAttach.Text
        End Get
        Set(ByVal value As String)
            Me._lblLinkToAttach.Text = value
        End Set
    End Property

    'Public Property Width() As Unit
    '    Get
    '        Return Me._pnlAttachedFiles.Width
    '    End Get
    '    Set(ByVal value As Unit)
    '        _pnlAttachedFiles.Width = value
    '    End Set
    'End Property

    'Public Property Height() As Unit
    '    Get
    '        Return Me._pnlAttachedFiles.Height
    '    End Get
    '    Set(ByVal value As Unit)
    '        If value.Value < 200 Then value = Unit.Pixel(200)
    '        _pnlAttachedFiles.Height = value
    '        Me._divFileAttachments.Style.Item("Height") = CInt(value.Value) - 50 & "px"
    '        _divLinkAttachments.Style.Item("Height") = CInt(value.Value) - 50 & "px"
    '    End Set
    'End Property

#End Region

#Region "Private Styles"
    Private mFileInputInstructions As WebControls.Style

    ''' <summary>
    ''' The purpose of this method is to control the position of the controls on this user control without being concerned about the developer using the control.
    ''' </summary>
    'Private Sub ApplyStyles()
    '    With _lblLinkToAttach
    '        .Style("text-align") = "right"
    '        .Style("padding-right") = "10px"
    '        .Font.Bold = True
    '        .Width = Unit.Pixel(150)
    '    End With

    '    With _lblLinkDescription
    '        '.Style("float") = "left"
    '        .Style("text-align") = "right"
    '        .Style("padding-right") = "10px"
    '        .Font.Bold = True
    '        .Width = Unit.Pixel(150)
    '    End With

    '    'With _btnBrowse
    '    '    .Style("position") = "relative"
    '    '    '.Style("float") = "left"
    '    '    .Style("top") = "0px"
    '    '    .Style("left") = "0px"
    '    '    .Style("z-index") = "1"
    '    '    '.Style("position") = "absolute"
    '    '    '.Style("top") = "0px"
    '    '    '.Style("right") = "0px"
    '    '    .Width = Unit.Pixel(100)
    '    'End With
    '    With _btnCancelLink
    '        .Width = Unit.Pixel(150)
    '    End With
    '    With _btnAttachLink
    '        .Width = Unit.Pixel(150)
    '    End With


    '    'With _fileUpload
    '    '    '.Style("position") = "relative"
    '    '    '.Style("top") = "0px"
    '    '    '.Style("left") = "-100px"
    '    '    '.Style("z-index") = "2"
    '    '    .Style("opacity") = "0"
    '    '    .Style("filter") = "alpha(opacity=0)"
    '    '    .Style("-ms-filter") = "alpha(opacity=0)"
    '    '    .Style("-khtml-opacity") = "0"
    '    '    .Style("-moz-opacity") = "0"
    '    '    .Width = Unit.Pixel(100)
    '    'End With

    '    With _txtLinkToAttach
    '        .Width = Unit.Pixel(400) 'Unit.Percentage(95)
    '    End With

    '    'With Me._txtFile
    '    '    .Width = Unit.Percentage(80)
    '    'End With

    '    With _txtLinkDescription
    '        .Width = Unit.Pixel(400) 'Unit.Percentage(95)
    '    End With

    'End Sub
#End Region

#Region "Methods"
    Public Function GetAttachmentDisplay(ByVal file As String, ByVal location As String) As String
        '        Dim returnVal As String = String.Empty ' COMMENTED BY CODEIT.RIGHT
        If file.Length > 0 Then 'Attachment is a file
            Return file
        Else 'Attachment is a URL
            Return location
        End If
    End Function
    Public Function GetFileLocation(ByVal file As String, ByVal location As String) As String
        '<%#string.format(Page.ResolveUrl("~/Uploads/{0}"),EVAL("FileName")) %>
        '        Dim returnVal As String = String.Empty ' COMMENTED BY CODEIT.RIGHT
        If file.Length > 0 And location.Length > 0 Then 'Attachment is a file
            'If Request.UserHostAddress = "127.1.1.0" Then 'Localhost
            'Return String.Format(CultureInfo.CurrentCulture, "{1}{0}", file, UploadsUrl)

            'jeb fix for security
            Dim urlpath As String

            urlpath = "http://" & HttpContext.Current.Request.Url.Host & ":" & HttpContext.Current.Request.Url.Port & "/uploads/"
            urlpath = urlpath & file
            Return urlpath

            ''location = "http:" & location
            'JEB Return String.Format(CultureInfo.CurrentCulture, "{1}{0}", file, location)
            'Else
            '   Return String.Format(CultureInfo.CurrentCulture, Page.ResolveUrl("~/Uploads/{0}"), file)
            'End If
        Else 'Attachment is a URL
            If location.StartsWith("www", StringComparison.CurrentCulture) Then
                location = "http://" & location
            End If
            Return location
        End If
    End Function

    Public Overloads Sub DataBind()
        Me._grvAttachedFiles.DataBind()
    End Sub

    Private Function GetAttachmentsList() As System.Collections.Generic.List(Of MttDocuments)
        Return DocumentsAndLinksBll.GetDocumentsAndLinks(TaskHeaderNumber, TaskItemNumber)
    End Function

    Private Sub GetDocumentsAndLinks()
        Dim attachmentsList As System.Collections.Generic.List(Of MttDocuments) = GetAttachmentsList()

        Me._grvAttachedFiles.DataSource = attachmentsList
        Me._grvAttachedFiles.DataBind()
        Me._grvAttachedFiles.AutoGenerateColumns = False
        AttachmentCount = Me._grvAttachedFiles.Rows.Count

        Me._grdAttachmentList.DataSource = attachmentsList
        Me._grdAttachmentList.DataBind()
        Me._grdAttachmentList.AutoGenerateColumns = False
        RaiseEvent DocumentsLoaded()
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
#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            IP.Bids.SharedFunctions.DisablePageCache(Response)
            SetUploadFolder()

            mCurrentUser = IP.Bids.SharedFunctions.GetCurrentUser
            TaskHeaderNumber = GetTaskHeader()
            TaskItemNumber = GetTaskItemNumber()
            If Request.Files.Count > 0 Then Exit Sub
            'If _fileUpload.IsInFileUploadPostBack Then Exit Sub
            'Verify Required Fields

            If Me.TaskHeaderNumber.Length = 0 Then
                Throw New MissingFieldException("Task Header Number is missing!  Please provide the TaskHeaderNumber property with a value")
                Exit Sub
            End If
            If IsNumeric(Me.TaskHeaderNumber) = False Then
                Throw New MissingFieldException("Task Header Number needs to be numeric!  Please provide the TaskHeaderNumber property with a value")
                Exit Sub
            End If
            If Me.SaveAsPath.Length = 0 Then
                Throw New MissingFieldException("The Path to File Uploads is missing!  Please provide the SaveAsPath property with a value")
            End If

            Dim taskSite As New TaskTrackerSiteBll
            Dim taskHeader As TaskHeaderBll = Nothing
            Try
                taskHeader = New TaskHeaderBll(CInt(TaskHeaderNumber))
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("Load", "Error attempting to create a new instance of TaskHeaderBLL for [" & TaskHeaderNumber & "]", ex)
            End Try
            If taskHeader IsNot Nothing AndAlso taskHeader.CurrentTaskHeaderRecord IsNot Nothing Then
                If taskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID = CStr(IP.Bids.SharedFunctions.SourceSystemID.Template) Then 'Template Header
                    Dim siteList As System.Collections.Generic.List(Of BusinessRegionSite) = Nothing
                    siteList = taskSite.GetBusinessRegionFacility(TaskHeaderNumber)

                    If siteList IsNot Nothing AndAlso siteList.Count > 0 Then
                        For i As Integer = 0 To siteList.Count - 1
                            If siteList.Item(i).ProcessedFlag.ToUpper = "Y" Then
                                Me._btnAttachLink.Enabled = False
                                Exit For
                            End If
                        Next
                    End If
                End If
                taskHeader = Nothing
            End If

            'Me._txtLinkToAttach.Attributes.Add("onchange", "alert(MultiFileUpload.is_URL_Available(this.value));")
            'Determine if this is the 1st time the page has been loaded

            'Me._btnBrowse.Attributes.Add("onclick", "try{document.getElementById('" & Me._fileUpload.ClientID & "').click();}catch(e){} return false;")
            Me.SetupPage()
            If Page.IsPostBack = False Then
                GetDocumentsAndLinks()
            End If
            If IP.Bids.SharedFunctions.CausedPostBack("_btnRefreshList") Then
                GetDocumentsAndLinks()
            End If
            'If Page.IsPostBack = False Then
            '    'Set control defaults
            '    'Me._pnlLinkAttachments.Style.Item("display") = "none"
            '    'Me._pnlFileAttachments.Style.Item("display") = ""
            '    'With Me._rblFileAttachments
            '    '    If .Items.Count > 0 Then
            '    '        .Items(0).Selected = False
            '    '        .Items(1).Selected = True 'Link Attachments is the default action
            '    '    End If
            '    'End With
            '    Me.SetupPage()
            '    'Me._btnBrowse.OnClientClick = "return false;"
            '    GetDocumentsAndLinks()
            'Else

            '    'Setup Page
            '    Me.SetupPage()
            '    'With Me._rblFileAttachments
            '    '    If .Items.Count >= 2 Then
            '    '        If Me._rblFileAttachments.Items(0).Selected = True Then
            '    '            Me._pnlLinkAttachments.Style.Item("display") = "none"
            '    '            Me._pnlFileAttachments.Style.Item("display") = ""
            '    '        ElseIf Me._rblFileAttachments.Items(1).Selected = True Then
            '    '            Me._pnlFileAttachments.Style.Item("display") = "none"
            '    '            Me._pnlLinkAttachments.Style.Item("display") = ""
            '    '        End If
            '    '    End If
            '    'End With
            'End If


            Page.ClientScript.RegisterClientScriptInclude("FileAttachments", Page.ResolveUrl("~/Scripts/FileAttachments.js"))
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("FileUpload", , ex)
        End Try
    End Sub


    'Protected Sub _rblFileAttachments_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles _rblFileAttachments.PreRender
    '    For Each l As ListItem In Me._rblFileAttachments.Items
    '        If l.Value = "1" Then
    '            l.Attributes.Add("onclick", "MultiFileUpload.toggleAttachments('" & _pnlFileAttachments.ClientID & "','" & _pnlLinkAttachments.ClientID & "',0);")
    '        ElseIf l.Value = "2" Then
    '            l.Attributes.Add("onclick", "MultiFileUpload.toggleAttachments('" & _pnlFileAttachments.ClientID & "','" & _pnlLinkAttachments.ClientID & "',1);")
    '        End If
    '    Next
    'End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'ApplyStyles()
        _tabAttachmentContainer.Visible = AllowAttachmentUpload
        '_tabFileAttachments.Visible = AllowAttachmentUpload
        'Me._pnlFileAttachments.Visible = AllowAttachmentUpload
        'Me._pnlLinkAttachments.Visible = AllowAttachmentUpload
        'Me._rblFileAttachments.Visible = AllowAttachmentUpload
        Me._grvAttachedFiles.Columns.Item(1).Visible = AllowAttachmentUpload
        ' Me._grvAttachedFiles.Columns.Item(2).Visible = AllowAttachmentUpload
        _pnlAttachments.Visible = (AllowAttachmentUpload = False)
        _pnlAttachedFiles.Visible = AllowAttachmentUpload

    End Sub

    Protected Sub _btnAttachLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnAttachLink.Click
        Try
            If Me._txtLinkToAttach.Text.Length > 0 Then
                'Save Link
                'Dim DocumentsAndLinks As New DocumentsAndLinksBLL
                'If DoesURLExists(Me._txtLinkToAttach.Text) Then
                DocumentsAndLinksBll.SaveDocumentsAndLinks("-1", Me.TaskHeaderNumber, Me.TaskItemNumber, "", Me._txtLinkToAttach.Text, Me._txtLinkDescription.Text, mCurrentUser.Username, "I") 'TODO:  Get Current User master.CurrentUser.Username 
                _lblLinkStatus.Text = "Link Attachment Successful"
                Me._txtLinkToAttach.Text = String.Empty
                Me._txtLinkDescription.Text = String.Empty
                'Else
                '    _lblLinkStatus.Text = Me._txtLinkToAttach.Text & " is not a valid url"
                'End If
                'Span1.InnerHtml = "File Uploaded Sucessfully."
                GetDocumentsAndLinks()


            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Protected Sub _grvAttachedFiles_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles _grvAttachedFiles.RowCancelingEdit
        Me._grvAttachedFiles.EditIndex = -1
        GetDocumentsAndLinks()
    End Sub


    Protected Sub _grvAttachedFiles_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles _grvAttachedFiles.RowCreated
        Dim data As IP.MEAS.BO.MttDocuments = CType(e.Row.DataItem, MttDocuments) 'CType(_grvAttachedFiles.Rows(e.Row.RowIndex).DataItem, MttDocuments) 'e.Row.DataItem, MttDocuments)
        If data Is Nothing Then
                Exit Sub
            End If

        'Dim fileTxt As TextBox = CType(e.Row.FindControl("_txtFileName"), TextBox)

        ''I need to find a way to look at the filename field to determine if it is an attachment or file
        'If fileTxt IsNot Nothing AndAlso data.FileName IsNot Nothing Then
        '    'If data.FileName.Length = 0 Then 'Link
        '    '    fileTxt.ReadOnly = False
        '    'Else
        '    fileTxt.ReadOnly = True
        '            fileTxt.BorderStyle = BorderStyle.None
        '            fileTxt.BorderWidth = 0
        '    'End If
        'End If

        Dim rowindex As Integer = e.Row.RowIndex
            Dim hiddenRowIndex As HiddenField = CType(e.Row.FindControl("_rowIndex"), HiddenField)
            If hiddenRowIndex IsNot Nothing Then
                hiddenRowIndex.Value = CStr(rowindex)
            End If
        'GetDocumentsAndLinks()
        'End If
    End Sub




    Protected Sub _grvAttachedFiles_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles _grvAttachedFiles.RowDeleting
        'Dim DocumentsAndLinks As New DocumentsAndLinksBLL
        'If DocumentsAndLinks IsNot Nothing Then
        Dim id As String = Me._grvAttachedFiles.DataKeys(e.RowIndex).Value.ToString
        DeleteAttachment(e.RowIndex)
        DocumentsAndLinksBll.DeleteDocumentsAndLinks(id, mCurrentUser.Username, "D")

        GetDocumentsAndLinks()
        'End If
    End Sub

    Protected Sub _grvAttachedFiles_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles _grvAttachedFiles.RowEditing

        Me._grvAttachedFiles.EditIndex = e.NewEditIndex
        GetDocumentsAndLinks()

        'Dim data As IP.MEAS.BO.MttDocuments = CType(_grvAttachedFiles.Rows(e.NewEditIndex).DataItem, MttDocuments) 'e.Row.DataItem, MttDocuments)
        'If data Is Nothing Then
        '    Exit Sub
        'End If
        'Edit mode
        'Dim fileTxt As TextBox = CType(_grvAttachedFiles.Rows(e.NewEditIndex).FindControl("_txtFileName"), TextBox)
        'Dim location As HiddenField = CType(_grvAttachedFiles.Rows(e.NewEditIndex).FindControl("_Location"), HiddenField)
        'If fileTxt IsNot Nothing AndAlso location IsNot Nothing Then
        '    If location.Value.Length = 0 Then 'Link
        '        fileTxt.ReadOnly = False
        '    Else
        '        fileTxt.ReadOnly = True
        '        fileTxt.BorderStyle = BorderStyle.None
        '        fileTxt.BorderWidth = 0
        '    End If
        'End If

        'Dim rowindex As Integer = _grvAttachedFiles.Rows(e.NewEditIndex).RowIndex
        'Dim hiddenRowIndex As HiddenField = CType(_grvAttachedFiles.Rows(e.NewEditIndex).FindControl("_rowIndex"), HiddenField)
        'If hiddenRowIndex IsNot Nothing Then
        '    hiddenRowIndex.Value = CStr(rowindex)
        'End If
        'GetDocumentsAndLinks()

    End Sub

    'Protected Sub _grvAttachedFiles_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles _grvAttachedFiles.RowUpdating

    '    Dim location As HiddenField = TryCast(Me._grvAttachedFiles.Rows.Item(e.RowIndex).FindControl("_Location"), HiddenField)
    '    Dim file As HiddenField = TryCast(Me._grvAttachedFiles.Rows.Item(e.RowIndex).FindControl("_FileName"), HiddenField)
    '    Dim taskDocumentSeqId As HiddenField = TryCast(Me._grvAttachedFiles.Rows.Item(e.RowIndex).FindControl("_TaskDocumentSeqID"), HiddenField)
    '    '        Dim taskHeaderSeqID As HiddenField = TryCast(Me._grvAttachedFiles.Rows.Item(e.RowIndex).FindControl("_TaskHeaderSeqID"), HiddenField) ' COMMENTED BY CODEIT.RIGHT
    '    Dim description As TextBox = TryCast(Me._grvAttachedFiles.Rows.Item(e.RowIndex).FindControl("_txtDescription"), TextBox)
    '    'Dim FileName As HiddenField = TryCast(Me._grvAttachedFiles.Rows.Item(e.RowIndex).FindControl("_FileName"), HiddenField)
    '    Dim fileName As TextBox = CType(Me._grvAttachedFiles.Rows.Item(e.RowIndex).FindControl("_txtFileName"), TextBox)
    '    '_lblFileUploadStatus.Text = fileName.Text & " was updated Sucessfully"
    '    'Dim DocumentsAndLinks As New DocumentsAndLinksBLL
    '    If fileName.Text = file.Value Then 'No change has been made to the filename

    '    ElseIf file.Value = "" Then 'The filename is blank for attachments
    '        If fileName.Text <> location.Value Then 'link has changed
    '            location.Value = fileName.Text
    '        End If
    '    End If
    '    Try
    '        DocumentsAndLinksBll.SaveDocumentsAndLinks(taskDocumentSeqId.Value, Me.TaskHeaderNumber, Me.TaskItemNumber, file.Value, location.Value, description.Text, mCurrentUser.Username, "I") 'TODO:  Get Current User master.CurrentUser.Username 
    '        'Span1.InnerHtml = "File Uploaded Sucessfully."
    '        e.Cancel = True
    '        Me._grvAttachedFiles.EditIndex = -1
    '        GetDocumentsAndLinks()
    '    Catch ex As Exception
    '        IP.Bids.SharedFunctions.HandleError("_grvAttachedFiles_RowUpdating", "Error Updating [" & fileName.Text & "].", ex, "An error occured while attempting to update [" & fileName.Text & "].  Please retry your update.")
    '    End Try
    'End Sub

    Public Sub EditAttachment(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As Button = TryCast(sender, Button)
        If btn IsNot Nothing AndAlso IsNumeric(btn.CommandArgument) Then
            SetupPage()
            Me._grvAttachedFiles.EditIndex = CInt(btn.CommandArgument)
            GetDocumentsAndLinks()
        End If
    End Sub
    Public Sub DeleteAttachment(ByVal rowIndex As Integer)
        Dim attachmentList = GetAttachmentsList()
        If attachmentList Is Nothing OrElse attachmentList.Count = 0 Then Exit Sub

        If attachmentList.Count < rowIndex Then Exit Sub

        Dim fileToDelete As String = attachmentList(rowIndex).FileName
        Dim filepath As String = SaveAsPath

        If filepath.EndsWith("\") Then
            filepath = filepath & fileToDelete
        Else
            filepath = filepath & "\" & fileToDelete
        End If
        If IO.File.Exists(filepath) Then
            IO.File.Delete(filepath)
        End If

    End Sub
    Public Sub UpdateAttachment(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As Button = TryCast(sender, Button)

        If btn IsNot Nothing AndAlso IsNumeric(btn.CommandArgument) Then

        End If
    End Sub
    Public Sub CancelEditAttachment(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As Button = TryCast(sender, Button)

        If btn IsNot Nothing AndAlso IsNumeric(btn.CommandArgument) Then
            ' taskTrackerBLL.DeleteSubTaskItem(CInt(btn.CommandArgument))
            'GetSubTasks(TaskItemNumber)
        End If

    End Sub
    Private Sub SetupPage()
        _dragDrop.Src = Page.ResolveUrl("~/Popups/DragDropAttachments.aspx" & Request.Url.Query)
    End Sub

    Private Sub SetUploadFolder()
        Dim uploadsUrl As String = ConfigurationManager.AppSettings.Item("DevelopmentServer")
        Dim uploadsFolder As String = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")

        ' =========================================================================================
        'JEB - commented out on 1/18/2019

        'If HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("localhost") Then
        '    uploadsUrl = ConfigurationManager.AppSettings.Item("DevelopmentUploadsUrl")
        '    uploadsFolder = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")
        'ElseIf HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("ridev") Then
        '    uploadsUrl = ConfigurationManager.AppSettings.Item("DevelopmentUploadsUrl")
        '    uploadsFolder = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")
        'ElseIf HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("ritest") Then
        '    uploadsUrl = ConfigurationManager.AppSettings.Item("DevelopmentUploadsUrl")
        '    uploadsFolder = ConfigurationManager.AppSettings.Item("DevelopmentUploadsFolder")
        'Else
        '    uploadsUrl = ConfigurationManager.AppSettings.Item("ProductionUploadsUrl")
        '    uploadsFolder = ConfigurationManager.AppSettings.Item("ProductionUploadsFolder")
        'End If
        ' =========================================================================================


        uploadsUrl = ConfigurationManager.AppSettings.Item("ProductionUploadsUrl")
        uploadsFolder = ConfigurationManager.AppSettings.Item("ProductionUploadsFolder")

        Me.UploadsUrl = uploadsUrl
        Me.SaveAsPath = uploadsFolder
    End Sub

    'Private Sub _tabAttachmentContainer_ActiveTabChanged(sender As Object, e As EventArgs) Handles _tabAttachmentContainer.ActiveTabChanged
    '    If _tabAttachmentContainer.ActiveTabIndex = 2 Then Me.GetDocumentsAndLinks()
    'End Sub

    Private Sub _btnRefreshList_Click(sender As Object, e As EventArgs) Handles _btnRefreshList.Click
        GetDocumentsAndLinks()
    End Sub

    Private Sub _btnRefreshFiles_Click(sender As Object, e As EventArgs) Handles _btnRefreshFiles.Click
        GetDocumentsAndLinks()
    End Sub

    Private Sub _lnkSaveAttachments_Click(sender As Object, e As EventArgs) Handles _lnkSaveAttachments.Click
        For Each r As GridViewRow In _grvAttachedFiles.Rows
            UpdateDescription(r.RowIndex)
        Next
        GetDocumentsAndLinks()
    End Sub

    Private Sub UpdateDescription(ByVal rowIndex As Integer)
        Dim rowHasChanged As Boolean = False
        Dim location As HiddenField = TryCast(Me._grvAttachedFiles.Rows.Item(rowIndex).FindControl("_Location"), HiddenField)
        Dim file As HiddenField = TryCast(Me._grvAttachedFiles.Rows.Item(rowIndex).FindControl("_FileName"), HiddenField)
        Dim taskDocumentSeqId As HiddenField = TryCast(Me._grvAttachedFiles.Rows.Item(rowIndex).FindControl("_TaskDocumentSeqID"), HiddenField)
        Dim hiddenDescription As HiddenField = TryCast(Me._grvAttachedFiles.Rows.Item(rowIndex).FindControl("_Description"), HiddenField) ' COMMENTED BY CODEIT.RIGHT
        Dim description As TextBox = TryCast(Me._grvAttachedFiles.Rows.Item(rowIndex).FindControl("_txtDescription"), TextBox)
        'Dim FileName As HiddenField = TryCast(Me._grvAttachedFiles.Rows.Item(e.RowIndex).FindControl("_FileName"), HiddenField)
        'Dim fileName As TextBox = CType(Me._grvAttachedFiles.Rows.Item(rowIndex).FindControl("_txtFileName"), TextBox)
        '_lblFileUploadStatus.Text = fileName.Text & " was updated Sucessfully"
        'Dim DocumentsAndLinks As New DocumentsAndLinksBLL
        'If fileName.Text = file.Value Then 'No change has been made to the filename

        'ElseIf file.Value = "" Then 'The filename is blank for attachments
        '    If fileName.Text <> location.Value Then 'link has changed
        '        location.Value = fileName.Text
        '        rowHasChanged = True
        '    End If
        'End If
        If hiddenDescription.Value <> description.Text Then
            rowHasChanged = True
        End If
        Try
            If rowHasChanged Then
                DocumentsAndLinksBll.SaveDocumentsAndLinks(taskDocumentSeqId.Value, Me.TaskHeaderNumber, Me.TaskItemNumber, file.Value, location.Value, description.Text, mCurrentUser.Username, "I") 'TODO:  Get Current User master.CurrentUser.Username 
            End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("_grvAttachedFiles_RowUpdating", "Error Updating [" & file.Value & "].", ex, "An error occured while attempting to update [" & file.Value & "].  Please retry your update.")
        End Try
    End Sub
End Class
