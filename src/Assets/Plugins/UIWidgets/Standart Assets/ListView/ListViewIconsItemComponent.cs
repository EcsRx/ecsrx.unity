using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UIWidgets {
	/// <summary>
	/// ListViewIcons item component.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ListViewIconsItemComponent")]
	public class ListViewIconsItemComponent : ListViewItem, IResizableItem {
		/// <summary>
		/// Gets the objects to resize.
		/// </summary>
		/// <value>The objects to resize.</value>
		public GameObject[] ObjectsToResize {
			get {
				return new GameObject[] {Icon.transform.parent.gameObject, Text.gameObject};
			}
		}

		/// <summary>
		/// The icon.
		/// </summary>
		[SerializeField]
		public Image Icon;

		/// <summary>
		/// The text.
		/// </summary>
		[SerializeField]
		public Text Text;

		/// <summary>
		/// Set icon native size.
		/// </summary>
		public bool SetNativeSize = true;

		/// <summary>
		/// Current item.
		/// </summary>
		protected ListViewIconsItemDescription item;

		/// <summary>
		/// Gets the current item.
		/// </summary>
		/// <value>Current item.</value>
		public ListViewIconsItemDescription Item {
			get {
				return item;
			}
		}

		/// <summary>
		/// Sets component data with specified item.
		/// </summary>
		/// <param name="newItem">Item.</param>
		public virtual void SetData(ListViewIconsItemDescription newItem)
		{
			item = newItem;
			name = newItem==null ? "DefaultItem (Clone)" : newItem.Name;
			if (item==null)
			{
				if (Icon!=null)
				{
					Icon.sprite = null;
				}
				Text.text = string.Empty;
			}
			else
			{
				if (Icon!=null)
				{
					Icon.sprite = item.Icon;
				}
				Text.text = item.LocalizedName ?? item.Name;
			}

			if ((SetNativeSize) && (Icon!=null))
			{
				Icon.SetNativeSize();
			}

			//set transparent color if no icon
			if (Icon!=null)
			{
				Icon.color = (Icon.sprite==null) ? Color.clear : Color.white;
			}
		}

		/// <summary>
		/// Set default colors.
		/// </summary>
		/// <param name="primary">Primary color.</param>
		/// <param name="background">Background color.</param>
		/// <param name="fadeDuration">Fade duration.</param>
		public virtual void DefaultColoring(Color primary, Color background, float fadeDuration = 0f)
		{
			Coloring(primary, background, fadeDuration);
		}

		/// <summary>
		/// Set highlights colors.
		/// </summary>
		/// <param name="primary">Primary color.</param>
		/// <param name="background">Background color.</param>
		/// <param name="fadeDuration">Fade duration.</param>
		public virtual void HighlightColoring(Color primary, Color background, float fadeDuration = 0f)
		{
			Coloring(primary, background, fadeDuration);
		}

		/// <summary>
		/// Set select colors.
		/// </summary>
		/// <param name="primary">Primary color.</param>
		/// <param name="background">Background color.</param>
		/// <param name="fadeDuration">Fade duration.</param>
		public virtual void SelectColoring(Color primary, Color background, float fadeDuration = 0f)
		{
			Coloring(primary, background, fadeDuration);
		}

		/// <summary>
		/// Is color setted at least once.
		/// </summary>
		protected bool IsColorSetted = false;

		/// <summary>
		/// Set colors.
		/// </summary>
		/// <param name="primary">Primary color.</param>
		/// <param name="background">Background color.</param>
		/// <param name="fadeDuration">Fade duration.</param>
		protected virtual void Coloring(Color primary, Color background, float fadeDuration=0.0f)
		{
			// reset default color to white, otherwise it will look darker than specified color,
			// because actual color = Text.color * Text.CrossFadeColor
			if (!IsColorSetted)
			{
				Text.color = Color.white;
				Background.color = Color.white;
			}

			// change color instantly for first time
			Text.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			Background.CrossFadeColor(background, IsColorSetted ? fadeDuration : 0f, true, true);

			IsColorSetted = true;
		}

		/// <summary>
		/// Called when item moved to cache, you can use it free used resources.
		/// </summary>
		public override void MovedToCache()
		{
			if (Icon!=null)
			{
				Icon.sprite = null;
			}
		}
	}
}