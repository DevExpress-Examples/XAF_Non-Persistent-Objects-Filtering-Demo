using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using NonPersistentObjectsDemo.Module.BusinessObjects;

namespace NonPersistentObjectsDemo.Module.Controllers {

    public class FindArticlesController : ViewController {
        private PopupWindowShowAction action;
        public FindArticlesController() {
            action = new PopupWindowShowAction(this, "FindArticles", PredefinedCategory.View);
            action.CustomizePopupWindowParams += Action_CustomizePopupWindowParams;
            action.Execute += Action_Execute;
        }
        private void Action_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
            var objectSpace = Application.CreateObjectSpace(typeof(FindArticlesDialog));
            var obj = new FindArticlesDialog();
            var detailView = Application.CreateDetailView(objectSpace, obj);
            detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            e.View = detailView;
        }
        private void Action_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {
        }
    }

    [DomainComponent]
    public class FindArticlesDialog :NonPersistentObjectImpl {
        private Contact _Author;
        [ImmediatePostData]
        public Contact Author {
            get { return _Author; }
            set { SetPropertyValue<Contact>(ref _Author, value); }
        }
        private float _AuthorMinRating;
        [Appearance("", Enabled = false, Criteria = "Author is not null")]
        public float AuthorMinRating {
            get { return _AuthorMinRating; }
            set { SetPropertyValue<float>(ref _AuthorMinRating, value); }
        }
        private BindingList<Article> _Articles;
        public BindingList<Article> Articles {
            get {
                if(_Articles == null) {
                    _Articles = new BindingList<Article>();
                }
                return _Articles;
            }
        }
        private void UpdateArticles() {
            if(_Articles != null) {
                var filter = GetCriteria();
                _Articles.RaiseListChangedEvents = false;
                _Articles.Clear();
                foreach(var obj in ObjectSpace.GetObjects<Article>(filter)) {
                    _Articles.Add(obj);
                }
                _Articles.RaiseListChangedEvents = true;
                _Articles.ResetBindings();
                OnPropertyChanged(nameof(Articles));
            }
        }
        private CriteriaOperator GetCriteria() {
            if(Author != null) {
                return new BinaryOperator("Author.UserName", Author.UserName);
            }
            else {
                return CriteriaOperator.Parse("Author.Rating >= ?", AuthorMinRating);
            }
        }
        [Action(PredefinedCategory.PopupActions)]
        public void Find() {
            UpdateArticles();
        }
    }
}
