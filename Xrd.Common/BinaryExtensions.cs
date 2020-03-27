using System;
using System.Collections.Generic;
using System.Linq;

namespace Xrd {
	/// <summary>
	/// Extension methods for binary data
	/// </summary>
	public static class BinaryExtensions {
		private static readonly char[] padding = { '=' };
		private static readonly char[] replace1 = { '+', '-' };
		private static readonly char[] replace2 = { '/', '_' };

		/// <summary>
		/// Encode the binary data to a url-safe base64 string.
		/// </summary>
		/// <param name="vs">The binary data to convert</param>
		/// <returns>A url-safe base64 encoded string.</returns>
		public static string ToUrlSafeBase64(this byte[] vs) {
			if (vs == null || vs.Length < 1)
				return string.Empty;

			return Convert.ToBase64String(vs)
				.TrimEnd(padding)
				.Replace(replace1[0], replace1[1])
				.Replace(replace2[0], replace2[1]);
		}

		internal static string FixBase64String(this string urlSafeBase64) {
			if (string.IsNullOrEmpty(urlSafeBase64))
				return string.Empty;

			string temp = urlSafeBase64
				.Replace(replace1[1], replace1[0])
				.Replace(replace2[1], replace2[0]);
			switch (urlSafeBase64.Length % 4) {
				case 2: temp += "=="; break;
				case 3: temp += "="; break;
			}
			return temp;
		}

		/// <summary>
		/// Decode a url-safe base64 string to binary data.
		/// </summary>
		/// <param name="urlSafeBase64">The url-safe base64 encoded string</param>
		/// <returns>The decoded binary data</returns>
		public static byte[] ToBinaryFromBase64(this string urlSafeBase64) {
			string temp = urlSafeBase64.FixBase64String();
			if (string.IsNullOrWhiteSpace(temp))
				return null;
			return Convert.FromBase64String(temp);
		}

		/// <summary>
		/// Concatenate multiple separate binary data into a single array
		/// </summary>
		/// <param name="a1">The original array</param>
		/// <param name="a2">The array to add to the original</param>
		/// <returns>The concatenated byte array</returns>
		public static byte[] Concatenate(this byte[] a1, byte[] a2) {
			byte[] res = new byte[a1.Length + a2.Length];
			Buffer.BlockCopy(a1, 0, res, 0, a1.Length);
			Buffer.BlockCopy(a2, 0, res, a1.Length, a2.Length);
			return res;
		}

		/// <summary>
		/// Concatenate multiple separate binary data into a single array
		/// </summary>
		/// <param name="a1">The original array</param>
		/// <param name="a2">The array to add to the original</param>
		/// <param name="a3">The next array to add to the original</param>
		/// <returns>The concatenated byte array</returns>
		public static byte[] Concatenate(this byte[] a1, byte[] a2, byte[] a3) {
			byte[] res = new byte[a1.Length + a2.Length + a3.Length];
			Buffer.BlockCopy(a1, 0, res, 0, a1.Length);
			Buffer.BlockCopy(a2, 0, res, a1.Length, a2.Length);
			Buffer.BlockCopy(a3, 0, res, a1.Length + a2.Length, a3.Length);
			return res;
		}

		/// <summary>
		/// Concatenate multiple separate binary data into a single array
		/// </summary>
		/// <param name="a1">The original array</param>
		/// <param name="arrays">The arrays to add to the original</param>
		/// <returns>The concatenated byte array</returns>
		public static byte[] Concatenate(this byte[] a1, params byte[][] arrays) {
			byte[] res = new byte[a1.Length + arrays.Sum(x => x.Length)];
			int offset = 0;
			Buffer.BlockCopy(a1, 0, res, 0, a1.Length);
			offset = a1.Length;
			foreach (byte[] data in arrays) {
				Buffer.BlockCopy(data, 0, res, offset, data.Length);
				offset += data.Length;
			}
			return res;
		}

		/// <summary>
		/// Concatenate multiple separate binary data into a single array
		/// </summary>
		/// <param name="vs">An IEnumberable list of arrays</param>
		/// <returns>The concatenated byte array</returns>
		public static byte[] Concatenate(this IEnumerable<byte[]> vs) {
			byte[] res = new byte[vs.Sum(x => x.Length)];
			int offset = 0;
			foreach (byte[] data in vs) {
				Buffer.BlockCopy(data, 0, res, offset, data.Length);
				offset += data.Length;
			}
			return res;
		}

		/// <summary>
		/// Validate that binary data is not null or empty, or is at least as long as the specified length
		/// </summary>
		/// <param name="data">The byte array to validate.</param>
		/// <param name="minLength">The minimum-required length of the array (default is 1, or that the array is not empty).</param>
		/// <returns>A bool if it passes. Otherwise, an <see cref="ArgumentNullException"/> is thrown</returns>
		public static bool ValidateBinaryData(this byte[] data, int? minLength = 1) {
			if (minLength < 1)
				minLength = 1;
			if (data == null || data.Length < minLength)
				throw new ArgumentNullException(nameof(data));
			return true;
		}

		/// <summary>
		/// Clear a binary array and set it to null.
		/// </summary>
		/// <param name="data">The binary array to clear and remove references to.</param>
		public static void Wipe(this byte[] data) {
			if (data == null || data.Length == 0)
				return;
			Array.Clear(data, 0, data.Length);
			data = null;
		}

		/// <summary>
		/// Indicates whether the specified array is null or has no elements.
		/// </summary>
		/// <param name="data">The byte array to test.</param>
		/// <returns>True if the data parameter is null or has no elements; otherwise, false.</returns>
		public static bool IsNullOrEmpty<T>(this T[] ts) =>
			ts == null || ts.Length < 1
			? true
			: false;
	}
}