using UnityEngine;
using System.Collections;

namespace UIWidgets {

	/// <summary>
	/// Draggable UI object..
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/Draggable")]
	[RequireComponent(typeof(RectTransform))]
	public class Draggable : MonoBehaviour {

		/// <summary>
		/// The handle.
		/// </summary>
		[SerializeField]
		GameObject handle;

		DraggableHandle handleScript;

		/// <summary>
		/// If specified, restricts dragging from starting unless the pointerdown occurs on the specified element.
		/// </summary>
		/// <value>The handler.</value>
		public GameObject Handle {
			get {
				return handle;
			}
			set {
				SetHandle(value);
			}
		}

		void Start()
		{
			SetHandle(handle!=null ? handle : gameObject);
		}

		/// <summary>
		/// Sets the handle.
		/// </summary>
		/// <param name="value">Value.</param>
		protected virtual void SetHandle(GameObject value)
		{
			if (handle)
			{
				Destroy(handleScript);
			}
			handle = value;
			handleScript = handle.AddComponent<DraggableHandle>();
			handleScript.Drag(gameObject.transform as RectTransform);
		}
	}
}