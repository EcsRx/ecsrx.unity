using UIWidgets;
using UnityEngine;

namespace UIWidgetsSamples
{
	public class TestSpinnerFloat : MonoBehaviour
	{
		[SerializeField]
		SpinnerFloat spinner;

		public void ChangeCulture()
		{
			#if !NETFX_CORE
			//Culture names https://msdn.microsoft.com/ru-ru/goglobal/bb896001.aspx
			spinner.Culture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
			#endif
		}
	}
}