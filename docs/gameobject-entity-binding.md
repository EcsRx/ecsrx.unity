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

## Linking to existing GameObjects

Since version 0.6.0 there was some helpers added here to allow you to link existing entities to existing game objects, this is for when you cannot create the view in code, or when a game object is created by a 3rd party but you still want to be able to hook an entity onto it. So the actual code is `myGameObject.LinkEntity(entityToLink, poolEntityIsPartOf)`, this is shorthand for:

- Add `EntityView` MonoBehaviour to the GameObject instance
- Add `ViewComponent` to the Entity if it doesnt already have it
- Set the `View` in the `ViewComponent` to be the GameObject being bound

This makes it simpler to just link entities and game objects together in a more succinct way, again it is advised to generate GameObjects as Views via code but in some cases there will be a need for this sort of approach, so it is now supported.

## Helpers

There are also 2 extensions to `IEntity` which provides `GetComponent<T>` and `AddComponent<T>` where `T` is a `MonoBehavior`, so this basically lets you treat the entity instance like its a GameObject to some degree. It will only work though if you have a `ViewComponent` on the entity with a valid `View` in place. Assuming you have those bits in place you can have the entity do things like:

```
var rigidBody = myEntity.AddComponent<RigidBody>(); // The underlying GO now has a RigidBody
```

A note on this though that currently I am not 100% sure if I like the signature being almost the same as the `IComponent` `AddComponent` and `GetComponent` methods, so although code will become more succinct it may lead people to think that you are getting an `IComponent` where you really are getting an MB back, so this may change going forward.
