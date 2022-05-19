<%@ Register Namespace="RealWorld.Grids" TagPrefix="RWG" %>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MultiTaskEdit.ascx.vb"
    Inherits="User_Controls_Tasks_MultiTaskEdit" %>
<%@ Import Namespace="System.Globalization" %>
<script type="text/javascript">
    function toggleDetailRow(checkedState) {

        if (checkedState === true || checkedState === "true" || checkedState === "checked") {
            $('.DetailRow').show();
            $('.DetailRow').attr('style', 'display:table-row!important');

        }
        else {
            $('.DetailRow').hide();
            $('.DetailRow').attr('style', 'display:none!important');
        }
    }
    function showNewCommentRow(id) {
        $('#' + id).show();
        $('#' + id).attr('style', 'display:table-row!important');
    }

    $(document).ready(function () {
        $(toggleDetailRow($("#_cbExpandRows").attr('checked')));
        //$(toggleDetailRow('#_cbExpandRows'.checked));
        $('.CommentRow').hide();
    });
</script>
<div>
    <div id="container-fluid">
        <section class="panel-footer">
            <div class="row">

                <div class="col-xs-12 col-sm-6 col-md-3 col-lg-2">
                    <div class="form-group">
                        <asp:LinkButton ID="_btnAddNewTask" runat="server" SkinID="Button">
                            <span class="glyphicon glyphicon-plus"></span>&nbsp;<asp:Label ID="_lblAddTaskItems" runat="server" Text="<%$IPResources:Global,Add Task Items%>"></asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3 col-lg-2">
                    <div class="form-group">
                        <asp:LinkButton ID="_btnSaveEdits" runat="server" SkinID="ButtonPrimary" ValidationGroup="TaskDetails" OnClientClick="DisplayBusy()">
                            <span class="glyphicon glyphicon-floppy-save"></span>&nbsp;<asp:Label ID="Label9" runat="server" Text="<%$IPResources:Global,Save Task%>"></asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3 col-lg-3">
                    <div class="form-group">
                        <asp:LinkButton ID="_btnReturnToTaskHeader" runat="server" SkinID="Button">
                            <span class="glyphicon glyphicon-step-backward"></span>&nbsp;<asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Return To Task Header%>"></asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-12 col-md-5">
                    <div class="btn-toolbar" role="toolbar" aria-label="...">
                        <div class="btn-group">
                            <asp:LinkButton ID="_btnPageFirst" runat="server" SkinID="ButtonToolbar">
                                <span class="glyphicon glyphicon-menu-left"></span><span class="glyphicon glyphicon-menu-left"></span>&nbsp;<asp:Label ID="Label10" runat="server" Text="<%$IPResources:Global,First%>"></asp:Label>
                            </asp:LinkButton>
                        </div>
                        <div class="btn-group">
                            <asp:LinkButton ID="_btnPagePrevious" runat="server" SkinID="ButtonToolbar">
                                <span class="glyphicon glyphicon-menu-left"></span>&nbsp;<asp:Label ID="Label11" runat="server" Text="<%$IPResources:Global,Previous%>"></asp:Label>
                            </asp:LinkButton>
                        </div>
                        <div class="btn-group">
                            <asp:LinkButton ID="_btnPageNext" runat="server" SkinID="ButtonToolbar">
                                <span class="glyphicon glyphicon-menu-right"></span>&nbsp;<asp:Label ID="Label12" runat="server" Text="<%$IPResources:Global,Next%>"></asp:Label>
                            </asp:LinkButton>
                        </div>
                        <div class="btn-group">
                            <asp:LinkButton ID="_btnPageLast" runat="server" SkinID="ButtonToolbar">
                                <span class="glyphicon glyphicon-menu-right"></span><span class="glyphicon glyphicon-menu-right"></span>&nbsp;<asp:Label ID="Label13" runat="server" Text="<%$IPResources:Global,Last%>"></asp:Label>
                            </asp:LinkButton>
                        </div>
                        <div class="btn-group">
                            <asp:Label ID="_lblPageHeader" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
               <%-- <div class="col-xs-12 col-sm-6 col-md-3 col-lg-1">
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3 col-lg-1">
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3 col-lg-1">
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3 col-lg-1">
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3 col-lg-1">
                </div>--%>
                
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-3 col-lg-3">
                    <div class="form-group">
                        <asp:Label ID="_lblInspectionTypes" Visible="false" runat="server" Text="<%$IPResources:Global,Inspection Types%>"></asp:Label><asp:DropDownList ID="_ddlInspectionTypeList" runat="server" AutoPostBack="true" Visible="false"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </section>
        <div class="rows">
            <section id="legend" class="col-xs-12 col-md-8 col-lg-4">
                <asp:Label runat="server" Text="<%$IPResources:Global, Legend%>" ID="_lblLegend"></asp:Label>:&nbsp;<asp:Localize runat="server" Text="<%$IPResources:Global, Parent Tasks%>"></asp:Localize>
                <span class='glyphicon glyphicon-list-alt' aria-hidden='true'></span>
                &nbsp;
                        <asp:Localize runat="server" Text="<%$IPResources:Global, Sub Tasks%>"></asp:Localize><span class='glyphicon glyphicon-chevron-left' aria-hidden='true'></span>
                &nbsp;
                        <asp:Localize runat="server" Text="<%$IPResources:Global, Subsequent Tasks%>"></asp:Localize><span class='glyphicon glyphicon-chevron-right' aria-hidden='true'></span>
            </section>
            <div class="col-xs-12 col-md-4 col-lg-2">
                <asp:Label ID="_taskListCount" runat="server" SkinID="none" Font-Size="12px"></asp:Label>
            </div>
            <div class="col-xs-12 col-lg-6">
                <div class="row">
                    <div class="col-xs-12 col-lg-6">
                        <asp:CheckBox ID="_cbShowClosedTasks" runat="server" AutoPostBack="true" Checked="false" CssClass="pull-left" Font-Bold="false" Text="<%$IPResources:Global,Show All Tasks%>" />
                    </div>
                    <div class="col-xs-12 col-lg-6">
                        <asp:CheckBox ID="_cbExpandRows" ClientIDMode="Static" runat="server" Text="<%$IPResources:Global, Click to show Description and Comments%>" Font-Bold="false" onclick="toggleDetailRow(this.checked);" />
                    </div>
                </div>
            </div>

        </div>

        <div id="grid" style="height: 100%; width: 100%;">
            <asp:ValidationSummary ID="_valEditTasks" runat="server" ValidationGroup="TaskDetails"
                EnableClientScript="true" ShowMessageBox="true" />
            <section id="no-more-gridView">
                <RWG:BulkEditGridView ID="_grdEditTaskList" runat="server" Caption=""
                    AllowSorting="true" CellPadding="4" EnableViewState="true" Width="100%" DataKeyNames="TaskItemSeqId">
                    <HeaderStyle Height="30px" BackColor="Black" ForeColor="white" Font-Underline="false" />
                    <SelectedRowStyle BackColor="Blue" />
                    <RowStyle BackColor="#cccccc" />
                    <Columns>
                        <asp:TemplateField HeaderText="Group"
                            HeaderStyle-Width="40px" SortExpression="group">
                            <HeaderTemplate>
                                <asp:LinkButton ID="LinkButtonEmpName" ForeColor="white" runat="server" CommandName="Sort" CommandArgument="group" ToolTip="<%$IPResources:Global, Click to sort by parent/subtask groupings%>">
                            <i class="glyphicon glyphicon-sort-by-attributes btn-lg"></i>
                                </asp:LinkButton>

                            </HeaderTemplate>
                            <EditItemTemplate>
                                <asp:Literal ID="_litTaskTypeIcon" runat="server"></asp:Literal>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="<%$IPResources:Global, Facility/Location%>" DataField="SiteName"
                            SortExpression="SiteName" ItemStyle-Width="90px" ReadOnly="true"/>
                        <asp:BoundField HeaderText="<%$IPResources:Global, Header Title%>" DataField="HeaderTitle"
                            SortExpression="SiteName" ItemStyle-Width="90px" ReadOnly="true" />
                        <asp:TemplateField HeaderText="<%$IPResources:Global, Title%>" SortExpression="ITEMTITLE"
                            HeaderStyle-Width="300px">
                            <EditItemTemplate>
                                <asp:TextBox ID="_txtTitle" runat="server" autofocus="true" CssClass="textExpand" TextMode="MultiLine" Rows="3" Wrap="true"
                                    Width="98%" MaxLength="80" onkeydown="maxTextboxLength(this,80,true);" onblur="maxTextboxLength(this,80,true);" onchange="maxTextboxLength(this,80,true);" onpaste="maxTextboxLength(this,80,true);"></asp:TextBox>
                                <asp:Literal ID="_litTitle" runat="server"></asp:Literal>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$IPResources:Global, Responsible%>" SortExpression="ResponsibleName" HeaderStyle-Width="200px">
                            <EditItemTemplate>
                                <IP:EmployeeList ID="_ResponsiblePerson" runat="server" UserMode="UsersAndRoles" ClientIDMode="Predictable"
                                    DisplayClearLink="false" OnUserChanged="_grdEditTaskList.HandleRowChanged" AutoPostBack="false"
                                    Employeelabel="" EnableValidation="true" style="width: 90%">
                                    <validation validationgroup="TaskDetails" validationerrormessage="<%$IPResources:Global,Responsible Person is required%>"
                                        validationerrortext="" validateemptytext="true" />
                                </IP:EmployeeList>
                                <asp:Literal ID="_litResponsiblePerson" runat="server"></asp:Literal>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$IPResources:Global, Due Date%>" SortExpression="DueDate"
                            HeaderStyle-Width="80px">
                            <EditItemTemplate>
                                <asp:Label ID="_sortableDate" runat="server" Text='<%#CDate(DirectCast(Container.DataItem, TaskListing).DueDate).ToString("yyyyMMdd")%>' Style="display: none"></asp:Label><br />
                                <IP:JQDatePicker ID="_EstimatedDueDate" runat="server" StartDate='<%#DirectCast(Container.DataItem, TaskListing).DueDATE%>'
                                    ShowFromDate="false" FromLabel="" ShowLabelsOnSameLine="true" AllowDateCritical="false"
                                    EnableValidation="false" OnTextChanged="_grdEditTaskList.HandleRowChanged" AllowPostBack="false">
                                    <validation validationerrormessage="<%$IPResources:Global,Due Date is required%>"
                                        validationerrortext="" validateemptytext="true" validationgroup="TaskDetails" />
                                </IP:JQDatePicker>
                                <asp:Literal ID="_litDueDate" runat="server"></asp:Literal>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$IPResources:Global, Closed Date%>" SortExpression="Closeddate"
                            HeaderStyle-Width="80px" Visible="false">
                            <EditItemTemplate>
                                <asp:Label ID="_sortableClosedDate" runat="server" Style="display: none"></asp:Label>
                                <IP:JQDatePicker ID="_ClosedDate" runat="server" ShowFromDate="false"
                                    FromLabel="" ShowLabelsOnSameLine="true" EnableValidation="false" AllowPostBack="false"
                                    OnTextChanged="_grdEditTaskList.HandleRowChanged">
                                </IP:JQDatePicker>
                                <asp:Literal ID="_litClosedDate" runat="server"></asp:Literal>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$IPResources:Global, Priority%>" SortExpression="Priority"
                            HeaderStyle-Width="50px">
                            <EditItemTemplate>
                                <asp:DropDownList ID="_ddlPriority" AutoPostBack="false" runat="server" Style="min-width: 90%" SkinID="nolivesearch">
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$IPResources:Global, Task Status%>" SortExpression="STATUSNAME"
                            HeaderStyle-Width="80px">
                            <EditItemTemplate>
                                <asp:DropDownList ID="_ddlTaskStatus" AutoPostBack="false" runat="server" Style="min-width: 90%" SkinID="nolivesearch">
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$IPResources:Global, Edit%>" HeaderStyle-Width="50px">
                            <EditItemTemplate>
                                <asp:LinkButton ID="_lnkEditTask" runat="server" CommandArgument="" CssClass="IconButtons"><i class="glyphicon glyphicon-pencil btn-lg"></i></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$IPResources:Global, Delete%>" HeaderStyle-Width="50px">
                            <EditItemTemplate>
                                <asp:LinkButton ID="_lnkDeleteTask" runat="server" CommandName="Delete" CommandArgument="<%#DirectCast(Container.DataItem, TaskListing).TaskItemSeqId %>" CssClass="IconButtons"><i class="glyphicon glyphicon-trash btn-lg"></i></asp:LinkButton>
                                <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="_lnkDeleteTask"
                                    ConfirmText="<%$IPResources:Global,Are you sure that you would like to delete this task?%>"></ajaxToolkit:ConfirmButtonExtender>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$IPResources:Global, Comments%>" HeaderStyle-Width="50px">
                            <EditItemTemplate>
                                <asp:LinkButton ID="_lnkComment" runat="server" CssClass="IconButtons" ToolTip="<%$IPResources:Global,Add Comments%>"><span class="glyphicon glyphicon-comment btn-lg"></span></asp:LinkButton>

                                </td></tr>
                                <tr class="CommentRow" id="comment<%#DirectCast(Container.DataItem, TaskListing).TaskItemSeqId %>" style="<%#"background-color:" & GetTaskBackColorAsHex(DirectCast(Container.DataItem, TaskListing).TaskItemLevel)%>; display: none!important">
                                    <td colspan="1">&nbsp;</td>
                                    <td colspan="100" style="text-align: left">
                                        <asp:Label runat="server" Text="<%$IPResources:Global, Comments%>"></asp:Label>:&nbsp;
                                         <asp:TextBox ID="_txtNewComment" autofocus="true" runat="server" CssClass="textExpand" TextMode="MultiLine" Rows="1" Wrap="true"
                                             Width="80%" MaxLength="4000" onkeydown="maxTextboxLength(this,4000,true);" onblur="maxTextboxLength(this,4000,true);" onchange="maxTextboxLength(this,4000,true);" onpaste="maxTextboxLength(this,4000,true);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="DetailRow" style="<%#"background-color:" & GetTaskBackColorAsHex(DirectCast(Container.DataItem, TaskListing).TaskItemLevel)%>; display: none!important">
                                    <td colspan="3">
                                        <table style="width: 100%">                                           
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td style="vertical-align: top">
                                                    <asp:Label runat="server" Text="<%$IPResources:Global, Description%>"></asp:Label><br />
                                                    <asp:TextBox ID="_txtDescription" autofocus="true" runat="server" CssClass="textExpand" TextMode="MultiLine" Rows="2" Wrap="true"
                                                        Width="98%" onkeydown="maxTextboxLength(this,4000,true);" onblur="maxTextboxLength(this,4000,true);" onchange="maxTextboxLength(this,4000,true);" onpaste="maxTextboxLength(this,4000,true);"></asp:TextBox>
                                                    <asp:Literal ID="_litDescription" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="100" style="vertical-align: top">
                                        <%--<asp:Label runat="server" Text="Comments"></asp:Label><br />--%>
                                        <asp:Panel ID="_pnlComments" runat="server" Visible="true" Width="100%">
                                            <div>
                                                <asp:GridView ID="_gvComments" runat="server" AutoGenerateColumns="false" Width="98%" ShowHeaderWhenEmpty="true">
                                                    <Columns>
                                                        <asp:BoundField DataField="Comments" HeaderText="<%$IPResources:Global,Comments%>"
                                                            ItemStyle-VerticalAlign="Top" ItemStyle-Width="60%" />
                                                        <asp:TemplateField HeaderText="<%$IPResources:Global,Last Updated%>" ItemStyle-VerticalAlign="Top" ItemStyle-Width="20%">
                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" Text='<%#IP.Bids.SharedFunctions.FormatDate(CDate(DirectCast(Container.DataItem, IP.MEAS.BO.TaskItemComments).LastUpdateDate)) %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="LastUpdateUsername" HeaderText="<%$IPResources:Global,Last Updated By%>"
                                                            ItemStyle-VerticalAlign="Top" ItemStyle-Width="20%" />

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        No Comments
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr style="background-color: #fff">
                                    <td colspan="100">
                                        <%--</td>
                                <td colspan="100">
                                    <asp:Label runat="server" Text="Comments"></asp:Label><br />
                                    <asp:TextBox ID="_txtComments" runat="server" CssClass="textExpand" TextMode="MultiLine" Rows="2" Wrap="true"
                                        Width="98%"></asp:TextBox>
                                    <asp:Literal ID="_litComments" runat="server"></asp:Literal>--%>
                            </EditItemTemplate>


                        </asp:TemplateField>
                    </Columns>
                </RWG:BulkEditGridView>
            </section>
        </div>
    </div>
</div>
<script type="text/javascript">
    /*
  $(function () {
        $(toggleDetailRow($("#_cbExpandRows").attr('checked')));
        //$(toggleDetailRow('#_cbExpandRows'.checked));
        $('.CommentRow').hide();
        console.log("ready!");
    });
 */

</script>
