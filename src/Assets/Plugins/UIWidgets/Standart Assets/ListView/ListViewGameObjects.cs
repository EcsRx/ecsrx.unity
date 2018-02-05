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
	/// ListViewGameObjects event.
	/// </summary>
	[System.Serializable]
	public class ListViewGameObjectsEvent : UnityEvent<int,GameObject>
	{
		
	}
	
	/// <summary>
	/// List view with GameObjects.
	/// Outdated. Replaced with ListViewCustom. It's provide better interface and usability.
	/// </summary>
	public class ListViewGameObjects : ListViewBase {
		[SerializeField]
		List<GameObject> objects = new List<GameObject>();
		
		/// <summary>
		/// Gets the objects.
		/// </summary>
		/// <value>The objects.</value>
		public List<GameObject> Objects {
			get {
				return new List<GameObject>(objects);
			}
			private set {
				UpdateItems(value);
			}
		}
		
		List<UnityAction<PointerEventData>> callbacksEnter = new List<UnityAction<PointerEventData>>();
		List<UnityAction<PointerEventData>> callbacksExit = new List<UnityAction<PointerEventData>>();
		
		/// <summary>
		/// Sort function.
		/// </summary>
		public Func<IEnumerable<GameObject>,IEnumerable<GameObject>> SortFunc = null;
		
		/// <summary>
		/// What to do when the object selected.
		/// </summary>
		public ListViewGameObjectsEvent OnSelectObject = new ListViewGameObjectsEvent();
		
		/// <summary>
		/// What to do when the object deselected.
		/// </summary>
		public ListViewGameObjectsEvent OnDeselectObject = new ListViewGameObjectsEvent();

		/// <summary>
		/// What to do when the event system send a pointer enter Event.
		/// </summary>
		public ListViewGameObjectsEvent OnPointerEnterObject = new ListViewGameObjectsEvent();

		/// <summary>
		/// What to do when the event system send a pointer exit Event.
		/// </summary>
		public ListViewGameObjectsEvent OnPointerExitObject = new ListViewGameObjectsEvent();

		/// <summary>
		/// Awake this instance.
		/// </summary>
		void Awake()
		{
			Start();
		}
		
		[System.NonSerialized]
		bool isStartedListViewGameObjects = false;

		/// <summary>
		/// Start this instance.
		/// </summary>
		public override void Start()
		{
			if (isStartedListViewGameObjects)
			{
				return ;
			}
			isStartedListViewGameObjects = true;
			
			base.Start();
			
			DestroyGameObjects = true;
			
			UpdateItems();
			
			OnSelect.AddListener(OnSelectCallback);
			OnDeselect.AddListener(OnDeselectCallback);
		}

		/// <summary>
		/// Raises the select callback event.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="item">Item.</param>
		void OnSelectCallback(int index, ListViewItem item)
		{
			OnSelectObject.Invoke(index, objects[index]);
		}

		/// <summary>
		/// Raises the deselect callback event.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="item">Item.</param>
		void OnDeselectCallback(int index, ListViewItem item)
		{
			OnDeselectObject.Invoke(index, objects[index]);
		}

		/// <summary>
		/// Raises the pointer enter callback event.
		/// </summary>
		/// <param name="index">Index.</param>
		void OnPointerEnterCallback(int index)
		{
			OnPointerEnterObject.Invoke(index, objects[index]);
		}

		/// <summary>
		/// Raises the pointer exit callback event.
		/// </summary>
		/// <param name="index">Index.</param>
		void OnPointerExitCallback(int index)
		{
			OnPointerExitObject.Invoke(index, objects[index]);
		}

		/// <summary>
		/// Updates the items.
		/// </summary>
		public override void UpdateItems()
		{
			UpdateItems(objects);
		}
		
		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>Index of added item.</returns>
		public virtual int Add(GameObject item)
		{
			var newObjects = Objects;
			newObjects.Add(item);
			UpdateItems(newObjects);
			
			var index = objects.IndexOf(item);
			
			return index;
		}
		
		/// <summary>
		/// Remove the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>Index of removed item.</returns>
		public virtual int Remove(GameObject item)
		{
			var index = objects.IndexOf(item);
			if (index==-1)
			{
				return index;
			}
			
			var newObjects = Objects;
			newObjects.Remove(item);
			UpdateItems(newObjects);
			
			return index;
		}

		void RemoveCallback(ListViewItem item, int index)
		{
			if (item==null)
			{
				return ;
			}
			if (index < callbacksEnter.Count)
			{
				item.onPointerEnter.RemoveListener(callbacksEnter[index]);
			}
			if (index < callbacksExit.Count)
			{
				item.onPointerExit.RemoveListener(callbacksExit[index]);
			}
		}

		/// <summary>
		/// Removes the callbacks.
		/// </summary>
		void RemoveCallbacks()
		{
			base.Items.ForEach(RemoveCallback);
			callbacksEnter.Clear();
			callbacksExit.Clear();
		}

		/// <summary>
		/// Adds the callbacks.
		/// </summary>
		void AddCallbacks()
		{
			base.Items.ForEach(AddCallback);
		}

		/// <summary>
		/// Adds the callback.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="index">Index.</param>
		void AddCallback(ListViewItem item, int index)
		{
			callbacksEnter.Add(ev => OnPointerEnterCallback(index));
			callbacksExit.Add(ev => OnPointerExitCallback(index));
			
			item.onPointerEnter.AddListener(callbacksEnter[index]);
			item.onPointerExit.AddListener(callbacksExit[index]);
		}

		/// <summary>
		/// Sorts the items.
		/// </summary>
		/// <returns>The items.</returns>
		/// <param name="newItems">New items.</param>
		List<GameObject> SortItems(IEnumerable<GameObject> newItems)
		{
			var temp = newItems;
			if (SortFunc!=null)
			{
				temp = SortFunc(temp);
			}
			
			return temp.ToList();
		}
		
		/// <summary>
		/// Clear items of this instance.
		/// </summary>
		public override void Clear()
		{
			if (DestroyGameObjects)
			{
				objects.ForEach(Destroy);
			}
			UpdateItems(new List<GameObject>());
		}

		/// <summary>
		/// Updates the items.
		/// </summary>
		/// <param name="newItems">New items.</param>
		void UpdateItems(List<GameObject> newItems)
		{
			RemoveCallbacks();
			
			newItems = SortItems(newItems);
			
			var new_selected_indicies = SelectedIndicies
				.Select(x => objects.Count > x ? newItems.IndexOf(objects[x]) : -1)
				.Where(x => x!=-1).ToList();
			SelectedIndicies.Except(new_selected_indicies).ForEach(Deselect);

			objects = newItems;
			base.Items = newItems.Convert(x => x.GetComponent<ListViewItem>() ?? x.AddComponent<ListViewItem>());

			SelectedIndicies = new_selected_indicies;

			AddCallbacks();
		}
		
		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected override void OnDestroy()
		{
			OnSelect.RemoveListener(OnSelectCallback);
			OnDeselect.RemoveListener(OnDeselectCallback);
			
			RemoveCallbacks();
			
			base.OnDestroy();
		}
	}
}