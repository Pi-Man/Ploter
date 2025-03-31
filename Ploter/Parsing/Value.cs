using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ploter.Parsing
{
    internal class Value : Expression
    {
        public Complex Val { get; set; }

        public Value(Complex val)
        {
            Val = val;
        }

        public override Complex Resolve(List<Variable> variables, List<FunctionDef> functions)
        {
            return Val;
        }

        public override bool DependsOn(string name)
        {
            return false;
        }
    }
}
