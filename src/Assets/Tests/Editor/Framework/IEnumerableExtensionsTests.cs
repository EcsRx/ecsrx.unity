using System.Collections.Generic;
using System.Linq;
using EcsRx.Extensions;
using EcsRx.Systems;
using EcsRx.Tests.Systems;
using NUnit.Framework;

namespace EcsRx.Tests
{
    [TestFixture]
    public class IEnumerableExtensionsTests
    {
        [Test]
        public void should_correctly_order_priorities()
        {
            var defaultPrioritySystem = new DefaultPrioritySystem();
            var higherThanDefaultPrioritySystem = new HigherThanDefaultPrioritySystem();
            var lowerThanDefaultPrioritySystem = new LowerThanDefaultPrioritySystem();
            var lowPrioritySystem = new LowPrioritySystem();
            var highPrioritySystem = new HighPrioritySystem();

            var systemList = new List<ISystem>
            {
                defaultPrioritySystem,
                higherThanDefaultPrioritySystem,
                lowerThanDefaultPrioritySystem,
                lowPrioritySystem,
                highPrioritySystem
            };

            var orderedList = systemList.OrderByPriority().ToList();
            Assert.That(orderedList, Has.Count.EqualTo(5));
            Assert.That(orderedList[0], Is.EqualTo(highPrioritySystem));
            Assert.That(orderedList[1], Is.EqualTo(higherThanDefaultPrioritySystem));
            Assert.That(orderedList[2], Is.EqualTo(defaultPrioritySystem));
            Assert.That(orderedList[3], Is.EqualTo(lowerThanDefaultPrioritySystem));
            Assert.That(orderedList[4], Is.EqualTo(lowPrioritySystem));
        }
    }
}