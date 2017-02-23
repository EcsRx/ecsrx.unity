using System;
using System.Collections.Generic;
using System.Linq;
using Assets.EcsRx.Persistence.Helpers;
using EcsRx.Components;
using EcsRx.Json;
using EcsRx.Persistence.Data;

namespace EcsRx.Persistence.Transformers
{
    public class JsonTransformer : ITransformer<JSONNode>
    {
        public readonly string EntityIdKey = "entityId";
        public readonly string DataKey = "data";

        public ComponentTypeHelper ComponentTypeHelper { get; private set; }

        public JsonTransformer(ComponentTypeHelper componentTypeHelper)
        {
            ComponentTypeHelper = componentTypeHelper;
        }

        public JSONNode Transform(ApplicationData applicationData)
        {
            var rootNode = new JSONClass();

            foreach (var entityData in applicationData.EntityData)
            {
                var entityNode = TransformEntity(entityData);
                rootNode.Add(entityNode);
            }

            return rootNode;
        }

        public JSONNode TransformEntity(EntityData entityData)
        {
            var entityNode = new JSONClass();

            entityNode.Add(EntityIdKey, new JSONData(entityData.EntityId.ToString()));
            var entityDataNode = new JSONClass();
            entityNode.Add(DataKey, entityDataNode);

            foreach (var dataValue in entityData.Data)
            {
                entityDataNode.Add(dataValue.Key, new JSONData(dataValue.Value.ToString()));
            }

            return entityNode;
        }

        public EntityData TransformEntity(JSONNode entityJson)
        {
            var entityData = new EntityData();
            entityData.EntityId = new Guid(entityJson[EntityIdKey].Value);

            foreach (KeyValuePair<string, JSONNode> dataJson in entityJson[DataKey].AsObject)
            {
                entityData.Data.Add(dataJson.Key, dataJson.Value.Value);
            }

            return entityData;
        }

        private Type FindComponentFromKey(string key)
        {
            var keySections = key.Split('.');
            var componentName = keySections[0];
            var componentType = ComponentTypeHelper.GetComponentFromName(componentName);
            
            if(componentType == null)
            { throw new Exception("Cannot locate component named [" + componentName + "] from key [" + key + "]"); }

            return componentType;
        }
    }
}