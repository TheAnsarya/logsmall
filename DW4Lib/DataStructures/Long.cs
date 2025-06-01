using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW4Lib.DataStructures;

internal struct Long {
	public byte Low { get; set; }

	public byte High { get; set; }

	public byte Bank { get; set; }

	public int Value {
		get => (int)((Bank << 16) | (High << 8) | Low);

		set {
			Low = (byte)(value & 0xFF);
			High = (byte)((value >> 8) & 0xFF);
			Bank = (byte)((value >> 16) & 0xFF);
		}
	}
}
