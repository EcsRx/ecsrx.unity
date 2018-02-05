using UnityEngine;
using UIWidgets;
using System.Linq;
using System;

namespace UIWidgetsSamples
{
	public class TestTabs : MonoBehaviour {
		[SerializeField]
		Tabs Tabs;

		void Start()
		{
			Tabs.OnTabSelect.AddListener(Test2);
		}

		public void Test2(int index)
		{
			Debug.Log("Name: " + Tabs.SelectedTab.Name);
			Debug.Log("Index: " + index);
			Debug.Log("Index: " + Array.IndexOf(Tabs.TabObjects, Tabs.SelectedTab));
		}
	}
}