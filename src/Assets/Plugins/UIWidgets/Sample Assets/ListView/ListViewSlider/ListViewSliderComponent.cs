using UnityEngine.UI;
using UnityEngine.EventSystems;
using UIWidgets;

namespace UIWidgetsSamples {
	public class ListViewSliderComponent : ListViewItem {
		public Slider Slider;

		public void SetData(ListViewSliderItem item)
		{
			Slider.value = item.Value;
		}

		public override void OnMove(AxisEventData eventData)
		{
			switch (eventData.moveDir)
			{
				case MoveDirection.Left:
				case MoveDirection.Right:
					Slider.OnMove(eventData);
					break;
				default:
					base.OnMove(eventData);
					break;
			}
		}
	}
}