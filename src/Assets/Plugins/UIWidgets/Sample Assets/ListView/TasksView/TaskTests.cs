using UnityEngine;
using System.Collections;

namespace UIWidgetsSamples.Tasks {
	public class TaskTests : MonoBehaviour
	{
		public TaskView Tasks;

		public void AddTask()
		{
			var task = new Task(){Name = "Random Task", Progress = 0};

			Tasks.DataSource.Add(task);

			StartCoroutine(UpdateProgress(task, 1f, Random.Range(1, 10)));
		}

		IEnumerator UpdateProgress(Task task, float time, int delta)
		{
			while (task.Progress < 100)
			{
				yield return new WaitForSeconds(time);
				task.Progress = Mathf.Min(task.Progress + delta, 100);
			}
		}
	}
}