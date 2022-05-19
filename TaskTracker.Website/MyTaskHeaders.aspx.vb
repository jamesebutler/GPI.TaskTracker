
Partial Class MyTaskHeaders
    Inherits IP.Bids.BasePage
#Region "Fields"
    Private instanceOfTaskHeader As TaskHeaderBll 'Holds an instance of the  Task Header Business Logic Layer
    Private currentTaskHeaderRecord As System.Collections.ObjectModel.Collection(Of TaskHeaderRecord)
    Private filters As New HeaderFilters
#End Region

    Private Structure HeaderFilters
        Dim UserName As String
        Dim PlantCode As String
        Dim BusinessUnitArea As String
        Dim HeaderCreateDateStart As Nullable(Of Date)
        Dim HeaderCreateDateEnd As Nullable(Of Date)
    End Structure
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            InitializeCurrentUserList()
            InitalizeFilterOptions()
            InitializeFilterValues()
            SetPageDefaults()
            PopulateMyTaskHeaders()
        End If
        Master.PageName = Master.GetLocalizedValue("Task Header Listing", False)
        _lblHeaderTitle.Text = Master.PageName
    End Sub

    Private Sub InitalizeFilterOptions()
        Me._cpeSearchSelections.CollapsedText = "+ <span class='glyphicon glyphicon-filter' aria-hidden='True'></span>&nbsp;" & IP.Bids.SharedFunctions.LocalizeValue("Show Search Criteria", True)
        Me._cpeSearchSelections.ExpandedText = "- <span Class='glyphicon glyphicon-filter' aria-hidden='True'></span>&nbsp;" & IP.Bids.SharedFunctions.LocalizeValue("Hide Search Criteria", True)
    End Sub
    Private Sub InitializeCurrentUserList()
        _currentUser.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
        _currentUser.PopulateEmployeeList()
    End Sub

    Private Sub SetPageDefaults()
        Dim defaults As RIUserDefaults.CurrentUserDefaults = Nothing
        If IP.Bids.SharedFunctions.GetCurrentUser IsNot Nothing AndAlso IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults IsNot Nothing Then
            defaults = IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults
        End If
        If defaults IsNot Nothing Then
            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.PlantCode) Then
                _cddlFacility.SelectedValue = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
                filters.PlantCode = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
            End If
            If defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.BusinessUnit) AndAlso defaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.Area) Then
                _cddlBusArea.SelectedValue = defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.BusinessUnit) & " - " & defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Area)
                filters.BusinessUnitArea = _ddlBusArea.SelectedValue ' defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.BusinessUnit) & " - " & defaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Area)
            End If
        End If
    End Sub
    Private Sub InitializeFilterValues()
        With filters
            .BusinessUnitArea = String.Empty
            .PlantCode = String.Empty
            .UserName = String.Empty
            .HeaderCreateDateStart = Nothing
            .HeaderCreateDateEnd = Nothing
        End With
    End Sub
    Private Sub SetFilterValues()
        filters.UserName = GetUserName()
        If _ddlFacility.SelectedValue.Length > 0 AndAlso _ddlFacility.SelectedValue.ToLower <> "all" Then
            filters.PlantCode = _ddlFacility.SelectedValue
        End If
        If _ddlBusArea.SelectedValue.Length > 0 AndAlso _ddlBusArea.SelectedValue.ToLower <> "all" Then
            filters.BusinessUnitArea = _ddlBusArea.SelectedValue
        End If
        If _dtHeaderDate.StartDate.Length > 0 Then
            filters.HeaderCreateDateStart = CDate(_dtHeaderDate.StartDate)
            filters.HeaderCreateDateEnd = CDate(_dtHeaderDate.EndDate)
        End If
    End Sub
    Private Function GetUserName() As String
        Dim userName As String = String.Empty
        If Page.IsPostBack Then
            userName = _currentUser.SelectedValue
        Else
            userName = IP.Bids.SharedFunctions.GetCurrentUser.Username
        End If

        If userName Is Nothing OrElse userName.Length = 0 Then
            Throw New ApplicationException("The application is unable to determine the current user name.")
        End If
        Return userName
    End Function
    Private Sub PopulateMyTaskHeaders()
        Dim userName As String = String.Empty
        Try
            SetFilterValues()

            instanceOfTaskHeader = New TaskHeaderBll(filters.UserName)
            If instanceOfTaskHeader IsNot Nothing AndAlso
                instanceOfTaskHeader.TaskHeaderRecord IsNot Nothing AndAlso
                instanceOfTaskHeader.TaskHeaderRecord.Count > 0 Then

                Dim filteredTaskHeaders As IEnumerable(Of TaskHeaderRecord) = instanceOfTaskHeader.TaskHeaderRecord '.Where(Function(x) x.PlantCode = filters.PlantCode Or filters.PlantCode = "all")
                If filters.PlantCode IsNot Nothing Then
                    filteredTaskHeaders = instanceOfTaskHeader.TaskHeaderRecord.Where(Function(x) x.PlantCode = filters.PlantCode Or filters.PlantCode = "all")
                End If
                If filters.BusinessUnitArea IsNot Nothing AndAlso filters.BusinessUnitArea.Length > 0 Then
                    filteredTaskHeaders = filteredTaskHeaders.Where(Function(x) x.BusinessUnitArea = filters.BusinessUnitArea)
                End If
                If filters.HeaderCreateDateStart IsNot Nothing Then
                    filteredTaskHeaders = filteredTaskHeaders.Where(Function(x) x.CreateDate >= CDate(filters.HeaderCreateDateStart) And x.CreateDate <= CDate(filters.HeaderCreateDateEnd))
                End If
                With _gvTaskHeaders
                    .DataSource = filteredTaskHeaders.ToList
                    .DataBind()
                    .UseAccessibleHeader = True
                    If .Rows.Count > 0 Then
                        .HeaderRow.TableSection = TableRowSection.TableHeader
                        Web.UI.ScriptManager.RegisterStartupScript(_udpTaskList, _udpTaskList.GetType(), "SortGrid_" & .ClientID, String.Format(System.Globalization.CultureInfo.CurrentCulture, "$(function(){{$('#{0}').tablesorter(); }});BindEvents();", .ClientID), True)
                    End If
                End With
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateMyTaskHeaders", "Error while attempting to display the User Task List for [" & userName & "]", ex)
        End Try
    End Sub

    Private Sub Refresh_Click(sender As Object, e As EventArgs) Handles _btnApplyFilter.Click
        PopulateMyTaskHeaders()
    End Sub

    Private Sub _gvTaskHeaders_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles _gvTaskHeaders.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).Attributes("data-title") = _gvTaskHeaders.Columns(i).HeaderText
            Next
        End If
    End Sub
End Class
