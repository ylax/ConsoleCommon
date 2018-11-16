using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ConsoleCommon.Tests
{
    public class PropertyBuilding
    {
        public PropertyBuilding()
        {

        }
        public PropertyBuilding(PropertyBuilder propertyBuild, MethodBuilder getBuild, MethodBuilder setBuild)
        {
            propertyBuilder = propertyBuild;
            getBuilder = getBuild;
            setBuilder = setBuild;
        }
        public PropertyBuilder propertyBuilder { get; set; }
        public MethodBuilder getBuilder { get; set; }
        public MethodBuilder setBuilder { get; set;  }
        public FieldBuilder backFieldBuilder { get; set; }
    }
}
