<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" 
    AutoEventWireup="false" CodeFile="TaskDetailsGPI.aspx.vb" Trace="false" Inherits="TaskDetailsGPI"  %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<%@ Register Src="~/User Controls/FileAttachments.ascx" TagName="FileAttachments"
    TagPrefix="IP" %>
<%@ Register Src="~/User Controls/Tasks/TaskItems.ascx" TagName="TaskItems" TagPrefix="IP" %>


<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <asp:UpdatePanel ID="_udpTaskDetails" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="_lblHeaderTitle" runat="server"></asp:Label>
                    </div>
                </div>

                <div class="panel-footer">
                    <div class="row">
                        <div class="col-xs-12 col-sm-4 col-md-2">
                            <div class="form-group">
                                <asp:LinkButton ID="_btnSaveTask" runat="server" SkinID="ButtonPrimary" OnClientClick="return CheckForm('TaskDetails');">
                                    <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-floppy-save"></span>&nbsp;<asp:Label ID="Label11" runat="server" Text="<%$IPResources:Global,Save Task%>"></asp:Label></asp:PlaceHolder>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-10 col-md-10">
                            <div class="form-group">
                                <asp:RadioButtonList ID="_rblUpdateTasks" runat="server" RepeatDirection="Horizontal"
                                    CellSpacing="5" Font-Bold="true" Width="60%">
                                    <asp:ListItem Text="<%$IPResources:Global,Only Update this task%>" Selected="true"
                                        Value="Current"></asp:ListItem>
                                    <asp:ListItem Text="<%$IPResources:Global,Update tasks from Due Date forward%>" Value="Future"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                    <div class="row">

                        <div class="col-xs-12 col-sm-4 col-md-4 col-lg-2">
                            <div class="form-group">
                              
                                <asp:LinkButton ID="_btnAddTask" runat="server" SkinID="Button">
                                    <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-plus"></span>&nbsp;<asp:Label ID="Label9" runat="server" Text="<%$IPResources:Global,Add Task%>"></asp:Label></asp:PlaceHolder>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-4 col-md-4 col-lg-2">
                            <div class="form-group">
                                <IP:ModalIframe ID="_btnCopyTask" runat="server" Width="90%" DisplayModalButtonText="<%$IPResources:Global,Copy Task%>"
                                    Url="Popups/CopyTask.aspx" Height="500" GlyphiconValue="glyphicon glyphicon-copy" BannerText="<%$IPResources:Global,Copy Task%>"
                                    ReloadPageOnClose="true" style="max-width: 1000px;" />
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-4 col-md-4 col-lg-2">
                            <div class="form-group">
                                <asp:LinkButton ID="_btnDelete" runat="server" SkinID="Button">
                                    <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-trash"></span>&nbsp;<asp:Label ID="Label10" runat="server" Text="<%$IPResources:Global,Delete Task%>"></asp:Label></asp:PlaceHolder>
                                </asp:LinkButton>
                                <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="_btnDelete"
                                    ConfirmText="<%$IPResources:Global,Are you sure that you would like to delete this task?%>"></ajaxToolkit:ConfirmButtonExtender>
                            </div>
                        </div>

                        <div class="col-xs-12 col-sm-4 col-md-4 col-lg-2">
                            <div class="form-group">
                                <IP:ModalIframe ID="_btnHistory" runat="server" Width="90%" DisplayModalButtonText="<%$IPResources:Global,Task History%>"
                                    Url="Popups/TaskHistory.aspx" Height="500" ButtonHeight="30px" BannerText="<%$IPResources:Global,History%>"
                                    ReloadPageOnClose="false" style="max-width: 1000px;" enabled="false" GlyphiconValue="glyphicon glyphicon-header" />
                            </div>
                        </div>

                        <div class="col-xs-12 col-sm-4 col-md-4 col-lg-2">
                            <div class="form-group">
                                <asp:LinkButton ID="_btnReturnToTaskList" runat="server" SkinID="Button" Visible="false">
                                    <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-step-backward"></span>&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$IPResources:Global,Return to Task List%>"></asp:Label></asp:PlaceHolder>
                                </asp:LinkButton>
                                <asp:LinkButton ID="_btnReturnToTaskSearch" runat="server" SkinID="Button" Visible="false">
                                    <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-step-backward"></span>&nbsp;<asp:Label ID="Label15" runat="server" Text="<%$IPResources:Global,Return to Task List%>"></asp:Label></asp:PlaceHolder>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-4 col-md-4 col-lg-2">
                            <div class="form-group">
                                <asp:LinkButton ID="_btnReturnToSourceSystem" runat="server" SkinID="Button" Visible="false">
                                    <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-new-window"></span>&nbsp;<asp:Label ID="_lblReturnToSourceSystem" runat="server" Text="<%$IPResources:Global,Return to Task List%>"></asp:Label></asp:PlaceHolder>
                                </asp:LinkButton>
                                <asp:LinkButton ID="_btnTaskHeader" runat="server" SkinID="Button" Visible="false">
                                    <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-step-backward"></span>&nbsp;<asp:Label ID="Label12" runat="server" Text="<%$IPResources:Global,Return to Task Header%>"></asp:Label></asp:PlaceHolder>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-4 col-md-4 col-lg-2">
                            <div class="form-group">
                                <IP:ModalIframe ID="_btnAssignReplication" runat="server" Width="800px" DisplayModalButtonText="<%$IPResources:Global,Assign Replication%>"
                                    Url="Popups/ReplicationAssignment.aspx" Height="500" ButtonHeight="30px" BannerText="<%$IPResources:Global,Assign/View Replication%>"
                                    ReloadPageOnClose="true" GlyphiconValue="glyphicon glyphicon-duplicate" />
                            </div>
                        </div>
                    </div>

                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12 col-md-6 col-lg-3">
                            <div class="form-group">
                                <IP:EmployeeList ID="_ResponsiblePerson" Employeelabel="<%$IPResources:Global,Responsible Person%>"
                                    runat="server" EnableValidation="true" DisplayClearLink="false">
                                    <validation validationgroup="TaskDetails" validationerrormessage="<%$IPResources:Global,Responsible Person is required%>"
                                        validationerrortext="" validateemptytext="true" />
                                </IP:EmployeeList>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-3 col-lg-2">
                            <div class="form-group">
                                <asp:Label Text="*" CssClass="labelerror" runat="server"></asp:Label>&nbsp;<asp:Label
                                    ID="_lblPriority" runat="server" Text="<%$IPResources:Global,Priority%>"></asp:Label><br />
                                <asp:DropDownList ID="_ddlPriority" runat="server" Width="80">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-3 col-lg-2">
                            <div class="form-group">
                                <asp:Label Text="*" CssClass="labelerror" runat="server"></asp:Label>&nbsp;<asp:Label runat="server" Text="<%$IPResources:Global,Due Date%>"></asp:Label><br />
                                <IP:JQDatePicker ID="_EstimatedDueDate" runat="server" StartDate='' ShowFromDate="false"
                                    FromLabel="<%$IPResources:Global,Due Date%>" AllowDateCritical="true" EnableValidation="true">
                                    <validation validationerrormessage="<%$IPResources:Global,Due Date is required%>"
                                        validationerrortext="" validateemptytext="true" validationgroup="TaskDetails" />
                                </IP:JQDatePicker>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-3 col-lg-2">
                            <div class="form-group">
                                <asp:HyperLink CssClass="toolTip" NavigateUrl="#" ID="_hypClosedateToolTip" runat="server">
                                    <asp:Label runat="server" Text="<%$IPResources:Global,Closed Date%>"></asp:Label><br />
                                    <IP:JQDatePicker ID="_ClosedDate" runat="server"  StartDate='' ShowFromDate="false"
                                        FromLabel="<%$IPResources:Global,Closed Date%>" EnableValidation="false" AllowPostBack="true" />
                                    <asp:Label ID="_lblClosedDateTip" runat="server" CssClass="tip">
                            
                                <br />***&nbsp;
                                <asp:Localize Text="<%$IPResources:Global,This task cannot be closed until all of the Sub Tasks have been closed%>" runat=server></asp:Localize>&nbsp;***
                                <br />
                                <br />
                                    </asp:Label>
                                </asp:HyperLink>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-3 col-lg-2">
                            <div class="form-group">
                                <asp:Label ID="_lblLeadTime" runat="server" Text="<%$IPResources:Global,Lead Time (Days)%>"></asp:Label>
                                <asp:RequiredFieldValidator ID="_rfvLeadTime" runat="server" ControlToValidate="_txtLeadTime"
                                    Text="*" Display="Dynamic" ErrorMessage="<%$IPResources:Global,Lead Time is required%>"
                                    ValidationGroup="TaskDetails"></asp:RequiredFieldValidator>
                                <br />
                                <asp:TextBox ID="_txtLeadTime" runat="server" Width="45" Text='0' MaxLength="4"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server"
                                    TargetControlID="_txtLeadTime" FilterType="Numbers" />
                                <asp:RangeValidator MinimumValue="0" MaximumValue="9999" runat="server" ID="_rvLeadTime"
                                    ControlToValidate="_txtLeadTime" Text="*" Type="Integer" ErrorMessage="<%$IPResources:Global,Lead Time Days should be between 0 and 9999%>"
                                    ValidationGroup="TaskDetails"></asp:RangeValidator>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-md-6">
                            <div class="form-group">
                                <asp:Label ID="Label4" Text="*" CssClass="labelerror" runat="server"></asp:Label>&nbsp;<asp:Label
                                    ID="Label14" runat="server" Text="<%$IPResources:Global,Title%>"></asp:Label>
                                <asp:RequiredFieldValidator ID="_rfvTitle" runat="server" ControlToValidate="_txtTitle"
                                    Text="" ErrorMessage="<%$IPResources:Global,Title is required%>" ValidationGroup="TaskDetails"
                                    Display="Static"></asp:RequiredFieldValidator>
                                <br />
                                <IP:AdvancedTextBox ExpandHeight="true" ID="_txtTitle" runat="server" Width="98%"
                                    TextMode="MultiLine" Rows="2" MaxLength="80" Wrap="true">
                                </IP:AdvancedTextBox>
                                <br />
                                <asp:Label ID="_lblTranslatedTitle" runat="server"></asp:Label>
                                <asp:DropDownList ID="_ddlInspectionTypeList" runat="server" Visible="false"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-6">
                            <div class="form-group">
                                <asp:Label ID="_lblDescription" runat="server" Text="<%$IPResources:Global,Description%>"></asp:Label><br />
                                <IP:AdvancedTextBox ExpandHeight="true" ID="_txtDescription" runat="server" Width="98%"
                                    TextMode="MultiLine" Rows="2" MaxLength="4000" Wrap="true">
                                </IP:AdvancedTextBox>
                                <br />
                                <asp:Label ID="_lblTranslatedDescription" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-md-6">
                            <div class="form-group">
                                <asp:Label ID="_lblTaskStatus" runat="server" Text="<%$IPResources:Global,Status%>"></asp:Label>&nbsp;<center>
                            <asp:Label ID="_lblTaskStatusMessage" CssClass="Message" runat="server" Text=""></asp:Label></center>
                                <asp:RadioButtonList ID="_rblStatus" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                                    Style="min-width: 80%" onclick="if (this.value>0){return CheckForm('TaskDetails')} else {return true}">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-2">
                            <div class="form-group">
                                <asp:Label ID="Label6" runat="server" Text="<%$IPResources:Global,Estimated Cost%>"></asp:Label><br />
                                <asp:TextBox ID="_txtEstimatedCost" runat="server" MaxLength="10"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="_ftbEstimatedCost" runat="server" FilterMode="ValidChars" FilterType="Numbers" TargetControlID="_txtEstimatedCost"></ajaxToolkit:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-2">
                            <div class="form-group">
                                <asp:Label ID="_lblActualCost" runat="server" Text="<%$IPResources:Global,Actual Cost%>"></asp:Label><br />
                                <asp:TextBox ID="_txtActualCost" runat="server" MaxLength="10"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="_ftbActualCost" runat="server" FilterMode="ValidChars" FilterType="Numbers" TargetControlID="_txtActualCost"></ajaxToolkit:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="col-xs-12 col-md-2">
                            <div class="form-group">
                                <asp:Label ID="_lblWorkOrder" runat="server" Text="<%$IPResources:Global,WO/PO%>"></asp:Label>
                                <br />
                                <asp:TextBox ID="_txtWorkOrder" runat="server" MaxLength="10" Style="min-width: 80%">
                                </asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-md-4 col-lg-3">
                            <asp:LinkButton ID="_lblShowHideRecurringTasks" runat="server" SkinID="Button" OnClientClick="return false">
                            </asp:LinkButton>
                            <IP:ModalIframe
                                ID="_btnRecurrence" runat="server" DisplayModalButtonText="<%$IPResources:Global,Recurrence Schedule%>"
                                Width="800px" Url="Popups/Recurrence.aspx" ReloadPageOnClose="true" AllowChildToCloseParent="false" bannertext="<%$IPResources:Global,Recurrence Schedule%>"
                                Height="600px" Visible="false" />

                            <ajaxToolkit:CollapsiblePanelExtender ID="_cpeRecurringTasks" runat="server" CollapsedSize="0"
                                Collapsed="true" CollapseControlID="_lblShowHideRecurringTasks" CollapsedText="<%$IPResources:Global,Show Recurring Tasks%>"
                                ExpandedText="<%$IPResources:Global,Hide Recurring Tasks%>" ExpandControlID="_lblShowHideRecurringTasks"
                                ExpandDirection="Vertical" TargetControlID="_pnlRecurringTasks" TextLabelID="_lblShowHideRecurringTasks"></ajaxToolkit:CollapsiblePanelExtender>

                        </div>
                        <div class="col-xs-12 col-md-4 col-lg-3">
                            <asp:LinkButton ID="_btnSaveandShowRecurrence" runat="server" SkinID="Button">
                                <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-repeat"></span>&nbsp;<asp:Label ID="Label16" runat="server" Text="<%$IPResources:Global,Recurrence Schedule%>"></asp:Label></asp:PlaceHolder>
                            </asp:LinkButton>
                        </div>
                        <div class="col-xs-12 col-md-4 col-lg-3">
                            <asp:LinkButton ID="_btnCancelRecurrence" runat="server" SkinID="Button">
                                <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-remove-sign"></span>&nbsp;<asp:Label ID="Label17" runat="server" Text="<%$IPResources:Global,End Recurrence%>"></asp:Label></asp:PlaceHolder>
                            </asp:LinkButton>
                            <ajaxToolkit:ConfirmButtonExtender ID="_cbeCancelRecurrence" runat="server" TargetControlID="_btnCancelRecurrence"
                                ConfirmText="<%$IPResources:Global,Are you sure that you would like to End the Recurrence Schedule? Doing so will delete all Open tasks in the future!%>"></ajaxToolkit:ConfirmButtonExtender>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-md-12">
                            <asp:Panel ID="_pnlRecurringTasks" runat="server" CssClass="panel panel-default">
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
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-md-4 col-lg-3">
                            <asp:LinkButton ID="_lblShowHideAttachments" runat="server" SkinID="Button" OnClientClick="return false">
                            </asp:LinkButton>
                            <IP:ModalIframe ID="_btnAttachments" runat="server"
                                ButtonWidth="300px" Width="1200px" DisplayModalButtonText="<%$IPResources:Global,Attachments and Links%>"
                                Url="Attachments.aspx" Height="600px" BannerText="<%$IPResources:Global,Attachments%>"
                                ReloadPageOnClose="true" Visible="false" />
                        </div>
                        <div class="col-xs-12 col-md-4 col-lg-3">
                            <asp:LinkButton ID="_btnSaveandShowAttachments" runat="server" SkinID="Button">
                                <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-paperclip"></span>&nbsp;<asp:Label ID="Label18" runat="server" Text="<%$IPResources:Global,Attachments and Links%>"></asp:Label></asp:PlaceHolder>
                            </asp:LinkButton>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Panel ID="_pnlAttachments" runat="server" ScrollBars="none ">
                                <IP:FileAttachments ID="_fa" AllowEdit="false" runat="server" AllowAttachmentUpload="false" />
                            </asp:Panel>
                            <ajaxToolkit:CollapsiblePanelExtender ID="_cpeAttachments" runat="server" CollapsedSize="0"
                                Collapsed="true" SuppressPostBack="true" CollapseControlID="_lblShowHideAttachments"
                                CollapsedText="<%$IPResources:Global,Show Attachments and Links%>" ExpandedText="<%$IPResources:Global,Hide Attachments and Links%>"
                                ExpandControlID="_lblShowHideAttachments" ExpandDirection="Vertical" TargetControlID="_pnlAttachments"
                                TextLabelID="_lblShowHideAttachments"></ajaxToolkit:CollapsiblePanelExtender>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-md-4 col-lg-3">
                            <asp:LinkButton ID="_lblShowHideSubtasks" runat="server" SkinID="Button" OnClientClick="return false">
                            </asp:LinkButton>
                            <IP:ModalIframe
                                ID="_btnSubTasks" runat="server" DisplayModalButtonText="<%$IPResources:Global,Subsequent Task Items%>"
                                Width="80%" Url="Popups/SubTasks.aspx" ReloadPageOnClose="true" ButtonWidth="300px"
                                Height="400px" BannerText="<%$IPResources:Global,Subsequent Task Items%>" Visible="false" />
                            <ajaxToolkit:CollapsiblePanelExtender ID="_cpeSubTasks" runat="server" CollapsedSize="0"
                                Collapsed="true" SuppressPostBack="true" CollapseControlID="_lblShowHideSubtasks"
                                CollapsedText="<%$IPResources:Global,Show Subsequent Task Items%>" ExpandedText="<%$IPResources:Global,Hide Subsequent Task Items%>"
                                ExpandControlID="_lblShowHideSubtasks" ExpandDirection="Vertical" TargetControlID="_pnlSubtask"
                                TextLabelID="_lblShowHideSubtasks"></ajaxToolkit:CollapsiblePanelExtender>
                        </div>
                        <div class="col-xs-12 col-md-4 col-lg-3">

                            <asp:LinkButton ID="_btnSaveandShowSubTasks" runat="server" SkinID="Button">
                                <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-chevron-left"></span>&nbsp;<asp:Label ID="Label19" runat="server" Text="<%$IPResources:Global,Subsequent Task Items%>"></asp:Label></asp:PlaceHolder>
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Panel ID="_pnlSubtask" CssClass="panel panel-default" runat="server">
                                <div class="panel-heading">
                                    <asp:Label ID="_lblSubTasks" runat="server" Text="<%$IPResources:Global,Subsequent Tasks%>"></asp:Label>
                                </div>
                                <br />
                                <div class="panel-body">
                                    <section id="no-more-gridView">
                                        <asp:GridView ID="_gvSubTask" runat="server" Width="90%" AutoGenerateColumns="false" CssClass="table table-striped"
                                            AutoGenerateEditButton="false" AutoGenerateDeleteButton="false" DataKeyNames="TaskItemSeqId">
                                            <Columns>
                                                <asp:BoundField HeaderText="<%$IPResources:Global,Title%>" DataField="Title" ItemStyle-VerticalAlign="Top"
                                                    ItemStyle-Wrap="true" ItemStyle-Width="30%" />
                                                <asp:BoundField HeaderText="<%$IPResources:Global,Description%>" DataField="Description"
                                                    ItemStyle-VerticalAlign="Top" ItemStyle-Wrap="true" ItemStyle-Width="40%" />
                                                <asp:TemplateField HeaderText="<%$IPResources:Global,Responsible%>" ItemStyle-VerticalAlign="Top"
                                                    ItemStyle-Wrap="true" ItemStyle-Width="25%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="_lblResponsible" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="<%$IPResources:Global,DaysAfter%>" DataField="DaysAfter"
                                                    ItemStyle-VerticalAlign="Top" ItemStyle-Wrap="true" ItemStyle-Width="5%" />
                                            </Columns>
                                        </asp:GridView>
                                    </section>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-md-4 col-lg-3">
                            <asp:LinkButton ID="_lblShowHideDependentTasks" runat="server" SkinID="Button" OnClientClick="return false">
                            </asp:LinkButton>
                            <IP:ModalIframe
                                ID="_btnDependentTaskItems" runat="server" DisplayModalButtonText="<%$IPResources:Global,Sub Task Items%>"
                                Width="80%" Url="Popups/Tasks.aspx" ReloadPageOnClose="true" ButtonWidth="300px"
                                Height="400px" BannerText="<%$IPResources:Global,Sub Task Items%>" Visible="false"
                                AllowChildToCloseParent="true" />
                        </div>
                        <div class="col-xs-12 col-md-4 col-lg-3">
                            <asp:LinkButton ID="_btnSaveAndShowDependentTaskItems" runat="server" SkinID="Button">
                                <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-chevron-right"></span>&nbsp;<asp:Label ID="Label20" runat="server" Text="<%$IPResources:Global,Sub Task Items%>"></asp:Label></asp:PlaceHolder>
                            </asp:LinkButton>

                            <ajaxToolkit:CollapsiblePanelExtender ID="_cpeDependentTasks" runat="server" CollapsedSize="0"
                                Collapsed="true" SuppressPostBack="true" CollapseControlID="_lblShowHideDependentTasks"
                                CollapsedText="<%$IPResources:Global,Show Sub Task Items%>" ExpandedText="<%$IPResources:Global,Hide Sub Task Items%>"
                                ExpandControlID="_lblShowHideDependentTasks" ExpandDirection="Vertical" TargetControlID="_pnlDependentTasks"
                                TextLabelID="_lblShowHideDependentTasks"></ajaxToolkit:CollapsiblePanelExtender>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Panel ID="_pnlDependentTasks" runat="server" ScrollBars="Auto">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="always">
                                    <ContentTemplate>
                                        <IP:TaskItems ID="_dependenttask" runat="server" AllowTasksToBeFiltered="false" AllowDelete="true" AllowEdit="true"
                                            InFrame="true" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-md-4 col-lg-3">
                            <asp:LinkButton ID="_lblShowHideComments" runat="server" SkinID="Button" OnClientClick="return false">
                            </asp:LinkButton>
                        </div>
                        <div class="col-xs-12 col-md-4 col-lg-3">
                            <IP:ModalIframe ID="_btnDisplayComments" runat="server" DisplayModalButtonText="<%$IPResources:Global,Edit Comments%>" Width="80%" Url="Popups/EditComments.aspx" ReloadPageOnClose="true" ButtonWidth="300px" Height="400px" BannerText="<%$IPResources:Global,Edit Comments%>" Visible="false" />
                            <asp:LinkButton ID="_btnEditComments" runat="server" SkinID="Button">
                                <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-pencil"></span>&nbsp;<asp:Label ID="Label21" runat="server" Text="<%$IPResources:Global,Edit Comments%>"></asp:Label></asp:PlaceHolder>
                            </asp:LinkButton>
                        </div>
                        <div class="col-xs-12 col-md-8 col-lg-6">
                            <asp:Label ID="_lblComments" runat="server" Text="<%$IPResources:Global,Comments%>"></asp:Label><br />
                            <IP:AdvancedTextBox ExpandHeight="true" ID="_txtComments" runat="server" Width="100%"
                                TextMode="multiLine" Rows="4" MaxLength="4000" Wrap="true">
                            </IP:AdvancedTextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Panel ID="_pnlComments" runat="server" Visible="true" Width="100%">
                                <div>
                                    <asp:GridView ID="_gvComments" runat="server" AutoGenerateColumns="false" Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$IPResources:Global,Comments%>" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <div class="break-word" style="width: 98%; max-width: 800px; min-width: 300px;">
                                                        <asp:Literal ID="_comments" runat="server" Text='<%#Bind("Comments") %>'></asp:Literal>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$IPResources:Global,Last Updated%>" ItemStyle-VerticalAlign="Top">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" Text='<%#IP.Bids.SharedFunctions.FormatDate(CDate(DirectCast(Container.DataItem, IP.MEAS.BO.TaskItemComments).LastUpdateDate)) %>'></asp:Literal>
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
                                TextLabelID="_lblShowHideComments"></ajaxToolkit:CollapsiblePanelExtender>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-xs-6 col-md-2 col-md-offset-1">
                            <asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Status%>"></asp:Label><br />
                            <asp:Label ID="_lblStatus" Font-Bold="false" runat="server" Text="<%$IPResources:Global,None%>" EnableTheming="false"></asp:Label>
                        </div>
                        <div class="col-xs-6 col-md-2">
                            <asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Created By%>"></asp:Label><br />
                            <asp:Label ID="_lblCreatedBy" Font-Bold="false" runat="server" Text="<%$IPResources:Global,None%>" EnableTheming="false"></asp:Label>
                        </div>
                        <div class="col-xs-6 col-md-2">
                            <asp:Label ID="Label3" runat="server" Text="<%$IPResources:Global,Creation Date%>"></asp:Label><br />
                            <asp:Label ID="_lblCreatedDate" Font-Bold="false" runat="server" Text="<%$IPResources:Global,None%>" EnableTheming="false"></asp:Label>
                        </div>
                        <div class="col-xs-6 col-md-2">
                            <asp:Label ID="Label5" runat="server" Text="<%$IPResources:Global,Last Updated By%>"></asp:Label><br />
                            <asp:Label ID="_lblLastUpdatedBy" Font-Bold="false" runat="server" Text="<%$IPResources:Global,None%>" EnableTheming="false"></asp:Label>
                        </div>
                        <div class="col-xs-6 col-md-2">
                            <asp:Label ID="Label7" runat="server" Text="<%$IPResources:Global,Last Update Date%>"></asp:Label><br />
                            <asp:Label ID="_lblLastUpdateDate" Font-Bold="false" runat="server" Text="<%$IPResources:Global,None%>" EnableTheming="false"></asp:Label>
                        </div>
                        <div class="col-xs-6 col-md-2">
                        </div>
                    </div>
                </div>
            </div>
            <IP:MessageBox ID="_msgTaskDetail" runat="server" AllowPostback="false" ButtonType="oK"
                Message="" Title="<%$IPResources:Global,Task Detail%>" />
            <asp:PlaceHolder ID="_phTaskDetails" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />


<%--     <ItemTemplate >
        <div class="panel" >
             
            <div class="panel-body form-inline">
                <div class="row marginBottom5">
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label1GPI" runat="server" Text="<%$IPResources:Global,Business%>"></asp:Label>
                        <br class="hidden-xs" />
                        <span class="visible-xs-inline">:</span>
                        <asp:Label ID="_lblDataBusiness" runat="server" Font-Bold="false" EnableTheming="false"
                            Text=""></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label2GPI" runat="server" Text="<%$IPResources:Global,Region%>"></asp:Label>
                         <br class="hidden-xs" />
                        <span class="visible-xs-inline">:</span>
                        <asp:Label ID="_lblDataRegion" runat="server" Font-Bold="false" EnableTheming="false"
                            Text=""></asp:Label>
                    </div>

                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label3GPI" runat="server" Text="<%$IPResources:Global,Facility%>"></asp:Label>
                         <br class="hidden-xs" />
                        <span class="visible-xs-inline">:</span>
                        <asp:Label ID="_lblDataFacilityGPI" runat="server" Font-Bold="false" EnableTheming="false"
                            Text=""></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label4GPI" runat="server" Text="<%$IPResources:Global,Business Unit-Area/Department%>"></asp:Label>
                        <br class="hidden-xs" />
                        <span class="visible-xs-inline">:</span>
                        <asp:Label ID="_lblDataBusAreaGPI" runat="server" Font-Bold="false" EnableTheming="false"
                            Text=""></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label5GPI" runat="server" Text="<%$IPResources:Global,Line/Machine%>"></asp:Label>
                        <br class="hidden-xs" />
                        <span class="visible-xs-inline">:</span>
                        <asp:Label ID="_lblDataLineGPI" runat="server" Font-Bold="false" EnableTheming="false"
                            Text=""></asp:Label>
                    </div>
                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label6GPI" runat="server" Text=""></asp:Label>                        
                        <span class="visible-xs-inline">:</span><br />
                        <asp:Label ID="_lblDataDateSelectionGPI" runat="server" Font-Bold="false" EnableTheming="false"
                            Text=""></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label7GPI" runat="server" Text=""></asp:Label>
                        #
                        <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblSourceSystemGPI" runat="server" Font-Bold="false" EnableTheming="false"
                            Text=""></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-4">
                        <asp:Label ID="Label8GPI" runat="server" Text=""></asp:Label>
                        <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="Label11GPI" runat="server" Font-Bold="false" EnableTheming="false" Text=""></asp:Label>
                    </div>
                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12">
                        <asp:Label ID="Label9GPI" runat="server" Text=""></asp:Label>
                        <span class="visible-xs-inline">:</span>
                        <br />
                        <asp:Label ID="_lblDataTitleGPI" runat="server" Font-Bold="false" EnableTheming="false"
                            Text=""></asp:Label>
                       <br /> <asp:Label ID="_lblTranslatedTitleGPI" runat="server" Font-Bold="false" EnableTheming ="false"  Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12">
                        <asp:Label ID="Label10GPI" runat="server" Text="<%$IPResources:Global,Description%>"></asp:Label>
                        <span class="visible-xs-inline">:</span>
                        <br />
                        <asp:Label ID="_lblDataDescriptionGPI" runat="server" Font-Bold="false" EnableTheming="false"
                            Text=""></asp:Label>
                        <br /><asp:Label ID="_lblTranslatedDescriptionGPI" runat="server" Font-Bold="false" EnableTheming ="false"  Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12 col-sm-6">
                        <asp:Label ID="Label12GPI" runat="server" Text="<%$IPResources:Global,Types%>"></asp:Label>
                         <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblDataTypesGPI" runat="server" Font-Bold="false" EnableTheming="false"
                            Text=''></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <asp:Label ID="Label13GPI" runat="server" Text="<%$IPResources:Global,Activity%>"></asp:Label>
                         <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblDataActivityGPI" runat="server" Font-Bold="false" EnableTheming="false"
                            Text=""></asp:Label>
                    </div>
                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12 col-sm-6">
                        <asp:Label ID="Label14GPI" runat="server" Text="<%$IPResources:Global,Task Type Manager%>"></asp:Label>
                         <span class="visible-xs-inline">:</span>
                        <br />
                        <asp:Label ID="Label15GPI" runat="server" Font-Bold="false" EnableTheming="false" Text=""></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <asp:Label ID="Label16GPI" runat="server" Text="<%$IPResources:Global,Business Manager%>"></asp:Label>
                         <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="Label17GPI" runat="server" Font-Bold="false" EnableTheming="false" Text=""></asp:Label>
                    </div>
                </div>
                <div class="row marginBottom5">
                    <div class="col-xs-12 col-sm-3">
                        <asp:Label ID="Label18GPI" runat="server" Text="<%$IPResources:Global,Created By%>"></asp:Label>
                         <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblCreatedByGPI" Font-Bold="false" runat="server" EnableTheming="false" Text=""></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-3">
                        <asp:Label ID="Label19GPI" runat="server" Text="<%$IPResources:Global,Creation Date%>"></asp:Label>
                        <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblCreationDateGPI" Font-Bold="false" runat="server" EnableTheming="false" Text=""></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-3">
                        <asp:Label ID="Label20GPI" runat="server" Text="<%$IPResources:Global,Last Updated By%>"></asp:Label>
                        <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblLastUpdatedByGPI" Font-Bold="false" runat="server" EnableTheming="false" Text=""></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-3">
                        <asp:Label ID="Label21GPI" runat="server" Text="<%$IPResources:Global,Last Update Date%>"></asp:Label>
                        <span class="visible-xs-inline">:</span>
                        <br class="hidden-xs" />
                        <asp:Label ID="_lblLastUpdateDateGPI" Font-Bold="false" runat="server" EnableTheming="false" Text=""></asp:Label>
                    </div>
                </div>
           
            </div>
        </div>
    </ItemTemplate>--%>




    <%--<asp:UpdatePanel ID="_udpTaskItems" runat="server" UpdateMode="conditional">
        <ContentTemplate>

            <div enableviewstate="false">
                <iframe id="_ifrTaskItems" runat="server" width="100%" frameborder="0" style="display: none;"></iframe>
                <div id="preload-img" style="display: none;">
                    <h1>
                        <asp:Localize runat="server" Text="<%$IPResources:Global, Loading%>"></asp:Localize>
                    </h1>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>


