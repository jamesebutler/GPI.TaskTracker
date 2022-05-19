<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UserTaskList.aspx.vb" Inherits="PopupsUserTaskList" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Register Src="~/User Controls/AdvancedEmployeeListDropdown.ascx" TagName="EmployeeList"
    TagPrefix="IP" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Untitled Page</title>
    <link href="~/Content/Reliability.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="_scriptManager" AsyncPostBackTimeout="10000"
            EnablePageMethods="true" LoadScriptsBeforeUI="true" ScriptMode="release" runat="server"
            EnablePartialRendering="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="_udpTaskList" runat="server" UpdateMode="always">
            <ContentTemplate>
                <ajaxToolkit:ModalPopupExtender ID="_mpeBusy" runat="server" TargetControlID="ctl00__imbBusy"
                    PopupControlID="_pnlBusy" BackgroundCssClass="modalBackground" DropShadow="false"
                    OkControlID="_btnCloseBusy" CancelControlID="_btnCloseBusy">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Button Visible="false" ID="_btnBusy" runat="server" />
                <asp:Panel ID="_pnlBusy" runat="server" Width="0" Height="0" Style="display: none;">
                    <div visible="false" style="display: none; visibility: hidden">
                        <asp:Button ID="_btnCloseBusy" runat="server" Text="Cancel" />
                    </div>
                </asp:Panel>
                <div visible="false" style="display: none; visibility: hidden">
                    <asp:ImageButton ImageUrl="~/Images/blank.gif" runat="server" ID="ctl00__imbBusy" ClientIDMode="Static" Visible="true" />
                </div>
                <ajaxToolkit:CollapsiblePanelExtender ID="_cpeSearchSelections" runat="Server" TargetControlID="_pnlFilterOptions"
                    Collapsed="true" CollapseControlID="_pnlFilterHeader" ExpandControlID="_pnlFilterHeader"
                    SuppressPostBack="true" TextLabelID="_lblFilter" CollapsedText="+ Show Search Criteria"
                    ExpandedText="- Hide Search Criteria" ScrollContents="false" Enabled="true" />
                <div class="container-fluid">
                    <div class="panel">
                        <div class="panel-header" id="_pnlFilterHeader">
                            <a href="#">
                                <asp:Label ID="_lblFilter" runat="server" Text="-"></asp:Label></a>
                        </div>
                        <asp:Panel ID="_pnlFilterOptions" CssClass="panel panel-primary" runat="server">
                            <div class="panel-body">
                                <div class="rows">
                                    <div class="col-sm-6 col-md-4">
                                        <IP:EmployeeList ID="_currentUser" runat="server" UserMode="UsersOnly"
                                            DisplayClearLink="false" AutoPostBack="false"
                                            Employeelabel="<%$IPResources:Global, Created By%>" EnableValidation="false">
                                        </IP:EmployeeList>
                                    </div>
                                    <div class="col-sm-6 col-md-4">
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
                                    <div class="col-sm-6 col-md-4">
                                        <asp:Label ID="_lblBusArea" runat="server" Text="<%$IPResources:Global,Business Unit-Area/Department %>"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="_ddlBusArea" runat="server" Width="75%">
                                        </asp:DropDownList>
                                        <ajaxToolkit:CascadingDropDown ID="_cddlBusArea" runat="server" Category="BusinessUnitArea" BehaviorID="ccdBusArea"
                                            LoadingText="[Loading...]" PromptText="    " ServiceMethod="GetBusinessUnitAreaList"
                                            ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlBusArea"
                                            ContextKey="" ParentControlID="_ddlFacility" UseContextKey="true"></ajaxToolkit:CascadingDropDown>
                                    </div>
                                </div>
                                <div class="rows">
                                    <div class="col-md-12">
                                        <asp:Label ID="_lblHeaderDate" runat="server" Text="<%$IPResources:Global, Header Date%>"></asp:Label><br />
                                        <IP:JQDatePicker ID="_dtHeaderDate" runat="server" ShowFromDate="true" ShowDateRange="true"
                                            ShowLabelsOnSameLine="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer">
                                <asp:Button ID="_btnRefresh" runat="server" Text="<%$IPResources:Global, Display Search Results%>" OnClientClick="$('#ctl00__imbBusy').click();" />
                            </div>
                        </asp:Panel>
                        <div class="panel panel-primary">
                            <asp:GridView ID="_gvTaskHeaders" runat="server" AutoGenerateColumns="False" Width="100%"
                                CssClass="tablesorter table" EnableViewState="false" ShowHeaderWhenEmpty="true">
                                <AlternatingRowStyle ForeColor="black" Font-Bold="false" />
                                <RowStyle ForeColor="black" Font-Bold="false" />
                                <HeaderStyle Height="30px" ForeColor="white" Font-Underline="true" />
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
                                    <asp:TemplateField HeaderText="<%$IPResources:Global, Start Date%>" SortExpression="STARTDATE"
                                        ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="Literal2" Text='<%#IP.Bids.SharedFunctions.GetSortableDate(DirectCast(Container.DataItem, TaskHeaderRecord).StartDate)%>'
                                                runat="server" Style="display: none"></asp:Label>
                                            <asp:Literal ID="Literal1" Text='<%#IP.Bids.SharedFunctions.GetDate(DirectCast(Container.DataItem, TaskHeaderRecord).StartDate)%>'
                                                runat="server"></asp:Literal>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$IPResources:Global, End Date%>" SortExpression="ENDDATE"
                                        ItemStyle-Width="10%">
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
                        </div>
                    </div>

                </div>
                <script type="text/javascript">
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
                    }
                    )
                </script>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>

</body>

</html>
