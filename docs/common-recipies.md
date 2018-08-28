# Common Recipes

This is an attempt to try and provide some basic use cases for different system types etc.

---

## Reacting to collision

Assuming you have an entity with a `ViewComponent` you may want to know when something collides with it, so you could achieve that using an `IReactToDataSystem` which provides you a way to access both the entity and some data related to it.

Here is an example of a bullet being shot

```
public class BulletInteractionSystem : IReactToDataSystem
{
	public IGroup Group { get { return new Group (typeof(BulletComponent), typeof(ViewComponent)); } }

	public IObservable<Collider> ReactToData(IEntity entity)
	{
		var viewComponent = entity.GetComponent<ViewComponent>();
		return viewComponent.View.OnTriggerEnterAsObservable ()
			.Where(collider => collider.gameObject.tag == "Enemy");
	}

	public void Process(Collider collider, IEntity entity)
	{
        // Do something with the collider and the entity
	}
}
```
---

## Handling events in systems

Sometimes you just want to listen for an event to happen then do something with it, for you there is the `EventReactionSystem<T>` which will take a type of event you wish to recieve and let you handle it in isolation.

Here is an example of an enemy being hit and removing them if their health <= 0

```
public class EnemyAttackedSystem : EventReactionSystem<EnemyHitEvent>
{
    public EnemyAttackedSystem(IEventSystem eventSystem) : base(eventSystem)
    {}

    public override void EventTriggered(EnemyHitEvent eventData)
    {
        // Do something with the event
    }
}
```

It is generally best practice to make events contain all the data it needs to run so you do not need to query external sources or access any state outside of the scope of the event. You can do this but it can often be more problematic, the above example came from the RogueLike2d example [EnemyAttackedSystem.cs](https://github.com/ecsrx/ecsrx.roguelike2d/blob/master/Assets/Game/Systems/EnemyAttackedSystem.cs)

--- 

## Handling complex non-entity specific logic in systems

Sometimes you need to do some logic at a higher level than just one entity or you need to have a few entities but your main interactions are at the UI level or somewhere else, in these cases you can solve the problem by using an `IManualSystem` to allow you more control over the underlying system and how it interacts with entities and other aspects of the application.

This example is stolen from the Roguelike2d example [FoodTextUpdateSystem.cs](https://github.com/ecsrx/ecsrx.roguelike2d/blob/master/Assets/Game/Systems/FoodTextUpdateSystem.cs) and shows how the system is really only fussed about keeping the UI reading for food up to date, but has to interact with many different events and the player entity to manage this.

```c#
 public class FoodTextUpdateSystem : IManualSystem
{
    public IGroup Group { get; } = new Group(typeof(PlayerComponent));

    private readonly IEventSystem _eventSystem;
    private PlayerComponent _playerComponent;
    private Text _foodText;
    private readonly IList<IDisposable> _subscriptions = new List<IDisposable>();

    public FoodTextUpdateSystem(IEventSystem eventSystem)
    { _eventSystem = eventSystem; }

    public void StartSystem(IObservableGroup group)
    {
        this.WaitForScene().Subscribe(x =>
        {
            var player = group.First();
            _playerComponent = player.GetComponent<PlayerComponent>();
            _foodText = GameObject.Find("FoodText").GetComponent<Text>();

            SetupSubscriptions();
        });
    }

    private void SetupSubscriptions()
    {
        _playerComponent.Food.DistinctUntilChanged()
            .Subscribe(foodAmount => { _foodText.text = $"Food: {foodAmount}"; })
            .AddTo(_subscriptions);

        _eventSystem.Receive<FoodPickupEvent>()
            .Subscribe(x =>
            {
                var foodPoints = x.Food.GetComponent<FoodComponent>().FoodAmount;
                _foodText.text = $"+{foodPoints} Food: {_playerComponent.Food.Value}";
            })
            .AddTo(_subscriptions);

        _eventSystem.Receive<PlayerHitEvent>()
            .Subscribe(x =>
            {
                var attackScore = x.Enemy.GetComponent<EnemyComponent>().EnemyPower;
                _foodText.text = $"-{attackScore} Food: {_playerComponent.Food.Value}";
            })
            .AddTo(_subscriptions);
    }

    public void StopSystem(IObservableGroup group)
    { _subscriptions.DisposeAll(); }
}
```
