using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrd.Time;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xrd.Time.Tests {
	[TestClass()]
	public class TimeFunctionsTests {
		// Thursday, 16 April 2020 10:17:20 AM
		private static DateTime TEST_DATE = new DateTime(2020, 04, 16, 10, 17, 20);

		[TestMethod()]
		public void GetNextWeekDayTest() {
			var thursday = new DateTime(2020, 04, 16);
			Assert.AreEqual(thursday, TEST_DATE.GetNextWeekDay(DayOfWeek.Thursday));
			Assert.AreEqual(thursday.AddDays(7), TEST_DATE.GetNextWeekDay(DayOfWeek.Thursday, true));
			Assert.AreEqual(thursday.AddDays(1), TEST_DATE.GetNextWeekDay(DayOfWeek.Friday));
		}

		[TestMethod()]
		public void GetPreviousWeekDayTest() {
			var thursday = new DateTime(2020, 04, 16);
			Assert.AreEqual(thursday.AddDays(-1), TEST_DATE.GetPreviousWeekDay(DayOfWeek.Wednesday));
			Assert.AreEqual(thursday, TEST_DATE.GetPreviousWeekDay(DayOfWeek.Thursday, false));
			Assert.AreEqual(thursday.AddDays(-7), TEST_DATE.GetPreviousWeekDay(DayOfWeek.Thursday, true));
		}

		[TestMethod()]
		public void GetWeekEndingDateTest() {
			var sat = new DateTime(2020, 04, 18);
			Assert.AreEqual(sat, TEST_DATE.GetWeekEndingDate());
		}

		[TestMethod()]
		public void GetWeekStartingDateTest() {
			var sun = new DateTime(2020, 04, 12);
			Assert.AreEqual(sun, TEST_DATE.GetWeekStartingDate());
		}

		[TestMethod()]
		public void GetXWeekDayTest() {
			var res = TimeFunctions.GetXWeekDay(2020, 4, DayOfWeek.Thursday, 3);
			Assert.AreEqual(res, TEST_DATE.Date);
			var test = TimeFunctions.GetXWeekDay(2020, 4, DayOfWeek.Friday, 5);
			Assert.IsNull(TimeFunctions.GetXWeekDay(2020, 4, DayOfWeek.Friday, 5));
		}

		[TestMethod()]
		public void GetLastWeekDayTest() {
			var res = TimeFunctions.GetLastWeekDay(2020, 4, DayOfWeek.Friday);
			Assert.AreEqual(new DateTime(2020, 04, 24), res);
			res = TimeFunctions.GetLastWeekDay(2020, 4, DayOfWeek.Thursday);
			Assert.AreEqual(new DateTime(2020, 04, 30), res);
		}

		[TestMethod()]
		public void RoundMinutesTest() {
			DateTime quarter = new DateTime(2020, 04, 16, 10, 15, 0);
			DateTime tenth = new DateTime(2020, 04, 16, 10, 18, 0);
			Assert.AreEqual(quarter, TEST_DATE.RoundMinutes(RoundingIncrements.Quarter));
			Assert.AreEqual(tenth, TEST_DATE.RoundMinutes());
		}

		[TestMethod()]
		public void RoundMinutesTest1() {
			DateTime? dt = null;
			Assert.IsNull(dt.RoundMinutes());
			DateTime rounded = new DateTime(2020, 04, 16, 10, 18, 0);
			dt = TEST_DATE;
			Assert.AreEqual(rounded, dt.RoundMinutes());
		}

		[TestMethod()]
		public void RoundMinutesTest2() {
			TimeSpan ts = new TimeSpan(10, 17, 20);
			Assert.AreEqual(new TimeSpan(10, 15, 0), ts.RoundMinutes(RoundingIncrements.Quarter));
			Assert.AreEqual(new TimeSpan(10, 18, 0), ts.RoundMinutes());
		}

		[TestMethod()]
		public void RoundMinutesTest3() { 
			TimeSpan? ts = null;
			Assert.IsNull(ts.RoundMinutes());
			ts = new TimeSpan(10, 17, 20);
			Assert.AreEqual(new TimeSpan(10, 15, 0), ts.RoundMinutes(RoundingIncrements.Quarter));
			Assert.AreEqual(new TimeSpan(10, 18, 0), ts.RoundMinutes());
		}

		[TestMethod()]
		public void RoundFractionalHourTest() {
			decimal hrs = 10.71m;
			Assert.AreEqual(10.75m, hrs.RoundFractionalHour(RoundingIncrements.Quarter));
			Assert.AreEqual(10.7m, hrs.RoundFractionalHour());
		}

		[TestMethod()]
		public void RoundFractionalHourTest1() {
			decimal? hrs = null;
			Assert.IsNull(hrs.RoundFractionalHour());
			hrs = 10.71m;
			Assert.AreEqual(10.75m, hrs.RoundFractionalHour(RoundingIncrements.Quarter));
			Assert.AreEqual(10.7m, hrs.RoundFractionalHour());
		}

		[TestMethod()]
		public void HoursToTimeSpanTest() {
			decimal? dHrs = null;
			Assert.IsNull(dHrs.HoursToTimeSpan());
			dHrs = 10.75m;
			Assert.AreEqual(new TimeSpan(10, 45, 0), dHrs.HoursToTimeSpan());
		}

		[TestMethod()]
		public void HoursToTimeSpanTest1() {
			decimal dHrs = 5.1m;
			Assert.AreEqual(new TimeSpan(5, 6, 0), dHrs.HoursToTimeSpan());
		}

		[TestMethod()]
		public void ToDecimalHoursTest() {
			TimeSpan ts = new TimeSpan(10, 17, 20);
			Assert.AreEqual(10.3m, ts.ToDecimalHours());
		}

		[TestMethod()]
		public void ToDecimalHoursTest1() {
			TimeSpan? ts = null;
			Assert.IsNull(ts.ToDecimalHours());
			ts = new TimeSpan(10, 17, 20);
			Assert.AreEqual(10.3m, ts.ToDecimalHours());
		}

		[TestMethod()]
		public void ToUtcTest() {
			Assert.AreNotEqual(DateTimeKind.Utc, TEST_DATE.Kind);
			DateTime utc = TEST_DATE.ToUtc();
			Assert.AreNotEqual(utc, TEST_DATE);
			Assert.AreEqual(DateTimeKind.Utc, utc.Kind);
		}

		[TestMethod()]
		public void ToUtcTest1() {
			DateTime? dt = null;
			Assert.IsNull(dt.ToUtc());
			dt = TEST_DATE;
			var utc = dt.ToUtc();
			Assert.IsNotNull(utc);
		}

		[TestMethod()]
		public void ToUIntTest() {
			uint zero = 0;
			DateTime? dt = null;
			uint offset = dt.ToUInt();
			Assert.AreEqual(zero, offset);
			offset = TEST_DATE.ToUInt();
			Assert.AreNotEqual(zero, offset);
			Assert.AreEqual(TEST_DATE, offset.FromUInt());
		}

		[TestMethod()]
		public void FromUIntTest() {
			DateTime offset = new DateTime(2020, 04, 01, 0, 0, 0, DateTimeKind.Utc);
			var ui = TEST_DATE.ToUInt(offset);
			var dt = ui.FromUInt(offset);
			Assert.AreEqual(TEST_DATE, dt);
		}
	}
}