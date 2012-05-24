using System;

namespace MachineLearning.Util {
	public static class Extensions {
		public static T Do<T>(this T obj, Action<T> action) {
			action(obj);
			return obj;
		}
	}
}
