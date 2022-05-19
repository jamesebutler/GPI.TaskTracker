<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UcThemes.ascx.vb" Inherits="UserControlsThemes" %>
<ajaxToolkit:ModalPopupExtender PopupControlID="_pnlThemes" ID="_mpeThemes" runat="server"
    TargetControlID="_lbSwitch" CancelControlID="_btnCancel" OkControlID="_btnCancel"
    DropShadow="true">
</ajaxToolkit:ModalPopupExtender>
<asp:LinkButton ID="_lbSwitch" runat="server" Font-Size="Small" Visible="true" Text="Change Theme" style="display:none;"></asp:LinkButton>
<asp:Panel ID="_pnlThemes" runat="server" BorderWidth="1" BorderColor="black" HorizontalAlign="center"
    Width="400px" Height="200px" BackColor="#CCCCCC" ScrollBars="Vertical">
    <asp:Table ID="Table1" runat="server" Width="100%">
        <asp:TableHeaderRow CssClass="BorderSecondary">
            <asp:TableHeaderCell HorizontalAlign="center">
                <asp:Label ID="_lblSelectHeader" runat="server" Text="Select Desired Theme"></asp:Label></asp:TableHeaderCell>
        </asp:TableHeaderRow>
        <asp:TableRow>
            <asp:TableCell Style="padding-left: 10px;" BorderWidth="0" Width="100%" HorizontalAlign="left">
                <asp:RadioButtonList ID="_rblThemes" Width="100%" AutoPostBack="false" runat="server"
                    RepeatColumns="3" RepeatDirection="horizontal" TextAlign="Right" RepeatLayout="Table">
                </asp:RadioButtonList><br />
                <asp:Button ID="_btnCancel" Text="Cancel" runat="server" CausesValidation="false" />
                <asp:Button ID="_btnChange" Text="Change Theme" runat="server" CausesValidation="false" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
