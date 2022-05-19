<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TaskHeader.ascx.vb" Inherits="User_Controls_Tasks_TaskHeader" %>
<div>
    <asp:Label ID="_lblTaskListingHeader" runat="server" Font-Size="Larger"></asp:Label>
</div>
<asp:FormView ID="_frvTaskHeader" runat="server" Width="100%"  CssClass="panel-body">
    <ItemTemplate>
        <div class="panel">
             
            <div class="panel-body form-inline">
                <div class="row marginBottom5">
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Business%>"></asp:Label>
                        <br class="hidden-xs" />
                        <span class="visible-xs-inline">:</span>
                        <asp:Label ID="_lblDataBusiness" runat="server" Font-Bold="false" EnableTheming="false"
                            Text='<%#IP.Bids.SharedFunctions.LocalizeValue(DirectCast(Container.DataItem, BO.TaskHeaderRecord).Division) %>'></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Region%>"></asp:Label>
                         <br class="hidden-xs" />
                        <span class="visible-xs-inline">:</span>
                        <asp:Label ID="_lblDataRegion" runat="server" Font-Bold="false" EnableTheming="false"
                            Text='<%#IP.Bids.SharedFunctions.LocalizeValue(DirectCast(Container.DataItem, BO.TaskHeaderRecord).Region) %>'></asp:Label>
                    </div>

                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label3" runat="server" Text="<%$IPResources:Global,Facility%>"></asp:Label>
                         <br class="hidden-xs" />
                        <span class="visible-xs-inline">:</span>
                        <asp:Label ID="_lblDataFacility" runat="server" Font-Bold="false" EnableTheming="false"
                            Text='<%#DirectCast(Container.DataItem, BO.TaskHeaderRecord).SiteName %>'></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label4" runat="server" Text="<%$IPResources:Global,Business Unit-Area/Department%>"></asp:Label>
                        <br class="hidden-xs" />
                        <span class="visible-xs-inline">:</span>
                        <asp:Label ID="_lblDataBusArea" runat="server" Font-Bold="false" EnableTheming="false"
                            Text='<%#IP.Bids.SharedFunctions.LocalizeList(DirectCast(Container.DataItem, BO.TaskHeaderRecord).BusinessUnitArea, "-") %>'></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label5" runat="server" Text="<%$IPResources:Global,Line/Machine%>"></asp:Label>
                        <br class="hidden-xs" />
                        <span class="visible-xs-inline">:</span>
                        <asp:Label ID="_lblDataLine" runat="server" Font-Bold="false" EnableTheming="false"
                            Text='<%#IP.Bids.SharedFunctions.LocalizeList(DirectCast(Container.DataItem, BO.TaskHeaderRecord).Line, "-")%>'></asp:Label>
                    </div>
                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label6" runat="server" Text="<%$IPResources:Global,Date Selection%>"></asp:Label>                        
                        <span class="visible-xs-inline">:</span><br />
                        <asp:Label ID="_lblDataDateSelection" runat="server" Font-Bold="false" EnableTheming="false"
                            Text='<%#IP.Bids.SharedFunctions.FormatDate(DirectCast(Container.DataItem, BO.TaskHeaderRecord).StartDate) & " - " & IP.Bids.SharedFunctions.FormatDate(DirectCast(Container.DataItem, BO.TaskHeaderRecord).EndDate) %>'></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label7" runat="server" Text="<%$IPResources:Global,Source System and Reference%>"></asp:Label>
                        #
                        <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblSourceSystem" runat="server" Font-Bold="false" EnableTheming="false"
                            Text='<%#IP.Bids.SharedFunctions.LocalizeValue(DirectCast(Container.DataItem, BO.TaskHeaderRecord).ExternalSourceName) & " " & DirectCast(Container.DataItem, BO.TaskHeaderRecord).ExternalRef %>'></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label8" runat="server" Text="<%$IPResources:Global,High Level Security%>"></asp:Label>
                        <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="Label11" runat="server" Font-Bold="false" EnableTheming="false" Text='<%#DirectCast(Container.DataItem, BO.TaskHeaderRecord).SecurityLevel %>'></asp:Label>
                    </div>
                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12">
                        <asp:Label ID="Label9" runat="server" Text="<%$IPResources:Global,Title%>"></asp:Label>
                        <span class="visible-xs-inline">:</span>
                        <br />
                        <asp:Label ID="_lblDataTitle" runat="server" Font-Bold="false" EnableTheming="false"
                            Text='<%#DirectCast(Container.DataItem, BO.TaskHeaderRecord).Title%>'></asp:Label>
                       <br /> <asp:Label ID="_lblTranslatedTitle" runat="server" Font-Bold="false" EnableTheming ="false"  Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12">
                        <asp:Label ID="Label10" runat="server" Text="<%$IPResources:Global,Description%>"></asp:Label>
                        <span class="visible-xs-inline">:</span>
                        <br />
                        <asp:Label ID="_lblDataDescription" runat="server" Font-Bold="false" EnableTheming="false"
                            Text='<%#DirectCast(Container.DataItem, BO.TaskHeaderRecord).Description %>'></asp:Label>
                        <br /><asp:Label ID="_lblTranslatedDescription" runat="server" Font-Bold="false" EnableTheming ="false"  Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12 col-sm-6">
                        <asp:Label ID="Label12" runat="server" Text="<%$IPResources:Global,Types%>"></asp:Label>
                         <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblDataTypes" runat="server" Font-Bold="false" EnableTheming="false"
                            Text='<%#GetTaskTypes()%>'></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <asp:Label ID="Label13" runat="server" Text="<%$IPResources:Global,Activity%>"></asp:Label>
                         <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblDataActivity" runat="server" Font-Bold="false" EnableTheming="false"
                            Text='<%#IP.Bids.SharedFunctions.LocalizeValue(DirectCast(Container.DataItem, BO.TaskHeaderRecord).ActivityName) %>'></asp:Label>
                    </div>
                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12 col-sm-6">
                        <asp:Label ID="Label14" runat="server" Text="<%$IPResources:Global,Task Type Manager%>"></asp:Label>
                         <span class="visible-xs-inline">:</span>
                        <br />
                        <asp:Label ID="Label15" runat="server" Font-Bold="false" EnableTheming="false" Text='<%#GetTypeManagers() %>'></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <asp:Label ID="Label16" runat="server" Text="<%$IPResources:Global,Business Manager%>"></asp:Label>
                         <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="Label17" runat="server" Font-Bold="false" EnableTheming="false" Text='<%#GetBusinessManagers() %>'></asp:Label>
                    </div>
                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12 col-sm-3">
                        <asp:Label ID="Label18" runat="server" Text="<%$IPResources:Global,Created By%>"></asp:Label>
                         <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblCreatedBy" Font-Bold="false" runat="server" EnableTheming="false" Text="<%#DirectCast(Container.DataItem, BO.TaskHeaderRecord).CreatedBy %>"></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-3">
                        <asp:Label ID="Label19" runat="server" Text="<%$IPResources:Global,Creation Date%>"></asp:Label>
                        <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblCreationDate" Font-Bold="false" runat="server" EnableTheming="false" Text="<%#IP.Bids.SharedFunctions.FormatDate(DirectCast(Container.DataItem, BO.TaskHeaderRecord).CreateDate) %>"></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-3">
                        <asp:Label ID="Label20" runat="server" Text="<%$IPResources:Global,Last Updated By%>"></asp:Label>
                        <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblLastUpdatedBy" Font-Bold="false" runat="server" EnableTheming="false" Text="<%#DirectCast(Container.DataItem, BO.TaskHeaderRecord).LastUpdateUserName %>"></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-3">
                        <asp:Label ID="Label21" runat="server" Text="<%$IPResources:Global,Last Update Date%>"></asp:Label>
                        <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblLastUpdateDate" Font-Bold="false" runat="server" EnableTheming="false" Text="<%#IP.Bids.SharedFunctions.FormatDate(DirectCast(Container.DataItem, BO.TaskHeaderRecord).LastUpdateDate) %>"></asp:Label>
                    </div>
                </div>
           
            </div>
        </div>
    </ItemTemplate>
</asp:FormView>
