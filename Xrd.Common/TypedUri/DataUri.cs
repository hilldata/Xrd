using System;
using System.Collections.Generic;

namespace Xrd.TypedUri {
	public class DataUri : ITypedUri {
		private readonly List<UriParameter> _params;


		/// <summary>
		/// Constructs a DataUri from the provided URI
		/// </summary>
		/// <param name="source"></param>
		public DataUri(Uri source) {
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (source.Scheme != SCHEME)
				throw new ArgumentOutOfRangeException(ParsingHelper.UriWrongSchemeError(SCHEME));

			Uri = source;
			if (Uri.LocalPath.Contains(DataTypes.Binary.SEPARATOR)) {
				BinaryData = new DataTypes.Binary(Uri.LocalPath, out _params);
				BinaryData.PropertyChanged += Value_PropertyChanged;
			} else {
				TextData = new DataTypes.Text(Uri.LocalPath, out _params);
				TextData.PropertyChanged += Value_PropertyChanged;
			}
		}

		/// <summary>
		/// Constructs a DataUri from the provided string.
		/// </summary>
		/// <param name="uriSource"></param>
		public DataUri(string uriSource) {
			if (string.IsNullOrWhiteSpace(uriSource))
				throw new ArgumentNullException(nameof(uriSource));
			if (!uriSource.Trim().ToLower().StartsWith(SCHEME))
				throw new ArgumentOutOfRangeException(ParsingHelper.UriWrongSchemeError(SCHEME));
			if (!Uri.IsWellFormedUriString(uriSource, UriKind.RelativeOrAbsolute))
				throw new UriFormatException();
			Uri = new Uri(uriSource);
			if (Uri.LocalPath.Contains(DataTypes.Binary.SEPARATOR)) {
				BinaryData = new DataTypes.Binary(Uri.LocalPath, out _params);
				BinaryData.PropertyChanged += Value_PropertyChanged;
			} else {
				TextData = new DataTypes.Text(Uri.LocalPath, out _params);
				TextData.PropertyChanged += Value_PropertyChanged;
			}
		}

		/// <summary>
		/// Constructs a DataUri from  the specified <see cref="DataTypes.Binary"/>
		/// </summary>
		/// <param name="binaryData">The Binary value to use for the resulting DataUri</param>
		public DataUri(DataTypes.Binary binaryData) {
			BinaryData = binaryData ?? throw new ArgumentNullException(nameof(binaryData));
			Uri = new Uri(ToString());
			BinaryData.PropertyChanged += Value_PropertyChanged;
			TextData = null;
			_params = null;
		}

		/// <summary>
		/// Constructs a DataUri from the specified <see cref="DataTypes.Text"/>
		/// </summary>
		/// <param name="textData">The Text value to use for the resulting DataUri</param>
		public DataUri(DataTypes.Text textData) {
			TextData = textData ?? throw new ArgumentNullException(nameof(textData));
			Uri = new Uri(ToString());
			TextData.PropertyChanged += Value_PropertyChanged;
			BinaryData = null;
			_params = null;
		}

		private void Value_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			try {
				Uri = new Uri(ToString());
			} catch { }
		}

		#region Properties
		/// <summary>
		/// The URI scheme used by the DataUri class
		/// </summary>
		public const string SCHEME = "data";

		/// <summary>
		/// The full value of the URI.
		/// </summary>
		public Uri Uri { get; private set; }

		/// <summary>
		/// The underlying BinaryData.
		/// </summary>
		public DataTypes.Binary BinaryData { get; }

		/// <summary>
		/// The underlying TextData.
		/// </summary>
		public DataTypes.Text TextData { get; }

		/// <summary>
		/// List of unknow parameters (undefined) that were included in the original URI
		/// </summary>
		public List<UriParameter> OtherParameters => _params;

		public bool IsValueNull {
			get {
				if (BinaryData != null)
					return BinaryData.IsValueNull;
				else if (TextData != null)
					return TextData.IsValueNull;
				else
					return true;
			}
		}
		#endregion

		/// <summary>
		/// Returns the current value of the instance as a well-formatted string.
		/// </summary>
		/// <returns>A URI formatted string</returns>
		public override string ToString() {
			string res = $"{SCHEME}:";
			if (BinaryData != null && !BinaryData.IsValueNull)
				res += BinaryData.ToString();
			else if (TextData != null && !TextData.IsValueNull)
				res += TextData.ToString();
			else
				return null;
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

		public static DataUri Parse(string input) => new DataUri(input);

		public static bool TryParse(string input, out DataUri result) {
			try {
				result = new DataUri(input);
				return true;
			} catch {
				result = null;
				return false;
			}
		}
	}
}