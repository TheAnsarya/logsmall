using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Common {
	public class RamByteArray {
		public int Address { get; set; }
		public Ram Ram { get; set; }

		public byte this[int offset] {
			get => this.Ram.Byte(this.Address + offset);
			set => this.Ram.Byte(this.Address + offset, value);
		}
	}
}
