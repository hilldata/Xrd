using System;
using System.Collections.Generic;

namespace Xrd.TypedUri {
	public interface ITypedUri :  INullable {
		Uri Uri { get; }
		List<UriParameter> OtherParameters { get; }
	}
}
