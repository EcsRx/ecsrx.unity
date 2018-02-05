using UnityEngine;
using System.Collections;
using System;
using UIWidgets;

namespace UIWidgetsSamples {

	[Serializable]
	public class SteamSpyItem : IItemHeight
	{
		public float Height {
			get; set;
		}

		[SerializeField]
		public string Name;

		[SerializeField]
		public int ScoreRank;

		[SerializeField]
		public int Owners;

		[SerializeField]
		public int OwnersVariance;

		[SerializeField]
		public int Players;
		
		[SerializeField]
		public int PlayersVariance;

		[SerializeField]
		public int PlayersIn2Week;
		
		[SerializeField]
		public int PlayersIn2WeekVariance;

		[SerializeField]
		public int AverageTimeIn2Weeks;

		[SerializeField]
		public int MedianTimeIn2Weeks;
	}
}