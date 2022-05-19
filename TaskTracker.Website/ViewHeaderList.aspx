<%@ Page 
    Language="VB" 
    MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" 
    AutoEventWireup="false" 
    ValidateRequest="false"
    CodeFile="ViewHeaderList.aspx.vb" 
    Inherits="ViewHeaderList" 
    Title="View Header List" 
    MaintainScrollPositionOnPostback="false" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<%@ Register Src="User Controls/Tasks/HeaderList.ascx" TagName="HeaderList" TagPrefix="IP" %>
<asp:Content ID="_ViewHeaderList" ContentPlaceHolderID="_cphMainContent" runat="Server">

    <div class="panel panel-default">        
        <IP:HeaderList ID="_headerList" runat="server" SearchMode="View" />
    </div>

</asp:Content>

