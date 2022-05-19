Imports Devart.Data.Oracle

Public Class Downtime
    Public Sub New()
        GetDowntimeMessageFromDatabase()
    End Sub

    Public Property StartDate As DateTime
    Public Property EndDate As DateTime
    Public Property Message As String = String.Empty
    Public Property ShowMessage As Boolean

    Private Sub GetDowntimeMessageFromDatabase()
        Dim adapter As IP.TaskTrackerDAL.GeneralSiteTableAdapters.DOWNTIMEMESSAGETableAdapter
        Dim table As IP.TaskTrackerDAL.GeneralSite.DOWNTIMEMESSAGEDataTable
        Dim emptyCursor As Object = Nothing

        Try
            'Get from the database
            adapter = New IP.TaskTrackerDAL.GeneralSiteTableAdapters.DOWNTIMEMESSAGETableAdapter With {
                .Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            }
            table = adapter.GetDowntimeMessage(emptyCursor)
            If table Is Nothing Then Exit Sub

            For Each rowItem As IP.TaskTrackerDAL.GeneralSite.DOWNTIMEMESSAGERow In table.Rows
                If rowItem.IsMESSAGENull Then
                    rowItem.MESSAGE = String.Empty
                End If
                If rowItem.IsSHOWMESSAGENull Then
                    rowItem.SHOWMESSAGE = 0
                End If

                With Me
                    .StartDate = rowItem.MESSAGESTARTDATE
                    .EndDate = rowItem.MESSAGEENDDATE
                    .Message = rowItem.MESSAGE
                    .ShowMessage = CBool(rowItem.SHOWMESSAGE)
                End With
            Next
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetDowntimeMessageFromDatabase", "Error getting the Downtime message from the database", ex)
        Finally
            adapter = Nothing
            table = Nothing
        End Try

    End Sub
End Class
