namespace Xrd.Text.DiffMatchPatch {
	/// <summary>
	/// Class representing options used in Match operations.
	/// </summary>
	/// <remarks>
	/// Adapted from https://github.com/google/diff-match-patch 
	/// Original algorith used an instance of a class, but this library converts all public members to extension methods. Class properties were encapsulated into this *optional* options class.
	/// </remarks>
	public class MatchOptions {
		/// <summary>
		/// Default constructor.
		/// </summary>
		public MatchOptions() { }

		/// <summary>
		/// Construct a customized MatchOptions object.
		/// </summary>
		/// <param name="matchThreshold">At what point is no match declared? (0.0 = exact, 1.0 = very loose)</param>
		/// <param name="matchDistance">How far to search for a match. (0 = exact location, 1000+ = broad search)</param>
		public MatchOptions(float matchThreshold, int matchDistance) {
			FuzzyThreshold = matchThreshold;
			FuzzyDistance = matchDistance;
		}

		/// <summary>
		/// Static getter for the default options.
		/// </summary>
		/// <remarks>Defaults:
		/// FuzzyThreshold = 0.5f
		/// FuzzyDistance = 1000
		/// </remarks>
		public static MatchOptions DefaultMatchOptions => new MatchOptions();

		/// <summary>
		/// At what point is no match declared? (0 = exact location, 1.0 = very loose)
		/// </summary>
		public readonly float FuzzyThreshold = 0.5f;

		/// <summary>
		/// How far to search for a match. (0 = exact location, 1000+ = broad search)
		/// A match this many characters away from the expected location will add 1.0 
		/// to the score (0.0 is a perfect match)
		/// </summary>
		public readonly int FuzzyDistance = 1000;

		/// <summary>
		/// Number of bits in an int.
		/// </summary>
		public const short MAX_BITS = 32;
	}
}