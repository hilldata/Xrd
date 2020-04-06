using System;
using System.Text;

namespace Xrd.Text.DiffMatchPatch {
	/// <summary>
	/// JScript compatibility Extension methods
	/// </summary>
	/// <remarks>
	/// Adapted from https://github.com/google/diff-match-patch 
	/// </remarks>
	public static class JScripCompatibilityExtensions {
		/// <summary>
		/// JScript substring function.
		/// </summary>
		/// <param name="s">The string to breakdown</param>
		/// <param name="begin">The index to start at</param>
		/// <param name="end">The index to end at</param>
		/// <returns>The substring from the start/end indices.</returns>
		internal static string JavaSubstring(this string s, int begin, int end) =>
			s.Substring(begin, end - begin);

		/// <summary>
		/// Encodes a string with URI-style % escaping. Compatible with JavaScript's encodeURI function.
		/// </summary>
		/// <param name="s">The string to encode</param>
		/// <returns>The %-escaped encoded string.</returns>
		/// <remarks>System.Web.HttpUtility.UrlEncode is overzealous in replacements, so we will walk back a few.</remarks>
		public static string EncodeURI(this string s) =>
			new StringBuilder(Uri.EscapeDataString(s))
			.Replace('+', ' ')
			.Replace("%20", " ")
			.Replace("%21", "!")
			.Replace("%23", "#")
			.Replace("%24", "$")
			.Replace("%26", "&")
			.Replace("%27", "'")
			.Replace("%28", "(")
			.Replace("%29", ")")
			.Replace("%2a", "*")
			.Replace("%2b", "+")
			.Replace("%2c", ",")
			.Replace("%2f", "/")
			.Replace("%3a", ":")
			.Replace("%3b", ";")
			.Replace("%3d", "=")
			.Replace("%3f", "?")
			.Replace("%40", "@")
			.Replace("%7e", "~")
			.ToString();
	}
}