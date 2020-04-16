using System;

namespace Xrd.vCard.Params {
	/// <summary>
	/// A C# representation of the vCard 4.0 / 5.[unrecognized] Property Parameter.
	/// https://tools.ietf.org/html/rfc6350#section-5
	/// </summary>
	/// <remarks>This representation simply treats the value as a single string and is intended to be used for improperly formed or extended parameters.</remarks>
	public class UnknownParameter : PropertyParameter<string> {
		/// <summary>
		/// Creates a new instance of an empty unrecognized Property Parameter.
		/// </summary>
		public UnknownParameter() {
			SetParameter(KnownParameters.unknown);
		}
		public UnknownParameter(KnownProperties property, string value) : base(KnownParameters.unknown, property, value) { }
		/// <summary>
		/// Creates a new instance of the unrecognized Property Parameter from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property Parameter</param>
		internal UnknownParameter(Tuple<string, string> input) : base(input) { }
	}
}