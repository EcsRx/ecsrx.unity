using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace UIWidgets {
	public class ComparisonComparer<T> : IComparer<T>
	{
		readonly Comparison<T> comparison;
		
		public ComparisonComparer(Comparison<T> newComparison)
		{
			if (newComparison==null)
			{
				throw new ArgumentNullException("newComparison");
			}
			comparison = newComparison;
		}
		
		public int Compare(T x, T y)
		{
			return comparison(x, y);
		}
	}
}