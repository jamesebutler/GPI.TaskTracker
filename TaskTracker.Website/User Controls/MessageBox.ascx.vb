'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 08-17-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Namespace IP.Bids.UserControl
    Partial Class MessageBox
        Inherits System.Web.UI.UserControl
#Region "Enum"
        Public Enum ButtonType1 As Integer
            OK = 1
            OKCancel = 2
            YesNo = 3
        End Enum
#End Region

#Region "Public Events"
        Public Event OKClick()
        Public Event CancelClick()
#End Region

#Region "Private Events"
        Protected Sub _btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnOK.Click
            Me._mpeMessage.Hide()
            RaiseEvent OKClick()
        End Sub

        Protected Sub _btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnClose.Click
            Me._mpeMessage.Hide()
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me._divMessage.InnerHtml = Message

            DisplayButtons()
            Page.ClientScript.RegisterStartupScript(Me.GetType, "", "var returnValue" & Me.ClientID & "; ", True)
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "CreatePopupJS", CreatePopupJS, True)
        End Sub
#End Region

#Region "Fields"
        Private mCancelScript As String = String.Empty
        Private mOKScript As String = String.Empty
        Private mButtonType As ButtonType1
        Private mMessage As String = String.Empty
        Private mTitle As String = String.Empty
        Private mWidth As Unit = Unit.Pixel(400)
        Private mAllowPostback As Boolean = True
        Private mAllowPrint As Boolean' = False
        Private mHeight As Unit = Unit.Pixel(100)
#End Region

#Region "Properties"
        Public Property AllowPrint() As Boolean
            Get
                Return mAllowPrint
            End Get
            Set(ByVal value As Boolean)
                mAllowPrint = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the javascript that will be executed after the Cancel/No button
        ''' has been clicked
        ''' </summary>
        Public Property CancelScript() As String
            Get
                Return mCancelScript
            End Get
            Set(ByVal value As String)
                mCancelScript = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the javascript that will be executed after the OK button
        ''' </summary>
        ''' <value></value>
        Public Property OKScript() As String
            Get
                Return mOKScript
            End Get
            Set(ByVal value As String)
                mOKScript = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of button set that will be displayed
        ''' </summary>
        ''' <value></value>
        Public Property ButtonType() As ButtonType1
            Get
                Return mButtonType
            End Get
            Set(ByVal value As ButtonType1)
                mButtonType = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the message that will be displayed
        ''' </summary>
        Public Property Message() As String
            Get
                Return mMessage
            End Get
            Set(ByVal value As String)
                mMessage = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the title that will be displayed
        ''' </summary>
        Public Property Title() As String
            Get
                Return mTitle
            End Get
            Set(ByVal value As String)
                mTitle = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the width of the message box
        ''' </summary>
        Public Property Width() As Unit
            Get
                Return mWidth
            End Get
            Set(ByVal value As Unit)
                mWidth = value
            End Set
        End Property

        Public Property Height() As Unit
            Get
                Return mHeight
            End Get
            Set(ByVal value As Unit)
                mHeight = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value indicating whether a postback to the server automatically occurs when the OK button is clicked.
        ''' </summary>
        Public Property AllowPostback() As Boolean
            Get
                Return mAllowPostback
            End Get
            Set(ByVal value As Boolean)
                mAllowPostback = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a handle to the OK button
        ''' </summary>
        Public ReadOnly Property OKClient() As Button
            Get
                Return Me._btnOK
            End Get
        End Property

        ''' <summary>
        ''' Gets a handle to the Cancel button
        ''' </summary>
        Public ReadOnly Property Cancel() As Button
            Get
                Return Me._btnClose
            End Get
        End Property

        ''' <summary>
        ''' Gets the ClientID of the OK button
        ''' </summary>
        Public ReadOnly Property OKClientID() As String
            Get
                Return Me._btnOK.ClientID
            End Get
        End Property

        ''' <summary>
        ''' Gets the ClientID of the Cancel button
        ''' </summary>
        Public ReadOnly Property CancelClientID() As String
            Get
                Return Me._btnClose.ClientID
            End Get
        End Property

        ''' <summary>
        ''' Gets the ClientID of the control that contains the message text
        ''' </summary>
        Public ReadOnly Property MessageClientID() As String
            Get
                Return Me._divMessage.ClientID
            End Get
        End Property

        ''' <summary>
        ''' Gets the ClientID of the control that is used to trigger the display of the message box
        ''' </summary>
        Public ReadOnly Property MessageTriggerClientID() As String
            Get
                Return Me._imbMessageBoxTrigger.ClientID
            End Get
        End Property

        Public ReadOnly Property TriggerPopupJS() As String
            Get
                Return "document.getElementById('" & MessageTriggerClientID & "').click();"
            End Get
        End Property
#End Region

#Region "Public Methods"
        ''' <summary>
        ''' Displays the message box
        ''' </summary>
        Public Sub ShowMessage()
            ConfigureMessage()
            Me._mpeMessage.Show()
        End Sub

        Private Function CreatePopupJS() As String
            Dim sb As New StringBuilder
            Dim targetID As String = MessageTriggerClientID
            sb.Append("function DisplayPopup_" & Me.ClientID & "(msg,title,action){var btn=$get('" & targetID & "');  var confirmationTitle=$get('" & Me._bannerTitle.ClientID & "'); if (confirmationTitle!=null){confirmationTitle.innerHTML=title;} var confirmationMsg=$get('" & Me._divMessage.ClientID & "'); if (confirmationMsg!=null){confirmationMsg.innerHTML=msg;} if (btn!=null){btn.click();}}")
            Return sb.ToString
        End Function

        Private Sub ConfigureMessage()
            DisplayButtons()
            Me._divMessage.InnerHtml = Message
            _bannerTitle.InnerText = Title
            Me._pnlMessageBox.Width = Width
            Me._pnlDivMessage.Height = Unit.Pixel(100)
            Me._pnlMessageBox.Attributes.Add("max-height", Height.Value.ToString)
        End Sub

        ''' <summary>
        ''' Hides the message box
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub HideMessage()
            Me._mpeMessage.Hide()
        End Sub

#End Region

#Region "Private Methods"
        ''' <summary>
        ''' Displays the message box buttons based on the ButtonType property
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub DisplayButtons()
            Select Case ButtonType
                Case ButtonType1.OK
                    _mpeMessage.OkControlID = Me._btnClose.ClientID
                    _mpeMessage.CancelControlID = Me._btnClose.ClientID
                    Me._btnClose.Visible = True
                    Me._btnOK.Visible = False
                    Me._btnClose.Text = IP.Bids.SharedFunctions.LocalizeValue("OK", True)
                Case ButtonType1.OKCancel
                    If AllowPostback = True Then
                        _mpeMessage.OkControlID = Me._btnClose.ClientID
                        _mpeMessage.CancelControlID = Me._btnClose.ClientID
                    Else
                        _mpeMessage.OkControlID = Me._btnOK.ClientID
                        _mpeMessage.CancelControlID = Me._btnClose.ClientID
                    End If
                    Me._btnClose.Visible = True
                    Me._btnOK.Visible = True
                    Me._btnOK.Text = IP.Bids.SharedFunctions.LocalizeValue("OK", True)
                    Me._btnClose.Text = IP.Bids.SharedFunctions.LocalizeValue("Cancel", True)
                Case ButtonType1.YesNo
                    If AllowPostback = True Then
                        _mpeMessage.OkControlID = Me._btnClose.ClientID
                        _mpeMessage.CancelControlID = Me._btnClose.ClientID
                    Else
                        _mpeMessage.OkControlID = Me._btnOK.ClientID
                        _mpeMessage.CancelControlID = Me._btnClose.ClientID
                    End If
                    Me._btnClose.Visible = True
                    Me._btnOK.Visible = True
                    Me._btnOK.Text = IP.Bids.SharedFunctions.LocalizeValue("Yes", True)
                    Me._btnClose.Text = IP.Bids.SharedFunctions.LocalizeValue("No", True)
                Case Else
                    Me._btnClose.Visible = False
                    Me._btnOK.Visible = True
                    Me._btnOK.Text = IP.Bids.SharedFunctions.LocalizeValue("OK", True)
            End Select
            '            Dim returnValue As String = "returnValue" & Me.ClientID & "= " ' COMMENTED BY CODEIT.RIGHT
            If CancelScript.Length > 0 Then
                Me._mpeMessage.OnCancelScript = CancelScript
            End If
            If OKScript.Length > 0 Then
                Me._mpeMessage.OnOkScript = OKScript
            End If
        End Sub

#End Region

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If AllowPrint Then
                Me._imgPrintDialog.OnClientClick = "Javascript:PrintDiv('" & Me._divMessage.ClientID & "');return false;"
                Me._imgPrintDialog.Visible = True
            Else
                Me._imgPrintDialog.Visible = False
            End If
            _pnlDivMessage.Width = Width
            Me._pnlMessageBox.Width = Width
            Me._pnlDivMessage.Height = Unit.Pixel(100)
            Me._pnlMessageBox.Attributes.Add("max-height", Height.Value.ToString)
            ConfigureMessage()
        End Sub
    End Class
End Namespace