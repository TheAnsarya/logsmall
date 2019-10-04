using logsmall.DataStructures;
using logsmall.FFMQ.Text;
using logsmall.FFMQ.Text.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.FFMQ {
	class LongText {

		public static ByteArrayStream DTELookup() => FFMQ.Game.Rom.GetStream(0x03ba86);

		public static ByteArrayStream LongTextData() => FFMQ.Game.Rom.GetStream(0x03d73b);

		public static ByteArrayStream Offsets() => FFMQ.Game.Rom.GetStream(0x03d636);

		public static ByteArrayStream Under30JumpTable() => FFMQ.Game.Rom.GetStream(0x00e1cf);

		public static byte ShortTextEndOfString = 0x03;

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

		public static Dictionary<byte, byte[]> GetRawTextLookups() {
			var output = new Dictionary<byte, byte[]>();

			for (byte code = 0x30; code < 0x80; code++) {
				var data = LookupBytes(code);
				output.Add(code, data);
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
		public static string AttemptTranslate(ByteArrayStream stream, int depthLeft = 2) {
			var code = stream.Byte();

			if (code >= 0x80) {
				return BasicTable.Lookup(code);
			}

			if (code < 0x30) {
				if (code == 0x01) {
					return $"{{windowbreak}}";
				}
				if (code == 0x05) {
					return $"{{05:{stream.Byte().ToString("x2")}}}";
				}
				if (code == 0x1b) {
					return $"{{swapspeaker:{stream.Byte().ToString("x2")}}}";
				}
				if (code == 0x1d) {
					return $"{{character:{CharacterNames.GetString(stream.Byte())}}}";
				}
				if (code == 0x1e) {
					return $"{{item:{ItemNames.GetString(stream.Byte())}}}";
				}
				if (code == 0x1f) {
					return $"{{location:{LocationNames.GetString(stream.Byte())}}}";
				}
				//if (code == 0x2f) {
				//	return $"{{if:{stream.Byte().ToString("x2")} {stream.Byte().ToString("x2")} {stream.Byte().ToString("x2")}}}";
				//}


				return $"{{{code.ToString("x2")}}}";
			}

			if (depthLeft == 0) {
				return $"{{{code.ToString("x2")}}}";
			}

			//var text = string.Join("", LookupBytes(code).Select(x => AttemptTranslate(x, depthLeft - 1)));
			var text = AttemptTranslateLine(LookupBytes(code), depthLeft - 1);

			return text;
		}

		public static string AttemptTranslateLine(ByteArrayStream stream) {
			var line = new List<string>();

			while (stream.ByteAt(0) != BasicTable.EndOfString) {
				line.Add(AttemptTranslate(stream));
			}

			var text = string.Join("", line);
			stream.Byte();

			return text;
		}

		public static string AttemptTranslateLine(byte[] input, int depthLeft = 3) {
			var line = new List<string>();
			var stream = new ByteArrayStream(input);
			while (!stream.AtEnd) {
				line.Add(AttemptTranslate(stream, depthLeft));
			}

			var text = string.Join("", line);

			if (!stream.AtEnd) {
				stream.Byte();
			}

			return text;
		}

		public static Dictionary<int, string> GetLongStrings() {
			var stream = LongTextData();
			var endOfData = FFMQ.Game.Rom.AddressToPC(0x03ffff);
			var lines = new Dictionary<int, string>();

			while (stream.Address < endOfData && lines.Count < 123) {
				lines.Add(FFMQ.Game.Rom.AddressToSNES(stream.Address), AttemptTranslateLine(stream));
			}

			return lines;
		}
	}
}
