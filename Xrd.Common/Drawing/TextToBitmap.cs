using System;
using System.Drawing;

using Xrd.Text;

namespace Xrd.Drawing {
	/// <summary>
	/// Class providing methods to draw text to <see cref="Bitmap"/>s
	/// </summary>
	public static class TextToBitmap {
		private static Color transparent = Color.Transparent;
		private static Color black = Color.Black;
		public const float MinimumFontSize = 5;

		/// <summary>
		/// Draw the specified text to a variable-sized Bitmap
		/// (result will be sized to the specified text/font)
		/// </summary>
		/// <param name="text">The text to draw.</param>
		/// <param name="fontName">The name of the font to use.</param>
		/// <param name="fontSize">The size of the font to use</param>
		/// <param name="fontStyle">the <paramref name="fontStyle"/> to use.</param>
		/// <param name="bgColor">The background <see cref="Color"/> to draw to the result</param>
		/// <param name="fgColor">The foreground <see cref="Color"/> to draw the text with.</param>
		/// <returns>A Bitmap</returns>
		/// <remarks>Can be used as an extension method.</remarks>
		public static Bitmap DrawTextToVariableBitmap(
			this string text,
			string fontName,
			float fontSize,
			FontStyle fontStyle = FontStyle.Regular,
			Color? bgColor = null,
			Color? fgColor = null) {
			if (!text.HasValue())
				throw new ArgumentNullException(nameof(text));

			if (!fontName.HasValue())
				fontName = Mdl2Assets.FONT_NAME;

			if (fontSize < MinimumFontSize)
				throw new ArgumentOutOfRangeException(nameof(fontSize));

			// Temps used to calculate the size of the final result.
			using (Bitmap bmp = new Bitmap(1, 1)) {
				using (Graphics temp = Graphics.FromImage(bmp)) {
					using (Font font = new Font(fontName, fontSize, fontStyle)) {

						// Measure the text
						SizeF size = temp.MeasureString(text, font);

						// Draw the result
						// Do NOT "using" the result, or the bitmap will be disposed when the method returns.
						Bitmap res = new Bitmap((int)size.Width, (int)size.Height);
						using (Graphics final = Graphics.FromImage(res)) {
							if (bgColor.HasValue && bgColor != transparent)
								final.FillRectangle(new SolidBrush(bgColor.Value), 0, 0, res.Width, res.Height);

							final.DrawString(text, font, new SolidBrush(fgColor ?? black), 0, 0);
							final.Flush();
							return res;
						}
					}
				}
			}
		}

		/// <summary>
		/// Draw the specified text to a fixed-sized Bitmap
		/// (text/font will be sized to fit the specified dimensions)
		/// </summary>
		/// <param name="text">The text to draw.</param>
		/// <param name="fontName">The name of the font to use.</param>
		/// <param name="width">The width of the resulting <see cref="Bitmap"/></param>
		/// <param name="height">The height of the resulting <see cref="Bitmap"/></param>
		/// <param name="fontStyle">the <paramref name="fontStyle"/> to use.</param>
		/// <param name="bgColor">The background <see cref="Color"/> to draw to the result</param>
		/// <param name="fgColor">The foreground <see cref="Color"/> to draw the text with.</param>
		/// <returns>A Bitmap</returns>
		/// <remarks>Can be used as an extension method.</remarks>
		public static Bitmap DrawTextToFixedBitmap(
			string text,
			string fontName,
			int width = 64,
			int height = 64,
			FontStyle fontStyle = FontStyle.Regular,
			Color? bgColor = null,
			Color? fgColor = null) {
			if (!text.HasValue())
				throw new ArgumentNullException(nameof(text));

			if (!fontName.HasValue())
				fontName = Text.Mdl2Assets.FONT_NAME;

			if (width < MinimumFontSize)
				throw new ArgumentOutOfRangeException(nameof(width));
			if (height < MinimumFontSize)
				throw new ArgumentOutOfRangeException(nameof(height));

			// Do NOT "using" the result, or the bitmap will be disposed when the method returns.
			Bitmap res = new Bitmap(width, height);
			using (Graphics gr = Graphics.FromImage(res)) {

				if (bgColor.HasValue && bgColor != transparent)
					gr.FillRectangle(new SolidBrush(bgColor.Value), 0, 0, width, height);

				int fontSize = height;
				bool tooBig = true;
				while (tooBig && fontSize > MinimumFontSize) {
					if (fontSize < MinimumFontSize)
						throw new Exception($"The text will not fit in the size specified [{width} x {height}].");
					using (Font font = new Font(fontName, fontSize, fontStyle)) {
						SizeF size = gr.MeasureString(text, font);
						tooBig = size.Width > width || size.Height > height;
						if (!tooBig) {
							// Center text
							float fW = (width - size.Width) > 0
								? (width - size.Width) / 2
								: 0;
							float fH = (height - size.Height) > 0
								? (height - size.Height) / 2
								: 0;
							gr.DrawString(text, font, new SolidBrush(fgColor ?? black), fW, fH);
							gr.Flush();
							return res;
						} else {
							fontSize--;
						}
					}
				}
			}
			throw new Exception($"The text will not fit in the size specified [{width} x {height}].");
		}

		/// <summary>
		/// Draw the specified text to a fixed-sized Bitmap
		/// (text/font will be sized to fit the specified dimensions)
		/// </summary>
		/// <param name="text">The text to draw.</param>
		/// <param name="options">The <see cref="TextToBmpOptions"/> options value containing the parameters for the output.</param>
		/// <returns>A Bitmap</returns>
		/// <remarks>Can be used as an extension method.</remarks>
		public static Bitmap DrawTextToFixedBitmap(this string text, TextToBmpOptions options) {
			if (options == default)
				options = new TextToBmpOptions();
			return DrawTextToFixedBitmap(text, options.FontName, options.Width, options.Height, options.FontStyle, options.Background, options.Foreground);
		}

		/// <summary>
		/// Draw the specified text to a fixed-sized Bitmap
		/// (text/font will be sized to fit the specified dimensions)
		/// </summary>
		/// <param name="text">The text to draw.</param>
		/// <param name="options">The <see cref="TextToBmpOptions"/> options value containing the parameters for the output.</param>
		/// <returns>A Bitmap</returns>
		/// <remarks>Can be used as an extension method.</remarks>
		public static Bitmap DrawMdl2ToBitmap(this Mdl2AssetsEnum value, TextToBmpOptions options) {
			// Make sure the MDL2 Assets font is used.
			if (!options.FontName.Equals(Mdl2Assets.FONT_NAME))
				options.FontName = Mdl2Assets.FONT_NAME;
			return value.ToChar().ToString().DrawTextToFixedBitmap(options);
		}
	}
}