using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIWidgets {
	/// <summary>
	/// Used only to attach custom editor DragSupport.
	/// </summary>
	public abstract class BaseDragSupport : MonoBehaviour {
		/// <summary>
		/// The drag points.
		/// </summary>
		protected static Dictionary<Transform,RectTransform> DragPoints;

		Transform canvasTransform;

		/// <summary>
		/// Gets a canvas transform of current gameobject.
		/// </summary>
		protected Transform CanvasTransform {
			get {
				if (canvasTransform==null)
				{
					canvasTransform = Utilites.FindTopmostCanvas(transform);
				}
				return canvasTransform;
			}
		}

		/// <summary>
		/// Gets the drag point.
		/// </summary>
		public RectTransform DragPoint {
			get {
				if (DragPoints==null)
				{
					DragPoints = new Dictionary<Transform, RectTransform>();
				}
				if (!DragPoints.ContainsKey(CanvasTransform))
				{
					var go = new GameObject("DragPoint");
					var dragPoint = go.AddComponent<RectTransform>();
					dragPoint.SetParent(CanvasTransform, false);
					dragPoint.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
					dragPoint.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);

					DragPoints.Add(CanvasTransform, dragPoint);
				}
				return DragPoints[CanvasTransform];
			}
		}

	}
}