using System;
using System.ComponentModel;

namespace Xrd.Time {
	/// <summary>
	/// Enumeration of the days of the week (as flags, so they can be combined).
	/// </summary>
	[Flags]
	[TypeConverter(typeof(EnumDisplayTypeConverter))]
	public enum DaysOfTheWeek : byte {
		[Description("(not set)")]
		NotSet = 0,
		[Description("Sunday")]
		Sunday = 0b_0000_0001,
		[Description("Monday")]
		Monday = 0b_0000_0010,
		[Description("Tueday")]
		Tuesday = 0b_0000_0100,
		[Description("Wednesday")]
		Wednesday = 0b_0000_1000,
		[Description("Thursday")]
		Thursday = 0b_0001_0000,
		[Description("Friday")]
		Friday = 0b_0010_0000,
		[Description("Saturday")]
		Saturday = 0b_0100_0000
	}
}