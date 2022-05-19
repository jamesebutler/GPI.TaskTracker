'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 06-15-2011
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class PopupsTaskList
    Inherits IP.Bids.BasePage

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Page.IsPostBack = False Then
        ConfigureTaskItemDisplay()
        'End If
    End Sub
#End Region

#Region "Private Methods"
    ''' <summary>
    ''' Purpose is to configure the properties for the task item list and to display the specified list.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ConfigureTaskItemDisplay()
        Try
            Dim taskHeaderNumber As String = IP.Bids.SharedFunctions.DataClean(Request.QueryString("HeaderNumber"), "")
            Dim parentTaskItemNumber As String = IP.Bids.SharedFunctions.DataClean(Request.QueryString("ParentTaskNumber"), "")
            Dim taskNumber As String = IP.Bids.SharedFunctions.DataClean(Request.QueryString("TaskNumber"), "")
            Dim ShowHeaderInfo As String = IP.Bids.SharedFunctions.DataClean(Request.QueryString("ShowHeaderInfo"), "")
            Dim allowEdit As String = IP.Bids.SharedFunctions.DataClean(Request.QueryString("AllowEdit"), "")
            Dim allowDelete As String = IP.Bids.SharedFunctions.DataClean(Request.QueryString("AllowDelete"), "")
            Dim inFrame As String = IP.Bids.SharedFunctions.DataClean(Request.QueryString("InFrame"), "")

            _taskEdit.Visible = False

            If taskHeaderNumber IsNot Nothing AndAlso taskHeaderNumber.Length > 0 Then
                _taskItems.TaskHeaderNumber = taskHeaderNumber
            End If

            If parentTaskItemNumber IsNot Nothing AndAlso parentTaskItemNumber.Length > 0 Then
                _taskItems.ParentTaskNumber = parentTaskItemNumber
            End If

            If taskNumber IsNot Nothing AndAlso taskNumber.Length > 0 Then
                _taskItems.TaskNumber = taskNumber
            End If

            If allowEdit IsNot Nothing AndAlso allowEdit.Length > 0 Then
                If allowEdit.ToLower = "false" Then 'false
                    _taskItems.AllowEdit = False
                Else
                    _taskItems.AllowEdit = True
                End If
            Else
                _taskItems.AllowEdit = True
            End If

            If allowDelete IsNot Nothing AndAlso allowDelete.Length > 0 Then
                If allowDelete.ToLower = "true" Then
                    _taskItems.AllowDelete = True
                    _taskItems.UpdateFlag = UserControlsTasksTaskListing.UpdateFlagValues.Future
                Else
                    _taskItems.AllowDelete = False
                End If
            Else
                _taskItems.AllowDelete = False
            End If

            If RefSite IsNot Nothing AndAlso RefSite.Length > 0 Then
                If RefSite.ToUpper = "IRIS" Then
                    _taskItems.DisplaySimpleList = True
                ElseIf RefSite.ToUpper = "TANKS" Then
                    _taskItems.DisplaySimpleList = True
                    _taskEdit.LoadTasks(False)
                    _taskEdit.Visible = True
                    _taskItems.Visible = False

                ElseIf RefSite.Length > 0 And RefSite.ToUpper <> "MTT" Then
                    _taskItems.DisplaySimpleList = False
                    _taskEdit.LoadTasks(False)
                    _taskEdit.Visible = True
                    _taskItems.Visible = False
                    Exit Sub
                Else
                    _taskItems.DisplaySimpleList = False
                End If
            End If

                'If RefSite.ToUpper = "RI" Then
                '    _taskItems.DisplaySimpleList = False
                'End If

                If ShowHeaderInfo IsNot Nothing Then
                    If ShowHeaderInfo.ToLower = "true" And _taskItems.DisplaySimpleList = False Then
                        _taskHeader.TaskHeaderNumber = taskHeaderNumber
                        _taskHeader.LoadHeaderInfo()
                    End If
                End If

                If inFrame IsNot Nothing Then
                    If inFrame = "true" Then
                        _taskItems.InFrame = True
                    Else
                        _taskItems.InFrame = False
                    End If
                Else
                    _taskItems.InFrame = False
                End If

            Dim taskCount As Integer = _taskItems.LoadTaskItems

                If taskCount = 0 Then 'And _taskItems.InFrame = False Then
                _taskItems.AllowTasksToBeFiltered = False
                    If _taskItems.DisplaySimpleList = True Then
                        'IP.Bids.SharedFunctions.InsertAuditRecord("ConfigureTaskItemDisplay", "User:[" & IP.Bids.CurrentUserProfile.GetCurrentUser & "] Response.Redirect to [" & IP.Bids.SiteURLs.GetTaskDetailsURL(taskHeaderNumber, RefSite, "-1") & "]")
                        'Response.Redirect(IP.Bids.SiteURLs.GetTaskDetailsURL(taskHeaderNumber, RefSite, "-1"), False)
                        'HttpContext.Current.ApplicationInstance.CompleteRequest()                    
                    If _taskItems.MinimumItems > 0 And _taskItems.InFrame = True And (RefSite.ToUpper <> "IRIS" And RefSite.ToUpper <> "TANKS") Then
                        Me.Visible = False
                    Else
                        Me.Visible = True
                    End If
                    Else
                        Me.Visible = False
                    End If
                    'Display new task screen           
                ElseIf taskCount = -1 And _taskItems.InFrame = False Then 'Invalid header specified
                    IP.Bids.SharedFunctions.InsertAuditRecord("ConfigureTaskItemDisplay", "User:[" & IP.Bids.CurrentUserProfile.GetCurrentUser & "] Response.Redirect to [" & IP.Bids.SiteURLs.GetTaskHeaderURL("-1", RefSite) & "]")
                    IP.Bids.SharedFunctions.ResponseRedirect(IP.Bids.SiteURLs.GetTaskHeaderURL("-1", RefSite))
                    ' HttpContext.Current.ApplicationInstance.CompleteRequest()
            End If



        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("ConfigureTaskItemDisplay", , ex, "An error has occured while attempting to configure the Task Item List")
        End Try
    End Sub

    'Private Sub PopupsTaskList_Init(sender As Object, e As EventArgs) Handles Me.Init
    '    Me.UseBootStrap = False
    'End Sub


#End Region

End Class
