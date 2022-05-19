<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RecurrencePattern.ascx.vb"
    Inherits="UserControlsRecurrencePattern" %>
<asp:Panel ID="_pnlRecurrencePattern" runat="server">
    <table width="100%" cellpadding="2" cellspacing="2">
        <tr>
            <td style="width: 150px; vertical-align: top;">
                <asp:RadioButtonList ID="_rblFrequency" AutoPostBack="true" runat="server">
                </asp:RadioButtonList>
            </td>
            <td style="vertical-align: bottom">
                <asp:Panel ID="_pnlDailyFrequency" runat="server" GroupingText="Daily" Width="98%">
                    <table>
                        <tr>
                            <td>
                                Every
                                <asp:TextBox ID="_txtEveryDay" runat="server" Width="50"></asp:TextBox>&nbsp;day(s)
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="_pnlWeeklyFrequency" runat="server" GroupingText="Weekly" Width="98%">
                    <table width="100%">
                        <tr>
                            <td>
                                Every
                                <asp:TextBox ID="_txtEveryWeek" runat="server" Width="50"></asp:TextBox>&nbsp;week(s)
                                on
                                <br />
                                <IP:ExtendedCheckBoxList ID="_cblDaysOfWeek" runat="server" RepeatColumns="4" RepeatDirection="horizontal"
                                    Width="100%">
                                </IP:ExtendedCheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="_pnlMonthlyFrequency" runat="server" GroupingText="Monthly" Width="98%">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="_rbDayOfMonth" runat="server" GroupName="MonthlyFrequency" Text="Day" />&nbsp;<asp:TextBox
                                    ID="_txtDayOfMonth" Width="50" runat="server"></asp:TextBox>&nbsp;of every&nbsp;<asp:TextBox
                                        ID="_txtMonthOfYear" runat="server" Width="50"></asp:TextBox>
                                &nbsp;month(s)<br />
                                <asp:RadioButton ID="_rbWeekDayOfMonth" runat="server" GroupName="MonthlyFrequency"
                                    Text="The" />&nbsp;<asp:DropDownList ID="_ddlDayOfMonth" runat="server">
                                        <asp:ListItem Text="First"></asp:ListItem>
                                        <asp:ListItem Text="Second"></asp:ListItem>
                                        <asp:ListItem Text="Third"></asp:ListItem>
                                        <asp:ListItem Text="Fourth"></asp:ListItem>
                                        <asp:ListItem Text="Last"></asp:ListItem>
                                    </asp:DropDownList>
                                &nbsp;
                                <asp:DropDownList ID="_ddlMonthDayOfWeek" runat="server">
                                </asp:DropDownList>&nbsp;of every&nbsp;<asp:TextBox ID="TextBox1" runat="server"
                                    Width="50"></asp:TextBox>
                                &nbsp;month(s)<br />
                                <asp:RadioButton ID="RadioButton7" runat="server" GroupName="MonthlyFrequency" Text="Last Day of Month" />&nbsp;of
                                every&nbsp;<asp:TextBox ID="TextBox5" runat="server" Width="50"></asp:TextBox>&nbsp;month(s)<br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="_pnlQuarterly" runat="server" GroupingText="Quarterly" Width="98%">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="_rblDayOfQtr" runat="server" GroupName="QuarterlyFrequency"
                                    Text="Day" />&nbsp;<asp:TextBox ID="_txtDayOfQtr" Width="50" runat="server"></asp:TextBox>&nbsp;of
                                every quarter&nbsp;<br />
                                <asp:RadioButton ID="_rblWeekOfQtr" runat="server" GroupName="QuarterlyFrequency"
                                    Text="The" />&nbsp;<asp:DropDownList ID="_ddlDayOfQtr" runat="server">
                                        <asp:ListItem Text="First"></asp:ListItem>
                                        <asp:ListItem Text="Second"></asp:ListItem>
                                        <asp:ListItem Text="Third"></asp:ListItem>
                                        <asp:ListItem Text="Fourth"></asp:ListItem>
                                        <asp:ListItem Text="Last"></asp:ListItem>
                                    </asp:DropDownList>
                                &nbsp;
                                <asp:DropDownList ID="_ddlQtrDayOfWeek" runat="server">
                                </asp:DropDownList>&nbsp;of every quarter<br />
                                <asp:RadioButton ID="_rblLastDayOfQtr" runat="server" GroupName="MonthlyFrequency"
                                    Text="Last Day of Quarter" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%--  <asp:Panel ID="_pnlSemiAnnual" runat="server" GroupingText="Semi-Annual" Width="98%">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButton ID="RadioButton3" runat="server" GroupName="MonthlyFrequency" Text="Day" />&nbsp;<asp:TextBox
                                    ID="TextBox3" Width="50" runat="server"></asp:TextBox>&nbsp;of every 6 Months&nbsp;<br />
                                <asp:RadioButton ID="RadioButton4" runat="server" GroupName="MonthlyFrequency" Text="The" />&nbsp;<asp:DropDownList
                                    ID="DropDownList3" runat="server">
                                    <asp:ListItem Text="First"></asp:ListItem>
                                    <asp:ListItem Text="Second"></asp:ListItem>
                                    <asp:ListItem Text="Third"></asp:ListItem>
                                    <asp:ListItem Text="Fourth"></asp:ListItem>
                                    <asp:ListItem Text="Last"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;
                                <asp:DropDownList ID="_ddlSemiDayOfWeek" runat="server">
                                </asp:DropDownList>&nbsp;of every 6 Months<br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>--%>
                <asp:Panel ID="_pnlYearly" runat="server" GroupingText="Yearly" Width="98%">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:RadioButton ID="RadioButton5" runat="server" GroupName="MonthlyFrequency" Text="Day" />&nbsp;<asp:TextBox
                                    ID="TextBox4" Width="50" runat="server"></asp:TextBox>&nbsp;of&nbsp;<asp:DropDownList
                                        ID="_ddlMonths" runat="server">
                                    </asp:DropDownList>
                                &nbsp;of every&nbsp;<asp:TextBox ID="TextBox7" runat="server" Width="50"></asp:TextBox>
                                &nbsp;year(s)<br />
                                <br />
                                <asp:RadioButton ID="RadioButton6" runat="server" GroupName="MonthlyFrequency" Text="The" />&nbsp;<asp:DropDownList
                                    ID="DropDownList5" runat="server">
                                    <asp:ListItem Text="First"></asp:ListItem>
                                    <asp:ListItem Text="Second"></asp:ListItem>
                                    <asp:ListItem Text="Third"></asp:ListItem>
                                    <asp:ListItem Text="Fourth"></asp:ListItem>
                                    <asp:ListItem Text="Last"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;
                                <asp:DropDownList ID="_ddlYearlyDayOfWeek" runat="server">
                                </asp:DropDownList>&nbsp;of
                                <asp:DropDownList ID="_ddlMonthsOfYear" runat="server">
                                </asp:DropDownList>
                                &nbsp;of every&nbsp;<asp:TextBox ID="TextBox6" runat="server" Width="50"></asp:TextBox>
                                &nbsp;year(s)<br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <asp:Panel ID="_pnlRangeOfRecurrence" runat="server" GroupingText="Range of recurrence"
                    Width="98%">
                    <table>
                        <%-- <tr>
                                    <td>
                                        Start of Recurrence</td>
                                    <td colspan="3">
                                        <IP:JQDatePicker ShowDateRange="false" ShowLabelsOnSameLine="true" ShowFromDate="false"
                                            ID="_dtStartOfRecurrence" runat="server" FromLabel="">
                                        </IP:JQDatePicker>
                                    </td>
                                </tr>--%>
                        <tr>
                            <td colspan="3">
                                <asp:RadioButton ID="_rbNoEndDate" runat="server" Text="No End Date" GroupName="RangeOfRecurrence" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton ID="_rbEndAfter" runat="server" Text="End After" GroupName="RangeOfRecurrence" /></td>
                            <td>
                                <asp:TextBox ID="_txtEndAfter" runat="server" Width="80"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="_lblEndAfterOccurrences" Text="Occurrences" runat="server" SkinID="None"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton ID="_rbEndBy" runat="server" Text="End By" GroupName="RangeOfRecurrence" /></td>
                            <td colspan="2">
                                <IP:JQDatePicker ShowDateRange="false" ShowLabelsOnSameLine="true" ShowFromDate="false"
                                    ID="_dtEndbyDate" runat="server" FromLabel="">
                                </IP:JQDatePicker>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="_pnlRecurringTasks" runat="server">
    <asp:DataList ID="_dlRecurringTasks" runat="server" Width="100%" CellSpacing="2"
        RepeatLayout="table" RepeatDirection="horizontal" RepeatColumns="5" EnableViewState="true">
        <ItemTemplate>
            <asp:HyperLink ID="_hypRecurringTasks" runat="server" BorderWidth="0" EnableViewState="true"
                NavigateUrl='<%# DirectCast(Container.DataItem, System.Data.DataRowView)("URL") %>'
                Text='<%# DirectCast(Container.DataItem, System.Data.DataRowView)("taskdate") %>'></asp:HyperLink>
        </ItemTemplate>
    </asp:DataList>
    <asp:Label ID="_lblNoRecurringTasks" runat="server" Text="None" Visible="false"></asp:Label>
</asp:Panel>
