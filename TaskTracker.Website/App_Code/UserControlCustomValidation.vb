'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 09-09-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-09-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports Microsoft.VisualBasic
Imports System.ComponentModel
Imports System.Web.UI.WebControls
Imports System.Web.UI

Namespace IP.Bids.UserControl
    ''' <summary>
    ''' Contains the validation properties that are supported by this user control class.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UserControlCustomValidation
        Implements System.Web.UI.INamingContainer

        Private mErrorMessage As String = String.Empty
        Private mValidationGroup As String = String.Empty
        Private mValidateEmptyText As Boolean ' = False
        Private mErrorText As String = String.Empty

        ''' <summary>
        ''' Gets or sets the text for the error message displayed when validation fails
        ''' </summary>
        Public Property ValidationErrorMessage() As String
            Get
                Return mErrorMessage
            End Get
            Set(ByVal value As String)
                mErrorMessage = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the text for the error message displayed when validation fails
        ''' </summary>
        Public Property ValidationErrorText() As String
            Get
                Return mErrorText
            End Get
            Set(ByVal value As String)
                mErrorText = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the name of the validation group to which this validation control belongs.
        ''' </summary>
        Public Property ValidationGroup() As String
            Get
                Return mValidationGroup
            End Get
            Set(ByVal value As String)
                mValidationGroup = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a Boolean value indicating whether empty text should be validated.
        ''' </summary>
        Public Property ValidateEmptyText() As Boolean
            Get
                Return mValidateEmptyText
            End Get
            Set(ByVal value As Boolean)
                mValidateEmptyText = value
            End Set

        End Property
    End Class
End Namespace
