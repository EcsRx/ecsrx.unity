using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Zenject;
using System.Linq;

namespace Zenject.Asteroids
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        Settings _settings = null;

        public override void InstallBindings()
        {
            // In this example there is only one 'installer' but in larger projects you
            // will likely end up with many different re-usable installers
            // that you'll want to use in several different scenes
            //
            // There are several ways to do this.  You can store your installer as a prefab,
            // a scriptable object, a component within the scene, etc.  Or, if you don't
            // need your installer to be a MonoBehaviour then you can just simply call
            // Container.Install
            //
            // See here for more details:
            // https://github.com/modesttree/zenject#installers
            //
            //Container.Install<MyOtherInstaller>();

            // Install the main game
            InstallAsteroids();
            InstallShip();
            InstallMisc();
            InitExecutionOrder();
        }

        void InstallAsteroids()
        {
            // ITickable, IFixedTickable, IInitializable and IDisposable are special Zenject interfaces.
            // Binding a class to any of these interfaces creates an instance of the class at startup.
            // Binding to any of these interfaces is also necessary to have the method defined in that interface be
            // called on the implementing class as follows:
            // Binding to ITickable or IFixedTickable will result in Tick() or FixedTick() being called like Update() or FixedUpdate().
            // Binding to IInitializable means that Initialize() will be called on startup.
            // Binding to IDisposable means that Dispose() will be called when the app closes, the scene changes,
            // or the composition root object is destroyed.

            // Any time you use To<Foo>().AsSingle, what that means is that the DiContainer will only ever instantiate
            // one instance of the type given inside the To<> (in this example, Foo). So in this case, any classes that take ITickable,
            // IFixedTickable, or AsteroidManager as inputs will receive the same instance of AsteroidManager.
            // We create multiple bindings for ITickable, so any dependencies that reference this type must be lists of ITickable.
            Container.Bind<ITickable>().To<AsteroidManager>().AsSingle();
            Container.Bind<IFixedTickable>().To<AsteroidManager>().AsSingle();
            Container.Bind<AsteroidManager>().AsSingle();

            // The above three lines are also identical to just doing this instead:
            // Container.BindInterfacesAndSelfTo<AsteroidManager>();

            // Here, we're defining a generic factory to create asteroid objects using the given prefab
            // So any classes that want to create new asteroid objects can simply include an injected field
            // or constructor parameter of type Asteroid.Factory, then call Create() on that
            Container.BindFactory<Asteroid, Asteroid.Factory>()
                .FromComponentInNewPrefab(_settings.AsteroidPrefab)
                // We can also tell Zenject what to name the new gameobject here
                .WithGameObjectName("Asteroid")
                // GameObjectGroup's are just game objects used for organization
                // This is nice so that it doesn't clutter up our scene hierarchy
                .UnderTransformGroup("Asteroids");
        }

        void InstallMisc()
        {
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            Container.Bind<LevelHelper>().AsSingle();

            Container.BindInterfacesTo<AudioHandler>().AsSingle();

            Container.Bind<ExplosionFactory>().AsSingle().WithArguments(_settings.ExplosionPrefab);
            Container.Bind<BrokenShipFactory>().AsSingle().WithArguments(_settings.BrokenShipPrefab);
        }

        void InstallShip()
        {
            Container.DeclareSignal<ShipCrashedSignal>();

            Container.Bind<ShipStateFactory>().AsSingle();

            // Note that the ship itself is bound using a ZenjectBinding component (see Ship
            // game object in scene heirarchy)

            Container.BindFactory<ShipStateWaitingToStart, ShipStateWaitingToStart.Factory>().WhenInjectedInto<ShipStateFactory>();
            Container.BindFactory<ShipStateDead, ShipStateDead.Factory>().WhenInjectedInto<ShipStateFactory>();
            Container.BindFactory<ShipStateMoving, ShipStateMoving.Factory>().WhenInjectedInto<ShipStateFactory>();
        }

        void InitExecutionOrder()
        {
            // In many cases you don't need to worry about execution order,
            // however sometimes it can be important
            // If for example we wanted to ensure that AsteroidManager.Initialize
            // always gets called before GameController.Initialize (and similarly for Tick)
            // Then we could do the following:
            Container.BindExecutionOrder<AsteroidManager>(-10);
            Container.BindExecutionOrder<GameController>(-20);

            // Note that they will be disposed of in the reverse order given here
        }

        [Serializable]
        public class Settings
        {
            public GameObject ExplosionPrefab;
            public GameObject BrokenShipPrefab;
            public GameObject AsteroidPrefab;
            public GameObject ShipPrefab;
        }
    }
}
