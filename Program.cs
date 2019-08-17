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

			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600bd -- jsl $c6061f.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600c1 -- jsl $c609e0.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600c5 -- jsl $c6029a.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600c9 -- jsl $c60f42.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600cd -- jsl $c60286.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600d1 -- jsl $c60951.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600d5 -- jsl $c60706.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600d9 -- jsl $c60b57.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600dd -- jsl $c60f98.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600e1 -- jsl $c6072b.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600e5 -- jsl $7e9893.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600e9 -- jsl $7e9897.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600ed -- jsl $7e989b.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600f1 -- jsl $c60654.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600f5 -- jsl $7e989f.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600f9 -- jsl $c0629e.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600fd -- jsl $c907cc.txt");

			processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\trying to find water 02.txt");
			//processMesen(@"C:\Users\Andy\Documents\Mesen-S\Debugger\c600d9 -- jsl $c60b57 -- c086 stuff.txt");

			//OverworldMap2.MakeTilesImage();
			//OverworldMap2.GetMapImage();

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

		static void ConvertBranches() {
			//string inname = @"c:\working\dq3\dq3-all.log";
			//string outname = @"c:\working\dq3\dq3-all-branched.log";
			string inname = @"c:\working\dq3\small.log";
			string outname = @"c:\working\dq3\small-branched.log";

			Console.WriteLine("reading");
			var lines = File.ReadAllLines(inname);

			Console.WriteLine("branches");
			var branchRegex = new Regex(@"^([a-f0-9]{2})([a-f0-9]{4}) (bcc|bcs|beq|bmi|bne|bpl|bra|bvc|bvs) \$([a-f0-9]{4})$", RegexOptions.Compiled);
			var branches =
				lines.Where(x => branchRegex.IsMatch(x))
				.Select(x => branchRegex.Match(x))
				.Select(
					x => new LabelMatch {
						line = x.Value,
						newline = x.Groups[1].Value + x.Groups[2].Value + " " + x.Groups[3].Value + " .Branch_" + x.Groups[1].Value + x.Groups[4].Value,
						label = ".Branch_" + x.Groups[1].Value + x.Groups[4].Value,
						targetaddress = x.Groups[1].Value + x.Groups[4].Value
					}
				);

			Console.WriteLine("longBranches");
			var longBranchRegex = new Regex(@"^([a-f0-9]{6}) brl \$([a-f0-9]{6})$", RegexOptions.Compiled);
			var longBranches =
				lines.Where(x => longBranchRegex.IsMatch(x))
				.Select(x => longBranchRegex.Match(x))
				.Select(
					x => new LabelMatch {
						line = x.Value,
						newline = x.Groups[1].Value + " brl .Branch_" + x.Groups[2].Value,
						label = ".Branch_" + x.Groups[2].Value,
						targetaddress = x.Groups[2].Value
					}
				);

			Console.WriteLine("jsr");
			var jsrRegex = new Regex(@"^([a-f0-9]{2})([a-f0-9]{4}) jsr \$([a-f0-9]{4})$", RegexOptions.Compiled);
			var jsr =
				lines.Where(x => jsrRegex.IsMatch(x))
				.Select(x => jsrRegex.Match(x))
				.Select(
					x => new LabelMatch {
						line = x.Value,
						newline = x.Groups[1].Value + x.Groups[2].Value + " jsr Routine_" + x.Groups[1].Value + x.Groups[3].Value,
						label = "Routine_" + x.Groups[1].Value + x.Groups[3].Value + ":",
						targetaddress = x.Groups[1].Value + x.Groups[3].Value
					}
				);

			Console.WriteLine("jsl");
			var jslRegex = new Regex(@"^([a-f0-9]{6}) jsl \$([a-f0-9]{6})$", RegexOptions.Compiled);
			var jsl =
				lines.Where(x => jslRegex.IsMatch(x))
				.Select(x => jslRegex.Match(x))
				.Select(
					x => new LabelMatch {
						line = x.Value,
						newline = x.Groups[1].Value + " jsl Routine_" + x.Groups[2].Value,
						label = "Routine_" + x.Groups[2].Value + ":",
						targetaddress = x.Groups[2].Value
					}
				);

			Console.WriteLine("jmp");
			var jmpRegex = new Regex(@"^([a-f0-9]{2})([a-f0-9]{4}) jmp \$([a-f0-9]{4})$", RegexOptions.Compiled);
			var jmp =
				lines.Where(x => jmpRegex.IsMatch(x))
				.Select(x => jmpRegex.Match(x))
				.Select(
					x => new LabelMatch {
						line = x.Value,
						newline = x.Groups[1].Value + x.Groups[2].Value + " jmp Jump_" + x.Groups[1].Value + x.Groups[3].Value,
						label = "Jump_" + x.Groups[1].Value + x.Groups[3].Value + ":",
						targetaddress = x.Groups[1].Value + x.Groups[3].Value
					}
				);

			Console.WriteLine("jml");
			var jmlRegex = new Regex(@"^([a-f0-9]{6}) jml \$([a-f0-9]{6})$", RegexOptions.Compiled);
			var jml =
				lines.Where(x => jmlRegex.IsMatch(x))
				.Select(x => jmlRegex.Match(x))
				.Select(
					x => new LabelMatch {
						line = x.Value,
						newline = x.Groups[1].Value + " jml Jump_" + x.Groups[2].Value,
						label = "Jump_" + x.Groups[2].Value + ":",
						targetaddress = x.Groups[2].Value
					}
				);

			Console.WriteLine("concat");
			var all = branches.Concat(longBranches).Concat(jsr).Concat(jsl).Concat(jmp).Concat(jml);

			Console.WriteLine("labels");
			var labels =
				all.GroupBy(x => x.targetaddress)
				.ToDictionary(
					x => x.Key,
					x => x.Select(y => y.label).Distinct()
				);

			var dupekeys =
				all.GroupBy(x => x.line)
				.Where(x => x.Count() > 1);

			var emptylines = all.Count(x => x.line == "");
			var oklines = all.Count(x => x.line != "");

			Console.WriteLine("replacements");
			var replacements = all.ToDictionary(x => x.line, x => x.newline);

			Console.WriteLine("writing");
			var addressRegex = new Regex(@"^([a-f0-9]{6}) ", RegexOptions.Compiled);
			string last = null;
			using (var writer = new StreamWriter(outname, false)) {
				foreach (string line in lines) {
					var match = addressRegex.Match(line);
					var address = match.Success ? match.Groups[1].Value : null;
					if ((address != null) && labels.ContainsKey(address)) {
						// attempt to not relabel
						if (last != labels[address].First()) {
							writer.WriteLine(Environment.NewLine + string.Join(Environment.NewLine, labels[address]));
							labels.Remove(address);
						}
					}

					string newline;
					if (replacements.TryGetValue(line, out newline)) {
						writer.WriteLine(newline + Environment.NewLine);
						replacements.Remove(line);
					} else {
						writer.WriteLine(line);
					}

					last = line;
				}
			}

			Console.WriteLine();
			Console.WriteLine("labels unused: " + labels.Count);
			Console.WriteLine("replacements unused: " + replacements.Count);
			Console.ReadKey();
		}

		private class LabelMatch {
			public string line;
			//public Match match;
			public string newline;
			public string label;
			public string targetaddress;
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
			var labeled = LabelCode(code);
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

		static List<string> LabelCode(List<string> lines) {
			var branchRegex = new Regex(@"^([a-f0-9]{2})([a-f0-9]{4}) (bcc|bcs|beq|bmi|bne|bpl|bra|bvc|bvs) \$([a-f0-9]{4}|[a-f0-9]{6})$", RegexOptions.Compiled);
			var branches =
				lines.Where(x => branchRegex.IsMatch(x))
				.Select(x => branchRegex.Match(x))
				.Select(x => new {
					line = x.Value,
					address = $"{x.Groups[1].Value}{x.Groups[2].Value}",
					target = x.Groups[4].Value.Length == 4 ? $"{x.Groups[1].Value}{x.Groups[4].Value}" : x.Groups[4].Value,
					op = x.Groups[3].Value
				})
				.Select(
					x => new LabelMatch {
						line = x.line,
						newline = $"{x.address} {x.op} .Branch_{x.target}",
						label = $".Branch_{x.target}",
						targetaddress = x.target
					}
				);

			var longBranchRegex = new Regex(@"^([a-f0-9]{6}) brl \$([a-f0-9]{6})$", RegexOptions.Compiled);
			var longBranches =
				lines.Where(x => longBranchRegex.IsMatch(x))
				.Select(x => longBranchRegex.Match(x))
				.Select(
					x => new LabelMatch {
						line = x.Value,
						newline = $"{x.Groups[1].Value} brl .Branch_{x.Groups[2].Value}",
						label = $".Branch_{x.Groups[2].Value}",
						targetaddress = x.Groups[2].Value
					}
				);

			var jsrRegex = new Regex(@"^([a-f0-9]{2})([a-f0-9]{4}) jsr \$([a-f0-9]{4}|[a-f0-9]{6})$", RegexOptions.Compiled);
			var jsr =
				lines.Where(x => jsrRegex.IsMatch(x))
				.Select(x => jsrRegex.Match(x))
				.Select(x => new {
					line = x.Value,
					address = $"{x.Groups[1].Value}{x.Groups[2].Value}",
					target = x.Groups[3].Value.Length == 4 ? $"{x.Groups[1].Value}{x.Groups[3].Value}" : x.Groups[3].Value
				})
				.Select(
					x => new LabelMatch {
						line = x.line,
						newline = $"{x.address} jsr Routine_{x.target}",
						label = $"Routine_{x.target}:",
						targetaddress = x.target
					}
				);

			var jslRegex = new Regex(@"^([a-f0-9]{6}) jsl \$([a-f0-9]{6})$", RegexOptions.Compiled);
			var jsl =
				lines.Where(x => jslRegex.IsMatch(x))
				.Select(x => jslRegex.Match(x))
				.Select(
					x => new LabelMatch {
						line = x.Value,
						newline = $"{x.Groups[1].Value} jsl Routine_{x.Groups[2].Value}",
						label = $"Routine_{x.Groups[2].Value}:",
						targetaddress = x.Groups[2].Value
					}
				);

			var jmpRegex = new Regex(@"^([a-f0-9]{2})([a-f0-9]{4}) jmp \$([a-f0-9]{4}|[a-f0-9]{6})$", RegexOptions.Compiled);
			var jmp =
				lines.Where(x => jmpRegex.IsMatch(x))
				.Select(x => jmpRegex.Match(x))
				.Select(x => new {
					line = x.Value,
					address = $"{x.Groups[1].Value}{x.Groups[2].Value}",
					target = x.Groups[3].Value.Length == 4 ? $"{x.Groups[1].Value}{x.Groups[3].Value}" : x.Groups[3].Value
				})
				.Select(
					x => new LabelMatch {
						line = x.line,
						newline = $"{x.address} jmp Jump_{x.target}",
						label = $"Jump_{x.target}:",
						targetaddress =  x.target 
					}
				);

			var jmlRegex = new Regex(@"^([a-f0-9]{6}) jml \$([a-f0-9]{6})$", RegexOptions.Compiled);
			var jml =
				lines.Where(x => jmlRegex.IsMatch(x))
				.Select(x => jmlRegex.Match(x))
				.Select(
					x => new LabelMatch {
						line = x.Value,
						newline = $"{x.Groups[1].Value} jml Jump_{x.Groups[2].Value}",
						label = $"Jump_{x.Groups[2].Value}:",
						targetaddress = x.Groups[2].Value
					}
				);

			var all = branches.Concat(longBranches).Concat(jsr).Concat(jsl).Concat(jmp).Concat(jml);

			var labels =
				all.GroupBy(x => x.targetaddress)
				.ToDictionary(
					x => x.Key,
					x => x.Select(y => y.label).Distinct()
				);

			var dupekeys =
				all.GroupBy(x => x.line)
				.Where(x => x.Count() > 1);

			var emptylines = all.Count(x => x.line == "");
			var oklines = all.Count(x => x.line != "");

			var replacements = all.ToDictionary(x => x.line, x => x.newline);

			var addressRegex = new Regex(@"^([a-f0-9]{6}) ", RegexOptions.Compiled);
			string last = null;

			var output = new List<string>();

			foreach (string line in lines) {
				var match = addressRegex.Match(line);
				var address = match.Success ? match.Groups[1].Value : null;
				if ((address != null) && labels.ContainsKey(address)) {
					output.Add("");
					output.AddRange(labels[address]);
					labels.Remove(address);
				}

				if (replacements.TryGetValue(line, out string newline)) {
					output.Add(newline);
					output.Add("");
					replacements.Remove(line);
				} else {
					output.Add(line);
					if ((line == "rts") || (line == "rtl") || (line == "rti")) {
						output.Add("");
					}
				}

				last = line;
			}

			return output;
		}

		static void processMesen(string filename) {
			var rawlines = File.ReadAllLines(filename);
			//var folder = Path.Combine(Path.GetDirectoryName(filename), $"{Path.GetFileNameWithoutExtension(filename)} {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}");
			var folder = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
			Directory.CreateDirectory(folder);

			// Raw input
			File.WriteAllLines(Path.Combine(folder, "raw.txt"), rawlines);

			// Parse lines into usable form
			var lines =
				rawlines
					.Where(x => MesenLine.IsA(x))
					.Select(x => new MesenLine(x));

			// Simple, just code & address ordered
			var code =
				lines
					.Select(x => $"{x.Address} {x.Op} {x.Parameters}".Trim())
					.Distinct()
					.OrderBy(x => x)
					.ToList();
			File.WriteAllLines(Path.Combine(folder, "code.txt"), code);

			// Code with labels and line breaks
			var labeled = LabelCode(code);
			File.WriteAllLines(Path.Combine(folder, "code-labeled.txt"), labeled);
			labeled = null;

			// Address/instruction usage counts
			var addresses =
				lines
					.Where(x => !string.IsNullOrEmpty(x.Target))
					.Select(x => $"{x.Target} {x.Op}")
					.GroupBy(x => x)
					.Select(x => $"{x.Key} {x.Count()}")
					.OrderBy(x => x)
					.ToList();
			File.WriteAllLines(Path.Combine(folder, "address-usage.txt"), addresses);
			addresses = null;

			// Code instruction counts
			var codeAddresses =
				lines
					.Select(x=>x.Address)
					.GroupBy(x => x)
					.Select(x => $"{x.Key} {x.Count()}")
					.OrderBy(x => x)
					.ToList();
			File.WriteAllLines(Path.Combine(folder, "code-address-counts.txt"), codeAddresses);
			codeAddresses = null;

			var olines = Line.ToLines(lines);
			var groups = LineGroup.MakeGroups(olines);
			var missing = GetMissingOutput(groups, (x) => x.ToString());
			File.WriteAllLines(Path.Combine(folder, "with-missing.txt"), missing);

			// Jump targets
			Regex targetsRegex = new Regex(@"^(?:jsr|jrl|jmp|jml)$", RegexOptions.Compiled);
			Regex containsIndirect = new Regex(@",|\[|\(", RegexOptions.Compiled);
			var targets =
				lines
					.Where(x => targetsRegex.IsMatch(x.Op) && containsIndirect.IsMatch(x.Parameters))
					.Select(x => $"{x.Address} {x.Op} {x.Parameters} [{x.Target}]")
					.GroupBy(x => x)
					.Select(x => $"{x.Key} - {x.Count()}")
					.OrderBy(x => x)
					.ToList();
			File.WriteAllLines(Path.Combine(folder, "jump-targets.txt"), targets);
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

		static void processSimple(string filename) {
			var folder = BuildFolder(filename);

			var lines =
				SimpleLine
					.FromFile(filename)
					.OrderBy(x => x.Address);

			var groups = LineGroup.MakeGroups(lines);

			var missing = GetMissingOutput(groups, (x) => x.ToString());
			File.WriteAllLines(Path.Combine(folder, "with-missing.txt"), missing);

			var bytecodes = GetBadBytecodeConversions(lines);
			File.WriteAllLines(Path.Combine(folder, "bad-bytecode-conversions.txt"), bytecodes);

			var allraw = GetAllRaw(lines, (x) => x.ToString());
			File.WriteAllLines(Path.Combine(folder, "all-raw.txt"), allraw);

			var allrawOps = GetAllRaw(lines, (x) => x.ToLongString());
			File.WriteAllLines(Path.Combine(folder, "all-raw-with-ops.txt"), allrawOps);
		}

		static List<string> GetMissingOutput(List<LineGroup> groups, Func<Line, string> lineToString) {
			var missing = new List<string>();
			LineGroup lastGroup = null;

			foreach (var group in groups) {
				if (lastGroup != null) {
					missing.AddRange(new List<string> {
						"",
						$"; Missing {lastGroup.NextAddress.ToString("x6")} - {(group.StartAddress - 1).ToString("x6")} ({lastGroup.BytesBetween(group)} bytes)",
						""
					});
				}

				missing.AddRange(
					group
						.Lines
						.Select(lineToString) //(x) => x.ToString())
						.ToList()
				);

				lastGroup = group;
			}

			return missing;
		}

		static List<string> GetBadBytecodeConversions(IOrderedEnumerable<Line> lines) {
			var bytecodes =
				lines
					.Where(x => x.AddressRaw >= 0xC00000U)
					.Select(x =>
						new {
							Line = x,
							Converted = x.Bytecode,
							Rom = Rom.GetString(x.AddressRaw, x.ByteLength)
						}
					)
					.Where(x => x.Converted != x.Rom)
					.Select(x => $"{x.Line.ToString()}   rom={x.Rom}  converted={x.Converted}")
					.ToList();
			return bytecodes;
		}

		static List<string> GetAllRaw(IOrderedEnumerable<Line> lines, Func<Line, string> lineToString) {
			var allraw =
				lines
					.Select(lineToString)
					.ToList();
			return allraw;
		}
	}
}
