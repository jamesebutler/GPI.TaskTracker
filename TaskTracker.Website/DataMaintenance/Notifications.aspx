<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master" AutoEventWireup="false"
    CodeFile="Notifications.aspx.vb" Inherits="UserProfileNotifications" Title="Untitled Page" %>


<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <div class="panel-title">
                <asp:Label ID="_lblHeaderTitle" runat="server"></asp:Label>
            </div>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-3">
                    <IP:EmployeeList ID="_NotificationUser" runat="server" Employeelabel="<%$IPResources:Global, Profile User%>"
                        EnableValidation="true" Enabled="true" EnableViewState="true">
                        <validation validateemptytext="true" validationerrormessage="<%$IPResources:Global, Employee must be selected%>"
                            validationerrortext="*" />
                    </IP:EmployeeList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <div class="panel-title">
                                <asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global, Task Creator Notify Me%>"></asp:Label>
                            </div>
                        </div>
                        <div class="panel-body">
                            <IP:NotificationFrequency ID="_notificationTaskCreator" runat="server" GroupingText=" "
                                UserType="Creator" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <div class="panel-title">
                                <asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global, Responsible Person Notify Me%>"></asp:Label>
                            </div>
                        </div>
                        <div class="panel-body">
                            <IP:NotificationFrequency ID="_notificationResponsible" runat="server" GroupingText=" "
                                UserType="Responsible" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <div class="panel-title">
                                <asp:Label ID="Label3" runat="server" Text="<%$IPResources:Global, Business Unit Manager or Type Manager Notify Me%>"></asp:Label>
                            </div>
                        </div>
                        <div class="panel-body">
                            <IP:NotificationFrequency ID="_notificationBusinessManager" runat="server" GroupingText=" "
                                UserType="BusinessUnitManager" Enabled="true" />
                        </div>
                    </div>
                </div>
                <%--<div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <div class="panel-title">
                                <asp:Label ID="Label4" runat="server" Text="<%$IPResources:Global, Type Manager Notify Me%>"></asp:Label>
                            </div>
                        </div>
                        <div class="panel-body">
                            <IP:NotificationFrequency ID="_notificationTypeManager" runat="server" GroupingText=" "
                                UserType="TypeManager" Enabled="true" />
                        </div>
                    </div>
                </div>--%>
            </div>
            <div class="row">
                <div class="panel-footer">
                    <div class="col-md-3 col-md-offset-2">
                         <asp:LinkButton ID="_btnCancel" runat="server" SkinID="Button">
                                <span class="glyphicon glyphicon-remove"></span>&nbsp;<asp:Label ID="Label6" runat="server" Text="<%$IPResources:Global,Cancel%>"></asp:Label>
                            </asp:LinkButton>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <asp:LinkButton ID="_btnSave" runat="server" SkinID="ButtonPrimary">
                                <span class="glyphicon  glyphicon-floppy-save"></span>&nbsp;<asp:Label ID="_lblSaveButton" runat="server" Text="<%$IPResources:Global,Save Changes%>"></asp:Label>
                            </asp:LinkButton>
                        </div>

                        
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <asp:LinkButton ID="_btnDefaultSettings" runat="server" SkinID="Button">
                                <span class="glyphicon glyphicon-refresh"></span>&nbsp;<asp:Label ID="Label5" runat="server" Text="<%$IPResources:Global,Use Default Settings%>"></asp:Label>
                            </asp:LinkButton>
                        </div>
                        
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
