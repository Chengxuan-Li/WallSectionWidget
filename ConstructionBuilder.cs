using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace WallSectionWidget
{
    public class ConstructionBuilder : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ConstructionBuilder class.
        /// </summary>
        public ConstructionBuilder()
          : base("ConstructionBuilder", "CB",
              "Construction builder for wall section widget",
              "WallSectionWidget", "Specifications")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddScriptVariableParameter("WSWLayers", "Lyrs", "WSW Layer definitions as a list", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("WSWConstruction", "C", "WSW Construction definition", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<GHIOParam<Layer>> layersGHIO = new List<GHIOParam<Layer>>();
            if (!DA.GetDataList(0, layersGHIO))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: WSWLayer definitions");
                return;
            }

            List<Layer> layers = new List<Layer>();
            Layer layer;
            foreach (GHIOParam<Layer> lghio in layersGHIO)
            {
                if (!lghio.GetContent(out layer))
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, String.Format("Input error: one element at position {0} of the input WSWLayers list is of wrong type", layersGHIO.IndexOf(lghio)));
                    return;
                }
                layers.Add(layer);
            }
            Construction construction = new Construction { Layers = layers };
            DA.SetData(0, construction.GHIOParam);
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
                return Properties.Resources.ConstructionBuilder.ToBitmap();
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("fc23e91f-ad90-4f97-9172-6b5fdfabf4e6"); }
        }
    }
}