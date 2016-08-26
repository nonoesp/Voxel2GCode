using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel2GCodeCore
{
    /// <summary>
    ///  A class to hold a set of paths contains in a slice.
    /// </summary>
    public class V2GSlice
    {
        /// <summary>
        /// A tag to identify an specific PrintSlice which displays on the G-code file if the printer is in verbose mode.
        /// </summary>
        public string tag = "Untitled";
        /// <summary>
        /// A list of paths.
        /// </summary>
        public List<V2GPath> Paths = new List<V2GPath>();
        /// <summary>
        /// Generate G-code instructions from the slice.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="printer"></param>
        public void GenerateGCode(StringBuilder s, V2GState printer)
        {
            s.Append("\n; PrintSlice");
            if (this.tag != "Untitled") s.Append(" (" + this.tag + ")");
            s.Append(".");
            foreach (V2GPath p in Paths)
            {
                p.GenerateGCode(s, printer);
            }
        }
    }


}
