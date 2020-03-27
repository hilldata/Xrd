using System;

namespace Xrd.Time {
	/// <summary>
	/// Structure used to store an indeterminate length of time in a database and perform calculations where the unit may be different from one row to the next.
	/// </summary>
	/// <remarks>
	/// The data length for this structure's binary storage value is 5 bytes,
	/// versus 8 for a TimeSpan, which struct is also harder for end users to grasp.
	/// 
	/// This structure was originally developed for automating Collective 
	/// Bargaining Agreement timeframe calculations. As each unit generally
	/// had their own established timeframes and timelimits, the likelyhood
	/// of each contract using the same units approached zero.
	/// </remarks>
	public struct Duration {
		/// <summary>
		/// Create an instance of a Duration with the specified value and unit
		/// </summary>
		/// <param name="value">The numerical value of the duration.</param>
		/// <param name="unit">The unit of the duration</param>
		public Duration(float value, DurationUnits unit) {
			Value = value;
			Unit = unit;
		}
		/// <summary>
		/// Create an instance of a Duration from a byte array.
		/// </summary>
		/// <param name="value"></param>
		public Duration(byte[] value) {
			if (value.IsNullOrEmpty() || value.Length != 5) {
				Value = 0;
				Unit = DurationUnits.NOT_SET;
			} else {
				Value = BitConverter.ToSingle(value, 0);
				Unit = (DurationUnits)value[4];
			}
		}

		#region Fields
		/// <summary>
		/// The <see cref="Duration"/>'s numeric value.
		/// </summary>
		public float Value { get; set; }
		/// <summary>
		/// The <see cref="Duration"/>'s unit
		/// </summary>
		public DurationUnits Unit { get; set; }

		/// <summary>
		/// Get/set the binary value.
		/// </summary>
		public byte[] BinaryValue {
			get {
				byte[] vs = new byte[5];
				Array.Copy(BitConverter.GetBytes(Value), vs, 4);
				vs[4] = (byte)Unit;
				return vs;
			}
			set {
				if (value.IsNullOrEmpty() || value.Length != 5) {
					Value = 0;
					Unit = DurationUnits.NOT_SET;
				} else {
					Value = BitConverter.ToSingle(value, 0);
					Unit = (DurationUnits)value[4];
				}
			}
		}
		#endregion

		#region Constants
		private const float HoursPerDay = 24f;
		private const float MinutesPerDay = 1440f;
		private const float MinutesPerHour = 60f;
		private const float SecondsPerMinute = 60f;
		private const float DaysPerMonth = 30.41667f;
		private const float DaysPerWeek = 7f;
		private const float DaysPerYear = 365.25f;

		/// <summary>
		/// The size of the byte array in the <see cref="BinaryValue"/> field.
		/// </summary>
		public const int SIZE = 5;
		#endregion

		public override string ToString() => (Unit == DurationUnits.NOT_SET || Value == 0) ? "(not set)" : $"{Value} {Unit}s";

		private static bool IsParseStringValid(string s) {
			if (string.IsNullOrWhiteSpace(s))
				return false;
			if (s.Contains("not set"))
				return false;
			return s.IndexOf(" ") > 0;
		}

		private static DurationUnits ParseUnit(string s) {
			if (string.IsNullOrWhiteSpace(s))
				return DurationUnits.NOT_SET;
			string t = s.Trim().ToLower();
			if (t.StartsWith("s"))
				return DurationUnits.Second;
			else if (t.StartsWith("mi"))
				return DurationUnits.Minute;
			else if (t.StartsWith("h"))
				return DurationUnits.Hour;
			else if (t.StartsWith("d"))
				return DurationUnits.Day;
			else if (t.StartsWith("w"))
				return DurationUnits.Week;
			else if (t.StartsWith("mo"))
				return DurationUnits.Month;
			else if (t.StartsWith("y"))
				return DurationUnits.Year;
			else
				return DurationUnits.NOT_SET;
		}

		/// <summary>
		/// Parse the specified string into a <see cref="Duration"/>
		/// </summary>
		/// <param name="s">The string representation to parse</param>
		/// <returns>A Duration represented by the string.</returns>
		public static Duration Parse(string s) {
			if (!IsParseStringValid(s))
				return new Duration(0, DurationUnits.NOT_SET);
			string[] split = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			DurationUnits unit = ParseUnit(split[1]);
			if (float.TryParse(split[0], out float f))
				return new Duration(f, unit);
			else
				throw new FormatException($"The value portion of the input string [{split[0]}] could not be parsed into a number.");
		}

		/// <summary>
		/// Attempt to parse the specified string into a Duration.
		/// </summary>
		/// <param name="s">The string representation to parse.</param>
		/// <param name="duration">The parsed duration</param>
		/// <returns>A boolean indicating whether or not the string was successfully parsed</returns>
		public static bool TryParse(string s, out Duration duration) {
			if (!IsParseStringValid(s)) {
				duration = new Duration(0, DurationUnits.NOT_SET);
				return false;
			}

			string[] split = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			DurationUnits unit = ParseUnit(split[1]);
			if (float.TryParse(split[0], out float f)) {
				duration = new Duration(f, unit);
				return true;
			}
			duration = new Duration(0, unit);
			return false;
		}

		/// <summary>
		/// Gets the duration as a TimeSpan for use in calculations.
		/// </summary>
		public TimeSpan? TimeSpan {
			get {
				if (Unit == DurationUnits.NOT_SET)
					return null;
				else if (Value < 0)
					return null;

				int[] vs = new int[5] { 0, 0, 0, 0, 0 };
				switch (Unit) {
					case DurationUnits.Day: {
							//Days
							vs[0] = (int)Math.Floor(Value);
							//Hours
							vs[1] = (int)Math.Floor((Value * HoursPerDay) - (int)(vs[0] * HoursPerDay));
							//Minutes
							vs[2] = (int)Math.Floor((Value * MinutesPerDay) - (int)(vs[0] * MinutesPerDay) - (int)(vs[1] * MinutesPerHour));
							break;
						}
					case DurationUnits.Hour: {
							//Hours
							vs[1] = (int)Math.Floor(Value);
							//Minutes
							vs[2] = (int)Math.Floor((Value - vs[1] * MinutesPerHour));
							break;
						}
					case DurationUnits.Minute: {
							//Minutes
							vs[2] = (int)Math.Floor(Value);
							//Seconds
							vs[3] = (int)Math.Floor(Value * SecondsPerMinute) - (int)Math.Floor(vs[2] * SecondsPerMinute);
							break;
						}
					case DurationUnits.Month: {
							//Days
							vs[0] = (int)Math.Floor(Value * DaysPerMonth);
							//Hours
							vs[1] = (int)Math.Floor((Value - (vs[0] / DaysPerMonth)) * HoursPerDay * DaysPerMonth);
							break;
						}
					case DurationUnits.Second: {
							//Seconds
							vs[3] = (int)Math.Floor(Value);
							//Milliseconds
							vs[4] = (int)Math.Floor((Value - vs[3]) * 1000);
							break;
						}
					case DurationUnits.Week: {
							//Days
							vs[0] = (int)Math.Floor(Value * DaysPerWeek);
							//Hours
							vs[1] = (int)Math.Floor(Value * HoursPerDay * DaysPerWeek) - (int)Math.Floor(vs[0] * HoursPerDay);
							break;
						}
					case DurationUnits.Year: {
							//Days
							vs[0] = (int)Math.Floor(Value * DaysPerYear);
							//Hours
							vs[0] = (int)Math.Floor(Value * DaysPerYear * HoursPerDay) - (int)Math.Floor(vs[0] * HoursPerDay);
							break;
						}
				}
				return new TimeSpan(vs[0], vs[1], vs[2], vs[3], vs[4]);
			}
		}
	}
}