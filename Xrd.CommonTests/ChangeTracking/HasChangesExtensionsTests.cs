using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrd.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xrd.ChangeTracking.Tests {
	[TestClass()]
	public class HasChangesExtensionsTests {
		[DataTestMethod]
		[DataRow(null, null, false)]
		[DataRow(null, 2, true)]
		[DataRow(2, 2, null)]
		[DataRow("someText", "someOther", null)]
		[DataRow("someText", null, true)]
		public void IsNullableDirtyTest1(object o, object c, bool? expected) =>
			Assert.AreEqual(expected, o.IsNullableDirty(c));

		[DataTestMethod()]
		[DataRow(1, 2, true)] // different lengths
		[DataRow(0, 0, false)]// same length, both zero
		[DataRow(1, 1, null)] // same length
		public void IsLengthDirtyTest(int o, int c, bool? expected) =>
			Assert.AreEqual(expected, o.IsLengthDirty(c));

		/// <summary>
		/// Used to time the Framework's String.Equals method
		/// </summary>
		[TestMethod]
		[Obsolete]
		public void IsStringDirtyTest() {
			string o = null;
			string c = null;
			Assert.AreEqual(false, o.IsStringDirty(c));
			o = "someText";
			Assert.AreEqual(true, o.IsStringDirty(c));
			c = "someText";
			Assert.AreEqual(false, o.IsStringDirty(c));
			c = "This is a much longer string of text.";
			Assert.AreEqual(true, o.IsStringDirty(c));
			c = "textSome";
			Assert.AreEqual(true, o.IsStringDirty(c));
		}

		/// <summary>
		/// Companion to previous method to test timing of HasChanges(string, string) method
		/// </summary>
		[TestMethod]
		public void StringHasChangesTest() {
			string o = null;
			string c = null;
			Assert.AreEqual(false, o.HasChanges(c));
			o = "someText";
			Assert.AreEqual(true, o.HasChanges(c));
			c = "someText";
			Assert.AreEqual(false, o.HasChanges(c));
			c = "This is a much longer string of text.";
			Assert.AreEqual(true, o.HasChanges(c));
			c = "textSome";
			Assert.AreEqual(true, o.HasChanges(c));
		}

		[TestMethod]
		public void HasChangesTest() {
			bool bO = true;
			bool bC = true;
			Assert.IsFalse(bO.HasChanges(bC));
			bC = false;
			Assert.IsTrue(bO.HasChanges(bC));
			bO = false; bC = true;
			Assert.IsTrue(bO.HasChanges(bC));

			bool? bnO = null;
			bool? bnC = null;
			Assert.IsFalse(bnO.HasChanges(bnC));
			bnO = true;
			Assert.IsTrue(bnO.HasChanges(bnC));
			bnC = true;
			Assert.IsFalse(bnO.HasChanges(bnC));
			bnC = false;
			Assert.IsTrue(bnO.HasChanges(bnC));

			short? snO = null, snC = null;
			Assert.IsFalse(snO.HasChanges(snC));
			snO = 0;
			Assert.IsTrue(snO.HasChanges(snC));
		}

		[DataTestMethod]
		[DataRow(true, true, false)]
		[DataRow(true, false, true)]
		[DataRow(false, true, true)]
		public void HasChangesTest_bool(bool o, bool c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow(null, null, false)]
		[DataRow(true, null, true)]
		[DataRow(true, true, false)]
		[DataRow(null, true, true)]
		[DataRow(null, false, true)]
		public void HasChangesTest_bool_(bool? o, bool? c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		public const int zero = 0;
		public const int one = 1;

		[DataTestMethod]
		[DataRow((byte)one, (byte)one, false)]
		[DataRow((byte)one, (byte)zero, true)]
		public void HasChangesTest_byte(byte o, byte c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow(null, null, false)]
		[DataRow(null, (byte)one, true)]
		[DataRow((byte)one, (byte)one, false)]
		[DataRow((byte)one, null, true)]
		[DataRow((byte)one, (byte)zero, true)]
		public void HasChangesTest_byte_(byte? o, byte? c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow((sbyte)one, (sbyte)one, false)]
		[DataRow((sbyte)one, (sbyte)zero, true)]
		public void HasChangesTest_sbyte(sbyte o, sbyte c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow(null, null, false)]
		[DataRow(null, (sbyte)one, true)]
		[DataRow((sbyte)one, (sbyte)one, false)]
		[DataRow((sbyte)one, (sbyte)zero, true)]
		public void HasChangesTest_sbyte_(sbyte? o, sbyte? c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow('a', 'a', false)]
		[DataRow('a', 'b', true)]
		[DataRow('a', 'A', true)]
		public void HasChangesTest_char(char o, char c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow(null, null, false)]
		[DataRow(null, 'a', true)]
		[DataRow('a', null, true)]
		[DataRow('a', 'a', false)]
		[DataRow('a', 'b', true)]
		[DataRow('a', 'A', true)]
		public void HasChangesTest_char_(char? o, char? c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow((short)zero, (short)zero, false)]
		[DataRow((short)zero, (short)one, true)]
		public void HasChangesTest_short(short o, short c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow(null, null, false)]
		[DataRow(null, (short)one, true)]
		[DataRow((short)one, (short)one, false)]
		[DataRow((short)one, (short)zero, true)]
		public void HasChangesTest_short_(short? o, short? c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow(zero, zero, false)]
		[DataRow(zero, one, true)]
		public void HasChangesTest_int(int o, int c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow(null, null, false)]
		[DataRow(null, one, true)]
		[DataRow(one, one, false)]
		[DataRow(one, zero, true)]
		public void HasChangesTest_int_(int? o, int? c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow((long)zero, (long)zero, false)]
		[DataRow((long)zero, (long)one, true)]
		public void HasChangesTest_long(long o, long c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow(null, null, false)]
		[DataRow(null, (long)one, true)]
		[DataRow((long)one, (long)one, false)]
		[DataRow((long)one, (long)zero, true)]
		public void HasChangesTest_long_(long? o, long? c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow((float)zero, (float)zero, false)]
		[DataRow((float)zero, (float)one, true)]
		public void HasChangesTest_float(float o, float c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow(null, null, false)]
		[DataRow(null, (float)one, true)]
		[DataRow((float)one, (float)one, false)]
		[DataRow((float)one, (float)zero, true)]
		public void HasChangesTest_float_(float? o, float? c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow((double)zero, (double)zero, false)]
		[DataRow((double)zero, (double)one, true)]
		public void HasChangesTest_double(double o, double c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[DataTestMethod]
		[DataRow(null, null, false)]
		[DataRow(null, (double)one, true)]
		[DataRow((double)one, (double)one, false)]
		[DataRow((double)one, (double)zero, true)]
		public void HasChangesTest_double_(double? o, double? c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[TestMethod]
		public void HasChangesTest_decimal() {
			decimal o = 0m, c = 0m;
			Assert.AreEqual(false, o.HasChanges(c));
			c = 1;
			Assert.AreEqual(true, o.HasChanges(c));
		}

		[TestMethod]
		public void HasChangesTest_decimal_() {
			decimal? o = null, c = null;
			Assert.AreEqual(false, o.HasChanges(c));
			c = 0m;
			Assert.AreEqual(true, o.HasChanges(c));
			o = 0m;
			Assert.AreEqual(false, o.HasChanges(c));
			c = null;
			Assert.AreEqual(true, o.HasChanges(c));
			c = 1;
			Assert.AreEqual(true, o.HasChanges(c));
		}

		[TestMethod]
		public void HasChangesTest_DateTime() {
			DateTime o = DateTime.MinValue, c = DateTime.MinValue;
			Assert.AreEqual(false, o.HasChanges(c));
			c = DateTime.Now;
			Assert.AreEqual(true, o.HasChanges(c));
			o = c.AddDays(10);
			Assert.AreEqual(true, o.HasChanges(c));
		}

		[TestMethod]
		public void HasChangesTest_DateTime_() {
			DateTime? o = null, c = null;
			Assert.AreEqual(false, o.HasChanges(c));
			c = DateTime.MinValue;
			Assert.AreEqual(true, o.HasChanges(c));
			o = DateTime.MinValue;
			Assert.AreEqual(false, o.HasChanges(c));
			c = null;
			Assert.AreEqual(true, o.HasChanges(c));
			c = o.Value.AddDays(10);
			Assert.AreEqual(true, o.HasChanges(c));
		}

		[TestMethod]
		public void HasChangesTest_TimeSpan() {
			TimeSpan o = TimeSpan.MinValue, c = TimeSpan.MinValue;
			Assert.AreEqual(false, o.HasChanges(c));
			o = DateTime.Now.TimeOfDay;
			c = o.Add(new TimeSpan(1, 10, 0));
			Assert.AreEqual(true, o.HasChanges(c));
		}

		[TestMethod]
		public void HasChangesTest_TimeSpan_() {
			TimeSpan? o = null, c = null;
			Assert.AreEqual(false, o.HasChanges(c));
			c = TimeSpan.MinValue;
			Assert.AreEqual(true, o.HasChanges(c));
			o = TimeSpan.MinValue;
			Assert.AreEqual(false, o.HasChanges(c));
			c = null;
			Assert.AreEqual(true, o.HasChanges(c));
			c = o.Value.Add(new TimeSpan(1, 10, 0));
			Assert.AreEqual(true, o.HasChanges(c));
		}

		[DataTestMethod]
		[DataRow(null, null, false)]
		[DataRow(null, "sometext", true)]
		[DataRow("sometext", "sometext", false)]
		[DataRow("someText", "sometext", true)]
		[DataRow("short", "Some bit of longer text.", true)]
		[DataRow("short", null, true)]
		public void HasChangesTest_String(string o, string c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[TestMethod]
		public void HasChangesTest_Guid() {
			Guid o = Guid.Empty, c = Guid.Empty;
			Assert.AreEqual(false, o.HasChanges(c));
			c = Guid.NewGuid();
			Assert.AreEqual(true, o.HasChanges(c));

			Guid? oN = null, cN = null;
			Assert.AreEqual(false, oN.HasChanges(cN));
			cN = Guid.Empty;
			Assert.AreEqual(true, oN.HasChanges(cN));
			oN = Guid.NewGuid();
			Assert.AreEqual(true, oN.HasChanges(cN));
			cN = null;
			Assert.AreEqual(true, oN.HasChanges(cN));
			cN = new Guid(oN.Value.ToByteArray());
			Assert.AreEqual(false, oN.HasChanges(cN));
		}

		[DataTestMethod]
		[DataRow(null, null, false)]
		[DataRow(null, new byte[] { 0, 0, 0, 0 }, true)]
		[DataRow(new byte[] { 0, 0 }, new byte[] { 0, 0, 0, 0 }, true)]
		[DataRow(new byte[] { 0, 0, 0, 0 }, new byte[] { 0, 0, 0, 0 }, false)]
		[DataRow(new byte[] { 0, 0, 0, 0 }, new byte[] { 1, 2, 3, 4 }, true)]
		public void HasChanges_Test_array_byte(byte[] o, byte[] c, bool expected) =>
			Assert.AreEqual(expected, o.HasChanges(c));

		[TestMethod]
		public void HasChangesTest_IList_byte() {
			IList<byte> o = null, c = null;
			Assert.AreEqual(false, o.HasChanges(c));
			o = new List<byte>();
			Assert.AreEqual(true, o.HasChanges(c));
			c = new List<byte>();
			Assert.AreEqual(false, o.HasChanges(c));
			o.Add(0);
			Assert.AreEqual(true, o.HasChanges(c));
			c.Add(0);
			Assert.AreEqual(false, o.HasChanges(c));
		}

		[TestMethod]
		public void HasChangesTest_Generic_ToString() {
			Xrd.Names.PersonName o = null, c = null;
			Assert.AreEqual(false, o.HasChanges(c));
			o = new Names.PersonName("John M. Hill");
			Assert.AreEqual(true, o.HasChanges(c));
			c = new Names.PersonName("John M. Hill");
			Assert.AreEqual(false, o.HasChanges(c));
			c.Middle = "Matthew";
			Assert.AreEqual(true, o.HasChanges(c));
		}

		[TestMethod]
		public void HasChangesTest_Generic_Json() {
			StateProvince o = null, c = null;
			Assert.AreEqual(false, o.HasChanges(c));
			o = StateProvince.NorthAmerican[0];
			Assert.AreEqual(true, o.HasChanges(c));
			c = StateProvince.NorthAmerican[0];
			Assert.AreEqual(false, o.HasChanges(c));
			c = StateProvince.NorthAmerican[1];
			Assert.AreEqual(true, o.HasChanges(c));
		}
	}
}