<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerPopup.master" AutoEventWireup="false"
    CodeFile="Attachments.aspx.vb" Inherits="PopupsAttachments" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerPopup.master" %>

<%@ Register Src="~/User Controls/FileAttachments.ascx" TagName="FileAttachments"
    TagPrefix="IP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <IP:FileAttachments ID="_fa" runat="server" />
</asp:Content>
