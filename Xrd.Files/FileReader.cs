using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace Xrd.Files {
	/// <summary>
	/// Helper class for reading binary data.
	/// </summary>
	public static class FileReader {
		private static bool ValidateSize(long fileLength) =>
			(fileLength > int.MaxValue) ? throw new Exception("This file is too large to read into memory") : true;
		/// <summary>
		/// Get a FileInfo instance for the specified path. Throws an exception if the path is invalid.
		/// </summary>
		/// <param name="path">The path to the file.</param>
		/// <returns>A FileInfo instance.</returns>
		public static FileInfo GetFileInfo(string path) {
			if (!System.IO.File.Exists(path))
				throw new FileNotFoundException("File not found.", path);
			return new FileInfo(path);
		}
		/// <summary>
		/// Read the <see cref="FileInfo"/>'s content into a byte array.
		/// </summary>
		/// <param name="file">The file to read.</param>
		/// <returns>The file's contents as a byte array.</returns>
		public static byte[] ToBinary(FileInfo file) {
			ValidateSize(file.Length);

			byte[] buffer = new byte[file.Length];
			using (FileStream fs = file.OpenRead()) {
				fs.Read(buffer, 0, buffer.Length);
				fs.Close();
			}
			return buffer;
		}

		/// <summary>
		/// Read the contents of the file at the specified path into a byte array.
		/// </summary>
		/// <param name="file">The path to the file to read.</param>
		/// <returns>The file's contents as a byte array.</returns>
		public static byte[] ToBinary(string path) =>
			ToBinary(GetFileInfo(path));

		/// <summary>
		/// Read the <see cref="FileInfo"/>'s content into a byte array.
		/// </summary>
		/// <param name="file">The file to read.</param>
		/// <returns>The file's contents as a byte array.</returns>
		public static async Task<byte[]> ToBinaryAsync(FileInfo file) {
			ValidateSize(file.Length);

			byte[] buffer = new byte[file.Length];
			using (FileStream fs = file.OpenRead()) {
				await fs.ReadAsync(buffer, 0, buffer.Length);
				fs.Close();
			}
			return buffer;
		}
		/// <summary>
		/// Read the contents of the file at the specified path into a byte array.
		/// </summary>
		/// <param name="file">The path to the file to read.</param>
		/// <returns>The file's contents as a byte array.</returns>
		public static async Task<byte[]> ToBinaryAsync(string path) =>
			await ToBinaryAsync(GetFileInfo(path));

		/// <summary>
		/// Read the <see cref="FileInfo"/>'s contents into a single string instance.
		/// </summary>
		/// <param name="file">The path to the file to read.</param>
		/// <returns>The file's contents as a single string.</returns>
		public static string ToSingleString(FileInfo file) {
			ValidateSize(file.Length);
			string buffer;
			using (FileStream fs = file.OpenRead()) {
				using (StreamReader sr = new StreamReader(fs)) {
					buffer = sr.ReadToEnd();
					sr.Close();
				}
			}
			return buffer;
		}
		/// <summary>
		/// Read the contents of the file at the specified path into a single string.
		/// </summary>
		/// <param name="file">The path to the file to read.</param>
		/// <returns>The file's contents as a single string.</returns>
		public static string ToSingleString(string path) =>
			ToSingleString(GetFileInfo(path));

		/// <summary>
		/// Read the <see cref="FileInfo"/>'s contents into a single string instance.
		/// </summary>
		/// <param name="file">The path to the file to read.</param>
		/// <returns>The file's contents as a single string.</returns>
		public static async Task<string> ToSingleStringAsync(FileInfo file) {
			ValidateSize(file.Length);

			string buffer;
			using (StreamReader sr = file.OpenText()) {
				buffer = await sr.ReadToEndAsync();
				sr.Close();
			}
			return buffer;
		}
		/// <summary>
		/// Read the contents of the file at the specified path into a single string.
		/// </summary>
		/// <param name="file">The path to the file to read.</param>
		/// <returns>The file's contents as a single string.</returns>
		public static async Task<string> ToSingleStringAsync(string path) =>
			await ToSingleStringAsync(GetFileInfo(path));

		/// <summary>
		/// Read the <see cref="FileInfo"/>'s contents into a read-only collection of strings (one per line).
		/// </summary>
		/// <param name="file">The path to the file to read.</param>
		/// <returns>The file's contents as a collection of strings (one per line).</returns>
		public static ReadOnlyCollection<string> ToStringCollection(FileInfo file) {
			ValidateSize(file.Length);

			List<string> buffer = new List<string>();
			using (StreamReader sr = file.OpenText()) {
				while (!sr.EndOfStream) {
					buffer.Add(sr.ReadLine());
				}
				sr.Close();
			}
			return new ReadOnlyCollection<string>(buffer);
		}
		/// <summary>
		/// Read the contents of the file at the specified path into a read-only collection of strings (one per line).
		/// </summary>
		/// <param name="file">The path to the file to read.</param>
		/// <returns>The file's contents as a collection of strings (one per line).</returns>
		public static ReadOnlyCollection<string> ToStringCollection(string path) =>
			ToStringCollection(GetFileInfo(path));

		/// <summary>
		/// Read the <see cref="FileInfo"/>'s contents into a read-only collection of strings (one per line).
		/// </summary>
		/// <param name="file">The path to the file to read.</param>
		/// <returns>The file's contents as a collection of strings (one per line).</returns>
		public static async Task<ReadOnlyCollection<string>> ToStringCollectionAsync(FileInfo file) {
			ValidateSize(file.Length);

			List<string> buffer = new List<string>();
			using (StreamReader sr = file.OpenText()) {
				while (!sr.EndOfStream) {
					buffer.Add(await sr.ReadLineAsync());
				}
				sr.Close();
			}
			return new ReadOnlyCollection<string>(buffer);
		}
		/// <summary>
		/// Read the contents of the file at the specified path into a read-only collection of strings (one per line).
		/// </summary>
		/// <param name="file">The path to the file to read.</param>
		/// <returns>The file's contents as a collection of strings (one per line).</returns>
		public static async Task<ReadOnlyCollection<string>> ToStringCollectionAsync(string path) =>
			await ToStringCollectionAsync(GetFileInfo(path));
	}
}