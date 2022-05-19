<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucCultureSelection.ascx.vb"
    Inherits="ucCultureSelection" %>
<asp:Label ID="_lblCurrentLanguage" runat="server" Text="Select a Language:"></asp:Label>&nbsp;
<asp:LinkButton ID="_lnkChangeLanguage" runat="server" Text="<%$IPResources:Global,Chooselanguage %>" data-toggle="modal" data-target="#ChooseLanguageModal"
    OnClientClick="Javascript:return false" Font-Underline="true"></asp:LinkButton>
<%--<ajaxToolkit:ModalPopupExtender ID="_mpeLanguageSelection" runat="server" TargetControlID="_lnkChangeLanguage"
    PopupControlID="_pnlLanguageSelection" BackgroundCssClass="modalBackground" DropShadow="true"
    OkControlID="_btnCancel" CancelControlID="_btnCancel">
</ajaxToolkit:ModalPopupExtender>--%>

<!-- Modal -->
<div class="modal fade" id="ChooseLanguageModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="myModalLabel"><span class="glyphicon glyphicon-globe"></span>&nbsp;<asp:Label ID="_lblChooseLanguage" runat="server" Text="<%$IPResources:ButtonText,Chooselanguage %>"></asp:Label></h4>
            </div>
            <div class="modal-body">
                <asp:RadioButtonList ID="_rblLanguages" AutoPostBack="true" runat="server" RepeatColumns="1">
                </asp:RadioButtonList>
            </div>
            <div class="modal-footer">
                <div class="row">
                    <div class="col-xs-12 col-md-6">
                        <asp:LinkButton ID="_btnOK" runat="server" SkinID="Button">
                            <asp:PlaceHolder runat="server">
                                <span class="glyphicon glyphicon-ok-circle"></span>&nbsp;<asp:Label ID="_lblOK" runat="server" Text="<%$IPResources:ButtonText,OK %>"></asp:Label>
                            </asp:PlaceHolder>
                        </asp:LinkButton>
                    </div>
                    <div class="col-xs-12 col-md-6">
                        <asp:LinkButton ID="_btnCancel" runat="server" SkinID="Button" data-dismiss="modal">
                            <asp:PlaceHolder runat="server">
                                <span class="glyphicon glyphicon-ban-circle"></span>&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$IPResources:ButtonText,Cancel %>"></asp:Label>
                            </asp:PlaceHolder>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<%--<asp:Panel ID="_pnlLanguageSelection" runat="server" Width="400px" ScrollBars="none"
    Style="display: none;" CssClass="panel panel-default modalPopup">
    <div class="panel-heading">
        <div class="panel-title">
        </div>
    </div>
    <div class="panel-body">
    </div>

</asp:Panel>--%>
