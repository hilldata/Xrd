using System;
using System.IO;
using System.Linq;

namespace Xrd.Files {
	/// <summary>
	/// Extensions methods for file operations.
	/// </summary>
	public static class FileXMethods {
		#region Version to String
		/// <summary>
		/// Convert an integer version to a string.
		/// </summary>
		/// <param name="version">The current version</param>
		/// <param name="padLength">The minimum number of characters in the output (will pad with leading zeros)</param>
		/// <returns>A string representation of the version.</returns>
		public static string VersionToString(this int version, byte padLength = 4) {
			if (version == 0)
				return "orig";
			string fmt = $"D{padLength}";
			return version.ToString(fmt);
		}

		/// <summary>
		/// Convert an integer version to a string.
		/// </summary>
		/// <param name="version">The current version</param>
		/// <param name="padLength">The minimum number of characters in the output (will pad with leading zeros)</param>
		/// <returns>A string representation of the version.</returns>
		public static string VersionToString(this int? version, byte padLength = 4) {
			if (version == null)
				return "????";
			else if (version == 0)
				return "orig";
			string fmt = $"D{padLength}";
			return version.Value.ToString(fmt);
		}
		#endregion

		#region MIME type checks
		public const string AUDIO_MIME = "audio";
		public const string IMAGE_MIME = "image";
		public const string VIDEO_MIME = "video";

		private static string fixMimeType(this string mimeType) {
			if (mimeType.Contains("/"))
				return mimeType.Trim().ToLower();
			string e = mimeType.FixFileExtension();
			var qry = from d
					  in MimeType.DefaultMimeTypes
					  where d.Ext == e
					  select d.MIME;
			if (qry.Any())
				return qry.First().ToLower();
			return mimeType;
		}
		/// <summary>
		/// Determine if the specified mime/media type represents an audio file
		/// </summary>
		/// <param name="mimeType">The mime/media type string to check.</param>
		/// <returns>Boolean indicating whether or not the mime/media type represents an audio file.</returns>
		public static bool IsFileAudio(this string mimeType) {
			if (string.IsNullOrWhiteSpace(mimeType))
				return false;
			return mimeType.fixMimeType().StartsWith(AUDIO_MIME);
		}

		/// <summary>
		/// Determine if the specified mime/media type represents a viedo file
		/// </summary>
		/// <param name="mimeType">The mime/media type string to check.</param>
		/// <returns>Boolean indicating whether or not the mime/media type represents a video file.</returns>
		public static bool IsFileVideo(this string mimeType) {
			if (string.IsNullOrWhiteSpace(mimeType))
				return false;
			return mimeType.fixMimeType().StartsWith(VIDEO_MIME);
		}

		/// <summary>
		/// Determine if the specified mime/media type represents an image file
		/// </summary>
		/// <param name="mimeType">The mime/media type string to check.</param>
		/// <returns>Boolean indicating whether or not the mime/media type represents an image file.</returns>
		public static bool IsFileImage(this string mimeType) {
			if (string.IsNullOrWhiteSpace(mimeType))
				return false;
			return mimeType.fixMimeType().StartsWith(IMAGE_MIME);
		}

		/// <summary>
		/// Determine if the specified mime/media type represents the specified file type
		/// </summary>
		/// <param name="mimeType">The mime/media type string to check</param>
		/// <param name="isAudioRequired">Does the file have to be an audio format?</param>
		/// <param name="isImageRequired">Does the file have to be an image format?</param>
		/// <param name="isVideoRequired">Does the file have to be a video format?</param>
		/// <returns>Boolean indicating whether or not the mime/media type is valid per the requirements.</returns>
		public static bool ValidateMimeType(this string mimeType, bool isAudioRequired = false, bool isImageRequired = false, bool isVideoRequired = false) {
			if (string.IsNullOrWhiteSpace(mimeType))
				return false;
			if (isAudioRequired && !mimeType.IsFileAudio())
				return false;
			if (isImageRequired && !mimeType.IsFileImage())
				return false;
			if (isVideoRequired && !mimeType.IsFileVideo())
				return false;
			return true;
		}
		#endregion

		#region Size to String
		/// <summary>
		/// Constant defining KB (2^10 bytes)
		/// </summary>
		public const int KB = 1024;

		/// <summary>
		/// Constant defining MB (2^20 bytes)
		/// </summary>
		public const int MB = 1024 * 1024;

		/// <summary>
		/// Constant defining GB (2^30 bytes)
		/// </summary>
		public const int GB = 1024 * 1024 * 1024;

		/// <summary>
		/// Convert an integral value to a human-readable FileSize string
		/// </summary>
		/// <param name="size">The number of bytes in the file data</param>
		/// <returns>A human-readable FileSize string</returns>
		public static string FileSizeToString(this short size) {
			if (size > KB)
				return (((double)size) / KB).ToString("N2") + " KB";
			else
				return size.ToString("N") + " B";
		}

		/// <summary>
		/// Convert an integral value to a human-readable FileSize string
		/// </summary>
		/// <param name="size">The number of bytes in the file data</param>
		/// <returns>A human-readable FileSize string</returns>
		public static string FileSizeToString(this ushort size) {
			if (size > KB)
				return (((double)size) / KB).ToString("N2") + " KB";
			else
				return size.ToString("N") + " B";
		}

		/// <summary>
		/// Convert an integral value to a human-readable FileSize string
		/// </summary>
		/// <param name="size">The number of bytes in the file data</param>
		/// <returns>A human-readable FileSize string</returns>
		public static string FileSizeToString(this int size) {
			if (size > GB)
				return (((double)size) / GB).ToString("N2") + " GB";
			else if (size > MB)
				return (((double)size) / MB).ToString("N2") + " MB";
			else if (size > KB)
				return (((double)size) / KB).ToString("N2") + " KB";
			else
				return size.ToString("N") + " B";
		}

		/// <summary>
		/// Convert an integral value to a human-readable FileSize string
		/// </summary>
		/// <param name="size">The number of bytes in the file data</param>
		/// <returns>A human-readable FileSize string</returns>
		public static string FileSizeToString(this int? size) {
			if (size == null)
				return "unknown";
			else
				return (size.Value).FileSizeToString();
		}

		/// <summary>
		/// Convert an integral value to a human-readable FileSize string
		/// </summary>
		/// <param name="size">The number of bytes in the file data</param>
		/// <returns>A human-readable FileSize string</returns>
		public static string FileSizeToString(this uint size) {
			if (size > GB)
				return (((double)size) / GB).ToString("N2") + " GB";
			else if (size > MB)
				return (((double)size) / MB).ToString("N2") + " MB";
			else if (size > KB)
				return (((double)size) / KB).ToString("N2") + " KB";
			else
				return size.ToString("N") + " B";
		}

		/// <summary>
		/// Convert an integral value to a human-readable FileSize string
		/// </summary>
		/// <param name="size">The number of bytes in the file data</param>
		/// <returns>A human-readable FileSize string</returns>
		public static string FileSizeToString(this long size) {
			if (size > GB)
				return (((double)size) / GB).ToString("N2") + " GB";
			else if (size > MB)
				return (((double)size) / MB).ToString("N2") + " MB";
			else if (size > KB)
				return (((double)size) / KB).ToString("N2") + " KB";
			else
				return size.ToString("N") + " B";
		}

		/// <summary>
		/// Convert an integral value to a human-readable FileSize string
		/// </summary>
		/// <param name="size">The number of bytes in the file data</param>
		/// <returns>A human-readable FileSize string</returns>
		public static string FileSizeToString(this ulong size) {
			if (size > GB)
				return (((double)size) / GB).ToString("N2") + " GB";
			else if (size > MB)
				return (((double)size) / MB).ToString("N2") + " MB";
			else if (size > KB)
				return (((double)size) / KB).ToString("N2") + " KB";
			else
				return size.ToString("N") + " B";
		}
		#endregion

		/// <summary>
		/// Fix the provided file extension by just getting the characters after the final dot (and trimming to the specified length)
		/// </summary>
		/// <param name="ext">The file extension to fix.</param>
		/// <param name="maxLength">The maximum length past which the result should be truncated.</param>
		/// <returns>The clean file extension</returns>
		public static string FixFileExtension(this string ext, int maxLength = 10) {
			if (string.IsNullOrWhiteSpace(ext))
				throw new ArgumentNullException(nameof(ext));
			if (!ext.Contains("."))
				return ext.TrimTo(maxLength).ToLower();
			string temp = ext.ToLower();
			while (temp.Contains("."))
				temp = temp.Substring(1);
			return temp.TrimTo(maxLength);
		}
		private static string TrimTo(this string text, int maxlen) {
			if (string.IsNullOrWhiteSpace(text))
				return string.Empty;
			string temp = text.Trim();
			if (temp.Length <= maxlen)
				return temp;
			else
				return temp.Substring(0, maxlen);
		}

		private static string FixExtension(this string s) {
			if (!s.StartsWith("."))
				return s;
			string temp = s;
			while (temp.StartsWith("."))
				temp = temp.Substring(1);
			return temp;
		}
		/// <summary>
		/// Split a filepath into Tuple containing Name (Item1) and Extension (Item2)
		/// </summary>
		/// <param name="path">The file path to split</param>
		/// <returns>A <see cref="Tuple{string, string}"/> containing the File Name (Item1) and Extension (Item2)</returns>
		public static Tuple<string, string> SplitFileName(this string path) {
			if (string.IsNullOrWhiteSpace(path))
				return null;

			string name = Path.GetFileNameWithoutExtension(path);
			string ext = Path.GetExtension(path);
			ext = ext.FixExtension();
			return new Tuple<string, string>(name, ext);
		}

		/// <summary>
		/// Split a path from a FileInfo instance into Tuple containing Name (Item1) and Extension (Item2)
		/// </summary>
		/// <param name="file">The FileInfo instance to split</param>
		/// <returns>A <see cref="Tuple{string, string}"/> containing the File Name (Item1) and Extension (Item2)</returns>
		public static Tuple<string, string> SplitFileName(this FileInfo file) {
			string name = Path.GetFileNameWithoutExtension(file.Name);
			string ext = file.Extension.FixExtension();
			return new Tuple<string, string>(name, ext);
		}
	}
}