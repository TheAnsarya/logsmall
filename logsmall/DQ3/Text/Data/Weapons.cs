using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3.Text.Data {
	class Weapons : TextList {
		private Weapons() { }

		private static Weapons _instance = null;
		public static Weapons Instance {
			get {
				if (_instance == null) {
					_instance = new Weapons();
				}

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(Weapons); }
		public override int StartAddress { get => 0xfed0d5; }
		public override int EndAddress { get => 0xfed2a4; }
		public override int RoughEndAddress { get => EndAddress; }

		// Entries have been ordered
		// Entries have been verified
		// Translations from: https://gamefaqs.gamespot.com/snes/564402-dragon-quest-iii-soshite-densetsu-e/faqs/7856
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
			new string[] { "ひのきのぼう", "Hinoki no Bou (Cypress Stick)" },
			new string[] { "こんぼう", "Konbou (Club)" },
			new string[] { "どうのつるぎ", "Dou no Tsurugi (Copper Sword)" },
			new string[] { "せいなるナイフ", "Seinarunaifu (Sacred Knife)" },
			new string[] { "てつのやり", "Tetsu no Yari (Iron Spear)" },
			new string[] { "てつのオノ", "Tetsu no Ono (Iron Axe)" },
			new string[] { "はがねのつるぎ", "Hagane no Tsurugi (Steel Sword)" },
			new string[] { "まどうしのつえ", "Madoushi no Tsue (Wizard Staff)" },
			new string[] { "どくばり", "Dokubari (Poison Needle)" },
			new string[] { "てつのつめ", "Tetsu no Tsume (Iron Claw)" },
			new string[] { "とげのむち", "Toge no Muchi (Thorn Whip)" },
			new string[] { "おおばさみ", "Oobasami (Giant Shears)" },
			new string[] { "くさりがま", "Kusarigama (Chain Sickle)" },
			new string[] { "らいじんのけん", "Raijin no Ken (Thunder God Sword)" },
			new string[] { "ふぶきのつるぎ", "Fubuki no Tsurugi (Blizzard Sword)" },
			new string[] { "まじんのオノ", "Majin no Ono (Devil Axe)" },
			new string[] { "あまぐものつえ", "Amagumo no Tsue (Rain Staff)" },
			new string[] { "ガイアのつるぎ", "Gaia no Tsurugi (Gaia Sword)" },
			new string[] { "さざなみのつえ", "Sazanami no Tsue (Staff of Reflection)" },
			new string[] { "はかいのつるぎ", "Hakai no Tsurugi (Destruction Sword)" },
			new string[] { "もろはのつるぎ", "Moroha no Tsurugi (Double-Edged Sword)" },
			new string[] { "りりょくのつえ", "Riryoku no Tsue (Force Staff)" },
			new string[] { "ゆうわくのけん", "Yuuwaku no Ken (Temptation Sword)" },
			new string[] { "ゾンビキラー", "Zonbikira- (Zombie Killer)" },
			new string[] { "はやぶさのけん", "Hayabusa no Ken (Falcon Sword)" },
			new string[] { "おおかなづち", "Ookanadzuchi (Giant Hammer)" },
			new string[] { "おうごんのつめ", "Ougon no Tsume (Gold Claw)" },
			new string[] { "いなずまのけん", "Inazuma no Ken (Lightning Sword)" }, // in docs as "いなずみのけん"
			new string[] { "いかずちのつえ", "Ikazuchi no Tsue (Thunder Staff)" },
			new string[] { "おうじゃのけん", "Ouja no Ken (King Sword)" },
			new string[] { "くさなぎのけん", "Kusanagi no Ken (Grass Cutter Sword)" },
			new string[] { "ドラゴンキラー", "Doragonkira- (Dragon Killer)" },
			new string[] { "さばきのつえ", "Sabaki no Tsue (Staff of Judgement)" },
			new string[] { "はがねのむち", "Hagane no Muchi (Steel Whip)" },
			new string[] { "チェーンクロス", "Che-nkurosu (Chain Cross)" },
			new string[] { "グリンガムのムチ", "Guringamu no Muchi (Gringam Whip)" },
			new string[] { "モーニングスター", "Mo-ningusuta- (Morning Star)" },
			new string[] { "ブーメラン", "Bu-meran (Boomerang)" },
			new string[] { "やいばのブーメラン", "Yaiba no Bu-meran (Blade Boomerang)" },
			new string[] { "ほのおのブーメラン", "Honoo no Bu-meran (Flame Boomerang)" },
			new string[] { "はかいのてっきゅう", "Hakai no Tekkyuu (Iron Ball of Destruction)" },
			new string[] { "てつのそろばん", "Tetsu no Soroban (Iron Abacus)" }, // dummy entry
			new string[] { "まほうのそろばん", "Mahou no Soroban (Magic Abacus)" },
			new string[] { "せいぎのそろばん", "Seigi no Soroban (Abacus of Virtue)" },
			new string[] { "バスタードソード", "Basuta-doso-do (Bastard Sword)" },
			new string[] { "ルーンスタッフ", "Ru-nsutaffu (Rune Staff)" },
			new string[] { "けんじゃのつえ", "Kenja no Tsue (Sage Staff)" },
			new string[] { "ねむりのつえ", "Nemuri no Tsue (Sleep Staff)" },
			new string[] { "アサシンダガー", "Asashindaga- (Assassin Dagger)" },
			new string[] { "ホーリーランス", "Ho-riransu (Holy Lance)" }, // in docs as "ホーリランス"
			new string[] { "パワーナックル", "Pawa-nakkuru (Power Knuckle)" },
			new string[] { "ドラゴンクロウ", "Doragon Kurou (Dragon Claw)" },
			new string[] { "まじゅうのツメ", "Majuu no Tsume (Beast Claw)" }, // in docs as "まじゅうのつめ"
			new string[] { "ブロンズナイフ", "Bronzunaifu (Bronze Knife)" },
			new string[] { "はがねのはりせん", "Hagane no Harisen (Steel Needle)" },
			new string[] { "まふうじのつえ", "Mafuuji no Tsue (Staff of Silence)" },
			new string[] { "ふっかつのつえ", "Fukkatsu no Tsue (Revival Staff)" },
			new string[] { "バトルアックス", "Batoruakkusu (Battle Axe)" },
			new string[] { "ウォーハンマー", "Uo-hanma (War Hammer)" }, // in docs as "ウオーハンマー"
			new string[] { "ドラゴンテイル", "Doragonteiru (Dragon Tail)" },
		};
	}
}
