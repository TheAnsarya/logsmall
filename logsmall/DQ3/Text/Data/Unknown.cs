using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3.Text.Data {
	class Unknown : TextList {
		private Unknown() { }

		private static Unknown _instance;
		public static Unknown Instance {
			get {
				_instance ??= new Unknown();

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(Unknown); }
		public override int StartAddress { get => 0xfecfb7; } // TODO: Not set
		public override int EndAddress { get => 0xfee82b; } // TODO: Not set
		public override int RoughEndAddress { get => EndAddress; }

		// TODO: Entries have NOT been ordered
		// TODO: Entries have NOT been verified
		// Translations from: https://gamefaqs.gamespot.com/snes/564402-dragon-quest-iii-soshite-densetsu-e/faqs/15068
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
new string[] { "ぼうけんをする", "Go out on the Adventure" },
new string[] { "せっていをかえる", "Change the Establisment" },
new string[] { "ぼうけんしょをつくる", "Create Adventure Log" },
new string[] { "ぼうけんしょ1", "Adventure Log 1" },
new string[] { "ぼうけんしょ2", "Adventure Log 2" },
new string[] { "ぼうけんしょ3", "Adventure Log 3" },
new string[] { "ぼうけんしょをうつす", "Copy Adventure Log" },
new string[] { "ぼうけんしょをけす", "Erase Adventure Log" },
new string[] { "はい", "Yes" },
new string[] { "いいえ", "No" },
new string[] { "おとこ", "Male" },
new string[] { "おんな", "Female" },
new string[] { "ひょうじそくど", "Message Display Speed" },
new string[] { "はやい", "Fast" },
new string[] { "おそい", "Slow" },
new string[] { "ステレオ", "Stereo" },
new string[] { "モノラル", "Monaural" },
new string[] { "はなす", "Talk" },
new string[] { "どうぐ", "Item" },
new string[] { "つかう", "Use" },
new string[] { "わたす", "Hand Over" },
new string[] { "そうび", "Equip" },
new string[] { "みる", "Look" },
new string[] { "みせる", "Show" },
new string[] { "すてる", "Throw Away" },
new string[] { "やめる", "End" },
new string[] { "ふくろ", "Sack" },
new string[] { "つよさ", "Strength" },
new string[] { "せいべつ", "Gender" },
new string[] { "せいかくめい", "Name of Personality" },
new string[] { "レベル", "Level" },
new string[] { "ちから", "Power" },
new string[] { "すばやさ", "Agility" },
new string[] { "たいりょく", "Physical" },
new string[] { "かしこさ", "Intelligence" },
new string[] { "うんのよさ", "Luck" },
new string[] { "さいだいえっちぴー", "Maximum Hit Point" },
new string[] { "さいだいえむぴー", "Maximum Magic Power" },
new string[] { "こうげきりょく", "Attack Power" },
new string[] { "しゅびりょく", "Defense Power" },
new string[] { "けいけんち", "Experience" },
new string[] { "ぜんいん", "All Members" },
new string[] { "じゅもん", "Spell" },
new string[] { "しらべる", "Search" },
new string[] { "さくせん", "Strategy" },
new string[] { "まんたん", "Fill Up" },
new string[] { "そうび", "Equip" },
new string[] { "ならびかえ", "Change Line Up" },
new string[] { "ひょうじそくど", "Display Speed" },
new string[] { "どうぐせいり", "Sort Items" },
new string[] { "ぜんいん", "All" },
new string[] { "ふくろせいり", "Sort Sack" },
new string[] { "しゅべつじゅん", "Order by Classification" },
new string[] { "じゅん", "Order by aiueo" },
new string[] { "いきかえらせる", "Revive" },
new string[] { "どくのちりょう", "Cure Poison" },
new string[] { "のろいをとく", "Remove Curse" },
new string[] { "あずける", "Deposit" },
new string[] { "ひきだす", "Withdraw" },
new string[] { "なかまをよびだす", "Call on Friends" },
new string[] { "なかまをあずける", "Leave Friends" },
new string[] { "めいぼをみる", "See the Register of Names" },
new string[] { "やめる", "End" },
new string[] { "じぶんでやる", "Do it yourself" },
new string[] { "おまかせにする", "Entrust to others" },
new string[] { "こうげき", "Attack" },
new string[] { "じゅもん", "Spell" },
new string[] { "ぼうぎょ", "Defend" },
new string[] { "どうぐ", "Item" },
new string[] { "そうび", "Equip" },
new string[] { "にげる", "Run Away" },
		};
	}
}
