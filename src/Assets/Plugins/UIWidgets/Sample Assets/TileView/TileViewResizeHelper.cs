using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples {
	/// <summary>
	/// TileView resize helper. Resize all items components when size one of them is changed.
	/// </summary>
	[RequireComponent(typeof(TileViewComponentSample))]
	[RequireComponent(typeof(Resizable))]
	public class TileViewResizeHelper : MonoBehaviour {
		[SerializeField]
		TileViewSample Tiles;

		void Start()
		{
			GetComponent<Resizable>().OnEndResize.AddListener(OnResize);
		}

		void OnResize(Resizable item)
		{
			var size = (item.transform as RectTransform).rect.size;
			Tiles.ForEachComponent(x => {
				if (x.gameObject==item.gameObject)
				{
					return ;
				}
				var rect = x.transform as RectTransform;

				rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
				rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
			});

			Tiles.Resize();
		}
	}
}