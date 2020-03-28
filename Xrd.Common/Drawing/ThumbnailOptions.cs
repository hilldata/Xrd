using System;

namespace Xrd.Drawing {
	/// <summary>
	/// Enumeration of the thumbnail options used by the
	/// <see cref="WindowsThumbnailProvider"/> class.
	/// </summary>
	/// <remarks>
	/// H/T https://github.com/rlv-dan/ShellThumbs
	/// IShellItemImageFactory Flags: https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nf-shobjidl_core-ishellitemimagefactory-getimage?redirectedfrom=MSDN
	/// </remarks>
	[Flags]
	public enum ThumbnailOptions {
		/// <summary>
		/// Shrink the bitmap as necessary to fit, preserving the aspect ratio.
		/// </summary>
		None = 0x00,
		/// <summary>
		/// Allows stretching of the source image to create a larger thumbnail.
		/// </summary>
		BiggerSizeOk = 0x01,
		/// <summary>
		/// Return the item only if it is already in memory.
		/// </summary>
		InMemoryOnly = 0x02,
		/// <summary>
		/// Return only the file type's icon, never a thumbnail.
		/// </summary>
		IconOnly = 0x04,
		/// <summary>
		/// Return only the thumbnail, never an icon. Note that since not all
		/// items have thumbnails, this flag will cause the method to fail if 
		/// not thumbnail can be generated.
		/// </summary>
		ThumbnailOnly = 0x08,
		/// <summary>
		/// Allows access to disk, but only to retrieve a cached item.
		/// </summary>
		InCacheOnly = 0x10,
		/// <summary>
		/// Intro'ed in Win8. Crop the bitmap to a square.
		/// </summary>
		CropToSquare_Win8 = 0x20,
		/// <summary>
		/// Intro'ed in Win8. Stretch and crop to 0.7 aspect ratio
		/// </summary>
		WideThumbnail_Win8 = 0x40,
		/// <summary>
		/// Intro'ed in Win8. If returning an icon, paint a background using
		/// the associated app's registered background color.
		/// </summary>
		IconBackground_Win8 = 0x80,
		/// <summary>
		/// Intro'ed in Win8. If necessary, stretch bitmap so height and width
		/// fit the given size.
		/// </summary>
		ScaleUp_Win8 = 0x100,
	}
}
