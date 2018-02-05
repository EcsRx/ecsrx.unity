using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Sprites;

namespace UIWidgets
{
	/// <summary>
	/// Connector base class.
	/// </summary>
	[RequireComponent(typeof(TransformListener))]
	public abstract class ConnectorBase : MaskableGraphic
	{
		[SerializeField]
		Sprite sprite;

		/// <summary>
		/// Gets or sets the sprite.
		/// </summary>
		/// <value>The sprite.</value>
		public Sprite Sprite {
			get {
				return sprite;
			}
			set {
				sprite = value;
				SetAllDirty();
			}
		}

        /// <summary>
        /// Image's texture comes from the UnityEngine.Image.
        /// </summary>
        public override Texture mainTexture
        {
            get
            {
				return sprite == null ? s_WhiteTexture : sprite.texture;
            }
        }

		TransformListener currentTransformListener;

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected override void Start()
		{
			base.Start();

			AddSelfListener();
		}

		/// <summary>
		/// Adds the self listener.
		/// </summary>
		protected void AddSelfListener()
		{
			if (currentTransformListener==null)
			{
				currentTransformListener = GetComponent<TransformListener>();
			}
			if (currentTransformListener!=null)
			{
				currentTransformListener.OnTransformChanged.AddListener(SetVerticesDirty);
			}
		}

		/// <summary>
		/// Removes the self listener.
		/// </summary>
		protected void RemoveSelfListener()
		{
			if (currentTransformListener!=null)
			{
				currentTransformListener.OnTransformChanged.RemoveListener(SetVerticesDirty);
			}
		}

		#if UNITY_EDITOR
		/// <summary>
		/// This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
		/// </summary>
		protected override void OnValidate()
		{
			RemoveSelfListener();
			AddSelfListener();

			base.OnValidate();
		}
		#endif

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected override void OnDestroy()
		{
			RemoveSelfListener();
			base.OnDestroy();
		}

		/// <summary>
		/// Gets the point.
		/// </summary>
		/// <returns>The point.</returns>
		/// <param name="currentRectTransform">Current rect transform.</param>
		/// <param name="position">Position.</param>
		protected Vector3 GetPoint(RectTransform currentRectTransform, ConnectorPosition position)
		{
			var rect = (! canvas || !canvas.pixelPerfect)
				? currentRectTransform.rect
				: RectTransformUtility.PixelAdjustRect(currentRectTransform, canvas);
			
			var delta = rectTransform.position - currentRectTransform.position;

			rect.x -= delta.x;
			rect.y -= delta.y;

			switch (position)
			{
				case ConnectorPosition.Left:
					rect.y += rect.height / 2f;
					break;
				case ConnectorPosition.Right:
					rect.x += rect.width;
					rect.y += rect.height / 2f;
					break;
				case ConnectorPosition.Top:
					rect.x += rect.width / 2f;
					rect.y += rect.height;
					break;
				case ConnectorPosition.Bottom:
					rect.x += rect.width / 2f;
					break;
				case ConnectorPosition.Center:
					rect.x += rect.width / 2f;
					rect.y += rect.height / 2f;
					break;
			}

			return new Vector3(rect.x, rect.y);
		}

		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1
		/// <summary>
		/// Adds the line.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="line">Line.</param>
		/// <param name="vbo">Vbo.</param>
		public void AddLine(RectTransform source, ConnectorLine line, List<UIVertex> vbo)
		{
			if ((line==null) || (line.Target==null) || (!line.Target.gameObject.activeInHierarchy))
			{
				return ;
			}
			var start_point = GetPoint(source, line.Start);
			var end_point = GetPoint(line.Target, line.End);

			SetPoints(start_point, end_point, line.Thickness, vbo);
		}

		/// <summary>
		/// Sets the points.
		/// </summary>
		/// <param name="point1">Point1.</param>
		/// <param name="point2">Point2.</param>
		/// <param name="width">Width.</param>
		/// <param name="vbo">Vbo.</param>
		void SetPoints(Vector3 point1, Vector3 point2, float width, List<UIVertex> vbo)
		{
			var angle_z = Mathf.Atan2(point2.y - point1.y, point2.x - point1.x) * 180f / Mathf.PI;
			var angle = Quaternion.Euler(new Vector3(0, 0, angle_z));
			
			var vertex1 = point1 + new Vector3(0, -width / 2);
			var vertex2 = point1 + new Vector3(0, +width / 2);
			var vertex3 = point2 + new Vector3(0, +width / 2);
			var vertex4 = point2 + new Vector3(0, -width / 2);

			vertex1 = RotatePoint(vertex1, point1, angle);
			vertex2 = RotatePoint(vertex2, point1, angle);
			vertex3 = RotatePoint(vertex3, point2, angle);
			vertex4 = RotatePoint(vertex4, point2, angle);

			var uv = (Sprite!=null) ? DataUtility.GetOuterUV(Sprite) : Vector4.zero;
			
			var vert = UIVertex.simpleVert;
			vert.color = color;

			vert.position = vertex1;
			vert.uv0 = new Vector2(uv.x, uv.y);
			vbo.Add(vert);

			vert.position = vertex2;
			vert.uv0 = new Vector2(uv.x, uv.w);
			vbo.Add(vert);

			vert.position = vertex3;
			vert.uv0 = new Vector2(uv.z, uv.w);
			vbo.Add(vert);

			vert.position = vertex4;
			vert.uv0 = new Vector2(uv.z, uv.y);
			vbo.Add(vert);
		}
		#else
		/// <summary>
		/// Adds the line.
		/// </summary>
		/// <returns>The line.</returns>
		/// <param name="source">Source.</param>
		/// <param name="line">Line.</param>
		/// <param name="vh">Vertex helper.</param>
		/// <param name="index">Index.</param>
		public int AddLine(RectTransform source, ConnectorLine line, VertexHelper vh, int index)
		{
			if ((line==null) || (line.Target==null) || (!line.Target.gameObject.activeInHierarchy))
			{
				return 0;
			}

			var start_point = GetPoint(source, line.Start);
			var end_point = GetPoint(line.Target, line.End);

			return SetPoints(start_point, end_point, line.Thickness, vh, index);
		}

		/// <summary>
		/// Sets the points.
		/// </summary>
		/// <returns>The points.</returns>
		/// <param name="point1">Point1.</param>
		/// <param name="point2">Point2.</param>
		/// <param name="width">Width.</param>
		/// <param name="vh">Vertex helper.</param>
		/// <param name="index">Index.</param>
		int SetPoints(Vector3 point1, Vector3 point2, float width, VertexHelper vh, int index)
		{
			var angle_z = Mathf.Atan2(point2.y - point1.y, point2.x - point1.x) * 180f / Mathf.PI;
			var angle = Quaternion.Euler(new Vector3(0, 0, angle_z));
			
			var vertex1 = point1 + new Vector3(0, -width / 2);
			var vertex2 = point1 + new Vector3(0, +width / 2);
			var vertex3 = point2 + new Vector3(0, +width / 2);
			var vertex4 = point2 + new Vector3(0, -width / 2);

			vertex1 = RotatePoint(vertex1, point1, angle);
			vertex2 = RotatePoint(vertex2, point1, angle);
			vertex3 = RotatePoint(vertex3, point2, angle);
			vertex4 = RotatePoint(vertex4, point2, angle);

			var uv = (Sprite!=null) ? DataUtility.GetOuterUV(Sprite) : Vector4.zero;

			Color32 color32 = color;
			vh.AddVert(vertex1, color32, new Vector2(uv.x, uv.y));
			vh.AddVert(vertex2, color32, new Vector2(uv.x, uv.w));
			vh.AddVert(vertex3, color32, new Vector2(uv.z, uv.w));
			vh.AddVert(vertex4, color32, new Vector2(uv.z, uv.y));

			vh.AddTriangle(index + 0, index + 1, index + 2);
			vh.AddTriangle(index + 2, index + 3, index + 0);

			return 4;
		}
		#endif

		/// <summary>
		/// Rotates the point.
		/// </summary>
		/// <returns>The point.</returns>
		/// <param name="point">Point.</param>
		/// <param name="pivot">Pivot.</param>
		/// <param name="angle">Angle.</param>
		static Vector3 RotatePoint(Vector3 point, Vector3 pivot, Quaternion angle)
		{
			var direction = angle * (point - pivot);

			return direction + pivot;
		}
	}
}

