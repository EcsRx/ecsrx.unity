using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EasyLayout
{
	/// <summary>
	/// EasyLayout utilites.
	/// </summary>
	public static class EasyLayoutUtilites
	{
		struct Sizes {
			public float Min;
			public float Preferred;
			public float Flexible;
		}
		struct SizesInfo {
			public float TotalMin;
			public float TotalPreferred;
			public float TotalFlexible;
			public Sizes[] Sizes;
		}

		static SizesInfo GetSizesInfo(Sizes[] sizes)
		{
			var result = new SizesInfo(){Sizes = sizes};
			for (int i = 0; i < sizes.Length; i++)
			{
				result.TotalMin += sizes[i].Min;
				result.TotalPreferred += sizes[i].Preferred;
				result.TotalFlexible += sizes[i].Flexible;
			}
			if (result.TotalFlexible==0f)
			{
				for (int i = 0; i < sizes.Length; i++)
				{
					sizes[i].Flexible = 1f;
				}
				result.TotalFlexible += sizes.Length;
			}
			return result;
		}

		static SizesInfo GetWidths(List<RectTransform> elems)
		{
			var sizes = new Sizes[elems.Count];
			for (int i = 0; i < elems.Count; i++)
			{
				sizes[i] = new Sizes(){
					Min = GetMinWidth(elems[i]),
					Preferred = GetPreferredWidth(elems[i]),
					Flexible = GetFlexibleWidth(elems[i]),
				};
			}
			return GetSizesInfo(sizes);
		}

		static SizesInfo GetHeights(List<RectTransform> elems)
		{
			var sizes = new Sizes[elems.Count];
			for (int i = 0; i < elems.Count; i++)
			{
				sizes[i] = new Sizes(){
					Min = GetMinHeight(elems[i]),
					Preferred = GetPreferredHeight(elems[i]),
					Flexible = GetFlexibleHeight(elems[i]),
				};
			}
			return GetSizesInfo(sizes);
		}

		static SizesInfo GetWidths(List<List<RectTransform>> elems)
		{
			var sizes = new Sizes[elems.Count];
			for (int i = 0; i < elems.Count; i++)
			{
				sizes[i] = new Sizes(){
					Min = GetMaxMinWidth(elems[i]),
					Preferred = GetMaxPreferredWidth(elems[i]),
					Flexible = GetMaxFlexibleWidth(elems[i]),
				};
			}
			return GetSizesInfo(sizes);
		}

		static SizesInfo GetHeights(List<List<RectTransform>> elems)
		{
			var sizes = new Sizes[elems.Count];
			for (int i = 0; i < elems.Count; i++)
			{
				sizes[i] = new Sizes(){
					Min = GetMaxMinHeight(elems[i]),
					Preferred = GetMaxPreferredHeight(elems[i]),
					Flexible = GetMaxFlexibleHeight(elems[i]),
				};
			}
			return GetSizesInfo(sizes);
		}

		static void ResizeToFit(float size, List<RectTransform> elems, float spacing, RectTransform.Axis axis)
		{
			var sizes = axis==RectTransform.Axis.Horizontal ? GetWidths(elems) : GetHeights(elems);

			float free_space = size - sizes.TotalPreferred - ((elems.Count - 1) * spacing);
			var per_flexible = Mathf.Max(0f, free_space / sizes.TotalFlexible);

			var minPrefLerp = 0f;
			if (sizes.TotalMin != sizes.TotalPreferred)
			{
				minPrefLerp = Mathf.Clamp01((size - sizes.TotalMin - ((elems.Count - 1) * spacing)) / (sizes.TotalPreferred - sizes.TotalMin));
			}

			for (int i = 0; i < elems.Count; i++)
			{
				var element_size = Mathf.Lerp(sizes.Sizes[i].Min, sizes.Sizes[i].Preferred, minPrefLerp) + (per_flexible * sizes.Sizes[i].Flexible);
				elems[i].SetSizeWithCurrentAnchors(axis, element_size);
			}
		}

		static void ResizeToFit(float size, List<List<RectTransform>> elems, float spacing, RectTransform.Axis axis)
		{
			var sizes = axis==RectTransform.Axis.Horizontal ? GetWidths(elems) : GetHeights(elems);

			float free_space = size - sizes.TotalPreferred - ((elems.Count - 1) * spacing);
			var per_flexible = Mathf.Max(0f, free_space / sizes.TotalFlexible);

			var minPrefLerp = 0f;
			if (sizes.TotalMin != sizes.TotalPreferred)
			{
				minPrefLerp = Mathf.Clamp01((size - sizes.TotalMin - ((elems.Count - 1) * spacing)) / (sizes.TotalPreferred - sizes.TotalMin));
			}

			for (int i = 0; i < elems.Count; i++)
			{
				var element_size = Mathf.Lerp(sizes.Sizes[i].Min, sizes.Sizes[i].Preferred, minPrefLerp) + (per_flexible * sizes.Sizes[i].Flexible);
				for (int j = 0; j < elems[i].Count; j++)
				{
					elems[i][j].SetSizeWithCurrentAnchors(axis, element_size);
				}
			}
		}

		static void ShrinkToFit(float size, List<RectTransform> elems, float spacing, RectTransform.Axis axis)
		{
			var sizes = axis==RectTransform.Axis.Horizontal ? GetWidths(elems) : GetHeights(elems);

			float free_space = size - sizes.TotalPreferred - ((elems.Count - 1) * spacing);
			if (free_space > 0f)
			{
				return ;
			}
			var per_flexible = Mathf.Max(0f, free_space / sizes.TotalFlexible);

			var minPrefLerp = 0f;
			if (sizes.TotalMin != sizes.TotalPreferred)
			{
				minPrefLerp = Mathf.Clamp01((size - sizes.TotalMin - ((elems.Count - 1) * spacing)) / (sizes.TotalPreferred - sizes.TotalMin));
			}

			for (int i = 0; i < elems.Count; i++)
			{
				var element_size = Mathf.Lerp(sizes.Sizes[i].Min, sizes.Sizes[i].Preferred, minPrefLerp) + (per_flexible * sizes.Sizes[i].Flexible);
				elems[i].SetSizeWithCurrentAnchors(axis, element_size);
			}
		}

		static void ShrinkToFit(float size, List<List<RectTransform>> elems, float spacing, RectTransform.Axis axis)
		{
			var sizes = axis==RectTransform.Axis.Horizontal ? GetWidths(elems) : GetHeights(elems);

			float free_space = size - sizes.TotalPreferred - ((elems.Count - 1) * spacing);
			if (free_space > 0f)
			{
				return ;
			}
			var per_flexible = Mathf.Max(0f, free_space / sizes.TotalFlexible);

			var minPrefLerp = 0f;
			if (sizes.TotalMin != sizes.TotalPreferred)
			{
				minPrefLerp = Mathf.Clamp01((size - sizes.TotalMin - ((elems.Count - 1) * spacing)) / (sizes.TotalPreferred - sizes.TotalMin));
			}

			for (int i = 0; i < elems.Count; i++)
			{
				var element_size = Mathf.Lerp(sizes.Sizes[i].Min, sizes.Sizes[i].Preferred, minPrefLerp) + (per_flexible * sizes.Sizes[i].Flexible);
				for (int j = 0; j < elems[i].Count; j++)
				{
					elems[i][j].SetSizeWithCurrentAnchors(axis, element_size);
				}
			}
		}

		/// <summary>
		/// Resizes the width to fit container.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="group">Group.</param>
		/// <param name="spacing">Spacing.</param>
		static public void ResizeWidthToFit(float width, List<List<RectTransform>> group, float spacing)
		{
			for (int i = 0; i < group.Count; i++)
			{
				ResizeToFit(width, group[i], spacing, RectTransform.Axis.Horizontal);
			}
		}

		/// <summary>
		/// Shrink the width to fit container.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="group">Group.</param>
		/// <param name="spacing">Spacing.</param>
		static public void ShrinkWidthToFit(float width, List<List<RectTransform>> group, float spacing)
		{
			for (int i = 0; i < group.Count; i++)
			{
				ShrinkToFit(width, group[i], spacing, RectTransform.Axis.Horizontal);
			}
		}

		/// <summary>
		/// Resizes the height to fit container.
		/// </summary>
		/// <param name="height">Height.</param>
		/// <param name="group">Group.</param>
		/// <param name="spacing">Spacing.</param>
		static public void ResizeHeightToFit(float height, List<List<RectTransform>> group, float spacing)
		{
			var transposed_group = Transpose(group);
			for (int i = 0; i < transposed_group.Count; i++)
			{
				ResizeToFit(height, transposed_group[i], spacing, RectTransform.Axis.Vertical);
			}
		}

		/// <summary>
		/// Shrink the height to fit container.
		/// </summary>
		/// <param name="height">Height.</param>
		/// <param name="group">Group.</param>
		/// <param name="spacing">Spacing.</param>
		static public void ShrinkHeightToFit(float height, List<List<RectTransform>> group, float spacing)
		{
			var transposed_group = Transpose(group);
			for (int i = 0; i < transposed_group.Count; i++)
			{
				ShrinkToFit(height, transposed_group[i], spacing, RectTransform.Axis.Vertical);
			}
		}

		/// <summary>
		/// Resizes the column width to fit container.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="group">Group.</param>
		/// <param name="spacing">Spacing.</param>
		/// <param name="padding">Padding.</param>
		static public void ResizeColumnWidthToFit(float width, List<List<RectTransform>> group, float spacing, float padding)
		{
			var transposed_group = Transpose(group);

			ResizeToFit(width - padding, transposed_group, spacing, RectTransform.Axis.Horizontal);
		}

		/// <summary>
		/// Shrink group elements if .
		/// </summary>
		/// <param name="size">Size.</param>
		/// <param name="group">Group.</param>
		/// <param name="spacing">Spacing.</param>
		/// <param name="padding">Padding.</param>
		static public void ShrinkToFit(Vector2 size, List<List<RectTransform>> group, Vector2 spacing, Padding padding)
		{
			//ResizeToFit(width - padding, transposed_group, spacing, RectTransform.Axis.Horizontal);
		}

		static public float GetShrinkScale(Vector2 requiredSize, Vector2 currentSize)
		{
			var scale = requiredSize.x / currentSize.x;
			if ((scale > 1) || ((currentSize.y * scale) > requiredSize.y))
			{
				return Math.Min(1f, requiredSize.y / currentSize.y);
			}
			return Math.Min(1f, scale);
		}

		static public Vector2 GetShrinkSize(Vector2 requiredSize, Vector2 currentSize)
		{
			if ((currentSize.x >= requiredSize.x) && (currentSize.y >= requiredSize.y))
			{
				return requiredSize;
			}
			var scale = requiredSize.x / currentSize.x;
			if ((scale > 1) || ((currentSize.y * scale) > requiredSize.y))
			{
				scale = requiredSize.y / currentSize.y;

				return new Vector2(currentSize.x * scale, requiredSize.y);
			}
			return new Vector2(requiredSize.x, currentSize.y * scale);
		}

		/// <summary>
		/// Shrink the column width to fit container.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="group">Group.</param>
		/// <param name="spacing">Spacing.</param>
		/// <param name="padding">Padding.</param>
		static public void ShrinkColumnWidthToFit(float width, List<List<RectTransform>> group, float spacing, float padding)
		{
			var transposed_group = Transpose(group);

			ShrinkToFit(width - padding, transposed_group, spacing, RectTransform.Axis.Horizontal);
		}

		/// <summary>
		/// Resizes the row height to fit container.
		/// </summary>
		/// <param name="height">Height.</param>
		/// <param name="group">Group.</param>
		/// <param name="spacing">Spacing.</param>
		/// <param name="padding">Padding.</param>
		static public void ResizeRowHeightToFit(float height, List<List<RectTransform>> group, float spacing, float padding)
		{
			ResizeToFit(height - padding, group, spacing, RectTransform.Axis.Vertical);
		}

		/// <summary>
		/// Shrink the row height to fit container.
		/// </summary>
		/// <param name="height">Height.</param>
		/// <param name="group">Group.</param>
		/// <param name="spacing">Spacing.</param>
		/// <param name="padding">Padding.</param>
		static public void ShrinkRowHeightToFit(float height, List<List<RectTransform>> group, float spacing, float padding)
		{
			ShrinkToFit(height - padding, group, spacing, RectTransform.Axis.Vertical);
		}

		/// <summary>
		/// Gets the maximum PreferredHeight.
		/// </summary>
		/// <returns>The max preferred height.</returns>
		/// <param name="column">Column.</param>
		public static float GetMaxPreferredHeight(List<RectTransform> column)
		{
			return column.Max<RectTransform,float>(GetPreferredHeight);
		}

		/// <summary>
		/// Gets the maximum MinHeight.
		/// </summary>
		/// <returns>The max minimum height.</returns>
		/// <param name="column">Column.</param>
		public static float GetMaxMinHeight(List<RectTransform> column)
		{
			return column.Max<RectTransform,float>(GetMinHeight);
		}

		/// <summary>
		/// Gets the maximum FlexibleHeight.
		/// </summary>
		/// <returns>The max flexible height.</returns>
		/// <param name="column">Column.</param>
		public static float GetMaxFlexibleHeight(List<RectTransform> column)
		{
			return column.Max<RectTransform,float>(GetFlexibleHeight);
		}

		/// <summary>
		/// Gets the maximum PreferredWidth.
		/// </summary>
		/// <returns>The max preferred width.</returns>
		/// <param name="row">Row.</param>
		public static float GetMaxPreferredWidth(List<RectTransform> row)
		{
			return row.Max<RectTransform,float>(GetPreferredWidth);
		}

		/// <summary>
		/// Gets the maximum MinWidth.
		/// </summary>
		/// <returns>The max minimum width.</returns>
		/// <param name="row">Row.</param>
		public static float GetMaxMinWidth(List<RectTransform> row)
		{
			return row.Max<RectTransform,float>(GetMinWidth);
		}

		/// <summary>
		/// Gets the maximum FlexibleWidth.
		/// </summary>
		/// <returns>The max flexible width.</returns>
		/// <param name="row">Row.</param>
		public static float GetMaxFlexibleWidth(List<RectTransform> row)
		{
			return row.Max<RectTransform,float>(GetFlexibleWidth);
		}

		/// <summary>
		/// Transpose the specified group.
		/// </summary>
		/// <param name="group">Group.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<List<T>> Transpose<T>(List<List<T>> group)
		{
			var result = new List<List<T>>();

			for (int i = 0; i < group.Count; i++)
			{
				for (int j = 0; j < group[i].Count; j++)
				{
					if (result.Count<=j)
					{
						result.Add(new List<T>());
					}
					result[j].Add(group[i][j]);
				}
			}

			return result;
		}

		static void Log(IEnumerable<float> values)
		{
			Debug.Log("[" + string.Join("; ", values.Select(x => x.ToString()).ToArray()) + "]");
		}

		/// <summary>
		/// Gets the preferred width of the RectTransform.
		/// </summary>
		/// <returns>The preferred width.</returns>
		/// <param name="rect">Rect.</param>
		public static float GetPreferredWidth(RectTransform rect)
		{
			if (rect==null)
			{
				return 0f;
			}
			if (rect.gameObject.activeInHierarchy)
			{
				return Mathf.Max(0f, LayoutUtility.GetPreferredWidth(rect));
			}
			else
			{
				float result = 0f;
				#if UNITY_4_6 || UNITY_4_7
				var elements = rect.GetComponents<Component>().OfType<ILayoutElement>();
				#else
				var elements = rect.GetComponents<ILayoutElement>();
				#endif
				var max_priority = elements.Max(x => x.layoutPriority);
				foreach (var elem in elements)
				{
					if (elem.layoutPriority==max_priority)
					{
						result = Mathf.Max(result, Mathf.Max(elem.preferredWidth, elem.minWidth));
					}
				}
				return result;
			}
		}

		/// <summary>
		/// Gets the min width of the RectTransform.
		/// </summary>
		/// <returns>The preferred width.</returns>
		/// <param name="rect">Rect.</param>
		public static float GetMinWidth(RectTransform rect)
		{
			if (rect==null)
			{
				return 0f;
			}
			if (rect.gameObject.activeInHierarchy)
			{
				return Mathf.Max(0f, LayoutUtility.GetMinWidth(rect));
			}
			else
			{
				float result = 0f;
				#if UNITY_4_6 || UNITY_4_7
				var elements = rect.GetComponents<Component>().OfType<ILayoutElement>();
				#else
				var elements = rect.GetComponents<ILayoutElement>();
				#endif
				var max_priority = elements.Max(x => x.layoutPriority);
				foreach (var elem in elements)
				{
					if (elem.layoutPriority==max_priority)
					{
						result = Mathf.Max(result, elem.minWidth);
					}
				}
				return result;
			}
		}

		/// <summary>
		/// Gets the preferred height of the RectTransform.
		/// </summary>
		/// <returns>The preferred height.</returns>
		/// <param name="rect">Rect.</param>
		public static float GetPreferredHeight(RectTransform rect)
		{
			if (rect==null)
			{
				return 0f;
			}
			if (rect.gameObject.activeInHierarchy)
			{
				return Mathf.Max(0f, LayoutUtility.GetPreferredHeight(rect));
			}
			else
			{
				float result = 0f;
				#if UNITY_4_6 || UNITY_4_7
				var elements = rect.GetComponents<Component>().OfType<ILayoutElement>();
				#else
				var elements = rect.GetComponents<ILayoutElement>();
				#endif
				var max_priority = elements.Max(x => x.layoutPriority);
				foreach (var elem in elements)
				{
					if (elem.layoutPriority==max_priority)
					{
						result = Mathf.Max(result, Mathf.Max(elem.preferredHeight, elem.minHeight));
					}
				}
				return result;
			}
		}

		/// <summary>
		/// Gets the min height of the RectTransform.
		/// </summary>
		/// <returns>The min height.</returns>
		/// <param name="rect">Rect.</param>
		public static float GetMinHeight(RectTransform rect)
		{
			if (rect==null)
			{
				return 0f;
			}
			if (rect.gameObject.activeInHierarchy)
			{
				return Mathf.Max(0f, LayoutUtility.GetMinHeight(rect));
			}
			else
			{
				float result = 0f;
				#if UNITY_4_6 || UNITY_4_7
				var elements = rect.GetComponents<Component>().OfType<ILayoutElement>();
				#else
				var elements = rect.GetComponents<ILayoutElement>();
				#endif
				var max_priority = elements.Max(x => x.layoutPriority);
				foreach (var elem in elements)
				{
					if (elem.layoutPriority==max_priority)
					{
						result = Mathf.Max(result, elem.minHeight);
					}
				}
				return result;
			}
		}

		/// <summary>
		/// Gets the flexible width of the RectTransform.
		/// </summary>
		/// <returns>The flexible width.</returns>
		/// <param name="rect">Rect.</param>
		public static float GetFlexibleWidth(RectTransform rect)
		{
			if (rect==null)
			{
				return 0f;
			}
			if (rect.gameObject.activeInHierarchy)
			{
				return Mathf.Max(0f, LayoutUtility.GetFlexibleWidth(rect));
			}
			else
			{
				float result = 0f;
				#if UNITY_4_6 || UNITY_4_7
				var elements = rect.GetComponents<Component>().OfType<ILayoutElement>();
				#else
				var elements = rect.GetComponents<ILayoutElement>();
				#endif
				var max_priority = elements.Max(x => x.layoutPriority);
				foreach (var elem in elements)
				{
					if (elem.layoutPriority==max_priority)
					{
						result = Mathf.Max(result, elem.flexibleWidth);
					}
				}
				return result;
			}
		}

		/// <summary>
		/// Gets the flexible height of the RectTransform.
		/// </summary>
		/// <returns>The flexible height.</returns>
		/// <param name="rect">Rect.</param>
		public static float GetFlexibleHeight(RectTransform rect)
		{
			if (rect==null)
			{
				return 0f;
			}
			if (rect.gameObject.activeInHierarchy)
			{
				return Mathf.Max(0f, LayoutUtility.GetFlexibleHeight(rect));
			}
			else
			{
				float result = 0f;
				#if UNITY_4_6 || UNITY_4_7
				var elements = rect.GetComponents<Component>().OfType<ILayoutElement>();
				#else
				var elements = rect.GetComponents<ILayoutElement>();
				#endif
				var max_priority = elements.Max(x => x.layoutPriority);
				foreach (var elem in elements)
				{
					if (elem.layoutPriority==max_priority)
					{
						result = Mathf.Max(result, elem.flexibleHeight);
					}
				}
				return result;
			}
		}

		/// <summary>
		/// Get scaled width.
		/// </summary>
		/// <returns>The width.</returns>
		/// <param name="ui">User interface.</param>
		public static float ScaledWidth(RectTransform ui)
		{
			return ui.rect.width * ui.localScale.x;
		}

		/// <summary>
		/// Get scaled height.
		/// </summary>
		/// <returns>The height.</returns>
		/// <param name="ui">User interface.</param>
		public static float ScaledHeight(RectTransform ui)
		{
			return ui.rect.height * ui.localScale.y;
		}
	}
}