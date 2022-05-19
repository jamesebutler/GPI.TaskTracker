'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 11-08-2010
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class UserControlsReportingSite
    Inherits System.Web.UI.UserControl
#Region "Fields"

#End Region

#Region "Properties"
    Public Property DisableRegion() As Boolean

    Public Property DisableDivision() As Boolean
    
    Public Property DefaultDivision() As String
        Get
            Return Me._cddlBusiness.SelectedValue
        End Get
        Set(ByVal value As String)
            Me._cddlBusiness.SelectedValue = value
            Me._cddlBusiness.ContextKey = value
        End Set
    End Property

    Public ReadOnly Property Division() As String
        Get
            Return ParseText(DefaultDivision)
        End Get
    End Property

    Public ReadOnly Property DivisionValue() As String
        Get
            Return ParseValue(DefaultDivision)
        End Get
    End Property

    Public Property DefaultRegion() As String
        Get
            Return Me._cddlRegion.SelectedValue
        End Get
        Set(ByVal value As String)
            _cddlRegion.SelectedValue = value
            _cddlRegion.ContextKey = value
        End Set
    End Property

    Public ReadOnly Property Region() As String
        Get
            Return ParseText(DefaultRegion)
        End Get
    End Property

    Public ReadOnly Property RegionValue() As String
        Get
            Return ParseValue(DefaultRegion)
        End Get
    End Property

    Public Property DefaultFacility() As String
        Get
            Return Me._cddlFacility.SelectedValue
        End Get
        Set(ByVal value As String)
            _cddlFacility.SelectedValue = value
            _cddlFacility.ContextKey = value
        End Set
    End Property

    Public ReadOnly Property Facility() As String
        Get
            Return ParseText(DefaultFacility)
        End Get
    End Property

    Public ReadOnly Property FacilityValue() As String
        Get
            Return ParseValue(DefaultFacility)
        End Get
    End Property

    Public Property DefaultBusinessUnit() As String
        Get
            Return Me._cddlBusinessUnit.SelectedValue
        End Get
        Set(ByVal value As String)
            _cddlBusinessUnit.SelectedValue = value
            _cddlBusinessUnit.ContextKey = value
        End Set
    End Property

    Public ReadOnly Property BusinessUnit() As String
        Get
            Return ParseText(DefaultBusinessUnit)
        End Get
    End Property

    Public ReadOnly Property BusinessUnitValue() As String
        Get
            Return ParseValue(DefaultBusinessUnit)
        End Get
    End Property

    Public Property DefaultArea() As String
        Get
            Return Me._cddlArea.SelectedValue
        End Get
        Set(ByVal value As String)
            _cddlArea.SelectedValue = value
            _cddlArea.ContextKey = value
        End Set
    End Property

    Public ReadOnly Property Area() As String
        Get
            Return ParseText(DefaultArea)
        End Get
    End Property

    Public ReadOnly Property AreaValue() As String
        Get
            Return ParseValue(DefaultArea)
        End Get
    End Property

    Public Property DefaultLine() As String
        Get
            Return Me._cddlLine.SelectedValue
        End Get
        Set(ByVal value As String)
            _cddlLine.SelectedValue = value
            _cddlLine.ContextKey = value
        End Set
    End Property

    Public ReadOnly Property Line() As String
        Get
            Return ParseText(DefaultLine)
        End Get
    End Property

    Public ReadOnly Property LineValue() As String
        Get
            Return ParseValue(DefaultLine)
        End Get
    End Property

    Public Property DefaultLineBreak() As String
        Get
            Return Me._cddlLineBreak.SelectedValue
        End Get
        Set(ByVal value As String)
            _cddlLineBreak.SelectedValue = value
            _cddlLineBreak.ContextKey = value
        End Set
    End Property

    Public ReadOnly Property LineBreak() As String
        Get
            Return ParseText(DefaultLineBreak)
        End Get
    End Property

    Public ReadOnly Property LineBreakValue() As String
        Get
            Return ParseValue(DefaultLineBreak)
        End Get
    End Property
#End Region

#Region "Methods"
    Private Function ParseText(ByVal value As String) As String
        Dim parsedRegion As String()
        parsedRegion = value.Split(New String() {":::"}, StringSplitOptions.None)
        If parsedRegion.Length >= 2 Then
            Return parsedRegion(1)
        Else
            Return ""
        End If
    End Function

    Private Function ParseValue(ByVal value As String) As String
        Dim parsedRegion As String()
        parsedRegion = value.Split(New String() {":::"}, StringSplitOptions.None)
        'If parsedRegion.Length >= 2 Then
        Return parsedRegion(0)
        'Else
        'Return ""
        'End If
    End Function
#End Region

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Refresh()
    End Sub

    Public Sub Refresh()
        If DisableDivision = True Then
            _cddlBusiness.Enabled = False
            _ddlDivision.Enabled = False
            _lblDivision.Enabled = False
        End If

        If DisableRegion = True Then
            _cddlRegion.Enabled = False
            _cddlFacility.ParentControlID = ""
            _ddlRegion.Enabled = False
            _lblRegion.Enabled = False
        End If
        _cddlFacility.ContextKey = DefaultFacility
        _cddlArea.ContextKey = DefaultArea
        _cddlBusiness.ContextKey = DefaultDivision
        _cddlBusinessUnit.ContextKey = DefaultBusinessUnit
        _cddlLine.ContextKey = DefaultLine
        _cddlLineBreak.ContextKey = DefaultLineBreak
        _cddlRegion.ContextKey = DefaultRegion

    End Sub
End Class
