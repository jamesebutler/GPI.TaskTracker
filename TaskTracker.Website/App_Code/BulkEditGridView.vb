Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.ComponentModel
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Web


Namespace RealWorld.Grids
    ''' <summary>
    ''' BulkEditGridView allows users to edit multiple rows of a gridview at once, and have them
    ''' all saved.
    ''' </summary>
    <DefaultEvent("SelectedIndexChanged"), Designer("System.Web.UI.Design.WebControls.GridViewDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), ControlValueProperty("SelectedValue")> _
    Public Class BulkEditGridView : Inherits System.Web.UI.WebControls.GridView
        'key for the RowInserting event handler list
        Public Shared ReadOnly RowInsertingEvent As Object = New Object()

        Private dirtyRows_Renamed As List(Of Integer) = New List(Of Integer)()
        Private newRows As List(Of Integer) = New List(Of Integer)()

        ''' <summary>
        ''' Default Constructor
        ''' </summary>
        Public Sub BulkEditGridView()
            Me.HeaderRow.TableSection = TableRowSection.TableHeader
        End Sub

        ''' <summary>
        ''' Modifies the creation of the row to set all rows as editable.
        ''' </summary>
        ''' <param name="rowIndex"></param>
        ''' <param name="dataSourceIndex"></param>
        ''' <param name="rowType"></param>
        ''' <param name="rowState"></param>
        ''' <returns></returns>
        Protected Overrides Function CreateRow(ByVal rowIndex As Integer, ByVal dataSourceIndex As Integer, ByVal rowType As DataControlRowType, ByVal rowState As DataControlRowState) As GridViewRow
            Return MyBase.CreateRow(rowIndex, dataSourceIndex, rowType, rowState Or DataControlRowState.Edit)
        End Function

        Public ReadOnly Property DirtyRows() As List(Of GridViewRow)
            Get
                Dim drs As List(Of GridViewRow) = New List(Of GridViewRow)()
                For Each rowIndex As Integer In dirtyRows_Renamed
                    drs.Add(Me.Rows(rowIndex))
                Next rowIndex

                Return drs
            End Get
        End Property

        ''' <summary>
        ''' Adds event handlers to controls in all the editable cells.
        ''' </summary>
        ''' <param name="row"></param>
        ''' <param name="fields"></param>
        Protected Overrides Sub InitializeRow(ByVal row As GridViewRow, ByVal fields As DataControlField())
            MyBase.InitializeRow(row, fields)
            For Each cell As TableCell In row.Cells
                If cell.Controls.Count > 0 Then
                    AddChangedHandlers(cell.Controls)
                End If
            Next cell
        End Sub

        ''' <summary>
        ''' Adds an event handler to editable controls.
        ''' </summary>
        ''' <param name="controls"></param>
        Private Sub AddChangedHandlers(ByVal controls As ControlCollection)
            For Each ctrl As Control In controls
                If TypeOf ctrl Is TextBox Then
                    AddHandler (CType(ctrl, TextBox)).TextChanged, AddressOf HandleRowChanged                    
                ElseIf TypeOf ctrl Is CheckBox Then
                    AddHandler (CType(ctrl, CheckBox)).CheckedChanged, AddressOf HandleRowChanged
                ElseIf TypeOf ctrl Is DropDownList Then
                    AddHandler (CType(ctrl, DropDownList)).SelectedIndexChanged, AddressOf HandleRowChanged
                ElseIf TypeOf ctrl Is AdvancedTextBox.AdvancedTextBox Then
                    AddHandler (CType(ctrl, AdvancedTextBox.AdvancedTextBox)).TextChanged, AddressOf HandleRowChanged                    
                ElseIf ctrl.GetType.ToString = "ASP.user_controls_advancedemployeelistdropdown_ascx" Then
                    AddChangedHandlers(ctrl.Controls)
                ElseIf ctrl.GetType.ToString = "ASP.UserControlsJQDateRange" Then
                    AddChangedHandlers(ctrl.Controls)
                ElseIf (ctrl.Controls.Count > 0 And Not (TypeOf ctrl Is INamingContainer)) Then
                    AddChangedHandlers(ctrl.Controls)

                Else
                    '  AddChangedHandlers(ctrl.Controls)
                    'ElseIf ctrl.GetType.ToString = "ASP.user_controls_employeesearch_ascx" Then
                    '    AddChangedHandlers(ctrl.Controls)
                    'ElseIf (ctrl.Controls.Count > 0) Then 'And Not (TypeOf ctrl Is INamingContainer)) Then
                    '    AddChangedHandlers(ctrl.Controls)
                End If
                '//could add recursion if we are missing some controls.
                'else if (ctrl.Controls.Count > 0 && !(ctrl is INamingContainer) )
                '{
                '    AddChangedHandlers(ctrl.Controls);
                '}
            Next ctrl
        End Sub


        ''' <summary>
        ''' This gets called when a row is changed.  Store the id of the row and wait to update
        ''' until save is called.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="args"></param>
        Public Sub HandleRowChanged(ByVal sender As Object, ByVal args As EventArgs)

            Dim row As GridViewRow = TryCast((CType(sender, Control)).BindingContainer, GridViewRow)

            If Not Nothing Is row Then
                If 0 <> (row.RowState And DataControlRowState.Insert) Then
                    Dim altRowIndex As Integer = Me.InnerTable.Rows.GetRowIndex(row)
                    If False = newRows.Contains(altRowIndex) Then
                        newRows.Add(altRowIndex)
                    End If
                Else
                    If False = dirtyRows_Renamed.Contains(row.RowIndex) Then
                        dirtyRows_Renamed.Add(row.RowIndex)
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' Setting this property will cause the grid to update all modified records when 
        ''' this button is clicked.  It currently supports Button, ImageButton, and LinkButton.
        ''' If you set this property, you do not need to call save programatically.
        ''' </summary>
        <IDReferenceProperty(GetType(Control))> _
        Public Property SaveButtonID() As String
            Get
                If Me.ViewState("SaveButtonID") Is Nothing Then
                    Return CStr(String.Empty)
                Else
                    Return Me.ViewState("SaveButtonID")
                End If
                'Return CStr(Me.ViewState("SaveButtonID") ?? String.Empty)
            End Get
            Set(ByVal value As String)
                Me.ViewState("SaveButtonID") = value
            End Set
        End Property
        ''' <summary>
        ''' Attaches an eventhandler to the onclick method of the save button.
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnLoad(ByVal e As EventArgs)
            MyBase.OnLoad(e)

            'Attach an event handler to the save button.
            If False = String.IsNullOrEmpty(Me.SaveButtonID) Then
                Dim btn As Control = RecursiveFindControl(Me.NamingContainer, Me.SaveButtonID)
                If Not Nothing Is btn Then
                    If TypeOf btn Is Button Then
                        AddHandler (CType(btn, Button)).Click, AddressOf SaveClicked
                    ElseIf TypeOf btn Is LinkButton Then
                        AddHandler (CType(btn, LinkButton)).Click, AddressOf SaveClicked
                    ElseIf TypeOf btn Is ImageButton Then
                        AddHandler (CType(btn, ImageButton)).Click, AddressOf SaveImageClicked                        
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' Looks for a control recursively up the control tree.  We need this because Page.FindControl
        ''' does not find the control if we are inside a masterpage content section.
        ''' </summary>
        ''' <param name="namingcontainer"></param>
        ''' <param name="controlName"></param>
        ''' <returns></returns>
        Private Function RecursiveFindControl(ByVal namingcontainer As Control, ByVal controlName As String) As Control
            Dim c As Control = namingcontainer.FindControl(controlName)

            If Not c Is Nothing Then
                Return c
            End If

            If Not namingcontainer.NamingContainer Is Nothing Then
                Return RecursiveFindControl(namingcontainer.NamingContainer, controlName)
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' Handles the save event, and calls the save method.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub SaveClicked(ByVal sender As Object, ByVal e As EventArgs)
            Me.Save()
            Me.DataBind()
        End Sub

        Private Sub SaveImageClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
            Me.Save()
            Me.DataBind()
        End Sub


        ''' <summary>
        ''' Saves any modified rows.  This is called automatically if the SaveButtonId is set.
        ''' </summary>
        Public Sub Save()
            Try
                For Each row As Integer In dirtyRows_Renamed
                    'TODO: need to check if we really want false here.  Probably want to pull this
                    'from the save button.
                    Me.UpdateRow(row, False)
                Next row

                For Each row As Integer In newRows
                    'Make the datasource save a new row.
                    Me.InsertRow(row, False)
                Next row
            Finally
                DirtyRows.Clear()
                newRows.Clear()
            End Try
        End Sub

        ''' <summary>
        ''' Prepares the <see cref="RowInserting"/> event and calls insert on the DataSource.
        ''' </summary>
        ''' <param name="rowIndex"></param>
        ''' <param name="causesValidation"></param>
        Private Sub InsertRow(ByVal rowIndex As Integer, ByVal causesValidation As Boolean)
            Dim row As GridViewRow = Nothing

            If ((Not causesValidation) OrElse (Me.Page Is Nothing)) OrElse Me.Page.IsValid Then
                Dim dsv As DataSourceView = Nothing
                Dim useDataSource As Boolean = MyBase.IsBoundUsingDataSourceID
                If useDataSource Then
                    dsv = Me.GetData()
                    If dsv Is Nothing Then
                        Throw New HttpException("DataSource Returned Null View")
                    End If
                End If
                Dim args As GridViewInsertEventArgs = New GridViewInsertEventArgs(rowIndex)
                If useDataSource Then
                    If (row Is Nothing) AndAlso (Me.InnerTable.Rows.Count > rowIndex) Then
                        row = TryCast(Me.InnerTable.Rows(rowIndex), GridViewRow)
                    End If
                    If Not row Is Nothing Then
                        Me.ExtractRowValues(args.NewValues, row, True, False)
                    End If
                End If

                Me.OnRowInserting(args)

                If (Not args.Cancel) AndAlso useDataSource Then
                    dsv.Insert(args.NewValues, New DataSourceViewOperationCallback(AddressOf DataSourceViewInsertCallback))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Callback for the datasource's insert command.
        ''' </summary>
        ''' <param name="i"></param>
        ''' <param name="ex"></param>
        ''' <returns></returns>
        Private Function DataSourceViewInsertCallback(ByVal i As Integer, ByVal ex As Exception) As Boolean
            If Not Nothing Is ex Then
                Throw ex
            End If

            Return True
        End Function

        ''' <summary>
        ''' Fires the <see cref="RowInserting"/> event.
        ''' </summary>
        ''' <param name="args"></param>
        Protected Overridable Sub OnRowInserting(ByVal args As GridViewInsertEventArgs)
            Dim handler As System.Delegate = Me.Events(RowInsertingEvent)
            If Not Nothing Is handler Then
                handler.DynamicInvoke(Me, args)
            End If
        End Sub

        ''' <summary>
        ''' Event fires when new row has been edited, and save is clicked.
        ''' </summary>
        Public Custom Event RowInserting As GridViewInsertEventHandler
            AddHandler(ByVal value As GridViewInsertEventHandler)
                Me.Events.AddHandler(RowInsertingEvent, value)
            End AddHandler
            RemoveHandler(ByVal value As GridViewInsertEventHandler)
                Me.Events.RemoveHandler(RowInsertingEvent, value)
            End RemoveHandler
            RaiseEvent(ByVal sender As Object, ByVal args As GridViewInsertEventArgs)
            End RaiseEvent
        End Event

        ''' <summary>
        ''' Access to the GridView's inner table.
        ''' </summary>
        Protected ReadOnly Property InnerTable() As Table
            Get
                If False = Me.HasControls() Then
                    Return Nothing
                End If
                Return CType(Me.Controls(0), Table)
            End Get
        End Property

        ''' <summary>
        ''' Enables inline inserting.  Off by default.
        ''' </summary>
        Public Property EnableInsert() As Boolean
            Get
                'Return CBool(Me.ViewState("EnableInsert") ?? False)
                If Me.ViewState("EnableInsert") Is Nothing Then
                    Return False
                Else
                    Return Me.ViewState("EnableInsert")
                End If
            End Get
            Set(ByVal value As Boolean)
                Me.ViewState("EnableInsert") = value
            End Set
        End Property
        ''' <summary>
        ''' We have to recreate our insert row so we can load the postback info from it.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Overrides Sub OnPagePreLoad(ByVal sender As Object, ByVal e As EventArgs)
            MyBase.OnPagePreLoad(sender, e)

            If Me.EnableInsert AndAlso Me.Page.IsPostBack Then
                Me.CreateInsertRow()
            End If
        End Sub

        ''' <summary>
        ''' After the controls are databound, add a row to the end.
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnDataBound(ByVal e As EventArgs)
            If Me.EnableInsert Then
                Me.CreateInsertRow()
            End If

            MyBase.OnDataBound(e)
        End Sub

        ''' <summary>
        ''' Creates the insert row and adds it to the inner table.
        ''' </summary>
        Protected Overridable Sub CreateInsertRow()
            Dim row As GridViewRow = Me.CreateRow(Me.Rows.Count, -1, DataControlRowType.DataRow, DataControlRowState.Insert)

            Dim fields As DataControlField() = New DataControlField(Me.Columns.Count - 1) {}
            Me.Columns.CopyTo(fields, 0)

            Me.InitializeRow(row, fields)

            Dim index As Integer
            If Not Me.InnerTable Is Nothing Then
                If Me.ShowFooter Then
                    index = Me.InnerTable.Rows.Count - (1)
                Else
                    index = Me.InnerTable.Rows.Count - (0)
                End If
                Me.InnerTable.Rows.AddAt(index, row)
                Me.InnerTable.Rows(index).FindControl("_lnkBtnDelete").Visible = "False"
            End If

            'this is where i need to hide the delete button on the new row.

        End Sub

        Private Sub BulkEditGridView_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles Me.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Attributes("data-title") = Me.Columns(i).HeaderText
                Next
            End If
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="args"></param>
        Public Delegate Sub GridViewInsertEventHandler(ByVal sender As Object, ByVal args As GridViewInsertEventArgs)

        ''' <summary>
        ''' 
        ''' </summary>
        Public Class GridViewInsertEventArgs : Inherits CancelEventArgs
            Private _rowIndex As Integer
            Private _values As IOrderedDictionary

            'INSTANT VB NOTE: The parameter rowIndex was renamed since Visual Basic will not uniquely identify class members when parameters have the same name:
            Public Sub New(ByVal rowIndex_Renamed As Integer)
                MyBase.New(False)
                Me._rowIndex = rowIndex_Renamed
            End Sub

            ''' <summary>
            ''' Gets a dictionary containing the revised values of the non-key field name/value
            ''' pairs in the row to update.
            ''' </summary>
            Public ReadOnly Property NewValues() As IOrderedDictionary
                Get
                    If Me._values Is Nothing Then
                        Me._values = New OrderedDictionary()
                    End If
                    Return Me._values
                End Get
            End Property

            ''' <summary>
            ''' Gets the index of the row being updated.
            ''' </summary>
            Public ReadOnly Property RowIndex() As Integer
                Get
                    Return Me._rowIndex
                End Get
            End Property
        End Class

    End Class
End Namespace