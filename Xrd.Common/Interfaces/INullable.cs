namespace Xrd {
	/// <summary>
	/// Interface used to indicate that the implementing complex type
	/// can be evaluated to a <see langword="null"/> value.
	/// </summary>
	public interface INullable {
		/// <summary>
		/// Is the instance of the implementing class evaluated as 
		/// <see langword="null"/> (Should not be serialized or operated on).
		/// </summary>
		bool IsValueNull { get; }
	}
}
