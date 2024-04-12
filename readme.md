<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/255626959/22.2.5%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T952649)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
*Files to look at*:

* [Contact.cs](./CS/EFCore/NonPersistentFilteringEF/NonPersistentFilteringEF.Module/BusinessObjects/Contact.cs)
* [Article.cs](./CS/EFCore/NonPersistentFilteringEF/NonPersistentFilteringEF.Module/BusinessObjects/Article.cs )
* [Module.cs](./CS/EFCore/NonPersistentFilteringEF/NonPersistentFilteringEF.Module/Module.cs )


# How to filter and sort Non-Persistent Objects

## Scenario

When a [Non\-Persistent Object](https://docs.devexpress.com/eXpressAppFramework/116516/concepts/business-model-design/non-persistent-objects) collection contains many objects, it is often useful to filter it. However, the built-in filtering facilities are disabled for non-persistent collections by default.

## Solution

To enable filtering and sorting for [Non\-Persistent Objects](https://docs.devexpress.com/eXpressAppFramework/116516/concepts/business-model-design/non-persistent-objects), use the built-in **DynamicCollection** class or a custom **DynamicCollectionBase** descendant.

Here, we create a **DynamicCollection** instance and pass it in the [NonPersistentObjectSpace\.ObjectsGetting](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.NonPersistentObjectSpace.ObjectsGetting) event handler. We subscribe to the **DynamicCollection.FetchObjects** event and pass a new collection of non-persistent objects every time the filtering or sorting parameters are changed. If you cannot filter the collection manually, set the **ShapeData** event parameter to *true*. Then, **DynamicCollection** will process data (filter, sort, trim) internally.

This example demonstrates two approaches to filter objects.

- *Contact* objects are filtered at the storage level. Criteria and Sorting values passed in event parameters are converted into a storage-specific format and used in arguments of the *DataTable.Select* method call. DataSet returns filtered and sorted data that is then transformed into non-persistent objects. This approach can be useful if data for non-persistent objects is obtained from a remote service, a custom database query or a stored procedure.

- *Article* objects are filtered and sorted by DynamicCollection internally. This functionality is enabled when the **ShapeData** parameter of the **DynamicCollection.FetchObjects** event is set to *true*. This approach is useful when all data is already available and no custom processing is required.

When **DynamicCollection** is used, the built-in [FullTextSearch Action](https://docs.devexpress.com/eXpressAppFramework/112997/concepts/filtering/full-text-search-action) is shown in corresponding non-persistent list views.

*FindArticlesController* in this example shows a custom search form with a lookup editor that allows filtering non-persistent objects in a lookup list view.

Filtering and sorting at the data source level is also supported in Reports. Use the [Criteria](https://docs.devexpress.com/eXpressAppFramework/DevExpress.Persistent.Base.ReportsV2.DataSourceBase.Criteria) and [Sorting](https://docs.devexpress.com/eXpressAppFramework/DevExpress.Persistent.Base.ReportsV2.DataSourceBase.Sorting) properties of [CollectionDataSource](https://docs.devexpress.com/eXpressAppFramework/DevExpress.Persistent.Base.ReportsV2.CollectionDataSource) to specify filter criteria and sorting parameters.

