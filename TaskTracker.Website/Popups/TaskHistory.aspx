<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TaskHistory.aspx.vb" Inherits="Popups_TaskHistory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:GridView ID="_grvTaskHistory" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="tablesorter">
                <AlternatingRowStyle ForeColor="black" Font-Bold="false" />
                <RowStyle ForeColor="black" Font-Bold="false" />
                <HeaderStyle Height="30px" ForeColor="white" Font-Underline="true" />
        <Columns>
            <asp:BoundField HeaderText="<%$IPResources:Global,Task Number%>" DataField="TaskNumber" />
            <asp:BoundField HeaderText="<%$IPResources:Global,Full Name%>" DataField="FullName" />
            <asp:BoundField HeaderText="<%$IPResources:Global,Date Updated%>" DataField="DateEntered" />
            <asp:BoundField HeaderText="<%$IPResources:Global,Description%>" DataField="Description" />
        </Columns> 
    </asp:GridView>
    </div>
    </form>
</body>
</html>
