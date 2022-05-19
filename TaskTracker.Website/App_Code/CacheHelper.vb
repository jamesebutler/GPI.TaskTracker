


Public NotInheritable Class CacheHelper
    Public Shared Function DeleteEntireCache() As Integer
        Dim objItem As Object
        Dim strName As String = String.Empty
        Dim currentCache As Caching.Cache = HttpContext.Current.Cache
        Dim deletedCacheCount As Integer = 0
        For Each objItem In currentCache
            strName = objItem.Key
            'Comment the If..Then if you want to see ALL (System, etc.) items the cache
            'We don't want to see ASP.NET cached system items or ASP.NET Worker Processes
            If (Left(strName, 7) <> "ISAPIWo") Then '(Left(strName, 7) <> "System.") And 
                If currentCache.Item(strName) IsNot Nothing Then
                    currentCache.Remove(strName)
                    deletedCacheCount += 1
                End If
            End If
        Next
        Return deletedCacheCount
    End Function
    Public Shared Function DeleteSingleCacheItem(ByVal key As String) As Integer
        Dim currentCache As Caching.Cache = HttpContext.Current.Cache
        Dim deletedCacheCount As Integer = 0
        If currentCache.Item(key) IsNot Nothing Then
            currentCache.Remove(key)
            deletedCacheCount += 1
        End If
        Return deletedCacheCount
    End Function
End Class
