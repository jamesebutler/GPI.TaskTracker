<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TaskSearch.ascx.vb" Inherits="UserControlsTaskSearch" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Register Src="~/User Controls/JQEventCalendar.ascx" TagName="JQEventCalendar"
    TagPrefix="IP" %>
<%@ Register Namespace="RealWorld.Grids" TagPrefix="RWG" %>
<%@ Register Src="~/User Controls/Tasks/MultiTaskEdit.ascx" TagName="MultiTaskEdit"
    TagPrefix="IP" %>
<%@ Register Src="~/User Controls/ReportingSite.ascx" TagName="ReportingSite" TagPrefix="IP" %>
<%@ Register TagPrefix="IP" Namespace="IP.Bids.UserControls" Assembly="IP.Bids.UserControls" %>
<%@ Register Src="~/User Controls/AdvancedEmployeeListDropdown.ascx" TagName="EmployeeList" TagPrefix="IP" %>
<%@ Register Src="~/User Controls/AlertBox.ascx" TagPrefix="IP" TagName="AlertBox" %>

<asp:UpdatePanel ID="_udpCalendarView" runat="server" RenderMode="block" UpdateMode="Always">
    <ContentTemplate>
        
        <script type="text/javascript">
            function ShowTodayOnCalendar() {
                $('#calendar').fullCalendar('today');
            }
            function SetActiveTab(tabNumber) {
                $(document).ready(function () {
                    $('#myTabs li:eq(' + tabNumber + ') a').tab('show');
                    ShowTodayOnCalendar();
                });
            }
            $(document).ready(function () {
                $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                    var target = $(e.target).attr("href") // activated tab
                    alert(target);
                });
            });

        </script>

        <IP:AlertBox runat="server" ID="_alertBox" />
        <ajaxToolkit:CollapsiblePanelExtender 
            ID="_cpeSearchSelections" 
            runat="Server" 
            TargetControlID="_pnlCalendarSearch"
            Collapsed="true" 
            CollapseControlID="_pnlFilterHeader" 
            ExpandControlID="_pnlFilterHeader"
            SuppressPostBack="true" 
            TextLabelID="_lblShowHideCalendar" 
            CollapsedText="+ Show Search Criteria"
            ExpandedText="- Hide Search Criteria" 
            ScrollContents="false" 
            Enabled="true" />
        
        <asp:Panel ID="_pnlReportSelection" runat="server" Visible="true">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:Label ID="_lblReportListing" runat="server" Text="<%$IPResources:Global, Report Listing%>"></asp:Label>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12 col-sm-6 col-md-4">
                            <asp:Label ID="_lblReport" runat="server" Text="<%$IPResources:Global,Report %>"></asp:Label><br />
                            <asp:DropDownList
                                ID="_ddlReportTitles" runat="server" AutoPostBack="true" Width="75%">
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-4">
                            <asp:Label ID="_lblReportSortValues" runat="server" Text="<%$IPResources:Global, Sort Order%>"></asp:Label><br />
                            <asp:DropDownList
                                ID="_ddlReportSortValue" runat="server" Width="75%">
                            </asp:DropDownList>
                        </div>
                    </div>

                </div>
            </div>


        </asp:Panel>
        
        
        <asp:Panel ID="_pnlSearchContainer" runat="server" CssClass="panel panel-default" Visible="false">
            <div class="panel-heading pointer">
                <div class="panel-title" id="_pnlFilterHeader">
                    <asp:Label ID="_lblShowHideCalendar" runat="server" Text=""></asp:Label>
                </div>
            </div>

            <asp:Panel ID="_pnlCalendarSearch" runat="server">
                <div class="panel-body">
                    <div class="panel-footer">
                        <div class="row">
                            <div class="hidden-xs hidden-sm col-md-2">&nbsp;</div>
                            <div class="col-sm-4 col-md-3">
                                <div class="form-group">
                                    <asp:Label ID="_lblSearchOptions" runat="server" Text="<%$IPResources:Global, Search Display Options%>"
                                        Visible="false"></asp:Label>
                                    <asp:DropDownList ID="_ddlSearchOptions" ClientIDMode="static" runat="server" AutoPostBack="false" Visible="true" Width="75%">
                                        <asp:ListItem Text="<%$IPResources:Global, List View By Headers%>" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="<%$IPResources:Global, List View By Task Items%>" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="<%$IPResources:Global, Calendar View by Month%>" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-4 col-md-3">
                                <div class="form-group">
                                    <asp:LinkButton ID="_btnSaveSearchCriteria" runat="server" SkinID="ButtonPrimary">
                                        <span class="glyphicon glyphicon-floppy-save"></span>&nbsp;<asp:Label ID="_lblSaveButton" runat="server" Text="<%$IPResources:Global,Save Search Criteria%>"></asp:Label>
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="col-sm-4 col-md-3">
                                <div class="form-group">

                                    <asp:LinkButton ID="_btnResetSearchCriteria" runat="server" SkinID="Button">
                                        <span class="glyphicon glyphicon-refresh"></span>&nbsp;<asp:Label ID="Label5" runat="server" Text="<%$IPResources:Global,Reset Search Criteria%>"></asp:Label>
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="hidden-xs hidden-sm col-md-1"></div>
                        </div>
                    </div>
                    <IP:ReportingSite ID="_ReportingSite" runat="server" EnableViewState="true" />
                    <div class="row">
                        <div class="col-sm-12 col-md-12 col-lg-6">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <asp:Label ID="_lblIncidentType" runat="server" Text="<%$IPResources:Global, Types%>"></asp:Label>
                                </div>
                                <div class="panel-body" style="height: 200px; overflow-y: scroll">
                                    <div class="form-group">
                                        <IP:ExtendedCheckBoxList ID="_cblIncidentType" RepeatLayout="table" runat="server"
                                            RepeatDirection="Vertical" Width="100%" AllowCheckAll="true" RepeatColumns="3"
                                            ShowToolTip="true" AllTextValue="All" AllTextLabel="<%$IPResources:Global, All%>"
                                            GroupingText="" Validation-Enabled="false">
                                        </IP:ExtendedCheckBoxList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 col-md-12 col-lg-6">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <asp:Label ID="_lblActivity" runat="server" Text="<%$IPResources:Global, Activity%>"></asp:Label>
                                </div>
                                <div class="panel-body" style="height: 200px; overflow-y: scroll">
                                    <div class="form-group">
                                        <IP:ExtendedCheckBoxList ID="_cblActivity" RepeatLayout="table" runat="server" RepeatDirection="Vertical"
                                            Width="100%" AllowCheckAll="true" RepeatColumns="3" AllTextValue="All" AllTextLabel="<%$IPResources:Global, All%>"
                                            ShowToolTip="true" Validation-Enabled="false" GroupingText="">
                                        </IP:ExtendedCheckBoxList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="form-group">
                                <asp:Label ID="_lblTaskStatus" runat="server" Text="<%$IPResources:Global, Task Status%>"></asp:Label><br />
                                <asp:RadioButtonList ID="_rblTaskStatus" runat="server" Width="100%" RepeatLayout="table"
                                    CellSpacing="2" RepeatDirection="horizontal" BorderWidth="0">
                                </asp:RadioButtonList>
                                <IP:ExtendedCheckBoxList ID="_cblTaskStatus" RepeatLayout="table" runat="server"
                                    RepeatDirection="Horizontal" Width="100%" AllowCheckAll="true" RepeatColumns="3"
                                    AllTextValue="All" AllTextLabel="<%$IPResources:Global, All%>" ShowToolTip="true"
                                    Validation-Enabled="false" GroupingText="">
                                </IP:ExtendedCheckBoxList>
                            </div>
                        </div>
                    </div>

<%-- START OF BOTTOM OF SEARCH --%>
                    <div class="row">
                        <div class="col-sm-12 col-md-6 col-lg-3">
                            <div class="form-group">
                                <IP:EmployeeList ID="_CreatedBy" runat="server" AllowUserRoles="false" Employeelabel="<%$IPResources:Global, Created By%>"
                                    EnableViewState="true" />
                            </div>
                        </div>
                        <div class="col-sm-12 col-md-6 col-lg-3">
                            <div class="form-group">
                                <IP:EmployeeList ID="_ResponsiblePerson" runat="server" Employeelabel="<%$IPResources:Global, Responsible Person%>"
                                    EnableViewState="true" UserMode="UsersOnly" />
                            </div>
                        </div>
                        <div class="col-sm-12 col-md-6 col-lg-3">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <asp:Label ID="_lblIncidentEventSource" runat="server" Text="<%$IPResources:Global,Source System%>"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="_ddlSourceSystem" runat="server" Width="75%">
                                            <asp:ListItem Text=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:Label ID="_lblIncidentEventNumber" runat="server" Text="<%$IPResources:Global,Reference%>"></asp:Label>#<br />
                                        <asp:TextBox ID="_txtIncidentEventNumber" runat="server" Width="100px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 col-md-6 col-lg-3">
                            <div class="form-group">
                                <asp:Label ID="_lblHighLevelSecurity" runat="server" Text="<%$IPResources:Global, High Level Security%>"></asp:Label><br />
                                <asp:CheckBox ID="_cbHighLevelSecurity" Text="<%$IPResources:Global, Restricted Access%>"
                                    ForeColor="red" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-md-8 col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="_lblDueDate" runat="server" Text="<%$IPResources:Global, Due Date%>"></asp:Label><br />
                                <IP:JQDatePicker ID="_dtDueDate" runat="server" ShowFromDate="true" ShowDateRange="true"
                                    ShowLabelsOnSameLine="true" />
                            </div>
                        </div>
                        <div class="col-sm-12 col-md-8 col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="_lblClosedDate" runat="server" Text="<%$IPResources:Global, Closed Date%>"></asp:Label><br />
                                <IP:JQDatePicker ID="_dtClosedDate" runat="server" ShowFromDate="true" ShowDateRange="true"
                                    ShowLabelsOnSameLine="true" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-md-8 col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="_lblHeaderDate" runat="server" Text="<%$IPResources:Global, Header Date%>"></asp:Label><br />
                                <IP:JQDatePicker ID="_dtHeaderDate" runat="server" ShowFromDate="true" ShowDateRange="true"
                                    ShowLabelsOnSameLine="true" />
                            </div>
                        </div>
                        <div class="col-sm-12 col-md-4">
                            <div class="form-group">
                                <asp:Label ID="_lblHeaderNumber" runat="server" Text="<%$IPResources:Global, Header%>"></asp:Label><br />
                                <asp:TextBox ID="_txtHeaderNumber" runat="server"></asp:TextBox>                                
                               
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 col-md-6">
                            <div class="form-group">
                                <asp:Label ID="_lblTitle" runat="server" Text="<%$IPResources:Global, Title%>"></asp:Label><br />
                                <asp:TextBox ID="_txtTitle" runat="server" Width="90%" TextMode="SingleLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-12 col-md-6">
                            <div class="form-group">
                                <asp:Label ID="_lblDescription" runat="server" Text="<%$IPResources:Global, Description%>"></asp:Label><br />
                                <asp:TextBox ID="_txtDescription" runat="server" Width="98%" TextMode="SingleLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
           
            
           
<%--  DISPLAY SEARCH  / EXPORT TO EXCEL--%>          
            
            
            
            
            <div class="panel-footer">
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-offset-2 col-md-4">
                        <div class="form-group">
                            <asp:LinkButton ID="_btnDisplaySearchResults" runat="server" SkinID="ButtonPrimary">
                                <span class="glyphicon glyphicon-search"></span>&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Display Search Results%>"></asp:Label>
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-4">
                        <div class="form-group">
                            <asp:LinkButton ID="_btnExportTasks" runat="server" SkinID="Button">
                                <span class="glyphicon glyphicon-download-alt"></span>&nbsp;<asp:Label ID="Label3" runat="server" Text="<%$IPResources:Global,Export To Excel%>"></asp:Label>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        
        
        <asp:Panel ID="_pnlReportCriteria" runat="server" CssClass="panel panel-default">
            <div class="panel-heading">
                <asp:Label ID="_lblReportSelections" runat="server" Text="<%$IPResources:Global, Report Selections%>"></asp:Label>
            </div>
            <div class="panel-body">
                <asp:Table ID="_tblTaskSearch" runat="server" CellPadding="4" CellSpacing="4" BackColor="white" EnableTheming="false"
                    BorderWidth="0" Width="100%" Visible="false" EnableViewState="true">

                    <asp:TableRow Visible="true">
                        <asp:TableCell ColumnSpan="2">
                            <asp:Label ID="_lblReportType" runat="server" Text="<%$IPResources:Global, Report Type%>"></asp:Label><br />
                            <asp:RadioButtonList ID="_rblReportType" runat="server" RepeatDirection="horizontal">
                            </asp:RadioButtonList>
                        </asp:TableCell>
                        <asp:TableCell ColumnSpan="2">
                            <asp:Label ID="_lblEstimatedDueDateRange" runat="server" Text="<%$IPResources:Global, Estimated Due Date%>"></asp:Label><br />
                            <IP:ExtendedCheckBoxList ID="_cblEstimatedDueDateRange" AllowCheckAll="true" AllTextLabel="<%$IPResources:Global, All%>"
                                AllTextValue="All" runat="server" RepeatDirection="horizontal" ShowToolTip="true">
                            </IP:ExtendedCheckBoxList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Visible="true">
                        <asp:TableCell ColumnSpan="2">
                            <asp:Label ID="_lblTaskListing" runat="server" Text="<%$IPResources:Global, Task Listing%>"></asp:Label><br />
                            <asp:RadioButtonList ID="_rblTaskListing" runat="server" RepeatDirection="horizontal">
                                <asp:ListItem Text="<%$IPResources:Global, All%>" Value="All" Selected="true"></asp:ListItem>
                                <asp:ListItem Text="None" Value="None"></asp:ListItem>
                            </asp:RadioButtonList>
                        </asp:TableCell>
                        <asp:TableCell ColumnSpan="2">
                            <IP:EmployeeList ID="_RoleList" runat="server" Employeelabel="<%$IPResources:Global, Role List%>"
                                EnableViewState="true" UserMode="RolesOnly" Visible="false" />
                            <asp:Label ID="_lblSubElement" runat="server" Text="<%$IPResources:Global, GMS Sub-Element%>" Visible="false"></asp:Label><br />
                            <asp:DropDownList ID="_ddlSubElement" runat="server" Visible="false" Width="75%"></asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>

        </asp:Panel>
       
        <ajaxToolkit:FilteredTextBoxExtender ID="_txtHeaderNumber_FilteredTextBoxExtender" runat="server" 
                                    Enabled="True" TargetControlID="_txtHeaderNumber" FilterType="Numbers">
                                </ajaxToolkit:FilteredTextBoxExtender>
        <div id="_divReportSearchButtons" runat="server" class="panel panel-default">
            <div class="panel-footer">
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-offset-2 col-md-4">
                        <div class="form-group">
                            <asp:LinkButton ID="_btnReportView" runat="server" SkinID="ButtonPrimary">
                                <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-file"></span>&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Display Report%>"></asp:Label></asp:PlaceHolder>
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-4">
                        <div class="form-group">
                            <asp:LinkButton ID="_btnResetReportSelections" runat="server" SkinID="Button">
                                <asp:PlaceHolder runat="server">
                                    <span class="glyphicon glyphicon-refresh"></span>&nbsp;<asp:Label ID="Label6" runat="server" Text="<%$IPResources:Global,Reset Selection Criteria%>"></asp:Label></asp:PlaceHolder>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>

       
        
        <div id="_tabSearchTabs" runat="server">
            <!-- Nav tabs -->
            <ul id="myTabs" class="nav nav-tabs ui-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#calendar" aria-controls="calendar" role="tab" data-toggle="tab">
                    <asp:Localize runat="server" Text="<%$IPResources:Global,Calendar View%>"></asp:Localize></a></li>
                <li role="presentation"><a href="#headerlisting" aria-controls="headerlisting" role="tab" data-toggle="tab">
                    <asp:Localize runat="server" Text="<%$IPResources:Global,Task Header Listing%>"></asp:Localize></a></li>
                <li role="presentation"><a href="#taskitemlisting" aria-controls="taskitemlisting" role="tab" data-toggle="tab">
                    <asp:Localize runat="server" Text="<%$IPResources:Global,Task Item Listing%>"></asp:Localize></a></li>

      
           

            </ul>
        </div>
        <!-- Tab panes -->
        
        
        <div class="tab-content table-responsive" id="_tabSearchResults" runat="server">
            <div role="tabpanel" class="tab-pane active" id="calendar">
                <asp:Panel ID="_pnlCalendarView" runat="server" ScrollBars="auto" Visible="true"
                    Height="100%" EnableViewState="False" Style="height: 100%">
                    <IP:JQEventCalendar ID="_calEvents" runat="server" />
                </asp:Panel>
            </div>
            <div role="tabpanel" class="tab-pane" id="headerlisting">
                <asp:Panel ID="_pnlCalendarListing" runat="server" ScrollBars="auto" Visible="false"
                    Height="100%" EnableViewState="False" Style="min-height: 600px; height: 100%">
                    <asp:Label CssClass="hidden-xs hidden-sm" ID="_lblHeaderListingSortInstructions" runat="server" Text="<%$IPResources:Global, SortMultipleColumns%>"></asp:Label>
                    <section id="no-more-gridView">
                        <asp:GridView ID="_gvCalendarListing" AutoGenerateColumns="false" runat="server"
                            Width="99%" CssClass="tablesorter {sortlist: [[0,0],[1,0],[2,0],[3,0],[4,0]]}"
                            AllowSorting="false" CellPadding="4" EnableViewState="false">
                            <AlternatingRowStyle ForeColor="black" Font-Bold="false" />
                            <RowStyle ForeColor="black" Font-Bold="false" />
                            <HeaderStyle Height="30px" ForeColor="Black" Font-Underline="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="<%$IPResources:Global, Date Entered%>" SortExpression="HeaderCreateDate" HeaderStyle-CssClass="sorter-longDate">
                                    <ItemTemplate>
                                        <%--  <asp:Label ID="Literal2" Text='<%#IP.Bids.SharedFunctions.GetSortableDate(DirectCast(Container.DataItem, TaskListing).HeaderCreateDate)%>'
                                            runat="server" Style="display: none"></asp:Label>--%>
                                        <asp:Literal ID="Literal1" Text='<%#IP.Bids.SharedFunctions.GetDate(DirectCast(Container.DataItem, TaskListing).HeaderCreateDate)%>'
                                            runat="server"></asp:Literal>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="<%$IPResources:Global, Facility/Location%>" DataField="SiteName"
                                    SortExpression="SiteName" />
                                <asp:TemplateField HeaderText="<%$IPResources:Global, Business Unit-Area/Department%>"
                                    SortExpression="BUSUNIT_AREA" ItemStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Literal ID="Literal1" Text='<%#IP.Bids.SharedFunctions.LocalizeList(DirectCast(Container.DataItem, TaskListing).BusinessUnitArea, "-")%>'
                                            runat="server"></asp:Literal>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global, Line/Machine%>" SortExpression="Line"
                                    ItemStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Literal ID="Literal1" Text='<%#IP.Bids.SharedFunctions.LocalizeList(DirectCast(Container.DataItem, TaskListing).Line, "-")%>'
                                            runat="server"></asp:Literal>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global, Title%>" SortExpression="Title">
                                    <ItemTemplate>
                                        <%--<a target="_blank" href="Javascript:parent.window.location=('<%#String.Format(CultureInfo.CurrentCulture, Page.ResolveUrl("~/TaskHeader.aspx?HeaderNumber={0}"), DirectCast(Container.DataItem, TaskListing).TaskHeaderSeqId) %>');"--%>
                                        <a target="_blank" href="<%#String.Format(CultureInfo.CurrentCulture, Page.ResolveUrl("~/TaskHeader.aspx?HeaderNumber={0}"), DirectCast(Container.DataItem, TaskListing).TaskHeaderSeqId) %>"    
                                            title='Goto Task Header'>
                                            <%#DirectCast(Container.DataItem, TaskListing).HeaderTitle%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global, Task Type%>" SortExpression="TASKTYPE"
                                    ItemStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Literal ID="Literal1" Text='<%#IP.Bids.SharedFunctions.LocalizeList(DirectCast(Container.DataItem, TaskListing).TaskType, ",")%>'
                                            runat="server"></asp:Literal>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global, Activity%>" SortExpression="ACTIVITYNAME">
                                    <ItemTemplate>
                                        <asp:Literal ID="Literal1" Text='<%#IP.Bids.SharedFunctions.LocalizeValue(DirectCast(Container.DataItem, TaskListing).ActivityName)%>'
                                            runat="server"></asp:Literal>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </section>
                </asp:Panel>
            </div>
            
            
            <div role="tabpanel" class="tab-pane" id="taskitemlisting" >
                <div id="preload-img">
                    <h1>
                        <asp:Label ID="Label4" runat="server" Text="<%$IPResources:Global,Loading%>"></asp:Label>...
                    </h1>
                </div>
                <asp:Panel ID="_pnlTaskListing" runat="server" ScrollBars="none" Visible="false"
                    EnableViewState="true" Style="vertical-align: top">
                    <iframe id="_ifrMultiEdit" runat="server" width="100%" height="2000px;" frameborder="0" style="display: none; overflow: hidden" src="~/Popups/BulkEditTasks.aspx"></iframe>
                </asp:Panel>
            </div>
        </div>
        
        <div style="display: none">
            <IP:ModalIframe ID="_btnModal" runat="server" />
        </div>
        <asp:Label ID="_lblReportUrl" runat="server"></asp:Label>
        <script type="text/javascript">
            $(document).ready(function () {
                if (currentTabNumber !== null)
                    SetActiveTab(currentTabNumber);
                else
                    SetActiveTab(0);
            });
        </script>
    </ContentTemplate>
</asp:UpdatePanel>
