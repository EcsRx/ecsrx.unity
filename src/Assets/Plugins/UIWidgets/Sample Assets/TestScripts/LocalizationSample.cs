using UnityEngine;
using System.Collections;
using UIWidgets;

namespace UIWidgetsSamples {
	/// <summary>
	/// Sample script how to add localization for ListViewIcons, ListViewCustom, TileView, TreeView.
	/// </summary>
	public class LocalizationSample : MonoBehaviour {
		[SerializeField]
		ListViewIcons targetListViewIcons;

		[SerializeField]
		ListView targetListView;

		public ListViewIcons TargetListViewIcons {
			get {
				if (targetListViewIcons==null)
				{
					targetListViewIcons = GetComponent<ListViewIcons>();
				}
				return targetListViewIcons;
			}
		}
		
		void Start()
		{
			TargetListViewIcons.Start();


			Localize();

			// Add callback on language change, if localization system support this.
			//LocalizationSystem.OnLanguageChange += Localize;
			//LocalizationSystem.OnLanguageChange.AddListener(Localize);
		}
		
		public void Localize()
		{
			// get localized strings, instead GetLocalizedString() use similar function from localization system
			TargetListViewIcons.DataSource.ForEach(x => x.LocalizedName = GetLocalizedString(x.Name));

			// update items in ListViewIcons
			// - or -
			// update ListViewCustom, TileView, TreeViewCustom
			// don't need to call for TreeView or simple ListView
			TargetListViewIcons.UpdateItems();
		}

		string GetLocalizedString(string str)
		{
			return str;
		}

		void OnDestroy()
		{
			// Remove callback on language change, if localization system support this.
			//LocalizationSystem.OnLanguageChange -= Localize;
			//LocalizationSystem.OnLanguageChange.RemoveListener(Localize);
		}

		/*
		ObservableList<string> originalList;
		void StartListView()
		{
			targetListView.Start();
			
			// keep original list
			originalList = targetListView.DataSource;
			
			Localize();
		}
		
		public void LocalizeListView()
		{
			targetListView.DataSource = originalList.Convert(x => GetLocalizedString(x));
		}
		*/
	}
}