using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Xrd.Text;

namespace Xrd.vCard.Params {
	/// <summary>
	/// Base property parameter definition. Abstract class containing core methods.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class PropertyParameter<T> : IPropertyParameter {
		#region Constructors
		internal PropertyParameter() { }

		internal PropertyParameter(KnownParameters paramType, KnownProperties propAssignedTo, T value) {
			Parameter = paramType;
			Name = paramType.GetvCardName();
			PropertyInfo pValu = GetType().GetRuntimeProperty("Value");
			pValu.SetValue(this, value);
			PropertyAssignedTo = propAssignedTo;
		}

		internal PropertyParameter(Tuple<string, string> pair) {
			if (pair == null)
				throw new ArgumentNullException(nameof(pair));

			Name = pair.Item1;
			if (Value is IList) {
				foreach (string s in pair.Item2.NonQuotedSplit(Constants.PARAM_MULTI_VAL_SEPARATOR, true))
					AddValue(IETFValueEncoding.DecodeParameterValue(s));
			} else
				SetValue(IETFValueEncoding.DecodeParameterValue(pair.Item2));
			Parameter = Name.ToKnownParameter();
		}
		#endregion

		internal void SetParameter(KnownParameters p) {
			Parameter = p;
			Name = p.GetvCardName();
			PropertyInfo prop = GetType().GetRuntimeProperty("Value");
			prop.SetValue(this, p.NullValue());
			PropertyAssignedTo = KnownProperties.unknown;
		}

		internal void ValidateAfterPropertyKnown() {
			if (Value is IList) {
				List<object> refList = Value as List<object>;
				if (Value != null || refList.Count > 0) {
					for (int i = refList.Count; i > 0; i--) {
						if (!IsValueAcceptable(refList[i - 1].ToString()))
							refList.RemoveAt(i);
					}
				}
			} else {
				if (!IsValueAcceptable(Value.ToString()))
					SetValue(null);
			}
		}

		#region Fields
		/// <summary>
		/// Get the KnownParameter represented by this instance.
		/// </summary>
		public KnownParameters Parameter { get; protected set; }
		/// <summary>
		/// Get the name of the parameter as read during parsing operation.
		/// </summary>
		public string Name { get; protected set; }
		/// <summary>
		/// The value of the parameter.
		/// </summary>
		public T Value { get; protected set; }
		protected KnownProperties _prop = KnownProperties.unknown;
		/// <summary>
		/// Gets the list of properties that this parameter can be assigned to.
		/// </summary>
		public KnownProperties PropertyAssignedTo {
			get => _prop;
			set {
				if (!_prop.Equals(value)) {
					_prop = value;
					ValidateAfterPropertyKnown();
				}
			}
		}

		/// <summary>
		/// List of acceptable values
		/// </summary>
		public virtual List<string> AcceptableValues => null;
		#endregion

		/// <summary>
		/// Virtual method used to determine if a read/input value is acceptable for the particular parameter.
		/// </summary>
		/// <param name="input">The input read from the text representation</param>
		/// <returns>A boolean indicating whether or not the input is acceptable.</returns>
		protected virtual bool IsValueAcceptable(string input) {
			if (AcceptableValues == null || AcceptableValues.Count < 1)
				return true;
			string check = input.Trim().ToLower();
			if (AcceptableValues.Contains(check))
				return true;

			foreach (string s in AcceptableValues) {
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
		/// Parse the input into a parameter.
		/// </summary>
		/// <param name="input">The parameter definition from the text representation</param>
		/// <returns>A concrete parameter instance.</returns>
		protected virtual object ParseInput(string input) {
			return input;
		}

		/// <summary>
		/// Attempt to set the value of the parameter.
		/// </summary>
		/// <param name="input">The string representation of the value to set.</param>
		public void SetValue(string input) {
			if (!IsValueAcceptable(input))
				return;
			object o = ParseInput(input);
			PropertyInfo prop = GetType().GetRuntimeProperty("Value");
			prop.SetValue(this, o);
		}

		internal void SetValue(T value) {
			PropertyInfo prop = GetType().GetRuntimeProperty("Value");
			prop.SetValue(this, value);
		}

		/// <summary>
		/// Attempt to add a new value (to List values)
		/// </summary>
		/// <param name="input">The string representation of the value to add.</param>
		public void AddValue(string input) {
			if (!IsValueAcceptable(input))
				return;

			object o = ParseInput(input);
			if (o != null) {
				var prop = GetType().GetRuntimeProperty("Value");
				List<object> v = prop.GetValue(this) as List<object>;
				v.Add(o);
				prop.SetValue(this, v);
			}
		}

		/// <summary>
		/// Outputs the parameter as a well-formed string according to the vCard specification https://tools.ietf.org/html/rfc6350
		/// </summary>
		/// <returns>A well-formed vCard parameter.</returns>
		public override string ToString() {
			if (Value.Equals(Parameter.NullValue()))
				return null;

			string res = string.Empty;
			if (Value is List<string>) {
				bool past1st = false;
				foreach (string s in Value as List<string>) {
					if (!string.IsNullOrWhiteSpace(s)) {
						if (past1st)
							res += Constants.PARAM_MULTI_VAL_SEPARATOR;
						else
							past1st = true;
						res += s;
					}
				}
			} else if (Value is string) {
				if (!string.IsNullOrWhiteSpace(Value as string))
					res += Value as string;
			} else if (Value is IList) {
				bool past1st = false;
				foreach (object o in Value as List<object>) {
					if (!string.IsNullOrWhiteSpace(o.ToString())) {
						if (past1st)
							res += Constants.PARAM_MULTI_VAL_SEPARATOR;
						else
							past1st = true;
						res += o.ToString();
					}
				}
			} else {
				if (!string.IsNullOrWhiteSpace(Value.ToString()))
					res += Value.ToString();
			}
			if (string.IsNullOrWhiteSpace(res))
				return null;
			return Parameter.GetvCardName() + Constants.PARAM_NAME_VAL_SEPARATOR + IETFValueEncoding.EncodeParameterValue(res);
		}

		/// <summary>
		/// Does this instance represent a NULL value?
		/// Implementation of the INullable interface.
		/// </summary>
		public virtual bool IsValueNull {
			get {
				if (typeof(INullable).IsAssignableFrom(Value.GetType()))
					return (Value as INullable).IsValueNull;
				if (Value is string)
					return string.IsNullOrWhiteSpace(Value as string);
				return Value == null;
			}
		}
	}
}
