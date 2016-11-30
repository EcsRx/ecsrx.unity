using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using EcsRx.Systems.Executor;
using EcsRx.Systems.Executor.Handlers;
using EcsRx.Tests.Components;
using NSubstitute;
using NUnit.Framework;
using UniRx;

namespace EcsRx.Tests
{
    [TestFixture]
    public class SystemExecutorTests
    {
        [Test]
        public void should_identify_as_setup_system_and_add_to_systems()
        {
            var mockPoolManager = Substitute.For<IPoolManager>();
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockSetupSystemHandler = Substitute.For<ISetupSystemHandler>();
            var fakeSystem = Substitute.For<ISetupSystem>();

            var systemExecutor = new SystemExecutor(mockPoolManager, mockEventSystem,
                null, null, mockSetupSystemHandler, null);

            systemExecutor.AddSystem(fakeSystem);

            mockSetupSystemHandler.Received().Setup(fakeSystem);
            Assert.That(systemExecutor.Systems, Contains.Item(fakeSystem));
        }

        [Test]
        public void should_identify_as_reactive_entity_system_and_add_to_systems()
        {
            var mockPoolManager = Substitute.For<IPoolManager>();
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockReactToEntitySystemHandler = Substitute.For<IReactToEntitySystemHandler>();
            var fakeSystem = Substitute.For<IReactToEntitySystem>();

            var systemExecutor = new SystemExecutor(mockPoolManager, mockEventSystem,
                mockReactToEntitySystemHandler, null, null, null);

            systemExecutor.AddSystem(fakeSystem);

            mockReactToEntitySystemHandler.Received().Setup(fakeSystem);
            Assert.That(systemExecutor.Systems, Contains.Item(fakeSystem));
        }

        [Test]
        public void should_identify_as_reactive_group_system_and_add_to_systems()
        {
            var mockPoolManager = Substitute.For<IPoolManager>();
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockReactToGroupSystemHandler = Substitute.For<IReactToGroupSystemHandler>();
            var fakeSystem = Substitute.For<IReactToGroupSystem>();

            var systemExecutor = new SystemExecutor(mockPoolManager, mockEventSystem,
                null, mockReactToGroupSystemHandler, null, null);

            systemExecutor.AddSystem(fakeSystem);

            mockReactToGroupSystemHandler.Received().Setup(fakeSystem);
            Assert.That(systemExecutor.Systems, Contains.Item(fakeSystem));
        }

        [Test]
        public void should_remove_system_from_systems()
        {
            var mockPoolManager = Substitute.For<IPoolManager>();
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockSetupSystemHandler = Substitute.For<ISetupSystemHandler>();
            var fakeSystem = Substitute.For<ISetupSystem>();

            var systemExecutor = new SystemExecutor(mockPoolManager, mockEventSystem,
                null, null, mockSetupSystemHandler, null);

            systemExecutor.AddSystem(fakeSystem);
            systemExecutor.RemoveSystem(fakeSystem);

            Assert.That(systemExecutor.Systems, Is.Empty);
        }


        [Test]
        public void should_effect_correct_setup_systems_once()
        {
            var dummyGroup = new Group(typeof(TestComponentOne), typeof(TestComponentTwo));
            var mockPoolManager = Substitute.For<IPoolManager>();
            var mockEventSystem = Substitute.For<IEventSystem>();
            var mockSetupSystemHandler = Substitute.For<ISetupSystemHandler>();
            var fakeSystem = Substitute.For<ISetupSystem>();
            fakeSystem.TargetGroup.Returns(dummyGroup);

            var systemExecutor = new SystemExecutor(mockPoolManager, mockEventSystem,
                null, null, mockSetupSystemHandler, null);

            systemExecutor.AddSystem(fakeSystem);

            var entity = new Entity(1, mockEventSystem);
            entity.AddComponent(new TestComponentOne());
            systemExecutor.OnEntityComponentAdded(new ComponentAddedEvent(entity, new TestComponentOne()));
            
            entity.AddComponent(new TestComponentTwo());
            systemExecutor.OnEntityComponentAdded(new ComponentAddedEvent(entity, new TestComponentTwo()));

            entity.AddComponent(new TestComponentThree());
            systemExecutor.OnEntityComponentAdded(new ComponentAddedEvent(entity, new TestComponentThree()));

            mockSetupSystemHandler.Received(1).ProcessEntity(Arg.Is(fakeSystem), Arg.Is(entity));
        }
    }
}