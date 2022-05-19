'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 10-28-2010
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class UserControlsSite
    Inherits System.Web.UI.UserControl
#Region "Fields"
    Private _ValidationGroup As String = String.Empty
    Private _Enabled As Boolean = True
    Private _DisableDivision As Boolean
    Private _DisableRegion As Boolean
#End Region

#Region "Properties"
    Public Property DisableRegion() As Boolean
        Get
            Return _DisableRegion
        End Get
        Set
            _DisableRegion = Value
        End Set
    End Property

    Public Property DisableDivision() As Boolean
        Get
            Return _DisableDivision
        End Get
        Set
            _DisableDivision = Value
        End Set
    End Property

    Public Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
        End Set
    End Property
    Public Property DefaultDivision() As String
        Get
            Return Me._cddlBusiness.SelectedValue
        End Get
        Set(ByVal value As String)
            Me._cddlBusiness.SelectedValue = value
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

    Public Property DefaultBusinessUnitArea() As String
        Get
            Return Me._cddlBusArea.SelectedValue
        End Get
        Set(ByVal value As String)
            _cddlBusArea.SelectedValue = value
        End Set
    End Property

    Public ReadOnly Property BusinessUnitArea() As String
        Get
            Return ParseText(DefaultBusinessUnitArea)
        End Get
    End Property

    Public ReadOnly Property BusinessUnitAreaValue() As String
        Get
            Return ParseValue(DefaultBusinessUnitArea)
        End Get
    End Property

    Public Property DefaultLineBreak() As String
        Get
            Return Me._cddlLineLineBreak.SelectedValue
        End Get
        Set(ByVal value As String)
            _cddlLineLineBreak.SelectedValue = value
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


    Public Property ValidationGroup() As String
        Get
            Return _ValidationGroup
        End Get
        Set(ByVal value As String)
            _ValidationGroup = value
        End Set
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
        If parsedRegion.Length >= 2 Then
            Return parsedRegion(0)
        Else
            Return ""
        End If
    End Function

'  COMMENTED BY CODEIT.RIGHT
'    Private Sub PopulateLists()
'
'    End Sub
#End Region

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If ValidationGroup.Length > 0 Then
            Me._ddlBusArea.ValidationGroup = ValidationGroup
            Me._rfvBusinessUnit.ValidationGroup = ValidationGroup
            Me._rfvFacility.ValidationGroup = ValidationGroup
            Me._ddlFacility.ValidationGroup = ValidationGroup
        End If

        '_pnlEntrySite.Enabled = Enabled


        'Disable Business, Region, Facility

        If DisableDivision = True Then
            _cddlBusiness.Enabled = False
            _ddlDivision.Enabled = False
        End If

        If DisableRegion = True Then
            _cddlRegion.Enabled = False
            _cddlFacility.ParentControlID = ""
            _ddlRegion.Enabled = False
        End If

    End Sub
End Class
