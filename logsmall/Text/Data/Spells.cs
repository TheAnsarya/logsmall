using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Text.Data {
	class Spells : TextList {
		private Spells() { }

		private static Spells _instance = null;
		public static Spells Instance {
			get {
				if (_instance == null) {
					_instance = new Spells();
				}

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(Spells); }
		public override int StartAddress { get => 0xfedd76; }
		public override int EndAddress { get => 0xfedee8; }
		public override int RoughEndAddress { get => EndAddress; }

		// Entries have been ordered
		// Entries have been verified
		// Translations from: https://gamefaqs.gamespot.com/snes/564402-dragon-quest-iii-soshite-densetsu-e/faqs/7856
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
			new string[] { "メラ", "Mera (Blaze)" },
			new string[] { "メラミ", "Merami (Blazemore)" },
			new string[] { "メラゾーマ", "Merazo-ma (Blazemost)" },
			new string[] { "ギラ", "Gira (Fireball)" },
			new string[] { "ベギラマ", "Begirama (Firebane)" },
			new string[] { "ベギラゴン", "Begiragon (Firevolt)" },
			new string[] { "イオ", "Io (Bang)" },
			new string[] { "イオラ", "Iora (Boom)" },
			new string[] { "イオナズン", "Ionazun (Explodet)" },
			new string[] { "ヒャド", "Hyado (Icebolt)" },
			new string[] { "ヒャダルコ", "Hyadaruko (Snowblast)" },
			new string[] { "ヒャダイン", "Hyadain (Icespears)" },
			new string[] { "マヒャド", "Mahyado (Snowstorm)" },
			new string[] { "バギ", "Bagi (Infernos)" },
			new string[] { "バギマ", "Bagima (Infermore)" },
			new string[] { "バギクロス", "Bagikurosu (Infermost)" },
			new string[] { "ライデイン", "Raidin (Zap)" },
			new string[] { "ギガデイン", "Gigadein (Lightning)" },
			new string[] { "ザキ", "Zaki (Beat)" },
			new string[] { "ザラキ", "Zaraki (Defeat)" },
			new string[] { "メガンテ", "Megante (Sacrifice)" },
			new string[] { "ニフラム", "Nifuramu (Expel)" },
			new string[] { "バシルーラ", "Bashiru-ra (Limbo)" },
			new string[] { "マホトラ", "Mahotora (Robmagic)" },
			new string[] { "ボミオス", "Bomiosu (Slow)" },
			new string[] { "ホイミ", "Hoimi (Heal)" },
			new string[] { "ベホイミ", "Behoimi (Healmore)" },
			new string[] { "ベホマ", "Behoma (Healall)" },
			new string[] { "ベホマラー", "Behomara- (Healus)" },
			new string[] { "ベホマズン", "Behomazun (Healusall)" },
			new string[] { "ザオラル", "Zaoraru (Vivify)" },
			new string[] { "ザオリク", "Zaoriku (Revive)" },
			new string[] { "ラリホー", "Rariho- (Sleep)" },
			new string[] { "ザメハ", "Zameha (Awaken)" },
			new string[] { "マホトーン", "Mahoto-n (Stopspell)" },
			new string[] { "マヌーサ", "Manu-sa (Surround)" },
			new string[] { "ルーラ", "Ru-ra (Return)" },
			new string[] { "ルカニ", "Rukani (Sap)" },
			new string[] { "ルカナン", "Rukanan (Defense)" },
			new string[] { "スカラ", "Sukara (Upper)" },
			new string[] { "スクルト", "Sukuruto (Increase)" },
			new string[] { "ピオリム", "Piorimu (Speed Up)" },
			new string[] { "メダパニ", "Medapani (Chaos)" },
			new string[] { "モシャス", "Moshasu (Transform)" },
			new string[] { "ドラゴラム", "Doragoramu (Bedragon)" },
			new string[] { "アストロン", "Asutoron (Ironize)" },
			new string[] { "マホカンタ", "Mahokanta (Magic Counter)" },
			new string[] { "フバーハ", "Fuba-ha (Barrier)" },
			new string[] { "バイキルト", "Baikiruto (Bikill)" },
			new string[] { "キアリー", "Kiari (Antidote)" }, // in docs as "キアリ"
			new string[] { "キアリク", "Kiariku (Numboff)" },
			new string[] { "パルプンテ", "Parupunte (Chance)" },
			new string[] { "リレミト", "Reremito (Outside)" },
			new string[] { "トヘロス", "Toherosu (Repel)" },
			new string[] { "インパス", "Inpasu (X-Ray)" },
			new string[] { "トラマナ", "Toramana (Stepguard)" },
			new string[] { "ラナルータ", "Ranaru-ta (Day/Night)" },
			new string[] { "シャナク", "Shanaku (Curseoff)" },
			new string[] { "レムオル", "Remuoru (Invisible)" },
			new string[] { "アバカム", "Abakamu (Open)" },
			new string[] { "おもいだす", "Omoidasu (Recall)" },
			new string[] { "もっとおもいだす", "Motto Omoidasu (More Recall)" },
			new string[] { "ふかくおもいだす", "Fukaku Omoidasu (Mega Recall)" },
			new string[] { "わすれる", "Wasureru (Forget)" },
			new string[] { "タカのめ", "Taka no Me (Hawk Eye)" },
			new string[] { "フローミ", "Furo-mi (Location)" },
			new string[] { "しのびあし", "Shinobi no Ashi (Shinobi Feet)" },
			new string[] { "とうぞくのはな", "Touzoku no Hana (Thief Nose)" },
			new string[] { "レミラーマ", "Remira-ma (Seek Out)" },
			new string[] { "くちぶえ", "Kuchibue (Whistle)" },
			new string[] { "あなほり", "Anahori (Dig Hole)" },
			new string[] { "おおごえ", "Oogoe (Loud Voice)" },
		};
	}
}
