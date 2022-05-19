<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" 
    AutoEventWireup="false" CodeFile="DataMaintenance.aspx.vb" Inherits="DataMaintenance" %>

<%@ Register Assembly="RIDataMaintenance" Namespace="RIDataMaintenance" TagPrefix="IP" %>
<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">

    <asp:UpdatePanel ID="_udpMaintenance" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <style>
                .ccTable{
                    
                }
            </style>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="_lblHeaderTitle" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="panel-body">
                    <center><IP:DataMaintenaceSelector ID="_RIDataMaintenance"  LabelCssClass="MaintenanceGridLabels" ApplicationText="Application" Width="80%" SiteText="Site" GridCssClass="MaintenanceGridRow" AlternatingRowCssClass="MaintenanceGridAlternatingRow" headerstylecssclass="MaintenanceGridHeader" runat="server" InstructionCssClass="MaintenanceInstructionalText" DefaultApplication="MTT" /></center>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

