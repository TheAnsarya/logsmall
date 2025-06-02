using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropToolsLib.Extensions;

static class Statics {
	public static string ToHexString(this byte[] data) {
		return string.Join(" ", data.Select(x => x.ToString("x2", CultureInfo.InvariantCulture)));
	}

	public static string[] ToHexStrings(this byte[] data) => [.. data.Chunk(16).Select(x => x.ToHexString())];

	// Reverse the bits of a byte
	// Seems overly complicated but it works and is fast
	// https://stackoverflow.com/a/3590938/3325644
	private static byte ReverseBits(this byte b) => (byte)((b * 0x80200802ul & 0x0884422110ul) * 0x0101010101ul >> 32);

	public static IEnumerable<string> Split(this string str, int chunkSize) {
		return
			Enumerable
				.Range(0, str.Length / chunkSize)
				.Select(x => str.Substring(x * chunkSize, chunkSize));
	}

	public static string FlipBytes(this string value) => string.Join("", value.Split(2).Reverse());

	// Allows collection initialization for Queues
	public static void Add<T>(this Queue<T> queue, T value) => queue.Enqueue(value);

	// Allows collection initialization for Stacks
	public static void Add<T>(this Stack<T> stack, T value) => stack.Push(value);

	// bits to bools
	public static Queue<bool> ToBooleanQueue(this byte value) {
		return [
			(value & 0b0000_0001) != 0,
			(value & 0b0000_0010) != 0,
			(value & 0b0000_0100) != 0,
			(value & 0b0000_1000) != 0,
			(value & 0b0001_0000) != 0,
			(value & 0b0010_0000) != 0,
			(value & 0b0100_0000) != 0,
			(value & 0b1000_0000) != 0,
		];
	}

	// Probably not needed
	// Given a ushort value, it gets or sets the high or low byte.

	// Get/Read the high byte of a word
	public static byte H(this ushort value) => (byte)(value >> 8);

	// Set/Replace the high byte of a word
	public static ushort H(this ushort value, byte b) => (ushort)((value & 0x00ff) + (b << 8));

	// Get/Read the low byte of a word
	public static byte L(this ushort value) => (byte)value;

	// Set/Replace the low byte of a word
	public static ushort L(this ushort value, byte b) => (ushort)((value & 0xff00) + b);
}
