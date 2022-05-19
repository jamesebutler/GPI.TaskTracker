Imports System.Linq

Partial Class Popups_CopyTask
    Inherits IP.Bids.BasePage
    Public AddedUsers As New List(Of ResponsibleUserForCopy)
    Private currentTaskItem As TaskItem = Nothing
    Private TaskNumber As String = String.Empty
    Const MaxAddedUsers = 20

    Private Sub _btnAddUser_Click(sender As Object, e As EventArgs) Handles _btnAddUser.Click

        With AddedUsers
            .Add(New ResponsibleUserForCopy(Me._ResponsiblePerson.SelectedValue, Me._ResponsiblePerson.SelectedText.Trim, CDate(_EstimatedDueDate.StartDate), Me._ResponsiblePerson.PlantCode, _EstimatedDueDate.DateIsCritical))
        End With
        BindUsers()

    End Sub

    Private Sub EnableOrDisableAddUserButton()
        If AddedUsers.Count >= MaxAddedUsers Then
            _btnAddUser.Enabled = False
        Else
            _btnAddUser.Enabled = True
        End If
    End Sub

    Private Sub Popups_CopyTask_Load(sender As Object, e As EventArgs) Handles Me.Load
        ValidateQueryInput()


        If Page.IsPostBack = False Then
            AddedUsers.Clear()
            If CDate(currentTaskItem.DueDate) < Now Then
                _EstimatedDueDate.StartDate = Now.ToShortDateString
            Else
                _EstimatedDueDate.StartDate = currentTaskItem.DueDate
            End If
            Me._ResponsiblePerson.DefaultUserName = IP.Bids.SharedFunctions.GetCurrentUser.Username

            Me._ResponsiblePerson.PopulateEmployeeList(IP.Bids.SharedFunctions.GetCurrentUser.Username)
            BindUsers()
        End If
        AddedUsers = GetAddedUsersFromSession()
    End Sub

    Private Sub ValidateQueryInput()
        TaskNumber = Request.QueryString("TaskNumber")
        If TaskNumber Is Nothing Then
            Throw New ApplicationException("Missing Task Number")
        End If
        If Not IsNumeric(TaskNumber) Then
            Throw New ApplicationException("Task Number is Invalid")
        End If
        Dim instanceOfTaskTrackerItem As New TaskTrackerItemBll
        currentTaskItem = instanceOfTaskTrackerItem.GetTaskItem(TaskNumber)

        If currentTaskItem Is Nothing Then
            Throw New ApplicationException("The specified Task Number does not exist")
        End If
    End Sub
    Private Sub BindUsers()
        If AddedUsers Is Nothing Then AddedUsers = New List(Of ResponsibleUserForCopy)
        Dim responsibleUsers = From userData In AddedUsers
                               Order By userData.ResponsibleUser
                               Select New With {.ResponsibleUser = userData.ResponsibleUser, .DueDate = userData.DueDate.ToShortDateString, .DateIsCritical = userData.DateIsCritical}

        _grvResponsiblePerson.DataSource = AddedUsers.OrderBy(Function(obj) obj.ResponsibleUser).ToList()
        _grvResponsiblePerson.Caption = String.Format("{0} users/roles can be added.", MaxAddedUsers - AddedUsers.Count)
        _grvResponsiblePerson.DataBind()
        EnableOrDisableAddUserButton()
        SaveAddedUsersIntoSession(AddedUsers)
    End Sub

    Private Sub SaveAddedUsersIntoSession(users As List(Of ResponsibleUserForCopy))
        Session.Remove("AddedUsers")
        Session.Item("AddedUsers") = users
    End Sub

    Private Function GetAddedUsersFromSession() As List(Of ResponsibleUserForCopy)
        If Session.Item("AddedUsers") IsNot Nothing Then
            Return Session.Item("AddedUsers")
        End If
        Return Nothing
    End Function

    Private Sub ClearAddedUsersFromSession()
        Session.Remove("AddedUsers")
    End Sub

    Protected Sub DeleteResponsibleUser(sender As Object, e As EventArgs)
        Dim deleteButton As LinkButton = TryCast(sender, LinkButton)
        If deleteButton Is Nothing Then Throw New ApplicationException("Error recognizing the delete button")

        AddedUsers.Remove(AddedUsers.Find(Function(obj) (obj.ResponsibleUserName = deleteButton.CommandArgument)))
        BindUsers()
    End Sub

    Private Sub CreateTasks_Click(sender As Object, e As EventArgs) Handles _btnCreateTasks.Click
        Try
            Dim instanceOfTaskTrackerItem As New TaskTrackerItemBll
            instanceOfTaskTrackerItem.CopyTask(IP.Bids.SharedFunctions.GetCurrentUser.Username, currentTaskItem.TaskHeaderSeqId, TaskNumber, AddedUsers)
            _lblCopyStatys.Text = String.Format("{0} task(s) have been created.", AddedUsers.Count)
            ClearAddedUsersFromSession()
            AddedUsers = GetAddedUsersFromSession()
            BindUsers()
        Catch ex As DataException
            _lblCopyStatys.Text = "The system was unable to create copies of the selected task"
            IP.Bids.SharedFunctions.HandleError("CopyTasks", ex.Message, ex)
        End Try
    End Sub
End Class
