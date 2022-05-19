<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false"
    CodeFile="EditRoles.aspx.vb" Inherits="EditRoles" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<%@ Register Namespace="RealWorld.Grids" TagPrefix="rwg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <div>
        <asp:UpdatePanel ID="_udpSubTask" runat="server" UpdateMode="always">
            <ContentTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="panel-title">
                            <asp:Label ID="_lblHeaderTitle" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Label ID="_lblInactiveStatus" Visible="false" Font-Bold="True" Height="40px"
                                    runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-2">                               
                                <IP:EmployeeList ID="_employeeList" Employeelabel="Name" runat="server" EnableValidation="false">
                                </IP:EmployeeList>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label ID="Label1" Text="<%$IPResources:Global,RoleDescription%>" runat="server"></asp:Label><span
                                    style="color: #ff3366">*</span><br />
                                <asp:Label ID="_lblRoleDescription" runat="server" Width="200px" CssClass="form-control" Font-Bold="false"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label ID="Label2" Text="<%$IPResources:Global,BusUnit%>" runat="server"></asp:Label><br />
                                <asp:DropDownList ID="_ddlBusUnit" runat="server" Width="200px" ClientIDMode="Static">
                                </asp:DropDownList>
                                <ajaxToolkit:CascadingDropDown ID="_cddlBusinessUnit" runat="server" Category="BusinessUnit" BehaviorID="cddlBusinessUnit"
                                    LoadingText="[Loading Business Units...]" PromptText="    " ServiceMethod="GetBusinessUnitList"
                                    ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlBusUnit" ParentControlID="ctl00__cphMainContent__employeeList__lbFacility"
                                    ContextKey="ALL" 
                                    UseContextKey="true"></ajaxToolkit:CascadingDropDown>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label ID="Label3" Text="<%$IPResources:Global,Area%>" runat="server"></asp:Label><br />
                                <asp:DropDownList ID="_ddlArea" runat="server" Width="200px" ClientIDMode="Static">
                                </asp:DropDownList>
                                <ajaxToolkit:CascadingDropDown ID="_cddlArea" runat="server" Category="Area" LoadingText="[Loading Areas...]" BehaviorID="cddlArea"
                                    PromptText="    " ServiceMethod="GetAreaList" ServicePath="~/WebServices/SiteDropDownsWS.asmx"
                                    TargetControlID="_ddlArea" ContextKey="ALL" ParentControlID="_ddlBusUnit" UseContextKey="true"></ajaxToolkit:CascadingDropDown>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label ID="Label4" Text="<%$IPResources:Global,Line%>" runat="server"></asp:Label><br />
                                <asp:DropDownList ID="_ddlLine" runat="server" Width="200px" ClientIDMode="Static">
                                </asp:DropDownList>
                                <ajaxToolkit:CascadingDropDown ID="_cddlLine" runat="server" Category="Line" LoadingText="[Loading Line...]" BehaviorID="cddlLine"
                                    PromptText="    " ServiceMethod="GetLineList" ServicePath="~/WebServices/SiteDropDownsWS.asmx"
                                    TargetControlID="_ddlLine" ContextKey="ALL" ParentControlID="_ddlArea" UseContextKey="true"></ajaxToolkit:CascadingDropDown>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer">
                        <div class="row">
                            <div class="col-xs-12 col-sm-3 col-sm-offset-3">
                                <div class="form-group">
                                    <asp:LinkButton ID="_btnCancel" runat="server" SkinID="Button">
                                        <span class="glyphicon glyphicon-ban-circle"></span>&nbsp;<asp:Label ID="Label5" runat="server" Text="<%$IPResources:Global,Cancel%>"></asp:Label>
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-3">
                                <div class="form-group">
                                    <asp:LinkButton ID="_btnUpdateRole" runat="server" SkinID="ButtonPrimary">
                                        <span class="glyphicon glyphicon-floppy-save"></span>&nbsp;<asp:Label ID="_lblSaveButton" runat="server" Text="<%$IPResources:Global,Update Role Assignment%>"></asp:Label>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
    function BindEvents() {
        $(document).ready(function () {
            behavior = $find('cddlBusinessUnit');
            if (behavior !== null) {
                behavior.add_populated(function () { $('#_ddlBusUnit').selectpicker('refresh') });
            }
            behavior = $find('cddlArea');
            if (behavior !== null) {
                behavior.add_populated(function () { $('#_ddlArea').selectpicker('refresh') });
            }
            behavior = $find('cddlLine');
            if (behavior !== null) {
                behavior.add_populated(function () { $('#_ddlLine').selectpicker('refresh') });
            }
           
        });
    }
    Sys.Application.add_load(BindEvents);
</script>
</asp:Content>
