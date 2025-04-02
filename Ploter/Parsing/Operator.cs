using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Parsing
{
    internal class Operator : IExpression
    {
        public IExpression Left { get; set; }
        public IExpression Right { get; set; }

        public Func<Complex, Complex, Complex> Op { get; set; }

        public Operator(IExpression left, IExpression right, Func<Complex, Complex, Complex> op)
        {
            Left = left;
            Right = right;
            Op = op;
        }

        public Complex Resolve(List<Definition> definitions)
        {
            return Op.Invoke(Left.Resolve(definitions), Right.Resolve(definitions));
        }

        public bool DependsOn(string name)
        {
            return Left.DependsOn(name) || Right.DependsOn(name);
        }
    }
}
