<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SubTasks.aspx.vb" Inherits="PopupsSubTasks" %>
<%@ Register Src="~/User Controls/AdvancedEmployeeListDropdown.ascx" TagName="EmployeeList" TagPrefix="IP" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Untitled Page</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
</head>
<body style="margin-left: 0; margin-top: 0">
    <form id="form1" runat="server">
         <asp:ScriptManager ID="_scriptManager" AsyncPostBackTimeout="10000"
            EnablePageMethods="false" LoadScriptsBeforeUI="true" ScriptMode="release" runat="server"
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
        <div class="container-fluid">
            <asp:UpdatePanel ID="_udpSubTask" runat="server" UpdateMode="always">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="Label4" Text="*" CssClass="labelerror" runat="server"></asp:Label>&nbsp;<asp:Label ID="_lblTitle" runat="server" Text="<%$IPResources:Global,Title%>"></asp:Label><br />
                                <asp:TextBox ID="_txtTitle" runat="server" autofocus="true" CssClass="textExpand" TextMode="MultiLine" Rows="2" Wrap="true" placeholder="<%$IPResources:Global,Title %>"
                                    Width="90%" MaxLength="80" onkeydown="maxTextboxLength(this,200,true);" onblur="maxTextboxLength(this,200,true);" onchange="maxTextboxLength(this,200,true);" onpaste="maxTextboxLength(this,200,true);"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="_rfvTitle" runat="server" ControlToValidate="_txtTitle"
                                    ErrorMessage="<%$IPResources:Global,Title is required%>" ValidationGroup="ValidateSubTask" Display="Dynamic"
                                    Text="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Label ID="_lblDescription" runat="server" Text="<%$IPResources:Global,Description%>"></asp:Label><br />
                                <asp:TextBox ID="_txtDescription" runat="server" autofocus="true" CssClass="textExpand" TextMode="MultiLine" Rows="2" Wrap="true" placeholder="<%$IPResources:Global,Description %>"
                                    Width="90%" MaxLength="3500" onkeydown="maxTextboxLength(this,3500,true);" onblur="maxTextboxLength(this,3500,true);" onchange="maxTextboxLength(this,3500,true);" onpaste="maxTextboxLength(this,Description,true);"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <IP:EmployeeList ID="_employeeList" Employeelabel="<%$IPResources:Global,Responsible%>" runat="server" EnableValidation="true">
                                    <Validation ValidationGroup="ValidateSubTask" ValidationErrorMessage="<%$IPResources:Global,Responsible Person is required%>"
                                        ValidationErrorText="*" ValidateEmptyText="true" />
                                </IP:EmployeeList>
                            </div>
                        </div>
                        <div class="col-md-6 col-md-offset-2">
                            <div class="form-group">
                                <asp:Label ID="Label2" Text="*" CssClass="labelerror" runat="server"></asp:Label>&nbsp;<asp:Label ID="_lblDaysAfterComplete"
                                    runat="server" Text="<%$IPResources:Global,Days After Complete%>"></asp:Label><br />
                                <asp:TextBox ID="_txtDaysAfterComplete" Width="50px" runat="server" ValidationGroup="ValidateSubTask"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server"
                                    TargetControlID="_txtDaysAfterComplete" FilterType="Numbers" />
                                <asp:RequiredFieldValidator ID="_rfvDaysAfterComplete" runat="server" ControlToValidate="_txtDaysAfterComplete"
                                    ErrorMessage="<%$IPResources:Global,Days after complete is required%>" ValidationGroup="ValidateSubTask"
                                    Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>

                    <div class="panel-footer">
                        <div class="row">
                            <div class="col-md-4 col-md-offset-4">
                                <div class="form-group">
                                    <asp:LinkButton ID="_btnAddSubTask" runat="server" SkinID="ButtonPrimary" CausesValidation="true" OnClientClick="return CheckForm('ValidateSubTask');" ValidationGroup="ValidateSubTask">
                                        <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-floppy-save"></span>&nbsp;<asp:Label ID="Label11" runat="server" Text="<%$IPResources:Global,Save Subsequent Task%>"></asp:Label></asp:PlaceHolder>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-body">
                        <asp:GridView ID="_gvSubTask" runat="server" Width="100%" AutoGenerateColumns="false"
                            AutoGenerateEditButton="false" AutoGenerateDeleteButton="false" DataKeyNames="TaskItemSeqId">
                            <Columns>
                                <asp:BoundField HeaderText="<%$IPResources:Global,Title%>" DataField="Title" ItemStyle-VerticalAlign="Top"
                                    ItemStyle-Wrap="true" ItemStyle-Width="20%" />
                                <asp:BoundField HeaderText="<%$IPResources:Global,Description%>" DataField="Description" ItemStyle-VerticalAlign="Top"
                                    ItemStyle-Wrap="true" ItemStyle-Width="40%" />
                                <asp:TemplateField HeaderText="<%$IPResources:Global,Responsible%>" ItemStyle-VerticalAlign="Top" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="_lblResponsible" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="<%$IPResources:Global,Days After%>" DataField="DaysAfter" ItemStyle-VerticalAlign="Top" ItemStyle-Width="5%"/>
                                <asp:TemplateField ItemStyle-VerticalAlign="Top" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <div class="form-group">
                                            <asp:LinkButton ID="_btnEdit" runat="server" SkinID="Button" CommandArgument='<%# DirectCast(Container.DataItem, SubTaskItem)("TaskItemSeqId")%>' Visible="true" OnClick="EditSubTask">
                                                <asp:PlaceHolder runat="server">
                                                    <span class="glyphicon glyphicon-pencil"></span>&nbsp;<asp:Label ID="_lblEditSubTask" runat="server" Text="<%$IPResources:Global,Edit%>"></asp:Label>
                                                </asp:PlaceHolder>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-VerticalAlign="Top" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <div class="form-group">
                                            <asp:LinkButton ID="_btnDelete" runat="server" SkinID="Button" CommandArgument='<%# DirectCast(Container.DataItem, SubTaskItem)("TaskItemSeqId")%>' Visible="true">
                                                <asp:PlaceHolder runat="server">
                                                    <span class="glyphicon glyphicon-trash"></span>&nbsp;<asp:Label ID="_lblDeleteHeader" runat="server" Text="<%$IPResources:Global,Delete%>"></asp:Label>
                                                </asp:PlaceHolder>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
