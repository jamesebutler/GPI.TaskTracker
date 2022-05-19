<%@ Control Language="VB" AutoEventWireup="false" CodeFile="JQEventCalendar.ascx.vb" Inherits="User_Controls_JQEventCalendar" %>
<div id='taskCalendar'></div>
<div id="fullCalModal" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span> <span class="sr-only">close</span></button>
                <h4 id="modalTitle" class="modal-title"></h4>
            </div>
            <div id="modalBody" class="modal-body"></div>
            <div class="modal-footer">
                <div class="row">
                    <div class="col-sm-12 col-md-offset-2 col-md-4 col-lg-offset-2">
                        <div class="form-group">
                            <asp:LinkButton ID="_btnClose" ClientIDMode="Static" runat="server" SkinID="Button" data-dismiss="modal">
                    <span aria-hidden="true">×</span>&nbsp;<asp:Localize runat="server" Text="<%$IPResources:Global,Close%>"></asp:Localize>
                            </asp:LinkButton>
                        </div>
                    </div>
                
                    <div class="col-sm-12 col-md-4">
                        <div class="form-group">

                            <asp:LinkButton ID="eventUrl" ClientIDMode="Static" runat="server" SkinID="Button">
                    <span aria-hidden="true" class="glyphicon glyphicon-pencil"></span>&nbsp;<asp:Localize runat="server" Text="<%$IPResources:Global,Task Detail%>"></asp:Localize>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
