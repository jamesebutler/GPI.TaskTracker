<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerPopup.master" AutoEventWireup="false"
    CodeFile="TaskList.aspx.vb" Inherits="PopupsTaskList" %>
<%@ Register Src="~/User Controls/Tasks/TaskHeader.ascx" TagName="TaskHeader" TagPrefix="IP" %>
<%@ Register Src="~/User Controls/Tasks/TaskItems.ascx" TagName="TaskItems" TagPrefix="IP" %>
<%@ Register Src="~/User Controls/Tasks/MultiTaskEdit.ascx" TagName="MultiTaskEdit"
    TagPrefix="IP" %>
<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerPopup.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
        <Asp:UpdatePanel ID="_udpTaskList" runat="server" UpdateMode="always">
            <ContentTemplate>
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
                <div>
                    <IP:TaskHeader ID="_taskHeader" runat="server" />
                    <IP:TaskItems ID="_taskItems" runat="server" InFrame="true" AllowTasksToBeFiltered="true" />
                    <IP:MultiTaskEdit ID="_taskEdit" runat="server" />
                </div>
            </ContentTemplate>
        </Asp:UpdatePanel>
   </asp:Content>