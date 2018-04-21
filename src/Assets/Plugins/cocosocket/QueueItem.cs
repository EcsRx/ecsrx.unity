using UnityEngine;
using System.Collections;

namespace cocosocket4unity {
	public class QueueItem {
		public int CMD;
		public object Model;
		public string Msg;
		public QueueItem(int cmd, object model, string msg = "")
		{
			CMD = cmd;
			Model = model;
			Msg = msg;
		}
	}
}
