using System;
using System.Collections.Generic;
using System.Text;
using Xrd.TypedUri;

namespace Xrd.vCard {
	internal static class EnumExtensions {
		internal static string GetvCardName(this KnownParameters value) {
			switch (value) {
				case KnownParameters.no_name_provided:
				case KnownParameters.unknown:
				case KnownParameters.read_not_parsed_yet:
					return string.Empty;
				case KnownParameters.SORT_AS:
					return "SORT-AS";
				default:
					return value.ToString();
			}
		}

		internal static string GetvCardName(this KnownProperties value) {
			switch (value) {
				case KnownProperties.read_not_parsed_yet:
				case KnownProperties.extended:
				case KnownProperties.unknown:
					return string.Empty;
				default:
					return value.ToString();
			}
		}

		internal static KnownParameters ParseParameter(string input) {
			if (string.IsNullOrWhiteSpace(input))
				return KnownParameters.no_name_provided;
			input = input.ToUpper();
			if (input.Equals("SORT-AS"))
				return KnownParameters.SORT_AS;
			try {
				return (KnownParameters)Enum.Parse(typeof(KnownParameters), input);
			} catch {
				return KnownParameters.unknown;
			}
		}

		internal static KnownParameters ToKnownParameter(this string input) {
			if (string.IsNullOrWhiteSpace(input))
				return KnownParameters.no_name_provided;
			input = input.Trim().ToUpper();
			if (input.Equals("SORT-AS"))
				return KnownParameters.SORT_AS;
			try {
				return (KnownParameters)Enum.Parse(typeof(KnownParameters), input);
			} catch {
				return KnownParameters.unknown;
			}
		}

		internal static KnownProperties ToKnownProperty(this string input) {
			if (string.IsNullOrWhiteSpace(input))
				return KnownProperties.unknown;
			input = input.Trim().ToUpper();
			try {
				return (KnownProperties)Enum.Parse(typeof(KnownProperties), input);
			} catch {
				return KnownProperties.extended;
			}
		}

		internal static object NullValue(this KnownParameters param) {
			if (param == KnownParameters.VALUE)
				return new List<string>() { "text" };
			if (param == KnownParameters.PREF)
				return (byte)101;
			if (param == KnownParameters.PID)
				return new List<Params.PID_Value>() { new Params.PID_Value(0) };
			if (param == KnownParameters.GEO)
				return new GeoUri(new TypedUri.DataTypes.GeoLoc(0, 0));
			if (param == KnownParameters.SORT_AS || param == KnownParameters.TYPE)
				return new List<string>();
			if (param == KnownParameters.TZ)
				return null;
			return string.Empty;
		}

		internal static KnownParameters GetAllowedParameters(this KnownProperties prop) {
			// BEGIN | END | CLIENTPIDMAP can not have properties
			if (prop == KnownProperties.BEGIN || prop == KnownProperties.END || prop == KnownProperties.CLIENTPIDMAP)
				return KnownParameters.read_not_parsed_yet;

			// Default one param = VALUE.
			KnownParameters res = KnownParameters.VALUE;
			switch (prop) {
				case KnownProperties.KIND:
				case KnownProperties.GENDER:
				case KnownProperties.PRODID:
				case KnownProperties.REV:
				case KnownProperties.UID:
				case KnownProperties.VERSION:
					return res;
				// All other properties include ALTID param.
				default:
					res = res | KnownParameters.ALTID;
					break;
			}

			// return props with 2, 3 & 4 params
			switch (prop) {
				case KnownProperties.XML:
					return res;
				case KnownProperties.BDAY:
				case KnownProperties.ANNIVERSARY:
					return res | KnownParameters.CALSCALE;
				case KnownProperties.N:
					return res | KnownParameters.LANGUAGE | KnownParameters.SORT_AS;
				// All other properties include PID & PREF params.
				default:
					res = res |
						KnownParameters.PID |
						KnownParameters.PREF;
					break;
			}

			// return the props with 5 params.
			// base 4 = VALUE | ALTID | PID | PREF
			switch (prop) {
				// base 4 | MEDIATYPE
				case KnownProperties.SOURCE:
				case KnownProperties.MEMBER:
					return res | KnownParameters.MEDIATYPE;
				// base 4 | TYPE
				case KnownProperties.TEL:
				case KnownProperties.EMAIL:
				case KnownProperties.LANG:
				case KnownProperties.RELATED:
				case KnownProperties.CATEGORIES:
					return res | KnownParameters.TYPE;
				// All other propterties include the TYPE param.
				default:
					res = res |
						KnownParameters.TYPE;
					break;
			}

			// return the props with 6 params
			// base 5 = VALUE | ALTID | PID | PREF | TYPE
			switch (prop) {
				// base 5 | MEDIATYPE
				case KnownProperties.PHOTO:
				case KnownProperties.IMPP:
				case KnownProperties.TZ:
				case KnownProperties.GEO:
				case KnownProperties.URL:
				case KnownProperties.KEY:
				case KnownProperties.FBURL:
				case KnownProperties.CALADRURI:
				case KnownProperties.CALURI:
					return res | KnownParameters.MEDIATYPE;
				// All other properties include the LANGUAGE param.
				default:
					res = res | KnownParameters.LANGUAGE;
					break;
			}

			// base 6 = VALUE | ALTID | PID | PREF | TYPE | LANGUAGE
			switch (prop) {
				// base 6 | SORT_AS
				case KnownProperties.ORG:
					return res | KnownParameters.SORT_AS;
				// base 6 | MEDIATYPE
				case KnownProperties.LOGO:
				case KnownProperties.SOUND:
					return res | KnownParameters.MEDIATYPE;
				// base 6 | GEO | LABEL | TZ
				case KnownProperties.ADR:
					return res |
						KnownParameters.GEO |
						KnownParameters.LABEL |
						KnownParameters.TZ;
				// just the base 6
				// Should be FN, NICKNAME, TITLE, ROLE, NOTE
				default:
					return res;
			}
		}
	}
}