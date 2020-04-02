using System;
using System.Collections.Generic;
using System.Linq;

namespace Xrd.Collections {
	/// <summary>
	/// class containing commonly-used extension methods for collections
	/// </summary>
	public static class CollectionExtensions {
		/// <summary>
		/// Is the collection/enumerable <see langword="null"/> or empty (contains no items)?
		/// </summary>
		/// <typeparam name="T">Any Type</typeparam>
		/// <param name="ts">The collection/enumarable to check</param>
		/// <returns><see langword="true"/> if <see langword="null"/> or empty; <see langword="false"/> if initialized and has at least one item.</returns>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> ts) {
			if (ts == null)
				return true;
			else if (ts.Count() < 1)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Add an item to the collection (but only if the ITEM is not null).
		/// Also has an optional unique check.
		/// </summary>
		/// <typeparam name="T">Any Type</typeparam>
		/// <param name="ts">The collection to add the item to.</param>
		/// <param name="item">The item to add, if it is not null</param>
		/// <param name="enforceUnique">Should the collection's items be unique? (If <see langword="true"/> and the item already exists in the collection, it will not be added).</param>
		/// <returns><see langword="true"/> if the item was added to the collection.</returns>
		public static bool AddIfNotNull<T>(this ICollection<T> ts, T item, bool enforceUnique = false) {
			if (ts == null)
				throw new ArgumentNullException(nameof(ts));
			if (item == null)
				return false;
			if(item is INullable nullable) {
				if (nullable.IsValueNull)
					return false;
			}
			if (enforceUnique && ts.Contains(item))
				return false;

			ts.Add(item);
			return true;
		}

		/// <summary>
		/// Add a range of items to the collection (but only if not null).
		/// Also has an optional unique check.
		/// </summary>
		/// <typeparam name="T">Any Type</typeparam>
		/// <param name="ts">The collection to add the item to.</param>
		/// <param name="items">A range of items to add (if not null)</param>
		/// <param name="enforceUnique">Should the collection's items be unique? (If <see langword="true"/> and the item already exists in the collection, it will not be added).</param>
		/// <returns>The number of items added to the collection.</returns>
		public static int AddRangeIfNotNull<T>(this ICollection<T> ts, IEnumerable<T> items, bool enforceUnique = false) {
			if (ts == null)
				throw new ArgumentNullException(nameof(ts));

			if (items.IsNullOrEmpty())
				return 0;
			int counter = 0;
			foreach (T item in items) {
				if (ts.AddIfNotNull(item, enforceUnique))
					counter++;
			}
			return counter;
		}

		/// <summary>
		/// Traverse a heirarchy and flatten all items into the root level.
		/// </summary>
		/// <typeparam name="T">The type of item contained in the heirarchy</typeparam>
		/// <param name="rootLevel">The root level of the collection.</param>
		/// <param name="nextLevel">A function that returns the next level</param>
		/// <returns>A flatten heirarchy collection where all items are shifted to the root level.</returns>
		public static IEnumerable<T> Flatten<T>(this IEnumerable<T> rootLevel, Func<T, IEnumerable<T>> nextLevel) {
			List<T> accumulation = new List<T>();
			accumulation.AddRange(rootLevel);
			_flattenLevel(accumulation, rootLevel, nextLevel);
			return accumulation;
		}

		/// <summary>
		/// Recursive helper method that traverses a hierarchy, accumulating items along the way.
		/// </summary>
		/// <typeparam name="T">any type</typeparam>
		/// <param name="accumulation">A collection in which to accumulate the items</param>
		/// <param name="currentLevel">The current level we are traversing</param>
		/// <param name="nextLevel">A function that returns the next level below a given item</param>
		private static void _flattenLevel<T>(List<T> accumulation, IEnumerable<T> currentLevel, Func<T, IEnumerable<T>> nextLevel) {
			foreach (T item in currentLevel) {
				accumulation.AddRange(currentLevel);
				_flattenLevel<T>(accumulation, nextLevel(item), nextLevel);
			}
		}

		/// <summary>
		/// JScript splice function
		/// </summary>
		/// <typeparam name="T">Generic type contained in the list.</typeparam>
		/// <param name="input">The list of objects.</param>
		/// <param name="start">The starting position.</param>
		/// <param name="count">The number of items to remove</param>
		/// <param name="objects">Array of objects to insert in place of the removed items.</param>
		/// <remarks>
		/// Adapted from https://github.com/google/diff-match-patch 
		/// </remarks>
		/// <returns>The spliced list.</returns>
		public static List<T> Splice<T>(this List<T> input, int start, int count, params T[] objects) {
			List<T> deletedRange = input.GetRange(start, count);
			input.RemoveRange(start, count);
			input.InsertRange(start, objects);

			return deletedRange;
		}

		/// <summary>
		/// Move an item up in a list.
		/// </summary>
		/// <typeparam name="T">Any Type</typeparam>
		/// <param name="list">The list of items</param>
		/// <param name="index">The index of the item to move up.</param>
		public static void MoveUp<T>(this IList<T> list, int index) {
			if (index < 1)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (list.IsNullOrEmpty())
				throw new ArgumentNullException(nameof(list));
			if (index > list.Count - 1)
				throw new ArgumentOutOfRangeException(nameof(index));
			T item = list[index];
			list.Remove(item);
			list.Insert(index - 1, item);
		}

		/// <summary>
		/// Move an item down in a list.
		/// </summary>
		/// <typeparam name="T">Any Type</typeparam>
		/// <param name="list">The list of items</param>
		/// <param name="index">the index of the item to move down.</param>
		public static void MoveDown<T>(this IList<T> list, int index) {
			if (list.IsNullOrEmpty())
				throw new ArgumentNullException(nameof(list));
			if (index > list.Count - 2 || index < 0)
				throw new ArgumentOutOfRangeException(nameof(index));
			T item = list[index];
			list.Remove(item);
			list.Insert(index + 1, item);
		}
	}
}