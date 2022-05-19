<%@ Page 
    Language="VB" 
    MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" 
    AutoEventWireup="false" 
    ValidateRequest="false"
    CodeFile="ViewTasksSimple.aspx.vb" 
    Inherits="ViewTaskSimple" 
    Title="View Tasks Simple" 
    MaintainScrollPositionOnPostback="false" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<%@ Register Src="User Controls/Tasks/TaskSearchSimple.ascx" TagName="TaskSearchSimple" TagPrefix="IP" %>
<asp:Content ID="_ViewTaskSimple" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <div class="panel panel-default">        
        <IP:TaskSearchSimple ID="_taskSearchSimple" runat="server" SearchMode="View" />
    </div>
</asp:Content>

