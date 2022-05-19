<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false"
    CodeFile="ReassignTasks.aspx.vb" Inherits="ReassignTasks" Title="Untitled Page" %>

<%@ Import Namespace="System.Globalization" %>
<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<%@ Register Namespace="RealWorld.Grids" TagPrefix="rwg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {

            $("#_gvReassignTaskList").tablesorter({
                // pass the headers argument and assing a object 
                headers: {
                    // assign the secound column (we start counting zero) 
                    0: {
                        // disable it by setting the property sorter to false 
                        sorter: false
                    }
                }
            });

            var headerChk = $(".chkHeader input");
            var itemChk = $(".chkItem input");

            headerChk.click(function () {
                itemChk.each(function () {
                    this.checked = headerChk[0].checked;
                })
            });

            itemChk.each(function () {
                $(this).click(function () {
                    if (this.checked === false) { headerChk[0].checked = false; }
                })
            });
        });

    </script>
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
                <asp:Label ID="_lblHeaderTitle" runat="server"></asp:Label>
            </div>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <asp:Label runat="server" Text="<%$IPResources:Global,Facility%>"></asp:Label><br />
                        <asp:DropDownList ID="_ddlFacility" runat="server" ClientIDMode="Static">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <asp:Label runat="server" Text="<%$IPResources:Global,Business Unit%>"></asp:Label><br />
                        <asp:DropDownList ID="_ddlBusUnit" runat="server" ClientIDMode="Static">
                        </asp:DropDownList>
                        <ajaxToolkit:CascadingDropDown ID="_cddlBusinessUnit" runat="server" Category="BusinessUnit" BehaviorID="ccdBusArea"
                            LoadingText="<%$IPResources:Global,Loading Business Units%>" PromptText="    "
                            ServiceMethod="GetBusinessUnitList" ServicePath="~/WebServices/SiteDropDownsWS.asmx"
                            TargetControlID="_ddlBusUnit" ContextKey="ALL" ParentControlID="_ddlFacility"
                            UseContextKey="true"></ajaxToolkit:CascadingDropDown>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <asp:Localize runat="server" Text="<%$IPResources:Global,Area%>"></asp:Localize><br />
                        <asp:DropDownList ID="_ddlArea" runat="server" ClientIDMode="Static">
                        </asp:DropDownList>
                        <ajaxToolkit:CascadingDropDown ID="_cddlArea" runat="server" Category="Area" LoadingText="<%$IPResources:Global,Loading Areas%>" BehaviorID="ccdArea"
                            PromptText="    " ServiceMethod="GetAreaList" ServicePath="~/WebServices/SiteDropDownsWS.asmx"
                            TargetControlID="_ddlArea" ContextKey="ALL" ParentControlID="_ddlBusUnit"
                            UseContextKey="true"></ajaxToolkit:CascadingDropDown>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <asp:Localize runat="server" Text="<%$IPResources:Global,Line%>"></asp:Localize><br>
                        <asp:DropDownList ID="_ddlLine" runat="server" ClientIDMode="Static">
                        </asp:DropDownList>
                        <ajaxToolkit:CascadingDropDown ID="_cddlLine" runat="server" Category="Line" LoadingText="<%$IPResources:Global,Loading Line%>" BehaviorID="cddlLine"
                            PromptText="    " ServiceMethod="GetLineList" ServicePath="~/WebServices/SiteDropDownsWS.asmx"
                            TargetControlID="_ddlLine" ContextKey="ALL" ParentControlID="_ddlArea"
                            UseContextKey="true"></ajaxToolkit:CascadingDropDown>

                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <asp:Label runat="server" Text="<%$IPResources:Global,Type%>"></asp:Label><br />
                        <IP:ExtendedCheckBoxList ID="_cblIncidentType" RepeatLayout="table" runat="server"
                            RepeatDirection="Vertical" Width="100%" AllowCheckAll="true" RepeatColumns="3"
                            ShowToolTip="true" AllTextValue="All" AllTextLabel="<%$IPResources:Global, All%>"
                            GroupingText="" Validation-Enabled="false">
                        </IP:ExtendedCheckBoxList>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <asp:Label runat="server" Text="<%$IPResources:Global,Activity%>"></asp:Label><br />
                        <IP:ExtendedCheckBoxList ID="_cblActivity" RepeatLayout="table" runat="server" RepeatDirection="Vertical"
                            Width="100%" AllowCheckAll="true" RepeatColumns="3" AllTextValue="All" AllTextLabel="<%$IPResources:Global, All%>"
                            ShowToolTip="true" Validation-Enabled="false" GroupingText="">
                        </IP:ExtendedCheckBoxList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <IP:EmployeeList ID="_ReassignedEmployeeList" UserMode="UsersAndRoles" Employeelabel="<%$IPResources:Global,Reassign From%>" AllowFacilityChange="True"
                            runat="server" EnableValidation="true" ShowInactiveUsers="true">
                            <validation validationgroup="ReassignEmp" validationerrormessage="<%$IPResources:Global,Responsible Person From is required%>"
                                validationerrortext="" validateemptytext="true" />
                        </IP:EmployeeList>
                    </div>
                </div>
                <div class="col-md-3 col-md-offset-3">
                    <div class="form-group">
                        <IP:EmployeeList ID="_NewReassignedEmployeeList" UserMode="UsersAndRoles" Employeelabel="<%$IPResources:Global,Reassign To%>" AllowFacilityChange="True"
                            runat="server" EnableValidation="true" showinactiveusers="false">
                            <validation validationgroup="ReassignEmp" validationerrormessage="<%$IPResources:Global,Responsible Person To is required%>"
                                validationerrortext="" validateemptytext="true" />
                        </IP:EmployeeList>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer">
            <div class="row">
                <div class="col-md-3 col-md-offset-3">
                    <div class="form-group">
                        <asp:LinkButton ID="_btnDisplay" runat="server" SkinID="Button">
                            <span class="glyphicon glyphicon-list-alt"></span>&nbsp;<asp:Label ID="Label11" runat="server" Text="<%$IPResources:Global,Display Tasks%>"></asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="col-md-3">
                     <div class="form-group">
                        <asp:LinkButton ID="_btnSubmit" runat="server" SkinID="Button"  OnClientClick="return CheckForm('ReassignEmp');">
                            <span class="glyphicon glyphicon-transfer"></span>&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Reassign Tasks%>"></asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-body text-center">
            <asp:Label ID="_lblNoRecords" runat="server" Visible="false" Font-Bold="True" Font-Size="Large" Text="<%$IPResources:Global,NoRecordsFound%>" ForeColor="Red" />
        </div>
        <div class="panel-body">
            <asp:GridView ID="_gvReassignTaskList" ClientIDMode="Static" runat="server" AllowSorting="false" AutoGenerateDeleteButton="false"
                AutoGenerateEditButton="false" AutoGenerateColumns="False" CellPadding="1" Width="100%" HorizontalAlign="Center"
                DataKeyNames="Taskitemseqid,Taskheaderseqid" CssClass="tablesorter" EnableViewState="true">
                <AlternatingRowStyle ForeColor="black" Font-Bold="false" />
                <RowStyle Wrap="true" ForeColor="black" Font-Bold="false" HorizontalAlign="Left" />

                <Columns>
                    <asp:TemplateField SortExpression="" HeaderText="<%$IPResources:Global,Reassign%>"
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="_cbAll" runat="server" CssClass="chkHeader" Text="<%$IPResources:Global,Reassign%>" Checked="true" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="_cbReassign" CssClass="chkItem" runat="server" Checked="true"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="HeaderTitle" HeaderText="<%$IPResources:Global,Header Title%>">
                        <ItemTemplate>
                            <asp:Label ID="_lblHeaderTitle" runat="server" Text="<%#DirectCast(Container.DataItem, ReassignTask).HeaderTitle%>"> </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="TaskTitle" HeaderText="<%$IPResources:Global,Task Title%>">
                        <ItemTemplate>
                            <asp:Label ID="_lblTaskTitle" runat="server" Text="<%#DirectCast(Container.DataItem, ReassignTask).TaskTitle%>"> </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="TaskDescription" HeaderText="<%$IPResources:Global,Task Description%>">
                        <ItemTemplate>
                            <asp:Label ID="_lblTaskDescription" runat="server" Text="<%#DirectCast(Container.DataItem, ReassignTask).TaskDescription%>"> </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="DueDate" HeaderText="<%$IPResources:Global,Due Date %>">
                        <ItemTemplate>
                            <asp:Label ID="_lblDueDate" runat="server" Text="<%#DirectCast(Container.DataItem, ReassignTask).Duedate%>"> </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>


    <div style="text-align: left">
        <asp:UpdatePanel ID="_udpRoleList" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <IP:ModalIframe ID="_btnEditFlag" runat="server" ButtonWidth="250px" DisplayModalButtonText="<%$IPResources:Global,Edit%>"
                    BannerText="<%$IPResources:Global,Edit Role%>" Width="70%" ReloadPageOnClose="false"
                    AllowChildToCloseParent="true" Url="DataMaintenance/EditRoles.aspx" Height="25%"
                    Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:CheckBox ID="_cbReassignAll" runat="server" CssClass="chkHeader" Checked="true" Visible="false"></asp:CheckBox>

        <asp:Label Text="<%$IPResources:Global,Reassign All%>" runat="server" Visible="false"></asp:Label>

        <div style="text-align: center;">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
       
        
            &nbsp;&nbsp;&nbsp;&nbsp;
        
        </div>



    </div>
    <script type="text/javascript">
        function BindEvents() {
            $(document).ready(function () {
                var behavior = $find('ccdFacility');
                if (behavior !== null) {
                    behavior.add_populated(function () {
                        $('#_ddlFacility').selectpicker('refresh')
                    });
                }
                behavior = $find('ccdBusArea');
                if (behavior !== null) {
                    behavior.add_populated(function () {
                        $('#_ddlBusUnit').selectpicker('refresh')
                    });
                }
                behavior = $find('ccdArea');
                if (behavior !== null) {
                    behavior.add_populated(function () {
                        $('#_ddlArea').selectpicker('refresh')
                    });
                }
                behavior = $find('cddlLine');
                if (behavior !== null) {
                    behavior.add_populated(function () {
                        $('#_ddlLine').selectpicker('refresh')
                    });
                }
            });
        }
        Sys.Application.add_load(BindEvents);
    </script>
</asp:Content>
