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
    public class ModelGHParam : IGH_Goo
    {
        public bool IsValid => throw new NotImplementedException();

        public string IsValidWhyNot => throw new NotImplementedException();

        public string TypeName => "WSWModel";

        public string TypeDescription => "Wall section widget model";

        public bool CastFrom(object source)
        {
            return !(source as ModelGHParam is null);
        }

        public bool CastTo<T>(out T target)
        {
            if (typeof(T) == GetType())
            {
                target = (T)(object)this;
                return true;
            } else if (typeof(T) == typeof(string))
            {
                target = (T)(object)this.ToString();
                return true;
            } else
            {
                target = default(T);
                return false;
            }
        }

        public IGH_Goo Duplicate()
        {
            throw new NotImplementedException();
        }

        public IGH_GooProxy EmitProxy()
        {
            return (IGH_GooProxy)this;
        }

        public bool Read(GH_IReader reader)
        {
            throw new NotImplementedException();
        }

        public object ScriptVariable()
        {
            throw new NotImplementedException();
        }

        public bool Write(GH_IWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
