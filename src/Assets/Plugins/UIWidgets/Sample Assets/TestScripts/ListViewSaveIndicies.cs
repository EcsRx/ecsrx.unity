using UnityEngine;
using UIWidgets;
using System.Collections.Generic;
using System.Linq;
 
namespace UIWidgetsSamples {
	[RequireComponent(typeof(ListViewBase))]
	public class ListViewSaveIndicies : MonoBehaviour {
		[SerializeField]
		public string Key = "Unique Key";
		
		[SerializeField]
		ListViewBase list;
		
		void Start()
		{
			list = GetComponent<ListViewBase>();
			list.Start();
			
			LoadIndicies();
			
			list.OnSelect.AddListener(SaveIndicies);
			list.OnDeselect.AddListener(SaveIndicies);
		}
		
		void LoadIndicies()
		{
			if (PlayerPrefs.HasKey(Key))
			{
				var indicies = String2Indicies(PlayerPrefs.GetString(Key));
				indicies.RemoveAll(x => !list.IsValid(x));
				list.SelectedIndicies = indicies;
			}
		}
		
		void SaveIndicies(int index, ListViewItem component)
		{
			PlayerPrefs.SetString(Key, Indicies2String(list.SelectedIndicies));
		}
		
		List<int> String2Indicies(string str)
		{
			if (str=="")
			{
				return new List<int>();
			}
			return str.Split(',').Select(x => int.Parse(x)).ToList();
		}
		
		string Indicies2String(List<int> indicies)
		{
			if ((indicies==null) || (indicies.Count==0))
			{
				return "";
			}
			return string.Join(",", indicies.Select(x => x.ToString()).ToArray());
		}
	}
}