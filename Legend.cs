using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Geometry;
using Rhino.Display;

namespace WallSectionWidget
{
    public class Legend
    {
        Rhino.DocObjects.DimensionStyle dimStyle = new Rhino.DocObjects.DimensionStyle();

        public void SetDimStyle()
        {
            dimStyle.TextHeight = 0.04 * Scale;
            dimStyle.TextHorizontalAlignment = (OnLeft) ? Rhino.DocObjects.TextHorizontalAlignment.Right : Rhino.DocObjects.TextHorizontalAlignment.Left;
            dimStyle.TextVerticalAlignment = Rhino.DocObjects.TextVerticalAlignment.Middle;
        }
        
        public double MinValue;
        public double MaxValue;
        public Plane Plane = Plane.WorldXY;
        public Transform Transform => Transform.PlaneToPlane(Plane.WorldXY, Plane);
        public double Scale = 1.0;
        public double Height = 1.0;
        public int DisiredCount = 20;
        public List<double> Labels => PrettyBreaks(MinValue, MaxValue, DisiredCount);
        public bool OnLeft = true;
        public List<double> Positions
        {
            get
            {
                List<double> pos = new List<double>();
                for (int i = 0; i < Labels.Count; i++)
                {
                    pos.Add(i * Height / (Labels.Count - 1) * Scale);
                }
                return pos;
            }
        }

        public List<Point3d> Points
        {
            get
            {
                List<Point3d> pts = new List<Point3d>();
                Positions.ForEach(p => pts.Add(new Point3d(0, p, 0)));
                for(int i = 0; i < pts.Count; i++)
                {
                    var p = pts[i];
                    p.Transform(Transform);
                    pts[i] = p;
                }

                return pts;
            }
        }

        public List<Line> Markers
        {
            get
            {
                List<Line> markers = new List<Line>();
                Positions.ForEach(p => markers.Add(new Line(
                    new Point3d(-0.05 * Scale, p, 0),
                    new Point3d(0.05 * Scale, p, 0)
                    )));
                for (int i = 0; i < markers.Count; i++)
                {
                    var l = markers[i];
                    l.Transform(Transform);
                    markers[i] = l;
                }
                return markers;
            }
        }

        public Line Axis
        {
            get
            {
                Line line = new Line(Points[0], Points[Points.Count - 1]);
                //line.Transform(Transform);
                return line;
            }
        }
        public string Title = "Legend";
        public Plane TitleLocation
        {
            get
            {
                Point3d pt = new Point3d(0, (Height + 0.1) * Scale, 0);
                Plane plane = new Plane(pt, Vector3d.XAxis, Vector3d.YAxis);
                plane.Transform(Transform);
                return plane;
            }
        }
        public TextEntity TitleTextEntity
        {
            get
            {
                SetDimStyle();
                var te = TextEntity.Create(Title, TitleLocation, dimStyle, false, 0.4, 0);
                te.TextHorizontalAlignment =
                    OnLeft? Rhino.DocObjects.TextHorizontalAlignment.Right : Rhino.DocObjects.TextHorizontalAlignment.Left;
                te.TextHeight = 0.12;
                return te;
            }
        }

        public List<TextEntity> AxisLabelTextEntities
        {
            get
            {
                SetDimStyle();
                List<TextEntity> tes = new List<TextEntity>();
                List<string> txts = new List<string>();
                Labels.ForEach(L => txts.Add(L.ToString()));
                List<Plane> planes = new List<Plane>();
                for (int i = 0; i < Positions.Count; i++)
                {
                    Point3d pt = new Point3d(OnLeft ? -0.1 : 0.1, Positions[i], 0);
                    Plane plane = new Plane(pt, Vector3d.XAxis, Vector3d.YAxis);
                    plane.Transform(Transform);
                    planes.Add(plane);
                }
                for (int i = 0; i < planes.Count; i++)
                {
                    TextEntity te = TextEntity.Create(
                            txts[i], planes[i], dimStyle, false, 0.4, 0
                            );
                    
                    tes.Add(te);
                }

                return tes;
            }
        }
        

        public static List<double> PrettyBreaks(double minValue, double maxValue, int desiredCount)
        {
            if (minValue >= maxValue)
            {
                double vv = minValue;
                minValue = maxValue;
                maxValue = vv;
            }
            double difference = maxValue - minValue;
            int pw = (int)Math.Floor(Math.Log10(difference));
            double base1 = Math.Pow(10.0, pw - 1);
            double base2 = Math.Pow(10.0, pw);
            List<double> result1 = PrettyBreaks(minValue, maxValue, base1);
            List<double> result2 = PrettyBreaks(minValue, maxValue, base2);
            if (Math.Abs(result1.Count - desiredCount) <= Math.Abs(result2.Count - desiredCount))
            {
                return result1;
            } else
            {
                return result2;
            }
        }

        public static List<double> PrettyBreaks(double minValue, double maxValue, double baseFactor)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < 1000; i++)
            {
                if (baseFactor * (i - 2) >= maxValue)
                {
                    break;
                }
                else if (baseFactor * (i + 1) >= minValue)
                {
                    result.Add(baseFactor * i);
                }
            }
            return result;
        }
    }
}
