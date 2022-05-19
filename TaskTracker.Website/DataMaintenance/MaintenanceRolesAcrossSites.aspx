<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false"
    CodeFile="MaintenanceRolesAcrossSites.aspx.vb" Inherits="MaintenanceRolebySite" Title="Untitled Page" %>

<%@ Import Namespace="System.Globalization" %>
<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<%@ Register Namespace="RealWorld.Grids" TagPrefix="rwg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <asp:UpdatePanel ID="_udpRoleList" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="Label4" runat="server" Text="<%$IPResources:Global,Role Maintenance%>"
                            Font-Size="Medium"></asp:Label>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12 col-md-4">
                            <asp:Label ID="Label3" runat="server" Text="<%$IPResources:Global,Role Maintenance for%>"
                                Font-Size="Medium"></asp:Label><br />
                            <asp:RequiredFieldValidator ID="_rfvRoleList" runat="server" ControlToValidate="_ddlrolelist"
                                ValidationGroup="DataMaintEmp" ErrorMessage="<%$IPResources:Global,Role is a required field%>"
                                Display="Dynamic" Text="*" InitialValue="0"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="_ddlrolelist" runat="server" AutoPostBack="false" Style="min-width: 192px;">
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-12 col-md-4">
                            <asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Business%>"
                                Font-Size="Medium"></asp:Label><br />
                            <asp:RequiredFieldValidator ID="_rfvBusTypeList" runat="server" ControlToValidate="_ddlbustypelist"
                                ValidationGroup="DataMaintEmp" ErrorMessage="<%$IPResources:Global,Business Type is a required field%>"
                                Display="Dynamic" Text="*" InitialValue="0"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="_ddlbustypelist" runat="server" AutoPostBack="false" Style="min-width: 192px;">
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-12 col-md-4">
                            <asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Division%>"
                                Font-Size="Medium"></asp:Label><br />
                            <asp:RequiredFieldValidator ID="_rfvDivisionList" runat="server" ControlToValidate="_ddldivisionlist"
                                ValidationGroup="DataMaintEmp" ErrorMessage="<%$IPResources:Global,Division is a required field%>"
                                Display="Dynamic" Text="*" InitialValue="0"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="_ddldivisionlist" runat="server" AutoPostBack="false" Style="min-width: 192px;">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-xs-12 col-md-offset-4 col-md-4">
                            <asp:LinkButton ID="_btnSubmit" runat="server" SkinID="Button"
                                Visible="true" CausesValidation="false">
                                <asp:PlaceHolder runat="server">
                                    <span class="glyphicon glyphicon-search"></span>&nbsp;<asp:Label ID="Label8" runat="server" Text="<%$IPResources:Global,Display Role List%>"></asp:Label>
                                </asp:PlaceHolder>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <div class="panel panel-default" id="_pnlAddRoleAssignment"  runat="server">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="Label5" runat="server" Text="<%$IPResources:Global,Add Role Assignment%>"
                            Font-Size="Medium"></asp:Label>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-4 col-lg-2">
                            <span class="required">*</span>&nbsp;<asp:Label runat="server" Text="<%$IPResources:Global,Facility%>"></asp:Label><br />
                            <asp:DropDownList ID="_newddlFacility" runat="server" ValidationGroup="roleMaint">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 col-lg-3">
                            <IP:EmployeeList ID="_employeeList2" UserMode="UsersOnly" Employeelabel="<%$IPResources:Global,Name%>" AllowFacilityChange="True"
                                runat="server" EnableValidation="true">
                                <validation validationgroup="roleMaint" validationerrormessage="<%$IPResources:Global,Responsible Person is required%>"
                                    validationerrortext="" validateemptytext="true" />
                            </IP:EmployeeList>
                        </div>
                        <div class="col-md-4 col-lg-2">
                            <span class="required">*</span>&nbsp;<asp:Label ID="Localize2" runat="server" Text="<%$IPResources:Global,Business Unit%>"></asp:Label><br />
                            <asp:DropDownList ID="_newddlBusUnit" runat="server" ClientIDMode="Static">
                            </asp:DropDownList>
                            <ajaxToolkit:CascadingDropDown ID="_cddlBusinessUnit" runat="server" Category="BusinessUnit" BehaviorID="cddlBusinessUnit"
                                LoadingText="<%$IPResources:Global,Loading Business Units%>" 
                                ServiceMethod="GetBusinessUnitList" ServicePath="~/WebServices/SiteDropDownsWS.asmx"
                                TargetControlID="_newddlBusUnit" ContextKey="ALL" ParentControlID="_newddlFacility"
                                UseContextKey="true"></ajaxToolkit:CascadingDropDown>
                        </div>
                        <div class="col-md-4 col-lg-2">
                            <span class="required">*</span>&nbsp;<asp:Label ID="Localize3" runat="server" Text="<%$IPResources:Global,Area%>"></asp:Label><br />
                            <asp:DropDownList ID="_newddlArea" runat="server" ClientIDMode="Static">
                            </asp:DropDownList>
                            <ajaxToolkit:CascadingDropDown ID="_cddlArea" runat="server" Category="Area" LoadingText="<%$IPResources:Global,Loading Areas%>"
                                ServiceMethod="GetAreaList" ServicePath="~/WebServices/SiteDropDownsWS.asmx" BehaviorID="cddlArea"
                                TargetControlID="_newddlArea" ContextKey="ALL" ParentControlID="_newddlBusUnit"
                                UseContextKey="true"></ajaxToolkit:CascadingDropDown>
                        </div>
                        <div class="col-md-4 col-lg-2">
                            <span class="required">*</span>&nbsp;<asp:Label ID="Localize4" runat="server" Text="<%$IPResources:Global,Line%>"></asp:Label><br />
                            <asp:DropDownList ID="_newddlLine" runat="server" ClientIDMode="Static">
                            </asp:DropDownList>
                            <ajaxToolkit:CascadingDropDown ID="_cddlLine" runat="server" Category="Line" LoadingText="<%$IPResources:Global,Loading Line%>"
                               ServiceMethod="GetLineList" ServicePath="~/WebServices/SiteDropDownsWS.asmx" BehaviorID="cddlLine"
                                TargetControlID="_newddlLine" ContextKey="ALL" ParentControlID="_newddlArea"
                                UseContextKey="true"></ajaxToolkit:CascadingDropDown>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-md-4 col-md-offset-4">
                            <asp:LinkButton ID="_btnAddUserRole" runat="server" SkinID="Button"
                                Visible="true" ValidationGroup="roleMaint" CausesValidation="true" OnClientClick="return CheckForm('roleMaint');">
                                <asp:PlaceHolder runat="server">
                                    <span class="glyphicon glyphicon-check"></span>&nbsp;<asp:Label ID="Label6" runat="server" Text="<%$IPResources:Global,Add New Role Assignment%>"></asp:Label>
                                </asp:PlaceHolder>
                            </asp:LinkButton>
                        </div>
                    </div>

                </div>
            </div>

            <div class="panel panel-default" id="_pnlRoleAssignment" runat="server">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="Label9" runat="server" Text="<%$IPResources:Global,Update Role Assignment%>"
                            Font-Size="Medium"></asp:Label>
                    </div>
                </div>
                 <div class="panel-body">
                    <div class="col-xs-12 col-md-8 col-md-offset-2">
                        <RWG:BulkEditGridView ID="_gvRoleList" runat="server" AllowSorting="false" AutoGenerateDeleteButton="false"
                            AutoGenerateEditButton="false" AutoGenerateColumns="False" CellPadding="1"  HorizontalAlign="Center"
                            CssClass="tablesorter {sortlist: [[0,0]]}" DataKeyNames="Plantcode,AssigneeNetworkId,BusUnit,Area,Line">
                            <AlternatingRowStyle ForeColor="black" Font-Bold="false" />
                            <RowStyle Wrap="false" ForeColor="black" Font-Bold="false" />
                            <HeaderStyle Height="30px" ForeColor="white" Font-Underline="true" />
                            <Columns>
                                <%--                <asp:TemplateField  HeaderText="RoleID" Visible="False" > <itemtemplate>
                <asp:TextBox ID="_tbRoleID" runat="server" Visible="False" text="<%#DirectCast(Container.DataItem, RoleBySite).RoleId%>" width="100px" ></asp:TextBox>
                </itemtemplate>  </asp:TemplateField>--%>
                                <asp:TemplateField SortExpression="Facility" HeaderText="<%$IPResources:Global,Facility %>"
                                    ItemStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:Label ID="_lblFacility" runat="server" Text="<%#DirectCast(Container.DataItem, RoleBySite).SiteName%>"> </asp:Label>
                                        <%-- <asp:DropDownList ID="_ddlFacility" runat="server"> </asp:DropDownList>
                      <ajaxToolkit:CascadingDropDown ID="_cddlfacility2" runat="server" Category="Facility"
                        LoadingText="Loading Facilities..." PromptText="    " SelectedValue="<%#DirectCast(Container.DataItem, RoleBySite).PlantCode%>"
                        ServiceMethod="GetFacilityList" ServicePath="~/WebServices/SiteDropDownsWS.asmx"
                        TargetControlID="_ddlFacility" ContextKey="ALL"
                        UseContextKey="true">
                       </ajaxToolkit:CascadingDropDown>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$IPResources:Global, Responsible%>" SortExpression="ResponsibleName"
                                    HeaderStyle-Width="250px">
                                    <EditItemTemplate>
                                        <IP:EmployeeList ID="_ResponsiblePerson" runat="server" UserMode="UsersOnly" SelectedValue="<%#DirectCast(Container.DataItem, RoleBySite).AssigneeNetworkId%>"
                                            DisplayClearLink="false" AutoPostBack="false" Employeelabel="" EnableValidation="true">
                                            <validation validationgroup="RoleEntry" validationerrormessage="<%$IPResources:Global,Responsible Person is required%>"
                                                validationerrortext="" validateemptytext="true" />
                                        </IP:EmployeeList>
                                        <asp:Literal ID="_litResponsiblePerson" runat="server"></asp:Literal>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- <asp:TemplateField HeaderText="<%$IPResources:Global,Business Unit%>" SortExpression="Business Unit" ItemStyle-Width="15%">
                    <EditItemTemplate>
                       <asp:DropDownList ID="_ddlBusUnit" runat="server"></asp:DropDownList>
                       <ajaxToolkit:CascadingDropDown ID="_cddlBusinessUnit2" runat="server" Category="BusinessUnit"
                        LoadingText="<%$IPResources:Global,Loading Business Units%>" PromptText="    "  SelectedValue="<%#DirectCast(Container.DataItem, RoleBySite).BusUnit%>"
                        ServiceMethod="GetBusinessUnitList" ServicePath="~/WebServices/SiteDropDownsWS.asmx"
                        TargetControlID="_ddlBusUnit" ContextKey="ALL" ParentControlID="_ddlFacility"
                        UseContextKey="true">
                    </ajaxToolkit:CascadingDropDown>
                    </EditItemTemplate>
                 </asp:TemplateField>

                   
               
                <asp:TemplateField HeaderText="<%$IPResources:Global,Area%>" SortExpression="Area" ItemStyle-Width="15%">
                    <EditItemTemplate>
                       <asp:DropDownList ID="_ddlArea" runat="server"></asp:DropDownList>  
                       <ajaxToolkit:CascadingDropDown ID="_cddlarea2" runat="server" Category="Area" SelectedValue="<%#DirectCast(Container.DataItem, RoleBySite).Area%>"
                        LoadingText="Loading Areas..." PromptText="    "
                        ServiceMethod="GetAreaList" ServicePath="~/WebServices/SiteDropDownsWS.asmx"
                        TargetControlID="_ddlArea" ContextKey="ALL" ParentControlID="_ddlBusUnit"
                        UseContextKey="true">
                       </ajaxToolkit:CascadingDropDown>
                    </EditItemTemplate>
               </asp:TemplateField>
                  
               
                <asp:TemplateField HeaderText="<%$IPResources:Global,Line%>" SortExpression="Line" ItemStyle-Width="15%">
                    <EditItemTemplate>
                       <asp:DropDownList ID="_ddlLine" runat="server"></asp:DropDownList>
                       <ajaxToolkit:CascadingDropDown ID="_cddlline2" runat="server" Category="Line"  SelectedValue="<%#DirectCast(Container.DataItem, RoleBySite).Line%>"
                        LoadingText="Loading Lines..." PromptText="    "
                        ServiceMethod="GetLineList" ServicePath="~/WebServices/SiteDropDownsWS.asmx"
                        TargetControlID="_ddlLine" ContextKey="ALL" ParentControlID="_ddlArea"
                        UseContextKey="true">
                       </ajaxToolkit:CascadingDropDown>
                    </EditItemTemplate>
                </asp:TemplateField>--%>

                                <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderText="" ItemStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ForeColor="black" runat="server" ToolTip='<%$IPResources:Global,Delete%>' ID="_btnDelete" CommandArgument='<%# DirectCast(Container.DataItem, RoleBySite).PlantCode & "," & DirectCast(Container.DataItem, RoleBySite).RoleId & "," & DirectCast(Container.DataItem, RoleBySite).AssigneeNetworkId & "," & DirectCast(Container.DataItem, RoleBySite).BusUnit & "," & DirectCast(Container.DataItem, RoleBySite).Area & "," & DirectCast(Container.DataItem, RoleBySite).Line %>' OnClick="DeleteRoleAssignment"><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </RWG:BulkEditGridView>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-md-4 col-md-offset-4">
                            <asp:LinkButton ID="_btnUpdateRoles" runat="server" SkinID="Button"
                                Visible="true" ValidationGroup="RoleEntry" CausesValidation="true" OnClientClick="return CheckForm('RoleEntry');">
                                <asp:PlaceHolder runat="server">
                                    <span class="glyphicon glyphicon-floppy-save"></span>&nbsp;<asp:Label ID="Label7" runat="server" Text="<%$IPResources:Global,Update Role Assignment%>"></asp:Label>
                                </asp:PlaceHolder>
                            </asp:LinkButton>
                        </div>
                    </div>

                </div>
               
            </div>



        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function BindEvents() {
            $(document).ready(function () {
                var behavior = $find('cddlBusinessUnit');
                if (behavior !== null) {
                    behavior.add_populated(function () {
                        $('#_newddlBusUnit').selectpicker('refresh')
                    });
                }
                behavior = $find('cddlArea');
                if (behavior !== null) {
                    behavior.add_populated(function () {
                        $('#_newddlArea').selectpicker('refresh')
                    });
                }
                behavior = $find('cddlLine');
                if (behavior !== null) {
                    behavior.add_populated(function () {
                        $('#_newddlLine').selectpicker('refresh')
                    });
                }

            });
        }
        Sys.Application.add_load(BindEvents);
    </script>
</asp:Content>
