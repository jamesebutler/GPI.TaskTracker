<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UcSearch.ascx.vb" Inherits="UserControlsUCSearch" %>
<!-- search box -->
<div style="width: 155; font-family: Arial; font-size: 11px; height: 40">
    <input type="text" id="_txtIPSearch" name="_txtIPSearch" runat="server" maxlength="30"
        style="font-size: 10; font-family: arial; line-height: 10px;" />&nbsp;&nbsp;<asp:Button
            ID="_btnSearchSubmit" runat="server" Text="<%$IPResources:Global,Search %>" />
</div>
<!-- search box END --> 
