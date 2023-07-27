using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.FFMQ.Text {
	class BasicTable {

		public const byte EndOfString = 0x00;
		public const char NotFound = '#';

		public static Dictionary<byte, char> Table = new Dictionary<byte, char> {
			//[0x03] = '*',
			//[0x06] = '_',
			[0x90] = '0',
			[0x91] = '1',
			[0x92] = '2',
			[0x93] = '3',
			[0x94] = '4',
			[0x95] = '5',
			[0x96] = '6',
			[0x97] = '7',
			[0x98] = '8',
			[0x99] = '9',
			[0x9a] = 'A',
			[0x9b] = 'B',
			[0x9c] = 'C',
			[0x9d] = 'D',
			[0x9e] = 'E',
			[0x9f] = 'F',
			[0xa0] = 'G',
			[0xa1] = 'H',
			[0xa2] = 'I',
			[0xa3] = 'J',
			[0xa4] = 'K',
			[0xa5] = 'L',
			[0xa6] = 'M',
			[0xa7] = 'N',
			[0xa8] = 'O',
			[0xa9] = 'P',
			[0xaa] = 'Q',
			[0xab] = 'R',
			[0xac] = 'S',
			[0xad] = 'T',
			[0xae] = 'U',
			[0xaf] = 'V',
			[0xb0] = 'W',
			[0xb1] = 'X',
			[0xb2] = 'Y',
			[0xb3] = 'Z',
			[0xb4] = 'a',
			[0xb5] = 'b',
			[0xb6] = 'c',
			[0xb7] = 'd',
			[0xb8] = 'e',
			[0xb9] = 'f',
			[0xba] = 'g',
			[0xbb] = 'h',
			[0xbc] = 'i',
			[0xbd] = 'j',
			[0xbe] = 'k',
			[0xbf] = 'l',
			[0xc0] = 'm',
			[0xc1] = 'n',
			[0xc2] = 'o',
			[0xc3] = 'p',
			[0xc4] = 'q',
			[0xc5] = 'r',
			[0xc6] = 's',
			[0xc7] = 't',
			[0xc8] = 'u',
			[0xc9] = 'v',
			[0xca] = 'w',
			[0xcb] = 'x',
			[0xcc] = 'y',
			[0xcd] = 'z',
			[0xce] = '!',
			[0xcf] = '?',
			[0xd0] = ',',
			[0xd1] = '\'',
			[0xd2] = '.',
			[0xd3] = '“',
			[0xd4] = '”',
			//[0xd5] = ".”",
			[0xd6] = ';',
			[0xd7] = ':',
			[0xd8] = '…',
			[0xd9] = '/',
			[0xda] = '-',
			[0xdb] = '&',
			[0xdc] = '⯈',
			[0xdd] = '%',

			[0xff] = ' ',
		};

		private static Dictionary<char, byte> _reverseTable;
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

			//return NotFound.ToString();
			return $"{{{source:x2}}}";
		}
	}
}
