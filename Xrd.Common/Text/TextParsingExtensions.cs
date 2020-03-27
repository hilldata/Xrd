using System;
using System.Collections.Generic;

namespace Xrd.Text {
	/// <summary>
	/// Extension methods for reading/parsing plain text into structured data formats.
	/// </summary>
	public static class TextParsingExtensions {
		/// <summary>
		/// The standard escape character (backslash)
		/// </summary>
		public const char ESCAPE_CHAR = (char)92;
		/// <summary>
		/// The double-quote character.
		/// </summary>
		public const char DOUBLE_QUOTE = (char)34;
		/// <summary>
		/// The opening double-quote character.
		/// </summary>
		public const char OPEN_DBL_QUOTE = (char)147;
		/// <summary>
		/// The closing double-quote character.
		/// </summary>
		public const char CLOS_DBL_QUOTE = (char)148;
		/// <summary>
		/// A double-quote character as a string.
		/// </summary>
		public const string PLAIN_QUOTE = "\"";
		/// <summary>
		/// A string representation of an escaped double-quote character.
		/// </summary>
		public const string ESCAPED_QUOTE = "\\\"";

		/// <summary>
		/// Get the non-quoted index of the specified character, starting at the specified index location.
		/// </summary>
		/// <param name="text">The string to search.</param>
		/// <param name="value">The character to find.</param>
		/// <param name="startIndex">The starting index (default is 0).</param>
		/// <returns>The position of the first non-escaped/non-quoted instance of the character. Returns -1 if no match was found.</returns>
		/// <remarks>This method ignores any instances of the specified character that are escaped or inside of a quoted range.</remarks>
		public static int NonQuotedIndexOf(this string text, char value, int startIndex = 0) {
			if (string.IsNullOrEmpty(text))
				return -1;
			if (startIndex < 0)
				startIndex = 0;
			if (startIndex >= text.Length)
				return -1;

			bool isEsc = false;
			bool isQuo = false;
			for (int i = startIndex; i < text.Length; i++) {
				if (text[i] == ESCAPE_CHAR && !isEsc)
					isEsc = true;
				else {
					if (isEsc)
						isEsc = false;
					else {
						if (text[i] == DOUBLE_QUOTE)
							isQuo = !isQuo;
						else if (text[i] == OPEN_DBL_QUOTE)
							isQuo = true;
						else if (text[i] == CLOS_DBL_QUOTE)
							isQuo = false;
						if (text[i] == value && !isQuo)
							return i;
					}
				}
			}
			return -1;
		}

		/// <summary>
		/// Get the index of the first non-escaped instance of the specified character.
		/// </summary>
		/// <param name="text">The string to search.</param>
		/// <param name="value">The character to find.</param>
		/// <param name="startIndex">The index to start at. (default = 0)</param>
		/// <returns></returns>
		public static int NonEscapedIndexOf(this string text, char value, int startIndex = 0) {
			if (string.IsNullOrEmpty(text))
				return -1;
			if (startIndex < 0)
				startIndex = 0;
			if (startIndex >= text.Length)
				return -1;

			bool isEsc = false;
			for (int i = startIndex; i < text.Length; i++) {
				if (text[i] == ESCAPE_CHAR && !isEsc)
					isEsc = true;
				else {
					if (isEsc)
						isEsc = false;
					else {
						if (text[i] == value)
							return i;
					}
				}
			}
			return -1;
		}

		/// <summary>
		/// Remove any leading or trailing quotes from the string.
		/// </summary>
		/// <param name="text">The string to clear leading and trailing quotes from.</param>
		/// <returns>The unquoted string.</returns>
		public static string UnQuote(this string text) {
			if (string.IsNullOrWhiteSpace(text))
				return null;
			string res = text.Trim();
			if (string.IsNullOrWhiteSpace(res))
				return res;
			if (res.StartsWith("\""))
				res = res.Substring(1);
			if (res.EndsWith("\""))
				res = res.Substring(0, res.Length - 1);
			return res.Trim();
		}

		/// <summary>
		/// Split the string at the specified character, but only on instances where the character is not escaped and not surrounded by quotes.
		/// </summary>
		/// <param name="text">The string to split.</param>
		/// <param name="separator">The character to split at.</param>
		/// <param name="splitOptions">Should the results include empty strings? Default value is "yes"</param>
		/// <param name="unQuoteText">A boolean value indicating whether or not to remove leading/trailing quotes from the resulting strings. Default is "no"</param>
		/// <returns>A list of strings split at the specified character.</returns>
		public static List<string> NonQuotedSplit(this string text, char separator, StringSplitOptions splitOptions = StringSplitOptions.None, bool unQuoteText = false) {
			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentNullException(nameof(text));
			List<int> indices = new List<int>();
			int i = text.NonQuotedIndexOf(separator);
			if (i < 0)
				return new List<string> { text };
			while (i > 0) {
				indices.Add(i);
				i = text.NonQuotedIndexOf(separator, i + 1);
			}
			List<string> res = new List<string>(indices.Count + 1);
			for (int j = 0; j <= indices.Count; j++) {
				if (j == 0)
					res.Add(text.Substring(0, indices[j]));
				else if (j == indices.Count)
					res.Add(text.Substring(indices[j - 1] + 1, text.Length - indices[j - 1] - 1));
				else
					res.Add(text.Substring(indices[j - 1] + 1, indices[j] - indices[j - 1] - 1));
			}
			if (splitOptions == StringSplitOptions.RemoveEmptyEntries) {
				for (int j = res.Count; j > 1; j--) {
					if (string.IsNullOrWhiteSpace(res[j - 1]))
						res.RemoveAt(j - 1);
				}
			}
			if (unQuoteText) {
				for (int j = 0; j < res.Count; j++) {
					res[j] = res[j].UnQuote();
				}
			}
			return res;
		}

		/// <summary>
		/// Split the string at the specified character, but only on instances where the character is not escaped and not surrounded by quotes.
		/// </summary>
		/// <param name="text">The string to split.</param>
		/// <param name="separators">An array of characters to split on.</param>
		/// <param name="splitOptions">Should the results include empty strings? Default value is "yes"</param>
		/// <param name="unQuoteText">A boolean value indicating whether or not to remove leading/trailing quotes from the resulting strings. Default is "no"</param>
		/// <returns>A list of strings split at the specified characters.</returns>
		public static List<string> NonQuotedSplit(this string text, char[] separators, StringSplitOptions splitOptions = StringSplitOptions.None, bool unQuoteText = false) {
			if (separators == null || separators.Length < 1)
				throw new ArgumentNullException("separators");
			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentNullException(nameof(text));

			List<int> indices = new List<int>();
			bool anyFound = false;
			foreach (char c in separators) {
				int i = text.NonQuotedIndexOf(c);
				while (i > 0) {
					if (!anyFound)
						anyFound = true;
					indices.Add(i);
					i = text.NonQuotedIndexOf(c, i + 1);
				}
			}
			if (!anyFound)
				return new List<string> { text };
			indices.Sort();
			List<string> res = new List<string>(indices.Count + 1);
			for (int j = 0; j <= indices.Count; j++) {
				if (j == 0)
					res.Add(text.Substring(0, indices[j]));
				else if (j == indices.Count)
					res.Add(text.Substring(indices[j - 1] + 1, text.Length - indices[j - 1] - 1));
				else
					res.Add(text.Substring(indices[j - 1] + 1, indices[j] - indices[j - 1] - 1));
			}
			if (splitOptions == StringSplitOptions.RemoveEmptyEntries) {
				for (int j = res.Count; j > 1; j--) {
					if (string.IsNullOrWhiteSpace(res[j - 1]))
						res.RemoveAt(j - 1);
				}
				if (unQuoteText) {
					for (int j = 0; j < res.Count; j++) {
						res[j] = res[j].UnQuote();
					}
				}
			}
			return res;
		}

		/// <summary>
		/// Split the string at the specified character, but only on instances where the character is not escaped and not surrounded by quotes.
		/// Empty entries are removed.
		/// </summary>
		/// <param name="text">The string to split.</param>
		/// <param name="separator">The character to split on.</param>
		/// <param name="unQuoteText">A boolean value indicating whether or not to remove leading/trailing quotes from the result strings.</param>
		/// <returns>A list of strings split at the specified character.</returns>
		public static List<string> NonQuotedSplit(this string text, char separator, bool unQuoteText) {
			return text.NonQuotedSplit(separator, StringSplitOptions.RemoveEmptyEntries, unQuoteText);
		}

		/// <summary>
		/// Split the string at the specified character, but only on instances where the character is not escaped and not surrounded by quotes.
		/// Empty entries are removed.
		/// </summary>
		/// <param name="text">The string to split.</param>
		/// <param name="separators">An array of characters to split on.</param>
		/// <param name="unQuoteText">A boolean value indicating whether or not to remove leading/trailing quotes from the result strings.</param>
		/// <returns>A list of strings split at the specified character.</returns>
		public static List<string> NonQuotedSplit(this string text, char[] separators, bool unQuoteText) {
			return text.NonQuotedSplit(separators, StringSplitOptions.RemoveEmptyEntries, unQuoteText);
		}

		/// <summary>
		/// Split the string at the first non-escaped/non-quoted instance of the specified character.
		/// </summary>
		/// <param name="text">The string to split.</param>
		/// <param name="separator">The character to split at.</param>
		/// <returns>A StringPair (name/value) split at the specified character.</returns>
		/// <remarks>
		/// If the character is not found, the StringPair assumes the entire text as the name, and a null value.
		/// If the character is the first character in the string, the StringPair assumes no name and assigns the entire string as the value.
		/// </remarks>
		public static Tuple<string, string> NonQuotedSplitOnFirst(this string text, char separator) {
			if (string.IsNullOrWhiteSpace(text))
				return null;
			int index = text.NonQuotedIndexOf(separator);
			if (index < 0)
				return new Tuple<string, string>(text, null);
			if (index == 0)
				return new Tuple<string, string>(string.Empty, text.Substring(1));
			return new Tuple<string, string>(
				text.Substring(0, index),
				text.Substring(index + 1, text.Length - index - 1)
				);
		}

		/// <summary>
		/// Insert the escape character before all quotes in the specified text.
		/// </summary>
		/// <param name="text">the string to escape.</param>
		/// <returns>The input with all quotes escaped.</returns>
		public static string EscapeQuotes(this string text) {
			if (string.IsNullOrWhiteSpace(text))
				return null;
			int index = text.NonEscapedIndexOf(DOUBLE_QUOTE, 0);
			string temp = text;
			while (index > -1) {
				temp = temp.Insert(index, ESCAPE_CHAR.ToString());
				index = temp.NonEscapedIndexOf(DOUBLE_QUOTE, index + 2);
			}
			return temp;
		}

		/// <summary>
		/// Remove all escape characters before quotes in the specified text.
		/// </summary>
		/// <param name="text">The text to replace all escaped quotes with quotes.</param>
		/// <returns>A string with all escaped quotes converted to quotes.</returns>
		public static string UnEscapeQuotes(this string text) {
			if (string.IsNullOrWhiteSpace(text))
				return null;
			string temp = text;
			while (temp.IndexOf(ESCAPED_QUOTE) > -1)
				temp = temp.Replace(ESCAPED_QUOTE, PLAIN_QUOTE);
			return temp;
		}

		/// <summary>
		/// Add leading and trailing quotes to the specified text, if they are not already present.
		/// </summary>
		/// <param name="text">The text to add quotes to.</param>
		/// <returns>A string with leading and trailing quotes added.</returns>
		public static string QuoteText(this string text) {
			if (string.IsNullOrWhiteSpace(text))
				return null;
			string temp = text;
			if (!text.StartsWith(PLAIN_QUOTE))
				temp = PLAIN_QUOTE + temp;
			if (text.EndsWith(PLAIN_QUOTE)) {
				if (text.EndsWith(ESCAPED_QUOTE))
					temp += PLAIN_QUOTE;
			} else
				temp += PLAIN_QUOTE;
			return temp;
		}
	}
}