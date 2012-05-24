using System;
using System.Collections.Generic;
using System.Linq;
using MachineLearning.Util;

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

# region Clustering Algorithms

    private static readonly int MIN = 0;
    private static readonly int MAX = 1;

    /// <summary>
    /// This function will perform a K Means Cluster on the provided data set.
    /// </summary>
    /// <typeparam name="T">The type of data to be clustered.</typeparam>
    /// <param name="k">The number of clusters to use.</param>
    /// <param name="maxIterations">The number of iterations to perform.</param>
    /// <param name="distance">A function that acceps two double[] and retuns a double representing the distance between the two vectors.</param>
    /// <param name="rows">The set of data to cluster, must be IEnumerable with doubles representing the data points to cluster on.</param>
    /// <returns>A List of Cluster objects containing each of its members in the Members attribute.</returns>
    public static List<Cluster<T>> KCluster<T>(int k, int maxIterations, Func<double[], double[], double> distance, List<T> rows) where T : IClusterable {

      if (rows.Count() == 0)
        return new List<Cluster<T>>();

      Random random = new Random();
      int featureCount = rows.First().Features.Count();
      List<Cluster<T>> centroids = new List<Cluster<T>>();
      List<Cluster<T>> lastMatches = new List<Cluster<T>>();

      // Find the min and max for each column
      Double[][] ranges = new Double[featureCount][];
			for(int featureIndex = 0; featureIndex < featureCount; ++featureIndex) {
				ranges[featureIndex][MIN] 	= rows.Select(row => row.Features[featureIndex]).ToList().Min();
				ranges[featureIndex][MAX] 	= rows.Select(row => row.Features[featureIndex]).ToList().Max();
			}

      // Create k randomly placed centroids
      for (int i = 0; i < k; ++i) {
        Double[] randomData = new Double[featureCount];
        for (int column = 0; column < featureCount; ++column) {
          randomData[column] = random.NextDouble() * (ranges[column][MAX] - ranges[column][MIN] + ranges[column][MIN]);
        }
        centroids.Add(new Cluster<T>(randomData));
      }

      // Iterate iterationCount times or until done
      for (int i = 0; i < maxIterations; ++i) {
        // Find which centroid is closest for each row and add it to that centroids members
				rows.ForEach(row =>
						centroids.Aggregate((closest, centroid) => (closest == null || distance(centroid.Features, row.Features) < distance(closest.Features, row.Features) ? centroid : closest)).Do(c => c.Members.Add(row)));

        // If the results are the same as last time then return the list
        if (lastMatches == centroids)
          break;
        lastMatches = centroids;

				centroids.ForEach(centroid => {
					for(int featureIndex = 0; featureIndex < featureCount; ++featureIndex) {
					  centroid.Features[featureIndex] = centroid.Members.Select(item => item.Features[featureIndex]).ToList().Average();
					}
					centroid.Members.Clear();
				});
      }
      return lastMatches;
    }
#endregion
  }
}