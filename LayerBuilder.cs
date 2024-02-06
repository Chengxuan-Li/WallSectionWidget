using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace WallSectionWidget
{
    public class LayerBuilder : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the LayerBuilder class.
        /// </summary>
        public LayerBuilder()
          : base("LayerBuilder", "LB",
              "Layer builder for wall section widget",
              "WallSectionWidget", "Specifications")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddScriptVariableParameter("WSWMaterial", "Mat", "WSW Material definition", GH_ParamAccess.item);
            pManager.AddNumberParameter("Thickness", "W", "Thickness (m)", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("WSWLayer", "Lyr", "WSW Layer definition", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GHIOParam<Material> MaterialGHIO = default; 
            double thickness = default;
            
            if (!DA.GetData(0, ref MaterialGHIO))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: WSW Material definition");
                return;
            }
            if (!DA.GetData(1, ref thickness))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: layer thickness");
                return;
            }

            Material material;
            if (!MaterialGHIO.GetContent(out material))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: WSWMaterial input is of wrong type");
                return;
            }
            
            if (thickness <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: thickness should be bigger than zero");
                return;
            }

            Layer layer = new Layer(material, thickness);
            DA.SetData(0, layer.GHIOParam);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d4876b08-fb68-456e-a990-27b769bb42cc"); }
        }
    }
}