Option Explicit On
Option Strict On
Imports Microsoft.VisualBasic
Imports IP.TaskTrackerDAL.TanksTableAdapters
Imports System.Linq

Namespace BusinessObjects

    Public Class InspectionType
        Private _inspectionId As Integer
        Private _Description As String
        Private _UserType As String

        Public Property InspectionID As Integer
            Get
                Return _inspectionId
            End Get
            Set(value As Integer)
                _inspectionId = value
            End Set
        End Property
        Public Property Description As String
            Get
                Return _Description
            End Get
            Set(value As String)
                _Description = value
            End Set
        End Property
        Public Property UserType As String
            Get
                Return _UserType
            End Get
            Set(value As String)
                _UserType = value
            End Set
        End Property

        Public Sub New(inspectionID As Integer, description As String, userType As String)
            Me.InspectionID = inspectionID
            Me.Description = description
            Me.UserType = userType
        End Sub


        Public Overrides Function ToString() As String
            Return String.Format("InspectionID: {0}, Description: {1}, UserType: {2}", InspectionID, Description, UserType)
        End Function

        Public Shared Function GetInspectionTypeList(ByVal selectedPlantCode As String) As System.Collections.Generic.List(Of InspectionType)
            Dim ta As New InspectionListTableAdapter
            Dim ref As New Object
            Dim listOfTankTypes As New System.Collections.Generic.List(Of InspectionType)
            For Each rowdata As IP.TaskTrackerDAL.Tanks.InspectionListRow In ta.GetInspectionList(selectedPlantCode, ref)
                listOfTankTypes.Add(New InspectionType(CInt(rowdata.INSPECTID), If(rowdata.IsDESCRIPTIONNull, "", rowdata.DESCRIPTION), If(rowdata.IsUSERTYPENull, "", rowdata.USERTYPE)))
            Next

            ta.Dispose()
            Return listOfTankTypes

        End Function

        Public Shared Function GetAllInspectionTypes() As System.Collections.Generic.List(Of InspectionType)
            Return GetInspectionTypeList("All")
        End Function
    End Class

End Namespace
