<%@ Control Language="VB" 
    AutoEventWireup="false" 
    
    CodeFile="HeaderList.ascx.vb" 
    Inherits="User_Controls_Tasks_HeaderList" %>
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
					<telerik:AjaxUpdatedControl ControlID="RadGridHeaderListing" LoadingPanelID="RadAjaxLoadingPanel1" />
					<telerik:AjaxUpdatedControl ControlID="ConfiguratorPanel" />					
				</UpdatedControls>
			</telerik:AjaxSetting>
			
			<telerik:AjaxSetting AjaxControlID="RadGridHeaderListing">
			<UpdatedControls>
			<telerik:AjaxUpdatedControl ControlID="RadGridHeaderListing" LoadingPanelID="RadAjaxLoadingPanel1"  />
			</UpdatedControls>
			</telerik:AjaxSetting>

		
	</AjaxSettings>

</telerik:RadAjaxManager>




<div class="row">
	
   <%--TELERIX GRID --%>     

    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
                <asp:Label ID="_lblHeaderTitle" runat="server" Text="Header Listing with Tasks"></asp:Label>
            </div>
        </div>

</div>

      <asp:ImageButton ID="DownloadCSV" runat="server" 
	 OnClick="DownloadCSV_Click" 
	 ImageUrl="~/images/file-extension-csv-icon_bigger.png"
            CssClass="pdfButton">

 </asp:ImageButton>  
	

<%--          <asp:ImageButton ID="DownloadXLS" runat="server" 
	 OnClick="DownloadXLS_Click" 
	 ImageUrl="~/images/file-extension-xls-icon_bigger.png"
            CssClass="pdfButton">

 </asp:ImageButton> --%>


      <%--Start of telerik Grid--%>
  <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1">
 </telerik:RadAjaxLoadingPanel>    
<telerik:RadGrid RenderMode="Lightweight" ID="RadGridHeaderListing" runat="server"

OnNeedDataSource="RadGridHeaderListing_NeedDataSource"
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
<Excel DefaultCellAlignment="Left"  Format="Xlsx" AutoFitColumnWidth="AutoFitAll" WorksheetName="HeaderListing" />
<Pdf PageWidth="297mm" PageHeight="210mm" />
</ExportSettings>

<MasterTableView CommandItemDisplay="Top" Width="100%" UseAllDataFields="true" >
		<%--TableLayout="Fixed"--%>
		<GroupHeaderItemStyle Height="10px" /> 
		<PagerStyle AlwaysVisible="true" />

<CommandItemSettings ShowExportToExcelButton="true"  ShowAddNewRecordButton="false" ShowRefreshButton="false" />

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
<telerik:GridBoundColumn HeaderStyle-Width="5%" AllowSorting="true" AllowFiltering="false" DataField="TASKHEADERSEQID" HeaderText="Header#" HeaderStyle-Wrap="true"  DataType="System.String"></telerik:GridBoundColumn>





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