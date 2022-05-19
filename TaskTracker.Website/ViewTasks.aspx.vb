Partial Class ViewTasks
    Inherits IP.Bids.BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _taskSearch.RefSite = "MTT"
        Master.PageName = Master.GetLocalizedValue("Task Search", False)
        Master.SetActiveNav(Enums.NavPages.ViewTask.ToString)
    End Sub
End Class
