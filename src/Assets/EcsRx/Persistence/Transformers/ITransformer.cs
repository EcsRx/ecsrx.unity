using EcsRx.Persistence.Data;

namespace EcsRx.Persistence.Transformers
{
    public interface ITransformer<T>
    {
        T Transform(ApplicationData applicationData);
    }
}