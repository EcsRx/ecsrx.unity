using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;
using System.Collections.Generic;

namespace UIWidgets
{
	/// <summary>
	/// Layout switcher event.
	/// </summary>
	[Serializable]
	public class LayoutSwitcherEvent : UnityEvent<UILayout> {
	}

	/// <summary>
	/// Layout switcher.
	/// </summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class LayoutSwitcher : MonoBehaviour
	{
		/// <summary>
		/// The trackable objects.
		/// </summary>
		[SerializeField]
		protected List<RectTransform> Objects = new List<RectTransform>();

		/// <summary>
		/// The layouts.
		/// </summary>
		[SerializeField]
		public List<UILayout> Layouts = new List<UILayout>();

		/// <summary>
		/// Layout changed event.
		/// </summary>
		[SerializeField]
		public LayoutSwitcherEvent LayoutChanged = new LayoutSwitcherEvent();

		/// <summary>
		/// The default display size.
		/// </summary>
		[SerializeField]
		[Tooltip("Display size used when actual display size cannot be detected.")]
		public float DefaultDisplaySize;

		protected int windowWidth = 0;
		protected int windowHeight = 0;

		/// <summary>
		/// Update this instance.
		/// </summary>
		protected virtual void Update()
		{
			if (windowWidth!=Screen.width || windowHeight!=Screen.height)
			{
				windowWidth = Screen.width;
				windowHeight = Screen.height;
				ResolutionChanged();
			}
		}

		/// <summary>
		/// Saves the layout.
		/// </summary>
		/// <param name="layout">Layout.</param>
		public virtual void SaveLayout(UILayout layout)
		{
			layout.Save(Objects);
		}

		/// <summary>
		/// Load layout when resolution changed.
		/// </summary>
		public virtual void ResolutionChanged()
		{
			var currentLayout = GetCurrentLayout();
			if (currentLayout==null)
			{
				return ;
			}
			//Debug.Log(currentLayout.Name);

			currentLayout.Load();
			LayoutChanged.Invoke(currentLayout);
		}

		/// <summary>
		/// Gets the current layout.
		/// </summary>
		/// <returns>The current layout.</returns>
		public virtual UILayout GetCurrentLayout()
		{
			if (Layouts.Count==0)
			{
				return null;
			}

			var displaySize = DisplaySize();
			var aspectRatio = AspectRatio();

			var layouts_ar = Layouts
				.Where(x => {
					var diff = Mathf.Abs(aspectRatio - (x.AspectRatio.x / x.AspectRatio.y));
					//Debug.Log(x.Name + ": " + diff);
					return diff < 0.05f;
				}).ToList();
				//.OrderBy(x => Mathf.Abs( - (x.AspectRatio.x / x.AspectRatio.y)));
			if (layouts_ar.Count==0)
			{
				return null;
			}

			var layouts_ds = layouts_ar.Where(x => displaySize < x.MaxDisplaySize)
				.OrderBy(x => Mathf.Abs(x.MaxDisplaySize - displaySize)).ToList();
			if (layouts_ds.Count==0)
			{
				layouts_ds = layouts_ar.OrderBy(x => Mathf.Abs(x.MaxDisplaySize - displaySize)).ToList();
			}

			return layouts_ds.First();
		}

		/// <summary>
		/// Current aspect ratio.
		/// </summary>
		/// <returns>The ratio.</returns>
		public virtual float AspectRatio()
		{
			return (float)windowWidth / (float)windowHeight;
		}

		/// <summary>
		/// Current display size.
		/// </summary>
		/// <returns>The size.</returns>
		public virtual float DisplaySize()
		{
			if (Screen.dpi==0)
			{
				return DefaultDisplaySize;
			}
			return Mathf.Sqrt(windowWidth ^ 2 + windowHeight ^ 2) / Screen.dpi;
		}
	}
}

