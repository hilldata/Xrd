using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xrd.Collections;

namespace Xrd.Text.DiffMatchPatch {
	/// <summary>
	/// Patching extension methods.
	/// </summary>
	/// <remarks>
	/// Adapted from https://github.com/google/diff-match-patch 
	/// Original algorith used an instance of a class, but this library converts all public members to extension methods. 
	/// Class properties were encapsulated into the *optional* PatchOptions class and optional method arguments.
	/// </remarks>
	public static class PatchExtensions {
		/// <summary>
		/// The number of bits in an int
		/// </summary>
		private const short MAX_BITS = 32;

		#region Private Methods
		/// <summary>
		/// Increase the context until it is unique, but don't let the pattern expand beyond MAX_BITS
		/// </summary>
		/// <param name="patch">The patch to grow</param>
		/// <param name="text">Source text</param>
		/// <param name="margin">Chunk size for context length.</param>
		private static void AddContext(this PatchOp patch, string text, short margin = 4) {
			if (string.IsNullOrEmpty(text))
				return;

			string pattern = text.Substring(patch.Start2, patch.Length1);
			int padding = 0;

			// Look for the first and last matches of pattern in text. If two different matches are found, increase the pattern length.
			while (text.IndexOf(pattern, StringComparison.Ordinal) != text.LastIndexOf(pattern, StringComparison.Ordinal)
				&& pattern.Length < MAX_BITS - margin - margin) {
				padding += margin;
				pattern = text.JavaSubstring(Math.Max(0, patch.Start2 - padding), Math.Min(text.Length, patch.Start2 + patch.Length1 + padding));
			}
			// Add one chunk for good luck
			padding += margin;

			// Add the prefix.
			string prefix = text.JavaSubstring(Math.Max(0, patch.Start2 - padding), patch.Start2);
			if (prefix.Length > 0)
				patch.Diffs.Insert(0, new DiffOp(Operation.EQUAL, prefix));
			// Add the suffix.
			string suffix = text.JavaSubstring(patch.Start2 + patch.Length1, Math.Min(text.Length, patch.Start2 + patch.Length1 + padding));
			if (suffix.Length > 0)
				patch.Diffs.Add(new DiffOp(Operation.EQUAL, suffix));

			// Roll back the start points.
			patch.Start1 -= prefix.Length;
			patch.Start2 -= prefix.Length;
			// Extend the lengths
			patch.Length1 += prefix.Length + suffix.Length;
			patch.Length2 += prefix.Length + suffix.Length;
		}

		/// <summary>
		/// Given a List of patches, return another List that is identical
		/// </summary>
		/// <param name="patches">List of PatchOp objects.</param>
		/// <returns>Identical List of PatchOp objects.</returns>
		private static List<PatchOp> DeepCopy(this List<PatchOp> patches) {
			List<PatchOp> res = new List<PatchOp>();
			foreach (var aPatch in patches) {
				res.Add(aPatch.Clone());
			}
			return res;
		}

		/// <summary>
		/// Add some padding on text start and end so that edges can match something.
		/// </summary>
		/// <param name="patches">List of PatchOp objects</param>
		/// <param name="margin">Chuck size for context length.</param>
		/// <returns>The padding added to each side.</returns>
		private static string AddPadding(this List<PatchOp> patches, short margin = 4) {
			string padding = string.Empty;
			for (short x = 1; x <= margin; x++)
				padding += (char)x;

			// Bump all the patches forward.
			foreach (var aPatch in patches) {
				aPatch.Start1 += margin;
				aPatch.Start2 += margin;
			}

			// Add some padding on start of first diff.
			PatchOp patch = patches.First();
			if (patch.Diffs.Count == 0 || patch.Diffs.First().Operation != Operation.EQUAL) {
				// Add padding equality.
				patch.Diffs.Insert(0, new DiffOp(Operation.EQUAL, padding));
				patch.Start1 -= margin; // should be 0;
				patch.Start2 -= margin; // should be 0;
				patch.Length1 += margin;
				patch.Length2 += margin;
			} else if (margin > patch.Diffs.First().Text.Length) {
				// Grow first equality.
				DiffOp fDif = patch.Diffs.First();
				int extraLen = margin - fDif.Text.Length;
				fDif.Text = padding.Substring(fDif.Text.Length) + fDif.Text;
				patch.Start1 -= extraLen;
				patch.Start2 -= extraLen;
				patch.Length1 += extraLen;
				patch.Length2 += extraLen;
			}

			// Add some padding on end of last diff.
			patch = patches.Last();
			if (patch.Diffs.Count == 0 || patch.Diffs.Last().Operation != Operation.EQUAL) {
				// Add padding equality
				patch.Diffs.Add(new DiffOp(Operation.EQUAL, padding));
				patch.Length1 += margin;
				patch.Length2 += margin;
			} else if (margin > patch.Diffs.Last().Text.Length) {
				// Grow last equality.
				DiffOp lDif = patch.Diffs.Last();
				int extaLen = margin - lDif.Text.Length;
				lDif.Text += padding.Substring(0, extaLen);
				patch.Length1 += extaLen;
				patch.Length2 += extaLen;
			}

			return padding;
		}

		/// <summary>
		/// Look through the patches and break up any which are longer than the maximum limit of the match algorithm.
		/// </summary>
		/// <param name="patches">List of PatchOp objects.</param>
		private static void SplitMax(this List<PatchOp> patches, short margin = 4) {
			for (int x = 0; x < patches.Count; x++) {
				if (patches[x].Length1 <= MAX_BITS) {
					continue;
				}
				PatchOp bigPatch = patches[x];
				// Remove the big old patch.
				patches.Splice(x--, 1);
				int start1 = bigPatch.Start1;
				int start2 = bigPatch.Start2;
				string precontext = string.Empty;
				while (bigPatch.Diffs.Count != 0) {
					// Create one of several small patches
					PatchOp nPatch = new PatchOp();
					bool empty = true;
					nPatch.Start1 = start1 - precontext.Length;
					nPatch.Start2 = start2 - precontext.Length;
					if (precontext.Length > 0) {
						nPatch.Length1 = precontext.Length;
						nPatch.Length2 = precontext.Length;
						nPatch.Diffs.Add(new DiffOp(Operation.EQUAL, precontext));
					}
					while (bigPatch.Diffs.Count != 0 && nPatch.Length1 < MAX_BITS - margin) {
						Operation diffType = bigPatch.Diffs[0].Operation;
						string diffText = bigPatch.Diffs[0].Text;
						if (diffType == Operation.INSERT) {
							// Insertions are harmless
							nPatch.Length2 = diffText.Length;
							start2 += diffText.Length;
							nPatch.Diffs.Add(bigPatch.Diffs[0].Clone());
							bigPatch.Diffs.RemoveAt(0);
						} else if (diffType == Operation.DELETE && nPatch.Diffs.Count == 1
							&& nPatch.Diffs.First().Operation == Operation.EQUAL && diffText.Length > 2 * MAX_BITS) {
							// This is a large deletion. Let it pass in one chunk.
							nPatch.Length1 += diffText.Length;
							start1 += diffText.Length;
							empty = false;
							nPatch.Diffs.Add(new DiffOp(diffType, diffText));
							bigPatch.Diffs.RemoveAt(0);
						} else {
							//Deletion or equality. Only take as much as we can stomach.
							diffText = diffText.Substring(0, Math.Min(diffText.Length, MAX_BITS - nPatch.Length1 - margin));
							nPatch.Length1 = diffText.Length;
							start1 += diffText.Length;
							if (diffType == Operation.EQUAL) {
								nPatch.Length2 += diffText.Length;
								start2 += diffText.Length;
							} else
								empty = false;
							nPatch.Diffs.Add(new DiffOp(diffType, diffText));
							if (diffText == bigPatch.Diffs[0].Text)
								bigPatch.Diffs.RemoveAt(0);
							else
								bigPatch.Diffs[0].Text = bigPatch.Diffs[0].Text.Substring(diffText.Length);
						}
					}
					// Compute the head context for the next patch.
					precontext = nPatch.Diffs.CurrentText();
					precontext = precontext.Substring(Math.Max(0, precontext.Length - margin));

					// Append the end context for this patch.
					string orig = bigPatch.Diffs.OriginalText();
					string postContext;
					if (orig.Length > margin)
						postContext = orig.Substring(0, margin);
					else
						postContext = orig;

					if (postContext.Length > 0) {
						nPatch.Length1 += postContext.Length;
						nPatch.Length2 += postContext.Length;
						if (nPatch.Diffs.Count > 0 && nPatch.Diffs[nPatch.Diffs.Count - 1].Operation == Operation.EQUAL)
							nPatch.Diffs[nPatch.Diffs.Count - 1].Text += postContext;
						else
							nPatch.Diffs.Add(new DiffOp(Operation.EQUAL, postContext));
					}
					if (!empty)
						patches.Splice(++x, 0, nPatch);
				}
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Compute a list of patches to turn original into current text.
		/// </summary>
		/// <remarks>A set of diffs will be computed.</remarks>
		/// <param name="original">Original text.</param>
		/// <param name="current">Current text.</param>
		/// <param name="margin">Chunk size for context length (default = 4).</param>
		/// <returns>List of PatchOp objects.</returns>
		public static List<PatchOp> MakePatch(this string original, string current, short margin = 4) {
			if (original == null) original = string.Empty;
			if (current == null) current = string.Empty;

			List<DiffOp> diffs = original.GetDiffs(current);
			if (diffs.Count > 2) {
				diffs.CleanupSemantic();
				diffs.CleanupEfficiency();
			}
			return original.MakePatch(diffs, margin);
		}

		/// <summary>
		/// Compute a list of patches to turn original into current text.
		/// </summary>
		/// <remarks>The original text will be derived from the provided diffs.</remarks>
		/// <param name="diffs">List of DiffOp objects</param>
		/// <param name="margin">Chunk size for context length (default = 4).</param>
		/// <returns>List of PatchOp objects.</returns>
		public static List<PatchOp> MakePatch(List<DiffOp> diffs, short margin = 4) {
			if (diffs == null || diffs.Count < 1)
				throw new ArgumentNullException(nameof(diffs));

			// No origin string provided, compute our own.
			string orig = diffs.OriginalText();
			return orig.MakePatch(diffs, margin);
		}

		/// <summary>
		/// Compute a list of patches to turn original into current text.
		/// </summary>
		/// <remarks>Current text is not provided, it is inferred from the DiffOps</remarks>
		/// <param name="original">Original text.</param>
		/// <param name="diffs">List of DiffOp objects between original and current text.</param>
		/// <param name="margin">Chuck size for context length (default = 4).</param>
		/// <returns>List of PatchOp objects.</returns>
		public static List<PatchOp> MakePatch(this string original, List<DiffOp> diffs, short margin = 4) {
			List<PatchOp> res = new List<PatchOp>();
			if (diffs == null || diffs.Count < 1)
				return res; // Get rid of the null case.

			PatchOp patch = new PatchOp();
			int cChar1 = 0; // Number of characters in the text1 string.
			int cChar2 = 0; // Number of characters in the text2 string.

			// Start with text1 (prepatch) and apply the diffs until we arrive at text2 (postpatch). 
			// We recreate the patches one-by-one to determine the contact info.
			string prepatch = original;
			string postpatch = original;
			foreach (DiffOp aDiff in diffs) {
				if (patch.Diffs.Count == 0 && aDiff.Operation != Operation.EQUAL) {
					// A new patch starts here.
					patch.Start1 = cChar1;
					patch.Start2 = cChar2;
				}

				switch (aDiff.Operation) {
					case Operation.INSERT: {
							patch.Diffs.Add(aDiff);
							patch.Length2 += aDiff.Text.Length;
							postpatch = postpatch.Insert(cChar2, aDiff.Text);
							break;
						}
					case Operation.DELETE: {
							patch.Length1 += aDiff.Text.Length;
							patch.Diffs.Add(aDiff);
							postpatch = postpatch.Remove(cChar2, aDiff.Text.Length);
							break;
						}
					default: {
							if (aDiff.Text.Length <= 2 * margin && patch.Diffs.Count != 0 && aDiff != diffs.Last()) {
								// Small equality inside a patch
								patch.Diffs.Add(aDiff);
								patch.Length1 += aDiff.Text.Length;
								patch.Length2 += aDiff.Text.Length;
							}

							if (aDiff.Text.Length >= 2 * margin) {
								// Time for a new patch.
								if (patch.Diffs.Count > 0) {
									patch.AddContext(prepatch);
									res.Add(patch);
									patch = new PatchOp();

									// Unlike UniDiff, our patch lists have a rolling context. https://github.com/google/diff-match-patch/wiki/unidiff
									// Update prepatch text & pos to reflect the application of the just completed patch.
									prepatch = postpatch;
									cChar1 = cChar2;
								}
							}
							break;
						}
				}

				// Update the current character count;
				if (aDiff.Operation != Operation.INSERT)
					cChar1 += aDiff.Text.Length;
				if (aDiff.Operation != Operation.DELETE)
					cChar2 += aDiff.Text.Length;
			}
			// Pick up the leftover patch if not empty
			if (patch.Diffs.Count != 0) {
				patch.AddContext(prepatch);
				res.Add(patch);
			}

			return res;
		}

		/// <summary>
		/// Merge a set of Patches onto the text.
		/// </summary>
		/// <param name="text">Original text.</param>
		/// <param name="input">List of PatchOp objects to apply to the text.</param>
		/// <param name="options">Options used to refine the matching/patching operation</param>
		/// <returns>A Tuple containing:
		/// Item1: The current text.
		/// Item2: An array of boolean values indicating whether or not each patch was applied.
		/// </returns>
		public static Tuple<string, bool[]> ApplyPatch(this string text, List<PatchOp> input, PatchOptions options = null) {
			if (input == null || input.Count < 1)
				return new Tuple<string, bool[]>(text, new bool[0]);

			// Deep copy the patches so that no changes are made to the originals
			var patches = input.DeepCopy();

			if (options == null)
				options = new PatchOptions();

			string padding = patches.AddPadding(options.Margin);
			text = padding + text + padding;
			patches.SplitMax(options.Margin);

			int x = 0;
			/* Delta keeps track of the offset between the expected and actual locaiton of the previous patch. If there
			 are patches expected at positions 10 and 20, but the first patch was found at 12, delta is 2 and the
			 second patch has an effective expected position of 22.*/
			int delta = 0;
			bool[] results = new bool[patches.Count];
			foreach (PatchOp aPatch in patches) {
				int expectedLoc = aPatch.Start2 + delta;
				string orig = aPatch.Diffs.OriginalText();
				int startLoc;
				int endLoc = -1;
				if (orig.Length > MAX_BITS) {
					// SplitMax will only provide an oversized pattern in the case of a monster delete.
					startLoc = text.Match(orig.Substring(0, MAX_BITS), expectedLoc, options);
					if (startLoc != -1) {
						endLoc = text.Match(orig.Substring(orig.Length - MAX_BITS), expectedLoc + orig.Length - MAX_BITS, options);
						if (endLoc == -1 || startLoc >= endLoc) {
							// Can't find valid trailing context. Drop this patch.
							startLoc = -1;
						}
					}
				} else {
					startLoc = text.Match(orig, expectedLoc, options);
				}

				if (startLoc == -1) {
					//No match found :(
					results[x] = false;
					// Subtract the delta for this failed patch from subsequent patches.
					delta -= aPatch.Length2 - aPatch.Length1;
				} else {
					// Found a match :)
					results[x] = true;
					delta = startLoc - expectedLoc;
					string curr;
					if (endLoc == -1)
						curr = text.JavaSubstring(startLoc, Math.Min(startLoc + orig.Length, text.Length));
					else
						curr = text.JavaSubstring(startLoc, Math.Min(endLoc + MAX_BITS, text.Length));

					if (orig == curr) {
						// Perfect match, just show the replacement text in.
						text = text.Substring(0, startLoc) + aPatch.Diffs.CurrentText() + text.Substring(startLoc + orig.Length);
					} else {
						// imperfect match. Run a diff to get a framework of equivalent indices.
						List<DiffOp> diffs = orig.GetDiffs(curr, false);
						if (orig.Length > MAX_BITS && diffs.Levenshtein() / (float)orig.Length > options.DeleteThreshold) {
							// The end points match, but the content is unacceptably bad.
							results[x] = false;
						} else {
							diffs.CleanupSemanticLossless();
							int index1 = 0;
							foreach (var aDif in aPatch.Diffs) {
								if (aDif.Operation != Operation.EQUAL) {
									int index2 = diffs.CrossIndex(index1);
									if (aDif.Operation == Operation.INSERT) {
										// Insertion
										text = text.Insert(startLoc + index2, aDif.Text);
									} else if (aDif.Operation == Operation.DELETE) {
										// Deletion
										text = text.Remove(startLoc + index2, diffs.CrossIndex(index1 + aDif.Text.Length) - index2);
									}
								}
								if (aDif.Operation != Operation.DELETE)
									index1 += aDif.Text.Length;
							}
						}
					}
				}
				x++;
			}
			// strip the padding off.
			text = text.Substring(padding.Length, text.Length - 2 * padding.Length);
			return new Tuple<string, bool[]>(text, results);
		}

		public static bool DidPatchSucceed(this bool[] results) =>
			results.Where(r => !r).Count() < 1;

		/// <summary>
		/// Take a list of patches and return a textual representation.
		/// </summary>
		/// <param name="patches">List of PatchOp objects</param>
		/// <returns>Text representation of patches.</returns>
		public static string PatchToText(this List<PatchOp> patches) {
			if (patches == null || patches.Count < 1)
				return "<null>";

			StringBuilder sb = new StringBuilder();
			foreach (var aP in patches)
				sb.Append(aP);
			return sb.ToString();
		}

		/// <summary>
		/// Parse a textual represenation of patches and return a List of PatchOp objects.
		/// </summary>
		/// <param name="textline">Text representation of patches</param>
		/// <returns>List of PatchOp objects.</returns>
		public static List<PatchOp> PatchFromText(this string textline) {
			List<PatchOp> res = new List<PatchOp>();
			if (string.IsNullOrEmpty(textline) || textline == "<null>")
				return res;

			string[] text = textline.Split('\n');
			int pos = 0;
			while (pos < text.Length) {
				if (!PatchOp.TryParse(text[pos], out PatchOp patch))
					throw new ArgumentException($"Invalid path string: {text[pos]}");
				res.Add(patch);
				pos++;

				while (pos < text.Length) {
					if (string.IsNullOrEmpty(text[pos])) {
						// Blank line; continue.
						pos++;
						continue;
					}
					if (text[pos].StartsWith("@")) // Start next patch.
						break;

					if (DiffOp.TryParse(text[pos], out DiffOp diff)) {
						patch.Diffs.AddIfNotNull(diff);
						pos++;
					} else
						break;
				}
			}
			return res;
		}

		/// <summary>
		/// Quick method to determine if a provided string matches the Patch schema: "@@ -xx,xx +xx,xx @@"
		/// </summary>
		/// <param name="s">String to evaluate</param>
		/// <returns>Bool indicating whether or not the provided string matches the Patch schema</returns>
		public static bool IsStringPatch(this string s) {
			if (string.IsNullOrWhiteSpace(s))
				return false;

			return PatchOp.PATCH_HEADER.IsMatch(s.Truncate());
		}
		#endregion
	}
}