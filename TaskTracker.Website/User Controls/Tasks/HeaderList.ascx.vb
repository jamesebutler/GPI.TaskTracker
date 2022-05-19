Imports Devart.Data.Oracle
Imports Telerik.Web.UI.Export
Imports Telerik.Web.UI.GridExcelBuilder
Imports Telerik.Web.UI
Imports System.Web.UI.WebControls
Imports xi = Telerik.Web.UI.ExportInfrastructure
Imports System.Web.UI
Imports System.Web
Imports System.Drawing



Partial Class User_Controls_Tasks_HeaderList
    Inherits System.Web.UI.UserControl


    Protected Sub RadGridHeaderListing_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGridHeaderListing.NeedDataSource

        If Not Session("headerlisting") Is Nothing Then
            RadGridHeaderListing.DataSource = Session("headerlisting")
        End If
    End Sub

    Private Sub User_Controls_Tasks_HeaderList_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            If Request.QueryString("HeaderNumber") IsNot Nothing Then

                Dim _headerNumber As String = Request.QueryString("HeaderNumber")



                Dim taskHeaderListDS As DataSet = GetHeaderItems(_headerNumber)

                RadGridHeaderListing.DataSource = taskHeaderListDS
                RadGridHeaderListing.DataBind()


            End If
        End If




    End Sub


    'get data
    Public Function GetHeaderItems(ByVal in_TaskHeader As String) As DataSet

        Dim paramCollection As New OracleParameterCollection
        Dim param As New OracleParameter
        Dim ds As System.Data.DataSet = Nothing
        Dim dsError As System.Data.DataSet = Nothing


        Try

            param = New OracleParameter
            param.ParameterName = "in_HeaderSeqId"
            param.OracleDbType = OracleDbType.VarChar
            param.Direction = Data.ParameterDirection.Input
            param.Value = in_TaskHeader
            paramCollection.Add(param)



            param = New OracleParameter
            param.ParameterName = "rsTaskList"
            param.OracleDbType = OracleDbType.Cursor
            param.Direction = Data.ParameterDirection.Output
            paramCollection.Add(param)

            ds = HelperDal.GetDSFromPackage(paramCollection, "mttviewgpi.MTTVIEWHeaders")

            Session("headerlisting") = ds
            Return ds

        Catch ex As Exception
            Return dsError
        End Try

    End Function


    Protected Sub DownloadPDF_Click(sender As Object, e As EventArgs)
        RadGridHeaderListing.MasterTableView.ExportToPdf()
    End Sub


    Protected Sub DownloadCSV_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ConfigureExport()
        RadGridHeaderListing.ExportSettings.FileName = "HeaderList_" + DateTime.Now.ToShortDateString()

        RadGridHeaderListing.MasterTableView.ExportToCSV()
    End Sub

    Protected Sub DownloadXLS_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'ConfigureExport()

        Dim alternateText As String = "3"
        'If alternateText = "Xlsx" AndAlso CheckBox2.Checked Then
        '    RadGridHeaderListing.MasterTableView.GetColumn("EmployeeID").HeaderStyle.BackColor = Color.LightGray
        '    RadGridHeaderListing.MasterTableView.GetColumn("EmployeeID").ItemStyle.BackColor = Color.LightGray
        'End If
        'RadGridHeaderListing.ExportSettings.Excel.Format = DirectCast([Enum].Parse(GetType(Telerik.Web.UI.GridExcelExportFormat), alternateText), Telerik.Web.UI.GridExcelExportFormat)

        RadGridHeaderListing.ExportSettings.ExportOnlyData = True
        RadGridHeaderListing.ExportSettings.OpenInNewWindow = True
        RadGridHeaderListing.ExportSettings.IgnorePaging = True

        RadGridHeaderListing.ExportSettings.FileName = "HeaderList_" + DateTime.Now.ToShortDateString()

        RadGridHeaderListing.MasterTableView.ExportToExcel()

    End Sub




    Public Sub ConfigureExport()

        RadGridHeaderListing.ExportSettings.ExportOnlyData = True
        RadGridHeaderListing.ExportSettings.IgnorePaging = True
        RadGridHeaderListing.ExportSettings.OpenInNewWindow = True


    End Sub


End Class
