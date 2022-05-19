<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EmployeeListDropdown.ascx.vb"
    Inherits="UserControlsEmployeeList2" %>
<%--<Asp:UpdatePanel ID="_udpUser" runat="server" UpdateMode="Always" RenderMode="Inline">
    <ContentTemplate>--%> <!-- <div style="display: inline; position: absolute;">-->
<div id="_divUserList" runat="server" style=" min-height: 50px;">
    <div style="display: inline;">
        <asp:Label ID="_lblRequiredField" Text="*" CssClass="labelerror" runat="server"></asp:Label>&nbsp;<asp:Label ID="_lblEmployeeListCaption" Text="Employee List" runat="server"></asp:Label>
        &nbsp;&nbsp;<asp:LinkButton ID="_btnFacilityList" runat="server" Text="<%$IPResources:Global,Facility%>"
            OnClientClick="return false;"></asp:LinkButton><br />
        <asp:DropDownList ID="_ddlResponsibleUser" runat="server">
        </asp:DropDownList><br />
        <asp:LinkButton ID="_btnClearSelection" runat="server" Font-Size="Smaller" Text="<%$IPResources:Global,Clear Selection%>"></asp:LinkButton>
        <br />
        <ajaxToolkit:PopupControlExtender ID="_pceFacility" runat="server" TargetControlID="_btnFacilityList"
            PopupControlID="_pnlFacility" CommitProperty="SelectedValue" Position="Bottom"
            OffsetX="10" OffsetY="10" CommitScript="this.blur();">
        </ajaxToolkit:PopupControlExtender>
        <!--Style="display: none; position: relative;
            left: 5px; top: 60px;"-->
        <asp:Panel ID="_pnlFacility" runat="server"  BackColor="white" ForeColor="black" style="position:fixed;">
            <asp:ListBox ID="_lbFacility" Rows="20" runat="server"></asp:ListBox>
        </asp:Panel>
    </div>&nbsp;
</div>
<%--</ContentTemplate>
</Asp:UpdatePanel>--%>
