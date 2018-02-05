using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System;

namespace UIWidgets {

	/// <summary>
	/// Base class for custom combobox.
	/// </summary>
	public class ComboboxCustom<TListViewCustom,TComponent,TItem> : MonoBehaviour, ISubmitHandler
			where TListViewCustom : ListViewCustom<TComponent,TItem>
			where TComponent : ListViewItem
	{
		/// <summary>
		/// Custom Combobox event.
		/// </summary>
		[System.Serializable]
		public class ComboboxCustomEvent : UnityEvent<int,TItem>
		{
			
		}

		[SerializeField]
		TListViewCustom listView;
		
		/// <summary>
		/// Gets or sets the ListView.
		/// </summary>
		/// <value>ListView component.</value>
		public TListViewCustom ListView {
			get {
				return listView;
			}
			set {
				SetListView(value);
			}
		}
		
		[SerializeField]
		Button toggleButton;
		
		/// <summary>
		/// Gets or sets the toggle button.
		/// </summary>
		/// <value>The toggle button.</value>
		public Button ToggleButton {
			get {
				return toggleButton;
			}
			set {
				SetToggleButton(value);
			}
		}

		[SerializeField]
		TComponent current;

		/// <summary>
		/// Gets or sets the current component.
		/// </summary>
		/// <value>The current.</value>
		public TComponent Current {
			get {
				return current;
			}
			set {
				current = value;
			}
		}
		
		/// <summary>
		/// OnSelect event.
		/// </summary>
		public ComboboxCustomEvent OnSelect = new ComboboxCustomEvent();
		UnityAction<int> onSelectCallback;

		Transform listCanvas;
		Transform listParent;

		/// <summary>
		/// The components list.
		/// </summary>
		protected List<TComponent> components = new List<TComponent>();

		/// <summary>
		/// The components cache list.
		/// </summary>
		protected List<TComponent> componentsCache = new List<TComponent>();

		void Awake()
		{
			ListView.Sort = false;
			Start();
		}
		
		[System.NonSerialized]
		bool isStartedComboboxCustom;

		/// <summary>
		/// Start this instance.
		/// </summary>
		public virtual void Start()
		{
			if (isStartedComboboxCustom)
			{
				return ;
			}
			isStartedComboboxCustom = true;

			onSelectCallback = index => OnSelect.Invoke(index, listView.DataSource[index]);

			SetToggleButton(toggleButton);

			SetListView(listView);

			if (listView!=null)
			{
				current.gameObject.SetActive(false);

				listView.OnSelectObject.RemoveListener(UpdateView);
				listView.OnDeselectObject.RemoveListener(UpdateView);

				listCanvas = Utilites.FindTopmostCanvas(listParent);

				listView.gameObject.SetActive(true);
				listView.Start();
				if ((listView.SelectedIndex==-1) && (listView.DataSource.Count > 0) && (!listView.Multiple))
				{
					listView.SelectedIndex = 0;
				}
				if (listView.SelectedIndex!=-1)
				{
					UpdateView();
				}
				listView.gameObject.SetActive(false);

				listView.OnSelectObject.AddListener(UpdateView);
				listView.OnDeselectObject.AddListener(UpdateView);
			}
		}

		/// <summary>
		/// Sets the toggle button.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetToggleButton(Button value)
		{
			if (toggleButton!=null)
			{
				toggleButton.onClick.RemoveListener(ToggleList);
			}
			toggleButton = value;
			if (toggleButton!=null)
			{
				toggleButton.onClick.AddListener(ToggleList);
			}
		}

		/// <summary>
		/// Sets the list view.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetListView(TListViewCustom value)
		{
			if (listView!=null)
			{
				listParent = null;
				
				listView.OnSelectObject.RemoveListener(UpdateView);
				listView.OnDeselectObject.RemoveListener(UpdateView);
				listView.OnSelectObject.RemoveListener(onSelectCallback);
				
				listView.OnFocusOut.RemoveListener(onFocusHideList);
				
				listView.onCancel.RemoveListener(OnListViewCancel);
				listView.onItemCancel.RemoveListener(OnListViewCancel);
				
				RemoveDeselectCallbacks();
			}
			listView = value;
			if (listView!=null)
			{
				listParent = listView.transform.parent;
				
				listView.OnSelectObject.AddListener(UpdateView);
				listView.OnDeselectObject.AddListener(UpdateView);
				listView.OnSelectObject.AddListener(onSelectCallback);
				
				listView.OnFocusOut.AddListener(onFocusHideList);
				
				listView.onCancel.AddListener(OnListViewCancel);
				listView.onItemCancel.AddListener(OnListViewCancel);
				
				AddDeselectCallbacks();
			}
		}

		/// <summary>
		/// Set the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="allowDuplicate">If set to <c>true</c> allow duplicate.</param>
		/// <returns>Index of item.</returns>
		public virtual int Set(TItem item, bool allowDuplicate=true)
		{
			return listView.Set(item, allowDuplicate);
		}

		/// <summary>
		/// Clear listview and selected item.
		/// </summary>
		public virtual void Clear()
		{
			listView.Clear();
			UpdateView();
		}

		/// <summary>
		/// Toggles the list visibility.
		/// </summary>
		public virtual void ToggleList()
		{
			if (listView==null)
			{
				return ;
			}

			if (listView.gameObject.activeSelf)
			{
				HideList();
			}
			else
			{
				ShowList();
			}
		}

		/// <summary>
		/// The modal key.
		/// </summary>
		protected int? ModalKey;

		/// <summary>
		/// Shows the list.
		/// </summary>
		public virtual void ShowList()
		{
			if (listView==null)
			{
				return ;
			}

			ModalKey = ModalHelper.Open(this, null, new Color(0, 0, 0, 0f), HideList);

			if (listCanvas!=null)
			{
				listParent = listView.transform.parent;
				listView.transform.SetParent(listCanvas);
			}

			listView.gameObject.SetActive(true);

			if (listView.Layout!=null)
			{
				listView.Layout.UpdateLayout();
			}

			if (listView.SelectComponent())
			{
				SetChildDeselectListener(EventSystem.current.currentSelectedGameObject);
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(listView.gameObject);
			}
		}

		/// <summary>
		/// Hides the list.
		/// </summary>
		public virtual void HideList()
		{
			if (ModalKey!=null)
			{
				ModalHelper.Close((int)ModalKey);
				ModalKey = null;
			}

			if (listView==null)
			{
				return ;
			}

			if (listCanvas!=null)
			{
				listView.transform.SetParent(listParent);
			}
			listView.gameObject.SetActive(false);
		}

		/// <summary>
		/// The children deselect.
		/// </summary>
		protected List<SelectListener> childrenDeselect = new List<SelectListener>();

		/// <summary>
		/// Hide list when focus lost.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected void onFocusHideList(BaseEventData eventData)
		{
			if (eventData.selectedObject==gameObject)
			{
				return ;
			}

			var ev_item = eventData as ListViewItemEventData;
			if (ev_item!=null)
			{
				if (ev_item.NewSelectedObject!=null)
				{
					SetChildDeselectListener(ev_item.NewSelectedObject);
				}
				return ;
			}

			var ev = eventData as PointerEventData;
			if (ev==null)
			{
				HideList();
				return ;
			}

			var go = ev.pointerPressRaycast.gameObject;//ev.pointerEnter
			if (go==null)
			{
				HideList();
				return ;
			}

			if (go.Equals(toggleButton.gameObject))
			{
				return ;
			}
			if (go.transform.IsChildOf(listView.transform))
			{
				SetChildDeselectListener(go);
				return ;
			}

			HideList();
		}

		/// <summary>
		/// Sets the child deselect listener.
		/// </summary>
		/// <param name="child">Child.</param>
		protected void SetChildDeselectListener(GameObject child)
		{
			var deselectListener = GetDeselectListener(child);
			if (!childrenDeselect.Contains(deselectListener))
			{
				deselectListener.onDeselect.AddListener(onFocusHideList);
				childrenDeselect.Add(deselectListener);
			}
		}

		/// <summary>
		/// Gets the deselect listener.
		/// </summary>
		/// <returns>The deselect listener.</returns>
		/// <param name="go">Go.</param>
		protected SelectListener GetDeselectListener(GameObject go)
		{
			var result = go.GetComponent<SelectListener>();
			return (result!=null) ? result : go.AddComponent<SelectListener>();
		}

		/// <summary>
		/// Adds the deselect callbacks.
		/// </summary>
		protected void AddDeselectCallbacks()
		{
			if (listView.ScrollRect==null)
			{
				return ;
			}
			if (listView.ScrollRect.verticalScrollbar==null)
			{
				return ;
			}
			var scrollbar = listView.ScrollRect.verticalScrollbar.gameObject;
			var deselectListener = GetDeselectListener(scrollbar);

			deselectListener.onDeselect.AddListener(onFocusHideList);
			childrenDeselect.Add(deselectListener);
		}

		/// <summary>
		/// Removes the deselect callbacks.
		/// </summary>
		protected void RemoveDeselectCallbacks()
		{
			childrenDeselect.ForEach(RemoveDeselectCallback);
			childrenDeselect.Clear();
		}

		/// <summary>
		/// Removes the deselect callback.
		/// </summary>
		/// <param name="listener">Listener.</param>
		protected void RemoveDeselectCallback(SelectListener listener)
		{
			if (listener!=null)
			{
				listener.onDeselect.RemoveListener(onFocusHideList);
			}
		}

		void UpdateView(int index)
		{
			UpdateView();
		}

		/// <summary>
		/// The current indicies.
		/// </summary>
		protected List<int> currentIndicies;

		/// <summary>
		/// Updates the view.
		/// </summary>
		protected virtual void UpdateView()
		{
			currentIndicies = ListView.SelectedIndicies;

			UpdateComponentsCount();

			components.ForEach(SetData);
			HideList();

			if ((EventSystem.current!=null) && (!EventSystem.current.alreadySelecting))
			{
				EventSystem.current.SetSelectedGameObject(gameObject);
			}
		}

		/// <summary>
		/// Sets the data.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <param name="i">The index.</param>
		protected virtual void SetData(TComponent component, int i)
		{
			component.Index = currentIndicies[i];
			SetData(component, ListView.DataSource[currentIndicies[i]]);
		}

		/// <summary>
		/// Sets component data with specified item.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <param name="item">Item.</param>
		protected virtual void SetData(TComponent component, TItem item)
		{

		}

		/// <summary>
		/// Hide list view.
		/// </summary>
		void OnListViewCancel()
		{
			HideList();
		}

		/// <summary>
		/// Adds the component.
		/// </summary>
		/// <param name="index">Index.</param>
		protected virtual void AddComponent(int index)
		{
			TComponent component;
			if (componentsCache.Count > 0)
			{
				component = componentsCache[componentsCache.Count - 1];
				componentsCache.RemoveAt(componentsCache.Count - 1);
			}
			else
			{
				component = Instantiate(current) as TComponent;
				component.transform.SetParent(current.transform.parent, false);

				Utilites.FixInstantiated(current, component);
			}
			component.Index = -1;
			component.transform.SetAsLastSibling();
			component.gameObject.SetActive(true);
			components.Add(component);
		}

		/// <summary>
		/// Deactivates the component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected virtual void DeactivateComponent(TComponent component)
		{
			if (component!=null)
			{
				component.MovedToCache();
				component.Index = -1;
				component.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Updates the components count.
		/// </summary>
		protected void UpdateComponentsCount()
		{
			components.RemoveAll(IsNullComponent);

			if (components.Count==currentIndicies.Count)
			{
				return ;
			}
			
			if (components.Count < currentIndicies.Count)
			{
				componentsCache.RemoveAll(IsNullComponent);
				
				Enumerable.Range(0, currentIndicies.Count - components.Count).ForEach(AddComponent);
			}
			else
			{
				var to_cache = components.GetRange(currentIndicies.Count, components.Count - currentIndicies.Count).OrderByDescending<TComponent,int>(GetComponentIndex);
				
				to_cache.ForEach(DeactivateComponent);
				componentsCache.AddRange(to_cache);
				components.RemoveRange(currentIndicies.Count, components.Count - currentIndicies.Count);
			}
		}

		/// <summary>
		/// Determines whether the specified component is null.
		/// </summary>
		/// <returns><c>true</c> if the specified component is null; otherwise, <c>false</c>.</returns>
		/// <param name="component">Component.</param>
		protected bool IsNullComponent(TComponent component)
		{
			return component==null;
		}

		/// <summary>
		/// Gets the index of the component.
		/// </summary>
		/// <returns>The component index.</returns>
		/// <param name="item">Item.</param>
		protected int GetComponentIndex(TComponent item)
		{
			return item.Index;
		}

		/// <summary>
		/// Raises the submit event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnSubmit(BaseEventData eventData)
		{
			ShowList();
		}

		/// <summary>
		/// Updates the current component.
		/// </summary>
		[Obsolete("Use SetData() instead.")]
		protected virtual void UpdateCurrent()
		{
			HideList();
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			ListView = null;
			ToggleButton = null;
		}
	}
}