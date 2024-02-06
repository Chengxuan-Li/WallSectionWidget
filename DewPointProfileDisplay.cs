using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Rhino.Display;

namespace WallSectionWidget
{
    public class DewPointProfileDisplay : GH_Component
    {
        
        public List<TextEntity> PreviewTexts = new List<TextEntity>();
        public Polyline Profile;

        /// <summary>
        /// Initializes a new instance of the ProfileDisplay class.
        /// </summary>
        public DewPointProfileDisplay()
          : base("DewPointProfileDisplay", "DPPFDis",
              "Display for a wall section dew point temperature profile",
              "WallSectionWidget", "Visualisation")
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
            pManager.AddNumberParameter("OverrideLegendMin", "OLMin", "Value used to override the legend minimum", GH_ParamAccess.item);
            pManager.AddNumberParameter("OverrideLegendMax", "OLMax", "Value used to override the legend maximum", GH_ParamAccess.item);
            pManager.AddBooleanParameter("BakeText", "BkTxt", "Bake texts", GH_ParamAccess.item);

            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
            pManager[7].Optional = true;
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
            double overrideLegendMin = 0.0;
            double overrideLegendMax = 30.0;
            bool overrideLegendMinMax = false;
            if (DA.GetData(5, ref overrideLegendMin) && DA.GetData(6, ref overrideLegendMax))
            {
                overrideLegendMinMax = true;
            }

            bool bake = false;
            DA.GetData(7, ref bake);

            ProfileVisualiser vis = new ProfileVisualiser(model.Depths, model.DewPoints);
            vis.OverrideLegendMinMax = overrideLegendMinMax;
            if (overrideLegendMinMax)
            {
                vis.OverrideLegendMax = overrideLegendMax;
                vis.OverrideLegendMin = overrideLegendMin;
            }
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
            Profile = vis.PolylineProfile();

            vis.Legend.Title = @"Dew Point Temperature (degC)";

            PreviewTexts.Clear();
            PreviewTexts.Add(vis.Legend.TitleTextEntity);
            PreviewTexts.AddRange(vis.Legend.AxisLabelTextEntities);

            if (bake)
            {
                PreviewTexts.ForEach(t => Rhino.RhinoDoc.ActiveDoc.Objects.AddText(t));
            }

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.DewPointDisplay.ToBitmap();
            }
        }


        public override void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            PreviewTexts.ForEach(t => args.Display.DrawText(t, System.Drawing.Color.Black));
            args.Display.DrawPolyline(Profile, System.Drawing.Color.Black, 3);
        }

        
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("feb1ed8f-1a93-465f-8be8-45ae965d7072"); }
        }
    }
}