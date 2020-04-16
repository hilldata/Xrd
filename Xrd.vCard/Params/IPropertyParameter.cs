using System.Collections.Generic;

namespace Xrd.vCard.Params {
	/// <summary>
	/// Definition of required property parameter members
	/// </summary>
	public interface IPropertyParameter : INullable {
		/// <summary>
		/// Gets the KnownParameter value for the instance.
		/// </summary>
		KnownParameters Parameter { get; }
		/// <summary>
		/// Gets the name of the parameter as parsed from  the text representation.
		/// </summary>
		string Name { get; }
		/// <summary>
		/// Get/set the KnownProperty the parameter is assigned to.
		/// </summary>
		KnownProperties PropertyAssignedTo { get; set; }
		/// <summary>
		/// List of acceptable values (in string format)
		/// </summary>
		List<string> AcceptableValues { get; }
	}
}