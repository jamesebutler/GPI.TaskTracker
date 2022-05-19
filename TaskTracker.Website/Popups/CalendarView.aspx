<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerPopup.master" AutoEventWireup="false"
    CodeFile="CalendarView.aspx.vb" Inherits="Popups_CalendarView" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerPopup.master" %>

<%@ Register Src="~/User Controls/JQEventCalendar.ascx" TagName="JQEventCalendar"
    TagPrefix="IP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <asp:Panel ID="_pnlCalendarView" runat="server" ScrollBars="vertical" Visible="true"
        Height="100%" EnableViewState="false">
        <IP:JQEventCalendar ID="_calEvents" runat="server" />
    </asp:Panel>

</asp:Content>
