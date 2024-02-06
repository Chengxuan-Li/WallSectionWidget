using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Geometry;

namespace WallSectionWidget
{
    public class ProfileVisualiser
    {
        public List<double> XValues;
        public List<double> YValues;
        public double SuggestedMinValue;
        public double SuggestedMaxValue;
        public Plane Plane;
        public Transform Transform => Transform.PlaneToPlane(Plane.WorldXY, Plane);
        public double Scale = 1.0;
        public double Height = 1.0;

        public double TailLength = 0.2;
        public double LegendGap = 0.1;

        public Legend Legend;

        public bool OverrideLegendMinMax = false;
        public double OverrideLegendMin;
        public double OverrideLegendMax;

        public double ActualMinValue => Math.Min(YValues.Min(), SuggestedMinValue);
        public double ActualMaxValue => Math.Max(YValues.Max(), SuggestedMaxValue);

        public ProfileVisualiser(List<double> xs, List<double> ys)
        {
            XValues = xs;
            YValues = ys;
        }

        public Polyline PolylineProfile()
        {

            double minValue = Legend.Labels[0];
            double maxValue = Legend.Labels[Legend.Labels.Count - 1];
            List<double> posX = new List<double> { -TailLength * Scale };
            XValues.ForEach(x => posX.Add(x * Scale));
            posX.Add(posX[posX.Count - 1] + TailLength * Scale);

            List<double> posY = new List<double>();
            YValues.ForEach(y => posY.Add((y - minValue) / (maxValue - minValue) * Height * Scale));
            posY.Insert(0, posY[0]);
            posY.Add(posY[posY.Count - 1]);
            List<Point3d> pts = new List<Point3d>();
            for (int i = 0; i < posX.Count; i++)
            {
                Point3d p = new Point3d(posX[i], posY[i], 0);
                p.Transform(Transform);
                pts.Add(p);
            }

            return new Polyline(pts);
        }

        public void LegendLeft()
        {
            Plane plane = new Plane(
                    new Point3d((-TailLength - LegendGap) * Scale, 0, 0),
                    Vector3d.XAxis,
                    Vector3d.YAxis
                    );
            plane.Transform(Transform);
            Legend = new Legend
            {
                MaxValue = ActualMaxValue,
                MinValue = ActualMinValue,
                Scale = this.Scale,
                Height = this.Height,
                Plane = plane,
                OnLeft = true,
                AutomaticLabelling = !OverrideLegendMinMax,
                AlternativeLabels = Legend.PrettyBreaks(OverrideLegendMin, OverrideLegendMax, 3),
            };
            return;
        }

        public void LegendRight()
        {
            Plane plane = new Plane(
                    new Point3d((XValues[XValues.Count - 1] + TailLength + LegendGap) * Scale, 0, 0),
                    Vector3d.XAxis,
                    Vector3d.YAxis
                    );
            plane.Transform(Transform);
            Legend = new Legend
            {
                MaxValue = ActualMaxValue,
                MinValue = ActualMinValue,
                Scale = this.Scale,
                Height = this.Height,
                Plane = plane,
                OnLeft = false,
                AutomaticLabelling = !OverrideLegendMinMax,
                AlternativeLabels = Legend.PrettyBreaks(OverrideLegendMin, OverrideLegendMax, 3),
            };
            return;
        }
    }
}
