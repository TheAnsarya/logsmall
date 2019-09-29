using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	class Utilities {
		public static void WriteBytesToFile(byte[] data, string filename) {
			var lines = data.ToHexStrings();
			File.WriteAllLines(filename, lines);
		}
	}
}
