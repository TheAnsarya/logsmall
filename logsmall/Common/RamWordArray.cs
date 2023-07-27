using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Common {
	public class RamWordArray {
		public int Address { get; set; }
		public Ram Ram { get; set; }

		// Offset is by bytes so increase by 2 each time when iterating
		public ushort this[int offset] {
			get => Ram.Word(Address + offset);
			set => Ram.Word(Address + offset, value);
		}
	}
}
