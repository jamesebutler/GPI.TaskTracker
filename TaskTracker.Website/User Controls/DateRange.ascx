<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DateRange.ascx.vb" Inherits="IP.Bids.UserControl.DateRange" %>
<asp:Panel ID="_pnlBorder" runat="server" GroupingText="<%$IPResources:Global,DateRange %>">
    <br />
    <div style="width: 100%; float: left;margin: 0 auto;">
        <div style="float: left; width: 200px;;height:21px;">
            <asp:Label ID="_lblStartDate" Text='<%$IPResources:Global,StartDate %>' runat="server"></asp:Label>&nbsp;<asp:TextBox
                ID="_txtStartDate" runat="server" CssClass="DateRange"></asp:TextBox>
            <asp:Image runat="server" ID="_imgStartCal" ImageUrl="~/Images/calendar.gif"
                ImageAlign="top" /></div>
        <div style="float: left;width:50px; text-align:center;height:21px;">
            <asp:Label ID="_lblTo" Text='<%$IPResources:Global,To %>'
                runat="server"></asp:Label><asp:Image runat="server" ImageUrl="~/Images/spacer.gif" Height="21px" Width="1px" AlternateText="" ImageAlign="Top" /></div>
        <div style="float: left; width: 200px;height:21px;">
            <asp:Label ID="_lblEndDate" Text='<%$IPResources:Global,EndDate %>' runat="server"></asp:Label>&nbsp;<asp:TextBox
                ID="_txtEndDate" runat="server" CssClass="DateRange"></asp:TextBox>
            <asp:image runat="server" ID="_imgEndCal" ImageUrl="~/Images/calendar.gif" ImageAlign="Top" />
        </div>
        <div style="float: left;;height:21px;">
            <asp:Label ID="_lblDateRange" runat="server" Text="Date Range"></asp:Label>
            <asp:DropDownList ID="_ddlDateRange" runat="server">
            </asp:DropDownList></div>
    </div>
    <%--    <table border="0" cellpadding="0" cellspacing="0" style="text-align: left">
        <tr>
            <td>
            </td>
            <td valign="baseline">
            </td>
            <td style="width: 80px; vertical-align: middle; text-align: center">
            </td>
            <td>
            </td>
            <td>
            </td>
            <td style="width: 140px; vertical-align: middle; text-align: center">
            </td>
            <td>
            </td>
        </tr>
    </table>--%>
    <ajaxToolkit:CalendarExtender ID="_calStartDate" BehaviorID="ce1" runat="server"
        TargetControlID="_txtStartDate" PopupButtonID="_imgStartCal" PopupPosition="TopLeft">
    </ajaxToolkit:CalendarExtender>
    <ajaxToolkit:CalendarExtender ID="_calEndDate" BehaviorID="ce2" runat="server" TargetControlID="_txtEndDate"
        PopupButtonID="_imgEndCal" PopupPosition="TopLeft">
    </ajaxToolkit:CalendarExtender>
</asp:Panel>
