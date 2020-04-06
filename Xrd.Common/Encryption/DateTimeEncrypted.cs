using System;

namespace Xrd.Encryption {
	/// <summary>
	/// Class used to securely store a date/time value (the actual date is encrypted, 
	/// and only the month portion is available without the key).
	/// </summary>
	public sealed class DateTimeEncrypted : EncryptedValue<DateTime> {
		#region Constructors
		/// <summary>
		/// Construct an instance from the encrypted value.
		/// </summary>
		/// <param name="data">The encrypted data.</param>
		/// <param name="key">The primary encryption key.</param>
		public DateTimeEncrypted(byte[] data, Guid key) : base(data, key) { }

		/// <summary>
		/// Create a new instance of the class
		/// </summary>
		/// <param name="value">The date/time to be encrypted.</param>
		/// <param name="includeTime">Should the Time portion of the value be included (or just the Date portion)?</param>
		/// <param name="keys">An array of encryption keys.</param>
		public DateTimeEncrypted(DateTime value, bool includeTime, params Guid[] keys) : base(keys) {
			ClearValue = value.Month;
			long ticks = includeTime ? value.Ticks:value.Date.Ticks;
			SetValue(BitConverter.GetBytes(ticks), keys);
		}

		/// <summary>
		/// Create a new instance of the class
		/// </summary>
		/// <param name="input">A string representation of the date/time to be encrypted.</param>
		/// <param name="includeTime">Should the Time portion of the value be included (or just the Date portion)?</param>
		/// <param name="keys">An array of encryption keys.</param>
		public DateTimeEncrypted(string input, bool includeTime, params Guid[] keys) : base(keys) {
			if(DateTime.TryParse(input, out DateTime res)) {
				ClearValue = res.Month;
				long ticks = includeTime ? res.Ticks : res.Date.Ticks;
				SetValue(BitConverter.GetBytes(ticks), keys);
			} else {
				Wipe();
				throw new ArgumentException($"The value provided [{input}] could not be parsed into a DateTime value.");
			}
		}
		#endregion

		/// <summary>
		/// Decrypt the value
		/// </summary>
		/// <param name="key">The (optional) secondary encryption key.</param>
		/// <returns>The decrypted value</returns>
		public override DateTime GetValue(params Guid[] key) {
			if (IsValueNull)
				return DateTime.MinValue;
			var temp = Decrypt(key);
			try {
				return new DateTime(BitConverter.ToInt64(temp, 0));
			}catch {
				return DateTime.MinValue;
			}
		}

		/// <summary>
		/// If the value is a date of birth, return the age (in years).
		/// </summary>
		/// <param name="key">The (optional) secondary encryption key.</param>
		/// <returns>The age, in years, if the value is a date of birth.</returns>
		public double? GetAge(params Guid[] key) {
			if (GetValue(key) == DateTime.MinValue)
				return null;
			else
				return (DateTime.Now - GetValue(key)).TotalDays / 365.25;
		}
	}
}
