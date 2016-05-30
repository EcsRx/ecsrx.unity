using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Tests.Components;
using NSubstitute;
using NUnit.Framework;

namespace EcsRx.Tests
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void should_correctly_add_component()
        {
            var entity = new Entity(1);
            var dummyComponent = Substitute.For<IComponent>();

            entity.AddComponent(dummyComponent);

            Assert.That(entity.Components.Count(), Is.EqualTo(1));
        }

        [Test]
        public void should_correctly_raise_event_when_adding_component()
        {
            var entity = new Entity(1);
            var dummyComponent = Substitute.For<IComponent>();

            var wasCalled = false;
            entity.OnComponentAdded += (o, e) => wasCalled = true;
            entity.AddComponent(dummyComponent);

            Assert.That(wasCalled);
        }

        [Test]
        public void should_correctly_remove_component()
        {
            var entity = new Entity(1);
            var dummyComponent = Substitute.For<IComponent>();
            entity.AddComponent(dummyComponent);
            
            entity.RemoveComponent(dummyComponent);

            Assert.That(entity.Components, Is.Empty);
        }

        [Test]
        public void should_correctly_raise_event_when_removing_component()
        {
            var entity = new Entity(1);
            var dummyComponent = Substitute.For<IComponent>();
            entity.AddComponent(dummyComponent);

            var wasCalled = false;
            entity.OnComponentRemoved += (o, e) => wasCalled = true;
            entity.RemoveComponent(dummyComponent);

            Assert.That(wasCalled);
        }

        [Test]
        public void should_return_true_when_entity_has_component()
        {
            var entity = new Entity(1);
            var dummyComponent = new TestComponentOne();
            entity.AddComponent(dummyComponent);

            Assert.That(entity.HasComponent<TestComponentOne>());
        }

        [Test]
        public void should_return_true_when_entity_matches_all_components()
        {
            var entity = new Entity(1);
            entity.AddComponent(new TestComponentOne());
            entity.AddComponent(new TestComponentTwo());

            Assert.That(entity.HasComponents(typeof(TestComponentOne), typeof(TestComponentTwo)));
        }

        [Test]
        public void should_return_false_when_entity_does_not_match_all_components()
        {
            var entity = new Entity(1);
            entity.AddComponent(new TestComponentOne());

            Assert.IsFalse(entity.HasComponents(typeof(TestComponentOne), typeof(TestComponentTwo)));
        }

        [Test]
        public void should_return_false_when_entity_is_missing_component()
        {
            var entity = new Entity(1);

            Assert.IsFalse(entity.HasComponent<TestComponentOne>());
        }

        [Test]
        public void should_throw_error_when_adding_component_that_already_exists()
        {
            var entity = new Entity(1);
            var dummyComponent = Substitute.For<IComponent>();
            entity.AddComponent(dummyComponent);

            Assert.Throws<ArgumentException>(() => entity.AddComponent(dummyComponent));
        }

        [Test]
        public void should_not_throw_error_when_removing_non_existent_component_with_generic()
        {
            var entity = new Entity(1);
            entity.RemoveComponent<TestComponentOne>();
        }

        [Test]
        public void should_not_throw_error_when_removing_non_existent_component_with_instance()
        {
            var entity = new Entity(1);
            var dummyComponent = new TestComponentOne();

            entity.RemoveComponent(dummyComponent);
        }
    }
}
