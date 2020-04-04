using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrd.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xrd.Text.Tests {
	[TestClass()]
	public class IETFValueEncodingTests {
		[DataTestMethod()]
		[DataRow("test", "test")]
		[DataRow("test", "\"test\"", true)]
		[DataRow(@"\test", @"\\test")]
		[DataRow(@"\test", "\"\\\\test\"", true)]
		[DataRow("\"test\"", "\\\"test\\\"")]
		[DataRow("\"test\"", "\"\\\"test\\\"\"", true)]
		[DataRow("test, test", "test\\, test")]
		[DataRow("test, test", "\"test\\, test\"", true)]
		[DataRow("test; test", "test\\; test")]
		[DataRow("test; test", "\"test\\; test\"", true)]
		[DataRow("test\r\ntest", "test\\ntest")]
		[DataRow("test\r\ntest", "\"test\\ntest\"", true)]
		public void EncodeValueTest(string input, string expected, bool encloseInQuotes = false) =>
			Assert.AreEqual(expected, IETFValueEncoding.EncodeValue(input, encloseInQuotes));

		[DataTestMethod()]
		[DataRow("test", "test")]
		[DataRow("test", "\"test\"")]
		[DataRow(@"\test", @"\\test")]
		[DataRow(@"\test", "\"\\\\test\"")]
		[DataRow("\"test\"", "\\\"test\\\"")]
		[DataRow("\"test\"", "\"\\\"test\\\"\"")]
		[DataRow("test, test", "test\\, test")]
		[DataRow("test, test", "\"test\\, test\"")]
		[DataRow("test; test", "test\\; test")]
		[DataRow("test; test", "\"test\\; test\"")]
		[DataRow("test\r\ntest", "test\\ntest")]
		[DataRow("test\r\ntest", "\"test\\ntest\"")]
		public void DecodeValueTest(string expected, string input) =>
			Assert.AreEqual(expected, IETFValueEncoding.DecodeValue(input));

		[DataTestMethod()]
		[DataRow("test", "test")]
		[DataRow("\"test\"", "&quot;test&quot;")]
		[DataRow("test:test", "\"test:test\"")]
		[DataRow("test;test", "\"test;test\"")]
		[DataRow("test\\;test", "\"test\\;test\"")]
		[DataRow("test,test", "\"test,test\"")]
		[DataRow("test, \"something\"; test", "\"test, &quot;something&quot;; test\"")]
		public void EncodeParameterValueTest(string input, string expected) =>
			Assert.AreEqual(expected, IETFValueEncoding.EncodeParameterValue(input));

		[DataTestMethod()]
		[DataRow("test", "test")]
		[DataRow("\"test\"", "&quot;test&quot;")]
		[DataRow("test:test", "\"test:test\"")]
		[DataRow("test;test", "\"test;test\"")]
		[DataRow("test\\;test", "\"test\\;test\"")]
		[DataRow("test,test", "\"test,test\"")]
		[DataRow("test, \"something\"; test", "\"test, &quot;something&quot;; test\"")]
		public void DecodeParameterValueTest(string expected, string input) =>
			Assert.AreEqual(expected, IETFValueEncoding.DecodeParameterValue(input));

		[TestMethod()]
		public void ParseISO8601StringTest() {
			Assert.AreEqual(null, IETFValueEncoding.ParseISO8601String("asdfflkjasdf"));
			DateTime now = DateTime.Now;
			foreach(var f in IETFValueEncoding.ISO8601DT_FORMATS) {
				string s = now.ToString(f);
				Assert.IsTrue(IETFValueEncoding.ParseISO8601String(s).HasValue);
			}
		}

		[TestMethod()]
		public void ToISO8601StringTest() {
			Assert.IsTrue(string.IsNullOrEmpty(IETFValueEncoding.ToISO8601String(null)));
			Assert.IsFalse(string.IsNullOrEmpty(IETFValueEncoding.ToISO8601String(DateTime.Now)));

			DateTime dt = new DateTime(2020, 4, 4, 12, 30, 23);
			// This was accurate as of the time and location tested (the date above in the US Eastern Time Zone)
			string exp = "20200404T123023-04:00";
			Assert.AreEqual(exp, IETFValueEncoding.ToISO8601String(dt));
		}
	}
}