using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace WallSectionWidget
{
    public class ModelParameters : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ModelParameters class.
        /// </summary>
        public ModelParameters()
          : base("ModelParameters", "MParams",
              "Model parameters",
              "WallSectionWidget", "Specifications")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("InternalTemperature", "InT", "Internal temperature (degC)", GH_ParamAccess.item);
            pManager.AddNumberParameter("ExternalTemperature", "ExT", "External temperature (degC)", GH_ParamAccess.item);
            pManager.AddNumberParameter("InternalRelativeHumidity", "InRH", @"Internal relative humidity (%)", GH_ParamAccess.item);
            pManager.AddNumberParameter("ExternalRelativeHumidity", "ExRH", @"External relative humidity (%)", GH_ParamAccess.item);
            pManager.AddNumberParameter("InternalSurfaceContactLayerDistance", "InSCLD", "Internal surface contact layer (air) distance (m); 0.00625 by default", GH_ParamAccess.item);
            pManager.AddNumberParameter("ExternalSurfaceContactLayerDistance", "ExSCLD", "External surface contact layer (air) distance (m); 0.001 by default", GH_ParamAccess.item);
            pManager.AddNumberParameter("MaxStepLength", "MaxSL", "Maximum length (m) when each layer is discretised; 0.005 by default", GH_ParamAccess.item);
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("WSWModelParameters", "MParams", "WSW model parameters definition", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double ti = default;
            double te = default;
            double hi = default;
            double he = default;
            double inscld = Parameters.DefaultSummer.InteriorContactDistance;
            double exscld = Parameters.DefaultSummer.ExteriorContactDistance;
            if (!DA.GetData(0, ref ti))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: internal temperature");
                return;
            }
            if (!DA.GetData(1, ref te))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: external temperature");
                return;
            }
            if (!DA.GetData(2, ref hi))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: internal humidity");
                return;
            }
            if (hi < 0 || hi > 100)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: relative humidity should be a percentage value between 0 and 100");
                return;
            }
            if (!DA.GetData(3, ref he))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: external humidity");
                return;
            }
            if (he < 0 || he > 100)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: relative humidity should be a percentage value between 0 and 100");
                return;
            }
            DA.GetData(4, ref inscld);
            DA.GetData(5, ref exscld);
            if (inscld <= 0 || exscld <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: contact distance should be bigger than zero");
                return;
            }
            if (inscld > 0.2 || exscld > 0.2)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Warning: contact distance should normally be a positive value smaller than 0.1 m");
            }
            double maxStepLength = 0.005;
            DA.GetData(6, ref maxStepLength);
            if (maxStepLength <= 0 || maxStepLength > 0.05)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: Max Step Length should be a small positive value");
                return;
            }
            Parameters parameters = new Parameters
            {
                InteriorTemperature = ti,
                ExteriorTemperature = te,
                InteriorHumidity = hi,
                ExteriorHumidity = he,
                InteriorContactDistance = inscld,
                ExteriorContactDistance = exscld,
                MaxStepLength = maxStepLength,
            };
            DA.SetData(0, parameters.GHIOParam);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.ModelParameters.ToBitmap();
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("75bb3247-574f-4689-85fe-3be1ba775f5a"); }
        }
    }
}