using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Text.Data {
	class Items : TextList {
		private Items() { }

		private static Items _instance = null;
		public static Items Instance {
			get {
				if (_instance == null) {
					_instance = new Items();
				}

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(Items); }

		// Entries are NOT contiguous
		public override int StartAddress { get => 0xfed140; }
		public override int EndAddress { get => 0xfed7ae; }
		public override int RoughEndAddress { get => EndAddress + 0xff; }

		// Entries have been ordered
		// Entries have been verified
		// Entries are NOT contiguous
		// Translations from: https://gamefaqs.gamespot.com/snes/564402-dragon-quest-iii-soshite-densetsu-e/faqs/7856
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
			new string[] { "あまぐものつえ", "Amagumo no Tsue (Rain Staff)" },
			new string[] { "ガイアのつるぎ", "Gaia no Tsurugi (Gaia Sword)" },

			new string[] { "せいなるまもり", "Seinarumamori (Sacred Talisman)" },
			new string[] { "いのりのゆびわ", "Inori no Yubiwa (Prayer Ring)" },

			new string[] { "さとりのしょ", "Satori no Sho (Book of Enlightenment)" },
			new string[] { "すいしょうだま", "Suishoudama (Crystal Ball)" }, // dummy entry

			new string[] { "くろこしょう", "Kurokoshou (Black Pepper)" },
			new string[] { "けんじゃのいし", "Kenja no Ishi (Sage Stone)" },
			new string[] { "ラーのかがみ", "Ra- no Kagami (Ra Mirror)" },
			new string[] { "かわきのつぼ", "Kawaki no Tsuba (Vase of Drought)" }, // in docs as "かわきのつば"
			new string[] { "やみのランプ", "Yami no Ranpu (Darkness Lamp)" },
			new string[] { "へんげのつえ", "Henge no Tsue (Change Staff)" }, // TODO: which one is right (seee what's before major block)
			new string[] { "いのちのいし", "Inochi no Ishi (Life Stone)" },
			new string[] { "きえさりそう", "Kiesarisou (Invisibility Herb)" },
			new string[] { "まほうのたま", "Mahou no Tama (Magic Ball)" },
			new string[] { "とうぞくのかぎ", "Touzoku no Kagi (Thief Key)" },
			new string[] { "まほうのかぎ", "Mahou no Kagi (Magic Key)" },
			new string[] { "さいごのかぎ", "Saigo no Kagi (Final Key)" },
			new string[] { "ゆめみるルビー", "Yumemirurubi- (Dream Ruby)" },
			new string[] { "めざめのこな", "Mezame no Kona (Wake-Up Powder)" },
			new string[] { "おうのてがみ", "Ou no Tegami (King's Letter)" }, // in docs as "王の手紙"
			new string[] { "オリハルコン", "Oriharukon (Oricon)" },
			new string[] { "ちからのたね", "Chikara no Tane (Power Seed)" },
			new string[] { "すばやさのたね", "Subayasa no Tane (Agility Seed)" },
			new string[] { "スタミナのたね", "Sutamina no Tane (Stamina Seed)" },
			new string[] { "ラックのたね", "Rakku no Tane (Luck Seed)" },
			new string[] { "かしこさのたね", "Kashikosa no Tane (Intelligence Seed)" },
			new string[] { "いのちのきのみ", "Inochi no Kinomi (Life Nut)" },
			new string[] { "やくそう", "Yakusou (Medical Herb)" },
			new string[] { "どくけしそう", "Dokukeshisou (Antidote Herb)" },
			new string[] { "せいすい", "Seisui (Holy Water)" },
			new string[] { "キメラのつばさ", "Kimera no Tsubasa (Chimera Wing)" },
			new string[] { "せかいじゅのは", "Sekaiju no Ha (Leaf of the World Tree)" },
			new string[] { "しのオルゴール", "Shi no Orugo-ru (Death Organ)" }, // dummy entry
			new string[] { "あいのおもいで", "Ai no Omoide (Love Memory)" },
			new string[] { "まんげつそう", "Mangetsusou (Full Moon Herb)" },
			new string[] { "みずでっぽう", "Mizudeppou (Water Pistol)" }, // dummy entry
			new string[] { "ふなのりのほね", "Funanori no Hone (Sailor's Bone)" },
			new string[] { "やまびこのふえ", "Yamabiko no Fue (Echo Flute)" },
			new string[] { "ようせいのふえ", "Yousei no Fue (Fairy Flute)" },
			new string[] { "ぎんのたてごと", "Gin no Tategoto (Silver Harp)" },
			new string[] { "ひかりのたま", "Hikari no Tama (Light Ball)" },
			new string[] { "どくがのこな", "Dokuga no Kona (Poisonous Powder)" },
			new string[] { "まだらくもいと", "Madarakumoito (Spider's Web)" },
			new string[] { "たいようのいし", "Taiyou no Ishi (Sun Stone)" },
			new string[] { "にじのしずく", "Niji no Shizuku (Rainbow Drop)" },
			new string[] { "シルバーオーブ", "Shiruba-o-bu (Silver Orb)" },
			new string[] { "レッドオーブ", "Reddoo-bu (Red Orb)" },
			new string[] { "イエローオーブ", "Iero-o-bu (Yellow Orb)" },
			new string[] { "パープルオーブ", "Pa-puruo-bu (Purple Orb)" },
			new string[] { "ブルーオーブ", "Buru-o-bu (Blue Orb)" },
			new string[] { "グリーンオーブ", "Guri-no-bu (Green Orb)" },
			new string[] { "ちいさなメダル", "Chiisanamedaru (Small Medal)" },
			new string[] { "ようせいのちず", "Yousei no Chizu (Fairy Map)" }, // in docs as "ようせいの地図"
			new string[] { "ふしぎなちず", "Fushigi na Chizu (Mystery Map)" }, // in docs as "ふしぎな地図"
			new string[] { "ゴールドパス", "Go-rudopasu (Gold Pass)" },

			new string[] { "ふしぎなきのみ", "Fushigi na Kinomi (Mystery Nut)" },
			new string[] { "やさしくなれるほん", "Yasashiku Nareru Hon (Promiscuous Book)" },
			new string[] { "あまえんぼうじてん", "Amaenbou Jiten (Spoiled Child Encyclopedia)" },
			new string[] { "しゅくじょへのみち", "Shukujohe no Michi (Lady Doctrine)" },
			new string[] { "あたまがさえるほん", "Atama ga Saeru Hon (Clear Head Book)" },
			new string[] { "ちからのひみつ", "Chikara no Himitsu (Power Secret)" },
			new string[] { "ごうけつのひけつ", "Gouketsu no Hiketsu (Heroic Secret)" },
			new string[] { "かいうんのほん", "Kaiun no Hon (Better Fortune Book)" },
			new string[] { "おてんばじてん", "Otenba Jiten (Tomboy Encyclopedia)" },
			new string[] { "かなしいものがたり", "Kanashii Monogatari (Sad Tale)" },
			new string[] { "ゆうき100ばい", "Yuuki 100 bai (Courage x 100)" },
			new string[] { "まけたらあかん", "Maketara Akan (Must Not Lose)" },
			new string[] { "ユーモアのほん", "Yu-moa no Hon (Humor Book)" },
			new string[] { "エッチなほん", "Ecchi na Hon (Pornography Book)" },
			new string[] { "ずるっこのほん", "Zurukko no Hon (Slyness Book)" },
			new string[] { "すごろくけん", "Sugorokuken (Dice Game Ticket)" },
		};
	}
}
