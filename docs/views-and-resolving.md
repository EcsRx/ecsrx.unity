# Views & View Resolvers

A large part of game logic will take place on and in game objects, so to this end the notion of views have been added to try
and provide some streamlined and consistent way of interacting with GameObjects from within the ECS layer. This is important 
as everyone could come up with their own way of managing the notion of views for an entity, and some people still will anyway, 
however as this is a foundation to be built upon if everyone can agree upon this convention then it makes it easier for people 
to develop and consume plugins which will interact with views, so it adds a sort of view contract that not only you as the 
developer can adhere to but people writing further plugins for the system can follow allowing there to be some basic standards
for view creation and control.

## ViewComponent

So the first part of this convention is a `ViewComponent` which contains the GameObject (known as the View) and some configuration
around the view, such as if the entity should self destruct if the view is destroyed, and may be added to more as the project 
evolves.

So to make use of `ViewResolverSystem`s you will need to have applied a `ViewComponent` to you entity, like so:

```
someEntity.AddComponent<ViewComponent>();
```

This will get populated elsewhere via a specific type of system, but when you are wanting to access the view from the ECS
layer you will be wanting to get it via this component.

## ViewResolverSystem

This is the other part of the puzzle, and is a specific kind of abstract `ISetupSystem` implementation which specifically 
targets entities with views and attempts to resolve the GameObject for the `ViewComponent` to use.

So the key thing about this class is that it requires you to implement your own `ResolveView` method, which will return an
instantiated GameObject for use as the View, it is completely up to you how you instantiate it and if you need to load a prefab
or multiple child views which you add to a singular container view etc.

It is also recommended that you override the `TargetGroup` to indicate the grouping for your view. So for example if you were
to have 2 view resolvers, one for a vehicle and a player you would want to make sure that you targetted the right groups for
each resolver, here is an example of two hypothetical resolvers for this scenario:

### VehicleViewResolver
```
public class VehicleViewResolver : ViewResolverSystem
{
	public override IGroup TargetGroup`
	{
		get { return base.TargetGroup.WithComponent<VehicleComponent>(); }
	}
	
	public override GameObject ResolveView(IEntity entity)
	{
		var vehicleComponent = entity.GetComponent<VehicleComponent>();
		var vehiclePrefab = Resources.Load(vehicleComponent.VehicleType);
		return (GameObject)GameObject.Instantiate(vehiclePrefab);
	}
}
```

### PlayerViewResolver
```
public class PlayerViewResolver : ViewResolverSystem
{
	private GameObject _playerPrefab;
	
	public override IGroup TargetGroup`
	{
		get { return base.TargetGroup.WithComponent<PlayerComponent>(); }
	}

	public PlayerViewResolver()
	{
		_playerPrefab = Resources.Load("player");
	}
	
	public override GameObject ResolveView(IEntity entity)
	{
		return (GameObject)GameObject.Instantiate(_playerPrefab);
	}
}
```

## Putting it all together

So as long as you apply the `ViewComponent` and add the various resolvers to the `SystemExecutor` it will automatically
setup these views for you when the entities are created. It is recommended that you pair this with the notion of blueprints
so you can have consistant setups for your entities.

## Do I need this?

This is only really for entities which need visual embodiments in the unity scene, if you are working with purely data driven 
entities then you wont need to apply and of this to those entities. So don't feel you NEED to have view based entities, it just 
provides you a mechanism to cope with these scenarios in a consistent way.

## Ignoring all of this and still using game objects

There is nothing stopping you ignoring this whole convention and having your own convention or just having various other 
components which have game objects and manually creating them via systems or whatever notion you want. This approach is still
using entities, components and systems so it is still true to the underlying pattern, it just simplifies the creation and 
management of views to the core parts, so really think if what you need is not already solved by this approach.