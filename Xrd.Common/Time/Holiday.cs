using System;

namespace Xrd.Time {
	/// <summary>
	/// Structure for combining Holiday dates and their common name
	/// </summary>
	public struct Holiday {
		/// <summary>
		/// Create an instance from a specific date and name.
		/// </summary>
		/// <param name="date">A DateTime value of the holiday date.</param>
		/// <param name="name">The common name of the holiday</param>
		public Holiday(DateTime date, string name) {
			Date = date.Date;
			Name = name.Trim();
		}

		/// <summary>
		/// Create an instance from int values and name.
		/// </summary>
		/// <param name="year">The year portion.</param>
		/// <param name="month">The month portion.</param>
		/// <param name="day">The day of the month portion</param>
		/// <param name="name">The common name of the holiday.</param>
		public Holiday(int year, int month, int day, string name) {
			Date = new DateTime(year, month, day);
			Name = name.Trim();
		}

		#region Readonly Properties
		/// <summary>
		/// The year portion
		/// </summary>
		public int Year => Date.Year;
		/// <summary>
		/// The Month portion
		/// </summary>
		public int Month => Date.Month;
		/// <summary>
		/// The Day portion
		/// </summary>
		public int Day => Date.Day;
		/// <summary>
		/// The day of the week.
		/// </summary>
		public DayOfWeek WeekDay => Date.DayOfWeek;
		#endregion

		/// <summary>
		/// Get the date of the holiday
		/// </summary>
		public DateTime Date { get; private set; }
		/// <summary>
		/// Get the common name of the holiday.
		/// </summary>
		public string Name { get; private set; }
	}
}