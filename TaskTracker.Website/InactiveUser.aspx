<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false" CodeFile="InactiveUser.aspx.vb" Inherits="InactiveUser" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
                <asp:Label ID="_lblHeaderTitle" runat="server"></asp:Label>
            </div>
        </div>

        <div class="panel-body">
            <div class="alert alert-danger" role="alert">
                <h1 id="_missingAccount" runat="server">Inactive User</h1>
            </div>
            <div class="row">
                <div class="col-md-8 col-md-offset-2">
                    <asp:GridView ID="_gvRoleList" runat="server" AllowSorting="false" AutoGenerateDeleteButton="false" AutoGenerateEditButton="false"
                        AutoGenerateColumns="False" CellPadding="1" Width="1000px"
                        CssClass="tablesorter {sortlist: [[1,0]]}">
                        <AlternatingRowStyle ForeColor="black" Font-Bold="false" />
                        <RowStyle Wrap="false" ForeColor="black" Font-Bold="false" />
                        <HeaderStyle Height="30px" ForeColor="white" Font-Underline="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="Name" SortExpression="Name" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="_tbName" runat="server" Text="<%#DirectCast(Container.DataItem, SiteUserRole).AssigneeName%>" Width="200px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Role Description" HeaderText="Role Description" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="_tbRoleDescription" runat="server" Text="<%#DirectCast(Container.DataItem, SiteUserRole).RoleDescription%>"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Business Unit" SortExpression="Business Unit" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="_tbBusUnit" runat="server" Text="<%#DirectCast(Container.DataItem, SiteUserRole).BusUnit%>"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Area" SortExpression="Area" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="_tbArea" runat="server" Text="<%#DirectCast(Container.DataItem, SiteUserRole).Area%>"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Line" SortExpression="Line" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="_tbLine" runat="server" Text="<%#DirectCast(Container.DataItem, SiteUserRole).Line%>"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

