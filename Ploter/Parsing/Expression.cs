using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Parsing
{
    public interface IExpression
    {
        Complex Resolve(List<Definition> definitions);
        bool DependsOn(string name);
        Call[] GetCalls(string name);
    }
}
