# EcsRx

[![Join the chat at https://gitter.im/grofit/ecsrx](https://badges.gitter.im/grofit/ecsrx.svg)](https://gitter.im/grofit/ecsrx?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

EcsRx is a reactive take on the common ECS pattern with a well separated design and support for dependency injection (if you want it).

## Dependencies

- UniRx (All)
- Zenject (Unity Bridge)

The core framework only depends upon UniRx however the unity bridge part of the framework depends upon zenject, however feel free to create your own unity bridge to consume the core framework if you do not want the dependency.

## Installation

You can take the unitypackage installation file from the dist folder, there are 2 files.

- EcsRx.Core.unitypackage
- EcsRx.Unity.unitypackage

The **Core** package contains the pure project files which are required for the framework to function.

The **Unity** package contains a wrapper around the **Core** framework and some unity helpers, this will be built upon going forward.

## Quick Start

Install both the above packages, install Zenject, install UniRx then assuming you are using the unity bridge project just look at one of the example projects and follow the conventions in there. You will ultimately need to create a `SceneContext` from *Zenject* then register the `DefaultEcsRxInstaller` and any of your own installer classes. Then create an your own implementation of `EcsRxContainer` where you can setup your systems and entities.

Much like any other ECS implementation you have the notion of entities (`IEntity`), components (`IComponent`) and systems (`ISystem`), although there are a few types of systems which you can implement based upon your needs, check out the docs folder for more information on these subjects as its more than just a 1 liner.

## Running Examples

If you want to run the examples then just clone it and open the unity project in the `src` folder, then run the examples, I will try to add to as the library matures, but there are 2 examples there currently.

There are also a suite of tests which are being expanded as the project grows, it was written with testability in mind.

## Docs

See the docs folder for more information. (This will grow)

## HEADS UP

This was not designed with performance in mind, it should be performant enough for most scenarios, and given its reactive nature and decoupled design you can easily replace implementations at will, but as lots of people love performance metrics etc, I have none and have put performance secondary to functionality.
