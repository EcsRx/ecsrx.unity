using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIWidgets {
	/// <summary>
	/// Multiple connector.
	/// </summary>
	[ExecuteInEditMode]
	public class MultipleConnector : ConnectorBase {
		[SerializeField]
		List<ConnectorLine> connectorLines;

		/// <summary>
		/// The lines.
		/// </summary>
		protected ObservableList<ConnectorLine> lines;

		/// <summary>
		/// Gets or sets the lines.
		/// </summary>
		/// <value>The lines.</value>
		public ObservableList<ConnectorLine> Lines {
			get {
				if (lines==null)
				{
					lines = (connectorLines!=null) ? new ObservableList<ConnectorLine>(connectorLines) :  new ObservableList<ConnectorLine>();
					lines.OnChange += LinesChanged;
				}
				return lines;
			}
			set {
				if (lines!=null)
				{
					lines.OnChange -= LinesChanged;
				}
				lines = value;
				if (lines!=null)
				{
					lines.OnChange += LinesChanged;
				}
				LinesChanged();
			}
		}

		/// <summary>
		/// The listeners.
		/// </summary>
		protected List<TransformListener> listeners = new List<TransformListener>();

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected override void Start()
		{
			base.Start();

			if (lines!=null)
			{
				lines.OnChange -= LinesChanged;
			}
			lines = (connectorLines!=null) ? new ObservableList<ConnectorLine>(connectorLines) :  new ObservableList<ConnectorLine>();
			lines.OnChange += LinesChanged;

			LinesChanged();
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
			RemoveListeners();
			base.OnDestroy();
		}

		/// <summary>
		/// Called when lines changed.
		/// </summary>
		protected virtual void LinesChanged()
		{
			RemoveListeners();
			AddListeners();
			SetVerticesDirty();
		}

		/// <summary>
		/// Removes the listeners.
		/// </summary>
		protected virtual void RemoveListeners()
		{
			listeners.ForEach(x => {
				if (x!=null)
				{
					x.OnTransformChanged.RemoveListener(SetVerticesDirty);
				}
			});
			listeners.Clear();
		}

		/// <summary>
		/// Adds the listeners.
		/// </summary>
		protected virtual void AddListeners()
		{
			Lines.ForEach(x => {
				if (x.Target!=null)
				{
					var listener = x.Target.GetComponent<TransformListener>();
					if (listener==null)
					{
						listener = x.Target.gameObject.AddComponent<TransformListener>();
					}
					listener.OnTransformChanged.AddListener(SetVerticesDirty);

					listeners.Add(listener);
				}
			});
		}

		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1
		/// <summary>
		/// Generate vertex for lines.
		/// </summary>
		/// <param name="vbo">Vbo.</param>
		protected override void OnFillVBO(List<UIVertex> vbo)
		{
			foreach (var line in Lines)
			{
				AddLine(rectTransform, line, vbo);
			}
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
				int index = 0;
				foreach (var line in Lines)
				{
					index += AddLine(rectTransform, line, vh, index);
				}
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
			int index = 0;
			vh.Clear();
			foreach (var line in Lines)
			{
				index += AddLine(rectTransform, line, vh, index);
			}
		}
		#endif
	}
}