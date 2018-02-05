using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UIWidgets {
	/// <summary>
	/// Scroll rect page with number.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ScrollRectPageWithNumber")]
	public class ScrollRectPageWithNumber : ScrollRectPage {
		/// <summary>
		/// The number.
		/// </summary>
		[SerializeField]
		public Text Number;

		/// <summary>
		/// Sets the page number.
		/// </summary>
		/// <param name="page">Page.</param>
		public override void SetPage(int page)
		{
			base.SetPage(page);
			if (Number!=null)
			{
				Number.text = (page + 1).ToString();
			}
		}
	}
}