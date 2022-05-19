<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false" CodeFile="UserDefaults.aspx.vb" Inherits="DataMaintenance_UserDefaults" %>

<%@ Register Src="~/User Controls/ReportingSite.ascx" TagName="Site" TagPrefix="IP" %>
<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <%--  <style>
        input[type='radio'][disabled], input[type='checkbox'][disabled], input[type='radio'].disabled, input[type='checkbox'].disabled, fieldset[disabled] input[type='radio'], fieldset[disabled] input[type='checkbox'] {
            cursor: not-allowed;
        }
    </style>--%>
    <%--  <asp:UpdatePanel ID="_udpDefaults" runat="server" UpdateMode="Always">
        <ContentTemplate>--%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
                <asp:Label ID="_lblHeaderTitle" runat="server"></asp:Label>
            </div>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-4">
                    <IP:EmployeeList ID="_CurrentUser" runat="server" Employeelabel="<%$IPResources:Global, Profile User%>"
                        EnableValidation="true" Enabled="true" EnableViewState="true" Height="60px" DisplayClearLink="false" autopostback="true">
                        <validation validateemptytext="true" validationerrormessage="<%$IPResources:Global, Employee must be selected%>"
                            validationerrortext="*" />
                    </IP:EmployeeList>
                </div>
                <div class="col-md-8">
                    <asp:Label runat="server" Text="<%$IPResources:Global, Application%>"></asp:Label><br />
                    <asp:RadioButtonList ID="_cblApplications" runat="server" Width="100%" AutoPostBack="true" RepeatColumns="4">
                    </asp:RadioButtonList>
                    <%-- <IP:ExtendedCheckBoxList ID="_cblApplications" RepeatLayout="table" runat="server" AutoPostBack="true"
                                RepeatDirection="Vertical" Width="100%" AllowCheckAll="true"
                                ShowToolTip="true" AllTextValue="All" AllTextLabel="<%$IPResources:Global, All%>"
                                GroupingText="" Validation-Enabled="false">
                            </IP:ExtendedCheckBoxList>--%>
                </div>
            </div>

            <asp:Panel ID="_pnlAvailability" CssClass="panel panel-default" runat="server" Width="100%">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="Label1" runat="server" Text="Availability Tracking"></asp:Label>
                    </div>
                </div>
                <IP:Site ID="_siteAvailabilityTracking" runat="server" EnableViewState="true" />
            </asp:Panel>

            <asp:Panel ID="_pnlMTT" runat="server" CssClass="panel panel-default" Width="100%">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="Label2" runat="server" Text="Task Tracker (MTT)"></asp:Label>
                    </div>
                </div>
                <IP:Site ID="_siteTaskTracker" runat="server" EnableViewState="true" />
            </asp:Panel>

            <asp:Panel ID="_pnlTanks" runat="server" CssClass="panel panel-default" Width="100%">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="Label3" runat="server" Text="Tanks"></asp:Label>
                    </div>
                </div>
                <IP:Site ID="_siteTanks" runat="server" EnableViewState="true" />
            </asp:Panel>
        </div>
        <div class="panel-footer">
            <div class="row">
                <div class="col-md-4 col-md-offset-2">
                    <div class="form-group">
                        <asp:LinkButton ID="_btnCancel" runat="server" SkinID="Button">
                            <span class="glyphicon glyphicon-remove"></span>&nbsp;<asp:Label runat="server" Text="<%$IPResources:Global,Cancel%>"></asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                        <asp:LinkButton ID="_btnSave" runat="server" SkinID="ButtonPrimary">
                            <span class="glyphicon glyphicon-floppy-save"></span>&nbsp;<asp:Label runat="server" Text="<%$IPResources:Global,Save Changes%>"></asp:Label>
                        </asp:LinkButton>
                    </div>
                   
                </div>
                <%--<div class="col-md-4">
                    <asp:Button ID="_btnDefaultSettings"
                        runat="server" Text="<%$IPResources:Global, Use Default Settings%>" />
                </div>--%>
            </div>
        </div>
    </div>
    <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

