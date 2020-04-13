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
		private const string brokenDoc = "<html><header></header><body><H1>Title</H1><p>The quick red fox jumped over the lazy brown dog.</p></boby></html>";

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
			throw new NotImplementedException();
		}

		[TestMethod()]
		public void GetHtmlInnerTextTest() {
			Assert.AreEqual(BODY, TEST_HTML.GetHtmlInnerText());
		}

		[TestMethod()]
		public void FixHtmlTagTest() {
			throw new NotImplementedException();
		}

		[TestMethod()]
		public void MakeStringUrlSafeTest() {
			throw new NotImplementedException();
		}

		[TestMethod()]
		public void CreateAnchorTest() {
			throw new NotImplementedException();
		}

		[TestMethod()]
		public void CreateTelAnchorTest() {
			throw new NotImplementedException();
		}

		[TestMethod()]
		public void CreateEmailAnchorTest() {
			throw new NotImplementedException();
		}
	}
}