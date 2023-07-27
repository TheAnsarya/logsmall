using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3.Text.Data {
	class Shield : TextList {
		private Shield() { }

		private static Shield _instance;
		public static Shield Instance {
			get {
				_instance ??= new Shield();

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(Shield); }
		public override int StartAddress { get => 0xfed3d6; }
		public override int EndAddress { get => 0xfed435; }
		public override int RoughEndAddress { get => EndAddress; }

		// Entries have been ordered
		// Entries have been verified
		// Translations from: https://gamefaqs.gamespot.com/snes/564402-dragon-quest-iii-soshite-densetsu-e/faqs/7856
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
			new string[] { "かわのたて", "Kawa no Tate (Leather Shield)" },
			new string[] { "てつのたて", "Tetsu no Tate (Iron Shield)" },
			new string[] { "ちからのたて", "Chikara no Tate (Power Shield)" },
			new string[] { "ゆうしゃのたて", "Yuusha no Tate (Hero Shield)" },
			new string[] { "なげきのたて", "Nageki no Tate (Grief Shield)" },
			new string[] { "せいどうのたて", "Seidou no Tate (Bronze Shield)" },
			new string[] { "みかがみのたて", "Mikagami no Tate (Silver Shield)" },
			new string[] { "うろこのたて", "Uroko no Tate (Scale Shield)" },
			new string[] { "まほうのたて", "Mahou no Tate (Magic Shield)" },
			new string[] { "ドラゴンシールド", "Doragonshi-rudo (Dragon Shield)" },
			new string[] { "ふうじんのたて", "Fuujin no Tate (Wind God Shield)" },
			new string[] { "おなべのフタ", "Onabe no Futa (Pot Lid)" },
			new string[] { "オーガシールド", "O-gashi-rudo (Ogre Shield)" },
		};
	}
}
