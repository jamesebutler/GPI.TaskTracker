'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 08-17-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-02-2010
' Description      : This is used to display a banner image and text
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text

Partial Class RIBanner
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Load the Image to be written on.
        Dim textMessage As String = String.Empty
        Dim bitMapImage As Bitmap = Nothing

        If Request.QueryString("Theme") IsNot Nothing Then
            Select Case Request.QueryString("Theme").ToUpper
                Case "RI"
                    bitMapImage = New System.Drawing.Bitmap(Server.MapPath("App_Themes\RI\Images\Reliability_back.jpg"))
                Case "EIS"
                    bitMapImage = New System.Drawing.Bitmap(Server.MapPath("App_Themes\EIS\Images\EIS_back.jpg"))
                Case Else
                    bitMapImage = New System.Drawing.Bitmap(Server.MapPath("App_Themes\RI\Images\Reliability_back.jpg"))
            End Select
        End If
       
        'Set the text message that will be displayed on the banner image
        If Request.QueryString("textMessage") IsNot Nothing Then
            textMessage = Request.QueryString("textMessage")
        Else
            textMessage = String.Empty
        End If

        If bitMapImage IsNot Nothing Then
            DisplayImageFromText(textMessage, 20, bitMapImage)
        End If
    End Sub

    ''' <summary>
    ''' Adds the specified text message to the banner image
    ''' </summary>
    ''' <param name="textMessage"></param>
    ''' <param name="margins"></param>
    ''' <param name="bitMapImage"></param>
    ''' <remarks></remarks>
    Public Sub DisplayImageFromText(ByVal textMessage As String, ByVal margins As Integer, ByVal bitMapImage As Bitmap)
        Dim g As Graphics = Nothing
        Dim graphicImage As Graphics = Graphics.FromImage(bitMapImage)
        Dim imgWidth As Integer = 0
        Dim imgHeight As Integer = 0
        Dim objSF As New StringFormat()
        Dim privateFontCollection As New PrivateFontCollection()
        Dim regFont As Font
        Dim solidBrush As New SolidBrush(Color.White)

        Try
            If bitMapImage IsNot Nothing Then
                imgWidth = bitMapImage.Width
                imgHeight = bitMapImage.Height
                g = Graphics.FromImage(bitMapImage)
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
                g.SmoothingMode = SmoothingMode.Default
            End If

            objSF.Alignment = StringAlignment.Center
            objSF.LineAlignment = StringAlignment.Center
            objSF.Trimming = StringTrimming.Character

            'privateFontCollection.AddFontFile(Server.MapPath("Images/Fonts/ARIALUNI.TTF")) ' load font from file
            'thisFont = privateFontCollection.Families(0)
            regFont = New Font("Arial Unicode MS", 24, FontStyle.Bold, GraphicsUnit.Pixel) ' Create a new font
            textMessage = IP.Bids.SharedFunctions.AdjustTextForDisplay(textMessage, imgWidth - 20 - margins, regFont)
            g.DrawString(textMessage, regFont, solidBrush, New Rectangle(0, 0, imgWidth, imgHeight), objSF) ' Using the font write the text using the font style
            bitMapImage.Save(Response.OutputStream, ImageFormat.Jpeg)
        Catch ex As Exception
            IP.Bids.SharedFunctions.HandleError("DisplayImageFromText", "Error Displaying the Banner Image - " & ex.Message, ex)
            Server.ClearError()
        Finally
            graphicImage.Dispose()
            bitMapImage.Dispose()
        End Try
    End Sub
End Class
