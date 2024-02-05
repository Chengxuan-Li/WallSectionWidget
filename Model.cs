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

        public Model(Construction construction, Parameters parameters)
        {
            Parameters = parameters;
            Construction = construction;

            List<Layer> layers = Construction.Layers;
            layers.Insert(0, new Layer(new Material().Air, Parameters.InteriorContactDistance));
            layers.Add(new Layer(new Material().Air, Parameters.ExteriorContactDistance));
            Construction = new Construction(layers);

            Construction.Ti = Parameters.InteriorTemperature;
            Construction.Hi = Parameters.InteriorHumidity;
            Construction.Te = Parameters.ExteriorTemperature;
            Construction.He = Parameters.ExteriorHumidity;
            Construction.UpdateLayers();
        }
    }
}
