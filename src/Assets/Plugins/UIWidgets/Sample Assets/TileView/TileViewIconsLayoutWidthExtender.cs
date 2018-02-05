using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UIWidgets;

namespace UIWidgetsSamples {
	[RequireComponent(typeof(EasyLayout.EasyLayout))]
	public class TileViewIconsLayoutWidthExtender : MonoBehaviour, ILayoutElement
	{
		[SerializeField]
		protected TileViewIcons TileViewIcons;

		public float flexibleHeight {
			get {
				return -1;
			}
		}

		public float flexibleWidth {
			get {
				return -1;
			}
		}

		public int layoutPriority {
			get {
				return 1;
			}
		}

		public float minHeight {
			get {
				return -1;
			}
		}

		public float minWidth {
			get {
				if (TileViewIcons==null)
				{
					return -1;
				}

				var width_min = WidthMin();
				var width_step = WidthStep();

				var k = Mathf.Ceil((Layout.minWidth - width_min) / width_step);
				return width_min + width_step * k;
			}
		}

		public float preferredHeight {
			get {
				return -1;
			}
		}

		public float preferredWidth {
			get {
				if (TileViewIcons==null)
				{
					return -1;
				}

				var width_min = WidthMin();
				var width_step = WidthStep();

				var k = Mathf.Ceil((Layout.preferredWidth - width_min) / width_step);
				return width_min + width_step * k;
			}
		}

		EasyLayout.EasyLayout layout;

		public EasyLayout.EasyLayout Layout {
			get {
				if (layout==null)
				{
					layout = GetComponent<EasyLayout.EasyLayout>();
				}
				return layout;
			}
		}

		float WidthMin()
		{
			var scroll_width = (TileViewIcons.ScrollRect.transform as RectTransform).rect.width;
			var item_width = (TileViewIcons.DefaultItem.transform as RectTransform).rect.width;
			var n = (scroll_width - item_width) / (item_width + Layout.Spacing.x);
			n = Mathf.Max(0, Mathf.Floor(n)) + 1;
			return item_width * n + Layout.Spacing.x * (n - 1);
		}

		float WidthStep()
		{
			return WidthMin() + Layout.Spacing.x;
		}

		public void CalculateLayoutInputHorizontal()
		{
		}

		public void CalculateLayoutInputVertical()
		{
		}
	}
}