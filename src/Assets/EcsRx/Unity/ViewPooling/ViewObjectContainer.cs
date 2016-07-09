using UnityEngine;

namespace Assets.EcsRx.Unity.ViewPooling
{
    public class ViewObjectContainer
    {
        public GameObject ViewObject { get; set; }
        public bool IsInUse { get; set; }

        public ViewObjectContainer(GameObject viewObject)
        {
            ViewObject = viewObject;
        }
    }
}