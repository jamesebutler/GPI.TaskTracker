'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-07-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports Microsoft.VisualBasic
Imports System.ComponentModel
Imports System.Web.UI.WebControls
Imports System.Web.UI

Namespace IP.Bids.UserControl
    Public Class UserControlValidation
        Inherits System.Web.UI.UserControl

#Region "Fields"
        Dim mErrorMessage As String = String.Empty
        Private mClientChange As String = String.Empty
        Private mUserControlCustomValidation As UserControlCustomValidation = Nothing
        Private mControlToValidate As String = String.Empty
        Private mEnableValidation As Boolean ' = False
        Private mClientValidationFunction As String = String.Empty
#End Region

#Region "Controls"
        <EditorBrowsable(EditorBrowsableState.Never), Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
        Private WithEvents mValidation As CustomValidator
#End Region

#Region "Properties"
        ''' <summary>
        ''' Gets or sets the javascript that will be executed on the change event
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OnClientChange() As String
            Get
                Return mClientChange
            End Get
            Set(ByVal value As String)
                mClientChange = value
            End Set
        End Property

        <Category("Behavior"), MergableProperty(False), DefaultValue(CStr(Nothing)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty)> _
        Public ReadOnly Property Validation() As UserControlCustomValidation
            Get
                If mUserControlCustomValidation Is Nothing Then mUserControlCustomValidation = New UserControlCustomValidation
                Return mUserControlCustomValidation
            End Get
        End Property

        Public Property EnableValidation() As Boolean
            Get
                Return mEnableValidation
            End Get
            Set(ByVal value As Boolean)
                mEnableValidation = value
            End Set
        End Property


        Public Property ClientValidationFunction() As String
            Get
                Return mClientValidationFunction
            End Get
            Set(ByVal value As String)
                mClientValidationFunction = value
            End Set
        End Property

        <EditorBrowsable(EditorBrowsableState.Never), Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
        Public Property ControlToValidate() As String
            Get
                Return mControlToValidate
            End Get
            Set(ByVal value As String)
                mControlToValidate = value
            End Set
        End Property
#End Region

#Region "Private Events"
        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If mUserControlCustomValidation IsNot Nothing AndAlso EnableValidation = True Then
                If mValidation Is Nothing Then
                    mValidation = New CustomValidator
                End If
                Me.mValidation.ID = "validator_" & Me.ID
                Me.Controls.Add(mValidation)
                Me.mValidation.ErrorMessage = mUserControlCustomValidation.ValidationErrorMessage
                Me.mValidation.ValidationGroup = mUserControlCustomValidation.ValidationGroup
                Me.mValidation.EnableClientScript = True
                Me.mValidation.ControlToValidate = ControlToValidate
                Me.mValidation.Display = ValidatorDisplay.Dynamic
                Me.mValidation.Text = mUserControlCustomValidation.ValidationErrorText
                Me.mValidation.ClientValidationFunction = ClientValidationFunction
                Me.mValidation.ValidateEmptyText = mUserControlCustomValidation.ValidateEmptyText
                Me.mValidation.Enabled = True

            ElseIf mUserControlCustomValidation IsNot Nothing AndAlso EnableValidation = False Then
                If mValidation Is Nothing Then
                    mValidation = New CustomValidator
                End If
                Me.mValidation.Enabled = False
            End If
        End Sub
#End Region

#Region "Public Methods"
        ''' <summary>
        ''' Adds an attribute to a server control's System.Web.UI.AttributeCollection object.
        ''' </summary>
        ''' <param name="obj">WebControl - The control that the attribute will be added to</param>
        ''' <param name="attribute">String - The attribute that will be added (i.e onchange, onclick)</param>
        ''' <param name="value">String - the javascript that will be added to the control's attribute</param>
        ''' <remarks></remarks>
        Public Sub AppendAttribute(ByRef obj As WebControl, ByVal attribute As String, ByVal value As String)
            Try
                If obj IsNot Nothing AndAlso TypeOf obj Is WebControl Then

                    Dim currentAttributes As String = String.Empty
                    If obj.Attributes.Item(attribute) IsNot Nothing Then
                        currentAttributes = Trim(obj.Attributes.Item(attribute))
                        If currentAttributes.Substring(currentAttributes.Length - 1) <> ";" Then
                            currentAttributes = currentAttributes & ";"
                        End If
                    End If
                    currentAttributes = currentAttributes & value
                    obj.Attributes.Add(attribute, currentAttributes)
                End If
            Catch
                Throw
            End Try

        End Sub
#End Region
    End Class



End Namespace
