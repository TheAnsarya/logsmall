using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3.Text.Data {
	class Personalities : TextList {
		private Personalities() { }

		private static Personalities _instance = null;
		public static Personalities Instance {
			get {
				if (_instance == null) {
					_instance = new Personalities();
				}

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(Personalities); }
		public override int StartAddress { get => 0xfed824; }
		public override int EndAddress { get => 0xfed957; }
		public override int RoughEndAddress { get => EndAddress; }

		// Entries have been ordered
		// Entries have been verified
		// Translations from: https://gamefaqs.gamespot.com/snes/564402-dragon-quest-iii-soshite-densetsu-e/faqs/7856
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
			new string[] { "ちからじまん", "Chikarajiman (Braggart)" },
			new string[] { "ごうけつ", "Gouketsu (Heroic)" },
			new string[] { "らんぼうもの", "Ranboumono (Violent)" },
			new string[] { "おとこまさり", "Otokomasari (Mannish)" },
			new string[] { "おおぐらい", "Oogurai (Glutton)" },
			new string[] { "ねっけつかん", "Nekketsukan (Hot-Blooded)" },
			new string[] { "おてんば", "Otenba (Tomboy)" },
			new string[] { "いのちしらず", "Inochishirazu (Daredevil)" },
			new string[] { "むっつりスケベ", "Muttsurisukebe (Sullen)" },
			new string[] { "おっちょこちょい", "Occhokochoi (Careless)" },
			new string[] { "まけずぎらい", "Makezugirai (Determined)" },
			new string[] { "のんきもの", "Nonkimono (Happy-Go-Lucky)" },
			new string[] { "せけんしらず", "Sekenshirazu (Ignorant)" },
			new string[] { "がんこもの", "Gankomono (Stubborn)" },
			new string[] { "おじょうさま", "Ojousama (Ladylike)" },
			new string[] { "しょうじきもの", "Shojikimono (Honest Person)" }, // is in docs as "しょじきもの"
			new string[] { "ふつう", "Futsuu (Ordinary)" },
			new string[] { "でんこうせっか", "Denkousekka (Lightning Speed)" },
			new string[] { "すばしっこい", "Subashikkoi (Nimble)" },
			new string[] { "ぬけめがない", "Nukemeganai (Cunning)" },
			new string[] { "きれもの", "Kiremono (Sharp Person)" },
			new string[] { "ひねくれもの", "Hinekuremono (Rebel)" },
			new string[] { "みえっぱり", "Mieppari (Vain)" },
			new string[] { "わがまま", "Wagamama (Selfish)" },
			new string[] { "なきむし", "Nakimushi (Crybaby)" },
			new string[] { "さびしがりや", "Sabishigariya (Lonely)" },
			new string[] { "タフガイ", "Tafugai (Tough Guy)" },
			new string[] { "てつじん", "Tetsujin (Strong Man)" },
			new string[] { "へこたれない", "Hekotarenai (Losing Heart)" },
			new string[] { "くろうにん", "Kurounin (Worldly-Wise)" },
			new string[] { "がんばりや", "Ganbariya (Tenacious)" },
			new string[] { "おせっかい", "Osekkai (Nosy)" },
			new string[] { "なまけもの", "Namakemono (Lazy)" },
			new string[] { "ずのうめいせき", "Zunoumeiseki (Clear-Headed)" },
			new string[] { "あたまでっかち", "Atamadekkachi (Egg-Headed)" },
			new string[] { "ロマンチスト", "Romanchisuto (Romantist)" },
			new string[] { "あまえんぼう", "Amaenbou (Spoiled Child)" },
			new string[] { "ラッキーマン", "Rakki-man (Lucky Man)" },
			new string[] { "しあわせもの", "Shiawasemono (Happy Person)" },
			new string[] { "うっかりもの", "Ukkarimono (Thoughtless)" },
			new string[] { "いっぴきおおかみ", "Ippikiookami (Lone Wolf)" },
			new string[] { "いくじなし", "Ikujinashi (Coward)" },
			new string[] { "おちょうしもの", "Ochoushimono (Frivolous)" },
			new string[] { "ひっこみじあん", "Hikkomijian (Shy)" },
			new string[] { "やさしいひと", "Yasashiihito (Promiscuous)" },
			new string[] { "セクシーギャル", "Sekushi-gyaru (Sexy Gal)" },
		};
	}
}
