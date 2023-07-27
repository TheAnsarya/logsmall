using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3.Text.Data {
	class Decoration : TextList {
		private Decoration() { }

		private static Decoration _instance;
		public static Decoration Instance {
			get {
				_instance ??= new Decoration();

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(Decoration); }

		// Entries are NOT contiguous
		public override int StartAddress { get => 0xfed4bb; }
		public override int EndAddress { get => 0xfed724; }
		public override int RoughEndAddress { get => EndAddress; }

		// Entries have been ordered
		// Entries have been verified
		// Entries are NOT contiguous
		// Translations from: https://gamefaqs.gamespot.com/snes/564402-dragon-quest-iii-soshite-densetsu-e/faqs/7856
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
			new string[] { "せいなるまもり", "Seinarumamori (Sacred Talisman)" },

			new string[] { "しあわせのくつ", "Shiawase no Kutsu (Happiness Shoes)" },
			new string[] { "ほしふるうでわ", "Hoshifuruudewa (Meteorite Armband)" },

			new string[] { "いのちのゆびわ", "Inochi no Yubiwa (Life Ring)" },
			new string[] { "ちからのゆびわ", "Chikara no Yubiwa (Power Ring)" },
			new string[] { "うさぎのしっぽ", "Usagi no Shippu (Rabbit's Tail)" },
			new string[] { "しっぷうのバンダナ", "Shippuu no Bandana (Gale Bandana)" },
			new string[] { "いしのかつら", "Ishi no Katsura (Stone Wig)" },
			new string[] { "ガーターベルト", "Ga-ta-beruto (Garter Belt)" },
			new string[] { "モヒカンのケ", "Mohikan no Ke (Mohican Hair)" },
			new string[] { "ルビーのうでわ", "Rubi- no Udewa (Ruby Armlet)" },
			new string[] { "いかりのタトゥー", "Ikari no Tatu- (Fury Tattoo)" },
			new string[] { "ごうけつのうでわ", "Gouketsu no Udewa (Heroic Armlet)" },
			new string[] { "インテリめがね", "Interimegane (Intelli-Glasses)" },
			new string[] { "ぎんのロザリオ", "Gin no Rozario (Silver Rosary)" },
			new string[] { "おしゃぶり", "Oshaburi (Pacifier)" },
			new string[] { "ヘビメタリング", "Hebimetaringu (Heavy Metal Ring)" },
			new string[] { "きんのネックレス", "Kin no Nekkuresu (Gold Necklace)" },
			new string[] { "はやてのリング", "Hayate no Ringu (Quick Ring)" },
			new string[] { "パワーベルト", "Pawa-beruto (Power Belt)" },
			new string[] { "きんのクチバシ", "Kin no Kuchibashi (Gold Beak)" },
			new string[] { "おうごんのティアラ", "Ougon no Tiara (Gold Tiara)" }, // in docs as "おうごんのテイアラ"
			new string[] { "スライムピアス", "Suraimupiasu (Slime Pierce)" },
			new string[] { "はくあいリング", "Hakuairingu (Benevolent Ring)" },
			new string[] { "にげにげリング", "Nigenigeringu (Flee Ring)" },
			new string[] { "くじけぬこころ", "Kujikenukokoro (Tough Heart)" },
			new string[] { "ルーズソックス", "Ru-zusokkusu (Loose Socks)" },

			new string[] { "めがみのゆびわ", "Megami no Yubiwa (Goddess Ring)" },
		};
	}
}
