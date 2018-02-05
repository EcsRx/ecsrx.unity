﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// TabIconActiveButton.
	/// </summary>
	public class TabIconActiveButton : TabIconButton {
		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="tab">Tab.</param>
		public override void SetData(TabIcons tab)
		{
			Name.text = tab.Name;
			if (Icon!=null)
			{
				Icon.sprite = tab.IconActive;

				if (SetNativeSize)
				{
					Icon.SetNativeSize();
				}

				Icon.color = (Icon.sprite==null) ? Color.clear : Color.white;
			}
		}
	}
}