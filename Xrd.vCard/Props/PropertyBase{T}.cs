using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Xrd.Collections;
using Xrd.Text;
using Xrd.vCard.Params;

namespace Xrd.vCard.Props {
	/// <summary>
	/// Base property definition. Abstract class containing core methods.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class PropertyBase<T> : IProperty {
		#region Properties
		/// <summary>
		/// The KnownProperty represented by the instance.
		/// </summary>
		public KnownProperties Property { get; protected set; } = KnownProperties.read_not_parsed_yet;
		/// <summary>
		/// The property group the property belongs to. (not actually described in the standard.
		/// </summary>
		public string Group { get; protected set; } = string.Empty;
		/// <summary>
		/// The name of the property.
		/// </summary>
		public string Name { get; protected set; } = string.Empty;
		/// <summary>
		/// The strongly-typed value of the property
		/// </summary>
		public T Value { get; set; }
		/// <summary>
		/// The raw value (as a string) of the property.
		/// </summary>
		public string RawValue { get; private set; }
		/// <summary>
		/// List of acceptable (per the standard) Parameters that have been defined for the property.
		/// </summary>
		public List<IPropertyParameter> Parameters { get; protected set; } = new List<IPropertyParameter>();
		/// <summary>
		/// List of unaccetable (per the standard) Parameters that have been defined for the property.
		/// </summary>
		public List<IPropertyParameter> INVALID_Params { get; protected set; } = new List<IPropertyParameter>();
		/// <summary>
		/// Character that is used to separate the Name/Value portion of the property string.
		/// </summary>
		public virtual char VALUE_SEPARATOR { get { return ';'; } }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PropertyBase() {
			SetProperty(KnownProperties.unknown);
		}
		/// <summary>
		/// internal method used to define the known property and then apply the standard-defined property name
		/// </summary>
		/// <param name="p"></param>
		protected void SetProperty(KnownProperties p) {
			Property = p;
			Name = p.GetvCardName();
		}

		/// <summary>
		/// Construct a property from the specified string input.
		/// </summary>
		/// <param name="input">A string representation of the property as read from a vCard object.</param>
		internal PropertyBase(string input) {
			// Make sure input is not empty
			if (string.IsNullOrWhiteSpace(input))
				throw new ArgumentNullException("input");

			// Split out the name/value.
			if (input.NonQuotedIndexOf(Constants.PROP_NAME_VAL_SEPARATOR) < 0)
				throw new ArgumentOutOfRangeException("input", $"No name/value separator [{Constants.PROP_NAME_VAL_SEPARATOR}] is could be found. Improperly formed input.");
			var nameValue = input.NonQuotedSplitOnFirst(Constants.PROP_NAME_VAL_SEPARATOR);
			nameValue.Item1.UnQuote();

			// Separate out the name/parameters
			if (nameValue.Item1.NonQuotedIndexOf(Constants.PROP_PARAMETER_SEPARATOR) > 0) {
				var nameParam = nameValue.Item1.NonQuotedSplitOnFirst(Constants.PROP_PARAMETER_SEPARATOR);
				// Parse the name
				ParseNamePart(nameParam.Item1);
				foreach (string s in nameParam.Item2.NonQuotedSplit(Constants.PROP_PARAMETER_SEPARATOR, false)) {
					try {
						AddParameter(s.ParseParam());
					} catch { }
				}
			} else {
				// No parameters, just parse the name
				ParseNamePart(nameValue.Item1);
			}

			// Set the RawValue
			RawValue = nameValue.Item2;

			// Process the values
			if (Value is IList) {
				if (Value == null)
					Value = Activator.CreateInstance<T>();
				foreach (string s in nameValue.Item2.NonQuotedSplit(VALUE_SEPARATOR, true))
					AddValue(IETFValueEncoding.DecodeValue(s));
			} else
				SetValue(IETFValueEncoding.DecodeValue(nameValue.Item2));
		}

		internal PropertyBase(string name, string parameters, string value) {
			Name = name;
			if (!string.IsNullOrWhiteSpace(parameters)) {
				foreach (string s in parameters.NonQuotedSplit(Constants.PROP_PARAMETER_SEPARATOR, false)) {
					try {
						AddParameter(s.ParseParam());
					} catch { }
				}
			}

			// Set RawValue
			RawValue = value;
			if (!string.IsNullOrWhiteSpace(value)) {
				// Process the values
				if (Value is IList) {
					if (Value == null)
						Value = Activator.CreateInstance<T>();
					foreach (string s in value.NonQuotedSplit(VALUE_SEPARATOR, true))
						AddValue(IETFValueEncoding.DecodeValue(s));
				} else
					SetValue(IETFValueEncoding.DecodeValue(value));
			} else
				SetValue(default);
		}

		private void ParseNamePart(string input) {
			if (string.IsNullOrWhiteSpace(input))
				throw new ArgumentException("The input is not properly formed. No property name was provided.");

			if (input.NonQuotedIndexOf(Constants.PROP_GROUP_NAME_SEPARATOR) > 0) {
				var p = input.NonQuotedSplitOnFirst(Constants.PROP_GROUP_NAME_SEPARATOR);
				Name = p.Item2.UnQuote();
				Group = p.Item1.UnQuote();
			} else {
				Group = string.Empty;
				Name = input.Trim().UnQuote();
			}
		}

		/// <summary>
		/// Parse the provided text input.
		/// </summary>
		/// <param name="input">A string representation of the property as defined in the vCard object.</param>
		/// <returns>A strongly typed property.</returns>
		protected virtual object ParseValue(string input) {
			return input;
		}

		/// <summary>
		/// Determine if the specified input is an acceptable value for the property.
		/// </summary>
		/// <param name="input">The value to set/add.</param>
		/// <returns>A boolean indicating whether or not the value is valid for this property.</returns>
		protected virtual bool IsValueAcceptable(string input) {
			// Default check => If no AcceptableValues are provided, IsOK if the input is not null/empty
			if (AcceptableValues == null || AcceptableValues.Count < 1)
				return !string.IsNullOrWhiteSpace(input);
			string check = input.Trim().ToLower();
			if (AcceptableValues.Contains(check))
				return true;

			foreach (string s in AcceptableValues) {
				if (s == string.Empty)
					return true;

				if (s.StartsWith(Constants.WILDCARD)) {
					if (s.EndsWith(Constants.WILDCARD)) {
						if (check.Contains(s.Replace(Constants.WILDCARD, string.Empty)))
							return true;
					} else {
						if (check.EndsWith(s.Replace(Constants.WILDCARD, string.Empty)))
							return true;
					}
				} else {
					if (s.EndsWith(Constants.WILDCARD)) {
						if (check.StartsWith(s.Replace(Constants.WILDCARD, string.Empty)))
							return true;
					} else {
						if (check.Equals(s.Replace(Constants.WILDCARD, string.Empty)))
							return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Add the specified value to the properties value (if type is a List)
		/// </summary>
		/// <param name="input">The value to add.</param>
		protected void AddValue(string input) {
			if (!IsValueAcceptable(input))
				return;

			object o = ParseValue(input);
			if (o != null) {
				var prop = GetType().GetRuntimeProperty("Value");
				if (Value == null)
					Value = Activator.CreateInstance<T>();
				List<object> v = prop.GetValue(this) as List<object>;
				v.Add(o);
				prop.SetValue(this, v);
			}
		}

		/// <summary>
		/// Set the property's value to the specified input (if type is NOT a list).
		/// </summary>
		/// <param name="input">The value to set.</param>
		protected void SetValue(string input) {
			if (!IsValueAcceptable(input))
				return;
			object o = ParseValue(input);
			PropertyInfo prop = GetType().GetRuntimeProperty("Value");
			prop.SetValue(this, o);
		}

		/// <summary>
		/// Add a parameter to the property
		/// </summary>
		/// <param name="param">The parameter to add</param>
		protected void AddParameter(IPropertyParameter param) {
			if (AllowedParameters.HasFlag(param.Parameter)) {
				Parameters.Add(param);
				param.PropertyAssignedTo = Property;
			} else {
				INVALID_Params.Add(param);
			}
		}

		public List<IPropertyParameter> GetParameters(string name) {
			var qry = from p
					  in Parameters
					  where p.Name.Equals(name)
					  select p;

			return qry.ToList();
		}

		public List<IPropertyParameter> GetParameters(KnownParameters paramType) {
			var qry = from p
					  in Parameters
					  where p.Parameter == paramType
					  select p;

			return qry.ToList();
		}

		/// <summary>
		/// List of the Parameters that can be applied to the property (per the standard)
		/// </summary>
		public KnownParameters AllowedParameters { get { return Property.GetAllowedParameters(); } }
		/// <summary>
		/// List of values that are acceptable.
		/// </summary>
		public virtual List<string> AcceptableValues { get { return null; } }

		/// <summary>
		/// Gets a boolean indicating if the property instance represents a NULL value.
		/// </summary>
		public virtual bool IsValueNull {
			get {
				if (typeof(INullable).IsAssignableFrom(Value.GetType()))
					return (Value as INullable).IsValueNull;
				if (Value is string)
					return string.IsNullOrWhiteSpace(Value as string);
				if (Value is IEnumerable<string>) {
					return Value != null && (Value as IEnumerable<string>).Count() > 0;
				}
				return Value == null;
			}
		}

		public byte? GetPref() {
			if (!(Parameters.Where(p => p.Parameter == KnownParameters.PREF).FirstOrDefault() is PREF pref))
				return null;
			else
				return pref.Value;
		}

		public List<string> GetTypes() {
			List<string> res = new List<string>();
			foreach (var v in Parameters.Where(p => p.Parameter == KnownParameters.TYPE).ToList()) {
				if ((v is TYPE type))
					res.AddRangeIfNotNull(type.Value);
			}
			return res;
		}
	}
}