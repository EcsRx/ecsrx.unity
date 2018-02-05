using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Accessors;
using EcsRx.Systems;
using EcsRx.UI;
using EcsRx.Unity.Components;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace EcsRx.MVVM
{
    public abstract class ViewModelBase<T, U> : ISetupSystem where T : ModelBase, new() where U : ViewBase, new()
    {
        protected string uiName;
        protected CompositeDisposable disposable;
        [Inject]
        protected UIManager uiManager;

        public virtual T CreateModel()
        {
            return new T();
        }

        public virtual U CreateView(IEntity entity)
        {
            return entity.AddComponent<U>();
        }

        public virtual void Initialize(U view, T model)
        {
            view.Model = model;
            SetupModel(model);
            view.View.OnEnableAsObservable().Subscribe(_ =>
            {
                Reinit(view, model);
            }).AddTo(view.View);
            view.View.OnDisableAsObservable().Subscribe(_ =>
            {
                view.Clear();
            }).AddTo(view.View);
        }

        protected void Reinit(U view, T model)
        {
            view.Bind();
            SetupEvents(view);
        }

        protected virtual void SetupEvents(U view)
        {
            
        }

        protected virtual void SetupModel(T model)
        {
            
        }

        public virtual Group TargetGroup {
            get { return new Group(entity => entity.GetComponent<UIComponet>().UIName == uiName, typeof (UIComponet), typeof(ViewComponent)); }
        }

        public void Setup(IEntity entity)
        {
            disposable = new CompositeDisposable();
            var viewComponet = entity.GetComponent<ViewComponent>();
            viewComponet.View.SetActive(false);
            var model = CreateModel();
            var view = CreateView(entity);
            view.InitWithView(viewComponet.View);
            Initialize(view, model);
            viewComponet.View.SetActive(true);
        }
    }
}
