using System.ComponentModel;

namespace Xrd.Time {
	/// <summary>
	/// Enumeration of units a custom duration represents. Used to represent variable-magnitude lengths of time when the unit is not known at database-design time, or can change from record to record.
	/// </summary>
	[TypeConverter(typeof(EnumDisplayTypeConverter))]
	public enum DurationUnits : byte {
		/// <summary>
		/// Not set.
		/// </summary>
		[Description("(please select)")]
		NOT_SET = 0,
		/// <summary>
		/// Seconds.
		/// </summary>
		[Description("Second")]
		Second = 1,
		/// <summary>
		/// Minutes.
		/// </summary>
		[Description("Minute")]
		Minute = 2,
		/// <summary>
		/// Hours.
		/// </summary>
		[Description("Hour")]
		Hour = 3,
		/// <summary>
		/// Days.
		/// </summary>
		[Description("Day")]
		Day = 4,
		/// <summary>
		/// Weeks.
		/// </summary>
		[Description("Week")]
		Week = 5,
		/// <summary>
		/// Months.
		/// </summary>
		[Description("Month")]
		Month = 6,
		/// <summary>
		/// Years.
		/// </summary>
		[Description("Year")]
		Year = 7
	}
}