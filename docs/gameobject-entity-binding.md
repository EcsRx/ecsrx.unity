# GameObject & Entity Binding (Still some work to do)

So we have the notion of `ViewComponents` and `ViewResolverSystems` and this solves view handling from the ECS framework side,
however there will be those rogue operatives who will want to create game objects in the scene first and then have them 
create entities when the scene begins.

So to solve this situation where you are effectively working scene first but still want to hook into the ECS systems and components 
we have added a MonoBehaviour called `EntityBinding`, which contains a link to the underlying entity for use in the unity world.

## Registering entities for GameObjects

There is another monobehaviour called `RegisterAsEntity` which acts as the bootstrapper for game objects so when you apply 
this mono behaviour to any gameobject it will automatically create an entity in a desired pool and populate and apply
the `EntityBinding` component, as well as setting up a `ViewComponent` on the underlying entity tying it back to the GameObject
in the scene.