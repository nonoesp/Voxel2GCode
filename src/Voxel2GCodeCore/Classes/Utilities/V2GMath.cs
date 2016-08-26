using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel2GCodeCore.Utilities
{
    public class V2GMath
    {   
        /// <summary>
        /// Precision for floating-point comparisons.
        /// </summary>
        public static readonly double EPSILON = 0.0000000001;
        /// <summary>
        /// Constant to convert from degrees to radians.
        /// </summary>
        public const double DEGREES_TO_RADIANS = Math.PI / 180.0;
        /// <summary>
        /// Constant to convert from radians to degrees.
        /// </summary>
        public const double RADIANS_TO_DEGREES = 1 / DEGREES_TO_RADIANS;
    }
}
