using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xrd.Collections;

namespace Xrd.Time {
	/// <summary>
	/// Helper class for US holidays
	/// </summary>
	public static class UsHolidays {
		public const string NewYearsDay = "New Year's Day";
		public const string MLK_Day = "Martin Luther King, Jr. Day";
		public const string GroundHogDay = "Groundhog Day";
		public const string ValentinesDay = "Valentine's Day";
		public const string AshWednesday = "Ash Wednesday";
		public const string PresidentsDay = "Presidents' Day";
		public const string StPatricksDay = "St. Patrick's Day";
		public const string PalmSunday = "Palm Sunday";
		public const string MaundyThursday = "Maundy Thursday";
		public const string GoodFriday = "Good Friday";
		public const string HolySaturday = "Holy Saturday";
		public const string Easter = "Easter";
		public const string EasterMonday = "Easter Monday";
		public const string TaxDay = "Tax Day";
		public const string AdminProfDay = "Administrative Professionals Day";
		public const string MothersDay = "Mother's Day";
		public const string PeaceOfficerMemDay = "Peace Officers Memorial Day";
		public const string MemorialDay = "Memorial Day";
		public const string FathersDay = "Father's Day";
		public const string Juneteenth = "Juneteenth";
		public const string IndependenceDay = "Independence Day";
		public const string LaborDay = "Labor Day";
		public const string ColumbusDay = "Columbus/Indigenous People's Day";
		public const string Halloween = "Halloween";
		public const string ElectionDay = "Election Day";
		public const string VeteransDay = "Veterans Day";
		public const string Thanksgiving = "Thanksgiving";
		public const string ThanksFriday = "Day after Thanksgiving";
		public const string XmasEve = "Christmas Eve";
		public const string XmasDay = "Christmas Day";
		public const string NewYearsEve = "New Year's Eve";

		/// <summary>
		/// Enumeration for whether or not to include Lent/Easter holidays and, if so, which calendar.
		/// </summary>
		public enum EasterOptions : byte {
			None = 0,
			EasternOthodox,
			WesternChristianity
		}

		/// <summary>
		/// Gets the easily calculable holiday name for the specified date.
		/// </summary>
		/// <param name="date">The date to check.</param>
		/// <returns>A standardized name for the holiday (if falls on set date/range of dates)</returns>
		public static string GetHolidayName(this DateTime date) {
			int year = date.Year;
			var qry = easter(year).Where(d => d.Date == date.Date);
			if (qry.Any())
				return qry.First().Name;

			qry = easter(year, false).Where(d => d.Date == date.Date);
			if (qry.Any())
				return qry.First().Name;

			var list = fixedHolidays(year);
			qry = list.Where(d => d.Date == date.Date);
			if (qry.Any())
				return qry.First().Name;

			qry = list.Where(d => d.Date >= date.Date.AddDays(-2) && d.Date <= date.Date.AddDays(2));
			if (qry.Any())
				return qry.First().Name + " (observed)";

			return null;
		}

		private static DateTime getXDay(int year, int month, DayOfWeek weekday, byte count) =>
			TimeFunctions.GetXWeekDay(year, month, weekday, count) ?? new DateTime(year, month, 1);
		private static DateTime getLastDay(int year, int month, DayOfWeek weekday) =>
			TimeFunctions.GetLastWeekDay(year, month, weekday);
		private static DateTime getElectionDate(int year) {
			DateTime res = getXDay(year, 11, DayOfWeek.Tuesday, 1);
			if (res.Day == 1)
				return res.AddDays(1);
			return res;
		}
		private static Holiday[] fixedHolidays(int year) => new Holiday[] {
			new Holiday(year, 1, 1, NewYearsDay),
			new Holiday(getXDay(year, 1, DayOfWeek.Monday, 3), MLK_Day),
			new Holiday(year, 2, 2, GroundHogDay),
			new Holiday(year, 2, 14, ValentinesDay),
			new Holiday(getXDay(year, 2, DayOfWeek.Monday, 3), PresidentsDay),
			new Holiday(year, 3, 17, StPatricksDay),
			new Holiday(taxDay(year), TaxDay),
			new Holiday(adminProfDay(year), AdminProfDay),
			new Holiday(getXDay(year, 5, DayOfWeek.Sunday, 2), MothersDay),
			new Holiday(year, 5, 15, PeaceOfficerMemDay),
			new Holiday(getLastDay(year, 5, DayOfWeek.Monday), MemorialDay),
			new Holiday(getXDay(year, 6, DayOfWeek.Sunday, 3), FathersDay),
			new Holiday(year, 6, 19, Juneteenth),
			new Holiday(year, 7, 4, IndependenceDay),
			new Holiday(getXDay(year, 9, DayOfWeek.Monday, 1), LaborDay),
			new Holiday(getXDay(year, 10, DayOfWeek.Monday, 2), ColumbusDay),
			new Holiday(year, 10, 31, Halloween),
			new Holiday(getElectionDate(year), ElectionDay),
			new Holiday(year, 11, 11, VeteransDay),
			new Holiday(getXDay(year, 11, DayOfWeek.Thursday, 4), Thanksgiving),
			new Holiday(getXDay(year, 11, DayOfWeek.Thursday, 4).AddDays(1), ThanksFriday),
			new Holiday(year, 12, 24, XmasEve),
			new Holiday(year, 12, 25, XmasDay),
			new Holiday(year, 12, 31, NewYearsEve)
		};

		private static DateTime taxDay(int year) {
			DateTime dt = new DateTime(year, 4, 15);
			while (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
				dt = dt.AddDays(1);
			return dt;
		}

		private static DateTime adminProfDay(int year) =>
			TimeFunctions.GetLastWeekDay(year, 4, DayOfWeek.Saturday).GetPreviousWeekDay(DayOfWeek.Wednesday);

		/// <summary>
		/// Calculate the dates of the major US holidays for the specified year.
		/// </summary>
		/// <param name="year">The year to calculate holidays for.</param>
		/// <param name="easterOptions">Options on which (if any) Lent/Easter holidays to include.</param>
		/// <returns>A list of major US holidays for the year.</returns>
		public static List<Holiday> GetHolidaysByYear(int year, EasterOptions easterOptions = EasterOptions.WesternChristianity) {
			List<Holiday> res = fixedHolidays(year).ToList();
			if (easterOptions == EasterOptions.WesternChristianity)
				res.AddRangeIfNotNull(easter(year, true));
			else if (easterOptions == EasterOptions.EasternOthodox)
				res.AddRangeIfNotNull(easter(year, false));

			return res.OrderBy(h => h.Date).ToList();
		}

		/// <summary>
		/// List of Western Easter dates from 1999 through 2039
		/// </summary>
		public static DateTime[] EasterDates_West = new DateTime[] {
			new DateTime(1999, 4, 4),
			new DateTime(2000, 4, 23),
			new DateTime(2001, 4, 15),
			new DateTime(2002, 3, 31),
			new DateTime(2003, 4, 20),
			new DateTime(2004, 4, 11),
			new DateTime(2005, 3, 27),
			new DateTime(2006, 4, 16),
			new DateTime(2007, 4, 8),
			new DateTime(2008, 3, 23),
			new DateTime(2009, 4, 12),
			new DateTime(2010, 4, 4),
			new DateTime(2011, 4, 24),
			new DateTime(2012, 4, 8),
			new DateTime(2013, 3, 31),
			new DateTime(2014, 4, 20),
			new DateTime(2015, 4, 5),
			new DateTime(2016, 3, 27),
			new DateTime(2017, 4, 16),
			new DateTime(2018, 4, 1),
			new DateTime(2019, 4, 21),
			new DateTime(2020, 4, 12),
			new DateTime(2021, 4, 4),
			new DateTime(2022, 4, 17),
			new DateTime(2023, 4, 9),
			new DateTime(2024, 3, 31),
			new DateTime(2025, 4, 20),
			new DateTime(2026, 4, 5),
			new DateTime(2027, 3, 28),
			new DateTime(2028, 4, 16),
			new DateTime(2029, 4, 1),
			new DateTime(2030, 4, 21),
			new DateTime(2031, 4, 14),
			new DateTime(2032, 3, 28),
			new DateTime(2033, 4, 17),
			new DateTime(2034, 4, 9),
			new DateTime(2035, 3, 25),
			new DateTime(2036, 4, 13),
			new DateTime(2037, 4, 5),
			new DateTime(2038, 4, 25),
			new DateTime(2039, 4, 10)
		};

		/// <summary>
		/// List of Eastern Orthodox Easter dates from 1999 through 2039
		/// </summary>
		public static DateTime[] EasterDates_East = new DateTime[] {
			new DateTime(1999, 4, 11),
			new DateTime(2000, 4, 30),
			new DateTime(2001, 4, 15),
			new DateTime(2002, 5, 5),
			new DateTime(2003, 4, 27),
			new DateTime(2004, 4, 11),
			new DateTime(2005, 5, 1),
			new DateTime(2006, 4, 23),
			new DateTime(2007, 4, 8),
			new DateTime(2008, 4, 27),
			new DateTime(2009, 4, 19),
			new DateTime(2010, 4, 4),
			new DateTime(2011, 4, 24),
			new DateTime(2012, 4, 15),
			new DateTime(2013, 5, 5),
			new DateTime(2014, 4, 20),
			new DateTime(2015, 4, 12),
			new DateTime(2016, 5, 1),
			new DateTime(2017, 4, 16),
			new DateTime(2018, 4, 8),
			new DateTime(2019, 4, 28),
			new DateTime(2020, 4, 19),
			new DateTime(2021, 5, 2),
			new DateTime(2022, 4, 24),
			new DateTime(2023, 4, 16),
			new DateTime(2024, 5, 5),
			new DateTime(2025, 4, 20),
			new DateTime(2026, 4, 12),
			new DateTime(2027, 5, 2),
			new DateTime(2028, 4, 16),
			new DateTime(2029, 4, 8),
			new DateTime(2030, 4, 28),
			new DateTime(2031, 4, 13),
			new DateTime(2032, 5, 2),
			new DateTime(2033, 4, 24),
			new DateTime(2034, 4, 9),
			new DateTime(2035, 4, 29),
			new DateTime(2036, 4, 20),
			new DateTime(2037, 4, 5),
			new DateTime(2038, 4, 25),
			new DateTime(2039, 4, 17)
		};

		private static Holiday[] easter(int year, bool western = true) {
			IEnumerable<DateTime> qry;
			if (western)
				qry = EasterDates_West.Where(e => e.Year == year);
			else
				qry = EasterDates_East.Where(e => e.Year == year);
			if (qry.Count() == 0)
				return null;
			DateTime dt = qry.First();
			return new Holiday[] {
				new Holiday(dt.AddDays(-46), AshWednesday),
				new Holiday(dt.AddDays(-7), PalmSunday),
				new Holiday(dt.AddDays(-3), MaundyThursday),
				new Holiday(dt.AddDays(-2), GoodFriday),
				new Holiday(dt.AddDays(-1), HolySaturday),
				new Holiday(dt, Easter),
				new Holiday(dt.AddDays(1), EasterMonday)
			};
		}
	}
}