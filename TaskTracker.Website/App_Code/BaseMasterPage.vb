Imports System.Globalization

Public MustInherit Class BaseMasterPage
    Inherits MasterPage

#Region "Fields"
    Private _IPResources As New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization 'Holds an instance of the Localization module
    Private _currentUser As IP.Bids.UserInfo
#End Region

#Region "properties"
    ''' <summary>
    ''' Gets or sets the name of the page.
    ''' </summary>
    ''' <value>The name of the page.</value>
    Public Property PageName() As String = String.Empty

    ''' <summary>
    ''' Gets an instance of the Localization module
    ''' </summary> 
    Public ReadOnly Property IPResources() As IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization
        Get
            Return _IPResources
        End Get
    End Property

    ''' <summary>
    ''' Gets an instance of the current user info object
    ''' </summary>
    Public ReadOnly Property CurrentUser() As IP.Bids.UserInfo
        Get
            If _currentUser Is Nothing Then _currentUser = IP.Bids.SharedFunctions.GetCurrentUser
            Return _currentUser
        End Get
    End Property

    ''' <summary>
    ''' Gets a boolean that indicates whether the current user has a support role
    ''' </summary>
    Public Property HasSupportRole() As Boolean

    ''' <summary>
    ''' Gets the name of the External or Reference Application
    ''' </summary>
    Public Property RefSite() As String = "MTT"
#End Region

#Region "Public Methods"
    Public MustOverride Sub HideFooter()
    Public MustOverride Sub HideHeaderAndMenu()
    Public MustOverride Sub HideBannerFooter()

    Public Sub SetCurrentUser(ByVal currentUser As IP.Bids.UserInfo)
        _currentUser = currentUser
    End Sub
    ''' <summary>
    ''' Gets the localized value for the specified value
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="returnKeyNotFound"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLocalizedValue(ByVal value As String, ByVal returnKeyNotFound As Boolean) As String
        Dim returnValue As String = value
        Try
            If IPResources Is Nothing Then
                _IPResources = New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization
            End If
            If IPResources IsNot Nothing Then
                returnValue = _IPResources.GetResourceValue(value, returnKeyNotFound)
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetLocalizedValue", "Error attempting to get localized value for [" & value & "]", ex)
        End Try
        Return returnValue
    End Function
#End Region



End Class
