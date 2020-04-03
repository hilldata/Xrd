using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrd.Names;
using System;
using System.Collections.Generic;
using System.Text;
using Xrd.ChangeTracking;

namespace Xrd.Names.Tests {
	[TestClass()]
	public class PersonNameTests {
		private const string FIRST = "John";
		private const string MIDDLE = "Matthew";
		private const string LAST = "Hill";
		private const string PREFIX = "Mr.";
		private const string SUFFIX = "Sr.";
		private const string NICKNAME = "Johnny";

		private const PersonNameProperties FL = PersonNameProperties.First | PersonNameProperties.Last;
		private const PersonNameProperties FML = FL | PersonNameProperties.Middle;
		private const PersonNameProperties ALL = FML | PersonNameProperties.Nickname | PersonNameProperties.Prefix | PersonNameProperties.Suffix;

		private void TestHelper(string text, PersonNameProperties flags) {
			PersonName input = new PersonName(text);
			if (flags.HasFlag(PersonNameProperties.Last))
				Assert.AreEqual(LAST, input.Last);
			if (flags.HasFlag(PersonNameProperties.First))
				Assert.AreEqual(FIRST, input.First);
			if (flags.HasFlag(PersonNameProperties.Middle))
				Assert.AreEqual(MIDDLE, input.Middle);
			if (flags.HasFlag(PersonNameProperties.Nickname))
				Assert.AreEqual(NICKNAME, input.Nickname);
			if (flags.HasFlag(PersonNameProperties.Prefix))
				TestPrefix(input);
			if (flags.HasFlag(PersonNameProperties.Suffix))
				TestSuffix(input);
		}
		private void TestHelper(PersonName input, PersonNameProperties flags) {
			if (flags.HasFlag(PersonNameProperties.Last))
				Assert.AreEqual(LAST, input.Last);
			if (flags.HasFlag(PersonNameProperties.First))
				Assert.AreEqual(FIRST, input.First);
			if (flags.HasFlag(PersonNameProperties.Middle))
				Assert.AreEqual(MIDDLE, input.Middle);
			if (flags.HasFlag(PersonNameProperties.Nickname))
				Assert.AreEqual(NICKNAME, input.Nickname);
			if (flags.HasFlag(PersonNameProperties.Prefix))
				TestPrefix(input);
			if (flags.HasFlag(PersonNameProperties.Suffix))
				TestSuffix(input);
		}

		private void TestPrefix(PersonName input) {
			string p1 = input.Prefix;
			if (!string.IsNullOrWhiteSpace(p1))
				p1 = p1.Replace(".", string.Empty).ToLower();
			string p2 = PREFIX.Replace(".", string.Empty).ToLower();
			Assert.AreEqual(p1, p2);
		}
		private void TestSuffix(PersonName input) {
			string s1 = input.Suffix;
			if (!string.IsNullOrWhiteSpace(s1))
				s1 = s1.Replace(".", string.Empty).ToLower();
			string s2 = SUFFIX.Replace(".", string.Empty).ToLower();
			Assert.AreEqual(s1, s2);
		}

		[DataTestMethod]
		[DataRow(LAST, PersonNameProperties.Last)]
		[DataRow(FIRST + " " + LAST, FL)]
		[DataRow(FIRST + "  " + LAST, FL)] // Testing double-space
		[DataRow(LAST + ", " + FIRST, FL)]
		[DataRow(FIRST + " " + MIDDLE + " " + LAST, FML)]
		[DataRow(LAST + ", " + FIRST + " " + MIDDLE, FML)]
		[DataRow(PREFIX + " " + FIRST + " " + MIDDLE + " " + LAST + " " + SUFFIX + " \"" + NICKNAME + "\"")]
		[DataRow(LAST + "," + PREFIX + " " + FIRST + " " + MIDDLE + " \"" + NICKNAME + "\"," + SUFFIX)] // Missing space after both commas
		[DataRow(PREFIX + " " + FIRST + " " + MIDDLE + " " + LAST + ", " + SUFFIX + "'" + NICKNAME + "'")] // Missing space between suffix and nickname
		public void PersonNameTest(string text, PersonNameProperties flagsToCheck = ALL) =>
			TestHelper(text, flagsToCheck);

		[DataTestMethod]
		[DataRow(LAST, PersonNameProperties.Last)]
		[DataRow(FIRST + " " + LAST, FL)]
		[DataRow(FIRST + "  " + LAST, FL)] // Testing double-space
		[DataRow(LAST + ", " + FIRST, FL)]
		[DataRow(FIRST + " " + MIDDLE + " " + LAST, FML)]
		[DataRow(LAST + ", " + FIRST + " " + MIDDLE, FML)]
		[DataRow(PREFIX + " " + FIRST + " " + MIDDLE + " " + LAST + " " + SUFFIX + " \"" + NICKNAME + "\"")]
		[DataRow(LAST + "," + PREFIX + " " + FIRST + " " + MIDDLE + " \"" + NICKNAME + "\"," + SUFFIX)] // Missing space after both commas
		[DataRow(PREFIX + " " + FIRST + " " + MIDDLE + " " + LAST + ", " + SUFFIX + "'" + NICKNAME + "'")] // Missing space between suffix and nickname
		public void ParseTest(string text, PersonNameProperties check = ALL) =>
			TestHelper(PersonName.Parse(text), check);

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("  ")]
		[DataRow("\t")]
		[DataRow("\r\n")]
		public void ParseTest_Exceptions(string text) =>
			Assert.ThrowsException<ArgumentNullException>(() => PersonName.Parse(text));

		[DataTestMethod]
		[DataRow(null, false)]
		[DataRow("", false)]
		[DataRow("  ", false)]
		[DataRow(LAST, true, PersonNameProperties.Last)]
		[DataRow(FIRST + " " + LAST, true, FL)]
		[DataRow(FIRST + "  " + LAST, true, FL)] // Testing double-space
		[DataRow(LAST + ", " + FIRST, true, FL)]
		[DataRow(FIRST + " " + MIDDLE + " " + LAST, true, FML)]
		[DataRow(LAST + ", " + FIRST + " " + MIDDLE, true, FML)]
		[DataRow(PREFIX + " " + FIRST + " " + MIDDLE + " " + LAST + " " + SUFFIX + " \"" + NICKNAME + "\"", true)]
		[DataRow(LAST + "," + PREFIX + " " + FIRST + " " + MIDDLE + " \"" + NICKNAME + "\"," + SUFFIX, true)] // Missing space after both commas
		[DataRow(PREFIX + " " + FIRST + " " + MIDDLE + " " + LAST + ", " + SUFFIX + "'" + NICKNAME + "'", true)] // Missing space between suffix and nickname
		public void TryParseTest(string text, bool expected, PersonNameProperties check = ALL) {
			bool res = PersonName.TryParse(text, out PersonName pn);
			Assert.AreEqual(expected, res);
			if (!res)
				Assert.AreEqual(default, pn);
			else
				TestHelper(pn, check);
		}

		private const string inits3 = "JMH";
		private const string inits2 = "JH";

		[DataTestMethod]
		[DataRow(LAST, "?H")]
		[DataRow(FIRST + " " + LAST, inits2)]
		[DataRow(FIRST + "  " + LAST, inits2)] // Testing double-space
		[DataRow(LAST + ", " + FIRST, inits2)]
		[DataRow(FIRST + " " + MIDDLE + " " + LAST)]
		[DataRow(LAST + ", " + FIRST + " " + MIDDLE)]
		[DataRow(PREFIX + " " + FIRST + " " + MIDDLE + " " + LAST + " " + SUFFIX + " \"" + NICKNAME + "\"")]
		[DataRow(LAST + "," + PREFIX + " " + FIRST + " " + MIDDLE + " \"" + NICKNAME + "\"," + SUFFIX)] // Missing space after both commas
		[DataRow(PREFIX + " " + FIRST + " " + MIDDLE + " " + LAST + ", " + SUFFIX + "'" + NICKNAME + "'")] // Missing space between suffix and nickname
		public void ToInitialsTest(string input, string expected = inits3) =>
			Assert.AreEqual(expected, new PersonName(input).ToInitials());

		[DataTestMethod]
		[DataRow(LAST, "?H")]
		[DataRow(FIRST + " " + LAST, inits2)]
		[DataRow(FIRST + "  " + LAST, inits2)] // Testing double-space
		[DataRow(LAST + ", " + FIRST, inits2)]
		[DataRow(FIRST + " " + MIDDLE + " " + LAST)]
		[DataRow(LAST + ", " + FIRST + " " + MIDDLE)]
		[DataRow(PREFIX + " " + FIRST + " " + MIDDLE + " " + LAST + " " + SUFFIX + " \"" + NICKNAME + "\"")]
		[DataRow(LAST + "," + PREFIX + " " + FIRST + " " + MIDDLE + " \"" + NICKNAME + "\"," + SUFFIX)] // Missing space after both commas
		[DataRow(PREFIX + " " + FIRST + " " + MIDDLE + " " + LAST + ", " + SUFFIX + "'" + NICKNAME + "'")] // Missing space between suffix and nickname
		[DataRow("Jane Doe-Smith", "JDS")]
		[DataRow("Jane Jennifer Doe", "JJD")]
		[DataRow("Jane Jennifer Julie Doe", "JJJD")]
		[DataRow("Jane Jennifer Julie Doe-Smith", "JJJDS")]
		public void ToInitialTest_MoreThan3(string input, string expected = inits3) =>
			Assert.AreEqual(expected, new PersonName(input).ToInitials(10));

		[DataTestMethod]
		[DataRow("Ms. Jane Jennifer Julie Doe-Smith, Esq. 'JJ'")]
		[DataRow("Doe-Smith, Ms. Jane Jennifer Julie, Esq. \"JJ\"")]
		public void ParseTestComplicated(string input) {
			PersonName pn = PersonName.Parse(input);
			Assert.AreEqual("Ms.", pn.Prefix);
			Assert.AreEqual("Jane", pn.First);
			Assert.AreEqual("Jennifer Julie", pn.Middle);
			Assert.AreEqual("Doe-Smith", pn.Last);
			Assert.AreEqual("Esq.", pn.Suffix);
			Assert.AreEqual("JJ", pn.Nickname);
		}

		private static readonly PersonName johnTest = new PersonName(FIRST, LAST, PREFIX, MIDDLE, SUFFIX, NICKNAME);
		private const string ORG = "ChiRho Data";
		private static readonly string org = $" ({ORG})";

		[TestMethod]
		public void ToFullNameTest() {
			string expected = "Hill, John";
			Assert.AreEqual(expected, johnTest.ToFullName());
			Assert.AreEqual(expected + org, johnTest.ToFullName(ORG));
			Assert.AreEqual(expected, PersonName.Parse(expected).ToFullName());

			expected += " Matthew";
			Assert.AreEqual(expected, johnTest.ToFullName(includeMiddle: true));
			Assert.AreEqual(expected + org, johnTest.ToFullName(ORG, includeMiddle: true));
			Assert.AreEqual(expected, PersonName.Parse(expected).ToFullName(includeMiddle: true));

			expected = "Hill, Mr. John";
			Assert.AreEqual(expected, johnTest.ToFullName(includePrefix: true));
			Assert.AreEqual(expected + org, johnTest.ToFullName(ORG, includePrefix: true));
			Assert.AreEqual(expected, PersonName.Parse(expected).ToFullName(includePrefix: true));

			expected += " Matthew";
			Assert.AreEqual(expected, johnTest.ToFullName(includeMiddle: true, includePrefix: true));
			Assert.AreEqual(expected + org, johnTest.ToFullName(ORG, includeMiddle: true, includePrefix: true));
			Assert.AreEqual(expected, PersonName.Parse(expected).ToFullName(includeMiddle: true, includePrefix: true));

			expected = "Hill, John, Sr.";
			Assert.AreEqual(expected, johnTest.ToFullName(includeSuffix: true));
			Assert.AreEqual(expected + org, johnTest.ToFullName(ORG, includeSuffix: true));
			Assert.AreEqual(expected, PersonName.Parse(expected).ToFullName(includeSuffix: true));

			expected = "Hill, John \"Johnny\"";
			Assert.AreEqual(expected, johnTest.ToFullName(includeNickname: true));
			Assert.AreEqual(expected + org, johnTest.ToFullName(ORG, includeNickname: true));
			Assert.AreEqual(expected, PersonName.Parse(expected).ToFullName(includeNickname: true));

			expected = "Hill, Mr. John \"Johnny\" Matthew, Sr.";
			Assert.AreEqual(expected, johnTest.ToFullName(null, true, true, true, true, true));
			Assert.AreEqual(expected + org, johnTest.ToFullName(ORG, true, true, true, true, true));
			Assert.AreEqual(expected, PersonName.Parse(expected).ToFullName(null, true, true, true, true));


			expected = "John Hill";
			Assert.AreEqual(expected, johnTest.ToFullName(lastFirst: false));
			Assert.AreEqual(expected + org, johnTest.ToFullName(ORG, lastFirst: false));
			Assert.AreEqual(expected, PersonName.Parse(expected).ToFullName(lastFirst: false));

			expected = "John Matthew Hill";
			Assert.AreEqual(expected, johnTest.ToFullName(includeMiddle: true, lastFirst: false));
			Assert.AreEqual(expected + org, johnTest.ToFullName(ORG, includeMiddle: true, lastFirst: false));
			Assert.AreEqual(expected, PersonName.Parse(expected).ToFullName(includeMiddle: true, lastFirst: false));

			expected = "Mr. John \"Johnny\" Matthew Hill, Sr.";
			Assert.AreEqual(expected, johnTest.ToFullName(null, true, true, true, true, false));
			Assert.AreEqual(expected + org, johnTest.ToFullName(ORG, true, true, true, true, false));
			Assert.AreEqual(expected, PersonName.Parse(expected).ToFullName(null, true, true, true, true, false));
		}

		[TestMethod]
		public void EqualsTest() {
			string input = "John Matthew Hill";
			Assert.IsFalse(johnTest.Equals(PersonName.Parse(input)));
			input = "Mr. John \"Johnny\" Matthew Hill, Sr.";
			Assert.IsTrue(johnTest.Equals(PersonName.Parse(input)));
		}

		[TestMethod]
		public void GetHashCodeTest() {
			int hash = johnTest.GetHashCode();
			string input = "John Matthew Hill";
			Assert.AreEqual(hash, PersonName.Parse(input).GetHashCode());
			input = "Mr. John \"Johnny\" Matthew Hill, Sr.";
			Assert.AreEqual(hash, PersonName.Parse(input).GetHashCode());
			input = "John Hill";
			Assert.AreNotEqual(hash, PersonName.Parse(input).GetHashCode());
		}

		[TestMethod()]
		public void ToFullNameTest1() {
			string expected = "Hill, John \"Johnny\"";
			Assert.AreEqual(expected, johnTest.ToFullName(PersonNameProperties.Nickname));
			Assert.AreEqual(expected + org, johnTest.ToFullName(PersonNameProperties.Nickname, ORG));
			Assert.AreEqual(expected, PersonName.Parse(expected).ToFullName(includeNickname: true));

			expected = "Hill, Mr. John \"Johnny\" Matthew, Sr.";
			Assert.AreEqual(expected, johnTest.ToFullName(ALL));
			Assert.AreEqual(expected + org, johnTest.ToFullName(ALL, ORG));
			Assert.AreEqual(expected, PersonName.Parse(expected).ToFullName(ALL));

			expected = "Mr. John Hill";
			Assert.AreEqual(expected, johnTest.ToFullName(FL | PersonNameProperties.Prefix, null, false));
		}

		[TestMethod()]
		public void GetValueTest() {
			Assert.AreEqual(PREFIX, johnTest[PersonNameProperties.Prefix]);
			Assert.AreEqual(SUFFIX, johnTest[PersonNameProperties.Suffix]);
			Assert.AreEqual(FIRST, johnTest[PersonNameProperties.First]);
			Assert.AreEqual(MIDDLE, johnTest[PersonNameProperties.Middle]);
			Assert.AreEqual(LAST, johnTest[PersonNameProperties.Last]);
			Assert.AreEqual(NICKNAME, johnTest.GetValue(PersonNameProperties.Nickname));

			// CREATE A COPY!!! Tests run asynchronously!
			PersonName test = new PersonName(johnTest.ToString());
			/*johnTest[PersonNameProperties.Nickname] = "SomethingElse";
			Assert.AreNotEqual(NICKNAME, johnTest.GetValue(PersonNameProperties.Nickname));*/
			test[PersonNameProperties.Nickname] = "SomethingElse";
			Assert.AreNotEqual(NICKNAME, test.GetValue(PersonNameProperties.Nickname));
		}
	}
}