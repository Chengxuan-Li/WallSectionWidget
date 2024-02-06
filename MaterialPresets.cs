using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace WallSectionWidget
{
    public class MaterialPresets : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MaterialPresets class.
        /// </summary>
        public MaterialPresets()
          : base("MaterialPresets", "MatPresets",
              "WSW Material preset definitions",
              "WallSectionWidget", "Specifications")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Air", "Air", "Generic air : WSW Material definition", GH_ParamAccess.item);
            pManager.AddGenericParameter("Concrete", "Conc", "Concrete : WSW Material definition", GH_ParamAccess.item);
            pManager.AddGenericParameter("Cork", "Cork", "Cork : WSW Material definition", GH_ParamAccess.item);
            pManager.AddGenericParameter("Lamination", "Lam", "Lamination : WSW Material definition", GH_ParamAccess.item);
            pManager.AddGenericParameter("VapourControlLayer", "VCL", "Vapour control layer (mu=70k) : WSW Material definition", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, Material.Air.GHIOParam);
            DA.SetData(1, Material.Concrete.GHIOParam);
            DA.SetData(2, Material.Cork.GHIOParam);
            DA.SetData(3, Material.Lamination.GHIOParam);
            DA.SetData(4, Material.VapourRetarder.GHIOParam);
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
                return Properties.Resources.MaterialPresets.ToBitmap();
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2d962993-de6b-4cea-8e1c-05e5b005ead7"); }
        }
    }
}