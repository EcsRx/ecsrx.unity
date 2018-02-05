using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace UIWidgets {
	/// <summary>
	/// Works like ToogleGroup for Switches.
	/// A Switch Group is not a visible UI control but rather a way to modify the behavior of a set of Switches.
	/// Switches that belong to the same group are constrained so that only one of them can switched on at a time - pressing one of them to switch it on automatically switches the others off.
	/// </summary>
	[AddComponentMenu("UI/UIWidgets/SwitchGroup")]
	public class SwitchGroup : MonoBehaviour
	{
		/// <summary>
		/// Is it allowed that no switch is switched on?
		/// </summary>
		[SerializeField]
		public bool AllowSwitchOff;
		
		[System.NonSerialized]
		protected HashSet<Switch> Switches = new HashSet<Switch>();

		/// <summary>
		/// Check if switch is in group.
		/// </summary>
		/// <param name="currentSwitch">Current switch.</param>
		protected void ValidateSwitchIsInGroup(Switch currentSwitch)
		{
			if (currentSwitch==null || !Switches.Contains(currentSwitch))
			{
				throw new ArgumentException(string.Format("Switch {0} is not part of SwitchGroup {1}", currentSwitch, this));
			}
		}

		/// <summary>
		/// Notifies the switch on.
		/// </summary>
		/// <param name="currentSwitch">Current switch.</param>
		public void NotifySwitchOn(Switch currentSwitch)
		{
			ValidateSwitchIsInGroup(currentSwitch);
			
			foreach (var s in Switches)
			{
				if (s!=currentSwitch)
				{
					s.IsOn = false;
				}
			}
		}

		/// <summary>
		/// Unregisters the switch.
		/// </summary>
		/// <param name="currentSwitch">Current switch.</param>
		public void UnregisterSwitch(Switch currentSwitch)
		{
			Switches.Remove(currentSwitch);
		}

		/// <summary>
		/// Registers the switch.
		/// </summary>
		/// <param name="toggle">Toggle.</param>
		public void RegisterSwitch(Switch toggle)
		{
			Switches.Add(toggle);
		}

		/// <summary>
		/// Is any switch on?
		/// </summary>
		/// <returns><c>true</c>, if switches on was anyed, <c>false</c> otherwise.</returns>
		public bool AnySwitchesOn()
		{
			return Switches.FirstOrDefault(x => x.IsOn) != null;
		}

		/// <summary>
		/// Get active the switches.
		/// </summary>
		/// <returns>The switches.</returns>
		public IEnumerable<Switch> ActiveSwitches()
		{
			return Switches.Where(x => x.IsOn);
		}

		/// <summary>
		/// Sets all switches off.
		/// </summary>
		public void SetAllSwitchesOff()
		{
			bool oldAllowSwitchOff = AllowSwitchOff;
			AllowSwitchOff = true;
			
			foreach (var s in Switches)
			{
				s.IsOn = false;
			}
			
			AllowSwitchOff = oldAllowSwitchOff;
		}
	}
}