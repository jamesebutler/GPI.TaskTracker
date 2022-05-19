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
'Imports System.Data.OracleClient
Imports System.Globalization
Imports IP.TaskTrackerDAL
Imports System.Configuration
Imports Devart.Data.Oracle


Public NotInheritable Class EmailDataBll

    ''' <summary>
    ''' Sends out an Immediate Email for the specified Task
    ''' </summary>
    ''' <param name="taskItemNumber"></param>
    ''' <param name="TaskHeaderNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Shared Function CheckForTesting() As String

        CheckForTesting = ConfigurationManager.AppSettings("Testingsite")

        Return CheckForTesting

    End Function

    Public Shared Function URLTesting() As String

        URLTesting = ConfigurationManager.AppSettings("TestingURL")

        Return URLTesting

    End Function


    Public Shared Function GetAndSendImmediateEmail(ByVal taskItemNumber As Integer, ByVal taskHeaderNumber As Integer) As Boolean
        Dim adapter As TaskDetailTableAdapters.ImmediateEmailTableAdapter
        Dim table As TaskDetail.ImmediateEmailDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim retVal As Boolean
        Dim taskHeader As TaskHeaderBll = Nothing
        Dim refSite As String = String.Empty


        Try

            taskHeader = New TaskHeaderBll(taskHeaderNumber)
            If taskHeader IsNot Nothing Then
                Select Case taskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID
                    Case IP.Bids.SharedFunctions.SourceSystemID.MOC
                        refSite = "&refsite=MOC"
                    Case IP.Bids.SharedFunctions.SourceSystemID.ReliabilityIncident
                        refSite = "&refsite=RI"
                    Case IP.Bids.SharedFunctions.SourceSystemID.IRIS
                        refSite = "&refsite=IRIS"
                End Select
            End If
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.ImmediateEmailTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetImmediateEmail(CStr(taskItemNumber), emptyCursor)
                If table IsNot Nothing AndAlso table.Rows.Count > 0 Then

                    For Each rowItem As TaskDetail.ImmediateEmailRow In table.Rows
                        With rowItem
                            If .IsACTIVITYNAMENull Then
                                .ACTIVITYNAME = String.Empty
                            End If
                            If .IsBUSUNITMGRNull Then
                                .BUSUNITMGR = String.Empty
                            End If
                            If .IsCREATEDBY_DEFAULTLANGUAGENull Then
                                .CREATEDBY_DEFAULTLANGUAGE = "EN-US"
                            End If
                            If .IsCREATEDBY_EMAILNull Then
                                .CREATEDBY_EMAIL = String.Empty
                            End If
                            If .IsCREATEDBYNull Then
                                .CREATEDBY = String.Empty
                            End If
                            If .IsHEADERTYPENAMENull Then
                                .HEADERTYPENAME = String.Empty
                            End If
                            If .IsITEM_CLOSEDDATENull Then

                            End If
                            If .IsITEM_DESCRIPTIONNull Then
                                .ITEM_DESCRIPTION = String.Empty
                            End If
                            If .IsITEM_DUEDATENull Then
                                '.ITEM_DUEDATE 
                            End If
                            If .IsITEM_TITLENull Then
                                .ITEM_TITLE = String.Empty
                            End If
                            If .IsLEADTIMENull Then
                                .LEADTIME = 1
                            End If
                            If .IsMGRNull Then
                                .MGR = String.Empty
                            End If
                            If .IsMTTCOMMENTNull Then
                                .MTTCOMMENT = String.Empty
                            End If
                            If .IsRECNUMNull Then
                                .RECNUM = 1
                            End If
                            If .IsRECTYPENull Then
                                .RECTYPE = "Entered"
                            End If
                            If .IsRESPONSIBLE_DEFAULTLANGUAGENull Then
                                .RESPONSIBLE_DEFAULTLANGUAGE = "EN-US"
                            End If
                            If .IsRESPONSIBLE_EMAILNull Then
                                .RESPONSIBLE_EMAIL = String.Empty
                            End If
                            If .IsRESPONSIBLE_ROLE_NAMESNull Then
                                .RESPONSIBLE_ROLE_NAMES = String.Empty
                            End If
                            If .IsRESPONSIBLEUSERNAMENull Then
                                .RESPONSIBLEUSERNAME = String.Empty
                            End If
                            If .IsROLEDESCRIPTIONNull Then
                                .ROLEDESCRIPTION = String.Empty
                            End If
                            If .IsSITENAMENull Then
                                .SITENAME = String.Empty
                            End If
                            If .IsTASKHEADERTITLENull Then
                                .TASKHEADERTITLE = String.Empty
                            End If
                            If .IsTYPEMGRNull Then
                                .TYPEMGR = String.Empty
                            End If
                            If .IsWHOLE_NAME_CREATEDBY_PERSONNull Then
                                .WHOLE_NAME_CREATEDBY_PERSON = String.Empty
                            End If
                            If .IsWHOLE_NAME_RESPONSIBLE_PERSONNull Then
                                .WHOLE_NAME_RESPONSIBLE_PERSON = String.Empty
                            End If

                        End With

                        Dim v_td As String() = {"<TD>", "</TD>"}
                        Dim sbEmailBody As New System.Text.StringBuilder
                        Dim strDueDate As String
                        Dim strEmailAddress As String
                        Dim strRecType As String
                        Dim strSiteName As String
                        Dim strTitle As String
                        Dim strDescription As String
                        Dim strResponsible As String
                        'Dim strBusUnitMgr As String
                        Dim strCreatedBy As String
                        Dim strTaskID As String
                        Dim strHeaderTitle As String
                        Dim strActivity As String
                        Dim strComments As String
                        Dim strTaskHeaderID As String
                        Dim strTaskItemID As String
                        Dim strSubject As String
                        Dim strMsg As String
                        Dim strBody As String = ""
                        Dim strFooter As String = ""
                        Dim strHeading As String = ""
                        Dim strDB As String
                        'Dim ipLoc As New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization(rowItem.RESPONSIBLE_DEFAULTLANGUAGE, "MTT")

                        'ipLoc = IP.Bids.SharedFunctions.LocalizeValue
                        strDB = IP.Bids.SharedFunctions.GetServerName

                        sbEmailBody = New System.Text.StringBuilder
                        strEmailAddress = rowItem.RESPONSIBLE_EMAIL

                        strRecType = IP.Bids.SharedFunctions.LocalizeValue(rowItem.RECTYPE, True)

                        If rowItem.RESPONSIBLE_DEFAULTLANGUAGE.ToUpper <> "EN-US" Then
                            strSiteName = IP.Bids.SharedFunctions.LocalizeValue(rowItem.SITENAME, True)  'Cannot be NULL
                        Else
                            strSiteName = rowItem.SITENAME
                        End If

                        strDueDate = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(rowItem.ITEM_DUEDATE, rowItem.RESPONSIBLE_DEFAULTLANGUAGE, "MMMM dd, yyyy")
                        strTitle = rowItem.ITEM_TITLE

                        If rowItem.WHOLE_NAME_RESPONSIBLE_PERSON.Trim.Length = 0 Then
                            strResponsible = IP.Bids.SharedFunctions.LocalizeValue(rowItem.ROLEDESCRIPTION, True) & " (" & rowItem.RESPONSIBLE_ROLE_NAMES & ")"
                        Else
                            strResponsible = rowItem.WHOLE_NAME_RESPONSIBLE_PERSON
                        End If

                        'strBusUnitMgr = rowItem.MGR
                        strTaskID = rowItem.TASKITEMSEQID
                        strCreatedBy = rowItem.WHOLE_NAME_CREATEDBY_PERSON
                        strHeaderTitle = rowItem.TASKHEADERTITLE
                        strDescription = rowItem.ITEM_DESCRIPTION

                        strActivity = rowItem.ACTIVITYNAME 'This needs to be localized within the stored procedure
                        'strActivity = IP.Bids.SharedFunctions.LocalizeValue(rowItem.ACTIVITYNAME, True)
                        strComments = rowItem.MTTCOMMENT

                        strTaskHeaderID = rowItem.TASKHEADERSEQID
                        strTaskItemID = rowItem.TASKITEMSEQID

                        sbEmailBody.Append("<P><font size =2 face=Arial><B><U>" & strRecType & "</B></U></FONT><BR>")
                        sbEmailBody.Append("<TABLE border=1 cellpadding='2' cellspacing='0' style='border-color:black' width='100%'><font size =2 face=Arial><TR valign=top><B><TD width=10%>" & IP.Bids.SharedFunctions.LocalizeValue("Due Date", True) & "{1}<TD width=20%>" & IP.Bids.SharedFunctions.LocalizeValue("Header Info", True) & "{1}<TD width=20%>" & IP.Bids.SharedFunctions.LocalizeValue("Title", True) & "{1}")
                        sbEmailBody.Append("{0}" & IP.Bids.SharedFunctions.LocalizeValue("Description", True) & "{1}<TD width=10% wrap=hard>" & IP.Bids.SharedFunctions.LocalizeValue("Comments", True) & "{1}{0}" & IP.Bids.SharedFunctions.LocalizeValue("Created By", True) & "{1}")
                        sbEmailBody.Append("</B></TR>")
                        sbEmailBody.Append("<BR><TR valign=top><font size=2>{0}" & strDueDate & "{1}")
                        sbEmailBody.Append("{0}" & strHeaderTitle & " (" & strActivity & "){1}")
                        If CheckForTesting() = "yes" Then
                            ' sbEmailBody.Append("{0}<A HREF=HTTP://gpitasktracker.graphicpkg.com:130/TaskDetails.aspx?HeaderNumber=" & strTaskHeaderID & "&TaskNumber=" & strTaskID & refSite & ">" & strTitle & "</A>{1}")
                            Dim URLTest As String = URLTesting()
                            sbEmailBody.Append("{0}<A HREF=" & URLTest & "/TaskDetails.aspx?HeaderNumber=" & strTaskHeaderID & "&TaskNumber=" & strTaskID & refSite & ">" & strTitle & "</A>{1}")
                        Else
                            sbEmailBody.Append("{0}<A HREF=HTTP://gpitasktracker.graphicpkg.com/TaskDetails.aspx?HeaderNumber=" & strTaskHeaderID & "&TaskNumber=" & strTaskID & refSite & ">" & strTitle & "</A>{1}")
                        End If

                        sbEmailBody.Append("{0}" & strDescription & "{1}")
                        sbEmailBody.Append("{0}" & strComments & "{1}")
                        sbEmailBody.Append("{0}" & strCreatedBy & "{1}")
                        sbEmailBody.Append("</TR></TABLE>")
                        strMsg = sbEmailBody.ToString
                        strMsg = String.Format(CultureInfo.CurrentCulture, strMsg, v_td)
                        strSubject = IP.Bids.SharedFunctions.LocalizeValue("Manufacturing Task Tracker tasks that were entered that you are responsible for.", True)
                        strHeading = "<HTML><BODY><font size=2 face=Arial><B>" & IP.Bids.SharedFunctions.LocalizeValue("MTTResponsibleEmail", True) & "</B>" '"Here are the tasks from Manufacturing Task Tracker that were entered today that you are responsible for.  Click Title to view or update (assign to another person, add comments, complete task by entering the closed date)."
                        strFooter = "</HTML></BODY>"

                        strBody = strHeading & "<BR>" & strMsg.ToString & strFooter
                        strBody = IP.Bids.SharedFunctions.CleanString(strBody, "<br>")
                        IP.Bids.SharedFunctions.SendEmail(strEmailAddress, "manufacturing.task@graphicpkg.com", strSubject, strBody)

                        strBody = ""
                        retVal = True
                    Next
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetAndSendImmediateEmail", "Attempting to send an email for TaskHeader=" & taskHeaderNumber & ", TaskItemNumber=" & taskItemNumber, ex)
            retVal = False
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return retVal
        adapter = Nothing
    End Function

    ''' <summary>
    ''' Sends out an Immediate Email for the specified Task
    ''' </summary>
    ''' <param name="taskItemNumber"></param>
    ''' <param name="TaskHeaderNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAndSendSubsequentTasksImmediateEmail(ByVal taskItemNumber As Integer, ByVal TaskHeaderNumber As Integer, ByVal dueDate As DateTime) As Boolean
        Dim adapter As TaskDetailTableAdapters.ImmediateEmailTableAdapter
        Dim table As TaskDetail.ImmediateEmailDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim retVal As Boolean
        Dim taskHeader As TaskHeaderBll = Nothing
        Dim refSite As String = String.Empty

        Try

            taskHeader = New TaskHeaderBll(TaskHeaderNumber)
            If taskHeader IsNot Nothing Then
                Select Case taskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID
                    Case IP.Bids.SharedFunctions.SourceSystemID.MOC
                        refSite = "&refsite=MOC"
                    Case IP.Bids.SharedFunctions.SourceSystemID.ReliabilityIncident
                        refSite = "&refsite=RI"
                    Case IP.Bids.SharedFunctions.SourceSystemID.IRIS
                        refSite = "&refsite=IRIS"
                    Case IP.Bids.SharedFunctions.SourceSystemID.Tanks
                        refSite = "&refsite=TANKS"
                End Select
            End If
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New IP.TaskTrackerDAL.TaskDetailTableAdapters.ImmediateEmailTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetImmediateEmail(CStr(taskItemNumber), emptyCursor)
                If table IsNot Nothing AndAlso table.Rows.Count > 0 Then

                    For Each rowItem As TaskDetail.ImmediateEmailRow In table.Rows
                        With rowItem
                            If .IsACTIVITYNAMENull Then
                                .ACTIVITYNAME = String.Empty
                            End If
                            If .IsBUSUNITMGRNull Then
                                .BUSUNITMGR = String.Empty
                            End If
                            If .IsCREATEDBY_DEFAULTLANGUAGENull Then
                                .CREATEDBY_DEFAULTLANGUAGE = "EN-US"
                            End If
                            If .IsCREATEDBY_EMAILNull Then
                                .CREATEDBY_EMAIL = String.Empty
                            End If
                            If .IsCREATEDBYNull Then
                                .CREATEDBY = String.Empty
                            End If
                            If .IsHEADERTYPENAMENull Then
                                .HEADERTYPENAME = String.Empty
                            End If
                            If .IsITEM_CLOSEDDATENull Then

                            End If
                            If .IsITEM_DESCRIPTIONNull Then
                                .ITEM_DESCRIPTION = String.Empty
                            End If
                            If .IsITEM_DUEDATENull Then
                                .ITEM_DUEDATE = dueDate
                            End If
                            If .IsITEM_TITLENull Then
                                .ITEM_TITLE = String.Empty
                            End If
                            If .IsLEADTIMENull Then
                                .LEADTIME = 1
                            End If
                            If .IsMGRNull Then
                                .MGR = String.Empty
                            End If
                            If .IsMTTCOMMENTNull Then
                                .MTTCOMMENT = String.Empty
                            End If
                            If .IsRECNUMNull Then
                                .RECNUM = 1
                            End If
                            If .IsRECTYPENull Then
                                .RECTYPE = "Entered"
                            End If
                            If .IsRESPONSIBLE_DEFAULTLANGUAGENull Then
                                .RESPONSIBLE_DEFAULTLANGUAGE = "EN-US"
                            End If
                            If .IsRESPONSIBLE_EMAILNull Then
                                .RESPONSIBLE_EMAIL = String.Empty
                            End If
                            If .IsRESPONSIBLE_ROLE_NAMESNull Then
                                .RESPONSIBLE_ROLE_NAMES = String.Empty
                            End If
                            If .IsRESPONSIBLEUSERNAMENull Then
                                .RESPONSIBLEUSERNAME = String.Empty
                            End If
                            If .IsROLEDESCRIPTIONNull Then
                                .ROLEDESCRIPTION = String.Empty
                            End If
                            If .IsSITENAMENull Then
                                .SITENAME = String.Empty
                            End If
                            If .IsTASKHEADERTITLENull Then
                                .TASKHEADERTITLE = String.Empty
                            End If
                            If .IsTYPEMGRNull Then
                                .TYPEMGR = String.Empty
                            End If
                            If .IsWHOLE_NAME_CREATEDBY_PERSONNull Then
                                .WHOLE_NAME_CREATEDBY_PERSON = String.Empty
                            End If
                            If .IsWHOLE_NAME_RESPONSIBLE_PERSONNull Then
                                .WHOLE_NAME_RESPONSIBLE_PERSON = String.Empty
                            End If

                        End With

                        Dim v_td As String() = {"<TD>", "</TD>"}
                        Dim sbEmailBody As New System.Text.StringBuilder
                        Dim strDueDate As String
                        Dim strEmailAddress As String
                        Dim strRecType As String
                        Dim strSiteName As String
                        Dim strTitle As String
                        Dim strDescription As String
                        Dim strResponsible As String
                        'Dim strBusUnitMgr As String
                        Dim strCreatedBy As String
                        Dim strTaskID As String
                        Dim strHeaderTitle As String
                        Dim strActivity As String
                        Dim strComments As String
                        Dim strTaskHeaderID As String
                        Dim strTaskItemID As String
                        Dim strSubject As String
                        Dim strMsg As String
                        Dim strBody As String = ""
                        Dim strFooter As String = ""
                        Dim strHeading As String = ""
                        Dim strDB As String
                        'Dim ipLoc As New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization(rowItem.RESPONSIBLE_DEFAULTLANGUAGE, "MTT")

                        'ipLoc = IP.Bids.SharedFunctions.LocalizeValue
                        strDB = IP.Bids.SharedFunctions.GetServerName

                        sbEmailBody = New System.Text.StringBuilder
                        strEmailAddress = rowItem.RESPONSIBLE_EMAIL

                        strRecType = IP.Bids.SharedFunctions.LocalizeValue(rowItem.RECTYPE, True)

                        If rowItem.RESPONSIBLE_DEFAULTLANGUAGE.ToUpper <> "EN-US" Then
                            strSiteName = IP.Bids.SharedFunctions.LocalizeValue(rowItem.SITENAME, True)  'Cannot be NULL
                        Else
                            strSiteName = rowItem.SITENAME
                        End If

                        strDueDate = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(rowItem.ITEM_DUEDATE, rowItem.RESPONSIBLE_DEFAULTLANGUAGE, "MMMM dd, yyyy")
                        strTitle = rowItem.ITEM_TITLE

                        If rowItem.WHOLE_NAME_RESPONSIBLE_PERSON.Trim.Length = 0 Then
                            strResponsible = IP.Bids.SharedFunctions.LocalizeValue(rowItem.ROLEDESCRIPTION, True) & " (" & rowItem.RESPONSIBLE_ROLE_NAMES & ")"
                        Else
                            strResponsible = rowItem.WHOLE_NAME_RESPONSIBLE_PERSON
                        End If

                        'strBusUnitMgr = rowItem.MGR
                        strTaskID = rowItem.TASKITEMSEQID
                        strCreatedBy = rowItem.WHOLE_NAME_CREATEDBY_PERSON
                        strHeaderTitle = rowItem.TASKHEADERTITLE
                        strDescription = rowItem.ITEM_DESCRIPTION

                        strActivity = rowItem.ACTIVITYNAME 'This needs to be localized within the stored procedure
                        'strActivity = IP.Bids.SharedFunctions.LocalizeValue(rowItem.ACTIVITYNAME, True)
                        strComments = rowItem.MTTCOMMENT

                        strTaskHeaderID = rowItem.TASKHEADERSEQID
                        strTaskItemID = rowItem.TASKITEMSEQID

                        sbEmailBody.Append("<P><font size =2 face=Arial><B><U>" & strRecType & "</B></U></FONT><BR>")
                        sbEmailBody.Append("<TABLE border=1 cellpadding='2' cellspacing='0' style='border-color:black' width='100%'><font size =2 face=Arial><TR valign=top><B><TD width=10%>" & IP.Bids.SharedFunctions.LocalizeValue("Due Date", True) & "{1}<TD width=20%>" & IP.Bids.SharedFunctions.LocalizeValue("Header Info", True) & "{1}<TD width=20%>" & IP.Bids.SharedFunctions.LocalizeValue("Title", True) & "{1}")
                        sbEmailBody.Append("{0}" & IP.Bids.SharedFunctions.LocalizeValue("Description", True) & "{1}<TD width=10% wrap=hard>" & IP.Bids.SharedFunctions.LocalizeValue("Comments", True) & "{1}{0}" & IP.Bids.SharedFunctions.LocalizeValue("Created By", True) & "{1}")
                        sbEmailBody.Append("</B></TR>")
                        sbEmailBody.Append("<BR><TR valign=top><font size=2>{0}" & strDueDate & "{1}")
                        sbEmailBody.Append("{0}" & strHeaderTitle & " (" & strActivity & "){1}")

                        If CheckForTesting() = "yes" Then
                            sbEmailBody.Append("{0}<A HREF=HTTP://gpitasktracker.graphicpkg.com:130/TaskDetails.aspx?HeaderNumber=" & strTaskHeaderID & "&TaskNumber=" & strTaskID & refSite & ">" & strTitle & "</A>{1}")
                        Else
                            sbEmailBody.Append("{0}<A HREF=HTTP://gpitasktracker.graphicpkg.com/TaskDetails.aspx?HeaderNumber=" & strTaskHeaderID & "&TaskNumber=" & strTaskID & refSite & ">" & strTitle & "</A>{1}")
                        End If


                        sbEmailBody.Append("{0}" & strDescription & "{1}")
                        sbEmailBody.Append("{0}" & strComments & "{1}")
                        sbEmailBody.Append("{0}" & strCreatedBy & "{1}")
                        sbEmailBody.Append("</TR></TABLE>")
                        strMsg = sbEmailBody.ToString
                        strMsg = String.Format(CultureInfo.CurrentCulture, strMsg, v_td)
                        strSubject = IP.Bids.SharedFunctions.LocalizeValue("Manufacturing Task Tracker tasks that were entered that you are responsible for.", True)
                        strHeading = "<HTML><BODY><font size=2 face=Arial><B>" & IP.Bids.SharedFunctions.LocalizeValue("MTTResponsibleEmail", True) & "</B>" '"Here are the tasks from Manufacturing Task Tracker that were entered today that you are responsible for.  Click Title to view or update (assign to another person, add comments, complete task by entering the closed date)."
                        strFooter = "</HTML></BODY>"

                        strBody = strHeading & "<BR>" & strMsg.ToString & strFooter
                        strBody = IP.Bids.SharedFunctions.CleanString(strBody, "<br>")
                        IP.Bids.SharedFunctions.SendEmail(strEmailAddress, "manufacturing.task@graphicpkg.com", strSubject, strBody)

                        strBody = ""
                        retVal = True
                    Next
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetAndSendSubsequentTasksImmediateEmail", "Attempting to send an email for TaskHeader=" & TaskHeaderNumber & ", TaskItemNumber=" & taskItemNumber, ex)
            retVal = False
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return retVal
        adapter = Nothing
    End Function
    ''' <summary>
    ''' Sends out an Immediate Email for the Reassigned Tasks
    ''' </summary>
    ''' <param name="TaskList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAndSendReassignedEmail(ByVal TaskList As Hashtable) As Boolean
        Dim adapter As TaskDetailTableAdapters.ImmediateEmailTableAdapter
        Dim table As TaskDetail.ImmediateEmailDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim retVal As Boolean
        Dim taskHeader As TaskHeaderBll = Nothing
        Dim refSite As String = String.Empty
        Dim taskitemkey As String = String.Empty
        Dim taskheaderkey As String = String.Empty
        Dim strSubject As String
        Dim strMsg As String
        Dim strBody As String = ""
        Dim strFooter As String = ""
        Dim strHeading As String = ""
        Dim strDB As String
        Dim sbEmailBody As New System.Text.StringBuilder
        Dim strEmailAddress As String = ""
        Dim v_td As String() = {"<TD>", "</TD>"}
        Dim strRecType As String = "Reassigned"

        sbEmailBody = New System.Text.StringBuilder
        sbEmailBody.Append("<P><font size =2 face=Arial><B><U>" & strRecType & "</B></U></FONT><BR>")
        sbEmailBody.Append("<TABLE border=1 cellpadding='2' cellspacing='0' style='border-color:black' width='100%'><font size =2 face=Arial><TR valign=top><B><TD width=10%>" & IP.Bids.SharedFunctions.LocalizeValue("Due Date", True) & "{1}<TD width=20%>" & IP.Bids.SharedFunctions.LocalizeValue("Header Info", True) & "{1}<TD width=20%>" & IP.Bids.SharedFunctions.LocalizeValue("Title", True) & "{1}")
        sbEmailBody.Append("{0}" & IP.Bids.SharedFunctions.LocalizeValue("Description", True) & "{1}<TD width=10% wrap=hard>" & IP.Bids.SharedFunctions.LocalizeValue("Comments", True) & "{1}{0}" & IP.Bids.SharedFunctions.LocalizeValue("Created By", True) & "{1}")
        sbEmailBody.Append("</B></TR>")

        Try
            For Each de As DictionaryEntry In TaskList

                taskheaderkey = de.Value
                taskitemkey = de.Key
                taskHeader = New TaskHeaderBll(taskheaderkey)
                'If taskHeader IsNot Nothing Then
                '    Select Case taskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID
                '        Case IP.Bids.SharedFunctions.SourceSystemID.MOC
                '            refSite = "&refsite=MOC"
                '        Case IP.Bids.SharedFunctions.SourceSystemID.ReliabilityIncident
                '            refSite = "&refsite=RI"
                '        Case IP.Bids.SharedFunctions.SourceSystemID.IRIS
                '            refSite = "&refsite=IRIS"
                '    End Select
                'End If
                ' If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.ImmediateEmailTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetImmediateEmail(CStr(taskitemkey), emptyCursor)
                If table IsNot Nothing AndAlso table.Rows.Count > 0 Then

                    For Each rowItem As TaskDetail.ImmediateEmailRow In table.Rows
                        With rowItem
                            If .IsACTIVITYNAMENull Then
                                .ACTIVITYNAME = String.Empty
                            End If
                            If .IsBUSUNITMGRNull Then
                                .BUSUNITMGR = String.Empty
                            End If
                            If .IsCREATEDBY_DEFAULTLANGUAGENull Then
                                .CREATEDBY_DEFAULTLANGUAGE = "EN-US"
                            End If
                            If .IsCREATEDBY_EMAILNull Then
                                .CREATEDBY_EMAIL = String.Empty
                            End If
                            If .IsCREATEDBYNull Then
                                .CREATEDBY = String.Empty
                            End If
                            If .IsHEADERTYPENAMENull Then
                                .HEADERTYPENAME = String.Empty
                            End If
                            If .IsITEM_CLOSEDDATENull Then

                            End If
                            If .IsITEM_DESCRIPTIONNull Then
                                .ITEM_DESCRIPTION = String.Empty
                            End If
                            If .IsITEM_DUEDATENull Then
                                '.ITEM_DUEDATE 
                            End If
                            If .IsITEM_TITLENull Then
                                .ITEM_TITLE = String.Empty
                            End If
                            If .IsLEADTIMENull Then
                                .LEADTIME = 1
                            End If
                            If .IsMGRNull Then
                                .MGR = String.Empty
                            End If
                            If .IsMTTCOMMENTNull Then
                                .MTTCOMMENT = String.Empty
                            End If
                            If .IsRECNUMNull Then
                                .RECNUM = 1
                            End If
                            If .IsRECTYPENull Then
                                .RECTYPE = "Entered"
                            End If
                            If .IsRESPONSIBLE_DEFAULTLANGUAGENull Then
                                .RESPONSIBLE_DEFAULTLANGUAGE = "EN-US"
                            End If
                            If .IsRESPONSIBLE_EMAILNull Then
                                .RESPONSIBLE_EMAIL = String.Empty
                            End If
                            If .IsRESPONSIBLE_ROLE_NAMESNull Then
                                .RESPONSIBLE_ROLE_NAMES = String.Empty
                            End If
                            If .IsRESPONSIBLEUSERNAMENull Then
                                .RESPONSIBLEUSERNAME = String.Empty
                            End If
                            If .IsROLEDESCRIPTIONNull Then
                                .ROLEDESCRIPTION = String.Empty
                            End If
                            If .IsSITENAMENull Then
                                .SITENAME = String.Empty
                            End If
                            If .IsTASKHEADERTITLENull Then
                                .TASKHEADERTITLE = String.Empty
                            End If
                            If .IsTYPEMGRNull Then
                                .TYPEMGR = String.Empty
                            End If
                            If .IsWHOLE_NAME_CREATEDBY_PERSONNull Then
                                .WHOLE_NAME_CREATEDBY_PERSON = String.Empty
                            End If
                            If .IsWHOLE_NAME_RESPONSIBLE_PERSONNull Then
                                .WHOLE_NAME_RESPONSIBLE_PERSON = String.Empty
                            End If

                        End With



                        Dim strDueDate As String
                        'Dim strRecType As String
                        Dim strSiteName As String
                        Dim strTitle As String
                        Dim strDescription As String
                        Dim strResponsible As String
                        'Dim strBusUnitMgr As String
                        Dim strCreatedBy As String
                        Dim strTaskID As String
                        Dim strHeaderTitle As String
                        Dim strActivity As String
                        Dim strComments As String
                        Dim strTaskHeaderID As String
                        Dim strTaskItemID As String


                        'Dim ipLoc As New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization(rowItem.RESPONSIBLE_DEFAULTLANGUAGE, "MTT")

                        'ipLoc = IP.Bids.SharedFunctions.LocalizeValue
                        strDB = IP.Bids.SharedFunctions.GetServerName


                        strEmailAddress = rowItem.RESPONSIBLE_EMAIL

                        strRecType = IP.Bids.SharedFunctions.LocalizeValue(rowItem.RECTYPE, True)

                        If rowItem.RESPONSIBLE_DEFAULTLANGUAGE.ToUpper <> "EN-US" Then
                            strSiteName = IP.Bids.SharedFunctions.LocalizeValue(rowItem.SITENAME, True)  'Cannot be NULL
                        Else
                            strSiteName = rowItem.SITENAME
                        End If

                        strDueDate = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(rowItem.ITEM_DUEDATE, rowItem.RESPONSIBLE_DEFAULTLANGUAGE, "MMMM dd, yyyy")
                        strTitle = rowItem.ITEM_TITLE

                        If rowItem.WHOLE_NAME_RESPONSIBLE_PERSON.Trim.Length = 0 Then
                            strResponsible = IP.Bids.SharedFunctions.LocalizeValue(rowItem.ROLEDESCRIPTION, True) & " (" & rowItem.RESPONSIBLE_ROLE_NAMES & ")"
                        Else
                            strResponsible = rowItem.WHOLE_NAME_RESPONSIBLE_PERSON
                        End If

                        'strBusUnitMgr = rowItem.MGR
                        strTaskID = rowItem.TASKITEMSEQID
                        strCreatedBy = rowItem.WHOLE_NAME_CREATEDBY_PERSON
                        strHeaderTitle = rowItem.TASKHEADERTITLE
                        strDescription = rowItem.ITEM_DESCRIPTION

                        strActivity = rowItem.ACTIVITYNAME 'This needs to be localized within the stored procedure
                        'strActivity = IP.Bids.SharedFunctions.LocalizeValue(rowItem.ACTIVITYNAME, True)
                        strComments = rowItem.MTTCOMMENT

                        strTaskHeaderID = rowItem.TASKHEADERSEQID
                        strTaskItemID = rowItem.TASKITEMSEQID
                        '

                        sbEmailBody.Append("<BR><TR valign=top><font size=2>{0}" & strDueDate & "{1}")
                        sbEmailBody.Append("{0}" & strHeaderTitle & " (" & strActivity & "){1}")

                        If CheckForTesting() = "yes" Then
                            sbEmailBody.Append("{0}<A HREF=HTTP://gpitasktracker.graphicpkg.com:130/TaskDetails.aspx?HeaderNumber=" & strTaskHeaderID & "&TaskNumber=" & strTaskID & refSite & ">" & strTitle & "</A>{1}")
                        Else
                            sbEmailBody.Append("{0}<A HREF=HTTP://gpitasktracker.graphicpkg.com/TaskDetails.aspx?HeaderNumber=" & strTaskHeaderID & "&TaskNumber=" & strTaskID & refSite & ">" & strTitle & "</A>{1}")
                        End If


                        sbEmailBody.Append("{0}" & strDescription & "{1}")
                        sbEmailBody.Append("{0}" & strComments & "{1}")
                        sbEmailBody.Append("{0}" & strCreatedBy & "{1}")
                        sbEmailBody.Append("</TR>")
                    Next
                    ' End If
                End If
            Next

            sbEmailBody.Append("</TABLE>")
            strMsg = sbEmailBody.ToString
            strMsg = String.Format(CultureInfo.CurrentCulture, strMsg, v_td)
            strSubject = IP.Bids.SharedFunctions.LocalizeValue("Manufacturing Task Tracker tasks that were reassigned that you are responsible for.", True)
            strHeading = "<HTML><BODY><font size=2 face=Arial><B>" & IP.Bids.SharedFunctions.LocalizeValue("MTTResponsibleEmail", True) & "</B>" '"Here are the tasks from Manufacturing Task Tracker that were entered today that you are responsible for.  Click Title to view or update (assign to another person, add comments, complete task by entering the closed date)."
            strFooter = "</HTML></BODY>"

            strBody = strHeading & "<BR>" & strMsg.ToString & strFooter
            strBody = IP.Bids.SharedFunctions.CleanString(strBody, "<br>")
            IP.Bids.SharedFunctions.SendEmail(strEmailAddress, "manufacturing.task@graphicpkg.com", strSubject, strBody)

            strBody = ""
            retVal = True

        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetAndSendReassignedEmail", "Attempting to send an email for TaskHeader=" & taskheaderkey & ", TaskItemNumber=" & taskitemkey, ex)
            retVal = False
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return retVal
        adapter = Nothing
    End Function
    'Public Shared Function GetAndSendImmediateEmail(ByVal taskItemNumber As Integer) As Boolean
    '    Dim adapter As TaskDetailTableAdapters.ImmediateEmailTableAdapter
    '    Dim table As TaskDetail.ImmediateEmailDataTable = Nothing
    '    Dim emptyCursor As Object = Nothing
    '    '        Dim record As TaskItem = Nothing ' COMMENTED BY CODEIT.RIGHT
    '    Dim retVal As Boolean

    '    Try
    '        If table Is Nothing OrElse table.Rows.Count = 0 Then
    '            adapter = New TaskDetailTableAdapters.ImmediateEmailTableAdapter
    '            table = adapter.GetImmediateEmail(CStr(taskItemNumber), emptyCursor)
    '            If table IsNot Nothing AndAlso table.Rows.Count > 0 Then

    '                For Each rowItem As TaskDetail.ImmediateEmailRow In table.Rows
    '                    With rowItem
    '                        If .IsACTIVITYNAMENull Then
    '                            .ACTIVITYNAME = String.Empty
    '                        End If
    '                        If .IsBUSUNITMGRNull Then
    '                            .BUSUNITMGR = String.Empty
    '                        End If
    '                        If .IsCREATEDBY_DEFAULTLANGUAGENull Then
    '                            .CREATEDBY_DEFAULTLANGUAGE = "EN-US"
    '                        End If
    '                        If .IsCREATEDBY_EMAILNull Then
    '                            .CREATEDBY_EMAIL = String.Empty
    '                        End If
    '                        If .IsCREATEDBYNull Then
    '                            .CREATEDBY = String.Empty
    '                        End If
    '                        If .IsHEADERTYPENAMENull Then
    '                            .HEADERTYPENAME = String.Empty
    '                        End If
    '                        If .IsITEM_CLOSEDDATENull Then

    '                        End If
    '                        If .IsITEM_DESCRIPTIONNull Then
    '                            .ITEM_DESCRIPTION = String.Empty
    '                        End If
    '                        If .IsITEM_DUEDATENull Then
    '                            '.ITEM_DUEDATE 
    '                        End If
    '                        If .IsITEM_TITLENull Then
    '                            .ITEM_TITLE = String.Empty
    '                        End If
    '                        If .IsLEADTIMENull Then
    '                            .LEADTIME = 1
    '                        End If
    '                        If .IsMGRNull Then
    '                            .MGR = String.Empty
    '                        End If
    '                        If .IsMTTCOMMENTNull Then
    '                            .MTTCOMMENT = String.Empty
    '                        End If
    '                        If .IsRECNUMNull Then
    '                            .RECNUM = 1
    '                        End If
    '                        If .IsRECTYPENull Then
    '                            .RECTYPE = "Entered"
    '                        End If
    '                        If .IsRESPONSIBLE_DEFAULTLANGUAGENull Then
    '                            .RESPONSIBLE_DEFAULTLANGUAGE = "EN-US"
    '                        End If
    '                        If .IsRESPONSIBLE_EMAILNull Then
    '                            .RESPONSIBLE_EMAIL = String.Empty
    '                        End If
    '                        If .IsRESPONSIBLE_ROLE_NAMESNull Then
    '                            .RESPONSIBLE_ROLE_NAMES = String.Empty
    '                        End If
    '                        If .IsRESPONSIBLEUSERNAMENull Then
    '                            .RESPONSIBLEUSERNAME = String.Empty
    '                        End If
    '                        If .IsROLEDESCRIPTIONNull Then
    '                            .ROLEDESCRIPTION = String.Empty
    '                        End If
    '                        If .IsSITENAMENull Then
    '                            .SITENAME = String.Empty
    '                        End If
    '                        If .IsTASKHEADERTITLENull Then
    '                            .TASKHEADERTITLE = String.Empty
    '                        End If
    '                        If .IsTYPEMGRNull Then
    '                            .TYPEMGR = String.Empty
    '                        End If
    '                        If .IsWHOLE_NAME_CREATEDBY_PERSONNull Then
    '                            .WHOLE_NAME_CREATEDBY_PERSON = String.Empty
    '                        End If
    '                        If .IsWHOLE_NAME_RESPONSIBLE_PERSONNull Then
    '                            .WHOLE_NAME_RESPONSIBLE_PERSON = String.Empty
    '                        End If

    '                    End With

    '                    Dim v_td As String() = {"<TD>", "</TD>"}
    '                    Dim sbEmailBody As New System.Text.StringBuilder
    '                    Dim strDueDate As String
    '                    Dim strEmailAddress As String
    '                    Dim strRecType As String
    '                    Dim strSiteName As String
    '                    Dim strTitle As String
    '                    Dim strResponsible As String
    '                    Dim strBusUnitMgr As String
    '                    Dim strTaskID As String
    '                    Dim strHeaderTitle As String
    '                    Dim strActivity As String
    '                    Dim strComments As String
    '                    Dim strTaskHeaderID As String
    '                    Dim strTaskItemID As String
    '                    Dim strSubject As String
    '                    Dim strMsg As String
    '                    Dim strBody As String = ""
    '                    Dim strFooter As String = ""
    '                    Dim strHeading As String = ""
    '                    Dim strDB As String
    '                    Dim ipLoc As New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization(rowItem.RESPONSIBLE_DEFAULTLANGUAGE, "RI")



    '                    strDueDate = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(rowItem.ITEM_DUEDATE, rowItem.RESPONSIBLE_DEFAULTLANGUAGE, "d")
    '                    strTitle = IP.Bids.SharedFunctions.LocalizeValue (rowItem.ITEM_TITLE)

    '                    If rowItem.WHOLE_NAME_RESPONSIBLE_PERSON.Trim.Length = 0 Then
    '                        strResponsible = rowItem.ROLEDESCRIPTION & " (" & rowItem.RESPONSIBLE_ROLE_NAMES & ")"
    '                    Else
    '                        strResponsible = rowItem.WHOLE_NAME_RESPONSIBLE_PERSON
    '                    End If

    '                    strBusUnitMgr = rowItem.MGR
    '                    strTaskID = rowItem.TASKITEMSEQID
    '                    strHeaderTitle = rowItem.TASKHEADERTITLE

    '                    strActivity = IP.Bids.SharedFunctions.LocalizeValue (rowItem.ACTIVITYNAME)
    '                    strComments = rowItem.MTTCOMMENT

    '                    strTaskHeaderID = rowItem.TASKHEADERSEQID
    '                    strTaskItemID = rowItem.TASKITEMSEQID

    '                    sbEmailBody.Append("<P><font size =2 face=Arial><B><U>" & strRecType & "</B></U></FONT><BR>")
    '                    sbEmailBody.Append("<TABLE border=1 cellpadding='2' cellspacing='0' style='border-color:black'><font size =2 face=Arial><TR valign=top><B>{0}" & IP.Bids.SharedFunctions.LocalizeValue ("DueDate") & "{1}<TD width=25%>Header Info{1}<TD width=25%>Task Description{1}")
    '                    sbEmailBody.Append("{0}Responsible{1}<TD width=15% wrap=hard>Comments{1}{0}BU/Type Manager{1}")
    '                    sbEmailBody.Append("</B></TR>")
    '                    sbEmailBody.Append("<BR><TR valign=top><font size=2>{0}" & strDueDate & "{1}")

    '                    sbEmailBody.Append("{0}<A HREF=HTTP://" & strDB & "/TaskTracker/TaskHeader.aspx?HeaderNumber=" & strTaskHeaderID & ">" & strHeaderTitle & " (" & strActivity & ")</A>{1}")
    '                    sbEmailBody.Append("{0}<A HREF=HTTP://" & strDB & "/TaskTracker/TaskDetails.aspx?HeaderNumber=" & strTaskHeaderID & "&TaskNumber=" & strTaskID & ">" & strTitle & "</A>{1}")
    '                    sbEmailBody.Append("{0}" & IP.Bids.SharedFunctions.LocalizeValue (strResponsible) & "{1}")
    '                    sbEmailBody.Append("{0}" & strComments & "{1}")
    '                    sbEmailBody.Append("{0}" & IP.Bids.SharedFunctions.LocalizeValue (strBusUnitMgr) & "{1}")
    '                    sbEmailBody.Append("</TR></TABLE>")
    '                    strMsg = sbEmailBody.ToString
    '                    strMsg = String.Format(CultureInfo.CurrentCulture, strMsg, v_td)
    '                    strSubject = "Manufacturing Task Tracker tasks that were entered and you."
    '                    strHeading = "<HTML><BODY><font size=3 face=Arial><B>Here are the tasks from Manufacturing Task Tracker that were entered today that you are responsible for.</B>"
    '                    strFooter = "</HTML></BODY>"

    '                    strBody = strHeading & "<BR>" & strMsg.ToString & strFooter
    '                    strBody = IP.Bids.SharedFunctions.CleanString(strBody, "<br>")
    '                    IP.Bids.SharedFunctions.SendEmail(strEmailAddress, "manufacturing.task@graphicpkg.com", strSubject, strBody)

    '                    strBody = ""
    '                    retVal = True
    '                Next
    '            End If
    '        End If
    '    Catch
    '        retVal = False
    '        Throw
    '    Finally
    '        adapter = Nothing
    '        table = Nothing
    '    End Try
    '    Return retVal
    '    adapter = Nothing
    'End Function

    ''' <summary>
    ''' Sends out the Replication Email
    ''' </summary>
    ''' <param name="taskItemNumber"></param>
    ''' <param name="webserverName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAndSendReplicationEmail(ByVal taskItemNumber As Integer, ByVal webserverName As String) As Boolean
        Dim adapter As TaskDetailTableAdapters.MTTBATCHREPLICATIONTableAdapter

        Dim table As TaskDetail.MTTBATCHREPLICATIONDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        Dim retVal As Boolean
        Dim v_td As String() = {"<TD>", "</TD>"}
        Dim sbEmailBody As New System.Text.StringBuilder
        Dim strPrevRecType As String
        Dim strEmailAddress As String = ""
        Dim strRecType As String
        Dim strSiteName As String
        Dim strTitle As String
        Dim strTemplateTitle As String = ""
        Dim strTemplateNbr As String = ""
        Dim strFailureFlag As String = ""
        Dim strWarningFlag As String = ""

        Dim strRoleDescription As String

        Dim strSubject As String = ""
        Dim strMsg As String = ""
        Dim strBody As String = ""
        Dim strFooter As String = ""
        Dim strHeading1 As String = ""
        Dim strHeading2 As String = ""
        Dim strDB As String = ""
        Dim strLang As String = ""

        Try
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.MTTBATCHREPLICATIONTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetEmailData(CStr(taskItemNumber), emptyCursor)
                If table IsNot Nothing AndAlso table.Rows.Count > 0 Then
                    strPrevRecType = String.Empty
                    sbEmailBody = New System.Text.StringBuilder
                    sbEmailBody.Append("<TABLE border='1' cellpadding='2' cellspacing='0' style='background-color:black'>")
                    For Each rowItem As TaskDetail.MTTBATCHREPLICATIONRow In table.Rows
                        With rowItem
                            If .IsEMAIL_SECTIONNull Then
                                .EMAIL_SECTION = String.Empty
                            End If
                            If .IsEMAILNull Then
                                .EMAIL = String.Empty
                            End If
                            If .IsROLEDESCRIPTIONNull Then
                                .ROLEDESCRIPTION = String.Empty
                            End If
                            If .IsSITENAMENull Then
                                .SITENAME = String.Empty
                            End If
                            If .IsTEMPLATE_TITLENull Then
                                .TEMPLATE_TITLE = String.Empty
                            End If
                            If .IsTEMPLATESEQIDNull Then
                                .TEMPLATESEQID = String.Empty
                            End If
                            If .IsSTATUSNull Then
                                .STATUS = String.Empty
                            End If
                            If .IsCREATEDBYNull Then
                                .CREATEDBY = String.Empty
                            End If
                            If .IsREPLICATOR_DEFAULTLANGUAGENull Then
                                .REPLICATOR_DEFAULTLANGUAGE = "EN-US"
                            End If
                        End With

                        strDB = webserverName

                        strEmailAddress = rowItem.EMAIL

                        strRecType = rowItem.EMAIL_SECTION
                        'strRecType = IP.Bids.SharedFunctions.LocalizeValue(rowItem.EMAIL_SECTION, rowItem.REPLICATOR_DEFAULTLANGUAGE, True)
                        strLang = rowItem.REPLICATOR_DEFAULTLANGUAGE

                        'strSiteName = rowItem.SITENAME  'Cannot be NULL
                        If rowItem.REPLICATOR_DEFAULTLANGUAGE.ToUpper <> "EN-US" Then
                            strSiteName = IP.Bids.SharedFunctions.LocalizeValue(rowItem.SITENAME, rowItem.REPLICATOR_DEFAULTLANGUAGE, True)  'Cannot be NULL
                        Else
                            strSiteName = rowItem.SITENAME
                        End If
                        'strDueDate = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(rowItem.ITEM_DUEDATE, rowItem.RESPONSIBLE_DEFAULTLANGUAGE, "d")
                        strTitle = rowItem.STATUS

                        strTemplateTitle = rowItem.TEMPLATE_TITLE
                        strTemplateNbr = rowItem.TEMPLATESEQID


                        strRoleDescription = rowItem.ROLEDESCRIPTION

                        'sbEmailBody.Append("<P><font size =2 face=Arial><B><U>" & strRecType & "</B></U></FONT><BR>")
                        'sbEmailBody.Append("<TABLE border=2 cellpadding='2' cellspacing='0' style='border-color:black'><font size =2 face=Arial><TR valign=top><B>")
                        'sbEmailBody.Append("</B></TR>")

                        If strRecType = "1Failure" And strPrevRecType <> strRecType Then
                            sbEmailBody.Append("<TR style='background-color:white'  ><TD bgcolor=red colspan=3>" & IP.Bids.SharedFunctions.LocalizeValue("FAILURES", rowItem.REPLICATOR_DEFAULTLANGUAGE, True) & "</TD></TR>")
                            strPrevRecType = strRecType
                            strFailureFlag = "Y"
                        Else
                            If strRecType = "2Warning" And strPrevRecType <> strRecType Then
                                sbEmailBody.Append("<TR style='background-color:white' ><TD  bgcolor=yellow colspan=3>" & IP.Bids.SharedFunctions.LocalizeValue("WARNINGS", rowItem.REPLICATOR_DEFAULTLANGUAGE, True) & "</TD></TR>")
                                strPrevRecType = strRecType
                                strWarningFlag = "Y"
                            Else
                                If strPrevRecType <> strRecType Then
                                    sbEmailBody.Append("<TR style='background-color:white' ><TD bgcolor=green colspan=3>" & IP.Bids.SharedFunctions.LocalizeValue("COMPLETED SUCCESSFULLY", rowItem.REPLICATOR_DEFAULTLANGUAGE, True) & "</TD></TR>")
                                    strPrevRecType = strRecType
                                End If
                            End If
                        End If

                        sbEmailBody.Append("<TR style='background-color:white'><TD>" & strSiteName & "</TD> <TD>" & strTitle & "</TD> <TD>" & strRoleDescription & "</TD></TR>")
                        'sbEmailBody.Append("{0}" & strComments & "{1}")
                        'sbEmailBody.Append("{0}" & IP.Bids.SharedFunctions.LocalizeValue (strBusUnitMgr) & "{1}")


                        If strFailureFlag = "Y" Then
                            strSubject = IP.Bids.SharedFunctions.LocalizeValue("Manufacturing Task Tracker Replication Complete with FAILURES", rowItem.REPLICATOR_DEFAULTLANGUAGE, True)
                        Else
                            If strFailureFlag <> "Y" And strWarningFlag = "Y" Then
                                strSubject = IP.Bids.SharedFunctions.LocalizeValue("Manufacturing Task Tracker Replication Complete with WARNINGS", rowItem.REPLICATOR_DEFAULTLANGUAGE, True)
                            Else
                                If strFailureFlag <> "Y" And strWarningFlag <> "Y" Then
                                    strSubject = IP.Bids.SharedFunctions.LocalizeValue("Manufacturing Task Tracker Replication Complete", rowItem.REPLICATOR_DEFAULTLANGUAGE, True)
                                End If
                            End If
                        End If

                    Next

                    sbEmailBody.Append("</TR></TABLE>")
                    strMsg = sbEmailBody.ToString
                    'strMsg = String.Format(CultureInfo.CurrentCulture, strMsg, v_td)


                    strHeading1 = "<HTML><BODY><font size=3 face=Arial><B>" & IP.Bids.SharedFunctions.LocalizeValue("Here are the Facilities from the Manufacturing Task Tracker Replication", strLang, True) & "</B><BR><BR><B>"

                    If CheckForTesting() = "yes" Then
                        strHeading2 = "<A HREF=HTTP://gpitasktracker.graphicpkg.com:130/TaskHeader.aspx?HeaderNumber=" & strTemplateNbr & ">" & strTemplateTitle & "</A></B><BR><BR>"
                    Else
                        strHeading2 = "<A HREF=HTTP://gpitasktracker.graphicpkg.com/TaskHeader.aspx?HeaderNumber=" & strTemplateNbr & ">" & strTemplateTitle & "</A></B><BR><BR>"
                    End If

                    strFooter = "</HTML></BODY>"
                    'sbEmailBody.Append("{0}<A HREF=HTTP://" & strDB & "/TaskTracker/TaskHeader.aspx?HeaderNumber=" & strTaskHeaderID & ">" & strHeaderTitle & " (" & strActivity & ")</A>{1}")

                    strBody = strHeading1 & strHeading2 & "<BR>" & strMsg.ToString & strFooter
                    strBody = IP.Bids.SharedFunctions.CleanString(strBody, "<br>")
                    IP.Bids.SharedFunctions.SendEmail(strEmailAddress, "manufacturing.task@graphicpkg.com", strSubject, strBody)

                    strBody = ""
                    retVal = True

                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetAndSendReplicationEmail", "Attempting to send an email for TaskItemNumber=" & taskItemNumber, ex)
            retVal = False
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return retVal
        adapter = Nothing
    End Function

    ''' <summary>
    ''' Sends out an email indicating that the Sub Task has been closed
    ''' </summary>
    ''' <param name="taskItemNumber"></param>
    ''' <param name="TaskHeaderNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAndSendSubtaskClosedEmail(ByVal taskItemNumber As Integer, ByVal TaskHeaderNumber As Integer) As Boolean
        Dim adapter As TaskDetailTableAdapters.ImmediateEmailTableAdapter
        Dim table As TaskDetail.ImmediateEmailDataTable = Nothing
        Dim emptyCursor As Object = Nothing
        '        Dim record As TaskItem = Nothing ' COMMENTED BY CODEIT.RIGHT
        Dim retVal As Boolean
        Dim taskHeader As TaskHeaderBll = Nothing
        Dim refSite As String = String.Empty
        Try
            taskHeader = New TaskHeaderBll(CInt(TaskHeaderNumber))
            If taskHeader IsNot Nothing Then
                Select Case taskHeader.CurrentTaskHeaderRecord.ExternalSourceSeqID
                    Case IP.Bids.SharedFunctions.SourceSystemID.MOC
                        refSite = "&refsite=MOC"
                End Select

            End If
            If table Is Nothing OrElse table.Rows.Count = 0 Then
                adapter = New TaskDetailTableAdapters.ImmediateEmailTableAdapter
                adapter.Connection = New Devart.Data.Oracle.OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetImmediateEmail(CStr(taskItemNumber), emptyCursor)
                If table IsNot Nothing AndAlso table.Rows.Count > 0 Then

                    For Each rowItem As TaskDetail.ImmediateEmailRow In table.Rows
                        With rowItem
                            If .IsACTIVITYNAMENull Then
                                .ACTIVITYNAME = String.Empty
                            End If
                            If .IsBUSUNITMGRNull Then
                                .BUSUNITMGR = String.Empty
                            End If
                            If .IsCREATEDBY_DEFAULTLANGUAGENull Then
                                .CREATEDBY_DEFAULTLANGUAGE = "EN-US"
                            End If
                            If .IsCREATEDBY_EMAILNull Then
                                .CREATEDBY_EMAIL = String.Empty
                            End If
                            If .IsCREATEDBYNull Then
                                .CREATEDBY = String.Empty
                            End If
                            If .IsHEADERTYPENAMENull Then
                                .HEADERTYPENAME = String.Empty
                            End If
                            If .IsITEM_CLOSEDDATENull Then

                            End If
                            If .IsITEM_DESCRIPTIONNull Then
                                .ITEM_DESCRIPTION = String.Empty
                            End If
                            If .IsITEM_DUEDATENull Then
                                '.ITEM_DUEDATE 
                            End If
                            If .IsITEM_TITLENull Then
                                .ITEM_TITLE = String.Empty
                            End If
                            If .IsLEADTIMENull Then
                                .LEADTIME = 1
                            End If
                            If .IsMGRNull Then
                                .MGR = String.Empty
                            End If
                            If .IsMTTCOMMENTNull Then
                                .MTTCOMMENT = String.Empty
                            End If
                            If .IsRECNUMNull Then
                                .RECNUM = 1
                            End If
                            If .IsRECTYPENull Then
                                .RECTYPE = "Responsible For"
                            End If
                            If .IsRESPONSIBLE_DEFAULTLANGUAGENull Then
                                .RESPONSIBLE_DEFAULTLANGUAGE = "EN-US"
                            End If
                            If .IsRESPONSIBLE_EMAILNull Then
                                .RESPONSIBLE_EMAIL = String.Empty
                            End If
                            If .IsRESPONSIBLE_ROLE_NAMESNull Then
                                .RESPONSIBLE_ROLE_NAMES = String.Empty
                            End If
                            If .IsRESPONSIBLEUSERNAMENull Then
                                .RESPONSIBLEUSERNAME = String.Empty
                            End If
                            If .IsROLEDESCRIPTIONNull Then
                                .ROLEDESCRIPTION = String.Empty
                            End If
                            If .IsSITENAMENull Then
                                .SITENAME = String.Empty
                            End If
                            If .IsTASKHEADERTITLENull Then
                                .TASKHEADERTITLE = String.Empty
                            End If
                            If .IsTYPEMGRNull Then
                                .TYPEMGR = String.Empty
                            End If
                            If .IsWHOLE_NAME_CREATEDBY_PERSONNull Then
                                .WHOLE_NAME_CREATEDBY_PERSON = String.Empty
                            End If
                            If .IsWHOLE_NAME_RESPONSIBLE_PERSONNull Then
                                .WHOLE_NAME_RESPONSIBLE_PERSON = String.Empty
                            End If

                        End With

                        Dim v_td As String() = {"<TD>", "</TD>"}
                        Dim sbEmailBody As New System.Text.StringBuilder
                        Dim strDueDate As String
                        Dim strEmailAddress As String
                        Dim strRecType As String
                        Dim strSiteName As String
                        Dim strTitle As String
                        Dim strResponsible As String
                        Dim strBusUnitMgr As String
                        Dim strTaskID As String
                        Dim strHeaderTitle As String
                        Dim strActivity As String
                        Dim strComments As String
                        Dim strTaskHeaderID As String
                        Dim strTaskItemID As String
                        Dim strSubject As String
                        Dim strMsg As String
                        Dim strBody As String = ""
                        Dim strFooter As String = ""
                        Dim strHeading As String = ""
                        Dim strDB As String
                        'Dim ipLoc As New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization(rowItem.RESPONSIBLE_DEFAULTLANGUAGE, "RI")

                        strDB = IP.Bids.SharedFunctions.GetServerName

                        sbEmailBody = New System.Text.StringBuilder
                        strEmailAddress = rowItem.RESPONSIBLE_EMAIL

                        strRecType = IP.Bids.SharedFunctions.LocalizeValue(rowItem.RECTYPE, True)

                        strSiteName = rowItem.SITENAME  'Cannot be NULL

                        strDueDate = IP.MEASFramework.ExtensibleLocalizationAssembly.DateTime.GetLocalizedDateTime(rowItem.ITEM_DUEDATE, rowItem.RESPONSIBLE_DEFAULTLANGUAGE, "d")
                        strTitle = rowItem.ITEM_TITLE

                        If rowItem.WHOLE_NAME_RESPONSIBLE_PERSON.Trim.Length = 0 Then
                            strResponsible = rowItem.ROLEDESCRIPTION & " (" & rowItem.RESPONSIBLE_ROLE_NAMES & ")"
                        Else
                            strResponsible = rowItem.WHOLE_NAME_RESPONSIBLE_PERSON
                        End If

                        strBusUnitMgr = rowItem.MGR
                        strTaskID = rowItem.TASKITEMSEQID
                        strHeaderTitle = rowItem.TASKHEADERTITLE

                        strActivity = rowItem.ACTIVITYNAME
                        strComments = rowItem.MTTCOMMENT

                        strTaskHeaderID = rowItem.TASKHEADERSEQID
                        strTaskItemID = rowItem.TASKITEMSEQID

                        sbEmailBody.Append("<P><font size =2 face=Arial><B><U>" & strRecType & "</B></U></FONT><BR>")
                        sbEmailBody.Append("<TABLE border=1 cellpadding='2' cellspacing='0' style='border-color:black'><font size =2 face=Arial><TR valign=top><B>{0}" & IP.Bids.SharedFunctions.LocalizeValue("Due Date") & "{1}<TD width=25%>" & IP.Bids.SharedFunctions.LocalizeValue("Header Info", True) & "{1}<TD width=25%>" & IP.Bids.SharedFunctions.LocalizeValue("Task Description", True) & "{1}")
                        sbEmailBody.Append("{0}" & IP.Bids.SharedFunctions.LocalizeValue("Responsible", True) & "{1}<TD width=15% wrap=hard>" & IP.Bids.SharedFunctions.LocalizeValue("Comments", True) & "{1}{0}" & IP.Bids.SharedFunctions.LocalizeValue("BU/Type Manager", True) & "{1}")
                        sbEmailBody.Append("</B></TR>")
                        sbEmailBody.Append("<BR><TR valign=top><font size=2>{0}" & strDueDate & "{1}")

                        If CheckForTesting() = "yes" Then
                            sbEmailBody.Append("{0}<A HREF=HTTP://gpitasktracker.graphicpkg.com:130/TaskHeader.aspx?HeaderNumber=" & strTaskHeaderID & refSite & ">" & strHeaderTitle & " (" & strActivity & ")</A>{1}")
                            sbEmailBody.Append("{0}<A HREF=HTTP://gpitasktracker.graphicpkg.com:130/TaskDetails.aspx?HeaderNumber=" & strTaskHeaderID & "&TaskNumber=" & strTaskID & refSite & ">" & strTitle & "</A>{1}")
                        Else
                            sbEmailBody.Append("{0}<A HREF=HTTP://gpitasktracker.graphicpkg.com/TaskHeader.aspx?HeaderNumber=" & strTaskHeaderID & refSite & ">" & strHeaderTitle & " (" & strActivity & ")</A>{1}")
                            sbEmailBody.Append("{0}<A HREF=HTTP://gpitasktracker.graphicpkg.com/TaskDetails.aspx?HeaderNumber=" & strTaskHeaderID & "&TaskNumber=" & strTaskID & refSite & ">" & strTitle & "</A>{1}")
                        End If

                        sbEmailBody.Append("{0}" & IP.Bids.SharedFunctions.LocalizeValue(strResponsible) & "{1}")
                        sbEmailBody.Append("{0}" & strComments & "{1}")
                        sbEmailBody.Append("{0}" & IP.Bids.SharedFunctions.LocalizeValue(strBusUnitMgr) & "{1}")
                        sbEmailBody.Append("</TR></TABLE>")
                        strMsg = sbEmailBody.ToString
                        strMsg = String.Format(CultureInfo.CurrentCulture, strMsg, v_td)
                        strSubject = String.Format("All subtask for have been completed. [{0}]", strTitle)
                        strHeading = "<HTML><BODY><font size=3 face=Arial><B>Here is the tasks from Manufacturing Task Tracker that you are responsible for.</B>"
                        strFooter = "</HTML></BODY>"

                        strBody = strHeading & "<BR>" & strMsg.ToString & strFooter
                        strBody = IP.Bids.SharedFunctions.CleanString(strBody, "<br>")
                        IP.Bids.SharedFunctions.SendEmail(strEmailAddress, "manufacturing.task@graphicpkg.com", strSubject, strBody)

                        strBody = ""
                        retVal = True
                    Next
                End If
            End If
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("GetAndSendSubtaskClosedEmail", "Attempting to send an email for TaskItemNumber=" & taskItemNumber, ex)
            retVal = False
        Finally
            adapter = Nothing
            table = Nothing
        End Try
        Return retVal
        adapter = Nothing
    End Function
End Class
