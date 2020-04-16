using System.Collections.Generic;
using System.Threading.Tasks;

using Xrd.Collections;

namespace Xrd.vCard {
	/// <summary>
	/// Class used to represent a vCard object.
	/// </summary>
	public sealed class VirtualCard {
		/// <summary>
		/// Default constructor.
		/// </summary>
		public VirtualCard() { }


		private static VirtualCard GetFirst(List<VirtualCard> list) =>
			(list == null || list.Count < 1) ? null : list[0];

		/// <summary>
		/// Read and parse the specified text input into a list of vCard instances.
		/// </summary>
		/// <param name="input">The text to read/parse</param>
		/// <returns>A list of vCard instances</returns>
		public static List<VirtualCard> ParseMultiple(string input) =>
			input.ParseCard();

		/// <summary>
		/// Read and parse the specified text input into a list of vCard instances and return the first instance.
		/// </summary>
		/// <param name="input">The text to read/parse</param>
		/// <returns>A single vCard instance</returns>
		public static VirtualCard Parse(string input) =>
			GetFirst(input.ParseCard(1));

		/// <summary>
		/// Opens and parses the specified file into a list of vCard instances and returns the first instance.
		/// </summary>
		/// <param name="fi">The vCard file to load</param>
		/// <returns>A single vCard instance</returns>
		public static VirtualCard Load(System.IO.FileInfo fi) =>
			GetFirst(fi.ReadFile(1));

		/// <summary>
		/// Asynchronously opens and parses the specified file into a list of vCard instances and returns the first instance.
		/// </summary>
		/// <param name="fi">The vCard file to load</param>
		/// <returns>A single vCard instance</returns>
		public static async Task<VirtualCard> LoadAsync(System.IO.FileInfo fi) =>
			GetFirst(await fi.ReadFileAsync(1));

		/// <summary>
		/// Opens and parses the specified file into a list of vCard instances
		/// </summary>
		/// <param name="fi">The vCard file to load</param>
		/// <returns>a list of vCard instances.</returns>
		public static List<VirtualCard> LoadMultiple(System.IO.FileInfo fi) =>
			fi.ReadFile();

		/// <summary>
		/// Asynchronously opens and parses the specified file into a list of vCard instances.
		/// </summary>
		/// <param name="fi">The vCard file to load</param>
		/// <returns>A list of vCard instances</returns>
		public static async Task<List<VirtualCard>> LoadMultipleAsync(System.IO.FileInfo fi) =>
			await fi.ReadFileAsync();

		#region Properties
		#region 6.1 General Properties
		/*	=======================================================================================================================================
			NOTE: 6.1.1. BEGIN & 6.1.2. END are intentionally excluded from this representation as they are simply used to mark out borders of each 
			syntactic entity within the vCard content-type.
			=======================================================================================================================================*/

		/// <summary>
		/// 6.1.3. SOURCE. 
		/// </summary>
		/// <remarks>
		///		Purpose:  To identify the source of directory information contained in the content type.
		///		Value type:  uri
		///		Cardinality:  * (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.1.3
		/// </remarks>
		public List<Props.SOURCE> SOURCE { get; set; } = new List<Props.SOURCE>();

		/// <summary>
		/// 6.1.4. KIND. 
		/// </summary>
		/// <remarks> 
		///		Purpose:  To specify the kind of object the vCard represents.
		///		Value type:  A single text value.
		///		Cardinality:  *1 (Optional, Single)
		///		https://tools.ietf.org/html/rfc6350#section-6.1.4
		///	</remarks>
		public Kinds KIND { get; set; } = Kinds.Individual;

		/// <summary>
		/// 6.1.5. XML. 
		/// </summary>
		/// <remarks>
		///		Purpose:  To include extended XML-encoded vCard data in a plain vCard.
		///		Value type:  A single text value.
		///		Cardinality:  * (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.1.5
		///	</remarks>
		public List<Props.XML> XML { get; set; } = new List<Props.XML>();
		#endregion

		#region 6.2 Identification Properties
		/// <summary>
		/// 6.2.1. FN
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the formatted text corresponding to the name of the object the vCard represents.
		///		Value type:  A single text value.
		///		Cardinality:  1* (Required, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.2.1
		///	</remarks>
		public List<Props.FN> FN { get; set; } = new List<Props.FN>();

		/// <summary>
		/// 6.2.2. N
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the components of the name of the object the vCard represents.
		///		Value type:   A single structured text value.  Each component can have multiple values.
		///		Cardinality:  *1 (Optional, Single)
		///		https://tools.ietf.org/html/rfc6350#section-6.2.2
		///	</remarks>
		public Props.N N { get; set; } = null;

		/// <summary>
		/// 6.2.3. Nickname
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the text corresponding to the nickname of the object the vCard represents.
		///		Value type:  One or more text values separated by a COMMA character (U+002C).
		///		Cardinality:  * (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.2.3
		///	</remarks>
		public List<Props.NICKNAME> NICKNAME { get; set; } = new List<Props.NICKNAME>();

		/// <summary>
		/// 6.2.4. PHOTO
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify an image or photograph information that annotates some aspect of the object the vCard represents.
		///		Value type:  A single URI.
		///		Cardinality:  * (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.2.4
		///	</remarks>
		public List<Props.PHOTO> PHOTO { get; set; } = new List<Props.PHOTO>();

		/// <summary>
		/// 6.2.5. BDAY
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the birth date of the object the vCard represents.
		///		Value type:  The default is a single date-and-or-time value.  It can also be reset to a single text value.
		///		Cardinality:  *1 (Optional, Single)
		///		https://tools.ietf.org/html/rfc6350#section-6.2.5
		///	</remarks>
		public Props.BDAY BDAY { get; set; } = null;

		/// <summary>
		/// 6.2.6. ANNIVERSARY
		/// </summary>
		/// <remarks>
		///		Purpose:  The date of marriage, or equivalent, of the object the vCard represents.
		///		Value type:  The default is a single date-and-or-time value.  It can also be reset to a single text value.
		///		Cardinality:  *1 (Optional, Single)
		///		https://tools.ietf.org/html/rfc6350#section-6.2.6
		///	</remarks>
		public Props.ANNIVERSARY ANNIVERSARY { get; set; } = null;

		/// <summary>
		/// 6.2.7. GENDER
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the components of the sex and gender identity of the object the vCard represents.
		///		Value type:  A single structured value with two components.  Each component has a single text value.
		///		Cardinality:  *1 (Optional, Single)
		///		https://tools.ietf.org/html/rfc6350#section-6.2.6
		///	</remarks>
		public Props.GENDER GENDER { get; set; } = null;
		#endregion

		#region 6.3. Delivery Addressing Properties
		/// <summary>
		/// 6.3.1. ADR
		/// </summary>
		/// <remarks>
		///		Purpose:	To specify the components of the delivery address for the vCard object.
		///		Value type:	A single structured text value, separated by the SEMICOLON character(U+003B).
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.3.1
		/// </remarks>
		public List<Props.ADR> ADR { get; set; } = new List<Props.ADR>();
		#endregion

		#region 6.4. Communication Properties
		/// <summary>
		/// 6.4.1. TEL
		/// </summary>
		/// <remarks>
		///		Purpose:	To specify the telephone number for telephony communication with the object the vCard represents.
		///		Value type:	By default, it is a single free-form text value (for backward compatibility with vCard 3), but it SHOULD be reset to a
		///					URI value.It is expected that the URI scheme will be "tel", as specified in [RFC3966], but other schemes MAY be used.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.4.1
		/// </remarks>
		public List<Props.TEL> TEL { get; set; } = new List<Props.TEL>();

		/// <summary>
		/// 6.4.2. EMAIL
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the electronic mail address for communication with the object the vCard represents.
		///		Value type:  A single text value.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.4.2
		/// </remarks>
		public List<Props.EMAIL> EMAIL { get; set; } = new List<Props.EMAIL>();

		/// <summary>
		/// 6.4.3. IMPP
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the URI for instant messaging and presence protocol communications with the object the vCard represents.
		///		Value type:  A single URI.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.4.3
		/// </remarks>
		public List<Props.IMPP> IMPP { get; set; } = new List<Props.IMPP>();

		/// <summary>
		/// 6.4.4. LANG
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the language(s) that may be used for contacting the entity associated with the vCard.
		///		Value type:  A single language-tag value.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.4.4
		/// </remarks>
		public List<Props.LANG> LANG { get; set; } = new List<Props.LANG>();
		#endregion

		#region 6.5. Geographical Properties
		/// <summary>
		/// 6.5.1. TZ
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify information related to the time zone of the object the vCard represents.
		///		Value type:  The default is a single text value.  It can also be reset to a single URI or utc-offset value.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.5.1
		/// </remarks>
		public List<Props.TZ> TZ { get; set; } = new List<Props.TZ>();

		/// <summary>
		/// 6.5.2. GEO
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify information related to the global positioning of the object the vCard represents.
		///		Value type:  A single URI.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.5.2
		/// </remarks>
		public List<Props.GEO> GEO { get; set; }
		#endregion

		#region 6.6. Organizational Properties
		/// <summary>
		/// 6.6.1. TITLE
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the position or job of the object the vCard represents.
		///		Value type:  A single text value.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.6.1
		/// </remarks>
		public List<Props.TITLE> TITLE { get; set; } = new List<Props.TITLE>();

		/// <summary>
		/// 6.6.2. ROLE
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the function or part played in a particular situation by the object the vCard represents.
		///		Value type:  A single text value.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.6.2
		/// </remarks>
		public List<Props.ROLE> ROLE { get; set; } = new List<Props.ROLE>();

		/// <summary>
		/// 6.6.3. LOGO
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify a graphic image of a logo associated with the object the vCard represents.
		///		Value type:  A single URI.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.6.3
		/// </remarks>
		public List<Props.LOGO> LOGO { get; set; } = new List<Props.LOGO>();

		/// <summary>
		/// 6.6.4. ORG
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the organizational name and units associated with the vCard.
		///		Value type:  A single structured text value consisting of components separated by the SEMICOLON character(U+003B).
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.6.4
		/// </remarks>
		public List<Props.ORG> ORG { get; set; } = new List<Props.ORG>();

		/// <summary>
		/// 6.6.5. MEMBER
		/// </summary>
		/// <remarks>
		///		Purpose:  To include a member in the group this vCard represents.
		///		Value type:  A single URI.It MAY refer to something other than a vCard object.  For example, an email distribution list could
		///			employ the "mailto" URI scheme[RFC6068] for efficiency.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.6.5
		/// </remarks>
		public List<Props.MEMBER> MEMBER { get; set; } = new List<Props.MEMBER>();

		/// <summary>
		/// 6.6.6. RELATED
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify a relationship between another entity and the entity represented by this vCard.
		///		Value type:  A single URI. It can also be reset to a single text value. The text value can be used to specify textual information.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.6.6
		/// </remarks>
		public List<Props.RELATED> RELATED { get; set; } = new List<Props.RELATED>();
		#endregion

		#region 6.7. Explanatory Properties
		/// <summary>
		/// 6.7.1. CATEGORIES
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify application category information about the vCard, also known as "tags".
		///		Value type:  One or more text values separated by a COMMA character (U+002C).
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.7.1
		/// </remarks>
		public List<Props.CATEGORIES> CATEGORIES { get; set; } = new List<Props.CATEGORIES>();

		/// <summary>
		/// 6.7.2. NOTE
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify supplemental information or a comment that is associated with the vCard.
		///		Value type:  A single text value.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.7.2
		/// </remarks>
		public List<Props.NOTE> NOTE { get; set; } = new List<Props.NOTE>();

		/// <summary>
		/// 6.7.3 PRODID
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the identifier for the product that created the vCard object.
		///		Type value:  A single text value.
		///		Cardinality:	*1 (Optional, Single)
		///		https://tools.ietf.org/html/rfc6350#section-6.7.3
		/// </remarks>
		public Props.PRODID PRODID { get; set; } = null;

		/// <summary>
		/// 6.7.4. REV
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify revision information about the current vCard.
		///		Value type:  A single timestamp value.
		///		Cardinality:	*1 (Optional, Single)
		///		https://tools.ietf.org/html/rfc6350#section-6.7.4
		/// </remarks>
		public Props.REV REV { get; set; } = null;

		/// <summary>
		/// 6.7.5. SOUND
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify a digital sound content information that annotates some aspect of the vCard.This property is often used
		///			to specify the proper pronunciation of the name property value of the vCard.
		///		Value type:  A single URI.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.7.5
		/// </remarks>
		public List<Props.SOUND> SOUND { get; set; }

		/// <summary>
		/// 6.7.6. UID
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify a value that represents a globally unique identifier corresponding to the entity associated with the vCard.
		///		Value type:  A single URI value.It MAY also be reset to free-form text.
		///		Cardinality:	*1 (Optional, Single)
		///		https://tools.ietf.org/html/rfc6350#section-6.7.6
		/// </remarks>
		public Props.UID UID { get; set; } = null;

		/// <summary>
		/// 6.7.7. CLIENTPIDMAP
		/// </summary>
		/// <remarks>
		///		Purpose:  To give a global meaning to a local PID source identifier.
		///		Value type:  A semicolon-separated pair of values.The first field is a small integer corresponding to the second field of a PID
		///			parameter instance.The second field is a URI.  The "uuid" URN namespace defined in [RFC4122] is particularly well suited to this
		///			task, but other URI schemes MAY be used.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.7.7
		/// </remarks>
		public List<Props.CLIENTPIDMAP> CLIENTPIDMAP { get; set; } = new List<Props.CLIENTPIDMAP>();

		/// <summary>
		/// 6.7.8. URL
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify a uniform resource locator associated with the object to which the vCard refers.Examples for individuals
		///			include personal web sites, blogs, and social networking site identifiers.
		///		Value type:  A single uri value.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.7.8
		/// </remarks>
		public List<Props.URL> URL { get; set; } = new List<Props.URL>();

		/// <summary>
		/// 6.7.9. VERSION
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the version of the vCard specification used to format this vCard.
		///		Value type:  A single text value.
		///		Cardinality:	1 (Required, Single)
		///		https://tools.ietf.org/html/rfc6350#section-6.7.9
		/// </remarks>
		public Props.VERSION VERSION { get; set; } = new Props.VERSION();
		#endregion

		#region 6.8 Security Properties
		/// <summary>
		/// 6.8.1. KEY
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify a public key or authentication certificate associated with the object that the vCard represents.
		///		Value type:  A single URI.It can also be reset to a text value.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.8.1
		/// </remarks>
		public List<Props.KEY> KEY { get; set; } = new List<Props.KEY>();
		#endregion

		#region 6.9 Calendar Properties
		/// <summary>
		/// 6.9.1. FBURL
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the URI for the busy time associated with the object that the vCard represents.
		///		Value type:  A single URI value.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.9.1
		/// </remarks>
		public List<Props.FBURL> FBURL { get; set; } = new List<Props.FBURL>();

		/// <summary>
		/// 6.9.2. CALADRURI
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the calendar user address <a href="https://tools.ietf.org/html/rfc5545">[RFC5545]</a> to which a
		///			scheduling request <a href="https://tools.ietf.org/html/rfc5546">[RFC5546]</a> should be sent for the object represented by the vCard.
		///		Value type:  A single URI value.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.9.2
		/// </remarks>
		public List<Props.CALADRURI> CALADRURI { get; set; } = new List<Props.CALADRURI>();

		/// <summary>
		/// 6.9.3. CALURI
		/// </summary>
		/// <remarks>
		///		Purpose:  To specify the URI for a calendar associated with the object represented by the vCard.
		///		Value type:  A single URI value.
		///		Cardinality:	* (Optional, Multiple)
		///		https://tools.ietf.org/html/rfc6350#section-6.9.3
		/// </remarks>
		public List<Props.CALURI> CALURI { get; set; } = new List<Props.CALURI>();
		#endregion
		#endregion

		/// <summary>
		/// Method used to add a property to the instance. Will be placed in the correct class Property.
		/// </summary>
		/// <param name="input">The vCard.Property to add.</param>
		public void AddProperty(Props.IProperty input) {
			if (input == null || input.IsValueNull)
				return;

			switch (input.Property) {
				case KnownProperties.ADR:
					ADR.AddIfNotNull(input as Props.ADR);
					break;
				case KnownProperties.ANNIVERSARY:
					ANNIVERSARY = input as Props.ANNIVERSARY;
					break;
				case KnownProperties.BDAY:
					BDAY = input as Props.BDAY;
					break;
				case KnownProperties.CALADRURI:
					CALADRURI.AddIfNotNull(input as Props.CALADRURI);
					break;
				case KnownProperties.CALURI:
					CALURI.AddIfNotNull(input as Props.CALURI);
					break;
				case KnownProperties.CATEGORIES:
					CATEGORIES.AddIfNotNull(input as Props.CATEGORIES);
					break;
				case KnownProperties.CLIENTPIDMAP:
					CLIENTPIDMAP.AddIfNotNull(input as Props.CLIENTPIDMAP);
					break;
				case KnownProperties.EMAIL:
					EMAIL.AddIfNotNull(input as Props.EMAIL);
					break;
				case KnownProperties.FBURL:
					FBURL.AddIfNotNull(input as Props.FBURL);
					break;
				case KnownProperties.FN:
					FN.AddIfNotNull(input as Props.FN);
					break;
				case KnownProperties.GENDER:
					GENDER = input as Props.GENDER;
					break;
				case KnownProperties.GEO:
					GEO.AddIfNotNull(input as Props.GEO);
					break;
				case KnownProperties.IMPP:
					IMPP.AddIfNotNull(input as Props.IMPP);
					break;
				case KnownProperties.KEY:
					KEY.AddIfNotNull(input as Props.KEY);
					break;
				case KnownProperties.KIND: {
						char k = (input as Props.KIND).Value.ToLower()[0];
						if (k == 'i')
							KIND = Kinds.Individual;
						else if (k == 'g')
							KIND = Kinds.Group;
						else if (k == 'o')
							KIND = Kinds.Org;
						else if (k == 'l')
							KIND = Kinds.Location;
						else
							KIND = Kinds.Individual;
						break;
					}
				case KnownProperties.LANG:
					LANG.AddIfNotNull(input as Props.LANG);
					break;
				case KnownProperties.LOGO:
					LOGO.AddIfNotNull(input as Props.LOGO);
					break;
				case KnownProperties.MEMBER:
					MEMBER.AddIfNotNull(input as Props.MEMBER);
					break;
				case KnownProperties.N:
					N = input as Props.N;
					break;
				case KnownProperties.NICKNAME:
					NICKNAME.AddIfNotNull(input as Props.NICKNAME);
					break;
				case KnownProperties.NOTE:
					NOTE.AddIfNotNull(input as Props.NOTE);
					break;
				case KnownProperties.ORG:
					ORG.AddIfNotNull(input as Props.ORG);
					break;
				case KnownProperties.PHOTO:
					PHOTO.AddIfNotNull(input as Props.PHOTO);
					break;
				case KnownProperties.PRODID:
					PRODID = input as Props.PRODID;
					break;
				case KnownProperties.RELATED:
					RELATED.AddIfNotNull(input as Props.RELATED);
					break;
				case KnownProperties.REV:
					REV = input as Props.REV;
					break;
				case KnownProperties.ROLE:
					ROLE.AddIfNotNull(input as Props.ROLE);
					break;
				case KnownProperties.SOUND:
					SOUND.AddIfNotNull(input as Props.SOUND);
					break;
				case KnownProperties.SOURCE:
					SOURCE.AddIfNotNull(input as Props.SOURCE);
					break;
				case KnownProperties.TEL:
					TEL.AddIfNotNull(input as Props.TEL);
					break;
				case KnownProperties.TITLE:
					TITLE.AddIfNotNull(input as Props.TITLE);
					break;
				case KnownProperties.TZ:
					TZ.AddIfNotNull(input as Props.TZ);
					break;
				case KnownProperties.UID:
					UID = input as Props.UID;
					break;
				case KnownProperties.URL:
					URL.AddIfNotNull(input as Props.URL);
					break;
				case KnownProperties.VERSION:
					VERSION = input as Props.VERSION;
					break;
				case KnownProperties.XML:
					XML.AddIfNotNull(input as Props.XML);
					break;
				default:
					return;
			}
		}
	}
}