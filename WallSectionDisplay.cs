using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace WallSectionWidget
{
    public class WallSectionDisplay : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the WallSectionVisualiser class.
        /// </summary>
        public WallSectionDisplay()
          : base("WallSectionDisplay", "WSDis",
              "Display for a wall section construction",
              "WallSectionWidget", "Display")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddScriptVariableParameter("WSWConstruction", "C", "WSW Construction definition", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "Pl", "Plane", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Height", "H", "Height", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Scale", "S", "Custom scale factor", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Layer Separator Lines", "LSL", "Lines that separate each layer", GH_ParamAccess.list);
            pManager.AddMeshParameter("Layer Hatch", "LM", "Mesh representation of layers", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GHIOParam<Construction> constructionGHIO = default;
            if (!DA.GetData(0, ref constructionGHIO))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: WSWConstruction definition");
                return;
            }
            Construction construction;
            if (!constructionGHIO.GetContent(out construction))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: WSWConstruction input is of wrong type");
                return;
            }

            Plane plane = Plane.WorldXY;
            double height = 1.0;
            double scale = 1.0;

            DA.GetData(1, ref plane);
            DA.GetData(2, ref height);
            if (height <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: height should be bigger than zero");
                return;
            }
            DA.GetData(3, ref scale);
            if (scale <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: scale should be bigger than zero");
                return;
            }

            WallSectionVisualiser vis = new WallSectionVisualiser(construction, plane, height, scale);

            List<Line> seps;
            List<Mesh> hatches;

            vis.LayersGeometry(out seps, out hatches);

            DA.SetDataList(0, seps);
            DA.SetDataList(1, hatches);
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
            get { return new Guid("8da61c5b-6a75-4b69-9e66-2d1c7621edf2"); }
        }
    }
}