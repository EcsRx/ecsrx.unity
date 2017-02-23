using EcsRx.Persistence.Data;

namespace EcsRx.Persistence.Persisters
{
    public interface IDataPersister
    {
        void PersistData(ApplicationData data);
    }
}