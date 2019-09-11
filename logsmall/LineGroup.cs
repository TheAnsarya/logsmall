using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	class LineGroup {
		public uint StartAddress { get; set; }
		public uint EndAddress { get => NextAddress - 1; }
		public uint NextAddress { get; set; }
		public int Length { get => (int)(NextAddress - StartAddress); }
		public int BytesBetween(LineGroup next) => (int)(next.StartAddress - NextAddress);
		
		public List<Line> Lines { get; set; }

		public LineGroup(Line line) {
			Lines = new List<Line>();
			Lines.Add(line);
			StartAddress = line.AddressRaw;
			NextAddress = (uint)(StartAddress + line.ByteLength);
		}

		public bool Add(Line line) {
			if (NextAddress == line.AddressRaw) {
				Lines.Add(line);
				NextAddress += (uint)line.ByteLength;
				return true;
			}
			return false;
		}

		public static List<LineGroup> MakeGroups(IOrderedEnumerable<Line> lines) {
			var groups = new List<LineGroup>();
			LineGroup group = null;

			foreach (var line in lines) {
				if (group == null) {
					group = new LineGroup(line);
				} else if (!group.Add(line)) {
					groups.Add(group);
					group = new LineGroup(line);
				}
			}

			if (group != null) {
				groups.Add(group);
			}

			return groups;
		}
	}
}
