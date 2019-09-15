using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Text.Data {
	class PartyMemberNames : TextList {
		private PartyMemberNames() { }

		private static PartyMemberNames _instance = null;
		public static PartyMemberNames Instance {
			get {
				if (_instance == null) {
					_instance = new PartyMemberNames();
				}

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(PartyMemberNames); }
		public override int StartAddress { get => 0xfed7af; }
		public override int EndAddress { get => 0xfed7f3; }
		public override int RoughEndAddress { get => EndAddress; }

		// Entries have been ordered
		// Entries have been verified
		// Translations from: bparker#0210 on the RHDN discord channel
		public override string[][] Known { get => _known; }
		private static readonly string[][] _known = new string[][] {
			new string[] { "なし", "None/Nothing" },
			new string[] { "じぶん", "Me/Myself" },
			new string[] { "そのた", "Other" },
			new string[] { "ライアス", "Lias" },
			new string[] { "ネルソン", "Nelson" },
			new string[] { "エルロイ", "Elroy" },
			new string[] { "ミザリー", "misery" },
			new string[] { "サマンサ", "Samantha" },
			new string[] { "ローザ", "Rosa" },
			new string[] { "ベティ", "Betty" },
			new string[] { "ドロシー", "Dorothy" },
			new string[] { "エルシト", "Elsit (erushito)" },
			new string[] { "ニコライ", "Nikolai" },
			new string[] { "ピエール", "Pierre" },
			new string[] { "マゴット", "maggot" },
		};
	}
}
