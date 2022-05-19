Imports System
Imports System.Linq
Imports System.Net
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports System.Web

Namespace MicrosoftTranslatorSdk
    Public Class Translator
        Private Function RequestToken() As AdmAccessToken
            Dim admToken As AdmAccessToken
            Dim admAuth As New AdmAuthentication("92735252-6327-4752-9c75-f14b699ac0d2", "14i9I2pFGm9gp2nczvTl0GG3Yv8JfezBb7gZCmvD7zA=")
            Try
                admToken = admAuth.GetAccessToken()
                ' Create a header with the access_token property of the returned token
                Return admToken
            Catch e As WebException
                ProcessWebException(e)
            Catch
                Throw
            End Try
            Return Nothing
        End Function

        Public Function TranslateMethod(textToTranslate As String, ByVal fromLocale As String, toLocale As String) As String
            'Dim text As String = "Use pixels to express measurements for padding and margins."

            If HttpContext.Current.Cache.Item(textToTranslate & fromLocale & toLocale) IsNot Nothing Then
                Return HttpContext.Current.Cache.Item(textToTranslate & fromLocale & toLocale)
            End If

            Dim uri As String = Convert.ToString((Convert.ToString("http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Web.HttpUtility.UrlEncode(textToTranslate) + "&from=") & fromLocale) + "&to=") & toLocale
            Dim httpWebRequest As HttpWebRequest = DirectCast(WebRequest.Create(uri), HttpWebRequest)
            Dim authToken As String = Convert.ToString("Bearer ") & RequestToken.AccessToken
            httpWebRequest.Headers.Add("Authorization", authToken)
            Dim response As WebResponse = Nothing
            Dim translatedVersion As String = String.Empty
            Try

                response = httpWebRequest.GetResponse()
                Using stream As Stream = response.GetResponseStream()
                    Dim dcs As New System.Runtime.Serialization.DataContractSerializer(Type.[GetType]("System.String"))
                    Dim translation As String = DirectCast(dcs.ReadObject(stream), String)
                    translatedVersion = translation
                End Using
                HttpContext.Current.Cache.Insert(textToTranslate & fromLocale & toLocale, translatedVersion, Nothing, DateTime.Now.AddHours(1), TimeSpan.Zero)
            Catch
                Throw
            Finally
                If response IsNot Nothing Then
                    response.Close()
                    response = Nothing
                End If
            End Try

            Return translatedVersion
        End Function
        Private Shared Sub ProcessWebException(e As WebException)
            Console.WriteLine("{0}", e.ToString())
            ' Obtain detailed error information
            Dim strResponse As String = String.Empty
            Using response As HttpWebResponse = DirectCast(e.Response, HttpWebResponse)
                Using responseStream As Stream = response.GetResponseStream()
                    Using sr As New StreamReader(responseStream, System.Text.Encoding.ASCII)
                        strResponse = sr.ReadToEnd()
                    End Using
                End Using
            End Using
            Console.WriteLine("Http status code={0}, error message={1}", e.Status, strResponse)
        End Sub
    End Class

    <DataContract> _
    Public Class AdmAccessToken
        <DataMember> _
        Public Property AccessToken() As String
        <DataMember> _
        Public Property TokenType() As String
        <DataMember> _
        Public Property ExpiresIn() As String
        <DataMember> _
        Public Property Scope() As String
    End Class

    Public Class AdmAuthentication
        Public Shared ReadOnly DatamarketAccessUri As String = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13"
        Property ClientId As String
        Property ClientSecret As String
        Property Request As String
        Private token As AdmAccessToken
        'Private accessTokenRenewer As System.Web.UI.Timer
        'Access token expires every 10 minutes. Renew it every 9 minutes only.
        'Private Const RefreshTokenDuration As Integer = 9

        Public Sub New(clientId As String, clientSecret As String)
            Me.clientId = clientId
            Me.clientSecret = clientSecret
            'If clientid or client secret has special characters, encode before sending request
            Me.request = String.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(clientSecret))
            Me.token = HttpPost(DatamarketAccessUri, Me.request)
            'renew the token every specified minutes
            'accessTokenRenewer = New System.Web.UI.Timer(New TimerCallback(AddressOf OnTokenExpiredCallback), Me, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1))
            'accessTokenRenewer = New Timer(New TimerCallback(AddressOf OnTokenExpiredCallback), Me, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1))
        End Sub
        Public Function GetAccessToken() As AdmAccessToken
            Return Me.token
        End Function
        Private Sub RenewAccessToken()
            Dim newAccessToken As AdmAccessToken = HttpPost(DatamarketAccessUri, Me.request)
            'swap the new token with old one
            'Note: the swap is thread unsafe
            Me.token = newAccessToken
            Console.WriteLine(String.Format("Renewed token for user: {0} is: {1}", Me.clientId, Me.token.AccessToken))
        End Sub
        Private Sub OnTokenExpiredCallback(stateInfo As Object)
            Try
                RenewAccessToken()
            Catch ex As Exception
                Console.WriteLine(String.Format("Failed renewing access token. Details: {0}", ex.Message))
            Finally
                Try
                    ' accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1))
                Catch ex As Exception
                    Console.WriteLine(String.Format("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message))
                End Try
            End Try
        End Sub
        Private Function HttpPost(DatamarketAccessUri As String, requestDetails As String) As AdmAccessToken
            'Prepare OAuth request 
            Dim webRequest__1 As WebRequest = WebRequest.Create(DatamarketAccessUri)
            webRequest__1.ContentType = "application/x-www-form-urlencoded"
            webRequest__1.Method = "POST"
            Dim bytes As Byte() = Encoding.ASCII.GetBytes(requestDetails)
            webRequest__1.ContentLength = bytes.Length
            Using outputStream As Stream = webRequest__1.GetRequestStream()
                outputStream.Write(bytes, 0, bytes.Length)
            End Using
            Using webResponse As WebResponse = webRequest__1.GetResponse()
                Dim serializer As New DataContractJsonSerializer(GetType(AdmAccessToken))
                'Get deserialized object from JSON stream
                Dim token As AdmAccessToken = DirectCast(serializer.ReadObject(webResponse.GetResponseStream()), AdmAccessToken)
                Return token
            End Using
        End Function
    End Class
End Namespace

