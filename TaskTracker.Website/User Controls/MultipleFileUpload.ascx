<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MultipleFileUpload.ascx.vb"
    Inherits="UserControlsMultipleFileUpload" %>
<asp:Panel ID="_pnlMultipleFileUpload" runat="server" Width="800px">
    <%--<div style="position: absolute; top: 0px; left: 740px; z-index: 1;">
        
        <input type="button" id="_btnBrowse" name="_btnBrowse" class="Button" value="Browse"
            onclick="MultiFileUpload.TriggerFileUpload()" />
    </div>--%>
    <table>
        <tr>
            <td id="tdFileInputs">
                <ajaxToolkit:AjaxFileUpload runat="server" ID="_fileUpload"  />
            </td>
        </tr>
        <tr>
            <td>
                Title:<input id="_txtFileTitle" type="text" /></td>
        </tr>
        <tr>
            <td>
                <select id="lstFileList" name="lstFileList" size="10" style="width: 100%;"></select> 
            </td>
        </tr>
        <tr>
            <td align="Right">
                <%-- <asp:Button ID="_btnAdd" CssClass="Button" runat="server" Text="Add Selected File"
                    OnClientClick="MultiFileUpload.AddUploadFile();return false;" />--%>
                <asp:Button ID="_btnRemove" runat="server" CssClass="Button" Text="Remove Selected File"
                    OnClientClick="MultiFileUpload.RemoveUploadFile();return false;" />
                <asp:Button ID="_btnUpload" runat="server" CssClass="Button" Text="Upload File(s)" />
            </td>
        </tr>
    </table>
</asp:Panel>
