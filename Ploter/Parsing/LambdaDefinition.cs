using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Parsing
{
    public class LambdaDefinition : Definition
    {

        private readonly Func<Complex[], Complex> _func;
        private readonly int _arg_count;

        public LambdaDefinition(string name, int arg_count, Func<Complex[], Complex> func) : base(name, [], new Literal(0), [])
        {
            _func = func;
            _arg_count = arg_count;
        }

        public override int GetParamCount()
        {
            return _arg_count;
        }

        override public Complex Invoke(params Complex[] args)
        {
            if (args.Length != _arg_count)
            {
                throw new ArgumentException($"Expected {GetParamCount()} arguments, got {args.Length}");
            }
            return _func(args);
        }
    }
}
