using DQ3Lib.Compression.Text.Huffman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3Lib.Extensions;

internal static class EnumerableExtensions {
	public static void ThrowIfNullOrEmpty<T>(this IEnumerable<T> enumerable, string name) {
		if (enumerable is null || !enumerable.Any()) {
			throw new ArgumentException($"Enumerable `{name}` cannot be null or empty.", name);
		}
	}

	public static Queue<T> ToQueue<T>(this IEnumerable<T> enumerable) {
		ArgumentNullException.ThrowIfNull(enumerable, nameof(enumerable));
		
		return new Queue<T>(enumerable);
	}

	public static PriorityQueue<T, T2> ToPriorityQueue<T, T2>(this IEnumerable<T> enumerable, Func<T, T2> predicate) {
		ArgumentNullException.ThrowIfNull(enumerable, nameof(enumerable));
		ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

		var queue = new PriorityQueue<T, T2>();

		foreach(var item in enumerable) {
			queue.Enqueue(item, predicate(item));
		}

		return queue;
	}
}
