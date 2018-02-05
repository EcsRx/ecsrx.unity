using UIWidgets;

namespace UIWidgetsSamples.Tasks {
	
	public class TaskAutocomplete : AutocompleteCustom<Task,TaskComponent,TaskView> {
		// check if item starts with input string
		public override bool Startswith(Task value)
		{
			if (CaseSensitive)
			{
				return value.Name.StartsWith(Input);
			}
			return value.Name.ToLower().StartsWith(Input.ToLower());
		}
		
		// check if item contains input string
		public override bool Contains(Task value)
		{
			if (CaseSensitive)
			{
				return value.Name.Contains(Input);
			}
			return value.Name.ToLower().Contains(Input.ToLower());
		}
		
		// convert item to string to display in InputField
		protected override string GetStringValue(Task value)
		{
			return value.Name;
		}
	}
}