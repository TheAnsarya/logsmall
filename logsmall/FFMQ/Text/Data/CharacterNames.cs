using logsmall.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.FFMQ.Text.Data {
	class CharacterNames {
		public const int RecordSize = 0x50;
		public const int EntrySize = 0x10;
		public const byte BufferByte = 0x03;

		// TODO: We can do this better once the whole character datastructure is done
		public static ByteArrayStream GetDataStream(int index) {
			if ((index < 0) || (index >= 9)) {
				throw new ArgumentOutOfRangeException($"{nameof(index)} must be between 0 and 8, actual value: {index}");
			}

			var offset = index * RecordSize;

			return FFMQ.Game.Rom.GetStream(0x0cd0b0 + offset);
		}
		public static string GetString(int index) {
			var stream = GetDataStream(index);
			var data = stream.GetBytes(EntrySize);
			var name = string.Join("", data.Where(x => x != BufferByte).Select(x => BasicTable.Lookup(x)));
			return name;
		}
	}
}
