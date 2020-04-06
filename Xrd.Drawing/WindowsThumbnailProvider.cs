using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Xrd.Drawing {
	/// <summary>
	/// Class that exposes the Windows Shell operations to retrieve
	/// a thumbnail image or icon for a file.
	/// </summary>
	/// <remarks>
	/// H/T: https://github.com/rlv-dan/ShellThumbs/blob/master/ShellThumbs.cs
	/// </remarks>
	/// <example>
	/// ==Thumbnail or Icon==
	///		Bitmap thumbOrIcon = Xrd.Drawing.WindowsThumbnailProvider.GetThumbnailBmp(@"c:\temp\video.avi", 64, 64, ThumbnailOptions.None);
	///		
	///	==Thumbnail ONLY==
	///		Bitmap thumb = Xrd.Drawing.WindowsThumbnailProvider.GetThumbnailBmp(@"c:\temp\video.avi", 64, 64, ThumbnailOptions.ThumbnailOnly);
	///	
	/// ==Icon ONLY==
	///		Bitmap icon = Xrd.Drawing.WindowsThumbnailProvider.GetThumbnailBmp(@"c:\temp\video.avi", 64, 64, ThumbnailOptions.IconOnly);
	///		
	/// ==Binary Data for thumbnail==
	///		byte[] data = Xrd.Drawing.WindowsThumbnailProvider.GetThumbnailData(@"c:\temp\video.avi", 64, 64, ThumbnailOptions.None);
	/// </example>
	public class WindowsThumbnailProvider {
		private const string IShellItem2Guid = "7E9FB0D3-919F-4307-AB2E-9B1860310C93";

		#region Enums
		internal enum SIGDN : uint {
			NORMALDISPLAY = 0,
			PARENTRELATIVEPARSING = 0x80018001,
			PARENTRELATIVEFORADDRESSBAR = 0x8001c001,
			DESKTOPABSOLUTEPARSING = 0x80028000,
			PARENTRELATIVEEDITING = 0x80031001,
			DESKTOPABSOLUTEEDITING = 0x8004c000,
			FILESYSPATH = 0x80058000,
			URL = 0x80068000
		}

		internal enum HResult {
			Ok = 0x0000,
			False = 0x0001,
			InvalidArguments = unchecked((int)0x80070057),
			OutOfMemory = unchecked((int)0x8007000E),
			NoInterface = unchecked((int)0x80004002),
			Fail = unchecked((int)0x80004005),
			ElementNotFound = unchecked((int)0x80070490),
			TypeElementNotFound = unchecked((int)0x8002802B),
			NoObject = unchecked((int)0x800401E5),
			Win32ErrorCanceled = 1223,
			Canceled = unchecked((int)0x800704C7),
			ResourceInUse = unchecked((int)0x800700AA),
			AccessDenied = unchecked((int)0x80030005)
		}
		#endregion

		#region Structs
		[StructLayout(LayoutKind.Sequential)]
		internal struct NativeSize {
			private int width;
			private int height;

			public int Width { set { width = value; } }
			public int Height { set { height = value; } }
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct RGBQUAD {
			public byte rgbBlue;
			public byte rgbGreen;
			public byte rgbRed;
			public byte rgbReserved;
		}
		#endregion

		#region Imports
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DeleteObject(IntPtr hObject);

		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
		internal interface IShellItem {
			void BindToHandler(IntPtr pbc,
				[MarshalAs(UnmanagedType.LPStruct)]Guid bhid,
				[MarshalAs(UnmanagedType.LPStruct)]Guid riid,
				out IntPtr ppv);

			void GetParent(out IShellItem ppsi);
			void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);
			void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);
			void Compare(IShellItem psi, uint hint, out int piOrder);
		};

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SHCreateItemFromParsingName(
			[MarshalAs(UnmanagedType.LPWStr)] string path,
			IntPtr pbc,
			ref Guid riid,
			[MarshalAs(UnmanagedType.Interface)] out IShellItem shellItem);

		[ComImport]
		[Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		internal interface IShellItemImageFactory {
			[PreserveSig]
			HResult GetImage(
			[In, MarshalAs(UnmanagedType.Struct)] NativeSize size,
			[In] ThumbnailOptions flags,
			[Out] out IntPtr phbm);
		}
		#endregion

		#region Private Members
		private static IntPtr GetHBitmap(string fileName, int width, int height, ThumbnailOptions options) {
			Guid shellItem2Guid = new Guid(IShellItem2Guid);
			int retCode = SHCreateItemFromParsingName(fileName, IntPtr.Zero, ref shellItem2Guid, out IShellItem nativeShellItem);

			if (retCode != 0)
				throw Marshal.GetExceptionForHR(retCode);

			NativeSize nativeSize = new NativeSize {
				Width = width,
				Height = height
			};

			HResult hr = ((IShellItemImageFactory)nativeShellItem).GetImage(nativeSize, options, out IntPtr hBitmap);

			Marshal.ReleaseComObject(nativeShellItem);

			if (hr == HResult.Ok)
				return hBitmap;

			throw Marshal.GetExceptionForHR((int)hr);
		}

		private static Bitmap CreateAlphaBitmap(Bitmap srcBitmap, PixelFormat targetPixelFormat) {
			Bitmap result = new Bitmap(srcBitmap.Width, srcBitmap.Height, targetPixelFormat);
			Rectangle bmpBounds = new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height);
			BitmapData srcData = srcBitmap.LockBits(bmpBounds, ImageLockMode.ReadOnly, srcBitmap.PixelFormat);
			try {
				for (int y = 0; y < srcData.Height; y++) {
					for (int x = 0; x < srcData.Width; x++) {
						Color pixelColor = Color.FromArgb(Marshal.ReadInt32(srcData.Scan0, (srcData.Stride * y) + (4 * x)));
						result.SetPixel(x, y, pixelColor);
					}
				}
			} finally {
				srcBitmap.UnlockBits(srcData);
			}
			return result;
		}

		private static Bitmap GetBitmapFromHBitmap(IntPtr nativeHBitmap) {
			Bitmap bmp = Image.FromHbitmap(nativeHBitmap);
			if (Image.GetPixelFormatSize(bmp.PixelFormat) < 32)
				return bmp;

			return CreateAlphaBitmap(bmp, PixelFormat.Format32bppArgb);
		}
		#endregion

		/// <summary>
		/// Retrieve a Bitmap object containing the thumbnail image for the specified file.
		/// </summary>
		/// <param name="fileName">The path to the file.</param>
		/// <param name="height">The height of the resulting bitmap</param>
		/// <param name="width">The (optional) width of the resulting bitmap (if null, is scaled to the height)</param>
		/// <param name="options">The options to use to build the image.</param>
		/// <returns>A Bitmap containing the thumbnail image</returns>
		public static Bitmap GetThumbnailBmp(string fileName, int height, int? width = null, ThumbnailOptions options = ThumbnailOptions.None) {
			if (!File.Exists(fileName))
				return null;

			int tHeight = height;
			int tWidth;
			if (width == null) {
				using (Image img = Image.FromFile(fileName)) {
					if (img.Height != img.Width) {
						tWidth = tHeight * img.Width / img.Height;
					} else {
						tWidth = tHeight;
					}
				}
			} else
				tWidth = width.Value;

			IntPtr hBitmap = GetHBitmap(Path.GetFullPath(fileName), tWidth, tHeight, options);
			try {
				// Return a System.Drawing.Bitmap from the hBitmap
				return GetBitmapFromHBitmap(hBitmap);
			} finally {
				// delete the hBitmap to avoid memory leaks
				DeleteObject(hBitmap);
			}
		}

		/// <summary>
		/// Retrieve the bits for the thumbnail for image for the specified file.
		/// </summary>
		/// <param name="fileName">The path to the file.</param>
		/// <param name="height">The height of the resulting bitmap</param>
		/// <param name="width">The (optional) width of the resulting bitmap (if null, is scaled to the height)</param>
		/// <param name="options">The options to use to build the image.</param>
		/// <returns>The bits for the thumbnail Bitmap</returns>
		public static byte[] GetThumbnailData(string fileName, int height, int? width = null, ThumbnailOptions options = ThumbnailOptions.None) {
			var bmp = GetThumbnailBmp(fileName, height, width, options);
			var res = bmp.GetImageBinaryData();
			bmp.Dispose();
			return res;
		}
	}
}