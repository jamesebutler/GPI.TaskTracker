<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false"
    CodeFile="OnlineTraining.aspx.vb" Inherits="HelpOnlineTraining" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
                <asp:Label ID="_lblHeaderTitle" runat="server"></asp:Label>
            </div>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-offset-2 col-xs-12 col-md-8">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <asp:Label ID="Label1" Font-Size="Medium"
                                Text="<%$IPResources:Global,Select the help file%>" runat="server"></asp:Label>
                        </div>
                        <div class="panel-body">

                            <section id="no-more-gridView">
                                <asp:GridView Width="100%" CssClass="table table-striped tablesorter"
                                    ID="_gvDemoList" runat="server" AutoGenerateColumns="False" DataKeyNames="DEMONAME"
                                    EnableViewState="false" AllowSorting="false" ShowFooter="false" EnableSortingAndPagingCallbacks="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$IPResources:Global,Demo Name%>">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="_hypDemoName" runat="server" Text="<%# IP.Bids.SharedFunctions.LocalizeValue(DirectCast(Container.DataItem, IP.MEAS.BO.HelpFiles).DemoName)%>"
                                                    NavigateUrl="<%# DirectCast(Container.DataItem, IP.MEAS.BO.HelpFiles).DemoFileName%>"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$IPResources:Global,Demo Description%>">
                                            <ItemTemplate>
                                                <asp:Label ID="_lblDemoDesc" runat="server" Text="<%# IP.Bids.SharedFunctions.LocalizeValue(DirectCast(Container.DataItem, IP.MEAS.BO.HelpFiles).DemoDescription)%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:HyperLinkField DataTextField="DemoName" HeaderText="<%$IPResources:Global,Demo Name%>"
                        DataNavigateUrlFields="DemoFileName" DataNavigateUrlFormatString="{0}" Target="_blank" />--%>
                                        <%-- <asp:BoundField DataField="DemoDescription" HeaderText="<%$IPResources:Global,Demo Description%>" />--%>
                                        <asp:BoundField Visible="false" DataField="DemoFileName" HeaderText="<%$IPResources:Global,Demo File%>" />
                                    </Columns>
                                </asp:GridView>
                            </section>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
