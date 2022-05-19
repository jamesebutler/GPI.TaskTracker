<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EmployeeList.ascx.vb"
    Inherits="UserControlsEmployeeList" %>
<Asp:UpdatePanel ID="_udpUser" runat="server" UpdateMode="Always" RenderMode="Block">
    <ContentTemplate>
        <div>
            <asp:Panel ID="_pnlResponsible" runat="server" CssClass="BorderSecondary" BorderColor="black"
                BorderWidth="2" style="display:none">
                <div style="display: none">
                    <asp:TextBox ID="_txtResponsibleUser" runat="server" ReadOnly="true"></asp:TextBox>
                    <asp:TextBox ID="_txtResponsibleUserName" runat="server" ReadOnly="true"></asp:TextBox>
                    <asp:TextBox ID="_txtTargetID" runat="server" ReadOnly="true"></asp:TextBox></div>
                <div class="Border">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="_lblFacility" runat="server" Text="Facility/Location" Font-Bold="true"
                                    Width="150px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <br />
                                <asp:DropDownList ID="_ddlResponsibleFacility" runat="server" Width="250px">
                                </asp:DropDownList>
                               <%-- <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" IsSorted="false"
                                    PromptPosition="Top" QueryPattern="Contains" TargetControlID="_ddlResponsibleFacility">
                                </ajaxToolkit:ListSearchExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Employee/Role Name" Font-Bold="true"></asp:Label>
                                &nbsp;<asp:LinkButton ID="_btnClearUser" runat="server" Text="Clear User"></asp:LinkButton></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                &nbsp;<br />
                                <%--<asp:DropDownList ID="_ddlResponsibleUser" runat="server" onchange="ResponsibleUser.SetResponsibleUser(this);"
                                    Style="min-width: 100px">
                                </asp:DropDownList>--%>
                                <asp:ListBox ID="_ddlResponsibleUser" Rows="10" Width="250px" runat="server" onchange="ResponsibleUser.SetResponsibleUser(this);">
                                </asp:ListBox>                                
                                <ajaxToolkit:ListSearchExtender ID="_lseUser" runat="server" IsSorted="false" PromptPosition="Top"
                                    QueryPattern="Contains" TargetControlID="_ddlResponsibleUser">
                                </ajaxToolkit:ListSearchExtender>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </div>
        <ajaxToolkit:PopupControlExtender ID="_pceResponsibleUser" BehaviorID="_bhResponsibleUser"
            runat="server" TargetControlID="_txtResponsibleUser" PopupControlID="_pnlResponsible"
            Position="Bottom">
        </ajaxToolkit:PopupControlExtender>
    </ContentTemplate>
</Asp:UpdatePanel>
