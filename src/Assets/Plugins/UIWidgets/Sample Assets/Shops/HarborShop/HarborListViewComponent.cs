using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UIWidgets;

namespace UIWidgetsSamples.Shops {
	/// <summary>
	/// HarborListViewComponent.
	/// </summary>
	public class HarborListViewComponent : ListViewItem {
		[SerializeField]
		public Text Name;

		[SerializeField]
		public Text SellPrice;

		[SerializeField]
		public Text BuyPrice;
		
		[SerializeField]
		public Text AvailableBuyCount;

		[SerializeField]
		public Text AvailableSellCount;

		[SerializeField]
		CenteredSlider Count;
		
		public HarborOrderLine OrderLine;

		protected override void Start()
		{
			Count.OnValuesChange.AddListener(ChangeCount);
			base.Start();
		}

		/// <summary>
		/// Change count on left and right movements.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public override void OnMove(AxisEventData eventData)
		{
			switch (eventData.moveDir)
			{
				case MoveDirection.Left:
					Count.Value -= 1;
					break;
				case MoveDirection.Right:
					Count.Value += 1;
					break;
				default:
					base.OnMove(eventData);
					break;
			}
		}

		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="orderLine">Order line.</param>
		public void SetData(HarborOrderLine orderLine)
		{
			OrderLine = orderLine;
			
			Name.text = OrderLine.Item.Name;

			BuyPrice.text = OrderLine.BuyPrice.ToString();
			SellPrice.text = OrderLine.SellPrice.ToString();

			AvailableBuyCount.text = OrderLine.BuyCount.ToString();
			AvailableSellCount.text = OrderLine.SellCount.ToString();

			Count.LimitMin = -OrderLine.SellCount;
			Count.LimitMax = OrderLine.BuyCount;
			Count.Value = OrderLine.Count;
		}
		
		void ChangeCount(int value)
		{
			OrderLine.Count = value;
		}

		protected override void OnDestroy()
		{
			if (Count!=null)
			{
				Count.OnValuesChange.RemoveListener(ChangeCount);
			}
		}

		protected bool IsColorSetted;
		
		public virtual void Coloring(Color primary, Color background, float fadeDuration)
		{
			// reset default color to white, otherwise it will look darker than specified color,
			// because actual color = Text.color * Text.CrossFadeColor
			if (!IsColorSetted)
			{
				Name.color = Color.white;
				BuyPrice.color = Color.white;
				SellPrice.color = Color.white;
				AvailableBuyCount.color = Color.white;
				AvailableSellCount.color = Color.white;
				Background.color = Color.white;
			}

			// change color instantly for first time
			Name.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			BuyPrice.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			SellPrice.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			AvailableBuyCount.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			AvailableSellCount.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			Background.CrossFadeColor(background, IsColorSetted ? fadeDuration : 0f, true, true);

			IsColorSetted = true;
		}
	}
}