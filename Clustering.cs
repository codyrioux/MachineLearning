using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLearning {
    /// <summary>
    /// A class containing code related to clustering.
    /// </summary>
    public class Clustering {

        #region Data Structures
        /// <summary>
        /// A data structure used to store our clusters while processing. Clusters will be returned as this data structure, simple iterate over Members to find out what is in each cluster.
        /// </summary>
        /// <typeparam name="T">The type of data on which we are performing k means clustering.</typeparam>
        public class Cluster<T> {

            private Double[] _data;

            public Cluster(Double[] data) {
                _data = data;
                Members = new List<T>();
            }

            public void SetData(Double[] data) {
                _data = data;
            }

            public Double[] Data { get { return _data; } }
            public List<T> Members { get; set; }
        }
        #endregion

        #region Distance Algorithms
        /// <summary>
        /// Calculates the pearson distance between two data points. Requires more than one value in each vector, otherwise distance is always zero.
        /// </summary>
        /// <param name="v1">The first vector of data points.</param>
        /// <param name="v2">The second vector of data points.</param>
        /// <returns>The pearson distance score between two vectors.</returns>
        public static double PearsonDistance(double[] v1, double[] v2) {
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

        /// <summary>
        /// Calculates the manhattan distance between two vectors of double data points.
        /// </summary>
        /// <param name="v1">The first vector of data points for which to calculate manhattan distance.</param>
        /// <param name="v2">THe second vector of data points for which to calculate manhattan distance.</param>
        /// <returns>A double representing the manhattan distance between v1 and v2.</returns>
        public static double ManhattanDistance(double[] v1, double[] v2) {
            double sum = 0.0;

            if (v1.GetUpperBound(0) != v2.GetUpperBound(0))
                throw new System.ArgumentException("The number of elements in v1 must equal the number of elements in v2");

            for (int i = 0; i < v1.Length; ++i)
                sum += Math.Abs(v1[i] - v2[i]);

            return sum;
        }
        #endregion

        #region Clustering Algorithms
        /// <summary>
        /// This function will perform a K Means Cluster on the provided data set.
        /// </summary>
        /// <typeparam name="T">The type of data to be clustered.</typeparam>
        /// <param name="k">The number of clusters to use.</param>
        /// <param name="iterationCount">The number of iterations to perform.</param>
        /// <param name="rows">The set of data to cluster, must be IEnumerable with doubles representing the data points to cluster on.</param>
        /// <returns>A List of Cluster objects containing each of its members in the Members attribute.</returns>
        public static List<Cluster<T>> KCluster<T>(int k, int iterationCount, List<T> rows) where T : IClusterable {

            Random random = new Random();

            int MIN = 0;
            int MAX = 1;
            int attributeCount = rows[0].ClusteringData.Count();
            double highestMax = 0.0;

            // Find the min and max for each column
            Double[][] ranges = new Double[attributeCount][];
            for (int i = 0; i < attributeCount; ++i) {
                ranges[i] = new Double[] { 0, 0 };
            }

            foreach (var row in rows) {
                int count = 0;
                foreach (var value in row.ClusteringData) {
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
                        double d = ManhattanDistance(cluster.Data, row.ClusteringData);
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
                    averages = centroid.Data;
                    if (centroid.Members.Count != 0) {
                        for (int j = 0; j < attributeCount; ++j)
                            averages[j] = 0.0;

                        foreach (var member in centroid.Members) {
                            int index = 0;
                            foreach (var item in member.ClusteringData) {
                                averages[index++] += item;
                            }
                        }

                        for (int index = 0; index < attributeCount; ++index)
                            averages[index] /= centroid.Members.Count();
                    }

                    centroid.SetData(averages);
                    centroid.Members.Clear();
                }
            }
            return lastMatches;
        }

        #endregion
    }
}
