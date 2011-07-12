using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MachineLearning.Clustering;

namespace MachineLearning.Test {
	[TestClass]
    public class ClusteringTests {

        #region Helpers
        private class ClusterableObject : IClusterable {

            private Int32 _id;
            private Double[] _data;

            public ClusterableObject(int id, Double[] data) {
                _id = id;
                _data = data;
            }

            Double[] IClusterable.ClusteringData { get { return _data; } }
            public Int32 Id { get { return _id; } }
        }
        #endregion

        [TestMethod]
        public void TestKClusterLargeGroup() {
            Random random = new Random();
            List<ClusterableObject> testObjects = new List<ClusterableObject>();

			// Generate a large group of data centered around a small area.
            for (int i = 0; i < 35000; ++i) {
                testObjects.Add(new ClusterableObject(i, new Double[] { random.NextDouble() }));
            }

            // Generate some "other" objects over a larger spread.
            for (int i = 0; i < 1000; ++i) {
                testObjects.Add(new ClusterableObject(i + 35000, new Double[] { random.Next() * 1000 }));
            }

            var clusters = MachineLearning.Clustering.Algorithms.KCluster(
				100,
				20,
				(Func<double[], double[], double>)MachineLearning.Distance.ManhattanDistance.Calculate,
				testObjects);
			
			int mean = 0;
			double sum = 0;
			foreach (var cluster in clusters) {
				mean += cluster.Members.Count;
			}

			int n = mean;
			mean /= clusters.Count;

			foreach (var cluster in clusters)
				sum += (cluster.Members.Count - mean);

			double stdDeviation = Math.Pow(sum, 2) / (n - 1);

			// Verify that there is one HUGE cluster in comparison to the rest.
			Assert.IsTrue(clusters.Any(c => c.Members.Count > mean + stdDeviation * 5.0));
        }
    }
}
