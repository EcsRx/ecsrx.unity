using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using EcsRx.Systems.Executor.Handlers;
using NSubstitute;
using NUnit.Framework;
using UniRx;

namespace EcsRx.Tests
{
    [TestFixture]
    public class ReactToGroupSystemHandlerTests
    {
        [Test]
        public void should_return_valid_subscription_token_when_processing()
        {
            var mockPoolManager = Substitute.For<IPoolManager>();
            var mockSystem = Substitute.For<IReactToGroupSystem>();
            var mockSubscription = Substitute.For<IObservable<GroupAccessor>>();
            mockSystem.ReactToGroup(Arg.Any<GroupAccessor>()).Returns(mockSubscription);

            var handler = new ReactToGroupSystemHandler(mockPoolManager);
            var subscriptionToken = handler.Setup(mockSystem);

            Assert.That(subscriptionToken, Is.Not.Null);
            Assert.That(subscriptionToken.AssociatedObject, Is.Null);
            Assert.That(subscriptionToken.Disposable, Is.Not.Null);
        }
    }
}