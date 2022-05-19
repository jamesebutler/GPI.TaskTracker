Option Explicit On
Option Strict On
Imports System.Linq

Namespace Popups
    Partial Class BulkEditTasks
        Inherits IP.Bids.BasePage
       
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Page.IsPostBack = False Then
                Dim useCachedData As Boolean = False
                If Request.QueryString("useCachedData") IsNot Nothing Then
                    Boolean.TryParse(Request.QueryString("useCachedData"), useCachedData)
                End If
                If Request.QueryString("postBackTrigger") IsNot Nothing Then
                    _taskEdit.PostBackTriggerControlId = Request.QueryString("postBackTrigger")
                End If
                If Request.QueryString("Page") IsNot Nothing AndAlso IsNumeric(Request.QueryString("Page")) Then
                    _taskEdit.CurrentPage = CInt(Request.QueryString("Page"))
                End If
                _taskEdit.LoadTasks(useCachedData)

            End If
        End Sub
    End Class
End Namespace
