<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AlertBox.ascx.vb" Inherits="User_Controls_AlertBox" %>
<div id="_alertInfo" runat="server" class="alert alert-warning alert-dismissible" role="alert" enableviewstate="false">
    <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
    <h2>
        <asp:Label ID="_lblAlertTitle" ClientIDMode="Static" runat="server"></asp:Label></h2>
    <asp:Label ID="_lblAlertInfo" runat="server" ClientIDMode="Static"></asp:Label>
</div>