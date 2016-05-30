using System.Linq;
using EcsRx.Pools;
using EcsRx.Pools.Identifiers;
using NSubstitute;
using NUnit.Framework;

namespace EcsRx.Tests
{
    [TestFixture]
    public class PoolTests
    {
        [Test]
        public void should_create_new_entity()
        {
            var expectedId = 123456;
            var mockIdentityGenerator = Substitute.For<IIdentifyGenerator>();
            mockIdentityGenerator.GenerateId().Returns(expectedId);

            var pool = new Pool("", mockIdentityGenerator);
            var entity = pool.CreateEntity();

            Assert.That(entity.Id, Is.EqualTo(expectedId));
            Assert.That(entity.Components, Is.Not.Null);
            Assert.That(entity.Components, Is.Empty);
        }

        [Test]
        public void should_raise_event_when_creating_entity()
        {
            var mockIdentityGenerator = Substitute.For<IIdentifyGenerator>();

            var hasRaised = false;
            var pool = new Pool("", mockIdentityGenerator);
            pool.OnEntityAdded += (sender, args) => hasRaised = true;
            pool.CreateEntity();

            Assert.IsTrue(hasRaised);
        }

        [Test]
        public void should_add_created_entity_into_the_pool()
        {
            var mockIdentityGenerator = Substitute.For<IIdentifyGenerator>();
            
            var pool = new Pool("", mockIdentityGenerator);
            var entity = pool.CreateEntity();

            Assert.That(pool.Entities.Count(), Is.EqualTo(1));
            Assert.That(pool.Entities.First(), Is.EqualTo(entity));
        }

        [Test]
        public void should_raise_event_when_removing_entity()
        {
            var mockIdentityGenerator = Substitute.For<IIdentifyGenerator>();

            var hasRaised = false;
            var pool = new Pool("", mockIdentityGenerator);
            pool.OnEntityRemoved += (sender, args) => hasRaised = true;
            var entity = pool.CreateEntity();
            pool.RemoveEntity(entity);

            Assert.IsTrue(hasRaised);
        }

        [Test]
        public void should_remove_created_entity_from_the_pool()
        {
            var mockIdentityGenerator = Substitute.For<IIdentifyGenerator>();

            var pool = new Pool("", mockIdentityGenerator);
            var entity = pool.CreateEntity();
            pool.RemoveEntity(entity);

            Assert.That(pool.Entities, Is.Empty);
        }
    }
}