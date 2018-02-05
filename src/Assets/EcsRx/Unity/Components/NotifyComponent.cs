using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Components;
using UIWidgets;
using UnityEngine;

namespace EcsRx.Unity.Components
{
    public class NotifyComponent : IComponent
    {
        public string Message;
        public float? CustomHideDelay;
        public Transform Container;
        public Func<Notify, IEnumerator> ShowAnimation;
        public Func<Notify, IEnumerator> HideAnimation;
        public bool? SlideUpOnHide;
        public NotifySequence SequenceType;
        public float SequenceDelay;
        public bool ClearSequence;
        public bool? NewUnscaledTime;
    }
}
