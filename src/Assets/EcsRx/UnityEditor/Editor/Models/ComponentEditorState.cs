using UnityEngine;

namespace EcsRx.UnityEditor.Editor.Models
{
    public class ComponentEditorState
    {
        public string ComponentName { get; set; }
        public Rect InteractionArea { get; set; }
        public bool ShowProperties { get; set; }
    }
}