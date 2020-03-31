using System;
using System.Collections.Generic;

using Xrd.Text;

namespace Xrd.TypedUri.DataTypes {
	/// <summary>
	/// Class representing geo-location data as used in URIs with the "geo:" scheme.
	/// </summary>
	/// <remarks>See https://tools.ietf.org/html/rfc5870 </remarks>
	public class GeoLoc : UriDataTypeBase {
		#region Properties
		private float _lat;
		private float _long;
		private float? _alt;
		private float? _uncertainty;
		private string _crs;
		#endregion

		#region Constants
		/// <summary>
		/// The minimum allowed value for latitude
		/// </summary>
		public const float MinLatValue = -90f;
		/// <summary>
		/// The maximum allowed value for latitude
		/// </summary>
		public const float MaxLatValue = 90f;
		/// <summary>
		/// The minimum allowed value for longitude
		/// </summary>
		public const float MinLongValue = -180f;
		/// <summary>
		/// /The maximum allowed value for longitude
		/// </summary>
		public const float MaxLongValue = 180f;
		/// <summary>
		/// The default CRS (coordinate reference system) used by the class
		/// </summary>
		public const string DefaultCRSLabel = "wgs84";
		#endregion

		/// <summary>
		/// Constructor for GeoLoc class
		/// </summary>
		/// <remarks>Default constructor results in Origin location with null altitude, uncertainty and default CRS</remarks>
		public GeoLoc() : this(0, 0) { }

		/// <summary>
		/// Constructor for GeoLoc class
		/// </summary>
		/// <param name="latitude">The latitude value</param>
		/// <param name="longitude">The longitude value</param>
		/// <param name="altitude">The altitude value (optional)</param>
		/// <param name="uncertainty">The uncertainty value (optional)</param>
		/// <param name="crsLabel">The CRS label for the system used (optional; defaults to WSG84)</param>
		/// <remarks>If all parameters are provided, the class is fully instantiated.</remarks>
		public GeoLoc(float latitude, float longitude, float? altitude = null, float? uncertainty = null, string crsLabel = null) {
			Latitude = latitude;
			Longitude = longitude;
			Altitude = altitude;
			Uncertainty = uncertainty;
			CrsLabel = crsLabel;
		}

		/// <summary>
		/// Constructor for GeoLoc class
		/// </summary>
		/// <param name="latitude">The latitude value</param>
		/// <param name="longitude">The longitude value</param>
		/// <param name="crsLabel">The CRS label for the sytem used</param>
		/// <remarks>This results in a partially instantiated object, with null altitude and uncertainty.</remarks>
		public GeoLoc(float latitude, float longitude, string crsLabel) : this(latitude, longitude, null, null, crsLabel) { }

		/// <summary>
		/// Construct an instance of the GeoLoc URI data type
		/// </summary>
		/// <param name="uriPath">Either the LocalPath or AbsolutePath from a "geo:" URI</param>
		/// <param name="otherParameters">Returns any additional, unrecognized URI parameters</param>
		public GeoLoc(string uriPath, out List<UriParameter> otherParameters) {
			if (string.IsNullOrWhiteSpace(uriPath))
				throw new ArgumentNullException(nameof(uriPath));

			List<string> split1 = uriPath.NonQuotedSplit(';', StringSplitOptions.RemoveEmptyEntries, true);
			if (!split1[0].Contains(","))
				throw new ArgumentException(ParsingHelper.UriPathNotWellFormedMessage("geo-loc"), nameof(uriPath));

			List<string> coords = split1[0].NonQuotedSplit(',', false);
			if (coords.Count > 1) {
				if (float.TryParse(coords[0], out float a))
					Latitude = a;
				else
					throw new ArgumentException(ParsingHelper.UriPathEncodingError(nameof(Latitude), "float"), nameof(uriPath));
				if (float.TryParse(coords[1], out float b))
					Longitude = b;
				else
					throw new ArgumentException(ParsingHelper.UriPathEncodingError(nameof(Longitude), "float"), nameof(uriPath));
			}
			if (coords.Count > 2) {
				if (float.TryParse(coords[2], out float c))
					Altitude = c;
				else
					throw new ArgumentException(ParsingHelper.UriPathEncodingError(nameof(Altitude), "float"), nameof(uriPath));
			} else
				Altitude = null;

			Uncertainty = null;

			if (split1.Count > 1) {
				otherParameters = new List<UriParameter>();
				for (int i = 1; i < split1.Count; i++) {
					UriParameter param;
					try {
						param = new UriParameter(split1[i]);
						if (param.Is("crs"))
							CrsLabel = param.Value;
						else if (param.Is("u")) {
							if (float.TryParse(param.Value, out float u))
								Uncertainty = u;
							else
								Uncertainty = null;
						} else
							otherParameters.Add(param);
					} catch { }
				}
				if (otherParameters.Count < 1)
					otherParameters = null;
			} else
				otherParameters = null;
		}

		#region Properties
		/// <summary>
		/// The Latitude
		/// </summary>
		public float Latitude {
			get => _lat;
			set {
				if (value < MinLatValue || value > MaxLatValue)
					throw new ArgumentOutOfRangeException(nameof(value), $"Latitude must be between {MinLatValue} and {MaxLatValue}, inclusive.");

				SetField(ref _lat, value);
				// Set the Longitude to 0 if Latitude is Min/Max (North or South Pole)
				if (_lat.Equals(MaxLatValue) || _lat.Equals(MinLatValue))
					SetField(ref _long, 0f);
			}
		}

		/// <summary>
		/// The Longitude
		/// </summary>
		public float Longitude {
			get => _long;
			set {
				if (value < MinLongValue || value > MaxLongValue)
					throw new ArgumentOutOfRangeException(nameof(value), $"Longitude must be between {MinLongValue} and {MaxLongValue}, inclusive.");
				SetField(ref _long, value);
				// If the Latitude is Min/Max (North or South Pole), the reset the Longitude to zero.
				if (_lat.Equals(MaxLatValue) || _lat.Equals(MinLatValue))
					SetField(ref _long, 0f);
			}
		}

		/// <summary>
		/// The Altitude (optional)
		/// </summary>
		public float? Altitude {
			get => _alt;
			set => SetField(ref _alt, value);
		}

		/// <summary>
		/// The uncertainty value of the positioning system (optional)
		/// </summary>
		public float? Uncertainty {
			get => _uncertainty;
			set => SetField(ref _uncertainty, value);
		}

		/// <summary>
		/// The CRS (Coordinate Reference System) label of the system used. Default = "wgs84"
		/// </summary>
		public string CrsLabel {
			get => string.IsNullOrWhiteSpace(_crs) ? DefaultCRSLabel : _crs;
			set {
				if (string.IsNullOrWhiteSpace(value) || value.Trim().Equals(DefaultCRSLabel))
					SetField(ref _crs, null);
				else
					SetField(ref _crs, value.Trim());
			}
		}
		#endregion

		public override string ToString() {
			string res = $"{Latitude},{Longitude}";
			if (Altitude.HasValue)
				res += $",{Altitude.Value}";
			if (Uncertainty.HasValue)
				res += new UriParameter("u", Uncertainty.Value.ToString()).ToString();
			if (!string.IsNullOrEmpty(CrsLabel))
				res += new UriParameter("crs", CrsLabel).ToString();
			return res;
		}
	}
}