<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ReplicationAssignment.ascx.vb" ClientIDMode="Static"
    Inherits="User_Controls_Tasks_ReplicationAssignment" %>
<div class="row">
    <div class="col-xs-12 col-md-6">
        <div class="panel panel-default" style="min-height:300px">
            <div class="panel-heading"><asp:label runat=server Text="<%$IPResources:Shared, Available Sites%>"></asp:label></div>
            <div class="panel-body">
                <IP:TreeViewList ID="_tvPlantModel" runat="server" ShowHiddenFields="false" EnableViewState="false"
                    AllowCheckAll="false" CheckAllText="<%$IPResources:Shared, Check All%>" ExpandAllText="<%$IPResources:Shared, Expand All%>" CollapseAllText="<%$IPResources:Shared, Collapse All%>">
                </IP:TreeViewList>
            </div>
        </div>
    </div>
    <div class="col-xs-12 col-md-6">
        <div class="panel panel-default" style="min-height:300px">
            <div class="panel-heading"><asp:label runat=server Text="<%$IPResources:Shared, Replicated Sites%>"></asp:label></div>
            <div class="panel-body">
                <IP:TreeViewList ID="_tvReplicatedSites" runat="server" ShowHiddenFields="false"
                    EnableViewState="false" AllowCheckAll="false" ScrollBars="auto" Height="500" ExpandAllText="<%$IPResources:Shared, Expand All%>" CollapseAllText="<%$IPResources:Shared, Collapse All%>">
                </IP:TreeViewList>
            </div>
        </div>
    </div>
</div>
<IP:MessageBox ID="_msgProcessReplication" runat="server" ButtonType="OK" />
