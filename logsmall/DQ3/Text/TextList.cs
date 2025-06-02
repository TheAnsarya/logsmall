using System.Collections.Generic;

namespace logsmall.DQ3.Text;

abstract class TextList {
	public virtual string TitleTag { get; }
	public virtual int StartAddress { get; }
	public virtual int EndAddress { get; }
	public virtual int RoughEndAddress { get => EndAddress; }
	public abstract string[][] Known { get; }

	private Dictionary<string, string> _toEnglishLookup;
	public Dictionary<string, string> ToEnglishLookup {
		get {
			if (_toEnglishLookup == null) {
				_toEnglishLookup = [];

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
		return ToEnglishLookup.ContainsKey(japanese) ? ToEnglishLookup[japanese] : "";
	}
}

