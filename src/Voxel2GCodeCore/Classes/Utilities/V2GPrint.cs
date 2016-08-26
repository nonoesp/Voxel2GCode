using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Geometry;
using System.IO;

namespace Voxel2GCodeCore.Utilities
{
    /// <summary>
    /// A class with 3D printing utilities and workflows.
    /// </summary>

    public class V2GPrint
    {
        public delegate double ThicknessIncrementDelegate(int index);

        /// <summary>
        /// Returns heights at which each slice should be printed for a given height
        /// </summary>
        /// <param name="height"></param>
        /// <param name="StartThickness"></param>
        /// <param name="ThicknessDel"></param>
        /// <param name="Thicknesses"></param>
        /// <param name="HeightFactor"></param>
        /// <param name="ThicknessIncrement"></param>
        /// <returns></returns>
        /// 
        public static List<double> ZHeights(double height, double StartThickness, ThicknessIncrementDelegate ThicknessDel, out List<double> Thicknesses, double HeightFactor = 1.0, double ThicknessIncrement = 0.0)
        {
            List<double> heights = new List<double>();
            List<double> thicknesses = new List<double>();

            double CurrentHeight = 0.005; // todo: Figure out why 0.0 flips the offset of the section curves
            double CurrentThickness = StartThickness;

            int i = 0;
            while (CurrentHeight < height)
            {
                heights.Add(CurrentHeight);
                if (ThicknessDel == null)
                {
                    CurrentThickness += ThicknessIncrement * i;
                }
                else
                {
                    CurrentThickness = StartThickness + ThicknessDel(i);
                }
                thicknesses.Add(CurrentThickness);
                CurrentHeight += V2GPrint.Height(CurrentThickness, HeightFactor);
                i++;
            }

            Thicknesses = thicknesses;

            return heights;
        }

        // Deprecated
        public static double Height(double _thickness, double _factor = 1.0)
        {
            return LayerHeight(_thickness, _factor);
        }

        public static double LayerHeight(double _thickness, double _factor = 1.0)
        {
            double FilamentDiameter = 1.75;
            double FilamentArea = Math.PI * Math.Pow((FilamentDiameter / 2), 2);
            double height = Math.Sqrt(FilamentArea * V2GPrint.EUnitIncrease(_thickness));
            height *= _factor;
            if (height < 0) return 0;
            return height * _factor;
        }

        public static double EUnitIncrease(double _thickness)
        {
            if (_thickness <= 0) return 0.0;
            return 0.033 * _thickness / 0.05;
        }

        public static void WriteStringToFilePath(string FilePath, string Text = "Intentionally left blank.")
        {
            using (StreamWriter outputFile = new StreamWriter(FilePath))
            {
                outputFile.Write(Text);
            }
        }
    
    }
}
