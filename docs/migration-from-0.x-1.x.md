# Migrating from 0.x -> 1.x versions

Hello, so if your project currently uses the 0.x versions of EcsRx then there will be a bit of refactoring needed to transition over to the new world. There is some benefits you will get for doing this though including better performance and more extensible code.

## Consumption changes with 1.x

So historically EcsRx started specifically as a unity project, and as such all its framework and unity code were within the unity package. However now the *core* part of the framework is now a generic .net project which can be consumed within unity, monogame, godot or anywhere else .net runs. Due to this change the unity project references dlls for core and just contains source code for the unity parts.

Another big change is the way DI is consumed, so historically it had a hard dependency in Zenject, but in this new version there is a built in DI abstraction, so you can write your binding logic without touching DI specific modules/registries, this allows you to setup DI concerns for any platform without having a hard dependency on the specific framework.

## Breaking Changes

### Groups & Collections
- `IGroupAccessor` has become `IObservableGroup`
- `IGroupAccessorFilter`, `ICacheableGroupAccessorFilter`, `IGroupWatcher` has become `IComputedGroup`
- `IPool` has become `IEntityCollection`
- `IPool.Entities` has been removed, `IEntityCollection` is now `IEnumerable<IEntity>`
- `IPoolManager` has become `IEntityCollectionManager`
- `IGroup.TargettedComponents` has become `IGroup.RequiredComponents`
- CRUD operations on `IPool/Manager` no longer raise system wide events, they are now local `IObservable` events
- CRUD events are now batched, i.e `ComponentAddedEvent` has become `ComponentsAddedEvent`

### Systems
- All system `Execute` methods have been renamed to `Process`
- `ITeardownSystem` is now triggered JUST BEFORE an entity has required components removed
- All systems (other than `IManualSystem`) have been moved to a separate `EcsRx.System` project

### Entities
- `IEntity.Id` is no longer a `Guid` and is now an `int`
- `IEntity.AddComponent<T>` has been removed, but kept as an extension method on `IEntity`
- Most interactions at entity level are batched, i.e `AddComponent<T>()` is now `AddComponents(params IComponent[] components)`

### Others
- `Priority` attribute logic has been changed slightly, 0 is default, 1 runs before default, -1 runs after default, see intellisense for more info.
- `EcsRxApplication` has become `EcsRxApplicationBehaviour` and is now within Zenject namespace, its very similar to the previous version though

## Advice on migrating

## Code Refactoring
If you have resharper or similar refactoring tools you shouldn't have much trouble, in most cases you want to just do a find and replace on:

- `IPool` -> `IEntityCollection`
- `IGroupAccessor` -> `IObservableGroup`
- `IPoolManager` -> `IEntityCollectionManager`
- `EcsRxApplication` -> `EcsRxApplicationBehaviour`

If you attempt to compile now you will probably get a lot of errors around:
 
 - `Entities` not being a valid property, in this case you can probably just remove the `Entities` property as the container objects are now enumerable.
 
 - `EcsRxApplicationBehaviour` cannot be found, in this case just fix up the references (resharper will auto recommend for you) to the Zenject namespace.
 
 There will probably be a couple of other code related errors depending on how much of the library you used, but most of them can be solved looking at the breaking changes above.
 
 ## Scene Refactoring
 
In the new version there is no hard dependency on Zenject (not entirely true, but stick with it), so you can setup your dependencies in the scene via installers like you currently do, or you can use the cross platform DI abstraction system to write and load your code directly in the application. This is optional though, and would look like so:

### Installer style 
```c#
// Added to SceneContext/ProjectContext
public class SceneCollectionsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<FloorTiles>().AsSingle();
        Container.Bind<OuterWallTiles>().AsSingle();
        Container.Bind<WallTiles>().AsSingle();
        Container.Bind<FoodTiles>().AsSingle();
        Container.Bind<EnemyTiles>().AsSingle();
        Container.Bind<ExitTiles>().AsSingle();
    }
}
```

We can change this to just use the abstraction system like shown below.

### Application module style

```c#
public class SceneCollectionsModule : IDependencyModule
{
    public void Setup(IDependencyContainer container)
    {
        container.Bind<FloorTiles>(new BindingConfiguration{AsSingleton = true});
        container.Bind<OuterWallTiles>(new BindingConfiguration{AsSingleton = true});
        container.Bind<WallTiles>(new BindingConfiguration{AsSingleton = true});
        container.Bind<FoodTiles>(new BindingConfiguration{AsSingleton = true});
        container.Bind<EnemyTiles>(new BindingConfiguration{AsSingleton = true});
        container.Bind<ExitTiles>(new BindingConfiguration{AsSingleton = true});
    }
}
```

Then in our application do:

```c#
public class Application : EcsRxApplicationBehaviour
{
    protected override void RegisterModules()
    {
        base.RegisterModules();
        DependencyContainer.LoadModule<SceneCollectionsModule>();
    }

    // ... Other code
}
```

This way you are not dependent on putting stuff in the scene to get installed, and for those who have plugins, this makes your plugins be able to be consumed on almost all platforms.

There are *some* features missing using this approach as not all DI frameworks have the same feature set, so only commonly used things are provided this way, if you need to use the native Zenject container just do:

```c#
var zenjectContainer = Container.NativeContainer as DiContainer;
```

## HELP IM STILL STUCK

If you are hop on over to the discord channel and someone will be able to advise on what to do.