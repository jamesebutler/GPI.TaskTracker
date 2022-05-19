<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FileAttachments.ascx.vb"
    Inherits="UserControlsFileAttachments" %>
<%--<script type="text/javascript">
    function refreshAttachments() {
        SetActiveTab(2);
        __doPostBack('_btnRefreshList', null);

    }
    function SetActiveTab(tabNumber) {
        var container = $find('_tabAttachmentContainer');
        if (container != null) {
            container.set_activeTabIndex(tabNumber);
        }
    }
</script>--%>
<asp:Button ID="_btnRefreshList" ClientIDMode="Static" runat="server" Style="display: none;" />
<asp:Panel ID="_tabAttachmentContainer" runat="server" ScrollBars="none">
    <div id="_tabAttachmentTabs" runat="server">
        <!-- Nav tabs -->
        <ul id="myTabs" class="nav nav-tabs ui-tabs" role="tablist">
            <li role="presentation" class="active"><a href="#files" aria-controls="files" role="tab" data-toggle="tab">
                <asp:Localize runat="server" Text="<%$IPResources:Global,File Attachments%>"></asp:Localize></a></li>
            <li role="presentation"><a href="#links" aria-controls="links" role="tab" data-toggle="tab">
                <asp:Localize runat="server" Text="<%$IPResources:Global,Link Attachments%>"></asp:Localize></a></li>
        </ul>
    </div>
    <!-- Tab panes -->
    <div class="tab-content table-responsive">
        <div role="tabpanel" class="tab-pane active" id="files">
            <div id="_divFileAttachments" runat="server" class="panel panel-default">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="_lblFileAttachmentInstructions" Text="<%$IPResources:Shared, File Attachments%>"
                            runat="server" Font-Bold="true"></asp:Label>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12">
                        <iframe frameborder="none" runat="server" class="embed-responsive-item" id="_dragDrop" src="~/Popups/DragDropAttachments.aspx" style="width: 100%; overflow: no-display;"></iframe>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-xs-12 col-sm-offset-4 col-sm-4">
                            <div class="form-group">
                                <asp:LinkButton ID="_btnRefreshFiles" runat="server" SkinID="Button"
                                    Visible="true">
                                    <asp:PlaceHolder runat="server">
                                        <span class="glyphicon glyphicon-paperclip"></span>&nbsp;<asp:Label ID="Label8" runat="server" Text="<%$IPResources:Global,Attach Files%>"></asp:Label>
                                    </asp:PlaceHolder>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div role="tabpanel" class="tab-pane" id="links">
            <div id="_divLinkAttachments" runat="server" class="panel panel-default">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="Label1" Text="<%$IPResources:Shared, Link Attachments%>"
                            runat="server" Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 text-center">
                        <asp:Label ID="_lblLinkInstructions" Text="<%$IPResources:Shared, To attach a link to this record. Enter a link, enter a description and click Attach Link.%>"
                            runat="server" Style="text-align: center"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-md-6">
                        <asp:Label ID="_lblLinkToAttach" runat="server" Text="<%$IPResources:Shared, URL%>" /><br />
                        <IP:AdvancedTextBox ExpandHeight="true" ID="_txtLinkToAttach" runat="server" cssclass="form-control"
                            TextMode="MultiLine" Rows="2" MaxLength="500" Wrap="true" placeholder="Http://">
                        </IP:AdvancedTextBox>
                    </div>
                    <div class="col-xs-12 col-md-6">
                        <asp:Label ID="_lblLinkDescription" runat="server" Text="<%$IPResources:Shared, Description%>"></asp:Label><br />
                        <IP:AdvancedTextBox ExpandHeight="true" ID="_txtLinkDescription" runat="server" cssclass="form-control"
                            TextMode="MultiLine" Rows="2" MaxLength="500" Wrap="true" placeholder="<%$IPResources:Shared, Description%>">
                        </IP:AdvancedTextBox>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-xs-12 col-sm-offset-2 col-sm-4">
                            <div class="form-group">
                                <asp:LinkButton ID="_btnCancelLink" runat="server" SkinID="Button" OnClientClick="$(window.location).attr('href', window.location); return false;">
                                    <span class="glyphicon glyphicon-ban-circle"></span>&nbsp;<asp:Label ID="Label11" runat="server" Text="<%$IPResources:Global,Cancel%>"></asp:Label>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <div class="form-group">
                                <asp:LinkButton ID="_btnAttachLink" runat="server" SkinID="Button">
                                    <span class="glyphicon glyphicon-paperclip"></span>&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Attach Link%>"></asp:Label>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <asp:Label ID="_lblLinkStatus" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>

<asp:Panel ID="_pnlAttachmentList" runat="server" ScrollBars="None">
    <div class="row">
        <div class="col-xs-12">
            <asp:Panel ID="_pnlAttachedFiles" runat="server" CssClass="panel panel-default" Width="100%" Visible="false" ScrollBars="Vertical" Height="300px">
                <div class="panel-heading">
                    <div class="panel-title">
                        <asp:Label ID="_lblHeaderTitle" runat="server" Text="<%$IPResources:Shared, Attached Files and Links%>"></asp:Label>
                    </div>
                </div>
                <div class="panel-footer">
                    <div class="row">
                        <div class="col-xs-12 col-sm-offset-4 col-sm-4">
                            <div>
                                <asp:LinkButton ID="_lnkSaveAttachments" runat="server" SkinID="Button" CommandArgument="" ToolTip="<%$IPResources:Shared, Save%>"><span class="glyphicon glyphicon-floppy-save"></span>&nbsp;<asp:Localize runat="server" Text="<%$IPResources:Shared, Save%>"></asp:Localize></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:GridView ID="_grvAttachedFiles" runat="server" Width="100%" AutoGenerateColumns="False"
                    DataKeyNames="TaskDocumentSeqID,taskheaderNumber" ShowHeader="false" CssClass="Table">
                    <RowStyle CssClass="grid_content" />
                    <HeaderStyle Wrap="false" Height="30px" />
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <div class="row">
                                    <div class="hidden-xs hidden-sm col-xs-12 col-md-2 text-left">
                                        <asp:Label Text="<%$IPResources:Shared, Attachments/Links%>" runat="server"></asp:Label>:
                                    </div>
                                    <div class="col-xs-12 col-md-10 text-left">
                                        <a target="_blank" href="<%#GetFileLocation(DirectCast(Container.DataItem, MttDocuments).FileName, DirectCast(Container.DataItem, MttDocuments).Location) %>">
                                            <%#GetAttachmentDisplay(DirectCast(Container.DataItem, MttDocuments).FileName, DirectCast(Container.DataItem, MttDocuments).Location)%>
                                        </a>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="hidden-xs hidden-sm col-xs-12 col-md-2 text-left">
                                        <asp:Label ID="_lblDescription" runat="server" Text="<%$IPResources:Shared, Description%>"></asp:Label>:
                                    </div>
                                    <div class="hidden-xs col-xs-12 col-md-10 text-left">
                                        <IP:AdvancedTextBox ID="_txtDescription" runat="server" Width="90%" Text='<%# Bind("Description") %>'
                                            Rows="2" TextMode="MultiLine" Wrap="true" MaxLength="3500">
                                        </IP:AdvancedTextBox>
                                    </div>
                                </div>
                                <asp:HiddenField ID="_TaskDocumentSeqID" runat="server" Value='<%#DirectCast(Container.DataItem, MttDocuments).TaskDocumentSeqID %>' />
                                <asp:HiddenField ID="_TaskHeaderSeqID" runat="server" Value='<%#DirectCast(Container.DataItem, MttDocuments).TaskHeaderNumber %>' />
                                <asp:HiddenField ID="_Location" runat="server" Value='<%#DirectCast(Container.DataItem, MttDocuments).Location %>' />
                                <asp:HiddenField ID="_FileName" runat="server" Value='<%#DirectCast(Container.DataItem, MttDocuments).FileName %>' />
                                <asp:HiddenField ID="_Description" runat="server" Value='<%#DirectCast(Container.DataItem, MttDocuments).Description %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-VerticalAlign="Top">
                            <ItemTemplate>
                                <div class="form-group">
                                    <asp:LinkButton ID="_lnkDeleteTask" SkinID="Button" runat="server" CommandName="Delete" CommandArgument="<%#Container.DataItemIndex%>" ToolTip="<%$IPResources:Shared, Delete%>"><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                                    <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="_lnkDeleteTask"
                                        ConfirmText="<%$IPResources:Global,Are you sure that you would like to delete this attachment?%>"></ajaxToolkit:ConfirmButtonExtender>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="_pnlAttachments" runat="server">
    <asp:Table runat="server" ID="Table2" Width="100%" SkinID="none">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel ID="Panel1" runat="server" CssClass="panel panel-default">
                    <div class="panel-heading">
                        <div class="panel-title">
                            <asp:Label ID="Label3" runat="server" Text="<%$IPResources:Shared, Attached Files and Links%>"></asp:Label>
                        </div>
                    </div>
                    <div class="panel-body">
                        <asp:GridView ID="_grdAttachmentList" runat="server" Width="100%" AutoGenerateColumns="False"
                            DataKeyNames="TaskDocumentSeqID,taskheaderNumber" ShowHeader="false">
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <table style="width: 100%">
                                            <tr>
                                                <td valign="top" style="width: 120px;">
                                                    <asp:Label Text="<%$IPResources:Shared, Attachments/Links%>" runat="server"></asp:Label>:
                                    
                                                </td>
                                                <td valign="top">
                                                    <div class="break-word" style="width: 90%; overflow: hidden; min-width: 400px; max-width: 800px;">
                                                        <a target="_blank" href="<%#GetFileLocation(DirectCast(Container.DataItem, MttDocuments).FileName, DirectCast(Container.DataItem, MttDocuments).Location) %>">
                                                            <%#GetAttachmentDisplay(DirectCast(Container.DataItem, MttDocuments).FileName, DirectCast(Container.DataItem, MttDocuments).Location)%>
                                                        </a>
                                                    </div>
                                                    <asp:HiddenField ID="_TaskDocumentSeqID" runat="server" Value='<%#DirectCast(Container.DataItem, MttDocuments).TaskDocumentSeqID %>' />
                                                    <asp:HiddenField ID="_TaskHeaderSeqID" runat="server" Value='<%#DirectCast(Container.DataItem, MttDocuments).TaskHeaderNumber %>' />
                                                    <asp:HiddenField ID="_Location" runat="server" Value='<%#DirectCast(Container.DataItem, MttDocuments).Location %>' />
                                                    <asp:HiddenField ID="_FileName" runat="server" Value='<%#DirectCast(Container.DataItem, MttDocuments).FileName %>' />
                                                    <asp:HiddenField ID="_rowIndex" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:Localize runat="server" Text="<%$IPResources:Shared, Description%>"></asp:Localize>:
                                                </td>
                                                <td valign="top">
                                                    <div class="break-word" style="width: 90%; overflow: hidden; min-width: 400px; max-width: 800px;">
                                                        <asp:Literal ID="_txtDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Literal>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
