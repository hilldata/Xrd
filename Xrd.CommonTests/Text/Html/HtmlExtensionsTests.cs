using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrd.Text.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xrd.Text.Html.Tests {
	[TestClass()]
	public class HtmlExtensionsTests {
		private const string TEST_HTML = "<html><header></header><body><H1>Title</H1><p>The quick red fox jumped over the lazy brown dog.</p></body></html>";
		private const string BODY = "<H1>Title</H1><p>The quick red fox jumped over the lazy brown dog.</p>";
		private const string brokenBody = "<h1Title</h1";

		[TestMethod()]
		public void GetNextHtmlTagIndicesTest() {
			Assert.AreEqual(null, string.Empty.GetNextHtmlTagIndices());
			Assert.AreEqual(null, brokenBody.GetNextHtmlTagIndices());

			var expected = new Tuple<int, int>(0, 3);
			var res = BODY.GetNextHtmlTagIndices();
			Assert.AreEqual(expected.Item1, res.Item1);
			Assert.AreEqual(expected.Item2, res.Item2);
		}

		[TestMethod()]
		public void StripTagsTest() {
			string expected = "Title The quick red fox jumped over the lazy brown dog.";
			string res = BODY.StripTags();
			Assert.AreEqual(expected, res);

			Assert.AreEqual(brokenBody, brokenBody.StripTags());
		}

		[TestMethod()]
		public void TruncateHtmlTest() {
			string expected = "Title<p>The quick&#8230;";
			Assert.AreEqual(expected, BODY.TruncateHtml(27));
			Assert.AreNotEqual(expected, BODY.TruncateHtml(28));
			expected = "Title<p>The quick red fox jumped over the lazy brown dog.";
			Assert.AreEqual(expected, BODY.TruncateHtml(1000));
		}

		[TestMethod()]
		public void GetHtmlInnerTextTest() {
			Assert.AreEqual(BODY, TEST_HTML.GetHtmlInnerText());
		}

		[TestMethod()]
		public void FixHtmlTagTest() {
			string expected = "<p>";
			List<string> ok = new List<string>();
			Assert.AreEqual(expected, "<P".FixHtmlTag(ok));
			Assert.AreEqual(expected, "P".FixHtmlTag(ok));
			Assert.AreEqual(expected, "<p>".FixHtmlTag(ok));
			Assert.AreEqual(expected, "<P>".FixHtmlTag(ok));
			ok.Add("<br>");
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => "<p".FixHtmlTag(ok));
		}

		[TestMethod()]
		public void MakeStringUrlSafeTest() {
			string input = null;
			Assert.ThrowsException<ArgumentNullException>(() => input.MakeStringUrlSafe());
			input = string.Empty;
			Assert.ThrowsException<ArgumentNullException>(() => input.MakeStringUrlSafe());
			input = "This is some text";
			string expected = "This%20is%20some%20text";
			Assert.AreEqual(expected, input.MakeStringUrlSafe());
		}

		[TestMethod()]
		public void CreateAnchorTest() {
			string input = null;
			string display = "This is an anchor";
			Assert.ThrowsException<ArgumentNullException>(() => input.CreateAnchor());
			input = " " + Environment.NewLine;
			Assert.ThrowsException<ArgumentNullException>(() => input.CreateAnchor(display));
			string url = "http://fake.com";
			string expected = "<a href=\"http://fake.com\">http://fake.com</a>";
			Assert.AreEqual(expected, url.CreateAnchor());
			expected = "<a href=\"http://fake.com\">This is an anchor</a>";
			Assert.AreEqual(expected, url.CreateAnchor(display));
		}

		[TestMethod()]
		public void CreateTelAnchorTest() {
			string input = null;
			string display = "This is a tel-anchor";
			Assert.ThrowsException<ArgumentNullException>(() => input.CreateTelAnchor());
			input = " " + Environment.NewLine;
			Assert.ThrowsException<ArgumentNullException>(() => input.CreateTelAnchor(display));
			input = "313.555.1234 ";
			string expected = "<a itemprop=\"telephone\" href=\"tel:313.555.1234\">313.555.1234</a>";
			Assert.AreEqual(expected, input.CreateTelAnchor());
			expected = "<a itemprop=\"telephone\" href=\"tel:313.555.1234\">This is a tel-anchor</a>";
			Assert.AreEqual(expected, input.CreateTelAnchor(display));
		}

		[TestMethod()]
		public void CreateEmailAnchorTest() {
			string emailAddress = null;
			Assert.ThrowsException<ArgumentNullException>(() => emailAddress.CreateEmailAnchor());
			emailAddress = "\t";
			Assert.ThrowsException<ArgumentNullException>(() => emailAddress.CreateEmailAnchor());
			string display = string.Empty;
			emailAddress = "me@hotmail.com";
			string expected = "<a itemprop=\"email\" href=\"mailto:me@hotmail.com\">me@hotmail.com</a>";
			Assert.AreEqual(expected, emailAddress.CreateEmailAnchor(display));
			string subject = string.Empty;
			string body = string.Empty;
			string cc = "me2@hotmail.com";
			string bcc = "me2@yahoo.com";
			expected = "<a itemprop=\"email\" href=\"mailto:me@hotmail.com?cc=me2@hotmail.com\">me@hotmail.com</a>";
			Assert.AreEqual(expected, emailAddress.CreateEmailAnchor(display, subject, body, cc));
			subject = "test email";
			display = "email me";
			expected = "<a itemprop=\"email\" href=\"mailto:me@hotmail.com?subject=test%20email&cc=me2@hotmail.com&bcc=me2@yahoo.com\">email me</a>";
			Assert.AreEqual(expected, emailAddress.CreateEmailAnchor(display, subject, body, cc, bcc));
			body = "This is a test email.";
			expected = "<a itemprop=\"email\" href=\"mailto:me@hotmail.com?subject=test%20email&body=This%20is%20a%20test%20email.&cc=me2@hotmail.com&bcc=me2@yahoo.com\">email me</a>";
			Assert.AreEqual(expected, emailAddress.CreateEmailAnchor(display, subject, body, cc,bcc));
		}
	}
}