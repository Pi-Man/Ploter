using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static readonly Func<Complex, Complex, Complex> ADD = (c1, c2) => c1 + c2;
        public static readonly Func<Complex, Complex, Complex> SUB = (c1, c2) => c1 - c2;
        public static readonly Func<Complex, Complex, Complex> MUL = (c1, c2) => c1 * c2;
        public static readonly Func<Complex, Complex, Complex> DIV = (c1, c2) => c1 / c2;
        public static readonly Func<Complex, Complex, Complex> MOD = (c1, c2) => new Complex(c1.Real % c2.Real, c1.Imaginary % c2.Imaginary);
        public static readonly Func<Complex, Complex, Complex> POW = (c1, c2) => Patches.Complex.Pow(c1, c2, StandardDefinitions.POW_K.Value);

        public static readonly Dictionary<char, Func<Complex, Complex, Complex>> Map = new Dictionary<char, Func<Complex, Complex, Complex>>
        {
            ['+'] = ADD,
            ['-'] = SUB,
            ['*'] = MUL,
            ['/'] = DIV,
            ['%'] = MOD,
            ['^'] = POW,
        };

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

        public Call[] GetCalls(string name)
        {
            return [.. Left.GetCalls(name), .. Right.GetCalls(name)];
        }
    }
}
