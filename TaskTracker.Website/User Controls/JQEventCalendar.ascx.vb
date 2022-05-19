Imports System.Linq
Partial Class User_Controls_JQEventCalendar
    Inherits System.Web.UI.UserControl
    Private ci As System.Globalization.CultureInfo
    Private listing As System.Collections.Generic.List(Of TaskListing)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    End Sub

    Public Sub DisplayEventCalendar(ByVal eventListing As System.Collections.Generic.List(Of TaskListing))
        listing = eventListing
        ScriptManager.RegisterStartupScript(Me.Page, Page.GetType, "JQFullCalendar_" & Me.ClientID, "try {" & AddEventCalendarJS() & "} catch(e){alert(e)}", True)
    End Sub
#Region "Private Methods"
    Private Function AddEventsJS() As String
        Dim imgPath As String = Page.ResolveClientUrl("~/Images/")
        'Dim taskSearch As New TaskTrackerListsBll
        'imgPath = Mid(Page.Request.Url.ToString, 1, Page.Request.Url.ToString.LastIndexOf("/", StringComparison.CurrentCulture)) & "/Images/"
        'imgPath = "Images/"
        Dim overdueStatusID As Integer = -1
        If listing Is Nothing OrElse listing.Count = 0 Then
            Return ""
            Exit Function
        End If
        Dim sb As New StringBuilder
        Dim urlBase As String = Page.ResolveClientUrl(IP.Bids.SiteURLs.GetTaskDetailsURL("{0}", "{1}", "{2}"))

        With sb
            .AppendLine("events: [ ")

            Dim itemCount As Integer = listing.Count
            Dim count As Integer = 0
            Dim imgStatus As String = "" 'String.Empty

            For Each item As TaskListing In listing
                count = count + 1
                If item.DueDate.Length > 0 Then
                    'Next
                    Dim dueDate As Date = CDate(item.DueDate)
                    'For i As Integer = 1 To 100
                    If dueDate < Now And item.StatusName.ToLower = "open" Then
                        imgStatus = GetTaskStatus(overdueStatusID, False, imgPath)
                    Else
                        If IsNumeric(item.StatusSeqId) Then
                            imgStatus = GetTaskStatus(item.StatusSeqId, False, imgPath)
                        Else
                            imgStatus = ""
                        End If
                    End If
                    ' Date Format '2010-01-05'
                    ' What do I need from the Item List
                    ' - Title, Status, Due Date, task number, Item Number
                    .Append("{ ")
                    '.AppendLine("className: 'fc-icon-noworkneeded',")
                    .Append("imageurl:'" & imgStatus.Replace("'", "''") & "',")
                    '.AppendLine("className: 'fc-event', ")
                    .Append("title:'" & (item.ItemTitle.Replace(Chr(13), " ").Replace("'", "").Replace("/", " - ").Replace(ControlChars.Lf, "").Replace("""", "").Replace(ControlChars.NewLine, "").Replace(ControlChars.CrLf, "").Replace(ControlChars.Quote, "")).Trim & "', ") '& " (" & item.StatusName & ")', ")
                    .Append("description:'" & GetTaskPreview(item) & "',") '(item.ItemDesc.Replace(Chr(13), " ").Replace("'", "").Replace("/", " - ").Replace(ControlChars.Lf, "").Replace("""", "").Replace(ControlChars.NewLine, "").Replace(ControlChars.CrLf, "").Replace(ControlChars.Quote, "")).Trim & "', ") '& " (" & item.StatusName & ")', ")
                    '.AppendLine("start: new Date(y, m, d+" & i.ToString & "), ")
                    'Dim dueDate As Date = CDate(item.DueDate)
                    .Append("start:new Date(" & dueDate.Year & "," & dueDate.Month - 1 & "," & dueDate.Day & "),")
                    '.AppendLine("allDay:true, ")
                    .Append("url:'" & String.Format(urlBase, item.TaskHeaderSeqId, "", item.TaskItemSeqId) & "'")
                    .Append("} ")

                    If count < itemCount Then
                        .Append(",")
                    End If
                End If
            Next
            .AppendLine("] ")
        End With
        Return sb.ToString
    End Function

    Private Function GetTaskPreview(item As TaskListing) As String
        Dim sb As New StringBuilder
        Dim bsRow As String = "<dt>{0}</dt><dd>{1}</dd>"
        sb.Append("<dl class=dl-horizontal>")
        sb.AppendFormat(bsRow, "Responsible", IIf(item.RoleDescription.Length > 0, item.RoleDescription, item.ResponsibleName).ToString.Replace("'", ""))
        sb.AppendFormat(bsRow, "Due Date", item.DueDate)
        sb.AppendFormat(bsRow, "Status", item.StatusName)
        sb.AppendFormat(bsRow, "Description", item.ItemDesc.Replace(Chr(13), " ").Replace("'", "").Replace("/", " - ").Replace(ControlChars.Lf, "").Replace("""", "").Replace(ControlChars.NewLine, "").Replace(ControlChars.CrLf, "").Replace(ControlChars.Quote, "").Trim)
        sb.Append("</dl>")
        Return sb.ToString
    End Function

    'Private Sub CombineTasksForCalendarView(ByVal tasks As System.Collections.Generic.List(Of TaskListing))
    '    'Tasks are sorted by due date
    '    Dim taskSearch As New TaskTrackerListsBll
    '    Dim imgPath As String = Page.ResolveUrl("~/Images/")
    '    Dim dueDate As String = ""
    '    Dim sbTasks As New StringBuilder
    '    imgPath = Mid(Page.Request.Url.ToString, 1, Page.Request.Url.ToString.LastIndexOf("/", StringComparison.CurrentCulture)) & "/Images/"
    '    TaskListing = New Collections.Specialized.OrderedDictionary
    '    If tasks Is Nothing OrElse tasks.Count = 0 Then
    '        Exit Sub
    '    End If
    '    Dim overdueStatusID As Integer = -1

    '    For Each item As TaskListing In tasks
    '        If dueDate.Length = 0 Then
    '            dueDate = item.DueDate
    '        End If
    '        If item.DueDate <> dueDate Then
    '            'Due Date has changed
    '            If TaskListing.Contains(CDate(dueDate).ToShortDateString) = False Then
    '                TaskListing.Add(CDate(dueDate).ToShortDateString, sbTasks.ToString)
    '                dueDate = item.DueDate
    '                sbTasks.Length = 0
    '                If item.DueDate < Now And item.StatusName.ToLower = "open" Then
    '                    sbTasks.Append(taskSearch.GetTaskStatus(overdueStatusID, False, imgPath) & "<a href='TaskDetails.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "&TaskNumber=" & item.TaskItemSeqId & "' title='" & item.ItemTitle & "'>")
    '                Else
    '                    sbTasks.Append(taskSearch.GetTaskStatus(item.StatusSeqId, False, imgPath) & "<a href='TaskDetails.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "&TaskNumber=" & item.TaskItemSeqId & "' title='" & item.ItemTitle & "'>")
    '                End If

    '                'If item.ItemTitle.Length > 25 Then
    '                '    sbTasks.Append(item.ItemTitle.Substring(0, 25) & "...")
    '                'Else
    '                '    sbTasks.Append(item.ItemTitle)
    '                'End If
    '                sbTasks.Append(item.ItemTitle)
    '                sbTasks.Append("</a>")
    '                sbTasks.Append("<br/><br/>")
    '            End If
    '        Else
    '            If item.DueDate < Now And item.StatusName.ToLower = "open" Then
    '                sbTasks.Append(taskSearch.GetTaskStatus(overdueStatusID, False, imgPath) & "<a href='TaskDetails.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "&TaskNumber=" & item.TaskItemSeqId & "' title='" & item.ItemTitle & "'> ")
    '            Else
    '                sbTasks.Append(taskSearch.GetTaskStatus(item.StatusSeqId, False, imgPath) & "<a href='TaskDetails.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "&TaskNumber=" & item.TaskItemSeqId & "' title='" & item.ItemTitle & "'> ")
    '            End If
    '            '"Javascript:parent.window.location=('<%#string.format(Page.ResolveUrl("~/TaskDetails.aspx?HeaderNumber={0}&TaskNumber={1}"),EVAL("TASKHEADERSEQID"),EVAL("TASKITEMSEQID")) %>');"
    '            'sbTasks.Append(taskSearch.GetTaskStatus(item.StatusSeqId, True, imgPath) & " - <a href='TaskItems.aspx?HeaderNumber=" & item.TaskHeaderSeqId & "' title='" & item.ItemTitle & "'>")
    '            sbTasks.Append(item.ItemTitle)
    '            sbTasks.Append("</a>")
    '            sbTasks.Append("<br/><br/>")
    '        End If
    '    Next

    '    If TaskListing.Item(CDate(dueDate).ToShortDateString) Is Nothing Then
    '        TaskListing.Add(CDate(dueDate).ToShortDateString, sbTasks.ToString)
    '    End If
    '    If Session.Item("TaskListing") IsNot Nothing Then
    '        Session.Remove("TaskListing")
    '    End If
    '    Session.Add("TaskListing", TaskListing)
    '    'For Each item As DictionaryEntry In TaskListing
    '    '    System.Diagnostics.Debug.Print(item.Key)
    '    '    System.Diagnostics.Debug.Print(item.Value)
    '    'Next

    'End Sub

    Private Function AddShortDayNamesJS() As String
        Dim ShortDayNames As String

        With ci.DateTimeFormat
            ShortDayNames = String.Format("dayNamesShort: ['{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}'] ", .DayNames(0), .DayNames(1), .DayNames(2), .DayNames(3), .DayNames(4), .DayNames(5), .DayNames(6)) '.AbbreviatedDayNames(0), .AbbreviatedDayNames(1), .AbbreviatedDayNames(2), .AbbreviatedDayNames(3), .AbbreviatedDayNames(4), .AbbreviatedDayNames(5), .AbbreviatedDayNames(6))
        End With
        Return ShortDayNames
    End Function

    Private Function AddFullDayNamesJS() As String
        Dim FullDayNames As String

        With ci.DateTimeFormat
            FullDayNames = String.Format("dayNames: ['{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}'] ", .DayNames(0), .DayNames(1), .DayNames(2), .DayNames(3), .DayNames(4), .DayNames(5), .DayNames(6))
        End With
        Return FullDayNames
    End Function

    Private Function AddShortMonthNamesJS() As String
        Dim ShortMonthNames As String
        With ci.DateTimeFormat
            ShortMonthNames = String.Format("monthNamesShort: ['{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}']", .GetAbbreviatedMonthName(1), .GetAbbreviatedMonthName(2), .GetAbbreviatedMonthName(3), .GetAbbreviatedMonthName(4), .GetAbbreviatedMonthName(5), .GetAbbreviatedMonthName(6), .GetAbbreviatedMonthName(7), .GetAbbreviatedMonthName(8), .GetAbbreviatedMonthName(9), .GetAbbreviatedMonthName(10), .GetAbbreviatedMonthName(11), .GetAbbreviatedMonthName(12))
        End With
        Return ShortMonthNames
    End Function

    Private Function AddFullMonthNamesJS() As String
        Dim FullMonthNames As String
        With ci.DateTimeFormat
            FullMonthNames = String.Format("monthNames: ['{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}']", .GetMonthName(1), .GetMonthName(2), .GetMonthName(3), .GetMonthName(4), .GetMonthName(5), .GetMonthName(6), .GetMonthName(7), .GetMonthName(8), .GetMonthName(9), .GetMonthName(10), .GetMonthName(11), .GetMonthName(12))
        End With
        Return FullMonthNames
    End Function

    Private Function AddEventCalendarJS() As String
        Dim s As String
        ci = System.Globalization.CultureInfo.CurrentCulture

        s = ci.DateTimeFormat.AbbreviatedDayNames(1)

        Dim sb As New StringBuilder
        With sb
            .Append("$(document).ready(function() {	")
            .Append("var date = new Date();	")
            .Append("var d = date.getDate();	")
            .Append("var m = date.getMonth();	")
            .Append("var y = date.getFullYear();	")
            .AppendLine("$('#taskCalendar').fullCalendar({ ")
            .Append("editable: false, ")
            '.Append("eventLimit: true, ")
            .Append("header:{ ")
            .Append("left: 'prev,next today', ")
            .Append("center: 'title', ")
            .Append("right: 'month,basicWeek' ")
            .Append("}, ")

            .Append("eventClick:  function(event, jsEvent, view)")
            .Append("{$('#modalTitle').html(event.title);")
            .Append("$('#modalBody').html(event.description);")
            .Append("$('#eventUrl').attr('href',event.url);")
            .Append("$('#fullCalModal').modal();return false;},")

            '.AppendFormat("defaultDate: '{0}',", Now.Year & "-" & Now.Month & "-" & Now.Day)
            .Append("buttonText: { ")
            .Append("theme: false,")
            .Append("today:      '" & Replace(IP.Bids.SharedFunctions.LocalizeValue("today", True), "'", "&#39;") & "', ")
            .Append("month:      '" & Replace(IP.Bids.SharedFunctions.LocalizeValue("month", True), "'", "&#39;") & "', ")
            .Append("week:       '" & Replace(IP.Bids.SharedFunctions.LocalizeValue("week", True), "'", "&#39;") & "', ")
            .Append("day:        '" & Replace(IP.Bids.SharedFunctions.LocalizeValue("day", True), "'", "&#39;") & "', ")
            .Append("listMonth:   '" & Replace(IP.Bids.SharedFunctions.LocalizeValue("Task Item Listing", True), "'", "&#39;") & "' ")
            .Append("}, ")
            .Append("firstDay: 0, ")
            .Append("allDayDefault : true, ")
            .Append(AddFullMonthNamesJS)
            .Append(",")
            .Append(AddShortMonthNamesJS)
            .Append(",")
            .Append(AddFullDayNamesJS)
            .Append(",")
            .Append(AddShortDayNamesJS())
            .Append(",")
            .Append(AddEventsJS())
            '10/11
            .Append(",")

            .Append(" eventRender: function(event, eventElement) {")
            .Append("if (event.imageurl) {")
            .Append("if (eventElement.find('span.fc-event-time').length) {")
            '            .AppendLine(" eventElement.find('span.fc-event-time').before($(event.imageurl));")
            .Append(" eventElement.find('span.fc-event-time').before($(event.imageurl));")
            '.AppendLine("   eventElement.find('span.fc-event-time').append(event.imageurl);")
            .Append(" } else {")
            .Append("   eventElement.find('span.fc-title').before($(event.imageurl));")
            '.AppendLine("   eventElement.find('span.fc-event-title').append(event.imageurl);")
            .Append(" }")
            .Append("}}")
            '10/11
            .Append("}); ")
            .Append("}); ")
            If listing IsNot Nothing AndAlso listing.Count > 0 Then
                Dim dt As Date = CDate(listing.Item(0).DueDate)
                .Append("$('#taskCalendar').fullCalendar('gotoDate',$('#taskCalendar').fullCalendar.moment('" & dt.Year.ToString & "-" & CStr(dt.Month - 1) & "-" & CStr(dt.Day) & "'));")
            End If
            'Add event render

            'End Event render
        End With
        Return sb.ToString
    End Function

    Private Function GetTaskStatus(ByVal statusID As Integer, ByVal includeLabel As Boolean, ByVal imagePath As String) As String
        Dim returnVal As String = String.Empty
        Dim taskSearch As New TaskTrackerListsBll
        Dim status As System.Collections.Generic.List(Of TaskStatus) = taskSearch.GetTaskStatus()
        For Each item As TaskStatus In status
            If item.StatusSeqid = statusID Then
                If includeLabel Then
                    Dim img As String = "<img class=statusImg src={0}>{1}"
                    'Dim img As String = "<img class='statusImg' src={0} border=0 align=center width=15 height=15>{1}"
                    Dim statusText As String = String.Format(img, imagePath & item.ImageIcon) ', item.StatusName)
                    returnVal = statusText
                Else
                    Dim img As String = "<img class=statusImg src={0}>"
                    ' Dim img As String = "<img src={0} border=0 align=center width=15 height=15>"
                    Dim statusText As String = String.Format(img, imagePath & item.ImageIcon) ', item.StatusName)
                    returnVal = statusText
                End If
            End If
        Next
        Return returnVal
    End Function
#End Region
End Class
