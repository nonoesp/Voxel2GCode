using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Geometry;

namespace Voxel2GCodeCore.Instructions
{
    /// <summary>
    /// A print instruction to extrude material along a segment.
    /// </summary>
    public class V2GPrintSegment : V2GInstruction
    {
        /// <summary>
        /// The destination point of the segment.
        /// </summary>
        public V2GPrintPosition PrintPosition;

        public V2GPrintSegment(V2GPrintPosition printPosition) {
            this.PrintPosition = printPosition;
        }

        /// <summary>
        /// Generate G-code instructions for the segment.
        /// </summary>
        /// <param name="s">StringBuilder to store the G-code.</param>
        /// <param name="printer">Printer state.</param>
        public override void GenerateGCode(StringBuilder s, V2GState printer)
        {
            V2GPrintPosition prevPrinterPosition = printer.Position;
            printer.F = this.PrintPosition.Speed;
            printer.SetPosition(this.PrintPosition, s);
            double length = printer.Position.DistanceTo(prevPrinterPosition);
      
            // Switch Extruder Head if Needed
            printer.SetHead(s, this.PrintPosition.Head);

            // Move to Start Point
            s.Append("\nG1");
            s.Append(" Z" + Math.Round(printer.Position.Z, 4));

            // Move and Extrude
            s.Append("\nG1");
            s.Append(" X" + Math.Round(printer.Position.X, 4));
            s.Append(" Y" + Math.Round(printer.Position.Y, 4));
            s.Append(" Z" + Math.Round(printer.Position.Z, 4));
            printer.IncreaseE(s, this.PrintPosition.MaterialAmount, length, this.PrintPosition.MixPercentage);
            s.Append(" F" + Math.Round(printer.F, 4));

            s.Append("\n(Debug: printer.ZOffset:"+printer.settings.ZOffset+", printer.Position.Z: "+printer.Position.Z+")");
        }

    }
}
