using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3.Text {
	abstract class TextList {
		public virtual string TitleTag { get; }
		public virtual int StartAddress { get; }
		public virtual int EndAddress { get; }
		public virtual int RoughEndAddress { get => EndAddress; }
		public abstract string[][] Known { get; }

		private Dictionary<string, string> _toEnglishLookup = null;
		public Dictionary<string, string> ToEnglishLookup {
			get {
				if (_toEnglishLookup == null) {
					_toEnglishLookup = new Dictionary<string, string>();

					foreach (var term in Known) {
						if (!_toEnglishLookup.ContainsKey(term[0])) {
							_toEnglishLookup.Add(term[0], term[1]);
						}
					}
				}

				return _toEnglishLookup;
			}
		}

		public string ToEnglish(string japanese) {
			if (ToEnglishLookup.ContainsKey(japanese)) {
				return ToEnglishLookup[japanese];
			}

			return "";
		}
	}
}
