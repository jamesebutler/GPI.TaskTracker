<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DowntimeMessage.ascx.vb" Inherits="User_Controls_DowntimeMessage" %>
<div id="_alertInfo" runat="server" class="alert alert-danger alert-dismissible" role="alert" enableviewstate="false">
    <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
    <h1>
       <asp:Label ID="_lblAlertInfo" runat="server" SkinID="none" CssClass="SiteOutage"  ClientIDMode="Static"></asp:Label></h1>
    
</div>