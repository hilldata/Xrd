namespace Xrd {
	/// <summary>
	/// Interface used to indicate that the implementing complex type
	/// can be evaluated as if it were a <see langword="null"/> value.
	/// </summary>
	public interface INullable {
		/// <summary>
		/// Should the instance of the implementing class be evaluated as if it were
		/// <see langword="null"/> (Should not be serialized or operated on).
		/// </summary>
		bool IsValueNull { get; }
	}
}
