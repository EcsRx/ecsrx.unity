using EcsRx.Pools;
using EcsRx.Pools.Identifiers;
using EcsRx.Systems.Executor;
using EcsRx.Systems.Executor.Handlers;
using EcsRx.Tests.Components;
using EcsRx.Tests.Systems;
using NUnit.Framework;

namespace EcsRx.Tests
{
    [TestFixture]
    public class SanityTests
    {
        private SystemExecutor CreateExecutor()
        {
            var identityGenerator = new SequentialIdentityGenerator();
            var poolManager = new PoolManager(identityGenerator);
            var reactsToEntityHandler = new ReactToEntitySystemHandler(poolManager);
            var reactsToGroupHandler = new ReactToGroupSystemHandler(poolManager);
            var reactsToDataHandler = new ReactToDataSystemHandler(poolManager);
            var setupHandler = new SetupSystemHandler(poolManager);
            return new SystemExecutor(poolManager, reactsToEntityHandler, reactsToGroupHandler, setupHandler, reactsToDataHandler);
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
            Assert.That(entityTwo.GetComponent<TestComponentTwo>().Data, Is.Empty);
        }
    }
}