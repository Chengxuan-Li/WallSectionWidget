using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallSectionWidget
{
    public class Model
    {
        public Construction Construction;
        public Parameters Parameters;
        public List<double> Depths = new List<double>();
        public List<double> Temperatures = new List<double>();
        public List<double> VapourPressures = new List<double>();
        public List<double> DewPoints = new List<double>();
        public List<double> RelativeHumidityLevels = new List<double>();

        public GHIOParam<Model> GHIOParam => new GHIOParam<Model>(this);


        public Model(Construction construction, Parameters parameters)
        {
            Parameters = parameters;
            Construction = construction;

            List<Layer> layers = Construction.Layers;
            layers.Insert(0, new Layer(Material.Air, Parameters.InteriorContactDistance));
            layers.Add(new Layer(Material.Air, Parameters.ExteriorContactDistance));
            Construction = new Construction { Layers = layers };

            Construction.InteriorTemperature = Parameters.InteriorTemperature;
            Construction.InteriorHumidity = Parameters.InteriorHumidity;
            Construction.ExteriorTemperature = Parameters.ExteriorTemperature;
            Construction.ExteriorHumidity = Parameters.ExteriorHumidity;
            Construction.UpdateLayers();
            Construction.Layers.ForEach(L => L.IntermediateConditions.MaxStepLength = Parameters.MaxStepLength);
            Solve();
        }

        void Solve()
        {
            foreach (Layer layer in Construction.Layers)
            {
                List<double> Depths = layer.IntermediateConditions.Depths;
                Depths.RemoveAt(Depths.Count - 1);
                this.Depths.AddRange(Depths);

                List<double> Temperatures = layer.IntermediateConditions.Temperatures;
                Temperatures.RemoveAt(Temperatures.Count - 1);
                this.Temperatures.AddRange(Temperatures);

                List<double> VapourPressures = layer.IntermediateConditions.VapourPressures;
                VapourPressures.RemoveAt(VapourPressures.Count - 1);
                this.VapourPressures.AddRange(VapourPressures);

                List<double> DewPoints = layer.IntermediateConditions.DewPoints;
                DewPoints.RemoveAt(DewPoints.Count - 1);
                this.DewPoints.AddRange(DewPoints);

                List<double> RelativeHumidities = layer.IntermediateConditions.RelativeHumidities;
                RelativeHumidities.RemoveAt(RelativeHumidities.Count - 1);
                RelativeHumidityLevels.AddRange(RelativeHumidities);
            }

            Depths.Add(Construction.Thickness);
            Temperatures.Add(Construction.ExteriorTemperature);
            VapourPressures.Add(Construction.ExteriorVapourPressure);
            DewPoints.Add(Psychrometrics.DewPoint(Construction.ExteriorVapourPressure));
            RelativeHumidityLevels.Add(Construction.ExteriorHumidity);

        }
    }
}
