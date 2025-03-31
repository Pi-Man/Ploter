using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ploter.Parsing
{
    public class FunctionDef : Definition
    {
        public string Name { get; set; }
        public string[] Parameters { get; set; }
        public Expression Expression { get; set; }

        public FunctionDef(string name, string[] parameters, Expression expression)
        {
            Name = name;
            Parameters = parameters;
            Expression = expression;
        }

        public Complex Invoke(params Complex[] args)
        {
            if (args.Length !=  Parameters.Length)
            {
                throw new ArgumentException($"Expected {Parameters.Length} arguments, got {args.Length}");
            }

            return Expression.Resolve(Parameters.Select((param, i) => new Variable(param, args[i])).ToList(), []);
        }
    }
}
