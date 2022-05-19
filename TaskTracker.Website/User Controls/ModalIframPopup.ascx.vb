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

Partial Class ModalIframePopup
    Inherits System.Web.UI.UserControl
    Private _Url As String = String.Empty
    Private _Height As Unit
    Private _Width As Unit
    Private _ScreenWidth As Integer = 640
    Private _ScreenHeight As Integer = 800
    Private _BannerText As String = String.Empty
    Private _ReloadPageOnClose As Boolean
    Private _AllowChildToCloseParent As Boolean
    Private _CloseMeQueryString As String = "CloseMe"

    Public Property GlyphiconValue As String = String.Empty
    Public Property AllowChildToCloseParent() As Boolean
        Get
            Return _AllowChildToCloseParent
        End Get
        Set(ByVal value As Boolean)
            _AllowChildToCloseParent = value
        End Set
    End Property

    Public Property BannerText() As String
        Get
            Return _BannerText
        End Get
        Set(ByVal value As String)
            _BannerText = value
        End Set
    End Property
    Public ReadOnly Property CloseMeQueryString() As String
        Get
            Return _CloseMeQueryString
        End Get
    End Property
    Public Property ScreenWidth() As Integer
        Get
            Return _ScreenWidth
        End Get
        Set(ByVal value As Integer)
            _ScreenWidth = value
        End Set
    End Property

    Public Property ScreenHeight() As Integer
        Get
            Return _ScreenHeight
        End Get
        Set(ByVal value As Integer)
            _ScreenHeight = value
        End Set
    End Property

    Public Property ButtonWidth() As Unit
        Get
            Return _lnkDisplayModal.Width
        End Get
        Set(ByVal value As Unit)
            _lnkDisplayModal.Width = value
        End Set
    End Property

    Public Property ButtonHeight() As Unit
        Get
            Return _lnkDisplayModal.Height
        End Get
        Set(ByVal value As Unit)
            _lnkDisplayModal.Height = value
        End Set
    End Property
    Public Property ReloadPageOnClose() As Boolean
        Get
            Return _ReloadPageOnClose
        End Get
        Set(ByVal value As Boolean)
            _ReloadPageOnClose = value
        End Set
    End Property
    Public ReadOnly Property CloseButtonID() As String
        Get
            Return _btnClose.ClientID
        End Get
    End Property
    Public ReadOnly Property CancelButtonID() As String
        Get
            Return _btnCancel.ClientID
        End Get
    End Property
    'Public ReadOnly Property TriggerPopupJS() As String
    '    Get
    '        Return "document.getElementById('" & _lnkDisplayModal.ClientID & "').click();"
    '    End Get
    'End Property
    Public ReadOnly Property ReturnValueClientID() As String
        Get
            Return Me._txtReturnValue.ClientID
        End Get
    End Property
    Public Property Width() As Unit
        Get
            Return _Width
        End Get
        Set(ByVal value As Unit)
            _Width = value
        End Set
    End Property
    Public Property Enabled() As Boolean
        Get
            Return Me._lnkDisplayModal.Enabled
        End Get
        Set(ByVal value As Boolean)
            Me._lnkDisplayModal.Enabled = value
        End Set
    End Property
    Public Property Height() As Unit
        Get
            Return _Height
        End Get
        Set(ByVal value As Unit)
            _Height = value
        End Set
    End Property
    Public Property Url() As String
        Get
            Return _Url
        End Get
        Set(ByVal value As String)
            _Url = value
        End Set
    End Property
    Public Property DisplayModalButtonText() As String
        Get
            Return Me._lnkDisplayModal.Text
        End Get
        Set(ByVal value As String)
            Me._lnkDisplayModal.Text = value
        End Set
    End Property

    Public Sub ShowPopup()
        SizePopup()
        If ReloadPageOnClose = True Then
            Me._btnClose.OnClientClick = "ReloadIframe_" & Me.ClientID & "('#');window.location.reload();return false;"
            'Me._imgClose.OnClientClick = "ReloadIframe_" & Me.ClientID & "('#');window.location.reload();return false;"
        Else
            Me._btnClose.OnClientClick = "" '"ReloadIframe_" & Me.ClientID & "('#');return false"
            'Me._imgClose.OnClientClick = ""
        End If
        Me._btnCancel.OnClientClick = ""

        Me.LoadIFrame()
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "TriggerPopup" & Me.ID, TriggerPopupJS(), True)
        ' Me._mpeDisplayModal.Show()
    End Sub
    Public Function CreateIframeJS() As String
        Dim sb As New StringBuilder
        Dim iFrame As String = Me._ifrModalPopup.ClientID
        sb.Append("function ReloadIframe_" & Me.ClientID & "(url){ var iframe=$get('" & iFrame & "');if (iframe!=null){iframe.src=url;$('#" & _ifrModalPopup.ClientID & "').load(function(){$('#" & _ifrModalPopup.ClientID & "').css('display', 'block'); $('#" & preload_img_modal.ClientID & "').css('display', 'none');});}}")
        Return sb.ToString
    End Function
    Public Function TriggerPopupJS() As String
        ' ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "TriggerPopup" & Me.ID, "DisplayPopup_" & Me.ClientID & "();", True)
        Return "document.getElementById('" & _lnkDisplayModal.ClientID & "').click();"
    End Function
    Private Function TriggerClosePopupJS() As String
        'Return "document.getElementById('" & Me._imgClose.ClientID & "').click();"
        'Return "$find('" & Me._mpeDisplayModal.BehaviorID & "').hide();"

        Return "$('#myModal').modal('show');"
    End Function
    Public Function TriggerPopupJS(ByVal url As String) As String
        If Me.AllowChildToCloseParent Then
            If Me._Url.Contains("?") Then
                _Url = _Url & String.Format("&CloseMe={0}&CancelMe={1}", Me.CloseButtonID, Me.CancelButtonID)
            Else
                _Url = _Url & String.Format("?CloseMe={0}&CancelMe={1}", Me.CloseButtonID, Me.CancelButtonID)
            End If
        End If
        Return "document.getElementById('" & _lnkDisplayModal.ClientID & "').click();" & "ReloadIframe_" & Me.ClientID & "('" & url & "');"
        'Return "document.getElementById('" & _lnkDisplayModal.ClientID & "').click();" & "ReloadIframe_" & Me.ClientID & "('" & url & "');return false;"
    End Function
    Private Function CreatePopupJS() As String
        Dim sb As New StringBuilder
        Dim targetID As String = Me._lnkDisplayModal.ClientID
        sb.Append("function DisplayPopup_" & Me.ClientID & "(){$('#" & targetID & "').modal('show')}") '"(){var btn=$get('" & targetID & "'); if (btn!=null){btn.click();}}")
        Return sb.ToString
    End Function
    Public Sub HideDisplayButton()
        Me._lnkDisplayModal.Attributes.Add("display", "none")
        Me._lnkDisplayModal.Style.Item("display") = "none"
    End Sub
    Public Sub LoadIFrame()
        _ifrModalPopup.Attributes.Item("src") = Page.ResolveUrl(Url)
        _ifrModalPopup.Attributes.Item("onload") = "$('#" & _ifrModalPopup.ClientID & "').load(function() {$('#" & _ifrModalPopup.ClientID & "').css('display', 'block'); $('#" & preload_img_modal.ClientID & "').css('display', 'none');resizeFrame(document.getElementById('" & Me._ifrModalPopup.ClientID & "'));});"
        '_ifrModalPopup.Attributes.Item("onload") = "$('#" & preload_img_modal.ClientID & "').css('display', 'block');$('#" & _ifrModalPopup.ClientID & "').load(function() {$('#" & _ifrModalPopup.ClientID & "').css('display', 'block'); $('#" & preload_img_modal.ClientID & "').css('display', 'none');resizeFrame(document.getElementById('" & Me._ifrModalPopup.ClientID & "'));});"
        HandleLoad()
    End Sub

    Public Sub HandleLoad()
        IP.Bids.SharedFunctions.DisablePageCache(Response)
        If Me._Url.Length = 0 Then
            Me._Url = Page.ResolveClientUrl("~/blank.htm")
        Else
            If Me.AllowChildToCloseParent Then
                If Me._Url.Contains("?") Then
                    _Url = _Url & String.Format("&CloseMe={0}&CancelMe={1}", Me.CloseButtonID, Me.CancelButtonID)
                Else
                    _Url = _Url & String.Format("?CloseMe={0}&CancelMe={1}", Me.CloseButtonID, Me.CancelButtonID)
                End If
            End If
        End If
        If Not Page.IsPostBack Then
            'Me._ifrModalPopup.Attributes.Item("src") = _Url
        End If
        'Me._Url = Page.ResolveClientUrl("~/blank.htm")
        preload_img_modal.ID = "preload_img_modal_" & Me.ClientID
        _lnkDisplayModal.Attributes.Add("data-target", "#" & Me._modalIframe.ClientID)
        '_mpeDisplayModal.BehaviorID = "_bhidDisplayModal_" & Me.ClientID
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "ReloadIframe_" & Me.ClientID, CreateIframeJS, True)
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "PopupJS_" & Me.ID, CreatePopupJS, True)
        Me._lnkDisplayModal.OnClientClick = "ReloadIframe_" & Me.ClientID & "('" & Url & "');return false;" 'IIf(Me._lnkDisplayModal.OnClientClick.Length > 0, Me._lnkDisplayModal.OnClientClick & ";", "") & "ReloadIframe_" & Me.ClientID & "('" & Url & "');"
        If ReloadPageOnClose = True Then
            Me._btnClose.OnClientClick = "ReloadIframe_" & Me.ClientID & "('#');try{window.location.reload();} catch(e){alert(e);}return false;"
            'Me._imgClose.OnClientClick = "ReloadIframe_" & Me.ClientID & "('#');try{window.location.reload();} catch(e){alert(e);}return false;"
            Me._btnCancel.OnClientClick = "ReloadIframe_" & Me.ClientID & "('#');return false;"
        Else
            Me._btnClose.OnClientClick = "ReloadIframe_" & Me.ClientID & "('" & Page.ResolveClientUrl("~/blank.htm") & "');" & TriggerClosePopupJS() & "return false;"
            'Me._imgClose.OnClientClick = "ReloadIframe_" & Me.ClientID & "('" & Page.ResolveClientUrl("~/blank.htm") & "');return false;"
            Me._btnCancel.OnClientClick = "ReloadIframe_" & Me.ClientID & "('" & Page.ResolveClientUrl("~/blank.htm") & "');return false;"
        End If

        SizePopup()
        'Me._RCE.MinimumHeight = newHeight
        'Me._RCE.MinimumWidth = newWidth

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HandleLoad()
    End Sub

    Private Sub SizePopup()
        Dim newHeight As Integer = Height.Value
        Dim newWidth As Integer = Width.Value
        If newHeight = 0 Then
            newHeight = 400
        End If
        If newWidth = 0 Then
            newWidth = 400
        End If
        If Height.Type = UnitType.Percentage And Height.Value > 0 Then
            If Height.Value > 94 Then
                Height = Unit.Percentage(94)
            End If
            'Me._pnlModalPopup.Height = Height
            'newHeight = ScreenHeight * Height.Value / 100
        Else
            'Me._pnlModalPopup.Height = newHeight
        End If

        If Width.Type = UnitType.Percentage And Width.Value > 0 Then
            'newWidth = ScreenWidth * Width.Value / 100
            'Me._pnlModalPopup.Width = Width
        Else
            'Me._pnlModalPopup.Width = newWidth
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me._lblHeaderText.Text = Me.BannerText
        If GlyphiconValue.Length > 0 Then
            _lnkDisplayModal.Controls.Add(New LiteralControl(String.Format("<span aria-hidden='true' class='{0}'></span>&nbsp;<b>{1}</b>", GlyphiconValue, Me.DisplayModalButtonText)))
        End If
    End Sub

    'Protected Sub _btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnClose.Click
    '    Me._mpeDisplayModal.Hide()
    'End Sub

    'Protected Sub _imgClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles _imgClose.Click
    '    Me._mpeDisplayModal.Hide()
    'End Sub

    'Protected Sub _btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnCancel.Click
    '    Me._mpeDisplayModal.Hide()
    'End Sub
End Class
