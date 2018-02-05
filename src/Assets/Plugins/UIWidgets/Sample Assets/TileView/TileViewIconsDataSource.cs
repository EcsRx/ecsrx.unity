using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UIWidgets;

namespace UIWidgetsSamples {
	[RequireComponent(typeof(TileViewIcons))]
	public class TileViewIconsDataSource : MonoBehaviour
	{
		[SerializeField]
		List<Sprite> icons;

		void Start()
		{
			var n = icons.Count - 1;
			var tiles = GetComponent<TileViewIcons>();
			tiles.Start();

			var items = tiles.DataSource;
			items.BeginUpdate();
			foreach (var i in Enumerable.Range(0, 140))
			{
				tiles.Add(new ListViewIconsItemDescription(){
					Name = "Tile " + i,
					Icon = icons.Count > 0 ? icons[Random.Range(0, n)] : null
				});
			}
			items.EndUpdate();
		}
	}
}