using System;
using System.Text;

using Xrd.Text;

namespace Xrd.Encryption {
	public sealed class UsTaxIdNumber : EncryptedValue<ulong> {
		#region Constructors
		/// <summary>
		/// Constuct an instance from the encrypted value.
		/// </summary>
		/// <param name="data">The encrypted value.</param>
		/// <param name="key">The primary encryption key.</param>
		public UsTaxIdNumber(byte[] data, Guid key) : base(data, key) { }

		/// <summary>
		/// Construct an instance with the specified value/keys
		/// </summary>
		/// <param name="value">The clear value of the TaxID# as a ulong.</param>
		/// <param name="keys">An array of encryption keys.</param>
		public UsTaxIdNumber(ulong value, params Guid[] keys) : base(keys) {
			string s = value.ToString();
			if (s.Length <= 4)
				ClearValue = int.Parse(s);
			else {
				s = s.Substring(s.Length - 4);
				ClearValue = int.Parse(s);
			}
			SetValue(BitConverter.GetBytes(value), keys);
		}

		/// <summary>
		/// Construct an instance with the specified text value and keys
		/// </summary>
		/// <param name="value">A test representation of the clear value.</param>
		/// <param name="keys">An array of encryption keys</param>
		public UsTaxIdNumber(string value, params Guid[] keys) : base(keys) {
			StringBuilder sb = new StringBuilder();
			if (!value.HasValue())
				throw new ArgumentNullException(nameof(value));
			foreach(var c in value.ToCharArray()) {
				if (char.IsDigit(c))
					sb.Append(c);
			}
			if(sb.Length != 9)
				throw new ArgumentOutOfRangeException($"The value provided [{value}] does not contain exactly nine numbers.");
			string s = sb.ToString();
			ClearValue = int.Parse(s.Substring(s.Length - 4));
			SetValue(BitConverter.GetBytes(ulong.Parse(s)), keys);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the last four digits of the TaxID Number.
		/// </summary>
		public string LastFour =>
			ClearValue.ToString("0000");
		#endregion

		/// <summary>
		/// Decrypt and return the underlying US TaxID # as a ulong.
		/// </summary>
		/// <param name="key">The (optional) secondary encryption keys.</param>
		/// <returns>The Tax ID # as a unsigned long integer.</returns>
		public override ulong GetValue(params Guid[] key) {
			if (IsValueNull)
				return 0;
			var temp = Decrypt(key);
			try {
				return BitConverter.ToUInt64(temp, 0);
			}catch {
				return 0;
			}
		}

		/// <summary>
		/// Decrypt and return the value of the TaxIdNumber formatted as a Social Security Number.
		/// </summary>
		/// <param name="key">The (optional) secondary encryption key.</param>
		/// <returns>The value formatted as a U.S. Social Security Number.</returns>
		public string ToSSN(params Guid[] key) =>
			GetValue(key).ToString("000-00-0000");

		/// <summary>
		/// Decrypt and return the value of the TaxIdNumber formatted as an Employer Identification Number.
		/// </summary>
		/// <param name="key">The (optional) secondary encryption key.</param>
		/// <returns>The value formatted as an U.S. Employer Identification Number.</returns>
		public string ToEIN(params Guid[] key) =>
			GetValue(key).ToString("00-0000000");

		public override string ToString() => LastFour;
	}
}
