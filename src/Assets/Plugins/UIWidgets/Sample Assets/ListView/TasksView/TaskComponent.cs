using UnityEngine;
using UnityEngine.UI;
using UIWidgets;

namespace UIWidgetsSamples.Tasks {
	public class TaskComponent : ListViewItem {
		public Text Name;

		public Progressbar Progressbar;

		Task item;

		public Task Item {
			get {
				return item;
			}
			set {
				if (item!=null)
				{
					item.OnProgressChange -= UpdateProgressbar;
				}
				item = value;
				if (item!=null)
				{
					Name.text = item.Name;
					Progressbar.Value = item.Progress;

					item.OnProgressChange += UpdateProgressbar;
				}
			}
		}

		public void SetData(Task item)
		{
			Item = item;
		}

		void UpdateProgressbar()
		{
			Progressbar.Animate(item.Progress);
		}

		protected override void OnDestroy()
		{
			Item = null;
		}

		protected bool IsColorSetted;
		
		public virtual void Coloring(Color primary, Color background, float fadeDuration)
		{
			// reset default color to white, otherwise it will look darker than specified color,
			// because actual color = Text.color * Text.CrossFadeColor
			if (!IsColorSetted)
			{
				Name.color = Color.white;
				Background.color = Color.white;
			}

			// change color instantly for first time
			Name.CrossFadeColor(primary, IsColorSetted ? fadeDuration : 0f, true, true);
			Background.CrossFadeColor(background, IsColorSetted ? fadeDuration : 0f, true, true);

			IsColorSetted = true;
		}
	}
}