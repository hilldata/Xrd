using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrd.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xrd.Encryption.Tests {
	[TestClass()]
	public class DateTimeEncryptedTests {
		private static Guid k1 = "First Key".HashGuid();
		private static Guid k2 = "Second Key".HashGuid();
		private static Guid k3 = "Third Key".HashGuid();
		private static Guid k4 = "Fourth Key".HashGuid();

		[TestMethod()]
		public void DateTimeEncryptedTest() {
			// Check errors thrown
			// null byte array
			Assert.ThrowsException<ArgumentNullException>(() => new DateTimeEncrypted(null, k1));
			// empty byte array
			Assert.ThrowsException<ArgumentNullException>(() => new DateTimeEncrypted(new byte[0], k1));
			// Empty key
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DateTimeEncrypted(new byte[68], Guid.Empty));
			// null param array for keys
			Assert.ThrowsException<ArgumentNullException>(() => new DateTimeEncrypted(DateTime.Now, true, null));

			// Set the source date and encrypt it.
			DateTime dt = DateTime.Now;
			DateTimeEncrypted dateTimeEncrypted = new DateTimeEncrypted(dt, true, k1, k2, k3, k4);

			// Validate the ClearValue property
			Assert.AreEqual(dt.Month, dateTimeEncrypted.ClearValue);

			// Get the value to assign to a new instance to simulate retrieval from storage.
			byte[] temp = dateTimeEncrypted.Value;

			// Test with wrong initial key
			DateTimeEncrypted wrong = new DateTimeEncrypted(temp, k2);
			DateTime wrongV = wrong.GetValue(k1, k2, k3, k4);
			Assert.AreNotEqual(dt, wrongV);

			// initialize with correct 1st key.
			DateTimeEncrypted dateTimeEncrypted1 = new DateTimeEncrypted(temp, k1);

			// test with various numbers of secondary keys.
			DateTime test1Key = dateTimeEncrypted1.GetValue(k1, k2);
			DateTime test2Key = dateTimeEncrypted1.GetValue(k1, k2, k3);
			DateTime testAllKey = dateTimeEncrypted1.GetValue(k1, k2, k3, k4);

			Assert.AreNotEqual(dt, test1Key);
			Assert.AreNotEqual(dt, test2Key);
			Assert.AreEqual(dt, testAllKey);
		}


		[TestMethod()]
		public void GetAgeTest() {
			DateTime source = new DateTime(2010, 4, 1, 1, 4, 3);
			DateTimeEncrypted encrypted = new DateTimeEncrypted(source, false, k1, k2, k3, k4);
			Assert.AreEqual(10, (int)encrypted.GetAge(k1, k2, k3, k4));
		}
	}
}