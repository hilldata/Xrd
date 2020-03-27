using System;
using System.Collections.Generic;
using System.Linq;

namespace Xrd.Text.Html {
	/// <summary>
	/// Extension methods for working with HTML text
	/// </summary>
	public static class HtmlExtensions {

		private const char HTML_TAG_OPENING_CHAR = '<';
		private const char HTML_TAG_CLOSING_CHAR = '>';

		/// <summary>
		/// Gets the indices of the first &lt; and following &gt; characters from the specified text.
		/// </summary>
		/// <param name="text">The text to search.</param>
		/// <param name="tagOpenChar">The character that indicates the start of a tag. (optional, default = &lt;)</param>
		/// <param name="tagCloseChar">The character that indicates the end of a tag. (optional, default = &gt;)</param>
		/// <returns>If no combination of tag opening & closing characters is found, null. Otherwise, the index of the first opening tag and next closing tag character.</returns>
		public static Tuple<int, int> GetNextHtmlTagIndices(this string text, char tagOpenChar = HTML_TAG_OPENING_CHAR, char tagCloseChar = HTML_TAG_CLOSING_CHAR) {
			if (string.IsNullOrWhiteSpace(text))
				return null;
			int i1 = text.IndexOf(tagOpenChar);
			if (i1 < 0)
				return null;
			int i2 = text.IndexOf(tagCloseChar, i1);
			if (i2 < 0)
				return null;
			return new Tuple<int, int>(i1, i2);
		}
		/// <summary>
		/// Strips any tags contained in &lt; and &gt; from the text, after first retrieving the inner Body text.
		/// </summary>
		/// <param name="text">The text marked up with HTML style tags.</param>
		/// <returns>The base text without any mark-up.</returns>
		public static string StripTags(this string text) {
			if (string.IsNullOrWhiteSpace(text))
				return null;
			string temp = text.Trim();

			Tuple<int, int> indices = null;

			while ((indices = GetNextHtmlTagIndices(temp)) != null) {
				temp = temp.Remove(indices.Item1, indices.Item2 - indices.Item1 + 1);
			}
			return temp;
		}

		/// <summary>
		/// Array of strings that HTML tags that should be converted to new line
		/// </summary>
		public static string[] TAGS_TO_NEWLINE = { "<br/>", "<br />", "<BR/>", "<BR />", "<br>", "<BR>", "</p>", "</P>" };
		/// <summary>
		/// HTML Paragraph tag
		/// </summary>
		public const string HTML_NEW_PARAGRAPH = "<p>";
		/// <summary>
		/// HTML encoded value for horizontal ellipsis
		/// </summary>
		public const string HTML_HORIZONTAL_ELLIPSIS = "&#8230;";

		/// <summary>
		/// Truncates the provided HTML text to the specified length.
		/// </summary>
		/// <param name="htmlText">The HTML text to be truncated.</param>
		/// <param name="maxLength">The maximum length of the results.</param>
		/// <returns>The truncated text.</returns>
		/// <remarks>
		/// 1) Retrieve the BODY text of the htmlText
		///	2) Replace all line-breaking tags with <see cref="Environment.NewLine"/>
		///	3) Strip out all remaining tags
		///	4) Re-insert line-breaks by replacing <see cref="Environment.NewLine"/> with line-break tags &lt;br/&gt;
		///	5) Truncate the remaining text to the specified length (if applicable). The underlying truncation starts at the maxlength and works backwards to find the first breaking-character to prevent truncating mid-word.
		///	6) Append Html Horizontal Ellipsis to end of text if a truncation occurred.
		/// </remarks>
		public static string TruncateHtml(this string htmlText, int maxLength = 250) {
			if (string.IsNullOrWhiteSpace(htmlText))
				return null;
			string res = htmlText;
			foreach (string s in TAGS_TO_NEWLINE)
				res = res.Replace(s, Environment.NewLine);
			res = res.StripTags();

			res = res.Replace(Environment.NewLine, HTML_NEW_PARAGRAPH);

			if (maxLength <= 0 || res.Length <= maxLength + 7)
				return res;
			int cutoff = res.IndexOfPreviousNonBreakingChar(maxLength - 7);
			if (cutoff <= 0)
				return res.Substring(0, maxLength - 7) + HTML_HORIZONTAL_ELLIPSIS;
			else
				return res.Substring(0, cutoff + 1) + HTML_HORIZONTAL_ELLIPSIS;
		}

		private static readonly string[] BODY_OPENING_TAGS = { "<body", "<BODY" };
		private static readonly string[] BODY_CLOSING_TAGS = { "</body", "</BODY" };

		/// <summary>
		/// Returns the body text of an HTML document. (strips out non-displayed sections to only return the text between "body" tags.
		/// </summary>
		/// <param name="htmlText">The full text to strip header tags from</param>
		/// <returns>The text that appears between "body" tags.</returns>
		public static string GetHtmlInnerText(this string htmlText) {
			if (string.IsNullOrWhiteSpace(htmlText))
				return null;

			htmlText = htmlText.Replace("&nbsp;", " ");

			int startIndex = -1;
			foreach (string s in BODY_OPENING_TAGS) {
				startIndex = htmlText.IndexOf(s);
				if (startIndex > -1)
					break;
			}
			if (startIndex < 0)
				startIndex = htmlText.ToLower().IndexOf(BODY_OPENING_TAGS[0]);

			if (startIndex < 0)
				startIndex = 0;
			else
				startIndex = htmlText.IndexOf(">", startIndex) + 1;

			if (startIndex < 0)
				throw new ArgumentException(nameof(htmlText), "The input HTML text is not well-formed.");

			int endIndex = -1;
			foreach (string s in BODY_CLOSING_TAGS) {
				endIndex = htmlText.IndexOf(s, startIndex);
				if (endIndex > -1)
					break;
			}
			if (endIndex < 0)
				endIndex = htmlText.ToLower().IndexOf(BODY_CLOSING_TAGS[0], startIndex);

			string res;
			if (endIndex < 0)
				res = htmlText.Substring(startIndex);
			else
				res = htmlText.Substring(startIndex, endIndex - startIndex);
			if (string.IsNullOrWhiteSpace(res.StripTags()))
				return null;
			return res;
		}

		/// <summary>
		/// Make sure that the provided HTML tag is properly formatted with braces and, optionally, is included in the list of ok values.
		/// </summary>
		/// <param name="tag">The Tag to fix</param>
		/// <param name="okValues">A list of OK values that the tag should be included in. If the list is populated, but the final tag is not in the list, an exception is thrown</param>
		/// <returns>The "fixed" tag value.</returns>
		public static string FixHtmlTag(this string tag, List<string> okValues = null) {
			if (string.IsNullOrWhiteSpace(tag))
				throw new ArgumentNullException(nameof(tag));
			string temp = tag.Trim().ToLower();
			if (!temp.StartsWith("<"))
				temp = "<" + temp;
			if (!temp.EndsWith(">"))
				temp += ">";
			if (okValues != null && okValues.Count > 0) {
				if (okValues.Contains(temp))
					return temp;
				throw new ArgumentOutOfRangeException(nameof(tag));
			}
			return temp;
		}

		/// <summary>
		/// Make a string URL-safe by replacing space characters with %20
		/// </summary>
		/// <param name="input">The text to make URL-safe.</param>
		/// <returns>The input with any space characters replaced with $20</returns>
		public static string MakeStringUrlSafe(this string input) {
			if (string.IsNullOrWhiteSpace(input))
				throw new ArgumentNullException(nameof(input));
			return input.Trim().Replace(" ", "&20");
		}

		/// <summary>
		/// Create an HTML anchor for the specified URL and display text.
		/// </summary>
		/// <param name="url">The URL to be passed to the HREF property</param>
		/// <param name="displayText">The (optional) text to be displayed. If IsNullOrWhiteSpace, the URL is used as the display text.</param>
		/// <returns>An HTML anchor fragment</returns>
		public static string CreateAnchor(this string url, string displayText = null) {
			if (string.IsNullOrWhiteSpace(url))
				throw new ArgumentNullException(nameof(url));

			if (string.IsNullOrWhiteSpace(displayText))
				return $"<a href=\"{url.Trim()}\">{url.Trim()}</a>";
			else
				return $"<a href=\"{url.Trim()}\">{displayText.Trim()}</a>";
		}

		/// <summary>
		/// Create an HTML anchor using the tel schema for the specified number.
		/// </summary>
		/// <param name="telNumber">The telephone number.</param>
		/// <param name="displayText">The (optional) text to be displayed. If IsNullOrWhiteSpace, the telNumber is used as the display text.</param>
		/// <returns></returns>
		public static string CreateTelAnchor(this string telNumber, string displayText = null) {
			if (string.IsNullOrWhiteSpace(telNumber))
				throw new ArgumentNullException(nameof(telNumber));

			if (string.IsNullOrWhiteSpace(displayText))
				return $"<a itemprop=\"telephone\" href=\"tel:{telNumber.Trim()}\">{telNumber.Trim()}</a>";
			else
				return $"<a itemprop=\"telephone\" href=\"tel:{telNumber.Trim()}\">{displayText.Trim()}</a>";
		}

		/// <summary>
		/// Create an HTML anchor using the mailto schema.
		/// </summary>
		/// <param name="emailAddress">The email address to send to.</param>
		/// <param name="displayText">The [optional] text to be displayed in the anchor (if not defined, the email address is displayed).</param>
		/// <param name="subject">The [optional] subject line of the email to be created.</param>
		/// <param name="body">The [optional] body of the email to be created.</param>
		/// <param name="cc">The [optional] email address to be CC'ed.</param>
		/// <param name="bcc">The [optional] email address to be BCC'ed.</param>
		/// <returns>An HTML mailto: anchor.</returns>
		public static string CreateEmailAnchor(this string emailAddress, string displayText = null, string subject = null, string body = null, string cc = null, string bcc = null) {
			if (string.IsNullOrWhiteSpace(emailAddress))
				throw new ArgumentNullException(nameof(emailAddress));
			if (!emailAddress.IsValidEmail())
				throw new ArgumentOutOfRangeException(nameof(emailAddress), $"{emailAddress} is not a valid email address");

			System.Text.StringBuilder sb = new System.Text.StringBuilder($"<a itemprop=\"email\" href=\"mailto:{emailAddress}");

			List<KeyValuePair<string, string>> parms = new List<KeyValuePair<string, string>>();
			if (!string.IsNullOrWhiteSpace(subject))
				parms.Add(new KeyValuePair<string, string>("subject", System.Net.WebUtility.UrlEncode(subject.Trim())));
			if (!string.IsNullOrWhiteSpace(body))
				parms.Add(new KeyValuePair<string, string>("body", System.Net.WebUtility.UrlEncode(body.Trim())));
			if (!string.IsNullOrWhiteSpace(cc)) {
				var add = cc.Split_ValidateEmailAddresses();
				if (add != null)
					parms.Add(new KeyValuePair<string, string>("cc", add[0].Trim()));
			}
			if (!string.IsNullOrWhiteSpace(bcc)) {
				var add = bcc.Split_ValidateEmailAddresses();
				if (add != null)
					parms.Add(new KeyValuePair<string, string>("bcc", add[0].Trim()));
			}
			if (parms.Count > 0) {
				sb.Append("?");
				var formattedParm = parms.Select(p => p.Key + "=" + p.Value);
				sb.Append(string.Join("&", formattedParm));
			}
			sb.Append("\">");
			if (string.IsNullOrWhiteSpace(displayText))
				sb.Append(emailAddress.Trim());
			else
				sb.Append(displayText.Trim());
			sb.Append("</a>");
			return sb.ToString();
		}
	}
}