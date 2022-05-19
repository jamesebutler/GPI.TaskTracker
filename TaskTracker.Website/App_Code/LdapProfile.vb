'***********************************************************************
' Assembly         : http://TaskTracker.ipaper.com
' Author           :Michael J Pope Jr
' Created          : 09-09-2010
'
' Last Modified By :Michael J Pope Jr
' Last Modified On : 09-09-2010
' Description      : 
'
' Copyright        : (c) International Paper. All rights reserved.
'***********************************************************************
Imports Microsoft.VisualBasic
Imports System.Data
Imports system.Security.Principal
Imports System.DirectoryServices
Imports System.Web.TraceContext
Imports System
Imports System.Text
Imports System.Collections
Imports System.Globalization

Namespace IP.Bids
    <Serializable()> _
           Public Class LdapProfile
        Private mdistinguishedname As System.String = String.Empty
        Public Property Distinguishedname() As System.String
            Get
                Return mdistinguishedname
            End Get
            Set(ByVal value As System.String)
                mdistinguishedname = value
            End Set
        End Property
        Private mcountrycode As System.String
        Public Property Countrycode() As System.String
            Get
                Return mcountrycode
            End Get
            Set(ByVal value As System.String)
                mcountrycode = value
            End Set
        End Property
        Private mcn As System.String
        Public Property CN() As System.String
            Get
                Return mcn
            End Get
            Set(ByVal value As System.String)
                mcn = value
            End Set
        End Property
        Private mlastlogoff As System.String
        Public Property Lastlogoff() As System.String
            Get
                Return mlastlogoff
            End Get
            Set(ByVal value As System.String)
                mlastlogoff = value
            End Set
        End Property
        Private museraccountcontrol As System.String
        Public Property Useraccountcontrol() As System.String
            Get
                Return museraccountcontrol
            End Get
            Set(ByVal value As System.String)
                museraccountcontrol = value
            End Set
        End Property
        Private musncreated As System.String
        Public Property Usncreated() As System.String
            Get
                Return musncreated
            End Get
            Set(ByVal value As System.String)
                musncreated = value
            End Set
        End Property
        Private mobjectguid As System.String
        Public Property Objectguid() As System.String
            Get
                Return mobjectguid
            End Get
            Set(ByVal value As System.String)
                mobjectguid = value
            End Set
        End Property
        Private mpostalcode As System.String
        Public Property Postalcode() As System.String
            Get
                Return mpostalcode
            End Get
            Set(ByVal value As System.String)
                mpostalcode = value
            End Set
        End Property
        Private mwhenchanged As System.String
        Public Property Whenchanged() As System.String
            Get
                Return mwhenchanged
            End Get
            Set(ByVal value As System.String)
                mwhenchanged = value
            End Set
        End Property
        Private mmemberof As System.String
        Public Property Memberof() As System.String
            Get
                Return mmemberof
            End Get
            Set(ByVal value As System.String)
                mmemberof = value
            End Set
        End Property
        Private maccountexpires As System.String
        Public Property Accountexpires() As System.String
            Get
                Return maccountexpires
            End Get
            Set(ByVal value As System.String)
                maccountexpires = value
            End Set
        End Property
        Private mdisplayname As System.String
        Public Property Displayname() As System.String
            Get
                Return mdisplayname
            End Get
            Set(ByVal value As System.String)
                mdisplayname = value
            End Set
        End Property
        Private memployeenumber As System.String
        Public Property Employeenumber() As System.String
            Get
                Return memployeenumber
            End Get
            Set(ByVal value As System.String)
                memployeenumber = value
            End Set
        End Property
        Private mprimarygroupid As System.String
        Public Property Primarygroupid() As System.String
            Get
                Return mprimarygroupid
            End Get
            Set(ByVal value As System.String)
                mprimarygroupid = value
            End Set
        End Property
        Private mstreetaddress As System.String
        Public Property Streetaddress() As System.String
            Get
                Return mstreetaddress
            End Get
            Set(ByVal value As System.String)
                mstreetaddress = value
            End Set
        End Property
        Private mbadpwdcount As System.String
        Public Property Badpwdcount() As System.String
            Get
                Return mbadpwdcount
            End Get
            Set(ByVal value As System.String)
                mbadpwdcount = value
            End Set
        End Property
        Private mobjectclass As System.String
        Public Property Objectclass() As System.String
            Get
                Return mobjectclass
            End Get
            Set(ByVal value As System.String)
                mobjectclass = value
            End Set
        End Property
        Private mobjectcategory As System.String
        Public Property Objectcategory() As System.String
            Get
                Return mobjectcategory
            End Get
            Set(ByVal value As System.String)
                mobjectcategory = value
            End Set
        End Property
        Private minstancetype As System.String
        Public Property Instancetype() As System.String
            Get
                Return minstancetype
            End Get
            Set(ByVal value As System.String)
                minstancetype = value
            End Set
        End Property
        Private mhomedrive As System.String
        Public Property Homedrive() As System.String
            Get
                Return mhomedrive
            End Get
            Set(ByVal value As System.String)
                mhomedrive = value
            End Set
        End Property
        Private msamaccounttype As System.String
        Public Property Samaccounttype() As System.String
            Get
                Return msamaccounttype
            End Get
            Set(ByVal value As System.String)
                msamaccounttype = value
            End Set
        End Property
        Private mhomedirectory As System.String
        Public Property Homedirectory() As System.String
            Get
                Return mhomedirectory
            End Get
            Set(ByVal value As System.String)
                mhomedirectory = value
            End Set
        End Property
        Private mwhencreated As System.String
        Public Property Whencreated() As System.String
            Get
                Return mwhencreated
            End Get
            Set(ByVal value As System.String)
                mwhencreated = value
            End Set
        End Property
        Private mlastlogon As System.String
        Public Property Lastlogon() As System.String
            Get
                Return mlastlogon
            End Get
            Set(ByVal value As System.String)
                mlastlogon = value
            End Set
        End Property
        Private ml As System.String
        Public Property L() As System.String
            Get
                Return ml
            End Get
            Set(ByVal value As System.String)
                ml = value
            End Set
        End Property
        Private mst As System.String
        Public Property ST() As System.String
            Get
                Return mst
            End Get
            Set(ByVal value As System.String)
                mst = value
            End Set
        End Property
        Private mco As System.String
        Public Property CO() As System.String
            Get
                Return mco
            End Get
            Set(ByVal value As System.String)
                mco = value
            End Set
        End Property
        Private mtitle As System.String
        Public Property Title() As System.String
            Get
                Return mtitle
            End Get
            Set(ByVal value As System.String)
                mtitle = value
            End Set
        End Property
        Private mc As System.String
        Public Property C() As System.String
            Get
                Return mc
            End Get
            Set(ByVal value As System.String)
                mc = value
            End Set
        End Property
        Private msamaccountname As System.String
        Public Property Samaccountname() As System.String
            Get
                Return msamaccountname
            End Get
            Set(ByVal value As System.String)
                msamaccountname = value
            End Set
        End Property
        Private memployeetype As System.String
        Public Property Employeetype() As System.String
            Get
                Return memployeetype
            End Get
            Set(ByVal value As System.String)
                memployeetype = value
            End Set
        End Property
        Private mgivenname As System.String
        Public Property Givenname() As System.String
            Get
                Return mgivenname
            End Get
            Set(ByVal value As System.String)
                mgivenname = value
            End Set
        End Property
        Public Property Manager As String
        Private mmail As System.String
        Public Property Mail() As System.String
            Get
                Return mmail
            End Get
            Set(ByVal value As System.String)
                mmail = value
            End Set
        End Property
        Private madspath As System.String
        Public Property Adspath() As System.String
            Get
                Return madspath
            End Get
            Set(ByVal value As System.String)
                madspath = value
            End Set
        End Property
        Private mlockouttime As System.String
        Public Property Lockouttime() As System.String
            Get
                Return mlockouttime
            End Get
            Set(ByVal value As System.String)
                mlockouttime = value
            End Set
        End Property
        Private mscriptpath As System.String
        Public Property Scriptpath() As System.String
            Get
                Return mscriptpath
            End Get
            Set(ByVal value As System.String)
                mscriptpath = value
            End Set
        End Property
        Private mpwdlastset As System.String
        Public Property Pwdlastset() As System.String
            Get
                Return mpwdlastset
            End Get
            Set(ByVal value As System.String)
                mpwdlastset = value
            End Set
        End Property
        Private mlogoncount As System.String
        Public Property Logoncount() As System.String
            Get
                Return mlogoncount
            End Get
            Set(ByVal value As System.String)
                mlogoncount = value
            End Set
        End Property
        Private mcodepage As System.String
        Public Property Codepage() As System.String
            Get
                Return mcodepage
            End Get
            Set(ByVal value As System.String)
                mcodepage = value
            End Set
        End Property
        Private mname As System.String
        Public Property Name() As System.String
            Get
                Return mname
            End Get
            Set(ByVal value As System.String)
                mname = value
            End Set
        End Property
        Private mphysicaldeliveryofficename As System.String
        Public Property Physicaldeliveryofficename() As System.String
            Get
                Return mphysicaldeliveryofficename
            End Get
            Set(ByVal value As System.String)
                mphysicaldeliveryofficename = value
            End Set
        End Property
        Private musnchanged As System.String
        Public Property Usnchanged() As System.String
            Get
                Return musnchanged
            End Get
            Set(ByVal value As System.String)
                musnchanged = value
            End Set
        End Property
        Private mdscorepropagationdata As System.String
        Public Property Dscorepropagationdata() As System.String
            Get
                Return mdscorepropagationdata
            End Get
            Set(ByVal value As System.String)
                mdscorepropagationdata = value
            End Set
        End Property
        Private muserprincipalname As System.String
        Public Property Userprincipalname() As System.String
            Get
                Return muserprincipalname
            End Get
            Set(ByVal value As System.String)
                muserprincipalname = value
            End Set
        End Property
        Private mbadpasswordtime As System.String
        Public Property Badpasswordtime() As System.String
            Get
                Return mbadpasswordtime
            End Get
            Set(ByVal value As System.String)
                mbadpasswordtime = value
            End Set
        End Property
        Private mobjectsid As System.String
        Public Property Objectsid() As System.String
            Get
                Return mobjectsid
            End Get
            Set(ByVal value As System.String)
                mobjectsid = value
            End Set
        End Property
        Private msn As System.String
        Public Property SN() As System.String
            Get
                Return msn
            End Get
            Set(ByVal value As System.String)
                msn = value
            End Set
        End Property
        Private mtelephonenumber As System.String
        Public Property Telephonenumber() As System.String
            Get
                Return mtelephonenumber
            End Get
            Set(ByVal value As System.String)
                mtelephonenumber = value
            End Set
        End Property
        Private mlogonhours As System.String
        Public Property Logonhours() As System.String
            Get
                Return mlogonhours
            End Get
            Set(ByVal value As System.String)
                mlogonhours = value
            End Set
        End Property
        Private mlastlogontimestamp As System.String
        Public Property Lastlogontimestamp() As System.String
            Get
                Return mlastlogontimestamp
            End Get
            Set(ByVal value As System.String)
                mlastlogontimestamp = value
            End Set
        End Property
        Private mPropertyNameValueDataTable As New Data.DataTable
        Public Property PropertyNameValueDataTable() As Data.DataTable
            Get
                Return mPropertyNameValueDataTable
            End Get
            Set(ByVal value As Data.DataTable)
                mPropertyNameValueDataTable = value
            End Set
        End Property

    End Class
End Namespace
