using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrd;
using System;
using System.Collections.Generic;
using System.Text;
using Xrd.ChangeTracking;

namespace Xrd.Tests {
	[TestClass()]
	public class BinaryExtensionsTests {
		private Xrd.Encryption.XRandom random = new Encryption.XRandom(Guid.NewGuid());

		[TestMethod()]
		public void UrlSafeBase64Test() {
			byte[] vs = new byte[64];
			random.NextBytes(vs);

			var s = vs.ToUrlSafeBase64();
			byte[] vs1 = s.ToBinaryFromBase64();
			Assert.IsFalse(vs.HasChanges(vs1));
		}

		[TestMethod()]
		public void GetByteLengthTest() {
			int i1 = 0;
			int i2 = 0;
			Assert.AreEqual(8, BinaryExtensions.GetByteLength(i1, i2));
			short s1 = 0;
			Assert.AreEqual(10, BinaryExtensions.GetByteLength(i1, i2, s1));
			long l1 = 0;
			Assert.AreEqual(18, BinaryExtensions.GetByteLength(i1, i2, s1, l1));
		}

		private static byte[] vs1 = { 1, 2};
		private static byte[] vs2 = { 3, 4, 5 };
		private static byte[] vs3 = { 6, 7, 8, 9 };
		private static byte[] vs4 = { 10, 11, 12, 13, 14 };
		[TestMethod()]
		public void ConcatenateTest() { 
			byte[] exp = new byte[] { 1, 2, 3, 4, 5 };

			byte[] res = vs1.Concatenate(vs2);
			Assert.AreEqual(5, res.Length);
			Assert.IsFalse(exp.HasChanges(res));
		}

		[TestMethod()]
		public void ConcatenateTest1() {
			byte[] res = vs1.Concatenate(vs2, vs3);
			byte[] exp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			Assert.AreEqual(9, res.Length);
			Assert.IsFalse(exp.HasChanges(res));
		}

		[TestMethod()]
		public void ConcatenateTest2() {
			byte[] exp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
			byte[] res = vs1.Concatenate(vs2, vs3, vs4);
			Assert.AreEqual(14, res.Length);
			Assert.IsFalse(exp.HasChanges(res));
		}

		[TestMethod()]
		public void ConcatenateTest3() {
			List<byte[]> input = new List<byte[]>() { vs1, vs2, vs3, vs4 };
			byte[] exp = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
			var res = input.Concatenate();
			Assert.AreEqual(14, res.Length);
			Assert.IsFalse(res.HasChanges(exp));
		}

		[TestMethod()]
		public void ValidateBinaryDataTest() {
			byte[] vs = null;
			Assert.ThrowsException<ArgumentNullException>(() => vs.ValidateBinaryData());
			vs = new byte[0];
			Assert.ThrowsException<ArgumentNullException>(() => vs.ValidateBinaryData());
			vs = new byte[10];
			Assert.IsTrue(vs.ValidateBinaryData());
			Assert.ThrowsException<ArgumentException>(() => vs.ValidateBinaryData(20));
		}

		[TestMethod()]
		public void WipeTest() {
			byte[] vs = vs1.Concatenate(vs2);
			Assert.IsNotNull(vs);
			Assert.IsFalse(vs.IsNullOrDefault());
			vs.Wipe();
			Assert.IsTrue(vs.IsNullOrDefault());
			vs = vs.Wipe();
			Assert.IsNull(vs);
		}

		[TestMethod()]
		public void IsNullOrEmptyTest() {
			byte[] vs = null;
			Assert.IsTrue(vs.IsNullOrEmpty());
			vs = new byte[0];
			Assert.IsTrue(vs.IsNullOrEmpty());
			vs = vs1.Concatenate(vs2);
			Assert.IsFalse(vs.IsNullOrEmpty());
		}
	}
}