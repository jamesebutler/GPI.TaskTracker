<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false"
    CodeFile="TaskHeader.aspx.vb" Inherits="TaskHeaderEntry" Title="Task Tracker Header"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/User Controls/Tasks/TaskHeader.ascx" TagName="TaskHeader" TagPrefix="IP" %>
<%@ Register Src="~/User Controls/EntrySite.ascx" TagName="Site" TagPrefix="IP" %>
<%@ Register Src="~/User Controls/Tasks/TaskItems.ascx" TagName="TaskItems" TagPrefix="IP" %>
<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<%@ Register Src="~/User Controls/JQDateRange.ascx" TagName="JQDatePicker" TagPrefix="IP" %>

 <%@ Register Src="~/User Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="IP" %>


                  
<asp:Content ID="_contentHeader" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
                <asp:Label ID="_lblHeaderTitle" runat="server"></asp:Label>
            </div>
        </div>
        <asp:Panel ID="_pnlTaskHeaderEdit" CssClass="panel" runat="server">

            <div class="panel-body">
                <IP:Site ID="_site" runat="server" ValidationGroup="TaskHeader" />
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-4">
                        <div class="form-group">
                            <span class="required">*</span><asp:Label ID="_lblDateSelection" runat="server" Text="<%$IPResources:Global,Date Selection %>"></asp:Label>
                            <IP:JQDatePicker ID="_dtStartEnd" runat="server" FromLabel="" ShowFromDate="true" ShowLabelsOnSameLine="true" />
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-4">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 col-sm-6">
                                    <asp:Label ID="_lblIncidentEventNumber" runat="server" Text="<%$IPResources:Global,Source System%>"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="_ddlSourceSystem" runat="server" Width="75%">
                                        <asp:ListItem Text=""></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-xs-12 col-sm-6">
                                    <asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Reference%>"></asp:Label>#<br />
                                    <asp:TextBox ID="_txtIncidentEventNumber" CssClass="form-control" Width="75%" runat="server"
                                        Text=""></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-4">
                        <div class="form-group">
                            <asp:Label ID="_lblHighLevelSecurity" runat="server" Text="<%$IPResources:Global,High Level Security %>"></asp:Label><br />
                            <asp:CheckBox ID="_cbHighLevelSecurity" Text="<%$IPResources:Global,Restricted Access %>"
                                ForeColor="red" Font-Bold="false" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-md-12 col-lg-6">
                        <span class="required">*</span><asp:Label ID="_lblIncidentTitle"
                            runat="server" Text="<%$IPResources:Global,Title %>"></asp:Label>
                        <asp:RequiredFieldValidator ControlToValidate="_txtTitle" runat="server" ID="RequiredFieldValidator1"
                            EnableClientScript="true" ErrorMessage="<%$IPResources:Global,Title is required%>" Text="" Display="Dynamic" CssClass="labelerror"
                            ValidationGroup="TaskHeader"></asp:RequiredFieldValidator><br />
                        <asp:TextBox ID="_txtTitle" runat="server" autofocus="true" CssClass="textExpand" TextMode="MultiLine" Rows="3" Wrap="true" placeholder="<%$IPResources:Global,Title %>"
                            Width="98%" MaxLength="200" onkeydown="maxTextboxLength(this,200,true);" onblur="maxTextboxLength(this,200,true);" onchange="maxTextboxLength(this,200,true);" onpaste="maxTextboxLength(this,200,true);"></asp:TextBox>
                        <asp:Label runat="server" ID="_lblTranslatedTitle" Width="98%"></asp:Label>
                    </div>

                    <div class="col-xs-12 col-md-12 col-lg-6">
                        <asp:Label ID="_lblDescription" runat="server" Text="<%$IPResources:Global,Description %>"></asp:Label><br />
                        <asp:TextBox ID="_txtDescription" runat="server" autofocus="true" CssClass="textExpand" TextMode="MultiLine" Rows="3" Wrap="true" placeholder="<%$IPResources:Global,Description %>"
                            Width="98%" MaxLength="3500" onkeydown="maxTextboxLength(this,3500,true);" onblur="maxTextboxLength(this,3500,true);" onchange="maxTextboxLength(this,3500,true);" onpaste="maxTextboxLength(this,3500,true);"></asp:TextBox>
                        <asp:Label runat="server" ID="_lblTranslatedDescription" Width="98%"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-md-12 col-lg-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <span class="required">*</span><asp:Label ID="Label4" runat="server" Text="<%$IPResources:Global,Types %>"></asp:Label>
                            </div>
                            <div class="panel-body" style="height: 200px; overflow-y: scroll">
                                <IP:ExtendedCheckBoxList ID="_cblIncidentType" CssClass="checkBoxList" RepeatLayout="table"
                                    runat="server" ShowToolTip="true" RepeatDirection="Vertical" Width="100%" AllowCheckAll="false"
                                    RepeatColumns="3" AllTextValue="-1" AllTextLabel="<%$IPResources:Global,All %>"
                                    GroupingText="" LabelFontBold="true" LabelText="">
                                    <validation runat="server" enabled="true" display="Dynamic" errormessage="<%$IPResources:Global,Type is required%>"
                                        text=" " validationgroup="TaskHeader" class="labelerror">                        
                    </validation>
                                </IP:ExtendedCheckBoxList>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-md-12 col-lg-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <span class="required">*</span><asp:Label ID="_lblActivity" runat="server" Text="<%$IPResources:Global,Activity %>"></asp:Label>
                                <asp:RequiredFieldValidator ControlToValidate="_rblActivity" runat="server" ID="_rfvActivity"
                                    EnableClientScript="true" ErrorMessage="<%$IPResources:Global,Activity is a required field %>"
                                    Text="*" Display="Dynamic" ValidationGroup="TaskHeader" class="labelerror"></asp:RequiredFieldValidator>
                            </div>
                            <div class="panel-body" style="height: 200px; overflow-y: scroll">
                                <asp:RadioButtonList ID="_rblActivity" RepeatLayout="Table" RepeatColumns="3" runat="server"
                                    RepeatDirection="Vertical" Width="100%">
                                </asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-md-12 col-lg-6">
                        <asp:Panel ID="_pnlTaskTypeManager" runat="server" CssClass="panel panel-default">
                            <div class="panel-heading">
                                <asp:Label ID="Label12" runat="server" Text="<%$IPResources:Global,Task Type Manager%>"></asp:Label>
                            </div>
                            <div class="panel-body" style="height: 100px">
                                <asp:DataList ID="_dlTaskTypeManager" runat="server" Width="90%" RepeatColumns="3"
                                    RepeatDirection="horizontal" RepeatLayout="table">
                                    <ItemTemplate>
                                        &bull;<asp:Label ID="_lblTypeManager" runat="server" Text='<%#DirectCast(Container.DataItem, DictionaryEntry).Value %>'
                                            SkinID="None"></asp:Label>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="col-xs-12 col-md-12 col-lg-6">
                        <asp:Panel ID="_pnlBusinessManager" runat="server" CssClass="panel panel-default">
                            <div class="panel-heading">
                                <asp:Label ID="_lblBusinessManager" runat="server" Text="<%$IPResources:Global,Business Manager%>"></asp:Label>
                            </div>
                            <div class="panel-body" style="height: 100px">
                                <asp:DataList ID="_dlBusinessManager" runat="server" Width="90%" RepeatColumns="3"
                                    RepeatDirection="horizontal" RepeatLayout="table">
                                    <ItemTemplate>
                                        &bull;<asp:Label ID="_lblBusinessManager" runat="server" Text='<%#DirectCast(Container.DataItem, DictionaryEntry).Value  %>'
                                            SkinID="None"></asp:Label>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-3 col-lg-3">
                        <asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Created By%>"></asp:Label>:
                    <br class="hidden-xs" />
                        <asp:Label ID="_lblCreatedBy" runat="server" Font-Bold="false" EnableTheming="false" Text="<%$IPResources:Global,None%>"></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-3 col-lg-3">
                        <asp:Label ID="Label3" runat="server" Text="<%$IPResources:Global,Creation Date%>"></asp:Label>:
                    <br class="hidden-xs" />
                        <asp:Label ID="_lblCreationDate" runat="server" Font-Bold="false" EnableTheming="false" Text="<%$IPResources:Global,None%>"></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-3 col-lg-3">
                        <asp:Label ID="Label5" runat="server" Text="<%$IPResources:Global,Last Updated By%>"></asp:Label>:
                   <br class="hidden-xs" />
                        <asp:Label ID="_lblLastUpdatedBy" runat="server" Font-Bold="false" EnableTheming="false" Text="<%$IPResources:Global,None%>"></asp:Label>
                    </div>
                    <div class="col-xs-12 col-sm-3 col-lg-3">
                        <asp:Label ID="Label7" runat="server" Text="<%$IPResources:Global,Last Update Date%>"></asp:Label>:
                    <br class="hidden-xs" />
                        <asp:Label ID="_lblLastUpdateDate" runat="server" Font-Bold="false" EnableTheming="false" Text="<%$IPResources:Global,None%>"></asp:Label>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <IP:TaskHeader ID="_taskHeader" runat="server" />
        <div class="panel">
            <div class="panel-footer">
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <IP:ModalIframe ID="_Attachments" runat="server" Width="1200px" DisplayModalButtonText="<%$IPResources:Global,Attachments/Links%>"
                                Url="Popups/Attachments.aspx" Height="600" BannerText="<%$IPResources:Global,Attachments%>"
                                ReloadPageOnClose="true" GlyphiconValue="glyphicon glyphicon-paperclip" />
                        </div>
                    </div>


                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <asp:LinkButton ID="_lnkMyTasks" runat="server" SkinID="Button" PostBackUrl="~/MyTaskHeaders.aspx">
                                <span class="glyphicon glyphicon-list-alt"></span>&nbsp;<asp:Label ID="Label11" runat="server" Text="<%$IPResources:Global,Show My Task Headers%>"></asp:Label>
                            </asp:LinkButton>
                        </div>
                    </div>

                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <asp:LinkButton ID="_lnkSaveButton" runat="server" SkinID="ButtonPrimary" OnClientClick="return CheckForm('TaskHeader');"
                                CausesValidation="true" ValidationGroup="TaskHeader">
                                <span class="glyphicon  glyphicon-floppy-save"></span>&nbsp;<asp:Label ID="_lblSaveButton" runat="server" Text="<%$IPResources:Global,Save Task Header%>"></asp:Label>
                            </asp:LinkButton>
                        </div>
                    </div>

                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <asp:LinkButton ID="_lnkDeleteHeader" runat="server" SkinID="Button"
                                Visible="false">
                                <asp:PlaceHolder runat="server">
                                    <span class="glyphicon glyphicon-trash"></span>&nbsp;<asp:Label ID="_lblDeleteHeader" runat="server" Text="<%$IPResources:Global,Delete Header%>"></asp:Label>
                                </asp:PlaceHolder>
                            </asp:LinkButton>

                        </div>
                    </div>

                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <asp:LinkButton ID="_lnkTaskItems" runat="server" SkinID="Button"
                                Visible="false">
                                <asp:PlaceHolder runat="server">
                                <span class="glyphicon glyphicon-plus"></span>&nbsp;<asp:Label ID="Label6" runat="server" Text="<%$IPResources:Global,Add Task Items%>"></asp:Label>
                           </asp:PlaceHolder>  </asp:LinkButton>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <IP:ModalIframe ID="_ImportTasks" runat="server" Width="90%" DisplayModalButtonText="<%$IPResources:Global,Import Tasks%>"
                                Url="Popups/TaskImport.aspx" Height="500" BannerText="<%$IPResources:Global,Tasks Import%>"
                                ReloadPageOnClose="true" GlyphiconValue="glyphicon glyphicon-cloud-upload" />
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <asp:LinkButton ID="_lnkEditTasks" runat="server" SkinID="Button"
                                Visible="false"><asp:PlaceHolder runat="server">
                                <span class="glyphicon glyphicon-pencil"></span>&nbsp;<asp:Label ID="Label9" runat="server" Text="<%$IPResources:Global,Edit Multiple Tasks%>"></asp:Label>
                          </asp:PlaceHolder>   </asp:LinkButton>
                        </div>
                    </div>

                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <asp:LinkButton ID="_btnReturnToSourceSystem" runat="server" SkinID="Button"
                                Visible="false"><asp:PlaceHolder runat="server">
                                <span class="glyphicon glyphicon-new-window"></span>&nbsp;<asp:Label ID="_lblReturnToSourceSystem" runat="server"></asp:Label>
                            
                                </asp:PlaceHolder> </asp:LinkButton>
                            <IP:ModalIframe ID="_btnAssignReplication" runat="server" Width="80%" DisplayModalButtonText="<%$IPResources:Global,Assign Replication%>"
                                Url="Popups/ReplicationAssignment.aspx" Height="500" BannerText="<%$IPResources:Global,Assign/View Replication%>"
                                ReloadPageOnClose="true" GlyphiconValue="glyphicon glyphicon-duplicate" />
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <asp:Button ID="_btnNewTaskHeader" SkinID="ButtonBlack" runat="server" Text="<%$IPResources:Global,Add New Task Header %>"
            CssClass="ButtonBlack" CausesValidation="true" ValidationGroup="TaskHeader"
            Visible="false" />
        <IP:MessageBox ID="_cbeDelete" runat="server" ButtonType="YesNo" AllowPostback="true"
            Title="<%$IPResources:Global,Confirm Delete Header%>" Message="<%$IPResources:Global,WarningDeleteHeaderConfirmation%>" />
        <IP:MessageBox ID="_confirmationDialogReplicationTasks" runat="server" ButtonType="YesNo"
            AllowPostback="false" Message="<%$IPResources:Global,[MTT Template] Warning%>"
            OKScript="return true" Title="<%$IPResources:Global,Task Replication%>" />
        <IP:MessageBox ID="_confirmationDialogTemplateTasks" runat="server" ButtonType="YesNo"
            AllowPostback="false" Message="<%$IPResources:Global,[Outage Template] Warning%>"
            OKScript="return true" Title="<%$IPResources:Global,Outage Template Tasks%>" />

        <div class="hidden-xs">
            <iframe id="_ifrTaskItems" runat="server" frameborder="0" style="display: none; width: 100%"></iframe>
            <div id="preload-img">
                <h1>
                    <asp:Label runat="server" Text="<%$IPResources:Global,Loading%>"></asp:Label>...
                </h1>
            </div>
        </div>

    </div>
</asp:Content>
