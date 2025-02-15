using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using NonPersistentObjectsDemo.Module.BusinessObjects;

namespace NonPersistentFilteringEF.Module;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
public sealed class NonPersistentFilteringEFModule : ModuleBase {
    public NonPersistentFilteringEFModule() {
		// 
		// NonPersistentFilteringEFModule
		// 
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule));
    }
    public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
        ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
        return new ModuleUpdater[] { updater };
    }
    public override void Setup(XafApplication application) {
        base.Setup(application);
        // Manage various aspects of the application UI and behavior at the module level.
        application.SetupComplete += Application_SetupComplete;
    }
    private void Application_SetupComplete(object sender, EventArgs e) {
        Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
    }
    private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e) {
        var npos = e.ObjectSpace as NonPersistentObjectSpace;
        if (npos != null) {
            new ArticleAdapter(npos);
            new ContactAdapter(npos);
        }
    }
}
