<%@ Control Language="VB" AutoEventWireup="false" CodeFile="NotificationFrequency.ascx.vb"
    Inherits="UserControlsNotificationFrequency" %>
<asp:Panel ID="_pnlNotificationFrequency" runat="server" Width="98%" HorizontalAlign="left" style="min-height:300px">
    <%--  <br />--%>
    <div class="panel-body">
        <div class="row">
            <div class="col-xs-12">
                <div class="form-group">
                    <asp:Label ID="_lblOptOut" runat="server" Text="<%$IPResources:Global, Opt Out of Email Notification%>"></asp:Label>
                    <asp:Literal ID="_litOptOut" runat="server" Text="<br />"></asp:Literal>
                    <asp:CheckBox ID="_cbOptOut" runat="server" Font-Bold="true" ForeColor="red" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <asp:Label ID="_lblWhenEntered" runat="server"></asp:Label>
                <div class="form-group">
                    <asp:RadioButtonList ID="_rblImmediate" runat="server" RepeatDirection="horizontal" RepeatColumns="2" Width="250px">
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <asp:Label ID="_lblNotificationHeading" runat="server"></asp:Label><br />

            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                <div class="form-group">
                    <asp:RadioButton ID="_rbEveryDay" runat="server" />
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <asp:RadioButton ID="_rbEveryWeek"
                        runat="server" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <asp:DropDownList ID="_ddlDayOfWeek" runat="server" Style="min-width: 50px">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <div class="form-group">
                    <asp:RadioButton ID="_rblEveryMonth" runat="server" />
                </div>
            </div>
            <div class="col-md-2">
                <div class="form-group">
                    <asp:DropDownList
                        ID="_ddlOrdinalMonth" runat="server" Style="min-width: 50px">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
                <div class="form-group">
                    <asp:Label ID="_lblFutureTimePeriodHeader" runat="server"></asp:Label><br />
                    <asp:RadioButtonList ID="_rblFutureNotificationPeriod" runat="server" RepeatDirection="Horizontal" Width="100%">
                </asp:RadioButtonList>
            </div>
        </div>
    </div>
    
</asp:Panel>
