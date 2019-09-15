using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Text.Data {
	class Classes : TextList {
		private Classes() { }

		private static Classes _instance = null;
		public static Classes Instance {
			get {
				if (_instance == null) {
					_instance = new Classes();
				}

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(Classes); }
		public override int StartAddress { get => 0xfed7f4; }
		public override int EndAddress { get => 0xfed823; }
		public override int RoughEndAddress { get => EndAddress; }

		// Entries have been ordered
		// Entries have been verified
		// Translations from: https://gamefaqs.gamespot.com/snes/564402-dragon-quest-iii-soshite-densetsu-e/faqs/7856
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
			new string[] { "せんし", "Senshi (Soldier)" },
			new string[] { "ぶとうか", "Butouka (Martial Artist)" },
			new string[] { "まほうつかい", "Mahoutsukai (Magician)" },
			new string[] { "そうりょ", "Souryo (Priest)" },
			new string[] { "しょうにん", "Shounin (Merchant)" },
			new string[] { "あそびにん", "Asobinin (Carouser)" },
			new string[] { "とうぞく", "Touzoku (Thief)" },
			new string[] { "けんじゃ", "Kenja (Sage)" },
			new string[] { "ゆうしゃ", "Yuusha (Hero)" },
		};
	}
}
