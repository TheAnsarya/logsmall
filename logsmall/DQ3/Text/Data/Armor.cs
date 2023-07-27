using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3.Text.Data {
	class Armor : TextList {
		private Armor() { }

		private static Armor _instance;
		public static Armor Instance {
			get {
				_instance ??= new Armor();

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(Armor); }
		public override int StartAddress { get => 0xfed2a5; }
		public override int EndAddress { get => 0xfed3d5; }
		public override int RoughEndAddress { get => EndAddress; }

		// Entries have been ordered
		// Entries have been verified
		// Translations from: https://gamefaqs.gamespot.com/snes/564402-dragon-quest-iii-soshite-densetsu-e/faqs/7856
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
			new string[] { "ぬののふく", "Nuno no Fuku (Clothes)" },
			new string[] { "けいこぎ", "Keikogi (Training Suit)" },
			new string[] { "かわのよろい", "Kawa no Yoroi (Leather Armor)" },
			new string[] { "はでなふく", "Hadenafuku (Colorful Clothes)" },
			new string[] { "てつのよろい", "Tetsu no Yoroi (Iron Armor)" },
			new string[] { "はがねのよろい", "Hagane no Yoroi (Steel Armor)" },
			new string[] { "まほうのよろい", "Mahou no Yoroi (Magic Armor)" },
			new string[] { "みかわしのふく", "Mikawashi no Fuku (Evasion Clothes)" },
			new string[] { "ひかりのよろい", "Hikari no Yoroi (Light Armor)" },
			new string[] { "てつのまえかけ", "Tetsu no Maekake (Iron Apron)" },
			new string[] { "ぬいぐるみ", "Nuigurumi (Stuffed Doll)" },
			new string[] { "ぶとうぎ", "Butougi (Martial Arts Suit)" },
			new string[] { "まほうのほうい", "Mahou no Houi (Magic Robe)" },
			new string[] { "じごくのよろい", "Jigoku no Yoroi (Hell Armor)" },
			new string[] { "みずのはごろも", "Mizu no Hagoromo (Water Angel's Cloth)" },
			new string[] { "くさりかたびら", "Kusarikatabira (Chain Mail)" },
			new string[] { "たびびとのふく", "Tabibito no Fuku (Traveler Cloth)" },
			new string[] { "あぶないみずぎ", "Abunaimizugi (Risky Swimsuit)" },
			new string[] { "まほうのビキニ", "Mahou no Bikini (Magic Bikini)" },
			new string[] { "こうらのよろい", "Koura no Yoroi (Shell Armor)" },
			new string[] { "だいちのよろい", "Daichi no Yoroi (Earth Armor)" },
			new string[] { "ドラゴンメイル", "Doragonmeiru (Dragon Mail)" },
			new string[] { "やいばのよろい", "Yaiba no Yoroi (Blade Armor)" },
			new string[] { "てんしのローブ", "Tenshi no Ro-bu (Angel Robe)" },
			new string[] { "カメのこうら", "Kame no Koura (Turtle Shell)" },
			new string[] { "かわのこしまき", "Kawa no Koshimaki (Leather Hide)" },
			new string[] { "かわのドレス", "Kawa no Doresu (Leather Dress)" },
			new string[] { "ステテコパンツ", "Sutetekopantsu (Boxer Shorts)" },
			new string[] { "マジカルスカート", "Majikarusuka-to (Magical Skirt)" },
			new string[] { "ひかりのドレス", "Hikari no Doresu (Light Dress)" },
			new string[] { "ドラゴンローブ", "Doragonro-bu (Dragon Robe)" },
			new string[] { "くろしょうぞく", "Kuroshouzoku (Black Outfit)" },
			new string[] { "きぬのローブ", "Kinu no Ro-bu (Silk Robe)" },
			new string[] { "まほうのまえかけ", "Mahou no Maekake (Magic Apron)" },
			new string[] { "おしゃれなスーツ", "Osharenasu-tsu (Fashion Suit)" },
			new string[] { "パーティードレス", "Pa-ti-doresu (Party Dress)" },
			new string[] { "やみのころも", "Yami no Koromo (Dark Robe)" },
			new string[] { "ふしぎなボレロ", "Fushigi na Borero (Mystery Bolero)" },
			new string[] { "しんぴのビキニ", "Shinpi no Bikini (Mystery Bikini)" },
			new string[] { "しのびのふく", "Shinobi no Fuku (Shinobi Clothes)" },
		};
	}
}
