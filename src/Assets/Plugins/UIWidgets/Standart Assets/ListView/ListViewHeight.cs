using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UIWidgets
{
	/// <summary>
	/// ListView with dynamic items heights.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ListViewHeight")]
	public class ListViewHeight : ListView
	{
		protected Dictionary<string,float> Heights = new Dictionary<string, float>();

		ListViewStringComponent defaultItemCopy;
		RectTransform defaultItemCopyRect;

		/// <summary>
		/// Gets the default item copy.
		/// </summary>
		/// <value>The default item copy.</value>
		protected ListViewStringComponent DefaultItemCopy {
			get {
				if (defaultItemCopy==null)
				{
					var copy = Instantiate(DefaultItem) as ImageAdvanced;
					copy.gameObject.SetActive(true);
					defaultItemCopy = copy.GetComponent<ListViewStringComponent>();
					defaultItemCopy.transform.SetParent(DefaultItem.transform.parent, false);
					defaultItemCopy.gameObject.name = "DefaultItemCopy";
					defaultItemCopy.gameObject.SetActive(false);

					Utilites.FixInstantiated(DefaultItem, defaultItemCopy);
				}
				return defaultItemCopy;
			}
		}

		/// <summary>
		/// Gets the RectTransform of DefaultItemCopy.
		/// </summary>
		/// <value>RectTransform.</value>
		protected RectTransform DefaultItemCopyRect {
			get {
				if (defaultItemCopyRect==null)
				{
					defaultItemCopyRect = defaultItemCopy.transform as RectTransform;
				}
				return defaultItemCopyRect;
			}
		}

		/// <summary>
		/// Awake this instance.
		/// </summary>
		protected override void Awake()
		{
			Start(); 
		}

		/// <summary>
		/// Calculates the max count of visible items.
		/// </summary>
		protected override void CalculateMaxVisibleItems()
		{
			SetItemsHeight(DataSource);

			var height = scrollHeight;
			maxVisibleItems = DataSource.OrderBy<string,float>(GetItemHeight).TakeWhile(x => {
				height -= Heights[x];
				return height > 0;
			}).Count() + 2;
		}

		/// <summary>
		/// Calculates the size of the item.
		/// </summary>
		protected override void CalculateItemSize()
		{
			var rect = DefaultItem.transform as RectTransform;
			#if UNITY_4_6 || UNITY_4_7
			var layout_elements = rect.GetComponents<Component>().OfType<ILayoutElement>();
			#else
			var layout_elements = rect.GetComponents<ILayoutElement>();
			#endif
			if (itemHeight==0)
			{
				var preffered_height = layout_elements.Max(x => Mathf.Max(x.minHeight, x.preferredHeight));
				itemHeight = (preffered_height > 0) ? preffered_height : rect.rect.height;
			}
			if (itemWidth==0)
			{
				var preffered_width = layout_elements.Max(x => Mathf.Max(x.minWidth, x.preferredWidth));
				itemWidth = (preffered_width > 0) ? preffered_width : rect.rect.width;
			}
		}
		
		/// <summary>
		/// Scrolls to item with specifid index.
		/// </summary>
		/// <param name="index">Index.</param>
		public override void ScrollTo(int index)
		{
			if (!CanOptimize())
			{
				return ;
			}

			var top = GetScrollValue();
			var bottom = GetScrollValue() + scrollHeight;

			var item_starts = ItemStartAt(index);

			var item_ends = ItemEndAt(index) + LayoutBridge.GetMargin();

			if (item_starts < top)
			{
				SetScrollValue(item_starts);
			}
			else if (item_ends > bottom)
			{
				SetScrollValue(item_ends - GetScrollSize());
			}
		}

		/// <summary>
		/// Calculates the size of the bottom filler.
		/// </summary>
		/// <returns>The bottom filler size.</returns>
		protected override float CalculateBottomFillerSize()
		{
			if (bottomHiddenItems==0)
			{
				return 0f;
			}
			var height = DataSource.GetRange(topHiddenItems + visibleItems, bottomHiddenItems).SumFloat(GetItemHeight);

			return Mathf.Max(0, height + (LayoutBridge.GetSpacing() * (bottomHiddenItems - 1)));
		}

		/// <summary>
		/// Calculates the size of the top filler.
		/// </summary>
		/// <returns>The top filler size.</returns>
		protected override float CalculateTopFillerSize()
		{
			if (topHiddenItems==0)
			{
				return 0f;
			}

			var height = DataSource.GetRange(0, topHiddenItems).SumFloat(GetItemHeight);

			return Mathf.Max(0, height + (LayoutBridge.GetSpacing() * (topHiddenItems - 1)));
		}

		float GetItemHeight(string item)
		{
			return Heights[item];
		}

		/// <summary>
		/// Total height of items before specified index.
		/// </summary>
		/// <returns>Height.</returns>
		/// <param name="index">Index.</param>
		float ItemStartAt(int index)
		{
			var height = DataSource.GetRange(0, index).SumFloat(GetItemHeight);

			return height + (LayoutBridge.GetSpacing() * index);
		}

		/// <summary>
		/// Total height of items before and with specified index.
		/// </summary>
		/// <returns>The <see cref="System.Single"/>.</returns>
		/// <param name="index">Index.</param>
		float ItemEndAt(int index)
		{
			var height = DataSource.GetRange(0, index + 1).SumFloat(GetItemHeight);
			
			return height + (LayoutBridge.GetSpacing() * index);
		}

		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>Index of added item.</returns>
		public override int Add(string item)
		{
			if (item==null)
			{
				throw new ArgumentNullException("item", "Item is null.");
			}
			if (!Heights.ContainsKey(item))
			{
				Heights.Add(item, CalculateItemHeight(item));
			}

			return base.Add(item);
		}

		/// <summary>
		/// Calculate and sets the height of the items.
		/// </summary>
		/// <param name="items">Items.</param>
		/// <param name="forceUpdate">If set to <c>true</c> force update.</param>
		void SetItemsHeight(ObservableList<string> items, bool forceUpdate = true)
		{
			if (forceUpdate)
			{
				Heights.Clear();
			}
			items.Except(Heights.Keys).Distinct().ForEach(x => {
				Heights.Add(x, CalculateItemHeight(x));
			});
		}

		/// <summary>
		/// Resize this instance.
		/// </summary>
		public override void Resize()
		{
			SetItemsHeight(DataSource, true);

			base.Resize();
		}

		/// <summary>
		/// Updates the items.
		/// </summary>
		/// <param name="newItems">New items.</param>
		protected override void SetNewItems(ObservableList<string> newItems)
		{
			SetItemsHeight(newItems);
			CalculateMaxVisibleItems();

			base.SetNewItems(newItems);
		}

		/// <summary>
		/// Gets the height of the index by.
		/// </summary>
		/// <returns>The index by height.</returns>
		/// <param name="height">Height.</param>
		int GetIndexByHeight(float height)
		{
			var spacing = LayoutBridge.GetSpacing();
			return DataSource.TakeWhile((item, index) => {
				height -= Heights[item];
				if (index > 0)
				{
					height -= spacing;
				}
				return height >= 0;
			}).Count();
		}

		/// <summary>
		/// Gets the last index of the visible.
		/// </summary>
		/// <returns>The last visible index.</returns>
		/// <param name="strict">If set to <c>true</c> strict.</param>
		protected override int GetLastVisibleIndex(bool strict=false)
		{
			var last_visible_index = GetIndexByHeight(GetScrollValue() + scrollHeight);

			return (strict) ? last_visible_index : last_visible_index + 2;
		}

		/// <summary>
		/// Gets the first index of the visible.
		/// </summary>
		/// <returns>The first visible index.</returns>
		/// <param name="strict">If set to <c>true</c> strict.</param>
		protected override int GetFirstVisibleIndex(bool strict=false)
		{
			var first_visible_index = GetIndexByHeight(GetScrollValue());

			if (strict)
			{
				return first_visible_index;
			}
			return Mathf.Min(first_visible_index, Mathf.Max(0, DataSource.Count - visibleItems));
		}
		
		LayoutGroup defaultItemLayoutGroup;

		/// <summary>
		/// Gets the height of the item.
		/// </summary>
		/// <returns>The item height.</returns>
		/// <param name="item">Item.</param>
		float CalculateItemHeight(string item)
		{
			if (defaultItemLayoutGroup==null)
			{
				defaultItemLayoutGroup = DefaultItemCopy.GetComponent<LayoutGroup>();
			}

			float height = 0f;
			if (defaultItemLayoutGroup!=null)
			{
				DefaultItemCopy.gameObject.SetActive(true);

				DefaultItemCopy.SetData(item);
				Utilites.UpdateLayout(defaultItemLayoutGroup);

				height = LayoutUtility.GetPreferredHeight(DefaultItemCopyRect);

				DefaultItemCopy.gameObject.SetActive(false);
			}

			return height;
		}

		/// <summary>
		/// Calls specified function with each component.
		/// </summary>
		/// <param name="func">Func.</param>
		public override void ForEachComponent(Action<ListViewItem> func)
		{
			base.ForEachComponent(func);
			func(DefaultItemCopy);
		}

		#region ListViewPaginator support
		public override int GetNearestItemIndex()
		{
			return GetIndexByHeight(GetScrollValue());
		}
		#endregion
	}
}