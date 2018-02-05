using UnityEngine;
using System.Collections;

namespace UIWidgets
{
	[RequireComponent(typeof(RectTransform))]
	/// <summary>
	/// Slide up. Helper component for Notify.
	/// </summary>
	public class SlideUp : MonoBehaviour
	{
		public bool UnscaledTime;

		RectTransform rect;
		void Awake()
		{
			rect = transform as RectTransform;
		}

		/// <summary>
		/// Start animation.
		/// </summary>
		public void Run()
		{
			StartCoroutine(StartAnimation());
		}

		IEnumerator StartAnimation()
		{
			yield return StartCoroutine(AnimationCollapse());

			gameObject.SetActive(false);
		}

		void OnDisable()
		{
			Notify.FreeSlide(rect);
		}

		IEnumerator AnimationCollapse()
		{
			var max_height = rect.rect.height;
			var speed = 200f;//pixels per second
			
			var time = max_height / speed;
			var end_time = GetTime() + time;
			
			while (GetTime() <= end_time)
			{
				var height = Mathf.Lerp(max_height, 0, 1 - (end_time - GetTime()) / time);

				rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
				yield return null;
			}
			
			//return height back for future use
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, max_height);
		}

		float GetTime()
		{
			return UnscaledTime ? Time.unscaledTime : Time.time;
		}
	}
}