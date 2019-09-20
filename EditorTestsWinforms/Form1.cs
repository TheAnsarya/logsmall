using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorTestsWinforms {
	public partial class Form1 : Form {

		private Bitmap mapBitmap = new Bitmap(4096, 4096);
		public Form1() {
			InitializeComponent();

			mapBox.Image = mapBitmap;
			mapBox.Height = mapBitmap.Height;
			mapBox.Width = mapBitmap.Width;

			DrawMapImage();
		}

		public void DrawMapImage() {
			var fullmap = logsmall.DQ3.OverworldMap2.GetTilemapData();

			var sources =
				Enumerable.Range(0, 256)
					.ToDictionary(
						x => x,
						x => new Rectangle((x % 16) * 16, (x / 16) * 16, 16, 16)
					);

			using (var tiles = Image.FromFile(@"C:\working\dq3\overworld\dq3-tiles.png")) {
				using (var g = Graphics.FromImage(mapBitmap)) {

					for (var row = 0; row < 256; row++) {
						for (var column = 0; column < 256; column++) {
							g.DrawImage(tiles, new Rectangle(column * 16, row * 16, 16, 16), sources[fullmap[row, column]], GraphicsUnit.Pixel);
						}
					}
				}
			}
		}

	}
}
