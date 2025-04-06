using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Parsing
{
    public class Literal : IExpression
    {
        public Complex Val { get; set; }

        public Literal(Complex val)
        {
            Val = val;
        }

        public Complex Resolve(List<Definition> definitions)
        {
            return Val;
        }

        public bool DependsOn(string name)
        {
            return false;
        }

        public Call[] GetCalls(string name)
        {
            return [];
        }
    }
}
