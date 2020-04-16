namespace Xrd.vCard.Props {
	/// <summary>
	/// A C# representation of the vCard 4.0 / 6.* unrecognized Property.
	/// </summary>
	/// <remarks>
	/// This representation simply treats the value as a single string and is intended to be used for improperly formed or extended properties.
	/// https://tools.ietf.org/html/rfc6350#section-6
	/// </remarks>
	public class UnknownProperty : StringProperty {
		/// <summary>
		/// Create a new instance of an empty unrecognized Property.
		/// </summary>
		public UnknownProperty() {
			SetProperty(KnownProperties.unknown);
		}

		/// <summary>
		/// Create a new instance of an unrecognized Property from the specified line of text.
		/// </summary>
		/// <param name="input">The line of text to parse when creating the Property</param>
		public UnknownProperty(string input) : base(input) { }

		internal UnknownProperty(string name, string parameters, string value) : base(name, parameters, value) { }
	}
}