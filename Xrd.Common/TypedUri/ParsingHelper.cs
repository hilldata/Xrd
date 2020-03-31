namespace Xrd.TypedUri {
	internal static class ParsingHelper {
		internal static string UriPathEncodingError(string propName, string dataType) => $"The URI path provided does not properly encode the [{propName}] as a [{dataType}] value.";
		internal static string UriWrongSchemeError(string scheme) => $"The URI provided does not implement the [{scheme}:] scheme.";
		internal static string UriPathNotWellFormedMessage(string dataType) => $"The uriPath is not well formed for a [{dataType}] value.";
	}
}