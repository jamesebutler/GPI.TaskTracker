'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 10-25-2010
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports System.Data
Partial Class PopupsResponsibleUsers
    Inherits System.Web.UI.Page



    'Public Sub PopulateNode(ByVal source As Object, ByVal e As TreeNodeEventArgs)
    '    Dim currentNode As TreeNode = e.Node '_tvFunctionalLocation.SelectedNode
    '    Dim currentValue As String = e.Node.Value ' Me._tvFunctionalLocation.SelectedValue
    '    AddChildNodes(currentNode, currentValue)
    'End Sub

    'Public Function AddChildNodes(ByVal currentNode As TreeNode, ByVal currentValue As String, Optional ByVal nextValue As String = "") As TreeNode


    '    'Dim dr As DataTableReader 'OracleDataReader 
    '    'Dim parentValue As String = currentValue
    '    'Dim newValue As String = String.Empty
    '    'Dim NextNode As TreeNode = Nothing
    '    'Dim currentLevel As Integer = currentValue.Length
    '    'Dim sql As String
    '    'Dim newNodeCount As Integer = 0
    '    'Dim showCheckBoxes As Boolean = True
    '    'Dim ds As DataSet = Nothing
    '    'Dim potentialChildNodes As Boolean = True

    '    'If Mid(currentValue, 1, 1) = "*" Then
    '    '    Dim siteid As String = Mid(currentValue, 2, 2)
    '    '    sql = "Select distinct rcfaflid,a.siteid,b.sitename,equipmentid,equipmentdesc,criticality,(Select  count(equipmentid) FROM RefEQUIPMENT c, refsite b WHERE c.siteid = b.siteid  and c.equipmentid like a.equipmentid||'%' and (instr(equipmentid,'-',1,1)>0 and instr(equipmentid,'-',1,2)=0)) childNodes  FROM RefEQUIPMENT a, refsite b WHERE a.siteid = b.siteid  and length(equipmentid)=3 and a.siteid='{0}'   order by b.sitename,equipmentid"
    '    '    sql = String.Format(sql, siteid)
    '    'ElseIf Mid(currentValue, 1, 1) = "$" Then
    '    '    Dim siteid As String = Mid(currentValue, 2, 2)
    '    '    'sql = "Select  rcfaflid,a.siteid,b.sitename,equipmentid,equipmentdesc  FROM RefEQUIPMENT a, refsite b WHERE a.siteid = b.siteid and  (instr(equipmentid,'-',1,1)>0 and instr(equipmentid,'-',1,3)=0) and a.siteid='{0}'   order by b.sitename,equipmentid"
    '    '    sql = "Select distinct a.siteid,b.sitename,substr(equipmentid,1,3) equipmentid,criticality,b.sitename||' ('||substr(equipmentid,1,3)||'...)' equipmentdesc,decode((Select distinct count(equipmentid) FROM RefEQUIPMENT c, refsite b WHERE c.siteid = b.siteid and c.siteid='{0}'  and c.equipmentid like a.equipmentid||'%'),0,0,1) childNodes FROM RefEQUIPMENT a, refsite b WHERE a.siteid = b.siteid  and  a.siteid='{0}' order by b.sitename,equipmentid"
    '    '    sql = String.Format(sql, siteid)
    '    '    showCheckBoxes = False
    '    'ElseIf Mid(currentNode.ValuePath, 1, 1) = "$" Then
    '    '    Dim siteid As String = Mid(currentNode.ValuePath, 2, 2)
    '    '    'sql = "Select  rcfaflid,a.siteid,b.sitename,equipmentid,equipmentdesc  FROM RefEQUIPMENT a, refsite b WHERE a.siteid = b.siteid  = 'Yes' and  (instr(equipmentid,'-',1,1)>0 and instr(equipmentid,'-',1,3)=0) and a.siteid='{0}'   order by b.sitename,equipmentid"
    '    '    sql = "Select distinct a.siteid,b.sitename,equipmentid,equipmentdesc,criticality,(Select  count(equipmentid) FROM RefEQUIPMENT c, refsite b WHERE c.siteid = b.siteid  and c.equipmentid like a.equipmentid||'%' and (instr(equipmentid,'-',1,{1})>0 and instr(equipmentid,'-',1,{1})=0)) childNodes FROM RefEQUIPMENT a, refsite b WHERE a.siteid = b.siteid  and  a.siteid='{0}' and equipmentid like '{1}%' order by b.sitename,equipmentid"
    '    '    sql = String.Format(sql, siteid, currentValue)
    '    '    potentialChildNodes = False
    '    'Else
    '    '    sql = "Select distinct rcfaflid,a.siteid,b.sitename,equipmentid,equipmentdesc,criticality,(Select  count(equipmentid) FROM RefEQUIPMENT c, refsite b WHERE c.siteid = b.siteid and c.equipmentid like a.equipmentid||'%' and (instr(equipmentid,'-',1,{2})>0 and instr(equipmentid,'-',1,{3})=0)) childNodes  FROM RefEQUIPMENT a, refsite b WHERE a.siteid = b.siteid  and equipmentid like'{0}-%' and (instr(equipmentid,'-',1,{1})>0 and instr(equipmentid,'-',1,{2})=0)   order by b.sitename,equipmentid"
    '    '    currentLevel = Me.InStrCount(currentValue, "-")
    '    '    sql = String.Format(sql, currentValue, currentLevel + 1, currentLevel + 2, currentLevel + 3)
    '    'End If


    '    ''Dim key As String = "FunctionalLocation_" & RI.SharedFunctions.CreateKey(sql)
    '    'Dim key As String = "RI_FUNCTIONAL_LOCATION_" & RI.SharedFunctions.CreateKey(sql)

    '    'If Cache.Item(key) IsNot Nothing Then
    '    '    ds = CType(Cache.Item(key), DataSet)
    '    'Else
    '    '    ds = RI.SharedFunctions.GetOracleDataSet(sql)
    '    '    If ds.Tables(0).DefaultView.Count > 0 Then
    '    '        Cache.Insert(key, ds, Nothing, DateTime.Now.AddHours(12), TimeSpan.Zero)
    '    '    End If
    '    'End If
    '    'dr = ds.Tables(0).CreateDataReader 'RI.SharedFunctions.GetOracleDataReader(sql)
    '    'If dr IsNot Nothing Then
    '    '    If dr.HasRows Then
    '    '        While dr.Read
    '    '            If dr.Item("equipmentid") IsNot Nothing Then
    '    '                newNodeCount += 1
    '    '                newValue = CStr(dr.Item("Equipmentid"))
    '    '                Dim relationship As NodeRelationship
    '    '                If newNodeCount = 1 Then ' first record is a child
    '    '                    relationship = NodeRelationship.Child
    '    '                Else
    '    '                    relationship = NodeRelationship.Sibling
    '    '                End If
    '    '                'relationship = Me.DetermineRelationship(parentValue, currentValue, newValue)
    '    '                Dim newNodeIndex As Integer = -1
    '    '                Dim newNode As TreeNode = New TreeNode(newValue & " * " & RI.SharedFunctions.DataClean(dr.Item("equipmentdesc".ToString) & " * " & RI.SharedFunctions.DataClean(dr.Item("Criticality"))), newValue)

    '    '                'newNode.PopulateOnDemand = True
    '    '                newNode.SelectAction = TreeNodeSelectAction.Expand
    '    '                If dr.Item("childNodes") IsNot Nothing Then
    '    '                    If CStr(dr.Item("childNodes")) = "0" Then
    '    '                        potentialChildNodes = False
    '    '                    Else
    '    '                        potentialChildNodes = True
    '    '                    End If
    '    '                Else
    '    '                    potentialChildNodes = True
    '    '                End If
    '    '                If potentialChildNodes = False Then

    '    '                    newNode.PopulateOnDemand = False
    '    '                Else
    '    '                    If newNode.ChildNodes.Count = 0 Then
    '    '                        newNode.PopulateOnDemand = True
    '    '                    Else
    '    '                        newNode.PopulateOnDemand = False
    '    '                    End If
    '    '                End If

    '    '                newNode.ShowCheckBox = showCheckBoxes

    '    '                Select Case relationship
    '    '                    Case NodeRelationship.Child
    '    '                        currentNode.ChildNodes.Add(newNode) 'New TreeNode(dr.Item("equipmentid".ToString) & "__" & RI.SharedFunctions.DataClean(dr.Item("equipmentdesc".ToString)), dr.Item("equipmentid".ToString)))
    '    '                        currentNode = currentNode.ChildNodes.Item(currentNode.ChildNodes.Count - 1)
    '    '                    Case NodeRelationship.Sibling
    '    '                        currentNode.Parent.ChildNodes.Add(newNode) 'New TreeNode(dr.Item("equipmentid".ToString) & "__" & RI.SharedFunctions.DataClean(dr.Item("equipmentdesc".ToString)), dr.Item("equipmentid".ToString)))
    '    '                        currentNode = currentNode.Parent.ChildNodes.Item(currentNode.Parent.ChildNodes.Count - 1)
    '    '                    Case NodeRelationship.ParentSibling
    '    '                        currentNode.Parent.Parent.ChildNodes.Add(newNode)
    '    '                        currentNode = currentNode.Parent.Parent.ChildNodes.Item(currentNode.Parent.Parent.ChildNodes.Count - 1)
    '    '                End Select
    '    '                If nextValue.Length > 0 And newValue = nextValue Then
    '    '                    NextNode = currentNode
    '    '                    'NextNode.Checked = True
    '    '                End If
    '    '                'currentNode.Selected = True
    '    '                'currentNode.Expanded = True
    '    '                'currentValue = CStr(dr.Item("equipmentid"))
    '    '            End If
    '    '        End While
    '    '        If dr IsNot Nothing Then
    '    '            dr.Close()
    '    '            dr = Nothing
    '    '        End If
    '    '        If ds IsNot Nothing Then
    '    '            ds = Nothing
    '    '        End If
    '    '    Else
    '    '        currentNode.Target = "populated"
    '    '        currentNode.NavigateUrl = "http://#"
    '    '    End If
    '    'End If
    '    'If NextNode IsNot Nothing Then
    '    '    Return NextNode
    '    'Else
    '    Return currentNode.Parent
    '    'End If
    'End Function
    Public Sub PopulateTreeRoot()
        Dim taskItem As New TaskTrackerListsBll
        Dim plantCode As String = Me._ddlResponsibleFacility.SelectedValue
        Dim userList As System.Collections.Generic.List(Of ResponsibleUsers) = TaskTrackerListsBll.GetResponsibleUsers(plantCode)
        Dim roleDescription As String = String.Empty

        roleDescription = String.Empty
        For Each item As ResponsibleUsers In userList
            Dim spaceChar As String = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
            With _ddlResponsibleUser

                If item.RoleDescription <> roleDescription Then 'New Group
                    Dim roleItem As New ListItem
                    roleDescription = item.RoleDescription
                    roleItem.Text = item.RoleDescription.ToUpper
                    roleItem.Value = item.RoleSeqID

                    If .Items.Count > 0 Then
                        Dim blankItem As New ListItem
                        With blankItem
                            .Attributes.Add("disabled", "true")
                            .Text = ""
                            .Value = -1
                        End With
                        .Items.Add(blankItem)
                    End If

                    If roleDescription.Length > 0 And item.RoleSeqID > 0 Then
                        'roleItem.Attributes.Add("optgroup", RoleDescription)

                        roleItem.Attributes.Add("style", "background-color:ActiveBorder; color:black; font-size:Larger;")
                        .Items.Add(roleItem)
                    Else
                        roleItem.Attributes.Add("style", "background-color:ActiveBorder; color:black;")
                        roleItem.Attributes.Add("disabled", "true")
                        .Items.Add(roleItem)
                    End If


                End If
                Dim useritem As New ListItem
                With useritem

                    .Text = Server.HtmlDecode(spaceChar & IP.Bids.SharedFunctions.ToTitleCase(item.FullName.ToLower))
                    .Value = item.UserName
                End With
                .Items.Add(useritem)
            End With
        Next
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sc As ScriptManager
        Dim siteItem As New TaskTrackerSiteBll
        Dim facilityList As System.Collections.Generic.List(Of Facility) = siteItem.GetFacility("", "")
        sc = CType(Page.Form.FindControl("_scriptManager"), ScriptManager)
        If sc IsNot Nothing Then
            Dim loService As New ServiceReference
            loService.InlineScript = False
            loService.Path = "~/WebServices/SiteDropDownsWS.asmx"
            sc.Services.Add(loService)
        End If
        Dim returnClientID As String = String.Empty
        If Request.QueryString("Ret") IsNot Nothing Then
            returnClientID = Request.QueryString("Ret")
            ' Me._ddlUsers2.Attributes.Add("onChange", "setReturnValue(this.options[this.selectedIndex].text,'" & returnClientID & "');")
        End If

        PopulateTreeRoot()
        For Each item As Facility In facilityList
            With Me._ddlResponsibleFacility
                .Items.Add(New ListItem(item.SiteName, item.PlantCode))
            End With
        Next

        'Me._scriptManager.Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/JQuery UI/js/jquery-1.4.2.min.js")))
        'Me._scriptManager.Scripts.Add(New ScriptReference(Page.ResolveUrl("~/scripts/Source/ResponsibleUsers.js")))
    End Sub
End Class
