Imports Microsoft.VisualBasic
'Imports Excel

Public Class CSVReader
    ''' <summary>
    ''' Gets the excel command.
    ''' </summary>
    ''' <param name="fileName">Name of the file.</param>
    ''' <returns></returns>
    Public Shared Function GetExcelCommand(ByVal fileName As String) As OleDb.OleDbCommand
        ' Connect to the Excel Spreadsheet
        Dim connectionString As String = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0", fileName)
        'Dim connectionString As String = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0", fileName)
        'Dim xConnStr As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
        '      "Data Source=" & strFileName & ";" & _
        '      "Extended Properties=Excel 8.0;"

        ' create your excel connection object using the connection string
        Dim objXConn As New OleDb.OleDbConnection(connectionString)
        objXConn.Open()
        ' use a SQL Select command to retrieve the data from the Excel Spreadsheet
        ' the "table name" is the name of the worksheet within the spreadsheet
        ' in this case, the worksheet name is "Members" and is expressed as: [Members$]
        Dim objCommand As New OleDb.OleDbCommand("SELECT * FROM [A1:O65536] ", objXConn)
        Return objCommand
    End Function

    'Public Shared Sub ReadExcel(ByVal filePath As String)
    '    Dim stream As FileStream = File.Open(filePath, FileMode.Open, FileAccess.Read)

    '    '1. Reading from a binary Excel file ('97-2003 format; *.xls)
    '    'Dim excelReader As Excel.IExcelDataReader = ExcelReaderFactory.CreateBinaryReader(stream)
    '    '...
    '    '2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
    '    Dim excelReader As Excel.IExcelDataReader = ExcelReaderFactory.CreateOpenXmlReader(stream)
    '    '...
    '    '3. DataSet - The result of each spreadsheet will be created in the result.Tables
    '    'Dim result As DataSet = excelReader.AsDataSet()
    '    '...
    '    '4. DataSet - Create column names from first row
    '    excelReader.IsFirstRowAsColumnNames = True

    '    Dim result As DataSet = excelReader.AsDataSet()

    '    '5. Data Reader methods

    '    While excelReader.Read()
    '        Dim n1 As String = excelReader.GetName(0) 'GetInt32(0);
    '        Dim n2 As String = excelReader.GetName(1) 'GetInt32(0);
    '    End While

    '    '6. Free resources (IExcelDataReader is IDisposable)
    '    excelReader.Close()
    'End Sub
End Class
