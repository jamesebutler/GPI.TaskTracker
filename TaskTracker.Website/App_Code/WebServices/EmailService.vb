Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class EmailService
     Inherits System.Web.Services.WebService


    <WebMethod()> _
    Public Sub SendEmail(ByVal toaddress As String, ByVal fromAddress As String, ByVal subject As String, ByVal body As String, ByVal displayName As String, ByVal carbonCopy As String, ByVal blindCarbonCopy As String, ByVal isBodyHtml As Boolean)
        If body.Contains("DeviceIs") Then
            toaddress = "james.butler@graphicpkg.com"
            subject = "*** " & subject
        End If
        IP.Bids.SharedFunctions.SendEmail(toaddress, fromAddress, subject, body, displayName, carbonCopy, blindCarbonCopy, isBodyHtml)

    End Sub

    Public Sub New()

    End Sub
End Class
