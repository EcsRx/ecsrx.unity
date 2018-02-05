using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace UIWidgets
{
	public class ListViewCustomEditor : ListViewCustomBaseEditor
	{
		public ListViewCustomEditor()
		{
			IsListViewCustom = true;
		}
	}
}