using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrd.ChangeTracking;
using Xrd.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xrd.Text.Tests {
	[TestClass()]
	public class TextExtensionsTests {
		[DataTestMethod()]
		[DataRow(null, false)]
		[DataRow("", false)]
		[DataRow(" ", false)]
		[DataRow("\t", false)]
		[DataRow("\r\n", false)]
		[DataRow("test", true)]
		public void HasValueTest(string input, bool expected) =>
			Assert.AreEqual(expected, input.HasValue());

		[TestMethod()]
		public void ToCharTest() {
			Assert.IsFalse(char.IsDigit(Mdl2AssetsEnum.Accept.ToChar()));
		}

		[TestMethod()]
		public void ConcatenateTest() {
			string[] vs = null;
			Assert.IsNull(vs.Concatenate());
			Assert.IsTrue(string.IsNullOrEmpty(vs.Concatenate()));

			vs = new string[0];
			Assert.IsNull(vs.Concatenate());
			Assert.IsTrue(string.IsNullOrEmpty(vs.Concatenate()));

			vs = new string[] { string.Empty };
			Assert.IsTrue(string.IsNullOrEmpty(vs.Concatenate()));

			vs[0] = "test";
			Assert.AreEqual("test", vs.Concatenate());

			vs = new string[] { "test1", "test2" };
			Assert.AreEqual("test1; test2", vs.Concatenate());
			Assert.AreEqual("test1/test2", vs.Concatenate("/"));

			List<string> vs1 = null;
			Assert.IsTrue(string.IsNullOrWhiteSpace(vs1.Concatenate()));
			vs1 = new List<string>();
			Assert.IsTrue(string.IsNullOrEmpty(vs1.Concatenate()));

			vs1.Add("test1");
			Assert.AreEqual("test1", vs1.Concatenate());

			vs1.AddRange(new string[] { "test2", "test3" });
			Assert.AreEqual("test1; test2; test3", vs1.Concatenate());
		}

		[DataTestMethod()]
		[DataRow(true, null)]
		[DataRow(false, null, true)]
		[DataRow(false, " ", true)]
		[DataRow(false, "t@t.")]
		[DataRow(true, "t@t.c")]
		[DataRow(false, "me.com")]
		[DataRow(false, "Me@.com")]
		[DataRow(false, "me@me.")]
		[DataRow(true, "me@me.com")]
		[DataRow(false, "me@me,com")]
		[DataRow(false, "me!me.com")]
		[DataRow(false, "me@me")]
		public void IsValidEmailTest(bool expected, string input, bool isRequired = false) =>
			Assert.AreEqual(expected, input.IsValidEmail(isRequired));

		[TestMethod()]
		public void Split_ValidateEmailAddressesTest() {
			string text = null;
			Assert.IsNull(text.Split_ValidateEmailAddresses());
			text = "me";
			Assert.AreEqual(0, text.Split_ValidateEmailAddresses().Count);

			text = "me@me.com";
			Assert.AreEqual(1, text.Split_ValidateEmailAddresses().Count);

			text += ",me@hotmail.com;;me@yahoo.com;me@.com";
			Assert.AreEqual(3, text.Split_ValidateEmailAddresses().Count);

		}

		[DataTestMethod()]
		[DataRow("", "")]
		[DataRow("  ", "")]
		[DataRow("test", "test")] // If there are no capital letters, there's no prefix, so the original result is returned.
		[DataRow("testTable", "Table")]
		[DataRow("tblTestTable", "Test Table")]
		[DataRow("tblTestTable_History", "Test Table History")]
		[DataRow("tblTest Table__History", "Test Table History")]
		[DataRow("SplitCamelCaseTest", "Split Camel Case Test")]
		public void SplitCamelCaseTest(string input, string expected) =>
			Assert.AreEqual(expected, input.SplitCamelCase());

		[DataTestMethod()]
		[DataRow("", false, null, "")]
		[DataRow("", false, new char[] { '_' }, "")]
		[DataRow("tblTest", false, new char[] { '_' }, "tbl Test")]
		[DataRow("tblTest", true, new char[] { '_' }, "Test")]
		[DataRow("tblTest-Test", false, new char[] { '_'}, "tbl Test- Test")]
		[DataRow("tblTest-Test", true, new char[] { '_' }, "Test- Test")]
		[DataRow("tblTest-Test", false, new char[] { '-' }, "tbl Test Test")]
		[DataRow("tblTest-Test", true, new char[] { '-' }, "Test Test")]
		[DataRow("tblTest--Test", true, new char[] { '-' }, "Test Test")]
		[DataRow("SplitCamelCaseTest1", true, null, "Split Camel Case Test1")]
		public void SplitCamelCaseTest1(string input, bool removePrefix, char[] spaceChars, string expected) =>
			Assert.AreEqual(expected, input.SplitCamelCase(removePrefix, spaceChars));

		[DataTestMethod()]
		// No Change
		[DataRow("advice", "advice")]
		[DataRow("fish", "fish")]
		[DataRow("Luggage", "Luggage")]
		[DataRow("police", "police")]
		// Irregular
		[DataRow("Agenda", "Agendas")]
		[DataRow("Die", "Dice")]
		[DataRow("Ox", "Oxen")]
		[DataRow("roof", "roofs")]
		[DataRow("person", "people")]
		[DataRow("virus", "viruses")]
		// Already Plural
		[DataRow("Churches", "Churches")]
		[DataRow("Forests", "Forests")]
		[DataRow("dogs", "dogs")]
		// Common
		// 1: ex => ices
		[DataRow("Index", "Indices")]
		[DataRow("Suffix", "Suffixes")] //(irregular)
		// 2: us => i
		[DataRow("Radius", "Radii")]
		[DataRow("virus", "viruses")] // (irregular)
		// 3: is => es
		[DataRow("parenthesis", "parentheses")]
		// 4: on => a
		[DataRow("criterion", "criteria")]
		// 5: um => a
		[DataRow("datum", "data")]
		// 6: ends in y
		[DataRow("party", "parties")]
		[DataRow("day", "days")]
		// 7: add es
		[DataRow("bench", "benches")]
		[DataRow("potato", "potatoes")]
		[DataRow("photo", "photos")] // (irregular)
		[DataRow("boss", "bosses")]
		[DataRow("Dish", "Dishes")]
		[DataRow("fox", "foxes")]
		[DataRow("tax", "taxes")]
		[DataRow("Blitz", "Blitzes")]
		// 8: f / fe => ves
		[DataRow("elf", "elves")]
		[DataRow("wife", "wives")]
		// 9: all others (add s)
		[DataRow("Thing", "Things")]
		[DataRow("test", "tests")]
		[DataRow("dog", "dogs")]
		public void PluralizeTest(string input, string expected) =>
			Assert.AreEqual(expected, input.Pluralize());

		public const string ELLIPSIS = "\u2026";
		[DataTestMethod()]
		[DataRow("testing something", "testing" + ELLIPSIS, 10)]
		[DataRow("testing something", "testin" + ELLIPSIS, 7)]
		[DataRow("testing", "testing", 10)]
		public void TruncateTest(string input, string expected, int maxLen) =>
			Assert.AreEqual(expected, input.Truncate(maxLen));

		[DataTestMethod()]
		[DataRow("testing something", "testing so", 10)]
		[DataRow("testing something", "testin", 6)]
		[DataRow("testing", "testing", 10)]
		public void TrimToTest(string input, string expected, int maxLen) =>
			Assert.AreEqual(expected, input.TrimTo(maxLen));

		[DataTestMethod()]
		[DataRow("something", "something")]
		[DataRow("*something", "%something")]
		[DataRow("*som?thing", "%som_thing")]
		public void ToSqlLikeFilterTest(string input, string expected) =>
			Assert.AreEqual(expected, input.ToSqlLikeFilter());

		[TestMethod()]
		public void ContainsWildcardTest() {
			Assert.IsFalse("test".ContainsWildcard());
			Assert.IsTrue("te?t".ContainsWildcard());
			Assert.IsTrue("*test".ContainsWildcard());
			Assert.IsTrue("*t*e?".ContainsWildcard());
		}

		[TestMethod()]
		public void SplitOnWildcardsTest() {
			string input = null;
			Assert.IsNull(input.SplitOnWildcards());
			input = "";
			Assert.IsNull(input.SplitOnWildcards());
			input = " ";
			Assert.IsNull(input.SplitOnWildcards());
			string s = "Th?s*That";
			string[] res = new string[] { "Th", "s", "That" };
			Assert.IsFalse(res.HasChanges(s.SplitOnWildcards()));
			s += "?";
			Assert.IsFalse(res.HasChanges(s.SplitOnWildcards()));
		}

		[DataTestMethod()]
		[DataRow("something", "*es", false)]
		[DataRow("something", "*ng", true)]
		[DataRow("Mass", "M?ss", true)]
		[DataRow("Mass", "m?ss", false, false)]
		[DataRow("Mess", "M?ss", true)]
		[DataRow("Miss", "M?ss", true)]
		[DataRow("Muss", "*ss", true)]
		[DataRow("fuss", "*ss", true)]
		[DataRow("Miss", "?ess", false)]
		public void EqualsWildcardTest(string text, string pattern, bool expected, bool ignoreCase = true) =>
			Assert.AreEqual(expected, text.EqualsWildcard(pattern, ignoreCase));

		[DataTestMethod()]
		[DataRow("", null)]
		[DataRow("pandemic", null)]
		[DataRow("0", false)]
		[DataRow("1", true)]
		[DataRow("-1", true)]
		[DataRow("True", true)]
		[DataRow("true", true)]
		[DataRow("f", false)]
		[DataRow("t", true)]
		[DataRow("false", false)]
		public void StringToBoolTest(string input, bool? expected) =>
			Assert.AreEqual(expected, input.StringToBool());

		[TestMethod()]
		public void CommonPrefixLengthTest() {
			Assert.AreEqual(0, "test".CommonPrefixLength("pass"));
			Assert.AreEqual(2, "test".CommonPrefixLength("tent"));
			Assert.AreEqual(1, "test".CommonPrefixLength("tarp"));
		}

		[TestMethod()]
		public void CommonSuffixLengthTest() {
			Assert.AreEqual(0, "test".CommonSuffixLength("pass"));
			Assert.AreEqual(1, "test".CommonSuffixLength("tent"));
			Assert.AreEqual(3, "test".CommonSuffixLength("nest"));
		}

		[TestMethod()]
		public void CommonOverlapTest() {
			Assert.AreEqual(0, "test".CommonOverlap("past"));
			Assert.AreEqual(1, "test".CommonOverlap("tent"));
			Assert.AreEqual(2, "test".CommonOverlap("stent"));
		}
	}
}