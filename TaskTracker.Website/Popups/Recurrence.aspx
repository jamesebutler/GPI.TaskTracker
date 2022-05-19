<%@ Page Language="VB" MasterPageFile="~/MasterPages/TaskTrackerPopup.master" AutoEventWireup="false"
    CodeFile="Recurrence.aspx.vb" Inherits="PopupsRecurrence" %>

<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerPopup.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
    <div class="panel panel-default">
        <div class="panel-heading">
            <h1 class="panel-title">
                <asp:Label ID="_lblHeaderTitle" runat="server"></asp:Label></h1>
        </div>
        <div class="panel-body">
            <asp:Label runat="server" Text="<%$IPResources:Global,Recurrence Pattern%>"></asp:Label><br />
            <asp:RadioButtonList ID="_rblFrequency" AutoPostBack="true" runat="server" RepeatDirection="Horizontal">
            </asp:RadioButtonList>

            <div class="row">
                <div class="col-xs-12 col-md-6">
                    <asp:Panel ID="_pnlDailyFrequency" runat="server"
                        Width="98%" Height="220px" CssClass="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">
                                <asp:Label ID="Label3" runat="server" Text="<%$IPResources:Global,Daily%>"></asp:Label></h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-inline">
                                <div class="form-group">
                                    <asp:Literal runat="server" Text="<%$IPResources:Global,Every%>"></asp:Literal>
                                    <asp:TextBox ID="_txtEveryDay" runat="server" Width="50" MaxLength="3" ValidationGroup="ValidationDays"></asp:TextBox>&nbsp;
                                    <asp:Localize runat="server" Text="<%$IPResources:Global,day(s)%>"></asp:Localize>
                                    <asp:RequiredFieldValidator ID="_rfvEveryDays" runat="server" ControlToValidate="_txtEveryDay"
                                        Display="Dynamic" Text="*" ErrorMessage="<%$IPResources:Global,Number of days is required%>"
                                        ValidationGroup="ValidationDays" InitialValue="" EnableClientScript="true"></asp:RequiredFieldValidator>
                                    <asp:RangeValidator
                                        ID="RangeValidator1" Type="Integer" runat="server" ControlToValidate="_txtEveryDay"
                                        Display="Static" Text="*" ErrorMessage="<%$IPResources:Global,Days must be between 1 and 365%>"
                                        MaximumValue="365" MinimumValue="1" ValidationGroup="ValidationDays"></asp:RangeValidator>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="_ftbe" runat="server" TargetControlID="_txtEveryDay"
                                        FilterType="Numbers" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="_pnlWeeklyFrequency" runat="server"
                        Width="98%" Height="220px" CssClass="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">
                                <asp:Label ID="Label1" runat="server" Text="<%$IPResources:Global,Weekly%>"></asp:Label></h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-inline">
                                <div class="form-group">
                                    <asp:Localize runat="server" Text="<%$IPResources:Global,Every%>"></asp:Localize>
                                    <asp:TextBox ID="_txtEveryXWeeks" runat="server" Width="50" MaxLength="3" ValidationGroup="ValidationWeeks"></asp:TextBox>&nbsp;
                                    <asp:Localize ID="Localize1" runat="server" Text="<%$IPResources:Global,week(s) on%>"></asp:Localize>

                                    <asp:RangeValidator Type="Integer" ID="_rfvWeeks" runat="server" ControlToValidate="_txtEveryXWeeks"
                                        Display="Static" ErrorMessage="<%$IPResources:Global,Weeks must be between 1 and 53%>"
                                        MaximumValue="53" MinimumValue="1" Text="*" ValidationGroup="ValidationWeeks"></asp:RangeValidator><asp:RequiredFieldValidator
                                            ID="_rfvEveryXWeeks" runat="server" ControlToValidate="_txtEveryXWeeks" Display="Dynamic"
                                            Text="*" ErrorMessage="<%$IPResources:Global,Number of weeks is required%>" ValidationGroup="ValidationWeeks"
                                            InitialValue="" EnableClientScript="true"></asp:RequiredFieldValidator>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="_ftbeWeeks" runat="server" TargetControlID="_txtEveryXWeeks"
                                        FilterType="Numbers" />
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-xs-12">
                                        <asp:CheckBoxList ID="_cblDaysOfWeek" runat="server" RepeatColumns="4" RepeatDirection="horizontal"
                                            Width="100%">
                                        </asp:CheckBoxList>
                                        <IP:ListValidator ID="_rfvDaysOfWeek" runat="server" ControlToValidate="_cblDaysOfWeek"
                                            Text="" ErrorMessage="<%$IPResources:Global,Day of week is required%>" ValidationGroup="ValidationWeeks">
                                        </IP:ListValidator>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </asp:Panel>

                    <asp:Panel ID="_pnlMonthlyFrequency" CssClass="panel panel-default" runat="server" Height="220px"
                        Width="98%">
                        <div class="panel-heading">
                            <h3 class="panel-title">
                                <asp:Label ID="Label6" runat="server" Text="<%$IPResources:Global,Monthly%>"></asp:Label></h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-inline">
                                <div class="form-group">
                                    <asp:RadioButton ID="_rbDayXOfMonth" AutoPostBack="true" runat="server" GroupName="MonthlyFrequency"
                                        Text="<%$IPResources:Global,Day%>" />
                                    <asp:TextBox ID="_txtDayXOfMonth" Width="50" MaxLength="2" runat="server"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Localize runat="server" Text="<%$IPResources:Global,of every%>"></asp:Localize>
                                    <asp:TextBox ID="_txtEveryXMonths"
                                        runat="server" MaxLength="3" Width="50"></asp:TextBox>
                                    <asp:Localize runat="server" Text="<%$IPResources:Global,month(s)%>"></asp:Localize>
                                </div>
                            </div>
                            <br />
                            <div class="form-inline">
                                <div class="form-group">
                                    <asp:RadioButton ID="_rbWeekDayOfMonth" AutoPostBack="true" runat="server" GroupName="MonthlyFrequency"
                                        Text="<%$IPResources:Global,The%>" />
                                </div>
                                <div class="form-group">
                                    <asp:DropDownList ID="_ddlOrdinalDayOfMonth" runat="server">
                                        <asp:ListItem Text="<%$IPResources:Global,First%>" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="<%$IPResources:Global,Second%>" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="<%$IPResources:Global,Third%>" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="<%$IPResources:Global,Fourth%>" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="<%$IPResources:Global,Last%>" Value="999"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:DropDownList ID="_ddlMonthDayOfWeek" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:Localize runat="server" Text="<%$IPResources:Global,of every%>"></asp:Localize>
                                    <asp:TextBox ID="_txtDayOfWeekForEveryXMonths"
                                        runat="server" Width="50" MaxLength="3"></asp:TextBox><asp:Localize runat="server" Text="<%$IPResources:Global,month(s)%>"></asp:Localize>
                                    <asp:RequiredFieldValidator ID="_rfvDayOfWeekForEveryXMonths"
                                        runat="server" ControlToValidate="_txtDayOfWeekForEveryXMonths" Text="*" Display="Dynamic"
                                        ErrorMessage="<%$IPResources:Global,The number of months is required%>" Enabled="false"
                                        InitialValue="" EnableClientScript="true"></asp:RequiredFieldValidator><asp:RangeValidator
                                            Type="Integer" ID="_rvDayOfWeekForEveryXMonths" Display="dynamic" runat="server"
                                            ValidationGroup="ValidationDayXOfMonth" ControlToValidate="_txtDayOfWeekForEveryXMonths"
                                            ErrorMessage="<%$IPResources:Global,Number of months should be between 1 and 60%>"
                                            MaximumValue="60" MinimumValue="1" Text="*"></asp:RangeValidator>
                                </div>
                            </div>
                            <br />
                            <div class="form-inline">
                                <div class="form-group">
                                    <asp:RadioButton ID="_rblLastDayOfMonth" AutoPostBack="true" runat="server" GroupName="MonthlyFrequency"
                                        Text="<%$IPResources:Global,Last Day of Month%>" />&nbsp;<asp:Localize runat="server"
                                            Text="<%$IPResources:Global,of every%>"></asp:Localize><asp:TextBox ID="_txtLastDayEveryXMonths"
                                                runat="server" Width="50" MaxLength="3"></asp:TextBox>&nbsp;<asp:Localize runat="server"
                                                    Text="<%$IPResources:Global,month(s)%>"></asp:Localize>
                                </div>
                            </div>
                            <br />

                            <div class="form-inline">
                                <div class="form-group">
                                    <asp:RequiredFieldValidator ID="_rfvDayXOfMonth" runat="server" ControlToValidate="_txtDayXOfMonth"
                                        Text="*" Display="Dynamic" ErrorMessage="<%$IPResources:Global,Day of month is required%>"
                                        Enabled="false" InitialValue="" EnableClientScript="true"></asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="_rvDayXOfMonth" Display="dynamic" runat="server" ValidationGroup="ValidationDayXOfMonth"
                                        ControlToValidate="_txtDayXOfMonth" ErrorMessage="<%$IPResources:Global,Day of month should be between 1 and 31%>"
                                        MaximumValue="31" MinimumValue="1" Text="*" Type="Integer"></asp:RangeValidator>
                                    <asp:RequiredFieldValidator ID="_rfvEveryXMonths" runat="server" ControlToValidate="_txtEveryXMonths"
                                        Text="*" Display="Dynamic" ErrorMessage="<%$IPResources:Global,The number of months is required%>"
                                        Enabled="false" InitialValue="" EnableClientScript="true"></asp:RequiredFieldValidator>
                                    <asp:RangeValidator Type="Integer" ID="_rvEveryXMonths" Display="dynamic" runat="server"
                                        ValidationGroup="ValidationDayXOfMonth" ControlToValidate="_txtEveryXMonths"
                                        ErrorMessage="<%$IPResources:Global,Number of months should be between 1 and 60%>"
                                        MaximumValue="60" MinimumValue="1" Text="*"></asp:RangeValidator>
                                </div>
                            </div>



                            <table>
                                <tr>
                                    <td>&nbsp;
                                    &nbsp;
                                    
                                    
                                        
                                        &nbsp;&nbsp;
                                        &nbsp;<br />

                                        &nbsp;<asp:RequiredFieldValidator ID="_rfvLastDayEveryXMonths" runat="server" ControlToValidate="_txtLastDayEveryXMonths"
                                            Text="*" Display="Dynamic" ErrorMessage="<%$IPResources:Global,The number of months is required%>"
                                            Enabled="false" InitialValue="" EnableClientScript="true"></asp:RequiredFieldValidator><asp:RangeValidator
                                                Type="Integer" ID="_rvLastDayEveryXMonths" Display="dynamic" runat="server" ValidationGroup="ValidationDayXOfMonth"
                                                ControlToValidate="_txtLastDayEveryXMonths" ErrorMessage="<%$IPResources:Global,Number of months should be between 1 and 60%>"
                                                MaximumValue="60" MinimumValue="1" Text="*"></asp:RangeValidator>
                                        <br />
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                            TargetControlID="_txtDayXOfMonth" FilterType="Numbers" />
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                            TargetControlID="_txtEveryXMonths" FilterType="Numbers" />
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                            TargetControlID="_txtDayOfWeekForEveryXMonths" FilterType="Numbers" />
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server"
                                            TargetControlID="_txtLastDayEveryXMonths" FilterType="Numbers" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="_pnlYearly" runat="server" CssClass="panel panel-default" Height="220px"
                        Width="98%">
                        <div class="panel-heading">
                            <h3 class="panel-title">
                                <asp:Label ID="Label7" runat="server" Text="<%$IPResources:Global,Yearly%>"></asp:Label></h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-inline">
                                <div class="form-group">
                                    <asp:RadioButton ID="_rblDayXOfYear" AutoPostBack="true" runat="server" GroupName="YearlyFrequency"
                                        Text="<%$IPResources:Global,Day%>" />&nbsp;<asp:RequiredFieldValidator ID="_rfvDayOfWeekXYears" runat="server"
                                            SetFocusOnError="true" Text="*" ControlToValidate="_txtDayXOfYear" Display="Dynamic"
                                            ErrorMessage="<%$IPResources:Global,The number of years is required%>">
                                        </asp:RequiredFieldValidator><asp:RangeValidator Type="Integer" ID="_rvDayXOfYear"
                                            Display="dynamic" runat="server" ValidationGroup="ValidationDayXOfMonth" ControlToValidate="_txtDayXOfYear"
                                            ErrorMessage="<%$IPResources:Global,Number of years should be between 1 and 31%>"
                                            MaximumValue="31" MinimumValue="1" Text="*"></asp:RangeValidator><asp:TextBox ID="_txtDayXOfYear"
                                                Width="50" runat="server" MaxLength="2"></asp:TextBox>&nbsp;<asp:Localize runat="server"
                                                    Text="<%$IPResources:Global,of%>"></asp:Localize>
                                </div>

                                <div class="form-group">
                                    <asp:DropDownList ID="_ddlDayXMonthsOfYear" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:Localize runat="server" Text="<%$IPResources:Global,of every%>"></asp:Localize>&nbsp;<asp:RequiredFieldValidator
                                        ID="_rfvDayOfWeekForEveryXYears" runat="server" SetFocusOnError="true" Text="*"
                                        ControlToValidate="_txtDayOfWeekForEveryXYears" Display="Dynamic" ErrorMessage="<%$IPResources:Global,Number of years is required%>">
                                    </asp:RequiredFieldValidator><asp:RangeValidator Type="Integer" ID="_rvDayOfWeekForEveryXYears"
                                        Display="dynamic" runat="server" ValidationGroup="ValidationDayXOfMonth" ControlToValidate="_txtDayOfWeekForEveryXYears"
                                        ErrorMessage="<%$IPResources:Global,Number of years should be between 1 and 60%>"
                                        MaximumValue="60" MinimumValue="1" Text="*"></asp:RangeValidator><asp:TextBox ID="_txtDayOfWeekForEveryXYears"
                                            runat="server" Width="50" MaxLength="2"></asp:TextBox>
                                    &nbsp;<asp:Localize runat="server" Text="<%$IPResources:Global,year(s)%>"></asp:Localize>
                                </div>
                            </div>
                            <br />
                            <div class="form-inline">
                                <div class="form-group">
                                    <asp:RadioButton ID="_rbWeekDayOfYear" AutoPostBack="true" runat="server" GroupName="YearlyFrequency"
                                        Text="<%$IPResources:Global,The%>" />
                                </div>
                                <div class="form-group">
                                    <asp:DropDownList ID="_ddlOrdinalDayOfYear"
                                        runat="server">
                                        <asp:ListItem Text="<%$IPResources:Global,First%>" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="<%$IPResources:Global,Second%>" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="<%$IPResources:Global,Third%>" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="<%$IPResources:Global,Fourth%>" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="<%$IPResources:Global,Last%>" Value="999"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:DropDownList ID="_ddlYearlyDayOfWeek" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:Localize runat="server" Text="<%$IPResources:Global,of%>"></asp:Localize>
                                </div>
                                <div class="form-group">
                                    <asp:DropDownList ID="_ddlMonthsOfYear" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <asp:Literal ID="Literal1" runat="server" Text="<%$IPResources:Global,Of Every%>"></asp:Literal>
                                    <asp:TextBox ID="_txtWeekDayOfXYears"
                                        runat="server" Width="50" MaxLength="2"></asp:TextBox>&nbsp;<asp:Localize runat="server" Text="<%$IPResources:Global,year(s)%>"></asp:Localize>
                                </div>
                            </div>
                            <table width="100%">
                                <tr>
                                    <td>&nbsp;&nbsp;<asp:RequiredFieldValidator ID="_rfvWeekDayOfXYears" runat="server"
                                        SetFocusOnError="true" Text="*" ControlToValidate="_txtWeekDayOfXYears" Display="Dynamic"
                                        ErrorMessage="<%$IPResources:Global,Number of years is required%>">
                                    </asp:RequiredFieldValidator><asp:RangeValidator Type="Integer" ID="_rvWeekDayOfXYears"
                                        Display="dynamic" runat="server" ValidationGroup="ValidationDayXOfMonth" ControlToValidate="_txtWeekDayOfXYears"
                                        ErrorMessage="<%$IPResources:Global,Number of years should be between 1 and 60%>"
                                        MaximumValue="60" MinimumValue="1" Text="*"></asp:RangeValidator>
                                        <br />
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                            TargetControlID="_txtDayXOfYear" FilterType="Numbers" />
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"
                                            TargetControlID="_txtDayOfWeekForEveryXYears" FilterType="Numbers" />
                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server"
                                            TargetControlID="_txtWeekDayOfXYears" FilterType="Numbers" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
                <div class="col-xs-12 col-md-6">
                    <asp:Panel ID="_pnlRangeOfRecurrence" runat="server"
                        Width="98%" Height="220px" CssClass="panel panel-default">
                        <div class="panel-heading">
                            <asp:Label ID="Label2" runat="server" Text="<%$IPResources:Global,Range of recurrence%>"></asp:Label>
                        </div>
                        <div class="panel-body">
                            <table>
                                <tr>
                                    <td colspan="3">
                                        <asp:RadioButton ID="_rbNoEndDate" AutoPostBack="true" runat="server" Text="<%$IPResources:Global,No End Date%>"
                                            GroupName="RangeOfRecurrence" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="_rbEndAfter" AutoPostBack="true" runat="server" Text="<%$IPResources:Global,End After%>"
                                            GroupName="RangeOfRecurrence" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="_rfEndAfter" runat="server" ControlToValidate="_txtEndAfter"
                                            Display="Dynamic" ErrorMessage="<%$IPResources:Global,Number of Occurrences is required%>"
                                            Enabled="false" InitialValue="" EnableClientScript="true" Text="*"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator Type="Integer" ID="_rvEndAfter" Display="dynamic" runat="server"
                                            ControlToValidate="_txtEndAfter" ErrorMessage="<%$IPResources:Global,Number of Occurrences should be between 1 and 500%>"
                                            Text="*" MaximumValue="500" MinimumValue="1"></asp:RangeValidator><asp:TextBox ID="_txtEndAfter"
                                                MaxLength="3" runat="server" Width="80" CausesValidation="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="_lblEndAfterOccurrences" Text="<%$IPResources:Global,Occurrences%>"
                                            runat="server" SkinID="None"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="_rbEndBy" AutoPostBack="true" runat="server" Text="<%$IPResources:Global,End By%>"
                                            GroupName="RangeOfRecurrence" />
                                    </td>
                                    <td colspan="2">
                                        <IP:JQDatePicker ShowDateRange="false" ShowLabelsOnSameLine="true" ShowFromDate="false"
                                            ID="_dtEndbyDate" runat="server" FromLabel="" EnableValidation="true">
                                            <validation validationerrortext="*" validateemptytext="true" validationerrormessage="<%$IPResources:Global,End by date is required%>" />
                                        </IP:JQDatePicker>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server"
                            TargetControlID="_txtEndAfter" FilterType="Numbers" />
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

    <asp:Panel ID="_pnlRecurrencePattern" runat="server" CssClass="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">
                <asp:Label ID="Label4" runat="server" Text="<%$IPResources:Global,Recurrence Pattern%>"></asp:Label></h3>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-xs-12 col-md-4">
                    <asp:LinkButton ID="_btnSaveRecurrence" runat="server" SkinID="Button">
                        <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-floppy-save"></span>&nbsp;<asp:Label ID="Label9" runat="server" Text="<%$IPResources:Global,Save Changes%>"></asp:Label></asp:PlaceHolder>
                    </asp:LinkButton>

                </div>
                <div class="col-xs-12 col-md-4">
                    <asp:LinkButton ID="_btnCancel" runat="server" SkinID="Button" OnClientClick="$(window.location).attr('href', window.location); return false;">
                        <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-refresh"></span>&nbsp;<asp:Label ID="_lblResetSelections" runat="server" Text="<%$IPResources:Global,Reset Selections%>"></asp:Label></asp:PlaceHolder>
                    </asp:LinkButton>
                </div>
                <div class="col-xs-12 col-md-4">
                    <asp:LinkButton ID="_btnCancelRecurrence" runat="server" SkinID="Button">
                        <asp:PlaceHolder runat="server"><span class="glyphicon glyphicon-ban-circle"></span>&nbsp;<asp:Label ID="Label10" runat="server" Text="<%$IPResources:Global,End Recurrence%>"></asp:Label></asp:PlaceHolder>
                    </asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender ID="_cbeCancelRecurrence" runat="server" TargetControlID="_btnCancelRecurrence"
                        ConfirmText="<%$IPResources:Global,Are you sure that you would like to End the Recurrence Schedule? Doing so will delete all Open tasks in the future!%>"></ajaxToolkit:ConfirmButtonExtender>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <asp:ValidationSummary ID="_valSummary" DisplayMode="BulletList" ShowMessageBox="true"
                        runat="server" HeaderText="<%$IPResources:Global,Please verify the entered data%>"></asp:ValidationSummary>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="_pnlRecurringTasks" runat="server" ScrollBars="Vertical" Height="200px" CssClass="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">
                <asp:Label ID="Label5" runat="server" Text="<%$IPResources:Global,Recurrence Schedule%>"></asp:Label></h3>
        </div>
        <div class="panel-body">
            <asp:DataList ID="_dlRecurringTasks" runat="server" Width="100%" CellSpacing="2"
                RepeatLayout="table" RepeatDirection="horizontal" RepeatColumns="6" EnableViewState="false">
                <ItemTemplate>
                    &bull;<asp:Label ID="_lblRecurringTasks" runat="server" BorderWidth="0" EnableViewState="true"
                        Text='<%# DirectCast(Container.DataItem, System.Data.DataRowView)("taskdate") %>'></asp:Label>
                </ItemTemplate>
            </asp:DataList>
            <asp:Label ID="_lblNoRecurringTasks" runat="server" Text="<%$IPResources:Global,None%>" Visible="false"></asp:Label>
        </div>

    </asp:Panel>
</asp:Content>

