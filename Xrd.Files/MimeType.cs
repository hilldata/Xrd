using System;
using System.Collections.Generic;

namespace Xrd.Files {
	public class MimeType {
		public MimeType(string ext, string kind, string mime) {
			Ext = !string.IsNullOrWhiteSpace(ext) ? ext.FixFileExtension() : throw new ArgumentNullException(nameof(Ext));
			Kind = kind.Trim();
			MIME = !string.IsNullOrWhiteSpace(mime) ? mime.Trim().ToLower() : throw new ArgumentNullException(nameof(mime));
		}
		#region Fields
		public readonly string Ext;
		public readonly string Kind;
		public readonly string MIME;
		#endregion

		// Pulled from https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Complete_list_of_MIME_types
		// on 12/13/2019
		public static List<MimeType> DefaultMimeTypes = new List<MimeType>() {
			new MimeType(".aac","AAC audio","audio/aac"),
			new MimeType(".abw","AbiWord document","application/x-abiword"),
			new MimeType(".arc","Archive document (multiple files embedded)","application/x-freearc"),
			new MimeType(".avi","AVI: Audio Video Interleave","video/x-msvideo"),
			new MimeType(".azw","Amazon Kindle eBook format","application/vnd.amazon.ebook"),
			new MimeType(".bin","Any kind of binary data","application/octet-stream"),
			new MimeType(".bmp","Windows OS/2 Bitmap Graphics","image/bmp"),
			new MimeType(".bz","BZip archive","application/x-bzip"),
			new MimeType(".bz2","BZip2 archive","application/x-bzip2"),
			new MimeType(".csh","C-Shell script","application/x-csh"),
			new MimeType(".css","Cascading Style Sheets (CSS)","text/css"),
			new MimeType(".csv","Comma-separated values (CSV)","text/csv"),
			new MimeType(".doc","Microsoft Word","application/msword"),
			new MimeType(".docx","Microsoft Word (OpenXML)","application/vnd.openxmlformats-officedocument.wordprocessingml.document"),
			new MimeType(".eot","MS Embedded OpenType fonts","application/vnd.ms-fontobject"),
			new MimeType(".epub","Electronic publication (EPUB)","application/epub+zip"),
			new MimeType(".gz","GZip Compressed Archive","application/gzip"),
			new MimeType(".gif","Graphics Interchange Format (GIF)","image/gif"),
			new MimeType(".htm","HyperText Markup Language (HTML)","text/html"),
			new MimeType(".html","HyperText Markup Language (HTML)","text/html"),
			new MimeType(".ico","Icon format","image/vnd.microsoft.icon"),
			new MimeType(".ics","iCalendar format","text/calendar"),
			new MimeType(".jar","Java Archive (JAR)","application/java-archive"),
			new MimeType(".jpeg","JPEG images","image/jpeg"),
			new MimeType(".jpg","JPEG images","image/jpeg"),
			new MimeType(".js","JavaScript","text/javascript"),
			new MimeType(".json","JSON format","application/json"),
			new MimeType(".jsonld","JSON-LD format","application/ld+json"),
			new MimeType(".mid","Musical Instrument Digital Interface (MIDI)","audio/x-midi"),
			new MimeType(".midi","Musical Instrument Digital Interface (MIDI)","audio/x-midi"),
			new MimeType(".mjs","JavaScript module","text/javascript"),
			new MimeType(".mp3","MP3 audio","audio/mpeg"),
			new MimeType(".mpeg","MPEG Video","video/mpeg"),
			new MimeType(".mpkg","Apple Installer Package","application/vnd.apple.installer+xml"),
			new MimeType(".odp","OpenDocument presentation document","application/vnd.oasis.opendocument.presentation"),
			new MimeType(".ods","OpenDocument spreadsheet document","application/vnd.oasis.opendocument.spreadsheet"),
			new MimeType(".odt","OpenDocument text document","application/vnd.oasis.opendocument.text"),
			new MimeType(".oga","OGG audio","audio/ogg"),
			new MimeType(".ogv","OGG video","video/ogg"),
			new MimeType(".ogx","OGG","application/ogg"),
			new MimeType(".opus","Opus audio","audio/opus"),
			new MimeType(".otf","OpenType font","font/otf"),
			new MimeType(".png","Portable Network Graphics","image/png"),
			new MimeType(".pdf","Adobe Portable Document Format (PDF)","application/pdf"),
			new MimeType(".php","Hypertext Preprocessor (Personal Home Page)","application/php"),
			new MimeType(".ppt","Microsoft PowerPoint","application/vnd.ms-powerpoint"),
			new MimeType(".pptx","Microsoft PowerPoint (OpenXML)","application/vnd.openxmlformats-officedocument.presentationml.presentation"),
			new MimeType(".rar","RAR archive","application/x-rar-compressed"),
			new MimeType(".rtf","Rich Text Format (RTF)","application/rtf"),
			new MimeType(".sh","Bourne shell script","application/x-sh"),
			new MimeType(".svg","Scalable Vector Graphics (SVG)","image/svg+xml"),
			new MimeType(".swf","Small web format (SWF) or Adobe Flash document","application/x-shockwave-flash"),
			new MimeType(".tar","Tape Archive (TAR)","application/x-tar"),
			new MimeType(".tif","Tagged Image File Format (TIFF)","image/tiff"),
			new MimeType(".tiff","Tagged Image File Format (TIFF)","image/tiff"),
			new MimeType(".ts","MPEG transport stream","video/mp2t"),
			new MimeType(".ttf","TrueType Font","font/ttf"),
			new MimeType(".txt","Text, (generally ASCII or ISO 8859-n)","text/plain"),
			new MimeType(".vsd","Microsoft Visio","application/vnd.visio"),
			new MimeType(".wav","Waveform Audio Format","audio/wav"),
			new MimeType(".weba","WEBM audio","audio/webm"),
			new MimeType(".webm","WEBM video","video/webm"),
			new MimeType(".webp","WEBP image","image/webp"),
			new MimeType(".woff","Web Open Font Format (WOFF)","font/woff"),
			new MimeType(".woff2","Web Open Font Format (WOFF)","font/woff2"),
			new MimeType(".xhtml","XHTML","application/xhtml+xml"),
			new MimeType(".xls","Microsoft Excel","application/vnd.ms-excel"),
			new MimeType(".xlsx","Microsoft Excel (OpenXML)","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"),
			new MimeType(".xml","XML","text/xml"),
			new MimeType(".xul","XUL","application/vnd.mozilla.xul+xml"),
			new MimeType(".zip","ZIP archive","application/zip"),
			new MimeType(".3gp","3GPP audio/video container","video/3gpp"),
			new MimeType(".3g2","3GPP2 audio/video container","video/3gpp2"),
			new MimeType(".7z","7-zip archive","application/x-7z-compressed")
		};
	}
}