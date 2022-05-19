'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-02-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Option Explicit On
Option Strict On
Imports Microsoft.VisualBasic
Imports System
Imports System.IO.Compression
Imports System.IO
Imports System.Web.UI
Imports IP.Bids
Imports System.Globalization

Namespace IP.Bids



    ''' <summary>
    ''' This class should be inherited by all pages to insure proper error handling
    ''' </summary>
    ''' <remarks>This page will contain all events or functions that should be inherited by all pages</remarks>
    Public Class BasePage
        Inherits System.Web.UI.Page



#Region "Properties"
        Public Property IsIris As Boolean
        Public Property UseBootStrap As Boolean = True
        Private _ScreenWidth As Integer = 1280
        Public Property ScreenWidth() As Integer
            Get
                Return _ScreenWidth
            End Get
            Set(ByVal value As Integer)
                _ScreenWidth = value
            End Set
        End Property

        Private _ScreenHeight As Integer = 1024
        Public Property ScreenHeight() As Integer
            Get
                Return _ScreenHeight
            End Get
            Set(ByVal value As Integer)
                _ScreenHeight = value
            End Set
        End Property

        Private _RefSite As String = String.Empty
        Public Property RefSite() As String
            Get
                Return _RefSite
            End Get
            Set(ByVal value As String)
                _RefSite = value
            End Set
        End Property
#End Region
        ''' <summary>
        ''' This event captures all page level errors and logs them appropriately
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Error
            'Log Errors Here
            SharedFunctions.HandleError(, , , "An unhandled exception has occured")
        End Sub

        Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            Dim currentSite As String = String.Empty




            Try
                If Request.QueryString("RefSite") IsNot Nothing Then
                    currentSite = Request.QueryString("RefSite")
                    If currentSite.Length > 0 Then
                        Session.Item("RefSite") = currentSite
                    ElseIf Session.Item("RefSite") IsNot Nothing Then
                        If CStr(Session.Item("RefSite")).Length > 0 Then
                            currentSite = CStr(Session.Item("RefSite"))
                        End If
                    End If
                ElseIf Session.Item("RefSite") IsNot Nothing Then
                    If CStr(Session.Item("RefSite")).Length > 0 Then
                        currentSite = CStr(Session.Item("RefSite"))
                    End If
                End If
                'Select Case currentSite.ToUpper
                '    Case "IRIS"
                '        Page.Theme = "IRIS"
                '        IsIris = True
                '    Case "TANKS"
                '        Page.Theme = "TANKS"
                '    Case Else
                'JEB 4/19/2022  commented out the line below
                'Page.Theme = "RI"

                'End Select

                RefSite = currentSite
                'Creating the cookie 
                'JEB 4/19/2022  commented out the line below
                'Response.Cookies("Theme").Value = Page.Theme
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("Page_PreInit", , ex)
            End Try
        End Sub

        Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
            'If Session.Item("ScreenResolution") IsNot Nothing Then
            '    Dim screenResolution As String() = Session.Item("ScreenResolution").ToString.Split(CChar("x"))
            '    If screenResolution.Length = 2 Then
            '        Me.ScreenHeight = CInt(screenResolution(1))
            '        Me.ScreenWidth = CInt(screenResolution(0))
            '    End If
            'Else
            '    Session("DetectScreenResolutionURL") = Page.Request.RawUrl
            '    Server.Transfer(Page.ResolveUrl("~/DetectScreenResolution.aspx")) ' & "?url=" & Server.HtmlEncode(Page.Request.RawUrl))
            'End If
            Try
                AddJavascriptFiles()
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) - 5, CultureInfo.CurrentCulture))
                If UseBootStrap Then
                    AddBootStrapStyleSheet()
                End If
                AddFontAwesomeStyleSheet()
                AddFullCalendarStyleSheet()
                AddResponsiveStyleSheet()
                'IP.Bids.SharedFunctions.DisablePageCache(Response)
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("Page_PreLoad", , ex)
            End Try
        End Sub

        Private Sub AddFontAwesomeStyleSheet()

            Dim styleSheet As New Literal
            With styleSheet
                .Text = String.Format("<link href=""{0}"" rel=""stylesheet"" type=""text/css"" />", "https://use.fontawesome.com/119e6a3746.css") 'Page.ResolveClientUrl("~/Content/font-awesome/css/font-awesome.min.css?ver=" & IP.Bids.SharedFunctions.GetJavasriptVersion()))
                .ID = "fontawesomestyle"
            End With
            AddCssFileToPageHeader(styleSheet)
        End Sub

        Private Sub AddFullCalendarStyleSheet() '

            Dim styleSheet As New Literal
            With styleSheet
                .Text = String.Format("<link href=""{0}"" rel=""stylesheet"" type=""text/css"" />", Page.ResolveClientUrl("~/Content/fullcalendar.css?ver=" & IP.Bids.SharedFunctions.GetJavasriptVersion()))
                .ID = "fullcalendar"
            End With
            AddCssFileToPageHeader(styleSheet)

        End Sub
        Private Sub AddResponsiveStyleSheet() '

            Dim styleSheet As New Literal
            With styleSheet
                .Text = String.Format("<link href=""{0}"" rel=""stylesheet"" type=""text/css"" />", Page.ResolveClientUrl("~/Content/Reliability.css?ver=" & IP.Bids.SharedFunctions.GetJavasriptVersion()))
                .ID = "RIResponsive"
            End With
            AddCssFileToPageHeader(styleSheet)

            Dim defaultStyleSheet As New Literal
            With defaultStyleSheet
                .Text = String.Format("<link href=""{0}"" rel=""stylesheet"" type=""text/css"" />", Page.ResolveClientUrl("~/Content/Default.css?ver=" & IP.Bids.SharedFunctions.GetJavasriptVersion()))
                .ID = "DefaultStyleSheet"
            End With
            AddCssFileToPageHeader(defaultStyleSheet)

        End Sub

        Private Sub AddBootStrapStyleSheet() '

            Dim styleSheet As New Literal
            With styleSheet
                .Text = String.Format("<link href=""{0}"" rel=""stylesheet"" type=""text/css"" />", Page.ResolveClientUrl("~/Content/bootstrap.min.css?ver=" & IP.Bids.SharedFunctions.GetJavasriptVersion()))
                .ID = "bootstrapstyle"
            End With
            AddCssFileToPageHeader(styleSheet)
            Dim styleSheetMultiSelect As New Literal
            With styleSheetMultiSelect
                .Text = String.Format("<link href=""{0}"" rel=""stylesheet"" type=""text/css"" />", Page.ResolveClientUrl("~/Content/bootstrap-select.min.css?ver=" & IP.Bids.SharedFunctions.GetJavasriptVersion()))
                .ID = "bootstrapstylemultiselect"
            End With
            AddCssFileToPageHeader(styleSheetMultiSelect)
        End Sub

        Private Sub AddCssFileToPageHeader(ByVal styleSheet As Literal)
            If Page.Header.FindControl(styleSheet.ID) Is Nothing Then Page.Header.Controls.Add(styleSheet)
        End Sub
        ''' <summary>
        ''' Adds the javascript files.
        ''' </summary>
        Private Sub AddJavascriptFiles()
            Dim localScriptManager As ScriptManager = Nothing
            Dim jsVersion As String = String.Empty
            Dim jqueryDateLangFile As String = ""

            Try
                jsVersion = IP.Bids.SharedFunctions.GetJavasriptVersion()
                If IP.Bids.SharedFunctions.FindControlRecursive(Me, "_scriptManager") IsNot Nothing Then
                    localScriptManager = CType(IP.Bids.SharedFunctions.FindControlRecursive(Me, "_scriptManager"), ScriptManager)
                End If
                If localScriptManager Is Nothing Then
                    localScriptManager = New ScriptManager
                    Me.Controls.Add(localScriptManager)
                End If


                If Page IsNot Nothing AndAlso Page.Culture IsNot Nothing AndAlso Page.Culture.Length > 0 Then
                    'Original source of language js files http://jquery-ui.googlecode.com/svn/trunk/ui/i18n/

                    Select Case DirectCast(System.Threading.Thread.CurrentThread.CurrentCulture, System.Globalization.CultureInfo).Name.ToLower
                        Case "en-us"
                            jqueryDateLangFile = "" 'Default
                        Case "ru-ru"
                            jqueryDateLangFile = "jquery.ui.datepicker-ru-ru.js"
                        Case "fr-fr"
                            jqueryDateLangFile = "jquery.ui.datepicker-fr.js"
                        Case "pt-br"
                            jqueryDateLangFile = "jquery.ui.datepicker-pt-BR.js"
                        Case "es-mx"
                            jqueryDateLangFile = "jquery.ui.datepicker-es-mx.js"
                        Case "zh-cn"
                            jqueryDateLangFile = "jquery.ui.datepicker-zh-CN.js"
                        Case Else
                            jqueryDateLangFile = String.Format("jquery.ui.datepicker-{0}.js", DirectCast(System.Threading.Thread.CurrentThread.CurrentCulture, System.Globalization.CultureInfo).Name.ToLower)
                    End Select
                End If

                With localScriptManager
                    .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/Source/StartEndCalendar.js?ver=" & jsVersion)))
                    If IsIris Then
                        .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/JQuery UI/js/jquery-1.4.2.min.js")))
                        .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/JQuery UI/js/jquery-ui-1.8.5.custom.min.js")))
                    Else
                        '.Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/JQuery UI/js/jquery-1.4.2.min.js")))
                        '.Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/JQuery UI/js/jquery-ui-1.8.5.custom.min.js")))

                        .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/jquery-2.1.4.min.js")))
                        .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/jquery-ui-1.9.0.min.js")))
                        .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/JavaScriptmsiefix.js")))   'JEB 11/19/2018
                        If UseBootStrap Then
                            .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/bootstrap.min.js")))
                            .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/bootstrap-select.min.js")))
                        End If
                    End If
                    '.Scripts.Add(New ScriptReference(Page.ResolveUrl("https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.3/jquery-ui.min.js")))
                    'jquery.tablesorter.min.js  
                    '.Scripts.Add(New ScriptReference("https://mottie.github.io/tablesorter/dist/js/jquery.tablesorter.min.js"))
                    .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/JQuery UI/js/jquery.tablesorter.min.js")))
                    .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/JQuery UI/js/jquery.metadata.js")))
                    '.Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/JQuery UI/js/jquery.ui.datepicker-fr.js")))
                    If jqueryDateLangFile.Length > 0 Then
                        '.Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/JQuery UI/js/" & jqueryDateLangFile & "?ver=" & jsVersion)))
                    End If
                    .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/Source/jquery.textareaAutoResize.js?ver=" & jsVersion)))
                    .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/Source/Master.js?ver=" & jsVersion)))
                    .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/Source/ResponsibleUsers.js?ver=" & jsVersion)))

                    'Add Full Calendar
                    .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/moment.min.js?ver=" & jsVersion)))
                    .Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/fullcalendar/fullcalendar.js?ver=" & jsVersion)))

                    '.Scripts.Add(New ScriptReference("https://use.fontawesome.com/119e6a3746.js"))


                End With

                If localScriptManager.IsInAsyncPostBack Then
                    ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "dialog-confirm", AddJQueryDialog(), True)
                Else
                    If Not Page.ClientScript.IsStartupScriptRegistered(Page.GetType, "dialog-confirm") Then
                        Page.ClientScript.RegisterStartupScript(Page.GetType, "dialog-confirm", AddJQueryDialog(), True)
                    End If
                End If

            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("AddJavascriptFiles", , ex)
            End Try
        End Sub

        ''' <summary>
        ''' Adds a Localized version of the JQuery Dialog
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function AddJQueryDialog() As String
            Dim sbJQuery As New StringBuilder
            Dim labelYes As String = String.Empty
            Dim labelNo As String = String.Empty
            Dim lableTitle As String = String.Empty

            Try
                labelYes = IP.Bids.SharedFunctions.LocalizeValue("Yes", True)
                labelNo = IP.Bids.SharedFunctions.LocalizeValue("No", True)
                lableTitle = IP.Bids.SharedFunctions.LocalizeValue("Please Confirm", True)
                With sbJQuery
                    .AppendLine("$(function(){")
                    .AppendLine("$( ""#dialog-confirm"").attr(""title"", """ & lableTitle & """); ")
                    .AppendLine("$( ""#dialog:dialog-confirm"" ).dialog( ""destroy"" );")
                    .AppendLine("$( ""#dialog-confirm"" ).dialog({")
                    .AppendLine("resizable: false,")
                    .AppendLine("height:300,")
                    .AppendLine("width:500,")
                    .AppendLine("modal: true,")
                    .AppendLine("autoOpen: false,")
                    .AppendLine("buttons: {")
                    .AppendLine("""" & labelYes & """: function(url) {")
                    .AppendLine("$( this ).dialog( ""close"" );")
                    .AppendLine("if (JQRedirectURL!='#'){")
                    .AppendLine(" $(window.location).attr('href', JQRedirectURL);")
                    .AppendLine("}")
                    .AppendLine("return true;")
                    .AppendLine("},")
                    .AppendLine("""" & labelNo & """: function() {")
                    .AppendLine("$( this ).dialog( ""close"" );")
                    .AppendLine("return false;")
                    .AppendLine("}")
                    .AppendLine("}")
                    .AppendLine("});")
                    .AppendLine("});")
                End With
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("AddJQueryDialog", , ex)
            End Try
            Return sbJQuery.ToString
        End Function
        Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

        End Sub

        Protected Overrides Sub InitializeCulture()
            MyBase.InitializeCulture()
            IP.Bids.SharedFunctions.InitCulture()
        End Sub

    End Class
End Namespace