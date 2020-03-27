using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Xrd.Text.DiffMatchPatch {
	/// <summary>
	/// Class representing one patch operation
	/// </summary>
	/// <remarks>
	/// Adapted from https://github.com/google/diff-match-patch 
	/// Original algorith used an instance of a class, but this library converts all public members to extension methods.
	/// </remarks>
	public class PatchOp {
		public List<DiffOp> Diffs;
		public int Start1;
		public int Start2;
		public int Length1;
		public int Length2;

		public PatchOp() {
			Diffs = new List<DiffOp>();
		}

		public PatchOp(string s) => Parse(s);

		public PatchOp(int s1, int s2, int l1, int l2) : this() {
			Start1 = s1;
			Start2 = s2;
			Length1 = l1;
			Length2 = l2;
		}
		private PatchOp(int[] coords) : this() {
			Start1 = coords[0];
			Length1 = coords[1];
			Start2 = coords[2];
			Length2 = coords[3];
		}

		public PatchOp(int s1, int s2, int l1, int l2, params DiffOp[] diffs) : this(s1, s2, l1, l2) {
			if (diffs != null && diffs.Length > 0) {
				foreach (var aDif in diffs)
					Diffs.Add(aDif.Clone());
			}
		}

		public PatchOp(int s1, int s2, int l1, int l2, List<DiffOp> diffs) : this(s1, s2, l1, l2) {
			if (diffs != null) {
				foreach (var aDif in diffs)
					Diffs.Add(aDif.Clone());
			}
		}

		public PatchOp Clone() =>
			new PatchOp(Start1, Start2, Length1, Length2, Diffs);

		public const string HEADER_REGEX_STRING = "^@@ -(\\d+),?(\\d*) \\+(\\d+),?(\\d*) @@$";
		internal static Regex PATCH_HEADER = new Regex(HEADER_REGEX_STRING);

		internal static PatchOp Parse(string s) {
			Match m = PATCH_HEADER.Match(s);
			if (!m.Success)
				throw new ArgumentException($"Invalid patch string: {s}", nameof(s));

			int[] coords = new int[4];
			for (int i = 1; i < 5; i++) {
				if (i % 2 == 1) // odd (start)
					coords[i - 1] = int.Parse(m.Groups[i].Value);
				else {  //even (length)
					if (m.Groups[i].Length == 0) {
						coords[i - 2]--;
						coords[i - 1] = 1;
					} else if (m.Groups[i].Value == "0")
						coords[i - 1] = 0;
					else {
						coords[i - 2]--;
						coords[i - 1] = int.Parse(m.Groups[i].Value);
					}
				}
			}
			return new PatchOp(coords);
		}

		internal static bool TryParse(string s, out PatchOp res) {
			Match m = PATCH_HEADER.Match(s);
			if (!m.Success) {
				res = null;
				return false;
			}

			int[] coords = new int[4];
			for (int i = 1; i < 5; i++) {
				if (i % 2 == 1) // odd (start)
					coords[i - 1] = int.Parse(m.Groups[i].Value);
				else {  //even (length)
					if (m.Groups[i].Length == 0) {
						coords[i - 2]--;
						coords[i - 1] = 1;
					} else if (m.Groups[i].Value == "0")
						coords[i - 1] = 0;
					else {
						coords[i - 2]--;
						coords[i - 1] = int.Parse(m.Groups[i].Value);
					}
				}
			}
			res = new PatchOp(coords);
			return true;
		}

		/// <summary>
		/// Emulate GNU diff's format.
		/// </summary>
		/// <remarks>
		/// Header: @@ -382,8 +481,9 @@
		/// Indices are printed as 1-based, not 0-based.
		/// </remarks>
		/// <returns>The GNU diff string.</returns>
		public override string ToString() {
			string coords1, coords2;
			if (Length1 == 0)
				coords1 = $"{Start1},0";
			else if (Length1 == 1)
				coords1 = $"{Start1 + 1}";
			else
				coords1 = $"{Start1 + 1},{Length1}";

			if (Length2 == 0)
				coords2 = $"{Start2},0";
			else if (Length2 == 1)
				coords2 = $"{Start2 + 1}";
			else
				coords2 = $"{Start2 + 1},{Length2}";

			StringBuilder text = new StringBuilder();
			text.Append("@@ -")
				.Append(coords1)
				.Append(" +")
				.Append(coords2)
				.Append(" @@\n");

			//Escape the body of the patch with %xx notation.
			foreach (DiffOp aDiff in Diffs) {
				switch (aDiff.Operation) {
					case Operation.INSERT: {
							text.Append('+');
							break;
						}
					case Operation.DELETE: {
							text.Append('-');
							break;
						}
					case Operation.EQUAL: {
							text.Append(' ');
							break;
						}
				}
				text.Append(aDiff.Text.EncodeURI())
					.Append("\n");
			}
			return text.ToString();
		}
	}
}