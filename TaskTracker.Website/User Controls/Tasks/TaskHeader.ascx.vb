
Partial Class User_Controls_Tasks_TaskHeader
    Inherits System.Web.UI.UserControl
    Private TaskHeader As TaskHeaderBll 'Holds an instance of the Task Header Business Logic Layer

    Private _taskHeaderNumber As Integer = 0
    Public Property TaskHeaderNumber() As Integer
        Get
            Return _taskHeaderNumber
        End Get
        Set(ByVal value As Integer)
            _taskHeaderNumber = value
        End Set
    End Property

    Public Sub LoadHeaderInfo()
        If TaskHeaderNumber > 0 Then
            TaskHeader = New TaskHeaderBll(CInt(TaskHeaderNumber))
            If TaskHeader IsNot Nothing Then
                _frvTaskHeader.DataSource = TaskHeader.TaskHeaderRecord
                _frvTaskHeader.DataBind()
            End If
        End If
    End Sub

  
    Protected Function GetTypeManagers() As String
        Dim returnVal As String = String.Empty
        Try
            If TaskHeader IsNot Nothing Then
                If TaskHeader.TaskTypeManagers IsNot Nothing AndAlso TaskHeader.TaskTypeManagers.Count > 0 Then
                    'With TaskHeader.TaskTypeManagers
                    '    'Dim taskItemList As New StringBuilder
                    '    For i As Integer = 0 To .Count - 1 'Build a comma delimited list of selected TaskTypes
                    '        'If taskItemList.Length > 0 Then
                    '        '    taskItemList.Append(", ")
                    '        'End If
                    '        'taskItemList.Append(.Item(i).FullName)
                    '    Next

                    'End With

                    If TaskHeader.TaskTypeManagers IsNot Nothing Then
                        'Ensure that we have a distinct list of names
                        Dim managerName As New Collections.Specialized.OrderedDictionary    'SortedList 
                        With managerName
                            For Each item As TypeManagers In TaskHeader.TaskTypeManagers

                                If .Contains(item.UserName) = False Then
                                    .Add(item.UserName, IP.Bids.SharedFunctions.ToTitleCase(item.FullName.ToLower))
                                End If
                            Next
                        End With
                        Dim taskItemList As New StringBuilder
                        taskItemList.Append("<table style='width:100%'><tr>")
                        Dim itemCount As Integer = 0
                        For Each item As DictionaryEntry In managerName
                            itemCount += 1

                            taskItemList.Append("<td style='width:33%'>&bull;")
                            'If taskItemList.Length > 0 Then
                            '    taskItemList.Append(", ")
                            'End If
                            taskItemList.Append(item.Value)
                            taskItemList.Append("</td>")
                            If itemCount Mod 3 = 0 Then
                                taskItemList.Append("<tr/><tr>")
                            End If
                        Next
                        taskItemList.Append("</tr></table>")
                        returnVal = taskItemList.ToString
                    End If
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError(, , ex)
        End Try
        Return returnVal
    End Function
    'Public Sub LoadHeaderInfo()        
    '    TaskHeader = New TaskHeaderBll(CInt(TaskHeaderNumber))
    '    If TaskHeader IsNot Nothing Then
    '        _frvTaskHeader.DataSource = TaskHeader.TaskHeaderRecord
    '        _frvTaskHeader.DataBind()
    '    End If

    'End Sub
    Protected Function GetBusinessManagers() As String
        Dim returnVal As String = String.Empty
        Try
            If TaskHeader IsNot Nothing Then
                If TaskHeader.BusinessManagers IsNot Nothing AndAlso TaskHeader.BusinessManagers.Count > 0 Then
                    'With TaskHeader.BusinessManagers
                    '    Dim taskItemList As New StringBuilder
                    '    For i As Integer = 0 To .Count - 1 'Build a comma delimited list of selected TaskTypes
                    '        If taskItemList.Length > 0 Then
                    '            taskItemList.Append(", ")
                    '        End If
                    '        taskItemList.Append(.Item(i).FullName)
                    '    Next
                    '    returnVal = taskItemList.ToString
                    'End With
                    If TaskHeader.BusinessManagers IsNot Nothing Then
                        'Ensure that we have a distinct list of names
                        Dim managerName As New Collections.Specialized.OrderedDictionary    'SortedList 
                        With managerName
                            For Each item As BusinessManagers In TaskHeader.BusinessManagers

                                If .Contains(item.UserName) = False Then
                                    .Add(item.UserName, IP.Bids.SharedFunctions.ToTitleCase(item.FullName.ToLower))
                                End If
                            Next
                        End With
                        Dim taskItemList As New StringBuilder
                        taskItemList.Append("<table style='width:100%'><tr>")
                        Dim itemCount As Integer = 0
                        For Each item As DictionaryEntry In managerName
                            itemCount += 1

                            taskItemList.Append("<td style='width:33%'>&bull;")
                            'If taskItemList.Length > 0 Then
                            '    taskItemList.Append(", ")
                            'End If
                            taskItemList.Append(item.Value)
                            taskItemList.Append("</td>")
                            If itemCount Mod 3 = 0 Then
                                taskItemList.Append("<tr/><tr>")
                            End If
                        Next
                        taskItemList.Append("</tr></table>")
                        returnVal = taskItemList.ToString
                    End If
                End If

                'End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError(, , ex)
        End Try
        Return returnVal
    End Function
    Protected Function GetTaskTypes() As String
        Dim returnVal As String = String.Empty
        Try
            If TaskHeader IsNot Nothing Then
                If TaskHeader.TaskTypes IsNot Nothing AndAlso TaskHeader.TaskTypes.Count > 0 Then
                    With TaskHeader.TaskTypes
                        Dim taskItemList As New StringBuilder
                        For i As Integer = 0 To .Count - 1 'Build a comma delimited list of selected TaskTypes
                            If taskItemList.Length > 0 Then
                                taskItemList.Append(", ")
                            End If
                            taskItemList.Append(IP.Bids.SharedFunctions.LocalizeValue(.Item(i).TaskName))
                        Next
                        returnVal = taskItemList.ToString
                    End With
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError(, , ex)
        End Try
        Return returnVal
    End Function
End Class
