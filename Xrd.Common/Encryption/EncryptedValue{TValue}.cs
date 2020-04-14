using System;
using System.Linq;

using Xrd.Uid;

namespace Xrd.Encryption {
	/// <summary>
	/// Abstract based class for encrypting structures.
	/// </summary>
	/// <typeparam name="TValue">Any struct</typeparam>
	public abstract class EncryptedValue<TValue> : INullable where TValue: struct {
		#region Fields
		protected readonly XrdEncryptor cryptoProvider;
		/// <summary>
		/// The length of the byte array that holds the encrypted value.
		/// </summary>
		public const int VALUE_LENGTH = 68;
		#endregion

		#region Constructors
		protected EncryptedValue(byte[] data, Guid key) {
			data.ValidateBinaryData(VALUE_LENGTH);
			if (key.IsEmpty())
				throw new ArgumentOutOfRangeException(nameof(key), "key must not be Guid.Empty");

			Value = data;
			ClearValue = BitConverter.ToInt32(Value, 0);
			cryptoProvider = new XrdEncryptor(key);
		}

		protected EncryptedValue(params Guid[] keys) {
			if (keys.IsNullOrEmpty())
				throw new ArgumentNullException(nameof(keys), "At least one key must be provided.");
			cryptoProvider = new XrdEncryptor(keys[0]);
		}
		#endregion

		#region Properties
		/// <summary>
		/// The value to write to the underlying store/stream.
		/// </summary>
		public byte[] Value { get; private set; }

		/// <summary>
		/// The clear value to be used for lookups/querying.
		/// </summary>
		public int ClearValue { get; protected set; }

		/// <summary>
		/// Is the internal (encrypted) value <see langword="null"/> or an empty array?
		/// </summary>
		public bool IsValueNull => Value.IsNullOrEmpty();
		#endregion

		#region Methods
		/// <summary>
		/// Decrypt and return the value stored.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public abstract TValue GetValue(params Guid[] key);

		/// <summary>
		/// Wipe the encypted value's array.
		/// </summary>
		public void Wipe() => Value.Wipe();

		protected byte[] Decrypt(params Guid[] keys) {
			if (IsValueNull)
				return null;

			Guid[] k1;
			if (!keys.IsNullOrEmpty())
				k1 = keys.Skip(1).ToArray();
			else
				k1 = new Guid[] { ClearValue.HashGuid() };

			byte[] temp = new byte[64];
			Array.Copy(Value, 4, temp, 0, 64);
			return cryptoProvider.Decrypt(temp, k1);
		}

		protected void SetValue(byte[] valAsBytes, params Guid[] keys) {
			Guid[] k1;
			if (keys.Length > 1)
				k1 = keys.Skip(1).ToArray();
			else
				k1 = new Guid[] { ClearValue.HashGuid() };

			Value = new byte[VALUE_LENGTH];
			// Copy the clear value to the first 4 bytes
			Array.Copy(BitConverter.GetBytes(ClearValue), Value, 4);
			// Copy the encrypted value to the remaining 64 bytes.
			Array.Copy(cryptoProvider.Encrypt(valAsBytes, k1), 0, Value, 4, 64);
		}
		#endregion
	}
}
