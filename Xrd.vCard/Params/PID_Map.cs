using System;

using Xrd.Text;

namespace Xrd.vCard.Params {
	/// Sub-class used to define a PID Map
	/// </summary>
	public class PID_Map {
		/// <summary>
		/// Create an instance of the PID Map class by parsing the specified text input
		/// </summary>
		/// <param name="input">The text to parse</param>
		public PID_Map(string input) {
			if (string.IsNullOrWhiteSpace(input))
				throw new ArgumentNullException("input");
			if (input.IndexOf(';') < 1)
				throw new FormatException("input is not of the format <digit>;<uri>");
			Tuple<string, string> split = input.NonQuotedSplitOnFirst(';');
			if (!byte.TryParse(split.Item1, out byte b))
				throw new FormatException("input is not of the format <digit>;<uri>");
			if (!Uri.IsWellFormedUriString(split.Item2, UriKind.RelativeOrAbsolute))
				throw new FormatException("input is not of the format <digit>;<uri>");

			_x = b;
			_y = new Uri(split.Item2, UriKind.RelativeOrAbsolute);
		}

		/// <summary>
		/// Create an instance of the PID Map class from the specified inputs.
		/// </summary>
		/// <param name="x">The digit indicating the source</param>
		/// <param name="y">The uri of the source.</param>
		public PID_Map(byte x, Uri y = null) {
			_x = x;
			_y = y;
		}

		/// <summary>
		/// Check to see if the text input is a well formed string that can be parsed.
		/// </summary>
		/// <param name="input">The text to check</param>
		/// <returns>boolean indicating whether or not the text can be parsed.</returns>
		public static bool IsWellFormedString(string input) {
			if (string.IsNullOrWhiteSpace(input))
				return false;
			if (input.IndexOf(';') < 1)
				return false;
			Tuple<string, string> split = input.NonQuotedSplitOnFirst(';');
			if (!byte.TryParse(split.Item1, out _))
				return false;
			return Uri.IsWellFormedUriString(split.Item2, UriKind.RelativeOrAbsolute);
		}
		#region Fields
		private byte _x = 0;
		private Uri _y = null;
		#endregion

		#region Properties
		/// <summary>
		/// The number indicating the source.
		/// </summary>
		public byte Source {
			get { return _x; }
			set {
				if (!_x.Equals(value)) {
					if (value > 9)
						value = 9;
					_x = value;
				}
			}
		}

		/// <summary>
		/// The uri to the source.
		/// </summary>
		public Uri Uri {
			get { return _y; }
			set {
				if (!_y.Equals(value)) {
					_y = value;
				}
			}
		}
		#endregion
	}
}