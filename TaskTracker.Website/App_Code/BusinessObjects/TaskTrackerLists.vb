'***********************************************************************
' Assembly         : C:\Personal Documents\Websites\EHSTaskTracker\
' Author           : mjpope
' Created          : 10-27-2010
'
' Last Modified By : mjpope
' Last Modified On : 06-22-2011
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Option Explicit On
Option Strict On
Imports Microsoft.VisualBasic
Namespace IP.MEAS.BO
    Public Class TaskActivity
#Region "Fields"
        Private mActivityName As String = String.Empty
        Private mActivitySeqid As Integer
#End Region

#Region "Properties"
        Public Property ActivityName() As String
            Get
                Return mActivityName
            End Get
            Set(ByVal value As String)
                mActivityName = value
            End Set
        End Property

        Public Property ActivitySeqid() As Integer
            Get
                Return mActivitySeqid
            End Get
            Set(ByVal value As Integer)
                mActivitySeqid = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "activityname"
                        Return ActivityName
                    Case "activityseqid"
                        Return CStr(ActivitySeqid)
                    Case Else
                        Return CStr(ActivitySeqid)
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()
            'Default Constructor
        End Sub
        Public Sub New(ByVal activityName As String, ByVal activitySeqID As Integer)
            mActivityName = activityName
            mActivitySeqid = activitySeqID
        End Sub
#End Region
    End Class

    Public Class TaskType
#Region "Fields"
        Private mTaskName As String = String.Empty
        Private mTaskSeqid As Integer
#End Region

#Region "Properties"
        Public Property TaskName() As String
            Get
                Return mTaskName
            End Get
            Set(ByVal value As String)
                mTaskName = value
            End Set
        End Property

        Public Property TaskSeqid() As Integer
            Get
                Return mTaskSeqid
            End Get
            Set(ByVal value As Integer)
                mTaskSeqid = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "taskname"
                        Return TaskName
                    Case "taskseqid"
                        Return CStr(TaskSeqid)
                    Case Else
                        Return CStr(TaskSeqid)
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()
            'Default Constructor
        End Sub
        Public Sub New(ByVal taskName As String, ByVal taskSeqId As Integer)
            Me.mTaskName = taskName
            Me.mTaskSeqid = taskSeqId
        End Sub
#End Region
    End Class

    Public Class BusinessManagers
#Region "Fields"
        Dim mFullName As String = String.Empty
        Dim mUserName As String = String.Empty
#End Region
        Public Property FullName() As String
            Get
                Return mFullName
            End Get
            Set(ByVal value As String)
                mFullName = value
            End Set
        End Property

        Public Property UserName() As String
            Get
                Return mUserName
            End Get
            Set(ByVal value As String)
                mUserName = value
            End Set
        End Property
        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "fullname"
                        Return mFullName
                    Case "username"
                        Return mUserName
                    Case Else
                        Return mUserName
                End Select
            End Get
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal userName As String, ByVal fullName As String)
            Me.mUserName = userName
            Me.mFullName = fullName
        End Sub
    End Class

    Public Class TypeManagers
#Region "Fields"
        Dim mUserName As String = String.Empty
        Dim mFullName As String = String.Empty
#End Region

#Region "Properties"
        Public Property FullName() As String
            Get
                Return mFullName
            End Get
            Set(ByVal value As String)
                mFullName = value
            End Set
        End Property

        Public Property UserName() As String
            Get
                Return mUserName
            End Get
            Set(ByVal value As String)
                mUserName = value
            End Set
        End Property
        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "fullname"
                        Return mFullName
                    Case "username"
                        Return mUserName
                    Case Else
                        Return mUserName
                End Select
            End Get
        End Property
#End Region

        Public Sub New()

        End Sub
        Public Sub New(ByVal userName As String, ByVal fullName As String)
            Me.mUserName = userName
            Me.mFullName = fullName
        End Sub
    End Class

    Public Class SourceSystems
#Region "Fields"
        Private _externalSource As String = String.Empty
        Private _externalSourceSeqid As Integer
        Private _externalSourceUrl As String = String.Empty
#End Region

#Region "Properties"
        Public Property ExternalSource() As String
            Get
                Return _externalSource
            End Get
            Set(ByVal value As String)
                _externalSource = value
            End Set
        End Property

        Public Property ExternalSourceSeqid() As Integer
            Get
                Return _externalSourceSeqid
            End Get
            Set(ByVal value As Integer)
                _externalSourceSeqid = value
            End Set
        End Property

        Public Property ExternalSourceUrl() As String
            Get
                Return _externalSourceUrl
            End Get
            Set(ByVal value As String)
                _externalSourceUrl = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "externalsource"
                        Return _externalSource
                    Case "externalsourceseqid"
                        Return CStr(_externalSourceSeqid)
                    Case Else
                        Return CStr(_externalSourceSeqid)
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()

        End Sub
        Public Sub New(ByVal externalSource As String, ByVal externalSourceSeqId As Integer, ByVal externalSourceUrl As String)
            Me._externalSource = externalSource
            Me._externalSourceSeqid = externalSourceSeqId
            Me._externalSourceUrl = externalSourceUrl
        End Sub
#End Region
    End Class

    Public Class Division
#Region "Fields"
        Private mDivisionName As String = String.Empty
#End Region

#Region "Properties"
        Public Property DivisionName() As String
            Get
                Return mDivisionName
            End Get
            Set(ByVal value As String)
                mDivisionName = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "divisionname"
                        Return DivisionName
                    Case Else
                        Return DivisionName
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()

        End Sub

        Public Sub New(ByVal divisionName As String)
            Me.mDivisionName = divisionName
        End Sub
#End Region
    End Class

    Public Class Region
#Region "Fields"
        Private mRegionName As String = String.Empty
#End Region

#Region "Properties"
        Public Property RegionName() As String
            Get
                Return mRegionName
            End Get
            Set(ByVal value As String)
                mRegionName = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "regionname"
                        Return RegionName
                    Case Else
                        Return RegionName
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()
            'Default Constructor
        End Sub
        Public Sub New(ByVal regionName As String)
            Me.mRegionName = regionName
        End Sub
#End Region
    End Class

    Public Class RecurringTasks
#Region "Fields"
        Private _dueDate As String
        Private _taskItemSeqId As Integer
        Private _rootTaskItemSeqId As Integer
        Private _statusSeqId As Integer
#End Region

#Region "Properties"

        Public Property StatusSeqId() As Integer
            Get
                Return _statusSeqId
            End Get
            Set(ByVal value As Integer)
                _statusSeqId = value
            End Set
        End Property
        Public Property DueDate() As String
            Get
                Return _dueDate
            End Get
            Set(ByVal value As String)
                _dueDate = value
            End Set
        End Property
        Public Property TaskItemSeqId() As Integer
            Get
                Return _taskItemSeqId
            End Get
            Set(ByVal value As Integer)
                _taskItemSeqId = value
            End Set
        End Property
        Public Property RootTaskItemSeqId() As Integer
            Get
                Return _rootTaskItemSeqId
            End Get
            Set(ByVal value As Integer)
                _rootTaskItemSeqId = value
            End Set
        End Property
#End Region

        Public Sub New(ByVal dueDate As String, ByVal taskItemSeqId As Integer, ByVal rootTaskItemSeqId As Integer, ByVal statusSeqId As Integer)
            _dueDate = dueDate
            _taskItemSeqId = taskItemSeqId
            _rootTaskItemSeqId = rootTaskItemSeqId
            _statusSeqId = statusSeqId
        End Sub
    End Class

    Public Class Employee
#Region "Fields"
        Private _NetworkId As String
        Private _FirstName As String
        Private _LastName As String
        Private _MiddleInit As String
        Private _Phone As String
        Private _Email As String
        Private _Domain As String
        Private _DefaultLang As String
        Private _InactiveFlag As String
        Private _TaskCount As String



#End Region

#Region "Properties"


        Public Property NetworkId() As String
            Get
                Return _NetworkId
            End Get
            Set(ByVal value As String)
                _NetworkId = value
            End Set
        End Property
        Public Property FirstName() As String
            Get
                Return _FirstName
            End Get
            Set(ByVal value As String)
                _FirstName = value
            End Set
        End Property
        Public Property LastName() As String
            Get
                Return _LastName
            End Get
            Set(ByVal value As String)
                _LastName = value
            End Set
        End Property
        Public Property MiddleInit() As String
            Get
                Return _MiddleInit
            End Get
            Set(ByVal value As String)
                _MiddleInit = value
            End Set
        End Property
        Public Property Phone() As String
            Get
                Return _Phone
            End Get
            Set(ByVal value As String)
                _Phone = value
            End Set
        End Property
        Public Property Email() As String
            Get
                Return _Email
            End Get
            Set(ByVal value As String)
                _Email = value
            End Set
        End Property
        Public Property Domain() As String
            Get
                Return _Domain
            End Get
            Set(ByVal value As String)
                _Domain = value
            End Set
        End Property
        Public Property DefaultLang() As String
            Get
                Return _DefaultLang
            End Get
            Set(ByVal value As String)
                _DefaultLang = value
            End Set
        End Property
        Public Property InactiveFlag() As String
            Get
                Return _InactiveFlag
            End Get
            Set(ByVal value As String)
                _InactiveFlag = value
            End Set
        End Property
        Public Property TaskCount() As String
            Get
                Return _TaskCount
            End Get
            Set(ByVal value As String)
                _TaskCount = value
            End Set
        End Property
#End Region

        Public Sub New(ByVal NetworkId As String, ByVal FirstName As String, ByVal LastName As String, ByVal MiddleInit As String, ByVal Email As String, ByVal Phone As String, ByVal Domain As String, ByVal DefaultLang As String, ByVal InactiveFlag As String, ByVal TaskCount As String)
            _NetworkId = NetworkId
            _FirstName = FirstName
            _LastName = LastName
            _MiddleInit = MiddleInit
            _Phone = Phone
            _Email = Email
            _Domain = Domain
            _DefaultLang = DefaultLang
            _InactiveFlag = InactiveFlag
            _TaskCount = TaskCount

        End Sub
    End Class

    Public Class EmployeeProfile
#Region "Fields"
        Private _NetworkId As String
        Private _FirstName As String
        Private _LastName As String
        Private _Domain As String
        Private _SiteName As String
        Private _PlantCode As String


#End Region

#Region "Properties"


        Public Sub New(networkId As String, firstName As String, lastName As String, siteName As String, plantCode As String, domain As String)
            _NetworkId = networkId
            _FirstName = firstName
            _LastName = lastName
            _SiteName = siteName
            _PlantCode = plantCode
            _Domain = domain
        End Sub

        Public Property NetworkId() As String
            Get
                Return _NetworkId
            End Get
            Set(ByVal value As String)
                _NetworkId = value
            End Set
        End Property
        Public Property FirstName() As String
            Get
                Return _FirstName
            End Get
            Set(ByVal value As String)
                _FirstName = value
            End Set
        End Property
        Public Property LastName() As String
            Get
                Return _LastName
            End Get
            Set(ByVal value As String)
                _LastName = value
            End Set
        End Property
        Public Property SiteName() As String
            Get
                Return _SiteName
            End Get
            Set(ByVal value As String)
                _SiteName = value
            End Set
        End Property
        Public Property PlantCode() As String
            Get
                Return _PlantCode
            End Get
            Set(ByVal value As String)
                _PlantCode = value
            End Set
        End Property
        
        Public Property Domain() As String
            Get
                Return _Domain
            End Get
            Set(ByVal value As String)
                _Domain = value
            End Set
        End Property
               
#End Region

       
    End Class

    Public Class SiteUserRole
#Region "Fields"
        Private _RoleDescription As String
        Private _RoleId As String
        Private _RoleName As String
        Private _AssigneeName As String
        Private _AssigneeNetworkId As String
        Private _BusUnit As String
        Private _Area As String
        Private _Line As String
        Private _PlantCode As String


#End Region

#Region "Properties"


        Public Property AssigneeNetworkId() As String
            Get
                Return _AssigneeNetworkId
            End Get
            Set(ByVal value As String)
                _AssigneeNetworkId = value
            End Set
        End Property
        Public Property RoleDescription() As String
            Get
                Return _RoleDescription
            End Get
            Set(ByVal value As String)
                _RoleDescription = value
            End Set
        End Property
        Public Property RoleName() As String
            Get
                Return _RoleName
            End Get
            Set(ByVal value As String)
                _RoleName = value
            End Set
        End Property
        Public Property AssigneeName() As String
            Get
                Return _AssigneeName
            End Get
            Set(ByVal value As String)
                _AssigneeName = value
            End Set
        End Property
        Public Property BusUnit() As String
            Get
                Return _BusUnit
            End Get
            Set(ByVal value As String)
                _BusUnit = value
            End Set
        End Property
        Public Property Area() As String
            Get
                Return _Area
            End Get
            Set(ByVal value As String)
                _Area = value
            End Set
        End Property
        Public Property Line() As String
            Get
                Return _Line
            End Get
            Set(ByVal value As String)
                _Line = value
            End Set
        End Property
        Public Property RoleId() As String
            Get
                Return _RoleId
            End Get
            Set(ByVal value As String)
                _RoleId = value
            End Set
        End Property
        Public Property PlantCode() As String
            Get
                Return _PlantCode
            End Get
            Set(ByVal value As String)
                _PlantCode = value
            End Set
        End Property
#End Region

        Public Sub New(ByVal PlantCode As String, ByVal RoleDescription As String, ByVal AssigneeName As String, ByVal AssigneeNetworkId As String, ByVal RoleId As String, ByVal RoleName As String, ByVal BusUnit As String, ByVal Area As String, ByVal Line As String)
            _PlantCode = PlantCode
            _RoleDescription = RoleDescription
            _AssigneeName = AssigneeName
            _AssigneeNetworkId = AssigneeNetworkId
            _RoleId = RoleId
            _RoleName = RoleName
            _BusUnit = BusUnit
            _Area = Area
            _Line = Line



        End Sub
    End Class
    Public Class BusType
#Region "Fields"
        Private _BusType As String
        Private _BusName As String

#End Region

#Region "Properties"

        Public Property BusType() As String
            Get
                Return _BusType
            End Get
            Set(ByVal value As String)
                _BusType = value
            End Set
        End Property
        Public Property BusName() As String
            Get
                Return _BusName
            End Get
            Set(ByVal value As String)
                _BusName = value
            End Set
        End Property

#End Region

        Public Sub New(ByVal BusType As String, ByVal BusName As String)
            _BusType = BusType
            _BusName = BusName
        End Sub
    End Class

    '    Public Class Division
    '#Region "Fields"
    '        Private _Division As String


    '#End Region

    '#Region "Properties"

    '        Public Property Division() As String
    '            Get
    '                Return _Division
    '            End Get
    '            Set(ByVal value As String)
    '                _Division = value
    '            End Set
    '        End Property


    '#End Region

    '        Public Sub New(ByVal BusType As String)
    '            _Division = Division
    '        End Sub
    '    End Class
    Public Class RoleBySite
#Region "Fields"
        Private _RoleDescription As String
        Private _RoleId As String
        Private _RoleName As String
        Private _AssigneeName As String
        Private _AssigneeNetworkId As String
        Private _BusUnit As String
        Private _Area As String
        Private _Line As String
        Private _PlantCode As String
        Private _Sitename As String


#End Region

#Region "Properties"


        Public Property AssigneeNetworkId() As String
            Get
                Return _AssigneeNetworkId
            End Get
            Set(ByVal value As String)
                _AssigneeNetworkId = value
            End Set
        End Property
        Public Property RoleDescription() As String
            Get
                Return _RoleDescription
            End Get
            Set(ByVal value As String)
                _RoleDescription = value
            End Set
        End Property
        Public Property RoleName() As String
            Get
                Return _RoleName
            End Get
            Set(ByVal value As String)
                _RoleName = value
            End Set
        End Property
        Public Property AssigneeName() As String
            Get
                Return _AssigneeName
            End Get
            Set(ByVal value As String)
                _AssigneeName = value
            End Set
        End Property
        Public Property BusUnit() As String
            Get
                Return _BusUnit
            End Get
            Set(ByVal value As String)
                _BusUnit = value
            End Set
        End Property
        Public Property Area() As String
            Get
                Return _Area
            End Get
            Set(ByVal value As String)
                _Area = value
            End Set
        End Property
        Public Property Line() As String
            Get
                Return _Line
            End Get
            Set(ByVal value As String)
                _Line = value
            End Set
        End Property
        Public Property RoleId() As String
            Get
                Return _RoleId
            End Get
            Set(ByVal value As String)
                _RoleId = value
            End Set
        End Property
        Public Property PlantCode() As String
            Get
                Return _PlantCode
            End Get
            Set(ByVal value As String)
                _PlantCode = value
            End Set
        End Property
        Public Property SiteName() As String
            Get
                Return _Sitename
            End Get
            Set(ByVal value As String)
                _Sitename = value
            End Set
        End Property
#End Region

        Public Sub New(ByVal SiteName As String, ByVal PlantCode As String, ByVal RoleDescription As String, ByVal AssigneeName As String, ByVal AssigneeNetworkId As String, ByVal RoleId As String, ByVal RoleName As String, ByVal BusUnit As String, ByVal Area As String, ByVal Line As String)
            _Sitename = SiteName
            _PlantCode = PlantCode
            _RoleDescription = RoleDescription
            _AssigneeName = AssigneeName
            _AssigneeNetworkId = AssigneeNetworkId
            _RoleId = RoleId
            _RoleName = RoleName
            _BusUnit = BusUnit
            _Area = Area
            _Line = Line



        End Sub
    End Class
    Public Class ReassignTask
#Region "Fields"
        Private _Sitename As String
        Private _Plantcode As String
        Private _HeaderTitle As String
        Private _TaskTitle As String
        Private _TaskDescription As String
        Private _Taskitemseqid As String
        Private _Taskheaderseqid As String
        Private _Duedate As String
        Private _Responsibleuser As String

#End Region

#Region "Properties"


        Public Property Sitename() As String
            Get
                Return _Sitename
            End Get
            Set(ByVal value As String)
                _Sitename = value
            End Set
        End Property
        Public Property PlantCode() As String
            Get
                Return _Plantcode
            End Get
            Set(ByVal value As String)
                _Plantcode = value
            End Set
        End Property
        Public Property HeaderTitle() As String
            Get
                Return _HeaderTitle
            End Get
            Set(ByVal value As String)
                _HeaderTitle = value
            End Set
        End Property
        Public Property TaskTitle() As String
            Get
                Return _TaskTitle
            End Get
            Set(ByVal value As String)
                _TaskTitle = value
            End Set
        End Property
        Public Property TaskDescription() As String
            Get
                Return _TaskDescription
            End Get
            Set(ByVal value As String)
                _TaskDescription = value
            End Set
        End Property
        Public Property Taskitemseqid() As String
            Get
                Return _Taskitemseqid
            End Get
            Set(ByVal value As String)
                _Taskitemseqid = value
            End Set
        End Property
        Public Property Taskheaderseqid() As String
            Get
                Return _Taskheaderseqid
            End Get
            Set(ByVal value As String)
                _Taskheaderseqid = value
            End Set
        End Property
        Public Property Duedate() As String
            Get
                Return _Duedate
            End Get
            Set(ByVal value As String)
                _Duedate = value
            End Set
        End Property
        Public Property Responsibleuser() As String
            Get
                Return _Responsibleuser
            End Get
            Set(ByVal value As String)
                _Responsibleuser = value
            End Set
        End Property
#End Region
        Public Sub New(ByVal SiteName As String, ByVal PlantCode As String, ByVal HeaderTitle As String, ByVal TaskTitle As String, ByVal TaskDescription As String, ByVal Taskitemseqid As String, ByVal Taskheaderseqid As String, ByVal DueDate As String, ByVal ResponsibleUser As String)
            _Sitename = SiteName
            _Plantcode = PlantCode
            _HeaderTitle = HeaderTitle
            _TaskTitle = TaskTitle
            _TaskDescription = TaskDescription
            _Taskitemseqid = Taskitemseqid
            _Taskheaderseqid = Taskheaderseqid
            _Duedate = DueDate
            _Responsibleuser = ResponsibleUser

        End Sub
    End Class

    Public Class Role
#Region "Fields"
        Private _RoleId As String
        Private _RoleName As String
        Private _RoleDescription As String
        Private _RoleType As String
        Private _DisplayFlag As String

#End Region

#Region "Properties"



        Public Property RoleDescription() As String
            Get
                Return _RoleDescription
            End Get
            Set(ByVal value As String)
                _RoleDescription = value
            End Set
        End Property
        Public Property RoleName() As String
            Get
                Return _RoleName
            End Get
            Set(ByVal value As String)
                _RoleName = value
            End Set
        End Property

        Public Property RoleId() As String
            Get
                Return _RoleId
            End Get
            Set(ByVal value As String)
                _RoleId = value
            End Set
        End Property
        Public Property RoleType() As String
            Get
                Return _RoleType
            End Get
            Set(ByVal value As String)
                _RoleType = value
            End Set
        End Property
        Public Property DisplayFlag() As String
            Get
                Return _DisplayFlag
            End Get
            Set(ByVal value As String)
                _DisplayFlag = value
            End Set
        End Property
#End Region

        Public Sub New(ByVal RoleId As String, ByVal RoleName As String, ByVal RoleDescription As String, ByVal RoleType As String, ByVal DisplayFlag As String)

            _RoleId = RoleId
            _RoleName = RoleName
            _RoleDescription = RoleDescription
            _RoleType = RoleType
            _DisplayFlag = DisplayFlag

        End Sub
    End Class
    Public Class Facility
#Region "Fields"
        Private mSiteName As String = String.Empty
        Private mPlantCode As String = String.Empty
#End Region

#Region "Properties"
        Public Property SiteName() As String
            Get
                Return mSiteName
            End Get
            Set(ByVal value As String)
                mSiteName = value
            End Set
        End Property

        Public Property PlantCode() As String
            Get
                Return mPlantCode
            End Get
            Set(ByVal value As String)
                mPlantCode = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "sitename"
                        Return SiteName
                    Case "plantcode"
                        Return PlantCode
                    Case Else
                        Return PlantCode
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()

        End Sub
        Public Sub New(ByVal siteName As String, ByVal plantCode As String)
            Me.mSiteName = siteName
            Me.mPlantCode = plantCode
        End Sub
#End Region
    End Class

    Public Class BusinessUnitArea
#Region "Fields"
        Private mBusinessUnit As String = String.Empty
#End Region

#Region "Properties"
        Public Property BusinessUnit() As String
            Get
                Return mBusinessUnit
            End Get
            Set(ByVal value As String)
                mBusinessUnit = value
            End Set
        End Property



        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "businessunit"
                        Return BusinessUnit
                    Case Else
                        Return BusinessUnit
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()

        End Sub
        Public Sub New(ByVal businessUnit As String)
            Me.mBusinessUnit = businessUnit
        End Sub
#End Region
    End Class

    Public Class Area
#Region "Fields"
        Private mArea As String = String.Empty
#End Region

#Region "Properties"
        Public Property Area() As String
            Get
                Return mArea
            End Get
            Set(ByVal value As String)
                mArea = value
            End Set
        End Property



        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "area"
                        Return Area
                    Case Else
                        Return Area
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()

        End Sub
        Public Sub New(ByVal area As String)
            Me.mArea = area
        End Sub
#End Region
    End Class

    Public Class BusinessUnit
#Region "Fields"
        Private mBusinessUnit As String = String.Empty
#End Region

#Region "Properties"
        Public Property BusinessUnit() As String
            Get
                Return mBusinessUnit
            End Get
            Set(ByVal value As String)
                mBusinessUnit = value
            End Set
        End Property



        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "businessunit"
                        Return BusinessUnit
                    Case Else
                        Return BusinessUnit
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()

        End Sub
        Public Sub New(ByVal businessUnit As String)
            Me.mBusinessUnit = businessUnit
        End Sub
#End Region
    End Class

    Public Class LineLineBreak
#Region "Fields"
        Private mLineBreak As String = String.Empty
#End Region

#Region "Properties"
        Public Property LineBreak() As String
            Get
                Return mLineBreak
            End Get
            Set(ByVal value As String)
                mLineBreak = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "linebreak"
                        Return LineBreak
                    Case Else
                        Return LineBreak
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()

        End Sub
        Public Sub New(ByVal lineBreak As String)
            Me.mLineBreak = lineBreak
        End Sub
#End Region
    End Class

    Public Class Line
#Region "Fields"
        Private mLine As String = String.Empty
#End Region

#Region "Properties"
        Public Property Line() As String
            Get
                Return mLine
            End Get
            Set(ByVal value As String)
                mLine = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "line"
                        Return Line
                    Case Else
                        Return Line
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()

        End Sub
        Public Sub New(ByVal line As String)
            Me.mLine = line
        End Sub
#End Region
    End Class

    Public Class LineBreak
#Region "Fields"
        Private mLineBreak As String = String.Empty
#End Region

#Region "Properties"
        Public Property LineBreak() As String
            Get
                Return mLineBreak
            End Get
            Set(ByVal value As String)
                mLineBreak = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "linebreak"
                        Return LineBreak
                    Case Else
                        Return LineBreak
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()

        End Sub
        Public Sub New(ByVal lineBreak As String)
            Me.mLineBreak = lineBreak
        End Sub
#End Region
    End Class

    <Serializable()>
    Public Class TaskHeaderRecord
#Region "Fields"
        Private mBusinessUnitArea As String
        Private mLine As String
        Private mSiteName As String
        Private mActivityName As String
        'Private mExternalSourceUrl As String
        Private mStartDate As Date
        Private mEndDate As Date
        Private mCreateDate As Date
        Private mLastUpdateDate As Date 'String
        Private mLastReplicatedDate As String
        Private mAttachmentCount As Integer
        Private mExternalSourceName As String
        Private mTaskHeaderSeqID As Integer
        Private mTitle As String
        Private mExternalRef As String
        Private mExternalSourceSeqID As String
        Private mSecurityLevel As String
        Private mActivitySeqID As Integer
        Private mPlantCode As String
        Private mCreatedBy As String
        Private mLastUpdateUserName As String
        Private mDescription As String
        Private mRegion As String
        Private mDivision As String
        Private mTaskTypes As String
#End Region
#Region "Properties"
        ''' <summary>
        ''' Gets or sets the business unit area.
        ''' </summary>
        ''' <value>The business unit area.</value>
        Public Property BusinessUnitArea() As String
            Get
                Return mBusinessUnitArea
            End Get
            Set(ByVal value As String)
                mBusinessUnitArea = value
            End Set
        End Property

        Public Property TaskTypes() As String
            Get
                Return mTaskTypes
            End Get
            Set(ByVal value As String)
                mTaskTypes = value
            End Set
        End Property
        ''' <summary>
        ''' Gets or sets the line.
        ''' </summary>
        ''' <value>The line.</value>
        Public Property Line() As String
            Get
                Return mLine
            End Get
            Set(ByVal value As String)
                mLine = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the name of the activity.
        ''' </summary>
        ''' <value>The name of the activity.</value>
        Public Property ActivityName() As String
            Get
                Return mActivityName
            End Get
            Set(ByVal value As String)
                mActivityName = value
            End Set
        End Property
        Public Property ActivitySeqID() As Integer
            Get
                Return mActivitySeqID
            End Get
            Set(ByVal value As Integer)
                mActivitySeqID = value
            End Set
        End Property
        Public Property AttachmentCount() As Integer
            Get
                Return mAttachmentCount
            End Get
            Set(ByVal value As Integer)
                mAttachmentCount = value
            End Set
        End Property
        Public Property CreateDate() As Date
            Get
                Return mCreateDate
            End Get
            Set(ByVal value As Date)
                mCreateDate = value
            End Set
        End Property
        Public Property CreatedBy() As String
            Get
                Return mCreatedBy
            End Get
            Set(ByVal value As String)
                mCreatedBy = value
            End Set
        End Property
        Public Property Description() As String
            Get
                Return mDescription
            End Get
            Set(ByVal value As String)
                mDescription = value
            End Set
        End Property
        Public Property Division() As String
            Get
                Return mDivision
            End Get
            Set(ByVal value As String)
                mDivision = value
            End Set
        End Property
        Public Property EndDate() As Date
            Get
                Return mEndDate
            End Get
            Set(ByVal value As Date)
                mEndDate = value
            End Set
        End Property
        Public Property LastReplicatedDate() As String
            Get
                Return mLastReplicatedDate
            End Get
            Set(ByVal value As String)
                mLastReplicatedDate = value
            End Set
        End Property
        Public Property ExternalRef() As String
            Get
                Return mExternalRef
            End Get
            Set(ByVal value As String)
                mExternalRef = value
            End Set
        End Property
        Public Property ExternalSourceName() As String
            Get
                Return mExternalSourceName
            End Get
            Set(ByVal value As String)
                mExternalSourceName = value
            End Set
        End Property
        Public Property ExternalSourceSeqID() As String
            Get
                Return mExternalSourceSeqID
            End Get
            Set(ByVal value As String)
                mExternalSourceSeqID = value
            End Set
        End Property
        'Public Property ExternalSourceUrl() As String
        '    Get
        '        Return mExternalSourceUrl
        '    End Get
        '    Set(ByVal value As String)
        '        mExternalSourceUrl = value
        '    End Set
        'End Property
        Public Property LastUpdateDate() As Date 'String
            Get
                Return mLastUpdateDate
            End Get
            Set(ByVal value As Date)
                mLastUpdateDate = value
            End Set
        End Property
        Public Property LastUpdateUserName() As String
            Get
                Return mLastUpdateUserName
            End Get
            Set(ByVal value As String)
                mLastUpdateUserName = value
            End Set
        End Property
        Public Property PlantCode() As String
            Get
                Return mPlantCode
            End Get
            Set(ByVal value As String)
                mPlantCode = value
            End Set
        End Property
        Public Property Region() As String
            Get
                Return mRegion
            End Get
            Set(ByVal value As String)
                mRegion = value
            End Set
        End Property
        Public Property SecurityLevel() As String
            Get
                Return mSecurityLevel
            End Get
            Set(ByVal value As String)
                mSecurityLevel = value
            End Set
        End Property
        Public Property SiteName() As String
            Get
                Return mSiteName
            End Get
            Set(ByVal value As String)
                mSiteName = value
            End Set
        End Property
        Public Property StartDate() As Date
            Get
                Return mStartDate
            End Get
            Set(ByVal value As Date)
                mStartDate = value
            End Set
        End Property
        Public Property TaskHeaderSeqID() As Integer
            Get
                Return mTaskHeaderSeqID
            End Get
            Set(ByVal value As Integer)
                mTaskHeaderSeqID = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the title.
        ''' </summary>
        ''' <value>The title.</value>
        Public Property Title() As String
            Get
                Return mTitle
            End Get
            Set(ByVal value As String)
                mTitle = value
            End Set
        End Property


        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "activityname"
                        Return Me.mActivityName
                    Case "activityseqid"
                        Return CStr(Me.mActivitySeqID)
                    Case "attachmentcount"
                        Return CStr(Me.mAttachmentCount)
                    Case "businessunitarea"
                        Return Me.mBusinessUnitArea
                    Case "createdate"
                        Return CStr(Me.mCreateDate)
                    Case "createdby"
                        Return Me.mCreatedBy
                    Case "description"
                        Return Me.mDescription
                    Case "division"
                        Return Me.mDivision
                    Case "enddate"
                        Return CStr(Me.mEndDate)
                    Case "externalref"
                        Return Me.mExternalRef
                    Case "externalsourcename"
                        Return Me.mExternalSourceName
                    Case "externalsourceseqid"
                        Return Me.mExternalSourceSeqID
                    Case "lastupdatedate"
                        Return Me.mLastUpdateDate.ToShortDateString
                    Case "lastupdateuserName"
                        Return Me.mLastUpdateUserName
                    Case "line"
                        Return Me.mLine
                    Case "plantcode"
                        Return Me.mPlantCode
                    Case "region"
                        Return Me.mRegion
                    Case "securitylevel"
                        Return Me.mSecurityLevel
                    Case "sitename"
                        Return Me.mSiteName
                    Case "startdate"
                        Return CStr(Me.mStartDate)
                    Case "taskheaderseqid"
                        Return CStr(Me.mTaskHeaderSeqID)
                    Case "tasktypes"
                        Return Me.mTaskTypes
                    Case "title"
                        Return Me.mTitle
                    Case Else
                        Return CStr(Me.mTaskHeaderSeqID)
                End Select
            End Get
        End Property
#End Region
        Public Overrides Function ToString() As String
            Return Me.GetXml(Me)
        End Function
        Public Sub New(ByVal businessUnitArea As String, ByVal line As String, ByVal siteName As String, ByVal activityName As String, ByVal startDate As Date, ByVal endDate As Date, ByVal createDate As Date, ByVal lastUpdateDate As String, ByVal attachmentCount As Integer, ByVal externalSourceName As String, ByVal taskHeaderSeqID As Integer, ByVal title As String, ByVal externalRef As String, ByVal externalSourceSeqID As String, ByVal securityLevel As String, ByVal activitySeqID As Integer, ByVal plantCode As String, ByVal createdBy As String, ByVal lastUpdateUserName As String, ByVal description As String, ByVal region As String, ByVal division As String, ByVal taskTypes As String, ByVal lastReplicatedDate As String) ', ByVal externalSourceUrl As String)
            mBusinessUnitArea = businessUnitArea
            mLine = line
            mSiteName = siteName
            mActivityName = activityName
            mStartDate = startDate
            mEndDate = endDate
            mCreateDate = createDate
            If IsDate(lastUpdateDate) Then
                mLastUpdateDate = CDate(lastUpdateDate)
                mLastUpdateUserName = lastUpdateUserName
            ElseIf lastUpdateDate.Length > 0 Then
                mLastUpdateDate = CDate(lastUpdateDate)
            Else
                mLastUpdateDate = createDate
                mLastUpdateUserName = createdBy
            End If
            mLastReplicatedDate = lastReplicatedDate
            mAttachmentCount = attachmentCount
            mExternalSourceName = externalSourceName
            mTaskHeaderSeqID = taskHeaderSeqID
            mTitle = title
            mExternalRef = externalRef
            mExternalSourceSeqID = externalSourceSeqID
            mSecurityLevel = securityLevel
            mActivitySeqID = activitySeqID
            mPlantCode = plantCode
            mCreatedBy = createdBy
            'mExternalSourceUrl = externalSourceUrl
            mDescription = description
            mRegion = region
            mDivision = division
            mTaskTypes = taskTypes
        End Sub
        Public Sub New()

        End Sub

        Private Function GetXml(ByVal instanceOfObject As TaskHeaderRecord) As String
            Try
                If instanceOfObject IsNot Nothing Then
                    Dim xmlRFQ As New System.Xml.Serialization.XmlSerializer(instanceOfObject.GetType)
                    Dim swRFQWriter As New IO.StringWriter()
                    xmlRFQ.Serialize(swRFQWriter, instanceOfObject)

                    Return swRFQWriter.ToString()
                Else
                    Throw New Exception("GetXml: Specified object is nothing")
                End If
            Catch ex As Exception
                Throw New Exception("GetXml: Could Not Serialize object to string", ex)
            End Try
        End Function

        ''' <summary>
        ''' XMLs to object.	
        ''' </summary>
        ''' <param name="containingXml">The containing XML.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function LoadXML(ByVal containingXml As String) As TaskHeaderRecord
            Try
                Dim x As New System.Xml.Serialization.XmlSerializer(GetType(TaskHeaderRecord))
                Dim sr As New IO.StringReader(containingXml)
                Dim obj As TaskHeaderRecord = CType(x.Deserialize(sr), TaskHeaderRecord)
                Return obj
            Catch ex As Exception
                Return Nothing
            End Try
        End Function
    End Class

    Public Class MttDocuments
#Region "Fields"
        Private mTaskHeaderNumber As Integer
        Private mFileName As String
        Private mLocation As String
        Private mDescription As String
        Private mTaskDocumentSeqID As Integer

#End Region

#Region "Properties"
        Public Property TaskHeaderNumber() As Integer
            Get
                Return mTaskHeaderNumber
            End Get
            Set(ByVal Value As Integer)
                mTaskHeaderNumber = Value
            End Set
        End Property

        Public Property FileName() As String
            Get
                Return mFileName
            End Get
            Set(ByVal value As String)
                mFileName = value
            End Set
        End Property

        Public Property Location() As String
            Get
                Return mLocation
            End Get
            Set(ByVal value As String)
                mLocation = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return mDescription
            End Get
            Set(ByVal value As String)
                mDescription = value
            End Set
        End Property

        Public Property TaskDocumentSeqID() As Integer
            Get
                Return mTaskDocumentSeqID
            End Get
            Set(ByVal Value As Integer)
                mTaskDocumentSeqID = Value
            End Set
        End Property
#End Region

        Public Sub New()

        End Sub
        Public Sub New(ByVal taskHeaderID As Integer, ByVal fileName As String, ByVal location As String, ByVal description As String, ByVal taskDocumentID As Integer)
            Me.mTaskHeaderNumber = taskHeaderID
            Me.mTaskDocumentSeqID = taskDocumentID
            Me.mFileName = fileName
            Me.mDescription = description
            Me.mLocation = location
        End Sub
    End Class

    Public Class TaskPriorities
#Region "Enums"
        Public Enum TaskItemPriority
            Low = 1
            Medium = 2
            High = 3
        End Enum
#End Region
#Region "Fields"
        Private _priorityID As TaskItemPriority
        Private _priorityName As String
#End Region

#Region "Properties"

        Public Property PriorityID() As TaskItemPriority
            Get
                Return _priorityID
            End Get
            Set(ByVal value As TaskItemPriority)
                _priorityID = value
            End Set
        End Property

        Public Property PriorityName() As String
            Get
                Return _priorityName
            End Get
            Set(ByVal value As String)
                _priorityName = value
            End Set
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()

        End Sub

        Public Sub New(ByVal id As TaskItemPriority, ByVal priorityName As String)
            Me._priorityID = id
            Me._priorityName = priorityName
        End Sub
#End Region


    End Class

    Public Class ResponsibleUsers
#Region "Fields"
        Dim mSortOrder As Integer = 1
        Dim mRoleDescription As String = String.Empty
        Dim mName As String = String.Empty
        Dim mUserName As String = String.Empty
        Dim mRoleSeqID As String = String.Empty
        Dim mRoleName As String = String.Empty
#End Region
        Public Property FullName() As String

        Public Property UserName() As String

        Public Property SortOrder() As Integer

        Public Property RoleSeqID() As String

        Public Property RoleName() As String

        Public Property RoleDescription() As String

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "fullname"
                        Return FullName
                    Case "username"
                        Return UserName
                    Case Else
                        Return UserName
                End Select
            End Get
        End Property
        Public Property InActiveFlag As String
        Public Sub New()

        End Sub

        Public Sub New(ByVal sortOrder As Integer, ByVal roleDescription As String, ByVal fullName As String, ByVal userName As String, ByVal roleSeqId As String, ByVal roleName As String, ByVal inactiveFlag As String)
            Me.SortOrder = sortOrder
            Me.RoleDescription = roleDescription
            Me.FullName = fullName
            Me.UserName = userName
            Me.RoleSeqID = roleSeqId
            Me.RoleName = roleName
            Me.InActiveFlag = inactiveFlag
        End Sub
    End Class

    Public Class TaskItem


#Region "Fields"
        Private mTaskItemSeqId As Integer
        Private mTaskHeaderSeqId As Integer
        Private mTitle As String = String.Empty
        Private mPriority As String = String.Empty
        Private mDateCritical As String = String.Empty
        Private mDueDate As String = String.Empty
        Private mClosedDate As String = String.Empty
        Private mStatusSeqId As Integer
        Private mStatusName As String = String.Empty
        Private mLeadTime As Integer
        Private mRootTaskItemSeqId As String
        Private mResponsibleName As String = String.Empty
        Private mResponsibleUserName As String = String.Empty
        Private mRoleName As String = String.Empty
        Private mResponsibleRoleSeqId As Integer
        Private mCreatedBy As String = String.Empty
        Private mCreatedDate As String = String.Empty
        Private mLastUpdateUserName As String = String.Empty
        Private mLastUpdateDate As String = String.Empty
        Private mDescription As String = String.Empty
        Private mComments As String = String.Empty
        Private mUpdateFlag As String = "Current"
        Private mResponsibleRolePlantCode As String = String.Empty
        Private mResponsibleRoleSiteName As String = String.Empty
        Private mRoleDescription As String = String.Empty
        Private mRecurringTasks As System.Collections.Generic.List(Of RecurringTasks)
        Private mDaysBefore As Integer
        Private mDependenttaskseqid As String = String.Empty
        Private mDependentChildSeqid As String = String.Empty
        Private mDaysAfter As Integer
        Private mWorkOrder As String = String.Empty

        Private mSortOrder As String = String.Empty
        Private mEstimatedCost As String = String.Empty
        Private mActualCost As String = String.Empty
        Private mTankInspectionId As String = String.Empty
        Private mOriginalRoleSeqId As String = String.Empty
#End Region

#Region "Properties"
        Public Property SortOrder() As String
            Get
                Return Me.mSortOrder
            End Get
            Set(value As String)
                Me.mSortOrder = value
            End Set
        End Property

        Public Property EstimatedCost() As String
            Get
                Return Me.mEstimatedCost
            End Get
            Set(value As String)
                Me.mEstimatedCost = value
            End Set
        End Property

        Public Property ActualCost() As String
            Get
                Return Me.mActualCost
            End Get
            Set(value As String)
                Me.mActualCost = value
            End Set
        End Property

        Public Property TankInspectionId() As String
            Get
                Return Me.mTankInspectionId
            End Get
            Set(value As String)
                Me.mTankInspectionId = value
            End Set
        End Property

        Public Property OriginalRoleSeqId() As String
            Get
                Return Me.mOriginalRoleSeqId
            End Get
            Set(value As String)
                Me.mOriginalRoleSeqId = value
            End Set
        End Property

        Public Property WorkOrder() As String
            Get
                Return mWorkOrder
            End Get
            Set(ByVal value As String)
                mWorkOrder = value
            End Set
        End Property
        Public Property DaysAfter() As Integer
            Get
                Return mDaysAfter
            End Get
            Set(ByVal value As Integer)
                mDaysAfter = value
            End Set
        End Property
        Public Property DaysBefore() As Integer
            Get
                Return mDaysBefore
            End Get
            Set(ByVal value As Integer)
                mDaysBefore = value
            End Set
        End Property

        Public Property Dependenttaskseqid() As String
            Get
                Return mDependenttaskseqid
            End Get
            Set(ByVal value As String)
                mDependenttaskseqid = value
            End Set
        End Property

        Public Property DependentChildSeqid() As String
            Get
                Return mDependentChildSeqid
            End Get
            Set(ByVal value As String)
                mDependentChildSeqid = value
            End Set
        End Property

        Public Property UpdateFlag() As String
            Get
                Return mUpdateFlag
            End Get
            Set(ByVal value As String)
                mUpdateFlag = value
            End Set
        End Property
        Public Property Comments() As String
            Get
                Return mComments
            End Get
            Set(ByVal value As String)
                mComments = value
            End Set
        End Property

        Public ReadOnly Property RecurringTasks() As System.Collections.Generic.List(Of RecurringTasks)
            Get
                Return mRecurringTasks
            End Get
        End Property
        Public Property TaskItemSeqId() As Integer
            Get
                Return mTaskItemSeqId
            End Get
            Set(ByVal value As Integer)
                mTaskItemSeqId = value
            End Set
        End Property

        Public Property TaskHeaderSeqId() As Integer
            Get
                Return mTaskHeaderSeqId
            End Get
            Set(ByVal Value As Integer)
                mTaskHeaderSeqId = Value
            End Set
        End Property


        Public Property ClosedDate() As String
            Get
                Return mClosedDate
            End Get
            Set(ByVal value As String)
                mClosedDate = value
            End Set
        End Property

        Public Property CreatedDate() As String
            Get
                Return mCreatedDate
            End Get
            Set(ByVal value As String)
                mCreatedDate = value
            End Set
        End Property
        Public Property CreatedBy() As String
            Get
                Return mCreatedBy
            End Get
            Set(ByVal value As String)
                mCreatedBy = value
            End Set
        End Property

        Public Property DateCritical() As String
            Get
                Return mDateCritical
            End Get
            Set(ByVal value As String)
                mDateCritical = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return mDescription
            End Get
            Set(ByVal value As String)
                mDescription = value
            End Set
        End Property

        Public Property DueDate() As String
            Get
                Return mDueDate
            End Get
            Set(ByVal value As String)
                mDueDate = value
            End Set
        End Property

        Public Property LastUpdateDate() As String
            Get
                Return mLastUpdateDate
            End Get
            Set(ByVal value As String)
                mLastUpdateDate = value
            End Set
        End Property

        Public Property LastUpdateUserName() As String
            Get
                Return mLastUpdateUserName
            End Get
            Set(ByVal value As String)
                mLastUpdateUserName = value
            End Set
        End Property

        Public Property LeadTime() As Integer
            Get
                Return mLeadTime
            End Get
            Set(ByVal value As Integer)
                mLeadTime = value
            End Set
        End Property

        Public Property Priority() As String
            Get
                Return mPriority
            End Get
            Set(ByVal value As String)
                mPriority = value
            End Set
        End Property

        Public Property ResponsibleName() As String
            Get
                Return mResponsibleName
            End Get
            Set(ByVal value As String)
                mResponsibleName = value
            End Set
        End Property

        Public Property ResponsibleRoleSeqId() As Integer
            Get
                Return mResponsibleRoleSeqId
            End Get
            Set(ByVal value As Integer)
                mResponsibleRoleSeqId = value
            End Set
        End Property

        Public Property ResponsibleUserName() As String
            Get
                Return mResponsibleUserName
            End Get
            Set(ByVal value As String)
                mResponsibleUserName = value
            End Set
        End Property

        Public Property RoleName() As String
            Get
                Return mRoleName
            End Get
            Set(ByVal value As String)
                mRoleName = value
            End Set
        End Property

        Public Property RoleDescription() As String
            Get
                Return mRoleDescription
            End Get
            Set(ByVal value As String)
                mRoleDescription = value
            End Set
        End Property

        Public Property RootTaskItemSeqId() As String
            Get
                Return mRootTaskItemSeqId
            End Get
            Set(ByVal value As String)
                mRootTaskItemSeqId = value
            End Set
        End Property

        Public Property StatusSeqId() As Integer
            Get
                Return mStatusSeqId
            End Get
            Set(ByVal value As Integer)
                mStatusSeqId = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return mTitle
            End Get
            Set(ByVal value As String)
                mTitle = value
            End Set
        End Property

        Public Property ResponsibleRolePlantCode() As String
            Get
                Return mResponsibleRolePlantCode
            End Get
            Set(ByVal value As String)
                mResponsibleRolePlantCode = value
            End Set
        End Property

        'RESPROLESITENAME
        Public Property ResponsibleRoleSiteName() As String
            Get
                Return mResponsibleRoleSiteName
            End Get
            Set(ByVal value As String)
                mResponsibleRoleSiteName = value
            End Set
        End Property

        Public Property CreatedByUserName As String

#End Region

        Public Sub New(ByVal closedDate As String, ByVal createdBy As String, ByVal createdDate As String, ByVal dateCritical As String, ByVal description As String, ByVal dueDate As String, ByVal lastUpdateDate As String, ByVal lastUpdateUserName As String, ByVal leadTime As Integer, ByVal priority As String, ByVal responsibleName As String, ByVal responsibleRoleSeqId As Integer, ByVal responsibleUserName As String, ByVal roleName As String, ByVal rootTaskItemSeqId As String, ByVal statusSeqId As Integer, ByVal taskHeaderSeqId As Integer, ByVal taskItemSeqId As Integer, ByVal title As String, ByVal responsibleRolePlantCode As String, ByVal roleDescription As String, ByVal responsibleRoleSiteName As String, ByVal daysBefore As Integer, ByVal dependenttaskseqid As String, ByVal dependendChildSeqID As String, ByVal DaysAfter As Integer, ByVal WorkOrder As String, ByVal SortOrder As String, ByVal EstimatedCost As String, ByVal ActualCost As String, ByVal TankInspectionId As String, ByVal OriginalRoleSeqId As String, ByVal createdByUserName As String)
            mClosedDate = closedDate
            mCreatedDate = createdDate
            mCreatedBy = createdBy
            mDateCritical = dateCritical
            mDescription = description
            mDueDate = dueDate
            mLastUpdateDate = lastUpdateDate
            mLastUpdateUserName = lastUpdateUserName
            mLeadTime = leadTime
            mPriority = priority
            mResponsibleName = responsibleName
            mResponsibleRoleSeqId = responsibleRoleSeqId
            mResponsibleUserName = responsibleUserName
            mRoleName = roleName
            mRootTaskItemSeqId = rootTaskItemSeqId
            mStatusSeqId = statusSeqId
            mTaskHeaderSeqId = taskHeaderSeqId
            mTaskItemSeqId = taskItemSeqId
            mTitle = title
            mResponsibleRolePlantCode = responsibleRolePlantCode
            mRoleDescription = roleDescription
            mResponsibleRoleSiteName = responsibleRoleSiteName
            'mComments = comments
            'mUpdateFlag = updateFlag
            mDaysBefore = daysBefore
            mDependenttaskseqid = dependenttaskseqid
            mDependentChildSeqid = dependendChildSeqID
            mDaysAfter = DaysAfter
            mWorkOrder = WorkOrder

            mSortOrder = SortOrder
            mEstimatedCost = EstimatedCost
            mActualCost = ActualCost
            mTankInspectionId = TankInspectionId
            mOriginalRoleSeqId = OriginalRoleSeqId
            mRecurringTasks = RecurringTasks
            Me.CreatedByUserName = createdByUserName
        End Sub

        Public Sub New(ByVal closedDate As String, ByVal createdBy As String, ByVal createdDate As String, ByVal dateCritical As String, ByVal description As String, ByVal dueDate As String, ByVal lastUpdateDate As String, ByVal lastUpdateUserName As String, ByVal leadTime As Integer, ByVal priority As String, ByVal responsibleName As String, ByVal responsibleRoleSeqId As Integer, ByVal responsibleUserName As String, ByVal roleName As String, ByVal rootTaskItemSeqId As String, ByVal statusSeqId As Integer, ByVal taskHeaderSeqId As Integer, ByVal taskItemSeqId As Integer, ByVal title As String, ByVal responsibleRolePlantCode As String, ByVal roleDescription As String, ByVal responsibleRoleSiteName As String, ByVal recurringTasks As System.Collections.Generic.List(Of RecurringTasks), ByVal daysBefore As Integer, ByVal dependenttaskseqid As String, ByVal dependendChildSeqID As String, ByVal DaysAfter As Integer, ByVal WorkOrder As String, ByVal SortOrder As String, ByVal EstimatedCost As String, ByVal ActualCost As String, ByVal TankInspectionId As String, ByVal OriginalRoleSeqId As String, ByVal createdByUserName As String)
            mClosedDate = closedDate
            mCreatedDate = createdDate
            mCreatedBy = createdBy
            mDateCritical = dateCritical
            mDescription = description
            mDueDate = dueDate
            mLastUpdateDate = lastUpdateDate
            mLastUpdateUserName = lastUpdateUserName
            mLeadTime = leadTime
            mPriority = priority
            mResponsibleName = responsibleName
            mResponsibleRoleSeqId = responsibleRoleSeqId
            mResponsibleUserName = responsibleUserName
            mRoleName = roleName
            mRootTaskItemSeqId = rootTaskItemSeqId
            mStatusSeqId = statusSeqId
            mTaskHeaderSeqId = taskHeaderSeqId
            mTaskItemSeqId = taskItemSeqId
            mTitle = title
            mRecurringTasks = recurringTasks
            mResponsibleRolePlantCode = responsibleRolePlantCode
            mRoleDescription = roleDescription
            mResponsibleRoleSiteName = responsibleRoleSiteName
            mDaysBefore = daysBefore
            mDependenttaskseqid = dependenttaskseqid
            mDependentChildSeqid = dependendChildSeqID
            mDaysAfter = DaysAfter
            mWorkOrder = WorkOrder

            mSortOrder = SortOrder
            mEstimatedCost = EstimatedCost
            mActualCost = ActualCost
            mTankInspectionId = TankInspectionId
            mOriginalRoleSeqId = OriginalRoleSeqId
            Me.CreatedByUserName = createdByUserName
        End Sub
        Public Sub New()

        End Sub
    End Class

    Public Class SubTaskItem


#Region "Fields"
        Private _TaskItemSeqId As String = String.Empty
        Private _TaskHeaderSeqId As String = String.Empty
        Private _Title As String = String.Empty
        'Private mPriority As String
        'Private mDateCritical As String
        'Private mDueDate As String
        'Private mClosedDate As String
        'Private mStatusSeqId As Integer
        'Private mStatusName As String
        'Private mLeadTime As Integer
        Private _RootTaskItemSeqId As String = String.Empty
        Private _ParentSubTaskSeqID As String = String.Empty
        Private _ResponsibleName As String = String.Empty
        Private _ResponsibleUserName As String = String.Empty
        Private _RoleName As String = String.Empty
        Private _ResponsibleRoleSeqId As Integer
        Private _CreatedBy As String = String.Empty
        Private _CreatedDate As String = String.Empty
        Private _LastUpdateUserName As String = String.Empty
        Private _LastUpdateDate As String = String.Empty
        Private _Description As String = String.Empty
        Private _RecurringTasks As System.Collections.Generic.List(Of RecurringTasks)
        Private _DaysAfter As Integer
        Private _ResponsibleRolePlantCode As String = String.Empty
        Private _ResponsibleRoleSiteName As String = String.Empty
        Private _RoleDescription As String = String.Empty
        Private _DueDate As String = String.Empty

        Private mSortOrder As Integer
        Private mEstimatedCost As Decimal
        Private mActualCost As Decimal
        Private mTankInspectionId As String
        Private mOriginalRoleSeqId As Integer
#End Region

#Region "Properties"
        Public Property SortOrder() As Integer
            Get
                Return Me.mSortOrder
            End Get
            Set(value As Integer)
                Me.mSortOrder = value
            End Set
        End Property

        Public Property EstimatedCost() As Decimal
            Get
                Return Me.mEstimatedCost
            End Get
            Set(value As Decimal)
                Me.mEstimatedCost = value
            End Set
        End Property

        Public Property ActualCost() As Decimal
            Get
                Return Me.mActualCost
            End Get
            Set(value As Decimal)
                Me.mActualCost = value
            End Set
        End Property

        Public Property TankInspectionId() As String
            Get
                Return Me.mTankInspectionId
            End Get
            Set(value As String)
                Me.mTankInspectionId = value
            End Set
        End Property

        Public Property OriginalRoleSeqId() As Integer
            Get
                Return Me.mOriginalRoleSeqId
            End Get
            Set(value As Integer)
                Me.mOriginalRoleSeqId = value
            End Set
        End Property

        Public Property DueDate() As String
            Get
                Return _DueDate
            End Get
            Set(ByVal value As String)
                _DueDate = value
            End Set
        End Property
        Public Property DaysAfter() As Integer
            Get
                Return _DaysAfter
            End Get
            Set(ByVal value As Integer)
                _DaysAfter = value
            End Set
        End Property
        Public Property ParentSubTaskSeqID() As String
            Get
                Return _ParentSubTaskSeqID
            End Get
            Set(ByVal value As String)
                _ParentSubTaskSeqID = value
            End Set
        End Property

        Public Property TaskItemSeqId() As String
            Get
                Return _TaskItemSeqId
            End Get
            Set(ByVal value As String)
                _TaskItemSeqId = value
            End Set
        End Property

        Public Property TaskHeaderSeqId() As String
            Get
                Return _TaskHeaderSeqId
            End Get
            Set(ByVal Value As String)
                _TaskHeaderSeqId = Value
            End Set
        End Property

        Public Property CreatedDate() As String
            Get
                Return _CreatedDate
            End Get
            Set(ByVal value As String)
                _CreatedDate = value
            End Set
        End Property

        Public Property CreatedBy() As String
            Get
                Return _CreatedBy
            End Get
            Set(ByVal value As String)
                _CreatedBy = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property

        Public Property LastUpdateDate() As String
            Get
                Return _LastUpdateDate
            End Get
            Set(ByVal value As String)
                _LastUpdateDate = value
            End Set
        End Property

        Public Property LastUpdateUserName() As String
            Get
                Return _LastUpdateUserName
            End Get
            Set(ByVal value As String)
                _LastUpdateUserName = value
            End Set
        End Property

        Public Property ResponsibleName() As String
            Get
                Return _ResponsibleName
            End Get
            Set(ByVal value As String)
                _ResponsibleName = value
            End Set
        End Property

        Public Property ResponsibleRoleSeqId() As Integer
            Get
                Return _ResponsibleRoleSeqId
            End Get
            Set(ByVal value As Integer)
                _ResponsibleRoleSeqId = value
            End Set
        End Property

        Public Property ResponsibleUserName() As String
            Get
                Return _ResponsibleUserName
            End Get
            Set(ByVal value As String)
                _ResponsibleUserName = value
            End Set
        End Property

        Public Property RoleName() As String
            Get
                Return _RoleName
            End Get
            Set(ByVal value As String)
                _RoleName = value
            End Set
        End Property

        Public Property RootTaskItemSeqId() As String
            Get
                Return _RootTaskItemSeqId
            End Get
            Set(ByVal value As String)
                _RootTaskItemSeqId = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal value As String)
                _Title = value
            End Set
        End Property

        Public Property ResponsibleRolePlantCode() As String
            Get
                Return _ResponsibleRolePlantCode
            End Get
            Set(ByVal value As String)
                _ResponsibleRolePlantCode = value
            End Set
        End Property

        Public Property ResponsibleRoleSiteName() As String
            Get
                Return _ResponsibleRoleSiteName
            End Get
            Set(ByVal value As String)
                _ResponsibleRoleSiteName = value
            End Set
        End Property

        Public Property RoleDescription() As String
            Get
                Return _RoleDescription
            End Get
            Set(ByVal value As String)
                _RoleDescription = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "taskitemseqid"
                        Return Me.TaskItemSeqId
                        'Case "activityseqid"
                        '    Return CStr(ActivitySeqid)
                    Case Else
                        Return Me.TaskItemSeqId
                End Select
            End Get
        End Property
#End Region

        Public Sub New(ByVal createdBy As String, ByVal createdDate As String, ByVal description As String, ByVal lastUpdateDate As String, ByVal lastUpdateUserName As String, ByVal responsibleName As String, ByVal responsibleRoleSeqId As Integer, ByVal responsibleUserName As String, ByVal roleName As String, ByVal rootTaskItemSeqId As String, ByVal taskHeaderSeqId As String, ByVal taskItemSeqId As String, ByVal title As String, ByVal daysAfter As Integer, ByVal parentSubTaskSeqId As String, ByVal responsibleRolePlantCode As String, ByVal roleDescription As String, ByVal responsiblerolesitename As String, ByVal dueDate As String)
            _CreatedDate = createdDate
            _CreatedBy = createdBy
            _Description = description
            _LastUpdateDate = lastUpdateDate
            _LastUpdateUserName = lastUpdateUserName
            _ResponsibleName = responsibleName
            _ResponsibleRoleSeqId = responsibleRoleSeqId
            _ResponsibleUserName = responsibleUserName
            _RoleName = roleName
            _RootTaskItemSeqId = rootTaskItemSeqId
            _TaskHeaderSeqId = taskHeaderSeqId
            _TaskItemSeqId = taskItemSeqId
            _Title = title
            _DaysAfter = daysAfter
            _ParentSubTaskSeqID = parentSubTaskSeqId
            _ResponsibleRolePlantCode = responsibleRolePlantCode
            _RoleDescription = roleDescription
            _ResponsibleRoleSiteName = responsiblerolesitename
            _DueDate = dueDate
        End Sub

        Public Sub New()

        End Sub
    End Class

    <Serializable()>
    Public Class TaskListing
#Region "Fields"
        Private mActivityName As String
        Private mActivitySeqID As Integer
        Private mAttachmentCount As Integer
        Private mBusinessUnitArea As String
        Private mDivision As String
        Private mDueDate As String
        Private mCloseDate As String
        Private mExternalRef As String
        Private mExternalSourceName As String
        Private mExternalSourceID As String
        Private mHeaderCreatedBy As String
        Private mHeaderCreateDate As String
        Private mHeaderDesc As String
        Private mHeaderLastUpdateDate As String
        Private mHeaderLastUpdateUserName As String
        Private mHeaderTitle As String
        Private mItemCreatedDate As String
        Private mItemDesc As String
        Private mItemLastUpdateDate As String
        Private mItemTitle As String
        Private mLine As String
        Private mPlantCode As String
        Private mSecurityLevel As String
        Private mSiteName As String
        Private mStartDate As String
        Private mStatusName As String
        Private mStatusSeqId As String
        Private mTaskHeaderSeqId As Integer
        Private mTaskItemSeqId As Integer
        Private mTaskType As String
        Private mRegion As String
        Private mEndDate As String
        Private mHeaderStatus As String
        Private mResponsibleName As String
        Private mRoleDescription As String
        Private mRoleSiteName As String
        Private mDateCritical As String
        Private mOpenSubTasks As String
        Private mTankInspectionTypeId As String


#End Region
        Private Function GetPriortyName(ByVal id As Integer) As String
            Select Case id
                Case 1
                    Return IP.Bids.SharedFunctions.LocalizeValue("Low")
                Case 2
                    Return IP.Bids.SharedFunctions.LocalizeValue("Medium")
                Case 3
                    Return IP.Bids.SharedFunctions.LocalizeValue("High")
                Case Else
                    Return IP.Bids.SharedFunctions.LocalizeValue("Low")
            End Select
        End Function

        Public Property BusinessUnitArea() As String
        Public Property Line() As String
        Public Property SiteName() As String
        Public Property ActivityName() As String
        Public Property ActivitySeqID() As Integer
        Public Property StartDate() As String
        Public Property EndDate() As String
        Public Property HeaderCreateDate() As String
        Public Property HeaderLastUpdateDate() As String
        Public Property ItemCreatedDate() As String
        Public Property ItemLastUpdateDate() As String
        Public Property AttachmentCount() As Integer
        Public Property ExternalRef() As String
        Public Property ExternalSourceName() As String
        Public Property ExternalSourceID() As String
        Public Property TaskHeaderSeqId() As Integer
        Public Property HeaderTitle() As String
        Public Property SecurityLevel() As String
        Public Property PlantCode() As String
        Public Property HeaderCreatedBy() As String
        Public Property TaskType() As String
        Public Property HeaderLastUpdateUserName() As String
        Public Property Region() As String
        Public Property Division() As String
        Public Property HeaderDesc() As String
        Public Property ItemDesc() As String
        Public Property ItemTitle() As String
        Public Property TaskItemSeqId() As Integer
        Public Property DueDate() As String
        Public Property ClosedDate() As String
        Public Property StatusName() As String
        Public Property StatusSeqId() As String
        Public Property HeaderStatus() As String
        Public Property ResponsibleName() As String
        Public Property RoleDescription() As String
        Public Property RoleSiteName() As String
        Public Property OpenSubTasks() As String
        Public Property DateCritical() As String
        Public Property Comments As String
        Public ReadOnly Property PriorityName As String
            Get
                Return GetPriortyName(Priority)
            End Get
        End Property
        Public Property Priority As Integer
        Public Property TankInspectionTypeId As String

        Public Property ParentDueDate As String
        Public Property DependentTaskSeqId As Integer
        Public Property ParentSubTaskSeqId As Integer
        Public Property TaskItemLevel As Integer = 0
        Public Property ParentTaskSeqId As Integer

        Public Sub New(ByVal activityName As String, ByVal activitySeqID As Integer, ByVal attachmentCount As Integer, ByVal businessUnitArea As String, ByVal division As String, ByVal dueDate As String, ByVal endDate As String, ByVal externalRef As String, ByVal externalSourceID As String, ByVal externalSourcename As String, ByVal headerCreateDate As String, ByVal headerCreatedBy As String, ByVal headerDesc As String, ByVal headerLastUpdateDate As String, ByVal headerLastUpdateUserName As String, ByVal headerTitle As String, ByVal itemCreatedDate As String, ByVal itemDesc As String, ByVal itemLastUpdateDate As String, ByVal itemTitle As String, ByVal line As String, ByVal plantCode As String, ByVal region As String, ByVal securityLevel As String, ByVal siteName As String, ByVal startDate As String, ByVal statusName As String, ByVal statusSeqId As String, ByVal taskHeaderSeqId As Integer, ByVal taskItemSeqId As Integer, ByVal taskType As String, ByVal headerStatus As String, ByVal roleDescription As String, ByVal responsibleName As String, ByVal roleSiteName As String, ByVal closedDate As String, ByVal datecritical As String, ByVal opensubtasks As String, tankInspectionTypeId As String, priority As Integer, comments As String, ByVal parentDueDate As String, ByVal parentSubTaskSeqId As Integer, ByVal dependentTaskSeqId As Integer)
            With Me
                .ActivityName = activityName
                .ActivitySeqID = activitySeqID
                .AttachmentCount = attachmentCount
                .BusinessUnitArea = businessUnitArea
                .Division = division
                .DueDate = dueDate
                .EndDate = endDate
                .ExternalRef = externalRef
                .ExternalSourceID = externalSourceID
                .ExternalSourceName = externalSourcename
                .HeaderCreateDate = headerCreateDate
                .HeaderCreatedBy = headerCreatedBy
                .HeaderDesc = headerDesc
                .HeaderLastUpdateDate = headerLastUpdateDate
                .HeaderLastUpdateUserName = headerLastUpdateUserName
                .HeaderTitle = headerTitle
                .ItemCreatedDate = itemCreatedDate
                .ItemDesc = itemDesc
                .ItemLastUpdateDate = itemLastUpdateDate
                .ItemTitle = itemTitle
                .Line = line
                .PlantCode = plantCode
                .Region = region
                .SecurityLevel = securityLevel
                .SiteName = siteName
                .StartDate = startDate
                .StatusName = statusName
                .StatusSeqId = statusSeqId
                .TaskHeaderSeqId = taskHeaderSeqId
                .TaskItemSeqId = taskItemSeqId
                .TaskType = taskType
                .HeaderStatus = headerStatus
                .RoleDescription = roleDescription
                .ResponsibleName = responsibleName
                .RoleSiteName = roleSiteName
                .ClosedDate = closedDate
                .DateCritical = datecritical
                .OpenSubTasks = opensubtasks
                .TankInspectionTypeId = tankInspectionTypeId
                .Priority = priority
                .Comments = comments
                .ParentDueDate = parentDueDate
                .ParentSubTaskSeqId = parentSubTaskSeqId
                .DependentTaskSeqId = dependentTaskSeqId

                If .TaskItemSeqId = .DependentTaskSeqId OrElse .TaskItemSeqId = parentSubTaskSeqId Then
                    .TaskItemLevel = 1 'Parent Task
                    .ParentTaskSeqId = .TaskItemSeqId
                ElseIf .ParentSubTaskSeqId > 0 Then
                    .TaskItemLevel = 2 'Subsequent
                    .ParentTaskSeqId = .ParentSubTaskSeqId
                ElseIf .ParentSubTaskSeqId = 0 And dependentTaskSeqId = 0 Then
                    .TaskItemLevel = 0 'Not a parent
                    .ParentTaskSeqId = .TaskItemSeqId
                Else
                    .TaskItemLevel = 3 'Sub Task
                    .ParentTaskSeqId = .DependentTaskSeqId
                End If
            End With


        End Sub

    End Class

    Public Class ReportTitles
#Region "Fields"
        Private _ReportTitle As String
#End Region
#Region "Properties"
        Public Property ReportTitle() As String
            Get
                Return _ReportTitle
            End Get
            Set(ByVal value As String)
                _ReportTitle = value
            End Set
        End Property
#End Region

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "reporttitle"
                        Return _ReportTitle
                    Case Else
                        Return _ReportTitle
                End Select
            End Get
        End Property

        Public Sub New(ByVal reportTitle As String)
            _ReportTitle = reportTitle
        End Sub
    End Class

    Public Class ReportSortValues
#Region "Fields"
        Private _reportTitle As String
        Private _reportSortValue As String
        Private _reportName As String
        Private _sortValueOrder As Integer
        Private _inactiveFlag As String
#End Region

#Region "Properties"
        Public Property ReportTitle() As String
            Get
                Return _reportTitle
            End Get
            Set(ByVal Value As String)
                _reportTitle = Value
            End Set
        End Property
        Public Property ReportSortValue() As String
            Get
                Return _reportSortValue
            End Get
            Set(ByVal value As String)
                _reportSortValue = value
            End Set
        End Property
        Public Property ReportName() As String
            Get
                Return _reportName
            End Get
            Set(ByVal value As String)
                _reportName = value
            End Set
        End Property
        Public Property SortValueOrder() As Integer
            Get
                Return _sortValueOrder
            End Get
            Set(ByVal value As Integer)
                _sortValueOrder = value
            End Set
        End Property
        Public Property InactiveFlag() As String
            Get
                Return _inactiveFlag
            End Get
            Set(ByVal value As String)
                _inactiveFlag = value
            End Set
        End Property
#End Region

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "reporttitle"
                        Return _reportTitle
                    Case "reportsortvalue"
                        Return _reportSortValue
                    Case "reportname"
                        Return _reportName
                    Case "sortvalueorder"
                        Return CStr(SortValueOrder)
                    Case "inactiveflag"
                        Return InactiveFlag
                    Case Else
                        Return _reportTitle
                End Select
            End Get
        End Property


        Public Sub New(ByVal reportTitle As String, ByVal reportSortValue As String, ByVal reportName As String, ByVal sortValueOrder As Integer, ByVal inActiveFlag As String)
            _reportTitle = reportTitle
            _reportSortValue = reportSortValue
            _reportName = reportName
            _sortValueOrder = sortValueOrder
            _inactiveFlag = inActiveFlag
        End Sub
    End Class

    Public Class ReportParameters
#Region "Fields"
        Private _ReportTitle As String
        Private _ParameterSort As String
        Private _ReportParameters As String
        Private _ReportParameterType As String

#End Region

#Region "Properties"

        Public Property ReportTitle() As String
            Get
                Return _ReportTitle
            End Get
            Set(ByVal value As String)
                _ReportTitle = value
            End Set
        End Property
        Public Property ParameterSort() As String
            Get
                Return _ParameterSort
            End Get
            Set(ByVal value As String)
                _ParameterSort = value
            End Set
        End Property
        Public Property ReportParameters() As String
            Get
                Return _ReportParameters
            End Get
            Set(ByVal value As String)
                _ReportParameters = value
            End Set
        End Property
        Public Property ReportParameterType() As String
            Get
                Return _ReportParameterType
            End Get
            Set(ByVal value As String)
                _ReportParameterType = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "reporttitle"
                        Return _ReportTitle
                    Case "parametersort"
                        Return _ParameterSort
                    Case "reportparameters"
                        Return _ReportParameters
                    Case "reportparametertype"
                        Return _ReportParameterType
                    Case Else
                        Return _ReportTitle
                End Select
            End Get
        End Property
#End Region



        Public Sub New(ByVal reportTitle As String, ByVal parameterSort As String, ByVal reportParameters As String, ByVal reportParameterType As String)
            _ReportTitle = reportTitle
            _ParameterSort = parameterSort
            _ReportParameters = reportParameters
            _ReportParameterType = reportParameterType
        End Sub
    End Class

    Public Class RecurringParametersList
#Region "Fields"
        Private _ProfileType As String
        Private _ProfileTypeSeqId As Integer
        Private _ProfileTypeName As String
#End Region

#Region "Properties"

        Public Property ProfileTypeSeqId() As Integer
            Get
                Return _ProfileTypeSeqId
            End Get
            Set(ByVal value As Integer)
                _ProfileTypeSeqId = value
            End Set
        End Property

        Public Property ProfileTypeName() As String
            Get
                Return _ProfileTypeName
            End Get
            Set(ByVal value As String)
                _ProfileTypeName = value
            End Set
        End Property

        Public Property ProfileType() As String
            Get
                Return _ProfileType
            End Get
            Set(ByVal value As String)
                _ProfileType = value
            End Set
        End Property
#End Region

#Region "Constructor(s)"

#End Region

        Public Sub New(ByVal profileTypeSeqId As Integer, ByVal profileTypeName As String, ByVal profileType As String)
            Me._ProfileTypeName = profileTypeName
            Me._ProfileTypeSeqId = profileTypeSeqId
            Me._ProfileType = profileType
        End Sub
    End Class
    Public Class RecurringParameters
#Region "Fields"
        Private _TaskItemSeqId As Integer
        Private _ProfileTypeSeqId As Integer
        Private _ProfileTypeValue As String
        Private _LastUpdateUsername As String
        Private _LastUpdateDate As DateTime
        Private _ProfileTypeName As String
#End Region

#Region "Properties"

        Public Property TaskItemSeqId() As Integer
            Get
                Return _TaskItemSeqId
            End Get
            Set(ByVal value As Integer)
                _TaskItemSeqId = value
            End Set
        End Property
        Public Property ProfileTypeSeqId() As Integer
            Get
                Return _ProfileTypeSeqId
            End Get
            Set(ByVal value As Integer)
                _ProfileTypeSeqId = value
            End Set
        End Property
        Public Property ProfileTypeValue() As String
            Get
                Return _ProfileTypeValue
            End Get
            Set(ByVal value As String)
                _ProfileTypeValue = value
            End Set
        End Property
        Public Property LastUpdateUsername() As String
            Get
                Return _LastUpdateUsername
            End Get
            Set(ByVal value As String)
                _LastUpdateUsername = value
            End Set
        End Property
        Public Property LastUpdateDate() As DateTime
            Get
                Return _LastUpdateDate
            End Get
            Set(ByVal value As DateTime)
                _LastUpdateDate = value
            End Set
        End Property
        Public Property ProfileTypeName() As String
            Get
                Return _ProfileTypeName
            End Get
            Set(ByVal value As String)
                _ProfileTypeName = value
            End Set
        End Property
#End Region

#Region "Constructor(s)"

#End Region

        Public Sub New(ByVal taskItemSeqId As Integer, ByVal profileTypeSeqId As Integer, ByVal profileTypeName As String, ByVal profileTypeValue As String, ByVal lastUpdateUserName As String, ByVal lastUpdateDate As Date)
            Me._LastUpdateDate = lastUpdateDate
            Me._LastUpdateUsername = lastUpdateUserName
            Me._ProfileTypeName = profileTypeName
            Me._ProfileTypeSeqId = profileTypeSeqId
            Me._ProfileTypeValue = profileTypeValue
            Me._TaskItemSeqId = taskItemSeqId
        End Sub
    End Class
    Public Class TaskStatus
#Region "Fields"
        Private mStatusName As String = String.Empty
        Private mStatusSeqid As Integer
        Private mImageIcon As String
#End Region

#Region "Properties"
        Public Property ImageIcon() As String
            Get
                Return mImageIcon
            End Get
            Set(ByVal value As String)
                mImageIcon = value
            End Set
        End Property
        Public Property StatusName() As String
            Get
                Return mStatusName
            End Get
            Set(ByVal value As String)
                mStatusName = value
            End Set
        End Property

        Public Property StatusSeqid() As Integer
            Get
                Return mStatusSeqid
            End Get
            Set(ByVal value As Integer)
                mStatusSeqid = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "statusname"
                        Return StatusName
                    Case "statusseqid"
                        Return CStr(StatusSeqid)
                    Case Else
                        Return CStr(StatusSeqid)
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()
            'Default Constructor
        End Sub
        Public Sub New(ByVal statusName As String, ByVal statusSeqId As Integer, ByVal imageIcon As String)
            Me.mStatusName = statusName
            Me.mStatusSeqid = statusSeqId
            mImageIcon = imageIcon
        End Sub
#End Region
    End Class

    Public Class UserDefaults
#Region "Fields"
        Private _Username As String
        Private _ProfileTypeSeqId As String
        Private _Application As String
        Private _ProfileTypeValue As String
        Private _LastUpdateUserName As String
        Private _LastUpdateDate As String
        Private _ProfileTypeName As String
#End Region

#Region "Properties"

#End Region

    End Class

    Public Class TaskItemComments

#Region "Properties"

        Public Property CommentsSeqId() As String

        Public Property Username() As String

        Public Property LastUpdateUsername() As String

        Public Property LastUpdateDate() As Date

        Public Property Comments() As String
        Public Property CreatedBy As String
#End Region

#Region "Constructor(s)"
        Public Sub New(ByVal commentsSeqId As String, ByVal username As String, ByVal lastUpdateUsername As String, ByVal lastUpdateDate As Date, ByVal comments As String, createdBy As String)
            Me.CommentsSeqId = commentsSeqId
            Me.Username = username
            Me.LastUpdateUsername = lastUpdateUsername
            Me.LastUpdateDate = lastUpdateDate
            Me.Comments = comments
            Me.CreatedBy = createdBy
        End Sub
        Public Sub New()

        End Sub
#End Region


    End Class

    Public Class NotificationProfile
#Region "Fields"
        Private _RoleName As String = String.Empty
        Private _RoleSeqId As Integer
        Private _EmailType As String = String.Empty
        Private _ProfileTypeSeqId As Integer
        Private _ProfileTypeValue As String = String.Empty
        Private _ProfileTypeName As String = String.Empty
#End Region
#Region "Properties"
        Public Property RoleName() As String
            Get
                Return _RoleName
            End Get
            Set(ByVal value As String)
                _RoleName = value
            End Set
        End Property

        Public Property RoleSeqId() As Integer
            Get
                Return _RoleSeqId
            End Get
            Set(ByVal value As Integer)
                _RoleSeqId = value
            End Set
        End Property

        Public Property EmailType() As String
            Get
                Return _EmailType
            End Get
            Set(ByVal value As String)
                _EmailType = value
            End Set
        End Property

        Public Property ProfileTypeSeqId() As Integer
            Get
                Return _ProfileTypeSeqId
            End Get
            Set(ByVal value As Integer)
                _ProfileTypeSeqId = value
            End Set
        End Property

        Public Property ProfileTypeValue() As String
            Get
                Return _ProfileTypeValue
            End Get
            Set(ByVal value As String)
                _ProfileTypeValue = value
            End Set
        End Property

        Public Property ProfileTypeName() As String
            Get
                Return _ProfileTypeName
            End Get
            Set(ByVal value As String)
                _ProfileTypeName = value
            End Set
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New(ByVal roleName As String, ByVal roleSeqId As Integer, ByVal emailType As String, ByVal profileTypeSeqId As Integer, ByVal profileTypeValue As String, ByVal profileTypeName As String)
            _RoleName = roleName
            _RoleSeqId = roleSeqId
            _EmailType = emailType
            _ProfileTypeSeqId = profileTypeSeqId
            _ProfileTypeValue = profileTypeValue
            _ProfileTypeName = profileTypeName
        End Sub
#End Region


    End Class

    Public Class UserRoles
#Region "Fields"
        Private _PlantCode As String
        Private _RoleDescription As String
        Private _Name As String
        Private _UserName As String
        Private _RoleSeqID As Integer
        Private _RoleName As String
#End Region
#Region "Properties"
        Public Property PlantCode() As String
            Get
                Return _PlantCode
            End Get
            Set(ByVal Value As String)
                _PlantCode = Value
            End Set
        End Property
        Public Property RoleDescription() As String
            Get
                Return _RoleDescription
            End Get
            Set(ByVal Value As String)
                _RoleDescription = Value
            End Set
        End Property
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal Value As String)
                _Name = Value
            End Set
        End Property
        Public Property UserName() As String
            Get
                Return _UserName
            End Get
            Set(ByVal Value As String)
                _UserName = Value
            End Set
        End Property
        Public Property RoleSeqID() As Integer
            Get
                Return _RoleSeqID
            End Get
            Set(ByVal Value As Integer)
                _RoleSeqID = Value
            End Set
        End Property
        Public Property RoleName() As String
            Get
                Return _RoleName
            End Get
            Set(ByVal Value As String)
                _RoleName = Value
            End Set
        End Property
#End Region

#Region "Constructor(s)"

#End Region

        Public Sub New(ByVal plantCode As String, ByVal roleDescription As String, ByVal name As String, ByVal userName As String, ByVal roleSeqID As Integer, ByVal roleName As String)
            _PlantCode = plantCode
            _RoleDescription = roleDescription
            _Name = name
            _UserName = userName
            _RoleSeqID = roleSeqID
            _RoleName = roleName
        End Sub
    End Class

    Public Class BusinessRegionSite
#Region "Fields"
        Dim _Region As String = String.Empty
        Dim _Business As String = String.Empty
        Dim _Site As String = String.Empty
        Dim _PlantCode As String = String.Empty
        Dim _ProcessedFlag As String = String.Empty
#End Region

#Region "Properties"
        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "site"
                        Return _Site
                    Case "plantcode"
                        Return _PlantCode
                    Case "businessname"
                        Return _Business
                    Case "regionname"
                        Return Me._Region
                    Case "processedflag"
                        Return _ProcessedFlag
                    Case Else
                        Return _PlantCode
                End Select
            End Get
        End Property
        Public Property PlantCode() As String
            Get
                Return _PlantCode
            End Get
            Set(ByVal value As String)
                _PlantCode = value
            End Set
        End Property
        Public Property BusinessName() As String
            Get
                Return _Business
            End Get
            Set(ByVal Value As String)
                _Business = Value
            End Set
        End Property

        Public Property RegionName() As String
            Get
                Return _Region
            End Get
            Set(ByVal Value As String)
                _Region = Value
            End Set
        End Property

        Public Property SiteName() As String
            Get
                Return _Site
            End Get
            Set(ByVal Value As String)
                _Site = Value
            End Set
        End Property

        Public Property ProcessedFlag() As String
            Get
                Return _ProcessedFlag
            End Get
            Set(ByVal value As String)
                _ProcessedFlag = value
            End Set
        End Property
#End Region

#Region "Constructors"
        Public Sub New(ByVal business As String, ByVal region As String, ByVal site As String, ByVal plantCode As String, ByVal processedFlag As String)
            _Business = business
            _Region = region
            _Site = site
            _PlantCode = plantCode
            _ProcessedFlag = processedFlag
        End Sub
#End Region



    End Class

    Public Class HelpFiles
#Region "Fields"
        Dim _DemoName As String
        Dim _DemoDescription As String
        Dim _DemoFileName As String
#End Region

#Region "Properties"
        Public Property DemoName() As String
            Get
                Return _DemoName
            End Get
            Set(ByVal value As String)
                _DemoName = value
            End Set
        End Property
        Public Property DemoDescription() As String
            Get
                Return _DemoDescription
            End Get
            Set(ByVal value As String)
                _DemoDescription = value
            End Set
        End Property
        Public Property DemoFileName() As String
            Get
                Return _DemoFileName
            End Get
            Set(ByVal value As String)
                _DemoFileName = value
            End Set
        End Property
#End Region

#Region "Constructors"
        Public Sub New(ByVal demoName As String, ByVal demoDescription As String, ByVal demoFileName As String)
            _DemoName = demoName
            _DemoDescription = demoDescription
            _DemoFileName = demoFileName
        End Sub
#End Region
    End Class


    Public Class GMSElementList
#Region "Fields"
        Private _ElementTitle As String
        Private _sortOrder As Integer
#End Region

#Region "Properties"
        Public Property ElementTitle() As String
            Get
                Return _ElementTitle
            End Get
            Set(ByVal value As String)
                _ElementTitle = value
            End Set
        End Property

        Public Property SortOrder() As Integer
            Get
                Return _sortOrder
            End Get
            Set(ByVal value As Integer)
                _sortOrder = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal index As String) As String
            Get
                Select Case index.ToLower
                    Case "title"
                        Return ElementTitle
                    Case "sortorder"
                        Return CStr(SortOrder)
                    Case Else
                        Return CStr(ElementTitle)
                End Select
            End Get
        End Property
#End Region

#Region "Constructor(s)"
        Public Sub New()
            'Default Constructor
        End Sub
        Public Sub New(ByVal elementTitle As String)
            _ElementTitle = elementTitle
        End Sub
#End Region
    End Class
End Namespace