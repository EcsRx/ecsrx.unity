using System.Collections.Generic;
using System.Collections;
using System;

namespace UIWidgets {
	/// <summary>
	/// IObservableList.
	/// </summary>
	public interface IObservableList<T> : IList<T>, IObservable, ICollectionChanged, ICollectionItemChanged, IDisposable {

		/// <summary>
		/// Maintains performance while items are added/removed/changed by preventing the widgets from drawing until the EndUpdate method is called.
		/// </summary>
		void BeginUpdate();

		/// <summary>
		/// Ends the update and raise OnChange if something was changed.
		/// </summary>
		void EndUpdate();
	}
}