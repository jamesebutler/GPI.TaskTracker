<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerPopup.master" AutoEventWireup="false"
    CodeFile="CopyTask.aspx.vb" Inherits="Popups_CopyTask" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerPopup.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <asp:UpdatePanel ID="_udpCopyTask" runat="server" RenderMode="block" UpdateMode="Always">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-5">
                    <asp:Panel ID="_pnlCopyTaskAvailableUsers" runat="server" ScrollBars="Auto" CssClass="panel panel-default" Height="300px">
                        <div class="panel-heading">
                            <span class="required">*</span><asp:Label ID="Label4" runat="server" Text="<%$IPResources:Global, Available Users %>"></asp:Label>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <IP:EmployeeList ID="_ResponsiblePerson" runat="server" Employeelabel="<%$IPResources:Global, Responsible Person%>"
                                            EnableViewState="true" UseOnDemandListPopulation="false" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <asp:Label runat="server" Text="<%$IPResources:Global,Due Date%>"></asp:Label><br />
                                        <IP:JQDatePicker ID="_EstimatedDueDate" runat="server" StartDate='' ShowFromDate="false"
                                            FromLabel="<%$IPResources:Global,Due Date%>" AllowDateCritical="true" EnableValidation="false">
                                        </IP:JQDatePicker>
                                    </div>
                                </div>
                            </div>
                        </div>



                    </asp:Panel>
                </div>

                <div class="col-md-2">
                    <div>
                        <div class="form-group">
                            <span class="hidden-xs hidden-sm">
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                            </span>
                            <asp:LinkButton ID="_btnAddUser" runat="server" SkinID="Button" OnClientClick="$('input[type=button]').hide();" UseSubmitBehavior="false">
                                <span class="glyphicon glyphicon-user"></span>&nbsp;<asp:Label ID="Label11" runat="server" Text="<%$IPResources:Global,Add Responsible Person%>"></asp:Label>
                            </asp:LinkButton>
                            <br />
                            <br />
                            <asp:LinkButton ID="_btnCreateTasks" runat="server" SkinID="Button" OnClientClick="$('input[type=button]').hide();" UseSubmitBehavior="false">
                                <span class="glyphicon glyphicon-plus"></span>&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Create New Task%>"></asp:Label>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" CssClass="panel panel-default" Height="300px">
                        <div class="panel-heading">
                            <span class="required">*</span><asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,  Selected User %>"></asp:Label>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <asp:GridView ID="_grvResponsiblePerson" runat="server" Width="100%" CaptionAlign="Bottom" AutoGenerateColumns="false" AutoGenerateDeleteButton="false">
                                            <Columns>
                                                <asp:BoundField HeaderText="Responsible" DataField="ResponsibleUser" />
                                                <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMMM yyyy}" />
                                                <asp:BoundField HeaderText="Critical" DataField="DateIsCritical" />
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ForeColor="black" runat="server" ToolTip='<%$IPResources:Global,Delete%>' ID="_btnDeleteResponsible" CommandArgument='<%#Eval("ResponsibleUsername") %>' OnClick="DeleteResponsibleUser"><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <center><h1><asp:Label ID="_lblCopyStatys" runat="server" ></asp:Label></h1></center>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
