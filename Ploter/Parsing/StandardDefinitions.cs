using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Parsing
{
    public static class StandardDefinitions
    {
        public static readonly ReferenceDefinition<int> POW_K = new ReferenceDefinition<int>("pow_k", 0);

        public static readonly LambdaDefinition PI = new LambdaDefinition("pi", 0, _ => Math.PI);
        public static readonly LambdaDefinition E = new LambdaDefinition("e", 0, _ => Math.E);
        public static readonly LambdaDefinition I = new LambdaDefinition("i", 0, _ => Complex.ImaginaryOne);
        public static readonly LambdaDefinition PHI = new LambdaDefinition("phi", 0, _ => 0.5 + 0.5 * Math.Sqrt(5));
        public static readonly LambdaDefinition PSI = new LambdaDefinition("psi", 0, _ => 0.5 - 0.5 * Math.Sqrt(5));


        public static readonly LambdaDefinition Real = new LambdaDefinition("real", 1, z => z[0].Real);
        public static readonly LambdaDefinition Imaginary = new LambdaDefinition("imaginary", 1, z => z[0].Imaginary);

        public static readonly LambdaDefinition Sin = new LambdaDefinition("sin", 1, z => Complex.Sin(z[0]));
        public static readonly LambdaDefinition Sinh = new LambdaDefinition("sinh", 1, z => Complex.Sinh(z[0]));
        public static readonly LambdaDefinition Asin = new LambdaDefinition("asin", 1, z => Complex.Asin(z[0]));
                               
        public static readonly LambdaDefinition Cos = new LambdaDefinition("cos", 1, z => Complex.Cos(z[0]));
        public static readonly LambdaDefinition Cosh = new LambdaDefinition("cosh", 1, z => Complex.Cosh(z[0]));
        public static readonly LambdaDefinition Acos = new LambdaDefinition("acos", 1, z => Complex.Acos(z[0]));
                               
        public static readonly LambdaDefinition Tan = new LambdaDefinition("tan", 1, z => Complex.Tan(z[0]));
        public static readonly LambdaDefinition Tanh = new LambdaDefinition("tanh", 1, z => Complex.Tanh(z[0]));
        public static readonly LambdaDefinition Atan = new LambdaDefinition("atan", 1, z => Complex.Atan(z[0]));
                               
        public static readonly LambdaDefinition Ln = new LambdaDefinition("ln", 1, z => Complex.Log(z[0]));
        public static readonly LambdaDefinition Log = new LambdaDefinition("log", 2, z => Complex.Log(z[0]) / Complex.Log(z[1]));
                               
        public static readonly LambdaDefinition Exp = new LambdaDefinition("exp", 1, z => Patches.Complex.Exp(z[0], POW_K.Value));
        public static readonly LambdaDefinition Sqrt = new LambdaDefinition("sqrt", 1, z => Patches.Complex.Sqrt(z[0], POW_K.Value));

        public static readonly List<Definition> definitions = [
            PI,
            E,
            I,
            PHI,
            PSI,
            Real,
            Imaginary,
            Sin,
            Sinh,
            Asin,
            Cos,
            Cosh,
            Acos,
            Tan,
            Tanh,
            Atan,
            Ln,
            Log,
            Exp,
            Sqrt,
        ];
    }
}
