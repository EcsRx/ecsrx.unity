# FAQs

There are alot of FAQs discussed on the [EcsRx Core FAQs page](https://github.com/EcsRx/ecsrx/blob/master/docs/faqs-etc.md).

## Unity Specific Queries

### In Unity how can I use it with playmaker/visual coding tool etc

This is more geared for coders, you can easily communicate from playmaker or any other tool into the underlying *Systems* etc you would just need to write your own custom actions to bridge between this framework and the one you are consuming. If you are one of the people wanting this functionality then I am happy to assist you as I have written a lot of other libraries bridging frameworks in the unity world, especially Node Canvas.

### Why cant entities be GameObjects and components be MonoBehaviours?

If you like that style go check out uFrame ECS as that is built around that concept.

However assuming you want more of an explanation as to why, then there are many reasons, such as if you have a dependency on unity's objects it is impossible to do any mockable tests (unit/integration tests), so you can only test inside active scenes which are slow and cumbersome. They are also quite slow to work with, so to instantiate a new Entity is trivial, however to instantiate a new GameObject is not so trivial, then also MonoBehaviours have some overhead, whereas a component within this system is just a POCO so it generally makes things slicker and quicker while having a more decoupled framework.

There is also one other point worth making here and that is why do you need an in-scene representation of every entity? some entities may only live as data without a physical embodiment, or some entities may only need a physical object when they are within a certain distance of the player. Take for example an RTS game or RPG game where you have lots of NPC/Units moving around the game which you dont really care about rendering or doing anything in the scene with, these entities can easily live on in memory with little overhead still reacting to things without much of an overhead, but the moment you force everything to be a game object you automatically need to worry about this and disabling components when it is too far away etc.

#### Half way house on this approach

Now lets say you know all the above, and you want to make sure every entity exists in the scene with a game object, well its quite simple to achieve.

Make a component like so (unity scenario):

```
public class ViewComponent : IComponent
{
    public string PrefabType { get; set; } // optional
    public GameObject GameObject { get; set; }
}
```

Make sure every entity has this applied, then make a system to automatically create the object/prefab when this component exists, then make an extension method like so:

```
public static IEntityExtensions
{
    public static GetGameObject(this IEnity entity)
    {
        return entity.GetComponet<ViewComponent>().GameObject;
    }
}
```

This way if you make every entity have this component you can then easily just have `entity.GetGameObject()` to get the unity world stuffs, making it slightly easier to work with the unity world..

There is even a views extension which is provided which has a barebones implementation for views so it can be built upon for each engine, such as Unity, Godot, Monogame etc.