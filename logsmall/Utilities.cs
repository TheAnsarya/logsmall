using System.IO;

namespace logsmall {
	class Utilities {
		public static void WriteBytesToFile(byte[] data, string filename) {
			var lines = data.ToHexStrings();
			File.WriteAllLines(filename, lines);
		}
	}
}
