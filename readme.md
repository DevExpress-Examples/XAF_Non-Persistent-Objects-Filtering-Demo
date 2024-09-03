<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/255626959/24.2.1%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T952649)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# XAF - How to filter and sort non-persistent objects

If a collection of [non\-persistent objects](https://docs.devexpress.com/eXpressAppFramework/116516/concepts/business-model-design/non-persistent-objects) contains many items, you may need to filter it. However, built-in filters are disabled for non-persistent collections by default.

> [!WARNING]
> We created this example for demonstration purposes and it is not intended to address all possible usage scenarios.
> If this example does not have certain functionality or you want to change its behavior, you can extend this example. This can be a complex task that requires good knowledge of XAF: [UI Customization Categories by Skill Level](https://www.devexpress.com/products/net/application_framework/xaf-considerations-for-newcomers.xml#ui-customization-categories), and you may need to research how our components work. Refer to the following help topic for more information: [Debug DevExpress .NET Source Code with PDB Symbols](https://docs.devexpress.com/GeneralInformation/403656/support-debug-troubleshooting/debug-controls-with-debug-symbols).
> We are unable to help with such tasks as custom programming is outside our Support Service scope: [Technical Support Scope](https://www.devexpress.com/products/net/application_framework/xaf-considerations-for-newcomers.xml#support).

## Implementation Details

To enable filtering and sorting for [non\-persistent objects](https://docs.devexpress.com/eXpressAppFramework/116516/concepts/business-model-design/non-persistent-objects), you can use either the built-in `DynamicCollection` class or a custom `DynamicCollectionBase` descendant.

1. Create a `DynamicCollection` instance and pass it in the [NonPersistentObjectSpace\.ObjectsGetting](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.NonPersistentObjectSpace.ObjectsGetting) event handler.
2. Subscribe to the `DynamicCollection.FetchObjects` event and pass a new collection of non-persistent objects every time filter or sort parameters change.
3. If you cannot filter the collection, set the `ShapeData` event parameter to `true`. The `DynamicCollection` then processes the data (filter, sort, and trim) internally.
   
     When you use the `DynamicCollection`, the built-in [FullTextSearch action](https://docs.devexpress.com/eXpressAppFramework/112997/concepts/filtering/full-text-search-action) is displayed in corresponding non-persistent list views.

4. The `FindArticlesController` in this example shows a custom search form with a lookup editor that allows filtering non-persistent objects in a lookup list view.

This example demonstrates two approaches to filter objects.

- **Contact** objects are filtered at the storage level. `Criteria` and `Sorting` values passed in event parameters are converted to a storage-specific format and used as arguments of the `DataTable.Select` method call. `DataSet` returns filtered and sorted data that is then transformed into non-persistent objects. This approach can be useful when data for non-persistent objects is obtained from a remote service, a custom database query or a stored procedure.

- **Article** objects are filtered and sorted internally by a `DynamicCollection`. This functionality is enabled when the `ShapeData` parameter of the `DynamicCollection.FetchObjects` event is set to `true`. This approach is useful when all data is already available and no custom processing is required.

## Files to Review

- [Contact.cs](./CS/EFCore/NonPersistentFilteringEF/NonPersistentFilteringEF.Module/BusinessObjects/Contact.cs)
- [Article.cs](./CS/EFCore/NonPersistentFilteringEF/NonPersistentFilteringEF.Module/BusinessObjects/Article.cs )
- [Module.cs](./CS/EFCore/NonPersistentFilteringEF/NonPersistentFilteringEF.Module/Module.cs )
- [FindArticlesController.cs](CS/EFCore/NonPersistentFilteringEF/NonPersistentFilteringEF.Module/Controllers/FindArticlesController.cs)

## Documentation

- [Non-Persistent Objects](https://docs.devexpress.com/eXpressAppFramework/116516/business-model-design-orm/non-persistent-objects)


## More Examples

- [How to implement CRUD operations for Non-Persistent Objects stored remotely in eXpressApp Framework](https://github.com/DevExpress-Examples/XAF_Non-Persistent-Objects-Editing-Demo)
- [How to edit Non-Persistent Objects nested in a Persistent Object](https://github.com/DevExpress-Examples/XAF_Non-Persistent-Objects-Nested-In-Persistent-Objects-Demo)
- [How to: Display a List of Non-Persistent Objects](https://github.com/DevExpress-Examples/XAF_how-to-display-a-list-of-non-persistent-objects-e980)
- [How to filter and sort Non-Persistent Objects](https://github.com/DevExpress-Examples/XAF_Non-Persistent-Objects-Filtering-Demo)
- [How to refresh Non-Persistent Objects and reload nested Persistent Objects](https://github.com/DevExpress-Examples/XAF_Non-Persistent-Objects-Reloading-Demo)
- [How to edit a collection of Persistent Objects linked to a Non-Persistent Object](https://github.com/DevExpress-Examples/XAF_Non-Persistent-Objects-Edit-Linked-Persistent-Objects-Demo)
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=XAF_Non-Persistent-Objects-Filtering-Demo&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=XAF_Non-Persistent-Objects-Filtering-Demo&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
