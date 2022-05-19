<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AdvancedEmployeeListDropdown.ascx.vb"
    Inherits="User_Controls.AdvancedEmployeeListDropdown" %>
<div id="_divUserList" runat="server">
    <div style="display: inline;">
       
        <asp:PlaceHolder ID="_plhUserTop" runat="server" EnableViewState="false">
            <asp:Label ID="_lblRequiredField" Text="*" CssClass="labelerror" runat="server"></asp:Label>&nbsp;<asp:Label ID="_lblEmployeeListCaption" EnableViewState="false" Text="Employee List"
                runat="server"></asp:Label>
            <asp:LinkButton ID="_btnFacilityList" runat="server" Text="<%$IPResources:Global,Facility%>"
                EnableViewState="false" CssClass="pull-right">
            </asp:LinkButton>
            <asp:DropDownList ID="_lbFacility" ClientIDMode="Predictable" runat="server" CssClass="selectPicker facilitypicker" EnableTheming="false" Style="display: none" data-live-search-style="contains" data-live-search="true" data-container="body" data-width="10px" data-size="5"></asp:DropDownList>
        </asp:PlaceHolder>
        <asp:DropDownList
            ID="_ddlResponsibleUser" runat="server" Style="min-width: 100px;" ClientIDMode="AutoID">
        </asp:DropDownList>
        
        
        <asp:PlaceHolder ID="_plhUserBottom" runat="server" EnableViewState="false">
            <asp:LinkButton ID="_btnSearch" runat="server" Text="<%$IPResources:Global,Search%>"
                OnClientClick="return false;" EnableViewState="false"></asp:LinkButton><asp:LinkButton
                    ID="_btnClearSelection" runat="server" Text="<%$IPResources:Global,Clear Selection%>"
                    EnableViewState="false" CssClass="pull-right"></asp:LinkButton>
        </asp:PlaceHolder>
        <asp:Panel ID="_pnlFacility" runat="server" BackColor="black" ForeColor="black" Style="position: fixed; display: none;">
        </asp:Panel>
        <asp:Panel ID="_pnlEmployeeLookup" runat="server" BackColor="White" ForeColor="Black"
            Style="position: fixed; display: none" Width="550px">
            <asp:Table ID="_tblMain" runat="server" Width="100%" CssClass="modalPopup">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="_lblSearchText" runat="server" Text="Search" Style="white-space: nowrap;"></asp:Label>:
                        <asp:TextBox ID="_txtSearchBox" runat="server" Width="300px">
                        </asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <select id="_lbSearchResults" runat="server" size="10" class="fixedWidthFont form-control" multiple="false"
                            style="width: 95%">
                        </select>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Button ID="_btnCancel" runat="server" Text="<%$IPResources:Global,Cancel%>"
                            Width="150px" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
        <ajaxToolkit:PopupControlExtender ID="_pceUsers" runat="server" TargetControlID="_btnSearch"
            BehaviorID="_bhUsers" PopupControlID="_pnlEmployeeLookup" CommitProperty="Text"
            Position="Bottom" CacheDynamicResults="true" OffsetX="10" OffsetY="10" CommitScript="this.blur();">
        </ajaxToolkit:PopupControlExtender>
    </div>
</div>

