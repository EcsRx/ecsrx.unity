using UnityEngine;
using UnityEngine.Serialization;
using UIWidgets;

namespace UIWidgetsSamples.Tasks {
	[System.Serializable]
	public class Task {
		public string Name;

		[SerializeField]
		[FormerlySerializedAs("Progress")]
		int progress;

		public int Progress {
			get {
				return progress;
			}
			set {
				progress = value;
				OnProgressChange();
			}
		}

		public event OnChange OnProgressChange = () => { };
	}
}