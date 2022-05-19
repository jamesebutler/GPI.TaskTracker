'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 04-26-2011
'
' Last Modified By : MJPOPE
' Last Modified On : 04-26-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class InactiveUser
    Inherits IP.Bids.BasePage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Master.PageName = Master.GetLocalizedValue("Inactive User", False)
        _lblHeaderTitle.Text = Master.PageName
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim userName As String
        Dim userPlantCode As String = ""
        Dim mCurrentUserInfo As IP.Bids.UserInfo = IP.Bids.SharedFunctions.GetCurrentUser

        Try
            If mCurrentUserInfo IsNot Nothing Then
                _missingAccount.InnerHtml = "This user account for " & mCurrentUserInfo.Name & " [" & mCurrentUserInfo.Username & "] is currently Inactive in the Task Tracker reference data. Please contact a Facility Administrator from the list below to get your inactive userid updated."
            Else
                _missingAccount.InnerHtml = "This user account for " & My.User.Name & " is currently Inactive in the Task Tracker reference data. Please contact a Facility Administrator from the list below to get your inactive userid updated."
            End If

            If Not Page.IsPostBack Then
                userName = IP.Bids.SharedFunctions.GetCurrentUser.Username
                If Request.QueryString("plantcode") IsNot Nothing Then
                    userPlantCode = Request.QueryString("plantcode")
                Else
                    userPlantCode = IP.Bids.SharedFunctions.GetCurrentUser.PlantCode
                End If

                PopulateRoleList(userPlantCode)
            End If
            IP.Bids.SharedFunctions.InsertAuditRecord("MTT Inactive User", _missingAccount.InnerHtml)
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("Inactive User", _missingAccount.InnerHtml, ex)
        End Try
    End Sub
    Private Sub PopulateRoleList(ByVal inPlantCode As String)
        Dim roleList As System.Collections.Generic.List(Of SiteUserRole) = DataMaintenanceBLL.GetSiteUserRoleList(inPlantCode)

        Try

            If roleList Is Nothing Then
                Me._gvRoleList.Visible = "False"
            Else
                Dim filteredRoleList = From role In roleList.Where(Function(obj) obj.RoleName = "FacilityAdmin")
                _gvRoleList.DataSource = filteredRoleList
                _gvRoleList.DataBind()
                _gvRoleList.UseAccessibleHeader = True
                _gvRoleList.HeaderRow.TableSection = TableRowSection.TableHeader
            End If

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("PopulateRoleList", , ex)
        End Try

    End Sub
End Class
