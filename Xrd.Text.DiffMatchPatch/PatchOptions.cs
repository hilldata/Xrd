namespace Xrd.Text.DiffMatchPatch {
	/// <summary>
	/// Class representing options used in Patch operations.
	/// </summary>
	/// <remarks>
	/// Adapted from https://github.com/google/diff-match-patch 
	/// Original algorith used an instance of a class, but this library converts all public members to extension methods. Class properties were encapsulated into this *optional* options class.
	/// </remarks>
	public class PatchOptions : MatchOptions {
		/// <summary>
		/// Default constructor.
		/// </summary>
		public PatchOptions() : base() { }

		/// <summary>
		/// Construct a customized PatchOptions object.
		/// </summary>
		/// <param name="matchThreshold">At what point is no match declared? (0.0 = exact, 1.0 = very loose)</param>
		/// <param name="matchDistance">How far to search for a match. (0 = exact location, 1000+ = broad search)</param>
		/// <param name="deleteThreshold">When deleting a large block of text, how close do the contents have to be to match the expected contents.</param>
		/// <param name="patchMargin">Chuck size for Patch context length.</param>
		public PatchOptions(float matchThreshold, int matchDistance, float deleteThreshold, short patchMargin) : base(matchThreshold, matchDistance) {
			DeleteThreshold = deleteThreshold;
			Margin = patchMargin;
		}

		/// <summary>
		/// Static getter for the default options.
		/// </summary>
		/// <remarks>Defaults:
		/// FuzzyThreshold = 0.5f
		/// FuzzyDistance = 1000
		/// DeleteThreshold = 0.5f
		/// Margin = 4
		/// </remarks>
		public static PatchOptions DefaultPatchOptions => new PatchOptions();


		/// <summary>
		/// When deleting a large block of text (over ~64 characters), how close do the contents
		/// have to be to match the expected contents? (0.0 = perfection, 1.0 = very loose.).
		/// Note that MatchThreshold controls how closely the end points of a delete need to match.
		/// </summary>
		/// <remarks>Default value = 0.5f</remarks>
		public readonly float DeleteThreshold = 0.5f;

		/// <summary>
		/// Chunk size for Patch context length .
		/// </summary>
		/// <remarks>Default value = 4</remarks>
		public readonly short Margin = 4;
	}
}