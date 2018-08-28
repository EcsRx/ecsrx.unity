# Views & View Resolvers

There is a document in the [EcsRx code docs around the same subject](https://github.com/EcsRx/ecsrx/blob/master/docs/views-and-resolving.md), this will be more unity specific in nature.

---

A large part of game logic will take place in game objects, so to this end the notion of views have been added to try and provide some streamlined and consistent way of interacting with GameObjects from within the ECS layer. This is important as everyone could come up with their own way of managing the notion of views for an entity, and some people still will anyway, however as this is a foundation to be built upon if everyone can agree upon this convention then it makes it easier for people to develop and consume plugins which will interact with views, so it adds a sort of view contract that not only you as the developer can adhere to but people writing further plugins for the system can follow allowing there to be some basic standards for view creation and control.

## ViewComponent

As mentioned in the core docs there is an existing notion of `ViewComponent`, so look there for more info on that.

## View Resolving System Conventions

There are a few pre-made systems which can be used for common conventions, or if you need to work outside these conventions you can extend the core `ViewResolverSystem` or `IViewResolverSystem`.

### PrefabViewResolverSystem

This conventional system allows you to just resolve views from prefabs then apply any custom data to the view once its been created. Here is an example of a simple prefab view resolver.

```c#
public class ExamplePrefabViewResolverSystem : PrefabViewResolverSystem
{	
	public override IGroup Group {get;} = new SomeViewGroup();

    protected override GameObject PrefabTemplate { get; } = Resources.Load<GameObject>("some-prefab");
	
	protected override void OnViewCreated(IEntity entity, GameObject view)
	{ 
	    // Do something on the GameObject (view)
	    view.name = $"some-view-name-{entity.Id}"; 
	}
}
```

### PooledPrefabViewResolverSystem

The pooled prefab system acts a lot like the normal prefab view resolver system, but in this case it will basically keep the underlying view elements alive, this lets you skip the overhead of constantly creating and destroying game objects, which can be useful in scenarios like shmups or other high volume scene object.

```c#
    public class SelfDestructionViewResolver : PooledPrefabViewResolverSystem
    {
        public override IGroup Group { get; } = new Group(typeof(SelfDestructComponent), typeof(ViewComponent));

        public SelfDestructionViewResolver(IInstantiator instantiator, IEntityCollectionManager collectionManager, IEventSystem eventSystem) : base(instantiator, collectionManager, eventSystem)
        {}

        protected override GameObject PrefabTemplate { get; } = Resources.Load("PooledPrefab") as GameObject;
        protected override int PoolIncrementSize => 5;

        protected override void OnPoolStarting()
        { ViewPool.PreAllocate(30); }

        protected override void OnViewAllocated(GameObject view, IEntity entity)
        {
            view.name = $"pooled-active-{entity.Id}";
            
            var selfDestructComponent = entity.GetComponent<SelfDestructComponent>();
            view.transform.position = selfDestructComponent.StartingPosition;
        }

        protected override void OnViewRecycled(GameObject view, IEntity entity)
        {
            view.name = "pooled-inactive";
        }
    }
```

### DynamicViewResolverSystem

This system provides a more flexible approach to view creation where you are free to create your game object via code without using an existing prefab, this can be useful in scenarios where you may have many prefabs and you want to pick one at random, or you need to factor in data on the entity when building the `GameObject`.

```c#
public class WallViewResolver : DynamicViewResolverSystem
{
    private readonly WallTiles _wallTiles;
    
    public override IGroup Group { get; } = new Group(typeof(WallComponent), typeof(ViewComponent));

    public WallViewResolver(IEventSystem eventSystem, IEntityCollectionManager collectionManager, IInstantiator instantiator, WallTiles wallTiles) 
        : base(eventSystem, collectionManager, instantiator)
    {
        _wallTiles = wallTiles;
    }

    public override GameObject CreateView(IEntity entity)
    {
        var tileChoice = _wallTiles.AvailableTiles.TakeRandom();
        var gameObject = Object.Instantiate(tileChoice, Vector3.zero, Quaternion.identity) as GameObject;
        gameObject.name = $"wall-{entity.Id}";
        return gameObject;
    }

    public override void DestroyView(IEntity entity, GameObject view)
    { GameObject.Destroy(view); }
}
```