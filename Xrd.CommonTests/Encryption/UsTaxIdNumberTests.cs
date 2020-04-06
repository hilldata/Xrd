using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrd.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xrd.Encryption.Tests {
	[TestClass()]
	public class UsTaxIdNumberTests {
		private static Guid k1 = "First Key".HashGuid();
		private static Guid k2 = "Second Key".HashGuid();
		private static Guid k3 = "Third Key".HashGuid();
		private static Guid k4 = "Fourth Key".HashGuid();

		[TestMethod()]
		public void UsTaxIdNumberTest() {
			// Set the source.
			string source = "123-45-678";
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => new UsTaxIdNumber(source, k1, k2, k3, k4));
			source += "9";
			UsTaxIdNumber usTax = new UsTaxIdNumber(source, k1, k2, k3, k4);
			Assert.AreEqual("6789", usTax.LastFour);

			byte[] encry = usTax.Value;
			UsTaxIdNumber usTax1 = new UsTaxIdNumber(encry, k1);
			Assert.AreEqual(6789, usTax1.ClearValue);

			Assert.AreNotEqual(source, usTax1.ToSSN(k1, k2));
			Assert.AreEqual(source, usTax1.ToSSN(k1, k2, k3, k4));
			Assert.AreEqual("12-3456789", usTax1.ToEIN(k1, k2, k3, k4));
		}

		[TestMethod()]
		public void UsTaxIdNumberTest1() {
			ulong source = 123456789;
			UsTaxIdNumber usTax = new UsTaxIdNumber(source, k1, k2, k3, k4);
			Assert.AreEqual("6789", usTax.LastFour);

			byte[] encry = usTax.Value;
			UsTaxIdNumber usTax1 = new UsTaxIdNumber(encry, k1);
			Assert.AreEqual(6789, usTax1.ClearValue);

			Assert.AreNotEqual(source.ToString("000-00-0000"), usTax1.ToSSN(k1, k2));
			Assert.AreEqual(source.ToString("000-00-0000"), usTax1.ToSSN(k1, k2, k3, k4));
			Assert.AreEqual("12-3456789", usTax1.ToEIN(k1, k2, k3, k4));
		}
	}
}