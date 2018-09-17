# Recommended Structure & Conventions

It is completely up to you how you layout your project but there are some recommended conventions that you follow to begin with and if you decide you want to do things another way, you are completely free to do so.
 
 However EcsRx comes with a raft of premade infrastructure helpers and conventions, so if you are happy to follow these basic structures then you will get a lot of infrastructure for free and will hopefully find your game more manageable.

## Recommended Project Structure

It is recommended that in your `Assets` folder you add a `Game` folder. You can call it something else if you want, the name doesn't matter, but it isolates all your `Game` related infrastructure from your other plugins/assets etc.

Then within there its recommended you make another folder for each `Application` you will have within your game. Which would look something like:

```
|- Assets
    |- Game
        |- MainMenu
        |- InGame
        |- CharacterCreator
        |- SomeMiniGame
        |- ...
    |- ...
```

So as you can see here we have the main unity `Assets` folder, where we carve out a `Game` folder to contain all our applications, then within there we have several applications, which can be seen as individual isolated parts of your game.

> Just to be clear a Scene != Application in this instance, as you could have a main menu which actually is made up of 3 additive scenes, but they all share the same application. You will ALWAYS have one parent scene and its recommended that you use that to load in your application and have other SceneContexts share the same contract names.

### Inside the application folders

So within the application folders you would generally have a layout like so:

```
|- MainMenu
    |- Blueprints
    |- Components
    |- Events
    |- Groups
    |- Computeds
    |- Scenes
    |- Systems
    |- ViewResolvers
    |- MainMenuApplication.cs
```

This then gives you places to put all your custom classes and scenes, most of the folder names are self explanatory, i.e you put your component implementations into your `Components` folder, your blueprints in the `Blueprints` folder etc, this way the aspect of your game is self contained and easy to work with.

As you can see the application sits in the root of your folder here, as it should be the entry point for your `MainMenu`, the parent scene should load this with a SceneContext and then all other scenes that are additively loaded should use this SceneContext as a parent.

### Conventions for application folders

If you have setup your game in the manner explained above you can make use of some of the system helper resolvers, which will automatically register your application scoped systems based off their folder names.

In most of the examples within this repository you can see there is often the following calls:

```c#
public class Application : EcsRxApplicationBehaviour
{        
    protected override void ApplicationStarting()
    {
        // IMPORTANT BITS HERE
        this.BindAllSystemsWithinApplicationScope();
        this.RegisterAllBoundSystems();
    }

    protected override void ApplicationStarted()
    {
        // ...
    }
}
```

As you can see there are 2 helpers used here which cut down a huge amount of boilerplate binding and registration code. The helpers are often split into **Binding** helpers and **Registration** helpers. The binding ones will tell the DI container about systems so they can be resolved with dependencies, the registration helpers scan the DI container for implementations and auto register them so they can be used by the application.

> This is another important reason why we isolate the folders in this way, as you may not want all systems registered and running at once, so this way only the systems needed for your application are bound, registered and then run.

#### `BindAllSystemsWithinApplicationScope()`

This helper method looks at the namespace of your application and then looks within that namespace for `Systems` and `ViewResolvers` namespaces, it then will get all implementations of Systems within those folders and automatically bind them to the DI container so they can be registered later.

#### `RegisterAllBoundSystems()`

This helper can be used in almost any convention and will get all system implementations out of the dependency container and will start registering them with the `SystemExecutor` so they will be auto hooked up.

### Other helpful conventions

There are some other helpers, so if you decide you want to use other namespaces or conventions you can easily use existing helpers or build off them, so for example `BindAllSystemsWithinApplicationScope` helper method is just a wrapper around `BindAnySystemsInNamespaces` helper, as shown below:

```c#
// Bind anything within the given namespaces
public static void BindAnySystemsInNamespace(this EcsRxApplicationBehaviour application, params string[] namespaces)
{
    BindSystemsInNamespace.Bind(application.DependencyContainer.NativeContainer as DiContainer, namespaces);
}

// Bind anything in Systems/ViewResolvers folders
public static void BindAllSystemsWithinApplicationScope(this EcsRxApplicationBehaviour application)
{
    var applicationNamespace = application.GetType().Namespace;
    var namespaces = new[]
    {
        $"{applicationNamespace}.Systems",
        $"{applicationNamespace}.ViewResolvers"
    };
    
    BindSystemsInNamespace.Bind(application.DependencyContainer.NativeContainer as DiContainer, namespaces);
}
```

So you could easily split your folders up further if you wished or move your applications around and generate the namespaces yourself, or manually just provide namespaces and it would auto bind those systems for you.

> You do not need to use any of this stuff, if you want you can manually bind/register or even ignore binding and just register your system manually with the `SystemExecutor`, but if you just want to change your folder structure the helpers above will greatly assist with this, save you re-inventing the wheel.

There are also some installers you can use which do the same sort of thing if you are a fan of scene based setup (I would advise against it), which would be the `AutoBindSystemsInstaller` and the `BindSystemsInNamespaceInstaller`.

> It is also worth noting that these extensions and installers are all tied to the Zenject flavour of EcsRx, if you wish to use ADIC or StrangeIoC etc you would need to make your own conventions in there.

Finally there is the registration helpers, which often wont be needed as you will generally do the following:

1. Setup all `System` bindings in DI container
2. Resolve and register all bound `Systems`

So in most cases all you will need to do is call `RegisterAllBoundSystems()` but if you want a more fine grained control you can always use `BindAndRegisterSystem<T>()` which will bind the specific system for you, then automatically register it straight afterwards.
 
 There is finally `RegisterSystem<T>` which will not do the binding but will resolve and register the system type directly, this can be helpful if you are wanting to manually control when systems are added but still want to make use of the DI system.

## I DONT LIKE YOUR CONVENTIONS, I WANT MY OWN!

That's fine, unless you are going to re-write the innards, you will always have `ISystem` implementations and an `ISystemExecutor` which takes systems and manages their lifecycle and resolving entities for them to use.

If you just work of that foundation you can easily wrap up the binding/registration in any way you want. The framework has tried to be as extensible and flexible as possible without giving too many options to the developer. So as long as you give the system executor its systems, be it through the pre-made extension methods or your own, no one will really care. 

## Some other blurbs

As mentioned it can be beneficial to work out the parts of your game which can work in isolation and have an application per section, however if your game is not that complicated you can just use a single application and just use that as the brain for everything.

There are no rules that force you down on avenue or another, if you don't like the application behaviour you are free to make your own, all it does is bootstrap the `SystemExecutor` and some other helpers and wrap the underlying native DI container, so if you do not need any of this have a look at the docs in the core `EcsRx` project on how to setup manually, as you can do all this without dependency injection and the conventions listed above... just make sure if you go down this path you do it for a good reason.