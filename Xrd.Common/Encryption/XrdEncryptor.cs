using System;
using Xrd.ChangeTracking;
using System.Collections.Generic;
using System.Text;
using Xrd.Uuid;

namespace Xrd.Encryption {
	/// <summary>
	/// Custom keyed encryption provider for encyrpting data at rest and 
	/// underneath the SSL transport layers.
	/// </summary>
	public sealed class XrdEncryptor {
		#region Fields
		private readonly byte[] key;
		private readonly int sKey;
		private readonly int pFront;
		private readonly XRandom random;

		private byte[] buffer;
		private byte[] temp;
		#endregion

		#region Constructors
		/// <summary>
		/// Create a new instance of the CryptoProvider using the specified key.
		/// </summary>
		/// <param name="passKey">The key to use when encrypting/decrypting data.</param>
		public XrdEncryptor(Guid passKey) {
			if (passKey.IsEmpty())
				throw new ArgumentNullException(nameof(passKey));

			var g = passKey.ToByteArray();

			for (int i = 0; i < 4; i += 4) {
				sKey += g[i];
				sKey *= g[i + 1] == 0 ? 2 : g[i + 1];
				sKey += g[i + 3] == 0 ? 1 : g[i + 3];
				sKey += g[i + 4];
			}
			sKey = Math.Abs(sKey);
			while (sKey > 500)
				sKey /= 2;
			random = new XRandom(passKey);

			while (pFront < 10) {
				pFront += random.Next(true) % 16;
			}
			key = new byte[sKey];
			random.NextBytes(key);
		}

		/// <summary>
		/// Create a new instance of the CryptoProvider using the specified key.
		/// </summary>
		/// <param name="passKey">The key to use when encrypting/decrypting data.</param>
		public XrdEncryptor(string passKey) : this(passKey.HashGuid()) { }
		#endregion

		#region Public Methods
		/// <summary>
		/// Encrypt the specified binary data.
		/// </summary>
		/// <param name="data">The data to be encrypted.</param>
		/// <param name="secondaryPassKeys">An array of Guids to be used as keys on additional encryption passes.</param>
		/// <returns>The encrypted data.</returns>
		public byte[] Encrypt(byte[] data, params Guid[] secondaryPassKeys) {
			data.ValidateBinaryData();

			temp = new byte[pFront];
			random.NextBytes(temp);

			int pBack = 0;
			int lOrig = data.Length;
			int lFinal = lOrig + 20 + pFront;
			if (lFinal < 64)
				lFinal = 64;
			while (lFinal % 16 != 0) {
				lFinal++;
				pBack++;
			}
			buffer = new byte[lFinal];
			byte[] hash = data.FastHash();

			Array.Copy(temp, buffer, pFront);
			Array.Copy(BitConverter.GetBytes(lOrig), 0, buffer, pFront, 4);
			Array.Copy(hash, 0, buffer, pFront + 4, 16);
			Array.Copy(data, 0, buffer, pFront + 20, lOrig);

			if (pBack > 0) {
				temp = new byte[pBack];
				random.NextBytes(temp);
				Array.Copy(temp, 0, buffer, pFront + 20 + lOrig, pBack);
			}
			temp.Wipe();
			shuffle(true, key[10]);
			transform(true);
			shuffle(true, key[16]);
			if(!secondaryPassKeys.IsNullOrEmpty()) {
				for(int i = 1; i< secondaryPassKeys.Length; i++) {
					transform(true, secondaryPassKeys[i].ToByteArray());
				}
			}
			byte[] res = new byte[lFinal];
			Array.Copy(buffer, res, lFinal);
			buffer.Wipe();
			return res;
		}

		/// <summary>
		/// Encrypt the specified binary data.
		/// </summary>
		/// <param name="data">The data to be encrypted.</param>
		/// <param name="secondaryPassKey">The (optional) key to use to run an additional encryption pass on the data.</param>
		/// <returns>The encrypted data.</returns>
		public byte[] Encrypt(byte[] data, string secondaryPassKey) =>
			Encrypt(data, secondaryPassKey.HashGuid());

		/// <summary>
		/// Encrypt the specified text string.
		/// </summary>
		/// <param name="data">The data to be encrypted.</param>
		/// <param name="secondaryPassKeys">An array of Guids to be used as keys on additional encryption passes.</param>
		/// <returns>The encrypted data.</returns>
		public byte[] Encrypt(string data, params Guid[] secondaryPassKeys) {
			if (string.IsNullOrEmpty(data))
				throw new ArgumentNullException(nameof(data));

			return Encrypt(Encoding.UTF8.GetBytes(data), secondaryPassKeys);
		}

		/// <summary>
		/// Encrypt the specified text string.
		/// </summary>
		/// <param name="data">The data to be encrypted.</param>
		/// <param name="secondaryPassKey">The (optional) key to use to run an additional encryption pass on the data.</param>
		/// <returns>The encrypted data.</returns>
		public byte[] Encrypt(string data, string secondaryPassKey) =>
			Encrypt(data, secondaryPassKey.HashGuid());

		/// <summary>
		/// Encrypt the specified DateTime value.
		/// </summary>
		/// <param name="data">The data to be encrypted.</param>
		/// <param name="secondaryPassKeys">An array of Guids to be used as keys on additional encryption passes.</param>
		/// <returns>The encrypted data.</returns>
		public byte[] Encrypt(DateTime data, params Guid[] secondaryPassKeys) =>
			Encrypt(BitConverter.GetBytes(data.Ticks), secondaryPassKeys);

		/// <summary>
		/// Encrypt the specified DateTime value.
		/// </summary>
		/// <param name="data">The data to be encrypted.</param>
		/// <param name="secondaryPassKey">The (optional) key to use to run an additional encryption pass on the data.</param>
		/// <returns>The encrypted data.</returns>
		public byte[] Encrypt(DateTime data, string secondaryPassKey) =>
			Encrypt(data, secondaryPassKey.HashGuid());

		/// <summary>
		/// Decrypt the specified data.
		/// </summary>
		/// <param name="data">The encrypted data to be decrypted.</param>
		/// <param name="secondaryPassKeys">An array of Guids that were used as secondary encryption keys.</param>
		/// <returns>The decrypted data as an array of bytes.</returns>
		public byte[] Decrypt(byte[] data, params Guid[] secondaryPassKeys) {
			data.ValidateBinaryData();
			buffer = new byte[data.Length];
			Array.Copy(data, buffer, data.Length);

			if(!secondaryPassKeys.IsNullOrEmpty()) {
				for(int i = 1; i < secondaryPassKeys.Length; i++) {
					transform(false, secondaryPassKeys[i].ToByteArray());
				}
			}
			shuffle(false, key[16]);
			transform(false);
			shuffle(false, key[10]);
			int lOrig = BitConverter.ToInt32(buffer, pFront);
			byte[] sHash = new byte[16];
			Array.Copy(buffer, pFront + 4, sHash, 0, 16);
			if (lOrig < 0 || lOrig > buffer.Length + 20 + pFront)
				return random.GetBytes(data.Length);

			temp = new byte[lOrig];
			Array.Copy(buffer, pFront + 20, temp, 0, lOrig);
			if (sHash.HasChanges(temp.FastHash()))
				return random.GetBytes(data.Length);

			byte[] res = new byte[lOrig];
			Array.Copy(temp, res, lOrig);
			temp.Wipe();
			return res;
		}

		/// <summary>
		/// Decrypt the specified data.
		/// </summary>
		/// <param name="data">The encrypted data to be decrypted.</param>
		/// <param name="secondaryPassKey">The (optional) key that was used to run an additional encryption pass on the original data.</param>
		/// <returns>The decrypted data as an array of bytes.</returns>
		/// <remarks>If an incorrect passkey or secondary passkey is supplied (detected when the decrypted data's length or hash do not match what was stored during the original encryption), garbage is returned.</remarks>
		public byte[] Decrypt(byte[] data, string secondaryPassKey) =>
			Decrypt(data, secondaryPassKey.HashGuid());

		/// <summary>
		/// Decrypt the specified data back into a string.
		/// </summary>
		/// <param name="data">The encrypted data to be decrypted.</param>
		/// <param name="secondaryPassKeys">An array of Guids that were used as secondary encryption keys.</param>
		/// <returns>The decrypted data as a string.</returns>
		/// <remarks>If an incorrect passkey or secondary passkey is supplied (detected when the decrypted data's length or hash do not match what was stored during the original encryption), garbage is returned.</remarks>
		public string DecryptToString(byte[] data, params Guid[] secondaryPassKeys) =>
			getString(Decrypt(data, secondaryPassKeys));

		private string getString(byte[] input) {
			if (input.IsNullOrEmpty())
				return string.Empty;
			return Encoding.UTF8.GetString(input, 0, input.Length);
		}
		/// <summary>
		/// Decrypt the specified data back into a string.
		/// </summary>
		/// <param name="data">The encrypted data to be decrypted.</param>
		/// <param name="secondaryPassKey">The (optional) key that was used to run an additional encryption pass on the original data.</param>
		/// <returns>The decrypted data as a string.</returns>
		/// <remarks>If an incorrect passkey or secondary passkey is supplied (detected when the decrypted data's length or hash do not match what was stored during the original encryption), garbage is returned.</remarks>
		public string DecryptToString(byte[] data, string secondaryPassKey) =>
			DecryptToString(data, secondaryPassKey.HashGuid());

		/// <summary>
		/// Decrypt the specified data back into a DateTime.
		/// </summary>
		/// <param name="data">The encrypted data to be decrypted.</param>
		/// <param name="secondaryPassKeys">An array of Guids that were used as secondary encryption keys.</param>
		/// <returns>The decrypted data as a DateTime.</returns>
		/// <remarks>If an incorrect passkey or secondary passkey is supplied (detected when the decrypted data's length or hash do not match what was stored during the original encryption), garbage is returned.</remarks>
		public DateTime DecryptToDateTime(byte[] data, params Guid[] secondaryPassKeys) =>
			new DateTime(BitConverter.ToInt64(Decrypt(data, secondaryPassKeys), 0));

		/// <summary>
		/// Decrypt the specified data back into a DateTime.
		/// </summary>
		/// <param name="data">The encrypted data to be decrypted.</param>
		/// <param name="secondaryPassKey">The (optional) key that was used to run an additional encryption pass on the original data.</param>
		/// <returns>The decrypted data as a DateTime.</returns>
		/// <remarks>If an incorrect passkey or secondary passkey is supplied (detected when the decrypted data's length or hash do not match what was stored during the original encryption), garbage is returned.</remarks>
		public DateTime DecryptToDateTime(byte[] data, string secondaryPassKey) =>
			DecryptToDateTime(data, secondaryPassKey.HashGuid());
		#endregion


		#region Private Methods
		private byte transform(byte s, byte v, bool d, int m) {
			switch (m) {
				case 1: return d ? (byte)(s + v) : (byte)(s - v);
				case 2: return d ? (byte)(s - v) : (byte)(s + v);
				default: return (byte)(s ^ v);
			}
		}

		private void transform(bool dir, byte[] vector = null) {
			if (buffer == null)
				return;
			if (vector == null)
				vector = key;
			int s = buffer.Length;
			int v = vector.Length;
			if (s < 1 || v < 1)
				return;

			if (v > s) {
				for (int i = 0; i < s; i++) {
					buffer[i] = transform(buffer[i], vector[i], dir, i % 3);
				}
			} else {
				int p = 0;
				for (int o = 0; o < s / v; o++) {
					for (int i = 0; i < v; i++) {
						if (p >= s)
							return;
						buffer[p] = transform(buffer[p], vector[i], dir, i % 3);
						p++;
					}
				}
			}
		}

		#region Shufflers
		private void s0() {
			temp = new byte[16];
			for (int o = 0; o < buffer.Length / 16; o++) {
				Array.Copy(buffer, o * 16, temp, 0, 16);
				for (int i = 0; i < 8; i++) {
					buffer[o * 16 + i] = temp[i + 8];
					buffer[o * 16 + i + 8] = temp[i];
				}
			}
			temp.Wipe();
		}

		private void s1(bool dir) {
			temp = new byte[4];
			if (dir) {
				for (int i = 0; i < buffer.Length / 4; i++) {
					Array.Copy(buffer, i * 4, temp, 0, 4);
					switch (i % 4) {
						case 3: {
								buffer[i * 4] = temp[3];
								buffer[i * 4 + 1] = temp[0];
								buffer[i * 4 + 2] = temp[1];
								buffer[i * 4 + 3] = temp[2];
								break;
							}
						case 2: {
								buffer[i * 4] = temp[2];
								buffer[i * 4 + 1] = temp[3];
								buffer[i * 4 + 2] = temp[0];
								buffer[i * 4 + 3] = temp[1];
								break;
							}
						case 1: {
								buffer[i * 4] = temp[2];
								buffer[i * 4 + 1] = temp[3];
								buffer[i * 4 + 2] = temp[1];
								buffer[i * 4 + 3] = temp[0];
								break;
							}
						default: {
								buffer[i * 4] = temp[1];
								buffer[i * 4 + 1] = temp[3];
								buffer[i * 4 + 2] = temp[0];
								buffer[i * 4 + 3] = temp[2];
								break;
							}
					}
				}
			} else {
				for (int i = 0; i < buffer.Length / 4; i++) {
					Array.Copy(buffer, i * 4, temp, 0, 4);
					switch (i % 4) {
						case 3: {
								buffer[i * 4] = temp[1];
								buffer[i * 4 + 1] = temp[2];
								buffer[i * 4 + 2] = temp[3];
								buffer[i * 4 + 3] = temp[0];
								break;
							}
						case 2: {
								buffer[i * 4] = temp[2];
								buffer[i * 4 + 1] = temp[3];
								buffer[i * 4 + 2] = temp[0];
								buffer[i * 4 + 3] = temp[1];
								break;
							}
						case 1: {
								buffer[i * 4] = temp[3];
								buffer[i * 4 + 1] = temp[2];
								buffer[i * 4 + 2] = temp[0];
								buffer[i * 4 + 3] = temp[1];
								break;
							}
						default: {
								buffer[i * 4] = temp[2];
								buffer[i * 4 + 1] = temp[0];
								buffer[i * 4 + 2] = temp[3];
								buffer[i * 4 + 3] = temp[1];
								break;
							}
					}
				}
			}
			temp.Wipe();
		}

		private void s2(bool dir) {
			int q = buffer.Length / 4;
			temp = new byte[buffer.Length];
			Array.Copy(buffer, temp, temp.Length);
			for (int i = 0; i < 4; i++) {
				if (dir) {
					buffer[i + 2 * q] = temp[i];
					buffer[i + 3 * q] = temp[i + q];
					buffer[i + q] = temp[i + 2 * q];
					buffer[i] = temp[i + 3 * q];
				} else {
					buffer[i] = temp[i + 2 * q];
					buffer[i + q] = temp[i + 3 * q];
					buffer[i + 2 * q] = temp[i + q];
					buffer[i + 3 * q] = temp[i];
				}
			}
			temp.Wipe();
		}

		private void s3(bool dir) {
			temp = new byte[buffer.Length];
			Array.Copy(buffer, temp, buffer.Length);
			if (dir) {
				Array.Reverse(temp);
				for (int i = 0; i < buffer.Length / 8; i++) {
					buffer[i * 8 + 0] = temp[i * 8 + 2];
					buffer[i * 8 + 1] = temp[i * 8 + 7];
					buffer[i * 8 + 2] = temp[i * 8 + 1];
					buffer[i * 8 + 3] = temp[i * 8 + 6];
					buffer[i * 8 + 4] = temp[i * 8 + 5];
					buffer[i * 8 + 5] = temp[i * 8 + 4];
					buffer[i * 8 + 6] = temp[i * 8 + 0];
					buffer[i * 8 + 7] = temp[i * 8 + 3];
				}
			} else {
				for (int i = 0; i < buffer.Length / 8; i++) {
					buffer[i * 8 + 0] = temp[i * 8 + 6];
					buffer[i * 8 + 1] = temp[i * 8 + 2];
					buffer[i * 8 + 2] = temp[i * 8 + 0];
					buffer[i * 8 + 3] = temp[i * 8 + 7];
					buffer[i * 8 + 4] = temp[i * 8 + 5];
					buffer[i * 8 + 5] = temp[i * 8 + 4];
					buffer[i * 8 + 6] = temp[i * 8 + 3];
					buffer[i * 8 + 7] = temp[i * 8 + 1];
				}
				Array.Reverse(buffer);
			}
			temp.Wipe();
		}
		#endregion

		private void shuffle(bool dir, byte m) {
			switch (m % 4) {
				case 3:
					s3(dir);
					break;
				case 2:
					s2(dir);
					break;
				case 1:
					s1(dir);
					break;
				default:
					s0();
					break;
			}
		}
		#endregion
	}
}