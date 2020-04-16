using System;
using System.ComponentModel;

namespace Xrd.vCard {
	/// <summary>
	/// Enumerations of the known parameter types.
	/// </summary>
	[Flags]
	public enum KnownParameters {
		/// <summary>
		/// The parameter has been read, but not parsed yet (default, placeholder value).
		/// </summary>
		[Description("Read; Not Yet Parsed")]
		read_not_parsed_yet = 0b0000,
		/// <summary>
		/// Indicates the language the property applies to (or is in)
		/// </summary>
		[Description("Language")]
		LANGUAGE = 0b0001,
		/// <summary>
		/// Indicates the property's value TYPE.
		/// </summary>
		[Description("Value")]
		VALUE = 0b0010,
		/// <summary>
		/// Indicates the relative preference for the property (basically, ORDER BY, or ordinal index)
		/// </summary>
		[Description("Preference")]
		PREF = 0b0100,
		/// <summary>
		/// Indicates the property's altid
		/// </summary>
		[Description("Alternate ID")]
		ALTID = 0b1000,
		/// <summary>
		/// Indicates the property's pid
		/// </summary>
		[Description("Property ID")]
		PID = 0b0001_0000,
		/// <summary>
		/// Indicates the type, or CATEGORY, the property falls into
		/// </summary>
		[Description("Type/Category")]
		TYPE = 0b0010_0000,
		/// <summary>
		/// Indicates the MIME type of the property's value.
		/// </summary>
		[Description("MIME/Media Type")]
		MEDIATYPE = 0b0100_0000,
		/// <summary>
		/// Indicates the cal scale to use for date properties.
		/// </summary>
		[Description("Calendar Scale")]
		CALSCALE = 0b1000_0000,
		/// <summary>
		/// Indicates how a text property should be sorted.
		/// </summary>
		[Description("Sort As")]
		SORT_AS = 0b0001_0000_0000,
		/// <summary>
		/// Indicates the property's location.
		/// </summary>
		[Description("Geo-Location")]
		GEO = 0b0010_0000_0000,
		/// <summary>
		/// Indicates the property's associated timezone.
		/// </summary>
		[Description("TimeZone")]
		TZ = 0b0100_0000_0000,
		/// <summary>
		/// Indicates the "label" or print text for address properties. (only useable on ADDRESS properties)
		/// </summary>
		[Description("Address Label")]
		LABEL = 0b1000_0000_0000,
		/// <summary>
		/// indicates that the parameter has been read, but no name was provide (i.e., the text string was not well formed)
		/// </summary>
		[Description("(No Name Provided)")]
		no_name_provided = 0b0100_0000_0000_0000,
		/// <summary>
		/// indicates that the parameter has been read, but the name is not one of those defined in the standard.
		/// </summary>
		[Description("(unknown parameter)")]
		unknown = 0b1000_0000_0000_0000
	}
}
