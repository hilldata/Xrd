using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Xrd.Drawing {
	/// <summary>
	/// Class containing the parameters for drawing text to a fixed bitmap
	/// </summary>
	public class TextToBmpOptions {
		/// <summary>
		/// Default constructor
		/// </summary>
		public TextToBmpOptions() { }

		#region Properties
		/// <summary>
		/// The name of the font to use when drawing the text
		/// </summary>
		public string FontName { get; set; } = Text.Mdl2Assets.FONT_NAME;

		/// <summary>
		/// The width of the resulting Bitmap
		/// </summary>
		public int Width { get; set; } = 96;

		/// <summary>
		/// The height of the resulting Bitmap
		/// </summary>
		public int Height { get; set; } = 96;

		/// <summary>
		/// The FontStyle to apply when drawing the text.
		/// </summary>
		public FontStyle FontStyle { get; set; } = FontStyle.Regular;

		/// <summary>
		/// The background color to draw on the Bitmap.
		/// </summary>
		public Color Background { get; set; } = Color.Transparent;

		/// <summary>
		/// The color to draw the text with.
		/// </summary>
		public Color Foreground { get; set; } = Color.Black;
		#endregion

		/// <summary>
		/// Default options for a 96x96 Bitmap
		/// </summary>
		public static TextToBmpOptions ExtraLarge => new TextToBmpOptions();
		/// <summary>
		/// Default options for a 64x64 Bitmap
		/// </summary>
		public static TextToBmpOptions Large => new TextToBmpOptions() { Width = 64, Height = 64 };
		/// <summary>
		/// Default options for a 32x32 Bitmap.
		/// </summary>
		public static TextToBmpOptions Medium => new TextToBmpOptions() { Width = 32, Height = 32 };
		/// <summary>
		/// Default options for a 16x16 Bitmap.
		/// </summary>
		public static TextToBmpOptions Small => new TextToBmpOptions() { Width = 16, Height = 16 };
	}
}
