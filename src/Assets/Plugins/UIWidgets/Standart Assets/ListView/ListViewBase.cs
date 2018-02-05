using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UIWidgets
{
	/// <summary>
	/// ListViewBase event.
	/// </summary>
	[Serializable]
	public class ListViewBaseEvent : UnityEvent<int, ListViewItem> {
	}

	/// <summary>
	/// ListViewFocus event.
	/// </summary>
	[Serializable]
	public class ListViewFocusEvent : UnityEvent<BaseEventData> {

	}

	/// <summary>
	/// ListViewBase.
	/// You can use it for creating custom ListViews.
	/// </summary>
	abstract public class ListViewBase : MonoBehaviour,
			ISelectHandler, IDeselectHandler,
			ISubmitHandler, ICancelHandler
	{

		[SerializeField]
		[HideInInspector]
		List<ListViewItem> items = new List<ListViewItem>();

		List<UnityAction> callbacks = new List<UnityAction>();

		/// <summary>
		/// Gets or sets the items.
		/// </summary>
		/// <value>Items.</value>
		public List<ListViewItem> Items {
			get {
				return new List<ListViewItem>(items);
			}
			set {
				UpdateItems(value);
			}
		}

		/// <summary>
		/// The destroy game objects after setting new items.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		public bool DestroyGameObjects = true;

		/// <summary>
		/// Allow select multiple items.
		/// </summary>
		[SerializeField]
		public bool Multiple;

		[SerializeField]
		int selectedIndex = -1;

		/// <summary>
		/// Gets or sets the index of the selected item.
		/// </summary>
		/// <value>The index of the selected.</value>
		public int SelectedIndex {
			get {
				return selectedIndex;
			}
			set {
				if (value==-1)
				{
					if (selectedIndex!=-1)
					{
						Deselect(selectedIndex);
					}

					selectedIndex = value;
				}
				else
				{
					Select(value);
				}
			}
		}

		[SerializeField]
		List<int> selectedIndicies = new List<int>();

		/// <summary>
		/// Gets or sets indicies of the selected items.
		/// </summary>
		/// <value>The selected indicies.</value>
		public List<int> SelectedIndicies {
			get {
				return new List<int>(selectedIndicies);
			}
			set {
				var deselect = selectedIndicies.Except(value).ToArray();
				var select = value.Except(selectedIndicies).ToArray();

				deselect.ForEach(Deselect);
				select.ForEach(Select);
			}
		}

		/// <summary>
		/// OnSelect event.
		/// </summary>
		public ListViewBaseEvent OnSelect = new ListViewBaseEvent();

		/// <summary>
		/// OnDeselect event.
		/// </summary>
		public ListViewBaseEvent OnDeselect = new ListViewBaseEvent();

		/// <summary>
		/// OnSubmit event.
		/// </summary>
		public UnityEvent onSubmit = new UnityEvent();

		/// <summary>
		/// OnCancel event.
		/// </summary>
		public UnityEvent onCancel = new UnityEvent();

		/// <summary>
		/// OnItemSelect event.
		/// </summary>
		public UnityEvent onItemSelect = new UnityEvent();

		/// <summary>
		/// onItemCancel event.
		/// </summary>
		public UnityEvent onItemCancel = new UnityEvent();

		/// <summary>
		/// The container for items objects.
		/// </summary>
		[SerializeField]
		public Transform Container;

		/// <summary>
		/// OnFocusIn event.
		/// </summary>
		public ListViewFocusEvent OnFocusIn = new ListViewFocusEvent();
		
		/// <summary>
		/// OnFocusOut event.
		/// </summary>
		public ListViewFocusEvent OnFocusOut = new ListViewFocusEvent();

		/// <summary>
		/// Set item indicies when items updated.
		/// </summary>
		[NonSerialized]
		protected bool SetItemIndicies = true;

		GameObject Unused;

		void Awake()
		{
			Start();
		}

		[System.NonSerialized]
		bool isStartedListViewBase;

		/// <summary>
		/// Start this instance.
		/// </summary>
		public virtual void Start()
		{
			if (isStartedListViewBase)
			{
				return ;
			}
			isStartedListViewBase = true;
			
			Unused = new GameObject("unused base");
			Unused.SetActive(false);
			Unused.transform.SetParent(transform, false);
			
			if ((selectedIndex!=-1) && (selectedIndicies.Count==0))
			{
				selectedIndicies.Add(selectedIndex);
			}
			selectedIndicies.RemoveAll(NotIsValid);
			if (selectedIndicies.Count==0)
			{
				selectedIndex = -1;
			}
		}

		/// <summary>
		/// Determines if item not exists with the specified index.
		/// </summary>
		/// <returns><c>true</c>, if item not exists, <c>false</c> otherwise.</returns>
		/// <param name="index">Index.</param>
		protected bool NotIsValid(int index)
		{
			return !IsValid(index);
		}

		/// <summary>
		/// Updates the items.
		/// </summary>
		public virtual void UpdateItems()
		{
			UpdateItems(items);
		}

		/// <summary>
		/// Determines whether this instance is horizontal. Not implemented for ListViewBase.
		/// </summary>
		/// <returns><c>true</c> if this instance is horizontal; otherwise, <c>false</c>.</returns>
		public virtual bool IsHorizontal()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Gets the default height of the item. Not implemented for ListViewBase.
		/// </summary>
		/// <returns>The default item height.</returns>
		public virtual float GetDefaultItemHeight()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Gets the default width of the item. Not implemented for ListViewBase.
		/// </summary>
		/// <returns>The default item width.</returns>
		public virtual float GetDefaultItemWidth()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Gets the spacing between items. Not implemented for ListViewBase.
		/// </summary>
		/// <returns>The item spacing.</returns>
		public virtual float GetItemSpacing()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Gets the horizontal spacing between items. Not implemented for ListViewBase.
		/// </summary>
		/// <returns>The item spacing.</returns>
		public virtual float GetItemSpacingX()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Gets the vertical spacing between items. Not implemented for ListViewBase.
		/// </summary>
		/// <returns>The item spacing.</returns>
		public virtual float GetItemSpacingY()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Gets the layout margin.
		/// </summary>
		/// <returns>The layout margin.</returns>
		public virtual Vector4 GetLayoutMargin()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Removes the callback.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="index">Index.</param>
		void RemoveCallback(ListViewItem item, int index)
		{
			if (item == null)
			{
				return;
			}
			if (index < callbacks.Count)
			{
				item.onClick.RemoveListener(callbacks[index]);
			}

			item.onSubmit.RemoveListener(Toggle);
			item.onCancel.RemoveListener(OnItemCancel);

			item.onSelect.RemoveListener(HighlightColoring);
			item.onDeselect.RemoveListener(Coloring);

			item.onMove.RemoveListener(OnItemMove);
		}

		/// <summary>
		/// Raises the item cancel event.
		/// </summary>
		/// <param name="item">Item.</param>
		void OnItemCancel(ListViewItem item)
		{
			if (EventSystem.current.alreadySelecting)
			{
				return;
			}

			EventSystem.current.SetSelectedGameObject(gameObject);

			onItemCancel.Invoke();
		}

		/// <summary>
		/// Removes the callbacks.
		/// </summary>
		void RemoveCallbacks()
		{
			if (callbacks.Count > 0)
			{
				items.ForEach(RemoveCallback);
			}
			callbacks.Clear();
		}

		/// <summary>
		/// Adds the callbacks.
		/// </summary>
		void AddCallbacks()
		{
			items.ForEach(AddCallback);
		}

		/// <summary>
		/// Adds the callback.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="index">Index.</param>
		void AddCallback(ListViewItem item, int index)
		{
			callbacks.Insert(index, () => Toggle(item));

			item.onClick.AddListener(callbacks[index]);

			item.onSubmit.AddListener(OnItemSubmit);
			item.onCancel.AddListener(OnItemCancel);

			item.onSelect.AddListener(HighlightColoring);
			item.onDeselect.AddListener(Coloring);

			item.onMove.AddListener(OnItemMove);
		}

		/// <summary>
		/// Raises the item select event.
		/// </summary>
		/// <param name="item">Item.</param>
		void OnItemSelect(ListViewItem item)
		{
			onItemSelect.Invoke();
		}

		/// <summary>
		/// Raises the item submit event.
		/// </summary>
		/// <param name="item">Item.</param>
		void OnItemSubmit(ListViewItem item)
		{
			Toggle(item);
			if (!IsSelected(item.Index))
			{
				HighlightColoring(item);
			}
		}

		/// <summary>
		/// Raises the item move event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		/// <param name="item">Item.</param>
		protected virtual void OnItemMove(AxisEventData eventData, ListViewItem item)
		{
			switch (eventData.moveDir)
			{
				case MoveDirection.Left:
					break;
				case MoveDirection.Right:
					break;
				case MoveDirection.Up:
					if (item.Index > 0)
					{
						SelectComponentByIndex(item.Index - 1);
					}
					break;
				case MoveDirection.Down:
					if (IsValid(item.Index + 1))
					{
						SelectComponentByIndex(item.Index + 1);
					}
					break;
			}
		}

		/// <summary>
		/// Scrolls to item with specifid index.
		/// </summary>
		/// <param name="index">Index.</param>
		public virtual void ScrollTo(int index)
		{

		}


		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>Index of added item.</returns>
		public virtual int Add(ListViewItem item)
		{
			if (item.transform.parent!=Container)
			{
				item.transform.SetParent(Container, false);
			}
			AddCallback(item, items.Count);

			items.Add(item);
			item.Index = callbacks.Count - 1;

			return callbacks.Count - 1;
		}

		/// <summary>
		/// Clear items of this instance.
		/// </summary>
		public virtual void Clear()
		{
			items.Clear();
			UpdateItems();
		}

		/// <summary>
		/// Remove the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>Index of removed item.</returns>
		protected virtual int Remove(ListViewItem item)
		{
			RemoveCallbacks();

			var index = item.Index;

			selectedIndicies = selectedIndicies.Where(x => x!=index).Select(x => x > index ? x-- : x).ToList();
			if (selectedIndex==index)
			{
				Deselect(index);
				selectedIndex = selectedIndicies.Count > 0 ? selectedIndicies.Last() : -1;
			}
			else if (selectedIndex > index)
			{
				selectedIndex -= 1;
			}

			items.Remove(item);
			Free(item);

			AddCallbacks();

			return index;
		}

		/// <summary>
		/// Free the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		void Free(Component item)
		{
			if (item==null)
			{
				return ;
			}
			/*
			if (DestroyGameObjects)
			{
				if (item.gameObject==null)
				{
					return ;
				}
				Destroy(item.gameObject);
			}
			else
			*/
			{
				if ((item.transform==null) || (Unused==null) || (Unused.transform==null))
				{
					return ;
				}
				item.transform.SetParent(Unused.transform, false);
			}
		}

		/// <summary>
		/// Updates the items.
		/// </summary>
		/// <param name="newItems">New items.</param>
		void UpdateItems(List<ListViewItem> newItems)
		{
			RemoveCallbacks();

			items.Where(item => item!=null && !newItems.Contains(item)).ForEach(Free);

			newItems.ForEach(UpdateItem);

			//selectedIndicies.Clear();
			//selectedIndex = -1;

			items = newItems;

			AddCallbacks();
		}

		void UpdateItem(ListViewItem item, int index)
		{
			if (item==null)
			{
				return ;
			}
			if (SetItemIndicies)
			{
				item.Index = index;
			}
			item.transform.SetParent(Container, false);
		}

		/// <summary>
		/// Determines if item exists with the specified index.
		/// </summary>
		/// <returns><c>true</c> if item exists with the specified index; otherwise, <c>false</c>.</returns>
		/// <param name="index">Index.</param>
		public virtual bool IsValid(int index)
		{
			return (index >= 0) && (index < items.Count);
		}

		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="index">Index.</param>
		protected ListViewItem GetItem(int index)
		{
			return items.Find(x => x.Index==index);
		}

		/// <summary>
		/// Select item by the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		public virtual void Select(int index)
		{
			if (index==-1)
			{
				return ;
			}

			if (!IsValid(index))
			{
				var message = string.Format("Index must be between 0 and Items.Count ({0}). Gameobject {1}.", items.Count - 1, name);
				throw new IndexOutOfRangeException(message);
			}

			if (IsSelected(index) && Multiple)
			{
				return ;
			}

			if (!Multiple)
			{
				if ((selectedIndex!=-1) && (selectedIndex!=index))
				{
					Deselect(selectedIndex);
				}

				selectedIndicies.Clear();
			}

			selectedIndicies.Add(index);
			selectedIndex = index;

			SelectItem(index);

			OnSelect.Invoke(index, GetItem(index));
		}

		/// <summary>
		/// Silents the deselect specified indicies.
		/// </summary>
		/// <param name="indicies">Indicies.</param>
		protected void SilentDeselect(List<int> indicies)
		{
			selectedIndicies = selectedIndicies.Except(indicies).ToList();

			selectedIndex = (selectedIndicies.Count > 0) ? selectedIndicies[selectedIndicies.Count - 1] : -1;
		}

		/// <summary>
		/// Silents the select specified indicies.
		/// </summary>
		/// <param name="indicies">Indicies.</param>
		protected void SilentSelect(List<int> indicies)
		{
			if (indicies==null)
			{
				indicies = new List<int>();
			}
			selectedIndicies.AddRange(indicies.Except(selectedIndicies));
			selectedIndex = (selectedIndicies.Count > 0) ? selectedIndicies[selectedIndicies.Count - 1] : -1;
		}

		/// <summary>
		/// Deselect item by the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		public void Deselect(int index)
		{
			if (index==-1)
			{
				return ;
			}
			if (!IsSelected(index))
			{
				return ;
			}

			selectedIndicies.Remove(index);
			selectedIndex = (selectedIndicies.Count > 0)
				? selectedIndicies.Last()
				: - 1;

			if (IsValid(index))
			{
				DeselectItem(index);

				OnDeselect.Invoke(index, GetItem(index));
			}
		}

		/// <summary>
		/// Determines if item is selected with the specified index.
		/// </summary>
		/// <returns><c>true</c> if item is selected with the specified index; otherwise, <c>false</c>.</returns>
		/// <param name="index">Index.</param>
		public bool IsSelected(int index)
		{
			return selectedIndicies.Contains(index);
		}

		/// <summary>
		/// Toggle item by the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		public void Toggle(int index)
		{
			if (IsSelected(index) && Multiple)
			{
				Deselect(index);
			}
			else
			{
				Select(index);
			}
		}

		/// <summary>
		/// Toggle the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		void Toggle(ListViewItem item)
		{
			var shift_pressed =  Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			var have_selected = selectedIndicies.Count > 0;
			if (Multiple && shift_pressed && have_selected && selectedIndicies[0]!=item.Index)
			{
				// deselect all items except first
				selectedIndicies.GetRange(1, selectedIndicies.Count - 1).ForEach(Deselect);
				
				// find min and max indicies
				var min = Mathf.Min(selectedIndicies[0], item.Index);
				var max = Mathf.Max(selectedIndicies[0], item.Index);
				// select items from min to max
				Enumerable.Range(min, max - min + 1).ForEach(Select);
				return ;
			}

			Toggle(item.Index);
		}

		/// <summary>
		/// Gets the index of the component.
		/// </summary>
		/// <returns>The component index.</returns>
		/// <param name="item">Item.</param>
		protected int GetComponentIndex(ListViewItem item)
		{
			return item.Index;
		}

		/// <summary>
		/// Move the component transform to the end of the local transform list.
		/// </summary>
		/// <param name="item">Item.</param>
		protected void SetComponentAsLastSibling(ListViewItem item)
		{
			item.transform.SetAsLastSibling();
		}

		/// <summary>
		/// Called when item selected.
		/// Use it for change visible style of selected item.
		/// </summary>
		/// <param name="index">Index.</param>
		protected virtual void SelectItem(int index)
		{

		}

		/// <summary>
		/// Called when item deselected.
		/// Use it for change visible style of deselected item.
		/// </summary>
		/// <param name="index">Index.</param>
		protected virtual void DeselectItem(int index)
		{
			
		}

		/// <summary>
		/// Coloring the specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected virtual void Coloring(ListViewItem component)
		{
		}
		
		/// <summary>
		/// Set highlights colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected virtual void HighlightColoring(ListViewItem component)
		{
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			RemoveCallbacks();
			
			items.ForEach(Free);
		}

		/// <summary>
		/// Set EventSystem.current.SetSelectedGameObject with selected or first item.
		/// </summary>
		/// <returns><c>true</c>, if component was selected, <c>false</c> otherwise.</returns>
		public virtual bool SelectComponent()
		{
			if (items.Count==0)
			{
				return false;
			}
			var index = (SelectedIndex!=-1) ? SelectedIndex : 0;
			SelectComponentByIndex(index);

			return true;
		}

		/// <summary>
		/// Selects the component by index.
		/// </summary>
		/// <param name="index">Index.</param>
		protected void SelectComponentByIndex(int index)
		{
			ScrollTo(index);

			var ev = new ListViewItemEventData(EventSystem.current) {
				NewSelectedObject = GetItem(index).gameObject
			};
			ExecuteEvents.Execute<ISelectHandler>(ev.NewSelectedObject, ev, ExecuteEvents.selectHandler);
		}

		/// <summary>
		/// Raises the select event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			if (!EventSystem.current.alreadySelecting)
			{
				EventSystem.current.SetSelectedGameObject(gameObject);
			}
			OnFocusIn.Invoke(eventData);
		}

		/// <summary>
		/// Raises the deselect event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			OnFocusOut.Invoke(eventData);
		}

		/// <summary>
		/// Raises the submit event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			SelectComponent();
			onSubmit.Invoke();
		}

		/// <summary>
		/// Raises the cancel event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		void ICancelHandler.OnCancel(BaseEventData eventData)
		{
			onCancel.Invoke();
		}

		/// <summary>
		/// Calls specified function with each component.
		/// </summary>
		/// <param name="func">Func.</param>
		public virtual void ForEachComponent(Action<ListViewItem> func)
		{
			items.ForEach(func);
		}

		#region ListViewPaginator support
		/// <summary>
		/// Gets the ScrollRect.
		/// </summary>
		/// <returns>The ScrollRect.</returns>
		public virtual ScrollRect GetScrollRect()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the items count.
		/// </summary>
		/// <returns>The items count.</returns>
		public virtual int GetItemsCount()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the items per block count.
		/// </summary>
		/// <returns>The items per block.</returns>
		public virtual int GetItemsPerBlock()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the item position by index.
		/// </summary>
		/// <returns>The item position.</returns>
		/// <param name="index">Index.</param>
		public virtual float GetItemPosition(int index)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the index of the nearest item.
		/// </summary>
		/// <returns>The nearest item index.</returns>
		public virtual int GetNearestItemIndex()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}