namespace logsmall.Common;

public class RamByteArray {
	public int Address { get; set; }
	public Ram Ram { get; set; }

	public byte this[int offset] {
		get => Ram.Byte(Address + offset);
		set => Ram.Byte(Address + offset, value);
	}
}

