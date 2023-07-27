using logsmall.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.FFMQ.Text.Data {
	class ItemNames {
		public static ByteArrayStream GetDataStream() => FFMQ.Game.Rom.GetStream(0x0cc120);

		public const byte BufferByte = 0x03;
		public const int EntrySize = 0x0c;

		public static string GetString(int index) {
			var stream = GetDataStream();
			stream.Skip(EntrySize * index);
			var data = stream.GetBytes(EntrySize);
			var name = string.Join("", data.Where(x => x != BufferByte).Select(x => BasicTable.Lookup(x)));
			return name;
		}
	}
}
