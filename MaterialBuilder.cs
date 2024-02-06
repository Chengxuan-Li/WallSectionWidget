using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace WallSectionWidget
{
    public class MaterialBuilder : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MaterialBuilder class.
        /// </summary>
        public MaterialBuilder()
          : base("MaterialBuilder", "MB",
              "Material builder for wall section widget",
              "WallSectionWidget", "Specifications")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Name for this material", GH_ParamAccess.item);
            pManager.AddNumberParameter("Conductivity", "C", "Thermal conductivity (W/mK)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Vapour resistance", "VR", "Water vapour diffusion resistance factor over ordinary air", GH_ParamAccess.item);
            pManager.AddNumberParameter("Density", "D", "Density (kg/m3)", GH_ParamAccess.item);
            pManager.AddNumberParameter("HeatCapacity", "HC", "Specific heat capacity (J/kgK)", GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("WSWMaterial", "Mat", "WSW Material definition", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = default;
            double conductivity = Material.Lamination.Conductivity;
            double vapourResistivity = Material.Lamination.VapourResistivity;
            double density = Material.Lamination.Density;
            double heatCapacity = Material.Lamination.HeatCapacity;
            if (!DA.GetData(0, ref name))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: material name");
                return;
            }
            if (!DA.GetData(1, ref conductivity))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: material thermal conductivity");
                return;
            }
            if (!DA.GetData(2, ref vapourResistivity))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: material vapour resistance");
                return;
            }
            DA.GetData(3, ref density);
            DA.GetData(4, ref heatCapacity);

            if (conductivity <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: thermal conductivity should be bigger than zero");
                return;
            }
            if (vapourResistivity <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: vapour resistance should be bigger than zero");
                return;
            }
            if (density <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: density should be bigger than zero");
                return;
            }
            if (heatCapacity <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: heat capacity should be bigger than zero");
                return;
            }


            Material material = new Material
            {
                Name = name,
                Conductivity = conductivity,
                VapourResistivity = vapourResistivity,
                Density = density,
                HeatCapacity = heatCapacity
            };

            DA.SetData(0, material.GHIOParam);
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
                return Properties.Resources.MaterialBuilder.ToBitmap();
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("21b7cd4a-4d74-49af-b0ca-bbc49ae99b3e"); }
        }
    }
}