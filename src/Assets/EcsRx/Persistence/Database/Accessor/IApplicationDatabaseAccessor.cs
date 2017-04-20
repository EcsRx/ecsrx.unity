using System;

namespace EcsRx.Persistence.Database.Accessor
{
    public interface IApplicationDatabaseAccessor
    {
        ApplicationDatabase Database { get; }
        void ReloadDatabase(Action onRefreshed);
        void PersistDatabase(Action onSaved);
    }
}