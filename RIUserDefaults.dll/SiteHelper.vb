Public Class SiteHelper


    Public Property SiteId As String = String.Empty

    Public Sub New(ByVal plantCode As String)
        GetSiteId(plantCode)
    End Sub

    Private Sub GetSiteId(plantCode As String)
        Dim userDefaultsTA As New UserDefaultsTableAdapters.SiteIdTableAdapter
        Dim userDefaultsDT As UserDefaults.SiteIdDataTable = Nothing
        Dim rsUserDefaults As Object = Nothing

        userDefaultsDT = userDefaultsTA.GetSiteIdByPlantCode(plantCode)
        If userDefaultsDT Is Nothing Then Exit Sub
        Try
            For Each rowItem As UserDefaults.SiteIdRow In userDefaultsDT.Rows
                If rowItem.IsSITEIDNull = False Then
                        SiteId = rowItem.SITEID
                        Exit For
                End If
            Next
        Catch
            Throw
        End Try
    End Sub

End Class
