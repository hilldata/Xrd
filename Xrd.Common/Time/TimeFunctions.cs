using System;

namespace Xrd.Time {
	/// <summary>
	/// Functions and Extension methods for DateTime and TimeSpan calculations.
	/// </summary>
	public static class TimeFunctions {
		#region GetDay Methods
		/// <summary>
		/// Gets the next date that is the specified DayOfWeek
		/// </summary>
		/// <param name="date">The starting date</param>
		/// <param name="dayOfWeek">The day of the week to find</param>
		/// <param name="skipIfCurrent">Should it ignore the provided date if it is already the day of the week? Default = false, so that if looking for Sunday, and the date IS Sunday, it returns the original date.</param>
		/// <returns>The next date that is the provided DayOfWeek</returns>
		public static DateTime GetNextWeekDay(this DateTime date, DayOfWeek dayOfWeek, bool skipIfCurrent = false) {
			if (date.DayOfWeek == dayOfWeek && !skipIfCurrent)
				return date.Date;

			DateTime res = date.AddDays(1).Date;
			while (res.DayOfWeek != dayOfWeek)
				res = res.AddDays(1);
			return res;
		}

		/// <summary>
		/// Gets the previous date that is the specified DayOfWeek
		/// </summary>
		/// <param name="date">The starting date</param>
		/// <param name="dayOfWeek">The day of the week to find</param>
		/// <param name="skipIfCurrent">Should it ignore the provided date if it is already the day of the week? Default = false, so that if looking for Sunday, and the date IS Sunday, it returns the original date.</param>
		/// <returns>The previous date that is the provided DayOfWeek</returns>
		public static DateTime GetPreviousWeekDay(this DateTime date, DayOfWeek dayOfWeek, bool skipIfCurrent = false) {
			if (date.DayOfWeek == dayOfWeek && !skipIfCurrent)
				return date.Date;
			DateTime res = date.AddDays(-1).Date;
			while (res.DayOfWeek != dayOfWeek)
				res = res.AddDays(-1);
			return res;
		}

		/// <summary>
		/// Gets the WeekEndingDate (Saturday) for the specified day
		/// </summary>
		/// <param name="date">The date to check</param>
		/// <returns>The next Saturday following the date provided (or the date, if it is already a Saturday)</returns>
		public static DateTime GetWeekEndingDate(this DateTime date) => date.GetNextWeekDay(DayOfWeek.Saturday, false);

		/// <summary>
		/// Gets the WeekStartingDate (Sunday) for the specified day
		/// </summary>
		/// <param name="date">The date to check</param>
		/// <returns>The previous Sunday prior to the day provided (or the date, if it is already a Sunday)</returns>
		public static DateTime GetWeekStartingDate(this DateTime date) => date.GetPreviousWeekDay(DayOfWeek.Sunday, false);

		/// <summary>
		/// Get the date that is the "x" "day of the week" from the specified month and year.
		/// </summary>
		/// <param name="year">The year in question</param>
		/// <param name="month">The month in question</param>
		/// <param name="weekday">The day of the week to find.</param>
		/// <param name="count">The index of the weekday to find</param>
		/// <returns>The date (if exists) of the weekday to find.</returns>
		/// <remarks>
		/// May return null if looking for the 5th occurance of the weekday in a month.
		/// </remarks>
		public static DateTime? GetXWeekDay(int year, int month, DayOfWeek weekday, byte count) {
			DateTime dt = new DateTime(year, month, 1);
			int c = 0;
			if (dt.DayOfWeek == weekday) {
				if (count == 1)
					return dt;
				else
					c = 1;
			}
			for (int i = 0; i < 31; i++) {
				dt = dt.AddDays(1);
				if (dt.DayOfWeek == weekday) {
					c++;
					if (c == count)
						return dt;
				}
			}
			return null;
		}

		/// <summary>
		/// Get the date of the last "Day of the Week" in a given month/year.
		/// </summary>
		/// <param name="year">The year in question</param>
		/// <param name="month">The month in question</param>
		/// <param name="weekday">The weekday to find</param>
		/// <returns>The date of the specified weekday</returns>
		public static DateTime GetLastWeekDay(int year, int month, DayOfWeek weekday) {
			int lastday = 31;
			if (month == 2) {
				if (year % 4 == 0)
					lastday = 29;
				else
					lastday = 28;
			} else if (month == 4 || month == 6 || month == 9 || month == 11)
				lastday = 30;
			DateTime dt = new DateTime(year, month, lastday);
			if (dt.DayOfWeek == weekday)
				return dt;
			for (int i = 0; i < 7; i++) {
				dt = dt.AddDays(-1);
				if (dt.DayOfWeek == weekday)
					return dt;
			}
			return dt;
		}

		public static int[] MonthsWith31Days = new int[] { 1, 3, 5, 7, 8, 10, 12 };
		public static int[] MonthsWith30Days = new int[] { 4, 6, 9, 11 };
		#endregion

		#region Public Round Minutes Methods
		/// <summary>
		/// Round a DateTime value to the specified increment.
		/// </summary>
		/// <param name="dateTime">The DateTime value to round.</param>
		/// <param name="roundTo">The increment to round the minutes to.</param>
		/// <returns>The original DateTime value, but rounded to the specified increment.</returns>
		public static DateTime RoundMinutes(this DateTime dateTime, RoundingIncrements roundTo = RoundingIncrements.Tenth) {
			byte minutes = (byte)dateTime.Minute;
			switch (roundTo) {
				case RoundingIncrements.Quarter:
					minutes = minutes.roundTo15Minutes();
					break;
				case RoundingIncrements.Tenth:
					minutes = minutes.roundTo6Minutes();
					break;
				default:
					break;
			}
			return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, minutes, 0, dateTime.Kind);
		}

		/// <summary>
		/// Round a DateTime value to the specified increment.
		/// </summary>
		/// <param name="dateTime">The DateTime value to round.</param>
		/// <param name="roundTo">The increment to round the minutes to.</param>
		/// <returns>The original DateTime value, but rounded to the specified increment.</returns>
		public static DateTime? RoundMinutes(this DateTime? dateTime, RoundingIncrements roundTo = RoundingIncrements.Tenth) {
			if (!dateTime.HasValue)
				return null;
			return dateTime.Value.RoundMinutes(roundTo);
		}

		/// <summary>
		/// Round a TimeSpan to the specified increment.
		/// </summary>
		/// <param name="timeSpan">The TimeSpan value to round.</param>
		/// <param name="roundTo">The increment to round the minutes to.</param>
		/// <returns>The original TimeSpan value, but rounded to the specified increment.</returns>
		public static TimeSpan RoundMinutes(this TimeSpan timeSpan, RoundingIncrements roundTo = RoundingIncrements.Tenth) {
			byte minutes = (byte)timeSpan.Minutes;
			switch (roundTo) {
				case RoundingIncrements.Quarter:
					minutes = minutes.roundTo15Minutes();
					break;
				case RoundingIncrements.Tenth:
					minutes = minutes.roundTo6Minutes();
					break;
				default:
					break;
			}
			return new TimeSpan(timeSpan.Days, timeSpan.Hours, minutes, 0);
		}

		/// <summary>
		/// Round a TimeSpan to the specified increment.
		/// </summary>
		/// <param name="timeSpan">The TimeSpan value to round.</param>
		/// <param name="roundTo">The increment to round the minutes to.</param>
		/// <returns>The original TimeSpan value, but rounded to the specified increment.</returns>
		public static TimeSpan? RoundMinutes(this TimeSpan? timeSpan, RoundingIncrements roundTo = RoundingIncrements.Tenth) {
			if (!timeSpan.HasValue)
				return null;
			return timeSpan.Value.RoundMinutes(roundTo);
		}

		/// <summary>
		/// Round a decimal hours value to the specified increment.
		/// </summary>
		/// <param name="hours">The fractional hours value to round.</param>
		/// <param name="roundTo">The increment to round the fraction to.</param>
		/// <returns>The original fractional hours value, but rounded to the specified increment.</returns>
		public static decimal RoundFractionalHour(this decimal hours, RoundingIncrements roundTo = RoundingIncrements.Tenth) {
			int hr = (int)hours;
			byte minutes = hours.validMinutes();
			switch (roundTo) {
				case RoundingIncrements.Quarter:
					minutes = minutes.roundTo15Minutes();
					break;
				case RoundingIncrements.Tenth:
					minutes = minutes.roundTo6Minutes();
					break;
				default:
					break;
			}
			return hours + ((decimal)minutes / 60);
		}

		/// <summary>
		/// Round a decimal hours value to the specified increment.
		/// </summary>
		/// <param name="hours">The fractional hours value to round.</param>
		/// <param name="roundTo">The increment to round the fraction to.</param>
		/// <returns>The original fractional hours value, but rounded to the specified increment.</returns>
		public static decimal? RoundFractionalHour(this decimal? hours, RoundingIncrements roundTo = RoundingIncrements.Tenth) {
			if (!hours.HasValue)
				return null;
			return hours.Value.RoundFractionalHour();
		}
		#endregion

		/// <summary>
		/// Convert a fractional hours value to a TimeSpan
		/// </summary>
		/// <param name="hoursAsDecimal">A fractional number of hours represented as a decimal.</param>
		/// <returns>A TimeSpan? value</returns>
		public static TimeSpan? HoursToTimeSpan(this decimal? hoursAsDecimal) {
			if (!hoursAsDecimal.HasValue)
				return null;

			return hoursAsDecimal.Value.HoursToTimeSpan();
		}

		/// <summary>
		/// Convert a fractional hours value to a TimeSpan
		/// </summary>
		/// <param name="hoursAsDecimal">A fractional number of hours</param>
		/// <returns></returns>
		public static TimeSpan? HoursToTimeSpan(this decimal hoursAsDecimal) {
			return new TimeSpan((int)hoursAsDecimal, hoursAsDecimal.validMinutes(), 0);
		}

		public static decimal ToDecimalHours(this TimeSpan timeSpan, RoundingIncrements roundTo = RoundingIncrements.Tenth) {
			TimeSpan temp = timeSpan.RoundMinutes(roundTo);
			return temp.Days * 24 +
				temp.Hours +
				(decimal)temp.Minutes / 60;
		}

		public static decimal? ToDecimalHours(this TimeSpan? timeSpan, RoundingIncrements roundTo = RoundingIncrements.Tenth) {
			if (!timeSpan.HasValue)
				return null;
			return timeSpan.Value.ToDecimalHours();
		}

		#region Trim minutes to < 60 Methods
		private static byte validMinutes(this byte minutes) {
			if (minutes >= 60)
				return (byte)(minutes % 60);
			return minutes;
		}
		private static byte validMinutes(this short minutes) {
			if (Math.Abs(minutes) >= 60)
				return (byte)(minutes % 60);
			return (byte)minutes;
		}
		private static byte validMinutes(this int minutes) {
			if (Math.Abs(minutes) >= 60)
				return (byte)(minutes % 60);
			return (byte)minutes;
		}
		private static byte validMinutes(this long minutes) {
			if (Math.Abs(minutes) >= 60)
				return (byte)(minutes % 60);
			return (byte)minutes;
		}
		private static byte validMinutes(this float minutes) {
			if (Math.Abs(minutes) >= 60)
				return (byte)(minutes % 60);
			return (byte)minutes;
		}
		private static byte validMinutes(this double minutes) {
			if (Math.Abs(minutes) >= 60)
				return (byte)(minutes % 60);
			return (byte)minutes;
		}
		private static byte validMinutes(this decimal hours) {
			decimal fraction = hours - (byte)(hours);
			return (byte)(fraction * 60);
		}
		#endregion

		#region Round to 15 Minutes Methods
		private static byte roundTo15Minutes(this byte minutes) {
			for (byte b = 0; b <= 4; b++) {
				if (minutes.validMinutes() <= (b * 15 + 7))
					return (byte)(b * 15);
			}
			return 60;
		}
		private static byte roundTo15MinuteS(this short minutes) {
			for (byte b = 0; b <= 4; b++) {
				if (minutes.validMinutes() <= (b * 15 + 7))
					return (byte)(b * 15);
			}
			return 60;
		}
		private static byte roundTo15Minutes(this int minutes) {
			for (byte b = 0; b <= 4; b++) {
				if (minutes.validMinutes() <= (b * 15 + 7))
					return (byte)(b * 15);
			}
			return 60;
		}
		private static byte roundTo15Minutes(this long minutes) {
			for (byte b = 0; b <= 4; b++) {
				if (minutes.validMinutes() <= (b * 15 + 7))
					return (byte)(b * 15);
			}
			return 60;
		}
		private static byte roundTo15Minutes(this float minutes) {
			for (byte b = 0; b <= 4; b++) {
				if (minutes.validMinutes() < (b * 15 + 7.5))
					return (byte)(b * 15);
			}
			return 60;
		}
		private static byte roundTo15Minutes(this double minutes) {
			for (byte b = 0; b <= 4; b++) {
				if (minutes.validMinutes() < (b * 15 + 7.5))
					return (byte)(b * 15);
			}
			return 60;
		}
		private static byte roundTo15Minutes(this decimal hours) {
			for (byte b = 0; b <= 4; b++) {
				if (hours.validMinutes() < (b * 15 + 7.5))
					return (byte)(b * 15);
			}
			return 60;
		}
		#endregion

		#region Round to 6 Minutes Methods
		private static byte roundTo6Minutes(this byte minutes) {
			for (byte b = 0; b <= 10; b++) {
				if (minutes.validMinutes() <= (b * 6 + 3))
					return (byte)(b * 6);
			}
			return 60;
		}
		private static byte roundTo6MinuteS(this short minutes) {
			for (byte b = 0; b <= 10; b++) {
				if (minutes.validMinutes() <= (b * 6 + 3))
					return (byte)(b * 6);
			}
			return 60;
		}
		private static byte roundTo6Minutes(this int minutes) {
			for (byte b = 0; b <= 10; b++) {
				if (minutes.validMinutes() <= (b * 6 + 3))
					return (byte)(b * 6);
			}
			return 60;
		}
		private static byte roundTo6Minutes(this long minutes) {
			for (byte b = 0; b <= 10; b++) {
				if (minutes.validMinutes() <= (b * 6 + 3))
					return (byte)(b * 6);
			}
			return 60;
		}
		private static byte roundTo6Minutes(this float minutes) {
			for (byte b = 0; b <= 10; b++) {
				if (minutes.validMinutes() < (b * 6 + 3))
					return (byte)(b * 6);
			}
			return 60;
		}
		private static byte roundTo6Minutes(this double minutes) {
			for (byte b = 0; b <= 10; b++) {
				if (minutes.validMinutes() < (b * 6 + 3))
					return (byte)(b * 6);
			}
			return 60;
		}
		private static byte roundTo6Minutes(this decimal hours) {
			for (byte b = 0; b <= 10; b++) {
				if (hours.validMinutes() < (b * 6 + 3))
					return (byte)(b * 6);
			}
			return 60;
		}
		#endregion

		/// <summary>
		/// Ensure that the provided DateTime value is a UTC value
		/// </summary>
		/// <param name="original">The DateTime value</param>
		/// <returns>The DateTime value as a UTC-Kind value</returns>
		public static DateTime ToUtc(this DateTime original) {
			if (original.Kind == DateTimeKind.Utc)
				return original;
			else {
				DateTime temp = new DateTime(original.Ticks, DateTimeKind.Local);
				return temp.ToUniversalTime();
			}
		}

		/// <summary>
		/// Ensure that the provided DateTime value is a UTC value
		/// </summary>
		/// <param name="original">The DateTime value</param>
		/// <returns>The DateTime value as a UTC-Kind value</returns>
		public static DateTime? ToUtc(this DateTime? original) {
			if (!original.HasValue)
				return null;
			if (original.Value.Kind == DateTimeKind.Utc)
				return original;
			else {
				DateTime temp = new DateTime(original.Value.Ticks, DateTimeKind.Local);
				return temp.ToUniversalTime();
			}
		}

		public const decimal MONTHS_IN_YEAR = 12;
		public const decimal WEEKS_IN_YEAR = 52;
		public const decimal DAYS_IN_YEAR = 365.25m;
		public const int DAYS_IN_YEAR_INT = 365;
		public const decimal HOURS_IN_YEAR = 8766;
		public const int HOURS_IN_YEAR_INT = 8760;
		public const int HOURS_IN_LEAPYEAR_INT = 8784;
		public const decimal MINUTES_IN_YEAR = 525600;
		public const decimal MINUTES_IN_LEAPYEAR = 527040;
		public const decimal SECONDS_IN_YEAR = 31536000;
		public const decimal SECONDS_IN_LEAPYEAR = 31622400;

		public const decimal DAYS_IN_MONTH = 30.4167m;
		public const decimal DAYS_IN_WEEK = 7;
		public const decimal HOURS_IN_WEEK = 168;
		public const decimal MINUTES_IN_WEEK = 10080;
		public const decimal SECONDS_IN_WEEK = 604800;

		public const decimal HOURS_IN_DAY = 24;
		public const decimal MINUTES_IN_DAY = 1440;
		public const decimal SECONDS_IN_DAY = 86400;

		public const decimal MINUTES_IN_HOUR = 60;
		public const decimal SECONDS_IN_HOUR = 3600;
		public const decimal SECONDS_IN_MINUTE = 60;

		/// <summary>
		/// The base-datetime value to use when converting <see cref="DateTime"/> values to/from <see cref="UInt32"/>
		/// </summary>
		public static DateTime CURRENT_EPOCH = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Convert a <see cref="DateTime?"/> value to a <see cref="uint"/> (null values become zero)
		/// </summary>
		/// <param name="dt">The DateTime value to convert</param>
		/// <param name="offset">The base starting point for the conversion. If NULL, the <see cref="CURRENT_EPOCH"/> constant is used.</param>
		/// <returns>The input represented by a uint</returns>
		public static uint ToUInt(this DateTime? dt, DateTime? offset = null) {
			if (!dt.HasValue)
				return 0;
			if (offset == null)
				offset = CURRENT_EPOCH;
			TimeSpan ts = dt.Value - offset.Value;
			ushort days = (ushort)ts.TotalDays;
			ushort min = (ushort)(ts.Hours * 60 + ts.Minutes);
			byte sec = (byte)(ts.Seconds / 4);

			byte[] vs = new byte[4];
			byte[] dArr = BitConverter.GetBytes(days);
			Array.Reverse(dArr);
			byte[] mArr = BitConverter.GetBytes(min);
			Array.Copy(dArr, vs, 2);
			TwoNybbles nybbles = new TwoNybbles(mArr[1], sec);
			mArr[1] = nybbles.Full;
			Array.Copy(mArr, 0, vs, 2, 2);
			Array.Reverse(vs);
			return BitConverter.ToUInt32(vs, 0);
		}

		/// <summary>
		/// Convert a <see cref="uint"/> back to a <see cref="DateTime"/> value. Zero input is returned as a NULL value.
		/// </summary>
		/// <param name="value">The value from the <see cref="ToUInt(DateTime, DateTime?)"/> conversion.</param>
		/// <param name="offset">The base starting point for the conversion. If NULL, the <see cref="CURRENT_EPOCH"/> constant is used.</param>
		/// <returns>The original DateTime value</returns>
		public static DateTime? FromUInt(this uint value, DateTime? offset = null) {
			if (value == 0)
				return null;
			if (offset == null)
				offset = CURRENT_EPOCH;

			byte[] vs = BitConverter.GetBytes(value);
			Array.Reverse(vs);
			byte[] dArr = new byte[2];
			byte[] mArr = new byte[2];

			Array.Copy(vs, dArr, 2);
			Array.Reverse(dArr);
			Array.Copy(vs, 2, mArr, 0, 2);
			TwoNybbles nybbles = new TwoNybbles(mArr[1]);
			mArr[1] = nybbles.High;

			ushort day = BitConverter.ToUInt16(dArr, 0);
			ushort min = BitConverter.ToUInt16(mArr, 0);
			return offset.Value.AddDays(day).AddMinutes(min).AddSeconds(nybbles.Low * 4);
		}
	}
}