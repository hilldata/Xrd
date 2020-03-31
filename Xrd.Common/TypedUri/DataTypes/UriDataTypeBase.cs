using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Xrd.TypedUri.DataTypes {
	/// <summary>
	/// Base class for Uri data types. Implements the core logic for 
	/// <see cref="INotifyPropertyChanged"/>
	/// </summary>
	/// <remarks>
	/// Use the <see cref="SetField{T}(ref T, T, string)"/> method to set 
	/// field values. This ensures that only actual changes are performed 
	/// and raises the PropertyChanged event.</remarks>
	public abstract class UriDataTypeBase : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Method called when properties are changed
		/// </summary>
		/// <param name="propName">The calling property</param>
		protected virtual void OnPropertyChanged([CallerMemberName] string propName = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

		/// <summary>
		/// Method used to set the value of the field underlying a property
		/// </summary>
		/// <typeparam name="T">The type of field/property</typeparam>
		/// <param name="field">The actual field.</param>
		/// <param name="value">The value being set.</param>
		/// <param name="propName">The calling property.</param>
		/// <returns></returns>
		protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propName = null) {
			if (EqualityComparer<T>.Default.Equals(field, value))
				return false;
			field = value;
			OnPropertyChanged(propName);
			return true;
		}
	}
}
