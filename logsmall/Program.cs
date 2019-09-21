using logsmall.Common;
using logsmall.Compression;
using logsmall.DataStructures;
using logsmall.DQ3.Text;
using logsmall.DQ3.Text.Data;
using logsmall.SourceCode;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace logsmall {
	class Program {
		static void Main(string[] args) {

			//OutputHexChunkFFMQ();

			//SimpleTailWindowCompression.TestLayout();
			SimpleTailWindowCompression.TestDumpData();

			//LookForABunchOfStrings();
			//DecodeStringBlocks();

			//GetC90572Parameters();
			//GetC90566Parameters();

			//var bytecode = SNES.OpToHex("c01576", "jml", "[$1d9a]");
			//var t = 0;
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\ffmq - maybe tilemap2 - no interrupts.txt");
			//Getc90717Calls();
			//GetPossibleGoldSpots();
			//LookForAString();
			//DecodeStringBlock();


			//BasicRing400.TestLayout();


			//TrimLog();
			//MergeTrace();
			//FixA16();
			//TestRange64();
			//ConvertBranches();
			//sortAddresses();

			//var filename = @"c:\working\dq3\dq3-a.log"
			//var filename = @"C:\Users\Andy\OneDrive\Desktop\DQ3 Stuff\dq3 - decompressing ow bg1.log";
			//processBSNES(filename);

			//var filename = @"C:\Users\Andy\OneDrive\Desktop\DQ3 Stuff\dq3 decompress overworld map - bg1 - from c0539b - wuth zeros.txt";
			//processMesen(filename);

			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600fd -- jsl $c907cc.txt");


			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\trying to find water 02.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600d9 -- jsl $c60b57 -- c086 stuff.txt");

			//OverworldMap2.TestGetLayout();
			//MakeTilesImage();
			//OverworldMap2.GetMapImage();
			//OverworldMap2.TestTilemapToChunks();
			//OverworldMap2.ProcessDQ4NesMap();
			//OverworldMap2.TranslateDQ4Map();
			//OverworldMap2.DrawDQ4SnesMap();

			//var filename = @"C:\Users\Andy\OneDrive\Desktop\DQ3 Stuff\dq3-all-raw.log";
			//processSimple(filename);
		}

		private static string BuildFolder(string filename) {
			var folder = Path.Combine(Path.GetDirectoryName(filename), $"{Path.GetFileNameWithoutExtension(filename)} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}");
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
				var goalbig = ((double)hp / (double)maxhp * 63).ToString("0.##");
				var goalstring = Convert.ToString(goal, 2).PadLeft(6, '0');
				var path = new List<ConsoleColor>();
				string bvals = "";

				var max = maxhp;
				var current = hp;

				for (int j = 0; j < 6; j++) {
					var b = max & 0x001;
					bvals += b;
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

				if (goalstring == output) {
					good++;
				} else {
					bad++;
				}

				Console.Write(
					maxhp.ToString().PadLeft(2) + " " + Convert.ToString(maxhp, 2).PadLeft(6, '0') + " " +
					hp.ToString().PadLeft(2) + " " + Convert.ToString(hp, 2).PadLeft(6, '0') + " " +
					goal.ToString().PadLeft(2) + " ");
				Console.Write(" ");
				for (int k = 0; k < 6; k++) {
					Console.BackgroundColor = path[k];
					Console.ForegroundColor = goalstring[k] == output[k] ? ConsoleColor.White : ConsoleColor.Red;
					Console.Write(goalstring[k]);
				}
				Console.BackgroundColor = ConsoleColor.Black;
				Console.Write("  ");
				for (int k = 0; k < 6; k++) {
					Console.BackgroundColor = path[k];
					Console.ForegroundColor = goalstring[k] == output[k] ? ConsoleColor.White : ConsoleColor.Red;
					Console.Write(output[k]);
				}
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = (goalstring == output) ? ConsoleColor.White : ConsoleColor.Red;
				Console.Write("  " + Convert.ToInt32(output, 2) + "  ");
				for (int k = 0; k < 6; k++) {
					Console.BackgroundColor = path[k];
					Console.ForegroundColor = goalstring[k] == output[k] ? ConsoleColor.White : ConsoleColor.Red;
					Console.Write(bvals[k]);
				}
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = (goalstring == output) ? ConsoleColor.White : ConsoleColor.Red;
				Console.WriteLine(" " + goalbig);
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
				using (var readera = new StreamReader(aname)) {
					using (var readerb = new StreamReader(bname)) {
						string linea = readera.ReadLine();
						string lineb = readerb.ReadLine();
						int a = 1;
						int b = 1;

						while ((linea != null) || (lineb != null)) {
							Console.WriteLine("a: " + a.ToString().PadLeft(8) + " b: " + b.ToString().PadLeft(8));

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

							string commanda = shorta.Replace("\t", "    ").Substring(0, Math.Min(21, shorta.Length)).Trim();
							string commandb = shortb.Replace("\t", "    ").Substring(0, Math.Min(21, shortb.Length)).Trim();
							int ikeya = int.Parse(shorta.Substring(0, 6), NumberStyles.HexNumber);
							int ikeyb = int.Parse(shortb.Substring(0, 6), NumberStyles.HexNumber);

							if (ikeya == ikeyb) {
								if (commanda.CompareTo(commandb) == 0) {
									if (bufferb.Length > 0) {
										outfile.Write(bufferb.ToString());
										bufferb.Clear();
									}

									outfile.WriteLine(lineb);

									linea = readera.ReadLine();
									a++;
									lineb = readerb.ReadLine();
									b++;
								} else if (commanda.CompareTo(commandb) < 0) {
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
							} else if (ikeya < ikeyb) {
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

						Console.WriteLine("a: " + a.ToString().PadLeft(8) + " b: " + b.ToString().PadLeft(8));
						Console.WriteLine("------------ DONE ------------");
					}
				}
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

			using (var infile = new StreamReader(infilename)) {
				using (var outfile = new StreamWriter(outfilename)) {
					while ((line = infile.ReadLine()) != null) {
						//outfile.WriteLine(line.Substring(0, 30));
						outfile.WriteLine(line.Substring(0, 21).Trim());
						//outfile.WriteLine(line.Substring(2));
					}
				}
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
				using (var readera = new StreamReader(aname)) {
					using (var readerb = new StreamReader(bname)) {
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
									string code = lineb.Substring(0, 6);
									do {
										linea = readera.ReadLine();
									} while (linea != null && (linea.Length < 6 || linea.Substring(0, 6) != code));

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
				}
			}

			Console.ReadKey();
		}


		static void sortAddresses() {
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


		static void processBSNES(string filename) {
			var lines = File.ReadAllLines(filename);
			var folder = Path.Combine(Path.GetDirectoryName(filename), $"{Path.GetFileNameWithoutExtension(filename)} {DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss")}");
			Directory.CreateDirectory(folder);

			// Raw input
			File.WriteAllLines(Path.Combine(folder, "raw.txt"), lines);

			// Simple, just code & address ordered
			var code =
				lines
					.Where(x => x != "")
					.Select(x => x.Substring(0, 20).Trim())
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

		static void processMesen(string filename) {
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
					.Where(x => !MesenLine.IsA(x));
			File.WriteAllLines(Path.Combine(folder, "not-mesen.txt"), notMesen);
			notMesen = null;

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
			var missing = SourceProcessing.GetMissingOutput(groups, (x) => x.ToString());
			File.WriteAllLines(Path.Combine(folder, "with-missing.txt"), missing);
			missing = null;

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

			var f = 8;
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
			} else { throw new Exception("Unknown addressing Mode"); }
			var source = $"${match.Groups[1].Value}{line.State.X}";
			var dest = $"${match.Groups[2].Value}{line.State.Y}";

			return $"{source} -> {dest}";
		}

		static void processSimple(string filename) {
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
					var (data, startAddress, endAddress) = stream.ReadUntil(SmallFontTable.EndOfString);
					startAddress += (int)Rom.AddressOffset;
					endAddress += (int)Rom.AddressOffset;
					var jap = SmallFontTable.Decode(data);
					var eng = list.ToEnglish(jap);
					lines.Add($"{startAddress.ToString("x6")} - {endAddress.ToString("x6")} -- {data.Length.ToString("x2")} -- {data.ToHexString()} -- {jap} -- {eng}");
				}

				File.WriteAllLines(string.Format(filename, list.TitleTag), lines);
			}
		}

		static void DecodeStringBlock() {
			var filename = @"c:\working\dq3\~~string block read results.txt";
			var lines = new List<string>();

			var maxAddress = 0xfede00 - Rom.AddressOffset;
			var stream = Rom.GetStream(MonsterNames.Instance.StartAddress);

			while (stream.Address < maxAddress) {
				var (data, startAddress, endAddress) = stream.ReadUntil(SmallFontTable.EndOfString);
				startAddress += (int)Rom.AddressOffset;
				endAddress += (int)Rom.AddressOffset;
				var jap = SmallFontTable.Decode(data);
				var eng = MonsterNames.Instance.ToEnglish(jap);
				lines.Add($"{startAddress.ToString("x6")} - {endAddress.ToString("x6")} -- {data.Length.ToString("x2")} -- {data.ToHexString()} -- {jap} -- {eng}");
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
						$"{x.Address.ToString("x6")} -- {x.DataAddress.ToString("x6")} -- {string.Join(" ", x.Data.Select(y => y.ToString("x2")))}"
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
			int startAddress = 0x0895ee;
			int endAddress = 0x089c4e;
			var filename = @"c:\working\ffmq\~OutputHexChunk.txt";

			int size = endAddress - startAddress + 1;

			var s = FFMQ.Game.Rom.GetStream(startAddress);
			var data = s.GetBytes(size);

			//WriteBytesToFile(data, filename);
			File.WriteAllLines(filename, new string[] { data.ToHexString() });
		}

		public static void WriteBytesToFile(byte[] data, string filename) {
			var lines = data.ToHexStrings();
			File.WriteAllLines(filename, lines);
		}
	}
}
