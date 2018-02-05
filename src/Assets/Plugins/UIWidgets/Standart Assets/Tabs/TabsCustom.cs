using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

namespace UIWidgets
{
	/// <summary>
	/// Base class for creating own tabs.
	/// </summary>
	public class TabsCustom<TTab,TButton> : MonoBehaviour
		where TTab : Tab
		where TButton : TabButton
	{
		/// <summary>
		/// The container for tab toggle buttons.
		/// </summary>
		[SerializeField]
		public Transform Container;
		
		/// <summary>
		/// The default tab button.
		/// </summary>
		[SerializeField]
		public TButton DefaultTabButton;
		
		/// <summary>
		/// The active tab button.
		/// </summary>
		[SerializeField]
		public TButton ActiveTabButton;
		
		[SerializeField]
		TTab[] tabObjects = new TTab[]{};
		
		/// <summary>
		/// Gets or sets the tab objects.
		/// </summary>
		/// <value>The tab objects.</value>
		public TTab[] TabObjects {
			get {
				return tabObjects;
			}
			set {
				tabObjects = value;
				UpdateButtons();
			}
		}

		/// <summary>
		/// The name of the default tab.
		/// </summary>
		[SerializeField]
		[Tooltip("Tab name which will be active by default, if not specified will be opened first Tab.")]
		public string DefaultTabName = string.Empty;

		/// <summary>
		/// If true does not deactivate hidden tabs.
		/// </summary>
		[SerializeField]
		[Tooltip("If true does not deactivate hidden tabs.")]
		public bool KeepTabsActive = false;

		/// <summary>
		/// OnTabSelect event.
		/// </summary>
		[SerializeField]
		public TabSelectEvent OnTabSelect = new TabSelectEvent();

		/// <summary>
		/// Gets or sets the selected tab.
		/// </summary>
		/// <value>The selected tab.</value>
		public TTab SelectedTab {
			get;
			protected set;
		}

		List<TButton> defaultButtons = new List<TButton>();
		List<TButton> activeButtons = new List<TButton>();
		List<UnityAction> callbacks = new List<UnityAction>();

		bool isStarted;
		/// <summary>
		/// Start this instance.
		/// </summary>
		public void Start()
		{
			if (isStarted)
			{
				return ;
			}
			isStarted = true;
			if (Container==null)
			{
				throw new NullReferenceException("Container is null. Set object of type GameObject to Container.");
			}
			if (DefaultTabButton==null)
			{
				throw new NullReferenceException("DefaultTabButton is null. Set object of type GameObject to DefaultTabButton.");
			}
			if (ActiveTabButton==null)
			{
				throw new NullReferenceException("ActiveTabButton is null. Set object of type GameObject to ActiveTabButton.");
			}
			DefaultTabButton.gameObject.SetActive(false);
			ActiveTabButton.gameObject.SetActive(false);
			
			UpdateButtons();
		}

		/// <summary>
		/// Updates the buttons.
		/// </summary>
		void UpdateButtons()
		{
			if (tabObjects.Length==0)
			{
				throw new ArgumentException("TabObjects array is empty. Fill it.");
			}
			
			RemoveCallbacks();

			CreateButtons();

			AddCallbacks();

			if (DefaultTabName!="")
			{
				var tab = GetTabByName(DefaultTabName);
				if (tab!=null)
				{
					SelectTab(tab);
				}
				else
				{
					Debug.LogWarning(string.Format("Tab with specified DefaultTabName \"{0}\" not found. Opened first Tab.", DefaultTabName), this);
					SelectTab(tabObjects[0]);
				}
			}
			else
			{
				SelectTab(tabObjects[0]);
			}
		}

		TTab GetTabByName(string tabName)
		{
			return tabObjects.FirstOrDefault(x => x.Name==tabName);
		}

		void AddCallback(TTab tab, int index)
		{
			UnityAction callback = () => SelectTab(tab);
			callbacks.Add(callback);
			
			defaultButtons[index].onClick.AddListener(callbacks[index]);
		}

		void AddCallbacks()
		{
			tabObjects.ForEach(AddCallback);
		}

		void RemoveCallback(TTab tab, int index)
		{
			if ((tab!=null) && (index < callbacks.Count))
			{
				defaultButtons[index].onClick.RemoveListener(callbacks[index]);
			}
		}

		void RemoveCallbacks()
		{
			if (callbacks.Count > 0)
			{
				tabObjects.ForEach(RemoveCallback);
				callbacks.Clear();
			}
		}

		void OnDestroy()
		{
			RemoveCallbacks();
		}

		/// <summary>
		/// Selects the tab.
		/// </summary>
		/// <param name="tabName">Tab name.</param>
		public void SelectTab(string tabName)
		{
			var tab = GetTabByName(tabName);
			if (tab!=null)
			{
				SelectTab(tab);
			}
			else
			{
				Debug.LogWarning(string.Format("Tab with specified name \"{0}\" not found.", tabName), this);
			}
		}

		/// <summary>
		/// Selects the tab.
		/// </summary>
		/// <param name="tab">Tab.</param>
		public void SelectTab(TTab tab)
		{
			var index = Array.IndexOf(tabObjects, tab);
			if (index==-1)
			{
				throw new ArgumentException(string.Format("Tab with name \"{0}\" not found.", tab.Name));
			}

			SelectedTab = tabObjects[index];

			if (KeepTabsActive)
			{
				tabObjects[index].TabObject.transform.SetAsLastSibling();
			}
			else
			{
				tabObjects.ForEach(DeactivateTab);
				tabObjects[index].TabObject.SetActive(true);
			}
			
			defaultButtons.ForEach(ActivateButton);
			defaultButtons[index].gameObject.SetActive(false);
			
			activeButtons.ForEach(DeactivateButton);
			activeButtons[index].gameObject.SetActive(true);

			OnTabSelect.Invoke(index);
		}

		void DeactivateTab(TTab tab)
		{
			tab.TabObject.SetActive(false);
		}

		void ActivateButton(TButton button)
		{
			button.gameObject.SetActive(true);
		}

		void DeactivateButton(TButton button)
		{
			button.gameObject.SetActive(false);
		}

		/// <summary>
		/// Creates the buttons.
		/// </summary>
		void CreateButtons()
		{
			if (tabObjects.Length > defaultButtons.Count)
			{
				for (var i = defaultButtons.Count; i < tabObjects.Length; i++)
				{
					var defaultButton = Instantiate(DefaultTabButton) as TButton;
					defaultButton.transform.SetParent(Container, false);
					
					//Utilites.FixInstantiated(DefaultTabButton, defaultButton);
					
					defaultButtons.Add(defaultButton);
					
					var activeButton = Instantiate(ActiveTabButton) as TButton;
					activeButton.transform.SetParent(Container, false);
					
					//Utilites.FixInstantiated(ActiveTabButton, activeButton);
					
					activeButtons.Add(activeButton);
				}
			}
			//del existing ui elements if necessary
			if (tabObjects.Length < defaultButtons.Count)
			{
				for (var i = defaultButtons.Count; i > tabObjects.Length; i--)
				{
					Destroy(defaultButtons[i]);
					Destroy(activeButtons[i]);
					
					defaultButtons.RemoveAt(i);
					activeButtons.RemoveAt(i);
				}
			}
			
			defaultButtons.ForEach(SetButtonData);
			activeButtons.ForEach(SetButtonData);
		}

		/// <summary>
		/// Sets the name of the button.
		/// </summary>
		/// <param name="button">Button.</param>
		/// <param name="index">Index.</param>
		protected virtual void SetButtonData(TButton button, int index)
		{
			//button.SetData(tabObjects[index]);
		}
	}
}