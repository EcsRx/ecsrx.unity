using System;
using System.Collections;
using System.Collections.Generic;
using EcsRx.Components;
using EcsRx.Entities;
using UnityEngine;
using EcsRx.Unity.Components;
using EcsRx.Unity.MonoBehaviours;
using UniRx;

namespace EcsRx.MVVM
{
    public class ViewBase : IComponent, IDisposable
    {
        protected ModelBase model;

        private CompositeDisposable disposables = new CompositeDisposable();
        protected CompositeDisposable dataSourceDisposables = new CompositeDisposable();
        protected bool isDisposed;
        public GameObject View { get; set; }
        public IEntity Entity {
            get
            {
                var entityView = View.GetComponent<EntityView>();
                return entityView.Entity;
            }
        }

        public CompositeDisposable Disposables { get { return disposables;} }

        public virtual ModelBase Model
        {
            get { return model; }
            set
            {
                if (model == value) return;
                //Unbind();

                model = value;

                //if (model == null)
                //{
                //    return;
                //}

                //Bind();
            }
        }

        public virtual void InitWithView(GameObject view)
        {
            View = view;
        }

        public virtual void Bind()
        {
            
        }

        protected virtual void Unbind()
        {
            if (dataSourceDisposables != null)
            {
                dataSourceDisposables.Clear();
            }
        }

        public virtual void Clear()
        {
            Unbind();
            disposables.Clear();
        }

        public virtual void Dispose()
        {
            if (!isDisposed)
            {
                dataSourceDisposables.Dispose();
                disposables.Dispose();
                isDisposed = true;
            }
        }
    }
}

