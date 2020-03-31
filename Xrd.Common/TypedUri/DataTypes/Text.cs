using System;
using System.Collections.Generic;

using Xrd.Text;

namespace Xrd.TypedUri.DataTypes {
	/// <summary>
	/// Class representing data being encoded into URIs with no scheme specified
	/// </summary>
	/// <remarks>See https://tools.ietf.org/html/rfc2397 </remarks>
	public class Text : UriDataTypeBase, INullable {
		#region Fields
		private string _mediaType;
		private string _data;
		private string _charSet;
		#endregion

		#region Constructors
		/// <summary>
		/// Construct an instance of the Text URI data type.
		/// </summary>
		/// <param name="data">The text</param>
		/// <param name="mediaType">the MIME type of the underlying text data.</param>
		/// <param name="charSet">The Character Set used by the underlying text data.</param>
		public Text(string data, string mediaType = null, string charSet = null) {
			if (string.IsNullOrWhiteSpace(data))
				throw new ArgumentNullException(nameof(data));

			Data = data;
			if (!string.IsNullOrWhiteSpace(mediaType) && !mediaType.ToLower().Equals(DEFAULT_MEDIATYPE))
				MediaType = mediaType;
			if (!string.IsNullOrWhiteSpace(charSet))
				CharacterSet = charSet;
		}

		/// <summary>
		/// Construct an instance of the Text URI data type.
		/// </summary>
		/// <param name="uriPath">Either the LocalPath or AbsolutePath from a "data:" URI (that does not contain "base64,"</param>
		/// <param name="otherParameters">Returns any additional, unrecognized URI parameters.</param>
		public Text(string uriPath, out List<UriParameter> otherParameters) {
			if (string.IsNullOrWhiteSpace(uriPath))
				throw new ArgumentNullException(nameof(uriPath));
			if (uriPath.NonQuotedIndexOf(SEPARATOR) < 0)
				throw new ArgumentException(ParsingHelper.UriPathNotWellFormedMessage("text data"), nameof(uriPath));

			var pair = uriPath.NonQuotedSplitOnFirst(SEPARATOR);
			Data = pair.Item2;
			if (string.IsNullOrWhiteSpace(pair.Item1) || pair.Item1.NonQuotedIndexOf(';') < 0) {
				_mediaType = null;
				_charSet = null;
				otherParameters = null;
				return;
			}
			otherParameters = new List<UriParameter>();
			foreach (string s in pair.Item1.NonQuotedSplit(';')) {
				UriParameter p = new UriParameter(s);
				if (string.IsNullOrWhiteSpace(p.Value))
					MediaType = p.Name;
				else if (p.Is("charset"))
					CharacterSet = p.Value;
				else
					otherParameters.Add(p);
			}
			if (otherParameters.Count < 1)
				otherParameters = null;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The MIME type of the underlying data string (default = "text/plain")
		/// </summary>
		public string MediaType {
			get => _mediaType;
			private set {
				SetField(ref _mediaType, value);
			}
		}

		/// <summary>
		/// The data text
		/// </summary>
		public string Data {
			get => _data;
			private set {
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException(nameof(value));

				SetField(ref _data, value);
			}
		}

		/// <summary>
		/// The character set used by the data.
		/// </summary>
		public string CharacterSet {
			get => _charSet;
			private set {
				SetField(ref _charSet, value);
			}
		}

		public bool IsValueNull => string.IsNullOrWhiteSpace(Data);
		#endregion

		/// <summary>
		/// Converts the text data to a string that contains the MIME type, optional CharSet, and is appropriate for use in the path section of a "data:" URI.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			if (string.IsNullOrWhiteSpace(Data))
				return null;

			string res = string.Empty;
			if (!string.IsNullOrWhiteSpace(MediaType) && !MediaType.ToLower().Equals(DEFAULT_MEDIATYPE))
				res += MediaType;
			if (!string.IsNullOrWhiteSpace(CharacterSet)) {
				UriParameter cParm = new UriParameter("charset", CharacterSet);
				if (res.Length < 1)
					res = $"{DEFAULT_MEDIATYPE}{cParm}";
				else
					res += $"{cParm}";
			}
			res += $"{SEPARATOR}{Data}";
			return res;
		}

		#region Constants
		/// <summary>
		/// The default MIME (media) type for string data.
		/// </summary>
		public const string DEFAULT_MEDIATYPE = "text/plain";

		/// <summary>
		/// Character string that separates the parameters from the text data.
		/// </summary>
		public const char SEPARATOR = ',';
		#endregion
	}
}