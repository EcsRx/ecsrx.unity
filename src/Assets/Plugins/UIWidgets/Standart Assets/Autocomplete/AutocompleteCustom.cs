using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UIWidgets {
	/// <summary>
	/// AutocompleteFilter.
	/// Startswith - value should beginnig with Input.
	/// Contains - Input occurs with value.
	/// </summary>
	public enum AutocompleteFilter {
		Startswith = 0,
		Contains = 1,
	}

	/// <summary>
	/// AutocompleteInput.
	/// Word - Use current word in input
	/// AllInput - use entire input.
	/// </summary>
	public enum AutocompleteInput {
		Word = 0,
		AllInput = 1,
	}

	/// <summary>
	/// AutocompleteResult.
	/// Append - append value to input.
	/// Result - replace input.
	/// </summary>
	public enum AutocompleteResult {
		Append = 0,
		Replace = 1,
	}

	/// <summary>
	/// Autocomplete.
	/// Allow quickly find and select from a list of values as user type.
	/// DisplayListView - used to display list of values.
	/// TargetListView - if specified selected value will be added to this list.
	/// DataSource - list of values.
	/// </summary>
	public abstract class AutocompleteCustom<TValue,TListViewComponent,TListView> : MonoBehaviour
		where TListView : ListViewCustom<TListViewComponent,TValue>
		where TListViewComponent : ListViewItem
	{
		/// <summary>
		/// InputField for autocomplete.
		/// </summary>
		[SerializeField]
		protected InputField InputField;

		IInputFieldProxy inputFieldProxy;

		/// <summary>
		/// Gets the InputFieldProxy.
		/// </summary>
		protected virtual IInputFieldProxy InputFieldProxy {
			get {
				if (inputFieldProxy==null)
				{
					inputFieldProxy = new InputFieldProxy(InputField);
				}
				return inputFieldProxy;
			}
		}

		/// <summary>
		/// ListView to display available values.
		/// </summary>
		[SerializeField]
		public TListView TargetListView;

		/// <summary>
		/// Selected value will be added to this ListView.
		/// </summary>
		[SerializeField]
		public TListView DisplayListView;

		/// <summary>
		/// The allow duplicate of TargetListView items.
		/// </summary>
		[SerializeField]
		public bool AllowDuplicate = false;

		/// <summary>
		/// List of values.
		/// </summary>
		[SerializeField]
		public List<TValue> DataSource;
		
		/// <summary>
		/// The filter.
		/// </summary>
		[SerializeField]
		protected AutocompleteFilter filter;

		/// <summary>
		/// Gets or sets the filter.
		/// </summary>
		/// <value>The filter.</value>
		public AutocompleteFilter Filter {
			get {
				return filter;
			}
			set {
				filter = value;
				CustomFilter = null;
			}
		}

		/// <summary>
		/// Is filter case sensitive?
		/// </summary>
		[SerializeField]
		public bool CaseSensitive;

		/// <summary>
		/// The delimiter chars to find word for autocomplete if InputType==Word.
		/// </summary>
		[SerializeField]
		public char[] DelimiterChars = new char[] {' ', '\n'};

		/// <summary>
		/// Custom filter.
		/// </summary>
		public Func<string,ObservableList<TValue>> CustomFilter;

		/// <summary>
		/// Use entire input or current word in input.
		/// </summary>
		[SerializeField]
		protected AutocompleteInput InputType = AutocompleteInput.Word;

		/// <summary>
		/// Append value to input or replace input.
		/// </summary>
		[SerializeField]
		protected AutocompleteResult Result = AutocompleteResult.Append;

		/// <summary>
		/// OnOptionSelected event.
		/// </summary>
		public UnityEvent OnOptionSelected = new UnityEvent();

		/// <summary>
		/// Current word in input or whole input for autocomplete.
		/// </summary>
		[HideInInspector]
		protected string Input = string.Empty;

		/// <summary>
		/// InputField.caretPosition. Used to keep caretPosition with Up and Down actions.
		/// </summary>
		protected int CaretPosition;

		/// <summary>
		/// Determines whether the beginnig of value matches the Input.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>true if beginnig of value matches the Input; otherwise, false.</returns>
		public abstract bool Startswith(TValue value);

		/// <summary>
		/// Returns a value indicating whether Input occurs within specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>true if the Input occurs within value parameter; otherwise, false.</returns>
		public abstract bool Contains(TValue value);

		/// <summary>
		/// Convert value to string.
		/// </summary>
		/// <returns>The string value.</returns>
		/// <param name="value">Value.</param>
		protected abstract string GetStringValue(TValue value);

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected virtual void Start()
		{
			InputFieldProxy.onValueChanged.AddListener(ApplyFilter);
			//InputFieldProxy.onEndEdit.AddListener(HideOptions);

			var inputListener = InputField.GetComponent<InputFieldListener>();
			if (inputListener==null)
			{
				inputListener = InputField.gameObject.AddComponent<InputFieldListener>();
			}
			inputListener.OnMoveEvent.AddListener(SelectResult);
			inputListener.OnSubmitEvent.AddListener(SubmitResult);
			inputListener.onDeselect.AddListener(InputDeselected);

			DisplayListView.gameObject.SetActive(false);

			DisplayListView.Multiple = false;
			DisplayListView.OnSelect.AddListener(ItemSelected);
		}

		/// <summary>
		/// Allow to process item selection event.
		/// </summary>
		protected bool AllowItemSelectionEvent;

		/// <summary>
		/// Process input deselected event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected virtual void InputDeselected(BaseEventData eventData)
		{
			var ev = eventData as PointerEventData;
			if ((ev!=null) && (ev.pointerEnter.transform.IsChildOf(DisplayListView.Container)))
			{
				AllowItemSelectionEvent = true;
			}
			else
			{
				HideOptions();
			}
		}

		/// <summary>
		/// Process item selected event.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="component">Component.</param>
		protected virtual void ItemSelected(int index, ListViewItem component)
		{
			if (AllowItemSelectionEvent)
			{
				AllowItemSelectionEvent = false;
				SubmitResult(null);
			}
		}

		/// <summary>
		/// Canvas will be used as parent for DisplayListView.
		/// </summary>
		protected Transform CanvasTransform;
		
		/// <summary>
		/// To keep DisplayListView position if InputField inside scrollable area.
		/// </summary>
		protected Vector2 DisplayListViewAnchoredPosition;
		
		/// <summary>
		/// Default parent for DisplayListView.
		/// </summary>
		protected Transform DisplayListViewParent;

		/// <summary>
		/// Closes the options.
		/// </summary>
		/// <param name="input">Input.</param>
		protected virtual void HideOptions(string input)
		{
			HideOptions();
		}

		/// <summary>
		/// Closes the options.
		/// </summary>
		protected virtual void HideOptions()
		{
			if (CanvasTransform!=null)
			{
				DisplayListView.transform.SetParent(DisplayListViewParent);
				(DisplayListView.transform as RectTransform).anchoredPosition = DisplayListViewAnchoredPosition;
			}
			
			DisplayListView.gameObject.SetActive(false);
		}

		/// <summary>
		/// Shows the options.
		/// </summary>
		protected virtual void ShowOptions()
		{
			CanvasTransform = Utilites.FindTopmostCanvas(DisplayListView.transform);
			if (CanvasTransform!=null)
			{
				DisplayListViewAnchoredPosition = (DisplayListView.transform as RectTransform).anchoredPosition;
				DisplayListViewParent = DisplayListView.transform.parent;
				DisplayListView.transform.SetParent(CanvasTransform);
			}

			DisplayListView.gameObject.SetActive(true);
		}

		/// <summary>
		/// Gets the results.
		/// </summary>
		/// <returns>Values matches filter.</returns>
		protected virtual ObservableList<TValue> GetResults()
		{
			if (CustomFilter!=null)
			{
				return CustomFilter(Input);
			}
			else
			{
				if (Filter==AutocompleteFilter.Startswith)
				{
					return DataSource.Where(Startswith).ToObservableList();
				}
				else
				{
					return DataSource.Where(Contains).ToObservableList();
				}
			}
		}

		/// <summary>
		/// Sets the input.
		/// </summary>
		protected virtual void SetInput()
		{
			if (InputType==AutocompleteInput.AllInput)
			{
				Input = InputFieldProxy.text;
			}
			else
			{
				int end_position = InputFieldProxy.caretPosition;

				var text = InputFieldProxy.text.Substring(0, end_position);
				var start_position = text.LastIndexOfAny(DelimiterChars) + 1;

				Input = text.Substring(start_position).Trim();
			}
		}

		/// <summary>
		/// The previous input string.
		/// </summary>
		protected string PrevInput;

		/// <summary>
		/// Applies the filter.
		/// </summary>
		/// <param name="input">Input.</param>
		protected virtual void ApplyFilter(string input)
		{
			SetInput();

			if (Input==PrevInput)
			{
				return ;
			}
			PrevInput = Input;

			if (Input.Length==0)
			{
				DisplayListView.DataSource.Clear();
				HideOptions();
				return ;
			}

			DisplayListView.Start();
			DisplayListView.Multiple = false;

			DisplayListView.DataSource = GetResults();

			if (DisplayListView.DataSource.Count > 0)
			{
				ShowOptions();
				DisplayListView.SelectedIndex = 0;
			}
			else
			{
				HideOptions();
			}
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		protected virtual void Update()
		{
			if (!AllowItemSelectionEvent)
			{
				CaretPosition = InputFieldProxy.caretPosition;
			}
		}

		/// <summary>
		/// Selects the result.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected virtual void SelectResult(AxisEventData eventData)
		{
			if (!DisplayListView.gameObject.activeInHierarchy)
			{
				return ;
			}

			if (DisplayListView.DataSource.Count==0)
			{
				return ;
			}

			switch (eventData.moveDir)
			{
				case MoveDirection.Up:
					if (DisplayListView.SelectedIndex==0)
					{
						DisplayListView.SelectedIndex = DisplayListView.DataSource.Count - 1;
					}
					else
					{
						DisplayListView.SelectedIndex -= 1;
					}
					DisplayListView.ScrollTo(DisplayListView.SelectedIndex);
					InputFieldProxy.caretPosition = CaretPosition;
					break;
				case MoveDirection.Down:
					if (DisplayListView.SelectedIndex==(DisplayListView.DataSource.Count - 1))
					{
						DisplayListView.SelectedIndex = 0;
					}
					else
					{
						DisplayListView.SelectedIndex += 1;
					}
					DisplayListView.ScrollTo(DisplayListView.SelectedIndex);
					InputFieldProxy.caretPosition = CaretPosition;
					break;
				default:
					var oldInput = Input;
					SetInput();
					if (oldInput!=Input)
					{
						ApplyFilter("");
					}
					break;
			}
		}

		/// <summary>
		/// Submits the result.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		protected virtual void SubmitResult(BaseEventData eventData)
		{
			SubmitResult(eventData, false);
		}

		/// <summary>
		/// Submits the result.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		/// <param name="isEnter">Is Enter pressed?</param>
		protected virtual void SubmitResult(BaseEventData eventData, bool isEnter)
		{
			if (DisplayListView.SelectedIndex==-1)
			{
				return ;
			}
			if (InputField.lineType==InputField.LineType.MultiLineNewline)
			{
				if (!DisplayListView.gameObject.activeInHierarchy)
				{
					return ;
				}
				else
				{
					isEnter = false;
				}
			}

			if (TargetListView!=null)
			{
				TargetListView.Set(DisplayListView.DataSource[DisplayListView.SelectedIndex], AllowDuplicate);
			}

			var value = GetStringValue(DisplayListView.DataSource[DisplayListView.SelectedIndex]);
			if (Result==AutocompleteResult.Append)
			{
				int end_position = (DisplayListView.gameObject.activeInHierarchy && eventData!=null && !isEnter) ? InputFieldProxy.caretPosition : CaretPosition;
				var text = InputFieldProxy.text.Substring(0, end_position);
				var start_position = text.LastIndexOfAny(DelimiterChars) + 1;

				InputFieldProxy.text = InputFieldProxy.text.Substring(0, start_position) + value + InputFieldProxy.text.Substring(end_position);

				InputFieldProxy.caretPosition = start_position + value.Length;
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
				InputField.MoveTextStart(false);
				InputField.MoveTextEnd(false);
				#else
				//InputField.selectionFocusPosition = start_position + value.Length;
				#endif
				if (isEnter)
				{
					FixCaretPosition = start_position + value.Length;
					InputField.ActivateInputField();
				}
			}
			else
			{
				InputFieldProxy.text = value;
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
				InputField.ActivateInputField();
				#else
				InputFieldProxy.caretPosition = value.Length;
				#endif
				FixCaretPosition = value.Length;
			}

			OnOptionSelected.Invoke();
			
			HideOptions();
		}

		/// <summary>
		/// Caret position after Enter pressed.
		/// </summary>
		protected int FixCaretPosition = -1;

		/// <summary>
		/// LateUpdate.
		/// </summary>
		protected virtual void LateUpdate()
		{
			if (FixCaretPosition!=-1)
			{
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
				InputField.MoveTextStart(false);
				InputField.MoveTextEnd(false);
				#else
				InputField.caretPosition = FixCaretPosition;
				FixCaretPosition = -1;
				#endif
			}
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			if (DisplayListView!=null)
			{
				DisplayListView.OnSelect.RemoveListener(ItemSelected);
			}
			if (InputField!=null)
			{
				InputFieldProxy.onValueChanged.RemoveListener(ApplyFilter);
				//InputFieldProxy.onEndEdit.RemoveListener(HideOptions);

				var inputListener = InputField.GetComponent<InputFieldListener>();
				if (inputListener!=null)
				{
					inputListener.OnMoveEvent.RemoveListener(SelectResult);
					inputListener.OnSubmitEvent.RemoveListener(SubmitResult);

					inputListener.onDeselect.RemoveListener(InputDeselected);
				}
			}
		}
	}
}