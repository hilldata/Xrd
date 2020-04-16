using System;
using System.Collections.Generic;
using System.Text;
using Xrd.Text;
using System.IO;
using System.Threading.Tasks;

namespace Xrd.vCard {
	internal static class ParsingExtensions {
		internal static Params.IPropertyParameter ParseParam(this string value) {
			if (string.IsNullOrWhiteSpace(value))
				return null;
			if (value.NonQuotedIndexOf(Constants.PARAM_NAME_VAL_SEPARATOR) < 1)
				return null;

			Tuple<string, string> pair = value.NonQuotedSplitOnFirst(Constants.PARAM_NAME_VAL_SEPARATOR);
			switch (pair.Item1.ToKnownParameter()) {
				case KnownParameters.ALTID: return new Params.ALTID(pair);
				case KnownParameters.CALSCALE: return new Params.CALSCALE(pair);
				case KnownParameters.GEO: return new Params.GEO(pair);
				case KnownParameters.LABEL: return new Params.LABEL(pair);
				case KnownParameters.LANGUAGE: return new Params.LANGUAGE(pair);
				case KnownParameters.MEDIATYPE: return new Params.MEDIATYPE(pair);
				case KnownParameters.PID: return new Params.PID(pair);
				case KnownParameters.PREF: return new Params.PREF(pair);
				case KnownParameters.SORT_AS: return new Params.SORT_AS(pair);
				case KnownParameters.TYPE: return new Params.TYPE(pair);
				case KnownParameters.TZ: return new Params.TZ(pair);
				case KnownParameters.VALUE: return new Params.VALUE(pair);
				default: return new Params.UnknownParameter(pair);
			}
		}

		internal static Props.IProperty ParseProp(this string value) {
			if (string.IsNullOrWhiteSpace(value))
				return null;
			if (value.NonQuotedIndexOf(Constants.PROP_NAME_VAL_SEPARATOR) < 1)
				return null;

			Tuple<string, string> nvPair = value.NonQuotedSplitOnFirst(Constants.PROP_NAME_VAL_SEPARATOR);
			string v = nvPair.Item2;
			string n;
			string p = string.Empty;

			if (nvPair.Item1.NonQuotedIndexOf(Constants.PROP_PARAMETER_SEPARATOR) > 0) {
				Tuple<string, string> pair2 = nvPair.Item1.NonQuotedSplitOnFirst(Constants.PROP_PARAMETER_SEPARATOR);
				n = pair2.Item1;
				p = pair2.Item2;
			} else
				n = nvPair.Item1;

			switch (n.ToKnownProperty()) {
				case KnownProperties.ADR: return new Props.ADR(n, p, v);
				case KnownProperties.ANNIVERSARY: return new Props.ANNIVERSARY(n, p, v);
				case KnownProperties.BDAY: return new Props.BDAY(n, p, v);
				case KnownProperties.BEGIN: return new Props.BEGIN();
				case KnownProperties.CALADRURI: return new Props.CALADRURI(n, p, v);
				case KnownProperties.CALURI: return new Props.CALURI(n, p, v);
				case KnownProperties.CATEGORIES: return new Props.CATEGORIES(n, p, v);
				case KnownProperties.CLIENTPIDMAP: return new Props.CLIENTPIDMAP(n, p, v);
				case KnownProperties.EMAIL: return new Props.EMAIL(n, p, v);
				case KnownProperties.END: return new Props.END();
				case KnownProperties.FBURL: return new Props.FBURL(n, p, v);
				case KnownProperties.FN: return new Props.FN(n, p, v);
				case KnownProperties.GENDER: return new Props.GENDER(n, p, v);
				case KnownProperties.GEO: return new Props.GEO(n, p, v);
				case KnownProperties.IMPP: return new Props.IMPP(n, p, v);
				case KnownProperties.KEY: return new Props.KEY(n, p, v);
				case KnownProperties.KIND: return new Props.KIND(n, p, v);
				case KnownProperties.LANG: return new Props.LANG(n, p, v);
				case KnownProperties.LOGO: return new Props.LOGO(n, p, v);
				case KnownProperties.MEMBER: return new Props.MEMBER(n, p, v);
				case KnownProperties.N: return new Props.N(n, p, v);
				case KnownProperties.NICKNAME: return new Props.NICKNAME(n, p, v);
				case KnownProperties.NOTE: return new Props.NOTE(n, p, v);
				case KnownProperties.ORG: return new Props.ORG(n, p, v);
				case KnownProperties.PHOTO: return new Props.PHOTO(n, p, v);
				case KnownProperties.PRODID: return new Props.PRODID(n, p, v);
				case KnownProperties.RELATED: return new Props.RELATED(n, p, v);
				case KnownProperties.REV: return new Props.REV(n, p, v);
				case KnownProperties.ROLE: return new Props.ROLE(n, p, v);
				case KnownProperties.SOUND: return new Props.SOUND(n, p, v);
				case KnownProperties.SOURCE: return new Props.SOURCE(n, p, v);
				case KnownProperties.TEL: return new Props.TEL(n, p, v);
				case KnownProperties.TITLE: return new Props.TITLE(n, p, v);
				case KnownProperties.TZ: return new Props.TZ(n, p, v);
				case KnownProperties.UID: return new Props.UID(n, p, v);
				case KnownProperties.URL: return new Props.URL(n, p, v);
				case KnownProperties.VERSION: return new Props.VERSION(n, p, v);
				case KnownProperties.XML: return new Props.XML(n, p, v);
				default: return new Props.UnknownProperty(n, p, v);
			}
		}

		#region VCard parsing
		private static string Unfold(this string value) {
			return value.Replace(Constants.LINE_FOLD1, string.Empty)
				.Replace(Constants.LINE_FOLD2, string.Empty);
		}

		private static void ValidateFile(this FileInfo fi) {
			if (!fi.Exists)
				throw new FileNotFoundException($"The specified file '{fi.Name}' does not exist.", fi.Name);
			if (!Constants.vCARD_EXTENSIONS.Contains(fi.Extension.ToLower().Replace(".", string.Empty)))
				throw new ArgumentException($"The specified file '{fi.Name}' does not have a valid vCard extension ('vcf' or 'vcard').", nameof(fi));
		}

		internal static List<VirtualCard> ReadFile(this FileInfo fi, int? maxCount = null) {
			fi.ValidateFile();
			using (StreamReader sr = fi.OpenText()) {
				string temp = sr.ReadToEnd();
				sr.Close();
				return temp.ParseCard(maxCount);
			}
		}

		internal static async Task<List<VirtualCard>> ReadFileAsync(this FileInfo fi, int? maxCount = null) {
			fi.ValidateFile();
			using (StreamReader sr = fi.OpenText()) {
				string temp = await sr.ReadToEndAsync();
				sr.Close();
				return await temp.ParseCardAsync(maxCount);
			}
		}

		internal static List<VirtualCard> ParseCard(this string value, int? maxCount = null) {
			List<VirtualCard> res = new List<VirtualCard>();
			using (StringReader sr = new StringReader(value.Unfold())) {
				VirtualCard curr = null;
				string line = null;
				while ((line = sr.ReadLine()) != null) {
					Props.IProperty prop = line.ParseProp();
					if (prop.Property == KnownProperties.BEGIN) {
						VirtualCard t = new VirtualCard();
						res.Add(t);
						curr = t;
					} else if (prop.Property == KnownProperties.END) {
						curr = null;
						if (res.Count >= (maxCount ?? int.MaxValue))
							return res;
					} else {
						if (curr != null)
							curr.AddProperty(prop);
					}
				}
			}
			return res;
		}

		internal static async Task<List<VirtualCard>> ParseCardAsync(this string value, int? maxCount = null) {
			List<VirtualCard> res = new List<VirtualCard>();
			using (StringReader sr = new StringReader(value.Unfold())) {
				VirtualCard curr = null;
				string line = null;
				while ((line = await sr.ReadLineAsync()) != null) {
					Props.IProperty prop = line.ParseProp();
					if (prop.Property == KnownProperties.BEGIN) {
						VirtualCard t = new VirtualCard();
						res.Add(t);
						curr = t;
					} else if (prop.Property == KnownProperties.END) {
						curr = null;
						if (res.Count >= (maxCount ?? int.MaxValue))
							return res;
					} else {
						if (curr != null)
							curr.AddProperty(prop);
					}
				}
			}
			return res;
		}
		#endregion
	}
}