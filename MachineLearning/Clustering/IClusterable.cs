using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineLearning.Clustering {
  /// <summary>
  /// Objects that implement this interface expose a Double[] named ClusteringData that can be used
  /// to perform clustering on the object.
  /// </summary>
  public interface IClusterable {

    /// <summary>
    /// A Double[] filled with data points that allow the object to have it's distance calculated from others.
    /// </summary>
    Double[] ClusteringData { get; }
  }
}
