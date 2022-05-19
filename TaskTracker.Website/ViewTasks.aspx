<%@ Page 
    Language="VB" 
    MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" 
    AutoEventWireup="false" 
    ValidateRequest="false"
    CodeFile="ViewTasks.aspx.vb" 
    Inherits="ViewTasks" 
    Title="View Tasks" 
    MaintainScrollPositionOnPostback="false" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<%@ Register Src="User Controls/Tasks/TaskSearch.ascx" TagName="TaskSearch" TagPrefix="IP" %>
<asp:Content ID="_ViewTasks" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <div class="panel panel-default">        
        <IP:TaskSearch ID="_taskSearch" runat="server" SearchMode="View" />
    </div>
</asp:Content>
