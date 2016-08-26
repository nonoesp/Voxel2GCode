using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Instructions;

namespace Voxel2GCodeCore
{
    /// <summary>
    ///  A class to hold a set of printable segments.
    /// </summary>
    public class V2GPath
    {
        /// <summary>
        /// A tag to identify an specific PrintPath which displays on the G-code file if the printer is in verbose mode.
        /// </summary>
        public string tag = "Untitled";

        /// <summary>
        /// A list of segments contained in the path.
        /// </summary>
        public List<V2GInstruction> Segments = new List<V2GInstruction>();

        /// <summary>
        /// Generate G-code instructions for the path.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="printer"></param>
        public void GenerateGCode(StringBuilder s, V2GState printer)
        {
            if (printer.settings.IsVerbose)
            {
                s.Append("\n; PrintPath.");
                if (this.tag != "Untitled") s.Append(" (" + this.tag + ".)");
                s.Append("(" + this.Segments.Count + " segments.)");
                s.Append("\n; -----");
            }
            int idx = 0;
            foreach (V2GInstruction ins in Segments)
            {
                ins.GenerateGCode(s, printer);
                idx++;
            }
            if (printer.settings.IsVerbose) s.Append("\n; -----");
        }
    }
}
