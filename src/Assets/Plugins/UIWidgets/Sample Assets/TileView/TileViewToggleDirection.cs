using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UIWidgets;

namespace UIWidgetsSamples {
	/// <summary>
	/// TileView toggle direction.
	/// </summary>
	public class TileViewToggleDirection : MonoBehaviour {
		[SerializeField]
		TileViewSample Tiles;

		[SerializeField]
		Scrollbar VerticalScrollbar;

		[SerializeField]
		Scrollbar HorizontalScrollbar;

		[SerializeField]
		ScrollRectPaginator VerticalPaginator;

		[SerializeField]
		ScrollRectPaginator HorizontalPaginator;

		public void ToggleDirection()
		{
			if (Tiles.Direction==ListViewDirection.Horizontal)
			{
				VerticalScrollbar.gameObject.SetActive(true);

				Tiles.Direction = ListViewDirection.Vertical;
				Tiles.ScrollRect.horizontalScrollbar.value = 1;
				Tiles.ScrollRect.horizontalScrollbar = null;
				Tiles.ScrollRect.verticalScrollbar = VerticalScrollbar;

				HorizontalScrollbar.gameObject.SetActive(false);

				if (HorizontalPaginator!=null)
				{
					HorizontalPaginator.gameObject.SetActive(false);
				}
				if (VerticalPaginator!=null)
				{
					VerticalPaginator.gameObject.SetActive(true);
				}
			}
			else
			{
				HorizontalScrollbar.gameObject.SetActive(true);

				Tiles.Direction = ListViewDirection.Horizontal;
				Tiles.ScrollRect.horizontalScrollbar = HorizontalScrollbar;
				Tiles.ScrollRect.verticalScrollbar.value = 1;
				Tiles.ScrollRect.verticalScrollbar = null;

				VerticalScrollbar.gameObject.SetActive(false);

				if (VerticalPaginator!=null)
				{
					VerticalPaginator.gameObject.SetActive(false);
				}
				if (HorizontalPaginator!=null)
				{
					HorizontalPaginator.gameObject.SetActive(true);
				}
			}
		}
	}
}