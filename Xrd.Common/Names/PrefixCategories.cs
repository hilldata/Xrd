using System;
using System.ComponentModel;

namespace Xrd.Names {
	/// <summary>
	/// Enumeration of the defined Prefix categories lists. (This is a Flag enumeration and can be combined using "|".)
	/// </summary>
	[Flags]
	[TypeConverter(typeof(EnumDisplayTypeConverter))]
	public enum PrefixCategories {
		/// <summary>
		/// Common personal prefixes
		/// </summary>
		[Description("Common personal prefixes")]
		Common = 0b_0000_0000,
		/// <summary>
		/// Formal honorific prefixes in the UK
		/// </summary>
		[Description("Formal honorific prefixes in the UK.")]
		Formal_UK = 0b_0000_0001,
		/// <summary>
		/// Formal honorific prefixes in the US
		/// </summary>
		[Description("Formal honorific prefixes in the US.")]
		Formal_US = 0b_0000_0010,
		/// <summary>
		/// Academic honorific prefixes
		/// </summary>
		[Description("Academic honorific prefixes.")]
		Academic = 0b_0000_0100,
		/// <summary>
		/// Professional honorific prefixes (US & Europe)
		/// </summary>
		[Description("Professional honorific prefixes (US & Europe).")]
		Professional = 0b_0000_1000,
		/// <summary>
		/// Roman Catholic & Orthodox Christian honorific prefixes
		/// </summary>
		[Description("Roman Catholic & Orthodox Christian honoric prefixes.")]
		CatholicOrthodox = 0b_0001_0000,
		/// <summary>
		/// Protestant Christian honorific prefixes.
		/// </summary>
		[Description("Protestant Christian honorific prefixes.")]
		Protestant = 0b_0010_0000,
		/// <summary>
		/// Judiac honorific prefixes.
		/// </summary>
		[Description("Judiac honorific prefixes.")]
		Judaic = 0b_0100_0000,
		/// <summary>
		/// Islamin honorific prefixes.
		/// </summary>
		[Description("Islamic honorific prefixes.")]
		Islamic = 0b_1000_0000
	}
}