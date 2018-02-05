using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	public class TestNotify : MonoBehaviour
	{
		[SerializeField]
		Notify notifyPrefab;//gameobject in Hierarchy window, parent gameobject should have Layout component (recommended EasyLayout)

		public void ShowNotify()
		{
			notifyPrefab.Template().Show("Achievement unlocked. Hide after 3 seconds.", customHideDelay: 3f);
		}
		/*
		[SerializeField]
		Notify notifyProjectPrefab;//prefab in Project window

		[SerializeField]
		Transform notifyContainer;//container for Notify gameobjects with Layout component (recommended EasyLayout)

		public void ShowNotify2()
		{
			notifyProjectPrefab.Template().Show("Achievement unlocked. Hide after 3 seconds.",
				customHideDelay: 3f, container: notifyContainer);
		}
		*/
	}
}