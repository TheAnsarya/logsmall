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

			TestRange64();
		}

		private static void TestRange64() {
			//int maxhp = 63;
			int maxhp = 10;
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
			string aname = @"c:\working\ffmq-a.log";
			string bname = @"c:\working\ffmq-b.log";
			string outname = @"c:\working\ffmq-merge.log";

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
			string infilename = @"c:\working\ffmq.log";
			string outfilename = @"c:\working\ffmq-small.log";
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
	}
}
