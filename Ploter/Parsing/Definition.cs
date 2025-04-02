using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Parsing
{
    public class Definition
    {
        public string Name { get; }
        public string[] Parameters { get; }
        public IExpression Expression { get; }
        protected readonly List<Definition> _defintions;

        public Definition(string name, string[] parameters, IExpression expression, List<Definition> definitions)
        {
            Name = name;
            Parameters = parameters;
            Expression = expression;
            _defintions = definitions;
        }

        virtual public int GetParamCount()
        {
            return Parameters.Length;
        }

        virtual public Complex Invoke(params Complex[] args)
        {
            if (args.Length !=  Parameters.Length)
            {
                throw new ArgumentException($"Expected {GetParamCount()} arguments, got {args.Length}");
            }

            return Expression.Resolve(Parameters.Select((param, i) => new Definition(param, [], new Literal(args[i]), _defintions)).Concat(_defintions).ToList());
        }
    }
}
