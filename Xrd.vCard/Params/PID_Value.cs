using System;

namespace Xrd.vCard.Params {
	/// <summary>
	/// Class representing a PID value.
	/// </summary>
	public class PID_Value {
		/// <summary>
		/// Initializes a PID_Value by parsing the input text.
		/// </summary>
		/// <param name="input">The text to parse</param>
		public PID_Value(string input) {
			Read(input);
		}
		/// <summary>
		/// Initializes a PID_Value with the specified whole numbers.
		/// </summary>
		/// <param name="x">The Instance portion of the PID Value.</param>
		/// <param name="y">The (optional) Source portion of the PID value.</param>
		public PID_Value(byte x, byte? y = null) {
			_x = x;
			_y = y;
		}

		/// <summary>
		/// Reads and parses the input to inialize the PID_Value
		/// </summary>
		/// <param name="input">The text to be read.</param>
		public void Read(string input) {
			if (string.IsNullOrWhiteSpace(input))
				throw new ArgumentNullException("input");

			bool xSet = false;
			bool dotHit = false;
			foreach (char c in input) {
				if (char.IsDigit(c)) {
					if (!xSet) {
						xSet = true;
						_x = byte.Parse(c.ToString());
					} else {
						if (dotHit) {
							_y = byte.Parse(c.ToString());
							return;
						}
					}
				} else if (c == '.')
					dotHit = true;
			}
		}
		#region Fields
		private byte _x = 0;
		private byte? _y = null;
		#endregion

		#region Properties
		/// <summary>
		/// Gets/Sets the Instance portion of the PID value.
		/// </summary>
		public byte Instance {
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
		/// Gets/Sets the (optional) Source portion of the PID value.
		/// </summary>
		public byte? Source {
			get { return _y; }
			set {
				if (!_y.Equals(value)) {
					if (value.HasValue && value.Value > 9)
						value = 9;
					_y = value;
				}
			}
		}
		#endregion

		/// <summary>
		/// Overrides the ToString method
		/// </summary>
		/// <returns>A PID value as a formatted string.</returns>
		public override string ToString() {
			if (_x < 1)
				return null;

			if (Source.HasValue)
				return $"{Instance}.{Source.Value}";
			else
				return $"{Instance}";
		}
	}
}