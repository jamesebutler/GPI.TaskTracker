<%@ Control Language="VB" AutoEventWireup="false" CodeFile="JQDateRange.ascx.vb"
    Inherits="UserControlsJQDateRange" ClassName="UserControlsJQDateRange" %>

<%--<div class="row">
    <div class="col-sm-12">--%>
        <div class="form-inline">
            <div class="form-group">
                <asp:TextBox ID="_txtDateFrom" runat="server"></asp:TextBox>
                <span style="white-space: nowrap; display: inline-block">
                    <asp:CheckBox
                        ID="_cbdateCritical" runat="server" Text="<%$IPResources:Global, Critical%>" CssClass="Warning" /></span>
            </div>
            <div class="form-group hidden-xs">
                <asp:Label ID="_lblDateTo" runat="server" Text="-"></asp:Label>
            </div>
            <div class="form-group">
                <asp:Literal ID="_lineBreakTo" runat="server" Text="<br />"></asp:Literal>
                <asp:TextBox ID="_txtDateTo" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="_lblDateRange" runat="server" Text="<%$IPResources:Global,Date Range %>"></asp:Label>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="_ddlDateRange" runat="server">
                </asp:DropDownList>

            </div>
      <%--  </div>
    </div>--%>

</div>
