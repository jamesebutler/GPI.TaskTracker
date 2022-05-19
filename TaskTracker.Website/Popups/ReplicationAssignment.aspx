<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerPopup.master" AutoEventWireup="false"
    CodeFile="ReplicationAssignment.aspx.vb" Inherits="ReplicationAssignment" ClientIDMode="Static" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerPopup.master" %>

<%@ Register Src="~/User Controls/Tasks/ReplicationAssignment.ascx" TagName="ReplicationAssignment"
    TagPrefix="IP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
                <asp:Label ID="_lblHeaderTitle" runat="server" Text="<%$IPResources:Global,Assign/View Replication%>"></asp:Label>
            </div>
        </div>
        <div class="panel-body">
             <asp:Panel ID="_pnlProcessReplication" CssClass="panel panel-default" runat="server" ScrollBars="auto" Height="400" Width="100%">
            <IP:ReplicationAssignment ID="_replicationAssignment" runat="server" />
        </asp:Panel>
        </div>
        <div class="panel-footer">
            <div class="row">
                 <div class="form-group col-sm-3">
            <asp:LinkButton ID="_btnProcessReplicaitonTop" runat="server" SkinID="Button">
                <span class="glyphicon glyphicon-log-in"></span>&nbsp;<asp:Label ID="_lblProcessReplication" runat="server" Text="<%$IPResources:Global,Process Replication%>"></asp:Label>
            </asp:LinkButton>
        </div>
            </div>
            
        </div>
    </div>

</asp:Content>

