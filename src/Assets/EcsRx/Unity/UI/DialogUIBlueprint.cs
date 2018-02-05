using System;
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
    public class DialogUIBlueprint : IBlueprint
    {
        private string uiName;
        private bool model;
        private Color? modelColor;
        private string title;
        private string message;
        private DialogActions buttons;


        public DialogUIBlueprint(string name, string title = null, string message = null, DialogActions buttons = null, bool model = false, Color? modelColor = null)
        {
            uiName = name;
            this.title = title;
            this.message = message;
            this.buttons = buttons;
            this.model = model;
            this.modelColor = modelColor;
        }
        public void Apply(IEntity entity)
        {
            var uiComponent = new UIComponet { UIName = uiName, IsDynamic = true, UIType = UIType.UI_DIALOG };
            var dialogComponent = new DialogComponent { Title = title, Message = message, Buttons = buttons, Model = model, ModalColor = modelColor };
            entity.AddComponent(uiComponent);
            entity.AddComponent(dialogComponent);
            entity.AddComponent<ViewComponent>();
        }
    }
}
