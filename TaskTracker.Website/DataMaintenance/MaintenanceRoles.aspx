<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false"
    CodeFile="MaintenanceRoles.aspx.vb" Inherits="MaintenanceRole" Title="Untitled Page" %>

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
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="Label3" runat="server" Text="<%$IPResources:Global,Role Maintenance for%>"
                                    Font-Size="Medium"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <asp:RequiredFieldValidator ID="_rfvFacility" runat="server" ControlToValidate="_ddlsitelist"
                                    ValidationGroup="DataMaintEmp" ErrorMessage="<%$IPResources:Global,Facility is a required field%>"
                                    Display="Dynamic" Text="*" InitialValue="0"></asp:RequiredFieldValidator>
                                <asp:DropDownList ID="_ddlsitelist" runat="server" AutoPostBack="true" Style="min-width: 192px;">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-body">

                    <div class="panel panel-default">
                        <div class="row">
                            <div class="col-md-4 col-lg-offset-1 col-lg-2">
                                <IP:EmployeeList ID="_employeeList" UserMode="UsersOnly" Employeelabel="<%$IPResources:Global,Name%>" AllowFacilityChange="True"
                                    runat="server" EnableValidation="true">
                                    <validation validationgroup="roleMaint" validationerrormessage="<%$IPResources:Global,Responsible Person is required%>"
                                        validationerrortext="" validateemptytext="true" />
                                </IP:EmployeeList>
                            </div>
                            <div class="col-md-4 col-lg-2">
                                <span class="required">*</span>&nbsp;<asp:Label ID="Localize1" runat="server" Text="<%$IPResources:Global,Role Description%>"></asp:Label><br />
                                <asp:DropDownList ID="_newddlRoleDescription" runat="server" ValidationGroup="roleMaint" CausesValidation="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="_rfvRoleDescription" runat="server" ControlToValidate="_newddlRoleDescription"
                                    Text="" ErrorMessage="<%$IPResources:Global,Role Description is required%>" ValidationGroup="roleMaint"
                                    Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 col-lg-2">
                                <span class="required">*</span>&nbsp;<asp:Label ID="Localize2" runat="server" Text="<%$IPResources:Global,Business Unit%>"></asp:Label><br />
                                <asp:DropDownList ID="_newddlBusUnit" runat="server" ClientIDMode="Static">
                                </asp:DropDownList>
                                <ajaxToolkit:CascadingDropDown ID="_cddlBusinessUnit" runat="server" Category="BusinessUnit" BehaviorID="cddlBusinessUnit"
                                    LoadingText="<%$IPResources:Global,Loading Business Units%>"
                                    ServiceMethod="GetBusinessUnitList" ServicePath="~/WebServices/SiteDropDownsWS.asmx"
                                    TargetControlID="_newddlBusUnit" ContextKey="ALL" ParentControlID="_ddlSiteList"
                                    UseContextKey="true"></ajaxToolkit:CascadingDropDown>
                                <asp:RequiredFieldValidator ID="_rvfBusinessUnit" runat="server" ControlToValidate="_newddlBusUnit"
                                    Text="" ErrorMessage="<%$IPResources:Global,Business Unit is required%>" ValidationGroup="roleMaint"
                                    Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 col-lg-2">
                                <span class="required">*</span>&nbsp;<asp:Label ID="Localize3" runat="server" Text="<%$IPResources:Global,Area%>"></asp:Label><br />

                                <asp:DropDownList ID="_newddlArea" runat="server" ClientIDMode="Static">
                                </asp:DropDownList>
                                <ajaxToolkit:CascadingDropDown ID="_cddlArea" runat="server" Category="Area" LoadingText="<%$IPResources:Global,Loading Areas%>"
                                    ServiceMethod="GetAreaList" ServicePath="~/WebServices/SiteDropDownsWS.asmx"
                                    TargetControlID="_newddlArea" ContextKey="ALL" ParentControlID="_newddlBusUnit"  BehaviorID="cddlArea"
                                    UseContextKey="true"></ajaxToolkit:CascadingDropDown>
                                <asp:RequiredFieldValidator ID="_rfvArea" runat="server" ControlToValidate="_newddlArea" 
                                    Text="" ErrorMessage="<%$IPResources:Global,Area is required%>" ValidationGroup="roleMaint"
                                    Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4 col-lg-2">
                                <span class="required">*</span>&nbsp;<asp:Label ID="Localize4" runat="server" Text="<%$IPResources:Global,Line%>"></asp:Label><br />
                                <asp:DropDownList ID="_newddlLine" runat="server" ClientIDMode="Static">
                                </asp:DropDownList>
                                <ajaxToolkit:CascadingDropDown ID="_cddlLine" runat="server" Category="Line" LoadingText="<%$IPResources:Global,Loading Line%>"
                                    ServiceMethod="GetLineList" ServicePath="~/WebServices/SiteDropDownsWS.asmx"
                                    TargetControlID="_newddlLine" ContextKey="ALL" ParentControlID="_newddlArea"  BehaviorID="cddlLine"
                                    UseContextKey="true"></ajaxToolkit:CascadingDropDown>
                                <asp:RequiredFieldValidator ID="_rfvLine" runat="server" ControlToValidate="_newddlLine"
                                    Text="" ErrorMessage="<%$IPResources:Global,Line is required%>" ValidationGroup="roleMaint"
                                    Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="panel-footer">
                            <div class="row">
                                <%--<div class="col-md-4 col-md-offset-2">
                                    <asp:LinkButton ID="LinkButton1" runat="server" SkinID="Button"
                                        Visible="true"  CausesValidation="false" OnClientClick="Page_ClientValidate(''); return false;">
                                        <asp:PlaceHolder runat="server">
                                            <span class="glyphicon glyphicon-remove-circle"></span>&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Cancelt%>"></asp:Label>
                                        </asp:PlaceHolder>
                                    </asp:LinkButton>
                                    
                                </div>--%>
                                <div class="col-md-4 col-md-offset-4">
                                    <asp:LinkButton ID="_btnAddUserRole" runat="server" SkinID="Button"
                                        Visible="true" ValidationGroup="roleMaint" CausesValidation="true" OnClientClick="return CheckForm('roleMaint');">
                                        <asp:PlaceHolder runat="server">
                                            <span class="glyphicon glyphicon-check"></span>&nbsp;<asp:Label ID="Label8" runat="server" Text="<%$IPResources:Global,Add New Role Assignment%>"></asp:Label>
                                        </asp:PlaceHolder>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="panel-body">
                    <section id="no-more-gridView">
                        <asp:GridView ID="_gvRoleList" runat="server" 
                            AllowSorting="false" 
                            AutoGenerateDeleteButton="false"
                            AutoGenerateEditButton="false" 
                            AutoGenerateColumns="False" 
                            CellPadding="1" Width="100%"
                            CssClass="tablesorter {sortlist: [[1,0]]}">

                            <Columns>
                                <%--                <asp:TemplateField  HeaderText="RoleID" Visible="False" > <itemtemplate>
                <asp:TextBox ID="_tbRoleID" runat="server" Visible="False" text="<%#DirectCast(Container.DataItem, SiteUserRole).RoleId%>" width="100px" ></asp:TextBox>
                </itemtemplate>  </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="<%$IPResources:Global,Name%>" SortExpression="Name"
                                    ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <%--                <asp:Label ID="Label1" runat="server" text="<%#DirectCast(Container.DataItem, SiteUserRole).AssigneeName%>" width="200px" ></asp:Label>
                                        --%>
                                        <a target="_top" href="Javascript:parent.window.location=('<%#String.Format(CultureInfo.CurrentCulture, Page.ResolveUrl("~/DataMaintenance/EditRoles.aspx?Plantcode={0}&Roleid={1}&RoleDescription={2}&Userid={3}&Busunit={4}&Area={5}&Line={6}"), DirectCast(Container.DataItem, SiteUserRole).PlantCode, DirectCast(Container.DataItem, SiteUserRole).RoleId, DirectCast(Container.DataItem, SiteUserRole).RoleDescription, DirectCast(Container.DataItem, SiteUserRole).AssigneeNetworkId, DirectCast(Container.DataItem, SiteUserRole).BusUnit, DirectCast(Container.DataItem, SiteUserRole).Area, DirectCast(Container.DataItem, SiteUserRole).Line) %>');"
                                            title="Edit Role Assignment">
                                            <%#DirectCast(Container.DataItem, SiteUserRole).AssigneeName%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Role Description" HeaderText="<%$IPResources:Global,Role Description%>"
                                    ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="_tbRoleDescription" runat="server" Text="<%#IP.Bids.SharedFunctions.LocalizeValue(DirectCast(Container.DataItem, SiteUserRole).RoleDescription, True)%>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global,Business Unit%>" SortExpression="Business Unit"
                                    ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="_tbBusUnit" runat="server" Text="<%# IP.Bids.SharedFunctions.LocalizeValue(DirectCast(Container.DataItem, SiteUserRole).BusUnit, True)%>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global,Area%>" SortExpression="Area"
                                    ItemStyle-Width="15%">
                                    <ItemTemplate>
                                       <%-- <asp:Label ID="_tbArea" runat="server" Text="<%#IP.Bids.SharedFunctions.LocalizeValue(DirectCast(Container.DataItem, SiteUserRole).Area, True)%>"></asp:Label>--%>
                                        <asp:Label ID="_tbArea" runat="server" Text="<%# DirectCast(Container.DataItem, SiteUserRole).Area %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$IPResources:Global,Line%>" SortExpression="Line"
                                    ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="_tbLine" runat="server" Text="<%#IP.Bids.SharedFunctions.LocalizeValue(DirectCast(Container.DataItem, SiteUserRole).Line, True)%>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField ItemStyle-VerticalAlign="Top"  HeaderText="">
                <ItemTemplate>
                  <asp:Button ID="_btnEdit"   runat="server" Text="Edit" OnClick="EditRoleAssignment"  CommandArgument='<%# DirectCast(Container.DataItem, SiteUserRole).PlantCode & "," & DirectCast(Container.DataItem, SiteUserRole).Roleid & "," & DirectCast(Container.DataItem, SiteUserRole).AssigneeNetworkID & "," & DirectCast(Container.DataItem, SiteUserRole).BusUnit & "," & DirectCast(Container.DataItem, SiteUserRole).Area & "," & DirectCast(Container.DataItem, SiteUserRole).Line %>' />
                 </ItemTemplate>
                 </asp:TemplateField>--%>
                                <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderText="" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <div class="form-group">
                                            <asp:LinkButton ID="_btnDelete" runat="server" SkinID="Button"
                                                Visible="true" CausesValidation="false" OnClick="DeleteRoleAssignment" CommandArgument='<%# DirectCast(Container.DataItem, SiteUserRole).PlantCode & "," & DirectCast(Container.DataItem, SiteUserRole).RoleId & "," & DirectCast(Container.DataItem, SiteUserRole).AssigneeNetworkId & "," & DirectCast(Container.DataItem, SiteUserRole).BusUnit & "," & DirectCast(Container.DataItem, SiteUserRole).Area & "," & DirectCast(Container.DataItem, SiteUserRole).Line %>'>
                                                <asp:PlaceHolder runat="server">
                                                    <span class="glyphicon glyphicon-trash"></span>&nbsp;<asp:Label ID="Label8" runat="server" Text="<%$IPResources:Global,Delete%>"></asp:Label>
                                                </asp:PlaceHolder>
                                            </asp:LinkButton>

                                        </div>
                                        <ajaxToolkit:ConfirmButtonExtender ID="_cbeCancelRecurrence" runat="server" TargetControlID="_btnDelete"
                                            ConfirmText="<%$IPResources:Global,Are you sure that you would like to [Delete] this Role Assignment?%>"></ajaxToolkit:ConfirmButtonExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </section>
                </div>


            </div>

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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
