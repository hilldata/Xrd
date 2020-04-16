using System;
using System.Collections.Generic;
using System.Text;

namespace Xrd.vCard {
	/// <summary>
	/// Class that defined constant values used in the library.
	/// </summary>
	internal static class Constants {
		#region Constants
		/// <summary>
		/// The wildcard character
		/// </summary>
		internal const string WILDCARD = "*";
		/// <summary>
		/// Parameter name/value separator character
		/// </summary>
		internal const char PARAM_NAME_VAL_SEPARATOR = '=';
		/// <summary>
		/// Parameter multiple value separator character (for list values)
		/// </summary>
		internal const char PARAM_MULTI_VAL_SEPARATOR = ',';

		/// <summary>
		/// Property name/value separator character
		/// </summary>
		internal const char PROP_NAME_VAL_SEPARATOR = ':';
		/// <summary>
		/// Property group/name separator character
		/// </summary>
		internal const char PROP_GROUP_NAME_SEPARATOR = '.';
		/// <summary>
		/// Property multiple-parameter separator character
		/// </summary>
		internal const char PROP_PARAMETER_SEPARATOR = ';';

		/// <summary>
		/// The carriage return character
		/// </summary>
		internal const char CR = (char)0x000D;
		/// <summary>
		/// The line-feed character
		/// </summary>
		internal const char LF = (char)0x000A;
		/// <summary>
		/// The space character
		/// </summary>
		internal const char SPACE = (char)0x0020;
		/// <summary>
		/// The TAB (horizontal) character
		/// </summary>
		internal const char HTAB = (char)0x0009;
		/// <summary>
		/// Carriage return/line feed character combination
		/// </summary>
		internal static string CRLF = $"{CR}{LF}";
		/// <summary>
		/// One possible character combination used to represent a line fold in a vCard text representation
		/// </summary>
		internal static string LINE_FOLD1 = $"{CR}{LF}{SPACE}";
		/// <summary>
		/// another possibel character combination used to represent a line fold in a vCard text representation.
		/// </summary>
		internal static string LINE_FOLD2 = $"{CR}{LF}{HTAB}";
		/// <summary>
		/// List of standard vCard file-type extensions.
		/// </summary>
		internal static List<string> vCARD_EXTENSIONS = new List<string>() { "vcf", "vcard" };
		#endregion
	}
}