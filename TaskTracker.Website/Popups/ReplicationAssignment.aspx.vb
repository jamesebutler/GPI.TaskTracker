'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : MJPOPE
' Created          : 06-10-2011
'
' Last Modified By : MJPOPE
' Last Modified On : 06-28-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************

Partial Class ReplicationAssignment
    Inherits IP.Bids.BasePage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        IP.Bids.SharedFunctions.DisablePageCache(Response)

        Dim HeaderNumber As String = String.Empty
        HeaderNumber = Request.QueryString("HeaderNumber")
        If HeaderNumber IsNot Nothing AndAlso IsNumeric(HeaderNumber) Then
            _replicationAssignment.PopulateTree(HeaderNumber)
        Else
            'Throw error or close
        End If
        
    End Sub



    Protected Sub _btnProcessReplicaitonTop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnProcessReplicaitonTop.Click
        _replicationAssignment.ProcessReplicaiton()
       
    End Sub
End Class
