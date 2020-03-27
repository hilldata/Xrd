using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xrd.Collections;

namespace Xrd.Text {
	/// <summary>
	/// Commonly used text functions.
	/// </summary>
	public static class TextExtensions {
		/// <summary>
		/// Does the string contain any non-whitespace characters?
		/// </summary>
		/// <param name="s">The text to test.</param>
		/// <returns><see langword="true"/>, if the text contains non-whitespace characters.</returns>
		public static bool HasValue(this string s) => !string.IsNullOrWhiteSpace(s);

		/// <summary>
		/// Convert an <see cref="Mdl2AssetsEnum"/> to a <see cref="char"/>
		/// </summary>
		/// <param name="mdl2Asset">The Mdl2Asset value</param>
		/// <returns>A char</returns>
		public static char ToChar(this Mdl2AssetsEnum mdl2Asset) =>
			(char)(mdl2Asset);

		/// <summary>
		/// Concatenates a collection of strings into a single string with the individual values separated by the specified string.
		/// </summary>
		/// <param name="enumerable">The collection of TAG strings to combine.</param>
		/// <param name="separator">The string to insert between values.</param>
		/// <returns>A single string suitable for for display in UX</returns>
		public static string Concatenate(this IEnumerable<string> enumerable, string separator = "; ") {
			if (enumerable.IsNullOrEmpty())
				return null;

			StringBuilder sb = new StringBuilder();
			foreach (string s in enumerable.Distinct()) {
				if (!string.IsNullOrWhiteSpace(s)) {
					if (sb.Length > 0)
						sb.Append(separator);
					sb.Append(s.Trim());
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// The Ellipsis character.
		/// </summary>
		public const char ELLIPSIS = (char)8230;
		/// <summary>
		/// Is the text provided a valid email address (not necessarily existing, but at least contains the required components).
		/// </summary>
		/// <param name="text">The text to test.</param>
		/// <param name="isRequired">Options "Is Required" parameters that checks for non-whitespace characters.</param>
		/// <returns>Boolean indicating the success of the test.</returns>
		public static bool IsValidEmail(this string text, bool isRequired = false) {
			// First, check for null or whitespace strings. If it's required, return false.
			if (string.IsNullOrWhiteSpace(text))
				return !isRequired;

			// Next, check bare-minimum length of 5 characters => a@b.c
			if (text.Length < 5)
				return false;


			// Check for the @ symbol.
			int iAt = text.IndexOf('@');
			// If no @ is found, or there is not at least one character before it, FAIL
			if (iAt < 1)
				return false;

			// Split out the domain portion.
			string domain = text.Substring(iAt + 1);
			// Only one @ is allowed.
			if (domain.Contains("@"))
				return false;
			// Make sure at least one dot is preset and that there is at least one character before it.
			int iDot = domain.IndexOf('.');
			if (iDot < 1)
				return false;

			// split out the domain portions and make sure none are empty (two dots in a row) or (nothing after the dot).
			var split = domain.Split(new char[] { '.' }, StringSplitOptions.None);
			foreach (var s in split) {
				if (string.IsNullOrWhiteSpace(s))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Take a string and split it into a list of valid email addresses.
		/// </summary>
		/// <param name="text">The text to split and validate.</param>
		/// <returns>A list of <see cref="IsValidEmail(string)"/> email addresses.</returns>
		/// <remarks>
		/// The split is performed if the input contains any semicolons or commas. If neither are present, it will just validate the string as a single email address. 
		/// During the split, each item is validated and only added to the results if it passes the <see cref="IsValidEmail(string)"/> check.</remarks>
		public static List<string> Split_ValidateEmailAddresses(this string text) {
			if (string.IsNullOrWhiteSpace(text))
				return null;
			List<string> res = new List<string>();
			if (text.Contains(";") || text.Contains(",")) {
				foreach (string s in text.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)) {
					if (s.IsValidEmail())
						res.Add(s.Trim());
				}
			} else if (text.IsValidEmail()) {
				res.Add(text.Trim());
			}
			return res;
		}

		private static readonly char[] DEFAULT_SPACE_CHARS = { '_', '+' };

		/// <summary>
		/// "Splits" a camel-cased word by inserting spaces before upper-cased characters. Also replaces punctuation with spaces and removes lower-case prefixes from the front of the word.
		/// </summary>
		/// <param name="s">The word to split</param>
		/// <returns>String of words with spaces inserted</returns>
		public static string SplitCamelCase(this string s) => s.SplitCamelCase(true, DEFAULT_SPACE_CHARS);
		/// <summary>
		/// "Splits" a camel-cased word by inserting spaces before upper-cased characters. Also replaces specified character with spaces and optionally removes lower-case prefixes from the front of the word.
		/// </summary>
		/// <param name="s">The word to split</param>
		/// <param name="removePrefix">Should the lower-case prefix be removed from the front of the word (if exists)?</param>
		/// <param name="spaceChars">Array of <see cref="char"/>s to be replaced with spaces</param>
		/// <returns>String of words with spaces inserted</returns>
		public static string SplitCamelCase(this string s, bool removePrefix, params char[] spaceChars) {
			if (string.IsNullOrWhiteSpace(s))
				return string.Empty;
			string res = s.Trim();

			// Remove any prefix (lower-case)
			if (removePrefix) {
				int pos = -1;
				for (int i = 0; i < res.Length; i++) {
					if (char.IsUpper(res[i])) {
						pos = i;
						break;
					}
				}
				if (pos > 0)
					res = res.Substring(pos);
			}

			// Work backward through the string to insert a space before every capital letter if the following is lower-case (to avoid acronyms)
			bool currUpper;
			bool nextUpper = false;
			for (int i = res.Length - 1; i > 0; i--) {
				currUpper = char.IsUpper(res[i]);
				if (currUpper && !nextUpper)
					res = res.Insert(i, " ");
				nextUpper = currUpper;
			}

			// Replace any "spaceChars" with spaces
			if (spaceChars != null && spaceChars.Length > 0) {
				foreach (char c in spaceChars)
					res = res.Replace(c, ' ');
			}

			// Remove any double-spaces.
			while (res.IndexOf("  ") >= 0)
				res = res.Replace("  ", " ");

			return res.Trim();
		}

		private static readonly List<string> SingularPluralSame = new List<string>() {
			"advice",
			"aircraft",
			"bison",
			"caribou",
			"cattle",
			"chalk",
			"chassis",
			"chinos",
			"clothing",
			"cod",
			"concrete",
			"correspondence",
			"deer",
			"elk",
			"faux pas",
			"fish",
			"flour",
			"food",
			"furnature",
			"haddock",
			"halibut",
			"help",
			"homework",
			"hovercraft",
			"insignia",
			"knickers",
			"knowledge",
			"kudos",
			"luggage",
			"moose",
			"offspring",
			"pyjamas",
			"police",
			"pendezvous",
			"salmon",
			"sheep",
			"shrimp",
			"spacecraft",
			"squid",
			"swine",
			"trout",
			"tuna",
			"you",
			"wheat",
			"wood"
		};
		private static readonly Dictionary<string, string> IrregularNouns = new Dictionary<string, string>() {
			{"agenda", "agendas" },
			{"belief", "beliefs" },
			{"chef", "chefs" },
			{"chief", "chiefs" },
			{"child", "children" },
			{"concerto", "concertos" },
			{"die", "dice" },
			{"fez", "fezzes" },
			{"foot", "feet" },
			{"gas", "gasses" },
			{"genus", "genera" },
			{"goose", "geese" },
			{"halo", "halos" },
			{"louse", "lice" },
			{"man", "men" },
			{"mouse", "mice" },
			{"octopus", "octopuses" },
			{"opus", "opera" },
			{ "person", "people" },
			{"photo", "photos" },
			{"piano", "pianos" },
			{"roof", "roofs" },
			{"tooth", "teeth" },
			{"virus", "viruses" },
			{"woman", "women" }
		};

		private static bool IsVowel(this char c) {
			if (!char.IsLetter(c))
				return false;
			char[] vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };
			return vowels.Contains(c);
		}
		private static bool IsConsonant(this char c) {
			if (!char.IsLetter(c))
				return false;
			return !c.IsVowel();
		}
		private static string DropFromEnd(this string s, int count) {
			if (s.Length < count)
				throw new ArgumentOutOfRangeException(nameof(count));
			return s.Substring(0, s.Length - count);
		}
		private static char FromEnd(this string s, int reverseIndex) {
			if (s.Length < reverseIndex)
				throw new ArgumentOutOfRangeException(nameof(reverseIndex));
			return s[s.Length - 1 - reverseIndex];
		}

		/// <summary>
		/// Pluralize the specified word
		/// </summary>
		/// <param name="s">The word to pluralize</param>
		/// <returns>The word in its plural form</returns>
		public static string Pluralize(this string s) {
			if (string.IsNullOrWhiteSpace(s))
				return string.Empty;
			s = s.Trim();
			string temp = s.Trim().ToLower();

			// First, check to see if already plural
			if (temp.EndsWith("es") || (temp.FromEnd(0) == 's' && temp.FromEnd(1).IsConsonant()))
				return s;

			// Check identical plural/singular list
			foreach (var ident in SingularPluralSame) {
				if (temp.EndsWith(ident))
					return s;
			}

			// Check irregular list
			foreach (var irr in IrregularNouns) {
				if (temp.EndsWith(irr.Key) || temp.EndsWith(irr.Value))
					return s.DropFromEnd(irr.Key.Length - 1) + irr.Value.Substring(1);
			}

			// Apply rules
			if (temp.EndsWith("a"))
				return s + "e";
			else if (temp.EndsWith("eau"))
				return s + "x";
			else if (temp.EndsWith("ex"))
				return s.DropFromEnd(2) + "ices";
			else if (temp.EndsWith("f"))
				return s.DropFromEnd(1) + "ves";
			else if (temp.EndsWith("fe"))
				return s.DropFromEnd(2) + "ves";
			else if (temp.EndsWith("is"))
				return s.DropFromEnd(2) + "es";
			else if (temp.EndsWith("ix"))
				return s.DropFromEnd(1) + "ces";
			else if (temp.EndsWith("on"))
				return s.DropFromEnd(2) + "a";
			else if (temp.EndsWith("um"))
				return s.DropFromEnd(2) + "a";
			else if (temp.EndsWith("us"))
				return s.DropFromEnd(2) + "i";
			else if (temp.EndsWith("ch") || temp.EndsWith("o") || temp.EndsWith("s") || temp.EndsWith("sh") || temp.EndsWith("ss") || temp.EndsWith("x") || temp.EndsWith("z"))
				return s + "es";
			else if (temp.EndsWith("y"))
				return s.Substring(0, s.Length - 2) + "ies";
			else
				return s + "s";
		}

		/// <summary>
		/// Truncates the input to not exceed the specified length (appends an ellipsis if any characters were truncated).
		/// </summary>
		/// <param name="text">The text to truncate</param>
		/// <param name="maxLen">The maximum length of the result. (default = 250 characters). Less than or equal to ZERO simply trims the string.</param>
		/// <returns>The truncated string. If any characters were actually truncated, an <see cref="ELLIPSIS"/> char is appended</returns>
		public static string Truncate(this string text, int maxLen = 250) {
			if (string.IsNullOrWhiteSpace(text))
				return string.Empty;

			string temp = text.Trim();
			if (maxLen <= 0 || temp.Length <= maxLen + 1)
				return temp;
			int cutoff = temp.IndexOfPreviousNonBreakingChar(maxLen - 1);
			if (cutoff <= 0)
				return temp.Substring(0, maxLen - 1) + ELLIPSIS;
			else
				return temp.Substring(0, cutoff + 1) + ELLIPSIS;
		}

		/// <summary>
		/// Trims the input to not exceed the specified length (nothing is appended if any characters were truncated).
		/// </summary>
		/// <param name="text">The text to truncate</param>
		/// <param name="maxLen">The maximum length of the result. (default = 250 characters). Less than or equal to ZERO simply trims the string.</param>
		/// <returns>The truncated string.</returns>
		public static string TrimTo(this string text, int maxLen = 250) {
			if (string.IsNullOrWhiteSpace(text))
				return string.Empty;

			string temp = text.Trim();
			if (maxLen <= 0 || temp.Length <= maxLen + 1)
				return temp;
			return temp.Substring(0, maxLen);
		}


		// Private method used to find the index of the closest (previous) non-breaking (letter or number) character before the specified maximum length.
		internal static int IndexOfPreviousNonBreakingChar(this string text, int maxLen) {
			if (string.IsNullOrWhiteSpace(text))
				return -1;
			if (text.Length <= maxLen)
				return -1;
			bool breakFound = false;
			for (int i = maxLen - 1; i > 0; i--) {
				if (IsBreakingChar(text[i])) {
					if (!breakFound)
						breakFound = true;
				} else if (breakFound)
					return i;
			}
			return -1;
		}

		// Private method used to determine if the specified character is a breaking character (control, whitespace, punctuation, etc.)
		private static bool IsBreakingChar(char charToCheck) {
			return !char.IsLetterOrDigit(charToCheck);
		}

		/// <summary>
		/// Wildcard character for a single matching character.
		/// </summary>
		public const char SINGLE_CHAR_WILDCARD = '?';
		/// <summary>
		/// Wildcard character for multiple/no matching characters.
		/// </summary>
		public const char MULTI_CHAR_WILDCARD = '*';

		/// <summary>
		/// Wildcard character for a single matching character (for TSQL LIKE filters)
		/// </summary>
		public const char TSQL_SINGLE_CHAR_WILDCARD = '_';
		/// <summary>
		/// Wildcard character for multiple/no matching characters (for TSQL LIKE filters)
		/// </summary>
		public const char TSQL_MULTI_CHAR_WILDCARD = '%';

		/// <summary>
		/// Convert a string with standard filesystem wildcards to a SQL "like" filter
		/// </summary>
		/// <param name="s">The string to convert.</param>
		/// <returns>The filter string.</returns>
		public static string ToSqlLikeFilter(this string s) {
			if (!s.ContainsWildcard())
				return s.Trim();
			return s.Replace(SINGLE_CHAR_WILDCARD, TSQL_SINGLE_CHAR_WILDCARD).Replace(MULTI_CHAR_WILDCARD, TSQL_MULTI_CHAR_WILDCARD).Trim();
		}

		/// <summary>
		/// Determine if the string contains any standard filesystem wildcards.
		/// </summary>
		/// <param name="s">The string to check</param>
		/// <returns>Boolean indicating whether or not the string contains wildcards</returns>
		public static bool ContainsWildcard(this string s) {
			if (string.IsNullOrWhiteSpace(s))
				return false;
			return s.Contains(MULTI_CHAR_WILDCARD.ToString()) || s.Contains(SINGLE_CHAR_WILDCARD.ToString());
		}

		/// <summary>
		/// Split the specified string on wildcard characters.
		/// </summary>
		/// <param name="s">The string to split</param>
		/// <returns>An array of strings.</returns>
		public static string[] SplitOnWildcards(this string s) =>
			s.Split(new char[] { MULTI_CHAR_WILDCARD, SINGLE_CHAR_WILDCARD }, StringSplitOptions.RemoveEmptyEntries);

		/// <summary>
		/// Determines if a string matches a wildcard pattern.
		/// </summary>
		/// <param name="text">The text to compare</param>
		/// <param name="wildcardString">String containing the comparison/matching pattern containing wildcards (* or ?)</param>
		/// <param name="ignoreCase">Boolean indicating whether or not the comparison is case-insensitive (default = ignore character casing)</param>
		/// <returns>A Boolean indicating whether or not the text matches the comparison string.</returns>
		public static bool EqualsWildcard(this string text, string wildcardString, bool ignoreCase = true) {
			// Return false if either of the strings are null or empty.
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(wildcardString))
				return false;

			if (ignoreCase) {
				text = text.ToLower();
				wildcardString = wildcardString.ToLower();
			}
			bool isLike = true;
			byte matchCase = 0;
			char[] filter;
			char[] reversedFilter;
			char[] reversedWord;
			char[] word;
			int currentPatternStartIndex = 0;
			int lastCheckedHeadIndex = 0;
			int lastCheckedTailIndex = 0;
			int reversedWordIndex = 0;
			List<char[]> reversedPatterns = new List<char[]>();

			word = text.ToCharArray();
			filter = wildcardString.ToCharArray();

			// Set which case will be used (0 = no wildcards, 1 = only ?, 2 = only *, 3 = both ? and *
			for (int i = 0; i < filter.Length; i++) {
				if (filter[i] == SINGLE_CHAR_WILDCARD) {
					matchCase += 1;
					break;
				}
			}
			for (int i = 0; i < filter.Length; i++) {
				if (filter[i] == MULTI_CHAR_WILDCARD) {
					matchCase += 2;
					break;
				}
			}

			if ((matchCase == 0 || matchCase == 1) && word.Length != filter.Length)
				return false;

			switch (matchCase) {
				case 0:
					isLike = (text == wildcardString);
					break;
				case 1: {
						for (int i = 0; i < text.Length; i++) {
							if ((word[i] != filter[i]) && filter[i] != SINGLE_CHAR_WILDCARD)
								isLike = false;
						}
						break;
					}
				case 2: {
						//Search for matches until the first *
						for (int i = 0; i < filter.Length; i++) {
							if (filter[i] != MULTI_CHAR_WILDCARD) {
								if (filter[i] != word[i])
									return false;
							} else {
								lastCheckedHeadIndex = i;
								break;
							}
						}
						// Search Tail for matches until first *
						for (int i = 0; i < filter.Length; i++) {
							if (filter[filter.Length - 1 - i] != MULTI_CHAR_WILDCARD) {
								if (filter[filter.Length - 1 - i] != word[word.Length - 1 - i])
									return false;
							} else {
								lastCheckedTailIndex = i;
								break;
							}
						}

						//Create a reverse word and filter for searching in reverse. The reversed word and filter do not include already checks chars
						reversedWord = new char[word.Length - lastCheckedHeadIndex - lastCheckedTailIndex];
						reversedFilter = new char[filter.Length - lastCheckedHeadIndex - lastCheckedTailIndex];
						for (int i = 0; i < reversedWord.Length; i++)
							reversedWord[i] = word[word.Length - (i + 1) - lastCheckedTailIndex];
						for (int i = 0; i < reversedFilter.Length; i++)
							reversedFilter[i] = filter[filter.Length - (i + 1) - lastCheckedTailIndex];

						//Cut up the filter into separate patterns, exclude * as they are no longer needed.
						for (int i = 0; i < reversedFilter.Length; i++) {
							if (reversedFilter[i] == MULTI_CHAR_WILDCARD) {
								if (i - currentPatternStartIndex > 0) {
									char[] pattern = new char[i - currentPatternStartIndex];
									for (int j = 0; j < pattern.Length; j++)
										pattern[j] = reversedFilter[currentPatternStartIndex + j];
									reversedPatterns.Add(pattern);
								}
								currentPatternStartIndex = i + 1;
							}
						}

						//Search for the patterns
						for (int i = 0; i < reversedPatterns.Count; i++) {
							for (int j = 0; j < reversedPatterns[i].Length; j++) {
								if (reversedWordIndex > reversedWord.Length - 1)
									return false;

								if (reversedPatterns[i][j] != reversedWord[reversedWordIndex + j]) {
									reversedWordIndex += 1;
									j = -1;
								} else {
									if (j == reversedPatterns[i].Length - 1)
										reversedWordIndex = reversedWordIndex + reversedPatterns[i].Length;
								}
							}
						}
						break;
					}
				case 3: {
						// Same as Case2, except ? is considered a match
						// Search Head for matches until first *
						for (int i = 0; i < filter.Length; i++) {
							if (filter[i] != MULTI_CHAR_WILDCARD) {
								if (filter[i] != word[i] && filter[i] != SINGLE_CHAR_WILDCARD)
									return false;
								else {
									lastCheckedHeadIndex = i;
									break;
								}
							}
						}
						//Search Tail for matches until first *
						for (int i = 0; i < filter.Length; i++) {
							if (filter[filter.Length - 1 - i] != MULTI_CHAR_WILDCARD) {
								if (filter[filter.Length - 1 - i] != word[word.Length - 1 - i] && filter[filter.Length - 1 - i] != SINGLE_CHAR_WILDCARD)
									return false;
							} else {
								lastCheckedTailIndex = i;
								break;
							}
						}
						// Reverse and trim word and filter.
						reversedWord = new char[word.Length - lastCheckedHeadIndex - lastCheckedTailIndex];
						reversedFilter = new char[filter.Length - lastCheckedHeadIndex - lastCheckedTailIndex];
						for (int i = 0; i < reversedWord.Length; i++)
							reversedWord[i] = word[word.Length - (i + 1) - lastCheckedTailIndex];
						for (int i = 0; i < reversedFilter.Length; i++)
							reversedFilter[i] = filter[filter.Length - (i + 1) - lastCheckedTailIndex];

						for (int i = 0; i < reversedFilter.Length; i++) {
							if (reversedFilter[i] == MULTI_CHAR_WILDCARD) {
								if (i - currentPatternStartIndex > 0) {
									char[] patter = new char[i - currentPatternStartIndex];
									for (int j = 0; j < patter.Length; j++)
										patter[j] = reversedFilter[currentPatternStartIndex + j];
									reversedPatterns.Add(patter);
								}
							}
						}
						// Search for the patterns
						for (int i = 0; i < reversedPatterns.Count; i++) {
							for (int j = 0; j < reversedPatterns[i].Length; j++) {
								if (reversedWordIndex > reversedWord.Length - 1)
									return false;
								if (reversedPatterns[i][j] != SINGLE_CHAR_WILDCARD && reversedPatterns[i][j] != reversedWord[reversedWordIndex + j]) {
									reversedWordIndex += 1;
									j = -1;
								} else {
									if (j == reversedPatterns[i].Length - 1)
										reversedWordIndex = reversedWordIndex + reversedPatterns[i].Length;
								}
							}
						}
						break;
					}
			}
			return isLike;
		}

		/// <summary>
		/// Convert a JS string result to a boolean
		/// </summary>
		/// <param name="jsResult">The JS string result</param>
		/// <returns>A nullable bool of the input (null if the result does not start with F/T or equals 0, 1, -1)</returns>
		public static bool? StringToBool(this string jsResult) {
			if (string.IsNullOrEmpty(jsResult))
				return null;
			string temp = jsResult.Trim().ToLower();
			if (temp == "0" || temp.StartsWith("f"))
				return false;
			else if (temp == "1" || temp.Equals("-1") || temp.StartsWith("t"))
				return true;
			return null;
		}

		#region Commonality Methods (adapted from https://github.com/google/diff-match-patch)
		/// <summary>
		/// Determine the common prefix of two strings.
		/// </summary>
		/// <param name="text1">First string</param>
		/// <param name="text2">Second string</param>
		/// <returns>The number of characters common to the start of each string.</returns>
		public static int CommonPrefixLength(this string text1, string text2) {
			// Performance analysis: https://neil.fraser.name/news/2007/10/09/
			int n = Math.Min(text1.Length, text2.Length);
			for (int i = 0; i < n; i++) {
				if (text1[i] != text2[i])
					return i;
			}
			return n;
		}

		/// <summary>
		/// Determine the common suffix of two strings.
		/// </summary>
		/// <param name="text1">First string</param>
		/// <param name="text2">Second string</param>
		/// <returns>The number of characters common to the end of each string.</returns>
		public static int CommonSuffixLength(this string text1, string text2) {
			// Performance analysis: https://neil.fraser.name/news/2007/10/09/
			int l1 = text1.Length;
			int l2 = text2.Length;
			int n = Math.Min(l1, l2);
			for (int i = 1; i <= n; i++) {
				if (text1[l1 - i] != text2[l2 - i])
					return i - 1;
			}
			return n;
		}

		/// <summary>
		/// Determine if the suffix of one string is the prefix of another. If so, by how many characters do they overlap?
		/// </summary>
		/// <param name="text1">First string.</param>
		/// <param name="text2">Second string.</param>
		/// <returns>The number of characters common to the end of the first string and the start of the second string.</returns>
		public static int CommonOverlap(this string text1, string text2) {
			// eliminate null/empty case.
			if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
				return 0;

			// quick check before proceeding.
			if (text1 == text2)
				return text1.Length;

			// Store the lengths to prevent multiple calls.
			int l1 = text1.Length;
			int l2 = text2.Length;

			// Truncate the longer string.
			if (l1 > l2)
				text1 = text1.Substring(l1 - l2);
			else if (l1 < l2)
				text2 = text2.Substring(0, l1);

			// Quick check for worst case.
			int lText = Math.Min(l1, l2);
			if (text1 == text2)
				return lText;

			// Start by looking for a single character match and increase length until no match is found.
			// Performance analysis: https://neil.fraser.name/news/2010/11/04/
			int best = 0;
			int len = 1;
			while (true) {
				string pattern = text1.Substring(lText - len);
				int found = text2.IndexOf(pattern, StringComparison.Ordinal);
				if (found == -1)
					return best;

				len += found;
				if (found == 0 || text1.Substring(lText - len) == text2.Substring(0, len)) {
					best = len;
					len++;
				}
			}
		}
		#endregion
	}
}