Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.ExpressApp.ConditionalAppearance
Imports DevExpress.ExpressApp.DC
Imports DevExpress.Persistent.Base
Imports NonPersistentObjectsDemo.Module.BusinessObjects

Namespace NonPersistentObjectsDemo.Module.Controllers

	Public Class FindArticlesController
		Inherits ViewController

		Private action As PopupWindowShowAction
		Public Sub New()
			action = New PopupWindowShowAction(Me, "FindArticles", PredefinedCategory.View)
			AddHandler action.CustomizePopupWindowParams, AddressOf Action_CustomizePopupWindowParams
			AddHandler action.Execute, AddressOf Action_Execute
		End Sub
		Private Sub Action_CustomizePopupWindowParams(ByVal sender As Object, ByVal e As CustomizePopupWindowParamsEventArgs)
			Dim objectSpace = Application.CreateObjectSpace(GetType(FindArticlesDialog))
			Dim obj = New FindArticlesDialog()
			Dim detailView = Application.CreateDetailView(objectSpace, obj)
			detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit
			e.View = detailView
		End Sub
		Private Sub Action_Execute(ByVal sender As Object, ByVal e As PopupWindowShowActionExecuteEventArgs)
		End Sub
	End Class

	<DomainComponent>
	Public Class FindArticlesDialog
		Inherits NonPersistentObjectBase

		Private _Author As Contact
		<ImmediatePostData>
		Public Property Author() As Contact
			Get
				Return _Author
			End Get
			Set(ByVal value As Contact)
				SetPropertyValue(Of Contact)(NameOf(Author), _Author, value)
			End Set
		End Property
		Private _AuthorMinRating As Single
		<Appearance("", Enabled := False, Criteria := "Author is not null")>
		Public Property AuthorMinRating() As Single
			Get
				Return _AuthorMinRating
			End Get
			Set(ByVal value As Single)
				SetPropertyValue(Of Single)(NameOf(AuthorMinRating), _AuthorMinRating, value)
			End Set
		End Property
		Private _Articles As BindingList(Of Article)
		Public ReadOnly Property Articles() As BindingList(Of Article)
			Get
				If _Articles Is Nothing Then
					_Articles = New BindingList(Of Article)()
				End If
				Return _Articles
			End Get
		End Property
		Private Sub UpdateArticles()
			If _Articles IsNot Nothing Then
				Dim filter = GetCriteria()
				_Articles.RaiseListChangedEvents = False
				_Articles.Clear()
				For Each obj In ObjectSpace.GetObjects(Of Article)(filter)
					_Articles.Add(obj)
				Next obj
				_Articles.RaiseListChangedEvents = True
				_Articles.ResetBindings()
				OnPropertyChanged(NameOf(Articles))
			End If
		End Sub
		Private Function GetCriteria() As CriteriaOperator
			If Author IsNot Nothing Then
				Return New BinaryOperator("Author.UserName", Author.UserName)
			Else
				Return CriteriaOperator.Parse("Author.Rating >= ?", AuthorMinRating)
			End If
		End Function
		<Action(PredefinedCategory.Filters)>
		Public Sub Find()
			UpdateArticles()
		End Sub
	End Class
End Namespace
