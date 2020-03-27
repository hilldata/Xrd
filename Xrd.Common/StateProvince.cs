using System.Collections.Generic;

namespace Xrd {
	/// <summary>
	/// Class representing a state or province of the USA, Canada, and Mexico
	/// </summary>
	public class StateProvince {
		/// <summary>
		/// Default constructor
		/// </summary>
		public StateProvince() { }
		private StateProvince(string abbr, string name, string country = "USA") {
			Abbreviation = abbr;
			Name = name;
			Country = country;
		}
		#region Fields
		/// <summary>
		/// The commonly accepted postal abbreviation for the state or province
		/// </summary>
		public string Abbreviation { get; set; }
		/// <summary>
		/// The full name of the state or province
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// The country for the state or province.
		/// </summary>
		public string Country { get; set; } = USA;
		#endregion

		public const string USA = "USA";
		public const string CANADA = "CANADA";
		public const string MEXICO = "MEXICO";

		/// <summary>
		/// List containing the known state/provinces/territories of the USA, Canada and Mexico
		/// </summary>
		public static List<StateProvince> NorthAmerican => new List<StateProvince> {
			new StateProvince("AL", "Alabama"),
			new StateProvince("AK", "Alaska"),
			new StateProvince("AZ", "Arizona"),
			new StateProvince("AR", "Arkansas"),
			new StateProvince("CA", "California"),
			new StateProvince("CO", "Colorado"),
			new StateProvince("CT", "Connecticut"),
			new StateProvince("DE", "Delaware"),
			new StateProvince("FL", "Florida"),
			new StateProvince("GA", "Georgia"),
			new StateProvince("HI", "Hawaii"),
			new StateProvince("ID", "Idaho"),
			new StateProvince("IL", "Illinois"),
			new StateProvince("IN", "Indiana"),
			new StateProvince("IA", "Iowa"),
			new StateProvince("KS", "Kansas"),
			new StateProvince("KY", "Kentucky"),
			new StateProvince("LA", "Louisiana"),
			new StateProvince("ME", "Maine"),
			new StateProvince("MD", "Maryland"),
			new StateProvince("MA", "Massachusetts"),
			new StateProvince("MI", "Michigan"),
			new StateProvince("MN", "Minnesota"),
			new StateProvince("MS", "Mississippi"),
			new StateProvince("MO", "Missouri"),
			new StateProvince("MT", "Montana"),
			new StateProvince("NE", "Nebraska"),
			new StateProvince("NV", "Nevada"),
			new StateProvince("NH", "New Hampshire"),
			new StateProvince("NJ", "New Jersey"),
			new StateProvince("NM", "New Mexico"),
			new StateProvince("NY", "New York"),
			new StateProvince("NC", "North Carolina"),
			new StateProvince("ND", "North Dakota"),
			new StateProvince("OH", "Ohio"),
			new StateProvince("OK", "Oklahoma"),
			new StateProvince("OR", "Oregon"),
			new StateProvince("PA", "Pennsylvania"),
			new StateProvince("RI", "Rhode Island"),
			new StateProvince("SC", "South Carolina"),
			new StateProvince("SD", "South Dakota"),
			new StateProvince("TN", "Tennessee"),
			new StateProvince("TX", "Texas"),
			new StateProvince("UT", "Utah"),
			new StateProvince("VT", "Vermont"),
			new StateProvince("VA", "Virginia"),
			new StateProvince("WA", "Washington"),
			new StateProvince("WV", "West Virginia"),
			new StateProvince("WI", "Wisconson"),
			new StateProvince("WY", "Wyoming"),
			new StateProvince("AB", "Alberta", CANADA),
			new StateProvince("BC", "British Columbia", CANADA),
			new StateProvince("MB", "Manitoba", CANADA),
			new StateProvince("NB", "New Brunswick", CANADA),
			new StateProvince("NL", "Newfoundland and Labrador", CANADA),
			new StateProvince("NS", "Nova Scotia", CANADA),
			new StateProvince("NT", "Northwest Territories", CANADA),
			new StateProvince("NU", "Nunavut", CANADA),
			new StateProvince("ON", "Ontario", CANADA),
			new StateProvince("PE", "Prince Edward Island", CANADA),
			new StateProvince("QC", "Quebec", CANADA),
			new StateProvince("SK", "Saskatchewan", CANADA),
			new StateProvince("YT", "Yukon", CANADA),
			new StateProvince("DC", "District of Columbia"),
			new StateProvince("AS", "American Samoa"),
			new StateProvince("GU", "Guam"),
			new StateProvince("MH", "Marshall Islands"),
			new StateProvince("MP", "Northern Mariana Islands"),
			new StateProvince("PR", "Puerto Rico"),
			new StateProvince("VI", "US Virgin Islands"),
			new StateProvince("Ags.","Aguascalientes", MEXICO),
			new StateProvince("B.C.","Baja California", MEXICO),
			new StateProvince("B.C.S.","Baja California Sur", MEXICO),
			new StateProvince("Camp.","Campeche", MEXICO),
			new StateProvince("Chis.", "Chiapas", MEXICO),
			new StateProvince("Chih.","Chihuahua", MEXICO),
			new StateProvince("Coah.","Coahuila", MEXICO),
			new StateProvince("Col.","Colima", MEXICO),
			new StateProvince("CDMX","Mexico City", MEXICO),
			new StateProvince("Dgo.","Durango", MEXICO),
			new StateProvince("Gto.", "Guanajuato", MEXICO),
			new StateProvince("Gro.", "Guerrero", MEXICO),
			new StateProvince("Hgo.","Hidalgo", MEXICO),
			new StateProvince("Jal.", "Jalisco", MEXICO),
			new StateProvince("Méx.", "México", MEXICO),
			new StateProvince("Mich.", "Michoacán", MEXICO),
			new StateProvince("Mor.", "Morelos", MEXICO),
			new StateProvince("Nay.", "Nayarit", MEXICO),
			new StateProvince("N.L.", "Nuevo León", MEXICO),
			new StateProvince("Oax.", "Oaxaca", MEXICO),
			new StateProvince("Pue.", "Puebla", MEXICO),
			new StateProvince("Qro.", "Querétaro", MEXICO),
			new StateProvince("Q.R.", "Quintana Roo", MEXICO),
			new StateProvince("S.L.P.", "San Luis Potosí", MEXICO),
			new StateProvince("Sin.", "Sinaloa", MEXICO),
			new StateProvince("Son.", "Sonora", MEXICO),
			new StateProvince("Tab.", "Tabasco", MEXICO),
			new StateProvince("Tamps.", "Tamaulipas", MEXICO),
			new StateProvince("Tlax.", "Tlaxcala", MEXICO),
			new StateProvince("Ver.", "Veracruz", MEXICO),
			new StateProvince("Yuc.", "Yucatán", MEXICO),
			new StateProvince("Zac.", "Zacatecas", MEXICO)
		};
	}
}