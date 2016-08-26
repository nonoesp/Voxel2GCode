using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel2GCodeCore.Geometry
{
    /// <summary>
    ///  This struct holds a printable point
    /// </summary>
    public class V2GPrintPosition : V2GPrintable
    {
        /// <summary>
        /// Position to print toward.
        /// </summary>
        public V2GPoint Position;
        /// <summary>
        /// Default feed rate to move and extrude, i.e. velocity in mm/min.
        /// </summary>
        public double Speed = 700.0;
        /// <summary>
        /// Default amount of material to be fed per mm. (If using a dual extruder, the material distributes among using the MixPercentage value.)
        /// </summary>
        public double MaterialAmount = 0.033;
        /// <summary>
        /// Extruder-head to be used to print this. (e.g. T0 for left extruder, T1 for right extruder, T3 for dual extruder, etc.) This values vary from printer to printer.
        /// </summary>
        public int Head = 1;
        /// <summary>
        /// Percentage of material to extrude with the secondary extruder.
        /// </summary>
        public double MixPercentage = 0.0;
        /// <summary>
        /// Create a point from XYZ coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public V2GPrintPosition(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        public V2GPrintPosition(V2GPoint position)
        {
            this.X = position.X;
            this.Y = position.Y;
            this.Z = position.Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="speed"></param>
        /// <param name="materialAmount"></param>
        /// <param name="head"></param>
        /// <param name="mixPercentage"></param>
        public V2GPrintPosition(V2GPoint position, double speed, double materialAmount, int head, double mixPercentage)
        {
            this.Position = position;
            this.Speed = speed;
            this.MaterialAmount = materialAmount;
            this.Head = head;
            this.MixPercentage = mixPercentage;
        }

        public double X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }

        public double Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }

        public double Z
        {
            get { return Position.Z; }
            set { Position.Z = value; }
        }

        /// <summary>
        /// Returns the distance of this PrintPoint to another.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public double DistanceTo(V2GPrintPosition p)
        {
            return Math.Sqrt(
                (this.X - p.X) * (this.X - p.X) +
                (this.Y - p.Y) * (this.Y - p.Y) +
                (this.Z - p.Z) * (this.Z - p.Z)
                );
        }

        public static V2GPrintPosition operator +(V2GPrintPosition p1, V2GPrintPosition p2)
        {
            return new V2GPrintPosition(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static V2GPrintPosition operator -(V2GPrintPosition p)
        {
            return new V2GPrintPosition(-p.X, -p.Y, -p.Z);
        }

        public static V2GPrintPosition operator -(V2GPrintPosition p1, V2GPrintPosition p2)
        {
            return new V2GPrintPosition(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static V2GPrintPosition operator *(Double s, V2GPrintPosition p)
        {
            return new V2GPrintPosition(s * p.X, s * p.Y, s * p.Z);
        }

        public static V2GPrintPosition operator *(V2GPrintPosition p, Double s)
        {
            return new V2GPrintPosition(s * p.X, s * p.Y, s * p.Z);
        }

        public static V2GPrintPosition operator +(V2GPrintPosition pp, V2GPoint p)
        {
            return new V2GPrintPosition(pp.X + p.X, pp.Y + p.Y, pp.Z + p.Z);
        }

        public static V2GPrintPosition operator -(V2GPrintPosition pp, V2GPoint p)
        {
            return new V2GPrintPosition(pp.X - p.X, pp.Y - p.Y, pp.Z - p.Z);
        }

        /// <summary>
        /// Add the coordinates of specified V2GPrintPoint to this one.
        /// </summary>
        /// <param name="p"></param>
        public void Add(V2GPrintPosition p)
        {
            this.X += p.X;
            this.Y += p.Y;
            this.Z += p.Z;
        }

        /// <summary>
        /// Add the coordinates of specified V2GPoint to this one.
        /// </summary>
        /// <param name="p"></param>
        public void Add(V2GPoint p)
        {
            this.X += p.X;
            this.Y += p.Y;
            this.Z += p.Z;
        }

        /// <summary>
        /// Substract the coordinates of specified V2GPrintPoint to this one.
        /// </summary>
        /// <param name="p"></param>
        public void Substract(V2GPrintPosition p)
        {
            this.X += -p.X;
            this.Y += -p.Y;
            this.Z += -p.Z;
        }

        /// <summary>
        /// Substract the coordinates of specified V2GPoint to this one.
        /// </summary>
        /// <param name="p"></param>
        public void Substract(V2GPoint p)
        {
            this.X += -p.X;
            this.Y += -p.Y;
            this.Z += -p.Z;
        }
    }
}
