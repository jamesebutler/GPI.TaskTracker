'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : mjpope
' Created          : 03-31-2011
'
' Last Modified By : mjpope
' Last Modified On : 06-17-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports Microsoft.VisualBasic
Imports HelperDal
Imports Devart.Data.Oracle
Imports System.Globalization
Imports IP.TaskTrackerDAL.TaskDetail
Imports IP.TaskTrackerDAL.TaskDetailTableAdapters


Public NotInheritable Class UserDataBll
    Public Shared Function GetNotificationProfileList(ByVal userName As String) As System.Collections.Generic.List(Of BO.NotificationProfile)
        Dim adapter As DataMaintenanceDalTableAdapters.NotificationProfileTableAdapter
        Dim table As DataMaintenanceDAL.NotificationProfileDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim record As System.Collections.Generic.List(Of BO.NotificationProfile) = Nothing
        Dim cacheKey As String = "MTTGeneralData_NotificationProfile_" & userName
        Dim cacheHours As Integer = 8

        Try
            record = CType(GetDataFromCache(cacheKey, cacheHours), System.Collections.Generic.List(Of BO.NotificationProfile))
            If record Is Nothing Then
                If table Is Nothing OrElse table.Rows.Count = 0 Then
                    adapter = New DataMaintenanceDALTableAdapters.NotificationProfileTableAdapter
                    adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                    table = adapter.GetNotificationProfile(userName.ToUpper, emptyCursor)
                    If table IsNot Nothing Then
                        record = New System.Collections.Generic.List(Of BO.NotificationProfile)
                        For Each rowItem As DataMaintenanceDAL.NotificationProfileRow In table.Rows
                            With rowItem
                                If .IsEMAILTYPENull Then
                                    .EMAILTYPE = String.Empty
                                End If
                                If .IsPROFILETYPESEQIDNull Then
                                    .PROFILETYPESEQID = 0
                                End If
                                If .IsPROFILETYPEVALUENull Then
                                    .PROFILETYPEVALUE = String.Empty
                                End If
                                If .IsROLENAMENull Then
                                    .ROLENAME = String.Empty
                                End If
                                If .IsROLESEQIDNull Then
                                    .ROLESEQID = 0
                                End If
                                'If .IsPROFILETYPENAMENull Then
                                '    .PROFILETYPENAME = String.Empty
                                'End If
                                record.Add(New NotificationProfile(rowItem.ROLENAME, rowItem.ROLESEQID, rowItem.EMAILTYPE, rowItem.PROFILETYPESEQID, rowItem.PROFILETYPEVALUE, rowItem.PROFILETYPENAME))
                            End With
                        Next
                        'InsertDataIntoCache(cacheKey, cacheHours, record)
                    End If
                End If
            End If
        Catch
            Throw
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return record
        adapter = Nothing
    End Function
    Public Shared Function SaveNotificationProfile(ByVal profileUsername As String, ByVal repeatingData As String, ByVal updateUserName As String) As Boolean
        Dim adapter As DataMaintenanceDalTableAdapters.NotificationProfileTableAdapter
        '        Dim emptyCursor As Object = Nothing ' COMMENTED BY CODEIT.RIGHT
        Dim cacheKey As String = "MTTGeneralData_NotificationProfile_" & profileUsername
        Dim returnValue As Integer
        Try
            adapter = New DataMaintenanceDALTableAdapters.NotificationProfileTableAdapter
            adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
            adapter.UpdateNotificationProfile(profileUsername.ToUpper, repeatingData, updateUserName.ToUpper, returnValue)
            HelperDal.DeleteFromCache(cacheKey)
        Catch
            Throw
        Finally
            adapter = Nothing
        End Try
        Return True
        adapter = Nothing
    End Function



  
End Class
