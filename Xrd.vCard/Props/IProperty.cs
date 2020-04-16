using System.Collections.Generic;

namespace Xrd.vCard.Props {
	/// <summary>
	/// Interface defining the required members of a vCard Property implementation.
	/// </summary>
	public interface IProperty : INullable {
		/// <summary>
		/// The KnownProperty the instance represents.
		/// </summary>
		KnownProperties Property { get; }
		/// <summary>
		/// The property group the property belongs to.
		/// </summary>
		string Group { get; }
		/// <summary>
		/// The property's name
		/// </summary>
		string Name { get; }
		/// <summary>
		/// List of acceptable (per the standard) parameters than have been applied to the property
		/// </summary>
		List<Params.IPropertyParameter> Parameters { get; }
		/// <summary>
		/// List of parameters than can be applied to the property (per the standard)
		/// </summary>
		KnownParameters AllowedParameters { get; }
		/// <summary>
		/// List of acceptable values that can be applied to the property (for enumerated types)
		/// </summary>
		List<string> AcceptableValues { get; }
		/// <summary>
		/// List of parameters that were defined in the property represetation text, but are not valid for the property per the standard.
		/// </summary>
		List<Params.IPropertyParameter> INVALID_Params { get; }

		List<Params.IPropertyParameter> GetParameters(KnownParameters known);
	}
}