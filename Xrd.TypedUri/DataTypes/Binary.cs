using System;
using System.Collections.Generic;
using System.Linq;

using Xrd.Collections;
using Xrd.Text;

namespace Xrd.TypedUri.DataTypes {
	/// <summary>
	/// Class representing binary data being encoded into URIs with the "data:" scheme
	/// </summary>
	/// <remarks>See https://tools.ietf.org/html/rfc2397 </remarks>
	public class Binary : UriDataTypeBase, INullable {
		#region Fields
		private string _mediaType;
		private byte[] _data;
		#endregion

		#region Constructors
		/// <summary>
		/// Construct a instance of the Binary URI data type
		/// </summary>
		/// <param name="mediaType">The MIME type of the underlying data.</param>
		/// <param name="data">The data being encoded</param>
		public Binary(string mediaType, byte[] data) {
			if (string.IsNullOrWhiteSpace(mediaType))
				throw new ArgumentNullException(nameof(mediaType));
			if (data == null || data.Length < 1)
				throw new ArgumentNullException(nameof(data));

			_mediaType = mediaType.Trim().ToLower();
			_data = data;
		}

		/// <summary>
		/// Construct an instance of the Binary URI data type
		/// </summary>
		/// <param name="uriPath">Either the LocalPath or AbsolutePath from a "data:" URI</param>
		/// <param name="otherParameters">Returns any additional, unrecognized URI parameters</param>
		public Binary(string uriPath, out List<UriParameter> otherParameters) {
			if (string.IsNullOrWhiteSpace(uriPath))
				throw new ArgumentNullException(nameof(uriPath));
			if (!uriPath.Contains(SEPARATOR))
				throw new ArgumentException(ParsingHelper.UriPathNotWellFormedMessage("base-64 encoded"), nameof(uriPath));

			List<string> split = uriPath.NonQuotedSplit(';', StringSplitOptions.RemoveEmptyEntries, true);
			List<UriParameter> parms = new List<UriParameter>();
			foreach (string s in split)
				parms.AddIfNotNull(UriParameter.Parse(s));

			MediaType = parms[0].Name;
			UriParameter data = parms.Where(p => p.Name.StartsWith(SEPARATOR)).FirstOrDefault();
			Data = Convert.FromBase64String(data.Name.Replace(SEPARATOR, string.Empty));

			if (parms.Count < 2)
				otherParameters = null;
			else {
				otherParameters = new List<UriParameter>();
				for (int i = 2; i < parms.Count; i++)
					otherParameters.Add(parms[i]);
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// The MIME type of the underlying data.
		/// </summary>
		public string MediaType {
			get => _mediaType;
			private set {
				SetField(ref _mediaType, value);
			}
		}

		/// <summary>
		/// The data
		/// </summary>
		public byte[] Data {
			get => _data;
			private set {
				if (value == null || value.Length < 1)
					throw new ArgumentNullException(nameof(value));

				SetField(ref _data, value);
			}
		}

		public bool IsValueNull => Data != null && Data.Length > 0;
		#endregion

		/// <summary>
		/// Character string that separates the MediaType from the base-64 encoded binary data.
		/// </summary>
		public const string SEPARATOR = "base64,";

		/// <summary>
		/// Converts the binary data to a Base-64-encoded string that contains the MIME type and is appropriate for use in the path section of a "data:" URI.
		/// </summary>
		/// <returns></returns>
		public override string ToString() => $"{MediaType};{SEPARATOR}{Convert.ToBase64String(Data)}";
	}
}