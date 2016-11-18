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
            bool speedChanged = false;
            if(printer.F != this.PrintPosition.Speed)
            {
                printer.F = this.PrintPosition.Speed;
                speedChanged = true;
            }
            printer.SetPosition(this.PrintPosition, s);
            double length = printer.Position.DistanceTo(prevPrinterPosition);

            // Switch Extruder Head if Needed
            printer.SetHead(s, this.PrintPosition.Head);

            // Move to Start Point
            //s.Append("\nG1");
            //s.Append(" Z" + Math.Round(printer.Position.Z, 4));

            // Move and Extrude
            s.Append("\nG1");
            if (prevPrinterPosition.X != printer.Position.X) s.Append(" X" + Math.Round(printer.Position.X, 4));
            if (prevPrinterPosition.Y != printer.Position.Y) s.Append(" Y" + Math.Round(printer.Position.Y, 4));
            if (Math.Abs(prevPrinterPosition.Z - printer.Position.Z) > 0.0001) s.Append(" Z" + Math.Round(printer.Position.Z, 4));
            printer.IncreaseE(s, this.PrintPosition.MaterialAmount, length, this.PrintPosition.MixPercentage);
            if(speedChanged) s.Append(" F" + Math.Round(printer.F, 4));
        }

    }
}
