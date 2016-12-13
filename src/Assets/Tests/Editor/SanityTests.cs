using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Pools.Identifiers;
using EcsRx.Systems.Executor;
using EcsRx.Systems.Executor.Handlers;
using EcsRx.Tests.Components;
using EcsRx.Tests.Systems;
using NUnit.Framework;
using UniRx;

namespace EcsRx.Tests
{
    [TestFixture]
    public class SanityTests
    {
        private SystemExecutor CreateExecutor()
        {
            var messageBroker = new EventSystem(new MessageBroker());
            var identityGenerator = new SequentialIdentityGenerator();
            var entityFactory = new DefaultEntityFactory(identityGenerator, messageBroker);
            var poolFactory = new DefaultPoolFactory(entityFactory, messageBroker);
            var groupAccessorFactory = new DefaultGroupAccessorFactory(messageBroker);
            var poolManager = new PoolManager(messageBroker, poolFactory, groupAccessorFactory);
            var reactsToEntityHandler = new ReactToEntitySystemHandler(poolManager);
            var reactsToGroupHandler = new ReactToGroupSystemHandler(poolManager);
            var reactsToDataHandler = new ReactToDataSystemHandler(poolManager);
            var manualSystemHandler = new ManualSystemHandler(poolManager);
            var setupHandler = new SetupSystemHandler(poolManager);
            return new SystemExecutor(poolManager, messageBroker, reactsToEntityHandler, 
                reactsToGroupHandler, setupHandler, reactsToDataHandler, manualSystemHandler);
        }

        [Test]
        public void should_execute_setup_for_matching_entities()
        {
            var executor = CreateExecutor();
            executor.AddSystem(new TestSetupSystem());

            var defaultPool = executor.PoolManager.GetPool();
            var entityOne = defaultPool.CreateEntity();
            var entityTwo = defaultPool.CreateEntity();

            entityOne.AddComponent(new TestComponentOne());
            entityTwo.AddComponent(new TestComponentTwo());

            Assert.That(entityOne.GetComponent<TestComponentOne>().Data, Is.EqualTo("woop"));
            Assert.That(entityTwo.GetComponent<TestComponentTwo>().Data, Is.Null);
        }
    }
}