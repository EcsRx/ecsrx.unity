using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Accessors;
using EcsRx.MVVM;
using EcsRx.Pools;
using EcsRx.Unity.Components;
using EcsRx.Unity.Events;
using EcsRx.Unity.UI;
using UIWidgets;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using EventSystem = EcsRx.Events.EventSystem;

namespace EcsRx.UI
{
    public class UIManager
    {
        public const string Resource = "Prefabs/UI/";
        public const string UIRoot = "Canvas";
        public ReactiveProperty<IEntity> CurrentScreen { get; set; }

        private IPool defaultPool;
        private IEventSystem eventSystem;
        private IGroupAccessor uiGroupAccessor;


        public UIManager(IPoolManager poolManager, IEventSystem eventSystem)
        {
            CurrentScreen = new ReactiveProperty<IEntity>();
            defaultPool = poolManager.GetPool();
            this.eventSystem = eventSystem;
            uiGroupAccessor = poolManager.CreateGroupAccessor(new Group(typeof(UIComponet)));
        }

        public IEntity GetUI(string ui)
        {
           return  uiGroupAccessor.Entities.SingleOrDefault(
                   entity => entity.GetComponent<UIComponet>().UIName == ui);
        }

        public IEntity ShowUI(string ui, UIType type)
        {
            var uiEntity = CreateUI(new DefaultUIBlueprint(ui , type));
            CreateUI(uiEntity);
            return uiEntity;
        }

        public IEntity ShowUI(string ui, UIType type, GameObject parent)
        {
            var entity = ShowUI(ui, type);
            var view = entity.GetComponent<ViewComponent>().View;
            view.transform.SetParent(parent.transform, false);
            return entity;
        }

        public IEntity ShowPopup(string ui, string title = null, string message = null, bool model = false, Color? modelColor = null)
        {
            var uiEntity = CreateUI(new PopupUIBlueprint(ui, title, message, model, modelColor??new Color(0.0f, 0.0f, 0.0f, 0.8f)));
            CreateUI(uiEntity);
            return uiEntity;
        }

        public IEntity ShowDialog(string ui, string title = null, string message = null, DialogActions butttons = null,
            bool model = false, Color? modelColor = null)
        {
            var uiEntity = CreateUI(new DialogUIBlueprint(ui, title, message, butttons, model, modelColor ?? new Color(0.0f, 0.0f, 0.0f, 0.8f)));
            CreateUI(uiEntity);
            return uiEntity;
        }

        public IEntity ShowNotify(string ui,
            string message = null,
            float? customHideDelay = null,
            Transform container = null,
            Func<Notify, IEnumerator> showAnimation = null,
            Func<Notify, IEnumerator> hideAnimation = null,
            bool? slideUpOnHide = null,
            NotifySequence sequenceType = NotifySequence.None,
            float sequenceDelay = 0.3f,
            bool clearSequence = false,
            bool? newUnscaledTime = null)
        {
            var uiEntity = CreateUI(new NotifyUIBlueprint(ui, message, customHideDelay, container, showAnimation, 
                hideAnimation, slideUpOnHide, sequenceType, sequenceDelay, clearSequence, newUnscaledTime));
            CreateUI(uiEntity);
            return uiEntity;
        }

        public IEntity CreateUI(IEntity ui)
        {
            var uiComponent = ui.GetComponent<UIComponet>();
            if (uiComponent.UIType == UIType.UI_ADD_SCREEN)
            {
                if (CurrentScreen.Value != null)
                {           
                    RemoveUI(CurrentScreen.Value);
                }
                CurrentScreen.Value = ui;
            }

            //var viewComponent = ui.GetComponent<ViewComponent>();
            //viewComponent.View.SetActive(true);
            eventSystem.Publish(new UIShowedEvent(ui));
            return ui;
        }

        public void ShowUI(IEntity ui)
        {
            var viewComponent = ui.GetComponent<ViewComponent>();
            viewComponent.View.SetActive(true);
            eventSystem.Publish(new UIShowedEvent(ui));
        }

        public void HideUI(IEntity ui)
        {
            var viewComponent = ui.GetComponent<ViewComponent>();
            viewComponent.View.SetActive(false);
            eventSystem.Publish(new UIHidedEvent(ui));
        }

        public void RemoveUI(IEntity ui)
        {
            defaultPool.RemoveEntity(ui);
            eventSystem.Publish(new UIRemovedEvent(ui));
        }

        public IEntity CreateUI<T>(T blueprint) where T : IBlueprint
        {
            IEntity entity = defaultPool.CreateEntity(blueprint);
            return entity;
        }

        public bool IsPointerOverUIObject()
        {//判断是否点击的是UI，有效应对安卓没有反应的情况，true为UI  
            PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}
