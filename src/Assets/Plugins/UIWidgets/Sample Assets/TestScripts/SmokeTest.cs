using UnityEngine;
using System.Collections;
using System.Linq;
using UIWidgets;

namespace UIWidgets.Tests
{
	public class SmokeTest : MonoBehaviour {
		[SerializeField]
		Accordion accordion;

		void Start()
		{
			#if UNITY_STANDALONE
			if (System.Environment.GetCommandLineArgs().Contains("-smoke-test"))
			{
				StartCoroutine(SimpleCheck());
			}
			#endif
		}
		
		IEnumerator SimpleCheck()
		{
			yield return new WaitForSeconds(5f);
			
			var items = accordion.Items;
			if (!accordion.Items[0].Open || !accordion.Items[0].ContentObject.activeSelf)
			{
				throw new UnityException("Overview is not active!");
			}

			foreach (var item in items)
			{
				if (item.ToggleObject.name=="Exit")
				{
					continue ;
				}
				accordion.ToggleItem(item);
				yield return new WaitForSeconds(5f);
			}

			Application.Quit();
		}
	}
}