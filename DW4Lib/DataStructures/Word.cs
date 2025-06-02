namespace DW4Lib.DataStructures;

internal struct Word {
	public byte Low { get; set; }

	public byte High { get; set; }

	public ushort Value {
		readonly get => (ushort)((High << 8) | Low);

		set {
			Low = (byte)(value & 0xFF);
			High = (byte)((value >> 8) & 0xFF);
		}
	}
}
