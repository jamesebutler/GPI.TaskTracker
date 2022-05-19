'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 09-24-2010
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class ProcessEmployees
    Inherits IP.Bids.BasePage
    'Private mTaskHeaderNumber As String = String.Empty
    'Private mTaskItemNumber As String = String.Empty
    'Private mSubTaskNumber As String = String.Empty
    Private IsTaskTemplate As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        Dim plantCode As String = String.Empty
        Dim inactive_control As String = ""

        Try
            If Not Page.IsPostBack Then
                If IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults IsNot Nothing Then
                    If IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults.DoesDefaultValueExist(RIUserDefaults.UserProfileTypes.PlantCode) Then
                        plantCode = IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode)
                    End If
                End If
                Dim currentUserName = Request("UserId")
                If currentUserName Is Nothing Then
                    Throw New ArgumentNullException("UserId", "UserId is required for this page")
                End If
                Dim user As IP.Bids.UserInfo = New IP.Bids.UserInfo("", currentUserName)

                If plantCode.Length > 0 Then
                    Me._employeeList.PlantCode = plantCode 'IP.Bids.SharedFunctions.GetCurrentUser.UserDefaults.Item("Facility")
                    If Request.QueryString("TaskCount") = 0 Then
                        Me._employeeList.DefaultUserName = ""
                        Me._employeeList.Enabled = False
                        Me._lblInactiveStatus.Text = IP.Bids.SharedFunctions.LocalizeValue("    ", True)
                        'Me._lblInactiveStatus.Visible = True
                    Else
                        Me._lblInactiveStatus.Text = IP.Bids.SharedFunctions.LocalizeValue("Select a Person from the dropdown to replace the Inactivated Employee for Active MTT Tasks Then Click Save Inactive User Button to Update OR Click Cancel Button to Cancel Update", True)
                        Me._employeeList.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                    End If
                    'If Me.IsTaskTemplate Then
                    '    Me._employeeList.UserMode = User_Controls.AdvancedEmployeeListDropdown.UserModes.RolesOnly 'UserModes.RolesOnly
                    '    Me._employeeList.PlantCode = "9998"
                    '    Me._employeeList.AllowFacilityChange = False
                    '    Me._employeeList.DefaultUserName = String.Empty
                    'End If
                    Me._employeeList.PopulateEmployeeList()
                End If
                If user IsNot Nothing Then
                    Me._lblFirstname.Text = user.FirstName
                    Me._lblLastname.Text = user.LastName
                Else
                    Me._lblFirstname.Text = Request.QueryString("firstname").Replace("'", "''")
                    Me._lblLastname.Text = Request.QueryString("lastname").Replace("'", "''")
                End If

                _btnInactivateUser.OnClientClick = _confirmationSaveInactiveUser.TriggerPopupJS & ";return false;"
                If Request.QueryString("cb") IsNot Nothing Then
                    inactive_control = Request.QueryString("cb")
                End If
                _btnCancel.OnClientClick = "try{window.parent.window.opener.document.getElementById('" & inactive_control & "').checked=false;window.close();}catch(e){alert(e)};return false"
            End If
        Catch
            Throw
        End Try
    End Sub

    
    Protected Sub _confirmationSaveInactiveUser_OKClick() Handles _confirmationSaveInactiveUser.OKClick
        Dim updateemployee As New DataMaintenanceDALTableAdapters.EmployeeTableAdapter
        Dim out_status As Decimal = 0

        'Dim PlantCode As String = Request.QueryString("PlantCode")

        Dim responsible As String = Me._employeeList.SelectedValue

        updateemployee.CLEANUPINACTIVEEMPLOYEE(Request.QueryString("UserID"), Request.QueryString("PlantCode"), responsible, IP.Bids.SharedFunctions.GetCurrentUser.Username, out_status)

        If out_status <> 0 Then
            Me._lblInactiveStatus.Visible = True
            Me._lblInactiveStatus.Text = IP.Bids.SharedFunctions.LocalizeValue("An update error has occurred. If problems continue please contact the Task Tracker System Administrator.", True)
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "CloseMe", "try{window.close();}catch(e){}", True)
        End If
    End Sub

    'Protected Sub _btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnCancel.Click
    '    Dim inactive_control As String = ""
    '    If Request.QueryString("cb") IsNot Nothing Then
    '        inactive_control = Request.QueryString("cb")
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "CloseMe", "try{window.parent.document.getElementById('" & inactive_control & "').checked=false;window.close();}catch(e){alert(e)}", True)
    '    Else
    '        ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "CloseMe", "try{window.close();}catch(e){}", True)
    '    End If
    'End Sub
End Class
