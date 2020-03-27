using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrd.Collections;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace Xrd.Collections.Tests {
	internal class TestPaginatedResults : PaginatedResults {
		private bool _canSearch;
		internal TestPaginatedResults(bool canSearch = false) : base() {
			_canSearch = canSearch;
		}
		public override bool CanSearch => _canSearch;
		protected async override Task<bool> PerformWork() {
			await Task.Delay(10);
			return true;
		}

		internal static TestPaginatedResults UninitializedResults =>
			new TestPaginatedResults(false);
		internal static TestPaginatedResults InitializedButNoResults =>
			new TestPaginatedResults(true);
		internal static TestPaginatedResults InitializedWithResults =>
			new TestPaginatedResults(true) {
				TotalItems = 100,
				PageSize = 25,
				PageIndex = 1
			};
	}
	[TestClass()]
	public class PaginatedResultsTests {
		[TestMethod()]
		public void CanSearch_TestWhenUnready() {
			// Arrange
			TestPaginatedResults set = TestPaginatedResults.UninitializedResults;

			// Assert
			Assert.IsFalse(set.CanSearch);
		}

		[TestMethod()]
		public void CanSearch_TestWhenReady() {
			TestPaginatedResults set = TestPaginatedResults.InitializedButNoResults;

			Assert.IsTrue(set.CanSearch);
		}

		[TestMethod()]
		public void CanMoveBack_TestWhenUnready() {
			// Arrange
			TestPaginatedResults set = TestPaginatedResults.UninitializedResults;

			Assert.IsFalse(set.CanMoveBack);
		}

		[TestMethod()]
		public void CanMoveBack_TestWhenSearchable() {
			var set = TestPaginatedResults.InitializedButNoResults;

			Assert.IsFalse(set.CanMoveBack);
		}

		[TestMethod()]
		public void CanMoveBack_TestWhenReadyPage1() {
			var set = TestPaginatedResults.InitializedWithResults;

			Assert.IsFalse(set.CanMoveBack);
		}

		[TestMethod()]
		public void CanMoveBack_TestWhenReadyPage2() {
			var set = TestPaginatedResults.InitializedWithResults;

			set.PageIndex = 3;

			Assert.IsTrue(set.CanMoveBack);
		}

		[TestMethod()]
		public async Task MoveFirst_TestWhenPage1() {
			var set = TestPaginatedResults.InitializedWithResults;
			Assert.IsFalse(await set.MoveFirst());
		}

		[TestMethod()]
		public async Task MoveFirst_TestWhenPage2() {
			var set = TestPaginatedResults.InitializedWithResults;
			set.PageIndex = 2;
			Assert.IsTrue(await set.MoveFirst());
		}

		[TestMethod()]
		public async Task MovePrevious_Test() {
			var set = TestPaginatedResults.InitializedWithResults;
			set.PageIndex = 2;
			Assert.IsTrue(await set.MovePrevious());
		}

		[TestMethod()]
		public async Task JumpToPageTest() {
			var set = TestPaginatedResults.InitializedWithResults;
			Assert.IsTrue(await set.JumpToPage(3));
		}

		[TestMethod()]
		public async Task MoveNextTest() {
			var set = TestPaginatedResults.InitializedWithResults;
			Assert.IsFalse(await set.MoveFirst());
			Assert.IsTrue(await set.MoveNext());
		}

		[TestMethod()]
		public async Task MoveLastTest() {
			var set = TestPaginatedResults.InitializedWithResults;
			Assert.IsTrue(await set.MoveLast());
		}

		[TestMethod()]
		public async Task MoveFirstTest() {
			var set = TestPaginatedResults.InitializedWithResults;
			set.PageIndex = 3;
			Assert.IsTrue(await set.MoveFirst());
		}
	}
}