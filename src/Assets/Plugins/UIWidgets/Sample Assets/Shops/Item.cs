using UnityEngine;
using System.Collections;
using System.ComponentModel;
using UIWidgets;

namespace UIWidgetsSamples.Shops {
	/// <summary>
	/// Item.
	/// </summary>
	public class Item : IObservable, INotifyPropertyChanged {
		/// <summary>
		/// The name.
		/// </summary>
		public string Name;

		int count;

		/// <summary>
		/// Occurs when data changed.
		/// </summary>
		public event OnChange OnChange = () => { };

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged = (x, y) => { };

		/// <summary>
		/// Gets or sets the count. -1 for infinity count.
		/// </summary>
		/// <value>The count.</value>
		public int Count {
			get {
				return count;
			}
			set {
				if (count==-1)
				{
					Changed();
					return ;
				}
				count = value;
				Changed();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UIWidgetsSamples.Shops.Item"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="count">Count.</param>
		public Item(string name, int count)
		{
			Name = name;
			Count = count;
		}

		void Changed()
		{
			PropertyChanged(this, null);

			OnChange();
		}
	}
}