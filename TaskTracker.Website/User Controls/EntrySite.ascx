<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EntrySite.ascx.vb" Inherits="UserControlsSite" %>
<%--<asp:Panel ID="_pnlEntrySite" runat="server" CssClass="Panel">
    <div class="panel-body">--%>
<div class="row">
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-4">
        <div class="form-group">
            <asp:Label ID="_lblDivision" runat="server" Text="<%$IPResources:Global,Business %>"></asp:Label>
            <br />
            <asp:DropDownList ID="_ddlDivision" ClientIDMode="Static" runat="server" Width="75%" SkinID="multiselectfilter">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-4">
        <div class="form-group">
            <asp:Label ID="_lblRegion" runat="server" Text="<%$IPResources:Global,Region %>"></asp:Label>
            <br />
            <asp:DropDownList ID="_ddlRegion" ClientIDMode="Static" runat="server" Width="75%" SkinID="multiselectfilter">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-4">
        <div class="form-group"></div>
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-4">
        <div class="form-group">

            <span class="required">*</span><asp:Label ID="_lblFacility" runat="server" Text="<%$IPResources:Global,Facility/Location %>" Style="vertical-align: bottom">
            </asp:Label>
            <asp:RequiredFieldValidator ID="_rfvFacility" runat="server" ControlToValidate="_ddlFacility"
                ErrorMessage="<%$IPResources:Global,Facility is required%>" Display="Dynamic" Text="" InitialValue="" CssClass="labelerror"></asp:RequiredFieldValidator>
            <br />
            <asp:DropDownList ID="_ddlFacility" ClientIDMode="Static" runat="server" Width="75%" SkinID="multiselectfilter">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-4">
        <div class="form-group">

            <span class="required">*</span><asp:Label ID="_lblBusArea" runat="server" Text="<%$IPResources:Global,Business Unit-Area/Department %>"></asp:Label>
            <asp:RequiredFieldValidator ID="_rfvBusinessUnit" runat="server" ControlToValidate="_ddlBusArea"
                ErrorMessage="<%$IPResources:Global,Business Unit-Area/Department is required%>" Display="Dynamic" Text="" InitialValue="" CssClass="labelerror"></asp:RequiredFieldValidator>
            <br />
            <asp:DropDownList ID="_ddlBusArea" ClientIDMode="Static" runat="server" Width="75%" CssClass="multiselectfilterd">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-4">
        <div class="form-group">
            <asp:Label ID="_lblLineBreak" runat="server" Text="<%$IPResources:Global,Line/Machine %>"></asp:Label>
            <br />
            <asp:DropDownList ID="_ddlLineLineBreak" ClientIDMode="Static" runat="server" Width="75%" CssClass="multiselectfilter">
            </asp:DropDownList>
        </div>
    </div>
</div>
<%-- </div>
</asp:Panel>--%>
<ajaxToolkit:CascadingDropDown ID="_cddlBusiness" runat="server" Category="Division" BehaviorID="ccdBusiness"
    LoadingText="[Loading...]" ServiceMethod="GetDivisionList"
    ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlDivision"
    ContextKey="" UseContextKey="true" SelectedValue="All"></ajaxToolkit:CascadingDropDown>
<ajaxToolkit:CascadingDropDown ID="_cddlRegion" runat="server" Category="Region" BehaviorID="ccdRegion"
    LoadingText="[Loading...]" ServiceMethod="GetRegionList"
    ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlRegion"
    ContextKey="All" ParentControlID="_ddlDivision" UseContextKey="true" SelectedValue="All"></ajaxToolkit:CascadingDropDown>
<ajaxToolkit:CascadingDropDown ID="_cddlFacility" runat="server" Category="Facility" BehaviorID="ccdFacility"
    LoadingText="[Loading...]" ServiceMethod="GetFacilityList"
    ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlFacility"
    ContextKey="" ParentControlID="_ddlRegion" UseContextKey="true"></ajaxToolkit:CascadingDropDown>
<ajaxToolkit:CascadingDropDown ID="_cddlBusArea" runat="server" Category="BusinessUnitArea" BehaviorID="ccdBusArea"
    LoadingText="[Loading...]" ServiceMethod="GetBusinessUnitAreaList"
    ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlBusArea"
    ContextKey="" ParentControlID="_ddlFacility" UseContextKey="true"></ajaxToolkit:CascadingDropDown>
<ajaxToolkit:CascadingDropDown ID="_cddlLineLineBreak" runat="server" Category="LineLineBreak" BehaviorID="ccdLineLineBreak"
    LoadingText="[Loading...]" PromptText="    " ServiceMethod="GetLineLineBreakList"
    ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlLineLineBreak"
    ContextKey="" ParentControlID="_ddlBusArea" UseContextKey="true" SelectedValue="none - none"></ajaxToolkit:CascadingDropDown>
<script type="text/javascript">
    function BindEvents() {
        $(document).ready(function () {
            behavior = $find('ccdBusiness');
            if (behavior !== null) {
                behavior.add_populated(function () { $('#_ddlDivision').selectpicker('refresh') });
            }
            behavior = $find('ccdRegion');
            if (behavior !== null) {
                behavior.add_populated(function () { $('#_ddlRegion').selectpicker('refresh') });
            }
            behavior = $find('ccdFacility');
            if (behavior !== null) {
                behavior.add_populated(function () { $('#_ddlFacility').selectpicker('refresh') });
            }
            behavior = $find('ccdBusArea');
            if (behavior !== null) {
                behavior.add_populated(function () { $('#_ddlBusArea').selectpicker('refresh') });
            }
            behavior = $find('ccdLineLineBreak');
            if (behavior !== null) {
                behavior.add_populated(function () { $('#_ddlLineLineBreak').selectpicker('refresh') });
            }
        });
    }
    Sys.Application.add_load(BindEvents);
</script>
