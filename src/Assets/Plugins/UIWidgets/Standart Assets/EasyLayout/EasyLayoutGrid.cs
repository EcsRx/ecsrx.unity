using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace EasyLayout {
	/// <summary>
	/// Easy layout grid layout.
	/// </summary>
	public static class EasyLayoutGrid {

		/// <summary>
		/// Gets the max columns count.
		/// </summary>
		/// <returns>The max columns count.</returns>
		/// <param name="uiElements">User interface elements.</param>
		/// <param name="baseLength">Base length.</param>
		/// <param name="layout">Layout.</param>
		/// <param name="maxColumns">Max columns.</param>
		public static int GetMaxColumnsCount(List<RectTransform> uiElements, float baseLength, EasyLayout layout, int maxColumns)
		{
			var length = baseLength;
			var spacing = (layout.Stacking==Stackings.Horizontal) ? layout.Spacing.x : layout.Spacing.y;
			
			bool min_columns_setted = false;
			int min_columns = maxColumns;
			int current_columns = 0;
			
			for (int i = 0; i < uiElements.Count; i++)
			{
				var ui_length = layout.GetLength(uiElements[i]);
				
				if (current_columns==maxColumns)
				{
					min_columns_setted = true;
					min_columns = Mathf.Min(min_columns, current_columns);
					
					current_columns = 1;
					length = baseLength - ui_length;
					continue;
				}
				if (current_columns == 0)
				{
					current_columns = 1;
					length = baseLength - ui_length;
					continue;
				}
				
				if (length >= (ui_length + spacing))
				{
					length -= ui_length + spacing;
					current_columns++;
				}
				else
				{
					min_columns_setted = true;
					min_columns = Mathf.Min(min_columns, current_columns);
					
					current_columns = 1;
					length = baseLength - ui_length;
				}
			}
			if (!min_columns_setted)
			{
				min_columns = current_columns;
			}
			
			return min_columns;
		}

		/// <summary>
		/// Group the specified uiElements with fixed columns count.
		/// </summary>
		/// <param name="uiElements">User interface elements.</param>
		/// <param name="layout">Layout.</param>
		/// <param name="max_columns">Maximum columns count.</param>
		/// <param name="group">Result</param>
		public static void GroupByColumnsVertical(List<RectTransform> uiElements, EasyLayout layout, int max_columns, List<List<RectTransform>> group)
		{
			int i = 0;
			for (int column = 0; column < max_columns; column++)
			{
				int max_rows = Mathf.CeilToInt(((float)(uiElements.Count - i)) / ((float)(max_columns - column)));
				for (int row = 0; row < max_rows; row++)
				{
					if (row==group.Count)
					{
						group.Add(layout.GetRectTransformList());
					}
					group[row].Add(uiElements[i]);
					i++;
				}
			}
		}

		/// <summary>
		/// Group the specified uiElements with fixed columns count.
		/// </summary>
		/// <param name="uiElements">User interface elements.</param>
		/// <param name="layout">Layout.</param>
		/// <param name="max_columns">Maximum columns count.</param>
		/// <param name="group">Result</param>
		public static void GroupByColumnsHorizontal(List<RectTransform> uiElements, EasyLayout layout, int max_columns, List<List<RectTransform>> group)
		{
			int column = -1;
			for (int i = 0; i < uiElements.Count; i++)
			{
				if ((i % max_columns)==0)
				{
					group.Add(layout.GetRectTransformList());
					column++;
				}
				group[column].Add(uiElements[i]);
			}
		}

		/// <summary>
		/// Group the specified uiElements with fixed rows count.
		/// </summary>
		/// <param name="uiElements">User interface elements.</param>
		/// <param name="layout">Layout.</param>
		/// <param name="max_rows">Maximum rows count.</param>
		/// <param name="group">Result</param>
		public static void GroupByRowsVertical(List<RectTransform> uiElements, EasyLayout layout, int max_rows, List<List<RectTransform>> group)
		{
			for (int i = 0; i < uiElements.Count; i++)
			{
				int row = i % max_rows;
				if (group.Count==row)
				{
					group.Add(layout.GetRectTransformList());
				}
				group[row].Add(uiElements[i]);
			}
		}

		/// <summary>
		/// Group the specified uiElements with fixed rows count.
		/// </summary>
		/// <param name="uiElements">User interface elements.</param>
		/// <param name="layout">Layout.</param>
		/// <param name="max_rows">Maximum rows count.</param>
		/// <param name="group">Result</param>
		public static void GroupByRowsHorizontal(List<RectTransform> uiElements, EasyLayout layout, int max_rows, List<List<RectTransform>> group)
		{
			int i = 0;
			for (int row = 0; row < max_rows; row++)
			{
				group.Add(layout.GetRectTransformList());

				int max_columns = Mathf.CeilToInt((float)(uiElements.Count - i) / (float)(max_rows - row));
				for (int column = 0; column < max_columns; column++)
				{
					group[row].Add(uiElements[i]);
					i++;
				}
			}
		}

		/// <summary>
		/// Group the specified uiElements.
		/// </summary>
		/// <param name="uiElements">User interface elements.</param>
		/// <param name="baseLength">Base length (width or size).</param>
		/// <param name="layout">Layout.</param>
		/// <param name="group">Result</param>
		public static void GroupFlexible(List<RectTransform> uiElements, float baseLength, EasyLayout layout, List<List<RectTransform>> group)
		{
			int max_columns = 999999;
			while (true)
			{
				var new_max_columns = GetMaxColumnsCount(uiElements, baseLength, layout, max_columns);
				
				if ((max_columns==new_max_columns) || (new_max_columns==1))
				{
					break;
				}
				max_columns = new_max_columns;
			}

			if (layout.Stacking==Stackings.Horizontal)
			{
				GroupByColumnsHorizontal(uiElements, layout, max_columns, group);
			}
			else
			{
				GroupByRowsVertical(uiElements, layout, max_columns, group);
			}
		}
	}
}