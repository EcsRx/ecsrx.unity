using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.UI;
using EcsRx.Unity.Components;
using UIWidgets;
using UnityEngine;

namespace EcsRx.Unity.UI
{
    public class NotifyUIBlueprint : IBlueprint
    {
        private string uiName;
        private string message;
        private float? customHideDelay;
        private Transform container;
        private Func<Notify, IEnumerator> showAnimation;
        private Func<Notify, IEnumerator> hideAnimation;
        private bool? slideUpOnHide;
        private NotifySequence sequenceType;
        private float sequenceDelay;
        private bool clearSequence;
        private bool? newUnscaledTime;

        public NotifyUIBlueprint(string name,
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
            uiName = name;
            this.message = message;
            this.customHideDelay = customHideDelay;
            this.container = container;
            this.showAnimation = showAnimation;
            this.hideAnimation = hideAnimation;
            this.slideUpOnHide = slideUpOnHide;
            this.sequenceType = sequenceType;
            this.sequenceDelay = sequenceDelay;
            this.clearSequence = clearSequence;
            this.newUnscaledTime = newUnscaledTime;
        }

        public void Apply(IEntity entity)
        {
            var uiComponent = new UIComponet { UIName = uiName, IsDynamic = true,  UIType = UIType.UI_NOTIFY };
            var notifyComponent = new NotifyComponent
            {
                Message = message, CustomHideDelay = customHideDelay, Container = container, ShowAnimation = showAnimation,
                HideAnimation = hideAnimation,  SlideUpOnHide = slideUpOnHide, SequenceType = sequenceType, SequenceDelay = sequenceDelay,
                ClearSequence = clearSequence, NewUnscaledTime = newUnscaledTime
            };
            entity.AddComponent(uiComponent);
            entity.AddComponent(notifyComponent);
            entity.AddComponent<ViewComponent>();
        }
    }
}
