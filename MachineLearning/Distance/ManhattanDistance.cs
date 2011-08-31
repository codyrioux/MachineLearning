using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineLearning.Distance {
  /// <summary>
  /// Taxicab geometry, considered by Hermann Minkowski in the 19th century, is a form of geometry.
  /// Here, in comparison to Euclidean geometry, the usual distance function or metric is replaced
  /// by a new metric in which the distance between two points is the sum of the absolute differences
  /// of their coordinates. Also known as Manhattan Distance.
  /// </summary>
  public static class ManhattanDistance {
    /// <summary>
    /// Calculates the manhattan distance between two vectors of double data points.
    /// </summary>
    /// <param name="v1">The first vector of data points for which to calculate manhattan distance.</param>
    /// <param name="v2">THe second vector of data points for which to calculate manhattan distance.</param>
    /// <returns>A double representing the manhattan distance between v1 and v2.</returns>
    public static double Calculate(double[] v1, double[] v2) {
      double sum = 0.0;

      if (v1.GetUpperBound(0) != v2.GetUpperBound(0))
        throw new System.ArgumentException("The number of elements in v1 must equal the number of elements in v2");

      for (int i = 0; i < v1.Length; ++i)
        sum += Math.Abs(v1[i] - v2[i]);

      return sum;
    }
  }
}
