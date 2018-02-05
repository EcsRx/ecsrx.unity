using UnityEngine;
using UIWidgets;
using System;
using System.Linq;

namespace UIWidgetsSamples {
	public class AccordionTest : MonoBehaviour
	{
		[SerializeField]
		Accordion TestAccordion;

		[SerializeField]
		GameObject ToggleGameObject;

		public void Test()
		{
			// find required item by toggle object
			var item = TestAccordion.Items.Find(x => x.ToggleObject==ToggleGameObject);
			// or content object
			//var item = TestAccordion.Items.Find(x => x.ContentObject==ContentGameObject);
			// or get item by index
			//var item = TestAccordion.Items[0];

			// and expand item
			TestAccordion.Open(item);
			//TestAccordion.Close(item);
			//TestAccordion.ToggleItem(item);
		}

		public void SimpleToggle()
		{
			var component = ToggleGameObject.GetComponent<AccordionItemComponent>();
			component.OnClick.Invoke();
		}
	}
}