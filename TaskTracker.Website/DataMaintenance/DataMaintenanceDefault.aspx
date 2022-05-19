<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false" CodeFile="DataMaintenanceDefault.aspx.vb" Inherits="DataMaintenanceDefault" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<%@ Register Assembly="RIDataMaintenance" Namespace="RIDataMaintenance" TagPrefix="IP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <style>
        .MaintenanceGridHeader {
            background-color: black;
            color: white;
            line-height: 30px;
        }

        .MaintenanceGridRow {
            background-color: #CCCC99;
        }

        .MaintenanceGridAlternatingRow {
            background-color: white;
        }

        .MaintenanceInstructionalText {
            border-style: solid;
            padding: 10px;
            border-width: 2px;
            text-align: center;
        }

            .MaintenanceInstructionalText span {
                font-size: 1.5em;
                color: black;
                border-width: 0px;
            }
    </style>
    <asp:UpdatePanel ID="_udpMaintenance" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <center><IP:DataMaintenaceSelector ID="_RIDataMaintenance" ApplicationText="Application" Width="900px" SiteText="Site" GridCssClass="MaintenanceGridRow" AlternatingRowCssClass="MaintenanceGridAlternatingRow" headerstylecssclass="MaintenanceGridHeader" runat="server" InstructionCssClass="MaintenanceInstructionalText" DefaultApplication="MTT"/></center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

