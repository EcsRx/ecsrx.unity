using UnityEngine;
using UnityEngine.UI;
using UIWidgets;

namespace UIWidgetsSamples {
	public class TableRowComponent : ListViewItem, IResizableItem {
		[SerializeField]
		public Text Cell01Text;

		[SerializeField]
		public Text Cell02Text;
		
		[SerializeField]
		public Image Cell03Image;

		[SerializeField]
		public Text Cell04Text;

		TableRow Item;

		// Required for Resizable Header
		public GameObject[] ObjectsToResize {
			get {
				return new GameObject[] {
					Cell01Text.transform.parent.gameObject,
					Cell02Text.transform.parent.gameObject,
					Cell03Image.transform.parent.gameObject,
					Cell04Text.transform.parent.gameObject,
				};
			}
		}

		public void SetData(TableRow item)
		{
			Item = item;

			Cell01Text.text = Item.Cell01;
			Cell02Text.text = Item.Cell02.ToString();
			Cell03Image.sprite = Item.Cell03;
			Cell04Text.text = Item.Cell04.ToString();

			//set transparent color if no icon
			Cell03Image.color = (Cell03Image.sprite==null) ? Color.clear : Color.white;
		}

		// this function will be called when cell clicked
		public void CellClicked(string cellName)
		{
			Debug.Log(string.Format("clicked row {0}, cell {1}", Index, cellName));
			switch (cellName)
			{
				case "Cell01":
					Debug.Log("cell value: " + Item.Cell01);
					break;
				case "Cell02":
					Debug.Log("cell value: " + Item.Cell02);
					break;
				case "Cell03":
					Debug.Log("cell value: " + Item.Cell03);
					break;
				case "Cell04":
					Debug.Log("cell value: " + Item.Cell04);
					break;
				default:
					Debug.Log("cell value: <unknown cell>");
					break;
			}
		}

		protected bool IsColorSetted;
		
		public virtual void Coloring(Color primary, Color background, float fadeDuration)
		{
			// reset default color to white, otherwise it will look darker than specified color,
			// because actual color = Text.color * Text.CrossFadeColor
			if (!IsColorSetted)
			{
				Cell01Text.color = Color.white;
				Cell02Text.color = Color.white;
				Cell04Text.color = Color.white;
			}

			// change color instantly for first time
			Cell01Text.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			Cell02Text.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			Cell04Text.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);

			IsColorSetted = true;
		}
	}
}