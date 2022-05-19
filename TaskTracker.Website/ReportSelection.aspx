<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false"
    CodeFile="ReportSelection.aspx.vb" Inherits="ReportSelection" Title="Report Selection"
    ValidateRequest="false" EnableViewState ="true" Trace="false" TraceMode="SortByCategory"%>

<%@ Register Src="User Controls/Tasks/ReportSelector.ascx" TagName="TaskSearch" TagPrefix="IP" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <div class="panel panel-default">
        <IP:TaskSearch ID="_taskSearch" runat="server" SearchMode="Reporting" Visible="true" />
    </div>
</asp:Content>
