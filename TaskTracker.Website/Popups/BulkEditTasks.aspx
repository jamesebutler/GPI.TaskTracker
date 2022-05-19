<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerPopup.master" AutoEventWireup="false"
    CodeFile="BulkEditTasks.aspx.vb" Inherits="Popups.BulkEditTasks" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerPopup.master" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Register Namespace="RealWorld.Grids" TagPrefix="RWG" %>
<%@ Register Src="~/User Controls/Tasks/MultiTaskEdit.ascx" TagName="MultiTaskEdit"
    TagPrefix="IP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <IP:MultiTaskEdit ID="_taskEdit" runat="server" AllowTasksToBeFiltered="false" />

    <%--<Asp:UpdatePanel ID="_udpBusy" runat="server" UpdateMode="always">
        <ContentTemplate>--%>
    <ajaxToolkit:ModalPopupExtender ID="_mpeBusy" runat="server" TargetControlID="_imbBusy"
        PopupControlID="_pnlBusy" BackgroundCssClass="modalBackground" DropShadow="false"
        OkControlID="_btnCloseBusy" CancelControlID="_btnCloseBusy">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Button Visible="false" ID="_btnBusy" runat="server" />
    <asp:Panel ID="_pnlBusy" runat="server" Width="0" Height="0" Style="display: none;">
        <div visible="false" style="display: none; visibility: hidden">
            <asp:Button ID="_btnCloseBusy" runat="server" Text="Cancel" />
        </div>
    </asp:Panel>
    <div visible="false" style="display: none; visibility: hidden">
        <asp:ImageButton ImageUrl="~/Images/blank.gif" runat="server" ID="_imbBusy" Visible="true" />
    </div>
    <IP:MessageBox ID="_AlertMessage" runat="server" ButtonType="OK" />
    <div id="dialog-confirm" title="Please Confirm" style="display: none;">
        <p style="text-align: left;">
            <asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,LeavePageConfirmation? %>"></asp:Label>
        </p>
    </div>
    <IP:MessageBox ID="_msgToUser" runat="server" AllowPostback="false" ButtonType="oK"
        Message="" Title="<%$IPResources:Global,Task Tracker %>" />
</asp:Content>


