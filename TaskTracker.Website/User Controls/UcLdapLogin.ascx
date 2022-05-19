<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UcLdapLogin.ascx.vb"
    Inherits="IP.Bids.UserControl.LdapLogin" %>
<%@ Register Src="~/User Controls/AdvancedEmployeeListDropdown.ascx" TagName="EmployeeList"
    TagPrefix="IP" %>

<div class="row pull-right">        
    <div class="col-xs-12">  
        <%--database,webserver,version and currernt user info below--%>
        <asp:Label ID="_lblWelcome" Font-Bold="true" runat="server"></asp:Label>  
         <%-- switch user link below--%>
        <asp:HyperLink ID="_loginStatus" NavigateUrl="#" Font-Underline="true"
            runat="server" data-toggle="modal" data-target="#loginDialog" />&nbsp;   
   
        <asp:LinkButton ID="_logOut" OnClick="LogOut" Font-Underline="true" ForeColor="black"
        Text="<%$IPResources:Global,Logout %>" runat="server" Visible="false" />   
        
        <asp:LinkButton ID="_runAs" Font-Underline="true" ForeColor="blue" Text="<%$IPResources:Global,RunAs %>"
            runat="server" />           
    </div>
</div>

<!-- Modal -->
<div class="modal fade text-left" id="loginDialog" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
    <div class="modal-dialog modal-sm" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel"><span class="glyphicon glyphicon-user"></span>&nbsp;<asp:Label ID="_lblLogin" runat="server" Text="<%$IPResources:Global,Login %>"></asp:Label></h4>
            </div>
            <div class="modal-body">
                <asp:Panel ID="_pnlLoginImpersonate" runat="server" CssClass="panel panel-default">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="form-group">
                                <span class="required">*</span><asp:Label ID="UserNameLabel" runat="server" Text="<%$IPResources:Global,Username %>"></asp:Label><br />
                                <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                    ErrorMessage="<%$IPResources:Global,UserNameRequired %>" ToolTip="<%$IPResources:Global,UserNameRequired %>"
                                    ValidationGroup="login" Display="Dynamic" Text=""></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="form-group">
                                <span class="required">*</span><asp:Label ID="PasswordLabel" runat="server"
                                    Text="<%$IPResources:Global,Password %>"></asp:Label><br />
                                <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                    ErrorMessage="<%$IPResources:Global,PasswordRequired %>" ToolTip="<%$IPResources:Global,PasswordRequired %>"
                                    ValidationGroup="login" Display="Dynamic" Text=""></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="form-group">
                                <span class="required">*</span><asp:Label ID="DomainLabel" runat="server"
                                    Font-Bold="true" Text="<%$IPResources:Global,Domain %>"></asp:Label><br />
                                <asp:DropDownList ID="_ddlDomain" runat="server" Width="200px">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="col-xs-12">
                            <div class="form-group">
                                <asp:LinkButton ID="LoginButton" runat="server" SkinID="Button" CommandName="Login" CommandArgument="Login" ValidationGroup="login" OnClientClick="return CheckForm('login');">
                                    <asp:PlaceHolder runat="server">
                                        <span class="glyphicon glyphicon-user"></span>&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$IPResources:ButtonText,Login %>"></asp:Label>
                                    </asp:PlaceHolder>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="form-group">
                                <asp:LinkButton ID="_btnCancelLogin" runat="server" SkinID="Button" data-dismiss="modal">
                                    <asp:PlaceHolder runat="server">
                                        <span class="glyphicon glyphicon-ban-circle"></span>&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$IPResources:ButtonText,Cancel %>"></asp:Label>
                                    </asp:PlaceHolder>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>



                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Panel ID="_pnlImpersonate" runat="server" Width="100%" Visible="true">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <hr />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <IP:EmployeeList runat="server" ID="_ImpersonateList" Employeelabel="<%$IPResources:Global,User to Impersonate %>"
                                            UserMode="UsersOnly">
                                        </IP:EmployeeList>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <div class="col-xs-12">
                                        <div class="form-group">
                                            <asp:LinkButton ID="_btnImpersonate" runat="server" SkinID="Button" CommandName="Login" CommandArgument="Impersonate">
                                                <asp:PlaceHolder runat="server">
                                                    <span class="glyphicon glyphicon-sunglasses"></span>&nbsp;<asp:Label ID="Label3" runat="server" Text="<%$IPResources:Global,Impersonate %>"></asp:Label>
                                                </asp:PlaceHolder>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="col-xs-12">
                                        <div class="form-group">
                                            <asp:LinkButton ID="LinkButton1" runat="server" SkinID="Button" data-dismiss="modal">
                                                <asp:PlaceHolder runat="server">
                                                    <span class="glyphicon glyphicon-ban-circle"></span>&nbsp;<asp:Label ID="Label4" runat="server" Text="<%$IPResources:ButtonText,Cancel %>"></asp:Label>
                                                </asp:PlaceHolder>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-xs-12">
                            <div class="form-group">
                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="form-group">
                                <asp:Label ID="_lblMessageToUser" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>

        </div>
    </div>
</div>

<div style="visibility: hidden; display: none;">
    <input type="button" id="_btnHideLogin" onclick="return false" runat="server" />
</div>
