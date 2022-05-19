'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : mjpope
' Created          : 10-28-2010
'
' Last Modified By : mjpope
' Last Modified On : 02-14-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports HelperDal
Imports IP.TaskTrackerDAL
Imports Devart.Data.Oracle

'Imports TaskTrackerListDAL

Public Class TaskTrackerListsBll

    Public Function GetDueDateOptions() As Collection
        Dim returnVal As New Collection
        Try
            With returnVal
                .Add(New ListItem("Overdue", "Overdue"), "Overdue")
                .Add(New ListItem("Next 7 Days", "Next 7 Days"), "Next 7 Days")
                .Add(New ListItem("Next 14 Days", "Next 14 Days"), "Next 14 Days")
                .Add(New ListItem("Next 30 Days", "Next 30 Days"), "Next 30 Days")
            End With
        Catch
            Throw
        End Try
        Return returnVal
    End Function

    Public Function GetActivities() As System.Collections.Generic.List(Of TaskActivity) 'TaskTrackerListDAL.ActivityListDataTable
        Dim activityTA As TaskTrackerListDALTableAdapters.ActivityListTableAdapter
        Dim activityDT As TaskTrackerListDAL.ActivityListDataTable = Nothing
        Dim activityList As System.Collections.Generic.List(Of TaskActivity)
        Dim rsactivityList As Object = Nothing
        Dim cacheKey As String = "MTTList_activityList"
        Dim cacheHours As Integer = 8
        Try
            activityList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of TaskActivity))
            If activityList Is Nothing OrElse activityList.Count = 0 Then
                activityTA = New TaskTrackerListDALTableAdapters.ActivityListTableAdapter
                activityTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                activityDT = activityTA.GetActivities(rsactivityList)
                activityList = New System.Collections.Generic.List(Of TaskActivity)
                For Each rowItem As TaskTrackerListDAL.ActivityListRow In activityDT.Rows
                    activityList.Add(New IP.MEAS.BO.TaskActivity(rowItem.ACTIVITYNAME, rowItem.ACTIVITYSEQID))
                Next
                InsertDataIntoCache(cacheKey, cacheHours, activityList)
            End If
        Catch
            Throw
        Finally
            activityTA = Nothing
            activityDT = Nothing
        End Try
        Return activityList
        activityTA = Nothing
    End Function

    Public Function GetTaskTypes() As System.Collections.Generic.List(Of TaskType)
        Dim tasktypeTA As TaskTrackerListDALTableAdapters.TaskTypesTableAdapter
        Dim tasktypeDT As TaskTrackerListDAL.TaskTypesDataTable = Nothing
        Dim rstasktypeList As Object = Nothing
        Dim taskTypeList As System.Collections.Generic.List(Of TaskType)
        Dim cacheKey As String = "MTTList_tasktypeList"
        Dim cacheHours As Integer = 8
        Try
            taskTypeList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of TaskType))
            If taskTypeList Is Nothing OrElse taskTypeList.Count = 0 Then
                tasktypeTA = New TaskTrackerListDALTableAdapters.TaskTypesTableAdapter
                tasktypeTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                tasktypeDT = tasktypeTA.GetTaskTypes(rstasktypeList)
                taskTypeList = New System.Collections.Generic.List(Of TaskType)
                For Each rowItem As TaskTrackerListDAL.TaskTypesRow In tasktypeDT.Rows
                    taskTypeList.Add(New IP.MEAS.BO.TaskType(rowItem.TASKTYPENAME, rowItem.TASKTYPESEQID))
                Next
                InsertDataIntoCache(cacheKey, cacheHours, taskTypeList)
            End If
        Catch
            Throw
        Finally
            tasktypeTA = Nothing
            tasktypeDT = Nothing
        End Try
        Return taskTypeList
        tasktypeTA = Nothing
    End Function

    Public Function GetSourceSystems() As System.Collections.Generic.List(Of SourceSystems)
        Dim sourcesystemTA As TaskTrackerListDALTableAdapters.SourceSystemsTableAdapter
        Dim sourcesystemDT As TaskTrackerListDAL.SourceSystemsDataTable = Nothing
        Dim rssourcesystemList As Object = Nothing
        Dim sourceSystemList As System.Collections.Generic.List(Of SourceSystems)
        Dim cacheKey As String = "MTTList_sourcesystemList"
        Dim cacheHours As Integer = 8
        Try
            sourceSystemList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of SourceSystems))
            If sourceSystemList Is Nothing OrElse sourceSystemList.Count = 0 Then
                sourcesystemTA = New TaskTrackerListDALTableAdapters.SourceSystemsTableAdapter
                sourcesystemTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                sourcesystemDT = sourcesystemTA.GetSourceSystems(rssourcesystemList)
                sourceSystemList = New System.Collections.Generic.List(Of SourceSystems)
                For Each rowItem As System.Data.DataRow In sourcesystemDT.Rows 'TaskTrackerListDAL.SourceSystemsRow In sourcesystemDT.Rows
                    'sourceSystemList.Add(New IP.MEAS.BO.SourceSystems(rowItem.Item(0), CInt(rowItem(1)), rowItem(2))) 'rowItem.EXTERNALSOURCENAME, rowItem.EXTERNALSOURCESEQID))
                    Dim sourceItem As New IP.MEAS.BO.SourceSystems
                    With sourceItem
                        .ExternalSource = IP.Bids.SharedFunctions.DataClean(rowItem.Item(0))
                        If rowItem(1) IsNot Nothing AndAlso IsNumeric(rowItem(1)) Then
                            .ExternalSourceSeqid = CInt(rowItem(1))
                        Else
                            .ExternalSourceSeqid = -1
                        End If
                        .ExternalSourceUrl = IP.Bids.SharedFunctions.DataClean(rowItem(2))
                    End With
                    sourceSystemList.Add(sourceItem)
                Next
                InsertDataIntoCache(cacheKey, cacheHours, sourceSystemList)
            End If
        Catch
            Throw
        Finally
            sourcesystemTA = Nothing
            sourcesystemDT = Nothing
        End Try
        Return sourceSystemList
    End Function
    ' distinctListingDT.Load(listingDT.DefaultView.ToTable(True, New String() {"DUEDATE", "ITEMTITLE", "TASKITEMSEQID", "STATUSNAME", "STATUSSEQID", "TASKHEADERSEQID", "ROLEDESCRIPTION", "RESPNAME", "RESPROLESITENAME"}).CreateDataReader)

    Public Shared Function GetResponsibleRoles() As System.Collections.Generic.List(Of ResponsibleUsers)
        Dim responsibleUsersTA As TaskDetailTableAdapters.ResponsibleUserTableAdapter
        Dim responsibleUsersDT As TaskDetail.ResponsibleUserDataTable = Nothing
        Dim distinctListingDT As New TaskDetail.ResponsibleUserDataTable
        Dim rsResponsibleUsersList As Object = Nothing
        Dim responsibleUsersList As System.Collections.Generic.List(Of ResponsibleUsers)

        'Dim cacheKey As String
        'Dim cacheHours As Integer = 4
        Try
            'If plantCode = "9998" Or (Not plantCode Is Nothing AndAlso String.IsNullOrEmpty(plantCode)) Then
            '    plantCode = "" '"0000"
            'End If
            'cacheKey = "MTTList_responsibleUsersList_" & plantCode
            'responsibleUsersList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of ResponsibleUsers))
            'If responsibleUsersList Is Nothing OrElse responsibleUsersList.Count = 0 Then
            responsibleUsersTA = New TaskDetailTableAdapters.ResponsibleUserTableAdapter
            responsibleUsersTA.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            responsibleUsersDT = responsibleUsersTA.GetResponsibleList("", rsResponsibleUsersList)

            distinctListingDT.Load(responsibleUsersDT.DefaultView.ToTable(True, New String() {"ROLEDESCRIPTION", "ROLESEQID", "SORTORDER"}).CreateDataReader)

            responsibleUsersList = New System.Collections.Generic.List(Of ResponsibleUsers)
            For Each rowItem As TaskDetail.ResponsibleUserRow In distinctListingDT.Rows
                If rowItem.IsNAMENull Then
                    rowItem.NAME = String.Empty
                End If
                If rowItem.IsROLEDESCRIPTIONNull Then
                    rowItem.ROLEDESCRIPTION = String.Empty
                End If
                If rowItem.IsROLESEQIDNull Then
                    rowItem.ROLESEQID = String.Empty
                End If
                If rowItem.IsSORTORDERNull Then
                    rowItem.SORTORDER = 1
                End If
                If rowItem.IsUSERNAMENull Then
                    rowItem.USERNAME = String.Empty
                End If
                If rowItem.IsINACTIVE_FLAGNull Then
                    rowItem.INACTIVE_FLAG = "N"
                End If
                ''If plantcode is empty only add roles
                'If plantCode = String.Empty And rowItem.USERNAME <> String.Empty Then
                responsibleUsersList.Add(New IP.MEAS.BO.ResponsibleUsers(rowItem.SORTORDER, rowItem.ROLEDESCRIPTION, rowItem.NAME, rowItem.USERNAME, rowItem.ROLESEQID, rowItem.ROLENAME, rowItem.INACTIVE_FLAG))
                'ElseIf plantCode <> String.Empty Then
                'responsibleUsersList.Add(New IP.MEAS.BO.ResponsibleUsers(rowItem.SORTORDER, rowItem.ROLEDESCRIPTION, rowItem.NAME, rowItem.USERNAME, rowItem.ROLESEQID, rowItem.ROLENAME))
                'End If
            Next
            'InsertDataIntoCache(cacheKey, cacheHours, responsibleUsersList)
            'End If
        Catch
            Throw
        Finally
            responsibleUsersTA = Nothing
            responsibleUsersDT = Nothing
        End Try
        Return responsibleUsersList
        responsibleUsersTA = Nothing
    End Function

    Public Shared Function GetResponsibleUsers(ByVal plantCode As String) As System.Collections.Generic.List(Of ResponsibleUsers)
        Dim responsibleUsersTA As TaskDetailTableAdapters.ResponsibleUserTableAdapter
        Dim responsibleUsersDT As TaskDetail.ResponsibleUserDataTable = Nothing
        Dim rsResponsibleUsersList As Object = Nothing
        Dim responsibleUsersList As System.Collections.Generic.List(Of ResponsibleUsers)

        Dim cacheKey As String
        Dim cacheHours As Integer = 4
        Try

            If plantCode = "9998" Or (Not plantCode Is Nothing AndAlso String.IsNullOrEmpty(plantCode)) Then
                plantCode = "9998" '"0000"
                'Return Nothing
            End If
            cacheKey = "MTTList_responsibleUsersList_" & plantCode
            responsibleUsersList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of ResponsibleUsers))
            If responsibleUsersList Is Nothing OrElse responsibleUsersList.Count = 0 Then
                responsibleUsersTA = New TaskDetailTableAdapters.ResponsibleUserTableAdapter
                responsibleUsersTA.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)

                responsibleUsersDT = responsibleUsersTA.GetResponsibleList(plantCode, rsResponsibleUsersList)
                responsibleUsersList = New System.Collections.Generic.List(Of ResponsibleUsers)
                For Each rowItem As TaskDetail.ResponsibleUserRow In responsibleUsersDT.Rows
                    If rowItem.IsNAMENull Then
                        rowItem.NAME = String.Empty
                    End If
                    If rowItem.IsROLEDESCRIPTIONNull Then
                        rowItem.ROLEDESCRIPTION = String.Empty
                    End If
                    If rowItem.IsROLESEQIDNull Then
                        rowItem.ROLESEQID = String.Empty
                    End If
                    If rowItem.IsSORTORDERNull Then
                        rowItem.SORTORDER = 1
                    End If
                    If rowItem.IsUSERNAMENull Then
                        rowItem.USERNAME = String.Empty
                    End If
                    If rowItem.IsINACTIVE_FLAGNull Then
                        rowItem.INACTIVE_FLAG = "N"
                    End If
                    If rowItem.INACTIVE_FLAG = "Y" Then
                        rowItem.NAME = "***" & rowItem.NAME
                    End If
                    ''If plantcode is empty only add roles
                    'If plantCode = String.Empty And rowItem.USERNAME <> String.Empty Then
                    responsibleUsersList.Add(New IP.MEAS.BO.ResponsibleUsers(rowItem.SORTORDER, rowItem.ROLEDESCRIPTION, rowItem.NAME, rowItem.USERNAME, rowItem.ROLESEQID, rowItem.ROLENAME, rowItem.INACTIVE_FLAG))
                    'ElseIf plantCode <> String.Empty Then
                    'responsibleUsersList.Add(New IP.MEAS.BO.ResponsibleUsers(rowItem.SORTORDER, rowItem.ROLEDESCRIPTION, rowItem.NAME, rowItem.USERNAME, rowItem.ROLESEQID, rowItem.ROLENAME))
                    'End If
                Next
                InsertDataIntoCache(cacheKey, cacheHours, responsibleUsersList)
            End If
        Catch e As Exception
            Throw
        Finally
            responsibleUsersTA = Nothing
            responsibleUsersDT = Nothing
        End Try
        Return responsibleUsersList
        responsibleUsersTA = Nothing
    End Function

    Public Function GetTaskStatus() As System.Collections.Generic.List(Of TaskStatus) 'TaskTrackerListDAL.ActivityListDataTable
        Dim statusTA As TaskTrackerListDALTableAdapters.TaskStatusTableAdapter
        Dim statusDT As TaskTrackerListDAL.TaskStatusDataTable = Nothing
        Dim statusList As System.Collections.Generic.List(Of TaskStatus)
        Dim rsactivityList As Object = Nothing
        Dim cacheKey As String = "MTTList_TaskStatus"
        Dim cacheHours As Integer = 8
        Dim imageIcon As String = String.Empty
        Try
            statusList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of TaskStatus))
            If statusList Is Nothing OrElse statusList.Count = 0 Then
                statusTA = New TaskTrackerListDALTableAdapters.TaskStatusTableAdapter
                statusTA.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                statusDT = statusTA.GetTaskStatus(rsactivityList)
                statusList = New System.Collections.Generic.List(Of TaskStatus)
                For Each rowItem As TaskTrackerListDAL.TaskStatusRow In statusDT.Rows
                    If rowItem.IsSTATUSNAMENull Then
                        Continue For
                    End If
                    Select Case rowItem.STATUSNAME.ToLower
                        Case "open"
                            imageIcon = "wip.gif"
                        Case "complete"
                            imageIcon = "complete.gif"
                        Case "cancelled"
                            imageIcon = "cancelled.gif"
                        Case "no work needed"
                            imageIcon = "noworkneeded.gif"
                    End Select
                    statusList.Add(New IP.MEAS.BO.TaskStatus(rowItem.STATUSNAME, rowItem.STATUSSEQID, imageIcon))
                Next
                statusList.Add(New IP.MEAS.BO.TaskStatus("Overdue", -1, "late_notcomp.gif"))
                InsertDataIntoCache(cacheKey, cacheHours, statusList)
            End If
        Catch
            Throw
        Finally
            statusTA = Nothing
            statusDT = Nothing
        End Try
        Return statusList
        statusTA = Nothing
    End Function

    Public Function GetTaskStatus(ByVal statusID As Integer, ByVal includeLabel As Boolean, ByVal imagePath As String) As String
        Dim returnVal As String = String.Empty
        Dim status As System.Collections.Generic.List(Of TaskStatus) = GetTaskStatus()
        For Each item As TaskStatus In status
            If item.StatusSeqid = statusID Then
                If includeLabel Then
                    Dim img As String = "<img src='{0}' border='0' align=center width=15 height=15 title='{1}' alt='{1}'>{1}"
                    Dim statusText As String = String.Format(img, imagePath & item.ImageIcon, IP.Bids.SharedFunctions.LocalizeValue(item.StatusName, True))
                    returnVal = statusText
                Else
                    Dim img As String = "<img src='{0}' border='0' align=center width=15 height=15 title='{1}' alt='{1}'>"
                    Dim statusText As String = String.Format(img, imagePath & item.ImageIcon, IP.Bids.SharedFunctions.LocalizeValue(item.StatusName, True))
                    returnVal = statusText
                End If
            End If
        Next
        Return returnVal
    End Function

    Public Function GetUserDefaults(ByVal userName As String) As RIUserDefaults.CurrentUserDefaults '  Specialized.StringDictionary
        'Dim userDefaultsTA As TaskDetailTableAdapters.UserDefaultsTableAdapter
        'Dim userDefaultsDT As TaskDetail.UserDefaultsDataTable = Nothing
        'Dim rsUserDefaults As Object = Nothing
        'Dim userDefaultList As Specialized.StringDictionary
        'Dim cacheKey As String = "MTTList_userDefaultsList_" & userName
        'Dim cacheHours As Integer = 8


        Try
            'Dim ws As New TaskTrackerWS.TaskTracker

            'Dim j = ws.GetUserDefaultValues(userName.ToUpper)

            'Dim ud As New RIUserDefaults.CurrentUserDefaults(userName, RIUserDefaults.Applications.MTT, ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            'System.Diagnostics.Debug.Print(ud.GetDefaultValue(RIUserDefaults.UserProfileTypes.Area, "1"))
            'System.Diagnostics.Debug.Print(ud.GetDefaultValue(RIUserDefaults.UserProfileTypes.BusinessUnit))
            'System.Diagnostics.Debug.Print(ud.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode))


            'ud = New RIUserDefaults.CurrentUserDefaults("cxcox", ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            'System.Diagnostics.Debug.Print(ud.GetDefaultValue(RIUserDefaults.UserProfileTypes.Area))
            'System.Diagnostics.Debug.Print(ud.GetDefaultValue(RIUserDefaults.UserProfileTypes.BusinessUnit))
            'System.Diagnostics.Debug.Print(ud.GetDefaultValue(RIUserDefaults.UserProfileTypes.PlantCode))

            userName = userName.ToUpper
            'userDefaultList = CType(GetDataFromCache(cacheKey, cacheHours), Specialized.StringDictionary)
            'If userDefaultList Is Nothing OrElse userDefaultList.Count = 0 Then
            Dim currentUserDefaultList As New RIUserDefaults.CurrentUserDefaults(userName, RIUserDefaults.Applications.MTT, ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            Return currentUserDefaultList
            'userDefaultList = New Specialized.StringDictionary
            'For Each item In currentUserDefaultList.UserDefaults
            '    userDefaultList.Add(item.ProfileTypeName, item.ProfileTypeValue)
            'Next


            'userDefaultsTA = New TaskDetailTableAdapters.UserDefaultsTableAdapter
            'userDefaultsTA.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            'userDefaultsDT = userDefaultsTA.GetUserDefaults(userName, rsUserDefaults)
            'userDefaultList = New Specialized.StringDictionary
            'For Each rowItem As TaskDetail.UserDefaultsRow In userDefaultsDT.Rows
            '    If rowItem.IsPROFILETYPENAMENull = False And rowItem.IsPROFILETYPEVALUENull = False And userDefaultList.ContainsKey(rowItem.PROFILETYPENAME) = False Then
            '        userDefaultList.Add(rowItem.PROFILETYPENAME, rowItem.PROFILETYPEVALUE)
            '    End If
            'Next
            'If userDefaultList.Count > 0 Then
            '    'InsertDataIntoCache(cacheKey, cacheHours, userDefaultList)
            'End If
            'End If
        Catch
            Throw
            Return Nothing
        Finally
            'userDefaultsTA = Nothing
            'userDefaultsDT = Nothing
        End Try
        'Return userDefaultList
    End Function
    Public Function GetGMSElements() As System.Collections.Generic.List(Of GMSElementList)
        Dim elementTA As GMSElementTableAdapters.GMSElementsTableAdapter
        Dim elementDT As GMSElement.GMSElementsDataTable = Nothing
        Dim elementList As System.Collections.Generic.List(Of GMSElementList)
        Dim rselementList As Object = Nothing
        Dim cacheKey As String = "MTTList_elementList"
        Dim cacheHours As Integer = 8
        Try
            elementList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of GMSElementList))
            If elementList Is Nothing OrElse elementList.Count = 0 Then
                elementTA = New GMSElementTableAdapters.GMSElementsTableAdapter
                elementTA.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                elementDT = elementTA.GetData(rselementList)
                elementList = New System.Collections.Generic.List(Of GMSElementList)
                elementList.Add(New IP.MEAS.BO.GMSElementList("All"))
                For Each rowItem As GMSElement.GMSElementsRow In elementDT.Rows
                    elementList.Add(New IP.MEAS.BO.GMSElementList(rowItem.TITLE))
                Next

                InsertDataIntoCache(cacheKey, cacheHours, elementList)
            End If
        Catch
            Throw
        Finally
            elementTA = Nothing
            elementDT = Nothing
        End Try
        Return elementList
        elementTA = Nothing
    End Function
    'Public Function GetGMSElements() As System.Collections.Generic.List(Of GMSElementList)
    '    Dim sourcesystemTA As GMSElementTableAdapters.GMSElementsTableAdapter
    '    Dim sourcesystemDT As GMSElement.GMSElementsDataTable = Nothing
    '    Dim rssourcesystemList As Object = Nothing
    '    Dim sourceSystemList As System.Collections.Generic.List(Of GMSElementList)
    '    Dim cacheKey As String = "MTTList_elementList"
    '    Dim cacheHours As Integer = 8
    '    Try
    '        sourceSystemList = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of GMSElementList))
    '        If sourceSystemList Is Nothing OrElse sourceSystemList.Count = 0 Then
    '            sourcesystemTA = New GMSElementTableAdapters.GMSElementsTableAdapter
    '            sourcesystemTA.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
    '            sourcesystemDT = sourcesystemTA.GetData(rssourcesystemList)
    '            sourceSystemList = New System.Collections.Generic.List(Of GMSElementList)
    '            For Each rowItem As GMSElement.GMSElementsRow In sourcesystemDT.Rows
    '                'Dim sourceItem As New IP.MEAS.BO.SourceSystems
    '                'With sourceItem
    '                '    .ExternalSource = IP.Bids.SharedFunctions.DataClean(rowItem.Item(0))
    '                '    If rowItem(1) IsNot Nothing AndAlso IsNumeric(rowItem(1)) Then
    '                '        .ExternalSourceSeqid = CInt(rowItem(1))
    '                '    Else
    '                '        .ExternalSourceSeqid = -1
    '                '    End If
    '                '    .ExternalSourceUrl = IP.Bids.SharedFunctions.DataClean(rowItem(2))
    '                'End With
    '                sourceSystemList.Add(New IP.MEAS.BO.GMSElementList(rowItem.TITLE))
    '            Next
    '            InsertDataIntoCache(cacheKey, cacheHours, sourceSystemList)
    '        End If
    '    Catch
    '        Throw
    '    Finally
    '        sourcesystemTA = Nothing
    '        sourcesystemDT = Nothing
    '    End Try
    '    Return sourceSystemList
    'End Function
End Class
