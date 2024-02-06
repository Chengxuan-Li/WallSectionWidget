using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace WallSectionWidget
{
    public class DisplayMaterialProperties : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DisplayMaterialProperties class.
        /// </summary>
        public DisplayMaterialProperties()
          : base("DisplayMaterialProperties", "DMat",
              "Display the material properties of a WSW Material definition",
              "WallSectionWidget", "Visualisation")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddScriptVariableParameter("WSWMaterial", "Mat", "WSW Material definition", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Name for this material", GH_ParamAccess.item);
            pManager.AddNumberParameter("Conductivity", "C", "Thermal conductivity (W/mK)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Vapour resistance", "VR", "Water vapour diffusion resistance factor over ordinary air", GH_ParamAccess.item);
            pManager.AddNumberParameter("Density", "D", "Density (kg/m3)", GH_ParamAccess.item);
            pManager.AddNumberParameter("HeatCapacity", "HC", "Specific heat capacity (J/kgK)", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GHIOParam<Material> MaterialGHIO = default;
            if (!DA.GetData(0, ref MaterialGHIO))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: WSW Material definition");
                return;
            }
            Material material;
            if (!MaterialGHIO.GetContent(out material))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: WSWMaterial input is of wrong type");
                return;
            }
            DA.SetData(0, material.Name);
            DA.SetData(1, material.Conductivity);
            DA.SetData(2, material.VapourResistivity);
            DA.SetData(3, material.Density);
            DA.SetData(4, material.HeatCapacity);
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
                return Properties.Resources.DisplayMaterial.ToBitmap();
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("508b7a7d-5029-4cbc-8c7d-ec2dbbb261a3"); }
        }
    }
}