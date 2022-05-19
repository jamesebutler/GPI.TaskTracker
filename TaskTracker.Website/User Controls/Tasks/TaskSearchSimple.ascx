<%@ Control Language="VB" 
    AutoEventWireup="false" 
    CodeFile="TaskSearchSimple.ascx.vb" 
    Inherits="User_Controls_Tasks_TaskSearchSimple" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
.blue_bottom {
    border-bottom: 1px solid blue;
}

.hyperlink a
	{
	   color:blue !important;
	}

</style>



<telerik:RadAjaxManager ID="RadAjaxManager" runat="server" UpdateInitiatorPanelsOnly="true">
		
	<AjaxSettings>

		   <telerik:AjaxSetting AjaxControlID="ConfiguratorPanel1">
				<UpdatedControls>
					<telerik:AjaxUpdatedControl ControlID="RadGridTaskListing" LoadingPanelID="RadAjaxLoadingPanel1" />
					<telerik:AjaxUpdatedControl ControlID="ConfiguratorPanel" />					
				</UpdatedControls>
			</telerik:AjaxSetting>
			
			<telerik:AjaxSetting AjaxControlID="RadGridTaskListing">
			<UpdatedControls>
			<telerik:AjaxUpdatedControl ControlID="RadGridTaskListing" LoadingPanelID="RadAjaxLoadingPanel1"  />
			</UpdatedControls>
			</telerik:AjaxSetting>

		
	</AjaxSettings>

</telerik:RadAjaxManager>


    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
                <asp:Label ID="_lblHeaderStatus" runat="server" Text="Task Status"></asp:Label>
            </div>
        </div>
    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="form-group">
                                <asp:Label ID="_lblTaskStatus"  runat="server" Text="<%$IPResources:Global, Task Status%>"></asp:Label><br />
                                <asp:RadioButtonList ID="_rblTaskStatus" runat="server" Width="100%" RepeatLayout="table"
                                    CellSpacing="2" RepeatDirection="Vertical" BorderWidth="0">
                                </asp:RadioButtonList>
<%--                                <IP:ExtendedCheckBoxList ID="_cblTaskStatus" RepeatLayout="table" runat="server"
                                    RepeatDirection="Horizontal" Width="100%" AllowCheckAll="true" RepeatColumns="3"
                                    AllTextValue="All" AllTextLabel="<%$IPResources:Global, All%>" ShowToolTip="true"
                                    Validation-Enabled="false" GroupingText="">
                                </IP:ExtendedCheckBoxList>--%>
                            </div>
                        </div>
                    </div>

   <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
                <asp:Label ID="_lblSearchStatus" runat="server" Text="Search By"></asp:Label>
            </div>
        </div>
    </div>

<%--                    <div class="row">
                    <div class="row blue_bottom"></div>
                        <br />
                    </div>--%>

<%--ROW 1--%>
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
<%--                        <div class="col-sm-12 col-md-6 col-lg-3">
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
                        </div>--%>
                    </div>
<%--ROW 2--%>
                    <div class="row">

                        <div class="col-sm-12 col-md-8 col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="_lblDueDate" runat="server" Text="<%$IPResources:Global, Due Date%>"></asp:Label><br />
                                <IP:JQDatePicker ID="_dtDueDate" runat="server" ShowFromDate="true" ShowDateRange="true"
                                    ShowLabelsOnSameLine="true" />
                            </div>
                        </div>

                    </div>

<%--ROW 2B--%>
                    <div class="row">
                        <div class="col-sm-12 col-md-6 col-lg-6">
                            <div class="form-group">
                                <asp:Label ID="_lblClosedDate" runat="server" Text="<%$IPResources:Global, Closed Date%>"></asp:Label><br />
                                <IP:JQDatePicker ID="_dtClosedDate" runat="server" ShowFromDate="true" ShowDateRange="true"
                                    ShowLabelsOnSameLine="true" />
                            </div>
                        </div>
                    </div>


<%--ROW 3--%>
<%--                    <div class="row">
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
                    </div>--%>
<%--ROW 4--%>
                    <div class="row">
                        <div class="col-sm-12 col-md-6">
                            <div class="form-group">
                                <asp:Label ID="_lblTitle" runat="server" Text="<%$IPResources:Global, Title%>"></asp:Label><br />
                                <asp:TextBox ID="_txtTitle" runat="server" Width="98%" TextMode="SingleLine"></asp:TextBox>
                            </div>
                        </div>

                    </div>


<%--ROW 5--%>
                    <div class="row">

                        <div class="col-sm-12 col-md-6">
                            <div class="form-group">
                                <asp:Label ID="_lblDescription" runat="server" Text="<%$IPResources:Global, Description%>"></asp:Label><br />
                                <asp:TextBox ID="_txtDescription" runat="server" Width="98%" TextMode="SingleLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
               

<%--SEARCH BUTTON --%>

            <div class="panel-footer">
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-offset-4 col-md-4">
                        <div class="form-group">
                            <asp:LinkButton ID="_btnDisplaySearchResults" runat="server" SkinID="ButtonPrimary">
                                <span class="glyphicon glyphicon-search"></span>&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Display Search Results%>"></asp:Label>
                            </asp:LinkButton>
                        </div>
                    </div>
                  
                </div>
            </div>



        <div class="tab-content table-responsive" id="_tabSearchResults" runat="server">
           
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
        
        
   <%--TELERIX GRID --%>   
            

     <asp:ImageButton ID="DownloadCSV" runat="server" 
	 OnClick="DownloadCSV_Click"  Visible="false"
	 ImageUrl="~/images/file-extension-csv-icon_bigger.png"
            CssClass="pdfButton">

 </asp:ImageButton>  


<%--      <asp:ImageButton ID="DownloadPDF" runat="server" OnClick="DownloadPDF_Click" 
          ImageUrl="~/images/file-extension-pdf-icon_bigger.png"
            CssClass="pdfButton"></asp:ImageButton>--%>
        
      <%--Start of telerik Grid--%>
  <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1">
 </telerik:RadAjaxLoadingPanel>    
    <telerik:RadGrid RenderMode="Lightweight" ID="RadGridTaskListing" runat="server"

    OnNeedDataSource="RadGridTaskListing_NeedDataSource"
			Width="99%"
			Visible="true"
			AllowPaging="true"
			ShowGroupPanel="true"
			
			AllowSorting="true"
			AllowFilteringByColumn="True"
			ExportSettings-FileName="Report"
			ExportSettings-ExportOnlyData="true"
			ExportSettings-IgnorePaging="true"
			ExportSettings-OpenInNewWindow="true"
			ExportSettings-UseItemStyles="true"
			PageSize="25"
			AutoGenerateColumns="false"
			
			HeaderStyle-HorizontalAlign="Left"
			HeaderStyle-Font-Underline="true"
			HeaderStyle-Wrap="false"
			HeaderStyle-Font-Bold="true"
			HeaderStyle-Font-Size="13px"
			HeaderStyle-Font-Names="Arial"
			
			ShowFooter="True"
			FooterStyle-HorizontalAlign="Right"
			FooterStyle-Font-Bold="true"
			FooterStyle-Font-Size="Medium"
			
			ItemStyle-Font-Names="Arial"
			ItemStyle-Font-Size="Larger"
			ItemStyle-Font-Bold="true"
			ItemStyle-BackColor="White"
			
			AlternatingItemStyle-Font-Names="Arial"
			AlternatingItemStyle-Font-Size="Larger"
			AlternatingItemStyle-Font-Bold="true"
			AlternatingItemStyle-BackColor="WhiteSmoke"

			EnableLinqExpressions="false"

			Skin="Office2010Silver">

    <ExportSettings>
     <Excel DefaultCellAlignment="Left" Format="Xlsx" WorksheetName="TaskListing" />
    <Pdf PageWidth="400mm" PageHeight="210mm" DefaultFontFamily="Arial Unicode MS" PageTopMargin="45mm"
                    BorderStyle="Medium" BorderColor="#666666"/>
    </ExportSettings>



    <MasterTableView CommandItemDisplay="Top" Width="100%" >
		    <%--TableLayout="Fixed"--%>
		    <GroupHeaderItemStyle Height="10px" /> 
		    <PagerStyle AlwaysVisible="true" />

    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />

    <Columns>

    <telerik:GridBoundColumn HeaderStyle-Width="9%" AllowSorting="true" AllowFiltering="true"   DataField="FULLNAME" HeaderText="Responsible Person" ></telerik:GridBoundColumn>
    <telerik:GridBoundColumn HeaderStyle-Width="7%" AllowSorting="true" AllowFiltering="true" DataField="STATUSNAME" HeaderText="Status" ></telerik:GridBoundColumn>
    <telerik:GridBoundColumn HeaderStyle-Width="7%" AllowSorting="true" AllowFiltering="true" DataField="PRIORITY_fullCase" HeaderText="Priority" ></telerik:GridBoundColumn>
    <telerik:GridBoundColumn HeaderStyle-Width="6%" AllowSorting="true" AllowFiltering="true" DataField="DUEDATE" ItemStyle-HorizontalAlign="Left" HeaderText="Due Date"  DataFormatString="{0:M/d/yyyy}"></telerik:GridBoundColumn>
    <telerik:GridBoundColumn HeaderStyle-Width="6%" AllowSorting="true" AllowFiltering="true" DataField="CLOSEDDATE" ItemStyle-HorizontalAlign="Left" HeaderText="Closed Date"  HeaderStyle-Wrap="true"	DataFormatString="{0:M/d/yyyy}"></telerik:GridBoundColumn>
 
  

				<telerik:GridHyperLinkColumn 
					HeaderStyle-Width="25%"
					DataTextField="Title"
					DataNavigateUrlFormatString="~/TaskDetailsGPI.aspx?HeaderNumber={0}&TaskNumber={1}"
					DataNavigateUrlFields="TASKHEADERSEQID,TASKITEMSEQID"
					UniqueName="HypColTitle" 
					HeaderText="Title"
					AllowSorting="true"
                    Target="_blank"
					AllowFiltering="true" >
					<ItemStyle CssClass="hyperlink" />
					</telerik:GridHyperLinkColumn>

    <%--<telerik:GridBoundColumn HeaderStyle-Width="5%" AllowSorting="true" AllowFiltering="false" DataField="TITLE" HeaderText="Title"  DataType="System.String"></telerik:GridBoundColumn>--%>
    <telerik:GridBoundColumn HeaderStyle-Width="30%" AllowSorting="true" AllowFiltering="true" DataField="DESCRIPTION" HeaderText="Description" HeaderStyle-Wrap="true"  DataType="System.String"></telerik:GridBoundColumn>
    <telerik:GridBoundColumn HeaderStyle-Width="5%" AllowSorting="true" AllowFiltering="true" DataField="TASKITEMSEQID" HeaderText="Task#" HeaderStyle-Wrap="true"  DataType="System.String"></telerik:GridBoundColumn>
<%--    <telerik:GridBoundColumn HeaderStyle-Width="5%" AllowSorting="true" AllowFiltering="true" DataField="TASKHEADERSEQID" HeaderText="TASKHEADERSEQID" HeaderStyle-Wrap="true"  DataType="System.String"></telerik:GridBoundColumn>--%>


				<telerik:GridHyperLinkColumn 
					HeaderStyle-Width="6%"
					DataTextField="TASKHEADERSEQID"
					DataNavigateUrlFormatString="~/ViewHeaderList.aspx?HeaderNumber={0}"
					DataNavigateUrlFields="TASKHEADERSEQID"
					UniqueName="HypColTASKHEADER" 
					HeaderText="Header#"
					AllowSorting="true"
                     Target="_blank"
					AllowFiltering="true" >
					<ItemStyle CssClass="hyperlink" />
					</telerik:GridHyperLinkColumn>


    </Columns>
    </MasterTableView>

    <ClientSettings ReorderColumnsOnClient="True" AllowDragToGroup="true" AllowColumnsReorder="True">
    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
    <Selecting AllowRowSelect="True"></Selecting>
    <Resizing
    AllowRowResize="False"
    AllowColumnResize="False"
    EnableRealTimeResize="False"
    ResizeGridOnColumnResize="False"></Resizing>

 
    </ClientSettings>

    <GroupingSettings ShowUnGroupButton="true" ></GroupingSettings>

    <FilterMenu RenderMode="Lightweight"></FilterMenu>

    <HeaderContextMenu RenderMode="Lightweight"></HeaderContextMenu>

    <PagerStyle Mode="NextPrevAndNumeric" PageSizeControlType="None"></PagerStyle>
    </telerik:RadGrid>


    <%--End of telerik Grid--%>    
        
        
        
</div>
        