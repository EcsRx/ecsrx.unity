using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Entities;

namespace EcsRx.Unity.Events
{
    public class UIShowedEvent
    {
        public IEntity UI { get; private set; }

        public UIShowedEvent(IEntity ui)
        {
            UI = ui;
        }
    }

    public class UIHidedEvent
    {
        public IEntity UI { get; private set; }

        public UIHidedEvent(IEntity ui)
        {
            UI = ui;
        }
    }

    public class UIRemovedEvent
    {
        public IEntity UI { get; private set; }

        public UIRemovedEvent(IEntity ui)
        {
            UI = ui;
        }
    }

    public class BackScreenEvent
    {
        
    }
}
