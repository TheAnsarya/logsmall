using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	class RamWordArray {
		public int Address;
		public Ram Ram;

		// Offset is by bytes so increase by 2 each time when iterating
		public ushort this[int offset] {
			get => this.Ram.Word(this.Address + offset);
			set => this.Ram.Word(this.Address + offset, value);
		}
	}
}
