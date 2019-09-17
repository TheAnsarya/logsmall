using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Text {
	class SmallFontTable {
		public static byte EndOfString = 0xac;
		public static char NotFound = 'x';

		public static Dictionary<byte, char> Table = new Dictionary<byte, char> {
			[0x01] = ' ',
			[0x09] = '＊',
			[0x0c] = 'あ',
			[0x0d] = 'い',
			[0x0e] = 'う',
			[0x0f] = 'え',
			[0x10] = 'お',
			[0x11] = 'か',
			[0x12] = 'き',
			[0x13] = 'く',
			[0x14] = 'け',
			[0x15] = 'こ',
			[0x16] = 'さ',
			[0x17] = 'し',
			[0x18] = 'す',
			[0x19] = 'せ',
			[0x1a] = 'そ',
			[0x1b] = 'た',
			[0x1c] = 'ち',
			[0x1d] = 'つ',
			[0x1e] = 'て',
			[0x1f] = 'と',
			[0x20] = 'な',
			[0x21] = 'に',
			[0x22] = 'ぬ',
			[0x23] = 'ね',
			[0x24] = 'の',
			[0x25] = 'は',
			[0x26] = 'ひ',
			[0x27] = 'ふ',
			[0x28] = 'へ',
			[0x29] = 'ほ',
			[0x2a] = 'ま',
			[0x2b] = 'み',
			[0x2c] = 'む',
			[0x2d] = 'め',
			[0x2e] = 'も',
			[0x2f] = 'や',
			[0x30] = 'ゆ',
			[0x31] = 'よ',
			[0x32] = 'ら',
			[0x33] = 'り',
			[0x34] = 'る',
			[0x35] = 'れ',
			[0x36] = 'ろ',
			[0x37] = 'わ',
			[0x38] = 'を',
			[0x39] = 'ん',
			[0x3a] = 'ぁ',
			[0x3b] = 'ぃ',
			[0x3c] = 'ぅ',
			[0x3d] = 'ぇ',
			[0x3e] = 'ぉ',
			[0x3f] = 'っ',
			[0x40] = 'ゃ',
			[0x41] = 'ゅ',
			[0x42] = 'ょ',
			[0x43] = 'ア',
			[0x44] = 'イ',
			[0x45] = 'ウ',
			[0x46] = 'エ',
			[0x47] = 'オ',
			[0x48] = 'カ',
			[0x49] = 'キ',
			[0x4a] = 'ク',
			[0x4b] = 'ケ',
			[0x4c] = 'コ',
			[0x4d] = 'サ',
			[0x4e] = 'シ',
			[0x4f] = 'ス',
			[0x50] = 'セ',
			[0x51] = 'ソ',
			[0x52] = 'タ',
			[0x53] = 'チ',
			[0x54] = 'ツ',
			[0x55] = 'テ',
			[0x56] = 'ト',
			[0x57] = 'ナ',
			[0x58] = 'ニ',
			[0x59] = 'ヌ',
			[0x5a] = 'ネ',
			[0x5b] = 'ノ',
			[0x5c] = 'ハ',
			[0x5d] = 'ヒ',
			[0x5e] = 'フ',
			[0x5f] = 'ヘ',
			[0x60] = 'ホ',
			[0x61] = 'マ',
			[0x62] = 'ミ',
			[0x63] = 'ム',
			[0x64] = 'メ',
			[0x65] = 'モ',
			[0x66] = 'ヤ',
			[0x67] = 'ユ',
			[0x68] = 'ヨ',
			[0x69] = 'ラ',
			[0x6a] = 'リ',
			[0x6b] = 'ル',
			[0x6c] = 'レ',
			[0x6d] = 'ロ',
			[0x6e] = 'ワ',
			[0x6f] = 'ヲ',
			[0x70] = 'ン',
			[0x71] = 'ァ',
			[0x72] = 'ィ',
			[0x73] = 'ゥ',
			[0x74] = 'ェ',
			[0x75] = 'ォ',
			[0x76] = 'ッ',
			[0x77] = 'ャ',
			[0x78] = 'ュ',
			[0x79] = 'ョ',
			[0x7c] = '★',
			[0x7d] = 'ー',
			[0x7e] = '?',
			[0x7f] = '!',
			[0x84] = '.',
			[0x85] = '「',
			[0x86] = '…',
			[0x87] = '0',
			[0x88] = '1',
			[0x89] = '2',
			[0x8a] = '3',
			[0x8b] = '4',
			[0x8c] = '5',
			[0x8d] = '6',
			[0x8e] = '7',
			[0x8f] = '8',
			[0x90] = '9',
			[0x91] = 'A',
			[0x92] = 'B',
			[0x93] = 'C',
			[0x94] = 'D',
			[0x95] = 'E',
			[0x96] = 'F',
			[0x97] = 'G',
			[0x98] = 'H',
			[0x99] = 'I',
			[0x9a] = 'J',
			[0x9b] = 'K',
			[0x9c] = 'L',
			[0x9d] = 'M',
			[0x9e] = 'N',
			[0x9f] = 'O',
			[0xa0] = 'P',
			[0xa1] = 'Q',
			[0xa2] = 'R',
			[0xa3] = 'S',
			[0xa4] = 'T',
			[0xa5] = 'U',
			[0xa6] = 'V',
			[0xa7] = 'W',
			[0xa8] = 'X',
			[0xa9] = 'Y',
			[0xaa] = 'Z',
			[0xc9] = 'が',
			[0xca] = 'ぎ',
			[0xcb] = 'ぐ',
			[0xcc] = 'げ',
			[0xcd] = 'ご',
			[0xce] = 'ざ',
			[0xcf] = 'じ',
			[0xd0] = 'ず',
			[0xd1] = 'ぜ',
			[0xd2] = 'ぞ',
			[0xd3] = 'だ',
			[0xd4] = 'ぢ',
			[0xd5] = 'づ',
			[0xd6] = 'で',
			[0xd7] = 'ど',
			[0xd8] = 'ば',
			[0xd9] = 'び',
			[0xda] = 'ぶ',
			[0xdb] = 'べ',
			[0xdc] = 'ぼ',
			[0xdd] = 'ヴ',
			[0xde] = 'ガ',
			[0xdf] = 'ギ',
			[0xe0] = 'グ',
			[0xe1] = 'ゲ',
			[0xe2] = 'ゴ',
			[0xe3] = 'ザ',
			[0xe4] = 'ジ',
			[0xe5] = 'ズ',
			[0xe6] = 'ゼ',
			[0xe7] = 'ゾ',
			[0xe8] = 'ダ',
			[0xe9] = 'ヂ',
			[0xea] = 'ヅ',
			[0xeb] = 'デ',
			[0xec] = 'ド',
			[0xed] = 'バ',
			[0xee] = 'ビ',
			[0xef] = 'ブ',
			[0xf0] = 'ベ',
			[0xf1] = 'ボ',
			[0xf2] = 'ぱ',
			[0xf3] = 'ぴ',
			[0xf4] = 'ぷ',
			[0xf5] = 'ぺ',
			[0xf6] = 'ぽ',
			[0xf7] = 'パ',
			[0xf8] = 'ピ',
			[0xf9] = 'プ',
			[0xfa] = 'ペ',
			[0xfb] = 'ポ',
			[0xfc] = '〜',
		};

		private static Dictionary<char, byte> _reverseTable = null;
		public static Dictionary<char, byte> ReverseTable {
			get {
				if (_reverseTable == null) {
					_reverseTable = new Dictionary<char, byte>();

					foreach (var key in Table.Keys) {
						_reverseTable.Add(Table[key], key);
					}
				}

				return _reverseTable;
			}
		}

		public static string Lookup(byte source, bool returnNull = false) {
			if (Table.ContainsKey(source)) {
				return Table[source].ToString();
			}

			if (returnNull) {
				return null;
			}

			return $"{{{source.ToString("x2")}}}";
		}

		public static bool TryEncode(string text, out byte[] data) {
			if (text == null) {
				throw new ArgumentNullException($"{nameof(text)} cannot be null");
			}

			var error = false;
			var output = new List<byte>();

			foreach (var character in text) {
				if (ReverseTable.ContainsKey(character)) {
					output.Add(ReverseTable[character]);
				} else {
					error = true;
					output.Add(0);
				}
			}

			data = output.ToArray();
			return !error;
		}

		public static string Decode(byte[] data) {
			TryDecode(data, out string text);
			return text;
		}

		public static bool TryDecode(byte[] data, out string text) {
			if (data == null) {
				throw new ArgumentNullException($"{nameof(data)} cannot be null");
			}

			var error = false;
			var output = new StringBuilder();

			foreach (var value in data) {
				if (Table.ContainsKey(value)) {
					output.Append(Table[value]);
				} else {
					error = true;
					output.Append(NotFound);
				}
			}

			text = output.ToString();
			return !error;
		}
	}
}
