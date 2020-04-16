using System;
using System.Collections.Generic;

using System.Globalization;

using Xrd.Text;
using Xrd.TypedUri;

namespace Xrd.vCard.Props {
	#region vCard 4.0 / 6.1.x
	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.1.1.BEGIN Property.
	/// To denote the beginning of a syntactic entity within a text/vcard content-type.
	/// https://tools.ietf.org/html/rfc6350#section-6.1.1
	/// </summary>
	/// <remarks>
	/// The content entity MUST begin with the BEGIN property with a value of "VCARD". The value is case-insensitive.
	/// The property value is implemented as a single string and set to "VCARD" automatically, regardless of what other value may be set.
	/// </remarks>
	public class BEGIN : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public BEGIN() {
			SetProperty(KnownProperties.BEGIN);
			Value = "VCARD";
		}

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public BEGIN(string input) : base(input) {
			Value = "VCARD";
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.1.2.END Property.
	/// To denote the end of a syntactic entity within a text/vcard content-type.
	/// https://tools.ietf.org/html/rfc6350#section-6.1.2
	/// </summary>
	/// <remarks>
	/// The content entity MUST end with the END tpye with a value of "VCARD". The value is case-insensitive.
	/// The property value is implemented as a single string and set to "VCARD" automatically, regardless of what other value may be set.
	/// </remarks>
	public class END : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public END() {
			SetProperty(KnownProperties.END);
			Value = "VCARD";
		}

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public END(string input) : base(input) {
			Value = "VCARD";
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.1.3.SOURCE Property.
	/// To identify the source of directory information contained in the content type.
	/// https://tools.ietf.org/html/rfc6350#section-6.1.3
	/// </summary>
	/// <remarks>
	/// The SOURCE property is used to provide the means by which applications knowledgeable in the given directory service protocol can obtain additional or more up-to-date
	///		information from the directory service.
	/// The property value is implemented as a System.Uri
	/// </remarks>
	public class SOURCE : PropertyBase<Uri> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public SOURCE() {
			SetProperty(KnownProperties.SOURCE);
		}
		internal SOURCE(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public SOURCE(string input) : base(input) { }

		/// <summary>
		/// Check to determine if the specified text input is a valid value
		/// </summary>
		/// <param name="input">The text to check</param>
		/// <returns>boolean</returns>
		protected override bool IsValueAcceptable(string input) {
			return Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute);
		}

		/// <summary>
		/// Parse the specified text input
		/// </summary>
		/// <param name="input">text to parse</param>
		/// <returns>a strongly-typed instance.</returns>
		protected override object ParseValue(string input) {
			return new Uri(input);
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.1.4.KIND Property.
	/// To specify the kind of object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.1.4
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single string restricted to the following set "individual", "group", "org", "location"
	/// </remarks>
	public class KIND : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public KIND() {
			SetProperty(KnownProperties.KIND);
		}
		internal KIND(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public KIND(string input) : base(input) { }

		/// <summary>
		/// List of acceptable values
		/// </summary>
		public override List<string> AcceptableValues {
			get {
				return new List<string>() {
					"individual",
					"group",
					"org",
					"location"
				};
			}
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.1.5.XML Property.
	/// To include extended XML-encoded vCard data in a plain vCard.
	/// https://tools.ietf.org/html/rfc6350#section-6.1.5
	/// </summary>
	/// <remarks>The property value is implemented as a single string. No validation is implemented, as .NET core does not contain XML validation at this time.</remarks>
	public class XML : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public XML() {
			SetProperty(KnownProperties.XML);
		}
		internal XML(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public XML(string input) : base(input) { }
	}
	#endregion

	#region vCard 4.0 / 6.2.x
	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.2.1.FN Property (formatted name).
	/// To specify the formatted text corresponding to the name of the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.2.1
	/// </summary>
	/// <remarks>The property value is implemented as a single string.</remarks>
	public class FN : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public FN() {
			SetProperty(KnownProperties.FN);
		}
		internal FN(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public FN(string input) : base(input) { }
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.2.2.N Property.
	/// To specify the components of the name of the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.2.2
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a list of strings separated by a SEMICOLON. The fixed indexes of each component are:
	///		0 = Family Names (surnames)
	///		1 = Given Names (first names)
	///		2 = Additional Names (middle names)
	///		3 = Honorific Prefixes (e.g. "Dr.", "Ms.")
	///		4 = Honorific Suffixes (e.g. "Sr.", "M.D.")
	///	Each component can consist of a sub-list separated by a COMMA. However, that level of details is not implemented in this reader, and should be implemented at the client side.
	///	This reader implementation simply treats each component as a single string value.
	/// </remarks>
	public class N : ListStringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public N() : base() {
			SetProperty(KnownProperties.N);
		}
		internal N(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public N(string input) : base(input) { }

		/// <summary>
		/// Check to determin if the specified text input is an acceptable value
		/// </summary>
		/// <param name="input">The text to check</param>
		/// <returns>boolean</returns>
		protected override bool IsValueAcceptable(string input) {
			// As long as the Value list has no more than 5 entries, accept anything, even nulls.
			return Value.Count < 6;
		}

		/// <summary>
		/// The family (last) portion of the name
		/// </summary>
		public string FamilyName {
			get { return this[0]; }
			set { this[0] = value; }
		}

		/// <summary>
		/// The given (first) portion of the name
		/// </summary>
		public string GivenName {
			get { return this[1]; }
			set { this[1] = value; }
		}

		/// <summary>
		/// Additional (middle) names
		/// </summary>
		public string AdditionalName {
			get { return this[2]; }
			set { this[2] = value; }
		}

		/// <summary>
		/// The honorific prefix (Dr., Mr., Ms., etc.)
		/// </summary>
		public string HonorificPrefix {
			get { return this[3]; }
			set { this[3] = value; }
		}

		/// <summary>
		/// The honorific suffice (Sr., FAACP, etc.
		/// </summary>
		public string HonorificSuffix {
			get { return this[4]; }
			set { this[4] = value; }
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.2.3.NICKNAME Property.
	/// To specify the text corresponding to the nickname of the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.2.3
	/// </summary>
	/// <remarks>
	/// The nickname is the descriptive name given instead of or in addition to the one belonging to the object the vCard represents. IT can also be used to specify a familiar form
	///		of a proper name specified by the FN or N properties.
	/// The property value is implemented as a list of strings separated by a COMMA.
	/// </remarks>
	public class NICKNAME : ListStringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public NICKNAME() {
			SetProperty(KnownProperties.NICKNAME);
			Value = new List<string>();
		}
		internal NICKNAME(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public NICKNAME(string input) : base(input) { }

		/// <summary>
		/// Gets the multiple-value separator.
		/// </summary>
		public override char VALUE_SEPARATOR { get { return ','; } }
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.2.4.PHOTO Property.
	/// To specify an image or photograph information that annotes some aspect of the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.2.4
	/// </summary>
	/// <remarks>
	/// The property is implemented as a Uri, with an optional conversion to <see cref="DataUri"/>
	/// </remarks>
	public class PHOTO : PropertyBase<Uri> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public PHOTO() {
			SetProperty(KnownProperties.PHOTO);
		}
		internal PHOTO(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public PHOTO(string input) : base(input) { }

		/// <summary>
		/// Check to determine if specified text input is valid.
		/// </summary>
		/// <param name="input">Text to check</param>
		/// <returns>Boolean</returns>
		protected override bool IsValueAcceptable(string input) {
			return Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute);
		}

		/// <summary>
		/// Parse the specifed text input.
		/// </summary>
		/// <param name="input">Text to parse</param>
		/// <returns>A strongly-typed instance</returns>
		protected override object ParseValue(string input) {
			if (Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute))
				return new Uri(input);
			return null;
		}

		public DataUri Data {
			get {
				if (!DataUri.IsWellFormedString(Value.ToString()))
					return null;

				try {
					return new DataUri(Value);
				} catch {
					return null;
				}
			}
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.2.5.BDAY Property.
	/// To specify the birth date of the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.2.5
	/// </summary>
	/// <remarks>The property value is implemented as a single nullable DateTime.</remarks>
	public class BDAY : PropertyBase<DateTime?> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public BDAY() {
			SetProperty(KnownProperties.BDAY);
		}

		internal BDAY(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public BDAY(string input) : base(input) { }

		/// <summary>
		/// Generate a BDay property from the specified text input.
		/// </summary>
		/// <param name="input">The text to parse</param>
		/// <returns>A BDay property instance.</returns>
		protected override object ParseValue(string input) {
			DateTime? res = IETFValueEncoding.ParseISO8601String(input);
			if (res == null)
				Value = null;
			return res;
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.2.6.ANNIVERSARY Property.
	/// The date of marriage, or equivalent, of the object that the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.2.6
	/// </summary>
	/// <remarks>The property value is implemented as a single nullable DateTime.</remarks>
	public class ANNIVERSARY : PropertyBase<DateTime?> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public ANNIVERSARY() {
			SetProperty(KnownProperties.ANNIVERSARY);
		}

		internal ANNIVERSARY(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public ANNIVERSARY(string input) : base(input) { }

		/// <summary>
		/// Returns an Anniversary property object.
		/// </summary>
		/// <param name="input">The text to parse.</param>
		/// <returns>An Anniversary object represented by the text.</returns>
		protected override object ParseValue(string input) {
			DateTime? res = IETFValueEncoding.ParseISO8601String(input);
			if (res == null)
				Value = null;
			return res;
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.2.7.GENDER Property.
	/// To specify the components of the sex and gender identity of the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.2.7
	/// </summary>
	/// <remarks>
	/// The components correspond, in sequence, to the sex (biological), and gender identity. Each component is optional.
	/// The property value is implemented as a List of strings.
	/// </remarks>
	public class GENDER : ListStringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public GENDER() {
			SetProperty(KnownProperties.GENDER);
		}
		internal GENDER(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public GENDER(string input) : base(input) { }

		/// <summary>
		/// Check to determine if specified text input is valid
		/// </summary>
		/// <param name="input">Text to check</param>
		/// <returns>boolean</returns>
		protected override bool IsValueAcceptable(string input) {
			return Value.Count < 3;
		}

		/// <summary>
		/// List of "acceptable" sexes defined in the standard
		/// </summary>
		public static List<string> OK_SEXES = new List<string>() { "", "M", "F", "O", "N", "U" };

		/// <summary>
		/// Labels to use for the defined "OK_SEXES"
		/// </summary>
		public static List<string> SEX_DESCRIPTIONS = new List<string>() { "", "male", "female", "other", "none or not applicable", "unknown" };

		/// <summary>
		/// Get/set the SEX portion of the property value.
		/// </summary>
		public string Sex {
			get {
				if (string.IsNullOrWhiteSpace(this[0]))
					return string.Empty;

				string temp = this[0].ToUpper();
				if (OK_SEXES.Contains(temp))
					return temp;
				return string.Empty;
			}
			set {
				if (string.IsNullOrWhiteSpace(value))
					this[0] = string.Empty;
				else if (OK_SEXES.Contains(value.ToUpper()))
					this[0] = value.ToUpper();
				else
					this[0] = string.Empty;
			}
		}

		/// <summary>
		/// Get/set the Gender identity portion of the property value.
		/// </summary>
		public string GenderIdentity {
			get { return this[1]; }
			set { this[1] = value; }
		}
	}
	#endregion

	#region vCard 4.0 / 6.3.x
	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.3.1.ADR Property.
	/// To specify the components of the delivery address for vCard object.
	/// https://tools.ietf.org/html/rfc6350#section-6.3.1
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a list of strings separated by a SEMICOLON. The fixed indexes of each component are:
	///		0 = Post Office Box (deprecated)
	///		1 = Extendeed Address (e.g. apartment or suite number) (deprecated)
	///		2 = Street Address
	///		3 = Locality (e.g. city)
	///		4 = Region (e.g. state or province)
	///		5 = postal code
	///		6 = country (full name in the language specified in section 5.1)
	///	Certain components can consist of a sub-list separated by a COMMA (e.g. street address). However, that level of detail is not implemented in this reader, and should be implemented at the client side.
	///	This reader implementation simply treats each component as a single string value.
	/// </remarks>
	public class ADR : ListStringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public ADR() : base() {
			SetProperty(KnownProperties.ADR);
		}

		internal ADR(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public ADR(string input) : base(input) { }

		/// <summary>
		/// Determine if the value to be set is acceptable.
		/// </summary>
		/// <param name="input">the value to add.</param>
		/// <returns>A boolean indicating whether or not a value can be added</returns>
		protected override bool IsValueAcceptable(string input) {
			// As long as the Value list has no more than 8 entries, accept anything, even nulls.
			return Value.Count < 8;
		}

		/// <summary>
		/// DEPRECATED. The PO Box portion of the address.
		/// </summary>
		public string DEPRECATED_PoBox {
			get { return this[0]; }
		}

		/// <summary>
		/// DEPRECATED. The Extended portion of the address.
		/// </summary>
		public string DEPRECATED_ExtAdd {
			get { return this[1]; }
		}

		/// <summary>
		/// The street portion of the address.
		/// </summary>
		public string Street {
			get { return this[2]; }
			set { this[2] = value; }
		}

		/// <summary>
		/// The city/locality portion of the address.
		/// </summary>
		public string City {
			get { return this[3]; }
			set { this[3] = value; }
		}

		/// <summary>
		/// The state/province/region portion of the address.
		/// </summary>
		public string State {
			get { return this[4]; }
			set { this[4] = value; }
		}

		/// <summary>
		/// The postal code of the address.
		/// </summary>
		public string PostalCode {
			get { return this[5]; }
			set { this[5] = value; }
		}

		/// <summary>
		/// The country portion of the address.
		/// </summary>
		public string Country {
			get { return this[6]; }
			set { this[6] = value; }
		}
	}
	#endregion

	#region vCard 4.0 / 6.4.x
	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.4.1.TEL Property.
	/// To specify the telephone number for telephony communications with the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.4.1
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single TelUri (using the "tel:" schema). 
	/// Even though the spec defines the value as a free-form text value for backwards compatibility, the spec recommends that it SHOULD be reset to a URI value (which this code will) 
	/// </remarks>
	public class TEL : PropertyBase<TelUri> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public TEL() {
			SetProperty(KnownProperties.TEL);
		}
		internal TEL(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public TEL(string input) : base(input) { }

		/// <summary>
		/// The list of acceptable values.
		/// </summary>
		public override List<string> AcceptableValues {
			get { return new List<string>() { string.Empty, "tel:*" }; }
		}

		protected override bool IsValueAcceptable(string input) {
			if (!TelUri.IsWellFormedString(input))
				return false;
			return base.IsValueAcceptable(input);
		}

		/// <summary>
		/// Parse the specified text input.
		/// </summary>
		/// <param name="input">Text to parse</param>
		/// <returns>a strongly-typed instance.</returns>
		protected override object ParseValue(string input) {
			if (TelUri.TryParse(input, out TelUri result))
				return result;
			return null;
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.4.2.EMAIL Property.
	/// To specify the electronic mail address for communication with the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.4.2
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single string. 
	/// </remarks>
	public class EMAIL : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public EMAIL() {
			SetProperty(KnownProperties.EMAIL);
		}
		internal EMAIL(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public EMAIL(string input) : base(input) { }

		/// <summary>
		/// Check to determine if specified text input is valid
		/// </summary>
		/// <param name="input">Text to check</param>
		/// <returns>Boolean indicating whether or not input is valid.</returns>
		protected override bool IsValueAcceptable(string input) {
			return input.IsValidEmail();
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.4.3.IMPP Property.
	/// To specify the URI for instant messaging and presence protocol communications with the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.4.3
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single Uri (using the "xmpp:" schema). 
	/// </remarks>
	public class IMPP : PropertyBase<Uri> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public IMPP() {
			SetProperty(KnownProperties.TEL);
		}
		internal IMPP(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public IMPP(string input) : base(input) { }

		/// <summary>
		/// List of acceptable values.
		/// </summary>
		public override List<string> AcceptableValues {
			get { return new List<string>() { "xmpp:*" }; }
		}

		/// <summary>
		/// Parse the specified text input.
		/// </summary>
		/// <param name="input">The text to parse</param>
		/// <returns>a strongly-typed instance</returns>
		protected override object ParseValue(string input) {
			if (Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute))
				return new Uri(input);
			return null;
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.4.4.LANG Property.
	/// To specify the language(s) that may be used for contacting the entity associated with the vCard.
	/// https://tools.ietf.org/html/rfc6350#section-5.1
	/// </summary>
	/// <remarks>The parameter value is recognized as a System.Globalization.CultureInfo value.</remarks>
	public class LANG : PropertyBase<CultureInfo> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public LANG() {
			SetProperty(KnownProperties.LANG);
			Value = CultureInfo.DefaultThreadCurrentCulture;
		}
		internal LANG(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public LANG(string input) : base(input) { }

		/// <summary>
		/// Parse the specified text input.
		/// </summary>
		/// <param name="input">The text to parse.</param>
		/// <returns>A strongly-typed instance.</returns>
		protected override object ParseValue(string input) {
			try {
				return new CultureInfo(input);
			} catch {
				return CultureInfo.DefaultThreadCurrentCulture;
			}
		}

		/// <summary>
		/// Gets the LangTag for the value.
		/// </summary>
		public string LangTag {
			get {
				if (Value == null)
					return string.Empty;
				return Value.Name;
			}
		}
	}
	#endregion

	#region vCard 4.0 / 6.5.x
	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.5.1.TZ Property.
	/// To specify information related to the time zone of the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.5.1
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single string. 
	/// Even though the spec allows the value to be either text, URI or utc-offset, this reader will simply treat it as a string. 
	/// </remarks>
	public class TZ : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public TZ() {
			SetProperty(KnownProperties.TZ);
		}
		internal TZ(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public TZ(string input) : base(input) { }
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.5.2.GEO Property.
	/// To specify information related to the global positioning of the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.5.2
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a "geo" uri <see cref="GeoUri"/>. 
	/// </remarks>
	public class GEO : PropertyBase<GeoUri> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public GEO() {
			SetProperty(KnownProperties.GEO);
		}
		internal GEO(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public GEO(string input) : base(input) { }

		/// <summary>
		/// Check to determine if specified text input is valid.
		/// </summary>
		/// <param name="input">Text to check</param>
		/// <returns>Boolean</returns>
		protected override bool IsValueAcceptable(string input) {
			return GeoUri.IsWellFormedString(input);
		}

		/// <summary>
		/// Parse the specifed text input.
		/// </summary>
		/// <param name="input">Text to parse</param>
		/// <returns>A strongly-typed instance</returns>
		protected override object ParseValue(string input) {
			if (GeoUri.TryParse(input, out GeoUri result))
				return result;
			return null;
		}
	}
	#endregion

	#region vCard 4.0 / 6.6.x
	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.6.1.TITLE Property.
	/// To specify the position or job of the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.6.1
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single string. 
	/// </remarks>
	public class TITLE : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public TITLE() {
			SetProperty(KnownProperties.TITLE);
		}
		internal TITLE(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public TITLE(string input) : base(input) { }
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.6.2.ROLE Property
	/// To specify the function or part played in a particular situation by the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.6.2
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single string. 
	/// </remarks>
	public class ROLE : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public ROLE() {
			SetProperty(KnownProperties.ROLE);
		}
		internal ROLE(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public ROLE(string input) : base(input) { }
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.6.3.LOGO Property.
	/// To specify an graphic image of a logo associated with the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.6.3
	/// </summary>
	/// <remarks>
	/// The property is implemented as a Uri, with an optional conversion to <see cref="DataUri"/>
	/// </remarks>
	public class LOGO : PropertyBase<Uri> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public LOGO() {
			SetProperty(KnownProperties.LOGO);
		}
		internal LOGO(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public LOGO(string input) : base(input) { }

		/// <summary>
		/// Check to determine if specified text input is valid.
		/// </summary>
		/// <param name="input">Text to check</param>
		/// <returns>Boolean</returns>
		protected override bool IsValueAcceptable(string input) {
			return Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute);
		}

		/// <summary>
		/// Parse the specifed text input.
		/// </summary>
		/// <param name="input">Text to parse</param>
		/// <returns>A strongly-typed instance</returns>
		protected override object ParseValue(string input) {
			if (Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute))
				return new Uri(input);
			return null;
		}

		public DataUri Data {
			get {
				if (!DataUri.IsWellFormedString(Value.ToString()))
					return null;

				try {
					return new DataUri(Value);
				} catch {
					return null;
				}
			}
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.6.4.ORG Property
	/// To specify the organizational name and unit(s) associated with the object the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.6.4
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a List of strings. 
	/// </remarks>
	public class ORG : PropertyBase<List<string>> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public ORG() {
			SetProperty(KnownProperties.ORG);
		}
		internal ORG(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public ORG(string input) : base(input) { }
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.6.5.MEMBER Property
	/// To include a member in the group this vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.6.5
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single URI.
	/// This property MUST NOT be present unless the value of the KIND property is "group"
	/// </remarks>
	public class MEMBER : PropertyBase<UrnUri> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public MEMBER() {
			SetProperty(KnownProperties.MEMBER);
		}
		internal MEMBER(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public MEMBER(string input) : base(input) { }

		/// <summary>
		/// Check to determine if the specified text input is an acceptable value
		/// </summary>
		/// <param name="input">Text to check</param>
		/// <returns>boolean</returns>
		protected override bool IsValueAcceptable(string input) {
			return UrnUri.IsWellFormedString(input);
		}

		/// <summary>
		/// Parse the specified text input
		/// </summary>
		/// <param name="input">Text to parse</param>
		/// <returns>A strongly-typed instance.</returns>
		protected override object ParseValue(string input) {
			if (UrnUri.TryParse(input, out UrnUri result))
				return result;
			else
				return null;
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.6.6.RELATED Property
	/// To specify a relationship between another entity and the entity represented by this vCard.
	/// https://tools.ietf.org/html/rfc6350#section-6.6.6
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single string, but also has a special accessor to get the value as a URI, which is the reverse of the spec:
	///		"Value type:	A single URI. It can also be reset to a single text value. The text value can be used to specify textual information."
	/// </remarks>
	public class RELATED : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public RELATED() {
			SetProperty(KnownProperties.RELATED);
		}
		internal RELATED(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public RELATED(string input) : base(input) { }

		/// <summary>
		/// Gets a boolean indicating if the value is a URI format.
		/// </summary>
		public bool IsURI {
			get { return Uri.IsWellFormedUriString(Value, UriKind.RelativeOrAbsolute); }
		}

		/// <summary>
		/// Gets the value as a URI (if possible)
		/// </summary>
		public Uri ValueAsUri {
			get {
				if (IsURI)
					return new Uri(Value, UriKind.RelativeOrAbsolute);
				else
					return null;
			}
		}

		public UrnUri UrnUri {
			get {
				if (UrnUri.TryParse(Value, out UrnUri result))
					return result;
				return null;
			}
		}
	}
	#endregion

	#region vCard 4.0 / 6.7.x
	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.7.1.CATEGORIES Property.
	/// To specify application category information about the vCard, also known as "tags".
	/// https://tools.ietf.org/html/rfc6350#section-6.7.1
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a list of strings separated by the COMMA character. 
	/// </remarks>
	public class CATEGORIES : ListStringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public CATEGORIES() {
			SetProperty(KnownProperties.CATEGORIES);
		}
		internal CATEGORIES(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public CATEGORIES(string input) : base(input) { }

		/// <summary>
		/// The name/value separator character.
		/// </summary>
		public override char VALUE_SEPARATOR {
			get { return ','; }
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.7.2.NOTES Property.
	/// To specify supplemental information or a comment that is associated with the vCard.
	/// https://tools.ietf.org/html/rfc6350#section-6.7.2
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single string. 
	/// </remarks>
	public class NOTE : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public NOTE() {
			SetProperty(KnownProperties.NOTE);
		}
		internal NOTE(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public NOTE(string input) : base(input) { }
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.7.3.PRODID Property.
	/// To specify the identifier for the product that create the cVard object.
	/// https://tools.ietf.org/html/rfc6350#section-6.7.3
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single string. 
	/// </remarks>
	public class PRODID : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public PRODID() {
			SetProperty(KnownProperties.PRODID);
		}
		internal PRODID(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public PRODID(string input) : base(input) { }
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.7.4.REV Property.
	/// To specify the identifier for the product that create the cVard object.
	/// https://tools.ietf.org/html/rfc6350#section-6.7.4
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single DateTime. 
	/// </remarks>
	public class REV : PropertyBase<DateTime> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public REV() {
			SetProperty(KnownProperties.REV);
		}
		internal REV(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public REV(string input) : base(input) { }

		/// <summary>
		/// Check to determine if the specified text input is a valid value
		/// </summary>
		/// <param name="input">The text to check</param>
		/// <returns>boolean</returns>
		protected override bool IsValueAcceptable(string input) {
			return IETFValueEncoding.ParseISO8601String(input) != null;
		}

		/// <summary>
		/// Parse the specified text input
		/// </summary>
		/// <param name="input">Text to parse</param>
		/// <returns>a strongly-typed instance</returns>
		protected override object ParseValue(string input) {
			return IETFValueEncoding.ParseISO8601String(input);
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.7.5.SOUND Property.
	/// To specify a digital sound content information that annotates some aspect of the vCard. This property is often used to specify the proper pronunciation of the name 
	///		property value of the vCard.
	///	https://tools.ietf.org/html/rfc6350#section-6.7.5
	/// </summary>
	/// <remarks>
	/// The property is implemented as a Uri, with an optional conversion to <see cref="DataUri"/>
	/// </remarks>
	public class SOUND : PropertyBase<Uri> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public SOUND() {
			SetProperty(KnownProperties.SOUND);
		}
		internal SOUND(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public SOUND(string input) : base(input) { }

		/// <summary>
		/// Check to determine if specified text input is valid.
		/// </summary>
		/// <param name="input">Text to check</param>
		/// <returns>Boolean</returns>
		protected override bool IsValueAcceptable(string input) {
			return Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute);
		}

		/// <summary>
		/// Parse the specifed text input.
		/// </summary>
		/// <param name="input">Text to parse</param>
		/// <returns>A strongly-typed instance</returns>
		protected override object ParseValue(string input) {
			if (Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute))
				return new Uri(input);
			return null;
		}

		public DataUri Data {
			get {
				if (!DataUri.IsWellFormedString(Value.ToString()))
					return null;

				try {
					return new DataUri(Value);
				} catch {
					return null;
				}
			}
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.7.6.UID Property
	/// To specify a relationship between another entity and the entity represented by this vCard.
	/// https://tools.ietf.org/html/rfc6350#section-6.7.6
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single string, but also has a special accessor to get the value as a URI, which is the reverse of the spec:
	///		"Value type:	A single URI. It can also be reset to a single text value. The text value can be used to specify textual information."
	/// </remarks>
	public class UID : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public UID() {
			SetProperty(KnownProperties.UID);
		}
		internal UID(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public UID(string input) : base(input) { }

		/// <summary>
		/// Boolean indicating whether or not the value is a UrnURI
		/// </summary>
		public bool IsURI {
			get { return UrnUri.IsWellFormedString(Value); }
		}

		/// <summary>
		/// Gets the value as a URI (if possible)
		/// </summary>
		public UrnUri ValueAsUri {
			get {
				if (UrnUri.TryParse(Value, out UrnUri result))
					return result;
				return null;
			}
			set {
				if (value == null || value.IsValueNull)
					Value = string.Empty;
				else
					Value = value.ToString();
			}
		}

		public Guid? ValueAsGuid {
			get {
				if (ValueAsUri == null || ValueAsUri.IsValueNull)
					return null;
				return ValueAsUri.GetGuid;
			}
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.7.7.CLIENTPIDMAP Property.
	/// To give global meaning to a local PID source identifier
	/// https://tools.ietf.org/html/rfc6350#section-6.7.7
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single PIDMAP (custom class)
	///		PID source identifiers (the source identifier is the second field in a PID parameter instance) are small integers that only have significance within the scope of a single
	///		vCard instance. Each distinct source identifier present in a vCard MUST have an associated CLIENTPIDMAP.
	/// </remarks>
	public class CLIENTPIDMAP : PropertyBase<Params.PID_Map> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public CLIENTPIDMAP() {
			SetProperty(KnownProperties.CLIENTPIDMAP);
		}

		internal CLIENTPIDMAP(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public CLIENTPIDMAP(string input) : base(input) { }

		/// <summary>
		/// Check to see if specified text input is a valid value.
		/// </summary>
		/// <param name="input">The text to check</param>
		/// <returns>Boolean indicating whether or not input is valid</returns>
		protected override bool IsValueAcceptable(string input) {
			return Params.PID_Map.IsWellFormedString(input);
		}

		/// <summary>
		/// Parse the specified text input.
		/// </summary>
		/// <param name="input">The text to parse.</param>
		/// <returns>A strongly-typed instance.</returns>
		protected override object ParseValue(string input) {
			if (!Params.PID_Map.IsWellFormedString(input))
				return null;
			return new Params.PID_Map(input);
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.7.8.URL Property
	/// To specify a uniform resource locator associated with the object to which the vCard refers. Examples for individuals include personal web sites, blogs, and social
	/// networking site identifiers.
	/// https://tools.ietf.org/html/rfc6350#section-6.7.8
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single URI.
	/// </remarks>
	public class URL : PropertyBase<Uri> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public URL() {
			SetProperty(KnownProperties.URL);
		}
		internal URL(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public URL(string input) : base(input) { }

		/// <summary>
		/// Check to determine if specified text input is a valid value.
		/// </summary>
		/// <param name="input">Text to check</param>
		/// <returns>duh</returns>
		protected override bool IsValueAcceptable(string input) {
			return Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute);
		}

		/// <summary>
		/// Parse the specified text input
		/// </summary>
		/// <param name="input">Text to parse</param>
		/// <returns>a strongly-typed instance.</returns>
		protected override object ParseValue(string input) {
			if (Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute))
				return new Uri(input);
			return null;
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.7.9.VERSION Property
	/// To specify the version of the vCard specification used to format this vCard.
	/// https://tools.ietf.org/html/rfc6350#section-6.7.9
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single string.
	///		This property MUST be present in the vCard object, and it must appear immediately after BEGIN:VCARD. The value MUST be "4.0" if the vCard corresponds to this specification.
	///		Note that earlier versions of vCard allowed this property to be placed anywhere in the vCard object, or even to be absent.
	/// </remarks>
	public class VERSION : StringProperty {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public VERSION() {
			SetProperty(KnownProperties.VERSION);
			Value = "4.0";
		}
		internal VERSION(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public VERSION(string input) : base(input) { }
	}
	#endregion

	#region vCard 4.0 / 6.8.x
	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.8.1.KEY Property
	/// To specify the public key used by the entity represented by this vCard.
	/// https://tools.ietf.org/html/rfc6350#section-6.8.1
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a URI, with an optional conversion to <see cref="DataUri"/>
	/// </remarks>
	public class KEY : PropertyBase<Uri> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public KEY() {
			SetProperty(KnownProperties.KEY);
		}
		internal KEY(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public KEY(string input) : base(input) { }

		/// <summary>
		/// Check to determine if specified text input is valid.
		/// </summary>
		/// <param name="input">Text to check</param>
		/// <returns>Boolean</returns>
		protected override bool IsValueAcceptable(string input) {
			return Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute);
		}

		/// <summary>
		/// Parse the specifed text input.
		/// </summary>
		/// <param name="input">Text to parse</param>
		/// <returns>A strongly-typed instance</returns>
		protected override object ParseValue(string input) {
			if (Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute))
				return new Uri(input);
			return null;
		}

		public DataUri Data {
			get {
				if (!DataUri.IsWellFormedString(Value.ToString()))
					return null;

				try {
					return new DataUri(Value);
				} catch {
					return null;
				}
			}
		}
	}
	#endregion

	#region vCard 4.0 / 6.9.x
	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.9.1.FBURL Property
	/// To specify the URI for the free/busy time associated with the object that the vCard represents.
	/// https://tools.ietf.org/html/rfc6350#section-6.9.1
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single URI.
	///		Where multiple FBURL properties are specified, the default FBURL property is indicated with the PREF parameter. The FTP or HTTP type of URI points to an iCalendar 
	///		[RFC5545] https://tools.ietf.org/html/rfc5545 object associated with a snapshot of the next few weeks or months of busy time data. If the iCalendar object
	///		is represented as a file or document, its file extension should be ".ifb"
	/// </remarks>
	public class FBURL : PropertyBase<Uri> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public FBURL() {
			SetProperty(KnownProperties.FBURL);
		}
		internal FBURL(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public FBURL(string input) : base(input) { }

		/// <summary>
		/// Check to determine if specified text input is valid.
		/// </summary>
		/// <param name="input">Text to check</param>
		/// <returns>Boolean</returns>
		protected override bool IsValueAcceptable(string input) {
			return Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute);
		}

		/// <summary>
		/// Parse the specified text input
		/// </summary>
		/// <param name="input">Text to parse</param>
		/// <returns>a strongly typed instance.</returns>
		protected override object ParseValue(string input) {
			if (Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute))
				return new Uri(input);
			return null;
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.9.2.CALADRURI Property
	/// To specify the calendar user address to which a scheduling request should be sent for the object represented by the vCard.
	/// https://tools.ietf.org/html/rfc6350#section-6.9.2
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single URI.
	///		Where multiple CALADRURI properties are specified, the default CALADRURI property is indicated with the PREF parameter.
	/// </remarks>
	public class CALADRURI : PropertyBase<Uri> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public CALADRURI() {
			SetProperty(KnownProperties.CALADRURI);
		}

		internal CALADRURI(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public CALADRURI(string input) : base(input) { }

		/// <summary>
		/// Determine if the specified text input is a valid value
		/// </summary>
		/// <param name="input">The text check</param>
		/// <returns>a boolean indicating whether or not the input is a valid value.</returns>
		protected override bool IsValueAcceptable(string input) {
			return Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute);
		}

		/// <summary>
		/// Return an instance of the CALADURI property from the specified text input.
		/// </summary>
		/// <param name="input">The text to parse</param>
		/// <returns>A strongly typed property instance</returns>
		protected override object ParseValue(string input) {
			if (Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute))
				return new Uri(input);
			return null;
		}
	}

	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.9.3.CALURI Property
	/// To specify the URI for a calendar associated with the object represented by the vCard.
	/// https://tools.ietf.org/html/rfc6350#section-6.9.3
	/// </summary>
	/// <remarks>
	/// The property value is implemented as a single URI.
	///		Where multiple CALURI properties are specified, the default CALURI property is indicated with the PREF parameter. The property should contain a URI pointing to an 
	///		iCalendar [RFC5545] https://tools.ietf.org/html/rfc5545 object associated with a snapshot of the user's calendar store. If the iCalendar object is 
	///		represented as a file or document, its file extension should be ".ics"
	/// </remarks>
	public class CALURI : PropertyBase<Uri> {
		/// <summary>
		/// Creates a new instance of an empty Property.
		/// </summary>
		public CALURI() {
			SetProperty(KnownProperties.CALURI);
		}

		internal CALURI(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Creates a new instance of the Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public CALURI(string input) : base(input) { }

		/// <summary>
		/// Check to see if specified text input is valid
		/// </summary>
		/// <param name="input">Text to check</param>
		/// <returns>A boolean indicating whether or not input is valid.</returns>
		protected override bool IsValueAcceptable(string input) {
			return Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute);
		}

		/// <summary>
		/// Parse the specified text input.
		/// </summary>
		/// <param name="input">The text to parse</param>
		/// <returns>a strongly-typed instance</returns>
		protected override object ParseValue(string input) {
			if (Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute))
				return new Uri(input);
			return null;
		}
	}
	#endregion

}