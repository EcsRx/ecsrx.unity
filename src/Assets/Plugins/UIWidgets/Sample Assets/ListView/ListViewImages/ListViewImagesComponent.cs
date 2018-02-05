using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
using System.Collections.Generic;
using System.Collections;

namespace UIWidgetsSamples {
	public class ListViewImagesComponent : ListViewItem {
		[SerializeField]
		public Text Url;

		[SerializeField]
		public RawImage Image;

		[SerializeField]
		protected LayoutElement ImageLayoutElement;

		protected ListViewImagesItem Item;

		protected static Dictionary<string,Texture2D> Cache = new Dictionary<string, Texture2D>();

		protected bool IsLoading;

		protected IEnumerator loadCorutine;

		protected override void OnEnable()
		{
			base.OnEnable();
			if (IsLoading)
			{
				return;
			}
			if ((Image.texture==null) && (Item!=null) && (Item.Url!=""))
			{
				loadCorutine = Load();
				StartCoroutine(loadCorutine);
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (IsLoading)
			{
				IsLoading = false;
				StopCoroutine(loadCorutine);
			}
		}

		// Displaying item data
		public void SetData(ListViewImagesItem item)
		{
			// save item so later can fix item.Height to actual value
			Item = item;

			Url.text = (Item.Url!="") ? Item.Url : "No image";

			if (Cache.ContainsKey(Item.Url))
			{
				SetImage();
			}
			else
			{
				// reset images parameter
				Image.texture = null;
				ImageLayoutElement.preferredHeight = -1;
				ImageLayoutElement.preferredWidth = -1;

				if ((Item.Url!="") && (Item.Url!=null))
				{
					Image.color = Color.white;
					ImageLayoutElement.minHeight = 100;
					ImageLayoutElement.minWidth = 100;

					loadCorutine = Load();
					StartCoroutine(loadCorutine);
				}
				else
				{
					Image.color = Color.clear;
					ImageLayoutElement.minHeight = -1;
					ImageLayoutElement.minWidth = -1;
				}
			}
		}

		void SetImage()
		{
			Image.color = Color.white;
			Image.texture = Cache[Item.Url];
			ImageLayoutElement.preferredHeight = Cache[Item.Url].height;
			ImageLayoutElement.preferredWidth = Cache[Item.Url].width;
		}

		IEnumerator Load()
		{
			IsLoading = true;

			var url = Item.Url;

			yield return null;

			var www = new WWW(url);

			yield return www;
			if (!Cache.ContainsKey(url))
			{
				Cache.Add(url, www.texture);
			}
			if (Cache.ContainsKey(Item.Url))
			{
				SetImage();
			}

			IsLoading = false;
		}
	}
}