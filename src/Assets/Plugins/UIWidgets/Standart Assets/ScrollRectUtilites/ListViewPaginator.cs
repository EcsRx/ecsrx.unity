using UnityEngine;
using System;

namespace UIWidgets {

	/// <summary>
	/// ListView's paginator. Also works with TileView's.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ListViewPaginator")]
	public class ListViewPaginator : ScrollRectPaginator {
		/// <summary>
		/// ListView.
		/// </summary>
		[SerializeField]
		protected ListViewBase ListView;

		/// <summary>
		/// Count of items on one page.
		/// </summary>
		[SerializeField]
		protected int perPage = 1;

		/// <summary>
		/// Gets or sets the count of items on one page.
		/// </summary>
		/// <value>The per page.</value>
		public int PerPage {
			get {
				return Mathf.Max(1, perPage);
			}
			set {
				perPage = Mathf.Max(1, value);
				RecalculatePages();
			}
		}

		bool isListViewPaginatorStarted;

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected override void Start()
		{
			if (isListViewPaginatorStarted)
			{
				return ;
			}
			isListViewPaginatorStarted = true;

			ScrollRect = ListView.GetScrollRect();
			base.Start();
		}

		/// <summary>
		/// Handle scroll changes.
		/// </summary>
		protected override void ScrollChanged()
		{
			//var page = Mathf.FloorToInt(((float)ListView.GetNearestItemIndex()) / (ListView.GetItemsPerBlock() * PerPage));
			//GoToPage(page, true);
			if (!gameObject.activeInHierarchy)
			{
				return ;
			}

			var distance = Mathf.Abs(IsHorizontal() ? DragDelta.x : DragDelta.y);
			var time = Time.unscaledTime - DragStarted;

			var is_fast = (distance >= FastDragDistance) && (time <= FastDragTime);
			if (!is_fast)
			{
				//var pos = IsHorizontal() ? -ScrollRect.content.anchoredPosition.x : ScrollRect.content.anchoredPosition.y;
				//var page = Mathf.RoundToInt(pos / GetPageSize());
				var page = Mathf.FloorToInt(((float)ListView.GetNearestItemIndex()) / (ListView.GetItemsPerBlock() * PerPage));
				GoToPage(page, true);

				DragDelta = Vector2.zero;
				DragStarted = 0f;
			}
			else
			{
				var direction = IsHorizontal() ? DragDelta.x : -DragDelta.y;
				DragDelta = Vector2.zero;
				if (direction==0f)
				{
					return ;
				}
				var page = direction < 0 ? CurrentPage + 1 : CurrentPage - 1;
				GoToPage(page, false);
			}
		}

		/// <summary>
		/// Recalculate the pages count.
		/// </summary>
		protected override void RecalculatePages()
		{
			SetScrollRectMaxDrag();

			var per_block = ListView.GetItemsPerBlock() * PerPage;
			Pages = (per_block==0) ? 0 : Mathf.CeilToInt(((float)ListView.GetItemsCount()) / per_block);
		}

		/// <summary>
		/// Gets the page position.
		/// </summary>
		/// <returns>The page position.</returns>
		/// <param name="page">Page.</param>
		protected override float GetPagePosition(int page)
		{
			var pos = ListView.GetItemPosition(page * ListView.GetItemsPerBlock() * PerPage);
			return pos;
		}
	}
}