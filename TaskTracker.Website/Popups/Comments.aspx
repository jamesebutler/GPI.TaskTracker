<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerPopup.master" AutoEventWireup="false"
    CodeFile="Comments.aspx.vb" Inherits="PopupsComments" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerPopup.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <div class="row">
        <div class="col-xs-12">
             <asp:Label ID="_lblComments" runat="server" Text="<%$IPResources:Global,Comments%>"></asp:Label><br />
                    <IP:AdvancedTextBox ExpandHeight="true" ID="_txtComments" runat="server" Width="100%" TextMode="multiLine" Rows="4"></IP:AdvancedTextBox>
        </div>
        </div>
    <div class="row">
        <div class="col-xs-12 col-md-4 col-md-offset-4">
            <asp:Button ID="_btnAddComments" Text="<%$IPResources:Global,Add Comments%>" runat="server" />
        </div>
        <div class="col-xs-12 col-md-4">
            <asp:Button
                        ID="_btnCancel" runat="server" Text="Cancel" />
        </div>
      
    </div>
</asp:Content>
