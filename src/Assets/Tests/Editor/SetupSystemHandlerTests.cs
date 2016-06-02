using EcsRx.Entities;
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
    public class SetupSystemHandlerTests
    {
        [Test]
        public void should_return_valid_subscription_token_when_processing()
        {
            var mockEnity = Substitute.For<IEntity>();
            var mockPoolManager = Substitute.For<IPoolManager>();
            var fakeGroupAccessor = new GroupAccessor(null, new [] {mockEnity});
            mockPoolManager.CreateGroupAccessor(Arg.Any<IGroup>()).Returns(fakeGroupAccessor);
            var mockSystem = Substitute.For<ISetupSystem>();
            mockSystem.TargetGroup.TargettedEntities.Returns(x => null);

            var handler = new SetupSystemHandler(mockPoolManager);
            handler.Setup(mockSystem);

            mockSystem.Received().Setup(mockEnity);
        }
    }
}