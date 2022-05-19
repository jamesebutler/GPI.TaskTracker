<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DateEntry.ascx.vb" Inherits="IP.Bids.UserControl.DateEntry" %>
<asp:Panel ID="_pnlBorder" runat="server" GroupingText="<%$IPResources:Global,DateRange %>">
    <%--<div style="width: 100%; float: left; margin: 0 auto;">
        <div style="float: left; height: 21px; vertical-align: bottom">--%>
            <asp:Label ID="_lblStartDate" runat="server"></asp:Label><br />
            <asp:TextBox ID="_txtStartDate" Text="" CssClass="DateRange" runat="server" onFocus="javascript:this.blur();ShowCalendar(1);"
                Width="80" AutoCompleteType="None" />
            <asp:Image runat="server" ID="_imgStartCal" OnClientClick="ShowCalendar(1);return false;"
                ImageUrl="~/Images/calendar.gif" ImageAlign="Top" />
                &nbsp;<asp:CheckBox ID="_cbdateCritical" runat="server" Text="Date Critical" CssClass="Warning" />
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
       <%--   </div>
    </div>--%>
</asp:Panel>
