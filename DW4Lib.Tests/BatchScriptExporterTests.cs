namespace DW4Lib.Tests;

using DW4Lib.Converters;
using Xunit;

/// <summary>
/// Tests for BatchScriptExporter.
/// </summary>
public class BatchScriptExporterTests {
	[Fact]
	public void ExportConfig_HasDefaultValues() {
		var config = new BatchScriptExporter.ExportConfig();

		Assert.True(config.IncludeRawBytes);
		Assert.True(config.IncludeDQ3rBytes);
		Assert.True(config.SeparateFiles);
		Assert.Equal("json", config.Format);
	}

	[Fact]
	public void ExportConfig_CanSetAllProperties() {
		var config = new BatchScriptExporter.ExportConfig {
			IncludeRawBytes = false,
			IncludeDQ3rBytes = false,
			SeparateFiles = false,
			Format = "csv",
		};

		Assert.False(config.IncludeRawBytes);
		Assert.False(config.IncludeDQ3rBytes);
		Assert.False(config.SeparateFiles);
		Assert.Equal("csv", config.Format);
	}

	[Fact]
	public void ExportedEntry_CanStoreAllData() {
		var entry = new BatchScriptExporter.ExportedEntry {
			Index = 5,
			RomOffset = 0x1C010,
			RawHex = "252627",
			OriginalText = "ABC",
			DQ3rText = "ABC",
			DQ3rHex = "020B020C020D",
		};

		Assert.Equal(5, entry.Index);
		Assert.Equal(0x1C010, entry.RomOffset);
		Assert.Equal("252627", entry.RawHex);
		Assert.Equal("ABC", entry.OriginalText);
		Assert.Equal("ABC", entry.DQ3rText);
		Assert.Equal("020B020C020D", entry.DQ3rHex);
	}

	[Fact]
	public void ExportedTable_CanStoreEntries() {
		var table = new BatchScriptExporter.ExportedTable {
			Name = "TestTable",
			Bank = 0x0C,
			EntryCount = 2,
			Entries = [
				new() { Index = 0, OriginalText = "First" },
				new() { Index = 1, OriginalText = "Second" },
			],
		};

		Assert.Equal("TestTable", table.Name);
		Assert.Equal(0x0C, table.Bank);
		Assert.Equal(2, table.EntryCount);
		Assert.Equal(2, table.Entries.Count);
	}

	[Fact]
	public void ExportResult_CanStoreMultipleTables() {
		var result = new BatchScriptExporter.ExportResult();
		result.Tables.Add(new() { Name = "Monsters" });
		result.Tables.Add(new() { Name = "Items" });

		Assert.Equal(2, result.Tables.Count);
	}

	[Fact]
	public void ExportResult_HasTimestamp() {
		var result = new BatchScriptExporter.ExportResult {
			SourceRom = "Test ROM",
			ExportedAt = DateTime.UtcNow,
		};

		Assert.Equal("Test ROM", result.SourceRom);
		Assert.True(result.ExportedAt > DateTime.MinValue);
	}
}
