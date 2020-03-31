using System;
using System.Collections.Generic;

using Xrd.TypedUri.DataTypes;

namespace Xrd.TypedUri {
	public class TelUri : ITypedUri {
		private readonly List<UriParameter> _params;

		#region Constructors
		/// <summary>
		/// Construct a TelUri from the provided URI
		/// </summary>
		/// <param name="source"></param>
		public TelUri(Uri source) {
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (source.Scheme != SCHEME)
				throw new ArgumentOutOfRangeException(ParsingHelper.UriWrongSchemeError(SCHEME));

			Uri = source;
			Value = new Tel(Uri.LocalPath, out _params);
			Value.PropertyChanged += Value_PropertyChanged;
		}

		/// <summary>
		/// Construct a TelUri from the provided string.
		/// </summary>
		/// <param name="uriSource"></param>
		public TelUri(string uriSource) {
			if (string.IsNullOrWhiteSpace(uriSource))
				throw new ArgumentNullException(nameof(uriSource));
			if (!uriSource.Trim().ToLower().StartsWith(SCHEME))
				throw new ArgumentOutOfRangeException(ParsingHelper.UriWrongSchemeError(SCHEME));
			if (!Uri.IsWellFormedUriString(uriSource, UriKind.RelativeOrAbsolute))
				throw new UriFormatException();

			Uri = new Uri(uriSource);
			Value = new Tel(Uri.LocalPath, out _params);
			Value.PropertyChanged += Value_PropertyChanged;
		}

		/// <summary>
		/// Construct a TelUri from the specified <see cref="Tel"/>
		/// </summary>
		/// <param name="tel">The Tel value to use for the resulting TelUri</param>
		public TelUri(Tel tel) {
			Value = tel ?? throw new ArgumentNullException(nameof(tel));
			Uri = new Uri(ToString());
			_params = null;
			Value.PropertyChanged += Value_PropertyChanged;
		}

		private void Value_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			Uri = new Uri(ToString());
		}
		#endregion

		#region Properties
		/// <summary>
		/// The URI scheme used by the TelUri class.
		/// </summary>
		public const string SCHEME = "tel";

		/// <summary>
		/// The Value of the TelUri
		/// </summary>
		public Tel Value { get; }

		/// <summary>
		/// The full value of the URI
		/// </summary>
		public Uri Uri { get; private set; }

		/// <summary>
		/// List of unknown parameters (undefined) that were included in the original URI
		/// </summary>
		public List<UriParameter> OtherParameters => _params;

		public bool IsValueNull => Value == null || Value.IsValueNull;
		#endregion

		/// <summary>
		/// Returns the current value of the instance as a well-formed string.
		/// </summary>
		/// <returns>A URI formatted string.</returns>
		public override string ToString() {
			if (Value.IsValueNull)
				return null;

			string res = $"{SCHEME}:{Value.ToString()}";
			if (_params != null && _params.Count > 0) {
				foreach (var p in _params)
					res += p.ToString();
			}
			return res;
		}

		public static bool IsWellFormedString(string input) {
			if (string.IsNullOrWhiteSpace(input))
				return false;
			if (!input.ToLower().StartsWith(SCHEME))
				return false;
			return Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute);
		}

		public static TelUri Parse(string input) => new TelUri(input);

		public static bool TryParse(string input, out TelUri result) {
			try {
				result = new TelUri(input);
				return true;
			} catch {
				result = null;
				return false;
			}
		}
	}
}