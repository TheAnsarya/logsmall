using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3.Text.Data {
	class PlaceNames : TextList {
		private PlaceNames() { }

		private static PlaceNames _instance;
		public static PlaceNames Instance {
			get {
				_instance ??= new PlaceNames();

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(PlaceNames); }
		public override int StartAddress { get => 0xfee3af; }
		public override int EndAddress { get => 0xfee5dd; }
		public override int RoughEndAddress { get => EndAddress + 0x200; }

		// TODO: Entries have NOT been ordered
		// TODO: Entries have NOT been verified
		// Translations from: https://gamefaqs.gamespot.com/snes/564402-dragon-quest-iii-soshite-densetsu-e/faqs/7856
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
new string[] { "ポルトガ", "Porutoga (Portoga)" },
// "バーク"
new string[] { "ノアニール", "Noani-ru (Noaniel)" },
new string[] { "アッサラーム", "Assara-mu (Assaram)" },
new string[] { "バハラタ", "Baharata (Baharata)" },
new string[] { "ランシール", "Ranshi-ru (Lancel)" },
new string[] { "メルキド", "Merukido (Merkido)" },
new string[] { "リムルダール", "Rimuruda-ru (Rimuldar)" },
new string[] { "ドムドーラ", "Domudo-ra (Domdora)" },
new string[] { "イシス", "Ishisu (Isis)" },
new string[] { "ルザミ", "Ruzami (Luzami)" },
new string[] { "カザーブ", "Kaza-bu (Kazab)" },
new string[] { "エルフのかくれざと", "Erufu no Kakurezato (Isolated Elf Village)" },
new string[] { "テドン", "Tedon (Tedon)" },
new string[] { "ムオル", "Muoru (Muor)" },
new string[] { "ジパング", "Jipangu (Jipang)" },
new string[] { "かいぞくのいえ", "Kaizoku no Ie (Pirate's House)" },
new string[] { "スー", "Su- (Sioux)" },
new string[] { "へんげろうじんのいえ", "Hengeroujin no Ie (Old Changing Man's House)" },
new string[] { "マイラ", "Maira (Maira)" },
new string[] { "アリアハン", "Ariahan (Aliahan)" },
new string[] { "エジンベア", "Ejinbea (Edinbear)" },
// second copy of { "イシス", "Ishisu (Isis)" } is here
new string[] { "バラモスじょう", "Baramosujou (Baramos' Castle)" },
new string[] { "りゅうのじょおうのしろ", "Ryuu no Jouou no Shiro (Dragon Queen's Castle)" },
new string[] { "ダーマ", "Da-ma (Dharma)" },
new string[] { "サマンオサ", "Samanosa (Samanosa)" },
new string[] { "ラダトーム", "Radato-mu (Ladutorm)" },
new string[] { "ゾーマのしろ", "Zo-ma no Shiro (Zoma's Castle)" },
new string[] { "ちいさなほこら", "Chiisanahokora (Small Shrine)" },
new string[] { "ポルトガのとうだい", "Porutoga no Toudai (Portoga Lighthouse)" },
new string[] { "ほこらのろうごく", "Hokora no Rougoku (Shrine Prison)" },
new string[] { "さばくのほこら", "Sabaku no Hokora (Desert Shrine)" },
new string[] { "たびびとのほこら", "Tabibito no Hokoro (Traveler's Shrine)" },
new string[] { "ネクロゴンドのほこら", "Nekurogondo no Hokora (Necrogond Shrine)" },
new string[] { "たびびとのやどや", "Tabibito no Yadoya (Traveler's Inn)" },
new string[] { "たびびとのきょうかい", "Tabibito no Kyoukai (Traveler's Church)" },
new string[] { "たびのとびらのほこら", "Tabi no Tobira no Hokora (Travel Door Shrine)" },
new string[] { "ガライのいえ", "Garai no Ie (Garai's House)" },
new string[] { "ふなつきば", "Funatsukiba (Harbor)" },
new string[] { "せいれいのほこら", "Seirei no Hokora (Spirit Shrine)" },
new string[] { "せいなるほこら", "Seinaru no Hokora (Sacred Shrine)" }, // in docs as "せいなるのほこら"
new string[] { "いざないのほこら", "Izanai no Hokora (Inviting Shrine)" },
new string[] { "ロマリアのせきしょ", "Romaria no Sekishou (Romaly Checking Station)" },
new string[] { "あさせのほこら", "Asase no Hokora (Shoal Shrine)" },
new string[] { "ホビットのほこら", "Hobitto no Hokora (Hobbit's Shrine)" },
new string[] { "せいれいのいずみ", "Seirei no Izumi (Spirit Spring)" },
new string[] { "オリビアのみさき", "Oribia no Misaki (Olivia's Cape)" },
new string[] { "ふしちょうのさいだん", "Fushichou no Saidan (Phoenix Bird's Altar)" },
new string[] { "みさきのどうくつ", "Misaki no Doukutsu (Promontory Cave)" },
new string[] { "そうげんのほこら", "Sougen no Hokora (Grassland Shrine)" },
new string[] { "いざないのどうくつ", "Izanai no Doukutsu (Inviting Cave)" },
new string[] { "ちていのみずうみ", "Chitei no Mizuumi (Lake Depths)" },
new string[] { "ノルドのどうくつ", "Norudo no Doukutsu (Norud's Cave)" },
new string[] { "ネクロゴンドのどうくつ", "Nekurogondo no Doukutsu (Necrogond Cave)" },
new string[] { "ひとさらいのアジト", "Hitosarai no Ajito (Kidnappers' Hideout)" },
new string[] { "おろちのどうくつ", "Orochi no Doukutsu (Orochi Cave)" },
new string[] { "ちきゅうのへそ", "Chikyuu no Hezo (Navel of the Earth)" }, // in docs as "ちきゅうのへぞ"
new string[] { "ラーのどうくつ", "Ra- no Doukutsu (Ra Cave)" },
new string[] { "まおうのツメあと", "Maou no Tsumeato (Devil's Nail Mark)" },
new string[] { "ぬまちのどうくつ", "Numachi no Doukutsu (Marshland Cave)" },
new string[] { "いわやまのどうくつ", "Iwayama no Doukutsu (Rocky Mountain Cave)" },
//"かくしダンジョン"
new string[] { "ナジミのとう", "Najimi no Tou (Najimi Tower)" },
new string[] { "ガルナのとう", "Garuna no Tou (Garuna Tower)" },
new string[] { "アープのとう", "A-pu no Tou (Arp Tower)" },
new string[] { "シャンパーニのとう", "Shanpa-ni no Tou (Champagne Tower)" },
new string[] { "ルビスのとう", "Rubisu no Tou (Rubiss Tower)" },
//"T06"
new string[] { "ゼニスのしろ", "Zenisu no Shiro (Zenith Castle)" },
new string[] { "ゆうれいせん", "Yuureisen (Phantom Ship)" },
new string[] { "ギアガのおおあな", "Giaga no Ooana (Great Pit of Giaga)" },
new string[] { "ピラミッド", "Piramiddo (Pyramid)" },



new string[] { "レーブ", "Re-bu (Reeve)" },
new string[] { "ロマリア", "Romaria (Romaly)" },
new string[] { "グリンラッド", "Gurinraddo (Greenlad)" },
new string[] { "レイアムランド", "Reiamrando (Leiamland)" },
new string[] { "天界のどうくつ", "Tenkai no Doukutsu (Sky World Cave)" },
new string[] { "天界のとう", "Tenkai no Tou (Sky World Tower)" },

		};
	}
}
