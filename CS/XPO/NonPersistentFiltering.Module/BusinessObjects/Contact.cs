using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace NonPersistentObjectsDemo.Module.BusinessObjects {

    [DefaultClassOptions]
    [DefaultProperty(nameof(Contact.FullName))]
    [DevExpress.ExpressApp.ConditionalAppearance.Appearance("", Enabled = false, TargetItems = "*")]
    [DevExpress.ExpressApp.DC.DomainComponent]
    public class Contact :NonPersistentEntityObject {
        internal Contact() { }
        private string _UserName;
        [DevExpress.ExpressApp.Data.Key]
        public string UserName {
            get { return _UserName; }
            set { _UserName = value; }
        }
        private string _FullName;
        public string FullName {
            get { return _FullName; }
            set { SetPropertyValue(ref _FullName, value); }
        }
        private int _Age;
        public int Age {
            get { return _Age; }
            set { SetPropertyValue<int>(ref _Age, value); }
        }
        private float _Rating;
        public float Rating {
            get { return _Rating; }
            set { SetPropertyValue<float>(ref _Rating, value); }
        }
    }

    class ContactAdapter {
        private NonPersistentObjectSpace objectSpace;

        public ContactAdapter(NonPersistentObjectSpace npos) {
            this.objectSpace = npos;
            objectSpace.ObjectsGetting += ObjectSpace_ObjectsGetting;
        }
        private void ObjectSpace_ObjectsGetting(object sender, ObjectsGettingEventArgs e) {
            if(e.ObjectType == typeof(Contact)) {
                var collection = new DynamicCollection(objectSpace, e.ObjectType, e.Criteria, e.Sorting, e.InTransaction);
                collection.FetchObjects += DynamicCollection_FetchObjects;
                e.Objects = collection;
            }
        }
        private void DynamicCollection_FetchObjects(object sender, FetchObjectsEventArgs e) {
            if(e.ObjectType == typeof(Contact)) {
                var rows = contactsStorage.GetContactRows(e.Criteria, e.Sorting);
                e.Objects = rows.Select(row => GetContact(row));
            }
        }
        private static Contact GetContact(DataRow row) {
            Contact obj;
            var key = row["UserName"] as string;
            if(!contactsCache.TryGetValue(key, out obj)) {
                obj = new Contact() {
                    UserName = key,
                    FullName = (string)row["FullName"],
                    Age = (int)row["Age"],
                    Rating = (float)row["Rating"]
                };
                contactsCache.Add(key, obj);
            }
            return obj;
        }

        private static ContactStorage contactsStorage;
        private static Dictionary<string, Contact> contactsCache;
        internal static IList<Contact> GetAllContacts() {
            return contactsStorage.GetContactRows(null, null).Select(row => GetContact(row)).ToList();
        }
        static ContactAdapter() {
            contactsStorage = new ContactStorage();
            contactsStorage.LoadDemoData();
            contactsCache = new Dictionary<string, Contact>();
        }
    }

    class ContactStorage {
        private DataSet dataSet;
        public IList<DataRow> GetContactRows(CriteriaOperator criteria, IList<SortProperty> sorting) {
            var filter = CriteriaToWhereClauseHelper.GetDataSetWhere(criteria);
            string sort = null;
            if(sorting != null && sorting.Count == 1 && sorting[0].Property is OperandProperty) {
                sort = string.Format("{0} {1}", sorting[0].PropertyName, sorting[0].Direction == DevExpress.Xpo.DB.SortingDirection.Ascending ? "ASC" : "DESC");
            }
            return dataSet.Tables["Contacts"].Select(filter, sort);
        }
        public ContactStorage() {
            dataSet = new DataSet();
            {
                var dt = dataSet.Tables.Add("Contacts");
                var colID = dt.Columns.Add("UserName", typeof(string));
                dt.Columns.Add("FullName", typeof(string));
                dt.Columns.Add("Age", typeof(int));
                dt.Columns.Add("Rating", typeof(float));
                dt.PrimaryKey = new DataColumn[] { colID };
            }
            LoadDemoData();
        }
        public void LoadDemoData() {
            var dt = dataSet.Tables["Contacts"];
            for(int i = 0; i < 50; i++) {
                var id = i;
                var fullName = "FullName" + i;
                var age = 16 + i;
                var rating = new Random(i).Next(10, 90);
                dt.LoadDataRow(new object[] { id, fullName, age, rating }, LoadOption.OverwriteChanges);
            }
        }
    }
}
