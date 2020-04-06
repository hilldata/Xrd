using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Xrd.ChangeTracking;

namespace Xrd.Encryption.Tests {
	[TestClass()]
	public class XRandomTests {
		[TestMethod()]
		public void XRandomTest() {
			Guid g1 = Guid.NewGuid();
			Guid g2 = Guid.NewGuid();

			XRandom x1 = new XRandom(g1);
			XRandom x11 = new XRandom(g1);
			XRandom x2 = new XRandom(g2);

			byte[] b1 = x1.GetBytes(100);
			byte[] b11 = x11.GetBytes(100);
			byte[] b2 = x2.GetBytes(100);

			Assert.IsFalse(b1.HasChanges(b11));
			Assert.IsTrue(b1.HasChanges(b2));
		}

		[TestMethod()]
		public void XRandomTest1() {
			XRandom x1 = new XRandom(987654321);
			XRandom x11 = new XRandom(987654321);
			XRandom x2 = new XRandom(123456789);

			byte[] b1 = x1.GetBytes(100);
			byte[] b11 = x11.GetBytes(100);
			byte[] b2 = x2.GetBytes(100);

			Assert.IsFalse(b1.HasChanges(b11));
			Assert.IsTrue(b1.HasChanges(b2));
		}

		[TestMethod()]
		public void NextTest() {
			Guid g1 = Guid.NewGuid();
			Guid g2 = Guid.NewGuid();

			XRandom x1 = new XRandom(g1);
			XRandom x11 = new XRandom(g1);
			XRandom x2 = new XRandom(g2);

			Assert.AreEqual(x1.Next(), x11.Next());
			Assert.AreNotEqual(x1.Next(), x2.Next());
		}

		[TestMethod()]
		public void NextBytesTest() {
			XRandom x1 = new XRandom(987654321);
			XRandom x11 = new XRandom(987654321);
			XRandom x2 = new XRandom(123456789);

			byte[] b1 = new byte[100];
			byte[] b11 = new byte[100];
			byte[] b2 = new byte[100];
			x1.NextBytes(b1);
			x11.NextBytes(b11);
			x2.NextBytes(b2);

			Assert.IsFalse(b1.HasChanges(b11));
			Assert.IsTrue(b1.HasChanges(b2));
		}

		[TestMethod()]
		public void GetBytesTest() {
			// Already handled in the two contructor tests.
		}
	}
}