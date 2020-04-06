using System;
using System.Collections.Generic;

namespace Xrd.Text.DiffMatchPatch {
	/// <summary>
	/// Matching extension methods
	/// </summary>
	/// <remarks>
	/// Adapted from https://github.com/google/diff-match-patch 
	/// Original algorith used an instance of a class, but this library converts all public members to extension methods. 
	/// Class properties were encapsulated into the *optional* MatchOptions class.
	/// </remarks>
	public static class MatchExtensions {
		/// <summary>
		///  Locate the best instance of 'pattern' in 'text' near 'loc'.
		/// </summary>
		/// <remarks>Returns -1 if no match found.</remarks>
		/// <param name="text">The text to search.</param>
		/// <param name="pattern">The pattern to search for.</param>
		/// <param name="loc">The location to search around.</param>
		/// <param name="fuzzyOptions">Options to be used if the match is forced to use the Bitmap algorithm.</param>
		/// <returns>Best match index or -1 for not found.</returns>
		public static int Match(this string text, string pattern, int loc, MatchOptions fuzzyOptions = default) {
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
				return -1;

			loc = Math.Max(0, Math.Min(loc, text.Length));
			if (text == pattern) {
				// shortcut (potentially not guaranteed by the Bitmap algorithm).
				return 0;
			} else if (loc + pattern.Length <= text.Length && text.Substring(loc, pattern.Length) == pattern) {
				// Perfect match at the perfect spot!
				return loc;
			} else {
				// do a fuzzy compare
				return text.Match_Bitmap(pattern, loc, fuzzyOptions);
			}
		}

		/// <summary>
		/// Locate the best instance of 'pattern' in 'text' near 'loc' using the Bitmap algorithm.
		/// </summary>
		/// <remarks>Returns -1 if no match found.</remarks>
		/// <param name="text">The text to search.</param>
		/// <param name="pattern">The pattern to search for.</param>
		/// <param name="loc">The location to search around.</param>
		/// <param name="fuzzyOptions">Options used to refine the 'fuzzy' match.</param>
		/// <returns>Best match index or -1 if not found.</returns>
		public static int Match_Bitmap(this string text, string pattern, int loc, MatchOptions fuzzyOptions = default) {
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
				return -1;

			// Initialize the alphabet.
			Dictionary<char, int> s = GetAlphabet(pattern);

			// Highest score beyond which we give up.
			if (fuzzyOptions == null)
				fuzzyOptions = new MatchOptions();

			double scoreThreshold = fuzzyOptions.FuzzyThreshold;

			// Is there a nearby exact match? (speedup)
			int bestLoc = text.IndexOf(pattern, loc, StringComparison.Ordinal);
			if (bestLoc != -1) {
				scoreThreshold = Math.Min(BitmapScore(0, bestLoc, loc, pattern, fuzzyOptions.FuzzyDistance), scoreThreshold);
				// What about the other direction? (speedup)
				bestLoc = text.LastIndexOf(pattern, Math.Min(loc + pattern.Length, text.Length), StringComparison.Ordinal);
				if (bestLoc != -1)
					scoreThreshold = Math.Min(BitmapScore(0, bestLoc, loc, pattern, fuzzyOptions.FuzzyDistance), scoreThreshold);
			}

			// Intitialize the bit arrays.
			int matchMask = 1 << (pattern.Length - 1);
			bestLoc = -1;

			int binMin, binMid;
			int binMax = pattern.Length + text.Length;

			int[] lastRd = new int[0];
			for (int d = 0; d < pattern.Length; d++) {
				// Scan for the best match; each iteration allows for one more error.
				// Run a binary search to determin how far from 'loc' we can stray at this error level.
				binMin = 0;
				binMid = binMax;
				while (binMin < binMid) {
					if (BitmapScore(d, loc + binMid, loc, pattern, fuzzyOptions.FuzzyDistance) <= scoreThreshold)
						binMin = binMid;
					else
						binMax = binMid;
					binMid = (binMax - binMin) / 2 + binMin;
				}
				// Use the result from this iteration as the maximum for the next.
				binMax = binMid;
				int start = Math.Max(1, loc - binMid + 1);
				int finish = Math.Min(loc + binMid, text.Length) + pattern.Length;

				int[] rd = new int[finish + 2];
				rd[finish + 1] = (1 << d) - 1;
				for (int j = finish; j >= start; j--) {
					int charMatch;
					if (text.Length <= j - 1 || !s.ContainsKey(text[j - 1])) {
						// out of range.
						charMatch = 0;
					} else
						charMatch = s[text[j - 1]];

					if (d == 0) {
						// first pass: exact match.
						rd[j] = ((rd[j + 1] << 1) | 1) & charMatch;
					} else {
						// subsequent passes: fuzzy match.
						rd[j] = ((rd[j + 1] << 1) | 1) & charMatch | (((lastRd[j + 1] | lastRd[j]) << 1) | 1) | lastRd[j + 1];
					}

					if ((rd[j] & matchMask) != 0) {
						double score = BitmapScore(d, j - 1, loc, pattern, fuzzyOptions.FuzzyDistance);
						// This match will almost certainly be better than any existing match, but check anyway.
						if (score <= scoreThreshold) {
							// Told you so.
							scoreThreshold = score;
							bestLoc = j - 1;
							if (bestLoc > loc) {
								// When passing loc, don't exceed out current distance from loc.
								start = Math.Max(1, 2 * loc - bestLoc);
							} else {
								// already passed loc, downhill from here.
								break;
							}
						}
					}
				}
				if (BitmapScore(d + 1, loc, loc, pattern, fuzzyOptions.FuzzyDistance) > scoreThreshold) {
					// No hope for a (better) match at greater error levels.
					break;
				}
				lastRd = rd;
			}
			return bestLoc;
		}

		/// <summary>
		/// Initialize the alphabet for the Bitmap algorithm
		/// </summary>
		/// <param name="pattern">The text to encode.</param>
		/// <returns>Hash of character locations.</returns>
		private static Dictionary<char, int> GetAlphabet(this string pattern) {
			Dictionary<char, int> s = new Dictionary<char, int>();
			char[] vs = pattern.ToCharArray();
			foreach (char c in vs) {
				if (!s.ContainsKey(c))
					s.Add(c, 0);
			}
			int i = 0;
			foreach (char c in vs) {
				int value = s[c] | (1 << (pattern.Length - i - 1));
				s[c] = value;
				i++;
			}
			return s;
		}

		/// <summary>
		/// Compute and return the score for a match with e errors and x location.
		/// </summary>
		/// <param name="e">Number of errors in match.</param>
		/// <param name="x">Location of match</param>
		/// <param name="loc">Expected location of match</param>
		/// <param name="pattern">Pattern being sought.</param>
		/// <param name="distance">How far to search for a match (0 = exact location, 1000+ = broad match)
		/// A match this many characters away from the expected location will add 1.0 to the score (0.0 is a perfect match).
		/// Default = 1000
		/// </param>
		/// <returns>Overall score for match (0.0 = good, 1.0 = bad)</returns>
		private static double BitmapScore(int e, int x, int loc, string pattern, int distance) {
			float accuracy = (float)e / pattern.Length;
			int proximity = Math.Abs(loc - x);
			if (distance == 0) {
				// Dodge divide by zero error.
				return proximity == 0 ? accuracy : 1.0;
			}
			return accuracy + (proximity / (float)distance);
		}
	}
}