using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Text.Data {
	class MonsterNames : TextList {
		private MonsterNames() { }

		private static MonsterNames _instance = null;
		public static MonsterNames Instance {
			get {
				if (_instance == null) {
					_instance = new MonsterNames();
				}

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(MonsterNames); }
		public override int StartAddress { get => 0xfed958; }
		public override int EndAddress { get => 0xfedd75; }
		public override int RoughEndAddress { get => EndAddress; }

		// Entries have been ordered
		// Entries have been verified
		// Translations from: https://gamefaqs.gamespot.com/snes/564402-dragon-quest-iii-soshite-densetsu-e/faqs/7856
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
			new string[] { "スライム", "Suraimu (Slime)" },
			new string[] { "おおがらす", "Oogarasu (Big Bird)/Black Raven" },
			new string[] { "いっかくうさぎ", "Ikkakuusagi (Horned Rabbit)" },
			new string[] { "おおありくい", "Ooarikui (Big Anteater)/Giant Anteater" },
			new string[] { "じんめんちょう", "Jinmenchou (Masked Moth)" },
			new string[] { "フロッガー", "Furogga- (Frogger)/Froggore" },
			new string[] { "バブルスライム", "Baburusuraimu (Bubble Slime)/Babble" },
			new string[] { "まほうつかい", "Mahoutsukai (Magician)" },
			new string[] { "さそりばち", "Sasoribachi (Scorpion Wasp)" },
			new string[] { "ホイミスライム", "Hoimisuraimu (Heal Slime)/Healer" },
			new string[] { "おばけありくい", "Obakearikui (Ghost Anteater)/Demon Anteater" },
			new string[] { "アルミラージ", "Arumira-ji (Almiraj)/Spiked Hare" },
			new string[] { "ポイズントード", "Poizunto-do (Poison Toad)" },
			new string[] { "キャタピラー", "Kyatapira- (Caterpillar)" },
			new string[] { "こうもりおとこ", "Koumoriotoko (Bat Man)/Humanabat" },
			new string[] { "アニマルゾンビ", "Animaruzonbi (Animal Zombie)/Putrepup" },
			new string[] { "キラービー", "Kira-bi- (Killer Bee)" },
			new string[] { "ぐんたいガニ", "Guntaigani (Army Crab)" }, // in docs as "ぐんたいがに"
			new string[] { "ギズモ", "Gizumo (Gizmo)/Gas Cloud" },
			new string[] { "おばけキノコ", "Obakekinoko (Ghost Mushroom)/Demon Toadstool" },
			new string[] { "どくイモムシ", "Dokuimomushi (Poisonous Green Caterpillar)/Poison Silkworm" }, // in docs as "どくいもむし"
			new string[] { "デスフラッター", "Desufuratta- (Death Flutter)/Avenger Raven" },
			new string[] { "バリイドドッグ", "Bariidodoggu (Buried Dog)/Madhound" },
			new string[] { "マタンゴ", "Matango (Matango)/Deadly Toadstool" },
			new string[] { "あやしいかげ", "Ayashiikage (Suspicious Shadow)/Shadow" },
			new string[] { "バンパイア", "Banpaia (Vampire)" },
			new string[] { "ひとくいが", "Hitokuiga (Cannibal Moth)/Man-Eater Moth" },
			new string[] { "さまようよろい", "Samayouyoroi (Wandering Armor)/Rogue Knight" },
			new string[] { "キャットフライ", "Kyattofurai (Catfly)/Vampire Cat" },
			new string[] { "だいおうガマ", "Daiougama (Great King Toad)/King Froggore" },
			new string[] { "あばれザル", "Abarezaru (Wild Ape)" },
			new string[] { "わらいぶくろ", "Waraibukuro (Laugh Bag)/Trick Bag" },
			new string[] { "ミイラおとこ", "Miiraotoko (Mummy Man)" },
			new string[] { "じごくのハサミ", "Jigoku no Hasami (Hell Scissors)/Infernus Crab" },
			new string[] { "ドルイド", "Doruido (Druid)/Lumpus" },
			new string[] { "かえんムカデ", "Kaenmukade (Flame Centipede)/Flamapede" },
			new string[] { "マミー", "Mami- (Mummy)" },
			new string[] { "マージマタンゴ", "Ma-jimatango (Magi Matango)/Mage Toadstool" },
			new string[] { "ハンターフライ", "Hanta-furai (Hunter Fly)" },
			new string[] { "デスジャッカル", "Desujakkaru (Death Jackal)/Avenger Jackal" },
			new string[] { "げんじゅつし", "Genjutsushi (Magic User)/Nev" },
			new string[] { "ヒートギズモ", "Hi-togizumo (Heat Gizmo)/Heat Cloud" },
			new string[] { "アントベア", "Antobea (Antbear)/Tonguebear" },
			new string[] { "ベホマスライム", "Behomasuraimu (Healall Slime)/Curer" },
			new string[] { "マッドオックス", "Maddookkusu (Mad Ox)/Rammore" },
			new string[] { "キャットバット", "Kyattobatto (Cat Bat)/Catula" },
			new string[] { "エビルマージ", "Ebiruma-ji (Evil Mage)" },
			new string[] { "キラーエイプ", "Kira-eipu (Killer Ape)/Simiac" },
			new string[] { "ガルーダ", "Garu-da (Garuda)" },
			new string[] { "メタルスライム", "Metarusuraimu (Metal Slime)" },
			new string[] { "ゴートドン", "Go-todon (Goatodon)/Goategon" },
			new string[] { "さつじんき", "Satsujinki (Murderer)/Executioner" },
			new string[] { "ベビーサタン", "Bebi-satan (Baby Satan)/Demonite" },
			new string[] { "きめんどうし", "Kimendoushi (Devil's Mask Pupil?)/Deranger" },
			new string[] { "ひとくいばこ", "Hitokuibako (Cannibal Box)/Man-Eater Chest" },
			new string[] { "エリミネーター", "Erimine-ta- (Eliminator)" },
			new string[] { "おおくちばし", "Ookuchibashi (Big Beak)/Great Beak" },
			new string[] { "スライムつむり", "Suraimutsumuri (Slime Snail)/Slime Snaii" },
			new string[] { "スカイドラゴン", "Sukaidoragon (Sky Dragon)" },
			new string[] { "バーナバス", "Ba-nabasu (Barnabas)" },
			new string[] { "まじょ", "Majo (Witch)" },
			new string[] { "デッドペッカー", "Deddopekka- (Dead Pecker)/Avenger Beak" },
			new string[] { "じごくのよろい", "Jigoku no Yoroi (Hell Armor)/Infernus Knight" },
			new string[] { "マリンスライム", "Marinsuraimu (Marine Slime)" },
			new string[] { "しびれくらげ", "Shibirekurage (Numbing Jellyfish)/Man O' War" },
			new string[] { "マーマン", "Ma-man (Merman)/Merzon" },
			new string[] { "だいおうイカ", "Daiouika (Great King Squid)/King Squid" },
			new string[] { "ガニラス", "Ganirasu (?)/Crabus" },
			new string[] { "マーマンダイン", "Ma-mandain (Mermandine)/Merzoncian" },
			new string[] { "ヘルコンドル", "Herukondoru (Hell Condor)/Hades' Condor" },
			new string[] { "ごうけつぐま", "Gouketsuguma (Heroic Bear)/Fierce Bear" },
			new string[] { "くさったしたい", "Kusattashitai (Rotting Corpse)/Hork" },
			new string[] { "ビッグホーン", "Bigguho-n (Bighorn)" },
			new string[] { "しびれあげは", "Shibireageha (Numbing Moth)/Stingwing" },
			new string[] { "どくどくゾンビ", "Dokudokuzonbi (Poisonous Zombie)/Venom Zombie" },
			new string[] { "アカイライ", "Akairai (?)/Blue Beak" },
			new string[] { "キラーアーマー", "Kira-a-ma- (Killer Armor)/Lethal Armor" },
			new string[] { "デスストーカー", "Desusuto-ka- (Death Stalker)/Avenger" },
			new string[] { "ようがんまじん", "Youganmajin (Lava Devil)/Lava Basher" },
			new string[] { "シャーマン", "Sha-man (Shaman)/Witch Doctor" },
			new string[] { "まほうおばば", "Mahoubaba (Magic Hag)/Old Hag" }, // in docs as "まほうばば"
			new string[] { "シャドー", "Shado- (Shadow)/Terror Shadow" },
			new string[] { "ひょうがまじん", "Hyougamajin (Glacier Devil)/Glacier Basher" },
			new string[] { "キメラ", "Kimera (Chimera)/Wyvern" },
			new string[] { "コング", "Kongu (Kong)" },
			new string[] { "ガメゴン", "Gamegon (Tortoise Dragon)/Tortragon" },
			new string[] { "ごくらくちょう", "Gokurakuchou (Bird of Paradise)/Elysium Bird" },
			new string[] { "ばくだんいわ", "Bakudaniwa (Bomb Crag)" },
			new string[] { "グリズリー", "Gurizuri- (Grizzly)" },
			new string[] { "ゾンビマスター", "Zonbimasuta- (Zombie Master)/Voodoo Shaman" },
			new string[] { "ガメゴンロード", "Gamegonro-do (Tortoise Dragon Lord)/King Tortragon" },
			new string[] { "スノードラゴン", "Suno-doragon (Snow Dragon)" },
			new string[] { "トロル", "Tororu (Troll)" },
			new string[] { "フロストギズモ", "Furosutogizumo (Frost Gizmo)/Frost Cloud" },
			new string[] { "おどるほうせき", "Odoruhouseki (Dancing Jewel)" },
			new string[] { "ミニデーモン", "Minide-mon (Minidemon)" },
			new string[] { "テンタクルス", "Tentakurusu (Tentacles)" },
			new string[] { "がいこつけんし", "Gaikotsukenshi (Skeleton Fencer)/Skeleton" },
			new string[] { "ミミック", "Mimikku (Mimic)" },
			new string[] { "じごくのきし", "Jigoku no Kishi (Hell Knight)/Marauder" },
			new string[] { "ホロゴースト", "Horogo-suto (Hologhost)" },
			new string[] { "やまたのおろち", "Yamata no Orochi (8-Headed Serpent, Orochi)" },
			new string[] { "うごくせきぞう", "Ugokusekizou (Moving Stone Statue)" }, // in docs as "うごくせいきぞう"
			new string[] { "サラマンダー", "Saramanda- (Salamander)" },
			new string[] { "スライムベス", "Suraimubesu (Slimebeth)/Red Slime" },
			new string[] { "マドハンド", "Madohando (Mudhand)/Goopi" },
			new string[] { "まおうのかげ", "Maou no Kage (Devil Shadow)/Vile Shadow" },
			new string[] { "マクロベータ", "Makurobe-ta (Macrobeta?)/Voodoo Warlock" },
			new string[] { "はぐれメタル", "Haguremetaru (Separating Metal)/Metal Babble" },
			new string[] { "グール", "Gu-ru (Ghoul)" },
			new string[] { "ライオンヘッド", "Raionheddo (Lion Head)" },
			new string[] { "ボストロール", "Bosutoro-ru (Boss Troll)" },
			new string[] { "ゴールドマン", "Go-rudoman (Gold Man)/Gold Basher" },
			new string[] { "スカルゴン", "Sukarugon (Skullgon)/Scalgon" },
			new string[] { "キングマーマン", "Kinguma-man (King Merman)/King Merzon" },
			new string[] { "クラーゴン", "Kura-gon (Kragon)/Kragacles" },
			new string[] { "ダースリカント", "Da-surikanto (Darth Bear)" },
			new string[] { "だいまじん", "Daimajin (Pedestal Devil)/Granite Titan" },
			new string[] { "ラゴンヌ", "Ragonnu (?)/Leona" },
			new string[] { "アークマージ", "A-kuma-ji (Archmage)" },
			new string[] { "メイジキメラ", "Meijikimera (Mage Chimera)/Magiwyvern" },
			new string[] { "サタンパピー", "Satanpapi- (Satan Pappy)/Winged Demon" },
			new string[] { "ヒドラ", "Hidora (Hydra)" },
			new string[] {  "トロルキング", "Toro-rukingu (Troll King)" }, // in docs as "トロールキング"
			new string[] { "ドラゴン", "Doragon (Dragon)/Green Dragon" },
			new string[] { "バルログ", "Barurogu (Balrog)/Barog" },
			new string[] { "ドラゴンゾンビ", "Doragonzonbi (Dragon Zombie)/Putregon" },
			new string[] { "マントゴーア", "Mantogo-a (Mantigore)/Lionroar" },
			new string[] { "ソードイド", "So-doido (Swordoid)" },
			new string[] { "キングヒドラ", "Kinguhidora (King Hydra)" },
			new string[] { "バラモスブロス", "Baramosuburosu (Baramos Buros)/Baramos Bomus" },
			new string[] { "バラモスゾンビ", "Baramosuzonbi (Baramos Zombie)/Baramos Gonus" },
			new string[] { "バラモス", "Baramosu (Baramos)" },
			new string[] { "ゾーマ", "Zo-ma (Zoma)" },
			new string[] { "オルテガ", "Orutega (Ortega)" }, // dummy entry
			new string[] { "カンダタ", "Kandata (Kandata)/Kandar" },
			new string[] { "カンダタこぶん", "Kandatakobun (Kandata Henchman)/Kandar's Henchman" },
			new string[] { "けいけんち", "Keikenchi (Experience)" }, // dummy entry
			new string[] { "おかね", "Okane (Money)" }, // dummy entry
			new string[] { "せんたくバカ", "Sentakubaka (Generally Stupid)" }, // dummy entry
			new string[] { "せんたくにんげん", "Sentakuningen (Generally Human)" }, // dummy entry
			new string[] { "せんたくかみ", "Sentakukami (Generally God)" }, // dummy entry
			new string[] { "せんたく0", "Sentaku 0 (Generally 0)" }, // dummy entry
			new string[] { "せんたく1", "Sentaku 1 (Generally 1)" }, // dummy entry
			new string[] { "せんたく2", "Sentaku 2 (Generally 2)" }, // dummy entry
			new string[] { "せんたく3", "Sentaku 3 (Generally 3)" }, // dummy entry
			new string[] { "ほうおう", "Houou (Phoenix)" },
			new string[] { "てんのもんばん", "Ten no Monban (Heaven's Gate Watcher)" },
			new string[] { "メタルキメラ", "Metarukimera (Metal Chimera)" },
			new string[] { "デビルウィザード", "Debiruuiza-do (Devil Wizard)" },
			new string[] { "キラークラブ", "Kira-kurabu (Killer Crab)" },
			new string[] { "ダークトロル", "Da-kutororu (Dark Troll)" },
			new string[] { "デーモンソード", "De-monso-do (Demon Sword)" },
			new string[] { "バラモスエビル", "Baramosuebiru (Baramos Evil)" },
			new string[] { "しんりゅう", "Shinryuu (God Dragon)" },
		};
	}
}
