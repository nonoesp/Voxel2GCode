using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel2GCodeCore.Utilities;
using Rhino.Geometry;

namespace Voxel2GCodeRhino.Utilities
{
    /// <summary>
    /// Test to see if we can add extension methods to core through a different project.
    /// </summary>
    public static class V2GRhinoGeometry
    {
        /// <summary>
        /// Extract bounding Z frames -- slice planes -- of a given geometry.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="XSpan"></param>
        /// <param name="YSpan"></param>
        /// <param name="ZSpan"></param>
        /// <param name="DXSpan"></param>
        /// <param name="DYSpan"></param>
        /// <param name="AngleDegrees"></param>
        /// <returns></returns>
        public static List<Plane> SlicePlanes(
            GeometryBase G,
            double LayerHeight,
            double OffsetBottomGlobal = 0,
            double OffsetBottom = 0,
            double OffsetTop = 0,
            bool ShouldForceTop = false)
        {
            List<Plane> Frames = new List<Plane>();

            // Default BoundingBox
            BoundingBox bb = G.GetBoundingBox(true);
            Point3d[] corners = bb.GetCorners();
            Line ZLine = new Line(corners[0], corners[4]);
            double ZLineLength = ZLine.Length;

            // Safety
            int FrameCap = 150;
            if (ZLineLength / LayerHeight > FrameCap - 1) LayerHeight = ZLine.Length / (FrameCap - 1);

            // Z Frames
            double ZAmount = ZLineLength / LayerHeight;
            bool HasTopLayer = false;

            for (double i = 0; i <= ZAmount; i++)
            {
                double distance = i * LayerHeight + OffsetBottomGlobal;
                if (i == 0) distance += OffsetBottom;
                if (distance == ZLineLength) HasTopLayer = true;
                if (distance == ZLineLength || !ShouldForceTop && LayerHeight * (i + 1) + OffsetBottomGlobal > ZLineLength) distance += -OffsetTop;
                double param = distance / ZLineLength;
                if (param <= 1)
                {
                    Plane pl;
                    ZLine.ToNurbsCurve().PerpendicularFrameAt(param, out pl);
                    Frames.Add(pl);
                }
            }
            if (ShouldForceTop && !HasTopLayer)
            {
                Plane pl;
                double param = 1 - OffsetTop / ZLineLength;
                if (param > 1) param = 1;
                ZLine.ToNurbsCurve().PerpendicularFrameAt(param, out pl);
                Frames.Add(pl);
            }
            return Frames;
        }

        /// <summary>
        /// Extract bounding frames to slice a given geometry.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="XSpan"></param>
        /// <param name="YSpan"></param>
        /// <param name="ZSpan"></param>
        /// <param name="DXSpan"></param>
        /// <param name="DYSpan"></param>
        /// <param name="AngleDegrees"></param>
        /// <returns></returns>
        public static List<List<Plane>> BoundingFrames(GeometryBase G, double XSpan, double YSpan, double ZSpan, double DXSpan, double DYSpan, double AngleDegrees = 45.0)
        {
            // Nono.Util
            // BoundingFrames
            // Nono Martínez Alonso (nono.ma)
            // 160622, Autodesk Generative Design

            List<List<Plane>> Frames = new List<List<Plane>>();

            // 1. Default BoundingBox

            BoundingBox bb = G.GetBoundingBox(true);
            Point3d[] corners = bb.GetCorners();

            Line XLine = new Line(corners[0], corners[1]);
            Line YLine = new Line(corners[0], corners[3]);
            Line ZLine = new Line(corners[0], corners[4]);

            List<Plane> XFrames = new List<Plane>();
            List<Plane> YFrames = new List<Plane>();
            List<Plane> ZFrames = new List<Plane>();

            // X Frames
            if (XSpan > 0)
            {
                double XAmount = Math.Floor(XLine.Length / XSpan);
                double XStep = XLine.Length / XAmount;
                double Xt = 0;
                for (double i = 0; i <= XAmount; i++)
                {
                    Plane pl;
                    Xt = i * XStep / XLine.Length;
                    XLine.ToNurbsCurve().PerpendicularFrameAt(Xt, out pl);
                    XFrames.Add(pl);
                }
            }

            // Y Frames
            if (YSpan > 0)
            {
                double YAmount = Math.Floor(YLine.Length / YSpan);
                double YStep = YLine.Length / YAmount;
                double Yt = 0;
                for (double i = 0; i <= YAmount; i++)
                {
                    Plane pl;
                    Yt = i * YStep / YLine.Length;
                    YLine.ToNurbsCurve().PerpendicularFrameAt(Yt, out pl);
                    YFrames.Add(pl);
                }
            }

            // Z Frames
            double ZAmount = ZLine.Length / ZSpan;
            for (double i = 0; i <= ZAmount; i++)
            {
                Plane pl;
                ZLine.ToNurbsCurve().PerpendicularFrameAt(i * ZSpan / ZLine.Length, out pl);
                ZFrames.Add(pl);
            }

            // 2. Diagonal BoundingBox

            Transform rotate = Transform.Rotation(-V2GMath.DEGREES_TO_RADIANS * AngleDegrees, new Vector3d(0, 0, 1), bb.Center);
            Transform rotateBack = Transform.Rotation(V2GMath.DEGREES_TO_RADIANS * AngleDegrees, new Vector3d(0, 0, 1), bb.Center);
            G.Transform(rotate);

            BoundingBox Dbb = G.GetBoundingBox(true);
            Point3d DbbCenter = Dbb.Center;
            Point3d[] Dcorners = Dbb.GetCorners();

            Line DXLine = new Line(Dcorners[0], Dcorners[1]);
            Line DYLine = new Line(Dcorners[0], Dcorners[3]);

            List<Plane> DXFrames = new List<Plane>();
            List<Plane> DYFrames = new List<Plane>();

            // DX Frames
            if (DXSpan > 0)
            {
                double DXAmount = Math.Floor(DXLine.Length / DXSpan);
                double DXStep = DXLine.Length / DXAmount;
                double DXt = 0;
                for (double i = 0; i <= DXAmount; i++)
                {
                    Plane pl;
                    DXt = i * DXStep / DXLine.Length;
                    DXLine.ToNurbsCurve().PerpendicularFrameAt(DXt, out pl);
                    pl.Transform(rotateBack);

                    DXFrames.Add(pl);
                }
            }

            // DY Frames
            if (DYSpan > 0)
            {
                double DYAmount = Math.Floor(DYLine.Length / DYSpan);
                double DYStep = DYLine.Length / DYAmount;
                double DYt = 0;
                for (double i = 0; i <= DYAmount; i++)
                {
                    Plane pl;
                    DYt = i * DYStep / DYLine.Length;
                    DYLine.ToNurbsCurve().PerpendicularFrameAt(DYt, out pl);
                    pl.Transform(rotateBack);
                    DYFrames.Add(pl);
                }
            }

            Frames.Add(XFrames);
            Frames.Add(YFrames);
            Frames.Add(ZFrames);
            Frames.Add(DXFrames);
            Frames.Add(DYFrames);

            return Frames;
        }
    }
}
