using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Xrd.Drawing {
	/// <summary>
	/// Class containing extension methods to easily convert 
	/// <see cref="Image"/>s to/from binary data (byte arrays) 
	/// and to create downsized thumbnails.
	/// </summary>
	public static class ImageExtensions {
		// Share the memory stream across functions to prevent early disposal.
		private static MemoryStream ms;
		/// <summary>
		/// Convert binary data (byte array) into <see cref="Image"/>
		/// using a MemoryStream
		/// </summary>
		/// <param name="imageData">The image's binary data.</param>
		/// <returns>An <see cref="Image"/></returns>
		public static Image CreateImage(this byte[] imageData) {
			if (imageData.IsNullOrEmpty())
				throw new ArgumentNullException(nameof(imageData));

			ms = new MemoryStream(imageData) {
				Position = 0
			};
			return Image.FromStream(ms);
		}

		/// <summary>
		/// Get the binary data (byte array) for the specified 
		/// <see cref="Image"/>
		/// </summary>
		/// <param name="image">The image to read into binary data.</param>
		/// <returns>The binary data that comprises the <see cref="Image"/></returns>
		public static byte[] GetImageBinaryData(this Image image) {
			ms = new MemoryStream();
			image.Save(ms, ImageFormat.Jpeg);
			ms.Position = 0;
			return ms.ToArray();
		}

		// Image.GetThumbnailImageAbort delegate.
		private static bool ThumbnailCallback() => false;

		/// <summary>
		/// Generate a thumbnail from an <see cref="Image"/> and return
		/// it as an <see cref="Image"/>
		/// </summary>
		/// <param name="source">
		/// The source image to downsize into a thumbnail
		/// </param>
		/// <param name="maxDim">
		/// The maximum dimension (height or width) for the resulting thumbnail.
		/// </param>
		/// <returns>The resulting thumbnail as an <see cref="Image"/></returns>
		public static Image CreateThumbnail(this Image source, int maxDim = 120) {
			// Initialize thumbnail dimensions to the maxDim parameter.
			int tH = maxDim;
			int tW = maxDim;

			// If the source image is not square, scale the shorter 
			// thumbnail dimension to preserve the aspect ratio.
			if(source.Height != source.Width) {
				if(source.Height > source.Width) {
					tW = tW * source.Width / source.Height;
				}else {
					tH = tH * source.Height / source.Width;
				}
			}

			Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
			return source.GetThumbnailImage(tW, tH, callback, IntPtr.Zero);
		}

		/// <summary>
		/// Generate a thumbnial from an image's binary data and return
		/// it as an <see cref="Image"/>
		/// </summary>
		/// <param name="imageData">The binary data for the image.</param>
		/// <param name="maxDim">
		/// The maximum dimension (height or width) for the resulting thumbnail.
		/// </param>
		/// <returns>The resulting thumbnail as an <see cref="Image"/></returns>
		public static Image CreateThumbnail(this byte[] imageData, int maxDim = 120) =>
			imageData.CreateImage().CreateThumbnail(maxDim);

		/// <summary>
		/// Generate a thumbnail from an image's binary data and return
		/// the resulting thumbnail's binary data (byte array).
		/// </summary>
		/// <param name="imageData">The binary data for the image.</param>
		/// <param name="maxDim">
		/// The maximum dimension (height or width) for the resulting thumbnail
		/// </param>
		/// <returns>The result thumbnail's binary data (byte array).</returns>
		public static byte[] GetThumbnailData(this byte[] imageData, int maxDim) =>
			imageData.CreateImage().CreateThumbnail(maxDim).GetImageBinaryData();
	}
}
