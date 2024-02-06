using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace WallSectionWidget
{
    public class ProfileDisplay : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ProfileDisplay class.
        /// </summary>
        public ProfileDisplay()
          : base("ProfileDisplay", "PFDis",
              "Display for a wall section numeric profile",
              "WallSectionWidget", "Display")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("WSModel", "SM", "WSW Model definition of a solved wall section model", GH_ParamAccess.item);
            //pManager.AddGenericParameter("WSLegend", "L", "WSW Legend definition of a legend parameter", GH_ParamAccess.item);
            pManager.AddBooleanParameter("LegendOnLeft", "LoL", "True if legend axis should be placed on the left; otherwise on the right", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "Pl", "Plane", GH_ParamAccess.item);
            pManager.AddNumberParameter("Height", "H", "Height", GH_ParamAccess.item);
            pManager.AddNumberParameter("Scale", "S", "Custom scale factor", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            //pManager[5].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("LegendGeometry", "LGeo", "Legend geometry as a list", GH_ParamAccess.list);
            pManager.AddCurveParameter("Profile", "PGeo", "Profile as a curve", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GHIOParam<Model> modelGHIO = default;
            if (!DA.GetData(0, ref modelGHIO))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: WSWModel definition");
                return;
            }

            Model model;
            if (!modelGHIO.GetContent(out model))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: WSWModel input is of wrong type");
                return;
            }

            bool legendOnLeft = true;
            DA.GetData(1, ref legendOnLeft);
            Plane plane = Plane.WorldXY;
            DA.GetData(2, ref plane);
            double height = 1.0;
            DA.GetData(3, ref height);
            if (height <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: height should be bigger than zero");
                return;
            }
            double scale = 1.0;
            DA.GetData(4, ref scale);
            if (scale <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: scale should be bigger than zero");
                return;
            }

            ProfileVisualiser vis = new ProfileVisualiser(model.Depths, model.Temperatures);
            vis.Plane = plane;
            vis.Height = height;
            vis.Scale = scale;
            if (legendOnLeft)
            {
                vis.LegendLeft();
            } else
            {
                vis.LegendRight();
            }

            List<Line> legendGeoCollector = new List<Line>();
            legendGeoCollector.AddRange(vis.Legend.Markers);
            legendGeoCollector.Add(vis.Legend.Axis);
            DA.SetDataList(0, legendGeoCollector);
            DA.SetData(1, vis.PolylineProfile().ToNurbsCurve());


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
            get { return new Guid("6afb7948-0ada-478b-8b76-0a2ad83de8c5"); }
        }
    }
}