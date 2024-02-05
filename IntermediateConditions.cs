using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallSectionWidget
{
    public class IntermediateConditions
    {
        public Layer Layer;
        public int NumStep;
        public double Step;
        public List<double> Temperatures;
        public List<double> VapourPressures;
        public List<double> Depths;
        public List<double> DewPoints;
        public List<double> RelativeHumidities;

        public IntermediateConditions(Layer layer, double minStep = 0.005)
        {
            Layer = layer;
            NumStep = (int)Math.Ceiling(Layer.Thickness / minStep);
            Step = Layer.Thickness / NumStep;
            Temperatures = new List<double>();
            VapourPressures = new List<double>();
            Depths = new List<double>();
            for (int i = 0; i < this.NumStep; i++)
            {
                Temperatures.Add(Layer.Ti + i / (double)NumStep * Layer.Dt);
                VapourPressures.Add(Layer.Pi + i / (double)NumStep * Layer.Dp);
                Depths.Add(Layer.Xi + i / (double)NumStep * Layer.Thickness);
            }
            Temperatures.Add(Layer.Te);
            VapourPressures.Add(Layer.Pe);
            Depths.Add(Layer.Xe);

            DewPoints = new List<double>();
            RelativeHumidities = new List<double>();
            for (int i = 0; i < NumStep + 1; i++)
            {
                DewPoints.Add(Psychrometrics.DewPoint(this.VapourPressures[i]));
                RelativeHumidities.Add(Psychrometrics.RelativeHumidity(this.Temperatures[i], this.VapourPressures[i]));
            }
        }
    }
}
