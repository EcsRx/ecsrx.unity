using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;
using UnityEngine.Serialization;

namespace UIWidgets {

	/// <summary>
	/// ListViewIcons item description.
	/// </summary>
	[System.Serializable]
	public class ListViewIconsItemDescription : INotifyPropertyChanged {
		[SerializeField]
		[FormerlySerializedAs("Icon")]
		Sprite icon;

		/// <summary>
		/// The icon.
		/// </summary>
		public Sprite Icon {
			get {
				return icon;
			}
			set {
				icon = value;
				Changed("Icon");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("Name")]
		string name;

		/// <summary>
		/// The name.
		/// </summary>
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
				Changed("Name");
			}
		}

		[System.NonSerialized]
		string localizedName;

		/// <summary>
		/// The localized name.
		/// </summary>
		public string LocalizedName {
			get {
				return localizedName;
			}
			set {
				localizedName = value;
				Changed("LocalizedName");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("Value")]
		int val;

		/// <summary>
		/// The value.
		/// </summary>
		public int Value {
			get {
				return val;
			}
			set {
				val = value;
				Changed("Value");
			}
		}

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged = (x, y) => { };

		void Changed(string propertyName)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	/// <summary>
	/// ListViewIcons.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/ListViewIcons")]
	public class ListViewIcons : ListViewCustom<ListViewIconsItemComponent,ListViewIconsItemDescription> {
		/// <summary>
		/// Awake this instance.
		/// </summary>
		protected override void Awake()
		{
			Start();
		}

		[System.NonSerialized]
		bool isStartedListViewIcons = false;

		protected Comparison<ListViewIconsItemDescription> ItemsComparison =
			(x, y) => (x.LocalizedName ?? x.Name).CompareTo(y.LocalizedName ?? y.Name);

		/// <summary>
		/// Start this instance.
		/// </summary>
		public override void Start()
		{
			if (isStartedListViewIcons)
			{
				return ;
			}
			isStartedListViewIcons = true;

			base.Start();
			SortFunc = list => list.OrderBy(item => item.LocalizedName ?? item.Name);
			//DataSource.Comparison = ItemsComparison;
		}

		/// <summary>
		/// Sets component data with specified item.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <param name="item">Item.</param>
		protected override void SetData(ListViewIconsItemComponent component, ListViewIconsItemDescription item)
		{
			component.SetData(item);
		}

		/// <summary>
		/// Set highlights colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected override void HighlightColoring(ListViewIconsItemComponent component)
		{
			component.HighlightColoring(HighlightedColor, HighlightedBackgroundColor, FadeDuration);
		}

		/// <summary>
		/// Set select colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected override void SelectColoring(ListViewIconsItemComponent component)
		{
			component.SelectColoring(SelectedColor, SelectedBackgroundColor, FadeDuration);
		}

		/// <summary>
		/// Set default colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected override void DefaultColoring(ListViewIconsItemComponent component)
		{
			if (component!=null)
			{
				component.DefaultColoring(DefaultColor, DefaultBackgroundColor, FadeDuration);
			}
		}
	}
}