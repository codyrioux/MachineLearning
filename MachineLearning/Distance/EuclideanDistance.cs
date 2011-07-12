using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineLearning.Distance {
	/// <summary>
	/// In mathematics, the Euclidean distance or Euclidean metric is the "ordinary" distance between two points
	/// that one would measure with a ruler, and is given by the Pythagorean formula. By using this formula as distance,
	/// Euclidean space (or even any inner product space) becomes a metric space. The associated norm is called the
	/// Euclidean norm. Older literature refers to the metric as Pythagorean metric.
	/// </summary>
	public static class EuclideanDistance {
		/// <summary>
		/// Calculates the euclidean distance between two vectors of double data points.
		/// </summary>
		/// <param name="v1">The first vector for which to calculate euclidean distance.</param>
		/// <param name="v2">The second vector for which to calculate euclidean distance.</param>
		/// <returns>A double representing the euclidean distance between v1 and v2.</returns>
		public static double Calculate(double[] v1, double[] v2) {
			double sum = 0.0;

            if (v1.GetUpperBound(0) != v2.GetUpperBound(0))
                throw new System.ArgumentException("The number of elements in v1 must equal the number of elements in v2");

			for (int i = 0; i < v1.Length; ++i)
				sum += Math.Pow(v1[i] - v2[i], 2);

			return Math.Sqrt(sum);
		}
	}
}
