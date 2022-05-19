<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/TaskTrackerResponsive.master"
    AutoEventWireup="false" CodeFile="Default2.aspx.vb" Inherits="Default2" %>
<%@ MasterType VirtualPath="~/MasterPages/TaskTrackerResponsive.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="_cphMainContent" runat="Server">
   <IP:EmployeeList ID="_ResponsiblePerson" runat="server" UserMode="UsersAndRoles"
                                DisplayClearLink="false"  AutoPostBack="false"
                                Employeelabel="" EnableValidation="true">
                                <validation validationgroup="TaskDetails" validationerrormessage="<%$IPResources:Global,Responsible Person is required%>"
                                    validationerrortext="" validateemptytext="true" />
                            </IP:EmployeeList>
     <IP:EmployeeList ID="EmployeeList1" runat="server" UserMode="UsersAndRoles"
                                DisplayClearLink="false"  AutoPostBack="false"
                                Employeelabel="" EnableValidation="true" ShowInactiveUsers="true">
                                <validation validationgroup="TaskDetails" validationerrormessage="<%$IPResources:Global,Responsible Person is required%>"
                                    validationerrortext="" validateemptytext="true" />
                            </IP:EmployeeList>
 
 

</asp:Content>
