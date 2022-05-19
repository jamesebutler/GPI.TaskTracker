<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false"
    CodeFile="MaintenanceEmployee.aspx.vb" Inherits="MaintenanceMain" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<%@ Register Namespace="RealWorld.Grids" TagPrefix="rwg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <%-- <style type="text/css">
        a.toolTip span {
            display: none;
        }

        a.toolTip:hover span {
            /*the span will display just on :hover state*/
            display: block;
            position: absolute;
            top: 2em;
            left: 2em;
            width: 25em;
            font-size: large;
            border: 1px solid #000;
            background-color: LightBlue;
            color: #fff;
            text-align: center;
        }
    </style>--%>

   

    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
                <asp:Label ID="_lblHeaderTitle" runat="server"></asp:Label>
            </div>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-offset-3 col-xs-12 col-md-2 text-right">
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Height="24px" Text="<%$IPResources:Global,Employee Maintenance for%>"
                        Width="200px"></asp:Label>:
                </div>
                <div class="col-xs-12 col-md-3">
                    <asp:RequiredFieldValidator ID="_rfvFacility" runat="server" ControlToValidate="_ddlsitelist"
                        ValidationGroup="DataMaintEmp" ErrorMessage="<%$IPResources:Global,Facility is a required field%>"
                        Display="Dynamic" Text="*" InitialValue="0"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="_ddlsitelist" runat="server" AutoPostBack="false" Height="24px"
                        Width="192px">
                    </asp:DropDownList>
                </div>
            </div>
            <br />
            <br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="Label3" runat="server" Text="<%$IPResources:Global,Add Employee%>"></asp:Label>
                        - <small>  <asp:Label Text="<%$IPResources:Global,Enter all Required fields%>" runat="server" EnableTheming="false"></asp:Label></small>
                        </div>
                </div>
               
             <div class="panel-body" >
                     <div class="row">
     <asp:UpdatePanel ID="UpdatePanelEmailLookup" runat="server"  >
        <Triggers>
            <asp:AsyncPostBackTrigger controlid="ButtonSearch"  EventName="Click" />
        </Triggers>
     <ContentTemplate>          
                        <asp:Label ID="LabelEmailLookup" runat="server" Text="Enter Email"></asp:Label>
                         <asp:TextBox ID="TextBoxEmailLookup"  Width="250" runat="server"></asp:TextBox>
                        <asp:Button ID="ButtonSearch" OnClick="ButtonSearch_Click" runat="server" SkinID="Button" width="150" Text="Search" />
  
</ContentTemplate>
</asp:UpdatePanel>
                                 </div>

                </div>
                
                
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-6 col-md-3">
                            <span class="required">*</span>&nbsp;<asp:Label runat="server" Text="<%$IPResources:Global,NetWorkID%>"></asp:Label><br />
                            <asp:TextBox ID="_networkid" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="_rfvNetworkID" runat="server" ControlToValidate="_networkid"
                                ValidationGroup="DataMaintEmp" ErrorMessage="<%$IPResources:Global,Network ID is a required field%>"
                                Text="*" Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-6 col-md-3">
                            <span class="required">*</span>&nbsp;<asp:Label runat="server" Text="<%$IPResources:Global,LastName%>"></asp:Label><br />
                            <asp:TextBox ID="_lastname" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="_rfvLastName" runat="server" ControlToValidate="_lastname"
                                ValidationGroup="DataMaintEmp" ErrorMessage="LastName is a required field" Text="*"
                                Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-6 col-md-3">
                            <span class="required">*</span>&nbsp;<asp:Label runat="server" Text="<%$IPResources:Global,FirstName%>"></asp:Label><br />
                            <asp:TextBox ID="_firstname" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="_rfvFirstName" runat="server" ControlToValidate="_firstname"
                                ValidationGroup="DataMaintEmp" ErrorMessage="<%$IPResources:Global,FirstName is a required field%>"
                                Text="*" Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-6 col-md-3">
                            <asp:Label runat="server" Text="<%$IPResources:Global,MidInit%>"></asp:Label><br />
                            <asp:TextBox ID="_midinit" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-6 col-md-3">
                            <span class="required">*</span>&nbsp;<asp:Label runat="server" Text="<%$IPResources:Global,Email Address%>"></asp:Label><br />
                            <asp:TextBox ID="_email" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="_rfvEmail" runat="server" ControlToValidate="_email"
                                ValidationGroup="DataMaintEmp" ErrorMessage="<%$IPResources:Global,Email Address is a required field%>"
                                Text="*" Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-6 col-md-3">
                            <asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Phone%>"></asp:Label>
                            #<br />
                            <asp:TextBox ID="_phone" runat="server"></asp:TextBox>

                        </div>
                        <div class="col-sm-6 col-md-3">
                            <span class="required">*</span>&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Domain%>"></asp:Label><br />
                            <asp:DropDownList ID="_ddldomain" runat="server"  SkinID="nolivesearch">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="_rfvDomain" runat="server" ControlToValidate="_ddldomain"
                                ValidationGroup="DataMaintEmp" ErrorMessage="<%$IPResources:Global,Domain is a required field%>"
                                Display="Dynamic" Text="*" InitialValue=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-6 col-md-3">
                            <span class="required">*</span>&nbsp;<asp:Label ID="Label4" runat="server" Text="<%$IPResources:Global,Default Lang%>"></asp:Label><br />
                            <asp:DropDownList ID="_ddldefaultlang" runat="server"  SkinID="nolivesearch">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="_rfvDefaultLang" runat="server" ControlToValidate="_ddldefaultlang"
                                ValidationGroup="DataMaintEmp" ErrorMessage="<%$IPResources:Global,Default Language is a required field%>"
                                Text="*" Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-md-4 col-md-offset-4">
                            <div class="form-group">
                                <asp:LinkButton ID="_btnSubmit" runat="server" SkinID="Button" ValidationGroup="DataMaintEmp" OnClientClick="return CheckForm('DataMaintEmp')">
                                    <span class="glyphicon glyphicon-user"></span>&nbsp;<asp:Label ID="Label11" runat="server" Text="<%$IPResources:Global,Add Employee%>"></asp:Label>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <br />
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="Label6" runat="server" Text="<%$IPResources:Global,Transfer Employee%>"></asp:Label>
                        - 
                        <small>
                            <asp:Label Text="<%$IPResources:Global,Employee Transfer Instructions%>" runat="server" EnableTheming="false"></asp:Label></small>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="col-md-3">
                        <div class="form-group">
                            <IP:EmployeeList ID="_ResponsiblePerson" usermode="UsersOnly" Employeelabel="<%$IPResources:Global,Transfer Employee%>"
                                runat="server" EnableValidation="true" DisplayClearLink="false">
                                <validation validationgroup="validationTransferUser" validationerrormessage="<%$IPResources:Global,Responsible Person is required%>"
                                    validationerrortext="" validateemptytext="true" />
                            </IP:EmployeeList>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-md-4 col-md-offset-4">
                            <div class="form-group">
                                <asp:LinkButton ID="_btnTransfer" runat="server" SkinID="Button" ValidationGroup="validationTransferUser" OnClientClick="return CheckForm('validationTransferUser')">
                                    <span class="glyphicon glyphicon-transfer"></span>&nbsp;<asp:Label ID="Label7" runat="server" Text="<%$IPResources:Global,Transfer Employee%>" OnClientClick="return CheckForm('DataMaintEmp')"></asp:Label>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="_lblEmpMaintenance" runat="server" Text="<%$IPResources:Global,Employee Maintenance%>"></asp:Label>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-md-4 col-md-offset-4">
                            <div class="form-group">
                                <asp:LinkButton ID="_btnUpdateEmp" runat="server" SkinID="Button" Enabled="true" ClientIDMode="Static">
                                    <span class="glyphicon  glyphicon-floppy-save"></span>&nbsp;<asp:Label ID="Label9" runat="server" Text="<%$IPResources:Global,Update Employee%>" OnClientClick="return CheckForm('DataMaintEmp')"></asp:Label>
                                </asp:LinkButton>

                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <asp:CheckBox ID="_cbShowHideInactiveUsers" ClientIDMode="Static" Enabled="true" AutoPostBack="true" runat="server" Text="<%$IPResources:Global,Show Inactive Users%>" Checked="false" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <section>
                        <RWG:BulkEditGridView ShowHeader="true" ID="_gvEmployeeList" runat="server" AutoGenerateColumns="False"
                            EnableInsert="False" DataKeyNames="Networkid" SaveButtonID="" CellPadding="1"
                            EnableViewState="true" Visible="false" HorizontalAlign="Center" CssClass="Border"
                            BorderStyle="Double" BorderWidth="1" Width="100%" AllowPaging="false" PageSize="10">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$IPResources:Global,NetworkID*%>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="_tbNetWorkID" runat="server" ReadOnly="true" Text="<%#DirectCast(Container.DataItem, Employee).NetworkID%>"
                                             OnTextChanged="_gvEmployeeList.HandleRowChanged"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global,Last Name*%>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="_tbLastName" runat="server" Text="<%#DirectCast(Container.DataItem, Employee).LastName%>"
                                           OnTextChanged="_gvEmployeeList.HandleRowChanged"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global,First Name*%>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="_tbFirstName" runat="server" Text="<%#DirectCast(Container.DataItem, Employee).FirstName%>"
                                            OnTextChanged="_gvEmployeeList.HandleRowChanged"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global,MidInit%>" ItemStyle-HorizontalAlign="center">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="_tbmidinit" runat="server" Text="<%#DirectCast(Container.DataItem, Employee).MiddleInit%>"
                                          OnTextChanged="_gvEmployeeList.HandleRowChanged"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global,Email%>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="_tbEmail" runat="server" Text="<%#DirectCast(Container.DataItem, Employee).Email%>"
                                            />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global,Phone%>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="_tbPhone" runat="server" Text="<%#DirectCast(Container.DataItem, Employee).Phone%>"
                                             OnTextChanged="_gvEmployeeList.HandleRowChanged"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global,Domain%>">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="_ddldomain2" runat="server" Style="min-width: 150px" SkinID="nolivesearch">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global,Default Language%>">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="_ddldefaultlang2" runat="server" Width="90%" Style="min-width: 180px" SkinID="nolivesearch">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global,Inactive?%>" ItemStyle-HorizontalAlign="center">
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="_cbInactiveFlag" runat="server" DataTextField="Inactive?"></asp:CheckBox>
                                        <asp:HiddenField ID="_rowChanged" runat="server" OnValueChanged="_gvEmployeeList.HandleRowChanged" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </RWG:BulkEditGridView>
                    </section>
                </div>

            </div>
        </div>
    </div>


    <div style="text-align: center">
        <asp:Label ID="_lblEmployeeStatus" BackColor="Red" Width="100%" Font-Bold="True"
            Font-Size="Large" Height="40px" runat="server">
        </asp:Label><br />

        &nbsp;
    </div>
    <RWG:BulkEditGridView ShowHeader="true" ID="_gvMyProfile" runat="server" AutoGenerateColumns="False"
        EnableInsert="False" DataKeyNames="NetworkId" SaveButtonID="" CellPadding="1"
        EnableViewState="true" Visible="false" HorizontalAlign="Center" CssClass="Border"
        BorderStyle="Double" BorderWidth="1" Width="90%" AllowPaging="false" PageSize="10">
        <Columns>
            <asp:TemplateField HeaderText="<%$IPResources:Global,NetworkID*%>">
                <ItemTemplate>
                    <asp:TextBox ID="_tbNetWorkID" runat="server" Text="<%#DirectCast(Container.DataItem, Employee).NetworkId %>"
                        Width="90%" Style="min-width: 90px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$IPResources:Global,Last Name*%>">
                <ItemTemplate>
                    <asp:TextBox ID="_tbLastName" runat="server" Text="<%#DirectCast(Container.DataItem, Employee).LastName%>"
                        Width="90%" Style="min-width: 90px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$IPResources:Global,First Name*%>">
                <ItemTemplate>
                    <asp:TextBox ID="_tbFirstName" runat="server" Text="<%#DirectCast(Container.DataItem, Employee).FirstName%>"
                        Width="90%" Style="min-width: 90px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$IPResources:Global,MidInit%>" ItemStyle-HorizontalAlign="center">
                <EditItemTemplate>
                    <asp:TextBox ID="_tbmidinit" runat="server" Text="<%#DirectCast(Container.DataItem, Employee).MiddleInit%>"
                        Width="25px"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$IPResources:Global,Email%>">
                <ItemTemplate>
                    <asp:TextBox ID="_tbEmail" runat="server" Text="<%#DirectCast(Container.DataItem, Employee).Email%>"
                        Width="200px" Style="min-width: 180px" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$IPResources:Global,Phone%>">
                <ItemTemplate>
                    <asp:TextBox ID="_tbPhone" runat="server" Text="<%#DirectCast(Container.DataItem, Employee).Phone%>"
                        Width="100px" Style="min-width: 100px"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$IPResources:Global,Domain%>">
                <EditItemTemplate>
                    <asp:TextBox ID="_txtDomain" ReadOnly="true" runat="server" Text='<%#DirectCast(Container.DataItem, Employee).Domain %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$IPResources:Global,Default Language%>">
                <EditItemTemplate>
                    <asp:DropDownList ID="_ddldefaultlang2" runat="server" Width="90%" Style="min-width: 180px" SkinID="nolivesearch">
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </RWG:BulkEditGridView>

 
</asp:Content>
