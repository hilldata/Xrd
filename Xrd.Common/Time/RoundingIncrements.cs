using System.ComponentModel;

namespace Xrd.Time {
	/// <summary>
	/// Enumeration indicating how a time value is to be rounded.
	/// </summary>
	[TypeConverter(typeof(EnumDisplayTypeConverter))]
	public enum RoundingIncrements : byte {
		/// <summary>
		/// Round the value to the nearest quarter (15 minutes or 0.25).
		/// </summary>
		[Description("Quarter-Hour (15 min)")]
		Quarter = 0,
		/// <summary>
		/// Round the value to the nearest tenth (6 mintues or 0.1).
		/// </summary>
		[Description("Tenth-Hour (6 min)")]
		Tenth = 1,
		/// <summary>
		/// Round the value to the nearest half (30 minutes or 0.5).
		/// </summary>
		[Description("Half-Hour (30 min)")]
		Half = 2,
		/// <summary>
		/// Round the value to the nearest integral value (60 minutes or 1.0).
		/// </summary>
		[Description("Whole-Hour (60 min)")]
		Whole = 3
	}
}