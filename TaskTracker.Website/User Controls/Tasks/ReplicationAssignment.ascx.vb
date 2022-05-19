
Partial Class User_Controls_Tasks_ReplicationAssignment
    Inherits System.Web.UI.UserControl

#Region "Fields"
    'Used to store the selected facilities
    Private _selectedValue As String
    Private _templateNumber As String
    Private _webServerName As String
#End Region

#Region "Properties"
    ''' <summary>
    ''' Gets or Sets the value of the selected facilities.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectedValue() As String
        Get
            Return _selectedValue
        End Get
        Set(ByVal value As String)
            _selectedValue = value
        End Set
    End Property

    Public ReadOnly Property TemplateNumber() As String
        Get
            Return _templateNumber
        End Get
    End Property
#End Region

#Region "Methods"
    Public Sub PopulateTree()
        PopulateTree("-1")
    End Sub
    ''' <summary>
    ''' Populates the Business, Region and Site Treeview
    ''' </summary>
    ''' <param name="TemplateNumber">String - Represents the Task Header Number for a template header</param>
    ''' <remarks></remarks>
    Public Sub PopulateTree(ByVal TemplateNumber As String)
        Dim selectedItems As String = String.Empty

        Dim siteList As System.Collections.Generic.List(Of BusinessRegionSite) = Nothing
        Dim replicatedSiteList As New System.Collections.Generic.List(Of BusinessRegionSite)
        Dim siteNode As TreeNode = Nothing
        Dim regionNode As TreeNode = Nothing
        Dim businessNode As TreeNode = Nothing

        Dim HeaderBusinessNode As TreeNode = Nothing
        Dim HeaderRegionNode As TreeNode = Nothing
        Dim taskSite As New TaskTrackerSiteBll ' COMMENTED BY CODEIT.RIGHT

        Dim replicatedSites As New StringBuilder

        ''CloseMe
        'If Request.QueryString("CloseMe") IsNot Nothing Then
        '    Dim closeMe As String = Request.QueryString("CloseMe")
        'End If
        _templateNumber = TemplateNumber

        Dim PlantModel As New Data.DataTable
        PlantModel.Columns.Add("PlantModelID")
        Dim dr As Data.DataRow = PlantModel.NewRow
        dr.Item(0) = 1
        PlantModel.Rows.Add(dr)
        selectedItems = Me._tvPlantModel.SelectedItems
        With Me._tvPlantModel
            .RepeatColumns = 1
            .Width = Unit.Percentage(90)
            .RepeatLayout = RepeatLayout.Table
            .ShowHiddenFields = False
            .DataSource = PlantModel
            .DataKeyField = "PlantModelID"
            ' .AllowCheckAll = True
            .GroupingText = "" 'IP.Bids.SharedFunctions.LocalizeValue("Available Facilities", True) '"Plant Model "
            '.ExpandAllText = IP.Bids.SharedFunctions.LocalizeValue("Expand All", True)
            '.CollapseAllText = IP.Bids.SharedFunctions.LocalizeValue("Collapse All", True)
            .DataBind()

            .ScrollBars = ScrollBars.Auto
            'HeaderBusinessNode = New TreeNode("Business", "Business")
            'HeaderBusinessNode.ShowCheckBox = False
            '.AddChildNode(1, HeaderBusinessNode)
          
        End With

        Dim currentBusiness As String = String.Empty
        Dim currentRegion As String = String.Empty
        Dim currentSite As String = String.Empty

        HeaderBusinessNode = New TreeNode
        With HeaderBusinessNode
            .Text = IP.Bids.SharedFunctions.LocalizeValue("Available Facilities", True)
            .Value = "Available Sites"
            .SelectAction = TreeNodeSelectAction.Expand
        End With
        siteList = taskSite.GetBusinessRegionFacility(TemplateNumber)
        If siteList IsNot Nothing AndAlso siteList.Count > 0 Then
            For i As Integer = 0 To siteList.Count - 1



                'Level 1 - Business
                If currentBusiness <> siteList(i).BusinessName Then
                    'Business has changed
                    If businessNode IsNot Nothing Then
                        Dim showCheckBox As Boolean
                        For Each item As TreeNode In businessNode.ChildNodes
                            If item.ShowCheckBox.Value = True Then
                                showCheckBox = True
                            End If
                        Next
                        businessNode.ShowCheckBox = showCheckBox
                        HeaderBusinessNode.ChildNodes.Add(businessNode)
                    End If

                    currentBusiness = siteList(i).BusinessName
                    businessNode = New TreeNode
                    With businessNode
                        .Text = siteList(i).BusinessName
                        .Value = siteList(i).BusinessName
                        'If siteList(i).ProcessedFlag.Length = 0 Then 'record has not been processed
                        .ShowCheckBox = False
                        'End If
                        '.ShowCheckBox = False
                        .SelectAction = TreeNodeSelectAction.Expand
                    End With
                End If

                'Level 2 - Region
                If currentBusiness & currentRegion <> siteList(i).BusinessName & siteList(i).RegionName Then
                    'Region has changed

                    'HeaderRegionNode = New TreeNode("Region", "Region")
                    'HeaderRegionNode.ShowCheckBox = False

                    currentRegion = siteList(i).RegionName
                    regionNode = New TreeNode
                    With regionNode
                        .Text = siteList(i).RegionName
                        .Value = siteList(i).RegionName
                        'If siteList(i).ProcessedFlag.Length = 0 Then 'record has not been processed
                        .ShowCheckBox = False
                        'End If
                        '.ShowCheckBox = False
                        .SelectAction = TreeNodeSelectAction.Expand
                    End With

                    If regionNode IsNot Nothing AndAlso businessNode IsNot Nothing Then
                        'HeaderRegionNode.ChildNodes.Add(regionNode)
                        'Dim showCheckBox As Boolean
                        'For Each item As TreeNode In regionNode.ChildNodes
                        '    If item.ShowCheckBox.Value = True Then
                        '        showCheckBox = True
                        '    End If
                        'Next
                        'regionNode.ShowCheckBox = showCheckBox
                        businessNode.ChildNodes.Add(regionNode)
                        'businessNode.ChildNodes.Add(regionNode)

                    End If
                End If

                'Level 3


                siteNode = New TreeNode
                With siteNode


                    If siteList(i).ProcessedFlag.ToUpper = "Y" Then
                        .ShowCheckBox = False
                        .SelectAction = TreeNodeSelectAction.None
                        .Text = siteList(i).SiteName & " " & IP.Bids.SharedFunctions.LocalizeValue("(Replicated)", True)
                        .Value = siteList(i).PlantCode                        
                        replicatedSiteList.Add(New BusinessRegionSite(siteList(i).BusinessName, siteList(i).RegionName, siteList(i).SiteName, siteList(i).PlantCode, siteList(i).ProcessedFlag))
                        If replicatedSites.Length > 0 Then
                            replicatedSites.Append(",")
                            replicatedSites.Append(siteList(i).SiteName)
                        Else
                            replicatedSites.Append(siteList(i).SiteName)
                        End If
                    Else
                        .ShowCheckBox = True
                        .SelectAction = TreeNodeSelectAction.Expand
                        .Text = siteList(i).SiteName
                        .Value = siteList(i).PlantCode
                    End If
                End With

                If regionNode IsNot Nothing Then
                    If siteNode.ShowCheckBox.Value = True Then
                        regionNode.ShowCheckBox = True
                    End If
                    regionNode.ChildNodes.Add(siteNode)
                End If

            Next

            If businessNode IsNot Nothing Then
                '_tvFacilities.Nodes.Add(businessNode)
                'If regionNode IsNot Nothing Then
                '    businessNode.ChildNodes.Add(regionNode)
                'End If
                Dim showCheckBox As Boolean
                For Each item As TreeNode In businessNode.ChildNodes
                    If item.ShowCheckBox.Value = True Then
                        showCheckBox = True
                    End If
                Next
                businessNode.ShowCheckBox = showCheckBox
                HeaderBusinessNode.ChildNodes.Add(businessNode)

            End If
            _tvPlantModel.AddChildNode(1, HeaderBusinessNode)
            ' _tvFacilities.AddChildNode(currentBusiness, businessNode)
            _tvPlantModel.ExpandAll()
            ' _tvRoles.RecheckSelected()
            _tvPlantModel.CollapseAll()

            If Page.IsPostBack Then
                Me._tvPlantModel.SelectedItems = selectedItems
                Me._tvPlantModel.RecheckSelected()
            End If

            If replicatedSiteList IsNot Nothing AndAlso replicatedSiteList.Count > 0 Then
                PopulateReplicatedListTree(replicatedSiteList)
            Else
                Me._tvReplicatedSites.Visible = False
            End If
        End If
    End Sub

    Private Sub PopulateReplicatedListTree(ByVal replicatedTreeList As System.Collections.Generic.List(Of BusinessRegionSite))
        Dim selectedItems As String = String.Empty

        Dim siteNode As TreeNode = Nothing
        Dim regionNode As TreeNode = Nothing
        Dim businessNode As TreeNode = Nothing

        Dim HeaderBusinessNode As TreeNode = Nothing
        Dim HeaderRegionNode As TreeNode = Nothing
        Dim taskSite As New TaskTrackerSiteBll ' COMMENTED BY CODEIT.RIGHT

        Dim replicatedSites As New StringBuilder



        Dim PlantModel As New Data.DataTable
        PlantModel.Columns.Add("PlantModelID")
        Dim dr As Data.DataRow = PlantModel.NewRow
        dr.Item(0) = 2
        PlantModel.Rows.Add(dr)
        selectedItems = Me._tvPlantModel.SelectedItems
        With Me._tvReplicatedSites
            .RepeatColumns = 1
            .Width = Unit.Percentage(90)
            .RepeatLayout = RepeatLayout.Table
            .ShowHiddenFields = False
            .DataSource = PlantModel
            .DataKeyField = "PlantModelID"
            .AllowCheckAll = False
            .GroupingText = "" 'IP.Bids.SharedFunctions.LocalizeValue("Replicated Sites", True) '"Plant Model "
            .ExpandAllText = IP.Bids.SharedFunctions.LocalizeValue("Expand All", True)
            .CollapseAllText = IP.Bids.SharedFunctions.LocalizeValue("Collapse All", True)
            .DataBind()
            .ScrollBars = ScrollBars.Auto
            'HeaderBusinessNode = New TreeNode("Business", "Business")
            'HeaderBusinessNode.ShowCheckBox = False
            '.AddChildNode(1, HeaderBusinessNode)
           
        End With

        Dim currentBusiness As String = String.Empty
        Dim currentRegion As String = String.Empty
        Dim currentSite As String = String.Empty

        HeaderBusinessNode = New TreeNode
        With HeaderBusinessNode
            .Text = IP.Bids.SharedFunctions.LocalizeValue("Replicated Sites", True)
            .Value = "Replicated Sites"
            .ShowCheckBox = False
            .SelectAction = TreeNodeSelectAction.Expand
        End With

        If replicatedTreeList IsNot Nothing AndAlso replicatedTreeList.Count > 0 Then
            For i As Integer = 0 To replicatedTreeList.Count - 1



                'Level 1 - Business
                If currentBusiness <> replicatedTreeList(i).BusinessName Then
                    'Business has changed
                    If businessNode IsNot Nothing Then
                        HeaderBusinessNode.ChildNodes.Add(businessNode)
                    End If

                    currentBusiness = replicatedTreeList(i).BusinessName
                    businessNode = New TreeNode
                    With businessNode
                        .Text = replicatedTreeList(i).BusinessName
                        .Value = replicatedTreeList(i).BusinessName
                        .ShowCheckBox = False
                        .SelectAction = TreeNodeSelectAction.Expand
                    End With
                End If

                'Level 2 - Region
                If currentRegion <> replicatedTreeList(i).RegionName Then
                    'Region has changed

                    'HeaderRegionNode = New TreeNode("Region", "Region")
                    'HeaderRegionNode.ShowCheckBox = False

                    currentRegion = replicatedTreeList(i).RegionName
                    regionNode = New TreeNode
                    With regionNode
                        .Text = replicatedTreeList(i).RegionName
                        .Value = replicatedTreeList(i).RegionName
                        .ShowCheckBox = False
                        .SelectAction = TreeNodeSelectAction.Expand
                    End With

                    If regionNode IsNot Nothing AndAlso businessNode IsNot Nothing Then
                        'HeaderRegionNode.ChildNodes.Add(regionNode)
                        businessNode.ChildNodes.Add(regionNode)
                        'businessNode.ChildNodes.Add(regionNode)

                    End If
                End If

                'Level 3


                siteNode = New TreeNode
                With siteNode
                    .ShowCheckBox = False
                    .SelectAction = TreeNodeSelectAction.Expand
                    .Text = replicatedTreeList(i).SiteName
                    .Value = replicatedTreeList(i).PlantCode
                End With

                If regionNode IsNot Nothing Then
                    regionNode.ChildNodes.Add(siteNode)
                End If

            Next

            If businessNode IsNot Nothing Then
                '_tvFacilities.Nodes.Add(businessNode)
                'If regionNode IsNot Nothing Then
                '    businessNode.ChildNodes.Add(regionNode)
                'End If
                HeaderBusinessNode.ChildNodes.Add(businessNode)

            End If
            _tvReplicatedSites.AddChildNode(2, HeaderBusinessNode)
            ' _tvFacilities.AddChildNode(currentBusiness, businessNode)
            _tvReplicatedSites.ExpandAll()
            ' _tvRoles.RecheckSelected()
            _tvReplicatedSites.CollapseAll()

            If Page.IsPostBack Then
                Me._tvReplicatedSites.SelectedItems = selectedItems
                Me._tvReplicatedSites.RecheckSelected()
            End If

        End If
    End Sub
#End Region

   
    Public Sub ProcessReplicaiton()
        Dim selectedSitesToReplicate As String
        Dim ta As New TaskHeaderDALTableAdapters.QueriesTableAdapter




        Dim sbSelectedList As New StringBuilder
        Dim selectedList As OrderedDictionary = _tvPlantModel.GetSelectedNodes(False)
        Dim sources As New Collections.ArrayList
        Dim outStatus As Decimal
        For Each source As Collections.DictionaryEntry In selectedList
            Dim sourceGroupings As String() = source.Key.ToString.Split(CChar("_"))
            For sourceGroupingsIndex As Integer = 0 To UBound(sourceGroupings)
                If Not sources.Contains(sourceGroupings(sourceGroupingsIndex)) Then
                    sources.Add(sourceGroupings(sourceGroupingsIndex))
                    ta.AddReplicationRequest(TemplateNumber, sourceGroupings(sourceGroupingsIndex), IP.Bids.SharedFunctions.GetCurrentUser.Username, outStatus)
                    If outStatus <> 0 Then 'Error Adding Replication request
                        IP.Bids.SharedFunctions.InsertAuditRecord("MTT:AddReplicationRequest", "Error calling AddReplicationRequest for [Template Number:" & TemplateNumber & ", Plantcode:" & sourceGroupings(sourceGroupingsIndex) & ", Error Status:" & outStatus.ToString)
                    End If
                End If
            Next
        Next

        With _msgProcessReplication
            .Title = IP.Bids.SharedFunctions.LocalizeValue("Pending Replication", True)
            .Message = IP.Bids.SharedFunctions.LocalizeValue("Your request for replication has been submitted", True)
            .ShowMessage()
            If Request.QueryString("CloseMe") IsNot Nothing Then
                ' Me._btnProcessReplicaitonTop.OnClientClick = "document.getElementById('" & Request.QueryString("CloseMe") & "').click()"
                'ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "CloseMe", "try{parent.document.getElementById('" & Request.QueryString("CloseMe") & "').click();}catch(e){alert(e);}", True)
                .OKScript = "try{parent.document.getElementById('" & Request.QueryString("CloseMe") & "').click();}catch(e){alert(e);}"
            End If
        End With
        ' create a delegate of MethodInvoker poiting
        ' to our Foo() function.

        selectedSitesToReplicate = Me._tvPlantModel.SelectedItems


        'Dim simpleDelegate As New Windows.Forms.MethodInvoker(AddressOf CallProcessReplication)
        Dim simpleDelegate As New Threading.Thread(New Threading.ThreadStart(AddressOf CallProcessReplication))

        ' Calling Foo
        'simpleDelegate.BeginInvoke(Nothing, Nothing)
        simpleDelegate.Start()
        PopulateTree(TemplateNumber)

    End Sub

    Private Sub CallProcessReplication()
        Dim r As New Replication
        r.ProcessReplication(TemplateNumber, _webServerName)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _webServerName = IP.Bids.SharedFunctions.GetServerName       
    End Sub

End Class
