using UnityEngine;
using UnityEngine.UI;

namespace UIWidgetsSamples {
	public class DialogInputHelper : MonoBehaviour {
		[SerializeField]
		public InputField Username;

		[SerializeField]
		public InputField Password;

		public void Refresh()
		{
			Username.text = "";
			Password.text = "";
		}

		public bool Validate()
		{
			var valid_username = Username.text.Trim().Length > 0;
			var valid_password = Password.text.Length > 0;

			if (!valid_username)
			{
				Username.Select();
			}
			else if (!valid_password)
			{
				Password.Select();
			}

			return valid_username && valid_password;
		}
	}
}