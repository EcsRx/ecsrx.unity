using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Components;
using UnityEngine;

namespace EcsRx.Unity.Components
{
    public class PopupComponent : IComponent
    {
        public bool Model;
        public Color? ModalColor;
        public string Title;
        public string Message;
    }
}
