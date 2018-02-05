using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIWidgets {
	/// <summary>
	/// Single connector.
	/// </summary>
	[ExecuteInEditMode]
	public class SingleConnector : ConnectorBase {
		/// <summary>
		/// The line.
		/// </summary>
		[SerializeField]
		protected ConnectorLine line;

		/// <summary>
		/// Gets or sets the line.
		/// </summary>
		/// <value>The line.</value>
		public ConnectorLine Line {
			get {
				return line;
			}
			set {
				if (line!=null)
				{
					line.OnChange -= LinesChanged;
				}
				line = value;
				if (line!=null)
				{
					line.OnChange += LinesChanged;
				}
				LinesChanged();
			}
		}

		/// <summary>
		/// The listener.
		/// </summary>
		protected TransformListener listener;

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected override void Start()
		{
			base.Start();

			Line = line;
		}

		/// <summary>
		/// This function is called when the object becomes enabled and active.
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();

			LinesChanged();
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected override void OnDestroy()
		{
			RemoveListener();

			base.OnDestroy();
		}

		/// <summary>
		/// Lineses the changed.
		/// </summary>
		protected virtual void LinesChanged()
		{
			RemoveListener();
			AddListener();
			SetVerticesDirty();
		}

		/// <summary>
		/// Removes the listener.
		/// </summary>
		protected virtual void RemoveListener()
		{
			if (listener!=null)
			{
				listener.OnTransformChanged.RemoveListener(SetVerticesDirty);
			}
		}

		/// <summary>
		/// Adds the listener.
		/// </summary>
		protected virtual void AddListener()
		{
			if ((Line!=null) && (Line.Target!=null))
			{
				listener = Line.Target.GetComponent<TransformListener>();
				if (listener==null)
				{
					listener = Line.Target.gameObject.AddComponent<TransformListener>();
				}
				listener.OnTransformChanged.AddListener(SetVerticesDirty);
			}
		}

		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1
		/// <summary>
		/// Generate vertex for line.
		/// </summary>
		/// <param name="vbo">Vbo.</param>
		protected override void OnFillVBO(List<UIVertex> vbo)
		{
			AddLine(rectTransform, Line, vbo);
		}
		#elif UNITY_5_2
		/// <summary>
		/// Fill the vertex buffer data.
		/// </summary>
		/// <param name="m">Mesh.</param>
		protected override void OnPopulateMesh(Mesh m)
		{
			using (VertexHelper vh = new VertexHelper())
			{
				AddLine(rectTransform, Line, vh, 0);
				vh.FillMesh(m);
			}
		}
		#else
		/// <summary>
		/// Fill the vertex buffer data.
		/// </summary>
		/// <param name="vh">VertexHelper.</param>
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			AddLine(rectTransform, Line, vh, 0);
		}
		#endif
	}
}