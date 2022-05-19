<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerPopup.master" AutoEventWireup="false"
    CodeFile="ProcessEmployees.aspx.vb" Inherits="ProcessEmployees" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerPopup.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <div>
        <asp:UpdatePanel ID="_udpSubTask" runat="server" UpdateMode="always">
            <ContentTemplate>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="panel-title">
                            <asp:Label ID="_lblHeaderTitle" runat="server" Text="<%$IPResources:Global,Selected Employee%>"></asp:Label>
                            <asp:Label ID="_lblFirstname" runat="server"></asp:Label>
                            <asp:Label ID="_lblLastname" runat="server"> </asp:Label>
                        </div>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-sm-6">
                                <asp:Label ID="_lblInactiveStatus" Visible="true" Font-Bold="True" Height="40px"
                                    runat="server"></asp:Label></p>
                            </div>
                            <div class="col-sm-6 col-md-4 col-lg-3">
                                <IP:EmployeeList ID="_employeeList" Employeelabel="<%$IPResources:Global,Responsible%>" runat="server" EnableValidation="false">
                                </IP:EmployeeList>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer">
                        <div class="row">
                            <div class="col-sm-4 col-md-3 col-sm-offset-2 col-md-offset-4">
                                <asp:LinkButton ID="_btnCancel" runat="server" SkinID="Button">
                                    <span class="glyphicon glyphicon-ban-circle"></span>&nbsp;<asp:Label ID="Label11" runat="server" Text="<%$IPResources:Global,Cancel%>"></asp:Label>
                                </asp:LinkButton>

                            </div>
                            <div class="col-sm-4  col-md-3">
                                <asp:LinkButton ID="_btnInactivateUser" runat="server" SkinID="Button">
                                    <span class="glyphicon glyphicon-floppy-save"></span>&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Save Inactive User%>"></asp:Label>
                                </asp:LinkButton>

                            </div>
                            <IP:MessageBox ID="_confirmationSaveInactiveUser" runat="server" ButtonType="YesNo"
                                AllowPostback="true" Message="<%$IPResources:Global,Inactivating a user will perform clean up for MTT roles, RI Notifications, RI Analysis Leader list, MOC approval lists. Do you want to continue?%>"
                                OKScript="return true" Title="<%$IPResources:Global,Save Inactive User%>" />
                        </div>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
