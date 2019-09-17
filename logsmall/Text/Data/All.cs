using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Text.Data {
	class All : TextList {
		public static List<TextList> AllLists =
				new List<TextList>() {
					MonsterNames.Instance,
					Personalities.Instance,
					Weapons.Instance,
					Armor.Instance,
					Items.Instance,
					Shield.Instance,
					Helmet.Instance,
					Decoration.Instance,
					Classes.Instance,
					Spells.Instance,
					PartyMemberNames.Instance,
					DebugMenu.Instance,
					PlaceNames.Instance,
					Unknown.Instance
				};

		private All() {
			_known = AllLists.Select(x => x.Known).SelectMany(x => x).ToArray();
		}

		private static All _instance = null;
		public static All Instance {
			get {
				if (_instance == null) {
					_instance = new All();
				}

				return _instance;
			}
		}

		public override string TitleTag { get => nameof(All); }
		public override int StartAddress { get => 0xfecfb7; } // AllLists.Min(x => x.StartAddress); }
		public override int EndAddress { get => 0xfee82b; } // AllLists.Max(x => x.EndAddress); }
		public override int RoughEndAddress { get => EndAddress; }

		public override string[][] Known { get => _known; }
		private string[][] _known = null;
	}
}
