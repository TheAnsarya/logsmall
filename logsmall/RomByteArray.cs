namespace logsmall;

public class RomByteArray {
	public int Address { get; set; }

	public byte this[int offset] {
		get => Rom.Byte(Address + offset);
	}
}

