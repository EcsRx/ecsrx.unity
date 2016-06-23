namespace EcsRx.Systems.Executor.Handlers
{
    public interface IManualSystemHandler
    {
        void Start(IManualSystem system);
        void Stop(IManualSystem system);
    }
}