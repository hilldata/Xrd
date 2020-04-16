using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrd.Time;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xrd.Time.Tests {
	[TestClass()]
	public class UsHolidaysTests {
		[TestMethod()]
		public void GetHolidayNameTest() {
			DateTime dt = new DateTime(2020, 04, 12);
			Assert.AreEqual(UsHolidays.Easter, dt.GetHolidayName());
		}

		[TestMethod()]
		public void GetHolidaysByYearTest() { 
			var res = UsHolidays.GetHolidaysByYear(2020);
			foreach(var h in res) {
				if (h.Name == UsHolidays.Easter)
					Assert.AreEqual(new DateTime(2020, 04, 12), h.Date);
			}
		}
	}
}