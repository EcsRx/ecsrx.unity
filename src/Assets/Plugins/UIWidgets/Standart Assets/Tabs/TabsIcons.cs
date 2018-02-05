using UnityEngine;

namespace UIWidgets
{
	/// <summary>
	/// TabsIcons.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/TabsIcons")]
	public class TabsIcons : TabsCustom<TabIcons,TabIconButtonBase>
	{
		/// <summary>
		/// Sets the name of the button.
		/// </summary>
		/// <param name="button">Button.</param>
		/// <param name="index">Index.</param>
		protected override void SetButtonData(TabIconButtonBase button, int index)
		{
			button.SetData(TabObjects[index]);
		}
	}
}