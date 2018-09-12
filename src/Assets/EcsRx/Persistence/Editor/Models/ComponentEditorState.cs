using UnityEngine;

namespace EcsRx.Persistence.Editor.Models
{
    public class ComponentEditorState
    {
        public string ComponentName { get; set; }
        public Rect InteractionArea { get; set; }
        public bool ShowProperties { get; set; }
    }
}