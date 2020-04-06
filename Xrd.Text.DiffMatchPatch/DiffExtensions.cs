using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using Xrd.Collections;

namespace Xrd.Text.DiffMatchPatch {
	/// <summary>
	/// Diff-ing Extension Methods
	/// </summary>
	/// <remarks>
	/// Adapted from https://github.com/google/diff-match-patch 
	/// Original algorith used an instance of a class, but this library converts all public members to extension methods. 
	/// Class properties were encapsulated into *optional* arguments.
	/// </remarks>
	public static class DiffExtensions {
		/// <summary>
		/// Find the differences between two texts.
		/// </summary>
		/// <param name="original">Old string to be diffed.</param>
		/// <param name="current">New string to be diffed</param>
		/// <param name="checkLines">Speedup flag
		/// If false, then don't run a line-level diff first to identify change areas.
		/// If try, then run a faster, slightly less optimal diff.</param>
		/// <param name="timeout">Number of seconds to process a diff before giving up (0 = infinity)</param>
		/// <returns>List of Diff objects.</returns>
		public static List<DiffOp> GetDiffs(this string original, string current, bool checkLines = true, float timeout = 1.0f) {
			// Set a deadline by which time the diff must complete.
			DateTime deadline;
			if (timeout <= 0)
				deadline = DateTime.MaxValue;
			else
				deadline = DateTime.Now + new TimeSpan(((long)(timeout * 1000)) * 10000);

			return GetDiffs(original, current, checkLines, deadline);
		}

		/// <summary>
		/// Find the differences between two texts.
		/// </summary>
		/// <param name="text1">Old string to be diffed.</param>
		/// <param name="text2">New string to be diffed.</param>
		/// <param name="checkLines">Speedup flag
		/// If false, then don't run a line-level diff first to identify change areas.
		/// If try, then run a faster, slightly less optimal diff.</param>
		/// <param name="deadline">Time when the diff should be completed by.
		/// Used internally for recursive calls. Uses should set the diff timeout instead.</param>
		/// <returns></returns>
		internal static List<DiffOp> GetDiffs(this string text1, string text2, bool checkLines, DateTime deadline) {
			// Check for equality (speedup)
			List<DiffOp> diffs;
			if (text1 == text2) {
				diffs = new List<DiffOp>();
				if (!string.IsNullOrEmpty(text1))
					diffs.Add(new DiffOp(Operation.EQUAL, text1));
				return diffs;
			}

			// Trim off common prefix (speedup)
			int commonLength = text1.CommonPrefixLength(text2);
			string commonPrefix = text1.Substring(0, commonLength);
			text1 = text1.Substring(commonLength);
			text2 = text2.Substring(commonLength);

			// Trim off common suffix (speedup)
			commonLength = text1.CommonSuffixLength(text2);
			string commonSuffix = text1.Substring(text1.Length - commonLength);
			text1 = text1.Substring(0, text1.Length - commonLength);
			text2 = text2.Substring(0, text2.Length - commonLength);

			// Compute the diff on the middle block
			diffs = Compute(text1, text2, checkLines, deadline);

			// Restore the prefix and suffix
			if (!string.IsNullOrEmpty(commonPrefix))
				diffs.Insert(0, new DiffOp(Operation.EQUAL, commonPrefix));
			if (!string.IsNullOrEmpty(commonSuffix))
				diffs.Add(new DiffOp(Operation.EQUAL, commonSuffix));

			diffs.CleanupMerge();
			return diffs;
		}

		/// <summary>
		/// Crush the diff into an encoded string which describes the operations required to transform the source text into the result text.
		/// </summary>
		/// <param name="diffs">List of Diff objects</param>
		/// <returns>Delta text</returns>
		/// <example>
		/// =3\t-2\t+ing => Keep 3 chars, delete 2 chars, insert 'ing'.
		/// </example>
		/// <remarks>
		/// Operations are tab-separated. 
		/// Inserted text is escaped using %xx notation.
		/// </remarks>
		public static string ToDelta(this List<DiffOp> diffs) {
			if (diffs == null || diffs.Count < 1)
				return string.Empty;

			StringBuilder sb = new StringBuilder();
			foreach (DiffOp aDiff in diffs) {
				switch (aDiff.Operation) {
					case Operation.INSERT: {
							sb.Append("+");
							sb.Append( aDiff.Text.EncodeURI());
							break;
						}
					case Operation.DELETE: {
							sb.Append("-");
							sb.Append(aDiff.Text);
							break;
						}
					default: {
							sb.Append("=");
							sb.Append(aDiff.Text);
							break;
						}
				}
				sb.Append("\t");
			}

			string delta = sb.ToString();
			if (delta.Length > 0) {
				// strip the trailing tab character.
				delta = delta.Substring(0, delta.Length - 1);
			}
			return delta;
		}

		/// <summary>
		/// Given the original text, and an encoded string which describes the operations required to transfer
		/// OriginalText into CurrentText, compute the full diff.
		/// </summary>
		/// <param name="sourceText">Source string for the diff</param>
		/// <param name="delta">Delta text (encoded)</param>
		/// <returns>List of Diff objects, or null if invalid</returns>
		/// <exception cref="ArgumentException">If invalid delta input.</exception>
		public static List<DiffOp> FromDelta(this string sourceText, string delta) {
			List<DiffOp> res = new List<DiffOp>();
			// Cursor in text.
			int pos = 0;
			string[] tokens = delta.Split(new string[] { "\t" }, StringSplitOptions.None);
			foreach (string token in tokens) {
				if (token.Length == 0) {
					// Blank tokens are ok (from a trailing \t).
					continue;
				}
				// Each token begins with a one charater parameter which specifies the operation of the token (DEL, INS, EQ)
				string param = token.Substring(1);
				switch (token[0]) {
					case '+': { // Insert
								// decode would change all "+" to " "
							param = param.Replace("+", "%2b");
							param = Uri.UnescapeDataString(param);
							res.Add(new DiffOp(Operation.INSERT, param));
							break;
						}
					case '-': // Delete, fall through
					case '=': {
							int n;
							try { n = Convert.ToInt32(param); } catch (FormatException e) {
								throw new ArgumentException($"Invalid number in FromDelta: {param}", nameof(delta), e);
							}
							if (n < 0)
								throw new ArgumentException($"Negative number in FromDelta: {param}", nameof(delta));
							string text;
							try {
								text = sourceText.Substring(pos, n);
								pos += n;
							} catch (ArgumentOutOfRangeException e) {
								throw new ArgumentException($"Delta length ({pos}) larger than source text length ({sourceText.Length}).", nameof(delta), e);
							}
							if (token[0] == '=')
								res.Add(new DiffOp(Operation.EQUAL, text));
							else
								res.Add(new DiffOp(Operation.DELETE, text));
							break;
						}
					default: {
							// Anything else is an error.
							throw new ArgumentException($"Invalid diff operation in FromDelta: {token[0]}", nameof(delta));
						}
				}
			}
			if (pos != sourceText.Length)
				throw new ArgumentException($"Delta length ({pos}) smaller than source text length ({sourceText.Length}.", nameof(delta));
			return res;
		}

		/// <summary>
		/// Compute and return the original (source) text (all equalities and deletions).
		/// </summary>
		/// <param name="diffs">List of Diff objects.</param>
		/// <returns>Source text.</returns>
		public static string OriginalText(this List<DiffOp> diffs) {
			StringBuilder text = new StringBuilder();
			foreach (DiffOp aDiff in diffs) {
				if (aDiff.Operation != Operation.INSERT)
					text.Append(aDiff.Text);
			}
			return text.ToString();
		}

		/// <summary>
		/// Compute and return the current (result) text (all equalities and insertions).
		/// </summary>
		/// <param name="diffs">List of Diff objects.</param>
		/// <returns>Result text.</returns>
		public static string CurrentText(this List<DiffOp> diffs) {
			StringBuilder text = new StringBuilder();
			foreach (DiffOp aDiff in diffs) {
				if (aDiff.Operation != Operation.DELETE)
					text.Append(aDiff.Text);
			}
			return text.ToString();
		}

		/// <summary>
		/// Compute the Levenshtein distance; the number of inserted, deleted, or substituted characters.
		/// </summary>
		/// <param name="diffs">List of Diff objects.</param>
		/// <returns>Number of changes.</returns>
		public static int Levenshtein(this List<DiffOp> diffs) {
			int levenshtein = 0;
			int cIns = 0;
			int cDel = 0;
			foreach (DiffOp aDiff in diffs) {
				switch (aDiff.Operation) {
					case Operation.INSERT:
						cIns += aDiff.Text.Length;
						break;
					case Operation.DELETE:
						cDel += aDiff.Text.Length;
						break;
					default: {
							// A deletion and an insertion is one substitution.
							levenshtein += Math.Max(cIns, cDel);
							cIns = 0;
							cDel = 0;
							break;
						}
				}
			}
			levenshtein += Math.Max(cIns, cDel);
			return levenshtein;
		}

		/// <summary>
		/// Convert a Diff list into a pretty HTML report.
		/// </summary>
		/// <param name="diffs">List of Diff objects.</param>
		/// <returns>HTML representation</returns>
		public static string PrettyHtml(this List<DiffOp> diffs) {
			StringBuilder html = new StringBuilder();
			foreach (DiffOp aDiff in diffs) {
				string text = aDiff.Text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "&para;<br>");
				switch (aDiff.Operation) {
					case Operation.INSERT: {
							html.Append("<ins style=\"background:#e6ffe6;\">");
							html.Append(text);
							html.Append("</ins>");
							break;
						}
					case Operation.DELETE: {
							html.Append("<del style=\"background:#ffe6e6;\">");
							html.Append(text);
							html.Append("</del>");
							break;
						}
					default: {
							html.Append("<span>");
							html.Append(text);
							html.Append("</span>");
							break;
						}
				}
			}
			return html.ToString();
		}

		/// <summary>
		/// Given the provided diffs, and origLoc as an index in the originalText, compute and return the equivalent location in the currentText.
		/// </summary>
		/// <param name="diffs">List of DiffOp objects defining a diff</param>
		/// <param name="origLoc">The index in the diff's original text.</param>
		/// <returns>The equivalent index in the diff's current (resulting) text.</returns>
		public static int CrossIndex(this List<DiffOp> diffs, int origLoc) {
			int c1 = 0;
			int c2 = 0;
			int lC1 = 0;
			int lC2 = 0;
			DiffOp lastDiff = null;
			foreach (var aDiff in diffs) {
				if (aDiff.Operation != Operation.INSERT) {
					// Equality or deletion.
					c1 += aDiff.Text.Length;
				}
				if (aDiff.Operation != Operation.DELETE) {
					// Equality or insertion
					c2 += aDiff.Text.Length;
				}

				if (c1 > origLoc) {
					// overshot the location.
					lastDiff = aDiff;
					break;
				}
				lC1 = c1;
				lC2 = c2;
			}
			if (lastDiff != null && lastDiff.Operation == Operation.DELETE) {
				// The location was deleted.
				return lC2;
			}
			// Add the remaining character length.
			return lC2 + (origLoc - lC1);
		}

		#region CleanUp Methods
		/// <summary>
		/// Reorder and merge like edit sections. Merge equalities.
		/// </summary>
		/// <remarks>Any edit section can move as long as it doesn't cross an equality.</remarks>
		/// <param name="diffs">List of Diff objects</param>
		internal static void CleanupMerge(this List<DiffOp> diffs) {
			// Add a dummy entry at the end.
			diffs.Add(new DiffOp(Operation.EQUAL, string.Empty));
			int pos = 0;
			int cDel = 0;
			int cIns = 0;
			string sDel = string.Empty;
			string sIns = string.Empty;
			int lCommon;

			while (pos < diffs.Count) {
				switch (diffs[pos].Operation) {
					case Operation.INSERT: {
							cIns++;
							sIns += diffs[pos].Text;
							pos++;
							break;
						}
					case Operation.DELETE: {
							cDel++;
							sDel += diffs[pos].Text;
							pos++;
							break;
						}
					default: {
							// Upon reaching an equality, check for prior redunancies.
							if (cDel + cIns > 1) {
								if (cDel != 0 && cIns != 0) {
									// Factor out any common prefixes.
									lCommon = sIns.CommonPrefixLength(sDel);
									if (lCommon != 0) {
										if ((pos - cDel - cIns) > 0 && diffs[pos - cDel - cIns - 1].Operation == Operation.EQUAL) {
											diffs[pos - cDel - cIns - 1].Text += sIns.Substring(0, lCommon);
										} else {
											diffs.Insert(0, new DiffOp(Operation.EQUAL, sIns.Substring(0, lCommon)));
											pos++;
										}
										sIns = sIns.Substring(lCommon);
										sDel = sDel.Substring(lCommon);
									}
									// Factor out any common suffixes.
									lCommon = sIns.CommonSuffixLength(sDel);
									if (lCommon != 0) {
										diffs[pos].Text = sIns.Substring(sIns.Length - lCommon) + diffs[pos].Text;
										sIns = sIns.Substring(0, sIns.Length - lCommon);
										sDel = sDel.Substring(0, sDel.Length - lCommon);
									}
								}
								// Delete the offending records and add the merged ones.
								pos -= cDel + cIns;
								diffs.Splice(pos, cDel + cIns);
								if (sDel.Length != 0) {
									diffs.Splice(pos, 0, new DiffOp(Operation.DELETE, sDel));
									pos++;
								}
								if (sIns.Length != 0) {
									diffs.Splice(pos, 0, new DiffOp(Operation.INSERT, sIns));
									pos++;
								}
								pos++;
							} else if (pos != 0 && diffs[pos - 1].Operation == Operation.EQUAL) {
								// Merge this equality with the previous one.
								diffs[pos - 1].Text += diffs[pos].Text;
								diffs.RemoveAt(pos);
							} else {
								pos++;
							}
							cIns = 0;
							cDel = 0;
							sDel = string.Empty;
							sIns = string.Empty;
							break;
						}
				}
			}
			if (diffs[diffs.Count - 1].Text.Length == 0) {
				// Remove the dummy entry at the end.
				diffs.RemoveAt(diffs.Count - 1);
			}

			/* Second pass: look for single edits surrounded on both side by 
			 * equalities that can be shifted sideways to eliminate an equality.
			 * e.g.: A<ins>B</ins>C => <ins>AB></ins>AC*/
			bool changes = false;
			pos = 1;
			// Intentionally ignore the first and last element (don't need checking).
			while (pos < (diffs.Count - 1)) {
				if (diffs[pos - 1].Operation == Operation.EQUAL && diffs[pos + 1].Operation == Operation.EQUAL) {
					// This is a single edit surrounded by equalities.
					if (diffs[pos].Text.EndsWith(diffs[pos - 1].Text, StringComparison.Ordinal)) {
						// Shift the edit over the previous equality.
						diffs[pos].Text = diffs[pos - 1].Text + diffs[pos].Text.Substring(0, diffs[pos].Text.Length - diffs[pos - 1].Text.Length);
						diffs[pos + 1].Text = diffs[pos - 1].Text + diffs[pos + 1].Text;
						diffs.Splice(pos - 1, 1);
						changes = true;
					} else if (diffs[pos].Text.StartsWith(diffs[pos + 1].Text, StringComparison.Ordinal)) {
						// Shift the edit over the next equality.
						diffs[pos - 1].Text += diffs[pos + 1].Text;
						diffs[pos].Text = diffs[pos].Text.Substring(diffs[pos + 1].Text.Length) + diffs[pos + 1].Text;
						diffs.Splice(pos + 1, 1);
						changes = true;
					}
				}
				pos++;
			}
			// If shifts were made, the diff needs reordering and another shift sweep.
			if (changes)
				CleanupMerge(diffs);
		}

		/// <summary>
		/// Reduce the number of edits by eliminating operationally trivial equalities
		/// </summary>
		/// <param name="diffs">List of Diff objects</param>
		/// <param name="editCost">Cost of an empty edit operation in terms of characters.</param>
		public static void CleanupEfficiency(this List<DiffOp> diffs, short editCost = 4) {
			bool changes = false;

			//Stack of indices where equalities are found.
			Stack<int> equalities = new Stack<int>();
			// Always equal to equalities[equalitiesLength - 1].Text;
			string lastEquality = string.Empty;
			// Index of current position
			int pos = 0;
			// Is there an insertion operation before the last equality?
			bool preIns = false;
			// Is there a deletion operation before the last equality?
			bool preDel = false;
			// Is there an insertion operation after the last equality?
			bool postIns = false;
			// Is there a deletion operation after the last equality?
			bool postDel = false;

			while (pos < diffs.Count) {
				if (diffs[pos].Operation == Operation.EQUAL) {
					// Equality found
					if (diffs[pos].Text.Length <= editCost && (postIns || postDel)) {
						// Candidate found
						equalities.Push(pos);
						preIns = postIns;
						preDel = postDel;
						lastEquality = diffs[pos].Text;
					} else {
						// Not a candidate, and can never become one.
						equalities.Clear();
						lastEquality = string.Empty;
					}
					postIns = false;
					postDel = false;
				} else {
					// An insertion or deletion.
					if (diffs[pos].Operation == Operation.DELETE)
						postDel = true;
					else
						postIns = true;
					/* Five types to be split:
					 * <ins>A</ins><del>B</del>XY<ins>C</ins></del>D</del>
					 * <ins>A</ins>X<ins>C</ins><del>D</del>
					 * <ins>A</ins><del>B</del>X<ins>C</ins>
					 * <ins>A</ins>X<ins>C</ins><del>D</del>
					 * <ins>A</ins><del>B</del>X<del>C</del>*/
					if ((lastEquality.Length != 0)
					   && ((preIns && preDel && postIns && postDel)
					   || ((lastEquality.Length < editCost / 2)
					   && ((preIns ? 1 : 0) | (preDel ? 1 : 0) + (postIns ? 1 : 0) + (postDel ? 1 : 0)) == 3))) {
						// Duplicate record
						diffs.Insert(equalities.Peek(), new DiffOp(Operation.DELETE, lastEquality));
						// Change second copy to insert.
						diffs[equalities.Peek() + 1].Operation = Operation.INSERT;
						equalities.Pop(); // Throw away the equality we just deleted.
						lastEquality = string.Empty;
						if (preIns && preDel) {
							// No changes made which could affect previous entry, keep going.
							postIns = true;
							postDel = true;
							equalities.Clear();
						} else {
							if (equalities.Count > 0)
								equalities.Pop();
							pos = equalities.Count > 0 ? equalities.Peek() : -1;
							postIns = false;
							postDel = false;
						}
						changes = true;
					}
				}
				pos++;
			}

			if (changes)
				diffs.CleanupMerge();
		}

		/// <summary>
		/// Reduce the number of edits by eliminating semantically trivial equalities.
		/// </summary>
		/// <param name="diffs">List of Diff objects</param>
		public static void CleanupSemantic(this List<DiffOp> diffs) {
			bool changes = false;

			// Stack of indices where equalities are found.
			Stack<int> equalities = new Stack<int>();
			// Always equal to equalities[equalitiesLength-1].Text;
			string lastEquality = null;
			// Index of current position.
			int ptr = 0;
			// Number of characters that changed prior to the equality.
			int lIns1 = 0;
			int lDel1 = 0;
			// Number of characters that changed after the equality.
			int lIns2 = 0;
			int lDel2 = 0;

			while (ptr < diffs.Count) {
				if (diffs[ptr].Operation == Operation.EQUAL) { // Equality found.
					equalities.Push(ptr);
					lIns1 = lIns2;
					lDel1 = lDel2;
					lIns2 = 0;
					lDel2 = 0;
					lastEquality = diffs[ptr].Text;
				} else { // an insertion or deletion.
					if (diffs[ptr].Operation == Operation.INSERT)
						lIns2 += diffs[ptr].Text.Length;
					else
						lDel2 += diffs[ptr].Text.Length;

					// Eliminate an equality that is smaller or equal to the edits on both sides of it.
					if (lastEquality != null && (lastEquality.Length <= Math.Max(lIns1, lDel1)) && (lastEquality.Length <= Math.Max(lIns2, lDel2))) {
						// Duplicate record.
						diffs.Insert(equalities.Peek(), new DiffOp(Operation.DELETE, lastEquality));
						// Change second copy to insert.
						diffs[equalities.Peek() + 1].Operation = Operation.INSERT;
						// Throw away the equality we just deleted.
						equalities.Pop();
						if (equalities.Count > 0)
							equalities.Pop();

						ptr = equalities.Count > 0 ? equalities.Pop() : -1;
						// reset the counters.
						lIns1 = 0;
						lIns2 = 0;
						lDel1 = 0;
						lDel2 = 0;
						lastEquality = null;
						changes = true;
					}
				}
				ptr++;
			}

			// Normalize the diff.
			if (changes)
				diffs.CleanupMerge();
			diffs.CleanupSemanticLossless();

			/* Find any overlaps between deletions and insertions.
			 * e.g. <del>abcxxx</del><ins>xxxdef</ins>
			 *	=>	<del>abc</del>xxx<ins>def</ins>
			 * e.g. <del>xxxabc</del><ins>defxxx</ins>
			 *  =>	<ins>def</ins>xxx<del>abc<del>
			 * Only extract an overlap if it is as big as the edit ahead or behind it.*/
			ptr = 1;
			while (ptr < diffs.Count) {
				if (diffs[ptr - 1].Operation == Operation.DELETE && diffs[ptr].Operation == Operation.INSERT) {
					string deletion = diffs[ptr - 1].Text;
					string insertion = diffs[ptr].Text;
					int olapLen1 = deletion.CommonOverlap(insertion);
					int olapLen2 = insertion.CommonOverlap(deletion);
					if (olapLen1 >= olapLen2) {
						if (olapLen1 >= deletion.Length / 2.0 || olapLen1 >= insertion.Length / 2.0) {
							// Overlap found.
							// Insert an equality and trim the surrounding edits.
							diffs.Insert(ptr, new DiffOp(Operation.EQUAL, insertion.Substring(0, olapLen1)));
							diffs[ptr - 1].Text = deletion.Substring(0, deletion.Length - olapLen1);
							diffs[ptr + 1].Text = insertion.Substring(olapLen1);
							ptr++;
						}
					} else {
						if (olapLen2 >= deletion.Length / 2.0 || olapLen2 >= insertion.Length / 2.0) {
							// Reverse overlap found.
							// Insert an equality and swap and trim the surrounding edits.
							diffs.Insert(ptr, new DiffOp(Operation.EQUAL, deletion.Substring(0, olapLen2)));
							diffs[ptr - 1].Operation = Operation.INSERT;
							diffs[ptr - 1].Text = insertion.Substring(0, insertion.Length - olapLen2);
							diffs[ptr + 1].Operation = Operation.DELETE;
							diffs[ptr + 1].Text = deletion.Substring(olapLen2);
							ptr++;
						}
					}
					ptr++;
				}
				ptr++;
			}
		}

		/// <summary>
		/// Look for single edits surrounded on both sides by equalities which can 
		/// be shifted sideways to align the edit to a word boundary. 
		/// </summary>
		/// <example>
		/// The c<ins>at c</ins>ame. => The <ins>cat </ins>came.
		/// </example>
		/// <param name="diffs">List of Diff objects</param>
		internal static void CleanupSemanticLossless(this List<DiffOp> diffs) {
			int pos = 1;
			// Intentionally ignore the first and last elements (don't need checking).
			while (pos < diffs.Count - 1) {
				if (diffs[pos - 1].Operation == Operation.EQUAL && diffs[pos + 1].Operation == Operation.EQUAL) {
					// This is a single edit surrounded by equalities.
					string equality1 = diffs[pos - 1].Text;
					string edit = diffs[pos].Text;
					string equality2 = diffs[pos + 1].Text;

					// First, shift the edit as far left as possible.
					int commonOffset = equality1.CommonSuffixLength(edit);
					if (commonOffset > 0) {
						string commonStr = edit.Substring(edit.Length - commonOffset);
						equality1 = equality1.Substring(0, equality1.Length - commonOffset);
						edit = commonStr + edit.Substring(0, edit.Length - commonOffset);
						equality2 = commonStr + equality2;
					}

					// Second, step character by character right, looking for the best fit.
					string bestEqu1 = equality1;
					string bestEdit = edit;
					string bestEqa2 = equality2;
					int bestScore = CleanupSemanticScore(equality1, edit) + CleanupSemanticScore(edit, equality2);
					while (edit.Length != 0 && equality2.Length != 0 && edit[0] == equality2[0]) {
						equality1 += edit[0];
						edit = edit.Substring(1) + equality2[0];
						equality2 = equality2.Substring(1);
						int score = CleanupSemanticScore(equality1, edit) + CleanupSemanticScore(edit, equality2);
						// The >= encourages trailing rather than leading whitespace on edits.
						if (score >= bestScore) {
							bestScore = score;
							bestEqu1 = equality1;
							bestEdit = edit;
							bestEqa2 = equality2;
						}
					}

					if (diffs[pos - 1].Text != bestEqu1) {
						// We have an improvement, save it back to the diff.
						if (bestEqu1.Length != 0) {
							diffs[pos - 1].Text = bestEqu1;
						} else {
							diffs.RemoveAt(pos - 1);
							pos--;
						}
						diffs[pos].Text = bestEdit;
						if (bestEqa2.Length != 0) {
							diffs[pos + 1].Text = bestEqa2;
						} else {
							diffs.RemoveAt(pos + 1);
							pos--;
						}
					}
				}
				pos++;
			}
		}

		//Define some regex patterns for matching boundaries.
		private static readonly Regex BLANKLINEEND = new Regex("\\n\\r?\\n\\Z");
		private static readonly Regex BLANKLINESTART = new Regex("\\A\\r?\\n\\r?\\n");

		/// <summary>
		/// Given two strings, compute a score representing wheter the internal boundary falls on logical boundaries.
		/// </summary>
		/// <remarks>Scores range from 6 (best) to 0 (worst).</remarks>
		/// <param name="one">First string.</param>
		/// <param name="two">Second string</param>
		/// <returns>The score (6 = best -> 0 = worst)</returns>
		internal static int CleanupSemanticScore(string one, string two) {
			if (one.Length == 0 || two.Length == 0)
				return 6;   // Edges are best.

			char c1 = one[one.Length - 1];
			char c2 = two[0];
			bool nonAlpha1 = !char.IsLetterOrDigit(c1);
			bool nonAlpha2 = !char.IsLetterOrDigit(c2);
			bool wsp1 = nonAlpha1 && char.IsWhiteSpace(c1);
			bool wsp2 = nonAlpha2 && char.IsWhiteSpace(c2);
			bool lb1 = wsp1 && char.IsControl(c1);
			bool lb2 = wsp2 && char.IsControl(c2);
			bool blk1 = lb1 && BLANKLINEEND.IsMatch(one);
			bool blk2 = lb2 && BLANKLINESTART.IsMatch(two);

			if (blk1 || blk2)
				return 5; // 5 points for blank lines.
			else if (lb1 || lb2)
				return 4; // 4 points for line breaks.
			else if (nonAlpha1 && !wsp1 && wsp2)
				return 3; // 3 points for end of sentences.
			else if (wsp1 || wsp2)
				return 2; // 2 points for whitespace
			else if (nonAlpha1 || nonAlpha2)
				return 1; // 1 point for non-alphanumeric
			else
				return 0;
		}
		#endregion

		#region private Methods
		/// <summary>
		/// Do a quick line-level diff on both strings. Then re-diff the parts for greater accuracy.
		/// </summary>
		/// <remarks>This speedup can produce non-minimal diffs.</remarks>
		/// <param name="text1">Old string to be diffed</param>
		/// <param name="text2">New string to be diffed</param>
		/// <param name="deadline">Time when the diff should be completed by</param>
		/// <returns>List of Diff objects</returns>
		private static List<DiffOp> LineMode(string text1, string text2, DateTime deadline) {
			// Scan the text on a lin-by-line basis first.
			Tuple<string, string, List<string>> tuple = text1.LinesToChars(text2);
			text1 = tuple.Item1;
			text2 = tuple.Item2;
			List<string> lineArray = tuple.Item3;

			List<DiffOp> res = GetDiffs(text1, text2, false, deadline);

			// Conver the diff back to original text.
			res.CharsToLines(lineArray);
			// Eliminate the freak matches (e.g. blank lines).
			res.CleanupSemantic();

			// Rediff any replacement blocks, this time character-by-character.
			// Add a dummy entry at the end.
			res.Add(new DiffOp(Operation.EQUAL, string.Empty));
			int ptr = 0;
			int cDel = 0;
			int cIns = 0;
			string sDel = string.Empty;
			string sIns = string.Empty;
			while (ptr < res.Count) {
				switch (res[ptr].Operation) {
					case Operation.INSERT: {
							cIns++;
							sIns += res[ptr].Text;
							break;
						}
					case Operation.DELETE: {
							cDel++;
							sDel += res[ptr].Text;
							break;
						}
					default: {
							//upon reaching an equality, check for prior redundancies
							if (cDel >= 1 && cIns >= 1) {
								// Delete the offending records and add the merged ones.
								res.RemoveRange(ptr - cDel - cIns, cDel + cIns);
								ptr = ptr - cDel - cIns;
								List<DiffOp> subDiff = GetDiffs(sDel, sIns, false, deadline);
								res.InsertRange(ptr, subDiff);
								ptr += subDiff.Count;
							}
							cDel = 0;
							cIns = 0;
							sDel = string.Empty;
							sIns = string.Empty;
							break;
						}
				}
				ptr++;
			}
			// Remove the dummy entry at the end.
			res.RemoveAt(res.Count - 1);
			return res;
		}

		/// <summary>
		/// Given the location of the 'middle snake', split the diff in two parts and recurse.
		/// </summary>
		/// <param name="text1">Old string to be diffed.</param>
		/// <param name="text2">New STring to be diffed.</param>
		/// <param name="x">Index of split point in text1</param>
		/// <param name="y">Index of split point in text2.</param>
		/// <param name="deadline">Time at which to bail if not yet complete.</param>
		/// <returns>List of Diff objects.</returns>
		private static List<DiffOp> BisectSplit(string text1, string text2, int x, int y, DateTime deadline) {
			string t1a = text1.Substring(0, x);
			string t2a = text2.Substring(0, y);
			string t1b = text1.Substring(x);
			string t2b = text2.Substring(y);

			// Compute both diffs serially.
			List<DiffOp> da = GetDiffs(t1a, t2a, false, deadline);
			List<DiffOp> db = GetDiffs(t1b, t2b, false, deadline);

			da.AddRange(db);
			return da;
		}
		/// <summary>
		/// Find the 'middle snake' of a diff, split the problem in two and return the recursively constructed diff.
		/// </summary>
		/// <remarks>See Myers 1986 paper: An O(ND) Difference Algorithm and Its Variations.</remarks>
		/// <param name="text1">Old string to be diffed.</param>
		/// <param name="text2">New string to be diffed.</param>
		/// <param name="deadline">Time at which to bail if not yet complete.</param>
		/// <returns>List of Diff objects.</returns>
		private static List<DiffOp> Bisect(string text1, string text2, DateTime deadline) {
			// Store the text lengths to prevent multiple calls
			int len1 = text1.Length;
			int len2 = text2.Length;
			int maxD = (len1 + len2 + 1) / 2;
			int vOffset = maxD;
			int vLen = 2 * maxD;
			int[] v1 = new int[vLen];
			int[] v2 = new int[vLen];

			for (int x = 0; x < vLen; x++) {
				v1[x] = -1;
				v2[x] = -1;
			}

			v1[vOffset + 1] = 0;
			v2[vOffset + 1] = 0;
			int delta = len1 - len2;

			// If the total number of characters is odd, then the front path will collide with the reverse path.
			bool front = (delta % 2 != 0);

			// Offsets for start and end of k loop. Prevents mapping of space beyond the grid.
			int k1Start = 0;
			int k1End = 0;
			int k2Start = 0;
			int k2End = 0;

			for (int d = 0; d < maxD; d++) {
				//Bail out if deadline is reached.
				if (DateTime.Now > deadline)
					break;

				// Walk the front path one step.
				for (int k1 = -d + k1Start; k1 <= d - k1End; k1 += 2) {
					int k1Offset = vOffset + k1;
					int x1;
					if (k1 == -d || k1 != d && v1[k1Offset - 1] < v1[k1Offset + 1])
						x1 = v1[k1Offset + 1];
					else
						x1 = v1[k1Offset - 1] + 1;

					int y1 = x1 - k1;
					while (x1 < len1 && y1 < len2 && text1[x1] == text2[y1]) {
						x1++;
						y1++;
					}

					v1[k1Offset] = x1;
					if (x1 > len1) {
						// Ran off the right of the graph.
						k1End += 2;
					} else if (y1 > len2) {
						// Ran off the bottom of the graph
						k1Start += 2;
					} else if (front) {
						int k2Offset = vOffset + delta - k1;
						if (k2Offset >= 0 && k2Offset < vLen && v2[k2Offset] != -1) {
							// Mirror x2 onto top-left coordinate system.
							int x2 = len1 - v2[k2Offset];
							if (x1 >= x2) {
								// overlap detected
								return BisectSplit(text1, text2, x1, y1, deadline);
							}
						}
					}
				}

				// Walk the revers path one step.
				for (int k2 = -d + k2Start; k2 <= d - k2End; k2 += 2) {
					int k2Offset = vOffset + k2;
					int x2;
					if (k2 == -d || k2 != d && v2[k2Offset - 1] < v2[k2Offset + 1])
						x2 = v2[k2Offset + 1];
					else
						x2 = v2[k2Offset - 1] + 1;

					int y2 = x2 - k2;
					while (x2 < len1 && y2 < len2 && text1[len1 - x2 - 1] == text2[len2 - y2 - 1]) {
						x2++;
						y2++;
					}

					v2[k2Offset] = x2;
					if (x2 > len1) {
						// ran off the left of the graph
						k2End += 2;
					} else if (y2 > len2) {
						// Ran off the top of the graph {
						k2Start += 2;
					} else if (!front) {
						int k1Offset = vOffset + delta - k2;
						if (k1Offset >= 0 && k1Offset < vLen && v1[k1Offset] != -1) {
							int x1 = v1[k1Offset];
							int y1 = vOffset + x1 - k1Offset;
							// Mirror x2 onto top-left coordinate system.
							x2 = len1 - v2[k2Offset];
							if (x1 >= x2) {
								//overlap detected.
								return BisectSplit(text1, text2, x1, y1, deadline);
							}
						}
					}
				}
			}

			// Diff took too long and hit the deadline or
			// number of diffs equal to number of characters, no commonality at all.
			return new List<DiffOp> {
				new DiffOp(Operation.DELETE, text1),
				new DiffOp(Operation.INSERT, text2)
			};
		}

		/// <summary>
		/// Does of Substring of shorttext exist within longtext such that the substring is at least half the length of longtext?
		/// </summary>
		/// <param name="lText">Longer string</param>
		/// <param name="sText">Shorter string</param>
		/// <param name="i">Start index of quarter length substring within longtext.</param>
		/// <returns>A HalfMatchResult structure</returns>
		private static HalfMatchResult halfMatch(string lText, string sText, int i) {
			// Start with a 1/4 length Substring at position i as a seed.
			string seed = lText.Substring(i, lText.Length / 4);
			int j = -1;
			string best_common = string.Empty;
			string best_longa = string.Empty, best_longb = string.Empty;
			string best_shorta = string.Empty, best_shortb = string.Empty;
			while (j < sText.Length && (j = sText.IndexOf(seed, j + 1, StringComparison.Ordinal)) != -1) {
				int lPref = lText.Substring(i).CommonPrefixLength(sText.Substring(j));
				int lSuf = lText.Substring(0, i).CommonSuffixLength(sText.Substring(0, j));
				if (best_common.Length < lSuf + lPref) {
					best_common = sText.Substring(j - lSuf, lSuf) + sText.Substring(j, lPref);
					best_longa = lText.Substring(0, i - lSuf);
					best_longb = lText.Substring(i + lPref);
					best_shorta = sText.Substring(0, j - lSuf);
					best_shortb = sText.Substring(j + lPref);
				}
			}
			if (best_common.Length * 2 >= lText.Length)
				return new HalfMatchResult(best_longa, best_longb, best_shorta, best_shortb, best_common);
			else
				return new HalfMatchResult();
		}

		/// <summary>
		/// Do the two texts share a substring which is at least half the length of the longer text?
		/// </summary>
		/// <remarks>This speedup can produce non-minimal diffs.</remarks>
		/// <param name="text1">First string</param>
		/// <param name="text2">Second string</param>
		/// <returns>A HalfMatchResult structure</returns>
		private static HalfMatchResult HalfMatch(string text1, string text2, DateTime deadline) {
			if (deadline == DateTime.MaxValue) {
				// Don't risk returned a non-optimal diff if we have unlimited time.
				return new HalfMatchResult();
			}
			string lText = text1.Length > text2.Length ? text1 : text2;
			string sText = text1.Length > text2.Length ? text2 : text1;
			if (lText.Length < 4 || sText.Length * 2 < lText.Length)
				return new HalfMatchResult(); // Pointless;

			// First check if the second quarter is the seed for a half-match;
			var hm1 = halfMatch(lText, sText, (lText.Length + 3) / 4);
			// Check again based on the third quarter.
			var hm2 = halfMatch(lText, sText, (lText.Length + 1) / 2);
			HalfMatchResult res;
			if (hm1.IsNull || hm2.IsNull)
				return new HalfMatchResult();
			else if (hm2.IsNull)
				res = hm1;
			else if (hm1.IsNull)
				res = hm2;
			else {
				// both matched, selected the longest.
				res = hm1.CommonMiddle.Length > hm2.CommonMiddle.Length ? hm1 : hm2;
			}

			// A half-match was found, sort out the return data
			if (text1.Length > text2.Length)
				return res;
			else
				return new HalfMatchResult(res.Prefix2, res.Suffix2, res.Prefix1, res.Suffix1, res.CommonMiddle);
		}

		/// <summary>
		/// Find the differences between two texts. Assumes that the texts for not have any common prefix or suffix.
		/// </summary>
		/// <param name="text1">Old string to be diffed.</param>
		/// <param name="text2">New string to be diffed.</param>
		/// <param name="checkLines">Speedup flag. If true, line level diff is first run to identify the changed areas. this is slightly faster, but a less optimal diff.</param>
		/// <param name="deadline">Time when the diff should be completed by.</param>
		/// <returns>List of DiffOf objects</returns>
		private static List<DiffOp> Compute(string text1, string text2, bool checkLines, DateTime deadline) {
			List<DiffOp> res = new List<DiffOp>();

			// Just added some text (speedup)
			if (string.IsNullOrEmpty(text1)) {
				res.Add(new DiffOp(Operation.INSERT, text2));
				return res;
			}

			// Just deleted some text (speedup)
			if (string.IsNullOrEmpty(text2)) {
				res.Add(new DiffOp(Operation.DELETE, text1));
				return res;
			}

			string longText = text1.Length > text2.Length ? text1 : text2;
			string shortText = text1.Length > text2.Length ? text2 : text1;

			// Check to see if the shorter text is inside the long text. (speedup);
			int i = longText.IndexOf(shortText, StringComparison.Ordinal);
			if (i != -1) {
				Operation op = (text1.Length > text2.Length) ? Operation.DELETE : Operation.INSERT;
				res.Add(new DiffOp(op, longText.Substring(0, i)));
				res.Add(new DiffOp(Operation.EQUAL, shortText));
				res.Add(new DiffOp(op, longText.Substring(i + shortText.Length)));
				return res;
			}

			// Single character string. After previous speedup, the character can't be an equality.
			if (shortText.Length == 1) {
				res.Add(new DiffOp(Operation.DELETE, text1));
				res.Add(new DiffOp(Operation.INSERT, text2));
				return res;
			}

			// Check to see if the problem can be split in two.
			HalfMatchResult hm = HalfMatch(text1, text2, deadline);
			// If a half-match was found, sort out the return data.
			if (!hm.IsNull) {
				// Send both pairs of diffs for separate processing.
				List<DiffOp> da = GetDiffs(hm.Prefix1, hm.Prefix2, checkLines, deadline);
				List<DiffOp> db = GetDiffs(hm.Suffix1, hm.Suffix2, checkLines, deadline);
				// Merge the results
				res = da;
				res.Add(new DiffOp(Operation.EQUAL, hm.CommonMiddle));
				res.AddRange(db);
				return res;
			}

			if (checkLines && text1.Length > 100 && text2.Length > 100)
				return LineMode(text1, text2, deadline);

			return Bisect(text1, text2, deadline);
		}
		#endregion

		#region Internal Methods
		/// <summary>
		/// Split two texts into a list of strings reduce the texts to a string of hashes where each Unicode character represents one line.
		/// </summary>
		/// <param name="text1">First string</param>
		/// <param name="text2">Second string</param>
		/// <returns> Tuple<string, string, List<string>:
		/// Item1 = The encoded text1
		/// Item2 = The encoded text2
		/// Item3 = The List of unique strings. (the zeroth element is intentionally blank)
		/// </returns>
		internal static Tuple<string, string, List<string>> LinesToChars(this string text1, string text2) {
			List<string> lineArray = new List<string>();
			Dictionary<string, int> lineHash = new Dictionary<string, int>();
			// e.g. lineArray[4] = "Hello\n";
			// e.g. lineHash["Hello\n") == 4;

			// "\x00" is a valid character, but various debuggers don't like it.
			// So, we'll insert a junk entry to avoid generating a null character.
			lineArray.Add(string.Empty);

			// Allocate 2/3rd of the space for text1, the rest for text2.
			string chars1 = LinesToCharsEncode(text1, lineArray, lineHash, 40000);
			string chars2 = LinesToCharsEncode(text2, lineArray, lineHash, 65535);
			return new Tuple<string, string, List<string>>(chars1, chars2, lineArray);

		}

		/// <summary>
		/// split a text into a list of strings. Reduce the texts to a string of hashes where each Unicode character represents one line.
		/// </summary>
		/// <param name="text">String to encode</param>
		/// <param name="lineArray">List of unique strings.</param>
		/// <param name="lineHash">Maximum length of line array</param>
		/// <param name="maxLines"></param>
		/// <returns></returns>
		internal static string LinesToCharsEncode(this string text, List<string> lineArray, Dictionary<string, int> lineHash, int maxLines) {
			int lineStart = 0;
			int lineEnd = -1;
			string line;
			StringBuilder chars = new StringBuilder();
			// Walk the text, pulling out a substring for each line.
			// text.Split('\n') would temporarily double our memory footprint.
			// Modifing text would create many large strings to garbage collect.
			while (lineEnd < text.Length - 1) {
				lineEnd = text.IndexOf('\n', lineStart);
				if (lineEnd == -1)
					lineEnd = text.Length - 1;
				line = text.JavaSubstring(lineStart, lineEnd + 1);

				if (lineHash.ContainsKey(line))
					chars.Append(((char)lineHash[line]));
				else {
					if (lineArray.Count == maxLines) {
						// Bail out at 65535 because char 65536 == char 0.
						line = text.Substring(lineStart);
						lineEnd = text.Length;
					}
					lineArray.Add(line);
					lineHash.Add(line, lineArray.Count - 1);
					chars.Append(((char)(lineArray.Count - 1)));
				}
				lineStart = lineEnd + 1;
			}
			return chars.ToString();
		}

		/// <summary>
		/// Rehydrate the text in a diff from a string of line hashes to real lines
		/// </summary>
		/// <param name="input">List of Diff objects</param>
		/// <param name="lineArray">List of unique strings.</param>
		internal static void CharsToLines(this ICollection<DiffOp> diffs, IList<string> lineArray) {
			StringBuilder text;
			foreach (DiffOp diff in diffs) {
				text = new StringBuilder();
				for (int j = 0; j < diff.Text.Length; j++) {
					text.Append(lineArray[diff.Text[j]]);
				}
				diff.Text = text.ToString();
			}
		}
		#endregion
	}
}