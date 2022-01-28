using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LccApiNet.Exceptions
{
    class WrongSimpleParameterTypeExeption : Exception
    {
        public WrongSimpleParameterTypeExeption(string type) : base($"Parameter type must be primitive type but was {type}") { }
    }
}
