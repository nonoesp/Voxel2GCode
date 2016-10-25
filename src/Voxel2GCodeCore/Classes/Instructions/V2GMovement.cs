using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Geometry;

namespace Voxel2GCodeCore.Instructions
{
    /// <summary>
    ///  A printing instruction to move the tool-head without extruding material.
    /// </summary>
    public class V2GMovement : V2GInstruction
    {
        // TODO: Implement IsRelative behavior
        public V2GPrintPosition PrintPosition;
        //public double Speed = 700.0;
        public double MovementThreshold = 0.001;

        public V2GMovement(V2GPrintPosition printPosition, double speed)
        {
            this.PrintPosition = printPosition;
            this.PrintPosition.Speed = speed;
        }
        /// <summary>
        /// Generate G-code instructions for the segment.
        /// </summary>
        /// <param name="s">StringBuilder to store the G-code.</param>
        /// <param name="printer">Printer state.</param>
        public override void GenerateGCode(StringBuilder s, V2GState printer)
        {
            double MovementLength = printer.Position.DistanceTo(printer.PrintPointOnBase(this.PrintPosition));
            if (MovementLength > MovementThreshold)
            {
                if (printer.settings.IsVerbose) s.Append("\n(A. PrintMovement) (Printer has to move)");
                printer.F = this.PrintPosition.Speed;
                printer.SetPosition(this.PrintPosition, s);

                // Move
                s.Append("\nG1");
                s.Append(" X" + Math.Round(printer.Position.X, 4));
                s.Append(" Y" + Math.Round(printer.Position.Y, 4));
                s.Append(" Z" + Math.Round(printer.Position.Z, 4));
                s.Append(" F" + Math.Round(printer.F, 4));
            }
            else
            {
                if (printer.settings.IsVerbose) s.Append("\n (A. PrintMovement) (Printer is already here)");
            }
        }
    }
}
