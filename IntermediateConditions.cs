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
        public double StepLength;
        public List<double> Temperatures;
        public List<double> VapourPressures;
        public List<double> Depths;
        public List<double> DewPoints;
        public List<double> RelativeHumidities;

        public IntermediateConditions(Layer layer, double minStep = 0.005)
        {
            Layer = layer;
            NumStep = (int)Math.Ceiling(Layer.Thickness / minStep);
            StepLength = Layer.Thickness / NumStep;
            Temperatures = new List<double>();
            VapourPressures = new List<double>();
            Depths = new List<double>();
            for (int i = 0; i < this.NumStep; i++)
            {
                Temperatures.Add(Layer.InteriorTemperature + i / (double)NumStep * Layer.TemperatureDifference);
                VapourPressures.Add(Layer.InteriorVapourPressure + i / (double)NumStep * Layer.VapourPressureDifference);
                Depths.Add(Layer.InteriorSideDepthFromSurface + i / (double)NumStep * Layer.Thickness);
            }
            Temperatures.Add(Layer.ExteriorTemperature);
            VapourPressures.Add(Layer.ExteriorVapourPressure);
            Depths.Add(Layer.ExteriorSideDepthFromSurface);

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
