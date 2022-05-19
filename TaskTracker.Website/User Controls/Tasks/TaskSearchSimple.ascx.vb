Imports Devart.Data.Oracle

Partial Class User_Controls_Tasks_TaskSearchSimple
    Inherits System.Web.UI.UserControl


    Private _searchMode As TaskSearchMode = TaskSearchMode.View

    Dim newIncident As New TaskTrackerListsBll
    Dim status As System.Collections.Generic.List(Of TaskStatus) = newIncident.GetTaskStatus

    Dim openStatusText As String = String.Empty
    Dim openStatusValue As String = String.Empty
    Dim closedStatusText As String = String.Empty
    Dim closedStatusValue As String = String.Empty

    Dim in_CreatedBy As String = String.Empty
    Dim in_Responsible As String = String.Empty
    Dim in_Title As String = String.Empty
    Dim in_Description As String = String.Empty
    Dim in_TaskStatus As String = String.Empty
    Dim in_DueDateFrom As String = String.Empty
    Dim in_DueDateTo As String = String.Empty
    Dim in_ClosedDateFrom As String = String.Empty
    Dim in_ClosedDateTo As String = String.Empty


    Private _currentUser As IP.Bids.UserInfo

    Public Enum TaskSearchMode
        View
        Reporting
    End Enum

    Public Property SearchMode() As TaskSearchMode
        Get
            Return _searchMode
        End Get
        Set(ByVal value As TaskSearchMode)
            _searchMode = value
        End Set
    End Property

    Public Sub PopulateData()
        Try


            Dim imgPath As String = Page.ResolveUrl("~/Images/")
            With Me._rblTaskStatus
                .Items.Clear()

                For Each item As TaskStatus In status
                    'If item.StatusSeqid = -1 Then Continue For
                    If Me.SearchMode = TaskSearchMode.View Then
                        Dim img As String = "<img src='{0}' align=center width=15 height=15 title='{1}' alt='{1}'>{1}"
                        Dim statusText As String = String.Format(img, imgPath & item.ImageIcon, IP.Bids.SharedFunctions.LocalizeValue(item.StatusName, True))
                        Select Case item.StatusName
                            Case "Open"
                                'Case "Open"
                                If openStatusText.Length > 0 Then
                                    openStatusText = openStatusText & "&nbsp;" & statusText
                                Else
                                    openStatusText = statusText
                                End If

                                If openStatusValue.Length > 0 Then
                                    openStatusValue = openStatusValue & "|" & item.StatusName
                                Else
                                    openStatusValue = item.StatusName
                                End If
                                'If openStatusValue.Length > 0 Then
                                '    openStatusValue = openStatusValue & "," & item.StatusSeqid
                                'Else
                                '    openStatusValue = item.StatusSeqid
                                'End If
                                .Items.Add(New ListItem(openStatusText, openStatusValue, True))

                            Case "Overdue"
                                'If openStatusText.Length > 0 Then
                                '    openStatusText = openStatusText & "&nbsp;" & statusText
                                'Else

                                If openStatusText.Length > 0 Then
                                    openStatusText = statusText
                                    openStatusValue = item.StatusName

                                    .Items.Add(New ListItem(openStatusText, openStatusValue, True))
                                End If
                        'JEB Case "Complete", "Cancelled", "No Work Needed"

                            Case "Complete"
                                If closedStatusText.Length > 0 Then
                                    closedStatusText = closedStatusText & "&nbsp;" & statusText
                                Else
                                    closedStatusText = statusText
                                End If

                                If closedStatusValue.Length > 0 Then
                                    closedStatusValue = closedStatusValue & "|" & item.StatusName
                                Else
                                    closedStatusValue = item.StatusName
                                End If
                                'If closedStatusValue.Length > 0 Then
                                '    closedStatusValue = closedStatusValue & "|" & item.StatusSeqid
                                'Else
                                '    closedStatusValue = item.StatusSeqid
                                'End If
                                .Items.Add(New ListItem(closedStatusText, closedStatusValue, True))
                        End Select


                    Else
                        ' .Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.StatusName, True), item.StatusName, True))
                    End If
                Next
                .Items.Insert(0, New ListItem(IP.Bids.SharedFunctions.LocalizeValue("All", True), "All"))
                .ClearSelection()
                .SelectedIndex = 1
                .DataBind()



                'Me._dtDueDate.EndDate = Now.AddMonths(1).ToShortDateString
                'Me._dtDueDate.StartDate = New Date(Now.Year(), 1, 1).ToShortDateString


            End With

        Catch ex As Exception

        End Try


    End Sub

    Private Sub User_Controls_Tasks_TaskSearchSimple_Load(sender As Object, e As EventArgs) Handles Me.Load


        If Not Page.IsPostBack Then

            Dim defaults As RIUserDefaults.CurrentUserDefaults = Nothing
            If IP.Bids.SharedFunctions.GetCurrentUser IsNot Nothing AndAlso IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults IsNot Nothing Then
                defaults = IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults

                If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.PlantCode) Then
                    Me._ResponsiblePerson.PlantCode = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
                    Me._CreatedBy.PlantCode = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
                End If
                'JEB If Me._dtHeaderDate.StartDate.Length = 0 Then
                'JEBMe._dtHeaderDate.DateRange = UserControlsJQDateRange.JQDateRange.CurrentYear
                'JEBEnd If
            End If

            PopulateData()

            _currentUser = IP.Bids.SharedFunctions.GetCurrentUser

            Me._ResponsiblePerson.SelectedValue = _currentUser.Username.ToString

        End If


    End Sub

    Protected Sub _btnDisplaySearchResults_Click(sender As Object, e As EventArgs) Handles _btnDisplaySearchResults.Click


        resetSearchValues()


        _currentUser = IP.Bids.SharedFunctions.GetCurrentUser
        'BUILD SEARCH CRITERIA
        Dim _username = Me._ResponsiblePerson.SelectedValue

        If Len(_username) = 0 Then
            _username = _currentUser.Username
        End If



        If _rblTaskStatus.SelectedValue = "Open" Then
            in_TaskStatus = "1"

        ElseIf _rblTaskStatus.SelectedValue = "Complete" Then
            in_TaskStatus = "2"

        ElseIf _rblTaskStatus.SelectedValue = "Overdue" Then
            in_TaskStatus = "3"

        End If

        If Len(Me._CreatedBy.SelectedValue) > 0 Then
            in_CreatedBy = Me._CreatedBy.SelectedValue
        End If



        If Len(_txtTitle.Text) > 0 Then

            in_Title = _txtTitle.Text

        End If

        If Len(_txtDescription.Text) > 0 Then

            in_Description = _txtDescription.Text

        End If

        If Me._dtDueDate.StartDate.Length > 0 Then

            in_DueDateFrom = Me._dtDueDate.StartDate
        End If


        If Me._dtDueDate.EndDate.Length > 0 Then

            in_DueDateTo = Me._dtDueDate.EndDate
        End If



        If Me._dtClosedDate.StartDate.Length > 0 Then

            in_ClosedDateFrom = Me._dtClosedDate.StartDate
        End If


        If Me._dtClosedDate.EndDate.Length > 0 Then

            in_ClosedDateTo = Me._dtClosedDate.EndDate
        End If



        Dim taskItemsDS As DataSet = GetTaskItems(_username,
                                                    in_CreatedBy,
                                                    in_Title,
                                                    in_Description,
                                                    in_TaskStatus,
                                                    in_DueDateFrom,
                                                    in_DueDateTo,
                                                    in_ClosedDateFrom,
                                                    in_ClosedDateTo)

        RadGridTaskListing.DataSource = taskItemsDS
        RadGridTaskListing.DataBind()

        'CALL DATABASE SEARCH


        'DISPLAY RESULTS

        DownloadCSV.Visible = True



    End Sub

    Public Sub resetSearchValues()

        in_CreatedBy = String.Empty
        in_Responsible = String.Empty
        in_Title = String.Empty
        in_Description = String.Empty
        in_TaskStatus = String.Empty
        in_DueDateFrom = String.Empty
        in_DueDateTo = String.Empty
        in_ClosedDateFrom = String.Empty
        in_ClosedDateTo = String.Empty



    End Sub



    Public Function GetTaskItems(ByVal username As String,
                                    ByVal in_CreatedBy As String,
                                    ByVal in_Title As String,
                                    ByVal in_Description As String,
                                    ByVal in_TaskStatus As String,
                                    ByVal in_DueDateFrom As String,
                                    ByVal in_DueDateTo As String,
                                    ByVal in_ClosedDateFrom As String,
                                    ByVal in_ClosedDateTo As String) As DataSet

        Dim paramCollection As New OracleParameterCollection
        Dim param As New OracleParameter
        Dim ds As System.Data.DataSet = Nothing
        Dim dsError As System.Data.DataSet = Nothing


        Try

            param = New OracleParameter
            param.ParameterName = "in_CreatedBy"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_CreatedBy
            paramCollection.Add(param)


            param = New OracleParameter
            param.ParameterName = "in_Title"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_Title
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "in_Description"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_Description
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "in_TaskStatus"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_TaskStatus
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "in_DueDateFrom"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_DueDateFrom
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "in_DueDateTo"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_DueDateTo
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "in_ClosedDateFrom"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_ClosedDateFrom
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "in_ClosedDateTo"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_ClosedDateTo
            paramCollection.Add(param)


            param = New OracleParameter
            param.ParameterName = "in_Responsible"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = username
            paramCollection.Add(param)

            param = New OracleParameter
            param.ParameterName = "rsTaskList"
            param.OracleDbType = OracleDbType.Cursor
            param.Direction = Data.ParameterDirection.Output
            paramCollection.Add(param)

            ds = HelperDal.GetDSFromPackage(paramCollection, "mttviewGPI.MTTVIEWSimple")

            Session("taskitemlisting") = ds
            Return ds

        Catch ex As Exception
            Return dsError
        End Try

    End Function

    Protected Sub RadGridTaskListing_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGridTaskListing.NeedDataSource


        If Not Session("taskitemlisting") Is Nothing Then
            RadGridTaskListing.DataSource = Session("taskitemlisting")
        End If
    End Sub


    Protected Sub DownloadCSV_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ConfigureExport()
        RadGridTaskListing.ExportSettings.FileName = "TaskList_" + DateTime.Now.ToShortDateString()
        RadGridTaskListing.MasterTableView.ExportToCSV()
    End Sub

    Protected Sub DownloadPDF_Click(sender As Object, e As EventArgs)
        RadGridTaskListing.MasterTableView.ExportToPdf()
    End Sub

    Public Sub ConfigureExport()

        RadGridTaskListing.ExportSettings.ExportOnlyData = True
        RadGridTaskListing.ExportSettings.IgnorePaging = True
        RadGridTaskListing.ExportSettings.OpenInNewWindow = True


    End Sub

End Class
