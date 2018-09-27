namespace EcsRx.Persistence.Editor.EditorInputs
{
    public class UIStateChange
    {
        public bool HasChanged { get; set; }
        public object Value { get; set; }

        public static UIStateChange NoChange { get; } = new UIStateChange {HasChanged = false};
    }
}