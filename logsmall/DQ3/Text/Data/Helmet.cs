using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3.Text.Data {
	class Helmet : TextList {
		private Helmet() { }

		private static Helmet _instance;
		public static Helmet Instance {
			get {
				_instance ??= new Helmet();

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(Helmet); }
		public override int StartAddress { get => 0xfed436; }
		public override int EndAddress { get => 0xfed4ba; }
		public override int RoughEndAddress { get => EndAddress + 0xff; }

		// Entries have been ordered
		// Entries have been verified
		// Translations from: https://gamefaqs.gamespot.com/snes/564402-dragon-quest-iii-soshite-densetsu-e/faqs/7856
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
			new string[] { "きんのかんむり", "Kin no Kanmuri (Gold Crown)" },
			new string[] { "てつかぶと", "Tetsukabuto (Iron Helmet)" },
			new string[] { "ふしぎなぼうし", "Fushigi na Boushi (Mystery Hat)" },
			new string[] { "ふこうのかぶと", "Fukou no Kabuto (Sorrow Helmet)" },
			new string[] { "オルテガのかぶと", "Orutega no Kabuto (Ortega's Helmet)" },
			new string[] { "ターバン", "Ta-ban (Turban)" },
			new string[] { "はんにゃのめん", "Hannya no Men (Noh Mask)" }, // in docs as "はんにやのめん"
			new string[] { "かわのぼうし", "Kawa no Boushi (Leather Hat)" },
			new string[] { "てっかめん", "Tekkamen (Iron Mask)" },
			new string[] { "きのぼうし", "Ki no Boushi (Wooden Helmet)" },
			new string[] { "けがわのフード", "Kegawa no Hu-do (Fur Hood)" },
			new string[] { "ぎんのかみかざり", "Gin no Kamikazari (Silver Hair Ornament)" },
			new string[] { "グレートヘルム", "Gure-toherumu (Great Helm)" },
			new string[] { "くろずきん", "Kurozukin (Black Hood)" },
			new string[] { "うさみみバンド", "Usamimibando (Bunny Ears Band)" },
			new string[] { "シルクハット", "Shirukuhatto (Silk Hat)" },
			new string[] { "ミスリルヘルム", "Misuriruherumu (Mithril Helm)" },
			new string[] { "とんがりぼうし", "Tongari no Boushi (Pointed Hat)" }, // in docs as "とんがりのぼうし"
		};
	}
}
