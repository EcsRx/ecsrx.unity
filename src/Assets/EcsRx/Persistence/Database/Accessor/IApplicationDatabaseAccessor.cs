using System;

namespace EcsRx.Persistence.Database.Accessor
{
    public interface IApplicationDatabaseAccessor
    {
        ApplicationDatabase Database { get; }
        bool HasInitialized { get; }

        void ReloadDatabase(Action onRefreshed);
        void PersistDatabase(Action onSaved);
        void ResetDatabase();
    }
}