<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false" CodeFile="MissingAccountInfo.aspx.vb" Inherits="MissingAccountInfo" title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" Runat="Server">
<div>
            <h1 id="_missingAccount" runat="server">
                Unauthorized Access</h1>
             <center>
        <asp:Panel ID="_pnlUsingMyHelp" runat="server" BorderColor="black" BorderWidth="1" Width="90%">
            <table border="0" cellpadding="4" style="background-color: White; border-color: Black;
                font-size: 14px; width: 100%; height: 80%; text-align: left; border-width: thin"
                cellspacing="0">
                <tr class="BorderHeader">
                    <th>
                        <asp:Localize ID="Localize1" runat="server" Text="<%$IPResources:Shared,DefinitionProblemReportingTitle %>"></asp:Localize></th>
                    <th rowspan="4" style="width: 10px;">
                    </th> 
                    <th>
                        <asp:Localize ID="Localize2" runat="server" Text="<%$IPResources:Shared,DefinitionUsingMyHelpTitle %>"></asp:Localize></th>
                </tr>
                <tr class="BorderSecondary">
                    <td>
                    </td>
                    <td align="right">
                        <asp:HyperLink ID="_imgMyHelp" runat="server" NavigateUrl="http://MyHelp" Target="_blank"
                            ImageUrl="~/Images/MyHelpSm.gif" /><br />
                    </td>
                </tr>
                <tr class="BorderSecondary">
                    <td style="width: 50%;" valign="top">
                        <asp:Localize ID="Localize3" runat="server" Text="<%$IPResources:Shared,DefinitionProblemReporting %>"></asp:Localize>
                        &nbsp;<asp:HyperLink ID="HyperLink1" NavigateUrl="Mailto:james.butler@graphicpkg.com"
                            runat="server">james.butler@graphicpkg.com</asp:HyperLink>
                    </td>
                    <td style="width: 50%;" valign="top">
                        <asp:Localize ID="Localize4" runat="server" Text="<%$IPResources:Shared,DefinitionUsingMyHelp %>"></asp:Localize>
                    </td>
                </tr>
                <tr class="BorderSecondary">
                    <td>
                        <br />
                    </td>
                    <td>
                        <br />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </center>
        </div>
</asp:Content>

