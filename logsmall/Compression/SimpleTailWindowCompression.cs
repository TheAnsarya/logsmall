using logsmall.DataStructures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Compression {
	// Not sure what else to call this
	// Used by FFMQ
	/*
	 * source layout is:
	 *		word offset to start of data[] array
	 *		byte[] commands - read as stream
	 *			1 or 2 bytes, start with byte #1
	 *			if 0, end decompression
	 *			if low nibble not 0, copy low nibble of bytes from data[] to output[]
	 *			if high nibble not 0, copy high nibble + 2 bytes from output[] to output[]
	 *				source address on output is current output address minus byte #2 minus 1
	 *				so source address can't be larger than the last byte written
	 *		byte[] data - read as stream
	 */
	class SimpleTailWindowCompression {

		// TODO: Need to get rid of outputSize as this compression has an ending command so no need
		public static byte[] Decompress(byte[] source, int outputSize) {
			return Decompress(new ByteArrayStream(source), outputSize);
		}

		public static byte[] Decompress(ByteArrayStream source, int outputSize) {
			var (comp, decomp) = DecompressFull(source, outputSize);
			return decomp;
		}

		public static (byte[] comp, byte[] decomp) DecompressFull(ByteArrayStream source, int outputSize) {
			var startAddress = source.Address;
			var dataSourceOffset = source.Word();
			var dataSource = source.Branch(source.Address + dataSourceOffset);

			var output = new ByteArrayStream(outputSize);

			byte command;

			while ((command = source.Byte()) != 0x00) {
				if ((command & 0x0f) != 0) {
					var length = command & 0x0f;
					dataSource.CopyTo(output, length);
				}

				if ((command & 0xf0) != 0) {
					var length = ((command & 0xf0) >> 4) + 2;

					var address = output.Address - source.Byte() - 1;
					if (address < 0) {
						throw new IndexOutOfRangeException($"{nameof(address)} cannot be less than 0");
					}

					output.Branch(address).CopyTo(output, length);
				}
			}

			var comp = source.GetBytes(dataSource.Address - startAddress, startAddress);
			var decomp = output.GetBytes(output.Address, 0);
			return (comp, decomp);
		}

		public static byte[] Compress(byte[] target) {
			return Compress(new ByteArrayStream(target));
		}

		public static byte[] Compress(ByteArrayStream target) {
			var commands = new List<byte>();
			var data = new List<byte>();
			var copyData = 0;

			while (!target.AtEnd) {
				var term = target.GetBytes(0x11);
				var copyOutput = 0;
				var copyOffset = -1;

				while (term.Length >= 3) {
					var (found, address) = target.FindLastInWindow(term, target.Address - 256, target.Address + term.Length - 1);

					if (found) {
						copyOutput = term.Length - 2;
						copyOffset = target.Address - address - 1;
						break;
					}

					term = target.GetBytes(term.Length - 1);
				}

				if (copyOutput == 0) {
					if (copyData == 0xf) {
						commands.Add((byte)copyData);
						copyData = 1;
					} else {
						copyData++;
					}

					data.Add(target.Byte());
				} else {
					commands.Add((byte)((copyOutput << 4) + copyData));
					commands.Add((byte)copyOffset);
					copyData = 0;
					target.Address += term.Length;
				}
			}

			// Add last copy data command
			if (copyData != 0) {
				commands.Add((byte)copyData);
			}

			// Add terminating command
			commands.Add(0);

			var output = new ByteArrayStream(commands.Count + data.Count + 2);

			var dataOffset = commands.Count;
			if (dataOffset > 0xffff) {
				throw new Exception($"{nameof(dataOffset)} cannot be larger than 0xffff. {nameof(commands)} is too large");
			}

			output.Word((ushort)dataOffset);
			output.Write(commands);
			output.Write(data);

			return output.Buffer;
		}

		// TODO: I think it's possible to increase the compression by someitmes writing additional values to data
		// TODO: like you can copy 3 bytes, then write 1, then copy 10 (5 byte command, 1 data, 14 byte out)
		// TODO: or write 1, then copy 13 (2 byte command, 1 byte data, 14 out)
		// TODO: certain situations only, so not bothering yet
		//public static byte[] CompressMax(byte[] target) {
		//	return CompressMax(new ByteArrayStream(target));
		//}

		//public static byte[] CompressMax(ByteArrayStream target) {
		//}

		// TODO: Move to unit test project
		public static void TestLayout() {
			var compressed = TestData.StringToBytes(TestData.FFMQForestTileMap_Input);
			var uncompressed = TestData.StringToBytes(TestData.FFMQForestTileMap_Output);

			var folder = @"c:\working\test\";
			Directory.CreateDirectory(folder);
			WriteBytesToFile(compressed, Path.Combine(folder, "compressed.txt"));
			WriteBytesToFile(uncompressed, Path.Combine(folder, "uncompressed.txt"));

			var testDecomp = SimpleTailWindowCompression.Decompress(compressed, 0x1000);
			var decompPassed = testDecomp.SequenceEqual(uncompressed);

			WriteBytesToFile(testDecomp, Path.Combine(folder, "testDecomp.txt"));
			Console.WriteLine($"  Decompression: {(decompPassed ? "Passed" : "Failed")}");
			Console.WriteLine($"           size: 0x{testDecomp.Length.ToString("x4", CultureInfo.InvariantCulture)}");
			Console.WriteLine($"       realsize: 0x{uncompressed.Length.ToString("x4", CultureInfo.InvariantCulture)}");

			Console.WriteLine();


			var testComp = SimpleTailWindowCompression.Compress(uncompressed);
			var compPassed = testComp.SequenceEqual(compressed);

			WriteBytesToFile(testComp, Path.Combine(folder, "testComp.txt"));
			Console.WriteLine($"    Compression: {(compPassed ? "Passed" : "Failed")}");
			Console.WriteLine($"           size: 0x{testComp.Length.ToString("x4", CultureInfo.InvariantCulture)}");
			Console.WriteLine($"       realsize: 0x{compressed.Length.ToString("x4", CultureInfo.InvariantCulture)}");

			Console.WriteLine();


			var testReDecomp = SimpleTailWindowCompression.Decompress(testComp, 0x1000);
			var reDecompPassed = testReDecomp.SequenceEqual(uncompressed);

			WriteBytesToFile(testReDecomp, Path.Combine(folder, "testReDecomp.txt"));
			Console.WriteLine($"ReDecompression: {(reDecompPassed ? "Passed" : "Failed")}");
			Console.WriteLine($"           size: 0x{testReDecomp.Length.ToString("x4", CultureInfo.InvariantCulture)}");
			Console.WriteLine($"       realsize: 0x{uncompressed.Length.ToString("x4", CultureInfo.InvariantCulture)}");

			Console.WriteLine();

			Console.ReadKey();
		}

		public static void TestDumpData() {
			// pointers: $0b8735 - $0b87b8
			var pointersAddress = 0x0b8735;
			var rom = FFMQ.Game.Rom;
			var pointerStream = rom.GetStream(pointersAddress);
			int pointer;
			int map = 0;

			while (((pointer = pointerStream.Long()) >= 0x070000) && (pointer <= 0x08ffff)) {
				var (comp, decomp) = DecompressFull(rom.GetStream(pointer), 0x2000);
				var recomp = Compress(decomp);
				var redecomp = Decompress(recomp, 0x2000);
				WriteBytesToFile(comp, $"c:\\working\\test\\map - {map.ToString("D2", CultureInfo.InvariantCulture)} - ${pointer.ToString("x6", CultureInfo.InvariantCulture)} - ${(pointer + comp.Length - 1).ToString("x6", CultureInfo.InvariantCulture)} -1- original.txt");
				WriteBytesToFile(decomp, $"c:\\working\\test\\map - {map.ToString("D2", CultureInfo.InvariantCulture)} - ${pointer.ToString("x6", CultureInfo.InvariantCulture)} - ${(pointer + comp.Length - 1).ToString("x6", CultureInfo.InvariantCulture)} -2- decompressed.txt");
				WriteBytesToFile(recomp, $"c:\\working\\test\\map - {map.ToString("D2", CultureInfo.InvariantCulture)} - ${pointer.ToString("x6", CultureInfo.InvariantCulture)} - ${(pointer + comp.Length - 1).ToString("x6", CultureInfo.InvariantCulture)} -3- compressed.txt");
				WriteBytesToFile(redecomp, $"c:\\working\\test\\map - {map.ToString("D2", CultureInfo.InvariantCulture)} - ${pointer.ToString("x6", CultureInfo.InvariantCulture)} - ${(pointer + comp.Length - 1).ToString("x6", CultureInfo.InvariantCulture)} -4- redecompressed.txt");

				Console.WriteLine($"map - {map.ToString("D2", CultureInfo.InvariantCulture)} - ${pointer.ToString("x6", CultureInfo.InvariantCulture)} - ${(pointer + comp.Length - 1).ToString("x6", CultureInfo.InvariantCulture)}");
				Console.WriteLine($"comp: {(comp.SequenceEqual(recomp) ? "Passed" : "Failed")}");
				Console.WriteLine($"	size 1: 0x{comp.Length.ToString("x4", CultureInfo.InvariantCulture)}");
				Console.WriteLine($"	size 2: 0x{recomp.Length.ToString("x4", CultureInfo.InvariantCulture)}");
				Console.WriteLine($"decomp: {(decomp.SequenceEqual(redecomp) ? "Passed" : "Failed")}");
				Console.WriteLine($"	size 1: 0x{decomp.Length.ToString("x4", CultureInfo.InvariantCulture)}");
				Console.WriteLine($"	size 2: 0x{redecomp.Length.ToString("x4", CultureInfo.InvariantCulture)}");
				Console.WriteLine();

				map++;
			}

				Console.WriteLine();
				Console.WriteLine($"pointers at: ${pointersAddress.ToString("x6", CultureInfo.InvariantCulture)} - ${rom.AddressToSNES(pointerStream.Address - 4):x6}");
				Console.WriteLine();
			Console.ReadKey();
		}


		public static void WriteBytesToFile(byte[] data, string filename) {
			var lines = data.Chunk(16).Select(x => string.Join(" ", x.Select(y => y.ToString("x2", CultureInfo.InvariantCulture))));
			File.WriteAllLines(filename, lines);
		}
	}
}
