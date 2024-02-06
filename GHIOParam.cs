using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.Display;
using GH_IO.Serialization;

namespace WallSectionWidget
{
    public class GHIOParam<ScriptClass> : IGH_Goo
    {
        public ScriptClass ScriptClassVariable;

        public GHIOParam()
        {

        }

        public GHIOParam(ScriptClass variable)
        {
            ScriptClassVariable = variable;
        }

        public bool GetContent<TargetClass>(out TargetClass variable)
        {
            if (typeof(TargetClass) == typeof(ScriptClass))
            {
                variable = (TargetClass)((object)ScriptClassVariable);
                return !(variable == null);
            } else
            {
                variable = default;
                return false;
            }
        }

        public bool SetContent<TargetClass>(TargetClass variable)
        {
            if (typeof(TargetClass) == typeof(ScriptClass))
            {
                ScriptClassVariable = (ScriptClass)((object)variable);
                return !(ScriptClassVariable == null);
            }
            else
            {
                return false;
            }
        }

        public bool IsValid => true; // dummy

        public string IsValidWhyNot => "dummy"; // dummy

        public string TypeName => "WSWModel";

        public string TypeDescription => "Wall section widget model";

        public bool CastFrom(object source)
        {
            return !(source as GHIOParam<ScriptClass> is null);
        }

        public bool CastTo<T>(out T target)
        {
            if (typeof(T) == GetType())
            {
                target = (T)(object)this;
                return true;
            } else if (typeof(T) == typeof(string))
            {
                target = (T)(object)ToString();
                return true;
            } else
            {
                target = default;
                return false;
            }
        }

        public IGH_Goo Duplicate()
        {
            return this; // Performance check req.
        }

        public IGH_GooProxy EmitProxy()
        {
            return (IGH_GooProxy)this; // Performance check req.
        }

        public bool Read(GH_IReader reader)
        {
            return true; // Performance check req.
        }

        public object ScriptVariable()
        {
            return ScriptClassVariable;
        }

        public bool Write(GH_IWriter writer)
        {
            return true; // Performance check req.
        }
    }
}
