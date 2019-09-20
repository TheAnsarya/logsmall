using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3.Text.Data {
	class DebugMenu : TextList {
		private DebugMenu() { }

		private static DebugMenu _instance = null;
		public static DebugMenu Instance {
			get {
				if (_instance == null) {
					_instance = new DebugMenu();
				}

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(DebugMenu); }
		public override int StartAddress { get => 0xfed009; }
		public override int EndAddress { get => 0xfed0d5; }
		public override int RoughEndAddress { get => EndAddress; }

		// Entries have been ordered
		// Entries have been verified
		// Gaps indicate the 4 pages of the menu
		// Translations from: most from DQ Translations patch --- TODO: these need to be better
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
			new string[] { "いんちきルーラ", "Return" },
			new string[] { "いんちきリレミト", "Outside" },
			new string[] { "いんちきラナルータ", "Day-Night" },
			new string[] { "おみせ", "Key places" },
			new string[] { "レベルアップ", "Level up" },
			new string[] { "てんしょく", "Change class" },
			new string[] { "せいかく", "Personality" },
			new string[] { "おかね", "Okane (Money)" },

			new string[] { "フラグショップ", "Flag shop" },
			new string[] { "ひとじょうたい", "Status effects" },
			new string[] { "ゲームじょうたい", "Game status" },
			new string[] { "どうぐふくろ", "Fill bag" },
			new string[] { "せんたくせんとう", "Choose battle" },
			new string[] { "タイルせんとう", "Tile battles" },
			new string[] { "モンスターをみる", "View monsters" },
			new string[] { "ぜんかいふく", "Fully year" },

			new string[] { "プログラムスタート", "Program start" },
			new string[] { "サウンド", "Sound" },
			new string[] { "ほこうせってい", "Collision disable" },
			new string[] { "エンカウントせってい", "Encounter setup" },
			new string[] { "スタートフロアせってい", "Start floor setup" },
			new string[] { "ざひょうウインドウ", "Coordinate window" },
			new string[] { "ひとひょうじ", "People display" },
			new string[] { "ROMバージョン", "ROM Version" },

			new string[] { "プレゼンよう", "For presentation" },
			new string[] { "へんげのつえ", "Henge no Tsue (Change Staff)" },
		};
	}
}
