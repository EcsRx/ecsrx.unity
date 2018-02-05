using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace UIWidgets {
	/// <summary>
	/// TabIconButton.
	/// </summary>
	public abstract class TabIconButtonBase : TabButton {
		public abstract void SetData(TabIcons tab);
	}
}