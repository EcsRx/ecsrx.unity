using UIWidgets;
using UnityEngine;

namespace UIWidgetsSamples
{
	public class TileViewIconsTest : MonoBehaviour
	{
		public void ChangeLayoutSettings()
		{
			var tiles = GetComponent<TileViewIcons>();
			tiles.Layout.Spacing = new Vector2(5, 50);
			tiles.Layout.UpdateLayout();

			tiles.ScrollRect.GetComponent<ResizeListener>().OnResize.Invoke();
		}
	}
}