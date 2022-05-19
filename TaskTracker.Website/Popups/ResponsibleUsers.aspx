<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ResponsibleUsers.aspx.vb"
    Inherits="PopupsResponsibleUsers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="_scriptManager" AsyncPostBackTimeout="10000"
            EnablePageMethods="true" LoadScriptsBeforeUI="true" ScriptMode="release" runat="server"
            EnablePartialRendering="true" >
        </asp:ScriptManager>
        <Asp:UpdatePanel ID="_udpUser" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div>
                    <asp:TextBox ID="_txtResponsibleUser" runat="server" ReadOnly="true"></asp:TextBox>
                    <asp:TextBox ID="_txtResponsibleUserName" runat="server" ReadOnly="true"></asp:TextBox>
                     <asp:TextBox ID="_txtTargetID" runat="server" ReadOnly="true"></asp:TextBox>
                     <br /><br />
                    <asp:TextBox ID="TextBox1" runat="server" ReadOnly="true" onclick="ShowResponsibleUser(this);"></asp:TextBox> <br /><br />
                    <asp:TextBox ID="TextBox2" runat="server" ReadOnly="true" onclick="ShowResponsibleUser(this);"></asp:TextBox> <br /><br />
                    <asp:TextBox ID="TextBox3" runat="server" ReadOnly="true" onclick="ShowResponsibleUser(this);"></asp:TextBox> <br /><br />
                    <asp:TextBox ID="TextBox4" runat="server" ReadOnly="true" onclick="ShowResponsibleUser(this);"></asp:TextBox> <br /><br />
                    <asp:TextBox ID="TextBox5" runat="server" ReadOnly="true" onclick="ShowResponsibleUser(this);"></asp:TextBox> <br /><br />
                    <asp:TextBox ID="TextBox6" runat="server" ReadOnly="true" onclick="ShowResponsibleUser(this);"></asp:TextBox> <br /><br />
                    
                   
                    <asp:Panel ID="_pnlResponsible" runat="server" BackColor="Gray" BorderColor="black"
                        BorderWidth="1">
                        <div>
                            <asp:Label ID="_lblFacility" runat="server" Text="<%$IPResources:Global,Facility/Location%>" Font-Bold="true"
                                Width="150px"></asp:Label>&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Facility/Location%>"
                                    Font-Bold="true" Width="100px"></asp:Label><br />
                            <asp:DropDownList ID="_ddlResponsibleFacility" onchange="populateResponsibleUsers(this);"
                                runat="server" Width="150px">
                            </asp:DropDownList>&nbsp;
                            <asp:DropDownList ID="_ddlResponsibleUser" runat="server" onchange="SetResponsibleUser(this);">
                            </asp:DropDownList>
                        </div>
                    </asp:Panel>
                </div>
                <asp:Button runat="server" Text="submit" />
                <ajaxToolkit:PopupControlExtender ID="_pceResponsibleUser" BehaviorID="_bhResponsibleUser"
                    runat="server" TargetControlID="_txtResponsibleUser" PopupControlID="_pnlResponsible"
                    Position="Bottom">
                </ajaxToolkit:PopupControlExtender>
            </ContentTemplate>
        </Asp:UpdatePanel>
    </form>
</body>
</html>
