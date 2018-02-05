using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Unity.Components;

namespace EcsRx.UI
{
    public class DefaultUIBlueprint : IBlueprint
    {
        private readonly string uiName;
        private UIType type;
        private bool model;

        public DefaultUIBlueprint(string ui, UIType type)
        {
            uiName = ui;
            this.type = type;
        }
        public void Apply(IEntity entity)
        {
            var uiComponent = new UIComponet {UIName = uiName, IsDynamic = true, UIType = type};
            entity.AddComponent(uiComponent);
            entity.AddComponent<ViewComponent>();
            
        }
    }
}
