<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false"
    CodeFile="UsingMyHelp.aspx.vb" Inherits="HelpUsingMyHelp" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
     <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
              <%--  <asp:label ID="Label1" runat="server" Text="<%$IPResources:Shared,DefinitionUsingMyHelpTitle %>"></asp:label>--%>
                  <asp:label ID="Label1" runat="server" Text="Help"></asp:label>
            </div>
        </div>
        <div class="panel-body">
             <div class="row">
        <div class="col-md-offset-3 col-xs-12 col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:label ID="Localize5" runat="server" Text="<%$IPResources:Shared,DefinitionProblemReportingTitle %>"></asp:label>
                </div>
                <div class="panel-body">
                    <%--<asp:Localize ID="Localize3" runat="server" Text="<%$IPResources:Shared,DefinitionProblemReporting %>"></asp:Localize>--%>
  <br />
                All problem calls directed to GPI Help Desk number: 1-800-329-9630
				<br />

                <br />
                If you experience any problems that are not handled in timely manner or routed correctly,
				please contact &nbsp;<asp:HyperLink ID="HyperLink1" NavigateUrl="Mailto:James.Butler@graphicpkg.com"
                    runat="server">James.Butler@graphicpkg.com</asp:HyperLink>
                </div>
            </div>
        </div>
    </div>
   <%-- <div class="row">
        <div class="col-md-offset-3 col-xs-12 col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:label ID="Localize6" runat="server" Text="<%$IPResources:Shared,DefinitionUsingMyHelpTitle %>"></asp:label>
                    <asp:HyperLink ID="_imgMyHelp" runat="server" NavigateUrl="http://MyHelp.ipaper.com" Target="_blank"
                        CssClass="pull-right" Text="http://MyHelp.ipaper.com" />
                </div>
                <div class="panel-body">
                    <asp:Localize ID="Localize4" runat="server" Text="<%$IPResources:Shared,DefinitionUsingMyHelp %>"></asp:Localize>
                </div>
            </div>
        </div>
    </div>--%>
        </div>
    </div>
   
</asp:Content>
