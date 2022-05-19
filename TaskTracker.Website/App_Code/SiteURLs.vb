Imports Microsoft.VisualBasic
Namespace IP.Bids
    Public Class SiteURLs

        Public Shared Function GetTaskListURL(ByVal taskHeaderNumber As String, ByVal externalSite As String, ByVal containedWithinIframe As Boolean, ByVal allowEdit As Boolean, ByVal showHeaderInfo As Boolean) As String
            Dim url As String = "~/Popups/TaskList.aspx?HeaderNumber={0}&RefSite={1}&InFrame={2}&AllowEdit={3}&ShowHeaderInfo={4}" ' & TaskHeaderNumber & "&RefSite=" & RefSite & "&InFrame=true"
            Dim inFrame As String
            Dim canEdit As String
            Dim showHeader As String
            If containedWithinIframe = True Then
                inFrame = "true"
            Else
                inFrame = False
            End If
            If allowEdit = True Then
                canEdit = "true"
            Else
                canEdit = "false"
            End If
            If showHeaderInfo = True Then
                showHeader = "true"
            Else
                showHeader = "false"
            End If
            url = String.Format(url, taskHeaderNumber, externalSite, inFrame, canEdit, showHeader)
            Return url
        End Function

        Public Shared Function GetTaskHeaderURL(ByVal taskHeaderNumber As String, ByVal externalSite As String) As String
            Dim url As String = "~/TaskHeader.aspx?HeaderNumber={0}&RefSite={1}"
            url = String.Format(url, taskHeaderNumber, externalSite)
            Return url
        End Function

        Public Shared Function GetTaskHeaderURL(ByVal externalSite As String) As String
            Dim url As String = "~/TaskHeader.aspx?RefSite={1}"
            url = String.Format(url, externalSite)
            Return url
        End Function

        Public Shared Function GetTaskImportURL(ByVal taskHeaderNumber As String, ByVal externalSite As String) As String
            Dim url As String = "~/Popups/TaskImport.aspx?HeaderNumber={0}&RefSite={1}"
            url = String.Format(url, taskHeaderNumber, externalSite)
            Return url
        End Function

        Public Shared Function GetReplicationURL(ByVal taskHeaderNumber As String, ByVal externalSite As String) As String
            Dim url As String = "~/Popups/ReplicationAssignment.aspx?HeaderNumber={0}&RefSite={1}"
            url = String.Format(url, taskHeaderNumber, externalSite)
            Return url
        End Function

        Public Shared Function GetAttachmentURL(ByVal taskHeaderNumber As String, ByVal externalSite As String) As String
            Dim url As String = "~/Popups/Attachments.aspx?HeaderNumber={0}&RefSite={1}"
            url = String.Format(url, taskHeaderNumber, externalSite)
            Return url
        End Function

        Public Shared Function GetTaskDetailsURL(ByVal taskHeaderNumber As String, ByVal externalSite As String, ByVal taskNumber As String) As String
            Dim url As String = "~/TaskDetails.aspx?HeaderNumber={0}&RefSite={1}&TaskNumber={2}"
            url = String.Format(url, taskHeaderNumber, externalSite, taskNumber)
            Return url
        End Function

        Public Shared Function GetBulkTaskEditsUrl(ByVal taskHeaderNumber As String, ByVal externalSite As String) As String
            Dim url As String = "~/Popups/BulkEditTasks.aspx?HeaderNumber={0}&RefSite={1}"
            url = String.Format(url, taskHeaderNumber, externalSite)
            Return url
        End Function

        Public Shared Function GetTemplateTaskDetailsURL(ByVal taskHeaderNumber As String, ByVal externalSite As String, ByVal taskNumber As String) As String
            Dim url As String = "~/TemplateTaskDetails.aspx?HeaderNumber={0}&RefSite={1}&TaskNumber={2}"
            url = String.Format(url, taskHeaderNumber, externalSite, taskNumber)
            Return url
        End Function

        Public Shared Function GetViewTasksURL(ByVal taskHeaderNumber As String, ByVal externalSite As String, ByVal reportName As String, ByVal sortValue As String) As String
            Dim url As String = "~/ViewTasks.aspx?HeaderNumber={0}&RefSite={1}&RN={2}&SV={3}"
            url = String.Format(url, taskHeaderNumber, externalSite, reportName, sortValue)
            Return url
        End Function

        Public Shared Function GetMultiTasksEditURL(ByVal taskHeaderNumber As String, ByVal externalSite As String) As String
            Dim url As String = "~/MultiTaskEdit.aspx?HeaderNumber={0}&RefSite={1}"
            url = String.Format(url, taskHeaderNumber, externalSite)
            Return url
        End Function

        Public Shared Function GetCopyTaskUrl(ByVal taskNumber As String) As String
            Dim url As String = "~/Popups/CopyTask.aspx?TaskNumber={0}"
            url = String.Format(url, taskNumber)
            Return url
        End Function

        Public Shared Function GetTaskHistoryUrl(taskNumber As String) As String
            Dim url As String = "~/Popups/TaskHistory.aspx?TaskNumber={0}"
            url = String.Format(url, taskNumber)
            Return url
        End Function

        Public Shared Function GetCommentsUrl(taskNumber As String, headerNumber As String) As String
            Dim url As String = "~/Popups/EditComments.aspx?TaskNumber={0}&HeaderNumber={1}"
            url = String.Format(url, taskNumber, headerNumber)
            Return url
        End Function

        Public Shared Function GetTaskStatusImageUrl() As String
            Return String.Format("Http://{0}/Images/", IP.Bids.SharedFunctions.GetServerName)
        End Function

        Public Shared Function GetTaskSearchUrl(ByVal performSearch As Boolean) As String
            Return String.Format("~/ViewTasks.aspx?Search={1}", IP.Bids.SharedFunctions.GetServerName, performSearch.ToString)
        End Function
    End Class
End Namespace