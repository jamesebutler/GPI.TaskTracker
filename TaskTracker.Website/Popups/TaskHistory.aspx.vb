

Partial Class Popups_TaskHistory
    Inherits IP.Bids.BasePage
    Private taskNumber As Integer
    Private Sub Popups_TaskHistory_Load(sender As Object, e As EventArgs) Handles Me.Load
        ValidateQueryString()
        PopulateHistoryData()
    End Sub

    Private Sub ValidateQueryString()
        If Request.QueryString("TaskNumber") Is Nothing Then
            Throw New ApplicationException("A valid Task Number is required")
        End If
        If Not IsNumeric(Request.QueryString("TaskNumber")) Then
            Throw New ApplicationException("A valid Task Number is required")
        End If
        taskNumber = CInt(Request.QueryString("TaskNumber"))
    End Sub
    Private Sub PopulateHistoryData()
        With _grvTaskHistory
            .DataSource = AuditRecord.GetTaskHistory(taskNumber)
            .DataBind
            If .Rows.Count > 0 Then
                .HeaderRow.TableSection = TableRowSection.TableHeader
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "SortGrid_" & .ClientID, String.Format(System.Globalization.CultureInfo.CurrentCulture, "$(function(){{$('#{0}').tablesorter(); }});", .ClientID), True)
        End With

    End Sub
End Class
