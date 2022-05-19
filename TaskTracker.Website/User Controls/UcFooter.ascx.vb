'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 08-17-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Namespace IP.Bids.UserControl
   
    Partial Class Footer
        Inherits System.Web.UI.UserControl

#Region "Private Events"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try

                '                Dim serverName As String = My.Computer.Name ' COMMENTED BY CODEIT.RIGHT
                '                Dim database As String = "" ' COMMENTED BY CODEIT.RIGHT
                '                Dim requestStartTime As System.Diagnostics.ProcessStartInfo = New System.Diagnostics.ProcessStartInfo ' COMMENTED BY CODEIT.RIGHT

                Dim resourceData As New IP.MEASFramework.ExtensibleLocalizationAssembly.WebLocalization()
                If resourceData IsNot Nothing Then
                    Me._lblCopyRight.Text = Convert.ToString(Now.Year) & " Graphic Packaging International. All rights reserved"
                    'Me._lblCopyRight.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0} " & resourceData.GetResourceValue("CopyRight", True, "Global"), Year(Now))
                End If

            Catch
                'An error updating the Copyright text should not affect the user
                Server.ClearError()
            End Try

        End Sub

#End Region
    End Class
End Namespace
