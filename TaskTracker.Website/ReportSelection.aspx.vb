'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 12-07-2010
'
' Last Modified By : MJPOPE
' Last Modified On : 04-27-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class ReportSelection
    Inherits IP.Bids.BasePage

#Region "Enums"
    'Private Enum ReportType As Integer
    '    Site
    '    ReportType
    'End Enum
#End Region

#Region "Controls"
    'Private _rblReportType As New RadioButtonList
#End Region

    ' Private selectionTable As New System.Collections.Generic.List(Of ReportSelectionBLL.ReportSelectionTable)

    ' ReportSelectionBLL.ReportselectionTable
#Region "Private Methods"
    ''' <summary>
    ''' Purpose is to handle all of the tasks that should occur when this page is loaded
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub HandlePageLoad()
        Dim refSite As String = String.Empty
        If Request.QueryString("RefSite") IsNot Nothing Then
            refSite = Request.QueryString("RefSite")
        End If
        Me._taskSearch.ScreenHeight = Me.ScreenHeight
        Me._taskSearch.ScreenWidth = Me.ScreenWidth
        Me._taskSearch.RefSite = refSite
        If Request.QueryString("RN") IsNot Nothing Then
            ' Master.HideBannerFooter()
        End If
        Master.SetActiveNav(Enums.NavPages.Reports.ToString)
        Master.PageName = Master.GetLocalizedValue("Reports", False)
    End Sub

#End Region

#Region "Events"
    ''' <summary>
    ''' Handles the Page Init event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Master.PageName = Master.GetLocalizedValue("MTT Reporting", False)
    End Sub

    ''' <summary>
    ''' Handles the Page Load event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        HandlePageLoad()
    End Sub
#End Region

End Class
