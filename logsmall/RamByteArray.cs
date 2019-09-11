using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	public class RamByteArray {
		public int Address;
		public Ram Ram;

		public byte this[int offset] {
			get => this.Ram.Byte(this.Address + offset);
			set => this.Ram.Byte(this.Address + offset, value);
		}
	}
}
