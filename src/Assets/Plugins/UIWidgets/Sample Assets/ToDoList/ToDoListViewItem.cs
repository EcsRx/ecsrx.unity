using UnityEngine;
using System.Collections;
using UIWidgets;

namespace UIWidgetsSamples.ToDoList {
	[System.Serializable]
	public class ToDoListItem : IItemHeight {
		[SerializeField]
		public bool Done;

		[SerializeField]
		public string Task;

		[SerializeField]
		float height;
		
		public float Height {
			get {
				return height;
			}
			set {
				height = value;
			}
		}
	}
}