using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace WallSectionWidget
{
    public class WallSectionSolver : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public WallSectionSolver()
          : base("WallSectionSolver", "WSS",
              "Core solver for wall section temperature and humidity profile.",
              "WallSectionWidget", "Core")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddScriptVariableParameter("WSWConstruction", "C", "WSW Construction definition", GH_ParamAccess.item);
            pManager.AddScriptVariableParameter("WSWModelParameters", "MParams", "WSW model parameters definition", GH_ParamAccess.item);           
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Use the pManager object to register your output parameters.
            // Output parameters do not have default values, but they too must have the correct access type.
            pManager.AddGenericParameter("WSModel", "SM", "WSW Model definition of a solved wall section model", GH_ParamAccess.item);
            pManager.AddNumberParameter("Depths", "D", "Depths from the inner surface of the wall", GH_ParamAccess.list);
            pManager.AddNumberParameter("Temperatures", "T", "Temperature profile from the inner surface of the wall", GH_ParamAccess.list);
            pManager.AddNumberParameter("VapourPressures", "VP", "Vapour pressure profile from the inner surface of the wall", GH_ParamAccess.list);
            pManager.AddNumberParameter("DewPoints", "DP", "Dew point temperature profile from the inner surface of the wall", GH_ParamAccess.list);
            pManager.AddNumberParameter("RelativeHumidityLevels", "RH", "Relative humidity profile from the inner surface of the wall", GH_ParamAccess.list);
            // Sometimes you want to hide a specific parameter from the Rhino preview.
            // You can use the HideParameter() method as a quick way:
            //pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
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

            GHIOParam<Parameters> parametersGHIO = default;
            if (!DA.GetData(1, ref parametersGHIO))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Missing input: WSWParameters definition");
                return;
            }
            Parameters parameters;
            if (!parametersGHIO.GetContent(out parameters))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input error: WSWParameters input is of wrong type");
                return;
            }

            // Instantiate the model
            Model model = new Model(construction, parameters);


            // Finally assign model results to the output parameter.
            DA.SetData(0, model.GHIOParam);
            DA.SetDataList(1, model.Depths);
            DA.SetDataList(2, model.Temperatures);
            DA.SetDataList(3, model.VapourPressures);
            DA.SetDataList(4, model.DewPoints);
            DA.SetDataList(5, model.RelativeHumidityLevels);
        }


        /// <summary>
        /// The Exposure property controls where in the panel a component icon 
        /// will appear. There are seven possible locations (primary to septenary), 
        /// each of which can be combined with the GH_Exposure.obscure flag, which 
        /// ensures the component will only be visible on panel dropdowns.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Properties.Resources.WallSectionSolver.ToBitmap();
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("561d7336-6a61-4240-acf7-fa566ca4aed4"); }
        }
    }
}
