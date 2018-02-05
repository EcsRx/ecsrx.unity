using UIWidgets;
using UnityEngine;
using System.Linq;
using System;

namespace UIWidgetsSamples {
	/// <summary>
	/// ChatView.
	/// </summary>
	public class ChatView : ListViewCustomHeight<ChatLineComponent,ChatLine> {
		/// <summary>
		/// Sets component data with specified item.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <param name="item">Item.</param>
		protected override void SetData(ChatLineComponent component, ChatLine item)
		{
			component.SetData(item);
		}

		// leave coloring functions empty
		protected override void HighlightColoring(ChatLineComponent component)
		{
		}

		protected override void SelectColoring(ChatLineComponent component)
		{
		}
		
		protected override void DefaultColoring(ChatLineComponent component)
		{
		}

		#region DataSource wrapper and Filter
		ObservableList<ChatLine> fullDataSource;

		public ObservableList<ChatLine> FullDataSource {
			get {
				return fullDataSource;
			}
			set {
				if (fullDataSource!=null)
				{
					// unsubscribe update event
					fullDataSource.OnChange -= UpdateDataSource;
				}
				fullDataSource = value;
				if (fullDataSource!=null)
				{
					// subscribe update event
					fullDataSource.OnChange += UpdateDataSource;
				}
				UpdateDataSource();
			}
		}

		Func<ChatLine,bool> filter;

		public Func<ChatLine, bool> Filter {
			get {
				return filter;
			}
			set {
				filter = value;
				UpdateDataSource();
			}
		}

		void UpdateDataSource()
		{
			DataSource.BeginUpdate();
			DataSource.Clear();
			if (filter!=null)
			{
				DataSource.AddRange(FullDataSource.Where(Filter));
			}
			else
			{
				DataSource.AddRange(FullDataSource);
			}
			DataSource.EndUpdate();
		}

		bool isStarted;

		public override void Start()
		{
			if (isStarted)
			{
				return ;
			}

			isStarted = true;

			base.Start();

			if (fullDataSource==null)
			{
				fullDataSource = new ObservableList<ChatLine>();
				fullDataSource.AddRange(DataSource);
				fullDataSource.OnChange += UpdateDataSource;

				UpdateDataSource();
			}
		}
		#endregion

		[SerializeField]
		ChatView SomeChatView;

		public void UsingListView()
		{
			SomeChatView.FullDataSource.Add(new ChatLine() {
				UserName = "some name",
				Message = "message",
				Time = DateTime.Now,
				Type = ChatLineType.User
			});

			// display only items with UserName=="some name"
			SomeChatView.Filter = x => x.UserName=="some name";
		}
	}
}
