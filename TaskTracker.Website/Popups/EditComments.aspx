<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditComments.aspx.vb" Inherits="Popups_EditComments" %>

<%@ Register Src="~/User Controls/Comments.ascx" TagPrefix="IP" TagName="Comments" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>Untitled Page</title>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="_scriptManager" AsyncPostBackTimeout="10000"
                EnablePageMethods="true" LoadScriptsBeforeUI="true" ScriptMode="release" runat="server"
                EnablePartialRendering="true">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="_udpComments" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <IP:Comments runat="server" ID="_Comments" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
