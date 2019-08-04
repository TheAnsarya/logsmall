using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	class RamWordArray {
		public int Address;
		public Ram Ram;

		public ushort this[int offset] {
			get => this.Ram.Word(this.Address + offset);
		}
	}
}
