using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineLearning.Clustering {
  /// <summary>
  /// Contains functions, data structures, and algorithms to perform clustering and calculate distances between
  /// IClusterable objects. Clustering can be used to find related pieces of data amongst a dataset with no relationships.
  /// 
  /// More information on Clustering from Wikipedia:
  /// Cluster analysis or clustering is the assignment of a set of observations into subsets (called clusters) so that observations
  /// in the same cluster are similar in some sense. Clustering is a method of unsupervised learning, and a common technique for 
  /// statistical data analysis used in many fields, including machine learning, data mining, pattern recognition, image analysis,
  /// information retrieval, and bioinformatics.
  /// </summary>
  public class Algorithms {

#region Data Structures
    /// <summary>
    /// A data structure used to store our clusters while processing.
    /// Clusters will be returned as this data structure, simple iterate over Members to find out what is in each cluster.
    /// </summary>
    /// <typeparam name="T">The type of data on which we are performing k means clustering.</typeparam>
    public class Cluster<T> {

      /// <summary>
      /// Constructor for the Cluster class.
      /// </summary>
      /// <param name="features">A Double[] representing the data points on which to cluster.</param>
      public Cluster(Double[] features) {
        Features = features;
        Members = new List<T>();
      }

      /// <summary>
      /// A Double[] exposed to allow clusters to have a "centroid" data point.
      /// </summary>
      public Double[] Features { get; set; }

      /// <summary>
      /// A list of IClusterable objects that belong to this cluster.
      /// </summary>
      public List<T> Members { get; set; }
    }
#endregion

#region Clustering Algorithms
    /// <summary>
    /// This function will perform a K Means Cluster on the provided data set.
    /// </summary>
    /// <typeparam name="T">The type of data to be clustered.</typeparam>
    /// <param name="k">The number of clusters to use.</param>
    /// <param name="iterationCount">The number of iterations to perform.</param>
    /// <param name="distance">A function that acceps two double[] and retuns a double representing the distance between the two vectors.</param>
    /// <param name="rows">The set of data to cluster, must be IEnumerable with doubles representing the data points to cluster on.</param>
    /// <returns>A List of Cluster objects containing each of its members in the Members attribute.</returns>
    public static List<Cluster<T>> KCluster<T>(int k, int iterationCount, Func<double[], double[], double> distance, List<T> rows) where T : IClusterable {

      if (rows.Count() == 0)
        return new List<Cluster<T>>();

      Random random = new Random();

      int MIN = 0;
      int MAX = 1;
      int attributeCount = rows[0].Features.Count();
      double highestMax = 0.0;

      // Find the min and max for each column
      Double[][] ranges = new Double[attributeCount][];
      for (int i = 0; i < attributeCount; ++i) {
        ranges[i] = new Double[] { 0, 0 };
      }

      foreach (var row in rows) {
        int count = 0;
        foreach (var value in row.Features) {
          if (value.CompareTo(ranges[count][MIN]) < 0)
            ranges[count][MIN] = value;
          if (value.CompareTo(ranges[count][MAX]) > 0) {
            ranges[count][MAX] = value;
            if (value.CompareTo(highestMax) > 0)
              highestMax = value;
          }
          ++count;
        }
      }

      // Create k randomly placed centroids
      List<Cluster<T>> centroids = new List<Cluster<T>>();
      for (int i = 0; i < k; ++i) {
        Double[] randomData = new Double[attributeCount];

        for (int column = 0; column < attributeCount; ++column) {
          randomData[column] = random.NextDouble() * (ranges[column][MAX] - ranges[column][MIN] + ranges[column][MIN]);
        }

        Cluster<T> newCluster = new Cluster<T>(randomData);
        centroids.Add(newCluster);
      }

      List<Cluster<T>> lastMatches = new List<Cluster<T>>();

      // Iterate iterationCount times or until done
      for (int i = 0; i < iterationCount; ++i) {

        // Find which centroid is closest for each row
        foreach (var row in rows) {

          double bestDistance = highestMax;
          Cluster<T> bestCluster = centroids[0];

          foreach (var cluster in centroids) {
            double d = distance(cluster.Features, row.Features);
            if (d < bestDistance) {
              bestDistance = d;
              bestCluster = cluster;
            }
          }
          bestCluster.Members.Add(row);
        }

        // If the results are the same as last time, this is complete
        if (lastMatches == centroids)
          break;
        lastMatches = centroids;

        // Move the centroids to the average of their members
        foreach (var centroid in centroids) {

          Double[] averages = new Double[attributeCount];
          averages = centroid.Features;
          if (centroid.Members.Count != 0) {
            for (int j = 0; j < attributeCount; ++j)
              averages[j] = 0.0;

            foreach (var member in centroid.Members) {
              int index = 0;
              foreach (var item in member.Features) {
                averages[index++] += item;
              }
            }

            for (int index = 0; index < attributeCount; ++index)
              averages[index] /= centroid.Members.Count();
          }

          centroid.Features = averages;
          centroid.Members.Clear();
        }
      }
      return lastMatches;
    }

#endregion
  }
}
