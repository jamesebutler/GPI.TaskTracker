<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false"
    CodeFile="TemplateTaskDetails.aspx.vb" Inherits="TemplateTaskDetails" Trace="false" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<%@ Register Src="~/User Controls/FileAttachments.ascx" TagName="FileAttachments"
    TagPrefix="IP" %>
<%@ Register Src="~/User Controls/Tasks/TaskItems.ascx" TagName="TaskItems" TagPrefix="IP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <Asp:UpdatePanel id="_udpTaskDetails" runat="server">
        <ContentTemplate>
            <asp:Table ID="_tblTask" runat="server" Width="100%" BackColor="white">
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="5" CssClass="BorderHeader" VerticalAlign="Middle" HorizontalAlign="left">
                        <center><div style="float: none; display: inline; vertical-align: middle; text-align: center">
                            <br />
                            <asp:Button ID="_btnSaveTask" runat="server" Text="Save Task" Height="30px" OnClientClick="return CheckForm('TaskDetails');" />&nbsp;&nbsp;&nbsp;<asp:Button
                                id="_btnAddTask" runat="server" Text="Add New Task" Height="30px" />
                            &nbsp;&nbsp;
                            <asp:Button ID="_btnTaskHeader" runat="server" Text="Return to Template Task Header" Height="30px" />
                        </div></center>
                    </asp:TableCell>

                </asp:TableRow>
                <asp:TableRow CssClass="Border">
                    <asp:TableCell VerticalAlign="Top" Width="30%">
                        <IP:EmployeeList id="_ResponsiblePerson" employeelabel="Responsible Role" runat="server"
                            EnableValidation="true" UserMode="RolesOnly" >
                            <Validation ValidationGroup="TaskDetails" ValidationErrorMessage="Responsible Person is required"
                                ValidationErrorText="*" ValidateEmptyText="true" />
                        </IP:EmployeeList>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" Width="20%">
                        <asp:Label ID="_lblPriority" runat="server" Text="Priority"></asp:Label><br />
                        <asp:DropDownList ID="_ddlPriority" runat="server" Width="100">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" Width="40%">
                        <asp:Label ID="_lblWeeksBefore" runat="server" Text="Task Occurrence"></asp:Label>
                        <asp:RadioButtonList ID="_rblWeeks" runat="server" RepeatDirection="horizontal">
                            <asp:ListItem Text="Weeks Before" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Weeks After"></asp:ListItem>
                        </asp:RadioButtonList>
                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="_txtWeeksBefore" runat="server" Width="40" Text='0' MaxLength="3"></asp:TextBox>
                        <asp:RequiredFieldValidator id="_rfvWeeks" runat="server" ControlToValidate="_rblWeeks"
                            Text="*" ErrorMessage="Weeks Before is required" ValidationGroup="TemplateTaskDetails"></asp:RequiredFieldValidator>
                        <br />
                        <ajaxToolkit:FilteredTextBoxExtender id="FilteredTextBoxExtender8" runat="server"
                            targetcontrolid="_txtWeeksBefore" filtertype="Numbers" />
                        <asp:RangeValidator MinimumValue="0" MaximumValue="9999" runat="server" ID="_rvWeeksBefore"
                            ControlToValidate="_txtWeeksBefore" Text="*" Type="Integer" ErrorMessage="Weeks Before should be between 0 and 9999"
                            ValidationGroup="TemplateTaskDetails"></asp:RangeValidator>
                    </asp:TableCell>
               </asp:TableRow>
                <asp:TableRow CssClass="Border">
                    <asp:TableCell ColumnSpan="2" VerticalAlign="Top">
                        <asp:Label ID="Label14" runat="server" Text="Title"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_txtTitle"
                            Text="*" ErrorMessage="Title is required" ValidationGroup="TemplateTaskDetails"></asp:RequiredFieldValidator>
                        <br />
                        <IP:AdvancedTextBox expandheight="true" id="_txtTitle" runat="server" width="98%"
                            style="max-width: 500px" textmode="MultiLine" rows="2" maxlength="200" wrap="true"></IP:AdvancedTextBox></asp:TableCell>
                    <asp:TableCell ColumnSpan="3" VerticalAlign="Top">
                        <asp:Label ID="_lblDescription" runat="server" Text="Description"></asp:Label><br />
                        <IP:AdvancedTextBox expandheight="true" id="_txtDescription" runat="server" width="98%"
                            style="max-width: 500px" textmode="MultiLine" rows="2" maxlength="3500" wrap="true"></IP:AdvancedTextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="Border">
                    <asp:TableCell VerticalAlign="Top" ColumnSpan="5">
                        <asp:Label ID="_lblShowHideRecurringTasks" runat="server" SkinID="LabelButton" Width="250px"></asp:Label>&nbsp;<IP:ModalIframe
                            id="_btnRecurrence" runat="server" buttonwidth="250px" displaymodalbuttontext="Recurrence Schedule"
                            width="70%" url="Popups/Recurrence.aspx" reloadpageonclose="true" allowchildtocloseparent="false"
                            height="80%" visible="false" />
                        <asp:Button ID="_btnSaveandShowRecurrence" runat="server" Text="Recurrence Schedule"
                            Width="250px" />
                        <asp:Panel ID="_pnlRecurringTasks" runat="server">
                            <asp:DataList ID="_dlRecurringTasks" runat="server" Width="100%" CellSpacing="2"
                                RepeatLayout="table" RepeatDirection="horizontal" RepeatColumns="5" EnableViewState="true">
                                <ItemTemplate>
                                    <asp:HyperLink ID="_hypRecurringTasks" runat="server" BorderWidth="0" EnableViewState="true"
                                        NavigateUrl='<%# DirectCast(Container.DataItem, System.Data.DataRowView)("URL") %>'
                                        Text='<%# DirectCast(Container.DataItem, System.Data.DataRowView)("taskdate") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:DataList>
                            <asp:Label ID="_lblNoRecurringTasks" runat="server" Text="None" Visible="false"></asp:Label>
                        </asp:Panel>
                        <ajaxToolkit:CollapsiblePanelExtender id="_cpeRecurringTasks" runat="server" collapsedsize="0"
                            collapsed="true" collapsecontrolid="_lblShowHideRecurringTasks" collapsedtext="+ Show Recurring Tasks"
                            expandedtext="- Hide Recurring Tasks" expandcontrolid="_lblShowHideRecurringTasks"
                            expanddirection="Vertical" targetcontrolid="_pnlRecurringTasks" textlabelid="_lblShowHideRecurringTasks">
                        </ajaxToolkit:CollapsiblePanelExtender>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="Border">
                    <asp:TableCell ColumnSpan="5">
                        <asp:Label ID="_lblShowHideAttachments" runat="server" Text="" SkinID="LabelButton"
                            Width="250px"></asp:Label>&nbsp;<IP:ModalIframe id="_btnAttachments" runat="server"
                                buttonwidth="250px" width="90%" displaymodalbuttontext="Attachments and Links"
                                url="Attachments.aspx" height="80%" bannertext="<%$IPResources:Global,Attachments%>"
                                reloadpageonclose="true" visible="false" />
                        <asp:Button ID="_btnSaveandShowAttachments" runat="server" Text="Attachments and Links"
                            Width="250px" />
                        <asp:Panel ID="_pnlAttachments" runat="server" ScrollBars="auto">
                            <IP:FileAttachments id="_fa" width="98%" allowedit="false" runat="server" allowattachmentupload="false" />
                        </asp:Panel>
                        <ajaxToolkit:CollapsiblePanelExtender id="_cpeAttachments" runat="server" collapsedsize="0"
                            collapsed="true" SuppressPostBack="true" collapsecontrolid="_lblShowHideAttachments"
                            collapsedtext="Show Attachments and Links" expandedtext="Hide Attachments and Links"
                            expandcontrolid="_lblShowHideAttachments" expanddirection="Vertical" targetcontrolid="_pnlAttachments"
                            textlabelid="_lblShowHideAttachments">
                        </ajaxToolkit:CollapsiblePanelExtender>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="Border">
                    <asp:TableCell ColumnSpan="1" VerticalAlign="top">
                        <asp:Label ID="_lblShowHideComments" Text="+ Show Comments" runat="server" SkinID="LabelButton"
                            Width="250px"></asp:Label>&nbsp;
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="4" VerticalAlign="top">
                        <asp:Label ID="_lblComments" runat="server" Text="Comments"></asp:Label><br />
                        <IP:AdvancedTextBox expandheight="true" id="_txtComments" runat="server" width="600px"
                            style="max-width: 1100px" textmode="multiLine" rows="4" maxlength="3500" wrap="true"></IP:AdvancedTextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="Border">
                    <asp:TableCell ColumnSpan="5">
                        <asp:Panel ID="_pnlComments" runat="server" Visible="true" Width="100%">
                            <div>
                                <asp:GridView ID="_gvComments" runat="server" AutoGenerateColumns="false" Width="98%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Comments" ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <div class="break-word" style="width: 800px">
                                                    <asp:Literal ID="_comments" runat="server" Text='<%#Bind("Comments") %>'></asp:Literal>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="LastUpdateDate" HeaderText="Last Updated" ItemStyle-VerticalAlign="Top" />
                                        <asp:BoundField DataField="LastUpdateUsername" HeaderText="Last Updated By" ItemStyle-VerticalAlign="Top" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <ajaxToolkit:CollapsiblePanelExtender id="_cpeComments" runat="server" collapsedsize="0"
                            collapsed="true" suppresspostback="true" collapsecontrolid="_lblShowHideComments"
                            collapsedtext="+ Show Comments" expandedtext="- Hide Comments" expandcontrolid="_lblShowHideComments"
                            expanddirection="Vertical" targetcontrolid="_pnlComments" textlabelid="_lblShowHideComments">
                        </ajaxToolkit:CollapsiblePanelExtender>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="Border">
                    <asp:TableCell ColumnSpan="5">
                        <asp:Table ID="Table2" CellPadding="0" CellSpacing="0" BackColor="white" BorderWidth="0"
                            Width="100%" runat="server">
                            <asp:TableRow CssClass="Border">
                               <asp:TableCell Width="25%">
                                    <asp:Label ID="Label1" runat="server" Text="Created By"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="25%">
                                    <asp:Label ID="Label3" runat="server" Text="Creation Date"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="25%" ColumnSpan="2">
                                    <asp:Label ID="Label5" runat="server" Text="Last Updated By"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="25%">
                                    <asp:Label ID="Label7" runat="server" Text="Last Update Date"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="25%">
                                    <asp:Label ID="_lblCreatedBy" runat="server" Text="None"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="25%">
                                    <asp:Label ID="_lblCreatedDate" runat="server" Text="None"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="25%" ColumnSpan="2">
                                    <asp:Label ID="_lblLastUpdatedBy" runat="server" Text="None"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="25%">
                                    <asp:Label ID="_lblLastUpdateDate" runat="server" Text="None"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <IP:MessageBox id="_msgTaskDetail" runat="server" allowpostback="false" buttontype="oK"
                message="" title="Template Task Detail" />
            <asp:PlaceHolder ID="_phTaskDetails" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </Asp:UpdatePanel>
    <br />
    <br />
    <Asp:UpdatePanel id="_udpTemplateTaskItems" runat="server" updatemode="conditional">
        <ContentTemplate>
            <div enableviewstate="false">
                <iframe id="_ifrTaskItems" runat="server" width="100%" frameborder="0" style="display: none;">
                </iframe>
                <div id="preload-img">
                    <h1>
                        Loading...</h1>
                </div>
            </div>
        </ContentTemplate>
    </Asp:UpdatePanel>
</asp:Content>
