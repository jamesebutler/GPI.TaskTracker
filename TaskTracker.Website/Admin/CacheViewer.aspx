<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false"
    CodeFile="CacheViewer.aspx.vb" Inherits="AdminDisplayCache" Title="Cache Viewer"
    EnableViewState="false" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
                <asp:Label ID="_lblHeaderTitle" runat="server"></asp:Label>
            </div>
        </div>
        <div class="panel-body">
            <p>
                <asp:Label Text='<%$IPResources:Global,The contents of the ASP.NET application cache are: %>'
                    runat="server"></asp:Label>
            </p>
            <asp:PlaceHolder ID="phTable" runat="server" />
            <div class="panel-footer">
                <div class="row">

                    <div class="col-md-offset-2 col-xs-12 col-md-4">
                        <div class="form-group">
                            <asp:LinkButton ID="_btnRefresh" runat="server" SkinID="Button"
                                Visible="true">
                                <asp:PlaceHolder runat="server">
                                    <span class="glyphicon glyphicon-refresh"></span>&nbsp;<asp:Label ID="_lblRefresh" runat="server" Text="<%$IPResources:Global,Refresh%>"></asp:Label>
                                </asp:PlaceHolder>
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div class="col-xs-12 col-md-4">
                        <div class="form-group">
                            <asp:LinkButton ID="_btnDeleteAll" runat="server" SkinID="Button"
                                Visible="true">
                                <asp:PlaceHolder runat="server">
                                    <span class="glyphicon glyphicon-trash"></span>&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Delete All Cache Items%>"></asp:Label>
                                </asp:PlaceHolder>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <asp:Label ID="_txtMissingKeyInsert" EnableViewState="false" runat="server" Style="min-height: 400PX"
                        Width="100%">
        
                    </asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
