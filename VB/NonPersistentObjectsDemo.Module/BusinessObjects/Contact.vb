Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Xpo

Namespace NonPersistentObjectsDemo.Module.BusinessObjects

	<DefaultClassOptions>
	<DefaultProperty(NameOf(FullName))>
	<DevExpress.ExpressApp.ConditionalAppearance.Appearance("", Enabled := False, TargetItems := "*")>
	<DevExpress.ExpressApp.DC.DomainComponent>
	Public Class Contact
		Inherits NonPersistentObjectBase

		Friend Sub New()
		End Sub
'INSTANT VB NOTE: The field userName was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private userName_Conflict As String
		<DevExpress.ExpressApp.Data.Key>
		Public Property UserName() As String
			Get
				Return userName_Conflict
			End Get
			Set(ByVal value As String)
				userName_Conflict = value
			End Set
		End Property
'INSTANT VB NOTE: The field fullName was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private fullName_Conflict As String
		Public Property FullName() As String
			Get
				Return fullName_Conflict
			End Get
			Set(ByVal value As String)
				SetPropertyValue(NameOf(FullName), fullName_Conflict, value)
			End Set
		End Property
		Private _Age As Integer
		Public Property Age() As Integer
			Get
				Return _Age
			End Get
			Set(ByVal value As Integer)
				SetPropertyValue(Of Integer)(NameOf(Age), _Age, value)
			End Set
		End Property
		Private _Rating As Single
		Public Property Rating() As Single
			Get
				Return _Rating
			End Get
			Set(ByVal value As Single)
				SetPropertyValue(Of Single)(NameOf(Rating), _Rating, value)
			End Set
		End Property
	End Class

	Friend Class ContactAdapter
		Private objectSpace As NonPersistentObjectSpace

		Public Sub New(ByVal npos As NonPersistentObjectSpace)
			Me.objectSpace = npos
			AddHandler objectSpace.ObjectsGetting, AddressOf ObjectSpace_ObjectsGetting
		End Sub
		Private Sub ObjectSpace_ObjectsGetting(ByVal sender As Object, ByVal e As ObjectsGettingEventArgs)
			If e.ObjectType Is GetType(Contact) Then
				Dim collection = New DynamicCollection(objectSpace, e.ObjectType, e.Criteria, e.Sorting, e.InTransaction)
				AddHandler collection.ObjectsGetting, AddressOf DynamicCollection_ObjectsGetting
				e.Objects = collection
			End If
		End Sub
		Private Sub DynamicCollection_ObjectsGetting(ByVal sender As Object, ByVal e As DynamicObjectsGettingEventArgs)
			If e.ObjectType Is GetType(Contact) Then
				Dim rows = contactsStorage.GetContactRows(e.Criteria, e.Sorting)
				e.Objects = rows.Select(Function(row) GetContact(row))
			End If
		End Sub
		Private Shared Function GetContact(ByVal row As DataRow) As Contact
			Dim obj As Contact = Nothing
			Dim key = TryCast(row("UserName"), String)
			If Not contactsCache.TryGetValue(key, obj) Then
				obj = New Contact() With {
					.UserName = key,
					.FullName = DirectCast(row("FullName"), String),
					.Age = DirectCast(row("Age"), Integer),
					.Rating = DirectCast(row("Rating"), Single)
				}
				contactsCache.Add(key, obj)
			End If
			Return obj
		End Function

		Private Shared contactsStorage As ContactStorage
		Private Shared contactsCache As Dictionary(Of String, Contact)
		Friend Shared Function GetAllContacts() As IList(Of Contact)
			Return contactsStorage.GetContactRows(Nothing, Nothing).Select(Function(row) GetContact(row)).ToList()
		End Function
		Shared Sub New()
			contactsStorage = New ContactStorage()
			contactsStorage.LoadDemoData()
			contactsCache = New Dictionary(Of String, Contact)()
		End Sub
	End Class

	Friend Class ContactStorage
		Private dataSet As DataSet
		Public Function GetContactRows(ByVal criteria As CriteriaOperator, ByVal sorting As IList(Of SortProperty)) As IList(Of DataRow)
			Dim filter = CriteriaToWhereClauseHelper.GetDataSetWhere(criteria)
			Dim sort As String = Nothing
			If sorting IsNot Nothing AndAlso sorting.Count = 1 AndAlso TypeOf sorting(0).Property Is OperandProperty Then
				sort = String.Format("{0} {1}", sorting(0).PropertyName,If(sorting(0).Direction = DevExpress.Xpo.DB.SortingDirection.Ascending, "ASC", "DESC"))
			End If
			Return dataSet.Tables("Contacts").Select(filter, sort)
		End Function
		Public Sub New()
			dataSet = New DataSet()
			If True Then
				Dim dt = dataSet.Tables.Add("Contacts")
				Dim colID = dt.Columns.Add("UserName", GetType(String))
				dt.Columns.Add("FullName", GetType(String))
				dt.Columns.Add("Age", GetType(Integer))
				dt.Columns.Add("Rating", GetType(Single))
				dt.PrimaryKey = New DataColumn() { colID }
			End If
			LoadDemoData()
		End Sub
		Public Sub LoadDemoData()
			Dim dt = dataSet.Tables("Contacts")
			Dim gen = New GenHelper()
			For i As Integer = 0 To 199
				Dim id = gen.MakeTosh(20)
				Dim fullName = gen.GetFullName()
				Dim age = 16 + gen.Next(80)
				Dim rating = gen.Next(100) * gen.Next(100) * 0.001F
				dt.LoadDataRow(New Object() { id, fullName, age, rating }, LoadOption.OverwriteChanges)
			Next i
		End Sub
	End Class
End Namespace
