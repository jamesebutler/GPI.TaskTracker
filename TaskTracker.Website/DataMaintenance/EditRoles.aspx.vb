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
Partial Class EditRoles
    Inherits IP.Bids.BasePage


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Master.PageName = Master.GetLocalizedValue("Edit Role Assignment", False)
        Master.SetActiveNav(Enums.NavPages.DataMaintenance.ToString)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim plantCode As String = String.Empty

        Try
            If Not Page.IsPostBack Then
                Me._lblRoleDescription.Text = Request.QueryString("RoleDescription")
                Me._employeeList.DefaultUserName = Request.QueryString("Userid")
                Me._employeeList.PlantCode = Request.QueryString("Plantcode")
                If Request.QueryString("RoleDescription") = "Business Unit" Then
                    Me._cddlBusinessUnit.SelectedValue = Request.QueryString("BusUnit")
                    Me._cddlArea.SelectedValue = Request.QueryString("Area")
                    Me._cddlLine.SelectedValue = Request.QueryString("Line")
                Else
                    Me._cddlBusinessUnit.SelectedValue = "All"
                    Me._cddlArea.SelectedValue = "All"
                    Me._cddlLine.SelectedValue = "All"
                    'Me._tcBusUnit.Enabled = False
                    'Me._tcArea.Enabled = False
                    'Me._tcLine.Enabled = False
                    _ddlBusUnit.Enabled = False
                    _ddlArea.Enabled = False
                    _ddlLine.Enabled = False
                End If
            End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("Page_Load", , ex)
        End Try

    End Sub

    Protected Sub _btnUpdateRole_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnUpdateRole.Click
        Dim updaterole As New DataMaintenanceDALTableAdapters.SiteUserRoleListTableAdapter
        Dim out_status As Decimal = 0

        'Dim PlantCode As String = Request.QueryString("PlantCode")

        Dim responsible As String = Me._employeeList.SelectedValue

        Try


            updaterole.UPDATEUSERROLES(responsible, Request.QueryString("roleid"), _ddlBusUnit.SelectedValue, _ddlArea.SelectedValue, _ddlLine.SelectedValue, Request.QueryString("PlantCode"), Request.QueryString("userid"), Request.QueryString("busunit"), Request.QueryString("area"), Request.QueryString("line"), IP.Bids.SharedFunctions.GetCurrentUser.Username, "Y", out_status)

            If out_status <> 0 Then
                Me._lblInactiveStatus.Visible = True
                Me._lblInactiveStatus.Text = IP.Bids.SharedFunctions.LocalizeValue("An update error has occurred. If problems continue please contact the Task Tracker System Administrator.", True)
            Else
                IP.Bids.SharedFunctions.ResponseRedirect("~/DataMaintenance/MaintenanceRoles.aspx?Plantcode=" & Request.QueryString("plantcode"))
                'HttpContext.Current.ApplicationInstance.CompleteRequest()
                'Server.Transfer(Page.ResolveUrl("~/DataMaintenance/MaintenanceRoles.aspx?Plantcode=" & Request.QueryString("plantcode")), False)
                'Me._lblInactiveStatus.Text = "You have successfully inactivated the selected employee."
                'If Request.QueryString("CloseMe") IsNot Nothing Then
                '    ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "CloseMe", "try{parent.document.getElementById('" & Request.QueryString("CloseMe") & "').click();}catch(e){}", True)
                '    '.OKScript = "try{parent.document.getElementById('" & Request.QueryString("CloseMe") & "').click();}catch(e){alert(e);}"
                'End If
                'Server.Transfer(Page.ResolveUrl("~/DetectScreenResolution.aspx")) ' & "?url=" & Server.HtmlEncode(Page.Request.RawUrl))

            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("_btnUpdateRole_Click", , ex)
        End Try
    End Sub

    Protected Sub _btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnCancel.Click
        IP.Bids.SharedFunctions.ResponseRedirect("~/DataMaintenance/MaintenanceRoles.aspx?Plantcode=" & Request.QueryString("plantcode"))
        'HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub

End Class
