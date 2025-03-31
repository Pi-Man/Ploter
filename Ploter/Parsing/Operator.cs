using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ploter.Parsing
{
    internal class Operator : Expression
    {
        public Expression Left { get; set; }
        public Expression Right { get; set; }

        public Func<Complex, Complex, Complex> Op { get; set; }

        public Operator(Expression left, Expression right, Func<Complex, Complex, Complex> op)
        {
            Left = left;
            Right = right;
            Op = op;
        }

        public override Complex Resolve(List<Variable> variables, List<FunctionDef> functions)
        {
            return Op.Invoke(Left.Resolve(variables, functions), Right.Resolve(variables, functions));
        }

        public override bool DependsOn(string name)
        {
            return Left.DependsOn(name) || Right.DependsOn(name);
        }
    }
}
