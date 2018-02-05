using UnityEngine;
using UnityEngine.Events;

namespace UIWidgets
{
	/// <summary>
	/// Transform listener.
	/// </summary>
	[ExecuteInEditMode]
	public class TransformListener : MonoBehaviour
	{
		/// <summary>
		/// The OnResize event.
		/// </summary>
		public UnityEvent OnTransformChanged = new UnityEvent();

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		protected virtual void Update()
		{
			if (transform.hasChanged)
			{
				OnTransformChanged.Invoke();
				transform.hasChanged = false;
			}
		}

		/// <summary>
		/// This function is called when the object becomes enabled and active.
		/// </summary>
		protected virtual void OnEnable()
		{
			OnTransformChanged.Invoke();
		}


		/// <summary>
		/// This function is called when the behaviour becomes disabled or inactive.
		/// </summary>
		protected virtual void OnDisable()
		{
			OnTransformChanged.Invoke();
		}
	}
}