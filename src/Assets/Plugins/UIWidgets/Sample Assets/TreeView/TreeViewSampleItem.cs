using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	
	public interface ITreeViewSampleItem : IObservable {
		void Display(TreeViewSampleComponent component);
	}
}