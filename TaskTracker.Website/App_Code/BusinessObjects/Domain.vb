Option Explicit On
Option Strict On
Imports HelperDal
Imports Devart.Data.Oracle

Namespace IP.MEAS.BO
    Public Class Domain
#Region "Properties"
        Public Property DomainName() As String
#End Region

#Region "Constructor(s)"
        Public Sub New()

        End Sub
        Public Sub New(ByVal domainName As String)
            Me.DomainName = domainName
        End Sub
#End Region
        Public Shared Function GetListOfDomains() As List(Of Domain)
            Dim cacheKey As String = "MTTDomain"
            Dim cacheHours As Double = 8
            Dim domainlist As List(Of Domain) = Nothing
            Dim instanceOfDomain As New Domain()

            Try
                'Get Domains from Cache
                domainlist = instanceOfDomain.GetDomainsFromCache(cacheKey:=cacheKey, cacheHours:=cacheHours)
                If domainlist IsNot Nothing AndAlso domainlist.Count > 0 Then
                    Return domainlist
                End If

                domainlist = instanceOfDomain.GetDomainsFromDatabase(cacheKey:=cacheKey, cacheHours:=cacheHours)
                If domainlist IsNot Nothing AndAlso domainlist.Count > 0 Then
                    Return domainlist
                End If
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("GetListOfDomains", "Error getting the domain list", ex)
            End Try

            Return domainlist
        End Function

        Private Function GetDomainsFromCache(ByVal cacheKey As String, ByVal cacheHours As Double) As List(Of Domain)
            Dim domainlist As List(Of Domain) = Nothing
            Try
                'Get Domains from Cache
                domainlist = CType(GetDataFromCache(cacheKey, cacheHours), List(Of Domain))
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("GetDomainsFromCache", String.Format("Error getting the Domain List from Cache using {0}", cacheKey), ex)
            End Try
            Return domainlist
        End Function

        Private Function GetDomainsFromDatabase(ByVal cacheKey As String, ByVal cacheHours As Double) As List(Of Domain)
            Dim domainlist As List(Of Domain) = Nothing
            Dim adapter As DataMaintenanceDALTableAdapters.DOMAINLISTTableAdapter
            Dim table As DataMaintenanceDAL.DOMAINLISTDataTable = Nothing
            Dim emptyCursor As Object = Nothing

            Try
                'Get Domains from the database
                domainlist = New List(Of Domain)
                adapter = New DataMaintenanceDALTableAdapters.DOMAINLISTTableAdapter
                adapter.Connection = New OracleConnection(ConfigurationManager.ConnectionStrings.Item("DatabaseConnection").ConnectionString)
                table = adapter.GetData(emptyCursor)
                If table Is Nothing Then Return Nothing

                For Each rowItem As DataMaintenanceDAL.DOMAINLISTRow In table.Rows
                    If rowItem.IsDOMAINNull Then
                        rowItem.DOMAIN = "Missing"
                    End If
                    domainlist.Add(New IP.MEAS.BO.Domain(rowItem.DOMAIN))
                Next
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("GetDomainsFromDatabase", "Error getting the Domain List from the database", ex)
            Finally
                adapter = Nothing
                Table = Nothing
            End Try

            Try
                'Add Domains to Cache
                If domainlist IsNot Nothing AndAlso domainlist.Count > 0 Then InsertDataIntoCache(cacheKey, cacheHours, domainlist)
            Catch ex As Exception
                IP.Bids.SharedFunctions.HandleError("GetDomain", "Error storing the Domain List in Cache", ex)
            End Try
            Return domainlist
        End Function
    End Class
End Namespace