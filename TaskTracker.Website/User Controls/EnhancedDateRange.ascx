<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EnhancedDateRange.ascx.vb"
    Inherits="IP.Bids.UserControl.CoolCalendarVB" %>
<asp:Panel ID="_pnlBorder" runat="server" GroupingText="<%$IPResources:Global,DateRange %>">
    <br />
    <div style="width: 100%; float: left; margin: 0 auto;">
        <div style="float: left; height: 21px; vertical-align: bottom">
            <asp:Label ID="_lblRequiredField" Text="*" CssClass="labelerror" runat="server"></asp:Label>&nbsp;<asp:Label ID="_lblStartDate" runat="server"></asp:Label>
            <asp:TextBox ID="_txtStartDate" Text="" CssClass="DateRange" runat="server" onFocus="javascript:this.blur();ShowCalendar(1);"
                Width="80" AutoCompleteType="None" />
            <asp:Image runat="server" ID="_imgStartCal" OnClientClick="ShowCalendar(1);return false;"
                ImageUrl="~/Images/calendar.gif" ImageAlign="Top" />
            <%--   <asp:RequiredFieldValidator ID="_txtStartDateRequired" Enabled="true" runat="server"
                ControlToValidate="_txtStartDate" ErrorMessage="Date is required" Display="Static">*</asp:RequiredFieldValidator>--%>
            <asp:Panel ID="_pnlStartDate" Visible="true" runat="server" CssClass="popupControl"
                Style="display: none;" BorderWidth="1" Width="195px" Height="230" BackColor="LightGray">
                <center>
                    <table border="0">
                        <tr>
                            <td valign="top" align="center" colspan="2">
                                <asp:TextBox ID="_txtStartDateHiddenValue" BackColor="LightGray" Text="" runat="server"
                                    Width="1" Style="visibility: hidden" Visible="true" AutoCompleteType="None" BorderWidth="0"
                                    Height="25" />
                                <asp:DropDownList ID="_ddlStartMonth" runat="Server" AutoPostBack="false" CssClass="calTitle">
                                </asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="_ddlStartYear" runat="Server" AutoPostBack="false" CssClass="calTitle">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <input id="_hdStartCal" type="text" />
                                <ajaxToolkit:CalendarExtender ID="_ceStartCalendar" BehaviorID="ce1" runat="server"
                                    PopupButtonID="_hdStartCal" TargetControlID="_txtStartDateHiddenValue" OnClientHidden="CalendarClosing">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <ajaxToolkit:PopupControlExtender ID="_pceStartDate" runat="server" TargetControlID="_txtStartDate"
                PopupControlID="_pnlStartDate" />
        </div>
        <div style="float: left; width: 50px; text-align: center; height: 21px;">
            <asp:Label ID="_lblTo" runat="server" Text='<%$IPResources:Global,To %>'></asp:Label><asp:TextBox
                ID="_txtTo" runat="server" Width="2" Style="visibility: hidden" CssClass="DateRange"></asp:TextBox></div>
        <div style="float: left;  height: 21px; vertical-align: bottom">
            <asp:Label ID="_lblEndDate" runat="server"></asp:Label>
            <asp:TextBox ID="_txtEndDate" CssClass="DateRange" Text="" runat="server" onFocus="javascript:this.blur();ShowCalendar(2);"
                Width="80" AutoCompleteType="None" /><asp:Image runat="server"
                    ID="CachedImageButton1" OnClientClick="ShowCalendar(2);return false;" ImageUrl="~/Images/calendar.gif"
                    ImageAlign="Top" />
            <asp:Panel ID="_pnlEndDate" Visible="true" runat="server" CssClass="popupControl"
                Style="display: none;" BorderWidth="1" Width="195px" Height="230" BackColor="LightGray">
                <center>
                    <table border="0">
                        <tr>
                            <td valign="top" align="center" colspan="2">
                                <asp:TextBox ID="_txtEndDateHiddenValue" Text="" runat="server" onFocus="javascript:this.blur();"
                                    Width="1" Style="visibility: hidden" Visible="true" AutoCompleteType="None" BorderWidth="0"
                                    Height="25" />
                                <asp:DropDownList ID="_ddlEndMonth" runat="Server" AutoPostBack="false" CssClass="calTitle">
                                </asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="_ddlEndYear" runat="Server" AutoPostBack="false" CssClass="calTitle">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <input id="_hdEndCal" type="text" />
                                <ajaxToolkit:CalendarExtender ID="_ceEndCalendar" BehaviorID="ce2" runat="server"
                                    PopupButtonID="_hdEndCal" TargetControlID="_txtEndDateHiddenValue" OnClientHidden="CalendarClosing"
                                    OnClientShowing="CalendarShowing">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <ajaxToolkit:PopupControlExtender ID="_pceEndDate" runat="server" TargetControlID="_txtEndDate"
                PopupControlID="_pnlEndDate" />
        </div>
        <asp:Panel ID="_pnlDateRange" runat="server" Visible="false">
        
            <div style="float: left; height: 21px; vertical-align: bottom">
                &nbsp;&nbsp;<asp:Label ID="_lblDateRange" runat="server" Text="Date Range"></asp:Label>
                <asp:DropDownList ID="_ddlDateRange" runat="server">
                </asp:DropDownList></div>
        </asp:Panel>
    </div>
</asp:Panel>
