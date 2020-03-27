using System;
using System.ComponentModel;

namespace Xrd.Names {
	/// <summary>
	/// Enumeration of the PersonalName properties.
	/// </summary>
	[Flags]
	public enum PersonNameProperties {
		/// <summary>
		/// The first or given name.
		/// </summary>
		[Description("First/Given Name")]
		First = 0x_0001,
		/// <summary>
		/// The middle name(s) or initial(s)
		/// </summary>
		[Description("Middle Name(s)/Initial(s)")]
		Middle = 0x_0010,
		/// <summary>
		/// The last or family name.
		/// </summary>
		[Description("Last/Family Name")]
		Last = 0x_0100,
		/// <summary>
		/// Honorific prefix.
		/// </summary>
		[Description("Honorific Prefix")]
		Prefix = 0x_1000,
		/// <summary>
		/// Generational or credential suffix.
		/// </summary>
		[Description("Honorific Suffix")]
		Suffix = 0x_0001_0000,
		/// <summary>
		/// Preferred nickname or alias.
		/// </summary>
		[Description("Nickname/Alias")]
		Nickname = 0x_0010_0000
	}
}