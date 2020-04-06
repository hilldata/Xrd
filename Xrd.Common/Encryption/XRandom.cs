using System;

namespace Xrd.Encryption {
	/// <summary>
	/// Class used to generate deterministic pseudo-random numbers for
	/// cryptographic/hashing operations. 
	/// </summary>
	/// <remarks>
	/// NOT suitable for "dice-rolling" app.
	/// </remarks>
	public sealed class XRandom {
		private int m1, m2;
		
		/// <summary>
		/// Instantiate an instance of the XRandom class using the specified seed.
		/// </summary>
		/// <param name="seed">The seed for the randomizer.</param>
		public XRandom(Guid seed) {
			if (seed.Equals(Guid.Empty)) {
				throw new ArgumentOutOfRangeException(nameof(seed));
			}

			byte[] vs = seed.ToByteArray();
			for (int i = 0; i < 8; i++) {
				m1 ^= vs[i * 2];
				m2 &= vs[i * 2 + 1];
			}
			// Clear the array.
			vs.Wipe();
			// Push next to initialize.
			Next();
		}

		/// <summary>
		/// Instantiate an instance of the XRandom class using the specified seed.
		/// </summary>
		/// <param name="seed">The seed for the randomizer.</param>
		public XRandom(long seed) {
			if (seed == 0)
				throw new ArgumentOutOfRangeException(nameof(seed));

			byte[] x = new byte[4];
			byte[] y = new byte[4];
			byte[] t = BitConverter.GetBytes(seed);

			x[0] = t[4];
			x[1] = t[6];
			x[2] = t[0];
			x[3] = t[2];
			y[0] = t[7];
			y[1] = t[1];
			y[2] = t[5];
			y[3] = t[3];

			m1 = BitConverter.ToInt32(x, 0);
			m2 = BitConverter.ToInt32(y, 0);

			// Clear the arrays.
			x.Wipe();
			y.Wipe();
			t.Wipe();

			// Push next to initialize.
			Next();
		}

		/// <summary>
		/// Get the next pseudo-random integer.
		/// </summary>
		/// <param name="positiveOnly">Should only positive results be returned?</param>
		/// <returns>The next pseudo-random integer.</returns>
		public int Next(bool positiveOnly = false) {
			m1 = 10007 * (m1 & 65535) + (m1 >> 16);
			m2 = 44701 * (m2 & 65535) + (m2 << 16);
			if (positiveOnly)
				return Math.Abs(m1 ^ m2);
			else
				return m1 ^ m2;
		}

		/// <summary>
		/// Fill the buffer with pseudo-random bytes.
		/// </summary>
		/// <param name="vs">The buffer to be filled.</param>
		public void NextBytes(byte[] vs) {
			if (vs.IsNullOrEmpty())
				return;

			byte[] t = new byte[4];
			for (int i = 0; i < vs.Length; i++) {
				if (i % 4 == 0)
					t = BitConverter.GetBytes(Next());
				vs[i] = t[i % 4];
			}
			t.Wipe();
		}

		/// <summary>
		/// Generate an array of pseudo-random bytes of the specified size.
		/// </summary>
		/// <param name="length">The size of the resulting array.</param>
		/// <returns>An array of pseudo-random bytes</returns>
		public byte[] GetBytes(int length) {
			// Make sure we're neither empty or too large
			if (length < 1 || length > short.MaxValue)
				throw new ArgumentOutOfRangeException(nameof(length));

			byte[] vs = new byte[length];
			NextBytes(vs);
			return vs;
		}
	}
}