<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Comments.ascx.vb" Inherits="User_Controls_Comments" %>
<asp:Panel ID="_pnlComments" runat="server" Visible="true" Width="100%" ScrollBars="Auto" Height="400px">
    <div>
        <asp:GridView ID="_gvComments" runat="server" AutoGenerateColumns="false" Width="98%" DataKeyNames="CommentsSeqId">
            <Columns>
                <asp:TemplateField HeaderText="<%$IPResources:Global,Edit%>">
                    <ItemTemplate>
                        <edititemtemplate>
                                <asp:LinkButton ID="_btnEditComment" Enabled="false" runat="server" CommandName="Edit" CssClass="IconButtons"><i class="glyphicon glyphicon-pencil btn-lg"></i></asp:LinkButton>
                            </edititemtemplate>
                        <asp:LinkButton ID="_btnUpdateComment" Enabled="false" runat="server" CommandName="Update" CssClass="IconButtons"><i class="glyphicon glyphicon-floppy-save btn-lg"></i></asp:LinkButton>
                        <asp:LinkButton ID="_btnCancelUpdate" Enabled="false" runat="server" CommandName="Cancel" CssClass="IconButtons"><i class="glyphicon glyphicon-ban-circle btn-lg"></i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="<%$IPResources:Global,Comments%>" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <div class="break-word" style="width: 98%; max-width: 800px; min-width: 300px;">
                            <asp:Literal ID="_comments" runat="server" Text='<%#Bind("Comments") %>'></asp:Literal>
                        </div>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <IP:AdvancedTextBox ExpandHeight="true" ID="_comments" runat="server" Width="100%" TextMode="multiLine" Rows="4" Text='<%#Bind("Comments") %>'></IP:AdvancedTextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$IPResources:Global,Last Updated%>" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <asp:Literal runat="server" Text='<%#IP.Bids.SharedFunctions.FormatDate(cdate(DirectCast(Container.DataItem, IP.MEAS.BO.TaskItemComments).LastUpdateDate)) %>'></asp:Literal>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Literal runat="server" Text='<%#IP.Bids.SharedFunctions.FormatDate(Now) %>'></asp:Literal>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$IPResources:Global,Last Updated By%>" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <asp:Literal runat="server" Text='<%#Eval("LastUpdateUsername") %>'></asp:Literal>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Literal runat="server" Text=''></asp:Literal>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="<%$IPResources:Global,Delete%>">
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnDeleteComment" Enabled="false" runat="server" CommandName="Delete"  CssClass="IconButtons"><i class="glyphicon glyphicon-trash btn-lg"></i></asp:LinkButton>
                        <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="_btnDeleteComment"
                            ConfirmText="<%$IPResources:Global,Are you sure that you would like to delete this comment?%>"></ajaxToolkit:ConfirmButtonExtender>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Panel>
