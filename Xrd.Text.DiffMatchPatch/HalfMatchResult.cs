namespace Xrd.Text.DiffMatchPatch {
	/// <remarks>
	/// Adapted from https://github.com/google/diff-match-patch 
	/// </remarks>
	internal struct HalfMatchResult {
		internal HalfMatchResult(params string[] array) {
			if (array == null || array.Length < 1)
				_array = new string[0];
			else if (array.Length != 5)
				_array = new string[0];
			else
				_array = (string[])array.Clone();
		}

		private readonly string[] _array;

		internal bool IsNull =>
			_array == null || _array.Length < 1;

		internal string Prefix1 => IsNull ? null : _array[0];
		internal string Suffix1 => IsNull ? null : _array[1];
		internal string Prefix2 => IsNull ? null : _array[2];
		internal string Suffix2 => IsNull ? null : _array[3];
		internal string CommonMiddle => IsNull ? null : _array[4];
	}
}