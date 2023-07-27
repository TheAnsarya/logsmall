using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	public class RomByteArray {
		public int Address { get; set; }

		public byte this[int offset] {
			get => Rom.Byte(Address + offset);
		}
	}
}
