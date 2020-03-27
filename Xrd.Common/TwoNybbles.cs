namespace Xrd {
	public struct TwoNybbles {
		#region Fields
		public readonly byte Full;
		#endregion

		#region Constructors
		public TwoNybbles(byte full) =>
			Full = full;

		public TwoNybbles(byte high, byte low) =>
			Full = (byte)((high << 4) | (low & 0x0f));
		#endregion

		#region Properties
		public byte High =>
			(byte)(Full >> 4);

		public byte Low => (byte)(Full & 0x0f);
		#endregion
	}
}