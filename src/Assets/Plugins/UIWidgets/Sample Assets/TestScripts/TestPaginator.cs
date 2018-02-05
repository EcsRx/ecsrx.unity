using UnityEngine;
using UIWidgets;

namespace UIWidgetsSamples
{
	public class TestPaginator : MonoBehaviour
	{
		[SerializeField]
		ScrollRectPaginator paginator;

		public void Test()
		{
			// pages count
			Debug.Log(paginator.Pages);

			// navigate to page
			paginator.CurrentPage = 2;
		}
	}
}