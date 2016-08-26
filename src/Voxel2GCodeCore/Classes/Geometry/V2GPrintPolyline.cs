using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voxel2GCodeCore.Geometry
{
    /// <summary>
    /// A printable polyline with printing information.
    /// </summary>
    public class V2GPrintPolyline : V2GPrintable
    {
        /// <summary>
        /// Set of printable points to print along.
        /// </summary>
        public List<V2GPrintPosition> PrintPositions = new List<V2GPrintPosition>();
        /// <summary>
        /// Default feed rate to move and extrude, i.e. velocity in mm/min.
        /// </summary>
        double Speed = 700.0;
        /// <summary>
        /// Default amount of material to be fed per mm. (If using a dual extruder, the material distributes among using the MixPercentage value.)
        /// </summary>
        double MaterialAmount = 0.033;
        /// <summary>
        /// Extruder-head to be used to print this. (e.g. T0 for left extruder, T1 for right extruder, T3 for dual extruder, etc.) This values vary from printer to printer.
        /// </summary>
        int Head = 1;
        /// <summary>
        /// Percentage of material to extrude with the secondary extruder.
        /// </summary>
        double MixPercentage = 0.0;

        public void AddPrintPosition(V2GPoint position, double speed, double materialAmount, int head, double mixPercentage)
        {
            this.Speed = speed;
            this.MaterialAmount = materialAmount;
            this.Head = head;
            this.MixPercentage = mixPercentage;
            this.PrintPositions.Add(new V2GPrintPosition(position, speed, materialAmount, head, mixPercentage));
        }

        public void AddPrintPosition(V2GPrintPosition printPosition)
        {
            this.Speed = printPosition.Speed;
            this.MaterialAmount = printPosition.MaterialAmount;
            this.Head = printPosition.Head;
            this.MixPercentage = printPosition.MixPercentage;
            this.PrintPositions.Add(printPosition);
        }

        public void AddPrintPosition(V2GPoint position)
        {
            this.PrintPositions.Add(new V2GPrintPosition(position));
        }

        // add variables to constructor

        /*
        // Tentative
        // Implement a string or string[] of segment types to apply effects to each of the segments
        // public string SegmentType = "PRINT_SEGMENT_2";
        // public string[] SegmentTypes; // this could be to input a series of iterating types (wavy, segment2, [repeat])

        // TODO: Implement a Generate instructions method inside each printable
        public override void GenerateInstructions(PrintModel model)
        {
            //base.GenerateInstructions();
            PrintPath path = new PrintPath();

            //PrintSegment segment = new PrintSegment2();
            //segment.p = 

            //path.Segments.Add(new PrintSegment2());
            //model.Pahts.Add(new Path);
            // Append self as paths
        }
        */
    }
}
