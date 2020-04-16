namespace Xrd.vCard.Props {
	/// <summary>
	/// Base property definition where the value is a single text value. Abstract class containing core methods.
	/// </summary>
	public abstract class StringProperty : PropertyBase<string> {
		public StringProperty() { }
		public StringProperty(string input) : base(input) { }
		internal StringProperty(string name, string parameters, string value) : base(name, parameters, value) { }
	}
}