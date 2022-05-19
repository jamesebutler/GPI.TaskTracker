<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MessageBox.ascx.vb" Inherits="IP.Bids.UserControl.MessageBox" %>

<asp:Panel ID="_pnlMessageBox" runat="server" Width="500" ScrollBars="None"
    Style="display: none;" CssClass="modalPopup">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%; text-align: left">
        <tr>
            <th colspan="2" class="BorderHeader">
                <div id="_bannerTitle" runat="server">
                </div>
            </th>
        </tr>
        <tr>
            <td>
                <br />
                <br />
            </td>
            <td style="width: 100%; height: 100%; vertical-align: top">
                <br />
                <asp:Panel ID="_pnlDivMessage" runat="server" ScrollBars="auto">
                    <div id="_divMessage" runat="server">
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="height: 42px; text-align: right" colspan="2">
                <asp:Button ID="_btnOK" runat="server" Text="<%$IPResources:Global,OK %>" Width="70"></asp:Button>&nbsp;<asp:Button ID="_btnClose" runat="server" Text="<%$IPResources:Global,Close %>"
                    Width="70"></asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:ImageButton ID="_imgPrintDialog" runat="server" ImageUrl="~/Images/Printer.gif" /></td>
        </tr>
    </table>
</asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="_mpeMessage" runat="server" TargetControlID="_imbMessageBoxTrigger"
    PopupControlID="_pnlMessageBox" BackgroundCssClass="modalBackground" DropShadow="false"
    OkControlID="_btnClose" CancelControlID="_btnClose">
</ajaxToolkit:ModalPopupExtender>
<div visible="false" style="display: none; visibility: hidden">
    <asp:Image ImageUrl="~/Images/blank.gif" runat="server"
        ID="_imbMessageBoxTrigger" Visible="true" />
</div>
