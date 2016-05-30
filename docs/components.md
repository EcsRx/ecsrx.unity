# Components

Components are the data containers in the ECS world and should only contain some properties to contain data, there should be no logic in components, if there is logic then you are probably a bad man and need to step away from the computer.

## Using components

You will need to make your own implementations of `IComponent` which encapsulate the data your components need to expose to the systems. It is fairly simple really just implement `IComponent` and it just does its stuff.

One thing to remember though as we are using *UniRx* you can use `ReactiveProperty<T>` if you want to have reactive properties, or just use normal value types etc, it allows you to choose if you want have reactive properties or not.

## Composition

So the whole point behind components are that they can contain anything, so if you need to contain complex data it would make sense to go write a c# POCO somewhere to encapsulate your data then just include that via composition inside your component. This lets you consume 3rd party content from unity or other libraries in your components without too much effort.