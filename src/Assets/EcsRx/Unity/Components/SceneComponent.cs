using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Components;
using UnityEngine.SceneManagement;

namespace EcsRx.Unity.Components
{
    public class SceneComponent : IComponent
    {
        public Scene Scene { get; set; }
    }
}
