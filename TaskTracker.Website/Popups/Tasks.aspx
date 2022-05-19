<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Tasks.aspx.vb" Inherits="PopupsTasks" %>

<%@ Register Src="~/User Controls/Comments.ascx" TagPrefix="IP" TagName="Comments" %>
<%@ Register Src="~/User Controls/Tasks/TaskItems.ascx" TagName="TaskItems" TagPrefix="IP" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Untitled Page</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="_scriptManager" AsyncPostBackTimeout="10000"
                EnablePageMethods="true" LoadScriptsBeforeUI="true" ScriptMode="release" runat="server"
                EnablePartialRendering="true">
            </asp:ScriptManager>
            <div id="errorModal" class="modal fade errorModal" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="valModalTitle">
                                <asp:Localize runat="server" Text="<%$IPResources:Global,Validation Warning%>"></asp:Localize></h4>
                        </div>
                        <div class="modal-body" id="valModalBody">
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <asp:UpdatePanel ID="_udpTaskDetails" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div class="panel panel-default">
                        <div class="panel-footer">
                            <div class="row">
                                <div class="col-md-4 col-md-offset-2">
                                    <div class="form-group">
                                        <asp:LinkButton ID="_btnSaveTask" runat="server" SkinID="ButtonPrimary" OnClientClick="return CheckForm('TaskDetails');"
                                            Visible="true">
                                            <asp:PlaceHolder runat="server">
                                                <span class="glyphicon glyphicon-floppy-save"></span>&nbsp;<asp:Label ID="Label9" runat="server" Text="<%$IPResources:Global,Save Subtask%>"></asp:Label>
                                            </asp:PlaceHolder>
                                        </asp:LinkButton>
                                    </div>

                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <asp:LinkButton ID="_btnCopyParentTask" runat="server" SkinID="Button"
                                            Visible="true">
                                            <asp:PlaceHolder runat="server">
                                                <span class="glyphicon glyphicon-duplicate"></span>&nbsp;<asp:Label ID="Label10" runat="server" Text="<%$IPResources:Global,Copy Parent Task%>"></asp:Label>
                                            </asp:PlaceHolder>
                                        </asp:LinkButton>
                                    </div>
                                    
                                </div>
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-3">
                                    <IP:EmployeeList ID="_ResponsiblePerson" Employeelabel="<%$IPResources:Global,Responsible Person%>"
                                        runat="server" EnableValidation="true">
                                        <validation validationgroup="TaskDetails" validationerrormessage="<%$IPResources:Global,Responsible Person is required%>"
                                            validationerrortext="*" validateemptytext="true" />
                                    </IP:EmployeeList>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="_lblPriority" runat="server" Text="<%$IPResources:Global,Priority%>"></asp:Label><br />
                                    <asp:DropDownList ID="_ddlPriority" runat="server" Width="100">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label ID="Label4" runat="server" Text="<%$IPResources:Global,Days Before Parent Due Date%>"></asp:Label><br />
                                    <asp:TextBox ID="_txtDaysBefore" runat="server" MaxLength="3" Width="50"></asp:TextBox>
                                    <asp:RangeValidator MinimumValue="0" MaximumValue="365" runat="server" ID="_rvDaysBefore"
                                        ControlToValidate="_txtDaysBefore" Text="*" Type="Integer" ErrorMessage="<%$IPResources:Global,Days Before Parent should be between 0 and 365%>"
                                        ValidationGroup="TaskDetails"></asp:RangeValidator>
                                </div>
                                <div class="col-md-3">
                                    <asp:Label runat="server" Text="<%$IPResources:Global,Closed Date%>"></asp:Label><br />
                                    <IP:JQDatePicker ID="_ClosedDate" runat="server" ShowFromDate="false" StartDate=''
                                        FromLabel="<%$IPResources:Global,Closed Date%>" EnableValidation="false" AllowPostBack="true" />
                                </div>
                                <div class="col-md-2">
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
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="Label14" runat="server" Text="<%$IPResources:Global,Title%>"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_txtTitle"
                                        Text="*" ErrorMessage="<%$IPResources:Global,Title is required%>" ValidationGroup="TaskDetails"></asp:RequiredFieldValidator>
                                    <br />
                                    <IP:AdvancedTextBox ExpandHeight="true" ID="_txtTitle" runat="server" Width="98%"
                                        Style="max-width: 500px" TextMode="MultiLine" Rows="2" MaxLength="80" Wrap="true">
                                    </IP:AdvancedTextBox>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="_lblDescription" runat="server" Text="<%$IPResources:Global,Description%>"></asp:Label><br />

                                    <IP:AdvancedTextBox ExpandHeight="true" ID="_txtDescription" runat="server" Width="98%"
                                        Style="max-width: 500px" TextMode="MultiLine" Rows="2" MaxLength="3500" Wrap="true">
                                    </IP:AdvancedTextBox>
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="Label15" runat="server" Text="<%$IPResources:Global,Status%>"></asp:Label><br />
                                    <asp:RadioButtonList ID="_rblStatus" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                                        Style="min-width: 80%" onclick="if (this.value>0){return CheckForm('TaskDetails')} else {return true}">
                                    </asp:RadioButtonList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label6" runat="server" Text="<%$IPResources:Global,Estimated Cost%>"></asp:Label><br />
                                    <asp:TextBox ID="_txtEstimatedCost" runat="server" MaxLength="10"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="_ftbEstimatedCost" runat="server" FilterMode="ValidChars" FilterType="Numbers" TargetControlID="_txtEstimatedCost"></ajaxToolkit:FilteredTextBoxExtender>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="_lblActualCost" runat="server" Text="<%$IPResources:Global,Actual Cost%>"></asp:Label><br />
                                    <asp:TextBox ID="_txtActualCost" runat="server" MaxLength="10"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="_ftbActualCost" runat="server" FilterMode="ValidChars" FilterType="Numbers" TargetControlID="_txtActualCost"></ajaxToolkit:FilteredTextBoxExtender>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="_lblWorkOrder" runat="server" Text="<%$IPResources:Global,WO/PO%>"></asp:Label><br />
                                    <asp:TextBox ID="_txtWorkOrder" runat="server" MaxLength="10" Style="min-width: 80%">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:Label ID="_lblShowHideComments" Text="<%$IPResources:Global,Show Comments%>"
                                        runat="server" SkinID="LabelButton" Width="250px"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="_lblComments" runat="server" Text="<%$IPResources:Global,Comments%>"></asp:Label><br />
                                    <IP:AdvancedTextBox ExpandHeight="true" ID="_txtComments" runat="server" Width="90%"
                                        Style="max-width: 1100px" TextMode="multiLine" Rows="4" MaxLength="3500" Wrap="true">
                                    </IP:AdvancedTextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <asp:Panel ID="_pnlComments" runat="server" Visible="true" Width="100%">
                                        <IP:Comments runat="server" ID="_Comments" />

                                    </asp:Panel>
                                    <ajaxToolkit:CollapsiblePanelExtender ID="_cpeComments" runat="server" CollapsedSize="0"
                                        Collapsed="true" SuppressPostBack="true" CollapseControlID="_lblShowHideComments"
                                        CollapsedText="<%$IPResources:Global,Show Comments%>" ExpandedText="<%$IPResources:Global,Hide Comments%>"
                                        ExpandControlID="_lblShowHideComments" ExpandDirection="Vertical" TargetControlID="_pnlComments"
                                        TextLabelID="_lblShowHideComments"></ajaxToolkit:CollapsiblePanelExtender>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">
                                    <asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Status%>"></asp:Label><br />
                                    <asp:Label ID="_lblStatus" runat="server" Text="<%$IPResources:Global,None%>"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Created By%>"></asp:Label><br />
                                    <asp:Label ID="_lblCreatedBy" runat="server" Text="<%$IPResources:Global,None%>"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label3" runat="server" Text="<%$IPResources:Global,Creation Date%>"></asp:Label><br />
                                    <asp:Label ID="_lblCreatedDate" runat="server" Text="<%$IPResources:Global,None%>"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label5" runat="server" Text="<%$IPResources:Global,Last Updated By%>"></asp:Label><br />
                                    <asp:Label ID="_lblLastUpdatedBy" runat="server" Text="<%$IPResources:Global,None%>"></asp:Label>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="Label7" runat="server" Text="<%$IPResources:Global,Last Update Date%>"></asp:Label><br />
                                    <asp:Label ID="_lblLastUpdateDate" runat="server" Text="<%$IPResources:Global,None%>"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <IP:MessageBox ID="_msgTaskDetail" runat="server" AllowPostback="false" ButtonType="oK"
                        Message="" Title="<%$IPResources:Global,Task Detail%>" />
                    <asp:PlaceHolder ID="_phTaskDetails" runat="server"></asp:PlaceHolder>
                    <asp:ValidationSummary ID="_valSummary" runat="server" ShowMessageBox="true" ShowSummary="true"
                        EnableClientScript="true" ValidationGroup="TaskDetails" />
                    <IP:MessageBox ID="_AlertMessage" runat="server" ButtonType="OK" />
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </form>
</body>
</html>
