using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Rhino;
using Rhino.Geometry;

namespace WallSectionWidget
{
    public class WallSectionVisualiser
    {
        public Construction Construction;
        public Plane Plane;
        public double Height;
        public double Scale;
        public double UnitScale => 1.0; // RhinoMath.UnitScale(RhinoDoc.ActiveDoc.ModelUnitSystem, UnitSystem.Meters);

        public WallSectionVisualiser(Construction construction, Plane plane, double height, double scale)
        {
            Construction = construction;
            Plane = plane;
            Height = height / UnitScale;
            Scale = scale;
        }

        public void LayersGeometry(out List<Line> seps, out List<Mesh> hatches)
        {
            seps = new List<Line>();
            hatches = new List<Mesh>();

            List<double> xSep = new List<double> { 0.0 };
            foreach (Layer layer in Construction.Layers)
            {
                xSep.Add(xSep[xSep.Count - 1] + layer.Thickness);
            }
            List<Point3d> SepPtsBelow = new List<Point3d>();
            List<Point3d> SepPtsAbove = new List<Point3d>();
            xSep.ForEach(x => SepPtsBelow.Add(new Point3d(x * Scale, 0, 0)));
            xSep.ForEach(x => SepPtsAbove.Add(new Point3d(x * Scale, Height * Scale, 0)));

            Transform transform = Transform.PlaneToPlane(Plane.WorldXY, Plane);
            SepPtsBelow.ForEach(p => p.Transform(transform));
            SepPtsAbove.ForEach(p => p.Transform(transform));
            for (int i = 0; i < SepPtsAbove.Count; i++)
            {
                seps.Add(new Line(SepPtsBelow[i], SepPtsAbove[i]));
            }
            for (int i = 0; i < SepPtsAbove.Count - 1; i++)
            {
                List<Point3d> pts = new List<Point3d>
                {
                    SepPtsBelow[i],
                    SepPtsBelow[i + 1],
                    SepPtsAbove[i + 1],
                    SepPtsAbove[i],
                    //SepPtsBelow[i]
                };
                //Mesh mesh = Mesh.CreateFromClosedPolyline(new Polyline(pts));
                Mesh mesh = new Mesh();
                mesh.Vertices.AddVertices(pts);
                mesh.Faces.AddFace(0, 1, 2, 3);
                mesh.VertexColors.CreateMonotoneMesh(Construction.Layers[i].Material.VisualiserSetting.Color);
                for (int j = 0; j < mesh.VertexColors.Count; j++)
                {
                    mesh.VertexColors.SetColor(j, Construction.Layers[i].Material.VisualiserSetting.Color);
                }
                hatches.Add(mesh);
            }
        }


    }
}
