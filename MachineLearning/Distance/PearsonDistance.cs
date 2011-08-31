using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineLearning.Distance {
  /// <summary>
  /// In statistics and in probability theory, distance correlation is a measure of statistical dependence between two random variables
  /// or two random vectors of arbitrary, not necessarily equal dimension. Its important property is that this measure of dependence is
  /// zero if and only if the random variables are statistically independent. This measure is derived from a number of other quantities
  /// that are used in its specification, specifically: distance variance, distance standard deviation and distance covariance.
  /// These take the same roles as the ordinary moments with corresponding names in the specification of the Pearson product-moment correlation coefficient.
  /// </summary>
  public static class PearsonDistance {

    /// <summary>
    /// Calculates the pearson distance between two data points. Requires more than one value in each vector, otherwise distance is always zero.
    /// </summary>
    /// <param name="v1">The first vector of data points.</param>
    /// <param name="v2">The second vector of data points.</param>
    /// <returns>The pearson distance score between two vectors.</returns>
    public static double Calculate(double[] v1, double[] v2) {
      double sum1 = v1.Sum();
      double sum2 = v2.Sum();

      double sum1Sq = 0.0;
      double sum2Sq = 0.0;

      foreach (var val in v1)
        sum1Sq += Math.Pow(val, 2);
      foreach (var val in v2)
        sum2Sq += Math.Pow(val, 2);

      double pSum = 0.0;
      for (int i = 0; i < v1.Count(); ++i)
        pSum += v1[i] * v2[i];

      double num = pSum - (sum1 * sum2 / v1.Count());
      double den = Math.Sqrt((sum1Sq - Math.Pow(sum1, 2) / v1.Count()) * (sum2Sq - Math.Pow(sum2, 2) / v1.Count()));
      if (den == 0)
        return 0;

      return 1.0 - num / den;
    }
  }
}
