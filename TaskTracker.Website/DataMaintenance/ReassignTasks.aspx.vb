Imports System.Globalization
Imports Devart.Data.Oracle
Imports IP.TaskTrackerDAL


Partial Class ReassignTasks
    Inherits IP.Bids.BasePage


#Region "Private Events"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Master.PageName = Master.GetLocalizedValue("Reassign Tasks", False)
        _lblHeaderTitle.Text = Master.PageName
        Master.SetActiveNav(Enums.NavPages.DataMaintenance.ToString)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandlePageLoad()
    End Sub

    private sub DisplayTasks()
         Dim plantcode As String
        Dim busunit As String
        Dim area As String
        Dim line As String
        Dim type As String
        Dim activity As String

        plantcode = _ddlFacility.SelectedValue 'IP.Bids.SharedFunctions.GetCurrentUser.PlantCode
        busunit = _ddlBusUnit.SelectedValue
        area = _ddlArea.SelectedValue
        line = _ddlLine.SelectedValue
        type = _cblIncidentType.SelectedValue
        activity = _cblActivity.SelectedValue
        _lblNoRecords.Text = ""

        PopulateReassignList(_ReassignedEmployeeList.SelectedValue, plantcode, busunit, area, line, type, activity)
    End sub
    Protected Sub Display_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnDisplay.Click
       DisplayTasks        
    End Sub

#End Region

#Region "Methods"
    Private Sub HandlePageLoad()
        Dim userName As String
        Dim plantcode As String
        Dim RoleSeqId As String = ""
        Dim Division As String = ""
        Dim BusType As String = ""
        ' Dim ReassignUser As String

        Try
            If Not Page.IsPostBack Then
                userName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                plantcode = IP.Bids.SharedFunctions.GetCurrentUser.PlantCode

                PopulateTypes()
                PopulateSite()
                _ddlFacility.SelectedValue = plantcode
                _ReassignedEmployeeList.PlantCode = plantcode
                _NewReassignedEmployeeList.PlantCode = plantcode
            End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("HandlePageLoad", , ex)
        End Try

    End Sub

    Private Sub PopulateReassignList(ByVal user As String, ByVal plantcode As String, ByVal busunit As String, ByVal area As String, ByVal line As String, ByVal type As String, ByVal activity As String)
        Dim reassignList As System.Collections.Generic.List(Of ReassignTask) = DataMaintenanceBLL.GetReassignTaskList(user, plantcode, busunit, area, line, type, activity)

        Try

            If reassignList Is Nothing Then
                Me._gvReassignTaskList.Visible = "False"
                Me._lblNoRecords.Visible = "True"
            Else

                _gvReassignTaskList.DataSource = reassignList
                _gvReassignTaskList.DataBind()
                If _gvReassignTaskList.Rows.Count = 0 Then
                    Me._lblNoRecords.Visible = "True"
                    _lblNoRecords.Text = IP.Bids.SharedFunctions.LocalizeValue("NoRecordsFound", True)
                Else
                    Me._lblNoRecords.Visible = "False"
                End If
                _gvReassignTaskList.UseAccessibleHeader = True
                _gvReassignTaskList.HeaderRow.TableSection = TableRowSection.TableHeader
                ScriptManager.RegisterStartupScript(Me._udpRoleList, _udpRoleList.GetType(), "SortGrid", String.Format(CultureInfo.CurrentCulture, "$(function(){{$('#{0}').tablesorter(); }});", _gvReassignTaskList.ClientID), True)

            End If


            'Me._ddlsitelist.Items.Insert(0, New ListItem("----Select Facility----", 0))
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateReassignList", , ex)
        End Try

    End Sub

    Public Sub PopulateTypes()
        Try

            Dim newIncident As New TaskTrackerListsBll
            If newIncident IsNot Nothing Then
                Dim types As System.Collections.Generic.List(Of TaskType) = newIncident.GetTaskTypes
                Dim activities As System.Collections.Generic.List(Of TaskActivity) = newIncident.GetActivities

                For Each item As TaskType In types
                    'MJP - We need to populate the value field using the English name so that we can pass the text value into MTTView
                    If _cblIncidentType.Items.FindByValue(CStr(item.TaskName)) Is Nothing Then
                        _cblIncidentType.Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.TaskName, True), item.TaskName)) 'CStr(item.TaskSeqid)))
                    End If
                Next


                For Each item As TaskActivity In activities
                    If _cblActivity.Items.FindByValue(CStr(item.ActivitySeqid)) Is Nothing Then
                        _cblActivity.Items.Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue(item.ActivityName, True), item.ActivitySeqid)) ' CStr(item.ActivitySeqid)))
                    End If
                Next
            End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateTypes", , ex)
        End Try
    End Sub

    Private Sub PopulateSite()
        Dim siteItem As New TaskTrackerSiteBll
        Dim facilityList As System.Collections.Generic.List(Of Facility) = siteItem.GetFacility("", "")

        Try
            For Each item As Facility In facilityList
                With Me._ddlFacility
                    If item.SiteName.Length > 0 Then
                        .Items.Add(New ListItem(item.SiteName, item.PlantCode))
                    End If
                End With
                
            Next
            Me._ddlFacility.Items.Insert(0, New ListItem("----Select Facility----", 0))

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateSite", , ex)
        End Try
    End Sub

    Private sub ReAssignTask()
        Dim updatetask As New DataMaintTableAdapters.GETREASSIGNTASKLISTTableAdapter
        Dim outStatus As Decimal = 0
        Dim introw As Integer
        Dim userName As String = IP.Bids.SharedFunctions.GetCurrentUser.Username
        Dim tasklist As New Hashtable

        Try

            For Each r As GridViewRow In _gvReassignTaskList.Rows
                introw = r.RowIndex
                Dim reassignflag As CheckBox = TryCast(Me._gvReassignTaskList.Rows(introw).FindControl("_cbReassign"), CheckBox)
                If reassignflag.Checked = "true" Then
                    Dim taskitem As String = Me._gvReassignTaskList.DataKeys(introw).Values(0).ToString()
                    Dim taskheader As String = Me._gvReassignTaskList.DataKeys(introw).Values(1).ToString()
                    tasklist.Add(taskitem, taskheader)
                    updatetask.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                    If IsNumeric(_NewReassignedEmployeeList.SelectedValue) Then   'Role
                        updatetask.Update(_ReassignedEmployeeList.SelectedValue, _NewReassignedEmployeeList.SelectedValue, _NewReassignedEmployeeList.PlantCode, "", "N", taskitem, userName, CType(outStatus, Global.System.Nullable(Of Decimal)))
                    Else
                        updatetask.Update(_ReassignedEmployeeList.SelectedValue, "", "", _NewReassignedEmployeeList.SelectedValue, "N", taskitem, userName, CType(outStatus, Global.System.Nullable(Of Decimal)))
                    End If
                End If
            Next
            Me._lblNoRecords.Visible = "True"
            Me._lblNoRecords.Text = IP.Bids.SharedFunctions.LocalizeValue("Reassignment Complete", True)

            If tasklist.Count > 0 Then
                Master.DisplayMessageToUser(IP.Bids.SharedFunctions.LocalizeValue("Reassignment Complete", True), IP.Bids.SharedFunctions.LocalizeValue("Reassignment Complete", True))
                EmailDataBll.GetAndSendReassignedEmail(tasklist)
            End If
            
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("ReAssignTask", , ex)
        End Try
    End sub
    Private Sub Submit_Click(sender As Object, e As EventArgs) Handles _btnSubmit.Click
        ReAssignTask()
        DisplayTasks()
    End Sub


#End Region


End Class
