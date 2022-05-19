Public Class ResponsibleUserForCopy
    Public Sub New(responsibleUserNameOrRoleId As String, ByVal responsibleUser As String, dueDate As Date, plantCode As String, dateIsCritical As Boolean)
        If IsNumeric(responsibleUserNameOrRoleId) Then
            Me.ResponsibleUserRoleId = CInt(responsibleUserNameOrRoleId)
            Me.ResponsibleUserName = String.Empty
        Else
            Me.ResponsibleUserRoleId = 0
            Me.ResponsibleUserName = responsibleUserNameOrRoleId
        End If
        Me.DueDate = dueDate ' IP.Bids.SharedFunctions.FormatDate(dueDate)
        Me.PlantCode = plantCode
        Me.DateIsCritical = dateIsCritical
        Me.ResponsibleUser = responsibleUser
    End Sub

    Public Property ResponsibleUserName As String
    Public Property ResponsibleUserRoleId As String
    Public Property DueDate As Date
    Public Property PlantCode As String
    Public Property DateIsCritical As Boolean
    Public Property ResponsibleUser As String
    Public Overrides Function ToString() As String
        Return String.Format("ResponsibleUserName: {0}, ResponsibleUserRoleId: {1}, ResponsibleUser: {2}, DueDate: {3}, PlantCode: {4}, DateIsCritical: {5}", ResponsibleUserName, ResponsibleUserRoleId, ResponsibleUser, IP.Bids.SharedFunctions.FormatDate(DueDate), PlantCode, DateIsCritical.ToString)
    End Function
End Class