using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UIWidgets {

	/// <summary>
	/// Range slider base.
	/// </summary>
	public abstract class RangeSliderBase<T> : MonoBehaviour, IPointerClickHandler
		where T : struct
	{
		/// <summary>
		/// OnChangeEvent.
		/// </summary>
		[System.Serializable]
		public class OnChangeEvent : UnityEvent<T,T> {

		}

		/// <summary>
		/// The minimum value.
		/// </summary>
		[SerializeField]
		protected T valueMin;

		/// <summary>
		/// Gets or sets the minimum value.
		/// </summary>
		/// <value>The minimum value.</value>
		public T ValueMin {
			get {
				return valueMin;
			}
			set {
				if (!EqualityComparer<T>.Default.Equals(valueMin, InBoundsMin(value)))
				{
					valueMin = InBoundsMin(value);
					UpdateMinHandle();
					UpdateFill();
					OnValuesChange.Invoke(valueMin, valueMax);
					OnChange.Invoke();
				}
			}
		}
		
		/// <summary>
		/// The maximum value.
		/// </summary>
		[SerializeField]
		protected T valueMax;

		/// <summary>
		/// Gets or sets the maximum value.
		/// </summary>
		/// <value>The maximum value.</value>
		public T ValueMax {
			get {
				return valueMax;
			}
			set {
				if (!EqualityComparer<T>.Default.Equals(valueMax, InBoundsMax(value)))
				{
					valueMax = InBoundsMax(value);
					UpdateMaxHandle();
					UpdateFill();
					OnValuesChange.Invoke(valueMin, valueMax);
					OnChange.Invoke();
				}
			}
		}
		
		/// <summary>
		/// The step.
		/// </summary>
		[SerializeField]
		protected T step;

		/// <summary>
		/// Gets or sets the step.
		/// </summary>
		/// <value>The step.</value>
		public T Step {
			get {
				return step;
			}
			set {
				step = value;
			}
		}
		
		/// <summary>
		/// The minimum limit.
		/// </summary>
		[SerializeField]
		protected T limitMin;

		/// <summary>
		/// Gets or sets the minimum limit.
		/// </summary>
		/// <value>The minimum limit.</value>
		public T LimitMin {
			get {
				return limitMin;
			}
			set {
				limitMin = value;
				ValueMin = valueMin;
				ValueMax = valueMax;
			}
		}
		
		/// <summary>
		/// The maximum limit.
		/// </summary>
		[SerializeField]
		protected T limitMax;

		/// <summary>
		/// Gets or sets the maximum limit.
		/// </summary>
		/// <value>The maximum limit.</value>
		public T LimitMax {
			get {
				return limitMax;
			}
			set {
				limitMax = value;
				ValueMin = valueMin;
				ValueMax = valueMax;
			}
		}
		
		/// <summary>
		/// The handle minimum.
		/// </summary>
		[SerializeField]
		protected RangeSliderHandle handleMin;

		/// <summary>
		/// The handle minimum rect.
		/// </summary>
		protected RectTransform handleMinRect;

		/// <summary>
		/// Gets the handle minimum rect.
		/// </summary>
		/// <value>The handle minimum rect.</value>
		public RectTransform HandleMinRect {
			get {
				if (handleMin!=null && handleMinRect==null)
				{
					handleMinRect = handleMin.transform as RectTransform;
				}
				return handleMinRect;
			}
		}

		/// <summary>
		/// Gets or sets the handle minimum.
		/// </summary>
		/// <value>The handle minimum.</value>
		public RangeSliderHandle HandleMin {
			get {
				return handleMin;
			}
			set {
				handleMin = value;
				handleMinRect = null;
				handleMin.IsHorizontal = IsHorizontal;
				handleMin.PositionLimits = MinPositionLimits;
				handleMin.PositionChanged = UpdateMinValue;
				handleMin.OnSubmit = SelectMaxHandle;
				handleMin.Increase = IncreaseMin;
				handleMin.Decrease = DecreaseMin;

				HandleMinRect.anchorMin = new Vector2(0, 0);
				HandleMinRect.anchorMax = (IsHorizontal()) ? new Vector2(0, 1) :  new Vector2(1, 0);
			}
		}
		
		/// <summary>
		/// The handle max.
		/// </summary>
		[SerializeField]
		protected RangeSliderHandle handleMax;

		/// <summary>
		/// The handle max rect.
		/// </summary>
		protected RectTransform handleMaxRect;

		/// <summary>
		/// Gets the handle maximum rect.
		/// </summary>
		/// <value>The handle maximum rect.</value>
		public RectTransform HandleMaxRect {
			get {
				if (handleMax!=null && handleMaxRect==null)
				{
					handleMaxRect = handleMax.transform as RectTransform;
				}
				return handleMaxRect;
			}
		}

		/// <summary>
		/// Gets or sets the handle max.
		/// </summary>
		/// <value>The handle max.</value>
		public RangeSliderHandle HandleMax {
			get {
				return handleMax;
			}
			set {
				handleMax = value;
				handleMaxRect = null;
				handleMax.IsHorizontal = IsHorizontal;
				handleMax.PositionLimits = MaxPositionLimits;
				handleMax.PositionChanged = UpdateMaxValue;
				handleMax.OnSubmit = SelectMinHandle;
				handleMax.Increase = IncreaseMax;
				handleMax.Decrease = DecreaseMax;

				HandleMaxRect.anchorMin = new Vector2(0, 0);
				HandleMaxRect.anchorMax = (IsHorizontal()) ? new Vector2(0, 1) :  new Vector2(1, 0);
			}
		}
		
		/// <summary>
		/// The usable range rect.
		/// </summary>
		[SerializeField]
		protected RectTransform UsableRangeRect;
		
		/// <summary>
		/// The fill rect.
		/// </summary>
		[SerializeField]
		protected RectTransform FillRect;

		/// <summary>
		/// The range slider rect.
		/// </summary>
		protected RectTransform rangeSliderRect;
		
		/// <summary>
		/// Gets the handle maximum rect.
		/// </summary>
		/// <value>The handle maximum rect.</value>
		public RectTransform RangeSliderRect {
			get {
				if (rangeSliderRect==null)
				{
					rangeSliderRect = transform as RectTransform;
				}
				return rangeSliderRect;
			}
		}

		/// <summary>
		/// OnValuesChange event.
		/// </summary>
		public OnChangeEvent OnValuesChange = new OnChangeEvent();

		/// <summary>
		/// OnChange event.
		/// </summary>
		public UnityEvent OnChange = new UnityEvent();

		/// <summary>
		/// Whole number of steps.
		/// </summary>
		public bool WholeNumberOfSteps = false;

		void Awake()
		{

		}
		
		bool initCalled;
		void Init()
		{
			if (initCalled)
			{
				return ;
			}
			initCalled = true;

			HandleMin = handleMin;
			HandleMax = handleMax;
			UpdateMinHandle();
			UpdateMaxHandle();
			UpdateFill();
		}

		void Start()
		{
			Init();
		}

		/// <summary>
		/// Sets the values.
		/// </summary>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		public void SetValue(T min, T max)
		{
			var oldValueMin = valueMin;
			var oldValueMax = valueMax;

			valueMin = min;
			valueMax = max;

			valueMin = InBoundsMin(valueMin);
			valueMax = InBoundsMax(valueMax);

			var min_equals = EqualityComparer<T>.Default.Equals(oldValueMin, valueMin);
			var max_equals = EqualityComparer<T>.Default.Equals(oldValueMax, valueMax);

			if (!min_equals || !max_equals)
			{
				UpdateMinHandle();
				UpdateMaxHandle();
				UpdateFill();

				OnValuesChange.Invoke(valueMin, valueMax);
				OnChange.Invoke();
			}
		}

		/// <summary>
		/// Sets the limits.
		/// </summary>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		public void SetLimit(T min, T max)
		{
			var oldValueMin = valueMin;
			var oldValueMax = valueMax;

			limitMin = min;
			limitMax = max;

			valueMin = InBoundsMin(valueMin);
			valueMax = InBoundsMax(valueMax);
			
			var min_equals = EqualityComparer<T>.Default.Equals(oldValueMin, valueMin);
			var max_equals = EqualityComparer<T>.Default.Equals(oldValueMax, valueMax);
			
			if (!min_equals || !max_equals)
			{
				UpdateMinHandle();
				UpdateMaxHandle();
				UpdateFill();
				
				OnValuesChange.Invoke(valueMin, valueMax);
				OnChange.Invoke();
			}
		}

		/// <summary>
		/// Determines whether this instance is horizontal.
		/// </summary>
		/// <returns><c>true</c> if this instance is horizontal; otherwise, <c>false</c>.</returns>
		protected virtual bool IsHorizontal()
		{
			return true;
		}

		/// <summary>
		/// Returns size of usable rect.
		/// </summary>
		/// <returns>The size.</returns>
		protected float FillSize()
		{
			return (IsHorizontal()) ? UsableRangeRect.rect.width : UsableRangeRect.rect.height;
		}

		/// <summary>
		/// Minimum size of the handle.
		/// </summary>
		/// <returns>The handle size.</returns>
		protected float MinHandleSize()
		{
			if (IsHorizontal())
			{
				return HandleMinRect.rect.width;
			}
			else
			{
				return HandleMinRect.rect.height;
			}
		}

		/// <summary>
		/// Maximum size of the handle.
		/// </summary>
		/// <returns>The handle size.</returns>
		protected float MaxHandleSize()
		{
			if (IsHorizontal())
			{
				return HandleMaxRect.rect.width;
			}
			else
			{
				return HandleMaxRect.rect.height;
			}
		}

		/// <summary>
		/// Updates the minimum value.
		/// </summary>
		/// <param name="position">Position.</param>
		protected void UpdateMinValue(float position)
		{
			valueMin = PositionToValue(position - GetStartPoint());
			UpdateMinHandle();
			UpdateFill();
			OnValuesChange.Invoke(valueMin, valueMax);
			OnChange.Invoke();
		}

		/// <summary>
		/// Updates the maximum value.
		/// </summary>
		/// <param name="position">Position.</param>
		protected void UpdateMaxValue(float position)
		{
			valueMax = PositionToValue(position - GetStartPoint());
			UpdateMaxHandle();
			UpdateFill();
			OnValuesChange.Invoke(valueMin, valueMax);
			OnChange.Invoke();
		}

		/// <summary>
		/// Value to position.
		/// </summary>
		/// <returns>Position.</returns>
		/// <param name="value">Value.</param>
		protected abstract float ValueToPosition(T value);

		/// <summary>
		/// Position to value.
		/// </summary>
		/// <returns>Value.</returns>
		/// <param name="position">Position.</param>
		protected abstract T PositionToValue(float position);

		/// <summary>
		/// Gets the start point.
		/// </summary>
		/// <returns>The start point.</returns>
		protected float GetStartPoint()
		{
			return IsHorizontal() ? -UsableRangeRect.sizeDelta.x / 2f : -UsableRangeRect.sizeDelta.y / 2f;
		}

		/// <summary>
		/// Position range for minimum handle.
		/// </summary>
		/// <returns>The position limits.</returns>
		protected abstract Vector2 MinPositionLimits();

		/// <summary>
		/// Position range for maximum handle.
		/// </summary>
		/// <returns>The position limits.</returns>
		protected abstract Vector2 MaxPositionLimits();

		/// <summary>
		/// Fit value to bounds.
		/// </summary>
		/// <returns>Value.</returns>
		/// <param name="value">Value.</param>
		protected abstract T InBounds(T value);

		/// <summary>
		/// Fit minumum value to bounds.
		/// </summary>
		/// <returns>Value.</returns>
		/// <param name="value">Value.</param>
		protected abstract T InBoundsMin(T value);
		
		/// <summary>
		/// Fit maximum value to bounds.
		/// </summary>
		/// <returns>Value.</returns>
		/// <param name="value">Value.</param>
		protected abstract T InBoundsMax(T value);

		/// <summary>
		/// Increases the minimum value.
		/// </summary>
		protected abstract void IncreaseMin();

		/// <summary>
		/// Decreases the minimum value.
		/// </summary>
		protected abstract void DecreaseMin();

		/// <summary>
		/// Increases the maximum value.
		/// </summary>
		protected abstract void IncreaseMax();

		/// <summary>
		/// Decreases the maximum value.
		/// </summary>
		protected abstract void DecreaseMax();

		/// <summary>
		/// Updates the minimum handle.
		/// </summary>
		protected void UpdateMinHandle()
		{
			UpdateHandle(HandleMinRect, valueMin);
		}

		/// <summary>
		/// Updates the maximum handle.
		/// </summary>
		protected void UpdateMaxHandle()
		{
			UpdateHandle(HandleMaxRect, valueMax);
		}

		/// <summary>
		/// Updates the fill.
		/// </summary>
		void UpdateFill()
		{
			if (IsHorizontal())
			{
				FillRect.anchoredPosition = new Vector2(HandleMinRect.anchoredPosition.x, FillRect.anchoredPosition.y);
				var sizeDelta = new Vector2((HandleMaxRect.anchoredPosition.x - HandleMinRect.anchoredPosition.x), FillRect.sizeDelta.y);
				FillRect.sizeDelta = sizeDelta;
			}
			else
			{
				FillRect.anchoredPosition = new Vector2(FillRect.anchoredPosition.x, HandleMinRect.anchoredPosition.y);
				var sizeDelta = new Vector2(FillRect.sizeDelta.x, (+ HandleMaxRect.anchoredPosition.y - HandleMinRect.anchoredPosition.y));
				FillRect.sizeDelta = sizeDelta;
			}
		}

		/// <summary>
		/// Updates the handle.
		/// </summary>
		/// <param name="handleTransform">Handle transform.</param>
		/// <param name="value">Value.</param>
		protected void UpdateHandle(RectTransform handleTransform, T value)
		{
			var new_position = handleTransform.anchoredPosition;
			if (IsHorizontal())
			{
				new_position.x = ValueToPosition(value) + handleTransform.rect.width * (handleTransform.pivot.x - 0.5f);
			}
			else
			{
				new_position.y = ValueToPosition(value) + handleTransform.rect.height * (handleTransform.pivot.y - 0.5f);
			}
			handleTransform.anchoredPosition = new_position;
		}

		/// <summary>
		/// Selects the minimum handle.
		/// </summary>
		void SelectMinHandle()
		{
			if ((EventSystem.current!=null) && (!EventSystem.current.alreadySelecting))
			{
				EventSystem.current.SetSelectedGameObject(handleMin.gameObject);
			}
		}

		/// <summary>
		/// Selects the max handle.
		/// </summary>
		void SelectMaxHandle()
		{
			if ((EventSystem.current!=null) && (!EventSystem.current.alreadySelecting))
			{
				EventSystem.current.SetSelectedGameObject(handleMax.gameObject);
			}
		}

		/// <summary>
		/// Raises the pointer click event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button!=PointerEventData.InputButton.Left)
			{
				return;
			}

			Vector2 curCursor;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(UsableRangeRect, eventData.position, eventData.pressEventCamera, out curCursor))
			{
				return ;
			}
			curCursor -= UsableRangeRect.rect.position;

			var new_min_position = (IsHorizontal() ? curCursor.x : curCursor.y) + GetStartPoint();
			var new_max_position = new_min_position - MaxHandleSize();
			var min_position = IsHorizontal() ? HandleMinRect.anchoredPosition.x : HandleMinRect.anchoredPosition.y;
			var max_position = IsHorizontal() ? HandleMaxRect.anchoredPosition.x : HandleMaxRect.anchoredPosition.y;
			var delta_min = new_min_position - min_position;
			var delta_max = max_position - new_max_position;
			if (delta_min < delta_max)
			{
				UpdateMinValue(new_min_position);
			}
			else
			{
				UpdateMaxValue(new_max_position);
			}
		}

#if UNITY_EDITOR
		/// <summary>
		/// Handle values change from editor.
		/// </summary>
		public void EditorUpdate()
		{
			if (handleMin!=null && handleMax!=null && UsableRangeRect!=null && FillRect!=null)
			{
				UpdateMinHandle();
				UpdateMaxHandle();
				UpdateFill();
			}
		}
#endif
	}
}