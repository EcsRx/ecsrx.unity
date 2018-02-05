using UnityEngine;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// Autocomplete for ListViewIcons.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/AutocompleteIcons")]
	[RequireComponent(typeof(ListViewIcons))]
	public class AutocompleteIcons : AutocompleteCustom<ListViewIconsItemDescription,ListViewIconsItemComponent,ListViewIcons> {
		/// <summary>
		/// Determines whether the beginnig of value matches the Input.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>true if beginnig of value matches the Input; otherwise, false.</returns>
		public override bool Startswith(ListViewIconsItemDescription value)
		{
			if (CaseSensitive)
			{
				return value.Name.StartsWith(Input) || (value.LocalizedName!=null && value.LocalizedName.StartsWith(Input));
			}
			return value.Name.ToLower().StartsWith(Input.ToLower()) || (value.LocalizedName!=null && value.LocalizedName.ToLower().StartsWith(Input.ToLower()));
		}

		/// <summary>
		/// Returns a value indicating whether Input occurs within specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>true if the Input occurs within value parameter; otherwise, false.</returns>
		public override bool Contains(ListViewIconsItemDescription value)
		{
			if (CaseSensitive)
			{
				return value.Name.Contains(Input) || (value.LocalizedName!=null && value.LocalizedName.Contains(Input));
			}
			return value.Name.ToLower().Contains(Input.ToLower()) || (value.LocalizedName!=null && value.LocalizedName.ToLower().Contains(Input.ToLower()));
		}

		/// <summary>
		/// Convert value to string.
		/// </summary>
		/// <returns>The string value.</returns>
		/// <param name="value">Value.</param>
		protected override string GetStringValue(ListViewIconsItemDescription value)
		{
			return value.LocalizedName ?? value.Name;
		}
	}
}