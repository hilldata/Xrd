using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xrd.Collections.Tests {
	[TestClass()]
	public class CollectionExtensionsTests {
		[TestMethod()]
		public void IsNullOrEmpty_TestNull() {
			// Arrange
			List<object> list = null;
			object[] vs = null;

			// Assert
			Assert.IsTrue(list.IsNullOrEmpty());
			Assert.IsTrue(vs.IsNullOrEmpty());
		}

		[TestMethod()]
		public void IsNullOrEmpty_TestEmpty() {
			// Arrange
			List<object> list = new List<object>();
			object[] vs = new object[0];

			// Assert
			Assert.IsTrue(list.IsNullOrEmpty());
			Assert.IsTrue(vs.IsNullOrEmpty());
		}

		[TestMethod()]
		public void IsNullOrEmpty_TestHasItems() {
			// Arrange
			List<object> list = new List<object>() { 1, 2, "string" };
			object[] vs = new object[] { 1, 2, 3 };

			// Assert
			Assert.IsFalse(list.IsNullOrEmpty());
			Assert.IsFalse(vs.IsNullOrEmpty());
		}
		[TestMethod()]
		public void AddIfNotNull_TestExceptions() {
			// Arrange
			List<object> list = null;

			// Test null source throws exception
			Assert.ThrowsException<ArgumentNullException>(() => list.AddIfNotNull(1));
		}

		[TestMethod()]
		public void AddIfNotNull_TestAdding() {
			// Arrange
			var list = new List<object>();
			object item = null;

			// Test null is not added.
			Assert.IsFalse(list.AddIfNotNull(item));

			// Arrange
			item = 1;
			// Test not null is added
			Assert.IsTrue(list.AddIfNotNull(item));

			// Test enforceUnique: true => not added
			Assert.IsFalse(list.AddIfNotNull(item, true));

			// Test enforceUnique: false => duplicate added
			Assert.IsTrue(list.AddIfNotNull(item));

			// Make sure list count is now 2
			Assert.AreEqual(2, list.Count);
		}

		public void AddRangeIfNotNull_TestException() {
			// Arrange
			List<object> list = null;

			// Test null source throws exception
			Assert.ThrowsException<ArgumentNullException>(() => list.AddRangeIfNotNull(_itemsToAdd));
		}

		private static object[] _itemsToAdd = new object[] { null, 1, 2, "string" };
		[TestMethod()]
		public void AddRangeIfNotNull_TestAdding() {
			// Arrange
			var list = new List<object>();

			// Test null item is not added.
			var res = list.AddRangeIfNotNull(_itemsToAdd);
			Assert.AreNotEqual(_itemsToAdd.Length, res);
			Assert.AreEqual(3, res);

			// Test enforceUnique: true => not added
			res = list.AddRangeIfNotNull(_itemsToAdd, true);
			Assert.AreEqual(0, res);

			// Test enforceUnique: false => duplicates added
			res = list.AddRangeIfNotNull(_itemsToAdd);
			Assert.AreEqual(3, res);

			// Make sure list count is now 6
			Assert.AreEqual(6, list.Count);
		}
	}
}