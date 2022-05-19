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

Partial Class UserControlsUCSearch
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Page.ClientScript.IsStartupScriptRegistered("IPSearchbox") = False Then
            'Page.ClientScript.RegisterStartupScript(GetType(Page), "IPSearchbox", "function IPSearchbox(id){var url='http://myip.ipaper.com/myip-asp/common/searchResults.asp?ct=ipaper,ipaperNotes&q1='+id;var win=dhtmlmodal.open('IPSearchbox', 'iframe', url, 'Search Results', 'width=450px,height=400px,resize=1,scrolling=1,center=1', 'recal');}", True)
            Page.ClientScript.RegisterStartupScript(GetType(Page), "IPSearchbox", "function IPSearchbox(id){window.open('http://myip.ipaper.com/myip-asp/common/searchResults.asp?ct=ipaper,ipaperNotes&q1='+id,null,'height=600, width=600,status= no, resizable=yes, scrollbars=yes, toolbar=no,location=no,menubar=no ');}", True)
        End If
        Me._btnSearchSubmit.OnClientClick = "IPSearchbox(document.forms(0)." & Me._txtIPSearch.ClientID & ".value);return false;"
    End Sub
End Class
