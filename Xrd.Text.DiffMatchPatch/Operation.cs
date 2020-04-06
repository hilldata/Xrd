namespace Xrd.Text.DiffMatchPatch {
	/// <summary>
	/// Enumeration of the operation in a Diff object.
	/// </summary>
	/// <remarks>
	/// Adapted from https://github.com/google/diff-match-patch 
	/// </remarks>
	public enum Operation {
		/// <summary>
		/// A deletion operation.
		/// </summary>
		DELETE,
		/// <summary>
		/// An insertion operation.
		/// </summary>
		INSERT,
		/// <summary>
		/// An equality.
		/// </summary>
		EQUAL
	}
}