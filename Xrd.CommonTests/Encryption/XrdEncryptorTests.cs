using System;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Xrd.ChangeTracking;

namespace Xrd.Encryption.Tests {
	[TestClass()]
	public class XrdEncryptorTests {
		private static Guid[] toArray(Tuple<Guid, Guid, Guid, Guid> tuple) =>
			new Guid[] { tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4 };

		private static Tuple<Guid, Guid, Guid, Guid> _keys1;
		private static Tuple<Guid,Guid, Guid, Guid> Keys1 {
			get {
				if(_keys1 == null) {
					_keys1 = new Tuple<Guid, Guid, Guid, Guid>(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
				}
				return _keys1;
			}
		}
		private static Tuple<string, string> Keys2 = new Tuple<string, string>("ThisIsTheFirstKey", "SecondKey");

		[TestMethod()]
		public void XrdEncryptorTest() {
			XrdEncryptor x;
			Assert.ThrowsException<ArgumentNullException>(() => x= new XrdEncryptor(Guid.Empty));
			x = new XrdEncryptor(Guid.NewGuid());
			Assert.IsNotNull(x);
		}

		[TestMethod()]
		public void XrdEncryptorTest1() {
			string s = null;
			XrdEncryptor x;
			Assert.ThrowsException<ArgumentNullException>(() => x = new XrdEncryptor(s));
			s = "SomeKey";
			x = new XrdEncryptor(s);
			Assert.IsNotNull(x);
		}

		[TestMethod()]
		public void EncryptTest() {
			var x = new XrdEncryptor(Keys1.Item1);
			byte[] clear = null;
			Assert.ThrowsException<ArgumentNullException>(() => x.Encrypt(clear));
			Random r = new Random();
			clear = new byte[100];
			r.NextBytes(clear);
			byte[] mixed1 = x.Encrypt(clear);
			byte[] mixed2 = x.Encrypt(clear, toArray(Keys1));
			
			// Verify all arrays are different.
			Assert.IsTrue(clear.HasChanges(mixed1));
			Assert.IsTrue(clear.HasChanges(mixed2));
			Assert.IsTrue(mixed2.HasChanges(mixed1));

			// Check decryption.
			Assert.IsFalse(clear.HasChanges(x.Decrypt(mixed1)));
			Assert.IsFalse(clear.HasChanges(x.Decrypt(mixed2, toArray(Keys1))));
			// Should fail - wrong key.
			Assert.IsTrue(clear.HasChanges(x.Decrypt(mixed2, Keys1.Item1)));

			// Verify gets result but wrong with different encryptor with different key.
			var x1 = new XrdEncryptor(Keys2.Item1);
			Assert.IsTrue(clear.HasChanges(x1.Decrypt(mixed1)));

			// Verify decryption with new encryptor instance.
			var x2 = new XrdEncryptor(Keys1.Item1);
			Assert.IsFalse(clear.HasChanges(x2.Decrypt(mixed1)));
			Assert.IsFalse(clear.HasChanges(x2.Decrypt(mixed2, toArray(Keys1))));

		}

		[TestMethod()]
		public void EncryptTest1() {
			var x = new XrdEncryptor(Keys2.Item1);
			byte[] clear = null;
			Assert.ThrowsException<ArgumentNullException>(() => x.Encrypt(clear));
			Random r = new Random();
			clear = new byte[100];
			r.NextBytes(clear);
			byte[] mixed1 = x.Encrypt(clear);
			byte[] mixed2 = x.Encrypt(clear, Keys2.Item2);
			Assert.IsTrue(clear.HasChanges(mixed1));
			Assert.IsTrue(clear.HasChanges(mixed2));
			Assert.IsTrue(mixed2.HasChanges(mixed1));

			Assert.IsFalse(clear.HasChanges(x.Decrypt(mixed1)));
			Assert.IsFalse(clear.HasChanges(x.Decrypt(mixed2, Keys2.Item2)));
		}

		[TestMethod()]
		public void EncryptTest2() {
			var x = new XrdEncryptor(Keys1.Item1);
			string clear = null;
			Assert.ThrowsException<ArgumentNullException>(() => x.Encrypt(clear));
			clear = "This is some secret that I want to hide.";
			Console.WriteLine("Clear:\t{0}", clear);
			byte[] bClear = Encoding.UTF8.GetBytes(clear);
			byte[] mixed1 = x.Encrypt(clear);
			byte[] mixed2 = x.Encrypt(clear, toArray(Keys1));

			// Verify all arrays are different.
			Assert.IsTrue(bClear.HasChanges(mixed1));
			Assert.IsTrue(bClear.HasChanges(mixed2));
			Assert.IsTrue(mixed2.HasChanges(mixed1));

			// Check decryption.
			string d1 = x.DecryptToString(mixed1);
			Assert.IsFalse(clear.HasChanges(d1));
			Console.WriteLine("D1:\t{0}", d1);
			string d2 = x.DecryptToString(mixed2, toArray(Keys1));
			Assert.IsFalse(clear.HasChanges(d2));
			Console.WriteLine("D2:\t{0}", d2);

			// Should fail - wrong key.
			string w1 = x.DecryptToString(mixed2, Keys1.Item1);
			Assert.IsTrue(clear.HasChanges(w1));
			Console.WriteLine("Wrong1:\t{0}", w1);
			// Should fail - not enough keys
			string w2 = x.DecryptToString(mixed2, Keys1.Item2);
			Assert.IsTrue(clear.HasChanges(w2));
			Console.WriteLine("Wrong2:\t{0}", w2);

			// Verify gets result but wrong with different encryptor with different key.
			var x1 = new XrdEncryptor(Keys2.Item1);
			Assert.IsTrue(clear.HasChanges(x1.DecryptToString(mixed1)));

			// Verify decryption with new encryptor instance.
			var x2 = new XrdEncryptor(Keys1.Item1);
			Assert.IsFalse(clear.HasChanges(x2.DecryptToString(mixed1)));
			Assert.IsFalse(clear.HasChanges(x2.DecryptToString(mixed2, toArray(Keys1))));

			var mixed3 = x.Encrypt(clear, Keys2.Item2);
			Assert.IsFalse(clear.HasChanges(x.DecryptToString(mixed3, Keys2.Item2)));
		}

		[TestMethod()]
		public void EncryptTest3() {
			var x = new XrdEncryptor(Keys1.Item1);
			DateTime clear = DateTime.Now;
			byte[] bClear = BitConverter.GetBytes(clear.Ticks);
			byte[] mixed1 = x.Encrypt(clear);
			byte[] mixed2 = x.Encrypt(clear, Keys1.Item2);

			// Verify all arrays are different.
			Assert.IsTrue(bClear.HasChanges(mixed1));
			Assert.IsTrue(bClear.HasChanges(mixed2));
			Assert.IsTrue(mixed2.HasChanges(mixed1));

			// Check decryption.
			Assert.IsFalse(clear.HasChanges(x.DecryptToDateTime(mixed1)));
			Assert.IsFalse(clear.HasChanges(x.DecryptToDateTime(mixed2, Guid.NewGuid())));
			// Should fail - wrong key.
			Assert.IsFalse(clear.HasChanges(x.DecryptToDateTime(mixed2, Guid.NewGuid())));

			// Verify gets result but wrong with different encryptor with different key.
			var x1 = new XrdEncryptor(Keys2.Item1);
			Assert.IsTrue(clear.HasChanges(x1.DecryptToDateTime(mixed1)));

			// Verify decryption with new encryptor instance.
			var x2 = new XrdEncryptor(Keys1.Item1);
			Assert.IsFalse(clear.HasChanges(x2.DecryptToDateTime(mixed1)));
			Assert.IsFalse(clear.HasChanges(x2.DecryptToDateTime(mixed2, Keys1.Item2)));

			var mixed3 = x.Encrypt(clear, Keys2.Item2);
			Assert.IsFalse(clear.HasChanges(x.DecryptToDateTime(mixed3, Keys2.Item2)));
		}
	}
}