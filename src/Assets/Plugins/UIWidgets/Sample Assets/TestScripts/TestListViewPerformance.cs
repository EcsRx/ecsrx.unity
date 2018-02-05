using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using UIWidgets;
using System.Diagnostics;

namespace UIWidgetsSamples {

	public class TestListViewPerformance : MonoBehaviour {
		[SerializeField]
		ListView lv;

		[SerializeField]
		ListViewIcons lvi;

		public void ListClear()
		{
			lvi.DestroyGameObjects = true;
			lvi.DataSource.Clear();
		}

		void TestN(int n)
		{
			lv.DataSource = Enumerable.Range(1, n).Select(x => x.ToString("00000")).ToObservableList();
		}

		public void Test2()
		{
			TestN(2);
		}

		public void Test5()
		{
			TestN(5);
		}

		public void Test10()
		{
			TestN(10);
		}

		public void Test100()
		{
			TestN(100);
		}

		public void Test1000()
		{
			TestN(1000);
		}

		public void Test10000()
		{
			TestN(10000);
		}

		public void TestiN(int n)
		{
			var data = Enumerable.Range(1, n).Select(x => new ListViewIconsItemDescription() {
				Name = x.ToString("00000")
			}).ToObservableList();
			lvi.DataSource = data;
		}

		public void Testi2()
		{
			TestiN(2);
		}

		public void Testi5()
		{
			TestiN(5);
		}

		public void Testi1000()
		{
			lvi.SortFunc = null;
			TestiN(1000);
		}

		public void Testi10000()
		{
			lvi.SortFunc = null;
			TestiN(10000);
		}
	}
}