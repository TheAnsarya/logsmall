using System.Globalization;
using System.Text;

namespace DQ3rAPI.Extensions;

public static class BasicExtensions {
	public static string ToHexString(this byte[] data) {
		ArgumentNullException.ThrowIfNull(data, nameof(data));

		var sb = new StringBuilder(data.Length * 2);

		foreach (var value in data) {
			_ = sb.Append(value.ToString("x2", CultureInfo.InvariantCulture));
		}

		var hex = sb.ToString();

		return hex;
	}
}
