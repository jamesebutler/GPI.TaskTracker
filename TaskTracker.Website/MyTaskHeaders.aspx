<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false" CodeFile="MyTaskHeaders.aspx.vb" Inherits="MyTaskHeaders" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<%@ Register Src="~/User Controls/JQDateRange.ascx" TagName="JQDatePicker" TagPrefix="IP" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Register Src="~/User Controls/AdvancedEmployeeListDropdown.ascx" TagName="EmployeeList"
    TagPrefix="IP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <asp:UpdatePanel ID="_udpTaskList" runat="server" UpdateMode="always">
        <ContentTemplate>
            <ajaxToolkit:CollapsiblePanelExtender ID="_cpeSearchSelections" runat="Server" TargetControlID="_pnlFilterOptions"
                Collapsed="true" CollapseControlID="_pnlFilterHeader" ExpandControlID="_pnlFilterHeader"
                SuppressPostBack="true" TextLabelID="_lblFilter" CollapsedText="+ Show Search Criteria"
                ExpandedText="- Hide Search Criteria" ScrollContents="false" Enabled="true" />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="_lblHeaderTitle" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="panel panel-default">
                        <div class="panel-heading pointer" id="_pnlFilterHeader">
                            <asp:Label ID="_lblFilter" runat="server" Text="-"></asp:Label>
                        </div>
                        <asp:Panel ID="_pnlFilterOptions" CssClass="panel panel-default" runat="server">
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-sm-6 col-md-4">
                                        <div class="form-group">
                                            <IP:EmployeeList ID="_currentUser" runat="server" UserMode="UsersOnly"
                                                DisplayClearLink="false" AutoPostBack="false"
                                                Employeelabel="<%$IPResources:Global, Created By%>" EnableValidation="false">
                                            </IP:EmployeeList>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-4">
                                        <div class="form-group">
                                            <asp:Label ID="_lblFacility" runat="server" Text="<%$IPResources:Global, Facility/Location%>" Style="vertical-align: bottom">
                                            </asp:Label>
                                            <br />
                                            <asp:DropDownList ID="_ddlFacility" runat="server" Width="75%" SkinID="multiselectfilter" ClientIDMode="Static">
                                            </asp:DropDownList>
                                            <ajaxToolkit:CascadingDropDown ID="_cddlFacility" runat="server" Category="Facility"
                                                LoadingText="[Loading...]" PromptText="    " ServiceMethod="GetFacilityList"
                                                ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlFacility" BehaviorID="ccdFacility"
                                                ContextKey="ALL" UseContextKey="true" SelectedValue="All"></ajaxToolkit:CascadingDropDown>

                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-4">
                                        <div class="form-group">
                                        <asp:Label ID="_lblBusArea" runat="server" Text="<%$IPResources:Global,Business Unit-Area/Department %>"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="_ddlBusArea" runat="server" Width="75%" ClientIDMode="Static">
                                        </asp:DropDownList>
                                        <ajaxToolkit:CascadingDropDown ID="_cddlBusArea" runat="server" Category="BusinessUnitArea" BehaviorID="ccdBusArea"
                                            LoadingText="[Loading...]" PromptText="    " ServiceMethod="GetBusinessUnitAreaList"
                                            ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlBusArea"
                                            ContextKey="" ParentControlID="_ddlFacility" UseContextKey="true"></ajaxToolkit:CascadingDropDown>
                                    </div></div> 
                                </div>
                                <div class="row">
                                    <div class="form-group">
                                    <div class="col-sm-8">
                                        <asp:Label ID="_lblHeaderDate" runat="server" Text="<%$IPResources:Global, Header Date%>"></asp:Label><br />
                                        <IP:JQDatePicker ID="_dtHeaderDate" runat="server" ShowFromDate="true" ShowDateRange="true"
                                            ShowLabelsOnSameLine="true" />
                                    </div>
                                    </div> 
                                </div>
                            </div>
                            <div class="panel-footer">
                                <div class="row">

                                    <div class="form-group">
                                        <div class="hidden-xs col-sm-4"></div>
                                        <div class="col-sm-4">
                                            <asp:LinkButton ID="_btnApplyFilter" runat="server" SkinID="Button" OnClientClick="DisplayBusy();">
                                                <span class="glyphicon glyphicon-search"></span>&nbsp;<asp:Label ID="Label5" runat="server" Text="<%$IPResources:Global,Display Search Results%>"></asp:Label>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="hiddent-xs col-sm-4"></div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="panel panel-primary">
                            <section id="no-more-gridView">
                                <asp:GridView ID="_gvTaskHeaders" runat="server" AutoGenerateColumns="False" Width="100%"
                                    CssClass="tablesorter" EnableViewState="false" ShowHeaderWhenEmpty="true">
                                    <AlternatingRowStyle ForeColor="black" Font-Bold="false" />
                                    <RowStyle ForeColor="black" Font-Bold="false" />
                                    <HeaderStyle Height="30px" ForeColor="black" Font-Underline="true" />
                                    <Columns>
                                        <asp:BoundField DataField="TASKHEADERSEQID" HeaderText="<%$IPResources:Global, Task Header Number%>"
                                            SortExpression="TASKHEADERSEQID" />
                                        <asp:BoundField DataField="SITENAME" HeaderText="<%$IPResources:Global, Facility%>"
                                            SortExpression="SITENAME" />
                                        <asp:TemplateField HeaderText="<%$IPResources:Global, Title%>">
                                            <ItemTemplate>
                                                <a target="_top" href="Javascript:parent.window.location=('<%#String.Format(CultureInfo.CurrentCulture, Page.ResolveUrl("~/TaskHeader.aspx?HeaderNumber={0}"), Eval("TASKHEADERSEQID")) %>');">
                                                    <%#Eval("Title")%>
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BusinessUnitArea" HeaderText="<%$IPResources:Global, Business Unit-Area%>"
                                            SortExpression="BusinessUnitArea" />
                                        <asp:TemplateField HeaderText="<%$IPResources:Global, Start Date%>" SortExpression="STARTDATE">
                                            <ItemTemplate>
                                                <asp:Label ID="Literal2" Text='<%#IP.Bids.SharedFunctions.GetSortableDate(DirectCast(Container.DataItem, TaskHeaderRecord).StartDate)%>'
                                                    runat="server" Style="display: none"></asp:Label>
                                                <asp:Literal ID="Literal1" Text='<%#IP.Bids.SharedFunctions.GetDate(DirectCast(Container.DataItem, TaskHeaderRecord).StartDate)%>'
                                                    runat="server"></asp:Literal>
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$IPResources:Global, End Date%>" SortExpression="ENDDATE">
                                            <ItemTemplate>
                                                <asp:Label ID="Literal2" Text='<%#IP.Bids.SharedFunctions.GetSortableDate(DirectCast(Container.DataItem, TaskHeaderRecord).EndDate)%>'
                                                    runat="server" Style="display: none"></asp:Label>
                                                <asp:Literal ID="Literal1" Text='<%#IP.Bids.SharedFunctions.GetDate(DirectCast(Container.DataItem, TaskHeaderRecord).EndDate)%>'
                                                    runat="server"></asp:Literal>
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </section>
                        </div>
                    </div>

                </div>
            </div>
            <script type="text/javascript">
                function BindEvents() {
                    $(document).ready(function () {
                        var behavior = $find('ccdFacility');
                        if (behavior !== null) {
                            behavior.add_populated(function () {
                                $('#_ddlFacility').selectpicker('refresh')
                            });
                        }
                        behavior = $find('ccdBusArea');
                        if (behavior !== null) {
                            behavior.add_populated(function () {
                                $('#_ddlBusArea').selectpicker('refresh')
                            });
                        }
                        $('.selectpicker').selectpicker();
                    }
                    )
                }
                Sys.Application.add_load(BindEvents);
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

