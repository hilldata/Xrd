using System.Collections.Generic;

namespace Xrd.vCard.Props {
	/// <summary>
	/// Base property definition where the value is a list of text values. Abstract class containing core methods.
	/// </summary>
	public abstract class ListStringProperty : PropertyBase<List<string>> {
		/// <summary>
		/// Default constructor
		/// </summary>
		public ListStringProperty() {
			Value = new List<string>();
		}

		/// <summary>
		/// Construct an instance of the property from the specified text input.
		/// </summary>
		/// <param name="input"></param>
		public ListStringProperty(string input) : base(input) { }

		internal ListStringProperty(string name, string parameters, string value) : base(name, parameters, value) { }

		/// <summary>
		/// Get/ser the string value from the list at the specified index.
		/// </summary>
		/// <param name="index">The 0-based index of the value to get/set.</param>
		/// <returns>The value at the specified index.</returns>
		public string this[int index] {
			get {
				if (Value == null)
					return null;
				else if (Value.Count > index)
					return Value[index];
				else
					return null;
			}
			set {
				// If Value is null, set it to a new list.
				if (Value == null)
					Value = new List<string>();


				// If the Value has at least as many members as the index, just set that member to the input.
				if (Value.Count > index)
					Value[index] = value;
				else {
					// If the Value does not have enough members to reach the index, add NULL until there are enough
					while (Value.Count < index)
						Value.Add(null);

					// Add the input as the last member.
					Value.Add(value);
				}
			}
		}

		/// <summary>
		/// A boolean indicating whether or not the instance represents a NULL value.
		/// </summary>
		public override bool IsValueNull {
			get {
				if (Value == null || Value.Count < 1)
					return true;
				return false;
			}
		}
	}
}