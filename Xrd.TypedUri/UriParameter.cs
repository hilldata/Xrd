using System;

using Xrd.Text;

namespace Xrd.TypedUri {
	/// <summary>
	/// Class representing a parameter in a URI
	/// </summary>
	public class UriParameter {
		#region Fields
		private readonly string _name;
		private string _value;
		#endregion

		#region Properties
		/// <summary>
		/// The parameter name
		/// </summary>
		public string Name => _name.Trim();

		/// <summary>
		/// The parameter value
		/// </summary>
		public string Value {
			get => _value.Trim();
			set => _value = value.Trim();
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Initialize a URI Parameter by parsing the specified string
		/// </summary>
		/// <param name="input">A string representing a standard URI parameter</param>
		public UriParameter(string input) {
			if (string.IsNullOrWhiteSpace(input))
				throw new ArgumentNullException(nameof(input));

			if (!input.Contains("=")) {
				_name = input.Trim();
				Value = null;
			} else {
				var split = input.NonQuotedSplitOnFirst('=');
				_name = split.Item1.Trim();
				Value = IETFValueEncoding.DecodeParameterValue(split.Item2);
			}
		}

		/// <summary>
		/// Initialize a URI Parameter using the specified values
		/// </summary>
		/// <param name="name">The parameter name</param>
		/// <param name="value">The parameter value</param>
		public UriParameter(string name, string value) {
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));

			_name = name.Trim();
			if (string.IsNullOrWhiteSpace(value))
				Value = null;
			else
				Value = value;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Determine if the current instance has the same name as is provided.
		/// </summary>
		/// <param name="name">The name to match</param>
		/// <returns>A boolean indicating whether or not the current instance's name is equivalent to that provided.</returns>
		public bool Is(string name) => Name.Trim().Equals(name.Trim(), StringComparison.OrdinalIgnoreCase);

		/// <summary>
		/// Convers the parameter instance to a string value ready to be used in a URI
		/// </summary>
		/// <returns>A string representation of the parameter instance suitably encoded for use in a URI</returns>
		public override string ToString() {
			if (string.IsNullOrWhiteSpace(Value))
				return null;
			return $"{Name}={IETFValueEncoding.EncodeParameterValue(Value)}";
		}

		/// <summary>
		/// Converts the parameter instance to a string using the specified options.
		/// </summary>
		/// <param name="encodeValue">Should the value be encoded? (default = true)</param>
		/// <returns></returns>
		public string ToString(bool encodeValue = true) {
			if (string.IsNullOrWhiteSpace(Value))
				return null;
			return $";{Name}={(encodeValue ? IETFValueEncoding.EncodeParameterValue(Value) : Value)}";
		}
		#endregion

		/// <summary>
		/// Parse the input into a UriParameter instance.
		/// </summary>
		/// <param name="input">The string to parse.</param>
		/// <returns>A UriParameter instance represented by the input string.</returns>
		public static UriParameter Parse(string input) {
			if (string.IsNullOrWhiteSpace(input))
				return null;

			try {
				return new UriParameter(input);
			} catch {
				return null;
			}
		}
	}
}