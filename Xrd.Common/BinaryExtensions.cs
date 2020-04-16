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
		/// Get the total number of bytes required to convert the input into binary data (byte array).
		/// </summary>
		/// <param name="value">The array of objects to convert to binary.</param>
		/// <returns>The total number of bytes required.</returns>
		public static int GetByteLength(params object[] value) {
			if (value.IsNullOrEmpty())
				return 0;
			int res = 0;
			foreach (var v in value) {
				if (v is null)
					continue;
				if (v is byte[] vs)
					res += vs.Length;
				else if (v is string s)
					res += System.Text.Encoding.UTF8.GetBytes(s).Length;
				else if (v is byte)
					res += 1;
				else if (v is short || v is ushort || v is char)
					res += 2;
				else if (v is double || v is long || v is ulong)
					res += 8;
				else if (v is DateTime || v is TimeSpan)
					res += 8;
				else
					res += 4;
			}
			return res;
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
			if (data.IsNullOrEmpty())
				throw new ArgumentNullException(nameof(data));
			else if (data.Length < minLength)
				throw new ArgumentException($"The binary data provided must be at least {minLength} bytes.", nameof(data));
			return true;
		}

		/// <summary>
		/// Clear a binary array and set it to null.
		/// </summary>
		/// <param name="data">The binary array to clear and remove references to.</param>
		public static byte[] Wipe(this byte[] data) {
			if (data == null || data.Length == 0)
				return null;
			Array.Clear(data, 0, data.Length);
#pragma warning disable IDE0059 // Unnecessary assignment of a value
			return null;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
		}

		/// <summary>
		/// Indicates whether the specified array is null or has no elements.
		/// </summary>
		/// <param name="data">The array to test.</param>
		/// <returns>True if the data parameter is null or has no elements; otherwise, false.</returns>
		public static bool IsNullOrEmpty<T>(this T[] ts) =>
			ts == null || ts.Length < 1
			? true
			: false;

		/// <summary>
		/// Indicates whether the specified array is null, has no elements, or all elements are equal to the default value.
		/// </summary>
		/// <typeparam name="T">Any type</typeparam>
		/// <param name="ts">THe array to test.</param>
		/// <returns>False if the array has any non-default elements; otherwise, true.</returns>
		public static bool IsNullOrDefault<T>(this T[] ts) {
			if (ts.IsNullOrEmpty())
				return true;
			foreach(var v in ts) {
				if (!v.Equals(default(T)))
					return false;
			}
			return true;
		}
	}
}