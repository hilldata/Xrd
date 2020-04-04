using System;
using System.Collections.Generic;
using System.Text;

namespace Xrd.Text {
	/// <summary>
	/// Static class used to Encode/Decode string values for URI consumption.
	/// </summary>
	public static class IETFValueEncoding {
		#region Constants
		/// <summary>
		/// The standard escape character (backslash)
		/// </summary>
		public const string ESCAPE_CHAR = "\\";
		/// <summary>
		/// An escaped escape character (for literally submitting the backslash character.)
		/// </summary>
		public const string ESC_ESCAPE_CHAR = "\\\\";
		/// <summary>
		/// An escaped comma.
		/// </summary>
		public const string ESC_COMMA = "\\,";
		/// <summary>
		/// An escaped semicolon.
		/// </summary>
		public const string ESC_SEMICOLON = "\\;";
		/// <summary>
		/// An escaped double-quote.
		/// </summary>
		public const string ESC_QUOTE = "\\\"";
		/// <summary>
		/// A comma (non-escaped)
		/// </summary>
		public const string COMMA = ",";
		/// <summary>
		/// A semicolon (non-escaped)
		/// </summary>
		public const string SEMICOLON = ";";
		/// <summary>
		/// A double-quote (non-escaped)
		/// </summary>
		public const string QUOTE = "\"";
		/// <summary>
		/// A colon (non-escaped)
		/// </summary>
		public const string COLON = ":";
		/// <summary>
		/// The HTTP markup value for the double-quote character.
		/// </summary>
		public const string MARKUP_QUOTE = "&quot;";
		/// <summary>
		/// A newline entry as used in vCards.
		/// </summary>
		public const string NEWLINE = "\\n";
		/// <summary>
		/// A alternative of the newline entry as used in vCards.
		/// </summary>
		public const string NEWLINE2 = "\\N";
		#endregion

		/// <summary>
		/// Encode a string as per the IETF specs for use in URIs and vCards.
		/// </summary>
		/// <param name="pValue">The plain string to encode.</param>
		/// <param name="encloseInQuotes">a boolean indicating whether or not to enclose the results in quotation marks.</param>
		/// <returns>The encoded string.</returns>
		public static string EncodeValue(string pValue, bool encloseInQuotes = false) {
			if (string.IsNullOrWhiteSpace(pValue))
				return string.Empty;

			string res = pValue.Replace(ESCAPE_CHAR, ESC_ESCAPE_CHAR);
			res = res.Replace(QUOTE, ESC_QUOTE);
			res = res.Replace(COMMA, ESC_COMMA);
			res = res.Replace(SEMICOLON, ESC_SEMICOLON);
			res = res.Replace(Environment.NewLine, NEWLINE);
			if (encloseInQuotes)
				res = QUOTE + res + QUOTE;
			return res;
		}

		/// <summary>
		/// Decode a string that was encoded per the IETF specs for use in URIs and vCards.
		/// </summary>
		/// <param name="pString">The encoded string to decode.</param>
		/// <returns>The plain string value.</returns>
		public static string DecodeValue(string pString) {
			if (string.IsNullOrWhiteSpace(pString))
				return string.Empty;
			int startInd = pString.StartsWith(QUOTE) ? 1 : 0;
			int length = (pString.EndsWith(QUOTE) && !pString.EndsWith(ESC_QUOTE)) ? pString.Length - startInd - 1 : pString.Length - startInd;
			string res = pString.Substring(startInd, length).Replace(NEWLINE, Environment.NewLine).Replace(NEWLINE2, Environment.NewLine);
			res = res.Replace(ESC_SEMICOLON, SEMICOLON);
			res = res.Replace(ESC_COMMA, COMMA);
			res = res.Replace(ESC_QUOTE, QUOTE);
			res = res.Replace(ESC_ESCAPE_CHAR, ESCAPE_CHAR);
			/*if (res.StartsWith(QUOTE)) {
				res = res.Substring(1);
				if (res.EndsWith(QUOTE) && !res.EndsWith(ESC_QUOTE))
					res = res.Substring(0, res.Length - 1);
			}*/
			return res;
		}

		/// <summary>
		/// Encode a string value as per the IETF specs for use in URI/vCard parameters.
		/// </summary>
		/// <param name="value">The plain value to encode.</param>
		/// <returns>The value encoded for used in parameters.</returns>
		public static string EncodeParameterValue(string value) {
			/*string temp;
			if (value.Contains(QUOTE))
				temp = value.Replace(QUOTE, MARKUP_QUOTE);
			else
				temp = value;*/

			bool needQuote = false;
			if (value.Contains(COLON))
				needQuote = true;
			if (value.Contains(SEMICOLON))
				needQuote = true;
			if (value.Contains(COMMA))
				needQuote = true;

			if (needQuote)
				return QUOTE + value.Replace(QUOTE, MARKUP_QUOTE) + QUOTE;
			else
				return value.Replace(QUOTE, MARKUP_QUOTE);
		}

		/// <summary>
		/// Decode a string that was encoded as per the IETF specs for use in parameters.
		/// </summary>
		/// <param name="value">The encoded string</param>
		/// <returns>The plain string value</returns>
		public static string DecodeParameterValue(string value) {
			int start = 0;
			int len = value.Length;
			string temp;
			if (value.StartsWith(QUOTE)) {
				start = 1;
				len--;
			}
			if (value.EndsWith(QUOTE) && !value.EndsWith(ESC_QUOTE))
				len--;

			temp = value.Substring(start, len);
			if (temp.Contains(MARKUP_QUOTE))
				temp = temp.Replace(MARKUP_QUOTE, QUOTE);
			return temp;
		}

		#region ISO 8601.2004 DateTime parsing methods
		/// <summary>
		/// List of format strings for ISO8601 date/time formats.
		/// </summary>
		public static readonly string[] ISO8601DT_FORMATS = {
			// Basic Formats
			"yyyyMMddTHHmmssfffzzzzz",
			"yyyyMMddTHHmmssfffZ",
			"yyyyMMddTHHmmssfff",
			// Extended Formats
			"yyyy-MM-ddTHH:mm:ss.fffzzzzz",
			"yyyy-MM-ddTHH:mm:ss.fffZ",
			"yyyy-MM-ddTHH:mm:ss.fff",
			// All above formats with Accuracy reduced to seconds
			"yyyyMMddTHHmmsszzzzz",
			"yyyyMMddTHHmmssZ",
			"yyyyMMddTHHmmss",
			"yyyy-MM-ddTHH:mm:sszzzzz",
			"yyyy-MM-ddTHH:mm:ssZ",
			"yyyy-MM-ddTHH:mm:ss",
			// Accuracy reduced to minutes
			"yyyyMMddTHHmmzzzzz",
			"yyyyMMddTHHmmZ",
			"yyyyMMddTHHmm",
			"yyyy-MM-ddTHH:mmzzzzz",
			"yyyy-MM-ddTHH:mmZ",
			"yyyy-MM-ddTHH:mm",
			// Accuracy reduced to hours
			"yyyyMMddTHHzzzzz",
			"yyyyMMddTHHZ",
			"yyyyMMddTHH",
			"yyyy-MM-ddTHHzzzzz",
			"yyyy-MM-ddTHHZ",
			"yyyy-MM-ddTHH",
			// Date only, no time
			"yyyyMMddzzzzz",
			"yyyyMMddZ",
			"yyyyMMdd",
			"yyyy-MM-ddzzzzz",
			"yyyy-MM-ddZ",
			"yyyy-MM-dd",
			// Time Only 
			"THHmmssfffzzzzz",
			"THHmmssfffZ",
			"THHmmssfff",
			"THH:mm:ss.fffzzzzz",
			"THH:mm:ss.fffZ",
			"THH:mm:ss.fff",
			"THHmmsszzzzz",
			"THHmmssZ",
			"THHmmss",
			"THH:mm:sszzzzz",
			"THH:mm:ssZ",
			"THH:mm:ss",
			"THHmmzzzzz",
			"THHmmZ",
			"THHmm",
			"THH:mmzzzzz",
			"THH:mmZ",
			"THH:mm",
			};

		/// <summary>
		/// Attempt to parse a DateTime string using any of the ISO8601 formats.
		/// </summary>
		/// <param name="input">The string to parse.</param>
		/// <returns>A datetime value (or null, if the parse failed)</returns>
		public static DateTime? ParseISO8601String(string input) {
			// First, make sure that if the TimeZone is entered, it is 5 digits in length (includes the minutes-offset)
			int index = input.IndexOf('+');
			if (index < 0) {
				if (input.ToUpper().StartsWith("T"))
					index = input.IndexOf('-');
				else
					index = input.IndexOf('-', 8);
			}
			if (index > 0) {
				while (input.Length < index + 5)
					input += "0";
			}

			if (!DateTime.TryParseExact(input, ISO8601DT_FORMATS, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime res))
				return null;
			return res;
		}
		#endregion

		/// <summary>
		/// Convert a DateTime value to a ISO8601 string format.
		/// </summary>
		/// <param name="input">The datetime to format.</param>
		/// <returns>A string representation of the datetime value.</returns>
		public static string ToISO8601String(DateTime? input) {
			if (input == null)
				return string.Empty;
			return input.Value.ToString(ISO8601DT_FORMATS[6]);
		}
	}
}