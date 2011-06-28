using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpLearning;

namespace SharpLearning.Tests {
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
        public void TestPearsonDistance() {
            double[] v1 = { 1, 3, 5, 7, 9 };
            double[] v2 = { 2, 4, 6, 8, 10 };

            Assert.AreEqual(0.0, Clustering.PearsonDistance(v1, v2));

            v1 = new Double[] { 1, 2, 3 };
            v2 = new Double[] { 3, 2, 1 };
            Assert.AreEqual(2.0, Clustering.PearsonDistance(v1, v2));

            v1 = new Double[] { 1.12212 };
            v2 = new Double[] { 41212.2222 };
            Assert.AreEqual(0.0, Clustering.PearsonDistance(v1, v2));

            v1 = new Double[] { 1.1, 1.22, 1.756, 0.9 };
            v2 = new Double[] { 1.4, 0.95, 1.05, 1.2 };
            Assert.AreEqual(1.4634, Math.Round(Clustering.PearsonDistance(v1, v2), 4));

            v1 = new Double[] {-4.4, -1, 0, 1.24 };
            v2 = new Double[] {2.11, 3, -2.0, -1.5 };
            Assert.AreEqual(1.6815, Math.Round(Clustering.PearsonDistance(v1, v2), 4));
        }

        [TestMethod]
        public void TestManhattanDistance() {
            // Todo: Find a known working manhattan distance calculator to compare results against.
        }

        [TestMethod]
        public void TestKClusterImport() {
            Random random = new Random();
            List<ClusterableObject> testObjects = new List<ClusterableObject>();

            // Generate a grouping simulating a JMS import (large set of data points cenetred around a small spread)
            for (int i = 0; i < 35000; ++i) {
                int numberOfDataPoints = random.Next(1, 20);
                testObjects.Add(new ClusterableObject(i, new Double[] { random.NextDouble() * 100 }));
            }

            // Generate some "other" objects.
            for (int i = 0; i < 300; ++i) {
                testObjects.Add(new ClusterableObject(i + 35000, new Double[] { random.Next() * 10000 }));
            }

            var results = Clustering.KCluster(2, 100, testObjects);

            Console.WriteLine(results.Count);
            foreach (var cluster in results)
                Console.WriteLine(cluster.Members.Count);
        }
    }
}
