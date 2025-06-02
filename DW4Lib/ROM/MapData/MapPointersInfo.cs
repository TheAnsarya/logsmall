using DW4Lib.DataStructures;

namespace DW4Lib.ROM.MapData;

// $B121-$B4AE
internal class MapPointersInfo {
	public byte TileSetID { get; set; }

	public Word AddressOffset { get; set; }

	//public byte Bank { get; set; }

	//public int Value => AddressOffset.Value + (Bank << 16);

	//public Long Address => new() {
	//	Low = AddressOffset.Low,
	//	High = AddressOffset.High,
	//	Bank = Bank
	//};
}
