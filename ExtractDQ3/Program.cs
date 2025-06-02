using DQ3Lib.Common.Rom;

namespace ExtractDQ3;

internal class Program {
	static void Main(string[] args) {
		var romFilePath = @"C:\~working\roms-unaltered\SNES\Dragon Quest III - Soshite Densetsu he... (J).smc";
		var outputFolder = $@"c:\~working\dq3r-extract\{DateTime.Now:yyyy-MM-dd HH-mm-ss}\";

		Directory.CreateDirectory(outputFolder);

		var romFile = new HiRom(romFilePath);
	}
}

