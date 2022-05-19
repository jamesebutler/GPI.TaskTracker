<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTracker.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="_Default" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <asp:Button ID="Button1" runat="server" Text="Button" />
    <IP:ModalIframe ID="_AdvancedSiteSearch" runat="server" DisplayModalButtonText="Advanced Site Search"
        Url="SiteSearch.aspx" Height="600" />
</asp:Content>
