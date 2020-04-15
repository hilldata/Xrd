using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Xrd.ChangeTracking {
	/// <summary>
	/// Extensions methods to compare two objects to see if their values are different
	/// (primarily used to detect if an Entity's property has changed.
	/// </summary>
	public static class HasChangesExtensions {
		#region Fast-checks
		/// <summary>
		/// Quickly Check == null status. If both are NOT null, return null to 
		/// indicate that closer inspection is required.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns>
		/// Either a true/false if the quick tests were obvious, or NULL
		/// if closer inspection is required.
		/// </returns>
		public static bool? IsNullableDirty(this object original, object current) {
			if (original == null && current == null)
				return false;
			else if (original == null && current != null)
				return true;
			else if (original != null && current == null)
				return true;
			else
				return null;
		}

		/// <summary>
		/// Quickly check the length of two items. If the lengths are the same, 
		/// return null to indicate that closer inspection is required.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns>
		/// Either a true/false if the quick tests were obvious, or NULL
		/// if closer inspection is required.
		/// </returns>
		public static bool? IsLengthDirty(this int original, int current) {
			if (original != current)
				return true;
			else if (original == 0) // If both lengths are zero, then they are the NOT different
				return false;
			else
				return null;
		}

		private static bool? coreEnumerableTest<T>(this IEnumerable<T> original, IEnumerable<T> current) {
			bool? tempChecks = original.IsNullableDirty(current);
			if (tempChecks.HasValue)
				return tempChecks.Value;
			tempChecks = original.Count().IsLengthDirty(current.Count());
			if (tempChecks.HasValue)
				return tempChecks.Value;
			return null;
		}
		#endregion

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this bool original, bool current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this bool? original, bool? current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this byte original, byte current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this byte? original, byte? current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this sbyte original, sbyte current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this sbyte? original, sbyte? current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this char original, char current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this char? original, char? current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this short original, short current) =>
			!original.Equals(current);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this short? original, short? current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this ushort original, ushort current) =>
			!original.Equals(current);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this ushort? original, ushort? current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this int original, int current) =>
			!original.Equals(current);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this int? original, int? current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this uint original, uint current) =>
			!original.Equals(current);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this uint? original, uint? current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this long original, long current) =>
			!original.Equals(current);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this long? original, long? current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this ulong original, ulong current) =>
			!original.Equals(current);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this ulong? original, ulong? current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this float original, float current) =>
			!original.Equals(current);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this float? original, float? current) =>
			!original.Equals(current);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this double original, double current) =>
			!original.Equals(current);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this double? original, double? current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this decimal original, decimal current) =>
			!original.Equals(current);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this decimal? original, decimal? current) =>
			!original.Equals(current);

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this DateTime original, DateTime current) =>
			!original.Ticks.Equals(current.Ticks);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this DateTime? original, DateTime? current) {
			var temp = original.IsNullableDirty(current);
			if (temp.HasValue)
				return temp.Value;
			return !original.Value.Ticks.Equals(current.Value.Ticks);
		}

		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this TimeSpan original, TimeSpan current) =>
			!original.Ticks.Equals(current.Ticks);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this TimeSpan? original, TimeSpan? current) {
			var temp = original.IsNullableDirty(current);
			if (temp.HasValue)
				return temp.Value;
			return !original.Value.Ticks.Equals(current.Value.Ticks);
		}

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this string original, string current) {
			bool? tempChecks = original.IsNullableDirty(current);
			if (tempChecks.HasValue)
				return tempChecks.Value;
			tempChecks = original.Length.IsLengthDirty(current.Length);
			if (tempChecks.HasValue)
				return tempChecks.Value;

			for(int i = 0; i < original.Length; i++) {
				if (!original[i].Equals(current[i]))
					return true;
			}
			return false;
		}

		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this Guid original, Guid current) =>
			!original.Equals(current);
		public static bool HasChanges(this IEnumerable<Guid> original, IEnumerable<Guid> current) {
			var temp = original.coreEnumerableTest(current);
			if (temp.HasValue)
				return temp.Value;
			int i = 0;
			foreach (var o in original) {
				if (o.HasChanges(current.ElementAt(i)))
					return true;
				i++;
			}
			return false;
		}
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this Guid? original, Guid? current) {
			var temp = original.IsNullableDirty(current);
			if (temp.HasValue)
				return temp.Value;
			return original.Value.HasChanges(current.Value);
		}

		/// <summary>
		/// Test method implemented to verify that stepping through each character and breaking on a
		/// mis-match is indeed faster than <see cref="String.Equals(string)"/>
		/// </summary>
		/// <param name="original">original value</param>
		/// <param name="current">current value</param>
		/// <returns>False if no changes were detected, or <see langword="true"/>if change were detected</returns>
		[Obsolete("Use the HasChanges(string,string) method instead. The custom 'IsNullableDirty', " +
			"'IsLengthDirty' then stepping through each character are significantly faster than " +
			"'String.Equals' - Also, 'String.Equals' does not allow for null on the left-hand input.")]
		public static bool IsStringDirty(this string original, string current) {
			bool? tC = original.IsNullableDirty(current);
			if (tC.HasValue)
				return tC.Value;
			return !original.Equals(current);
		}

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <typeparam name="T">Any type</typeparam>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges<T>(this T original, T current) {
			// Check for string
			if (original is string oString && current is string cString)
				return oString.HasChanges(cString);
			
			// Check for nullable structs
			if(Nullable.GetUnderlyingType(typeof(T)) != null)
				return !original.Equals(current);

			// Check null
			bool? tempChecks = original.IsNullableDirty(current);
			if (tempChecks.HasValue)
				return tempChecks.Value;
			// Check for enumerables

			// Check for concrete type implementations
			if (original is bool oBool && current is bool cBool)
				return !oBool.Equals(cBool);
			if (original is byte oByte && current is byte cByte)
				return !oByte.Equals(cByte);
			if (original is sbyte oSByte && current is sbyte cSByte)
				return !oSByte.Equals(cSByte);
			if (original is char oChar && current is char cChar)
				return !oChar.Equals(cChar);
			if (original is short oShort && current is short cShort)
				return !oShort.Equals(cShort);
			if (original is ushort oUShort && current is ushort cUShort)
				return !oUShort.Equals(cUShort);
			if (original is int oInt && current is int cInt)
				return !oInt.Equals(cInt);
			if (original is uint oUInt && current is uint cUInt)
				return !oUInt.Equals(cUInt);
			if (original is long oLong && current is long cLong)
				return !oLong.Equals(cLong);
			if (original is ulong oULong && current is ulong cULong)
				return !oULong.Equals(cULong);
			if (original is float oFloat && current is float cFloat)
				return !oFloat.Equals(cFloat);
			if (original is double oDouble && current is double cDouble)
				return !oDouble.Equals(cDouble);
			if (original is decimal oDecimal && current is decimal cDecimal)
				return !oDecimal.Equals(cDecimal);
			if (original is DateTime oDT && current is DateTime cDT)
				return !oDT.Ticks.Equals(cDT.Ticks);
			if (original is TimeSpan oTS && current is TimeSpan cTS)
				return !oTS.Ticks.Equals(cTS.Ticks);
			if (original is Guid oGuid && current is Guid cGuid)
				return oGuid.HasChanges(cGuid);

			// Concrete implementations exhausted. Try converting to ToString()
			// If it's the fully qualified type name, use Json to get the values for comparison.
			string sO = original.ToString();
			string sC;
			if (sO.EndsWith(original.GetType().FullName)) {
				sO = Newtonsoft.Json.JsonConvert.SerializeObject(original);
				sC = Newtonsoft.Json.JsonConvert.SerializeObject(current);
			} else {
				sC = current.ToString();
			}
			return sO.HasChanges(sC);
		}

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <typeparam name="T">Any type</typeparam>
		/// <param name="original">The original array of values</param>
		/// <param name="current">The current array of values</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges<T>(this T[] original, T[] current) {
			var temp = original.coreEnumerableTest(current);
			if (temp.HasValue)
				return temp.Value;

			for(int i = 0; i< original.Length; i++) {
				if (original[i].HasChanges(current[i]))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <typeparam name="T">Any type</typeparam>
		/// <param name="original">The original List of values</param>
		/// <param name="current">The current List of values</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges<T>(this List<T> original, List<T> current) {
			bool? tempChecks = original.coreEnumerableTest(current);
			if (tempChecks.HasValue)
				return tempChecks.Value;

			for(int i = 0; i < original.Count; i++) {
				if (original[i].HasChanges(current[i]))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <typeparam name="T">Any type</typeparam>
		/// <param name="original">The original IList of values</param>
		/// <param name="current">The current IList of values</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges<T>(this IList<T> original, IList<T> current) {
			bool? tempChecks = original.coreEnumerableTest(current);
			if (tempChecks.HasValue)
				return tempChecks.Value;

			for(int i = 0; i < original.Count; i++) {
				if (original[i].HasChanges(current[i]))
					return true;
			}
			return false;
		}
	}
}
