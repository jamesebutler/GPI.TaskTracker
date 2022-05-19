'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-07-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Drawing
Imports System.Security.Cryptography
Imports System.Web.Security
Imports System.Configuration
Imports System.DateTime
Imports System
Imports System.DayOfWeek
Imports System.Collections.Specialized
Imports System.Collections
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Globalization
Imports System.Xml
Imports System.Net
Imports System.Xml.Xsl
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO.Compression
Imports HelperDal
Imports Devart.Data.Oracle
Imports System.ComponentModel

Imports System.Diagnostics


Namespace IP.Bids
    Public NotInheritable Class SharedFunctions
        Public Enum Ordinal
            First = 1
            Second = 2
            Third = 3
            Fourth = 4
            Fifth = 6
            Sixth = 7
            Last = 999
        End Enum

        Public Enum SourceSystemID As Integer
            Replicated = 62
            Template = 61
            IRIS = 1
            ReliabilityIncident = 2
            EIS = 3
            MOC = 41
            MemphisEHS = 21
            OutageTemplate = 82
            OutageReplicated = 81
            Tanks = 83
        End Enum

        Public Shared Function GetServerName() As String
            Dim serverName As String = ConfigurationManager.AppSettings.Item("DevelopmentServer")
            If HttpContext.Current IsNot Nothing Then
                'If HttpContext.Current.Request.UserHostAddress = "127.0.0.1" Then
                If HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("localhost") Then
                    serverName = ConfigurationManager.AppSettings.Item("DevelopmentServer")
                ElseIf HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("ridev") Then
                    serverName = ConfigurationManager.AppSettings.Item("DevelopmentServer")
                ElseIf HttpContext.Current.Request.ServerVariables("SERVER_NAME").ToLower.Contains("ritest") Then
                    serverName = ConfigurationManager.AppSettings.Item("TestServer")
                Else
                    serverName = ConfigurationManager.AppSettings.Item("ProductionServer")
                End If
            Else
                serverName = ""
            End If
            Return serverName
        End Function
        Public Shared Function TotalMemory() As String
            Dim retVal As String = String.Empty
            'Dim pc As New Diagnostics.PerformanceCounter("ASP.NET Applications", "Cache % Machine Memory Limit Used", True)
            'pc.InstanceName = "TOTAL"
            'retVal = String.Format("{0:0.00}%", pc.NextValue())
            Return retVal
        End Function
        ''' <summary>
        ''' This routine adds a Glyph symbol to a specified column within a Gridview
        ''' </summary>
        ''' <param name="grid"></param>
        ''' <param name="item"></param>
        ''' <param name="sortexp"></param>
        ''' <param name="sortDirec"></param>
        ''' <remarks></remarks>
        Public Shared Sub AddGlyph(ByVal grid As GridView, ByVal item As GridViewRow, ByVal sortexp As String, ByVal sortDirec As String)
            Dim glyph As New Label
            Try
                glyph.EnableTheming = False
                glyph.Font.Name = "webdings"
                glyph.Font.Size = FontUnit.Small
                glyph.Text = CStr(IIf(CDbl(sortDirec) = SortDirection.Ascending, "5", "6"))

                For i As Integer = 0 To grid.Columns.Count - 1
                    Dim colExpr As String = grid.Columns(i).SortExpression
                    If colExpr.Length > 0 And colExpr = sortexp Then
                        item.Cells(i).Controls.Add(glyph)
                    End If
                Next
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("AddGlyph", , ex)
            End Try

        End Sub
        'Public Shared Function IsValidDate(ByVal pExpression As Object) As Boolean
        '    Try
        '        ' Default
        '        IsValidDate = False
        '        ' Check we have an object
        '        If Not pExpression Is Nothing Then
        '            Dim expressionString As String = ""

        '            ' Check it's a string (and isn't empty)
        '            If pExpression.GetType Is expressionString.GetType AndAlso Not pExpression = String.Empty Then
        '                Dim dateRegEx As Regex = New Regex("^(((0[1-9]|[12][0-9]|3[01])([-./])((jan)|(mar)|(may)|(jul)|(aug)|(oct)|(dec))([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])((apr)|(jun)|(sep)|(nov))([-./])(\{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(feb)([-./])(\d{4}))|((29)(\.|-|\/)(feb)([-./])([02468][048]00))|((29)([-./])(feb)([-./])([13579][26]00))|((29)([-./])(feb)([-./])([0-9][0-9][0][48]))|((29)([-./])(feb)([-./])([0-9][0-9][2468][048]))|((29)([-./])(feb)([-./])([0-9][0-9][13579][26])))$")
        '                ' Check it's a date
        '                If Not pExpression Is Nothing Then
        '                    If dateRegEx.IsMatch(pExpression) Then
        '                        IsValidDate = True
        '                    Else
        '                        IsValidDate = False
        '                    End If
        '                End If
        '            End If
        '        End If
        '    Catch
        '        IsValidDate = False
        '    End Try


        'End Function
        Shared Sub ClearCurrentUserSession()
            HttpContext.Current.Session.Remove("CurrentUserInfo")
        End Sub
        Shared Function GetCurrentUser() As IP.Bids.UserInfo
            Dim currentUser As IP.Bids.UserInfo
            currentUser = HttpContext.Current.Session.Item("CurrentUserInfo")
            'currentUser = "APACIPAPER\JARAWA"
            If currentUser Is Nothing OrElse currentUser.Username Is Nothing Then
                currentUser = New IP.Bids.UserInfo()
                If currentUser IsNot Nothing AndAlso currentUser.Username IsNot Nothing Then
                    HttpContext.Current.Session.Item("CurrentUserInfo") = currentUser
                End If
            End If
            Return currentUser
        End Function
        Shared Sub SendEmail(ByVal toaddress As String, ByVal fromAddress As String, ByVal subject As String, ByVal body As String, Optional ByVal displayName As String = "Manufacturing Task", Optional ByVal carbonCopy As String = "", Optional ByVal blindCarbonCopy As String = "", Optional ByVal isBodyHtml As Boolean = True)
            Dim mail As System.Net.Mail.MailMessage = New Net.Mail.MailMessage '= New MailMessage(New MailAddress(fromAddress, displayName), New MailAddress(toaddress))

            Dim okToSend As Boolean = False
            Dim inputAddress As New System.Text.StringBuilder

            'MsgBox(toaddress)
            Try
                'Comment following line after test runs
                'subject = subject & toaddress

                'toaddress = strDefaultEmail
                'toaddress = "amy.albrinck@ipaper.com"

                If toaddress.Length > 0 Then
                    Dim toEmail As String() = Split(toaddress, ",")
                    For i As Integer = 0 To toEmail.Length - 1
                        If toEmail(i).Length > 0 Then 'And isEmail(toEmail(i)) Then
                            mail.To.Add(toEmail(i))
                        End If
                    Next
                    If mail.To.Count > 0 Then
                        okToSend = True
                    End If
                End If

                'carbonCopy = "cathy.cox@ipaper.com,amy.albrinck@ipaper.com"
                If carbonCopy.Length > 0 Then
                    Dim copyEmail As String() = Split(carbonCopy, ",")
                    For i As Integer = 0 To copyEmail.Length - 1
                        If copyEmail(i).Length > 0 Then 'And isEmail(copyEmail(i)) Then
                            mail.CC.Add(copyEmail(i))
                        End If
                    Next
                    If mail.CC.Count > 0 Then
                        okToSend = True
                    End If
                End If

                'If strEmailBCC <> "" Then
                '    blindCarbonCopy = strEmailBCC
                'End If
                'blindCarbonCopy = "amy.albrinck@ipaper.com"
                If blindCarbonCopy.Length > 0 Then
                    Dim bccEmail As String() = Split(blindCarbonCopy, ",")
                    For i As Integer = 0 To bccEmail.Length - 1
                        If bccEmail(i).Length > 0 Then 'And isEmail(bccEmail(i)) Then
                            mail.Bcc.Add(bccEmail(i))
                        End If
                    Next
                    If mail.Bcc.Count > 0 Then
                        okToSend = True
                    End If
                End If

                If fromAddress.Trim.Length > 0 Then ' And isEmail(fromAddress) Then
                    mail.From = New Net.Mail.MailAddress(fromAddress, displayName)
                Else
                    mail.From = New Net.Mail.MailAddress("manufacturing.task@graphicpkg.com", "Manufacturing Task")
                End If
                mail.Priority = Net.Mail.MailPriority.High
                mail.IsBodyHtml = isBodyHtml

                Dim isNonAscii As Boolean
                For Each c As Char In subject
                    If AscW(c) > 127 Then
                        isNonAscii = True
                        Exit For
                    End If
                Next
                If isNonAscii Then
                    If subject.Length > 60 Then
                        subject = subject.Substring(0, 60) & "..."
                    End If
                End If

                mail.SubjectEncoding = System.Text.Encoding.UTF8
                mail.Subject = subject
                mail.Body = body
                'Send the email message
                If okToSend = True Then
                    Dim emailTryCount As Integer = 0
                    Dim emailSuccess As Boolean = False
                    Do While emailTryCount < 5 And emailSuccess = False
                        Dim client As Net.Mail.SmtpClient = New Net.Mail.SmtpClient()
                        Try
                            With client
                                emailTryCount += 1
                                '.Host = "smtp.ipaper.com"
                                .Host = "gpimail.na.graphicpkg.pri"
                                .Timeout = 1000000
                                .Send(mail)
                                emailSuccess = True
                                InsertAuditRecord("MTT SendEmail", body)
                            End With
                        Catch ex As Net.Mail.SmtpException
                            If emailTryCount < 5 Then
                                System.Threading.Thread.Sleep(1000)
                            Else
                                Throw New Net.Mail.SmtpException(ex.StatusCode, "Unable to deliver email after " & emailTryCount & " attempts")
                            End If
                        Finally
                            client = Nothing
                        End Try

                    Loop
                    If emailSuccess = False Then
                        Throw New Net.Mail.SmtpException("Unable to deliver email after " & emailTryCount & " attempts")
                    End If
                End If

            Catch ex As Net.Mail.SmtpException
                HandleError("Send Email", "This attempted email message was not sent b/c :" & ex.Message & "<br>" & body & inputAddress.ToString, ex)
            Catch ex As Exception
                HandleError("Send Email", "This attempted email message was not sent b/c :" & ex.Message & "<br>" & body & inputAddress.ToString, ex)
            Finally
                If mail IsNot Nothing Then
                    mail = Nothing
                End If
            End Try
        End Sub
        Shared Function CleanString(ByVal strEdit As String, ByVal defaultValue As String) As String
            Return System.Text.RegularExpressions.Regex.Replace(strEdit, "[\n]", defaultValue).Trim
        End Function
        Shared Sub DisablePageCache(ByVal resp As System.Web.HttpResponse)
            resp.Cache.SetCacheability(HttpCacheability.NoCache)
            resp.Cache.SetExpires(Now.AddSeconds(-1))
            resp.Cache.SetNoStore()
            resp.AppendHeader("Pragma", "no-cache")

        End Sub
        Public Shared Function ConvertOracleOrdinalToMS(ByVal dayOfWeek As Integer) As Integer
            If dayOfWeek >= 1 And dayOfWeek <= 7 Then
                Return dayOfWeek - 1
            Else
                Return dayOfWeek
            End If
        End Function

        Public Shared Function ListToDataTable(Of T)(lst As IList(Of T)) As DataTable

            Dim currentDT = CreateTable(Of T)()

            Dim entType As Type = GetType(T)

            Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(entType)
            For Each item As T In lst
                Dim row As DataRow = currentDT.NewRow()
                For Each prop As PropertyDescriptor In properties

                    If prop.PropertyType = GetType(Nullable(Of Decimal)) OrElse prop.PropertyType = GetType(Nullable(Of Integer)) OrElse prop.PropertyType = GetType(Nullable(Of Int64)) Then
                        If prop.GetValue(item) Is Nothing Then
                            row(prop.Name) = 0
                        Else
                            row(prop.Name) = prop.GetValue(item)
                        End If
                    Else
                        row(prop.Name) = prop.GetValue(item)



                    End If
                Next
                currentDT.Rows.Add(row)
            Next

            Return currentDT

        End Function

        Public Shared Function CreateTable(Of T)() As DataTable
            Dim entType As Type = GetType(T)
            Dim tbl As New DataTable("NewDataTable")
            Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(entType)
            For Each prop As PropertyDescriptor In properties
                If prop.PropertyType = GetType(Nullable(Of Decimal)) Then
                    tbl.Columns.Add(prop.Name, GetType(Decimal))
                ElseIf prop.PropertyType = GetType(Nullable(Of Integer)) Then
                    tbl.Columns.Add(prop.Name, GetType(Integer))
                ElseIf prop.PropertyType = GetType(Nullable(Of Int64)) Then
                    tbl.Columns.Add(prop.Name, GetType(Int64))
                Else
                    tbl.Columns.Add(prop.Name, prop.PropertyType)
                End If
            Next
            Return tbl
        End Function


        Public Shared Function ConvertMSToOracleOrdinal(ByVal dayOfWeek As Integer) As Integer
            If dayOfWeek >= 0 And dayOfWeek <= 6 Then
                Return dayOfWeek + 1
            Else
                Return dayOfWeek
            End If
        End Function
        Public Shared Function OrdinalDayOfWeekOfMonth(ByVal inputDate As Date, ByVal dayOfWeek As System.DayOfWeek, ByVal inputOrdinal As Ordinal) As Date

            Dim newDate As Date = New Date(inputDate.Year, inputDate.Month, 1)
            Dim count As Integer = 1
            Do While count < 31 Or count = inputOrdinal
                Do While newDate.DayOfWeek <> dayOfWeek
                    newDate = newDate.AddDays(1)
                Loop
                count += 1
            Loop
            Return newDate

        End Function

        Public Shared Function LastDayOfMonth(ByVal dt As Date) As Date
            Dim t As New Date(dt.Year, dt.Month, 28)
            Return t.AddDays(4 - t.AddDays(4).Day)
        End Function

        Public Shared Function FirstDayOfWeek(ByVal dt As Date) As Date
            Do While dt.DayOfWeek <> Sunday
                dt = dt.AddDays(-1)
            Loop
            Return dt
        End Function

        Public Shared Function LastDayOfWeek(ByVal dt As Date) As Date
            Do While dt.DayOfWeek <> Saturday
                dt = dt.AddDays(1)
            Loop
            Return dt
        End Function

        Public Shared Sub WriteTrace(ByVal enteringFunction As Boolean)
            If Not HttpContext.Current.Trace.IsEnabled Then
                Exit Sub
            End If
            Dim callingMethodName As String = ""
            Dim action As String = CStr(IIf(enteringFunction = True, "Entering", "Exiting"))
            Try
                callingMethodName = CallingFunctionName(2)
            Catch
                Throw
            End Try
            HttpContext.Current.Trace.Write(action, callingMethodName)
        End Sub
        Public Shared Function FindBusinessDay(ByVal numberOfBusinessDays As Integer, ByVal fromDate As DateTime) As DateTime

            'This is used to count the number of business days 

            Dim businessDays As Integer = 0

            Dim noOfDays As Integer = numberOfBusinessDays

            For i As Integer = 1 To numberOfBusinessDays

                'if current date is the WeekEnd increase the
                'numberOfBusinessDays with one
                'this is because we need one more loop ocurrrence
                If fromDate.DayOfWeek = System.DayOfWeek.Saturday Or fromDate.DayOfWeek = System.DayOfWeek.Sunday Then
                    numberOfBusinessDays += 1
                Else
                    businessDays += 1
                End If

                'When businessDays is not equal to noOfDays,

                'add one day in the current date.

                If businessDays <> noOfDays Then
                    fromDate = fromDate.AddDays(1)
                Else
                    Exit For
                End If
            Next

            Return fromDate
        End Function
        Public Shared Sub WriteTrace(ByVal enteringFunction As Boolean, ByVal callingFunctionName As String)
            If Not HttpContext.Current.Trace.IsEnabled Then
                Exit Sub
            End If
            Dim callingMethodName As String = ""
            Dim action As String = CStr(IIf(enteringFunction = True, "Entering", "Exiting"))
            Try
                If callingFunctionName.Length = 0 Then
                    callingMethodName = callingFunctionName(2)
                Else
                    callingMethodName = callingFunctionName
                End If
            Catch
                Throw
            End Try
            HttpContext.Current.Trace.Write(action, callingMethodName)
        End Sub

        ''' <summary>
        ''' Determine the name of the calling function. 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CallingFunctionName(Optional ByVal stackLevel As Integer = 1) As String
            Dim retVal As String = String.Empty
            Try
                Dim stackTrace As New System.Diagnostics.StackTrace()
                If stackTrace.FrameCount >= stackLevel Then
                    retVal = stackTrace.GetFrame(stackLevel).GetMethod().Name
                End If
            Catch ex As Exception
                HttpContext.Current.ClearError()
                retVal = "Undetermined Method"
            End Try
            Return retVal
        End Function

        Public Shared Function GetJavasriptVersion() As String
            Return GetJavasriptVersion("")
        End Function

        Public Shared Function GetJavasriptVersion(ByVal appSettingsKey As String) As String
            Dim javascriptVersion As String
            If appSettingsKey.Length > 0 Then
                javascriptVersion = System.Configuration.ConfigurationManager.AppSettings(appSettingsKey).ToString(CultureInfo.CurrentCulture)
            Else
                javascriptVersion = System.Configuration.ConfigurationManager.AppSettings("JavascriptVersion").ToString(CultureInfo.CurrentCulture)
            End If
            If javascriptVersion.Length = 0 Then
                javascriptVersion = 1
            End If
            Return javascriptVersion
        End Function

        Public Shared Sub BindList(ByRef rbl As UI.WebControls.ListControl, ByVal obj As OrderedDictionary, ByVal forceUnique As Boolean, ByVal insertBlank As Boolean)
            If obj IsNot Nothing Then
                rbl.DataSource = obj
                rbl.Items.Clear()
                rbl.DataTextField = "Key"
                rbl.DataValueField = "Value"

                rbl.DataBind()
                If insertBlank = True Then
                    rbl.Items.Insert(0, "")
                End If
                If forceUnique = True Then
                    RemoveDuplicateItems(rbl)
                End If

                If System.Web.HttpContext.Current.Request.Form(rbl.UniqueID.ToString) IsNot Nothing Then
                    Dim curValue As String = System.Web.HttpContext.Current.Request.Form(rbl.UniqueID.ToString)
                    If rbl.Items.FindByValue(curValue) IsNot Nothing And curValue.Length > 0 Then
                        rbl.SelectedValue = curValue
                    End If
                End If
            End If
        End Sub
        Public Shared Function CausedPostBack(ByVal sender As String) As Boolean
            Dim currentRequest As System.Web.HttpContext
            currentRequest = System.Web.HttpContext.Current
            If currentRequest.Request.Form("__EventTarget") IsNot Nothing Then
                If currentRequest.Request.Form("__EventTarget").Contains(sender) Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        Public Shared Sub RemoveDuplicateItems(ByRef cbo As UI.WebControls.ListControl)
            Dim ht As New Hashtable
            Dim j As Integer

            If cbo.Items.Count > 0 Then
                While j < cbo.Items.Count
                    If ht.ContainsKey(cbo.Items(j)) Then
                        cbo.Items.RemoveAt(j)
                    Else
                        ht.Add(cbo.Items(j), cbo.Items(j))
                        j = j + 1
                    End If
                End While

                ht = Nothing
            End If
        End Sub
        ''' <summary>
        ''' Change the case of the first letter of each word to upper case.   
        ''' </summary>
        ''' <param name="text">The string to convert to title case.</param>
        ''' <param name="culture">The culture information to be used.</param>
        ''' <param name="forceCasing">When true, forces all words to be lower case before changing everything to title case.</param>
        ''' <returns>The string in title case.</returns>
        Public Shared Function ToTitleCase(ByVal text As String, ByVal culture As System.Globalization.CultureInfo, ByVal forceCasing As Boolean) As String
            If (forceCasing) Then
                Return culture.TextInfo.ToTitleCase(text.ToLower(CultureInfo.CurrentCulture))
            Else
                Return culture.TextInfo.ToTitleCase(text)
            End If
        End Function
        ''' <summary>
        ''' Change the case of the first letter of each word to upper case.
        ''' </summary>
        ''' <param name="text">The string to convert to title case.</param>
        ''' <returns>The string in title case.</returns>
        ''' <remarks></remarks>
        Public Shared Function ToTitleCase(ByVal text As String) As String
            Return ToTitleCase(text, System.Threading.Thread.CurrentThread.CurrentCulture, False)
        End Function

        ''' <summary>
        ''' Change the case of the first letter of each word to upper case.
        ''' </summary>
        ''' <param name="text">The string to convert to title case</param>
        ''' <param name="forceCasing">When true, forces all words to be lower case before changing everything to title case</param>
        ''' <returns>The string in title case</returns>
        ''' <remarks></remarks>
        Public Shared Function ToTitleCase(ByVal text As String, ByVal forceCasing As Boolean) As String
            Return ToTitleCase(text, System.Threading.Thread.CurrentThread.CurrentCulture, forceCasing)
        End Function

        ''' <summary>
        ''' Change the case of the first letter of each word to upper case.
        ''' </summary>
        ''' <param name="text">The string to convert to title case</param>
        ''' <param name="culture">The culture information to be used</param>
        ''' <returns>The string in title case</returns>
        ''' <remarks></remarks>
        Public Shared Function ToTitleCase(ByVal text As String, ByVal culture As System.Globalization.CultureInfo) As String
            Return ToTitleCase(text, culture, False)
        End Function

        Public Shared Function GetTheme(Optional ByVal id As Integer = -1) As String()
            'Dim themeTable As New ThemesTableAdapters.GetThemesTableAdapter
            Dim sb As New StringBuilder
            If id > 0 Then
                '    Dim themeData As System.Data.DataTable = themeTable.GetThemeDataByID(id)
                '    If themeData IsNot Nothing Then
                '        If themeData.Rows.Count > 0 Then
                '            sb.Append(DataClean(themeData.Rows(0).Item("Level1")))
                '            sb.Append(",")
                '            sb.Append(DataClean(themeData.Rows(0).Item("Level2")))
                '            sb.Append(",")
                '            sb.Append(DataClean(themeData.Rows(0).Item("Level3")))
                '            sb.Append(",")
                '            sb.Append(DataClean(themeData.Rows(0).Item("Level4")))
                '            sb.Append(",")
                '        End If
                '    End If
            Else
                '    sb.Append("#4981CE")
                '    sb.Append(",")
                '    sb.Append("7AA3DB")
                '    sb.Append(",")
                '    sb.Append("BBD0ED")
                '    sb.Append(",")
                '    sb.Append("DBE6F6")
                '    sb.Append(",")
            End If
            Return Split(sb.ToString, ",")
        End Function

        Public Shared Function GetSqlServerDataReader(ByVal cmd As Data.SqlClient.SqlCommand) As Data.SqlClient.SqlDataReader
            Dim cn As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings.Item("CEMRConnectionString").ConnectionString
            Dim newConnection As New Data.SqlClient.SqlConnection(cn)
            newConnection.Open()
            cmd.Connection = newConnection
            Dim sqlReader As Data.SqlClient.SqlDataReader = cmd.ExecuteReader(Data.CommandBehavior.CloseConnection)
            Return sqlReader
        End Function
        Public Shared Function DataClean(ByVal value As Object) As String
            Return DataClean(value, "")
        End Function
        Public Shared Function DataClean(ByVal value As Object, ByVal alternateValue As String) As String
            If value IsNot Nothing And value IsNot DBNull.Value Then
                Return Trim(CStr(value))
            Else
                Return alternateValue
            End If
        End Function
        Public Shared Function GenerateException(ByVal excep As Exception, ByVal additionalErrMsg As String, Optional ByVal methodName As String = "", Optional ByVal friendlyMessage As String = "") As String
            Dim le As Exception
            Dim exceptionMessage As String = String.Empty

            Try
                If excep IsNot Nothing Then
                    le = excep
                Else
                    le = HttpContext.Current.Server.GetLastError
                End If

                If methodName.Length = 0 Then
                    'Get the calling method name
                    Dim stackTrace As New System.Diagnostics.StackTrace()
                    methodName = stackTrace.GetFrame(1).GetMethod().Name
                End If
                exceptionMessage = "Exception in " & methodName & ": " & additionalErrMsg
                If friendlyMessage.Length > 0 Then
                    exceptionMessage = exceptionMessage & ", " & friendlyMessage
                End If
            Catch ex As Exception
                exceptionMessage = exceptionMessage & "," & ex.Message
            End Try
            Return exceptionMessage
        End Function
        Public Shared Sub HandleError(Optional ByVal methodName As String = "", Optional ByVal additionalErrMsg As String = "", Optional ByVal excep As Exception = Nothing, Optional ByVal friendlyMessage As String = "")
            Dim le As Exception
            Dim errorMessage As StringBuilder = New StringBuilder
            Dim errorCount As Integer = 0
            Dim errMsg As String = String.Empty
            '            Dim chunkLength As Integer = 0 ' COMMENTED BY CODEIT.RIGHT
            'Dim maxLen As Integer = 10000

            Try
                If excep IsNot Nothing Then
                    le = excep
                Else
                    le = HttpContext.Current.Server.GetLastError
                End If
                If le.Message.Contains("System.Threading.ThreadAbortException") Then
                    Exit Sub
                End If
                If methodName.Length = 0 Then
                    'Get the calling method name
                    Dim stackTrace As New System.Diagnostics.StackTrace()
                    methodName = stackTrace.GetFrame(1).GetMethod().Name
                End If
                If le IsNot Nothing Then
                    'Dim sb As New StringBuilder

                    Do While le IsNot Nothing
                        errorCount = errorCount + 1
                        'errorMessage.Length = 0
                        errorMessage.Append("<Table width=100% border=1 cellpadding=2 cellspacing=2 bgcolor='#cccccc'>")
                        errorMessage.Append("<tr><th colspan=2><h2>Page Error</h2></th></tr>")
                        errorMessage.Append("<tr><td><b>User:</b></td><td>{0}</td></tr>")
                        errorMessage.Append("<tr><td><b>Exception #</b></td><td>{1}</td></tr>")
                        errorMessage.Append("<tr><td><b>Browser Info:</b></td><td>{2}</td></tr>")
                        errorMessage.Append("<tr><td><b>Page:</b></td><td>{3}</td></tr>")
                        errorMessage.Append("<tr><td><b>Method Name:</b></td><td>{4}</td></tr>")
                        errorMessage.Append("<tr><td><b>Time:</b></td><td>{5}</td></tr>")
                        errorMessage.Append("<tr><td><b>Details:</b></td><td>{6}</td></tr>")
                        errorMessage.Append("<tr><td><b>Server:</b></td><td>{8}</td></tr>")
                        errorMessage.Append("<tr><td><b>Additional Info:</b></td><td>{7}</td></tr>")
                        errorMessage.Append("<tr><td><b>URL:</b></td><td>{9}</td></tr>")
                        errorMessage.Append("</table>")
                        errMsg = errorMessage.ToString
                        If additionalErrMsg.Length = 0 And friendlyMessage.Length > 0 Then
                            additionalErrMsg = friendlyMessage
                        End If
                        Dim servername As String = HttpContext.Current.Request.ServerVariables("SERVER_NAME") & " (" & My.Computer.Name & ") "
                        Dim url As String = HttpContext.Current.Request.Url.AbsolutePath
                        errMsg = String.Format(System.Globalization.CultureInfo.CurrentCulture, errMsg, CurrentUserProfile.GetCurrentUser, errorCount, HttpContext.Current.Request.ServerVariables("HTTP_USER_AGENT"), HttpContext.Current.Request.Url, methodName, FormatDateTime(Now, DateFormat.LongDate) & " " & Now.ToShortTimeString, le.ToString & ", " & le.StackTrace, additionalErrMsg, servername, url)
                        additionalErrMsg = ""
                        le = le.InnerException
                        errorMessage.Length = 0

                        'For i As Integer = 0 To errMsg.Length Step maxLen
                        '    If errMsg.Length < maxLen Then
                        '        chunkLength = errMsg.Length - 1
                        '    Else
                        '        If errMsg.Length - i < maxLen Then
                        '            chunkLength = errMsg.Length - i
                        '        Else
                        '            chunkLength = maxLen
                        '        End If
                        '    End If

                        '    Dim errValue As String = errMsg.Substring(i, chunkLength)
                        InsertAuditRecord(methodName, errMsg)

                        '    System.Threading.Thread.Sleep(1000) ' Sleep for 1 second
                        '    Next
                    Loop
                    If friendlyMessage.Length > 0 Then
                        AddPageError(friendlyMessage)
                    End If
                End If
            Catch ex As Exception
                HttpContext.Current.Server.ClearError()
            Finally
                le = Nothing
                'HttpContext.Current.Response.Redirect("~/ri/Help/ErrorPage.aspx", False)

            End Try
        End Sub
        Public Shared Sub AddPageError(ByVal message As String)
            Try
                Dim sb As New StringBuilder
                If HttpContext.Current.Session.Item("PageError") IsNot Nothing Then
                    sb = TryCast(HttpContext.Current.Session.Item("PageError"), StringBuilder)
                End If

                If sb IsNot Nothing Then
                    If sb.Length > 0 Then
                        sb.Append("<hr>")
                    End If
                    sb.Append("<h2>What Happened?</h2>")
                    sb.Append(message)
                    'sb.Append("<h2>How This Will Affect You</h2>")
                    'sb.Append("<h2>What You Can Do About It</h2>")
                    sb.Append("<h2>Support Info</h2>")
                End If
                HttpContext.Current.Session.Add("PageError", sb)
            Catch
                Throw
            End Try
        End Sub
        Public Shared Function GetPageError() As String
            Try
                Dim sb As New StringBuilder
                If HttpContext.Current.Session.Item("PageError") IsNot Nothing Then
                    sb = TryCast(HttpContext.Current.Session.Item("PageError"), StringBuilder)
                End If
                Return sb.ToString
            Catch
                Throw
            End Try
        End Function
        Public Shared Sub ClearPageError()
            If HttpContext.Current.Session.Item("PageError") IsNot Nothing Then
                HttpContext.Current.Session.Remove("PageError")
            End If
        End Sub
        Public Shared Function IsDBConnected() As Boolean
            Dim connectionString As String = ConfigurationManager.ConnectionStrings.Item("CEMRConnectionString").ConnectionString
            Dim retValue As Boolean
            Dim sqlCon As New Data.SqlClient.SqlConnection(connectionString)
            Try
                sqlCon.Open()
                retValue = True
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("IsDBConnected", "Error connecting to " & connectionString, ex)
                retValue = False
            Finally
                sqlCon.Close()
                If sqlCon IsNot Nothing Then
                    sqlCon = Nothing
                End If
            End Try
            Return retValue
        End Function

        Public Shared Sub InsertAuditRecord(ByVal sourceName As String, ByVal errorMessage As String)
            'INSERT INTO RCFA_AUDIT_LOG VALUES ('DeleteRINumber', SYSDATE, SUBSTR(V_ERRMSG,1,1000) );
            Dim paramCollection As New OracleParameterCollection
            Dim param As New OracleParameter
            '            Dim ds As System.Data.DataSet = Nothing ' COMMENTED BY CODEIT.RIGHT

            Try


                sourceName = "MTT:" & My.Computer.Name & ":" & sourceName
                param = New OracleParameter
                param.ParameterName = "in_name"
                param.OracleDbType = OracleDbType.VarChar
                param.Direction = Data.ParameterDirection.Input
                param.Value = sourceName
                paramCollection.Add(param)

                param = New OracleParameter
                param.ParameterName = "in_desc"
                param.OracleDbType = OracleDbType.VarChar
                param.Direction = Data.ParameterDirection.Input
                param.Value = Left(errorMessage, 3000)
                paramCollection.Add(param)

                Dim returnStatus As String = CallDROraclePackage(paramCollection, "Reladmin.RIAUDIT.InsertErrorRecord")
                If CDbl(returnStatus) <> 0 Then
                    'Throw New Data.DataException("Error Marking Analysis Complete for " & RINumber)
                End If
            Catch ex As Exception
                HttpContext.Current.Server.ClearError()
                'Throw New Data.DataException("Error Marking Analysis Complete for " & RINumber)
            Finally
                param = Nothing
                paramCollection = Nothing
            End Try
        End Sub
        Public Shared Sub InsertAuditRecordOld(ByVal sourceName As String, ByVal errorMessage As String)
            Try
                If sourceName.Length > 0 AndAlso errorMessage.Length > 0 Then

                End If
                '    Dim ta As New UserDataTableAdapters.AuditRecordsTableAdapter
                '    ta.Insert(sourceName, errorMessage, Now, My.User.Name)
                '    ta = Nothing
                'Catch ex As Exception
                '    HttpContext.Current.Response.Write(ex.Message & "-" & errorMessage)
                'End Try
                '    'INSERT INTO RCFA_AUDIT_LOG VALUES ('DeleteRINumber', SYSDATE, SUBSTR(V_ERRMSG,1,1000) );
                '    Dim paramCollection As New OracleParameterCollection
                '    Dim param As New OracleParameter
                '    Dim ds As System.Data.DataSet = Nothing

                '    Try

                '        param = New OracleParameter
                '        param.ParameterName = "in_name"
                '        param.OracledbType = OracledbType.VarChar
                '        param.Direction = Data.ParameterDirection.Input
                '        param.Value = sourceName
                '        paramCollection.Add(param)

                '        param = New OracleParameter
                '        param.ParameterName = "in_desc"
                '        param.OracledbType = OracledbType.VarChar
                '        param.Direction = Data.ParameterDirection.Input
                '        param.Value = errorMessage
                '        paramCollection.Add(param)

                '        Dim returnStatus As String = RI.SharedFunctions.CallDROraclePackage(paramCollection, "Reladmin.RIAUDIT.InsertErrorRecord")
                '        If CDbl(returnStatus) <> 0 Then
                '            'Throw New Data.DataException("Error Marking Analysis Complete for " & RINumber)
                '        End If
            Catch ex As Exception
                '        HttpContext.Current.Server.ClearError()
                '        'Throw New Data.DataException("Error Marking Analysis Complete for " & RINumber)
                '    Finally
                '        param = Nothing
                '        paramCollection = Nothing
            End Try
        End Sub
        Public Shared Function AdjustTextForDisplay(ByVal text As String, ByVal targetWidth As Integer, ByVal ft As Font) As String
            Dim textSize As Drawing.SizeF = MeasureString(text, ft)
            Dim colWidth As Integer = targetWidth
            If textSize.Width > colWidth Then
                Dim delta As Integer = CType((textSize.Width - colWidth), Integer)
                Dim avgCharWidth As Integer = CType((textSize.Width / text.Length), Integer)
                Dim chrToTrim As Integer = CType((delta / avgCharWidth), Integer)
                Dim rawText As String = text.Substring(0, text.Length - (chrToTrim + 2)) '+ "..."
                Dim fmt As String = "{1}"
                Return String.Format(CultureInfo.CurrentCulture, fmt, text, rawText)
            End If
            Return text
        End Function
        Public Shared Function MeasureString(ByVal text As String, ByVal fontInfo As Font) As SizeF
            Dim size As SizeF
            Dim emSize As Single = fontInfo.Size 'Convert.ToSingle(fontInfo.Size.Unit.Value + 1)
            If emSize = 0 Then
                emSize = 12
            End If
            Dim stringFont As Font = New Font(fontInfo.Name, emSize)
            Dim bmp As Bitmap = New Bitmap(1000, 100)
            Dim g As Graphics = Graphics.FromImage(bmp)
            size = g.MeasureString(text, stringFont)
            g.Dispose()
            Return size
        End Function
        Public Shared Function FindControlRecursive(ByVal root As Control, ByVal id As String) As Control
            If root.ID = id Then
                Return root
            End If
            For Each ctl As Control In root.Controls
                Dim foundCtl As Control = FindControlRecursive(ctl, id)
                If foundCtl IsNot Nothing Then
                    Return foundCtl
                End If
            Next
            Return Nothing
        End Function

        Public Shared Function GetListBoxValues(ByVal cb As ListBox) As String
            Dim sb As New StringBuilder
            For i As Integer = 0 To cb.Items.Count - 1
                If cb.Items(i) IsNot Nothing Then
                    ' List the selected items
                    If sb.Length > 0 Then
                        sb.Append(",")
                    End If
                    sb.Append(cb.Items(i).Value.Trim)
                End If
            Next
            Return sb.ToString
        End Function
        Public Shared Sub SetListBoxValues(ByRef cb As ListBox, ByVal value As String, Optional ByVal delimeter As String = ",")
            Dim list As String() = value.Split(CChar(delimeter))
            If list.Length > 0 Then
                For i As Integer = 0 To list.Length - 1
                    If cb.Items.FindByValue(list(i).Trim) IsNot Nothing Then
                        cb.Items.FindByValue(list(i).Trim).Selected = True
                    Else
                        cb.Items.Add(list(i))
                    End If
                Next
            Else
                If cb.Items.FindByValue(value) IsNot Nothing Then
                    cb.SelectedValue = value
                End If
            End If
        End Sub
        Public Shared Function JoinValues(ByVal value As String) As String
            Return "'" & String.Join("','", value.Split(CChar(","))) & "'"
        End Function
        Public Shared Function GetLocalizedJQueryDateFormat() As String
            Return "d MM yy" '"d M yy" '"M d, yy"
        End Function
        Public Shared Function GetLocalizedDateFormat() As String
            Return "dd MMMM yyyy"
            'Dim numberOfMs = Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.YearMonthPattern.ToCharArray().Count(Function(c) c = "M")
            'Dim numberOfYs = Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.YearMonthPattern.ToCharArray().Count(Function(c) c = "y")
            'Dim numberOfDs = Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.MonthDayPattern.ToCharArray().Count(Function(c) c = "d")

            'Return String.Format("{0} {1} {2}", New String("d", numberOfDs), New String("M", numberOfMs), New String("y", numberOfYs))
            ''Return Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern
            'Return Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern
            ''If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower = "es-mx" Then
            ''    Return "dd MMMM yyyy" '"dd MMM yyyy" '"MMM dd, yyyy"
            ''    'ElseIf Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower = "pt-br" Then
            ''    '    Return "dd MMM yyyy" '"dd MMM yyyy" '"MMM dd, yyyy"
            ''Else
            ''    Return "dd MMMM yyyy" '"MMM dd, yyyy"
            ''End If
        End Function
        Public Shared Function GetSortableDateFormat() As String
            Return "yyyy-MM-dd" '"MMM dd, yyyy"
        End Function
        Public Shared Function GetSortableDate(ByVal dt As String) As String
            If IsDate(dt) Then
                Return Microsoft.VisualBasic.Strings.Format(CDate(dt), GetSortableDateFormat) 'Microsoft.VisualBasic.Strings.Format(CDate(IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(dt, "en-us")), "yyyy-MM-dd")
            Else
                Return Microsoft.VisualBasic.Strings.Format(New Date(1900, 1, 1), GetSortableDateFormat)
            End If
        End Function
        Public Shared Function GetDate(ByVal dt As String) As String
            If IsDate(dt) Then
                Return Microsoft.VisualBasic.Strings.Format(CDate(dt), GetLocalizedDateFormat)
            Else
                Return ""
            End If
        End Function

        Public Shared Function GetCheckBoxValues(ByVal cb As CheckBoxList) As String
            Dim sb As New StringBuilder
            For i As Integer = 0 To cb.Items.Count - 1
                If cb.Items(i).Selected Then
                    ' List the selected items
                    If sb.Length > 0 Then
                        sb.Append(",")
                    End If
                    sb.Append(cb.Items(i).Value.Trim)
                End If
            Next
            Return sb.ToString
        End Function
        Public Shared Sub SetCheckBoxValues(ByRef cb As CheckBoxList, ByVal value As String, Optional ByVal delimeter As String = ",")
            value = DataClean(value)
            Dim list As String() = value.Split(CChar(delimeter))
            If list.Length > 0 Then
                'TODO: MJP Added on 9/24/2009
                cb.ClearSelection()
                For i As Integer = 0 To list.Length - 1
                    If cb.Items.FindByValue(list(i).Trim) IsNot Nothing Then
                        cb.Items.FindByValue(list(i).Trim).Selected = True
                    End If
                Next
            Else
                If cb.Items.FindByValue(value) IsNot Nothing Then
                    cb.SelectedValue = value
                End If
            End If
        End Sub
        Public Shared Sub SetCheckBoxValues(ByRef cb As IP.Bids.UserControls.ExtendedCheckBoxList, ByVal value As String, Optional ByVal delimeter As String = ",")
            value = DataClean(value)
            Dim list As String() = value.Split(CChar(delimeter))
            If list.Length > 0 Then
                'TODO: MJP Added on 9/24/2009
                cb.ClearSelection()
                For i As Integer = 0 To list.Length - 1
                    If cb.Items.FindByValue(list(i).Trim) IsNot Nothing Then
                        cb.Items.FindByValue(list(i).Trim).Selected = True
                    End If
                Next
            Else
                If cb.Items.FindByValue(value) IsNot Nothing Then
                    cb.SelectedValue = value
                End If
            End If
        End Sub
        Public Shared Function GetCurrentTrustLevel() As String
            For Each trustLevel As AspNetHostingPermissionLevel In New AspNetHostingPermissionLevel() {AspNetHostingPermissionLevel.Unrestricted, AspNetHostingPermissionLevel.High, AspNetHostingPermissionLevel.Medium, AspNetHostingPermissionLevel.Low, AspNetHostingPermissionLevel.Minimal}
                Try

                    Dim x As New AspNetHostingPermission(trustLevel)
                    x.Demand()
                Catch generatedExceptionName As System.Security.SecurityException
                    Continue For
                End Try

                Return DirectCast([Enum].Parse(GetType(AspNetHostingPermissionLevel), trustLevel), AspNetHostingPermissionLevel).ToString  'trustLevel
            Next

            Return DirectCast([Enum].Parse(GetType(AspNetHostingPermissionLevel), AspNetHostingPermissionLevel.None), AspNetHostingPermissionLevel) 'AspNetHostingPermissionLevel.None
        End Function

        Public Shared Function GetLoadedAssemblies() As String
            Dim sb As New StringBuilder
            For Each a As System.Reflection.Assembly In AppDomain.CurrentDomain.GetAssemblies()
                If sb.Length > 0 Then
                    sb.Append("<br>")
                End If
                sb.Append(a.FullName)
            Next
            Return sb.ToString
        End Function

        Public Shared Sub DeleteEntireCache()
            Dim objItem As Object
            Dim strName As String = String.Empty
            For Each objItem In HttpContext.Current.Cache
                strName = objItem.Key
                'Comment the If..Then if you want to see ALL (System, etc.) items the cache
                'We don't want to see ASP.NET cached system items or ASP.NET Worker Processes
                If (Left(strName, 7) <> "ISAPIWo") Then '(Left(strName, 7) <> "System.") And 
                    If HttpContext.Current.Cache.Item(strName) IsNot Nothing Then
                        HttpContext.Current.Cache.Remove(strName)
                    End If
                End If
            Next

        End Sub

        Public Shared Function InitCulture(ByVal selectedCulture As String) As String
            Dim returnCulture As String = ""

            Try
                Dim cultureIsSet As Boolean

                If selectedCulture.Length > 0 Then
                    cultureIsSet = SetCulture(selectedCulture)
                    If cultureIsSet = True Then returnCulture = selectedCulture
                End If

                If cultureIsSet = False Then
                    returnCulture = "Auto"
                End If
            Catch
                Throw
            Finally

            End Try
            Return returnCulture
        End Function
        Public Shared Function SetCulture(ByVal culture As String) As Boolean
            Dim returnValue As Boolean
            If (culture <> "Auto") Then
                Try
                    'Dim ci As New System.Globalization.CultureInfo(culture)
                    Dim ci As System.Globalization.CultureInfo
                    ci = System.Globalization.CultureInfo.GetCultureInfo(culture)
                    System.Threading.Thread.CurrentThread.CurrentCulture = ci
                    System.Threading.Thread.CurrentThread.CurrentUICulture = ci
                    'Me.UICulture = culture
                    'Me.Culture = culture
                    returnValue = True
                Catch ex As ArgumentNullException
                    'System.ArgumentNullException: name is null.
                    returnValue = False
                Catch ex As System.ArgumentException
                    'System.ArgumentException: name specifies a culture that is not supported.
                    returnValue = False
                Catch
                    Throw
                End Try
            End If
            Return returnValue
        End Function

        Public Shared Function CDateFromEnglishDate(ByVal dateValue As String) As Date
            Dim newDate As Date
            Dim cI As System.Globalization.CultureInfo
            Try
                cI = System.Globalization.CultureInfo.GetCultureInfo("EN-US")
                newDate = Convert.ToDateTime(dateValue, cI)
            Catch
                Throw
            End Try
            Return newDate
        End Function

        Public Shared Function FormatDateTimeToEnglish(ByVal dateValue As Date) As String
            Dim monthValue As Integer = dateValue.Month
            Dim dayValue As Integer = dateValue.Day
            Dim yearValue As Integer = dateValue.Year
            Dim newDate As Date = New Date(yearValue, monthValue, dayValue)
            Dim tmp As String = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(newDate, "EN-US", "d")
            Return tmp
        End Function

        Public Shared Function FormatDateTimeToEnglish(ByVal dateValue As Date, ByVal includeTime As Boolean) As String
            Dim monthValue As Integer = dateValue.Month
            Dim dayValue As Integer = dateValue.Day
            Dim yearValue As Integer = dateValue.Year
            Dim newDate As Date = New Date(yearValue, monthValue, dayValue)
            Dim tmp As String
            If includeTime = True Then
                tmp = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(newDate, "EN-US", "G")
            Else
                tmp = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(newDate, "EN-US", "d")
            End If
            Return tmp
        End Function

        Public Shared Function FormatDate(ByVal dt As DateTime) As String
            Return Microsoft.VisualBasic.Strings.Format(dt, GetLocalizedDateFormat)
        End Function
        Public Shared Function FormatDate(ByVal dt As String) As String
            Return IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(dt, "EN-US", GetLocalizedDateFormat)
        End Function

        Public Shared Function LocalizeDropDownNameValue(ByVal data As AjaxControlToolkit.CascadingDropDownNameValue()) As AjaxControlToolkit.CascadingDropDownNameValue()
            Dim riResources As IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization
            'If HttpContext.Current.Request.Cookies("SelectedCulture") IsNot Nothing Then
            If HttpContext.Current.Session.Item("SelectedCulture") IsNot Nothing Then
                Dim lang As String = HttpContext.Current.Session.Item("SelectedCulture") 'HttpContext.Current.Request.Cookies("SelectedCulture").Value
                Dim appID As String = ConfigurationManager.AppSettings("ResourceApplicationID")
                riResources = New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization(lang, appID)
            Else
                riResources = New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization()
            End If
            If riResources.CurrentLocale.ToUpper <> "EN-US" Then
                Dim newData As New Generic.List(Of AjaxControlToolkit.CascadingDropDownNameValue)
                For i As Integer = 0 To data.Length - 1
                    newData.Add(New AjaxControlToolkit.CascadingDropDownNameValue(riResources.GetResourceValue(data(i).name), data(i).value, data(i).isDefaultValue))
                Next
                riResources = Nothing
                Return newData.ToArray
            Else
                Return data
            End If
        End Function

        Public Shared Function LocalizeDropDownNameValue(ByVal data As AjaxControlToolkit.CascadingDropDownNameValue(), ByVal compoundFieldDelimiter As String) As AjaxControlToolkit.CascadingDropDownNameValue()
            If compoundFieldDelimiter.Length = 0 Then
                Return LocalizeDropDownNameValue(data)
                Exit Function
            End If
            Dim riResources As IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization
            'If HttpContext.Current.Request.Cookies("SelectedCulture") IsNot Nothing Then
            If HttpContext.Current.Session.Item("SelectedCulture") IsNot Nothing Then
                Dim lang As String = HttpContext.Current.Session.Item("SelectedCulture") 'HttpContext.Current.Request.Cookies("SelectedCulture").Value
                Dim appID As String = ConfigurationManager.AppSettings("ResourceApplicationID")
                riResources = New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization(lang, appID)
            Else
                riResources = New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization()
            End If
            If riResources.CurrentLocale.ToUpper <> "EN-US" Then
                Dim newData As New Generic.List(Of AjaxControlToolkit.CascadingDropDownNameValue)
                For i As Integer = 0 To data.Length - 1
                    Dim dataValues As String() = Split(data(i).name, compoundFieldDelimiter)
                    Dim newDataValue As String
                    If dataValues.Length > 1 Then
                        Dim sb As New StringBuilder
                        For j As Integer = 0 To dataValues.Length - 1
                            sb.Append(riResources.GetResourceValue(dataValues(j).Trim))
                            If j < dataValues.Length - 1 Then
                                sb.Append(compoundFieldDelimiter)
                            End If
                        Next
                        newDataValue = sb.ToString
                    Else
                        newDataValue = riResources.GetResourceValue(data(i).name)
                    End If
                    newData.Add(New AjaxControlToolkit.CascadingDropDownNameValue(newDataValue, data(i).value, data(i).isDefaultValue))
                Next
                riResources = Nothing
                Return newData.ToArray
            Else
                Return data
            End If
        End Function
        Public Shared Function LocalizeList(ByVal data As String, ByVal delimiter As String) As String
            Dim newList() As String = data.Split(delimiter)
            Dim localizedValue As String = String.Empty
            If newList.Length > 0 Then
                For i As Integer = 0 To newList.Length - 1
                    If localizedValue.Length > 0 Then
                        localizedValue = localizedValue & delimiter & " "
                    End If
                    localizedValue = localizedValue & LocalizeValue(newList(i))
                Next
            Else
                localizedValue = LocalizeValue(data)
            End If
            Return localizedValue
        End Function
        Public Shared Function LocalizeValue(ByVal data As String) As String
            Return LocalizeValue(data, False)
        End Function
        Public Shared Function LocalizeValue(ByVal data As String, ByVal locale As String, ByVal returnKeyNotFound As Boolean) As String
            Dim riResources As IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization
            Dim appID As String = ConfigurationManager.AppSettings("ResourceApplicationID")
            riResources = New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization(locale, appID)
            Return riResources.GetResourceValue(data, returnKeyNotFound)
        End Function
        Public Shared Function LocalizeValue(ByVal data As String, ByVal returnKeyNotFound As Boolean) As String
            Dim riResources As IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization
            'If HttpContext.Current.Request.Cookies("SelectedCulture") IsNot Nothing Then
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session.Item("SelectedCulture") IsNot Nothing Then
                Dim lang As String = HttpContext.Current.Session.Item("SelectedCulture") 'HttpContext.Current.Request.Cookies("SelectedCulture").Value
                Dim appID As String = ConfigurationManager.AppSettings("ResourceApplicationID")
                riResources = New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization(lang, appID)
            Else
                riResources = New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization()
            End If
            Return riResources.GetResourceValue(data, returnKeyNotFound)
        End Function

        Public Shared Function StripOffTime(ByVal dateValue As String) As String
            Dim value() As String = dateValue.Split(" ")
            If value.Length > 1 Then
                Return value(0)
            Else
                Return dateValue
            End If
        End Function
        ''' <summary>
        ''' This function will return a unique key based on the value that has been provided
        ''' </summary>
        ''' <param name="criteria">Object - The value that will be converted to a unique key</param>
        ''' <returns>String - a unique key based on the value that has been provided</returns>
        ''' <remarks></remarks>
        Public Shared Function CreateKey(ByVal criteria As Object) As String
            Dim bf As BinaryFormatter = Nothing
            Dim ms As MemoryStream = Nothing
            Dim b() As Byte
            Dim key As String = String.Empty

            Try
                bf = New BinaryFormatter
                ms = New MemoryStream

                bf.Serialize(ms, criteria)

                If ms IsNot Nothing Then
                    b = CompressgZip(ms.ToArray)
                    If b IsNot Nothing Then
                        key = Convert.ToBase64String(b).GetHashCode.ToString  'Convert.ToBase64String(b)
                    End If
                End If
                Return key
            Catch ex As Exception
                Throw
                Return ""
            End Try
        End Function

        ''' <summary>
        ''' Compresses a byte array using gZip Compression
        ''' </summary>
        ''' <param name="b">an array of bytes</param>
        ''' <returns>a compressed array of bytes</returns>
        ''' <remarks></remarks>
        Public Shared Function CompressgZip(ByVal b() As Byte) As Byte()

            Dim ms As MemoryStream = Nothing
            Dim gZip As GZipStream = Nothing

            Try
                ms = New MemoryStream
                gZip = New GZipStream(ms, CompressionMode.Compress, False)
                gZip.Write(b, 0, b.Length)
                gZip.Close()
                Return ms.ToArray
            Catch ex As Exception
                Throw
            Finally
                Try
                    If Not gZip Is Nothing Then
                        gZip.Close()
                        gZip = Nothing
                    End If
                    If Not ms Is Nothing Then
                        ms.Close()
                        ms = Nothing
                    End If
                Catch ex As Exception
                End Try

            End Try

        End Function

        ''' <summary>
        ''' Decompresses a byte array using gZip Compression
        ''' </summary>
        ''' <param name="b">a compressed array of bytes</param>
        ''' <returns>an uncompressed array of bytes</returns>
        ''' <remarks></remarks>
        Public Shared Function DECompressgZip(ByVal b() As Byte) As Byte()

            Dim ms As MemoryStream = Nothing
            Dim gZip As Compression.GZipStream = Nothing

            Dim bChunk(100) As Byte
            Dim size As Integer = 0

            Try
                ms = New MemoryStream
                gZip = New Compression.GZipStream(New MemoryStream(b), CompressionMode.Decompress, True)
                Do While True
                    size = gZip.Read(bChunk, 0, bChunk.Length)
                    If size <= 0 Then
                        Exit Do
                    End If
                    ms.Write(bChunk, 0, size)
                Loop
                gZip.Close()
                Return ms.ToArray
            Catch ex As Exception
                Throw
            Finally
                Try
                    If Not gZip Is Nothing Then
                        gZip.Close()
                        gZip = Nothing
                    End If
                    If Not ms Is Nothing Then
                        ms.Close()
                        ms = Nothing
                    End If
                Catch ex As Exception
                End Try
            End Try
        End Function

        Public Shared Function CleanKnownCategoryValues(ByVal knownCategoryValues As String) As String
            Return Replace(knownCategoryValues, "undefined:", "Facility:")
        End Function


        Public Shared Function GetValidationError(ByVal isValid As Boolean, ByVal pageValidators As System.Web.UI.ValidatorCollection) As String
            Dim msg As String = String.Empty
            If (Not isValid) Then
                ' Loop through all validation controls to see which 
                ' generated the error(s).
                Dim oValidator As IValidator
                For Each oValidator In pageValidators
                    If oValidator.IsValid = False Then
                        msg = msg & "<br />" & oValidator.ErrorMessage
                    End If
                Next
            End If
            Return msg
        End Function

        Public Shared Function IsEnglishDate(ByVal dt As String) As Boolean
            Dim provider As New CultureInfo("en-us")
            Dim format As String = "d"
            Try
                Date.ParseExact(dt, format, provider)
                Return True
            Catch ex As Exception

            End Try
            Return False
        End Function

        Public Shared Function IsEnglishDate(ByVal dt As Date) As Boolean
            Dim provider As New CultureInfo("en-us")
            Dim format As String = "d"
            Try
                Date.ParseExact(dt.ToShortDateString, format, provider)
                Return True
            Catch ex As Exception

            End Try
            Return False
        End Function

        Public Shared Function GetEnglishDate(ByVal dt As String) As Date
            If IsEnglishDate(dt) = False Then Return Nothing
            Dim provider As New CultureInfo("en-us")
            Dim format As String = "d"
            Dim englishDate As Date
            Try
                englishDate = Date.ParseExact(dt, format, provider)
            Catch ex As Exception
                englishDate = Nothing
            End Try
            Return englishDate
        End Function
        Public Shared Function WriteExcelXml(ByRef source As Data.Common.DbDataReader, ByVal excludedFields As ArrayList) As String

            Dim excelDoc As New System.Text.StringBuilder

            If excludedFields IsNot Nothing Then
                excludedFields.Sort()
            Else
                excludedFields = New ArrayList
            End If
            Dim startExcelXML As String = _
                "<?xml version='1.0' ?>" & vbCrLf & _
                "<Workbook " & _
                "xmlns='urn:schemas-microsoft-com:office:spreadsheet' " & vbCrLf & _
                "xmlns:o='urn:schemas-microsoft-com:office:office' " & vbCrLf & _
                "xmlns:x='urn:schemas-microsoft-com:office:excel' " & vbCrLf & _
                "xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet'>" & vbCrLf & _
                "<Styles>" & vbCrLf & _
                "<Style ss:ID='Default' ss:Name='Normal'>" & vbCrLf & _
                "<Alignment ss:Vertical='Bottom'/>" & vbCrLf & _
                "<Borders/>" & vbCrLf & _
                "<Font/>" & vbCrLf & _
                "<Interior/>" & vbCrLf & _
                "<NumberFormat/>" & vbCrLf & _
                "<Protection/>" & vbCrLf & _
                "</Style>" & vbCrLf & _
                "<Style ss:ID='BoldColumn'>" & vbCrLf & _
                "<Font x:Family='Swiss' ss:Bold='1'/>" & vbCrLf & _
                "</Style>" & vbCrLf & _
                "<Style ss:ID=""s21"">" & vbCrLf & _
                "<NumberFormat ss:Format=""m/d/yyyy;@""/>" & vbCrLf & _
                "</Style>" & vbCrLf & _
                "<Style ss:ID=""s23"">" & vbCrLf & _
                "<NumberFormat ss:Format=""Standard""/>" & vbCrLf & _
                "</Style>" & vbCrLf & _
                "<Style ss:ID=""s24""><NumberFormat/></Style>" & vbCrLf & _
                "<Style ss:ID=""s26"">" & vbCrLf & _
                "<NumberFormat ss:Format=""[$-419]d\ mmm\ yy;@""/></Style>" & vbCrLf & _
                "<Style ss:ID=""s28"">" & vbCrLf & _
                "<NumberFormat ss:Format=""[ENG][$-409]d\-mmm\-yy;@""/></Style>" & vbCrLf & _
                "<Style ss:ID=""s66"">" & vbCrLf & _
                "<NumberFormat ss:Format=""[$-416]d\-mmm\-yy;@""/></Style>" & vbCrLf & _
                "<Style ss:ID=""s64""><NumberFormat ss:Format=""[$-40C]d\-mmm\-yy;@""/></Style>" & vbCrLf & _
                "</Styles>"

            Dim endExcelXML As String = "</Workbook>"
            Dim rowCount As Integer = 0
            Dim sheetCount As Integer = 0
            Dim newPage As Boolean = True
            Dim maxRows As Integer = 64000
            Dim iploc As New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization

            ' Write XML header for Excel (changing single to double quotes)
            excelDoc.Append(Replace(startExcelXML, "'", Chr(34)))

            '// Write dataset
            Do While source.Read()
                rowCount = rowCount + 1

                'if the number of rows is > 64000 create a new page to continue output
                If (rowCount > maxRows) Then
                    rowCount = 0
                    newPage = True
                End If

                ' Start a new page?
                If (newPage) Then

                    newPage = False

                    ' Close out subsequent pages
                    If (sheetCount > 0) Then

                        excelDoc.AppendLine("</Table>")
                        excelDoc.AppendLine(" </Worksheet>")

                    End If

                    ' Start a new worksheet
                    sheetCount = sheetCount + 1
                    excelDoc.AppendLine("<Worksheet ss:Name=""Sheet" & sheetCount & """>")
                    excelDoc.AppendLine("<Table>")
                    excelDoc.AppendLine("<Row>")

                    '// Write column headers
                    Dim z As Integer = 0

                    Do While (z < source.FieldCount)
                        excelDoc.Append("<Cell ss:StyleID=""BoldColumn""><Data ss:Type=""String"">")
                        excelDoc.Append(iploc.GetResourceValue(source.GetName(z))) 'Tables(0).Columns(z).ColumnName)
                        excelDoc.AppendLine("</Data></Cell>")
                        z = z + 1
                    Loop

                    excelDoc.AppendLine("</Row>")

                End If

                ' Write out a row of data
                excelDoc.AppendLine("<Row>")

                Dim y As Integer = 0
                Dim sb As New System.Text.StringBuilder
                Dim xxx As New XmlTextWriter(New System.IO.StringWriter(sb))

                Do While (y < source.FieldCount)

                    Dim rowType As System.Type

                    rowType = source.Item(y).GetType()

                    Select Case (rowType.ToString)

                        Case "System.String"
                            If Not IsDate(source.Item(y).ToString) Then
                                Dim xmLstring As String = source.Item(y).ToString

                                If excludedFields.BinarySearch(source.GetName(y), CaseInsensitiveComparer.Default) < 0 Then
                                    xmLstring = iploc.GetResourceValue(xmLstring)
                                End If
                                xmLstring = xmLstring.Trim
                                xmLstring = xmLstring.Replace("&", "&")
                                xmLstring = xmLstring.Replace(">", ">")
                                xmLstring = xmLstring.Replace("<", "<")


                                sb = New System.Text.StringBuilder
                                xxx = New XmlTextWriter(New System.IO.StringWriter(sb))
                                xxx.WriteString(xmLstring)
                                xxx.Flush()
                                xmLstring = sb.ToString()


                                excelDoc.Append("<Cell>" & _
                                    "<Data ss:Type=""String"">")
                                excelDoc.Append(xmLstring)
                                excelDoc.AppendLine("</Data></Cell>")
                            Else
                                Dim xmlDate As Date = CType(source.Item(y), Date)
                                Dim dtformat As String = "{0}-{1}-{2}T00:00:00.000"
                                Dim yr As String = CStr(xmlDate.Year)
                                Dim mon As String = CStr(xmlDate.Month)
                                Dim dy As String = CStr(xmlDate.Day)
                                If mon.Length <> 2 Then
                                    mon = "0" & mon
                                End If
                                If dy.Length <> 2 Then
                                    dy = "0" & dy
                                End If
                                Dim dt As String = String.Format(dtformat, yr, mon, dy)

                                Dim styleKey As String = "s21"
                                If System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToUpper(System.Globalization.CultureInfo.CurrentCulture) = "EN-US" Then
                                    styleKey = "s28"
                                ElseIf System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToUpper(System.Globalization.CultureInfo.CurrentCulture) = "RU-RU" Then
                                    styleKey = "s26"
                                ElseIf System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToUpper(System.Globalization.CultureInfo.CurrentCulture) = "PT-BR" Then
                                    styleKey = "s66"
                                ElseIf System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToUpper(System.Globalization.CultureInfo.CurrentCulture) = "FR-FR" Then
                                    styleKey = "s64"
                                End If
                                excelDoc.Append("<Cell ss:StyleID=""" & styleKey & """>" & _
                                    "<Data ss:Type=""DateTime"">")
                                excelDoc.Append(dt)
                                excelDoc.AppendLine("</Data></Cell>")
                            End If

                        Case "System.DateTime"
                            Dim xmlDate As Date = CType(source.Item(y), Date)
                            Dim dtformat As String = "{0}-{1}-{2}T00:00:00.000"
                            Dim yr As String = CStr(xmlDate.Year)
                            Dim mon As String = CStr(xmlDate.Month)
                            Dim dy As String = CStr(xmlDate.Day)
                            If mon.Length <> 2 Then
                                mon = "0" & mon
                            End If
                            If dy.Length <> 2 Then
                                dy = "0" & dy
                            End If
                            Dim dt As String = String.Format(dtformat, yr, mon, dy)

                            Dim styleKey As String = "s21"
                            If System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToUpper(System.Globalization.CultureInfo.CurrentCulture) = "EN-US" Then
                                styleKey = "s28"
                            ElseIf System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToUpper(System.Globalization.CultureInfo.CurrentCulture) = "RU-RU" Then
                                styleKey = "s26"
                            ElseIf System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToUpper(System.Globalization.CultureInfo.CurrentCulture) = "PT-BR" Then
                                styleKey = "s66"
                            ElseIf System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToUpper(System.Globalization.CultureInfo.CurrentCulture) = "FR-FR" Then
                                styleKey = "s64"
                            End If
                            excelDoc.Append("<Cell ss:StyleID=""" & styleKey & """>" & _
                                "<Data ss:Type=""DateTime"">")
                            excelDoc.Append(dt)
                            excelDoc.AppendLine("</Data></Cell>")

                            'excelDoc.Append("<Cell ss:StyleID=""s21"">" & _
                            '    "<Data ss:Type=""DateTime"">")
                            'excelDoc.Append(dt)
                            'excelDoc.AppendLine("</Data></Cell>")

                        Case "System.Boolean"
                            Dim xmlString As String = source.Item(y).ToString
                            excelDoc.Append("<Cell>" & _
                                "<Data ss:Type=""String"">")
                            If excludedFields.BinarySearch(source.GetName(y), CaseInsensitiveComparer.Default) < 0 Then
                                xmlString = iploc.GetResourceValue(xmlString)
                            End If
                            excelDoc.Append(xmlString)
                            excelDoc.AppendLine("</Data></Cell>")

                        Case "System.Int16", "System.Int32", "System.Int64", "System.Byte"
                            Dim xmlString As String = source.Item(y).ToString

                            If excludedFields.BinarySearch(source.GetName(y), CaseInsensitiveComparer.Default) < 0 Then
                                excelDoc.Append("<Cell ss:StyleID=""s23""><Data ss:Type=""Number"">")
                            Else
                                excelDoc.Append("<Cell ss:StyleID=""s24""><Data ss:Type=""Number"">")
                                '    <Cell ss:StyleID="s24"><Data ss:Type="Number">81657</Data></Cell>
                            End If
                            excelDoc.Append(xmlString)
                            excelDoc.AppendLine("</Data></Cell>")

                        Case "System.Decimal", "System.Double"
                            Dim xmlString As String = source.Item(y).ToString
                            '<Cell ss:StyleID="s23"><Data ss:Type="Number">234234</Data></Cell>
                            'excelDoc.Append("<Cell ss:StyleID=""s23"">" & _
                            '   "<Data ss:Type=""Number"">")
                            If excludedFields.BinarySearch(source.GetName(y), CaseInsensitiveComparer.Default) < 0 Then
                                '    XMLString = FormatNumber(XMLString, 2)
                                excelDoc.Append("<Cell ss:StyleID=""s23""><Data ss:Type=""Number"">")
                                excelDoc.Append(IP.MEASFramework.ExtensibleLocalizationAssembly.Numbers.GetLocalizedNumber(CSng(xmlString), "en-us"))
                            Else
                                excelDoc.Append("<Cell ss:StyleID=""s24""><Data ss:Type=""Number"">")
                                excelDoc.Append(xmlString)

                            End If

                            excelDoc.AppendLine("</Data></Cell>")

                        Case "System.DBNull"
                            excelDoc.Append("<Cell>" & _
                                "<Data ss:Type=""String"">")
                            excelDoc.Append("")
                            excelDoc.AppendLine("</Data></Cell>")

                        Case Else

                            Throw New ArgumentException((rowType.ToString & " not handled."))

                    End Select

                    y = y + 1

                Loop

                excelDoc.AppendLine("</Row>")

            Loop

            ' Close out XML and flush
            excelDoc.AppendLine("</Table>")
            excelDoc.AppendLine("</Worksheet>")
            excelDoc.Append(endExcelXML)
            Return excelDoc.ToString

        End Function

        ''' <summary>
        ''' Fnction to return the text value based on the index provided
        ''' </summary>
        ''' <param name="enum1">Enum to work with</param>
        ''' <param name="name">The value we're looking for</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetEnumStringValue(ByVal enum1 As [Enum], ByVal name As String) As String
            Dim str As String = [Enum].GetName(enum1.GetType(), name)
            Return str
        End Function

        Public Shared Sub SortDdlByText(ByRef objDdl As DropDownList)
            Dim textList As New ArrayList()
            Dim valueList As New ArrayList()


            For Each li As ListItem In objDdl.Items
                textList.Add(li.Text)
            Next

            textList.Sort()


            For Each item As Object In textList
                Dim value As String = objDdl.Items.FindByText(item.ToString()).Value
                valueList.Add(value)
            Next
            objDdl.Items.Clear()

            For i As Integer = 0 To textList.Count - 1
                Dim objItem As New ListItem(textList(i).ToString(), valueList(i).ToString())
                objDdl.Items.Add(objItem)
            Next
        End Sub

        Public Shared Sub ResponseRedirect(ByVal url As String) ', Optional ByVal endResponse As Boolean = False)
            Try
                'InsertAuditRecord("Response.Redirect", "User [" & CurrentUserProfile.GetCurrentUser & "] is Attempting to redirect to [" & url & "]")
                HttpContext.Current.Response.Redirect(url, False) ' endResponse)
                HttpContext.Current.ApplicationInstance.CompleteRequest()
                'InsertAuditRecord("Response.Redirect", "User [" & CurrentUserProfile.GetCurrentUser & "] has redirected to [" & url & "]")
            Catch ex As Exception
                HandleError("Response.Redirect", "Error attempting to redirect to [" & url & "]", ex)
            End Try
        End Sub

        Public Shared Sub InitCulture()
            Dim userprofile As CurrentUserProfile = Nothing
            Dim defaultCulture As String = ""
            Dim selectedCulture As String = ""
            Dim cultureBeingUsed As String = ""
            Dim allKeys() As String = HttpContext.Current.Request.Form.AllKeys
            Dim cultureIsSet As Boolean
            Dim currentUser As IP.Bids.UserInfo
            'Dim defaultLanguage As String = ""

            Try
                currentUser = IP.Bids.SharedFunctions.GetCurrentUser
                'Look to see if the user has selected a different language
                For i As Integer = 0 To allKeys.Length - 1
                    If allKeys(i) IsNot Nothing Then
                        If allKeys(i).Contains("_rblLanguages") Then
                            If HttpContext.Current.Request.Form(allKeys(i).ToString) IsNot Nothing And HttpContext.Current.Request.Form(allKeys(i).ToString).Length > 0 Then
                                selectedCulture = HttpContext.Current.Request.Form(allKeys(i).ToString)
                            End If
                            Exit For
                        End If
                    End If
                Next

                If selectedCulture.Length > 0 Then
                    cultureBeingUsed = IP.Bids.SharedFunctions.InitCulture(selectedCulture)
                    If cultureBeingUsed <> "Auto" And cultureBeingUsed.Length > 0 Then
                        cultureIsSet = True
                    Else
                        cultureIsSet = False
                    End If
                End If



                If cultureIsSet = False Then
                    defaultCulture = currentUser.UserDefaults.GetDefaultValue(RIUserDefaults.UserProfileTypes.Language, "Auto").ToUpper
                    'Populate the current culture from the User Profile table                
                    cultureBeingUsed = IP.Bids.SharedFunctions.InitCulture(defaultCulture)
                    If cultureBeingUsed <> "Auto" And cultureBeingUsed.Length > 0 Then
                        cultureIsSet = True
                    Else
                        cultureIsSet = False
                    End If
                End If

                ' Dim CultureCookie As HttpCookie
                If cultureIsSet = False Then
                    'Use the last selected culture from the cookies
                    'If HttpContext.Current.Request.Cookies("SelectedCulture") IsNot Nothing Then
                    If HttpContext.Current.Session.Item("SelectedCulture") IsNot Nothing Then
                        cultureBeingUsed = IP.Bids.SharedFunctions.InitCulture(IP.Bids.SharedFunctions.DataClean(HttpContext.Current.Session.Item("SelectedCulture"), "EN-US"))
                    End If
                    If cultureBeingUsed <> "Auto" And cultureBeingUsed.Length > 0 Then
                        cultureIsSet = True
                    Else
                        cultureIsSet = False
                        'Me.UICulture = "Auto"
                        'Me.Culture = "Auto"
                        'If HttpContext.Current.Request.Cookies("SelectedCulture") IsNot Nothing Then
                        'If HttpContext.Current.Session.Item("SelectedCulture") IsNot Nothing Then
                        '    HttpContext.Current.Response.Cookies("SelectedCulture").Expires = DateTime.Now.AddDays(-1)
                        'End If
                    End If

                Else
                    'HttpContext.Current.Response.Cookies.Remove("SelectedCulture")
                    'CultureCookie = New HttpCookie("SelectedCulture")
                    'CultureCookie.Value = cultureBeingUsed
                    'HttpContext.Current.Response.SetCookie(CultureCookie)
                    HttpContext.Current.Session.Add("SelectedCulture", cultureBeingUsed)
                End If

            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("InitCulture", , ex)
            Finally
                userprofile = Nothing
            End Try
        End Sub


        Public Shared Sub AddToTraceLog(ByVal Filename As String, ByVal Entry As String, ByVal Sent As Boolean)


            Exit Sub

            Dim s As String = ""
            ' If this us the result of an outgoing transmission, prepend the
            ' text with a right pointing arrow, otherwise use a left pointing
            ' arrow
            If Sent Then

                s += vbCrLf + "START--------------------------------------------------------------------> "
                s += vbCrLf + Entry
            Else
                s += vbCrLf + Entry + vbCrLf
                's += vbCrLf + "<------------------------------------------------------------------------END"
                's += vbCrLf
            End If

            Dim Retry As Long
            Retry = 10
            Do While Retry > 0
                Try
                    Dim Stream As New IO.FileStream(Filename, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                    Stream.Seek(0, IO.SeekOrigin.End)
                    Stream.Write(System.Text.Encoding.Default.GetBytes(s.ToCharArray), 0, System.Text.ASCIIEncoding.ASCII.GetByteCount(s.ToCharArray))
                    Stream.Close()
                    Retry = 0
                Catch ex As Exception
                    Throw
                End Try
            Loop
        End Sub


        Public Shared Function GetDatabaseName() As String
            Dim paramCollection As New OracleParameterCollection
            Dim param As New OracleParameter
            Dim ds As System.Data.DataSet = Nothing
            Dim ret As String = String.Empty
            Try

                param = New OracleParameter
                param.ParameterName = "rsServiceName"
                param.OracleDbType = OracleDbType.Cursor
                param.Direction = Data.ParameterDirection.Output
                paramCollection.Add(param)

                Dim dr As OracleDataReader = GetOraclePackageDR(paramCollection, "Reladmin.RI.GETSERVICENAME")

                If dr IsNot Nothing Then
                    Do While dr.Read
                        If dr.Item("SERVICE_NAME") IsNot Nothing Then
                            ret = CStr(dr.Item("SERVICE_NAME"))
                        End If
                    Loop
                End If
            Catch ex As Exception
                Throw
            Finally
                GetDatabaseName = ret
            End Try
        End Function


        Public Shared Function GetOraclePackageDR(ByRef parms As OracleParameterCollection, ByVal packageName As String) As OracleDataReader
            Dim conCust As OracleConnection = Nothing
            Dim cmdSQL As OracleCommand = Nothing
            Dim connection As String = String.Empty
            Dim provider As String = String.Empty
            Dim dr As OracleDataReader = Nothing
            Dim cnConnection As OracleConnection = Nothing
            Dim returnParamName As String = String.Empty
            Dim returnValue As String = String.Empty
            Dim returnParms As New StringCollection
            Try


                Dim cn As String = System.Web.Configuration.WebConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString



                If connection.Length = 0 Then
                    connection = System.Web.Configuration.WebConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString
                End If
                If provider.Length = 0 Then
                    provider = System.Web.Configuration.WebConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ProviderName
                End If
                cmdSQL = New OracleCommand

                With cmdSQL
                    cnConnection = New OracleConnection(connection)
                    cnConnection.Open()
                    .Connection = cnConnection
                    .CommandText = packageName
                    .CommandType = CommandType.StoredProcedure
                    .PassParametersByName = True
                    Dim sb As New StringBuilder
                    For i As Integer = 0 To parms.Count - 1
                        If parms.Item(i).Value Is Nothing Then parms.Item(i).Value = DBNull.Value
                        Dim parm As New OracleParameter
                        parm.Direction = parms.Item(i).Direction
                        parm.DbType = parms.Item(i).DbType
                        parm.OracleDbType = parms.Item(i).OracleDbType
                        parm.Size = parms.Item(i).Size
                        If parms.Item(i).Direction = ParameterDirection.Input Or parms.Item(i).Direction = ParameterDirection.InputOutput Then
                            If parms.Item(i).Value IsNot Nothing Then
                                parm.Value = parms.Item(i).Value
                                If parm.Value.ToString = "" Then
                                    parm.IsNullable = True
                                    parm.Value = System.DBNull.Value
                                End If
                            Else
                                If parm.OracleDbType = OracleDbType.NVarChar Then
                                    'parm.Value = DBNull.Value
                                    'parm.Size = 2
                                End If
                            End If
                        ElseIf parms.Item(i).Direction = ParameterDirection.Output Then
                            returnParms.Add(parms.Item(i).ParameterName)
                            returnParamName = parms.Item(i).ParameterName
                        End If
                        parm.ParameterName = parms.Item(i).ParameterName
                        .Parameters.Add(parm)
                        If sb.Length > 0 Then sb.Append(",")
                        If parm.OracleDbType = OracleDbType.VarChar Then
                            If parm.Value IsNot Nothing Then
                                sb.Append(parm.ParameterName & "= '" & parm.Value.ToString & "' Type=" & parm.OracleDbType.ToString)
                            Else
                                sb.Append(parm.ParameterName & "= '" & "Null" & "' Type=" & parm.OracleDbType.ToString)
                            End If
                        Else
                            If parm.Value IsNot Nothing Then
                                sb.Append(parm.ParameterName & "= '" & parm.Value.ToString & "' Type=" & parm.OracleDbType.ToString)
                            Else
                                sb.Append(parm.ParameterName)
                            End If
                        End If
                        sb.AppendLine()

                    Next

                End With
                Debug.WriteLine("start: cmdSQL.ExecuteReader  " & DateTime.Now.ToString)
                dr = cmdSQL.ExecuteReader(Data.CommandBehavior.CloseConnection)
                'cmdSQL.ExecuteNonQuery()
                Debug.WriteLine("End: cmdSQL.ExecuteReader  " & DateTime.Now.ToString)
                'Populate the original parms collection with the data from the output parameters
                For i As Integer = 0 To returnParms.Count - 1
                    parms.Item(cmdSQL.Parameters(returnParms.Item(i)).ToString).Value = cmdSQL.Parameters(returnParms.Item(i)).Value.ToString
                Next
                '// return the return value if there is one
                If returnParamName.Length > 0 Then
                    returnValue = cmdSQL.Parameters(returnParamName).Value.ToString
                Else
                    returnValue = CStr(0)
                End If

            Catch ex As Exception
                If returnValue.Length = 0 Then returnValue = "Error Occurred"
                Throw New DataException("GetOraclePackageDR", ex)
                If Not conCust Is Nothing Then conCust = Nothing

            Finally
                Debug.WriteLine("start: GetOraclePackageDR  " & DateTime.Now.ToString)
                GetOraclePackageDR = dr
                Debug.WriteLine("End: GetOraclePackageDR  " & DateTime.Now.ToString)
                If Not dr Is Nothing Then dr = Nothing
                If Not cmdSQL Is Nothing Then cmdSQL = Nothing
                'If cnConnection IsNot Nothing Then
                '    If cnConnection.State = ConnectionState.Open Then cnConnection.Close()
                '    cnConnection = Nothing
                'End If
            End Try
        End Function


    End Class
End Namespace