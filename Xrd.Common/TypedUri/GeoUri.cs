using System;
using System.Collections.Generic;

namespace Xrd.TypedUri {
	public class GeoUri : ITypedUri {
		private readonly List<UriParameter> _params;

		/// <summary>
		/// Constructs a GeoUri from the provided URI
		/// </summary>
		/// <param name="source"></param>
		public GeoUri(Uri source) {
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (source.Scheme != SCHEME)
				throw new ArgumentOutOfRangeException(ParsingHelper.UriWrongSchemeError(SCHEME));
			Uri = source;
			Value = new DataTypes.GeoLoc(Uri.LocalPath, out _params);
			Value.PropertyChanged += Value_PropertyChanged;
		}

		/// <summary>
		/// Constructs a GeoUri from the provided string
		/// </summary>
		/// <param name="uriSource"></param>
		public GeoUri(string uriSource) {
			if (string.IsNullOrWhiteSpace(uriSource))
				throw new ArgumentNullException(nameof(uriSource));
			if (!uriSource.Trim().ToLower().StartsWith(SCHEME))
				throw new ArgumentOutOfRangeException(ParsingHelper.UriWrongSchemeError(SCHEME));
			if (!Uri.IsWellFormedUriString(uriSource, UriKind.Relative))
				throw new UriFormatException();
			Uri = new Uri(uriSource);
			Value = new DataTypes.GeoLoc(Uri.LocalPath, out _params);
			Value.PropertyChanged += Value_PropertyChanged;
		}

		/// <summary>
		/// Constructs a GeoUri from the specified <see cref="DataTypes.GeoLoc"/>
		/// </summary>
		/// <param name="geoLoc">The GeoLoc value to use for the resulting GeoUri</param>
		public GeoUri(DataTypes.GeoLoc geoLoc) {
			Value = geoLoc ?? throw new ArgumentNullException(nameof(geoLoc));
			Uri = new Uri(ToString());
			_params = null;
			Value.PropertyChanged += Value_PropertyChanged;
		}

		private void Value_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			Uri = new Uri(ToString());
		}

		#region Properties
		/// <summary>
		/// The URI scheme used by the GeoUri class.
		/// </summary>
		public const string SCHEME = "geo";

		/// <summary>
		/// The full value of the URI
		/// </summary>
		public Uri Uri { get; private set; }

		/// <summary>
		/// The value of the GeoUri
		/// </summary>
		public DataTypes.GeoLoc Value { get; }

		/// <summary>
		/// List of unknown parameters (undefined) that were included in the original URI
		/// </summary>
		public List<UriParameter> OtherParameters => _params;

		public bool IsValueNull => Value == null;
		#endregion

		/// <summary>
		/// Returns the current value of the instance as a well-formatted string.
		/// </summary>
		/// <returns>A URI formatted string</returns>
		public override string ToString() {
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

		public static GeoUri Parse(string input) => new GeoUri(input);

		public static bool TryParse(string input, out GeoUri result) {
			try {
				result = new GeoUri(input);
				return true;
			} catch {
				result = null;
				return false;
			}
		}
	}
}