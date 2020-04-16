using System;
using System.Collections.Generic;
using System.Globalization;

using Xrd.Text;
using Xrd.TypedUri;

namespace Xrd.vCard.Params {
	/// <summary>
	/// A C# representation of the vCard 4.0 / 5.4.ALTID Property Parameter.
	/// https://tools.ietf.org/html/rfc6350#section-5.4
	/// </summary>
	/// <remarks>The parameter value is recognized as a single string.</remarks>
	public class ALTID : PropertyParameter<string> {
		/// <summary>
		/// Creates a new instance of an empty Property Parameter.
		/// </summary>
		public ALTID() {
			SetParameter(KnownParameters.ALTID);
		}

		public ALTID(KnownProperties property, string value) : base(KnownParameters.ALTID, property, value) { }

		/// <summary>
		/// Creates a new instance of the Property Parameter from the specified line of text.
		/// </summary>
		internal ALTID(Tuple<string, string> pair) : base(pair) { }
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 5.8.CALSCALE Property Parameter.
	/// https://tools.ietf.org/html/rfc6350#section-5.8
	/// </summary>
	/// <remarks>The parameter value is recognized as a single string.</remarks>
	public class CALSCALE : PropertyParameter<string> {
		/// <summary>
		/// Creates a new instance of an empty Property Parameter.
		/// </summary>
		public CALSCALE() {
			SetParameter(KnownParameters.CALSCALE);
		}

		public CALSCALE(KnownProperties property, string value) : base(KnownParameters.CALSCALE, property, value) { }

		/// <summary>
		/// Creates a new instance of the Property Parameter from the specified line of text.
		/// </summary>
		internal CALSCALE(Tuple<string, string> input) : base(input) { }

		/// <summary>
		/// Returns a list of strings that are acceptable input.
		/// </summary>
		public override List<string> AcceptableValues {
			get {
				if (PropertyAssignedTo == KnownProperties.BDAY || PropertyAssignedTo == KnownProperties.ANNIVERSARY)
					return new List<string>() {
						"gregorian"
					};
				else
					return new List<string>() { "NO_ACCEPTABLE_VALUES" };
			}
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 5.10.GEO Property Parameter.
	/// https://tools.ietf.org/html/rfc6350#section-5.10
	/// </summary>
	/// <remarks>
	/// The parameter value is recognized as a IQC.Data.GeoUri.
	/// https://tools.ietf.org/html/rfc5870
	/// </remarks>
	/// <seealso cref="GeoUri"/>
	public class GEO : PropertyParameter<GeoUri> {
		/// <summary>
		/// Creates a new instance of an empty Property Parameter.
		/// </summary>
		public GEO() {
			SetParameter(KnownParameters.GEO);
		}

		public GEO(KnownProperties property, GeoUri value) : base(KnownParameters.GEO, property, value) { }
		/// <summary>
		/// Creates a new instance of the Property Parameter from the specified line of text.
		/// </summary>
		internal GEO(Tuple<string, string> input) : base(input) { }

		/// <summary>
		/// Overrides the ParseInput method to specifically return a GeoUri.
		/// </summary>
		/// <param name="input">The line of text to parse</param>
		/// <returns>A GeoUri value</returns>
		protected override object ParseInput(string input) {
			if (GeoUri.TryParse(input, out GeoUri result))
				return result;
			return null;
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.3.1.ADR / LABEL Property Parameter.
	/// https://tools.ietf.org/html/rfc6350#section-6.3.1
	/// </summary>
	/// <remarks>The parameter value is recognized as a single string.</remarks>
	public class LABEL : PropertyParameter<string> {
		/// <summary>
		/// Creates a new instance of an empty Property Parameter.
		/// </summary>
		public LABEL() {
			SetParameter(KnownParameters.LABEL);
		}

		public LABEL(KnownProperties property, string value) : base(KnownParameters.LABEL, property, value) { }
		/// <summary>
		/// Creates a new instance of the Property Parameter from the specified line of text.
		/// </summary>
		internal LABEL(Tuple<string, string> input) : base(input) { }
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 5.1.LANGUAGE Property Parameter.
	/// https://tools.ietf.org/html/rfc6350#section-5.1
	/// </summary>
	/// <remarks>The parameter value is recognized as a System.Globalization.CultureInfo value.</remarks>
	public class LANGUAGE : PropertyParameter<CultureInfo> {
		/// <summary>
		/// Creates a new instance of an empty Property Parameter.
		/// </summary>
		public LANGUAGE() {
			SetParameter(KnownParameters.LANGUAGE);
			Value = CultureInfo.DefaultThreadCurrentCulture;
		}

		public LANGUAGE(KnownProperties property, CultureInfo value) : base(KnownParameters.LANGUAGE, property, value) { }

		/// <summary>
		/// Creates a new instance of the Property Parameter from the specified line of text.
		/// </summary>
		internal LANGUAGE(Tuple<string, string> input) : base(input) { }

		/// <summary>
		/// Overrides the ParseInput method to return the input as a CultureInfo object <see cref="CultureInfo"/>
		/// </summary>
		/// <param name="input">The line to be parsed.</param>
		/// <returns>A CultureInfo object</returns>
		protected override object ParseInput(string input) {
			try {
				return new CultureInfo(input);
			} catch {
				return CultureInfo.DefaultThreadCurrentCulture;
			}
		}

		/// <summary>
		/// Returns the LanguageTag for the selected language
		/// </summary>
		public string Tag {
			get {
				if (Value == null)
					return string.Empty;
				return Value.Name;
			}
		}

		/// <summary>
		/// Overrides the ToString method
		/// </summary>
		/// <returns>A string value</returns>
		public override string ToString() {
			return Parameter.GetvCardName() + Constants.PARAM_NAME_VAL_SEPARATOR + IETFValueEncoding.EncodeParameterValue(Tag);
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 5.7.MEDIATYPE Property Parameter.
	/// https://tools.ietf.org/html/rfc6350#section-5.7
	/// </summary>
	/// <remarks>The parameter value is recognized as a single string.</remarks>
	public class MEDIATYPE : PropertyParameter<string> {
		/// <summary>
		/// Creates a new instance of an empty Property Parameter.
		/// </summary>
		public MEDIATYPE() {
			SetParameter(KnownParameters.MEDIATYPE);
		}

		public MEDIATYPE(KnownProperties property, string value) : base(KnownParameters.MEDIATYPE, property, value) { }
		/// <summary>
		/// Creates a new instance of the Property Parameter from the specified line of text.
		/// </summary>
		internal MEDIATYPE(Tuple<string, string> input) : base(input) { }
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 5.5.PID Property Parameter.
	/// https://tools.ietf.org/html/rfc6350#section-5.5
	/// </summary>
	/// <remarks>The parameter value is recognized as a single PID (subclass of this parameter).</remarks>
	public class PID : PropertyParameter<List<PID_Value>> {
		/// <summary>
		/// Creates a new instance of an empty Property Parameter.
		/// </summary>
		public PID() {
			SetParameter(KnownParameters.PID);
		}

		public PID(KnownProperties property, List<PID_Value> value) : base(KnownParameters.PID, property, value) { }
		/// <summary>
		/// Creates a new instance of the Property Parameter from the specified line of text.
		/// </summary>
		internal PID(Tuple<string, string> input) : base(input) { }

		/// <summary>
		/// Overrides the ParseInput method.
		/// </summary>
		/// <param name="input">The text to be parsed</param>
		/// <returns>A PID_VALUE object.</returns>
		protected override object ParseInput(string input) {
			if (string.IsNullOrWhiteSpace(input))
				return new PID_Value(0);
			return new PID_Value(input);
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 5.3.PREF Property Parameter.
	/// https://tools.ietf.org/html/rfc6350#section-5.3
	/// </summary>
	/// <remarks>The parameter value is recognized as a byte and is restricted to between 1 and 100, inclusive.</remarks>
	public class PREF : PropertyParameter<byte> {
		/// <summary>
		/// Creates a new instance of an empty Property Parameter.
		/// </summary>
		public PREF() {
			SetParameter(KnownParameters.PREF);
		}
		public PREF(KnownProperties property, byte value) : base(KnownParameters.PREF, property, value) { }
		/// <summary>
		/// Creates a new instance of the Property Parameter from the specified line of text.
		/// </summary>
		internal PREF(Tuple<string, string> input) : base(input) { }

		public PREF(byte value) {
			SetParameter(KnownParameters.PREF);
			Value = value;
		}

		/// <summary>
		/// Overrides the ParseInput value to set the value as a byte.
		/// </summary>
		/// <remarks>If the value is null or out of range, returns 101 (outside of range) to ensure that properties without a set preference will always appear after properties 
		/// that are explicitly preferred.</remarks>
		/// <param name="input">The text to be parsed</param>
		/// <returns>The value as a byte.</returns>
		protected override object ParseInput(string input) {
			if (byte.TryParse(input, out byte v)) {
				if (v > 0 && v < 101)
					return v;
			}
			return 101;
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 5.9.SORT-AS Property Parameter.
	/// https://tools.ietf.org/html/rfc6350#section-5.9
	/// </summary>
	/// <remarks>The parameter value is recognized as a List&lt;string&gt;.</remarks>
	public class SORT_AS : PropertyParameter<List<string>> {
		/// <summary>
		/// Creates a new instance of an empty Property Parameter.
		/// </summary>
		public SORT_AS() {
			SetParameter(KnownParameters.SORT_AS);
		}
		public SORT_AS(KnownProperties property, List<string> value) : base(KnownParameters.SORT_AS, property, value) { }
		/// <summary>
		/// Creates a new instance of the Property Parameter from the specified line of text.
		/// </summary>
		internal SORT_AS(Tuple<string, string> input) : base(input) { }
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 5.6.TYPE Property Parameter.
	/// https://tools.ietf.org/html/rfc6350#section-5.6
	/// </summary>
	/// <remarks>The parameter value is recognized as a List&lt;string&gt;. Implements validation against the values specified for each property</remarks>
	public class TYPE : PropertyParameter<List<string>> {
		/// <summary>
		/// Creates a new instance of an empty Property Parameter.
		/// </summary>
		public TYPE() {
			SetParameter(KnownParameters.TYPE);
		}

		public TYPE(KnownProperties property, List<string> value) : base(KnownParameters.TYPE, property, value) { }
		/// <summary>
		/// Creates a new instance of the Property Parameter from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property Parameter</param>
		internal TYPE(Tuple<string, string> input) : base(input) { }

		public TYPE(List<string> value) {
			SetParameter(KnownParameters.TYPE);
			Value = value;
		}

		/// <summary>
		/// Overrides the ParseInput method
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		protected override object ParseInput(string input) {
			if (string.IsNullOrWhiteSpace(input))
				return null;
			return input.Trim().ToLower();
		}

		private List<KnownProperties> _noType => new List<KnownProperties>() {
			KnownProperties.BEGIN,
			KnownProperties.END,
			KnownProperties.SOURCE,
			KnownProperties.KIND,
			KnownProperties.XML,
			KnownProperties.N,
			KnownProperties.BDAY,
			KnownProperties.ANNIVERSARY,
			KnownProperties.GENDER,
			KnownProperties.MEMBER,
			KnownProperties.PRODID,
			KnownProperties.REV,
			KnownProperties.UID,
			KnownProperties.CLIENTPIDMAP,
			KnownProperties.VERSION,
			KnownProperties.read_not_parsed_yet
		};

		/// <summary>
		/// Overrides the list of acceptable values.
		/// </summary>
		public override List<string> AcceptableValues {
			get {
				if (_noType.Contains(PropertyAssignedTo))
					return null;

				List<string> res = new List<string>() {
					"home",
					"work"
				};

				if (PropertyAssignedTo == KnownProperties.TEL) {
					res.AddRange(new string[] {
						"text",
						"voice",
						"fax",
						"cell",
						"video",
						"pager",
						"textphone"
					});
				}
				if (PropertyAssignedTo == KnownProperties.RELATED) {
					res.AddRange(new string[] {
						"contact",
						"acquaintance",
						"friend",
						"met",
						"co-worker",
						"colleague",
						"co-resident",
						"neighbor",
						"child",
						"parent",
						"sibling",
						"spouse",
						"kin",
						"muse",
						"crush",
						"date",
						"sweetheart",
						"me",
						"agent",
						"emergency"
					});
				}

				return res;
			}
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 5.11.TZ Property Parameter.
	/// https://tools.ietf.org/html/rfc6350#section-5.11
	/// </summary>
	/// <remarks>The parameter value is recognized as a System.Uri.</remarks>
	public class TZ : PropertyParameter<Uri> {
		/// <summary>
		/// Creates a new instance of an empty Property Parameter.
		/// </summary>
		public TZ() {
			SetParameter(KnownParameters.TZ);
		}

		public TZ(KnownProperties property, Uri value) : base(KnownParameters.TZ, property, value) { }
		/// <summary>
		/// Creates a new instance of the Property Parameter from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property Parameter</param>
		internal TZ(Tuple<string, string> input) : base(input) { }

		/// <summary>
		/// Overrides the ParseInput method
		/// </summary>
		/// <param name="input">The text to be parsed</param>
		/// <returns>A System.Uri value if the input is a valid URI, or NULL if not.</returns>
		protected override object ParseInput(string input) {
			if (Uri.IsWellFormedUriString(input, UriKind.Absolute))
				return new Uri(input, UriKind.Absolute);
			else
				return null;
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 5.2.VALUE Property Parameter.
	/// https://tools.ietf.org/html/rfc6350#section-5.2
	/// </summary>
	/// <remarks>The parameter value is recognized as a List&lt;string&gt;.</remarks>
	public class VALUE : PropertyParameter<List<string>> {
		/// <summary>
		/// Creates a new instance of an empty Property Parameter.
		/// </summary>
		public VALUE() {
			SetParameter(KnownParameters.VALUE);
		}

		public VALUE(KnownProperties property, List<string> value) : base(KnownParameters.VALUE, property, value) { }
		/// <summary>
		/// Creates a new instance of the Property Parameter from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property Parameter</param>
		internal VALUE(Tuple<string, string> input) : base(input) { }

		/// <summary>
		/// A list of the values that are specifically defined in the vCard 4.0 standard.
		/// </summary>
		public static List<string> DEFINED_VALUES = new List<string>() {
					"text",
					"uri",
					"date",
					"time",
					"date-time",
					"date-and-or-time",
					"timestamp",
					"boolean",
					"integer",
					"float",
					"utc-offset",
					"language-tag"

		};

		/// <summary>
		/// The list of acceptable values, as determined by the Property that the Parameter applies to.
		/// </summary>
		public override List<string> AcceptableValues {
			get {
				switch (PropertyAssignedTo) {
					case KnownProperties.BEGIN:
					case KnownProperties.END:
					case KnownProperties.CLIENTPIDMAP:
						return null;
					case KnownProperties.TZ:
						return new List<string>() { "text", "uri", "utc-offset" };
					case KnownProperties.TEL:
					case KnownProperties.RELATED:
					case KnownProperties.UID:
					case KnownProperties.KEY:
						return new List<string>() { "text", "uri" };
					case KnownProperties.BDAY:
					case KnownProperties.ANNIVERSARY:
						return new List<string>() { "text", "date-and-or-time", "date", "time", "date-time" };
					case KnownProperties.LANG:
						return new List<string>() { "language-tag" };
					case KnownProperties.REV:
						return new List<string>() { "timestamp" };
					case KnownProperties.SOURCE:
					case KnownProperties.PHOTO:
					case KnownProperties.IMPP:
					case KnownProperties.GEO:
					case KnownProperties.LOGO:
					case KnownProperties.MEMBER:
					case KnownProperties.SOUND:
					case KnownProperties.URL:
					case KnownProperties.FBURL:
					case KnownProperties.CALADRURI:
					case KnownProperties.CALURI:
						return new List<string>() { "uri" };
					case KnownProperties.extended:
					case KnownProperties.read_not_parsed_yet:
					case KnownProperties.unknown:
						return DEFINED_VALUES;
					default:
						return new List<string>() { "text" };
				}
			}
		}

		/// <summary>
		/// Overrides the ParseInput method
		/// </summary>
		/// <param name="input">The text to be parsed</param>
		/// <returns>The input if it is applicable to the property, or NULL if not.</returns>
		protected override object ParseInput(string input) {
			if (!AcceptableValues.Contains(input.Trim().ToLower()))
				return null;
			switch (PropertyAssignedTo) {
				case KnownProperties.BDAY:
				case KnownProperties.ANNIVERSARY: {
						int index = AcceptableValues.IndexOf(input.Trim().ToLower());
						if (index > 1)
							index = 1;
						return AcceptableValues[index];
					}
				default:
					return input.Trim().ToLower();
			}
		}
	}
}