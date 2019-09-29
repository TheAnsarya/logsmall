using logsmall.DataStructures;
using logsmall.FFMQ.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.FFMQ {
	class LongText {

		public static ByteArrayStream DTELookup() => FFMQ.Game.Rom.GetStream(0x03ba86);

		public static ByteArrayStream LongTextData() => FFMQ.Game.Rom.GetStream(0x03d73b);

		// Codes $30 to $7f represent short runs of text
		public static byte[] LookupBytes(byte code) {
			if (!((code < 0x80) && (code >= 30))) {
				throw new ArgumentOutOfRangeException($"{nameof(code)} must be between 0x30 and 0x7f, actual value: {code}");
			}

			var lookups = DTELookup();
			var index = code - 0x30;

			while (index > 0) {
				lookups.Skip(lookups.Byte());
				index--;
			}

			var length = lookups.Byte();
			var bytes = lookups.GetBytes(length);

			return bytes;
		}

		public static Dictionary<byte, string> GetTextLookups() {
			var output = new Dictionary<byte, string>();

			for (byte code = 0x30; code < 0x80; code++) {
				var text = AttemptTranslate(code);
				output.Add(code, text);
			}

			return output;
		}

		public static string AttemptTranslate(byte[] codes) => string.Join("", codes.Select(x => AttemptTranslate(x)));

		public static string AttemptTranslate(byte code, int depthLeft = 2) {
			if (code < 0x30) {
				return $"{{{code.ToString("x2")}}}";
			} else if (code >= 0x80) {
				return BasicTable.Lookup(code);
			} else if (depthLeft == 0) {
				return $"{{{code.ToString("x2")}}}";
			}
			var text = string.Join("", LookupBytes(code).Select(x => AttemptTranslate(x, depthLeft - 1)));

			return text;
		}
		public static string AttemptTranslateLine(ByteArrayStream stream) {
			var (data, _, _) = stream.ReadUntil(BasicTable.EndOfString);

			var text = string.Join("", data.Select(x => AttemptTranslate(x)));

			return text;
		}

		public static Dictionary<int, string> GetLongStrings() {
			var stream = LongTextData();
			var endOfData = FFMQ.Game.Rom.AddressToPC(0x03ffff);
			var lines = new Dictionary<int, string>();

			while (stream.Address < endOfData) {
				lines.Add(stream.Address, AttemptTranslateLine(stream));
			}

			return lines;
		}
	}
}
