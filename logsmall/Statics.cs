﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	static class Statics {
		public static IEnumerable<string> Split(this string str, int chunkSize) {
			return Enumerable.Range(0, str.Length / chunkSize)
				.Select(x => str.Substring(x * chunkSize, chunkSize));
		}

		public static string FlipBytes(this string value) {
			return string.Join("", value.Split(2).Reverse());
		}
		/*
		public static int LengthWithTabs(this string value) {
			if (value)

		}
		*/

		// Allows collection initialization for Queues
		public static void Add<T>(this Queue<T> queue, T value) {
			queue.Enqueue(value);
		}

		// Allows collection initialization for Stacks
		public static void Add<T>(this Stack<T> stack, T value) {
			stack.Push(value);
		}

		// bits to bools
		public static Queue<bool> ToBooleanQueue(this byte value) {
			return new Queue<bool> {
				(value & 0b0000_0001) != 0,
				(value & 0b0000_0010) != 0,
				(value & 0b0000_0100) != 0,
				(value & 0b0000_1000) != 0,
				(value & 0b0001_0000) != 0,
				(value & 0b0010_0000) != 0,
				(value & 0b0100_0000) != 0,
				(value & 0b1000_0000) != 0,
			};
		}


		// Probably not needed
		// Adds high byte and low btye access to a ushort (little endian)
		public static byte H(this ushort value) => (byte)(value >> 8);
		public static ushort H(this ushort value, byte b) => (ushort)((value & 0x00ff) + (b << 8));
		public static byte L(this ushort value) => (byte)value;
		public static ushort L(this ushort value, byte b) => (ushort)((value & 0xff00) + b);
	}
}