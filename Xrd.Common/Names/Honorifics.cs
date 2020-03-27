using System.Collections.Generic;
using System.Linq;

using Xrd.Collections;

namespace Xrd.Names {
	/// <summary>
	/// Pre-defined lists of Personal Honorific Titles
	/// </summary>
	public static class Honorifics {
		/// Get a list of personal honorific titles from the specified TitleClasses. (Common are always included.)
		/// </summary>
		/// <param name="titleClasses">The various classes of titles to be included.</param>
		/// <returns>A list of unique titles, in alphabetical order.</returns>
		public static List<string> GetHonorificList(PrefixCategories titleClasses) {
			List<string> res = new List<string>(Common);
			if (titleClasses.HasFlag(PrefixCategories.Academic))
				res.AddRangeIfNotNull(Academic);
			if (titleClasses.HasFlag(PrefixCategories.CatholicOrthodox))
				res.AddRangeIfNotNull(Catholic);
			if (titleClasses.HasFlag(PrefixCategories.Formal_UK))
				res.AddRangeIfNotNull(UKFormal);
			if (titleClasses.HasFlag(PrefixCategories.Formal_US))
				res.AddRangeIfNotNull(USFormal);
			if (titleClasses.HasFlag(PrefixCategories.Islamic))
				res.AddRangeIfNotNull(Islamic);
			if (titleClasses.HasFlag(PrefixCategories.Judaic))
				res.AddRangeIfNotNull(Judaic);
			if (titleClasses.HasFlag(PrefixCategories.Professional))
				res.AddRangeIfNotNull(Professional);
			if (titleClasses.HasFlag(PrefixCategories.Protestant))
				res.AddRangeIfNotNull(Protestant);
			return res.Distinct().OrderBy(a => a).ToList();
		}

		/// <summary>
		/// Get a list of all common honorific prefixes for name splitting.
		/// </summary>
		/// <returns></returns>
		internal static List<string> GetAll() {
			var res = GetHonorificList(
			PrefixCategories.Academic |
			PrefixCategories.CatholicOrthodox |
			PrefixCategories.Formal_UK |
			PrefixCategories.Formal_US |
			PrefixCategories.Islamic |
			PrefixCategories.Judaic |
			PrefixCategories.Professional |
			PrefixCategories.Protestant
			);
			for (int i = 0; i < res.Count; i++) {
				if (res[i].Contains("."))
					res.Add(res[i].Replace(".", string.Empty));
			}
			return res;
		}

		internal static string[] ForParsingName = new string[] {
			"Master",
			"Mr.",
			"Mr",
			"Mister",
			"Miss",
			"Mrs.",
			"Mrs",
			"Missus",
			"Ms.",
			"Ms",
			"Mx.",
			"Mx",
			"Sir",
			"Madam",
			"Dame",
			"Lord",
			"Lady",
			"Her Excellency",
			"His Excellency",
			"Her Honour",
			"His Honour",
			"The Hon.",
			"The Hon",
			"Hon.",
			"Hon",
			"The",
			"The Honourable",
			"The Right Honourable",
			"The Most Honourable",
			"M.P.",
			"MP",
			"Rep.",
			"Rep",
			"Representative",
			"Sen.",
			"Sen",
			"Senator",
			"Dr.",
			"Dr",
			"Doctor",
			"Prof.",
			"Prof",
			"Professor",
			"Chancellor",
			"Vice-Chancellor",
			"Vice Chancellor",
			"Principal",
			"President",
			"Warden",
			"Dean",
			"Regent",
			"Rector",
			"Provost",
			"Director",
			"Chief Executive",
			"Cl",
			"Counsel",
			"SCl",
			"Senior Counsel",
			"Eur Ing",
			"European Engineer",
			"HH",
			"His Holiness",
			"HAH",
			"His All Holiness",
			"His Beatitude",
			"HE",
			"His Excellency",
			"HMEH",
			"His Most Eminent Highness",
			"His Eminence",
			"The Most Rev.",
			"The Most Rev",
			"The Most Reverend",
			"His Grace",
			"The Rt. Rev.",
			"The Rt Rev",
			"The Right Reverend",
			"The Rev.",
			"The Rev",
			"The Reverend",
			"Fr.",
			"Fr",
			"Father",
			"Br.",
			"Br",
			"Brother",
			"Sr.",
			"Sr",
			"Sister",
			"Pr.",
			"Pr",
			"Pastor",
			"Rev.",
			"Rev",
			"Reverend",
			"Elder",
			"Rabbi",
			"Imām",
			"Shaykh",
			"Muftī",
			"Hāfiz",
			"Hāfizah",
			"Qārī",
			"Mawlānā",
			"Hājī",
			"Sayyid",
			"Sayyidah",
			"Sharif"
		};

		/// <summary>
		/// Common personal honorific titles
		/// </summary>
		public static string[] Common = new string[] {
			"Master",
			"Mr.",
			"Mister",
			"Miss",
			"Mrs.",
			"Missus",
			"Ms.",
			"Mx.",
			"M."
		};

		/// <summary>
		/// Formal (UK) honorific titles.
		/// </summary>
		public static string[] UKFormal = new string[] {
			"Sir",
			"Madam",
			"Dame",
			"Lord",
			"Lady",
			"Her Excellency",
			"His Excellency",
			"Her Honour",
			"His Honour",
			"The Hon.",
			"Hon.",
			"The",
			"The Honourable",
			"The Right Honourable",
			"The Most Honourable",
			"M.P.",
		};

		/// <summary>
		/// Formal (US) honorific titles.
		/// </summary>
		public static string[] USFormal = new string[] {
			"Sir",
			"Madam",
			"Her Honor",
			"His Honor",
			"The Honorable",
			"Rep.",
			"Representative",
			"Sen.",
			"Senator",
		};

		/// <summary>
		/// Academic personal titles.
		/// </summary>
		public static string[] Academic = new string[] {
			"Dr.",
			"Doctor",
			"Prof.",
			"Professor",
			"Chancellor",
			"Vice-Chancellor",
			"Principal",
			"President",
			"Warden",
			"Dean",
			"Regent",
			"Rector",
			"Provost",
			"Director",
			"Chief Executive"
		};

		/// <summary>
		/// Professional personal titles (US & EU)
		/// </summary>
		public static string[] Professional = new string[] {
			"Dr.",
			"Doctor",
			"Cl",
			"Counsel",
			"SCl",
			"Senior Counsel",
			"Eur Ing",
			"European Engineer",
			"President",
			"Director",
			"Chief Executive"
		};

		/// <summary>
		/// Roman Catholic & Orthodox personal titles.
		/// </summary>
		public static string[] Catholic = new string[] {
			"HH",
			"His Holiness",
			"HAH",
			"His All Holiness",
			"His Beatitude",
			"HE",
			"His Excellency",
			"HMEH",
			"His Most Eminent Highness",
			"His Eminence",
			"The Most Rev.",
			"The Most Reverend",
			"His Grace",
			"The Rt. Rev.",
			"The Right Reverend",
			"The Rev.",
			"The Reverend",
			"Fr.",
			"Father",
			"Br.",
			"Brother",
			"Sr.",
			"Sister"
		};

		/// <summary>
		/// Protestant Christian personal titles.
		/// </summary>
		public static string[] Protestant = new string[] {
			"Fr.",
			"Father",
			"Pr.",
			"Pastor",
			"Br.",
			"Brother",
			"Rev.",
			"Reverend",
			"Elder"
		};

		/// <summary>
		/// Judaic personal titles.
		/// </summary>
		public static string[] Judaic = new string[] {
			"Rabbi"
		};

		/// <summary>
		/// Islamic personal titles.
		/// </summary>
		public static string[] Islamic = new string[] {
			"Imām",
			"Shaykh",
			"Muftī",
			"Hāfiz",
			"Hāfizah",
			"Qārī",
			"Mawlānā",
			"Hājī",
			"Sayyid",
			"Sayyidah",
			"Sharif"
		};
	}
}