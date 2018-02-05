using UnityEngine;
using System.Linq;
using UIWidgets;

namespace UIWidgetsSamples {
	[RequireComponent(typeof(TileViewSample))]
	public class TileViewSampleDataSource : MonoBehaviour
	{
		void Start()
		{
			var tiles = GetComponent<TileViewSample>();
			tiles.Start();

			var items = tiles.DataSource;
			items.BeginUpdate();
			foreach (var i in Enumerable.Range(0, 40))
			{
				tiles.Add(new TileViewItemSample(){
					Name = "Tile " + i,
					Capital = "",
					Area = Random.Range(10, 10*6),
					Population = Random.Range(100, 100*6),
				});
			}
			items.EndUpdate();
		}
	}
}