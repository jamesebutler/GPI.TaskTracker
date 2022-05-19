<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EmployeeSearch.ascx.vb"
    Inherits="User_Controls.EmployeeSearch" %>
<div id="_divUserList" runat="server" style="min-height: 50px;">
    <div style="display: inline;">
        <asp:Label ID="_lblRequiredField" Text="*" CssClass="labelerror" runat="server"></asp:Label>&nbsp;<asp:Label
            ID="_lblEmployeeListCaption" Text="Employee List" runat="server"></asp:Label>
        <asp:Label ID="_lblResponsibleUser" runat="server" Font-Bold="false" SkinID="none"></asp:Label>
        <asp:TextBox ID="_txtResponsibleUserValue" runat="server" Style="display: none"
            AutoPostBack="true" AutoCompleteType="None"></asp:TextBox>
        <br />
        <asp:LinkButton ID="_btnSearch" runat="server" Font-Size="Smaller" Text="<%$IPResources:Global,Search%>"
            OnClientClick="return false;" Style="padding-right: 100px;"></asp:LinkButton><asp:LinkButton
                ID="_btnClearSelection" runat="server" Font-Size="Smaller" Text="<%$IPResources:Global,Clear Selection%>"
                Style="display: none"></asp:LinkButton>
        <asp:Panel ID="_pnlEmployeeLookup" runat="server" BackColor="White" ForeColor="Black"
            Style="position: fixed; display: none" Width="550px">
            <asp:Table ID="_tblMain" runat="server" Width="100%" CssClass="modalPopup">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="_lblSearchText" runat="server" Text="Search"></asp:Label>:
                        <asp:TextBox ID="_txtSearchBox" runat="server" Width="300px">
                        </asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <select id="_lbSearchResults" runat="server" size="10" class="fixedWidthFont" multiple="false"
                            style="width: 95%">
                        </select>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center">
                        <%-- <asp:Button ID="_btnSelect" runat="server" Text="<%$IPResources:Global,OK%>" Width="150px" />
                        &nbsp;&nbsp;--%>
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
    &nbsp;
</div>
