using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrd.Text;
using System;
using System.Collections.Generic;
using System.Text;
using Xrd.ChangeTracking;

namespace Xrd.Text.Tests {
	[TestClass()]
	public class TextParsingExtensionsTests {
		[DataTestMethod()]
		[DataRow("Key;Value", ';', 3)] // Straight find, no oddities.
		[DataRow("'Key;Value'", ';', 4)] // Ensure ignores single quotes.
		[DataRow("", ';', -1)] // Test empty input
		[DataRow("\"Key;Value\";SomethingElse", ';', 11)] // Check ignores inside quotes
		[DataRow("Key\\;Value;SomethingElse", ';', 10)] // Check ignores escaped value.
		[DataRow("Key;Value;SomethingElse", ';', 9, 4)] // Check startIndex provided.
		public void NonQuotedIndexOfTest(string input, char c, int expected, int startIndex = 0) =>
			Assert.AreEqual(expected, input.NonQuotedIndexOf(c, startIndex));

		[TestMethod]
		public void NonQuoteIndexOfTest2() {
			string input = $"{TextParsingExtensions.OPEN_DBL_QUOTE}Key;Value{TextParsingExtensions.CLOS_DBL_QUOTE};SomethingElse";
			Assert.AreEqual(11, input.NonQuotedIndexOf(';'));
		}

		[DataTestMethod()]
		[DataRow("Key;Value", ';', 3)] // Straight find, no oddities.
		[DataRow("'Key;Value'", ';', 4)] // Ensure ignores single quotes.
		[DataRow("", ';', -1)] // Test empty input
		[DataRow("\"Key;Value\";SomethingElse", ';', 4)] // Check does NOT ignore inside quotes
		[DataRow("Key\\;Value;SomethingElse", ';', 10)] // Check ignores escaped value.
		[DataRow("Key;Value;SomethingElse", ';', 9, 4)] // Check startIndex provided.
		public void NonEscapedIndexOfTest(string input, char c, int expected, int startIndex = 0) =>
			Assert.AreEqual(expected, input.NonEscapedIndexOf(c, startIndex));

		private const string RES_UNQUOTE = "test";
		[DataTestMethod()]
		[DataRow(null, null)]	// null input
		[DataRow("", null)] // empty string input
		[DataRow("  ", null)] // whitespace only input
		[DataRow("test", RES_UNQUOTE)] // no quotes
		[DataRow(" test", RES_UNQUOTE)]	// no quotes; leading whitespace
		[DataRow("\"test\"", RES_UNQUOTE)] // standard quotes on both ends.
		[DataRow("\" test \"", RES_UNQUOTE)] // quotes and lead/trailing whitespace
		[DataRow("\"test", RES_UNQUOTE)] // only opening quote
		[DataRow("test\"", RES_UNQUOTE)] // only trailing quote
		public void UnQuoteTest(string input, string expected) =>
			Assert.AreEqual(expected, input.UnQuote());

		// Testing unquote with the open/closing double-quote characters.
		[TestMethod()]
		public void UnQuoteTest2() {
			string input = TextParsingExtensions.OPEN_DBL_QUOTE + "test";
			Assert.AreEqual(RES_UNQUOTE, input.UnQuote());
			input += TextParsingExtensions.CLOS_DBL_QUOTE;
			Assert.AreEqual(RES_UNQUOTE, input.UnQuote());
			input = TextParsingExtensions.OPEN_DBL_QUOTE + "test\"";
			Assert.AreEqual(RES_UNQUOTE, input.UnQuote());
		}

		private static List<string> RES_SPLIT = new List<string>() {
			"Key",
			"Value"
		};

		[TestMethod()]
		public void NonQuotedSplitTest() {
			string input = null;
			Assert.ThrowsException<ArgumentNullException>(() => input.NonQuotedSplit(';'));
			input = "Key;Value";
			Assert.IsFalse(RES_SPLIT.HasChanges(input.NonQuotedSplit(';')));
			input = "Key;;Value";
			var res = input.NonQuotedSplit(';');
			Assert.IsTrue(RES_SPLIT.HasChanges(input.NonQuotedSplit(';')));
			Assert.IsFalse(RES_SPLIT.HasChanges(input.NonQuotedSplit(';', StringSplitOptions.RemoveEmptyEntries)));
			input = "Key;\"Value\"";
			Assert.IsTrue(RES_SPLIT.HasChanges(input.NonQuotedSplit(';', false)));
			Assert.IsFalse(RES_SPLIT.HasChanges(input.NonQuotedSplit(';', true)));
			var expected = new List<string>(RES_SPLIT) { "SomethingElse" };
			input += "\\;SomethingElse";
			Assert.AreEqual(2, input.NonQuotedSplit(';').Count);
			input = "Key;Value;SomethingElse";
			Assert.IsFalse(expected.HasChanges(input.NonQuotedSplit(';')));
		}

		private static List<string> RES_SPLITARRAY = new List<string>() {
			"Key",
			"Value",
			"Parameter"
		};

		[TestMethod()]
		public void NonQuotedSplitTest1() {
			char[] separators = new char[] { ';', ':' };
			string input = null;
			Assert.ThrowsException<ArgumentNullException>(() => input.NonQuotedSplit(separators));
			input = "Key;Value:Parameter";
			Assert.ThrowsException<ArgumentNullException>(() => input.NonQuotedSplit(null));
			Assert.IsFalse(RES_SPLITARRAY.HasChanges(input.NonQuotedSplit(separators)));
			input = "Key;;Value:Parameter";
			Assert.IsTrue(RES_SPLITARRAY.HasChanges(input.NonQuotedSplit(separators)));
			Assert.IsFalse(RES_SPLITARRAY.HasChanges(input.NonQuotedSplit(separators, StringSplitOptions.RemoveEmptyEntries)));
			input += ":SomethingElse";
			Assert.IsTrue(RES_SPLITARRAY.HasChanges(input.NonQuotedSplit(separators, StringSplitOptions.RemoveEmptyEntries)));
			input = "\"Key\";Value:Parameter";
			Assert.IsTrue(RES_SPLITARRAY.HasChanges(input.NonQuotedSplit(separators)));
			Assert.IsFalse(RES_SPLITARRAY.HasChanges(input.NonQuotedSplit(separators, true)));
		}

		[TestMethod()]
		public void NonQuotedSplitOnFirstTest() {
			Tuple<string, string> expected = new Tuple<string, string>("Key", "Value:Parameter");
			string input = null;
			Assert.AreEqual(null, input.NonQuotedSplitOnFirst(';'));
			input = string.Empty;
			Assert.AreEqual(null, input.NonQuotedSplitOnFirst(';'));
			input = "Key;Value:Parameter";
			Assert.IsFalse(expected.HasChanges(input.NonQuotedSplitOnFirst(';')));
			Assert.IsTrue(expected.HasChanges(input.NonQuotedSplitOnFirst(':')));
			expected = new Tuple<string, string>("Key;Value", "Parameter");
			Assert.IsFalse(expected.HasChanges(input.NonQuotedSplitOnFirst(':')));
			Assert.IsTrue(expected.HasChanges(input.NonQuotedSplitOnFirst(';')));
			expected = new Tuple<string, string>("Value", "Parameter");
			var split1 = input.NonQuotedSplitOnFirst(';');
			Assert.IsFalse(expected.HasChanges(split1.Item2.NonQuotedSplitOnFirst(':')));
		}

		[TestMethod()]
		public void EscapeQuotesTest() {
			string input = null;
			Assert.AreEqual(null, input.EscapeQuotes());
			input = " \r\n";
			Assert.AreEqual(null, input.EscapeQuotes());
			input = "\"Key\"";
			Assert.AreEqual("\\\"Key\\\"", input.EscapeQuotes());
		}

		[TestMethod()]
		public void UnEscapeQuotesTest() {
			string input = null;
			Assert.AreEqual(null, input.UnEscapeQuotes());
			input = " \r\n";
			Assert.AreEqual(null, input.UnEscapeQuotes());
			input = "\\\"Key\\\"";
			Assert.AreEqual("\"Key\"", input.UnEscapeQuotes());
		}

		[DataTestMethod()]
		[DataRow(null, null)]
		[DataRow("", null)]
		[DataRow("  ", null)]
		[DataRow("Key", "\"Key\"")]
		[DataRow("\"Key", "\"Key\"")]
		[DataRow("\"Key\"", "\"Key\"")]
		[DataRow("\"Key\\\"", "\"Key\\\"\"")]
		public void QuoteTextTest(string input, string expected) =>
			Assert.AreEqual(expected, input.QuoteText());
	}
}