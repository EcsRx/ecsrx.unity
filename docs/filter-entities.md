# Filtering/Caching Entities

So now you know what entities are and how you can get hold of them, its worth going over the layers of filtration in place out the box as well as at what levels caching takes place.

## Filtration Flow

```
IPoolManager          <-  This contains all pools, which in turn contains ALL entities
     |
     |
IGroupAccessor        <-  This filters all entities down to only ones which are within the group
     |                    i.e All entities which contain PlayerComponent
     |
IGroupAccessorFilter  <-  This acts as another layer of filtration on an IGroupAccessor
                          i.e Top 5 entities with PlayerComponent sorted by Score
```

## IPoolManager

The pool manager is the root most point where all entity queries should originate from.

The pool manager also maintains a collection of `IGroupAccessors` so if you have 5 systems which all use the same group, there will only actually be 1 instance of the `IGroupAccessor` that is shared between them all. 

## IGroupAccessor

The group accessor is created within and maintained by an `IPoolManager` and exposes all entities which match the associated group. There are a couple of implementations of group accessor, and while both expose the matching entities, they differ greatly on how they get the maintaining entities.

### `CacheableGroupAccessor` (Used by default)

The cacheable implementation actively tracks when an entity is added/removed and maintains an internal list of currently applicable entities. While there is a small amount of overhead for it to subscribe to entity changes in most cases this will be outweighed by the performance benefit of having a list which doesn't need to enumerate when queried.

### `GroupAccessor`

This is the simplest version which just contains an `IEnumerable` which can be evaluated to get the entities matching the group at point of invocation. This can be slightly more performant if you have a custom small pool and small group and want to just evaluate them all easily, but generally due to the lack of caching at the entity level this doesnt really scale well with larger pools/groups.

## IGroupAccessorFilter

The group accessor filter is created manually and requires a `IGroupAccessorFilter` for it to use for the basis of its queries. It is provided to allow you to filter past the group level and get more specific data sets without having to hardcode the logic for the lookup in various systems.

General use cases for this may be things like:

- Get 5 highest scoring players
- Get enemies within a radius of the player
- Transform entitys with various game state components to some single poco for saving state

It is meant to be an interface for you to implement with your own filtration logic, however there is an existing abstract implementation which provides caching for the filter results out of the box.

### `CacheableGroupAccessorFilter`

This group filter has caching built in so it will try to keep a pre-evaluted list of entities which match the filtration requirements, this can be beneficial if you are using this in a few places and want it to update automatically when the underlying data changes.

## Querys

So up to this point we have discussed the general filtration process, however there are some extension methods which let you do more ad-hoc queries on data, these are not cached in any way but allow you to drill down into a subset of data in a pre-defined way, this overlaps a bit with the `IGroupAccessorFilter` but lets you query directly at the pool level or accessor level.

Both `IPool` and `IGroupAccessor` has a `Query` extension method which takes an `IPoolQuery` or `IGroupAccessorQuery` where you can implement your desired query logic. This was added so you could use the pools and group accessors more like repositories and use pre defined queries to access them consistently.
