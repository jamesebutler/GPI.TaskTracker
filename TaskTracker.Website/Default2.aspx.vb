
Imports IP.Bids.SharedFunctions
Imports System.Web.Services

Partial Class Default2
    Inherits IP.Bids.BasePage


    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub

    Private Sub Default2_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Try
        '    Dim translator As TranslatorHelper = New TranslatorHelper()

        '    Dim test = translator.ClientId

        '    Dim translatedText = translator.TranslateText("this is a test", "", "ru-ru")

        '    Dim desiredResult = "this is a test"

        'Catch ex As Exception

        '    Dim msg = ex.Message
        'End Try
        Dim sb As New StringBuilder
        For i As Integer = 1 To 316
            sb.AppendFormat("<div><img data-u=""image"" src=""images/LipSync/LipSync ({0}).JPG"" /><img data-u=""thumb"" src=""images/LipSync/LipSync ({0}).JPG"" /></div>", i.ToString)
            sb.AppendLine(" ")
        Next
        Response.Write(sb.ToString)
    End Sub
End Class
