using DQ3SFC.DataStructures;
using DQ3SFC.DataStructures.SNES.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3SFC.Overworld {
	public class MetaTileDefinition {
		public BGMapEntry TopLeft { get; set; }
		public BGMapEntry TopRight { get; set; }
		public BGMapEntry BottomLeft { get; set; }
		public BGMapEntry BottomRight { get; set; }

		public MetaTileDefinition(ByteArrayStream source) {
			if (source == null) {
				throw new ArgumentNullException(nameof(source));
			}

			TopLeft = new BGMapEntry(source.Word());
			TopRight = new BGMapEntry(source.Word());
			BottomLeft = new BGMapEntry(source.Word());
			BottomRight = new BGMapEntry(source.Word());
		}
	}
}
