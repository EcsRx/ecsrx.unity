using UnityEngine;
using UIWidgets;

[RequireComponent(typeof(Progressbar))]
public class SampleProgressbar : MonoBehaviour
{
	void Start()
	{
		var bar = GetComponent<Progressbar>();
		bar.TextFunc = x => {
			return string.Format("Exp to next level: {0} / {1}", x.Value, x.Max);
		};
	}

}
