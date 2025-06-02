using DW4Lib.DataStructures;

namespace DW4Lib.ROM.MapData;

internal class MapPointers {
	const int NumberOfMaps = 73;

	public Word[] Data { get; set; } = new Word[NumberOfMaps];

	//$B08D-$B11F
	public MapPointers(Span<Word> data) {
		if (data.Length != NumberOfMaps) {
			throw new ArgumentException($"Expected {NumberOfMaps} entries, but got {data.Length}.", nameof(data));
		}

		for (int i = 0; i < NumberOfMaps; i++) {
			Data[i] = data[i];
		}
	}

	public Word this[int index, int subIndex] {
		get {
			return index is < 0 or >= NumberOfMaps
				? throw new ArgumentOutOfRangeException(nameof(index), $"Index {index} is out of range.")
				: Data[index + (subIndex * 3)];
		}
		set {
			if (index is < 0 or >= NumberOfMaps) {
				throw new ArgumentOutOfRangeException(nameof(index), $"Index {index} is out of range.");
			}

			Data[index + (subIndex * 3)] = value;
		}
	}
}
