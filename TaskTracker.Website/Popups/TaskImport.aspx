<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TaskImport.aspx.vb" Inherits="Popups_AuditImport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script type="text/javascript">
        var validFiles = ["xlsx"];//var validFiles=["xls","xlsx"];
        function OnUpload(source) {
            return true

        }


    </script>

    <style>
        .btn-file {
            position: relative;
            overflow: hidden;
        }

            .btn-file input[type=file] {
                position: absolute;
                top: 0;
                right: 0;
                min-width: 100%;
                min-height: 100%;
                font-size: 100px;
                text-align: right;
                filter: alpha(opacity=0);
                opacity: 0;
                outline: none;
                background: white;
                cursor: inherit;
                display: block;
            }

                .btn-file input[type=file][disabled] {
                    background-color: #eee;
                    background-color: #eee !important;
                    opacity: 1;
                }

        .btn-default input[type=file][disabled] {
            background-color: #eee;
            background-color: #eee !important;
            opacity: 1;
        }
    </style>
</head>
<%--<!-- STEP TWO: Copy this code into the BODY of your HTML document  -->
<body>
    <center>
        Please upload only images that end in:

        <script>
document.write(extArray.join("  "));
        </script>

        </head>--%>
<body>
    <form id="form1" runat="server">
         <div class="container-fluid">
            <asp:ScriptManager ID="_scriptManager" AsyncPostBackTimeout="10000"
                EnablePageMethods="true" LoadScriptsBeforeUI="true" ScriptMode="release" runat="server"
                EnablePartialRendering="true">
            </asp:ScriptManager>

            <asp:Panel ID="_pnlAttachments" runat="server">
                <asp:Panel ID="_pnlFileAttachments" GroupingText="" runat="server" ScrollBars="None">
                    <div id="_divFileAttachments" runat="server">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Label ID="_lblFileAttachmentInstructions" Text="<%$IPResources:Global,To import a file to this record. Select an Excel file (.xlsx) and click Upload File.%>"
                                    runat="server" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div class="panel-footer">
                            <div class="row">                                
                                <div class="col-xs-12 col-sm-4 col-md-3">
                                    <div class="form-group">
                                        <label id="_fileContainer" class="btn btn-default btn-file form-control">
                                            <asp:FileUpload ID="fileUpEx" ClientIDMode="Static" CssClass="form-control" runat="server" />
                                            <span class="glyphicon glyphicon-paperclip"></span>&nbsp;<asp:Label runat="server" Text="<%$IPResources:Global,Select a File%>"></asp:Label>
                                            &nbsp;
                                        </label>
                                    </div>
                                </div>                                
                                <div class="col-xs-12 col-sm-4 col-md-3">
                                    <div class="form-group">
                                        <asp:LinkButton ID="_btnUploadFile" runat="server" SkinID="Button"
                                            Style="display: none">
                                            <asp:PlaceHolder runat="server">
                                                <span class="glyphicon glyphicon-cloud-upload"></span>&nbsp;<asp:Label ID="Label9" runat="server" Text="<%$IPResources:Global,Import File%>"></asp:Label>
                                            </asp:PlaceHolder>
                                        </asp:LinkButton>
                                         <asp:LinkButton ID="_btnAddTasksToHeader" runat="server" SkinID="ButtonPrimary">
                                            <asp:PlaceHolder runat="server">
                                                <span class="glyphicon glyphicon-cloud-upload"></span>&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Add Tasks%>"></asp:Label>
                                            </asp:PlaceHolder>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="form-group">
                                        <asp:Label ID="_lblSelectedFile" runat="server" ClientIDMode="Static"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Label ID="_lblFileUploadStatus" runat="server"></asp:Label>
                            </div>
                        </div>

                        <%--    <asp:Table ID="_tblFileAttachments" runat="server" BorderWidth="0" CellSpacing="1"
                            CellPadding="0" Width="900px">
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
                                    <br />
                                    <br />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="150px">
                                    <asp:Label ID="_lblFileToUpload" runat="server" Text="<%$IPResources:Global,File To Import%>" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="_txtFileName" runat="server" ReadOnly="true"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell Width="200px" VerticalAlign="top">
                                   
                                    <%-- <asp:Button ID="_btnFileUpload" Style="position: absolute; top: 35px; left: 770px;
                                        z-index: 1; height: 30px;" runat="server" Text="<%$IPResources:Shared,Browse %>"
                                        Width="70px" OnClientClick="Javascript:return false;" />--%>
                        <%--  </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell HorizontalAlign="center">
                                   
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
                                    <br />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>-- %>--%>
                    </div>
                    <asp:Repeater ID="_rpTasks" runat="server">
                        <SeparatorTemplate>
                            &nbsp;
                        </SeparatorTemplate>
                        <ItemTemplate>
                            <asp:Table ID="Table1" Width="100%" runat="server" CellSpacing="0" BorderColor="black"
                                BorderWidth="1" CellPadding="2">
                                <asp:TableHeaderRow CssClass="Border">
                                    <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="50%" ColumnSpan="2">
                                        <asp:Label ID="_lblResponsiblePerson" runat="server" Text="<%$IPResources:Global,Responsible Person%>"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="25%">
                                        <asp:Label ID="_lblPriority" runat="server" Text="<%$IPResources:Global,Priority%>"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="25%">
                                        <asp:Label ID="_lblDueDate" runat="server" Text="<%$IPResources:Global,Due Date%>"></asp:Label>
                                    </asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                                <asp:TableRow CssClass="Border">
                                    <asp:TableCell VerticalAlign="bottom" Width="50%" ColumnSpan="2">
                                        <asp:Label ID="_lblTaskResponsiblePerson" runat="server" Font-Bold="false" EnableTheming="false"
                                            Text="<%#DirectCast(Container.DataItem, IP.MEAS.BO.TaskItem).RoleName%>"></asp:Label>
                                        <asp:HiddenField ID="Label1" runat="server" Value="<%#DirectCast(Container.DataItem, IP.MEAS.BO.TaskItem).ResponsibleRoleSeqId%>"></asp:HiddenField>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="bottom" Width="25%">
                                        <asp:Label ID="_lblTaskPriority" runat="server" Font-Bold="false" EnableTheming="false"
                                            Text="<%#DirectCast(Container.DataItem, IP.MEAS.BO.TaskItem).Priority%>"> </asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="bottom" Width="25%">
                                        <asp:Label ID="_lblTaskDueDate" runat="server" Font-Bold="false" EnableTheming="false"
                                            Text="<%#DirectCast(Container.DataItem, IP.MEAS.BO.TaskItem).DueDate%>"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableHeaderRow CssClass="Border">
                                    <asp:TableHeaderCell ColumnSpan="2" HorizontalAlign="left" Width="50%">
                                        <asp:Label ID="_lblTitle" runat="server" Text="<%$IPResources:Global,Title%>"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell ColumnSpan="2" HorizontalAlign="left" Width="50%">
                                        <asp:Label ID="_lblDescription" runat="server" Text="<%$IPResources:Global,Description%>"></asp:Label>
                                    </asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                                <asp:TableRow CssClass="Border">
                                    <asp:TableCell ColumnSpan="2" VerticalAlign="bottom" Width="50%">
                                        <asp:Label ID="_lblTaskTitle" runat="server" EnableTheming="false" Text="<%#DirectCast(Container.DataItem, IP.MEAS.BO.TaskItem).Title%>"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell ColumnSpan="2" VerticalAlign="bottom" Width="50%">
                                        <asp:Label ID="_lblTaskDescription" runat="server" EnableTheming="false" Text="<%#DirectCast(Container.DataItem, IP.MEAS.BO.TaskItem).Description%>"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <asp:Table ID="Table1" Width="100%" runat="server" CellSpacing="0" BorderColor="black"
                                BorderWidth="1" CellPadding="2">
                                <asp:TableHeaderRow CssClass="BorderSecondary">
                                    <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="50%" ColumnSpan="2">
                                        <asp:Label ID="_lblResponsiblePerson" runat="server" Text="<%$IPResources:Global,Responsible Person%>"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="25%">
                                        <asp:Label ID="_lblPriority" runat="server" Text="<%$IPResources:Global,Priority%>"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell VerticalAlign="top" HorizontalAlign="left" Width="25%">
                                        <asp:Label ID="_lblDueDate" runat="server" Text="<%$IPResources:Global,Due Date%>"></asp:Label>
                                    </asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                                <asp:TableRow CssClass="BorderSecondary">
                                    <asp:TableCell VerticalAlign="bottom" Width="50%" ColumnSpan="2">
                                        <asp:Label ID="_lblTaskResponsiblePerson" runat="server" Font-Bold="false" EnableTheming="false"
                                            Text="<%#DirectCast(Container.DataItem, IP.MEAS.BO.TaskItem).RoleName%>"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="bottom" Width="25%">
                                        <asp:Label ID="_lblTaskPriority" runat="server" Font-Bold="false" EnableTheming="false"
                                            Text="<%#DirectCast(Container.DataItem, IP.MEAS.BO.TaskItem).Priority%>"> </asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="bottom" Width="25%">
                                        <asp:Label ID="_lblTaskDueDate" runat="server" Font-Bold="false" EnableTheming="false"
                                            Text="<%#DirectCast(Container.DataItem, IP.MEAS.BO.TaskItem).DueDate%>"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableHeaderRow CssClass="BorderSecondary">
                                    <asp:TableHeaderCell ColumnSpan="2" HorizontalAlign="left" Width="50%">
                                        <asp:Label ID="_lblTitle" runat="server" Text="<%$IPResources:Global,Title%>"></asp:Label>
                                    </asp:TableHeaderCell>
                                    <asp:TableHeaderCell ColumnSpan="2" HorizontalAlign="left" Width="50%">
                                        <asp:Label ID="_lblDescription" runat="server" Text="<%$IPResources:Global,Description%>"></asp:Label>
                                    </asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                                <asp:TableRow CssClass="BorderSecondary">
                                    <asp:TableCell ColumnSpan="2" VerticalAlign="bottom" Width="50%">
                                        <asp:Label ID="_lblTaskTitle" runat="server" EnableTheming="false" Text="<%#DirectCast(Container.DataItem, IP.MEAS.BO.TaskItem).Title%>"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell ColumnSpan="2" VerticalAlign="bottom" Width="50%">
                                        <asp:Label ID="_lblTaskDescription" runat="server" EnableTheming="false" Text="<%#DirectCast(Container.DataItem, IP.MEAS.BO.TaskItem).Description%>"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
                <%--  <asp:Panel ID="_pnlAttachedFiles" runat="server" GroupingText="" Width="98%">
        <asp:Label ID="Label1" Text="Attached Files and Links" runat="server"></asp:Label>
        <asp:GridView ID="_grvAttachedFiles" runat="server" Width="100%" AutoGenerateColumns="False"
            DataKeyNames="TaskDocumentSeqID,taskheaderNumber" ShowHeader="false">
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <table width="100%">
                            <tr>
                                <td valign="top">
                                    Attachments/Links:</td>
                                <td valign="top">
                                    <div class="break-word" style="width: 800px; overflow: hidden;">
                                        <a target="_blank" href="<%#GetFileLocation(DirectCast(Container.DataItem, MTTDocuments).FileName,DirectCast(Container.DataItem, MTTDocuments).Location) %>">
                                            <%#GetAttachmentDisplay(DirectCast(Container.DataItem, MttDocuments).FileName, DirectCast(Container.DataItem, MttDocuments).Location)%>
                                        </a>
                                    </div>
                                    <asp:HiddenField ID="_TaskDocumentSeqID" runat="server" Value='<%#DirectCast(Container.DataItem, MTTDocuments).TaskDocumentSeqID %>' />
                                    <asp:HiddenField ID="_TaskHeaderSeqID" runat="server" Value='<%#DirectCast(Container.DataItem, MTTDocuments).TaskHeaderNumber %>' />
                                    <asp:HiddenField ID="_Location" runat="server" Value='<%#DirectCast(Container.DataItem, MTTDocuments).Location %>' />
                                    <asp:HiddenField ID="_FileName" runat="server" Value='<%#DirectCast(Container.DataItem, MTTDocuments).FileName %>' />
                                    <asp:HiddenField ID="_rowIndex" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    Description:</td>
                                <td valign="top">
                                    <div class="break-word" style="width: 800px; overflow: hidden;">
                                        <asp:Literal ID="_txtDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Literal></div>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="_txtFileName" runat="server" Width="90%" Text='<%#GetAttachmentDisplay(DirectCast(Container.DataItem, MttDocuments).FileName, DirectCast(Container.DataItem, MttDocuments).Location)%>'></asp:TextBox>
                        <asp:HiddenField ID="_TaskDocumentSeqID" runat="server" Value='<%#DirectCast(Container.DataItem, MTTDocuments).TaskDocumentSeqID %>' />
                        <asp:HiddenField ID="_TaskHeaderSeqID" runat="server" Value='<%#DirectCast(Container.DataItem, MTTDocuments).TaskHeaderNumber %>' />
                        <asp:HiddenField ID="_Location" runat="server" Value='<%#DirectCast(Container.DataItem, MTTDocuments).Location %>' />
                        <asp:HiddenField ID="_FileName" runat="server" Value='<%#DirectCast(Container.DataItem, MTTDocuments).FileName %>' />
                        <br />
                        <IP:AdvancedTextBox ID="_txtDescription" runat="server" Width="90%" Text='<%# Bind("Description") %>'
                            Rows="2" TextMode="MultiLine" Wrap="true" MaxLength="3500"></IP:AdvancedTextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <asp:Button ID="_btnEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%#DirectCast(Container.DataItem, MTTDocuments).TaskDocumentSeqID%>'
                            OnClick="EditAttachment" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button ID="_btnUpdate" runat="server" Text="Update" CommandName="Update" CommandArgument='<%#DirectCast(Container.DataItem, MTTDocuments).TaskDocumentSeqID%>'
                            OnClick="UpdateAttachment" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>                        
                        <asp:Button ID="_btnDelete" runat="server" Text="Delete" CommandName="Delete" CommandArgument='<%#DirectCast(Container.DataItem, MTTDocuments).TaskDocumentSeqID%>'
                            OnClick="DeleteAttachment" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button ID="_btnCancel" runat="server" Text="Cancel" CommandName="Cancel" CommandArgument='<%#DirectCast(Container.DataItem, MTTDocuments).TaskDocumentSeqID%>'
                            OnClick="CancelEditAttachment" />
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>--%>
            </asp:Panel>
            <IP:MessageBox ID="ctl00__AlertMessage" runat="server" ButtonType="OK" />


        </div>
    </form>
    <script type="text/javascript">
        $(document).on('change', ':file', function () {
            var val = $(this).val();

            switch (val.substring(val.lastIndexOf('.') + 1).toLowerCase()) {
                case 'xlsx':
                    $('#_lblSelectedFile').text(val);
                    $('#_btnUploadFile').show();
                   // $('#_fileContainer').attr("display", "none");
                    //$(this).attr("disabled","disabled");
                    break;
                default:
                    $(this).val('');
                    $('#_lblSelectedFile').text('');
                    $('#_btnUploadFile').hide();
                    // error message here
                    alert("This is not a valid file.  Please upload a file with an extention of one of the following:\n\n.xlsx");
                    break;
            }

            var input = $(this),
                numFiles = input.get(0).files ? input.get(0).files.length : 1,
                label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
            input.trigger('fileUpEx', [numFiles, label]);

        });

        $(document).ready(function () {
            $(':file').on('fileUpEx', function (event, numFiles, label) {
                //console.log(numFiles);
                //console.log(label);


            });
        });
    </script>
</body>
</html>
