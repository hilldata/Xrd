using System;
using System.Collections.Generic;

using Xrd.TypedUri.DataTypes;

namespace Xrd.TypedUri {
	public class UrnUri : ITypedUri {
		private readonly List<UriParameter> _params;

		#region Constructors
		/// <summary>
		/// Construct a UrnUri from the provided URI
		/// </summary>
		/// <param name="source"></param>
		public UrnUri(Uri source) {
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (source.Scheme != SCHEME)
				throw new ArgumentOutOfRangeException(ParsingHelper.UriWrongSchemeError(SCHEME));

			Uri = source;
			Value = new Uuid(Uri.LocalPath, out _params);
			Value.PropertyChanged += Value_PropertyChanged;
		}

		/// <summary>
		/// Construct a UrnUri from the provided string.
		/// </summary>
		/// <param name="uriSource"></param>
		public UrnUri(string uriSource) {
			if (string.IsNullOrWhiteSpace(uriSource))
				throw new ArgumentNullException(nameof(uriSource));
			if (!uriSource.Trim().ToLower().StartsWith(SCHEME))
				throw new ArgumentOutOfRangeException(ParsingHelper.UriWrongSchemeError(SCHEME));
			if (!Uri.IsWellFormedUriString(uriSource, UriKind.RelativeOrAbsolute))
				throw new UriFormatException();

			Uri = new Uri(uriSource);
			Value = new Uuid(Uri.LocalPath, out _params);
			Value.PropertyChanged += Value_PropertyChanged;
		}

		/// <summary>
		/// Construct a UrnUri from the provided Guid
		/// </summary>
		/// <param name="value"></param>
		public UrnUri(Guid value) {
			Value = new Uuid(value);
			Uri = new Uri(ToString());
			_params = null;
			Value.PropertyChanged += Value_PropertyChanged;
		}

		/// <summary>
		/// Construct a UrnUri from the provided <see cref="Uuid"/>
		/// </summary>
		/// <param name="value"></param>
		public UrnUri(Uuid value) {
			Value = value;
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
		/// The URI scheme used by the UrnUri class.
		/// </summary>
		public const string SCHEME = "urn";

		/// <summary>
		/// The value of the UrnUri
		/// </summary>
		public Uuid Value { get; }

		public Guid? GetGuid {
			get {
				if (Value == null || Value.IsValueNull || Value.Value.Value.Equals(Guid.Empty))
					return null;
				return Value.Value;
			}
		}

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
		/// Returns the current value of the instance as a well-formed string
		/// </summary>
		/// <returns>A URI formatted string.</returns>
		public override string ToString() {
			if (Value.IsValueNull)
				return null;

			string res = $"{SCHEME}:{Value}";
			if (_params != null && _params.Count > 0) {
				foreach (var pa in _params)
					res += pa.ToString();
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

		public static UrnUri Parse(string input) => new UrnUri(input);

		public static bool TryParse(string input, out UrnUri result) {
			try {
				result = new UrnUri(input);
				return true;
			} catch {
				result = null;
				return false;
			}
		}
	}
}