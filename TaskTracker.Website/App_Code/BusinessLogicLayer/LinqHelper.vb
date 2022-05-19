Imports System.Linq
Imports System.Linq.Expressions

Namespace App_Code.BusinessLogicLayer

    Public Module LinqHelper
        '<System.Runtime.CompilerServices.Extension> _
        <System.Runtime.CompilerServices.Extension()> _
        Public Function OrderByField(Of T)(q As IQueryable(Of T), sortField As String, ascending As Boolean) As IQueryable(Of T)
            Dim param = Expression.Parameter(GetType(T), "p")
            Dim prop = Expression.Property(param, SortField)
            Dim exp = Expression.Lambda(prop, param)
            Dim method As String = If(Ascending, "OrderBy", "OrderByDescending")
            Dim types As Type()
            types = New Type() {q.ElementType, exp.Body.Type}
            Dim mce = Expression.Call(GetType(Queryable), method, types, q.Expression, exp)
            Return q.Provider.CreateQuery(Of T)(mce)
        End Function
    End Module

End Namespace


