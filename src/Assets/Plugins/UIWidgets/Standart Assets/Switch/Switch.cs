using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace UIWidgets {
	/// <summary>
	/// Switch event.
	/// </summary>
	public class SwitchEvent : UnityEvent<bool> {
	}

	/// <summary>
	/// Switch direction.
	/// </summary>
	public enum SwitchDirection {
		LeftToRight,
		RightToLeft,
		BottomToTop,
		TopToBottom,
	}

	/// <summary>
	/// Switch.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/Switch")]
	public class Switch : Selectable, ISubmitHandler, IPointerClickHandler  {
		/// <summary>
		/// Is on?
		/// </summary>
		[SerializeField]
		protected bool isOn;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is on.
		/// </summary>
		/// <value><c>true</c> if this instance is on; otherwise, <c>false</c>.</value>
		public virtual bool IsOn {
			get {
				return isOn;
			}
			set {
				if (isOn!=value)
				{
					isOn = value;
					if (Group!=null)
					{
						if (isOn || (!Group.AnySwitchesOn() && !Group.AllowSwitchOff))
						{
							isOn = true;
							Group.NotifySwitchOn(this);
						}
					}

					Changed();
				}
			}
		}

		/// <summary>
		/// Switch group.
		/// </summary>
		[SerializeField]
		protected SwitchGroup Group;

		/// <summary>
		/// Gets or sets the switch group.
		/// </summary>
		/// <value>The switch group.</value>
		public virtual SwitchGroup SwitchGroup
		{
			get {
				return Group;
			}
			set {
				if (Group != null)
				{
					Group.UnregisterSwitch(this);
				}

				Group = value;

				if (Group != null)
				{
					Group.RegisterSwitch(this);
					if (IsOn)
					{
						Group.NotifySwitchOn(this);
					}
				}
			}
		}

		/// <summary>
		/// The direction.
		/// </summary>
		[SerializeField]
		protected SwitchDirection direction = SwitchDirection.LeftToRight;

		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		/// <value>The direction.</value>
		public SwitchDirection Direction {
			get {
				return direction;
			}
			set {
				direction = value;
				SetMarkPosition(false);
			}
		}

		/// <summary>
		/// The mark.
		/// </summary>
		[SerializeField]
		public RectTransform Mark;

		/// <summary>
		/// The mark.
		/// </summary>
		[SerializeField]
		public Graphic MarkGraphic;

		/// <summary>
		/// The background.
		/// </summary>
		[SerializeField]
		public Graphic Background;


		[SerializeField]
		Color markOnColor = new Color(1f, 1f, 1f, 1f);

		/// <summary>
		/// Gets or sets the color of the mark for On state.
		/// </summary>
		/// <value>The color of the mark for On state.</value>
		public Color MarkOnColor {
			get {
				return markOnColor;
			}
			set {
				markOnColor = value;
				SetMarkColor();
			}
		}

		[SerializeField]
		Color markOffColor = new Color(1f, 215f/255f, 115f/255f, 1f);

		/// <summary>
		/// Gets or sets the color of the mark for Off State.
		/// </summary>
		/// <value>The color of the mark for Off State.</value>
		public Color MarkOffColor {
			get {
				return markOffColor;
			}
			set {
				markOffColor = value;
				SetMarkColor();
			}
		}

		[SerializeField]
		Color backgroundOnColor = new Color(1f, 1f, 1f, 1f);

		/// <summary>
		/// Gets or sets the color of the background for On state.
		/// </summary>
		/// <value>The color of the background on.</value>
		public Color BackgroundOnColor {
			get {
				return backgroundOnColor;
			}
			set {
				backgroundOnColor = value;
				SetBackgroundColor();
			}
		}

		[SerializeField]
		Color backgroundOffColor = new Color(1f, 215f/255f, 115f/255f, 1f);

		/// <summary>
		/// Gets or sets the color of the background for Off State.
		/// </summary>
		/// <value>The color of the background off.</value>
		public Color BackgroundOffColor {
			get {
				return backgroundOffColor;
			}
			set {
				backgroundOffColor = value;
				SetBackgroundColor();
			}
		}

		/// <summary>
		/// The duration of the animation.
		/// </summary>
		[SerializeField]
		public float AnimationDuration = 0.3f;

		/// <summary>
		/// Use unscaled time.
		/// </summary>
		[SerializeField]
		public bool UnscaledTime;

		/// <summary>
		/// Callback executed when the IsOn of the switch is changed.
		/// </summary>
		[SerializeField]
		public SwitchEvent OnValueChanged = new SwitchEvent();

		protected override void Awake ()
		{
			base.Awake();
			SwitchGroup = Group;
		}

		/// <summary>
		/// Changed this instance.
		/// </summary>
		protected virtual void Changed()
		{
			SetMarkPosition();

			SetBackgroundColor();
			SetMarkColor();

			OnValueChanged.Invoke(IsOn);
		}

		/// <summary>
		/// The current corutine.
		/// </summary>
		protected IEnumerator CurrentCorutine;

		/// <summary>
		/// Sets the mark position.
		/// </summary>
		/// <param name="animate">If set to <c>true</c> animate.</param>
		protected virtual void SetMarkPosition(bool animate=true)
		{
			if (CurrentCorutine!=null)
			{
				StopCoroutine(CurrentCorutine);
				CurrentCorutine = null;
			}
			
			if (animate)
			{
				CurrentCorutine = AnimateSwitch(IsOn, AnimationDuration);
				StartCoroutine(CurrentCorutine);
			}
			else
			{
				SetMarkPosition(GetPosition(IsOn));
			}
		}

		/// <summary>
		/// Animates the switch.
		/// </summary>
		/// <returns>The switch.</returns>
		/// <param name="state">IsOn.</param>
		/// <param name="time">Time.</param>
		protected virtual IEnumerator AnimateSwitch(bool state, float time)
		{
			SetMarkPosition(GetPosition(!IsOn));

			var prev_position = GetPosition(!IsOn);
			var next_position = GetPosition(IsOn);
			var end_time = GetTime() + time;

			while (GetTime() <= end_time)
			{
				var distance = 1 - ((end_time - GetTime()) / time);
				var pos = Mathf.Lerp(prev_position, next_position, distance);

				SetMarkPosition(pos);

				yield return null;
			}
			SetMarkPosition(GetPosition(IsOn));
		}

		protected float GetTime()
		{
			return UnscaledTime ? Time.unscaledTime : Time.time;
		}

		/// <summary>
		/// Sets the mark position.
		/// </summary>
		/// <param name="position">Position.</param>
		protected virtual void SetMarkPosition(float position)
		{
			if (Mark==null)
			{
				return ;
			}
			if (IsHorizontal())
			{
				Mark.anchorMin = new Vector2(position, Mark.anchorMin.y);
				Mark.anchorMax = new Vector2(position, Mark.anchorMax.y);
				Mark.pivot = new Vector2(position, Mark.pivot.y);
			}
			else
			{
				Mark.anchorMin = new Vector2(Mark.anchorMin.x, position);
				Mark.anchorMax = new Vector2(Mark.anchorMax.x, position);
				Mark.pivot = new Vector2(Mark.pivot.x, position);
			}
		}

		/// <summary>
		/// Determines whether this instance is horizontal.
		/// </summary>
		/// <returns><c>true</c> if this instance is horizontal; otherwise, <c>false</c>.</returns>
		protected bool IsHorizontal()
		{
			return Direction==SwitchDirection.LeftToRight || Direction==SwitchDirection.RightToLeft;
		}

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <returns>The position.</returns>
		/// <param name="state">State.</param>
		protected float GetPosition(bool state)
		{
			switch (Direction)
			{
				case SwitchDirection.LeftToRight:
				case SwitchDirection.BottomToTop:
					return (state) ? 1f : 0f;
					//break;
				case SwitchDirection.RightToLeft:
				case SwitchDirection.TopToBottom:
					return (state) ? 0f : 1f;
					//break;
			}
			return 0f;
		}

		/// <summary>
		/// Sets the color of the background.
		/// </summary>
		protected virtual void SetBackgroundColor()
		{
			if (Background==null)
			{
				return ;
			}
			Background.color = (IsOn) ? BackgroundOnColor : BackgroundOffColor;
		}

		/// <summary>
		/// Sets the color of the mark.
		/// </summary>
		protected virtual void SetMarkColor()
		{
			if (MarkGraphic==null)
			{
				return ;
			}
			MarkGraphic.color = (IsOn) ? MarkOnColor : MarkOffColor;
		}

		/// <summary>
		/// Calls the changed.
		/// </summary>
		protected virtual void CallChanged()
		{
			if (!IsActive() || !IsInteractable())
			{
				return ;
			}
			IsOn = !IsOn;
		}

		#region ISubmitHandler implementation
		/// <summary>
		/// Called by a BaseInputModule when an OnSubmit event occurs.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnSubmit(BaseEventData eventData)
		{
			CallChanged();
		}
		#endregion

		#region IPointerClickHandler implementation
		/// <summary>
		/// Called by a BaseInputModule when an OnPointerClick event occurs.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return ;
			}
			CallChanged();
		}
		#endregion

		#if UNITY_EDITOR
		protected override void OnValidate()
		{
			base.OnValidate();
			SetMarkPosition(false);
			SetBackgroundColor();
			SetMarkColor();
		}
		#endif
	}
}