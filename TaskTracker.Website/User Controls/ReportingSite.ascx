<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ReportingSite.ascx.vb"
    Inherits="UserControlsReportingSite" ClientIDMode="Static" %>
<div class="row">
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-3">
        <div class="form-group">
            <asp:Label ID="_lblDivision" runat="server" Text="<%$IPResources:Global,Business %>"></asp:Label>
            <br />
            <asp:DropDownList ID="_ddlDivision" runat="server" Width="75%" ClientIDMode="Static">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-3">
        <div class="form-group">
            <asp:Label ID="_lblRegion" runat="server" Text="<%$IPResources:Global,Region %>"></asp:Label>
            <br />
            <asp:DropDownList ID="_ddlRegion" runat="server" Width="75%" ClientIDMode="Static"  SkinID="multiselectfilter">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-3">
        <div class="form-group">
            <asp:Label ID="_lblFacility" runat="server" Text="<%$IPResources:Global, Facility/Location%>" Style="vertical-align: bottom">
            </asp:Label>
            <br />
            <asp:DropDownList ID="_ddlFacility" runat="server" Width="75%" ClientIDMode="Static"  SkinID="multiselectfilter">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-xs-12 hidden-sm hidden-md">
    </div>
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-3">
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Business Unit %>"></asp:Label>
            <br />
            <asp:DropDownList ID="_ddlBusinessUnit" runat="server" Width="75%" ClientIDMode="Static"  SkinID="multiselectfilter">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-3">
        <div class="form-group">
            <asp:Label ID="_lblBusArea" runat="server" Text="<%$IPResources:Global, Area/Department%>"></asp:Label>
            <br />
            <asp:DropDownList ID="_ddlArea" runat="server" Width="75%" ClientIDMode="Static"  SkinID="multiselectfilter">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-3">
        <div class="form-group">
            <asp:Label ID="_lblLineBreak" runat="server" Text="<%$IPResources:Global,Line %>"></asp:Label>
            <br />
            <asp:DropDownList ID="_ddlLine" runat="server" Width="75%" ClientIDMode="Static"  SkinID="multiselectfilter">
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-3">
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global, Machine%>"></asp:Label>
            <br />
            <asp:DropDownList ID="_ddlLineBreak" runat="server" Width="75%" ClientIDMode="Static" SkinID="multiselectfilter">
            </asp:DropDownList>
        </div>
    </div>
</div>

<ajaxToolkit:CascadingDropDown ID="_cddlBusiness" runat="server" Category="Division"
    LoadingText="[Loading...]" ServiceMethod="GetDivisionList"
    ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlDivision" BehaviorID="ccdBusiness"
    ContextKey="" UseContextKey="true" SelectedValue="All"></ajaxToolkit:CascadingDropDown>
<ajaxToolkit:CascadingDropDown ID="_cddlRegion" runat="server" Category="Region" BehaviorID="cddlRegion"
    LoadingText="[Loading...]" ServiceMethod="GetRegionList"
    ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlRegion"
    ContextKey="ALL" ParentControlID="_ddlDivision" UseContextKey="true" SelectedValue="All"></ajaxToolkit:CascadingDropDown>
<ajaxToolkit:CascadingDropDown ID="_cddlFacility" runat="server" Category="Facility" BehaviorID="cddlFacility"
    LoadingText="[Loading...]" ServiceMethod="GetFacilityList"
    ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlFacility"
    ContextKey="ALL" ParentControlID="_ddlRegion" UseContextKey="true" SelectedValue="All"></ajaxToolkit:CascadingDropDown>
<ajaxToolkit:CascadingDropDown ID="_cddlBusinessUnit" runat="server" Category="BusinessUnit" BehaviorID="cddlBusinessUnit"
    LoadingText="[Loading...]" ServiceMethod="GetBusinessUnitList"
    ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlBusinessUnit"
    ContextKey="ALL" ParentControlID="_ddlFacility" UseContextKey="true"></ajaxToolkit:CascadingDropDown>
<ajaxToolkit:CascadingDropDown ID="_cddlArea" runat="server" Category="Area" BehaviorID="cddlArea"
    LoadingText="[Loading...]" ServiceMethod="GetAreaList"
    ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlArea"
    ContextKey="ALL" ParentControlID="_ddlBusinessUnit" UseContextKey="true"></ajaxToolkit:CascadingDropDown>
<ajaxToolkit:CascadingDropDown ID="_cddlLine" runat="server" Category="Line" BehaviorID="cddlLine"
    LoadingText="[Loading...]" ServiceMethod="GetLineList"
    ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlLine"
    ContextKey="ALL" ParentControlID="_ddlArea" UseContextKey="true"></ajaxToolkit:CascadingDropDown>
<ajaxToolkit:CascadingDropDown ID="_cddlLineBreak" runat="server" Category="LineBreak" BehaviorID="cddlLineBreak"
    LoadingText="[Loading...]" ServiceMethod="GetLineBreakList"
    ServicePath="~/WebServices/SiteDropDownsWS.asmx" TargetControlID="_ddlLineBreak"
    ContextKey="ALL" ParentControlID="_ddlLine" UseContextKey="true"></ajaxToolkit:CascadingDropDown>

<script type="text/javascript">
    function BindEvents() {
        $(document).ready(function () {
            var behavior;
            behavior = $find('ccdBusiness');
            if (behavior !== null) {
                behavior.add_populated(function () {  $('#_ddlDivision').selectpicker('refresh'); $('.multiselectfilter').selectpicker('refresh'); });
            }
            behavior = $find('cddlRegion');
            if (behavior !== null) {
                behavior.add_populated(function () {  $('#_ddlRegion').selectpicker('refresh') });
            }
            behavior = $find('cddlFacility');
            if (behavior !== null) {
                behavior.add_populated(function () { $('#_ddlFacility').selectpicker('refresh') });
            }
            behavior = $find('cddlBusinessUnit');
            if (behavior !== null) {
                behavior.add_populated(function () { $('#_ddlBusinessUnit').selectpicker('refresh') });
            }
            behavior = $find('cddlArea');
            if (behavior !== null) {
                behavior.add_populated(function () { $('#_ddlArea').selectpicker('refresh') });
            }
            behavior = $find('cddlLine');
            if (behavior !== null) {
                behavior.add_populated(function () { $('#_ddlLine').selectpicker('refresh') });
            }
            behavior = $find('cddlLineBreak');
            if (behavior !== null) {
                behavior.add_populated(function () { $('#_ddlLineBreak').selectpicker('refresh') });
            }
        });
    }
    Sys.Application.add_load(BindEvents);
</script>
