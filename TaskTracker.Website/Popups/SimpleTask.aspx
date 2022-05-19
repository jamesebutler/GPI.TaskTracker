<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SimpleTask.aspx.vb" Inherits="SimpleTasks" %>

<%@ Register Src="~/User Controls/Tasks/TaskItems.ascx" TagName="TaskItems" TagPrefix="IP" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
     <div class="container-fluid">
        <asp:ScriptManager ID="_scriptManager" AsyncPostBackTimeout="10000"
            EnablePageMethods="true" LoadScriptsBeforeUI="true" ScriptMode="release" runat="server"
            EnablePartialRendering="true">
        </asp:ScriptManager>
        <Asp:UpdatePanel ID="_udpTaskDetails" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:UpdateProgress AssociatedUpdatePanelID="_udpTaskDetails" runat="server" DynamicLayout="true">
                    <ProgressTemplate>
                        <asp:Panel ID="_pnlProgress" runat="server" CssClass="modalBackground" Width="100%"
                            Height="100%">
                            <br />
                            <br />
                            <br />
                            <br />
                            <center>
                                <h1>
                                    <asp:Localize runat="server" Text="<%$IPResources:Global,Please Wait%>"></asp:Localize>
                                </h1>
                            </center>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <ajaxToolkit:AlwaysVisibleControlExtender ID="_avcProgress" runat="server" TargetControlID="_pnlProgress"
                    HorizontalSide="left" VerticalSide="top">
                </ajaxToolkit:AlwaysVisibleControlExtender>
                <asp:Table ID="_tblTask" runat="server" Width="100%" BackColor="white">
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="5" CssClass="BorderHeader" VerticalAlign="Middle" HorizontalAlign="center">
                            <asp:Button ID="_btnSaveTask" runat="server" Text="<%$IPResources:Global,Save task%>"
                                Height="30px" OnClientClick="return CheckForm('TaskDetails');" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow CssClass="Border">
                        <asp:TableCell VerticalAlign="Top" Width="20%">
                            <IP:EmployeeList ID="_ResponsiblePerson" Employeelabel="<%$IPResources:Global,Responsible Person%>"
                                runat="server" EnableValidation="true">
                                <Validation ValidationGroup="TaskDetails" ValidationErrorMessage="<%$IPResources:Global,Responsible Person is required%>"
                                    ValidationErrorText="*" ValidateEmptyText="true" />
                            </IP:EmployeeList>
                        </asp:TableCell>
                        <asp:TableCell VerticalAlign="Top" Width="20%">
                            <asp:Label ID="_lblPriority" runat="server" Text="<%$IPResources:Global,Priority%>"></asp:Label><br />
                            <asp:DropDownList ID="_ddlPriority" runat="server" Width="100">
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell VerticalAlign="Top" Width="20%">
                            <IP:JQDatePicker ID="_EstimatedDueDate" runat="server" StartDate='' ShowFromDate="false"
                            FromLabel="<%$IPResources:Global,Due Date%>" AllowDateCritical="true" EnableValidation="true">
                            <Validation ValidationErrorMessage="<%$IPResources:Global,Due Date is required%>"
                                ValidationErrorText="" ValidateEmptyText="true" ValidationGroup="TaskDetails" />
                        </IP:JQDatePicker>
                        </asp:TableCell>
                        <asp:TableCell VerticalAlign="Top" Width="20%">
                            <IP:JQDatePicker ID="_ClosedDate" runat="server" ShowFromDate="false" StartDate=''
                                FromLabel="<%$IPResources:Global,Closed Date%>" EnableValidation="false" AllowPostBack="true" />
                        </asp:TableCell>
                        <asp:TableCell VerticalAlign="Top" Width="20%">
                            <asp:Label ID="_lblLeadTime" runat="server" Text="<%$IPResources:Global,Lead Time (Days)%>"></asp:Label>
                            <asp:RequiredFieldValidator ID="_rfvLeadTime" runat="server" ControlToValidate="_txtLeadTime"
                                Text="*" ErrorMessage="<%$IPResources:Global,Lead Time is required%>" ValidationGroup="TaskDetails"></asp:RequiredFieldValidator>
                            <br />
                            <asp:TextBox ID="_txtLeadTime" runat="server" Width="50" Text='0' MaxLength="4"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server"
                                TargetControlID="_txtLeadTime" FilterType="Numbers" />
                            <asp:RangeValidator MinimumValue="0" MaximumValue="9999" runat="server" ID="_rvLeadTime"
                                ControlToValidate="_txtLeadTime" Text="*" Type="Integer" ErrorMessage="<%$IPResources:Global,Lead Time Days should be between 0 and 9999%>"
                                ValidationGroup="TaskDetails"></asp:RangeValidator>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow CssClass="Border">
                        <asp:TableCell ColumnSpan="2" VerticalAlign="Top">
                            <asp:Label ID="Label14" runat="server" Text="<%$IPResources:Global,Title%>"></asp:Label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_txtTitle"
                                Text="*" ErrorMessage="<%$IPResources:Global,Title is required%>" ValidationGroup="TaskDetails"></asp:RequiredFieldValidator>
                            <br />
                            <IP:AdvancedTextBox ExpandHeight="true" ID="_txtTitle" runat="server" Width="98%"
                                Style="max-width: 500px" TextMode="MultiLine" Rows="2" MaxLength="80" Wrap="true"></IP:AdvancedTextBox></asp:TableCell>
                        <asp:TableCell ColumnSpan="3" VerticalAlign="Top">
                            <asp:Label ID="_lblDescription" runat="server" Text="<%$IPResources:Global,Description%>"></asp:Label><br />
                            <%--<asp:TextBox ID="_txtDescriptiond" runat="server" Width="98%" Style="max-width: 600px"
                                TextMode="MultiLine" Rows="2"></asp:TextBox>--%>
                            <IP:AdvancedTextBox ExpandHeight="true" ID="_txtDescription" runat="server" Width="98%"
                                Style="max-width: 500px" TextMode="MultiLine" Rows="2" MaxLength="3500" Wrap="true"></IP:AdvancedTextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow CssClass="Border">
                        <asp:TableCell ColumnSpan="4">
                            <asp:Label ID="Label15" runat="server" Text="<%$IPResources:Global,Status%>"></asp:Label><br />
                            <asp:RadioButtonList ID="_rblStatus" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                                Style="min-width: 80%" onclick="if (this.value>0){return CheckForm('TaskDetails')} else {return true}">
                            </asp:RadioButtonList>
                        </asp:TableCell>
                        <asp:TableCell ColumnSpan="1">
                            <asp:Label ID="_lblWorkOrder" runat="server" Text="<%$IPResources:Global,WO/PO%>"></asp:Label><br />
                            <asp:TextBox ID="_txtWorkOrder" runat="server" MaxLength="10" Style="min-width: 80%">
                            </asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow CssClass="Border">
                        <asp:TableCell ColumnSpan="1" VerticalAlign="top">
                            <asp:Label ID="_lblShowHideComments" Text="<%$IPResources:Global,Show Comments%>"
                                runat="server" SkinID="LabelButton" Width="250px"></asp:Label>&nbsp;
                        </asp:TableCell>
                        <asp:TableCell ColumnSpan="4" VerticalAlign="top">
                            <asp:Label ID="_lblComments" runat="server" Text="<%$IPResources:Global,Comments%>"></asp:Label><br />
                            <IP:AdvancedTextBox ExpandHeight="true" ID="_txtComments" runat="server" Width="500px"
                                Style="max-width: 1100px" TextMode="multiLine" Rows="4" MaxLength="3500" Wrap="true"></IP:AdvancedTextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow CssClass="Border">
                        <asp:TableCell ColumnSpan="5">
                            <asp:Panel ID="_pnlComments" runat="server" Visible="true" Width="100%">
                                <div>
                                    <asp:GridView ID="_gvComments" runat="server" AutoGenerateColumns="false" Width="98%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$IPResources:Global,Comments%>" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <div class="break-word" style="width: 800px">
                                                        <asp:Literal ID="_comments" runat="server" Text='<%#Bind("Comments") %>'></asp:Literal>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$IPResources:Global,Last Updated%>" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <div>
                                                        <asp:Literal ID="_comments" runat="server" Text='<%#IP.Bids.SharedFunctions.FormatDate(cdate(DirectCast(Container.DataItem, IP.MEAS.BO.TaskItemComments).LastUpdateDate)) %>'></asp:Literal>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="LastUpdateUsername" HeaderText="<%$IPResources:Global,Last Updated By%>"
                                                ItemStyle-VerticalAlign="Top" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                            <ajaxToolkit:CollapsiblePanelExtender ID="_cpeComments" runat="server" CollapsedSize="0"
                                Collapsed="true" SuppressPostBack="true" CollapseControlID="_lblShowHideComments"
                                CollapsedText="<%$IPResources:Global,Show Comments%>" ExpandedText="<%$IPResources:Global,Hide Comments%>"
                                ExpandControlID="_lblShowHideComments" ExpandDirection="Vertical" TargetControlID="_pnlComments"
                                TextLabelID="_lblShowHideComments">
                            </ajaxToolkit:CollapsiblePanelExtender>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow CssClass="Border">
                        <asp:TableCell ColumnSpan="5">
                            <asp:Table ID="Table2" CellPadding="0" CellSpacing="0" BackColor="white" BorderWidth="0"
                                Width="100%" runat="server">
                                <asp:TableRow CssClass="Border">
                                    <asp:TableCell Width="20%">
                                        <asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Status%>"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell Width="20%">
                                        <asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Created By%>"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell Width="20%">
                                        <asp:Label ID="Label3" runat="server" Text="<%$IPResources:Global,Creation Date%>"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell Width="20%" ColumnSpan="2">
                                        <asp:Label ID="Label5" runat="server" Text="<%$IPResources:Global,Last Updated By%>"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell Width="20%">
                                        <asp:Label ID="Label7" runat="server" Text="<%$IPResources:Global,Last Update Date%>"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell Width="20%">
                                        <asp:Label ID="_lblStatus" runat="server" Text="<%$IPResources:Global,None%>"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell Width="20%">
                                        <asp:Label ID="_lblCreatedBy" runat="server" Text="<%$IPResources:Global,None%>"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell Width="20%">
                                        <asp:Label ID="_lblCreatedDate" runat="server" Text="<%$IPResources:Global,None%>"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell Width="20%" ColumnSpan="2">
                                        <asp:Label ID="_lblLastUpdatedBy" runat="server" Text="<%$IPResources:Global,None%>"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell Width="20%">
                                        <asp:Label ID="_lblLastUpdateDate" runat="server" Text="<%$IPResources:Global,None%>"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <%--<asp:TableRow CssClass="Border">
            <asp:TableCell ColumnSpan="5">
                <center>
                    &nbsp; &nbsp;<br />
                   
                    
                   
                </center>
            </asp:TableCell>
        </asp:TableRow>--%>
                </asp:Table>
                <IP:MessageBox ID="_msgTaskDetail" runat="server" AllowPostback="false" ButtonType="oK"
                    Message="" Title="<%$IPResources:Global,Task Detail%>" />
                <asp:PlaceHolder ID="_phTaskDetails" runat="server"></asp:PlaceHolder>
                <asp:ValidationSummary ID="_valSummary" runat="server" ShowMessageBox="true" ShowSummary="true"
                    EnableClientScript="true" ValidationGroup="TaskDetails" />
                <IP:MessageBox ID="_AlertMessage" runat="server" ButtonType="OK" />
            </ContentTemplate>
        </Asp:UpdatePanel>
        
    </div>
    </form>
</body>
</html>
