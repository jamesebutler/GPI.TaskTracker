Imports Microsoft.VisualBasic

Public Class BootstrapHelper
    Public Shared minusLabel As String = "<span class=""glyphicon glyphicon-chevron-up pull-left""></span>&nbsp; "
    Public Shared plusLabel As String = "<span class=""glyphicon glyphicon-chevron-down pull-left""></span>&nbsp; "
    Public Shared Function GetBadgeLabel(ByVal badgeValue As String) As String
        Dim badgeLabel As String = "<span class=""badge pull-right"">{0}</span>"
        Return String.Format(badgeLabel, badgeValue)
    End Function
End Class
