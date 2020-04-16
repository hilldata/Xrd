using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrd.Text.DiffMatchPatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xrd.Text.DiffMatchPatch.Tests {
	[TestClass()]
	public class PatchExtensionsTests {
		private const string FINAL = "The slow orange fox snuck by the vigilant yellow dog.";
		private const string ORIGINAL = "The quick red fox jumped over the lazy brown dog.";
		
		private List<PatchOp> makePatch() {
			return ORIGINAL.MakePatch(FINAL);
		}
		private List<DiffOp> makeDiff() {
			return ORIGINAL.GetDiffs(FINAL);
		}

		[TestMethod]
		public void PatchTest() {
			var patch = makePatch();
			string working = "The quick red fox jumped over the lazy brown dog.";
			Assert.AreNotEqual(working, FINAL);
			var patched = working.ApplyPatch(patch);
			Assert.AreEqual(FINAL, patched.Item1);
			for (int i = 0; i < patch.Count; i++)
				Assert.IsTrue(patched.Item2[i]);
			Assert.IsTrue(patched.Item2.DidPatchSucceed());

			string patchText = patch.PatchToText();
			Assert.IsTrue(patchText.IsStringPatch());
			Assert.IsFalse(ORIGINAL.IsStringPatch());
			Assert.AreEqual(FINAL, working.ApplyPatch(patchText.PatchFromText()).Item1);
		}

		[TestMethod()]
		public void MakePatchTest() {
			var patch = makePatch();
			Assert.IsTrue(patch.PatchToText().IsStringPatch());
			var res = ORIGINAL.ApplyPatch(patch);
			Assert.IsTrue(res.Item2.DidPatchSucceed());
			Assert.AreEqual(FINAL, res.Item1);
		}

		[TestMethod()]
		public void MakePatchTest1() {
			var patch = ORIGINAL.MakePatch(makeDiff());
			Assert.IsTrue(patch.PatchToText().IsStringPatch());
			var res = ORIGINAL.ApplyPatch(patch);
			Assert.IsTrue(res.Item2.DidPatchSucceed());
			Assert.AreEqual(FINAL, res.Item1);
		}

		[TestMethod()]
		public void MakePatchTest2() {
			var patch = PatchExtensions.MakePatch(makeDiff());
			Assert.IsTrue(patch.PatchToText().IsStringPatch());
			var res = ORIGINAL.ApplyPatch(patch);
			Assert.IsTrue(res.Item2.DidPatchSucceed());
			Assert.AreEqual(FINAL, res.Item1);
		}

		[TestMethod()]
		public void ApplyPatchTest() {
			var patch = ORIGINAL.MakePatch(FINAL);
			string working = "The quick red fox jumped over the lazy brown dog.";
			Assert.AreNotEqual(working, FINAL);
			var patched = working.ApplyPatch(patch);
			Assert.AreEqual(FINAL, patched.Item1);
			for (int i = 0; i < patch.Count; i++)
				Assert.IsTrue(patched.Item2[i]);
			Assert.IsTrue(patched.Item2.DidPatchSucceed());

			string patchText = patch.PatchToText();
			Assert.IsTrue(patchText.IsStringPatch());
			Assert.IsFalse(ORIGINAL.IsStringPatch());
			Assert.AreEqual(FINAL, working.ApplyPatch(patchText.PatchFromText()).Item1);
		}

		[TestMethod()]
		public void DidPatchSucceedTest() {
			var patch = makePatch();
			var res = "Something".ApplyPatch(patch);
			Assert.IsFalse(res.Item2.DidPatchSucceed());
		}

		[TestMethod()]
		public void PatchToTextTest() {
			var patch = makePatch();
			string pText = patch.PatchToText();
			Assert.IsTrue(pText.IsStringPatch());
		}

		[TestMethod()]
		public void PatchFromTextTest() {
			var p1 = makePatch();
			string pText = p1.PatchToText();
			var p2 = pText.PatchFromText();
			Assert.AreEqual(FINAL, ORIGINAL.ApplyPatch(p2).Item1);
		}

		[TestMethod()]
		public void IsStringPatchTest() {
			var patch = makePatch();
			string pText = patch.PatchToText();
			Assert.IsTrue(pText.IsStringPatch());
		}
	}
}