using System;
using System.Collections.Generic;

using Xrd.Text;

namespace Xrd.TypedUri.DataTypes {
	/// <summary>
	/// Class representing Guid/UUID data as used in URIs with the "urn:" scheme
	/// </summary>
	/// <remarks>See https://tools.ietf.org/html/draft-kindel-uuid-uri-00 </remarks>
	public class Uuid : UriDataTypeBase, INullable {
		#region Fields
		private Guid? _value;
		#endregion

		public const string DEFUALT_NAME = "uuid";

		#region Properties
		/// <summary>
		/// The Unique identifier
		/// </summary>
		public Guid? Value {
			get => _value;
			set => SetField(ref _value, value);
		}

		public bool IsValueNull => !Value.HasValue || Value.Value.Equals(Guid.Empty);
		#endregion

		#region Constructors
		/// <summary>
		/// Construct an instance of the Uuid URI data type
		/// </summary>
		/// <param name="value">The Guid value</param>
		public Uuid(Guid value) {
			Value = value;
		}

		/// <summary>
		/// Construct an instance of the Uuid URI data type
		/// </summary>
		/// <param name="uriPath">Either the LocalPath or AbsolutePath from a "urn:" URI</param>
		/// <param name="otherParameters">Returns any additional, unrecognized URI parameters.</param>
		public Uuid(string uriPath, out List<UriParameter> otherParameters) {
			if (string.IsNullOrWhiteSpace(uriPath))
				throw new ArgumentNullException(nameof(uriPath));
			otherParameters = null;
			if (uriPath.Contains(";")) {
				otherParameters = new List<UriParameter>();
				foreach (string s in uriPath.NonQuotedSplit(';')) {
					UriParameter p = new UriParameter(s);
					if (string.IsNullOrWhiteSpace(p.Value)) {
						try {
							SetValue(p.Name);
						} catch { }
					}
					otherParameters.Add(p);
				}
			}
			if (otherParameters.Count < 1)
				otherParameters = null;
		}
		#endregion

		public override string ToString() {
			if (_value.HasValue)
				return $"{DEFUALT_NAME}:{Value}";
			return null;
		}

		private void SetValue(string uuid) {
			if (string.IsNullOrWhiteSpace(uuid))
				throw new ArgumentNullException(nameof(uuid));
			string temp;
			if (uuid.Contains(":"))
				temp = uuid.Split(':')[1];
			else
				temp = uuid;
			if (Guid.TryParse(temp, out Guid res))
				Value = res;
			throw new Exception("The uuid text is not properly formatted and cannot be parsed.");
		}
	}
}