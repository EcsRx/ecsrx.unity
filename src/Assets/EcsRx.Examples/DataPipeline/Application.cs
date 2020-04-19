using System;
using System.Threading.Tasks;
using EcsRx.Examples.DataPipeline.Components;
using EcsRx.Extensions;
using EcsRx.Plugins.Persistence;
using EcsRx.Plugins.Persistence.Pipelines;
using EcsRx.Zenject;
using LazyData;
using Persistity.Wiretap.Extensions;
using UnityEngine;
using Zenject;

public class Application : EcsRxApplicationBehaviour
{
    [Inject]
    private ISaveEntityDatabasePipeline _exportPipeline;

    protected override void LoadPlugins()
    {
        base.LoadPlugins();
        RegisterPlugin(new PersistencePlugin());
    }

    protected override void StartSystems()
    {
        // Dont need systems
    }

    protected override void ApplicationStarted()
    {
        var entityCollection = EntityDatabase.GetCollection();

        var dummyEntity = entityCollection.CreateEntity();
        var dataComponent = dummyEntity.AddComponent<DataComponent>();
        dataComponent.Age = 20;
        dataComponent.Name = "Testy McTest Face";

        Task.Run(ExportDatabase);
    }

    protected async Task ExportDatabase()
    {
        var tappedExporter = _exportPipeline.AsWireTappable();
        tappedExporter.StartWiretap(2, async (data, state) =>
        {
            var binaryDataOject =  (DataObject) data;
            var binaryString = BitConverter.ToString(binaryDataOject.AsBytes);
            Debug.Log($"Bytes Serialized: {binaryString}");
        });
        
        await tappedExporter.Execute(EntityDatabase);
    }
}
