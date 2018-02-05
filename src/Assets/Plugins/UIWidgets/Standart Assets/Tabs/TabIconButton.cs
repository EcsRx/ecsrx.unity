using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace UIWidgets {
	/// <summary>
	/// TabIconButton.
	/// </summary>
	public class TabIconButton : TabIconButtonBase {
		/// <summary>
		/// The name.
		/// </summary>
		[SerializeField]
		public Text Name;

		/// <summary>
		/// The icon.
		/// </summary>
		[SerializeField]
		public Image Icon;

		/// <summary>
		/// The size of the set native.
		/// </summary>
		[SerializeField]
		public bool SetNativeSize;

		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="tab">Tab.</param>
		public override void SetData(TabIcons tab)
		{
			Name.text = tab.Name;
		}
	}
}