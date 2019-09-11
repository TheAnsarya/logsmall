using logsmall.DataStructures;
using logsmall.DataStructures.SNES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Overworld {
	public class MetaTileDefinition {
		public BGMapEntry TopLeft;
		public BGMapEntry TopRight;
		public BGMapEntry BottomLeft;
		public BGMapEntry BottomRight;

		public MetaTileDefinition(ByteArrayStream source) {
			TopLeft = new BGMapEntry(source.Word());
			TopRight = new BGMapEntry(source.Word());
			BottomLeft = new BGMapEntry(source.Word());
			BottomRight = new BGMapEntry(source.Word());
		}
	}
}
