namespace Xrd.vCard {
	/// <summary>
	/// Enumerations of the various kinds of vCard objects.
	/// </summary>
	public enum Kinds {
		/// <summary>
		/// The vCard represents an individual person.
		/// </summary>
		Individual = 0,
		/// <summary>
		/// The vCard represents a group (distribution list)
		/// </summary>
		Group = 1,
		/// <summary>
		/// The vCard represents an organization (company, government, etc.)
		/// </summary>
		Org = 2,
		/// <summary>
		/// The vCard represents a location (landmark, historical marker, building, etc.)
		/// </summary>
		Location = 3
	}
}
