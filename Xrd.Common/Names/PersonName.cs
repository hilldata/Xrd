using System;
using Xrd.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xrd.Text;

namespace Xrd.Names {
	/// <summary>
	/// Structured person's name. Used to parse a text into a structured name.
	/// </summary>
	/// <example>
	/// In order to correctly parse an Honorific Title (Mr., Ms., Dr.), it either needs to be located:
	///		1) If Formatting Last/Family/Surname first, it must follow the comma, but proceed the given name.
	///		2) If Formatting Given (Middle) Family name, it must be at the beginning of the input.
	///	English patrimonial suffixes (Jr, II, IV, etc.) and SOME credential suffixes should be found 
	///		whether or not they are proceeded by a comma.
	///	Credentialing suffixes are guaranteed to be found if they are proceeded by a comma
	/// 
	/// Accepted input format orders for parsing/constructing a new instance. 
	/// (Optional fields and associated comma are in paratheses.)
	///		1) Last 
	///			* A single word is always assigned to LAST name.
	///		2) Last, (Prefix) First (Middle)(, Suffix)
	///		3) (Prefix) First (Middle) Last(, Suffix)
	///		
	/// **Nicknames (enclosed in single- or double-quotes) are identified first, so location is immaterial.**
	/// </example>
	/// <remarks>
	/// Legend for fields as they may be identified outside of the US:
	///		Prefix:		Honorific Title/Prefix (Mr., Ms., Dr., etc)
	///		First:		First/Given name (John, Juan, Johan, Ivan, etc.)
	///		Middle:		Middle/Additional Names/Initials
	///		Last:		Last/Family/Surname (Smith, Yeltzin, Gonzales, etc.)
	///		Suffix:		Honorofic Suffix (patrimonial or credentials; e.g. Jr., Sr., FAACP, MD, DDS, etc.)
	///		Nickname:	Preferred name or alias
	/// </remarks>
	public class PersonName {
		public PersonName() { }
		/// <summary>
		/// Construct an instance of PersonName by parsing the text provided.
		/// </summary>
		/// <param name="text">The text to parse into a name.</param>
		public PersonName(string text) {
			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentNullException(nameof(text));

			// Temp value to be search/changed
			string working = text.Trim();

			// Eliminate all double-spaces.
			while (working.Contains("  "))
				working = working.Replace("  ", " ");

			// Initialize variables
			Prefix = string.Empty;
			Last = string.Empty;
			First = string.Empty;
			Middle = string.Empty;
			Suffix = string.Empty;
			Nickname = string.Empty;

			// Check for nicknames (in quotes)
			if (working.Contains("\"")) {
				int x1 = working.IndexOf("\"");
				int x2 = working.IndexOf("\"", x1 + 1);
				if (x2 > x1) {
					Nickname = working.Substring(x1 + 1, x2 - x1 - 1).Trim();
					working = text.Replace("\"" + Nickname + "\"", string.Empty);
				}
			} else if (working.Contains("'")) {
				int x1 = working.IndexOf("'");
				int x2 = working.IndexOf("'", x1 + 1);
				if (x2 > x1) {
					Nickname = working.Substring(x1 + 1, x2 - x1 - 1).Trim();
					working = working.Replace("'" + Nickname + "'", string.Empty);
				}
			}

			// Temp array for catching prefix
			List<string> vsP = new List<string>();
			
			StringBuilder m = new StringBuilder();

			// Check to see if text contains comma.
			if (working.Contains(",")) {
				int commaInd = working.IndexOf(",");
				int spaceInd = working.IndexOf(" ");
				// If a space occurs before the comma, it's most likely formatted as [First Last, Suffix]
				if (spaceInd > 0 && spaceInd < commaInd) {
					Suffix = working.Substring(commaInd + 1).Trim();
					working = working.Substring(0, commaInd);
					// Check for known prefixes.
					foreach (var prefix in Honorifics.ForParsingName) {
						if (working.ToLower().StartsWith(prefix.ToLower())) {
							vsP.Add(prefix);
							working = working.Substring(prefix.Length).Trim();
						}
					}
					Prefix = vsP.Concatenate(" ");
					vsP.Clear();

					var split = splitText(working).ToList();
					if (split.Count > 2) {
						First = split.First().Trim();
						Last = split.Last().Trim();
						foreach (var s in split.Skip(1).Take(split.Count - 2)) {
							m.Append(" ");
							m.Append(s);
						}
						Middle = m.ToString().Trim();
					} else if (split.Count == 2) {
						First = split.First().Trim();
						Last = split.Last().Trim();
					} else {
						Last = split[0].Trim();
					}
				} else { // If there is no space before the comma, it's most likely formatted as [Last, First]
					Last = working.Substring(0, commaInd).Trim();
					working = working.Substring(commaInd + 1).Trim();

					// Check for known prefixes.
					foreach (var prefix in Honorifics.ForParsingName) {
						if (working.ToLower().StartsWith(prefix.ToLower())) {
							vsP.Add(prefix);
							working = working.Substring(prefix.Length).Trim();
						}
					}

					Prefix = vsP.Concatenate(" ");
					vsP.Clear();
					// Check for additional comma. If exists, what follows is suffix
					int index = working.IndexOf(",");
					if(index >0) {
						Suffix = working.Substring(index + 1).Trim();
						working = working.Substring(0, index).Trim();
					}

					if (!working.Contains(" ")) {
						First = working.Trim();
					} else {
						var split = splitText(working);
						First = split.First().Trim();
						foreach (var s in split.Skip(1)) {
							m.Append(" ");
							m.Append(s.Trim());
						}
						Middle = m.ToString().Trim();
					}
				}
			} else { // No commas. Search for common suffixes and assume it's formatted as [First Last]
				if (!working.Contains(" ")) {// No space. It's just a last name.
					Last = working.Trim();
				} else {
					// Check for known prefixes.
					foreach (var prefix in Honorifics.ForParsingName) {
						if (working.ToLower().StartsWith(prefix.ToLower())) {
							vsP.Add(prefix);
							working = working.Substring(prefix.Length).Trim();
						}
					}
					Prefix = vsP.Concatenate(" ");
					vsP.Clear();

					var split = splitText(working).ToList();
					string suff = string.Empty;
					foreach (var s in split) {
						if (CommonSuffixes.Contains(s, StringComparer.OrdinalIgnoreCase)) {
							suff = s;
						}
					}
					if (!string.IsNullOrEmpty(suff)) {
						split.Remove(suff);
						Suffix = suff.Trim();
					}
					if (split.Count == 1)
						Last = split.First().Trim();
					else if (split.Count == 2) {
						First = split.First().Trim();
						Last = split.Last().Trim();
					} else {
						First = split.First().Trim();
						Last = split.Last().Trim();
						foreach (var s in split.Skip(1).Take(split.Count - 2)) {
							m.Append(" ");
							m.Append(s);
						}
						Middle = m.ToString().Trim();
					}
				}
			}
		}

		/// <summary>
		/// Construct an instance of PersonName using the provided values.
		/// </summary>
		/// <param name="first">The first/given name.</param>
		/// <param name="last">The last/family name.</param>
		/// <param name="prefix">Honorific prefix</param>
		/// <param name="middle">Middle name(s)</param>
		/// <param name="suffix">Suffix (generational or credentials)</param>
		/// <param name="nickname">Preferred nickname/alias.</param>
		public PersonName(string first, string last, string prefix = null, string middle = null, string suffix = null, string nickname = null) {
			Prefix = prefix.HasValue() ? prefix.Trim() : string.Empty;
			First = first.HasValue() ? first.Trim() : string.Empty;
			Middle = middle.HasValue() ? middle.Trim() : string.Empty;
			Last = last.HasValue() ? last.Trim() : string.Empty;
			Suffix = suffix.HasValue() ? suffix.Trim() : string.Empty;
			Nickname = nickname.HasValue() ? nickname.Trim() : string.Empty;
		}

		/// <summary>
		/// Converts the string into a structured personal name.
		/// </summary>
		/// <param name="text">A string that contains the value to parse.</param>
		/// <returns>The structured personal name.</returns>
		public static PersonName Parse(string text) => new PersonName(text);
		/// <summary>
		/// Converts the string into a structure personal name and returns a value that indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="text">A string that contains the value to parse.</param>
		/// <param name="name">The structured personal name.</param>
		/// <returns>A value that indicates whether the conversion succeeded.</returns>
		public static bool TryParse(string text, out PersonName name) {
			if (!text.HasValue()) {
				name = default;
				return false;
			}
			name = new PersonName(text);
			return true;
		}

		/// <summary>
		/// The person's honorific prefix.
		/// </summary>
		public string Prefix { get; set; }
		/// <summary>
		/// The person's first/given name.
		/// </summary>
		public string First { get; set; }
		/// <summary>
		/// The person's middle name(s)/initial(s).
		/// </summary>
		public string Middle { get; set; }
		/// <summary>
		/// The person's last/family name.
		/// </summary>
		public string Last { get; set; }
		/// <summary>
		/// The person's suffix.
		/// </summary>
		public string Suffix { get; set; }
		/// <summary>
		/// The person's preferred nickname/alias.
		/// </summary>
		public string Nickname { get; set; }

		/// <summary>
		/// List of common patrimonial and credentialing suffixes.
		/// </summary>
		private static List<string> CommonSuffixes => new List<string>() {
			"JR",
			"JR.",
			"SR",
			"SR.",
			"I",
			"II",
			"III",
			"IV",
			"V",
			"VI",
			"MD",
			"M.D.",
			"PHD",
			"PH.D.",
			"DO",
			"D.O.",
			"DDS",
			"D.D.S."
		};

		private string[] splitText(string text, char c = ' ') {
			if (!text.Contains(c.ToString()))
				return new string[] { text.Trim() };
			return text.Split(new char[] { c }, StringSplitOptions.RemoveEmptyEntries);
		}

		private const char QUESTION = '?';
		private string midInit {
			get {
				if (string.IsNullOrWhiteSpace(Middle))
					return string.Empty;
				if(Middle.Contains(" ")) {
					StringBuilder sb = new StringBuilder();
					var split = Middle.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (var n in split)
						sb.Append(n[0]);
					return sb.ToString();
				}
				return Middle.Substring(0,1);
			}
		}
		private string lastInit {
			get {
				if (string.IsNullOrWhiteSpace(Last))
					return string.Empty;
				if(Last.Contains(" ") || Last.Contains("-")) {
					StringBuilder sb = new StringBuilder();
					var split = Last.Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
					foreach(var n in split) {
						sb.Append(n[0]);
					}
						return sb.ToString();
				}
				return Last.Substring(0, 1);
			}
		}

		/// <summary>
		/// Gets the initials from the name
		/// </summary>
		/// <param name="maxLength">The maximum number of characters to include in the initials. (minimum == 2)</param>
		/// <returns>The initials from the name.</returns>
		/// <remarks>
		/// Will always return at least 2 characters. Empty field values will be replaced with a question mark,
		/// e.g. _ Hill => ?H; John _ => J?
		/// </remarks>
		public string ToInitials(int maxLength = 3) {
			StringBuilder sb = new StringBuilder();
			if (maxLength < 2)
				maxLength = 2;
			// if maxLength == 2, only concerned with First/Last
			// If either are blank, replace with question mark (QUESTION)
			if (maxLength == 2) {
				if (First.HasValue())
					sb.Append(First[0]);
				else
					sb.Append(QUESTION);

				if (Last.HasValue())
					sb.Append(Last[0]);
				else
					sb.Append(QUESTION);
				return sb.ToString();
			} else {
				// First initial
				if (First.HasValue()) {
					sb.Append(First[0]);
				}

				// Middle initial
				string mi = midInit;
				if (mi.HasValue()) {
					sb.Append(mi[0]);
				}else {
					// If there was no middle name and no first name, start with a question mark
					if (sb.Length < 1)
						sb.Append(QUESTION);
				}

				// Last initial(s)
				string li = lastInit;
				if (li.HasValue()) {
					sb.Append(li);
				}else {
					sb.Append(QUESTION);
				}
				
				// Make sure total length is not greater than max length.
				if(sb.Length > maxLength) {
					int i = sb.Length - maxLength;
					sb.Remove(sb.Length - i, i);
				} else if(sb.Length < maxLength && mi.Length >1) {
					// Finally, check to see if maxLength is met. 
					// If not, check to see if middle initials has more than one character.
					// If so, insert each character until the maxLength is met.
					int i = 1;
					while(sb.Length < maxLength && i < mi.Length) {
						sb.Insert(i + 1, mi[i]);
						i++;
					}
				}
				return sb.ToString();
			}
		}

		private const string COMMA = ", ";
		private const string SPACE = " ";
		private const string QUOTE1 = "\"";
		private const string QUOTE2 = "\" ";
		private const string PAREN1 = "(";
		private const string PAREN2 = ") ";

		public override string ToString() => ToFullName(includedFields: PersonNameProperties.First | PersonNameProperties.Last | PersonNameProperties.Middle | PersonNameProperties.Nickname | PersonNameProperties.Prefix | PersonNameProperties.Suffix);

		/// <summary>
		/// Build a person's formatted full name for display using the provided criteria.
		/// *ALWAYS* includes first and last name (if present)
		/// </summary>
		/// <param name="orgName">The name of the organization the person is primarily associated with.</param>
		/// <param name="includeMiddle">Should the middle name(s) be included in the result?</param>
		/// <param name="includePrefix">Should the honorific prefix be included in the result?</param>
		/// <param name="includeSuffix">Should the honorific suffix be included in the result?</param>
		/// <param name="includeNickname">Should the nickname be included in the result?</param>
		/// <param name="lastFirst">Should the last/family name be listed first?</param>
		/// <returns>The full name as computed from the provided criteria.</returns>
		public string ToFullName(string orgName = null,
			bool includeMiddle = false, 
			bool includePrefix = false, 
			bool includeSuffix = false, 
			bool includeNickname = false, 
			bool lastFirst = true) {
			StringBuilder sb = new StringBuilder();
			if (lastFirst) {
				if (Last.HasValue()) {
					sb.Append(Last);
					sb.Append(COMMA);
				}
				if (includePrefix && Prefix.HasValue()) {
					sb.Append(SPACE);
					sb.Append(Prefix);
					sb.Append(SPACE);
				}
				if (First.HasValue()) {
					sb.Append(First);
					sb.Append(SPACE);
				}
				if (includeNickname && Nickname.HasValue()) {
					sb.Append(QUOTE1);
					sb.Append(Nickname);
					sb.Append(QUOTE2);
				}
				if (includeMiddle && Middle.HasValue()) {
					sb.Append(Middle);
					sb.Append(SPACE);
				}
				if (includeSuffix && Suffix.HasValue()) {
					sb.Append(COMMA);
					sb.Append(Suffix);
				}
			} else {
				if (includePrefix && Prefix.HasValue()) {
					sb.Append(Prefix);
					sb.Append(SPACE);
				}
				if (First.HasValue()) {
					sb.Append(First);
					sb.Append(SPACE);
				}
				if (includeNickname && Nickname.HasValue()) {
					sb.Append(QUOTE1);
					sb.Append(Nickname);
					sb.Append(QUOTE2);
				}
				if (includeMiddle && Middle.HasValue()) {
					sb.Append(Middle);
					sb.Append(SPACE);
				}
				if (Last.HasValue()) {
					sb.Append(Last);
				}
				if (includeSuffix && Suffix.HasValue()) {
					sb.Append(COMMA);
					sb.Append(Suffix);
				}
			}

			if (orgName.HasValue()) {
				sb.Append(SPACE);
				sb.Append(PAREN1);
				sb.Append(orgName);
				sb.Append(PAREN2);
			}

			string res = sb.ToString().Replace(" ,", ",");
			while (res.EndsWith(","))
				res = res.Substring(0, res.Length - 1);
			while (res.Contains("  "))
				res = res.Replace("  ", " ");
			return res.Trim();
		}

		/// <summary>
		/// Build a person's formatted full name for display using the provided criteria.
		/// *ALWAYS* includes first and last name (if present)
		/// </summary>
		/// <param name="includedFields">The fields to include.</param>
		/// <param name="orgName">The name of the organization the person is primarily associated with.</param>
		/// <param name="lastFirst">Should the last/family name be listed first?</param>
		/// <returns>The full name as computed from teh provided criteria</returns>
		public string ToFullName(PersonNameProperties includedFields, string orgName = null, bool lastFirst = true) =>
			ToFullName(orgName,
				includeMiddle: includedFields.HasFlag(PersonNameProperties.Middle),
				includePrefix: includedFields.HasFlag(PersonNameProperties.Prefix),
				includeSuffix: includedFields.HasFlag(PersonNameProperties.Suffix),
				includeNickname: includedFields.HasFlag(PersonNameProperties.Nickname),
				lastFirst
			);

		/// <summary>
		/// Indexer for name properties.
		/// </summary>
		/// <param name="name">The name property to get or set.</param>
		/// <returns>The value of the name property.</returns>
		public string this[PersonNameProperties name] {
			get {
				switch (name) {
					case PersonNameProperties.First: return First.HasValue() ? First : string.Empty;
					case PersonNameProperties.Last: return Last.HasValue() ? Last : string.Empty;
					case PersonNameProperties.Middle: return Middle.HasValue() ? Middle : string.Empty;
					case PersonNameProperties.Nickname: return Nickname.HasValue() ? Nickname : string.Empty;
					case PersonNameProperties.Prefix: return Prefix.HasValue() ? Prefix : string.Empty;
					case PersonNameProperties.Suffix: return Suffix.HasValue() ? Suffix : string.Empty;
					default: return string.Empty;
				}
			}
			set {
				switch (name) {
					case PersonNameProperties.First:
						First = value.HasValue() ? value.Trim() : string.Empty;
						break;
					case PersonNameProperties.Last:
						Last = value.HasValue() ? value.Trim() : string.Empty;
						break;
					case PersonNameProperties.Middle:
						Middle = value.HasValue() ? value.Trim() : string.Empty;
						break;
					case PersonNameProperties.Nickname:
						Nickname = value.HasValue() ? value.Trim() : string.Empty;
						break;
					case PersonNameProperties.Prefix:
						Prefix = value.HasValue() ? value.Trim() : string.Empty;
						break;
					case PersonNameProperties.Suffix:
						Suffix = value.HasValue() ? value.Trim() : string.Empty;
						break;
					default: break;
				}
			}
		}

		/// <summary>
		/// Get the value of the specified name property
		/// </summary>
		/// <param name="name">The name property to retrieve</param>
		/// <param name="maxLen">The maximum length of the value to return. If the actual value is longer, it is <see cref="TextExtensions.Truncate(string, int)"/>d to the specified length.</param>
		/// <returns>Either <see cref="string.Empty"/> (for null or empty values) or the value of the name property truncated to the specified maxLen (if applicable).</returns>
		public string GetValue(PersonNameProperties name, int maxLen = 50) {
			switch (name) {
				case PersonNameProperties.First: return First.HasValue() ? First.Truncate(maxLen) : string.Empty;
				case PersonNameProperties.Last: return Last.HasValue() ? Last.Truncate(maxLen) : string.Empty;
				case PersonNameProperties.Middle: return Middle.HasValue() ? Middle.Truncate(maxLen) : string.Empty;
				case PersonNameProperties.Nickname: return Nickname.HasValue() ? Nickname.Truncate(maxLen) : string.Empty;
				case PersonNameProperties.Prefix: return Prefix.HasValue() ? Prefix.Truncate(maxLen) : string.Empty;
				case PersonNameProperties.Suffix: return Suffix.HasValue() ? Suffix.Truncate(50) : string.Empty;
				default: return string.Empty;
			}
		}

		/// <summary>
		/// Is the specified object equivalent to this instance? (All fields identical)
		/// </summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns><see langword="true"/>, if the object is a <see cref="PersonName"/> and ALL fields are identical; otherwise, <see langword="false"/></returns>
		public override bool Equals(object obj) {
			if (!(obj is PersonName pn))
				return false;

			if (First.HasChanges(pn.First))
				return false;
			if (Last.HasChanges(pn.Last))
				return false;
			if (Middle.HasChanges(pn.Middle))
				return false;
			if (Prefix.HasChanges(pn.Prefix))
				return false;
			if (Suffix.HasChanges(pn.Suffix))
				return false;
			if (Nickname.HasChanges(pn.Nickname))
				return false;
			return true;
		}

		/// <summary>
		/// Override of the <see cref="Object.GetHashCode"/> Method.
		/// </summary>
		/// <returns>A Hash-Code suitable for use in hashtables.</returns>
		/// <remarks>Only considers the First, Middle, and Last names; ignores other fields.</remarks>
		public override int GetHashCode() {
			unchecked {
				int hash = 17;
				hash = hash * 23 + (First.HasValue() ? First.GetHashCode() : 0);
				hash = hash * 23 + (Last.HasValue() ? Last.GetHashCode() : 0);
				hash = hash * 23 + (Middle.HasValue() ? Middle.GetHashCode() : 0);
				return hash;
			}
		}
	}
}