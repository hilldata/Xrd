using System;
using System.Collections.Generic;

using Xrd.Text;

namespace Xrd.TypedUri.DataTypes {
	/// <summary>
	/// Class representing telephone data as used in URIs with the "tel:" scheme.
	/// </summary>
	/// <remarks>See https://tools.ietf.org/html/rfc3966 </remarks>
	public class Tel : UriDataTypeBase, INullable {
		#region Fields
		private string _number;
		private string _context;
		private string _extension;
		private bool? _isExtension;
		#endregion

		public const string DEFAULT_CONTEXT = "+313";

		#region Properties
		/// <summary>
		/// The phone number
		/// </summary>
		public string Number {
			get => _number.Trim();
			set {
				if (string.IsNullOrWhiteSpace(value))
					SetField(ref _number, null);
				else
					SetField(ref _number, value.Trim());
			}
		}

		/// <summary>
		/// The context a local number is valid within.
		/// </summary>
		public string Context {
			get => _context.Trim();
			set {
				if (string.IsNullOrWhiteSpace(value))
					SetField(ref _context, null);
				else
					SetField(ref _context, value.Trim());
			}
		}

		/// <summary>
		/// The station behind PBX the number targets
		/// </summary>
		public string Extension {
			get {
				if (_isExtension ?? true)
					return _extension.Trim();
				else
					return null;
			}
			set {
				if (string.IsNullOrWhiteSpace(value)) {
					SetField(ref _extension, null);
					SetField(ref _isExtension, null);
				} else {
					SetField(ref _extension, value.Trim());
					SetField(ref _isExtension, true);
				}
			}
		}

		/// <summary>
		/// The ISDN subaddress the number targets.
		/// </summary>
		public string IsdnSubAddress {
			get {
				if (!_isExtension ?? true)
					return _extension.Trim();
				else
					return null;
			}
			set {
				if (string.IsNullOrWhiteSpace(value)) {
					SetField(ref _extension, null);
					SetField(ref _isExtension, null);
				} else {
					SetField(ref _extension, value.Trim());
					SetField(ref _isExtension, false);
				}
			}
		}

		/// <summary>
		/// A Boolean value indicating whether or not the number contains a leading '+' character.
		/// </summary>
		public bool IsGlobal => Number.StartsWith("+");

		/// <summary>
		/// A Booelan value indicating whether or not the number does NOT contain a leading '+' character. Local numbers must specify a Context.
		/// </summary>
		public bool IsLocal => !IsGlobal;

		public bool IsValueNull {
			get {
				if (string.IsNullOrWhiteSpace(Number))
					return true;
				if (IsLocal && string.IsNullOrWhiteSpace(Context))
					return true;
				return false;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Construct an instance of the Tel URI data type
		/// </summary>
		/// <param name="number">The global phone number.</param>
		public Tel(string number) : this(number, null, false, null) { }

		/// <summary>
		/// Construct an instance of the Tel URI data type
		/// </summary>
		/// <param name="number">The phone number</param>
		/// <param name="extension">The extension or ISDN subaddress for the target</param>
		/// <param name="isExtension">A boolean indicating whether the <paramref name="extension"/> is an Extension (true) or ISDN subaddress (false)</param>
		/// <param name="context">The contact for a local number (if the number is local, the default context of "+313" is used.</param>
		public Tel(string number, string extension, bool isExtension = true, string context = null) {
			if (string.IsNullOrWhiteSpace(number))
				throw new ArgumentNullException(nameof(number));

			Number = number;

			if (!string.IsNullOrWhiteSpace(context))
				Context = context;
			else {
				if (IsLocal)
					Context = DEFAULT_CONTEXT;
				Context = null;
			}

			if (string.IsNullOrWhiteSpace(extension)) {
				Extension = null;
			} else {
				if (isExtension)
					Extension = extension.Trim();
				else
					IsdnSubAddress = extension.Trim();
			}
		}

		/// <summary>
		/// Construct an instance of the Tel URI data type
		/// </summary>
		/// <param name="uriPath">Either the LocalPath or AbsolutePath from a "tel:" URI</param>
		/// <param name="otherParameters">Returns any additional, unrecognized URI parameters.</param>
		public Tel(string uriPath, out List<UriParameter> otherParameters) {
			if (string.IsNullOrWhiteSpace(uriPath))
				throw new ArgumentNullException(nameof(uriPath));
			if (!uriPath.Contains(";")) {
				Number = uriPath;
				otherParameters = null;
				return;
			}

			List<string> split1 = uriPath.NonQuotedSplit(';', StringSplitOptions.RemoveEmptyEntries, true);
			Number = split1[0];
			otherParameters = new List<UriParameter>();
			for (int i = 1; i < split1.Count; i++) {
				UriParameter parameter = new UriParameter(split1[i]);
				if (parameter.Is("isub"))
					IsdnSubAddress = parameter.Value;
				else if (parameter.Is("ext"))
					Extension = parameter.Value;
				else if (parameter.Is("phone-context"))
					Context = parameter.Value;
				else
					otherParameters.Add(parameter);
			}

			if (otherParameters.Count < 1)
				otherParameters = null;
		}
		#endregion

		public override string ToString() {
			string res = Number;
			if (IsLocal)
				res += new UriParameter("phone-context", Context).ToString();
			if (!string.IsNullOrWhiteSpace(Extension))
				res += new UriParameter("ext", Extension).ToString();
			else if (!string.IsNullOrWhiteSpace(IsdnSubAddress))
				res += new UriParameter("isub", IsdnSubAddress).ToString();
			return res;
		}
	}
}