using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.UI;
using EcsRx.Unity.Components;
using EcsRx.Unity.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace EcsRx.UI
{
    public class DefaultUIViewResolver : ViewResolverSystem
    {
        private IInstantiator instantiator;
        public override Group TargetGroup
        {
            get { return new Group(entity => entity.GetComponent<UIComponet>().IsDynamic, typeof(UIComponet), typeof(ViewComponent)); }
        }

        public DefaultUIViewResolver(IViewHandler viewHandler, IInstantiator instantiator) : base(viewHandler)
        {
            this.instantiator = instantiator;
        }

        public override GameObject ResolveView(IEntity entity)
        {
            UIComponet uiComponet = entity.GetComponent<UIComponet>();
            //GameObject prefab = Resources.Load(UIManager.Resource + uiComponet.UIName) as GameObject;
            Scene scene = SceneManager.GetActiveScene();
            GameObject uiRoot = scene.GetRootGameObjects().Single(o => o.name == UIManager.UIRoot);
            Transform container = uiRoot.transform;
            if (uiComponet.Container != "")
            {
                container = uiRoot.transform.Find(uiComponet.Container);
            }
            GameObject ui = instantiator.InstantiatePrefabResource(UIManager.Resource + uiComponet.UIName);
            ui.transform.SetParent(container, false);
            return ui;
        }
    }
}
