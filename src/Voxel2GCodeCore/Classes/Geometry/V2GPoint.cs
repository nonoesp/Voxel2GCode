using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Utilities;

namespace Voxel2GCodeCore.Geometry
{
    /// <summary>
    /// A generic point class.
    /// </summary>
    public struct V2GPoint
    {
        public double X, Y, Z;

        /// <summary>
        /// Create a Point from its XYZ coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public V2GPoint(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Creates a shallow copy of the specified Point.
        /// </summary>
        /// <param name="p"></param>
        public V2GPoint(V2GPoint p)
        {
            this.X = p.X;
            this.Y = p.Y;
            this.Z = p.Z;
        }

        /// <summary>
        /// Returns the distance of this PrintPoint to another.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public double DistanceTo(V2GPoint p)
        {
            return Math.Sqrt(
                (this.X - p.X) * (this.X - p.X) +
                (this.Y - p.Y) * (this.Y - p.Y) +
                (this.Z - p.Z) * (this.Z - p.Z)
                );
        }

        public static V2GPoint operator +(V2GPoint p1, V2GPoint p2)
        {
            return new V2GPoint(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static V2GPoint operator -(V2GPoint p)
        {
            return new V2GPoint(-p.X, -p.Y, -p.Z);
        }

        public static V2GPoint operator -(V2GPoint p1, V2GPoint p2)
        {
            return new V2GPoint(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static V2GPoint operator *(Double s, V2GPoint p)
        {
            return new V2GPoint(s * p.X, s * p.Y, s * p.Z);
        }

        public static V2GPoint operator *(V2GPoint p, Double s)
        {
            return new V2GPoint(s * p.X, s * p.Y, s * p.Z);
        }

        /// <summary>
        /// Unit X Vector.
        /// </summary>
        public static V2GPoint XAxis = new V2GPoint(1, 0, 0);

        /// <summary>
        /// Unit Y Vector.
        /// </summary>
        public static V2GPoint YAxis = new V2GPoint(0, 1, 0);

        /// <summary>
        /// Unit Z Vector.
        /// </summary>
        public static V2GPoint ZAxis = new V2GPoint(0, 0, 1);

        /// <summary>
        /// Returns the length of this Vector.
        /// </summary>
        /// <returns></returns>
        public double Length()
        {
            return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }
        
        /// <summary>
        /// Unitizes this Vector. Will return false if Vector is not unitizable
        /// (zero length Vector).
        /// </summary>
        /// <returns></returns>
        public bool Normalize()
        {
            double len = this.Length();
            if (len < V2GMath.EPSILON) return false;
            this.X /= len;
            this.Y /= len;
            this.Z /= len;
            return true;
        }

        /// <summary>
        /// Returns the <a href="https://en.wikipedia.org/wiki/Cross_product">Cross Product</a>
        /// of specified Vectors (Points).
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static V2GPoint CrossProduct(V2GPoint p1, V2GPoint p2)
        {
            return new V2GPoint(
                p1.Y * p2.Z - p1.Z * p2.Y,
                p1.Z * p2.X - p1.X * p2.Z,
                p1.X * p2.Y - p1.Y * p2.X);
        }
    }
}
