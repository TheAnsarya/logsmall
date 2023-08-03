using logsmall.Common;
using logsmall.Compression;
using logsmall.DataStructures;
using logsmall.DQ3.Text;
using logsmall.DQ3.Text.Data;
using logsmall.FFMQ;
using logsmall.SourceCode;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace logsmall {
	class Program {
		static void Main(string[] args) {
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\ffmq - text if statement 2.txt");


			//LookingForxce0080();
			//CompareAndFind();
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\2020-01-23 load first enemy tile.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\2020-01-31 slime wiggle 1.txt");
			//processSimple(@"C:\Users\Andy\Documents\Mesen-S\Debugger\attack raven diff.txt");

			//FZeroFindJetColorsJSR();

			//FFMQGetLongTextFromOffsets();
			//FFMQGetLongTextOffsets();
			//FFMQGetLongTextUnder30JumpTable();
			//FFMQGetLongTextLines();
			//FFMQGetLongTextLookups();
			//OutputHexChunkFFMQ();
			//MapData.Go();
			//SimpleTailWindowCompression.TestLayout();
			//SimpleTailWindowCompression.TestDumpData();

			//LookForABunchOfStrings();
			//DecodeStringBlocks();

			//GetC90572Parameters();
			//GetC90566Parameters();

			//var bytecode = SNES.OpToHex("c01576", "jml", "[$1d9a]");
			//var t = 0;
			//Getc90717Calls();
			//GetPossibleGoldSpots();
			//LookForAString();
			//DecodeStringBlock();

			//var filename = @"c:\working\dq3\dq3-a.log"
			//var filename = @"C:\Users\Andy\OneDrive\Desktop\DQ3 Stuff\dq3 - decompressing ow bg1.log";
			//processBSNES(filename);

			Console.WriteLine("Hello, World!");

			//FindMinusMinus();
			//FindOneBitDifferent();
			//FindOneBitDifferent2();
			//LevelsAsHex();
			FindByteDifferenceDQ4HP();

			Console.ReadKey();
		}

		private static string BuildFolder(string filename) {
			var folder = Path.Combine(Path.GetDirectoryName(filename), $"{Path.GetFileNameWithoutExtension(filename)} {DateTime.Now:yyyy-MM-dd HH-mm-ss}");
			Directory.CreateDirectory(folder);
			return folder;
		}

		private static void TestRange64() {
			//int maxhp = 63;
			int maxhp = 3;
			var debughp = 9;
			var debugall = true;

			int good = 0;
			int bad = 0;

			for (int hp = 0; hp <= maxhp; hp++) {
				string output = "";
				var goal = (int)Math.Round((double)hp / (double)maxhp * 63);
				var goalBig = $"{hp / (double)maxhp * 63:0.##}";
				var goalString = Convert.ToString(goal, 2).PadLeft(6, '0');
				var path = new List<ConsoleColor>();
				string bVals = "";

				var max = maxhp;
				var current = hp;

				for (int j = 0; j < 6; j++) {
					var b = max & 0x001;
					bVals += b;
					var half = max >> 1;

					if (hp == debughp || debugall) {
						Console.WriteLine("j: {0}  b: {1}  half: {2}  max: {3}  current {4}  truehalf {5}  hphalf {6}",
							j, b, half, max, current, maxhp / Math.Pow(2, j + 1), hp / Math.Pow(2, j + 1));
					}

					if (current > half) {
						output += "1";
						current -= half + b;
						max = half;
						path.Add(ConsoleColor.DarkBlue);
					} else if (current < half) {
						output += "0";
						max = half;
						path.Add(ConsoleColor.Black);
					} else {
						max -= half;
						if (b > 0) {
							output += "0";
							path.Add(ConsoleColor.DarkGreen);
						} else {
							current -= half;
							output += "1";
							path.Add(ConsoleColor.DarkGray);
						}
					}
				}

				if (hp == debughp || debugall) {
					Console.WriteLine("max: {0}  current {1}", max, current);
				}

				/* Works for maxhp = 63
				for (int j = 0; j < 6; j++) {
					var b = max & 0x001;
					bvals += b;
					var half = max >> 1;

					if (hp == debughp) {
						Console.WriteLine("j: {0}  b: {1}  half: {2}  max: {3}  current {4}  truehalf {5}", j, b, half, max, current, maxhp /Math.Pow(2, j+1));
					}

					if (current > half) {
						output += "1";
						current -= half + b;
						max = half;
						path.Add(ConsoleColor.DarkBlue);
					} else if (current < half) {
						output += "0";
						max = half;
						path.Add(ConsoleColor.Black);
					} else {
						max -= half;
						if (b > 0) {
							output += "0";
							path.Add(ConsoleColor.DarkGreen);
						} else {
							current -= half;
							output += "1";
							path.Add(ConsoleColor.DarkGray);
						}
					}
				}
				if (hp == debughp) {
					Console.WriteLine("max: {0}  current {1}", max, current);
				}
				}*/

				if (goalString == output) {
					good++;
				} else {
					bad++;
				}

				Console.Write(
					"{maxhp}".PadLeft(2) + " " + Convert.ToString(maxhp, 2).PadLeft(6, '0') + " " +
					"{hp}".PadLeft(2) + " " + Convert.ToString(hp, 2).PadLeft(6, '0') + " " +
					"{goal}".PadLeft(2) + " ");
				Console.Write(" ");
				for (int k = 0; k < 6; k++) {
					Console.BackgroundColor = path[k];
					Console.ForegroundColor = goalString[k] == output[k] ? ConsoleColor.White : ConsoleColor.Red;
					Console.Write(goalString[k]);
				}

				Console.BackgroundColor = ConsoleColor.Black;
				Console.Write("  ");
				for (int k = 0; k < 6; k++) {
					Console.BackgroundColor = path[k];
					Console.ForegroundColor = goalString[k] == output[k] ? ConsoleColor.White : ConsoleColor.Red;
					Console.Write(output[k]);
				}

				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = (goalString == output) ? ConsoleColor.White : ConsoleColor.Red;
				Console.Write("  " + Convert.ToInt32(output, 2) + "  ");
				for (int k = 0; k < 6; k++) {
					Console.BackgroundColor = path[k];
					Console.ForegroundColor = goalString[k] == output[k] ? ConsoleColor.White : ConsoleColor.Red;
					Console.Write(bVals[k]);
				}

				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = (goalString == output) ? ConsoleColor.White : ConsoleColor.Red;
				Console.WriteLine(" " + goalBig);
				Console.ForegroundColor = ConsoleColor.White;
			}

			Console.WriteLine("good: " + good);
			Console.WriteLine("bad: " + bad);
			Console.ReadKey();
		}

		// Merges A into B into new file
		// So use the already annotated one for B
		static void MergeTrace() {
			//string aname = @"c:\working\ffmq-a.log";
			//string bname = @"c:\working\ffmq-b.log";
			//string outname = @"c:\working\ffmq-merge.log";
			string aname = @"c:\working\dq3\dq3-a.log";
			string bname = @"c:\working\dq3\dq3-b.log";
			string outname = @"c:\working\dq3\dq3-merge.log";

			Regex isCode = new Regex("^[0-9a-f]{6} ");

			StringBuilder bufferb = new StringBuilder();

			using (var outfile = new StreamWriter(outname)) {
				using var readera = new StreamReader(aname);
				using var readerb = new StreamReader(bname);
				string linea = readera.ReadLine();
				string lineb = readerb.ReadLine();
				int a = 1;
				int b = 1;

				while ((linea != null) || (lineb != null)) {
					Console.WriteLine($"a: {a,8} b: {b,8}");

					// if only B left, flush buffer and write all remaining
					if (linea == null) {
						if (bufferb.Length > 0) {
							outfile.Write(bufferb.ToString());
							bufferb.Clear();
						}

						outfile.WriteLine(lineb);

						while ((lineb = readerb.ReadLine()) != null) {
							b++;
							outfile.WriteLine(lineb);
						}

						break;
					}

					// if only A left, write all remaining
					if (lineb == null) {
						outfile.WriteLine(linea);

						while ((linea = readera.ReadLine()) != null) {
							a++;
							outfile.WriteLine(linea);
						}

						break;
					}

					// Grab comment buffer
					while ((lineb != null) && (!isCode.IsMatch(lineb.TrimStart()))) {
						bufferb.AppendLine(lineb);
						lineb = readerb.ReadLine();
						b++;
					}

					string shorta = linea.TrimStart();
					string shortb = lineb.TrimStart();

					string commandA = shorta.Replace("\t", "    ")[..Math.Min(21, shorta.Length)].Trim();
					string commandB = shortb.Replace("\t", "    ")[..Math.Min(21, shortb.Length)].Trim();
					int iKeyA = int.Parse(shorta[..6], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
					int iKeyB = int.Parse(shortb[..6], NumberStyles.HexNumber, CultureInfo.InvariantCulture);

					if (iKeyA == iKeyB) {
						if (string.Compare(commandA, commandB, CultureInfo.InvariantCulture, CompareOptions.None) == 0) {
							if (bufferb.Length > 0) {
								outfile.Write(bufferb.ToString());
								bufferb.Clear();
							}

							outfile.WriteLine(lineb);

							linea = readera.ReadLine();
							a++;
							lineb = readerb.ReadLine();
							b++;
						} else if (string.Compare(commandA, commandB, CultureInfo.InvariantCulture, CompareOptions.None) < 0) {
							outfile.WriteLine(linea);
							linea = readera.ReadLine();
							a++;
						} else {
							if (bufferb.Length > 0) {
								outfile.Write(bufferb.ToString());
								bufferb.Clear();
							}

							outfile.WriteLine(lineb);
							lineb = readerb.ReadLine();
							b++;
						}
					} else if (iKeyA < iKeyB) {
						outfile.WriteLine(linea);
						linea = readera.ReadLine();
						a++;
					} else {
						if (bufferb.Length > 0) {
							outfile.Write(bufferb.ToString());
							bufferb.Clear();
						}

						outfile.WriteLine(lineb);
						lineb = readerb.ReadLine();
						b++;
					}
				}

				// Flush comment buffer
				if (bufferb.Length > 0) {
					outfile.Write(bufferb.ToString());
				}

				Console.WriteLine("a: " + a.ToString(CultureInfo.InvariantCulture).PadLeft(8) + " b: " + b.ToString(CultureInfo.InvariantCulture).PadLeft(8));
				Console.WriteLine("------------ DONE ------------");
			}

			Console.ReadKey();
		}

		static void TrimLog() {
			//string infilename = @"c:\working\ffmq.log";
			//string infilename = @"c:\working\ffmq.log";
			//string outfilename = @"c:\working\ffmq-small.log";
			string infilename = @"c:\working\dq3\big.log";
			string outfilename = @"c:\working\dq3\small.log";
			string line;

			using var infile = new StreamReader(infilename);
			using var outfile = new StreamWriter(outfilename);
			while ((line = infile.ReadLine()) != null) {
				//outfile.WriteLine(line.Substring(0, 30));
				outfile.WriteLine(line[..21].Trim());
				//outfile.WriteLine(line.Substring(2));
			}
		}

		// a bunch of "sep #$20" were accidently replaced to "%setAto16bit()"
		// compares and fixes that
		static void FixA16() {
			string aname = @"c:\working\ffmq-a16.log";
			string bname = @"c:\working\ffmq-b16.log";
			string outname = @"c:\working\ffmq-merge16.log";

			Regex isCode = new Regex("^[0-9a-f]{6} ");


			using (var outfile = new StreamWriter(outname)) {
				using var readera = new StreamReader(aname);
				using var readerb = new StreamReader(bname);
				string linea;
				string lineb;
				int b = 0;
				int contains16 = 0;
				int containssep = 0;
				int iscode = 0;

				while ((lineb = readerb.ReadLine()) != null) {
					b++;
					if (lineb.Contains("%setAto16bit()")) {
						contains16++;
						if (isCode.IsMatch(lineb)) {
							iscode++;
							string code = lineb[..6];
							do {
								linea = readera.ReadLine();
							} while (linea != null && (linea.Length < 6 || linea[..6] != code));

							if (linea == null) {
								Console.WriteLine("Couldn't find in A!!!   " + b + "   " + lineb);
								break;
							}

							if (linea.Contains("sep #$20")) {
								containssep++;
								lineb = lineb.Replace("%setAto16bit()", "%setAto8bit()");
							} else {
								Console.WriteLine("Not SEP: " + linea);
							}
						} else {
							Console.WriteLine("Not a Code!!! " + b + "   " + lineb);
						}
					}

					outfile.WriteLine(lineb);
				}

				Console.WriteLine("b: " + b);
				Console.WriteLine("contains16: " + contains16);
				Console.WriteLine("iscode: " + iscode);
				Console.WriteLine("containssep: " + containssep);
				Console.WriteLine("------------ DONE ------------");
			}

			Console.ReadKey();
		}


		static void SortAddresses() {
			string inname = @"c:\working\dq3\addresses-in.log";
			string outname = @"c:\working\dq3\addresses-out.log";

			Regex getAddress = new Regex(@"^.{23}(...).*\[([0-9A-F]{6})\].*$");

			var lines = File.ReadAllLines(inname);

			var addresses =
				lines
					.Where(x => getAddress.IsMatch(x))
					.Select(x => getAddress.Match(x))
					.Select(x => $"{x.Groups[2].Value} {x.Groups[1].Value}")
					.GroupBy(x => x)
					.Select(x => $"{x.Key} {x.Count()}")
					.OrderBy(x => x)
					.ToList();

			File.WriteAllLines(outname, addresses);
		}


		static void ProcessBSNES(string filename) {
			var lines = File.ReadAllLines(filename);
			var folder = Path.Combine(Path.GetDirectoryName(filename), $"{Path.GetFileNameWithoutExtension(filename)} {DateTime.Now:yyyy-MM-dd hh-mm-ss}");
			Directory.CreateDirectory(folder);

			// Raw input
			File.WriteAllLines(Path.Combine(folder, "raw.txt"), lines);

			// Simple, just code & address ordered
			var code =
				lines
					.Where(x => !string.IsNullOrEmpty(x))
					.Select(x => x[..20].Trim())
					.Distinct()
					.OrderBy(x => x)
					.ToList();
			File.WriteAllLines(Path.Combine(folder, "code.txt"), code);

			// Code with labels and line breaks
			var labeled = SourceProcessing.LabelCode(code);
			File.WriteAllLines(Path.Combine(folder, "code-labeled.txt"), labeled);

			// Address/instruction usage counts
			Regex getAddress = new Regex(@"^.{7}(...).{11}\[([0-9a-f]{6})\].*$");
			var addresses =
				lines
					.Where(x => getAddress.IsMatch(x))
					.Select(x => getAddress.Match(x))
					.Select(x => $"{x.Groups[2].Value} {x.Groups[1].Value}")
					.GroupBy(x => x)
					.Select(x => $"{x.Key} {x.Count()}")
					.OrderBy(x => x)
					.ToList();
			File.WriteAllLines(Path.Combine(folder, "address-usage.txt"), addresses);

			// Jump targets
			Regex targetsRegex = new Regex(@"^([a-f0-9]{6}) (jsr|jrl|jmp|jml)(.{11})\[([0-9a-f]{6})\].*$", RegexOptions.Compiled);
			Regex containsIndirect = new Regex(@",|\[|\(", RegexOptions.Compiled);
			var targets =
				lines
					.Where(x => targetsRegex.IsMatch(x))
					.Select(x => targetsRegex.Match(x))
					.Where(x => containsIndirect.IsMatch(x.Groups[3].Value))
					.Select(x => $"{x.Groups[1].Value} {x.Groups[2].Value}{x.Groups[3].Value}[{x.Groups[4].Value}]")
					.GroupBy(x => x)
					.Select(x => $"{x.Key} - {x.Count()}")
					.OrderBy(x => x)
					.ToList();
			File.WriteAllLines(Path.Combine(folder, "jump-targets.txt"), targets);
		}

		static void ProcessMesen(string filename) {
			var rawlines = File.ReadAllLines(filename);
			//var folder = Path.Combine(Path.GetDirectoryName(filename), $"{Path.GetFileNameWithoutExtension(filename)} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}");
			var folder = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
			Directory.CreateDirectory(folder);

			// Raw input
			File.WriteAllLines(Path.Combine(folder, "raw.txt"), rawlines);

			// Parse lines into usable form
			var lines = MesenLine.ConvertTrace(rawlines.ToList());

			var notMesen =
				rawlines
					.Where(x => !string.IsNullOrEmpty(x) && !MesenLine.IsA(x));
			File.WriteAllLines(Path.Combine(folder, "not-mesen.txt"), notMesen);
			notMesen = null;
			rawlines = null;

			// Simple, just code & address ordered
			var code =
				lines
					.Select(x => $"{x.Address} {x.Op} {x.Parameters}".Trim())
					.Distinct()
					.OrderBy(x => x)
					.ToList();
			File.WriteAllLines(Path.Combine(folder, "code.txt"), code);

			// Code with labels and line breaks
			var labeled = SourceProcessing.LabelCode(code);
			File.WriteAllLines(Path.Combine(folder, "code-labeled.txt"), labeled);
			code = null;
			labeled = null;

			// Address/instruction usage counts
			var addresses =
				lines
					.Where(x => !string.IsNullOrEmpty(x.Target))
					.Select(x => $"{x.Target} {x.Op}");
			GroupCountAndWriteFile(addresses, Path.Combine(folder, "address-usage.txt"));
			addresses = null;

			// Code instruction counts
			var codeAddresses =
				lines
					.Select(x => x.Address);
			GroupCountAndWriteFile(codeAddresses, Path.Combine(folder, "code-address-counts.txt"));
			codeAddresses = null;

			// Move counts
			var moves =
				lines
					.Where(x => x.Op == "mvn" || x.Op == "mvp")
					.Select(x => $"{getMoveAddresses(x)} {x.Op}");
			GroupCountAndWriteFile(moves, Path.Combine(folder, "moves.txt"));
			moves = null;

			// Move counts - source
			moves =
				lines
					.Where(x => x.Op == "mvn" || x.Op == "mvp")
					.Select(x => $"{getMoveAddresses(x).Split(' ')[0]} {x.Op}");
			GroupCountAndWriteFile(moves, Path.Combine(folder, "moves-source.txt"));
			moves = null;

			// Move counts - destination
			moves =
				lines
					.Where(x => x.Op == "mvn" || x.Op == "mvp")
					.Select(x => $"{getMoveAddresses(x).Split(' ')[2]} {x.Op}");
			GroupCountAndWriteFile(moves, Path.Combine(folder, "moves-destination.txt"));
			moves = null;

			// Move counts - both
			moves =
				lines
					.Where(x => x.Op == "mvn" || x.Op == "mvp")
					.Select(x => $"{getMoveAddresses(x).Split(' ')[0]} {x.Op}")
					.Concat(
						lines
							.Where(x => x.Op == "mvn" || x.Op == "mvp")
							.Select(x => $"{getMoveAddresses(x).Split(' ')[2]} {x.Op}")
						);
			GroupCountAndWriteFile(moves, Path.Combine(folder, "moves-both.txt"));
			moves = null;


			// With missing sections
			var olines = Line.ToLines(lines);
			var groups = LineGroup.MakeGroups(olines);
			olines = null;
			var missing = SourceProcessing.GetMissingOutput(groups, (x) => x.ToString());
			File.WriteAllLines(Path.Combine(folder, "with-missing.txt"), missing);
			missing = null;
			groups = null;

			// Jump targets
			Regex targetsRegex = new Regex(@"^(?:jsr|jrl|jmp|jml)$", RegexOptions.Compiled);
			Regex containsIndirect = new Regex(@",|\[|\(", RegexOptions.Compiled);
			var targets =
				lines
					.Where(x => targetsRegex.IsMatch(x.Op) && containsIndirect.IsMatch(x.Parameters))
					.Select(x => $"{x.Address} {x.Op} {x.Parameters} [{x.Target}]");
			GroupCountAndWriteFile(targets, Path.Combine(folder, "jump-targets.txt"));
			targets = null;

			// Wrong bytecode conversions
			var wrong =
				lines
					.Where(x => x.BytecodeOriginal != x.Bytecode)
					.Select(x => $"{x.Op} {x.Parameters}   log={x.BytecodeOriginal}  computed={x.Bytecode}")
					.ToList();
			File.WriteAllLines(Path.Combine(folder, "wrong-bytecode.txt"), wrong);
			wrong = null;
			lines = null;
		}

		static void GroupCountAndWriteFile(IEnumerable<string> lines, string filename) {
			var output =
				lines
					.GroupBy(x => x)
					.Select(x => $"{x.Key} - {x.Count()}")
					.OrderBy(x => x)
					.ToList();
			File.WriteAllLines(filename, output);
		}

		static string getMoveAddresses(MesenLine line) {
			Match match;
			if (AddressingMode.BlockMove.IsMatch(line.Parameters)) {
				match = AddressingMode.BlockMove.Match(line.Parameters);
			} else if (AddressingMode.BlockMoveArrow.IsMatch(line.Parameters)) {
				match = AddressingMode.BlockMoveArrow.Match(line.Parameters);
			} else {
				throw new Exception("Unknown addressing Mode");
			}

			var source = $"${match.Groups[1].Value}{line.State.X}";
			var dest = $"${match.Groups[2].Value}{line.State.Y}";

			return $"{source} -> {dest}";
		}

		static void ProcessSimple(string filename) {
			var folder = BuildFolder(filename);

			var lines =
				SimpleLine
					.FromFile(filename)
					.OrderBy(x => x.Address);

			var groups = LineGroup.MakeGroups(lines);

			var missing = SourceProcessing.GetMissingOutput(groups, (x) => x.ToString());
			File.WriteAllLines(Path.Combine(folder, "with-missing.txt"), missing);

			var bytecodes = SourceProcessing.GetBadBytecodeConversions(lines);
			File.WriteAllLines(Path.Combine(folder, "bad-bytecode-conversions.txt"), bytecodes);

			var allraw = SourceProcessing.GetAllRaw(lines, (x) => x.ToString());
			File.WriteAllLines(Path.Combine(folder, "all-raw.txt"), allraw);

			var allrawOps = SourceProcessing.GetAllRaw(lines, (x) => x.ToLongString());
			File.WriteAllLines(Path.Combine(folder, "all-raw-with-ops.txt"), allrawOps);

			// Code with labels and line breaks
			var labeled = SourceProcessing.LabelCode(allraw);
			File.WriteAllLines(Path.Combine(folder, "code-labeled.txt"), labeled);
		}



		static void GetC90572Parameters() {
			var filename = @"c:\working\dq3\c90572 parameters.txt";
			var searchTerm = new byte[] { 0x22, 0x72, 0x05, 0xC9 };
			var lines = FindInRom(searchTerm, 4, 0xb);
			File.WriteAllLines(filename, lines);
		}

		static void GetC90566Parameters() {
			var filename = @"c:\working\dq3\c90566 parameters.txt";
			var searchTerm = new byte[] { 0x22, 0x66, 0x05, 0xC9 };
			var lines = FindInRom(searchTerm, 4, 0xb);
			File.WriteAllLines(filename, lines);
		}

		static void LookingForxce0080() {
			var filename = @"c:\working\dq3\looking for $ce0080.txt";
			//var searchTerm = new byte[] { 0xA9, 0x80, 0x00 };
			//var lines = FindInRom(searchTerm, 0, 0xa);
			var searchTerm = new byte[] { 0xee, 0x45, 0xc5 };
			var lines = FindInRom(searchTerm, 0, 0x3);
			File.WriteAllLines(filename, lines);
		}

		static void Getc90717Calls() {
			var filename = @"c:\working\dq3\c90717 calls.txt";
			var searchTerm = new byte[] { 0x22, 0x17, 0x07, 0xC9 };
			var lines = FindInRom(searchTerm, -9, 9);
			File.WriteAllLines(filename, lines);
		}

		static void GetPossibleGoldSpots() {
			var filename = @"c:\working\dq3\PossibleGoldSpots.txt";
			//var searchTerm = new byte[] { 0xad, 0x96, 0x36 };
			var searchTerm = new byte[] { 0x96, 0x36 };
			var lines = FindInRom(searchTerm, -32, 64);
			File.WriteAllLines(filename, lines);
		}

		static void LookForAString() {
			var filename = @"c:\working\dq3\~~string search results.txt";


			var searchTerm = "いざないのほこら";
			//var searchTerm = "";
			//var searchTerm = "";
			//var searchTerm = "";
			//var searchTerm = "";
			//var searchTerm = "";
			//var searchTerm = "";


			var lines = FindSmallTextInRom(searchTerm, true);
			File.WriteAllLines(filename, lines);
		}

		static void LookForABunchOfStrings() {
			var filename = @"c:\working\dq3\~~string search results.txt";

			var searchTerms = Unknown.Instance.Known;

			var lines = new List<string>();

			foreach (var term in searchTerms) {
				lines.Add(term[0]);
				lines.Add(term[1]);
				lines.AddRange(FindSmallTextInRom(term[0], true));
				lines.Add(Environment.NewLine);
			}

			File.WriteAllLines(filename, lines);
		}

		static void DecodeStringBlocks() {
			var filename = @"c:\working\dq3\~~string block read results -- {0}.txt";
			var lists = All.AllLists.Concat(new List<TextList> { All.Instance });

			// Process each separately
			foreach (var list in lists) {
				var lines = new List<string>();

				var maxAddress = list.RoughEndAddress - Rom.AddressOffset;
				var stream = Rom.GetStream(list.StartAddress);

				while (stream.Address < maxAddress) {
					var startAddress = stream.Address;
					var data = stream.ReadUntil(SmallFontTable.EndOfString);
					var endAddress = stream.Address - 2;

					startAddress += (int)Rom.AddressOffset;
					endAddress += (int)Rom.AddressOffset;
					var jap = SmallFontTable.Decode(data);
					var eng = list.ToEnglish(jap);
					lines.Add($"{startAddress.ToString("x6", CultureInfo.InvariantCulture)} - {endAddress.ToString("x6", CultureInfo.InvariantCulture)} -- {data.Length.ToString("x2", CultureInfo.InvariantCulture)} -- {data.ToHexString()} -- {jap} -- {eng}");
				}

				File.WriteAllLines(string.Format(CultureInfo.InvariantCulture, filename, list.TitleTag), lines);
			}
		}

		static void DecodeStringBlock() {
			var filename = @"c:\working\dq3\~~string block read results.txt";
			var lines = new List<string>();

			var maxAddress = 0xfede00 - Rom.AddressOffset;
			var stream = Rom.GetStream(MonsterNames.Instance.StartAddress);

			while (stream.Address < maxAddress) {
				var startAddress = stream.Address;
				var data = stream.ReadUntil(SmallFontTable.EndOfString);
				var endAddress = stream.Address - 2;

				startAddress += (int)Rom.AddressOffset;
				endAddress += (int)Rom.AddressOffset;
				var jap = SmallFontTable.Decode(data);
				var eng = MonsterNames.Instance.ToEnglish(jap);
				lines.Add($"{startAddress.ToString("x6", CultureInfo.InvariantCulture)} - {endAddress.ToString("x6", CultureInfo.InvariantCulture)} -- {data.Length.ToString("x2", CultureInfo.InvariantCulture)} -- {data.ToHexString()} -- {jap} -- {eng}");
			}

			File.WriteAllLines(filename, lines);
		}

		static List<string> FindInRom(byte[] searchTerm, int dataOffset, int dataLength) {
			var spots =
			   new ByteArrayStream(Rom.ROM)
				   .FindAll(searchTerm)
				   .Select(x => new {
					   Address = x.Address + Rom.AddressOffset,
					   DataAddress = x.Address + Rom.AddressOffset + dataOffset,
					   Data = x.GetBytes(dataLength, x.Address + dataOffset)
				   });

			var lines =
				spots
					.Select(x =>
						$"{x.Address.ToString("x6", CultureInfo.InvariantCulture)} -- {x.DataAddress.ToString("x6", CultureInfo.InvariantCulture)} -- {string.Join(" ", x.Data.Select(y => y.ToString("x2", CultureInfo.InvariantCulture)))}"
					);

			return lines.ToList();
		}

		static List<string> FindSmallTextInRom(string searchTerm, bool includeNextCharacter = false) {
			if (SmallFontTable.TryEncode(searchTerm, out byte[] data)) {
				return FindInRom(data, 0, data.Length + (includeNextCharacter ? 1 : 0));
			}

			return new List<string> { "ERROR: Couldn't encode string correctly" };
			//throw new Exception("Couldn't encode string correctly");
		}

		static void OutputHexChunkFFMQ() {
			//int startAddress = 0x0895ee;
			//int endAddress = 0x089c4e;
			int startAddress = 0x07b013;
			int endAddress = 0x07b013 + 0x2900;
			var filename = @"c:\working\ffmq\~OutputHexChunk.txt";

			int size = endAddress - startAddress + 1;

			var s = FFMQ.Game.Rom.GetStream(startAddress);
			var data = s.GetBytes(size);

			Utilities.WriteBytesToFile(data, filename);
			//File.WriteAllLines(filename, new string[] { data.ToHexString() });
		}

		static void FFMQGetLongTextLookups() {
			var filename = @"c:\working\ffmq\longtext.tbl";
			//var filename = @"c:\working\ffmq\longtext.raw.txt";
			var lines =
				FFMQ.LongText
					.GetTextLookups()
					.Select(x => $"{x.Key.ToString("X2", CultureInfo.InvariantCulture)}={x.Value}")
					//.GetRawTextLookups()
					//.Select(x => $"{x.Key.ToString("X2")}={x.Value.ToHexString()}")
					.OrderBy(x => x)
					.ToList();

			File.WriteAllLines(filename, lines);
		}

		static void FFMQGetLongTextLines() {
			var filename = @"c:\working\ffmq\~long text.txt";
			var lines =
				FFMQ.LongText.GetLongStrings()
					.Select(x => $"{x.Key.ToString("x6", CultureInfo.InvariantCulture)} - {x.Value}")
					.OrderBy(x => x)
					.ToList();

			File.WriteAllLines(filename, lines);
		}

		static void FFMQGetLongTextUnder30JumpTable() {
			var filename = @"c:\working\ffmq\~long text under 30 jump table.txt";
			var stream = FFMQ.LongText.Under30JumpTable();
			var lines =
				Enumerable.Range(0, 0x2f)
					.Select(x => $"{x.ToString("x2", CultureInfo.InvariantCulture)} - {stream.Word().ToString("x6", CultureInfo.InvariantCulture)}")
					.OrderBy(x => x)
					.ToList();

			File.WriteAllLines(filename, lines);
		}

		static void FFMQGetLongTextOffsets() {
			var rom = FFMQ.Game.Rom;
			var filename = @"c:\working\ffmq\~long text offsets.txt";
			var stream = FFMQ.LongText.Offsets();
			var lines =
				Enumerable.Range(0, 0x7b)
					.Select(x =>
						new {
							Index = x.ToString("x2", CultureInfo.InvariantCulture),
							Source = rom.AddressToSNES(stream.Address).ToString("x6", CultureInfo.InvariantCulture),
							Address = 0x030000 + stream.Word()
						}
					)
					.Select(x => $"{x.Index} - {x.Source} - {x.Address.ToString("x6", CultureInfo.InvariantCulture)} - {FFMQGetLongTextOffsets_Helper(x.Address)}")
					.OrderBy(x => x)
					.ToList();

			File.WriteAllLines(filename, lines);
		}

		static string FFMQGetLongTextOffsets_Helper(int address) {
			try {
				return address == 0x030000 ? "" : LongText.AttemptTranslateLine(FFMQ.Game.Rom.GetStream(address));
			} catch (Exception ex) {
				return ex.Message;
			}
		}

		static void FFMQGetLongTextFromOffsets() {
			var rom = FFMQ.Game.Rom;
			var filename = @"c:\working\ffmq\~long text from offsets.txt";
			var stream = FFMQ.LongText.Offsets();
			var addresses =
				Enumerable.Range(0, 0x7b)
					.Select(x => 0x030000 + stream.Word())
					.Where(x => x != 0x030000)
					.Distinct()
					.OrderBy(x => x)
					.ToList();

			addresses.Add(addresses[^1] + 0x30);

			var lines = new List<string>();
			var nl = Environment.NewLine;

			for (int i = 0; i < addresses.Count - 1; i++) {
				var data = rom.GetStream(addresses[i]).GetBytes(addresses[i + 1] - addresses[i]);

				lines.Add($"{addresses[i].ToString("x6", CultureInfo.InvariantCulture)}{nl}{data.ToHexString()}{nl}{FFMQGetLongTextFromOffsets_Helper(data)}{nl}");
			}

			File.WriteAllLines(filename, lines);
		}

		static string FFMQGetLongTextFromOffsets_Helper(byte[] data) {
			try {
				return LongText.AttemptTranslateLine(data);
			} catch (Exception ex) {
				return ex.Message;
			}
		}
		static void FZeroFindJetColorsJSR() {
			var rom = new LoRom {
				Filename = @"c:\working\F-ZERO (U) [!].smc"
			};

			var searchTerm =
				string.Join("",
					new List<string> {
						SNES.OpToHex("000000", "lsr", "a"),
						SNES.OpToHex("000000", "lsr", "a"),
						SNES.OpToHex("000000", "lsr", "a"),
						SNES.OpToHex("000000", "sep", "#$20"),
						SNES.OpToHex("000000", "sta", "$4202"),
						SNES.OpToHex("000000", "lda", "#$5e"),
						SNES.OpToHex("000000", "sta", "$4203"),
						SNES.OpToHex("000000", "nop", ""),
						SNES.OpToHex("000000", "nop", ""),
						SNES.OpToHex("000000", "nop", ""),
					})
				.Split(2)
				.Select(x => byte.Parse(x, NumberStyles.HexNumber, CultureInfo.InvariantCulture))
				.ToArray();

			var dataOffset = searchTerm.Length;
			var dataLength = 16 + 3;

			var found =
				rom
					.GetStream(0x8000)
					.FindAll(searchTerm)
					   .Select(x => new {
						   Address = rom.AddressToSNES(x.Address),
						   DataAddress = rom.AddressToSNES(x.Address) + dataOffset,
						   Data = x.GetBytes(dataLength, x.Address + dataOffset)
					   });

			var lines =
				found
					.Select(x =>
						$"{x.Address.ToString("x6", CultureInfo.InvariantCulture)} -- {x.DataAddress.ToString("x6", CultureInfo.InvariantCulture)} -- {string.Join(" ", x.Data.Select(y => y.ToString("x2", CultureInfo.InvariantCulture)))}"
					);

			Directory.CreateDirectory(@"c:\working\fzero\");
			var filename = @"c:\working\fzero\~find jsr.txt";
			File.WriteAllLines(filename, lines);
		}


		static void CompareAndFind() {
			var before = File.ReadAllBytes(@"c:\working\dq3\wram before attack slime.dmp");
			var after = File.ReadAllBytes(@"c:\working\dq3\wram before attack slime 2.dmp");
			var output = new List<string>();

			for (var i = 0; i < before.Length; i++) {
				if ((before[i] == 8) && (after[i] == 0)) {
					output.Add(i.ToString("x4", CultureInfo.InvariantCulture));
				}
			}

			File.WriteAllLines(@"c:\working\dq3\wram before attack slime.txt", output);
		}

		static void FindMinusMinus() {
			var one = File.ReadAllBytes("C:\\working\\dq3\\moving\\Work RAM - 1.dmp");
			var two = File.ReadAllBytes("C:\\working\\dq3\\moving\\Work RAM - 2.dmp");
			var three = File.ReadAllBytes("C:\\working\\dq3\\moving\\Work RAM - 3.dmp");

			if ((one.Length != two.Length) || (two.Length != three.Length)) {
				Console.WriteLine("Work RAM size mismatch!");
				return;
			}

			for (int i = 0; i < one.Length; i++) {
				if ((one[i] == two[i] + 1) && (two[i] == three[i] + 1)) {
					Console.WriteLine($"Found: {i:X2}");
				}
			}
		}

		static void FindOneBitDifferent() {
			var one = File.ReadAllBytes("C:\\working\\dq3\\moving\\Work RAM - 1.dmp");
			var two = File.ReadAllBytes("C:\\working\\dq3\\moving\\Work RAM - 2.dmp");

			if (one.Length != two.Length) {
				Console.WriteLine("Work RAM size mismatch!");
				return;
			}

			var count = 0;
			for (int i = 0; i < one.Length; i++) {
				if (BitOperations.PopCount((uint)(one[i] ^ two[i])) == 1) {
					Console.WriteLine($"Found: {i:X2}");
					count++;
				}
			}

			Console.WriteLine();
			Console.WriteLine($"Count: {count}");
		}

		static void FindOneBitDifferent2() {
			var ones = new List<byte[]>() {
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w10.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w11.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w12.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w13.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w14.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w15.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w16.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w17.dmp"),
			};

			var twos = new List<byte[]>() {
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w20.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w21.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w22.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w23.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w24.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w25.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w26.dmp"),
				File.ReadAllBytes("C:\\working\\dq3\\moving\\w27.dmp"),
			};

			var length = ones[0].Length;

			var count = 0;
			for (int i = 0; i < length; i++) {
				//Console.WriteLine(i);
				var one = ones[0][i];
				var two = twos[0][i];
				if (BitOperations.PopCount((uint)(one ^ two)) == 1) {
					if (ones.All(x => x[i] == one) && twos.All(x => x[i] == two)) {
						var newBit = (((one ^ two) & two) != 0) ? 1 : 0;
						Console.WriteLine($"Found: {i:X6}    {newBit}");
						count++;
					}
				}
			}

			Console.WriteLine();
			Console.WriteLine($"Count: {count}");
		}

		// |  Levels  ||  $042345 - $04246a  ||  98 longs (294 bytes)  ||  Hero's experience required per level, first value is to reach level 2
		static readonly int[] levelsHero = { 29, 87, 174, 304, 499, 792, 1232, 1891, 2880, 4364, 6218, 8534, 11428, 15045, 19114, 23690, 28837, 34627, 41141, 48468, 56711, 65983, 76413, 88147, 101347, 116196, 132901, 151694, 172836, 196621, 223378, 253480, 287344, 325440, 368298, 416512, 470752, 531771, 600417, 677644, 764524, 862263, 960002, 1057741, 1155480, 1253219, 1350958, 1448697, 1546436, 1644175, 1741914, 1839653, 1937392, 2035131, 2132870, 2230609, 2328348, 2426087, 2523826, 2621565, 2719304, 2817043, 2914782, 3012521, 3110260, 3207999, 3305738, 3403477, 3501216, 3598955, 3696694, 3794433, 3892172, 3989911, 4087650, 4185389, 4283128, 4380867, 4478606, 4576345, 4674084, 4771823, 4869562, 4967301, 5065040, 5162779, 5260518, 5358257, 5455996, 5553735, 5651474, 5749213, 5846952, 5944691, 6042430, 6140169, 6237908, 6335647 };
		// |  Levels  ||  $0417ad - $0418d2  ||  98 longs (294 bytes)  ||  Soldier's experience required per level, first value is to reach level 2
		static readonly int[] levelsSoldier = { 12, 36, 84, 156, 264, 426, 669, 1033, 1579, 2398, 3627, 5163, 7083, 9483, 12483, 16233, 20920, 26779, 34102, 42340, 51608, 62034, 73763, 86957, 101801, 118500, 137286, 158421, 182197, 208945, 239036, 272888, 310972, 353816, 402015, 456239, 517241, 585868, 663073, 749928, 847639, 945350, 1043061, 1140772, 1238483, 1336194, 1433905, 1531616, 1629327, 1727038, 1824749, 1922460, 2020171, 2117882, 2215593, 2313304, 2411015, 2508726, 2606437, 2704148, 2801859, 2899570, 2997281, 3094992, 3192703, 3290414, 3388125, 3485836, 3583547, 3681258, 3778969, 3876680, 3974391, 4072102, 4169813, 4267524, 4365235, 4462946, 4560657, 4658368, 4756079, 4853790, 4951501, 5049212, 5146923, 5244634, 5342345, 5440056, 5537767, 5635478, 5733189, 5830900, 5928611, 6026322, 6124033, 6221744, 6319455, 6417166 };
		// |  Levels  ||  $041c06 - $041d2b  ||  98 longs (294 bytes)  ||  Pilgrim's experience required per level, first value is to reach level 2
		static readonly int[] levelsPilgrim = { 14, 42, 98, 182, 308, 497, 780, 1205, 1842, 2798, 4232, 6024, 8264, 11064, 14564, 18939, 24407, 30559, 37479, 45263, 54020, 63872, 74955, 87423, 101450, 117229, 134981, 154952, 177419, 202694, 231128, 263116, 299102, 339585, 385128, 436364, 494004, 558849, 631799, 713867, 806194, 910061, 1026912, 1143763, 1260614, 1377465, 1494316, 1611167, 1728018, 1844869, 1961720, 2078571, 2195422, 2312273, 2429124, 2545975, 2662826, 2779677, 2896528, 3013379, 3130230, 3247081, 3363932, 3480783, 3597634, 3714485, 3831336, 3948187, 4065038, 4181889, 4298740, 4415591, 4532442, 4649293, 4766144, 4882995, 4999846, 5116697, 5233548, 5350399, 5467250, 5584101, 5700952, 5817803, 5934654, 6051505, 6168356, 6285207, 6402058, 6518909, 6635760, 6752611, 6869462, 6986313, 7103164, 7220015, 7336866, 7453717 };
		// |  Levels  ||  $041a93 - $041bb8  ||  98 longs (294 bytes)  ||  Wizard's experience required per level, first value is to reach level 2
		static readonly int[] levelsWizard = { 15, 45, 105, 195, 330, 532, 835, 1290, 1973, 2997, 4533, 6453, 8853, 11853, 15603, 20290, 25563, 31495, 38169, 45676, 54121, 63622, 74310, 86334, 99861, 115078, 132197, 151456, 173121, 197494, 224913, 255758, 290458, 329495, 373412, 422818, 478399, 540927, 611271, 690408, 779436, 879592, 992268, 1119028, 1245788, 1372548, 1499308, 1626068, 1752828, 1879588, 2006348, 2133108, 2259868, 2386628, 2513388, 2640148, 2766908, 2893668, 3020428, 3147188, 3273948, 3400708, 3527468, 3654228, 3780988, 3907748, 4034508, 4161268, 4288028, 4414788, 4541548, 4668308, 4795068, 4921828, 5048588, 5175348, 5302108, 5428868, 5555628, 5682388, 5809148, 5935908, 6062668, 6189428, 6316188, 6442948, 6569708, 6696468, 6823228, 6949988, 7076748, 7203508, 7330268, 7457028, 7583788, 7710548, 7837308, 7964068 };
		// |  Levels  ||  $04212d - $0422f7  ||  98 longs (294 bytes)  ||  Sage's experience required per level, first value is to reach level 2
		static readonly int[] levelsSage = { 20, 60, 140, 260, 440, 710, 1115, 1722, 2633, 3999, 6047, 8607, 11807, 15807, 20807, 27057, 34869, 43657, 53543, 64664, 77175, 91250, 107083, 124895, 144933, 167475, 192835, 221365, 253461, 289568, 330188, 375885, 427293, 485126, 550188, 623383, 705726, 798362, 902577, 1019818, 1151714, 1300096, 1448478, 1596860, 1745242, 1893624, 2042006, 2190388, 2338770, 2487152, 2635534, 2783916, 2932298, 3080680, 3229062, 3377444, 3525826, 3674208, 3822590, 3970972, 4119354, 4267736, 4416118, 4564500, 4712882, 4861264, 5009646, 5158028, 5306410, 5454792, 5603174, 5751556, 5899938, 6048320, 6196702, 6345084, 6493466, 6641848, 6790230, 6938612, 7086994, 7235376, 7383758, 7532140, 7680522, 7828904, 7977286, 8125668, 8274050, 8422432, 8570814, 8719196, 8867578, 9015960, 9164342, 9312724, 9461106, 9609488 };
		// |  Levels  ||  $041920 - $041a45  ||  98 longs (294 bytes)  ||  Fighter's experience required per level, first value is to reach level 2
		static readonly int[] levelsFighter = { 18, 54, 126, 234, 396, 639, 1003, 1549, 2369, 3598, 5441, 7745, 10625, 14225, 18725, 24350, 30678, 37797, 45805, 54814, 64949, 76350, 89176, 103605, 119837, 138098, 158641, 181751, 207749, 236996, 269898, 306912, 348552, 395397, 448097, 507384, 574081, 649115, 733528, 828492, 935326, 1055514, 1190725, 1325936, 1461147, 1596358, 1731569, 1866780, 2001991, 2137202, 2272413, 2407624, 2542835, 2678046, 2813257, 2948468, 3083679, 3218890, 3354101, 3489312, 3624523, 3759734, 3894945, 4030156, 4165367, 4300578, 4435789, 4571000, 4706211, 4841422, 4976633, 5111844, 5247055, 5382266, 5517477, 5652688, 5787899, 5923110, 6058321, 6193532, 6328743, 6463954, 6599165, 6734376, 6869587, 7004798, 7140009, 7275220, 7410431, 7545642, 7680853, 7816064, 7951275, 8086486, 8221697, 8356908, 8492119, 8627330 };
		// |  Levels  ||  $041d79 - $041e9e  ||  98 longs (294 bytes)  ||  Merchant's experience required per level, first value is to reach level 2
		static readonly int[] levelsMerchant = { 10, 30, 70, 130, 220, 355, 557, 860, 1315, 1998, 3022, 4302, 5902, 7902, 10402, 13527, 17433, 22315, 27807, 33985, 40935, 48754, 57550, 67445, 78577, 91100, 105188, 121037, 138867, 158925, 181490, 206876, 235435, 267563, 303707, 344368, 390112, 441573, 499467, 564597, 637868, 720298, 813032, 905766, 998500, 1091234, 1183968, 1276702, 1369436, 1462170, 1554904, 1647638, 1740372, 1833106, 1925840, 2018574, 2111308, 2204042, 2296776, 2389510, 2482244, 2574978, 2667712, 2760446, 2853180, 2945914, 3038648, 3131382, 3224116, 3316850, 3409584, 3502318, 3595052, 3687786, 3780520, 3873254, 3965988, 4058722, 4151456, 4244190, 4336924, 4429658, 4522392, 4615126, 4707860, 4800594, 4893328, 4986062, 5078796, 5171530, 5264264, 5356998, 5449732, 5542466, 5635200, 5727934, 5820668, 5913402 };
		// |  Levels  ||  $041eec - $042011  ||  98 longs (294 bytes)  ||  GoofOff's experience required per level, first value is to reach level 2
		static readonly int[] levelsGoofOff = { 11, 33, 77, 143, 242, 390, 612, 946, 1447, 2198, 3324, 4732, 6492, 8692, 11442, 14879, 19175, 24545, 31258, 38810, 47306, 56863, 67614, 79709, 93316, 108623, 125844, 145217, 167012, 191531, 219114, 250145, 285055, 324329, 368511, 418216, 474134, 537042, 607813, 687430, 776998, 877762, 991121, 1104480, 1217839, 1331198, 1444557, 1557916, 1671275, 1784634, 1897993, 2011352, 2124711, 2238070, 2351429, 2464788, 2578147, 2691506, 2804865, 2918224, 3031583, 3144942, 3258301, 3371660, 3485019, 3598378, 3711737, 3825096, 3938455, 4051814, 4165173, 4278532, 4391891, 4505250, 4618609, 4731968, 4845327, 4958686, 5072045, 5185404, 5298763, 5412122, 5525481, 5638840, 5752199, 5865558, 5978917, 6092276, 6205635, 6318994, 6432353, 6545712, 6659071, 6772430, 6885789, 6999148, 7112507, 7225866 };
		// |  Levels  ||  $04205f - $042184  ||  98 longs (294 bytes)  ||  Thief's experience required per level, first value is to reach level 2
		static readonly int[] levelsThief = { 13, 39, 78, 136, 223, 353, 548, 840, 1278, 1935, 2920, 4397, 6243, 8550, 11433, 15036, 19539, 25167, 32202, 40116, 49019, 59034, 70300, 82974, 97232, 113272, 131317, 151617, 174454, 200145, 229047, 261561, 298139, 339289, 385582, 437661, 496249, 562160, 636309, 719726, 813570, 907414, 1001258, 1095102, 1188946, 1282790, 1376634, 1470478, 1564322, 1658166, 1752010, 1845854, 1939698, 2033542, 2127386, 2221230, 2315074, 2408918, 2502762, 2596606, 2690450, 2784294, 2878138, 2971982, 3065826, 3159670, 3253514, 3347358, 3441202, 3535046, 3628890, 3722734, 3816578, 3910422, 4004266, 4098110, 4191954, 4285798, 4379642, 4473486, 4567330, 4661174, 4755018, 4848862, 4942706, 5036550, 5130394, 5224238, 5318082, 5411926, 5505770, 5599614, 5693458, 5787302, 5881146, 5974990, 6068834, 6162678 };

		static void LevelsAsHex() {
			var all =
				string.Join(' ',
					levelsThief
						.Select(x => BitConverter.ToString(
							BitConverter.GetBytes(x), 0, 3)
						.Replace('-', ' ')
						)
					)
				.ToLowerInvariant();

			Console.WriteLine(all);
		}

		//static void FindByteDifference(byte[] one, byte[] two, byte original, byte changed) {
		//
		//}

		static void FindByteDifferenceDQ4HP() {
			var one = File.ReadAllBytes("C:\\working\\dq3\\moving\\hp ram 5.dmp");
			var two = File.ReadAllBytes("C:\\working\\dq3\\moving\\hp ram 6.dmp");
			byte original = 35;
			byte changed = 27;

			if (one.Length != two.Length) {
				Console.WriteLine("Work RAM size mismatch!");
				return;
			}

			var count = 0;
			for (int i = 0; i < one.Length; i++) {
				if ((one[i] == original) && (two[i] == changed)) {
					Console.WriteLine($"Found: {i:x2}");
					count++;
				}
			}

			Console.WriteLine();
			Console.WriteLine($"Count: {count}");
		}
	}
}
