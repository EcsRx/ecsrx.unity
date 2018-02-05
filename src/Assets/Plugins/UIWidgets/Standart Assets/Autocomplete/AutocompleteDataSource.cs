using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UIWidgets {
	/// <summary>
	/// AutocompleteDataSource.
	/// Set Autocomplete.DataSource with strings from file.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/AutocompleteDataSource")]
	[RequireComponent(typeof(Autocomplete))]
	public class AutocompleteDataSource : MonoBehaviour {
		[SerializeField]
		TextAsset file;

		/// <summary>
		/// Gets or sets the file.
		/// </summary>
		/// <value>The file.</value>
		public TextAsset File {
			get {
				return file;
			}
			set {
				file = value;
				if (file!=null)
				{
					SetDataSource(file);
				}
			}
		}

		/// <summary>
		/// The comments in file start with specified strings.
		/// </summary>
		[SerializeField]
		public List<string> CommentsStartWith = new List<string>(){"#", "//"};

		bool isStarted;

		/// <summary>
		/// Start this instance.
		/// </summary>
		public virtual void Start()
		{
			if (isStarted)
			{
				return ;
			}
			isStarted = true;

			File = file;
		}

		/// <summary>
		/// Gets the items from file.
		/// </summary>
		/// <param name="sourceFile">Source file.</param>
		public virtual void SetDataSource(TextAsset sourceFile)
		{
			if (file==null)
			{
				return ;
			}

			var new_items = sourceFile.text.Split(new string[] {"\r\n", "\r", "\n"}, StringSplitOptions.None).Select<string,string>(StringTrimEnd).Where(x => x!="");

			if (CommentsStartWith.Count > 0)
			{
				new_items = new_items.Where(NotComment);
			}

			GetComponent<Autocomplete>().DataSource = new_items.ToList();
		}

		/// <summary>
		/// Removes ending characters from specified string.
		/// </summary>
		/// <returns>String.</returns>
		/// <param name="str">String.</param>
		protected string StringTrimEnd(string str)
		{
			return str.TrimEnd();
		}

		/// <summary>
		/// Check if string is the comment.
		/// </summary>
		/// <returns><c>true</c>, if string not comment, <c>false</c> otherwise.</returns>
		/// <param name="str">String.</param>
		protected virtual bool NotComment(string str)
		{
			return !CommentsStartWith.Any(str.StartsWith);
		}
	}
}