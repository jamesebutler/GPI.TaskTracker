'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 06-21-2011
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Imports System.Globalization

Partial Class HelpOnlineTraining
    Inherits IP.Bids.BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandlePageLoad()
    End Sub

    Private Sub HandlePageLoad()
        Master.PageName = "MTT Online Training"
        With Me._gvDemoList
            .DataSource = GeneralTaskTrackerBll.GetHelpFiles
            .DataBind()
            If .Rows.Count > 0 Then
                .UseAccessibleHeader = True
                .HeaderRow.TableSection = TableRowSection.TableHeader
                'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "SortGrid", String.Format(CultureInfo.CurrentCulture, "$(function(){{$('#{0}').tablesorter({headers:{0:false}}); }});", _gvDemoList.ClientID), True)
                'Me.ClientScript.RegisterStartupScript(Page.GetType, "tablesort", String.Format(CultureInfo.CurrentCulture, "$(function(){{$('#{0}').tablesorter({headers:{0:false}}); }});", _gvDemoList.ClientID))
                'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "tablesort", String.Format(CultureInfo.CurrentCulture, "$(function(){{$('#{0}').tablesorter({headers:{0:false}}); }});", _gvDemoList.ClientID), True)
            End If
        End With
    End Sub

    Private Sub HelpOnlineTraining_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.PageName = Master.GetLocalizedValue("Online Training", False) '"Task Details"
        _lblHeaderTitle.Text = Master.PageName
    End Sub
End Class
