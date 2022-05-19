<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ModalIframPopup.ascx.vb"
    Inherits="ModalIframePopup" %>
<%--<div style="float:left;">--%>
<asp:LinkButton ID="_lnkDisplayModal" runat="server" SkinID="Button" data-toggle="modal">
    
</asp:LinkButton>
<%--<asp:Button ID="_btnDisplayModal" runat="server" Text="Display Modal" />--%>
<asp:TextBox ID="_txtReturnValue" runat="server" Style="display: none;"></asp:TextBox>
<%--</div>--%>
<asp:UpdatePanel ID="_udpModalPopup" RenderMode="Inline" runat="server" UpdateMode="always"
    EnableViewState="true">
    <ContentTemplate>
        <div class="modal fade" tabindex="-1" role="dialog" runat="server" id="_modalIframe">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">
                            <asp:Label ID="_lblHeaderText" runat="server" Text="<%$IPResources:Global, Title%>"></asp:Label></h4>
                    </div>
                    <div class="modal-body">
                        <iframe runat="server" frameborder="0" id="_ifrModalPopup" class="modalPopupNoBorder"
                            style="width: 98%; height: 600px; display: block;"></iframe>
                        <div id="preload_img_modal" runat="server" style="display: none;">
                            <h1>
                                <asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Loading%>"></asp:Label>...
                            </h1>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-xs-12 col-sm-6">
                                <div class="form-group">
                                    <asp:LinkButton ID="_btnCancel" runat="server" SkinID="Button"
                                        Visible="true" data-dismiss="modal">
                                        <asp:PlaceHolder runat="server">
                                            <span class="glyphicon glyphicon-ban-circle"></span>&nbsp;<asp:Label ID="Label9" runat="server" Text="<%$IPResources:Global,Cancel%>"></asp:Label>
                                        </asp:PlaceHolder>
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6">
                                <asp:LinkButton ID="_btnClose" runat="server" SkinID="Button"
                                    Visible="true" data-dismiss="modal">
                                    <asp:PlaceHolder runat="server">
                                        <span class="glyphicon glyphicon-remove"></span>&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Close%>"></asp:Label>
                                    </asp:PlaceHolder>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->

        <%--<asp:Panel ID="_pnlModalPopup" runat="server" Style="display: none" CssClass="modalPopupNoBorder">
            <table border="0" class="modalPopup" style="height: 100%; width: 100%">
                
                <tr>
                    <td style="height: 95%; vertical-align: top;" class="Border" colspan="2">
                        
                    </td>
                </tr>

            </table>
            
        </asp:Panel>--%>
    </ContentTemplate>
</asp:UpdatePanel>
<%--<ajaxToolkit:ModalPopupExtender ID="_mpeDisplayModal" runat="server" TargetControlID="_lnkDisplayModal"
    BehaviorID="_bhidDisplayModal" PopupControlID="_pnlModalPopup" BackgroundCssClass="modalBackground"
    DropShadow="false" CancelControlID="_btnCancel" Y="50" OkControlID="_imgClose"
    RepositionMode="RepositionOnWindowScroll" Drag="true" PopupDragHandleControlID="PopupDragHandle">
</ajaxToolkit:ModalPopupExtender>--%>
