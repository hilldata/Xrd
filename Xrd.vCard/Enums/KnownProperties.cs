using System.ComponentModel;

namespace Xrd.vCard {
	/// <summary>
	/// List of properties defined in the standard.
	/// </summary>
	public enum KnownProperties {
		/// <summary>
		/// Indicates that the property has been read, but not parsed yet. Default placeholder value.
		/// </summary>
		[Description("Read; Not Yet Parsed")]
		read_not_parsed_yet = 0,
		/// <summary>
		/// BEGIN https://tools.ietf.org/html/rfc6350#section-6.1.1
		/// </summary>
		[Description("Begin")]
		BEGIN = 60101,
		/// <summary>
		/// END https://tools.ietf.org/html/rfc6350#section-6.1.2 
		/// </summary>
		[Description("End")]
		END = 60102,
		/// <summary>
		/// SOURCE https://tools.ietf.org/html/rfc6350#section-6.1.3
		/// </summary>
		[Description("Source")]
		SOURCE = 60103,
		/// <summary>
		/// KIND https://tools.ietf.org/html/rfc6350#section-6.1.4
		/// </summary>
		[Description("Kind")]
		KIND = 60104,
		/// <summary>
		/// XML https://tools.ietf.org/html/rfc6350#section-6.1.5
		/// </summary>
		[Description("XML")]
		XML = 60105,
		/// <summary>
		/// FN (FullName) https://tools.ietf.org/html/rfc6350#section-6.2.1
		/// </summary>
		[Description("Full Name")]
		FN = 60201,
		/// <summary>
		/// N (definitive name) https://tools.ietf.org/html/rfc6350#section-6.2.2
		/// </summary>
		[Description("Definitive Name")]
		N = 60202,
		/// <summary>
		/// Nickname https://tools.ietf.org/html/rfc6350#section-6.2.3
		/// </summary>
		[Description("Nickname")]
		NICKNAME = 60203,
		/// <summary>
		/// Photo https://tools.ietf.org/html/rfc6350#section-6.2.4
		/// </summary>
		[Description("Photo")]
		PHOTO = 60204,
		/// <summary>
		/// DoB https://tools.ietf.org/html/rfc6350#section-6.2.5
		/// </summary>
		[Description("Date of Birth")]
		BDAY = 60205,
		/// <summary>
		/// Wedding/other anniversary https://tools.ietf.org/html/rfc6350#section-6.2.6
		/// </summary>
		[Description("Anniversary")]
		ANNIVERSARY = 60206,
		/// <summary>
		/// "gender" https://tools.ietf.org/html/rfc6350#section-6.2.7
		/// </summary>
		[Description("Gender")]
		GENDER = 60207,
		/// <summary>
		/// Address https://tools.ietf.org/html/rfc6350#section-6.3.1
		/// </summary>
		[Description("Address")]
		ADR = 60301,
		/// <summary>
		/// Telephone https://tools.ietf.org/html/rfc6350#section-6.4.1
		/// </summary>
		[Description("Telephone")]
		TEL = 60401,
		/// <summary>
		/// Email address https://tools.ietf.org/html/rfc6350#section-6.4.2
		/// </summary>
		[Description("Email")]
		EMAIL = 60402,
		/// <summary>
		/// Instant Messaging handle https://tools.ietf.org/html/rfc6350#section-6.4.3
		/// </summary>
		[Description("Instant Messaging")]
		IMPP = 60403,
		/// <summary>
		/// Language(s) known https://tools.ietf.org/html/rfc6350#section-6.4.4
		/// </summary>
		[Description("Language")]
		LANG = 60404,
		/// <summary>
		/// Timezone https://tools.ietf.org/html/rfc6350#section-6.5.1
		/// </summary>
		[Description("TimeZone")]
		TZ = 60501,
		/// <summary>
		/// Geo positioning location https://tools.ietf.org/html/rfc6350#section-6.5.2
		/// </summary>
		[Description("Geo-Location")]
		GEO = 60502,
		/// <summary>
		/// Job Title https://tools.ietf.org/html/rfc6350#section-6.6.1
		/// </summary>
		[Description("Job Title")]
		TITLE = 60601,
		/// <summary>
		/// Role in the organization https://tools.ietf.org/html/rfc6350#section-6.6.2
		/// </summary>
		[Description("Role")]
		ROLE = 60602,
		/// <summary>
		/// Logo image https://tools.ietf.org/html/rfc6350#section-6.6.3
		/// </summary>
		[Description("Logo")]
		LOGO = 60603,
		/// <summary>
		/// Organization/company name https://tools.ietf.org/html/rfc6350#section-6.6.4
		/// </summary>
		[Description("Organization Name")]
		ORG = 60604,
		/// <summary>
		/// Member (in groups, contains ID for individual vCard object) https://tools.ietf.org/html/rfc6350#section-6.6.5
		/// </summary>
		[Description("Member")]
		MEMBER = 60605,
		/// <summary>
		/// Relatives/associates https://tools.ietf.org/html/rfc6350#section-6.6.6
		/// </summary>
		[Description("Relatives/Associates")]
		RELATED = 60606,
		/// <summary>
		/// Categories the object belongs to https://tools.ietf.org/html/rfc6350#section-6.7.1
		/// </summary>
		[Description("Categories")]
		CATEGORIES = 60701,
		/// <summary>
		/// Misc. Notes about the object https://tools.ietf.org/html/rfc6350#section-6.7.2
		/// </summary>
		[Description("Notes")]
		NOTE = 60702,
		/// <summary>
		/// The "prodid" of the software that generated the vCard https://tools.ietf.org/html/rfc6350#section-6.7.3
		/// </summary>
		[Description("Uid of Generating Software")]
		PRODID = 60703,
		/// <summary>
		/// The revision (timestamp) of the vCard's current state https://tools.ietf.org/html/rfc6350#section-6.7.4
		/// </summary>
		[Description("Revision")]
		REV = 60704,
		/// <summary>
		/// Recording of how to pronounce name, or theme https://tools.ietf.org/html/rfc6350#section-6.7.5
		/// </summary>
		[Description("Sound")]
		SOUND = 60705,
		/// <summary>
		/// The UID https://tools.ietf.org/html/rfc6350#section-6.7.6
		/// </summary>
		[Description("Entity Uid")]
		UID = 60706,
		/// <summary>
		/// ClientPidMap https://tools.ietf.org/html/rfc6350#section-6.7.7
		/// </summary>
		[Description("Client PID Map")]
		CLIENTPIDMAP = 60707,
		/// <summary>
		/// URL for websites/pages related to the object https://tools.ietf.org/html/rfc6350#section-6.7.8
		/// </summary>
		[Description("URLs")]
		URL = 60708,
		/// <summary>
		/// Version https://tools.ietf.org/html/rfc6350#section-6.7.9
		/// </summary>
		[Description("vCard Standard Version")]
		VERSION = 60709,
		/// <summary>
		/// Public key https://tools.ietf.org/html/rfc6350#section-6.8.1
		/// </summary>
		[Description("PKI Public Key")]
		KEY = 60801,
		/// <summary>
		/// FBURL https://tools.ietf.org/html/rfc6350#section-6.9.1
		/// </summary>
		[Description("Free-Busy URL")]
		FBURL = 60901,
		/// <summary>
		/// CalAdUri https://tools.ietf.org/html/rfc6350#section-6.9.2
		/// </summary>
		[Description("Calendar/Address Book URI")]
		CALADRURI = 60902,
		/// <summary>
		/// CalUri	https://tools.ietf.org/html/rfc6350#section-6.9.3
		/// </summary>
		[Description("Calendar URI")]
		CALURI = 60903,
		/// <summary>
		/// Indicates that the property is an extended property not defined in the standard https://tools.ietf.org/html/rfc6350#section-6.10
		/// </summary>
		[Description("(Extended Property)")]
		extended = 61000,
		/// <summary>
		/// Indicates that the property has been parsed, but the name is not defined in the standard.
		/// </summary>
		[Description("(Unknown Property)")]
		unknown = 1
	}
}