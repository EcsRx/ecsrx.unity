using UnityEngine;
using System;

namespace UIWidgetsSamples {
	
	[Serializable]
	public class ListViewCustomSampleItemDescription {
		[SerializeField]
		public Sprite Icon;

		[SerializeField]
		public string Name;

		[SerializeField]
		public int Progress;
	}
}