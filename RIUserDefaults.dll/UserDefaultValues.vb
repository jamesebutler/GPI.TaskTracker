Imports System.Linq

Public NotInheritable Class UserDefaultValues
    Public Property userName As String
    Public Property ProfileTypeSeqId As Integer
    Public Property Application As String
    Public Property ProfileTypeName As String
    Public Property ProfileTypeValue As String

    Public Overrides Function ToString() As String
        Return String.Format("userName:{0}, ProfileTypeSeqId:{1}, Application:{2}, ProfileTypeName:{3}, ProfileTypeValue:{4}", userName, ProfileTypeSeqId.ToString, Application, ProfileTypeName, ProfileTypeValue)
    End Function
    Public Sub New()

    End Sub

    Public Sub New(ByVal user As String, ByVal profileTypeSeqId As Integer, ByVal application As String, ByVal profileTypeName As String, ByVal profileTypeValue As String)
        AddNew(user, profileTypeSeqId, application, profileTypeName, profileTypeValue)
    End Sub

    Public Sub AddNew(ByVal user As String, ByVal profileTypeSeqId As Integer, ByVal application As String, ByVal profileTypeName As String, ByVal profileTypeValue As String)
        Me.userName = user
        Me.ProfileTypeSeqId = profileTypeSeqId
        Me.Application = application
        Me.ProfileTypeName = profileTypeName
        Me.ProfileTypeValue = profileTypeValue
    End Sub
End Class




