using System;
using System.Collections.Generic;
using System.Text;

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
			!original.Equals(current);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this DateTime? original, DateTime? current) =>
			!original.Equals(current);

		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this TimeSpan original, TimeSpan current) =>
			!original.Equals(current);
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this TimeSpan? original, TimeSpan? current) =>
			!original.Equals(current);

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
		/// <summary>
		/// Determine whether or not a value has changed over time.
		/// </summary>
		/// <param name="original">The original value</param>
		/// <param name="current">The current value</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges(this Guid? original, Guid? current) =>
			!original.Equals(current);

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
			Type t = typeof(T);
			if (t.Name.StartsWith("Nullable"))
				return !original.Equals(current);

			bool? tempChecks = original.IsNullableDirty(current);
			if (tempChecks.HasValue)
				return tempChecks.Value;

			// Try ToString(). If it's the fully qualified type name, use Json
			// to get the values.
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
			bool? tempChecks = original.IsNullableDirty(current);
			if (tempChecks.HasValue)
				return tempChecks.Value;
			tempChecks = original.Length.IsLengthDirty(current.Length);
			if (tempChecks.HasValue)
				return tempChecks.Value;
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
		/// <param name="original">The original IList of values</param>
		/// <param name="current">The current IList of values</param>
		/// <returns><see langword="false"/>, if no changes were detected, or true
		/// if changes were detected.</returns>
		public static bool HasChanges<T>(this IList<T> original, IList<T> current) {
			bool? tempChecks = original.IsNullableDirty(current);
			if (tempChecks.HasValue)
				return tempChecks.Value;
			tempChecks = original.Count.IsLengthDirty(current.Count);
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
