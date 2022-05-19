<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TaskItems.ascx.vb" Inherits="UserControlsTasksTaskListing" %>
<asp:UpdatePanel ID="_udpTaskItems" runat="server" UpdateMode="Always">
    <ContentTemplate>

        <asp:Panel ID="_pnlTaskItems" runat="server" EnableViewState="false" CssClass="panel panel-default">
            <div class="panel-heading">
                <div class="panel-title">
                    <asp:Label ID="_pnlTaskItemsHeading" runat="server"></asp:Label>
                </div>
            </div>
            <div class="panel-body">
                <div style="float: right; text-align: center; display: inline; vertical-align: middle; width: 100%">
                    <asp:Label ID="_lblInspectionTypes" Visible="false" runat="server" Text="<%$IPResources:Global,Inspection Types%>"></asp:Label>&nbsp;<asp:DropDownList ID="_ddlInspectionTypeList" runat="server" AutoPostBack="true" Visible="false"></asp:DropDownList>
                    &nbsp;
                <asp:CheckBox ID="_cbShowClosedTasks" runat="server" AutoPostBack="true" Checked="true" Visible="false" Font-Bold="true" Text="<%$IPResources:Global,Show All Tasks%>" />&nbsp;
                <asp:Button ID="_btnAddNewTask" runat="server" Text="<%$IPResources:Global,Add Task Items%>"
                    Style="margin-right: 50px;" />
                </div> 
                <asp:Repeater ID="_rpTasks" runat="server">
                    <ItemTemplate>
                        <asp:Table ID="Table1" Width="100%" runat="server" CellSpacing="1" BorderColor="Black"  
                            BorderWidth="1" SkinID="none">
                            <asp:TableRow CssClass="Border">
                                <asp:TableCell VerticalAlign="Middle" Width="95%">
                                    <asp:Table ID="_tblTask" runat="server" Width="100%" BackColor="WHITE" CellPadding="2"
                                        CellSpacing="0" BorderWidth="0" SkinID="none">
                                        <asp:TableHeaderRow CssClass="Border">
                                            <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="30%">
                                                <asp:Label ID="_lblResponsiblePerson" runat="server" Text="<%$IPResources:Global,Responsible Person%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="10%">
                                                <asp:Label ID="_lblPriority" runat="server" Text="<%$IPResources:Global,Priority%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="15%">
                                                <asp:Label ID="_lblDueDate" runat="server" Text="<%$IPResources:Global,Due Date%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="15%">
                                                <asp:Label ID="_lblClosedDate" runat="server" Text="<%$IPResources:Global,Closed Date%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="20%">
                                                <asp:Label ID="_lblStatus" runat="server" Text="<%$IPResources:Global,Status%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="10%">
                                                <asp:Label ID="_lblWorkOrder" runat="server" Text="<%$IPResources:Global,WO/PO%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                        </asp:TableHeaderRow>
                                        <asp:TableRow CssClass="Border">
                                            <asp:TableCell VerticalAlign="bottom" Width="30%">
                                                <asp:Label ID="_lblTaskResponsiblePerson" runat="server" Font-Bold="false" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell VerticalAlign="bottom" Width="10%">
                                                <asp:Label ID="_lblTaskPriority" runat="server" Font-Bold="false" EnableTheming="false"> </asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell VerticalAlign="bottom" Width="15%">
                                                <asp:Label ID="_lblTaskDueDate" runat="server" Font-Bold="false" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell VerticalAlign="bottom" Width="15%">
                                                <asp:Label ID="_lblTaskClosedDate" runat="server" Font-Bold="false" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell VerticalAlign="bottom" Width="20%">
                                                <asp:Label ID="_lblTaskStatus" runat="server" EnableTheming="false"></asp:Label>
                                                <%--<asp:Label ID="_lblTaskLeadTime" runat="server" Font-Bold="false" EnableTheming="false"></asp:Label>--%>
                                            </asp:TableCell>
                                            <asp:TableCell VerticalAlign="bottom" Width="10%">
                                                <asp:Label ID="_lblTaskWorkOrder" runat="server" Font-Bold="false" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableHeaderRow CssClass="Border">
                                            <asp:TableHeaderCell ColumnSpan="2" HorizontalAlign="left">
                                                <asp:Label ID="_lblTitle" runat="server" Text="<%$IPResources:Global,Title%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell ColumnSpan="2" HorizontalAlign="left" Width="50%">
                                                <asp:Label ID="_lblDescription" runat="server" Text="<%$IPResources:Global,Description%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell HorizontalAlign="left">
                                                <asp:Label ID="Label6" runat="server" Text="<%$IPResources:Global,Estimated Cost%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell HorizontalAlign="left">
                                                <asp:Label ID="Label8" runat="server" Text="<%$IPResources:Global,Actual Cost%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                        </asp:TableHeaderRow>
                                        <asp:TableRow CssClass="Border">
                                            <asp:TableCell ColumnSpan="2" VerticalAlign="bottom" Width="50%">
                                                <asp:Label ID="_lblTaskTitle" runat="server" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ColumnSpan="2" VerticalAlign="bottom">
                                                <asp:Label ID="_lblTaskDescription" runat="server" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="left">
                                                <asp:Label ID="_lblEstimatedCost" runat="server" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="left">
                                                <asp:Label ID="_lblActualCost" runat="server" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableHeaderRow CssClass="Border" ID="_thrRecurringTask" Visible="false">
                                            <asp:TableHeaderCell ColumnSpan="6" HorizontalAlign="left" Width="100%">
                                                <asp:Label ID="_lblRecurringTasks" runat="server" Text="<%$IPResources:Global,Recurring Tasks%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                        </asp:TableHeaderRow>
                                        <asp:TableRow  CssClass="BorderWhite" ID="_trRecurringTask" Visible="false">
                                            <asp:TableCell ColumnSpan="6" VerticalAlign="top" Width="100%">
                                                <asp:Panel ID="_pnlRecurringTasks" runat="server" ScrollBars="vertical" Height="50px"
                                                    BorderWidth="1">
                                                    <asp:DataList ID="_rbRecurringTasks" RepeatColumns="5" RepeatDirection="horizontal"
                                                        RepeatLayout="table" runat="server" Width="98%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label20" runat="server" Text='<%# Global.TaskSearchBll.GetTaskStatus(DirectCast(Container.DataItem, IP.MEAS.BO.RecurringTasks).StatusSeqId,false,DirectCast(Container.DataItem, IP.MEAS.BO.RecurringTasks).DueDate,"Http://RI/TaskTracker/Images/") %>'></asp:Label>&nbsp;
                                                <asp:HyperLink ID="_hlRecurringTask" runat="server" Text='<%#IP.Bids.SharedFunctions.FormatDate( cdate(DirectCast(Container.DataItem, IP.MEAS.BO.RecurringTasks).DueDate)) %>'
                                                    EnableTheming="false" NavigateUrl='<%# GetTaskDetailUrl(DirectCast(Container.DataItem, IP.MEAS.BO.RecurringTasks).TaskItemSeqId)%>'></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </asp:Panel>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow CssClass="Border" Visible="false">
                                            <asp:TableCell Width="30%" VerticalAlign="bottom">
                                                <asp:Label ID="_lblBottomStatus" runat="server" Text="<%$IPResources:Global,Status%>"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="20%" VerticalAlign="bottom">
                                                <asp:Label ID="_lblCreatedBy" runat="server" Text="<%$IPResources:Global,Created By%>"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="15%" VerticalAlign="bottom">
                                                <asp:Label ID="_lblCreatedDate" runat="server" Text="<%$IPResources:Global,Creation Date%>"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="15%" VerticalAlign="bottom">
                                                <asp:Label ID="_lblLastUpdatedBy" runat="server" Text="<%$IPResources:Global,Last Updated By%>"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="20%" VerticalAlign="bottom">
                                                <asp:Label ID="_lblLastUpdatedDate" runat="server" Text="<%$IPResources:Global,Last Update Date%>"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow CssClass="Border" Visible="false">
                                            <asp:TableCell Width="30%" VerticalAlign="bottom">
                                   
                                            </asp:TableCell>
                                            <asp:TableCell Width="20%" VerticalAlign="bottom">
                                                <asp:Label ID="Label10" runat="server" Text='<%#DirectCast(Container.DataItem, TaskItem).CreatedBy %>'
                                                    EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="15%" VerticalAlign="bottom">
                                                <asp:Label ID="Label22" runat="server" Text='<%#IP.Bids.SharedFunctions.FormatDate(CDate(DirectCast(Container.DataItem, TaskItem).CreatedDate)) %>'
                                                    EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="15%" VerticalAlign="bottom">
                                                <asp:Label ID="Label24" runat="server" Text='<%#DirectCast(Container.DataItem, TaskItem).LastupdateUserName %>'
                                                    EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="20%" VerticalAlign="bottom">
                                                <asp:Label ID="Label26" runat="server" Text='<%#IP.Bids.SharedFunctions.FormatDate(CDate(DirectCast(Container.DataItem, TaskItem).LastUpdateDate)) %>'
                                                    EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </asp:TableCell>
                                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="5%" CssClass="Border">
                                    <asp:LinkButton ID="_btnEdit" runat="server" CommandArgument="" CssClass="IconButtons" ToolTip="<%$IPResources:Global,Edit Task%>"><i class="glyphicon glyphicon-pencil btn-lg"></i></asp:LinkButton>
                                    <br />
                                    <br />
                                    <asp:LinkButton ID="_btnDelete" runat="server" CommandName="Delete" CommandArgument="" CssClass="IconButtons" ToolTip="<%$IPResources:Global,Delete Task%>"><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                                    <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="_btnDelete"
                                        ConfirmText="<%$IPResources:Global,Are you sure that you would like to delete this task?%>"></ajaxToolkit:ConfirmButtonExtender>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <asp:Table ID="Table1" Width="100%" runat="server" CellSpacing="1" BorderColor="black"
                            BorderWidth="1" SkinID="none">
                            <asp:TableRow CssClass="Border">
                                <asp:TableCell VerticalAlign="Middle" Width="95%">
                                    <asp:Table ID="_tblTask" runat="server" Width="100%" BackColor="WHITE" CellPadding="2"
                                        CellSpacing="0" BorderWidth="0" SkinID="none">
                                        <asp:TableHeaderRow CssClass="BorderWhite">
                                            <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="30%">
                                                <asp:Label ID="_lblResponsiblePerson" runat="server" Text="<%$IPResources:Global,Responsible Person%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="10%">
                                                <asp:Label ID="_lblPriority" runat="server" Text="<%$IPResources:Global,Priority%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="15%">
                                                <asp:Label ID="_lblDueDate" runat="server" Text="<%$IPResources:Global,Due Date%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="15%">
                                                <asp:Label ID="_lblClosedDate" runat="server" Text="<%$IPResources:Global,Closed Date%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="20%">
                                                <asp:Label ID="_lblStatus" runat="server" Text="<%$IPResources:Global,Status%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="10%">
                                                <asp:Label ID="_lblWorkOrder" runat="server" Text="<%$IPResources:Global,WO/PO%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                        </asp:TableHeaderRow>
                                        <asp:TableRow CssClass="BorderWhite">
                                            <asp:TableCell VerticalAlign="bottom" Width="30%">
                                                <asp:Label ID="_lblTaskResponsiblePerson" runat="server" Font-Bold="false" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell VerticalAlign="bottom" Width="10%">
                                                <asp:Label ID="_lblTaskPriority" runat="server" Font-Bold="false" EnableTheming="false"> </asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell VerticalAlign="bottom" Width="15%">
                                                <asp:Label ID="_lblTaskDueDate" runat="server" Font-Bold="false" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell VerticalAlign="bottom" Width="15%">
                                                <asp:Label ID="_lblTaskClosedDate" runat="server" Font-Bold="false" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell VerticalAlign="bottom" Width="20%">
                                                <asp:Label ID="_lblTaskStatus" runat="server" EnableTheming="false"></asp:Label>
                                                <%--<asp:Label ID="_lblTaskLeadTime" runat="server" Font-Bold="false" EnableTheming="false"></asp:Label>--%>
                                            </asp:TableCell>
                                            <asp:TableCell VerticalAlign="bottom" Width="10%">
                                                <asp:Label ID="_lblTaskWorkOrder" runat="server" Font-Bold="false" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableHeaderRow CssClass="BorderWhite">
                                            <asp:TableHeaderCell ColumnSpan="2" HorizontalAlign="left" Width="50%">
                                                <asp:Label ID="Label16" runat="server" Text="<%$IPResources:Global,Title%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell ColumnSpan="2" HorizontalAlign="left" Width="50%">
                                                <asp:Label ID="Label17" runat="server" Text="<%$IPResources:Global,Description%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell HorizontalAlign="left">
                                                <asp:Label ID="Label6" runat="server" Text="<%$IPResources:Global,Estimated Cost%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                            <asp:TableHeaderCell HorizontalAlign="left">
                                                <asp:Label ID="Label8" runat="server" Text="<%$IPResources:Global,Actual Cost%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                        </asp:TableHeaderRow>
                                        <asp:TableRow CssClass="BorderWhite">
                                            <asp:TableCell ColumnSpan="2" VerticalAlign="bottom" Width="50%">
                                                <asp:Label ID="_lblTaskTitle" runat="server" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell ColumnSpan="2" VerticalAlign="bottom">
                                                <asp:Label ID="_lblTaskDescription" runat="server" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Label ID="_lblEstimatedCost" runat="server" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <asp:Label ID="_lblActualCost" runat="server" EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableHeaderRow CssClass="BorderWhite" ID="_thrRecurringTask" Visible="false">
                                            <asp:TableHeaderCell ColumnSpan="6" HorizontalAlign="left" Width="100%">
                                                <asp:Label ID="_lblRecurringTasks" runat="server" Text="<%$IPResources:Global,Recurring Tasks%>"></asp:Label>
                                            </asp:TableHeaderCell>
                                        </asp:TableHeaderRow>
                                        <asp:TableRow CssClass="BorderWhite" ID="_trRecurringTask" Visible="false">
                                            <asp:TableCell ColumnSpan="6" VerticalAlign="top" Width="100%">
                                                <asp:Panel ID="_pnlRecurringTasks" runat="server" ScrollBars="vertical" Height="50px"
                                                    BorderWidth="1">
                                                    <asp:DataList ID="_rbRecurringTasks" RepeatColumns="5" RepeatDirection="horizontal"
                                                        RepeatLayout="table" runat="server" Width="98%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label20" runat="server" Text='<%# Global.TaskSearchBll.GetTaskStatus(DirectCast(Container.DataItem, IP.MEAS.BO.RecurringTasks).StatusSeqId,false,DirectCast(Container.DataItem, IP.MEAS.BO.RecurringTasks).DueDate,"Http://RI/TaskTracker/Images/") %>'></asp:Label>&nbsp;
                                                <asp:HyperLink ID="_hlRecurringTask" runat="server" Text='<%#IP.Bids.SharedFunctions.FormatDate(cdate(DirectCast(Container.DataItem, IP.MEAS.BO.RecurringTasks).DueDate)) %>'
                                                    EnableTheming="false" NavigateUrl='<%# GetTaskDetailUrl(DirectCast(Container.DataItem, IP.MEAS.BO.RecurringTasks).TaskItemSeqId)%>'></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </asp:Panel>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow CssClass="BorderWhite" Visible="false">
                                            <asp:TableCell Width="30%" VerticalAlign="bottom">
                                                <asp:Label ID="Label18" runat="server" Text="<%$IPResources:Global,Status%>"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="20%" VerticalAlign="bottom">
                                                <asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Created By%>"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="15%" VerticalAlign="bottom">
                                                <asp:Label ID="Label3" runat="server" Text="<%$IPResources:Global,Creation Date%>"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="15%" VerticalAlign="bottom">
                                                <asp:Label ID="Label5" runat="server" Text="<%$IPResources:Global,Last Updated By%>"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="20%" VerticalAlign="bottom">
                                                <asp:Label ID="Label7" runat="server" Text="<%$IPResources:Global,Last Update Date%>"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow CssClass="BorderWhite" Visible="false">
                                            <asp:TableCell Width="30%" VerticalAlign="bottom">
                                   
                                            </asp:TableCell>
                                            <asp:TableCell Width="20%" VerticalAlign="bottom">
                                                <asp:Label ID="Label10" runat="server" Text='<%#DirectCast(Container.DataItem, TaskItem).CreatedBy %>'
                                                    EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="15%" VerticalAlign="bottom">
                                                <asp:Label ID="Label22" runat="server" Text='<%#IP.Bids.SharedFunctions.FormatDate(DirectCast(Container.DataItem, TaskItem).CreatedDate )%>'
                                                    EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="15%" VerticalAlign="bottom">
                                                <asp:Label ID="Label24" runat="server" Text='<%#DirectCast(Container.DataItem, TaskItem).LastupdateUserName %>'
                                                    EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                            <asp:TableCell Width="20%" VerticalAlign="bottom">
                                                <asp:Label ID="Label26" runat="server" Text='<%#IP.Bids.SharedFunctions.FormatDate(DirectCast(Container.DataItem, TaskItem).LastUpdateDate )%>'
                                                    EnableTheming="false"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </asp:TableCell>
                                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="5%" CssClass="BorderWhite">
                                    <asp:LinkButton ID="_btnEdit" runat="server" CommandArgument="" CssClass="IconButtons" ToolTip="<%$IPResources:Global,Edit Task%>"><i class="glyphicon glyphicon-pencil btn-lg"></i></asp:LinkButton>
                                    <br />
                                    <br />
                                    <asp:LinkButton ID="_btnDelete" runat="server" CommandName="Delete" CommandArgument="" CssClass="IconButtons" ToolTip="<%$IPResources:Global,Delete Task%>"><i class="glyphicon glyphicon-trash"></i></asp:LinkButton>
                                    <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="_btnDelete"
                                        ConfirmText="<%$IPResources:Global,Are you sure that you would like to delete this task?%>"></ajaxToolkit:ConfirmButtonExtender>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:GridView ID="_gvSimpleList" runat="server" AutoGenerateColumns="False" Width="100%"
                    CssClass="tablesorter">
                    <AlternatingRowStyle ForeColor="black" Font-Bold="false" />
                    <RowStyle ForeColor="black" Font-Bold="false" />
                    <HeaderStyle Height="30px" ForeColor="white" Font-Underline="true" />
                    <Columns>
                        <asp:TemplateField HeaderText="<%$IPResources:Global,Responsible Person%>">
                            <ItemTemplate>
                                <asp:Label ID="_lblTaskResponsiblePerson" runat="server" Font-Bold="false" EnableTheming="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$IPResources:Global,Due Date%>">
                            <ItemTemplate>
                                <asp:Label ID="Literal2" Text='<%#IP.Bids.SharedFunctions.GetSortableDate(DirectCast(Container.DataItem, TaskListing).DueDate)%>'
                                    runat="server" Style="display: none"></asp:Label>
                                <asp:Label ID="_lblTaskDueDate" runat="server" Font-Bold="false" EnableTheming="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$IPResources:Global,Status%>">
                            <ItemTemplate>
                                <asp:Label ID="_lblStatus" runat="server" Font-Bold="false" EnableTheming="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemTitle" HeaderText="<%$IPResources:Global,Title%>"
                            SortExpression="ItemTitle" />
                        <asp:TemplateField HeaderText="<%$IPResources:Global,Edit%>">
                            <ItemTemplate>
                                <asp:Button ID="_btnEdit" runat="server" Text="<%$IPResources:Global,Edit Task%>"
                                    Width="150px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <IP:ModalIframe ID="_mifDependentTasks" runat="server" Width="90%" Height="80%" ReloadPageOnClose="true"
                    AllowChildToCloseParent="true" DisplayModalButtonText="<%$IPResources:Global,Sub Tasks%>"
                    Visible="true" />
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
