using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ploter.Parsing
{
    public abstract class Expression
    {
        abstract public Complex Resolve(List<Variable> variables, List<FunctionDef> functions);
        abstract public bool DependsOn(string name);
    }
}
