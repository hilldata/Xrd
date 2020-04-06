using System;
using System.Collections.Generic;
using System.Text;

namespace Xrd.Uuid {
	/// <summary>
	/// Extension Methods for Guids
	/// </summary>
	/// <remarks>This is primarily used by the EF/targeting interfaces, but could be useful in other areas, so it is included in the core library.</remarks>
	public static class GuidExtensions {
		/// <summary>
		/// Convert a Guid value to a byte array and then to a url-safe Base64 encoded value.
		/// </summary>
		/// <param name="guid">The value to convert.</param>
		/// <returns>A url-safe Base64 encoded string of the value's byte array.</returns>
		public static string ToUrlSafeBase64(this Guid guid) {
			if (guid == Guid.Empty)
				return string.Empty;
			return guid.ToByteArray().ToUrlSafeBase64();
		}

		/// <summary>
		/// Convert a url-safe Base64 encoded string back into a <see cref="Guid"/>.
		/// </summary>
		/// <param name="urlSafeBase64">The url-safe Base64 encoded string.</param>
		/// <returns>The Guid value that was encoded.</returns>
		public static Guid ToGuidFromBase64(this string urlSafeBase64) {
			string temp = urlSafeBase64.FixBase64String();
			if (string.IsNullOrWhiteSpace(temp))
				throw new ArgumentNullException(nameof(urlSafeBase64));
			if (temp.Length != 24)
				throw new ArgumentOutOfRangeException("UrlSafeBase64-encoded Guids must have 24 characters", nameof(urlSafeBase64));

			return new Guid(Convert.FromBase64String(temp));
		}

		private static Guid ToGuid(this byte[] vs) {
			if (vs == null || vs.Length < 1)
				return Guid.Empty;
			int max = vs.Length;
			if (max > 16)
				max = 16;

			var arrG = new byte[16];
			Array.Copy(vs, arrG, max);
			Array.Reverse(arrG);
			return new Guid(arrG);
		}

		/// <summary>
		/// Convert a <see cref="bool"/> value to a <see cref="Guid"/>
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>A Guid containing the input</returns>
		public static Guid ToGuid(this bool value) =>
			ToGuid(BitConverter.GetBytes(value));
		/// <summary>
		/// Convert a <see cref="byte"/> value to a <see cref="Guid"/>
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>A Guid containing the input</returns>
		public static Guid ToGuid(this byte value) =>
			ToGuid(new byte[1] { value });
		/// <summary>
		/// Convert a <see cref="char"/> value to a <see cref="Guid"/>
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>A Guid containing the input</returns>
		public static Guid ToGuid(this char value) =>
			ToGuid(BitConverter.GetBytes(value));
		/// <summary>
		/// Convert a <see cref="double"/> value to a <see cref="Guid"/>
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>A Guid containing the input</returns>
		public static Guid ToGuid(this double value) =>
			ToGuid(BitConverter.GetBytes(value));
		/// <summary>
		/// Convert a <see cref="float"/> value to a <see cref="Guid"/>
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>A Guid containing the input</returns>
		public static Guid ToGuid(this float value) =>
			ToGuid(BitConverter.GetBytes(value));
		/// <summary>
		/// Convert an <see cref="int"/> value to a <see cref="Guid"/>
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>A Guid containing the input</returns>
		public static Guid ToGuid(this int value) =>
			ToGuid(BitConverter.GetBytes(value));
		/// <summary>
		/// Convert a <see cref="long"/> value to a <see cref="Guid"/>
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>A Guid containing the input</returns>
		public static Guid ToGuid(this long value) =>
			ToGuid(BitConverter.GetBytes(value));
		/// <summary>
		/// Convert a <see cref="short"/> value to a <see cref="Guid"/>
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>A Guid containing the input</returns>
		public static Guid ToGuid(this short value) =>
			ToGuid(BitConverter.GetBytes(value));
		/// <summary>
		/// Convert a <see cref="uint"/> value to a <see cref="Guid"/>
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>A Guid containing the input</returns>
		public static Guid ToGuid(this uint value) =>
			ToGuid(BitConverter.GetBytes(value));
		/// <summary>
		/// Convert a <see cref="ulong"/> value to a <see cref="Guid"/>
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>A Guid containing the input</returns>
		public static Guid ToGuid(this ulong value) =>
			ToGuid(BitConverter.GetBytes(value));
		/// <summary>
		/// Convert a <see cref="ushort"/> value to a <see cref="Guid"/>
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>A Guid containing the input</returns>
		public static Guid ToGuid(this ushort value) =>
			ToGuid(BitConverter.GetBytes(value));
		/// <summary>
		/// Convert an <see cref="object"/> value to a <see cref="Guid"/> by returning its FastHashAsGuid
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>A Guid containing the input</returns>
		public static Guid ToGuid(this object value) =>
			value.HashGuid();

		private static void AddToArray(this byte[] target, byte[] source, ref int pos) {
			for (int i = 0; i < source.Length; i++) {
				target[pos++] = unchecked((byte)(target[pos] ^ source[i]));
				if (pos == 15)
					pos = 0;
			}
		}

		/// <summary>
		/// Convert an array of objects into a <see cref="Guid"/>, but only those that will fit into 16 bytes.
		/// </summary>
		/// <param name="value">An array of objects to be converted/encoded into a <see cref="Guid"/></param>
		/// <returns>A Guid containing the input</returns>
		public static Guid ToGuid(params object[] value) {
			byte[] arrGuid = new byte[16];
			int pos = 0;
			foreach (object v in value) {
				if (v == null)
					continue;
				if (v is Guid)
					arrGuid.AddToArray(BitConverter.GetBytes(((Guid)v).GetHashCode()), ref pos);
				else if (v is byte)
					arrGuid.AddToArray(new byte[] { (byte)v }, ref pos);
				else if (v is char)
					arrGuid.AddToArray(BitConverter.GetBytes((char)v), ref pos);
				else if (v is short)
					arrGuid.AddToArray(BitConverter.GetBytes((short)v), ref pos);
				else if (v is double)
					arrGuid.AddToArray(BitConverter.GetBytes((double)v), ref pos);
				else if (v is float)
					arrGuid.AddToArray(BitConverter.GetBytes((float)v), ref pos);
				else if (v is int)
					arrGuid.AddToArray(BitConverter.GetBytes((int)v), ref pos);
				else if (v is long)
					arrGuid.AddToArray(BitConverter.GetBytes((long)v), ref pos);
				else if (v is short)
					arrGuid.AddToArray(BitConverter.GetBytes((short)v), ref pos);
				else if (v is uint)
					arrGuid.AddToArray(BitConverter.GetBytes((uint)v), ref pos);
				else if (v is ulong)
					arrGuid.AddToArray(BitConverter.GetBytes((ulong)v), ref pos);
				else if (v is ushort)
					arrGuid.AddToArray(BitConverter.GetBytes((ushort)v), ref pos);
				else if (v is object)
					arrGuid.AddToArray(BitConverter.GetBytes(v.GetHashCode()), ref pos);
			}
			return new Guid(arrGuid);
		}

		/// <summary>
		/// Determines if a <see cref="Guid"/> variable is <see cref="Guid.Empty"/>
		/// </summary>
		/// <param name="guid">The value to check.</param>
		/// <returns>Boolean indicating whether or not the value is equal to <see cref="Guid.Empty"/></returns>
		public static bool IsEmpty(this Guid guid) =>
			guid == Guid.Empty;

		/// <summary>
		/// Determines if a <see cref="Guid?"/> variable is NULL or is <see cref="Guid.Empty"/>
		/// </summary>
		/// <param name="guid">The value to check.</param>
		/// <returns>Boolean indicating whether or not the value is NULL or is equal to <see cref="Guid.Empty"/></returns>
		public static bool IsNullOrEmpty(this Guid? guid) =>
			!guid.HasValue || guid.Value == Guid.Empty;
	}
}