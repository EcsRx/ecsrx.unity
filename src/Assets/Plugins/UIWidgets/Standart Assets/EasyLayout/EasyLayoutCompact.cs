using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EasyLayout {
	/// <summary>
	/// EasyLayout compact layout.
	/// </summary>
	public static class EasyLayoutCompact {

		/// <summary>
		/// Group the specified uiElements.
		/// </summary>
		/// <param name="uiElements">User interface elements.</param>
		/// <param name="baseLength">Base length (width or height).</param>
		/// <param name="layout">Layout.</param>
		/// <param name="group">Result</param>
		public static void Group(List<RectTransform> uiElements, float baseLength, EasyLayout layout, List<List<RectTransform>> group)
		{
			var length = baseLength;
			
			var spacing = (layout.Stacking==Stackings.Horizontal) ? layout.Spacing.x : layout.Spacing.y;
			
			var row = layout.GetRectTransformList();
			
			for (int i = 0; i < uiElements.Count; i++)
			{
				var ui_length = layout.GetLength(uiElements[i]);
				
				if (row.Count == 0)
				{
					length -= ui_length;
					row.Add(uiElements[i]);
					continue;
				}
				
				if (length >= (ui_length + spacing))
				{
					length -= ui_length + spacing;
					row.Add(uiElements[i]);
				}
				else
				{
					group.Add(row);
					length = baseLength;
					length -= ui_length;
					row = layout.GetRectTransformList();
					row.Add(uiElements[i]);
				}
			}
			if (row.Count > 0)
			{
				group.Add(row);
			}
		}
	}
}