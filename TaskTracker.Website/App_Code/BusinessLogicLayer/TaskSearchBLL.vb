'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 09-01-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-07-2010 
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Option Explicit On
Option Strict On
Imports System.Web
Imports System.Web.UI.WebControls

Public Class TaskSearchBll
#Region "Fields"
    Private _imgCompleted As String = "<br/><img src='Images/complete.gif' align=center width=15 height=15><a href='' style='color:Green'>Task Name</a>"
    Private _imgLateNotCompleted As String = "<br/><img src='Images/late_notcomp.gif' align=center width=15 height=15/><a href='' style='color:Red'>Task Name</a>"
    Private _imgWorkInProcess As String = "<br/><img src='Images/wip.gif' align=center width=15 height=15/><a href='' style='color:Black'>Task Name</a>"
    Public ReadOnly Property ImgCompleted() As String
        Get
            Return _imgCompleted
        End Get
    End Property
    Public ReadOnly Property ImgLateNotCompleted() As String
        Get
            Return _imgLateNotCompleted
        End Get
    End Property
    Public ReadOnly Property ImgWorkInProcess() As String
        Get
            Return _imgWorkInProcess
        End Get
    End Property
#End Region

#Region "Properties"
    Public Property EventDate() As Date
        Get
            Return Now
        End Get
        Set(ByVal value As Date)

        End Set
    End Property

    Public Property Site() As String
        Get
            Return ""
        End Get
        Set(ByVal value As String)

        End Set
    End Property

#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDueDateOptions() As Collection
        Dim returnVal As New Collection
        Try
            With returnVal
                .Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Overdue", True), "Overdue"), "Overdue")
                .Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Next 7 Days", True), "Next 7 Days"), "Next 7 Days")
                .Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Next 14 Days", True), "Next 14 Days"), "Next 14 Days")
                .Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Next 30 Days", True), "Next 30 Days"), "Next 30 Days")
            End With
        Catch
            Throw
        End Try
        Return returnVal
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="includeIcons"></param>
    ''' <param name="imagePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTaskStatusList(ByVal includeIcons As Boolean, ByVal imagePath As String) As Collection
        Dim returnVal As New Collection
        Try

            Dim ClosedComplete As String = IP.Bids.SharedFunctions.LocalizeValue("Closed Complete", True)
            Dim Cancelled As String = IP.Bids.SharedFunctions.LocalizeValue("Cancelled", True)
            Dim NoWorkNeeded As String = IP.Bids.SharedFunctions.LocalizeValue("No Work Needed", True)
            Dim Open As String = IP.Bids.SharedFunctions.LocalizeValue("Open", True)
            Dim CompleteNoWorkNeededCancelled As String = ClosedComplete & "," & NoWorkNeeded & "," & Cancelled

            If includeIcons Then               
                Dim imgCompleted As String = "(<img src='" & imagePath & "complete.gif' align=center width=15 height=15>" & ClosedComplete & " <img src='" & imagePath & "cancelled.gif' align=center width=15 height=15>" & Cancelled & " <img src='" & imagePath & "noworkneeded.gif' align=center width=15 height=15>" & NoWorkNeeded & ")"
                Dim imgWorkInProcess As String = "(<img src='" & imagePath & "wip.gif' align=center width=15 height=15/>" & Open & " <img src='" & imagePath & "late_notcomp.gif' align=center width=15 height=15/>Overdue)"
                With returnVal
                    .Add(New ListItem(imgWorkInProcess, "Open"), "Open")
                    .Add(New ListItem(imgCompleted, "Complete,No Work Needed,Cancelled"), "Complete,No Work Needed,Cancelled")                 
                End With
            Else
                With returnVal
                    .Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Open", True), "Open"), "Open")
                    .Add(New ListItem(IP.Bids.SharedFunctions.LocalizeValue("Closed", True), "Complete,No Work Needed,Cancelled"), "Complete,No Work Needed,Cancelled")

                End With
            End If
        Catch
            Throw
        End Try
        Return returnVal
    End Function

    Public Function GetTaskListing() As Collection 's.Specialized.StringDictionary
        Dim returnVal As New Collection 's.Specialized.StringDictionary
        Dim rnd As System.Random

        Try
            Dim listing As String
            '            Dim imgNoWorkNeeded As String = "<br/><img src='Images/noworkneeded.gif' align=center width=15 height=15><a href='' style='color:Green'>Task Name</a>" ' COMMENTED BY CODEIT.RIGHT
            'Dim imgCanceled As String = "<br/><img src='Images/canceled.gif' align=center width=15 height=15><a href='' style='color:Green'>Task Name</a>"
            Dim imgCompleted As String = "<br/><img src='Images/complete.gif' align=center width=15 height=15><a href='' style='color:Green'>Task Name</a>"
            Dim imgLateNotCompleted As String = "<br/><img src='Images/late_notcomp.gif' align=center width=15 height=15/><a href='' style='color:Red'>Task Name</a>"
            Dim imgWorkInProcess As String = "<br/><img src='Images/wip.gif' align=center width=15 height=15/><a href='' style='color:Black'>Task Name</a>"
            '            Dim br As String = "<br/>" ' COMMENTED BY CODEIT.RIGHT
            rnd = New System.Random()
            For i As Integer = 0 To 30
                Dim rndNumber As Integer = rnd.Next(1, 100)
                listing = String.Empty
                If rndNumber < 3 Then
                    listing = imgWorkInProcess & imgCompleted & imgWorkInProcess & imgWorkInProcess & imgCompleted & imgWorkInProcess & imgWorkInProcess & imgCompleted & imgWorkInProcess
                ElseIf rndNumber < 20 Then
                    listing = imgLateNotCompleted
                ElseIf rndNumber < 40 Then
                    listing = imgCompleted & imgCompleted
                ElseIf rndNumber < 60 Then
                    'dt = "" 'e.Day.Date.Day.ToString() & "<br/><br/><br/><a href=''>Task Name</a><br/><br/><a href=''>Task Name</a><br/><br/><a href=''>Task Name</a><br/><br/>"
                ElseIf rndNumber < 80 Then
                    'dt = e.Day.Date.Day.ToString() & "<br/><br/><br/><a href=''>Task Name</a><br/><br/><a href=''>Task Name</a><br/><br/><a href=''>Task Name</a><br/><br/>"
                Else
                    listing = imgWorkInProcess & imgCompleted & imgWorkInProcess
                End If

                Dim newDate As Date = New Date(Now.Year, Now.Month, 1)
                returnVal.Add(listing, newDate.AddDays(i).ToString)
            Next
        Catch ex As System.Exception
            IP.Bids.SharedFunctions.HandleError("GetTaskListing", "Error while getting the Task Listing", ex)
        End Try
        Return returnVal
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="statusID"></param>
    ''' <param name="includeLabel"></param>
    ''' <param name="dueDate"></param>
    ''' <param name="imagePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetTaskStatus(ByVal statusID As Integer, ByVal includeLabel As Boolean, Optional ByVal dueDate As String = "", Optional ByVal imagePath As String = "Images/") As String
        Dim returnVal As String = String.Empty
        Try
            Dim noWorkNeeded As String
            Dim cancelled As String
            Dim completed As String
            Dim lateNotCompleted As String
            Dim workInProcess As String
            If includeLabel = True Then
                noWorkNeeded = IP.Bids.SharedFunctions.LocalizeValue("No Work Needed", True)
                cancelled = IP.Bids.SharedFunctions.LocalizeValue("Cancelled", True)
                completed = IP.Bids.SharedFunctions.LocalizeValue("Completed", True)
                lateNotCompleted = IP.Bids.SharedFunctions.LocalizeValue("Late/Not Completed", True)
                workInProcess = IP.Bids.SharedFunctions.LocalizeValue("Open", True)
            Else
                noWorkNeeded = String.Empty
                cancelled = String.Empty
                completed = String.Empty
                lateNotCompleted = String.Empty
                workInProcess = String.Empty
            End If
            '            Dim p As New Page ' COMMENTED BY CODEIT.RIGHT
            '            Dim url As String = p.ResolveClientUrl("Images/noworkneeded.gif") ' COMMENTED BY CODEIT.RIGHT
            Dim imgNoWorkNeeded As String = "<img src='" & imagePath & String.Format("noworkneeded.gif' align=center width=15 height=15 title='{0}' alt='{0}'>{0}", noWorkNeeded)
            Dim imgCanceled As String = "<img src='" & imagePath & String.Format("cancelled.gif' align=center width=15 height=15 title='{0}' alt='{0}'>{0}", cancelled)
            Dim imgCompleted As String = "<img src='" & imagePath & String.Format("complete.gif' align=center width=15 height=15 title='{0}' alt='{0}'>{0}", completed)
            Dim imgLateNotCompleted As String = "<img src='" & imagePath & String.Format("late_notcomp.gif' align=center width=15 height=15  title='{0}' alt='{0}'/>{0}", lateNotCompleted)
            Dim imgWorkInProcess As String = "<img src='" & imagePath & String.Format("wip.gif' align=center width=15 height=15 title='{0}' alt='{0}'/>{0}", workInProcess)

            If dueDate.Length > 0 AndAlso IsDate(dueDate) Then
                If statusID = 1 And CDate(dueDate) < Now Then
                    statusID = 0
                End If
            End If
            Select Case statusID
                Case 0 'Overdue
                    Return imgLateNotCompleted
                Case 1 'Open
                    returnVal = imgWorkInProcess
                Case 2 'Complete
                    returnVal = imgCompleted
                Case 3 'No Work Needed
                    returnVal = imgNoWorkNeeded
                Case 4 'Cancelled
                    returnVal = imgCanceled
                Case Else
                    returnVal = ""
            End Select

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetTaskStatus", "Error getting the Task Status for Status ID[" & statusID & "]", ex)
        End Try
        Return returnVal
    End Function
End Class
