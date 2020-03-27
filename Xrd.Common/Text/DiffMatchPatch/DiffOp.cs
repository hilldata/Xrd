using System;
using System.Collections.Generic;
using System.Text;

namespace Xrd.Text.DiffMatchPatch {
	/// <summary>
	/// Class representing one diff operation
	/// </summary>
	/// <remarks>
	/// Adapted from https://github.com/google/diff-match-patch 
	/// </remarks>
	public class DiffOp {
		/// <summary>
		/// The operation that was performed.
		/// </summary>
		public Operation Operation;
		/// <summary>
		/// The text of the operation.
		/// </summary>
		public string Text;

		public DiffOp(Operation operation, string text) {
			Operation = operation;
			Text = text;
		}

		public DiffOp Clone() =>
			new DiffOp(Operation, Text);

		internal static DiffOp Parse(string s) {
			if (string.IsNullOrEmpty(s))
				return null;

			char sign = s[0];
			string line = Uri.UnescapeDataString(s.Substring(1).Replace("+", "%2b"));
			if (sign == '-')    //Deletion
				return new DiffOp(Operation.DELETE, line);
			else if (sign == '+')   //Insertion
				return new DiffOp(Operation.INSERT, line);
			else if (sign == ' ')   //Minor equality
				return new DiffOp(Operation.EQUAL, line);
			else if (sign == '@')   // Start of next patch
				return null;
			else // ?? Unrecongized sign
				throw new ArgumentException($"Invalid patch mode '{sign}' in: {line}");
		}

		internal static bool TryParse(string s, out DiffOp res) {
			if (string.IsNullOrEmpty(s)) {
				res = null;
				return true;
			}

			char sign = s[0];
			string line = Uri.UnescapeDataString(s.Substring(1).Replace("+", "%2b"));
			if (sign == '-') // Deletion
				res = new DiffOp(Operation.DELETE, line);
			else if (sign == '+') // Insertion
				res = new DiffOp(Operation.INSERT, line);
			else if (sign == ' ') // minori equality
				res = new DiffOp(Operation.EQUAL, line);
			else if (sign == '@') { // Start of next patch
				res = null;
				return false;
			} else // ?? unrecognized sign.
				throw new ArgumentException($"Invalid patch mode: '{sign}' in: {line}");

			return true;
		}

		public override string ToString() {
			string prettyText = Text.Replace('\n', '\u00b6');
			return $"Diff({Operation},\"{prettyText}\"";
		}

		public override bool Equals(object obj) {
			//If parameter is null return false;
			if (obj == null)
				return false;

			//If parameter cannot be cast to Diff return false
			if (!(obj is DiffOp p))
				return false;

			//Return true if the fields match
			return p.Operation == Operation && p.Text == Text;
		}

		public bool Equals(DiffOp obj) {
			// if parameter is null return false
			if (obj == null)
				return false;

			//Return true if the fields match
			return obj.Operation == Operation && obj.Text == Text;
		}

		public override int GetHashCode() =>
			Text.GetHashCode() ^ Operation.GetHashCode();
	}
}