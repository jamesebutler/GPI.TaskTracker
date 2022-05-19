Option Explicit On
Option Strict On
Imports IP.Bids
Partial Class TaskHeaderEntry
    Inherits IP.Bids.BasePage

#Region "Fields"
    Private taskHeader As TaskHeaderBll 'Holds an instance of the Task Header Business Logic Layer
    Private currentTaskHeaderRecord As BO.TaskHeaderRecord  'Holds the Header Record details for the current task        
    Private taskHeaderNumber As String = String.Empty 'Holds the current Header Number
    Private headerIsReplicated As Boolean 'Indicates that the current header has been replicated
#End Region

#Region "Methods"
    ''' <summary>
    ''' Purpose is to redirect the user to the New Task Header screen
    ''' </summary>
    Private Sub DisplayNewTaskHeader()
        Try
            IP.Bids.SiteURLs.GetTaskHeaderURL(RefSite)
            IP.Bids.SharedFunctions.ResponseRedirect(Page.AppRelativeVirtualPath)
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("DisplayNewTaskHeader", , ex)
        End Try
    End Sub

    Private Sub PopulateQueryStringData()
        Try
            taskHeaderNumber = Request.QueryString(QueryStringConstants.HeaderNumber)
            If taskHeaderNumber Is Nothing Then taskHeaderNumber = String.Empty

            RefSite = Request.QueryString(ReferenceSite)
            If RefSite Is Nothing Then RefSite = String.Empty
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateQueryStringData", , ex)
        End Try
    End Sub

    ''' <summary>
    ''' Purpose is to handle all of the tasks that should occur when this page is loaded
    ''' </summary>
    Private Sub HandlePageLoad()
        Try
            Master.SetActiveNav(Enums.NavPages.TaskHeader.ToString)
            PopulateQueryStringData()
            IP.Bids.SharedFunctions.DisablePageCache(Response)
            If Page.IsPostBack = False Then
                PopulateListData()
                SetPageDefaults()
            End If

            ConfigureButtonsForNewTaskHeader()
            ConfigureReplicationDialog()
            ConfigureTemplateTaskDialog()
            ConfigurePageForExternalReferences()

            If Not IsNumeric(taskHeaderNumber) Then
                PrepareNewTaskHeader()
                Exit Sub
            End If

            'Populate Task Header
            If PopulateTaskHeader() = False Then
                'The user has entered an invalid header number so let's force them to create a new one
                IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetTaskHeaderURL("", RefSite))
                Exit Sub
            End If

            currentTaskHeaderRecord = taskHeader.CurrentTaskHeaderRecord
            _ImportTasks.Visible = True
            _lnkEditTasks.Visible = True
            '_lnkTaskItems.Text = Master.IPResources.GetResourceValue("Add Task Items", True)
            _lnkDeleteHeader.OnClientClick = _cbeDelete.TriggerPopupJS & ";return false;"
            _lnkDeleteHeader.Visible = True
            _lnkEditTasks.OnClientClick = "JQConfirmRedirect('" & Page.ResolveClientUrl(IP.Bids.SiteURLs.GetMultiTasksEditURL(taskHeaderNumber, RefSite)) & "',false);return false;"
            _ImportTasks.Url = Page.ResolveUrl(IP.Bids.SiteURLs.GetTaskImportURL(taskHeaderNumber, RefSite))
            _ImportTasks.AllowChildToCloseParent = True
            _ImportTasks.ReloadPageOnClose = True

            If currentTaskHeaderRecord.ExternalSourceSeqID = CStr(IP.Bids.SharedFunctions.SourceSystemID.Replicated) Then 'Replicated Header
                Master.PageName = Master.GetLocalizedValue("Replicated Task Header", False) & " '" & taskHeaderNumber & "'"
                _btnAssignReplication.Visible = False
            ElseIf currentTaskHeaderRecord.ExternalSourceSeqID = CStr(IP.Bids.SharedFunctions.SourceSystemID.Template) Then 'Templated Header
                Master.PageName = Master.GetLocalizedValue("Template Task Header", False) & " '" & taskHeaderNumber & "'"

                _btnAssignReplication.Visible = True
                _btnAssignReplication.Url = Page.ResolveUrl(IP.Bids.SiteURLs.GetReplicationURL(taskHeaderNumber, RefSite))
                _btnAssignReplication.AllowChildToCloseParent = True
                _btnAssignReplication.ReloadPageOnClose = True

                Dim taskItem As New TaskTrackerItemBll
                Dim tasklist As System.Collections.Generic.List(Of TaskItem)
                If taskItem IsNot Nothing Then
                    tasklist = taskItem.GetRootTaskItemList(CInt(taskHeaderNumber))
                    If tasklist IsNot Nothing AndAlso tasklist.Count > 0 Then
                        _btnAssignReplication.Enabled = True
                    Else
                        _btnAssignReplication.Enabled = False
                    End If
                End If

                taskItem = Nothing
                tasklist = Nothing
            ElseIf currentTaskHeaderRecord.ExternalSourceSeqID = CStr(IP.Bids.SharedFunctions.SourceSystemID.OutageTemplate) Then 'Outage Template
                Master.PageName = Master.GetLocalizedValue("Outage Template Task Header", False) & " '" & taskHeaderNumber & "'"
                Me._Attachments.Visible = False
                _Attachments.Enabled = False
            Else
                'Normal Header
                Master.PageName = Master.GetLocalizedValue("Update Task Header", False) & " '" & taskHeaderNumber & "'"
            End If
            _lblHeaderTitle.Text = Master.PageName
            _pnlBusinessManager.Visible = True
            _pnlTaskTypeManager.Visible = True
            _pnlTaskHeaderEdit.Visible = True
            _Attachments.ScreenHeight = Me.ScreenHeight
            _Attachments.ScreenWidth = Me.ScreenWidth
            _Attachments.BannerText = Master.GetLocalizedValue("Documents and Links", True)
            _Attachments.DisplayModalButtonText = Master.GetLocalizedValue("Attachments/Links", True) & " (" & currentTaskHeaderRecord.AttachmentCount & ")"
            Me._Attachments.Visible = True
            Me._lnkTaskItems.Visible = True
            Me._Attachments.Enabled = True
            Me._lnkTaskItems.Enabled = True
            Me._Attachments.Url = Page.ResolveUrl(IP.Bids.SiteURLs.GetAttachmentURL(taskHeaderNumber, RefSite))

            If Page.IsPostBack = False Then
                ReSelectControls()
                If headerIsReplicated Then
                    _ifrTaskItems.Attributes.Item("src") = Page.ResolveUrl(IP.Bids.SiteURLs.GetTaskListURL(taskHeaderNumber, RefSite, True, False, False))
                End If
            End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("HandlePageLoad", , ex)
        End Try
    End Sub

    Private Function PopulateTaskHeader() As Boolean
        'Populate Task Header
        Try
            If IsNumeric(taskHeaderNumber) Then
                taskHeader = New TaskHeaderBll(CInt(taskHeaderNumber))
                If taskHeader IsNot Nothing AndAlso taskHeader.CurrentTaskHeaderRecord IsNot Nothing Then
                    DisplayTaskItemsForCurrentTaskHeader()
                Else
                    IP.Bids.SharedFunctions.HandleError("PopulateTaskHeader", "Error attempting to create a new instance of TaskHeaderBLL for [" & taskHeaderNumber & "]", New ApplicationException(String.Format("Error loading data for Task Header number {0}", taskHeaderNumber)), String.Format("Error loading data for Task Header number {0}", taskHeaderNumber))
                    IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetTaskHeaderURL("", RefSite))
                    Return False
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateTaskHeader", "Error attempting to create a new instance of TaskHeaderBLL for [" & taskHeaderNumber & "]", ex, String.Format("Error loading data for Task Header number {0}", taskHeaderNumber))
            IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetTaskHeaderURL("", RefSite))
            Return False
        End Try
        If taskHeader IsNot Nothing AndAlso taskHeader.CurrentTaskHeaderRecord IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub DisplayTaskItemsForCurrentTaskHeader()
        'Display Task Items for the current Header
        _ifrTaskItems.Attributes.Item("src") = Page.ResolveUrl(IP.Bids.SiteURLs.GetTaskListURL(taskHeaderNumber, RefSite, True, True, False))
        '_ifrTaskItems.Attributes.Item("onload") = "alert('Loaded');$('#" & _ifrTaskItems.ClientID & "').load(function() {$('#" & _ifrTaskItems.ClientID & "').css('display', 'block'); $('#preload-img').css('display', 'none');  resizeFrame(document.getElementById('" & Me._ifrTaskItems.ClientID & "'));});"
        _ifrTaskItems.Attributes.Item("onload") = "$('#" & _ifrTaskItems.ClientID & "').css('display', 'block'); $('#preload-img').css('display', 'none');  resizeFrame(document.getElementById('" & Me._ifrTaskItems.ClientID & "'));"
    End Sub

    Public Sub ConfigurePageForExternalReferences()
        Dim listOfExternalSites = {Iris.ToLower, Moc.ToLower, Reliability.ToLower, Outage.ToLower, Tanks.ToLower}
        If listOfExternalSites.Contains(RefSite.ToLower) Then
            Master.HideFooter()
            Master.HideHeaderAndMenu()
        End If
    End Sub

    Private Sub PrepareNewTaskHeader()
        'New Task Header
        _ifrTaskItems.Attributes.Item("src") = Page.ResolveUrl("~/blank.htm")
        _ifrTaskItems.Attributes.Item("onload") = "$('#" & _ifrTaskItems.ClientID & "').css('display', 'block'); $('#preload-img').css('display', 'none');  resizeFrame(document.getElementById('" & Me._ifrTaskItems.ClientID & "'));"
        '_ifrTaskItems.Attributes.Item("onload") = "$('#" & _ifrTaskItems.ClientID & "').load(function() {$('#" & _ifrTaskItems.ClientID & "').css('display', 'block'); $('#preload-img').css('display', 'none');  resizeFrame(document.getElementById('" & Me._ifrTaskItems.ClientID & "'));});"

        Master.PageName = Master.GetLocalizedValue("New Task Header", False)
        _lblHeaderTitle.Text = Master.PageName
        ' _trNotification.Visible = False
        Me._Attachments.Visible = False
        Me._lnkTaskItems.Visible = False
        _btnAssignReplication.Visible = False
        _pnlBusinessManager.Visible = False
        _pnlTaskTypeManager.Visible = False
    End Sub
    Private Sub ConfigureTemplateTaskDialog()
        With _confirmationDialogTemplateTasks
            .Message = Master.IPResources.GetResourceValue("[Outage Template] Warning", True) '<b>Outage Template</b>' should only be used to create template task(s) that will be used for an Outage record.  Do you want to continue?")
            .Title = Master.IPResources.GetResourceValue("Outage Template Tasks", True)
            .ButtonType = UserControl.MessageBox.ButtonType1.YesNo
            .OKScript = "return false;"
            .CancelScript = "try{$get('" & _ddlSourceSystem.ClientID & "').selectedIndex=-1}catch(e){};"
        End With
    End Sub
    Private Sub ConfigureReplicationDialog()
        With _confirmationDialogReplicationTasks
            .Message = Master.IPResources.GetResourceValue("[MTT Template] Warning", True)
            .Title = Master.IPResources.GetResourceValue("Replication Tasks", True)
            .ButtonType = UserControl.MessageBox.ButtonType1.YesNo
            .OKScript = "return false;"
            .CancelScript = "try{$get('" & _ddlSourceSystem.ClientID & "').selectedIndex=-1}catch(e){};"
        End With
    End Sub
    Private Sub ConfigureButtonsForNewTaskHeader()
        '_btnShowMyTasks.ScreenHeight = ScreenHeight
        '_btnShowMyTasks.ScreenWidth = ScreenWidth
        '_btnShowMyTasks.Enabled = True
        _btnNewTaskHeader.OnClientClick = "JQConfirmRedirect('" & Page.ResolveClientUrl(IP.Bids.SiteURLs.GetTaskHeaderURL("", RefSite)) & "',false); return false;"
        _lnkDeleteHeader.Visible = False
        _btnAssignReplication.Visible = False
        _ImportTasks.Visible = False
        _lnkEditTasks.Visible = False
    End Sub
    Private Sub PopulateExternalSourceButton(ByVal currentSourceId As Integer, ByVal externalSourceReferenceNumber As String)
        Dim newIncident As TaskTrackerListsBll
        Dim displaySourceSystemJs As String = "PopupWindow('{0}{2}','{1}',600,600,'yes','yes','true');return false;"
        Dim sourceFound As Boolean
        Dim externalSourceName As String = String.Empty
        Try
            newIncident = New TaskTrackerListsBll()
            If newIncident IsNot Nothing Then
                Dim externalSites As System.Collections.Generic.List(Of SourceSystems) = newIncident.GetSourceSystems

                For Each item As SourceSystems In externalSites
                    If item.ExternalSourceSeqid = currentSourceId Then
                        _btnReturnToSourceSystem.OnClientClick = String.Format(displaySourceSystemJs, item.ExternalSourceUrl, "MTTSource", externalSourceReferenceNumber)
                        sourceFound = True
                        externalSourceName = item.ExternalSource
                    End If
                Next
            End If
            If sourceFound = True Then
                _btnReturnToSourceSystem.Visible = True
                _lblReturnToSourceSystem.Text = Master.IPResources.GetResourceValue("Go To") & " " & Master.GetLocalizedValue(externalSourceName, True)
            Else
                _btnReturnToSourceSystem.Visible = False
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateExternalSourceButton", , ex)
        Finally
            newIncident = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Purpose of this method is to populate all listitems with the required data
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateListData()
        Try
            Dim newIncident As New TaskTrackerListsBll
            If newIncident IsNot Nothing Then
                Dim types As System.Collections.Generic.List(Of TaskType) = newIncident.GetTaskTypes
                Dim activities As System.Collections.Generic.List(Of TaskActivity) = newIncident.GetActivities
                Dim externalSites As System.Collections.Generic.List(Of SourceSystems) = newIncident.GetSourceSystems

                _cblIncidentType.RepeatLayout = RepeatLayout.Flow
                _cblIncidentType.RepeatDirection = RepeatDirection.Horizontal
                _cblIncidentType.RepeatColumns = 12
                For Each item As TaskType In types
                    If _cblIncidentType.Items.FindByValue(CStr(item.TaskSeqid)) Is Nothing Then
                        _cblIncidentType.Items.Add(New ListItem(Master.GetLocalizedValue(item.TaskName, True), CStr(item.TaskSeqid)))
                        _cblIncidentType.Items.FindByValue(CStr(item.TaskSeqid)).Attributes.Add("class", "col-xs-12 col-sm-4")
                    End If
                Next

                _rblActivity.RepeatLayout = RepeatLayout.Flow
                _rblActivity.RepeatDirection = RepeatDirection.Horizontal
                _rblActivity.RepeatColumns = 12
                For Each item As TaskActivity In activities
                    If _rblActivity.Items.FindByValue(CStr(item.ActivitySeqid)) Is Nothing Then
                        _rblActivity.Items.Add(New ListItem(Master.GetLocalizedValue(item.ActivityName, True), CStr(item.ActivitySeqid)))
                        If item.ActivityName.ToLower = "other" Then
                            _rblActivity.Items.FindByValue(CStr(item.ActivitySeqid)).Selected = True
                        End If
                        _rblActivity.Items.FindByValue(CStr(item.ActivitySeqid)).Attributes.Add("class", "col-xs-12 col-sm-4")
                    End If
                Next

                Dim sourceSystemMessageJS As String
                sourceSystemMessageJS = "if (this.value==" & IP.Bids.SharedFunctions.SourceSystemID.Template & "){" & _confirmationDialogReplicationTasks.TriggerPopupJS & "};if (this.value==" & IP.Bids.SharedFunctions.SourceSystemID.OutageTemplate & "){" & _confirmationDialogTemplateTasks.TriggerPopupJS & "}"
                _ddlSourceSystem.Attributes.Add("onchange", sourceSystemMessageJS)
                For Each item As SourceSystems In externalSites
                    If Me._ddlSourceSystem.Items.FindByValue(CStr(item.ExternalSourceSeqid)) Is Nothing Then
                        Select Case item.ExternalSourceSeqid
                            Case IP.Bids.SharedFunctions.SourceSystemID.MemphisEHS, IP.Bids.SharedFunctions.SourceSystemID.Template, IP.Bids.SharedFunctions.SourceSystemID.OutageTemplate
                                Dim newItem As New ListItem(Master.GetLocalizedValue(item.ExternalSource, True), CStr(item.ExternalSourceSeqid))
                                _ddlSourceSystem.Items.Add(newItem)
                            Case Else
                                'Don't add the other source systems at this time.  they should only be added as needed
                        End Select
                    End If
                Next
            Else
                IP.Bids.SharedFunctions.HandleError("PopulateListData", "Error Instantiating clsEnterIncident")
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateListData", "Error Populating Task Header Data", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Purpose of this method is to reselect the controls based on the data for the current header record
    ''' </summary>
    ''' <remarks>This method is called by HandlePageLoad after data for the specified Header Number has been retrieved.</remarks>
    Private Sub ReSelectControls()
        Try
            'lblResult.Text = "This is a test"
            'lblResult.Text = lblResult.Text & " to " & t.TranslateMethod("this is a test", "en-us", "ru-ru")

            If currentTaskHeaderRecord IsNot Nothing Then
                With currentTaskHeaderRecord
                    'Reset the Plant Model Selections
                    _site.DefaultBusinessUnitArea = .BusinessUnitArea
                    _site.DefaultFacility = .PlantCode
                    _site.DefaultRegion = .Region
                    _site.DefaultDivision = .Division
                    _site.DefaultLineBreak = .Line

                    _txtDescription.Text = .Description
                    '_lblTranslatedDescription.Text = Translation.TranslateUsingApi(.Description)
                    If _lblTranslatedDescription.Text.Trim = .Description.Trim Then _lblTranslatedDescription.Text = String.Empty
                    _txtTitle.Text = .Title
                    '_lblTranslatedTitle.Text = Translation.TranslateUsingApi(.Title)
                    If _lblTranslatedTitle.Text.Trim = .Title.Trim Then _lblTranslatedTitle.Text = String.Empty
                    If IsDate(.StartDate) Then
                        Me._dtStartEnd.StartDate = CDate(currentTaskHeaderRecord.StartDate).ToShortDateString 'IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(CurrentTaskHeaderRecord.StartDate, "EN-US", "d")
                    End If
                    If IsDate(.EndDate) Then
                        Me._dtStartEnd.EndDate = CDate(currentTaskHeaderRecord.EndDate).ToShortDateString 'IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(CurrentTaskHeaderRecord.EndDate.ToShortDateString, "EN-US", "d")
                    End If

                    If .ExternalSourceName.Length > 0 AndAlso IsNumeric(.ExternalSourceSeqID) Then
                        Select Case CDbl(.ExternalSourceSeqID)
                            Case IP.Bids.SharedFunctions.SourceSystemID.Template
                                'Template Task
                                Dim taskSite As TaskTrackerSiteBll
                                Dim siteList As System.Collections.Generic.List(Of BusinessRegionSite) = Nothing
                                taskSite = New TaskTrackerSiteBll
                                siteList = taskSite.GetBusinessRegionFacility(taskHeaderNumber)

                                If siteList IsNot Nothing Then
                                    If siteList.Count > 0 Then
                                        For i As Integer = 0 To siteList.Count - 1
                                            If siteList.Item(i).ProcessedFlag.ToUpper = "Y" Then
                                                headerIsReplicated = True
                                                Exit For
                                            End If
                                        Next
                                    End If
                                End If
                                taskSite = Nothing
                                siteList = Nothing
                                If headerIsReplicated Then
                                    _taskHeader.TaskHeaderNumber = CInt(taskHeaderNumber)
                                    _taskHeader.LoadHeaderInfo()
                                    _site.Visible = False
                                    _pnlTaskHeaderEdit.Visible = False
                                    ' Me._tblTaskHeader1.Visible = False
                                    'Me._tblTaskHeader2.Visible = False
                                    Me._lnkTaskItems.Enabled = False
                                    'Me._btnSaveHeader.Enabled = False
                                    _lnkSaveButton.Enabled = False
                                End If
                            Case IP.Bids.SharedFunctions.SourceSystemID.MemphisEHS, IP.Bids.SharedFunctions.SourceSystemID.EIS, IP.Bids.SharedFunctions.SourceSystemID.OutageTemplate ', IP.Bids.SharedFunctions.SourceSystemID.IRIS
                                If Me._ddlSourceSystem.Items.FindByValue(.ExternalSourceSeqID) Is Nothing Then
                                    Me._ddlSourceSystem.Items.Add(New ListItem(Master.GetLocalizedValue(.ExternalSourceName, True), .ExternalSourceSeqID))
                                End If
                                'Remove the MTT Template option from existing Task Headers that are not Templates
                                If Me._ddlSourceSystem.Items.FindByValue(CStr(IP.Bids.SharedFunctions.SourceSystemID.Template)) IsNot Nothing Then
                                    Me._ddlSourceSystem.Items.Remove(Me._ddlSourceSystem.Items.FindByValue(CStr(IP.Bids.SharedFunctions.SourceSystemID.Template)))
                                End If
                            Case IP.Bids.SharedFunctions.SourceSystemID.IRIS
                                If Me._ddlSourceSystem.Items.FindByValue(.ExternalSourceSeqID) Is Nothing Then
                                    Me._ddlSourceSystem.Items.Add(New ListItem(Master.GetLocalizedValue(.ExternalSourceName, True), .ExternalSourceSeqID))
                                End If
                                Me._ddlSourceSystem.Enabled = False
                                Me._txtIncidentEventNumber.ReadOnly = True
                                _dtStartEnd.Enabled = False
                                _cbHighLevelSecurity.Enabled = False
                                _txtDescription.Enabled = False
                                _txtTitle.Enabled = False
                                _site.DisableDivision = True
                                _site.DisableRegion = True
                            Case Else
                                If Me._ddlSourceSystem.Items.FindByValue(.ExternalSourceSeqID) Is Nothing Then
                                    Me._ddlSourceSystem.Items.Add(New ListItem(Master.GetLocalizedValue(.ExternalSourceName, True), .ExternalSourceSeqID))
                                End If
                                _taskHeader.TaskHeaderNumber = CInt(taskHeaderNumber)
                                _taskHeader.LoadHeaderInfo()
                                _site.Visible = False
                                _pnlTaskHeaderEdit.Visible = False
                                'Me._tblTaskHeader1.Visible = False
                                ' Me._tblTaskHeader2.Visible = False
                                'Me._btnSaveHeader.Enabled = False
                                _lnkSaveButton.Enabled = False
                                'Me._btnSpellCheck.Enabled = False
                                If .ExternalSourceSeqID IsNot Nothing AndAlso IsNumeric(.ExternalSourceSeqID) Then
                                    PopulateExternalSourceButton(CInt(.ExternalSourceSeqID), .ExternalRef)
                                    If .ExternalSourceName = "MTT Replication" Then
                                        _btnAssignReplication.Visible = True
                                        _btnReturnToSourceSystem.Visible = False
                                    Else
                                        _btnAssignReplication.Visible = False
                                        _btnReturnToSourceSystem.Visible = True
                                    End If

                                    'Remove the MTT Template option from existing Task Headers that are not Templates
                                    If Me._ddlSourceSystem.Items.FindByValue(CStr(IP.Bids.SharedFunctions.SourceSystemID.Template)) IsNot Nothing Then
                                            Me._ddlSourceSystem.Items.Remove(Me._ddlSourceSystem.Items.FindByValue(CStr(IP.Bids.SharedFunctions.SourceSystemID.Template)))
                                        End If
                                    End If
                        End Select
                    End If

                    'If .ExternalSourceName.Length > 0 AndAlso CDbl(.ExternalSourceSeqID) = IP.Bids.SharedFunctions.SourceSystemID.Replicated Then
                    '    'All sources except for Template, Memphis EHS, and EIS
                    '    'Replicated Header

                    'ElseIf CDbl(.ExternalSourceSeqID) = IP.Bids.SharedFunctions.SourceSystemID.Template Then '.GetTemplateSourceSeqID Then



                    'ElseIf CDbl(.ExternalSourceSeqID) <> IP.Bids.SharedFunctions.SourceSystemID.Template Then 'GetTemplateSourceSeqID Then
                    '    'Remove the MTT Template option from existing Task Headers that are not Templates
                    '    If Me._ddlSourceSystem.Items.FindByValue(CStr(IP.Bids.SharedFunctions.SourceSystemID.Template)) IsNot Nothing Then
                    '        Me._ddlSourceSystem.Items.Remove(Me._ddlSourceSystem.Items.FindByValue(CStr(IP.Bids.SharedFunctions.SourceSystemID.Template)))
                    '    End If
                    'End If
                    If Me._ddlSourceSystem.Items.FindByValue(.ExternalSourceSeqID) IsNot Nothing Then
                        _ddlSourceSystem.ClearSelection() 'Clear the current selection to prevent a multi selection exception
                        Me._ddlSourceSystem.Items.FindByValue(.ExternalSourceSeqID).Selected = True
                    End If
                    Me._txtIncidentEventNumber.Text = .ExternalRef

                    If .SecurityLevel.ToUpper.Trim = "Y" Then
                        Me._cbHighLevelSecurity.Checked = True
                    Else 'Assumes that value is 'N' or Null
                        Me._cbHighLevelSecurity.Checked = False
                    End If
                    If Me._rblActivity.Items.FindByValue(CStr(.ActivitySeqID)) IsNot Nothing Then
                        Me._rblActivity.ClearSelection() 'Clear the current selection to prevent a multi selection exception
                        Me._rblActivity.Items.FindByValue(CStr(.ActivitySeqID)).Selected = True
                    End If
                    Me._lblCreatedBy.Text = .CreatedBy
                    Me._lblCreationDate.Text = IP.Bids.SharedFunctions.FormatDate(.CreateDate) '.CreateDate.ToShortDateString
                    Me._lblLastUpdateDate.Text = IP.Bids.SharedFunctions.FormatDate(.LastUpdateDate) ' CDate(IP.Bids.SharedFunctions.DataClean(.LastUpdateDate)).ToShortDateString
                    Me._lblLastUpdatedBy.Text = .LastUpdateUserName
                End With
            End If

            If taskHeader IsNot Nothing Then
                _cblIncidentType.ClearSelection() 'Clear the currently selected items
                If taskHeader.TaskTypes IsNot Nothing AndAlso taskHeader.TaskTypes.Count > 0 Then
                    With taskHeader.TaskTypes
                        Dim taskItemList As New StringBuilder
                        For i As Integer = 0 To .Count - 1 'Build a comma delimited list of selected TaskTypes
                            If taskItemList.Length > 0 Then
                                taskItemList.Append(",")
                            End If
                            taskItemList.Append(.Item(i).TaskSeqid)
                        Next
                        _cblIncidentType.SelectedValue = taskItemList.ToString

                        If taskHeader.TaskTypeManagers IsNot Nothing Then
                            'Ensure that we have a distinct list of names
                            Dim managerName As New Collections.Specialized.OrderedDictionary    'SortedList 
                            With managerName
                                For Each item As TypeManagers In taskHeader.TaskTypeManagers
                                    If .Contains(item.UserName) = False Then
                                        .Add(item.UserName, IP.Bids.SharedFunctions.ToTitleCase(item.FullName.ToLower))
                                    End If
                                Next
                            End With
                            With _dlTaskTypeManager
                                .DataSource = managerName
                                .DataBind()
                            End With
                        End If
                    End With
                End If
                If taskHeader.BusinessManagers IsNot Nothing Then
                    With taskHeader.TaskTypes
                        'Ensure that we have a distinct list of names
                        Dim businessManagerName As New Collections.Specialized.OrderedDictionary 'SortedList

                        With businessManagerName
                            For Each item As BusinessManagers In taskHeader.BusinessManagers
                                If .Contains(item.UserName) = False Then
                                    .Add(item.UserName, IP.Bids.SharedFunctions.ToTitleCase(item.FullName.ToLower))
                                End If
                            Next
                        End With
                        With _dlBusinessManager
                            .DataSource = businessManagerName
                            .DataBind()
                        End With
                    End With
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("ReSelectControls", , ex, "An error has occured while trying to Reselect the selected items on this page.  Please refresh your screen and try again.")
        End Try
    End Sub

    ''' <summary>
    ''' Purpose is to save the current Task Header record
    ''' </summary>
    ''' <remarks>Called from the _btnSaveHeader_Click Event</remarks>
    Private Sub SaveTaskHeader(Optional ByVal redirectUrl As String = "")
        Try
            Me.Validate("TaskHeader") 'Verify that this page passes all validation rules before saving
            If Page.IsValid Then
                If _site.Visible = True And _site.FacilityValue.Trim.Length = 0 Then
                    IP.Bids.SharedFunctions.HandleError("SaveTaskHeader", , , "An error has occured while attempting to save the Task Header page.  The facility is missing.")
                    Exit Sub
                End If
                If taskHeader IsNot Nothing AndAlso currentTaskHeaderRecord IsNot Nothing Then
                    currentTaskHeaderRecord = StoreExistingTaskHeader()

                    'With currentTaskHeaderRecord
                    '    .ActivitySeqID = CInt(Me._rblActivity.SelectedValue)
                    '    .BusinessUnitArea = _site.BusinessUnitAreaValue
                    '    .Description = IP.Bids.SharedFunctions.DataClean(_txtDescription.Text) 'Server.HtmlEncode(_txtDescription.Text.Replace("'", "''"))
                    '    .Division = _site.DivisionValue
                    '    .EndDate = CDate(_dtStartEnd.EndDate)
                    '    .ExternalRef = Me._txtIncidentEventNumber.Text
                    '    .ExternalSourceSeqID = Me._ddlSourceSystem.SelectedValue
                    '    .LastUpdateDate = Now
                    '    .LastUpdateUserName = Master.CurrentUser.Username
                    '    .Line = _site.LineBreakValue
                    '    .PlantCode = _site.FacilityValue
                    '    .Region = _site.RegionValue
                    '    .SecurityLevel = CStr((IIf(Me._cbHighLevelSecurity.Checked, "Y", "N")))
                    '    .SiteName = _site.Facility
                    '    .StartDate = CDate(_dtStartEnd.StartDate)
                    '    .Title = IP.Bids.SharedFunctions.DataClean(_txtTitle.Text)
                    '    .TaskTypes = Me._cblIncidentType.SelectedValue.Replace("-1,", "")
                    'End With

                    Try
                        TaskHeaderBll.SaveTaskHeader(currentTaskHeaderRecord)
                        If redirectUrl.Length > 0 Then
                            IP.Bids.SharedFunctions.ResponseRedirect(redirectUrl)
                            'HttpContext.Current.ApplicationInstance.CompleteRequest()
                        Else
                            IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetTaskHeaderURL(CStr(currentTaskHeaderRecord.TaskHeaderSeqID), RefSite))
                            'HttpContext.Current.ApplicationInstance.CompleteRequest()
                        End If
                    Catch ex As System.Threading.ThreadAbortException
                        Server.ClearError()
                    Catch
                        Throw
                    End Try
                Else
                    'Create new header
                    Dim item As New IP.MEAS.BO.TaskHeaderRecord
                    item = StoreNewTaskHeader()
                    'With item
                    '    .ActivitySeqID = CInt(Me._rblActivity.SelectedValue)
                    '    .BusinessUnitArea = _site.BusinessUnitAreaValue
                    '    .CreateDate = Now
                    '    .CreatedBy = Master.CurrentUser.Username
                    '    .Description = _txtDescription.Text
                    '    .Division = _site.DivisionValue
                    '    .EndDate = CDate(_dtStartEnd.EndDate)
                    '    .ExternalRef = Me._txtIncidentEventNumber.Text
                    '    .ExternalSourceSeqID = Me._ddlSourceSystem.SelectedValue
                    '    .LastUpdateDate = Now
                    '    .LastUpdateUserName = Master.CurrentUser.Username
                    '    .Line = _site.LineBreakValue
                    '    .PlantCode = _site.FacilityValue
                    '    .Region = _site.RegionValue
                    '    .SecurityLevel = CStr((IIf(Me._cbHighLevelSecurity.Checked, "Y", "N")))
                    '    .SiteName = _site.Facility
                    '    .StartDate = CDate(_dtStartEnd.StartDate)
                    '    .Title = _txtTitle.Text
                    '    .TaskHeaderSeqID = -1
                    '    .TaskTypes = Me._cblIncidentType.SelectedValue
                    'End With

                    Try
                        Dim taskHeaderNumber As Integer = TaskHeaderBll.SaveTaskHeader(item)
                        If taskHeaderNumber <> -1 Then
                            If redirectUrl.Length > 0 Then
                                IP.Bids.SharedFunctions.ResponseRedirect(redirectUrl)
                            Else
                                IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetTaskHeaderURL(CStr(taskHeaderNumber), RefSite))
                            End If

                            'HttpContext.Current.ApplicationInstance.CompleteRequest()
                        End If
                    Catch ex As System.Threading.ThreadAbortException
                        Server.ClearError()
                    Catch ex As Exception
                        IP.Bids.SharedFunctions.HandleError("SaveTaskHeader", , ex, "An error has occured while attempting to create a new Task Header record")
                    End Try
                End If

            Else
                'Lets find out why this page is not valid
                Dim msg As String
                msg = IP.Bids.SharedFunctions.GetValidationError(Page.IsValid, Validators)
                IP.Bids.SharedFunctions.HandleError("SaveTaskHeader", msg, , "An error has occured while attempting to validate the Task Header page")
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("SaveTaskHeader", "", ex, "An error has occured while attempting to save the Task Header record")
        End Try
    End Sub
    Private Function StoreNewTaskHeader() As TaskHeaderRecord
        Dim item As New IP.MEAS.BO.TaskHeaderRecord
        With item
            .ActivitySeqID = CInt(Me._rblActivity.SelectedValue)
            .BusinessUnitArea = _site.BusinessUnitAreaValue
            .CreateDate = Now
            .CreatedBy = Master.CurrentUser.Username
            .Description = _txtDescription.Text
            .Division = _site.DivisionValue
            .EndDate = CDate(_dtStartEnd.EndDate)
            .ExternalRef = Me._txtIncidentEventNumber.Text
            .ExternalSourceSeqID = Me._ddlSourceSystem.SelectedValue
            .LastUpdateDate = Now
            .LastUpdateUserName = Master.CurrentUser.Username
            .Line = _site.LineBreakValue
            .PlantCode = _site.FacilityValue
            .Region = _site.RegionValue
            .SecurityLevel = CStr((IIf(Me._cbHighLevelSecurity.Checked, "Y", "N")))
            .SiteName = _site.Facility
            .StartDate = CDate(_dtStartEnd.StartDate)
            .Title = _txtTitle.Text
            .TaskHeaderSeqID = -1
            .TaskTypes = Me._cblIncidentType.SelectedValue
        End With
        Return item
    End Function
    Private Function StoreExistingTaskHeader() As TaskHeaderRecord
        With currentTaskHeaderRecord
            .ActivitySeqID = CInt(Me._rblActivity.SelectedValue)
            .BusinessUnitArea = _site.BusinessUnitAreaValue
            .Description = IP.Bids.SharedFunctions.DataClean(_txtDescription.Text) 'Server.HtmlEncode(_txtDescription.Text.Replace("'", "''"))
            .Division = _site.DivisionValue
            .EndDate = CDate(_dtStartEnd.EndDate)
            .ExternalRef = Me._txtIncidentEventNumber.Text
            .ExternalSourceSeqID = Me._ddlSourceSystem.SelectedValue
            .LastUpdateDate = Now
            .LastUpdateUserName = Master.CurrentUser.Username
            .Line = _site.LineBreakValue
            .PlantCode = _site.FacilityValue
            .Region = _site.RegionValue
            .SecurityLevel = CStr((IIf(Me._cbHighLevelSecurity.Checked, "Y", "N")))
            .SiteName = _site.Facility
            .StartDate = CDate(_dtStartEnd.StartDate)
            .Title = IP.Bids.SharedFunctions.DataClean(_txtTitle.Text)
            .TaskTypes = Me._cblIncidentType.SelectedValue.Replace("-1,", "")
        End With
        Return currentTaskHeaderRecord
    End Function

    ''' <summary>
    ''' Purpose to Save the Task Header and Redirect
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveTaskHeaderAndRedirect()
        Dim url As String
        'Save and then redirect to Add Task screen        
        If Me.currentTaskHeaderRecord.ExternalSourceSeqID = CStr(IP.Bids.SharedFunctions.SourceSystemID.OutageTemplate) Then 'Outage Template
            url = Page.ResolveClientUrl(IP.Bids.SiteURLs.GetTemplateTaskDetailsURL(taskHeaderNumber, RefSite, "-1")) '-1 indicates a new Task
        Else
            url = Page.ResolveClientUrl(IP.Bids.SiteURLs.GetTaskDetailsURL(taskHeaderNumber, RefSite, "-1")) '-1 indicates a new Task
        End If

        'Me.SaveTaskHeader(url)

        If url.Length > 0 Then
            IP.Bids.SharedFunctions.ResponseRedirect(url)
            'HttpContext.Current.ApplicationInstance.CompleteRequest()
        End If

    End Sub

    ''' <summary>
    ''' Purpose is to Set the page defaults based on the current users profile
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetPageDefaults()
        Try
            Dim defaults As RIUserDefaults.CurrentUserDefaults = Nothing
            'Set page defaults
            'Date
            Me._dtStartEnd.StartDate = Now.ToShortDateString
            Me._dtStartEnd.EndDate = Now.ToShortDateString

            If Master.CurrentUser IsNot Nothing AndAlso Master.CurrentUser.UserDefaults IsNot Nothing Then
                defaults = Master.CurrentUser.UserDefaults
            End If
            'Dim defaults As New RIUserDefaults.CurrentUserDefaults(SharedFunctions.GetCurrentUser.Username, RIUserDefaults.Applications.MTT, ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)

            If defaults IsNot Nothing AndAlso defaults.UserDefaults.Count > 0 Then
                If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.BusinessUnit) AndAlso defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Area) Then
                    _site.DefaultBusinessUnitArea = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.BusinessUnit) & " - " & defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Area)
                End If

                If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Business) Then
                    _site.DefaultDivision = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Business)
                End If

                If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Region) Then
                    _site.DefaultRegion = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Region)
                End If

                If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.PlantCode) Then
                    _site.DefaultFacility = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
                End If

                If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Line) AndAlso defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Machine) Then
                    _site.DefaultLineBreak = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Line) & " - " & defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Machine)
                End If

                If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.TaskType) Then
                    If Me._cblIncidentType.Items.FindByValue(defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.TaskType)) IsNot Nothing Then
                        Me._cblIncidentType.Items.FindByValue(defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.TaskType)).Selected = True
                        _cblIncidentType.SelectedValue = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.TaskType)
                    End If
                End If

                'If defaults.DoesDefaultValueExist("TaskArea") Then
                '    If Me._rblActivity.Items.FindByValue(defaults.GetDefaultValue("TaskArea")) IsNot Nothing Then
                '        Me._rblActivity.Items.FindByValue(defaults.GetDefaultValue(t)).Selected = True
                '    End If
                'End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("SetPageDefaults", , ex)
        End Try
    End Sub

    'Private Sub SavePageDefaults()
    '    Dim userName As String = IP.Bids.SharedFunctions.GetCurrentUser.Username
    '    Dim defaults As New RIUserDefaults.CurrentUserDefaults(userName, RIUserDefaults.Applications.MTT, ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
    '    With _site
    '        'If .RegionValue.Equals("ALL", StringComparison.InvariantCultureIgnoreCase) = False Then
    '        defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Region, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Region.ToString, .RegionValue))
    '        defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Business, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Business.ToString, .DivisionValue))
    '        defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.PlantCode, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.PlantCode.ToString, .FacilityValue))
    '        If .BusinessUnitAreaValue.Length > 0 Then
    '            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.BusinessUnit, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.BusinessUnit.ToString, .BusinessUnitAreaValue.Split(CType("-", Char())).ElementAtOrDefault(0).Trim))
    '            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Area, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Area.ToString, .BusinessUnitAreaValue.Split(CType("-", Char())).ElementAtOrDefault(1).Trim))
    '        End If
    '        If .LineBreakValue.Length > 0 Then
    '            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Line, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Line.ToString, .LineBreakValue.Split(CType("-", Char())).ElementAtOrDefault(0).Trim))
    '            defaults.InsertOrUpdateDefaults(New RIUserDefaults.UserDefaultValues(userName, RIUserDefaults.UserProfileTypes.Machine, RIUserDefaults.Applications.MTT.ToString, RIUserDefaults.UserProfileTypes.Machine.ToString, .LineBreakValue.Split(CType("-", Char())).ElementAtOrDefault(1).Trim))
    '        End If
    '    End With
    'End Sub
    'Public Function GetAccessToken() As AdmAccessToken
    '    Dim admToken As AdmAccessToken
    '    Dim headerValue As String

    '    'Get Client Id and Client Secret from https://datamarket.azure.com/developer/applications/
    '    Dim clientId As String = "92735252-6327-4752-9c75-f14b699ac0d2"
    '    Dim clientSecret As String = "14i9I2pFGm9gp2nczvTl0GG3Yv8JfezBb7gZCmvD7zA="

    '    Dim admAuth = New AdmAuthentication(clientId, clientSecret)
    '    'ihg93SUbA4RbrON9ESTlxTeM3UEhXjlWsIrQnTeyuSo
    '    '92735252-6327-4752-9c75-f14b699ac0d2

    '    admToken = admAuth.GetAccessToken()
    '    '// Create a header with the access_token property of the returned token
    '    headerValue = "Bearer" & " " & HttpUtility.UrlEncode(admToken.access_token)

    '    Return admToken
    'End Function

    'Public Sub TranslateText()
    '    Dim t As New Translator
    '    'lblResult.Text = "This is a test"
    '    'lblResult.Text = lblResult.Text & " to " & t.TranslateMethod("this is a test", "en-us", "ru-ru")
    'End Sub
#End Region

#Region "Events"

    Protected Sub Page_Error1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error

    End Sub
    ''' <summary>
    ''' Handles the Page Load event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandlePageLoad()
    End Sub

    ''' <summary>
    ''' Handles the Click event for the Save Header Button
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub BtnSaveHeaderClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles _lnkSaveButton.Click
        SaveTaskHeader()
    End Sub

    ''' <summary>
    ''' Handles the Click event of the _btnNewTaskHeader control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Protected Sub BtnNewTaskHeaderClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnNewTaskHeader.Click
        DisplayNewTaskHeader()
    End Sub

    ''' <summary>
    ''' Handles the Click event of the _btnTaskItems control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Protected Sub BtnTaskItemsClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles _lnkTaskItems.Click
        SaveTaskHeaderAndRedirect()
    End Sub

    ''' <summary>
    ''' Handles the OKClick event for the Delete Confirmation button
    ''' </summary>
    Protected Sub ConfirmDelete_OKClick() Handles _cbeDelete.OKClick
        If TaskHeaderBll.DeleteTaskHeader(taskHeaderNumber, IP.Bids.SharedFunctions.GetCurrentUser.Username) Then
            Me.BtnNewTaskHeaderClick(Me._btnNewTaskHeader, Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Handles the Unload event of the Page control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        taskHeader = Nothing
        currentTaskHeaderRecord = Nothing
    End Sub

    Private Sub TaskHeaderEntry_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        _rblActivity.RepeatLayout = RepeatLayout.Flow
        _rblActivity.RepeatDirection = RepeatDirection.Horizontal
        _rblActivity.RepeatColumns = 12
        For Each item As ListItem In _rblActivity.Items
            item.Attributes.Add("class", "col-xs-12 col-sm-6 col-md-4")
        Next
        _cblIncidentType.RepeatLayout = RepeatLayout.Flow
        _cblIncidentType.RepeatDirection = RepeatDirection.Horizontal
        _cblIncidentType.RepeatColumns = 12
        For Each item As ListItem In _cblIncidentType.Items
            item.Attributes.Add("class", "col-xs-12 col-sm-6 col-md-4")
        Next
    End Sub

    Private Sub TaskHeaderEntry_CommitTransaction(sender As Object, e As EventArgs) Handles Me.CommitTransaction

    End Sub
#End Region










End Class
