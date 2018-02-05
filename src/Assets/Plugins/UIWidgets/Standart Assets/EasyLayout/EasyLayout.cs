using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;

namespace EasyLayout {
	/// <summary>
	/// Grid constraints.
	/// Flexible - Don't constrain the number of rows or columns.
	/// FixedColumnCount - Constraint the number of columns to a specified number.
	/// FixedRowCount - Constraint the number of rows to a specified number.
	/// </summary>
	public enum GridConstraints
	{
		Flexible = 0,
		FixedColumnCount = 1,
		FixedRowCount = 2,
	}

	/// <summary>
	/// Compact constraints.
	/// Flexible - Don't constrain the number of rows or columns.
	/// MaxColumnCount - Constraint the number of columns to a specified number.
	/// MaxRowCount - Constraint the number of rows to a specified number.
	/// </summary>
	public enum CompactConstraints
	{
		Flexible = 0,
		MaxColumnCount = 1,
		MaxRowCount = 2,
	}

	/// <summary>
	/// Padding.
	/// </summary>
	[Serializable]
	public struct Padding
	{
		/// <summary>
		/// The left padding.
		/// </summary>
		[SerializeField]
		public float Left;

		/// <summary>
		/// The right padding.
		/// </summary>
		[SerializeField]
		public float Right;

		/// <summary>
		/// The top padding.
		/// </summary>
		[SerializeField]
		public float Top;

		/// <summary>
		/// The bottom padding.
		/// </summary>
		[SerializeField]
		public float Bottom;

		/// <summary>
		/// Initializes a new instance of the struct.
		/// </summary>
		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
		/// <param name="top">Top.</param>
		/// <param name="bottom">Bottom.</param>
		public Padding(float left, float right, float top, float bottom)
		{
			Left = left;
			Right = right;
			Top = top;
			Bottom = bottom;
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return String.Format("Padding(left: {0}, right: {1}, top: {2}, bottom: {3})",
				Left,
				Right,
				Top,
				Bottom
			);
		}
	}

	/// <summary>
	/// Children size.
	/// DoNothing - Don't change size of children.
	/// SetPreferred - Set children size to preferred.
	/// SetMaxFromPreferred - Set children size to maximum size of children preferred.
	/// FitContainer - Stretch children size to fit container.
	/// ShrinkOnOverflow - Shrink children size if UI size more than RectTransform size.
	/// </summary>
	[Flags]
	public enum ChildrenSize {
		DoNothing = 0,
		SetPreferred = 1,
		SetMaxFromPreferred = 2,
		FitContainer = 3,
		ShrinkOnOverflow = 4,
	}

	/// <summary>
	/// Anchors.
	/// UpperLeft - UpperLeft.
	/// UpperCenter - UpperCenter.
	/// UpperRight - UpperRight.
	/// MiddleLeft - MiddleLeft.
	/// MiddleCenter - MiddleCenter.
	/// MiddleRight - MiddleRight.
	/// LowerLeft - LowerLeft.
	/// LowerCenter - LowerCenter.
	/// LowerRight - LowerRight.
	/// </summary>
	[Flags]
	public enum Anchors {
		UpperLeft = 0,
		UpperCenter = 1,
		UpperRight = 2,
		
		MiddleLeft = 3,
		MiddleCenter = 4,
		MiddleRight = 5,
		
		LowerLeft = 6,
		LowerCenter = 7,
		LowerRight = 8,
	}
	
	/// <summary>
	/// Stackings.
	/// Horizontal - Horizontal.
	/// Vertical - Vertical.
	/// </summary>
	[Flags]
	public enum Stackings {
		Horizontal = 0,
		Vertical = 1,
	}

	/// <summary>
	/// Horizontal aligns.
	/// Left - Left.
	/// Center - Center.
	/// Right - Right.
	/// </summary>
	[Flags]
	public enum HorizontalAligns {
		Left = 0,
		Center = 1,
		Right = 2,
	}

	/// <summary>
	/// Inner aligns.
	/// Top - Top.
	/// Middle - Middle.
	/// Bottom - Bottom.
	/// </summary>
	[Flags]
	public enum InnerAligns {
		Top = 0,
		Middle = 1,
		Bottom = 2,
	}

	/// <summary>
	/// Layout type to use.
	/// Compact - Compact.
	/// Grid - Grid.
	/// </summary>
	[Flags]
	public enum LayoutTypes {
		Compact = 0,
		Grid = 1,
	}

	/// <summary>
	/// EasyLayout.
	/// Warning: using RectTransform relative size with positive size delta (like 100% + 10) with ContentSizeFitter can lead to infinite increased size.
	/// </summary>
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/UIWidgets/EasyLayout")]
	public class EasyLayout : UnityEngine.UI.LayoutGroup, INotifyPropertyChanged
	{
		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged = (x, y) => { };

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		[SerializeField]
		public UnityEvent SettingsChanged = new UnityEvent();

		[SerializeField]
		[FormerlySerializedAs("GroupPosition")]
		Anchors groupPosition = Anchors.UpperLeft;

		/// <summary>
		/// The group position.
		/// </summary>
		public Anchors GroupPosition {
			get {
				return groupPosition;
			}
			set {
				groupPosition = value;
				Changed("GroupPosition");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("Stacking")]
		Stackings stacking = Stackings.Horizontal;

		/// <summary>
		/// The stacking type.
		/// </summary>
		public Stackings Stacking {
			get {
				return stacking;
			}
			set {
				stacking = value;
				Changed("Stacking");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("LayoutType")]
		LayoutTypes layoutType = LayoutTypes.Compact;

		/// <summary>
		/// The type of the layout.
		/// </summary>
		public LayoutTypes LayoutType {
			get {
				return layoutType;
			}
			set {
				layoutType = value;
				Changed("LayoutType");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("CompactConstraint")]
		CompactConstraints compactConstraint = CompactConstraints.Flexible;

		/// <summary>
		/// Which constraint to use for the Grid layout.
		/// </summary>
		public CompactConstraints CompactConstraint {
			get {
				return compactConstraint;
			}
			set {
				compactConstraint = value;
				Changed("CompactConstraint");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("CompactConstraintCount")]
		int compactConstraintCount = 1;

		/// <summary>
		/// How many elements there should be along the constrained axis.
		/// </summary>
		public int CompactConstraintCount {
			get {
				return compactConstraintCount;
			}
			set {
				compactConstraintCount = value;
				Changed("CompactConstraintCount");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("GridConstraint")]
		GridConstraints gridConstraint = GridConstraints.Flexible;

		/// <summary>
		/// Which constraint to use for the Grid layout.
		/// </summary>
		public GridConstraints GridConstraint {
			get {
				return gridConstraint;
			}
			set {
				gridConstraint = value;
				Changed("GridConstraint");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("GridConstraintCount")]
		int gridConstraintCount = 1;

		/// <summary>
		/// How many cells there should be along the constrained axis.
		/// </summary>
		public int GridConstraintCount {
			get {
				return gridConstraintCount;
			}
			set {
				gridConstraintCount = value;
				Changed("GridConstraintCount");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("RowAlign")]
		HorizontalAligns rowAlign = HorizontalAligns.Left;

		/// <summary>
		/// The row align.
		/// </summary>
		public HorizontalAligns RowAlign {
			get {
				return rowAlign;
			}
			set {
				rowAlign = value;
				Changed("RowAlign");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("InnerAlign")]
		InnerAligns innerAlign = InnerAligns.Top;

		/// <summary>
		/// The inner align.
		/// </summary>
		public InnerAligns InnerAlign {
			get {
				return innerAlign;
			}
			set {
				innerAlign = value;
				Changed("InnerAlign");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("CellAlign")]
		Anchors cellAlign = Anchors.UpperLeft;

		/// <summary>
		/// The cell align.
		/// </summary>
		public Anchors CellAlign {
			get {
				return cellAlign;
			}
			set {
				cellAlign = value;
				Changed("CellAlign");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("Spacing")]
		Vector2 spacing = new Vector2(5, 5);

		/// <summary>
		/// The spacing.
		/// </summary>
		public Vector2 Spacing {
			get {
				return spacing;
			}
			set {
				spacing = value;
				Changed("Spacing");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("Symmetric")]
		bool symmetric = true;

		/// <summary>
		/// Symmetric margin.
		/// </summary>
		public bool Symmetric {
			get {
				return symmetric;
			}
			set {
				symmetric = value;
				Changed("Symmetric");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("Margin")]
		Vector2 margin = new Vector2(5, 5);

		/// <summary>
		/// The margin.
		/// </summary>
		public Vector2 Margin {
			get {
				return margin;
			}
			set {
				margin = value;
				Changed("Margin");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("PaddingInner")]
		Padding paddingInner = new Padding();

		/// <summary>
		/// The padding.
		/// </summary>
		public Padding PaddingInner {
			get {
				return paddingInner;
			}
			set {
				paddingInner = value;
				Changed("PaddingInner");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("MarginTop")]
		float marginTop = 5f;

		/// <summary>
		/// The margin top.
		/// </summary>
		public float MarginTop {
			get {
				return marginTop;
			}
			set {
				marginTop = value;
				Changed("MarginTop");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("MarginBottom")]
		float marginBottom = 5f;

		/// <summary>
		/// The margin bottom.
		/// </summary>
		public float MarginBottom {
			get {
				return marginBottom;
			}
			set {
				marginBottom = value;
				Changed("MarginBottom");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("MarginLeft")]
		float marginLeft = 5f;

		/// <summary>
		/// The margin left.
		/// </summary>
		public float MarginLeft {
			get {
				return marginLeft;
			}
			set {
				marginLeft = value;
				Changed("MarginLeft");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("MarginRight")]
		float marginRight = 5f;

		/// <summary>
		/// The margin right.
		/// </summary>
		public float MarginRight {
			get {
				return marginRight;
			}
			set {
				marginRight = value;
				Changed("MarginRight");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("RightToLeft")]
		bool rightToLeft = false;

		/// <summary>
		/// The right to left stacking.
		/// </summary>
		public bool RightToLeft {
			get {
				return rightToLeft;
			}
			set {
				rightToLeft = value;
				Changed("RightToLeft");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("TopToBottom")]
		bool topToBottom = true;

		/// <summary>
		/// The top to bottom stacking.
		/// </summary>
		public bool TopToBottom {
			get {
				return topToBottom;
			}
			set {
				topToBottom = value;
				Changed("TopToBottom");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("SkipInactive")]
		bool skipInactive = true;

		/// <summary>
		/// The skip inactive.
		/// </summary>
		public bool SkipInactive {
			get {
				return skipInactive;
			}
			set {
				skipInactive = value;
				Changed("SkipInactive");
			}
		}

		Func<IEnumerable<GameObject>,IEnumerable<GameObject>> filter = null;

		/// <summary>
		/// The filter.
		/// </summary>
		public Func<IEnumerable<GameObject>,IEnumerable<GameObject>> Filter {
			get {
				return filter;
			}
			set {
				filter = value;
				Changed("Filter");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("ChildrenWidth")]
		ChildrenSize childrenWidth;

		/// <summary>
		/// How to control width of the children.
		/// </summary>
		public ChildrenSize ChildrenWidth {
			get {
				return childrenWidth;
			}
			set {
				childrenWidth = value;
				Changed("ChildrenWidth");
			}
		}

		[SerializeField]
		[FormerlySerializedAs("ChildrenHeight")]
		ChildrenSize childrenHeight;

		/// <summary>
		/// How to control height of the children.
		/// </summary>
		public ChildrenSize ChildrenHeight {
			get {
				return childrenHeight;
			}
			set {
				childrenHeight = value;
				Changed("ChildrenHeight");
			}
		}

		/// <summary>
		/// Control width of children.
		/// </summary>
		[SerializeField]
		[Obsolete("Use ChildrenWidth with ChildrenSize.SetPreferred instead.")]
		public bool ControlWidth;

		/// <summary>
		/// Control height of children.
		/// </summary>
		[SerializeField]
		[Obsolete("Use ChildrenHeight with ChildrenSize.SetPreferred instead.")]
		[FormerlySerializedAs("ControlHeight")]
		public bool ControlHeight;

		/// <summary>
		/// Sets width of the chidren to maximum width from them.
		/// </summary>
		[SerializeField]
		[Obsolete("Use ChildrenWidth with ChildrenSize.SetMaxFromPreferred instead.")]
		[FormerlySerializedAs("MaxWidth")]
		public bool MaxWidth;
		
		/// <summary>
		/// Sets height of the chidren to maximum height from them.
		/// </summary>
		[SerializeField]
		[Obsolete("Use ChildrenHeight with ChildrenSize.SetMaxFromPreferred instead.")]
		[FormerlySerializedAs("MaxHeight")]
		public bool MaxHeight;

		Vector2 _blockSize;

		/// <summary>
		/// Gets or sets the size of the inner block.
		/// </summary>
		/// <value>The size of the inner block.</value>
		public Vector2 BlockSize {
			get {
				return _blockSize;
			}
			protected set {
				_blockSize = value;
			}
		}

		Vector2 _uiSize;
		/// <summary>
		/// Gets or sets the UI size.
		/// </summary>
		/// <value>The UI size.</value>
		public Vector2 UISize {
			get {
				return _uiSize;
			}
			protected set {
				_uiSize = value;
			}
		}

		/// <summary>
		/// Gets the minimum height.
		/// </summary>
		/// <value>The minimum height.</value>
		public override float minHeight
		{
			get
			{
				//CalculateLayoutSize();
				return BlockSize[1];
			}
		}

		/// <summary>
		/// Gets the minimum width.
		/// </summary>
		/// <value>The minimum width.</value>
		public override float minWidth
		{
			get
			{
				//CalculateLayoutSize();
				return BlockSize[0];
			}
		}

		/// <summary>
		/// Gets the preferred height.
		/// </summary>
		/// <value>The preferred height.</value>
		public override float preferredHeight
		{
			get
			{
				//CalculateLayoutSize();
				return BlockSize[1];
			}
		}

		/// <summary>
		/// Gets the preferred width.
		/// </summary>
		/// <value>The preferred width.</value>
		public override float preferredWidth
		{
			get
			{
				//CalculateLayoutSize();
				return BlockSize[0];
			}
		}

		static readonly List<Vector2> groupPositions = new List<Vector2>{
			new Vector2(0.0f, 1.0f),//Anchors.UpperLeft
			new Vector2(0.5f, 1.0f),//Anchors.UpperCenter
			new Vector2(1.0f, 1.0f),//Anchors.UpperRight

			new Vector2(0.0f, 0.5f),//Anchors.MiddleLeft
			new Vector2(0.5f, 0.5f),//Anchors.MiddleCenter
			new Vector2(1.0f, 0.5f),//Anchors.MiddleRight
			
			new Vector2(0.0f, 0.0f),//Anchors.LowerLeft
			new Vector2(0.5f, 0.0f),//Anchors.LowerCenter
			new Vector2(1.0f, 0.0f),//Anchors.LowerRight
		};

		static readonly List<float> rowAligns = new List<float>{
			0.0f,//HorizontalAligns.Left
			0.5f,//HorizontalAligns.Center
			1.0f,//HorizontalAligns.Right
		};
		static readonly List<float> innerAligns = new List<float>{
			0.0f,//InnerAligns.Top
			0.5f,//InnerAligns.Middle
			1.0f,//InnerAligns.Bottom
		};

		protected void Changed(string propertyName)
		{
			SetDirty();

			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

			SettingsChanged.Invoke();
		}

		/// <summary>
		/// Raises the disable event.
		/// </summary>
		protected override void OnDisable()
		{
			propertiesTracker.Clear();
			base.OnDisable();
		}

		/// <summary>
		/// Raises the rect transform removed event.
		/// </summary>
		void OnRectTransformRemoved()
		{
			SetDirty();
		}

		/// <summary>
		/// Sets the layout horizontal.
		/// </summary>
		public override void SetLayoutHorizontal()
		{
			RepositionUIElements();
		}

		/// <summary>
		/// Sets the layout vertical.
		/// </summary>
		public override void SetLayoutVertical()
		{
			RepositionUIElements();
		}

		/// <summary>
		/// Calculates the layout input horizontal.
		/// </summary>
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			CalculateLayoutSize();
		}

		/// <summary>
		/// Calculates the layout input vertical.
		/// </summary>
		public override void CalculateLayoutInputVertical()
		{
			CalculateLayoutSize();
		}

		/// <summary>
		/// Gets the length.
		/// </summary>
		/// <returns>The length.</returns>
		/// <param name="ui">User interface.</param>
		/// <param name="scaled">If set to <c>true</c> scaled.</param>
		public float GetLength(RectTransform ui, bool scaled=true)
		{
			if (scaled)
			{
				return Stacking==Stackings.Horizontal ? EasyLayoutUtilites.ScaledWidth(ui) : EasyLayoutUtilites.ScaledHeight(ui);
			}
			return Stacking==Stackings.Horizontal ? ui.rect.width : ui.rect.height;
		}

		/// <summary>
		/// Calculates the size of the group.
		/// </summary>
		/// <returns>The group size.</returns>
		/// <param name="group">Group.</param>
		Vector2 CalculateGroupSize(List<List<RectTransform>> group)
		{
			float width = 0f;
			if (LayoutType==LayoutTypes.Compact)
			{
				for (int i = 0; i < group.Count; i++)
				{
					float row_width = Spacing.x * (group[i].Count - 1);
					for (int j = 0; j < group[i].Count; j++)
					{
						row_width += EasyLayoutUtilites.ScaledWidth(group[i][j]);
					}
					width = Mathf.Max(width, row_width);
				}
			}
			else
			{
				GetMaxColumnsWidths(group, MaxColumnsWidths);
				for (int i = 0; i < MaxColumnsWidths.Count; i++)
				{
					width += MaxColumnsWidths[i];
				}
				width += MaxColumnsWidths.Count * Spacing.x - Spacing.x;
			}

			float height = Spacing.y * (group.Count - 1);
			for (int i = 0; i < group.Count; i++)
			{
				float row_height = 0f;
				for (int j = 0; j < group[i].Count; j++)
				{
					row_height = Mathf.Max(row_height, EasyLayoutUtilites.ScaledHeight(group[i][j]));
				}
				height += row_height;
			}

			width += PaddingInner.Left + PaddingInner.Right;
			height += PaddingInner.Top + PaddingInner.Bottom;

			return new Vector2(width, height);
		}

		/// <summary>
		/// Marks layout to update.
		/// </summary>
		public void NeedUpdateLayout()
		{
			UpdateLayout();
		}

		/// <summary>
		/// Updates the size of the block.
		/// </summary>
		void UpdateBlockSize()
		{
			if (Symmetric)
			{
				BlockSize = new Vector2(UISize.x + Margin.x * 2, UISize.y + Margin.y * 2);
			}
			else
			{
				BlockSize = new Vector2(UISize.x + MarginLeft + MarginRight, UISize.y + MarginTop + MarginBottom);
			}
		}

		/// <summary>
		/// Calculates the size of the layout.
		/// </summary>
		public void CalculateLayoutSize()
		{
			var group = GroupUIElements();
			if (group.Count==0)
			{
				UISize = new Vector2(0, 0);
				UpdateBlockSize();
				
				return ;
			}
			
			UISize = CalculateGroupSize(group);
			UpdateBlockSize();
		}

		/// <summary>
		/// Repositions the user interface elements.
		/// </summary>
		void RepositionUIElements()
		{
			var group = GroupUIElements();
			if (group.Count==0)
			{
				UISize = new Vector2(0, 0);
				UpdateBlockSize();
				
				//LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
				return ;
			}
			
			UISize = CalculateGroupSize(group);
			UpdateBlockSize();
			
			var anchor_position = groupPositions[(int)GroupPosition];
			var start_position = new Vector2(
				rectTransform.rect.width * (anchor_position.x - rectTransform.pivot.x),
				rectTransform.rect.height * (anchor_position.y - rectTransform.pivot.y)
			);
			
			start_position.x -= anchor_position.x * UISize.x;
			start_position.y += (1 - anchor_position.y) * UISize.y;

			start_position.x += GetMarginLeft() * ((1 - anchor_position.x) * 2 - 1);
			start_position.y += GetMarginTop() * ((1 - anchor_position.y) * 2 - 1);
			
			start_position.x += PaddingInner.Left;
			start_position.y -= PaddingInner.Top;

			SetPositions(group, start_position, UISize);
			
			//LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}

		/// <summary>
		/// Updates the layout.
		/// </summary>
		public void UpdateLayout()
		{
			CalculateLayoutInputHorizontal();
			SetLayoutHorizontal();
			CalculateLayoutInputVertical();
			SetLayoutVertical();
		}

		/// <summary>
		/// Gets the user interface element position.
		/// </summary>
		/// <returns>The user interface position.</returns>
		/// <param name="ui">User interface.</param>
		/// <param name="position">Position.</param>
		/// <param name="align">Align.</param>
		Vector2 GetUIPosition(RectTransform ui, Vector2 position, Vector2 align)
		{
			var pivot_fix_x = EasyLayoutUtilites.ScaledWidth(ui) * ui.pivot.x;
			var pivox_fix_y = EasyLayoutUtilites.ScaledHeight(ui) * ui.pivot.y;
			var new_x = position.x + pivot_fix_x + align.x;
			var new_y = position.y - EasyLayoutUtilites.ScaledHeight(ui) + pivox_fix_y - align.y;
			
			return new Vector2(new_x, new_y);
		}
		
		void GetRowsWidths(List<List<RectTransform>> group, List<float> widths)
		{
			widths.Clear();

			for (int i = 0; i < group.Count; i++)
			{
				float width = 0f;
				for (int j = 0; j < group[i].Count; j++)
				{
					width += EasyLayoutUtilites.ScaledWidth(group[i][j]);
				}
				widths.Add(width + (group[i].Count - 1) * Spacing.x);
			}
		}

		void GetMaxColumnsWidths(List<List<RectTransform>> group, List<float> maxColumnsWidths)
		{
			maxColumnsWidths.Clear();

			for (var i = 0; i < group.Count; i++)
			{
				for (var j = 0; j < group[i].Count; j++)
				{
					if (maxColumnsWidths.Count <= j)
					{
						maxColumnsWidths.Add(0);
					}
					maxColumnsWidths[j] = Mathf.Max(maxColumnsWidths[j], EasyLayoutUtilites.ScaledWidth(group[i][j]));
				}
			}
		}

		void GetColumnsHeights(List<List<RectTransform>> group, List<float> heights)
		{
			heights.Clear();
			
			for (int i = 0; i < group.Count; i++)
			{
				float height = 0;
				for (int j = 0; j < group[i].Count; j++)
				{
					height += EasyLayoutUtilites.ScaledHeight(group[i][j]);
				}
				heights.Add(height + (group[i].Count - 1) * Spacing.y);
			}
		}

		float GetMaxRowHeight(List<RectTransform> row)
		{
			float height = 0;
			for (int i = 0; i < row.Count; i++)
			{
				height = Mathf.Max(height, EasyLayoutUtilites.ScaledHeight(row[i]));
			}
			return height;
		}

		void GetMaxRowsHeights(List<List<RectTransform>> group, List<float> heights)
		{
			heights.Clear();
			var transposed_group = EasyLayoutUtilites.Transpose(group);

			for (int i = 0; i < transposed_group.Count; i++)
			{
				heights.Add(GetMaxRowHeight(transposed_group[i]));
			}
		}

		Vector2 GetMaxCellSize(List<List<RectTransform>> group)
		{
			float x = 0f;
			float y = 0f;
			for (int i = 0; i < group.Count; i++)
			{
				for (int j = 0; j < group[i].Count; j++)
				{
					x = Mathf.Max(x, EasyLayoutUtilites.ScaledWidth(group[i][j]));
					y = Mathf.Max(y, EasyLayoutUtilites.ScaledHeight(group[i][j]));
				}
			}

			return new Vector2(x, y);
		}

		Vector2 GetMaxCellSize(List<RectTransform> row)
		{
			float x = 0f;
			float y = 0f;
			for (int i = 0; i < row.Count; i++)
			{
				x = Mathf.Max(x, EasyLayoutUtilites.ScaledWidth(row[i]));
				y = Mathf.Max(y, EasyLayoutUtilites.ScaledHeight(row[i]));
			}

			return new Vector2(x, y);
		}

		Vector2 GetAlignByWidth(RectTransform ui, float maxWidth, Vector2 cellMaxSize, float emptyWidth)
		{
			if (LayoutType==LayoutTypes.Compact)
			{
				return new Vector2(
					emptyWidth * rowAligns[(int)RowAlign],
					(cellMaxSize.y - EasyLayoutUtilites.ScaledHeight(ui)) * innerAligns[(int)InnerAlign]
				);
			}
			else
			{
				var cell_align = groupPositions[(int)CellAlign];

				return new Vector2(
					(maxWidth - EasyLayoutUtilites.ScaledWidth(ui)) * cell_align.x,
					(cellMaxSize.y - EasyLayoutUtilites.ScaledHeight(ui)) * (1 - cell_align.y)
				);
			}
		}

		Vector2 GetAlignByHeight(RectTransform ui, float maxHeight, Vector2 cellMaxSize, float emptyHeight)
		{
			if (LayoutType==LayoutTypes.Compact)
			{
				return new Vector2(
					(cellMaxSize.x - EasyLayoutUtilites.ScaledWidth(ui)) * innerAligns[(int)InnerAlign],
					emptyHeight * rowAligns[(int)RowAlign]
				);
			}
			else
			{
				var cell_align = groupPositions[(int)CellAlign];
				
				return new Vector2(
					(cellMaxSize.x - EasyLayoutUtilites.ScaledWidth(ui)) * (1 - cell_align.x),
					(maxHeight - EasyLayoutUtilites.ScaledHeight(ui)) * cell_align.y
				);
			}
		}

		List<float> RowsWidths = new List<float>();
		List<float> MaxColumnsWidths = new List<float>();

		List<float> ColumnsHeights = new List<float>();
		List<float> MaxRowsHeights = new List<float>();

		RectTransform currentUIElement;

		void SetPositions(List<List<RectTransform>> group, Vector2 startPosition, Vector2 groupSize)
		{
			if (Stacking==Stackings.Horizontal)
			{
				SetPositionsHorizontal(group, startPosition, groupSize);
			}
			else
			{
				SetPositionsVertical(group, startPosition, groupSize);
			}
		}

		void SetPositionsHorizontal(List<List<RectTransform>> group, Vector2 startPosition, Vector2 groupSize)
		{
			var position = startPosition;

			GetRowsWidths(group, RowsWidths);
			GetMaxColumnsWidths(group, MaxColumnsWidths);

			var align = new Vector2(0, 0);

			for (int coord_x = 0; coord_x < group.Count; coord_x++)
			{
				var row_cell_max_size = GetMaxCellSize(group[coord_x]);

				for (int coord_y = 0; coord_y < group[coord_x].Count; coord_y++)
				{
					currentUIElement = group[coord_x][coord_y];
					align = GetAlignByWidth(currentUIElement, MaxColumnsWidths[coord_y], row_cell_max_size, groupSize.x - RowsWidths[coord_x]);

					var new_position = GetUIPosition(currentUIElement, position, align);
					if (currentUIElement.localPosition.x != new_position.x || currentUIElement.localPosition.y != new_position.y)
					{
						currentUIElement.localPosition = new_position;
					}

					position.x += ((LayoutType==LayoutTypes.Compact)
						? EasyLayoutUtilites.ScaledWidth(currentUIElement)
						: MaxColumnsWidths[coord_y]) + Spacing.x;
				}
				position.x = startPosition.x;
				position.y -= row_cell_max_size.y + Spacing.y;
			}
		}

		void SetPositionsVertical(List<List<RectTransform>> group, Vector2 startPosition, Vector2 groupSize)
		{
			var position = startPosition;

			group = EasyLayoutUtilites.Transpose(group);
			GetColumnsHeights(group, ColumnsHeights);
			GetMaxRowsHeights(group, MaxRowsHeights);
			
			var align = new Vector2(0, 0);
			
			for (int coord_y = 0; coord_y < group.Count; coord_y++)
			{
				var column_cell_max_size = GetMaxCellSize(group[coord_y]);
				
				for (int coord_x = 0; coord_x < group[coord_y].Count; coord_x++)
				{
					currentUIElement = group[coord_y][coord_x];

					align = GetAlignByHeight(currentUIElement, MaxRowsHeights[coord_x], column_cell_max_size, groupSize.y - ColumnsHeights[coord_y]);
					
					var new_position = GetUIPosition(currentUIElement, position, align);
					if (currentUIElement.localPosition.x != new_position.x || currentUIElement.localPosition.y != new_position.y)
					{
						currentUIElement.localPosition = new_position;
					}
					
					position.y -= ((LayoutType==LayoutTypes.Compact)
						? EasyLayoutUtilites.ScaledHeight(currentUIElement)
						: MaxRowsHeights[coord_x]) + Spacing.y;
				}
				position.y = startPosition.y;
				position.x += column_cell_max_size.x + Spacing.x;
			}
		}

		float max_width = -1;
		float max_height = -1;
		void ResizeElements(List<RectTransform> elements)
		{
			propertiesTracker.Clear();

			if (ChildrenWidth==ChildrenSize.DoNothing && ChildrenHeight==ChildrenSize.DoNothing)
			{
				return ;
			}
			if (elements==null)
			{
				return ;
			}
			if (elements.Count==0)
			{
				return ;
			}
			if (LayoutType==LayoutTypes.Grid && GridConstraint!=GridConstraints.Flexible)
			{
				//elements.ForEach(Add2Tracker);
				//return ;
			}

			max_width = (ChildrenWidth==ChildrenSize.SetMaxFromPreferred) ? elements.Select<RectTransform,float>(EasyLayoutUtilites.GetPreferredWidth).Max() : -1f;
			max_height = (ChildrenHeight==ChildrenSize.SetMaxFromPreferred) ? elements.Select<RectTransform,float>(EasyLayoutUtilites.GetPreferredHeight).Max() : -1f;

			elements.ForEach(ResizeChild);
		}

		DrivenRectTransformTracker propertiesTracker;

		void Add2Tracker(RectTransform child)
		{
			DrivenTransformProperties driven_properties = DrivenTransformProperties.None;

			if (ChildrenWidth!=ChildrenSize.DoNothing)
			{
				driven_properties |= DrivenTransformProperties.SizeDeltaX;
			}
			if (ChildrenHeight!=ChildrenSize.DoNothing)
			{
				driven_properties |= DrivenTransformProperties.SizeDeltaY;
			}

			propertiesTracker.Add(this, child, driven_properties);
		}

		void ResizeChild(RectTransform child)
		{
			DrivenTransformProperties driven_properties = DrivenTransformProperties.None;

			if (ChildrenWidth!=ChildrenSize.DoNothing)
			{
				driven_properties |= DrivenTransformProperties.SizeDeltaX;
				var width = (max_width!=-1) ? max_width : EasyLayoutUtilites.GetPreferredWidth(child);
				child.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
			}
			if (ChildrenHeight!=ChildrenSize.DoNothing)
			{
				driven_properties |= DrivenTransformProperties.SizeDeltaY;
				var height = (max_height!=-1) ? max_height : EasyLayoutUtilites.GetPreferredHeight(child);
				child.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
			}

			propertiesTracker.Add(this, child, driven_properties);
		}

		bool IsIgnoreLayout(Transform rect)
		{
			#if UNITY_4_6 || UNITY_4_7
			var ignorer = rect.GetComponent(typeof(ILayoutIgnorer)) as ILayoutIgnorer;
			#else
			var ignorer = rect.GetComponent<ILayoutIgnorer>();
			#endif
			return (ignorer!=null) && ignorer.ignoreLayout;
		}

		List<RectTransform> GetUIElements()
		{
			var elements = rectChildren;

			if (!SkipInactive)
			{
				elements = new List<RectTransform>();
				foreach (Transform child in transform)
				{
					if (!IsIgnoreLayout(child))
					{
						elements.Add(child as RectTransform);
					}
				}
			}

			if (Filter!=null)
			{
				var temp = Filter(elements.Convert<RectTransform,GameObject>(GetGameObject));
				var result = temp.Select<GameObject,RectTransform>(GetRectTransform).ToList();

				ResizeElements(result);

				return result;
			}

			ResizeElements(elements);

			return elements;
		}

		GameObject GetGameObject(RectTransform element)
		{
			return element.gameObject;
		}

		RectTransform GetRectTransform(GameObject go)
		{
			return go.transform as RectTransform;
		}

		/// <summary>
		/// Gets the margin top.
		/// </summary>
		public float GetMarginTop()
		{
			return Symmetric ? Margin.y : MarginTop;
		}
		
		/// <summary>
		/// Gets the margin bottom.
		/// </summary>
		public float GetMarginBottom()
		{
			return Symmetric ? Margin.y : MarginBottom;
		}

		/// <summary>
		/// Gets the margin left.
		/// </summary>
		public float GetMarginLeft()
		{
			return Symmetric ? Margin.x : MarginLeft;
		}

		/// <summary>
		/// Gets the margin right.
		/// </summary>
		public float GetMarginRight()
		{
			return Symmetric ? Margin.y : MarginRight;
		}

		void ReverseList(List<RectTransform> list)
		{
			list.Reverse();
		}

		void ClearUIElementsGroup()
		{
			for (int i = 0; i < uiElementsGroup.Count; i++)
			{
				uiElementsGroup[i].Clear();
				ListCache.Push(uiElementsGroup[i]);
			}
			uiElementsGroup.Clear();
		}

		static Stack<List<RectTransform>> ListCache = new Stack<List<RectTransform>>();

		/// <summary>
		/// Gets the List<RectTransform>.
		/// </summary>
		/// <returns>The rect transform list.</returns>
		public List<RectTransform> GetRectTransformList()
		{
			if (ListCache.Count > 0)
			{
				return ListCache.Pop();
			}
			else
			{
				return new List<RectTransform>();
			}
		}

		List<List<RectTransform>> uiElementsGroup = new List<List<RectTransform>>();
		List<List<RectTransform>> GroupUIElements()
		{
			var base_length = GetLength(rectTransform, false);
			base_length -= (Stacking==Stackings.Horizontal) ? (GetMarginLeft() + GetMarginRight()) : (GetMarginTop() + GetMarginBottom());

			var ui_elements = GetUIElements();

			ClearUIElementsGroup();
			if (LayoutType==LayoutTypes.Compact)
			{
				EasyLayoutCompact.Group(ui_elements, base_length, this, uiElementsGroup);

				float rows = 0f;
				float columns = 0f;
				if (Stacking==Stackings.Vertical)
				{
					columns = uiElementsGroup.Count;
					uiElementsGroup = EasyLayoutUtilites.Transpose(uiElementsGroup);
					rows = uiElementsGroup.Count;
				}
				else
				{
					rows = uiElementsGroup.Count;
					columns = rows > 0 ? uiElementsGroup.Max(x => x.Count) : 0;
				}

				CompactConstraintCount = Mathf.Max(1, CompactConstraintCount);
				if ((CompactConstraint==CompactConstraints.MaxRowCount) && (rows>CompactConstraintCount))
				{
					ClearUIElementsGroup();
					if (Stacking==Stackings.Horizontal)
					{
						EasyLayoutGrid.GroupByRowsHorizontal(ui_elements, this, CompactConstraintCount, uiElementsGroup);
					}
					else
					{
						EasyLayoutGrid.GroupByRowsVertical(ui_elements, this, CompactConstraintCount, uiElementsGroup);
					}
				}
				else if ((CompactConstraint==CompactConstraints.MaxColumnCount) && (columns>CompactConstraintCount))
				{
					ClearUIElementsGroup();
					if (Stacking==Stackings.Horizontal)
					{
						EasyLayoutGrid.GroupByColumnsHorizontal(ui_elements, this, CompactConstraintCount, uiElementsGroup);
					}
					else
					{
						EasyLayoutGrid.GroupByColumnsVertical(ui_elements, this, CompactConstraintCount, uiElementsGroup);
					}
				}
			}
			else
			{
				GridConstraintCount = Mathf.Max(1, GridConstraintCount);
				if (GridConstraint==GridConstraints.Flexible)
				{
					EasyLayoutGrid.GroupFlexible(ui_elements, base_length, this, uiElementsGroup);
				}
				else if (GridConstraint==GridConstraints.FixedRowCount)
				{
					if (Stacking==Stackings.Vertical)
					{
						EasyLayoutGrid.GroupByRowsVertical(ui_elements, this, GridConstraintCount, uiElementsGroup);
					}
					else
					{
						EasyLayoutGrid.GroupByRowsHorizontal(ui_elements, this, GridConstraintCount, uiElementsGroup);
					}
				}
				else if (GridConstraint==GridConstraints.FixedColumnCount)
				{
					if (Stacking==Stackings.Vertical)
					{
						EasyLayoutGrid.GroupByColumnsVertical(ui_elements, this, GridConstraintCount, uiElementsGroup);
					}
					else
					{
						EasyLayoutGrid.GroupByColumnsHorizontal(ui_elements, this, GridConstraintCount, uiElementsGroup);
					}
				}
			}

			if (!TopToBottom)
			{
				uiElementsGroup.Reverse();
			}
			
			if (RightToLeft)
			{
				uiElementsGroup.ForEach(ReverseList);
			}

			var width = rectTransform.rect.width - (GetMarginLeft() + GetMarginRight());
			var height = rectTransform.rect.height - (GetMarginTop() + GetMarginBottom());
			var size = new Vector2(width, height);
			if ((ChildrenWidth==ChildrenSize.ShrinkOnOverflow) && (ChildrenHeight==ChildrenSize.ShrinkOnOverflow))
			{
				if (uiElementsGroup.Count>0)
				{
					UISize = CalculateGroupSize(uiElementsGroup);
					UpdateBlockSize();

					var rows = uiElementsGroup.Count - 1;
					var columns = uiElementsGroup.Max(x => x.Count) - 1;
					var size_without_spacing = new Vector2(size.x - (Spacing.x * columns), size.y - (Spacing.y * rows));
					var ui_size_without_spacing = new Vector2(UISize.x - (Spacing.x * columns), UISize.y - (Spacing.y * rows));
					var scale = EasyLayoutUtilites.GetShrinkScale(size_without_spacing, ui_size_without_spacing);
					foreach (var row in uiElementsGroup)
					{
						foreach (var elem in row)
						{
							elem.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, elem.rect.width * scale);
							elem.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, elem.rect.height * scale);
						}
					}
					//size = EasyLayoutUtilites.GetShrinkSize(size_without_spacing, ui_size_without_spacing);
					//size = new Vector2(size.x + (Spacing.x * columns), size.y + (Spacing.y * rows));
					//EasyLayoutUtilites.ShrinkToFit(new_size, uiElementsGroup, Spacing, PaddingInner);
				}
			}
			else if (LayoutType==LayoutTypes.Grid)
			{
				if (ChildrenWidth==ChildrenSize.FitContainer)
				{
					EasyLayoutUtilites.ResizeColumnWidthToFit(size.x, uiElementsGroup, Spacing.x, PaddingInner.Left + PaddingInner.Right);
				}
				else if (ChildrenWidth==ChildrenSize.ShrinkOnOverflow)
				{
					EasyLayoutUtilites.ShrinkColumnWidthToFit(size.x, uiElementsGroup, Spacing.x, PaddingInner.Left + PaddingInner.Right);
				}

				if (ChildrenHeight==ChildrenSize.FitContainer)
				{
					EasyLayoutUtilites.ResizeRowHeightToFit(size.y, uiElementsGroup, Spacing.y, PaddingInner.Top + PaddingInner.Bottom);
				}
				else if (ChildrenHeight==ChildrenSize.ShrinkOnOverflow)
				{
					EasyLayoutUtilites.ShrinkRowHeightToFit(size.y, uiElementsGroup, Spacing.y, PaddingInner.Top + PaddingInner.Bottom);
				}
			}
			else
			{
				if (Stacking==Stackings.Horizontal)
				{
					if (ChildrenWidth==ChildrenSize.FitContainer)
					{
						EasyLayoutUtilites.ResizeWidthToFit(size.x, uiElementsGroup, Spacing.x);
					}
					else if (ChildrenWidth==ChildrenSize.ShrinkOnOverflow)
					{
						EasyLayoutUtilites.ShrinkWidthToFit(size.x, uiElementsGroup, Spacing.x);
					}

					if (ChildrenHeight==ChildrenSize.FitContainer)
					{
						EasyLayoutUtilites.ResizeRowHeightToFit(size.y, uiElementsGroup, Spacing.y, PaddingInner.Top + PaddingInner.Bottom);
					}
					else if (ChildrenHeight==ChildrenSize.ShrinkOnOverflow)
					{
						EasyLayoutUtilites.ShrinkRowHeightToFit(size.y, uiElementsGroup, Spacing.y, PaddingInner.Top + PaddingInner.Bottom);
					}
				}
				else
				{
					if (ChildrenWidth==ChildrenSize.FitContainer)
					{
						EasyLayoutUtilites.ResizeColumnWidthToFit(size.x, uiElementsGroup, Spacing.x, PaddingInner.Left + PaddingInner.Right);
					}
					else if (ChildrenWidth==ChildrenSize.ShrinkOnOverflow)
					{
						EasyLayoutUtilites.ShrinkColumnWidthToFit(size.x, uiElementsGroup, Spacing.x, PaddingInner.Left + PaddingInner.Right);
					}

					if (ChildrenHeight==ChildrenSize.FitContainer)
					{
						EasyLayoutUtilites.ResizeHeightToFit(size.y, uiElementsGroup, Spacing.y);
					}
					else if (ChildrenHeight==ChildrenSize.ShrinkOnOverflow)
					{
						EasyLayoutUtilites.ShrinkHeightToFit(size.y, uiElementsGroup, Spacing.y);
					}
				}
			}

			return uiElementsGroup;
		}

		/// <summary>
		/// Awake this instance.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();
			Upgrade();
		}

		[SerializeField]
		int version = 0;

		#pragma warning disable 0618
		/// <summary>
		/// Upgrade to keep compatibility between versions.
		/// </summary>
		public virtual void Upgrade()
		{
			//upgrade to 1.6
			if (version==0)
			{
				if (ControlWidth)
				{
					ChildrenWidth = (MaxWidth) ? ChildrenSize.SetMaxFromPreferred : ChildrenSize.SetPreferred;
				}
				if (ControlHeight)
				{
					ChildrenHeight = (MaxHeight) ? ChildrenSize.SetMaxFromPreferred : ChildrenSize.SetPreferred;
				}
			}
			version = 1;
		}
		#pragma warning restore 0618
	}
}